using System;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Microsoft.Practices.Unity;

using MediaVF.UI.Core;
using System.Text;
using ExternalServicesPOC.Web;
using MediaVF.Common.Entities;
using MediaVF.Common.Communication;
using System.Collections.ObjectModel;

namespace ExternalServicesPOC.Facebook
{
    public class FacebookAuthorizationViewModel : ViewModelBase
    {
        #region Properties

        #region Urls

        string _authorizationUrl;
        string AuthorizationUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_authorizationUrl))
                {
                    StringBuilder authUrl = new StringBuilder();
                    authUrl.Append(App.Current.FacebookSettings.AuthorizationUrl).Append("?")
                           .Append("client_id=").Append(App.Current.FacebookSettings.ApplicationID)
                           .Append("&redirect_uri=").Append(App.Current.FacebookSettings.RedirectUrl);

                    if (!string.IsNullOrEmpty(App.Current.FacebookSettings.Scope))
                        authUrl.Append("&scope=").Append(App.Current.FacebookSettings.Scope);

                    _authorizationUrl = authUrl.ToString();
                }

                return _authorizationUrl;
            }
        }

        #endregion

        #region Codes/Tokens

        /// <summary>
        /// Gets or sets the authorization token from Facebook
        /// </summary>
        string _authorizationCode;
        public string AuthorizationCode
        {
            get { return _authorizationCode; }
            set
            {
                string cur = _authorizationCode;
                if (!string.IsNullOrEmpty(_authorizationCode))
                    cur = EncryptionUtility.Decrypt(App.Current.FacebookSettings.EncryptionKey, _authorizationCode);

                if (cur != value)
                {
                    _authorizationCode = EncryptionUtility.Encrypt(App.Current.FacebookSettings.EncryptionKey, value);

                    RaisePropertyChanged("AuthorizationCode");
                }
            }
        }

        /// <summary>
        /// Gets or sets the access token for making calls to Facebook
        /// </summary>
        string _accessToken;
        public string AccessToken
        {
            get
            {
                if (string.IsNullOrEmpty(_accessToken) && IsolatedStorageSettings.ApplicationSettings.Contains("FacebookAccessToken") &&
                    IsolatedStorageSettings.ApplicationSettings["FacebookAccessToken"] != null)
                    _accessToken = IsolatedStorageSettings.ApplicationSettings["FacebookAccessToken"].ToString();
                return _accessToken;
            }
            set
            {
                string cur = AccessToken;
                if (!string.IsNullOrEmpty(AccessToken))
                    cur = EncryptionUtility.Decrypt(App.Current.FacebookSettings.EncryptionKey, AccessToken);

                if (cur != value)
                {
                    _accessToken = EncryptionUtility.Encrypt(App.Current.FacebookSettings.EncryptionKey, value);

                    if (!IsolatedStorageSettings.ApplicationSettings.Contains("FacebookAccessToken"))
                        IsolatedStorageSettings.ApplicationSettings.Add("FacebookAccessToken", null);
                    IsolatedStorageSettings.ApplicationSettings["FacebookAccessToken"] = _accessToken;
                    IsolatedStorageSettings.ApplicationSettings.Save();

                    RaisePropertyChanged("AccessToken");
                }
            }
        }

        #endregion

        #region Bands

        /// <summary>
        /// Gets the collection of bands retrieved from Facebook
        /// </summary>
        ObservableCollection<Band> _bands;
        public ObservableCollection<Band> Bands
        {
            get
            {
                if (_bands == null)
                    _bands = new ObservableCollection<Band>();
                return _bands;
            }
        }

        /// <summary>
        /// Gets or sets the currently selected band in the list
        /// </summary>
        Band _selectedBand;
        public Band SelectedBand
        {
            get { return _selectedBand; }
            set
            {
                if (_selectedBand != value)
                {
                    _selectedBand = value;

                    RaisePropertyChanged("SelectedBand");
                }
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets the command used to pop up a Facebook window for Facebook authorization
        /// </summary>
        DelegateCommand _grantFacebookAuthorizationCommand;
        public DelegateCommand GrantFacebookAuthorizationCommand
        {
            get
            {
                if (_grantFacebookAuthorizationCommand == null)
                    _grantFacebookAuthorizationCommand = new DelegateCommand(
                        obj =>
                        {
                            HtmlPage.PopupWindow(new Uri(AuthorizationUrl),
                                "facebookAuth",
                                new HtmlPopupWindowOptions()
                                {
                                    Resizeable = false,
                                    Directories = false,
                                    Height = 400,
                                    Width = 600,
                                    Menubar = false,
                                    Scrollbars = true,
                                    Status = true,
                                    Toolbar = false
                                });
                        });
                return _grantFacebookAuthorizationCommand;
            }
        }

        /// <summary>
        /// Gets a command used to get a list of artists from the user's Facebook profile
        /// </summary>
        DelegateCommand _getBandsCommand;
        public DelegateCommand GetBandsCommand
        {
            get
            {
                if (_getBandsCommand == null)
                    _getBandsCommand = new DelegateCommand(
                        obj =>
                        {
                            if (string.IsNullOrEmpty(AccessToken))
                                SetAccessToken(() => GetBands());
                            else
                                GetBands();
                        });
                return _getBandsCommand;
            }
        }

        DelegateCommand _openBandCommand;
        public DelegateCommand OpenBandCommand
        {
            get
            {
                if (_openBandCommand == null)
                    _openBandCommand = new DelegateCommand(
                        obj =>
                        {
                            if (SelectedBand != null)
                                HtmlPage.PopupWindow(new Uri(string.Format("https://graph.facebook.com/{0}", SelectedBand.FacebookID)),
                                    "band",
                                    new HtmlPopupWindowOptions()
                                    {
                                        Resizeable = false,
                                        Directories = false,
                                        Height = 400,
                                        Width = 600,
                                        Menubar = false,
                                        Scrollbars = true,
                                        Status = true,
                                        Toolbar = false
                                    });
                        });
                return _openBandCommand;
            }
        }

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates the view model and registers it for script calls
        /// </summary>
        public FacebookAuthorizationViewModel()
            : base(null)
        {
            HtmlPage.RegisterScriptableObject("facebookManager", this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the authorization token from javascript
        /// </summary>
        /// <param name="authorizationToken"></param>
        [ScriptableMember]
        public void SetAuthorizationToken(string authToken)
        {
            AuthorizationCode = authToken;
        }

        private void SetAccessToken(Action callback)
        {
            FacebookContext context = new FacebookContext();
            context.GetAccessToken(App.Current.FacebookSettings.AccessTokenUrl,
                App.Current.FacebookSettings.ApplicationID,
                App.Current.FacebookSettings.RedirectUrl,
                App.Current.FacebookSettings.ApplicationSecret,
                AuthorizationCode,
                (invokeOp) =>
                {
                    if (!invokeOp.HasError)
                    {
                        AccessToken = invokeOp.Value;
                        if (callback != null)
                            callback();
                    }
                    else
                        MessageBox.Show(invokeOp.Error.ToString());
                },
                null);
        }

        private void GetBands()
        {
            try
            {
                FacebookContext context = new FacebookContext();
                context.Load<Band>(context.GetBandsQuery(AccessToken),
                    (loadOp) =>
                    {
                        if (!loadOp.HasError)
                        {
                            Bands.Clear();
                            foreach (Band band in loadOp.Entities)
                                Bands.Add(band);
                        }
                        else
                            MessageBox.Show(loadOp.Error.ToString());
                    },
                    null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion
    }
}
