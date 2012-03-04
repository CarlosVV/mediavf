using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

using MediaVF.Web.ArtistTrack.UI.Admin.ViewModels;

namespace MediaVF.Web.ArtistTrack.UI.Admin.Views
{
    public partial class LoginControl : UserControl
    {
        IUnityContainer Container { get; set; }

        public LoginControl()
        {
            InitializeComponent();
        }

        public LoginControl(IUnityContainer container)
        {
            InitializeComponent();

            Container = container;

            if (Container != null)
                DataContext = Container.Resolve<LoginViewModel>();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ((LoginViewModel)DataContext).Login();
        }
    }
}
