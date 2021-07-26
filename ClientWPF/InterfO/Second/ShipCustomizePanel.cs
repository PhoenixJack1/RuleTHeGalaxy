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
    class ShipCustomizePanel : Grid
    {
        public static ScrolledPanelShip panel1;
        public static ScrolledPanelShip panel2;
        public static ScrolledPanelShip panel3;
        public static ScrolledPanelShip panel4;
        public static ScrolledPanelShip panel5;
        static ShipB CurShip;
        static Schema CurShema;
        static Grid CurGrid;
        static int Model = 0;
        public ShipCustomizePanel(Schema schema)
        {
            Width = 1100;
            Height = 500;
            CurShema = schema;
            CurGrid = this;
            Rectangle border = new Rectangle();
            border.Stroke = Brushes.Black;
            border.StrokeThickness = 3;
            Children.Add(border);
            Grid.SetRowSpan(border, 3);
            Grid.SetColumnSpan(border, 6);
            Background = Brushes.Black;
            //Background = Common.GetLinearBrush(Colors.Gray, Colors.White, Colors.Gray);
            byte[] list = BitConverter.GetBytes(Model);
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions[5].Width = new GridLength(320);

            RowDefinitions.Add(new RowDefinition()); RowDefinitions[0].Height = new GridLength(50);
            RowDefinitions.Add(new RowDefinition());
            RowDefinitions.Add(new RowDefinition()); RowDefinitions[2].Height = new GridLength(50);

            List<Rectangle> Modelslist = new List<Rectangle>();
            for (byte i = 0; i < ShipModelNew.Models.Count; i++)
                Modelslist.Add(Common.GetRectangle(80, ShipModelNew.GetShipModelBrush(schema.ShipType.WeaponCapacity, i)));
            
            panel1 = new ScrolledPanelShip(Modelslist.ToArray(), 3, 90);
            panel1.SetElement(list[0]);
            Children.Add(panel1);
            Grid.SetRow(panel1, 1);

            List<Border> BordList = new List<Border>();
            List<Border> BeltList = new List<Border>();
            //List<Rectangle> HullBrushesList = new List<Rectangle>();
            for (byte i = 0; i < ShipBrush.HullBrush.Count; i++)
            {
                Border bord = new Border(); bord.Width = 50; bord.Height = 50; bord.Background = ShipBrush.HullBrush[i];
                Label lb = new Label();
                lb.Content = i.ToString(); lb.FontFamily = Links.Font;
                bord.Child = lb;
                BordList.Add(bord);
                //HullBrushesList.Add(Common.GetRectangle(50, ShipBrush.HullBrush[i]));
                Border belt = new Border(); belt.Width = 50; belt.Height = 50; belt.Background = ShipBrush.BeltBrush[i];
                Label beltlbl = new Label();
                beltlbl.Content = i.ToString(); beltlbl.FontFamily = Links.Font;
                belt.Child = beltlbl;
                BeltList.Add(belt);
            }
            panel2 = new ScrolledPanelShip(BeltList.ToArray(), 5, 54);
            //    panel2 = new ScrolledPanelShip(HullBrushesList.ToArray(), 5, 54);
            panel2.SetElement(list[1]);
            Children.Add(panel2);
            Grid.SetColumn(panel2, 1);
            Grid.SetRow(panel2, 1);

            List<Rectangle> BeltBrushesList = new List<Rectangle>();
            for (byte i = 0; i < ShipBrush.BeltBrush.Count; i++)
                BeltBrushesList.Add(Common.GetRectangle(50, ShipBrush.BeltBrush[i]));
            panel3 = new ScrolledPanelShip(BordList.ToArray(), 5, 54);
            //panel3 = new ScrolledPanelShip(BeltBrushesList.ToArray(), 5, 54);
            panel3.SetElement(list[2]);
            Children.Add(panel3);
            Grid.SetColumn(panel3, 2);
            Grid.SetRow(panel3, 1);

            List<Path> CocpitList = new List<Path>();
            for (int i = 0; i < 16; i++)
                CocpitList.Add(Cockpit.GetCockpit(i, 0));
            panel4 = new ScrolledPanelShip(CocpitList.ToArray(), 5, 54);
            panel4.SetElement((byte)(list[3] & 15));
            Children.Add(panel4);
            Grid.SetColumn(panel4, 3);
            Grid.SetRow(panel4, 1);

            List<Path> CocpitBrushList = new List<Path>();
            for (int i = 0; i < 16; i++)
                CocpitBrushList.Add(Cockpit.GetCockpit(0, i));
            panel5 = new ScrolledPanelShip(CocpitBrushList.ToArray(), 5, 54);
            panel5.SetElement((byte)(list[3] >> 4));
            Children.Add(panel5);
            Grid.SetColumn(panel5, 4);
            Grid.SetRow(panel5, 1);

                DrawShip(list);
           
            
            

            Label TitleLabel = new Label();
            TitleLabel.Style = Links.TextStyle;
            TitleLabel.Foreground = Brushes.White;
            TitleLabel.Content = Links.Interface("SelectShipModel");
            TitleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            Children.Add(TitleLabel);
            Grid.SetColumnSpan(TitleLabel, 6);

            Button CancelButton = new Button();
            CancelButton.Style = Links.ButtonStyle;
            CancelButton.Content = Links.Interface("Cancel");
            Children.Add(CancelButton);
            Grid.SetRow(CancelButton, 2);
            Grid.SetColumn(CancelButton, 3);
            CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
            CancelButton.Height = 50;
            CancelButton.Width = 100;

            Button OkButton = new Button();
            OkButton.Style = Links.ButtonStyle;
            OkButton.Content = Links.Interface("Ok");
            Children.Add(OkButton);
            Grid.SetRow(OkButton, 2);
            Grid.SetColumn(OkButton, 1);
            OkButton.Height = 50;
            OkButton.Width = 100;
            OkButton.Click += new RoutedEventHandler(OkButton_Click);
       
        }
        
        void OkButton_Click(object sender, RoutedEventArgs e)
        {
            string eventresult = Events.BuildNewShip(CurShema, Model);
            if (eventresult != "")
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult), true);
                return;
            }
            Gets.GetTotalInfo("После события по созданию нового корабля");
            Gets.GetResources();
            //Gets.GetShips();
            Links.Controller.NewSchemasCanvas.Refresh();
        }

        void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Links.Controller.NewSchemasCanvas.Refresh();
        }
         
        public static void DrawShip(byte[] list)
        {
            if (CurShip != null)
                CurGrid.Children.Remove(CurShip.HexShip);
            CurShip = new ShipB(1, CurShema, 100, null, list, ShipSide.Defense, null, true, null, ShipBMode.Battle, 255);
            CurGrid.Children.Add(CurShip.HexShip);
            Grid.SetColumn(CurShip.HexShip, 5);
            Grid.SetRow(CurShip.HexShip, 1);
            Model = BitConverter.ToInt32(list, 0);
        }
    }
    
    public class ScrolledPanelShip : StackPanel
    {
        object[] Array;
        int Elements;
        public byte currentid = 0;
        int elementheight;
        public ScrolledPanelShip(object[] array, int elements, int height)
        {
            elementheight = height;
            VerticalAlignment = VerticalAlignment.Center;
            Background = Brushes.DarkGray;
            Array = array;
            Elements = elements;
            Width = 100;
            Place();
            PreviewMouseWheel += new MouseWheelEventHandler(ScrolledPanel_PreviewMouseWheel);
        }

        void ScrolledPanel_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0) currentid = (byte)((currentid - 1 + Array.Length) % Array.Length);
            else currentid = (byte)((currentid + 1) % Array.Length);
            Place();
        }
        public void SetElement(byte id)
        {
            currentid = id;
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
                //lbl.Foreground = Brushes.White;
                lbl.Content = Array[pos];
                lbl.HorizontalAlignment = HorizontalAlignment.Center;
                lbl.VerticalAlignment = VerticalAlignment.Center;
                lbl.Height = elementheight;
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
            byte[] list = new byte[4];
            if (ShipCustomizePanel.panel1 != null) list[0] = ShipCustomizePanel.panel1.currentid; else list[0] = 0;
            if (ShipCustomizePanel.panel2 != null) list[1] = ShipCustomizePanel.panel2.currentid; else list[1] = 0;
            if (ShipCustomizePanel.panel3 != null) list[2] = ShipCustomizePanel.panel3.currentid; else list[2] = 0;
            if (ShipCustomizePanel.panel4 != null) list[3] = ShipCustomizePanel.panel4.currentid; else list[3] = 0;
            if (ShipCustomizePanel.panel5 != null) list[3] = (byte)(list[3] + (ShipCustomizePanel.panel5.currentid << 4)); 
            ShipCustomizePanel.DrawShip(list);
        }
    }
    public abstract class Cockpit
    {
        public static Path GetCockpit(int form, int brush)
        {
            Path result = new Path();
            result.Stroke = Brushes.Black;
            result.StrokeThickness = 1;
            PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            switch (form)
            {
                case 0:
                    result.Data = new EllipseGeometry(new Point(25, 15), 25, 15); break;
                case 1:
                    result.Data = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M0,10 a50,60 180 0,1 50,0 v20 h-50 v-20")); break;
                case 2:
                    result.Data = new CombinedGeometry(new EllipseGeometry(new Point(25, 10), 25, 10), new RectangleGeometry(new Rect(10, 10, 30, 20))); break;
                case 3:
                    result.Data = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M25,0 l25,15 l-25,15 l-25,-15 Z")); break;
                case 4:
                    result.Data = new RectangleGeometry(new Rect(0, 0, 50, 30), 10, 10); break;
                case 5:
                    result.Data = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M25,0 l25,15 a30,30 180 0,1 -50,0 z")); break;
                case 6:
                    result.Data = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M0,15 a25,15 180 0,1 50,0 l-25,15 z")); break;
                case 7:
                    result.Data = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M25,0 l25,15 h-10 v15 h-30 v-15 h-10 z")); break;
                case 8:
                    result.Data = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M0,0 h50 v15 a25,15 180 1,1 -50,0 z")); break;
                case 9:
                    GeometryGroup cockpit9group = new GeometryGroup();
                    cockpit9group.Children.Add(new EllipseGeometry(new Point(12, 15), 12, 15));
                    cockpit9group.Children.Add(new EllipseGeometry(new Point(38, 15), 12, 15));
                    result.Data = cockpit9group; break;
                case 10:
                    result.Data = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M0,20 a25,20 180 0,1 50,0 a15.5,15 180 0,0 -25,0 a15,25 180 0,1 -25,0")); break;
                case 11:
                    result.Data = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M0,12 a25,20 180 0,0 50,0 a15.5,25 180 0,0 -25,0 a15,15 180 0,1 -25,0")); break;
                case 12:
                    result.Data = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M5,30 a10,5 100 0,1 15,-25 a25,25 0 0,1 -15,25 M45,30 a10,5 -100 0,0 -15,-25 a25,25 0 0,0 15,25")); break;
                case 13:
                    result.Data = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M3,8 a4,4 0 0,1 6,-6 l8,6 a14,14 0 0,0 16,0 l8,-6 a4,4 0 0,1 6,6 l-4,4 a6,6 0 0,0 0,6"+ 
              "l4,4 a4,4 0 0,1 -6,6 l-8,-6 a14,14 0 0,0 -16,0 l-8,6 a4,4 0 0,1 -6,-6 l4,-4 a6,6 0 0,0 0,-6 l-4,-4")); break;
                case 14:
                    result.Data = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M25,3 a10,12 0 1,0 0.1,0 M8,3 a5,5 0 1,0 0.1,0 M42,3 a5,5 0 1,0 0.1,0")); break;
                case 15:
                    result.Data = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M25,3 l20,25 h-40z")); break;
            }
            Brush resultbrush = new LinearGradientBrush();
            
            switch (brush)
            {
                case 0:
                    resultbrush = Links.Brushes.Glass1; break;
                case 1:
                    resultbrush = Links.Brushes.Glass2; break;
                case 2:
                    resultbrush = Links.Brushes.Glass3; break;
                case 3:
                    resultbrush = Links.Brushes.Glass4; break;
                case 4:
                    resultbrush = Links.Brushes.Glass5; break;
                case 5:
                    resultbrush = Links.Brushes.Glass6; break;
                case 6:
                    resultbrush = Links.Brushes.Glass7; break;
                case 7:
                    resultbrush = Links.Brushes.Glass8; break;
                case 8:
                    resultbrush = Links.Brushes.Glass9; break;
                case 9:
                    resultbrush = Links.Brushes.Glass10; break;
                case 10:
                    resultbrush = Links.Brushes.Glass11; break;
                case 11:
                    resultbrush = Links.Brushes.Glass12; break;
                case 12:
                    resultbrush = Links.Brushes.Glass13; break;
                case 13:
                    resultbrush = Links.Brushes.Glass14; break;
                case 14:
                    resultbrush = Links.Brushes.Glass15; break;
                case 15:
                    resultbrush = Links.Brushes.Glass16; break;
            }
             
            result.Fill = resultbrush;
            return result;
        }
    }
    
}
