namespace AutoTrade.Accounts.Data
{
    public interface IAccountRepositoryFactory
    {
        /// <summary>
        /// Creates an <see cref="IAccountRepository"/>
        /// </summary>
        /// <returns></returns>
        IAccountRepository CreateRepository();
    }
}
