using System;
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

using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

using MediaVF.Common.Communication.Utilities;
using MediaVF.Common.Utilities;
using MediaVF.Entities.ArtistTrack;
using MediaVF.UI.Core;
using MediaVF.Web.ArtistTrack.UI.Admin;

namespace MediaVF.Web.ArtistTrack.UI.Admin.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        #region Properties

        IEventAggregator EventAggregator { get; set; }

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

        User _currentUser;
        public User CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;

                Container.RegisterInstance<User>("CurrentUser", _currentUser);
            }
        }

        /// <summary>
        /// Gets or sets a flag indicating if login is still required
        /// </summary>
        bool _logInRequired = true;
        public bool LogInRequired
        {
            get { return _logInRequired; }
            set
            {
                if (_logInRequired != value)
                {
                    _logInRequired = value;

                    if (!_logInRequired)
                    {
                        Container.Resolve<IDisplayService>().Display<AdminViewModel>();

                        EventAggregator.GetEvent<CompositePresentationEvent<UIEventArgs<bool>>>().Publish(new UIEventArgs<bool>("LoggedIn", true));
                    }

                    RaisePropertyChanged("LogInRequired");
                }
            }
        }

        /// <summary>
        /// Gets or sets flag indicating if the user's credentials should be stored locally
        /// </summary>
        bool _storeCredentials = false;
        public bool StoreCredentials
        {
            get { return _storeCredentials; }
            set
            {
                if (_storeCredentials != value)
                {
                    _storeCredentials = value;

                    RaisePropertyChanged("StoreCredentials");
                }
            }
        }

        /// <summary>
        /// Gets or sets the user's email address
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

        /// <summary>
        /// Gets or sets an encrypted version of the user's password
        /// </summary>
        string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                string cur = string.Empty;
                if (!string.IsNullOrEmpty(_password))
                    cur = EncryptionUtility.Decrypt(LoginKey, _password).Trim('\0');

                if (value != cur)
                {
                    _password = EncryptionUtility.Encrypt(LoginKey, value);

                    RaisePropertyChanged("Password");
                }
            }
        }

        /// <summary>
        /// Gets or sets the command used to login
        /// </summary>
        ICommand _loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                    _loginCommand = new DelegateCommand(
                        (obj) => 
                            Login(),
                        (obj) =>
                            !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password),
                        this);
                return _loginCommand;
            }
        }

        /// <summary>
        /// Gets or sets the command used to login
        /// </summary>
        ICommand _registerCommand;
        public ICommand RegisterCommand
        {
            get
            {
                if (_registerCommand == null)
                    _registerCommand = new DelegateCommand((obj) => Register());
                return _registerCommand;
            }
        }

        #endregion

        #region Constructor

        public LoginViewModel(IUnityContainer container, IEventAggregator eventAggregator)
            : base(container)
        {
            EventAggregator = eventAggregator;

            AdminModule adminModule = Container.Resolve<AdminModule>();
            if (adminModule != null && !string.IsNullOrEmpty(adminModule.Email) && !string.IsNullOrEmpty(adminModule.Password))
            {
                _email = adminModule.Email;
                _password = adminModule.Password;
                Login();
            }
        }

        #endregion

        public void Login()
        {
            IsBusy = true;
            BusyMessage = "Logging in...";

            if (StoreCredentials)
            {
                AdminModule adminModule = Container.Resolve<AdminModule>();
                if (adminModule != null)
                {
                    adminModule.Email = Email;
                    adminModule.Password = Password;
                }
            }

            /*
            AdminServiceClient userServiceClient = Container.Resolve<AdminServiceClient>();
            userServiceClient.Login(Email, Password, LoginKey,
                (result) =>
                {*/
                    IsBusy = false;

                    /*if (result != null)
                    {*/
                        CurrentUser = new User() { UserName = "evf" }; //result;
                        LogInRequired = false;
                    /*}
                });
             */
        }

        public void Register()
        {
            RegisterViewModel registerConfirm = Container.Resolve<RegisterViewModel>();
            registerConfirm.Close += () =>
            {
                if (registerConfirm.NewUser != null)
                {
                    CurrentUser = registerConfirm.NewUser;
                    LogInRequired = false;
                }
            };
            Container.Resolve<DisplayService>().Display(registerConfirm);
        }
    }
}
