using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MediaVF.Services.Core.Data
{
    public class InvalidColumnMappingException : Exception
    {
        public PropertyInfo Property { get; protected set; }
        public string ColumnName { get; protected set; }

        protected string InnerMessage { get; set; }

        public override string Message
        {
            get
            {
                return string.Format("Column mapping is not valid.{0}Name: {1}{0}Type: {2}{0}Column Name: {3}{4}{5}",
                    Environment.NewLine,
                    Property.Name,
                    Property.PropertyType.FullName,
                    ColumnName,
                    !string.IsNullOrEmpty(InnerMessage) ? ": " : "",
                    InnerMessage);
            }
        }

        public InvalidColumnMappingException(PropertyInfo property, string columnName)
        {
            Property = property;
            ColumnName = columnName;
        }

        public InvalidColumnMappingException(PropertyInfo property, string columnName, string message)
            : this(property, columnName)
        {
            InnerMessage = message;
        }
    }
}
