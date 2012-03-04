using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MediaVF.Services.Configuration
{
    public class SharedDataContextElement : ConfigurationElement
    {
        [ConfigurationProperty("assembly")]
        public AssemblyElement Assembly
        {
            get { return (AssemblyElement)base["assembly"]; }
            set { base["assembly"] = value; }
        }

        [ConfigurationProperty("dataContext")]
        public DataContextElement DataContext
        {
            get { return (DataContextElement)base["dataContext"]; }
            set { base["dataContext"] = value; }
        }
    }
}
