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
using System.IO;
using System.Reflection;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;


namespace Umini.Test.mgh3326
{
    /// <summary>
    /// Interaction logic for Test_PlaylistParsing.xaml
    /// </summary>
    public partial class Test_PlaylistParsing : Window
    {
        static int hoho=0;
        public Test_PlaylistParsing()
        {
            InitializeComponent();
            TextBox.Text = "Play List 가져오는중..";
            PlaylistUpdates object1 = new PlaylistUpdates();
            object1.ohoh();
            while(true)
            {
                if (hoho == 1)
                    break;
            }
            TextBox.Text = object1.output;
            


        }
        class PlaylistUpdates
        {
            
            public String output = "11";
            

            [STAThread]
            public void ohoh()
            {
               output += "22";

            Console.WriteLine("YouTube Data API: Playlist Updates");
                Console.WriteLine("==================================");

                try
                {
                    new PlaylistUpdates().Run().Wait();
                }
                catch (AggregateException ex)
                {
                    foreach (var e in ex.InnerExceptions)
                    {
                        Console.WriteLine("Error: " + e.Message);
                        output = "Error: " + e.Message;

                    }
                }

                Console.WriteLine("Press any key to continue...");
                output += "Press any key to continue...";

                //Console.ReadKey();
            }
            
            private async Task Run()
            {
               
                output += "33";

                UserCredential credential;
                using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
                {
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        // This OAuth 2.0 access scope allows for full read/write access to the
                        // authenticated user's account.
                        new[] { YouTubeService.Scope.Youtube },
                        "user",
                        CancellationToken.None,
                        new FileDataStore(this.GetType().ToString())
                    );
                }

                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = this.GetType().ToString()
                });


                //////////////////////////////////////////////////////


                Console.WriteLine("start");
                var lRequest = youtubeService.Playlists.List("snippet");
                lRequest.Mine = true;
                var result = lRequest.Execute();

                //System.Diagnostics.Debugger.Break();
                //Console.WriteLine(result.Items.Count);
                for (int i = 0; i < result.Items.Count; i++)
                {
                    //if (result.Items[i].Snippet.Title == "Favorites")//내꺼에서 오류나서 일단 지움
                    //    Console.WriteLine("Test");
                    Console.WriteLine("재생목록" + i + 1);
                    output += "재생목록" + i + 1;
                    Console.WriteLine("제목 : " + result.Items[i].Snippet.Title);
                    output += "제목 : " + result.Items[i].Snippet.Title;
                    var resItem = youtubeService.PlaylistItems.List("snippet");
                    resItem.PlaylistId = result.Items[i].Id;
                    var resitem2 = resItem.Execute();
                    //Console.WriteLine(resitem2.Items.Count);
                    //Console.WriteLine(result.Items[i].Snippet.Title);
                    for (int j = 0; j < resitem2.Items.Count; j++)
                    {
                        Console.WriteLine(resitem2.Items[j].Snippet.Title);
                        output += resitem2.Items[j].Snippet.Title;
                    }

                }
                output += "complete";
                hoho = 1;
                

    //Console.WriteLine(resitem2.Items[1].Snippet.Title);
    

                //System.Diagnostics.Debugger.Break();


                //Console.WriteLine("start2");
                //var listRequest = youtubeService.PlaylistItems.List("snippet");
                //listRequest.PlaylistId = "노래";
                //var result = listRequest.Execute();



                // var searchListResponse = await listRequest.ExecuteAsync();

                //Console.WriteLine("list :: {0} !!", result);

                //// Create a new, private playlist in the authorized user's channel.
                //var newPlaylist = new Playlist();
                //newPlaylist.Snippet = new PlaylistSnippet();
                //newPlaylist.Snippet.Title = "Test Playlist";
                //newPlaylist.Snippet.Description = "A playlist created with the YouTube API v3";
                //newPlaylist.Status = new PlaylistStatus();
                //newPlaylist.Status.PrivacyStatus = "public";
                //newPlaylist = await youtubeService.Playlists.Insert(newPlaylist, "snippet,status").ExecuteAsync();

                //// Add a video to the newly created playlist.
                //var newPlaylistItem = new PlaylistItem();
                //newPlaylistItem.Snippet = new PlaylistItemSnippet();
                //newPlaylistItem.Snippet.PlaylistId = newPlaylist.Id;
                //newPlaylistItem.Snippet.ResourceId = new ResourceId();
                //newPlaylistItem.Snippet.ResourceId.Kind = "youtube#video";
                //newPlaylistItem.Snippet.ResourceId.VideoId = "GNRMeaz6QRI";
                //newPlaylistItem = await youtubeService.PlaylistItems.Insert(newPlaylistItem, "snippet").ExecuteAsync();

                //Console.WriteLine("Playlist item id {0} was added to playlist id {1}.", newPlaylistItem.Id, newPlaylist.Id);
            }
        }
    }
}
