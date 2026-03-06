using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Platform;
using System;
using System.Security;
using System.Threading.Tasks;
namespace NitsoAsset_Maui.Assets.Helpers
{
    public class RuntimePermission
    {
        public RuntimePermission()
        {
        }

        public async static Task<PermissionStatus> RuntimePermissionCameraStatus()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    var results = await Permissions.RequestAsync<Permissions.Camera>();
                    var statuscamera = await Permissions.CheckStatusAsync<Permissions.Camera>();
                    //return status = results[permission];
                }
                return status;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("\nIn PriorityZOCCAM.Assets.Helper.RuntimePermissionStatus() - Exception attempting to check or request permission to Permission.{0}:\n{1}\n", ex);
                return PermissionStatus.Unknown;
            }
        }

        public async static Task<PermissionStatus> RuntimePermissionStorageStatus()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                if (status != PermissionStatus.Granted)
                {
                    var results = await Permissions.RequestAsync<Permissions.StorageRead>();
                    //return status = results[permission];
                }
                return status;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("\nIn PriorityZOCCAM.Assets.Helper.RuntimePermissionStatus() - Exception attempting to check or request permission to Permission.{0}:\n{1}\n", ex);
                return PermissionStatus.Unknown;
            }
        }
    }
}