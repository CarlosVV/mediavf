using System.Collections.Generic;
using System.Linq;
using System.Timers;

using Microsoft.Practices.Unity;

using MediaVF.Services.Core.Components;
using MediaVF.Services.Core.Configuration;
using MediaVF.Services.Core.Data;
using MediaVF.Services.Core.Logging;

using MediaVF.Services.Polling.Content;
using MediaVF.Services.Polling.Processing;
using System;

namespace MediaVF.Services.Polling
{
    public abstract class PollingComponent<T> : ServiceComponent
    {
        #region Properties

        Action _pollingAction;
        Action PollingAction
        {
            get
            {
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

        Timer _pollingTimer;
        Timer PollingTimer
        {
            get
            {
                if (_pollingTimer == null)
                {
                    int pollingInterval;
                    if (!int.TryParse(ConfigManager.Components[this.GetType()].Settings["PollingInterval"].Value, out pollingInterval))
                        pollingInterval = 0;

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

        protected override void RegisterTypes()
        {
            ConfigureEndpoints();

            ConfigureContentAccessors();

            ConfigureFilter();

            ConfigureProcessors();
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
                        IContent content;
                        if (!pollingEndpoint.ProcessAsPlainText)
                            content = ContentFactory.GetContent(ComponentContainer, newItem);
                        else
                            content = (IContent)ContentFactory.GetTextContent(ComponentContainer, newItem);

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
