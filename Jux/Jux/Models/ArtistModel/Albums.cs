using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jux.Models.ArtistModel
{
    public class Albums
    {
        public int Id { get; set; }

        /// <summary>
        /// Need to be decoded
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "publish_date")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "pic")]
        public string Album_Picture { get; set; }
    }
}
