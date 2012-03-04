using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MediaVF.Services.Core.Utilities
{
    public static class EnumHelper
    {
        #region Descriptions

        public static string ToDescription(this Enum enumItem)
        {
            EnumStringAttribute attribute = GetAttribute<EnumStringAttribute>(enumItem);

            return attribute != null && !string.IsNullOrEmpty(attribute.Description) ? attribute.Description : enumItem.ToString();
        }

        public static string ToCommaDelimitedDescriptionList(this Type t, bool addSpaceAfterCommas)
        {
            List<EnumStringAttribute> attributes = GetAttributes<EnumStringAttribute>(t);

            StringBuilder sb = new StringBuilder();
            attributes.ForEach(attribute =>
                sb.Append(sb.Length > 0 ? "," : "")
                  .Append(sb.Length > 0 && addSpaceAfterCommas ? " " : "")
                  .Append(attribute.Description));

            return sb.ToString();
        }

        #endregion Descriptions

        #region Abbreviations

        public static string ToAbbreviation(this Enum enumItem)
        {
            EnumStringAttribute attribute = GetAttribute<EnumStringAttribute>(enumItem);

            return attribute != null ? attribute.Abbreviation : string.Empty;
        }

        public static string ToCommaDelimitedAbbreviationList(this Type t, bool addSpaceAfterCommas)
        {
            List<EnumStringAttribute> attributes = GetAttributes<EnumStringAttribute>(t);
            
            StringBuilder sb = new StringBuilder();
            attributes.ForEach(attribute =>
                sb.Append(sb.Length > 0 ? "," : "")
                  .Append(sb.Length > 0 && addSpaceAfterCommas ? " " : "")
                  .Append(attribute.Abbreviation));

            return sb.ToString();
        }

        #endregion Abbreviations

        #region Get Attributes

        private static List<T> GetAttributes<T>(Type t)
        {
            return t.GetFields().ToList().ConvertAll(field => GetAttribute<T>(field)).Where(attribute => attribute != null).ToList();
        }

        private static T GetAttribute<T>(Enum enumItem) where T : Attribute
        {
            Type enumType = enumItem.GetType();
            FieldInfo field = enumType.GetField(enumItem.ToString());

            return GetAttribute<T>(field);
        }

        private static T GetAttribute<T>(FieldInfo field)
        {
            if (field != null)
            {
                List<T> attributes = field.GetCustomAttributes(typeof(T), true).Cast<T>().ToList();

                if (attributes != null && attributes.Count > 0)
                    return attributes[0];
            }

            return default(T);
        }

        #endregion Get Attributes
    }
}
