using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;
using AutoTrade.Core.UI;
using AutoTrade.MarketData.Dashboard.DataService;
using AutoTrade.MarketData.Dashboard.Properties;

namespace AutoTrade.MarketData.Dashboard
{
    class SubscriptionViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// The underlying subscription
        /// </summary>
        private readonly Subscription _subscription;

        /// <summary>
        /// The collection of quote summaries
        /// </summary>
        private ObservableCollection<SubscriptionQuoteSummary> _quoteSummaries;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="SubscriptionViewModel"/>
        /// </summary>
        /// <param name="subscription"></param>
        public SubscriptionViewModel(Subscription subscription)
        {
            _subscription = subscription;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the underlying subscription
        /// </summary>
        public Subscription Subscription
        {
            get { return _subscription; }
        }

        /// <summary>
        /// Gets or sets the collection of quote summaries
        /// </summary>
        public ObservableCollection<SubscriptionQuoteSummary> QuoteSummaries
        {
            get { return _quoteSummaries; }
            set
            {
                if (_quoteSummaries == value) return;
                _quoteSummaries = value;
                RaisePropertyChanged("QuoteSummaries");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads quote summaries for the subscription
        /// </summary>
        public async void LoadQuoteSummariesAsync()
        {
            BusyMessage = Resources.LoadingQuoteSummariesMessage;

            // get the data service url
            var url = new Uri(ConfigurationManager.AppSettings["DataServiceUrl"]);

            // create repository
            var repository = new MarketDataRepository(url);

            // build url to query
            var queryUrl = string.Format("GetQuoteSummaries?subscriptionId={0}", Subscription.ID);

            // get quote summaries
            var quoteSummaries =
                await Task.Factory.StartNew(() =>
                    repository.Execute<SubscriptionQuoteSummary>(new Uri(queryUrl, UriKind.Relative), "GET", false));

            // set quote summaries
            Application.Current.Dispatcher.Invoke(() =>
                {
                    // set quote summaries
                    QuoteSummaries = new ObservableCollection<SubscriptionQuoteSummary>(quoteSummaries);

                    // clear busy message 
                    BusyMessage = null;
                });
        }

        #endregion
    }
}
