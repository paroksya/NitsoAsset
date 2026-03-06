using System;
using NitsoAsset_Maui.Assets.Helpers;
using NitsoAsset_Maui.Services.AppServices;
using NitsoAsset_Maui.Services.Data;
using NitsoAsset_Maui.ViewModels.Base;

namespace NitsoAsset_Maui.ViewModels
{
	public class AssetListPageViewModel : ViewModel
    {
        #region Properties
        private INavigationService _navigationService { get; }

        private string _url;
        public string Url
        {
            get => _url;
            set
            {
                if (_url != value)
                {
                    _url = value;
                    OnPropertyChanged("Url");
                }
            }
        }
        #endregion

        public AssetListPageViewModel(INavigationService navigationService, CustomProxy customProxy, IAppUtility appUtility) : base(navigationService, customProxy, appUtility)
        {
            Title = "Asset List";
            _navigationService = navigationService;
            SetUpURLForAssetList();
        }

        public void SetUpURLForAssetList()
        {
            string BaseURL = "https://assetspecialist.in/Acquisition1/GetAcquisition";
            string cc = Settings.UserDetails.comp_code;
            int uid = Settings.UserDetails.user_pid;
            string queryString = $"?cc={cc}&uid={uid}";
            Url = BaseURL + queryString;
            Console.WriteLine(Url);
        }
    }
}