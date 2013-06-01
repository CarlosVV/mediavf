using System.IO;
using System.Net;
using AutoTrade.Core.Properties;

namespace AutoTrade.Core.Web
{
    public class WebRequestExecutor : IWebRequestExecutor
    {
        /// <summary>
        /// Executes a web request and returns the response
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string ExecuteRequest(string url)
        {
            // execute request
            var request = WebRequest.Create(url);

            using (var response = request.GetResponse())
            {
                // get the response stream
                var responseStream = response.GetResponseStream();
                if (responseStream == null)
                    throw new NullWebResponseStreamException(Resources.NullWebResponseStreamMessageFormat);

                using (var reader = new StreamReader(responseStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}