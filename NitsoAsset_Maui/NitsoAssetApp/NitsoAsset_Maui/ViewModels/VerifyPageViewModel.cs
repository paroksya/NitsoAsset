using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Controls.UserDialogs.Maui;
using NitsoAsset_Maui.Assets.Helpers;
using NitsoAsset_Maui.Assets.Validations;
using NitsoAsset_Maui.Models;
using NitsoAsset_Maui.Services.AppServices;
using NitsoAsset_Maui.Services.Data;
using NitsoAsset_Maui.ViewModels.Base;
using NitsoAsset_Maui.ViewModels.Popups;
//using Plugin.Media;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;

namespace NitsoAsset_Maui.ViewModels
{
    public class VerifyPageViewModel : ViewModel
    {
        #region Properties
        private INavigationService _navigationService { get; }
        CustomProxy _customProxy { get; }
        public Page VerifyPage { get; set; }

        ValidatableObject<string> _verificationQRCodeDescription = new ValidatableObject<string>();
        public ValidatableObject<string> VerificationQRCodeDescription
        {
            get { return _verificationQRCodeDescription; }
            set { _verificationQRCodeDescription = value; OnPropertyChanged(); }
        }

        ValidatableObject<string> _verificationAssetCode = new ValidatableObject<string>();
        public ValidatableObject<string> VerificationAssetCode
        {
            get { return _verificationAssetCode; }
            set { _verificationAssetCode = value; OnPropertyChanged(); }
        }

        ValidatableObject<string> _verificationAssetCodeRemark = new ValidatableObject<string>();
        public ValidatableObject<string> VerificationAssetCodeRemark
        {
            get { return _verificationAssetCodeRemark; }
            set { _verificationAssetCodeRemark = value; OnPropertyChanged(); }
        }

        Asset _assetCodeInfo;
        public Asset AssetCodeInfo
        {
            get { return _assetCodeInfo; }
            set { _assetCodeInfo = value; OnPropertyChanged(); }
        }

        bool _isStackVisible = false;
        public bool IsStackVisible
        {
            get { return _isStackVisible; }
            set { _isStackVisible = value; OnPropertyChanged(); }
        }

        bool _isVerificationQRCodeDescriptionError = false;
        public bool IsVerificationQRCodeDescriptionError
        {
            get { return _isVerificationQRCodeDescriptionError; }
            set { _isVerificationQRCodeDescriptionError = value; OnPropertyChanged(); }
        }

        bool _isVerificationAssetCodeError = false;
        public bool IsVerificationAssetCodeError
        {
            get { return _isVerificationAssetCodeError; }
            set { _isVerificationAssetCodeError = value; OnPropertyChanged(); }
        }

        bool _isVerificationAssetCodeRemarkError = false;
        public bool IsVerificationAssetCodeRemarkError
        {
            get { return _isVerificationAssetCodeRemarkError; }
            set { _isVerificationAssetCodeRemarkError = value; OnPropertyChanged(); }
        }

        bool _isVerificationAssetCodeErrorVisibleForGet = false;
        public bool IsVerificationAssetCodeErrorVisibleForGet
        {
            get { return _isVerificationAssetCodeErrorVisibleForGet; }
            set { _isVerificationAssetCodeErrorVisibleForGet = value; OnPropertyChanged(); }
        }

        bool _isVerifiedSuccess = false;
        public bool IsVerifiedSuccess
        {
            get { return _isVerifiedSuccess; }
            set { _isVerifiedSuccess = value; OnPropertyChanged(); }
        }

        private string _validationMessage;
        public string ValidationMessage
        {
            get => _validationMessage;
            set { _validationMessage = value; OnPropertyChanged(); }
        }

        private bool _isValidationVisible;
        public bool IsValidationVisible
        {
            get => _isValidationVisible;
            set { _isValidationVisible = value; OnPropertyChanged(); }
        }


        #endregion

        #region Commands
        public ICommand ScanCommand
        {
            get
            {
                return new RelayCommandWithArgsAsync<string>(async (arg) =>
                {
                    var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                    if (status != PermissionStatus.Granted)
                    {
                        await Permissions.RequestAsync<Permissions.Camera>();
                        var Camerastatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
                        if (Camerastatus == PermissionStatus.Granted)
                        {
                            await Navigation.NavigateToAsync<QRcodeScannerPageViewModel>();
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Alert", "You have not permission to access Camera.", "OK");
                            return;
                        }
                    }
                    else if (status == PermissionStatus.Granted)
                    {
                        await Navigation.NavigateToAsync<QRcodeScannerPageViewModel>();
                        return;
                    }
                }, this);
            }
        }

        public ICommand VerifyCommand
        {
            get
            {
                return new RelayCommandWithArgsAsync<string>(async (arg) =>
                {
                    await VerificationAssetByCode();
                }, this);
            }
        }

