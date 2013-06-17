using AutoTrade.MarketData.Data;

namespace AutoTrade.MarketData
{
    public interface ISubscriptionFactory
    {
        /// <summary>
        /// Creates a market data subscription
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        IMarketDataSubscription CreateSubscription(Subscription subscription);
    }
}
