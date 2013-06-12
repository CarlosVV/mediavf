using System.Xml;

namespace AutoTrade.Core.Modularity.Configuration
{
    public interface IAssemblyConfigurationSection
    {
        /// <summary>
        /// Loads the section from an xml node
        /// </summary>
        /// <param name="xmlElement"></param>
        void Load(XmlElement xmlElement);
    }
}
