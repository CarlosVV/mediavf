using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using AutoTrade.Core.Modularity.Configuration.Xml;

namespace AutoTrade.Core.Modularity.Configuration
{
    public class AssemblyConfigurationManager : IAssemblyConfigurationManager
    {
        #region Fields

        /// <summary>
        /// The assembly configurations, by assembly
        /// </summary>
        private static readonly Dictionary<Assembly, AssemblyConfiguration> AssemblyConfigurations =
            new Dictionary<Assembly, AssemblyConfiguration>();

        /// <summary>
        /// The factory for creating assembly configuration
        /// </summary>
        private readonly IAssemblyConfigurationFactory _configurationFactory;

        /// <summary>
        /// The list of exceptions encountered when handling loading of configuration for assemblies
        /// </summary>
        private readonly List<Exception> _configurationLoadExceptions = new List<Exception>(); 

        /// <summary>
        /// Flag indicating if the AssemblyLoad event is attached
        /// </summary>
        private static bool _isAssemblyLoadHandled;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates an <see cref="AssemblyConfigurationManager"/> by attaching to the AssemblyLoad event
        /// </summary>
        public AssemblyConfigurationManager(IAssemblyConfigurationFactory configurationFactory)
        {
            // set config factory
            _configurationFactory = configurationFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Attaches events
        /// </summary>
        public void StartMonitoringLoadedAssemblies()
        {
            // check if already attached
            if (_isAssemblyLoadHandled) return;

            // attach to the AssemblyLoad event
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomainOnAssemblyLoad;

            // set flag indicating attached
            _isAssemblyLoadHandled = true;   
        }

        /// <summary>
        /// Gets the configuration for an assembly
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public AssemblyConfiguration GetAssemblyConfiguration(Type type)
        {
            return GetAssemblyConfiguration(type.Assembly);
        }

        /// <summary>
        /// Gets the configuration for an assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public AssemblyConfiguration GetAssemblyConfiguration(Assembly assembly)
        {
            // check if the assembly has a configuration
            return AssemblyConfigurations.ContainsKey(assembly) ? AssemblyConfigurations[assembly] : null;
        }

        /// <summary>
        /// Throws exceptions encountered while trying to load assemblies, if any
        /// </summary>
        public void ThrowIfAnyConfigurationErrors()
        {
            if (_configurationLoadExceptions.Count > 0)
                throw new AggregateException(_configurationLoadExceptions);
        }

        /// <summary>
        /// Handles loading of an assembly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CurrentDomainOnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            try
            {
                // try to create the configuration for the assembly
                var assemblyConfiguration = _configurationFactory.CreateAssemblyConfiguration(args.LoadedAssembly);

                // if configuration was created, map it to the assembly
                if (assemblyConfiguration == null) return;

                // if the assembly has not been added to the collection, add it
                if (!AssemblyConfigurations.ContainsKey(args.LoadedAssembly))
                    AssemblyConfigurations.Add(args.LoadedAssembly, null);

                AssemblyConfigurations[args.LoadedAssembly] = assemblyConfiguration;
            }
            catch (Exception ex)
            {
                _configurationLoadExceptions.Add(ex);
            }
        }

        #endregion
    }
}
