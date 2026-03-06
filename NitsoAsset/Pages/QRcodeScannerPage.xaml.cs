using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using DCVXamarin;
using NitsoAsset.Pages.Base;
using NitsoAsset.Services;
using NitsoAsset.ViewModels;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace NitsoAsset.Pages
{
    public partial class QRcodeScannerPage : CustomPage //, IBarcodeResultListener
    {
        QRcodeScannerPageViewModel Context;
        ZXingDefaultOverlay overlay;

        string lblResult;
        string lblCode;
        public QRcodeScannerPage()
        {
            InitializeComponent();
            lblResult = null;
            //App.dbr.AddResultListener(this);
            //App.dbr.UpdateRuntimeSettings(EnumDBRPresetTemplate.VIDEO_SINGLE_BARCODE);
            //App.dbr.SetCameraEnhancer(App.dce);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Context = ((QRcodeScannerPageViewModel)this.BindingContext);
            Device.BeginInvokeOnMainThread(() =>
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

        //public void BarcodeResultCallback(int frameID, BarcodeResult[] textResults)
        //{
        //    try
        //    {
        //        Device.BeginInvokeOnMainThread(async () =>
        //        {
        //            lblResult = textResults[0].BarcodeText;
        //            QRcodeScannerPageViewModel.DCVHandleQRcodeScannerEvent?.Invoke(this, textResults[0]);
        //        });
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //Context.IsScanning = true;
                    //Context.ScanDetails = result.Text + " (type: " + result.BarcodeFormat.ToString() + ")";
                    //App.ScanResult = result.Text; // + " (type: " + result.BarcodeFormat.ToString() + ")";
                    //App.ScanResultCode = result.BarcodeFormat.ToString();
                    lblResult = result.Text;
                    lblCode = result.BarcodeFormat.ToString();
                    Context.IsAnalyzing = false;
                    //Context.IsScanning = false;
                    QRcodeScannerPageViewModel.HandleQRcodeScannerEvent?.Invoke(this, result);
                    //App.Current.MainPage.Navigation.PopAsync();

                });
            }
            catch (Exception ex)
            {
                //UserDialogs.Instance.Toast("Invalid QR code");
            }
        }

        void FlashButton_Clicked(Xamarin.Forms.Button sender, System.EventArgs e)
        {
            scanner.IsTorchOn = !scanner.IsTorchOn;
        }
    }
}