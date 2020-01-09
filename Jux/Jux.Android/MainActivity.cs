
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using MediaManager;
using Plugin.Permissions;
using Rg.Plugins.Popup.Services;
using System.IO;

namespace Jux.Droid
{
    [Activity(Label = "Jux", Icon = "@drawable/logo", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private static MainActivity instance;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            instance = this;
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            CrossMediaManager.Current.Init(this);

            LoadApplication(new App());
        }
        protected override void OnDestroy()
        {
            instance = null;
        }
        public static MainActivity GetActivity()
        {
            return instance;
        }
        public async override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                await PopupNavigation.PopAsync(true);
            }
        }
        #region Folders
        public string JuxFolder
        {
            get
            {
                return Path.Combine(RootFolder, "Jux");
            }
        }        

        /// <summary>
        /// Jux Settings folder, /sdcard/Jux/Settings
        /// </summary>
        public string JuxSettings
        {
            get
            {
                return Path.Combine(JuxFolder, "Settings");
            }
        }

        /// <summary>
        /// Where music would be downloaded, /sdcard/Music/Jux Music/
        /// </summary>
        public string JuxMusic
        {
            get
            {
                return Path.Combine(RootFolder, "Music", "Jux Music");
            }
        }

        private string RootFolder
        {
            get
            {
                string tempPath = ApplicationContext.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
                string path = tempPath.Substring(0, tempPath.IndexOf("0/") + 1);
                return path;
            }
        }

        #endregion
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}