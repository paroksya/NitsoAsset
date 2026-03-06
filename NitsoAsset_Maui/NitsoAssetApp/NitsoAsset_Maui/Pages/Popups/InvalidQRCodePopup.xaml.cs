using System;
using System.Collections.Generic;
using Mopups;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Mopups.Pages;

namespace NitsoAsset_Maui.Pages.Popups
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