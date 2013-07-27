namespace AutoTrade.Core.Email
{
    public interface IEmail
    {
        /// <summary>
        /// Gets the sender of the email
        /// </summary>
        string From { get;  }

        /// <summary>
        /// Gets the subject of the email
        /// </summary>
        string Subject { get; }

        /// <summary>
        /// Gets the body of the email
        /// </summary>
        string Body { get; }

        /// <summary>
        /// Gets flag indicating if the message has been downloaded from the server yet
        /// </summary>
        bool IsDownloaded { get; }

        /// <summary>
        /// Downloads the message from the server
        /// </summary>
        void Download();
    }
}
