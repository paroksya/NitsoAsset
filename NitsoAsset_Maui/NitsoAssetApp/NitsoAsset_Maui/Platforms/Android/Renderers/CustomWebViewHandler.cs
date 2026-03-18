using Android.App;
using Android.Content;
using Android.Webkit;
using Android.OS;
using Android.Widget;
using Microsoft.Maui.Handlers;
using WebView = Android.Webkit.WebView;
using AndroidUri = Android.Net.Uri;
using AndroidLayerType = Android.Views.LayerType;
using Android.Provider;
using AndroidX.Core.Content;

namespace NitsoAsset_Maui.Platforms.Android.Renderers;

public class CustomWebViewHandler : WebViewHandler
{
    private IValueCallback? _filePathCallback;
    private WebView? _nativeWebView;
    private AndroidUri? _cameraOutputUri;
    private const int FILE_CHOOSER_REQUEST_CODE = 1001;

    public CustomWebViewHandler() : base(WebViewHandler.Mapper)
    {
        System.Diagnostics.Debug.WriteLine("🔥 CustomWebViewHandler CONSTRUCTOR");
    }

    protected override void ConnectHandler(WebView platformView)
    {
        System.Diagnostics.Debug.WriteLine("🔥 ConnectHandler STARTED");

        base.ConnectHandler(platformView);
        _nativeWebView = platformView;

        ApplySettings(platformView);
        ApplyChromeClient(platformView);

        platformView.SetLayerType(AndroidLayerType.Hardware, null);

        MainActivity.OnActivityResultCallback = (requestCode, resultCode, data) =>
        {
            HandleActivityResult(requestCode, resultCode, data);
        };

        System.Diagnostics.Debug.WriteLine("✅ CustomWebViewHandler fully connected!");
    }

    // ✅ This fires every time MAUI updates a property including Source
    public override void UpdateValue(string property)
    {
        base.UpdateValue(property);

        System.Diagnostics.Debug.WriteLine($"🔵 UpdateValue: {property}");

        // Re-apply ChromeClient after MAUI resets it on Source change
        if (_nativeWebView != null)
        {
            ApplyChromeClient(_nativeWebView);
        }
    }

    private void ApplySettings(WebView platformView)
    {
        platformView.Settings.JavaScriptEnabled = true;
        platformView.Settings.AllowFileAccess = true;
        platformView.Settings.AllowContentAccess = true;
        platformView.Settings.DomStorageEnabled = true;
        platformView.Settings.SetSupportZoom(false);
        platformView.Settings.MediaPlaybackRequiresUserGesture = false;
        platformView.Settings.SetGeolocationEnabled(true);
        platformView.Settings.DatabaseEnabled = true;
        platformView.Settings.MixedContentMode = MixedContentHandling.AlwaysAllow;
        platformView.Settings.AllowUniversalAccessFromFileURLs = true;
        platformView.Settings.AllowFileAccessFromFileURLs = true;

        System.Diagnostics.Debug.WriteLine("✅ Settings applied");
    }

    private void ApplyChromeClient(WebView platformView)
    {
        platformView.SetWebChromeClient(new CustomWebChromeClient(this));
        System.Diagnostics.Debug.WriteLine("✅ ChromeClient applied");
    }

    protected override void DisconnectHandler(WebView platformView)
    {
        System.Diagnostics.Debug.WriteLine("🔴 DisconnectHandler");
        platformView.SetWebChromeClient(null);
        _nativeWebView = null;
        MainActivity.OnActivityResultCallback = null;
        base.DisconnectHandler(platformView);
    }


    // private void HandleActivityResult(int requestCode, Result resultCode, Intent? data)
    // {
    //     System.Diagnostics.Debug.WriteLine($"📦 HandleActivityResult: code={requestCode}, result={resultCode}");

    //     if (requestCode != FILE_CHOOSER_REQUEST_CODE) return;

    //     if (_filePathCallback == null)
    //     {
    //         System.Diagnostics.Debug.WriteLine("❌ _filePathCallback NULL");
    //         return;
    //     }

    //     AndroidUri[]? results = null;

    //     if (resultCode == Result.Ok)
    //     {
    //         if (data?.Data != null)
    //         {
    //             // ✅ Gallery file selected
    //             results = new[] { data.Data };
    //             System.Diagnostics.Debug.WriteLine($"📦 Gallery file: {data.Data}");
    //         }
    //         else if (data?.ClipData != null)
    //         {
    //             // ✅ Multiple files selected
    //             int count = data.ClipData.ItemCount;
    //             results = new AndroidUri[count];
    //             for (int i = 0; i < count; i++)
    //                 results[i] = data.ClipData.GetItemAt(i)!.Uri!;
    //             System.Diagnostics.Debug.WriteLine($"📦 Multiple files: {count}");
    //         }
    //         else if (_cameraOutputUri != null)
    //         {
    //             // ✅ Camera photo — data is null but file was saved to _cameraOutputUri
    //             results = new[] { _cameraOutputUri };
    //             System.Diagnostics.Debug.WriteLine($"📦 Camera file: {_cameraOutputUri}");
    //         }
    //     }
    //     else
    //     {
    //         System.Diagnostics.Debug.WriteLine($"⚠️ Cancelled or failed: {resultCode}");
    //     }

