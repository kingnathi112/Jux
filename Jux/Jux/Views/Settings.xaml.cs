using Jux.Helpers;
using Jux.Interface;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Pages;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Jux.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : PopupPage
    {
        string settingsPath = Path.Combine(DependencyService.Get<IFolderManager>().Settings(), "Settings.json");

        public Settings()
        {
            InitializeComponent();
            try
            {
                if (File.Exists(settingsPath))
                {
                    LoadUserSettings();
                }
                else
                {
                    LoadDefaultSettings();
                }
            }
            catch (Exception)
            {
                DependencyService.Get<IMessageCenter>().LongMessage($"An error occurred trying to load User Settings, \nDefault settings will be loaded.");
                LoadDefaultSettings();
            }
            TempSettings.Quality = SwitchQuality.IsToggled;
            TempSettings.SaveImage = SwitchAlbum.IsToggled;
        }

        private void LoadDefaultSettings()
        {
            SwitchAlbum.IsToggled = DependencyService.Get<ISettingsManager>().DefaultAlbumCover;
            SwitchQuality.IsToggled = DependencyService.Get<ISettingsManager>().DefaultQuality;
            LblAlbum.Text = DependencyService.Get<ISettingsManager>().DefaultAlbumTitle;
            LblTrack.Text = DependencyService.Get<ISettingsManager>().DefaultTrack;
        }

        private void SaveUserSettings()
        {
            DependencyService.Get<ISettingsManager>().UserAlbumCover = SwitchAlbum.IsToggled;
            DependencyService.Get<ISettingsManager>().UserAlbumTitle = LblAlbum.Text;
            DependencyService.Get<ISettingsManager>().UserLocation = TxtMusicFolder.Text;
            DependencyService.Get<ISettingsManager>().UserLyrics = false;
            DependencyService.Get<ISettingsManager>().UserQuality = SwitchQuality.IsToggled;
            DependencyService.Get<ISettingsManager>().UserTrack = LblTrack.Text;

            try
            {
                DependencyService.Get<ISettingsManager>().SaveSettings();
                DependencyService.Get<IMessageCenter>().ShortMessage("Settings Saved!");
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessageCenter>().LongMessage(ex.Message);
            }
        }

        private void LoadUserSettings()
        {
            try
            {
                SwitchAlbum.IsToggled = DependencyService.Get<ISettingsManager>().UserAlbumCover;
                LblAlbum.Text = DependencyService.Get<ISettingsManager>().UserAlbumTitle;
                TxtMusicFolder.Text = DependencyService.Get<ISettingsManager>().UserLocation;
                //SwitchLyrics.IsToggled =  DependencyService.Get<ISettingsManager>().UserLyrics;
                SwitchQuality.IsToggled = DependencyService.Get<ISettingsManager>().UserQuality;
                LblTrack.Text = DependencyService.Get<ISettingsManager>().UserTrack;
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessageCenter>().LongMessage(ex.Message);
            }
        }

        #region Popup Settings
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return false;
        }
        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }
        #endregion
        private async void BtnSaveSettings_Clicked(object sender, System.EventArgs e)
        {
            PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
            try
            {
                if (status != PermissionStatus.Granted)
                {
                    await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage);
                    status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                    DependencyService.Get<IMessageCenter>().ShortMessage($"Storage Request {status.ToString()}");
                    DependencyService.Get<IFolderManager>().Settings();
                    SaveUserSettings();
                }
                else
                {
                    DependencyService.Get<IFolderManager>().Settings();
                    SaveUserSettings();
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessageCenter>().LongMessage(ex.Message);
            }
        }

        private void BtnDefaultSettings_Clicked(object sender, EventArgs e)
        {
            try
            {
                LoadDefaultSettings();
                DependencyService.Get<IMessageCenter>().ShortMessage("Settings Restored!");
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessageCenter>().LongMessage(ex.Message);
            }
        }

        private void SwitchQuality_Toggled(object sender, ToggledEventArgs e)
        {
            TempSettings.Quality = SwitchQuality.IsToggled;
        }

        private void SwitchAlbum_Toggled(object sender, ToggledEventArgs e)
        {
            TempSettings.SaveImage = SwitchAlbum.IsToggled;
        }
    }
}