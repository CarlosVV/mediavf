using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AutoTrade.Core;
using AutoTrade.MarketData.Data;
using AutoTrade.MarketData.Exceptions;
using AutoTrade.MarketData.Properties;
using log4net;

namespace AutoTrade.MarketData
{
    public class SubscriptionManager : ISubscriptionManager
    {
        #region Fields

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILog _logger;

        /// <summary>
        /// The repository
        /// </summary>
        private readonly IMarketDataRepository _repository;

        /// <summary>
        /// The repository
        /// </summary>
        private readonly ISubscriptionFactory _subscriptionFactory;

        /// <summary>
        /// The subscriptions that have already been loaded
        /// </summary>
        private readonly Dictionary<int, IMarketDataSubscription> _subscriptions = new Dictionary<int, IMarketDataSubscription>();

        /// <summary>
        /// The timer for updating subscription data at regular intervals
        /// </summary>
        private readonly Timer _dataUpdateTimer;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="SubscriptionManager"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repository"></param>
        /// <param name="subscriptionFactory"></param>
        public SubscriptionManager(ILog logger, IMarketDataRepository repository, ISubscriptionFactory subscriptionFactory)
        {
            _logger = logger;
            _repository = repository;
            _subscriptionFactory = subscriptionFactory;

            // set up timer - wait five minutes, then update data every minute
            _dataUpdateTimer = new Timer(UpdateSubscriptionData);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates subscription data at regular intervals
        /// </summary>
        /// <param name="state"></param>
        private void UpdateSubscriptionData(object state)
        {
            _subscriptions.Values.AsParallel().ForAll(s => s.UpdateData());
        }

        /// <summary>
        /// Starts all subscriptions in the manager
        /// </summary>
        public void StartAllSubscriptions()
        {
            // get list of active subscriptions
            var activeSubscriptions = _repository.Subscriptions.Where(s => s.Active).ToDictionary(s => s.ID, s => s);

            if (activeSubscriptions.Count > 0)
            {
                // add any new subscriptions to the list
                AddNewSubscriptions(activeSubscriptions.Values);

                // start all subscriptions that are not already running
                _subscriptions.Where(kvp => activeSubscriptions.ContainsKey(kvp.Key) &&
                                            kvp.Value.Status != SubscriptionStatus.Running)
                              .AsParallel()
                              .ForAll(kvp =>
                              {
                                  // update data for the subscription
                                  kvp.Value.UpdateData(activeSubscriptions[kvp.Key]);

                                  // start running the subscription
                                  kvp.Value.Start();
                              });

                // start updating data for subscriptions regularly
                _dataUpdateTimer.Change(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(1));
            }
            else
                _logger.WarnFormat(Resources.NoSubscriptionsFoundWarning);
        }

        /// <summary>
        /// Starts any new subscriptions
        /// </summary>
        /// <param name="activeSubscriptions"></param>
        private void AddNewSubscriptions(IEnumerable<Subscription> activeSubscriptions)
        {
            activeSubscriptions.Where(s => !_subscriptions.ContainsKey(s.ID))
                               .ForEach(s => _subscriptions.Add(s.ID, _subscriptionFactory.CreateSubscription(s)));
        }

        /// <summary>
        /// Stops all subscriptions in the manager
        /// </summary>
        public void StopAllSubscriptions()
        {
            _subscriptions.Values.AsParallel().ForAll(s => s.Stop());

            // start updating data for subscriptions regularly
            _dataUpdateTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }

        /// <summary>
        /// Starts a single subscription
        /// </summary>
        /// <param name="subscriptionId"></param>
        public void StartSubcription(int subscriptionId)
        {
            // get subscription data
            var subscriptionData = _repository.Subscriptions.FirstOrDefault(s => s.ID == subscriptionId);

            // if data was not found, throw an exception
            if (subscriptionData == null)
                throw new SubscriptionNotFoundException(subscriptionId);

            // create subscription, if necessary
            if (!_subscriptions.ContainsKey(subscriptionId))
                _subscriptions.Add(subscriptionId, _subscriptionFactory.CreateSubscription(subscriptionData));
            else
                _subscriptions[subscriptionId].UpdateData(subscriptionData);

            // start the subscription
            _subscriptions[subscriptionId].Start();
        }

        /// <summary>
        /// Stops a single subscription
        /// </summary>
        /// <param name="subscriptionId"></param>
        public void StopSubscription(int subscriptionId)
        {
            // stop the subscription; if not found, log warning
            if (_subscriptions.ContainsKey(subscriptionId))
                _subscriptions[subscriptionId].Stop();
            else
                _logger.WarnFormat(Resources.SubscriptionNotLoadedWarningFormat, subscriptionId);
        }

        #endregion
    }
}