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
      
        MainWindow mw;

        public PlaylistPage()
        {
            InitializeComponent();
            mw = (MainWindow)Application.Current.MainWindow;
            dgPlaylist.ItemsSource = mw.mNowPlayingList.mMediaList;
        }
       
        private void btnURLAdd_Click(object sender, RoutedEventArgs e)
        {
            ParsingYoutube(txtUrl.Text);
            string link = txtUrl.Text;
            Thread downloadThread = new Thread(() => DownloadYoutube(link));
            downloadThread.Start();
            dgPlaylist.Items.Refresh();
        }
       
        /// <summary>
        /// Parsing youtube media and add information to youtube class
        /// </summary>
        /// <param name="link"></param>
        private void ParsingYoutube(string link)
        {
            Test_PlaylistParsing parsing = new Test_PlaylistParsing();

            Youtube youtube = parsing.ParsingYoutube(txtUrl.Text);  // parsing part

            MessageBox.Show(youtube.mTitle);
            mw.mNowPlayingList.mMediaList.Add((MediaFile)youtube);
      
        }


        /// <summary>
        /// using https://github.com/i3arnon/libvideo
        /// NuGet download : Install-Package VideoLibrary
        /// </summary>
        /// <param name="link"></param>
        private void DownloadYoutube(string link)
        {

            var youTube = YouTube.Default; // starting point for YouTube actions
            var videos = youTube.GetAllVideos(link);
            var video = videos.FirstOrDefault(v => v.Resolution == 144); // gets a Video object with info about the video

            if (video == null)  // If there is no resolution, set default resolution.
                video = youTube.GetVideo(link);

            ImportExport ie = new ImportExport();
            string path  = System.IO.Path.Combine(ie.makeFolder("videotmp"), link.Split('=').Last() + ".mp4");

            File.WriteAllBytes(path, video.GetBytes()); // save media file
            int index = mw.mNowPlayingList.mMediaList.FindIndex(s => ((Youtube)s).mURL.Contains(link));
            mw.mNowPlayingList.mMediaList[index].mPath = path;

            /*
            var item = playlist.Items.FindItemWithText(mw.mNowPlayingList.mMediaList[index].mTitle);
            int itemNum = playlist         .Items.IndexOf();
            ListViewItem item = (ListViewItem)playlist.Items[itemNum];
            */

            return;
        }
    }

}
