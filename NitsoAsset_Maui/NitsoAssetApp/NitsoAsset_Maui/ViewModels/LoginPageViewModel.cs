using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Controls.UserDialogs.Maui;
using NitsoAsset_Maui.Assets.Helpers;
using NitsoAsset_Maui.Assets.Validations;
using NitsoAsset_Maui.Services.AppServices;
using NitsoAsset_Maui.Services.Data;
using NitsoAsset_Maui.ViewModels.Base;
using NitsoAsset_Maui.ViewModels.Popups;
using Newtonsoft.Json;
using NitsoAsset_Maui.Models;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;

namespace NitsoAsset_Maui.ViewModels
{
    public class LoginPageViewModel : ViewModel
    {
        #region Properties
        private INavigationService _navigationService { get; }
        CustomProxy _customProxy { get; }
        public Page LoginPage { get; set; }

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
        ValidatableObject<string> _password = new ValidatableObject<string>();
        public ValidatableObject<string> Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        public bool HandlebgLogin { get; set; }
        #endregion

        #region Commands
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommandWithArgsAsync<string>(async (arg) =>
                {
                    await Login();
                }, this);
            }
        }

        public ICommand ForgotPasswordCommand
        {
            get
            {
                return new RelayCommandWithArgsAsync<string>(async (arg) =>
                {
                    await Navigation.NavigateToAsync<ForgotPasswordPageViewModel>();
                }, this);
            }
        }
        public ICommand NavigateToSiteCommand
        {
            get
            {
                return new RelayCommandWithArgsAsync<string>(async (arg) =>
                {
                    await Microsoft.Maui.ApplicationModel.Launcher.OpenAsync("https://www.nitsotech.com/");
                }, this);
            }
        }
        #endregion

        public LoginPageViewModel(INavigationService navigationService, CustomProxy customProxy, IAppUtility appUtility) : base(navigationService, customProxy, appUtility)
        {
            AddLoginValidation();
            customProxy = _customProxy;
#if DEBUG
            ClientId.Value = "demo1";
            UserId.Value = "admin";
            Password.Value = "Admin@1122";

            //ClientId.Value = "muwci";
            //UserId.Value = "admin";
            //Password.Value = "Admin@1122";
#endif
        }

        #region Methods
        public async override void Init(object args)
        {
            base.Init(args);
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();
        }

        internal void AddLoginValidation()
        {
            try
            {
                _clientId.Validations.Add(new IsNotNullOrEmptyValidationRule<string> { ValidationMessage = "Client Id is required" });
                _userId.Validations.Add(new IsNotNullOrEmptyValidationRule<string> { ValidationMessage = "User Id is required" });
                _password.Validations.Add(new IsNotNullOrEmptyValidationRule<string>() { ValidationMessage = "Password is required" });
            }
            catch (Exception ex)
            {

            }
        }

        public async Task Login()
        {
            try
            {
                Settings.ServiceBaseURL = null;
                if (Validate() && await HandleInternetConnection())
                {
                    if (ClientId.Value == "muwci")
                    {
                        Settings.ServiceBaseURL = null;
                        Settings.ServiceBaseURL = "https://apifams.muwci.net";
                    }
                    else
                    {
                        Settings.ServiceBaseURL = null;
                        Settings.ServiceBaseURL = "http://api-asset.assetspecialist.in";
                    }

                    LoginModel model = new LoginModel();
                    model.CompanyCode = ClientId.Value;
                    model.UserName = UserId.Value;
                    model.Password = Password.Value;

                    UserDialogs.Instance.Loading("Loading....");
                    // Do Login
                    var UserLoginResult = await CustomProxy.UserLogin(model);
                    if (UserLoginResult != null && UserLoginResult.Response != null)
                    {
                        Settings.UserDetails = UserLoginResult.Response;
                        Settings.CompanyCode = UserLoginResult.Response.comp_code; // model.CompanyCode; //ClientId.Value; //UserLoginResult.Response.CompanyFid;
                        Settings.UserName = UserLoginResult.Response.username;
                        Settings.Password = Password.Value;
                        Settings.CompanyName = UserLoginResult.Response.Compname;
                        Settings.SavedDetails = JsonConvert.SerializeObject(UserLoginResult.Response);
                        Settings.ServiceToken = $"{UserLoginResult.TokenType} {UserLoginResult.AccessToken}";
                        Navigation.SetMainViewModel<DashBoardPageViewModel>();
                    }
                    else
                    {
                        await Navigation.ShowPopup<AlertPopupViewModel>("Invalid credentials");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                UserDialogs.Instance.HideHud();
            }
        }

        internal Boolean Validate()
        {
            var clientIdValidation = this.ClientId.Validate();
            var userIdValidation = this.UserId.Validate();
            var passwordValidation = this.Password.Validate();

            return clientIdValidation && userIdValidation && passwordValidation;
        }

        public async Task<bool> bgLogin()
        {
            try
            {
                LoginModel model = new LoginModel();
                model.CompanyCode = Settings.CompanyCode;
                model.UserName = Settings.UserName;
                model.Password = Settings.Password;

                UserDialogs.Instance.Loading("Loading....");
                var UserLoginResult = await CustomProxy.UserLogin(model);
                if (UserLoginResult != null && UserLoginResult.Response != null)
                {
                    Settings.UserDetails = UserLoginResult.Response;
                    //Settings.CompanyCode = UserLoginResult.Data.CompanyCode;
                    //Settings.UserName = UserLoginResult.Data.UserName;
                    //Settings.Password = Password.Value;
                    //Settings.PunchAttendaceWithLocation = EmployeeLoginResult.Data.punchAtten_withloc;
                    Settings.SavedDetails = JsonConvert.SerializeObject(UserLoginResult.Response);
                    Settings.ServiceToken = $"{UserLoginResult.TokenType} {UserLoginResult.AccessToken}";
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                UserDialogs.Instance.HideHud();
            }
        }


        public async Task SetStartPage()
        {
            if (Navigation != null)
            {
                if (!string.IsNullOrEmpty(Settings.CompanyCode) && !string.IsNullOrEmpty(Settings.UserName) && !string.IsNullOrEmpty(Settings.Password))
                {
                    var res = await bgLogin();

                    if (res)
                    {
                        Navigation.SetMainViewModel<DashBoardPageViewModel>();
                    }
                    else
                    {
                        Navigation.SetMainViewModel<LoginPageViewModel>();
                    }
                    //IDictionary<string, object> dict = new Dictionary<string, object>();
                    //dict.Add("HandlebgLogin", true);

                    //Navigation.SetMainViewModel<DashboardPageViewModel>(dict);
                }
                else
                {
                    Navigation.SetMainViewModel<LoginPageViewModel>();
                }
            }
        }
        #endregion
    }
}