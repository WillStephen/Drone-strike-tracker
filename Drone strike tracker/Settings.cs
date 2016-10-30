using Android.Content.Res;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Drone_strike_tracker
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        #region Setting keys

        private const string MapTypeKey = "map_type";

        private const string SettingsKey = "settings_key";
        private const string DownLoadOnlyOnWifiKey = "download_only_on_wifi_key";

        #endregion

        #region Setting constants

        private const string MapTypeDefault = "Hybrid";

        private static readonly string SettingsDefault = string.Empty;
        private const bool DownloadOnlyOnWifiDefault = false;

        public const string MinApiUrl = "http://api.dronestre.am/min";
        public const string FullApiUrl = "http://api.dronestre.am/data";
        public const string TwitterUrlStart = "https://twitter.com/dronestream/status/";
        public const string GoogleSearchUrlTemplate = "http://www.google.com/search?q={0}&tbm=isch";

        #endregion

        public static string MapType
        {
            get { return AppSettings.GetValueOrDefault(MapTypeKey, MapTypeDefault); }
            set { AppSettings.AddOrUpdateValue(MapTypeKey, value); }
        }

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