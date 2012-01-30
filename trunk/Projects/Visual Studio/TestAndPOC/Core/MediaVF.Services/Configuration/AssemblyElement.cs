using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MediaVF.Services.Configuration
{
    public class AssemblyElement : ConfigurationElement
    {
        [ConfigurationProperty("fullName")]
        public string FullName
        {
            get { return (string)base["fullName"]; }
            set { base["fullName"] = value; }
        }
    }
}
