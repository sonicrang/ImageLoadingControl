using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace ImageLoadingControl
{
    /// <summary>
    /// ImageLoadingControl.xaml 的交互逻辑
    /// </summary>
    public partial class ImageLoadingControl : UserControl
    {
        private Storyboard story;
        private string path;
        BackgroundWorker worker = new BackgroundWorker();
        public ImageLoadingControl()
        {
            InitializeComponent();
            InitializeComponent();
            story = (base.Resources["waiting"] as Storyboard);


            worker.DoWork += (s, e) =>
            {
                Uri uri = new Uri(e.Argument.ToString());

                using (WebClient webClient = new WebClient())
                {
                    webClient.Proxy = null;  //avoids dynamic proxy discovery delay
                    webClient.CachePolicy = new RequestCachePolicy(RequestCacheLevel.Default);
                    try
                    {
                        byte[] imageBytes = null;

                        imageBytes = webClient.DownloadData(uri);

                        if (imageBytes == null)
                        {
                            e.Result = null;
                            return;
                        }
                        MemoryStream imageStream = new MemoryStream(imageBytes);
                        BitmapImage image = new BitmapImage();

                        image.BeginInit();
                        image.StreamSource = imageStream;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.EndInit();

                        image.Freeze();
                        imageStream.Close();

                        e.Result = image;
                    }
                    catch (WebException ex)
                    {
                        e.Result = null;
                    }
                }
            };

            worker.RunWorkerCompleted += (s, e) =>
            {
                try
                {
                    this.story.Stop();
                    imageLoading.Visibility = Visibility.Collapsed;

                    BitmapImage bitmapImage = e.Result as BitmapImage;
                    if (bitmapImage != null)
                    {
                        imageShow.Source = bitmapImage;
                        gridError.Visibility = Visibility.Collapsed;
                    }

                    worker.Dispose();
                }
                catch
                {
                    imageShow.Source = null;
                }
            };
        }
        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(string), typeof(ImageLoadingControl), new FrameworkPropertyMetadata(new PropertyChangedCallback(CallBack)));

        private static void CallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageLoadingControl imgCtl = d as ImageLoadingControl;
            imgCtl.path = e.NewValue != null ? e.NewValue.ToString() : "";
            imgCtl.GetImage();
        }

        private void GetImage()
        {
            try
            {
                if (!string.IsNullOrEmpty(path))
                {

                    if (path.StartsWith("http"))
                    {
                        this.story.Begin();
                        imageLoading.Visibility = Visibility.Visible;
                        worker.RunWorkerAsync(path);
                    }
                    else
                    {
                        imageShow.Source = GetBitmapImage(path);
                    }

                }
                else
                {
                    imageShow.Source = null;
                    gridError.Visibility = Visibility.Visible;
                }
            }
            catch
            {
                imageShow.Source = null;
                gridError.Visibility = Visibility.Visible;
            }
        }
        private BitmapImage GetBitmapImage(string path, int imageWidth = 0)
        {
            BitmapImage bitmap;
            if (File.Exists(path))
            {
                using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(path)))
                {
                    bitmap = new BitmapImage();
                    bitmap.BeginInit();

                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    if (imageWidth > 0)
                    {
                        using (System.Drawing.Image drawingImage = System.Drawing.Image.FromStream(ms))
                        {
                            int old_w = drawingImage.Width;
                            int old_h = drawingImage.Height;
                            int imageHeight = (int)(old_h * (imageWidth * 1.0 / old_w));
                            using (System.Drawing.Image thumImage = drawingImage.GetThumbnailImage(imageWidth, imageHeight, () => { return true; }, IntPtr.Zero))
                            {
                                MemoryStream ms_thum = new MemoryStream();
                                thumImage.Save(ms_thum, System.Drawing.Imaging.ImageFormat.Png);
                                bitmap.StreamSource = ms_thum;
                            }
                        }
                    }
                    else
                    {
                        bitmap.StreamSource = ms;
                    }
                    bitmap.EndInit();
                    bitmap.Freeze();

                    gridError.Visibility = Visibility.Collapsed;
                }

                return bitmap;
            }
            else
                return null;
        }

    }
}
