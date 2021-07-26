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
    class GaussAnim : ShootAnim
    {
        public static byte ShieldMode = 1;
        Path BeamLeft;
        Path BeamRight;
        static RadialGradientBrush corebrush = GetCoreBrush();
        public GaussAnim(Point firstPoint, Point secondPoint, bool IsMiss)
            : base(firstPoint, secondPoint, IsMiss)
        {
            CreateCanvas(78);
            if (Links.ShootAnimSpeed == 1)
                new MySound("Battle/Gauss.mp3");
            //Links.Controller.mainWindow.PlaySound("Gauss.wav");
            //Sounds.ShootSound(EWeaponType.Gauss);
            int steps = (int)(Length / 150) + 1;
            PathGeometry left = new PathGeometry();
            PathGeometry right = new PathGeometry();
            PathFigure ll = new PathFigure();
            left.Figures.Add(ll);
            PathFigure rr = new PathFigure();
            right.Figures.Add(rr);
            BeamLeft = new Path();
            BeamRight = new Path();
            PathCanvas.Children.Add(BeamLeft);
            PathCanvas.Children.Add(BeamRight);
            BeamLeft.Data = left;
            BeamRight.Data = right;
            BeamLeft.StrokeThickness = 5;
            BeamRight.StrokeThickness = 5;
            ll.StartPoint = new Point(18, 0);
            rr.StartPoint = new Point(60, 0);
            for (int i = 0; i < steps; i++)
            {
                int y = i * 150 + 75;
                ll.Segments.Add(new BezierSegment(new Point(63, y), new Point(-27, y), new Point(18, y + 75), true));
                rr.Segments.Add(new BezierSegment(new Point(15, y), new Point(105, y), new Point(60, y + 75), true));
            }
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0, 0.5); brush.EndPoint = new Point(1, 0.5);
            brush.GradientStops.Add(new GradientStop(Colors.Red, 0.3));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Red, 0.7));
            BeamLeft.Stroke = brush;
            BeamRight.Stroke = brush;

            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(Links.ShootAnimSpeed));
            BeamLeft.BeginAnimation(Path.OpacityProperty, anim);
            anim.Completed += new EventHandler(anim_Completed);
            BeamRight.BeginAnimation(Path.OpacityProperty, anim);
            if (IsRealBattle)
                BattleController.ShieldFlashStarted(TimeSpan.FromSeconds((RealLength / 2000 - 0.2) * Links.ShootAnimSpeed + Links.ShootAnimSpeed), Shoots(), WaveDelay(), Angle, ShieldMode);
            else
                HelpImage.ShowWeaponCanvas.ShieldFlashStarted(TimeSpan.FromSeconds((RealLength / 2000 - 0.2) * Links.ShootAnimSpeed + Links.ShootAnimSpeed), Shoots(), WaveDelay(), Angle);
        }
        public static int Shoots()
        {
            return 1;
        }
        public static TimeSpan WaveDelay()
        {
            return Links.ZeroTime;
        }
        void anim_Completed(object sender, EventArgs e)
        {
            Path core = new Path();
            core.Fill = corebrush;
            core.Stroke = Brushes.Black;
            core.StrokeLineJoin = PenLineJoin.Round;
            core.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M39,60 A15,20 0 0,1 49,40 A30,30 0 0,1 54,0 A30,30 0 0,1 24,0 A30,30 0 0,1 29,40 A15,20 0 0,1 39,60"));
            PathCanvas.Children.Add(core);
            Canvas.SetTop(core, -60);

            DoubleAnimation anim2 = new DoubleAnimation(-60, Length + 60, TimeSpan.FromSeconds(Length / 2000*Links.ShootAnimSpeed));
            anim2.Completed += new EventHandler(anim2_Completed);
            core.BeginAnimation(Canvas.TopProperty, anim2);
        }
        static RadialGradientBrush GetCoreBrush()
        {
            RadialGradientBrush corebrush = new RadialGradientBrush();
            corebrush.GradientOrigin = new Point(0.8, 0.2);
            corebrush.GradientStops.Add(new GradientStop(Colors.Gray, 1));
            corebrush.GradientStops.Add(new GradientStop(Colors.White, 0.2));
            return corebrush;
        }
        void anim2_Completed(object sender, EventArgs e)
        {
            DoubleAnimation anim3 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5 * Links.ShootAnimSpeed));
            anim3.Name = "Anim3";
            BeamRight.BeginAnimation(Path.OpacityProperty, anim3);
            anim3.Completed += new EventHandler(Anim_Completed);
            BeamLeft.BeginAnimation(Path.OpacityProperty, anim3);
        }
    }
}
