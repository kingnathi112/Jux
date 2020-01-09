using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jux.Models.AlbumArtistModel
{
    public class AlbumAndArtistSearchModel
    {
        [JsonProperty(PropertyName = "result")]
        public List<Results> Results { get; set; }

        [JsonProperty(PropertyName = "resultnum")]
        public int Total { get; set; }
    }
}
