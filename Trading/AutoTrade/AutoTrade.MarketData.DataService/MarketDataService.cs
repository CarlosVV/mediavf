using System.Collections.Generic;
using AutoTrade.MarketData.Data;

namespace AutoTrade.MarketData.DataService
{
    public class MarketDataService : IMarketDataService
    {
        /// <summary>
        /// The repository to access for data
        /// </summary>
        private readonly IMarketDataRepository _marketDataRepository;

        /// <summary>
        /// Instantiates a <see cref="MarketDataService"/>
        /// </summary>
        /// <param name="marketDataRepository"></param>
        public MarketDataService(IMarketDataRepository marketDataRepository)
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
