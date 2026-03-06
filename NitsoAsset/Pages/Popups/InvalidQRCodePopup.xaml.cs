using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace NitsoAsset.Pages.Popups
{
    public partial class InvalidQRCodePopup : PopupPage
    {
        public InvalidQRCodePopup()
        {
            InitializeComponent();
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }
    }
}