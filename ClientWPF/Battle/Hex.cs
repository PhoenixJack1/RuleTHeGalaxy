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
    public enum BrightSelect { None, Move, Jump, Attack, Target}
    public class Hex:IComparable
    {
        static double Angle30 = 30.0 / 180 * Math.PI;
        static double Sin30 = Math.Sin(Angle30);
        static double Cos30 = Math.Cos(Angle30);
        public static double W = 300;
        public static double L = W / (1.0 + 2 * Sin30);
        public static double LSin = L * Sin30;
        public static double LCos = L * Cos30;
        public static double H = 2 * LCos;
        public Path path;
        public Canvas canvas;
        public Rectangle rect;
        public byte Column;
        public byte Row;
        public double CenterX;
        public double CenterY;
        public Point CenterPoint;
        public byte ID;
        //public static Hex[] Hexes = FillBasicHexes();
        public Hex[] NearHexes;
        public bool IsBright = false;
        public bool IsSelected = false;
        public static Hex SelectedHex = null;
        public bool IsGate = false;
        public static Brush Green1 = Gets.GetIntBoyaImage("HexGreen");// new ImageBrush(new BitmapImage(new Uri("C:/123/HexGreen.png")));
        public static Brush Violet1 = Gets.GetIntBoyaImage("HexPurple"); //new ImageBrush(new BitmapImage(new Uri("C:/123/HexPurple.png")));
        public static Brush Red1 = Gets.GetIntBoyaImage("HexRed");// new ImageBrush(new BitmapImage(new Uri("C:/123/HexRed.png")));
        public static Brush Blue1 = Gets.GetIntBoyaImage("Hex");// new ImageBrush(new BitmapImage(new Uri("C:/123/Hex.png")));
        public static Brush OppBrush = GetOppBrush();
        public static Brush Green = new SolidColorBrush(Color.FromArgb(255, 0, 96, 0));
        public static Brush Violet = new SolidColorBrush(Color.FromArgb(255, 96, 0, 96));
        public static Brush Red = new SolidColorBrush(Color.FromArgb(255, 96, 0, 0));
        public static Brush SkyBlue = new SolidColorBrush(Color.FromArgb(255, 192, 192, 255));
        public static Brush BrightGreen = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
        public static Brush BrightRed = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
        public static Brush BrightViolet = new SolidColorBrush(Color.FromArgb(255, 255, 0, 255));
        public static Brush BrightSkyBlue = new SolidColorBrush(Color.FromArgb(255, 192, 192, 255));
        public Hex(byte hex, byte row, byte column)
        {
            ID = hex;
            Row = row;
            Column = column;
            double deltax = Column * (LSin + L);
            double deltay;
            if (Column % 2 == 0)
                deltay = Row * 2 * LCos;
            else
                deltay = LCos + Row * 2 * LCos;
            CenterX = Column * (L + LSin) + 2 * LSin;
            CenterY = Row * 2 * LCos + LCos + Column % 2 * LCos;
            CenterPoint = new Point(CenterX, CenterY);
        }
        public int CompareTo(object b)
        {
            Hex hex = (Hex)b;
            if (hex.ID > ID) return 1;
            else if (hex.ID < ID) return -1;
            else return 0;
        }
        static RadialGradientBrush GetOppBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.RadiusY = 0.6;
            brush.GradientStops.Add(new GradientStop(Colors.White, 1));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(50, 0, 0, 0), 0.7));
            return brush;
        }
        public void CreatePath()
        {
            path = new Path();
            Canvas.SetZIndex(path, Links.ZIndex.Hexes);
            //path.Fill = SkyBlue;
            //path.Fill = Blue1;
            //path.Fill = Brushes.SkyBlue;
            //path.Stroke = Brushes.Black;
            //path.StrokeThickness = 3;
            PathFigure fig = new PathFigure();
            fig.StartPoint = new Point(W/2 - L / 2, H/2 - LCos);
            fig.Segments.Add(new LineSegment(new Point(W/2 + L / 2, H/2 - LCos), true));
            fig.Segments.Add(new LineSegment(new Point(W/2+ LSin + L / 2, H / 2), true));
            fig.Segments.Add(new LineSegment(new Point(W / 2 + L / 2, H / 2 + LCos), true));
            fig.Segments.Add(new LineSegment(new Point(W / 2 - L / 2, H / 2 + LCos), true));
            fig.Segments.Add(new LineSegment(new Point(W / 2 - L / 2 - LSin, H / 2), true));

            //fig.StartPoint = new Point(CenterX-L/2, CenterY-LCos);
            //fig.Segments.Add(new LineSegment(new Point(CenterX + L / 2, CenterY - LCos), true));
            //fig.Segments.Add(new LineSegment(new Point(CenterX + LSin + L / 2, CenterY), true));
            //fig.Segments.Add(new LineSegment(new Point(CenterX + L / 2, CenterY + LCos), true));
            //fig.Segments.Add(new LineSegment(new Point(CenterX - L / 2, CenterY + LCos), true));
            //fig.Segments.Add(new LineSegment(new Point(CenterX - L / 2 - LSin, CenterY), true));
            fig.IsClosed = true;
            PathGeometry geom = new PathGeometry();
            geom.Figures.Add(fig);
            path.Data = geom;
            path.Fill = Links.Brushes.Transparent;
            canvas = new Canvas(); canvas.Width = W; canvas.Height = H;
            Canvas.SetZIndex(canvas, Links.ZIndex.Hexes);
            Canvas.SetLeft(canvas, CenterX - W / 2); Canvas.SetTop(canvas, CenterY - H / 2);
            rect = new Rectangle(); rect.Width = W * 1.3667; rect.Height = H * 1.3462;
            canvas.Children.Add(rect); Canvas.SetLeft(rect, -55); Canvas.SetTop(rect, -45);
            canvas.Children.Add(path);
            
        }
        public void SetSelect()
        {
            if (SelectedHex == this)
            {
                IsSelected = false;
                SelectedHex = null;
            }
            else
            {
                if (SelectedHex != null)
                {
                    SelectedHex.IsSelected = false;
                }
                SelectedHex = this;
                IsSelected = true;
            }
        }
        public void SetGateBrush(bool IsAttack)
        {
            if (IsAttack)
                rect.Fill = Red1;
            else
                rect.Fill = Green1;
        }
        public void SetBright(BrightSelect type)
        {
            if (type == BrightSelect.Move)
            {
                IsBright = true;
                rect.Fill = Green1;
                Canvas.SetZIndex(canvas, Links.ZIndex.TopHexes);
                //path.Fill = Green1;
            }
            else if (type == BrightSelect.Jump)
            {
                IsBright = true;
                rect.Fill = Violet1;
                Canvas.SetZIndex(canvas, Links.ZIndex.TopHexes);
                //path.Fill = Violet1;
            }
            else if (type == BrightSelect.Attack)
            {
                IsBright = true;
                path.Stroke = Brushes.Gold;
                path.Fill = Brushes.Gold;
                //path.StrokeThickness = 5;
            }
            else if (type == BrightSelect.Target)
            {
                IsBright = true;
                rect.Fill = Red1;
                Canvas.SetZIndex(canvas, Links.ZIndex.TopHexes);
                //path.Fill = Red1;

            }
            else
            {
                IsBright = false;
                rect.Fill = Blue1;
                Canvas.SetZIndex(canvas, Links.ZIndex.Hexes);
                //path.Fill = Blue1;

            }
        }
       
        public void SetD(double dx, double dy)
        {
            CenterX = CenterX + dx; CenterY = CenterY + dy;
            CenterPoint = new Point(CenterX, CenterY);
        }
        public void PlaceShip(Canvas ship)
        {
           
                Canvas.SetLeft(ship, CenterX - ship.Width / 2);

                Canvas.SetTop(ship, CenterY - ship.Height / 2);
                DoubleAnimation leftanim = new DoubleAnimation(CenterX - ship.Width / 2, Links.ZeroTime);
                DoubleAnimation topanim = new DoubleAnimation(CenterY - ship.Height / 2, Links.ZeroTime);
                ship.BeginAnimation(Canvas.LeftProperty, leftanim);
                ship.BeginAnimation(Canvas.TopProperty, topanim);
                
        }
        static PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
        
        public static Path GetHexPath(double width)
        {
            Path path = new Path(); path.Stroke = Brushes.White; path.StrokeThickness = 2;
            double L = width / (2 * Sin30 + 1);
            double LCos = L * Cos30;
            double LSin = L * Sin30;
            PathGeometry geom = new PathGeometry();
            PathFigure fig = new PathFigure();
            fig.StartPoint = new Point(0, LCos);
            fig.Segments.Add(new LineSegment(new Point(LSin, 0), true));
            fig.Segments.Add(new LineSegment(new Point(L + LSin, 0), true));
            fig.Segments.Add(new LineSegment(new Point(L + 2 * LSin, LCos), true));
            fig.Segments.Add(new LineSegment(new Point(L + LSin, 2 * LCos), true));
            fig.Segments.Add(new LineSegment(new Point(LSin, 2 * LCos), true));
            fig.IsClosed = true;
            geom.Figures.Add(fig);
            path.Data = geom;
            return path;
        }
        public static Canvas Get4HexesCanvas(double width)
        {
            Canvas canvas = new Canvas();
            double L = width / (2 * Sin30 + 1);
            double LCos = L * Cos30;
            double LSin = L * Sin30;
            Path[] hexes = new Path[4];
            for (int i = 0; i < 4; i++)
            {
                hexes[i] = GetHexPath(width);
                canvas.Children.Add(hexes[i]);
            }
            Canvas.SetLeft(hexes[0], L + LSin);
            Canvas.SetTop(hexes[1], LCos);
            Canvas.SetLeft(hexes[2], L + LSin);
            Canvas.SetTop(hexes[2], 2 * LCos);
            Canvas.SetLeft(hexes[3], width + L);
            Canvas.SetTop(hexes[3], LCos);
            hexes[0].Fill = Brushes.Red;
            hexes[1].Fill = Brushes.Black;
            hexes[2].Fill = Brushes.Green;
            hexes[3].Fill = Brushes.Black;
            canvas.Width = 2 * width + L;
            canvas.Height = 4 * LCos;
            return canvas;
        }
        public Point GetGunPoint(int gun, double pos)
        {
            double baseangle = pos;
            int divangle, length;
            if (gun > 2) { divangle = 30; length = 240; gun -= 3; } else { divangle = 45; length = 120; }
            double curangle = baseangle + divangle * (gun - 1);
            double sina = length * Math.Sin(curangle / 180 * Math.PI);
            double cosa = -length * Math.Cos(curangle / 180 * Math.PI);
            return new Point(CenterX + sina, CenterY + cosa);
        }
        
     
    }
    /*
    struct MLine
    {
        public double K;
        public double B;
        public double X1;
        public double Y1;
        public double X2;
        public double Y2;
        public bool IsVertical;
        public bool IsHorizontal;
        public MLine(double x1, double y1, double x2, double y2)
        {
            IsVertical = false; IsHorizontal = false;
            if (x1 == x2)
            {
                IsVertical = true;
                if (y1 < y2)
                { X1 = x1; Y1 = y1; X2 = x2; Y2 = y2; }
                else
                { X1 = x2; Y1 = y2; X2 = x1; Y2 = y1; }
            }
            else if (y1 == y2)
            {
                IsHorizontal = true;
                if (x1 < x2)
                { X1 = x1; Y1 = y1; X2 = x2; Y2 = y2; }
                else
                { X1 = x2; Y1 = y2; X2 = x1; Y2 = y1; }
            }
            else
            {
                if (x1 < x2)
                { X1 = x1; Y1 = y1; X2 = x2; Y2 = y2; }
                else
                { X1 = x2; Y1 = y2; X2 = x1; Y2 = y1; }

            }

            K = (Y1 - Y2) / (X1 - X2);
            B = Y2 - K * X2;
        }
        public bool IsParrallel(MLine line)
        {
            if (IsVertical && line.IsVertical) return true;
            if (IsHorizontal && line.IsHorizontal) return true;
            if (K == line.K) return true; else return false;
        }
        public bool IsIntersect(MLine line)
        {
            if (IsParrallel(line)) return false;
            double X = (line.B - B) / (K - line.K);
            if (X < X1 || X > X2) return false;
            if (X < line.X1 || X > line.X2) return false;
            if (IsHorizontal && line.IsVertical)
                if (Y1 >= line.Y1 && Y1 <= line.Y2 && X1 <= line.X1 && X2 >= line.X1) return true; else return false;
            else if (IsVertical && line.IsHorizontal)
                if (X1 >= line.X1 && X1 <= line.X2 && Y1 <= line.Y1 && Y2 >= line.Y1) return true; else return false;
            else if (line.IsVertical)
            { double Y = K * line.X1 + B; if (Y >= line.Y1 && Y <= line.Y2 && ((Y >= Y1 && Y <= Y2) || (Y >= Y2 && Y <= Y1))) return true; else return false; }
            else if (IsVertical)
            { double Y = line.K * X1 + line.B; if (Y >= line.Y1 && Y <= line.Y2 && Y >= Y1 && Y <= Y2) return true; else return false; }
            return true;
        }
    
    }
    */
}
