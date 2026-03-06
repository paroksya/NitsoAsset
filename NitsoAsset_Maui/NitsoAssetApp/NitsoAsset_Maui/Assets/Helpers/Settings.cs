using System;
using Newtonsoft.Json;
using NitsoAsset_Maui.Models;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace NitsoAsset_Maui.Assets.Helpers
{
    public static class Settings
    {
        #region Setting

        private const string CompanyCodeKey = "CompanyCode_key";
        private static readonly string CompanyCodeDefault = string.Empty;
        public static string CompanyCode
        {
            get => Preferences.Default.Get(CompanyCodeKey, CompanyCodeDefault);
            set => Preferences.Default.Set(CompanyCodeKey, value);
        }

        private const string CompanyNameKey = "CompanyName_key";
        private static readonly string CompanyNameKeyDefault = string.Empty;
        public static string CompanyName
        {
            get => Preferences.Default.Get(CompanyNameKey, CompanyNameKeyDefault);
            set => Preferences.Default.Set(CompanyNameKey, value);
        }

        private const string UserNameKey = "UserName_key";
        private static readonly string UserNameDefault = string.Empty;
        public static string UserName
        {
            get => Preferences.Default.Get(UserNameKey, UserNameDefault);
            set => Preferences.Default.Set(UserNameKey, value);
        }

        private const string PasswordKey = "Password_key";
        private static readonly string PasswordDefault = string.Empty;
        public static string Password
        {
            get => Preferences.Default.Get(PasswordKey, PasswordDefault);
            set => Preferences.Default.Set(PasswordKey, value);
        }

        private const string UserLoginWithLocationKey = "UserLoginWithLocation_key";
        private static readonly string UserLoginWithLocationDefault = string.Empty;
        public static string UserLoginWithLocation
        {
            get => Preferences.Default.Get(UserLoginWithLocationKey, UserLoginWithLocationDefault);
            set => Preferences.Default.Set(UserLoginWithLocationKey, value);
        }

        private const string UserDetailsKey = "UserDetails_key";
        public static UserModel UserDetails
        {
            get
            {
                var value = Preferences.Default.Get(UserDetailsKey, string.Empty);
                return string.IsNullOrEmpty(value) ? new UserModel() : JsonConvert.DeserializeObject<UserModel>(value);
            }
            set
            {
                string data = value != null ? JsonConvert.SerializeObject(value) : string.Empty;
                Preferences.Default.Set(UserDetailsKey, data);
            }
        }

        private const string SavedDetailsKey = "SavedDetails_key";
        private static readonly string SavedDetailsDefault = string.Empty;
        public static string SavedDetails
        {
            get => Preferences.Default.Get(SavedDetailsKey, SavedDetailsDefault);
            set => Preferences.Default.Set(SavedDetailsKey, value);
        }

        private const string ServiceTokenKey = "ServiceToken_key";
        private static readonly string ServiceTokenDefault = string.Empty;
        public static string ServiceToken
        {
            get => Preferences.Default.Get(ServiceTokenKey, ServiceTokenDefault);
            set => Preferences.Default.Set(ServiceTokenKey, value);
        }

        private const string ServiceBaseURLKey = "ServiceBaseURL_key";
        private static readonly string ServiceBaseURLDefault = string.Empty;
        public static string ServiceBaseURL
        {
            get => Preferences.Default.Get(ServiceBaseURLKey, ServiceBaseURLDefault);
            set => Preferences.Default.Set(ServiceBaseURLKey, value);
        }

        #endregion
    }
}