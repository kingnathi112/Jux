using Jux.Data.Song;
using Jux.Helpers;
using Jux.Interface;
using Jux.Models.PlaylistModel;
using Jux.Views;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Jux.Data.Search_Category.Category;

namespace Jux.CustomViews
{
   public class PlaylistView: Frame
    {
        #region Propeties
        public string Artist
        {
            get => (string)GetValue(ArtistProperty);
            set => SetValue(ArtistProperty, value);
        }

        public Songs SongList{get;set;}
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public string Year
        {
            get => (string)GetValue(YearProperty);
            set => SetValue(YearProperty, value);
        }
        public string Number_Of_Songs
        {
            get => (string)GetValue(NumberOfSongsProperty);
            set => SetValue(NumberOfSongsProperty, value);
        }
        public string ImageUrl
        {
            get => (string)GetValue(ImageUrlProperty);
            set => SetValue(ImageUrlProperty, value);
        }
        public string DownloadCount
        {
            get => (string)GetValue(DownloadCountProperty);
            set => SetValue(DownloadCountProperty, value);
        }

        bool Quality;
        bool SaveImage;
        #endregion

        #region BindableProperty
        private static readonly BindableProperty ArtistProperty =
            BindableProperty.Create("Artist", typeof(string), typeof(PlaylistView), string.Empty,
            BindingMode.TwoWay, propertyChanged: ArtistPropertyChanged);        

        private static readonly BindableProperty TitleProperty =
            BindableProperty.Create("Title", typeof(string), typeof(PlaylistView), string.Empty,
            BindingMode.TwoWay, propertyChanged: TitlePropertyChanged);

        private static readonly BindableProperty YearProperty =
            BindableProperty.Create("Year", typeof(string), typeof(PlaylistView), string.Empty,
            BindingMode.TwoWay, propertyChanged: YearPropertyChanged);

        private static readonly BindableProperty NumberOfSongsProperty =
            BindableProperty.Create("Number_Of_Songs", typeof(string), typeof(PlaylistView), string.Empty,
            BindingMode.TwoWay, propertyChanged: NumberOfSongsPropertyChanged);

        private static readonly BindableProperty ImageUrlProperty =
            BindableProperty.Create("ImageUrl", typeof(string), typeof(PlaylistView), string.Empty,
            BindingMode.TwoWay, propertyChanged: ImageUrlPropertyChanged);

        private static readonly BindableProperty DownloadCountProperty =
            BindableProperty.Create("DownloadCount", typeof(string), typeof(PlaylistView), string.Empty,
            BindingMode.TwoWay, propertyChanged: DownloadCountPropertyChanged);
        #endregion

        #region PropertyChanged
        private static void ImageUrlPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (PlaylistView)bindable;
            if (control != null)
                control.ImgAlbum.Source = (string)newValue;
        }       
        
