using System.Collections.Generic;

namespace AutoTrade.MarketData.Yahoo.Yql
{
    public interface IYqlQueryProvider
    {
        /// <summary>
        /// Gets the select for getting stock data
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        string GetStockSelect(IEnumerable<string> symbols);

        /// <summary>
        /// Gets the YQL used to selected data for multiple stock quotes
        /// </summary>
        /// <param name="tickers"></param>
        /// <returns></returns>
        string GetMultiStockQuoteSelect(IEnumerable<string> tickers);
    }
}
