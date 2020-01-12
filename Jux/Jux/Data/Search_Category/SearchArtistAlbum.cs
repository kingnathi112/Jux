using Jux.Helpers;
using Jux.Interface;
using Jux.Models.AlbumArtistModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Jux.Data.Search_Category.Category;

namespace Jux.Data.Search_Category
{
    public class SearchArtistAlbum
    {
        public static async Task<AlbumAndArtistSearchModel> Search(Search SearchType, string SearchText, int Start = 0, int ResultCount = 30)
        {
            string searchString = "";

            if(SearchType.ToString() == "Artist")
            {
                searchString = $"https://api-jooxtt.sanook.com/web-fcgi-bin/web_category_search?country=za&lang=en&search_input={SearchText}&sin={Start}&ein={ResultCount}&type=2";
            }
            else
            {
                searchString = $"https://api-jooxtt.sanook.com/web-fcgi-bin/web_category_search?country=za&lang=en&search_input={SearchText}&sin={Start}&ein={ResultCount}&type=1";
            }

            var resultSearchModels = new AlbumAndArtistSearchModel();

            using (HttpResponseMessage response = await ApiClient.MobileApiClient.GetAsync(searchString))
            {
                if (response.IsSuccessStatusCode)
                {
                    var results = await response.Content.ReadAsStringAsync();
                    resultSearchModels = JsonConvert.DeserializeObject<AlbumAndArtistSearchModel>(results);
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
