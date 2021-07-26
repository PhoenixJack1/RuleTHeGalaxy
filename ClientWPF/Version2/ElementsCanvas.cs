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
    public class ElementsCanvas : Canvas
    {
        RoundCanvas Round;
        Canvas UpPart;
        Rectangle LowerRectangle;
        Rectangle UpperRectangle;
        BackEllipse El1, El2;
        // Rectangle Hand1, Hand2, Hand3, Hand4;
        Canvas LeftBorder, RightBorder;
        Ellipse Glass;
        public bool IsRightBorder = false;
        Viewbox ScienceVBX;
        public ElementsCanvas()
        {
            Width = 1920; Height = 1080;
            Round = new RoundCanvas();
            UpPart = new Canvas();
            Children.Add(UpPart);
            Rectangle wall = GetRectangle(1920, 628, 0, 0, Links.Brushes.Science.WallUp2, 5, 1);
            UpPart.Children.Add(wall);
            UpperRectangle = Round.GetUpperRectangle();
            UpPart.Children.Add(UpperRectangle);
            Canvas.SetZIndex(UpperRectangle, 10);

            Glass = new Ellipse(); Glass.Width = 500; Glass.Height = 500; Canvas.SetLeft(Glass, 708); Canvas.SetTop(Glass, 284);
            Glass.Fill = Links.Brushes.Science.Glass; Canvas.SetZIndex(Glass, 250);
            //Glass = Science2Canvas.GetRectangle(600, 600, 652, 234, Links.Brushes.Science.Glass, 250, 1);
            //Glass = Science2Canvas.GetRectangle(600, 600, 652, 234, Brushes.Red, 250, 1);

            UpPart.Children.Add(Glass);

            LowerRectangle = Round.GetLowerRectangle();
            Children.Add(LowerRectangle);
            Canvas.SetZIndex(LowerRectangle, 20);

            El1 = new BackEllipse();
            UpPart.Children.Add(El1); 
            El2 = new BackEllipse();
            UpPart.Children.Add(El2);
           // Hand1 = GetRectangle(500, 500, 710, 279, Links.Brushes.Science.Hand1, 0, 0);
           // UpPart.Children.Add(Hand1);
            //Hand2 = GetRectangle(500, 500, 710, 279, Links.Brushes.Science.Hand2, 0, 0);
            //UpPart.Children.Add(Hand2);
            //Hand3 = GetRectangle(500, 500, 710, 279, Links.Brushes.Science.Hand3, 0, 0);
            //UpPart.Children.Add(Hand3);
            //Hand4 = GetRectangle(500, 500, 710, 279, Links.Brushes.Science.Hand4, 0, 0);
            //UpPart.Children.Add(Hand4);

            LeftBorder = GetCanvas(615, 850, 0, -850, Links.Brushes.Science.Border, 20, 1);
            Children.Add(LeftBorder);

            RightBorder = GetCanvas(615, 850, 1300, -850, Links.Brushes.Science.Border, 20, 1);
            Children.Add(RightBorder);

            ScienceVBX = new Viewbox();
            ScienceVBX.Width = 580; ScienceVBX.Height = 650;
            Children.Add(ScienceVBX);
            Canvas.SetLeft(ScienceVBX, 1318);
            Canvas.SetTop(ScienceVBX, 200);
            Canvas.SetZIndex(ScienceVBX, 25);
        }
        public static Canvas GetCanvas(int width, int height, int left, int top, Brush Fill, int z, double opacity)
        {
            Canvas rect = new Canvas();
            rect.Width = width; rect.Height = height;
            Canvas.SetLeft(rect, left); Canvas.SetTop(rect, top);
            rect.Background = Fill;
            Canvas.SetZIndex(rect, z);
            rect.Opacity = opacity;
            return rect;
        }
        public static Rectangle GetRectangle(int width, int height, int left, int top, Brush Fill, int z, double opacity)
        {
            Rectangle rect = new Rectangle();
            rect.Width = width; rect.Height = height;
            Canvas.SetLeft(rect, left); Canvas.SetTop(rect, top);
            rect.Fill = Fill;
            Canvas.SetZIndex(rect, z);
            rect.Opacity = opacity;
            return rect;
        }
        class BackEllipse:Canvas
        {
            Ellipse El;
            Rectangle Rect;
            public BackEllipse()
            {
                Width = 500; Height = 500; Canvas.SetLeft(this, 710); Canvas.SetTop(this,279);
                El = new Ellipse(); El.Width = 500; El.Height = 500; Children.Add(El);
                Rect = new Rectangle(); Rect.Width = 300; Rect.Height = 300;
                Children.Add(Rect); Canvas.SetLeft(Rect, 100); Canvas.SetTop(Rect, 100);
            }
            public void SetBack(Brush brush)
            {
                El.Fill = brush;
            }
            public void SetForward(Brush brush)
            {
                Rect.Fill = brush;
            }
        }
        GameScience Science;
        public void Play(ushort scienceid)
        {
            Science = Links.Science.GameSciences[scienceid];
            new MySound("Interface/Menu_science_open.wav");
            DoubleAnimation anim = new DoubleAnimation(-700, 0, TimeSpan.FromSeconds(0.5));
            UpPart.BeginAnimation(Canvas.TopProperty, anim);

            DoubleAnimation anim1 = new DoubleAnimation(1080, 228, TimeSpan.FromSeconds(0.5));
            anim1.Completed += Anim1_Completed;
            LowerRectangle.BeginAnimation(Canvas.TopProperty, anim1);
        }

        private void Anim1_Completed(object sender, EventArgs e)
        {
            LowerRectangle.Opacity = 0;
            UpperRectangle.Opacity = 0;
            Children.Add(Round);
            DoubleAnimation hand1anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
            //hand1anim.Completed += Hand1anim_Completed;
            //Round.LeftButton.PreviewMouseDown += LeftButton_PreviewMouseDown;
            //Round.RightButton.PreviewMouseDown += RightButton_PreviewMouseDown;
            //Round.Regerator.PreviewMouseDown += Glass_PreviewMouseDown;
            Glass.PreviewMouseDown += Glass_PreviewMouseDown;
            UpPart.Children.Remove(Glass);
            Children.Add(Glass);
            El1.BeginAnimation(Rectangle.OpacityProperty, hand1anim);
            new Spark(new Point(685, 575), Links.Controller.Science3Canvas, 1);
            new Spark(new Point(1205, 610), Links.Controller.Science3Canvas, 1);
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.2);
            timer.Tick += Timer_Tick;
            step = 0;
            Random rnd = new Random();
            steps = new byte[10];
            rnd.NextBytes(steps);
            types = new ScienceType[10];
            for (int i = 0; i < 10; i++)
                types[i] = (ScienceType)(steps[i] % (int)ScienceObserver.MaxType);
            types[9] = Science.SType;
            El1.SetBack(new SolidColorBrush(BackRectangle.GetColor(types[0])));
            El1.SetForward(TypeElement.GetBrush(types[0]));
            timer.Start();
        }

        private void Glass_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            quick = true;
        }

        int step = 0;
        byte[] steps;
        ScienceType[] types;
        bool quick = false;
        private void Timer_Tick(object sender, EventArgs e)
        {
            System.Windows.Threading.DispatcherTimer timer = (System.Windows.Threading.DispatcherTimer)sender;
            if (steps[step] > 100)
                LeftButton_PreviewMouseDown(null, null);
            else
                RightButton_PreviewMouseDown(null, null);
            step++;
            TimeSpan nexttime;
            if (quick)
                nexttime = TimeSpan.FromSeconds(0.1);
            else
                nexttime = TimeSpan.FromSeconds(0.3 + 0.01 * Math.Pow(step, 1.6));
            timer.Interval = nexttime;
            time = nexttime;
            Random rnd = new Random();
           // for (int i=0;i<5;i++)
           //     new Spark(new Point(800+rnd.Next(0,300), 370+rnd.Next(0,300)), Links.Controller.Science3Canvas, 1);
            if (step == 9)
            {
                PutLeftBorder();
                PutRightBorder();
                timer.Stop();
            }
        }

  
        public void PutRightBorder()
        {
            DoubleAnimation leftborderanim = new DoubleAnimation(-850, 100, TimeSpan.FromSeconds(2));
            ElasticEase ease = new ElasticEase();
            leftborderanim.EasingFunction = ease;
            RightBorder.BeginAnimation(Canvas.TopProperty, leftborderanim);
            Viewbox Info = new Viewbox();
            Info.Width = 400;
            switch (Science.Type)
            {
                case Links.Science.EType.ShipTypes: Info.Child = new ShipTypeLargeInfo(Links.ShipTypes[Science.ID]); break;
                case Links.Science.EType.Weapon: Info.Child = new WeaponLargeInfo(Links.WeaponTypes[Science.ID]); break;
                case Links.Science.EType.Generator: Info.Child = new GeneratorLargeInfo(Links.GeneratorTypes[Science.ID]); break;
                case Links.Science.EType.Shield: Info.Child = new ShieldLargeInfo(Links.ShieldTypes[Science.ID]); break;
                case Links.Science.EType.Computer: Info.Child = new ComputerLargeInfo(Links.ComputerTypes[Science.ID]); break;
                case Links.Science.EType.Engine: Info.Child = new EngineLargeInfo(Links.EngineTypes[Science.ID]); break;
                case Links.Science.EType.Equipment: Info.Child = new EquipLargeInfo(Links.EquipmentTypes[Science.ID]); break;
                default: return;
            }
            RightBorder.Children.Add(Info);
            Canvas.SetLeft(Info, 100); Canvas.SetTop(Info, 130);
        }

        void PutLeftBorder()
        {
            DoubleAnimation leftborderanim = new DoubleAnimation(-850, 100, TimeSpan.FromSeconds(2));
            ElasticEase ease = new ElasticEase();
            leftborderanim.EasingFunction = ease;
            //leftborderanim.Completed += Leftborderanim_Completed1;
            LeftBorder.BeginAnimation(Canvas.TopProperty, leftborderanim);
            GameObjectImage image = new GameObjectImage(GOImageType.Standard, Science.ID);
            image.Width = 600; image.Height = 600;
            Canvas.SetLeft(image, 20); Canvas.SetTop(image, 120);
            LeftBorder.Children.Add(image);
        }
        /*private void Leftborderanim_Completed1(object sender, EventArgs e)
        {
            DoubleAnimation anim1 = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));


            Links.Controller.Science2Canvas.ScienceBorder = new ScienceListBorder();
            //Children.Add(Links.Controller.Science2Canvas.ScienceBorder);
            //Canvas.SetZIndex(Links.Controller.Science2Canvas.ScienceBorder, 100);
            //Canvas.SetLeft(Links.Controller.Science2Canvas.ScienceBorder, 60);
            //Canvas.SetTop(Links.Controller.Science2Canvas.ScienceBorder, 200);
            Links.Controller.Science2Canvas.ScienceBorder.Draw();
        }*/

        private void RightButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            new MySound("Interface/Menu_circle_clockwise.wav");
            int newpos = pos - 1;
            DoubleAnimation anim = new DoubleAnimation(pos * 90, newpos * 90, time);
            anim.AccelerationRatio = 0.3;
            anim.DecelerationRatio = 0.3;
            Round.Rotate_Button.BeginAnimation(RotateTransform.AngleProperty, anim);
            double curangle = Round.Rotate_STV2.Angle;
            DoubleAnimation stv_Anim = new DoubleAnimation(curangle + 90, time);
            stv_Anim.AccelerationRatio = 0.3;
            stv_Anim.DecelerationRatio = 0.3;
            Round.Rotate_STV2.BeginAnimation(RotateTransform.AngleProperty, stv_Anim);
            DoubleAnimation animimage1 = new DoubleAnimation(1, 0, time);
            DoubleAnimation animimage2 = new DoubleAnimation(0, 1, time);
            DoubleAnimation animimage0 = new DoubleAnimation(0, TimeSpan.Zero);
            if (pos == 1)
            {
                El1.SetBack(new SolidColorBrush(BackRectangle.GetColor(types[step+1])));
                El1.SetForward(TypeElement.GetBrush(types[step+1]));
                El1.BeginAnimation(Canvas.OpacityProperty, animimage2);
                El2.BeginAnimation(Canvas.OpacityProperty, animimage1);
                //Hand1.BeginAnimation(Rectangle.OpacityProperty, animimage1);
                //Hand4.BeginAnimation(Rectangle.OpacityProperty, animimage2);
                //Hand2.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                //Hand3.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                pos = 4;
            }
            else if (pos == 2)
            {
                El2.SetBack(new SolidColorBrush(BackRectangle.GetColor(types[step+1])));
                El2.SetForward(TypeElement.GetBrush(types[step+1]));
                El2.BeginAnimation(Canvas.OpacityProperty, animimage2);
                El1.BeginAnimation(Canvas.OpacityProperty, animimage1);
                // Hand2.BeginAnimation(Rectangle.OpacityProperty, animimage1);
                // Hand1.BeginAnimation(Rectangle.OpacityProperty, animimage2);
                // Hand3.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                // Hand4.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                pos = 1;
            }
            else if (pos == 3)
            {
                El1.SetBack(new SolidColorBrush(BackRectangle.GetColor(types[step+1])));
                El1.SetForward(TypeElement.GetBrush(types[step+1]));
                El1.BeginAnimation(Canvas.OpacityProperty, animimage2);
                El2.BeginAnimation(Canvas.OpacityProperty, animimage1);
                // Hand3.BeginAnimation(Rectangle.OpacityProperty, animimage1);
                // Hand2.BeginAnimation(Rectangle.OpacityProperty, animimage2);
                // Hand1.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                //  Hand4.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                pos = 2;
            }
            else
            {
                El2.SetBack(new SolidColorBrush(BackRectangle.GetColor(types[step+1])));
                El2.SetForward(TypeElement.GetBrush(types[step+1]));
                El2.BeginAnimation(Canvas.OpacityProperty, animimage2);
                El1.BeginAnimation(Canvas.OpacityProperty, animimage1);
                // Hand4.BeginAnimation(Rectangle.OpacityProperty, animimage1);
                //  Hand3.BeginAnimation(Rectangle.OpacityProperty, animimage2);
                //  Hand1.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                //  Hand2.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                pos = 3;
            }
        }

        int pos = 4;
        TimeSpan time = TimeSpan.FromSeconds(0.3);
        private void LeftButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            new MySound("Interface/Menu_circle_anticlockwise.wav");
            int newpos = pos + 1;
            DoubleAnimation anim = new DoubleAnimation(pos * 90, newpos * 90, time);
            anim.AccelerationRatio = 0.3;
            anim.DecelerationRatio = 0.3;
            Round.Rotate_Button.BeginAnimation(RotateTransform.AngleProperty, anim);
            double curangle = Round.Rotate_STV1.Angle;
            DoubleAnimation stv1_Anim = new DoubleAnimation(curangle - 90, time);
            stv1_Anim.AccelerationRatio = 0.3;
            stv1_Anim.DecelerationRatio = 0.3;
            Round.Rotate_STV1.BeginAnimation(RotateTransform.AngleProperty, stv1_Anim);
            DoubleAnimation animimage1 = new DoubleAnimation(1, 0, time);
            DoubleAnimation animimage2 = new DoubleAnimation(0, 1, time);
            DoubleAnimation animimage0 = new DoubleAnimation(0, TimeSpan.Zero);
            if (pos == 1)
            {
                El1.SetBack(new SolidColorBrush(BackRectangle.GetColor(types[step+1])));
                El1.SetForward(TypeElement.GetBrush(types[step+1]));
                El1.BeginAnimation(Canvas.OpacityProperty, animimage2);
                El2.BeginAnimation(Canvas.OpacityProperty, animimage1);
                //  Hand1.BeginAnimation(Rectangle.OpacityProperty, animimage1);
                // Hand2.BeginAnimation(Rectangle.OpacityProperty, animimage2);
                //  Hand3.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                //  Hand4.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                pos = 2;
            }
            else if (pos == 2)
            {
                El2.SetBack(new SolidColorBrush(BackRectangle.GetColor(types[step+1])));
                El2.SetForward(TypeElement.GetBrush(types[step+1]));
                El2.BeginAnimation(Canvas.OpacityProperty, animimage2);
                El1.BeginAnimation(Canvas.OpacityProperty, animimage1);
                //   Hand2.BeginAnimation(Rectangle.OpacityProperty, animimage1);
                //  Hand3.BeginAnimation(Rectangle.OpacityProperty, animimage2);
                //  Hand1.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                //  Hand4.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                pos = 3;
            }
            else if (pos == 3)
            {
                El1.SetBack(new SolidColorBrush(BackRectangle.GetColor(types[step+1])));
                El1.SetForward(TypeElement.GetBrush(types[step+1]));
                El1.BeginAnimation(Canvas.OpacityProperty, animimage2);
                El2.BeginAnimation(Canvas.OpacityProperty, animimage1);
                // Hand3.BeginAnimation(Rectangle.OpacityProperty, animimage1);
                //  Hand4.BeginAnimation(Rectangle.OpacityProperty, animimage2);
                //  Hand1.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                //  Hand2.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                pos = 4;
            }
            else
            {
                El2.SetBack(new SolidColorBrush(BackRectangle.GetColor(types[step+1])));
                El2.SetForward(TypeElement.GetBrush(types[step+1]));
                El2.BeginAnimation(Canvas.OpacityProperty, animimage2);
                El1.BeginAnimation(Canvas.OpacityProperty, animimage1); 
             //   Hand4.BeginAnimation(Rectangle.OpacityProperty, animimage1);
              //  Hand1.BeginAnimation(Rectangle.OpacityProperty, animimage2);
             //   Hand2.BeginAnimation(Rectangle.OpacityProperty, animimage0);
             //   Hand3.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                pos = 1;
            }
        }
    }
    class RoundCanvas : Canvas
    {
        public RotateTransform Rotate_STV1;
        public RotateTransform Rotate_STV2;
        public RotateTransform Rotate_Button;
        public Canvas ButtonCanvas;
        public Rectangle LeftButton;
        public Rectangle RightButton;
        public Rectangle Regerator;
        public RoundCanvas()
        {
            Canvas.SetZIndex(this, 30);
            Width = 631;
            Height = 600;
            Rectangle roundbase = ElementsCanvas.GetRectangle(594, 594, 662, 231, Links.Brushes.Science.Round, 0, 1);
            Children.Add(roundbase);
            Rectangle STV1 = ElementsCanvas.GetRectangle(600, 600, 659, 228, Links.Brushes.Science.STV1, 0, 1);
            Children.Add(STV1);
            STV1.RenderTransformOrigin = new Point(0.5, 0.5);
            Rotate_STV1 = new RotateTransform();
            STV1.RenderTransform = Rotate_STV1;

            Rectangle STV2 = ElementsCanvas.GetRectangle(600, 600, 662, 228, Links.Brushes.Science.STV2, 0, 1);
            Children.Add(STV2);
            STV2.RenderTransformOrigin = new Point(0.5, 0.5);
            Rotate_STV2 = new RotateTransform();
            STV2.RenderTransform = Rotate_STV2;

            ButtonCanvas = new Canvas();
            ButtonCanvas.Width = 594; ButtonCanvas.Height = 594;
            Canvas.SetLeft(ButtonCanvas, 662);
            Canvas.SetTop(ButtonCanvas, 231);
            Children.Add(ButtonCanvas);
            ButtonCanvas.RenderTransformOrigin = new Point(0.5, 0.5);
            Rotate_Button = new RotateTransform();
            ButtonCanvas.RenderTransform = Rotate_Button;

            Regerator = ElementsCanvas.GetRectangle(155, 258, 473, 168, Links.Brushes.Science.Regirator, 0, 1);
            ButtonCanvas.Children.Add(Regerator);

            LeftButton = ElementsCanvas.GetRectangle(77, 103, 500, 400, Links.Brushes.Science.Button, 0, 1);
            ButtonCanvas.Children.Add(LeftButton);

            RightButton = ElementsCanvas.GetRectangle(77, 103, 500, 90, Links.Brushes.Science.Button, 0, 1);
            ButtonCanvas.Children.Add(RightButton); RightButton.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform scale = new ScaleTransform();
            scale.ScaleY = -1;
            RightButton.RenderTransform = scale;
        }
        public Rectangle GetUpperRectangle()
        {
            VisualBrush brush = new VisualBrush(this);
            Rectangle rect = ElementsCanvas.GetRectangle(631, 600, 659, 228, brush, 0, 1);
            rect.Clip = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M0,0 v325 h10 l10,7 l5,18 l40,0 h434 v3 l40,15 v10 l60,20 h34  v-399z"));
            return rect;
        }
        public Rectangle GetLowerRectangle()
        {
            VisualBrush brush = new VisualBrush(this);
            Rectangle rect = ElementsCanvas.GetRectangle(631, 600, 659, 228, brush, 0, 1);
            rect.Clip = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M0,325 h10 l10,7 l5,18 l40,0 h434 v3 l40,15 v10 l60,20 h34  v201 h-631z"));
            return rect;
        }
    }
    class Spark
    {
        static Random rnd = new Random();
        List<Canvas> Rects = new List<Canvas>();
        Canvas cur;
        public Spark(Point pt, Canvas canvas, double size)
        {
            cur = canvas;
            int count = rnd.Next(15, 25);
            for (int i = 0; i < count; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Width = 5 * size;

                rect.Height = rnd.Next((int)(25 * size), (int)(55 * size));

                rect.Stroke = Brushes.Orange;
                rect.Fill = Brushes.White;
                rect.StrokeThickness = 3 * size;
                rect.Opacity = 1;
                Canvas c = new Canvas();
                Canvas.SetZIndex(c, 50);
                c.Width = 5 * size;
                c.Height = rnd.Next((int)(150 * size), (int)(600 * size));
                c.ClipToBounds = true;
                c.Children.Add(rect);
                RotateTransform rotate = new RotateTransform();
                c.RenderTransformOrigin = new Point(0.5, 0);
                Canvas.SetLeft(c, pt.X - 2.5 * size);
                Canvas.SetTop(c, pt.Y);
                c.RenderTransform = rotate;
                int angle = rnd.Next(0, 360);
                rotate.Angle = angle;
                Rects.Add(c);
                cur.Children.Add(c);
                int length = rnd.Next(3);
                if (length == 0)
                {
                    DoubleAnimation anim = new DoubleAnimation(0, 600 * size, TimeSpan.FromSeconds(0.4));
                    anim.DecelerationRatio = 1;
                    anim.Completed += Anim_Completed;
                    rect.BeginAnimation(Canvas.TopProperty, anim);
                }
                else if (length == 1)
                {
                    DoubleAnimation anim = new DoubleAnimation(0, 450 * size, TimeSpan.FromSeconds(0.4));
                    anim.DecelerationRatio = 1;
                    anim.Completed += Anim_Completed;
                    rect.BeginAnimation(Canvas.TopProperty, anim);
                }
                else
                {
                    DoubleAnimation anim = new DoubleAnimation(0, 150 * size, TimeSpan.FromSeconds(0.4));
                    anim.DecelerationRatio = 1;
                    anim.Completed += Anim_Completed;
                    rect.BeginAnimation(Canvas.TopProperty, anim);
                }
            }
        }

        private void Anim_Completed(object sender, EventArgs e)
        {
            foreach (Canvas c in Rects)
                cur.Children.Remove(c);
        }
    }
    class PricePanel : Canvas
    {
        double width;
        int money, metall, chips, anti;
        TextBlock Money, Metall, Chips, Anti;
        public PricePanel(double width)
        {
            this.width = width;
            Width = width; Height = width / 3;
            Background = Links.Brushes.Interface.MBButtonBack;

            Money = AddBack(14, 44, 17, 23, Links.Brushes.MoneyImageBrush);
            Metall = AddBack(14, 44, 52, 58, Links.Brushes.MetalImageBrush);
            Chips = AddBack(156, 186, 17, 23, Links.Brushes.ChipsImageBrush);
            Anti = AddBack(156, 186, 52, 58, Links.Brushes.AntiImageBrush);

        }
        TextBlock AddBack(double left1, double left2, double top, double top2, Brush brush)
        {
            Rectangle ib = Common.GetRectangle((int)(width / 10), Links.Brushes.Interface.ImageBack);
            Children.Add(ib); Canvas.SetLeft(ib, width * left1 / 300); Canvas.SetTop(ib, width * top / 300);
            Rectangle tb = new Rectangle(); tb.Width = width / 3; tb.Height = width / 10; tb.Fill = Links.Brushes.Interface.BigSquare;
            Children.Add(tb); Canvas.SetLeft(tb, width * left2 / 300); Canvas.SetTop(tb, width * top / 300);
            TextBlock block = Common.GetBlock((int)(width / 20), "", Brushes.White, (int)(width / 3));
            Children.Add(block); Canvas.SetLeft(block, width * left2 / 300); Canvas.SetTop(block, width * top2 / 300);
            Rectangle rect = Common.GetRectangle((int)(width / 12), brush);
            Children.Add(rect); Canvas.SetLeft(rect, width * (left1 + 2) / 300); Canvas.SetTop(rect, width * (top + 2) / 300);
            return block;
        }
        static double[] stepsvalue = new double[] { 0, 0.3, 0.55, 0.75, 0.9, 0.95, 1.0 };
        static double[] stepstime = new double[] { 0, 0.05, 0.15, 0.3, 0.5, 0.75, 1.0 };
        string Symbol;
        public void ChangePrice(ItemPrice price, params string[] symbol)
        {
            if (symbol.Length > 0)
                Symbol = symbol[0];
            else
                Symbol = "";
            if (price.Money > GSGameInfo.Money) Money.Foreground = Brushes.Red; else Money.Foreground = Brushes.White;
            if (money != price.Money)
                AddChangeAnim(Money, money, price.Money);
            if (price.Metall > GSGameInfo.Metals) Metall.Foreground = Brushes.Red; else Metall.Foreground = Brushes.White;
            if (metall != price.Metall)
                AddChangeAnim(Metall, metall, price.Metall);
            if (price.Chips > GSGameInfo.Chips) Chips.Foreground = Brushes.Red; else Chips.Foreground = Brushes.White;
            if (chips != price.Chips)
                AddChangeAnim(Chips, chips, price.Chips);
            if (price.Anti > GSGameInfo.Anti) Anti.Foreground = Brushes.Red; else Anti.Foreground = Brushes.White;
            if (anti != price.Anti)
                AddChangeAnim(Anti, anti, price.Anti);
            money = price.Money; metall = price.Metall; chips = price.Chips; anti = price.Anti;

        }
        void AddChangeAnim(TextBlock block, int curvalue, int newvalue)
        {
            int delta = newvalue - curvalue;
            StringAnimationUsingKeyFrames anim = new StringAnimationUsingKeyFrames();
            anim.Duration = TimeSpan.FromSeconds(0.3);
            List<double> values = new List<double>();
            for (int i = 0; i < 7; i++)
                anim.KeyFrames.Add(new DiscreteStringKeyFrame(Symbol + (curvalue + delta * stepsvalue[i]).ToString("### ### ### ###"), KeyTime.FromPercent(stepstime[i])));

            block.BeginAnimation(TextBlock.TextProperty, anim);
        }
    }
}
