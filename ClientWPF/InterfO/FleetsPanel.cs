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
using System.Windows.Threading;

namespace Client
{
    
    class FleetFilter : Canvas
    {
        string[] Values;
        TextBlock block;
        public byte curvalue = 0;
        public FleetFilter(string[] list)
        {
            Values = list;
            Width = 300; Height = 50;
            Background = Brushes.Black;
            Path LeftArrow = new Path();
            LeftArrow.Width = 50; LeftArrow.Height = 50;
            LeftArrow.Stroke = Brushes.White;
            LeftArrow.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,0 a25,25 0 1,0 0.1,0 M25,1 a24,24 0 1,0 0.1,0 M7,25 l18,-18 a18,18 0 0,1 0,36z"));
            Children.Add(LeftArrow);
            LeftArrow.Fill = Common.GetRadialBrush(Colors.Green, 0.7, 0.3);
            LeftArrow.PreviewMouseDown += new MouseButtonEventHandler(LeftArrow_PreviewMouseDown);

            Path RightArrow = new Path();
            RightArrow.Width = 50; RightArrow.Height = 50;
            RightArrow.Stroke = Brushes.White;
            RightArrow.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,0 a25,25 0 1,0 0.1,0 M25,1 a24,24 0 1,0 0.1,0 M44,25 l-18,-18 a18,18 0 0,0 0,36z"));
            Children.Add(RightArrow);
            Canvas.SetLeft(RightArrow, 250);
            RightArrow.Fill = Common.GetRadialBrush(Colors.Green, 0.7, 0.3);
            RightArrow.PreviewMouseDown += new MouseButtonEventHandler(RightArrow_PreviewMouseDown);

            Path Body = new Path();
            Body.Stroke = Brushes.White;
            Body.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,0 h250 M25,50 h250"));
            Children.Add(Body);
            Margin = new Thickness(10);

            block = Common.GetBlock(24, Values[0]);
            Children.Add(block);
            block.Foreground = Brushes.White;
            block.TextAlignment = TextAlignment.Center;
            block.Width = 200;
            Canvas.SetLeft(block, 50);
            Canvas.SetTop(block, 10);
        }
        public void ShowFreeFleets()
        {
            curvalue = 1;
            block.Text = Values[curvalue];
        }
        public void ShowAllFleets()
        {
            curvalue = 0;
            block.Text = Values[curvalue];
        }
        void RightArrow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            curvalue++;
            if (curvalue == 255) curvalue = (byte)(Values.Length - 1);
            else if (curvalue == Values.Length) curvalue = 0;
            block.Text = Values[curvalue];
            Links.Controller.FleetsCanvas.Refresh();
        }

