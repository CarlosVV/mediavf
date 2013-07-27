using System.Linq;
using ImapX;
using System.Collections.Generic;

namespace AutoTrade.Core.Email
{
    public class ImapManager : IEmailManager
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
        /// Instantiates a <see cref="ImapManager"/>
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="useSsl"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public ImapManager(string host, int port, bool useSsl, string userName, string password)
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
        /// Searches the ac
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public IEnumerable<IEmail> Search(EmailSearchCriteria searchCriteria)
        {
            // create client
            var client = new ImapClient(_host, _port, _useSsl);

            // connect to host
            if (!client.Connection())
                throw new ImapException(string.Format("Failed to connect to server {0}", _host));

            // create result list
            var results = new List<IEmail>();

            try
            {
                // try to log in
                if (!client.LogIn(_userName, _password))
                    throw new LogInException(_host, _userName);

                // search either a single folder or all folders
                var folders = string.IsNullOrWhiteSpace(searchCriteria.Folder)
                                  ? (IEnumerable<Folder>)client.Folders
                                  : new[] {client.Folders[searchCriteria.Folder]};

                // search folders
                foreach (var folder in folders)
                {
                    // if folder is null, it was specified by name and not found
                    if (folder == null) throw new FolderNotFoundException(_host, _userName, searchCriteria.Folder);

                    // add results of search
                    results.AddRange(SearchFolder(folder, searchCriteria));
                }
            }
            finally
            {
                // disconnect after search is done
                client.Disconnect();
            }

            return results;
        }

        /// <summary>
        /// Searches a folder
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        private static IEnumerable<IEmail> SearchFolder(Folder folder, EmailSearchCriteria searchCriteria)
        {
            // select the folder
            folder.Select();

            // create query
            var searchQuery = BuildSearch(searchCriteria);

            // if query was created, run it and return results
            return !string.IsNullOrWhiteSpace(searchQuery) ?
                (IEnumerable<IEmail>)folder.Search(searchQuery, false).Select(m => new ImapMessage(m)) :
                new List<IEmail>();
        }

        /// <summary>
        /// Checks if a message matches the search criteria
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        private static string BuildSearch(EmailSearchCriteria searchCriteria)
        {
            var clauses = new List<string>();

            if (!string.IsNullOrWhiteSpace(searchCriteria.From))
                clauses.Add(string.Format("FROM \"{0}\"", searchCriteria.From));

            if (searchCriteria.ReceivedBefore.HasValue)
                clauses.Add(string.Format("BEFORE \"{0}\"", searchCriteria.ReceivedBefore.Value));

            if (searchCriteria.ReceivedAfter.HasValue)
                clauses.Add(string.Format("SINCE \"{0}\"", searchCriteria.ReceivedAfter.Value));

            if (searchCriteria.SubjectKeywords != null && searchCriteria.SubjectKeywords.Any())
                clauses.Add(string.Format("OR \"{0}\"", string.Join(" ", searchCriteria.SubjectKeywords.Select(k => string.Format("SUBJECT {0}", k)))));

            if (searchCriteria.BodyKeywords != null && searchCriteria.BodyKeywords.Any())
                clauses.Add(string.Format("OR \"{0}\"", string.Join(" ", searchCriteria.BodyKeywords.Select(k => string.Format("BODY {0}", k)))));

            return clauses.Count > 0 ? string.Join(" ", clauses) : string.Empty;
        }

        #endregion
    }
}
