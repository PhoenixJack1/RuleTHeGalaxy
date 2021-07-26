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
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Client
{
    enum HandStatus { Hided, Showing, Work, Hiding }
    class ShipCreateNew:Canvas
    {
        Canvas ShipModel;
        Schema Schema;
        HexShip LastShip;
        HexShip NewShip;
        public static ShipCreateNew CurCanvas;
        static int CurModel = 0;
        static byte CurBase = 0;
        MovingLine l1, l2, l3;
        bool IsChanging = false;
        DispatcherTimer timer = new DispatcherTimer();
        int sparks = 0;
        Random rnd = new Random();
        List<Border> ShipBorders = new List<Border>();
        RotateTransform[] Rotates = new RotateTransform[5];
        Rectangle P1, P2, P3, P4;
        Rectangle P51, P52, P53;
        RotateTransform PR1, PR2, PR3, PR4;
        HandStatus Status = HandStatus.Hided;
        int top = -50;
        int[] AvailableImages;
        SortedList<int, double[]> AvailableWidths = GetWidths();
        public ShipCreateNew(Schema schema)
        {
            Schema = schema; 
            switch(schema.ShipType.Type)
            {
                case ShipGenerator2Types.Scout: AvailableImages = new int[] { 1, 2 };  break;
                case ShipGenerator2Types.Linkor:
                case ShipGenerator2Types.Cruiser:
                case ShipGenerator2Types.Cargo: AvailableImages = new int[] { 6, 7 }; break;
                case ShipGenerator2Types.Frigate:
                case ShipGenerator2Types.Corvett: AvailableImages = new int[] { 10, 11 }; break;
                case ShipGenerator2Types.Devostator: AvailableImages = new int[] { 3 };break;
                case ShipGenerator2Types.Fighter: AvailableImages = new int[] { 4, 5 }; break;
                case ShipGenerator2Types.Dreadnought:
                case ShipGenerator2Types.Warrior: AvailableImages = new int[] { 8, 9 }; break;
            }
            CurCanvas = this; CurBase = (byte)AvailableImages[0]; CurModel = BitConverter.ToInt32(new byte[] { CurBase, 0, 0, 0 }, 0);
            Width = 1300; Height = 600; Background = Brushes.Black;
            ImageBrush[] Part1 = new ImageBrush[10];
            for (int i = 10; i < 20; i++)
                Part1[i - 10] = GetBrush(AvailableImages[0], i);
            l1 = new MovingLine(4, 120, Part1, AvailableWidths[AvailableImages[0]][0], new int[] { 0, -3 });
            Children.Add(l1);
            Canvas.SetLeft(l1, 50); Canvas.SetTop(l1, 50);
            ImageBrush[] Part2 = new ImageBrush[10];
            for (int i = 20; i < 30; i++)
                Part2[i-20] = GetBrush(AvailableImages[0], i);
            //for (int i = 0; i < 10; i++)
            //    Part2[i] = Links.Brushes.Ships.Model6Brushes[string.Format("S62{0}", i.ToString())];
            l2 = new MovingLine(4, 120, Part2, AvailableWidths[AvailableImages[0]][1], new int[] { -1, 4 });
            Children.Add(l2);
            Canvas.SetLeft(l2, 50); Canvas.SetTop(l2, 180);
            ImageBrush[] Part3 = new ImageBrush[10];
            for (int i = 30; i < 40; i++)
                Part3[i-30] = GetBrush(AvailableImages[0], i);
            //for (int i = 0; i < 10; i++)
            //    Part3[i] = Links.Brushes.Ships.Model6Brushes[string.Format("S63{0}", i.ToString())];
            l3 = new MovingLine(4, 120, Part3, AvailableWidths[AvailableImages[0]][2], new int[] { 2 });
            Children.Add(l3);
            Canvas.SetLeft(l3, 50); Canvas.SetTop(l3, 310);
            AddRectangle(500, 700, 50+top, Links.Brushes.Interface.ShipCreateR1);
            Rotates[0] = AddRectangle(400, 750, 98+top, Links.Brushes.Interface.ShipCreateR2);
            Rotates[1] = AddRectangle(500, 700, 49+top, Links.Brushes.Interface.ShipCreateR3);
            Rotates[2] = AddRectangle(400, 750, 99+top, Links.Brushes.Interface.ShipCreateR4);
            Rotates[3] = AddRectangle(400, 750, 99+top, Links.Brushes.Interface.ShipCreateR5);
            Rotates[4] = AddRectangle(275, 813, 163+top, Links.Brushes.Interface.ShipCreateR6);

            Viewbox vbx = new Viewbox(); vbx.Width = 300; vbx.Height = 300;
            Children.Add(vbx); Canvas.SetLeft(vbx, 850); Canvas.SetTop(vbx, 210+top);
            ShipModel = new Canvas();
            vbx.Child = ShipModel; ShipModel.Width = 450; ShipModel.Height = 450;
            //Children.Add(ShipModel);
            //Canvas.SetLeft(ShipModel, 750);
            //Canvas.SetTop(ShipModel, 150);
            if (CurBase != 0)
                ChangeElements();
            ChangeShip(false, 0);
            timer = new DispatcherTimer(); timer.Interval = TimeSpan.FromSeconds(0.2); timer.Tick += Timer_Tick;

            for (int i=0;i<AvailableImages.Length;i++)
                ShipBorders.Add(GetShipModelBorder((byte)AvailableImages[i], 110+125*i, 440));

            InterfaceButton CancelButton = new InterfaceButton(150, 50, 7, 24);
            CancelButton.SetText(Links.Interface("Cancel"));
            CancelButton.PutToCanvas(this, 1000, 580+top);
            CancelButton.PreviewMouseDown += CancelButton_Click;

            InterfaceButton OkButton = new InterfaceButton(150, 50, 7, 24);
            OkButton.SetText(Links.Interface("Ok"));
            OkButton.PutToCanvas(this, 800, 580+top);
            OkButton.PreviewMouseDown += OkButton_Click;
            
            P1 = new Rectangle(); P1.Width = 250; P1.Height = 250; P1.Fill = Links.Brushes.Interface.ShipCreateP1;
            Children.Add(P1); Canvas.SetLeft(P1, 820); Canvas.SetTop(P1, 390+top);
            PR1 = new RotateTransform(0, 130, -90); P1.RenderTransform = PR1;
            P2 = new Rectangle(); P2.Width = 100; P2.Height = 167; P2.Fill = Links.Brushes.Interface.ShipCreateP2;
            Children.Add(P2); Canvas.SetLeft(P2, 874); Canvas.SetTop(P2, 254+top); P2.RenderTransformOrigin = new Point(0.655, 0.815);
            PR2 = new RotateTransform(0); P2.RenderTransform = PR2;
            P51 = Common.GetRectangle(45, Links.Brushes.Interface.ShipCreateP5);
            Children.Add(P51); Canvas.SetLeft(P51, 918); Canvas.SetTop(P51, 368+top);
            P3 = new Rectangle(); P3.Width = 67; P3.Height = 67; P3.Fill = Links.Brushes.Interface.ShipCreateP3;
            Children.Add(P3); Canvas.SetLeft(P3, 875); Canvas.SetTop(P3, 222+top); P3.RenderTransformOrigin = new Point(0.62, 0.92);
            PR3 = new RotateTransform(0); P3.RenderTransform = PR3;
            P52 = Common.GetRectangle(45, Links.Brushes.Interface.ShipCreateP5);
            Children.Add(P52); Canvas.SetLeft(P52, 895); Canvas.SetTop(P52, 262+top);
            P4 = new Rectangle(); P4.Width = 33;P4.Height = 33; P4.Fill = Links.Brushes.Interface.ShipCreateP4;
            Children.Add(P4); Canvas.SetLeft(P4, 903); Canvas.SetTop(P4, 216+top); P4.RenderTransformOrigin = new Point(0.02, 0.35);
            PR4 = new RotateTransform(-60); P4.RenderTransform = PR4;
            P53 = Common.GetRectangle(24, Links.Brushes.Interface.ShipCreateP5);
            Children.Add(P53); Canvas.SetLeft(P53, 892); Canvas.SetTop(P53, 216+top);
            time = TimeSpan.FromSeconds(0);
            SetEllipseAngle(0, 180, -10, -130);
            time = TimeSpan.FromSeconds(0.2);
        }
        ImageBrush GetBrush(int pos, int i)
        {
            switch (pos)
            {
                case 1: return Links.Brushes.Ships.ModelNew1Brushes[string.Format("S{0}", i.ToString())];
                case 2: return Links.Brushes.Ships.ModelNew2Brushes[string.Format("S{0}", i.ToString())];
                case 3: return Links.Brushes.Ships.ModelNew3Brushes[string.Format("S{0}", i.ToString())];
                case 4: return Links.Brushes.Ships.ModelNew4Brushes[string.Format("S{0}", (i/10*10).ToString())];
                case 5: return Links.Brushes.Ships.ModelNew5Brushes[string.Format("S{0}", (i / 10 * 10).ToString())];
                case 6: return Links.Brushes.Ships.ModelNew6Brushes[string.Format("S{0}", (i / 10 * 10).ToString())];
                case 7: return Links.Brushes.Ships.ModelNew7Brushes[string.Format("S{0}", (i / 10 * 10).ToString())];
                case 8: return Links.Brushes.Ships.ModelNew8Brushes[string.Format("S{0}", i.ToString())];
                case 9: return Links.Brushes.Ships.ModelNew9Brushes[string.Format("S{0}", i.ToString())];
                case 10: return Links.Brushes.Ships.ModelNew10Brushes[string.Format("S{0}", (i / 10 * 10).ToString())];
                case 11: return Links.Brushes.Ships.ModelNew11Brushes[string.Format("S{0}", (i / 10 * 10).ToString())];
                default: return null;
            }
        }
        static SortedList<int, double[]> GetWidths()
        {
            SortedList<int, double[]> result = new SortedList<int, double[]>();
            result.Add(1, new double[] { 1.22, 2.48, 1.76 });
            result.Add(2, new double[] { 1, 1, 1 });
            result.Add(3, new double[] { 1.5, 1.5, 1.33 });
            result.Add(4, new double[] { 1, 1, 1 });
            result.Add(5, new double[] { 1, 1, 1 });
            result.Add(6, new double[] { 1, 1, 1 });
            result.Add(7, new double[] { 1, 1, 1 });
            result.Add(8, new double[] { 1, 1, 1 });
            result.Add(9, new double[] { 1, 1, 1 });
            result.Add(10, new double[] { 1, 1, 1 });
            result.Add(11, new double[] { 1, 1, 1 });
            return result;
        }
        RotateTransform AddRectangle(int size, int left, int top, Brush brush)
        {
            Rectangle rect = Common.GetRectangle(size, brush);
            Children.Add(rect); Canvas.SetLeft(rect, left); Canvas.SetTop(rect, top);
            rect.RenderTransformOrigin = new Point(0.5, 0.5);
            RotateTransform rotate = new RotateTransform(0);
            rect.RenderTransform = rotate;
            return rotate;
        }
        public void RotateRounds(int round, int pos, int time, double acc, double dec)
        {
            DoubleAnimation anim = new DoubleAnimation(pos * 36, TimeSpan.FromMilliseconds(time));
            anim.AccelerationRatio = acc; anim.DecelerationRatio = dec;
            Rotates[round].BeginAnimation(RotateTransform.AngleProperty, anim);
        }
        void OkButton_Click(object sender, RoutedEventArgs e)
        {
            string eventresult = Events.BuildNewShip(Schema, CurModel);
            if (eventresult != "")
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult), true);
                return;
            }
            Gets.GetTotalInfo("После откртия панели создания кораблей");
            Gets.GetResources();
            //Gets.GetShips();
            Links.Controller.NewSchemasCanvas.Refresh();
        }

        void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Links.Controller.NewSchemasCanvas.Refresh();
        }

        Border GetShipModelBorder(byte model, int left, int top)
        {
            Border border = new Border();
            border.Width = 120; border.Height = 120;
            border.Background = Links.Brushes.Interface.ShipCreateBorder;
            ShipB ship = new ShipB(0, Schema, 100, null, new byte[] { model, 0, 0, 0 }, ShipSide.Attack, null, false, null, ShipBMode.Info,255);
            Viewbox vbx = new Viewbox(); vbx.Width = 80; vbx.Height = 80;
            border.Child = vbx;
            vbx.Child = ship.HexShip;
            ship.HexShip.Children.Remove(ship.HexShip.Health);
            ship.HexShip.Children.Remove(ship.HexShip.Shield);
            ship.HexShip.Children.Remove(ship.HexShip.Energy);
            ship.HexShip.Children.Remove(ship.HexShip.ShieldField.Image);
            border.Tag = model;
            border.BorderThickness = new Thickness(2);
            if (CurBase == model)
                border.BorderBrush = Brushes.Red;
            Children.Add(border);
            Canvas.SetLeft(border, left); Canvas.SetTop(border, top);
            border.PreviewMouseDown += ShipModelChange_PreviewMouseDown;
            return border;
        }

        private void ShipModelChange_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Border border = (Border)sender;
            byte tag = (byte)border.Tag;
            if (CurBase == tag) return;
            foreach (Border bord in ShipBorders)
                bord.BorderBrush = Brushes.Transparent;
            CurBase = tag;
            border.BorderBrush = Brushes.Red;
            ChangeElements();
        }

        public void ChangeElements()
        {
            ImageBrush[] Part1, Part2, Part3;

            Part1 = new ImageBrush[10];
            for (int i = 10; i < 20; i++)
                Part1[i - 10] = GetBrush(CurBase, i);
                    l1.ChangeElements(Part1, AvailableWidths[CurBase][0]);
            Part2 = new ImageBrush[10];
            for (int i = 20; i < 30; i++)
                Part2[i - 20] = GetBrush(CurBase, i);
            l2.ChangeElements(Part2, AvailableWidths[CurBase][1]);
            Part3 = new ImageBrush[10];
            for (int i = 30; i < 40; i++)
                Part3[i - 30] = GetBrush(CurBase, i);
            l3.ChangeElements(Part3, AvailableWidths[CurBase][2]);
            ChangeShip(true, 1);
        }
        int a1 = 0; int a2 = -10; int a3 = -10; int a4 = -130;
        int dx2 = 1; int dx3 = 1; int dx4 = 1;
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Status == HandStatus.Hided)
            {

                sparks = 10;
                SetEllipseAngle(a1, (int)(a2 + 180.0 - 18.0 * (10 - sparks)), a3, a4);
                Status = HandStatus.Showing;
            }
            else if (Status == HandStatus.Showing)
            {
                if (sparks > 0)
                {
                    sparks--;
                    SetEllipseAngle(a1, (int)(a2 + 180.0 - 18.0 * (10 - sparks)), a3, a4);
                }
                else
                {
                    sparks = rnd.Next(10, 15);
                    Status = HandStatus.Work;
                    return;
                }
            }
            else if (Status == HandStatus.Work)
            {
                if (sparks > 0)
                {
                    sparks--;
                    a1 += 5;
                    a2 += 10 * dx2;
                    if ((a2 % 360) <= (-10 + a1 % 360)) dx2 = 1; else if ((a2 % 360) >= (50 + a1 % 360)) dx2 = -1;
                    a3 += 15 * dx3;
                    if ((a3 % 360) <= (-10 + a2 % 360)) dx3 = 1; else if ((a3 % 360) >= (50 + a2 % 360)) dx3 = -1;
                    a4 += 30 * dx4;
                    if (a4 % 360 > -60 + a3 % 360) dx4 = -1; else if (a4 % 360 < -120 + a3 % 360) dx4 = 1;
                    Point pt = SetEllipseAngle(a1, a2, a3, a4);
                    Spark spark = new Spark(pt, this, 0.2);
                }
                else
                {
                    Status = HandStatus.Hiding;
                    sparks = 10;
                    SetEllipseAngle(a1, (int)(a2 + 180.0 - 18.0 * sparks), a3, a4);
                }
            }
            else
            {
                if (sparks>0)
                {
                    sparks--;
                    SetEllipseAngle(a1, (int)(a2 + 180.0 - 18.0 * sparks), a3, a4);
                }
                else
                {
                    Status = HandStatus.Hided;
                    timer.Stop();
                }
            }
            
        }
        TimeSpan time = TimeSpan.FromSeconds(0.2);
        public Point SetEllipseAngle(int angle, int angle2, int angle3, int angle4)
        {
            DoubleAnimation anim1 = new DoubleAnimation(angle, time);
            PR1.BeginAnimation(RotateTransform.AngleProperty, anim1);
            int innerangle = (angle + 180) % 360;
            int LY1 = 90; int LX1 = 10;
            double X = 950 + LY1 * Math.Sin(innerangle / 180.0 * Math.PI) + LX1 * Math.Cos(innerangle / 180.0 * Math.PI);
            double Y = 300 - LY1 * Math.Cos(innerangle / 180.0 * Math.PI) + LX1 * Math.Sin(innerangle / 180.0 * Math.PI)+top;
            
            DoubleAnimation anim2 = new DoubleAnimation(X - 22.5, time);
            P51.BeginAnimation(Canvas.LeftProperty, anim2);
            DoubleAnimation anim3 = new DoubleAnimation(Y - 22.5, time);
            P51.BeginAnimation(Canvas.TopProperty, anim3);
            
            DoubleAnimation anim4 = new DoubleAnimation(X - 66, time);
            P2.BeginAnimation(Canvas.LeftProperty, anim4);
            DoubleAnimation anim5 = new DoubleAnimation(Y - 136, time);
            P2.BeginAnimation(Canvas.TopProperty, anim5);
            DoubleAnimation anim6 = new DoubleAnimation(angle2, time);
            PR2.BeginAnimation(RotateTransform.AngleProperty, anim6);
            
            int LX2 = -23; int LY2 = 106;
            double X2 = X + LY2 * Math.Sin(angle2 / 180.0 * Math.PI) + LX2 * Math.Cos(angle2 / 180.0 * Math.PI);
            double Y2 = Y - LY2 * Math.Cos(angle2 / 180.0 * Math.PI) + LX2 * Math.Sin(angle2 / 180.0 * Math.PI);
            
            DoubleAnimation anim7 = new DoubleAnimation(X2 - 22.5, time);
            P52.BeginAnimation(Canvas.LeftProperty, anim7);
            DoubleAnimation anim8 = new DoubleAnimation(Y2 - 22.5, time);
            P52.BeginAnimation(Canvas.TopProperty, anim8);
            
            DoubleAnimation anim9 = new DoubleAnimation(X2 - 42, time);
            P3.BeginAnimation(Canvas.LeftProperty, anim9);
            DoubleAnimation anim10 = new DoubleAnimation(Y2 - 62, time);
            P3.BeginAnimation(Canvas.TopProperty, anim10);
            DoubleAnimation anim11 = new DoubleAnimation(angle3, time);
            PR3.BeginAnimation(RotateTransform.AngleProperty, anim11);
            
            int LX3 = -13; int LY3 = 56;
            double X3 = X2 + LY3 * Math.Sin(angle3 / 180.0 * Math.PI) + LX3 * Math.Cos(angle3 / 180.0 * Math.PI);
            double Y3 = Y2 - LY3 * Math.Cos(angle3 / 180.0 * Math.PI) + LX3 * Math.Sin(angle3 / 180.0 * Math.PI);
            
            DoubleAnimation anim12 = new DoubleAnimation(X3 - 12, time);
            P53.BeginAnimation(Canvas.LeftProperty, anim12);
            DoubleAnimation anim13 = new DoubleAnimation(Y3 - 12, time);
            P53.BeginAnimation(Canvas.TopProperty, anim13);
            
            DoubleAnimation anim14 = new DoubleAnimation(X3 - 1, time);
            P4.BeginAnimation(Canvas.LeftProperty, anim14);
            P4.BeginAnimation(Canvas.TopProperty, anim13);
            DoubleAnimation anim15 = new DoubleAnimation(angle4, time);
            PR4.BeginAnimation(RotateTransform.AngleProperty, anim15);
            
            int LX4 = 35; int LY4 = -15;
            double X4 = X3 + LY4 * Math.Sin(angle4 / 180.0 * Math.PI) + LX4 * Math.Cos(angle4 / 180.0 * Math.PI);
            double Y4 = Y3 - LY4 * Math.Cos(angle4 / 180.0 * Math.PI) + LX4 * Math.Sin(angle4 / 180.0 * Math.PI);
            return new Point(X4, Y4);
        }
        public void ChangeShip(bool needcheck, int reason)
        {
            int newmodel = BitConverter.ToInt32(new byte[] { CurBase, (byte)l1.SelectPos, (byte)l2.SelectPos, (byte)l3.SelectPos }, 0);
            if (needcheck)
            {
                if (newmodel == CurModel)
                    return;
                if (IsChanging)
                {
                    return;
                }
            }
            IsChanging = true;
            CurModel = newmodel;
            if (LastShip != null)
            {
                DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(2.0));
                anim.Completed += LastShipHide_Completed;
                LastShip.BeginAnimation(Canvas.OpacityProperty, anim);
            }
            ShipB ship = new ShipB(0, Schema, 100, null, BitConverter.GetBytes(CurModel), ShipSide.Attack, null, false, null, ShipBMode.Info,255);
            NewShip = ship.HexShip;
            ship.HexShip.Opacity = 0;
            ShipModel.Children.Add(ship.HexShip);
            ship.HexShip.Children.Remove(ship.HexShip.Health);
            ship.HexShip.Children.Remove(ship.HexShip.Shield);
            ship.HexShip.Children.Remove(ship.HexShip.Energy);
            DoubleAnimation anim1 = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(3));
            anim1.Completed += NewShipShow_Completed;
            NewShip.BeginAnimation(Canvas.OpacityProperty, anim1);
            if (Status == HandStatus.Work)
                sparks += 10;
            else if (Status == HandStatus.Hiding) { Status = HandStatus.Showing; sparks = 10 - sparks; }
            else { if (P1 == null) return; Timer_Tick(null, null); timer.Start(); }
            //sparks = rnd.Next(10, 20);
            //timer.Start();
            GC.Collect();
        }
        
        private void NewShipShow_Completed(object sender, EventArgs e)
        {
            LastShip = NewShip;
            IsChanging = false;
            ChangeShip(true, 2);
        }

        private void LastShipHide_Completed(object sender, EventArgs e)
        {
            ShipModel.Children.Remove(LastShip);
        }
    }
    public class MovingLine : Canvas
    {
        Canvas InnerCanvas;
        Canvas DownCanvas;
        int curpos = 0;
        bool RightPressed = false;
        bool LeftPressed = false;
        Brush[] Elements;
        double ElSoot;
        int ElementWidth;
        int Count;
        SortedList<int, Border> Rects = new SortedList<int, Border>();
        static TimeSpan LongTime = TimeSpan.FromSeconds(0.5);
        static TimeSpan MiddleTime = TimeSpan.FromSeconds(0.35);
        static TimeSpan ShortTime = TimeSpan.FromSeconds(0.2);
        bool IsMoving = false;
        int steps = 0;
        public int SelectPos = 0;
        int[] Rounds;
        public MovingLine(int count, int elementwidth, Brush[] elements, double elSoot, int[] rounds)
        {
            Width = elementwidth * count + (elementwidth * (count - 1) * 0.05) + elementwidth;
            Height = elementwidth;
            Elements = elements;
            ElSoot = elSoot;
            ElementWidth = elementwidth;
            Count = count;
            Rounds = rounds;
            InnerCanvas = new Canvas(); InnerCanvas.Height = Height;
            InnerCanvas.Width = Width - elementwidth;
            InnerCanvas.ClipToBounds = true;
            Children.Add(InnerCanvas); Canvas.SetLeft(InnerCanvas, elementwidth / 2.0);
            Rectangle LeftArrow = new Rectangle(); LeftArrow.Width = elementwidth / 2; LeftArrow.Height = elementwidth;
            LeftArrow.Fill = Links.Brushes.Interface.ShipCreateLArrow; Children.Add(LeftArrow);
            Rectangle RightArrow = new Rectangle(); RightArrow.Width = elementwidth / 2; RightArrow.Height = elementwidth;
            RightArrow.Fill = Links.Brushes.Interface.ShipCreareRArrow; Children.Add(RightArrow); Canvas.SetLeft(RightArrow, Width - elementwidth / 2.0);
            DownCanvas = new Canvas();
            InnerCanvas.Children.Add(DownCanvas); Canvas.SetLeft(DownCanvas, 0);
            for (int i = -1; i <= count; i++)
                AddElement(i);
            RightArrow.PreviewMouseDown += LeftArrow_PreviewMouseDown;
            RightArrow.PreviewMouseUp += LeftArrow_PreviewMouseUp;
            LeftArrow.PreviewMouseDown += RightArrow_PreviewMouseDown;
            LeftArrow.PreviewMouseUp += RightArrow_PreviewMouseUp;
            this.PreviewMouseWheel += MovingLine_PreviewMouseWheel;
        }
        public void ChangeElements(Brush[] elements, double elSoot)
        {
            Elements = elements;
            ElSoot = elSoot;
            foreach (Border border in Rects.Values)
            {
                int tag = (int)border.Tag;
                Rectangle rect = new Rectangle();
                rect.Fill = Elements[tag];
                double Sootn = ElSoot;
                if (Sootn > 1)
                {
                    rect.Width = ElementWidth * 0.8;
                    rect.Height = ElementWidth * 0.8 / ElSoot;
                }
                else
                {
                    rect.Width = ElementWidth * 0.8 * ElSoot;
                    rect.Height = ElementWidth * 0.8;
                }
                border.Child = rect;   
            }
        }
        private void MovingLine_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            if (e.Delta > 0)
            {
                RightArrow_PreviewMouseDown(null, null);
                RightArrow_PreviewMouseUp(null, null);
                steps = 3;
            }
            else
            {
                LeftArrow_PreviewMouseDown(null, null);
                LeftArrow_PreviewMouseUp(null, null);
                steps = -3;
            }
        }

        private void LeftArrow_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            LeftPressed = false;
        }

        private void LeftArrow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e != null) e.Handled = true;
            LeftPressed = true;
            if (IsMoving) return;
            IsMoving = true;
            curpos--;
            DoubleAnimation anim = new DoubleAnimation(-(curpos) * ElementWidth * 1.05, LongTime);
            anim.AccelerationRatio = 0.5; anim.DecelerationRatio = 0.5;
            anim.Completed += Anim_LeftCompletedFirst;
            DownCanvas.BeginAnimation(Canvas.LeftProperty, anim);
            foreach (int val in Rounds)
            {
                int round, pos;
                if (val>=0) { round = val; pos = curpos; } else { round = -val; pos = -curpos; }
                ShipCreateNew.CurCanvas.RotateRounds(round, pos, (int)LongTime.TotalMilliseconds, 0.5, 0.5);
            }
        }

        private void Anim_LeftCompletedFirst(object sender, EventArgs e)
        {
            int maxkey = Rects.Keys.Max();
            DownCanvas.Children.Remove(Rects[maxkey]);
            Rects.Remove(maxkey);
            AddElement(curpos - 1);

            if (LeftPressed || steps < 0)
            {
                steps++;
                curpos--;
                DoubleAnimation anim;
                anim = new DoubleAnimation(-curpos * ElementWidth * 1.05, MiddleTime); anim.AccelerationRatio = 0.5;
                anim.Completed += Anim_LeftCompletedNext;
                DownCanvas.BeginAnimation(Canvas.LeftProperty, anim);
                foreach (int val in Rounds)
                {
                    int round, pos;
                    if (val >= 0) { round = val; pos = curpos; } else { round = -val; pos = -curpos; }
                    ShipCreateNew.CurCanvas.RotateRounds(round, pos, (int)MiddleTime.TotalMilliseconds, 0.5, 0);
                }
            }
            else
            { IsMoving = false; steps = 0; }
        }

        private void Anim_LeftCompletedNext(object sender, EventArgs e)
        {
            int maxkey = Rects.Keys.Max();
            DownCanvas.Children.Remove(Rects[maxkey]);
            Rects.Remove(maxkey);
            AddElement(curpos - 1);

            if (LeftPressed || steps < 0)
            {
                steps++;
                curpos--;
                DoubleAnimation anim;
                anim = new DoubleAnimation(-curpos * ElementWidth * 1.05, ShortTime);
                anim.Completed += Anim_LeftCompletedNext;
                DownCanvas.BeginAnimation(Canvas.LeftProperty, anim);
                foreach (int val in Rounds)
                {
                    int round, pos;
                    if (val >= 0) { round = val; pos = curpos; } else { round = -val; pos = -curpos; }
                    ShipCreateNew.CurCanvas.RotateRounds(round, pos, (int)ShortTime.TotalMilliseconds, 0, 0);
                }
            }
            else
            { IsMoving = false; steps = 0; }
        }

        private void RightArrow_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            RightPressed = false;
        }

        private void RightArrow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e != null) e.Handled = true;
            RightPressed = true;
            if (IsMoving) return;
            IsMoving = true;
            curpos++;
            DoubleAnimation anim = new DoubleAnimation(-(curpos) * ElementWidth * 1.05, LongTime);
            anim.AccelerationRatio = 0.5; anim.DecelerationRatio = 0.5;
            anim.Completed += Anim_RightCompletedFirst;
            DownCanvas.BeginAnimation(Canvas.LeftProperty, anim);
            foreach (int val in Rounds)
            {
                int round, pos;
                if (val >= 0) { round = val; pos = curpos; } else { round = -val; pos = -curpos; }
                ShipCreateNew.CurCanvas.RotateRounds(round, pos, (int)LongTime.TotalMilliseconds, 0.5, 0.5);
            }
        }

        private void Anim_RightCompletedFirst(object sender, EventArgs e)
        {
            int minkey = Rects.Keys.Min();
            DownCanvas.Children.Remove(Rects[minkey]);
            Rects.Remove(minkey);
            //if (Rects.ContainsKey(curpos - 2))
            /*{
                DownCanvas.Children.Remove(Rects[curpos - 2]);
                Rects.Remove(curpos - 2);
            }*/
            AddElement(curpos + Count);

            if (RightPressed || steps > 0)
            {
                steps--;
                curpos++;
                DoubleAnimation anim;
                anim = new DoubleAnimation(-curpos * ElementWidth * 1.05, MiddleTime); anim.AccelerationRatio = 0.5;
                anim.Completed += Anim_RightCompletedNext;
                DownCanvas.BeginAnimation(Canvas.LeftProperty, anim);
                foreach (int val in Rounds)
                {
                    int round, pos;
                    if (val >= 0) { round = val; pos = curpos; } else { round = -val; pos = -curpos; }
                    ShipCreateNew.CurCanvas.RotateRounds(round, pos, (int)MiddleTime.TotalMilliseconds, 0.5, 0);
                }
            }
            else
            { IsMoving = false; steps = 0; }

        }

        private void Anim_RightCompletedNext(object sender, EventArgs e)
        {
            int minkey = Rects.Keys.Min();
            DownCanvas.Children.Remove(Rects[minkey]);
            Rects.Remove(minkey);
            //if (Rects.ContainsKey(curpos - 2))
            /*{
                DownCanvas.Children.Remove(Rects[curpos - 2]);
                Rects.Remove(curpos - 2);
            }*/
            AddElement(curpos + Count);

            if (RightPressed || steps > 0)
            {
                steps--;
                curpos++;
                DoubleAnimation anim;
                anim = new DoubleAnimation(-curpos * ElementWidth * 1.05, ShortTime);
                anim.Completed += Anim_RightCompletedNext;
                DownCanvas.BeginAnimation(Canvas.LeftProperty, anim);
                foreach (int val in Rounds)
                {
                    int round, pos;
                    if (val >= 0) { round = val; pos = curpos; } else { round = -val; pos = -curpos; }
                    ShipCreateNew.CurCanvas.RotateRounds(round, pos, (int)ShortTime.TotalMilliseconds, 0, 0);
                }
            }
            else
            { IsMoving = false; steps = 0; }
        }
        void AddElement(int pos)
        {
            int k = pos;
            if (k < 0)
            {
                int b = -k / Elements.Length + 1;
                k = b * Elements.Length + k;
            }
            Border border = new Border(); border.Width = ElementWidth; border.Height = ElementWidth;
            border.Background = Links.Brushes.Interface.ShipCreateBorder;
            border.BorderThickness = new Thickness(2);
            if (k%Elements.Length == SelectPos)
                border.BorderBrush = Brushes.Red;
            Rectangle rect = new Rectangle();
            rect.Fill = Elements[k % Elements.Length];
            double Sootn = ElSoot;
            if (Sootn > 1)
            {
                rect.Width = ElementWidth * 0.8;
                rect.Height = ElementWidth * 0.8 / ElSoot;
            }
            else
            {
                rect.Width = ElementWidth * 0.8 * ElSoot;
                rect.Height = ElementWidth * 0.8;
            }
            border.Child = rect;
            DownCanvas.Children.Add(border);
            Canvas.SetLeft(border, pos * ElementWidth * 1.05);
            if (Rects.ContainsKey(pos))
                Rects.Remove(pos);
            Rects.Add(pos, border);
            border.Tag = k % Elements.Length;
            border.PreviewMouseDown += Border_PreviewMouseDown;
        }

        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Border border = (Border)sender;
            int pos = (int)border.Tag;
            if (SelectPos == pos) return;
            foreach (KeyValuePair<int, Border> pair in Rects)
            {
                int k = pair.Key;
                if (k < 0)
                {
                    int b = -k / Elements.Length + 1;
                    k = b * Elements.Length + k;
                }
                if (k%Elements.Length==SelectPos)
                {
                    pair.Value.BorderBrush = Brushes.Transparent;
                    break;
                }
            }
            border.BorderBrush = Brushes.Red;
            SelectPos = pos;
            ShipCreateNew.CurCanvas.ChangeShip(true, 4);
        }
    }
}
