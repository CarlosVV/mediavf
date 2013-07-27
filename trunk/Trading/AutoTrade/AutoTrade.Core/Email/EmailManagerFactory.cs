namespace AutoTrade.Core.Email
{
    public class EmailManagerFactory : IEmailManagerFactory
    {
        /// <summary>
        /// Creates an IMAP email manager
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="useSsl"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IEmailManager CreateImapManager(string host, int port, bool useSsl, string userName, string password)
        {
            return new ImapManager(host, port, useSsl, userName, password);
        }
    }
}
