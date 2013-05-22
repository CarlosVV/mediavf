//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutoTrade.MarketData
{
    using System;
    using System.Collections.Generic;
    
    public partial class Stock
    {
        public Stock()
        {
            this.StockQuotes = new HashSet<StockQuote>();
            this.Subscriptions = new HashSet<Subscription>();
        }
    
        public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public Nullable<int> Sector { get; set; }
        public Nullable<int> Industry { get; set; }
    
        public virtual ICollection<StockQuote> StockQuotes { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
