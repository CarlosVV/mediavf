using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MediaVF.Services.Core.Configuration
{
    public class PropertyMappingElement : ConfigurationElement
    {
        [ConfigurationProperty("propertyName")]
        public string PropertyName
        {
            get { return (string)base["propertyName"]; }
            set { base["propertyName"] = value; }
        }

        [ConfigurationProperty("columnName")]
        public string ColumnName
        {
            get { return (string)base["columnName"]; }
            set { base["columnName"] = value; }
        }
    }
    
    [ConfigurationCollection(typeof(PropertyMappingElement), AddItemName = "propertyMapping")]
    public class PropertyMappingsCollection : ConfigurationElementCollection<PropertyMappingElement>
    {
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PropertyMappingElement)element).ColumnName;
        }
    }
}
