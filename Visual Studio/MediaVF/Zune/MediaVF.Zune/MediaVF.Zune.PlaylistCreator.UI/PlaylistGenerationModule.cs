using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System.Configuration;
using System.Windows;
using MediaVF.Zune.PlaylistCreator.UI.ViewModels;

namespace MediaVF.Zune.PlaylistCreator.UI
{
    [Module(ModuleName = "PlaylistGenerationModule", OnDemand = false)]
    public class PlaylistGenerationModule : IModule
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unity container
        /// </summary>
        IUnityContainer Container { get; set; }

        /// <summary>
        /// Gets or sets the region manager
        /// </summary>
        IRegionManager RegionManager { get; set; }

        /// <summary>
        /// Gets the location of the playlist generators XML file
        /// </summary>
        string PlaylistGeneratorsXMLFile
        {
            get
            {
                if (string.IsNullOrEmpty(_playlistGeneratorsXMLFile))
                    _playlistGeneratorsXMLFile = ConfigurationManager.AppSettings["PlaylistGeneratorsXML"];
                return _playlistGeneratorsXMLFile;
            }
        }
        string _playlistGeneratorsXMLFile;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates the module with a container and region manager
        /// </summary>
        /// <param name="container"></param>
        /// <param name="regionManager"></param>
        public PlaylistGenerationModule(IUnityContainer container, IRegionManager regionManager)
        {
            Container = container;
            RegionManager = regionManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the playlist generation module
        /// </summary>
        public void Initialize()
        {
            // create playlist generation manager, load it and register with container
            PlaylistManager playlistGenerationManager = Container.Resolve<PlaylistManager>();
            playlistGenerationManager.Load(PlaylistGeneratorsXMLFile);
            Container.RegisterInstance<PlaylistManager>(playlistGenerationManager);

            // save the generators on exit
            Application.Current.Exit +=
                (sender, e) =>
                {
                    try
                    {
                        playlistGenerationManager.Save(PlaylistGeneratorsXMLFile);
                    }
                    catch { }
                };

            // create playlist generation view model
            PlaylistGenerationViewModel playlistGenerationVM = Container.Resolve<PlaylistGenerationViewModel>();
            playlistGenerationManager.GeneratorAdded += playlistGenerationVM.OnGeneratorAdded;

            // set selected if first tab in tabs region
            if (!RegionManager.Regions["TabsRegion"].Views.Any())
                playlistGenerationVM.IsSelected = true;

            // register view
            RegionManager.RegisterViewWithRegion("TabsRegion", () => playlistGenerationVM);
        }

        #endregion
    }
}
