﻿using System;
using System.Threading;
using AutoTrade.MarketData.Data;
using AutoTrade.MarketData.Properties;
using log4net;

namespace AutoTrade.MarketData
{
    public class MarketDataSubscription : IMarketDataSubscription
    {
        #region Fields

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILog _logger;

        /// <summary>
        /// The repository for storing and retrieving market data
        /// </summary>
        private readonly IMarketDataRepository _marketDataRepository;

        /// <summary>
        /// The provider for market data
        /// </summary>
        private readonly IMarketDataProvider _marketDataProvider;

        private readonly IStockListProvider _stockListProvider;

        /// <summary>
        /// The lock for regulating access to subscriptionData data
        /// </summary>
        private readonly object _subscriptionDataLock = new object();

        /// <summary>
        /// The timer to perform periodic updates of data
        /// </summary>
        private readonly Timer _timer;

        /// <summary>
        /// The data for the subscription
        /// </summary>
        private Subscription _subscriptionData;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="MarketDataSubscription"/>
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="marketDataRepository">The repository to store and retrieve market data</param>
        /// <param name="marketDataProvider">The provider for refreshing market data</param>
        /// <param name="stockListProvider">The provider for lists of stocks for which to retrieve data</param>
        /// <param name="subscriptionData">The subscriptionData data for determining</param>
        public MarketDataSubscription(ILog logger,
            IMarketDataRepository marketDataRepository,
            IMarketDataProvider marketDataProvider,
            IStockListProvider stockListProvider,
            Subscription subscriptionData)
        {
            // perform null checks
            if (logger == null)
                throw new ArgumentNullException("logger");
            if (marketDataRepository == null)
                throw new ArgumentNullException("marketDataRepository");
            if (marketDataProvider == null)
                throw new ArgumentNullException("marketDataProvider");
            if (subscriptionData == null)
                throw new ArgumentNullException("subscriptionData");

            // set dependencies
            _logger = logger;
            _marketDataRepository = marketDataRepository;
            _marketDataProvider = marketDataProvider;
            _stockListProvider = stockListProvider;
            _subscriptionData = subscriptionData;

            // set up timer
            _timer = new Timer(obj => GetLatestData());

            // status initialized to idle
            Status = SubscriptionStatus.Idle;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current status of the subscriptionData
        /// </summary>
        public SubscriptionStatus Status { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Updates data for the subscription by retrieving the latest data
        /// </summary>
        public void UpdateData()
        {
            // get the latest data from the database
            Subscription subscriptionData;
            lock (_subscriptionDataLock)
                subscriptionData = _marketDataRepository.GetSubscription(_subscriptionData.ID);

            // update the data
            UpdateData(subscriptionData);
        }

        /// <summary>
        /// Updates the data for the subscription for a subscription data object
        /// </summary>
        public void UpdateData(Subscription subscriptionData)
        {
            lock (_subscriptionDataLock)
            {
                // stop the timer
                StopTimer();
                
                // set new subscription data
                _subscriptionData = subscriptionData;

                // start timer with new data
                StartTimer();
            }
        }

        /// <summary>
        /// Starts regular updates of data
        /// </summary>
        public void Start()
        {
            lock (_subscriptionDataLock)
            {
                // set status
                Status = SubscriptionStatus.Running;

                // stop the timer
                StartTimer();
            }
        }

        /// <summary>
        /// Stops regular updates of data
        /// </summary>
        public void Stop()
        {
            lock (_subscriptionDataLock)
            {
                // set status
                Status = SubscriptionStatus.Idle;

                // stop the timer
                StopTimer();
            }
        }

        /// <summary>
        /// Starts running the timer
        /// </summary>
        private void StartTimer()
        {
            // start running timer
            _timer.Change(_subscriptionData.UpdateInterval.TimeOfDay, _subscriptionData.UpdateInterval.TimeOfDay);
        }

        /// <summary>
        /// Stops running the timer
        /// </summary>
        private void StopTimer()
        {
            // stop running timer
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Refreshes data using the market data provider
        /// </summary>
        private void GetLatestData()
        {
            lock (_subscriptionDataLock)
            {
                // stop timer while data is retrieved
                Stop();

                if (_subscriptionData.IsActiveForCurrentTimeOfDay)
                {
                    try
                    {
                        // get the list of stocks for which to get quotes
                        var stocks = _stockListProvider.GetStocks(_subscriptionData);

                        // get quotes from provider
                        var quotes = _marketDataProvider.GetQuotes(stocks);

                        if (quotes != null)
                        {
                            foreach (var quote in quotes)
                            {
                                // set created date
                                quote.SubscriptionID = _subscriptionData.ID;
                                quote.Created = DateTime.Now;

                                // add to repository
                                _marketDataRepository.StockQuotes.Add(quote);
                            }

                            // save
                            _marketDataRepository.SaveChanges();
                        }
                    }
                    catch (Exception exception)
                    {
                        // log the exception
                        _logger.Error(Resources.SubscriptionUpdateException, exception);
                    }
                }

                // restart the timer now 
                Start();
            }
        }

        #endregion
    }
}