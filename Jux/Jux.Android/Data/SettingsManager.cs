using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Jux.Droid.Data;
using Jux.Droid.Models;
using Jux.Interface;
using Newtonsoft.Json;

[assembly: Xamarin.Forms.Dependency(typeof(SettingsManager))]
namespace Jux.Droid.Data
{
    public class SettingsManager : ISettingsManager
    {
        private string SettingsPath = Path.Combine(MainActivity.GetActivity().JuxSettings, "Settings.json");
        private string DefaultSettingsJson = "{\"Location\": \"/sdcard/Music/Jux Music/\",\"Quality\": false,\"FolderNameFormat\": \"1999 - Album Title\",\"TrackNameFormat\": \"00. Song Title\",\"AlbumCover\": false,\"Lyrics\": false}";
        bool Quality;
        bool AlbumCover;
        bool Lyrics;
        string Location;
        string Album;
        string Track;

        private SettingsModel UserSettingsModel()
        {
            if (File.Exists(SettingsPath))
            {
                var readSettingFile = File.ReadAllText(SettingsPath);
                var settingsManager = JsonConvert.DeserializeObject<SettingsModel>(readSettingFile);
                return settingsManager;
            }
            else
            {
                return null;
            }
        }

        private SettingsModel DefaultSettingsModel()
        {
            var settingsManager = JsonConvert.DeserializeObject<SettingsModel>(DefaultSettingsJson);
            return settingsManager;
        }

        #region User Settings
        public bool UserQuality 
        { 
            get => UserSettingsModel().Quality;
            set => Quality = value;
        } 
        public bool UserLyrics
        {
            get => UserSettingsModel().Lyrics;
            set => Lyrics = value;
        }
        public bool UserAlbumCover
        {
            get => UserSettingsModel().AlbumCover;
            set => AlbumCover = value;
        }
        public string UserLocation
        {
            get => UserSettingsModel().Location;
            set => Location = value;
        }
        public string UserAlbumTitle
        {
            get => UserSettingsModel().FolderNameFormat;
            set => Album = value;
        }
        public string UserTrack
        {
            get => UserSettingsModel().TrackNameFormat;
            set => Track = value;
        }
        #endregion

        #region Default Settings
        public bool DefaultQuality => DefaultSettingsModel().Quality;
        public bool DefaultLyrics => DefaultSettingsModel().Lyrics;
        public bool DefaultAlbumCover => DefaultSettingsModel().AlbumCover;
        public string DefaultLocation => DefaultSettingsModel().Location;
        public string DefaultAlbumTitle => DefaultSettingsModel().FolderNameFormat;
        public string DefaultTrack => DefaultSettingsModel().TrackNameFormat;

       
        #endregion
        public void SaveSettings()
        {
            string UserSettings = "{\"Location\": \"" + Location + "\",\"Quality\": " + Quality.ToString().ToLower() + ",\"FolderNameFormat\": \"" + Album +"\",\"TrackNameFormat\": \"" + Track + "\",\"AlbumCover\": " + AlbumCover.ToString().ToLower() + ",\"Lyrics\": " + Lyrics.ToString().ToLower() + "}";
            File.WriteAllText(SettingsPath, UserSettings);
        }
    }
}