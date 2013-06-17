﻿using System.Data.Objects;
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
    public interface IMarketDataService
    {
        [OperationContract]
        [DataContractResolver(typeof(ProxyDataContractResolver))]
        IEnumerable<Subscription> GetSubscriptions();
    }
}