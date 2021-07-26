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
    enum EHexShipStasus { Blind,  Mark }
    class HexShipStatus : Grid
    {
        static Brush BlindBrush = GetBlindBrush();
        static Brush MarkBrush = new VisualBrush(DroneAnim.PutCritDrone.GetElementCanvas());
        /*static Brush SlowBrush = GetSlowBrush();
        static Brush RadiationBrush = RoundIndicator2.CorruptedBrush;
        static RadialGradientBrush PortBrush = GetPortBrush();
        static RadialGradientBrush GateBrush = GetGateBrush();
        static RadialGradientBrush AttackBrush = GetAttackBrush();
        static RadialGradientBrush DefenseBrush = GetGateBrush();*/
        
        RotateTransform TransformRotate;
        ScaleTransform TransformScale;
        double Angle = 0;
        public HexShipStatus(EHexShipStasus status, byte pos, double angle)
        {
            Width = 50; Height = 50; 
            RenderTransformOrigin = new Point(0.5, 0.5);
            Ellipse el = new Ellipse();
            el.Width = 50; el.Height = 50; Children.Add(el);
            TransformGroup group = new TransformGroup();
            TransformRotate = new RotateTransform();
            group.Children.Add(TransformRotate);
            TransformScale = new ScaleTransform();
            group.Children.Add(TransformScale);
            RenderTransform = group;
            Angle = -angle;
            TransformRotate.Angle = Angle;
            switch (status)
            {
                case EHexShipStasus.Blind: el.Fill = BlindBrush; break;
                case EHexShipStasus.Mark: el.Fill = MarkBrush; break;

            }
        }
        public void RotateTo(double da)
        {
            double newangle = Angle + da;
            DoubleAnimation anim = new DoubleAnimation(Angle, newangle, TimeSpan.FromSeconds(Math.Abs(da) / 200));
            TransformRotate.BeginAnimation(RotateTransform.AngleProperty, anim);
            Angle = newangle % 360;

        }
        public void SetBig()
        {
            DoubleAnimation anim = new DoubleAnimation(1, 5, TimeSpan.FromSeconds(Links.ShootAnimSpeed * 2));
            anim.AutoReverse = true;
            TransformScale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            TransformScale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
        }
        public void SetAngle(double da)
        {
            double newangle = Angle + da;
            DoubleAnimation anim = new DoubleAnimation(newangle, Links.ZeroTime);
            TransformRotate.BeginAnimation(RotateTransform.AngleProperty, anim);
            //Transform.Angle = newangle;
            Angle = newangle;
        }
        void PutLabel(int value)
        {
            if (value == 255) return;
            Label lbl = new Label();
            lbl.Content = value.ToString();
            lbl.Style = Links.TextStyle;
            lbl.FontSize = 18;
            Children.Add(lbl);
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            lbl.VerticalAlignment = VerticalAlignment.Center;
        }
        static RadialGradientBrush GetAttackBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Red, 1));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            return brush;
        }
        static Brush GetBlindBrush()
        {
            GeometryGroup group = new GeometryGroup();
            CombinedGeometry circle = new CombinedGeometry();
            circle.GeometryCombineMode = GeometryCombineMode.Exclude;
            circle.Geometry1 = new EllipseGeometry(new Point(15, 15), 13, 13);
            circle.Geometry2 = new EllipseGeometry(new Point(15, 15), 10, 10);
            
            group.Children.Add(circle);
            group.Children.Add(new RectangleGeometry(new Rect(0, 14, 11, 2)));
            group.Children.Add(new RectangleGeometry(new Rect(19, 14, 11, 2)));
            group.Children.Add(new RectangleGeometry(new Rect(14, 0, 2, 11)));
            group.Children.Add(new RectangleGeometry(new Rect(14, 19, 2, 11)));
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.7));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.3));

            DrawingGroup drawinggroup = new DrawingGroup();
            drawinggroup.Children.Add(new GeometryDrawing(Brushes.Gold, null, new EllipseGeometry(new Point(15, 15), 15, 15)));
            drawinggroup.Children.Add(new GeometryDrawing(brush, null, group));
            return new DrawingBrush(drawinggroup);
        }
        static Brush GetSlowBrush()
        {
            GeometryGroup group = new GeometryGroup();
            CombinedGeometry circle = new CombinedGeometry();
            circle.GeometryCombineMode = GeometryCombineMode.Exclude;
            circle.Geometry1 = new EllipseGeometry(new Point(15, 15), 13, 13);
            circle.Geometry2 = new EllipseGeometry(new Point(15, 15), 10, 10);
            group.Children.Add(circle);
            group.Children.Add(new RectangleGeometry(new Rect(0, 14, 15, 2), 0, 0, new RotateTransform(45, 15, 15)));
            group.Children.Add(new RectangleGeometry(new Rect(15, 14, 15, 2), 0, 0, new RotateTransform(45, 15, 15)));
            group.Children.Add(new RectangleGeometry(new Rect(14, 0, 2, 15), 0, 0, new RotateTransform(45, 15, 15)));
            group.Children.Add(new RectangleGeometry(new Rect(14, 15, 2, 15), 0, 0, new RotateTransform(45, 15, 15)));


            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.7));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.3));
            brush.StartPoint = new Point(0.3, 0); brush.EndPoint = new Point(0.7, 1);

            DrawingGroup drawinggroup = new DrawingGroup();
            drawinggroup.Children.Add(new GeometryDrawing(Brushes.Gold, null, new EllipseGeometry(new Point(15, 15), 15, 15)));
            drawinggroup.Children.Add(new GeometryDrawing(brush, null, group));
            return new DrawingBrush(drawinggroup);
        }
        //не используется
        static Brush GBB()
        {
            EllipseGeometry ellipse = new EllipseGeometry(new Point(15, 15), 15, 15);
            GeometryDrawing ellipsedrawing = new GeometryDrawing(new SolidColorBrush(Color.FromArgb(0,0,0,0)), new Pen(), ellipse);
            PathGeometry back = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M15,15 l6,-0 l-7,-8 l8,3 l-2,-5 l5,5 l-2,-5 l3,0 a15,15 0 0,1 -20,22 l-3,-8 l8,3 l-6,-8 l12,5z"));
            RadialGradientBrush backbrush = new RadialGradientBrush();
            backbrush.GradientStops.Add(new GradientStop(Colors.White, 0));
            backbrush.GradientStops.Add(new GradientStop(Colors.LightBlue, 0.5));
            backbrush.GradientStops.Add(new GradientStop(Color.FromArgb(140, 0, 0, 255), 0.9));
            backbrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 255, 255, 255), 1.2));
            GeometryDrawing backdrawing = new GeometryDrawing(backbrush, new Pen(), back);
            LineGeometry laser = new LineGeometry(new Point(6, 6), new Point(20, 20));
            LinearGradientBrush laserbrush = new LinearGradientBrush();
            laserbrush.StartPoint = new Point(1, 0); laserbrush.EndPoint = new Point(0, 1);
            laserbrush.GradientStops.Add(new GradientStop(Colors.Blue, 0.55));
            laserbrush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            laserbrush.GradientStops.Add(new GradientStop(Colors.Black, 0.45));
            Pen laserpen = new Pen(laserbrush, 3);
            laserpen.StartLineCap = PenLineCap.Round;
            laserpen.EndLineCap = PenLineCap.Round;
            GeometryDrawing laserdrawing = new GeometryDrawing(new SolidColorBrush(), laserpen, laser);
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(ellipsedrawing); group.Children.Add(backdrawing); group.Children.Add(laserdrawing);
            DrawingBrush result = new DrawingBrush();
            result.Drawing = group;
            return result;
        }
        static RadialGradientBrush GetPortBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Gold, 1));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            return brush;
        }
        static RadialGradientBrush GetGateBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Green, 1));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            return brush;
        }
    }
    class TextShipInfo:Label
    {
        HexShip Ship;
        public TextShipInfo(string text, HexShip ship)
        {
            Width = 220;
            Height = 100;
            FontFamily = Links.Font;
            Foreground = Brushes.White;
            Opacity = 0.7;
            FontSize = 60;
            FontWeight = FontWeights.Bold;
            Content = text;
            HorizontalContentAlignment = HorizontalAlignment.Center;
            VerticalContentAlignment = VerticalAlignment.Center;
            
            //ship.Children.Add(this);
            Canvas.SetLeft(this, ship.Hex.CenterX - 110);
            Canvas.SetTop(this, ship.Hex.CenterY - 50);
            Canvas.SetZIndex(this, 50);
            HexCanvas.Inner.Children.Add(this);
            Ship = ship;
            MouseEnter += TextShipInfo_MouseEnter;
            PreviewMouseDown += TextShipInfo_PreviewMouseDown;
            //RenderTransformOrigin = new Point(0.5, 0.5);
            //RotateTransform rotate = new RotateTransform();
            //rotate.Angle = -ship.Angle;
            //this.RenderTransform = rotate;
        }

        private void TextShipInfo_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton==MouseButtonState.Pressed)
            {
                Links.Controller.ShipPopUp.Place(Ship.Ship);
            }
        }

        private void TextShipInfo_MouseEnter(object sender, MouseEventArgs e)
        {
            HexCanvasPopup.Place(Ship);
        }
        /*
public void Remove()
{
   Canvas par = (Canvas)Parent;
   if (par != null)
       par.Children.Remove(this);
}
*/
    }
}
