using System.Collections.Generic;
using System.Linq;
using AutoTrade.Core.StockData;
using AutoTrade.Core.Web;
using AutoTrade.MarketData.Data;
using AutoTrade.MarketData.Yahoo.Exceptions;

namespace AutoTrade.MarketData.Yahoo
{
    public abstract class YahooMarketDataProviderBase : IYahooMarketDataProvider, IStockDataProvider
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
        /// The translator for interpreting responses containing quotes
        /// </summary>
        private readonly IQuotesResultTranslator _quotesResultTranslator;

        /// <summary>
        /// The translator for interpreting responses containing stock data
        /// </summary>
        private readonly IStockDataResultTranslator _stockDataResultTranslator;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="YahooMarketDataProvider"/>
        /// </summary>
        /// <param name="urlProvider"></param>
        /// <param name="webRequestExecutor"></param>
        /// <param name="quotesResultTranslator"></param>
        /// <param name="stockDataResultTranslator"></param>
        protected YahooMarketDataProviderBase(IUrlProvider urlProvider,
            IWebRequestExecutor webRequestExecutor,
            IQuotesResultTranslator quotesResultTranslator,
            IStockDataResultTranslator stockDataResultTranslator)
        {
            _urlProvider = urlProvider;
            _webRequestExecutor = webRequestExecutor;
            _quotesResultTranslator = quotesResultTranslator;
            _stockDataResultTranslator = stockDataResultTranslator;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the precedence of the market provider
        /// </summary>
        public abstract int Precedence { get; }

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
            if (stocks == null) return new List<StockQuote>();

            // enumerate and check count; if no stocks provided, return empty list
            var stockList = stocks.ToList();
            if (stockList.Count == 0) return new List<StockQuote>();

            // get url for query
            string queryUrl = _urlProvider.GetQuotesUrl(stockList.Select(s => s.Symbol));
            if (string.IsNullOrWhiteSpace(queryUrl)) throw new QueryUrlNotProvidedException();

            // get results of query
            string results = _webRequestExecutor.ExecuteRequest(queryUrl);
            if (string.IsNullOrWhiteSpace(results)) throw new QueryResultsAreEmptyException();

            // interpret response and return
            return _quotesResultTranslator.TranslateResultsToQuotes(results);
        }

        /// <summary>
        /// Gets stock data using a Yql query
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        public IEnumerable<StockData> GetStockData(IEnumerable<string> symbols)
        {
            // if no stocks provided, return empty list
            if (symbols == null) return new List<StockData>();

            // enumerate and check count; if no stocks provided, return empty list
            var symbolList = symbols as IList<string> ?? symbols.ToList();
            if (symbolList.Count == 0) return new List<StockData>();

            // get url for query
            var queryUrl = _urlProvider.GetStockUrl(symbolList);
            if (string.IsNullOrWhiteSpace(queryUrl)) throw new QueryUrlNotProvidedException();

            // get results of query
            var results = _webRequestExecutor.ExecuteRequest(queryUrl);
            if (string.IsNullOrWhiteSpace(results)) throw new QueryResultsAreEmptyException();

            // interpret response and return
            return _stockDataResultTranslator.TranslateResultsToStockData(results);
        }

        #endregion
    }
}
