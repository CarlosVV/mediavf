using AutoTrade.Accounts.Data;

namespace AutoTrade.Accounts
{
    public interface ITransactionProcessor
    {
        /// <summary>
        /// Processes a transaction
        /// </summary>
        /// <param name="transaction"></param>
        void ProcessTransaction(AccountTransaction transaction);
    }
}
