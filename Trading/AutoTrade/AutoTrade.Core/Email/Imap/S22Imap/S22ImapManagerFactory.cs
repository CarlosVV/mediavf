namespace AutoTrade.Core.Email.Imap.S22Imap
{
    public class S22ImapManagerFactory : IEmailManagerFactory
    {
        /// <summary>
        /// Creates an IMAP email manager
        /// </summary>
        /// <returns></returns>
        public IEmailManager CreateEmailManager(string host, int port, bool useSsl, string userName, string password)
        {
            return new S22ImapManager(host, port, useSsl, userName, password);
        }
    }
}
