using System;
using NitsoAsset.ViewModels.Base;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace NitsoAsset.Services.AppServices.PageLocator
{
    public interface IPageLocator
    {
        Page ResolvePageAndViewModel(Type viewModelType, object args = null);

        Page ResolvePage(IViewModel viewModel);

        Type ResolvePageType(Type viewmodel);

        PopupPage ResolvePopupAndViewModel(Type viewModelType, object args = null);
    }
}