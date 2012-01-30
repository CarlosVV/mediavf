using System;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

using MediaVF.Entities.ArtistTrack;
using MediaVF.Web.BandedTogether.UI.Admin.Views;
using MediaVF.UI.Core;

namespace MediaVF.Web.BandedTogether.UI.Admin
{
    public class AdminModule : IModule
    {
        #region Properties

        IUnityContainer Container { get; set; }

        IRegionManager RegionManager { get; set; }

        #region Isolated Storage

        string _loginKey;
        public string LoginKey
        {
            get
            {
                if (string.IsNullOrEmpty(_loginKey))
                {
                    if (!IsolatedStorageSettings.ApplicationSettings.Contains("LoginKey"))
                        IsolatedStorageSettings.ApplicationSettings.Add("LoginKey", Guid.NewGuid().ToString());

                    _loginKey = IsolatedStorageSettings.ApplicationSettings["LoginKey"].ToString();
                }

                return _loginKey;
            }
        }

        string _email;
        public string Email
        {
            get
            {
                if (string.IsNullOrEmpty(_email) && IsolatedStorageSettings.ApplicationSettings.Contains("Email"))
                    _email = IsolatedStorageSettings.ApplicationSettings["Email"].ToString();
                return _email;
            }
            set
            {
                if (_email != value)
                {
                    _email = value;

                    if (!IsolatedStorageSettings.ApplicationSettings.Contains("Email"))
                        IsolatedStorageSettings.ApplicationSettings.Add("Email", string.Empty);

                    IsolatedStorageSettings.ApplicationSettings["Email"] = _email;
                }
            }
        }

        string _password;
        public string Password
        {
            get
            {
                if (string.IsNullOrEmpty(_password) && IsolatedStorageSettings.ApplicationSettings.Contains("Password"))
                    _password = IsolatedStorageSettings.ApplicationSettings["Password"].ToString();
                return _password;
            }
            set
            {
                if (_password != value)
                {
                    _password = value;

                    if (!IsolatedStorageSettings.ApplicationSettings.Contains("Password"))
                        IsolatedStorageSettings.ApplicationSettings.Add("Password", string.Empty);

                    IsolatedStorageSettings.ApplicationSettings["Password"] = _password;
                }
            }
        }

        #endregion

        #endregion

        #region Constructors

        public AdminModule(IUnityContainer container, IRegionManager regionManager)
        {
            Container = container;
            RegionManager = regionManager;
        }

        #endregion

        public void Initialize()
        {
            Container.RegisterInstance<AdminModule>(this);

            RegionManager.RegisterViewWithRegion("LoginRegion", typeof(LoginControl));
            RegionManager.RegisterViewWithRegion("LoginRegion", typeof(AdminControl));
        }
    }
}
