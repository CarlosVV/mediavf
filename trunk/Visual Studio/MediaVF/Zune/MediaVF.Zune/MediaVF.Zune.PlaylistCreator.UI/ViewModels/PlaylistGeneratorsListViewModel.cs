using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaVF.UI.Core;
using Microsoft.Practices.Unity;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Regions;

namespace MediaVF.Zune.PlaylistCreator.UI.ViewModels
{
    public class PlaylistGeneratorsListViewModel : ViewModelBase
    {
        #region Events

        public event Action<IEnumerable<PlaylistGenerator>> RequestOpenGenerators;

        private void RaiseOpenGenerator(IEnumerable<PlaylistGenerator> playlistGeneratorVMs)
        {
            if (RequestOpenGenerators != null)
                RequestOpenGenerators(playlistGeneratorVMs);
        }

        public event Action<IEnumerable<PlaylistGenerator>> RequestCloseGenerator;

        private void RaiseCloseGenerator(IEnumerable<PlaylistGenerator> playlistGeneratorVMs)
        {
            if (RequestCloseGenerator != null)
                RequestCloseGenerator(playlistGeneratorVMs);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets collection of playlist generators
        /// </summary>
        public ObservableCollection<PlaylistGeneratorViewModel> PlaylistGeneratorVMs
        {
            get
            {
                if (_playlistGeneratorVMs == null)
                    _playlistGeneratorVMs = new ObservableCollection<PlaylistGeneratorViewModel>();
                return _playlistGeneratorVMs;
            }
        }
        ObservableCollection<PlaylistGeneratorViewModel> _playlistGeneratorVMs;
        
        /// <summary>
        /// Gets or sets the selected playlist generator
        /// </summary>
        public IEnumerable<PlaylistGeneratorViewModel> SelectedPlaylistGenerators
        {
            get { return PlaylistGeneratorVMs.Where(pgvm => pgvm.IsSelected); }
        }

        #region Commands

        /// <summary>
        /// Gets command for adding a generator
        /// </summary>
        public DelegateCommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                    _addCommand = new DelegateCommand(obj => CreateNewGenerator(), this);
                return _addCommand;
            }
        }
        DelegateCommand _addCommand;

        /// <summary>
        /// Gets command for removing selected generators
        /// </summary>
        public DelegateCommand RemoveCommand
        {
            get
            {
                if (_removeCommand == null)
                    _removeCommand = new DelegateCommand(obj => RemoveGenerators(), this);
                return _removeCommand;
            }
        }
        DelegateCommand _removeCommand;

        /// <summary>
        /// Gets command for starting selected playlist generators
        /// </summary>
        public DelegateCommand StartCommand
        {
            get
            {
                if (_startCommand == null)
                    _startCommand = new DelegateCommand(obj => StartGenerators(), this);
                return _startCommand;
            }
        }
        DelegateCommand _startCommand;

        /// <summary>
        /// Gets command for stopping selected playlist generators
        /// </summary>
        public DelegateCommand StopCommand
        {
            get
            {
                if (_stopCommand == null)
                    _stopCommand = new DelegateCommand(obj => StopGenerators(), this);
                return _stopCommand;
            }
        }
        DelegateCommand _stopCommand;

        #endregion

        #endregion

        /// <summary>
        /// Instantiates the playlist generators list view model
        /// </summary>
        /// <param name="container"></param>
        /// <param name="regionManager"></param>
        public PlaylistGeneratorsListViewModel(IUnityContainer container)
            : base(container)
        {
            // get the playlist manager
            PlaylistManager playlistManager = Container.Resolve<PlaylistManager>();

            // loop through playlist generators and create view models
            foreach (PlaylistGenerator playlistGenerator in playlistManager.GetPlaylistGenerators())
                PlaylistGeneratorVMs.Add(
                    Container.Resolve<PlaylistGeneratorViewModel>(playlistGenerator.Name, new ParameterOverride("playlistGenerator", playlistGenerator)));
        }

        #region Methods

        /// <summary>
        /// Adds a new generator to the list
        /// </summary>
        public void CreateNewGenerator()
        {
            RaiseOpenGenerator(new PlaylistGenerator[] { });
        }

        /// <summary>
        /// Adds a generator to the list
        /// </summary>
        /// <param name="playlistGenerator"></param>
        public void AddGenerator(PlaylistGenerator playlistGenerator)
        {
            PlaylistGeneratorVMs.Add(new PlaylistGeneratorViewModel(Container, playlistGenerator));
        }

        /// <summary>
        /// Opens the selected generators
        /// </summary>
        public void OpenGenerators()
        {
            if (SelectedPlaylistGenerators.Any())
                RaiseOpenGenerator(SelectedPlaylistGenerators.Select(pgvm => pgvm.PlaylistGenerator));
        }

        /// <summary>
        /// Removes selected generators from the list
        /// </summary>
        public void RemoveGenerators()
        {
            // raise close to ensure all generators are closed
            //RaiseCloseGenerator(SelectedPlaylistGenerators);

            foreach (PlaylistGeneratorViewModel playlistGeneratorVM in SelectedPlaylistGenerators)
            {
                // stop the generator
                if (playlistGeneratorVM.Status == PlaylistGeneratorViewModel.StatusType.Running)
                    playlistGeneratorVM.StopGenerator();

                // remove the generator from the list
                PlaylistGeneratorVMs.Remove(playlistGeneratorVM);
            }
        }

        /// <summary>
        /// Starts the selected generators
        /// </summary>
        public void StartGenerators()
        {
            foreach (PlaylistGeneratorViewModel playlistGeneratorVM in SelectedPlaylistGenerators)
            {
                // stop the generator
                if (playlistGeneratorVM.Status == PlaylistGeneratorViewModel.StatusType.Stopped)
                    playlistGeneratorVM.StartGenerator();
            }
        }

        /// <summary>
        /// Starts the selected generators
        /// </summary>
        public void StopGenerators()
        {
            foreach (PlaylistGeneratorViewModel playlistGeneratorVM in SelectedPlaylistGenerators)
            {
                // stop the generator
                if (playlistGeneratorVM.Status == PlaylistGeneratorViewModel.StatusType.Running)
                    playlistGeneratorVM.StopGenerator();
            }
        }

        #endregion
    }
}
