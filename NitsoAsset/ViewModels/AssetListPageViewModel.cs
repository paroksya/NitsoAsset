using System;
using NitsoAsset.Assets.Helpers;
using NitsoAsset.Services.AppServices;
using NitsoAsset.Services.Data;
using NitsoAsset.ViewModels.Base;

namespace NitsoAsset.ViewModels
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