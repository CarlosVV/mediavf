//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutoTrade.Accounts.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class TransactionStatu
    {
        public TransactionStatu()
        {
            this.CashAccountTransactions = new HashSet<CashAccountTransaction>();
            this.TradingAccountTransactions = new HashSet<TradingAccountTransaction>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<CashAccountTransaction> CashAccountTransactions { get; set; }
        public virtual ICollection<TradingAccountTransaction> TradingAccountTransactions { get; set; }
    }
}
