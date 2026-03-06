using CommunityToolkit.Maui;
using Controls.UserDialogs.Maui;
using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using NitsoAsset_Maui.Assets.Controls;
using ZXing.Net.Maui.Controls;
//using NitsoAsset_Maui.Platforms.Android.Renderers;
#if ANDROID
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
            }).UseMauiCommunityToolkit();

        //.ConfigureMauiHandlers(handlers =>
        //{
//#if ANDROID
//            handlers.AddHandler<WebView, CustomWebViewRenderer>();
//#endif
//        });


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

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();

        }
    }
}