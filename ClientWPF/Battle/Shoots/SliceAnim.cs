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
    class SliceAnim : ShootAnim
    {
        public static byte ShieldMode = 1;
        public SliceAnim(Point firstPoint, Point secondPoint, bool IsMiss)
            : base(firstPoint, secondPoint, IsMiss)
        {
            CreateCanvas(69);
            if (Links.ShootAnimSpeed == 1)
                new MySound("Battle/Slice.mp3");
            //Links.Controller.mainWindow.PlaySound("Slice.wav");
            //Sounds.ShootSound(EWeaponType.Slicing);
            Label lbl = GetSliceLabel();
            PathCanvas.Children.Add(lbl);
            Canvas.SetTop(lbl, -1000);
            DoubleAnimation anim = new DoubleAnimation(-1740, Length, TimeSpan.FromSeconds(Length / 700 * Links.ShootAnimSpeed));
            anim.Completed += new EventHandler(Anim_Completed);
            lbl.BeginAnimation(Canvas.TopProperty, anim);
            if (IsRealBattle)
                BattleController.ShieldFlashStarted(TimeSpan.FromSeconds((RealLength - 500) / 1500 * Links.ShootAnimSpeed), Shoots(), WaveDelay(), Angle, ShieldMode);
            else
                HelpImage.ShowWeaponCanvas.ShieldFlashStarted(TimeSpan.FromSeconds((RealLength - 500) / 1500 * Links.ShootAnimSpeed), Shoots(), WaveDelay(), Angle);
        }
        public static int Shoots()
        {
            return 10;
        }
        public static TimeSpan WaveDelay()
        {
            return TimeSpan.FromSeconds(Links.ShootAnimSpeed / 10);
        }
        Label GetSliceLabel()
        {
            Label lbl = new Label();
            LinearGradientBrush front = new LinearGradientBrush();
            front.StartPoint = new Point(0, 0.5); front.EndPoint = new Point(1, 0.5);
            front.GradientStops.Add(new GradientStop(Colors.Green, 0));
            front.GradientStops.Add(new GradientStop(Colors.LightGreen, 0.5));
            front.GradientStops.Add(new GradientStop(Colors.Green, 1));
            lbl.Foreground = front;
            lbl.FontSize = 30;
            lbl.FontWeight = FontWeights.Bold;
            Random rnd = new Random();
            byte[] array = new byte[40];
            for (int i = 0; i < array.Length; i++)
                array[i] = (byte)rnd.Next(16, 256);
            string result = "";
            for (int i = 0; i < array.Length; i++)
                result += String.Format("{0:X}\n", array[i]);
            lbl.Content = result;
            return lbl;
        }
    }
}
