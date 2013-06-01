using System;
using AutoTrade.Core.Properties;

namespace AutoTrade.Core.Web
{
    public class NullWebResponseStreamException : Exception
    {
        /// <summary>
        /// The url that generated the exception
        /// </summary>
        private readonly string _url;

        /// <summary>
        /// Instantiates a <see cref="NullWebResponseStreamException"/>
        /// </summary>
        /// <param name="url"></param>
        public NullWebResponseStreamException(string url)
        {
            _url = url;
        }

        /// <summary>
        /// The message indicating that the response stream for the url was null
        /// </summary>
        public override string Message
        {
            get { return string.Format(Resources.NullWebResponseStreamMessageFormat, _url); }
        }
    }
}