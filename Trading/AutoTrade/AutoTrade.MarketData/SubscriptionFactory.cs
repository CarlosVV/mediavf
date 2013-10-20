using System;
using System.Collections.Generic;
using System.Linq;
using AutoTrade.Core;
using AutoTrade.MarketData.Data;
using AutoTrade.MarketData.Exceptions;
using AutoTrade.MarketData.Properties;
using AutoTrade.MarketData.Publication;
using log4net;

namespace AutoTrade.MarketData
{
    class SubscriptionFactory : ISubscriptionFactory
    {
        #region Fields

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILog _logger;

        /// <summary>
        /// The repository for market data
        /// </summary>
        private readonly IMarketDataRepositoryFactory _repositoryFactory;

        /// <summary>
        /// The list of registered market data providers
        /// </summary>
        private readonly IEnumerable<IMarketDataProvider> _marketDataProviders;

        /// <summary>
        /// The list of registered stock list providers
        /// </summary>
        private readonly IEnumerable<IStockListProvider> _stockListProviders;

        /// <summary>
        /// The publisher factory
        /// </summary>
        private readonly IPublisherFactory _publisherFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="SubscriptionFactory"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repositoryFactory"></param>
        /// <param name="marketDataProviders"></param>
        /// <param name="stockListProviders"></param>
        /// <param name="publisherFactory"></param>
        public SubscriptionFactory(ILog logger,
            IMarketDataRepositoryFactory repositoryFactory,
            IEnumerable<IMarketDataProvider> marketDataProviders,
            IEnumerable<IStockListProvider> stockListProviders,
            IPublisherFactory publisherFactory)
        {
            _logger = logger;
            _repositoryFactory = repositoryFactory;
            _marketDataProviders = marketDataProviders;
            _stockListProviders = stockListProviders;
            _publisherFactory = publisherFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a market data subscription
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        public IMarketDataSubscription CreateSubscription(Subscription subscription)
        {
            // ensure subscription is not null
            if (subscription == null) throw new ArgumentNullException("subscription");

            return new MarketDataSubscription(_logger,
                _repositoryFactory,
                GetDataProvider(subscription),
                GetStockListProvider(subscription),
                _publisherFactory.CreatePublisher<NewQuotesData>(),
                subscription);
        }

        /// <summary>
        /// Gets the stock list provider for the subscription
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        private IStockListProvider GetStockListProvider(Subscription subscription)
        {
            // ensure data provider is not null
            if (subscription.StockListProvider == null)
                throw new SubscriptionCreationException(Resources.StockListProviderNullException);

            // create the data provider
            var stockListProvider =
                _stockListProviders.FirstOrDefault(slp => slp.GetType() == subscription.StockListProvider.Type.ParseType());
            if (stockListProvider == null)
                throw new SubscriptionCreationException(Resources.StockListProviderTypeNotRegisteredExceptionMessage,
                    subscription.StockListProvider.Type);

            return stockListProvider;
        }

        /// <summary>
        /// Gets the market data provider for the subscription
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        private IMarketDataProvider GetDataProvider(Subscription subscription)
        {
            // ensure data provider is not null
            if (subscription.DataProvider == null)
                throw new SubscriptionCreationException(Resources.DataProviderNullException);

            // create the data provider
            var marketDataProvider =
                _marketDataProviders.FirstOrDefault(mdp => mdp.GetType() == subscription.DataProvider.Type.ParseType());
            if (marketDataProvider == null)
                throw new SubscriptionCreationException(Resources.DataProviderTypeNotRegisteredExceptionFormat,
                    subscription.DataProvider.Type);

            return marketDataProvider;
        }

        #endregion
    }
}