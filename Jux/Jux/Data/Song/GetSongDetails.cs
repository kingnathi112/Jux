using Jux.Helpers;
using Jux.Models.ArtistModel;
using Jux.Models.SongModel;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Jux.Data.Song
{
    public class GetSongDetails
    {
        public static async Task<SongInformationModel> ById(string SongId)
        {
            string url =
                $"https://api-jooxtt.sanook.com/web-fcgi-bin/web_get_songinfo?country=za&lang=en&songid={SongId}";
            var resultSearchModels = new SongInformationModel();

            using (HttpResponseMessage response = await ApiClient.MobileApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var returnJson = await response.Content.ReadAsStringAsync();
                    resultSearchModels = JsonConvert.DeserializeObject<SongInformationModel>(returnJson);
                    return resultSearchModels;
                }
                else
                {
                    return null;
                }
            }
        }

        public static async Task<ArtistSongsModel> ByArtistId(int ArtistId, int Index = 0, int Limit = 30)
        {
            string url =
                $"https://api-jooxtt.sanook.com/web-fcgi-bin/web_album_singer?country=za&lang=en&cmd=2&singerid={ArtistId}&ein={Limit}&sin={Index}";
            var resultSearchModels = new ArtistSongsModel();

            using (HttpResponseMessage response = await ApiClient.MobileApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var returnJson = await response.Content.ReadAsStringAsync();
                    resultSearchModels = JsonConvert.DeserializeObject<ArtistSongsModel>(returnJson);
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
