using System.Collections.Generic;
using System.Linq;
using System.Xml;
using AutoTrade.Core;
using AutoTrade.Core.Modularity.Configuration;
using AutoTrade.Core.Modularity.Configuration.Xml;
using AutoTrade.MarketData.Data;
using System;

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

        #endregion

        #region Fields

        /// <summary>
        /// The column configurations
        /// </summary>
        private IEnumerable<YqlPropertyMappingElement> _propertyMappings; 

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the collection of tags
        /// </summary>
        public IEnumerable<YqlPropertyMappingElement> PropertyMappings
        {
            get { return _propertyMappings; }
        }

        /// <summary>
        /// Gets the collection of columns currently enabled for the csv
        /// </summary>
        private List<YqlPropertyMappingElement> EnabledPropertyMappings
        {
            get { return _propertyMappings.Where(t => t.Enabled).ToList(); }
        }

        /// <summary>
        /// Gets the number of columns currently enabled
        /// </summary>
        public int EnabledColumnCount
        {
            get { return EnabledPropertyMappings.Count; }
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

            // create column configurations
            _propertyMappings =
                xmlElement.FirstChild.ChildNodes.OfType<XmlElement>().Select(e => new YqlPropertyMappingElement(e)).ToList();
        }

        /// <summary>
        /// Sets properties on a quote
        /// </summary>
        /// <param name="quote"></param>
        /// <param name="elements"></param>
        public void SetPropertiesOnQuote(StockQuote quote, IEnumerable<XmlElement> elements)
        {
            foreach (var propertyMapping in EnabledPropertyMappings)
            {
                // get the property from the mapping
                var property = typeof(StockQuote).GetProperty(propertyMapping.PropertyName);

                // if the property does not exist on the StockQuote type, skip it
                if (property == null) continue;

                // set the value on the property from the value of the element
                property.SetValue(quote, GetElementValue(property.PropertyType, elements, propertyMapping.XmlElementName));
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
