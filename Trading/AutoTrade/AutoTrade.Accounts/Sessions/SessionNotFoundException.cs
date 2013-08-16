using System;
using AutoTrade.Accounts.Properties;

namespace AutoTrade.Accounts.Sessions
{
    public class SessionNotFoundException : Exception
    {
        /// <summary>
        /// The token for which a session was not found
        /// </summary>
        private readonly Guid _token;

        /// <summary>
        /// Instantiates a <see cref="SessionNotFoundException"/>
        /// </summary>
        /// <param name="token"></param>
        public SessionNotFoundException(Guid token)
        {
            _token = token;
        }

        /// <summary>
        /// Gets the message indicating the token for which a session was not found
        /// </summary>
        public override string Message
        {
            get { return string.Format(Resources.SessionNotFoundMessage, _token); }
        }
    }
}