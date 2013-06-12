using System.Reflection;
using AutoTrade.Core.Modularity.Configuration.Xml;

namespace AutoTrade.Core.Modularity.Configuration
{
    public interface IAssemblyConfigurationFactory
    {
        /// <summary>
        /// Creates the assembly configuration for an assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        AssemblyConfiguration CreateAssemblyConfiguration(Assembly assembly);
    }
}
