using System;
using CoreAnimation;
using NitsoAsset.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Editor), typeof(CustomEditorRenderer))]
namespace NitsoAsset.iOS.Renderers
{
    public class CustomEditorRenderer : EditorRenderer
    {
        public CustomEditorRenderer() : base()
        {
        }

        CALayer border = new CALayer();
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            var view = (Editor)Element;

            if (view != null)
            {
                //Control.TextContainerInset = new UIEdgeInsets(10, -5, 10, 5);

            }
        }
    }
}