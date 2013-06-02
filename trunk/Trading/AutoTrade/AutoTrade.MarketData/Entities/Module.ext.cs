using System.Collections.Generic;
using System.Linq;
using AutoTrade.Core.Modularity;

namespace AutoTrade.MarketData.Entities
{
    public partial class Module : IModuleData
    {
        /// <summary>
        /// Gets the name of the module
        /// </summary>
        public string ModuleName
        {
            get { return Name; }
        }

        /// <summary>
        /// Gets the type of the module
        /// </summary>
        public string ModuleType
        {
            get { return Type; }
        }

        /// <summary>
        /// Gets the modules on which this module depends
        /// </summary>
        public IEnumerable<string> DependsOn
        {
            get
            {
                // create empty
                var dependsOnModules = new List<string>();

                // check that there are any modules on which this module depends, and, if so, get their names
                if (DependentOnModules != null && DependentOnModules.Count > 0)
                    dependsOnModules.AddRange(DependentOnModules.Select(m => m.DependentOnModule.Name));

                return dependsOnModules;
            }
        }
    }
}
