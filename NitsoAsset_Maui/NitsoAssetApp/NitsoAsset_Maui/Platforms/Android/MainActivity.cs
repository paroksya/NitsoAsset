using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Webkit;
using NitsoAsset_Maui.Platforms.Android.Renderers;

namespace NitsoAsset_Maui.Platforms.Android
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public static MainActivity? Instance { get; private set; }
        // private CustomWebViewHandler? WebViewHandler;
        public static Action<int, Result, Intent?>? OnActivityResultCallback;

        protected override void OnCreate(Bundle bundle)
        {
             Instance = this; 
            base.OnCreate(bundle);
            // Call the method on the handler
            // MauiProgram.GlobalWebViewHandler?.TriggerFileChooser();
            global::Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);

            Microsoft.Maui.ApplicationModel.Platform.Init(this, bundle);
            Microsoft.Maui.ApplicationModel.Platform.ActivityStateChanged += Platform_ActivityStateChanged;
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            System.Diagnostics.Debug.WriteLine($"🟢 MainActivity OnActivityResult: {requestCode}");

            // Forward to CustomWebChromeClient
            OnActivityResultCallback?.Invoke(requestCode, resultCode, data);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            System.Diagnostics.Debug.WriteLine($"🟢 OnRequestPermissionsResult: {requestCode}");
            for (int i = 0; i < permissions.Length; i++)
            {
                System.Diagnostics.Debug.WriteLine($"   {permissions[i]}: {grantResults[i]}");
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            //Microsoft.Maui.ApplicationModel.Platform.OnResume(this);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            //Microsoft.Maui.ApplicationModel.Platform.OnNewIntent(intent);
        }

        protected override void OnDestroy()
        {
            Instance = null;
            base.OnDestroy();

            //Microsoft.Maui.ApplicationModel.Platform.ActivityStateChanged -= Platform_ActivityStateChanged;
        }

        void Platform_ActivityStateChanged(object sender, Microsoft.Maui.ApplicationModel.ActivityStateChangedEventArgs e)
        {
            //Toast.MakeText(this, e.State.ToString(), ToastLength.Short).Show();
        }

        // public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        // {
        //     base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //     //CustomWebViewRenderer.CustomWebChromeClient.HandlePermissionResult(requestCode, permissions, grantResults);
        //     //Microsoft.Maui.ApplicationModel.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //     //   Microsoft.Maui.Authentication.WebAuthenticatorCallbackActivity.SetResult(requestCode, grantResults, permissions);
        //     //  Microsoft.Maui.Authentication.WebAuthenticatorCallbackActivity.SetResult();
        // }

    }
}