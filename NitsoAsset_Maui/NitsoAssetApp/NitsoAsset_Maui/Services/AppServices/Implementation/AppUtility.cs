using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NitsoAsset_Maui.Assets.Helpers;
using GeolocatorPlugin;
using Location = Microsoft.Maui.Devices.Sensors.Location;
//using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices.Sensors;
using GeolocatorPlugin.Abstractions;
using Microsoft.Maui.ApplicationModel;

namespace NitsoAsset_Maui.Services.AppServices.Implementation
{
    public class AppUtility : IAppUtility
    {
        public AppUtility()
        {
        }
        CancellationTokenSource cts;

        public string ScanDetails { get; set; }

        public async Task<Position> GetCurrentLocation()
        {
            try
            {
                //todo
                //var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (status != PermissionStatus.Granted)
                {
                    //if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    {
                        //await App.Current.MainPage.DisplayAlert("Alert", "Location is required", "Ok");
                    }

                    //var results = await Permissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                    var results = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    //todo
                    //status = results[Permission.Location];

                    if (status == PermissionStatus.Denied)
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "Please allow 'HRMS app' to access location, Please enable location service from settings.", "Ok");
                        //CrossPermissions.Current.OpenAppSettings();
                    }
                }

                if (status == PermissionStatus.Granted)
                {
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 50;

                    if (locator.IsGeolocationEnabled)
                    {

                        try
                        {
                            Location userLocation;

                            var request = new GeolocationRequest(GeolocationAccuracy.Best);
                            userLocation = await Geolocation.GetLastKnownLocationAsync();
                            Debug.WriteLine(userLocation?.ToString() ?? "no location");
                            userLocation = await Geolocation.GetLocationAsync(request);
                            Debug.WriteLine(userLocation?.ToString() ?? "no location");

                            if (userLocation != null)
                            {
                                return new Position(userLocation.Latitude, userLocation.Longitude);
                            }
                        }
                        catch (FeatureNotSupportedException fnsEx)
                        {
                            // Handle not supported on device exception
                            Debug.WriteLine(fnsEx);
                            return new Position();
                        }
                        catch (PermissionException pEx)
                        {
                            // Handle permission exception
                            Debug.WriteLine(pEx);
                            return new Position();
                        }
                        catch (Exception ex)
                        {
                            // Unable to get location
                            Debug.WriteLine(ex);
                            return new Position();
                        }

                        var position = await locator.GetPositionAsync(new TimeSpan(0, 0, 30));

                        if (position != null)
                        {
                            return new Position(position.Latitude, position.Longitude);
                        }
                    }
                    else
                    {
                        // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
                        if (Device.RuntimePlatform == Device.Android)
                        {
                            try
                            {
                                var title = "App need to access your location";
                                var message = "Please activate location service.";
                                var answer = await App.Current.MainPage.DisplayAlert(title, message, "Settings", "ok");

                                if (answer)
                                {
                                    var result = await NativeDependencyServices.Instance.EnableGPSService();

                                    if (result != false)
                                    {
                                        if (locator.IsGeolocationEnabled)
                                        {
                                            var position = await locator.GetPositionAsync(new TimeSpan(0, 0, 30));
                                            if (position != null)
                                            {
                                                return new Position(position.Latitude, position.Longitude);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception exception)
                            {
                                System.Diagnostics.Debug.WriteLine(exception.Message);
                            }
                        }
                    }
                }

                return new Position();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new Position();
            }
        }

        //public async void ImagePicker(Action<MediaFile> result)
        //{
        //    try
        //    {
        //        MediaFile mediaFile = new MediaFile(null, null);
        //        var action = "Take Photo";
        //        //action = await App.Current.MainPage.DisplayActionSheet("Select option", "Cancel", null, "Take Photo", "Camera Roll");

        //        if (action == "Take Photo")
        //        {
        //            var status = await RuntimePermission.RuntimePermissionStatus(Plugin.Permissions.Abstractions.Permission.Camera);
        //            var Storegestatus = await RuntimePermission.RuntimePermissionStatus(Plugin.Permissions.Abstractions.Permission.Storage);
        //            if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted && Storegestatus == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
        //            {
        //                Device.BeginInvokeOnMainThread(async () =>
        //                {
        //                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
        //                    {
        //                        await App.Current.MainPage.DisplayAlert("Alert", "Camera not available.", "OK");
        //                        return;
        //                    }

        //                    mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
        //                    {
        //                        Directory = "Sample",
        //                        Name = "test.jpg",
        //                        CompressionQuality = 70,
        //                        PhotoSize = PhotoSize.Medium
        //                    });

        //                    result.Invoke(mediaFile);

        //                });
        //            }
        //            else if (status != Plugin.Permissions.Abstractions.PermissionStatus.Unknown)
        //            {
        //                await App.Current.MainPage.DisplayAlert("Alert", "You have not permission to access Camera.", "OK");
        //                result.Invoke(null);
        //            }
        //            else
        //            {
        //                result.Invoke(null);
        //            }
        //        }
        //        else if (action == "Camera Roll")
        //        {
        //            var PhotosStatus = await RuntimePermission.RuntimePermissionStatus(Plugin.Permissions.Abstractions.Permission.Camera);
        //            var Storegestatus = await RuntimePermission.RuntimePermissionStatus(Plugin.Permissions.Abstractions.Permission.Storage);
        //            if (PhotosStatus == Plugin.Permissions.Abstractions.PermissionStatus.Granted && Storegestatus == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
        //            {
        //                Device.BeginInvokeOnMainThread(async () =>
        //                {
        //                    if (!CrossMedia.Current.IsPickPhotoSupported)
        //                    {
        //                        await App.Current.MainPage.DisplayAlert("Alert", "Camera not available.", "OK");
        //                        return;
        //                    }
        //                    mediaFile = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
        //                    {
        //                        CompressionQuality = 70,
        //                        PhotoSize = PhotoSize.Medium
        //                    });
        //                    result.Invoke(mediaFile);

        //                });
        //            }
        //            else if (PhotosStatus != Plugin.Permissions.Abstractions.PermissionStatus.Unknown)
        //            {
        //                await App.Current.MainPage.DisplayAlert("Alert", "You have not permission to access Photos.", "OK");
        //                result.Invoke(null);
        //            }
        //            else
        //            {
        //                result.Invoke(null);
        //            }
        //        }
        //        else
        //        {
        //            result.Invoke(null);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //        result.Invoke(null);
        //    }
        //}
        public async void ImagePicker(Action<FileResult> result)
        {
            try
            {
                var action = await App.Current.MainPage.DisplayActionSheet("Select option", "Cancel", null, "Take Photo", "Camera Roll");
                //var action = "Take Photo";

                if (action == "Take Photo")
                {
                    var camerastatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
                    if (camerastatus != PermissionStatus.Granted)
                    {
                        camerastatus = await Permissions.RequestAsync<Permissions.Camera>();
                    }

                    if (camerastatus == PermissionStatus.Granted)
                    {
                        if (MediaPicker.IsCaptureSupported)
                        {
                            var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                            {
                                Title = "Take a photo"
                            });
                            if (photo != null)
                            {
                                result.Invoke(null);
                            }
                            else
                            {
                                result.Invoke(null);
                            }
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Alert", "Camera not available on this device.", "OK");
                            result.Invoke(null);
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Permission Denied", "Camera access is required to take a photo.", "OK");
                        result.Invoke(null);
                    }
                }
                else if (action == "Camera Roll")
                {
                    var Storegestatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                    if (Storegestatus != PermissionStatus.Granted)
                    {
                        Storegestatus = await Permissions.RequestAsync<Permissions.StorageRead>();
                    }

                    if (Storegestatus == PermissionStatus.Granted)
                    {

                        var photo = await FilePicker.PickAsync(new PickOptions
                        {
                            FileTypes = FilePickerFileType.Images,
                            PickerTitle = "Pick an image"
                        });
                        if (photo != null)
                        {
                            result.Invoke(photo);
                        }
                        else
                        {
                            result.Invoke(null);
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Permission Denied", "Camera access is required to take a photo.", "OK");
                        result.Invoke(null);
                    }
                }
                else
                {
                    result.Invoke(null);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                result.Invoke(null);
            }
        }
    }
}