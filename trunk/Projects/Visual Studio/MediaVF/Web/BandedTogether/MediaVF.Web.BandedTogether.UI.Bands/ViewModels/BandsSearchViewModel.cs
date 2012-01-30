using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Events;

using MediaVF.Entities.ArtistTrack;
using MediaVF.UI.Core;

namespace MediaVF.Web.BandedTogether.UI.Bands.ViewModels
{
    public class BandsSearchViewModel : ContainerViewModel
    {
        IEventAggregator EventAggregator { get; set; }

        string _filterText;
        public string FilterText
        {
            get { return _filterText; }
            set
            {
                if (_filterText != value)
                {
                    _filterText = value;

                    RaisePropertyChanged("FilteredSortedBands");
                }
            }
        }

        ObservableCollection<Band> _bands;
        public ObservableCollection<Band> Bands
        {
            get
            {
                if (_bands == null)
                {
                    _bands = new ObservableCollection<Band>();
                    _bands.CollectionChanged += (sender, e) => RaisePropertyChanged("FilteredSortedBands");
                }
                return _bands;
            }
        }

        public IEnumerable<Band> FilteredSortedBands
        {
            get
            {
                IEnumerable<Band> bands = Bands.OrderBy(b => b.Name);

                if (!string.IsNullOrEmpty(FilterText))
                    bands = bands.Where(b => b.Name != null && b.Name.ToLower().Contains(FilterText.ToLower()));

                return bands;
            }
        }

        Band _selectedBand;
        public Band SelectedBand
        {
            get { return _selectedBand; }
            set
            {
                if (_selectedBand != value)
                {
                    _selectedBand = value;

                    RaisePropertyChanged("SelectedBand");
                }
            }
        }

        ICommand _filterCommand;
        public ICommand FilterCommand
        {
            get
            {
                if (_filterCommand == null)
                    _filterCommand = new DelegateCommand((obj) => FilterText = (string)obj);
                return _filterCommand;
            }
        }

        ICommand _addBandsCommand;
        public ICommand AddBandsCommand
        {
            get
            {
                if (_addBandsCommand == null)
                    _addBandsCommand = new DelegateCommand(obj =>
                    {
                        AddBandsViewModel addBandsVM = Container.Resolve<AddBandsViewModel>();
                        addBandsVM.Close += () => GetBands();

                        Container.Resolve<IDisplayService>().Display(addBandsVM);
                    });
                return _addBandsCommand;
            }
        }

        ICommand _bandSelectedCommand;
        public ICommand BandSelectedCommand
        {
            get
            {
                if (_bandSelectedCommand == null)
                    _bandSelectedCommand = new DelegateCommand(obj =>
                    {
                        BandDetailsViewModel bandDetailsVM = Container.Resolve<BandDetailsViewModel>();
                        bandDetailsVM.Band = (Band)obj;
                        Container.Resolve<IDisplayService>().Display(bandDetailsVM);
                    });

                return _bandSelectedCommand;
            }
        }

        public BandsSearchViewModel(IUnityContainer container, IEventAggregator eventAggregator)
            : base(container)
        {
            EventAggregator = eventAggregator;

            EventAggregator.GetEvent<CompositePresentationEvent<UIEventArgs<bool>>>().Subscribe(
                OnLoggedIn,
                ThreadOption.UIThread,
                false,
                FilterEvents);
        }

        public void OnLoggedIn(UIEventArgs<bool> args)
        {
            if (args.EventData)
                GetBands();
        }

        public bool FilterEvents(UIEventArgs<bool> args)
        {
            return args.EventID == "LoggedIn";
        }

        private void GetBands()
        {
            User currentUser = Container.Resolve<User>("CurrentUser");

            if (currentUser != null)
            {
                BandsServiceClient serviceClient = Container.Resolve<BandsServiceClient>();
                serviceClient.GetBandsForUser(currentUser.ID,
                    (bands) =>
                    {
                        // clear existing results
                        Bands.Clear();

                        // populate new results
                        bands.ForEach(band => Bands.Add(band));
                    });
            }
        }
    }
}
