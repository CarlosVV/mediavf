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
    
    public partial class AccountType
    {
        public AccountType()
        {
            this.Accounts = new HashSet<Account>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string SynchronizationManagerType { get; set; }
        public System.DateTime Created { get; set; }
        public System.DateTime Modified { get; set; }
    
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
