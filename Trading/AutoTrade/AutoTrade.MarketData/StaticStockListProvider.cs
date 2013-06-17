using System;
using System.Collections.Generic;
using System.Linq;
using AutoTrade.MarketData.Data;
using AutoTrade.MarketData.Exceptions;

namespace AutoTrade.MarketData
{
    class StaticStockListProvider : IStockListProvider
    {
        /// <summary>
        /// The market data repository
        /// </summary>
        private readonly IMarketDataRepository _marketDataRepository;

        /// <summary>
        /// Instantiates a <see cref="StaticStockListProvider"/>
        /// </summary>
        /// <param name="marketDataRepository"></param>
        public StaticStockListProvider(IMarketDataRepository marketDataRepository)
        {
            _marketDataRepository = marketDataRepository;
        }

        /// <summary>
        /// Gets stocks to retrieve quotes for
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Stock> GetStocks(Subscription subscription)
        {
            if (subscription == null) throw new ArgumentNullException("subscription");

            // get the id from the subscription
            var subscriptionId = subscription.ID;

            // get the latest data for the subscription
            subscription = _marketDataRepository.Subscriptions.FirstOrDefault(s => s.ID == subscriptionId);
            
            // check that the subscription was found
            if (subscription == null)
                throw new SubscriptionNotFoundException(subscriptionId);

            // return the stocks for that subscription
            return subscription.Stocks;
        }
    }
}