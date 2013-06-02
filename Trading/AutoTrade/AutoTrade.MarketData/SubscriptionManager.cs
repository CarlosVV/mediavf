using System.Collections.Generic;
using System.Linq;
using AutoTrade.Core;
using AutoTrade.MarketData.Entities;
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
        }

        #endregion

        #region Methods

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
                              .ForEach(kvp =>
                              {
                                  // update data for the subscription
                                  kvp.Value.UpdateData(activeSubscriptions[kvp.Key]);

                                  // start running the subscription
                                  kvp.Value.Start();
                              });
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
            _subscriptions.Values.ForEach(s => s.Stop());
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