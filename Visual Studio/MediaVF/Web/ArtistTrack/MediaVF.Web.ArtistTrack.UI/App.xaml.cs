using System;
using System.Windows.Browser;
using System.Windows;

using Microsoft.Practices.Prism;
using Microsoft.Practices.Unity;

using MediaVF.UI.Core;
using MediaVF.Common.Communication.Utilities;

namespace MediaVF.Web.ArtistTrack.UI
{
    public partial class App : MediaVFApplication
    {
        #region Constants

        /// <summary>
        /// Encryption key used to encrypt and decrypt configuration being passed from the server
        /// NOTE: This must be the same on both the client and the server
        /// </summary>
        const string CONFIG_ENCRYPTION_KEY = "8bb91201-95b2-46c9-8634-8f6b6f994784";

        #endregion

        #region Properties

        /// <summary>
        /// Overrides the base Current to get the application as App
        /// </summary>
        public static new App Current
        {
            get { return MediaVFApplication.Current as App; }
        }

        /// <summary>
        /// Gets or sets the container for the application
        /// </summary>
        public IUnityContainer Container { get; private set; }

        /// <summary>
        /// Gets the ID for this application
        /// </summary>
        Guid _id;
        public Guid ID
        {
            get
            {
                if (_id == Guid.Empty)
                    _id = Guid.NewGuid();
                return _id;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates an App object
        /// </summary>
        public App() : base()
        {
            InitializeComponent();
        }

        #endregion

        /// <summary>
        /// Handles startup of the application by setting the configuration encryption key for retrieving config, then
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            // set encryption key
            ConfigurationManager.EncryptionKey = CONFIG_ENCRYPTION_KEY;

            // load up all available data contract types
            DataContractUtility.LoadDataContractTypes();

            // start the app once loaded
            ConfigurationLoaded +=
                (app, args) =>
                {
                    Deployment.Current.Dispatcher.BeginInvoke(
                        () =>
                        {
                            // once config is loaded, kick off the bootstrapper
                            Bootstrapper bootstrapper = new Bootstrapper();
                            bootstrapper.Run();

                            // set the container
                            Container = bootstrapper.Container;
                        });
                };

            // call base to load configuration
            base.OnStartup(sender, e);
        }

        /// <summary>
        /// Handles unhandled exceptions in the app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }

        /// <summary>
        /// Reports errors to the webpage
        /// </summary>
        /// <param name="e"></param>
        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
