using AutoTrade.Accounts.Data;

namespace AutoTrade.Accounts.Managers
{
    public interface IAccountManagerFactory
    {
        /// <summary>
        /// Creates an account manager for an account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        IAccountManager CreateAccountManager(CashAccount account);
    }
}