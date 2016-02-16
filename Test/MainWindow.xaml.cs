using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string imageUrl1;

        public string ImageUrl1
        {
            get { return imageUrl1; }
            set 
            {
                imageUrl1 = value;
                OnPropertyChanged("ImageUrl1");
            }
        }

        private string imageUrl2;

        public string ImageUrl2
        {
            get { return imageUrl2; }
            set
            {
                imageUrl2 = value;
                OnPropertyChanged("ImageUrl2");
            }
        }

        private string imageUrl3;

        public string ImageUrl3
        {
            get { return imageUrl3; }
            set
            {
                imageUrl3 = value;
                OnPropertyChanged("ImageUrl3");
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

       
        private void button_Click(object sender, RoutedEventArgs e)
        {
            ImageUrl1 = @"https://pixabay.com/static/uploads/photo/2016/02/09/13/45/rock-carvings-1189288_960_720.jpg";
            ImageUrl2 = @"https://pixabay.com/static/uploads/photo/2016/02/14/14/32/construction-1199586_960_720.jpg";
            ImageUrl3 = @"c:\test.jpg";
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
