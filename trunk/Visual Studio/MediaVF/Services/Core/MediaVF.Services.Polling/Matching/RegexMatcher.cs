using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using MediaVF.Services.Components;
using MediaVF.Services.Data;
using MediaVF.Services.Utilities;
using System.Text.RegularExpressions;
using MediaVF.Services.Polling.Matching;

namespace MediaVF.Services.ArtistTrack.Polling.Matching
{
    public class RegexMatcher<T> : ITextMatcher<T> where T : new()
    {
        #region Properties

        /// <summary>
        /// Gets or sets the service component to which this matcher belongs
        /// </summary>
        IServiceComponent ServiceComponent { get; set; }

        /// <summary>
        /// Gets or sets the regex provider for retrieving the regexes with which to find matches
        /// </summary>
        Func<IEnumerable<IRegex>> RegexProvider { get; set; }

        /// <summary>
        /// Gets or sets a collection of regex objects for finding matches in the text
        /// </summary>
        IEnumerable<IRegex> Regexes { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a regex matcher with the given regex provider
        /// </summary>
        /// <param name="serviceComponent"></param>
        /// <param name="dataManager"></param>
        /// <param name="regexProvider"></param>
        public RegexMatcher(IServiceComponent serviceComponent, Func<IEnumerable<IRegex>> regexProvider)
        {
            ServiceComponent = serviceComponent;
            RegexProvider = regexProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the regex matcher by getting the its regex objects
        /// </summary>
        public void Initialize()
        {
            if (RegexProvider != null)
                Regexes = RegexProvider();
            //Regexes = DataManager.GetDataContext<Regex>().GetByModuleID<Regex>(ServiceComponent.ID).Where(regex => regex.MatchType == typeof(T).FullName).ToList();
        }

        /// <summary>
        /// Gets matches from a given text
        /// </summary>
        /// <param name="text">The text to check for matches</param>
        /// <returns>A list of object matches found in the given text</returns>
        public List<T> GetMatches(string text)
        {
            List<T> results = new List<T>();

            foreach (IRegex regex in Regexes)
            {
                // get the matches for this regex
                MatchCollection matches = Regex.Matches(text, regex.RegexPattern);

                // for each match, get the captures it contains
                foreach (Match match in matches)
                {
                    // create a new matched object
                    T result = new T();

                    // for each capture for this regex
                    foreach (string captureName in regex.Captures.Keys)
                    {
                        // set the corresponding property on the matching object from the captured text
                        PropertyInfo property = typeof(T).GetProperty(captureName);
                        if (property != null)
                        {
                            // get the capture text from the match
                            string captureText = match.Groups[regex.Captures[captureName]].Value;

                            // set the property's value from the capture text
                            property.SetValue(result, captureText.ConvertToType(property.PropertyType), null);
                        }
                    }

                    // add to the results
                    results.Add(result);
                }
            }

            return results;
        }

        #endregion
    }
}
