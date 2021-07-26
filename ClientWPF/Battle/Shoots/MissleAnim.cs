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
    class MissleAnim : ShootAnim
    {
        public static byte ShieldMode = 1;
        public bool InHull = false;
        public bool IsMiss = false;
        public bool HasCrit = false;
        public Canvas BulletCanvas;
        public MissleAnim(Point firstPoint, Point secondPoint, bool IsMiss, bool inhull)
            : base(firstPoint, secondPoint, IsMiss)
        {
            InHull = inhull;
            this.IsMiss = IsMiss;
            CreateCanvas(500);
            BulletCanvas = new Canvas(); BulletCanvas.Width = PathCanvas.Width;
            BulletCanvas.Height = PathCanvas.Height;
            BulletCanvas.ClipToBounds = true;
            PathCanvas.Children.Add(BulletCanvas);
            PathCanvas.ClipToBounds = false;
            //PathCanvas.Background = Brushes.White;

            if (Links.ShootAnimSpeed == 1)
                new MySound("Battle/Cannon.mp3");

            new MakeShoot(Length, this, IsMiss);

            if (IsRealBattle)
                BattleController.ShieldFlashStarted(TimeSpan.FromSeconds(((250+RealLength-300) / 700) * Links.ShootAnimSpeed), Shoots(), WaveDelay(), Angle, 1);
            else
                HelpImage.ShowWeaponCanvas.ShieldFlashStarted(TimeSpan.FromSeconds(((250 + RealLength - 300) / 700) * Links.ShootAnimSpeed), Shoots(), WaveDelay(), Angle);
        }

        public static int Shoots()
        {
            return 1;
        }
        public static TimeSpan WaveDelay()
        {
            return TimeSpan.Zero;
            //return TimeSpan.FromSeconds(Links.ShootAnimSpeed / 2);
        }
        void ShootCompleted()
        {
            new MakeBlow(Length, this, InHull);
        }
        class MakeBlow
        {
            public ImageBrush Image = new ImageBrush((new BitmapImage(new Uri("Images/Shoots/Missle/missle_blow.png", UriKind.Relative))));
            MissleAnim CurMissle;
            public MakeBlow(double length, MissleAnim missle, bool InHull)
            {
                CurMissle = missle;
                Rectangle flash = new Rectangle(); flash.Width = 500; flash.Height = 500;
                missle.PathCanvas.Children.Add(flash);
                if (InHull)
                    Canvas.SetTop(flash, length - 250 - 100);
                else
                    Canvas.SetTop(flash, length - 250 - 300);
                RectAnimationUsingKeyFrames imageanim = new RectAnimationUsingKeyFrames();
                imageanim.Duration = TimeSpan.FromSeconds(Links.ShootAnimSpeed * 0.5);
                for (int i = 0; i < 36; i++)
                {
                    Rect rect = new Rect(i % 10 * 0.1, (i / 10) / 4.0, 0.1, 1 / 4.0);
                    imageanim.KeyFrames.Add(new DiscreteRectKeyFrame(rect, KeyTime.FromPercent(i / 35.0)));
                }
                flash.Fill = Image;
                imageanim.Completed += Imageanim_Completed;
                Image.BeginAnimation(ImageBrush.ViewboxProperty, imageanim);
            }

            private void Imageanim_Completed(object sender, EventArgs e)
            {
                CurMissle.Anim_Completed(null, null);
            }
        }
        class MakeShoot
        {
            public ImageBrush Image = new ImageBrush((new BitmapImage(new Uri("Images/Shoots/Missle/missle_shoot.png", UriKind.Relative))));
            Rectangle flash;
            public static double speed = 700;
            MissleAnim CurMissle;
            double Seconds1;
            double Seconds2;
            double Length;
            public static int flashdistance = 300;
            public MakeShoot(double length, MissleAnim missle, bool IsMiss)
            {
                CurMissle = missle;
                Length = length;
                flash = new Rectangle(); flash.Width = 1017; flash.Height = 596;
                flash.Fill = Image;
                flash.RenderTransformOrigin = new Point(0.5, 0.5);
                flash.RenderTransform = new RotateTransform(90);
                missle.BulletCanvas.Children.Add(flash); Canvas.SetLeft(flash, -260); Canvas.SetTop(flash, -250);
                RectAnimationUsingKeyFrames imageanim = new RectAnimationUsingKeyFrames();
                for (int i = 0; i < 56; i++)
                {
                    Rect rect = new Rect(i % 10 * 0.1, (i / 10) / 6.0, 0.1, 1 / 6.0);
                    imageanim.KeyFrames.Add(new DiscreteRectKeyFrame(rect, KeyTime.FromPercent(i / 55.0)));
                }
                DoubleAnimation moveanim;
                if (IsMiss == true)
                {
                    imageanim.Duration = TimeSpan.FromSeconds(length / speed * Links.ShootAnimSpeed);
                    moveanim = new DoubleAnimation(-250, length, imageanim.Duration);
                    moveanim.Completed += Shoot_Miss_Completed;
                }
                else
                {
                    Seconds1 = (250+length - flashdistance) / speed * Links.ShootAnimSpeed;
                    if (Seconds1 < 0) Seconds1 = 0.1;
                    Seconds2 = flashdistance / speed * Links.ShootAnimSpeed;
                    imageanim.Duration = TimeSpan.FromSeconds((Seconds1 + Seconds2) * Links.ShootAnimSpeed);
                    moveanim = new DoubleAnimation(-250, length - flashdistance, TimeSpan.FromSeconds(Seconds1));
                    moveanim.Completed += Shoot_Hit_Completed;
                }
                Image.BeginAnimation(ImageBrush.ViewboxProperty, imageanim);
                moveanim.AccelerationRatio = 1;
                flash.BeginAnimation(Canvas.TopProperty, moveanim);
            }

            private void Shoot_Hit_Completed(object sender, EventArgs e)
            {
                CurMissle.ShootCompleted();
                DoubleAnimation anim = new DoubleAnimation(Length - flashdistance, Length, TimeSpan.FromSeconds(Seconds2));
                flash.BeginAnimation(Canvas.TopProperty, anim);
            }

            private void Shoot_Miss_Completed(object sender, EventArgs e)
            {
                CurMissle.Anim_Completed(null, null);
            }
        }
    }
}