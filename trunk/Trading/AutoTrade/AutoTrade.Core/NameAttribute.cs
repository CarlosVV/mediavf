using System;
using System.Linq;

namespace AutoTrade.Core
{
    [AttributeUsage(AttributeTargets.All)]
    public class NameAttribute : Attribute
    {
        #region Static

        /// <summary>
        /// Gets the name attribute from an object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static NameAttribute Get(object obj)
        {
            if (obj == null) return null;

            return Get(obj.GetType());
        }

        /// <summary>
        /// Gets the name attribute for a type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static NameAttribute Get<T>()
        {
            return Get(typeof(T));
        }

        /// <summary>
        /// Gets the name attribute for a type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static NameAttribute Get(Type type)
        {
            return (NameAttribute)type.GetCustomAttributes(typeof(NameAttribute), true).FirstOrDefault();
        }

        #endregion

        #region Fields

        /// <summary>
        /// The name
        /// </summary>
        private readonly string _name;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="NameAttribute"/>
        /// </summary>
        /// <param name="name"></param>
        public NameAttribute(string name)
        {
            _name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        #endregion
    }
}
