using System;
using System.Linq;
using AutoTrade.Accounts.Data;

namespace AutoTrade.Accounts.Managers
{
    public class AccountManager : IAccountManager
    {
        #region Fields

        /// <summary>
        /// The settings for managing accounts
        /// </summary>
        private readonly IAccountManagementSettings _settings;

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

        /// <summary>
        /// The lock to regulate accessing the account
        /// </summary>
        private readonly object _accessLock = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates an <see cref="AccountManager"/>
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="transactionProcessor"></param>
        /// <param name="repositoryFactory"></param>
        /// <param name="account"></param>
        public AccountManager(IAccountManagementSettings settings,
            ITransactionProcessor transactionProcessor,
            IAccountRepositoryFactory repositoryFactory,
            Account account)
        {
            _settings = settings;
            _transactionProcessor = transactionProcessor;
            _repositoryFactory = repositoryFactory;
            _account = account;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the currently available balance, excluding reserved funds
        /// </summary>
        public decimal AvailableBalance
        {
            get
            {
                lock (_accessLock)
                    return _account.Balance - _account.FundReservations.Sum(fr => fr.Amount);
            }
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
            lock (_accessLock)
                return AvailableBalance - amount >= _account.MinimumRequiredBalance;
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
            lock (_accessLock)
                using (var repository = _repositoryFactory.CreateRepository())
                    repository.CreateFundReservation(_account.Id, reservationKey, amount, timeToReserve);
        }

        /// <summary>
        /// Releases reserved funds
        /// </summary>
        /// <param name="reservationKey"></param>
        public void Release(Guid reservationKey)
        {
            lock (_accessLock)
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
            lock (_accessLock)
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
            lock (_accessLock)
                using (var repository = _repositoryFactory.CreateRepository())
                    repository.CreateTransaction(_account.Id, -amount);
        }

        /// <summary>
        /// Finalizes pending transactions
        /// </summary>
        public void ProcessTransactions()
        {
            lock (_accessLock)
            {
                using (var repository = _repositoryFactory.CreateRepository())
                {
                    // get the latest transaction data
                    var account = repository.GetAccountWithTransactions(_account.Id);

                    // update account data
                    _account = account;
                    
                    // get the collection of transactions to process
                    var transactionsToProcess = _account.Transactions
                                                        .Where(t => t.StatusId == (int) TransactionStatusType.Pending &&
                                                                    t.FinalizationDateTime <= DateTime.Now)
                                                        .ToList();

                    // set transactions to in progress
                    transactionsToProcess.ForEach(t => t.StatusId = (int) TransactionStatusType.InProgress);

                    // save the changes
                    repository.SaveChanges();

                    // update pending transactions
                    _account.Balance += transactionsToProcess.Sum(t => UpdateBalanceFromPendingTransaction(t));

                    // remove completed/canceled transactions
                    foreach (
                        var completedTransaction in
                            _account.Transactions.Where(
                                t =>
                                DateTime.Now - t.FinalizationDateTime >= _settings.FinalizedTransactionLifetime &&
                                (t.StatusId == (int) TransactionStatusType.Completed ||
                                 t.StatusId == (int) TransactionStatusType.Cancelled)).ToList())
                    {
                        // remove from account
                        _account.Transactions.Remove(completedTransaction);

                        // mark for deletion in repository
                        repository.AccountTransactions.Remove(completedTransaction);
                    }

                    // save changes made to the account
                    repository.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Updates the balance for the account from pending transactions
        /// </summary>
        /// <param name="transaction"></param>
        private decimal UpdateBalanceFromPendingTransaction(AccountTransaction transaction)
        {
            try
            {
                // process the transaction
                _transactionProcessor.ProcessTransaction(transaction);

                // set the status on the transaction to completed
                transaction.StatusId = (int)TransactionStatusType.Completed;

                // return the amount for the transaction
                return transaction.Amount;
            }
            catch
            {
                // mark as failed
                transaction.StatusId = (int) TransactionStatusType.Failed;

                return 0;
            }
        }

        #endregion
    }
}