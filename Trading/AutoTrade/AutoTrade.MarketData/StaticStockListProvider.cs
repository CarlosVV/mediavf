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

            // get the latest data for the subscription
            return _marketDataRepository.GetStaticStocksForSubscription(subscription.ID);
        }
    }
}