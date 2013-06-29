using System.Collections.Generic;
using AutoTrade.MarketData.Data;

namespace AutoTrade.MarketData.DataService
{
    public class OldService : IOldService
    {
        /// <summary>
        /// The repository to access for data
        /// </summary>
        private readonly IMarketDataRepository _marketDataRepository;

        /// <summary>
        /// Instantiates a <see cref="OldService"/>
        /// </summary>
        /// <param name="marketDataRepository"></param>
        public OldService(IMarketDataRepository marketDataRepository)
        {
            _marketDataRepository = marketDataRepository;
        }

        /// <summary>
        /// Gets the collection of subscriptions
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Subscription> GetSubscriptions()
        {
            return _marketDataRepository.Subscriptions;
        }
    }
}
