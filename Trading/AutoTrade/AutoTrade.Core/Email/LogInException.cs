using System;

namespace AutoTrade.Core.Email
{
    public class LogInException : Exception
    {
        #region Fields

        /// <summary>
        /// The server on which logging in failed
        /// </summary>
        private readonly string _server;

        /// <summary>
        /// The user name for which logging in failed
        /// </summary>
        private readonly string _userName;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="LogInException"/>
        /// </summary>
        /// <param name="server"></param>
        /// <param name="userName"></param>
        public LogInException(string server, string userName)
        {
            _server = server;
            _userName = userName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the message for the exception
        /// </summary>
        public override string Message
        {
            get { return string.Format("Failed to log in to the IMAP server {0} for user {1}.", _server, _userName); }
        }

        #endregion
    }
}