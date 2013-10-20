using System;
using System.Collections.Generic;
using AutoTrade.MarketData.Data;

namespace AutoTrade.Trading
{
    public interface IQuoteSubscription
    {
        /// <summary>
        /// Event raised when new quotes are found
        /// </summary>
        event EventHandler<IEnumerable<StockQuote>> NewQuotesFound; 

        /// <summary>
        /// Starts subscription to quote data
        /// </summary>
        void Start();

        /// <summary>
        /// Stops subscription
        /// </summary>
        void Stop();
    }
}
