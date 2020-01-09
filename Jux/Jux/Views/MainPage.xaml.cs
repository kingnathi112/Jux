using Jux.CustomControls;
using Jux.CustomViews;
using Jux.Data.Search_Category;
using Jux.Helpers;
using Jux.Models.AlbumArtistModel;
using Jux.Views;
using Rg.Plugins.Popup.Extensions;
using Jux.Interface;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Jux.Data.Album;

namespace Jux
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        event ConnectivityChangedEventHandler ConnectivityChanged;
        Category.Search SearchType;
        string SearchText = "";
        public MainPage()
        {
            InitializeComponent();
            ApiClient.Init();
            RadioButtonControl();
            CheckSelectedButton();
            ConnectivityChanged +=  (sender, args) =>
            {
                DependencyService.Get<IMessageCenter>().ShortMessage($"{args.IsConnected}");
            };
        }
        public delegate void ConnectivityChangedEventHandler(object sender, ConnectivityChangedEventArgs e);

        Settings SettingsPage = new Settings();
        AlbumView albumView;
        ArtistView artistView;
        void RadioButtonControl()
        {
            RadioAlbum.OnTapped += (s, e) =>
            {
                RadioArtist.IsSelected = false;
                RadioSong.IsSelected = false;
                SearchType = Category.Search.Album;
            };
            RadioSong.OnTapped += (s, e) =>
            {
                RadioArtist.IsSelected = false;
                RadioAlbum.IsSelected = false;
            };
            RadioArtist.OnTapped += (s, e) =>
            {
                RadioAlbum.IsSelected = false;
                RadioSong.IsSelected = false;
                SearchType = Category.Search.Artist;
            };
        }

        void CheckSelectedButton()
        {
            if (RadioAlbum.IsSelected)
            {
                SearchType = Category.Search.Album;
            }else if(RadioArtist.IsSelected)
            {
                SearchType = Category.Search.Artist;
            }
            else
            {
                DependencyService.Get<IMessageCenter>().ShortMessage("Nothing has been selected");
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            SearchStack.IsVisible = false;
            StackResults.IsVisible = false;
            await Task.WhenAll( SplashGrid.FadeTo(0, 2000), Logo.ScaleTo(10, 2000, Easing.SpringOut));
            SearchStack.IsVisible = true;
            StackResults.IsVisible = true;
        }

        private async void BtnSettings_Clicked(object sender, EventArgs e)
        {
            BtnSettings.IsEnabled = false;
            await Navigation.PushPopupAsync(SettingsPage, true);
            BtnSettings.IsEnabled = true;
        }

        private async void BtnLink_Clicked(object sender, EventArgs e)
        {
            BtnLink.IsEnabled = false;
           await Navigation.PushPopupAsync(new Link(), true);
            BtnLink.IsEnabled = true;
        }

        private async void BtnSearch_Clicked(object sender, EventArgs e)
        {
            SearchText = TxtSearch.Text;
            if (RadioAlbum.IsSelected)
            {
                BtnSearch.IsEnabled = false;
                ScrollResults.Content = await ActiveIndicator.DisplayBusy();
                ScrollResults.Content = await AlbumStackAsync();
                BtnSearch.IsEnabled = true;
            }
            else
            {
                BtnSearch.IsEnabled = false;
                ScrollResults.Content = await ActiveIndicator.DisplayBusy();
                ScrollResults.Content = await ArtistStackAsync();
                BtnSearch.IsEnabled = true;
            }
        }

        #region Search Methods
        private async Task<AlbumAndArtistSearchModel> ArtistOrAlbumSearchAsync()
        {
            if(SearchText != "" && SearchText != null)
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var SearchResults = await SearchArtistAlbum.Search(SearchType, SearchText);
                    return SearchResults;
                }
                else
                {
                    DependencyService.Get<IMessageCenter>().ShortMessage("Please check connectivity and try again");
                    return null;
                }
            }
            else
            {
                DependencyService.Get<IMessageCenter>().ShortMessage("Please type text to continue");
                return null;
            }
        }
        #endregion

        #region GridView
        private async Task<StackLayout> AlbumStackAsync()
        {
            StackLayout albumStack = new StackLayout();
            var JuxResults = await ArtistOrAlbumSearchAsync();
            if(JuxResults != null)
            {

                var ResultCount = JuxResults.Results.Count;
                var CountMatch = 0;

                string Artist = "";
                string Album = "";
                string Url = "";
                int Id = 0;

                await Task.Run(() =>
                {
                    for (int i = 0; i <= ResultCount - 1; i++)
                    {
                        albumView = new AlbumView();

                        Artist = ConvertJuxString.Decode(JuxResults.Results[i].Artist);
                        Album = ConvertJuxString.Decode(JuxResults.Results[i].Album);
                        Url = JuxResults.Results[i].Album_Picture;
                        Id = JuxResults.Results[i].Id;

                        albumView.Artist = Artist;
                        albumView.Album = Album;
                        albumView.AlbumId = Id;
                        albumView.ImageUrl = Url;
                        albumView.DownloadCount = $" ";

                        if (Album.ToLower().Contains(SearchText.ToLower()))
                        {
                            CountMatch += 1;
                            albumStack.Children.Add(albumView);
                        }
                    }
                    if (CountMatch < 1)
                    {
                        albumStack.Children.Add(new Label
                        {
                            Text = $"Nothing was found for {SearchText}, try with another Title.",
                            FontSize = 15,
                            TextColor = Color.White,
                            LineBreakMode = LineBreakMode.WordWrap,
                            HorizontalTextAlignment = TextAlignment.Center
                        });
                    }
                });
            }
            return albumStack;
        }

        private async Task<StackLayout> ArtistStackAsync()
        {
            StackLayout artistStack = new StackLayout();
            var JuxResults = await ArtistOrAlbumSearchAsync();
            if (JuxResults != null)
            {

                var ResultCount = JuxResults.Results.Count;
                var CountMatch = 0;

                string Artist = "";
                string Album = "";
                string Url = "";
                int Id = 0;

                await Task.Run(() =>
                {
                    for (int i = 0; i <= ResultCount - 1; i++)
                    {
                        artistView = new ArtistView();

                        Artist = ConvertJuxString.Decode(JuxResults.Results[i].Artist);
                        Album = ConvertJuxString.Decode(JuxResults.Results[i].Album);
                        Url = JuxResults.Results[i].Artist_Picture.Replace("%d", "500");
                        Id = JuxResults.Results[i].Id;

                        artistView.Artist = Artist;
                        artistView.NumberOfAlbums = JuxResults.Results[i].Number_Of_Songs;
                        artistView.ImageUrl = Url;
                        artistView.ArtistId = Id;

                        if (Artist.ToLower().Contains(SearchText.ToLower()))
                        {
                            CountMatch += 1;
                            artistStack.Children.Add(artistView);
                        }
                    }
                    if (CountMatch < 1)
                    {
                        artistStack.Children.Add(new Label
                        {
                            Text = $"Nothing was found for {SearchText}, try with another Title.",
                            FontSize = 15,
                            TextColor = Color.White,
                            LineBreakMode = LineBreakMode.WordWrap,
                            HorizontalTextAlignment = TextAlignment.Center
                        });
                    }
                });
            }
            return artistStack;
        }
        #endregion
    }
}
