using System;

using Windows.Storage;

namespace Zhihu.Common.Helper
{
    public sealed class LocalSettingUtility
    {
        #region Singleton

        private static LocalSettingUtility _instance;

        private LocalSettingUtility()
        {
        }

        public static LocalSettingUtility Instance => _instance ?? (_instance = new LocalSettingUtility());

        #endregion

        public void Add(String key, String value)
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            // Create a simple setting
            localSettings.Values[key] = value;
        }

        public T Read<T>(String key) where T : class
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            if (localSettings.Values.ContainsKey(key) == false)
            {
                return default(T);
            }

            var value = localSettings.Values[key];

            return value as T;
        }

        public void Remove(String key)
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            localSettings.Values.Remove(key);
        }
    }
}
