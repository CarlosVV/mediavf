using System;
using AutoTrade.Accounts.Managers;

namespace AutoTrade.Accounts.Sessions
{
    public interface ISession
    {
        /// <summary>
        /// Gets the session id
        /// </summary>
        Guid Id { get;  }

        /// <summary>
        /// Gets the account id for the session
        /// </summary>
        int AccountId { get; }

        /// <summary>
        /// Gets the account manager for the session
        /// </summary>
        IAccountManager AccountManager { get; }

        /// <summary>
        /// Adds an action to the session
        /// </summary>
        /// <param name="action"></param>
        void AddAction(Action<IAccountManager> action);

        /// <summary>
        /// Completes the session
        /// </summary>
        void Complete();

        /// <summary>
        /// Cancels the session
        /// </summary>
        void Cancel();
    }
}
