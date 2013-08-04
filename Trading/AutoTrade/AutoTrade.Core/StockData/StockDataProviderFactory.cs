using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoTrade.Core.StockData
{
    public class StockDataProviderFactory : IStockDataProviderFactory
    {
        #region Fields

        /// <summary>
        /// The collection of all registered stock data providers
        /// </summary>
        private readonly IEnumerable<IStockDataProvider> _stockDataProviders;

        /// <summary>
        /// Dictionary mapping providers by name
        /// </summary>
        private readonly Dictionary<string, IStockDataProvider> _providersByName; 

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stockDataProviders"></param>
        public StockDataProviderFactory(IEnumerable<IStockDataProvider> stockDataProviders)
        {
            if (stockDataProviders == null) throw new ArgumentNullException("stockDataProviders");

            // set list of providers
            _stockDataProviders = stockDataProviders;

            // map providers by name
            _providersByName =
                _stockDataProviders.Select(provider => Tuple.Create(NameAttribute.Get(provider), provider))
                                   .Where(x => x.Item1 != null)
                                   .ToDictionary(x => x.Item1.Name, x => x.Item2);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the stock data provider
        /// </summary>
        /// <returns></returns>
        public T GetStockDataProvider<T>() where T : IStockDataProvider
        {
            return (T)GetStockDataProvider(typeof(T));
        }

        /// <summary>
        /// Gets the stock data provider
        /// </summary>
        /// <returns></returns>
        public IStockDataProvider GetStockDataProvider(Type type)
        {
            // get by type
            var stockDataProvider = _stockDataProviders.FirstOrDefault(sdp => sdp.GetType() == type);

            // if not found, throw an exception
            if (stockDataProvider == null) throw new StockDataProviderNotFoundException(type);

            return stockDataProvider;
        }

        /// <summary>
        /// Gets the stock data provider
        /// </summary>
        /// <returns></returns>
        public IStockDataProvider GetStockDataProvider(string name)
        {
            // ensure name is not null
            if (name == null) throw new ArgumentNullException("name");

            // if a provider is mapped to the given name, return it
            if (_providersByName.ContainsKey(name)) return _providersByName[name];
            
            // provider not found, throw an exception
            throw new StockDataProviderNotFoundException(name);
        }

        #endregion
    }
}
