namespace AutoTrade.Accounts.Managers
{
    public interface IAccountManagerProvider
    {
        /// <summary>
        /// Gets the manager for an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        IAccountManager GetManagerForAccount(int accountId);
    }
}
