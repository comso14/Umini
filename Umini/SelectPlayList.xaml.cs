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
using Player;

namespace Umini
{
    /// <summary>
    /// SelectPlayList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SelectPlayList : Window
    {
        MainWindow mw;
        string _youtubeID;
        public SelectPlayList()
        {
            InitializeComponent();
            mw = (MainWindow)Application.Current.MainWindow;

            for(int i=0;i <  mw.mAccount.mPlaylistList.Count;i++)
            {
                listviewPlayList.Items.Add(mw.mAccount.mPlaylistList[i].mName);
            }
        }
        public SelectPlayList(string youtubeID)
        {
            InitializeComponent();
            mw = (MainWindow)Application.Current.MainWindow;

            for (int i = 0; i < mw.mAccount.mPlaylistList.Count; i++)
            {
                listviewPlayList.Items.Add(mw.mAccount.mPlaylistList[i].mName);
            }
            _youtubeID = youtubeID;
        }
        public Playlist FindPlaylist(string name)
        {
            //Playlist mfile = new Playlist();

            return mw.mAccount.mPlaylistList.FirstOrDefault(x => x.mName.Equals(name));
        }
        private void listviewPlayList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listviewPlayList.SelectedItems.Count == 1)
            {
                var items = listviewPlayList.SelectedItems;

                MessageBox.Show(_youtubeID+"을 "+items[0].ToString()+"넣었습니당");
                Playlist mfile = new Playlist();
                mfile= FindPlaylist(items[0].ToString());
                MessageBox.Show("forDebugingTest");

                //mw.mAccount.mPlaylistList
            }
        }
    }
}
