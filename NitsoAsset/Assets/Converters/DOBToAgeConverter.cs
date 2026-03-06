using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace NitsoAsset.Assets.Converters
{
    public class DOBToAgeConverter : IValueConverter, IMarkupExtension
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                var dateOfBirth = (DateTime)value;

                int age = 0;
                age = DateTime.Now.Year - dateOfBirth.Year;
                if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                    age = age - 1;

                return age;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return DateTime.Now;
            return DateTime.Parse(value as string);
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return null;
        }
    }
}