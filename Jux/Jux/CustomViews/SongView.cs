using Jux.Data.Song;
using Jux.Helpers;
using Jux.Interface;
using Jux.Views;
using MediaManager;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Jux.Data.Search_Category.Category;

namespace Jux.CustomViews
{
    public class SongView: Frame
    {
        #region Properties
        public string Artist
        {
            get => (string)GetValue(ArtistProperty);
            set => SetValue(ArtistProperty, value);
        }
        public string SongId { get; set; }
        public string Album
        {
            get => (string)GetValue(AlbumProperty);
            set => SetValue(AlbumProperty, value);
        }
        public string ImageUrl
        {
            get => (string)GetValue(ImageUrlProperty);
            set => SetValue(ImageUrlProperty, value);
        }
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public int NumberOfAlbums { get; set; }
        public string Duration
        {
            get => (string)GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }

        string Year = "";
        bool IsPlaying = false;
        bool Quality;
        bool SaveImage;
        #endregion

        #region BindableProperty
        private static readonly BindableProperty ArtistProperty =
            BindableProperty.Create("Artist", typeof(string), typeof(SongView), string.Empty,
            BindingMode.TwoWay, propertyChanged: ArtistPropertyChanged);

        private static readonly BindableProperty AlbumProperty =
            BindableProperty.Create("Album", typeof(string), typeof(SongView), string.Empty,
            BindingMode.TwoWay, propertyChanged: AlbumPropertyChanged);

        private static readonly BindableProperty TitleProperty =
            BindableProperty.Create("Title", typeof(string), typeof(SongView), string.Empty,
            BindingMode.TwoWay, propertyChanged: TitlePropertyChanged);

        private static readonly BindableProperty ImageUrlProperty =
            BindableProperty.Create("ImageUrl", typeof(string), typeof(SongView), string.Empty,
            BindingMode.TwoWay, propertyChanged: ImageUrlPropertyChanged);

        private static readonly BindableProperty DurationProperty =
            BindableProperty.Create("Duration", typeof(string), typeof(SongView), string.Empty,
            BindingMode.TwoWay, propertyChanged: DurationPropertyChanged);
        #endregion

