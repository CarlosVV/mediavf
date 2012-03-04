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

namespace MediaVF.Web.ArtistTrack.UI.Artists.ViewModels
{
    public class ArtistsSearchViewModel : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the event aggregator for handling events
        /// </summary>
        IEventAggregator EventAggregator { get; set; }

        /// <summary>
        /// Gets the master list of artists
        /// </summary>
        ObservableCollection<Artist> _artists;
        public ObservableCollection<Artist> Artists
        {
            get
            {
                if (_artists == null)
                    _artists = new ObservableCollection<Artist>();
                return _artists;
            }
        }

        /// <summary>
        /// Gets the currently selected artist in the list
        /// </summary>
        Artist _selectedArtist;
        public Artist SelectedArtist
        {
            get { return _selectedArtist; }
            set
            {
                if (_selectedArtist != value)
                {
                    _selectedArtist = value;

                    RaisePropertyChanged("SelectedArtist");
                }
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets the command for handling selection of artists from the list
        /// </summary>
        ICommand _artistSelectedCommand;
        public ICommand ArtistSelectedCommand
        {
            get
            {
                if (_artistSelectedCommand == null)
                    _artistSelectedCommand = new DelegateCommand(obj =>
                    {
                        ArtistDetailsViewModel artistDetailsVM = Container.Resolve<ArtistDetailsViewModel>();
                        artistDetailsVM.Artist = (Artist)obj;
                        Container.Resolve<IDisplayService>().Display(artistDetailsVM);
                    });

                return _artistSelectedCommand;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a ArtistsSearchViewModel object
        /// </summary>
        /// <param name="container"></param>
        /// <param name="eventAggregator"></param>
        public ArtistsSearchViewModel(IUnityContainer container, IEventAggregator eventAggregator)
            : base(container)
        {
            EventAggregator = eventAggregator;

            EventAggregator.GetEvent<CompositePresentationEvent<UIEventArgs<bool>>>().Subscribe(
                OnLoggedIn,
                ThreadOption.UIThread,
                false,
                FilterEvents);
        }

        #endregion

        #region Event Handling

        /// <summary>
        /// Handles log in event
        /// </summary>
        /// <param name="args"></param>
        public void OnLoggedIn(UIEventArgs<bool> args)
        {
            if (args.EventData)
                GetArtists();
        }

        /// <summary>
        /// Filters for the log in event
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool FilterEvents(UIEventArgs<bool> args)
        {
            return args.EventID == "LoggedIn";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets artists
        /// </summary>
        private void GetArtists()
        {
            User currentUser = Container.Resolve<User>("CurrentUser");

            if (currentUser != null)
            {
                /*
                ArtistsServiceClient serviceClient = Container.Resolve<ArtistsServiceClient>();
                serviceClient.GetArtistsForUser(currentUser.ID,
                    (artists) =>
                    {*/
                // clear existing results
                        Artists.Clear();

                        // populate new results
                        //artists.ForEach(artist => Artists.Add(artist));

                        Artists.Add(new Artist() { Name = "October Tide" });
                        Artists.Add(new Artist() { Name = "The Devin Townsend Project" });
                        Artists.Add(new Artist() { Name = "Astomatous" });
                        Artists.Add(new Artist() { Name = "Carnifex" });
                        Artists.Add(new Artist() { Name = "Machine Head" });
                        Artists.Add(new Artist() { Name = "The Atlas Moth" });
                        Artists.Add(new Artist() { Name = "Norma Jean" });
                        Artists.Add(new Artist() { Name = "All Shall Perish" });
                        Artists.Add(new Artist() { Name = "All Else Failed" });
                        Artists.Add(new Artist() { Name = "All That Remains" });
                        Artists.Add(new Artist() { Name = "All Pigs Must Die" });
                    /*});
                 */
            }
        }

        #endregion
    }
}
