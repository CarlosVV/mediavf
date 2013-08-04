using System;

namespace AutoTrade.Core.StockData
{
    public interface IStockDataProviderFactory
    {
        /// <summary>
        /// Gets the stock data provider
        /// </summary>
        /// <returns></returns>
        T GetStockDataProvider<T>() where T : IStockDataProvider;

        /// <summary>
        /// Gets the stock data provider
        /// </summary>
        /// <returns></returns>
        IStockDataProvider GetStockDataProvider(Type type);

        /// <summary>
        /// Gets the stock data provider
        /// </summary>
        /// <returns></returns>
        IStockDataProvider GetStockDataProvider(string name);
    }
}
