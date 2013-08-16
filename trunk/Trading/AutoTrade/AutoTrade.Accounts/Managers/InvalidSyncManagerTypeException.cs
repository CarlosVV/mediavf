using System;
using AutoTrade.Accounts.Properties;

namespace AutoTrade.Accounts
{
    public class InvalidSyncManagerTypeException : Exception
    {
        /// <summary>
        /// The type of sync manager
        /// </summary>
        private readonly string _syncManagerType;

        /// <summary>
        /// Instantiates an <see cref="InvalidSyncManagerTypeException"/>
        /// </summary>
        /// <param name="syncManagerType"></param>
        public InvalidSyncManagerTypeException(string syncManagerType)
        {
            _syncManagerType = syncManagerType;
        }

        /// <summary>
        /// Gets the message for the exception indicating the invalid type
        /// </summary>
        public override string Message
        {
            get { return string.Format(Resources.InvalidSyncManagerTypeMessage, _syncManagerType); }
        }
    }
}