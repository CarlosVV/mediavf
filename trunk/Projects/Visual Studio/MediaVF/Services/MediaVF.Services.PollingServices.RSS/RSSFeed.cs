using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;

using Microsoft.Practices.Unity;

using MediaVF.Services.Polling.Content;

namespace MediaVF.Services.Polling.RSS
{
    public class RSSFeed : IPollingEndpoint<SyndicationItem>
    {
        #region Properties

        IUnityContainer ComponentContainer { get; set; }

        /// <summary>
        /// ID of the feed
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Name of the feed
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Uri of the feed
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Last time the feeds were processed
        /// </summary>
        DateTime LastProcessedDateTime { get; set; }

        /// <summary>
        /// Gets or sets flag indicating if this feed can be processed as plain text
        /// </summary>
        public bool ProcessAsPlainText { get; set; }

        #endregion

        #region Constructors

        public RSSFeed(IUnityContainer componentContainer)
        {
            ComponentContainer = componentContainer;
        }

        #endregion

        #region IListenerSource Implementation

        /// <summary>
        /// Gets new items and returns content accessors for each
        /// </summary>
        /// <returns></returns>
        public List<SyndicationItem> Poll()
        {
            // load up the feed from the uri
            try
            {
                if (!string.IsNullOrEmpty(Url))
                    return SyndicationFeed.Load(XmlReader.Create(Url)).Items
                        .Where(item => (item.PublishDate > LastProcessedDateTime || item.LastUpdatedTime > LastProcessedDateTime)).ToList();
                else
                    return new List<SyndicationItem>();
            }
            finally
            {
                // set last processed datetime
                LastProcessedDateTime = DateTime.Now;
            }
        }

        #endregion
    }
}
