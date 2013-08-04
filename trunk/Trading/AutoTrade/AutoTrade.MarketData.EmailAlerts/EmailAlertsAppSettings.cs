using AutoTrade.Core.Modularity.Configuration;

namespace AutoTrade.MarketData.EmailAlerts
{
    class EmailAlertsAppSettings : IEmailAlertsAppSettings
    {
        #region Constants

        /// <summary>
        /// The key for the PennyPicksFeedName setting
        /// </summary>
        private const string PennyPicksFeedNameKey = "PennyPicksFeedName";

        /// <summary>
        /// The default value for the PennyPicksFeedName
        /// </summary>
        private const string DefaultPennyPicksFeedName = "PennyPicks";

        /// <summary>
        /// The key for the PennyPicksStockDataProviderName setting
        /// </summary>
        private const string PennyPicksStockDataProviderNameKey = "PennyPicksStockDataProviderName";

        /// <summary>
        /// The default value for the PennyPicksStockDataProviderName
        /// </summary>
        private const string DefaultPennyPicksStockDataProviderName = "Yahoo";

        #endregion

        #region Fields

        /// <summary>
        /// The feed name for PennyPicks emails
        /// </summary>
        private readonly string _pennyPicksFeedName;

        /// <summary>
        /// The stock data provider name for PennyPicks
        /// </summary>
        private readonly string _pennyPicksStockDataProviderName;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates an <see cref="EmailAlertsAppSettings"/>
        /// </summary>
        /// <param name="assemblyConfigurationManager"></param>
        public EmailAlertsAppSettings(IAssemblyConfigurationManager assemblyConfigurationManager)
        {
            // get configuration for this assembly
            var configuration = assemblyConfigurationManager.GetAssemblyConfiguration(GetType());

            // set feed name for PennyPicks
            _pennyPicksFeedName = configuration.Settings.GetSetting(PennyPicksFeedNameKey, DefaultPennyPicksFeedName);

            // set stock data provider name for PennyPicks
            _pennyPicksStockDataProviderName =
                configuration.Settings.GetSetting(PennyPicksStockDataProviderNameKey, DefaultPennyPicksStockDataProviderName);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the feed name for PennyPicks emails
        /// </summary>
        public string PennyPicksFeedName
        {
            get { return _pennyPicksFeedName; }
        }
        
        /// <summary>
        /// Gets the name of the PennyPicks stock data provider
        /// </summary>
        public string PennyPicksStockDataProviderName
        {
            get { return _pennyPicksStockDataProviderName; }
        }

        #endregion
    }
}