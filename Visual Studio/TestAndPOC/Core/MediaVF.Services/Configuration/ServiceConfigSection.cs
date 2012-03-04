using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MediaVF.Services.Configuration
{
    public class ServiceConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("sharedDataContext")]
        public SharedDataContextElement SharedDataContext
        {
            get { return (SharedDataContextElement)base["sharedDataContext"]; }
            set { base["sharedDataContext"] = value; }
        }

        [ConfigurationProperty("serviceComponents")]
        public ServiceComponentsCollection Components
        {
            get { return (ServiceComponentsCollection)base["serviceComponents"]; }
        }
    }
}
