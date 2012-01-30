using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MediaVF.Services.Configuration
{
    public class ChildDataTypeMappingElement : DataTypeMappingElement
    {
        [ConfigurationProperty("linkedColumnOnChild")]
        public string LinkedColumnOnChild
        {
            get { return (string)base["linkedColumnOnChild"]; }
            set { base["linkedColumnOnChild"] = value; }
        }

        [ConfigurationProperty("linkedColumnOnParent")]
        public string LinkedColumnOnParent
        {
            get { return (string)base["linkedColumnOnParent"]; }
            set { base["linkedColumnOnParent"] = value; }
        }

        [ConfigurationProperty("linkedPropertyOnChild")]
        public string LinkedPropertyOnChild
        {
            get { return (string)base["linkedPropertyOnChild"]; }
            set { base["linkedPropertyOnChild"] = value; }
        }

        [ConfigurationProperty("linkedPropertyOnParent")]
        public string LinkedPropertyOnParent
        {
            get { return (string)base["linkedPropertyOnParent"]; }
            set { base["linkedPropertyOnParent"] = value; }
        }
    }

    [ConfigurationCollection(typeof(ChildDataTypeMappingElement), AddItemName = "childDataTypeMapping")]
    public class ChildDataTypeMappingsCollection : ConfigurationElementCollection<ChildDataTypeMappingElement>
    {
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ChildDataTypeMappingElement)element).TableName;
        }
    }
}
