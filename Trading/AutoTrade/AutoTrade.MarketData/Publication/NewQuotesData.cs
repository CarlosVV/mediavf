using System.Collections.Generic;
using AutoTrade.MarketData.Data;

namespace AutoTrade.MarketData.Publication
{
    public class NewQuotesData
    {
        /// <summary>
        /// The name of the queue
        /// </summary>
        public static readonly string QueueName = typeof(NewQuotesData).FullName.Replace(".", "-");

        /// <summary>
        /// Gets or sets the subscription id
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the collection of quotes
        /// </summary>
        public List<StockQuote> Quotes { get; set; }
    }
}