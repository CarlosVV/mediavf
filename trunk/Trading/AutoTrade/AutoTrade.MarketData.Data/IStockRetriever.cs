using System.Collections.Generic;

namespace AutoTrade.MarketData.Data
{
    public interface IStockRetriever
    {
        /// <summary>
        /// Gets stocks by their symbols
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        IEnumerable<Stock> GetStocks(IEnumerable<string> symbols);
    }
}
