using System;
using System.Collections.Generic;
using AutoTrade.Core.Email;

namespace AutoTrade.MarketData.EmailAlerts
{
    public class EmailEventArgs : EventArgs
    {
        /// <summary>
        /// The emails associated with the event
        /// </summary>
        private readonly IEnumerable<IEmail> _emails;

        /// <summary>
        /// Instantiates a <see cref="EmailEventArgs"/>
        /// </summary>
        /// <param name="emails"></param>
        public EmailEventArgs(IEnumerable<IEmail> emails)
        {
            _emails = emails;
        }

        /// <summary>
        /// Gets the emails associated with the event
        /// </summary>
        public IEnumerable<IEmail> Emails
        {
            get { return _emails; }
        }
    }
}