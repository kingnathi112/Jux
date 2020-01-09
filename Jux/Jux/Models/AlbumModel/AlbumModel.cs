using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jux.Models.AlbumModel
{
    public class AlbumModel
    {
        [JsonProperty(PropertyName = "album_sum")]
        public int NumberOfSongs { get; set; }

        [JsonProperty(PropertyName = "albuminfo")]
        public AlbumInfo AlbumInformation { get; set; }
    }
}
