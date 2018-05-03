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

namespace Umini.Test.hhhh24
{
    /// <summary>
    /// Test_play.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Test_play : Window
    {
        TimeSpan t;
        //DispatcherTimer timer == new DispatcherTimer();
        public Test_play()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            
           // string path = @"C:\Users\Kwang1\Desktop\123.mp4";
           // video.Source = new Uri(@"C:\Users\Kwang1\Desktop\123.mp4", UriKind.RelativeOrAbsolute);
            
           // video.Play();
            


            // ---------youtube영상 embed하는방법-------
            //    var ytb = new YouTubeService(new BaseClientService.Initializer()
            //    {
            //        ApiKey = "AIzaSyB2oz5IFRID3wtpe5v7Sa1KQWst1zBIqBc",
            //        ApplicationName = "My tube"
            //    });

            //    string url = txtbox.Text;
            //    string[] sp = url.Split('=');
            //    string id = sp[1];
            //    string embed = "<html><head>" + "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=Edge=c\"/>" +
            //                   "</head><body>" + "<iframe src=\"https://youtube.com/embed/" 
            //                   + id + "?autoplay=1&vq=light\"" +
            //                   "allow = \" encrypted-media\"></iframe>" +
            //                   "</body></html>";
            //    web.NavigateToString(embed);
            //------------------------------------------------
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog(this).GetValueOrDefault() == true)

            {

                video.Source = new Uri(ofd.FileName);
                video.LoadedBehavior = MediaState.Manual;
                video.UnloadedBehavior = MediaState.Manual;
                video.Play();
               
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            video.Play();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            video.Pause();
        }

        private void video_MediaOpened(object sender, RoutedEventArgs e)
        {
            t = video.NaturalDuration.TimeSpan;
            slide.Minimum = 0;
            slide.Maximum = t.TotalSeconds;
        }
      

        private void f5_Click(object sender, RoutedEventArgs e)
        {
            t = video.Position;
            time.Text = t.ToString();
        }

        private void slide_LostMouseCapture(object sender, MouseEventArgs e)
        {
            int pos = Convert.ToInt32(slide.Value);
            video.Position = new TimeSpan(0, 0, 0, pos, 0);
            time.Text = (new TimeSpan(0, 0, 0, pos, 0)).ToString();
        }
    }
}
