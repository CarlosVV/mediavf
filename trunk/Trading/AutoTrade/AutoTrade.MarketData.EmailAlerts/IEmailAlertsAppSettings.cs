namespace AutoTrade.MarketData.EmailAlerts
{
    interface IEmailAlertsAppSettings
    {
        /// <summary>
        /// Gets the name of the PennyPicks feed
        /// </summary>
        string PennyPicksFeedName { get; }

        /// <summary>
        /// Gets the name of the PennyPicks stock data provider
        /// </summary>
        string PennyPicksStockDataProviderName { get; }
    }
}
