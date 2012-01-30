using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MediaVF.Services.Configuration
{
    public class StoredProcDefinition : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("objectType")]
        public string ObjectTypeName
        {
            get { return (string)base["objectType"]; }
            set { base["objectType"] = value; }
        }

        public Type ObjectType
        {
            get { return Type.GetType(ObjectTypeName, true); }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(StoredProcParameter), AddItemName = "parameter")]
        public StoredProcParameterCollection Parameters
        {
            get { return (StoredProcParameterCollection)base[""]; }
        }
    }

    public class StoredProcDefinitionCollection : ConfigurationElementCollection<StoredProcDefinition>
    {
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((StoredProcDefinition)element).Name;
        }
    }
}
