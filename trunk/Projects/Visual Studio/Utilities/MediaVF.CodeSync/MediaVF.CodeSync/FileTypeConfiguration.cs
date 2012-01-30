using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MediaVF.CodeSync
{
    public class FileTypeConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(FileTypeElement), AddItemName = "fileType")]
        public FileTypeElementCollection FileTypes
        {
            get { return (FileTypeElementCollection)base[""]; }
            set { base[""] = value; }
        }
    }

    public class FileTypeElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FileTypeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FileTypeElement)element).Name;
        }
    }

    public class FileTypeElement : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public FileExtensionElementCollection FileExtensions
        {
            get { return (FileExtensionElementCollection)base[""]; }
            set { base[""] = value; }
        }
    }

    public class FileExtensionElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FileExtensionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FileExtensionElement)element).Extension;
        }
    }

    public class FileExtensionElement : ConfigurationElement
    {
        [ConfigurationProperty("extension")]
        public string Extension
        {
            get { return (string)base["extension"]; }
            set { base["extension"] = value; }
        }
    }
}
