using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MediaVF.Services.Configuration
{
    public class DataTypeMappingElement : ConfigurationElement
    {
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

        [ConfigurationProperty("tableName")]
        public string TableName
        {
            get { return (string)base["tableName"]; }
            set { base["tableName"] = value; }
        }

        [ConfigurationProperty("idColumn")]
        public string IDColumn
        {
            get { return (string)base["idColumn"]; }
            set { base["idColumn"] = value; }
        }

        [ConfigurationProperty("cacheData")]
        public bool CacheData
        {
            get { return (bool)base["cacheData"]; }
            set { base["cacheData"] = value; }
        }

        [ConfigurationProperty("propertyMappings")]
        [ConfigurationCollection(typeof(PropertyMappingElement), AddItemName="propertyMapping")]
        public PropertyMappingsCollection PropertyMappings
        {
            get { return (PropertyMappingsCollection)base["propertyMappings"]; }
        }

        [ConfigurationProperty("childTableMappings")]
        [ConfigurationCollection(typeof(ChildDataTypeMappingElement), AddItemName = "childTableMapping")]
        public ChildDataTypeMappingsCollection ChildTableMappings
        {
            get { return (ChildDataTypeMappingsCollection)base["childTableMappings"]; }
        }

    }

    #region Collection

    [ConfigurationCollection(typeof(DataTypeMappingElement), AddItemName="dataTypeMapping")]
    public class DataTypeMappingsCollection : ConfigurationElementCollection<DataTypeMappingElement>
    {
        /// <summary>
        /// Get assembly-qualified type name as key
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DataTypeMappingElement)element).ObjectTypeName;
        }
    }

    #endregion Collection
}
