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

namespace Client
{
    
    class RoundButtons : Canvas
    {
        public Ellipse Circle;
        double Diam;
        GSEllipse Ell;
        List<Point> Points;
        List<GSUIElement> NewButtons;
        List<GSUIElement> Buttons;
        public RoundButtons()
        {
            Grid.SetRowSpan(this, 2);
            Grid.SetRow(this, 1);
            ClipToBounds = true;
        }
        public void SetSize()
        {
            this.Width = Links.Controller.mainWindow.mainGrid.ActualWidth;
            this.Height = Links.Controller.mainWindow.mainGrid.ActualHeight;
        }
        public void Place()
        {
            if (!Links.Controller.mainWindow.mainGrid.Children.Contains(this))
                Links.Controller.mainWindow.mainGrid.Children.Add(this);
        }
        public void Remove()
        {
            Children.Clear();
            if (Links.Controller.mainWindow.mainGrid.Children.Contains(this))
                Links.Controller.mainWindow.mainGrid.Children.Remove(this);
        }
        public void Create(FrameworkElement element, List<GSButton> elements, bool IsBattle)
        {
            List<GSUIElement> list = new List<GSUIElement>();
            foreach (GSButton btn in elements)
                list.Add(btn);
            Create(element,list, IsBattle);
        }
        public void Create(FrameworkElement element, List<GSUIElement> elements, bool IsBattle)
        {
            SetSize();
            Point StartPoint;
            if (IsBattle)
            {
                HexShip ship = (HexShip)element;
                Point SP = element.PointToScreen(new Point(ship.Width/2, ship.Height/2));
                StartPoint = Links.Controller.mainWindow.mainGrid.PointFromScreen(SP);
            }
            else
                StartPoint = element.TranslatePoint(new Point(0, -40), Links.Controller.mainWindow.mainGrid);
            Point Center;
            if (IsBattle)
                Center = new Point(StartPoint.X, StartPoint.Y);
            else
                Center = new Point(StartPoint.X + element.Width / 2, StartPoint.Y + element.Height / 2);
            Diam = Math.Sqrt(element.Width * element.Width + element.Height * element.Height) + 10;
            Circle = new Ellipse();
            Circle.Stroke = Brushes.Black;
            Circle.StrokeThickness = 3;
            Children.Add(Circle);
            Circle.Width = Diam; Circle.Height = Diam;
            Canvas.SetLeft(Circle, Center.X - Diam / 2);
            Canvas.SetTop(Circle, Center.Y - Diam / 2);
            Background = new SolidColorBrush(Color.FromArgb(150, 255, 255, 255));
            Ell = new GSEllipse(Center.X, Center.Y, Diam / 2);
            NewButtons = elements;

            Path Back = new Path();
            CombinedGeometry comb = new CombinedGeometry();
            comb.Geometry1 = new RectangleGeometry(new Rect(new Point(0, 0), new Point(Width, Height)));
            comb.Geometry2 = new EllipseGeometry(new Point(Canvas.GetLeft(Circle) + Diam / 2, Canvas.GetTop(Circle) + Diam / 2), Diam / 2, Diam / 2);
            comb.GeometryCombineMode = GeometryCombineMode.Exclude;
            Back.Data = comb;
            Back.Fill = Brushes.LightGray;
            Children.Add(Back);
            Points = new List<Point>();
            for (int i = 0; i < Width; i++)
            {
                if (i % 5 != 0) continue;
                for (int j = 0; j < Height; j++)
                {
                    if (j % 5 != 0) continue;
                    if (Ell.InEllipse(i, j)) continue;
                    //if (Ell.GetDistance(i, j) > Ell.R + 50) continue;
                    Points.Add(new Point(i, j));
                }
            }
            Buttons = new List<GSUIElement>();
            foreach (GSUIElement btn in NewButtons)
                AddButton(btn);
        }
        /// <summary> метод размещает кнопку на форме в зависимости от её размеров, положения центрального круга и других кнопок </summary>
        void AddButton(GSUIElement btn)
        {
            //начальные желаемые углы между которыми должна быть расположена образующая
            
            double minangle = 15;
            double maxangle = 20;
            double scale = 1;
            //если кнопки на форме уже есть, то берём не базовые углы, а максимальный угол последней кнопки
            if (Buttons.Count != 0)
            {
                GSUIElement lastbtn = Buttons[Buttons.Count - 1]; //последняя кнопка расположенная на форме
                minangle = Ell.GetMaxAngle(lastbtn); //определение максимального угла последней кнопки и запись его как минимальный 
                maxangle = minangle + 5;
            }
            double startmin = minangle; 
            //цикл по подбору параметров
            for (; ; )
            {
                //список точек, расположенных между образующих, их вычисление
                List<Point> CurPoints = new List<Point>();
                foreach (Point pt in Points)
                {
                    double angle = Ell.GetAngle(pt.X, pt.Y);
                    if (angle > minangle && angle < maxangle)
                        CurPoints.Add(pt);
                }
                //начальные параметры - точка,угол объекта от которого вычисляется минимальное расстояние до центра круга и расстояние от центра круга до обхекта
                Point minpt = new Point(-1, -1);
                int mincorner = 0;
                double mindist = -1;
                //пробегаемся по всем точкам, расположенным между образующими
                foreach (Point pt in CurPoints)
                {
                    //Ellipse el = new Ellipse(); el.Fill = Brushes.Blue; el.Width = 1; el.Height = 1;
                    //canvas.Children.Add(el); Canvas.SetLeft(el, pt.X-0.5); Canvas.SetTop(el, pt.Y-0.5); Canvas.SetZIndex(el, 2);
                    for (int i = 0; i < 4; i++) //пробегаемся по четырём углам
                    {
                        btn.SetPoint(pt.X, pt.Y, i); //распологаем объект в точке в зависимости от выбранного угла
                        if (btn.X1 < 0 || btn.Y1 < 0 || btn.Y2 > Height || btn.X2 > Width) continue; //если крайние точки объекта находятся за пределом формы, то этот угол не подходит
                        if (btn.Intersect(Ell)) continue; //если круг пересекается с объектом, то этот угол не подходит
                        bool IsButtonsIntersects = false; //переменная для определения пересечения с другими кнопками формы
                        Rect CurRect = new Rect(btn.X1, btn.Y1, btn.Width, btn.Height); //прямоугольник который занимает новая кнопка
                        foreach (GSUIElement old in Buttons) //пробегаем по всем кнопкам формы
                        {
                            Rect OldRect = new Rect(old.X1, old.Y1, old.Width, old.Height);  //прямоугольник который занимает старая кнопка
                            if (OldRect.IntersectsWith(CurRect)) { IsButtonsIntersects = true; break; }
                        }
                        if (IsButtonsIntersects) continue;
                        if (mindist == -1) { mindist = Ell.GetBtnDistance(btn); minpt = pt; mincorner = i; }
                        else
                        {
                            double curdist = Ell.GetBtnDistance(btn);
                            if (curdist < mindist) { mindist = curdist; minpt = pt; mincorner = i; }
                        }
                    }
                }
                if (minpt.X != -1)
                {
                    btn.SetPoint(minpt.X, minpt.Y, mincorner);
                    Children.Add(btn.Element);
                    Canvas.SetZIndex(btn.Element, 2);
                    Buttons.Add(btn);
                    
                    break;
                }
                else
                {
                    minangle = (minangle + 5) % 360;
                    maxangle = (maxangle + 5) % 360;
                    if (minangle == startmin)
                    {
                        scale*=0.95;
                        ScaleTransform transform = new ScaleTransform(scale, scale);
                        btn.Element.RenderTransform = transform;
                        btn.Width *= scale;
                        btn.Height *= scale;
                    }
                }

            }
        }
    }
    class GSUIElement
    {
        public double Width;
        public double Height;
        public double X1;
        public double X2;
        public double Y1;
        public double Y2;
        public UIElement Element;
        public GSUIElement(double width, double height, UIElement element)
        {
            Width = width;
            Height = height;
            
            X1 = 0; X2 = 0; Y1 = 0; Y2 = 0;
            if (element != null)
            {
                Element = element;
                SetPoint(0, 0, 0);
            }
        }
        public void SetPoint(double x1, double y1, int corner)
        {
            switch (corner)
            {
                case 0: X1 = x1; Y1 = y1; X2 = X1 + Width; Y2 = Y1 + Height; break;
                case 1: X1 = x1; Y1 = y1 - Height; X2 = x1 + Width; Y2 = y1; break;
                case 2: X1 = x1 - Width; Y1 = y1; X2 = x1; Y2 = y1 + Height; break;
                default: X1 = x1 - Width; Y1 = y1 - Height; X2 = x1; Y2 = y1; break;
            }
            Canvas.SetLeft(Element, X1);
            Canvas.SetTop(Element, Y1);
        }
        Rectangle GetGSRect()
        {
            Rectangle rect = new Rectangle();
            rect.Fill = Brushes.Black;
            rect.Width = Width;
            rect.Height = Height;
            Canvas.SetLeft(rect, X1);
            Canvas.SetTop(rect, Y1);
            return rect;
        }
        public bool Intersect(GSEllipse el)
        {
            if (InRectangle(X1 - el.R, Y1 - el.R, X2 + el.R, Y2 + el.R, el.X, el.Y))
            {
                //Если центр круга принадлежит большому квадрату
                if (InRectangle(X1 - el.R, Y1, X2 + el.R, Y2, el.X, el.Y)
                    || InRectangle(X1, Y1 - el.R, X2, Y2 + el.R, el.X, el.Y))
                    return true; //если центр круга принадлежит промежуточному квадрату то есть пересечение
                else if (GetDistance(X1, Y1, el.X, el.Y) < el.R ||
                GetDistance(X1, Y2, el.X, el.Y) < el.R ||
                GetDistance(X2, Y1, el.X, el.Y) < el.R ||
                GetDistance(X2, Y2, el.X, el.Y) < el.R) return true; //Если центры вершин прямоугольника находятся внутри круга
                else return false;
            }
            else if (GetDistance(X1, Y1, el.X, el.Y) < el.R ||
                GetDistance(X1, Y2, el.X, el.Y) < el.R ||
                GetDistance(X2, Y1, el.X, el.Y) < el.R ||
                GetDistance(X2, Y2, el.X, el.Y) < el.R) return true; //Если центры вершин прямоугольника находятся внутри круга
            else return false; //если ни вершины прямоугольника не принадлежат квадрату, ни центр круга не принадлежит большому квардрату
        }
        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
        public static bool InRectangle(double x1, double y1, double x2, double y2, double x0, double y0)
        {
            if (x0 > x1 && x0 < x2 && y0 > y1 && y0 < y2) return true;
            else return false;
        }
    }
    class GSButton:GSUIElement
    {
        public string Text;
        public GSButton(double width, double height, string text, RoutedEventHandler ClickEvent, object tag)
            :base(width,height,null)
        {
            Text = text;
            Button Rect = new Button();
            Rect.Width = width;
            Rect.Height = height;
            Rect.Content = Text;
            Rect.Style = Links.ButtonStyle;
            if (ClickEvent != null)
                Rect.Click += ClickEvent;
            Element = Rect;
            SetPoint(0, 0, 0);
            if (tag!=null)
                Rect.Tag = tag;
        }

