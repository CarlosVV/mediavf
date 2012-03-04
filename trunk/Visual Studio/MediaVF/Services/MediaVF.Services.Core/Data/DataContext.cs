using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

using MediaVF.Services.Core.Configuration;
using Microsoft.Practices.Unity;
using System.Collections;
using System.Xml;
using System.Transactions;

namespace MediaVF.Services.Core.Data
{
    #region DataContext

    public class DataContext
    {
        #region TableData

        /// <summary>
        /// Contains data for a table object
        /// </summary>
        public class TableData
        {
            #region TableAssociation

            public class TableAssociation
            {
                #region Parent

                public TableData ParentTable { get; set; }

                public string LinkedColumnOnParent { get; set; }

                public string LinkedPropertyOnParent { get; set; }

                public PropertyInfo ParentProperty
                {
                    get
                    {
                        PropertyInfo parentProperty = null;
                        if (!string.IsNullOrEmpty(LinkedPropertyOnParent) && ParentTable != null && ParentTable.ObjectType != null)
                            parentProperty = ParentTable.ObjectType.GetProperty(LinkedPropertyOnParent);

                        return parentProperty;
                    }
                }

                #endregion

                #region Child

                public TableData ChildTable { get; set; }

                public string LinkedColumnOnChild { get; set; }

                public string LinkedPropertyOnChild { get; set; }

                public PropertyInfo ChildProperty
                {
                    get
                    {
                        PropertyInfo childProperty = null;
                        if (!string.IsNullOrEmpty(LinkedPropertyOnChild) && ChildTable != null && ChildTable.ObjectType != null)
                            childProperty = ChildTable.ObjectType.GetProperty(LinkedPropertyOnChild);

                        return childProperty;
                    }
                }

                #endregion

                #region Constructors

                public TableAssociation(TableData parentTable,
                    TableData childTable)
                    : this(parentTable.KeyColumn,
                        parentTable.ObjectType.Name,
                        parentTable,
                        childTable.KeyColumn,
                        childTable.ObjectType.Name,
                        childTable) { }

                public TableAssociation(string linkedColumnOnParent,
                    TableData parentTable,
                    string linkedColumnOnChild,
                    TableData childTable)
                    : this(linkedColumnOnParent,
                        parentTable.ObjectType.Name,
                        parentTable,
                        linkedColumnOnChild,
                        childTable.ObjectType.Name,
                        childTable) { }

                public TableAssociation(string linkedColumnOnParent,
                    string linkedPropertyOnParent,
                    TableData parentTable,
                    string linkedColumnOnChild,
                    string linkedPropertyOnChild,
                    TableData childTable)
                {
                    LinkedColumnOnParent = linkedColumnOnParent;
                    LinkedPropertyOnParent = linkedPropertyOnParent;
                    ParentTable = parentTable;

                    LinkedColumnOnChild = linkedColumnOnChild;
                    LinkedPropertyOnChild = LinkedPropertyOnChild;
                    ChildTable = childTable;
                }

                #endregion
            }

            #endregion

            private const string COLUMN_NAME_COL = "COLUMN_NAME";
            private const string COLUMN_NAME_FILTER = COLUMN_NAME_COL + " = '{0}'";

            /// <summary>
            /// Name of the table
            /// </summary>
            public string TableName { get; set; }

            /// <summary>
            /// Name of the table's id column
            /// </summary>
            public string KeyColumn { get; set; }

            public Type ObjectType { get; set; }

            public bool CacheData { get; set; }

            public PropertyInfo KeyProperty
            {
                get
                {
                    if (!string.IsNullOrEmpty(KeyColumn) && ColumnMappings.ContainsKey(KeyColumn))
                        return ColumnMappings[KeyColumn];
                    else
                        return null;
                }
            }

            /// <summary>
            /// Mapping of column names to properties
            /// </summary>
            Dictionary<string, PropertyInfo> _columnMappings;
            public Dictionary<string, PropertyInfo> ColumnMappings
            {
                get
                {
                    if (_columnMappings == null)
                        _columnMappings = new Dictionary<string, PropertyInfo>();
                    return _columnMappings;
                }
            }

            List<TableAssociation> _linkedTables;
            public List<TableAssociation> LinkedTables
            {
                get
                {
                    if (_linkedTables == null)
                        _linkedTables = new List<TableAssociation>();
                    return _linkedTables;
                }
            }

