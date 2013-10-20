﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial interface IAccountRepository
    {
        IDbSet<AccountType> AccountTypes { get; set; }
        DbQuery<AccountType> AccountTypesQuery { get; }
    
        IDbSet<Broker> Brokers { get; set; }
        DbQuery<Broker> BrokersQuery { get; }
    
        IDbSet<CashAccount> CashAccounts { get; set; }
        DbQuery<CashAccount> CashAccountsQuery { get; }
    
        IDbSet<CashAccountFundReservation> CashAccountFundReservations { get; set; }
        DbQuery<CashAccountFundReservation> CashAccountFundReservationsQuery { get; }
    
        IDbSet<CashAccountTransaction> CashAccountTransactions { get; set; }
        DbQuery<CashAccountTransaction> CashAccountTransactionsQuery { get; }
    
        IDbSet<TradingAccount> TradingAccounts { get; set; }
        DbQuery<TradingAccount> TradingAccountsQuery { get; }
    
        IDbSet<TradingAccountPosition> TradingAccountPositions { get; set; }
        DbQuery<TradingAccountPosition> TradingAccountPositionsQuery { get; }
    
        IDbSet<TradingAccountTransaction> TradingAccountTransactions { get; set; }
        DbQuery<TradingAccountTransaction> TradingAccountTransactionsQuery { get; }
    
        IDbSet<TransactionStatu> TransactionStatus { get; set; }
        DbQuery<TransactionStatu> TransactionStatusQuery { get; }
    
    
    	int SaveChanges();
    }
    
    public partial class AccountRepository : DbContext, IAccountRepository
    {
        public AccountRepository()
            : base("name=AccountRepository")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public IDbSet<AccountType> AccountTypes { get; set; }
        public DbQuery<AccountType> AccountTypesQuery { get { return AccountTypes as DbQuery<AccountType>; } }
        public IDbSet<Broker> Brokers { get; set; }
        public DbQuery<Broker> BrokersQuery { get { return Brokers as DbQuery<Broker>; } }
        public IDbSet<CashAccount> CashAccounts { get; set; }
        public DbQuery<CashAccount> CashAccountsQuery { get { return CashAccounts as DbQuery<CashAccount>; } }
        public IDbSet<CashAccountFundReservation> CashAccountFundReservations { get; set; }
        public DbQuery<CashAccountFundReservation> CashAccountFundReservationsQuery { get { return CashAccountFundReservations as DbQuery<CashAccountFundReservation>; } }
        public IDbSet<CashAccountTransaction> CashAccountTransactions { get; set; }
        public DbQuery<CashAccountTransaction> CashAccountTransactionsQuery { get { return CashAccountTransactions as DbQuery<CashAccountTransaction>; } }
        public IDbSet<TradingAccount> TradingAccounts { get; set; }
        public DbQuery<TradingAccount> TradingAccountsQuery { get { return TradingAccounts as DbQuery<TradingAccount>; } }
        public IDbSet<TradingAccountPosition> TradingAccountPositions { get; set; }
        public DbQuery<TradingAccountPosition> TradingAccountPositionsQuery { get { return TradingAccountPositions as DbQuery<TradingAccountPosition>; } }
        public IDbSet<TradingAccountTransaction> TradingAccountTransactions { get; set; }
        public DbQuery<TradingAccountTransaction> TradingAccountTransactionsQuery { get { return TradingAccountTransactions as DbQuery<TradingAccountTransaction>; } }
        public IDbSet<TransactionStatu> TransactionStatus { get; set; }
        public DbQuery<TransactionStatu> TransactionStatusQuery { get { return TransactionStatus as DbQuery<TransactionStatu>; } }
    }
}