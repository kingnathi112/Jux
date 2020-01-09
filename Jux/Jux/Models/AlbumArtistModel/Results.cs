using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jux.Models.AlbumArtistModel
{
   public class Results
    {
		public int Id { get; set; }

		[JsonProperty(PropertyName = "songnum")]
		public int Number_Of_Songs { get; set; }

		[JsonProperty(PropertyName = "singername")]
		public string Artist { get; set; }

		[JsonProperty(PropertyName = "title")]
		public string Album { get; set; }

		[JsonProperty(PropertyName = "album_url")]
		public string Album_Picture { get; set; }

		[JsonProperty(PropertyName = "singer_url")]
		public string Artist_Picture { get; set; }

		[JsonProperty(PropertyName = "bigpic")]
		public string High_Quality_Picture { get; set; }

		[JsonProperty(PropertyName = "smallpic")]
		public string Normal_Picture { get; set; }
	}
}
