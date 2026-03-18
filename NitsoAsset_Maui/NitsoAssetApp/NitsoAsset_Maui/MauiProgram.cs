using CommunityToolkit.Maui;
using Controls.UserDialogs.Maui;
using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using NitsoAsset_Maui.Assets.Controls;
using ZXing.Net.Maui.Controls;

//using NitsoAsset_Maui.Platforms.Android.Renderers;
#if ANDROID
using NitsoAsset_Maui.Platforms.Android.Renderers;
using NitsoAsset_Maui.Platforms;
#endif

namespace NitsoAsset_Maui
{
    public static class MauiProgram
    {
        //public static CustomWebViewHandler GlobalWebViewHandler;

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureMopups()
                .UseBarcodeReader()
                .UseUserDialogs(registerInterface: true, () =>
                {
#if ANDROID
                    var fontFamily = "OpenSans-Regular.ttf";
#else
                    var fontFamily = "OpenSans-Semibold.ttf";
#endif
                    AlertConfig.DefaultMessageFontFamily = fontFamily;
                    AlertConfig.DefaultUserInterfaceStyle = UserInterfaceStyle.Dark;
                    AlertConfig.DefaultPositiveButtonTextColor = Colors.Purple;
                    ConfirmConfig.DefaultMessageFontFamily = fontFamily;
                    ActionSheetConfig.DefaultMessageFontFamily = fontFamily;
                    ToastConfig.DefaultMessageFontFamily = fontFamily;
                    SnackbarConfig.DefaultMessageFontFamily = fontFamily;
                    HudDialogConfig.DefaultMessageFontFamily = fontFamily;
                })
                .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Bold.ttf", "OpenSansBold");
                fonts.AddFont("OpenSans-BoldItalic.ttf", "OpenSansBoldItalic");
                fonts.AddFont("OpenSans-ExtraBold.ttf", "OpenSansExtraBold");
                fonts.AddFont("OpenSans-ExtraBoldItalic.ttf", "OpenSansExtraBoldItalic");
                fonts.AddFont("OpenSans-Italic.ttf", "OpenSansItalic");
                fonts.AddFont("OpenSans-Light.ttf", "OpenSansLight");
                fonts.AddFont("OpenSans-LightItalic.ttf", "OpenSansLightItalic");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("OpenSans-SemiBoldItalic.ttf", "OpenSansSemiBoldItalic");
            })
                  .ConfigureMauiHandlers(handlers =>
        {
#if ANDROID
            handlers.AddHandler<Microsoft.Maui.Controls.WebView, CustomWebViewHandler>();
#endif
        })

       //                 .ConfigureMauiHandlers(handlers =>
       //                 {
       // #if ANDROID
       //                     handlers.AddHandler<Microsoft.Maui.Controls.WebView, CustomWebViewHandler>();
       // #endif
       //                 })


       .UseMauiCommunityToolkit();




            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(CustomEntry), (handler, view) =>
       {

#if __ANDROID__
           handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif __IOS__
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
       });
            Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping(nameof(CustomPicker), (handler, view) =>
            {
#if __ANDROID__
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif __IOS__
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
#endif
            });
            Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping(nameof(Editor), (handler, view) =>
            {

#if __ANDROID__
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif __IOS__
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
#endif
            });

            //builder.ConfigureMauiHandlers(handlers =>
            //{
            //    handlers.AddHandler(typeof(WebView), typeof(CustomWebViewHandler));
            //});



            // ✅✅✅ ALTERNATIVE: Use ModifyMapping instead of AppendToMapping ✅✅✅
            // #if ANDROID
            //             Microsoft.Maui.Handlers.WebViewHandler.Mapper.ModifyMapping(
            //                 nameof(Microsoft.Maui.IWebView.Source),
            //                 (handler, view, previousAction) =>
            //             {
            //                 // First, call the previous/default action
            //                 previousAction?.Invoke(handler, view);

            //                 System.Diagnostics.Debug.WriteLine("🔵🔵🔵 WebView ModifyMapping CALLED (after Source set)! 🔵🔵🔵");

            //                 var platformWebView = handler.PlatformView;

            //                 // Check if already configured
            //                 if (platformWebView.Tag?.ToString() == "configured")
            //                 {
            //                     System.Diagnostics.Debug.WriteLine("⏭️ Already configured, skipping...");
            //                     return;
            //                 }

            //                 // Mark as configured
            //                 platformWebView.Tag = new Java.Lang.String("configured");

            //                 // Configure WebView settings
            //                 platformWebView.Settings.JavaScriptEnabled = true;
            //                 platformWebView.Settings.DomStorageEnabled = true;
            //                 platformWebView.Settings.AllowFileAccess = true;
            //                 platformWebView.Settings.AllowContentAccess = true;
            //                 platformWebView.Settings.MediaPlaybackRequiresUserGesture = false;
            //                 platformWebView.Settings.SetGeolocationEnabled(true);
            //                 platformWebView.Settings.DatabaseEnabled = true;
            //                 platformWebView.Settings.MixedContentMode = Android.Webkit.MixedContentHandling.AlwaysAllow;

            //                 // ✅ Set custom WebChromeClient
            //                 platformWebView.SetWebChromeClient(new CustomWebChromeClient());

            //                 System.Diagnostics.Debug.WriteLine("🔵🔵🔵 CustomWebChromeClient SET via ModifyMapping! 🔵🔵🔵");
            //             });
            // #endif

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();

        }
    }
}