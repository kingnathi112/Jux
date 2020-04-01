using Newtonsoft.Json;

namespace Jux.Models.NewAlbumsModel
{
    public class ArtistList
    {
        public string Id { get; set; }

        /// <summary>
        /// Already decoded
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string ArtistName { get; set; }
    }
}
