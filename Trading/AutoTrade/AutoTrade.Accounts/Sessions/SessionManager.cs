using System;
using System.Collections.Generic;
using System.Linq;
using AutoTrade.Accounts.Managers;

namespace AutoTrade.Accounts.Sessions
{
    public class SessionManager : ISessionManager
    {
        #region Fields

        /// <summary>
        /// The session factory
        /// </summary>
        private readonly ISessionFactory _sessionFactory;

        /// <summary>
        /// The account manager provider
        /// </summary>
        private readonly IAccountManagerProvider _accountManagerProvider;

        /// <summary>
        /// The lock for accessing the active sessions
        /// </summary>
        private readonly object _sessionLock = new object();

        /// <summary>
        /// The collection of open sessions account
        /// </summary>
        private readonly List<ISession> _sessions = new List<ISession>(); 

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="SessionManager"/>
        /// </summary>
        /// <param name="accountManagerProvider"></param>
        /// <param name="sessionFactory"></param>
        public SessionManager(IAccountManagerProvider accountManagerProvider, ISessionFactory sessionFactory)
        {
            _accountManagerProvider = accountManagerProvider;
            _sessionFactory = sessionFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts a session
        /// </summary>
        /// <returns></returns>
        public Guid StartSession(int accountId)
        {
            lock (_sessionLock)
            {
                // check if account already has an open session
                if (_sessions.Any(s => s.AccountId == accountId)) throw new ExistingSessionException(accountId);

                // get the account manager
                var accountManager = _accountManagerProvider.GetManagerForAccount(accountId);

                // create session
                var session = _sessionFactory.CreateSession(accountId, accountManager);

                // add the new session
                _sessions.Add(session);

                // return the id of the new session
                return session.Id;
            }
        }

        /// <summary>
        /// Gets the account associated with a session
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ISession GetSession(Guid token)
        {
            lock (_sessionLock)
            {
                // try to retrieve session by id
                var session = _sessions.FirstOrDefault(s => s.Id == token);

                // if session is null, throw
                if (session == null) throw new SessionNotFoundException(token);

                // return the session
                return session;
            }
        }

        /// <summary>
        /// Completes a session
        /// </summary>
        /// <param name="token"></param>
        /// <param name="keepOpen"></param>
        public void CompleteSession(Guid token, bool keepOpen = false)
        {
            lock (_sessionLock)
            {
                // get the session
                var session = _sessions.FirstOrDefault(s => s.Id == token);

                // if session was not found, just return
                if (session == null) throw new SessionNotFoundException(token);

                // complete the session
                session.Complete();

                // remove the session, if indicated
                if (!keepOpen) _sessions.Remove(session);
            }
        }

        /// <summary>
        /// Cancels a session
        /// </summary>
        /// <param name="token"></param>
        /// <param name="keepOpen"></param>
        public void CancelSession(Guid token, bool keepOpen = false)
        {
            lock (_sessionLock)
            {
                // get the session
                var session = _sessions.FirstOrDefault(s => s.Id == token);

                // if session was not found, just return
                if (session == null) throw new SessionNotFoundException(token);

                // complete the session
                session.Cancel();

                // remove the session, if indicated
                if (!keepOpen) _sessions.Remove(session);
            }
        }

        #endregion
    }
}