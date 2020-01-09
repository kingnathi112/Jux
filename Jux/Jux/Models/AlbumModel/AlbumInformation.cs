using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jux.Models.AlbumModel
{
    public class AlbumInfo
    {
        [JsonProperty(PropertyName = "subtitle")]
        public string Artist { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Album { get; set; }

        public string Date { get; set; }

        [JsonProperty(PropertyName = "song_sum")]
        public int NumberOfSongs { get; set; }

        [JsonProperty(PropertyName = "songlist")]
        public List<SongList> Songs { get; set; }

        [JsonProperty(PropertyName = "picUrl")]
        public string High_Quality_Picture { get; set; }

        [JsonProperty(PropertyName = "smallPicUrl")]
        public string Small_Quality_Picture { get; set; }
    }
}
