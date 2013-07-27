using System.Collections.Generic;
using AutoTrade.MarketData.Data;

namespace AutoTrade.MarketData.EmailAlerts
{
    interface IEmailFeedFactory
    {
        /// <summary>
        /// Adds configurations for feeds to the factory
        /// </summary>
        /// <param name="configurations"></param>
        void AddConfigurations(IEnumerable<EmailFeedConfiguration> configurations);

        /// <summary>
        /// Creates a feed
        /// </summary>
        /// <param name="feedName"></param>
        /// <returns></returns>
        IEmailFeed CreateFeed(string feedName);
    }
}
