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
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.Auth;
using Microsoft.Win32;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.IO;
using Player;

namespace Umini.Test.hhhh24
{
    /// <summary>
    /// Test_play.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Test_play : Window
    {
        TimeSpan t;
        bool mIsPlayed = false;
        DispatcherTimer mTimer = new DispatcherTimer();
        List<MediaFile> mfiles = new List<MediaFile>();

        public Test_play()
        {
            InitializeComponent();
            mTimer.Interval = TimeSpan.FromMilliseconds(1000);
            mTimer.Tick += new EventHandler(update);
            mTimer.Start();
           // CompositionTarget.Rendering += new EventHandler(Rendering_Update);
        }
        
        public void Music_Open(string path)
        {
            video.Source = new Uri(path);
            video.LoadedBehavior = MediaState.Manual;
            video.UnloadedBehavior = MediaState.Manual;

            FileInfo file = new FileInfo(path);
            MediaFile files = new MediaFile();

            string[] ext = path.Split('.');

            files.mExt = ext[ext.Length == 0 ? ext.Length - 1 : 0];
            files.mPath = path;
            files.mLength = Convert.ToUInt32(video.Position.TotalSeconds);

            if (files.mExt == "mp3")
            {

                
                using (TagLib.File mp3 = TagLib.File.Create(path))
                {
                    int k = 0;

                    files.mTitle = mp3.Tag.Title;
                    files.mAllbum = mp3.Tag.Album;
                    files.mYear = mp3.Tag.Year.ToString();
                    files.mTrack = Convert.ToInt32(mp3.Tag.Track);
                    while (true)
                    {
                        if (mp3.Tag.AlbumArtists[k] == null) //이거 나중에 다시 수정해야할듯
                            break;
                        files.mArtist += mp3.Tag.AlbumArtists[k++] + ", ";
                        //artists가 string이 아니고 string[]형식이라서 그냥 아티스트 전부 ,로 구분해서 한 string에 담기로함
                    }
                    k = 0;
                    while (true)
                    {
                        if (mp3.Tag.Genres[k] == null)
                            break;
                        files.mGenre += mp3.Tag.Genres[k++] + ", ";
                        //장르도 마찬가지
                    }
                    files.mcomment = mp3.Tag.Comment;
                    files.mLyric = mp3.Tag.Lyrics;
                }

            }
            
            mfiles.Add(files); // 다 저장된 하나의 mediafile 클래스를 리스트에 추가함\
            VideoPlay();
           
        }

        void update(object sender,EventArgs e)
        {
            Slider_Time.Value = video.Position.TotalSeconds;
            time.Text = new TimeSpan(0,0,(Convert.ToInt32(Slider_Time.Value))).ToString() + "/" + new TimeSpan(0, 0, Convert.ToInt32(t.TotalSeconds)); ;
            
        }
        
        public void YoutubePlay(string url)
        {
            var ytb = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyB2oz5IFRID3wtpe5v7Sa1KQWst1zBIqBc",
                ApplicationName = "My tube"
            });

            string[] sp = url.Split('=');
            string id = sp[1];
            string embed = "<html><head>" + "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=Edge=c\"/>" +
                           "</head><body>" + "<iframe src=\"https://youtube.com/embed/"
                           + id + "?autoplay=1&vq=light\"" +
                           "allow = \" encrypted-media\"></iframe>" +
                           "</body></html>";
            web.NavigateToString(embed);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            YoutubePlay(txtbox.Text);

        }
        public void VideoPlay()
        {
            btn_Play.Content = "||";
            video.Play();
            mIsPlayed = true;
            
        }
        public void VideoPause()
        {
            btn_Play.Content = ">";
            video.Pause();
            mIsPlayed = false;
        }
        

        private void video_MediaOpened(object sender, RoutedEventArgs e)
        {
            t = video.NaturalDuration.TimeSpan;
            Slider_Time.Minimum = 0;
            Slider_Time.Maximum = t.TotalSeconds;
            Slider_Time.IsMoveToPointEnabled = true;
        }
 
        private void Slider_Time_LostMouseCapture(object sender, MouseEventArgs e)
        {
            
            int pos = Convert.ToInt32(Slider_Time.Value);
            video.Position = new TimeSpan(0, 0, 0, pos);
            string curTime = (new TimeSpan(0, 0, 0, pos)).ToString() + "/" + new TimeSpan(0, 0, Convert.ToInt32(t.TotalSeconds));
            if (time.Text != curTime)
                time.Text = curTime;
        }
        private void Slider_Time_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                int pos = Convert.ToInt32(Slider_Time.Value);
                video.Position = new TimeSpan(0, 0, 0, pos);
                string curTime = (new TimeSpan(0, 0, 0, pos)).ToString() + "/" + new TimeSpan(0, 0, Convert.ToInt32(t.TotalSeconds));
                if (time.Text != curTime)
                    time.Text = curTime;
            }
        }
        private void btn_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog(this).GetValueOrDefault() == true)

            {
                //Music_Open(여기에 파일 path받아야함); 그리고 어디서받을지 몰라서 일단 여기 이벤트에넣어놨는데
                //노래 리스트에서 더블클릭같은 이벤트에서 Music_Open(path)가 발생해야할듯?? 잘모름..

                video.Source = new Uri(ofd.FileName);
                video.LoadedBehavior = MediaState.Manual;
                video.UnloadedBehavior = MediaState.Manual;

                VideoPlay();
                MessageBox.Show(ofd.FileName);
            }
        }

        public void btn_Play_Click(object sender, RoutedEventArgs e)
        {
            if (mIsPlayed)              
                VideoPause();
            else
                VideoPlay();
                
        }

        private void Slider_Volume_LostMouseCapture(object sender, MouseEventArgs e)
        {
            label_Vol.Content = Convert.ToInt32(Slider_Volume.Value * 100);
            video.Volume = Slider_Volume.Value;
        }
        private void Slider_Volume_OnMouseMove(object sender, MouseEventArgs e)
        {
            Slider_Volume.IsMoveToPointEnabled = true;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                label_Vol.Content = Convert.ToInt32(Slider_Volume.Value * 100);
                video.Volume = Slider_Volume.Value;
            }
        }
    }
}
