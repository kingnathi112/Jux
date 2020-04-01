using Id3;
using Id3.Frames;
using Jux.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using TagLib;
using Xamarin.Forms;

namespace Jux.Helpers
{
    public class WriteTags
    {
        string[] artists;
        string title = "";
        string album = "";
        uint year = 0;
        uint position = 0;
        string image = "";
        string path = "";
        public WriteTags(string[] Artists, string Title, string Album, uint Year, uint Position, string Image, string Path)
        {
            artists = Artists;
            title = Title;
            album = Album;
            year = Year;
            position = Position;
            image = Image;
            path = Path;
        }

        public void Save()
        {
            try
            {
                var audio = TagLib.File.Create(path);
                audio.Tag.Performers = artists;
                audio.Tag.Album = album;
                audio.Tag.AlbumArtists = artists;
                audio.Tag.Title = title;
                audio.Tag.Year = year;
                audio.Tag.Track = position;

                var picture = new IPicture[1];
                if (image != "")
                {
                    picture[0] = new Picture(image);
                    audio.Tag.Pictures = picture;
                  }
                audio.Save();
                Device.BeginInvokeOnMainThread(() => DependencyService.Get<IMessageCenter>().ShortMessage($"Saved {title}"));
            }
            catch(Exception ex)
            {
                Device.BeginInvokeOnMainThread(() => DependencyService.Get<IMessageCenter>().ShortMessage($"{ex.Message}"));
            } 
        }
    }
}
