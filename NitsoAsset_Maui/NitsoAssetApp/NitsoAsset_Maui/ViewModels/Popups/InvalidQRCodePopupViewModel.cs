using System;
using System.Windows.Input;
using NitsoAsset_Maui.Services.AppServices;
using NitsoAsset_Maui.Services.Data;
using NitsoAsset_Maui.ViewModels.Base;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace NitsoAsset_Maui.ViewModels.Popups
{
    public class InvalidQRCodePopupViewModel : ViewModel
    {
        #region Properties
        private INavigationService _navigationService { get; }
        CustomProxy _customProxy { get; }
        private IAppUtility _utilityService { get; }


        string _confirmMessage;
        public string ConfirmMessage
        {
            get
            {
                return _confirmMessage;
            }
            set
            {
                _confirmMessage = value;
                OnPropertyChanged();
            }
        }

        string _confirmTitle;
        public string ConfirmTitle
        {
            get
            {
                return _confirmTitle;
            }
            set
            {
                _confirmTitle = value;
                OnPropertyChanged();
            }
        }

        string _ConfirmBtnText = "CONTINUE";
        public string ConfirmBtnText
        {
            get
            {
                return _ConfirmBtnText;
            }
            set
            {
                _ConfirmBtnText = value;
                OnPropertyChanged("ConfirmBtnText");
            }
        }

        string _CancelBtnText = "CANCEL";
        public string CancelBtnText
        {
            get
            {
                return _CancelBtnText;
            }
            set
            {
                _CancelBtnText = value;
                OnPropertyChanged("CancelBtnText");
            }
        }

        object _confirmArgs = null;
        public object ConfirmArgs
        {
            get
            {
                return _confirmArgs;
            }
            set
            {
                _confirmArgs = value;
                OnPropertyChanged();
            }
        }

        public ICommand HandleClosePopupCommand { get; set; }

        #endregion

        #region Commands
        public ICommand CloseCommand
        {
            get
            {
                return new RelayCommandWithArgsAsync<object>(async (arg) =>
                {
                    await Navigation.ClosePopup();
                    await Navigation.PopAsync();
                    //if (arg != null)
                    //{
                    //    ConfirmArgs = arg;
                    //}
                    //HandleClosePopupCommand?.Execute(ConfirmArgs);

                }, this);
            }
        }

        public ICommand ConfirmCommand
        {
            get
            {
                return new RelayCommandWithArgsAsync<object>(async (arg) =>
                {
                    await Navigation.ClosePopup();

                    if (arg != null)
                    {
                        ConfirmArgs = arg;
                    }
                    HandleClosePopupCommand?.Execute(ConfirmArgs);
                }, this);
            }
        }
        #endregion

        public InvalidQRCodePopupViewModel(INavigationService navigationService, CustomProxy customProxy, IAppUtility appUtility) : base(navigationService)
        {
            _customProxy = customProxy;
            _navigationService = navigationService;
            _utilityService = appUtility;
            ShowBusyLoader = true;
        }

        #region Methods
        public override void Init(object args)
        {
            base.Init(args);
            if (args is Dictionary<string, dynamic> info)
            {
                ConfirmTitle = info.ContainsKey("ConfirmTitle") ? info["ConfirmTitle"] : string.Empty;
                ConfirmMessage = info.ContainsKey("ConfirmMessage") ? info["ConfirmMessage"] : string.Empty;
                CancelBtnText = info.ContainsKey("CancelBtnText") ? info["CancelBtnText"] : "CANCEL"; // Default value
                ConfirmBtnText = info.ContainsKey("ConfirmBtnText") ? info["ConfirmBtnText"] : "YES"; // Default value
                ConfirmArgs = info.ContainsKey("ConfirmArgs") ? info["ConfirmArgs"] : null; // Handle as needed
                HandleClosePopupCommand = info.ContainsKey("ConfirmCommand") ? info["ConfirmCommand"] as ICommand : null;
            }
        }
        #endregion
    }
}