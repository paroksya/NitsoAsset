using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
namespace NitsoAsset.ViewModels.Base
{
    public abstract class AbstractNpcObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Object.Equals(property, value))
            {
                property = value;
                OnPropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}