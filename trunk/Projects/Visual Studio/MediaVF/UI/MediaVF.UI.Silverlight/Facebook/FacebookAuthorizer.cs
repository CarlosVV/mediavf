using System;
using System.Text;
using System.Windows.Browser;
using MediaVF.Common.Communication.Configuration.Facebook;

namespace MediaVF.UI.Core.Facebook
{
    public class FacebookAuthorizer
    {
        #region Properties

        /// <summary>
        /// Gets or sets the authorization code
        /// </summary>
        public string AuthorizationCode { get; set; }

        /// <summary>
        /// Gets or sets an action to invoke after authorization
        /// </summary>
        Action AuthorizationCallback { get; set; }

        /// <summary>
        /// Gets the url used to authorize the application for a user on Facebook
        /// </summary>  
        string _authorizationUrl;
        string AuthorizationUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_authorizationUrl) && MediaVFApplication.Current != null)
                {
                    FacebookSettings facebookSettings = MediaVFApplication.Current.ConfigurationManager.GetSection<FacebookSettings>("facebookConfig");
                    if (facebookSettings != null)
                    {
                        StringBuilder authUrl = new StringBuilder();
                        authUrl.Append(facebookSettings.AuthorizationUrl).Append("?")
                               .Append("client_id=").Append(facebookSettings.ApplicationID)
                               .Append("&redirect_uri=").Append(facebookSettings.RedirectUrl);

                        if (!string.IsNullOrEmpty(facebookSettings.Scope))
                            authUrl.Append("&scope=").Append(facebookSettings.Scope);

                        _authorizationUrl = authUrl.ToString();
                    }
                }

                return _authorizationUrl;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a FacebookAuthorizer and registers it as scriptable
        /// </summary>
        public FacebookAuthorizer()
        {
            HtmlPage.RegisterScriptableObject("facebookAuthorizer", this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Opens the facebook authorization url
        /// </summary>
        public void Authorize(Action callback)
        {
            if (string.IsNullOrEmpty(AuthorizationCode))
            {
                AuthorizationCallback = callback;

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
            }
        }

        /// <summary>
        /// Sets the authorization token from javascript
        /// </summary>
        /// <param name="authorizationToken"></param>
        [ScriptableMember]
        public void SetAuthorizationCode(string authorizationCode)
        {
            AuthorizationCode = authorizationCode;

            if (AuthorizationCallback != null)
            {
                AuthorizationCallback();

                AuthorizationCallback = null;
            }
        }

        #endregion
    }
}
