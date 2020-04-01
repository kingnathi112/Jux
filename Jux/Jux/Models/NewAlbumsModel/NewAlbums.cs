using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jux.Models.NewAlbumsModel
{
    public class NewAlbums
    {
        [JsonProperty(PropertyName = "items")]
        public List<AlbumDetails> Albums { get; set; }

        [JsonProperty(PropertyName = "list_count")]
        public int Current_Count { get; set; }

        [JsonProperty(PropertyName = "next_index")]
        public int Next_Count { get; set; }


        [JsonProperty(PropertyName = "total_count")]
        public int Total_Count { get; set; }
    }
}
