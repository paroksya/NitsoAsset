using System;
using Android.Content;
using NitsoAsset.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(DatePicker), typeof(CustomDatePickerRenderer))]
namespace NitsoAsset.Droid.Renderers
{
    public class CustomDatePickerRenderer : DatePickerRenderer
    {
        public CustomDatePickerRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            var view = (DatePicker)Element;

            if (view != null && Control != null)
            {
                SetBackgroundColor(view);
                Control.SetBackgroundColor(global::Android.Graphics.Color.Argb(0, 0, 0, 0));
                Control?.SetPadding(Control.PaddingLeft, 0, Control.PaddingRight, 0);
            }
        }

        void SetBackgroundColor(DatePicker view)
        {
            Control.SetBackgroundColor(view.BackgroundColor.ToAndroid());
        }

    }
}