using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Jux.Droid.Models
{
    public class SettingsModel
    {
        public string Location { get; set; }
        public string TrackNameFormat { get; set; }
        public string FolderNameFormat { get; set; }
        public bool Quality { get; set; }
        public bool AlbumCover { get; set; }
        public bool Lyrics { get; set; }
    }
}