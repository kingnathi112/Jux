using Jux.Helpers;
using Jux.Models.RedirectUrlModel;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Jux.Data.Redirect
{
    public class GetUrls
    {
        public static async Task<RedirectorModel> Url(string UrlToRedirect)
        {
            string url = $"https://httpstatus-backend-production.herokuapp.com/api";
            var resultSearchModels = new RedirectorModel();

            string PostString = $"{{\"urls\":[\"{UrlToRedirect}\"],\"userAgent\":\"googlebot\",\"userName\":\"\",\"passWord\":\"\",\"headerName\":\"\",\"headerValue\":\"\",\"strictSSL\":true,\"canonicalDomain\":false,\"additionalSubdomains\":[\"www\"],\"followRedirect\":true,\"throttleRequests\":100,\"escapeCharacters\":false}}";

            var content = new StringContent(PostString, Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = await ApiClient.MobileApiClient.PostAsync(url, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var returnJson = await response.Content.ReadAsStringAsync();
                    var editedJson = string.Concat(returnJson.Skip(1).Take(returnJson.Length - 2));
                    resultSearchModels = JsonConvert.DeserializeObject<RedirectorModel>(editedJson);
                    return resultSearchModels;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