            public TableData(DataContext context, DataTypeMappingElement mappingConfig)
            {
                TableName = mappingConfig.TableName;
                ObjectType = mappingConfig.ObjectType;

                DataTable schemaTable = context.SchemaTables[mappingConfig];

                if (!string.IsNullOrEmpty(mappingConfig.IDColumn))
                {
                    DataRow idColumnRow = schemaTable.Select(string.Format(COLUMN_NAME_FILTER, mappingConfig.IDColumn)).FirstOrDefault();
                    if (idColumnRow != null)
                        KeyColumn = mappingConfig.IDColumn;
                }
                else
                    KeyColumn = Constants.DEFAULT_ID_COLUMN;

                CacheData = mappingConfig.CacheData;

                bool idColumnMapped = false;
                schemaTable.Rows.Cast<DataRow>().ToList().ForEach(columnDataRow =>
                {
                    string columnName = columnDataRow[COLUMN_NAME_COL].ToString();
                    PropertyMappingElement columnPropMapping =
                        mappingConfig.PropertyMappings.FirstOrDefault(propMapping => string.Compare(propMapping.ColumnName, columnName, true) == 0);
                    if (columnPropMapping != null)
                    {
                        PropertyInfo propInfo = mappingConfig.ObjectType.GetProperty(columnPropMapping.PropertyName);
                        if (propInfo != null)
                        {
                            if (columnName == KeyColumn)
                                idColumnMapped = true;
                            ColumnMappings.Add(columnName, propInfo);
                        }
                    }
                    else
                    {
                        string columNameWithoutUnderscores = columnName.Replace("_", "");
                        PropertyInfo propInfo = mappingConfig.ObjectType.GetProperties().FirstOrDefault(propMatch =>
                            string.Compare(propMatch.Name, columNameWithoutUnderscores, true) == 0);
                        if (propInfo != null)
                        {
                            if (columnName == KeyColumn)
                                idColumnMapped = true;
                            ColumnMappings.Add(columnName, propInfo);
                        }
                    }
                });

                MapChildTables(context, mappingConfig);

                // TODO: throw invalid column mapping exceptions here
            }

            private void MapChildTables(DataContext context, DataTypeMappingElement mappingConfig)
            {
                foreach (ChildDataTypeMappingElement childMapping in mappingConfig.ChildTableMappings)
                {
                    if (context.SchemaTables.ContainsKey(childMapping))
                    {
                        TableData childTable = new TableData(context, childMapping);

                        TableAssociation tableAssociation = new TableAssociation(
                            !string.IsNullOrEmpty(childMapping.LinkedColumnOnParent) ? childMapping.LinkedColumnOnParent : Constants.DEFAULT_ID_COLUMN,
                            !string.IsNullOrEmpty(childMapping.LinkedPropertyOnParent) ? childMapping.LinkedPropertyOnParent : childMapping.ObjectTypeName,
                            this,
                            !string.IsNullOrEmpty(childMapping.LinkedColumnOnChild) ? childMapping.LinkedColumnOnChild : Constants.DEFAULT_ID_COLUMN,
                            !string.IsNullOrEmpty(childMapping.LinkedPropertyOnChild) ? childMapping.LinkedPropertyOnChild : mappingConfig.ObjectTypeName,
                            childTable);

                        LinkedTables.Add(tableAssociation);
                    }
                }
            }

            public string GetKey<T>(T obj)
            {
                if (ColumnMappings.ContainsKey(KeyColumn))
                {
                    object keyValue = ColumnMappings[KeyColumn].GetValue(obj, null);
                    if (keyValue != null)
                        return keyValue.ToString();
                }

                return null;
            }
        }

        #endregion TableData

        #region Properties

        /// <summary>
        /// Component container
        /// </summary>
        IUnityContainer ComponentContainer { get; set; }

        /// <summary>
        /// Name of context
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Database connection string
        /// </summary>
        public string DBConnectionString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Dictionary<DataTypeMappingElement, DataTable> _schemaTables;
        protected Dictionary<DataTypeMappingElement, DataTable> SchemaTables
        {
            get
            {
                if (_schemaTables == null)
                    _schemaTables = new Dictionary<DataTypeMappingElement, DataTable>();
                return _schemaTables;
            }
        }

        Dictionary<Type, TableData> _tables;
        protected Dictionary<Type, TableData> Tables
        {
            get
            {
                if (_tables == null)
                    _tables = new Dictionary<Type, TableData>();
                return _tables;
            }
        }

        #endregion

        #region Constructors

        public DataContext(IUnityContainer container, string connectionName, string connectionString)
        {
            ComponentContainer = container;
            Name = connectionName;
            DBConnectionString = connectionString;
        }

