using System;
using Autofac;
using NitsoAsset.Pages.Base;
using NitsoAsset.ViewModels.Base;
using Xamarin.Forms;

namespace NitsoAsset.Services.AppServices.PageLocator
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