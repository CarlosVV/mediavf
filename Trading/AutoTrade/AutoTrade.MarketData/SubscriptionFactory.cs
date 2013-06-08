using System;
using System.Collections.Generic;
using System.Linq;
using AutoTrade.MarketData.Entities;
using AutoTrade.MarketData.Exceptions;
using AutoTrade.MarketData.Properties;
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
        private readonly IMarketDataRepository _marketDataRepository;

        /// <summary>
        /// The list of registered market data providers
        /// </summary>
        private readonly IEnumerable<IMarketDataProvider> _marketDataProviders;

        #endregion

        #region Constructors

        public SubscriptionFactory(ILog logger, IMarketDataRepository marketDataRepository, IEnumerable<IMarketDataProvider> marketDataProviders)
        {
            _logger = logger;
            _marketDataRepository = marketDataRepository;
            _marketDataProviders = marketDataProviders;
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
            if (subscription == null)
                throw new ArgumentNullException("subscription");

            // ensure data provider is not null
            if (subscription.DataProvider == null)
                throw new SubscriptionCreationException(Resources.DataProviderNullException);

            // create the data provider
            var marketDataProvider =
                _marketDataProviders.FirstOrDefault(mdp => mdp.GetType().FullName == subscription.DataProvider.Type);
            if (marketDataProvider == null)
                throw new SubscriptionCreationException(Resources.DataProviderTypeNotRegisteredExceptionFormat,
                    subscription.DataProvider.Type);

            return new MarketDataSubscription(_logger, _marketDataRepository, marketDataProvider, subscription);
        }

        #endregion
    }
}