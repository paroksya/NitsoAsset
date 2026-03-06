using System;
using System.Windows.Input;
using NitsoAsset_Maui.Models;
using NitsoAsset_Maui.Services.AppServices;
using NitsoAsset_Maui.Services.Data;
using NitsoAsset_Maui.ViewModels.Base;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace NitsoAsset_Maui.ViewModels
{
    public class ScannerDetailsPageViewModel : ViewModel
    {
        #region Properties
        private INavigationService _navigationService { get; }
        CustomProxy _customProxy { get; }

        public Page ScannerDetailsPage { get; set; }

        string _scanDetails;
        public string ScanDetails
        {
            get { return _scanDetails; }
            set { _scanDetails = value; OnPropertyChanged(); }
        }

        string _assetCode;
        public string AssetCode
        {
            get { return _assetCode; }
            set { _assetCode = value; OnPropertyChanged(); }
        }

        Asset _assetCodeInfo;
        public Asset AssetCodeInfo
        {
            get { return _assetCodeInfo; }
            set { _assetCodeInfo = value; OnPropertyChanged(); }
        }
        #endregion

        #region Commands
        public ICommand ScanCommand
        {
            get
            {
                return new RelayCommandWithArgsAsync<string>(async (arg) =>
                {
                    //var page = new QRcodeScannerPage();
                    //await App.Current.MainPage.Navigation.PushAsync(page);
                    //await Navigation.NavigateToAsync<QRcodeScannerPageViewModel>();
                }, this);
            }
        }
        #endregion

        public ScannerDetailsPageViewModel(INavigationService navigationService, CustomProxy customProxy, IAppUtility appUtility) : base(navigationService, customProxy, appUtility)
        {
            Title = "Asset Details";
        }

        #region Methods
        public async override void Init(object args)
        {
            base.Init(args);
            if (args != null)
            {
                AssetCodeInfo = args as Asset;
            }
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();
            ScanDetails = App.ScanResult;
            AssetCode = App.ScanResultCode;
            BindData();
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            Navigation.SetMainViewModel<DashBoardPageViewModel>();
        }

        public async void BindData()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}