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
    public enum RIType { Health, Shield, Energy}
    public class RoundIndicator : Canvas
    {
        GradientStop MoveStop1;
        GradientStop MoveStop2;
        GradientStop MoveStop3;
        public int Max { get; private set; } //максимальное значение индикатора
        public int Value { get; private set; } //текущее значение индикатора
        Color color;
        Path path;
        public RoundIndicator(int max, int value, RIType type)
        {
            path = new Path();
            path.Stroke = Brushes.Black;
            LinearGradientBrush brush = new LinearGradientBrush();
            color = Colors.Red;
            switch (type)
            {
                case RIType.Health:
                    path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 a10,10 0 0,0 10,5 a150,150 0 0,0 -10,130 h-5 a150,150 0 0,1 5,-135"));
                    brush.StartPoint = new Point(0.5, 1); brush.EndPoint = new Point(0.5, 0);
                    color = Colors.Red;
                    break;
                case RIType.Shield:
                    path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 a10,10 0 0,1 -10,5 a150,150 0 0,1 10,130 h5 a150,150 0 0,0 -5,-135"));
                    brush.StartPoint = new Point(0.5, 1); brush.EndPoint = new Point(0.5, 0);
                    color = Colors.Blue;
                    break;
                case RIType.Energy:
                    path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 a10,10 0 0,1 -5,10 a150,150 0 0,0 130,10 v-5 a150,150 0 0,1 -125,-15"));
                    brush.StartPoint = new Point(1, 0.5); brush.EndPoint = new Point(0, 0.5);
                    color = Colors.Green;
                    break;
            }
            Children.Add(path);
            brush.GradientStops.Add(new GradientStop(color, 0));
            MoveStop1 = new GradientStop(color, 1);
            brush.GradientStops.Add(MoveStop1);
            MoveStop2 = new GradientStop(Colors.White, 1);
            brush.GradientStops.Add(MoveStop2);
            MoveStop3 = new GradientStop(Colors.Black, 1);
            brush.GradientStops.Add(MoveStop3);
            path.Fill = brush;
            Max = max; Value = value;
            Calc();
        }
        public void SetBigSize()
        {
            if (color == Colors.Red)
                path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 a20,20 0 0,0 20,10 a300,300 0 0,0 -20,260 h-10 a300,300 0 0,1 10,-270"));
            else if (color == Colors.Blue)
                path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 a20,20 0 0,1 -20,10 a300,300 0 0,1 20,260 h10 a300,300 0 0,0 -10,-270"));
            else
                path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 a20,20 0 0,1 -10,20 a300,300 0 0,0 260,20 v-10 a300,300 0 0,1 -250,-30"));

        }
        void Calc()
        {
            double pos = (double)Value / Max;
            if (pos > 0.03) MoveStop1.Offset = pos - 0.03; else MoveStop1.Offset = pos;
            MoveStop2.Offset = pos;
            if (pos < 0.97) MoveStop3.Offset = pos + 0.03; else MoveStop3.Offset = pos;
        }
        public void SetSelecting(int value)
        {

        }
        public void SetMax(int value)
        {
            if (Max == value) return;
            Max = value;
            Calc();
        }
        public void RemoveSelecting()
        {

        }

        public void Move(int value) //новое значение показателя
        {
            if (value < 0) value = 0; //если новое значение меньше нуля то приравниваем его нулю
            if (value > Max) value = Max; //если новое значение больше максимального, то приравниваем его к максимальному
            if (value == Value) return; //если новое значение равно старому то ничего не делаем
            Value = value; //Запоминаем новое значение
            Calc();
        }

        /*public void RotateTo(double da)
        {


        }*/
        public void SetBig()
        {

        }

       /* public void SetAngle(double da)
        {
        }*/

        public void SetCorrupted()
        {

        }
        public void RemoveCorrupted()
        {

        }
    }
    
    public class RoundIndicator2 : Canvas
    {
        public int Max { get; private set; } //максимальное значение индикатора
        public int Value { get; private set; } //текущее значение индикатора
        Path Circle; //Основной рабочий путь
        EllipseGeometry Full; //Круг для отображения полной шкалы
        PathGeometry geom; //путь для отображения сектора
        ArcSegment arc; //основная дуга
        LineSegment l2; //линия для отображающая линию от конца дуги до центра круга 
        Point pt; //текущая рабочая точка на дуге
        Ellipse el;
        static Point TopPoint = new Point(15, 0); //верхняя точка
        static Point MiddlePoint = new Point(15, 15); //средняя точка
        static Size CurSize = new Size(15, 15); //размер сектора
        public static Brush CorruptedBrush = GetCorruptedBrush();
        Label CorruptedLabel;
        DoubleAnimation CorrAnim;
        double Angle = 0;
        RotateTransform TransformRotate;
        ScaleTransform TransformScale;
        public RoundIndicator2(int max, int value, Color color)
        {
            Width = 30; Height = 30;
            RenderTransformOrigin = new Point(0.5, 0.5);
            TransformGroup group = new TransformGroup();
            TransformRotate = new RotateTransform();
            group.Children.Add(TransformRotate);
            TransformScale = new ScaleTransform();
            group.Children.Add(TransformScale);
            RenderTransform = group;
            el = new Ellipse();
            el.Stroke = new SolidColorBrush(color);
            el.StrokeThickness = 2;
            el.Width = 30; el.Height = 30;
            Children.Add(el);
            el.Fill = Brushes.Black;
            //Margin = new Thickness(5);
            Max = max; Value = value; //сохранение начальных значенией
            pt = new Point(15, 0);
            Circle = new Path(); //объект для основного пути
            Children.Add(Circle); //размещение на канвасе
            RadialGradientBrush br = new RadialGradientBrush(); //заливка фигуры
            br.GradientOrigin = new Point(0.8, 0.2); //смещение центра заливки
            br.GradientStops.Add(new GradientStop(color, 1)); //основной цвет
            br.GradientStops.Add(new GradientStop(Colors.White, 0)); //вспомогательный цвет
            Circle.Fill = br; //присвоение заливки
            Circle.Stroke = Brushes.Black; //цвет контурной линии
            Circle.StrokeThickness = 0.2; //толщина контурной линии
            geom = new PathGeometry(); //объект для геометрии сектора
            PathFigure figure = new PathFigure(); //Фигура
            figure.StartPoint = TopPoint; //начальная точка сектора
            arc = new ArcSegment(); //дуга для сектора
            LineSegment l1 = new LineSegment(); //вертикальная линия сектора
            l2 = new LineSegment(); //вторая линия сектора
            figure.Segments.Add(l1);
            figure.Segments.Add(l2);
            figure.Segments.Add(arc);
            l1.Point = MiddlePoint;
            arc.Size = CurSize;
            arc.Point = TopPoint;
            arc.SweepDirection = SweepDirection.Clockwise;
            geom.Figures.Add(figure);
            Circle.Data = geom;
            Full = new EllipseGeometry(MiddlePoint, 15, 15);
            CalcPT();
            arc.IsLargeArc = value * 2 > Max;


            Select = new Path();

            Select.Stroke = Brushes.Black;
            Select.StrokeThickness = 1;
            PathGeometry SelectGeom = new PathGeometry();
            Select.Data = SelectGeom;
            PathFigure Sfigure = new PathFigure();
            Sfigure.StartPoint = new Point(15, 15);
            s1 = new LineSegment();
            SArc = new ArcSegment();
            LineSegment s2 = new LineSegment();
            Sfigure.Segments.Add(s1);
            Sfigure.Segments.Add(SArc);
            Sfigure.Segments.Add(s2);
            SArc.Size = new Size(15, 15);
            SArc.SweepDirection = SweepDirection.Clockwise;
            s2.Point = new Point(15, 15);

            SelectGeom.Figures.Add(Sfigure);
            Children.Add(Select);
            Select.Opacity = 0;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = TimeSpan.FromSeconds(1.0 / 60);

            CorruptedLabel = new Label();
            CorruptedLabel.Width = 30; CorruptedLabel.Height = 30;
            CorruptedLabel.Background = CorruptedBrush;
            Children.Add(CorruptedLabel);
            CorruptedLabel.Opacity = 0;
            CorrAnim = new DoubleAnimation(-2, 1, TimeSpan.FromSeconds(1));
            CorrAnim.DecelerationRatio = 1;
            CorrAnim.AutoReverse = true;
            CorrAnim.RepeatBehavior = RepeatBehavior.Forever;
        }
        Point Spt = new Point();
        Path Select;
        ArcSegment SArc;
        LineSegment s1;
        public void SetSelecting(int value)
        {
            double da = 360 * value / Max;
            double angle = 360 - 360.0 * (Value - value) / Max;
            Spt.X = 15 + 15 * Math.Sin(angle / 180 * Math.PI);
            Spt.Y = 15 - 15 * Math.Cos(angle / 180 * Math.PI);
            s1.Point = pt;
            SArc.Point = Spt;
            SArc.IsLargeArc = da > 180;
            if (value > Value) Select.Fill = Brushes.Gray;
            else Select.Fill = Brushes.Gold;
            Select.Opacity = 1;
        }
        public void SetMax(int value)
        {
            if (Max == value) return;
            Max = value;
            CalcPT();
        }
        public void RemoveSelecting()
        {
            Select.Opacity = 0;
        }
        void CalcPT()
        {
            double angle = 360 - 360.0 * Value / Max;
            pt.X = 15 + 15 * Math.Sin(angle / 180 * Math.PI);
            pt.Y = 15 - 15 * Math.Cos(angle / 180 * Math.PI);
            l2.Point = pt;
            arc.IsLargeArc = angle < 180;
            if (angle == 0) Visibility = Visibility.Hidden;// Circle.Data = Full;
            else Visibility = Visibility.Visible;// Circle.Data = geom;
            Angle2 = angle;
        }
        double Angle1;
        double Angle2;
        double Da;
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        public void Move(int value) //новое значение показателя
        {
            if (value < 0) value = 0; //если новое значение меньше нуля то приравниваем его нулю
            if (value > Max) value = Max; //если новое значение больше максимального, то приравниваем его к максимальному
            if (value == Value) return; //если новое значение равно старому то ничего не делаем
            Value = value; //Запоминаем новое значение
            Angle1 = Angle2; //в переменной angle2 сохранено текущее значение угла. запоминаем его в переменной angle1
            Circle.Data = geom; //присваеваем основному путю геометрию с сектором (на случай если была присвоена геометрия с кругом) 
            Angle2 = 360 - 360.0 * Value / Max; //новое значение угла
            timer.Stop();  //останавливаем таймер          
            Da = (Angle2 - Angle1) / 10; //шаг на который будет сдвигаться угол при каждой итерации
            timer.Start(); //запуск таймера
        }

        void timer_Tick(object sender, EventArgs e)
        {
            Angle1 += Da; //новое временное значение угла увеличивается на шаг итерации
            if (Da > 0 && Angle1 > Angle2) { timer.Stop(); CalcPT(); return; } //если шаг больше нуля и временное значение угла больше необходимого то стоп
            else if (Da < 0 && Angle1 < Angle2) { timer.Stop(); CalcPT(); return; } //если шаг меньше нуля и временное значение угла меньше необходимого - то стоп.
            Point p = new Point(); //Новая временная точка на дуге сектора
            p.X = 15 + 15 * Math.Sin(Angle1 / 180 * Math.PI); //расчёт координаты Х точки на дуге
            p.Y = 15 - 15 * Math.Cos(Angle1 / 180 * Math.PI); //расчёт координаты У на дуге
            l2.Point = p; //присвоение прямой новой временной координаты
            arc.IsLargeArc = Angle1 < 180; //если текущий угол больше 180 - то дуга большая, если меньше 180 - то маленькая.       
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
            Canvas.SetZIndex(this, 5);
            anim.Completed += new EventHandler(anim_Completed);
            anim.AutoReverse = true;
            TransformScale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            TransformScale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
        }

        void anim_Completed(object sender, EventArgs e)
        {
            Canvas.SetZIndex(this, 1);
        }
        public void SetAngle(double da)
        {
            double newangle = Angle + da;
            DoubleAnimation anim = new DoubleAnimation(newangle, Links.ZeroTime);
            TransformRotate.BeginAnimation(RotateTransform.AngleProperty, anim);
            //Transform.Angle = newangle;
            Angle = newangle;
        }
        static Brush GetCorruptedBrush()
        {
            PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            PathGeometry ellipse = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M14.95,0 a15,15 0 1,0 0.1,0"));
            GeometryDrawing ellipsedrawing = new GeometryDrawing(Brushes.Black, new Pen(), ellipse);
            LinearGradientBrush fillbrush = new LinearGradientBrush();
            fillbrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.8));
            fillbrush.GradientStops.Add(new GradientStop(Colors.Green, 0.5));
            fillbrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.2));
            PathGeometry figure = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M15,6 l-10,10 h10 l-10,10 l20,-14 h-10 l6,-6z"));
            GeometryDrawing figuredrawing = new GeometryDrawing(fillbrush, new Pen(), figure);
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(ellipsedrawing); group.Children.Add(figuredrawing);
            DrawingBrush brush = new DrawingBrush(group);
            return brush;
        }
        public void SetCorrupted()
        {
            CorruptedLabel.BeginAnimation(Label.OpacityProperty, CorrAnim);
            SetBig();
        }
        public void RemoveCorrupted()
        {
            CorruptedLabel.BeginAnimation(Label.OpacityProperty, null);
        }
    }
    
}
