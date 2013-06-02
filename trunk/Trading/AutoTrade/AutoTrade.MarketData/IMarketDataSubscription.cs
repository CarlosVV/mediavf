using AutoTrade.MarketData.Entities;

namespace AutoTrade.MarketData
{
    public interface IMarketDataSubscription
    {
        /// <summary>
        /// Gets the status of the subscriptionData
        /// </summary>
        SubscriptionStatus Status { get; }

        /// <summary>
        /// Updates the data of the subscriptionData
        /// </summary>
        void UpdateData(Subscription subscriptionData);

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