    //     System.Diagnostics.Debug.WriteLine($"📦 Delivering {results?.Length ?? 0} URI(s)");
    //     _filePathCallback.OnReceiveValue(results);
    //     _filePathCallback = null;
    //     _cameraOutputUri = null; // ✅ Clear after use
    // }

    private void HandleActivityResult(int requestCode, Result resultCode, Intent? data)
    {
        System.Diagnostics.Debug.WriteLine($"📦 HandleActivityResult: code={requestCode}, result={resultCode}");

        if (requestCode != FILE_CHOOSER_REQUEST_CODE) return;

        if (_filePathCallback == null)
        {
            System.Diagnostics.Debug.WriteLine("❌ _filePathCallback NULL");
            return;
        }

        AndroidUri[]? results = null;

        if (resultCode == Result.Ok)
        {
            if (data?.Data != null)
            {
                // ✅ Gallery — grant read permission
                var uri = data.Data;
                try
                {
                    Platform.CurrentActivity?.ContentResolver?.TakePersistableUriPermission(
                        uri,
                        ActivityFlags.GrantReadUriPermission);
                }
                catch { /* some URIs don't support persistable permissions */ }

                results = new[] { uri };
                System.Diagnostics.Debug.WriteLine($"📦 Gallery: {uri}");
            }
            else if (data?.ClipData != null)
            {
                int count = data.ClipData.ItemCount;
                results = new AndroidUri[count];
                for (int i = 0; i < count; i++)
                    results[i] = data.ClipData.GetItemAt(i)!.Uri!;
            }
            else if (_cameraOutputUri != null)
            {
                // ✅ Camera — grant read permission to WebView process
                try
                {
                    Platform.CurrentActivity?.GrantUriPermission(
                        "com.google.android.webview",
                        _cameraOutputUri,
                        ActivityFlags.GrantReadUriPermission);
                }
                catch { }

                results = new[] { _cameraOutputUri };
                System.Diagnostics.Debug.WriteLine($"📦 Camera URI: {_cameraOutputUri}");
            }
        }

        System.Diagnostics.Debug.WriteLine($"📦 Delivering {results?.Length ?? 0} URI(s)");
        _filePathCallback.OnReceiveValue(results);
        _filePathCallback = null;
        _cameraOutputUri = null;
    }

    public class CustomWebChromeClient : WebChromeClient
    {
        private readonly CustomWebViewHandler _handler;

        public CustomWebChromeClient(CustomWebViewHandler handler)
        {
            _handler = handler;
            System.Diagnostics.Debug.WriteLine("✅ CustomWebChromeClient CREATED");
        }

        // ✅ THIS is what grants camera access to getUserMedia
        public override void OnPermissionRequest(PermissionRequest? request)
        {
            System.Diagnostics.Debug.WriteLine("🎥 OnPermissionRequest CALLED ✅✅✅");

            if (request == null)
            {
                System.Diagnostics.Debug.WriteLine("❌ request NULL");
                return;
            }

            var resources = request.GetResources() ?? Array.Empty<string>();
            System.Diagnostics.Debug.WriteLine($"🎥 Resources: {string.Join(", ", resources)}");

            var activity = Platform.CurrentActivity;
            if (activity == null)
            {
                System.Diagnostics.Debug.WriteLine("❌ Activity NULL — denying");
                request.Deny();
                return;
            }

            activity.RunOnUiThread(async () =>
            {
                try
                {
                    // ✅ Request Android OS permissions first
                    var camResult = await Permissions.RequestAsync<Permissions.Camera>();
                    var micResult = await Permissions.RequestAsync<Permissions.Microphone>();

                    System.Diagnostics.Debug.WriteLine($"🎥 Camera={camResult}, Mic={micResult}");

                    var toGrant = new List<string>();

                    foreach (var res in resources)
                    {
                        System.Diagnostics.Debug.WriteLine($"🎥 Checking resource: {res}");

                        if (res == PermissionRequest.ResourceVideoCapture
                            && camResult == PermissionStatus.Granted)
                        {
                            toGrant.Add(res);
                            System.Diagnostics.Debug.WriteLine("✅ VideoCapture granted");
                        }
                        else if (res == PermissionRequest.ResourceAudioCapture
                            && micResult == PermissionStatus.Granted)
                        {
                            toGrant.Add(res);
                            System.Diagnostics.Debug.WriteLine("✅ AudioCapture granted");
                        }
                        else if (res == PermissionRequest.ResourceProtectedMediaId)
                        {
                            toGrant.Add(res);
                            System.Diagnostics.Debug.WriteLine("✅ ProtectedMediaId granted");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"⚠️ Skipped: {res}");
                        }
                    }

                    if (toGrant.Count > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"✅ Granting {toGrant.Count} resource(s)");
                        request.Grant(toGrant.ToArray());
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("❌ Nothing to grant — denying");
                        request.Deny();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Error: {ex.Message}");
                    request.Deny();
                }
            });
        }

