using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Controls.UserDialogs.Maui;
using NitsoAsset_Maui.Assets.Helpers;
using NitsoAsset_Maui.Assets.Validations;
using NitsoAsset_Maui.Services.AppServices;
using NitsoAsset_Maui.Services.Data;
using NitsoAsset_Maui.ViewModels.Base;
using NitsoAsset_Maui.ViewModels.Popups;
using Newtonsoft.Json;
using NitsoAsset_Maui.Services;
using NitsoAsset_Maui.Pages;
using NitsoAsset_Maui.Models;
using NitsoAsset_Maui.Models.Enums;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;

namespace NitsoAsset_Maui.ViewModels
{
    public class DashBoardPageViewModel : ViewModel
    {
        #region Properties
        private INavigationService _navigationService { get; }
        CustomProxy _customProxy { get; }
        public Page DashBoardPage { get; set; }

        ObservableCollection<DashboardModel> _dashboardItems = new ObservableCollection<DashboardModel>();
        public ObservableCollection<DashboardModel> DashboardItems
        {
            get { return _dashboardItems; }
            set { _dashboardItems = value; OnPropertyChanged(); }
        }

        string _userNameDisplay;
        public string UserNameDisplay
        {
            get { return _userNameDisplay; }
            set { _userNameDisplay = value; OnPropertyChanged(); }
        }

        string _companyNameDisplay;
        public string CompanyNameDisplay
        {
            get { return _companyNameDisplay; }
            set { _companyNameDisplay = value; OnPropertyChanged(); }
        }
        #endregion

        #region Commands
        public ICommand LogoutCommand
        {
            get
            {
                return new RelayCommandWithArgsAsync<object>(async (arg) =>
                {
                    Logout();
                }, this);
            }
        }

        public ICommand DashboardItemTapCommand
        {
            get
            {
                return new RelayCommandWithArgsAsync<DashboardModel>(async (arg) =>
                {
                    if (arg == null) return;
                    DashboardItemTap(arg);
                }, this);
            }
        }

        #endregion

        public DashBoardPageViewModel(INavigationService navigationService, CustomProxy customProxy, IAppUtility appUtility) : base(navigationService, customProxy, appUtility)
        {
            Title = "DashBoard";

            UserNameDisplay = Settings.UserName.ToUpper();

            //CompanyNameDisplay = "Company Name : " + Settings.CompanyName.ToUpper();

            CompanyNameDisplay = Settings.CompanyName.ToUpper();
        }

        #region Methods
        public async override void Init(object args)
        {
            base.Init(args);

            var maindata = (IDictionary<string, object>)args;
            if (maindata != null && maindata.ContainsKey("HandlebgLogin") && Settings.SavedDetails != null)
            {
                Settings.UserDetails = JsonConvert.DeserializeObject<UserModel>(Settings.SavedDetails);
            }

            //var _LogoutTool = new ToolbarItem
            //{
            //    IconImageSource = "poweroff",
            //    Text = "Logout",
            //    Command = LogoutCommand
            //};

            //ToolbarItems.Add(_LogoutTool);
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();
            //ScanDetails = App.ScanResult;
            //AssetCode = App.ScanResultCode;
            BindData();
        }

        public async void BindData()
        {
            try
            {
                var tempDashboardItems = new ObservableCollection<DashboardModel>();
                //tempDashboardItems.Add(new DashboardModel(){Title = "Scan", BgColor = Color.FromHex("#ebf9fe"), DashboardNavType = DashboardEnums.SCAN });
                tempDashboardItems.Add(new DashboardModel() { Title = "Asset Code", BgColor = Color.FromArgb("#003f4e"), DashboardNavType = DashboardEnums.ASSETCODE });
                //tempDashboardItems.Add(new DashboardModel() { Title = "Search", BgColor = Color.FromHex("#eaf2fd"), DashboardNavType = DashboardEnums.SEARCH });
                tempDashboardItems.Add(new DashboardModel() { Title = "Verify", BgColor = Color.FromArgb("#003f4e"), DashboardNavType = DashboardEnums.VERIFY });
                tempDashboardItems.Add(new DashboardModel() { Title = "Add Asset", BgColor = Color.FromArgb("#003f4e"), DashboardNavType = DashboardEnums.ADDASSET });
                tempDashboardItems.Add(new DashboardModel() { Title = "Asset List", BgColor = Color.FromArgb("#003f4e"), DashboardNavType = DashboardEnums.ASSETLIST });
                DashboardItems = new ObservableCollection<DashboardModel>(tempDashboardItems);
            }
            catch (Exception ex)
            {

            }
        }

        public async void DashboardItemTap(DashboardModel model)
        {
            try
            {
                if (model == null)
                    return;

                //if (model.DashboardNavType == DashboardEnums.SCAN)
                //{
                //    await Navigation.NavigateToAsync<QRcodeScannerPageViewModel>();
                //}
                if (model.DashboardNavType == DashboardEnums.ASSETCODE)
                {
                    await Navigation.NavigateToAsync<SearchAssetCodePageViewModel>();
                }
                //else if (model.DashboardNavType == DashboardEnums.SEARCH)
                //{
                //    await Navigation.NavigateToAsync<SearchPageViewModel>();
                //}
                else if (model.DashboardNavType == DashboardEnums.VERIFY)
                {
                    await Navigation.NavigateToAsync<VerifyPageViewModel>();
                }
                else if (model.DashboardNavType == DashboardEnums.ADDASSET)
                {
                    await RuntimePermission.RuntimePermissionCameraStatus();
                    await Navigation.NavigateToAsync<AddAssetPageViewModel>();
                }
                else if (model.DashboardNavType == DashboardEnums.ASSETLIST)
                {
                    await Navigation.NavigateToAsync<AssetListPageViewModel>();
                }
            }
            catch (Exception ex)
            {

            }
        }

        internal void ClearUserData()
        {
            Settings.UserName = string.Empty;
            Settings.Password = string.Empty;
            Settings.CompanyCode = string.Empty;
            Settings.UserDetails = null;
            Settings.ServiceToken = null;
            Settings.ServiceBaseURL = null;
        }

        public void Logout()
        {
            Navigation.ShowPopup<AlertLogoutPopupViewModel>();
            //ClearUserData();
        }

        #endregion

        //public override Type DrawerMenuViewModelType => typeof(DrawerPageViewModel);
    }
}