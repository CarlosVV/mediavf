
namespace ExternalServicesPOC.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using System.Net;
    using System.IO;
    using System.Text.RegularExpressions;


    // TODO: Create methods containing your application logic.
    [EnableClientAccess()]
    public class SearchService : DomainService
    {
        const string SEARCH_URL = "http://images.google.com/images?q={0}";

        const string IMAGE_MATCH_REGEX = "/imgres?imgurl\\x3d";

        [Invoke]
        public IEnumerable<string> GetSearchResults(string keywordText)
        {
            keywordText = keywordText.Replace(" ", "+");

            string searchUrl = string.Format(SEARCH_URL, keywordText.ToString());

            using (WebClient webClient = new WebClient())
            using (Stream responseStream = webClient.OpenRead(searchUrl))
            {
                string response = string.Empty;
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    response = reader.ReadToEnd();
                }

                if (!string.IsNullOrEmpty(response))
                    return GetImageURLs(response);
            }

            return new List<string>();
        }

        private List<string> GetImageURLs(string searchResults)
        {
            return new List<string>();
        }
    }
}