        public override void OnGeolocationPermissionsShowPrompt(
            string? origin, GeolocationPermissions.ICallback? callback)
        {
            System.Diagnostics.Debug.WriteLine($"📍 Geolocation: {origin}");
            callback?.Invoke(origin, true, false);
        }

        // public override bool OnShowFileChooser(
        //     WebView? webView,
        //     IValueCallback filePathCallback,
        //     FileChooserParams? fileChooserParams)
        // {
        //     System.Diagnostics.Debug.WriteLine("📁 OnShowFileChooser CALLED");

        //     _handler._filePathCallback?.OnReceiveValue(null);
        //     _handler._filePathCallback = filePathCallback;

        //     var activity = Platform.CurrentActivity;
        //     if (activity == null)
        //     {
        //         filePathCallback.OnReceiveValue(null);
        //         _handler._filePathCallback = null;
        //         return false;
        //     }

        //     try
        //     {
        //         var captureIntent = new Intent(MediaStore.ActionImageCapture);
        //         var galleryIntent = new Intent(Intent.ActionGetContent);
        //         galleryIntent.SetType("*/*");
        //         galleryIntent.AddCategory(Intent.CategoryOpenable);

        //         if (fileChooserParams != null)
        //         {
        //             var acceptTypes = fileChooserParams.GetAcceptTypes();
        //             if (acceptTypes?.Length > 0)
        //             {
        //                 var mimeType = string.Join(",", acceptTypes);
        //                 if (!string.IsNullOrWhiteSpace(mimeType) && mimeType != "*")
        //                     galleryIntent.SetType(mimeType);
        //             }
        //         }

        //         var chooserIntent = Intent.CreateChooser(galleryIntent, "Select or Capture Image");
        //         chooserIntent!.PutExtra(Intent.ExtraInitialIntents, new Intent[] { captureIntent });

        //         activity.StartActivityForResult(chooserIntent, FILE_CHOOSER_REQUEST_CODE);
        //         System.Diagnostics.Debug.WriteLine("📁 File picker launched");
        //         return true;
        //     }
        //     catch (Exception ex)
        //     {
        //         System.Diagnostics.Debug.WriteLine($"❌ OnShowFileChooser error: {ex.Message}");
        //         filePathCallback.OnReceiveValue(null);
        //         _handler._filePathCallback = null;
        //         return false;
        //     }
        // }

        public override bool OnShowFileChooser(
    WebView? webView,
    IValueCallback filePathCallback,
    FileChooserParams? fileChooserParams)
        {
            System.Diagnostics.Debug.WriteLine("📁 OnShowFileChooser CALLED");

            _handler._filePathCallback?.OnReceiveValue(null);
            _handler._filePathCallback = filePathCallback;

            var activity = Platform.CurrentActivity;
            if (activity == null)
            {
                filePathCallback.OnReceiveValue(null);
                _handler._filePathCallback = null;
                return false;
            }

            try
            {
                // ✅ Create a real file URI for camera output using FileProvider
                var photoFile = CreateImageFile(activity);
                _handler._cameraOutputUri = photoFile != null
                    ? AndroidX.Core.Content.FileProvider.GetUriForFile(
                        activity,
                        activity.PackageName + ".fileprovider",
                        photoFile)
                    : null;

                System.Diagnostics.Debug.WriteLine($"📁 Camera output URI: {_handler._cameraOutputUri}");

                // ✅ Camera intent with proper output URI
                var captureIntent = new Intent(MediaStore.ActionImageCapture);
                if (_handler._cameraOutputUri != null)
                {
                    captureIntent.PutExtra(MediaStore.ExtraOutput, _handler._cameraOutputUri);
                    captureIntent.AddFlags(ActivityFlags.GrantWriteUriPermission);
                    captureIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
                }

                // ✅ Gallery intent
                var galleryIntent = new Intent(Intent.ActionGetContent);
                galleryIntent.SetType("image/*");
                galleryIntent.AddCategory(Intent.CategoryOpenable);

                var chooserIntent = Intent.CreateChooser(galleryIntent, "Select or Capture Image");
                chooserIntent!.PutExtra(Intent.ExtraInitialIntents, new Intent[] { captureIntent });

                activity.StartActivityForResult(chooserIntent, FILE_CHOOSER_REQUEST_CODE);
                System.Diagnostics.Debug.WriteLine("📁 File picker launched");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ OnShowFileChooser error: {ex.Message}");
                filePathCallback.OnReceiveValue(null);
                _handler._filePathCallback = null;
                return false;
            }
        }

        private static Java.IO.File? CreateImageFile(Context context)
        {
            try
            {
                var timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var imageFileName = $"IMG_{timeStamp}";

                // ✅ Use alias to avoid namespace conflict
                var storageDir = context.GetExternalFilesDir(
                    global::Android.OS.Environment.DirectoryPictures);

                var image = Java.IO.File.CreateTempFile(imageFileName, ".jpg", storageDir);
                System.Diagnostics.Debug.WriteLine($"📁 Image file created: {image.AbsolutePath}");
                return image;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ CreateImageFile error: {ex.Message}");
                return null;
            }
        }
    }
}