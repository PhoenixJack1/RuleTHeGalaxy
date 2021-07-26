using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    class LoadWindow:Window
    {
        System.Timers.Timer timer;
        ByteMessage request;
        ByteMessage answer;
        bool isWorked;
        TextBlock LoadingInfo;
        public LoadWindow()
        {
            Height = 300; Width = 600;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            Background = new SolidColorBrush(Color.FromArgb(0, 200, 200, 200));
            AllowsTransparency = true;
            Grid grd = new Grid();
            Content = grd;
            grd.RowDefinitions.Add(new RowDefinition());
            grd.RowDefinitions.Add(new RowDefinition());
            grd.RowDefinitions.Add(new RowDefinition());
            grd.RowDefinitions.Add(new RowDefinition());
            
            grd.ColumnDefinitions.Add(new ColumnDefinition());
            grd.ColumnDefinitions.Add(new ColumnDefinition());
            grd.ColumnDefinitions.Add(new ColumnDefinition());

            Label TextLoading = new Label();
            TextLoading.HorizontalAlignment = HorizontalAlignment.Center;
            TextLoading.Content = "Loading";
            TextLoading.Background = Brushes.LightGreen;
            grd.Children.Add(TextLoading);
            Grid.SetRow(TextLoading, 1); Grid.SetColumn(TextLoading, 1);
            TextLoading.FontSize = 20;

            LoadingInfo = new TextBlock();
            LoadingInfo.Text = "Info";
            LoadingInfo.Background = Brushes.LightCyan;
            LoadingInfo.TextWrapping = TextWrapping.Wrap;
            LoadingInfo.TextAlignment = TextAlignment.Center;
            grd.Children.Add(LoadingInfo);
            Grid.SetRow(LoadingInfo, 2); Grid.SetColumn(LoadingInfo, 1);
            Activated+=new EventHandler(LoadWindow_Activated);
            timer = new System.Timers.Timer();
            timer.Interval = 10;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
        }
        public ByteMessage GetResult(ByteMessage query, string Title)
        {
            request = query;
            LoadingInfo.Text = Title;
            isWorked = false;
            answer = null;
            ShowDialog();
            ByteMessage result;
            int i=0;
            while (!isWorked)
            {
                i++;
                System.Threading.Thread.Sleep(20);
                if (i > 100)
                {
                    MessageBox.Show("Load Error");
                    return null;
                }
            }
            return answer;
        }

        void LoadWindow_Activated(object sender, EventArgs e)
        {  
            timer.Start();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            ByteMessage responce = null;
            try
            {
               responce = new ByteMessage(Links.Loginproxy.GetInformation(GSGameInfo.Login, GSGameInfo.random, request.GetBytes()));
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            answer = responce;
            isWorked = true;
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                (System.Threading.ThreadStart)Hide);
        }
        
        
    }
}
