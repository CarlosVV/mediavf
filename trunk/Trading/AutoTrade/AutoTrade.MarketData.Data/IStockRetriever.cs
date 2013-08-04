using System.Collections.Generic;
using AutoTrade.Core.StockData;

namespace AutoTrade.MarketData.Data
{
    public interface IStockRetriever
    {
        /// <summary>
        /// Gets stocks by their symbols
        /// </summary>
        /// <param name="stockDataProvider"></param>
        /// <param name="symbols"></param>
        /// <returns></returns>
        IEnumerable<Stock> GetStocks(IStockDataProvider stockDataProvider, IEnumerable<string> symbols);
    }
}
