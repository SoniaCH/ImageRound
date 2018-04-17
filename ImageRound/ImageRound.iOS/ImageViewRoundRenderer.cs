using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Foundation;
using ImageRound;
using ImageRound.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ImageViewRound), typeof(ImageViewRoundRenderer))]
namespace ImageRound.iOS
{
    public class ImageViewRoundRenderer : ImageRenderer
    {
        #region method to create a circle
        private void CreateCircle()
        {
            try
            {
                double min = Math.Min(Element.Width, Element.Height);
                Control.Layer.CornerRadius = (float)(min / 2.0);
                Control.Layer.MasksToBounds = false;
                Control.Layer.BorderColor = Color.White.ToCGColor();
                Control.Layer.BorderWidth = 3;
                Control.ClipsToBounds = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to create circle image: " + ex);
            }
        }

        #endregion

        #region overriding the OnElementChanged
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
                return;

            CreateCircle();
        }
        #endregion

        #region the overriding of the onElementPropertyChanged
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == VisualElement.HeightProperty.PropertyName ||
                e.PropertyName == VisualElement.WidthProperty.PropertyName)
            {
                CreateCircle();
            }
        }
        #endregion



    }
}