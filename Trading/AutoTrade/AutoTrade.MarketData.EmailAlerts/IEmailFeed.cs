using System;

namespace AutoTrade.MarketData.EmailAlerts
{
    public interface IEmailFeed
    {
        /// <summary>
        /// Event raised when new emails are found
        /// </summary>
        event EventHandler<EmailEventArgs> NewEmailsFound;

        /// <summary>
        /// Starts polling
        /// </summary>
        void Start();

        /// <summary>
        /// Stops polling
        /// </summary>
        void Stop();
    }
}