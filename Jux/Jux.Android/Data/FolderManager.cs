using Android.App;
using Android.Widget;
using Jux.Droid.Data;
using Jux.Interface;
using System;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(FolderManager))]
namespace Jux.Droid.Data
{
    public class FolderManager : IFolderManager
    {
        public string Music(string Artist, string Album)
        {
            var MainPath = MainActivity.GetActivity().JuxMusic;
            var MusicPath = Path.Combine(MainPath, Artist, Album);
            try
            {
                if (!Directory.Exists(MusicPath))
                {
                    Directory.CreateDirectory(MusicPath);
                }
            }
            catch (Exception e)
            {
                Toast.MakeText(Application.Context, e.Message, ToastLength.Long).Show();
            }
            return MusicPath;
        }

        public string Settings()
        {
            var SettingsPath = Path.Combine(MainActivity.GetActivity().JuxSettings);
            try
            {
                if (!Directory.Exists(SettingsPath))
                {
                    Directory.CreateDirectory(SettingsPath);
                }
            }
            catch (Exception e)
            {
                Toast.MakeText(Application.Context, e.Message, ToastLength.Long).Show();
            }
            return SettingsPath;
        }
    }
}