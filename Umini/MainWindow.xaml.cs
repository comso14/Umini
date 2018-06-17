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
using System.Windows.Interop;
using System.Threading;
using System.Windows.Threading;
using VideoLibrary;
using System.IO;
using System.Runtime.InteropServices;



namespace Umini
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        private static extern int RegisterHotKey(int hwnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        private static extern int UnregisterHotKey(int hwnd, int id); 
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam); //시스템 볼륨 조절
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;

        public NowPlayingList mNowPlayingList;
        public Test_play play;
        public System.Windows.Forms.NotifyIcon notify;
        public MainWindow()
        {
            InitializeComponent();
            HotKey _hotkey;
            
            Loaded += (s, e) =>   //핫키등록
            {
                _hotkey = new HotKey(ModifierKeys.Control | ModifierKeys.Alt, Keys.F1, this);
                _hotkey.HotKeyPressed += (k) => VolDown();
                _hotkey = new HotKey(ModifierKeys.Control | ModifierKeys.Alt, Keys.F2, this);
                _hotkey.HotKeyPressed += (k) => VolUp();
                _hotkey = new HotKey(ModifierKeys.Control | ModifierKeys.Alt, Keys.F3, this);
                _hotkey.HotKeyPressed += (k) => Mute();
                _hotkey = new HotKey(ModifierKeys.Control | ModifierKeys.Alt, Keys.F4, this);
                _hotkey.HotKeyPressed += (k) => PrevPlay();
                _hotkey = new HotKey(ModifierKeys.Control | ModifierKeys.Alt, Keys.F6, this);
                    _hotkey.HotKeyPressed += (k) => Pause();
                _hotkey = new HotKey(ModifierKeys.Control | ModifierKeys.Alt, Keys.F5, this);
                _hotkey.HotKeyPressed += (k) => Play();
                _hotkey = new HotKey(ModifierKeys.Control | ModifierKeys.Alt, Keys.F7, this);
                _hotkey.HotKeyPressed += (k) => NextPlay();
            };

            DataContext = new WindowViewModel(this);

            play = new Test_play();
            mNowPlayingList = new NowPlayingList();

            play.video.MediaEnded += new RoutedEventHandler(MediaEnded);
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
            notify = new System.Windows.Forms.NotifyIcon();
            notify.Icon = Properties.Resources.Icon1;
            notify.Text = "Umini";
            notify.MouseClick += Notify_Click;
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
        private void Mute()
        {
            SendMessageW(new WindowInteropHelper(this).Handle, WM_APPCOMMAND, new WindowInteropHelper(this).Handle,
                (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }

        private void VolDown()
        {
            SendMessageW(new WindowInteropHelper(this).Handle, WM_APPCOMMAND, new WindowInteropHelper(this).Handle,
                (IntPtr)APPCOMMAND_VOLUME_DOWN);
        }

        private void VolUp()
        {
            SendMessageW(new WindowInteropHelper(this).Handle, WM_APPCOMMAND, new WindowInteropHelper(this).Handle,
                (IntPtr)APPCOMMAND_VOLUME_UP);
        }
   

 
        private void Notify_Click(object sender, EventArgs e) // 트레이 버튼 클릭
        {
            notify.Visible = false;
            this.Visibility = Visibility.Visible;
        }

        private void AppWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (notify.Visible == true) { }
            else
            {
                string messageBoxText = "트레이로 최소화 하시려면 Yes , 종료는 No입니다.";
                string caption = "트레이";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;


                MessageBoxResult messageBoxResult = MessageBox.Show(messageBoxText, caption, button);
                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes: // 트레이로 보냄
                        e.Cancel = true;
                        notify.Visible = true;
                        this.Visibility = Visibility.Hidden;
                        break;
                    case MessageBoxResult.No:
                        // 윈도우 종료
                        break;
                    case MessageBoxResult.Cancel: // 취소
                        e.Cancel = true;
                        break;
                }
            }
            return;
        }
    }

}


