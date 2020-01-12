using Jux.Helpers;
using Jux.Models.PlaylistModel;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Jux.Data.Playlist
{
    public class GetPlaylistDetails
    {
        public static async Task<PlaylistModel> ById(string Id, int Start = 0, int ResultsCount = 50)
        {
            string url = $"https://api-jooxtt.sanook.com/openjoox/v1/playlist/{Id}/tracks?country=za&lang=en&index={Start}&num={ResultsCount}";
            var resultSearchModels = new PlaylistModel();

            using (System.Net.Http.HttpResponseMessage response = await ApiClient.MobileApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var results = await response.Content.ReadAsStringAsync();
                    resultSearchModels = JsonConvert.DeserializeObject<PlaylistModel>(results);
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
