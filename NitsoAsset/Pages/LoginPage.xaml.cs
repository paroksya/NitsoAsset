using System;
using System.Collections.Generic;
using NitsoAsset.Pages.Base;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace NitsoAsset.Pages
{
    public partial class LoginPage : CustomPage
    {
        public LoginPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}