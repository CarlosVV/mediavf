using AutoTrade.Accounts.Managers;

namespace AutoTrade.Accounts.Sessions
{
    public interface ISessionFactory
    {
        /// <summary>
        /// Creates a session for an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="accountManager"></param>
        /// <returns></returns>
        ISession CreateSession(int accountId, IAccountManager accountManager);
    }
}
