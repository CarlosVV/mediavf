using System;

namespace AutoTrade.Core.Email
{
    public class FolderNotFoundException : Exception
    {
        #region Fields

        /// <summary>
        /// The IMAP host
        /// </summary>
        private readonly string _host;

        /// <summary>
        /// The user name for the email account
        /// </summary>
        private readonly string _userName;

        /// <summary>
        /// The folder that was not found
        /// </summary>
        private readonly string _folder;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="FolderNotFoundException"/>
        /// </summary>
        /// <param name="host"></param>
        /// <param name="userName"></param>
        /// <param name="folder"></param>
        public FolderNotFoundException(string host, string userName, string folder)
        {
            _host = host;
            _userName = userName;
            _folder = folder;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the message for the exception
        /// </summary>
        public override string Message
        {
            get { return string.Format("Folder {0} not found for account {1} on server {2}.", _folder, _userName, _host); }
        }

        #endregion
    }
}