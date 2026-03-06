using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using NitsoAsset_Maui.Models.Enums;
using NitsoAsset_Maui.ViewModels.Base;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace NitsoAsset_Maui.Models
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