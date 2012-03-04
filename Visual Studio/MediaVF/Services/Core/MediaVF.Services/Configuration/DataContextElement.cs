using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MediaVF.Services.Configuration
{
    public class DataContextElement : ConfigurationElement
    {
        [ConfigurationProperty("connectionString")]
        public ConnectionStringSettings ConnectionString
        {
            get { return (ConnectionStringSettings)base["connectionString"]; }
            set { base["connectionString"] = value; }
        }

        [ConfigurationProperty("typeMappings", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(DataTypeMappingsCollection), AddItemName="dataTypeMapping")]
        public DataTypeMappingsCollection TypeMappings
        {
            get { return (DataTypeMappingsCollection)base["typeMappings"]; }
        }

        [ConfigurationProperty("storedProcs", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoredProcDefinitionCollection), AddItemName = "storedProc")]
        public StoredProcDefinitionCollection StoredProcs
        {
            get { return (StoredProcDefinitionCollection)base["storedProcs"]; }
        }
    }

    [ConfigurationCollection(typeof(DataContextElement), AddItemName = "dataContext")]
    public class DataContextsCollection : ConfigurationElementCollection<DataContextElement>
    {
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DataContextElement)element).ConnectionString.Name;
        }
    }
}
