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
    class AntiAnim : ShootAnim
    {
        public static byte ShieldMode = 1;
        static PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
        public AntiAnim(Point firstPoint, Point secondPoint, bool IsMiss)
            : base(firstPoint, secondPoint, IsMiss)
        {
            CreateCanvas(80);
            if (Links.ShootAnimSpeed == 1)
                new MySound("Battle/Anti.mp3");
            Canvas InnerCanvas = new Canvas();
            InnerCanvas.Width = 80; InnerCanvas.Height = 80;
            PathCanvas.Children.Add(InnerCanvas);
            Ellipse core = new Ellipse();
            core.Width = 60; core.Height = 60;
            InnerCanvas.Children.Add(core);
            Canvas.SetLeft(core, 15);
            Canvas.SetTop(core, 15);
            RadialGradientBrush corebrush = new RadialGradientBrush();
            corebrush.GradientOrigin = new Point(0.8, 0.2);
            corebrush.GradientStops.Add(new GradientStop(Colors.Red, 1));
            corebrush.GradientStops.Add(new GradientStop(Colors.White, 0));
            core.Fill = corebrush;
            Path belt = new Path();
            belt.Data = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M39,9  a36,36 0 0,1 12,0 a6,6 0 0,1 0,12 a13.5,13.5 0 0,0 18,18 a6,6 0 0,1 12,0" +
                                      "a36,36 0 0,1 0,12 a6,6 0 0,1 -12,0 a13.5,13.5 0 0,0 -18,18 a6,6 0 0,1 0,12" +
                                      "a36,36 0 0,1 -12,0 a6,6 0 0,1 0,-12 a13.5,13.5 0 0,0 -18,-18 a6,6 0 0,1 -12,0" +
                                      "a36,36 0 0,1 0,-12 a6,6 0 0,1 12,0 a13.5,13.5 0 0,0 18,-18 a6,6 0 0,1 0,-12"));
            LinearGradientBrush beltbrush = new LinearGradientBrush();
            beltbrush.GradientStops.Add(new GradientStop(Colors.Gray, 0.7));
            beltbrush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            beltbrush.GradientStops.Add(new GradientStop(Colors.Gray, 0.3));
            belt.Fill = beltbrush;
            InnerCanvas.Children.Add(belt);
            Canvas.SetTop(InnerCanvas, -90);

            TimeSpan duration = TimeSpan.FromSeconds((Length / 1000) * Links.ShootAnimSpeed);
            DoubleAnimation anim = new DoubleAnimation(-100, Length + 100, duration);
            DoubleAnimation beltanim = new DoubleAnimation(1, 0, duration);
            beltanim.DecelerationRatio = 1;
            belt.BeginAnimation(Path.OpacityProperty, beltanim);
            anim.Completed += new EventHandler(Anim_Completed);
            InnerCanvas.BeginAnimation(Canvas.TopProperty, anim);
            //BattleController.FireAnnimationStarted(TimeSpan.FromSeconds((RealLength / 1000 - 0.2) * Links.ShootAnimSpeed), Angle);
            if (IsRealBattle)
                BattleController.ShieldFlashStarted(TimeSpan.FromSeconds((RealLength / 1000 - 0.2) * Links.ShootAnimSpeed), Shoots(), WaveDelay(), Angle, ShieldMode);
            else
                HelpImage.ShowWeaponCanvas.ShieldFlashStarted(TimeSpan.FromSeconds((RealLength / 1000 - 0.2) * Links.ShootAnimSpeed), Shoots(), WaveDelay(), Angle);
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
