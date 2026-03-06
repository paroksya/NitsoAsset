using System;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace NitsoAsset_Maui.ViewModels.Base
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