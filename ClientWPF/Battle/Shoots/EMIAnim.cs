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
    class EMIAnim : ShootAnim
    {
        public static byte ShieldMode = 1;
        static LinearGradientBrush brush = GetBrush();
        public EMIAnim(Point firstPoint, Point secondPoint, bool IsMiss)
            : base(firstPoint, secondPoint, IsMiss)
        {
            CreateCanvas(78);
            if (Links.ShootAnimSpeed == 1)
                new MySound("Battle/EMI.mp3");
            //Links.Controller.mainWindow.PlaySound("EMI.wav");
            //Sounds.ShootSound(EWeaponType.EMI);
            //System.Media.SoundPlayer player = new System.Media.SoundPlayer("C://123//EMI.wav");
            //player.Play();
            //PathCanvas.Background = Brushes.White;
            int steps = (int)(Length / 150) + 1;
            PathGeometry left = new PathGeometry();
            PathGeometry right = new PathGeometry();
            PathFigure ll = new PathFigure();
            left.Figures.Add(ll);
            PathFigure rr = new PathFigure();
            right.Figures.Add(rr);
            Path emileft = new Path();
            Path emiright = new Path();
            PathCanvas.Children.Add(emileft);
            PathCanvas.Children.Add(emiright);
            emileft.Data = left;
            emiright.Data = right;
            emileft.StrokeThickness = 15;
            emiright.StrokeThickness = 15;
            ll.StartPoint = new Point(18, 0);
            rr.StartPoint = new Point(60, 0);
            for (int i = 0; i < steps; i++)
            {
                int y = i * 150 + 75;
                ll.Segments.Add(new BezierSegment(new Point(63, y), new Point(-27, y), new Point(18, y + 75), true));
                rr.Segments.Add(new BezierSegment(new Point(15, y), new Point(105, y), new Point(60, y + 75), true));
            }
            emileft.Stroke = brush;
            emiright.Stroke = brush;
            double delta = 90 / Length;
            Color nullcolor = Color.FromArgb(0, 255, 255, 255);
            Color fullcolor = Color.FromArgb(255, 255, 255, 255);
            GradientStop gr1 = new GradientStop(nullcolor, 0 - delta - 0.01);
            GradientStop gr2 = new GradientStop(fullcolor, 0 - delta);
            GradientStop gr3 = new GradientStop(fullcolor, -0.01);
            GradientStop gr4 = new GradientStop(nullcolor, 0);
            LinearGradientBrush oppacitybrush = new LinearGradientBrush();
            oppacitybrush.StartPoint = new Point(0.5, 0);
            oppacitybrush.EndPoint = new Point(0.5, 1);
            oppacitybrush.GradientStops.Add(gr1);
            oppacitybrush.GradientStops.Add(gr2);
            oppacitybrush.GradientStops.Add(gr3);
            oppacitybrush.GradientStops.Add(gr4);
            PathCanvas.OpacityMask = oppacitybrush;
            TimeSpan duration = TimeSpan.FromSeconds((Length / 2000) * Links.ShootAnimSpeed);
            DoubleAnimation anim1 = new DoubleAnimation(0 - delta - 0.01, 1, duration);
            DoubleAnimation anim2 = new DoubleAnimation(0 - delta, 1.01, duration);
            DoubleAnimation anim3 = new DoubleAnimation(-0.01, 1 + delta - 0.01, duration);
            DoubleAnimation anim4 = new DoubleAnimation(0, 1 + delta, duration);
            anim4.Completed += new EventHandler(Anim_Completed);
            gr1.BeginAnimation(GradientStop.OffsetProperty, anim1);
            gr2.BeginAnimation(GradientStop.OffsetProperty, anim2);
            gr3.BeginAnimation(GradientStop.OffsetProperty, anim3);
            gr4.BeginAnimation(GradientStop.OffsetProperty, anim4);
            //BattleController.FireAnnimationStarted(TimeSpan.FromSeconds((RealLength / 2000 - 0.2) * Links.ShootAnimSpeed), Angle);
            if (IsRealBattle)
                BattleController.ShieldFlashStarted(TimeSpan.FromSeconds((RealLength / 2000 - 0.2) * Links.ShootAnimSpeed), Shoots(), WaveDelay(), Angle, ShieldMode);
            else
                HelpImage.ShowWeaponCanvas.ShieldFlashStarted(TimeSpan.FromSeconds((RealLength / 2000 - 0.2) * Links.ShootAnimSpeed), Shoots(), WaveDelay(), Angle);
        }
        static LinearGradientBrush GetBrush()
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0, 0.5); brush.EndPoint = new Point(1, 0.5);
            brush.GradientStops.Add(new GradientStop(Colors.Blue, 0.3));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Blue, 0.7));
            return brush;
        }
        public static int Shoots()
        {
            return 1;
        }
        public static TimeSpan WaveDelay()
        {
            return Links.ZeroTime;
        }
    }
}
