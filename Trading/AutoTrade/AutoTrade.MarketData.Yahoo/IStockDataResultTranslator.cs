using System.Collections.Generic;
using AutoTrade.Core.StockData;

namespace AutoTrade.MarketData.Yahoo
{
    public interface IStockDataResultTranslator
    {
        /// <summary>
        /// Translates the results of a YQL query to StockData
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        IEnumerable<StockData> TranslateResultsToStockData(string response);
    }
}