namespace AutoTrade.Core.Email.Imap.ImapX
{
    public class ImapXManagerFactory : IEmailManagerFactory
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
        public IEmailManager CreateEmailManager(string host, int port, bool useSsl, string userName, string password)
        {
            return new ImapXManager(host, port, useSsl, userName, password);
        }
    }
}
