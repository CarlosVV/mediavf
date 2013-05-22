﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial interface IMarketDataRepository
    {
        IDbSet<Stock> Stocks { get; set; }
        IDbSet<StockQuote> StockQuotes { get; set; }
        IDbSet<Subscription> Subscriptions { get; set; }
    
    	int SaveChanges();
    }
    
    public partial class AutoTradeEntities : DbContext, IMarketDataRepository
    {
        public AutoTradeEntities()
            : base("name=AutoTradeEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public IDbSet<Stock> Stocks { get; set; }
        public IDbSet<StockQuote> StockQuotes { get; set; }
        public IDbSet<Subscription> Subscriptions { get; set; }
    }
}
