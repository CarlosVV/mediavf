using System.Linq;
using AutoTrade.Accounts.Data;

namespace AutoTrade.Accounts.Managers
{
    public class AccountManagerProvider : IAccountManagerProvider
    {
        #region Fields

        /// <summary>
        /// The account repository factory
        /// </summary>
        private readonly IAccountRepositoryFactory _accountRepositoryFactory;

        /// <summary>
        /// The account manager factory
        /// </summary>
        private readonly IAccountManagerFactory _accountManagerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates an <see cref="AccountManagerProvider"/>
        /// </summary>
        /// <param name="accountRepositoryFactory"></param>
        /// <param name="accountManagerFactory"></param>
        public AccountManagerProvider(IAccountRepositoryFactory accountRepositoryFactory, IAccountManagerFactory accountManagerFactory)
        {
            _accountRepositoryFactory = accountRepositoryFactory;
            _accountManagerFactory = accountManagerFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the manager for an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public IAccountManager GetManagerForAccount(int accountId)
        {
            // get the account
            Account account;
            using (var repository = _accountRepositoryFactory.CreateRepository())
                account = repository.AccountsQuery.FirstOrDefault(a => a.Id == accountId);

            // throw exception indicating the account was not found
            if (account == null) throw new AccountNotFoundException(accountId);

            // create manager for account data
            return _accountManagerFactory.CreateAccountManager(account);
        }

        #endregion
    }
}