using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;

using MediaVF.Web.ArtistTrack.UI.Artists.Views;
using MediaVF.UI.Core;
using Microsoft.Practices.Prism.Events;

namespace MediaVF.Web.ArtistTrack.UI.Artists
{
    public class ArtistsModule : IModule
    {
        public const string DirectoryBrowserPopupKey = "DirectoryBrowser";
        public const string DirectoryBrowserArtistsFoundCallbackKey = "ArtistsFound";

        IUnityContainer Container { get; set; }

        IRegionManager RegionManager { get; set; }

        IEventAggregator EventAggregator { get; set; }

        public ArtistsModule(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            Container = container;
            RegionManager = regionManager;
            EventAggregator = eventAggregator;
        }

        public void Initialize()
        {
            Container.RegisterInstance<ArtistsModule>(this);

            // register views with region
            RegionManager.RegisterViewWithRegion("ArtistsSearchRegion", typeof(ArtistsSearchView));
            RegionManager.RegisterViewWithRegion("ArtistDetailsRegion", typeof(ArtistDetailsView));

            // get popup manager and add popup info for directory browser
            PopupManager popupManager = Container.Resolve<PopupManager>();
            Container.RegisterInstance<IBrowserMessageManager>(ArtistsModule.DirectoryBrowserPopupKey, popupManager);
            popupManager.AddPopup(ArtistsModule.DirectoryBrowserPopupKey,
                "DirectoryBrowserApplet.aspx",
                false,
                false,
                false,
                false,
                false,
                520,
                320);

            /*EventAggregator.GetEvent<CompositePresentationEvent<UIEventArgs<bool>>>().Subscribe(
                args => ,
                ThreadOption.UIThread,
                false,
                args => args.EventID == "Registered");*/
        }
    }
}
