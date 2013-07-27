using System;
using System.Collections.Generic;

namespace AutoTrade.Core.Email
{
    public class EmailSearchCriteria
    {
        /// <summary>
        /// Gets or sets the folder to search
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        /// Gets or sets the address from which the email was sent
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets a list of keywords to look for in the subject of the email
        /// </summary>
        public IEnumerable<string> SubjectKeywords { get; set; }

        /// <summary>
        /// Gets or sets a list of keywords to look for in the body of the email
        /// </summary>
        public IEnumerable<string> BodyKeywords { get; set; }

        /// <summary>
        /// Gets or sets the date after which the email must have been sent
        /// </summary>
        public DateTime? ReceivedAfter { get; set; }

        /// <summary>
        /// Gets or sets the date before which the email must have been sent
        /// </summary>
        public DateTime? ReceivedBefore { get; set; }
    }
}