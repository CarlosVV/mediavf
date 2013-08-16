using System;

namespace AutoTrade.Accounts.Managers
{
    public interface IAccountManager
    {
        /// <summary>
        /// Checks if the account has sufficient funds
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        bool HasSufficient(decimal amount);

        /// <summary>
        /// Reserves funds 
        /// </summary>
        /// <param name="reservationKey"></param>
        /// <param name="amount"></param>
        /// <param name="timeToReserve"></param>
        /// <returns></returns>
        void Reserve(Guid reservationKey, decimal amount, TimeSpan? timeToReserve = null);

        /// <summary>
        /// Releases reserved funds
        /// </summary>
        /// <param name="reservationKey"></param>
        void Release(Guid reservationKey);

        /// <summary>
        /// Adds funds to the account
        /// </summary>
        /// <param name="amount"></param>
        void Deposit(decimal amount);

        /// <summary>
        /// Withdraws funds from the account
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        void Withdraw(decimal amount);
    }
}
