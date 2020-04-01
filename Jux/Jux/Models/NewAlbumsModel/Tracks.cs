using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jux.Models.NewAlbumsModel
{
    public class Tracks
    {
        [JsonProperty(PropertyName = "items")]
        public List<NewAlbumSongDetails> Details { get; set; }

        [JsonProperty(PropertyName = "total_count")]
        public int Number_Of_Songs { get; set; }
    }
}
