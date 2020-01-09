using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jux.Models.PlaylistModel
{
    public class Details
    {
        /// <summary>
        /// Already decoded
        /// </summary>
        public string Album_Name { get; set; }

        [JsonProperty(PropertyName = "artist_list")]
        public List<ArtistList> Artists { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string SongId { get; set; }

        public List<SongImages> Images { get; set; }

        /// <summary>
        /// Already decoded
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Title { get; set; }

        /// <summary>
        /// Convert this to Friendly time
        /// </summary>
        [JsonProperty(PropertyName = "play_duration")]
        public int Duration { get; set; }
    }
}
