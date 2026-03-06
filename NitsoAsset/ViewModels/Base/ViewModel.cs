using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using NitsoAsset.Assets.Helpers;
using NitsoAsset.Services.AppServices;
using NitsoAsset.Services.Data;
using NitsoAsset.ViewModels.Popups;
using Plugin.Connectivity;
using Xamarin.Essentials;
using Xamarin.Forms;
using ConnectivityChangedEventArgs = Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs;
namespace NitsoAsset.ViewModels.Base
{
    public abstract class ViewModel : AbstractNpcObject, IViewModel
    {
        private object _initArgs;

        protected readonly INavigationService Navigation;

        protected readonly CustomProxy CustomProxy;
        protected readonly IAppUtility AppUtility;

        protected bool started;

        protected ViewModel(INavigationService navigationService)
        {
            Navigation = navigationService;

            this.ToolbarItems = new ObservableCollection<ToolbarItem>();

            Title = string.Empty; // so we don't forget to set a title ;-)
        }

        protected ViewModel(INavigationService navigationService, CustomProxy customProxy)
        {
            Navigation = navigationService;
            CustomProxy = customProxy;

            this.ToolbarItems = new ObservableCollection<ToolbarItem>();

            Title = string.Empty; // so we don't forget to set a title ;-)
        }
        protected ViewModel(INavigationService navigationService, CustomProxy customProxy, IAppUtility appUtility)
        {
            Navigation = navigationService;
            CustomProxy = customProxy;
            AppUtility = appUtility;

            this.ToolbarItems = new ObservableCollection<ToolbarItem>();

            Title = string.Empty; // so we don't forget to set a title ;-)
        }

        public bool IsVisible { get; private set; }

        public bool IsOffline => !CrossConnectivity.Current.IsConnected;

        bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                    HandleBusyLoader();
                }
            }
        }

        private ToolbarItem _cartTool { get; set; }

        public bool ShowBusyLoader { get; set; }

        public string Title { get; set; }

        public string Icon { get; set; }

        public string Badge { get; set; }

        public virtual Type DrawerMenuViewModelType => null;

        public virtual Type PageType
        {
            get { return null; }
        }


        public ObservableCollection<ToolbarItem> ToolbarItems { get; set; }


        public virtual void Init(object args)
        {
            _initArgs = args;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == null)
                return;

            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(Badge))
            {
                OnBadgeChange();
            }
        }

        public event Action<ViewModel> BadgeChange;

        public void OnBadgeChange()
        {
            BadgeChange?.Invoke(this);
        }

        public void SetDefaultToolbar(bool removeSearch = false)
        {

        }

        #region Comman Comand


        public ICommand PopPageCommand
        {
            get
            {
                return new RelayCommandWithArgsAsync<object>(async (arg) =>
                {
                    if (Navigation != null)
                        await Navigation.PopAsync(false);
                }, this);
            }
        }

        #endregion

        public virtual void OnStart()
        {
            started = true;
        }

        protected virtual void TrackAppearingEvent()
        {
            var dict = new Dictionary<string, string>();

            if (_initArgs != null)
            {
                dict["Init argument type"] = _initArgs.GetType().Name;
                dict["Init argument value"] = _initArgs.ToString();
                //var arg = _initArgs as IDTO;
                //if (arg != null)
                //{
                //    dict["Init argument Id"] = arg.Id;
                //}
            }
            var page = GetType().Name.Replace("ViewModel", "");
        }

        public virtual void OnAppearing()
        {
            IsVisible = true;

            CrossConnectivity.Current.ConnectivityChanged += OnConnectivityChanged;

            TrackAppearingEvent();

            if (!started)
            {
                started = true;
                OnStart();
            }
            //ShowCartBadge();
        }

        public virtual void OnDisappearing()
        {
            IsVisible = false;

            CrossConnectivity.Current.ConnectivityChanged -= OnConnectivityChanged;
        }

        protected virtual void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs connectivityChangedEventArgs)
        {
        }

        public virtual void OnClosing()
        {

        }

        public virtual void OnNavigationServiceNotification(object sender, ViewModelNotificationType notificationType, object args)
        {
            var data = new Dictionary<string, string>()
            {
                {"Sender", sender.GetType().Name},
                {"Receiver", GetType().Name},
            };
        }

        internal async void HandleBusyLoader()
        {
            try
            {
                if (ShowBusyLoader)
                {
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.Write(ex);
            }
        }

        public async Task<bool> HandleInternetConnection()
        {
            var reacheble = Connectivity.NetworkAccess == NetworkAccess.Internet;
            if (!reacheble)
            {
                await Navigation.ShowPopup<AlertPopupViewModel>("Please check your internet connection.");
                return false;
            }
            return true;
        }

        //public bool IsLoggedIn
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(Settings.ServiceToken) && string.IsNullOrEmpty(Settings.UserDetails.UserPid.ToString()))
        //            return false;
        //        else
        //            return true;
        //    }
        //}
    }
}