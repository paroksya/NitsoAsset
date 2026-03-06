using System;
using System.Threading.Tasks;
using NitsoAsset.Services;
using NitsoAsset.Services.AppServices;
using Xamarin.Forms;
using ZXing.Mobile;

[assembly: Dependency(typeof(NitsoAsset.Droid.Services.QrScanningService))]
namespace NitsoAsset.Droid.Services
{
    public class QrScanningService : IQrScanningService
    {
        public async Task<string> ScanAsync()
        {
            var optionsDefault = new MobileBarcodeScanningOptions();
            var optionsCustom = new MobileBarcodeScanningOptions();

            var scanner = new MobileBarcodeScanner()
            {
                TopText = "Scan the QR Code",
                BottomText = "Please Wait",
            };

            var scanResult = await scanner.Scan(optionsCustom);
            return scanResult.Text;
        }
    }
}