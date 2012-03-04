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

using Microsoft.Practices.Unity;

using MediaVF.UI.Core;
using MediaVF.Web.BandedTogether.UI.Bands.ViewModels;

namespace MediaVF.Web.BandedTogether.UI.Bands.Views
{
    public partial class BandViewer : UserControl
    {
        IUnityContainer Container { get; set; }

        public BandViewer(IUnityContainer container)
        {
            InitializeComponent();

            Container = container;

            DataContext = Container.Resolve<BandViewerViewModel>();
        }
    }
}
