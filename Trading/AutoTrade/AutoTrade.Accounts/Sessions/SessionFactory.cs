using System;
using AutoTrade.Accounts.Managers;

namespace AutoTrade.Accounts.Sessions
{
    public class SessionFactory : ISessionFactory
    {
        /// <summary>
        /// Creates a session for an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="accountManager"></param>
        /// <returns></returns>
        public ISession CreateSession(int accountId, IAccountManager accountManager)
        {
            return new Session(Guid.NewGuid(), accountId, accountManager);
        }
    }
}