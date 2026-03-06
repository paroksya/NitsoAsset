using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using NitsoAsset_Maui.ViewModels;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Controls.UserDialogs.Maui;

namespace NitsoAsset_Maui
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private LoginPageViewModel _viewModel;
        public MainPage()
        {
            InitializeComponent();
            _viewModel = App.Container.Resolve<LoginPageViewModel>();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Dispatcher.Dispatch(async () =>
            {
                await Task.Delay(100);

                //Task.Run(async () =>
                //{
                //    if (_viewModel != null)
                //    {
                //        UserDialogs.Instance.Loading("Loading....");
                //        {
                //            await _viewModel.SetStartPage();
                //        }
                //    }
                //});
            });
        }
    }
}