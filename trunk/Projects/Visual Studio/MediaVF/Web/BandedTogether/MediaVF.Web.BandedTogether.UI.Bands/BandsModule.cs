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

using MediaVF.Web.BandedTogether.UI.Bands.Views;
using MediaVF.UI.Core;

namespace MediaVF.Web.BandedTogether.UI.Bands
{
    public class BandsModule : IModule
    {
        public const string DirectorBrowserPopupKey = "DirectoryBrowser";
        public const string DirectoryBrowserBandsFoundCallbackKey = "BandsFound";

        IUnityContainer Container { get; set; }

        IRegionManager RegionManager { get; set; }

        public BandsModule(IUnityContainer container, IRegionManager regionManager)
        {
            Container = container;
            RegionManager = regionManager;
        }

        public void Initialize()
        {
            Container.RegisterInstance<BandsModule>(this);

            // register views with region
            RegionManager.RegisterViewWithRegion("BandsSearchRegion", typeof(BandsSearchView));
            RegionManager.RegisterViewWithRegion("BandDetailsRegion", typeof(BandDetailsView));

            // get popup manager and add popup info for directory browser
            PopupManager popupManager = Container.Resolve<PopupManager>();
            Container.RegisterInstance<IBrowserMessageManager>(BandsModule.DirectorBrowserPopupKey, popupManager);
            popupManager.AddPopup(BandsModule.DirectorBrowserPopupKey,
                "DirectoryBrowserApplet.aspx",
                false,
                false,
                false,
                false,
                false,
                520,
                320);
        }
    }
}
