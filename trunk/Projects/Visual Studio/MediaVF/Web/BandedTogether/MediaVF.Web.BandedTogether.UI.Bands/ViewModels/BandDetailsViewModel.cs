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

using MediaVF.UI.Core;
using Microsoft.Practices.Unity;
using MediaVF.Entities.ArtistTrack;

namespace MediaVF.Web.BandedTogether.UI.Bands.ViewModels
{
    public class BandDetailsViewModel : ContainerViewModel
    {
        Band _band;
        public Band Band
        {
            get { return _band; }
            set
            {
                if (_band != value)
                {
                    _band = value;

                    RaisePropertyChanged("Band");
                }
            }
        }

        public BandDetailsViewModel(IUnityContainer container)
            : base(container) { }
    }
}
