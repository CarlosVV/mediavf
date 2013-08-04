using ImapX;

namespace AutoTrade.Core.Email.Imap.ImapX
{
    internal class ImapXMessage : IEmail
    {
        #region Fields

        /// <summary>
        /// The underlying message
        /// </summary>
        private readonly Message _message;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="ImapXMessage"/>
        /// </summary>
        /// <param name="message"></param>
        public ImapXMessage(Message message)
        {
            _message = message;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the sender of the email
        /// </summary>
        public string From
        {
            get { return _message.From.Address; }
        }

        /// <summary>
        /// Gets the subject of the email
        /// </summary>
        public string Subject
        {
            get { return _message.Subject; }
        }

        /// <summary>
        /// Gets the pure text body of the email
        /// </summary>
        public string Body
        {
            get { return _message.TextBody.TextData; }
        }

        /// <summary>
        /// Gets flag indicating if message has been downloaded
        /// </summary>
        public bool IsDownloaded
        {
            get { return !string.IsNullOrWhiteSpace(_message.From.Address); }
        }

        #endregion
        
        #region Methods

        /// <summary>
        /// Downloads a message
        /// </summary>
        public void Download()
        {
            _message.Process();
        }

        #endregion
    }
}