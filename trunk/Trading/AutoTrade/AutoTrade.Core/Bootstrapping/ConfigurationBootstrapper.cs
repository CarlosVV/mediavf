using System;
using System.Configuration;
using AutoTrade.Core.Properties;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using log4net;

namespace AutoTrade.Core.Bootstrapping
{
    public class ConfigurationBootstrapper<T>
    {
        #region Properties

        /// <summary>
        /// The unity container
        /// </summary>
        protected IUnityContainer Container { get; private set; }

        /// <summary>
        /// The logger
        /// </summary>
        protected ILog Logger { get; private set; }

        /// <summary>
        /// The module catalog
        /// </summary>
        protected IModuleCatalog ModuleCatalog { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the logger
        /// </summary>
        /// <returns></returns>
        protected virtual ILog CreateLogger()
        {
            return LogManager.GetLogger(typeof(T));
        }

        /// <summary>
        /// Creates the dependency injection container for the application
        /// </summary>
        /// <returns></returns>
        protected virtual IUnityContainer CreateContainer()
        {
            // get unity config section
            var configSection = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            if (configSection == null)
                throw new Exception("Unity section not found.");

            // configure container
            return configSection.Configure(new UnityContainer());
        }

        /// <summary>
        /// Creates the module catalog for loading modules
        /// </summary>
        /// <returns></returns>
        protected virtual IModuleCatalog CreateModuleCatalog()
        {
            // resolve module catalog
            var moduleCatalog = Container.Resolve<IModuleCatalog>();

            // load the catalog, if possible
            var catalog = moduleCatalog as ModuleCatalog;
            if (catalog != null)
                catalog.Load();

            // return catalog
            return moduleCatalog;
        }

        /// <summary>
        /// Initializes modules
        /// </summary>
        protected virtual void InitializeModules()
        {
            // get the module manager and run it
            var manager = Container.Resolve<IModuleManager>();
            if (manager != null)
                manager.Run();
        }

        /// <summary>
        /// Runs the configuration bootstrapper
        /// </summary>
        public virtual IDisposable Run()
        {
            // create logger
            Logger = CreateLogger();
            if (Logger == null)
                throw new InvalidOperationException(Resources.NullLoggerMessage);
            Logger.Debug(Resources.LoggerCreatedMessage);

            // create container
            Container = CreateContainer();
            if (Container == null)
                throw new InvalidOperationException(Resources.NullContainerMessage);
            Logger.Debug(Resources.ContainerCreatedMessage);

            // registers the logger for access throughout the application
            Container.RegisterInstance(Logger);

            // create module catalog
            ModuleCatalog = CreateModuleCatalog();
            if (ModuleCatalog == null)
                throw new InvalidOperationException(Resources.NullModuleCatalogMessage);
            Logger.Debug(Resources.ModuleCatalogCreatedMessage);

            // initialize modules
            if (Container.IsRegistered<IModuleManager>())
            {
                Logger.Debug(Resources.InitializingModulesMessage);
                InitializeModules();
            }

            // log completion
            Logger.Debug(Resources.BootstrapperCompletedMessage);

            return Container;
        }

        #endregion
    }
}
