using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Unity;

using MediaVF.UI.Core;

namespace MediaVF.Zune.PlaylistCreator.UI.ViewModels
{
    public class EditPlaylistGeneratorViewModel : PlaylistGeneratorViewModel, IHeadered
    {
        #region Properties

        /// <summary>
        /// Gets or sets the header text
        /// </summary>
        public string Header { get { return Name; } }

        #region Commands

        /// <summary>
        /// Gets the command for saving changes by creating a new generator from the current values
        /// </summary>
        public DelegateCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new DelegateCommand(obj => CreateGenerator(), this);
                return _saveCommand;
            }
        }
        DelegateCommand _saveCommand;

        /// <summary>
        /// Gets the command for canceling changes to the generator
        /// </summary>
        public DelegateCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new DelegateCommand(obj => SetValuesFromGenerator(), this);
                return _cancelCommand;
            }
        }
        DelegateCommand _cancelCommand;

        public DelegateCommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new DelegateCommand(obj =>
                        RaiseClose(),
                        this);
                return _closeCommand;
            }
        }
        DelegateCommand _closeCommand;

        /// <summary>
        /// Gets the command for browsing folders for media
        /// </summary>
        public DelegateCommand BrowseMediaFolderCommand
        {
            get
            {
                if (_browseMediaFolderCommand == null)
                    _browseMediaFolderCommand =
                        new DelegateCommand(obj =>
                            MediaFolder = Container.Resolve<IDialogService>().ShowFolderDialog(
                                "Please select a folder to watch for new media.",
                                Environment.SpecialFolder.Desktop,
                                false),
                            this);
                return _browseMediaFolderCommand;
            }
        }
        DelegateCommand _browseMediaFolderCommand;

        /// <summary>
        /// Gets the command for browsing folders for media
        /// </summary>
        public DelegateCommand BrowseOutputFolderCommand
        {
            get
            {
                if (_browseOutputFolderCommand == null)
                    _browseOutputFolderCommand =
                        new DelegateCommand(obj =>
                            OutputFolder = Container.Resolve<IDialogService>().ShowFolderDialog(
                                "Please select the folder to which to save playlists.",
                                Environment.SpecialFolder.Desktop,
                                true),
                            this);
                return _browseOutputFolderCommand;
            }
        }
        DelegateCommand _browseOutputFolderCommand;

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates the view model
        /// </summary>
        /// <param name="container"></param>
        public EditPlaylistGeneratorViewModel(IUnityContainer container)
            : base(container, null)
        {
            Name = "<New>";
        }

        /// <summary>
        /// Instantiates the view model with an existing playlist generator
        /// </summary>
        /// <param name="container"></param>
        /// <param name="playlistGenerator"></param>
        public EditPlaylistGeneratorViewModel(IUnityContainer container, PlaylistGenerator playlistGenerator)
            : base(container, playlistGenerator)
        {
            // set values on generator
            SetValuesFromGenerator();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the playlist generator
        /// </summary>
        private void CreateGenerator()
        {
            // get the playlist manager
            PlaylistManager playlistManager = Container.Resolve<PlaylistManager>();

            // remove the old
            if (PlaylistGenerator != null)
                playlistManager.RemovePlaylistGenerator(PlaylistGenerator.Name);

            // add playlist generator
            PlaylistGenerator = playlistManager.AddPlaylistGenerator(Name,
                MediaFolder,
                OutputFolder,
                new TimeSpan(0, (int)ScanInterval, 0),
                new TimeSpan(0, (int)PlaylistCreationInterval, 0));

            // indicate that the status has changed
            RaiseStatusChanged();
        }

        #endregion
    }
}
