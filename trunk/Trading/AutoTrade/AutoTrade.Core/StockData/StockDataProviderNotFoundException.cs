using System;
using AutoTrade.Core.Properties;

namespace AutoTrade.Core.StockData
{
    public class StockDataProviderNotFoundException : Exception
    {
        #region Fields

        /// <summary>
        /// The type not found
        /// </summary>
        private readonly Type _type;

        /// <summary>
        /// The name not found
        /// </summary>
        private readonly string _name;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="StockDataProviderNotFoundException"/>
        /// </summary>
        /// <param name="type"></param>
        public StockDataProviderNotFoundException(Type type)
        {
            _type = type;
        }

        /// <summary>
        /// Instantiates a <see cref="StockDataProviderNotFoundException"/>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public StockDataProviderNotFoundException(string name)
        {
            _name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the message indicating the type not found
        /// </summary>
        public override string Message
        {
            get
            {
                return _type != null ?
                    string.Format(Resources.StockDataProviderTypeNotRegisteredMessage, _type.FullName) :
                    string.Format(Resources.StockDataProviderNotFoundMessage, _name);
            }
        }

        #endregion
    }
}