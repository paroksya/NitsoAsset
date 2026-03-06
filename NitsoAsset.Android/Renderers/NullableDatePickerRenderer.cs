using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.App;
using Android.Widget;
using System.ComponentModel;
using Android.Content;
using NitsoAsset.Assets.Controls;
namespace NitsoAsset.Droid.Renderers
{
    public class NullableDatePickerRenderer : ViewRenderer<NullableDatePicker, EditText>
    {
        DatePickerDialog _dialog;

        public NullableDatePickerRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<NullableDatePicker> e)
        {
            base.OnElementChanged(e);

            this.SetNativeControl(new Android.Widget.EditText(Context));
            if (Control == null || e.NewElement == null)
                return;

            var entry = (NullableDatePicker)this.Element;

            //Control.SetBackgroundColor(global::Android.Graphics.Color.White);
            Control.SetBackgroundColor(global::Android.Graphics.Color.Argb(0, 0, 0, 0));
            Control?.SetPadding(Control.PaddingLeft, 0, Control.PaddingRight, 0);

            this.Control.VerticalScrollBarEnabled = false;
            this.Control.HorizontalScrollBarEnabled = false;
            //var GradientBackground = new GradientDrawable();
            //GradientBackground.SetCornerRadius(MyEntryRenderer.DpToPixels(this.Context, Convert.ToSingle(7)));
            //GradientBackground.SetStroke(1, global::Android.Graphics.Color.Rgb(188, 188, 188));
            //GradientBackground.SetColor(global::Android.Graphics.Color.White);
            //Control.SetBackground(GradientBackground);

            //var font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, entry.FontFamily + ".ttf");
            //Control.Typeface = font;
            this.Control.SetTextColor(global::Android.Graphics.Color.Rgb(88, 88, 88));
            Control.TextSize = (float)entry.FontSize;

            this.Control.Click += OnPickerClick;
            this.Control.Text = !entry.NullableDate.HasValue ? entry.PlaceHolder : Element.Date.ToString(Element.Format);
            this.Control.KeyListener = null;
            this.Control.FocusChange += OnPickerFocusChange;
            this.Control.Enabled = Element.IsEnabled;

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var entry = (NullableDatePicker)this.Element;
            if (e.PropertyName == Xamarin.Forms.DatePicker.DateProperty.PropertyName || e.PropertyName == Xamarin.Forms.DatePicker.FormatProperty.PropertyName)
            {
                if (this.Element.Format == entry.PlaceHolder)
                {
                    this.Control.Text = entry.PlaceHolder;
                    SetValue(entry);

                    return;
                }
                else
                {
                    this.Control.Text = Element.Date.ToString(Element.Format);
                    SetValue(entry);
                }
            }

            base.OnElementPropertyChanged(sender, e);
        }

        protected override void OnFocusChangeRequested(object sender, VisualElement.FocusRequestArgs e)
        {
            //base.OnFocusChangeRequested(sender, e);

            if (e.Focus)
            {
                ShowDatePicker();
            }
        }

        private void SetValue(NullableDatePicker customDatePicker)
        {
            if (customDatePicker.NullableDate.HasValue)
            {
                this.Control.SetTextColor(Xamarin.Forms.Color.FromHex("#000000").ToAndroid());//Xamarin.Forms.Color.FromHex("#0886c3").ToAndroid());
            }
            else
            {
                this.Control.SetTextColor(global::Android.Graphics.Color.Rgb(88, 88, 88)); //Xamarin.Forms.Color.Silver.ToAndroid());
            }
        }

        void OnPickerFocusChange(object sender, Android.Views.View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                ShowDatePicker();
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (Control != null)
                {
                    this.Control.Click -= OnPickerClick;
                    this.Control.FocusChange -= OnPickerFocusChange;

                    if (_dialog != null)
                    {
                        _dialog.Hide();
                        _dialog.Dispose();
                        _dialog = null;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }


            base.Dispose(disposing);
        }

        void OnPickerClick(object sender, EventArgs e)
        {
            ShowDatePicker();
        }

        void SetDate(DateTime date)
        {

            this.Control.Text = date.ToString(!string.IsNullOrEmpty(Element.Format) ? Element.Format : "dd/MM/yyyy");
            Element.Date = date;

            SetValue(Element);

        }

        private void ShowDatePicker()
        {
            var date = Element.MaximumDate;
            var maxDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59).ToUniversalTime();
            var maxDateInMillis = (long)maxDate.Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;



            var mdate = Element.MinimumDate;
            var minDate = new DateTime(mdate.Year, mdate.Month, mdate.Day, 23, 59, 59).ToUniversalTime();
            var minDateInMillis = (long)minDate.Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;


            CreateDatePickerDialog(this.Element.Date.Year, this.Element.Date.Month - 1, this.Element.Date.Day);
            _dialog.DatePicker.MaxDate = maxDateInMillis;

            _dialog.DatePicker.MinDate = minDateInMillis;

            //if (_dialog != null)
            //{
            //	_dialog.DatePicker.MaxDate = (long)Element.MaximumDate.ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
            //}

            _dialog.Show();
        }

        void CreateDatePickerDialog(int year, int month, int day)
        {

            NullableDatePicker view = Element;

            _dialog = new DatePickerDialog(Context, Resource.Style.my_dialog_theme, (o, e) =>
            {
                view.Date = e.Date;
                ((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                Control.ClearFocus();
                _dialog = null;
            }, year, month, day);

            _dialog.SetButton("DONE", (sender, e) =>
            {
                this.Element.Format = this.Element._originalFormat;
                SetDate(_dialog.DatePicker.DateTime);
                this.Element.AssignValue();
                this.Element.SelectDateCommand?.Execute(null);
            });
            _dialog.SetButton2("CANCEL", (sender, e) =>
            {
                this.Element.CleanDate();
                this.Element.ClearDateCommand?.Execute(null);
                Control.Text = this.Element.Format;
            });
        }
    }
}