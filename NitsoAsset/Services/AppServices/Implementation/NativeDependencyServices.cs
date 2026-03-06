using System;
using Xamarin.Forms;

namespace NitsoAsset.Services.AppServices.Implementation
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