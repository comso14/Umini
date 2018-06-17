using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Player;

namespace Umini
{
    /// <summary>
    /// Interaction logic for DetailMediaWindow.xaml
    /// </summary>
    public partial class DetailMediaWindow : Window
    {
        public DetailMediaWindow()
        {
            InitializeComponent();
        }

        public DetailMediaWindow(MediaFile media)
        {
            InitializeComponent();
            UpdateInfo(media);
        }

        public void UpdateInfo(MediaFile media)
        {
            imgAlbum.Source = new BitmapImage(new Uri(media.mImagePath));
            lbAlbum.Content = media.mAlbum;
            lbArtist.Content = media.mArtist;
            lbTitle.Content = media.mTitle;
            txtLyric.Text = media.mLyric;
        }
    }
}
