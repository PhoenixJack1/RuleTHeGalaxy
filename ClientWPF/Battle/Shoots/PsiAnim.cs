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

namespace Client
{
    class PsiAnim : ShootAnim
    {
        public static byte ShieldMode = 1;
        static LinearGradientBrush PurlpeBrush = GetBrush();
        Random rnd = new Random(1);
        public PsiAnim(Point firstPoint, Point secondPoint, bool IsMiss)
            : base(firstPoint, secondPoint, IsMiss)
        {
            CreateCanvas(46);
            if (Links.ShootAnimSpeed == 1)
                new MySound("Battle/Psi.mp3");
            //Links.Controller.mainWindow.PlaySound("Psi.wav");
            //Sounds.ShootSound(EWeaponType.Psi);
            Rectangle rect = new Rectangle();
            rect.Width = 46;
            rect.Height = Length;
            rect.Fill = PurlpeBrush;
            PathCanvas.Children.Add(rect);
            Canvas c1 = GetBackGroundCanvas(1, 500);
            PathCanvas.Children.Add(c1);
            Canvas.SetTop(c1, -500);
            Canvas c2 = GetBackGroundCanvas(2, 750);
            PathCanvas.Children.Add(c2);
            Canvas.SetTop(c2, -750);


            DoubleAnimation anim = new DoubleAnimation(-500, 0, TimeSpan.FromSeconds(Links.ShootAnimSpeed * 1.5));
            anim.Completed += new EventHandler(Anim_Completed);
            c1.BeginAnimation(Canvas.TopProperty, anim);
            DoubleAnimation anim2 = new DoubleAnimation(-750, 0, TimeSpan.FromSeconds(Links.ShootAnimSpeed * 1.5));
            c2.BeginAnimation(Canvas.TopProperty, anim2);
            if (IsRealBattle)
                BattleController.ShieldFlashStarted(TimeSpan.FromSeconds((0.01)), Shoots(), WaveDelay(), Angle, ShieldMode);
            else
                HelpImage.ShowWeaponCanvas.ShieldFlashStarted(TimeSpan.FromSeconds((0.01)), Shoots(), WaveDelay(), Angle);
        }
        public static int Shoots()
        {
            return 3;
        }
        public static TimeSpan WaveDelay()
        {
            return TimeSpan.FromSeconds(Links.ShootAnimSpeed / 3);
        }
        Canvas GetBackGroundCanvas(int seed, double dx)
        {
            Random rnd = new Random(seed);
            Canvas c = new Canvas();
            c.Width = 46; c.Height = Length + dx;
            int height = (int)c.Height;
            Path path = new Path();
            PathGeometry geom = new PathGeometry();
            path.Stroke = Brushes.White;
            path.StrokeThickness = 2;
            for (int i = 0; i < 100; i++)
            {
                PathFigure fig = new PathFigure();
                Point pt1 = new Point(rnd.Next(0, 46), rnd.Next(0, height));
                fig.StartPoint = pt1;
                fig.Segments.Add(new LineSegment(new Point(pt1.X, pt1.Y + rnd.Next(15, 30)), true));
                geom.Figures.Add(fig);
            }
            path.Data = geom;
            c.Children.Add(path);
            return c;
        }
        static LinearGradientBrush GetBrush()
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0, 0.5);
            brush.EndPoint = new Point(1, 0.5);
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 139, 0, 255), 0));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 139, 0, 255), 0.5));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 139, 0, 255), 1));
            return brush;
        }
    }
}
