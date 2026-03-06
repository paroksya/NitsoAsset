using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitsoAsset_Maui.Platforms.Android
{
    public class PermissionHelper
    {
        private static TaskCompletionSource<bool>? _permissionTcs;

        public static async Task<bool> EnsurePermissionsGrantedAsync(Java.Lang.Object activity)
        {
            if (activity is not Activity androidActivity)
                return false;

            var permissions = new[]
            {
                Manifest.Permission.Camera,
                Manifest.Permission.RecordAudio,
                Manifest.Permission.ReadExternalStorage,
                Manifest.Permission.WriteExternalStorage
            };

            var permissionsToRequest = new List<string>();

            foreach (var permission in permissions)
            {
                if (ContextCompat.CheckSelfPermission(androidActivity, permission) != Permission.Granted)
                {
                    permissionsToRequest.Add(permission);
                }
            }

            if (!permissionsToRequest.Any())
                return true;

            _permissionTcs = new TaskCompletionSource<bool>();

            ActivityCompat.RequestPermissions(androidActivity, permissionsToRequest.ToArray(), 1000);

            return await _permissionTcs.Task;
        }

        public static void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == 1000)
            {
                bool allGranted = grantResults.All(result => result == Permission.Granted);
                _permissionTcs?.SetResult(allGranted);
                _permissionTcs = null;
            }
        }

        public static bool HasCameraPermission(Activity activity)
        {
            return ContextCompat.CheckSelfPermission(activity, Manifest.Permission.Camera) == Permission.Granted;
        }

        public static bool HasAudioPermission(Activity activity)
        {
            return ContextCompat.CheckSelfPermission(activity, Manifest.Permission.RecordAudio) == Permission.Granted;
        }
    }
}