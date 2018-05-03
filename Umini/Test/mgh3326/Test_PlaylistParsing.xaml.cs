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
using HtmlAgilityPack;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Xml;

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
                TextBox1.AppendText("재생목록" + (i + 1) + "\r\n");

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
                    TextBox1.AppendText(resitem2.Items[j].Snippet.Title + "\r\n");

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
            resItem.PlaylistId = result.Items[num - 1].Id;
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
            if (TextBoxPlayListNumber.Text == "")
            {
                MessageBox.Show("텍스트박스가 비었습니다. 채우고 입력해주세요");
                return;
            }

            Run(Int32.Parse(TextBoxPlayListNumber.Text)).Wait();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //var youtube_name = "오마이걸 반하나 - 바나나 알러지 원숭이 [세로라이브] OH MY GIRL BANHANA - Banana allergy monkey";

            var youtube_name = TextBoxPlay.Text;
            var url = Uri.EscapeUriString(@"http://search.api.mnet.com/search/totalweb?q=" + youtube_name + "&sort=r&callback=angular.callbacks._0");//인코딩?

            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            string doc = "";
            doc = wc.DownloadString(url);
            wc.Dispose();//이게 해제인가
            //doc = doc.Substring(21);
            doc = doc.Substring(21, doc.Length - 1 - 21);

            JObject obj = JObject.Parse(doc);
            Console.WriteLine(obj["message"].ToString());

            if (obj["resultCode"].ToString() == "S0000" || obj["message"].ToString() == "성공")
            {
                if (obj["info"]["songcnt"].ToString() != "0")
                {
                    string song_name = obj["data"]["songlist"][0]["songnm"].ToString();
                    string album_mname = obj["data"]["songlist"][0]["albumnm"].ToString();
                    string articst_name = obj["data"]["songlist"][0]["ARTIST_NMS"].ToString();
                    //TextBox1.Text = "";
                    //TextBox1.AppendText("song_name : " + song_name + "\r\n");

                    //TextBox1.AppendText("album_mname : " + album_mname + "\r\n");
                    //TextBox1.AppendText("articst_name : " + articst_name + "\r\n");
                    TextBoxPlayname.Text = song_name;
                    TextBoxPlayartist.Text = articst_name;

                }
                else
                {
                    TextBox1.Text = "검색결과가 없습니다.";
                }
            }
            else
            {
                TextBox1.Text = "";
                TextBox1.AppendText("실패입니다.\r\n");
            }
            //TextBox1.Text = doc2.DocumentNode.OuterHtml;

        }

        [STAThread]
        private void Buttonlyric_Click(object sender, RoutedEventArgs e)
        {
            TextBox1.Text = "가사를 가져오는중입니다.\r\n";
            try
            {
                Run_lyric().Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var ee in ex.InnerExceptions)
                {
                    TextBox1.Text = "Error: " + ee.Message;
                }
            }
            //TextBox1.AppendText("선택할 재생목록을 입력해주세요\r\n");
        }

        private async Task Run_lyric()
        {
            
            String callUrl = "http://lyrics.alsong.co.kr/alsongwebservice/service1.asmx";

            string title = TextBoxPlayname.Text;
            string artist = TextBoxPlayartist.Text;

            //String postData = "<?xml version=" + '\u0022' + "1.0" + '\u0022' + " encoding=" + '\u0022' + "UTF-8" + '\u0022' + "?>" + "<SOAP-ENV:Envelope  xmlns:SOAP-ENV=" + '\u0022' + "http://www.w3.org/2003/05/soap-envelope" + '\u0022' + " xmlns:SOAP-ENC=" + '\u0022' + "http://www.w3.org/2003/05/soap-encoding" + '\u0022' + " xmlns:xsi=" + '\u0022' + "http://www.w3.org/2001/XMLSchema-instance" + '\u0022' + " xmlns:xsd=" + '\u0022' + "http://www.w3.org/2001/XMLSchema" + '\u0022' + " xmlns:ns2=" + '\u0022' + "ALSongWebServer/Service1Soap" + '\u0022' + " xmlns:ns1=" + '\u0022' + "ALSongWebServer" + '\u0022' + " xmlns:ns3=" + '\u0022' + "ALSongWebServer/Service1Soap12" + '\u0022' + ">" + "<SOAP-ENV:Body>" + "<ns1:GetLyric5>" + "<ns1:stQuery>" + "<ns1:strChecksum>" + musicmd5 + "</ns1:strChecksum>" + "<ns1:strVersion>3.36</ns1:strVersion>" + "<ns1:strMACAddress>00ff667f9a08</ns1:strMACAddress>" + "<ns1:strIPAddress>xxx.xxx.xxx.xxx</ns1:strIPAddress>" + "</ns1:stQuery>" + "</ns1:GetLyric5>" + "</SOAP-ENV:Body>" + "</SOAP-ENV:Envelope>";
            String xml_string = "<?xml version=" + '\u0022' + "1.0" + '\u0022' + " encoding=" + '\u0022' + "UTF-8" + '\u0022' + "?><SOAP-ENV:Envelope xmlns:SOAP-ENV=" + '\u0022' + "http://www.w3.org/2003/05/soap-envelope" + '\u0022' + " xmlns:SOAP-ENC=" + '\u0022' + "http://www.w3.org/2003/05/soap-encoding" + '\u0022' + " xmlns:xsi=" + '\u0022' + "http://www.w3.org/2001/XMLSchema-instance" + '\u0022' + " xmlns:xsd=" + '\u0022' + "http://www.w3.org/2001/XMLSchema" + '\u0022' + " xmlns:ns2=" + '\u0022' + "ALSongWebServer/Service1Soap" + '\u0022' + " xmlns:ns1=" + '\u0022' + "ALSongWebServer" + '\u0022' + " xmlns:ns3=" + '\u0022' + "ALSongWebServer/Service1Soap12" + '\u0022' + "><SOAP-ENV:Body><ns1:GetResembleLyric2><ns1:stQuery><ns1:strTitle>" + title + "</ns1:strTitle><ns1:strArtistName>" + artist + "</ns1:strArtistName><ns1:nCurPage>0</ns1:nCurPage></ns1:stQuery></ns1:GetResembleLyric2></SOAP-ENV:Body></SOAP-ENV:Envelope>";

            //Console.WriteLine("{0}", xml_string);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(callUrl);
            // 인코딩 UTF-8
            byte[] sendData = UTF8Encoding.UTF8.GetBytes(xml_string);
            httpWebRequest.ContentType = "application/soap+xml; charset=UTF-8";
            httpWebRequest.UserAgent = "gSOAP/2.7";
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentLength = sendData.Length;
            Stream requestStream = httpWebRequest.GetRequestStream();
            requestStream.Write(sendData, 0, sendData.Length);
            requestStream.Close();
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            string respone = streamReader.ReadToEnd();
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(respone);
            //이거 아직 예외 처리 안함
            XmlNodeList xnList = xdoc.SelectNodes("Body/GetResembleLyric2Response/GetResembleLyric2Result/ST_GET_RESEMBLELYRIC2_RETURN");
            string alsong_name = xdoc.ChildNodes[1].FirstChild.FirstChild.FirstChild.FirstChild["strTitle"].InnerText;
            string alsong_artist = xdoc.ChildNodes[1].FirstChild.FirstChild.FirstChild.FirstChild["strArtistName"].InnerText;
            string lyric = xdoc.ChildNodes[1].FirstChild.FirstChild.FirstChild.FirstChild["strLyric"].InnerText;

            //Console.WriteLine(lyric);
            string[] tokens = lyric.Split(new[] { "<br>" }, StringSplitOptions.None);
            TextBox1.Text = "";
            foreach (var word in tokens)
            {
                TextBox1.AppendText(word+"\r\n");

                //Console.WriteLine(word);
            }
        }

    }
}
