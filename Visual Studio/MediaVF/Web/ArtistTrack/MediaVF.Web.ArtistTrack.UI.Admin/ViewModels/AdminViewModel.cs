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

using Microsoft.Practices.Unity;

using MediaVF.Entities.ArtistTrack;
using MediaVF.UI.Core;

namespace MediaVF.Web.ArtistTrack.UI.Admin.ViewModels
{
    public class AdminViewModel : ViewModelBase
    {
        User _currentUser;
        public User CurrentUser
        {
            get
            {
                if (_currentUser == null)
                    _currentUser = Container.Resolve<User>("CurrentUser");
                return _currentUser;
            }
        }

        public AdminViewModel(IUnityContainer container)
            : base(container) { }
    }
}
