using System;
namespace NitsoAsset.Services.AppServices
{
    public interface IViewLocationFetcher
    {
        System.Drawing.PointF GetCoordinates(global::Xamarin.Forms.VisualElement view);
    }
}