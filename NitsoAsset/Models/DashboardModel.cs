using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using NitsoAsset.Models.Enums;
using NitsoAsset.ViewModels.Base;
using Sharpnado.Shades;
using Xamarin.Forms;

namespace NitsoAsset.Models
{
    public class DashboardModel : AbstractNpcObject
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public Color BgColor { get; set; }
        //public Shade Shades { get; set; }
        //ObservableCollection<Shade> _Shades;
        //public ObservableCollection<Shade> Shades
        //{
        //    get { return _Shades; }
        //    set
        //    {
        //        _Shades = value;
        //        OnPropertyChanged(nameof(Shades));
        //    }
        //}

        public DashboardEnums DashboardNavType { get; set; }
        
        public ICommand ItemTapCommand { get; set; }
    }
}