using System;
using System.Collections.Generic;
using System.Threading;
using AutoTrade.Core;
using AutoTrade.MarketData.Properties;
using log4net.Core;

namespace AutoTrade.MarketData
{
    public class MarketDataSubscription
    {
        #region Fields

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// The repository for storing and retrieving market data
        /// </summary>
        private readonly IMarketDataRepository _marketDataRepository;

        /// <summary>
        /// The provider for market data
        /// </summary>
        private readonly IMarketDataProvider _marketDataProvider;

        /// <summary>
        /// The subscription containing data for which to 
        /// </summary>
        private readonly Subscription _subscription;

        /// <summary>
        /// The timer to perform periodic updates of data
        /// </summary>
        private readonly Timer _timer;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="MarketDataSubscription"/>
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="marketDataRepository">The repository to store and retrieve market data</param>
        /// <param name="marketDataProvider">The provider for refreshing market data</param>
        /// <param name="subscription">The subscription data for determining</param>
        public MarketDataSubscription(ILogger logger,
            IMarketDataRepository marketDataRepository,
            IMarketDataProvider marketDataProvider,
            Subscription subscription)
        {
            _logger = logger;
            _marketDataRepository = marketDataRepository;
            _marketDataProvider = marketDataProvider;
            _subscription = subscription;

            _timer = new Timer(obj => GetLatestData());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts regular updates of data
        /// </summary>
        public void Start()
        {
            _timer.Change(_subscription.UpdateInterval, _subscription.UpdateInterval);
        }

        /// <summary>
        /// Stops regular updates of data
        /// </summary>
        public void Stop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Refreshes data using the market data provider
        /// </summary>
        private void GetLatestData()
        {
            // stop timer while data is retrieved
            Stop();

            try
            {
                // get quotes from provider
                IEnumerable<StockQuote> quotes = _marketDataProvider.GetQuotes(_subscription.Stocks);

                // add quotes to repository
                if (quotes != null)
                {
                    foreach (StockQuote quote in quotes)
                        _marketDataRepository.StockQuotes.Add(quote);

                    _marketDataRepository.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                // log the exception
                _logger.LogException(Resources.SubscriptionUpdateFailure, exception);
            }

            // restart the timer now 
            Start();
        }

        #endregion
    }
}
