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
    /*class Science2Canvas:Canvas
    {
        public Viewbox VBX;

        public ElementsCanvas _ElementsCanvas;
        public ScienceListBorder ScienceBorder;
        public Science2Canvas()
        {
            Background = Brushes.Black;
            Width = 1920;
            Height = 1080;
            ClipToBounds = true;
            VBX = new Viewbox();
            VBX.Child = this;
        }
        public void Select()
        {
            Gets.GetTotalInfo("После откртыия панели науки");
            Gets.GetSciencePriceBases();
            Children.Clear();
            _ElementsCanvas = new ElementsCanvas();
            Children.Add(_ElementsCanvas);
            _ElementsCanvas.Play();
            ScienceAnalys.Fill2();
            Gets.GetSciencePriceBases();
            //Links.Controller.MainCanvas.LowBorder.Child = VBX;
        }
       
        public static Rectangle GetRectangle(int width, int height, int left, int top, Brush Fill, int z, double opacity)
        {
            Rectangle rect = new Rectangle();
            rect.Width = width; rect.Height = height;
            Canvas.SetLeft(rect, left); Canvas.SetTop(rect, top);
            rect.Fill = Fill;
            Canvas.SetZIndex(rect, z);
            rect.Opacity = opacity;
            return rect;
        }
        public class ElementsCanvas : Canvas
        {
            RoundCanvas Round;
            Canvas UpPart;
            Rectangle LowerRectangle;
            Rectangle UpperRectangle;
            Rectangle Hand1, Hand2, Hand3, Hand4;
            Rectangle LeftBorder, RightBorder;
            Ellipse Glass;
            public bool IsRightBorder = false;
            Viewbox ScienceVBX;
            public PricePanel PricePanel;
            public ElementsCanvas()
            {
                Width = 1920; Height = 1080;
                Round = new RoundCanvas();
                UpPart = new Canvas();
                Children.Add(UpPart);
                Rectangle wall = Science2Canvas.GetRectangle(1920, 628, 0, 0, Links.Brushes.Science.WallUp2, 5, 1);
                UpPart.Children.Add(wall);
                UpperRectangle = Round.GetUpperRectangle();
                UpPart.Children.Add(UpperRectangle);
                Canvas.SetZIndex(UpperRectangle, 10);

                Glass = new Ellipse(); Glass.Width = 500; Glass.Height = 500; Canvas.SetLeft(Glass, 708); Canvas.SetTop(Glass, 284);
                Glass.Fill = Links.Brushes.Science.Glass; Canvas.SetZIndex(Glass, 250); 
                //Glass = Science2Canvas.GetRectangle(600, 600, 652, 234, Links.Brushes.Science.Glass, 250, 1);
                //Glass = Science2Canvas.GetRectangle(600, 600, 652, 234, Brushes.Red, 250, 1);

                UpPart.Children.Add(Glass);

                LowerRectangle = Round.GetLowerRectangle();
                Children.Add(LowerRectangle);
                Canvas.SetZIndex(LowerRectangle, 20);

                Hand1 = Science2Canvas.GetRectangle(500, 500, 710, 279, Links.Brushes.Science.Hand1, 0, 0);
                UpPart.Children.Add(Hand1);
                Hand2 = Science2Canvas.GetRectangle(500, 500, 710, 279, Links.Brushes.Science.Hand2, 0, 0);
                UpPart.Children.Add(Hand2);
                Hand3 = Science2Canvas.GetRectangle(500, 500, 710, 279, Links.Brushes.Science.Hand3, 0, 0);
                UpPart.Children.Add(Hand3);
                Hand4 = Science2Canvas.GetRectangle(500, 500, 710, 279, Links.Brushes.Science.Hand4, 0, 0);
                UpPart.Children.Add(Hand4);

                LeftBorder = Science2Canvas.GetRectangle(615, 850, 0, -850, Links.Brushes.Science.Border, 20, 1);
                Children.Add(LeftBorder);

                RightBorder = Science2Canvas.GetRectangle(615, 850, 1300, -850, Links.Brushes.Science.Border, 20, 1);
                Children.Add(RightBorder);

                ScienceVBX = new Viewbox();
                ScienceVBX.Width = 580; ScienceVBX.Height = 650;
                Children.Add(ScienceVBX);
                Canvas.SetLeft(ScienceVBX, 1318);
                Canvas.SetTop(ScienceVBX, 200);
                Canvas.SetZIndex(ScienceVBX, 25);


                BigWindow.ShowWindow(this);
                Canvas.SetZIndex(BigWindow.ThisWindow, 100);
                Canvas.SetLeft(BigWindow.ThisWindow, 40);
                Canvas.SetTop(BigWindow.ThisWindow, 200);

            }
            public void Play()
            {
                new MySound("Interface/Menu_science_open.wav");
                DoubleAnimation anim = new DoubleAnimation(-700, 0, TimeSpan.FromSeconds(0.5));
                UpPart.BeginAnimation(Canvas.TopProperty, anim);

                DoubleAnimation anim1 = new DoubleAnimation(1080, 228, TimeSpan.FromSeconds(0.5));
                anim1.Completed += Anim1_Completed;
                LowerRectangle.BeginAnimation(Canvas.TopProperty, anim1);
            }
            public void LearnNewScinece(GameScience science, bool needget)
            {
                UIElement element = new GameObjectImage(GOImageType.Standard, science.ID);
                ScienceVBX.Child = element; //  canvas;
                DoubleAnimation oppanim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(2));
                element.BeginAnimation(Viewbox.OpacityProperty, oppanim);
                if (needget)
                {
                    Gets.GetResources();
                    Gets.GetTotalInfo("После изучения технологии");
                    ScienceAnalys.Fill2();
                    Gets.GetSciencePriceBases();
                    switch (pos)
                    {
                        case 1: PricePanel.ChangePrice(SciencePrice.GetSciencePrice(EScienceField.Cybernetic)); break;
                        case 2: PricePanel.ChangePrice(SciencePrice.GetSciencePrice(EScienceField.Irregular)); break;
                        case 3: PricePanel.ChangePrice(SciencePrice.GetSciencePrice(EScienceField.Energy));break;
                        default: PricePanel.ChangePrice(SciencePrice.GetSciencePrice(EScienceField.Physic)); break;
                    }
                }
            }
            private void Anim1_Completed(object sender, EventArgs e)
            {
                LowerRectangle.Opacity = 0;
                UpperRectangle.Opacity = 0;
                Children.Add(Round);
                DoubleAnimation hand1anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
                hand1anim.Completed += Hand1anim_Completed;
                Round.LeftButton.PreviewMouseDown += LeftButton_PreviewMouseDown;
                Round.RightButton.PreviewMouseDown += RightButton_PreviewMouseDown;
                Round.Regerator.PreviewMouseDown += Glass_PreviewMouseDown;
                Glass.PreviewMouseDown += Glass_PreviewMouseDown;
                UpPart.Children.Remove(Glass);
                Children.Add(Glass);
                Hand4.BeginAnimation(Rectangle.OpacityProperty, hand1anim);
                new Spark(new Point(685, 575), Links.Controller.Science2Canvas,1);
                new Spark(new Point(1205, 610), Links.Controller.Science2Canvas,1);
                PricePanel = new PricePanel(500);
                Children.Add(PricePanel);
                Canvas.SetLeft(PricePanel, 710);
                Canvas.SetTop(PricePanel, 830);
                DoubleAnimation priceanim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
                PricePanel.BeginAnimation(Canvas.OpacityProperty, priceanim);
                PricePanel.ChangePrice(SciencePrice.GetSciencePrice(EScienceField.Physic));
            }

            private void Glass_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            {
                if (IsRightBorder == false)
                {
                    PutRightBorder(true);
                    IsRightBorder = true;
                }
                else
                    Learn();
            }
            public void Learn()
            {
                EScienceField field;
                switch (pos)
                {
                    case 1: field = EScienceField.Cybernetic; break;
                    case 2: field = EScienceField.Irregular; break;
                    case 3: field = EScienceField.Energy; break;
                    default: field = EScienceField.Physic; break;
                        
                }
                Events.LearnScience(field);
            }
            public void PutRightBorder(bool needlearn)
            {
                DoubleAnimation leftborderanim = new DoubleAnimation(-850, 100, TimeSpan.FromSeconds(2));
                if (needlearn)
                    leftborderanim.Completed += Leftborderanim_Completed;
                else
                 leftborderanim.Completed += Leftborderanim_Completed2;
                ElasticEase ease = new ElasticEase();
                leftborderanim.EasingFunction = ease;
                RightBorder.BeginAnimation(Canvas.TopProperty, leftborderanim);
            }

            private void Leftborderanim_Completed2(object sender, EventArgs e)
            {
                Links.Controller.Science2Canvas._ElementsCanvas.LearnNewScinece(Links.Helper.Science, false);
            }

            private void Leftborderanim_Completed(object sender, EventArgs e)
            {
                Learn();
            }

            private void Hand1anim_Completed(object sender, EventArgs e)
            {
                DoubleAnimation leftborderanim = new DoubleAnimation(-850, 100, TimeSpan.FromSeconds(2));
                ElasticEase ease = new ElasticEase();
                leftborderanim.EasingFunction = ease;
                leftborderanim.Completed += Leftborderanim_Completed1;
                LeftBorder.BeginAnimation(Canvas.TopProperty, leftborderanim);
            }

            private void Leftborderanim_Completed1(object sender, EventArgs e)
            {
                DoubleAnimation anim1 = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));
                BigWindow.ThisWindow.BeginAnimation(Viewbox.OpacityProperty, anim1);
                BigWindow.ThisWindow.SetBack(EScienceField.Physic);
                //Children.Add(BigWindow.ThisWindow);
                //Canvas.SetZIndex(BigWindow.ThisWindow, 100);
                //Canvas.SetLeft(BigWindow.ThisWindow, 40);
                //Canvas.SetTop(BigWindow.ThisWindow, 200);


                Links.Controller.Science2Canvas.ScienceBorder = new ScienceListBorder();
                //Children.Add(Links.Controller.Science2Canvas.ScienceBorder);
                //Canvas.SetZIndex(Links.Controller.Science2Canvas.ScienceBorder, 100);
                //Canvas.SetLeft(Links.Controller.Science2Canvas.ScienceBorder, 60);
                //Canvas.SetTop(Links.Controller.Science2Canvas.ScienceBorder, 200);
                Links.Controller.Science2Canvas.ScienceBorder.Draw();
            }

            private void RightButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            {
                new MySound("Interface/Menu_circle_clockwise.wav");
                int newpos = pos - 1;
                DoubleAnimation anim = new DoubleAnimation(pos * 90, newpos * 90, time);
                anim.AccelerationRatio = 0.3;
                anim.DecelerationRatio = 0.3;
                Round.Rotate_Button.BeginAnimation(RotateTransform.AngleProperty, anim);
                double curangle = Round.Rotate_STV2.Angle;
                DoubleAnimation stv_Anim = new DoubleAnimation(curangle + 90, time);
                stv_Anim.AccelerationRatio = 0.3;
                stv_Anim.DecelerationRatio = 0.3;
                Round.Rotate_STV2.BeginAnimation(RotateTransform.AngleProperty, stv_Anim);
                DoubleAnimation animimage1 = new DoubleAnimation(1, 0, time);
                DoubleAnimation animimage2 = new DoubleAnimation(0, 1, time);
                DoubleAnimation animimage0 = new DoubleAnimation(0, TimeSpan.Zero);
                if (pos == 1)
                {
                    Hand1.BeginAnimation(Rectangle.OpacityProperty, animimage1);
                    Hand4.BeginAnimation(Rectangle.OpacityProperty, animimage2);
                    Hand2.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                    Hand3.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                    pos = 4;
                    PricePanel.ChangePrice(SciencePrice.GetSciencePrice(EScienceField.Physic));
                    BigWindow.ThisWindow.SetBack(EScienceField.Physic);
                }
                else if (pos == 2)
                {
                    Hand2.BeginAnimation(Rectangle.OpacityProperty, animimage1);
                    Hand1.BeginAnimation(Rectangle.OpacityProperty, animimage2);
                    Hand3.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                    Hand4.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                    pos = 1;
                    PricePanel.ChangePrice(SciencePrice.GetSciencePrice(EScienceField.Cybernetic));
                    BigWindow.ThisWindow.SetBack(EScienceField.Cybernetic);
                }
                else if (pos == 3)
                {
                    Hand3.BeginAnimation(Rectangle.OpacityProperty, animimage1);
                    Hand2.BeginAnimation(Rectangle.OpacityProperty, animimage2);
                    Hand1.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                    Hand4.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                    pos = 2;
                    PricePanel.ChangePrice(SciencePrice.GetSciencePrice(EScienceField.Irregular));
                    BigWindow.ThisWindow.SetBack(EScienceField.Irregular);
                }
                else
                {
                    Hand4.BeginAnimation(Rectangle.OpacityProperty, animimage1);
                    Hand3.BeginAnimation(Rectangle.OpacityProperty, animimage2);
                    Hand1.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                    Hand2.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                    pos = 3;
                    PricePanel.ChangePrice(SciencePrice.GetSciencePrice(EScienceField.Energy));
                    BigWindow.ThisWindow.SetBack(EScienceField.Energy);
                }
            }

            int pos = 4;
            TimeSpan time = TimeSpan.FromSeconds(1);
            private void LeftButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            {
                new MySound("Interface/Menu_circle_anticlockwise.wav");
                int newpos = pos + 1;
                DoubleAnimation anim = new DoubleAnimation(pos * 90, newpos * 90, time);
                anim.AccelerationRatio = 0.3;
                anim.DecelerationRatio = 0.3;
                Round.Rotate_Button.BeginAnimation(RotateTransform.AngleProperty, anim);
                double curangle = Round.Rotate_STV1.Angle;
                DoubleAnimation stv1_Anim = new DoubleAnimation(curangle - 90, time);
                stv1_Anim.AccelerationRatio = 0.3;
                stv1_Anim.DecelerationRatio = 0.3;
                Round.Rotate_STV1.BeginAnimation(RotateTransform.AngleProperty, stv1_Anim);
                DoubleAnimation animimage1 = new DoubleAnimation(1, 0, time);
                DoubleAnimation animimage2 = new DoubleAnimation(0, 1, time);
                DoubleAnimation animimage0 = new DoubleAnimation(0, TimeSpan.Zero);
                if (pos == 1)
                {
                    Hand1.BeginAnimation(Rectangle.OpacityProperty, animimage1);
                    Hand2.BeginAnimation(Rectangle.OpacityProperty, animimage2);
                    Hand3.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                    Hand4.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                    pos = 2;
                    PricePanel.ChangePrice(SciencePrice.GetSciencePrice(EScienceField.Irregular));
                    BigWindow.ThisWindow.SetBack(EScienceField.Irregular);
                }
                else if (pos == 2)
                {
                    Hand2.BeginAnimation(Rectangle.OpacityProperty, animimage1);
                    Hand3.BeginAnimation(Rectangle.OpacityProperty, animimage2);
                    Hand1.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                    Hand4.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                    pos = 3;
                    PricePanel.ChangePrice(SciencePrice.GetSciencePrice(EScienceField.Energy));
                    BigWindow.ThisWindow.SetBack(EScienceField.Energy);
                }
                else if (pos == 3)
                {
                    Hand3.BeginAnimation(Rectangle.OpacityProperty, animimage1);
                    Hand4.BeginAnimation(Rectangle.OpacityProperty, animimage2);
                    Hand1.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                    Hand2.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                    pos = 4;
                    PricePanel.ChangePrice(SciencePrice.GetSciencePrice(EScienceField.Physic));
                    BigWindow.ThisWindow.SetBack(EScienceField.Physic);
                }
                else
                {
                    Hand4.BeginAnimation(Rectangle.OpacityProperty, animimage1);
                    Hand1.BeginAnimation(Rectangle.OpacityProperty, animimage2);
                    Hand2.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                    Hand3.BeginAnimation(Rectangle.OpacityProperty, animimage0);
                    pos = 1;
                    PricePanel.ChangePrice(SciencePrice.GetSciencePrice(EScienceField.Cybernetic));
                    BigWindow.ThisWindow.SetBack(EScienceField.Cybernetic);
                }
            }
        }
        class RoundCanvas:Canvas
        {
            public RotateTransform Rotate_STV1;
            public RotateTransform Rotate_STV2;
            public RotateTransform Rotate_Button;
            public Canvas ButtonCanvas;
            public Rectangle LeftButton;
            public Rectangle RightButton;
            public Rectangle Regerator;
            public RoundCanvas()
            {
                Canvas.SetZIndex(this, 30);
                Width = 631;
                Height = 600;
                Rectangle roundbase = Science2Canvas.GetRectangle(594, 594, 662, 231, Links.Brushes.Science.Round, 0, 1);
                Children.Add(roundbase);
                Rectangle STV1 = Science2Canvas.GetRectangle(600, 600, 659, 228, Links.Brushes.Science.STV1, 0, 1);
                Children.Add(STV1);
                STV1.RenderTransformOrigin = new Point(0.5, 0.5);
                Rotate_STV1 = new RotateTransform();
                STV1.RenderTransform = Rotate_STV1;

                Rectangle STV2 = Science2Canvas.GetRectangle(600, 600, 662, 228, Links.Brushes.Science.STV2, 0, 1);
                Children.Add(STV2);
                STV2.RenderTransformOrigin = new Point(0.5, 0.5);
                Rotate_STV2 = new RotateTransform();
                STV2.RenderTransform = Rotate_STV2;

                ButtonCanvas = new Canvas();
                ButtonCanvas.Width = 594; ButtonCanvas.Height = 594;
                Canvas.SetLeft(ButtonCanvas, 662);
                Canvas.SetTop(ButtonCanvas, 231);
                Children.Add(ButtonCanvas);
                ButtonCanvas.RenderTransformOrigin = new Point(0.5, 0.5);
                Rotate_Button = new RotateTransform();
                ButtonCanvas.RenderTransform = Rotate_Button;

                Regerator = Science2Canvas.GetRectangle(155, 258, 473, 168, Links.Brushes.Science.Regirator, 0, 1);
                ButtonCanvas.Children.Add(Regerator);

                LeftButton = Science2Canvas.GetRectangle(77, 103, 500, 400, Links.Brushes.Science.Button, 0, 1);
                ButtonCanvas.Children.Add(LeftButton);

                RightButton = Science2Canvas.GetRectangle(77, 103, 500, 90, Links.Brushes.Science.Button, 0, 1);
                ButtonCanvas.Children.Add(RightButton); RightButton.RenderTransformOrigin = new Point(0.5, 0.5);
                ScaleTransform scale = new ScaleTransform();
                scale.ScaleY = -1;
                RightButton.RenderTransform = scale;
            }
            public Rectangle GetUpperRectangle()
            {
                VisualBrush brush = new VisualBrush(this);
                Rectangle rect = Science2Canvas.GetRectangle(631, 600, 659, 228, brush, 0, 1);
                rect.Clip = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                    "M0,0 v325 h10 l10,7 l5,18 l40,0 h434 v3 l40,15 v10 l60,20 h34  v-399z"));
                return rect;
            }
            public Rectangle GetLowerRectangle()
            {
                VisualBrush brush = new VisualBrush(this);
                Rectangle rect = Science2Canvas.GetRectangle(631, 600, 659, 228, brush, 0, 1);
                rect.Clip = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                    "M0,325 h10 l10,7 l5,18 l40,0 h434 v3 l40,15 v10 l60,20 h34  v201 h-631z"));
                return rect;
            }
        }
    }
    class Spark
    {
        static Random rnd = new Random();
        List<Canvas> Rects = new List<Canvas>();
        Canvas cur;
        public Spark(Point pt, Canvas canvas, double size)
        {
            cur = canvas;
            int count = rnd.Next(15, 25);
            for (int i = 0; i < count; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Width = 5*size;
                
                rect.Height = rnd.Next((int)(25*size),(int)(55* size));
               
                rect.Stroke = Brushes.Orange;
                rect.Fill = Brushes.White;
                rect.StrokeThickness = 3*size;
                rect.Opacity = 1;
                Canvas c = new Canvas();
                Canvas.SetZIndex(c, 50);
                c.Width = 5*size;
                c.Height = rnd.Next((int)(150* size), (int)(600* size));
                c.ClipToBounds = true;
                c.Children.Add(rect);
                RotateTransform rotate = new RotateTransform();
                c.RenderTransformOrigin = new Point(0.5, 0);
                Canvas.SetLeft(c, pt.X - 2.5*size);
                Canvas.SetTop(c, pt.Y);
                c.RenderTransform = rotate;
                int angle = rnd.Next(0, 360);
                rotate.Angle = angle;
                Rects.Add(c);
                cur.Children.Add(c);
                int length = rnd.Next(3);
                if (length == 0)
                {
                    DoubleAnimation anim = new DoubleAnimation(0, 600*size, TimeSpan.FromSeconds(0.4));
                    anim.DecelerationRatio = 1;
                    anim.Completed += Anim_Completed;
                    rect.BeginAnimation(Canvas.TopProperty, anim);
                }
                else if (length == 1)
                {
                    DoubleAnimation anim = new DoubleAnimation(0, 450*size, TimeSpan.FromSeconds(0.4));
                    anim.DecelerationRatio = 1;
                    anim.Completed += Anim_Completed;
                    rect.BeginAnimation(Canvas.TopProperty, anim);
                }
                else
                {
                    DoubleAnimation anim = new DoubleAnimation(0, 150*size, TimeSpan.FromSeconds(0.4));
                    anim.DecelerationRatio = 1;
                    anim.Completed += Anim_Completed;
                    rect.BeginAnimation(Canvas.TopProperty, anim);
                }
            }
        }

        private void Anim_Completed(object sender, EventArgs e)
        {
            foreach (Canvas c in Rects)
                cur.Children.Remove(c);
        }
    }
    class PricePanel:Canvas
    {
        double width;
        int money, metall, chips, anti;
        TextBlock Money, Metall, Chips, Anti;
        public PricePanel(double width)
        {
            this.width = width;
            Width = width; Height = width / 3;
            Background = Links.Brushes.Interface.MBButtonBack;

            Money=AddBack(14, 44, 17, 23, Links.Brushes.MoneyImageBrush);
            Metall=AddBack(14, 44, 52, 58, Links.Brushes.MetalImageBrush);
            Chips=AddBack(156, 186, 17, 23, Links.Brushes.ChipsImageBrush);
            Anti=AddBack(156, 186, 52, 58, Links.Brushes.AntiImageBrush);

        }
        TextBlock AddBack(double left1, double left2, double top, double top2, Brush brush)
        {
            Rectangle ib = Common.GetRectangle((int)(width / 10), Links.Brushes.Interface.ImageBack);
            Children.Add(ib); Canvas.SetLeft(ib, width*left1/300); Canvas.SetTop(ib, width*top/300);
            Rectangle tb = new Rectangle(); tb.Width = width / 3; tb.Height = width / 10; tb.Fill = Links.Brushes.Interface.BigSquare;
            Children.Add(tb); Canvas.SetLeft(tb, width*left2/300); Canvas.SetTop(tb, width*top/300);
            TextBlock block = Common.GetBlock((int)(width / 20), "", Brushes.White, (int)(width / 3));
            Children.Add(block); Canvas.SetLeft(block, width*left2/300); Canvas.SetTop(block, width*top2/300);
            Rectangle rect = Common.GetRectangle((int)(width / 12), brush);
            Children.Add(rect); Canvas.SetLeft(rect, width * (left1 + 2) / 300); Canvas.SetTop(rect, width * (top + 2) / 300);
            return block;
        }
        static double[] stepsvalue = new double[] { 0, 0.3, 0.55, 0.75, 0.9, 0.95, 1.0 };
        static double[] stepstime = new double[] { 0, 0.05, 0.15, 0.3, 0.5, 0.75, 1.0 };
        string Symbol;
        public void ChangePrice(ItemPrice price, params string[] symbol)
        {
            if (symbol.Length > 0)
                Symbol = symbol[0];
            else
                Symbol = "";
            if (price.Money > GSGameInfo.Money) Money.Foreground = Brushes.Red; else Money.Foreground = Brushes.White;
            if (money != price.Money)
                AddChangeAnim(Money, money, price.Money);
            if (price.Metall > GSGameInfo.Metals) Metall.Foreground = Brushes.Red; else Metall.Foreground = Brushes.White;
            if (metall != price.Metall)
                AddChangeAnim(Metall, metall, price.Metall);
            if (price.Chips > GSGameInfo.Chips) Chips.Foreground = Brushes.Red; else Chips.Foreground = Brushes.White;
            if (chips != price.Chips)
                AddChangeAnim(Chips, chips, price.Chips);
            if (price.Anti > GSGameInfo.Anti) Anti.Foreground = Brushes.Red; else Anti.Foreground = Brushes.White;
            if (anti != price.Anti)
                AddChangeAnim(Anti, anti, price.Anti);
            money = price.Money; metall = price.Metall; chips = price.Chips; anti = price.Anti;
            
        }
        void AddChangeAnim(TextBlock block, int curvalue, int newvalue)
        {
            int delta = newvalue - curvalue;
            StringAnimationUsingKeyFrames anim = new StringAnimationUsingKeyFrames();
            anim.Duration = TimeSpan.FromSeconds(0.3);
            List<double> values = new List<double>();
            for (int i = 0; i < 7; i++)
                anim.KeyFrames.Add(new DiscreteStringKeyFrame(Symbol+(curvalue + delta * stepsvalue[i]).ToString("### ### ### ###"), KeyTime.FromPercent(stepstime[i])));
            
            block.BeginAnimation(TextBlock.TextProperty, anim);
        }
    }
    class BigWindow:Viewbox
    {
        Canvas maincanvas, HorCanvas, canvas;
        public ScrollViewer HorScroll, Scroll;
        Viewbox HorBox, Box;
        //Slider slider;
        static double horsize = 130; static double versize = 160;
        int levels = 50; int items = 86;
        double width, height;
        Point StartPoint;
        bool IsStartSelected = false;
        double zoom = 1;
        public static BigWindow ThisWindow;
        int maxlevel;
        public SortedList<ushort, Ellipse> Elements;
        public bool IsAuto = false;
        List<Line> Lines = new List<Line>();
        public BigWindow()
        {
            ThisWindow = this;
            Elements = new SortedList<ushort, Ellipse>();
            Width = 540;
            Height = 800;
            maxlevel = 0;
            foreach (ushort scienceid in GSGameInfo.SciencesArray)
            {
                GameScience science = Links.Science.GameSciences[scienceid];
                if (science.Level > maxlevel)
                    maxlevel = science.Level;
            }
            if (maxlevel < 49) maxlevel++;
            if (maxlevel < 10) maxlevel = 7;
            levels = maxlevel;
            width = horsize * levels;
            height = versize * items;
            
            
            maincanvas = new Canvas(); maincanvas.Width = 600; maincanvas.Height = 800;
            Child = maincanvas;

            HorScroll = new ScrollViewer();
            HorScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            HorScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            HorScroll.Height = 20; HorScroll.Width = 600;
            maincanvas.Children.Add(HorScroll); 
            HorBox = new Viewbox(); HorBox.Width = width/zoom; HorBox.Stretch = Stretch.Fill;
            HorBox.Height = 20;  HorScroll.Content = HorBox;
            HorCanvas = new Canvas();
            HorCanvas.Width = width;
            HorCanvas.Height = 20;
            HorCanvas.Background = Brushes.Black;
            HorBox.Child = HorCanvas;
           
            Scroll = new ScrollViewer();
            Scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            Scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            Scroll.Width = 600; Scroll.Height = 650;
            maincanvas.Children.Add(Scroll); 
            Canvas.SetTop(Scroll, 20);
            Box = new Viewbox(); Box.Width = width/zoom; Box.Height = height/zoom;
            Scroll.Content = Box;
            canvas = new Canvas();
            canvas.Width = width; canvas.Height = height;
            Box.Child = canvas;
            canvas.Background = Brushes.Black;
            for (int i=0;i<levels;i++)
            {
                TextBlock block = Common.GetBlock(20, (i + 1).ToString(), Brushes.White, (int)horsize);
                HorCanvas.Children.Add(block); Canvas.SetLeft(block, i * horsize);
                Line l = new Line(); l.X1 = (i + 1) * horsize; l.X2 = l.X1; l.Y1 = -20; l.Y2 = height + 20;
                l.StrokeDashArray = new DoubleCollection(new double[] { 0.5 });
                canvas.Children.Add(l); l.Stroke = Brushes.SkyBlue; l.StrokeThickness = 2;
            }
            for (int i=0;i< items; i++)
            {
                Line l = new Line(); l.X1 = 0; l.X2 = width; l.Y1 = (i+1)*versize; l.Y2 = (i + 1) * versize;
                l.StrokeDashArray = new DoubleCollection(new double[] { 0.5 });
                canvas.Children.Add(l); l.Stroke = Brushes.SkyBlue; l.StrokeThickness = 2;
                Lines.Add(l);
            }
            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseUp += Canvas_MouseUp;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseLeave += Canvas_MouseLeave;
            foreach (GameScience science in Links.Science.GameSciences.Values)
            {
                int element = (int)science.Element;
                if (GSGameInfo.SciencesArray.Contains(science.ID) == false)
                {
                    Ellipse el = new Ellipse(); el.Width = horsize/2; el.Height = el.Width;
                    canvas.Children.Add(el); Canvas.SetLeft(el, horsize/4 + science.Level * horsize);
                    Canvas.SetTop(el, (versize-el.Width)/2 + element * versize);
                    el.Fill = Brushes.SkyBlue;
                    Elements.Add(science.ID, el);
                    continue;
                }
                
                //if (element<84)
               // {
                    GameObjectImage goi = new GameObjectImage(GOImageType.Standard, science.ID);
                    goi.Width = 120;
                    goi.Height = 150;
                    canvas.Children.Add(goi);
                    Canvas.SetLeft(goi, 5 + science.Level * horsize);
                    Canvas.SetTop(goi, 5 + element * versize);
                    goi.Tag = science;
                    goi.PreviewMouseDown += Goi_PreviewMouseDown;
                    //Elements.Add(science.ID, goi);
             //   }
            }

            //slider = new Slider(); slider.Width = 30; slider.Height = 300; slider.Orientation = Orientation.Vertical;
            //maincanvas.Children.Add(slider); Canvas.SetLeft(slider, 1050); Canvas.SetTop(slider, 100);
            //slider.Maximum = 5; slider.Minimum = 0.5;
            //slider.Value = 1;
            //slider.ValueChanged += Slider_ValueChanged;
            timer.Interval = TimeSpan.FromSeconds(1 / 24.0);
            timer.Tick += Timer_Tick;

            GeometryDrawing geom = new GeometryDrawing(Brushes.White, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M0,70 l50,-70 h450 l50,70 v660 l-50,70 h-440 l-50,-70z")));
            OpacityMask = new DrawingBrush(geom);

            AddArrow("M0,10 a10,10 0 0,1 10,-10 h20 a10,10 0 0,1 7,17 l-20,20 a10,10 0 0,1 -17,-7z", 30, 40, true, false, true, false);
            AddArrow("M0,10 a10,10 0 0,1 15,0 l15,15 a10,10 0 0,1 -7,17 h-31 a10,10 0 0,1 -7,-17 l15,-15", 300, 20, false, false, true, false);
            AddArrow("M10,0 a10,10 0 0,1 10,10 v20 a10,10 0 0,1 -17,7 l-20,-20 a10,10 0 0,1 7,-17z", 560, 40, false, true, true, false);
            AddArrow("M0,10 a10,10 0 0,1 0,15 l-15,15 a10,10 0 0,1 -17,-7 v-31 a10,10 0 0,1 17,-7 l15,15", 590, 300, false, true, false, false);
            AddArrow("M10,0 a10,10 0 0,1 -10,10 h-20 a10,10 0 0,1 -7,-17 l20,-20 a10,10 0 0,1 17,7z", 570, 620, false, true, false, true);
            AddArrow("M0,10 a10,10 0 0,1 -15,0 l-15,-15 a10,10 0 0,1 7,-17 h31 a10,10 0 0,1 7,17 l-15,15", 300, 650, false, false, false, true);
            AddArrow("M0,10 a10,10 0 0,1 -10,-10 v-20 a10,10 0 0,1 17,-7 l20,20 a10,10 0 0,1 -7,17z", 40, 620, true, false, false, true);
            AddArrow("M0,10 a10,10 0 0,1 0,-15 l15,-15 a10,10 0 0,1 17,7 v31 a10,10 0 0,1 -17,7 l-15,-15", 20, 300, true, false, false, false);
            //Button btn = new Button(); btn.Width = 50; btn.Height = 30; maincanvas.Children.Add(btn); Canvas.SetLeft(btn, 1050); Canvas.SetTop(btn, 470);
            //btn.PreviewMouseDown += Btn_PreviewMouseDown;
            movetimer = new System.Windows.Threading.DispatcherTimer();
            movetimer.Interval = TimeSpan.FromSeconds(1 / 24.0);
            movetimer.Tick += Movetimer_Tick;
        }
        public void CheckLevel()
        {
            foreach (ushort scienceid in GSGameInfo.SciencesArray)
            {
                GameScience science = Links.Science.GameSciences[scienceid];
                if (science.Level > maxlevel)
                    maxlevel = science.Level;
            }
            if (maxlevel < 49) maxlevel++;
            if (maxlevel < 10) maxlevel = 7;
            if (levels == maxlevel) return;
            levels = maxlevel;
            width = horsize * levels;
            height = versize * items;
            canvas.Width = width;
            HorCanvas.Width = width;
            foreach (Line ll in Lines)
                ll.X2 = width;
            TextBlock block = Common.GetBlock(20, levels.ToString(), Brushes.White, (int)horsize);
            HorCanvas.Children.Add(block); Canvas.SetLeft(block, (levels - 1) * horsize);
            Line l = new Line(); l.X1 = levels * horsize; l.X2 = l.X1; l.Y1 = -20; l.Y2 = height + 20;
            l.StrokeDashArray = new DoubleCollection(new double[] { 0.5 });
            canvas.Children.Add(l); l.Stroke = Brushes.SkyBlue; l.StrokeThickness = 2;
            foreach (GameScience science in Links.Science.GameSciences.Values)
            {
                int element = (int)science.Element;
                if (GSGameInfo.SciencesArray.Contains(science.ID) == false && Elements.ContainsKey(science.ID)==false)
                {
                    Ellipse el = new Ellipse(); el.Width = horsize / 2; el.Height = el.Width;
                    canvas.Children.Add(el); Canvas.SetLeft(el, horsize / 4 + science.Level * horsize);
                    Canvas.SetTop(el, (versize - el.Width) / 2 + element * versize);
                    el.Fill = Brushes.SkyBlue;
                    Elements.Add(science.ID, el);
                    continue;
                }
            }
        }
        System.Windows.Threading.DispatcherTimer movetimer;
        private void Movetimer_Tick(object sender, EventArgs e)
        {
            if (MoveDirectionLeft)
            {
                Scroll.ScrollToHorizontalOffset(Scroll.HorizontalOffset - 50);
                HorScroll.ScrollToHorizontalOffset(HorScroll.HorizontalOffset - 50);
            }
            if (MoveDirectionRight)
            {
                Scroll.ScrollToHorizontalOffset(Scroll.HorizontalOffset + 50);
                HorScroll.ScrollToHorizontalOffset(HorScroll.HorizontalOffset + 50);
            }
            if (MoveDirectionUp)
                Scroll.ScrollToVerticalOffset(Scroll.VerticalOffset - 50);
            if (MoveDirectionDown)
                Scroll.ScrollToVerticalOffset(Scroll.VerticalOffset + 50);
            if (!MoveDirectionLeft && !MoveDirectionRight && !MoveDirectionUp && !MoveDirectionDown)
            movetimer.Stop();
        }

        bool MoveDirectionLeft, MoveDirectionUp, MoveDirectionRight, MoveDirectionDown;
        private void LeftArrow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MoveDirectionLeft = false;
        }

        private void LeftArrow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsAuto == true) return;
            MoveDirectionLeft = true;
            movetimer.Start();
        }
        private void RightArrow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MoveDirectionRight = false;
        }

        private void RightArrow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsAuto == true) return;
            MoveDirectionRight = true;
            movetimer.Start();
        }
        private void UpArrow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MoveDirectionUp = false;
        }

        private void UpArrow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsAuto == true) return;
            MoveDirectionUp = true;
            movetimer.Start();
        }
        private void DownArrow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MoveDirectionDown = false;
        }

        private void DownArrow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsAuto == true) return;
            MoveDirectionDown = true;
            movetimer.Start();
        }
        void AddArrow(string data, int left, int top, bool isleft, bool isright, bool isup, bool isdown)
        {
            Path Arrow = new Path();
            Arrow.Stroke = Brushes.SkyBlue;
            Arrow.Fill = new SolidColorBrush(Color.FromRgb(0, 128, 255));
            Arrow.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(data));
            maincanvas.Children.Add(Arrow); Canvas.SetLeft(Arrow, left); Canvas.SetTop(Arrow, top);
            if (isleft)
            {
                Arrow.MouseDown += LeftArrow_MouseDown;
                Arrow.MouseUp += LeftArrow_MouseUp;
            }
            if (isright)
            {
                Arrow.MouseDown += RightArrow_MouseDown;
                Arrow.MouseUp += RightArrow_MouseUp;
            }
            if (isup)
            {
                Arrow.MouseDown += UpArrow_MouseDown;
                Arrow.MouseUp += UpArrow_MouseUp;
            }
            if (isdown)
            {
                Arrow.MouseDown += DownArrow_MouseDown;
                Arrow.MouseUp += DownArrow_MouseUp;
            }
            Arrow.MouseLeave += Arrow_MouseLeave;
        }

        private void Arrow_MouseLeave(object sender, MouseEventArgs e)
        {
            MoveDirectionUp = false; MoveDirectionDown = false;
            MoveDirectionLeft = false; MoveDirectionRight = false;
        }

        private static void Goi_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            GameObjectImage goi = (GameObjectImage)sender;
            GameScience science = (GameScience)goi.Tag;
            Links.Controller.Science2Canvas._ElementsCanvas.LearnNewScinece(science, false);
        }
        public void SetBack(EScienceField field)
        {
            Brush brush = null;
            switch (field)
            {
                case EScienceField.Energy: brush = Links.Brushes.Science.Hand3; break;
                case EScienceField.Physic: brush = Links.Brushes.Science.Hand4; break;
                case EScienceField.Irregular: brush = Links.Brushes.Science.Hand2; break;
                case EScienceField.Cybernetic: brush = Links.Brushes.Science.Hand1; break;
            }
            foreach (KeyValuePair<ushort, Ellipse> pair in Elements)
            {
                GameScience science = Links.Science.GameSciences[pair.Key];
                if (science.PrimaryField == field || science.SecondaryField == field)
                {
                    pair.Value.Fill = brush;
                    pair.Value.Stroke = null;
                }
                else
                {
                    pair.Value.Fill = blankbrush;
                    pair.Value.Stroke = Brushes.SkyBlue;
                }
            }
        }
        static SolidColorBrush blankbrush = new SolidColorBrush(Color.FromArgb(80, 80, 80, 80));
        static System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        static double hormax, horcur, vermax, vercur, verdir, hordir;
        static Queue<ushort> learnedscience = new Queue<ushort>();// static ushort learnedscience;
        static Queue<ushort> Removingscience=new Queue<ushort>();
        public static void LearnScience(ushort id)
        {
            //ThisWindow.slider.Value = 1;
            ThisWindow.IsAuto = true;
            learnedscience.Enqueue(id);
            //Links.Controller.PopUpCanvas.Place(ThisWindow);
            GameScience science = Links.Science.GameSciences[id];
            int element = (int)science.Element;
            hormax = science.Level * horsize - horsize * 2;
            if (hormax < 0) hormax = 0;
            vermax = element * versize - versize * 2;
            if (vermax < 0) vermax = 0;
            //timer = new System.Windows.Threading.DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(1 / 24.0);
            horcur = ThisWindow.Scroll.HorizontalOffset;
            vercur = ThisWindow.Scroll.VerticalOffset;
            if (hormax > horcur) hordir = 100; else hordir = -100;
            if (vermax > vercur) verdir = 100; else verdir = -100;
            //timer.Tick += Timer_Tick;
            timer.Start();
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            bool horend = false; bool verend = false;
            if ((hordir >0  && horcur >= hormax) || (hordir <0 && horcur <= hormax)) horend = true;
            if ((verdir >0 && vercur >= vermax) || (verdir <0 && vercur <= vermax)) verend = true;
            if (horend && verend)
            {
                timer.Stop();
                for (;;)
                {
                    if (learnedscience.Count == 0) break;
                    ushort id = learnedscience.Dequeue();
                    Removingscience.Enqueue(id);
                    Ellipse el = ThisWindow.Elements[id];
                    DoubleAnimation hideanim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));
                    hideanim.Completed += Hideanim_Completed;
                    el.BeginAnimation(Ellipse.OpacityProperty, hideanim);
                    GameScience science = Links.Science.GameSciences[id];
                    int element = (int)science.Element;
                    GameObjectImage goi = new GameObjectImage(GOImageType.Standard, id);
                    goi.Width = 120;
                    goi.Height = 150;
                    ThisWindow.canvas.Children.Add(goi);
                    goi.Tag = science;
                    goi.PreviewMouseDown += Goi_PreviewMouseDown;
                    Canvas.SetLeft(goi, 5 + science.Level * horsize);
                    Canvas.SetTop(goi, 5 + element * versize);
                    DoubleAnimation showanim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));
                    goi.BeginAnimation(Viewbox.OpacityProperty, showanim);
                }
                return;
            }
            if (horend == false)
            {
                horcur += hordir;
                ThisWindow.Scroll.ScrollToHorizontalOffset(horcur);
                ThisWindow.HorScroll.ScrollToHorizontalOffset(horcur);
            }
            if (verend == false)
            {
                vercur += verdir;
                ThisWindow.Scroll.ScrollToVerticalOffset(vercur);
            }
            
        }

        
        private static void Hideanim_Completed(object sender, EventArgs e)
        {
            for (;;)
            {
                if (Removingscience.Count == 0) break;
                ThisWindow.Elements.Remove(Removingscience.Dequeue());
            }
            ThisWindow.CheckLevel();
            ThisWindow.IsAuto = false;
        }

        
        static new Canvas Parent;
        public static void ShowWindow(Canvas canvas)
        {
            if (Parent != null)
                Parent.Children.Remove(ThisWindow);
            Parent = canvas;
            Parent.Children.Add(ThisWindow);
            DoubleAnimation anim1 = new DoubleAnimation(0, TimeSpan.Zero);
            ThisWindow.BeginAnimation(Viewbox.OpacityProperty, anim1);

        }
        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            if (IsAuto) return;
            IsStartSelected = false;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsAuto) return;
            if (IsStartSelected == false) return;
            Point cur = e.GetPosition(canvas);
            cur = new Point(cur.X / zoom, cur.Y / zoom);
            double dx = StartPoint.X - cur.X;
            double dy = StartPoint.Y - cur.Y;
            Scroll.ScrollToHorizontalOffset(Scroll.HorizontalOffset + dx);
            Scroll.ScrollToVerticalOffset(Scroll.VerticalOffset + dy);
            HorScroll.ScrollToHorizontalOffset(Scroll.HorizontalOffset);
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (IsAuto) return;
            IsStartSelected = false;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsAuto) return;
            IsStartSelected = true;
            StartPoint = e.GetPosition(canvas);
            StartPoint = new Point(StartPoint.X / zoom, StartPoint.Y / zoom);
        }
    }*/
}
