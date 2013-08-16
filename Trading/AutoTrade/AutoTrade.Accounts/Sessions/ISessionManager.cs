using System;

namespace AutoTrade.Accounts.Sessions
{
    public interface ISessionManager
    {
        /// <summary>
        /// Starts a session
        /// </summary>
        /// <returns></returns>
        Guid StartSession(int accountId);

        /// <summary>
        /// Gets a session
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ISession GetSession(Guid token);

        /// <summary>
        /// Completes a session
        /// </summary>
        /// <param name="token"></param>
        /// <param name="keepOpen"></param>
        void CompleteSession(Guid token, bool keepOpen = false);

        /// <summary>
        /// Cancels a session
        /// </summary>
        /// <param name="token"></param>
        /// <param name="keepOpen"></param>
        void CancelSession(Guid token, bool keepOpen = false);
    }
}
