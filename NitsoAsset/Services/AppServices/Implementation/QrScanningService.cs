using System;
using Xamarin.Forms;

namespace NitsoAsset.Services.AppServices.Implementation
{
    public class QrScanningService
    {
        static readonly Lazy<IQrScanningService> _instanceHolder =
               new Lazy<IQrScanningService>(() => GetInstance());


        static IQrScanningService GetInstance()
        {
            return DependencyService.Get<IQrScanningService>();
        }

        public static IQrScanningService Instance => _instanceHolder.Value;
    }
}