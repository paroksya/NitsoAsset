using System;
using System.Windows.Input;
using NitsoAsset_Maui.Assets.Helpers;
using NitsoAsset_Maui.Services.AppServices;
using NitsoAsset_Maui.Services.Data;
using NitsoAsset_Maui.ViewModels.Base;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace NitsoAsset_Maui.ViewModels.Popups
{
    public class AlertLogoutPopupViewModel : ViewModel
    {
        #region Properties
        private INavigationService _navigationService { get; }
        CustomProxy _customProxy { get; }
        private IAppUtility _utilityService { get; }
        public ICommand HandleClosePopupCommand { get; set; }
        #endregion

        #region Command
        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommandWithArgsAsync<object>(async (arg) =>
                {
                    await Navigation.ClosePopup();
                }, this);
            }
        }
        public ICommand ConfirmCommand
        {
            get
            {
                return new RelayCommandWithArgsAsync<object>(async (arg) =>
                {
                    await Navigation.ClosePopup();
                    ClearUserData();
                    Navigation.SetMainViewModel<LoginPageViewModel>();
                }, this);
            }
        }

        //public ICommand CloseCommand
        //{
        //    get
        //    {
        //        return new RelayCommandWithArgsAsync<object>(async (arg) =>
        //        {
        //            await Navigation.ClosePopup();
        //        }, this);
        //    }
        //}

        public EventHandler<bool> ClosePopupEvent;
        #endregion

        public AlertLogoutPopupViewModel(INavigationService navigationService, CustomProxy customProxy, IAppUtility appUtility) : base(navigationService)
        {
            _customProxy = customProxy;
            _navigationService = navigationService;
            _utilityService = appUtility;
            ShowBusyLoader = true;
        }

        #region Methods
        public override void Init(object args)
        {
            base.Init(args);
        }

        internal void ClearUserData()
        {
            Settings.UserName = string.Empty;
            Settings.Password = string.Empty;
            Settings.UserDetails = null;
            Settings.ServiceToken = null;
        }
        #endregion
    }
}