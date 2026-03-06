using Controls.UserDialogs.Maui;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Mopups;
using Mopups.Pages;
using Newtonsoft.Json;
using NitsoAsset_Maui.Assets.Helpers;
using NitsoAsset_Maui.Assets.Validations;
using NitsoAsset_Maui.Models;
using NitsoAsset_Maui.Services;
using NitsoAsset_Maui.Services.AppServices;
using NitsoAsset_Maui.Services.AppServices.Implementation;
using NitsoAsset_Maui.Services.Data;
using NitsoAsset_Maui.ViewModels.Base;
using NitsoAsset_Maui.ViewModels.Popups;
using System;
using System.Windows.Input;
using ZXing;

namespace NitsoAsset_Maui.ViewModels
{
    public class QRcodeScannerPageViewModel : ViewModel
    {
        #region Properties
        private INavigationService _navigationService { get; }
        CustomProxy _customProxy { get; }
        public Page QRcodeScannerPage { get; set; }

        bool _isAnalyzing = true;
        public bool IsAnalyzing
        {
            get { return _isAnalyzing; }
            set { _isAnalyzing = value; OnPropertyChanged(); }
        }

        bool _isScanning = true;
        public bool IsScanning
        {
            get { return _isScanning; }
            set { _isScanning = value; OnPropertyChanged(); }
        }

        bool _isTorch = false;
        public bool IsTorch
        {
            get { return _isTorch; }
            set { _isTorch = value; OnPropertyChanged(); }
        }
        private bool _isHandlingScan = false;
        PopupPage _invalidQRCodePopup { get; set; }
        InvalidQRCodePopupViewModel _invalidQRCodePopupContext { get; set; }
        public InvalidQRCodePopupViewModel InvalidQRCodePopupContext
        {
            get
            {
                return _invalidQRCodePopupContext;
            }
        }

        string _barcodeNumber;
        public string BarcodeNumber
        {
            get { return _barcodeNumber; }
            set { _barcodeNumber = value; OnPropertyChanged(BarcodeNumber); }
        }

        #endregion

        #region Commands
        public ICommand ScanCommand
        {
            get
            {
                return new RelayCommandWithArgsAsync<string>(async (arg) =>
                {
                }, this);
            }
        }
        public ICommand FlashCommand
        {
            get
            {
                return new RelayCommandWithArgsAsync<string>(async (arg) =>
                {
                    IsTorch = !IsTorch;
                }, this);
            }
        }

        public ICommand RescanCommand
        {
            get
            {
                return new RelayCommandWithArgsAsync<object>(async (arg) =>
                {
                    IsAnalyzing = true;
                }, this);
            }
        }

        #endregion
        public QRcodeScannerPageViewModel(INavigationService navigationService, CustomProxy customProxy, IAppUtility appUtility) : base(navigationService, customProxy, appUtility)
        {
            Title = "Scanner";
            //DCVHandleQRcodeScannerEvent = DCVHandleQRcodeScanner;
            HandleQRcodeScannerEvent = HandleQRcodeScanner;
            _navigationService = navigationService;
        }

        #region Methods
        public static EventHandler<ZXing.Net.Maui.BarcodeDetectionEventArgs> HandleQRcodeScannerEvent { get; set; }
        public async void HandleQRcodeScanner(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs obj)
        {
            if (_isHandlingScan)
                return;

            IsAnalyzing = false;
            try
            {
                var scannedValue = obj.Results.FirstOrDefault().Value;
                BarcodeNumber = scannedValue;

                AssetRequestModel model = new AssetRequestModel();
                model.assetcode = obj.Results.FirstOrDefault().Value;
                model.CompanyCode = Settings.CompanyCode; //"demo1";

                UserDialogs.Instance.Loading("Loading....");
                var AssetByCodeResult = await CustomProxy.SearchAssetByCode(model);
                if (AssetByCodeResult != null && AssetByCodeResult.Response != null)
                {
                    Asset AssetCodeDetail = new Asset();
                    AssetCodeDetail = AssetByCodeResult.Response;

                    Navigation.RemoveFromNavigationStack<VerifyPageViewModel>();
                    await Navigation.NavigateToAsync<VerifyPageViewModel>(AssetCodeDetail);
                    Navigation.RemoveFromNavigationStack<QRcodeScannerPageViewModel>();
                }
                else
                {
                    await ShowInvalidQRPopup();
                }
            }
            catch (Exception ex)
            {
                await Navigation.ShowPopup<AlertPopupViewModel>("Invalid QR code");
                IsAnalyzing = true;
            }
            finally
            {
                UserDialogs.Instance.HideHud();
            }
        }

        private async Task ShowInvalidQRPopup()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                _invalidQRCodePopup = Navigation.ResolvepopupFor<InvalidQRCodePopupViewModel>();
                _invalidQRCodePopupContext = (InvalidQRCodePopupViewModel)_invalidQRCodePopup.BindingContext;
                _invalidQRCodePopupContext.HandleClosePopupCommand = RescanCommand;

                var confirmPopupParams = new Dictionary<string, dynamic>
                {
                    { "ConfirmTitle", "Invalid Code" },
                    { "ConfirmMessage", "Want to Rescan ?" },
                    { "CancelBtnText", "CANCEL" },
                    { "ConfirmBtnText", "YES" },
                    { "ConfirmArgs", null},
                    { "ConfirmCommand", new Command(async () => { IsAnalyzing = true; })},
                    { "CancelCommand", new Command(async () => { IsAnalyzing = false; })}
                };

                await Navigation.ShowPopup<InvalidQRCodePopupViewModel>(confirmPopupParams);
            });
        }

        public async override void Init(object args)
        {
            base.Init(args);
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();
        }
        #endregion
    }
}