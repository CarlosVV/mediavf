namespace AutoTrade.MarketData.EmailAlerts
{
    interface IEmailAlertsConfiguration
    {
        /// <summary>
        /// Gets the name of the PennyPicks feed
        /// </summary>
        string PennyPicksFeedName { get; }
    }

    class EmailAlertsConfiguration : IEmailAlertsConfiguration
    {
        /// <summary>
        /// Gets the name of the PennyPicks feed
        /// </summary>
        public string PennyPicksFeedName { get; private set; }
    }
}
