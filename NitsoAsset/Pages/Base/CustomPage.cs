using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using NitsoAsset.ViewModels.Base;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
namespace NitsoAsset.Pages.Base
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

        public CustomPage()
        {
            this.BackgroundColor = Color.White;

            CustomNavigationPage.SetBackButtonTitle(this, string.Empty);

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        public void SetBinding<TSource>(BindableProperty targetProperty, Expression<Func<TSource, object>> sourceProperty, BindingMode mode = BindingMode.Default,
                                      IValueConverter converter = null, string stringFormat = null)
        {
            (this as BindableObject).SetBinding(targetProperty, sourceProperty, mode,
                converter, stringFormat);
        }


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