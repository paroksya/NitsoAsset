using System;
using NitsoAsset.Assets.Controls;
using NitsoAsset.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace NitsoAsset.iOS.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            var view = (CustomEntry)Element;
            if (view != null && Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;
                Control.Layer.BorderWidth = 0;

                view.Unfocused += (object sender, FocusEventArgs ev) =>
                {
                    Control.ResignFirstResponder();
                    view.Text = (!view.IsPassword) ? view.Text?.Trim() : view.Text;
                };

                view.Focused += (object sender, FocusEventArgs ev) =>
                {
                    Control.BecomeFirstResponder();
                };

            }
        }
    }
}