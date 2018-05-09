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
using Umini.Test.junpil;
using Umini.ViewModel;
using Umini.Test;
using Player;
using Umini.Test.mgh3326;
using System.Web;
using Umini.Test.hhhh24;


namespace Umini
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new WindowViewModel(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window window = new TestWindow();
            window.Show();
        }

        private void btnURLAdd_Click(object sender, RoutedEventArgs e)
        {
            Test_PlaylistParsing parsing = new Test_PlaylistParsing();
            Youtube youtube = parsing.ParsingYoutube(txtUrl.Text);
            MessageBox.Show(youtube.mTitle);
            playlist.AppendText(youtube.mTitle+"\n");

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Test_play play = new Test_play();
            play.YoutubePlay(txtUrl.Text);
        }
    }

}


