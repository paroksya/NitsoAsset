using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using DCVXamarin;
using DLToolkit.Forms.Controls;
using NitsoAsset.Assets.Extensions;
using NitsoAsset.Assets.Helpers;
using NitsoAsset.Services.AppServices;
using NitsoAsset.Services.AppServices.Implementation;
using NitsoAsset.Services.Data;
using NitsoAsset.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NitsoAsset
{
    public partial class App : Application //, ILicenseVerificationListener
    {
        public static IDCVCameraEnhancer dce;
        public static IDCVBarcodeReader dbr;

        public static Autofac.IContainer Container { get; private set; }
        public static string ScanResult { get; set; }
        public static string ScanResultCode { get; set; }

        //public App(IDCVCameraEnhancer enhancer, IDCVBarcodeReader reader)
        //{
        //    InitializeComponent();
        //    dce = enhancer;
        //    dbr = reader;
        //    dbr.InitLicense("DLS2eyJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSJ9", this);
        //    //MainPage = new NavigationPage(new MainPage());
            
        //}

        //public void LicenseVerificationCallback(bool isSuccess, string errorMsg)
        //{
        //    if (!isSuccess)
        //    {
        //        System.Console.WriteLine(errorMsg);
        //    }
        //}

        public App()
        {
            SetupDependencyInjection();
            FlowListView.Init();
            InitializeComponent();

            Device.SetFlags(new[] {
                    "CarouselView_Experimental",
                    "IndicatorView_Experimental",
                    "SwipeView_Experimental",
                    "Brush_Experimental",
                "DragAndDrop_Experimental",
                "Expander_Experimental",
                "Markup_Experimental",
                "Shapes_Experimental",
                "AppTheme_Experimental"
                });
            Sharpnado.Shades.Initializer.Initialize(loggerEnable: false);

            App.Current.UserAppTheme = OSAppTheme.Light;

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            //Task.Run(async () =>
            //{
            //    await SetStartPage();
            //});
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