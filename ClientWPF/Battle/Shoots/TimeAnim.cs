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
    class TimeAnim : ShootAnim
    {
        public static byte ShieldMode = 1;
        static PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
        Random rnd = new Random(0);
        Point LastPoint = new Point(45, 0);
        LinearGradientBrush brush;
        int[] array;
        int parts;
        DispatcherTimer Timer;
        public TimeAnim(Point firstPoint, Point secondPoint, bool IsMiss)
            : base(firstPoint, secondPoint, IsMiss)
        {
            CreateCanvas(100);
            if (Links.ShootAnimSpeed == 1)
                new MySound("Battle/Time.mp3");
            //Links.Controller.mainWindow.PlaySound("Time.wav");
            //Sounds.ShootSound(EWeaponType.Time);
            //System.Media.SoundPlayer player = new System.Media.SoundPlayer("C://123//time.wav");
            //player.Play();
            parts = (int)Math.Round(Length / 90, 0);
            if (Length / 90 > parts) parts += 1;
            brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0, 0.5);
            brush.EndPoint = new Point(1, 0.5);
            brush.GradientStops.Add(new GradientStop(Colors.Purple, 0.8));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Purple, 0.2));

            array = new int[parts];
            int last = 0;
            int val = 0;
            for (int i = 0; i < parts; i++)
            {
                if (last == 3) { array[i] = 2; last = 2; continue; }
                else if (last == -3) { array[i] = -2; last = -2; continue; }
                val = rnd.Next(2);
                val = val == 0 ? -1 : 1;
                last = last + val;
                array[i] = last;
            }
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(Links.ShootAnimSpeed / 20);
            Timer.Tick += new EventHandler(Timer_Tick);
            AddLine();
            Timer.Start();
            //BattleController.FireAnnimationStarted(TimeSpan.FromSeconds(Links.ShootAnimSpeed / 20 * RealLength / 90), Angle);
            if (IsRealBattle)
                BattleController.ShieldFlashStarted(TimeSpan.FromSeconds(Links.ShootAnimSpeed / 20 * RealLength / 90), Shoots(), WaveDelay(), Angle, ShieldMode);
            else
                HelpImage.ShowWeaponCanvas.ShieldFlashStarted(TimeSpan.FromSeconds(Links.ShootAnimSpeed / 20 * RealLength / 90), Shoots(), WaveDelay(), Angle);
        }
        public static int Shoots()
        {
            return 1;
        }
        public static TimeSpan WaveDelay()
        {
            return Links.ZeroTime;
        }
        int curline = 0;
        int lastway = 0;
        void Timer_Tick(object sender, EventArgs e)
        {
            if (curline == parts - 1) Timer.Stop();
            AddLine();
        }
        void AddLine()
        {
            if (curline >= array.Length) return;
            GetLine(curline, array[curline], lastway);
            lastway = array[curline];
            curline++;
        }
        void GetLine(int pos, int way, int lastway)
        {
            Point NewPoint = new Point(75 - way * 25, (pos + 1) * 90);
            Line l = new Line();

            double length = Math.Sqrt(Math.Pow(NewPoint.X - LastPoint.X, 2) + Math.Pow(NewPoint.Y - LastPoint.Y, 2));
            double angle = Common.CalcAngle(LastPoint, NewPoint);
            l.Y2 = length;
            l.Stroke = brush;
            l.StrokeStartLineCap = PenLineCap.Round;
            l.StrokeEndLineCap = PenLineCap.Round;
            l.StrokeThickness = 15;
            PathCanvas.Children.Add(l);
            Canvas.SetLeft(l, LastPoint.X);
            Canvas.SetTop(l, LastPoint.Y);
            l.RenderTransformOrigin = new Point(0.5, 0);
            RotateTransform trans = new RotateTransform(angle - 180);
            l.RenderTransform = trans;
            LastPoint = NewPoint;
            DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(Links.ShootAnimSpeed / 2));
            if (pos == array.Length - 1)
                anim.Completed += new EventHandler(Anim_Completed);
            l.BeginAnimation(Line.OpacityProperty, anim);
        }


    }
}
