using System.Collections.Generic;
using System.Linq;
using System.Timers;

using Microsoft.Practices.Unity;

using MediaVF.Services.Components;
using MediaVF.Services.Configuration;
using MediaVF.Services.Data;
using MediaVF.Services.Logging;

using MediaVF.Services.Polling.Content;
using MediaVF.Services.Polling.Processing;
using System;

namespace MediaVF.Services.Polling
{
    public abstract class PollingComponent<T> : ServiceComponent
    {
        #region Properties

        /// <summary>
        /// Gets the action to be taken when polling
        /// </summary>
        Action _pollingAction;
        Action PollingAction
        {
            get
            {
                // if a polling timer is set up, poll at intervals
                // otherwise, just poll the endpoints once
                if (_pollingAction == null)
                {
                    if (PollingTimer != null)
                        _pollingAction = (() => PollingTimer.Start());
                    else
                        _pollingAction = (() => PollEndpoints());
                }

                return _pollingAction;
            }
        }

        /// <summary>
        /// Gets the timer used to poll at intervals 
        /// </summary>
        Timer _pollingTimer;
        Timer PollingTimer
        {
            get
            {
                if (_pollingTimer == null)
                {
                    // check for a polling interval, defaulting to 0
                    int pollingInterval;
                    if (!int.TryParse(ConfigManager.Components[this.GetType()].Settings["PollingInterval"].Value, out pollingInterval))
                        pollingInterval = 0;

                    // if a polling interval was set up, create and configure the timer
                    if (pollingInterval > 0)
                    {
                        _pollingTimer = new Timer();
                        _pollingTimer.Elapsed += ((sender, e) => PollEndpoints());
                        _pollingTimer.Interval = pollingInterval * 1000;
                    }
                }

                return _pollingTimer;
            }
        }

        /// <summary>
        /// Gets a list of endpoints to poll for content
        /// </summary>
        List<IPollingEndpoint<T>> _pollingEndpoints;
        List<IPollingEndpoint<T>> PollingEndpoints
        {
            get
            {
                if (_pollingEndpoints == null)
                    _pollingEndpoints = ComponentContainer.ResolveAll<IPollingEndpoint<T>>().ToList();
                return _pollingEndpoints;
            }
        }

        /// <summary>
        /// Gets a polling filter used to filter content from the endpoints
        /// </summary>
        IPollingFilter<T> _pollingFilter;
        IPollingFilter<T> PollingFilter
        {
            get
            {
                if (_pollingFilter == null)
                    _pollingFilter = ComponentContainer.Resolve<IPollingFilter<T>>();
                return _pollingFilter;
            }
        }

        /// <summary>
        /// Gets the processors for handling content from the endpoints
        /// </summary>
        List<IContentProcessor> _contentProcessors;
        List<IContentProcessor> ContentProcessors
        {
            get
            {
                if (_contentProcessors == null)
                    _contentProcessors = ComponentContainer.ResolveAll<IContentProcessor>().ToList();
                return _contentProcessors;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create new component with service config manager injected
        /// </summary>
        /// <param name="configManager"></param>
        public PollingComponent(IServiceConfigManager configManager, IComboLog log)
            : base(configManager, log) { }

        #endregion

        #region ServiceComponent Implementation

        /// <summary>
        /// Configures endpoints, content accessors, and filter for polling
        /// </summary>
        protected override void RegisterTypes()
        {
            // configure the endpoints to poll
            ConfigureEndpoints();

            // configure the objects to be used to access content
            ConfigureContentAccessors();

            // configure the filter to be used to filter the content polled for
            ConfigureFilter();

            // configure the processors to be used on the content returned
            ConfigureProcessors();

            // if any processors have been set up, initialize them
            if (ContentProcessors != null && ContentProcessors.Count > 0)
                ContentProcessors.ForEach(contentProcessor => contentProcessor.Initialize());
        }

        /// <summary>
        /// Run the listener and processor
        /// </summary>
        public override void Run()
        {
            PollingAction();
        }

        /// <summary>
        /// Poll endpoints for new items
        /// </summary>
        private void PollEndpoints()
        {
            PollingEndpoints.ForEach(pollingEndpoint =>
            {
                // get new items from endpoint
                List<T> newItems = pollingEndpoint.Poll();

                // for those items that pass all filters, convert to content
                List<IContent> contentToProcess = new List<IContent>();
                newItems.ForEach(newItem =>
                {
                    if (PollingFilter.PassesFilter(newItem))
                    {
                        // get the content from the endpoint
                        IContent content;
                        if (!pollingEndpoint.ProcessAsPlainText)
                            content = ContentFactory.GetContent(ComponentContainer, newItem);
                        else
                            content = (IContent)ContentFactory.GetTextContent(ComponentContainer, newItem);

                        // add to the list of content to process
                        contentToProcess.Add(content);
                    }
                });

                // process new content
                if (contentToProcess != null && contentToProcess.Count > 0)
                    ContentProcessors.ForEach(contentProcessor => contentProcessor.Process(contentToProcess));
            });
        }

        #endregion

        #region Abstract

        /// <summary>
        /// Resolve the endpoints for this polling component
        /// </summary>
        protected abstract void ConfigureEndpoints();

        /// <summary>
        /// Resolve the endpoints for this polling component
        /// </summary>
        protected abstract void ConfigureContentAccessors();

        /// <summary>
        /// Resolve the listener and sources for this component
        /// </summary>
        protected abstract void ConfigureFilter();

        /// <summary>
        /// Resolve the listener and sources for this component
        /// </summary>
        protected abstract void ConfigureProcessors();

        #endregion
    }
}
