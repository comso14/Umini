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

namespace Umini
{
    /// <summary>
    /// Interaction logic for NowPlayingPage.xaml
    /// </summary>
    public partial class NowPlayingPage : Page
    {
        MainWindow mw = (MainWindow)Application.Current.MainWindow;

        public NowPlayingPage()
        {
            InitializeComponent();
            dgPlaylist.ItemsSource = mw.mAccount.mNowPlayingList.mMediaList;
        }
    }
}
