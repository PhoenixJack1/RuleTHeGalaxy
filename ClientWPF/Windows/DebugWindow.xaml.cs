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
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для DebugWindow.xaml
    /// </summary>
    public partial class DebugWindow : Window
    {
        public static bool NeedDebug = true;
        public static TextBlock tb1;
        public static TextBlock tb2;
        public static List<string> Block1List = new List<string>();
        public static List<string> Block2List = new List<string>();
        static int rows1 = 0;
        static int rows2 = 0;
        static Grid grid;
        static ScrollViewer viewer;
        public DebugWindow()
        {
            //InitializeComponent();
            Height = 300; Width = 400;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //WindowStyle = WindowStyle.None;
            //ResizeMode = ResizeMode.NoResize;
            //this.ShowInTaskbar = false;
            grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions[2].Width = new GridLength(20);
            //Content = canvas;
            viewer = new ScrollViewer();
            viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            Content = viewer;
            viewer.Content = grid;
            StackPanel panel1 = new StackPanel();
            //tb1 = Common.GetBlock(20, "");
            //grid.Children.Add(tb1);
            //tb1.Width = 200;
            //tb2 = Common.GetBlock(20, "");
            //grid.Children.Add(tb2);
            //Grid.SetColumn(tb2, 1);
            //Canvas.SetLeft(tb2, 200);
            

        }
        static Queue<DebugWindowElement> TB1Q = new Queue<DebugWindowElement>();
        static Queue<DebugWindowElement> TB2Q = new Queue<DebugWindowElement>();
        static int Stack1PosAdd = 0;
        static int Stack1PosWrite = 0;
        static int Stack2PosAdd = 0;
        static int Stack2PosWrite = 0;
        static SortedList<int, Rectangle> StatusRectangles = new SortedList<int, Rectangle>();
        static SortedList<int, TextBlock> Col1Blocks = new SortedList<int, TextBlock>();
        static SortedList<int, TextBlock> Col2Blocks = new SortedList<int, TextBlock>();
        public static void AddTB1(string text, bool isnew)
        {
            if (NeedDebug == false) return;
            //if (tb1 == null) return;
            TB1Q.Enqueue(new DebugWindowElement(text, isnew, Stack1PosAdd));
            Stack1PosAdd++;
            if (isnew) Block1List.Add(text);
            else Block1List[Block1List.Count - 1] += text;
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke((Action)_AddTB1);
        }
        static void _AddTB1()
        {
            DebugWindowElement element = null;
            for (;;)
            {
                element = TB1Q.Dequeue();
                if (element.Pos != Stack1PosWrite) TB1Q.Enqueue(element);
                else break;
            }
            Stack1PosWrite++;
            if (element.IsNew)
            {
                rows1++;
                if (rows1 > rows2)
                {
                    grid.RowDefinitions.Add(new RowDefinition());
                    grid.RowDefinitions[rows1 - 1].Height = new GridLength(20);
                    viewer.ScrollToEnd();
                }
                tb1 = Common.GetBlock(20, element.Text);
                tb1.Tag = tb1.Text.GetHashCode();
                Col1Blocks.Add(rows1 - 1, tb1);
                grid.Children.Add(tb1);
                Grid.SetRow(tb1, rows1 - 1);
            }
            else
            {
                tb1.Text += element.Text;
                tb1.Tag = tb1.Text.GetHashCode();
            }
            Test();
           
        }
        public static void AddTB2(string text, bool isnew)
        {
            if (NeedDebug == false) return;
            TB2Q.Enqueue(new DebugWindowElement(text, isnew, Stack2PosAdd));
            Stack2PosAdd++;
            if (isnew) Block2List.Add(text);
            else Block2List[Block2List.Count - 1] += text;
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke((Action)_AddTB2);
        }
        static void _AddTB2()
        {
            DebugWindowElement element = null;
            for (;;)
            {
                element = TB2Q.Dequeue();
                if (element.Pos != Stack2PosWrite) TB2Q.Enqueue(element);
                else break;
            }
            Stack2PosWrite++;
                if (element.IsNew)
            {
                rows2++;
                if (rows2 > rows1)
                {
                    grid.RowDefinitions.Add(new RowDefinition());
                    grid.RowDefinitions[rows2 - 1].Height = new GridLength(20);
                    viewer.ScrollToEnd();
                }
                tb2 = Common.GetBlock(20, element.Text);
                tb2.Tag = tb2.Text.GetHashCode();
                Col2Blocks.Add(rows2 - 1, tb2);
                grid.Children.Add(tb2);
                Grid.SetRow(tb2, rows2 - 1);
                Grid.SetColumn(tb2, 1);
            }
            else
            {
                tb2.Text += element.Text;
                tb2.Tag = tb2.Text.GetHashCode();
            }
            Test();
        }
        public static void Test()
        {
            for (int i = 0; i < grid.RowDefinitions.Count; i++)
            {
                if (!StatusRectangles.ContainsKey(i))
                {
                    if (!Col1Blocks.ContainsKey(i)) { StatusRectangles.Add(i, GetRectangle(false, i)); continue; }
                    if (!Col2Blocks.ContainsKey(i)) { StatusRectangles.Add(i, GetRectangle(false, i)); continue; }
                    int t1 = (int)Col1Blocks[i].Tag; int t2 = (int)Col2Blocks[i].Tag;
                    bool check = t1 == t2;
                    if (check == true)
                    {
                        StatusRectangles.Add(i, GetRectangle(true, i));
                        continue;
                    }
                    else
                    {
                        StatusRectangles.Add(i, GetRectangle(false, i));
                        continue;
                    }
                }
                else
                {
                    Rectangle rect = StatusRectangles[i];
                    //if ((bool)rect.Tag == true) continue;
                    if (!Col1Blocks.ContainsKey(i) || !Col2Blocks.ContainsKey(i))
                    { rect.Fill = Brushes.Red; rect.Tag = false; continue; }
                    int t1 = (int)Col1Blocks[i].Tag; int t2 = (int)Col2Blocks[i].Tag;
                    bool check = t1==t2;
                    if (check== true)
                    {
                        rect.Fill = Brushes.Green;
                        rect.Tag = true;
                        continue;
                    }
                    else
                    {
                        rect.Fill = Brushes.Red;
                        rect.Tag = false;
                        continue;
                    }
                }
            }
        }
        /*
        public static bool CheckString(string t1, string t2)
        {
            if (t1.Length != t2.Length) return false;
            for (int i = 0; i < t1.Length; i++)
                if (t1[i] != t2[i]) return false;
            return true;
        }*/
        public static Rectangle GetRectangle(bool status, int i)
        {
            Rectangle rect = Common.GetRectangle(20, status ? Brushes.Green : Brushes.Red);
            rect.Tag = status;
            grid.Children.Add(rect);
            Grid.SetRow(rect, i);
            Grid.SetColumn(rect, 2);
            return rect;
        }
        class DebugWindowElement
        {
            public string Text;
            public bool IsNew;
            public int Pos;
            public DebugWindowElement(string text, bool isnew, int pos)
            {
                Text = text; IsNew = isnew; Pos = pos;
            }
        }
    }
}
