using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NitsoAsset.Pages.Base;
using NitsoAsset.Services.AppServices.PageLocator;
using NitsoAsset.ViewModels.Base;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace NitsoAsset.Services.AppServices.Implementation
{
    public class NavigationService : INavigationService
    {
        private DateTime _lastNavTime = DateTime.MinValue;
        public INavigation Navigation { get; private set; }
        static INavigation _navigation { get; set; }
        protected IPageLocator PageLocator { get; private set; }

        public NavigationService(IPageLocator pageLocator)
        {
            this.PageLocator = pageLocator;
        }

        public void SetMainViewModel<T>(object args = null) where T : IViewModel
        {
            Action setmainView = () =>
            {
                try
                {
                    var page = ResolvePageFor<T>(args);

                    if (page == null)
                        throw new Exception("Resolve page for " + typeof(T).Name + " returned null!");

                    var masterDetailPage = page as MasterDetailPage;

                    if (masterDetailPage != null)
                    {
                        masterDetailPage.Detail = new CustomNavigationPage(masterDetailPage.Detail);
                        Navigation = masterDetailPage.Detail.Navigation;
                        _navigation = masterDetailPage.Detail.Navigation;
                        App.Current.MainPage = masterDetailPage;
                    }
                    else
                    {
                        var navigationPage = new CustomNavigationPage(page);
                        Navigation = navigationPage.Navigation;
                        _navigation = navigationPage.Navigation;
                        App.Current.MainPage = navigationPage;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            };

            if (Device.RuntimePlatform == Device.Android)
            {
                Device.BeginInvokeOnMainThread(setmainView);
            }
            else
            {
                if (App.Current.MainPage == null)
                {
                    setmainView();
                }
                else
                {
                    Device.BeginInvokeOnMainThread(setmainView);
                }
            }
        }

        public Task NavigateToAsync<T>(object args = null, bool animation = true) where T : IViewModel
        {
            try
            {
                var page = ResolvePageFor<T>(args);
                if (page == null)
                {
                    var msg = "No page found for args: " + args == null
                        ? "null" :
                        args?.GetType() + " " + args;

                    return Task.FromResult(false);
                }

                if (Navigation.NavigationStack.Last().GetType() != page.GetType() || (DateTime.Now - _lastNavTime).Seconds > 1)
                {
                    _lastNavTime = DateTime.Now;
                    var tcs = new TaskCompletionSource<bool>();

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            if (Navigation == null)
                                Navigation = _navigation;

                            await Navigation.PushAsync(page, animation);
                            tcs.SetResult(true);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                            tcs.SetException(ex);
                        }
                    });

                    return tcs.Task;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return Task.FromResult(false);
        }

        public async Task NavigateToAsync<T1, T2>(object args1 = null, object args2 = null)
            where T1 : IViewModel
            where T2 : IViewModel
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var page1 = ResolvePageFor<T1>(args1);
                    var page2 = ResolvePageFor<T2>(args2);
                    await Navigation.PushAsync(page1);
                    Navigation.InsertPageBefore(page2, page1);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });
        }

        public async Task NavigateToAsync<T1, T2, T3>(object args1 = null, object args2 = null, object args3 = null)
            where T1 : IViewModel
            where T2 : IViewModel
            where T3 : IViewModel
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var page1 = ResolvePageFor<T1>(args1);
                    var page2 = ResolvePageFor<T2>(args2);
                    var page3 = ResolvePageFor<T3>(args3);

                    await Navigation.PushAsync(page1);
                    Navigation.InsertPageBefore(page2, page1);
                    Navigation.InsertPageBefore(page3, page2);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });
        }

        public async Task NavigateToModalAsync<T>(object args = null) where T : IViewModel
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var page = ResolvePageFor<T>(args);
                    await Navigation.PushModalAsync(page);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });
        }

        public bool IsPopping
        {
            get;
            set;
        }

        public async Task PopAsync(bool animation = true)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await Navigation.PopAsync(animation);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });
        }

        public async Task PopModalAsync()
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopModalAsync();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task PopToRootAsync()
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopToRootAsync();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public Page ResolvePageFor<T>(object args = null) where T : IViewModel
        {
#if DEBUG
            Debug.WriteLine("Resolving page for VM " + typeof(T).Name);
            var sw = new Stopwatch();
            sw.Start();
#endif

            var page = PageLocator.ResolvePageAndViewModel(typeof(T), args);

#if DEBUG
            sw.Stop();
            Debug.WriteLine("Page resolved in " + sw.ElapsedMilliseconds + "ms");
#endif

            return page;
        }

        public PopupPage ResolvepopupFor<T>(object args = null) where T : IViewModel
        {
#if DEBUG
            Debug.WriteLine("Resolving page for VM " + typeof(T).Name);
            var sw = new Stopwatch();
            sw.Start();
#endif

            var page = PageLocator.ResolvePopupAndViewModel(typeof(T), args);

#if DEBUG
            sw.Stop();
            Debug.WriteLine("Page resolved in " + sw.ElapsedMilliseconds + "ms");
#endif

            return page;
        }

        public void RemoveFromNavigationStack<T>(bool removeFirstOccurenceOnly = true) where T : IViewModel
        {
            if (Navigation != null)
            {
                Type pageType = PageLocator.ResolvePageType(typeof(T));

                var navigationStack = Navigation.NavigationStack.Reverse();
                foreach (var page in navigationStack)
                {
                    if (page.GetType() == pageType)
                    {
                        Navigation.RemovePage(page);
                        if (removeFirstOccurenceOnly)
                            break;
                    }
                }
            }
        }

        public void PopToPage<T>() where T : IViewModel
        {
            try
            {
                if (Navigation != null)
                {
                    Type pageType = PageLocator.ResolvePageType(typeof(T));

                    var navigationStack = Navigation.NavigationStack.Reverse();

                    if (!navigationStack.Any(page => page.GetType() == pageType))
                        return;

                    foreach (var page in navigationStack)
                    {
                        if (page.GetType() == pageType)
                        {
                            break;
                        }
                        else
                        {
                            Navigation.RemovePage(page);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }

        }

        public IReadOnlyList<IViewModel> GetNavigationStack()
        {
            try
            {
                if (Navigation != null && Navigation.NavigationStack != null && Navigation.NavigationStack.Any())
                {
                    return Navigation.NavigationStack
                        .Where(page => page?.BindingContext is IViewModel)
                        .Select(page => page.BindingContext as IViewModel).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return Enumerable.Empty<IViewModel>().ToList();
        }

        public void NotifyViewModel<T>(object sender, ViewModelNotificationType notificationType, object args = null) where T : IViewModel
        {
            //if (typeof(T) == typeof(DrawerMenuViewModel))
            //{
            //    var masterDetailPage = App.MainPage as MasterDetailPage;

            //    if (masterDetailPage != null)
            //    {
            //        var drawerViewModel = masterDetailPage?.Master?.BindingContext as DrawerMenuViewModel;
            //        drawerViewModel.OnNavigationServiceNotification(sender, notificationType, args);
            //    }
            //}

            var navigationStack = GetNavigationStack().Reverse();
            foreach (var viewModel in navigationStack)
            {
                if (viewModel.GetType() == typeof(T))
                {
                    viewModel.OnNavigationServiceNotification(sender, notificationType, args);
                    break;
                }
                //else if (viewModel.GetType().IsAssignableTo<ITabbedViewModel>())
                //{
                //    var tabbedViewModel = (ITabbedViewModel)viewModel;
                //    if (tabbedViewModel.ChildViewModels.Any(x => x.Value.GetType() == typeof(T)))
                //    {
                //        tabbedViewModel.ChildViewModels.First(x => x.Value.GetType() == typeof(T)).Value.
                //            OnNavigationServiceNotification(sender, notificationType, args);
                //        break;
                //    }
                //}
            }
        }

        public bool IsRootPage
        {
            get { return Navigation.NavigationStack.Count == 1; }
        }

        public IViewModel CurrentViewModel
        {
            get { return CurrentPage?.BindingContext as IViewModel; }
        }

        public IViewModel CurrentModalViewModel
        {
            get
            {
                if (Navigation == null || Navigation.ModalStack == null)
                    return null;

                return Navigation.ModalStack.LastOrDefault()?.BindingContext as IViewModel;
            }
        }

        public Page CurrentPage
        {
            get { return Navigation?.NavigationStack?.LastOrDefault(); }
        }

        public void OpenDrawerMenu()
        {
            PresentDrawerMenu(true);
        }

        public void CloseDrawerMenu()
        {
            PresentDrawerMenu(false);
        }

        public void ToggleDrawerMenu()
        {
            var masterDetailPage = App.Current.MainPage as MasterDetailPage;

            if (masterDetailPage != null)
                masterDetailPage.IsPresented = !masterDetailPage.IsPresented;
        }

        private void PresentDrawerMenu(bool isPresented)
        {
            var masterDetailPage = App.Current.MainPage as MasterDetailPage;

            if (masterDetailPage != null)
                masterDetailPage.IsPresented = isPresented;
        }

        public async Task GoBackTo<TViewModel>()
        {
            if (Navigation != null)
            {
                Type pageType = PageLocator.ResolvePageType(typeof(TViewModel));

                var navigationStack = Navigation.NavigationStack.Reverse().Skip(1).ToList();
                foreach (var page in navigationStack)
                {
                    if (page.GetType() != pageType)
                    {
                        Navigation.RemovePage(page);
                    }
                    else
                    {
                        break;
                    }
                }

                await this.PopAsync();
            }
        }

        public Task ShowPopup<T>(object args = null) where T : IViewModel
        {
            try
            {
                var page = ResolvepopupFor<T>(args);
                if (page == null)
                {
                    var msg = "No page found for args: " + args == null
                        ? "null" :
                        args?.GetType() + " " + args;

                    return Task.FromResult(false);
                }

                var popup = page as PopupPage;

                if (Navigation.NavigationStack.Last().GetType() != popup.GetType() || (DateTime.Now - _lastNavTime).Seconds > 1)
                {
                    var tcs = new TaskCompletionSource<bool>();

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            if (Navigation == null)
                                Navigation = _navigation;

                            await Navigation.PushPopupAsync(popup);
                            tcs.SetResult(true);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                            tcs.SetException(ex);
                        }
                    });

                    return tcs.Task;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Task.FromResult(false);
            }
            return Task.FromResult(false);
        }

        public async Task ClosePopup()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    if (PopupNavigation.PopupStack.Count > 0)
                        await Navigation.PopPopupAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });
        }
    }
}