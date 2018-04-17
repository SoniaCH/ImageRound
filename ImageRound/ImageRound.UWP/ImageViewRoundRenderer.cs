using Xamarin.Forms.Platform.UWP;
using System;
using System.IO;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Xamarin.Forms;
using ImageRound;
using ImageRound.UWP;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(ImageViewRound), typeof(ImageViewRoundRenderer))]
namespace ImageRound.UWP
{
    public class ImageViewRoundRenderer: ViewRenderer<Image, Ellipse>
    {
        

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || Element == null)
                return;

            var ellipse = new Ellipse();
            SetNativeControl(ellipse);
        }

        Xamarin.Forms.ImageSource file = null;
       // private BitmapImage bitmapImage;

        protected async override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null)
                return;



            var min = Math.Min(Element.Width, Element.Height);
            if (min / 2.0f <= 0)
                return;

            try
            {
                Control.Width = min;
                Control.Height = min;

                Control.Width = min;
                Control.Height = min;
          
                // Fill background color
                Control.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));

                // Fill stroke
                Control.Stroke= new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));
                
                Control.StrokeThickness = 0.1;

                var force = e.PropertyName == VisualElement.XProperty.PropertyName ||
                            e.PropertyName == VisualElement.YProperty.PropertyName ||
                            e.PropertyName == VisualElement.WidthProperty.PropertyName ||
                            e.PropertyName == VisualElement.HeightProperty.PropertyName ||
                            e.PropertyName == VisualElement.ScaleProperty.PropertyName ||
                            e.PropertyName == VisualElement.TranslationXProperty.PropertyName ||
                            e.PropertyName == VisualElement.TranslationYProperty.PropertyName ||
                            e.PropertyName == VisualElement.RotationYProperty.PropertyName ||
                            e.PropertyName == VisualElement.RotationXProperty.PropertyName ||
                            e.PropertyName == VisualElement.RotationProperty.PropertyName ||
                            e.PropertyName == VisualElement.AnchorXProperty.PropertyName ||
                            e.PropertyName == VisualElement.AnchorYProperty.PropertyName;


                //already set
                if (file == Element.Source && !force)
                    return;

                file = Element.Source;

                BitmapImage bitmapImage = null;
              
                // Handle file images
                if (file is FileImageSource)
                {
                    var fi = Element.Source as FileImageSource;
                    var myFile = System.IO.Path.Combine(Package.Current.InstalledLocation.Path, fi.File);
                    var myFolder = await StorageFolder.GetFolderFromPathAsync(System.IO.Path.GetDirectoryName(myFile));

                    using (Stream s = await myFolder.OpenStreamForReadAsync(System.IO.Path.GetFileName(myFile)))
                    {
                        var memStream = new MemoryStream();
                        await s.CopyToAsync(memStream);
                        memStream.Position = 0;
                        bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(memStream.AsRandomAccessStream());
                    }
                }
                else if (file is UriImageSource)
                {
                    bitmapImage = new BitmapImage((Element.Source as UriImageSource).Uri);
                }

                else if (file is StreamImageSource)
                {
                    var handler = new StreamImageSourceHandler();
                    var imageSource = await handler.LoadImageAsync(file);

                    if (imageSource != null)
                    {
                        Control.Fill = new ImageBrush
                        {
                            ImageSource = imageSource,
                            Stretch = Stretch.UniformToFill,
                        };
                    }
                    return;
                }

                if (bitmapImage != null)
                {
                    Control.Fill = new ImageBrush
                    {
                        ImageSource = bitmapImage,
                        Stretch = Stretch.UniformToFill,
                    };
                }
            }
            catch
            {
            }
        }


    }
}
