using System;
using AutoTrade.MarketData.Yahoo.Properties;

namespace AutoTrade.MarketData.Yahoo.Csv.Exceptions
{
    public class InvalidCsvFormatException : Exception
    {
        /// <summary>
        /// The details of the problem with the CSV format
        /// </summary>
        private readonly string _details;

        /// <summary>
        /// Instantiates a <see cref="InvalidCsvFormatException"/>
        /// </summary>
        /// <param name="details"></param>
        public InvalidCsvFormatException(string details)
        {
            _details = details;
        }

        /// <summary>
        /// Gets the message indicating what is wrong with the CSV format
        /// </summary>
        public override string Message
        {
            get { return string.Format(Resources.InvalidCsvFormatMessageFormat, _details); }
        }
    }
}
