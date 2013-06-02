﻿using System;
using System.ServiceProcess;
using AutoTrade.Core.Bootstrapping;

namespace AutoTrade.MarketData.CollectionService
{
    public partial class CollectionService : ServiceBase
    {
        /// <summary>
        /// The DI container
        /// </summary>
        private IDisposable _container;

        /// <summary>
        /// Instantiates a <see cref="CollectionService"/>
        /// </summary>
        public CollectionService()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles starting of the service
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            // create bootstrapper
            var bootstrapper = new ConfigurationBootstrapper();
            
            // run bootstrapper and get back DI container
            _container = bootstrapper.Run();
        }

        /// <summary>
        /// Handles stopping of the service
        /// </summary>
        protected override void OnStop()
        {
            if (_container != null)
                _container.Dispose();
        }
    }
}