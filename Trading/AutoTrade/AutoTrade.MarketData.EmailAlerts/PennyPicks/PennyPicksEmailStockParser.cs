using System;
using System.Text;
using AutoTrade.Core.Email;
using AutoTrade.MarketData.Data;

namespace AutoTrade.MarketData.EmailAlerts.PennyPicks
{
    public class PennyPicksEmailStockParser : IEmailStockParser
    {
        #region Methods

        /// <summary>
        /// Parses a stock symbol from an email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string ParseStockSymbol(IEmail email)
        {
            // check that email is not null
            if (email == null) throw new ArgumentNullException("email");

            // check that subject is not empty
            if (string.IsNullOrWhiteSpace(email.Subject)) return null;

            // get the subject with leading whitespace and stars removed
            var subject = email.Subject.TrimStart().TrimStart('*');

            // check that the trimmed subject is not empty
            if (string.IsNullOrWhiteSpace(subject)) return null;

            // if the first character of the subject is not upper-case, this is not an
            // an email with a stock symbol in it
            if (!char.IsUpper(subject[0])) return null;

            // build stock symbol character by character
            var stockSymbol = new StringBuilder();

            // assume all characters are upper-case until proven otherwise
            var allCaps = true;

            // read characters from subject until a stock symbol is found
            var i = 0;
            while (allCaps && !char.IsWhiteSpace(subject[i]))
            {
                // indicate that a non-capital letter was found
                if (!char.IsUpper(subject[i]))
                    allCaps = false;

                // add the character
                stockSymbol.Append(subject[i]);

                // move to next character
                i++;
            }

            // if all characters found were capital letters and the length was more than 1,
            // it's a stock symbol - return it
            if (allCaps && stockSymbol.Length > 1) return stockSymbol.ToString();

            return null;
        }

        #endregion
    }
}
