//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutoTrade.MarketData.Data
{
    using System;
    
    public partial class SubscriptionQuoteSummary
    {
        public int SubscriptionId { get; set; }
        public string Symbol { get; set; }
        public int QuoteCount { get; set; }
        public System.DateTime FirstQuoted { get; set; }
        public System.DateTime LastQuoted { get; set; }
        public decimal LatestPrice { get; set; }
        public decimal LatestBid { get; set; }
        public decimal LatestAsk { get; set; }
        public decimal LatestChange { get; set; }
        public decimal LatestOpen { get; set; }
    }
}