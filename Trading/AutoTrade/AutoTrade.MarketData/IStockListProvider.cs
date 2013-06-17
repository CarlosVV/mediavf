using System.Collections.Generic;
using AutoTrade.MarketData.Data;

namespace AutoTrade.MarketData
{
    public interface IStockListProvider
    {
        /// <summary>
        /// Gets stocks to retrieve quotes for
        /// </summary>
        /// <returns></returns>
        IEnumerable<Stock> GetStocks(Subscription subscription);
    }
}
