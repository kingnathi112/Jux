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
using System.Linq;

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
        int ResultNum = 0;
        int limit = 30;
        int index = 0;
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
                RadioNewAlbum.IsSelected = false;
                SearchType = Category.Search.Album;
                limit = 30;
                if (TxtSearch.Text != "")
                {
                    BtnSearch_Clicked(s, e);
                }
            };
            RadioSong.OnTapped += (s, e) =>
            {
                RadioArtist.IsSelected = false;
                RadioNewAlbum.IsSelected = false;
                RadioAlbum.IsSelected = false;
            }; 
            RadioNewAlbum.OnTapped += RadioNewAlbum_OnTapped;
            RadioArtist.OnTapped += (s, e) =>
            {
                RadioAlbum.IsSelected = false;
                RadioSong.IsSelected = false;
                RadioNewAlbum.IsSelected = false;
                SearchType = Category.Search.Artist;
                limit = 30;
                if (TxtSearch.Text != "")
                {
                    BtnSearch_Clicked(s, e);
                }
            };
        }

        private async void RadioNewAlbum_OnTapped(object sender, EventArgs e)
        {
            RadioArtist.IsSelected = false;
            RadioSong.IsSelected = false;
            RadioAlbum.IsSelected = false;
            limit = 50;

            StackNavigation.IsVisible = false;

            if (RadioNewAlbum.IsSelected)
            {
                BtnSearch.IsEnabled = false;
                ScrollResults.Content = await ActiveIndicator.DisplayBusy();
                ScrollResults.Content = await NewAlbumsAsync();
                BtnSearch.IsEnabled = true;
            }
        }

        void CheckSelectedButton()
        {
            if (RadioAlbum.IsSelected)
            {
                SearchType = Category.Search.Album;
            }
            else if(RadioArtist.IsSelected)
            {
                SearchType = Category.Search.Artist;
            }
            else if (RadioSong.IsSelected)
            {
                SearchType = Category.Search.Song;
            }
            else if (RadioNewAlbum.IsSelected)
            {
                SearchType = Category.Search.NewAlbums;
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
            //await Navigation.PushPopupAsync(new FileExplorer(), true);
            BtnLink.IsEnabled = true;
        }

        private async void BtnSearch_Clicked(object sender, EventArgs e)
        {
            SearchText = TxtSearch.Text.Trim();
            StackNavigation.IsVisible = false;
            if (RadioAlbum.IsSelected)
            {
                BtnSearch.IsEnabled = false;
                ScrollResults.Content = await ActiveIndicator.DisplayBusy();
                ScrollResults.Content = await AlbumStackAsync();
                BtnSearch.IsEnabled = true;

                if (ResultNum > 30)
                {
                    StackNavigation.IsVisible = true;
                }
                else
                {
                    StackNavigation.IsVisible = false;
                }
            }
            else
            {
                BtnSearch.IsEnabled = false;
                ScrollResults.Content = await ActiveIndicator.DisplayBusy();
                ScrollResults.Content = await ArtistStackAsync();
                BtnSearch.IsEnabled = true;

                if (ResultNum > 30)
                {
                    StackNavigation.IsVisible = true;
                }
                else
                {
                    StackNavigation.IsVisible = false;
                }
            }
        }

        #region Search Methods
        private async Task<AlbumAndArtistSearchModel> ArtistOrAlbumSearchAsync(int Index = 0, int Limit = 30)
        {
            if(SearchText != "" && SearchText != null)
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var SearchResults = await SearchArtistAlbum.Search(SearchType, SearchText, Index, Limit);
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
            var JuxResults = await ArtistOrAlbumSearchAsync(index, limit);
            if(JuxResults != null)
            {

                var ResultCount = JuxResults.Results.Count;
                ResultNum = JuxResults.Total;
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
                        StackNavigation.IsVisible = false;
                    }
                });
            }
            return albumStack;
        }

        private async Task<StackLayout> ArtistStackAsync()
        {
            StackLayout artistStack = new StackLayout();
            var JuxResults = await ArtistOrAlbumSearchAsync(index, limit);
            if (JuxResults != null)
            {

                var ResultCount = JuxResults.Results.Count;
                ResultNum = JuxResults.Total;
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
                        StackNavigation.IsVisible = false;
                    }
                });
            }
            return artistStack;
        }

        private async Task<StackLayout> NewAlbumsAsync()
        {
            var stackNewAlbums = new StackLayout();
            var juxResults = await SearchArtistAlbum.NewAlbums(index, limit);

            if (juxResults != null)
            {
                var ResultCount = juxResults.Albums.Albums.Count;
                var Albums = juxResults.Albums.Albums;
                ResultNum = juxResults.Albums.Total_Count;

                string Artist = "";
                string Album = "";
                string Url = "";
                string Id = "";
                string ReleaseDate = "";

                await Task.Run(() => 
                { 
                    for(int i = 0; i <= ResultCount - 1; i++)
                    {
                        albumView = new AlbumView();

                         Artist = Albums[i].Artists.Select(a => a.ArtistName).FirstOrDefault();
                         Album = Albums[i].AlbumTitle;
                         Url = Albums[i].AlbumCover.Where(p => p.Height > 300).Select(p => p.Url).FirstOrDefault();
                         Id = Albums[i].AlbumId;
                         ReleaseDate = Convert.ToDateTime(Albums[i].Date).Year.ToString();

                        albumView.Artist = Artist;
                        albumView.Album = Album;
                        albumView.AlbumIdStr = Id;
                        albumView.ImageUrl = Url;
                        albumView.Year = ReleaseDate;
                        albumView.DownloadCount = $" ";

                        stackNewAlbums.Children.Add(albumView);
                    }                
                });
            }
            else
            {
                stackNewAlbums.Children.Add(new Label
                {
                    Text = $"Nothing was found for {SearchText}, try with another Title.",
                    FontSize = 15,
                    TextColor = Color.White,
                    LineBreakMode = LineBreakMode.WordWrap,
                    HorizontalTextAlignment = TextAlignment.Center
                });
                StackNavigation.IsVisible = false;
            }
            return stackNewAlbums;
        }
        #endregion

        private void BtnLast_Clicked(object sender, EventArgs e)
        {
            index = ResultNum - 30;
            limit = ResultNum;
            BtnSearch_Clicked(sender, e);

            BtnLast.IsEnabled = false;
            BtnLast.Source = "LastDisabled.png";

            BtnNext.IsEnabled = false;
            BtnNext.Source = "NextDisabled.png";

            BtnFirst.IsEnabled = true;
            BtnFirst.Source = "First.png";

            BtnPrevious.IsEnabled = true;
            BtnPrevious.Source = "Previous.png";
        }

        private void BtnFirst_Clicked(object sender, EventArgs e)
        {
            index = 0;
            limit = 30;
            BtnSearch_Clicked(sender, e);

            BtnFirst.IsEnabled = false;
            BtnFirst.Source = "FirstDisabled.png";

            BtnPrevious.IsEnabled = false;
            BtnPrevious.Source = "PreviousDisabled.png";

            BtnLast.IsEnabled = true;
            BtnLast.Source = "Last.png";

            BtnNext.IsEnabled = true;
            BtnNext.Source = "Next.png";
        }

        private void BtnNext_Clicked(object sender, EventArgs e)
        {
            index += 30;
            limit += 30;

            if(limit <= ResultNum)
            {
                BtnFirst.IsEnabled = true;
                BtnFirst.Source = "First.png";

                BtnPrevious.IsEnabled = true;
                BtnPrevious.Source = "Previous.png";
                BtnSearch_Clicked(sender, e);
            }
            else
            {
                BtnLast.IsEnabled = false;
                BtnLast.Source = "LastDisabled.png";

                BtnNext.IsEnabled = false;
                BtnNext.Source = "NextDisabled.png";
            }
        }

        private void BtnPrevious_Clicked(object sender, EventArgs e)
        {
            index -= 30;
            limit -= 30;

            if (index >= 0)
            {
                BtnLast.IsEnabled = true;
                BtnLast.Source = "Last.png";

                BtnNext.IsEnabled = true;
                BtnNext.Source = "Next.png";

                BtnSearch_Clicked(sender, e);
            }
            else
            {
                BtnFirst.IsEnabled = false;
                BtnFirst.Source = "FirstDisabled.png";
                BtnPrevious.IsEnabled = false;
                BtnPrevious.Source = "PreviousDisabled.png";
            }
        }
    }
}
