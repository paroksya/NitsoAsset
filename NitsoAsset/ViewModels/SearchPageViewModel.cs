using System;
using System.Threading.Tasks;
using System.Windows.Input;
using NitsoAsset.Services.AppServices;
using NitsoAsset.Services.Data;
using NitsoAsset.ViewModels.Base;
using Xamarin.Forms;

namespace NitsoAsset.ViewModels
{
    public class SearchPageViewModel : ViewModel
    {
        #region Properties
        private INavigationService _navigationService { get; }
        CustomProxy _customProxy { get; }

        public Page MenuPage { get; set; }

        string _searchAssetCode;
        public string SearchAssetCode
        {
            get { return _searchAssetCode; }
            set { _searchAssetCode = value; OnPropertyChanged(); }
        }
        public Page SearchPage { get; set; }

        #endregion

        #region Commands
        public ICommand SearchCommand
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

        public SearchPageViewModel(INavigationService navigationService, CustomProxy customProxy, IAppUtility appUtility) : base(navigationService, customProxy, appUtility)
        {
            Title = "Search";
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

        public async Task BindData()
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