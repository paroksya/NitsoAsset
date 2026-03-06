using System;
using System.Collections.Generic;
using NitsoAsset_Maui.ViewModels.Popups;
using Mopups;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Mopups.Pages;
namespace NitsoAsset_Maui.Pages.Popups
{
    public partial class AlertLogoutPopup : PopupPage
    {
        AlertLogoutPopupViewModel Context;
        public AlertLogoutPopup()
        {
            InitializeComponent();
        }
    }
}