using System;
using System.Diagnostics;
using System.Windows.Input;
using NitsoAsset.Services.AppServices;
using NitsoAsset.ViewModels.Base;
namespace NitsoAsset.ViewModels.Popups
{
    public class AlertPopupViewModel : ViewModel
    {
        #region Properties
        private string _alertMessage;
        public string AlertMessage
        {
            get
            {
                return _alertMessage;
            }
            set
            {
                _alertMessage = value;
                OnPropertyChanged();
            }
        }

        private string _oKText = "Ok";
        public string OKText
        {
            get
            {
                return _oKText;
            }
            set
            {
                _oKText = value;
                OnPropertyChanged("OKText");
            }
        }
        #endregion

        #region Command

        public ICommand CloseCommand
        {
            get
            {
                return new RelayCommandWithArgsAsync<object>(async (arg) =>
                {


                    try
                    {
                        if (ClosePopupEvent == null)
                        {
                            await Navigation.ClosePopup();
                        }
                        else
                        {
                            await Navigation.ClosePopup();
                            ClosePopupEvent?.Invoke(this, true);
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                }, this);
            }
        }

        public EventHandler<bool> ClosePopupEvent;

        #endregion

        public AlertPopupViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override void Init(object args)
        {
            base.Init(args);
            var msg = args as string;
            AlertMessage = msg;
        }
    }
}