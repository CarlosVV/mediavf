using System;
using System.Collections.Generic;
using System.ServiceModel;
using AutoTrade.Accounts.Data;

namespace AutoTrade.Accounts.Service
{
    [ServiceContract]
    public interface IAccountsService
    {
        #region Accounts

        /// <summary>
        /// Gets all accounts
        /// </summary>
        /// <returns></returns>
        IEnumerable<Account> GetAccounts();

        /// <summary>
        /// Gets an account by its name
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        Account GetAccountByName(string accountName);

        /// <summary>
        /// Gets an account by its id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Account GetAccount(int accountId);

        #endregion

        #region Sessions

        /// <summary>
        /// Creates a token that allows for transactions against an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Guid StartSession(int accountId);

        /// <summary>
        /// Completes a session - all related transactions are committed
        /// </summary>
        /// <param name="token"></param>
        /// <param name="keepOpen"></param>
        /// <returns></returns>
        void CompleteSession(Guid token, bool keepOpen = false);

        /// <summary>
        /// Cancels a session - all related transactions are not committed
        /// </summary>
        /// <param name="token"></param>
        /// <param name="keepOpen"></param>
        /// <returns></returns>
        void CancelSession(Guid token, bool keepOpen = false);

        #endregion

        #region Funds

        /// <summary>
        /// Checks if the account has sufficient funds
        /// </summary>
        /// <param name="token"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        bool HasSufficientFunds(Guid token, decimal amount);

        /// <summary>
        /// Reserves funds 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="amount"></param>
        /// <param name="timeToReserve"></param>
        /// <returns></returns>
        Guid ReserveFunds(Guid token, decimal amount, TimeSpan? timeToReserve = null);

        /// <summary>
        /// Releases reserved funds
        /// </summary>
        /// <param name="token"></param>
        /// <param name="reservationKey"></param>
        void ReleaseFunds(Guid token, Guid reservationKey);

        /// <summary>
        /// Adds funds to the account
        /// </summary>
        /// <param name="token"></param>
        /// <param name="amount"></param>
        void DepositFunds(Guid token, decimal amount);

        /// <summary>
        /// Withdraws funds from the account
        /// </summary>
        /// <param name="token"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        void WithdrawFunds(Guid token, decimal amount);

        #endregion
    }
}
