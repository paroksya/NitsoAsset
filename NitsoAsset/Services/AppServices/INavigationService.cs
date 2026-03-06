using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NitsoAsset.Pages;
using NitsoAsset.ViewModels.Base;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
namespace NitsoAsset.Services.AppServices
{
    public interface INavigationService
    {
        INavigation Navigation { get; }

        void SetMainViewModel<T>(object args = null) where T : IViewModel;

        Task NavigateToAsync<T>(object args = null, bool animation = true) where T : IViewModel;

        Task NavigateToAsync<T1, T2>(object args1 = null, object args2 = null) where T1 : IViewModel where T2 : IViewModel;

        Task NavigateToAsync<T1, T2, T3>(object args1 = null, object args2 = null, object args3 = null) where T1 : IViewModel where T2 : IViewModel where T3 : IViewModel;

        Task NavigateToModalAsync<T>(object args = null) where T : IViewModel;

        bool IsPopping { get; set; }

        Task PopAsync(bool animation = true);

        Task PopModalAsync();

        Task PopToRootAsync();

        Page ResolvePageFor<T>(object args = null) where T : IViewModel;

        PopupPage ResolvepopupFor<T>(object args = null) where T : IViewModel;

        void RemoveFromNavigationStack<T>(bool removeFirstOccurenceOnly = true) where T : IViewModel;

        void PopToPage<T>() where T : IViewModel;

        IReadOnlyList<IViewModel> GetNavigationStack();

        void NotifyViewModel<T>(object sender, ViewModelNotificationType notificationType, object args = null) where T : IViewModel;

        Task GoBackTo<TViewModel>();

        bool IsRootPage { get; }

        IViewModel CurrentViewModel { get; }

        IViewModel CurrentModalViewModel { get; }

        Page CurrentPage { get; }

        void OpenDrawerMenu();

        void CloseDrawerMenu();

        void ToggleDrawerMenu();

        Task ShowPopup<T>(object args = null) where T : IViewModel;

        Task ClosePopup();
    }
}