using System;
using System.Collections.Generic;
using NitsoAsset.Pages.Base;
using Xamarin.Forms;

namespace NitsoAsset.Pages
{
    public partial class DashBoardPage : CustomPage
    {
        public double SWidth { get; set; }

        public DashBoardPage()
        {
            InitializeComponent();
            SWidth = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width / Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density;
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