using Jux.Data.Album;
using Jux.Helpers;
using Jux.Interface;
using Jux.Views;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Jux.CustomViews
{
   public class ArtistView: Frame
    {
        #region Properties
        public string Artist
        {
            get => (string)GetValue(ArtistProperty);
            set => SetValue(ArtistProperty, value);
        }

        public int ArtistId = 0;

        public string ImageUrl
        {
            get => (string)GetValue(ImageUrlProperty);
            set => SetValue(ImageUrlProperty, value);
        }

        public int NumberOfAlbums 
        {
            get => (int)GetValue(AlbumsProperty);
            set => SetValue(AlbumsProperty, value);
        }
        #endregion

        #region BindableProperty
        private static readonly BindableProperty ArtistProperty =
            BindableProperty.Create("Artist", typeof(string), typeof(ArtistView), string.Empty,
            BindingMode.TwoWay, propertyChanged: ArtistPropertyChanged);

        private static readonly BindableProperty AlbumsProperty =
            BindableProperty.Create("NumberOfAlbums", typeof(int), typeof(ArtistView), 0,
            BindingMode.TwoWay, propertyChanged: AlbumsPropertyChanged);

        private static readonly BindableProperty ImageUrlProperty =
            BindableProperty.Create("ImageUrl", typeof(string), typeof(ArtistView), string.Empty,
            BindingMode.TwoWay, propertyChanged: ImageUrlPropertyChanged);
        #endregion

        #region Property Changed
        private static void ImageUrlPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ArtistView)bindable;
            if (control != null)
                control.ImageArtist.Source = (string)newValue;
        }
        private static void AlbumsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ArtistView)bindable;
            if (control != null)
                control.LblNumberOfAlbums.Text = $"{newValue}";
        }
        private static void ArtistPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ArtistView)bindable;
            if (control != null)
                control.LblArtist.Text = (string)newValue;
        }
        #endregion

        #region Controls
        Image ImageAlbum;
        ImageButton BtnDownload;
        Image ImageArtist;
        Image ImageSong;

        Label LblNumberOfAlbums;
        Label LblArtist;

        Grid ArtistGrid;
        StackLayout AlbumStack;
        StackLayout SongStack;

        AlbumsAndSongs AlbumsAndSongsPage;
        #endregion

        public event EventHandler OnTapped;

        public ArtistView()
        {
            BackgroundColor = (Color)App.Current.Resources["Icon"];
            Padding = new Thickness(0, 0, 0, 5);
            CornerRadius = 5;

            LblArtist = new Label
            {
                Text = Artist,
                Style = (Style)App.Current.Resources["BoldLabel"],
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.WordWrap
            };

            LblNumberOfAlbums = new Label
            {
                Text = $"{NumberOfAlbums}",
                Style = (Style)App.Current.Resources["LightLabel"],
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Center
            };

            ImageArtist = new Image
            {
                Source = ImageUrl,
                HorizontalOptions = LayoutOptions.Start,
                WidthRequest = 87,
                HeightRequest = 87
            };

            ImageAlbum = new Image
            {
                Source = "Album.png",
                HeightRequest = 30,
                WidthRequest = 30
            };

            ImageSong = new Image
            {
                Source = "Song.png",
                HeightRequest = 30,
                WidthRequest = 30
            };

            BtnDownload = new ImageButton
            {
                Source = "Download.png",
                HeightRequest = 30,
                WidthRequest = 30,
                BackgroundColor = Color.Transparent
            };

            AlbumStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            SongStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            var AlbumGesture = new TapGestureRecognizer();
            AlbumGesture.Tapped += AlbumGesture_Tapped; 
            AlbumStack.GestureRecognizers.Add(AlbumGesture);

            var SongGesture = new TapGestureRecognizer();
            SongGesture.Tapped += SongGesture_Tapped;
            SongStack.GestureRecognizers.Add(SongGesture);

            ArtistGrid = new Grid
            {
                BackgroundColor = (Color)App.Current.Resources["Background"]
            };            
            SetGridView();
            Content = ArtistGrid;
        }

        private async void SongGesture_Tapped(object sender, EventArgs e)
        {
            AlbumsAndSongsPage = new AlbumsAndSongs(ArtistId, false);
            await Navigation.PushPopupAsync(AlbumsAndSongsPage, true);
            OnTapped?.Invoke(sender, e);
        }

        private async void AlbumGesture_Tapped(object sender, EventArgs e)
        {
            AlbumsAndSongsPage = new AlbumsAndSongs(ArtistId);
            await Navigation.PushPopupAsync(AlbumsAndSongsPage, true);
            OnTapped?.Invoke(sender, e);
        }

        private void SetGridView()
        {
            AlbumStack.Children.Add(ImageAlbum);
            SongStack.Children.Add(ImageSong);

            ArtistGrid.Children.Add(ImageArtist, 0, 0);
            ArtistGrid.Children.Add(LblArtist, 1, 0);
            ArtistGrid.Children.Add(AlbumStack, 3, 1);
            ArtistGrid.Children.Add(SongStack, 4, 1);
            ArtistGrid.Children.Add(BtnDownload, 5, 1);

            Grid.SetRowSpan(ImageArtist, 3);
            Grid.SetRowSpan(LblArtist, 3);
            Grid.SetColumnSpan(LblArtist, 2);
        }
    }
}
