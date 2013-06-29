using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AutoTrade.Core.UI;
using AutoTrade.MarketData.Dashboard.DataService;
using AutoTrade.MarketData.Dashboard.Properties;

namespace AutoTrade.MarketData.Dashboard
{
    class SubscriptionsViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        /// The collection of data providers
        /// </summary>
        private ObservableCollection<DataProvider> _dataProviders;

        /// <summary>
        /// The collection of stock list providers
        /// </summary>
        private ObservableCollection<StockListProvider> _stockListProviders;

        /// <summary>
        /// The collection of subscriptions
        /// </summary>
        private ObservableCollection<SubscriptionViewModel> _subscriptions;

        /// <summary>
        /// The selected subscription
        /// </summary>
        private SubscriptionViewModel _selectedSubscription;

        /// <summary>
        /// The command for loading subscriptions
        /// </summary>
        private DelegateCommand _loadSubscriptionsCommand;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="SubscriptionsViewModel"/>
        /// </summary>
        public SubscriptionsViewModel()
        {
            LoadAsync();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets flag indicating if a subscription is selected
        /// </summary>
        public bool IsSubscriptionSelected
        {
            get { return _selectedSubscription != null; }
        }

        /// <summary>
        /// Gets or sets the collection of data providers
        /// </summary>
        public ObservableCollection<DataProvider> DataProviders
        {
            get { return _dataProviders; }
            set
            {
                if (_dataProviders == value) return;
                _dataProviders = value;
                RaisePropertyChanged("DataProviders");
            }
        }

        /// <summary>
        /// Gets or sets the collection of stock list providers
        /// </summary>
        public ObservableCollection<StockListProvider> StockListProviders
        {
            get { return _stockListProviders; }
            set
            {
                if (_stockListProviders == value) return;
                _stockListProviders = value;
                RaisePropertyChanged("StockListProviders");
            }
        }

        /// <summary>
        /// Gets the subscriptions
        /// </summary>
        public ObservableCollection<SubscriptionViewModel> Subscriptions
        {
            get { return _subscriptions; }
            set
            {
                if (_subscriptions == value) return;
                _subscriptions = value;
                RaisePropertyChanged("Subscriptions");
            }
        }

        /// <summary>
        /// Gets or sets the selected subscription
        /// </summary>
        public SubscriptionViewModel SelectedSubscription
        {
            get { return _selectedSubscription; }
            set
            {
                if (_selectedSubscription == value) return;
                _selectedSubscription = value;
                RaisePropertyChanged("SelectedSubscription");

                // load quote summaries
                if (_selectedSubscription != null)
                    _selectedSubscription.LoadQuoteSummariesAsync();
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets the command to load subscriptions
        /// </summary>
        public DelegateCommand LoadSubscriptionsCommand
        {
            get { return _loadSubscriptionsCommand ?? (_loadSubscriptionsCommand = new DelegateCommand(obj => LoadAsync())); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Asynchronously loads subscriptions
        /// </summary>
        public async void LoadAsync()
        {
            // set busy message
            BusyMessage = Resources.LoadingSubscriptionsMessage;

            // get the data service url
            var url = new Uri(ConfigurationManager.AppSettings["DataServiceUrl"]);

            // create repository
            var repository = new MarketDataRepository(url);

            // create collection into which to load data
            IEnumerable<DataProvider> dataProviders = null;
            IEnumerable<StockListProvider> stockListProviders = null;
            IEnumerable<Subscription> subscriptions = null;

            // load data async
            await Task.Factory.StartNew(() =>
                 {
                     dataProviders = repository.DataProviders.Execute();

                     stockListProviders = repository.StockListProviders.Execute();

                     subscriptions = repository.Subscriptions.Expand("DataProvider, StockListProvider").Execute();
                 });

            Application.Current.Dispatcher.Invoke(() =>
            {
                // set subscriptions
                DataProviders = new ObservableCollection<DataProvider>(dataProviders);
                StockListProviders = new ObservableCollection<StockListProvider>(stockListProviders);
                Subscriptions = new ObservableCollection<SubscriptionViewModel>(subscriptions.Select(s => new SubscriptionViewModel(s)));

                // clear busy message
                BusyMessage = string.Empty;
            });
        }

        #endregion
    }
}
