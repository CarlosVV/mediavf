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
        /// The factory for creating repositories
        /// </summary>
        private readonly IMarketDataRepositoryFactory _marketDataRepositoryFactory;

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
        /// <param name="marketDataRepositoryFactory"></param>
        /// <param name="emailFeedConfiguration"></param>
        public EmailFeed(IEmailManagerFactory emailManagerFactory,
            IMarketDataRepositoryFactory marketDataRepositoryFactory,
            EmailFeedConfiguration emailFeedConfiguration)
        {
            // set config
            _emailManagerFactory = emailManagerFactory;
            _marketDataRepositoryFactory = marketDataRepositoryFactory;
            _emailFeedConfiguration = emailFeedConfiguration;

            // create email manager
            _emailManager = _emailFeedConfiguration.CreateEmailManager(_emailManagerFactory);

            // set last polled time
            _lastPolled = _emailFeedConfiguration.LastChecked;

            // create timer
            _pollingTimer = new Timer(obj => RetrieveNewEmails());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the last time the email account was polled
        /// </summary>
        public DateTime? LastPolled
        {
            get { return _lastPolled; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts polling
        /// </summary>
        public void Start()
        {
            // retrieve new emails immediately
            RetrieveRecentEmails();

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
        /// Retrieves recent emails
        /// </summary>
        private void RetrieveRecentEmails()
        {
            // check that 
            if (_emailFeedConfiguration.PreviousEmailLoadDays.HasValue)
            {
                var previousEmailLoadDays = _emailFeedConfiguration.PreviousEmailLoadDays.Value;

                // create criteria for search
                var searchCriteria = _emailFeedConfiguration.CreateSearchCriteria();

                // set the since using the number of days from config
                searchCriteria.Since = DateTime.Now - TimeSpan.FromDays(previousEmailLoadDays);

                // retrieve old emails
                RetrieveEmails(searchCriteria);
            }
            else
                RetrieveNewEmails();
        }

        /// <summary>
        /// Retrieves new emails
        /// </summary>
        private void RetrieveNewEmails()
        {
            // create criteria for search
            var searchCriteria = _emailFeedConfiguration.CreateSearchCriteria();
            searchCriteria.Unread = true;

            // retrieve emails that match criteria
            RetrieveEmails(searchCriteria);
        }

        /// <summary>
        /// Retrieves emails based on search criteria
        /// </summary>
        /// <param name="searchCriteria"></param>
        private void RetrieveEmails(EmailSearchCriteria searchCriteria)
        {
            // perform search
            var newEmails = _emailManager.Search(searchCriteria);
            var newEmailList = newEmails as IList<IEmail> ?? newEmails.ToList();

            // set polling time
            using (var repository = _marketDataRepositoryFactory.CreateRepository())
                _lastPolled = repository.UpdateEmailFeedLastPolled(_emailFeedConfiguration.ID);

            // if any new emails were found, raise event
            if (newEmailList.Count > 0)
                RaiseNewEmailsFound(newEmailList);
        }

        #endregion
    }
}
