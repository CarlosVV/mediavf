using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Unity;

using MediaVF.Services.Configuration;
using MediaVF.Services.Data;
using MediaVF.Services.Logging;

namespace MediaVF.Services.Components
{
    public abstract class ServiceComponent : IServiceComponent
    {
        #region Properties

        public int ID { get; set; }

        public IComboLog Log { get; set; }

        /// <summary>
        /// Container for this service component
        /// </summary>
        public IUnityContainer ComponentContainer { get; set; }

        /// <summary>
        /// Config manager resolved from parent container
        /// </summary>
        protected IServiceConfigManager ConfigManager { get; set; }

        /// <summary>
        /// Data manager for this service component
        /// </summary>
        protected IDataManager DataManager { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create new component with service config manager injected
        /// </summary>
        /// <param name="configManager"></param>
        public ServiceComponent(IServiceConfigManager configManager, IComboLog log)
        {
            ConfigManager = configManager;
            Log = log;
        }

        #endregion

        #region IModule Implementation

        public void Initialize()
        {
        }

        #endregion

        #region IServiceComponent Implementation

        /// <summary>
        /// Initializes the component by creating a data manager and loading data contexts from config
        /// </summary>
        public void InitializeComponent()
        {
            // create data manager
            DataManager = ComponentContainer.Resolve<IDataManager>();

            // add data contexts
            if (ConfigManager.Components.ContainsKey(this.GetType()) &&
                ConfigManager.Components[this.GetType()].DataContexts != null)
                ConfigManager.Components[this.GetType()].DataContexts.ToList().ForEach(dataContextConfig =>
                    DataManager.AddDataContext(dataContextConfig));

            ComponentContainer.RegisterInstance<IServiceComponent>(GetType().Name, this);

            RegisterTypes();
        }

        #endregion

        #region Abstract

        /// <summary>
        /// Register necessary types for this component
        /// </summary>
        protected abstract void RegisterTypes();

        /// <summary>
        /// Run the component
        /// </summary>
        public abstract void Run();

        #endregion
    }
}
