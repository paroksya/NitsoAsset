using System;
using Android.Content;
using NitsoAsset.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Editor), typeof(CustomEditorRenderer))]
namespace NitsoAsset.Droid.Renderers
{
	public class CustomEditorRenderer : EditorRenderer
	{
		public CustomEditorRenderer(Context context) : base(context)
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);

			var view = (Editor)Element;
			if (view != null && Control != null)
			{

				Control.SetBackgroundColor(global::Android.Graphics.Color.Argb(0, 0, 0, 0));

				//Control.SetPadding(5, Control.PaddingTop, 10, Control.PaddingBottom);
			}
		}

	}
}