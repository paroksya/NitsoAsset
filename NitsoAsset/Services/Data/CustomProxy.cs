using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NitsoAsset.Assets;
using NitsoAsset.Assets.Helpers;
using NitsoAsset.Models;
using Xamarin.Forms;

namespace NitsoAsset.Services.Data
{
    public class CustomProxy
    {
        public CustomProxy()
        {
        }

        public async Task<LoginResponse> UserLogin(LoginModel model)
        {
            try
            {
                var result = await ProxyBase<LoginResponse>.Post($"{Settings.ServiceBaseURL}{AppConst.UserLoginUrl}", model, false, false);

                if (result != null)
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }

            return null;
        }

        public async Task<AssetResponse> SearchAssetByCode(AssetRequestModel model)
        {
            try
            {
                var result = await ProxyBase<AssetResponse>.Post($"{Settings.ServiceBaseURL}{AppConst.SearchAssetByCodeUrl}", model, true, false);

                if (result != null)
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }

            return null;
        }

        public async Task<VerificationAssetResponse> VerificationAssetByCode(VerificationAssetRequestModel model)
        {
            try
            {
                var result = await ProxyBase<VerificationAssetResponse>.Post($"{Settings.ServiceBaseURL}{AppConst.AssetVerificationUrl}", model, true, false);

                if (result != null)
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }

            return null;
        }
    }
}