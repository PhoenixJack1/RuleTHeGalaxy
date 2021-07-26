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

namespace Client
{
    class Premium_Coin:Viewbox
    {
        static RadialGradientBrush Path1Brush = GetPath1Brush();
        static RadialGradientBrush Path2Brush = GetPath2Brush();
        static RadialGradientBrush Path3Brush = GetPath3Brush();
        TextBlock Block;
        public int Value { get; private set; }
        public Premium_Coin(int width, int height, int value)
        {
            Width = width; Height = height; Value = value;
            Canvas canvas = new Canvas();
            Child = canvas;
            canvas.Width = 100; canvas.Height = 100;
            Path path1 = new Path();
            path1.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M50,0 a50,50 0 1,0 0.1,0"));
            path1.Fill = Path1Brush;
            canvas.Children.Add(path1);
            Path path2 = new Path();
            path2.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M50,2 a48,48 0 1,0 0.1,0"));
            path2.Fill = Path2Brush;
            canvas.Children.Add(path2);
            Path path3 = new Path();
            path3.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M50,20 a30,30 0 1,0 0.1,0"));
            path3.Stroke = Brushes.Black;
            path3.StrokeThickness = 0.2;
            path3.Fill = Path1Brush;
            canvas.Children.Add(path3);
            Block = new TextBlock();
            Block.Foreground = Brushes.DarkGoldenrod;
            Block.FontFamily = Links.Font;
            canvas.Children.Add(Block);
            Canvas.SetLeft(Block, 25);
            Canvas.SetTop(Block, 35);
            Block.FontSize = 27;
            Block.Width = 50;
            Block.Height = 30;
            Block.TextAlignment = TextAlignment.Center;
            Block.FontWeight = FontWeights.Bold;
            Block.Text = value.ToString();

            Path path = new Path();
            path.Stroke = Brushes.Orange;
            path.StrokeThickness = 0.2;
            canvas.Children.Add(path);
            path.Fill = Path3Brush;
            GeometryGroup group = new GeometryGroup();
            path.Data = group;
            for (double i = 0; i < 360; i += 22.5)
            {
                double x = Math.Sin(i / 180.0 * Math.PI) * 39;
                double y = Math.Cos(i / 180.0 * Math.PI) * 39;
                string s = String.Format("M{0},{1} l2,4 l5,0 l-4,2 l2,4 l-5,-2.5 l-5,2.5 l2,-4 l-4,-2 l5,0z", (int)(50 + x), (int)(50 - y - 5));
                group.Children.Add(new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(s)));
            }
            Path path4 = new Path();
            canvas.Children.Add(path4);
            path4.Fill = Brushes.Gold;
            GeometryGroup group1 = new GeometryGroup();
            path4.Data = group1;
            for (double i = 0; i <= 360; i += 3)
            {
                double x = Math.Sin(i / 180.0 * Math.PI) * 49;
                double y = Math.Cos(i / 180.0 * Math.PI) * 49;
                group1.Children.Add(new EllipseGeometry(new Point(x+50, 50-y), 1, 1));
            }
        }
        static RadialGradientBrush GetPath1Brush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.3); brush.RadiusX = 1; brush.RadiusY = 1;
            brush.GradientStops.Add(new GradientStop(Colors.Gold, 0.8));
            brush.GradientStops.Add(new GradientStop(Color.FromRgb(255, 255, 153), 0));
            return brush;
        }
        static RadialGradientBrush GetPath2Brush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.3); brush.RadiusX = 1; brush.RadiusY = 1;
            brush.GradientStops.Add(new GradientStop(Color.FromRgb(119,119,0), 0.8));
            brush.GradientStops.Add(new GradientStop(Color.FromRgb(255, 255, 153), 0));
            return brush;
        }
        static RadialGradientBrush GetPath3Brush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.3); brush.RadiusX = 1; brush.RadiusY = 1;
            brush.GradientStops.Add(new GradientStop(Colors.Gold, 0.8));
            brush.GradientStops.Add(new GradientStop(Colors.Goldenrod, 0));
            return brush;
        }
    }
    class Premium_Coin_Silver : Viewbox
    {
        static RadialGradientBrush Path1Brush = GetPath1Brush();
        static RadialGradientBrush Path2Brush = GetPath2Brush();
        static RadialGradientBrush Path3Brush = GetPath3Brush();
        TextBlock Block;
        public int Value { get; private set; }
        public Premium_Coin_Silver(int width, int height, int value)
        {
            Width = width; Height = height; Value = value;
            Canvas canvas = new Canvas();
            Child = canvas;
            canvas.Width = 100; canvas.Height = 100;
            Path path1 = new Path();
            path1.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M50,0 a50,50 0 1,0 0.1,0"));
            path1.Fill = Path1Brush;
            canvas.Children.Add(path1);
            Path path2 = new Path();
            path2.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M50,2 a48,48 0 1,0 0.1,0"));
            path2.Fill = Path2Brush;
            canvas.Children.Add(path2);
            Path path3 = new Path();
            path3.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M50,20 a30,30 0 1,0 0.1,0"));
            path3.Stroke = Brushes.Black;
            path3.StrokeThickness = 0.2;
            path3.Fill = Path1Brush;
            canvas.Children.Add(path3);
            Block = new TextBlock();
            Block.Foreground = new SolidColorBrush(Color.FromRgb(50, 50, 50));
            Block.FontFamily = Links.Font;
            canvas.Children.Add(Block);
            Canvas.SetLeft(Block, 25);
            Canvas.SetTop(Block, 35);
            Block.FontSize = 27;
            Block.Width = 50;
            Block.Height = 30;
            Block.TextAlignment = TextAlignment.Center;
            Block.FontWeight = FontWeights.Bold;
            Block.Text = value.ToString();

            Path path = new Path();
            path.Stroke = Brushes.Orange;
            path.StrokeThickness = 0.2;
            canvas.Children.Add(path);
            path.Fill = Path3Brush;
            GeometryGroup group = new GeometryGroup();
            path.Data = group;
            for (double i = 0; i < 360; i += 22.5)
            {
                double x = Math.Sin(i / 180.0 * Math.PI) * 39;
                double y = Math.Cos(i / 180.0 * Math.PI) * 39;
                string s = String.Format("M{0},{1} l2,4 l5,0 l-4,2 l2,4 l-5,-2.5 l-5,2.5 l2,-4 l-4,-2 l5,0z", (int)(50 + x), (int)(50 - y - 5));
                group.Children.Add(new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(s)));
            }
            Path path4 = new Path();
            canvas.Children.Add(path4);
            path4.Fill = Brushes.Silver;
            GeometryGroup group1 = new GeometryGroup();
            path4.Data = group1;
            for (double i = 0; i <= 360; i += 3)
            {
                double x = Math.Sin(i / 180.0 * Math.PI) * 49;
                double y = Math.Cos(i / 180.0 * Math.PI) * 49;
                group1.Children.Add(new EllipseGeometry(new Point(x + 50, 50 - y), 1, 1));
            }
        }
        static RadialGradientBrush GetPath1Brush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.3); brush.RadiusX = 1; brush.RadiusY = 1;
            brush.GradientStops.Add(new GradientStop(Colors.Silver, 0.8));
            brush.GradientStops.Add(new GradientStop(Color.FromRgb(200, 200, 200), 0));
            return brush;
        }
        static RadialGradientBrush GetPath2Brush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.3); brush.RadiusX = 1; brush.RadiusY = 1;
            brush.GradientStops.Add(new GradientStop(Color.FromRgb(50, 50, 50), 0.8));
            brush.GradientStops.Add(new GradientStop(Color.FromRgb(200, 200, 200), 0));
            return brush;
        }
        static RadialGradientBrush GetPath3Brush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.3); brush.RadiusX = 1; brush.RadiusY = 1;
            brush.GradientStops.Add(new GradientStop(Colors.Silver, 0.8));
            brush.GradientStops.Add(new GradientStop(Color.FromRgb(100,100,100), 0));
            return brush;
        }
    }
}
