using System;
using AutoTrade.MarketData.Properties;

namespace AutoTrade.MarketData.Exceptions
{
    public class SubscriptionNotFoundException : Exception
    {
        /// <summary>
        /// The id of the subscription that was not found
        /// </summary>
        private readonly int _subscriptionId;

        /// <summary>
        /// Instantiates a <see cref="SubscriptionNotFoundException"/>
        /// </summary>
        /// <param name="subscriptionId"></param>
        public SubscriptionNotFoundException(int subscriptionId)
        {
            _subscriptionId = subscriptionId;
        }

        /// <summary>
        /// Gets message indicating that the subscription was not found for the given id
        /// </summary>
        public override string Message
        {
            get { return string.Format(Resources.SubscriptionNotFoundExceptionFormat, _subscriptionId); }
        }
    }
}