        #endregion

        #region Methods

        public void UpdateFromConfig(DataContextElement dataContextConfig)
        {
            List<DataTypeMappingElement> newTypeMappings = new List<DataTypeMappingElement>();
            List<DataTypeMappingElement> parentTypeMappings = dataContextConfig.TypeMappings.Where(typeMapping => !Tables.ContainsKey(typeMapping.ObjectType)).ToList();

            newTypeMappings.AddRange(parentTypeMappings);

            parentTypeMappings.ForEach(parentTypeMapping => AddChildTypeMappings(newTypeMappings, parentTypeMapping));

            if (newTypeMappings != null)
            {
                using (SqlConnection sqlConn = new SqlConnection(DBConnectionString))
                {
                    sqlConn.Open();

                    newTypeMappings.ForEach(typeMapping =>
                        SchemaTables.Add(typeMapping, sqlConn.GetSchema("Columns", new string[] { sqlConn.Database, null, typeMapping.TableName, null })));
                }

                // add table data for each config and schema pair
                if (SchemaTables != null)
                    newTypeMappings.ForEach(typeMapping =>
                    {
                        if (SchemaTables.ContainsKey(typeMapping))
                            Tables.Add(typeMapping.ObjectType, new TableData(this, typeMapping));
                    });
            }
        }

        private void AddChildTypeMappings(List<DataTypeMappingElement> typeMappings, DataTypeMappingElement mappingElement)
        {
            if (mappingElement != null && mappingElement.ChildTableMappings != null)
            {
                foreach (ChildDataTypeMappingElement childTypeMapping in mappingElement.ChildTableMappings.Where(typeMapping => !Tables.ContainsKey(typeMapping.ObjectType)))
                {
                    typeMappings.Add(childTypeMapping);

                    AddChildTypeMappings(typeMappings, childTypeMapping);
                }
            }
        }

        #region Save

        public enum ChangeType
        {
            Add,
            Update,
            Delete
        }

        Dictionary<ChangeType, List<object>> _changes;
        public Dictionary<ChangeType, List<object>> Changes
        {
            get
            {
                if (_changes == null)
                {
                    _changes = new Dictionary<ChangeType, List<object>>();
                    _changes.Add(ChangeType.Add, new List<object>());
                    _changes.Add(ChangeType.Update, new List<object>());
                    _changes.Add(ChangeType.Delete, new List<object>());
                }
                return _changes;
            }
        }

        public void Save()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                StringBuilder persistScript = new StringBuilder();

                // add insert portion to script
                Dictionary<Guid, KeyValuePair<PropertyInfo, object>> inserts = AddInsertScript(persistScript);

                #region Updates

                foreach (object updateObj in Changes[ChangeType.Update])
                {
                    if (updateObj != null)
                    {
                        Type updateType = updateObj.GetType();
                        if (Tables.ContainsKey(updateType))
                        {
                            TableData tableData = Tables[updateType];

                            // Add Update script building

                            // invalidate cache data
                            if (CachedData.ContainsKey(updateType))
                                CachedData.Remove(updateType);
                        }
                    }
                }

                #endregion

                #region Deletes

                foreach (object deleteObj in Changes[ChangeType.Delete])
                {
                    if (deleteObj != null)
                    {
                        Type deleteType = deleteObj.GetType();
                        if (Tables.ContainsKey(deleteType))
                        {
                            TableData tableData = Tables[deleteType];

                            // Add Delete script building

                            // invalidate cache data
                            if (CachedData.ContainsKey(deleteType))
                                CachedData.Remove(deleteType);
                        }
                    }
                }

                #endregion

                ExecutePersist(persistScript.ToString(), inserts);

                transaction.Complete();
            }

