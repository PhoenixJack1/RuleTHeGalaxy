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
    class CannonAnim : ShootAnim
    {
        public static byte ShieldMode = 1;
        public new static bool  HaveSelfSparks = true;
        public bool InHull = false;
        public bool IsMiss = false;
        public Canvas BulletCanvas;
        public CannonAnim(Point firstPoint, Point secondPoint, bool IsMiss, bool inhull)
            : base(firstPoint, secondPoint, IsMiss)
        {
            InHull = inhull;
            this.IsMiss = IsMiss;
            CreateCanvas(200);
            //PathCanvas.Background = Brushes.White;
            BulletCanvas = new Canvas(); BulletCanvas.Width = PathCanvas.Width; BulletCanvas.Height = PathCanvas.Height;
            PathCanvas.Children.Add(BulletCanvas); PathCanvas.ClipToBounds = false; BulletCanvas.ClipToBounds = true;
            if (Links.ShootAnimSpeed == 1)
                new MySound("Battle/Cannon.mp3");

            MakeOneShoot(); 
            if (IsRealBattle)
                BattleController.ShieldFlashStarted(TimeSpan.FromSeconds((RealLength / 1500 - 0.6) * Links.ShootAnimSpeed), Shoots(), WaveDelay(), Angle, ShieldMode);
            else
                HelpImage.ShowWeaponCanvas.ShieldFlashStarted(TimeSpan.FromSeconds((RealLength / 1500 - 0.6) * Links.ShootAnimSpeed), Shoots(), WaveDelay(), Angle);
        }

        public void ShootCompleted(int curshoot)
        {
            new MakeHullSpark(Length, curshoot == Shoots() - 1, this, IsMiss == false && InHull);
            if (IsMiss == false && InHull == false)
                new MakeShieldSpark(Length, this);
        }
        public void ShootFinished()
        {
            Anim_Completed(null, null);
        }
        public void FlashCompleted()
        {
            curshoot++;
            if (curshoot < Shoots())
                MakeOneShoot();
        }
        public static int Shoots()
        {
            return 5;
        }
        void MakeOneShoot()
        {
            new MakeFlash(PathCanvas, this); //вспышка 
            new MakeShoot(Length, this, curshoot); //Полёт снаряда

        }
        public static TimeSpan WaveDelay()
        {
            return TimeSpan.FromSeconds(Links.ShootAnimSpeed / 4);
        }
        int curshoot = 0;
        public class CritFlash : Canvas
        {
            public ImageBrush Image = new ImageBrush(new BitmapImage(new Uri("Images/Shoots/Cannon/skeleton-target krit_-result.png", UriKind.Relative)));
            Canvas CurCanvas;
            public RectAnimationUsingKeyFrames anim;
            public CritFlash(Canvas canvas, Hex hex)
            {
                CurCanvas = canvas;
                Width = 678; Height = 452;
                Background = Image;
                canvas.Children.Add(this); Canvas.SetLeft(this, hex.CenterX - 678 / 2); Canvas.SetTop(this, hex.CenterY - 452 / 2 - 130);
                Canvas.SetZIndex(this, Links.ZIndex.Flashes);
                anim = new RectAnimationUsingKeyFrames();
                anim.Duration = TimeSpan.FromSeconds(2.0 * Links.ShootAnimSpeed);
                for (int i = 0; i < 91; i++)
                {
                    Rect rect = new Rect(i % 10 * 0.1, (i / 10) / 9.0, 0.1, 1 / 9.0);
                    anim.KeyFrames.Add(new DiscreteRectKeyFrame(rect, KeyTime.FromPercent(i / 90.0)));
                }
                anim.Completed += Anim_Completed1;
                Image.BeginAnimation(ImageBrush.ViewboxProperty, anim);
            }

            private void Anim_Completed1(object sender, EventArgs e)
            {
                CurCanvas.Children.Remove(this);
                BattleController.FireCannon();
            }
        }

    }
    class MakeFlash
    {
        public static ImageBrush Fire = new ImageBrush(new BitmapImage(new Uri("Images/Shoots/Cannon/skeleton-shot_-result2.png", UriKind.Relative)));
        Rectangle flash;
        Canvas Shoot;
        CannonAnim CurCannon;
        public MakeFlash(Canvas shoot, CannonAnim cannon)
        {
            Shoot = shoot;
            CurCannon = cannon;
            flash = new Rectangle(); flash.Width = 200; flash.Height = 250;
            flash.Fill = Fire;
            RectAnimationUsingKeyFrames anim = new RectAnimationUsingKeyFrames();
            anim.Duration = TimeSpan.FromSeconds(0.25 * Links.ShootAnimSpeed);
            anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.9, 0, 0.1, 1), KeyTime.FromPercent(0)));
            anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.8, 0, 0.1, 1), KeyTime.FromPercent(0.05)));
            anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.7, 0, 0.1, 1), KeyTime.FromPercent(0.1)));
            anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.6, 0, 0.1, 1), KeyTime.FromPercent(0.15)));
            anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.5, 0, 0.1, 1), KeyTime.FromPercent(0.25)));
            anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.4, 0, 0.1, 1), KeyTime.FromPercent(0.45)));
            anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.3, 0, 0.1, 1), KeyTime.FromPercent(0.65)));
            anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.2, 0, 0.1, 1), KeyTime.FromPercent(0.85)));
            anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.1, 0, 0.1, 1), KeyTime.FromPercent(1.0)));
            anim.Completed += Anim_Completed;
            Fire.BeginAnimation(ImageBrush.ViewboxProperty, anim);
            shoot.Children.Add(flash); Canvas.SetLeft(flash, 0); Canvas.SetTop(flash, 10);
        }

        private void Anim_Completed(object sender, EventArgs e)
        {
            Shoot.Children.Remove(flash);
            CurCannon.FlashCompleted();
        }
    }
    class MakeShieldSpark
    {
        public static ImageBrush Image = new ImageBrush(new BitmapImage(new Uri("Images/Shoots/Cannon/FlashBlue.png", UriKind.Relative)));
        Rectangle flash;
        CannonAnim CurCannon;
        public MakeShieldSpark(double Length, CannonAnim cannon)
        {
            CurCannon = cannon;
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
            cannon.PathCanvas.Children.Add(flash); Canvas.SetLeft(flash, -120); Canvas.SetTop(flash, Length - 300);
        }

        private void Anim_Completed(object sender, EventArgs e)
        {
            CurCannon.PathCanvas.Children.Remove(flash);
            //if (Last) CurCannon.ShootFinished();
        }
    }
    class MakeHullSpark
    {
        public static ImageBrush Image = new ImageBrush(new BitmapImage(new Uri("Images/Shoots/Cannon/FlashRed.png", UriKind.Relative)));
        Rectangle flash;
        CannonAnim CurCannon;
        bool Last;
        bool Show;
        public MakeHullSpark(double Length, bool last, CannonAnim cannon, bool show)
        {
            CurCannon = cannon;
            Last = last;
            Show = show;
            flash = new Rectangle(); flash.Width = 425; flash.Height = 335;
            if (Show) flash.Fill = Image;
            RectAnimationUsingKeyFrames anim = new RectAnimationUsingKeyFrames();
            anim.Duration = TimeSpan.FromSeconds(0.1 * Links.ShootAnimSpeed);
            anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.0, 0, 0.1, 1), KeyTime.FromPercent(0)));
            anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.1, 0, 0.1, 1), KeyTime.FromPercent(0.2)));
            anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.2, 0, 0.1, 1), KeyTime.FromPercent(0.4)));
            anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.3, 0, 0.1, 1), KeyTime.FromPercent(0.6)));
            anim.KeyFrames.Add(new DiscreteRectKeyFrame(new Rect(0.5, 0, 0.1, 1), KeyTime.FromPercent(0.8)));
            anim.Completed += Anim_Completed;
            Image.BeginAnimation(ImageBrush.ViewboxProperty, anim);
            cannon.PathCanvas.Children.Add(flash); Canvas.SetLeft(flash, -120); Canvas.SetTop(flash, Length - 170);
        }

        private void Anim_Completed(object sender, EventArgs e)
        {
            CurCannon.PathCanvas.Children.Remove(flash);
            if (Last) CurCannon.ShootFinished();
        }
    }
    class MakeShoot
    {
        public ImageBrush Image = new ImageBrush(new BitmapImage(new Uri("Images/Shoots/Cannon/shoot2.png", UriKind.Relative)));
        Rectangle flash;
        double speed = 1500;
        CannonAnim Curcannon;
        int Curshoot;
        double Seconds1;
        double Seconds2;
        double Length;
        public MakeShoot(double length, CannonAnim cannon, int curshoot)
        {
            Curcannon = cannon;
            Curshoot = curshoot;
            Length = length;
            flash = new Rectangle(); flash.Width = 50; flash.Height = 450;
            flash.Fill = Image;
            cannon.BulletCanvas.Children.Add(flash); Canvas.SetLeft(flash, 70); Canvas.SetTop(flash, -450);
            Seconds1 = (length - 500) / speed * Links.ShootAnimSpeed;
            if (Seconds1 < 0) Seconds1 = 0.1;
            Seconds2 = 500 / speed * Links.ShootAnimSpeed;

            DoubleAnimation anim = new DoubleAnimation(-450, length - 500, TimeSpan.FromSeconds(Seconds1));

            //DoubleAnimation anim = new DoubleAnimation(-450, length-500, TimeSpan.FromSeconds((length / speed)*Links.ShootAnimSpeed));
            anim.Completed += Anim_Completed0;
            flash.BeginAnimation(Canvas.TopProperty, anim);
        }

        private void Anim_Completed0(object sender, EventArgs e)
        {
            Curcannon.ShootCompleted(Curshoot);
            DoubleAnimation anim = new DoubleAnimation(Length - 500, Length, TimeSpan.FromSeconds(Seconds2));
            anim.Completed += Anim_Completed;
            flash.BeginAnimation(Canvas.TopProperty, anim);
        }
        private void Anim_Completed(object sender, EventArgs e)
        {
            Curcannon.BulletCanvas.Children.Remove(flash);

        }

    }
}


