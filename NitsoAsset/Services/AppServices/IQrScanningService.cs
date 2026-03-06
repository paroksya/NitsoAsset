using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NitsoAsset.Services.AppServices
{
    public interface IQrScanningService
    {
        Task<string> ScanAsync();
    }
}