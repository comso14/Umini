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
        static int hoho = 0;
        public Test_PlaylistParsing()
        {
            InitializeComponent();
            TextBox1.Text = "Update 버튼을 눌러주세요 처음에는 로그인을 해야되서 로그인 후에 프로그램을 재실행 해주시면 감사하겠습니다.\r\n";
            //PlaylistUpdates object1 = new PlaylistUpdates();
            //object1.ohoh();
            //while(true)
            //{
            //    if (hoho == 1)
            //        break;
            //}
            //TextBox.Text = object1.output;
        }

        //class PlaylistUpdates : Test_PlaylistParsing
        //{



        [STAThread]
        private void Button_update_Click(object sender, RoutedEventArgs e)
        {
            
            TextBox1.Text = "YouTube Data API: Playlist Updates\r\n";
            try
            {
                Run().Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var ee in ex.InnerExceptions)
                {
                    TextBox1.Text = "Error: " + ee.Message;
                }
            }
            TextBox1.AppendText("선택할 재생목록을 입력해주세요\r\n");
            
            //Console.ReadKey();
        }
        private async Task Run()
        {
            TextBox1.AppendText("Run Start\r\n");
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
                TextBox1.AppendText("재생목록" + (i + 1)+"\r\n");

                Console.WriteLine("제목 : " + result.Items[i].Snippet.Title);
                TextBox1.AppendText("제목 : " + result.Items[i].Snippet.Title + "\r\n");
                var resItem = youtubeService.PlaylistItems.List("snippet");
                resItem.PlaylistId = result.Items[i].Id;
                var resitem2 = resItem.Execute();
                //Console.WriteLine(resitem2.Items.Count);
                //Console.WriteLine(result.Items[i].Snippet.Title);
                for (int j = 0; j < resitem2.Items.Count; j++)
                {
                    Console.WriteLine(resitem2.Items[j].Snippet.Title);
                    TextBox1.AppendText(resitem2.Items[j].Snippet.Title+"\r\n");

                }
            }
        }
        private async Task Run(int num)
        {
            TextBox1.Text = "";
            
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
            
                //if (result.Items[i].Snippet.Title == "Favorites")//내꺼에서 오류나서 일단 지움
                //    Console.WriteLine("Test");
               
                var resItem = youtubeService.PlaylistItems.List("snippet");
                resItem.PlaylistId = result.Items[num-1].Id;
                var resitem2 = resItem.Execute();
                //Console.WriteLine(resitem2.Items.Count);
                //Console.WriteLine(result.Items[i].Snippet.Title);
                for (int j = 0; j < resitem2.Items.Count; j++)
                {
                    Console.WriteLine(resitem2.Items[j].Snippet.Title);
                    TextBox1.AppendText(resitem2.Items[j].Snippet.Title + "\r\n");

                }
            
        }
        private void ButtonPlayListNumberSelect_Click(object sender, RoutedEventArgs e)
        {
            if(TextBoxPlayListNumber.Text=="")
            {
                MessageBox.Show("텍스트박스가 비었습니다. 채우고 입력해주세요");
                return;
            }
            
            Run(Int32.Parse(TextBoxPlayListNumber.Text)).Wait();


        }
    }
}
