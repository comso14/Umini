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
using System.Threading;
using System.Windows.Threading;
using Umini;
using Umini.Test.mgh3326;
using VideoLibrary;
using Player;
using System.IO;
using System.Net;
using System.Globalization;
using System.Drawing;
using System.Windows.Interop;

namespace Umini
{
   
    /// <summary>
    /// Interaction logic for PlaylistPage.xaml
    /// </summary>
    public partial class PlaylistPage : Page
    {
      
        MainWindow mw = (MainWindow)Application.Current.MainWindow;
        Playlist playlist;

        public PlaylistPage()
        {
            InitializeComponent();
            dgPlaylist.ItemsSource = mw.mAccount.mNowPlayingList.mMediaList;

        }



        public PlaylistPage(Playlist _playlist)
        {
            InitializeComponent();
            playlist = _playlist;
            dgPlaylist.ItemsSource = playlist.mMediaList;
        }

        private void btnURLAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!txtUrl.Text.Contains("watch?v="))
            {
                MessageBox.Show("Youtube 영상 주소를 확인 해주세요 ");
                return;
            }
            ParsingYoutube(txtUrl.Text, playlist);
            string link = txtUrl.Text;
            Thread downloadThread = new Thread(() => DownloadYoutube(link, playlist));
            downloadThread.Start();
            dgPlaylist.Items.Refresh();
        }

        /// <summary>
        /// Parsing youtube media and add information to youtube class
        /// </summary>
        /// <param name="link"></param>
        public  void ParsingYoutube(string link, Playlist playlist)
        {
            Test_PlaylistParsing parsing = new Test_PlaylistParsing();

            Youtube youtube = parsing.ParsingYoutube(link);  // parsing part

            MessageBox.Show(youtube.mTitle);
            playlist.mMediaList.Add((MediaFile)youtube);
      
        }


        /// <summary>
        /// using https://github.com/i3arnon/libvideo
        /// NuGet download : Install-Package VideoLibrary
        /// </summary>
        /// <param name="link"></param>
        public void DownloadYoutube(string link, Playlist playlist)
        {

            var youTube = YouTube.Default; // starting point for YouTube actions
            var videos = youTube.GetAllVideos(link);
            var video = videos.FirstOrDefault(v => v.Resolution == 144); // gets a Video object with info about the video

            if (video == null)  // If there is no resolution, set default resolution.
                video = youTube.GetVideo(link);

            ImportExport ie = new ImportExport();
            string path  = System.IO.Path.Combine(ie.makeFolder("videotmp"), link.Split('=').Last() + ".mp4");

            System.IO.FileInfo fi = new System.IO.FileInfo(path);
            if (fi.Exists)
            {
                //MessageBox.Show("이미 있다리");
            }
            else
            {
                // 파일 없음.
                File.WriteAllBytes(path, video.GetBytes()); // save media file

            }


            int index = playlist.mMediaList.FindIndex(s => ((Youtube)s).mURL.Contains(link));
            playlist.mMediaList[index].mPath = path;

            /*
            var item = playlist.Items.FindItemWithText(mw.mNowPlayingList.mMediaList[index].mTitle);
            int itemNum = playlist         .Items.IndexOf();
            ListViewItem item = (ListViewItem)playlist.Items[itemNum];
            */

            return;
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            MediaFile media = row.DataContext as MediaFile;
            mw.mAccount.mNowPlayingList.mMediaList.Add(media);
        }

        private void DataGridRow_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            MediaFile media = row.DataContext as MediaFile;
            DetailMediaWindow dmw = new DetailMediaWindow(media);
            dmw.Show();
        }
    }

}
