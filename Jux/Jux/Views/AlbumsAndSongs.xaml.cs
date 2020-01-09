using Jux.CustomControls;
using Jux.CustomViews;
using Jux.Data.Album;
using Jux.Data.Song;
using Jux.Helpers;
using Jux.Models.PlaylistModel;
using Rg.Plugins.Popup.Pages;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Jux.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlbumsAndSongs : PopupPage
    {
        int ArtistId = 0;
        bool IsAlbum = true;
        bool IsPlaylist = false;
        Songs SongList;
        public AlbumsAndSongs(int ArtistId, bool IsAlbum = true)
        {
            InitializeComponent();
            this.ArtistId = ArtistId;
            this.IsAlbum = IsAlbum;
        }

        public AlbumsAndSongs(Songs Playlist)
        {
            InitializeComponent();
            SongList = Playlist;
            IsPlaylist = true;
            IsAlbum = false;
        }

        protected async override void OnAppearing()
        {
            if (IsAlbum && IsPlaylist == false)
            {
                ScrollResults.Content = await ActiveIndicator.DisplayBusy();
                ScrollResults.Content = await GetArtistAlbums();
            }
            else if(IsAlbum == false && IsPlaylist)
            {
                ScrollResults.Content = await ActiveIndicator.DisplayBusy();
                ScrollResults.Content = await GetPlaylistSongs();
            }
            else
            {
                ScrollResults.Content = await ActiveIndicator.DisplayBusy();
                ScrollResults.Content = await GetArtistSongs();
            }
        }

        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return false;
        }
        #region Animation
        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }

        #endregion
        private async Task<StackLayout> GetArtistAlbums()
        {
            var ArtistAlbums = await GetAlbumDetails.ByArtistId(ArtistId);
            var NumberOfAlbums = ArtistAlbums.Number_Of_Albums;
            var Artist = ConvertJuxString.Decode(ArtistAlbums.Artist);
            var Albums = ArtistAlbums.Albums;

          
            var stack = new StackLayout();
            await Task.Run(() =>
            {
                foreach (var Album in Albums)
                {
                    var albumView = new AlbumView();
                    albumView.Artist = Artist;
                    albumView.Album = ConvertJuxString.Decode(Album.Title);
                    albumView.ImageUrl = Album.Album_Picture;
                    albumView.AlbumId = Album.Id;
                    albumView.Year = Album.Date;
                    stack.Children.Add(albumView);
                }
            });
            return stack;
        }

        private async Task<StackLayout> GetArtistSongs()
        {
            var ArtistSongs = await GetSongDetails.ByArtistId(ArtistId);
            var Artist = ConvertJuxString.Decode(ArtistSongs.Artist);
            var Songs = ArtistSongs.Songs;

            var stack = new StackLayout();

            await Task.Run(() =>
            { 
                foreach(var Song in Songs)
                {
                    var songView = new SongView();
                    songView.Album = ConvertJuxString.Decode(Song.Album);
                    songView.Artist = Artist;
                    songView.Title = ConvertJuxString.Decode(Song.Title);
                    songView.Duration = Duration(Song.Duration);
                    songView.SongId = Song.SongId;
                    songView.ImageUrl = "Song.png";

                    stack.Children.Add(songView);
                }
            });
            return stack;
        }

        private async Task<StackLayout> GetPlaylistSongs()
        {
            var PlaylistSongs = SongList;
            var stack = new StackLayout();
            var LblMessage = new Label
            {
                Text = "To download the entire list, Press Back button.",
                Style = (Style)App.Current.Resources["LightLabel"],
                TextColor = Color.Red,
                HorizontalOptions = LayoutOptions.Center
            };
            stack.Children.Add(LblMessage);
           await Task.Run(() =>
            {
                foreach (var Song in PlaylistSongs.Details)
                {
                    var songView = new SongView();
                    songView.Album = Song.Album_Name;
                    songView.Artist = Song.Artists.Select(s => s.Name).FirstOrDefault();
                    songView.Title = Song.Title;
                    songView.Duration = Duration(Song.Duration);
                    songView.SongId = Song.SongId;
                    songView.ImageUrl = Song.Images.Where(s => s.Height > 300).Select(u => u.Url).FirstOrDefault();

                    stack.Children.Add(songView);
                }
            });
            return stack;
        }


        private string Duration(int seconds)
        {
            string duration = "";
            var Tim = TimeSpan.FromSeconds(seconds);
            if (Tim.Hours > 0)
            {
                duration = Tim.Hours.ToString("00") + ":" + Tim.Minutes.ToString("00") + ":" + Tim.Seconds.ToString("00");
            }
            else
            {
                duration = Tim.Minutes.ToString("00") + ":" + Tim.Seconds.ToString("00");
            }
            return duration;
        }
    }
}