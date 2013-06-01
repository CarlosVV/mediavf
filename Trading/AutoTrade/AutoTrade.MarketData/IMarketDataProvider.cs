using System.Collections.Generic;

namespace AutoTrade.MarketData
{
    public interface IMarketDataProvider
    {
        /// <summary>
        /// Gets quotes for a set of stocks
        /// </summary>
        /// <param name="stocks"></param>
        /// <returns></returns>
        IEnumerable<StockQuote> GetQuotes(IEnumerable<Stock> stocks);
    }
}
