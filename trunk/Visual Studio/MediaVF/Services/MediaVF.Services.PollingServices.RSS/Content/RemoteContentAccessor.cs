using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;

using Microsoft.Practices.ServiceLocation;

using MediaVF.Services.Core.Logging;
using MediaVF.Services.Polling.Content;

namespace MediaVF.Services.Polling.RSS.Content
{
    public class RemoteContentAccessor : IContentAccessor<SyndicationItem>
    {
        #region UrlProperty

        public enum UrlPropertyType
        {
            None,
            ID,
            Content,
            Links
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets flag indicating that the content of this content accessor is plain text
        /// </summary>
        public bool RawContentIsPlainText { get { return true; } }

        /// <summary>
        /// Content retrieved from source
        /// </summary>
        public object RawContent { get; private set; }

        /// <summary>
        /// Content type retrieved from source
        /// </summary>
        public string ContentType { get; private set; }

        private UrlPropertyType UrlProperty { get; set; }

        #endregion Properties

        #region IContent Accessor Implementation

        /// <summary>
        /// Check if syndication item contains valid remote content (content accessed through url)
        /// </summary>
        /// <param name="item">Syndication item to check for content</param>
        /// <returns>True if item has valid remote content; else, false</returns>
        public bool HasValidContent(SyndicationItem item)
        {
            if (Uri.IsWellFormedUriString(item.Id, UriKind.Absolute))
                UrlProperty = UrlPropertyType.ID;
            else if (item.Content is UrlSyndicationContent)
                UrlProperty = UrlPropertyType.Content;
            else if (item.Links != null && item.Links.Count > 0)
                UrlProperty = UrlPropertyType.Links;

            return UrlProperty != UrlPropertyType.None;
        }

        /// <summary>
        /// Gets content from remote source
        /// </summary>
        /// <param name="item">Syndication item to get content from</param>
        public void Load(SyndicationItem item)
        {
            // get uri, either from id or from content
            Uri remoteContentLocation;
            if (UrlProperty == UrlPropertyType.ID)
                remoteContentLocation = new Uri(item.Id);
            else if (UrlProperty == UrlPropertyType.Content)
                remoteContentLocation = ((UrlSyndicationContent)item.Content).Url;
            else
                remoteContentLocation = item.Links[0].Uri;

            // make a request to the url to get meta-data first
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(remoteContentLocation);
            request.Method = "GET";

            // get response as text
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                ContentType = response.ContentType;

                RawContent = string.Empty;
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    RawContent = streamReader.ReadToEnd();
                }
            }
        }

        #endregion IContentAccessor Implementation
    }
}
