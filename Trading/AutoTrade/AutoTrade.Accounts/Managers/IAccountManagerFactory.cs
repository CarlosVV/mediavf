using AutoTrade.Accounts.Data;
using AutoTrade.Accounts.Managers;

namespace AutoTrade.Accounts
{
    public interface IAccountManagerFactory
    {
        /// <summary>
        /// Creates an account manager for an account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        IAccountManager CreateAccountManager(Account account);
    }
}