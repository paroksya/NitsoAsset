using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Devices;
using NitsoAsset_Maui.Pages.Base;

namespace NitsoAsset_Maui.Pages
{
    public partial class DashBoardPage : CustomPage
    {
        public double SWidth { get; set; }

        public DashBoardPage()
        {
            InitializeComponent();
            SWidth = Microsoft.Maui.Devices.DeviceDisplay.MainDisplayInfo.Width / Microsoft.Maui.Devices.DeviceDisplay.MainDisplayInfo.Density;
            var totalPadding = Device.Idiom == TargetIdiom.Phone ? 120 : 258;
            SWidth = SWidth - totalPadding;

            var hw = Device.Idiom == TargetIdiom.Phone ? SWidth / 3 : SWidth / 4;
            double imgHeightWidth = hw;


            var hwPadding = Device.Idiom == TargetIdiom.Phone ? 10 : 5;
            //App.Current.Resources["DashboardColumnSize"] = imgHeightWidth;
            //App.Current.Resources["DashboardColumnHeight"] = imgHeightWidth - hwPadding;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}