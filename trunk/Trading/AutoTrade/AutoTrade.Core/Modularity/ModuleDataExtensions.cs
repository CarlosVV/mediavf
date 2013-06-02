using Microsoft.Practices.Prism.Modularity;

namespace AutoTrade.Core.Modularity
{
    public static class ModuleDataExtensions
    {
        public static InitializationMode GetInitializationMode(this IModuleData moduleData)
        {
            if (moduleData.IsLoadedOnStartup)
                return InitializationMode.OnDemand;

            return InitializationMode.WhenAvailable;
        }
    }
}
