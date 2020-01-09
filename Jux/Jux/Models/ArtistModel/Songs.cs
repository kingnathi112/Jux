using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jux.Models.ArtistModel
{
    public class Songs
    {
		public string SongId { get; set; }

		/// <summary>
		/// Need to be decoded
		/// </summary>
		[JsonProperty(PropertyName = "songname")]
		public string Title { get; set; }

		/// <summary>
		/// Need to be decoded
		/// </summary>
		[JsonProperty(PropertyName = "singername")]
		public string Artist { get; set; }

		public int AlbumId { get; set; }

		/// <summary>
		/// Need to be decoded
		/// </summary>
		[JsonProperty(PropertyName = "albumname")]
		public string Album { get; set; }

		/// <summary>
		/// Need to be converted
		/// </summary>
		[JsonProperty(PropertyName = "playtime")]
		public int Duration { get; set; }

		[JsonProperty(PropertyName = "track_label_flag")]
		public int Number { get; set; }
	}
}
