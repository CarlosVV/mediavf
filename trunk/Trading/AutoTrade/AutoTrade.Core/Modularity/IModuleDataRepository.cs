using System.Collections.Generic;

namespace AutoTrade.Core.Modularity
{
    public interface IModuleDataRepository
    {
        /// <summary>
        /// Gets module data from the repository
        /// </summary>
        /// <returns></returns>
        IEnumerable<IModuleData> GetModuleData();
    }
}
