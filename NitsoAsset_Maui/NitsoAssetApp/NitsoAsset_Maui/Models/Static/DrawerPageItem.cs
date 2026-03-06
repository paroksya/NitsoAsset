using System;
using System.Windows.Input;
using NitsoAsset_Maui.Models.Enums;
using NitsoAsset_Maui.ViewModels.Base;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace NitsoAsset_Maui.Models.Static
{
    public class DrawerPageItem : AbstractNpcObject
    {
        public string drawerpage_gridlabel { get; set; }
        public string drawerpage_gridimage { get; set; }
        public ICommand Grid_ItemTapCommand { get; set; }

        public SideNavigationEnums NavigationType { get; set; }

        private bool _IsVisibleBadge { get; set; } = false;
        public bool IsVisibleBadge
        {
            get { return _IsVisibleBadge; }
            set
            {
                _IsVisibleBadge = value;
                OnPropertyChanged(nameof(IsVisibleBadge));
            }
        }

        private int _BadgeCount { get; set; }
        public int BadgeCount
        {
            get { return _BadgeCount; }
            set
            {
                _BadgeCount = value;
                OnPropertyChanged(nameof(BadgeCount));
            }
        }
    }
}