using System;
using System.Collections.Generic;

namespace AutoTrade.Accounts.Data
{
    public partial interface IAccountRepository : IDisposable
    {
        /// <summary>
        /// Creates a fund reservation and returns the id
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="reservationKey"></param>
        /// <param name="amount"></param>
        /// <param name="expiresIn"></param>
        /// <returns></returns>
        void CreateFundReservation(int accountId, Guid reservationKey, decimal amount, TimeSpan? expiresIn = null);

        /// <summary>
        /// Releases reserved funds
        /// </summary>
        /// <param name="reservationKey"></param>
        /// <returns></returns>
        void ReleaseFundReservation(Guid reservationKey);

        /// <summary>
        /// Creates a pending transaction and returns the id
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="amount"></param>
        /// <param name="finalizesIn"></param>
        /// <returns></returns>
        int CreateTransaction(int accountId, decimal amount, TimeSpan? finalizesIn = null);

        /// <summary>
        /// Gets pending transactions for an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Account GetAccountWithTransactions(int accountId);

        /// <summary>
        /// Updates the balance for an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="balance"></param>
        void UpdateAccountBalance(int accountId, decimal balance);
    }
}
