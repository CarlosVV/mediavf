namespace AutoTrade.Core.Email
{
    public interface IEmailManagerFactory
    {
        /// <summary>
        /// Creates an IMAP email manager
        /// </summary>
        /// <returns></returns>
        IEmailManager CreateImapManager(string host, int port, bool useSsl, string userName, string password);
    }
}
