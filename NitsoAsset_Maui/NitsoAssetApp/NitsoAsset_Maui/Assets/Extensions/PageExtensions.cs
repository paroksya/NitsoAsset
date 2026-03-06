using System;
using NitsoAsset_Maui.Pages.Base;
using NitsoAsset_Maui.ViewModels.Base;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace NitsoAsset_Maui.Assets.Extensions
{
    public static class PageExtensions
    {
        public static void BindViewModel(this ICustomPage page, IViewModel viewModel)
        {
            page.BindingContext = viewModel;
            page.SetBinding<IViewModel>(Page.IsBusyProperty, "IsBusy");
            page.SetBinding<IViewModel>(Page.TitleProperty,"Title");
            page.SetBinding<IViewModel>(Page.IconImageSourceProperty, "Icon");


            page.Appearing += (sender, args) => viewModel.OnAppearing();
            page.Disappearing += (sender, args) => viewModel.OnDisappearing();
            page.PageClosing += (sender, args) => viewModel.OnClosing();
        }
    }
}