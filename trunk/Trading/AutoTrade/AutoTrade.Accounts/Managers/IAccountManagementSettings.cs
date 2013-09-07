using System;

namespace AutoTrade.Accounts.Managers
{
    public interface IAccountManagementSettings
    {
        /// <summary>
        /// Gets the amount of time to keep finalized transactions
        /// </summary>
        TimeSpan FinalizedTransactionLifetime { get; }
    }
}
