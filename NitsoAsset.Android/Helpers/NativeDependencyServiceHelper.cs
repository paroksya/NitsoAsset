using System;
using Android.App;
using Android.Content;
using Android.Views.InputMethods;
using Xamarin.Forms;
using NitsoAsset.Droid.Helpers;
using NitsoAsset.Services.AppServices;
using System.Threading.Tasks;
using Android.Locations;

[assembly: Xamarin.Forms.Dependency(typeof(NativeDependencyServiceHelper))]
namespace NitsoAsset.Droid.Helpers
{
    public class NativeDependencyServiceHelper : INativeDependencyServices
    {

        public void HideKeyboard()
        {
            var context = Android.App.Application.Context;
            var inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            if (inputMethodManager != null && context is Activity)
            {
                var activity = context as Activity;
                var token = activity.CurrentFocus?.WindowToken;
                inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);

                activity.Window.DecorView.ClearFocus();
            }
        }
        public Task<bool> EnableGPSService()
        {
            var context = (MainActivity)Android.App.Application.Context;

            var listener = new ActivityResultListener(context);
            const int RequestEnableGps = 2;

            LocationManager lm = (LocationManager)context.GetSystemService(Context.LocationService);

            Intent intent = new Intent(global::Android.Provider.Settings.ActionLocationSourceSettings);
            context.StartActivityForResult(intent, RequestEnableGps);
            return listener.Task;
        }
    }
}