using Microsoft.Maui;
using Microsoft.Maui.Controls;
//using ZXing.Net.Mobile.Forms;
using Microsoft.Maui.Controls.Compatibility;
using Newtonsoft.Json.Linq;
using Controls.UserDialogs.Maui;
using NitsoAsset_Maui.Pages.Base;
using NitsoAsset_Maui.Services;
using NitsoAsset_Maui.ViewModels;
using System;
using System.Collections.Generic;
using ZXing;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
using ZXing.Net.Maui.Readers;
using ZXing.QrCode.Internal;

namespace NitsoAsset_Maui.Pages
{
    public partial class QRcodeScannerPage : CustomPage //, IBarcodeResultListener
    {
        QRcodeScannerPageViewModel Context;
        ZXingBarcodeReader overlay;

        string lblResult;
        string lblCode;
        string previousdata;

        public QRcodeScannerPage()
        {
            InitializeComponent();
            lblResult = null;
            barcodeView.Options = new BarcodeReaderOptions
            {
                Formats = BarcodeFormats.All,
                AutoRotate = true,
                Multiple = true
            };
            //App.dbr.AddResultListener(this);
            //App.dbr.UpdateRuntimeSettings(EnumDBRPresetTemplate.VIDEO_SINGLE_BARCODE);
            //App.dbr.SetCameraEnhancer(App.dce);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Context = ((QRcodeScannerPageViewModel)this.BindingContext);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var animation = new Animation(v => line.TranslationY = v, 0, 240);
                animation.Commit(this, "SimpleAnimation", 16, 2500, Easing.Linear, (v, c) => line.TranslationY = 240, () => true);
            });
            //App.dbr.StartScanning();
            //App.dce.Open();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Context = ((QRcodeScannerPageViewModel)this.BindingContext);
            //App.dbr.StopScanning();
            //App.dce.Close();
        }

        protected async void ZXingScannerView_OnScanResult(object sender, BarcodeDetectionEventArgs e)
        {
            foreach (var barcode in e.Results)
                Console.WriteLine($"Barcodes: {barcode.Format} -> {barcode.Value}");

            var first = e.Results?.FirstOrDefault();
            if (first is not null)
            {
                Dispatcher.Dispatch(() =>
                {
                    barcodeView.ClearValue(BarcodeGeneratorView.ValueProperty);
                    //barcodeView.Format = first.Format;
                    //barcodeView.Value= first.Value
                    Context.IsAnalyzing = false;
                    //ResultLabel.Text = $"Barcodes: {first.Format} -> {first.Value}";
                    // ResultLabel.Text = barcodeView
                    if (previousdata != first.Value)
                    {
                        previousdata = first.Value;
                        QRcodeScannerPageViewModel.HandleQRcodeScannerEvent?.Invoke(this, e);
                    }
                });
            }
        }

        void SwitchCameraButton_Clicked(object sender, EventArgs e)
        {
            barcodeView.CameraLocation = barcodeView.CameraLocation == CameraLocation.Rear ? CameraLocation.Front : CameraLocation.Rear;
        }

        void TorchButton_Clicked(object sender, EventArgs e)
        {
            barcodeView.IsTorchOn = !barcodeView.IsTorchOn;
        }
    }
}