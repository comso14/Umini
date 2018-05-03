using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.InteropServices;// Win32API사용
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading; // 타이머 사용
using System.Management; // 자동종료 사용
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows.Interop;
using System.Drawing;

namespace Umini
{
    /// <summary>
    /// Page1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Page1 : Page
    {
       
        public string a = "test ";
        public System.Windows.Forms.NotifyIcon notify;

        [DllImport("user32.dll")]

        private static extern int RegisterHotKey(int hwnd, int id, int fsModifiers, int vk); // 핫키 등록

        [DllImport("user32.dll")]

        private static extern int UnregisterHotKey(int hwnd, int id); // 핫키 제거

        System.Timers.Timer timer = new System.Timers.Timer();



        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }



        private new void PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (!(((Key.D0 <= e.Key) && (e.Key <= Key.D9))
                || ((Key.NumPad0 <= e.Key) && (e.Key <= Key.NumPad9))
                || e.Key == Key.Back || e.Key == Key.Right || e.Key == Key.Left || e.Key == Key.Up || e.Key == Key.Down))
            {
                System.Windows.MessageBox.Show("숫자만 입력 해주세요");
                e.Handled = true;
            }
            else
            {

            }

        }

        private void Button_Click(object sender, RoutedEventArgs e) // 알람 버튼
        {

            int inHour = Int32.Parse(Txtb1.Text);
            int inMin = Int32.Parse(Txtb3.Text);
            DateTime nowTime = DateTime.Now;
            DateTime desTime;
            desTime = nowTime;
            if (inHour != 0 && inMin != 0)
                timer.Interval = 1000 * (3600 * inHour) * (60 * inMin);
            else if (inHour == 0 && inMin != 0)
            {
                timer.Interval = 1000 * (60 * inMin);
            }
            else if (inMin == 0)
            {
                timer.Interval = 1000 * (3600 * inHour);
            }
            timer.Elapsed += new ElapsedEventHandler(timer_Event_Alarm);
            timer.Start();


            /* desTime = desTime.AddHours(inHour);
             desTime = desTime.AddMinutes(inMin);*/



            System.Windows.MessageBox.Show(inHour + "시간" + inMin + "분 후에 알람이 울립니다");


        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int inHour = Int32.Parse(Txtb2.Text);
            int inMin = Int32.Parse(Txtb4.Text);
            if (inHour != 0 && inMin != 0)
                timer.Interval = 1000 * (3600 * inHour) * (60 * inMin);
            else if (inHour == 0 && inMin != 0)
            {
                timer.Interval = 1000 * (60 * inMin);
            }
            else if (inMin == 0)
            {
                timer.Interval = 1000 * (3600 * inHour);
            }
            timer.Elapsed += new ElapsedEventHandler(timer_Event_Shut);
            timer.Start();
        }


        void timer_Event_Alarm(object sender, ElapsedEventArgs e)
        {
            System.Windows.MessageBox.Show("알람 시간");
            timer.Stop();
        }
        void timer_Event_Shut(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            Process.Start("shutdown.exe", "-s -t -f 00"); // api 찾기
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e) // Enable TOP MOST
        {
            App.Current.MainWindow.Topmost = true;

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e) //Disable TOP MOST
        {
            App.Current.MainWindow.Topmost = false;
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {

        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Window win = new Window();
            TextBox tb = new TextBox();
            tb.HorizontalAlignment = HorizontalAlignment;
            tb.Text = "test";


            win.Width = 600;
            win.Height = 100;
            win.Left = SystemParameters.PrimaryScreenWidth - win.Width + 5;
            win.Top = SystemParameters.PrimaryScreenHeight - win.Height * 9 + 35;
            win.Visibility = Visibility.Hidden;
            win.WindowStyle = WindowStyle.None;
            win.Visibility = Visibility.Visible;
            win.Show();



            //win.Close();
            //win.Visibility = Visibility.Hidden;



        }

        private void B(object sender, RoutedEventArgs e)
        {

        }
    }
}
