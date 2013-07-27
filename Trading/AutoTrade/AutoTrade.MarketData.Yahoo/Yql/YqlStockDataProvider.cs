using System.Collections.Generic;
using System.Linq;
using AutoTrade.Core.StockData;
using AutoTrade.Core.Web;
using AutoTrade.MarketData.Data;
using AutoTrade.MarketData.Yahoo.Exceptions;

namespace AutoTrade.MarketData.Yahoo.Yql
{
    public class YqlStockDataProvider : IStockDataProvider
    {
        #region Fields

        /// <summary>
        /// The YQL url provider
        /// </summary>
        private readonly IYqlUrlProvider _urlProvider;

        /// <summary>
        /// The web request executor
        /// </summary>
        private readonly IWebRequestExecutor _webRequestExecutor;

        /// <summary>
        /// The YQL result translator
        /// </summary>
        private readonly IYqlResultTranslator _quotesResultTranslator;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="YqlStockDataProvider"/>
        /// </summary>
        /// <param name="urlProvider"></param>
        /// <param name="webRequestExecutor"></param>
        /// <param name="quotesResultTranslator"></param>
        public YqlStockDataProvider(IYqlUrlProvider urlProvider, IWebRequestExecutor webRequestExecutor, IYqlResultTranslator quotesResultTranslator)
        {
            _urlProvider = urlProvider;
            _webRequestExecutor = webRequestExecutor;
            _quotesResultTranslator = quotesResultTranslator;
        }

        #endregion

        #region Methods

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
            return _quotesResultTranslator.TranslateResultsToStockData(results);
        }

        #endregion
    }
}
