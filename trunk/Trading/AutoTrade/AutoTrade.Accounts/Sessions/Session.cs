using System;
using System.Collections.Generic;
using System.Transactions;
using AutoTrade.Accounts.Managers;

namespace AutoTrade.Accounts.Sessions
{
    public class Session : ISession
    {
        #region Fields

        /// <summary>
        /// The session id
        /// </summary>
        private readonly Guid _id;

        /// <summary>
        /// The id of the account for the session
        /// </summary>
        private readonly int _accountId;

        /// <summary>
        /// The account manager
        /// </summary>
        private readonly IAccountManager _accountManager;

        /// <summary>
        /// The collection of pending actions created as part of the session
        /// </summary>
        private readonly List<Action<IAccountManager>> _pendingActions = new List<Action<IAccountManager>>();

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="Session"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accountId"></param>
        /// <param name="accountManager"></param>
        public Session(Guid id, int accountId, IAccountManager accountManager)
        {
            _id = id;
            _accountId = accountId;
            _accountManager = accountManager;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the session id
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets the account id for the session
        /// </summary>
        public int AccountId
        {
            get { return _accountId; }
        }

        /// <summary>
        /// Gets the account manager for the session
        /// </summary>
        public IAccountManager AccountManager
        {
            get { return _accountManager; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an action to be completed when the session is completed
        /// </summary>
        /// <param name="action"></param>
        public void AddAction(Action<IAccountManager> action)
        {
            _pendingActions.Add(action);
        }

        /// <summary>
        /// Completes the session
        /// </summary>
        public void Complete()
        {
            using (var transactionScope = new TransactionScope())
            {
                // initialize loop variables
                var index = 0;
                var errorOccurred = false;

                // execute the actions
                while (!errorOccurred && index < _pendingActions.Count)
                {
                    try
                    {
                        // execute the current action
                        _pendingActions[index](_accountManager);

                        // move to the next action
                        index++;
                    }
                    catch
                    {
                        // indicate that an error occurred
                        errorOccurred = true;
                    }
                }

                // if no errors occurred, commit the transaction 
                if (errorOccurred) return;

                // complete the transaction
                transactionScope.Complete();

                // clear the actions for the session
                _pendingActions.Clear();
            }
        }

        /// <summary>
        /// Cancels the session
        /// </summary>
        public void Cancel()
        {
            // clear pending actions
            _pendingActions.Clear();
        }

        #endregion
    }
}