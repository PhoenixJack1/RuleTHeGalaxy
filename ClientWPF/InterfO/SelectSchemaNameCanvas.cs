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
using System.Windows.Media.Animation;

namespace Client
{
    class SelectSchemaNameCanvas:Canvas
    {
        public static SortedList<int, string> BaseRoman;
        public static string[] NameList;
        public static SortedList<int, char> Volumes;
        ScrolledPanel NamePanel, VolumePanel, RomanPanel, NumberPanel;
        public SelectSchemaNameCanvas()
        {
            Width = 600; Height = 500;

            Grid grid = new Grid();
            grid.Background = Brushes.Black;
            Children.Add(grid);
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            NamePanel = new ScrolledPanel(NameList, 15);
            List<string> Volumes = new List<string>();
            foreach (char vol in   SelectSchemaNameCanvas.Volumes.Values)
                Volumes.Add(vol.ToString());
            VolumePanel = new ScrolledPanel(Volumes.ToArray(), 15);
            List<string> Romans = new List<string>();
            List<string> Numbers = new List<string>();
            Romans.Add(" ");
            Numbers.Add(" ");
            for (byte i = 1; i < 255; i++)
            {
                Romans.Add(GetRoman(i));
                Numbers.Add(i.ToString());
            }
            RomanPanel = new ScrolledPanel(Romans.ToArray(), 15);
            NumberPanel = new ScrolledPanel(Numbers.ToArray(), 15);
            grid.Children.Add(NamePanel);
            grid.Children.Add(VolumePanel);
            grid.Children.Add(RomanPanel);
            grid.Children.Add(NumberPanel);
            Grid.SetColumn(VolumePanel, 1);
            Grid.SetColumn(RomanPanel, 2);
            Grid.SetColumn(NumberPanel, 3);
            Button SelectButton = new Button();
            SelectButton.FontSize = 20;
            Children.Add(SelectButton);
            Canvas.SetTop(SelectButton, 400);
            SelectButton.Content = "Select";
            SelectButton.Click += new RoutedEventHandler(SelectButton_Click);

        }
        public static string GetNameResult(int code)
        {
            byte[] Bytes = BitConverter.GetBytes(code);
            string result = "";
            byte nameid = Bytes[0];
            byte volid = Bytes[1];
            byte romid = Bytes[2];
            byte numid = Bytes[3];
            if (nameid != 0)
                result += NameList[nameid];
            if (volid != 0)
            {
                if (nameid != 0) result += "-";
                result += Volumes[volid];
            }
            if (romid != 0)
            {
                if (nameid != 0 || volid != 0) result += "-";
                result += GetRoman((byte)romid);
            }
            if (numid != 0)
            {
                if (nameid != 0 || volid != 0 || romid != 0) result += "-";
                result += numid.ToString();
            }
            return result;
        }
        void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            byte[] array = new byte[] { NamePanel.currentid, VolumePanel.currentid, RomanPanel.currentid, NumberPanel.currentid };
            ((SchemaPanel)Links.Controller.schemaspanel).SelectID = BitConverter.ToInt32(array,0);
            ((SchemaPanel)Links.Controller.schemaspanel).Target = ApplyEnum.Name;
            ((SchemaPanel)Links.Controller.schemaspanel).ApplyChanges();
            Links.Controller.PopUpCanvas.Remove();
            
        }
        public static string GetRoman(byte k)
        {
            StringBuilder result = new StringBuilder();
            var neednumbers =
                from z in BaseRoman.Keys
                where z <= k
                orderby z descending
                select z;
            foreach (byte cur in neednumbers)
            {
                while ((k / cur) >= 1)
                {
                    k -= cur;
                    result.Append(BaseRoman[cur]);
                }
            }
            return result.ToString();
        }
        public static void Prepare()
        {
            BaseRoman = new SortedList<int, string>();
            BaseRoman.Add(1, "I");
            BaseRoman.Add(4, "IV");
            BaseRoman.Add(5, "V");
            BaseRoman.Add(9, "IX");
            BaseRoman.Add(10, "X");
            BaseRoman.Add(40, "XL");
            BaseRoman.Add(50, "L");
            BaseRoman.Add(90, "XC");
            BaseRoman.Add(100, "C");
            NameList = new string[] { "", "Mk", "Type", "Var", "Ver", "Vol", "Group", "Param", "Obj", "Mod" };
            Volumes = new SortedList<int, char>();
            Volumes.Add(0, ' ');
            for (int i = 1; i < 27; i++)
                Volumes.Add(i, (char)(i + 64));
            for (int i = 27; i < 53; i++)
                Volumes.Add(i, (char)(i + 70));
            for (int i = 53; i < 85; i++)
                Volumes.Add(i, (char)(i + 987));
            Volumes.Add(86, 'Ё');
            for (int i = 87; i < 119; i++)
                Volumes.Add(i, (char)(i + 985));
            Volumes.Add(120, 'ё');

        }
    }
    class ScrolledPanel : StackPanel
    {
        string[] Array;
        int Elements;
        public byte currentid = 0;
        public ScrolledPanel(string[] array, int elements)
        {
            Array = array;
            Elements = elements;
            Width = 100;
            Place();
            PreviewMouseWheel += new MouseWheelEventHandler(ScrolledPanel_PreviewMouseWheel);
        }

        void ScrolledPanel_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0) currentid = (byte)((currentid + 1) % Array.Length);
            else currentid = (byte)((currentid - 1 + Array.Length) % Array.Length);
            Place();
        }

        void bord_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Border border = (Border)sender;
            int tag = (int)border.Tag;
            currentid = (byte)tag;
            Place();
        }
        void Place()
        {
            Children.Clear();
            int delta = (int)Elements / 2;
            int first = Array.Length - delta + currentid;
            for (int i = first; i < first + Elements; i++)
            {
                Label lbl = new Label();
                int pos = i % Array.Length;
                lbl.Foreground = Brushes.White;
                lbl.Content = Array[pos];
                if ((pos) == currentid) lbl.Background = Brushes.Red;
                lbl.FontWeight = FontWeights.Bold;
                Border bord = new Border();
                bord.BorderBrush = Brushes.Black;
                bord.BorderThickness = new Thickness(1);
                bord.Child = lbl;
                Children.Add(bord);

                bord.Tag = pos;
                bord.PreviewMouseDown += new MouseButtonEventHandler(bord_PreviewMouseDown);
            }
        }
    }
}
