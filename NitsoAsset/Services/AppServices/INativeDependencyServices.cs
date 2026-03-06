using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NitsoAsset.Services.AppServices
{
    public interface INativeDependencyServices
    {
        void HideKeyboard();
        Task<bool> EnableGPSService();
    }
}