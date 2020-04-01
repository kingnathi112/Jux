using Jux.Helpers;
using Jux.Models.AlbumModel;
using Jux.Models.ArtistModel;
using Jux.Models.NewAlbumsModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jux.Data.Album
{
    public class GetAlbumDetails
    {
        public static async Task<AlbumModel> ById(int Id)
        {
            string url = $"https://api-jooxtt.sanook.com/web-fcgi-bin/web_get_albuminfo?country=za&lang=en&albumid={Id}&all=1";
            var resultSearchModels = new AlbumModel();

            using (System.Net.Http.HttpResponseMessage response = await ApiClient.MobileApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var results = await response.Content.ReadAsStringAsync();
                    resultSearchModels = JsonConvert.DeserializeObject<AlbumModel>(results);
                    return resultSearchModels;
                }
                else
                {
                    return null;
                }
            }
        }

        public static async Task<ArtistAlbumsModel> ByArtistId(int ArtistId, int Index = 0, int Limit = 29)
        {
            string url = $"https://api-jooxtt.sanook.com/web-fcgi-bin/web_album_singer?country=za&lang=en&cmd=1&singerid={ArtistId}&ein={Limit}&sin={Index}";
            var resultSearchModels = new ArtistAlbumsModel();

            using (System.Net.Http.HttpResponseMessage response = await ApiClient.MobileApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var results = await response.Content.ReadAsStringAsync();
                    resultSearchModels = JsonConvert.DeserializeObject<ArtistAlbumsModel>(results);
                    return resultSearchModels;
                }
                else
                {
                    return null;
                }
            }
        }

        public static async Task<NewAlbumSongsModel> ByEncodedId(string Id)
        {
            string url = $"https://api-jooxtt.sanook.com/openjoox/v1/album/{Id}/tracks?country=za&lang=en";
            var resultSearchModels = new NewAlbumSongsModel();

            using (System.Net.Http.HttpResponseMessage response = await ApiClient.MobileApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var results = await response.Content.ReadAsStringAsync();
                    resultSearchModels = JsonConvert.DeserializeObject<NewAlbumSongsModel>(results);
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
