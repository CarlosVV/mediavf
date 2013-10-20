using System;
using AutoTrade.Accounts.Data;
using AutoTrade.Accounts.Properties;

namespace AutoTrade.Accounts.Managers
{
    public class AccountManagerFactory : IAccountManagerFactory
    {
        #region Fields

        /// <summary>
        /// The settings for account management
        /// </summary>
        private readonly IAccountManagementSettings _settings;

        /// <summary>
        /// The repository factory
        /// </summary>
        private readonly IAccountRepositoryFactory _repositoryFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates an <see cref="AccountManagerFactory"/>
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="repositoryFactory"></param>
        public AccountManagerFactory(IAccountManagementSettings settings, IAccountRepositoryFactory repositoryFactory)
        {
            _settings = settings;
            _repositoryFactory = repositoryFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates an account manager for an account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public IAccountManager CreateAccountManager(CashAccount account)
        {
            // check that the account is not null
            if (account == null) throw new ArgumentNullException("account");

            // check that the account has an account type
            if (account.AccountType == null)
                throw new ArgumentException(string.Format(Resources.AccountTypeNotLoadedMessage, account.Name));

            // create sync manager
            var transactionProcessor = CreateTransactionProcessor(account.AccountType);

            // create account manager
            return new AccountManager(_settings, transactionProcessor, _repositoryFactory, account);
        }
        
        /// <summary>
        /// Creates a sync manager from the account type
        /// </summary>
        /// <param name="accountType"></param>
        /// <returns></returns>
        private static ITransactionProcessor CreateTransactionProcessor(AccountType accountType)
        {
            // try to get the sync manager type for the account
            var transactionProcessorType = Type.GetType(accountType.TransactionProcessorType);

            // check if sync manager type is valid
            if (transactionProcessorType == null || !typeof(ITransactionProcessor).IsAssignableFrom(transactionProcessorType))
                throw new InvalidTransactionProcessorTypeException(accountType.TransactionProcessorType);

            return Activator.CreateInstance(transactionProcessorType) as ITransactionProcessor;
        }

        #endregion
    }
}