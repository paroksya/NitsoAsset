using System;
using System.Threading.Tasks;
using NitsoAsset.iOS.Helpers;
using NitsoAsset.Services.AppServices;
using UIKit;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(NativeDependencyServiceHelper))]
namespace NitsoAsset.iOS.Helpers
{
    public class NativeDependencyServiceHelper : INativeDependencyServices
    {
        public Task<bool> EnableGPSService()
        {
            return null;
        }

        public void HideKeyboard()
        {
            UIApplication.SharedApplication.KeyWindow.EndEditing(true);
        }
    }
}