using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using NitsoAsset.Assets.Controls;
using NitsoAsset.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace NitsoAsset.iOS.Renderers
{
    public class CustomPickerRenderer : PickerRenderer
    {
        CALayer border = new CALayer();
        public CustomPickerRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            var view = (CustomPicker)Element;

            if (view != null && Control != null)
            {
                SetPlaceholderColor(view);
                Control.BorderStyle = UITextBorderStyle.None;


                SetTextAlignment(view);


                if (!string.IsNullOrEmpty(view.Image))
                {
                    var downarrow = UIImage.FromBundle(view.Image);
                    Control.RightViewMode = UITextFieldViewMode.Always;
                    Control.RightView = new UIImageView(downarrow);
                }
            }

        }

        [Foundation.Export("textField:shouldChangeCharactersInRange:replacementString:")]
        public bool ShouldChangeCharacters(UIKit.UITextField textField, Foundation.NSRange range, string replacementString)
        {
            return false;
        }

        void SetPlaceholderColor(CustomPicker view)
        {
            var placeholderString = new NSAttributedString(view.Title, new UIStringAttributes() { ForegroundColor = view.PlaceholderColor.ToUIColor() });
            Control.AttributedPlaceholder = placeholderString;
        }

        void SetTextAlignment(CustomPicker view)
        {
            switch (view.XAlign)
            {
                case TextAlignment.Center:
                    Control.TextAlignment = UITextAlignment.Center;
                    break;
                case TextAlignment.End:
                    Control.TextAlignment = UITextAlignment.Right;
                    break;
                case TextAlignment.Start:
                    Control.TextAlignment = UITextAlignment.Left;
                    break;
            }
        }
    }
}