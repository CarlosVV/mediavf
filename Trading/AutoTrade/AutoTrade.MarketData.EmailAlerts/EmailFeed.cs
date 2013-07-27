using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AutoTrade.Core.Email;
using AutoTrade.MarketData.Data;

namespace AutoTrade.MarketData.EmailAlerts
{
    public class EmailFeed : IEmailFeed
    {
        #region Events

        /// <summary>
        /// Event raised when new emails are found
        /// </summary>
        public event EventHandler<EmailEventArgs> NewEmailsFound;

        /// <summary>
        /// Raises the <see cref="NewEmailsFound"/> event
        /// </summary>
        /// <param name="newEmails"></param>
        private void RaiseNewEmailsFound(IEnumerable<IEmail> newEmails)
        {
            if (NewEmailsFound != null)
                NewEmailsFound(this, new EmailEventArgs(newEmails));
        }

        #endregion

        #region Fields

        /// <summary>
        /// The factory for creating email managers
        /// </summary>
        private readonly IEmailManagerFactory _emailManagerFactory;

        /// <summary>
        /// The configuration for the feed
        /// </summary>
        private readonly EmailFeedConfiguration _emailFeedConfiguration;

        /// <summary>
        /// The email manager
        /// </summary>
        private readonly IEmailManager _emailManager;

        /// <summary>
        /// The polling timer
        /// </summary>
        private readonly Timer _pollingTimer;

        /// <summary>
        /// The last time the email account was polled
        /// </summary>
        private DateTime? _lastPolled;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates an <see cref="EmailFeed"/>
        /// </summary>
        /// <param name="emailManagerFactory"></param>
        /// <param name="emailFeedConfiguration"></param>
        public EmailFeed(IEmailManagerFactory emailManagerFactory, EmailFeedConfiguration emailFeedConfiguration)
        {
            // set config
            _emailManagerFactory = emailManagerFactory;
            _emailFeedConfiguration = emailFeedConfiguration;

            // create email manager
            _emailManager = _emailFeedConfiguration.CreateEmailManager(_emailManagerFactory);

            // create timer
            _pollingTimer = new Timer(RetrieveNewEmails);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts polling
        /// </summary>
        public void Start()
        {
            // start polling
            _pollingTimer.Change(_emailFeedConfiguration.PollingInterval, _emailFeedConfiguration.PollingInterval);
        }

        /// <summary>
        /// Stops polling
        /// </summary>
        public void Stop()
        {
            // stop polling
            _pollingTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Retrieves new emails
        /// </summary>
        /// <param name="state"></param>
        private void RetrieveNewEmails(object state)
        {
            // create criteria for search
            var searchCriteria = _emailFeedConfiguration.CreateSearchCriteria();
            searchCriteria.ReceivedAfter = _lastPolled;

            // perform search
            var newEmails = _emailManager.Search(searchCriteria);
            var newEmailList = newEmails as IList<IEmail> ?? newEmails.ToList();

            // set polling time
            _lastPolled = DateTime.Now;

            // if any new emails were found, raise event
            if (newEmailList.Count > 0)
                RaiseNewEmailsFound(newEmailList);
        }

        #endregion
    }
}
