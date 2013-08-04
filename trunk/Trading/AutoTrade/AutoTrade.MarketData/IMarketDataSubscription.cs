using AutoTrade.MarketData.Data;

namespace AutoTrade.MarketData
{
    public interface IMarketDataSubscription
    {
        /// <summary>
        /// Gets the status of the subscriptionData
        /// </summary>
        SubscriptionStatus Status { get; }

        /// <summary>
        /// Updates data for a subscription
        /// </summary>
        void UpdateSubscriptionData();

        /// <summary>
        /// Updates the data of the subscriptionData
        /// </summary>
        void UpdateSubscriptionData(Subscription subscriptionData);

        /// <summary>
        /// Starts regular updates of data
        /// </summary>
        void Start();

        /// <summary>
        /// Stops regular updates of data
        /// </summary>
        void Stop();
    }
}