using System;
using NitsoAsset.Assets.Controls;
using NitsoAsset.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace NitsoAsset.Droid.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer() : base(Android.App.Application.Context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            Control?.SetBackgroundColor(Color.Transparent.ToAndroid());
            //Control?.SetPadding(0, 0, 0, 0);

        }
    }
}