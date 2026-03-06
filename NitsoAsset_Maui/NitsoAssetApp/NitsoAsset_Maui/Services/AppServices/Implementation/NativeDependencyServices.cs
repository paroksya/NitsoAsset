using System;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace NitsoAsset_Maui.Services.AppServices.Implementation
{
    public class NativeDependencyServices
    {
        static readonly Lazy<INativeDependencyServices> _instanceHolder =
                new Lazy<INativeDependencyServices>(() => GetInstance());


        static INativeDependencyServices GetInstance()
        {
            return DependencyService.Get<INativeDependencyServices>();
        }

        public static INativeDependencyServices Instance => _instanceHolder.Value;
    }
}