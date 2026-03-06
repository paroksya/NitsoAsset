using System;
using System.Collections.Generic;
using NitsoAsset_Maui.Pages.Base;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace NitsoAsset_Maui.Pages
{
    public partial class LoginPage : CustomPage
    {
        public LoginPage()
        {
            InitializeComponent();
            On<Microsoft.Maui.Controls.PlatformConfiguration.iOS>().SetUseSafeArea(false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}