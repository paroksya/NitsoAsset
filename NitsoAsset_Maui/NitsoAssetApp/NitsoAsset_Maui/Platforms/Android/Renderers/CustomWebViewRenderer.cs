using Android.Content;
using Android.Webkit;
using Microsoft.Maui.Handlers;
using AndroidX.Activity;
using Android.Content.PM;
using Android;
using AndroidX.Core.Content;
using AndroidX.Core.App;
using Java.IO;
using Android.Provider;
using AndroidX.Activity.Result;
using AndroidX.Activity.Result.Contract;
using Microsoft.Maui.Controls.Compatibility;
using WebView = Microsoft.Maui.Controls.WebView;
using NitsoAsset_Maui.Platforms.Android;
using NitsoAsset_Maui.Platforms.Android.Renderers;
using AndroidX.AppCompat.App;

[assembly: ExportRenderer(typeof(WebView), typeof(CustomWebViewRenderer))]
namespace NitsoAsset_Maui.Platforms.Android.Renderers
{
    public class CustomWebViewRenderer : WebViewHandler
    {
        
    }
}