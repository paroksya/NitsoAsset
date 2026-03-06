using System;
using Newtonsoft.Json;
using NitsoAsset.Models;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Forms;

namespace NitsoAsset.Assets.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        private const string UserLoginWithLocationKey = "UserLoginWithLocation_key";
        private static readonly string UserLoginWithLocationDefault = string.Empty;

        #endregion

        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }

        private const string ServiceTokenKey = "ServiceToken_key";
        private static readonly string ServiceTokenDefault = string.Empty;

        public static string ServiceToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(ServiceTokenKey, ServiceTokenDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ServiceTokenKey, value);
            }
        }

        private const string ServiceBaseURLKey = "ServiceBaseURL_key";
        private static readonly string ServiceBaseURLDefault = string.Empty;

        public static string ServiceBaseURL
        {
            get
            {
                return AppSettings.GetValueOrDefault(ServiceBaseURLKey, ServiceBaseURLDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ServiceBaseURLKey, value);
            }
        }

        public static string UserLoginWithLocation
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserLoginWithLocationKey, UserLoginWithLocationDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserLoginWithLocationKey, value);
            }
        }

        private const string CompanyCodeKey = "CompanyCode_key";
        private static readonly string CompanyCodeDefault = string.Empty;

        public static string CompanyCode
        {
            get
            {
                return AppSettings.GetValueOrDefault(CompanyCodeKey, CompanyCodeDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(CompanyCodeKey, value);
            }
        }

        private const string UserNameKey = "UserName_key";
        private static readonly string UserNameDefault = string.Empty;

        public static string UserName
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserNameKey, UserNameDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserNameKey, value);
            }
        }

        private const string PasswordKey = "Password_key";
        private static readonly string PasswordDefault = string.Empty;

        public static string Password
        {
            get
            {
                return AppSettings.GetValueOrDefault(PasswordKey, PasswordDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(PasswordKey, value);
            }
        }

        private const string CompanyNameKey = "CompanyName_key";
        private static readonly string CompanyNameKeyDefault = string.Empty;

        public static string CompanyName
        {
            get
            {
                return AppSettings.GetValueOrDefault(CompanyNameKey, CompanyNameKeyDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(CompanyNameKey, value);
            }
        }

        private const string SavedDetailsKey = "SavedDetails_key";
        private static readonly string SavedDetailsDefault = string.Empty;

        public static string SavedDetails
        {
            get
            {
                return AppSettings.GetValueOrDefault(SavedDetailsKey, SavedDetailsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SavedDetailsKey, value);
            }
        }

        private const string UserDetailsKey = "UserDetails_key";
        public static UserModel UserDetails
        {
            get
            {
                var value = AppSettings.GetValueOrDefault(UserDetailsKey, string.Empty);
                if (string.IsNullOrEmpty(value)) { return new UserModel(); }
                else { return JsonConvert.DeserializeObject<UserModel>(value); }
            }
            set
            {
                string data = string.Empty;
                if (value != null) { data = JsonConvert.SerializeObject(value); }
                AppSettings.AddOrUpdateValue(UserDetailsKey, data);
            }
        }
    }
}