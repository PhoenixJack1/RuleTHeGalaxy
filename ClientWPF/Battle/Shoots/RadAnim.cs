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
    class RadAnim : ShootAnim
    {
        public static byte ShieldMode = 1;
        int parts;
        LinearGradientBrush brush;
        int[,] array;
        Random rnd = new Random();
        Path[] Pathes;
        DispatcherTimer Timer;
        int curshoot = 0;
        public RadAnim(Point firstPoint, Point secondPoint, bool IsMiss)
            : base(firstPoint, secondPoint, IsMiss)
        {
            CreateCanvas(60);
            //System.Media.SoundPlayer player = new System.Media.SoundPlayer("C://123//rad.wav");
            //player.Play();
            if (Links.ShootAnimSpeed == 1)
                new MySound("Battle/Radiation.mp3");
            //Links.Controller.mainWindow.PlaySound("Radiation.wav");
            parts = (int)Math.Round(Length / 45, 0);
            if (Length / 45 > parts) parts += 1;
            brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0, 0.5);
            brush.EndPoint = new Point(1, 0.5);
            brush.GradientStops.Add(new GradientStop(Colors.Green, 0.8));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Green, 0.2));

            array = new int[parts, 5];
            Pathes = new Path[5];
            for (int j = 0; j < 5; j++)
            {
                int last = 0;
                int val = 0;
                for (int i = 0; i < parts; i++)
                {
                    if (last == 3) { array[i, j] = 2; last = 2; continue; }
                    else if (last == -3) { array[i, j] = -2; last = -2; continue; }
                    val = rnd.Next(2);
                    val = val == 0 ? -1 : 1;
                    last = last + val;
                    array[i, j] = last;
                }
                Pathes[j] = new Path();
                Pathes[j].Stroke = brush;
                Pathes[j].StrokeThickness = 5;
                PathFigure figure = new PathFigure();
                Point LastPoint = new Point(30, 0);
                figure.StartPoint = LastPoint;
                for (int i = 0; i < parts; i++)
                {
                    Point NewPoint = new Point(30 + array[i, j] * 10, (i + 1) * 45);
                    figure.Segments.Add(new LineSegment(NewPoint, true));
                    LastPoint = NewPoint;
                }
                PathGeometry geom = new PathGeometry(); geom.Figures.Add(figure);
                Pathes[j].Data = geom;
            }
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(Links.ShootAnimSpeed / 4);
            MakeShoot();
            Timer.Tick += new EventHandler(Timer_Tick);
            if (IsRealBattle)
                BattleController.ShieldFlashStarted(TimeSpan.FromSeconds(0), Shoots(), WaveDelay(), Angle, ShieldMode);
            else
                HelpImage.ShowWeaponCanvas.ShieldFlashStarted(TimeSpan.FromSeconds(0), Shoots(), WaveDelay(), Angle);
            Timer.Start();

        }
        public static int Shoots()
        {
            return 5;
        }
        public static TimeSpan WaveDelay()
        {
            return TimeSpan.FromSeconds(Links.ShootAnimSpeed / 5);
        }
        void MakeShoot()
        {
            PathCanvas.Children.Add(Pathes[curshoot]);
            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(Links.ShootAnimSpeed / 2));
            if (curshoot == 4) anim.Completed += new EventHandler(Anim_Completed);
            anim.AutoReverse = true;
            Pathes[curshoot].BeginAnimation(Path.OpacityProperty, anim);
            curshoot++;
        }
        void Timer_Tick(object sender, EventArgs e)
        {
            if (curshoot == 4) Timer.Stop();
            MakeShoot();
        }
    }
}
