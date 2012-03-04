using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

using log4net;

using MediaVF.Common.Modularity;

using MediaVF.Services.Core.Components;
using MediaVF.Services.Core.Configuration;
using MediaVF.Services.Core.Data;
using MediaVF.Services.Core.Logging;
using MediaVF.Services.Core.Modularity;
using MediaVF.Services.Core.Properties;

namespace MediaVF.Services.Core
{

    public class ServiceBootstrapper<T> : ServiceBootstrapper where T : DependencyObject, IShell
    {
        protected override DependencyObject CreateShell()
        {
            Container.RegisterType<IShell, T>();

            IShell shell = Container.Resolve<IShell>();
            shell.Initialize();

            return shell as DependencyObject;
        }
    }
    /// <summary>
    /// Service bootstrapper
    /// </summary>
    public class ServiceBootstrapper : Bootstrapper
    {
        #region Properties

        /// <summary>
        /// Gets the default <see cref="IUnityContainer"/> for the application.
        /// </summary>
        /// <value>The default <see cref="IUnityContainer"/> instance.</value>
        [CLSCompliant(false)]
        public IUnityContainer Container { get; protected set; }

        #endregion Properties

        #region Bootstrapper Implementation

        /// <summary>
        /// Runs the bootstrapper
        /// </summary>
        /// <param name="runWithDefaultConfiguration"></param>
        public override void Run(bool runWithDefaultConfiguration)
        {
            // create unity container
            Container = new UnityContainer();

            // create log
            Logger = CreateLogger();

            try
            {
                // load configuration
                LoadConfiguration();

                // create module catalog
                ModuleCatalog = CreateModuleCatalog();

                // configure module catalog
                ConfigureModuleCatalog();

                // configure unity container
                ConfigureContainer();

                AppDomain.CurrentDomain.AssemblyLoad += ((sender, e) =>
                    {
                        Assembly assembly = e.LoadedAssembly;
                    });

                // load the modules for the service
                Container.Resolve<IModuleManager>().Run();

                Shell = CreateShell();

                // run service components
                Container.Resolve<IServiceComponentManager>().RunComponents();
            }
            catch (Exception ex)
            {
                ((IComboLog)Logger).Fatal("Failed to run bootstrapper.", ex);
            }
        }

        /// <summary>
        /// Configures the service locator
        /// </summary>
        protected override void ConfigureServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => Container.Resolve<IServiceLocator>());
        }

        #endregion Bootstrapper Implementation

        #region Logger

        /// <summary>
        /// Creates a log that implements both the ILoggerFacade (MS) and ILog (log4net) interfaces
        /// </summary>
        /// <returns></returns>
        protected override ILoggerFacade CreateLogger()
        {
            try
            {
                ILoggerFacade loggerFacade = new NetLogger();

                Container.RegisterInstance<ILoggerFacade>(loggerFacade);
                Container.RegisterInstance<IComboLog>((IComboLog)loggerFacade);

                return loggerFacade;
            }
            catch (Exception ex)
            {
                // log in the event log that the log was not created
                EventLog appEventLog = new EventLog(Resources.ApplicationEventLogName, Environment.MachineName);
                appEventLog.WriteEntry(Resources.LogFailureMessage + Environment.NewLine + ex.ToString(), EventLogEntryType.Error);

                throw ex;
            }
        }

        #endregion Logger

        #region ModuleCatalog

        /// <summary>
        /// Resolve module catalog from unity configuration
        /// </summary>
        /// <returns></returns>
        protected override IModuleCatalog CreateModuleCatalog()
        {
            //return new DirectoryModuleCatalog() { ModulePath = Path.GetFullPath("Components") };
            return new DatabaseModuleCatalog(Container.Resolve<IServiceConfigManager>());
        }

        protected override void ConfigureModuleCatalog()
        {
        }

        #endregion ModuleCatalog

        #region UnityContainer

        /// <summary>
        /// Configure the Unity container by registering the necessary types
        /// </summary>
        protected void ConfigureContainer()
        {
            // register instances of types that have already been created
            Container.RegisterInstance<ILoggerFacade>(Logger);
            Container.RegisterInstance<IComboLog>((IComboLog)Logger);
            Container.RegisterInstance<IModuleCatalog>(ModuleCatalog);

            Container.RegisterInstance<IServiceLocator>(new UnityServiceLocatorAdapter(Container), new ContainerControlledLifetimeManager());
            ServiceLocator.SetLocatorProvider(() => Container.Resolve<IServiceLocator>());

            // register container controlled types
            Container.RegisterType<IModuleInitializer, ModuleInitializer>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IModuleManager, ModuleManager>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());

            // register the service component manager
            Container.RegisterType<IServiceComponentManager, ServiceComponentManager>(new ContainerControlledLifetimeManager());

            // register data manager to resolve to a new instance for all child containers
            Container.RegisterType<IDataManager, DataManager>(new HierarchicalLifetimeManager());

            // check for registered module load listeners
            Container.ConfigureModuleLoadListeners();
        }

        #endregion UnityContainer

        #region Configuration

        protected void LoadConfiguration()
        {
            try
            {
                Container.RegisterType<IServiceConfigManager, ServiceConfigManager>(new ContainerControlledLifetimeManager());

                Container.Resolve<IServiceConfigManager>().Load();
            }
            catch (Exception ex)
            {
                ((IComboLog)Logger).Fatal("Error loading configuration.", ex);
                throw ex;
            }
        }

        #endregion Configuration

        protected override DependencyObject CreateShell()
        {
            return null;
        }
    }
}
