using System;
using Xamarin.Forms;
namespace NitsoAsset.Assets.Controls
{
    public class CustomPicker : Picker
    {
        public CustomPicker()
        {
        }

        public static readonly BindableProperty ImageProperty =
            BindableProperty.Create(nameof(Image), typeof(string), typeof(CustomPicker), string.Empty);

        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }


        public static readonly BindableProperty XAlignProperty =
            BindableProperty.Create("XAlign", typeof(TextAlignment), typeof(CustomPicker), TextAlignment.Start);

        public TextAlignment XAlign
        {
            get
            {
                return (TextAlignment)GetValue(XAlignProperty);
            }
            set
            {
                SetValue(XAlignProperty, value);
            }
        }

        public static readonly BindableProperty PlaceholderColorProperty =
            BindableProperty.Create("PlaceholderColor", typeof(Color), typeof(CustomPicker), Color.Transparent);

        public Color PlaceholderColor
        {
            get
            {
                return (Color)GetValue(PlaceholderColorProperty);
            }
            set
            {
                SetValue(PlaceholderColorProperty, value);
            }
        }

    }
}