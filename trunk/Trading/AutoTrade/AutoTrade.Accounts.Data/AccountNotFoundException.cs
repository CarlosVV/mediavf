using System;

namespace AutoTrade.Accounts.Data
{
    public class AccountNotFoundException : Exception
    {
        /// <summary>
        /// The id of the account not found
        /// </summary>
        private readonly int _accountId;

        /// <summary>
        /// Instantiates an <see cref="AccountNotFoundException"/>
        /// </summary>
        /// <param name="accountId"></param>
        public AccountNotFoundException(int accountId)
        {
            _accountId = accountId;
        }

        /// <summary>
        /// Gets the message indicating the account that was not found
        /// </summary>
        public override string Message
        {
            get { return string.Format("", _accountId); }
        }
    }
}