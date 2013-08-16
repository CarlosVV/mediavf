using System;
using AutoTrade.Accounts.Properties;

namespace AutoTrade.Accounts
{
    public class ExistingSessionException : Exception
    {
        /// <summary>
        /// The account that already has an open session
        /// </summary>
        private readonly int _accountId;

        /// <summary>
        /// Instantiates an <see cref="ExistingSessionException"/>
        /// </summary>
        /// <param name="accountId"></param>
        public ExistingSessionException(int accountId)
        {
            _accountId = accountId;
        }

        /// <summary>
        /// Gets the message indicating a session already open for the account
        /// </summary>
        public override string Message
        {
            get { return string.Format(Resources.ExistingSessionMessage, _accountId); }
        }
    }
}