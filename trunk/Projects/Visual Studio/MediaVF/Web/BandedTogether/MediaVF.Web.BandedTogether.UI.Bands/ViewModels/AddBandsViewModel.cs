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

using MediaVF.UI.Core;
using Microsoft.Practices.Unity;
using MediaVF.Entities.ArtistTrack;

namespace MediaVF.Web.BandedTogether.UI.Bands.ViewModels
{
    public class AddBandsViewModel : ContainerViewModel, IHeadered
    {
        #region BandItem

        public class BandItem : ContainerViewModel
        {
            bool _add;
            public bool Add
            {
                get { return _add; }
                set
                {
                    if (_add != value)
                    {
                        _add = value;

                        RaisePropertyChanged("Add");
                    }
                }
            }

            string _bandName;
            public string BandName
            {
                get { return _bandName; }
                set
                {
                    if (_bandName != value)
                    {
                        _bandName = value;

                        RaisePropertyChanged("BandName");
                    }
                }
            }

            public BandItem(IUnityContainer container)
                : base(container) { }
        }

        #endregion

        #region Properties

        IDisplayService DisplayService { get; set; }

        public string Header { get { return "Bands"; } }

        ObservableCollection<BandItem> _bands;
        public ObservableCollection<BandItem> Bands
        {
            get
            {
                if (_bands == null)
                    _bands = new ObservableCollection<BandItem>();
                return _bands;
            }
        }

        DelegateCommand _browseCommand;
        public DelegateCommand BrowseCommand
        {
            get
            {
                if (_browseCommand == null)
                    _browseCommand = new DelegateCommand((obj) =>
                        DisplayService.ShowPopup(BandsModule.DirectorBrowserPopupKey,
                            BandsModule.DirectoryBrowserBandsFoundCallbackKey,
                            (values) =>
                            {
                                if (values != null && values.Length > 0)
                                    PopulateBands(values);
                            }));
                return _browseCommand;
            }
        }

        DelegateCommand _toggleAllCommand;
        public DelegateCommand ToggleAllCommand
        {
            get
            {
                if (_toggleAllCommand == null)
                    _toggleAllCommand = new DelegateCommand((obj) => ToggleAll((bool)obj));
                return _toggleAllCommand;
            }
        }

        DelegateCommand _addBandsCommand;
        public DelegateCommand AddBandsCommand
        {
            get
            {
                if (_addBandsCommand == null)
                    _addBandsCommand = new DelegateCommand((obj) => AddBands());
                return _addBandsCommand;
            }
        }

        #endregion

        #region Constructor

        public AddBandsViewModel(IUnityContainer container, IDisplayService displayService)
            : base(container)
        {
            DisplayService = displayService;
        }

        #endregion

        #region Methods

        public void PopulateBands(string bandsText)
        {
            Bands.Clear();

            if (!string.IsNullOrEmpty(bandsText))
            {
                bandsText.Split('|').ToList().ForEach(band =>
                {
                    if (!string.IsNullOrEmpty(band))
                        Bands.Add(new BandItem(Container) { Add = true, BandName = band });
                });
            }
        }

        public void ToggleAll(bool value)
        {
            if (Bands != null)
                foreach (BandItem bandItem in Bands)
                    bandItem.Add = value;
        }

        public void AddBands()
        {
            User currentUser = Container.Resolve<User>("CurrentUser");

            BandsServiceClient serviceClient = Container.Resolve<BandsServiceClient>();
            serviceClient.AddBandsForUser(currentUser.ID,
                Bands.Where(b => b.Add).Select(b => new Band() { Name = b.BandName }),
                () => RaiseClose());
        }

        #endregion
    }
}
