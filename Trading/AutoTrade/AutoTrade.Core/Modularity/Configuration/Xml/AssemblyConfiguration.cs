using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;

namespace AutoTrade.Core.Modularity.Configuration.Xml
{
    public class AssemblyConfiguration
    {
        #region Constants

        /// <summary>
        /// The name of the assembly configuration root
        /// </summary>
        private const string RootName = "assemblyConfiguration";

        /// <summary>
        /// The name of the custom section's type
        /// </summary>
        private const string CustomSectionTypeAttributeName = "type";

        #endregion

        #region Fields

        /// <summary>
        /// The settings for the assembly
        /// </summary>
        private readonly AssemblyConfigurationSettings _settings;

        /// <summary>
        /// The registrations for the assembly
        /// </summary>
        private readonly AssemblyConfigurationRegistrations _registrations;

        /// <summary>
        /// The collection of custom sections in the configuration
        /// </summary>
        private readonly List<IAssemblyConfigurationSection> _customSections = new List<IAssemblyConfigurationSection>();

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="AssemblyConfiguration"/>
        /// </summary>
        public AssemblyConfiguration(string xml)
        {
            // load xml
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            // validate first child
            xmlDocument.FirstChild.ShouldBeNamed(RootName);

            // get different sections
            foreach (var childNode in xmlDocument.FirstChild.ChildNodes.OfType<XmlElement>())
            {
                switch (childNode.Name)
                {
                    case AssemblyConfigurationSettings.SettingsCollectionName:
                        _settings = new AssemblyConfigurationSettings(childNode);
                        break;
                    case AssemblyConfigurationRegistrations.RegistrationsCollectionName:
                        _registrations = new AssemblyConfigurationRegistrations(childNode);
                        break;
                    default:
                        _customSections.Add(CreateCustomSection(childNode));
                        break;
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the settings for the assembly
        /// </summary>
        public AssemblyConfigurationSettings Settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// Gets the registrations for the assembly
        /// </summary>
        public AssemblyConfigurationRegistrations Registrations
        {
            get { return _registrations; }
        }

        /// <summary>
        /// Gets the custom sections
        /// </summary>
        public IEnumerable<IAssemblyConfigurationSection> CustomSections
        {
            get { return new ReadOnlyCollection<IAssemblyConfigurationSection>(_customSections); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a custom section
        /// </summary>
        /// <param name="xmlElement"></param>
        /// <returns></returns>
        private static IAssemblyConfigurationSection CreateCustomSection(XmlElement xmlElement)
        {
            // get the section type from its type attribute
            var sectionType = xmlElement.GetTypeFromAttribute(CustomSectionTypeAttributeName);

            // create an instance of the section's type
            var customSection = (IAssemblyConfigurationSection)Activator.CreateInstance(sectionType);
            
            // load the section from xml
            customSection.Load(xmlElement);

            return customSection;
        }

        #endregion
    }
}
