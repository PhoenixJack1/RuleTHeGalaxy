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
using System.Windows.Media.Animation;
using System.Threading.Tasks;

namespace Client
{
    /*
    public delegate void WorkMethod();
    public delegate bool HelpMethod(ByteMessage message);
    class Load2
    {
        LoadIcon load;
        WorkMethod WorkInvoker;
        HelpMethod HelpMethod;
        System.Windows.Threading.DispatcherTimer timer;
        bool IsWorked = false;
        bool Result = false;
        ByteMessage Query;
        ByteMessage Response;
        bool IsImportant = false;
        public Load2()
        {
            load = new LoadIcon();
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            if (!IsWorked)
            {
                if (Result)
                {
                    if (HelpMethod != null)
                        HelpMethod(Response);
                    WorkInvoker();
                }
                else
                    Commands.Stop_Game();
            }
            else
                timer.Start();
        }

        public void LoadImportantInfo(WorkMethod method, ByteMessage query, string text, HelpMethod helpMethod)
        {
            WorkInvoker = method;
            HelpMethod = helpMethod;
            Query = query;
            Result = false;
            IsWorked = true;
            TaskFactory fact = new TaskFactory();
            Task task = fact.StartNew(load.Place);
            Task task2 = task.ContinueWith(work => { Work2(); });
            timer.Start();
            //Task task3 = task2.ContinueWith(invoke=> { if (Result) { if (HelpMethod != null) HelpMethod(Response); WorkInvoker(); } else Commands.Stop_Game(); });
        }
        public void Work2()
        {
            //System.Threading.Thread.Sleep(1000);
            byte[] RandomArr = BitConverter.GetBytes(GSGameInfo.random);
            try
            {
                Response = new ByteMessage(Links.Loginproxy.GetInformation(RandomArr, Query.GetBytes()));
                Result = true;
            }
            catch (Exception)
            {
                
            }
            load.Remove();
            IsWorked = false;
        }
    }
    class LoadIcon : Grid
    {
        public LoadIcon()
        {
            
            Grid.SetRowSpan(this, 2);
            Grid.SetRow(this, 1);
            Background = Brushes.Black;
            //Width = 300;
            //Height = 300;
            
            Background = new SolidColorBrush(Color.FromArgb(100, 100, 100, 100));
            Panel.SetZIndex(this, 200);
            RowDefinitions.Add(new RowDefinition());
            RowDefinitions.Add(new RowDefinition());
            RowDefinitions.Add(new RowDefinition());
            RowDefinitions.Add(new RowDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            RowDefinitions[1].Height = new GridLength(200, GridUnitType.Pixel);
            RowDefinitions[2].Height = new GridLength(50, GridUnitType.Pixel);
            ColumnDefinitions[1].Width = new GridLength(200, GridUnitType.Pixel);
            

            Rectangle rect = new Rectangle();
            rect.Width = 200; rect.Height = 20; rect.Fill = Brushes.Red;
            DoubleAnimation anim = new DoubleAnimation(0, 360, TimeSpan.FromSeconds(2));
            anim.RepeatBehavior = RepeatBehavior.Forever;
            rect.RenderTransformOrigin = new Point(0.5, 0.5);
            RotateTransform rotate = new RotateTransform();
            rect.RenderTransform = rotate;
            rotate.BeginAnimation(RotateTransform.AngleProperty, anim);
            Children.Add(rect);
            Grid.SetRow(rect, 1);
            Grid.SetColumn(rect, 1);
        }


        void SetSize()
        {
            if (Links.Controller.ControllerStatus == EControllerStatus.Main)
            {
                this.Width = Links.Controller.mainWindow.mainGrid.ActualWidth;
                this.Height = Links.Controller.mainWindow.mainGrid.ActualHeight;
            }
            else if (Links.Controller.ControllerStatus==EControllerStatus.Login)
            {
                this.Width = Links.Controller.mainWindow.loginGrid.ActualWidth;
                this.Height = Links.Controller.mainWindow.loginGrid.ActualHeight;
                //Canvas.SetLeft(this, (Width - 200) / 2);
                //Canvas.SetTop(this, (Height - 250) / 2);
            }
            else
            {
                this.Width = Links.Controller.mainWindow.signGrid.ActualWidth;
                this.Height = Links.Controller.mainWindow.signGrid.ActualHeight;
            }
        }
        void Place2()
        {
            SetSize();
            if (Links.Controller.ControllerStatus == EControllerStatus.Main)
            {
                if (!Links.Controller.mainWindow.mainGrid.Children.Contains(this))
                    Links.Controller.mainWindow.mainGrid.Children.Add(this);
            }
            else if (Links.Controller.ControllerStatus == EControllerStatus.Login)
            {
                if (!Links.Controller.mainWindow.loginGrid.Children.Contains(this))
                    Links.Controller.mainWindow.loginGrid.Children.Add(this);
            }
            else
            {
                if (!Links.Controller.mainWindow.signGrid.Children.Contains(this))
                    Links.Controller.mainWindow.signGrid.Children.Add(this);
            }
        }
        public void Place()
        {
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)Place2);
        }
        public void Remove()
        {
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)Remove2);
        }
        void Remove2()
        {
            if (Links.Controller.ControllerStatus == EControllerStatus.Main)
            {
                if (Links.Controller.mainWindow.mainGrid.Children.Contains(this))
                    Links.Controller.mainWindow.mainGrid.Children.Remove(this);
            }
            else if (Links.Controller.ControllerStatus == EControllerStatus.Login)
            {
                if (Links.Controller.mainWindow.loginGrid.Children.Contains(this))
                    Links.Controller.mainWindow.loginGrid.Children.Remove(this);
            }
            else
            {
                if (Links.Controller.mainWindow.signGrid.Children.Contains(this))
                    Links.Controller.mainWindow.signGrid.Children.Remove(this);
            }
        }
    }
    */
    class LoadWindow:Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        ByteMessage request;
        ByteMessage answer;
        bool isWorked;
        TextBlock LoadingInfo;
        bool isArchive = false;
        Rectangle rect;
        Grid grd;
        bool IsError = false;
        System.Threading.Tasks.TaskFactory Factory = new TaskFactory();
        public LoadWindow()
        {
            Height = 300; Width = 200;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            this.ShowInTaskbar = false;
            Background = new SolidColorBrush(Color.FromArgb(1, 200, 200, 200));
            AllowsTransparency = true;
            grd = new Grid();
            Content = grd;
            grd.RowDefinitions.Add(new RowDefinition());
            grd.RowDefinitions.Add(new RowDefinition());
            grd.RowDefinitions[0].Height = new GridLength(2, GridUnitType.Star);

            LoadingInfo = new TextBlock();
            LoadingInfo.Text = "Info";
            LoadingInfo.Background = Brushes.LightCyan;
            LoadingInfo.TextWrapping = TextWrapping.Wrap;
            LoadingInfo.TextAlignment = TextAlignment.Center;
            LoadingInfo.VerticalAlignment = VerticalAlignment.Center;
            LoadingInfo.FontFamily = Links.Font;
            LoadingInfo.FontWeight = FontWeights.Bold;
            LoadingInfo.FontSize = 30;
            grd.Children.Add(LoadingInfo);
            Grid.SetRow(LoadingInfo, 1); 
            
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;

            if (rect != null) grd.Children.Remove(rect);
            rect = new Rectangle();
            rect.Width = 200; rect.Height = 200;
            rect.Fill = Links.Brushes.LoadWindowBrush;
            grd.Children.Add(rect);
            RotateTransform rotate = new RotateTransform();
            rect.RenderTransformOrigin = new Point(0.5, 0.5);
            rect.RenderTransform = rotate;
            DoubleAnimation anim = new DoubleAnimation(0, 90, TimeSpan.FromSeconds(0.5));
            anim.RepeatBehavior = RepeatBehavior.Forever;

            rotate.BeginAnimation(RotateTransform.AngleProperty, anim);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            Factory.StartNew(Run);
            //Run();
        }
        void Run()
        {
            ByteMessage responce = null;
            //System.Threading.Thread.Sleep(1000);
            try
            {
                if (isArchive)
                {
                    byte[] array = Links.Loginproxy.LoadFile(BitConverter.GetBytes(GSGameInfo.random), Links.GameData.GameData1);
                    responce = new ByteMessage(0, 0, array);
                }
                else
                {
                    byte[] RandomArr = BitConverter.GetBytes(GSGameInfo.random);
                    responce = new ByteMessage(Links.Loginproxy.GetInformation(RandomArr, request.GetBytes()));
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                Commands.Stop_Game();
                IsError = true;
            }
            answer = responce;
            isWorked = true;
            Links.Controller.loadWindow.Dispatcher.BeginInvoke(
                (Action)Hide);
        }
        public ByteMessage LoadArchive()
        {
            IsError = false;
            isArchive = true;
            LoadingInfo.Text = "Loading Game archive";
            isWorked = false;
            answer = null;
            timer.Start();
            ShowDialog();
            return answer;
        }
        public ByteMessage GetResult(ByteMessage query, string Title)
        {
            IsError = false;
            isArchive = false;
            request = query;
            LoadingInfo.Text = Title;
            isWorked = false;
            answer = null;
            timer.Start();
            ShowDialog();
            return answer;
        }
        /*
        void LoadWindow_Activated(object sender, EventArgs e)
        {

            if (rect != null) grd.Children.Remove(rect);
            rect = new Rectangle();
            rect.Width = 200; rect.Height = 200;
            rect.Fill = Links.Brushes.LoadWindowBrush;
            grd.Children.Add(rect);
            RotateTransform rotate = new RotateTransform();
            rect.RenderTransformOrigin = new Point(0.5, 0.5);
            rect.RenderTransform = rotate;
            DoubleAnimation anim = new DoubleAnimation(0, 90, TimeSpan.FromSeconds(0.5));
            anim.RepeatBehavior = RepeatBehavior.Forever;
           
            rotate.BeginAnimation(RotateTransform.AngleProperty, anim);
            GSGameInfo.SetPlayerName(new GSString("Timer Started"));
            timer.Start();


        }
        */
    }
    
}
