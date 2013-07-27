using System;
using AutoTrade.Core.Email;
using AutoTrade.MarketData.Data;

namespace AutoTrade.MarketData.EmailAlerts
{
    public static class EmailFeedConfigurationExtensions
    {
        #region Constants

        /// <summary>
        /// The IMAP protocol name
        /// </summary>
        private const string ImapProtocol = "IMAP";

        /// <summary>
        /// The default port for the IMAP protocol
        /// </summary>
        private const int ImapPort = 993;

        #endregion

        #region Methods

        /// <summary>
        /// Creates an <see cref="IEmailManager"/>
        /// </summary>
        /// <param name="emailFeedConfiguration"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IEmailManager CreateEmailManager(this EmailFeedConfiguration emailFeedConfiguration, IEmailManagerFactory factory)
        {
            // get protocol
            var protocol = emailFeedConfiguration.Protocol ?? string.Empty;

            // compare protocol
            if (string.CompareOrdinal(protocol, ImapProtocol) == 0)
                return factory.CreateImapManager(emailFeedConfiguration.Host,
                                                 ImapPort,
                                                 true,
                                                 emailFeedConfiguration.UserName,
                                                 emailFeedConfiguration.Password);

            throw new UnsupportedEmailProtocolException(protocol);
        }

        /// <summary>
        /// Creates search criteria from feed configuration
        /// </summary>
        /// <param name="emailFeedConfiguration"></param>
        /// <returns></returns>
        public static EmailSearchCriteria CreateSearchCriteria(this EmailFeedConfiguration emailFeedConfiguration)
        {
            return new EmailSearchCriteria
            {
                Folder = emailFeedConfiguration.Folder,
                From = emailFeedConfiguration.From,
                ReceivedAfter = emailFeedConfiguration.After,
                ReceivedBefore = emailFeedConfiguration.Before,
                BodyKeywords = !string.IsNullOrWhiteSpace(emailFeedConfiguration.BodyKeywords)
                                   ? emailFeedConfiguration.BodyKeywords.Split(new[] { "|" },
                                                                                StringSplitOptions.RemoveEmptyEntries)
                                   : new string[0],
                SubjectKeywords = !string.IsNullOrWhiteSpace(emailFeedConfiguration.SubjectKeywords)
                                      ? emailFeedConfiguration.SubjectKeywords.Split(new[] { "|" },
                                                                                      StringSplitOptions.RemoveEmptyEntries)
                                      : new string[0]
            };
        }

        #endregion
    }
}
