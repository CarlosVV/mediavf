using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;

using Microsoft.Practices.Unity;

using MediaVF.Common.Entities;

using MediaVF.Services.Core.Components;
using MediaVF.Services.Core.Configuration;
using MediaVF.Services.Core.Data;
using MediaVF.Services.Core.Logging;

using MediaVF.Services.Polling.Content;
using MediaVF.Services.Polling.Processing;
using MediaVF.Services.Polling.RSS.Content;
using MediaVF.Services.Polling.Matching;
using System.Reflection;

namespace MediaVF.Services.Polling.RSS
{
    public class RSSPollingComponent : PollingComponent<SyndicationItem>
    {
        #region Constructors

        /// <summary>
        /// Create new component with service config manager injected
        /// </summary>
        /// <param name="configManager"></param>
        public RSSPollingComponent(IServiceConfigManager configManager, IComboLog log)
            : base(configManager, log) { }

        #endregion

        #region ServiceComponent Implementation

        /// <summary>
        /// Initializes the component by creating a data manager and loading data contexts from config
        /// </summary>
        protected override void ConfigureEndpoints()
        {
            // register listener sources (in this case, RSS feeds)
            List<RSSFeed> rssFeeds = DataManager.GetDataContext<RSSFeed>().GetByModuleID<RSSFeed>(ID);
            if (rssFeeds != null)
                rssFeeds.ForEach(rssFeed => ComponentContainer.RegisterInstance<IPollingEndpoint<SyndicationItem>>(rssFeed.ID.ToString(), rssFeed));
        }

        /// <summary>
        /// Initializes the component by creating a data manager and loading data contexts from config
        /// </summary>
        protected override void ConfigureContentAccessors()
        {
            ComponentContainer.RegisterType<IContentAccessor<SyndicationItem>, EmbeddedContentAccessor>(typeof(EmbeddedContentAccessor).Name);
            ComponentContainer.RegisterType<IContentAccessor<SyndicationItem>, RemoteContentAccessor>(typeof(RemoteContentAccessor).Name);
        }

        /// <summary>
        /// Initializes the component by creating a data manager and loading data contexts from config
        /// </summary>
        protected override void ConfigureFilter()
        {
            ComponentContainer.RegisterType<IPollingFilter<SyndicationItem>, RSSPollingFilter>();
        }

        /// <summary>
        /// Run the listener and processor
        /// </summary>
        protected override void ConfigureProcessors()
        {
            PropertyValueMatcher<Band> bandMatcher = new PropertyValueMatcher<Band>(this, DataManager, "Name");
            ComponentContainer.RegisterInstance<ITextMatcher<Band>>("BandTourDateMatchingProcessor", bandMatcher);

            RegexMatcher<TourDate> regexMatcher = new RegexMatcher<TourDate>(this, DataManager);
            ComponentContainer.RegisterInstance<ITextMatcher<TourDate>>("BandTourDateMatchingProcessor", regexMatcher);
            
            ParentChildMatchingProcessor<Band, TourDate> bandTourDateMatchingProcessor =
                new ParentChildMatchingProcessor<Band, TourDate>(this, DataManager, "BandTourDateMatchingProcessor");
            ComponentContainer.RegisterInstance<IContentProcessor>(bandTourDateMatchingProcessor.Name, bandTourDateMatchingProcessor);

            bandTourDateMatchingProcessor.SetParentChildLink("ID", "BandID");
        }

        #endregion
    }
}
