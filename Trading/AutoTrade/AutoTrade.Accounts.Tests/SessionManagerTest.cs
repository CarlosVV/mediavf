using System;
using System.Collections.Generic;
using AutoTrade.Accounts.Managers;
using AutoTrade.Accounts.Sessions;
using AutoTrade.Tests;
using FakeItEasy;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTrade.Accounts.Tests
{
    [TestClass]
    public class SessionManagerTest
    {
        #region Fields

        /// <summary>
        /// The fake account manager provider
        /// </summary>
        private IAccountManagerProvider _accountManagerProvider;

        /// <summary>
        /// The fake session factory
        /// </summary>
        private ISessionFactory _sessionFactory;

        /// <summary>
        /// The session manager to test against
        /// </summary>
        private SessionManager _sessionManager;

        #endregion

        #region Initialize

        /// <summary>
        /// Initializes fields for test
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            // create fakes
            _accountManagerProvider = A.Fake<IAccountManagerProvider>();
            _sessionFactory = A.Fake<ISessionFactory>();

            // create session manager
            _sessionManager = new SessionManager(_accountManagerProvider, _sessionFactory);
        }

        #endregion

        #region Tests

        /// <summary>
        /// An exception should be thrown in the account already has an existing open session
        /// </summary>
        [TestMethod]
        public void StartSession_ShouldThrowIfAccountIsAlreadyInUse()
        {
            // get list of sessions
            var sessions = _sessionManager.GetFieldValue<List<ISession>>("_sessions");

            // create fake session
            var fakeSession = A.Fake<ISession>();
            A.CallTo(() => fakeSession.AccountId).Returns(1);

            // add fake session
            sessions.Add(fakeSession);

            // create action to start session
            Action start = () => _sessionManager.StartSession(1);

            // ensure an exception was thrown
            start.ShouldThrow<ExistingSessionException>();
        }


        /// <summary>
        /// An exception should be thrown in the account already has an existing open session
        /// </summary>
        [TestMethod]
        public void StartSession_ShouldCreateNewSessionSuccessfully()
        {
            // get list of sessions
            var sessions = _sessionManager.GetFieldValue<List<ISession>>("_sessions");

            // create fake session
            var fakeSession = A.Fake<ISession>();
            A.CallTo(() => fakeSession.AccountId).Returns(2);

            // add fake session
            sessions.Add(fakeSession);

            var accountManager = A.Fake<IAccountManager>();
            A.CallTo(() => _accountManagerProvider.GetManagerForAccount(1)).Returns(accountManager);

            var session = A.Fake<ISession>();
            A.CallTo(() => session.Id).Returns(Guid.NewGuid());
            A.CallTo(() => _sessionFactory.CreateSession(1, accountManager)).Returns(session);

            // create action to start session
            var sessionId = _sessionManager.StartSession(1);

            // ensure accout manager was created for account
            A.CallTo(() => _accountManagerProvider.GetManagerForAccount(1)).MustHaveHappened();

            // ensure session was created with account manager
            A.CallTo(() => _sessionFactory.CreateSession(1, accountManager)).MustHaveHappened();

            sessionId.Should().Be(session.Id);
            sessions.Should().Contain(new[] {session});
            sessions.Should().HaveCount(2);
        }

        #endregion
    }
}
