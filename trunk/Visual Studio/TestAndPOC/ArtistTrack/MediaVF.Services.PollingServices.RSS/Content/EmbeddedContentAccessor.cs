using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

using Microsoft.Practices.ServiceLocation;

using MediaVF.Services.Logging;
using MediaVF.Services.Polling.Content;
using MediaVF.Services.Polling.RSS;

namespace MediaVF.Services.Polling.RSS.Content
{
    public class EmbeddedContentAccessor : IContentAccessor<SyndicationItem>
    {
        #region IContentAccessor Implementation

        /// <summary>
        /// Gets flag indicating that the content of this content accessor is plain text
        /// </summary>
        public bool RawContentIsPlainText { get { return true; } }

        /// <summary>
        /// Embedded content
        /// </summary>
        public object RawContent { get; private set; }

        /// <summary>
        /// Embedded content type
        /// </summary>
        public string ContentType { get; private set; }

        /// <summary>
        /// Check if the item has valid embedded content (html/ xml/ text content in the item itself)
        /// </summary>
        /// <param name="item">Syndication item to check content for</param>
        /// <returns>True if item has valid embedded content; else, false</returns>
        public bool HasValidContent(SyndicationItem item)
        {
            return item.Content != null && !(item.Content is UrlSyndicationContent);
        }

        /// <summary>
        /// Gets embedded content from the item
        /// </summary>
        /// <param name="seed"></param>
        public void Load(SyndicationItem item)
        {
            try
            {
                // read in content as xml
                StringWriter stringWriter = new StringWriter();
                XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);
                item.Content.WriteTo(xmlWriter, "content", null);

                RawContent = xmlWriter.ToString();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(RawContent as string);

                // check if first node is <html> node
                // NOTE: Is there a better way to do this??
                if (xmlDoc.FirstChild != null && xmlDoc.FirstChild.FirstChild != null && xmlDoc.FirstChild.FirstChild.LocalName == "html")
                    ContentType = "text/html";
                else
                    ContentType = "text/xml";
            }
            catch (Exception ex)
            {
                ServiceLocator.Current.GetInstance<IComboLog>().Error("Error loading content from item.", ex);
            }
        }

        #endregion IContentAccessor Implementation
    }
}
