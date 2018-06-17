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
using Player;

namespace Umini
{
    /// <summary>
    /// Interaction logic for ManagePlaylistPage.xaml
    /// </summary>
    public partial class ManagePlaylistPage : Page
    {
        MainWindow mw = (MainWindow)Application.Current.MainWindow;

        public ManagePlaylistPage()
        {
            InitializeComponent();
            dgPlaylist.ItemsSource = mw.mAccount.mPlaylistList;
        }

        private void btnAddPlaylist_Click(object sender, RoutedEventArgs e)
        {
            mw.mAccount.mPlaylistList.Add(new Playlist() { mName = txtPlaylistName.Text });
            dgPlaylist.Items.Refresh();
            mw.tviPlaylist.Items.Add(new TreeViewItem() { Header = txtPlaylistName.Text });

        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            DataGridRow row = sender as DataGridRow;
            Playlist playlist = row.DataContext as Playlist;


            if (MessageBox.Show(playlist.mName+"을 삭제 하시겠습니까?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                mw.mAccount.mPlaylistList.Remove(playlist);
                dgPlaylist.Items.Refresh();
                foreach(TreeViewItem item in mw.tviPlaylist.Items)
                {
                    if(item.Header.ToString() == playlist.mName)
                    {
                        mw.tviPlaylist.Items.Remove(item);
                        break;
                    }
                }
                
            }


        }
    }
}
