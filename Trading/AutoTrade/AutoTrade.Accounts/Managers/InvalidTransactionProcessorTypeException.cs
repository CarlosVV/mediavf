using System;
using AutoTrade.Accounts.Properties;

namespace AutoTrade.Accounts.Managers
{
    public class InvalidTransactionProcessorTypeException : Exception
    {
        /// <summary>
        /// The type of sync manager
        /// </summary>
        private readonly string _transactionProcessorType;

        /// <summary>
        /// Instantiates an <see cref="InvalidTransactionProcessorTypeException"/>
        /// </summary>
        /// <param name="transactionProcessorType"></param>
        public InvalidTransactionProcessorTypeException(string transactionProcessorType)
        {
            _transactionProcessorType = transactionProcessorType;
        }

        /// <summary>
        /// Gets the message for the exception indicating the invalid type
        /// </summary>
        public override string Message
        {
            get { return string.Format(Resources.InvalidTransactionProcessorTypeMessage, _transactionProcessorType); }
        }
    }
}