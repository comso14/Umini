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

        private void btn_Play_Click(object sender, RoutedEventArgs e)
        {
            if (mNowPlayingList.mMediaList[mNowPlayingList.mNowPlayingOrder].mPath != null)
            {
                BitmapImage bi = new BitmapImage(new Uri(mNowPlayingList.mMediaList[mNowPlayingList.mNowPlayingOrder].mImagePath, UriKind.RelativeOrAbsolute));
                album.Source = bi;
                MediaFile media = mNowPlayingList.mMediaList[mNowPlayingList.mNowPlayingOrder];
                play.Music_Open(mNowPlayingList.mMediaList[mNowPlayingList.mNowPlayingOrder].mPath);
                txtNoPlayingInformation.AppendText("노래 제목 : " + media.mTitle + "\n가     수 : " + media.mArtist);
                txtLyric.AppendText(media.mLyric);
                play.video.Position = new TimeSpan(0, 0, 0, 0,Convert.ToInt32(mNowPlayingList.mNowMediaPosition));
                mNowPlayingList.mIsPlay = true;
            }
        }

        private void btn_Pause_Click(object sender, RoutedEventArgs e)
        {
            Pause();
        }

        public void Pause()
        {
            mNowPlayingList.mNowMediaPosition = play.CurPosition();
            mNowPlayingList.mIsPlay = false;
            play.VideoPause();
        }



        private void btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            play.video.Stop();
            mNowPlayingList.mIsPlay = false;
        }

        private void btn_Next_Click(object sender, RoutedEventArgs e)
        {
            if (++mNowPlayingList.mNowPlayingOrder == mNowPlayingList.mMediaList.Count)
                mNowPlayingList.mNowPlayingOrder = 0;
            if (mNowPlayingList.mMediaList[mNowPlayingList.mNowPlayingOrder].mPath != null)
            {
                mNowPlayingList.mNowMediaPosition = 0;
                play.Music_Open(mNowPlayingList.mMediaList[mNowPlayingList.mNowPlayingOrder].mPath);
                
            }
        }

        private void btn_Prev_Click(object sender, RoutedEventArgs e)
        {
            if (--mNowPlayingList.mNowPlayingOrder < 0)
                mNowPlayingList.mNowPlayingOrder = mNowPlayingList.mMediaList.Count - 1;
            if (mNowPlayingList.mMediaList[mNowPlayingList.mNowPlayingOrder].mPath != null)
            {
                mNowPlayingList.mNowMediaPosition = 0;
                play.Music_Open(mNowPlayingList.mMediaList[mNowPlayingList.mNowPlayingOrder].mPath);
            }
        }
    }

}


