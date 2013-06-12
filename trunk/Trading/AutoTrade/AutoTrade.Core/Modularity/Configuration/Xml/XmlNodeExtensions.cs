using System;
using System.Xml;
using AutoTrade.Core.Properties;

namespace AutoTrade.Core.Modularity.Configuration.Xml
{
    public static class XmlNodeExtensions
    {
        #region Constants

        /// <summary>
        /// The text representing the open of a CDATA tag
        /// </summary>
        private const string OpenCData = "<![CDATA[";

        /// <summary>
        /// The text representing the close of a CDATA tag
        /// </summary>
        private const string CloseCData = "]]>";

        #endregion

        #region Extension Methods

        /// <summary>
        /// Validates a node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodeName"></param>
        public static void ShouldBeNamed(this XmlNode node, string nodeName)
        {
            // verify root node
            if (StringComparer.OrdinalIgnoreCase.Compare(node.Name, nodeName) != 0)
                throw new AssemblyConfigurationException(Resources.UnexpectedNodeExceptionMessage,
                    nodeName,
                    node.Name);
        }

        /// <summary>
        /// Gets the value of an attribute
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <param name="required"></param>
        /// <returns></returns>
        public static string GetAttributeValue(this XmlElement element, string name, bool required = true)
        {
            // get the attribute
            var attribute = element.GetAttributeNode(name);
            if (attribute == null)
            {
                // if not required, just return empty string
                if (!required) return string.Empty;
                
                // otherwise, throw an error
                throw new AssemblyConfigurationException(Resources.MissingAttributeExceptionMessage, name, element.Name);
            }

            // check that the attribute is populated
            if (String.IsNullOrWhiteSpace(attribute.Value))
                throw new AssemblyConfigurationException(Resources.AttributeValueNullExceptionMessage, name, element.Name);

            return attribute.Value;
        }

        /// <summary>
        /// Gets a type from an attribute on an element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static Type GetTypeFromAttribute(this XmlElement element, string attributeName)
        {
            return element.GetAttributeValue(attributeName).ParseType();
        }

        /// <summary>
        /// Gets the inner text of a node, without surrounding CData tags
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string ExtractInnerTextFromCData(this XmlNode node)
        {
            // check that node is not null first
            if (node == null) throw new ArgumentNullException("node");

            // check that there is inner text
            if (string.IsNullOrWhiteSpace(node.InnerText)) return string.Empty;

            // remove open and close CData tags
            return node.InnerText.Replace(OpenCData, string.Empty)
                                 .Replace(CloseCData, string.Empty);
        }

        #endregion
    }
}
