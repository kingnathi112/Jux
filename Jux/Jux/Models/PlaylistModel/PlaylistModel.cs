using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jux.Models.PlaylistModel
{
    public class PlaylistModel
    {
        public string Description { get; set; }
        public List<PlaylistImages> Images { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Playlist_Name { get; set; }

        [JsonProperty(PropertyName = "publish_date")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "track_count")]
        public int Number_Of_Songs { get; set; }

        [JsonProperty(PropertyName = "tracks")]
        public Songs Songs { get; set; }
    }
}
