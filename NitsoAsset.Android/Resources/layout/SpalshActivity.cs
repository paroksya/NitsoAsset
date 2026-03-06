using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace NitsoAsset.Droid.Resources.layout
{
    [Activity(Theme = "@style/splashscreen", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class SpalshActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //PushNotification start
            var mainIntent = new Intent(Application.Context, typeof(MainActivity));
            if (Intent.Extras != null)
            {
                mainIntent.PutExtras(Intent.Extras);
            }
            mainIntent.SetFlags(ActivityFlags.SingleTop);
            StartActivity(mainIntent);
            //PushNotification end

            //StartActivity(typeof(MainActivity));
        }
    }
}