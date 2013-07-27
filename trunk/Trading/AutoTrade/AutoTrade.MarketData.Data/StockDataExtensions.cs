using AutoTrade.Core.StockData;

namespace AutoTrade.MarketData.Data
{
    public static class StockDataExtensions
    {
        /// <summary>
        /// Converts a <see cref="StockData"/> object to a <see cref="Stock"/> entity
        /// </summary>
        /// <param name="stockData"></param>
        /// <returns></returns>
        public static Stock ToStockEntity(this StockData stockData)
        {
            return new Stock
                {
                    Symbol = stockData.Symbol,
                    CompanyName = stockData.CompanyName,
                    IndustryName = stockData.Industry,
                    SectorName = stockData.Sector
                };
        }
    }
}
