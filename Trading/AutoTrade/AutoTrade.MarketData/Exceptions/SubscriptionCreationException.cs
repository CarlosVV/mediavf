using System;
using AutoTrade.MarketData.Properties;

namespace AutoTrade.MarketData.Exceptions
{
    class SubscriptionCreationException : Exception
    {
        /// <summary>
        /// The details of the subscription creation failure
        /// </summary>
        private readonly string _details;

        /// <summary>
        /// Instantiates a <see cref="SubscriptionCreationException"/>
        /// </summary>
        /// <param name="details"></param>
        public SubscriptionCreationException(string details, params object[] args)
        {
            _details = string.Format(details, args);
        }

        /// <summary>
        /// Gets the message indicating that subscription creation failed
        /// </summary>
        public override string Message
        {
            get { return string.Format(Resources.FailedToCreateSubscriptionExceptionFormat, _details); }
        }
    }
}
