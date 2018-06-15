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
using System.Net;
using Newtonsoft.Json.Linq;
using System.Xml;
using Player;
using System.Web;

namespace Umini.Test.mgh3326
{
    /// <summary>
    /// Interaction logic for Test_PlaylistParsing.xaml
    /// </summary>

    public partial class Test_PlaylistParsing : Window
    {
        string mYoutubeID = "";//유튜브 아이디
        string _mYoutbueTitle = "";//유튜브 제목
        string mMusicLyric = "";//가사
        string mMusicTitle = "";//노래 제목
        string mMusicArtist = "";//노래 가수
        string mMusicAlbumName = "";//앨범 이름
        string mMusicAlbumPictureUrl = "";//앨범 사진 url
        string mMusicGenre = "";//장르
        string mMusicYear = "";//출시 일자
        List<string> temp_list = new List<string>();//임시 전역변수

        /// <summary>
        /// 함수 대충
        /// </summary>
        /// <param name="url">이 파라미터는 유알엘 입니다.</param>
        /// <returns></returns>
        public Youtube ParsingYoutube(string url)
        {
            Youtube mfile = new Youtube();
            mfile.mURL = url;

            Uri myUri = new Uri(url);//id 반환
            mfile.mYoutubeID = HttpUtility.ParseQueryString(myUri.Query).Get("v");
            if (mfile.mYoutubeID[0] == '-')
            {
                mfile.mYoutubeID = "\\" + mfile.mYoutubeID;
            }
            //MessageBox.Show(mfile.mYoutubeID);
            Update(mfile);
            return mfile;
        }

