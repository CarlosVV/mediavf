using System.Collections.Generic;
using System.Linq;
using AutoTrade.Core.Modularity;

namespace AutoTrade.MarketData.Data
{
    public partial class MarketDataRepository
    {
        /// <summary>
        /// Gets module data from the repository
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IModuleData> GetModuleData()
        {
            // return all active modules
            return Modules.Where(m => m.Active);
        }
    }
}
