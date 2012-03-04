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

namespace ExternalServicesPOC.Configuration
{
    public class FacebookSettings : DependencyObject
    {
        #region Dependency Properties

        public static readonly DependencyProperty EncryptionKeyProperty = DependencyProperty.Register("EncryptionKey",
            typeof(string),
            typeof(FacebookSettings),
            new PropertyMetadata(string.Empty));

        public string EncryptionKey
        {
            get { return (string)GetValue(EncryptionKeyProperty); }
            set { SetValue(EncryptionKeyProperty, value); }
        }

        public static readonly DependencyProperty AuthorizationUrlProperty = DependencyProperty.Register("AuthorizationUrl",
            typeof(string),
            typeof(FacebookSettings),
            new PropertyMetadata(string.Empty));

        public string AuthorizationUrl
        {
            get { return (string)GetValue(AuthorizationUrlProperty); }
            set { SetValue(AuthorizationUrlProperty, value); }
        }

        public static readonly DependencyProperty AccessTokenUrlProperty = DependencyProperty.Register("AccessTokenUrl",
            typeof(string),
            typeof(FacebookSettings),
            new PropertyMetadata(string.Empty));

        public string AccessTokenUrl
        {
            get { return (string)GetValue(AccessTokenUrlProperty); }
            set { SetValue(AccessTokenUrlProperty, value); }
        }

        public static readonly DependencyProperty RedirectUrlProperty = DependencyProperty.Register("RedirectUrl",
            typeof(string),
            typeof(FacebookSettings),
            new PropertyMetadata(string.Empty));

        public string RedirectUrl
        {
            get { return (string)GetValue(RedirectUrlProperty); }
            set { SetValue(RedirectUrlProperty, value); }
        }

        public static readonly DependencyProperty ApplicationIDProperty = DependencyProperty.Register("ApplicationID",
            typeof(string),
            typeof(FacebookSettings),
            new PropertyMetadata(string.Empty));

        public string ApplicationID
        {
            get { return (string)GetValue(ApplicationIDProperty); }
            set { SetValue(ApplicationIDProperty, value); }
        }

        public static readonly DependencyProperty ApplicationSecretProperty = DependencyProperty.Register("ApplicationSecret",
            typeof(string),
            typeof(FacebookSettings),
            new PropertyMetadata(string.Empty));

        public string ApplicationSecret
        {
            get { return (string)GetValue(ApplicationSecretProperty); }
            set { SetValue(ApplicationSecretProperty, value); }
        }

        public static readonly DependencyProperty ScopeProperty = DependencyProperty.Register("Scope",
            typeof(string),
            typeof(FacebookSettings),
            new PropertyMetadata(string.Empty));

        public string Scope
        {
            get { return (string)GetValue(ScopeProperty); }
            set { SetValue(ScopeProperty, value); }
        }

        #endregion
    }
}
