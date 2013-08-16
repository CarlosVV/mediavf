using System;
using AutoTrade.Accounts.Data;
using AutoTrade.Accounts.Managers;
using AutoTrade.Accounts.Properties;

namespace AutoTrade.Accounts
{
    public class AccountManagerFactory : IAccountManagerFactory
    {
        #region Fields

        /// <summary>
        /// The repository factory
        /// </summary>
        private readonly IAccountRepositoryFactory _repositoryFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates an <see cref="AccountManagerFactory"/>
        /// </summary>
        /// <param name="repositoryFactory"></param>
        public AccountManagerFactory(IAccountRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates an account manager for an account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public IAccountManager CreateAccountManager(Account account)
        {
            // check that the account is not null
            if (account == null) throw new ArgumentNullException("account");

            // check that the account has an account type
            if (account.AccountType == null)
                throw new ArgumentException(string.Format(Resources.AccountTypeNotLoadedMessage, account.Name));

            // create sync manager
            var syncManager = CreateAccountSyncManager(account.AccountType);

            // create account manager
            return new AccountManager(syncManager, _repositoryFactory, account);
        }
        
        /// <summary>
        /// Creates a sync manager from the account type
        /// </summary>
        /// <param name="accountType"></param>
        /// <returns></returns>
        private static ITransactionProcessor CreateAccountSyncManager(AccountType accountType)
        {
            // try to get the sync manager type for the account
            var syncManagerType = Type.GetType(accountType.SynchronizationManagerType);

            // check if sync manager type is valid
            if (syncManagerType == null || !typeof(ITransactionProcessor).IsAssignableFrom(syncManagerType))
                throw new InvalidSyncManagerTypeException(accountType.SynchronizationManagerType);

            return Activator.CreateInstance(syncManagerType) as ITransactionProcessor;
        }

        #endregion
    }
}