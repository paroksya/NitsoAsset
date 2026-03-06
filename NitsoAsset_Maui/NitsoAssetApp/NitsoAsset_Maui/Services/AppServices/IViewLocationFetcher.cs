using System;
using Microsoft.Maui.Controls;

namespace NitsoAsset_Maui.Services.AppServices
{
    public interface IViewLocationFetcher
    {
        //System.Drawing.PointF GetCoordinates(global::Xamarin.Forms.VisualElement view);
        System.Drawing.PointF GetCoordinates(VisualElement view);
    }
}