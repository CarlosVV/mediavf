using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoTrade.Core.Modularity;

namespace AutoTrade.MarketData.Data
{
    public partial class MarketDataRepository
    {
        /// <summary>
        /// Gets module data from the repository
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IModuleData> GetModuleData()
        {
            // return all active modules
            return Modules.Where(m => m.Active);
        }

        /// <summary>
        /// Gets all subscriptions
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Subscription> GetAllActiveSubscriptions()
        {
            return SubscriptionsQuery.Include("DataProvider")
                                     .Include("StockListProvider")
                                     .Where(s => s.Active);
        }

        /// <summary>
        /// Gets a single subscription by id
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        public Subscription GetSubscription(int subscriptionId)
        {
            return SubscriptionsQuery.Include("DataProvider")
                                     .Include("StockListProvider")
                                     .FirstOrDefault(s => s.ID == subscriptionId);
        }

        /// <summary>
        /// Gets static list of stocks for a subscription
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        public IEnumerable<Stock> GetStaticStocksForSubscription(int subscriptionId)
        {
            return SubscriptionsQuery.Where(s => s.ID == subscriptionId).Include("Stocks").SelectMany(s => s.Stocks);
        }

        /// <summary>
        /// Updates the last polled time for an email feed
        /// </summary>
        /// <param name="emailFeedId"></param>
        public DateTime UpdateEmailFeedLastPolled(int emailFeedId)
        {
            // last checked time is now
            var lastChecked = DateTime.Now;

            // get the feed data
            var emailFeedConfig = EmailFeedConfigurationsQuery.FirstOrDefault(ef => ef.ID == emailFeedId);

            // check that the feed data was found
            if (emailFeedConfig != null)
            {
                // set the last checked time
                emailFeedConfig.LastChecked = lastChecked;

                // save changes
                SaveChanges();
            }

            return lastChecked;
        }
    }
}
