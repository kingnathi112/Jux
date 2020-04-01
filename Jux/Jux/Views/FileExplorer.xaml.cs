using Id3;
using Jux.Helpers;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms.Xaml;

namespace Jux.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileExplorer : PopupPage
    {
        WriteTags tags;
        public FileExplorer()
        {
            InitializeComponent();
        }

        private async void BtnFilePicker_Clicked(object sender, EventArgs e)
        {
            try
            {
                
                FileData fileData = await CrossFilePicker.Current.PickFile();
                if (fileData == null)
                    return; // user canceled file picking

                if (fileData.FileName.EndsWith("mp3"))
                {
                    string fileName = fileData.FileName;
                    LblFileName.Text = fileName;

                    string musicFile = fileData.FilePath;
                    var audio = TagLib.File.Create(musicFile);

                    LblName.Text = "Title: " + audio.Tag.Title;
                    LblArtName.Text = "Album: " + audio.Tag.Album;
                    LblArtistsName.Text = "Artist: " + audio.Tag.FirstAlbumArtist;
                    var artists = new string[]{ TxtArtist.Text };
                    tags = new WriteTags(artists, TxtTitle.Text, TxtAlbum.Text, 2020, 1, "", musicFile);

                }
                else
                {
                    LblFileName.Text = "Can only read mp3 tags";
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception choosing file: " + ex.ToString());
            }
        }

        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            tags.Save();
        }
    }
}