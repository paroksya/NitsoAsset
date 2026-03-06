using System;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace NitsoAsset_Maui.Services.AppServices.Implementation
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