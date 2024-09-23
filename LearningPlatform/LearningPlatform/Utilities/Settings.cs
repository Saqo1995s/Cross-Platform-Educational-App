using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace LearningPlatform.Utilities
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static bool IsUserLoggedIn
        {
            get => AppSettings.GetValueOrDefault(nameof(IsUserLoggedIn), false);
            set => AppSettings.AddOrUpdateValue(nameof(IsUserLoggedIn), value);
        }

        public static string Group    
        {
            get => AppSettings.GetValueOrDefault(nameof(Group), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Group), value);
        }

        public static int UserId
        {
            get => AppSettings.GetValueOrDefault(nameof(UserId), -1);
            set => AppSettings.AddOrUpdateValue(nameof(UserId), value);
        }

        #region Authentication

        public static string BearerToken
        {
            get => AppSettings.GetValueOrDefault(nameof(BearerToken), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(BearerToken), value);
        }

        public static string RefreshToken
        {
            get => AppSettings.GetValueOrDefault(nameof(RefreshToken), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(RefreshToken), value);
        }

        #endregion
    }
}
