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
    class ShootAnim
    {
        public static bool HaveSelfSparks = false;
        public Canvas PathCanvas;
        public double Angle;
        protected double Length;
        protected double RealLength; //расстояние до цели
        protected Point FirstPoint; //расстояние полёта снаряда
        public Canvas Canvas;
        static double MissDelta = 100;
        public static bool IsRealBattle = true;
        public ShootAnim() { }
        public ShootAnim(Point firstPoint, Point secondPoint, bool IsMiss)
        {
            if (firstPoint.X == secondPoint.X && firstPoint.Y == secondPoint.Y)
            {
                firstPoint = new Point(0, 0); secondPoint = new Point(1, 1);
            }
            FirstPoint = firstPoint;
            double dy = FirstPoint.Y - secondPoint.Y;
            double dx = secondPoint.X - FirstPoint.X;
            RealLength = Math.Sqrt(Math.Pow(FirstPoint.X - secondPoint.X, 2) + Math.Pow(FirstPoint.Y - secondPoint.Y, 2));
            double tga;

            if (IsMiss)
            {
                double curangle = Common.CalcAngle(firstPoint, secondPoint); //текущий угол между пушкой и целью
                bool IsLeft = curangle % 2 > 1;
                double missdx, missdy;
                if (IsLeft)
                { missdx = MissDelta * Math.Cos(curangle * Math.PI / 180); missdy = MissDelta * Math.Sin(curangle * Math.PI / 180); }
                else
                { missdx = -MissDelta * Math.Cos(curangle * Math.PI / 180); missdy = -MissDelta * Math.Sin(curangle * Math.PI / 180); }
                secondPoint=new Point(secondPoint.X+missdx,secondPoint.Y+missdy);
                dy = secondPoint.Y - firstPoint.Y;
                dx = secondPoint.X - FirstPoint.X;
                tga = dx / dy;
                double DX = 0, DY = 0;
                double H = HexCanvas.Inner.Height;
                double W = HexCanvas.Inner.Width;
                double maxY = dy > 0 ? H : 0;
                double maxX = dx > 0 ? W : 0;
                DX = maxX - firstPoint.X;
                DY = DX / tga;
                double Y2 = firstPoint.Y + DY;
                if (Y2 < 0 || Y2 > H)
                {
                    DY = maxY - firstPoint.Y;
                    DX = tga * DY;
                }

                Point sp = new Point(firstPoint.X + DX, firstPoint.Y + DY);
                secondPoint = sp;
            }
            dy = FirstPoint.Y - secondPoint.Y;
            dx = secondPoint.X - FirstPoint.X;
            Length = Math.Sqrt(Math.Pow(FirstPoint.X - secondPoint.X, 2) + Math.Pow(FirstPoint.Y - secondPoint.Y, 2));

            tga = dy / dx;
            if (dx == 0)
                Angle = dy > 0 ? 0 : 180;
            else if (dx > 0)
                Angle = 90 - Math.Atan(tga) * 180 / Math.PI;
            else
                Angle = 270 - Math.Atan(tga) * 180 / Math.PI;

        }
        
        protected void CreateCanvas(double width)
        {
            PathCanvas = new Canvas();
            Canvas.SetZIndex(PathCanvas, Links.ZIndex.Shoots);
            PathCanvas.Width = width;
            PathCanvas.ClipToBounds = true;
            //Canvas.Children.Add(PathCanvas);
            PathCanvas.Height = Length;
            Canvas.SetLeft(PathCanvas, FirstPoint.X - PathCanvas.Width / 2);
            Canvas.SetTop(PathCanvas, FirstPoint.Y);
            RotateTransform transform = new RotateTransform(Angle + 180);
            PathCanvas.RenderTransformOrigin = new Point(0.5, 0);
            PathCanvas.RenderTransform = transform;
            Canvas = PathCanvas;
            //PathCanvas.Background = Brushes.White;
        }
        public virtual void BeginAnimation()
        {

        }
        protected void Anim_Completed(object sender, EventArgs e)
        {
            Canvas parent = (Canvas)Canvas.Parent;
            parent.Children.Remove(Canvas);
            //BattleController.Working = false;
        }
    }
}
