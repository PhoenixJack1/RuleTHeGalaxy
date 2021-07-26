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
    class SelectSchemaNameCanvas : Canvas
    {
        public static SortedList<int, string> BaseRoman;
        public static string[] TitleList;
        public static string[] Letters;
        //public static SortedList<int, char> Volumes;
        ScrolledPanel NamePanel, VolumePanel, RomanPanel, NumberPanel;
        public static int CurName;
        public SelectSchemaNameCanvas(int curname)
        {
            CurName = curname;
            Width = 500; Height = 500;
            Grid grid = new Grid();
            grid.Background = Common.GetLinearBrush(Colors.Black, Colors.White, Colors.Gray);
            Children.Add(grid);
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            
            NamePanel = new ScrolledPanel(TitleList, 11, BitConverter.GetBytes(CurName)[0]);
            List<string> Volumes = new List<string>();
            //foreach (char vol in   SelectSchemaNameCanvas.Volumes.Values)
            //    Volumes.Add(vol.ToString());
            VolumePanel = new ScrolledPanel(Letters, 11, BitConverter.GetBytes(CurName)[1]);
            List<string> Romans = new List<string>();
            List<string> Numbers = new List<string>();
            Romans.Add(" ");
            Numbers.Add(" ");
            for (byte i = 1; i < 255; i++)
            {
                Romans.Add(GetRoman(i));
                Numbers.Add(i.ToString());
            }
            RomanPanel = new ScrolledPanel(Romans.ToArray(), 11, BitConverter.GetBytes(CurName)[2]);
            NumberPanel = new ScrolledPanel(Numbers.ToArray(), 11, BitConverter.GetBytes(CurName)[3]);
            grid.Children.Add(NamePanel);
            grid.Children.Add(VolumePanel);
            grid.Children.Add(RomanPanel);
            grid.Children.Add(NumberPanel);
            Grid.SetColumn(VolumePanel, 1);
            Grid.SetColumn(RomanPanel, 2);
            Grid.SetColumn(NumberPanel, 3);
            Button SelectButton = new Button();
            //SelectButton.FontSize = 20;
            Children.Add(SelectButton);
            Canvas.SetTop(SelectButton, 420);
            Canvas.SetLeft(SelectButton, 100);
            SelectButton.Width = 200;
            SelectButton.Height = 50;
            SelectButton.Content = Links.Interface("Ok");
            SelectButton.Style = Links.ButtonStyle;
            SelectButton.Click += new RoutedEventHandler(SelectButton_Click);

        }
        public static string GetNameResult(string shiptypename, int code)
        {
            byte[] Bytes = BitConverter.GetBytes(code);
            byte nameid = Bytes[0];
            if (Links.Lang==0)
            switch (nameid)
                {
                    case 252: return Links.Interface("PirateSchemaName") + shiptypename;
                    case 251: return Links.Interface("GreenTeamSchemaName") + shiptypename;
                    case 254: return Links.Interface("TechsSchemaName") + shiptypename;
                    case 255: return Links.Interface("MercsSchemaName") + shiptypename;
                    case 253: return Links.Interface("AliensSchemaName") + shiptypename;
                }
            else if (Links.Lang==1)
                switch (nameid)
                {
                    case 252: return shiptypename + Links.Interface("PirateSchemaName");
                    case 251: return shiptypename + Links.Interface("GreenTeamSchemaName");
                    case 254: return shiptypename + Links.Interface("TechsSchemaName");
                    case 255: return shiptypename + Links.Interface("MercsSchemaName");
                    case 253: return shiptypename + Links.Interface("AliensSchemaName");
                }
            return shiptypename;
        }
        public static string GetNameResult(int code)
        {
            byte[] Bytes = BitConverter.GetBytes(code);
            string result = "";
            byte nameid = Bytes[0];
            byte volid = Bytes[1];
            byte romid = Bytes[2];
            byte numid = Bytes[3];
            if (nameid == 252) return "Pirate's";
            if (nameid == 251) return "Green Alliance's";
            if (nameid == 254) return "Technocrate's";
            if (nameid == 255) return "Mercenaries";
            if (nameid == 253) return "Alien's";
            if (nameid != 0)
                result += TitleList[nameid];
            if (volid != 0)
            {
                if (nameid != 0) result += "-";
                result += Letters[volid];
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
            CurName = BitConverter.ToInt32(array, 0);
            SchemasCanvas.Target = ApplyEnum.Name;
            ((SchemasCanvas)Links.Controller.SchemasCanvas).ApplyChanges();
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
    }
    class ScrolledPanel : StackPanel
    {
        string[] Array;
        int Elements;
        public byte currentid = 0;
        public ScrolledPanel(string[] array, int elements, byte curid)
        {
            Array = array;
            Elements = elements;
            Width = 100;
            currentid = curid;
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
                lbl.FontSize = 22;
                int pos = i % Array.Length;
                lbl.Foreground = Brushes.Black;
                lbl.Content = Array[pos];
                if ((pos) == currentid) { lbl.Background = Brushes.Red; lbl.Foreground = Brushes.White; }
                lbl.FontFamily = Links.Font;
                lbl.FontWeight = FontWeights.Bold;
                lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
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
