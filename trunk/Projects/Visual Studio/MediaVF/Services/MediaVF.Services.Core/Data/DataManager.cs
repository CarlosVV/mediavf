using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.Practices.Prism.Modularity;

using MediaVF.Services.Core.Configuration;
using MediaVF.Services.Core.Logging;
using MediaVF.Services.Core.Components;
using System.Data;
using Microsoft.Practices.Unity;

namespace MediaVF.Services.Core.Data
{
    public class DataManager : IDataManager
    {
        #region Properties

        /// <summary>
        /// Component container
        /// </summary>
        IUnityContainer ComponentContainer { get; set; }

        /// <summary>
        /// Log
        /// </summary>
        IComboLog Log { get; set; }

        /// <summary>
        /// Log
        /// </summary>
        IServiceConfigManager ConfigManager { get; set; }

        DataContext SharedDataContext { get; set; }

        /// <summary>
        /// Mapping of types to table data
        /// </summary>
        Dictionary<string, DataContext> _dataContextsByName;
        private Dictionary<string, DataContext> DataContextsByName
        {
            get
            {
                if (_dataContextsByName == null)
                    _dataContextsByName = new Dictionary<string, DataContext>();
                return _dataContextsByName;
            }
        }

        /// <summary>
        /// Mapping of types to table data
        /// </summary>
        Dictionary<Type, DataContext> _dataContextsByType;
        private Dictionary<Type, DataContext> DataContextsByType
        {
            get
            {
                if (_dataContextsByType == null)
                    _dataContextsByType = new Dictionary<Type, DataContext>();
                return _dataContextsByType;
            }
        }

        #endregion Properties

        #region Constructors
        
        public DataManager(IUnityContainer componentContainer, IComboLog log, IServiceConfigManager configManager)
        {
            ComponentContainer = componentContainer;
            Log = log;
            ConfigManager = configManager;

            SharedDataContextElement sharedDataContextSection = ConfigManager.Configuration.SharedDataContext as SharedDataContextElement;
            if (sharedDataContextSection != null)
            {
                Assembly entityAssembly = null;
                try
                {
                    AssemblyName assemblyName = new AssemblyName(sharedDataContextSection.Assembly.FullName);
                    entityAssembly = Assembly.Load(assemblyName);
                }
                catch { }

                if (entityAssembly != null)
                {
                    SharedDataContext = new DataContext(ComponentContainer, sharedDataContextSection.DataContext.ConnectionString.Name, sharedDataContextSection.DataContext.ConnectionString.ConnectionString);
                    SharedDataContext.UpdateFromConfig(sharedDataContextSection.DataContext);

                    SharedDataContext.CacheAllTables();
                }
            }
        }

        #endregion Constructors

        #region IDataManager Implementation

        /// <summary>
        /// Register table data for given type - if type is not a valid data type, throw an exception
        /// </summary>
        public void AddDataContext(DataContextElement dataContextConfig)
        {
            if (!DataContextsByName.ContainsKey(dataContextConfig.ConnectionString.Name))
                DataContextsByName.Add(dataContextConfig.ConnectionString.Name, new DataContext(ComponentContainer, dataContextConfig.ConnectionString.Name, dataContextConfig.ConnectionString.ConnectionString));

            DataContext dataContext = DataContextsByName[dataContextConfig.ConnectionString.Name];

            dataContext.UpdateFromConfig(dataContextConfig);

            RegisterTypes(dataContext, dataContextConfig.TypeMappings.Cast<DataTypeMappingElement>());
        }

        private void RegisterTypes(DataContext dataContext, IEnumerable<DataTypeMappingElement> mappings)
        {
            mappings.ToList().ForEach(typeMapping =>
            {
                if (!ComponentContainer.IsRegistered(typeMapping.ObjectType))
                    ComponentContainer.RegisterType(typeMapping.ObjectType, typeMapping.ObjectTypeName, new TransientLifetimeManager());
                if (!DataContextsByType.ContainsKey(typeMapping.ObjectType))
                    DataContextsByType.Add(typeMapping.ObjectType, dataContext);
                if (typeMapping.ChildTableMappings != null && typeMapping.ChildTableMappings.Count > 0)
                    RegisterTypes(dataContext, typeMapping.ChildTableMappings.Cast<DataTypeMappingElement>());
            });
        }

        /// <summary>
        /// Get all objects for a given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public DataContext GetDataContext<T>()
        {
            if (DataContextsByType.ContainsKey(typeof(T)))
                return DataContextsByType[typeof(T)];
            else
                return null;
        }

        public bool IsSharedCachedData<T>()
        {
            return IsSharedCachedData(typeof(T));
        }

        public bool IsSharedCachedData(Type t)
        {
            return SharedDataContext.CachedData.ContainsKey(t);
        }

        public List<T> GetSharedCachedData<T>()
        {
            return (List<T>)GetSharedCachedData(typeof(T));
        }

        public IList GetSharedCachedData(Type t)
        {
            if (SharedDataContext.CachedData.ContainsKey(t))
                return SharedDataContext.CachedData[t];
            else
                return null;
        }

        #endregion
    }
}
