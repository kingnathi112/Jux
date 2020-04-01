using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jux.Models.NewAlbumsModel
{
   public  class AlbumDetails
    {
        [JsonProperty(PropertyName = "artist_list")]
        public List<ArtistList> Artists { get; set; }

        public string Description { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string AlbumId { get; set; }

        [JsonProperty(PropertyName = "images")]
        public List<AlbumCovers> AlbumCover { get; set; }

        /// <summary>
        /// Already decoded
        /// </summary>
        [JsonProperty(PropertyName = "name")]        
        public string AlbumTitle { get; set; }

        /// <summary>
        /// Already decoded, just need to get a year
        /// </summary>
        [JsonProperty(PropertyName = "publish_date")]
        public string Date { get; set; }
    }
}
