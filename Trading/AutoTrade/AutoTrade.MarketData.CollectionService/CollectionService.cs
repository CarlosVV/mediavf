using System;
using System.ServiceProcess;

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
            StartService();
        }

        /// <summary>
        /// Starts the collection service
        /// </summary>
        public void StartService()
        {
            // create bootstrapper
            var bootstrapper = new CollectionServiceBootstrapper();

            // run bootstrapper and get back DI container
            _container = bootstrapper.Run();
        }

        /// <summary>
        /// Handles stopping of the service
        /// </summary>
        protected override void OnStop()
        {
            StopService();
        }

        /// <summary>
        /// Stops the collection service
        /// </summary>
        public void StopService()
        {
            if (_container != null)
                _container.Dispose();
        }
    }
}
