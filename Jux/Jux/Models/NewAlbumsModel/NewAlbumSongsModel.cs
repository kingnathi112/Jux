using Newtonsoft.Json;

namespace Jux.Models.NewAlbumsModel
{
    public class NewAlbumSongsModel
    {
        [JsonProperty (PropertyName = "tracks")]
        public Tracks Songs { get; set; }
    }
}
