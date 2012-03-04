using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Windows;

using Microsoft.Practices.Unity;

using MediaVF.Common;
using MediaVF.Entities.ArtistTrack;
using MediaVF.UI.Core;
using MediaVF.Web.ArtistTrack.Server.Admin;

namespace MediaVF.Web.ArtistTrack.UI.Admin
{
    public class AdminServiceClient
    {
        IUnityContainer Container { get; set; }

        InvokableServiceClient<IAdminService> Service { get; set; }

        public AdminServiceClient(IUnityContainer container)
        {
            Container = container;
            Service = container.Resolve<InvokableServiceClient<IAdminService>>();
        }

        /// <summary>
        /// Calls service for logging in a user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="passwordKey"></param>
        /// <param name="callback"></param>
        public void Login(string email, string password, string passwordKey, Action<User> callback)
        {
            Service.Invoke("Login",
                response =>
                {
                    if (response.Error != null)
                        MessageBox.Show(response.Error.Message, "MediaVF", MessageBoxButton.OK);

                    User user = response.GetValue<User>();

                    callback(user);
                },
                email,
                password,
                passwordKey);
        }

        public void CheckForExistingUser(string email, Action<bool> callback)
        {
            Service.Invoke("CheckForExistingUser",
                response => callback(response.GetValue<bool>()),
                email);
        }

        /// <summary>
        /// Calls service for registering a new user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="passwordKey"></param>
        /// <param name="callback"></param>
        public void Register(User newUser, string passwordKey, string facebookAuthCode, Action<bool> callback)
        {
            Service.Invoke("Register",
                response =>
                {
                    if (response.Error != null)
                        MessageBox.Show(response.Error.Message, "MediaVF", MessageBoxButton.OK);

                    callback(response.Error == null);
                },
                newUser,
                passwordKey,
                facebookAuthCode);
        }
    }
}
