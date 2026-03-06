using Android.Content;
using Android.Webkit;
using Android.Widget;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Handlers;
using NitsoAsset_Maui.Platforms.Android.Renderers;
using WebView = Android.Webkit.WebView;

[assembly: ExportRenderer(typeof(WebView), typeof(CustomWebViewHandler))]
namespace NitsoAsset_Maui.Platforms.Android.Renderers;

public class CustomWebViewHandler : WebViewHandler
{
    private IValueCallback _filePathCallback;
    private WebView _nativeWebView;

    protected override void ConnectHandler(WebView platformView)
    {
        base.ConnectHandler(platformView);

        _nativeWebView = platformView; // Store reference to native WebView

        platformView.Settings.JavaScriptEnabled = true;
        platformView.Settings.AllowFileAccess = true;
        platformView.Settings.AllowContentAccess = true;
        platformView.SetWebChromeClient(new CustomWebChromeClient(this));
    }

    public void TriggerFileChooser()
    {
        if (_nativeWebView?.WebChromeClient is CustomWebChromeClient client)
        {
            client.OpenFileChooser();
        }
    }

    private class CustomWebChromeClient : WebChromeClient
    {
        private readonly CustomWebViewHandler _handler;

        public CustomWebChromeClient(CustomWebViewHandler handler)
        {
            _handler = handler;
        }

        public void OpenFileChooser()
        {
            Intent intent = new Intent(Intent.ActionGetContent);
            intent.SetType("*/*");
            intent.AddCategory(Intent.CategoryOpenable);

            try
            {
                var activity = Platform.CurrentActivity ?? throw new InvalidOperationException("Activity is null");
                activity.StartActivityForResult(intent, 1001);
            }
            catch (ActivityNotFoundException)
            {
                _handler._filePathCallback?.OnReceiveValue(null);
                Toast.MakeText(Platform.CurrentActivity, "Cannot open file chooser", ToastLength.Short).Show();
            }
        }

        public override bool OnShowFileChooser(WebView webView, IValueCallback filePathCallback, FileChooserParams fileChooserParams)
        {
            _handler._filePathCallback = filePathCallback;

            Intent intent = fileChooserParams.CreateIntent();
            try
            {
                var activity = Platform.CurrentActivity ?? throw new InvalidOperationException("Activity is null");
                activity.StartActivityForResult(intent, 1001);
            }
            catch (ActivityNotFoundException)
            {
                filePathCallback.OnReceiveValue(null);
                Toast.MakeText(Platform.CurrentActivity, "Cannot open file chooser", ToastLength.Short).Show();
                return false;
            }

            return true;
        }
    }
}