        #region PropertyChanged
        private static void ImageUrlPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SongView)bindable;
            if (control != null)
                control.ImageAlbum.Source = (string)newValue;
        }
        private static void TitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SongView)bindable;
            if (control != null)
                control.LblTitle.Text = (string)newValue;
        }
        private static void AlbumPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SongView)bindable;
            if (control != null)
                control.LblAlbum.Text = (string)newValue;
        }
        private static void ArtistPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SongView)bindable;
            if (control != null)
                control.LblArtist.Text = (string)newValue;
        }
        private static void DurationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SongView)bindable;
            if (control != null)
                control.LblDuration.Text = (string)newValue;
        }
        #endregion

        #region Controls
        Image ImageAlbum;
        ImageButton BtnDownload;
        ImageButton BtnPlay;

        Label LblDuration;
        Label LblArtist;
        Label LblAlbum;
        Label LblTitle;

        StackLayout AudioStack;

        Grid SongGrid;

        ProgressBar DownloadProgress;

        DownloadHelper download;
        #endregion

        public event EventHandler OnTapped;

        public SongView()
        {
            BackgroundColor = (Color)App.Current.Resources["Icon"];
            Padding = new Thickness(0, 0, 0, 5);
            CornerRadius = 5;

            ImageAlbum = new Image             
            {
                Source = ImageUrl,
                HorizontalOptions = LayoutOptions.Start,
                WidthRequest = 87,
                HeightRequest = 87
            };

            BtnDownload = new ImageButton 
            {
                Source = "Download.png",
                HeightRequest = 25,
                WidthRequest = 25,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.Center
            };
            BtnPlay = new ImageButton 
            {
                Source = "Play.png",
                HeightRequest = 25,
                WidthRequest = 25,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.Center
            };

            LblDuration = new Label 
             {
                 Text = $"{Duration}",
                 Style = (Style)App.Current.Resources["LightLabel"],
                 HorizontalTextAlignment = TextAlignment.Center,
                 TextColor = Color.White
             };
            LblArtist = new Label
             {
                 Text = $"{Artist}",
                 Style = (Style)App.Current.Resources["LightLabel"],
                 TextColor = Color.White
             }; 
            LblAlbum = new Label
             {
                 Text = $"{Album}",
                 Style = (Style)App.Current.Resources["LightLabel"],
                 TextColor = Color.White
             }; 
            LblTitle = new Label
             {
                 Text = $"{Title}",
                 Style = (Style)App.Current.Resources["BoldLabel"],
                 TextColor = Color.White
             };

            DownloadProgress = new ProgressBar
            {
                ProgressColor = (Color)App.Current.Resources["Icon"],
                HorizontalOptions = LayoutOptions.Center
            };

            SongGrid = new Grid
            {
                BackgroundColor = (Color)App.Current.Resources["Background"]
            };

            AudioStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing =  10
            };

            BtnPlay.Clicked += BtnPlay_Clicked;
            BtnDownload.Clicked += BtnDownload_Clicked;

            Quality = TempSettings.Quality;
            SaveImage = TempSettings.SaveImage;

            SetGridView();
            Content = SongGrid;
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
            DependencyService.Get<IMessageCenter>().ShortMessage($"Downloading {Title}");
            await Task.Run(() => DownloadMusic());
            BackgroundColor = Color.Green;

            DependencyService.Get<IMessageCenter>().ShortMessage($"{Artist} - {Title} Downloaded!");
            DownloadProgress.Progress = 0;        
        }

        private void BtnPlay_Clicked(object sender, EventArgs e)
        {
            // Method to Play A Song
            DependencyService.Get<IMessageCenter>().LongMessage($"Playing {Artist} - {Title}, \r\nSong will start soon");
            PlaySong();
            OnTapped?.Invoke(sender, e);
        }

        private async void PlaySong()
        {
            var song = await GetSongDetails.ById(SongId);

            if (IsPlaying)
            {
                await CrossMediaManager.Current.Stop();
                IsPlaying = false;
            }
            else
            {
                await CrossMediaManager.Current.Play(song.Normal_Quality);
                IsPlaying = true;
            }
        }

        private void SetGridView()
        {
            AudioStack.Children.Add(BtnPlay);
            AudioStack.Children.Add(BtnDownload);

            SongGrid.Children.Add(ImageAlbum, 0, 0);
            SongGrid.Children.Add(LblTitle, 1, 0);
            SongGrid.Children.Add(LblArtist, 1, 1);
            SongGrid.Children.Add(LblAlbum, 1, 2);
            SongGrid.Children.Add(LblDuration, 2, 1);
            SongGrid.Children.Add(AudioStack, 3, 1);
            SongGrid.Children.Add(DownloadProgress, 3, 2);

            Grid.SetRowSpan(ImageAlbum, 3);
            Grid.SetColumnSpan(LblTitle, 3);
            Grid.SetColumnSpan(LblArtist, 2);
            Grid.SetColumnSpan(LblAlbum, 3);
        }

        private void DownloadMusic()
        {
            var Song = GetSongDetails.ById(SongId).Result;
            Year = $"{Convert.ToDateTime(Song.Date).Year}";
            var AlbumTitle = $"{Year} - {Album}";
            var DownloadPath = DependencyService.Get<IFolderManager>().Music(Artist, AlbumTitle);

            var AlbumPicture = "";

            double progress = 0.0;

            if (Quality)
            {
                var Title = Song.Title;
                var Number = Song.Number;
                var Url = Song.High_Quality;
                AlbumPicture = Song.Album_Image;

                download = new DownloadHelper();
                if (Url != "")
                {
                    var Downloaded = download.Song(Download.Single, Url, Artist, Album, Title, DownloadPath, "mp3", Song.High_Quality_Size, Number);
                    var Completed = false;
                    if (!download.IsExisting)
                    {
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
                    Url = Song.Normal_Quality;

                    if(Url != "")
                    {
                        DependencyService.Get<IMessageCenter>().ShortMessage($"{Title} will be downloaded in normal quality");
                        var Downloaded = download.Song(Download.Single, Url, Artist, Album, Title, DownloadPath, "mp3", Song.Normal_Quality_Size, Number);
                        var Completed = false;
                        if (!download.IsExisting)
                        {
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
            else
            {
                var Title = Song.Title;
                var Number = Song.Number;
                var Url = Song.Normal_Quality;
                AlbumPicture = Song.Album_Image;

                if(Url !="")
                {
                    download = new DownloadHelper();

                    var Downloaded = download.Song(Download.Single, Url, Artist, Album, Title, DownloadPath, "mp3", Song.Normal_Quality_Size, Number);

                    var Completed = false;
                    if (!download.IsExisting)
                    {
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

            if (SaveImage)
            {
                var ImgUrl = AlbumPicture;
                var DownloadImage = download.AlbumImage(ImgUrl, Album, DownloadPath);
            }
        }
    }
}
