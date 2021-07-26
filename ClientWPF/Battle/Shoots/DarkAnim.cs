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
    class DarkAnim : ShootAnim
    {
        public static byte ShieldMode = 1;
        LinearGradientBrush brush;
        DispatcherTimer Timer;
        int curshoot;
        double[] lefts = new double[] { 20, 30, 10, 40, 15, 35, 50 };
        public DarkAnim(Point firstPoint, Point secondPoint, bool IsMiss)
            : base(firstPoint, secondPoint, IsMiss)
        {
            CreateCanvas(60);
            if (Links.ShootAnimSpeed == 1)
                new MySound("Battle/Dark.mp3");
            //Links.Controller.mainWindow.PlaySound("Dark.wav");
            //Sounds.ShootSound(EWeaponType.Dark);
            //System.Media.SoundPlayer player = new System.Media.SoundPlayer("C://123//dark.wav");
            //player.Play();
            //Shoots = 7;
            brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0, 0.5);
            brush.EndPoint = new Point(1, 0.5);
            brush.GradientStops.Add(new GradientStop(Colors.Purple, 0.7));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Purple, 0.3));
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(Links.ShootAnimSpeed / 6);
            MakeShoot();
            Timer.Tick += new EventHandler(Timer_Tick);
            if (IsRealBattle)
                BattleController.ShieldFlashStarted(TimeSpan.FromSeconds(Links.ShootAnimSpeed/12), Shoots(), WaveDelay(), Angle, ShieldMode);
            else
                HelpImage.ShowWeaponCanvas.ShieldFlashStarted(TimeSpan.FromSeconds(Links.ShootAnimSpeed / 12), Shoots(), WaveDelay(), Angle);
            Timer.Start();
        }
        public static int Shoots()
        {
            return 7;
        }
        public static TimeSpan WaveDelay()
        {
            TimeSpan ts = TimeSpan.FromSeconds(Links.ShootAnimSpeed / 7);
            return ts;
        }
        void Timer_Tick(object sender, EventArgs e)
        {
            if (curshoot == Shoots()) { Timer.Stop(); return; }
            MakeShoot();
        }
        void MakeShoot()
        {
            Rectangle rect = GetBeamRect(lefts[curshoot]);
            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(Links.ShootAnimSpeed / 3));
            anim.AutoReverse = true;
            if (curshoot == Shoots()) anim.Completed += new EventHandler(Anim_Completed);
            rect.BeginAnimation(Rectangle.OpacityProperty, anim);
            curshoot++;
        }
        Rectangle GetBeamRect(double left)
        {
            Rectangle rect = new Rectangle();
            rect.Width = 7;
            rect.Height = Length;
            rect.Fill = brush;
            PathCanvas.Children.Add(rect);
            Canvas.SetLeft(rect, left);
            return rect;
        }
    }
}
