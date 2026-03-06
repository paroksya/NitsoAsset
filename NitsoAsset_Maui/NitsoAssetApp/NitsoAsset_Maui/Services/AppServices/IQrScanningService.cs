using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace NitsoAsset_Maui.Services.AppServices
{
    public interface IQrScanningService
    {
        Task<string> ScanAsync();
    }
}