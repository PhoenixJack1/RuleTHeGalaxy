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

namespace InterfeysBiya
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow ThisWindow;
        bool Status = false;
        IntBoya b;
        public MainWindow()
        {
            InitializeComponent();
            ThisWindow = this;
            b = new IntBoya();
            box.Child = b;
            b.PreviewMouseDown += B_PreviewMouseDown;
            //Rectangle rect = new Rectangle();
            //rect.Width = 50; rect.Height = 5; rect.Fill = Brushes.Red;
            //b.Children.Add(rect);
            //rect.RenderTransform = rotate = new RotateTransform();
            //rotate.Angle = 5;
            //DoubleAnimation anim = new DoubleAnimation(0, 360, TimeSpan.FromSeconds(1 / 60.0));
            //anim.RepeatBehavior = RepeatBehavior.Forever;
            //rotate.BeginAnimation(RotateTransform.AngleProperty, anim);
            b.Start();
        }

        private void B_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Status== true)
            {
                Status = false;
                b.Stop();    
            }
            else
            {
                Status = true;
                b.Restart();
            }
        }
    }
    class IntBoya:Canvas
    {
        public bool Status = false;
        bool IsYellowLine = false;
        bool IsBlueLine = false;
        public bool IsBigFire = false;
        public bool IsSmallFire = false;
        bool IsRightLine = false;
        BigFireMove BigFire;
        SmallFireMove SmallFire;
        Canvas BottomCanvas;
        Canvas UpCanvas;
        Canvas BackCanvas;
        RotateTransform End0R, End1R, End2R;
        Canvas EndTurnCanvas;
        Canvas ExitCanvas;
        RotateTransform Exit0R, Exit1R, Exit2R;
        public IntBoya()
        {
            Width = 1920; Height = 1080;
            ClipToBounds = true;
            Background = Brushes.Black;
            BackCanvas = new Canvas();
            Children.Add(BackCanvas);
            BackCanvas.Width = 25;
            BackCanvas.Height = 1080 * 2;
            Canvas.SetLeft(BackCanvas, 1895);
            PutImage(25, 1080, 0, 0, "YellowRight", BackCanvas);
            PutImage(25, 1080, 0, 1080, "YellowRight", BackCanvas);
            PutImage(1152, 56, 540, 1014, "Bottom", this);
            BottomCanvas = new Canvas();
            Children.Add(BottomCanvas);
            BottomCanvas.Width = 1920 * 2;
            BottomCanvas.Height = 25;
            Canvas.SetTop(BottomCanvas, 1055);
            PutImage(1920, 25, 0, 0, "SideYellow", BottomCanvas);
            PutImage(1920, 25, 1920, 0, "SideYellow", BottomCanvas);

            UpCanvas = new Canvas();
            Children.Add(UpCanvas);
            UpCanvas.Width = 1920 * 2;
            UpCanvas.Height = 25;
            Canvas.SetTop(UpCanvas, 1030);
            PutImage(1920, 25, 0, 0, "SideBlue", UpCanvas);
            PutImage(1920, 25, 1920, 0, "SideBlue", UpCanvas);

            //PutImage(25, 1080, 1895, 0, "YellowRight", this);
            PutImage(241, 207, 1679, 873, "RightBottomCorner", this);
            PutImage(552, 171, 0, 909, "LeftBottomCorner", this);
            PutImage(627, 924, 1293, 0, "RightUpCorner", this);
            PutImage(228, 559, 0, 0, "LeftUpCorner",this);
            PutImage(914, 88, 450, 0, "TopCenter", this);
            BigFire = new BigFireMove(this);
            SmallFire = new SmallFireMove(this);
            PutImage(103, 102, 1803, 180, "Items", this);
            PutImage(103, 102, 1803, 305, "Items", this);
            PutImage(103, 102, 1803, 437, "Items", this);
            EndTurnCanvas = new Canvas(); EndTurnCanvas.Width = 185; EndTurnCanvas.Height = 185;
            Children.Add(EndTurnCanvas); Canvas.SetLeft(EndTurnCanvas, 1715); Canvas.SetTop(EndTurnCanvas, 882);
            Rectangle r0 = PutImage(185, 185, 0, 0, "EndTurn0", EndTurnCanvas);
            r0.RenderTransformOrigin = new Point(0.5, 0.5);
            r0.RenderTransform = End0R = new RotateTransform();
            Rectangle r1 = PutImage(129, 129, 29, 28, "EndTurn1", EndTurnCanvas);
            r1.RenderTransformOrigin = new Point(0.5, 0.5);
            r1.RenderTransform = End1R = new RotateTransform();
            Rectangle r2 = PutImage(185, 185, 0, 0, "EndTurn2", EndTurnCanvas);
            r2.RenderTransformOrigin = new Point(0.5, 0.5);
            r2.RenderTransform = End2R = new RotateTransform();
            EndTurnCanvas.PreviewMouseDown += IntBoya_PreviewMouseDown;

            ExitCanvas = new Canvas(); ExitCanvas.Width = 139; ExitCanvas.Height = 139;
            Children.Add(ExitCanvas); Canvas.SetLeft(ExitCanvas, 1770); Canvas.SetTop(ExitCanvas, 9);
            Rectangle ex0 = PutImage(139, 139, 0, 0, "Exit0", ExitCanvas);
            ex0.RenderTransformOrigin = new Point(0.5, 0.5);
            ex0.RenderTransform = Exit0R = new RotateTransform();
            Rectangle ex1 = PutImage(108, 108, 15, 14, "Exit1", ExitCanvas);
            ex1.RenderTransformOrigin = new Point(0.5, 0.5);
            ex1.RenderTransform = Exit1R = new RotateTransform();
            Rectangle ex2 = PutImage(82, 82, 29, 28, "Exit2", ExitCanvas);
            ex2.RenderTransformOrigin = new Point(0.5, 0.5);
            ex2.RenderTransform = Exit2R = new RotateTransform();
            ExitCanvas.MouseEnter += ExitCanvas_MouseEnter;
        }

        private void ExitCanvas_MouseEnter(object sender, EventArgs e)
        {
            if (VisualTreeHelper.HitTest(ExitCanvas, Mouse.GetPosition(ExitCanvas)) == null) return;
            DoubleAnimation anim0 = new DoubleAnimation(0, 360, TimeSpan.FromSeconds(2));
            anim0.AccelerationRatio = 0.5;anim0.DecelerationRatio = 0.5;
            anim0.Completed += ExitCanvas_MouseEnter;
            Exit0R.BeginAnimation(RotateTransform.AngleProperty, anim0);
            DoubleAnimation anim1 = new DoubleAnimation(0, -360, TimeSpan.FromSeconds(2));
            Exit1R.BeginAnimation(RotateTransform.AngleProperty, anim1);
            DoubleAnimation anim2 = new DoubleAnimation(0, 360, TimeSpan.FromSeconds(2));
            Exit2R.BeginAnimation(RotateTransform.AngleProperty, anim2);
        }

        public void EndTurn()
        {
            DoubleAnimation anim0 = new DoubleAnimation(0, 360, TimeSpan.FromSeconds(2));
            anim0.AccelerationRatio = 0.5; anim0.DecelerationRatio = 0.5;
            End0R.BeginAnimation(RotateTransform.AngleProperty, anim0);
            DoubleAnimation anim1 = new DoubleAnimation(0, 90, TimeSpan.FromSeconds(1));
            anim1.AutoReverse = true;
            End1R.BeginAnimation(RotateTransform.AngleProperty, anim1);
            DoubleAnimation anim2 = new DoubleAnimation(360, 0, TimeSpan.FromSeconds(2));
            anim2.AccelerationRatio = 0.5; anim2.DecelerationRatio = 0.5;
            End2R.BeginAnimation(RotateTransform.AngleProperty, anim2);
        }
        private void IntBoya_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            EndTurn();
        }

        private void StartRightLine(object sender, EventArgs e)
        {
            if (Status)
            {
                DoubleAnimation anim = new DoubleAnimation(0, -1080, TimeSpan.FromSeconds(30));
                anim.Completed += StartRightLine;
                BackCanvas.BeginAnimation(Canvas.TopProperty, anim);
                IsRightLine = true;
            }
            else
                IsRightLine = false;
        }
        private void StartBlueLine(object sender, EventArgs e)
        {
            if (Status)
            {
                DoubleAnimation anim = new DoubleAnimation(0, -1920, TimeSpan.FromSeconds(25));
                anim.Completed += StartBlueLine;
                UpCanvas.BeginAnimation(Canvas.LeftProperty, anim);
                IsBlueLine = true;
            }
            else
                IsBlueLine = false;
        }
        private void StartYellowLine(object sender, EventArgs e)
        {
            if (Status)
            {
                DoubleAnimation anim = new DoubleAnimation(0, -1920, TimeSpan.FromSeconds(35));
                anim.Completed += StartYellowLine;
                BottomCanvas.BeginAnimation(Canvas.LeftProperty, anim);
                IsYellowLine = true;
            }
            else
                IsYellowLine = false;
        }
        
        public void Restart()
        {
            Status = true;
            if (IsYellowLine == false) StartYellowLine(null, null);
            if (IsBigFire == false) BigFire.Start();
            if (IsBlueLine == false) StartBlueLine(null, null);
            if (IsSmallFire == false) SmallFire.Start();
            if (IsRightLine == false) StartRightLine(null, null);
        }
        public void Stop()
        {
            Status = false;
        }
        public void Start()
        {
            Status = true;
            BigFire.Start();
            StartYellowLine(null, null);
            StartBlueLine(null, null);
            SmallFire.Start();
            StartRightLine(null, null);
        }

        Rectangle PutImage(int width, int height, int left, int top, string brush, Canvas canvas)
        {
            Rectangle rect = new Rectangle(); rect.Width = width; rect.Height = height;
            canvas.Children.Add(rect); Canvas.SetLeft(rect, left); Canvas.SetTop(rect, top);
            rect.Fill = new ImageBrush(new BitmapImage(new Uri(string.Format("C:/123/IB/{0}.png", brush))));
            return rect;
        }
        
        
        class SmallFireMove
        {
            enum Locations { Top1, Top2, Top3, Top4, Middle1, Middle2, Middle3, Middle4, Down1, Down2, Down3, Down4}
            Locations Location = Locations.Middle3;
            Rectangle rect;
            RotateTransform rotate;
            IntBoya Main;
            public SmallFireMove(IntBoya canvas)
            {
                Main = canvas;
                rect = new Rectangle(); rect.Width = 16; rect.Height = 28;
                rect.Fill = new ImageBrush(new BitmapImage(new Uri("C:/123/IB/FireSmall.png")));
                canvas.Children.Add(rect);
                rect.RenderTransformOrigin = new Point(0.5, 0.5);
                rect.RenderTransform = rotate = new RotateTransform();
            }
            public void Start()
            {
                Next(null, null);
            }
            private void Next(object sender, EventArgs e)
            {
                if (Main.Status==true)
                    Main.IsSmallFire = true;
                else
                {
                    Main.IsSmallFire = false;
                    return;
                } 
                Location = (Locations)(((int)Location + 1) % 12);
                float x1, x2, xt=0, y1, y2, yt=0, ht=3;
                switch (Location)
                {
                    case Locations.Top1:
                        rotate.Angle = 0;
                        x1 = 1797; x2 = 1797; y1 = 160; y2 = 260; yt = 3;
                        break;
                    case Locations.Top2:
                        rotate.Angle = -90;
                        x1 = 1800; x2 = 1890; xt = 3; y1 = 265; y2 = 265;
                        break;
                    case Locations.Top3:
                        rotate.Angle = 180;
                        x1 = 1895; x2 = 1895; y1 = 270; y2 = 170; yt = 3;
                        break;
                    case Locations.Top4:
                        rotate.Angle = 90;
                        x1 = 1890; x2 = 1800; xt = 3; y1 = 168; y2 = 168; 
                        break;
                    case Locations.Middle1:
                        rotate.Angle = 0;
                        x1 = 1797; x2 = 1797; y1 = 290; y2 = 390; yt = 3;
                        break;
                    case Locations.Middle2:
                        rotate.Angle = -90;
                        x1 = 1800; x2 = 1890; xt = 3; y1 = 390; y2 = 390;
                        break;
                    case Locations.Middle3:
                        rotate.Angle = 180;
                        x1 = 1895; x2 = 1895; y1 = 395; y2 = 300; yt = 3;
                        break;
                    case Locations.Middle4:
                        rotate.Angle = 90;
                        x1 = 1890; x2 = 1800; xt = 3; y1 = 293; y2 = 293;
                        break;
                    case Locations.Down1:
                        rotate.Angle = 0;
                        x1 = 1797; x2 = 1797; y1 = 420; y2 = 520; yt = 3;
                        break;
                    case Locations.Down2:
                        rotate.Angle = -90;
                        x1 = 1800; x2 = 1890; xt = 3; y1 = 522; y2 = 522;
                        break;
                    case Locations.Down3:
                        rotate.Angle = 180;
                        x1 = 1895; x2 = 1895; y1 = 520; y2 = 430; yt = 3;
                        break;
                    case Locations.Down4:
                        rotate.Angle = 90;
                        x1 = 1890; x2 = 1800; xt = 3; y1 = 425; y2 = 425;
                        break;
                    default: x1 = 0; x2 = 0; xt = 0; y1 = 0; y2 = 0; yt = 0; ht = 0; break;
                }
                DoubleAnimation animx = new DoubleAnimation(x1, x2, TimeSpan.FromSeconds(xt));
                rect.BeginAnimation(Canvas.LeftProperty, animx);
                DoubleAnimation animy = new DoubleAnimation(y1, y2, TimeSpan.FromSeconds(yt));
                rect.BeginAnimation(Canvas.TopProperty, animy);
                DoubleAnimationUsingKeyFrames animh = new DoubleAnimationUsingKeyFrames();
                animh.Duration = TimeSpan.FromSeconds(ht);
                animh.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.0))));
                animh.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.5))));
                animh.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(ht - 0.5))));
                animh.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(ht))));
                animh.Completed += Next;
                rect.BeginAnimation(Rectangle.OpacityProperty, animh);

            }

        }
        class BigFireMove
        {
            enum Locations { Down1, Down2, Up1, Up2, Left1, Right1, Right2, Right3, Right4}
            Locations Location = Locations.Right4;
            Rectangle rect;
            RotateTransform rotate;
            IntBoya Main;
            public BigFireMove(IntBoya canvas)
            {
                Main = canvas;
                rect = new Rectangle(); rect.Width = 80; rect.Height = 32;
                rect.Fill = new ImageBrush(new BitmapImage(new Uri("C:/123/IB/FireBig.png")));
                canvas.Children.Add(rect);
                rect.RenderTransformOrigin = new Point(0.5, 0.5);
                rect.RenderTransform = rotate = new RotateTransform();    
            }
            public void Start()
            {
                Next(null, null);
            }
            private void Next(object sender,EventArgs e)
            {
                if (Main.Status == true)
                    Main.IsBigFire = true;
                else
                {
                    Main.IsBigFire = false;
                    return;
                }
                Location = (Locations)(((int)Location + 1) % 9);
                float x1, x2, xt, y1, y2, yt, ht;
                switch (Location)
                {
                    case Locations.Down1:
                        rotate.Angle = 0;
                        x1 = 0; x2 = 410; xt = 8; y1 = 895; y2 = 895; yt = 0; ht = 8f; 
                        break;
                    case Locations.Down2:
                        rotate.Angle = 60;
                        x1 = 437; x2 = 500; xt = 3; y1 = 925; y2 = 1025; yt = 3; ht = 3;// M428,910 l90,145 v40"
                        break;
                    case Locations.Up1:
                        rotate.Angle = 0;
                        x1 = 460; x2 = 1080; xt = 13; y1 = 72; y2 = 72; yt = 0; ht = 13;
                        break;
                    case Locations.Up2:
                        rotate.Angle = 0;
                        x1 = 1180; x2 = 1290; xt = 3; y1 = 53; y2 = 53; yt = 0; ht = 3;
                        break;
                    case Locations.Left1:
                        rotate.Angle = 90;
                        x1 = 30; x2 = 30; xt = 0; y1 = 100; y2 = 420; yt = 6; ht = 6;
                            break;
                    case Locations.Right1:
                        rotate.Angle = 0;
                        x1 = 1350; x2 = 1650; xt = 5; y1 = 33; y2 = 33; yt = 0; ht = 5;
                        break;
                    case Locations.Right2:
                        rotate.Angle = 90;
                        x1 = 1675; x2 = 1675; xt = 0; y1 = 60; y2 = 120; yt = 1.5f; ht = 1.5f;
                        break;
                    case Locations.Right3:
                        rotate.Angle = 180;
                        x1 = 1670; x2 = 1400; xt = 4; y1 = 133; y2 = 133; yt = 0; ht = 4;
                        break;
                    case Locations.Right4:
                        rotate.Angle = 245;
                        x1 = 1370; x2 = 1345; xt = 2; y1 = 110; y2 = 50; yt = 2; ht = 2;
                        break;
                    default: x1 = 0; x2 = 0; xt = 0; y1 = 0; y2 = 0; yt = 0; ht = 0; break;
                }
                DoubleAnimation animx = new DoubleAnimation(x1, x2, TimeSpan.FromSeconds(xt));
                rect.BeginAnimation(Canvas.LeftProperty, animx);
                DoubleAnimation animy = new DoubleAnimation(y1, y2, TimeSpan.FromSeconds(yt));
                rect.BeginAnimation(Canvas.TopProperty, animy);
                DoubleAnimationUsingKeyFrames animh = new DoubleAnimationUsingKeyFrames();
                animh.Duration = TimeSpan.FromSeconds(ht);
                animh.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.0))));
                animh.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.5))));
                animh.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(ht-0.5))));
                animh.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(ht))));
                animh.Completed += Next;
                rect.BeginAnimation(Rectangle.OpacityProperty, animh);

            }
        }
        
    }
}
