using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jux.Models.PlaylistModel
{
    public class Songs
    {
        [JsonProperty(PropertyName = "items")]
        public List<Details> Details { get; set; }
    }
}
