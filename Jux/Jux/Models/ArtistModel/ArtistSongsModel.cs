using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jux.Models.ArtistModel
{
    public class ArtistSongsModel
    {
        [JsonProperty(PropertyName = "name")]
        public string Artist { get; set; }

        [JsonProperty(PropertyName = "songnum")]
        public int Number_Of_Songs { get; set; }

        [JsonProperty(PropertyName = "albumnum")]
        public int Number_Of_Albums { get; set; }

        [JsonProperty(PropertyName = "songlist")]
        public List<Songs> Songs { get; set; }
    }
}
