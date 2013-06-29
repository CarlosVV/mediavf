using System;

namespace AutoTrade.MarketData.Data
{
    public partial class Subscription
    {
        /// <summary>
        /// Gets flag indicating if the subscription is active for the current time of day
        /// </summary>
        public bool IsActiveForCurrentTimeOfDay
        {
            get { return DateTime.Now.TimeOfDay >= TimeOfDayStart.TimeOfDay && DateTime.Now.TimeOfDay <= TimeOfDayEnd.TimeOfDay; }
        }
    }
}
