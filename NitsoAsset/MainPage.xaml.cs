using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Autofac;
using NitsoAsset.ViewModels;
using Xamarin.Forms;

namespace NitsoAsset
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var _viewModel = App.Container.Resolve<LoginPageViewModel>();
            BindingContext = _viewModel;
            Task.Run(async () =>
            {
                if (_viewModel != null)
                {
                    using (UserDialogs.Instance.Loading("Loading...."))
                    {
                        await _viewModel.SetStartPage();
                    }
                }
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
    }
}