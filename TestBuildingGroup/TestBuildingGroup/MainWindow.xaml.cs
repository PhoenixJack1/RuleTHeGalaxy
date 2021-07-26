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

namespace TestBuildingGroup
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static ImageBrush[,] images = GetImages();
        
        public MainWindow()
        {
            InitializeComponent();
            canvas.Background = new ImageBrush(new BitmapImage(new Uri("Image3.png", UriKind.Relative)));
            string s = System.IO.File.ReadAllText("sizes.txt");
            string[] ss = s.Split(' ');
            List<int> sss = new List<int>();
            foreach (string sc in ss)
                sss.Add(Int32.Parse(sc));
            
            PutLocation1(150, 150, canvas, 0, 355, sss[0],0);
            PutLocation1(150, 150, canvas, 180, 275, sss[1], 1);
            PutLocation1(150, 150, canvas, 300, 5,  sss[2], 2);
            PutLocation1(150, 150, canvas, 510, 450, sss[3], 3);
            PutLocation1(150, 150, canvas, 715, 335, sss[4], 4);
            PutLocation1(150, 150, canvas, 885, 35, sss[5], 0);
            PutLocation1(150, 150, canvas, 485, 305,  sss[6], 1);
            PutLocation1(150, 150, canvas, 830, 435, sss[7], 2);
            PutLocation1(150, 150, canvas, 350, 165, sss[8], 3);
            PutLocation1(150, 150, canvas, 625, 205, sss[9], 4);

        }
        static double cos30 = Math.Cos(30 / 180.0 * Math.PI);
        static double cos302 = 2 * cos30;
        static PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
        void PutLocation1(int width, int height, Canvas c, int left, int top, int bsize, int image)
        {
            Canvas canvas = new Canvas(); canvas.Width = width*cos302; canvas.Height = 2*height;
            c.Children.Add(canvas); Canvas.SetLeft(canvas, left); Canvas.SetTop(canvas, top - height);
            Path path = new Path(); //path.Stroke = Brushes.Brown; path.StrokeThickness = 3;
            Point P1 = new Point(0, height * 1.5); Point P2 = new Point(width * cos30, height);
            Point P3 = new Point(width * cos302, height * 1.5); Point P4 = new Point(width * cos30, height * 2);
            PathFigure figure = new PathFigure(); figure.StartPoint = P1; figure.Segments.Add(new LineSegment(P2, true));
            figure.Segments.Add(new LineSegment(P3, true)); figure.Segments.Add(new LineSegment(P4, true));
            figure.IsClosed = true;
            path.Data = new PathGeometry(new PathFigure[] { figure });
            //path.Fill = Brushes.LightGreen;
            canvas.Children.Add(path);
            int text = bsize;
            int[] sizes = new int[] { 1, 10, 50, 150 };
            int[] buildings = new int[] { 0, 0, 0, 0 };
            int[] dims = new int[] { 20, 30, 50, 60 };
            for (int i = 3; i >= 0; i--)
                if (bsize >= sizes[i]) { buildings[i]++; bsize -= sizes[i]; i++; }
            List<Point> Points = new List<Point>();
            for (int i = 5; i < width; i += 10)
                for (int j = 5; j < height; j += 10)
                    Points.Add(new Point(i, j));
            List<Rect> Rects = new List<Rect>();
            for (int i = 3; i >= 0; i--)
                for (int j = 0; j < buildings[i]; j++)
                {
                    int dim = dims[i];
                    ImageBrush img = images[image, i];
                    double dh = img.ImageSource.Height / (img.ImageSource.Width / 1.73) - 1;
                    Rectangle rect = new Rectangle(); rect.Width = dim*cos302; rect.Height = dim * (dh + 1);
                    rect.Fill = images[image, i];// rect.Stroke = Brushes.White;
                    canvas.Children.Add(rect);
                    Point pt = new Point(); int maxx = 5; int maxy = 5;
                    for (int k = 0; k < Points.Count; k++)
                    {
                        pt = Points[k];
                        maxx = (int)pt.X + dim;
                        maxy = (int)pt.Y + dim;
                        if (maxx > width || maxy > height) continue;
                        else
                        {
                            Rect cur = new Rect(pt.X, pt.Y, dim - 1, dim - 1);
                            bool error = false;
                            foreach (Rect r in Rects)
                            {
                                if (r.IntersectsWith(cur))
                                { error = true; break; }
                            }
                            if (error) continue; else break;
                        }
                    }
                    Rects.Add(new Rect(pt.X, pt.Y, dim - 1, dim - 1));
                    Point isopt = ConvertPT(pt, height, dim, dh);
                    Canvas.SetLeft(rect, isopt.X); Canvas.SetTop(rect, isopt.Y);
                    Canvas.SetZIndex(rect, (byte)(maxx/5));
                    List<Point> pts = new List<Point>();
                    foreach (Point p in Points)
                    {
                        if (p.X >= pt.X && p.X < maxx && p.Y >= pt.Y && p.Y < maxy)
                            continue;
                        pts.Add(p);
                    }
                    Points = pts;
                }
            
            Ellipse el = new Ellipse(); el.Width = 30; el.Height = 25; el.Stroke = Brushes.SkyBlue; el.StrokeThickness = 2;
            el.Fill = Brushes.Black; canvas.Children.Add(el); Canvas.SetLeft(el, width*cos30 - 15); Canvas.SetTop(el, height*2 - 30);
            TextBlock tb = new TextBlock(); tb.Text = text.ToString(); tb.Foreground = Brushes.White; canvas.Children.Add(tb);
            Canvas.SetLeft(tb, width*cos30 - 10); Canvas.SetTop(tb, height*2 - 25); tb.FontSize = 9; tb.FontWeight = FontWeights.Bold; tb.TextAlignment = TextAlignment.Center;
            tb.Width = 20;
            
        }
        Point ConvertPT(Point pt, int height, double size, double bh)
        {
            return new Point(pt.X * cos30 + pt.Y * cos30, height*1.5-size*0.5-bh*size + pt.X * 0.5 - pt.Y * 0.5);
        }
    public static ImageBrush[,] GetImages()
        {
            ImageBrush[,] result = new ImageBrush[12, 4];
            string[] texts = new string[] { "-20.png", "-30.png", "-50.png", "-60.png" };
            for (int i=0;i<12;i++)
                for (int j=0;j<4;j++)
                {
                    int k = (i % 5) + 1;
                    result[i, j] = new ImageBrush(new BitmapImage(new Uri(string.Format("Images/{0}{1}", k.ToString(), texts[j]), UriKind.Relative)));
                }
            return result;
        }
        void PutLocation(int width, int height, Canvas c, int left, int top, int bsize, int image)
        {
            Canvas canvas = new Canvas(); canvas.Width = width; canvas.Height = height;
            c.Children.Add(canvas); Canvas.SetLeft(canvas, left); Canvas.SetTop(canvas, top);
            canvas.RenderTransformOrigin = new Point(0.5, 0.5);
            canvas.RenderTransform = new SkewTransform(45, -35.264);

            Rectangle Back = new Rectangle(); Back.Width = width; Back.Height = height;
            canvas.Children.Add(Back); Back.Fill = Brushes.Gray; Back.Stroke = Brushes.Brown; Back.StrokeThickness = 3;
            
            int text = bsize;
            int[] sizes = new int[] { 1, 10, 50, 150 };
            int[] buildings = new int[] { 0, 0, 0, 0 };
            int[] dims = new int[] { 20, 30, 50, 60 };
            for (int i = 3; i >= 0; i--)
                if (bsize >= sizes[i]) { buildings[i]++; bsize -= sizes[i]; i++; }
            List<Point> Points = new List<Point>();
            for (int i = 5; i < width; i += 10)
                for (int j = 5; j < height; j += 10)
                    Points.Add(new Point(i, j));
            List<Rect> Rects = new List<Rect>();
            for (int i = 3; i >= 0; i--)
                for (int j = 0; j < buildings[i]; j++)
                {
                    int dim = dims[i];
                    Rectangle rect = new Rectangle(); rect.Width = dim; rect.Height = dim;
                    rect.Fill = images[image, i]; //rect.Stroke = Brushes.White;
                    canvas.Children.Add(rect);
                    Point pt = new Point(); int maxx = 5; int maxy = 5;
                    for (int k = 0; k < Points.Count; k++)
                    {
                        pt = Points[k];
                        maxx = (int)pt.X + dim;
                        maxy = (int)pt.Y + dim;
                        if (maxx > width || maxy > height) continue;
                        else
                        {
                            Rect cur = new Rect(pt.X, pt.Y, dim-1, dim-1);
                            bool error = false;
                            foreach (Rect r in Rects)
                            {
                                if (r.IntersectsWith(cur))
                                { error = true;  break; }
                            }
                            if (error) continue; else break;
                        }    
                    }
                    Rects.Add(new Rect(pt.X, pt.Y, dim-1, dim-1));
                    Canvas.SetLeft(rect, pt.X); Canvas.SetTop(rect, pt.Y);
                    
                    List<Point> pts = new List<Point>();
                    foreach (Point p in Points)
                    {
                        if (p.X >= pt.X && p.X < maxx && p.Y >= pt.Y && p.Y < maxy)
                            continue;
                        pts.Add(p);
                    }
                    Points = pts;
                }
            Ellipse el = new Ellipse(); el.Width = 30; el.Height = 25; el.Stroke = Brushes.SkyBlue; el.StrokeThickness = 2;
            el.Fill = Brushes.Black; canvas.Children.Add(el); Canvas.SetLeft(el, width-35); Canvas.SetTop(el, height-30);
            TextBlock tb = new TextBlock(); tb.Text = text.ToString(); tb.Foreground = Brushes.White; canvas.Children.Add(tb);
            Canvas.SetLeft(tb, width-30); Canvas.SetTop(tb, height-25); tb.FontSize = 9; tb.FontWeight = FontWeights.Bold; tb.TextAlignment = TextAlignment.Center;
            tb.Width = 20;
        }
    }
}
