using System;
using System.Collections.Generic;
using NitsoAsset_Maui.Pages.Base;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace NitsoAsset_Maui.Pages
{
    public partial class VerifyPage : CustomPage
    {
        public VerifyPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        //private void Button_Clicked(object sender, EventArgs e)
        //{
        //    Navigation.PushAsync(new QRcodeScannerPage());
        //}
    }
}