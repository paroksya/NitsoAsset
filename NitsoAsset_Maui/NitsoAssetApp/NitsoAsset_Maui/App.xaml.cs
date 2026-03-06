using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using NitsoAsset_Maui.Assets.Extensions;
using NitsoAsset_Maui.Assets.Helpers;
using NitsoAsset_Maui.Services.AppServices;
using NitsoAsset_Maui.Services.AppServices.Implementation;
using NitsoAsset_Maui.Services.Data;
using NitsoAsset_Maui.ViewModels;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;

namespace NitsoAsset_Maui
{
    public partial class App : Application
    {
        public static Autofac.IContainer Container { get; private set; }
        public static string ScanResult { get; set; }
        public static string ScanResultCode { get; set; }

        public App()
        {
            SetupDependencyInjection();
            InitializeComponent();

            App.Current.UserAppTheme = AppTheme.Light;
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            Task.Run(async () =>
            {
                await SetStartPage();
            });
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private void SetupDependencyInjection()
        {
            if (Container != null)
                return;

            var builder = new ContainerBuilder();
            builder.RegisterMvvmComponents(typeof(App).GetTypeInfo().Assembly);

            builder.RegisterType<NavigationService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<AppUtility>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CustomProxy>().SingleInstance();

            var mobileAssembly = typeof(App).GetTypeInfo().Assembly;

            builder.RegisterRepositories(mobileAssembly);
            //builder.RegisterPushHandlers(mobileAssembly);
            //builder.RegisterDataCachers(mobileAssembly);

            try
            {
                Container = builder.Build();
            }
            catch (Exception ex)
            {
                Container = builder.Build();
            }
        }

        public async Task SetStartPage()
        {
            var navigationService = Container.Resolve<INavigationService>();
            if (navigationService != null)
            {
                if (!string.IsNullOrEmpty(Settings.CompanyCode) && !string.IsNullOrEmpty(Settings.UserName)
                    && !string.IsNullOrEmpty(Settings.Password))
                {
                    IDictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("HandlebgLogin", true);

                    navigationService.SetMainViewModel<DashBoardPageViewModel>(dict);
                }
                else
                {
                    navigationService.SetMainViewModel<LoginPageViewModel>();
                }
            }
        }
    }
}