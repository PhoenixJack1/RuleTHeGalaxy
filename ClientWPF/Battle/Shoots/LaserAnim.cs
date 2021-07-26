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
    class LaserAnim : ShootAnim
    {
        public static byte ShieldMode = 1;
        Rectangle laser;
        DoubleAnimation anim;
        static LinearGradientBrush brush = GetBrush();
        public LaserAnim(Point firstPoint, Point secondPoint, bool IsMiss)
            : base(firstPoint, secondPoint, IsMiss)
        {
            CreateCanvas(15);
            if (Links.ShootAnimSpeed == 1)
                new MySound("Battle/Laser.mp3");
            //Links.Controller.mainWindow.PlaySound("Laser.wav");
            laser = new Rectangle();
            laser.Width = 15;
            laser.Height = 100;
            PathCanvas.Children.Add(laser);
            laser.Fill = brush;
            laser.RadiusX = 15;
            laser.RadiusY = 15;
            Canvas.SetTop(laser, -100);
            TimeSpan duration = TimeSpan.FromSeconds((Length / 2000) * Links.ShootAnimSpeed);
            anim = new DoubleAnimation(-100, Length + 100, duration);
            
            anim.Completed += new EventHandler(Anim_Completed);
            laser.BeginAnimation(Canvas.TopProperty, anim);
            //BattleController.FireAnnimationStarted(TimeSpan.FromSeconds((RealLength / 2000 - 0.2) * Links.ShootAnimSpeed), Angle);
            if (IsRealBattle)
                BattleController.ShieldFlashStarted(TimeSpan.FromSeconds((RealLength / 2000 - 0.2) * Links.ShootAnimSpeed), Shoots(), WaveDelay(), Angle, ShieldMode);
            else
                HelpImage.ShowWeaponCanvas.ShieldFlashStarted(TimeSpan.FromSeconds((RealLength / 2000 - 0.2) * Links.ShootAnimSpeed), Shoots(), WaveDelay(), Angle);
        }
        static LinearGradientBrush GetBrush()
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Blue, 0));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Blue, 1));
            brush.StartPoint = new Point(0, 0.5); brush.EndPoint = new Point(1, 0.5);
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
