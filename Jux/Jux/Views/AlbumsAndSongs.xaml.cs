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
        private int index = 0;
        private int limit = 30;
        private int ResultNum = 0;

        #region Control
        private StackLayout StackNavigation = new StackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            IsVisible = true,
            Orientation = StackOrientation.Horizontal,
            Spacing = 15
        };
        ImageButton BtnFirst = new ImageButton
        {
            HeightRequest = 30,
            IsEnabled = false,
            Source = "FirstDisabled.png",
            WidthRequest = 30
        };
        ImageButton BtnLast = new ImageButton
        {
            HeightRequest = 30,
            IsEnabled = true,
            Source = "Last.png",
            WidthRequest = 30
        };
        ImageButton BtnNext = new ImageButton
        {
            HeightRequest = 30,
            Source = "Next.png",
            WidthRequest = 30
        };
        ImageButton BtnPrevious = new ImageButton
        {
            HeightRequest = 30,
            IsEnabled = false,
            Source = "PreviousDisabled.png",
            WidthRequest = 30
        };
        #endregion
        public AlbumsAndSongs(int ArtistId, bool IsAlbum = true)
        {
            InitializeComponent();
            this.ArtistId = ArtistId;
            this.IsAlbum = IsAlbum;

            BtnFirst.Clicked += BtnFirst_Clicked;
            BtnPrevious.Clicked += BtnPrevious_Clicked;
            BtnNext.Clicked += BtnNext_Clicked;
            BtnLast.Clicked += BtnLast_Clicked;

            StackNavigation.Children.Add(BtnFirst);
            StackNavigation.Children.Add(BtnPrevious);
            StackNavigation.Children.Add(BtnNext);
            StackNavigation.Children.Add(BtnLast);
        }
               
        public AlbumsAndSongs(Songs Playlist)
        {
            InitializeComponent();
            SongList = Playlist;
            IsPlaylist = true;
            IsAlbum = false;

            BtnFirst.Clicked += BtnFirst_Clicked;
            BtnPrevious.Clicked += BtnPrevious_Clicked;
            BtnNext.Clicked += BtnNext_Clicked;
            BtnLast.Clicked += BtnLast_Clicked;

            StackNavigation.Children.Add(BtnFirst);
            StackNavigation.Children.Add(BtnPrevious);
            StackNavigation.Children.Add(BtnNext);
            StackNavigation.Children.Add(BtnLast);
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
            var ArtistAlbums = await GetAlbumDetails.ByArtistId(ArtistId, index, limit);
            var NumberOfAlbums = ArtistAlbums.Number_Of_Albums;
            var Artist = ConvertJuxString.Decode(ArtistAlbums.Artist);
            var Albums = ArtistAlbums.Albums;
            ResultNum = NumberOfAlbums;
            
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
                stack.Children.Add(StackNavigation);
            });
            return stack;
        }

        private async Task<StackLayout> GetArtistSongs()
        {
            var ArtistSongs = await GetSongDetails.ByArtistId(ArtistId, index, limit);
            var Artist = ConvertJuxString.Decode(ArtistSongs.Artist);
            var Songs = ArtistSongs.Songs;
            ResultNum = ArtistSongs.Number_Of_Songs;

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
                stack.Children.Add(StackNavigation);
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

        private async void BtnFirst_Clicked(object sender, EventArgs e)
        {
            index = 0;
            limit = 30;

            if (IsAlbum)
            {
                ScrollResults.Content = await ActiveIndicator.DisplayBusy();
                ScrollResults.Content = await GetArtistAlbums();
            }
            else
            {
                ScrollResults.Content = await ActiveIndicator.DisplayBusy();
                ScrollResults.Content = await GetArtistSongs();
            }

            BtnFirst.IsEnabled = false;
            BtnFirst.Source = "FirstDisabled.png";

            BtnPrevious.IsEnabled = false;
            BtnPrevious.Source = "PreviousDisabled.png";

            BtnLast.IsEnabled = true;
            BtnLast.Source = "Last.png";

            BtnNext.IsEnabled = true;
            BtnNext.Source = "Next.png";
        }

        private async void BtnPrevious_Clicked(object sender, EventArgs e)
        {
            index -= 30;
            limit -= 30;

            if (index >= 0)
            {
                BtnLast.IsEnabled = true;
                BtnLast.Source = "Last.png";

                BtnNext.IsEnabled = true;
                BtnNext.Source = "Next.png";

                if (IsAlbum)
                {
                    ScrollResults.Content = await ActiveIndicator.DisplayBusy();
                    ScrollResults.Content = await GetArtistAlbums();
                }
                else
                {
                    ScrollResults.Content = await ActiveIndicator.DisplayBusy();
                    ScrollResults.Content = await GetArtistSongs();
                }
            }
            else
            {
                BtnFirst.IsEnabled = false;
                BtnFirst.Source = "FirstDisabled.png";
                BtnPrevious.IsEnabled = false;
                BtnPrevious.Source = "PreviousDisabled.png";
            }
        }

        private async void BtnNext_Clicked(object sender, EventArgs e)
        {
            index += 30;
            limit += 30;

            if (limit <= ResultNum)
            {
                BtnFirst.IsEnabled = true;
                BtnFirst.Source = "First.png";

                BtnPrevious.IsEnabled = true;
                BtnPrevious.Source = "Previous.png";

                if (IsAlbum)
                {
                    ScrollResults.Content = await ActiveIndicator.DisplayBusy();
                    ScrollResults.Content = await GetArtistAlbums();
                }
                else
                {
                    ScrollResults.Content = await ActiveIndicator.DisplayBusy();
                    ScrollResults.Content = await GetArtistSongs();
                }
            }
            else
            {
                BtnLast.IsEnabled = false;
                BtnLast.Source = "LastDisabled.png";

                BtnNext.IsEnabled = false;
                BtnNext.Source = "NextDisabled.png";
            }
        }

        private async void BtnLast_Clicked(object sender, EventArgs e)
        {
            index = ResultNum - 30;
            limit = ResultNum;

            if (IsAlbum)
            {
                ScrollResults.Content = await ActiveIndicator.DisplayBusy();
                ScrollResults.Content = await GetArtistAlbums();
            }
            else
            {
                ScrollResults.Content = await ActiveIndicator.DisplayBusy();
                ScrollResults.Content = await GetArtistSongs();
            }

            BtnLast.IsEnabled = false;
            BtnLast.Source = "LastDisabled.png";

            BtnNext.IsEnabled = false;
            BtnNext.Source = "NextDisabled.png";

            BtnFirst.IsEnabled = true;
            BtnFirst.Source = "First.png";

            BtnPrevious.IsEnabled = true;
            BtnPrevious.Source = "Previous.png";
        }
    }
}