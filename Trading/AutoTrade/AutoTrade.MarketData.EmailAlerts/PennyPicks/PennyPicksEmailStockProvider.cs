using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoTrade.MarketData.Data;

namespace AutoTrade.MarketData.EmailAlerts.PennyPicks
{
    class PennyPicksEmailStockProvider : IStockListProvider
    {
        #region Fields

        /// <summary>
        /// The email feed
        /// </summary>
        private readonly IEmailFeed _feed;

        /// <summary>
        /// The parser for getting stock symbols from emails
        /// </summary>
        private readonly IEmailStockParser _stockParser;

        /// <summary>
        /// The stock retriever
        /// </summary>
        private readonly IStockRetriever _stockRetriever;

        /// <summary>
        /// The collection of stocks that have been found in the feed
        /// </summary>
        private readonly List<Stock> _stocks = new List<Stock>();

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="PennyPicksEmailStockProvider"/>
        /// </summary>
        /// <param name="emailAlertsConfiguration"></param>
        /// <param name="emailFeedFactory"></param>
        /// <param name="stockParser"></param>
        /// <param name="stockRetriever"></param>
        public PennyPicksEmailStockProvider(IEmailAlertsConfiguration emailAlertsConfiguration,
            IEmailFeedFactory emailFeedFactory,
            IEmailStockParser stockParser,
            IStockRetriever stockRetriever)
        {
            _feed = emailFeedFactory.CreateFeed(emailAlertsConfiguration.PennyPicksFeedName);
            _feed.NewEmailsFound += FeedOnNewEmailsFound;
            _feed.Start();

            _stockParser = stockParser;
            _stockRetriever = stockRetriever;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles new emails found in the feed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="emailEventArgs"></param>
        private void FeedOnNewEmailsFound(object sender, EmailEventArgs emailEventArgs)
        {
            // check that emails were found
            if (emailEventArgs.Emails == null) return;

            // get symbols from emails
            var symbols = emailEventArgs.Emails.Select(e => _stockParser.ParseStockSymbol(e))
                                               .Where(s => !string.IsNullOrWhiteSpace(s));

            // get stocks from symbols
            var stocks = _stockRetriever.GetStocks(symbols);

            // add stocks to collection
            _stocks.AddRange(stocks);
        }

        /// <summary>
        /// Gets the collection of stocks that have been parsed from the email feed
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        public IEnumerable<Stock> GetStocks(Subscription subscription)
        {
            return new ReadOnlyCollection<Stock>(_stocks);
        }

        #endregion
    }
}