        public ICommand GetDetailsAssetCommand
        {
            get
            {
                return new RelayCommandWithArgsAsync<string>(async (arg) =>
                {
                    await SearchAssetByCode();
                }, this);
            }
        }
        #endregion

        public VerifyPageViewModel(INavigationService navigationService, CustomProxy customProxy, IAppUtility appUtility) : base(navigationService, customProxy, appUtility)
        {
            Title = "Verify";
            AddVerificationAssetValidation();
        }

        #region Methods
        public async override void Init(object args)
        {
            base.Init(args);
            if (args == null)
                return;

            // QR Scanner value
            if (args is string barcode)
            {
                var safeValue = barcode.Split(',')[0];
                VerificationQRCodeDescription.Value = barcode;
                VerificationAssetCode.Value = safeValue;
                return;
            }
            if (args is Asset asset)
            {
                AssetCodeInfo = asset;
                VerificationAssetCode.Value = asset.assetcode;
                return;
            }
            // if (args != null)
            // {
            //     //VerificationAssetCode.Value = args.ToString();
            //     AssetCodeInfo = args as Asset;
            //     VerificationAssetCode.Value = AssetCodeInfo.assetcode.ToString();
            // }
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();
            await BindData();
        }

        internal void AddVerificationAssetValidation()
        {
            try
            {
                _verificationQRCodeDescription.Validations.Add(new IsNotNullOrEmptyValidationRule<string> { ValidationMessage = "Verification QR Code Description is required" });
                _verificationAssetCode.Validations.Add(new IsNotNullOrEmptyValidationRule<string> { ValidationMessage = "Verification Asset Code is required" });
                _verificationAssetCodeRemark.Validations.Add(new IsNotNullOrEmptyValidationRule<string> { ValidationMessage = "Verification remark is required" });
            }
            catch (Exception ex)
            {

            }
        }

