using System;
using System.IO;
using System.Linq;
using Microsoft.Practices.Prism.Modularity;

namespace AutoTrade.Core.Modularity
{
    public class RepositoryModuleCatalog : ModuleCatalog
    {
        #region Fields

        /// <summary>
        /// The repository containing module data
        /// </summary>
        private readonly IModuleDataRepository _moduleDataRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="RepositoryModuleCatalog"/>
        /// </summary>
        /// <param name="moduleDataRepository"></param>
        public RepositoryModuleCatalog(IModuleDataRepository moduleDataRepository)
        {
            if (moduleDataRepository == null)
                throw new ArgumentNullException("moduleDataRepository");

            _moduleDataRepository = moduleDataRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads module data from a repository
        /// </summary>
        protected sealed override void InnerLoad()
        {
            // load module collection from repository
            var moduleDataCollection = _moduleDataRepository.GetModuleData();

            // ensure collection is not null
            if (moduleDataCollection != null) moduleDataCollection.ForEach(AddModule);
        }

        /// <summary>
        /// Adds a module from module data
        /// </summary>
        /// <param name="moduleData"></param>
        protected void AddModule(IModuleData moduleData)
        {
            AddModule(moduleData.ModuleName,
             moduleData.ModuleType,
             CreateFileUri(moduleData.AssemblyPath),
             moduleData.IsLoadedOnStartup
                 ? InitializationMode.WhenAvailable
                 : InitializationMode.OnDemand,
             moduleData.DependsOn.ToArray());
        }

        /// <summary>
        /// Creates a file uri
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string CreateFileUri(string path)
        {
            return new Uri(Path.GetFullPath(path)).AbsoluteUri;
        }

        #endregion
    }
}
