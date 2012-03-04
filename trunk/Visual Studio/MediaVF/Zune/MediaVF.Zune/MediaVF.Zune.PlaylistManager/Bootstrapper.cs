using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using System.Configuration;
using MediaVF.UI.Core;

namespace MediaVF.Zune.PlaylistManager
{
    class Bootstrapper : UnityBootstrapper
    {
        public Shell GetShell()
        {
            return (Shell)Shell;
        }

        public override void Run(bool runWithDefaultConfiguration)
        {
            base.Run(runWithDefaultConfiguration);
        }

        protected override DependencyObject CreateShell()
        {
            Shell shell = Container.Resolve<Shell>();
            Container.RegisterInstance<Shell>(shell);
            App.Current.MainWindow = shell;
            return shell;
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            DeepDirectoryModuleCatalog moduleCatalog = new DeepDirectoryModuleCatalog();
            moduleCatalog.ModulePath = ConfigurationManager.AppSettings["ModuleDirectory"];
            return moduleCatalog;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            // register the dialog service
            Container.RegisterType<IDialogService, DialogService>();

            AppDomain.CurrentDomain.AssemblyLoad +=
                (sender, e) =>
                {
                    // create uri to the Bindings resource dictionary
                    Uri packUri = new Uri(string.Format("pack://application:,,,/{0};component/Resources.xaml",
                        e.LoadedAssembly.GetName().Name));

                    ResourceDictionary bindingsDictionary = new ResourceDictionary() { Source = packUri };

                    // add bindings to resources
                    foreach (object key in bindingsDictionary.Keys)
                        Application.Current.Resources.Add(key, bindingsDictionary[key]);
                };
        }
    }
}
