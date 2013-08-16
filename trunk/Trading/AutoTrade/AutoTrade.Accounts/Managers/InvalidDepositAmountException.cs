using System;
using AutoTrade.Accounts.Properties;

namespace AutoTrade.Accounts
{
    public class InvalidDepositAmountException : Exception
    {
        /// <summary>
        /// The deposit amount that's invalid
        /// </summary>
        private readonly decimal _amount;

        /// <summary>
        /// Instantiates an <see cref="InvalidDepositAmountException"/>
        /// </summary>
        /// <param name="amount"></param>
        public InvalidDepositAmountException(decimal amount)
        {
            _amount = amount;
        }

        /// <summary>
        /// Gets the message for the exception
        /// </summary>
        public override string Message
        {
            get { return string.Format(Resources.InvalidDepositAmountMessage, _amount); }
        }
    }
}