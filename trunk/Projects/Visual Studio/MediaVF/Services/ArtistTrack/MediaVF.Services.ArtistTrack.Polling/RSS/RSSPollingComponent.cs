using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Syndication;

using Microsoft.Practices.Unity;

using MediaVF.Entities.ArtistTrack;

using MediaVF.Services.Components;
using MediaVF.Services.Configuration;
using MediaVF.Services.Data;
using MediaVF.Services.Logging;

using MediaVF.Services.Polling;
using MediaVF.Services.Polling.Content;
using MediaVF.Services.Polling.Processing;
using MediaVF.Services.Polling.Matching;

using MediaVF.Services.ArtistTrack.Polling.RSS;
using MediaVF.Services.ArtistTrack.Polling.RSS.Content;

namespace MediaVF.Services.ArtistTrack.Polling.RSS
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
            PropertyValueMatcher<Band> bandMatcher = new PropertyValueMatcher<Band>(this,
                () => DataManager.GetDataContext<Band>().GetAll<Band>(),
                "Name");
            ComponentContainer.RegisterInstance<ITextMatcher<Band>>("BandTourDateMatchingProcessor", bandMatcher);

            RegexMatcher<TourDate> regexMatcher = new RegexMatcher<TourDate>(this,
                () => DataManager.GetDataContext<Regex>().GetByModuleID<Regex>(ID).Select(r => new RSSRegex(r)));
            ComponentContainer.RegisterInstance<ITextMatcher<TourDate>>("BandTourDateMatchingProcessor", regexMatcher);
            
            ParentChildMatchingProcessor<Band, TourDate> bandTourDateMatchingProcessor =
                new ParentChildMatchingProcessor<Band, TourDate>(this,
                    "BandTourDateMatchingProcessor",
                    (tourDates) => DataManager.GetDataContext<TourDate>().AddObjects<TourDate>(tourDates.ToList()));
            ComponentContainer.RegisterInstance<IContentProcessor>(bandTourDateMatchingProcessor.Name, bandTourDateMatchingProcessor);

            bandTourDateMatchingProcessor.SetParentChildLink("ID", "BandID");
        }

        #endregion
    }
}
