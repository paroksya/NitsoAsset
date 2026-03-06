using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Grid = Microsoft.Maui.Controls.Grid;
using StackLayout = Microsoft.Maui.Controls.StackLayout;
namespace NitsoAsset_Maui.Assets.Controls
{
    public class RepeaterView : StackLayout
    {
        #region Bindable Properties

        /// <summary>
        /// Property bound to <see cref="ItemTemplate"/>.
        /// </summary>
        public static readonly BindableProperty ItemTemplateProperty =
           BindableProperty.Create(
               propertyName: nameof(ItemTemplate),
               returnType: typeof(DataTemplate),
                declaringType: typeof(RepeaterView),
               defaultValue: default(DataTemplate),
               propertyChanged: OnItemTemplateChanged);

        /// <summary>
        /// Property bound to <see cref="ItemsSource"/>.
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                propertyName: nameof(ItemsSource),
                returnType: typeof(IEnumerable<object>),
                declaringType: typeof(RepeaterView),
                propertyChanged: OnItemsSourceChanged);

        public static readonly BindableProperty ColumnCountProperty =
                BindableProperty.Create(
                propertyName: nameof(ColumnCount),
                returnType: typeof(double),
                declaringType: typeof(RepeaterView),
                defaultValue: double.MinValue,
                propertyChanged: OnColumnCountChanged);

        public static readonly BindableProperty RowCountProperty =
               BindableProperty.Create(
               propertyName: nameof(RowCount),
               returnType: typeof(double),
               declaringType: typeof(RepeaterView),
               defaultValue: double.MinValue,
               propertyChanged: OnRowCountChanged);

        public static readonly BindableProperty HasUnEvenColumnProperty =
               BindableProperty.Create(
               propertyName: nameof(HasUnEvenColumn),
               returnType: typeof(bool),
               declaringType: typeof(RepeaterView),
               defaultValue: true,
               propertyChanged: OnHasUnEvenColumnChanged);


        public static readonly BindableProperty RowSpacingProperty =
                BindableProperty.Create(
                propertyName: nameof(RowSpacing),
                returnType: typeof(double),
                declaringType: typeof(RepeaterView),
                defaultValue: null);
        #endregion

        #region Properties 

        /// <summary>
        /// Gets or sets the <see cref="DataTemplate"/>.
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the collection of view models to bind to the item views.
        /// </summary>
        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public double ColumnCount
        {
            get { return (double)GetValue(ColumnCountProperty); }
            set { SetValue(ColumnCountProperty, value); }
        }

        public double RowCount
        {
            get { return (double)GetValue(RowCountProperty); }
            set { SetValue(RowCountProperty, value); }
        }

        public bool HasUnEvenColumn
        {
            get { return (bool)GetValue(HasUnEvenColumnProperty); }
            set { SetValue(HasUnEvenColumnProperty, value); }
        }
        public double RowSpacing
        {
            get { return (double)GetValue(RowSpacingProperty); }
            set { SetValue(RowSpacingProperty, value); }
        }
        #endregion

        #region Property Changed Callbacks

        /// <summary>
        /// Called when <see cref="ItemTemplate"/> changes.
        /// </summary>
        /// <param name="bindable">The <see cref="BindableObject"/> being changed.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var layout = (RepeaterView)bindable;
            if (newValue == null)
            {
                return;
            }

            layout.PopulateItems();
        }

        /// <summary>
        /// Called when <see cref="ItemsSource"/> is changed.
        /// </summary>
        /// <param name="bindable">The <see cref="BindableObject"/> being changed.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var layout = (RepeaterView)bindable;
            if (newValue == null)
            {
                layout.Children.Clear();
                return;
            }

            layout.PopulateItems();
        }

        private static void OnColumnCountChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var layout = (RepeaterView)bindable;
            if (newValue == null)
            {
                return;
            }

            layout.PopulateItems();
        }

        private static void OnRowCountChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var layout = (RepeaterView)bindable;
            if (newValue == null)
            {
                return;
            }

            layout.PopulateItems();
        }

        private static void OnHasUnEvenColumnChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var layout = (RepeaterView)bindable;
            if (newValue == null)
            {
                return;
            }

            layout.PopulateItems();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates and binds the item views based on <see cref="ItemTemplate"/>.
        /// </summary>
        private void PopulateItems()
        {
            var items = ItemsSource;
            if (items == null || ItemTemplate == null)
            {
                return;
            }

            var children = Children;
            children.Clear();

            if (ColumnCount > 0 && RowCount <= 0)
            {
                var ViewCollection = items.ToList();
                var grdUnitType = HasUnEvenColumn ? GridUnitType.Auto : GridUnitType.Star;
                var grd = new Grid()
                {
                    ColumnSpacing = this.Spacing,
                    RowSpacing = RowSpacing > 0 ? RowSpacing : this.Spacing,
                    Padding = 0,
                    ColumnDefinitions ={
                        new ColumnDefinition(){ Width = new GridLength(1,grdUnitType)},
                    }
                };
                var row = 0;
                for (int i = 0; i < ViewCollection.Count; i++)
                {
                    for (int j = 0; j < ColumnCount; j++)
                    {
                        if (i < ViewCollection.Count && ViewCollection[i] != null)
                        {
                            var item = ViewCollection[i];

                            //grd.Children.Add(InflateView(item), j, row);

                            if (j != ColumnCount - 1)
                                i++;
                        }
                    }
                    row++;
                }
                children.Add(grd);
            }
            else if (RowCount > 0)
            {
                var ViewCollection = items.ToList();
                var grd = new Grid()
                {
                    ColumnSpacing = this.Spacing,
                    RowSpacing = this.Spacing,
                    Padding = 0,
                };

                for (int i = 0; i < RowCount; i++)
                {
                    grd.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
                }

                var column = 0;
                for (int i = 0; i < ViewCollection.Count; i++)
                {
                    grd.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
                    for (int j = 0; j < RowCount; j++)
                    {
                        if (i < ViewCollection.Count && ViewCollection[i] != null)
                        {
                            var item = ViewCollection[i];

                            //grd.Children.Add(InflateView(item), column, j);

                            if (j != RowCount - 1)
                                i++;
                        }
                    }
                    column++;
                }

                var scrollView = new ScrollView()
                {
                    Orientation = ScrollOrientation.Horizontal,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
                    Content = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, Children = { grd } }
                };

                children.Add(scrollView);
            }
            else
            {
                foreach (var item in items)
                {
                    children.Add(InflateView(item));
                }
            }

        }

        /// <summary>
        /// Inflates an item view using the correct <see cref="DataTemplate"/> for the given view model.
        /// </summary>
        /// <param name="viewModel">The view model to bind the item view to.</param>
        /// <returns>The new view with the view model as its binding context.</returns>
        private View InflateView(object viewModel)
        {
            var view = (View)CreateContent(ItemTemplate, viewModel, this);
            view.BindingContext = viewModel;
            return view;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Create UI content from a <see cref="DataTemplate"/> (or optionally a <see cref="DataTemplateSelector"/>).
        /// </summary>
        /// <param name="template">The <see cref="DataTemplate"/>.</param>
        /// <param name="item">The view model object.</param>
        /// <param name="container">The <see cref="BindableObject"/> that will be the parent to the content.</param>
        /// <returns>The content created by the template.</returns>
        public static object CreateContent(DataTemplate template, object item, BindableObject container)
        {
            var selector = template as DataTemplateSelector;
            if (selector != null)
            {
                template = selector.SelectTemplate(item, container);
            }

            return template.CreateContent();
        }

        #endregion
    }
}
