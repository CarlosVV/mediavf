using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaVF.UI.Core;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;
using System.Collections.ObjectModel;

namespace MediaVF.Zune.PlaylistCreator.UI.ViewModels
{
    class PlaylistGenerationViewModel : ViewModelBase, IHeadered, ISelectable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the region manager
        /// </summary>
        IRegionManager RegionManager { get; set; }

        /// <summary>
        /// Gets or sets the header text
        /// </summary>
        public string Header { get { return "Generate"; } }

        /// <summary>
        /// Gets or sets flag indicating if the item is selected
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;

                    RaisePropertyChanged("IsSelected");
                }
            }
        }
        bool _isSelected;

        /// <summary>
        /// Gets the playlist generators list view model
        /// </summary>
        public PlaylistGeneratorsListViewModel PlaylistGeneratorsListVM
        {
            get { return _playlistGeneratorsListVM; }
            set
            {
                if (_playlistGeneratorsListVM != value)
                {
                    _playlistGeneratorsListVM = value;
                    if (_playlistGeneratorsListVM != null)
                    {
                        _playlistGeneratorsListVM.RequestOpenGenerators += OpenGenerators;
                        _playlistGeneratorsListVM.RequestCloseGenerator += CloseGenerators;
                    }

                    RaisePropertyChanged("PlaylistGeneratorsListVM");
                }
            }
        }
        PlaylistGeneratorsListViewModel _playlistGeneratorsListVM;

        /// <summary>
        /// Gets collection of open playlist generator view models
        /// </summary>
        public ObservableCollection<EditPlaylistGeneratorViewModel> OpenPlaylistGeneratorVMs
        {
            get
            {
                if (_openPlaylistGeneratorVMs == null)
                    _openPlaylistGeneratorVMs = new ObservableCollection<EditPlaylistGeneratorViewModel>();
                return _openPlaylistGeneratorVMs;
            }
        }
        ObservableCollection<EditPlaylistGeneratorViewModel> _openPlaylistGeneratorVMs;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates the view model with a container and region manager
        /// </summary>
        /// <param name="container"></param>
        /// <param name="regionManager"></param>
        public PlaylistGenerationViewModel(IUnityContainer container)
            : base(container)
        {
            PlaylistGeneratorsListVM = Container.Resolve<PlaylistGeneratorsListViewModel>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles adding of a generator to the playlist manager
        /// </summary>
        /// <param name="playlistGenerator"></param>
        public void OnGeneratorAdded(PlaylistGenerator playlistGenerator)
        {
            PlaylistGeneratorsListVM.AddGenerator(playlistGenerator);
        }

        /// <summary>
        /// Opens a collection of generators
        /// </summary>
        /// <param name="generators"></param>
        private void OpenGenerators(IEnumerable<PlaylistGenerator> generators)
        {
            if (generators != null && generators.Any())
            {
                foreach (PlaylistGenerator generator in generators)
                {
                    EditPlaylistGeneratorViewModel playlistGeneratorVM = OpenPlaylistGeneratorVMs.FirstOrDefault(vm => vm.Name == generator.Name);
                    if (playlistGeneratorVM == null)
                    {
                        playlistGeneratorVM =
                            Container.Resolve<EditPlaylistGeneratorViewModel>(generator.Name,
                                new ParameterOverride("playlistGenerator", generator));
                        playlistGeneratorVM.Close += () => OpenPlaylistGeneratorVMs.Remove(playlistGeneratorVM);
                        OpenPlaylistGeneratorVMs.Add(playlistGeneratorVM);
                        if (generators.First() == generator)
                            playlistGeneratorVM.IsSelected = true;
                    }
                    else
                        playlistGeneratorVM.IsSelected = true;
                }
            }
            else
            {
                // create the playlist generator
                EditPlaylistGeneratorViewModel playlistGeneratorVM = Container.Resolve<EditPlaylistGeneratorViewModel>();
                playlistGeneratorVM.IsSelected = true;
                playlistGeneratorVM.Name = "<New>";
                playlistGeneratorVM.Close += () => OpenPlaylistGeneratorVMs.Remove(playlistGeneratorVM);

                OpenPlaylistGeneratorVMs.Add(playlistGeneratorVM);
            }
        }

        /// <summary>
        /// Closes a collection of generators
        /// </summary>
        /// <param name="generators"></param>
        private void CloseGenerators(IEnumerable<PlaylistGenerator> generators)
        {
            if (generators != null && generators.Any())
            {
                IEnumerable<EditPlaylistGeneratorViewModel> removeVMs = OpenPlaylistGeneratorVMs.Where(vm => generators.Any(g => g.Name == vm.Name));
                foreach (EditPlaylistGeneratorViewModel playlistGeneratorVM in removeVMs)
                    OpenPlaylistGeneratorVMs.Remove(playlistGeneratorVM);
            }
            else // remove all
                OpenPlaylistGeneratorVMs.Clear();
        }

        #endregion
    }
}
