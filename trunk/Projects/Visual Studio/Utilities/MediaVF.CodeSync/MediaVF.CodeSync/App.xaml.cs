using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using MediaVF.UI.Core;
using Microsoft.Practices.Unity;

namespace MediaVF.CodeSync
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IUnityContainer Container { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            Container = new UnityContainer();

            Container.RegisterType<IDialogService, DialogService>();
            Container.RegisterType<IDisplayService, DisplayService>();
        }
    }
}
