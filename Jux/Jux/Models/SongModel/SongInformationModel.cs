using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Jux.Models.SongModel
{
   public class SongInformationModel
    {
        /// <summary>
        /// URL for album picture
        /// </summary>
        [JsonProperty(PropertyName = "album_url")]
        public string Album_Image { get; set; }

        /// <summary>
        /// This is already decoded
        /// </summary>
        [JsonProperty(PropertyName = "malbum")]
        public string Album { get; set; }

        /// <summary>
        /// Need to be converted to Minutes (Divide this by 60)
        /// </summary>
        [JsonProperty(PropertyName = "minterval")]
        public int Duration { get; set; }

        /// <summary>
        /// URL for normal quality mp3 (128 kbps)
        /// </summary>
        [JsonProperty(PropertyName = "mp3Url")]
        public string Normal_Quality { get; set; }

        /// <summary>
        /// This is already decoded
        /// </summary>
        [JsonProperty(PropertyName = "msinger")]
        public string Artist { get; set; }

        /// <summary>
        /// Song Title, already decoded
        /// </summary>
        [JsonProperty(PropertyName = "msong")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "public_time")]
        public string Date { get; set; }

        /// <summary>
        /// URL for high quality mp3 (320 kbps)
        /// </summary>
        [JsonProperty(PropertyName = "r320Url")]
        public string High_Quality { get; set; }

        /// <summary>
        /// Need to be converted to MegaBytes (Divide this by Million)
        /// </summary>
        [JsonProperty(PropertyName = "size128")]
        public int Normal_Quality_Size { get; set; }

        /// <summary>
        /// Need to be converted to MegaBytes (Divide this by Million)
        /// </summary>
        [JsonProperty(PropertyName = "size320")]
        public int High_Quality_Size { get; set; }

        [JsonProperty(PropertyName = "track_label_flag")]
        public int Number { get; set; }
    }
}
