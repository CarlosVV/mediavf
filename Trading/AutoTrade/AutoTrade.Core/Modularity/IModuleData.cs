using System.Collections.Generic;

namespace AutoTrade.Core.Modularity
{
    public interface IModuleData
    {
        /// <summary>
        /// Gets the path to the assembly in which the module is located
        /// </summary>
        string AssemblyPath { get; }

        /// <summary>
        /// Gets the module's type
        /// </summary>
        string ModuleType { get; }

        /// <summary>
        /// Gets the module's name
        /// </summary>
        string ModuleName { get; }

        /// <summary>
        /// Gets flag indicating if data is loaded on startup
        /// </summary>
        bool IsLoadedOnStartup { get; }

        /// <summary>
        /// Gets the list of modules this module depends on
        /// </summary>
        IEnumerable<string> DependsOn { get; }
    }
}
