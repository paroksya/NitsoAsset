using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Locations;
namespace NitsoAsset.Droid.Helpers
{
	public class ActivityResultListener
	{
		private TaskCompletionSource<bool> Complete = new TaskCompletionSource<bool>();
		public Task<bool> Task { get { return this.Complete.Task; } }

		public ActivityResultListener(MainActivity activity)
		{
            // subscribe to activity results
            activity.LocationActivityResult += OnActivityResult;
		}

		private void OnActivityResult(int requestCode, global::Android.App.Result resultCode, Intent data)
		{
			// unsubscribe from activity results
			var context = Android.App.Application.Context;
			var activity = (MainActivity)context;
			activity.LocationActivityResult -= OnActivityResult;

			LocationManager lm = (LocationManager)context.GetSystemService(Context.LocationService);
			bool gps_enabled = false;

			gps_enabled = lm.IsProviderEnabled(LocationManager.GpsProvider);

			this.Complete.TrySetResult(gps_enabled);
		}
	}
}