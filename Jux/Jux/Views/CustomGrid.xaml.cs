using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Jux.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomGrid : PopupPage
    {
        private string _Artist = "";
        private string _Album = "";
        private string _Year = "";
        private string _Url = "";
        private int _Id = 0;
        public CustomGrid(string Artist, string Album, string Year, int AlbumId, string AlbumImageUrl)
        {
            InitializeComponent();
            _Artist = Artist;
            _Year = Year;
            _Album = Album;
            _Id = AlbumId;
            _Url = AlbumImageUrl;
        }

        public void DisplayAlbum()
        {
            //LblAlbum.Text = _Album;
            //LblArtist.Text = _Artist;
            //LblYear.Text = _Year;
            //ImgAlbum.Source = _Url;
        }
    }
}