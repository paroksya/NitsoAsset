using System;
using System.Threading.Tasks;
using Plugin.Geolocator.Abstractions;
using Plugin.Media.Abstractions;
namespace NitsoAsset.Services.AppServices
{
    public interface IAppUtility
    {
        Task<Position> GetCurrentLocation();

        void ImagePicker(Action<MediaFile> result);

        string ScanDetails { get; set; }
    }
}