using System;
using CoreAnimation;
using NitsoAsset.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(DatePicker), typeof(CustomDatePickerRenderer))]
namespace NitsoAsset.iOS.Renderers
{
    public class CustomDatePickerRenderer : DatePickerRenderer
    {
        public CustomDatePickerRenderer()
        {
        }
        CALayer border = new CALayer();
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            var view = (DatePicker)Element;

            if (view != null && Control != null)
            {

                Control.BorderStyle = UITextBorderStyle.None;

                //try
                //{
                //    UIView paddingView = new UIView(new CoreGraphics.CGRect(0, 0, 10, 20));
                //    Control.LeftView = paddingView;
                //    Control.LeftViewMode = UITextFieldViewMode.Always;
                //}
                //catch (Exception ex)
                //{
                //    Debug.WriteLine(ex.Message);
                //}
            }
        }
        [Foundation.Export("textField:shouldChangeCharactersInRange:replacementString:")]
        public bool ShouldChangeCharacters(UIKit.UITextField textField, Foundation.NSRange range, string replacementString)
        {
            return false;
        }
    }
}