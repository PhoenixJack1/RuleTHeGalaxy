using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    class GlobalCanvasBtn:Canvas
    {
        RotateTransform Left_Shest_Rotate, Right_Shest_Rotate;
        Rectangle Black_Background;
        TextBlock Button_Text;
        Canvas Stvorks;
        Rectangle Left_Stvork, Right_Stvork;
        Rectangle Top_Border, Low_Border;
        Canvas Move_Canvas;
        RotateTransform Top_Buffer_Rotate, Bottom_Buffer_Rotate;
        bool opening = false;
        bool closing = false;
        public Viewbox VBX;
        public GlobalCanvasBtn()
        {
            Width = 820; Height = 291;
            VBX = new Viewbox();
            VBX.Width = 410; VBX.Height = 145.5;
            VBX.Child = this;
            Canvas.SetZIndex(this, 10);
            Children.Add(GetRect(291, 291, 60, 0, Brushes.Brush1, 10));
            Children.Add(GetRect(88, 194, 240, 48.5, Brushes.Brush2, 15));
            Rectangle Left_Shest = GetRect(85, 85, 215, 98, Brushes.Brush7, 12);
            Left_Shest.RenderTransformOrigin = new Point(0.5, 0.5);
            Left_Shest_Rotate = new RotateTransform();
            Left_Shest.RenderTransform = Left_Shest_Rotate;
            Children.Add(Left_Shest);
            Canvas Stvorks_Canvas = GetCanvas(520, 192, 250, 48.5, true);
            Children.Add(Stvorks_Canvas);
            Black_Background = GetRect(410, 180, -400, 6, System.Windows.Media.Brushes.Black, 0);
            Stvorks_Canvas.Children.Add(Black_Background);
            Button_Text = new TextBlock();
            Button_Text.Width = 453;
            Button_Text.Foreground = System.Windows.Media.Brushes.White;
            Button_Text.FontSize = 40;
            Canvas.SetLeft(Button_Text, -400);
            Canvas.SetTop(Button_Text, 70);
            Button_Text.TextAlignment = TextAlignment.Center;
            Stvorks_Canvas.Children.Add(Button_Text);
            Stvorks = GetCanvas(473, 134, -400, 29.5, true);
            Stvorks_Canvas.Children.Add(Stvorks);
            Stvorks.Children.Add(Left_Stvork = GetRect(265, 134, 1, 0, Brushes.Brush4, 0));
            Stvorks.Children.Add(Right_Stvork = GetRect(265, 134, 205, 0, Brushes.Brush4, 0));
            Right_Stvork.RenderTransformOrigin = new Point(0.5, 0.5);
            Right_Stvork.RenderTransform = new ScaleTransform(-1, -1);
            Canvas Border_Canvas = GetCanvas(420, 192, 250, 48.5, true);
            Children.Add(Border_Canvas);
            Border_Canvas.Children.Add(Top_Border = GetRect(410, 40, -400, 0, Brushes.Brush3, 0));
            Border_Canvas.Children.Add(Low_Border = GetRect(410, 40, -400, 154, Brushes.Brush3, 0));
            Move_Canvas = GetCanvas(820, 291, -410, 0, false);
            Children.Add(Move_Canvas);
            Rectangle Top_Small_Thing = GetRect(80, 51, 700, 80, Brushes.Brush5, 0);
            Top_Small_Thing.RenderTransformOrigin = new Point(0.5, 0.5);
            Top_Small_Thing.RenderTransform = new RotateTransform(90);
            Move_Canvas.Children.Add(Top_Small_Thing);
            Rectangle Low_Small_Thing = GetRect(80, 51, 700, 160, Brushes.Brush5, 0);
            Low_Small_Thing.RenderTransformOrigin = new Point(0.5, 0.5);
            TransformGroup Low_Small_Thing_Group = new TransformGroup();
            Low_Small_Thing_Group.Children.Add(new ScaleTransform(-1, 1));
            Low_Small_Thing_Group.Children.Add(new RotateTransform(90));
            Low_Small_Thing.RenderTransform = Low_Small_Thing_Group;
            Move_Canvas.Children.Add(Low_Small_Thing);
            Rectangle Top_Buffer = GetRect(155, 99, 650, 160, Brushes.Brush6, 0);
            Top_Buffer.RenderTransformOrigin = new Point(0.5, -0.2);
            Top_Buffer.RenderTransform = Top_Buffer_Rotate = new RotateTransform(360);
            Move_Canvas.Children.Add(Top_Buffer);
            Rectangle Bottom_Buffer = GetRect(155, 99, 650, 163, Brushes.Brush6, 0);
            Bottom_Buffer.RenderTransformOrigin = new Point(0.5, -0.2);
            TransformGroup Bottom_Buffer_Group = new TransformGroup();
            Bottom_Buffer.RenderTransform = Bottom_Buffer_Group;
            Bottom_Buffer_Group.Children.Add(Bottom_Buffer_Rotate = new RotateTransform(360));
            Bottom_Buffer_Group.Children.Add(new ScaleTransform(1, -1));
            Move_Canvas.Children.Add(Bottom_Buffer);
            Rectangle Right_Rotate = GetRect(85, 85, 665, 98, Brushes.Brush7, 0);
            Right_Rotate.RenderTransformOrigin = new Point(0.5, 0.5);
            Right_Rotate.RenderTransform = Right_Shest_Rotate = new RotateTransform(0);
            Move_Canvas.Children.Add(Right_Rotate);

            MouseEnter += Canvas_MouseEnter;
            //MouseLeave += Canvas_MouseLeave;

        }
        public void SetText(string text)
        {
            Button_Text.Text = text;
        }
        public void PutToCanvas(Canvas canvas, int left, int top)
        {
            canvas.Children.Add(VBX);
            Canvas.SetLeft(VBX, left);
            Canvas.SetTop(VBX, top);
        }
        public void Rotate()
        {
            RenderTransformOrigin = new Point(0.5, 0.5);
            RenderTransform = new ScaleTransform(-1, 1);
            Button_Text.RenderTransformOrigin = new Point(0.5, 0.5);
            Button_Text.RenderTransform = new ScaleTransform(-1, 1);
        }
        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender!=null) MouseLeave -= Canvas_MouseLeave;
            if (opening == true) { closing = true;  return; }
            closing = true;
            DoubleAnimation LeftStvorkAnim = new DoubleAnimation(1, time);
            DoubleAnimation RightStvorkAnim = new DoubleAnimation(205, time);
            RightStvorkAnim.Completed += FullMoveAnim_Completed;
            Left_Stvork.BeginAnimation(Canvas.LeftProperty, LeftStvorkAnim);
            Right_Stvork.BeginAnimation(Canvas.LeftProperty, RightStvorkAnim);
            DoubleAnimation ShestRightAnim = new DoubleAnimation(0, time);
            Right_Shest_Rotate.BeginAnimation(RotateTransform.AngleProperty, ShestRightAnim);
        }

        private void FullMoveAnim_Completed(object sender, EventArgs e)
        {

            DoubleAnimation ShestLeftAnim = new DoubleAnimation(0, time);
            Left_Shest_Rotate.BeginAnimation(RotateTransform.AngleProperty, ShestLeftAnim);
            DoubleAnimation BufferAnim = new DoubleAnimation(360, time);
            Top_Buffer_Rotate.BeginAnimation(RotateTransform.AngleProperty, BufferAnim);
            Bottom_Buffer_Rotate.BeginAnimation(RotateTransform.AngleProperty, BufferAnim);
            DoubleAnimation BorderAnim = new DoubleAnimation(-400, time);
            Top_Border.BeginAnimation(Canvas.LeftProperty, BorderAnim);
            Low_Border.BeginAnimation(Canvas.LeftProperty, BorderAnim);
            Black_Background.BeginAnimation(Canvas.LeftProperty, BorderAnim);
            Button_Text.BeginAnimation(Canvas.LeftProperty, BorderAnim);
            Stvorks.BeginAnimation(Canvas.LeftProperty, BorderAnim);
            DoubleAnimation FullMoveAnim = new DoubleAnimation(-410, time);
            FullMoveAnim.Completed += Closing_Finish;
            Move_Canvas.BeginAnimation(Canvas.LeftProperty, FullMoveAnim);
        }

        private void Closing_Finish(object sender, EventArgs e)
        {
            closing = false;
            MouseEnter += Canvas_MouseEnter;
        }

        TimeSpan time = TimeSpan.FromSeconds(0.5);
        private void Canvas_MouseEnter(object sender, MouseEventArgs e)
        {
            MouseEnter -= Canvas_MouseEnter;
            opening = true;
            MouseLeave += Canvas_MouseLeave;
            DoubleAnimation ShestLeftAnim = new DoubleAnimation(360, time);
            Left_Shest_Rotate.BeginAnimation(RotateTransform.AngleProperty, ShestLeftAnim);
            DoubleAnimation BufferAnim = new DoubleAnimation(180, time);
            Top_Buffer_Rotate.BeginAnimation(RotateTransform.AngleProperty, BufferAnim);
            Bottom_Buffer_Rotate.BeginAnimation(RotateTransform.AngleProperty, BufferAnim);
            DoubleAnimation BorderAnim = new DoubleAnimation(10, time);
            Top_Border.BeginAnimation(Canvas.LeftProperty, BorderAnim);
            Low_Border.BeginAnimation(Canvas.LeftProperty, BorderAnim);
            Black_Background.BeginAnimation(Canvas.LeftProperty, BorderAnim);
            Button_Text.BeginAnimation(Canvas.LeftProperty, BorderAnim);
            Stvorks.BeginAnimation(Canvas.LeftProperty, BorderAnim);
            DoubleAnimation FullMoveAnim = new DoubleAnimation(0, time);
            FullMoveAnim.Completed += FullMoveAnim_Completed2;
            Move_Canvas.BeginAnimation(Canvas.LeftProperty, FullMoveAnim);
        }
        private void FullMoveAnim_Completed2(object sender, EventArgs e)
        {
            DoubleAnimation LeftStvorkAnim = new DoubleAnimation(-149, time);
            DoubleAnimation RightStvorkAnim = new DoubleAnimation(355, time);
            Left_Stvork.BeginAnimation(Canvas.LeftProperty, LeftStvorkAnim);
            Right_Stvork.BeginAnimation(Canvas.LeftProperty, RightStvorkAnim);
            DoubleAnimation ShestRightAnim = new DoubleAnimation(-360, time);
            ShestRightAnim.Completed += Opening_Finish;
            Right_Shest_Rotate.BeginAnimation(RotateTransform.AngleProperty, ShestRightAnim);
        }

        private void Opening_Finish(object sender, EventArgs e)
        {
            opening = false;
            if (closing == true)
            {
                Canvas_MouseLeave(null, null);
            }
        }

        Canvas GetCanvas(int width, int height, double left, double top, bool cliptobounds)
        {
            Canvas canvas = new Canvas();
            canvas.Width = width; canvas.Height = height;
            Canvas.SetLeft(canvas, left); Canvas.SetTop(canvas, top);
            canvas.ClipToBounds = cliptobounds;
            return canvas;
        }
        Rectangle GetRect(int width, int height, double left, double top, Brush brush, int z)
        {
            Rectangle rect = new Rectangle();
            rect.Width = width; rect.Height = height;
            Canvas.SetLeft(rect, left); Canvas.SetTop(rect, top);
            Canvas.SetZIndex(rect, z); rect.Fill = brush;
            return rect;
        }
        public class Brushes
        {
            public static ImageBrush Brush1 = Gets.AddGCB("GCB_01");
            public static ImageBrush Brush2 = Gets.AddGCB("GCB_02");
            public static ImageBrush Brush3 = Gets.AddGCB("GCB_03");
            public static ImageBrush Brush4 = Gets.AddGCB("GCB_04");
            public static ImageBrush Brush5 = Gets.AddGCB("GCB_05");
            public static ImageBrush Brush6 = Gets.AddGCB("GCB_06");
            public static ImageBrush Brush7 = Gets.AddGCB("GCB_07");
        }
    }
}