        public int Update(Youtube mediaFile)//Youtube id를 넣어주고 함수를 호출하면 제목 가수 앨범 사진url 장르 출시일자 가사를 가져옴
        {
            //mediaFile.mYoutubeID
            try
            {
                YoutubePlaySearch(mediaFile.mYoutubeID).Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
            StringBuilder builder = new StringBuilder(_mYoutbueTitle);
            //builder.Replace("\"", "");//이거 왜 있지

            builder.Replace("Music Video", "");

            mediaFile.mYoutubeTitle = builder.ToString(); // Value of y is "Hello 1st 2nd world"
            //mediaFile.mYoutubeTitle = _mYoutbueTitle;//타이틀 받아오기
            int temp;
            temp = MnetParsing(mediaFile.mYoutubeTitle, out mMusicTitle, out mMusicAlbumName, out mMusicArtist, out mMusicAlbumPictureUrl, out mMusicGenre, out mMusicYear);
            mediaFile.mTitle = mMusicTitle;
            mediaFile.mAlbum = mMusicAlbumName;
            mediaFile.mImagePath = mMusicAlbumPictureUrl;
            mediaFile.mArtist = mMusicArtist;
            mediaFile.mGenre = mMusicGenre;
            mediaFile.mYear = mMusicYear;
            if (temp == 0)
            {
                AlsongParsing(mediaFile.mTitle, mediaFile.mArtist, out mMusicLyric);//알송 가사 파싱
                mediaFile.mLyric = mMusicLyric;
            }

            return 0; //true
            return 1;//false
        }
        private async Task YoutubePlaySearch(string youtubeIDorName)
        {
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
                //ApiKey = "REPLACE_ME",
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = youtubeIDorName;// Replace with your search term.
            searchListRequest.MaxResults = 1;//50->1

            // Call the search.list method to retrieve results matching the specified query term.
            //var searchListResponse = await searchListRequest.ExecuteAsync();
            var searchListResponse = searchListRequest.Execute();

            //List<string> videos = new List<string>();
            //List<string> channels = new List<string>();
            //List<string> playlists = new List<string>();
            //MessageBox.Show(String.Format("{0} ({1})", searchListResponse.Items[0].Snippet.Title, searchListResponse.Items[0].Id.VideoId));
            _mYoutbueTitle = searchListResponse.Items[0].Snippet.Title;

            //// Add each result to the appropriate list, and then display the lists of
            //// matching videos, channels, and playlists.
            //foreach (var searchResult in searchListResponse.Items)
            //{
            //    MessageBox.Show(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));

            //}

            //Console.WriteLine(String.Format("Videos:\n{0}\n", string.Join("\n", videos)));
            //Console.WriteLine(String.Format("Channels:\n{0}\n", string.Join("\n", channels)));
            //Console.WriteLine(String.Format("Playlists:\n{0}\n", string.Join("\n", playlists)));
        }
        int MnetParsing(string youtube_name, out string song_name, out string album_name, out string artist_name, out string image_path, out string genrenm_name, out string releaseymd_name)
        {
            //M / V

            //var youtube_name = TextBoxPlay.Text;
            var url = Uri.EscapeUriString(@"http://search.api.mnet.com/search/totalweb?q=" + youtube_name + "&sort=r");//인코딩?
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            string doc = "";
            doc = wc.DownloadString(url);
            wc.Dispose();//이게 해제인가
                         //doc = doc.Substring(21);

            JObject obj = JObject.Parse(doc);
            //Console.WriteLine(obj["message"].ToString());
            image_path = "";
            artist_name = "";
            album_name = "";
            song_name = "";
            genrenm_name = "";
            releaseymd_name = "";
            if (obj["resultCode"].ToString() == "S0000" || obj["message"].ToString() == "성공")
            {
                if (obj["info"]["songcnt"].ToString() != "0")
                {
                    song_name = obj["data"]["songlist"][0]["songnm"].ToString();
                    album_name = obj["data"]["songlist"][0]["albumnm"].ToString();
                    artist_name = obj["data"]["songlist"][0]["ARTIST_NMS"].ToString();
                    string album_id = obj["data"]["songlist"][0]["albumid"].ToString();
                    genrenm_name = obj["data"]["songlist"][0]["genrenm"].ToString();
                    releaseymd_name = obj["data"]["songlist"][0]["releaseymd"].ToString();
                    //장르 : genrenm 
                    //출시 : releaseymd
                    image_path = "";
                    if (album_id.Length == 7)
                    {
                        image_path = "http://cmsimg.mnet.com/clipimage/album/480/00" + album_id[0] + "/" + album_id.Substring(1, 3) + "/" + album_id + ".jpg";
                    }
                    else
                    {
                        image_path = "http://cmsimg.mnet.com/clipimage/album/480/000/" + album_id.Substring(0, 3) + "/" + album_id + ".jpg";
                    }


                    return 0;
                }
                else
                {
                    //TextBox1.Text = "검색결과가 없습니다.";

                    return -1;
                }
            }
            else
            {
                //TextBox1.Text = "";
                //TextBox1.AppendText("실패입니다.\r\n");
                return -1;
            }
        }

        int AlsongParsing(string title, string artist, out string lyric)
        {
            lyric = "가사를 찾지 못했습니다.";

            String callUrl = "http://lyrics.alsong.co.kr/alsongwebservice/service1.asmx";

            //string title = TextBoxPlayname.Text;
            //string artist = TextBoxPlayartist.Text;

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
            if (xdoc.ChildNodes[1].FirstChild.FirstChild.FirstChild.FirstChild == null)
                return -1;//예외처리
            string alsong_name = xdoc.ChildNodes[1].FirstChild.FirstChild.FirstChild.FirstChild["strTitle"].InnerText;
            string alsong_artist = xdoc.ChildNodes[1].FirstChild.FirstChild.FirstChild.FirstChild["strArtistName"].InnerText;
            string lyric_parsing = xdoc.ChildNodes[1].FirstChild.FirstChild.FirstChild.FirstChild["strLyric"].InnerText;
            string[] tokens = lyric_parsing.Split(new[] { "<br>" }, StringSplitOptions.None);
            lyric = "";
            foreach (var word in tokens)
            {
                //TextBox1.AppendText(word + "\r\n");
                lyric += (word + "\n");
            }

            return 0;//성공
            return -1;//실패
        }
        public Test_PlaylistParsing()
        {
            InitializeComponent();
            TextBox1.Text = "Update 버튼을 눌러주세요 처음에는 로그인을 해야되서 로그인 후에 프로그램을 재실행 해주시면 감사하겠습니다.\r\n";

        }
        [STAThread]
        private void Button_update_Click(object sender, RoutedEventArgs e)
        {

            TextBox1.Text = "YouTube Data API: Playlist Updates\r\n";
            try
            {
                YoutubePlylist().Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var ee in ex.InnerExceptions)
                {
                    TextBox1.Text = "Error: " + ee.Message;
                }
            }
            TextBox1.AppendText("선택할 재생목록을 입력해주세요\r\n");
        }
        private async Task YoutubePlylist()
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
            var lRequest = youtubeService.Playlists.List("snippet");
            lRequest.Mine = true;
            var result = lRequest.Execute();
            for (int i = 0; i < result.Items.Count; i++)
            {
                TextBox1.AppendText("======================\r\n재생목록" + (i + 1) + " " + result.Items[i].Snippet.Title + "\r\n");
                //TextBox1.AppendText("제목 : " + result.Items[i].Snippet.Title + "\r\n");
                var resItem = youtubeService.PlaylistItems.List("snippet");
                resItem.PlaylistId = result.Items[i].Id;
                var resitem2 = resItem.Execute();
                //Console.WriteLine(resitem2.Items.Count);
                //Console.WriteLine(result.Items[i].Snippet.Title);
                for (int j = 0; j < resitem2.Items.Count; j++)
                {
                    TextBox1.AppendText(resitem2.Items[j].Snippet.Title + "\r\n");
                }
                TextBox1.AppendText("======================\r\n");

            }
        }
        private async Task YoutubePlylist(int num)
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
            var lRequest = youtubeService.Playlists.List("snippet");
            lRequest.Mine = true;
            var result = lRequest.Execute();
            var resItem = youtubeService.PlaylistItems.List("snippet");
            resItem.PlaylistId = result.Items[num - 1].Id;
            var resitem2 = resItem.Execute();
            for (int j = 0; j < resitem2.Items.Count; j++)
            {
                //Console.WriteLine(resitem2.Items[j].Snippet.Title);
                //TextBox1.AppendText(resitem2.Items[j].Snippet.Title + "\r\n");

                //나와서
                foreach (string oh in temp_list)
                {
                    Youtube youtube = ParsingYoutube(oh);  // parsing part

                    //MessageBox.Show(youtube.mTitle);
                    TextBox1.AppendText(youtube.mTitle + "\n");
                }
                Youtube mfile = new Youtube();
                mfile.mURL = "https://www.youtube.com/watch?v" + resitem2.Items[j].Snippet.ResourceId.VideoId;
                mfile.mYoutubeID = resitem2.Items[j].Snippet.ResourceId.VideoId;
                if (mfile.mYoutubeID[0] == '-')
                {
                    mfile.mYoutubeID = "\\" + mfile.mYoutubeID;
                }
                //MessageBox.Show(mfile.mYoutubeID);
                Update(mfile);
                TextBox1.AppendText(mfile.mTitle + "\t" + mfile.mArtist + "\r\n");
                //mw.mNowPlayingList.mMediaList.Add((MediaFile)youtube);
            }

        }
        private void ButtonPlayListNumberSelect_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxPlayListNumber.Text == "")
            {
                MessageBox.Show("텍스트박스가 비었습니다. 채우고 입력해주세요");
                return;
            }
            YoutubePlylist(Int32.Parse(TextBoxPlayListNumber.Text)).Wait();

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
                    string album_id = obj["data"]["songlist"][0]["albumid"].ToString();
                    string image_path = "";
                    if (album_id.Length == 7)
                    {
                        image_path = "http://cmsimg.mnet.com/clipimage/album/480/00" + album_id[0] + "/" + album_id.Substring(1, 3) + "/" + album_id + ".jpg";
                    }
                    else
                    {
                        image_path = "http://cmsimg.mnet.com/clipimage/album/480/000/" + album_id.Substring(0, 3) + "/" + album_id + ".jpg";
                    }
                    if (image_path != "")
                    {
                        var image = new Image();
                        //var fullFilePath = @"http://www.americanlayout.com/wp/wp-content/uploads/2012/08/C-To-Go-300x300.png";
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(image_path, UriKind.Absolute);
                        bitmap.EndInit();
                        image_music.Source = bitmap;
                        //MessageBox.Show("이미지 로드 완료");
                        //wrapPanel1.Children.Add(image);
                    }
                    else
                    {
                        image_music.Source = null;
                    }
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
            string[] tokens = lyric.Split(new[] { "<br>" }, StringSplitOptions.None);
            TextBox1.Text = "";
            foreach (var word in tokens)
            {
                TextBox1.AppendText(word + "\r\n");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Youtube mfile = new Youtube();
            mfile.mYoutubeID = txtYoutubeId.Text;
            Update(mfile);
            MessageBox.Show("업데이트 완료!" + mfile.mYoutubeTitle);
            TextBox1.Text = mfile.mTitle + "\r\n" + mfile.mArtist + "\r\n" + mfile.mLyric + "\r\n";

            if (mfile.mImagePath != "")
            {
                //var fullFilePath = @"http://www.americanlayout.com/wp/wp-content/uploads/2012/08/C-To-Go-300x300.png";
                var image = new Image();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(mfile.mImagePath, UriKind.Absolute);
                bitmap.EndInit();
                image_music.Source = bitmap;
                //MessageBox.Show("이미지 로드 완료");
                //wrapPanel1.Children.Add(image);
            }
            else
            {
                image_music.Source = null;
            }
        }
    }
}