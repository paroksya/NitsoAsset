using System;
using System.Collections.Generic;
using NitsoAsset_Maui.Pages.Base;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace NitsoAsset_Maui.Pages
{
    public partial class AssetListPage : CustomPage
    {
        public AssetListPage()
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