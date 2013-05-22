using System.Collections.Generic;

namespace AutoTrade.MarketData
{
    public interface IMarketDataProvider
    {
        IEnumerable<StockQuote> GetQuotes(IEnumerable<Stock> stocks);
    }
}
