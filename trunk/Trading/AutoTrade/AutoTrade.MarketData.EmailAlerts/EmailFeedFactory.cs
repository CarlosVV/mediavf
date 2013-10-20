﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoTrade.Core.Email;
using AutoTrade.MarketData.Data;

namespace AutoTrade.MarketData.EmailAlerts
{
    class EmailFeedFactory : IEmailFeedFactory
    {
        #region Fields

        /// <summary>
        /// The email manager factory
        /// </summary>
        private readonly IEmailManagerFactory _emailManagerFactory;

        /// <summary>
        /// The factory for creating repositories
        /// </summary>
        private readonly IMarketDataRepositoryFactory _repositoryFactory;

        /// <summary>
        /// The mapping of feed configurations by name
        /// </summary>
        private Dictionary<string, EmailFeedConfiguration> _configurations;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates an <see cref="EmailFeedFactory"/>
        /// </summary>
        /// <param name="emailManagerFactory"></param>
        /// <param name="repositoryFactory"></param>
        public EmailFeedFactory(IEmailManagerFactory emailManagerFactory, IMarketDataRepositoryFactory repositoryFactory)
        {
            _emailManagerFactory = emailManagerFactory;
            _repositoryFactory = repositoryFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds configurations to the factory
        /// </summary>
        /// <param name="configurations"></param>
        public void AddConfigurations(IEnumerable<EmailFeedConfiguration> configurations)
        {
            if (configurations != null)
                _configurations = configurations.ToDictionary(c => c.Name, c => c);
        }

        /// <summary>
        /// Creates a feed
        /// </summary>
        /// <param name="feedName"></param>
        /// <returns></returns>
        public IEmailFeed CreateFeed(string feedName)
        {
            if (!_configurations.ContainsKey(feedName)) throw new ArgumentException(string.Format("Feed not found: {0}", feedName));

            return new EmailFeed(_emailManagerFactory, _repositoryFactory, _configurations[feedName]);
        }

        #endregion
    }
}