        void LeftArrow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            curvalue--;
            if (curvalue == 255) curvalue = (byte)(Values.Length - 1);
            else if (curvalue == Values.Length) curvalue = 0;
            block.Text = Values[curvalue];
            Links.Controller.FleetsCanvas.Refresh();
        }
    }
    class CreateFleetPanel:Border
    {
        public static ScrolledPanelFleet panel1;
        public static ScrolledPanelFleet panel2;
        public static ScrolledPanelFleet panel3;
        public static ScrolledPanelFleet panel4;
        public static FleetEmblem Emblem;
        FleetBase Sector;
        Canvas canvas;
        public CreateFleetPanel(FleetBase sector)
        {
            Sector = sector;
            Width = 860;
            Height = 430;
            BorderBrush = new SolidColorBrush(Color.FromRgb(0, 168, 255));
            BorderThickness = new Thickness(3);
            CornerRadius = new CornerRadius(20);
            Background = Links.Brushes.Interface.SchemaBack;
            canvas = new Canvas();
            Child = canvas;

            Common.PutLabel(canvas, 50, 5, 30, "Выберите эмблему нового флота", Brushes.White);

            Canvas CloseCanvas = Common.GetCloseCanvas(true);
            canvas.Children.Add(CloseCanvas);
            Canvas.SetLeft(CloseCanvas, 810);
            Canvas.SetTop(CloseCanvas, -5);

            Canvas OkButton = Common.GetOkCanvas();
            canvas.Children.Add(OkButton);
            Canvas.SetLeft(OkButton, 400);
            Canvas.SetTop(OkButton, 380);
            OkButton.PreviewMouseDown += OkButton_PreviewMouseDown;

            Border emblemborder = new Border();
            emblemborder.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 168, 255));
            emblemborder.BorderThickness = new Thickness(1);
            emblemborder.Width = 655;
            emblemborder.Height = 320;
            Canvas.SetTop(emblemborder, 50);
            canvas.Children.Add(emblemborder);
            emblemborder.CornerRadius = new CornerRadius(20);

            Grid grid = new Grid();
            emblemborder.Child = grid;
            grid.ColumnDefinitions.Add(new ColumnDefinition()); grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition()); grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition()); grid.ColumnDefinitions[0].Width = new GridLength(150);

            panel1 = new ScrolledPanelFleet(new Path[] { GetPath(0), GetPath(1), GetPath(2), GetPath(3), GetPath(4), GetPath(5), GetPath(6), GetPath(7), GetPath(8), GetPath(9) }, 5);
            grid.Children.Add(panel1);
            Grid.SetColumn(panel1, 1);
            panel2 = new ScrolledPanelFleet(new Rectangle[] { GetColor(0), GetColor(1), GetColor(2), GetColor(3), GetColor(4), GetColor(5), GetColor(6), GetColor(7),
                GetColor(8), GetColor(9), GetColor(10), GetColor(11), GetColor(12)}, 5);
            grid.Children.Add(panel2);
            Grid.SetColumn(panel2, 2);

            List<Label> arr3 = new List<Label>();
            for (int i = 0; i < FleetEmblem.Volumes.Length; i++)
                arr3.Add(GetNumberLabel(FleetEmblem.Volumes[i].ToString()));
            panel3 = new ScrolledPanelFleet(arr3.ToArray(), 5);
            grid.Children.Add(panel3);
            Grid.SetColumn(panel3, 4);

            List<Label> arr4 = new List<Label>();
            for (int i = 0; i < FleetEmblem.Numbers.Length; i++)
                arr4.Add(GetNumberLabel(FleetEmblem.Numbers[i].ToString()));
            panel4 = new ScrolledPanelFleet(arr4.ToArray(), 5);
            grid.Children.Add(panel4);
            Grid.SetColumn(panel4, 3);

            Emblem = new FleetEmblem(0);
            grid.Children.Add(Emblem);
            Grid.SetColumn(Emblem, 0);

            Rectangle Helper = new Rectangle(); Helper.Width = 150;
            canvas.Children.Add(Helper); Canvas.SetLeft(Helper, 686);
            Random rnd = new Random();
            if (rnd.Next(10) != 0)
            {
                Helper.Height = 318; Canvas.SetTop(Helper, 106); Helper.Fill = Links.Brushes.Helper2_2;
            }
            else
            {
                Helper.Height = 311; Canvas.SetTop(Helper, 113); Helper.Fill = Links.Brushes.Helper2_1;
            }

        }

        private void OkButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            string result = Events.CreateFleet(Sector, Emblem.Array);
            if (result == "")
            {
                Links.Controller.PopUpCanvas.Change(new SimpleInfoMessage(Links.Interface("FleetCreated")),"При создании флота");
                if (Links.Controller.CurPanel == GamePanels.Colonies)
                {
                    Gets.GetTotalInfo("При создании нового флота");
                    Colonies.InnerRefresh();
                }
            }
            else
            {
                Links.Controller.PopUpCanvas.Change(new SimpleInfoMessage(result),"Ошибка при создании флота");
            }
        }
        public Rectangle GetColor(byte pos)
        {
            Rectangle rec = new Rectangle();
            rec.Width = 50;
            rec.Height = 50;
            rec.Stroke = Brushes.White;
            rec.StrokeThickness = 1;
            rec.HorizontalAlignment = HorizontalAlignment.Center;
            rec.VerticalAlignment = VerticalAlignment.Center;
            rec.Fill = FleetEmblem.EmblemBrushes[pos];
            return rec;
        }

        public Path GetPath(byte pos)
        {
            Path res = FleetEmblem.GetFigure(pos);
            ScaleTransform scale = new ScaleTransform(0.5, 0.5, 50, 50);
            res.RenderTransform = scale;

            //res.RenderTransformOrigin = new Point(0.5, 0.5);
            //res.Width = 100;
            //res.Height = 100;
            res.Margin = new Thickness(0, -28, 0, -28);
            res.Fill = Brushes.White;
            return res;
        }
        public Label GetNumberLabel(string text)
        {
            Label lbl = new Label();
            lbl.Content = text;
            lbl.Style = Links.TextStyle;
            lbl.Foreground = Brushes.White;
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            lbl.VerticalAlignment = VerticalAlignment.Center;
            lbl.FontSize = 35;
            return lbl;
        }
    }
    class FleetParamsPanel : Border
    {
        public GSFleet Fleet;
        Canvas mainCanvas;
        public FleetParamsPanel(GSFleet fleet)
        {
            Fleet = fleet;
            Width = 850;
            Height = 430;
            BorderBrush = new SolidColorBrush(Color.FromRgb(0, 168, 255));
            BorderThickness = new Thickness(3);
            CornerRadius = new CornerRadius(20);

            Background = Links.Brushes.Interface.SchemaBack;

            mainCanvas = new Canvas();
            Child = mainCanvas;

            Common.PutLabel(mainCanvas, (int)(30), 5, 30, Links.Interface("FleetCommands"), Brushes.White);

            Border emblemborder = new Border();
            emblemborder.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 168, 255));
            emblemborder.BorderThickness = new Thickness(1);
            emblemborder.Width = 650;
            emblemborder.Height = 320;
            Canvas.SetTop(emblemborder, 50);
            mainCanvas.Children.Add(emblemborder);
            emblemborder.CornerRadius = new CornerRadius(20);

            Canvas canvas = new Canvas();
            emblemborder.Child = canvas;

            if (Fleet.Target == null)
            {
                

                FleetScoutButton scoutbtn = new FleetScoutButton(Fleet, null, 0.5);
                canvas.Children.Add(scoutbtn); Canvas.SetLeft(scoutbtn, 10); Canvas.SetTop(scoutbtn, 10);
                PillagePanel pillage = new PillagePanel(null, Fleet, 0.5);
                canvas.Children.Add(pillage); Canvas.SetLeft(pillage, 210); Canvas.SetTop(pillage, 10);
               

            }
            else if (Fleet.Target.Order == FleetOrder.InBattle)
            {
                FleetInBattleButton battlebutton = new FleetInBattleButton(fleet, 0.5);
                canvas.Children.Add(battlebutton); Canvas.SetLeft(battlebutton, 250); Canvas.SetTop(battlebutton, 80);
            }
            else if (Fleet.Target.Order == FleetOrder.Defense)
            {
                FleetReturnButton returnscout = new FleetReturnButton(Fleet, 0.5);
                canvas.Children.Add(returnscout); Canvas.SetLeft(returnscout, 250); Canvas.SetTop(returnscout, 80);
            }
           
           
            Canvas CloseCanvas = Common.GetCloseCanvas(true);
            mainCanvas.Children.Add(CloseCanvas); Canvas.SetLeft(CloseCanvas, 800); Canvas.SetTop(CloseCanvas, -5);

            Rectangle Helper = new Rectangle(); Helper.Width = 150;
            mainCanvas.Children.Add(Helper); Canvas.SetLeft(Helper, 686);
            Random rnd = new Random();
            if (rnd.Next(10)!=0)
            {
                Helper.Height = 318; Canvas.SetTop(Helper, 106); Helper.Fill = Links.Brushes.Helper2_2;
            }
            else
            {
                Helper.Height = 311; Canvas.SetTop(Helper, 113); Helper.Fill = Links.Brushes.Helper2_1;
            }
        }
        public class FleetReturnButton:Border
        {
            public GSFleet Fleet;
            public FleetReturnButton(GSFleet fleet, double size)
            {
                Fleet = fleet;
                Width = 400 * size; Height = 300 * size; CornerRadius = new CornerRadius(20 * size); BorderBrush = Links.Brushes.SkyBlue;
                BorderThickness = new Thickness(4 * size);
                Background = Brushes.Black;
                Canvas canvas = new Canvas(); Child = canvas;
                InterfaceButton ScoutButton = new InterfaceButton((int)(320 * size), (int)(80 * size), (int)(7 * size), (int)(40 * size));
                ScoutButton.SetText(Links.Interface("FleetReturn"));
                ScoutButton.PutToCanvas(canvas, (int)(40 * size), (int)(210 * size));
                Rectangle rect;
                if (fleet.Target.Mission == FleetMission.Scout)
                    rect = Common.GetRectangle((int)(200 * size), Links.Brushes.FleetScoutBrush, null);
                else if (fleet.Target.Mission == FleetMission.Pillage)
                    rect = Common.GetRectangle((int)(200 * size), Links.Brushes.FleetPillageBrush, null);
                else if (fleet.Target.Mission==FleetMission.Conquer)
                    rect=Common.GetRectangle((int)(800*size), Links.Brushes.FleetConquerBrush, null);
                else
                    throw new Exception();
                canvas.Children.Add(rect); Canvas.SetLeft(rect, (int)(100 * size)); Canvas.SetTop(rect, (int)(5 * size));
                PreviewMouseDown += FleetReturnButton_PreviewMouseDown;
            }

            private void FleetReturnButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            {
                string eventresult = Events.RemoveFleet(Fleet);
                if (eventresult == "")
                {
                    Gets.GetTotalInfo("После события по возвращению флота");
                    Links.Controller.PopUpCanvas.Remove();
                    if (Links.Controller.CurPanel == GamePanels.Galaxy) Links.Controller.SelectPanel(GamePanels.Galaxy, SelectModifiers.None);
                    else if (Links.Controller.CurPanel == GamePanels.FleetsCanvas) Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.None);
                }
                else Links.Controller.PopUpCanvas.Change(new SimpleInfoMessage(eventresult), "Информация об ошибке при возврате флота");
            }
        
       }
        public class FleetInBattleButton:Border
        {
            public GSFleet Fleet;
            public FleetInBattleButton(GSFleet fleet, double size)
            {
                Fleet = fleet;
                Width = 400 * size; Height = 300 * size; CornerRadius = new CornerRadius(20 * size); BorderBrush = Links.Brushes.SkyBlue;
                BorderThickness = new Thickness(4 * size);
                Background = Brushes.Black;
                Canvas canvas = new Canvas(); Child = canvas;
                InterfaceButton BattleButton = new InterfaceButton((int)(320 * size), (int)(80 * size), (int)(7 * size), (int)(40 * size));
                BattleButton.SetText(Links.Interface("EnterInBattle"));
                BattleButton.PutToCanvas(canvas, (int)(40 * size), (int)(210 * size));
                Rectangle battlerect = Common.GetRectangle((int)(200 * size), Links.Brushes.TwoSwords, null);
                canvas.Children.Add(battlerect); Canvas.SetLeft(battlerect, (int)(100 * size)); Canvas.SetTop(battlerect, (int)(5 * size));
                PreviewMouseDown += FleetInBattleButton_PreviewMouseDown;
            }

            private void FleetInBattleButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            {
                Links.Helper.Fleet = Fleet;
                FleetMissions.EnterInBattle_btn(null, null);
            }
        }
        public class FleetScoutButton:Border
        {
            public GSFleet Fleet;
            public GSStar Star;
            public FleetScoutButton (GSFleet fleet, GSStar star, double size)
            {
                Fleet = fleet; Star = star;
                Width = 400*size; Height = 300*size; CornerRadius = new CornerRadius(20*size); BorderBrush = Links.Brushes.SkyBlue;
                BorderThickness = new Thickness(4*size);
                Background = Brushes.Black;
                Canvas canvas = new Canvas(); Child = canvas;
                InterfaceButton ScoutButton = new InterfaceButton((int)(320*size), (int)(80*size), (int)(7*size), (int)(40*size));
                ScoutButton.SetText(Links.Interface("FleetScout"));
                ScoutButton.PutToCanvas(canvas, (int)(40*size), (int)(210*size));
                Rectangle scoutrect = Common.GetRectangle((int)(200*size), Links.Brushes.FleetScoutBrush, null);
                canvas.Children.Add(scoutrect); Canvas.SetLeft(scoutrect, (int)(100*size)); Canvas.SetTop(scoutrect, (int)(5*size));
                PreviewMouseDown += FleetScoutButton_PreviewMouseDown;
            }

            private void FleetScoutButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            {
                if (Fleet==null)
                {
                    Links.Helper.Star = Star;
                    Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.FleetForScout);
                }
                else if (Star==null)
                {
                    Links.Helper.Fleet = Fleet;
                    FleetMissions.ScoutFleet_Click(null, null);
                }
            }
        }
        /*void AddNoRule()
        {
            Canvas NoRule = new Canvas();
            NoRule.Width = 336;
            NoRule.Height = 320;
            NoRule.Clip = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M0,20 a20,20 0 0,1 20,-20 h296 a20,20 0 0,1 20,20 v280 a20,20 0 0,1 -20,20 h-296 a20,20 0 0,1 -20,-20z"));
            NoRule.ClipToBounds = true;
            NoRule.Background = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
            mainCanvas.Children.Add(NoRule);
            Canvas.SetZIndex(NoRule, 5);
            Canvas.SetLeft(NoRule, 650);
            Canvas.SetTop(NoRule, 50);
            TextBlock norule = Common.GetBlock(36, Links.Interface("NoRule"), Links.Brushes.SkyBlue, new Thickness());
            norule.Width = 336;
            NoRule.Children.Add(norule);
            Canvas.SetTop(norule, 140);
        }*/
        
        public Label GetNumberLabel(string text)
        {
            Label lbl = new Label();
            lbl.Content = text;
            lbl.Style = Links.TextStyle;
            lbl.Foreground = Brushes.White;
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            lbl.VerticalAlignment = VerticalAlignment.Center;
            lbl.FontSize = 35;
            return lbl;
        }

        public Rectangle GetColor(byte pos)
        {
            Rectangle rec = new Rectangle();
            rec.Width = 50;
            rec.Height = 50;
            rec.Stroke = Brushes.White;
            rec.StrokeThickness = 1;
            rec.HorizontalAlignment = HorizontalAlignment.Center;
            rec.VerticalAlignment = VerticalAlignment.Center;
            rec.Fill = FleetEmblem.EmblemBrushes[pos];
            return rec;
        }

        public Path GetPath(byte pos)
        {
            Path res = FleetEmblem.GetFigure(pos);
            ScaleTransform scale = new ScaleTransform(0.5, 0.5, 50, 50);
            res.RenderTransform = scale;

            //res.RenderTransformOrigin = new Point(0.5, 0.5);
            //res.Width = 100;
            //res.Height = 100;
            res.Margin = new Thickness(0, -28, 0, -28);
            res.Fill = Brushes.White;
            return res;
        }

    }
    class FleetLeaveButton : Canvas
    {
        Path btn1, btn2, btn3;
        RadialGradientBrush brushOn;
        RadialGradientBrush brushOff;

        Label Text1;
        Label Text2;
        Label Text3;

        public byte Value;

        public FleetLeaveButton(byte value)
        {
            Width = 270;
            Height = 40;
            ClipToBounds = true;
            Path path = new Path();
            path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M20,0 h230 a20,20 0 0,1 0,40 h-230 a20,20 0 0,1 0,-40"));
            path.Stroke = Brushes.White;
            path.Fill = Common.GetLinearBrush(Colors.Gray, Colors.White, Colors.Gray);
            Children.Add(path);

            brushOn = new RadialGradientBrush(); brushOn.GradientOrigin = new Point(0.8, 0.3);
            brushOn.GradientStops.Add(new GradientStop(Colors.Black, 1));
            brushOn.GradientStops.Add(new GradientStop(Colors.Green, 0));

            brushOff = new RadialGradientBrush(); brushOff.GradientOrigin = new Point(0.8, 0.3);
            brushOff.GradientStops.Add(new GradientStop(Colors.Black, 1));
            brushOff.GradientStops.Add(new GradientStop(Colors.White, 0));

            btn1 = PutButton("M0,20 a18,18 0 1,1 0,0.1", 80);
            btn2 = PutButton("M0,20 a18,18 0 1,1 0,0.1", 155);
            btn3 = PutButton("M0,20 a18,18 0 1,1 0,0.1", 230);

            Common.PutRect(this, 30, new DrawingBrush(WeapCanvas.WarpDraw), 2, 5, false);
            //Common.PutRect(this, 30, Links.Brushes.RepairPict, 2, 5, false);

            Text1 = Common.PutLabel(this, 40, 5, 20, "20%");
            Text1.Foreground = Brushes.Black;

            Text2 = Common.PutLabel(this, 115, 5, 20, "50%");
            Text2.Foreground = Brushes.Black;

            Text3 = Common.PutLabel(this, 190, 5, 20, "80%");
            Text3.Foreground = Brushes.Black;

            btn1.PreviewMouseDown += new MouseButtonEventHandler(btn_PreviewMouseDown);
            btn2.PreviewMouseDown += new MouseButtonEventHandler(btn_PreviewMouseDown);
            btn3.PreviewMouseDown += new MouseButtonEventHandler(btn_PreviewMouseDown);
            if (value == 20)
                btn_PreviewMouseDown(btn1, null);
            else if (value == 50)
                btn_PreviewMouseDown(btn2, null);
            else if (value == 80)
                btn_PreviewMouseDown(btn3, null);
        }


        Path PutButton(string content, int left)
        {
            Path btn = new Path();
            btn.Stroke = Brushes.Black;
            btn.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(content));
            btn.Fill = brushOff;
            Children.Add(btn);
            Canvas.SetLeft(btn, left);
            return btn;
        }
        public void SetWarpValue(byte value)
        {
            Value = value;
            switch (Value)
            {
                case 0: btn1.Fill = brushOff; btn2.Fill = brushOff; btn3.Fill = brushOff;
                    Text1.Foreground = Brushes.Black; Text2.Foreground = Brushes.Black; Text3.Foreground = Brushes.Black; break;
                case 20: btn1.Fill = brushOn; btn2.Fill = brushOff; btn3.Fill = brushOff;
                    Text1.Foreground = Brushes.Green; Text2.Foreground = Brushes.Black; Text3.Foreground = Brushes.Black; break;
                case 50: btn1.Fill = brushOff; btn2.Fill = brushOn; btn3.Fill = brushOff;
                    Text1.Foreground = Brushes.Black; Text2.Foreground = Brushes.Green; Text3.Foreground = Brushes.Black; break;
                case 80: btn1.Fill = brushOff; btn2.Fill = brushOff; btn3.Fill = brushOn;
                    Text1.Foreground = Brushes.Black; Text2.Foreground = Brushes.Black; Text3.Foreground = Brushes.Green; break;
            }
        }
        void btn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Path btn = (Path)sender;
            if (btn == btn1)
            {
                if (Value != 20) SetWarpValue(20); else SetWarpValue(0);
            }
            else if (btn == btn2)
            {
                if (Value != 50) SetWarpValue(50); else SetWarpValue(0);
            }
            else
            {
                if (Value != 80) SetWarpValue(80); else SetWarpValue(0);
            }

        }

    }
    class SwitchOnButton : Canvas
    {
        Path btn;
        RadialGradientBrush brushOn;
        RadialGradientBrush brushOff;
        public bool Status = false;
        Label Text;
        public SwitchOnButton(bool status)
        {
            Status = status;
            Width = 130;
            Height = 40;
            Path path = new Path();
            path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M20,0 h90 a20,20 0 0,1 0,40 h-90 a20,20 0 0,1 0,-40"));
            path.Stroke = Brushes.White;
            path.Fill = Common.GetLinearBrush(Colors.Gray, Colors.White, Colors.Gray);
            Children.Add(path);

            brushOn = new RadialGradientBrush(); brushOn.GradientOrigin = new Point(0.8, 0.3);
            brushOn.GradientStops.Add(new GradientStop(Colors.Black, 1));
            brushOn.GradientStops.Add(new GradientStop(Colors.Green, 0));

            brushOff = new RadialGradientBrush(); brushOff.GradientOrigin = new Point(0.8, 0.3);
            brushOff.GradientStops.Add(new GradientStop(Colors.Black, 1));
            brushOff.GradientStops.Add(new GradientStop(Colors.White, 0));

            btn = new Path();
            btn.Stroke = Brushes.White;
            btn.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,20 a18,18 0 1,1 0,0.1"));
            btn.Fill = brushOff;
            Children.Add(btn);
            Canvas.SetLeft(btn, 90);

            Common.PutRect(this, 30, Links.Brushes.RepairPict, 2, 5, false);
            if (Status)
            { Text = Common.PutLabel(this, 40, 5, 20, "AUTO"); btn.Fill = brushOn; }
            else
            { Text = Common.PutLabel(this, 40, 5, 20, "SEMI"); btn.Fill = brushOff; }
            Text.Foreground = Brushes.Green;

            btn.PreviewMouseDown += new MouseButtonEventHandler(btn_PreviewMouseDown);
        }

        void btn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Status)
            {
                Status = false;
                btn.Fill = brushOff;
                Text.Content = "SEMI";
            }
            else
            {
                Status = true;
                btn.Fill = brushOn;
                Text.Content = "AUTO";
            }
        }

    }
    class SpeedIndicator : Canvas
    {
        Path path;
        ArcSegment arc;
        LineSegment seg;
        DispatcherTimer timer;
        double targetangle;
        double curangle;
        Label lbl1;
        Label lbl2;
        int MathPower;
        int Ships;
        public SpeedIndicator(byte start, int Math, int ships)
        {
            MathPower = Math; Ships = ships;
            Width = 200;
            Height = 200;
            path = new Path();
            PathFigure figure = new PathFigure();
            figure.StartPoint = new Point(100, 200);
            figure.Segments.Add(new LineSegment(GetPoint(-45), true));
            figure.Segments.Add(new ArcSegment(GetPoint(45), new Size(150, 150), 0, false, SweepDirection.Clockwise, true));
            figure.IsClosed = true;
            PathGeometry geom = new PathGeometry();
            geom.Figures.Add(figure);
            path.Data = geom;
            path.Stroke = Brushes.White;
            Children.Add(path);
            path.Fill = Brushes.Gray;
            this.PreviewMouseDown += new MouseButtonEventHandler(path_PreviewMouseDown);

            curangle = start / 100.0 * 90.0 - 45;
            targetangle = curangle;
            PathFigure figure1 = new PathFigure();
            figure1.StartPoint = new Point(100, 200);
            figure1.Segments.Add(new LineSegment(GetPoint(-45), true));
            arc = new ArcSegment(GetPoint(curangle), new Size(150, 150), 0, false, SweepDirection.Clockwise, true);
            figure1.Segments.Add(arc);
            figure1.IsClosed = true;

            PathFigure figure2 = new PathFigure();
            figure2.StartPoint = new Point(100, 200);
            seg = new LineSegment(GetPoint(curangle), true);
            figure2.Segments.Add(seg);
            figure2.Segments.Add(new ArcSegment(GetPoint(45), new Size(150, 150), 0, false, SweepDirection.Clockwise, true));
            figure2.IsClosed = true;

            PathGeometry geom1 = new PathGeometry();
            geom1.Figures.Add(figure1);
            PathGeometry geom2 = new PathGeometry();
            geom2.Figures.Add(figure2);

            Path path1 = new Path();
            path1.StrokeLineJoin = PenLineJoin.Round;
            path1.StrokeThickness = 3;
            path1.Stroke = Brushes.White;
            path1.Data = geom1;
            path1.Fill = GetBrush(Colors.Blue);
            Children.Add(path1);

            Path path2 = new Path();
            path2.StrokeLineJoin = PenLineJoin.Round;
            path2.StrokeThickness = 3;
            path2.Stroke = Brushes.White;
            path2.Data = geom2;
            path2.Fill = GetBrush(Colors.Red);
            Children.Add(path2);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Tick += new EventHandler(timer_Tick);

            UIElement health = Common.GetRectangle(30, Pictogram.HealthBrush, Links.Interface("WarpHealth"));
            //UIElement health = Pictogram.GetPict(Picts.Health, Target.None, false, Links.Interface("WarpHealth"));
            Children.Add(health); Canvas.SetLeft(health, 5); Canvas.SetTop(health, 170);
            UIElement speed = Common.GetRectangle(30, Pictogram.FleetSpeedBrush, Links.Interface("FleetsSpeed"));
            //UIElement speed = Pictogram.GetPict(Picts.Speed, Target.None, false, Links.Interface("FleetsSpeed"));
            Children.Add(speed); Canvas.SetLeft(speed, 165); Canvas.SetTop(speed, 170);

            lbl1 = Common.PutLabel(this, 5, 140, 20, "", Brushes.White);

            lbl2 = Common.PutLabel(this, 165, 140, 20, "", Brushes.White);

            Draw(curangle);
        }
        public byte GetSpeedValue()
        {
            return (byte)((targetangle + 45) / 90.0 * 100);
        }
        void timer_Tick(object sender, EventArgs e)
        {
            double dx = targetangle - curangle;
            double step = 0;
            if (Math.Abs(dx) > 3) step = dx > 0 ? 3 : -3;
            else { step = dx; timer.Stop(); };
            curangle += step;
            Draw(curangle);
        }
        Brush GetBrush(Color color)
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(color, 0.8));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            brush.GradientStops.Add(new GradientStop(color, 0.2));
            return brush;
        }
        void path_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult result = VisualTreeHelper.HitTest(path, e.GetPosition(this));
            if (result == null) return;
            Point pt = path.TranslatePoint(e.GetPosition(path), path);
            double length = Math.Sqrt(Math.Pow(pt.X - 100, 2) + Math.Pow(pt.Y - 200, 2));
            double sin = (pt.X - 100) / length;
            double angle = Math.Asin(sin) * 180 / Math.PI;
            targetangle = angle;
            if (!timer.IsEnabled) timer.Start();
            //Draw((angle + 45) / 90.0);
        }

        void Draw(double angle)
        {
            arc.Point = GetPoint(angle);
            seg.Point = GetPoint(angle);
            int curspeed = 5;// GSFleet.GetSpeed(MathPower, Ships, (byte)(((angle + 45) / 90) * 100));
            lbl2.Content = curspeed.ToString();
            lbl1.Content = 1000;// GSFleet.GetHealth(MathPower, Ships, (byte)(((angle + 45) / 90) * 100));
        }
        public double GetX(double angle, double length)
        {
            return length * Math.Sin(angle * Math.PI / 180.0);
        }
        public double GetY(double angle, double length)
        {
            return length * Math.Cos(angle * Math.PI / 180.0);
        }
        Point GetPoint(double angle)
        {
            return new Point(100 + GetX(angle, 150), 200 - GetY(angle, 150));
        }
        static bool CheckParams(int math, int ships, int speed)
        {
            throw new Exception();/*
            //ТУТ НЕ ТАК
            int sumships = GSFleet.ShipSum[ships, 1];// GSFleet.CalcShipSum(ships);
            int sumspeed = GSFleet.SpeedSum[1, ships-1, speed]; //GSFleet.CalcSpeedSum(ships, speed);
            if ((sumspeed + sumships) > math) return false;
            else return true;*/
        }

    }
    public class FleetEmblem : Canvas
    {
        public double Size { get; private set; }
        public int Image;
        public byte[] Array;
        public static List<RadialGradientBrush> EmblemBrushes = GetBasicColors();
        static GlyphTypeface gt = new GlyphTypeface(new Uri("pack://application:,,,/resources/Agru.ttf", UriKind.Absolute));
        public static char[] Volumes = new char[] { ' ', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'X', 'Y', 'Z' };
        public static char[] Numbers = new char[] { ' ', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        public FleetEmblem(byte[] array)
        {
            Size = 100;
            Image = BitConverter.ToInt32(array, 0);
            Array = array;
            Draw();
        }
        public FleetEmblem(byte[] array, double size)
        {
            Size = size;
            Image = BitConverter.ToInt32(array, 0);
            Array = array;
            Draw();
        }
        public FleetEmblem(int image)
        {
            Image = image;
            Array = BitConverter.GetBytes(image);
            Size = 100;
            Draw();
        }
        public FleetEmblem(int image, double size)
        {
            Image = image;
            Array = BitConverter.GetBytes(image);
            Size = size;
            Draw();
        }
        public void SetImage(byte[] array)
        {
            Children.Clear();
            Image = BitConverter.ToInt32(array, 0);
            Array = array;
            Draw();
        }
        void Draw()
        {
            Width = Size;
            Height = Size;
            switch (Array[0])
            {
                case 252: Background = Links.Brushes.PirateBrush; return;
                case 251: Background = Links.Brushes.GreenTeamBrush; return;
                case 254: Background = Links.Brushes.TechTeamBrush; return;
                case 255: Background = Links.Brushes.MercTeamBrush; return;
                case 253: Background = Links.Brushes.AlienBrush; return;
            }
            DrawingGroup group = new DrawingGroup();
            Pen pen = new Pen(Brushes.Black, 1); pen.LineJoin = PenLineJoin.Round;
            RadialGradientBrush brush = EmblemBrushes[Array[1] % EmblemBrushes.Count];
            string form = "";
            Point baseLine;
            switch (Array[0])
            {
                case 0: form = "M50,0 a50,50 0 1,1 -0.1,0 M0,0 v0.01 M100,100 h-0.01"; baseLine = new Point(30, 70); break;
                case 1: form = "M1,1 h98 v98 h-98z"; baseLine = new Point(30, 65); break;
                case 2: form = "M0,50 L25,7 L75,7 L100,50 L75,93 L25,93z M0,0 v0.01 M100,100 h-0.01"; baseLine = new Point(30, 70); break;
                case 3: form = "M50,2 a55,95 0 0,0 -32.5,60 a24,27 0 0,0 65,0 a55,95 0 0,0 -32.5,-60  M0,0 v0.01 M100,100 h-0.01"; baseLine = new Point(30, 70); break;
                case 4: form = "M0,10 a80,80 0 0,1 100,0 a100,100 0 0,1 -50,90 a100,100 0 0,1 -50,-90  M0,0 v0.01 M100,100 h-0.01"; baseLine = new Point(30, 60); break;
                case 5: form = "M50,0 l18,35 h32 l-25,16 l25,49 l-50,-32 l-50,32 l25,-49 l-25,-16 h32zM0,0 v0.01 M100,100 h-0.01"; baseLine = new Point(30, 70); break;
                case 6: form = "M50,0 l10,20 l20,20 l20,10 l-20,10 l-20,20 l-10,20 l-10,-20 l-20,-20 l-20,-10 l20,-10 l20,-20z M0,0 v0.01 M100,100 h-0.01"; baseLine = new Point(30, 70); break;
                case 7: form = "M50,0 l10,20 l20,20 l20,10 l-20,10 l-10,10 l10,30 l-30,-20 l-30,20 l10,-30 l-10,-10 l-20,-10 l20,-10 l20,-20zM0,0 v0.01 M100,100 h-0.01"; baseLine = new Point(30, 70); break;
                case 8: form = "M0,0 l50,20 l50,-20 l-20,50 l20,50 l-50,-20 l-50,20 l20,-50z M0,0 v0.01 M100,100 h-0.01"; baseLine = new Point(30, 70); break;
                case 9: form = "M50,0 a15,12 0 0,0 0,20 a40,40 0 0,1 50,30 a12,15 0 0,0 -20,0 a40,40 0 0,1 -30,50 a15,12 0 0,0 0,-20" +
                                             "a40,40 0 0,1 -50,-30 a12,15 0 0,0 20,0 a40,40 0 0,1 30,-50 M0,0 v0.01 M100,100 h-0.01"; baseLine = new Point(30, 70); break;
                default: baseLine = new Point(); break;
            }
            group.Children.Add(new GeometryDrawing(brush, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(form))));
            GlyphRun theGlyphRun = new GlyphRun(gt, 0, false, 50, new ushort[] { gt.CharacterToGlyphMap[(ushort)Volumes[Array[2] % Volumes.Length]], gt.CharacterToGlyphMap[(ushort)Numbers[Array[3] % Numbers.Length]] },
            baseLine, new double[] { 22, 20 }, null, null, null, null, null, null);


            GlyphRunDrawing gDrawing = new GlyphRunDrawing(Brushes.Black, theGlyphRun);

            group.Children.Add(gDrawing);
            Background = new DrawingBrush(group);
        }

        public static Path GetFigure(int pos)
        {
            Path path = new Path();
            path.Width = 100; path.Height = 100;
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 2;
            string form = "";
            switch (pos)
            {
                case 0: form = "M50,0 a50,50 0 1,1 -0.1,0 M0,0 v0.01 M100,100 h-0.01"; break;
                case 1: form = "M1,1 h98 v98 h-98z"; break;
                case 2: form = "M0,50 L25,7 L75,7 L100,50 L75,93 L25,93z M0,0 v0.01 M100,100 h-0.01"; break;
                case 3: form = "M50,2 a55,95 0 0,0 -32.5,60 a24,27 0 0,0 65,0 a55,95 0 0,0 -32.5,-60  M0,0 v0.01 M100,100 h-0.01"; break;
                case 4: form = "M0,10 a80,80 0 0,1 100,0 a100,100 0 0,1 -50,90 a100,100 0 0,1 -50,-90  M0,0 v0.01 M100,100 h-0.01"; break;
                case 5: form = "M50,0 l18,35 h32 l-25,16 l25,49 l-50,-32 l-50,32 l25,-49 l-25,-16 h32zM0,0 v0.01 M100,100 h-0.01"; break;
                case 6: form = "M50,0 l10,20 l20,20 l20,10 l-20,10 l-20,20 l-10,20 l-10,-20 l-20,-20 l-20,-10 l20,-10 l20,-20z M0,0 v0.01 M100,100 h-0.01"; break;
                case 7: form = "M50,0 l10,20 l20,20 l20,10 l-20,10 l-10,10 l10,30 l-30,-20 l-30,20 l10,-30 l-10,-10 l-20,-10 l20,-10 l20,-20zM0,0 v0.01 M100,100 h-0.01"; break;
                case 8: form = "M0,0 l50,20 l50,-20 l-20,50 l20,50 l-50,-20 l-50,20 l20,-50z M0,0 v0.01 M100,100 h-0.01"; break;
                case 9: form = "M50,0 a15,12 0 0,0 0,20 a40,40 0 0,1 50,30 a12,15 0 0,0 -20,0 a40,40 0 0,1 -30,50 a15,12 0 0,0 0,-20" +
                                             "a40,40 0 0,1 -50,-30 a12,15 0 0,0 20,0 a40,40 0 0,1 30,-50 M0,0 v0.01 M100,100 h-0.01"; break;
            }
            path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(form));

            return path;
        }

        static List<RadialGradientBrush> GetBasicColors()
        {
            List<RadialGradientBrush> list = new List<RadialGradientBrush>();
            list.Add(Common.GetRadialBrush(Colors.Blue, 0.8, 0.3));
            list.Add(Common.GetRadialBrush(Colors.Red, 0.8, 0.3));
            list.Add(Common.GetRadialBrush(Colors.Green, 0.8, 0.3));
            list.Add(Common.GetRadialBrush(Colors.Gold, 0.8, 0.3));
            list.Add(Common.GetRadialBrush(Colors.Silver, 0.8, 0.3));
            list.Add(Common.GetRadialBrush(Colors.Black, 0.8, 0.3));
            list.Add(Common.GetRadialBrush(Colors.Purple, 0.8, 0.3));
            list.Add(Common.GetRadialBrush(Colors.Violet, 0.8, 0.3));
            list.Add(Common.GetRadialBrush(Colors.Orange, 0.8, 0.3));
            list.Add(Common.GetRadialBrush(Colors.SkyBlue, 0.8, 0.3));
            list.Add(Common.GetRadialBrush(Colors.Brown, 0.8, 0.3));
            list.Add(Common.GetRadialBrush(Colors.Navy, 0.8, 0.3));
            list.Add(Common.GetRadialBrush(Colors.YellowGreen, 0.8, 0.3));
            return list;
        }
    }

    public class ScrolledPanelFleet : StackPanel
    {
        object[] Array;
        int Elements;
        public byte currentid = 0;

        public ScrolledPanelFleet(object[] array, int elements)
        {
            VerticalAlignment = VerticalAlignment.Center;
            //Background = Brushes.DarkGray;
            Array = array;
            Elements = elements;
            Width = 110;
            Place();
            PreviewMouseWheel += new MouseWheelEventHandler(ScrolledPanel_PreviewMouseWheel);
        }

        void ScrolledPanel_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0) currentid = (byte)((currentid - 1 + Array.Length) % Array.Length);
            else currentid = (byte)((currentid + 1) % Array.Length);
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
                lbl.HorizontalAlignment = HorizontalAlignment.Center;
                lbl.VerticalAlignment = VerticalAlignment.Center;
                lbl.Height = 60;
                if ((pos) == currentid) lbl.Background = Brushes.Red;
                //lbl.FontWeight = FontWeights.Bold;
                Border bord = new Border();
                bord.BorderBrush = Brushes.White;
                bord.BorderThickness = new Thickness(1);
                bord.Child = lbl;
                Children.Add(bord);

                bord.Tag = pos;
                bord.PreviewMouseDown += new MouseButtonEventHandler(bord_PreviewMouseDown);

                byte[] list = new byte[4];
                if (CreateFleetPanel.panel1 != null) list[0] = CreateFleetPanel.panel1.currentid;
                if (CreateFleetPanel.panel2 != null) list[1] = CreateFleetPanel.panel2.currentid;
                if (CreateFleetPanel.panel3 != null) list[2] = CreateFleetPanel.panel3.currentid;
                if (CreateFleetPanel.panel4 != null) list[3] = CreateFleetPanel.panel4.currentid;
                if (CreateFleetPanel.Emblem != null)
                    CreateFleetPanel.Emblem.SetImage(list.ToArray());
            }
        }
    }
    /// <summary> структура, в которой описывается результат отправления флота на какое-либо задание. В том числе ID боя или иная текстовая информация </summary>
    struct StartBattleEventResult
    {
        public bool Result;
        public long Battleid;
        public string ErrorText;
        public StartBattleEventResult(long battleid)
        {
            Result = true; Battleid = battleid; ErrorText = "";
        }
        public StartBattleEventResult(string text)
        {
            Result = false; Battleid = -1; ErrorText = text;
        }
        public StartBattleEventResult(bool result, string text)
        {
            Result = result; Battleid = -1; ErrorText = text;
        }
    }
    class StartBattlePanel : Border
    {
        long BattleID;
        public StartBattlePanel(long battleID)
        {
            BattleID = battleID;
            Width = 600;
            Height = 420;
            BorderBrush = new SolidColorBrush(Color.FromRgb(0, 168, 255));
            BorderThickness = new Thickness(2);
            CornerRadius = new CornerRadius(20);
            Background = Brushes.Black;
  
            Canvas mainCanvas = new Canvas();
            Child = mainCanvas;
            Rectangle rect = new Rectangle(); rect.Width = 600; rect.Height = 360;
            rect.Fill = Links.Brushes.Interface.BattleZastavka;
            mainCanvas.Children.Add(rect); Canvas.SetTop(rect, 20);
            Common.PutBlock(40, Links.Interface("BATTLE!!!"), mainCanvas, 10, 5, 590);

            InterfaceButton Enter = new InterfaceButton(150, 50, 7, 22);
            Enter.SetText(Links.Interface("Manual"));
            Enter.PutToCanvas(mainCanvas, 100, 360);
            Enter.PreviewMouseDown += Swords_PreviewMouseDown;

            InterfaceButton Auto = new InterfaceButton(150, 50, 7, 22);
            Auto.SetText(Links.Interface("Auto"));
            Auto.PutToCanvas(mainCanvas, 320, 360);
            Auto.PreviewMouseDown += Auto_PreviewMouseDown;
            /*
            InterfaceButton Close = new InterfaceButton(150, 50, 7, 22);
            Close.SetText(Links.Interface("Close"));
            Close.PutToCanvas(mainCanvas, 400, 360);
            Close.PreviewMouseDown += CloseButton_Click;*/

            Links.Controller.PopUpCanvas.Remove();
            Links.Controller.PopUpCanvas.Place(this, false);
            new MySound("Interface/BattleStart.wav");
        }
        private void Auto_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Events.SetBattleToAutoMode(BattleID);
            Gets.GetTotalInfo("После события по автоматическому расчёту боя");
            NoticeBorder.CheckNotice();
            if (Links.Controller.CurPanel == GamePanels.FleetsCanvas)
                Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.None);
            Links.Controller.PopUpCanvas.Remove();
        }

        private void Swords_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
            Links.Controller.IntBoya.Place(BattleID);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }
    }
    class FleetMissions
    {
        public static void ScoutFleet_Click(object sender, MouseButtonEventArgs e)
        {
            // FrameworkElement fe = (FrameworkElement)sender;
            //FleetParamsPanel panel = (FleetParamsPanel)fe.Tag;
            // GSFleet Fleet = panel.Fleet;
            GSFleet Fleet = Links.Helper.Fleet;
            Links.Controller.PopUpCanvas.Remove();

            if (Fleet.CheckHealth() == false)
            {
                Links.Controller.PopUpCanvas.Change(new SimpleInfoMessage(Links.Interface("FleetNotReady")), "Сообщение что флот не готов при отправке его в патруль из окна флота");
                return;
            }
            Links.Controller.PopUpCanvas.Remove();
            //Links.Helper.FleetParamsPanel = panel;
            Links.Helper.Fleet = Fleet;
            Links.Helper.ClickDelegate = StartScout_Click;
            Links.Controller.SelectPanel(GamePanels.Galaxy, SelectModifiers.StarForScout);
        }
        public static void StartScout_Click(object sender, RoutedEventArgs e)
        {
            ///флот отправляется на патрулирование. Доступность проверяется на сервере.
            GSStar star1 = Links.Helper.Fleet.FleetBase.Land.Planet.Star;
            GSStar star2 = Links.Helper.Star;
            double distance = Math.Sqrt(Math.Pow(star1.X - star2.X, 2) + Math.Pow(star1.Y - star2.Y, 2)) / 10;
            if (distance > Links.Helper.Fleet.FleetBase.Range)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("TooFar")));
                return;
            }
            if (Links.Helper.Fleet.CheckHealth() == false)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("FleetNotReady")));
                return;
            }
            StartBattleEventResult eventresult = Events.SendFleetToScout();

            if (eventresult.Result == false && eventresult.ErrorText == "")
            {
                Gets.GetTotalInfo("После события по отправке флота на разведку");
                Links.Controller.SelectPanel(GamePanels.Galaxy, SelectModifiers.None);
            }
            else if (eventresult.Result == false)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult.ErrorText));
                return;
            }
            else
            {
                new StartBattlePanel(eventresult.Battleid);
            }
        }
    
        public static void PillageEnemy_Click(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)sender;
            FleetParamsPanel panel = (FleetParamsPanel)fe.Tag;
            GSFleet Fleet = panel.Fleet;

            Links.Controller.PopUpCanvas.Remove();
            if (Fleet.CheckHealth() == false)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("FleetNotReady")), true);
                return;
            }
            //Links.Controller.SwitchSpaceObjectSelect(SpaceObjectSelect.Land);
            Links.Helper.FleetParamsPanel = panel;
            Links.Helper.ClickDelegate = PillageEnemyFinal_Click;
            Links.Controller.SelectPanel(GamePanels.Galaxy, SelectModifiers.Land);
            //Links.Controller.galaxypanel.Select();
        }
        public static void PillageEnemyFinal_Click(object sender, RoutedEventArgs e)
        {
            ///флот отправляется на грабёж. Доступность проверяется на сервере.
            GSStar star1 = Links.Helper.Fleet.FleetBase.Land.Planet.Star;
            GSStar star2 = Links.Helper.Planet.Star;
            double distance = Math.Sqrt(Math.Pow(star1.X - star2.X, 2) + Math.Pow(star1.Y - star2.Y, 2)) / 10;
            if (distance > Links.Helper.Fleet.FleetBase.Range)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("TooFar")));
                return;
            }
            if (Links.Helper.Fleet.CheckHealth() == false)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("FleetNotReady")));
                return;
            }
            if (Links.Helper.Fleet.GetActiveShips() < 5)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("MinFleetsShips")));
                return;
            }
            if (Links.Helper.Planet.PlanetSide == PlanetSide.Free || Links.Helper.Planet.PlanetSide==PlanetSide.No || Links.Helper.Planet.PlanetSide==PlanetSide.Player || Links.Helper.Planet.PlanetSide==PlanetSide.Unknown)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("BadPlanetPillage")));
                return;
            }
            StartBattleEventResult eventresult = Events.SendFleetToPillage();

            
            if (eventresult.Result == false)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult.ErrorText));
                return;
            }
            else if (eventresult.Battleid==-1)
            {
                if (Links.Controller.CurPanel == GamePanels.FleetsCanvas) Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.None);
                else if (Links.Controller.CurPanel == GamePanels.Galaxy) Links.Controller.SelectPanel(GamePanels.Galaxy, SelectModifiers.None);
            }
            else
            {
                new StartBattlePanel(eventresult.Battleid);
            }
        }
       
        public static void CaptureEnemy_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)sender;
            FleetParamsPanel panel = (FleetParamsPanel)fe.Tag;
            GSFleet Fleet = panel.Fleet;

            Links.Controller.PopUpCanvas.Remove();
            if (Fleet.CheckHealth() == false)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("FleetNotReady")), true);
                return;
            }
            //Links.Controller.SwitchSpaceObjectSelect(SpaceObjectSelect.Land);
            Links.Helper.FleetParamsPanel = panel;
            Links.Helper.Fleet = Fleet;
            Links.Helper.ClickDelegate = ConquerFinal_Click;
            Links.Controller.SelectPanel(GamePanels.Galaxy, SelectModifiers.Land);
            //Links.Controller.galaxypanel.Select();
        }
        public static void ConquerFinal_Click(object sender, RoutedEventArgs e)
        {
            ///флот отправляется на захват. Доступность проверяется на сервере.
            GSStar star1 = Links.Helper.Fleet.FleetBase.Land.Planet.Star;
            GSStar star2 = Links.Helper.Planet.Star;
            double distance = Math.Sqrt(Math.Pow(star1.X - star2.X, 2) + Math.Pow(star1.Y - star2.Y, 2)) / 10;
            if (distance > Links.Helper.Fleet.FleetBase.Range)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("TooFar")));
                return;
            }
            if (Links.Helper.Fleet.CheckHealth() == false)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("FleetNotReady")));
                return;
            }
            if (Links.Helper.Fleet.GetActiveShips() < 7)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("MinFleetsShips")));
                return;
            }
            if (Links.Helper.Planet.PlanetSide == PlanetSide.Free || Links.Helper.Planet.PlanetSide == PlanetSide.No || Links.Helper.Planet.PlanetSide == PlanetSide.Player || Links.Helper.Planet.PlanetSide == PlanetSide.Unknown)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("BadPlanetPillage")));
                return;
            }
            StartBattleEventResult eventresult = Events.SendFleetToConquer();


            if (eventresult.Result == false)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult.ErrorText));
                return;
            }
            else if (eventresult.Battleid == -1)
            {
                if (Links.Controller.CurPanel == GamePanels.FleetsCanvas) Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.None);
                else if (Links.Controller.CurPanel == GamePanels.Galaxy) Links.Controller.SelectPanel(GamePanels.Galaxy, SelectModifiers.None);
            }
            else
            {
                new StartBattlePanel(eventresult.Battleid);
            }
        }
       
        public static void EnterInBattle_btn(object sender, RoutedEventArgs e)
        {
            GSFleet Fleet = Links.Helper.Fleet;
            Links.Controller.PopUpCanvas.Remove();
            long fleetid = Fleet.ID;
            long battleid = -1;
            foreach (Battle battle in GSGameInfo.Battles.Values)
                if (battle.Fleet1ID == fleetid || battle.Fleet2ID == fleetid)
                {
                    battleid = battle.ID;
                }
            if (battleid == -1)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("BattleNotFinded")), true);
                return;
            }

            Links.Controller.IntBoya.Place(battleid);
        }
    }
}
