using Jux.Data.Album;
using Jux.Data.Search_Category;
using Jux.Data.Song;
using Jux.Helpers;
using Jux.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Jux.Data.Search_Category.Category;

namespace Jux.CustomViews
{
   public class AlbumView: Frame
    {
        #region Propeties
        public string Artist
        {
            get => (string)GetValue(ArtistProperty);
            set => SetValue(ArtistProperty, value);
        }
        public string Album
        {
            get => (string)GetValue(AlbumProperty);
            set => SetValue(AlbumProperty, value);
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
        public int AlbumId = 0;

        bool Quality;
        bool SaveImage;
        #endregion

        #region BindableProperty
        private static readonly BindableProperty ArtistProperty =
            BindableProperty.Create("Artist", typeof(string), typeof(AlbumView), string.Empty,
            BindingMode.TwoWay, propertyChanged: ArtistPropertyChanged);

        private static readonly BindableProperty AlbumProperty =
            BindableProperty.Create("Album", typeof(string), typeof(AlbumView), string.Empty,
            BindingMode.TwoWay, propertyChanged: AlbumPropertyChanged);

        private static readonly BindableProperty YearProperty =
            BindableProperty.Create("Year", typeof(string), typeof(AlbumView), string.Empty,
            BindingMode.TwoWay, propertyChanged: YearPropertyChanged);       
        
        private static readonly BindableProperty NumberOfSongsProperty =
            BindableProperty.Create("Number_Of_Songs", typeof(string), typeof(AlbumView), string.Empty,
            BindingMode.TwoWay, propertyChanged: NumberOfSongsPropertyChanged);

        private static readonly BindableProperty ImageUrlProperty =
            BindableProperty.Create("ImageUrl", typeof(string), typeof(AlbumView), string.Empty,
            BindingMode.TwoWay, propertyChanged: ImageUrlPropertyChanged);

        private static readonly BindableProperty DownloadCountProperty =
            BindableProperty.Create("DownloadCount", typeof(string), typeof(AlbumView), string.Empty,
            BindingMode.TwoWay, propertyChanged: DownloadCountPropertyChanged);
        #endregion

        #region PropertyChanged
        private static void ImageUrlPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (AlbumView)bindable;
            if (control != null)
                control.ImgAlbum.Source = (string)newValue;
        }
        private static void YearPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (AlbumView)bindable;
            if (control != null)
                control.LblYear.Text = (string)newValue;
        }       
        private static void NumberOfSongsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (AlbumView)bindable;
            if (control != null)
                control.LblDownloadCount.Text = (string)newValue;
        }
        private static void AlbumPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (AlbumView)bindable;
            if (control != null)
                control.LblAlbum.Text = (string)newValue;
        }
        private static void ArtistPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (AlbumView)bindable;
            if (control != null)
                control.LblArtist.Text = (string)newValue;
        }
        private static void DownloadCountPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (AlbumView)bindable;
            if (control != null)
                control.LblDownloadCount.Text = (string)newValue;
        }
        #endregion

        public event EventHandler OnTapped;

        #region Controls
        Grid AlbumGrid;

        Image ImgAlbum;
        ImageButton BtnDownload;

        string NumberOfSongs = "";

        Label LblArtist;
        Label LblAlbum;
        Label LblYear;
        Label LblDownloadCount;

        ProgressBar DownloadProgress;

        DownloadHelper download;

        #endregion

        public AlbumView()
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
            LblAlbum = new Label
            {
                Text = Album,
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

            AlbumGrid = new Grid
            {
                BackgroundColor = (Color)App.Current.Resources["Background"]
            };

            var Gesture = new TapGestureRecognizer();
            Gesture.Tapped += Gesture_Tapped; ;
            GestureRecognizers.Add(Gesture);

            SetGridView();
            Content = AlbumGrid;           
        }

        private void Gesture_Tapped(object sender, EventArgs e)
        {
            UpdateWithYearAndNumberOfSongs();
            OnTapped?.Invoke(sender, e);
        }

        public async void UpdateWithYearAndNumberOfSongs()
        {
            var AlbumDetails =  await GetAlbumDetails.ById(AlbumId);
            LblDownloadCount.Text = $"{AlbumDetails.NumberOfSongs}";
            string year = ConvertJuxString.Decode(AlbumDetails.AlbumInformation.Date);
            LblYear.Text = $"{Convert.ToDateTime(year).Year}";
        }
        private async void BtnDownload_Clicked(object sender, EventArgs e)
        {
            download = new DownloadHelper();
           if(LblDownloadCount.Text == " " || LblDownloadCount.Text == "")
            {
                UpdateWithYearAndNumberOfSongs();
            }

            DependencyService.Get<IMessageCenter>().LongMessage($"Downloading {Album}");
            await Task.Run(() => DownloadMusic());
            BackgroundColor = Color.Green;

            DependencyService.Get<IMessageCenter>().ShortMessage($"{Artist} - {Album} Downloaded!");
            DownloadProgress.Progress = 0;
        }
        private void SetGridView()
        {
            // AlbumGrid.Children.Add(LblArtist, Column, Row);
            AlbumGrid.Children.Add(ImgAlbum, 0, 0);
            AlbumGrid.Children.Add(LblAlbum, 1, 0);
            AlbumGrid.Children.Add(LblArtist, 1, 1);
            AlbumGrid.Children.Add(LblYear, 1, 2);
            AlbumGrid.Children.Add(LblDownloadCount, 2, 2);
            AlbumGrid.Children.Add(DownloadProgress, 3, 2);
            AlbumGrid.Children.Add(BtnDownload, 3, 1);

            Grid.SetRowSpan(ImgAlbum, 3);
            Grid.SetColumnSpan(LblAlbum, 3);
            Grid.SetColumnSpan(LblArtist, 3);
        }
        private void DownloadMusic()
        {
            var AlbumDetails = GetAlbumDetails.ById(AlbumId).Result;
            string year = ConvertJuxString.Decode(AlbumDetails.AlbumInformation.Date);
            Year = $"{Convert.ToDateTime(year).Year}";
            NumberOfSongs = $"{AlbumDetails.NumberOfSongs}";
            var AlbumTitle = $"{Year} - {Album}";
            var DownloadPath = DependencyService.Get<IFolderManager>().Music(Artist, AlbumTitle);

            var AlbumPicture = "";

            double progress = 0.0;

            if (Quality)
            {
                foreach(var Song in AlbumDetails.AlbumInformation.Songs)
                {
                    var SongId = Song.Id;
                    var Title = ConvertJuxString.Decode(Song.Title);
                    var Number = Song.Number;

                    var SongDetail = GetSongDetails.ById(SongId).Result;
                    var Url = SongDetail.High_Quality;
                    AlbumPicture = SongDetail.Album_Image;

                    download = new DownloadHelper();

                    if (Url != "" || Url != null)
                    {
                            var Downloaded = download.Song(Download.Album, Url, Artist, Album, Title, DownloadPath, "mp3", Song.High_Quality_Size, Number);                        
                    }
                    else
                    {
                            DependencyService.Get<IMessageCenter>().ShortMessage($"{Title} will be downloaded in normal quality");
                            var Downloaded = download.Song(Download.Album, Url, Artist, Album, Title, DownloadPath, "mp3", Song.Normal_Quality_Size, Number);
                    }

                    var Completed = false;

                    if (!download.IsExisting)
                    {
                        Device.BeginInvokeOnMainThread(() => LblDownloadCount.Text = $"{Number}/{NumberOfSongs}");
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
            else
            {
                foreach (var Song in AlbumDetails.AlbumInformation.Songs)
                {
                    var SongId = Song.Id;
                    var Title = ConvertJuxString.Decode(Song.Title);
                    var Number = Song.Number;

                    var SongDetail = GetSongDetails.ById(SongId).Result;
                    var Url = SongDetail.Normal_Quality;
                    AlbumPicture = SongDetail.Album_Image;

                    download = new DownloadHelper();

                    var Downloaded = download.Song(Download.Album, Url, Artist, Album, Title, DownloadPath, "mp3", Song.Normal_Quality_Size, Number);

                    var Completed = false;

                    if (!download.IsExisting)
                    {
                        Device.BeginInvokeOnMainThread(() => LblDownloadCount.Text = $"{Number}/{NumberOfSongs}");
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
