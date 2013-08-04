using System;

namespace AutoTrade.Core.Email.Imap.S22Imap
{
    public class ImapMessageDownloadException : Exception
    {
        #region Fields

        /// <summary>
        /// The IMAP host
        /// </summary>
        private readonly string _host;

        /// <summary>
        /// The IMAP port
        /// </summary>
        private readonly int _port;

        /// <summary>
        /// The user name for the email account
        /// </summary>
        private readonly string _userName;

        /// <summary>
        /// The folder on the account
        /// </summary>
        private readonly string _folder;

        /// <summary>
        /// The ids of the messages
        /// </summary>
        private readonly uint[] _messageIds;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="ImapMessageDownloadException"/>
        /// </summary>
        /// <param name="host">The IMAP host</param>
        /// <param name="port">The port on the IMAP host</param>
        /// <param name="userName">The user name to connect to the host</param>
        /// <param name="folder">The folder</param>
        /// <param name="messageIds">The ids of the messages</param>
        public ImapMessageDownloadException(string host, int port, string userName, string folder, uint[] messageIds)
        {
            _host = host;
            _port = port;
            _userName = userName;
            _folder = folder;
            _messageIds = messageIds;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the IMAP host
        /// </summary>
        public string Host
        {
            get { return _host; }
        }

        /// <summary>
        /// Gets the port on the IMAP host
        /// </summary>
        public int Port
        {
            get { return _port; }
        }

        /// <summary>
        /// Gets the user name used to connect to the IMAP host
        /// </summary>
        public string UserName
        {
            get { return _userName; }
        }

        /// <summary>
        /// Gets the folder from which messages failed to download
        /// </summary>
        public string Folder
        {
            get { return _folder; }
        }

        /// <summary>
        /// Gets the ids of the messages that failed to download
        /// </summary>
        public uint[] MessageIds
        {
            get { return _messageIds; }
        }

        #endregion
    }
}