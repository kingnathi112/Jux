using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jux.Models.ArtistModel
{
   public class ArtistAlbumsModel
    {
        /// <summary>
        /// Need to be decoded
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Artist { get; set; }

        [JsonProperty(PropertyName = "songnum")]
        public int Number_Of_Songs { get; set; }

        [JsonProperty(PropertyName = "albumnum")]
        public int Number_Of_Albums { get; set; }

        [JsonProperty(PropertyName = "albumlist")]
        public List<Albums> Albums { get; set; }
    }
}
