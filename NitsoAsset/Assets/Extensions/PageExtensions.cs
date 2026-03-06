using System;
using NitsoAsset.Pages.Base;
using NitsoAsset.ViewModels.Base;
using Xamarin.Forms;
namespace NitsoAsset.Assets.Extensions
{
    public static class PageExtensions
    {
        public static void BindViewModel(this ICustomPage page, IViewModel viewModel)
        {
            page.BindingContext = viewModel;
            page.SetBinding<IViewModel>(Page.IsBusyProperty, x => x.IsBusy);
            page.SetBinding<IViewModel>(Page.TitleProperty, x => x.Title);
            page.SetBinding<IViewModel>(Page.IconProperty, x => x.Icon);


            page.Appearing += (sender, args) => viewModel.OnAppearing();
            page.Disappearing += (sender, args) => viewModel.OnDisappearing();
            page.PageClosing += (sender, args) => viewModel.OnClosing();
        }
    }
}