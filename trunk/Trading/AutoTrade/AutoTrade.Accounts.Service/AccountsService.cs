using System;
using System.Collections.Generic;
using System.Linq;
using AutoTrade.Accounts.Data;
using AutoTrade.Accounts.Sessions;

namespace AutoTrade.Accounts.Service
{
    public class AccountsService : IAccountsService
    {
        #region Fields

        /// <summary>
        /// The accounts repository factory
        /// </summary>
        private readonly IAccountRepositoryFactory _repositoryFactory;

        /// <summary>
        /// The session manager
        /// </summary>
        private readonly ISessionManager _sessionManager;
 
        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates an <see cref="AccountsService"/>
        /// </summary>
        /// <param name="repositoryFactory"></param>
        public AccountsService(IAccountRepositoryFactory repositoryFactory, ISessionManager sessionManager)
        {
            _repositoryFactory = repositoryFactory;
            _sessionManager = sessionManager;
        }

        #endregion

        #region Accounts

        /// <summary>
        /// Gets all accounts
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Account> GetAccounts()
        {
            using (var repository = _repositoryFactory.CreateRepository())
                return repository.AccountsQuery.ToList();
        }

        /// <summary>
        /// Gets an account by its name
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public Account GetAccountByName(string accountName)
        {
            using (var repository = _repositoryFactory.CreateRepository())
                return repository.AccountsQuery.FirstOrDefault(a => a.Name == accountName);
        }

        /// <summary>
        /// Gets an account by its id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public Account GetAccount(int accountId)
        {
            using (var repository = _repositoryFactory.CreateRepository())
                return repository.AccountsQuery.FirstOrDefault(a => a.Id == accountId);
        }

        #endregion

        #region Sessions

        /// <summary>
        /// Creates a token that allows for transactions against an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public Guid StartSession(int accountId)
        {
            return _sessionManager.StartSession(accountId);
        }

        /// <summary>
        /// Completes a session - all related transactions are committed
        /// </summary>
        /// <param name="token"></param>
        /// <param name="keepOpen"></param>
        /// <returns></returns>
        public void CompleteSession(Guid token, bool keepOpen = false)
        {
            _sessionManager.CompleteSession(token, keepOpen);
        }

        /// <summary>
        /// Cancels a session - all related transactions are not committed
        /// </summary>
        /// <param name="token"></param>
        /// <param name="keepOpen"></param>
        /// <returns></returns>
        public void CancelSession(Guid token, bool keepOpen = false)
        {
            _sessionManager.CancelSession(token, keepOpen);
        }

        #endregion

        #region Funds

        /// <summary>
        /// Checks if the account has sufficient funds
        /// </summary>
        /// <param name="sessionToken"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool HasSufficientFunds(Guid sessionToken, decimal amount)
        {
            // get the session
            var session = _sessionManager.GetSession(sessionToken);

            // check if the account has sufficient funds
            return session.AccountManager.HasSufficient(amount);
        }

        /// <summary>
        /// Reserves funds 
        /// </summary>
        /// <param name="sessionToken"></param>
        /// <param name="amount"></param>
        /// <param name="timeToReserve"></param>
        /// <returns></returns>
        public Guid ReserveFunds(Guid sessionToken, decimal amount, TimeSpan? timeToReserve = null)
        {
            // get the session
            var session = _sessionManager.GetSession(sessionToken);

            // create key for reservation
            var reservationKey = Guid.NewGuid();

            // reserve funds in the account
            session.AddAction(accountManager => accountManager.Reserve(reservationKey, amount));

            // return the key for the reservation
            return reservationKey;
        }

        /// <summary>
        /// Releases reserved funds
        /// </summary>
        /// <param name="sessionToken"></param>
        /// <param name="reservationKey"></param>
        public void ReleaseFunds(Guid sessionToken, Guid reservationKey)
        {
            // get the session
            var session = _sessionManager.GetSession(sessionToken);

            // release reserved funds in the account
            session.AddAction(accountManager => accountManager.Release(reservationKey));
        }

        /// <summary>
        /// Adds funds to the account
        /// </summary>
        /// <param name="sessionToken"></param>
        /// <param name="amount"></param>
        public void DepositFunds(Guid sessionToken, decimal amount)
        {
            // get the session
            var session = _sessionManager.GetSession(sessionToken);

            // deposit to account
            session.AddAction(accountManager => accountManager.Deposit(amount));
        }

        /// <summary>
        /// Withdraws funds from the account
        /// </summary>
        /// <param name="sessionToken"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public void WithdrawFunds(Guid sessionToken, decimal amount)
        {
            // get the session
            var session = _sessionManager.GetSession(sessionToken);

            // deposit to account
            session.AddAction(accountManager => accountManager.Withdraw(amount));
        }

        #endregion
    }
}