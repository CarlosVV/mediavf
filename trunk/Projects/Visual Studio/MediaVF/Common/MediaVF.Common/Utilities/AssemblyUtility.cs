using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MediaVF.Common.Utilities
{
    /// <summary>
    /// Represents a static utility used for interacting with loaded assemblies
    /// </summary>
    public static class AssemblyUtility
    {
        /// <summary>
        /// Gets the list of all loaded assemblies
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        /// <summary>
        /// Gets a list of loaded assemblies, filtering for text in the assembly name
        /// </summary>
        /// <param name="filter">Text used to filter assemblies by name</param>
        /// <returns>A list of loaded assemblies matching the filtered name</returns>
        public static IEnumerable<Assembly> GetAssemblies(string filter)
        {
            return GetAssemblies().Where(a => a.FullName.Contains(filter));
        }
    }
}
