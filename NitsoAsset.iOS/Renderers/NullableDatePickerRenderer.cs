using System;
using System.Collections.Generic;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using NitsoAsset.Assets.Controls;
using NitsoAsset.iOS.Renderers;

[assembly: ExportRenderer(typeof(NullableDatePicker), typeof(NullableDatePickerRenderer))]
namespace NitsoAsset.iOS.Renderers
{
	public class NullableDatePickerRenderer : DatePickerRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null && this.Control != null)
			{
				this.AddClearButton();

				//this.Control.BorderStyle = UITextBorderStyle.Line;
				//Control.Layer.BorderColor = UIColor.LightGray.CGColor;
				//Control.Layer.BorderWidth = 1;

				Control.BorderStyle = UITextBorderStyle.None;
				//Control.Layer.CornerRadius = 7;
				////Control.Layer.BorderWidth = 1f;
				Control.AdjustsFontSizeToFitWidth = true;
				Control.TextColor = UIColor.FromRGB(88, 88, 88);

				var entry = (NullableDatePicker)this.Element;
				if (!entry.NullableDate.HasValue)
				{
					this.Control.Text = entry.PlaceHolder;
				}

				//if (Device.Idiom == TargetIdiom.Tablet)
				//{
				//	this.Control.Font = UIFont.SystemFontOfSize(25);
				//}
			}
		}

		[Foundation.Export("textField:shouldChangeCharactersInRange:replacementString:")]
		public bool ShouldChangeCharacters(UIKit.UITextField textField, Foundation.NSRange range, string replacementString)
		{
			return false;
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			// Check if the property we are updating is the format property
			if (e.PropertyName == Xamarin.Forms.DatePicker.DateProperty.PropertyName || e.PropertyName == Xamarin.Forms.DatePicker.FormatProperty.PropertyName)
			{
				var entry = (NullableDatePicker)this.Element;

				// If we are updating the format to the placeholder then just update the text and return
				if (this.Element.Format == entry.PlaceHolder)
				{
					this.Control.Text = entry.PlaceHolder;
					SetValue(entry);

					return;
				}
			}

			base.OnElementPropertyChanged(sender, e);
		}

		private void SetValue(NullableDatePicker customDatePicker)
		{
			if (customDatePicker.NullableDate.HasValue)
			{
				this.Control.TextColor = Color.FromHex("#000000").ToUIColor();
			}
			else
			{
				this.Control.TextColor = UIColor.FromRGB(88, 88, 88);
			}
		}

		private void AddClearButton()
		{
			var originalToolbar = this.Control.InputAccessoryView as UIToolbar;

			if (originalToolbar != null && originalToolbar.Items.Length <= 2)
			{
				var clearButton = new UIBarButtonItem("Clear", UIBarButtonItemStyle.Plain, ((sender, ev) =>
				{
					NullableDatePicker baseDatePicker = this.Element as NullableDatePicker;
					this.Element.Unfocus();
					this.Element.Date = DateTime.Now;
					baseDatePicker.CleanDate();
					baseDatePicker.ClearDateCommand?.Execute(null);
				}));

				var newItems = new List<UIBarButtonItem>();
				foreach (var item in originalToolbar.Items)
				{
					newItems.Add(item);
				}

				newItems.Insert(0, clearButton);

				originalToolbar.Items = newItems.ToArray();
				originalToolbar.SetNeedsDisplay();
			}

			var toolbar = (UIToolbar)Control.InputAccessoryView;
			var doneBtn = originalToolbar.Items[2];

			doneBtn.Clicked += DoneBtn_Clicked;

		}

		void DoneBtn_Clicked(object sender, EventArgs e)
		{
			//var view = (NullableDatePicker)Element;

			//if (view.NullableDate.HasValue)
			//{
			//	Control.Text = view.NullableDate?.ToString(view.Format);

			//	//view.NullableDate = DateTime.Now;

			//	//SetValue(view);
			//}
			//else
			//{
			//	this.Control.Text = view.PlaceHolder;
			//}
		}
	}
}