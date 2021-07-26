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
    class WarpAnim : ShootAnim
    {
        public static byte ShieldMode = 1;
        static PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
        public WarpAnim(Point firstPoint, Point secondPoint, bool IsMiss)
            : base(firstPoint, secondPoint, IsMiss)
        {
            CreateCanvas(102);
            if (Links.ShootAnimSpeed == 1)
                new MySound("Battle/Warp.mp3");
            //Links.Controller.mainWindow.PlaySound("Warp.wav");
            //Sounds.ShootSound(EWeaponType.Warp);
            Path warp = new Path();
            warp.Data = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M51,0 A13.5,13.5 0 0,0 51,24 A33,33 0 0,1 102,51" +
                              "A13.5,13.5 0 0,0 78,51 A33,33 0 0,1 51,102" +
                              "A13.5,13.5 0 0,0 51,78 A33,33 0 0,1 0,51" +
                              "A13.5,13.5 0 0,0 24,51 A33,33 0 0,1 51,0"));
            RadialGradientBrush warpbrush = new RadialGradientBrush();
            warpbrush.GradientStops.Add(new GradientStop(Colors.Black, 0.35));
            warpbrush.GradientStops.Add(new GradientStop(Colors.Violet, 0.35));
            warpbrush.GradientStops.Add(new GradientStop(Colors.Purple, 0.7));
            warpbrush.GradientStops.Add(new GradientStop(Colors.Black, 1));
            warp.Fill = warpbrush;
            PathCanvas.Children.Add(warp);
            Canvas.SetTop(warp, 68);
            RotateTransform transform = new RotateTransform();
            warp.RenderTransform = transform;
            warp.RenderTransformOrigin = new Point(0.5, 0.5);
            TimeSpan duration = TimeSpan.FromSeconds((Length / 1500) * Links.ShootAnimSpeed);
            DoubleAnimation anim = new DoubleAnimation(-68, Length + 68, duration);
            DoubleAnimation rotateanim = new DoubleAnimation(0, 360, TimeSpan.FromSeconds(Links.ShootAnimSpeed / 3));
            rotateanim.RepeatBehavior = RepeatBehavior.Forever;
            transform.BeginAnimation(RotateTransform.AngleProperty, rotateanim);
            anim.Completed += new EventHandler(Anim_Completed);
            warp.BeginAnimation(Canvas.TopProperty, anim);
            //BattleController.FireAnnimationStarted(TimeSpan.FromSeconds((RealLength / 1500 - 0.2) * Links.ShootAnimSpeed), Angle);
            if (IsRealBattle)
                BattleController.ShieldFlashStarted(TimeSpan.FromSeconds((RealLength / 1500 - 0.2) * Links.ShootAnimSpeed), Shoots(), WaveDelay(), Angle, ShieldMode);
            else
                HelpImage.ShowWeaponCanvas.ShieldFlashStarted(TimeSpan.FromSeconds((RealLength / 1500 - 0.2) * Links.ShootAnimSpeed), Shoots(), WaveDelay(), Angle);
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

