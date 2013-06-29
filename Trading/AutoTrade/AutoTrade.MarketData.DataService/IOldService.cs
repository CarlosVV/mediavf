using System.Data.Objects;
using AutoTrade.Core.Serialization;
using AutoTrade.MarketData.Data;
using System.Collections.Generic;
using System.ServiceModel;

namespace AutoTrade.MarketData.DataService
{
    /// <summary>
    /// Service for interacting with market data
    /// </summary>
    [ServiceContract]
    public interface IOldService
    {
        [OperationContract]
        [DataContractResolver(typeof(ProxyDataContractResolver))]
        IEnumerable<Subscription> GetSubscriptions();

        [OperationContract]
        [DataContractResolver(typeof(ProxyDataContractResolver))]
        void UpdateSubscriptions(IEnumerable<Subscription> subscriptions);
    }
}
