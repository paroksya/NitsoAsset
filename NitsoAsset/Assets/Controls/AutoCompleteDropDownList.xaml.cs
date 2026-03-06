using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

namespace NitsoAsset.Assets.Controls
{
    public partial class AutoCompleteDropDownList : ContentView
    {
        public AutoCompleteDropDownList()
        {
            InitializeComponent();

            _listView.ItemSelected += (object sender, SelectedItemChangedEventArgs e) =>
            {
                if (_listView.SelectedItem == null) return;

                _listView.SelectedItem = null;
            };
        }

        public ListView ListView
        {
            get { return _listView; }
        }

        public ContentView WrapperView
        {
            get { return _dropdownWrapper; }
        }

        public PancakeView ListPancakeView
        {
            get { return _dropdownPancake; }
        }
        public ActivityIndicator ListLoaderView
        {
            get { return _dropdownLoader; }
        }
    }
}