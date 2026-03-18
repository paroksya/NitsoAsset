using NitsoAsset_Maui.Pages.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using NitsoAsset_Maui.Pages.Base;

namespace NitsoAsset_Maui.Pages
{
    public partial class AddAssetPage : CustomPage
    {
        public AddAssetPage()
        {
            InitializeComponent();

            webView.Navigating += WebView_Navigating;
            webView.Navigated += WebView_Navigated;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void WebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            // Show loader
            ErrorView.IsVisible = false;
            webView.IsVisible = false;
        }

        private void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            if (e.Result != WebNavigationResult.Success)
            {
                // Show error
                ErrorView.IsVisible = true;
                webView.IsVisible = false;
            }
            else
            {
                // Show webview
                ErrorView.IsVisible = false;
                webView.IsVisible = true;
            }
        }

        private void OnRetryClicked(object sender, EventArgs e)
        {
            // Reload WebView
            webView.Reload();
        }
    }
}