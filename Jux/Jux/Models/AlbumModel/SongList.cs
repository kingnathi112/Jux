using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jux.Models.AlbumModel
{
    public class SongList
    {

        [JsonProperty(PropertyName = "songid")]
        public string Id { get; set; }

        /// <summary>
        /// Need to be Decoded to Friendly Title
        /// </summary>
        [JsonProperty(PropertyName = "songname")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "cdIdx")]
        public int Number { get; set; }

        /// <summary>
        /// Need to be converted to Minutes (Divide this by 60)
        /// </summary>
        [JsonProperty(PropertyName = "playtime")]
        public int Duration { get; set; }

        /// <summary>
        /// Need to be converted to MegaBytes (Divide this by Million)
        /// </summary>
        [JsonProperty(PropertyName = "n128Size")]
        public int Normal_Quality_Size { get; set; }

        /// <summary>
        /// Need to be converted to MegaBytes (Divide this by Million)
        /// </summary>
        [JsonProperty(PropertyName = "320size")]
        public int High_Quality_Size { get; set; }

        /// <summary>
        /// Zero means no lyrics
        /// </summary>
        [JsonProperty(PropertyName = "lrc_exist")]
        public int Has_Lyrics { get; set; }



    }
}