        private static void YearPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (PlaylistView)bindable;
            if (control != null)
                control.LblYear.Text = (string)newValue;
        }
        private static void NumberOfSongsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (PlaylistView)bindable;
            if (control != null)
                control.LblDownloadCount.Text = (string)newValue;
        }
        private static void TitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (PlaylistView)bindable;
            if (control != null)
                control.LblTitle.Text = (string)newValue;
        }
        private static void ArtistPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (PlaylistView)bindable;
            if (control != null)
                control.LblArtist.Text = (string)newValue;
        }
        private static void DownloadCountPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (PlaylistView)bindable;
            if (control != null)
                control.LblDownloadCount.Text = (string)newValue;
        }
        #endregion

        public event EventHandler OnTapped;

        #region Controls
        Grid PlaylistGrid;

        Image ImgAlbum;
        ImageButton BtnDownload;

        Label LblArtist;
        Label LblTitle;
        Label LblYear;
        Label LblDownloadCount;


        ProgressBar DownloadProgress;

        DownloadHelper download;

        #endregion

        public PlaylistView()
        {
            BackgroundColor = (Color)App.Current.Resources["Icon"];
            Padding = new Thickness(0, 0, 0, 5);
            CornerRadius = 5;
            
            LblArtist = new Label
            {
                Text = Artist,
                Style = (Style)App.Current.Resources["LightLabel"],
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            LblTitle = new Label
            {
                Text = Title,
                Style = (Style)App.Current.Resources["BoldLabel"],
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            LblYear = new Label
            {
                Text = Year,
                Style = (Style)App.Current.Resources["LightLabel"],
                TextColor = Color.White
            };
            LblDownloadCount = new Label
            {
                Text = Number_Of_Songs,
                Style = (Style)App.Current.Resources["LightLabel"],
                TextColor = Color.White
            };

            DownloadProgress = new ProgressBar
            {
                ProgressColor = (Color)App.Current.Resources["Icon"],
                HorizontalOptions = LayoutOptions.End
            };

            Quality = TempSettings.Quality;
            SaveImage = TempSettings.SaveImage;

            BtnDownload = new ImageButton
            {
                Source = "Download.png",
                HeightRequest = 25,
                WidthRequest = 25,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.End
            };

            BtnDownload.Clicked += BtnDownload_Clicked;
            ImgAlbum = new Image
            {
                Source = ImageUrl,
                HorizontalOptions = LayoutOptions.Start,
                WidthRequest = 87,
                HeightRequest = 87
            };

            PlaylistGrid = new Grid
            {
                BackgroundColor = (Color)App.Current.Resources["Background"]
            };

            var Gesture = new TapGestureRecognizer();
            Gesture.Tapped += Gesture_Tapped; ;
            GestureRecognizers.Add(Gesture);

            SetGridView();
            Content = PlaylistGrid;
        }

        private void SetGridView()
        {
            PlaylistGrid.Children.Add(ImgAlbum, 0, 0);
            PlaylistGrid.Children.Add(LblTitle, 1, 0);
            PlaylistGrid.Children.Add(LblArtist, 1, 1);
            PlaylistGrid.Children.Add(LblYear, 1, 2);
            PlaylistGrid.Children.Add(LblDownloadCount, 2, 2);
            PlaylistGrid.Children.Add(DownloadProgress, 3, 2);
            PlaylistGrid.Children.Add(BtnDownload, 3, 1);

            Grid.SetRowSpan(ImgAlbum, 3);
            Grid.SetColumnSpan(LblTitle, 3);
            Grid.SetColumnSpan(LblArtist, 3);
        }

        private async void Gesture_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new AlbumsAndSongs(SongList), true);
            OnTapped?.Invoke(sender, e);
        }

        private async void BtnDownload_Clicked(object sender, EventArgs e)
        {
            PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();

            if (status != PermissionStatus.Granted)
            {
                await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage);
                status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                DependencyService.Get<IMessageCenter>().ShortMessage($"Storage Request {status.ToString()}");
            }
            DependencyService.Get<IMessageCenter>().LongMessage($"Downloading {Title}");
            await Task.Run(() => DownloadMusic());
            BackgroundColor = Color.Green;

            DependencyService.Get<IMessageCenter>().ShortMessage($"{Title} Downloaded!");
            DownloadProgress.Progress = 0;
        }

        private void DownloadMusic()
        {
            var AlbumTitle = $"{Year} - {Title}";
            var DownloadPath = DependencyService.Get<IFolderManager>().Music("Playlists", AlbumTitle);

            var AlbumPicture = ImageUrl;
            double progress = 0.0;
            int count = 0;
            if (Quality)
            {
                foreach (var Song in SongList.Details)
                {
                    var SongInfo = GetSongDetails.ById(Song.SongId).Result;
                    var Url = SongInfo.High_Quality;
                    var SongTitle = SongInfo.Title;
                    var Number = SongInfo.Number;

                    download = new DownloadHelper();

                    count += 1;
                    if(Url != "")
                    {
                        var Downloaded = download.Song(Download.Single, Url, SongInfo.Artist, Title, SongTitle, DownloadPath, "mp3", SongInfo.High_Quality_Size, count);
                        var Completed = false;
                        if (!download.IsExisting)
                        {
                            Device.BeginInvokeOnMainThread(() => LblDownloadCount.Text = $"{count}/{Number_Of_Songs}");
                            do
                            {
                                if (download.downloadProgress != 0)
                                {
                                    progress = (double)download.downloadProgress / 100;
                                    Device.BeginInvokeOnMainThread(() => DownloadProgress.Progress = progress);
                                    Completed = download.downloadCompleted;
                                }
                            } while (Completed == false);
                        }
                    }
                    else
                    {
                        Url = SongInfo.Normal_Quality;
                        if(Url != "")
                        {
                            DependencyService.Get<IMessageCenter>().ShortMessage($"{SongTitle} will be downloaded in normal quality");
                            var Downloaded = download.Song(Download.Single, SongInfo.Normal_Quality, SongInfo.Artist, Title, SongTitle, DownloadPath, "mp3", SongInfo.Normal_Quality_Size, count);
                            var Completed = false;
                            if (!download.IsExisting)
                            {
                                Device.BeginInvokeOnMainThread(() => LblDownloadCount.Text = $"{count}/{Number_Of_Songs}");
                                do
                                {
                                    if (download.downloadProgress != 0)
                                    {
                                        progress = (double)download.downloadProgress / 100;
                                        Device.BeginInvokeOnMainThread(() => DownloadProgress.Progress = progress);
                                        Completed = download.downloadCompleted;
                                    }
                                } while (Completed == false);
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var Song in SongList.Details)
                {
                    var SongInfo = GetSongDetails.ById(Song.SongId).Result;
                    var Url = SongInfo.Normal_Quality;
                    var SongTitle = SongInfo.Title;
                    var Number = SongInfo.Number;
                    count += 1;

                    download = new DownloadHelper();
                    if (Url != "")
                    {
                        var Downloaded = download.Song(Download.Single, Url, SongInfo.Artist, Title, SongTitle, DownloadPath, "mp3", SongInfo.Normal_Quality_Size, count);
                        var Completed = false;
                        if (!download.IsExisting)
                        {
                            Device.BeginInvokeOnMainThread(() => LblDownloadCount.Text = $"{count}/{Number_Of_Songs}");
                            do
                            {
                                if (download.downloadProgress != 0)
                                {
                                    progress = (double)download.downloadProgress / 100;
                                    Device.BeginInvokeOnMainThread(() => DownloadProgress.Progress = progress);
                                    Completed = download.downloadCompleted;
                                }
                            } while (Completed == false);
                        }
                    }
                }
            }


            if (SaveImage)
            {
                var ImgUrl = AlbumPicture;
                var DownloadImage = download.AlbumImage(ImgUrl, Title, DownloadPath);
            }
        }
    }
}
