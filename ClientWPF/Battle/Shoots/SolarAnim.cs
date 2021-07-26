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
    class SolarAnim : ShootAnim
    {
        public static byte ShieldMode = 1;
        ScaleTransform transform;
        public SolarAnim(Point firstPoint, Point secondPoint, bool IsMiss)
            : base(firstPoint, secondPoint, IsMiss)
        {
            CreateCanvas(40);
            if (Links.ShootAnimSpeed == 1)
                new MySound("Battle/Solar.mp3");
            //Links.Controller.mainWindow.PlaySound("Solar.wav");
            //Sounds.ShootSound(EWeaponType.Solar);
            Rectangle beam = new Rectangle();
            beam.Height = Length;
            beam.Width = 30;
            LinearGradientBrush beambrush = new LinearGradientBrush();
            beambrush.StartPoint = new Point(0, 0.5);
            beambrush.EndPoint = new Point(1, 0.5);
            beambrush.GradientStops.Add(new GradientStop(Color.FromArgb(0,255,255,0), 1));
            beambrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.5));
            beambrush.GradientStops.Add(new GradientStop(Color.FromArgb(0,255,255,0), 0));
            beam.Fill = beambrush;
            Canvas.SetLeft(beam, 5.0);
            beam.RadiusX = 20;
            beam.RadiusY = 20;
            PathCanvas.Children.Add(beam);
            transform = new ScaleTransform(0, 1);
            beam.RenderTransformOrigin = new Point(0.5, 0.5);
            beam.RenderTransform = transform;

            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(Links.ShootAnimSpeed));
            anim.Completed += new EventHandler(anim_Completed);
            transform.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
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
            Ellipse el = new Ellipse();
            RadialGradientBrush elbrush = new RadialGradientBrush();
            elbrush.GradientOrigin = new Point(0.7, 0.3);
            elbrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.21));
            elbrush.GradientStops.Add(new GradientStop(Colors.White, 0.2));
            
            el.Fill = elbrush;

            el.Width = 40;
            el.Height = 100;
            PathCanvas.Children.Add(el);
            Canvas.SetTop(el, -100);

            DoubleAnimation anim2 = new DoubleAnimation(-100, Length + 100, TimeSpan.FromSeconds(Length / 2000*Links.ShootAnimSpeed));
            anim2.Completed += new EventHandler(anim2_Completed);
            el.BeginAnimation(Canvas.TopProperty, anim2);
        }
        void anim2_Completed(object sender, EventArgs e)
        {
            DoubleAnimation anim3 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5 * Links.ShootAnimSpeed));
            anim3.Completed += new EventHandler(Anim_Completed);
            transform.BeginAnimation(ScaleTransform.ScaleXProperty, anim3);
        }
    }
}

