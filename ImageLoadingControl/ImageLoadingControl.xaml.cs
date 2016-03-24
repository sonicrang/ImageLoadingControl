using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        private string token;
        public ImageLoadingControl()
        {
            InitializeComponent();
            story = (base.Resources["waiting"] as Storyboard);
        }
        private async Task<BitmapImage> DownloadHttpImage(string Path)
        {
            //空值直接返回
            if (string.IsNullOrEmpty(Path))
                return null;

            return await Task.Run<BitmapImage>(() =>
            {
                using (WebClient webClient = new WebClient())
                {
                    Uri uri = new Uri(Path);
                    BitmapImage image = new BitmapImage();
                    webClient.CachePolicy = new RequestCachePolicy(RequestCacheLevel.Default);
                    try
                    {
                        byte[] imageBytes = null;
                        imageBytes = webClient.DownloadData(uri);
                        if (imageBytes == null)
                        {
                            image = null;
                        }
                        MemoryStream imageStream = new MemoryStream(imageBytes);
                        image.BeginInit();
                        image.StreamSource = imageStream;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.EndInit();

                        image.Freeze();
                        imageStream.Close();
                    }
                    catch (WebException ex)
                    {
                        image = null;
                    }
                    return image;
                }
            });
        }

        private async void GetHttpImage(string Path, string token)
        {
            if (!string.IsNullOrEmpty(Path))
            {
                BitmapImage image = await DownloadHttpImage(Path);

                if (token == this.token)  //下载队列token = 当前请求token 才能赋值
                {
                    this.story.Stop();
                    imageLoading.Visibility = Visibility.Collapsed;
                    imageShow.Source = image;
                    if (image != null)
                    {
                        gridError.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(string), typeof(ImageLoadingControl), new FrameworkPropertyMetadata(new PropertyChangedCallback(CallBack)));

        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ImageLoadingControl), new FrameworkPropertyMetadata(Stretch.Fill, new PropertyChangedCallback(StretchCallBack)));

        private static void CallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageLoadingControl imgCtl = d as ImageLoadingControl;
            imgCtl.imageShow.Source = null;
            string token = Guid.NewGuid().ToString("N");
            imgCtl.token = token;
            imgCtl.path = e.NewValue != null ? e.NewValue.ToString() : "";
            imgCtl.GetImage(token);
        }

        private static void StretchCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageLoadingControl imgCtl = d as ImageLoadingControl;
            imgCtl.imageShow.Stretch = (Stretch)e.NewValue;
        }

        private void GetImage(string token = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(path))
                {

                    if (path.StartsWith("http"))
                    {
                        this.story.Begin();
                        imageLoading.Visibility = Visibility.Visible;
                        GetHttpImage(path, token);
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
            catch (Exception er)
            {
                imageShow.Source = null;
                Console.WriteLine("图片加载异常：" + er.ToString());
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
