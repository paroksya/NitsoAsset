using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using NitsoAsset.Assets.Helpers;
using NitsoAsset.Assets.Validations;
using NitsoAsset.Models;
using NitsoAsset.Services.AppServices;
using NitsoAsset.Services.Data;
using NitsoAsset.ViewModels.Base;
using NitsoAsset.ViewModels.Popups;
using Xamarin.Forms;

namespace NitsoAsset.ViewModels
{
    public class SearchAssetCodePageViewModel : ViewModel
    {
        #region Properties
        private INavigationService _navigationService { get; }
        CustomProxy _customProxy { get; }
        private IAppUtility _utilityService { get; }

        public Page MenuPage { get; set; }

        ValidatableObject<string> _searchAssetCode = new ValidatableObject<string>();
        public ValidatableObject<string> SearchAssetCode
        {
            get { return _searchAssetCode; }
            set { _searchAssetCode = value; OnPropertyChanged(); }
        }

        public Page SearchAssetCodePage { get; set; }

        Asset _assetCodeDetail;
        public Asset AssetCodeDetail
        {
            get { return _assetCodeDetail; }
            set { _assetCodeDetail = value; OnPropertyChanged(); }
        }

        #endregion

        #region Commands
        public ICommand SearchAssetCommand
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

        public SearchAssetCodePageViewModel(INavigationService navigationService, CustomProxy customProxy, IAppUtility appUtility) : base(navigationService, customProxy, appUtility)
        {
            Title = "Search Asset Code";
            _navigationService = navigationService;
            _customProxy = customProxy;
            _utilityService = appUtility;
            AddAssetValidation();
        }

        #region Methods
        public async override void Init(object args)
        {
            base.Init(args);
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();
            await BindData();
        }

        internal void AddAssetValidation()
        {
            try
            {
                _searchAssetCode.Validations.Add(new IsNotNullOrEmptyValidationRule<string> { ValidationMessage = "Asset Code is required" });
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

        public async Task SearchAssetByCode()
        {
            if (Validate() && await HandleInternetConnection()) 
            {
                AssetRequestModel model = new AssetRequestModel();
                model.assetcode = SearchAssetCode.Value;
                model.CompanyCode = Settings.CompanyCode; //"demo1";

                using (UserDialogs.Instance.Loading("Loading...."))
                {
                    var AssetByCodeResult = await CustomProxy.SearchAssetByCode(model);
                    if (AssetByCodeResult != null && AssetByCodeResult.Response != null)
                    {
                        AssetCodeDetail = AssetByCodeResult.Response;
                        await Navigation.NavigateToAsync<ScannerDetailsPageViewModel>(AssetCodeDetail);
                    }
                    else
                    {
                        await Navigation.ShowPopup<AlertPopupViewModel>(AssetByCodeResult.ResponseMessage);
                        //await Navigation.ShowPopup<AlertPopupViewModel>("Invalid Asset Code");
                    }
                }
            }
        }

        internal Boolean Validate()
        {
            var assetcodeValidation = this.SearchAssetCode.Validate();

            return assetcodeValidation ;
        }
        #endregion
    }
}