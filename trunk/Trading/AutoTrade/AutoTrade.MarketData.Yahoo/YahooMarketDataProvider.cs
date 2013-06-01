using System.Collections.Generic;
using System.Linq;
using AutoTrade.Core.Web;
using AutoTrade.MarketData.Yahoo.Exceptions;

namespace AutoTrade.MarketData.Yahoo
{
    public class YahooMarketDataProvider : IMarketDataProvider
    {
        #region Fields

        /// <summary>
        /// The urlProvider of YQL queries
        /// </summary>
        private readonly IUrlProvider _urlProvider;

        /// <summary>
        /// The urlProvider of web requests
        /// </summary>
        private readonly IWebRequestExecutor _webRequestExecutor;

        /// <summary>
        /// The translator for interpreting YQL responses
        /// </summary>
        private readonly IResultTranslator _resultTranslator;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="YahooMarketDataProvider"/>
        /// </summary>
        /// <param name="urlProvider"></param>
        /// <param name="webRequestExecutor"></param>
        /// <param name="resultTranslator"></param>
        public YahooMarketDataProvider(IUrlProvider urlProvider,
            IWebRequestExecutor webRequestExecutor,
            IResultTranslator resultTranslator)
        {
            _urlProvider = urlProvider;
            _webRequestExecutor = webRequestExecutor;
            _resultTranslator = resultTranslator;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets quotes by executing queries against Yahoo Finance using YQL
        /// </summary>
        /// <param name="stocks"></param>
        /// <returns></returns>
        public IEnumerable<StockQuote> GetQuotes(IEnumerable<Stock> stocks)
        {
            // if no stocks provided, return empty list
            if (stocks == null)
                return new List<StockQuote>();

            // enumerate and check count; if no stocks provided, return empty list
            var stockList = stocks.ToList();
            if (stockList.Count == 0)
                return new List<StockQuote>();

            // get url for query
            string queryUrl = _urlProvider.GetUrl(stockList.Select(s => s.Symbol));
            if (string.IsNullOrWhiteSpace(queryUrl))
                throw new QueryUrlNotProvidedException();

            // get results of query
            string results = _webRequestExecutor.ExecuteRequest(queryUrl);
            if (string.IsNullOrWhiteSpace(results))
                throw new QueryResultsAreEmptyException();

            // interpret response and return
            return _resultTranslator.TranslateResultsToQuotes(results);
        }

        #endregion
    }
}
