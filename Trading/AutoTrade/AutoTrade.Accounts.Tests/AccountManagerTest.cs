using System;
using System.Collections.ObjectModel;
using AutoTrade.Accounts.Data;
using AutoTrade.Accounts.Managers;
using AutoTrade.Tests;
using FakeItEasy;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTrade.Accounts.Tests
{
    [TestClass]
    public class AccountManagerTest
    {
        #region Fields

        /// <summary>
        /// The fake settings
        /// </summary>
        private IAccountManagementSettings _settings;

        /// <summary>
        /// The fake transaction processor
        /// </summary>
        private ITransactionProcessor _transactionProcessor;

        /// <summary>
        /// The account repository factory
        /// </summary>
        private IAccountRepositoryFactory _repositoryFactory;

        /// <summary>
        /// The account for the manager
        /// </summary>
        private CashAccount _account;

        /// <summary>
        /// The fake repository
        /// </summary>
        private IAccountRepository _repository;

        /// <summary>
        /// The account manager
        /// </summary>
        private AccountManager _accountManager;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes fields before running a test
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _settings = A.Fake<IAccountManagementSettings>();
            A.CallTo(() => _settings.FinalizedTransactionLifetime).Returns(TimeSpan.FromDays(7));

            _transactionProcessor = A.Fake<ITransactionProcessor>();
            _repositoryFactory = A.Fake<IAccountRepositoryFactory>();
            _repository = A.Fake<IAccountRepository>();

            A.CallTo(() => _repositoryFactory.CreateRepository()).Returns(_repository);

            _account = new CashAccount();

            _accountManager = new AccountManager(_settings, _transactionProcessor, _repositoryFactory, _account);
        }

        #endregion

        #region Tests

        #region HasSufficient

        /// <summary>
        /// If not enough funds, HasSufficient should return false
        /// </summary>
        [TestMethod]
        public void HasSufficient_ShouldReturnFalse()
        {
            // set balance
            _account.Balance = 100;

            // check if sufficient with number greater than balance
            var result = _accountManager.HasSufficient(200);

            // should return false
            result.Should().BeFalse();
        }

        /// <summary>
        /// If not enough funds, HasSufficient should return false
        /// </summary>
        [TestMethod]
        public void HasSufficient_ShouldReturnTrue()
        {
            // set balance
            _account.Balance = 200;

            // check if sufficient with number less than balance
            var result = _accountManager.HasSufficient(100);

            // should return false
            result.Should().BeTrue();
        }

        #endregion

        #region Deposit

        /// <summary>
        /// Exception should be thrown for invalid deposit value
        /// </summary>
        [TestMethod]
        public void Deposit_ShouldThrowForNegativeValue()
        {
            Action deposit = () => _accountManager.Deposit(-100);

            deposit.ShouldThrow<InvalidDepositAmountException>();
        }

        /// <summary>
        /// Balance should be increased by deposit
        /// </summary>
        [TestMethod]
        public void Deposit_ShouldIncreaseBalance()
        {
            // deposit
            _accountManager.Deposit(100);

            A.CallTo(() => _repository.CreateTransaction(A<int>.Ignored, 100, null)).MustHaveHappened();
        }

        #endregion

        #region Withdraw

        /// <summary>
        /// Balance should be decreased by withdrawal
        /// </summary>
        [TestMethod]
        public void Withdraw_ShouldDecreaseWithNegativeValue()
        {
            // withdraw negative amount
            _accountManager.Withdraw(-100);

            A.CallTo(() => _repository.CreateTransaction(A<int>.Ignored, -100, null)).MustHaveHappened();
        }

        /// <summary>
        /// Balance should be decreased by withdrawal
        /// </summary>
        [TestMethod]
        public void Withdraw_ShouldDecreaseWithPositiveValue()
        {
            // withdraw positive amount
            _accountManager.Withdraw(100);

            A.CallTo(() => _repository.CreateTransaction(A<int>.Ignored, -100, null)).MustHaveHappened();
        }

        #endregion

        #region Reserve/Release

        /// <summary>
        /// Fund reservations should be created
        /// </summary>
        [TestMethod]
        public void Reserve_ShouldCreateFundReservations()
        {
            // create key
            var reservationKey = Guid.NewGuid();

            // withdraw negative amount
            _accountManager.Reserve(reservationKey, 100);

            // check that a fund reserveation was created
            A.CallTo(() => _repository.CreateFundReservation(0, reservationKey, 100, null)).MustHaveHappened();
        }

        /// <summary>
        /// Fund reservations should be created
        /// </summary>
        [TestMethod]
        public void Release_ShouldRemoveFundReservations()
        {
            // create key
            var reservationKey = Guid.NewGuid();

            // withdraw negative amount
            _accountManager.Release(reservationKey);

            // check that a fund reserveation was created
            A.CallTo(() => _repository.ReleaseFundReservation(reservationKey)).MustHaveHappened();
        }

        #endregion

        #region ProcessTransactions

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ProcessTransactions_ShouldRemoveOldCancelledTransaction()
        {
            // create old cancelled transaction
            var oldCancelledTransaction = new CashAccountTransaction
                {
                    FinalizationDateTime = DateTime.Now.AddDays(-10),
                    StatusId = (int) TransactionStatusType.Cancelled
                };

            // create account with old transaction
            var account = new CashAccount { Transactions = new Collection<CashAccountTransaction> { oldCancelledTransaction } };

            // return account when retrieving from the repository
            A.CallTo(() => _repository.GetAccountWithTransactions(A<int>.Ignored)).Returns(account);

            // process transactions for the account
            _accountManager.ProcessTransactions();

            // transactions should be retrieved from the repository
            A.CallTo(() => _repository.GetAccountWithTransactions(A<int>.Ignored)).MustHaveHappened();

            // account should have been updated
            var newAccount = _accountManager.GetFieldValue<CashAccount>("_account");
            newAccount.Should().Be(account);

            // check that transactions are as expected
            newAccount.Transactions.Should().NotContain(t => t == oldCancelledTransaction);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ProcessTransactions_ShouldRemoveOldCompletedTransaction()
        {
            // create old cancelled transaction
            var oldCompletedTransaction = new CashAccountTransaction
                {
                    FinalizationDateTime = DateTime.Now.AddDays(-10),
                    StatusId = (int) TransactionStatusType.Completed
                };

            // create account with old transaction
            var account = new CashAccount { Transactions = new Collection<CashAccountTransaction> { oldCompletedTransaction } };

            // return account when retrieving from the repository
            A.CallTo(() => _repository.GetAccountWithTransactions(A<int>.Ignored)).Returns(account);

            // process transactions for the account
            _accountManager.ProcessTransactions();

            // transactions should be retrieved from the repository
            A.CallTo(() => _repository.GetAccountWithTransactions(A<int>.Ignored)).MustHaveHappened();

            // account should have been updated
            var newAccount = _accountManager.GetFieldValue<CashAccount>("_account");
            newAccount.Should().Be(account);

            // check that transactions are as expected
            newAccount.Transactions.Should().NotContain(t => t == oldCompletedTransaction);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ProcessTransactions_ShouldNotRemoveNewCancelledTransaction()
        {
            // create new cancelled transaction
            var newCancelledTransaction = new CashAccountTransaction
                {
                    FinalizationDateTime = DateTime.Now.AddDays(-2),
                    StatusId = (int) TransactionStatusType.Cancelled
                };

            // create account with old transaction
            var account = new CashAccount { Transactions = new Collection<CashAccountTransaction> { newCancelledTransaction } };

            // return account when retrieving from the repository
            A.CallTo(() => _repository.GetAccountWithTransactions(A<int>.Ignored)).Returns(account);

            // process transactions for the account
            _accountManager.ProcessTransactions();

            // transactions should be retrieved from the repository
            A.CallTo(() => _repository.GetAccountWithTransactions(A<int>.Ignored)).MustHaveHappened();

            // account should have been updated
            var newAccount = _accountManager.GetFieldValue<CashAccount>("_account");
            newAccount.Should().Be(account);

            // check that transactions are as expected
            newAccount.Transactions.Should().Contain(t => t == newCancelledTransaction);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ProcessTransactions_ShouldNotRemoveNewCompletedTransaction()
        {
            // create new cancelled transaction
            var newCompletedTransaction = new CashAccountTransaction
            {
                FinalizationDateTime = DateTime.Now.AddDays(-1),
                StatusId = (int)TransactionStatusType.Completed
            };

            // create account with old transaction
            var account = new CashAccount { Transactions = new Collection<CashAccountTransaction> { newCompletedTransaction } };

            // return account when retrieving from the repository
            A.CallTo(() => _repository.GetAccountWithTransactions(A<int>.Ignored)).Returns(account);

            // process transactions for the account
            _accountManager.ProcessTransactions();

            // transactions should be retrieved from the repository
            A.CallTo(() => _repository.GetAccountWithTransactions(A<int>.Ignored)).MustHaveHappened();

            // account should have been updated
            var newAccount = _accountManager.GetFieldValue<CashAccount>("_account");
            newAccount.Should().Be(account);

            // check that transactions are as expected
            newAccount.Transactions.Should().Contain(t => t == newCompletedTransaction);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ProcessTransactions_ShouldProcessPastDuePendingTransactionAndUpdateBalance()
        {
            // create new cancelled transaction
            var pastDuePendingTransaction = new CashAccountTransaction
                {
                    Amount = -100,
                    FinalizationDateTime = DateTime.Now.AddDays(-2),
                    StatusId = (int) TransactionStatusType.Pending
                };

            // create account with old transaction
            var account = new CashAccount { Balance = 500, Transactions = new Collection<CashAccountTransaction> { pastDuePendingTransaction } };

            // return account when retrieving from the repository
            A.CallTo(() => _repository.GetAccountWithTransactions(A<int>.Ignored)).Returns(account);

            // process transactions for the account
            _accountManager.ProcessTransactions();

            // transactions should be retrieved from the repository
            A.CallTo(() => _repository.GetAccountWithTransactions(A<int>.Ignored)).MustHaveHappened();

            // account should have been updated
            var newAccount = _accountManager.GetFieldValue<CashAccount>("_account");
            newAccount.Should().Be(account);

            // ensure the transaction was processed
            A.CallTo(() => _transactionProcessor.ProcessTransaction(pastDuePendingTransaction)).MustHaveHappened();

            // check that transactions are as expected
            newAccount.Transactions.Should().Contain(t => t == pastDuePendingTransaction);

            // check that balance was updated correctly
            newAccount.Balance.Should().Be(400);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ProcessTransactions_ShouldProcessDuePendingTransactionAndUpdateBalance()
        {
            // create new cancelled transaction
            var duePendingTransaction = new CashAccountTransaction
            {
                Amount = 500,
                FinalizationDateTime = DateTime.Now.AddMinutes(-1),
                StatusId = (int)TransactionStatusType.Pending
            };

            // create account with old transaction
            var account = new CashAccount { Balance = 500, Transactions = new Collection<CashAccountTransaction> { duePendingTransaction } };

            // return account when retrieving from the repository
            A.CallTo(() => _repository.GetAccountWithTransactions(A<int>.Ignored)).Returns(account);

            // process transactions for the account
            _accountManager.ProcessTransactions();

            // transactions should be retrieved from the repository
            A.CallTo(() => _repository.GetAccountWithTransactions(A<int>.Ignored)).MustHaveHappened();

            // account should have been updated
            var newAccount = _accountManager.GetFieldValue<CashAccount>("_account");
            newAccount.Should().Be(account);

            // ensure the transaction was processed
            A.CallTo(() => _transactionProcessor.ProcessTransaction(duePendingTransaction)).MustHaveHappened();

            // check that transactions are as expected
            newAccount.Transactions.Should().Contain(t => t == duePendingTransaction);

            // check that balance was updated correctly
            newAccount.Balance.Should().Be(1000);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ProcessTransactions_ShouldNotProcessPendingTransactionBeforeDate()
        {
            // create new cancelled transaction
            var pendingTransaction = new CashAccountTransaction
            {
                Amount = 500,
                FinalizationDateTime = DateTime.Now.AddDays(1),
                StatusId = (int)TransactionStatusType.Pending
            };

            // create account with old transaction
            var account = new CashAccount { Balance = 500, Transactions = new Collection<CashAccountTransaction> { pendingTransaction } };

            // return account when retrieving from the repository
            A.CallTo(() => _repository.GetAccountWithTransactions(A<int>.Ignored)).Returns(account);

            // process transactions for the account
            _accountManager.ProcessTransactions();

            // transactions should be retrieved from the repository
            A.CallTo(() => _repository.GetAccountWithTransactions(A<int>.Ignored)).MustHaveHappened();

            // account should have been updated
            var newAccount = _accountManager.GetFieldValue<CashAccount>("_account");
            newAccount.Should().Be(account);

            // ensure the transaction was processed
            A.CallTo(() => _transactionProcessor.ProcessTransaction(pendingTransaction)).MustNotHaveHappened();

            // check that transactions are as expected
            newAccount.Transactions.Should().Contain(t => t == pendingTransaction);

            // check that balance was updated correctly
            newAccount.Balance.Should().Be(500);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ProcessTransactions_ShouldNotProcessInProgressTransaction()
        {
            // create new cancelled transaction
            var inProgressTransaction = new CashAccountTransaction
            {
                Amount = 500,
                FinalizationDateTime = DateTime.Now.AddDays(-1),
                StatusId = (int)TransactionStatusType.InProgress
            };

            // create account with old transaction
            var account = new CashAccount { Balance = 500, Transactions = new Collection<CashAccountTransaction> { inProgressTransaction } };

            // return account when retrieving from the repository
            A.CallTo(() => _repository.GetAccountWithTransactions(A<int>.Ignored)).Returns(account);

            // process transactions for the account
            _accountManager.ProcessTransactions();

            // transactions should be retrieved from the repository
            A.CallTo(() => _repository.GetAccountWithTransactions(A<int>.Ignored)).MustHaveHappened();

            // account should have been updated
            var newAccount = _accountManager.GetFieldValue<CashAccount>("_account");
            newAccount.Should().Be(account);

            // ensure the transaction was processed
            A.CallTo(() => _transactionProcessor.ProcessTransaction(inProgressTransaction)).MustNotHaveHappened();

            // check that transactions are as expected
            newAccount.Transactions.Should().Contain(t => t == inProgressTransaction);

            // check that balance was updated correctly
            newAccount.Balance.Should().Be(500);
        }

        #endregion

        #endregion
    }
}
