using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using MediaVF.Common.Entities;

using MediaVF.Services.Core.Components;
using MediaVF.Services.Core.Data;
using MediaVF.Services.Core.Utilities;

namespace MediaVF.Services.Polling.Matching
{
    public class RegexMatcher<T> : ITextMatcher<T> where T : new()
    {
        IServiceComponent ServiceComponent { get; set; }

        IDataManager DataManager { get; set; }

        List<Regex> Regexes { get; set; }

        public RegexMatcher(IServiceComponent serviceComponent, IDataManager dataManager)
        {
            ServiceComponent = serviceComponent;
            DataManager = dataManager;
        }

        public void Initialize()
        {
            Regexes = DataManager.GetDataContext<Regex>().GetByModuleID<Regex>(ServiceComponent.ID).Where(regex => regex.MatchType == typeof(T).FullName).ToList();
        }

        public List<T> GetMatches(string text)
        {
            List<T> results = new List<T>();

            Regexes.ForEach(regex =>
            {
                System.Text.RegularExpressions.MatchCollection matches = System.Text.RegularExpressions.Regex.Matches(text, regex.RegexPattern);

                foreach (System.Text.RegularExpressions.Match match in matches)
                {
                    T result = new T();
                    regex.Captures.ForEach(capture =>
                    {
                        PropertyInfo property = typeof(T).GetProperty(capture.Name);
                        if (property != null)
                        {
                            string captureText = match.Groups[capture.CaptureIndex].Value;
                            property.SetValue(result, captureText.ConvertToType(property.PropertyType), null);
                        }
                    });

                    results.Add(result);
                }
            });

            return results;
        }
    }
}
