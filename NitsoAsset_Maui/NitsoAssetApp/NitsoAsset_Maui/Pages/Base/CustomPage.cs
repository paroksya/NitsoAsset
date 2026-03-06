using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using NitsoAsset_Maui.ViewModels.Base;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace NitsoAsset_Maui.Pages.Base
{
    public delegate void PageClosedEventHandler(object sender, EventArgs e);

    public class CustomPage : ContentPage, ICustomPage
    {
        public static readonly BindableProperty FormattedTitleProperty = BindableProperty.Create(nameof(FormattedTitle), typeof(FormattedString), typeof(CustomPage), null);

        public FormattedString FormattedTitle
        {
            get { return (FormattedString)GetValue(FormattedTitleProperty); }
            set
            {
                SetValue(FormattedTitleProperty, value);
            }
        }

        public static readonly BindableProperty FormattedSubtitleProperty = BindableProperty.Create(nameof(FormattedSubtitle), typeof(FormattedString), typeof(CustomPage), null);

        public FormattedString FormattedSubtitle
        {
            get { return (FormattedString)GetValue(FormattedSubtitleProperty); }
            set
            {
                SetValue(FormattedSubtitleProperty, value);
            }
        }

        public static readonly BindableProperty SubtitleProperty = BindableProperty.Create(nameof(Subtitle), typeof(string), typeof(CustomPage), null);


        public string Subtitle
        {
            get { return (string)GetValue(SubtitleProperty); }
            set
            {
                SetValue(SubtitleProperty, value);
            }
        }

        public FileImageSource Icon { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public CustomPage()
        {
            this.BackgroundColor = Colors.White;

            CustomNavigationPage.SetBackButtonTitle(this, string.Empty);

            On<Microsoft.Maui.Controls.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        public void SetBinding<TSource>(BindableProperty targetProperty, string sourceProperty, BindingMode mode = BindingMode.Default,
                                     IValueConverter converter = null, string stringFormat = null)
        {
            (this as BindableObject).SetBinding(targetProperty, sourceProperty, mode,
                converter, stringFormat);
        }

        //public void SetBinding<TSource>(BindableProperty targetProperty, Expression<Func<TSource, object>> sourceProperty, BindingMode mode = BindingMode.Default,
        //                              IValueConverter converter = null, string stringFormat = null)
        //{
        //    (this as BindableObject).SetBinding(targetProperty, sourceProperty, mode,
        //        converter, stringFormat);
        //}


        public event PageClosedEventHandler PageClosing;

        public void OnPageClosing()
        {
            PageClosing?.Invoke(this, new EventArgs());
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var viewModel = BindingContext as IViewModel;

            if (viewModel?.ToolbarItems == null)
                return;

            viewModel.ToolbarItems.CollectionChanged += ViewModel_ToolbarItems_CollectionChanged;

            foreach (var toolBarItem in viewModel.ToolbarItems)
                if (ToolbarItems.All(x => x.Text != toolBarItem.Text))
                    ToolbarItems.Add(toolBarItem);

        }

        private void ViewModel_ToolbarItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ToolbarItems.Clear();

            var vmToolbar = sender as ObservableCollection<ToolbarItem>;

            if (vmToolbar == null)
                return;

            foreach (var item in vmToolbar)
                if (ToolbarItems.All(x => x.Text != item.Text))
                    ToolbarItems.Add(item);
        }

       
    }
}