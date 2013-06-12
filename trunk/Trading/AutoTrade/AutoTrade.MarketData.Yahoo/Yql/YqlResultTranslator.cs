using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using AutoTrade.Core;
using AutoTrade.Core.Modularity.Configuration.Xml;
using AutoTrade.MarketData.Entities;
using AutoTrade.MarketData.Yahoo.Yql.Exceptions;

namespace AutoTrade.MarketData.Yahoo.Yql
{
    public class YqlResultTranslator : IYqlResultTranslator
    {
        #region Constants

        /// <summary>
        /// The XPath used to access the quote elements in the response
        /// </summary>
        private const string QuoteXPath = "//results/quote";

        /// <summary>
        /// The name of the symbol attribute for a quote
        /// </summary>
        private const string SymbolAttributeName = "symbol";

        /// <summary>
        /// The name of the Ask element for a quote
        /// </summary>
        private const string AskElementName = "Ask";

        /// <summary>
        /// The name of the Bid element for a quote
        /// </summary>
        private const string BidElementName = "Bid";

        /// <summary>
        /// The name of the Change element for a quote
        /// </summary>
        private const string ChangeElementName = "Change";

        /// <summary>
        /// The name of the OpenPrice element for a quote
        /// </summary>
        private const string OpenPriceElementName = "OpenPrice";

        /// <summary>
        /// The XPath used to get the javascript element
        /// </summary>
        private const string JavascriptXPath = "//javascript";

        #endregion

        #region Fields

        /// <summary>
        /// The settings for Yahoo market data
        /// </summary>
        private readonly IYahooMarketDataSettings _settings;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="YqlResultTranslator"/>
        /// </summary>
        /// <param name="settings"></param>
        public YqlResultTranslator(IYahooMarketDataSettings settings)
        {
            _settings = settings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Translates the results of a YQL query to StockQuotes
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public IEnumerable<StockQuote> TranslateResultsToQuotes(string response)
        {
            // create xml doc
            var xmlDoc = new XmlDocument();

            // load xml into doc
            try
            {
                xmlDoc.LoadXml(response);
            }
            catch (Exception ex)
            {
                throw new InvalidYqlResponseException(ex);
            }

            // check for any errors in the xml doc
            CheckForErrors(xmlDoc);

            // if no quote elements were found, return an empty list
            var quoteElements = xmlDoc.SelectNodes(QuoteXPath);
            if (quoteElements == null)
                return new List<StockQuote>();

            // convert quote elements to quotes
            return quoteElements.Cast<XmlElement>().Select(TranslateToQuote);
        }

        /// <summary>
        /// Checks if any errors were returned in the xml
        /// </summary>
        /// <param name="node"></param>
        private void CheckForErrors(XmlNode node)
        {
            // get message for table blocking
            var tableBlockingMessage = GetTableBlockingMessage(node);

            // if a table-blocking message was found, throw an error
            if (!string.IsNullOrWhiteSpace(tableBlockingMessage))
                throw new YqlTableBlockedException(tableBlockingMessage);
        }

        /// <summary>
        /// Checks for the blocked table message in the results
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <returns></returns>
        private string GetTableBlockingMessage(XmlNode xmlDocument)
        {
            // get the javascript node from diagnostics
            var javaScriptNode = xmlDocument.SelectSingleNode(JavascriptXPath);

            // if javascript node was not present, no issues
            if (javaScriptNode == null)
                return string.Empty;

            // get inner text without CData tags
            var innerText = javaScriptNode.ExtractInnerTextFromCData();

            // if the text contains the YQL table blocked message, return it
            return innerText.Contains(_settings.YqlTableBlockedMessage) ? innerText : string.Empty;
        }

        /// <summary>
        /// Translates a quote element into a <see cref="StockQuote"/>
        /// </summary>
        /// <param name="quoteElement"></param>
        /// <returns></returns>
        private static StockQuote TranslateToQuote(XmlElement quoteElement)
        {
            // get the symbol
            var symbol = quoteElement.GetAttribute(SymbolAttributeName);
            if (string.IsNullOrWhiteSpace(symbol))
                throw new YqlResponseNodeMissingException(SymbolAttributeName);

            // get child elements of quote
            var childElements = quoteElement.ChildNodes.OfType<XmlElement>().ToList();
            
            // create quote
            return new StockQuote
                {
                    Symbol = symbol,
                    AskPrice = GetElementValue<decimal>(childElements, AskElementName),
                    BidPrice = GetElementValue<decimal>(childElements, BidElementName),
                    Change = GetElementValue<decimal>(childElements, ChangeElementName),
                    OpenPrice = GetElementValue<decimal>(childElements, OpenPriceElementName),
                    QuoteDateTime = DateTime.Now
                };
        }

        /// <summary>
        /// Gets the value for an element =
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elements"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        private static T GetElementValue<T>(IEnumerable<XmlElement> elements, string elementName)
        {
            // get the element by name
            var element =
                elements.FirstOrDefault(e => StringComparer.OrdinalIgnoreCase.Compare(e.Name, elementName) == 0);

            // check that the element was found and has a value; if not, return the default value for the type
            if (element == null || string.IsNullOrWhiteSpace(element.InnerText))
                return default(T);

            // conver the inner text of the element to the value type
            return element.InnerText.ConvertTo<T>();
        }

        #endregion
    }
}