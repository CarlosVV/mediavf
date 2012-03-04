using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;

using Microsoft.Practices.Unity;

using MediaVF.Services.Core.Components;
using MediaVF.Services.Core.Configuration;
using System.Collections;
using MediaVF.Services.Core.Data;
using System.Reflection;

namespace MediaVF.Services.Polling.RSS
{
    public class RSSPollingFilter : IPollingFilter<SyndicationItem>
    {
        IServiceComponent ServiceComponent { get; set; }

        IDataManager DataManager { get; set; }

        IServiceConfigManager ConfigManager { get; set; }

        Type _filterType;
        Type FilterType
        {
            get
            {
                if (_filterType == null)
                {
                    string filterType = ConfigManager.Components[ServiceComponent.GetType()].Settings["FilterType"].Value;
                    if (!string.IsNullOrEmpty(filterType))
                        _filterType = Type.GetType(filterType);
                }

                return _filterType;
            }
        }

        string _filterTypeProperty;
        string FilterTypeProperty
        {
            get
            {
                if (_filterTypeProperty == null)
                    _filterTypeProperty = ConfigManager.Components[ServiceComponent.GetType()].Settings["FilterTypeProperty"].Value;

                return _filterTypeProperty;
            }
        }

        List<string> _filterPhrases;
        List<string> FilterPhrases
        {
            get
            {
                if (_filterPhrases == null && ConfigManager != null && ServiceComponent != null)
                {
                    _filterPhrases = new List<string>();

                    string pollingFilters = ConfigManager.Components[ServiceComponent.GetType()].Settings["FilterPhrases"].Value;
                    if (!string.IsNullOrEmpty(pollingFilters))
                        _filterPhrases.AddRange(pollingFilters.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
                }

                return _filterPhrases;
            }
        }

        public RSSPollingFilter(IServiceComponent serviceComponent, IDataManager dataManager, IServiceConfigManager configManager)
        {
            ServiceComponent = serviceComponent;
            DataManager = dataManager;
            ConfigManager = configManager;
        }

        public bool PassesFilter(SyndicationItem item)
        {
            bool passesFilter = false;

            FilterPhrases.ForEach(filterPhrase => passesFilter |= item.Title.Text.Contains(filterPhrase) || item.Summary.Text.Contains(filterPhrase));

            return passesFilter;
        }
    }
}
