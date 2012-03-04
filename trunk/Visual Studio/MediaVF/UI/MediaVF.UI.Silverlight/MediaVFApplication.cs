using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ServiceModel.Configuration;
using MediaVF.UI.Core.Configuration;

namespace MediaVF.UI.Core
{
    /// <summary>
    /// Represents a MediaVF Silverlight application
    /// </summary>
    public class MediaVFApplication : Application
    {
        #region Events

        /// <summary>
        /// Event raised when configuration has finished loading from the server
        /// </summary>
        public event EventHandler ConfigurationLoaded;

        /// <summary>
        /// Raises the ConfigurationLoaded event
        /// </summary>
        private void RaiseConfigurationLoaded()
        {
            if (ConfigurationLoaded != null)
                ConfigurationLoaded(this, EventArgs.Empty);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Replaces the base Current to return the current application as a MediaVFApplication
        /// </summary>
        public static new MediaVFApplication Current
        {
            get { return (MediaVFApplication)Application.Current; }
        }

        /// <summary>
        /// Gets the configuration manager for the application
        /// </summary>
        ConfigurationManager _configurationManager;
        public ConfigurationManager ConfigurationManager
        {
            get
            {
                if (_configurationManager == null)
                    _configurationManager = new ConfigurationManager();
                return _configurationManager;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a MediaVFApplication object by attaching to application events
        /// </summary>
        public MediaVFApplication()
        {
            Startup += OnStartup;
            Exit += OnExit;
            UnhandledException += OnUnhandledException;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the startup of the application by loading configuration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected virtual void OnStartup(object sender, StartupEventArgs args)
        {
            ConfigurationManager.LoadConfiguration(() => RaiseConfigurationLoaded());
        }

        /// <summary>
        /// Handles exiting the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected virtual void OnExit(object sender, EventArgs args)
        {
        }

        /// <summary>
        /// Handles unhandled exception raised by the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected virtual void OnUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs args)
        {
        }

        #endregion
    }
}
