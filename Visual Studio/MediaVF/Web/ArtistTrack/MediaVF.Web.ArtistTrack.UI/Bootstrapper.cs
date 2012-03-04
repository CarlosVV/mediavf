using System.Windows;

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

using MediaVF.UI.Core;
using MediaVF.Web.ArtistTrack.UI.Admin;
using MediaVF.Web.ArtistTrack.UI.Artists;

namespace MediaVF.Web.ArtistTrack.UI
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            Shell shell = Container.Resolve<Shell>();
            App.Current.RootVisual = shell;
            return shell;
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            ModuleCatalog moduleCatalog = new ModuleCatalog();
            moduleCatalog.AddModule(typeof(AdminModule));
            moduleCatalog.AddModule(typeof(ArtistsModule));
            return moduleCatalog;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<IInvocableService, ArtistTrackServiceClient>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IDisplayService, DisplayService>(new ContainerControlledLifetimeManager());

            Container.RegisterInstance<IBrowserMessageManager>(Container.Resolve<BrowserManager>(), new ContainerControlledLifetimeManager());

        }
    }
}
