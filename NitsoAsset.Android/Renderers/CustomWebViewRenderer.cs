using System;
using Android.App;
using Android.Content;
using Android.Provider;
using Android.Webkit;
using Java.IO;
using NitsoAsset.Droid.Renderers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using WebView = Xamarin.Forms.WebView;

[assembly: ExportRenderer(typeof(WebView), typeof(CustomWebViewRenderer))]
namespace NitsoAsset.Droid.Renderers
{
    public class CustomWebViewRenderer : WebViewRenderer
    {
        Context _context;

        public CustomWebViewRenderer(Context context) : base(context)
        {
            _context = context;
        }

        [Obsolete]
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var customWebView = Element as WebView;

                if (Control != null)
                {
                    Control.Settings.JavaScriptEnabled = true;
                    Control.Settings.DomStorageEnabled = true;
                    Control.Settings.DatabaseEnabled = true;
                    Control.Settings.AllowFileAccess = true;
                    Control.Settings.AllowContentAccess = true;

                    Control.Settings.AllowFileAccessFromFileURLs = true;
                    Control.Settings.AllowUniversalAccessFromFileURLs = true;
                    Control.Settings.MediaPlaybackRequiresUserGesture = false;
                    Control.Settings.SetPluginState(WebSettings.PluginState.On);

                    // Set a single WebChromeClient that handles both camera and file selection
                    Control.SetWebChromeClient(new CombinedWebChromeClient((Activity)Context));

#if DEBUG
                    Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
#endif
                }
            }
        }

        // Combined WebChromeClient that handles both permissions and file selection
        public class CombinedWebChromeClient : WebChromeClient
        {
            private Activity _activity;
            private IValueCallback _filePathCallback;
            private const int FILECHOOSER_RESULTCODE = 1;

            public CombinedWebChromeClient(Activity activity)
            {
                _activity = activity;
            }

            // Handle permission requests (camera, etc.)
            public override void OnPermissionRequest(PermissionRequest request)
            {
                request.Grant(request.GetResources());
            }

            // Handle file selection
            public override bool OnShowFileChooser(Android.Webkit.WebView webView, IValueCallback filePathCallback, FileChooserParams fileChooserParams)
            {
                // Store the callback reference
                _filePathCallback = filePathCallback;

                try
                {
                    // Create a file picker intent
                    Intent intent = new Intent(Intent.ActionGetContent);
                    intent.AddCategory(Intent.CategoryOpenable);
                    intent.SetType("*/*");

                    // Launch the file picker
                    _activity.StartActivityForResult(Intent.CreateChooser(intent, "Select File"), FILECHOOSER_RESULTCODE);

                    // You'll need to set up a callback handler in your MainActivity
                    // This part should register with your main activity to receive the result
                    if (_activity is MainActivity mainActivity)
                    {
                        mainActivity.RegisterFileSelectorCallback(this);
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("File chooser error: " + ex.Message);
                    return false;
                }
            }

            // Method to be called from MainActivity when file selection completes
            public void OnActivityResult(int requestCode, Result resultCode, Intent data)
            {
                Android.Net.Uri[] results = null;

                if (requestCode == FILECHOOSER_RESULTCODE)
                {
                    if (resultCode == Result.Ok && data != null && data.Data != null)
                    {
                        results = new Android.Net.Uri[] { data.Data };
                    }

                    // Always call the callback, even with null results
                    _filePathCallback?.OnReceiveValue(results);
                    _filePathCallback = null;
                }
            }
        }
    }
}