using System;
using NitsoAsset_Maui.ViewModels.Base;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Mopups.Pages;

namespace NitsoAsset_Maui.Services.AppServices.PageLocator
{
    public interface IPageLocator
    {
        Page ResolvePageAndViewModel(Type viewModelType, object args = null);

        Page ResolvePage(IViewModel viewModel);

        Type ResolvePageType(Type viewmodel);

        PopupPage ResolvePopupAndViewModel(Type viewModelType, object args = null);
    }
}