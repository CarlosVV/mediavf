using System;
using System.Collections.Generic;
using AutoTrade.Core.Modularity;

namespace AutoTrade.MarketData.Data
{
    public partial interface IMarketDataRepository : IModuleDataRepository, IDisposable
    {
        /// <summary>
        /// Gets all subscriptions
        /// </summary>
        /// <returns></returns>
        IEnumerable<Subscription> GetAllActiveSubscriptions();

        /// <summary>
        /// Gets a single subscription by id
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        Subscription GetSubscription(int subscriptionId);

        /// <summary>
        /// Gets the static list of stocks for a subscription
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        IEnumerable<Stock> GetStaticStocksForSubscription(int subscriptionId);

        /// <summary>
        /// Updates the last polled time for an email feed
        /// </summary>
        /// <param name="emailFeedId"></param>
        DateTime UpdateEmailFeedLastPolled(int emailFeedId);
    }
}
