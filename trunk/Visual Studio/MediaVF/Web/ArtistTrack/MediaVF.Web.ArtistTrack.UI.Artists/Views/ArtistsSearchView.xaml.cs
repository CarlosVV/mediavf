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

using MediaVF.Web.ArtistTrack.UI.Artists.ViewModels;

namespace MediaVF.Web.ArtistTrack.UI.Artists.Views
{
    public partial class ArtistsSearchView : UserControl
    {
        public ArtistsSearchView(IUnityContainer container)
        {
            InitializeComponent();

            DataContext = container.Resolve<ArtistsSearchViewModel>();
        }
    }
}
