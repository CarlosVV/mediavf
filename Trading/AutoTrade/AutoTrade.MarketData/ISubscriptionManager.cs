namespace AutoTrade.MarketData
{
    public interface ISubscriptionManager
    {
        /// <summary>
        /// Starts all subscriptions in the manager
        /// </summary>
        void StartAllSubscriptions();

        /// <summary>
        /// Stops all subscriptions in the manager
        /// </summary>
        void StopAllSubscriptions();

        /// <summary>
        /// Starts a single subscriptionData
        /// </summary>
        /// <param name="subscriptionId"></param>
        void StartSubcription(int subscriptionId);

        /// <summary>
        /// Stops a single subscriptionData
        /// </summary>
        /// <param name="subscriptionId"></param>
        void StopSubscription(int subscriptionId);
    }
}