            Changes.Clear();
        }

        public Dictionary<Guid, KeyValuePair<PropertyInfo, object>> AddInsertScript(StringBuilder persistScript)
        {
            Dictionary<Guid, KeyValuePair<PropertyInfo, object>> inserts = new Dictionary<Guid, KeyValuePair<PropertyInfo, object>>();
            foreach (object addObj in Changes[ChangeType.Add])
            {
                if (addObj != null)
                {
                    Type addType = addObj.GetType();
                    if (Tables.ContainsKey(addType))
                    {
                        TableData tableData = Tables[addType];
                        Dictionary<string, object> columnValues =
                            tableData.ColumnMappings.Where(kvp => string.Compare(kvp.Key, tableData.KeyColumn, true) != 0)
                                                    .ToDictionary(cm => cm.Key, cm => cm.Value.GetValue(addObj, null));

                        if (inserts.Count == 0)
                            persistScript.Append(SQLBuilder.OutputTableScript);

                        Guid insertKey = Guid.NewGuid();
                        persistScript.AppendLine(SQLBuilder.BuildInsert(tableData.TableName, insertKey.ToString(), tableData.KeyColumn, columnValues));

                        inserts.Add(insertKey, new KeyValuePair<PropertyInfo, object>(tableData.KeyProperty, addObj));

                        // invalidate cache data
                        if (CachedData.ContainsKey(addType))
                            CachedData.Remove(addType);
                    }
                }
            }

            if (inserts.Count > 0)
                persistScript.AppendLine(SQLBuilder.OutputSelectScript);

            return inserts;
        }

        #endregion

        #region Cache

        Dictionary<Type, IList> _cachedData;
        public Dictionary<Type, IList> CachedData
        {
            get
            {
                if (_cachedData == null)
                    _cachedData = new Dictionary<Type, IList>();
                return _cachedData;
            }
        }

        public void CacheAllTables()
        {
            CachedData.Clear();

            foreach (Type t in Tables.Keys)
            {
                Type objListType = typeof(List<>).MakeGenericType(t);
                IList objList = Activator.CreateInstance(objListType) as IList;

                MethodInfo getMethod = GetType().GetMethod("GetAll", Type.EmptyTypes).MakeGenericMethod(t);
                objList = getMethod.Invoke(this, null) as IList;

                CachedData.Add(t, objList);
            }
        }

        #endregion

        #region Get

        public string GetKey<T>(T obj)
        {
            if (Tables.ContainsKey(typeof(T)))
                return Tables[typeof(T)].GetKey<T>(obj);
            else if (obj != null)
                return obj.ToString();
            else
                return null;
        }
        
        public List<T> GetAll<T>()
        {
            return GetAll<T>(true);
        }

        public List<T> GetAll<T>(bool loadChildren)
        {
            List<T> objects = null;
            if (Tables.ContainsKey(typeof(T)))
            {
                if (Tables[typeof(T)].CacheData && CachedData.ContainsKey(typeof(T)))
                    objects = CachedData[typeof(T)] as List<T>;
                else
                    objects = ExecuteSelect<T>(SQLBuilder.BuildSelect(Tables[typeof(T)].TableName));
            }

            return objects;
        }

        public List<T> GetByModuleID<T>(int moduleID)
        {
            return GetByModuleID<T>(moduleID, true);
        }

        public List<T> GetByModuleID<T>(int moduleID, bool loadChildren)
        {
            if (Tables.ContainsKey(typeof(T)))
            {
                TableData tableData = Tables[typeof(T)];
                string sql = SQLBuilder.BuildSelectByModuleID(tableData.TableName, moduleID);
                if (loadChildren && tableData.LinkedTables != null && tableData.LinkedTables.Count > 0)
                {
                    foreach (TableData.TableAssociation tableAssociation in tableData.LinkedTables)
                        sql += string.Format("{0}{0}{1}",
                            Environment.NewLine,
                            SQLBuilder.BuildLinkedTableSelectByModuleID(tableAssociation.ParentTable.TableName,
                                tableAssociation.LinkedColumnOnParent,
                                tableAssociation.ChildTable.TableName,
                                tableAssociation.LinkedColumnOnChild,
                                moduleID));
                }

                return ExecuteSelect<T>(sql);
            }
            else
                return null;
        }

        public void Get()
        {
        }

        #endregion

        #region Add

        public void AddObjects<T>(List<T> objects)
        {
            objects.ForEach(obj =>
            {
                Changes[ChangeType.Add].Add(obj);
            });
        }

        #endregion

        #region Data Mapping

        private List<T> MapDataToObjects<T>(DataSet results)
        {
            Dictionary<object, T> resultList = null;

            if (Tables.ContainsKey(typeof(T)) && results != null && results.Tables != null && results.Tables.Count > 0)
            {
                TableData primaryTable = Tables[typeof(T)];

                resultList = new Dictionary<object, T>();
                DataTable primaryResults = results.Tables[0];
                foreach (DataRow dataRow in primaryResults.Rows)
                {
                    object key = dataRow[primaryTable.KeyColumn];
                    T result = MapRowToObject<T>(dataRow);
                    resultList.Add(key, result);
                }

                if (results.Tables.Count > 1 && primaryTable.LinkedTables.Count > 0)
                {
                    for (int i = 1; i < results.Tables.Count; i++)
                    {
                        DataTable currentTable = results.Tables[i];
                        TableData.TableAssociation tableAssociation = primaryTable.LinkedTables[i - 1];

                        if (Tables.ContainsKey(tableAssociation.ChildTable.ObjectType))
                        {
                            Type childType = tableAssociation.ChildTable.ObjectType;
                            foreach (DataRow childRow in currentTable.Rows)
                            {
                                object parentKey = childRow[tableAssociation.LinkedColumnOnChild];

                                if (resultList.ContainsKey(parentKey))
                                {
                                    T parent = resultList[parentKey];
                                    object child = MapRowToObject(childType, childRow);

                                    if (tableAssociation.ParentProperty != null)
                                        SetValueOnLinkedProperty(parent, tableAssociation.ParentProperty, childType, child);

                                    if (tableAssociation.ChildProperty != null)
                                        SetValueOnLinkedProperty(child, tableAssociation.ChildProperty, typeof(T), parent);
                                }
                            }
                        }
                    }
                }
            }

            return resultList.Values.ToList();
        }

        private void SetValueOnLinkedProperty(object obj, PropertyInfo property, Type valueType, object value)
        {
            if (typeof(IList).IsAssignableFrom(property.PropertyType) &&
                (!property.PropertyType.IsGenericType ||
                  property.PropertyType.GetGenericArguments().First() == valueType))
            {
                IList list = (IList)property.GetGetMethod().Invoke(obj, null);
                list.Add(value);
            }
            else if (property.PropertyType.IsAssignableFrom(valueType))
                property.SetValue(obj, value, null);
        }

        private object MapRowToObject(Type type, DataRow row)
        {
            object obj = null;
            MethodInfo genericBaseMethod = GetType().GetMethod(MethodBase.GetCurrentMethod().Name,
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new Type[] { typeof(DataRow) },
                null);

            if (genericBaseMethod != null)
            {
                MethodInfo genericMethod = genericBaseMethod.MakeGenericMethod(type);
                if (genericMethod != null)
                    obj = genericMethod.Invoke(this, new object[] { row });
            }

            return obj;
        }

        private T MapRowToObject<T>(DataRow row)
        {
            T resultObj = default(T);

            if (Tables.ContainsKey(typeof(T)) && row != null)
            {
                resultObj = ComponentContainer.Resolve<T>();

                TableData table = Tables[typeof(T)];

                table.ColumnMappings.ToList().ForEach(columAndPropMapping =>
                {
                    if (row.Table.Columns.Contains(columAndPropMapping.Key))
                    {
                        object objValue = row[columAndPropMapping.Key];

                        if (columAndPropMapping.Value.PropertyType == objValue.GetType())
                            columAndPropMapping.Value.SetValue(resultObj, objValue, null);
                    }
                });
            }

            return resultObj;
        }

        #endregion

        #region Execute SQL

        private List<T> ExecuteSelect<T>(string selectSql)
        {
            DataSet resultSet = null;

            using (SqlConnection connection = new SqlConnection(DBConnectionString))
            {
                connection.Open();

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(selectSql, connection))
                {
                    resultSet = new DataSet();
                    dataAdapter.Fill(resultSet);
                }
            }

            List<T> objects = null;
            if (resultSet != null)
                objects = MapDataToObjects<T>(resultSet);

            if (Tables[typeof(T)].CacheData)
            {
                IList cached;
                if (!CachedData.ContainsKey(typeof(T)))
                    CachedData.Add(typeof(T), objects.ToList());
                else
                {
                    cached = CachedData[typeof(T)];
                    foreach (T obj in objects)
                        if (!((IEnumerable<T>)cached).Any(item => Tables[typeof(T)].GetKey((T)item) == Tables[typeof(T)].GetKey(obj)))
                            cached.Add(obj);
                }
            }

            return objects;
        }

        private void ExecutePersist(string persistSql, Dictionary<Guid, KeyValuePair<PropertyInfo, object>> inserts)
        {
            if (!string.IsNullOrEmpty(persistSql))
            {
                using (SqlConnection connection = new SqlConnection(DBConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(persistSql, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Guid insertKey;
                            if (Guid.TryParse(reader[0].ToString(), out insertKey) && inserts.ContainsKey(insertKey))
                            {
                                PropertyInfo keyProperty = inserts[insertKey].Key;
                                keyProperty.SetValue(inserts[insertKey].Value, reader[1], null);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #endregion
    }

    #endregion
}
