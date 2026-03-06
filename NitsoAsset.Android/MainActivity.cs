using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using FFImageLoading.Forms.Platform;
using Plugin.Permissions;
using Android.Content;
 using NitsoAsset.Droid.Helpers;
using Acr.UserDialogs;
using DCVXamarin.Droid;
using Android;
using static NitsoAsset.Droid.Renderers.CustomWebViewRenderer;

namespace NitsoAsset.Droid
{
    [Activity(Label = "NitsoAsset", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public event Action<int, global::Android.App.Result, Intent> LocationActivityResult;

        // Reference to the WebChromeClient to pass back the result
        private CombinedWebChromeClient _fileSelectorCallback;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this);
            CachedImageRenderer.Init(enableFastRenderer: true);

            UserDialogs.Init(() => this);

            FixXamarin.FixDevice(); // before init if forms using Incorrect values from Device.Idiom
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            FixXamarin.FixDevice(); // and after init for extra safety if Forms reseted value Device.Idiom

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
           
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            CachedImageRenderer.Init(enableFastRenderer: true);
            CachedImageRenderer.InitImageViewHandler();

            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            //LoadApplication(new App(new DCVCameraEnhancer(context: this), new DCVBarcodeReader()));

            CheckAndRequestPermissions();  // ✅ Request camera permissions
            LoadApplication(new App());

            if (Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Window?.SetStatusBarColor(Android.Graphics.Color.Argb(255, 255, 255, 255));
            }
        }

        private void CheckAndRequestPermissions()
        {
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.Camera) != Permission.Granted)
                {
                    RequestPermissions(new string[] { Manifest.Permission.Camera }, 1);
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, global::Android.Content.Intent data)
        {
            // Call the WebChromeClient's handler if it exists
            if (_fileSelectorCallback != null)
            {
                _fileSelectorCallback.OnActivityResult(requestCode, resultCode, data);
                return;
            }

            base.OnActivityResult(requestCode, resultCode, data);

            if (this.LocationActivityResult != null)
            {
                this.LocationActivityResult(requestCode, resultCode, data);
            }
        }

        // Method for the WebChromeClient to register itself for callbacks
        public void RegisterFileSelectorCallback(CombinedWebChromeClient callback)
        {
            _fileSelectorCallback = callback;
        }
    }
}