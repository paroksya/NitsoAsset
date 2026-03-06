using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace NitsoAsset.Assets.Controls
{
    public class NullableDatePicker : DatePicker
    {
        public NullableDatePicker()
        {
            Format = "dd/MM/yyyy";
            _originalFormat = Format;
        }
        public string _originalFormat = null;

        public static readonly BindableProperty PlaceHolderProperty =
            BindableProperty.Create(nameof(PlaceHolder), typeof(string), typeof(NullableDatePicker), "Select date");

        public string PlaceHolder
        {
            get { return (string)GetValue(PlaceHolderProperty); }
            set
            {
                SetValue(PlaceHolderProperty, value);
            }
        }


        public static readonly BindableProperty NullableDateProperty =
        BindableProperty.Create(nameof(NullableDate), typeof(DateTime?), typeof(NullableDatePicker), null, defaultBindingMode: BindingMode.TwoWay);

        public DateTime? NullableDate
        {
            get { return (DateTime?)GetValue(NullableDateProperty); }
            set { SetValue(NullableDateProperty, value); UpdateDate(); }
        }

        private void UpdateDate()
        {
            if (NullableDate != null)
            {
                if (_originalFormat != null)
                {
                    Format = _originalFormat;
                }
                TextColor = Color.FromHex("#000000");
            }
            else
            {
                Format = PlaceHolder;
                TextColor = Color.FromHex("#80F5F5F5");
            }

        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext != null)
            {
                _originalFormat = Format;
                UpdateDate();
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == DateProperty.PropertyName || (propertyName == IsFocusedProperty.PropertyName && !IsFocused && (Date.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy"))))
            {
                AssignValue();
            }

            if (propertyName == NullableDateProperty.PropertyName && NullableDate.HasValue)
            {
                Date = NullableDate.Value;
                if (Date.ToString(_originalFormat) == DateTime.Now.ToString(_originalFormat))
                {
                    //this code was done because when date selected is the actual date the"DateProperty" does not raise  
                    UpdateDate();
                }
            }
        }

        public void CleanDate()
        {
            NullableDate = null;
            UpdateDate();
        }
        public void AssignValue()
        {
            NullableDate = Date;
            UpdateDate();

        }


        public static BindableProperty ClearDateProperty = BindableProperty.Create("ClearDateProperty", typeof(ICommand), typeof(NullableDatePicker));

        public ICommand ClearDateCommand
        {
            get
            {
                return (ICommand)this.GetValue(ClearDateProperty);
            }
            set
            {
                this.SetValue(ClearDateProperty, value);
            }
        }
        private void DatePicker_ClearDate(object sender, DateChangedEventArgs e)
        {
            if (this.ClearDateCommand != null)
                this.ClearDateCommand.Execute(e);
        }

        public static BindableProperty SelectDateProperty = BindableProperty.Create("SelectDateProperty", typeof(ICommand), typeof(NullableDatePicker));

        public ICommand SelectDateCommand
        {
            get
            {
                return (ICommand)this.GetValue(SelectDateProperty);
            }
            set
            {
                this.SetValue(SelectDateProperty, value);
            }
        }
        private void DatePicker_SelectDate(object sender, DateChangedEventArgs e)
        {
            if (this.SelectDateCommand != null)
                this.SelectDateCommand.Execute(e);
        }
    }
}