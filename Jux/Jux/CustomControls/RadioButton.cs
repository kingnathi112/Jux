using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Jux.CustomControls
{
   public class RadioButton : StackLayout
    {
        #region Events
        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        private static readonly BindableProperty IsSelectedProperty =
            BindableProperty.Create("IsSelected", typeof(bool), typeof(RadioButton), false,
                BindingMode.TwoWay, propertyChanged: IsSelectedPropertyChanged);
        private static void IsSelectedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RadioButton)bindable;
            if (control != null)
            {
                control.IsSelected = (bool)newValue;
                control.Update();
            }
        }

        public event EventHandler OnTapped;
        #endregion

        #region Radio Button Images
        Image ImgRoundRadio;
        Image ImgCategory;

        public string CheckedImage
        {
            get => (string)GetValue(CheckedImageProperty);
            set => SetValue(CheckedImageProperty, value);
        }
        public string UnCheckedImage
        {
            get => (string)GetValue(UnCheckedImageProperty);
            set => SetValue(UnCheckedImageProperty, value);
        }
        public string CategoryImage
        {
            get => (string)GetValue(CategoryImageProperty);
            set => SetValue(CategoryImageProperty, value);
        }
       
        private static readonly BindableProperty CategoryImageProperty =
            BindableProperty.Create("CategoryImage", typeof(string), typeof(RadioButton), string.Empty,
                BindingMode.OneWay, propertyChanged: CategoryImagePropertyChanged);


        private static readonly BindableProperty CheckedImageProperty =
            BindableProperty.Create("CheckedImage", typeof(string), typeof(RadioButton), string.Empty,
                BindingMode.TwoWay, propertyChanged: ImagePropertyChanged);
               
        private static readonly BindableProperty UnCheckedImageProperty =
            BindableProperty.Create("UnCheckedImage", typeof(string), typeof(RadioButton), string.Empty,
                BindingMode.TwoWay, propertyChanged: ImagePropertyChanged);
        private static void ImagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RadioButton)bindable;
            if (control != null)
                control.Update();
        }
        private static void CategoryImagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RadioButton)bindable;
            if (control != null)
            {
                control.ImgCategory.Source = (string)newValue;
            }
        }

        private void Update()
        {
            ImgRoundRadio.Source = IsSelected ? CheckedImage : (ImageSource)UnCheckedImage;
        }
        #endregion
        public RadioButton()
        {
            HeightRequest = 30;
            Orientation = StackOrientation.Horizontal;

            ImgRoundRadio = new Image
            {
                Source = CategoryImage,
                BackgroundColor = Color.Transparent,
                WidthRequest = this.HeightRequest,
                HeightRequest = this.HeightRequest
            };

            ImgCategory = new Image
            {
                Source = UnCheckedImage,
                BackgroundColor = Color.Transparent,
                WidthRequest = this.HeightRequest,
                HeightRequest = this.HeightRequest,
            };

            Children.Add(ImgRoundRadio);
            Children.Add(ImgCategory);

            var Tap = new TapGestureRecognizer();
            Tap.Tapped += Tap_Tapped;
            GestureRecognizers.Add(Tap);
        }

        private void Tap_Tapped(object sender, EventArgs e)
        {
            IsSelected = true;
            OnTapped?.Invoke(sender, e);
        }
    }
}
