using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.Text;

using Microsoft.Practices.Unity;

using MediaVF.Services.Components;
using MediaVF.Services.Configuration;
using MediaVF.Services.Data;
using MediaVF.Services.Polling;

namespace MediaVF.Services.ArtistTrack.Polling.RSS
{
    public class RSSPollingFilter : IPollingFilter<SyndicationItem>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the service component to which this filter belongs
        /// </summary>
        IServiceComponent ServiceComponent { get; set; }

        /// <summary>
        /// Gets or sets the data manager for the filter
        /// </summary>
        IDataManager DataManager { get; set; }

        /// <summary>
        /// Gets or sets the config manager
        /// </summary>
        IServiceConfigManager ConfigManager { get; set; }

        /// <summary>
        /// Gets the filter type from the config manager
        /// </summary>
        Type _filterType;
        Type FilterType
        {
            get
            {
                if (_filterType == null)
                {
                    string filterType = ConfigManager.GetComponentSetting(ServiceComponent.GetType(), "FilterType");
                    if (!string.IsNullOrEmpty(filterType))
                        _filterType = Type.GetType(filterType);
                }

                return _filterType;
            }
        }

        /// <summary>
        /// Gets the filter type's property from the config manager
        /// </summary>
        string _filterTypeProperty;
        string FilterTypeProperty
        {
            get
            {
                if (_filterTypeProperty == null)
                    _filterTypeProperty = ConfigManager.GetComponentSetting(ServiceComponent.GetType(), "FilterTypeProperty");

                return _filterTypeProperty;
            }
        }

        /// <summary>
        /// Gets the filter phrases from the config manager
        /// </summary>
        List<string> _filterPhrases;
        List<string> FilterPhrases
        {
            get
            {
                if (_filterPhrases == null && ConfigManager != null && ServiceComponent != null)
                {
                    _filterPhrases = new List<string>();

                    string pollingFilters = ConfigManager.GetComponentSetting(ServiceComponent.GetType(), "FilterPhrases");
                    if (!string.IsNullOrEmpty(pollingFilters))
                        _filterPhrases.AddRange(pollingFilters.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
                }

                return _filterPhrases;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a polling filter for RSS feeds
        /// </summary>
        /// <param name="serviceComponent"></param>
        /// <param name="dataManager"></param>
        /// <param name="configManager"></param>
        public RSSPollingFilter(IServiceComponent serviceComponent, IDataManager dataManager, IServiceConfigManager configManager)
        {
            ServiceComponent = serviceComponent;
            DataManager = dataManager;
            ConfigManager = configManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks if a syndication item passes the filter set up from configuration
        /// </summary>
        /// <param name="item">The syndication item to check the filter against</param>
        /// <returns></returns>
        public bool PassesFilter(SyndicationItem item)
        {
            // initialize to false
            bool passesFilter = false;

            // if any of the phrases are found, it passes the filter
            FilterPhrases.ForEach(filterPhrase => passesFilter |= item.Title.Text.Contains(filterPhrase) || item.Summary.Text.Contains(filterPhrase));

            // return the result
            return passesFilter;
        }

        #endregion
    }
}
