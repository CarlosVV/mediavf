
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using AutoTrade.MarketData.Data;

namespace AutoTrade.MarketData.DataService
{
    public class MarketDataService : DataService<IMarketDataRepository>
    {
        #region Static

        /// <summary>
        /// Initializes the <see cref="MarketDataService"/> for use
        /// </summary>
        /// <param name="config"></param>
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;

            config.SetEntitySetAccessRule("Subscriptions", EntitySetRights.All);
            config.SetEntitySetAccessRule("DataProviders", EntitySetRights.All);
            config.SetEntitySetAccessRule("StockListProviders", EntitySetRights.All);

            config.RegisterKnownType(typeof(SubscriptionQuoteSummary));
            config.SetServiceOperationAccessRule("GetQuoteSummaries", ServiceOperationRights.AllRead);
        }

        #endregion

        #region Fields

        /// <summary>
        /// The factory for creating repositories
        /// </summary>
        private readonly IMarketDataRepositoryFactory _repositoryFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="MarketDataService"/>
        /// </summary>
        /// <param name="repositoryFactory"></param>
        public MarketDataService(IMarketDataRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a <see cref="IMarketDataRepository"/> as the data source for the service
        /// </summary>
        /// <returns></returns>
        protected override IMarketDataRepository CreateDataSource()
        {
            return _repositoryFactory.GetRepository();
        }

        /// <summary>
        /// Gets summaries of quotes for a subscription
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        [WebGet]
        public IQueryable<SubscriptionQuoteSummary> GetQuoteSummaries(int subscriptionId)
        {
            // get most recent quotes
            var mostRecentQuotes =
                CurrentDataSource.StockQuotes
                                 .GroupBy(quote => new { SubscriptionId = quote.SubscriptionID, quote.Symbol })
                                 .ToDictionary(g => g.Key, g => g.OrderByDescending(q => q.QuoteDateTime).First());

            // create summaries by grouping
            return
                CurrentDataSource.StockQuotes
                                 .OrderByDescending(q => q.QuoteDateTime)
                                 .GroupBy(quote => new { SubscriptionId = quote.SubscriptionID, quote.Symbol })
                                 .Select(grouping =>
                                     new
                                     {
                                         grouping.Key,
                                         grouping.Key.SubscriptionId,
                                         grouping.Key.Symbol,
                                         Count = grouping.Count(),
                                         FirstQuoted = grouping.Min(q => q.QuoteDateTime),
                                         LastQuoted = grouping.Max(q => q.QuoteDateTime)
                                     })
                                 .ToList()
                                 .Select(x =>
                                     new SubscriptionQuoteSummary
                                         {
                                             SubscriptionId = x.SubscriptionId,
                                             Symbol = x.Symbol,
                                             QuoteCount = x.Count,
                                             FirstQuoted = x.FirstQuoted,
                                             LastQuoted = x.LastQuoted,
                                             LatestPrice = mostRecentQuotes[x.Key].LastTradePrice,
                                             LatestAsk = mostRecentQuotes[x.Key].AskPrice,
                                             LatestBid = mostRecentQuotes[x.Key].BidPrice,
                                             LatestChange = mostRecentQuotes[x.Key].Change,
                                             LatestOpen = mostRecentQuotes[x.Key].OpenPrice
                                         }).AsQueryable();
        }

        #endregion
    }
}
