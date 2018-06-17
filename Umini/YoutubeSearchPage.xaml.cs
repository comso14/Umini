using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Umini
{
    /// <summary>
    /// Interaction logic for YoutubeSearchPage.xaml
    /// </summary>
    public partial class YoutubeSearchPage : Page
    {
        MainWindow mw;
        public YoutubeSearchPage()
        {
            InitializeComponent();
            mw = (MainWindow)Application.Current.MainWindow;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            YoutubePlaySearch(txtTitle.Text).Wait();

        }
        private async Task YoutubePlaySearch(string youtubeIDorName)
        {
            UserCredential credential;
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { YouTubeService.Scope.Youtube },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString())
                );
            }


            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                //ApiKey = "REPLACE_ME",
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = youtubeIDorName;// Replace with your search term.
            searchListRequest.MaxResults = 20;//50->1

            var searchListResponse = searchListRequest.Execute();
            listviewSearchList.Items.Clear();
            for (int i = 0; i < searchListResponse.Items.Count; i++)
            {

                listviewSearchList.Items.Add(new { searchListResponse.Items[i].Snippet.Title, searchListResponse.Items[i].Id.VideoId });


            }
        }

        private void listviewSearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listviewSearchList.SelectedItems.Count == 1)
            {
                var items = listviewSearchList.SelectedItems;

                //MessageBox.Show("https://www.youtube.com/watch?v=" + items[0].GetType().GetProperty("VideoId").GetValue(items[0], null).ToString());

                SelectPlayList selectPlayList = new SelectPlayList("https://www.youtube.com/watch?v=" + items[0].GetType().GetProperty("VideoId").GetValue(items[0], null).ToString());
                selectPlayList.Show();
                //mw.mAccount.mPlaylistList
            }
        }
    }
}
