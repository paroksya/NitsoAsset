using GeolocatorPlugin.Abstractions;
using System;
using System.Threading.Tasks;
namespace NitsoAsset_Maui.Services.AppServices
{
    public interface IAppUtility
    {
        Task<Position> GetCurrentLocation();

        void ImagePicker(Action<FileResult> result);

        string ScanDetails { get; set; }
    }
}