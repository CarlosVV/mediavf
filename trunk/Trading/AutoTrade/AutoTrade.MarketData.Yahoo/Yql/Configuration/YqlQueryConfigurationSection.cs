using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using AutoTrade.Core;
using AutoTrade.Core.Modularity.Configuration;
using AutoTrade.Core.Modularity.Configuration.Xml;

namespace AutoTrade.MarketData.Yahoo.Yql.Configuration
{
    /// <summary>
    /// Represents a section in config for setting up Yahoo CSV queries
    /// </summary>
    internal class YqlQueryConfigurationSection : IAssemblyConfigurationSection, IYqlPropertyMapper
    {
        #region Constants

        /// <summary>
        /// The name of the section
        /// </summary>
        public const string SectionName = "yahooYqlQueries";

        /// <summary>
        /// The name of the tags element
        /// </summary>
        private const string PropertyMappingsElementName = "propertyMappings";

        /// <summary>
        /// The name of the type attribute
        /// </summary>
        private const string TypeAttributeName = "type";

        #endregion

        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the collection of tags
        /// </summary>
        private Dictionary<Type, IEnumerable<YqlPropertyMappingElement>> PropertyMappings { get; set; }

        /// <summary>
        /// Gets the collection of columns currently enabled for the csv
        /// </summary>
        private IEnumerable<YqlPropertyMappingElement> GetEnabledPropertyMappings<T>()
        {
            return PropertyMappings.ContainsKey(typeof(T))
                ? PropertyMappings[typeof(T)].Where(t => t.Enabled).ToList()
                : new List<YqlPropertyMappingElement>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads section from xml
        /// </summary>
        /// <param name="xmlElement"></param>
        public void Load(XmlElement xmlElement)
        {
            // validate name
            xmlElement.ShouldBeNamed(SectionName);

            // validate name of columns collection
            xmlElement.FirstChild.ShouldBeNamed(PropertyMappingsElementName);

            // instantiate dictionary
            PropertyMappings = xmlElement.ChildNodes
                                         .OfType<XmlElement>()
                                         .ToDictionary(e => Type.GetType(e.GetAttributeValue(TypeAttributeName)),
                                                       e => e.ChildNodes.OfType<XmlElement>().Select(e2 => new YqlPropertyMappingElement(e2)));
        }

        /// <summary>
        /// Sets properties on a quote from a Yql result
        /// </summary>
        /// <typeparam name="T">The type of item to populate</typeparam>
        /// <param name="item">The item to populate</param>
        /// <param name="elements">The xml elements from which to populate properties</param>
        public void SetPropertiesFromXml<T>(T item, IEnumerable<XmlElement> elements)
        {
            foreach (var propertyMapping in GetEnabledPropertyMappings<T>())
            {
                // get the property from the mapping
                var property = typeof(T).GetProperty(propertyMapping.PropertyName);

                // if the property does not exist on the StockQuote type, skip it
                if (property == null) continue;

                // set the value on the property from the value of the element
                property.SetValue(item, GetElementValue(property.PropertyType, elements, propertyMapping.XmlElementName));
            }
        }

        /// <summary>
        /// Gets the value for an element =
        /// </summary>
        /// <param name="type"></param>
        /// <param name="elements"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        private static object GetElementValue(Type type, IEnumerable<XmlElement> elements, string elementName)
        {
            // get the element by name
            var element =
                elements.FirstOrDefault(e => StringComparer.OrdinalIgnoreCase.Compare(e.Name, elementName) == 0);

            // check that the element was found and has a value; if not, return the default value for the type
            if (element == null || string.IsNullOrWhiteSpace(element.InnerText))
                return type.GetDefault();

            // conver the inner text of the element to the value type
            return element.InnerText.ConvertTo(type);
        }

        #endregion
    }
}
