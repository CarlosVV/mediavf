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

namespace MediaVF.Web.ArtistTrack.UI.Artists.ViewModels
{
    public class AddArtistsViewModel : ViewModelBase, IHeadered
    {
        #region ArtistItem

        public class ArtistItem : ViewModelBase
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

            string _artistName;
            public string ArtistName
            {
                get { return _artistName; }
                set
                {
                    if (_artistName != value)
                    {
                        _artistName = value;

                        RaisePropertyChanged("ArtistName");
                    }
                }
            }

            public ArtistItem(IUnityContainer container)
                : base(container) { }
        }

        #endregion

        #region Properties

        IDisplayService DisplayService { get; set; }

        public string Header { get { return "Artists"; } }

        ObservableCollection<ArtistItem> _artists;
        public ObservableCollection<ArtistItem> Artists
        {
            get
            {
                if (_artists == null)
                    _artists = new ObservableCollection<ArtistItem>();
                return _artists;
            }
        }

        DelegateCommand _browseCommand;
        public DelegateCommand BrowseCommand
        {
            get
            {
                if (_browseCommand == null)
                    _browseCommand = new DelegateCommand((obj) =>
                        DisplayService.ShowPopup(ArtistsModule.DirectoryBrowserPopupKey,
                            ArtistsModule.DirectoryBrowserArtistsFoundCallbackKey,
                            (values) =>
                            {
                                if (values != null && values.Length > 0)
                                    PopulateArtists(values);
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

        DelegateCommand _addArtistsCommand;
        public DelegateCommand AddArtistsCommand
        {
            get
            {
                if (_addArtistsCommand == null)
                    _addArtistsCommand = new DelegateCommand((obj) => AddArtists());
                return _addArtistsCommand;
            }
        }

        #endregion

        #region Constructor

        public AddArtistsViewModel(IUnityContainer container, IDisplayService displayService)
            : base(container)
        {
            DisplayService = displayService;
        }

        #endregion

        #region Methods

        public void PopulateArtists(string artistsText)
        {
            Artists.Clear();

            if (!string.IsNullOrEmpty(artistsText))
            {
                artistsText.Split('|').ToList().ForEach(artist =>
                {
                    if (!string.IsNullOrEmpty(artist))
                        Artists.Add(new ArtistItem(Container) { Add = true, ArtistName = artist });
                });
            }
        }

        public void ToggleAll(bool value)
        {
            if (Artists != null)
                foreach (ArtistItem artistItem in Artists)
                    artistItem.Add = value;
        }

        public void AddArtists()
        {
            User currentUser = Container.Resolve<User>("CurrentUser");

            ArtistsServiceClient serviceClient = Container.Resolve<ArtistsServiceClient>();
            serviceClient.AddArtistsForUser(currentUser.ID,
                Artists.Where(b => b.Add).Select(b => new Artist() { Name = b.ArtistName }),
                () => RaiseClose());
        }

        #endregion
    }
}
