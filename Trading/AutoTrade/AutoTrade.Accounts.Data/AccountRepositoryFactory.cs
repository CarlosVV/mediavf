namespace AutoTrade.Accounts.Data
{
    public class AccountRepositoryFactory : IAccountRepositoryFactory
    {
        /// <summary>
        /// Creates an <see cref="IAccountRepository"/>
        /// </summary>
        /// <returns></returns>
        public IAccountRepository CreateRepository()
        {
            // create repository
            var repository = new AccountRepository();

            // turn off lazy-loading
            repository.Configuration.LazyLoadingEnabled = false;

            // return the repository
            return repository;
        }
    }
}