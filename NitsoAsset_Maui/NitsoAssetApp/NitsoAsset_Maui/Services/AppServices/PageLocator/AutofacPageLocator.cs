using System;
using Autofac;
using NitsoAsset_Maui.Pages.Base;
using NitsoAsset_Maui.ViewModels.Base;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace NitsoAsset_Maui.Services.AppServices.PageLocator
{
    public class AutofacPageLocator : PageLocator
    {
        private readonly ILifetimeScope container;

        public AutofacPageLocator(ILifetimeScope container)
        {
            this.container = container;
        }

        protected override ICustomPage CreatePage(Type pageType)
        {
            return this.container.Resolve(pageType) as ICustomPage;
        }

        protected override IViewModel CreateViewModel(Type viewModelType)
        {
            return this.container.Resolve(viewModelType) as IViewModel;
        }
    }
}