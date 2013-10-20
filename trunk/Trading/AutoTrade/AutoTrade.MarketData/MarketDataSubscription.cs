using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using AutoTrade.MarketData.Data;
using AutoTrade.MarketData.Properties;
using AutoTrade.MarketData.Publication;
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
        private readonly IMarketDataRepositoryFactory _repositoryFactory;

        /// <summary>
        /// The provider for market data
        /// </summary>
        private readonly IMarketDataProvider _marketDataProvider;

        /// <summary>
        /// The provider of the list of stocks for the subscription
        /// </summary>
        private readonly IStockListProvider _stockListProvider;

        /// <summary>
        /// The publisher for new quotes data
        /// </summary>
        private readonly IPublisher<NewQuotesData> _quotesPublisher;

        /// <summary>
        /// The data for the subscription
        /// </summary>
        private Subscription _subscriptionData;

        /// <summary>
        /// The lock for regulating access to subscriptionData data
        /// </summary>
        private readonly object _subscriptionDataLock = new object();

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
        /// <param name="repositoryFactory">The repository to store and retrieve market data</param>
        /// <param name="marketDataProvider">The provider for refreshing market data</param>
        /// <param name="stockListProvider">The provider for lists of stocks for which to retrieve data</param>
        /// <param name="quotesPublisher"></param>
        /// <param name="subscriptionData">The subscriptionData data for determining</param>
        public MarketDataSubscription(ILog logger,
            IMarketDataRepositoryFactory repositoryFactory,
            IMarketDataProvider marketDataProvider,
            IStockListProvider stockListProvider,
            IPublisher<NewQuotesData> quotesPublisher,
            Subscription subscriptionData)
        {
            // perform null checks
            if (logger == null)
                throw new ArgumentNullException("logger");
            if (repositoryFactory == null)
                throw new ArgumentNullException("repositoryFactory");
            if (marketDataProvider == null)
                throw new ArgumentNullException("marketDataProvider");
            if (subscriptionData == null)
                throw new ArgumentNullException("subscriptionData");

            // set dependencies
            _logger = logger;
            _repositoryFactory = repositoryFactory;
            _marketDataProvider = marketDataProvider;
            _stockListProvider = stockListProvider;
            _quotesPublisher = quotesPublisher;
            _subscriptionData = subscriptionData;

            // set up timer
            _timer = new Timer(obj => GetLatestQuotes());

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
        public void UpdateSubscriptionData()
        {
            // update the data
            UpdateSubscriptionData(GetLatestSubscriptionData());
        }

        /// <summary>
        /// Gets the latest subscription data
        /// </summary>
        /// <returns></returns>
        private Subscription GetLatestSubscriptionData()
        {
            // get the latest data from the database
            lock (_subscriptionDataLock)
                using (var repository = _repositoryFactory.CreateRepository())
                    return repository.GetSubscription(_subscriptionData.ID);
        }

        /// <summary>
        /// Updates the data for the subscription for a subscription data object
        /// </summary>
        public void UpdateSubscriptionData(Subscription subscriptionData)
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
        private void GetLatestQuotes()
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
                            using (var repository = _repositoryFactory.CreateRepository())
                            {
                                foreach (var quote in quotes)
                                {
                                    // set created date
                                    quote.SubscriptionID = _subscriptionData.ID;
                                    quote.Created = DateTime.Now;

                                    // add to repository
                                    repository.StockQuotes.Add(quote);
                                }

                                // save
                                repository.SaveChanges();
                            }

                            // if a publisher was provided, publish new quote data
                            if (_quotesPublisher != null)
                                _quotesPublisher.Publish(new NewQuotesData
                                    {
                                        SubscriptionId = _subscriptionData.ID.ToString(CultureInfo.InvariantCulture),
                                        Quotes = quotes.ToList()
                                    });
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
