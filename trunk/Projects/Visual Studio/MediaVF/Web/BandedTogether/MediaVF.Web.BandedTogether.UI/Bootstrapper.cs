using System.Windows;

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

using MediaVF.UI.Core;
using MediaVF.Web.BandedTogether.UI.Admin;
using MediaVF.Web.BandedTogether.UI.Bands;

namespace MediaVF.Web.BandedTogether.UI
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
            moduleCatalog.AddModule(typeof(BandsModule));
            return moduleCatalog;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<IInvocableService, BandedTogetherServiceClient>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IDisplayService, DisplayService>(new ContainerControlledLifetimeManager());

            Container.RegisterInstance<IBrowserMessageManager>(Container.Resolve<BrowserManager>(), new ContainerControlledLifetimeManager());

        }
    }
}
