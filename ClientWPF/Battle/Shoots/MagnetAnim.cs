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
    class MagnetAnim : ShootAnim
    {
        public static byte ShieldMode = 1;
        ScaleTransform bluetransform, redtransform;
        LinearGradientBrush bluebrush, redbrush;
        Rectangle red, blue;
        public MagnetAnim(Point firstPoint, Point secondPoint, bool IsMiss)
            : base(firstPoint, secondPoint, IsMiss)
        {
            CreateCanvas(40);
            if (Links.ShootAnimSpeed == 1)
                new MySound("Battle/Magnet.mp3");
            //Links.Controller.mainWindow.PlaySound("Magnet.wav");
            bluebrush = new LinearGradientBrush();
            bluebrush.StartPoint = new Point(0, 0.5);
            bluebrush.EndPoint = new Point(1, 0.5);
            bluebrush.GradientStops.Add(new GradientStop(Colors.Blue, 0.5));
            bluebrush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            bluebrush.GradientStops.Add(new GradientStop(Colors.Blue, 0.5));
            redbrush = new LinearGradientBrush();
            redbrush.StartPoint = new Point(0, 0.5);
            redbrush.EndPoint = new Point(1, 0.5);
            redbrush.GradientStops.Add(new GradientStop(Colors.Red, 0.5));
            redbrush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            redbrush.GradientStops.Add(new GradientStop(Colors.Red, 0.5));
            blue = new Rectangle();
            blue.Width = 20; blue.Height = Length;
            blue.RadiusX = 15; blue.RadiusY = 15;
            PathCanvas.Children.Add(blue);
            blue.Fill = bluebrush;
            red = new Rectangle();
            red.Width = 20; red.Height = Length;
            red.RadiusY = 15; red.RadiusX = 15;
            PathCanvas.Children.Add(red);
            red.Fill = redbrush;
            Canvas.SetLeft(red, 20);
            bluetransform = new ScaleTransform(0, 1);
            blue.RenderTransformOrigin = new Point(0.5, 0.5);
            blue.RenderTransform = bluetransform;
            redtransform = new ScaleTransform(0, 1);
            red.RenderTransformOrigin = new Point(0.5, 0.5);
            red.RenderTransform = redtransform;
            if (IsRealBattle)
                BattleController.ShieldFlashStarted(TimeSpan.FromSeconds(Links.ShootAnimSpeed / 3), Shoots(), WaveDelay(), Angle, ShieldMode);
            else
                HelpImage.ShowWeaponCanvas.ShieldFlashStarted(TimeSpan.FromSeconds(Links.ShootAnimSpeed / 3), Shoots(), WaveDelay(), Angle);
            DoubleAnimation anim1 = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(Links.ShootAnimSpeed / 3));
            anim1.Completed += new EventHandler(anim1_Completed);
            redtransform.BeginAnimation(ScaleTransform.ScaleXProperty, anim1);
        }
        public static int Shoots()
        {
            return 2;
        }
        public static TimeSpan WaveDelay()
        {
            return TimeSpan.FromSeconds(Links.ShootAnimSpeed / 4);
        }
        void anim1_Completed(object sender, EventArgs e)
        {
            DoubleAnimation anim2 = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(Links.ShootAnimSpeed / 3));
            anim2.Completed += new EventHandler(anim2_Completed);
            bluetransform.BeginAnimation(ScaleTransform.ScaleXProperty, anim2);
        }

        void anim2_Completed(object sender, EventArgs e)
        {
            DoubleAnimation anim3 = new DoubleAnimation(0.5, 1, TimeSpan.FromSeconds(Links.ShootAnimSpeed / 4));
            redbrush.GradientStops[2].BeginAnimation(GradientStop.OffsetProperty, anim3);
            DoubleAnimation anim4 = new DoubleAnimation(0.5, 0, TimeSpan.FromSeconds(Links.ShootAnimSpeed / 4));
            anim4.Completed += new EventHandler(anim4_Completed);
            redbrush.GradientStops[0].BeginAnimation(GradientStop.OffsetProperty, anim4);
        }

        void anim4_Completed(object sender, EventArgs e)
        {
            red.Opacity = 0;
            DoubleAnimation anim5 = new DoubleAnimation(0.5, 1, TimeSpan.FromSeconds(Links.ShootAnimSpeed / 4));
            bluebrush.GradientStops[2].BeginAnimation(GradientStop.OffsetProperty, anim5);
            DoubleAnimation anim6 = new DoubleAnimation(0.5, 0, TimeSpan.FromSeconds(Links.ShootAnimSpeed / 4));
            anim6.Completed += new EventHandler(Anim_Completed);
            bluebrush.GradientStops[0].BeginAnimation(GradientStop.OffsetProperty, anim6);
        }
    }
}
