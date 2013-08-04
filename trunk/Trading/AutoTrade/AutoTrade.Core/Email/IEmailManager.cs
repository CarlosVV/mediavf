﻿using System.Collections.Generic;

namespace AutoTrade.Core.Email
{
    public interface IEmailManager
    {
        /// <summary>
        /// Search an email account
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="processMessages"></param>
        /// <returns></returns>
        IEnumerable<IEmail> Search(EmailSearchCriteria searchCriteria, bool processMessages = true);
    }
}
