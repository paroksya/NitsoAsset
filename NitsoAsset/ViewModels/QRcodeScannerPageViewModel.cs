using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using NitsoAsset.Assets.Helpers;
using NitsoAsset.Assets.Validations;
using NitsoAsset.Services.AppServices;
using NitsoAsset.Services.Data;
using NitsoAsset.ViewModels.Base;
using NitsoAsset.ViewModels.Popups;
using Newtonsoft.Json;
using Xamarin.Forms;
using NitsoAsset.Services;
using Xamarin.Essentials;
using NitsoAsset.Services.AppServices.Implementation;
using ZXing;
using NitsoAsset.Models;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using System.Threading;

namespace NitsoAsset.ViewModels
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
        //public static EventHandler<DCVXamarin.BarcodeResult> DCVHandleQRcodeScannerEvent { get; set; }

        //public async void DCVHandleQRcodeScanner(object sender, DCVXamarin.BarcodeResult obj)
        //{
        //    try
        //    {
        //        if (obj != null)
        //        {
        //            BarcodeNumber = obj.BarcodeText.ToString();

        //            if (BarcodeNumber !=null)
        //            {

        //                Navigation.RemoveFromNavigationStack<QRcodeScannerPageViewModel>();
        //                Navigation.RemoveFromNavigationStack<VerifyPageViewModel>();
        //                await Navigation.NavigateToAsync<VerifyPageViewModel>(BarcodeNumber);
        //            }

        //            //await Navigation.PopAsync();
        //            //Navigation.RemoveFromNavigationStack<VerifyPageViewModel>();
        //            //await Navigation.NavigateToAsync<VerifyPageViewModel>(tempdata);
        //            //Navigation.RemoveFromNavigationStack<QRcodeScannerPageViewModel>();
        //        }
        //        else
        //        {
        //            _invalidQRCodePopup = Navigation.ResolvepopupFor<InvalidQRCodePopupViewModel>();
        //            _invalidQRCodePopupContext = (InvalidQRCodePopupViewModel)_invalidQRCodePopup.BindingContext;
        //            _invalidQRCodePopupContext.ConfirmTitle = "Invalid Code";
        //            _invalidQRCodePopupContext.ConfirmMessage = "Want to Rescan ?";
        //            _invalidQRCodePopupContext.CancelBtnText = "CANCEL";
        //            _invalidQRCodePopupContext.ConfirmBtnText = "YES";

        //            _invalidQRCodePopupContext.HandleClosePopupCommand = RescanCommand;

        //            await Navigation.Navigation.PushPopupAsync(_invalidQRCodePopup);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await Navigation.ShowPopup<AlertPopupViewModel>("Invalid QR code");
        //        IsAnalyzing = true;
        //    }
        //}

        public static EventHandler<ZXing.Result> HandleQRcodeScannerEvent { get; set; }

        public async void HandleQRcodeScanner(object sender, ZXing.Result obj)
        {
            try
            {
                BarcodeNumber = obj.Text;

                //if (BarcodeNumber != null)
                //{
                //    var tempdata = BarcodeNumber;

                //    Navigation.RemoveFromNavigationStack<VerifyPageViewModel>();
                //    await Navigation.NavigateToAsync<VerifyPageViewModel>(tempdata);
                //    Navigation.RemoveFromNavigationStack<QRcodeScannerPageViewModel>();
                //}
                //else
                //{
                //    _invalidQRCodePopup = Navigation.ResolvepopupFor<InvalidQRCodePopupViewModel>();
                //    _invalidQRCodePopupContext = (InvalidQRCodePopupViewModel)_invalidQRCodePopup.BindingContext;
                //    _invalidQRCodePopupContext.ConfirmTitle = "Invalid Code";
                //    _invalidQRCodePopupContext.ConfirmMessage = "Want to Rescan ?";
                //    _invalidQRCodePopupContext.CancelBtnText = "CANCEL";
                //    _invalidQRCodePopupContext.ConfirmBtnText = "YES";
                //    //_invalidQRCodePopupContext.ConfirmArgs = arg;
                //    //_confirmPopupContext.IsDelete = false;

                //    _invalidQRCodePopupContext.HandleClosePopupCommand = RescanCommand;

                //    await Navigation.Navigation.PushPopupAsync(_invalidQRCodePopup);

                //    //UserDialogs.Instance.Toast("Invalid QR code");
                //    //await Navigation.ShowPopup<AlertPopupViewModel>("Invalid QR code");
                //    //IsAnalyzing = true;
                //}



                AssetRequestModel model = new AssetRequestModel();
                model.assetcode = obj.Text;
                model.CompanyCode = Settings.CompanyCode; //"demo1";

                using (UserDialogs.Instance.Loading("Loading...."))
                {
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
                        _invalidQRCodePopup = Navigation.ResolvepopupFor<InvalidQRCodePopupViewModel>();
                        _invalidQRCodePopupContext = (InvalidQRCodePopupViewModel)_invalidQRCodePopup.BindingContext;
                        _invalidQRCodePopupContext.ConfirmTitle = "Invalid Code";
                        _invalidQRCodePopupContext.ConfirmMessage = "Want to Rescan ?";
                        _invalidQRCodePopupContext.CancelBtnText = "CANCEL";
                        _invalidQRCodePopupContext.ConfirmBtnText = "YES";
                        //_invalidQRCodePopupContext.ConfirmArgs = arg;
                        //_confirmPopupContext.IsDelete = false;

                        _invalidQRCodePopupContext.HandleClosePopupCommand = RescanCommand;

                        await Navigation.Navigation.PushPopupAsync(_invalidQRCodePopup);

                        //UserDialogs.Instance.Toast("Invalid QR code");
                        //await Navigation.ShowPopup<AlertPopupViewModel>("Invalid QR code");
                        //IsAnalyzing = true;
                    }
                }
            }
            catch (Exception ex)
            {
                await Navigation.ShowPopup<AlertPopupViewModel>("Invalid QR code");
                IsAnalyzing = true;
            }
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