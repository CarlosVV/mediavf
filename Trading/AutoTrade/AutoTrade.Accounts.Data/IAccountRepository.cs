using System;

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
        /// <returns></returns>
        void CreateFundReservation(int accountId, Guid reservationKey, decimal amount, TimeSpan? expiresIn = null);

        /// <summary>
        /// Releases reserved funds
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        void ReleaseFundReservation(Guid reservationKey);

        /// <summary>
        /// Creates a pending transaction and returns the id
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        int CreateTransaction(int accountId, decimal amount, TimeSpan? finalizesIn = null);
    }
}
