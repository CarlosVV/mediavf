using System;
using AutoTrade.MarketData.Yahoo.Properties;

namespace AutoTrade.MarketData.Yahoo.Exceptions
{
    public class YqlResponseNodeMissingException : Exception
    {
        /// <summary>
        /// The name of the node that's missing
        /// </summary>
        private readonly string _nodeName;

        /// <summary>
        /// Instantiates a <see cref="YqlResponseNodeMissingException"/>
        /// </summary>
        /// <param name="nodeName"></param>
        public YqlResponseNodeMissingException(string nodeName)
        {
            _nodeName = nodeName;
        }

        /// <summary>
        /// Gets the message to display when a YQL response node is missing
        /// </summary>
        public override string Message
        {
            get { return string.Format(Resources.YqlResultNodeMissingFormat, _nodeName); }
        }
    }
}