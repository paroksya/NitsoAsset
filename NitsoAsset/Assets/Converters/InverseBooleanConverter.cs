using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace NitsoAsset.Assets.Converters
{
    public class InverseBooleanConverter : IValueConverter, IMarkupExtension
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                try
                {
                    var flag = (bool)value;

                    return !flag;
                }
                catch (Exception ex)
                { }
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}