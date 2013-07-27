using System.Collections.Generic;

namespace AutoTrade.Core.StockData
{
    public interface IStockDataProvider
    {
        /// <summary>
        /// Gets stock data for a collection of symbols
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        IEnumerable<StockData> GetStockData(IEnumerable<string> symbols);
    }
}
