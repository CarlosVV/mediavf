using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

using MediaVF.Services.Components;

namespace MediaVF.Services.Configuration
{
    public class ServiceComponentElement : ConfigurationElement
    {
        [ConfigurationProperty("componentType", IsKey = true)]
        public string ComponentTypeName
        {
            get { return (string)base["componentType"]; }
            set { base["componentType"] = value; }
        }

        public Type ComponentType
        {
            get { return Type.GetType(ComponentTypeName, true); }
        }

        [ConfigurationProperty("settings", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(KeyValueConfigurationElement))]
        public KeyValueConfigurationCollection Settings
        {
            get { return (KeyValueConfigurationCollection)base["settings"]; }
        }

        [ConfigurationProperty("dataContexts", IsDefaultCollection = false)]
        public DataContextsCollection DataContexts
        {
            get { return (DataContextsCollection)base["dataContexts"]; }
        }
    }

    #region Collection

    [ConfigurationCollection(typeof(ServiceComponentElement), AddItemName = "serviceComponent")]
    public class ServiceComponentsCollection : ConfigurationElementCollection<ServiceComponentElement>
    {
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceComponentElement)element).ComponentTypeName;
        }
    }

    #endregion
}
