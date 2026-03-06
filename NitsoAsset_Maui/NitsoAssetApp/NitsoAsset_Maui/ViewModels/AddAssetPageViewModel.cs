using NitsoAsset_Maui.Assets.Helpers;
using NitsoAsset_Maui.Services.AppServices;
using NitsoAsset_Maui.Services.AppServices.Implementation;
using NitsoAsset_Maui.Services.Data;
using NitsoAsset_Maui.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NitsoAsset_Maui.ViewModels
{
    public class AddAssetPageViewModel : ViewModel
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

        public AddAssetPageViewModel(INavigationService navigationService, CustomProxy customProxy, IAppUtility appUtility) : base(navigationService, customProxy, appUtility)
        {
            Title = "Add Asset";
            _navigationService = navigationService;
            SetUpURL();
        }

        public void SetUpURL()
        {
            string BaseURL = "https://assetspecialist.in/Acquisition1/acquisitionIndex1";
            string cc = Settings.UserDetails.comp_code;
            int uid = Settings.UserDetails.user_pid; 
            string queryString = $"?cc={cc}&uid={uid}";
            Url = BaseURL + queryString;
            Console.WriteLine(Url);
        }
    }
}
