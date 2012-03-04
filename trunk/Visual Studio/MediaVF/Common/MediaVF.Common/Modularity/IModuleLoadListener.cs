using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Prism.Modularity;

namespace MediaVF.Common.Modularity
{
    /// <summary>
    /// Represents an interface for listening to loading of modules
    /// </summary>
    public interface IModuleLoadListener
    {
        /// <summary>
        /// Implemented to handle loading of modules
        /// </summary>
        /// <param name="e">The module loaded event args</param>
        void OnModuleLoaded(LoadModuleCompletedEventArgs e);
    }
}
