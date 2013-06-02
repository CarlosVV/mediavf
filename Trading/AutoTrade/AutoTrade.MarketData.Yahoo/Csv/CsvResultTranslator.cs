using System;
using System.Collections.Generic;
using System.Linq;
using AutoTrade.Core;
using AutoTrade.MarketData.Yahoo.Csv.Exceptions;
using AutoTrade.MarketData.Yahoo.Exceptions;
using AutoTrade.MarketData.Yahoo.Properties;

namespace AutoTrade.MarketData.Yahoo.Csv
{
    class CsvResultTranslator : ICsvResultTranslator
    {
        #region Fields

        /// <summary>
        /// The provider for columns in the CSV
        /// </summary>
        private readonly ICsvColumnProvider _columnProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="CsvResultTranslator"/>
        /// </summary>
        /// <param name="columnProvider"></param>
        public CsvResultTranslator(ICsvColumnProvider columnProvider)
        {
            _columnProvider = columnProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Translates the results of a CSV query into quotes
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public IEnumerable<StockQuote> TranslateResultsToQuotes(string response)
        {
            // check that the response is not empty
            if (string.IsNullOrWhiteSpace(response))
                throw new QueryResultsAreEmptyException();

            // parse the response
            var parsedResponse = ParseCsvResponse(response);

            // get the mappings of properties to column indexes
            var columnPropertyMappings = _columnProvider.GetProperties();

            // create quotes from data
            return parsedResponse.Select(row => CreateQuoteFromRow(row, columnPropertyMappings));
        }

        /// <summary>
        /// Parses the CSV response into a list of rows and values
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private IEnumerable<List<string>> ParseCsvResponse(string response)
        {
            // create list
            var rowsAndCols = new List<List<string>>();

            // split on new lines to get rows
            var rows = response.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            // if no data, return empty list
            if (rows.Length == 0)
                return rowsAndCols;

            foreach (var columns in rows.Select(row => row.Split(',')))
            {
                // check that the number of columns is the expected
                if (columns.Length < _columnProvider.EnabledColumnCount)
                    throw new InvalidCsvFormatException(Resources.CsvMissingColumnsMessage);

                // add to collection
                rowsAndCols.Add(columns.ToList());
            }

            return rowsAndCols;
        }

        /// <summary>
        /// Creates a stock quote from a row in the CSV
        /// </summary>
        /// <param name="values"></param>
        /// <param name="columnPropertyMappings"></param>
        /// <returns></returns>
        private static StockQuote CreateQuoteFromRow(List<string> values, Dictionary<int, string> columnPropertyMappings)
        {
            // create new quote
            var quote = new StockQuote();

            // go through all values that are mapped to a property and populate the property from the value in the list
            foreach (var value in values.Where(v => columnPropertyMappings.ContainsKey(values.IndexOf(v))))
                PopulatePropertyWithValue(quote, columnPropertyMappings[values.IndexOf(value)], value);

            return quote;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quote"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        private static void PopulatePropertyWithValue(StockQuote quote, string propertyName, string value)
        {
            // get the property
            var property = quote.GetType().GetProperty(propertyName);

            // if the property was found, set its value
            if (property != null)
                property.SetValue(quote, value.ConvertTo(property.PropertyType), null);
        }

        #endregion
    }
}
