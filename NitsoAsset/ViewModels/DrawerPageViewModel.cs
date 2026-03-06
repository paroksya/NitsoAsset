using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using NitsoAsset.Assets.Helpers;
using NitsoAsset.Models.Enums;
using NitsoAsset.Models.Static;
using NitsoAsset.Services.AppServices;
using NitsoAsset.ViewModels.Base;
using NitsoAsset.ViewModels.Popups;
using Xamarin.Forms;

namespace NitsoAsset.ViewModels
{
    public class DrawerPageViewModel : ViewModel
    {
        #region Properties
        ObservableCollection<DrawerPageItem> _drawerdatalist = new ObservableCollection<DrawerPageItem>();
        public ObservableCollection<DrawerPageItem> Drawerdatalist
        {
            get { return _drawerdatalist; }
            set { _drawerdatalist = value; OnPropertyChanged(); }
        }
        #endregion

        #region Commands
        public ICommand ListItemTapCommand
        {
            get
            {
                return new RelayCommandWithArgsAsync<DrawerPageItem>(async (arg) =>
                {
                    if (arg == null) return;
                    Navigation.CloseDrawerMenu();

                    if (arg.NavigationType == SideNavigationEnums.LOGOUT)
                    {
                        Logout();
                    }
                    else if (arg.NavigationType == SideNavigationEnums.MY_PROFILE)
                    {
                        //await Navigation.NavigateToAsync<MyProfilePageViewModel>();
                    }
                    //else if (arg.NavigationType == SideNavigationEnums.SCAN)
                    //{
                    //    await Navigation.NavigateToAsync<QRcodeScannerPageViewModel>();
                    //}
                    else if (arg.NavigationType == SideNavigationEnums.ASSETCODE)
                    {
                        await Navigation.NavigateToAsync<SearchAssetCodePageViewModel>();
                    }
                    else if (arg.NavigationType == SideNavigationEnums.SEARCH)
                    {
                        await Navigation.NavigateToAsync<SearchPageViewModel>();
                    }
                    else if (arg.NavigationType == SideNavigationEnums.VERIFY)
                    {
                        await Navigation.NavigateToAsync<VerifyPageViewModel>();
                    }
                }, this);
            }
        }
        #endregion
        public DrawerPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "menu";
            Icon = "menu";
        }
        #region PageMethods

        public override void Init(object args)
        {
            base.Init(args);
            bindData();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
        }
        #endregion
        #region Methods

        private void bindData()
        {
            var drawertempData = new ObservableCollection<DrawerPageItem>();

            drawertempData.Add(new DrawerPageItem()
            {
                drawerpage_gridlabel = "MY PROFILE",
                drawerpage_gridimage = "menu_birthday",
                NavigationType = SideNavigationEnums.MY_PROFILE,
                Grid_ItemTapCommand = ListItemTapCommand
            });

            //drawertempData.Add(new DrawerPageItem()
            //{
            //    drawerpage_gridlabel = "SCAN",
            //    drawerpage_gridimage = "qr_code_icon",
            //    NavigationType = SideNavigationEnums.SCAN,
            //    Grid_ItemTapCommand = ListItemTapCommand
            //});

            drawertempData.Add(new DrawerPageItem()
            {
                drawerpage_gridlabel = "ASSET CODE",
                drawerpage_gridimage = "back",
                NavigationType = SideNavigationEnums.ASSETCODE,
                Grid_ItemTapCommand = ListItemTapCommand
            });

            drawertempData.Add(new DrawerPageItem()
            {
                drawerpage_gridlabel = "SEARCH",
                drawerpage_gridimage = "back",
                NavigationType = SideNavigationEnums.SEARCH,
                Grid_ItemTapCommand = ListItemTapCommand
            });

            drawertempData.Add(new DrawerPageItem()
            {
                drawerpage_gridlabel = "VERIFY",
                drawerpage_gridimage = "back",
                NavigationType = SideNavigationEnums.VERIFY,
                Grid_ItemTapCommand = ListItemTapCommand
            });

            drawertempData.Add(new DrawerPageItem()
            {
                drawerpage_gridlabel = "LOGOUT",
                drawerpage_gridimage = "back",
                NavigationType = SideNavigationEnums.LOGOUT,
                Grid_ItemTapCommand = ListItemTapCommand
            });

            Drawerdatalist = new ObservableCollection<DrawerPageItem>(drawertempData);
        }

        internal void ClearUserData()
        {
            Settings.UserName = string.Empty;
            Settings.Password = string.Empty;
            Settings.CompanyCode = string.Empty;
            Settings.UserDetails = null;
            //Settings.PunchAttendaceWithLocation = false;
            Settings.ServiceToken = null;
            Settings.ServiceBaseURL = null;
        }

        public void Logout()
        {
            Navigation.ShowPopup<AlertLogoutPopupViewModel>();
            ClearUserData();
            //Navigation.SetMainViewModel<LoginPageViewModel>();
        }
        #endregion
    }
}