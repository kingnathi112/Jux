using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Jux.Helpers
{
    public class ApiClient
    {
        public static HttpClient MobileApiClient { get; set; }

        public static void Init()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                UseDefaultCredentials = true
            };

            MobileApiClient = new HttpClient(handler);
            MobileApiClient.DefaultRequestHeaders.Accept.Clear();
            MobileApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            MobileApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
        }
    }
}
