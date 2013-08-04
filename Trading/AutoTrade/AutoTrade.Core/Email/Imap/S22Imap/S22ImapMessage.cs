using System.Net.Mail;

namespace AutoTrade.Core.Email.Imap.S22Imap
{
    public class S22ImapMessage : IEmail
    {
        #region Fields

        /// <summary>
        /// The message
        /// </summary>
        private readonly MailMessage _message;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="S22ImapMessage"/>
        /// </summary>
        /// <param name="message"></param>
        public S22ImapMessage(MailMessage message)
        {
            _message = message;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the sender of the message 
        /// </summary>
        public string From
        {
            get { return _message.From.Address; }
        }

        /// <summary>
        /// Gets the subject of the message
        /// </summary>
        public string Subject
        {
            get { return _message.Subject; }
        }

        /// <summary>
        /// Gets the body of the message
        /// </summary>
        public string Body
        {
            get { return _message.Body; }
        }

        /// <summary>
        /// Gets flag indicating that the message has been downloaded
        /// </summary>
        public bool IsDownloaded
        {
            get { return true; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Does nothing
        /// </summary>
        public void Download()
        {
            // nothing to do - should always be downloaded
        }

        #endregion
    }
}