        public async Task BindData()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        public async Task VerificationAssetByCode()
        {
            try
            {
                IsStackVisible = false;
                IsVerifiedSuccess = false;
                if (string.IsNullOrEmpty(VerificationQRCodeDescription.Value) && string.IsNullOrEmpty(VerificationAssetCode.Value) && string.IsNullOrWhiteSpace(VerificationAssetCodeRemark.Value) && await HandleInternetConnection())
                {
                    IsVerificationQRCodeDescriptionError = true;
                    IsVerificationAssetCodeError = true;
                    IsVerificationAssetCodeRemarkError = true;
                    IsVerificationAssetCodeErrorVisibleForGet = false;
                    IsVerifiedSuccess = false;
                    IsStackVisible = false;
                    return;
                }
                else if (string.IsNullOrWhiteSpace(VerificationQRCodeDescription.Value))
                {
                    IsVerificationQRCodeDescriptionError = true;
                    IsVerificationAssetCodeError = false;
                    IsVerificationAssetCodeRemarkError = false;
                    IsVerificationAssetCodeErrorVisibleForGet = false;
                    IsVerifiedSuccess = false;
                    IsStackVisible = false;
                    return;
                }
                else if (string.IsNullOrWhiteSpace(VerificationAssetCode.Value))
                {
                    IsVerificationQRCodeDescriptionError = false;
                    IsVerificationAssetCodeError = true;
                    IsVerificationAssetCodeRemarkError = false;
                    IsVerificationAssetCodeErrorVisibleForGet = false;
                    IsVerifiedSuccess = false;
                    IsStackVisible = false;
                    return;
                }
                else if (string.IsNullOrWhiteSpace(VerificationAssetCodeRemark.Value))
                {
                    IsVerificationQRCodeDescriptionError = false;
                    IsVerificationAssetCodeError = false;
                    IsVerificationAssetCodeRemarkError = true;
                    IsVerificationAssetCodeErrorVisibleForGet = false;
                    IsVerifiedSuccess = false;
                    IsStackVisible = false;
                    return;
                }
                else
                {
                    IsVerifiedSuccess = false;
                    IsVerificationQRCodeDescriptionError = false;
                    IsVerificationAssetCodeError = false;
                    IsVerificationAssetCodeRemarkError = false;
                    IsVerificationAssetCodeErrorVisibleForGet = false;

                    IsVerifiedSuccess = false;

                    VerificationAssetRequestModel model = new VerificationAssetRequestModel();
                    model.assetcode = VerificationAssetCode.Value;
                    model.CompanyCode = Settings.CompanyCode; //"demo1";
                    model.qrcodedescription = VerificationQRCodeDescription.Value;
                    model.verificationremarks = VerificationAssetCodeRemark.Value;
                    model.verificationdoneby = Settings.UserName; //"admin";

                    UserDialogs.Instance.Loading("Loading....");
                    var VerificationAssetByCodeResult = await CustomProxy.VerificationAssetByCode(model);
                    if (VerificationQRCodeDescription.Value != null && VerificationAssetByCodeResult != null && VerificationAssetByCodeResult.Response != null && VerificationAssetByCodeResult.Response == true)
                    {
                        //await Navigation.ShowPopup<AlertPopupViewModel>(VerificationAssetByCodeResult.ResponseMessage);
                        //await Navigation.ShowPopup<AlertVerificationPopupViewModel>();
                        await SearchAssetByCode();
                        IsVerifiedSuccess = true;
                        ClearUserData();
                    }
                    else
                    {
                        IsVerifiedSuccess = false;
                        //IsVerificationAssetCodeErrorVisibleForGet = true;
                        ValidationMessage = "Invalid Asset Code ❌";
                        //await Navigation.ShowPopup<AlertPopupViewModel>(VerificationAssetByCodeResult.ResponseMessage);
                        await Navigation.ShowPopup<AlertPopupViewModel>("Invalid Asset Code");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideHud();
            }
        }

        public async Task SearchAssetByCode()
        {
            try
            {
                if (string.IsNullOrEmpty(VerificationAssetCode.Value) && string.IsNullOrWhiteSpace(VerificationAssetCode.Value))
                {
                    IsVerificationAssetCodeErrorVisibleForGet = true;
                    IsVerificationQRCodeDescriptionError = false;
                    IsVerificationAssetCodeError = false;
                    IsVerificationAssetCodeRemarkError = false;
                    IsVerifiedSuccess = false;
                    IsStackVisible = false;
                    return;
                }
                else
                {
                    IsVerificationAssetCodeErrorVisibleForGet = false;
                    IsVerificationQRCodeDescriptionError = false;
                    IsVerificationAssetCodeError = false;
                    IsVerificationAssetCodeRemarkError = false;
                    AssetRequestModel model = new AssetRequestModel();
                    model.assetcode = VerificationAssetCode.Value;
                    model.CompanyCode = Settings.CompanyCode; //"demo1";
                    model.qrcodedescription = VerificationQRCodeDescription.Value;
                    UserDialogs.Instance.Loading("Loading....");
                    var AssetByCodeResult = await CustomProxy.SearchAssetByCode(model);
                    if (AssetByCodeResult != null && AssetByCodeResult.Response != null)
                    {
                        AssetCodeInfo = AssetByCodeResult.Response;
                        IsStackVisible = true;
                        //ClearUserData();
                    }
                    else
                    {
                        IsVerifiedSuccess = false;
                        IsStackVisible = false;
                        await Navigation.ShowPopup<AlertPopupViewModel>(AssetByCodeResult.ResponseMessage);
                        //await Navigation.ShowPopup<AlertPopupViewModel>("Invalid Asset Code");
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                UserDialogs.Instance.HideHud();
            }
        }

        public async void HandleQRcodeScanner(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs obj)
        {
            try
            {
                // var scannedValue = obj.Results.FirstOrDefault().Value;
                // VerificationAssetCode.Value = scannedValue;

                AssetRequestModel model = new AssetRequestModel();
                model.qrcodedescription = obj.Results.FirstOrDefault().Value;
                model.assetcode = obj.Results.FirstOrDefault().Value;
                model.CompanyCode = Settings.CompanyCode; //"demo1";

                UserDialogs.Instance.Loading("Loading....");
                var AssetByCodeResult = await CustomProxy.SearchAssetByCode(model);
                if (AssetByCodeResult != null && AssetByCodeResult.Response != null)
                {
                    Asset AssetCodeDetail = new Asset();
                    AssetCodeDetail = AssetByCodeResult.Response;

                    IsStackVisible = true;

                    IsValidationVisible = true;
                    ValidationMessage = "Valid Asset Code";
                }
                else
                {
                    IsStackVisible = false;

                    IsValidationVisible = true;
                    ValidationMessage = "Invalid Asset Code";
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                UserDialogs.Instance.HideHud();
            }
        }

        internal void ClearUserData()
        {
            VerificationQRCodeDescription.Value = string.Empty;
            VerificationAssetCode.Value = string.Empty;
            VerificationAssetCodeRemark.Value = string.Empty;
        }

        internal Boolean Validate()
        {
            var verificationqrcodedescription = this.VerificationQRCodeDescription.Validate();
            var verificationassetcodeValidation = this.VerificationAssetCode.Validate();
            var verificationassetcoderemarkValidation = this.VerificationAssetCodeRemark.Validate();

            return verificationqrcodedescription && verificationassetcodeValidation && verificationassetcoderemarkValidation;
        }
        #endregion
    }
}