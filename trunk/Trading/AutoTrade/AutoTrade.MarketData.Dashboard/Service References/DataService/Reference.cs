﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutoTrade.MarketData.Dashboard.DataService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Subscription", Namespace="http://schemas.datacontract.org/2004/07/AutoTrade.MarketData.Data")]
    [System.SerializableAttribute()]
    public partial class Subscription : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool ActiveField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime CreatedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AutoTrade.MarketData.Dashboard.DataService.DataProvider DataProviderField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int DataProviderIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> LastUpdatedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime ModifiedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AutoTrade.MarketData.Dashboard.DataService.StockListProvider StockListProviderField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int StockListProviderIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AutoTrade.MarketData.Dashboard.DataService.Stock[] StocksField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.TimeSpan TimeOfDayEndField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.TimeSpan TimeOfDayStartField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.TimeSpan UpdateIntervalField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Active {
            get {
                return this.ActiveField;
            }
            set {
                if ((this.ActiveField.Equals(value) != true)) {
                    this.ActiveField = value;
                    this.RaisePropertyChanged("Active");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Created {
            get {
                return this.CreatedField;
            }
            set {
                if ((this.CreatedField.Equals(value) != true)) {
                    this.CreatedField = value;
                    this.RaisePropertyChanged("Created");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AutoTrade.MarketData.Dashboard.DataService.DataProvider DataProvider {
            get {
                return this.DataProviderField;
            }
            set {
                if ((object.ReferenceEquals(this.DataProviderField, value) != true)) {
                    this.DataProviderField = value;
                    this.RaisePropertyChanged("DataProvider");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int DataProviderID {
            get {
                return this.DataProviderIDField;
            }
            set {
                if ((this.DataProviderIDField.Equals(value) != true)) {
                    this.DataProviderIDField = value;
                    this.RaisePropertyChanged("DataProviderID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ID {
            get {
                return this.IDField;
            }
            set {
                if ((this.IDField.Equals(value) != true)) {
                    this.IDField = value;
                    this.RaisePropertyChanged("ID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> LastUpdated {
            get {
                return this.LastUpdatedField;
            }
            set {
                if ((this.LastUpdatedField.Equals(value) != true)) {
                    this.LastUpdatedField = value;
                    this.RaisePropertyChanged("LastUpdated");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Modified {
            get {
                return this.ModifiedField;
            }
            set {
                if ((this.ModifiedField.Equals(value) != true)) {
                    this.ModifiedField = value;
                    this.RaisePropertyChanged("Modified");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AutoTrade.MarketData.Dashboard.DataService.StockListProvider StockListProvider {
            get {
                return this.StockListProviderField;
            }
            set {
                if ((object.ReferenceEquals(this.StockListProviderField, value) != true)) {
                    this.StockListProviderField = value;
                    this.RaisePropertyChanged("StockListProvider");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int StockListProviderID {
            get {
                return this.StockListProviderIDField;
            }
            set {
                if ((this.StockListProviderIDField.Equals(value) != true)) {
                    this.StockListProviderIDField = value;
                    this.RaisePropertyChanged("StockListProviderID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AutoTrade.MarketData.Dashboard.DataService.Stock[] Stocks {
            get {
                return this.StocksField;
            }
            set {
                if ((object.ReferenceEquals(this.StocksField, value) != true)) {
                    this.StocksField = value;
                    this.RaisePropertyChanged("Stocks");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.TimeSpan TimeOfDayEnd {
            get {
                return this.TimeOfDayEndField;
            }
            set {
                if ((this.TimeOfDayEndField.Equals(value) != true)) {
                    this.TimeOfDayEndField = value;
                    this.RaisePropertyChanged("TimeOfDayEnd");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.TimeSpan TimeOfDayStart {
            get {
                return this.TimeOfDayStartField;
            }
            set {
                if ((this.TimeOfDayStartField.Equals(value) != true)) {
                    this.TimeOfDayStartField = value;
                    this.RaisePropertyChanged("TimeOfDayStart");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.TimeSpan UpdateInterval {
            get {
                return this.UpdateIntervalField;
            }
            set {
                if ((this.UpdateIntervalField.Equals(value) != true)) {
                    this.UpdateIntervalField = value;
                    this.RaisePropertyChanged("UpdateInterval");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DataProvider", Namespace="http://schemas.datacontract.org/2004/07/AutoTrade.MarketData.Data")]
    [System.SerializableAttribute()]
    public partial class DataProvider : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool ActiveField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime CreatedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime ModifiedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AutoTrade.MarketData.Dashboard.DataService.Subscription[] SubscriptionsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TypeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Active {
            get {
                return this.ActiveField;
            }
            set {
                if ((this.ActiveField.Equals(value) != true)) {
                    this.ActiveField = value;
                    this.RaisePropertyChanged("Active");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Created {
            get {
                return this.CreatedField;
            }
            set {
                if ((this.CreatedField.Equals(value) != true)) {
                    this.CreatedField = value;
                    this.RaisePropertyChanged("Created");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ID {
            get {
                return this.IDField;
            }
            set {
                if ((this.IDField.Equals(value) != true)) {
                    this.IDField = value;
                    this.RaisePropertyChanged("ID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Modified {
            get {
                return this.ModifiedField;
            }
            set {
                if ((this.ModifiedField.Equals(value) != true)) {
                    this.ModifiedField = value;
                    this.RaisePropertyChanged("Modified");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AutoTrade.MarketData.Dashboard.DataService.Subscription[] Subscriptions {
            get {
                return this.SubscriptionsField;
            }
            set {
                if ((object.ReferenceEquals(this.SubscriptionsField, value) != true)) {
                    this.SubscriptionsField = value;
                    this.RaisePropertyChanged("Subscriptions");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Type {
            get {
                return this.TypeField;
            }
            set {
                if ((object.ReferenceEquals(this.TypeField, value) != true)) {
                    this.TypeField = value;
                    this.RaisePropertyChanged("Type");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StockListProvider", Namespace="http://schemas.datacontract.org/2004/07/AutoTrade.MarketData.Data")]
    [System.SerializableAttribute()]
    public partial class StockListProvider : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool ActiveField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime CreatedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime ModifiedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AutoTrade.MarketData.Dashboard.DataService.Subscription[] SubscriptionsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TypeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Active {
            get {
                return this.ActiveField;
            }
            set {
                if ((this.ActiveField.Equals(value) != true)) {
                    this.ActiveField = value;
                    this.RaisePropertyChanged("Active");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Created {
            get {
                return this.CreatedField;
            }
            set {
                if ((this.CreatedField.Equals(value) != true)) {
                    this.CreatedField = value;
                    this.RaisePropertyChanged("Created");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ID {
            get {
                return this.IDField;
            }
            set {
                if ((this.IDField.Equals(value) != true)) {
                    this.IDField = value;
                    this.RaisePropertyChanged("ID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Modified {
            get {
                return this.ModifiedField;
            }
            set {
                if ((this.ModifiedField.Equals(value) != true)) {
                    this.ModifiedField = value;
                    this.RaisePropertyChanged("Modified");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AutoTrade.MarketData.Dashboard.DataService.Subscription[] Subscriptions {
            get {
                return this.SubscriptionsField;
            }
            set {
                if ((object.ReferenceEquals(this.SubscriptionsField, value) != true)) {
                    this.SubscriptionsField = value;
                    this.RaisePropertyChanged("Subscriptions");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Type {
            get {
                return this.TypeField;
            }
            set {
                if ((object.ReferenceEquals(this.TypeField, value) != true)) {
                    this.TypeField = value;
                    this.RaisePropertyChanged("Type");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Stock", Namespace="http://schemas.datacontract.org/2004/07/AutoTrade.MarketData.Data")]
    [System.SerializableAttribute()]
    public partial class Stock : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CompanyNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime CreatedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> IndustryField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime ModifiedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> SectorField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AutoTrade.MarketData.Dashboard.DataService.StockQuote[] StockQuotesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AutoTrade.MarketData.Dashboard.DataService.Subscription[] SubscriptionsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SymbolField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CompanyName {
            get {
                return this.CompanyNameField;
            }
            set {
                if ((object.ReferenceEquals(this.CompanyNameField, value) != true)) {
                    this.CompanyNameField = value;
                    this.RaisePropertyChanged("CompanyName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Created {
            get {
                return this.CreatedField;
            }
            set {
                if ((this.CreatedField.Equals(value) != true)) {
                    this.CreatedField = value;
                    this.RaisePropertyChanged("Created");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> Industry {
            get {
                return this.IndustryField;
            }
            set {
                if ((this.IndustryField.Equals(value) != true)) {
                    this.IndustryField = value;
                    this.RaisePropertyChanged("Industry");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Modified {
            get {
                return this.ModifiedField;
            }
            set {
                if ((this.ModifiedField.Equals(value) != true)) {
                    this.ModifiedField = value;
                    this.RaisePropertyChanged("Modified");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> Sector {
            get {
                return this.SectorField;
            }
            set {
                if ((this.SectorField.Equals(value) != true)) {
                    this.SectorField = value;
                    this.RaisePropertyChanged("Sector");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AutoTrade.MarketData.Dashboard.DataService.StockQuote[] StockQuotes {
            get {
                return this.StockQuotesField;
            }
            set {
                if ((object.ReferenceEquals(this.StockQuotesField, value) != true)) {
                    this.StockQuotesField = value;
                    this.RaisePropertyChanged("StockQuotes");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AutoTrade.MarketData.Dashboard.DataService.Subscription[] Subscriptions {
            get {
                return this.SubscriptionsField;
            }
            set {
                if ((object.ReferenceEquals(this.SubscriptionsField, value) != true)) {
                    this.SubscriptionsField = value;
                    this.RaisePropertyChanged("Subscriptions");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Symbol {
            get {
                return this.SymbolField;
            }
            set {
                if ((object.ReferenceEquals(this.SymbolField, value) != true)) {
                    this.SymbolField = value;
                    this.RaisePropertyChanged("Symbol");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StockQuote", Namespace="http://schemas.datacontract.org/2004/07/AutoTrade.MarketData.Data")]
    [System.SerializableAttribute()]
    public partial class StockQuote : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal AskPriceField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal BidPriceField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal ChangeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime CreatedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal OpenPriceField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime QuoteDateTimeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private AutoTrade.MarketData.Dashboard.DataService.Stock StockField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SymbolField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal AskPrice {
            get {
                return this.AskPriceField;
            }
            set {
                if ((this.AskPriceField.Equals(value) != true)) {
                    this.AskPriceField = value;
                    this.RaisePropertyChanged("AskPrice");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal BidPrice {
            get {
                return this.BidPriceField;
            }
            set {
                if ((this.BidPriceField.Equals(value) != true)) {
                    this.BidPriceField = value;
                    this.RaisePropertyChanged("BidPrice");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal Change {
            get {
                return this.ChangeField;
            }
            set {
                if ((this.ChangeField.Equals(value) != true)) {
                    this.ChangeField = value;
                    this.RaisePropertyChanged("Change");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Created {
            get {
                return this.CreatedField;
            }
            set {
                if ((this.CreatedField.Equals(value) != true)) {
                    this.CreatedField = value;
                    this.RaisePropertyChanged("Created");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal OpenPrice {
            get {
                return this.OpenPriceField;
            }
            set {
                if ((this.OpenPriceField.Equals(value) != true)) {
                    this.OpenPriceField = value;
                    this.RaisePropertyChanged("OpenPrice");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime QuoteDateTime {
            get {
                return this.QuoteDateTimeField;
            }
            set {
                if ((this.QuoteDateTimeField.Equals(value) != true)) {
                    this.QuoteDateTimeField = value;
                    this.RaisePropertyChanged("QuoteDateTime");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AutoTrade.MarketData.Dashboard.DataService.Stock Stock {
            get {
                return this.StockField;
            }
            set {
                if ((object.ReferenceEquals(this.StockField, value) != true)) {
                    this.StockField = value;
                    this.RaisePropertyChanged("Stock");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Symbol {
            get {
                return this.SymbolField;
            }
            set {
                if ((object.ReferenceEquals(this.SymbolField, value) != true)) {
                    this.SymbolField = value;
                    this.RaisePropertyChanged("Symbol");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="DataService.IMarketDataService")]
    public interface IMarketDataService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMarketDataService/GetSubscriptions", ReplyAction="http://tempuri.org/IMarketDataService/GetSubscriptionsResponse")]
        AutoTrade.MarketData.Dashboard.DataService.Subscription[] GetSubscriptions();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMarketDataService/GetSubscriptions", ReplyAction="http://tempuri.org/IMarketDataService/GetSubscriptionsResponse")]
        System.Threading.Tasks.Task<AutoTrade.MarketData.Dashboard.DataService.Subscription[]> GetSubscriptionsAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMarketDataServiceChannel : AutoTrade.MarketData.Dashboard.DataService.IMarketDataService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MarketDataServiceClient : System.ServiceModel.ClientBase<AutoTrade.MarketData.Dashboard.DataService.IMarketDataService>, AutoTrade.MarketData.Dashboard.DataService.IMarketDataService {
        
        public MarketDataServiceClient() {
        }
        
        public MarketDataServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MarketDataServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MarketDataServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MarketDataServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public AutoTrade.MarketData.Dashboard.DataService.Subscription[] GetSubscriptions() {
            return base.Channel.GetSubscriptions();
        }
        
        public System.Threading.Tasks.Task<AutoTrade.MarketData.Dashboard.DataService.Subscription[]> GetSubscriptionsAsync() {
            return base.Channel.GetSubscriptionsAsync();
        }
    }
}
