using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Mopups.Pages;
using NitsoAsset_Maui.Assets.Extensions;
using NitsoAsset_Maui.Pages.Base;
using NitsoAsset_Maui.ViewModels.Base;
using Mopups;
using System;
using System.Diagnostics;
using System.Reflection;

namespace NitsoAsset_Maui.Services.AppServices.PageLocator
{
    public class PageLocator : IPageLocator
    {
        protected virtual ICustomPage CreatePage(Type pageType)
        {
            return Activator.CreateInstance(pageType) as ICustomPage;
        }

        protected virtual PopupPage CreatePopup(Type pageType)
        {
            return Activator.CreateInstance(pageType) as PopupPage;
        }

        protected virtual IViewModel CreateViewModel(Type viewModelType)
        {
            return Activator.CreateInstance(viewModelType) as IViewModel;
        }

        protected virtual Type FindPageTypeForViewModel(Type viewModelType)
        {
            var pageTypeName = viewModelType
                .AssemblyQualifiedName
                .Replace("ViewModel", "");

            pageTypeName = pageTypeName.Replace(".s.", ".Pages.");

            var pageType = Type.GetType(pageTypeName);

            if (pageType == null)
                throw new ArgumentException("Can't find a page of type '" + pageTypeName + "' for ViewModel '" +
                                            viewModelType.Name +
                                            "'");

            return pageType;
        }

        public Type ResolvePageType(Type viewModelType)
        {
            return FindPageTypeForViewModel(viewModelType);
        }

        public Page ResolvePageAndViewModel(Type viewModelType, object args)
        {
            Page page = null;

            var viewModel = CreateViewModel(viewModelType);
            try
            {
                var viewModelTypeInfo = viewModelType.GetTypeInfo();


                page = ResolvePage(viewModel);


                viewModel.Init(args);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            if (viewModel.DrawerMenuViewModelType != null)
            {
                var _drawer = ResolvePageAndViewModel(viewModel.DrawerMenuViewModelType, null);
                _drawer.IconImageSource = "menu";
                _drawer.IconImageSource = "menu";

                var masterDetailPage = new FlyoutPage
                {
                    IconImageSource = "menu"
                    //Master = _drawer,
                    //Detail = page
                };

                masterDetailPage.IsPresentedChanged += (sender, eventArgs) =>
                {
                    if (masterDetailPage.IsPresented)
                    {
                        ((IViewModel)masterDetailPage.Flyout.BindingContext).OnAppearing();
                    }
                };

                return masterDetailPage;
            }

            return page;
        }

        public Page ResolvePage(IViewModel viewModel)
        {
            var pageType = viewModel.PageType ?? ResolvePageType(viewModel.GetType());
            var page = CreatePage(pageType);

            if (!(page is ICustomPage))
                throw new ArgumentException("Page for '" + viewModel.GetType().Name +
                                            "' should be of type 'CustomPage' instead of '" +
                                            pageType.Name + "'");

            page.BindViewModel(viewModel);

            return page as Page;
        }

        public PopupPage ResolvePopup(IViewModel viewModel)
        {
            var pageType = viewModel.PageType ?? ResolvePageType(viewModel.GetType());
            var page = CreatePopup(pageType);

            if (!(page is PopupPage))
                throw new ArgumentException("Page for '" + viewModel.GetType().Name +
                                            "' should be of type 'PopupPage' instead of '" +
                                            pageType.Name + "'");

            page.BindingContext = viewModel;

            return page as PopupPage;
        }

        public PopupPage ResolvePopupAndViewModel(Type viewModelType, object args)
        {
            PopupPage page = null;

            var viewModel = CreateViewModel(viewModelType);
            try
            {
                var viewModelTypeInfo = viewModelType.GetTypeInfo();


                page = ResolvePopup(viewModel);
                viewModel.Init(args);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return page;
        }
    }
}