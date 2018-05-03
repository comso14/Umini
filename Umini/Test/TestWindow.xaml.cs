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
using System.Diagnostics;

namespace Umini.Test
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();
        }

        private void btn_BasicLayout_Click(object sender, RoutedEventArgs e)
        {
            Window window = new bearics.Test_BasicLayout();
            window.Show();
        }

        private void btn_PlaylistParsing_Click(object sender, RoutedEventArgs e)
        {
            Window window = new mgh3326.Test_PlaylistParsing();
            window.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window win = new hhhh24.Test_play();
            win.Show();
        }

        private void btn_optionFunction_Click(object sender, RoutedEventArgs e)
        {
            Window window = new junpil.Test_optionFunction();
            window.Show();
        }
    }
}
