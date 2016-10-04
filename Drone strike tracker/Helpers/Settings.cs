using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Drone_strike_tracker.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        #region Setting keys

        private const string SettingsKey = "settings_key";
        private const string DownLoadOnlyOnWifiKey = "download_only_on_wifi_key";

        #endregion

        #region Setting constants

        private static readonly string SettingsDefault = string.Empty;
        private const bool DownloadOnlyOnWifiDefault = false;

        public const string ApiUri = "http://api.dronestre.am/min";

        #endregion

        /// <summary>
        /// This setting is true if the user wants to only check the API when connected to Wi-Fi
        /// </summary>
        public static bool DownloadOnlyOnWifi
        {
            get { return AppSettings.GetValueOrDefault(DownLoadOnlyOnWifiKey, DownloadOnlyOnWifiDefault); }
            set { AppSettings.AddOrUpdateValue(DownLoadOnlyOnWifiKey, value); }
        }
    }
}