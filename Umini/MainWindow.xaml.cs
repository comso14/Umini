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
    /// 

    public partial class MainWindow : Window
    {
        public NowPlayingList mNowPlayingList;
        public Test_play play;
        public Account account;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new WindowViewModel(this);

            play = new Test_play();
            mNowPlayingList = new NowPlayingList();

            play.video.MediaEnded += new RoutedEventHandler(MediaEnded);
            account = new Account();

            LoadAccount();
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
                lbTitleBar.Content = media.mTitle + " - " + media.mArtist;
                //txtLyric.Text = media.mLyric;
                Play();

            }
        }



        private void btn_Pause_Click(object sender, RoutedEventArgs e)
        {
            Pause();
        }


        private void btn_Next_Click(object sender, RoutedEventArgs e)
        {
            NextPlay();
        }

        private void btn_Prev_Click(object sender, RoutedEventArgs e)
        {
            PrevPlay();
        }
        private void AppWindow_Loaded(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new PlaylistPage());
        }


        public void Play()
        {
            play.Music_Open(mNowPlayingList.mMediaList[mNowPlayingList.mNowPlayingOrder].mPath);
            play.video.Position = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(mNowPlayingList.mNowMediaPosition));
            mNowPlayingList.mIsPlay = true;
        }

        public void Pause()
        {
            mNowPlayingList.mNowMediaPosition = play.CurPosition();
            mNowPlayingList.mIsPlay = false;
            play.VideoPause();
        }
        public void Stop()
        {
            play.video.Stop();
            mNowPlayingList.mIsPlay = false;
        }

        public void NextPlay()
        {
            if (++mNowPlayingList.mNowPlayingOrder == mNowPlayingList.mMediaList.Count)
                mNowPlayingList.mNowPlayingOrder = 0;
            if (mNowPlayingList.mMediaList[mNowPlayingList.mNowPlayingOrder].mPath != null)
            {
                mNowPlayingList.mNowMediaPosition = 0;
                play.Music_Open(mNowPlayingList.mMediaList[mNowPlayingList.mNowPlayingOrder].mPath);

            }
        }

        public void PrevPlay()
        {
            if (--mNowPlayingList.mNowPlayingOrder < 0)
                mNowPlayingList.mNowPlayingOrder = mNowPlayingList.mMediaList.Count - 1;
            if (mNowPlayingList.mMediaList[mNowPlayingList.mNowPlayingOrder].mPath != null)
            {
                mNowPlayingList.mNowMediaPosition = 0;
                play.Music_Open(mNowPlayingList.mMediaList[mNowPlayingList.mNowPlayingOrder].mPath);
            }
        }
        public void MediaEnded(object sender, RoutedEventArgs e)
        {
            NextPlay();

        }

        /// <summary>
        /// load account profile in ./profile/[account-name].json
        /// return no;
        /// </summary>
        public void LoadAccount()//이거 파라미터가 들어온다면 다르게 폴더 이름이 달리질듯 하오
        {
            String path;
            ImportExport importExport = new ImportExport();
            path = importExport.makeFolder("profile");
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
            if (di.GetFiles().Length > 0)
            {
                foreach (System.IO.FileInfo File in di.GetFiles())
                {

                    if (File.Extension.ToLower().CompareTo(".json") == 0)
                    {
                        String FileNameOnly = File.Name.Substring(0, File.Name.Length - 5);
                        String FullFileName = File.FullName;
                        if(FileNameOnly.Equals(account.mID))
                        {
                            account = importExport.importAccount(account.mID);
                            return;
                        }
                    }
                }
                //ㅠㅠ 디포트가 없으면 여길로 오겠구먼
                if(account.mID=="")
                {
                    account.mID = "default";
                }
                importExport.exportAccount(account);
            }
            else//아무것도 없을때
            {
                //MessageBox.Show("default.json 파일이 없습니다. 그래서 json 파일을 만들겠습니다.");
                account.mID = "default";
                importExport.exportAccount(account);
            }

        }

        private void btnAccount_Click(object sender, RoutedEventArgs e)
        {
            new AccountWindow().Show();

        }
        
    }

}


