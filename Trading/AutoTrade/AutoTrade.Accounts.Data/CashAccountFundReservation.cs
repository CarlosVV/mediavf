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
    
    public partial class CashAccountFundReservation
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string ReservationKey { get; set; }
        public decimal Amount { get; set; }
        public Nullable<System.DateTime> Expiration { get; set; }
        public System.DateTime Created { get; set; }
        public System.DateTime Modified { get; set; }
    
        public virtual CashAccount CashAccount { get; set; }
    }
}
