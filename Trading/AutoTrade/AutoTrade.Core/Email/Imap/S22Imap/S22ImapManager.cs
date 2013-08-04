using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using S22.Imap;

namespace AutoTrade.Core.Email.Imap.S22Imap
{
    public class S22ImapManager : IEmailManager
    {
        #region Fields

        /// <summary>
        /// The host to which to connect
        /// </summary>
        private readonly string _host;

        /// <summary>
        /// The port of the host to which to connect
        /// </summary>
        private readonly int _port;

        /// <summary>
        /// Flag indicating whether or not to use SSL
        /// </summary>
        private readonly bool _useSsl;

        /// <summary>
        /// The user name for the account to which to log in
        /// </summary>
        private readonly string _userName;

        /// <summary>
        /// The password for the account to which to log in
        /// </summary>
        private readonly string _password;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="S22ImapManager"/>
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="useSsl"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public S22ImapManager(string host, int port, bool useSsl, string userName, string password)
        {
            _host = host;
            _port = port;
            _useSsl = useSsl;
            _userName = userName;
            _password = password;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Searches using an S22 Imap client
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="processMessages"></param>
        /// <returns></returns>
        public IEnumerable<IEmail> Search(EmailSearchCriteria searchCriteria, bool processMessages = true)
        {
            // create client
            var imapClient = new ImapClient(_host, _port, _useSsl);

            // log in
            imapClient.Login(_userName, _password, AuthMethod.Auto);

            // search for messages
            var messageIds = imapClient.Search(GetSearchCondition(searchCriteria), searchCriteria.Folder);

            // no messages found - return empty collection
            if (messageIds == null || messageIds.Length == 0) return new List<IEmail>();

            // get messages
            var messages = imapClient.GetMessages(messageIds, FetchOptions.TextOnly, true, searchCriteria.Folder);

            // check if messages downloaded properly
            if (messages == null || messages.Length == 0)
                throw new ImapMessageDownloadException(_host, _port, _userName, searchCriteria.Folder, messageIds);

            // create as S22ImapMessages
            return messages.Select(m => new S22ImapMessage(m));
        }

        /// <summary>
        /// Creates a <see cref="SearchCondition"/> object from a <see cref="EmailSearchCriteria"/>
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        private SearchCondition GetSearchCondition(EmailSearchCriteria searchCriteria)
        {
            var searchCondition = new SearchCondition();

            if (!string.IsNullOrWhiteSpace(searchCriteria.From))
                searchCondition = searchCondition.And(SearchCondition.From(searchCriteria.From));

            if (searchCriteria.After.HasValue)
                searchCondition = searchCondition.And(SearchCondition.SentBefore(searchCriteria.After.Value));

            if (searchCriteria.Since.HasValue)
                searchCondition = searchCondition.And(SearchCondition.SentSince(searchCriteria.Since.Value));

            if (searchCriteria.Unread.HasValue)
                searchCondition = searchCondition.And(searchCriteria.Unread.Value ? SearchCondition.Unseen() : SearchCondition.Seen());

            if (searchCriteria.SubjectKeywords != null && searchCriteria.SubjectKeywords.Any())
                searchCondition = searchCondition.And(
                    searchCriteria.SubjectKeywords.Select(SearchCondition.Subject).Aggregate((sc1, sc2) => sc1.Or(sc2)));

            if (searchCriteria.BodyKeywords != null && searchCriteria.BodyKeywords.Any())
                searchCondition = searchCondition.And(
                    searchCriteria.BodyKeywords.Select(SearchCondition.Body).Aggregate((sc1, sc2) => sc1.Or(sc2)));

            return searchCondition;
        }

        #endregion
    }
}
