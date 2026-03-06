using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NitsoAsset.Assets.Helpers;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Location = Xamarin.Essentials.Location;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

namespace NitsoAsset.Services.AppServices.Implementation
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
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        //await App.Current.MainPage.DisplayAlert("Alert", "Location is required", "Ok");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                    status = results[Permission.Location];

                    if (status == PermissionStatus.Denied)
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "Please allow 'HRMS app' to access location, Please enable location service from settings.", "Ok");
                        CrossPermissions.Current.OpenAppSettings();
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

        public async void ImagePicker(Action<MediaFile> result)
        {
            try
            {
                MediaFile mediaFile = new MediaFile(null, null);
                var action = "Take Photo";
                //action = await App.Current.MainPage.DisplayActionSheet("Select option", "Cancel", null, "Take Photo", "Camera Roll");

                if (action == "Take Photo")
                {
                    var status = await RuntimePermission.RuntimePermissionStatus(Plugin.Permissions.Abstractions.Permission.Camera);
                    var Storegestatus = await RuntimePermission.RuntimePermissionStatus(Plugin.Permissions.Abstractions.Permission.Storage);
                    if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted && Storegestatus == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                            {
                                await App.Current.MainPage.DisplayAlert("Alert", "Camera not available.", "OK");
                                return;
                            }

                            mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                            {
                                Directory = "Sample",
                                Name = "test.jpg",
                                CompressionQuality = 70,
                                PhotoSize = PhotoSize.Medium
                            });

                            result.Invoke(mediaFile);

                        });
                    }
                    else if (status != Plugin.Permissions.Abstractions.PermissionStatus.Unknown)
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "You have not permission to access Camera.", "OK");
                        result.Invoke(null);
                    }
                    else
                    {
                        result.Invoke(null);
                    }
                }
                else if (action == "Camera Roll")
                {
                    var PhotosStatus = await RuntimePermission.RuntimePermissionStatus(Plugin.Permissions.Abstractions.Permission.Camera);
                    var Storegestatus = await RuntimePermission.RuntimePermissionStatus(Plugin.Permissions.Abstractions.Permission.Storage);
                    if (PhotosStatus == Plugin.Permissions.Abstractions.PermissionStatus.Granted && Storegestatus == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            if (!CrossMedia.Current.IsPickPhotoSupported)
                            {
                                await App.Current.MainPage.DisplayAlert("Alert", "Camera not available.", "OK");
                                return;
                            }
                            mediaFile = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                            {
                                CompressionQuality = 70,
                                PhotoSize = PhotoSize.Medium
                            });
                            result.Invoke(mediaFile);

                        });
                    }
                    else if (PhotosStatus != Plugin.Permissions.Abstractions.PermissionStatus.Unknown)
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "You have not permission to access Photos.", "OK");
                        result.Invoke(null);
                    }
                    else
                    {
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