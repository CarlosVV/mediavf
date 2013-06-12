using AutoTrade.Core.Modularity.Configuration;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace AutoTrade.Core.Modularity
{
    public abstract class ConfigurableModule : IModule
    {
        #region Fields

        /// <summary>
        /// The unity container
        /// </summary>
        private readonly IUnityContainer _unityContainer;

        /// <summary>
        /// The assembly configuration manager
        /// </summary>
        private readonly IAssemblyConfigurationManager _assemblyConfigurationManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="ConfigurableModule"/>
        /// </summary>
        /// <param name="unityContainer"></param>
        /// <param name="assemblyConfigurationManager"></param>
        protected ConfigurableModule(IUnityContainer unityContainer, IAssemblyConfigurationManager assemblyConfigurationManager)
        {
            _assemblyConfigurationManager = assemblyConfigurationManager;
            _unityContainer = unityContainer;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the module from configuration
        /// </summary>
        public virtual void Initialize()
        {
            // get the configuration for the assembly
            var assemblyConfiguration =
                _assemblyConfigurationManager.GetAssemblyConfiguration(GetType().Assembly);

            // add registrations to container
            if (assemblyConfiguration != null)
                assemblyConfiguration.Registrations.AddRegistrationsToContainer(_unityContainer);
        }

        #endregion
    }
}
