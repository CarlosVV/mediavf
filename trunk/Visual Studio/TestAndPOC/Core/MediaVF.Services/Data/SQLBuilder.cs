using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.Services.Data
{
    public static class SQLBuilder
    {
        #region Select

        private const string SELECT = "SELECT {0} FROM [{1}]";

        private const string INNER_JOIN = "INNER JOIN [{0}] ON {1}";

        public static string BuildSelect(string tableName)
        {
            return BuildSelect(tableName, null);
        }

        public static string BuildSelect(string tableName, Dictionary<string, object> criteria)
        {
            StringBuilder select = new StringBuilder();
            select.AppendLine(string.Format(SELECT, "*", tableName));

            string whereClause = BuildWhereClause(tableName, criteria);
            if (!string.IsNullOrEmpty(whereClause))
                select.Append(whereClause);

            return select.ToString();
        }

        public static string BuildLinkedTableSelect(string table1Name, string table1Column, string table2Name, string table2Column, Dictionary<string, object> criteria)
        {
            Dictionary<string, string> columnMappings = new Dictionary<string, string>();
            columnMappings.Add(table1Column, table2Column);

            return BuildLinkedTableSelect(table1Name, table2Name, columnMappings, criteria);
        }

        public static string BuildLinkedTableSelect(string table1Name, string table2Name, Dictionary<string, string> table1ToTable2ColumnMappings, Dictionary<string, object> criteria)
        {
            StringBuilder select = new StringBuilder();
            select.AppendLine(string.Format(SELECT, "*", table1Name));

            string innerJoin = BuildInnerJoin(table1Name, table2Name, table1ToTable2ColumnMappings);
            if (!string.IsNullOrEmpty(innerJoin))
                select.AppendLine(innerJoin);

            string whereClause = BuildWhereClause(table1Name, criteria);
            if (!string.IsNullOrEmpty(whereClause))
                select.Append(whereClause);

            return select.ToString();
        }

        #endregion

        #region Filtering

        private const string WHERE = "WHERE {0}";

        public static string BuildInnerJoin(string table1Name, string table2Name, Dictionary<string, string> table1ToTable2Mappings)
        {
            StringBuilder onClause = new StringBuilder();
            foreach (KeyValuePair<string, string> columnMapping in table1ToTable2Mappings)
                onClause.Append(onClause.Length > 0 ? " AND " : "")
                    .Append("[").Append(table1Name).Append("]").Append(".").Append(columnMapping.Key)
                    .Append(" = ")
                    .Append("[").Append(table2Name).Append("]").Append(".").Append(columnMapping.Value);

            return string.Format(INNER_JOIN, table2Name, onClause);
        }

        public static string BuildWhereClause(string tableName, Dictionary<string, object> criteria)
        {
            StringBuilder whereClause = new StringBuilder();

            if (criteria != null && criteria.Count > 0)
            {
                foreach (KeyValuePair<string, object> whereFilter in criteria)
                {
                    string whereExpression = BuildWhereExpression(tableName, whereFilter.Key, whereFilter.Value);

                    // TODO: handle OR operator
                    if (!string.IsNullOrWhiteSpace(whereExpression))
                        whereClause.Append(whereClause.Length > 0 ? " AND " : "").Append(whereExpression);
                }
            }

            if (whereClause.Length > 0)
                return string.Format(WHERE, whereClause.ToString());
            else
                return string.Empty;
        }


        /// <summary>
        /// Builds a where expression for the given column and value
        /// </summary>
        /// <param name="columnName">The name of the column in the where expression</param>
        /// <param name="value">The value of the column to look for</param>
        /// <returns></returns>
        public static string BuildWhereExpression(string tableName, string columnName, object value)
        {
            string whereExpression = string.Empty;
            if (!string.IsNullOrWhiteSpace(columnName))
            {
                string valueString = "NULL";
                if (value != null)
                {
                    Type valueType = value.GetType();

                    // treat nullable types with values as their underlying type
                    bool nullableWithoutValue = false;
                    if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        nullableWithoutValue = value == null;
                        valueType = valueType.GetGenericArguments().First();
                    }

                    // if type is nullable and value is null, nothing needs to be done (default string is "NULL")
                    if (!nullableWithoutValue)
                    {
                        valueString = value.ToString();
                        if (valueType == typeof(DateTime) || valueType == typeof(string))
                            valueString = "'" + valueString + "'";
                    }
                }

                whereExpression = string.Format("{0}.{1} {2} {3}", tableName, columnName, valueString != "NULL" ? "=" : "IS", valueString);
            }

            return whereExpression;
        }

        #endregion

        #region Insert

        private const string INSERT = "INSERT INTO ";

        private const string OUTPUT = "OUTPUT";

        private const string INSERTED_PREFIX = "inserted.";

        private const string OUTPUT_TO = "INTO @insertOutput";

        private const string VALUES = "VALUES";

        public const string OutputTableScript = @"
DECLARE @insertOutput TABLE
(
    [insertionID]   varchar(100),
    [objectID]      int
)

";

        public const string OutputSelectScript = @"
SELECT
    [insertionID],
    [objectID]
FROM
    @insertOutput

";

        public static string BuildInsert(string tableName, string uniqueKey, string idColumn, Dictionary<string, object> columnValues)
        {
            StringBuilder columnNames = new StringBuilder();
            StringBuilder columnValuesText = new StringBuilder();
            for (int i = 0; i < columnValues.Count; i++)
            {
                columnNames.Append("\t").Append(i > 0 ? "," : " ").Append("[").Append(columnValues.ElementAt(i).Key).AppendLine("]");

                object columnValue = columnValues.ElementAt(i).Value;

                string columnValueText = GetValueAsSQLText(columnValue);

                columnValuesText.Append("\t").Append(i > 0 ? "," : " ").AppendLine(columnValueText);
            }

            StringBuilder insert = new StringBuilder();

            insert.Append(INSERT).Append("[").Append(tableName).AppendLine("]");

            insert.AppendLine("(");
            insert.Append(columnNames.ToString());
            insert.AppendLine(")");

            insert.AppendLine(OUTPUT);
            insert.Append("\t").Append("'").Append(uniqueKey).Append("'").AppendLine(", ");
            insert.Append("\t").Append(INSERTED_PREFIX).AppendLine(idColumn);
            insert.AppendLine(OUTPUT_TO);

            insert.AppendLine(VALUES);

            insert.AppendLine("(");
            insert.Append(columnValuesText.ToString());
            insert.AppendLine(")");

            return insert.ToString();
        }

        #endregion

        #region Stored Procs

        const string EXECUTE = "EXECUTE";

        public static string BuildStoredProcExecute(string procName, Dictionary<string, object> parameters)
        {
            StringBuilder execute = new StringBuilder();

            execute.Append(EXECUTE).Append(" ").AppendLine(procName);

            bool firstParameter = true;
            foreach (string parameterName in parameters.Keys)
            {
                execute.Append(firstParameter ? " " : ",").Append(parameterName).Append(" = ").AppendLine(GetValueAsSQLText(parameters[parameterName]));
                firstParameter = false;
            }

            return execute.ToString();
        }

        #endregion

        #region Helper

        private static string GetValueAsSQLText(object value)
        {
            string valueText = "NULL";
            if (value != null)
            {
                if (value.GetType() == typeof(string) || value.GetType() == typeof(DateTime) || value.GetType() == typeof(DateTime?))
                    valueText = "'" + value.ToString() + "'";
                else if (value.GetType() == typeof(bool))
                    valueText = (bool)value ? "1" : "0";
                else if (value.GetType() != typeof(DBNull))
                    valueText = value.ToString();
            }

            return valueText;
        }

        #endregion
    }
}
