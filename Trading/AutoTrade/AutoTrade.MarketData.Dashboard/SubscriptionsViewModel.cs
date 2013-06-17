using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using AutoTrade.Core.UI;
using AutoTrade.MarketData.Dashboard.DataService;

namespace AutoTrade.MarketData.Dashboard
{
    class SubscriptionsViewModel : ViewModelBase
    {
        private ObservableCollection<Subscription> _subscriptions; 

        private DelegateCommand _loadSubscriptionsCommand;

        /// <summary>
        /// Gets the subscriptions
        /// </summary>
        public ObservableCollection<Subscription> Subscriptions
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
        /// Gets the command to load subscriptions
        /// </summary>
        public DelegateCommand LoadSubscriptionsCommand
        {
            get { return _loadSubscriptionsCommand ?? (_loadSubscriptionsCommand = new DelegateCommand(obj => LoadAsync())); }
        }

        /// <summary>
        /// Asynchronously loads subscriptions
        /// </summary>
        public async void LoadAsync()
        {
            IEnumerable<Subscription> subscriptions;
            using (var dataService = new MarketDataServiceClient())
            {
                // create collection of subscriptions from collection returned from the server
                subscriptions = new ObservableCollection<Subscription>(await dataService.GetSubscriptionsAsync());
            }

            Application.Current.Dispatcher.Invoke(() => Subscriptions = new ObservableCollection<Subscription>(subscriptions));
        }
    }
}
