namespace Jux.Interface
{
    public interface ISettingsManager
    {
         bool UserQuality { get; set; }
         bool UserLyrics { get; set; }
         bool UserAlbumCover { get; set; }
         string UserLocation { get; set; }
         string UserAlbumTitle { get; set; }
         string UserTrack { get; set; }

         bool DefaultQuality { get; }
         bool DefaultLyrics { get; }
         bool DefaultAlbumCover { get; }
         string DefaultLocation { get; }
         string DefaultAlbumTitle { get; }
         string DefaultTrack { get; }

         void SaveSettings();
    }
}
