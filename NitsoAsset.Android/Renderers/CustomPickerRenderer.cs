using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Views;
using NitsoAsset.Assets.Controls;
using NitsoAsset.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace NitsoAsset.Droid.Renderers
{
    public class CustomPickerRenderer : PickerRenderer
    {
        public CustomPickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            try
            {
                var view = (CustomPicker)Element;

                if (view != null && Control != null)
                {
                    SetPlaceholderColor(view);
                    SetBorder(view);
                    SetTextAlignment(view);

                    if (!string.IsNullOrEmpty(view.Image))
                    {
                        Control.Background = AddPickerStyles(view.Image);
                        this.Control.SetPadding(Control.PaddingLeft + 0, Control.PaddingTop + 0, Control.PaddingRight, Control.PaddingBottom + 0);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public LayerDrawable AddPickerStyles(string imagePath)
        {
            ShapeDrawable border = new ShapeDrawable();
            border.Paint.Color = Android.Graphics.Color.Transparent;
            border.SetPadding(10, 5, 10, 5);
            border.Paint.SetStyle(Paint.Style.Stroke);

            Drawable[] layers = { border, GetDrawable(imagePath) };
            LayerDrawable layerDrawable = new LayerDrawable(layers);
            layerDrawable.SetLayerInset(0, 0, 0, 0, 0);


            return layerDrawable;
        }

        private BitmapDrawable GetDrawable(string imagePath)
        {
            int resID = Resources.GetIdentifier(imagePath, "drawable", this.Context.PackageName);
            var drawable = Resources.GetDrawable(imagePath);
            //var drawable = ContextCompat.GetDrawable(this.Context, resID);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;

            //var result = new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, 70, 70, true));

            var result = new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, bitmap.Width, bitmap.Height, true));

            result.Gravity = Android.Views.GravityFlags.Right;

            return result;
        }

        void SetBorder(CustomPicker view)
        {
            try
            {
                Control.SetBackgroundColor(global::Android.Graphics.Color.Argb(0, 0, 0, 0));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void SetPlaceholderColor(CustomPicker view)
        {
            try
            {
                //Control.TintColor = view.TextColor.ToUIColor();
                Control.SetHintTextColor(view.PlaceholderColor.ToAndroid());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        void SetTextAlignment(CustomPicker view)
        {
            try
            {
                switch (view.XAlign)
                {
                    case Xamarin.Forms.TextAlignment.Center:
                        Control.Gravity = GravityFlags.Center;
                        break;
                    case Xamarin.Forms.TextAlignment.End:
                        Control.Gravity = GravityFlags.End | GravityFlags.CenterVertical;
                        break;
                    case Xamarin.Forms.TextAlignment.Start:
                        Control.Gravity = GravityFlags.Start | GravityFlags.CenterVertical;
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}