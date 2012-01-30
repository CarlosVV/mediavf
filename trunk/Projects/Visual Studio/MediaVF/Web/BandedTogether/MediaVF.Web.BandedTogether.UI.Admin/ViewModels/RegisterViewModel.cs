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

using MediaVF.Common.Communication.Utilities;
using MediaVF.Common.Utilities;
using MediaVF.Entities.ArtistTrack;
using MediaVF.UI.Core;
using MediaVF.UI.Core.Facebook;

namespace MediaVF.Web.BandedTogether.UI.Admin.ViewModels
{
    public class RegisterViewModel : ContainerViewModel
    {
        #region Properties

        /// <summary>
        /// Gets the current login key
        /// </summary>
        string LoginKey
        {
            get
            {
                if (Container != null)
                    return Container.Resolve<AdminModule>().LoginKey;
                else
                    return null;
            }
        }

        #region User

        /// <summary>
        /// Gets the user entity to be populated
        /// </summary>
        User _newUser;
        public User NewUser
        {
            get
            {
                if (_newUser == null)
                    _newUser = new User();
                return _newUser;
            }
        }

        /// <summary>
        /// Gets or sets the user name
        /// </summary>
        string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    RaisePropertyChanged("UserName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the user's email
        /// </summary>
        string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    RaisePropertyChanged("Email");
                }
            }
        }

        #region Password

        /// <summary>
        /// Gets or sets the user's password
        /// </summary>
        string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                // decrypt the current value for comparison
                string cur = string.Empty;
                if (!string.IsNullOrEmpty(_password))
                    cur = EncryptionUtility.Decrypt(LoginKey, _password);

                // compare the current value to the new value
                if (!string.IsNullOrEmpty(value) && value != cur)
                {
                    // store an encrypted version of the new password
                    _password = EncryptionUtility.Encrypt(LoginKey, value);

                    RaisePropertyChanged("Password");
                    RaisePropertyChanged("PasswordsMismatch");
                }
            }
        }

        /// <summary>
        /// Gets or sets the confirmed password
        /// </summary>
        string _confirmPassword;
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                // decrypt the current value for comparison
                string cur = string.Empty;
                if (!string.IsNullOrEmpty(_confirmPassword))
                    cur = EncryptionUtility.Decrypt(LoginKey, _confirmPassword);

                // compare the current value to the new value
                if (!string.IsNullOrEmpty(value) && value != cur)
                {
                    // store an encrypted version of the new password
                    _confirmPassword = EncryptionUtility.Encrypt(LoginKey, value);

                    RaisePropertyChanged("ConfirmPassword");
                    RaisePropertyChanged("PasswordsMismatch");
                }
            }
        }

        /// <summary>
        /// Gets a flag indicating if there is a mismatch between the initial password and the confirmation
        /// </summary>
        public bool PasswordsMismatch
        {
            get
            {
                // check that both a password and confirmation have been given
                if (!string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(ConfirmPassword))
                {
                    // decrypt the passwords
                    string password = EncryptionUtility.Decrypt(LoginKey, Password);
                    string confirmPassword = EncryptionUtility.Decrypt(LoginKey, ConfirmPassword);

                    // return comparison
                    return password != confirmPassword;
                }
                else
                    return false;
            }
        }

        #endregion

        #region Facebook

        /// <summary>
        /// Gets or set flag indicating if the user wishes to sync with Facebook
        /// </summary>
        bool _syncWithFacebook;
        public bool SyncWithFacebook
        {
            get { return _syncWithFacebook; }
            set
            {
                if (_syncWithFacebook != value)
                {
                    _syncWithFacebook = value;

                    RaisePropertyChanged("SyncWithFacebook");
                }
            }
        }

        /// <summary>
        /// Gets or sets the user's email address that's used to login to Facebook
        /// </summary>
        string _facebookEmail;
        public string FacebookEmail
        {
            get { return _facebookEmail; }
            set
            {
                if (_facebookEmail != value)
                {
                    _facebookEmail = value;

                    RaisePropertyChanged("FacebookEmail");
                }
            }
        }

        /// <summary>
        /// Gets or sets the user's Facebook password
        /// </summary>
        string _facebookPassword;
        public string FacebookPassword
        {
            get { return _facebookPassword; }
            set
            {
                // decrypt the current value for comparison
                string cur = string.Empty;
                if (!string.IsNullOrEmpty(_facebookPassword))
                    cur = EncryptionUtility.Decrypt(LoginKey, _facebookPassword);

                // compare the current value to the new value
                if (cur != value && !string.IsNullOrEmpty(value))
                {
                    // store an encrypted version of the new password
                    _facebookPassword = EncryptionUtility.Encrypt(LoginKey, value);

                    RaisePropertyChanged("FacebookPassword");
                }
            }
        }

        #endregion

        #endregion

        #region Facebook

        FacebookAuthorizer _facebookAuthorizer;
        FacebookAuthorizer FacebookAuthorizer
        {
            get
            {
                if (_facebookAuthorizer == null)
                    _facebookAuthorizer = new FacebookAuthorizer();
                return _facebookAuthorizer;
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets the command for registering
        /// </summary>
        DelegateCommand _registerCommand;
        public DelegateCommand RegisterCommand
        {
            get
            {
                if (_registerCommand == null)
                    _registerCommand = new DelegateCommand(
                        (obj) => SetUserPropertiesAndRegister(),
                        (obj) => !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(ConfirmPassword) && !PasswordsMismatch,
                        this);
                return _registerCommand;
            }
        }

        /// <summary>
        /// Gets the command for canceling registration
        /// </summary>
        DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new DelegateCommand(obj => RaiseClose());
                return _cancelCommand;
            }
        }

        #endregion

        #endregion

        #region Constructors

        public RegisterViewModel(IUnityContainer container)
            : base(container) { }

        #endregion

        #region Methods

        /// <summary>
        /// Registers the user by calling the server
        /// </summary>
        public void SetUserPropertiesAndRegister()
        {
            // show busy message
            IsBusy = true;
            BusyMessage = "Registering...";

            // set properties on user object
            NewUser.UserName = UserName;
            NewUser.Password = Password;
            NewUser.Email = Email;
            NewUser.SyncWithFacebook = SyncWithFacebook;

            // set facebook values
            if (SyncWithFacebook)
            {
                NewUser.FacebookEmail = FacebookEmail;
                NewUser.FacebookPassword = FacebookPassword;
            }

            // if the user has chosen to sync with facebook, 
            if (NewUser.SyncWithFacebook)
                FacebookAuthorizer.Authorize(Register);
            else
                Register();
        }

        /// <summary>
        /// Registers the user, with the given Facebook values if applicable
        /// </summary>
        private void Register()
        {
            // check if user needs to be synced with facebook
            if (!NewUser.SyncWithFacebook || !string.IsNullOrEmpty(FacebookAuthorizer.AuthorizationCode))
            {
                // get service client and register user
                AdminServiceClient userServiceClient = Container.Resolve<AdminServiceClient>();
                userServiceClient.Register(NewUser, LoginKey, FacebookAuthorizer.AuthorizationCode,
                    (result) =>
                    {
                        // turn off busy message
                        IsBusy = false;

                        // if the user was successfully registered, show confirmation message
                        if (result)
                        {
                            MessageBox.Show("Thank you for joining ArtistTrack!", "ArtistTrack", MessageBoxButton.OK);
                            RaiseClose();
                        }
                    });
            }
            else
                MessageBox.Show("Facebook authorization was not granted. Please uncheck the Sync with Facebook checkbox or try registering again.",
                    "ArtistTrack",
                    MessageBoxButton.OK);
        }

        #endregion
    }
}
