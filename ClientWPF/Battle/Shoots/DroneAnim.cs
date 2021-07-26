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
using System.Windows.Threading;

namespace Client
{
    enum Gun { Left, Top,Right }
    class DroneAnim : ShootAnim
    {
        public static byte ShieldMode = 1;
        public new static bool HaveSelfSparks = true;
        byte curshoot = 0;
        int drones = 3;
        public int shoots = 9;
        Gun Direction;
        public bool InHull;
        public bool IsMiss;
        public DroneAnim(Point firstPoint, Point secondPoint, bool ismiss, bool inhull, Gun direction)
            : base(firstPoint, secondPoint, ismiss)
        {
            IsMiss = ismiss;
            InHull = inhull;
            Direction = direction;
            CreateCanvas(200);
            PathCanvas.ClipToBounds = false;
            //PathCanvas.Background = Brushes.White;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(Links.ShootAnimSpeed * 0.5);
            timer.Tick += Timer_Tick;
            timer.Start();
            drones = 3;
            if (IsRealBattle)
                BattleController.ShieldFlashStarted(TimeSpan.FromSeconds((RealLength / 1500 + 0.8) * Links.ShootAnimSpeed), Shoots(), WaveDelay(), Angle, 2);
            else
                HelpImage.ShowWeaponCanvas.ShieldFlashStarted(TimeSpan.FromSeconds((RealLength / 1500 + 0.8) * Links.ShootAnimSpeed), Shoots(), WaveDelay(), Angle);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            curshoot++;
            new OneElement(Direction, this);
            if (curshoot == 3) ((DispatcherTimer)sender).Stop();
        }
        public static TimeSpan WaveDelay()
        {
            return TimeSpan.FromSeconds(Links.ShootAnimSpeed / 3.5);
        }
        public static int Shoots()
        {
            return 9;
        }

