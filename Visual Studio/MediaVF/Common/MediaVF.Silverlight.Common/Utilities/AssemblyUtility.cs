using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Resources;

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
            return GetAssemblies(string.Empty);
        }

        /// <summary>
        /// Gets a list of loaded assemblies, filtering for text in the assembly name
        /// </summary>
        /// <param name="filter">Text used to filter assemblies by name</param>
        /// <returns>A list of loaded assemblies matching the filtered name</returns>
        public static IEnumerable<Assembly> GetAssemblies(string filter)
        {
            return
                Deployment.Current.Parts.Where(p => string.IsNullOrEmpty(filter) || p.Source.Contains(filter))
                                        .Select(p =>
                                        {
                                            // get the associated assembly and load it to return it
                                            StreamResourceInfo sri = Application.GetResourceStream(new Uri(p.Source, UriKind.Relative));
                                            return p.Load(sri.Stream);
                                        });
        }
    }
}
