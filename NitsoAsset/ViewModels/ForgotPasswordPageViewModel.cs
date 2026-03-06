using System;
using System.Threading.Tasks;
using System.Windows.Input;
using NitsoAsset.Assets.Validations;
using NitsoAsset.Services.AppServices;
using NitsoAsset.Services.Data;
using NitsoAsset.ViewModels.Base;

namespace NitsoAsset.ViewModels
{
    public class ForgotPasswordPageViewModel : ViewModel
    {
        #region Properties
        private INavigationService _navigationService { get; }
        CustomProxy _customProxy { get; }

        ValidatableObject<string> _clientId = new ValidatableObject<string>();
        public ValidatableObject<string> ClientId
        {
            get { return _clientId; }
            set { _clientId = value; OnPropertyChanged(); }
        }
        ValidatableObject<string> _userId = new ValidatableObject<string>();
        public ValidatableObject<string> UserId
        {
            get { return _userId; }
            set { _userId = value; OnPropertyChanged(); }
        }

        DateTime? _dateOfBirth;
        public DateTime? DateOfBirth
        {
            get
            {
                return _dateOfBirth;
            }
            set
            {
                _dateOfBirth = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands
        public ICommand SendCommand
        {
            get
            {
                return new RelayCommandWithArgsAsync<object>(async (arg) =>
                {
                    Send();
                }, this);
            }
        }
        public ICommand BackToLoginCommand
        {
            get
            {
                return new RelayCommandWithArgsAsync<object>(async (arg) =>
                {

                    await Navigation.PopAsync();
                }, this);
            }
        }

        #endregion

        public ForgotPasswordPageViewModel(INavigationService navigationService, CustomProxy customProxy, IAppUtility appUtility) : base(navigationService, customProxy, appUtility)
        {
            AddForgotPasswordValidation();
        }

        internal void AddForgotPasswordValidation()
        {
            try
            {
                _clientId.Validations.Add(new IsNotNullOrEmptyValidationRule<string> { ValidationMessage = "Company Id is required" });
                _userId.Validations.Add(new IsNotNullOrEmptyValidationRule<string> { ValidationMessage = "User Id is required" });
            }
            catch (Exception ex)
            {

            }
        }

        internal Boolean Validate()
        {
            var clientIdValidation = this.ClientId.Validate();
            var userIdValidation = this.UserId.Validate();

            return clientIdValidation && userIdValidation;
        }

        public async Task Send()
        {
            if (Validate() && await HandleInternetConnection())
            {
                //    ForgotPasswordRequest forgotPassword = new ForgotPasswordRequest();
                //    forgotPassword.CompanyCode = ClientId.Value;
                //    forgotPassword.UserName = UserId.Value;
                //    forgotPassword.DOB = DateOfBirth.Value;
                //    using (UserDialogs.Instance.Loading("Loading...."))
                //    {

                //        var ForgotPwdResult = await CustomProxy.ForgotPassword(forgotPassword);
                //        if (ForgotPwdResult != null && ForgotPwdResult.Result && ForgotPwdResult.Data != null)
                //        {
                //            await Navigation.ShowPopup<AlertPopupViewModel>("Password has been sent to your email Id.");
                //            await Navigation.PopToRootAsync();
                //        }
                //        else
                //        {
                //            await Navigation.ShowPopup<AlertPopupViewModel>("Invalid details");
                //        }
                //    }
                }
            }
    }
}