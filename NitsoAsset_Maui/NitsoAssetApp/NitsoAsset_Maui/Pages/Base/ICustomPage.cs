using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace NitsoAsset_Maui.Pages.Base
{
    public interface ICustomPage
    {
        void SetBinding<TSource>(BindableProperty targetProperty,
                string sourceProperty,
                BindingMode mode = 0,
                IValueConverter converter = null,
                string stringFormat = null);

        IList<ToolbarItem> ToolbarItems { get; }

        string Title { get; set; }

        FileImageSource Icon { get; set; }

        object BindingContext { get; set; }

        event EventHandler Appearing;

        event EventHandler Disappearing;

        event PageClosedEventHandler PageClosing;

        void OnPageClosing();
    }
}