using Jux.CustomControls;
using Jux.CustomViews;
using Jux.Data.Album;
using Jux.Data.Playlist;
using Jux.Data.Song;
using Jux.Helpers;
using Jux.Interface;
using Rg.Plugins.Popup.Pages;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Jux.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Link : PopupPage
    {
        AlbumView albumView;
        SongView songView;

        int AlbumId;
        string SongId;
        string PlaytlistId;
        string Search;

        bool isPlaylist;
        bool isAlbum;
        bool isSong;
        public Link()
        {
            InitializeComponent();
            if (Clipboard.HasText)
            {
                TxtLink.Text = Clipboard.GetTextAsync().Result;                
            }
        }
        #region Popup Properties
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return false;
        }
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

        private string GetId(string url)
        {
            if (url.Contains("page=playsong"))
            {
                var Id = url.Substring(url.LastIndexOf("/") + 1).Trim().Replace("?appshare=android", "");
                isSong = true;
                return Id;
            }
            else if (url.Contains("page=album"))
            {
                var subUrl = url.Substring(url.IndexOf("id=") + 3);
                var Id = subUrl.Substring(0, subUrl.IndexOf("&"));
                isAlbum = true;
                return Id;
            }
            else if(url.Contains("playlist"))
            {
                var Id = url.Substring(url.LastIndexOf("/") + 1).Trim().Replace("?appshare=android", "");
                isPlaylist = true;
                return Id;
            }
            else
            {
                return null;
            }
        }

        private async void ImageButton_Clicked(object sender, System.EventArgs e)
        {
            Search = TxtLink.Text;
            TxtLink.Text = "Getting Url";
            string Url = "";

            ScrollResults.Content = await ActiveIndicator.DisplayBusy();
            await Task.Run(() =>
            {
                Url = UrlHelper.JooxUrl(Search).Result;
            });

            if(Url != null)
            {
                TxtLink.Text = Url;
                int id = 0;
                if (GetId(Url) != null && isSong)
                {
                    StackLayout StackResults = new StackLayout()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = 3,
                        BackgroundColor = Color.FromHex("#B54800")
                    };
                    SongId = GetId(Url);
                    ScrollResults.Content = await GetSong();                    
                }
                else if(GetId(Url) != null && isAlbum)
                {
                    StackLayout StackResults = new StackLayout()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = 3,
                        BackgroundColor = Color.FromHex("#B54800")
                    };

                    if (Int32.TryParse(GetId(Url), out id))
                    {
                        AlbumId = id;
                        ScrollResults.Content = await GetAlbum();
                    }
                } 
                else if((GetId(Url) != null && isPlaylist))
                {
                    PlaytlistId = (GetId(Url));
                    ScrollResults.Content = await GetPlaylist();
                }
                else
                {
                    DependencyService.Get<IMessageCenter>().ShortMessage("Cannot find ID");
                }

            }
            else
            {
                DependencyService.Get<IMessageCenter>().ShortMessage("Cannot find url");
            }
        }

        private async Task<StackLayout> GetAlbum()
        {
            var Results = await GetAlbumDetails.ById(AlbumId);
            var Album = Results.AlbumInformation;
            var Title = ConvertJuxString.Decode(Album.Album);
            var Artist = ConvertJuxString.Decode(Album.Artist);
            string Year = Convert.ToDateTime(ConvertJuxString.Decode(Album.Date)).Year.ToString();
            

            var stack = new StackLayout();
            await Task.Run(() =>
            {
                var albumView = new AlbumView();
                albumView.Artist = Artist;
                albumView.Album = Title;
                albumView.ImageUrl = Album.High_Quality_Picture;
                albumView.AlbumId = AlbumId;
                albumView.Year = Year;
                stack.Children.Add(albumView);
                
            });
            return stack;
        }
        private async Task<StackLayout> GetSong()
        {
            var Song = await GetSongDetails.ById(SongId);
            var stack = new StackLayout();

            await Task.Run(() =>
            {
                var songView = new SongView();
                songView.Album = Song.Album;
                songView.Artist = Song.Artist;
                songView.Title = Song.Title;
                songView.Duration = Duration(Song.Duration);
                songView.SongId = SongId;
                songView.ImageUrl = Song.Album_Image;

                stack.Children.Add(songView);
                
            });
            return stack;
        }

        private async Task<StackLayout> GetPlaylist()
        {
            var Results = await GetPlaylistDetails.ById(PlaytlistId);
            var CoverImg = Results.Images.Where(size => size.Height > 300).Select(img => img.Url).FirstOrDefault();
            var NumberOfSongs = Results.Number_Of_Songs;
            var Date = Convert.ToDateTime(Results.Date).Year.ToString();
            var Name = Results.Playlist_Name;
            var Playlist = Results.Songs;

            var stack = new StackLayout();

            await Task.Run(() =>
            {
                var playlistView = new PlaylistView();
                playlistView.Artist = "Variety";
                playlistView.Title = Name;
                playlistView.ImageUrl = CoverImg;          
                playlistView.Number_Of_Songs = $"{NumberOfSongs}";
                playlistView.Year = Date;
                playlistView.SongList = Playlist;
                stack.Children.Add(playlistView);
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