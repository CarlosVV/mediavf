using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MediaVF.Services.Configuration
{
    public class StoredProcParameter : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("type")]
        public string TypeName
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }

        public Type ParameterType
        {
            get { return Type.GetType(TypeName, true); }
        }
    }

    public class StoredProcParameterCollection : ConfigurationElementCollection<StoredProcParameter>
    {
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((StoredProcParameter)element).Name;
        }
    }
}
