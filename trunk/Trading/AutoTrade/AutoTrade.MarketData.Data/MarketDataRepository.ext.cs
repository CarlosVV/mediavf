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
    }
}