        public void DroneRemove()
        {
            drones--;
            Links.Controller.mainWindow.Title = String.Format("Drones={0} Shoots={1}", drones.ToString(), shoots.ToString());
            if (drones == 0 && shoots==0)
                Anim_Completed(null, null);
        }
        public void ShootRemove()
        {
            shoots--;
            Links.Controller.mainWindow.Title = String.Format("Drones={0} Shoots={1}", drones.ToString(), shoots.ToString());
            if (drones == 0 && shoots == 0)
                Anim_Completed(null, null);
        }
        public void AddCritAnim()
        {
            new PutCritDrone(Length, this);
        }
        public class PutCritDrone
        {
            double Length;
            Canvas DroneCanvas;
            DroneAnim CurDrone;
            static PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            static ImageBrush Image = new ImageBrush(new BitmapImage(new Uri("Images/Shoots/Drone/drone2.png", UriKind.Relative)));
            ImageBrush FireImage = new ImageBrush(new BitmapImage(new Uri("Images/Shoots/Drone/drone_fire.png", UriKind.Relative)));
            public PutCritDrone(double length, DroneAnim drone)
            {
                CurDrone = drone;

                DroneCanvas = new Canvas();
                drone.PathCanvas.Children.Add(DroneCanvas);
                Ellipse Fire = new Ellipse(); Fire.Width = 30; Fire.Height = 30; Fire.Fill = FireImage;
                DroneCanvas.Children.Add(Fire); Canvas.SetTop(Fire, -20); Canvas.SetLeft(Fire, 85);
                DoubleAnimation anim = new DoubleAnimation(0.2, 1.0, TimeSpan.FromSeconds(0.3));
                anim.RepeatBehavior = RepeatBehavior.Forever;
                anim.AutoReverse = true;
                Fire.BeginAnimation(Ellipse.OpacityProperty, anim);
                Ellipse el = new Ellipse(); el.Width = 40; el.Height = 40; el.Fill = Image;
                Canvas.SetLeft(el, 80);
                DroneCanvas.Children.Add(el);
                DoubleAnimation moveanim = new DoubleAnimation(0, length, TimeSpan.FromSeconds(length / 1000 * Links.ShootAnimSpeed));
                moveanim.Completed += Moveanim_Completed;
                DroneCanvas.BeginAnimation(Canvas.TopProperty, moveanim);
            }
            public static Canvas GetElementCanvas()
            {
                Canvas result = new Canvas();
                result.Width = 100; result.Height = 100;
                Ellipse el0 = new Ellipse(); el0.Width = 50; el0.Height = 50; el0.Stroke = Brushes.LightGreen;
                result.Children.Add(el0); Canvas.SetLeft(el0, 25); Canvas.SetTop(el0, 25);
                Ellipse el1 = new Ellipse(); el1.Width = 20; el1.Height = 20; el1.Fill = Brushes.LightGreen;
                result.Children.Add(el1); Canvas.SetLeft(el1, 40); Canvas.SetTop(el1, 17);
                Ellipse el2 = new Ellipse(); el2.Width = 20; el2.Height = 20; el2.Fill = Brushes.LightGreen;
                result.Children.Add(el2); Canvas.SetLeft(el2, 20); Canvas.SetTop(el2, 50);
                Ellipse el3 = new Ellipse(); el3.Width = 20; el3.Height = 20; el3.Fill = Brushes.LightGreen;
                result.Children.Add(el3); Canvas.SetLeft(el3, 60); Canvas.SetTop(el3, 50);
                return result;
            }
            private void Moveanim_Completed(object sender, EventArgs e)
            {
                Canvas elementcanvas = GetElementCanvas();
                CurDrone.PathCanvas.Children.Add(elementcanvas);
                Canvas.SetLeft(elementcanvas, 50);
                Canvas.SetTop(elementcanvas, Canvas.GetTop(DroneCanvas) - 20);
                DoubleAnimation hide = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5 * Links.ShootAnimSpeed));
                DoubleAnimation show = new DoubleAnimation(0, 1, hide.Duration);
                DroneCanvas.BeginAnimation(Canvas.OpacityProperty, hide);
                elementcanvas.BeginAnimation(Canvas.OpacityProperty, show);
            }

        }
        class OneElement
        {
            DroneAnim CurDrone;
            Canvas DroneCanvas;
            static PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            static ImageBrush Image = new ImageBrush(new BitmapImage(new Uri("Images/Shoots/Drone/drone2.png",UriKind.Relative)));
            static ImageBrush FireImage = new ImageBrush(new BitmapImage(new Uri("Images/Shoots/Drone/drone_fire.png",UriKind.Relative)));
            static byte shoots = 3;
            byte curshoot = 0;
            public OneElement(Gun gun, DroneAnim drone)
            {
                CurDrone = drone;
                PathGeometry data;
                switch (gun)
                {
                    case Gun.Right: data = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M100,0 a175,175 0 0,1 -200,100 a100,25 0 1,0 0,15 a100,25 0 1,1 0,15 a100,100 0 0,1 200,-130")); break;
                    default: data = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M100,0 a175,175 0 0,0 200,100 a100,50 0 1,1 0,15 a100,50 0 1,0 0,15 a100,100 0 0,0 -200,-130")); break;
                }
                DroneCanvas = new Canvas();
                drone.PathCanvas.Children.Add(DroneCanvas);
                Ellipse Fire = new Ellipse(); Fire.Width = 30; Fire.Height = 30; Fire.Fill = FireImage;
                DroneCanvas.Children.Add(Fire); Canvas.SetTop(Fire, -20); Canvas.SetLeft(Fire, 5);
                DoubleAnimation anim = new DoubleAnimation(0.2, 1.0, TimeSpan.FromSeconds(0.3));
                anim.RepeatBehavior = RepeatBehavior.Forever;
                anim.AutoReverse = true;
                Fire.BeginAnimation(Ellipse.OpacityProperty, anim);
                Ellipse el = new Ellipse(); el.Width = 40; el.Height = 40; el.Fill = Image;
                DroneCanvas.Children.Add(el);
                DoubleAnimationUsingPath animX = new DoubleAnimationUsingPath();
                animX.Duration = TimeSpan.FromSeconds(3.0 * Links.ShootAnimSpeed);
                animX.PathGeometry = (PathGeometry)data;
                animX.Source = PathAnimationSource.X;
                DroneCanvas.BeginAnimation(Canvas.LeftProperty, animX);
                DoubleAnimationUsingPath animY = new DoubleAnimationUsingPath();
                animY.Duration = TimeSpan.FromSeconds(3.0 * Links.ShootAnimSpeed);
                animY.PathGeometry = (PathGeometry)data;
                animY.Source = PathAnimationSource.Y;
                animY.Completed += AnimY_Completed;
                DroneCanvas.BeginAnimation(Canvas.TopProperty, animY);
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(Links.ShootAnimSpeed * 0.75);
                timer.Tick += Timer_Tick1;
                timer.Start();
            }

            private void Timer_Tick1(object sender, EventArgs e)
            {
                curshoot++; 
                Line l = new Line(); l.Stroke = Brushes.Green;
                Point pt = new Point(Canvas.GetLeft(DroneCanvas) + 30, Canvas.GetTop(DroneCanvas));
                OneShoot oneshoot = new OneShoot(pt, new Point(100, CurDrone.PathCanvas.Height), CurDrone);
                CurDrone.PathCanvas.Children.Add(oneshoot.PathCanvas);
                if (curshoot == shoots) ((DispatcherTimer)sender).Stop();
            }

            private void AnimY_Completed(object sender, EventArgs e)
            {
                CurDrone.PathCanvas.Children.Remove(DroneCanvas);
                CurDrone.DroneRemove();
            }
            class OneShoot
            {
                public Canvas PathCanvas;
                ImageBrush Image = new ImageBrush(new BitmapImage(new Uri("Images/Shoots/Drone/drone_shot.png",UriKind.Relative)));
                DroneAnim CurDrone;
                double Length;
                Rectangle rect;
                public OneShoot(Point firstPoint, Point secondPoint, DroneAnim drone)
                {
                    CurDrone = drone;
                    if (firstPoint.X == secondPoint.X && firstPoint.Y == secondPoint.Y)
                    {
                        firstPoint = new Point(0, 0); secondPoint = new Point(1, 1);
                    }
                    double dy = firstPoint.Y - secondPoint.Y;
                    double dx = secondPoint.X - firstPoint.X;
                    double RealLength = Math.Sqrt(Math.Pow(firstPoint.X - secondPoint.X, 2) + Math.Pow(firstPoint.Y - secondPoint.Y, 2));
                    double tga;
                    dy = firstPoint.Y - secondPoint.Y;
                    dx = secondPoint.X - firstPoint.X;
                    Length = Math.Sqrt(Math.Pow(firstPoint.X - secondPoint.X, 2) + Math.Pow(firstPoint.Y - secondPoint.Y, 2));
                    double Angle;
                    tga = dy / dx;
                    if (dx == 0)
                        Angle = dy > 0 ? 0 : 180;
                    else if (dx > 0)
                        Angle = 90 - Math.Atan(tga) * 180 / Math.PI;
                    else
                        Angle = 270 - Math.Atan(tga) * 180 / Math.PI;
                    double otstup = 400;
                    
                    PathCanvas = new Canvas();
                    Canvas.SetZIndex(PathCanvas, Links.ZIndex.Shoots);
                    PathCanvas.Width = 40;
                    //PathCanvas.ClipToBounds = true;
                    PathCanvas.Height = Length;
                    Canvas.SetLeft(PathCanvas, firstPoint.X - PathCanvas.Width / 2);
                    Canvas.SetTop(PathCanvas, firstPoint.Y);
                    RotateTransform transform = new RotateTransform(Angle + 180);
                    PathCanvas.RenderTransformOrigin = new Point(0.5, 0);
                    PathCanvas.RenderTransform = transform;

                    rect = new Rectangle(); rect.Width = 40; rect.Height = 200; rect.Fill = Image;
                    PathCanvas.Children.Add(rect);
                    if (CurDrone.IsMiss == false && CurDrone.InHull == false)
                    {
                        if (Length <= 400) otstup = Length - 1;
                        TimeSpan ts = TimeSpan.FromSeconds((Length - otstup) / 1500 * Links.ShootAnimSpeed);
                        DoubleAnimation anim = new DoubleAnimation(-0, Length - otstup, ts);
                        if (ts > TimeSpan.FromMilliseconds(10))
                            anim.Completed += Anim_Completed2;
                        else
                            Anim_Completed2(null, null);
                        rect.BeginAnimation(Canvas.TopProperty, anim);
                    }
                    else
                    {
                        otstup = 200;
                        if (Length <= 200) otstup = Length - 1;
                        TimeSpan ts = TimeSpan.FromSeconds((Length - otstup) / 1500 * Links.ShootAnimSpeed);
                        DoubleAnimation anim = new DoubleAnimation(-0, Length - otstup, ts);
                        if (ts > TimeSpan.FromMilliseconds(10))
                            anim.Completed += Anim_Completed1;
                        else
                            Anim_Completed1(null, null);
                        rect.BeginAnimation(Canvas.TopProperty, anim);
                    }
                }
                private void Anim_Completed2(object sender, EventArgs e)
                {
                    new MakeShieldSpark(this, Length);
                    DoubleAnimation anim = new DoubleAnimation(Length - 400, Length - 200, TimeSpan.FromSeconds((200) / 1500 * Links.ShootAnimSpeed));
                    rect.BeginAnimation(Canvas.TopProperty, anim);
                }
                private void Anim_Completed1(object sender, EventArgs e)
                {
                    if (CurDrone.IsMiss == true)
                    {
                        Canvas parrent = (Canvas)PathCanvas.Parent;
                        parrent.Children.Remove(PathCanvas);
                        CurDrone.ShootRemove();
                    }
                    else if (CurDrone.InHull)
                    {
                        new MakeHullSpark(this, Length);

                    }
                }

                public void Hull_Spark_Completed()
                {
                    Canvas parrent = (Canvas)PathCanvas.Parent;
                    parrent.Children.Remove(PathCanvas);
                    CurDrone.ShootRemove();
                }
            }
            class MakeHullSpark
            {
                public static ImageBrush Image = new ImageBrush(new BitmapImage(new Uri("Images/Shoots/Drone/FlashGreen.png", UriKind.Relative)));
                Rectangle flash;
                OneShoot Shoot;
                public MakeHullSpark(OneShoot shoot, double Length)
                {
                    Shoot = shoot;
                    flash = new Rectangle(); flash.Width = 425; flash.Height = 335;
                    flash.Fill = Image;
                    RectAnimationUsingKeyFrames anim = new RectAnimationUsingKeyFrames();
                    anim.Duration = TimeSpan.FromSeconds(0.1 * Links.ShootAnimSpeed);
                    anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.0, 0, 0.1, 1), KeyTime.FromPercent(0)));
                    anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.1, 0, 0.1, 1), KeyTime.FromPercent(0.2)));
                    anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.2, 0, 0.1, 1), KeyTime.FromPercent(0.4)));
                    anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.3, 0, 0.1, 1), KeyTime.FromPercent(0.6)));
                    anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.5, 0, 0.1, 1), KeyTime.FromPercent(0.8)));
                    anim.Completed += Anim_Completed;
                    Image.BeginAnimation(ImageBrush.ViewboxProperty, anim);
                    Shoot.PathCanvas.Children.Add(flash); Canvas.SetLeft(flash, -190); Canvas.SetTop(flash, Length - 170);
                }

                private void Anim_Completed(object sender, EventArgs e)
                {
                    Shoot.Hull_Spark_Completed();
                }
            }
            class MakeShieldSpark
            {
                public static ImageBrush Image = new ImageBrush(new BitmapImage(new Uri("Images/Shoots/Drone/FlashBlue.png",UriKind.Relative)));
                Rectangle flash;
                OneShoot Shoot;
                public MakeShieldSpark(OneShoot shoot, double Length)
                {
                    Shoot = shoot;
                    flash = new Rectangle(); flash.Width = 425; flash.Height = 335;
                    flash.Fill = Image;
                    RectAnimationUsingKeyFrames anim = new RectAnimationUsingKeyFrames();
                    anim.Duration = TimeSpan.FromSeconds(0.1 * Links.ShootAnimSpeed);
                    anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.0, 0, 0.1, 1), KeyTime.FromPercent(0)));
                    anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.1, 0, 0.1, 1), KeyTime.FromPercent(0.2)));
                    anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.2, 0, 0.1, 1), KeyTime.FromPercent(0.4)));
                    anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.3, 0, 0.1, 1), KeyTime.FromPercent(0.6)));
                    anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.5, 0, 0.1, 1), KeyTime.FromPercent(0.8)));
                    anim.Completed += Anim_Completed;
                    Image.BeginAnimation(ImageBrush.ViewboxProperty, anim);
                    Shoot.PathCanvas.Children.Add(flash); Canvas.SetLeft(flash, -190); Canvas.SetTop(flash, Length - 320);
                }

                private void Anim_Completed(object sender, EventArgs e)
                {
                    Shoot.Hull_Spark_Completed();
                }
            }
        }
    }
}
