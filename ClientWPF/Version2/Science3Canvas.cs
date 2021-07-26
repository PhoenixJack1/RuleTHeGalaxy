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
    class Science3Canvas:Canvas
    {
        public Viewbox VBX;
        public ScienceObserver Observer;
        GameButton SimpleLearn;
        GameButton MediumLearn;
        GameButton FullLearn;
        ScienceInfo Info;
        LearnInfo Learn;
        ElementsCanvas ElementsCanvas;
        public Science3Canvas()
        {
            Background = Brushes.Black;
            Width = 1920;
            Height = 1080;
            ClipToBounds = true;
            VBX = new Viewbox();
            VBX.Child = this;
            SimpleLearn = new GameButton(600, 150, "Простое изучение", 60);
            Canvas.SetLeft(SimpleLearn, 1920 / 2 -300); Canvas.SetTop(SimpleLearn, 200);
            SimpleLearn.Tag = ScienceLearn.Low;
            SimpleLearn.PreviewMouseDown += Btn_PrviewMouseDown;
            SimpleLearn.MouseEnter += Btn_MouseEnter;
            MediumLearn = new GameButton(600, 150, "Среднее изучение", 60);
            Canvas.SetLeft(MediumLearn, 1920 / 2 - 300); Canvas.SetTop(MediumLearn, 400);
            MediumLearn.Tag = ScienceLearn.Current;
            MediumLearn.PreviewMouseDown += Btn_PrviewMouseDown;
            MediumLearn.MouseEnter += Btn_MouseEnter;
            FullLearn = new GameButton(600, 150, "Полное изучение", 60);
            Canvas.SetLeft(FullLearn, 1920 / 2 - 300); Canvas.SetTop(FullLearn, 600);
            FullLearn.Tag = ScienceLearn.Max;
            FullLearn.MouseEnter += Btn_MouseEnter;
            FullLearn.PreviewMouseDown += Btn_PrviewMouseDown;
            Info = new ScienceInfo();
            Canvas.SetLeft(Info, 50); Canvas.SetTop(Info, 200);
            Learn = new LearnInfo();
            Canvas.SetLeft(Learn, 1300); Canvas.SetTop(Learn, 200);
            Observer = new ScienceObserver();
            
            //ElementsCanvas.Play();

        }

      
        private void Btn_MouseEnter(object sender, MouseEventArgs e)
        {
            GameButton btn = (GameButton)sender;
            ScienceLearn sl = (ScienceLearn)btn.Tag;
            Learn.Fill(sl);
        }

      

        class LearnInfo:Border
        {
            TextBlock Text;
            PricePanel Price;
            public LearnInfo()
            {
                Width = 500; Height = 600;
                BorderBrush = Links.Brushes.SkyBlue; BorderThickness = new Thickness(3);
                CornerRadius = new CornerRadius(20);
                Background = Brushes.Black;
                Canvas canvas = new Canvas();
                Child = canvas;
                Text = Common.GetBlock(40, "", Brushes.White, 500);
                canvas.Children.Add(Text);
                Price = new PricePanel(450);
                canvas.Children.Add(Price);
                Canvas.SetTop(Price, 400); Canvas.SetLeft(Price, 25);
            }
            public void Fill(ScienceLearn level)
            {
                switch (level)
                {
                    case ScienceLearn.Low:
                        Text.Text = "Изучение технологий по минимальному финансированию. Изучаются технологии низших уровней. Открыть новый уровень технологий невозможно."; break;
                    case ScienceLearn.Current:
                        Text.Text = "Изучение технологий по среднему финансированию. Возможно открыть новый уровень технологию."; break;
                    case ScienceLearn.Max:
                        Text.Text = "Максимальное финансирование науки. Высока вероятность открыть новый технолгический уровень."; break;
                }
                Price.ChangePrice(SciencePrice.GetSciencePrice(GSGameInfo.MaxScienceLevel, GSGameInfo.SciencesArray.Count, level));
            }
        }
        class ScienceInfo:Border
        {
            public TextBlock MaxLevel, SciencesCount;
            public ScienceInfo()
            {
                Width = 500; Height = 600;
                BorderBrush = Links.Brushes.SkyBlue; BorderThickness = new Thickness(3);
                CornerRadius = new CornerRadius(20);
                Background = Brushes.Black;
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Vertical;
                MaxLevel = Common.GetBlock(40, "", Brushes.White, 400);
                MaxLevel.Margin = new Thickness(0, 50, 0, 50);
                panel.Children.Add(MaxLevel);
                SciencesCount = Common.GetBlock(40, "", Brushes.White, 400);
                SciencesCount.Margin = new Thickness(0, 50, 0, 150);
                panel.Children.Add(SciencesCount);
                GameButton show = new GameButton(450, 150, "Показать технологии", 40);
                panel.Children.Add(show);
                show.PreviewMouseDown += Show_PreviewMouseDown;
                Child = panel;
            }
            public void Fill()
            {
                MaxLevel.Text = String.Format("Максимальный уровень техологий: {0}", GSGameInfo.MaxScienceLevel);
                SciencesCount.Text = String.Format("Количество технологий: {0}", GSGameInfo.SciencesArray.Count);
            }
            private void Show_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            {
                Links.Controller.MainCanvas.Children.Add(Links.Controller.Science3Canvas.Observer);
                Links.Controller.Science3Canvas.Observer.Draw();
                DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1.0));
                Links.Controller.Science3Canvas.Observer.BeginAnimation(Viewbox.OpacityProperty, anim);
            }
        }
        private void Btn_PrviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            GameButton btn = (GameButton)sender;
            ScienceLearn sl = (ScienceLearn)btn.Tag;
            Children.Clear();
            int learnresult = Events.LearnScience(sl);
            Gets.GetResources();
            Gets.GetTotalInfo("выполнение исследования");
            Children.Add(ElementsCanvas);
            ElementsCanvas.Play((ushort)learnresult);
        }

        private void ShowObserver_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.MainCanvas.Children.Add(Observer);
            Observer.Draw();
            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1.0));
            Observer.BeginAnimation(Viewbox.OpacityProperty, anim);
        }
        public void RemoveObserver()
        {
            Links.Controller.MainCanvas.Children.Remove(Observer);
        }
        public void Select()
        {
            Gets.GetTotalInfo("После откртыия панели науки");
            //Gets.GetSciencePriceBases();
            Children.Clear();
            //ScienceAnalys.Fill2();
            if (Children.Contains(SimpleLearn) == false)
                Children.Add(SimpleLearn);
            if (Children.Contains(MediumLearn) == false)
                Children.Add(MediumLearn);
            if (Children.Contains(FullLearn) == false)
                Children.Add(FullLearn);
            if (Children.Contains(Info) == false)
                Children.Add(Info);
            Info.Fill();
            if (Children.Contains(Learn) == false)
                Children.Add(Learn);
            ElementsCanvas = new ElementsCanvas();
            //if (Children.Contains(Observer) == false)
            //    Children.Add(Observer);
            //Observer.Draw();
        }
    }
    public class ScienceObserver: Viewbox
    {
        Canvas LeftCanvas, TopCanvas, MiddleCanvas;
        public static ScienceType MaxType = ScienceType.ModuleImmune;
        public double CurCenterX = 0;
        public double CurCenterY = 0;
        bool MouseDown = false;
        Point pt;
        public SortedList<int, LevelElement> LevelElements = new SortedList<int, LevelElement>();
        public SortedList<ScienceType, TypeElement> TypeElements = new SortedList<ScienceType, TypeElement>();
        SortedList<int, GameObjectImage> ImagingSciences = new SortedList<int, GameObjectImage>();
        public SortedList<ScienceType, Rectangle> BackRects = new SortedList<ScienceType, Rectangle>();
        public ScienceObserver()
        {
            Width = 1920; Height = 1080; Canvas.SetZIndex(this, 170); //Canvas.SetTop(this, 170);
            Stretch = Stretch.Fill;
            Canvas canvas = new Canvas(); canvas.Width = 1920; canvas.Height = 1080;
            canvas.Background = Brushes.Black;
            Child = canvas;
            LeftCanvas = new Canvas(); LeftCanvas.Width = 100; LeftCanvas.Height = 980; LeftCanvas.ClipToBounds = true;
            //LeftCanvas.Background = Brushes.LightGray;
            canvas.Children.Add(LeftCanvas); Canvas.SetTop(LeftCanvas, 100);
            LeftCanvas.PreviewMouseDown += LeftCanvas_PreviewMouseDown;

            TopCanvas = new Canvas(); TopCanvas.Width = 1820; TopCanvas.Height = 100; TopCanvas.ClipToBounds = true;
            //TopCanvas.Background = Brushes.LightGray;
            canvas.Children.Add(TopCanvas); Canvas.SetLeft(TopCanvas, 100);
            TopCanvas.PreviewMouseDown += TopCanvas_PreviewMouseDown;

            Rectangle ShowAll = Common.GetRectangle(90, Brushes.Red);
            canvas.Children.Add(ShowAll); Canvas.SetLeft(ShowAll, 5); Canvas.SetTop(ShowAll, 5);
            ShowAll.PreviewMouseDown += Back;

            MiddleCanvas = new Canvas(); MiddleCanvas.Width = 1820; MiddleCanvas.Height = 980; MiddleCanvas.ClipToBounds = true;
            MiddleCanvas.Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0)); canvas.Children.Add(MiddleCanvas);
            Canvas.SetLeft(MiddleCanvas, 100); Canvas.SetTop(MiddleCanvas, 100);
            MiddleCanvas.PreviewMouseDown += MiddleCanvas_PreviewMouseDown;
            MiddleCanvas.PreviewMouseUp += MiddleCanvas_PreviewMouseUp;
            MiddleCanvas.PreviewMouseMove += MiddleCanvas_PreviewMouseMove;
            MiddleCanvas.MouseLeave += MiddleCanvas_MouseLeave;

            //Draw();
        }

        private void LeftCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Canvas ShowAllCanvas = new Canvas(); ShowAllCanvas.Width = 1920; ShowAllCanvas.Height = 1080;
            Links.Controller.PopUpCanvas.Place(ShowAllCanvas, true);
            WrapPanel ItemsPanel = new WrapPanel();
            ItemsPanel.Width = 1820; ShowAllCanvas.Children.Add(ItemsPanel); Canvas.SetLeft(ItemsPanel, 50); Canvas.SetTop(ItemsPanel, 50);
            for (int i = 0; i <= (int)MaxType; i++)
            {
                TypeElement el = new TypeElement((ScienceType)i);
                el.Margin = new Thickness(5);
                ItemsPanel.Children.Add(el);
                el.ShowLineEventOn();
            }
        }

        private void TopCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Canvas ShowAllCanvas = new Canvas(); ShowAllCanvas.Width = 1920; ShowAllCanvas.Height = 1080;
            Links.Controller.PopUpCanvas.Place(ShowAllCanvas, true);
            WrapPanel ItemsPanel = new WrapPanel();
            ItemsPanel.Width = 1820; ShowAllCanvas.Children.Add(ItemsPanel); Canvas.SetLeft(ItemsPanel, 50); Canvas.SetTop(ItemsPanel, 50);
            for (byte i = 0; i <= 50; i++)
            {
                LevelElement el = new LevelElement(i);
                el.Margin = new Thickness(5);
                ItemsPanel.Children.Add(el);
                el.ShowLineEventOn();
            }
        }

        private void Back(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.Science3Canvas.RemoveObserver();
            /*
            Canvas ShowAllCanvas = new Canvas(); ShowAllCanvas.Width = 1920; ShowAllCanvas.Height = 1080;
            Links.Controller.PopUpCanvas.Place(ShowAllCanvas, true);
            WrapPanel ItemsPanel = new WrapPanel();
            ItemsPanel.Width = 1820; ShowAllCanvas.Children.Add(ItemsPanel); Canvas.SetLeft(ItemsPanel, 50); Canvas.SetTop(ItemsPanel, 50);
            for (int i=0;i<=(int)MaxType;i++)
            {
                TypeElement el = new TypeElement((ScienceType)i);
                el.Margin = new Thickness(5);
                ItemsPanel.Children.Add(el);
                el.ShowLineEventOn();
            }
            */
        }

        private void MiddleCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            if (MouseDown == true) MiddleCanvas_PreviewMouseUp(null, null);
        }

        private void MiddleCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (MouseDown == false) return;
            Point pt1 = e.GetPosition(MiddleCanvas);
            double dx = pt1.X - pt.X;
            double dy = pt1.Y - pt.Y;
            CurCenterX -= dx*2;
            if (CurCenterX < 0) CurCenterX = 0;
            if (CurCenterX > 50 * 600) CurCenterX = 50 * 600;
                CurCenterY -= dy*2;
            if (CurCenterY < 0) CurCenterY = 0;
            else if (CurCenterY > (int)MaxType * 600) CurCenterY = (int)MaxType * 600;
            pt = pt1;
            Draw();
        }
        public void Draw()
        {
            int CurCenterLevel = (int)Math.Round((CurCenterX / 600), 0);
            int MinLevelValue = (int)CurCenterX - 3100;
            int MaxLevelValue = (int)CurCenterX + 3100;
            SortedSet<int> Levels = new SortedSet<int>();
            for (int i = CurCenterLevel - 5; i < CurCenterLevel + 6; i++)
            {
                if (i < 0) continue;
                int pos = i * 600;
                if (pos < MinLevelValue || pos > MaxLevelValue) continue;
                int dx = (int)(pos - CurCenterX);
                double x = 0;
                if (dx > 0)
                {
                    if (dx < 300)
                        x = dx / 300.0 * 200;
                    else if (dx < 900)
                        x = (dx - 300) / 600.0 * 300 + 200;
                    else if (dx < 1500)
                        x = (dx - 900) / 600.0 * 200 + 500;
                    else if (dx < 2300)
                        x = (dx - 1500) / 800.0 * 150 + 700;
                    else
                        x = (dx - 2300) / 1000.0 * 60.0 + 850;
                }
                else
                {
                    if (dx > -300)
                        x = dx / 300.0 * 200;
                    else if (dx > -900)
                        x = (dx + 300) / 600.0 * 300 - 200;
                    else if (dx > -1500)
                        x = (dx + 900) / 600.0 * 200 - 500;
                    else if (dx > -2300)
                        x = (dx + 1500) / 800.0 * 150 - 700;
                    else
                        x = (dx + 2300) / 1000.0 * 60.0 - 850;
                }
                if (LevelElements.ContainsKey(i) == false)
                {
                    LevelElements.Add(i, new LevelElement(i));
                    TopCanvas.Children.Add(LevelElements[i]);
                }
                Levels.Add(i);
                Viewbox el = LevelElements[i];
                Canvas.SetLeft(el, 910 + x - 50);
            }
            List<int> RemoveElements = new List<int>();
            foreach (int i in LevelElements.Keys)
                if (Levels.Contains(i) == false) RemoveElements.Add(i);
            foreach (int i in RemoveElements)
            {
                TopCanvas.Children.Remove(LevelElements[i]);
                LevelElements.Remove(i);
            }
            
            int CurCenterType = (int)Math.Round((CurCenterY / 600), 0);
            int MinLevelType = (int)CurCenterY - 1000;
            int MaxLevelType = (int)CurCenterY + 1000;
            SortedSet<ScienceType> Types = new SortedSet<ScienceType>();
            for (int i = CurCenterType - 2; i < CurCenterType + 3; i++)
            {
                if (i < 0) continue;
                if (i > (int)MaxType) continue;
                int pos = i * 600;
                if (pos < MinLevelType || pos > MaxLevelType) continue;
                ScienceType curtype = (ScienceType)i;
                int dy = (int)(pos - CurCenterY);
                double y = 0;
                if (dy > 0)
                {
                    if (dy < 300)
                        y = dy / 300.0 * 200;
                    else
                        y = (dy - 300) / 600.0 * 300 + 200;
                }
                else
                {
                    if (dy > -300)
                        y = dy / 300.0 * 200;
                    else
                        y = (dy + 300) / 600.0 * 300 - 200;
                }
                if (TypeElements.ContainsKey(curtype) == false)
                {
                    TypeElements.Add(curtype, new TypeElement(curtype));
                    LeftCanvas.Children.Add(TypeElements[curtype]);
                }
                Types.Add(curtype);
                Viewbox el = TypeElements[curtype];
                Canvas.SetTop(el, 490 + y - 70);
            }
            List<ScienceType> RemoveElementsY = new List<ScienceType>();
            foreach (ScienceType i in TypeElements.Keys)
                if (Types.Contains(i) == false) RemoveElementsY.Add(i);
            foreach (ScienceType i in RemoveElementsY)
            {
                LeftCanvas.Children.Remove(TypeElements[i]);
                TypeElements.Remove(i);
            }

            List<ScienceType> rectlist = new List<ScienceType>();
            for (int i = -3; i < 4; i++)
            {
                int nextsciencetype = CurCenterType + i;
                if (nextsciencetype < 0 || nextsciencetype > (int)MaxType) continue;
                ScienceType type = (ScienceType)nextsciencetype;
                int pos = nextsciencetype * 600;
                int dy = (int)(pos - CurCenterY);
                double y = 0;
                if (dy > 0)
                {
                    if (dy < 300)
                        y = dy / 300.0 * 200;
                    else
                        y = (dy - 300) / 600.0 * 300 + 200;
                }
                else
                {
                    if (dy > -300)
                        y = dy / 300.0 * 200;
                    else
                        y = (dy + 300) / 600.0 * 300 - 200;
                }
                if (y < -640) continue;
                else if (y > 640) continue;
                if (BackRects.ContainsKey(type) == false)
                {
                    BackRects.Add(type, BackRectangle.GetRect(type));
                    MiddleCanvas.Children.Add(BackRects[type]);
                }
                rectlist.Add(type);
                Rectangle rect = BackRects[type];
                rect.Tag = 490 + y;
                rect.Height = 0;
                Canvas.SetTop(rect, 490 + y);

            }
            List<ScienceType> RemoveBacks = new List<ScienceType>();
            foreach (ScienceType i in BackRects.Keys)
                if (rectlist.Contains(i) == false) RemoveBacks.Add(i);
            foreach (ScienceType i in RemoveBacks)
            {
                MiddleCanvas.Children.Remove(BackRects[i]);
                BackRects.Remove(i);
            }
            for (int i = 0; i < BackRects.Count; i++)
            {
                Rectangle r = BackRects.Values[i];
                if (i == 0)
                {
                    double y1 = (double)(BackRects.Values[i + 1].Tag);
                    double y0 = (double)r.Tag;
                    r.Height = (y1 - y0) / 2 + y0;
                    Canvas.SetTop(r, 0);
                }
                else if (i == BackRects.Count - 1)
                {
                    double y0 = (double)(BackRects.Values[i - 1].Tag);
                    double y1 = (double)r.Tag;
                    r.Height = 980 - (y1 - y0) / 2 + y0;
                    Canvas.SetTop(r, (y1 - y0) / 2 + y0);
                }
                else
                {
                    double y0 = (double)(BackRects.Values[i - 1].Tag);
                    double y1 = (double)r.Tag;
                    double y2 = (double)(BackRects.Values[i + 1].Tag);
                    r.Height = (y2 - y1) / 2 + y1 - ((y1 - y0) / 2 + y0);
                    Canvas.SetTop(r, (y1 - y0) / 2 + y0);
                }
            }


            SortedSet<int> CurSciences = new SortedSet<int>();
            foreach (GameScience sc in Links.Science.GameSciences.Values)
            {
                //if (sc.Type != ScienceType.Science1) continue;
                int scLeft = sc.Level * 600;
                int scTop = ((int)sc.SType) * 600;
                if (scLeft < MinLevelValue || scLeft > MaxLevelValue) continue;
                if (scTop < MinLevelType || scTop > MaxLevelType) continue;
                if (sc.SType == ScienceType.None) continue;
                if (ImagingSciences.ContainsKey(sc.ID) == false)
                {
                    GameObjectImage img = new GameObjectImage(GOImageType.Hided, sc.ID);
                    img.PreviewMouseDown += Img_PreviewMouseDown;
                    ImagingSciences.Add(sc.ID, img);
                    MiddleCanvas.Children.Add(img);
                    Canvas.SetZIndex(img, 50);
                }
                CurSciences.Add(sc.ID);

                int dx = (int)(scLeft - CurCenterX);
                double x = 0;
                if (dx > 0)
                {
                    if (dx < 300)
                        x = dx / 300.0 * 200;
                    else if (dx < 900)
                        x = (dx - 300) / 600.0 * 300 + 200;
                    else if (dx < 1500)
                        x = (dx - 900) / 600.0 * 200 + 500;
                    else if (dx < 2300)
                        x = (dx - 1500) / 800.0 * 150 + 700;
                    else
                        x = (dx - 2300) / 1000.0 * 60.0 + 850;
                }
                else
                {
                    if (dx > -300)
                        x = dx / 300.0 * 200;
                    else if (dx > -900)
                        x = (dx + 300) / 600.0 * 300 - 200;
                    else if (dx > -1500)
                        x = (dx + 900) / 600.0 * 200 - 500;
                    else if (dx > -2300)
                        x = (dx + 1500) / 800.0 * 150 - 700;
                    else
                        x = (dx + 2300) / 1000.0 * 60.0 - 850;
                }

                int dy = (int)(scTop - CurCenterY);
                double y = 0;
                if (dy > 0)
                {
                    if (dy < 300)
                        y = dy / 300.0 * 200;
                    else
                        y = (dy - 300) / 600.0 * 300 + 200;
                }
                else
                {
                    if (dy > -300)
                        y = dy / 300.0 * 200;
                    else
                        y = (dy + 300) / 600.0 * 300 - 200;
                }

                GameObjectImage el = ImagingSciences[sc.ID];
                double ScaleXDelta = Math.Abs(x);
                double ScaleX = 1;
                if (ScaleXDelta < 350) ScaleX = (1.0 - 0.5) / 350 * (350 - ScaleXDelta) + 0.5;
                else if (ScaleXDelta < 600) ScaleX = (0.5 - 0.33) / 250 * (250 - ScaleXDelta + 350) + 0.33;
                else if (ScaleXDelta < 775) ScaleX = (0.33 - 0.2) / 175 * (175 - ScaleXDelta + 600) + 0.2;
                else ScaleX = (0.2 - 0.10) / 135 * (135 - ScaleXDelta + 775) + 0.10;
                double ScaleYDelta = Math.Abs(y);
                double ScaleY = 1;
                if (ScaleYDelta < 350) ScaleY = (1.0 - 0.5) / 350 * (350 - ScaleYDelta) + 0.5;
                else ScaleY = (0.5 - 0.33) / 250 * (250 - ScaleYDelta + 350) + 0.33;
                double Scale = ScaleX > ScaleY ? ScaleY : ScaleX;
                el.Width = 300 * Scale; el.Height = 400 * Scale;
                Canvas.SetLeft(el, 910 + x - el.Width / 2);
                Canvas.SetTop(el, 490 + y - el.Height / 2);
            }
            List<int> RemoveSciences = new List<int>();
            foreach (int i in ImagingSciences.Keys)
                if (CurSciences.Contains(i) == false) RemoveSciences.Add(i);
            foreach (int i in RemoveSciences)
            {
                MiddleCanvas.Children.Remove(ImagingSciences[i]);
                ImagingSciences.Remove(i);
            }
            
        }

        private void Img_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            GameObjectImage img = (GameObjectImage)sender;
            double left = Canvas.GetLeft(img);
            double top = Canvas.GetTop(img)+170;
            new GameObjectInfo(img.ScienceID, left, top, img.Width, img.Height);
        }

        private void MiddleCanvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            MouseDown = false;
        }

        private void MiddleCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseDown = true;
            pt = e.GetPosition(MiddleCanvas);
        }
    }
    public class LevelElement : Viewbox
    {
        public int Level;
        public LevelElement(int level)
        {
            Level = level;
            Width = 100; Height = 100;
            Border border = new Border();
            border.BorderThickness = new Thickness(3); border.BorderBrush = Links.Brushes.SkyBlue;
            border.Width = 100; border.Height = 100; border.CornerRadius = new CornerRadius(20);
            Child = border; border.Background = Brushes.Black;
            TextBlock tb = new TextBlock();
            tb.Width = 100;
            tb.TextAlignment = TextAlignment.Center;
            tb.Text = level.ToString();
            tb.Foreground = Brushes.White;
            tb.FontSize = 36;
            tb.VerticalAlignment = VerticalAlignment.Center; tb.HorizontalAlignment = HorizontalAlignment.Center;
            border.Child = tb;
        }
        public void ShowLineEventOn()
        {
            PreviewMouseDown += LevelElement_PreviewMouseDown;
        }

        private void LevelElement_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Links.Controller.PopUpCanvas.Remove();
            Links.Controller.Science3Canvas.Observer.CurCenterX = (Level) * 600;
            Links.Controller.Science3Canvas.Observer.Draw();
        }
    }
    public class TypeElement : Viewbox
    {
        public ScienceType Type;
        public TypeElement(ScienceType type)
        {
            Type = type;
            Width = 100; Height = 140;
            Border border = new Border(); border.BorderBrush = Links.Brushes.SkyBlue;
            border.BorderThickness = new Thickness(3); border.CornerRadius = new CornerRadius(20);
            border.Width = 100; border.Height = 140;
            border.Background = new SolidColorBrush(BackRectangle.GetColor(type));
            Child = border;
            Rectangle rect = Common.GetRectangle(90, null);
            rect.Margin = new Thickness(5, 25, 5, 25);
            border.Child = rect;
            rect.Fill = GetBrush(type);
          
        }
        public static Brush GetBrush(ScienceType type)
        {
            switch (type)
            {
                case ScienceType.Ship1Gun: return Gets.AddPicColony2("ship1gun");
                case ScienceType.Ship2Gun: return Gets.AddPicColony2("ship2gun"); 
                case ScienceType.Ship3Gun: return Gets.AddPicColony2("ship3gun"); 
                case ScienceType.Laser: return Links.Brushes.WeaponsBrushes[EWeaponType.Laser];
                case ScienceType.Emi: return Links.Brushes.WeaponsBrushes[EWeaponType.EMI]; 
                case ScienceType.Plasma: return Links.Brushes.WeaponsBrushes[EWeaponType.Plasma]; 
                case ScienceType.Solar: return Links.Brushes.WeaponsBrushes[EWeaponType.Solar];
                case ScienceType.Cannon: return Links.Brushes.WeaponsBrushes[EWeaponType.Cannon];
                case ScienceType.Gauss: return Links.Brushes.WeaponsBrushes[EWeaponType.Gauss];
                case ScienceType.Missle: return Links.Brushes.WeaponsBrushes[EWeaponType.Missle]; 
                case ScienceType.Anti: return Links.Brushes.WeaponsBrushes[EWeaponType.AntiMatter]; 
                case ScienceType.Psi: return Links.Brushes.WeaponsBrushes[EWeaponType.Psi]; 
                case ScienceType.Dark: return Links.Brushes.WeaponsBrushes[EWeaponType.Dark]; 
                case ScienceType.Warp: return Links.Brushes.WeaponsBrushes[EWeaponType.Warp];
                case ScienceType.Time: return Links.Brushes.WeaponsBrushes[EWeaponType.Time];
                case ScienceType.Slice: return Links.Brushes.WeaponsBrushes[EWeaponType.Slicing];
                case ScienceType.Rad: return Links.Brushes.WeaponsBrushes[EWeaponType.Radiation];
                case ScienceType.Drone: return Links.Brushes.WeaponsBrushes[EWeaponType.Drone];
                case ScienceType.Magnet: return Links.Brushes.WeaponsBrushes[EWeaponType.Magnet]; 
                case ScienceType.GeneratorS: return Links.Brushes.Items.GeneratorBrushes[ItemSize.Small]; 
                case ScienceType.GeneratorM: return Links.Brushes.Items.GeneratorBrushes[ItemSize.Medium]; 
                case ScienceType.GeneratorL: return Links.Brushes.Items.GeneratorBrushes[ItemSize.Large]; 
                case ScienceType.ShieldS: return Links.Brushes.Items.ShieldBrushes[ItemSize.Small]; 
                case ScienceType.ShieldM: return Links.Brushes.Items.ShieldBrushes[ItemSize.Medium]; 
                case ScienceType.ShieldL: return Links.Brushes.Items.ShieldBrushes[ItemSize.Large]; 
                case ScienceType.ComputerEn: return Links.Brushes.Items.ComputerBrushesG.Get(WeaponGroup.Energy, ItemSize.Large); 
                case ScienceType.ComputerPh: return Links.Brushes.Items.ComputerBrushesG.Get(WeaponGroup.Physic, ItemSize.Large); 
                case ScienceType.ComputerIr: return Links.Brushes.Items.ComputerBrushesG.Get(WeaponGroup.Irregular, ItemSize.Large); 
                case ScienceType.ComputerCy: return Links.Brushes.Items.ComputerBrushesG.Get(WeaponGroup.Cyber, ItemSize.Large); 
                case ScienceType.ComputerAl: return Links.Brushes.Items.ComputerBrushesG.Get(WeaponGroup.Any, ItemSize.Large); 
                case ScienceType.EngineS: return Links.Brushes.Items.EngineBrushes[ItemSize.Small];
                case ScienceType.EngineM: return Links.Brushes.Items.EngineBrushes[ItemSize.Medium]; 
                case ScienceType.EngineL: return Links.Brushes.Items.EngineBrushes[ItemSize.Large]; 
                case ScienceType.ModuleHealth: return Pictogram.HealthBrush; 
                case ScienceType.ModuleShield: return Pictogram.ShieldBrush; 
                case ScienceType.ModuleEnergy: return Pictogram.EnergyBrush; 
                case ScienceType.ModuleHRestore: return Pictogram.RestoreHealth;
                case ScienceType.ModuleSRestore: return Pictogram.RestoreShield; 
                case ScienceType.ModuleERestore: return Pictogram.RestoreEnergy; 
                case ScienceType.ModuleAccuracy: return Pictogram.Accuracy; 
                case ScienceType.ModuleEvasion: return Pictogram.Evasion; 
                case ScienceType.ModuleDamage: return Pictogram.Damage; 
                case ScienceType.ModuleIgnore: return Pictogram.Ignore; 
                case ScienceType.ModuleImmune: return Pictogram.Immune; 
                default: return Brushes.Yellow;
            }
        }
        public void ShowLineEventOn()
        {
            PreviewMouseDown += TypeElement_PreviewMouseDown;
        }

        private void TypeElement_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Links.Controller.PopUpCanvas.Remove();
            Links.Controller.Science3Canvas.Observer.CurCenterY = ((int)Type) * 600;
            Links.Controller.Science3Canvas.Observer.Draw();
        }
    }
    public class BackRectangle
    {
        public static Rectangle GetRect(ScienceType type)
        {
            Rectangle rect = new Rectangle();
            rect.Width = 1820; rect.Height = 300;
            Color c = GetColor(type);
            byte a = 255;
           
            c.A = a;
            rect.Opacity = 0.5;
            LinearGradientBrush l = new LinearGradientBrush();
            l.StartPoint = new Point(0.5, 0); l.EndPoint = new Point(0.5, 1);
            l.GradientStops.Add(new GradientStop(Colors.Black, 0.01));
            l.GradientStops.Add(new GradientStop(c, 0.02));
            //l.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            l.GradientStops.Add(new GradientStop(c, 0.98));
            l.GradientStops.Add(new GradientStop(Colors.Black, 0.99));

            rect.Fill = l;
            return rect;
        }
        public static Color GetColor(ScienceType type)
        {
            switch (type)
            {
                case ScienceType.Ship1Gun: return Color.FromRgb(255, 165, 0);  //orange
                case ScienceType.Ship2Gun: return Color.FromRgb(160, 82, 45);  //sienna
                case ScienceType.Ship3Gun: return Color.FromRgb(255, 140, 0);  //darkorange
                case ScienceType.Laser: return Color.FromRgb(0, 0, 205); //mediumblue
                case ScienceType.Emi: return Color.FromRgb(0, 0, 128);  //navy
                case ScienceType.Plasma: return Color.FromRgb(24, 116, 205); //dodgerblue3
                case ScienceType.Solar: return Color.FromRgb(0, 255, 255);  //cyan
                case ScienceType.Cannon: return Color.FromRgb(255, 36, 0);  //Алый
                case ScienceType.Gauss: return Color.FromRgb(229, 43, 80);//Амарантовый
                case ScienceType.Missle: return Color.FromRgb(176, 0, 0); //Бордовый
                case ScienceType.Anti: return Color.FromRgb(252, 64, 64); //Коралловый
                case ScienceType.Psi: return Color.FromRgb(255, 182, 193);  //LightPink
                case ScienceType.Dark: return Color.FromRgb(208, 132, 144); //VioletRed
                case ScienceType.Warp: return Color.FromRgb(255, 20, 147);//DeepPink
                case ScienceType.Time: return Color.FromRgb(148, 0, 211);  //DarkViolet
                case ScienceType.Slice: return Color.FromRgb(34, 139, 34); //ForestGreen
                case ScienceType.Rad: return Color.FromRgb(173, 255, 47);  //GreenYellow
                case ScienceType.Drone: return Color.FromRgb(124, 252, 0);  //LawnGreen
                case ScienceType.Magnet: return Color.FromRgb(0, 255, 127);  //SpringGreen
                case ScienceType.GeneratorS: return Colors.Yellow;
                case ScienceType.GeneratorM: return Color.FromRgb(0, 255, 127);  //SpringGreen
                case ScienceType.GeneratorL: return Color.FromRgb(24, 116, 205); //dodgerblue3
                case ScienceType.ShieldS: return Color.FromRgb(0, 255, 255);  //cyan
                case ScienceType.ShieldM: return Color.FromRgb(0, 0, 205); //mediumblue
                case ScienceType.ShieldL: return Color.FromRgb(0, 0, 128);  //navy
                case ScienceType.ComputerEn: return Color.FromRgb(0, 0, 205); //mediumblue
                case ScienceType.ComputerPh: return Color.FromRgb(229, 43, 80);//Амарантовый
                case ScienceType.ComputerIr: return Color.FromRgb(148, 0, 211);  //DarkViolet
                case ScienceType.ComputerCy: return Color.FromRgb(34, 139, 34); //ForestGreen
                case ScienceType.ComputerAl: return Colors.Yellow;
                case ScienceType.EngineS: return Color.FromRgb(173, 255, 47);  //GreenYellow
                case ScienceType.EngineM: return Color.FromRgb(124, 252, 0);  //LawnGreen
                case ScienceType.EngineL: return Color.FromRgb(0, 255, 127);  //SpringGreen
                case ScienceType.ModuleHealth: return Color.FromRgb(252, 64, 64); //Коралловый
                case ScienceType.ModuleShield: return Color.FromRgb(0, 0, 205); //mediumblue
                case ScienceType.ModuleEnergy: return Color.FromRgb(34, 139, 34); //ForestGreen
                case ScienceType.ModuleHRestore: return Color.FromRgb(252, 64, 64); //Коралловый
                case ScienceType.ModuleSRestore: return Color.FromRgb(0, 0, 205); //mediumblue
                case ScienceType.ModuleERestore: return Color.FromRgb(34, 139, 34); //ForestGreen
                case ScienceType.ModuleAccuracy: return Color.FromRgb(229, 43, 80);//Амарантовый 
                case ScienceType.ModuleEvasion: return Color.FromRgb(255, 165, 0);  //orange
                case ScienceType.ModuleDamage: return Colors.Yellow;
                case ScienceType.ModuleIgnore: return Color.FromRgb(34, 139, 34); //ForestGreen
                case ScienceType.ModuleImmune: return Color.FromRgb(0, 255, 255);  //cyan
                default: return Colors.Yellow;
            }
        }
    }
    public class GameObjectInfo:Canvas
    {
        public GameObjectInfo(ushort id, double left, double top, double width, double height)
        {
            Width = 1920; Height = 1080;
            GameScience science = Links.Science.GameSciences[id];
            GameObjectImage img = new GameObjectImage(GOImageType.Hided, id);
            Children.Add(img);
            img.Width = width; img.Height = height;
            Canvas.SetLeft(img, left); Canvas.SetTop(img, top);
            DoubleAnimation MoveLeft = new DoubleAnimation(left, 100, TimeSpan.FromSeconds(0.5));
            DoubleAnimation MoveTop = new DoubleAnimation(top, 100, TimeSpan.FromSeconds(0.5));
            img.BeginAnimation(Canvas.LeftProperty, MoveLeft);
            img.BeginAnimation(Canvas.TopProperty, MoveTop);
            DoubleAnimation GrowUp = new DoubleAnimation(height, 800, TimeSpan.FromSeconds(0.5));
            DoubleAnimation GrowDown = new DoubleAnimation(width, 600, TimeSpan.FromSeconds(0.5));
            img.BeginAnimation(Viewbox.HeightProperty, GrowUp);
            img.BeginAnimation(Viewbox.WidthProperty, GrowDown);
            Links.Controller.PopUpCanvas.Place(this, true);

            Viewbox Info = new Viewbox();
            Info.Width = 500; 
            switch (science.Type)
            {
                case Links.Science.EType.ShipTypes: Info.Child = new ShipTypeLargeInfo(Links.ShipTypes[id]); break;
                case Links.Science.EType.Weapon: Info.Child = new WeaponLargeInfo(Links.WeaponTypes[id]); break;
                case Links.Science.EType.Generator: Info.Child = new GeneratorLargeInfo(Links.GeneratorTypes[id]); break;
                case Links.Science.EType.Shield: Info.Child = new ShieldLargeInfo(Links.ShieldTypes[id]); break;
                case Links.Science.EType.Computer: Info.Child = new ComputerLargeInfo(Links.ComputerTypes[id]); break;
                case Links.Science.EType.Engine: Info.Child = new EngineLargeInfo(Links.EngineTypes[id]); break;
                case Links.Science.EType.Equipment: Info.Child = new EquipLargeInfo(Links.EquipmentTypes[id]); break;
                default: return;
            }
            Children.Add(Info);
            Canvas.SetLeft(Info, 1000); Canvas.SetTop(Info, 150);
            DoubleAnimationUsingKeyFrames showanim = new DoubleAnimationUsingKeyFrames();
            showanim.Duration = TimeSpan.FromSeconds(1);
            showanim.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0)));
            showanim.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.5)));
            showanim.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromPercent(1)));
            Info.BeginAnimation(FrameworkElement.OpacityProperty, showanim);

        }

    }
    class BasicLargeInfo:Border
    {
        internal StackPanel panel;
        internal bool unknown=false;
        public BasicLargeInfo(string titletext, ushort id)
        {
            Width = 400;
            BorderBrush = Links.Brushes.SkyBlue;
            BorderThickness = new Thickness(2);
            CornerRadius = new CornerRadius(10);
            Background = Brushes.Black;
            panel = new StackPanel(); panel.Orientation = Orientation.Vertical;
            Child = panel;
            TextBlock title = Common.GetBlock(32, titletext, Links.Brushes.SkyBlue, 390);
            panel.Children.Add(title);

            if (GSGameInfo.SciencesArray.Contains(id) == false)
            {
                Border UnknownBorder = new Border(); UnknownBorder.Width = 390; UnknownBorder.BorderBrush = Links.Brushes.SkyBlue;
                UnknownBorder.CornerRadius = new CornerRadius(10); UnknownBorder.Background = Brushes.Black; UnknownBorder.BorderThickness = new Thickness(2);
                panel.Children.Add(UnknownBorder);
                UnknownBorder.Child = Common.GetBlock(30, "Параметры неизвестны", Links.Brushes.SkyBlue, 380);
                UnknownBorder.Height = 300;
                unknown = true;
                return;
            }

        }
        internal StackPanel GetInnerPanel(string title)
        {
            Border BaseParams = new Border(); BaseParams.Width = 390; BaseParams.BorderBrush = Links.Brushes.SkyBlue;
            BaseParams.CornerRadius = new CornerRadius(10); BaseParams.Background = Brushes.Black; BaseParams.BorderThickness = new Thickness(2);
            panel.Children.Add(BaseParams);
            StackPanel BasePanel = new StackPanel();
            BaseParams.Child = BasePanel;
            BasePanel.Orientation = Orientation.Vertical;
            TextBlock parameters = Common.GetBlock(26, title, Brushes.White, 380);
            BasePanel.Children.Add(parameters);
            return BasePanel;
        }
        internal TextBlock GetParamBlock(string text, int param, Brush color, string finishtext)
        {
            TextBlock block = Common.GetBlock(24, text, Brushes.White, 380);
            Run run = new Run(param.ToString());
            run.Foreground = color;
            block.Inlines.Add(run);
            block.Inlines.Add(new Run(finishtext));
            block.TextAlignment = TextAlignment.Left;
            return block;
        }
        internal TextBlock GetSizeBlock(ItemSize size)
        {
            TextBlock block = Common.GetBlock(24, "Размер: ", Brushes.White, 380);
            Run sizerun = null;
            switch (size)
            {
                case ItemSize.Small: sizerun = new Run(" Малый"); sizerun.Foreground = Links.Brushes.Green; break;
                case ItemSize.Medium: sizerun = new Run(" Средний"); sizerun.Foreground = Brushes.Blue; break;
                case ItemSize.Large: sizerun = new Run(" Большой"); sizerun.Foreground = Brushes.Red; break;
            }
            block.Inlines.Add(sizerun);
            block.TextAlignment = TextAlignment.Left;
            return block;
        }

        internal TextBlock GetLevelBlock(int level)
        {
            TextBlock block = Common.GetBlock(24, "Уровень: ", Brushes.White, 380);
            Run run = new Run(level.ToString()); run.Foreground = Links.Brushes.SkyBlue;
            block.Inlines.Add(run);
            block.TextAlignment = TextAlignment.Left;
            return block;
        }
        internal TextBlock GetLengthBlock(string text, ItemSize size)
        {
            TextBlock block = Common.GetBlock(24, text, Brushes.White, 380);
            Run r = new Run();
            switch (size)
            {
                case ItemSize.Small: r.Foreground = Brushes.Red; r.Text = "3 раунда"; break;
                case ItemSize.Medium: r.Foreground = Brushes.Blue; r.Text = "2 раунда"; break;
                case ItemSize.Large: r.Foreground = Links.Brushes.Green; r.Text = "1 раунд"; break;
            }
            block.Inlines.Add(r);
            block.TextAlignment = TextAlignment.Left;
            return block;
        }
        internal TextBlock GetWeaponType(WeaponGroup group)
        {
            TextBlock block = Common.GetBlock(24, "Тип урона: ", Brushes.White, 380);

            Run grouprun = null;
            switch (group)
            {
                case WeaponGroup.Energy: grouprun = new Run("Энергетический"); grouprun.Foreground = Brushes.Blue; break;
                case WeaponGroup.Physic: grouprun = new Run("Физический"); grouprun.Foreground = Brushes.Red; break;
                case WeaponGroup.Irregular: grouprun = new Run("Аномальный"); grouprun.Foreground = Brushes.Violet; break;
                case WeaponGroup.Cyber: grouprun = new Run("Кибернетический"); grouprun.Foreground = Links.Brushes.Green; break;
                case WeaponGroup.Any: grouprun = new Run("Любой"); grouprun.Foreground = Brushes.Red; break;
            }
            block.Inlines.Add(grouprun);
            block.TextAlignment = TextAlignment.Left;
            return block;
        }
    }
    class EquipLargeInfo : BasicLargeInfo
    {
        public EquipLargeInfo(Equipmentclass equip) : base("Дополнительное оборудование", equip.ID)
        {
            TextBlock name = Common.GetBlock(30, equip.GetName(), Brushes.White, 390);
            panel.Children.Add(name);
            if (unknown)
            {
                UIElement el = panel.Children[panel.Children.Count - 2];
                panel.Children.Remove(el); panel.Children.Add(el); return;
            }

            StackPanel BasePanel = GetInnerPanel("Базовые параметры");
            BasePanel.Children.Add(GetLevelBlock(equip.Level));
            BasePanel.Children.Add(GetSizeBlock(equip.Size));

            StackPanel BattleParamsPanel = GetInnerPanel("Боевые параметры");
            switch (equip.EqType)
            {
                case EquipmentType.Hull: if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает прочность корпуса группы кораблей на: ", equip.Value, Brushes.Red, " ед"));
                else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает прочность корпуса корабля на: ", equip.Value, Brushes.Red, " ед")); break;
                case EquipmentType.Shield: if (equip.Size==ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает прочность силового щита группы кораблей на: ", equip.Value, Brushes.Blue, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает прочность силового щита корабля на: ", equip.Value, Brushes.Blue, " ед")); break;
                case EquipmentType.Energy:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает запас энергии генератора группы кораблей на: ", equip.Value, Links.Brushes.Green, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает запас энергии генератора корабля на: ", equip.Value, Links.Brushes.Green, " ед")); break;
                case EquipmentType.HullRegen:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Восстанавливает прочность корпуса группы кораблей на: ", equip.Value, Brushes.Red, " ед/раунд"));
                else
                        BattleParamsPanel.Children.Add(GetParamBlock("Восстанавливает прочность корпуса корабля на: ", equip.Value, Brushes.Red, " ед/раунд")); break;
                case EquipmentType.ShieldRegen:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Восстанавливает прочность силового щита группы кораблей на: ", equip.Value, Brushes.Blue, " ед/раунд"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Восстанавливает прочность силового щита корабля на: ", equip.Value, Brushes.Blue, " ед/раунд")); break;
                case EquipmentType.EnergyRegen:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Восстанавливает запас энергии генератора группы кораблей на: ", equip.Value, Links.Brushes.Green, " ед/раунд"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Восстанавливает запас энергии генератора корабля на: ", equip.Value, Links.Brushes.Green, " ед/раунд")); break;
                case EquipmentType.AccEn:
                    if (equip.Size==ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает точность", WeaponGroup.Energy, "вооружения группы кораблей на: ", equip.Value, Brushes.Blue, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает точность", WeaponGroup.Energy, "вооружения корабля на: ", equip.Value, Brushes.Blue, " ед")); break;
                case EquipmentType.AccPh:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает точность", WeaponGroup.Physic, "вооружения группы кораблей на: ", equip.Value, Brushes.Red, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает точность", WeaponGroup.Physic, "вооружения корабля на: ", equip.Value, Brushes.Red, " ед")); break;
                case EquipmentType.AccIr:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает точность", WeaponGroup.Irregular, "вооружения группы кораблей на: ", equip.Value, Brushes.Purple, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает точность", WeaponGroup.Irregular, "вооружения корабля на: ", equip.Value, Brushes.Purple, " ед")); break;
                case EquipmentType.AccCy:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает точность", WeaponGroup.Cyber, "вооружения группы кораблей на: ", equip.Value, Links.Brushes.Green, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает точность", WeaponGroup.Cyber, "вооружения корабля на: ", equip.Value, Links.Brushes.Green, " ед")); break;
                case EquipmentType.AccAl:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает точность", WeaponGroup.Any, "вооружения группы кораблей на: ", equip.Value, Brushes.Gold, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает точность", WeaponGroup.Any, "вооружения корабля на: ", equip.Value, Brushes.Gold, " ед")); break;
                case EquipmentType.EvaEn:
                    if (equip.Size==ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает уклонение группы кораблей против", WeaponGroup.Energy, "вооружения на: ", equip.Value, Brushes.Blue, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает уклонение корабля против", WeaponGroup.Energy, "вооружения на: ", equip.Value, Brushes.Blue, " ед")); break;
                case EquipmentType.EvaPh:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает уклонение группы кораблей против", WeaponGroup.Physic, "вооружения на: ", equip.Value, Brushes.Red, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает уклонение корабля против", WeaponGroup.Physic, "вооружения на: ", equip.Value, Brushes.Red, " ед")); break;
                case EquipmentType.EvaIr:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает уклонение группы кораблей против", WeaponGroup.Irregular, "вооружения на: ", equip.Value, Brushes.Purple, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает уклонение корабля против", WeaponGroup.Irregular, "вооружения на: ", equip.Value, Brushes.Purple, " ед")); break;
                case EquipmentType.EvaCy:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает уклонение группы кораблей против", WeaponGroup.Cyber, "вооружения на: ", equip.Value, Links.Brushes.Green, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает уклонение корабля против", WeaponGroup.Cyber, "вооружения на: ", equip.Value, Links.Brushes.Green, " ед")); break;
                case EquipmentType.EvaAl:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает уклонение группы кораблей против", WeaponGroup.Any, "вооружения на: ", equip.Value, Brushes.Gold, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает уклонение корабля против", WeaponGroup.Any, "вооружения на: ", equip.Value, Brushes.Gold, " ед")); break;
                case EquipmentType.DamEn:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает урон", WeaponGroup.Energy, "вооружения группы кораблей на: ", equip.Value, Brushes.Blue, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает урон", WeaponGroup.Energy, "вооружения корабля на: ", equip.Value, Brushes.Blue, " ед")); break;
                case EquipmentType.DamPh:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает урон", WeaponGroup.Physic, "вооружения группы кораблей на: ", equip.Value, Brushes.Red, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает урон", WeaponGroup.Physic, "вооружения корабля на: ", equip.Value, Brushes.Red, " ед")); break;
                case EquipmentType.DamIr:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает урон", WeaponGroup.Irregular, "вооружения группы кораблей на: ", equip.Value, Brushes.Purple, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает урон", WeaponGroup.Irregular, "вооружения корабля на: ", equip.Value, Brushes.Purple, " ед")); break;
                case EquipmentType.DamCy:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает урон", WeaponGroup.Cyber, "вооружения группы кораблей на: ", equip.Value, Links.Brushes.Green, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает урон", WeaponGroup.Cyber, "вооружения корабля на: ", equip.Value, Links.Brushes.Green, " ед")); break;
                case EquipmentType.DamAl:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает урон", WeaponGroup.Any, "вооружения группы кораблей на: ", equip.Value, Brushes.Gold, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает урон", WeaponGroup.Any, "вооружения корабля на: ", equip.Value, Brushes.Gold, " ед")); break;
                case EquipmentType.IgnEn:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает бронирование группы кораблей против", WeaponGroup.Energy, "вооружения на: ", equip.Value, Brushes.Blue, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает бронирование корабля против", WeaponGroup.Energy, "вооружения на: ", equip.Value, Brushes.Blue, " ед")); break;
                case EquipmentType.IgnPh:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает бронирование группы кораблей против", WeaponGroup.Physic, "вооружения на: ", equip.Value, Brushes.Red, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает бронирование корабля против", WeaponGroup.Physic, "вооружения на: ", equip.Value, Brushes.Red, " ед")); break;
                case EquipmentType.IgnIr:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает бронирование группы кораблей против", WeaponGroup.Irregular, "вооружения на: ", equip.Value, Brushes.Purple, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает бронирование корабля против", WeaponGroup.Irregular, "вооружения на: ", equip.Value, Brushes.Purple, " ед")); break;
                case EquipmentType.IgnCy:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает бронирование группы кораблей против", WeaponGroup.Cyber, "вооружения на: ", equip.Value, Links.Brushes.Green, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает бронирование корабля против", WeaponGroup.Cyber, "вооружения на: ", equip.Value, Links.Brushes.Green, " ед")); break;
                case EquipmentType.IgnAl:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает бронирование группы кораблей против", WeaponGroup.Any, "вооружения на: ", equip.Value, Brushes.Gold, " ед"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает бронирование корабля против", WeaponGroup.Any, "вооружения на: ", equip.Value, Brushes.Gold, " ед")); break;
                case EquipmentType.ImmEn:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает шанс блокировать критический эффект", WeaponGroup.Energy, "вооружения для группы кораблей на: ", equip.Value, Brushes.Blue, "%"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает шанс блокировать критический эффект", WeaponGroup.Energy, "вооружения для корабля на: ", equip.Value, Brushes.Blue, "%")); break;
                case EquipmentType.ImmPh:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает шанс блокировать критический эффект", WeaponGroup.Physic, "вооружения для группы кораблей на: ", equip.Value, Brushes.Red, "%"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает шанс блокировать критический эффект", WeaponGroup.Physic, "вооружения для корабля на: ", equip.Value, Brushes.Red, "%")); break;
                case EquipmentType.ImmIr:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает шанс блокировать критический эффект", WeaponGroup.Irregular, "вооружения для группы кораблей на: ", equip.Value, Brushes.Purple, "%"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает шанс блокировать критический эффект", WeaponGroup.Irregular, "вооружения для корабля на: ", equip.Value, Brushes.Purple, "%")); break;
                case EquipmentType.ImmCy:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает шанс блокировать критический эффект", WeaponGroup.Cyber, "вооружения для группы кораблей на: ", equip.Value, Links.Brushes.Green, "%"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает шанс блокировать критический эффект", WeaponGroup.Cyber, "вооружения для корабля на: ", equip.Value, Links.Brushes.Green, "%")); break;
                case EquipmentType.ImmAl:
                    if (equip.Size == ItemSize.Large)
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает шанс блокировать критический эффект", WeaponGroup.Any, "вооружения для группы кораблей на: ", equip.Value, Brushes.Gold, "%"));
                    else
                        BattleParamsPanel.Children.Add(GetParamBlock("Увеличивает шанс блокировать критический эффект", WeaponGroup.Any, "вооружения для корабля на: ", equip.Value, Brushes.Gold, "%")); break;

            }
            if (equip.Consume>0) BattleParamsPanel.Children.Add(GetParamBlock("Расход энергии: ", equip.Consume, Links.Brushes.Green, " ед/раунд"));
        }
        internal TextBlock GetParamBlock(string text1, WeaponGroup group, string text2, int param, Brush color, string finishtext)
        {
            TextBlock block = Common.GetBlock(24, text1, Brushes.White, 380);
            Run grouprun;
            switch (group)
            {
                case WeaponGroup.Energy: grouprun = new Run(" энергетического "); grouprun.Foreground = Brushes.Blue; break;
                case WeaponGroup.Physic: grouprun = new Run(" физического "); grouprun.Foreground = Brushes.Red; break;
                case WeaponGroup.Irregular: grouprun = new Run(" аномального "); grouprun.Foreground = Brushes.Purple; break;
                case WeaponGroup.Cyber: grouprun = new Run(" кибернетического "); grouprun.Foreground = Links.Brushes.Green; break;
                default: grouprun = new Run(" любого "); grouprun.Foreground = Brushes.Gold; break;
            }
            block.Inlines.Add(grouprun);
            block.Inlines.Add(new Run(text2));
            Run run = new Run(param.ToString());
            run.Foreground = color;
            block.Inlines.Add(run);
            block.Inlines.Add(new Run(finishtext));
            block.TextAlignment = TextAlignment.Left;
            return block;
        }
    }
    class EngineLargeInfo : BasicLargeInfo
    {
        public EngineLargeInfo(Engineclass engine) : base("Двигатель", engine.ID)
        {
            TextBlock name = Common.GetBlock(30, engine.GetName(), Brushes.White, 390);
            panel.Children.Add(name);
            if (unknown)
            {
                UIElement el = panel.Children[panel.Children.Count - 2];
                panel.Children.Remove(el); panel.Children.Add(el); return;
            }

            StackPanel BasePanel = GetInnerPanel("Базовые параметры");
            BasePanel.Children.Add(GetLevelBlock(engine.Level));
            BasePanel.Children.Add(GetSizeBlock(engine.Size));

            StackPanel BattleParamsPanel = GetInnerPanel("Боевые параметры");
            if (engine.EnergyEvasion > 0) BattleParamsPanel.Children.Add(GetParamBlock("Уклонение от энергетических атак: ", engine.EnergyEvasion, Brushes.Blue, " ед"));
            if (engine.PhysicEvasion > 0) BattleParamsPanel.Children.Add(GetParamBlock("Уклонение от физических атак: ", engine.PhysicEvasion, Brushes.Red, " ед"));
            if (engine.IrregularEvasion > 0) BattleParamsPanel.Children.Add(GetParamBlock("Уклонение от аномальных атак: ", engine.IrregularEvasion, Brushes.Purple, " ед"));
            if (engine.CyberEvasion > 0) BattleParamsPanel.Children.Add(GetParamBlock("Уклонение от кибернетических атак: ", engine.CyberEvasion, Links.Brushes.Green, " ед"));
            BattleParamsPanel.Children.Add(GetParamBlock("Расход энергии: ", engine.Consume, Links.Brushes.Green, " ед/раунд"));
            if (engine.Size == ItemSize.Large) BattleParamsPanel.Children.Add(GetParamBlock("Расход энергии при перемещении на один хекс: ", 60, Links.Brushes.Green, "%"));
            else if (engine.Size == ItemSize.Medium) BattleParamsPanel.Children.Add(GetParamBlock("Расход энергии при перемещении на один хекс: ", 40, Brushes.Blue, "%"));
            else BattleParamsPanel.Children.Add(GetParamBlock("Расход энергии при перемещении на один хекс: ", 20, Brushes.Red, "%"));
        }
    }
    class ComputerLargeInfo : BasicLargeInfo
    {
        public ComputerLargeInfo(Computerclass comp) : base("Вычислитель", comp.ID)
        {
            TextBlock name = Common.GetBlock(30, comp.GetName(), Brushes.White, 390);
            panel.Children.Add(name);
            if (unknown)
            {
                UIElement el = panel.Children[panel.Children.Count - 2];
                panel.Children.Remove(el); panel.Children.Add(el); return;
            }

            StackPanel BasePanel = GetInnerPanel("Базовые параметры");
            BasePanel.Children.Add(GetLevelBlock(comp.Level));
            BasePanel.Children.Add(GetSizeBlock(comp.Size));

            StackPanel BattleParamsPanel = GetInnerPanel("Боевые параметры");
            BattleParamsPanel.Children.Add(GetWeaponType((WeaponGroup)comp.GetWeaponGroupValue()));
            BattleParamsPanel.Children.Add(GetParamBlock("Бонус точности: ", comp.MaxAccuracy, Links.Brushes.SkyBlue, " ед"));
            if (comp.MaxDamage>0)
                BattleParamsPanel.Children.Add(GetParamBlock("Бонус урона: ", comp.MaxDamage, Brushes.Red, " ед"));
            BattleParamsPanel.Children.Add(GetParamBlock("Расход энергии: ", comp.Consume, Links.Brushes.Green, " ед/раунд"));
            if (comp.Size == ItemSize.Large) BattleParamsPanel.Children.Add(GetParamBlock("Дальность прыжка в бою: ", 3, Links.Brushes.Green, " хекса"));
            else if (comp.Size == ItemSize.Medium) BattleParamsPanel.Children.Add(GetParamBlock("Дальность прыжка в бою: ", 2, Brushes.Blue, " хекса"));
            else BattleParamsPanel.Children.Add(GetParamBlock("Дальность прыжка в бою: ", 1, Brushes.Red, " хекс"));
        }
    }
    class ShieldLargeInfo : BasicLargeInfo
    {
        public ShieldLargeInfo(Shieldclass shield):base("Генератор силового щита", shield.ID)
        {
            TextBlock name = Common.GetBlock(30, shield.GetName(), Brushes.White, 390);
            panel.Children.Add(name);
            if (unknown)
            {
                UIElement el = panel.Children[panel.Children.Count - 2];
                panel.Children.Remove(el); panel.Children.Add(el); return;
            }

            StackPanel BasePanel = GetInnerPanel("Базовые параметры");
            BasePanel.Children.Add(GetLevelBlock(shield.Level));
            BasePanel.Children.Add(GetSizeBlock(shield.Size));

            StackPanel BattleParamsPanel = GetInnerPanel("Боевые параметры");
            BattleParamsPanel.Children.Add(GetParamBlock("Прочность щита: ", shield.Capacity, Links.Brushes.SkyBlue," ед"));
            BattleParamsPanel.Children.Add(GetParamBlock("Регенерация щита: ", shield.Recharge, Brushes.Red," ед/раунд"));
            BattleParamsPanel.Children.Add(GetParamBlock("Расход энергии: ", shield.Consume, Links.Brushes.Green," ед/раунд"));
            if (shield.Size==ItemSize.Large) BattleParamsPanel.Children.Add(GetParamBlock("Защищаемый контур: ", 270,Links.Brushes.Green, " градусов"));
            else if (shield.Size == ItemSize.Medium) BattleParamsPanel.Children.Add(GetParamBlock("Защищаемый контур: ", 180, Brushes.Blue, " градусов"));
            else BattleParamsPanel.Children.Add(GetParamBlock("Защищаемый контур: ", 90, Brushes.Red, " градусов"));
        }
    }
    class GeneratorLargeInfo : BasicLargeInfo
    {
        public GeneratorLargeInfo(Generatorclass gen):base ("Генератор энергии", gen.ID)
        {
            TextBlock name = Common.GetBlock(30, gen.GetName(), Brushes.White, 390);
            panel.Children.Add(name);
            if (unknown)
            {
                UIElement el = panel.Children[panel.Children.Count - 2];
                panel.Children.Remove(el); panel.Children.Add(el); return;
            }

            StackPanel BasePanel = GetInnerPanel("Базовые параметры");
            BasePanel.Children.Add(GetLevelBlock(gen.Level));
            BasePanel.Children.Add(GetSizeBlock(gen.Size));
            
            StackPanel BattleParamsPanel = GetInnerPanel("Боевые параметры");
            BattleParamsPanel.Children.Add(GetParamBlock("Запас энергии: ", gen.Capacity, Links.Brushes.SkyBlue," ед"));
            BattleParamsPanel.Children.Add(GetParamBlock("Выработка энергии: ", gen.Recharge, Brushes.Red, " ед/раунд"));
            BattleParamsPanel.Children.Add(GetLengthBlock("Длительность подготовки к прыжку на поле боя: ", gen.Size));
        }
    }
    class WeaponLargeInfo : BasicLargeInfo
    {
        public WeaponLargeInfo(Weaponclass weapon):base("Вооружение", weapon.ID)
        {
            TextBlock name = Common.GetBlock(30, weapon.GetName(), Brushes.White, 390);
            panel.Children.Add(name);
            if (unknown)
            {
                UIElement el = panel.Children[panel.Children.Count - 2];
                panel.Children.Remove(el); panel.Children.Add(el); return;
            }

            StackPanel BasePanel = GetInnerPanel("Базовые параметры");
            BasePanel.Children.Add(GetLevelBlock(weapon.Level));
            BasePanel.Children.Add(GetSizeBlock(weapon.Size));
            BasePanel.Children.Add(GetWeaponType(weapon.Group));

            StackPanel WeaponPanel = GetInnerPanel("Боевые параметры");
            WeaponPanel.Children.Add(GetAccBlock(weapon.Type));
            WeaponPanel.Children.Add(GetParamBlock("Урон по щиту: ", weapon.ShieldDamage, Links.Brushes.SkyBlue, " ед"));
            WeaponPanel.Children.Add(GetParamBlock("Урон по корпусу: ", weapon.HealthDamage, Brushes.Red, " ед"));
            WeaponPanel.Children.Add(GetParamBlock("Расход энергии: ", weapon.Consume, Links.Brushes.Green, " ед"));

            StackPanel CritPanel = GetInnerPanel("Критический урон");
            TextBlock critchance = GetParamBlock("Шанс критического урона: ", Links.Modifiers[weapon.Type].Crit[weapon.Size], Brushes.Orange, "%");
            CritPanel.Children.Add(critchance);
            TextBlock critinfo = Common.GetBlock(26, "Описание критического эффекта:", Brushes.White, 380);
            critinfo.Inlines.Add(new LineBreak());
            critinfo.Inlines.Add(new Run(GameObjectName.GetWeaponDescription(weapon)));
            critinfo.TextAlignment = TextAlignment.Left;
            CritPanel.Children.Add(critinfo);
        }
        TextBlock GetAccBlock(EWeaponType type)
        {

            TextBlock block = Common.GetBlock(24, "Точность: ", Brushes.White, 380);
            Run run = null;
            int val = Links.Modifiers[type].Accuracy;
            switch (val)
            {
                case 2: run = new Run("Максимальная"); run.Foreground = Links.Brushes.Green; break;
                case 3: run = new Run("Высокая"); run.Foreground = Brushes.Blue; break;
                case 4: run = new Run("Средняя"); run.Foreground = Brushes.Yellow; break;
                case 5: run = new Run("Низкая"); run.Foreground = Brushes.Red; break;
            }
            block.Inlines.Add(run);
            block.TextAlignment = TextAlignment.Left;
            return block;
        }
    }

    class ShipTypeLargeInfo : BasicLargeInfo
    {
        public ShipTypeLargeInfo(ShipTypeclass shiptype):base("Модель корабля", shiptype.ID)
        {
            TextBlock name = Common.GetBlock(30, shiptype.GetName(), Brushes.White, 390);
            panel.Children.Add(name);
            if (unknown)
            {
                UIElement el = panel.Children[panel.Children.Count - 2];
                panel.Children.Remove(el); panel.Children.Add(el); return;
            }

            StackPanel BasePanel = GetInnerPanel("Базовые параметры");
            BasePanel.Children.Add(GetLevelBlock(shiptype.Level));
            BasePanel.Children.Add(GetParamBlock("Прочность корпуса: ", shiptype.Health, Links.Brushes.SkyBlue, " ед"));
            BasePanel.Children.Add(GetParamBlock("Общее бронирование: ", shiptype.Armor, Links.Brushes.SkyBlue, " ед"));

            StackPanel WeaponPanel = GetInnerPanel("Допустимое вооружение");
            for (int i = 0; i < shiptype.WeaponCapacity; i++)
                WeaponPanel.Children.Add(GetWeaponBlock(shiptype.WeaponsParam[i].Size, shiptype.WeaponsParam[i].Group, i, shiptype.WeaponCapacity));

            StackPanel StandartModulesPanel = GetInnerPanel("Допустимые стандартные модули");
            StandartModulesPanel.Children.Add(GetSizeBlock(shiptype.GeneratorSize, "Генератор: "));
            StandartModulesPanel.Children.Add(GetSizeBlock(shiptype.ShieldSize, "Генератор щита: "));
            StandartModulesPanel.Children.Add(GetSizeBlock(shiptype.ComputerSize, "Вычислитель: "));
            StandartModulesPanel.Children.Add(GetSizeBlock(shiptype.EngineSize, "Двигатель: "));
            
            StackPanel EquipsPanel = GetInnerPanel("Допустимые дополнительные модули");
            for (int i=0;i<shiptype.EquipmentCapacity;i++)
            {
                EquipsPanel.Children.Add(GetSizeBlock(shiptype.EquipmentsSize[i], string.Format("Модуль {0}: ", i + 1)));
            }
        }
        TextBlock GetSizeBlock(ItemSize size, string title)
        {
            TextBlock block = Common.GetBlock(24, title, Brushes.White, 380);
            Run sizerun = null;
            switch (size)
            {
                case ItemSize.Small: sizerun = new Run(" Малое"); sizerun.Foreground = Links.Brushes.Green; break;
                case ItemSize.Medium: sizerun = new Run(" Малое и Среднее"); sizerun.Foreground = Brushes.Blue; break;
                case ItemSize.Large: sizerun = new Run(" Любого размера"); sizerun.Foreground = Brushes.Red; break;
            }
            block.Inlines.Add(sizerun);
            block.TextAlignment = TextAlignment.Left;
            return block;
        }
        TextBlock GetWeaponBlock(ItemSize size, WeaponGroup group, int curgun, int maxgun)
        {
            TextBlock block = Common.GetBlock(24, "", Brushes.White, 380);
            if (curgun == 0)
            {
                if (maxgun == 1)
                    block.Inlines.Add(new Run("Центральное орудие: "));
                else
                    block.Inlines.Add(new Run("Левое орудие: "));
            }
            else if (curgun == 1)
            {
                if (maxgun == 2)
                    block.Inlines.Add(new Run("Правое орудие: "));
                else
                    block.Inlines.Add(new Run("Центральное орудие: "));
            }
            else
                block.Inlines.Add(new Run("Правое орудие: "));
            Run sizerun = null;
            switch (size)
            {
                case ItemSize.Small: sizerun = new Run("Малое "); sizerun.Foreground = Links.Brushes.Green; break;
                case ItemSize.Medium: sizerun = new Run("Малое и Среднее "); sizerun.Foreground = Brushes.Blue; break;
                case ItemSize.Large: sizerun = new Run("Любого размера "); sizerun.Foreground = Brushes.Red; break;
            }
            block.Inlines.Add(sizerun);
            Run grouprun = null;
            switch (group)
            {
                case WeaponGroup.Energy: grouprun = new Run("Энергетическое"); grouprun.Foreground = Brushes.Blue; break;
                case WeaponGroup.Physic: grouprun = new Run("Физическое"); grouprun.Foreground = Brushes.Red; break;
                case WeaponGroup.Irregular: grouprun = new Run("Аномальное"); grouprun.Foreground = Brushes.Violet; break;
                case WeaponGroup.Cyber: grouprun = new Run("Кибернетическое"); grouprun.Foreground = Links.Brushes.Green; break;
                case WeaponGroup.Any: grouprun = new Run("Любой группы"); grouprun.Foreground = Brushes.Gold; break;
            }
            block.Inlines.Add(grouprun);
            block.TextAlignment = TextAlignment.Left;
            return block;
        }       
    }
}
