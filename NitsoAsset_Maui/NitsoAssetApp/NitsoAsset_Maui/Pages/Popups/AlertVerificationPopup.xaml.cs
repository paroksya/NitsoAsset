using System;
using System.Collections.Generic;
using NitsoAsset_Maui.ViewModels.Popups;
using Mopups.Pages;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace NitsoAsset_Maui.Pages.Popups
{
    public partial class AlertVerificationPopup : PopupPage
    {
        AlertVerificationPopupViewModel Context;
        public AlertVerificationPopup()
        {
            InitializeComponent();
        }
    }
}