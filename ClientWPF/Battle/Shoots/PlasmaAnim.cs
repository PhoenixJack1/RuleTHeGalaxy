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
    class PlasmaAnim : ShootAnim
    {
        public static byte ShieldMode = 1;
        public new static bool HaveSelfSparks = true;
        public bool InHull = false;
        public bool IsMiss = false;
        public bool HasCrit = false;
        public Canvas BulletCanvas;
        public PlasmaAnim(Point firstPoint, Point secondPoint, bool IsMiss, bool inhull, bool hascrit)
            : base(firstPoint, secondPoint, IsMiss)
        {
            //Shoots = 10;

            InHull = inhull;
            this.IsMiss = IsMiss;
            HasCrit = hascrit;
            CreateCanvas(500);
            //PathCanvas.Background = Brushes.White;
            BulletCanvas = new Canvas(); BulletCanvas.Width = PathCanvas.Width; BulletCanvas.Height = PathCanvas.Height;
            PathCanvas.Children.Add(BulletCanvas); PathCanvas.ClipToBounds = false; BulletCanvas.ClipToBounds = true;
            if (Links.ShootAnimSpeed == 1)
                new MySound("Battle/Plasma.mp3");
            new MakeFlash(PathCanvas, this);

            if (IsRealBattle)
                BattleController.ShieldFlashStarted(TimeSpan.FromSeconds((RealLength / 1500 - 0.6) * Links.ShootAnimSpeed), Shoots(), WaveDelay(), Angle, 1);
            else
                HelpImage.ShowWeaponCanvas.ShieldFlashStarted(TimeSpan.FromSeconds((RealLength / 1500 - 0.6) * Links.ShootAnimSpeed), Shoots(), WaveDelay(), Angle);
        }
        public void ShootCompleted()
        {
            if (IsMiss == false && InHull == true)
                new MakeHullSpark(Length, this);
            if (IsMiss == false && InHull == false)
                new MakeShieldSpark(Length, this);
            new CritFlash(this, 0, (int)(Length - 250), HasCrit);

        }
        public void ShootFinished()
        {
            Anim_Completed(null, null);
        }
        public void FlashCompleted()
        {
            new MakeShoot(Length, this);

        }

        public static int Shoots()
        {
            return 1;
        }
        public static TimeSpan WaveDelay()
        {
            return TimeSpan.FromSeconds(Links.ShootAnimSpeed / 2);
        }

        class MakeFlash
        {
            public ImageBrush Fire = new ImageBrush((new BitmapImage(new Uri("Images/Shoots/Plasma/plasma_shot.png", UriKind.Relative))));
            Rectangle flash;
            Canvas Shoot;
            PlasmaAnim CurPlasma;
            public MakeFlash(Canvas shoot, PlasmaAnim cannon)
            {
                Shoot = shoot;
                CurPlasma = cannon;
                flash = new Rectangle(); flash.Width = 500; flash.Height = 500;
                flash.Fill = Fire;
                flash.RenderTransformOrigin = new Point(0.5, 0.5);
                flash.RenderTransform = new RotateTransform(90);
                RectAnimationUsingKeyFrames anim = new RectAnimationUsingKeyFrames();
                anim.Duration = TimeSpan.FromSeconds(0.5 / 12 * 5 * Links.ShootAnimSpeed);
                for (int i = 0; i < 5; i++)
                {
                    anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect((i % 10) / 10.0, (i / 10) * 0.5, 0.1, 0.5), KeyTime.FromPercent(i / 5.0)));
                }
                anim.Completed += Anim_Completed;
                Fire.BeginAnimation(ImageBrush.ViewboxProperty, anim);
                shoot.Children.Add(flash); Canvas.SetLeft(flash, 0); Canvas.SetTop(flash, -150);
            }
            private void Anim_Completed(object sender, EventArgs e)
            {
                CurPlasma.FlashCompleted();
                RectAnimationUsingKeyFrames anim = new RectAnimationUsingKeyFrames();
                anim.Duration = TimeSpan.FromSeconds(0.5 / 12 * 7 * Links.ShootAnimSpeed);
                for (int i = 5; i < 12; i++)
                {
                    anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect((i % 10) / 10.0, (i / 10) * 0.5, 0.1, 0.5), KeyTime.FromPercent((i - 5) / 7.0)));
                }
                anim.Completed += Anim_Completed1;
                Fire.BeginAnimation(ImageBrush.ViewboxProperty, anim);
            }
            private void Anim_Completed1(object sender, EventArgs e)
            {
                Shoot.Children.Remove(flash);
            }
        }
        public class CritFlash : Canvas
        {
            public ImageBrush Image = new ImageBrush((new BitmapImage(new Uri("Images/Shoots/Plasma/plasma_crit.png", UriKind.Relative))));
            PlasmaAnim CurPlasma;
            public RectAnimationUsingKeyFrames anim;
            public CritFlash(PlasmaAnim plasma, int left, int top, bool show)
            {
                CurPlasma = plasma;
                Width = 500; Height = 500;
                if (show) Background = Image;
                plasma.PathCanvas.Children.Add(this); Canvas.SetLeft(this, left); Canvas.SetTop(this, top);
                anim = new RectAnimationUsingKeyFrames();
                anim.Duration = TimeSpan.FromSeconds(2.0 * Links.ShootAnimSpeed);
                for (int i = 0; i < 33; i++)
                {
                    Rect rect = new Rect(i % 10 * 0.1, (i / 10) / 4.0, 0.1, 1 / 4.0);
                    anim.KeyFrames.Add(new DiscreteRectKeyFrame(rect, KeyTime.FromPercent(i / 32.0)));
                }
                anim.Completed += Anim_Completed1;
                Image.BeginAnimation(ImageBrush.ViewboxProperty, anim);
            }

            private void Anim_Completed1(object sender, EventArgs e)
            {
                CurPlasma.ShootFinished();
            }
        }
        class MakeShoot
        {
            public ImageBrush Image = new ImageBrush((new BitmapImage(new Uri("Images/Shoots/Plasma/plasma.png", UriKind.Relative))));
            Rectangle flash;
            double speed = 1500;
            PlasmaAnim CurPlasma;
            double Seconds1;
            double Seconds2;
            double Length;
            int flashdistance = 100;
            public MakeShoot(double length, PlasmaAnim plasma)
            {
                CurPlasma = plasma;
                Length = length;
                flash = new Rectangle(); flash.Width = 250; flash.Height = 250;
                flash.Fill = Image;
                flash.RenderTransformOrigin = new Point(0.5, 0.5);
                flash.RenderTransform = new RotateTransform(90);
                plasma.BulletCanvas.Children.Add(flash); Canvas.SetLeft(flash, 125); Canvas.SetTop(flash, -250);
                Seconds1 = (length - flashdistance) / speed * Links.ShootAnimSpeed;
                if (Seconds1 < 0) Seconds1 = 0.1;
                Seconds2 = flashdistance / speed * Links.ShootAnimSpeed;
                RectAnimationUsingKeyFrames imageanim = new RectAnimationUsingKeyFrames();
                imageanim.Duration = TimeSpan.FromSeconds(0.25 * Links.ShootAnimSpeed);
                for (int i = 0; i < 13; i++)
                {
                    Rect rect = new Rect(i % 10 * 0.1, (i / 10) / 2.0, 0.1, 1 / 2.0);
                    imageanim.KeyFrames.Add(new DiscreteRectKeyFrame(rect, KeyTime.FromPercent(i / 12.0)));
                }
                imageanim.RepeatBehavior = RepeatBehavior.Forever;
                Image.BeginAnimation(ImageBrush.ViewboxProperty, imageanim);
                DoubleAnimation anim = new DoubleAnimation(-250, length - flashdistance, TimeSpan.FromSeconds(Seconds1));

                //DoubleAnimation anim = new DoubleAnimation(-450, length-500, TimeSpan.FromSeconds((length / speed)*Links.ShootAnimSpeed));
                anim.Completed += Anim_Completed0;
                flash.BeginAnimation(Canvas.TopProperty, anim);
            }

            private void Anim_Completed0(object sender, EventArgs e)
            {
                CurPlasma.ShootCompleted();
                DoubleAnimation anim = new DoubleAnimation(Length - flashdistance, Length, TimeSpan.FromSeconds(Seconds2));
                anim.Completed += Anim_Completed;
                flash.BeginAnimation(Canvas.TopProperty, anim);
            }
            private void Anim_Completed(object sender, EventArgs e)
            {
                CurPlasma.BulletCanvas.Children.Remove(flash);

            }

        }

        class MakeHullSpark
        {
            public ImageBrush Image = new ImageBrush((new BitmapImage(new Uri("Images/Shoots/Common/FlashBlue.png", UriKind.Relative))));
            Rectangle flash;
            PlasmaAnim CurPlasma;
            public MakeHullSpark(double Length, PlasmaAnim plasma)
            {
                CurPlasma = plasma;
                flash = new Rectangle(); flash.Width = 425; flash.Height = 335;
                flash.Fill = Image;
                RectAnimationUsingKeyFrames anim = new RectAnimationUsingKeyFrames();
                anim.Duration = TimeSpan.FromSeconds(0.1 * Links.ShootAnimSpeed);
                anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.0, 0, 0.1, 1), KeyTime.FromPercent(0)));
                anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.1, 0, 0.1, 1), KeyTime.FromPercent(0.2)));
                anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.2, 0, 0.1, 1), KeyTime.FromPercent(0.4)));
                anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.3, 0, 0.1, 1), KeyTime.FromPercent(0.6)));
                anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.5, 0, 0.1, 1), KeyTime.FromPercent(0.8)));
                anim.Completed += Anim_Completed;
                Image.BeginAnimation(ImageBrush.ViewboxProperty, anim);
                plasma.PathCanvas.Children.Add(flash); Canvas.SetLeft(flash, 30); Canvas.SetTop(flash, Length - 170);
            }

            private void Anim_Completed(object sender, EventArgs e)
            {
                CurPlasma.PathCanvas.Children.Remove(flash);
                //CurPlasma.ShootFinished();
            }
        }
        class MakeShieldSpark
        {
            public ImageBrush Image = new ImageBrush((new BitmapImage(new Uri("Images/Shoots/Common/FlashBlue.png", UriKind.Relative))));
            Rectangle flash;
            PlasmaAnim CurPlasma;
            public MakeShieldSpark(double Length, PlasmaAnim plasma)
            {
                CurPlasma = plasma;
                flash = new Rectangle(); flash.Width = 425; flash.Height = 335;
                flash.Fill = Image;
                RectAnimationUsingKeyFrames anim = new RectAnimationUsingKeyFrames();
                anim.Duration = TimeSpan.FromSeconds(0.1 * Links.ShootAnimSpeed);
                anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.0, 0, 0.1, 1), KeyTime.FromPercent(0)));
                anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.1, 0, 0.1, 1), KeyTime.FromPercent(0.2)));
                anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.2, 0, 0.1, 1), KeyTime.FromPercent(0.4)));
                anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.3, 0, 0.1, 1), KeyTime.FromPercent(0.6)));
                anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.5, 0, 0.1, 1), KeyTime.FromPercent(0.8)));
                anim.Completed += Anim_Completed;
                Image.BeginAnimation(ImageBrush.ViewboxProperty, anim);
                plasma.PathCanvas.Children.Add(flash); Canvas.SetLeft(flash, 30); Canvas.SetTop(flash, Length - 300);
            }

            private void Anim_Completed(object sender, EventArgs e)
            {
                CurPlasma.PathCanvas.Children.Remove(flash);
                //if (Last) CurCannon.ShootFinished();
            }
        }
    }
}
