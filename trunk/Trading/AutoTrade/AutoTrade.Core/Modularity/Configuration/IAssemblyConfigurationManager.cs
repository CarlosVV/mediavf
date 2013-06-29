using System;
using System.Reflection;
using AutoTrade.Core.Modularity.Configuration.Xml;

namespace AutoTrade.Core.Modularity.Configuration
{
    public interface IAssemblyConfigurationManager
    {
        /// <summary>
        /// Starts monitoring loaded assemblies for configuration
        /// </summary>
        void StartMonitoringLoadedAssemblies();

        /// <summary>
        /// Gets the configuration for the aseembly of a type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        AssemblyConfiguration GetAssemblyConfiguration(Type type);

        /// <summary>
        /// Gets the configuration for an assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        AssemblyConfiguration GetAssemblyConfiguration(Assembly assembly);

        /// <summary>
        /// Throws an exception if any assembly configuration errors occurred
        /// </summary>
        void ThrowIfAnyConfigurationErrors();
    }
}