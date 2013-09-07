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
    public class SessionTest
    {
        #region Fields

        /// <summary>
        /// The fake account manager
        /// </summary>
        private IAccountManager _accountManager;

        /// <summary>
        /// The session to test
        /// </summary>
        private Session _session;

        #endregion

        #region Initialize

        [TestInitialize]
        public void Initialize()
        {
            _accountManager = A.Fake<IAccountManager>();

            _session = new Session(Guid.NewGuid(), 1, _accountManager);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void AddAction_ShouldAddToUnderlyingCollectionAndNotExecute()
        {
            // create action to be executed
            Action<IAccountManager> deposit = acctMgr => acctMgr.Deposit(100);

            // add the action
            _session.AddAction(deposit);

            // get pending actions
            var pendingActions = _session.GetFieldValue<IEnumerable<Action<IAccountManager>>>("_pendingActions");

            // check that action was added to collection
            pendingActions.Should().Contain(new[] {deposit});

            // ensure deposit was not yet called
            A.CallTo(() => _accountManager.Deposit(A<decimal>.Ignored)).MustNotHaveHappened();
        }

        [TestMethod]
        public void Complete_ShouldInvokeActionsAndClearCollection()
        {
            // create action to be executed
            Action<IAccountManager> deposit = acctMgr => acctMgr.Deposit(500);
            Action<IAccountManager> withdraw = acctMgr => acctMgr.Withdraw(200);
            Action<IAccountManager> reserve = acctMgr => acctMgr.Reserve(Guid.NewGuid(), 300);

            // add the action
            _session.AddAction(deposit);
            _session.AddAction(withdraw);
            _session.AddAction(reserve);

            // complete the session
            _session.Complete();

            // check that all actions were executed
            A.CallTo(() => _accountManager.Deposit(500)).MustHaveHappened();
            A.CallTo(() => _accountManager.Withdraw(200)).MustHaveHappened();
            A.CallTo(() => _accountManager.Reserve(A<Guid>.Ignored, 300, null)).MustHaveHappened();

            // check that actions were cleared
            var pendingActions = _session.GetFieldValue<IEnumerable<Action<IAccountManager>>>("_pendingActions");
            pendingActions.Should().BeEmpty();
        }

        [TestMethod]
        public void Complete_ShouldNotContinueExecutingOnFailure()
        {
            // create action to be executed
            Action<IAccountManager> deposit = acctMgr => acctMgr.Deposit(500);
            Action<IAccountManager> withdraw = acctMgr => acctMgr.Withdraw(200);
            Action<IAccountManager> reserve = acctMgr => acctMgr.Reserve(Guid.NewGuid(), 300);

            // throw exception on withdrawal
            A.CallTo(() => _accountManager.Withdraw(A<decimal>.Ignored)).Throws<Exception>();

            // add the action
            _session.AddAction(deposit);
            _session.AddAction(withdraw);
            _session.AddAction(reserve);

            // complete the session
            _session.Complete();

            // check that all actions were executed
            A.CallTo(() => _accountManager.Deposit(500)).MustHaveHappened();
            A.CallTo(() => _accountManager.Withdraw(200)).MustHaveHappened();
            A.CallTo(() => _accountManager.Reserve(A<Guid>.Ignored, 300, null)).MustNotHaveHappened();

            // check that actions were cleared
            var pendingActions = _session.GetFieldValue<IEnumerable<Action<IAccountManager>>>("_pendingActions");
            pendingActions.Should().NotBeEmpty();
        }

        [TestMethod]
        public void Cancel_ShouldClearActions()
        {
            // create action to be executed
            Action<IAccountManager> deposit = acctMgr => acctMgr.Deposit(500);
            Action<IAccountManager> withdraw = acctMgr => acctMgr.Withdraw(200);
            Action<IAccountManager> reserve = acctMgr => acctMgr.Reserve(Guid.NewGuid(), 300);

            // add the action
            _session.AddAction(deposit);
            _session.AddAction(withdraw);
            _session.AddAction(reserve);

            // complete the session
            _session.Cancel();

            // check that all actions were executed
            A.CallTo(() => _accountManager.Deposit(500)).MustNotHaveHappened();
            A.CallTo(() => _accountManager.Withdraw(200)).MustNotHaveHappened();
            A.CallTo(() => _accountManager.Reserve(A<Guid>.Ignored, 300, null)).MustNotHaveHappened();

            // check that actions were cleared
            var pendingActions = _session.GetFieldValue<IEnumerable<Action<IAccountManager>>>("_pendingActions");
            pendingActions.Should().BeEmpty();
        }

        #endregion
    }
}
