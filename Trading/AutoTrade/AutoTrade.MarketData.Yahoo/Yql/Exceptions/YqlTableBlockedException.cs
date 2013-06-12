using System;
using AutoTrade.MarketData.Yahoo.Properties;

namespace AutoTrade.MarketData.Yahoo.Yql.Exceptions
{
    public class YqlTableBlockedException : Exception
    {
        /// <summary>
        /// The full message regarding table blocking
        /// </summary>
        private readonly string _blockedMessage;

        /// <summary>
        /// Instantiates a <see cref="YqlTableBlockedException"/>
        /// </summary>
        /// <param name="blockedMessage"></param>
        public YqlTableBlockedException(string blockedMessage)
        {
            _blockedMessage = blockedMessage;
        }

        /// <summary>
        /// Gets the message indicating the table is blocked
        /// </summary>
        public override string Message
        {
            get { return string.Format(Resources.YqlTableBlockedMessageFormat, _blockedMessage); }
        }
    }
}
