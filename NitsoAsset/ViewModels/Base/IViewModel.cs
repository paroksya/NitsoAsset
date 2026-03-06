using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
namespace NitsoAsset.ViewModels.Base
{
    public interface IViewModel : INotifyPropertyChanged
    {
        void Init(object args);

        void OnAppearing();

        void OnDisappearing();

        void OnClosing();

        void OnNavigationServiceNotification(object sender, ViewModelNotificationType notificationType, object args);

        bool IsBusy { get; set; }

        string Title { get; set; }

        string Icon { get; set; }

        ObservableCollection<ToolbarItem> ToolbarItems { get; set; }

        Type PageType { get; }

        Type DrawerMenuViewModelType { get; }
    }
}