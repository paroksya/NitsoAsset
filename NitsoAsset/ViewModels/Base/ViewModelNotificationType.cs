using System;

using Xamarin.Forms;

namespace NitsoAsset.ViewModels.Base
{
    public enum ViewModelNotificationType
    {
        LocalRefresh = 0,
        ReSync,
        Pop,
        PopIfTop,
        LoginCompleted,
        CheckForProfilePictures,
        DeleteProfileMedia,
    }
}