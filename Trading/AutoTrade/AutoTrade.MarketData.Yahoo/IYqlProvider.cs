using System.Collections.Generic;

namespace AutoTrade.MarketData.Yahoo
{
    public interface IYqlProvider
    {
        /// <summary>
        /// Gets the YQL used to selected data for multiple stock quotes
        /// </summary>
        /// <param name="tickers"></param>
        /// <returns></returns>
        string GetMultiStockQuoteSelect(IEnumerable<string> tickers);
    }
}
