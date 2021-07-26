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
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace Client
{
    class MainButton:Canvas
    {
        static TimeSpan time = TimeSpan.FromSeconds(0.5);
        DoubleAnimation shestrightanim = new DoubleAnimation(0, 720, time);
        DoubleAnimation shestleftanim = new DoubleAnimation(720, 0, time);
        DoubleAnimation leftstartanim = new DoubleAnimation(-45, time);
        DoubleAnimation leftendanim = new DoubleAnimation(0, time);
        DoubleAnimation rightstartanim = new DoubleAnimation(90, time);
        DoubleAnimation rightendanim = new DoubleAnimation(47, time);
        RotateTransform rotateleft = new RotateTransform();
        RotateTransform rotateright = new RotateTransform();
        Rectangle StvorkaLeft;
        Rectangle StvorkaRight;
        TextBlock block;
        public MainButton()
        {
            Width = 200;
            Height = 100;
            Canvas.SetZIndex(this, MainCanvas.ZIndexes.Buttons);
            Rectangle YgolokLeft = PutRectangle(80, 60, 0, 4, 20, Links.Brushes.Interface.MBYgolokLeft, false);
            Rectangle YgolokRight = PutRectangle(80, 60, 120, 4, 20, Links.Brushes.Interface.MBYgolorRight, false);
            Rectangle Shest00 = PutRectangle(25, 25, 9, 1, 10, Links.Brushes.Interface.MBShester2, false);
            Shest00.RenderTransform = rotateright;
            Rectangle Shest01 = PutRectangle(25, 25, 164, 1, 10, Links.Brushes.Interface.MBShester2, false);
            Shest01.RenderTransform = rotateleft;
            Rectangle Shluz0 = PutRectangle(50, 20, 20, 1, 5, Links.Brushes.Interface.MBShluz, false);
            Rectangle Shluz1 = PutRectangle(50, 20, 129, 1, 5, Links.Brushes.Interface.MBShluz, true);
            Rectangle Klapon0 = PutRectangle(50, 28, 48, 14, 5, Links.Brushes.Interface.MBKlapon, false);
            Rectangle Klapon1 = PutRectangle(50, 28, 100, 14, 5, Links.Brushes.Interface.MBKlapon, true);
            Rectangle Shest10 = PutRectangle(25, 25, 26, 37, 25, Links.Brushes.Interface.MBShester1, false);
            Shest10.RenderTransform = rotateright;  
            Rectangle Shest11 = PutRectangle(25, 25, 147, 37, 25, Links.Brushes.Interface.MBShester1, false);
            Shest11.RenderTransform = rotateleft;
            Rectangle ButtonBack = PutRectangle(103, 28, 48, 35, 0, Links.Brushes.Interface.MBButtonBack, false);
            Canvas DopCanvas = new Canvas();
            DopCanvas.Width = 102; DopCanvas.Height = 27;
            Children.Add(DopCanvas);
            Canvas.SetLeft(DopCanvas, 49); Canvas.SetTop(DopCanvas, 36);
            Canvas.SetZIndex(DopCanvas, 10); DopCanvas.ClipToBounds = true;
            StvorkaLeft = PutSpecRectangle(55, 29, 0, 0, DopCanvas, Links.Brushes.Interface.MBStvorkaLeft);
            StvorkaRight = PutSpecRectangle(55, 29, 47, -1, DopCanvas, Links.Brushes.Interface.MBStvorkaRight);
            MouseEnter += MainButton_MouseEnter;
            MouseLeave += MainButton_MouseLeave;

            block = new TextBlock();
            block.FontFamily = Links.Font;
            block.FontWeight = FontWeights.Bold;
            block.FontSize = 18;
            block.Width = 100;
            block.TextAlignment = TextAlignment.Center;
            block.Foreground = Brushes.White;
            Children.Add(block);
            Canvas.SetLeft(block, 49); Canvas.SetTop(block, 38); Canvas.SetZIndex(block, 5);
        }
        public void SetText(string text)
        {
            block.Text = text;
        }
        private void MainButton_MouseLeave(object sender, MouseEventArgs e)
        {
            rotateleft.BeginAnimation(RotateTransform.AngleProperty, shestrightanim);
            rotateright.BeginAnimation(RotateTransform.AngleProperty, shestleftanim);
            StvorkaLeft.BeginAnimation(Canvas.LeftProperty, leftendanim);
            StvorkaRight.BeginAnimation(Canvas.LeftProperty, rightendanim);
        }

        private void MainButton_MouseEnter(object sender, MouseEventArgs e)
        {
            rotateleft.BeginAnimation(RotateTransform.AngleProperty, shestleftanim);
            rotateright.BeginAnimation(RotateTransform.AngleProperty, shestrightanim);
            StvorkaLeft.BeginAnimation(Canvas.LeftProperty, leftstartanim);
            StvorkaRight.BeginAnimation(Canvas.LeftProperty, rightstartanim);
        }

        Rectangle PutRectangle(int width, int height, int left, int top, int z, Brush brush, bool reverse)
        {
            Rectangle rect = new Rectangle();
            rect.Width = width;
            rect.Height = height;
            Children.Add(rect);
            Canvas.SetZIndex(rect, z);
            Canvas.SetLeft(rect, left);
            Canvas.SetTop(rect, top);
            rect.Fill = brush;
            rect.RenderTransformOrigin = new Point(0.5, 0.5);
            if (reverse)
            {
                ScaleTransform scale = new ScaleTransform();
                scale.ScaleX = -1;
                rect.RenderTransform = scale;
            }
            return rect;
        }
        Rectangle PutSpecRectangle(int width, int height, int left, int top, Canvas canvas, Brush brush)
        {
            Rectangle rect = new Rectangle();
            rect.Width = width;
            rect.Height = height;
            canvas.Children.Add(rect);
            Canvas.SetLeft(rect, left);
            Canvas.SetTop(rect, top);
            rect.Fill = brush;
            return rect;
        }
    }
}
