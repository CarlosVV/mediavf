using System;
using System.Linq;

namespace AutoTrade.Accounts.Data
{
    public partial class AccountRepository
    {
        /// <summary>
        /// Creates a fund reservation and returns the id
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="reservationKey"></param>
        /// <param name="amount"></param>
        /// <param name="expiresIn"></param>
        /// <returns></returns>
        public void CreateFundReservation(int accountId, Guid reservationKey, decimal amount, TimeSpan? expiresIn = null)
        {
            // create fund reservation
            var fundReservation = new AccountFundReservation
                {
                    AccountId = accountId,
                    ReservationKey = reservationKey.ToString(),
                    Amount = amount,
                    Expiration = expiresIn.HasValue ? (DateTime?)(DateTime.Now + expiresIn.Value) : null,
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                };

            // add to db set
            AccountFundReservations.Add(fundReservation);

            // save changes in repository
            SaveChanges();
        }

        /// <summary>
        /// Releases reserved funds
        /// </summary>
        /// <param name="reservationKey"></param>
        /// <returns></returns>
        public void ReleaseFundReservation(Guid reservationKey)
        {
            // get the reservation
            var reservation = AccountFundReservations.FirstOrDefault(r => r.ReservationKey == reservationKey.ToString());

            // if reservation was not found, it doesn't exist - just return
            if (reservation == null) return;

            // remove the reservation
            AccountFundReservations.Remove(reservation);

            // save changes
            SaveChanges();
        }

        /// <summary>
        /// Creates a pending transaction and returns the id
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="amount"></param>
        /// <param name="finalizeIn"></param>
        /// <returns></returns>
        public int CreateTransaction(int accountId, decimal amount, TimeSpan? finalizeIn = null)
        {
            // create fund reservation
            var transaction = new AccountTransaction
                {
                    AccountId = accountId,
                    StatusId = (int)TransactionStatusType.Pending,
                    Amount = amount,
                    FinalizationDateTime = DateTime.Now + (finalizeIn ?? TimeSpan.Zero),
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                };

            // add to db set
            AccountTransactions.Add(transaction);

            // save changes in repository
            SaveChanges();

            // return the id
            return transaction.Id;
        }
    }
}
