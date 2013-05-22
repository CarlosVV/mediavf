using System.Collections.Generic;
using System.Linq;
using AutoTrade.MarketData.Yahoo.Exceptions;

namespace AutoTrade.MarketData.Yahoo
{
    public class YahooMarketDataProvider : IMarketDataProvider
    {
        #region Fields

        /// <summary>
        /// The provider for getting YQL queries
        /// </summary>
        private readonly IYqlProvider _yqlProvider;

        /// <summary>
        /// The executor of YQL queries
        /// </summary>
        private readonly IYqlExecutor _yqlExecutor;

        /// <summary>
        /// The translator for interpreting YQL responses
        /// </summary>
        private readonly IYqlResultTranslator _yqlResultTranslator;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="YahooMarketDataProvider"/>
        /// </summary>
        /// <param name="yqlProvider"></param>
        /// <param name="yqlExecutor"></param>
        /// <param name="yqlResultTranslator"></param>
        public YahooMarketDataProvider(IYqlProvider yqlProvider, IYqlExecutor yqlExecutor, IYqlResultTranslator yqlResultTranslator)
        {
            _yqlProvider = yqlProvider;
            _yqlExecutor = yqlExecutor;
            _yqlResultTranslator = yqlResultTranslator;
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

            // get the yql to execute
            string yql = _yqlProvider.GetMultiStockQuoteSelect(stockList.Select(s => s.Symbol));
            if (string.IsNullOrWhiteSpace(yql))
                throw new EmptyYqlQueryException();

            // execute yql
            string yqlResponse = _yqlExecutor.ExecuteYqlQuery(yql);
            if (string.IsNullOrWhiteSpace(yqlResponse))
                throw new EmptyYqlResponseException();

            // interpret response and return
            return _yqlResultTranslator.TranslateResultsToQuotes(yqlResponse);
        }

        #endregion
    }
}
