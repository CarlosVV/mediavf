using System;
using System.Linq;
using AutoTrade.Accounts.Data;

namespace AutoTrade.Accounts.Managers
{
    public class AccountManager : IAccountManager
    {
        #region Fields

        /// <summary>
        /// Manages synchronization between data maintained for the account remotely and locally
        /// </summary>
        private readonly ITransactionProcessor _transactionProcessor;

        /// <summary>
        /// The factory for creating account repositories
        /// </summary>
        private readonly IAccountRepositoryFactory _repositoryFactory;

        /// <summary>
        /// The account data
        /// </summary>
        private Account _account;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates an <see cref="AccountManager"/>
        /// </summary>
        /// <param name="transactionProcessor"></param>
        /// <param name="repositoryFactory"></param>
        /// <param name="account"></param>
        public AccountManager(ITransactionProcessor transactionProcessor,
            IAccountRepositoryFactory repositoryFactory,
            Account account)
        {
            _transactionProcessor = transactionProcessor;
            _repositoryFactory = repositoryFactory;
            _account = account;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks if the account has sufficient funds
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool HasSufficient(decimal amount)
        {
            return _account.Balance - amount >= _account.MinimumRequiredBalance;
        }

        /// <summary>
        /// Reserves funds 
        /// </summary>
        /// <param name="reservationKey"></param>
        /// <param name="amount"></param>
        /// <param name="timeToReserve"></param>
        /// <returns></returns>
        public void Reserve(Guid reservationKey, decimal amount, TimeSpan? timeToReserve = null)
        {
            using (var repository = _repositoryFactory.CreateRepository())
                repository.CreateFundReservation(_account.Id, reservationKey, amount, timeToReserve);
        }

        /// <summary>
        /// Releases reserved funds
        /// </summary>
        /// <param name="reservationKey"></param>
        public void Release(Guid reservationKey)
        {
            using (var repository = _repositoryFactory.CreateRepository())
                repository.ReleaseFundReservation(reservationKey);
        }

        /// <summary>
        /// Adds funds to the account
        /// </summary>
        /// <param name="amount"></param>
        public void Deposit(decimal amount)
        {
            // validate amount
            if (amount <= 0m) throw new InvalidDepositAmountException(amount);

            // create transaction that finalizes now
            using (var repository = _repositoryFactory.CreateRepository())
                repository.CreateTransaction(_account.Id, amount);
        }

        /// <summary>
        /// Withdraws funds from the account
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public void Withdraw(decimal amount)
        {
            // ensure the amount is a positive number
            amount = Math.Abs(amount);

            // create transaction to remove funds that finalizes now
            using (var repository = _repositoryFactory.CreateRepository())
                repository.CreateTransaction(_account.Id, amount);
        }

        /// <summary>
        /// Finalizes pending transactions
        /// </summary>
        public void ProcessTransactions()
        {
            using (var repository = _repositoryFactory.CreateRepository())
            {
                // get the latest account data
                var account =
                    repository.AccountsQuery.Include("AccountTransactions").FirstOrDefault(a => a.Id == _account.Id);

                // if the account was not found, throw an exception
                if (account == null) throw new AccountNotFoundException(_account.Id);

                // update pending transactions
                foreach (var pendingTransaction in account.Transactions.Where(t => t.StatusId == (int)TransactionStatusType.Pending))
                    UpdateBalanceFromPendingTransaction(account, pendingTransaction);

                // remove completed/canceled transactions
                foreach (
                    var completedTransaction in
                        account.Transactions.Where(
                            t =>
                            t.StatusId == (int) TransactionStatusType.Completed ||
                            t.StatusId == (int) TransactionStatusType.Cancelled))
                    repository.AccountTransactions.Remove(completedTransaction);

                // update the account
                _account = account;
            }
        }

        /// <summary>
        /// Updates the balance for the account from pending transactions
        /// </summary>
        /// <param name="account"></param>
        /// <param name="transaction"></param>
        private void UpdateBalanceFromPendingTransaction(Account account,
            AccountTransaction transaction)
        {
            // process the transaction
            _transactionProcessor.ProcessTransaction(transaction);

            // update the balance
            account.Balance += transaction.Amount;

            // set the status on the transaction to completed
            transaction.StatusId = (int)TransactionStatusType.Completed;
        }

        #endregion
    }
}