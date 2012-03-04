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

namespace MediaVF.Web.ArtistTrack.UI.Artists.ViewModels
{
    public class ArtistDetailsViewModel : ViewModelBase
    {
        Artist _artist;
        public Artist Artist
        {
            get { return _artist; }
            set
            {
                if (_artist != value)
                {
                    _artist = value;

                    RaisePropertyChanged("Artist");
                }
            }
        }

        public ArtistDetailsViewModel(IUnityContainer container)
            : base(container) { }
    }
}
