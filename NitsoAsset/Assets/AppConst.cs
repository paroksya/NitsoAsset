using System;
using NitsoAsset.Assets.Helpers;
using Xamarin.Forms;

namespace NitsoAsset.Assets
{
    public class AppConst
    {
        #region URLs

        //public const string ServerBaseURL = "http://api-asset.assetspecialist.in"; //"http://103.205.65.52:8081"; //"http://103.205.65.52/";

        //For UniqueServer
        //"https://apifams.muwci.net";

        public static string ServerBaseURL
        {
            get
            {
                return Settings.ServiceBaseURL;
                //return string.IsNullOrEmpty(Settings.ServiceBaseURL) ? Settings.ServiceBaseURL : Settings.ServiceBaseURL;
            }
        }

        //public const string Enviroment = "MobileAppTest"; //"MobileAppNew1"; //MobileApp;

        //public static string ImageBaseUrl = ServerBaseURL + "/" + Enviroment;

        //public static string UserLoginUrl = $"{ServerBaseURL}/api/Account/Login/UserLogin";

        //public static string SearchAssetByCodeUrl = $"{ServerBaseURL}/api/Client/Asset/AssetByCode";

        //public static string AssetVerificationUrl = $"{ServerBaseURL}/api/Client/Asset/AssetVerification";

        public static string UserLoginUrl = $"/api/Account/Login/UserLogin";

        public static string SearchAssetByCodeUrl = $"/api/Client/Asset/AssetByCode";

        public static string AssetVerificationUrl = $"/api/Client/Asset/AssetVerification";

        #endregion
    }
}