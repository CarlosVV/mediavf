using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace MediaVF.Zune.PlaylistManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        DispatcherSynchronizationContext MainContext { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            // run bootstrapper
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();

            // set shut down mode
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            MainContext = (DispatcherSynchronizationContext)DispatcherSynchronizationContext.Current;

            MainWindow.Closing += (sender, args) =>
                MainContext.Send(o => Shutdown(), null);

            // handle exceptions
            DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            
            base.OnStartup(e);

            MainWindow.Show();
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // set as handled
            e.Handled = true;

            // prompt user
            MessageBoxResult result = MessageBox.Show(string.Format("A system exception has occurred. The application may be unstable now.{0}Exception Details: {1}: {2}{0}{0}Would you like to close the application now?",
                Environment.NewLine,
                e.Exception.GetType().Name,
                e.Exception.Message),
                "System Error",
                MessageBoxButton.YesNo);

            // if the user does not wish to continue, close the app
            if (result == MessageBoxResult.Yes)
                MainContext.Send(o =>
                    Shutdown(),
                    null);
        }
    }
}
