using System;
using AutoTrade.Core.Settings;

namespace AutoTrade.Accounts.Managers
{
    public class AccountManagementSettings : IAccountManagementSettings
    {
        #region Constants

        /// <summary>
        /// The key for the FinalizedTransactionLifetime setting
        /// </summary>
        private const string FinalizedTransactionLifetimeKey = "FinalizedTransactionLifetime";

        #endregion

        #region Fields

        /// <summary>
        /// The time to keep finalized transactions
        /// </summary>
        private readonly TimeSpan _finalizedTransactionLifetime;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="AccountManagementSettings"/>
        /// </summary>
        /// <param name="settingsProvider"></param>
        public AccountManagementSettings(ISettingsProvider settingsProvider)
        {
            // get time to keep finalized transactions
            _finalizedTransactionLifetime = settingsProvider.GetSetting(FinalizedTransactionLifetimeKey, TimeSpan.FromDays(7));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the amount of time to keep finalized transactions
        /// </summary>
        public TimeSpan FinalizedTransactionLifetime
        {
            get { return _finalizedTransactionLifetime; }
        }

        #endregion
    }
}