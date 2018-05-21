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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Umini.Test.junpil;
using Umini.ViewModel;
using Umini.Test;
using Player;
using Umini.Test.mgh3326;
using System.Web;
using Umini.Test.hhhh24;
using System.Threading;
using System.Windows.Threading;
using VideoLibrary;
using System.IO;


namespace Umini
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>



    public partial class MainWindow : Window
    {
        public NowPlayingList mNowPlayingList;
        public Test_play play;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new WindowViewModel(this);

            play = new Test_play();
            mNowPlayingList = new NowPlayingList();
        }

        private void AppWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //frame.NavigationService.Navigate(new PlaylistPage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window window = new TestWindow();
            window.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BitmapImage bi = new BitmapImage(new Uri(mNowPlayingList.mMediaList[0].mImagePath, UriKind.RelativeOrAbsolute));
            album.Source = bi;
            MediaFile media = mNowPlayingList.mMediaList[0];
            play.Music_Open(media.mPath);
            txtNoPlayingInformation.AppendText("노래 제목 : " + media.mTitle + "\n가     수 : " + media.mArtist);
            txtLyric.AppendText(media.mLyric);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            play.VideoPause();

        }

    }

}


