using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.Practices.Prism.Modularity;

using MediaVF.Services.Configuration;
using System.Reflection.Emit;

namespace MediaVF.Services.Modularity
{
    #region DatabaseModuleCatalog

    public class DatabaseModuleCatalog : ModuleCatalog
    {

        const string MODULE_SELECT = @"
SELECT
    md.ModuleDirectory,
    md.CheckSubfolders,
    m.ID,
    m.AssemblyName,
    m.Class,
    m.ModuleName
FROM
    ModuleDirectory md
INNER JOIN Module m ON
    md.ID = m.ModuleDirectoryID
INNER JOIN Application a ON
    md.ApplicationID = a.ID
WHERE
    a.ID = {0} AND
    m.Active = 1";

        IServiceConfigManager ConfigManager { get; set; }

        Dictionary<ModuleInfo, ModuleData> ModuleData { get; set; }

        public DatabaseModuleCatalog(IServiceConfigManager configManager)
        {
            ConfigManager = configManager;
        }

        protected override void InnerLoad()
        {
            // get directories and active modules from the database
            Dictionary<string, Dictionary<string, List<ModuleData>>> moduleDirectories = GetModuleDirectories();

            // reflection-only load into child domain for loaded assemblies
            List<string> loadedAssemblies = new List<string>();

            var assemblies = (
                                 from Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()
                                 where !(assembly is System.Reflection.Emit.AssemblyBuilder)
                                    && assembly.GetType().FullName != "System.Reflection.Emit.InternalAssemblyBuilder"
                                    && !String.IsNullOrEmpty(assembly.Location)
                                 select assembly.Location
                             );

            loadedAssemblies.AddRange(assemblies);

            // create child domain to do reflection-only loading of assemblies
            AppDomain childDomain = AppDomain.CreateDomain("ReflectionOnly",
                AppDomain.CurrentDomain.Evidence,
                AppDomain.CurrentDomain.SetupInformation);

            try
            {
                // create child domain module loader
                ChildDomainModuleInfoLoader moduleLoader =
                    (ChildDomainModuleInfoLoader)childDomain.CreateInstanceFrom(GetType().Assembly.Location, typeof(ChildDomainModuleInfoLoader).FullName).Unwrap();

                // add module infos found by module loader
                List<ModuleInfo> moduleInfos = moduleLoader.GetModuleInfos(loadedAssemblies.ToList(), moduleDirectories);
                moduleInfos.ForEach(moduleInfo => Items.Add(moduleInfo));
            }
            catch
            {
            }
            finally
            {
                AppDomain.Unload(childDomain);
            }
        }

        /// <summary>
        /// Gets active module directories, assemblies, and modules from the database
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, Dictionary<string, List<ModuleData>>> GetModuleDirectories()
        {
            Dictionary<string, Dictionary<string, List<ModuleData>>> moduleDirectories = new Dictionary<string, Dictionary<string, List<ModuleData>>>();

            int applicationID = ConfigManager.ApplicationID;

            // get directories, assemblies, and module classes
            using (SqlConnection connection = new SqlConnection(ConfigManager.ComponentDBConnectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(string.Format(MODULE_SELECT, applicationID), connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // TODO: Try to resolve both relative and absolute paths
                        string directory = ResolveDirectoryPath(reader["ModuleDirectory"].ToString());
                        if (!string.IsNullOrEmpty(directory))
                        {
                            // add module directory
                            if (!moduleDirectories.ContainsKey(directory))
                                moduleDirectories.Add(directory, new Dictionary<string, List<ModuleData>>());

                            // add assembly for directory
                            string assemblyName = reader["AssemblyName"].ToString();
                            if (!moduleDirectories[directory].ContainsKey(assemblyName))
                                moduleDirectories[directory].Add(assemblyName, new List<ModuleData>());

                            // add class for assembly
                            string className = reader["Class"].ToString();
                            int moduleID = (int)reader["ID"];
                            string moduleName = reader["ModuleName"].ToString();
                            moduleDirectories[directory][assemblyName].Add(new ModuleData() { ModuleID = moduleID, ModuleName = moduleName, Class = className });
                        }
                    }
                }
            }

            return moduleDirectories;
        }

        /// <summary>
        /// Attempts to resolve the path to a directory by checking both relative and absolute paths
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        private string ResolveDirectoryPath(string directory)
        {
            // check absolute path
            if (Directory.Exists(directory))
                return directory;
            // check relative path
            else if (Directory.Exists(Path.GetFullPath(directory)))
                return Path.GetFullPath(directory);
            // directory not found
            else
                return string.Empty;
        }
    }

    #endregion

    #region ChildDomainModuleInfoLoader

    #endregion
}