        public static void btn_Cancel(object sender, RoutedEventArgs e)
        {
            Links.Controller.RoundButtons.Remove();
        }
        public static GSButton GetCancelButton()
        {
            return new GSButton(100, 50, Links.Interface("Cancel"), btn_Cancel, null);
        }
        


    }
    struct GSEllipse
    {
        public double X;
        public double Y;
        public double R;
        public GSEllipse(double x, double y, double radius)
        {
            X = x; Y = y; R = radius;
        }
        public bool InEllipse(double x, double y)
        {
            if (Math.Sqrt(Math.Pow(X - x, 2) + Math.Pow(Y - y, 2)) < R) return true;
            else return false;
        }
        public double GetAngle(double x, double y)
        {
            double dy = Y - y;
            double dx = X - x;
            double tga = dy / dx;
            double a = Math.Atan(tga) * 180 / Math.PI;
            if (dx < 0) a = 180 + a;
            else if (dy < 0) a = 360 + a;
            //if (a < 45) return 315 + a;
            //else return a - 45;
            return a;
        }
        public double GetMinAngle(GSUIElement btn)
        {
            double angle1 = GetAngle(btn.X1, btn.Y1);
            double angle2 = GetAngle(btn.X1, btn.Y2);
            double angle3 = GetAngle(btn.X2, btn.Y1);
            double angle4 = GetAngle(btn.X2, btn.Y2);
            double angle = angle1;
            if (angle2 < angle) angle = angle2;
            if (angle3 < angle) angle = angle3;
            if (angle4 < angle) angle = angle4;
            return angle;
        }
        public double GetMaxAngle(GSUIElement btn)
        {
            double angle1 = GetAngle(btn.X1, btn.Y1);
            double angle2 = GetAngle(btn.X1, btn.Y2);
            double angle3 = GetAngle(btn.X2, btn.Y1);
            double angle4 = GetAngle(btn.X2, btn.Y2);
            double angle = angle1;
            if (angle2 > angle) angle = angle2;
            if (angle3 > angle) angle = angle3;
            if (angle4 > angle) angle = angle4;
            return angle;
        }
        public double GetDistance(double x, double y)
        {
            return Math.Sqrt(Math.Pow(X - x, 2) + Math.Pow(Y - y, 2));
        }
        public double GetBtnDistance(GSUIElement btn)
        {
            double dist1 = GetDistance(btn.X1, btn.Y1);
            double dist2 = GetDistance(btn.X1, btn.Y2);
            double dist3 = GetDistance(btn.X2, btn.Y1);
            double dist4 = GetDistance(btn.X2, btn.Y2);
            if (dist2 < dist1) dist1 = dist2;
            if (dist3 < dist1) dist1 = dist3;
            if (dist4 < dist1) dist1 = dist4;
            return dist1;
        }
    }
    
}
