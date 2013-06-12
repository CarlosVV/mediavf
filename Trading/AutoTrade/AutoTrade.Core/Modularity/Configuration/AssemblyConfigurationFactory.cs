using System.IO;
using System.Reflection;
using AutoTrade.Core.Modularity.Configuration.Xml;

namespace AutoTrade.Core.Modularity.Configuration
{
    class AssemblyConfigurationFactory : IAssemblyConfigurationFactory
    {
        #region Constants

        /// <summary>
        /// The prefix indicating a system assembly
        /// </summary>
        private const string SystemPrefix = "System";

        /// <summary>
        /// The format for creating the path to the config file for the module
        /// </summary>
        private const string ConfigFilePathFormat = "{0}.config";

        #endregion

        #region Methods

        /// <summary>
        /// Get the path to the config file
        /// </summary>
        protected static string GetConfigFilePath(Assembly assembly)
        {
            return string.Format(ConfigFilePathFormat, assembly.Location);
        }

        /// <summary>
        /// Creates the assembly configuration for an assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public AssemblyConfiguration CreateAssemblyConfiguration(Assembly assembly)
        {
            // skip system assemblies
            if (assembly.FullName.StartsWith(SystemPrefix) || assembly.IsDynamic)
                return null;

            // get the path to the config file
            var assemblyConfigPath = GetConfigFilePath(assembly);

            // check that a config file exists
            if (File.Exists(assemblyConfigPath))
            {
                // get xml from file
                var xml = File.ReadAllText(assemblyConfigPath);

                // check that xml is populated
                if (!string.IsNullOrWhiteSpace(xml))
                    return new AssemblyConfiguration(xml);
            }

            return null;
        }

        #endregion
    }
}