using System;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Essentials;
using Xamarin.Forms;
namespace NitsoAsset.Droid.Helpers
{
    public static class FixXamarin
    {
        public static void FixDevice()
        {
            try
            {
                var map = new Dictionary<DeviceIdiom, TargetIdiom>()
                {
                    [DeviceIdiom.Desktop] = TargetIdiom.Desktop,
                    [DeviceIdiom.Phone] = TargetIdiom.Phone,
                    [DeviceIdiom.Tablet] = TargetIdiom.Tablet,
                    [DeviceIdiom.Unknown] = TargetIdiom.Unsupported,
                    [DeviceIdiom.Watch] = TargetIdiom.Watch,
                    [DeviceIdiom.TV] = TargetIdiom.TV,
                };

                var deviceIdiomProperty = typeof(Device).GetProperty("Idiom", BindingFlags.Public | BindingFlags.Static);
                var mappedIdiom = map[Xamarin.Essentials.DeviceInfo.Idiom];
                deviceIdiomProperty?.SetValue(null, mappedIdiom);
            }
            catch (Exception ex)
            {

            }
        }
    }
}