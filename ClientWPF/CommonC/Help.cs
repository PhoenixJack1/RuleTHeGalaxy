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
    class HelpStart:Canvas
    {
        public HelpStart()
        {
            Width = 1920;
            Height = 1080;
            Canvas.SetZIndex(this, 251);
            Background = new SolidColorBrush(Color.FromArgb(150, 0, 0, 0));
            GameButton show = new GameButton(1000, 300, "Показать обучение", 60);
            Children.Add(show);
            Canvas.SetLeft(show, 460);
            Canvas.SetTop(show, 200);
            show.PreviewMouseDown += (sender, e) => { Hide(); Links.Controller.HelpCanvas.Show(-2); };
            GameButton hide = new GameButton(1000, 300, "Скрыть обучение", 60);
            Children.Add(hide);
            Canvas.SetLeft(hide, 460);
            Canvas.SetTop(hide, 600);
            hide.PreviewMouseDown += (sender, e) => { Hide(); };
        }
        public void Show()
        {
            if (!Links.Controller.MainCanvas.Children.Contains(this))
                Links.Controller.MainCanvas.Children.Add(this);
            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(3));
            this.BeginAnimation(Canvas.OpacityProperty, anim);
        }
        public void Hide()
        {
                    if (Links.Controller.MainCanvas.Children.Contains(this))
                        Links.Controller.MainCanvas.Children.Remove(this);
        }
    }
    class HelpTopic
    {
        public static SortedList<int, HelpTopic> Topics;// = Create();
        public int ID;
        public int Step;//уровень вложенности, 0 - верхний
        public string Title;
        public string Text;
        public string Back;
        public string TextPos;
        public string Selection;
        public List<String> Arrows;
        public string StartMenu;
        public string Spec;
        public HelpTopic(string fulltext)
        {
            SortedList<string, string> elements = Common.SearchForHelp(fulltext);
            if (elements.ContainsKey("Step")) Step = Int32.Parse(elements["Step"]);
            if (elements.ContainsKey("Title")) Title = elements["Title"];
            if (elements.ContainsKey("Text")) Text = elements["Text"];
            if (elements.ContainsKey("Back")) Back = elements["Back"];
            if (elements.ContainsKey("TextPos")) TextPos = elements["TextPos"];
            if (elements.ContainsKey("Selection")) Selection = elements["Selection"];
            if (elements.ContainsKey("StartMenu"))
                StartMenu = elements["StartMenu"];
            if (elements.ContainsKey("Spec")) Spec = elements["Spec"];
            Arrows = new List<string>();
            for (int i=1; ;i++)
            {
                string t = string.Format("Arrow{0}", i);
                if (elements.ContainsKey(t)) Arrows.Add(elements[t]);
                else break;
            }
        }
        public static void Create()
        {
            List<string> file = new List<string>(System.IO.File.ReadAllLines("GameData/Help.txt", Encoding.GetEncoding(1251)));
            int curid = 0;
            SortedList<int, HelpTopic> list = new SortedList<int, HelpTopic>();
            foreach (string s in file)
            {
                HelpTopic topic = new HelpTopic(s);
                
                topic.ID = curid; curid++;
                list.Add(topic.ID, topic);

            }
            Topics = list;
        }
    }
    class HelpContent:StackPanel
    {
        public HelpContent()
        {
            Orientation = Orientation.Vertical;
            foreach (HelpTopic topic in HelpTopic.Topics.Values)
            {
                TextBlock block = Common.GetBlock(30, topic.Title, Links.Brushes.SkyBlue, 900);
                block.TextAlignment = TextAlignment.Left;
                block.Margin = new Thickness(topic.Step * 60, 0, 0, 0);
                Children.Add(block);
                block.Tag = topic.ID;
                block.PreviewMouseDown += Block_PreviewMouseDown;
            }
        }

        private void Block_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock block = (TextBlock)sender;
            int topicid = (int)block.Tag;
            Links.Controller.HelpCanvas.Change(topicid);
        }
    }
    class HelpImage:Canvas
    {
        CombinedGeometry FadeGeom;
        TextBlock Text;
        int ID;
        public HelpImage(int id)
        {
            ID = id;
            Width = 1920; Height = 1080;
            HelpTopic topic = HelpTopic.Topics[id];
            if (topic.Back != null) PlaceBackRectangle(topic.Back);
            Path Fade = new Path(); Fade.Fill = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0));
            FadeGeom = new CombinedGeometry();
            FadeGeom.Geometry1 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h1920 v1080 h-1920z"));
            FadeGeom.GeometryCombineMode = GeometryCombineMode.Exclude;
            Children.Add(Fade); Fade.Data = FadeGeom;
            if (topic.TextPos != null) PlaceTextBack(topic.TextPos);
            if (topic.Text != null) PutText(topic.Text);
            if (topic.Selection != null) PutSelection(topic.Selection);
            foreach (string s in topic.Arrows)
                PutArrow(s);
            if (topic.Spec != null) PutSpec(topic.Spec);
        }
        public static ShowWeaponCanvas ShowWeaponCanvas;
        void PutSpec(string text)
        {
            text = text.Trim();
            ShootAnim.IsRealBattle = false;
            ShowWeaponCanvas c = null;
            switch (text)
            {
                case "Laser": c = new ShowWeaponCanvas(EWeaponType.Laser); break;
                case "EMI": c = new ShowWeaponCanvas(EWeaponType.EMI); break;
                case "Plasma": c = new ShowWeaponCanvas(EWeaponType.Plasma); break;
                case "Solar": c = new ShowWeaponCanvas(EWeaponType.Solar); break;
                case "Cannon": c = new ShowWeaponCanvas(EWeaponType.Cannon); break;
                case "Gauss": c = new ShowWeaponCanvas(EWeaponType.Gauss); break;
                case "Missle": c = new ShowWeaponCanvas(EWeaponType.Missle); break;
                case "Anti": c = new ShowWeaponCanvas(EWeaponType.AntiMatter); break;
                case "Psi": c = new ShowWeaponCanvas(EWeaponType.Psi); break;
                case "Dark": c = new ShowWeaponCanvas(EWeaponType.Dark); break;
                case "Warp":c = new ShowWeaponCanvas(EWeaponType.Warp); break;
                case "Time":c = new ShowWeaponCanvas(EWeaponType.Time); break;
                case "Slice": c = new ShowWeaponCanvas(EWeaponType.Slicing); break;
                case "Rad": c = new ShowWeaponCanvas(EWeaponType.Radiation); break;
                case "Drone":c = new ShowWeaponCanvas(EWeaponType.Drone); break;
                case "Magnet": c = new ShowWeaponCanvas(EWeaponType.Magnet); break;
            }
            ShowWeaponCanvas = c;
            Children.Add(c.VBX);
            Canvas.SetLeft(c.VBX, 200); Canvas.SetTop(c.VBX, 180);
        }
        void PutArrow(string text)
        {
            string[] e = text.Split(',');
            Canvas arrow = new Canvas(); arrow.Width = 100; arrow.Height = 200;
            Children.Add(arrow); Canvas.SetLeft(arrow, Int32.Parse(e[0])); Canvas.SetTop(arrow, Int32.Parse(e[1]));
            arrow.RenderTransformOrigin = new Point(0.5, 0);
            arrow.RenderTransform = new RotateTransform(Int32.Parse(e[2]));
            Path path = new Path(); path.Stroke = Links.Brushes.SkyBlue; arrow.Children.Add(path);
            path.StrokeLineJoin = PenLineJoin.Round;
            path.StrokeThickness = 3;
            path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M50,0 l50,80 a55,50 0 0,0 -30,-20 v140 h-40 v-140 a55,50 0 0,0 -30,20z"));
            ColorAnimation anim = new ColorAnimation(Colors.SkyBlue, Colors.White, TimeSpan.FromSeconds(0.3));
            anim.RepeatBehavior = RepeatBehavior.Forever;
            anim.AutoReverse = true;
            SolidColorBrush brush = new SolidColorBrush();
            brush.BeginAnimation(SolidColorBrush.ColorProperty, anim);
            path.Fill = brush;
        }
        void PutSelection(string text)
        {
            FadeGeom.Geometry2 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(text));
            Path path = new Path(); path.Stroke = Links.Brushes.SkyBlue; path.StrokeThickness = 3;
            path.Data = FadeGeom.Geometry2;
            Children.Add(path);
        }
        void PutText(string text)
        {
            Text.Text = text;
            DoubleAnimationUsingKeyFrames anim = new DoubleAnimationUsingKeyFrames();
            anim.Duration = TimeSpan.FromSeconds(0.3);
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromPercent(0)));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.05)));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromPercent(0.25)));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.4)));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromPercent(0.55)));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.9)));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromPercent(1)));
            Text.BeginAnimation(TextBlock.OpacityProperty, anim);
           /* StringAnimationUsingKeyFrames anim = new StringAnimationUsingKeyFrames();
            anim.Duration = TimeSpan.FromSeconds(0.01 * text.Length);
            for (int i = 1; i < text.Length; i++)
                anim.KeyFrames.Add(new DiscreteStringKeyFrame(text.Substring(0, i + 1) , KeyTime.FromPercent(i / (double)text.Length)));
            Text.BeginAnimation(TextBlock.TextProperty, anim);*/
        }
        void PlaceTextBack(string crypt)
        {
            string[] values = crypt.Split(',');
            StackPanel TopPanel = new StackPanel(); TopPanel.Orientation = Orientation.Vertical; Children.Add(TopPanel);
            Canvas.SetLeft(TopPanel, Int32.Parse(values[0])); Canvas.SetTop(TopPanel, Int32.Parse(values[1]));
            Border border = new Border(); border.BorderThickness = new Thickness(3); border.BorderBrush = Links.Brushes.SkyBlue;
            border.Background = Brushes.Black; TopPanel.Children.Add(border);
            
            StackPanel panel = new StackPanel(); 
            border.Child = panel; panel.Margin = new Thickness(10);
            Text = Common.GetBlock(36, "", Brushes.White, Int32.Parse(values[2]));
            panel.Children.Add(Text);
            Text.TextAlignment = TextAlignment.Left;
            WrapPanel buttonpanel = new WrapPanel();
            buttonpanel.Width = Text.Width;
            buttonpanel.Orientation = Orientation.Horizontal;
            TopPanel.Children.Add(buttonpanel);
            GameButton Back = new GameButton((int)(Text.Width/2), 80, "Назад", 36);
            buttonpanel.Children.Add(Back); Back.Margin = new Thickness(0, 5, 0, 5);
            Back.PreviewMouseDown += (sender, e) => { Links.Controller.HelpCanvas.Change(ID-1); };
            GameButton Forward = new GameButton((int)Back.Width, 80, "Вперёд", 36);
            buttonpanel.Children.Add(Forward);Forward.Margin = new Thickness(0, 5, 0, 5);
            Forward.PreviewMouseDown += (sender, e) => { Links.Controller.HelpCanvas.Change(ID+1); };
            GameButton ToContent = new GameButton((int)Back.Width, 80, "К оглавлению", 36);
            buttonpanel.Children.Add(ToContent); ToContent.Margin = new Thickness(0, 5, 0, 5);
            ToContent.PreviewMouseDown += (sender, e) => { Links.Controller.HelpCanvas.Change(-1); };
            GameButton Close = new GameButton((int)Back.Width, 80, "Закрыть", 36);
            buttonpanel.Children.Add(Close);Close.Margin = new Thickness(0, 5, 0, 5);
            Close.PreviewMouseDown += Links.Controller.HelpCanvas.Close_PreviewMouseDown;
        }


        void PlaceBackRectangle(string crypt)
        {
            string[] values = crypt.Split(',');
            Rectangle rect = new Rectangle();
            rect.Width = Int32.Parse(values[1]);
            rect.Height = Int32.Parse(values[2]);
            Canvas.SetLeft(rect, Int32.Parse(values[3]));
            Canvas.SetTop(rect, Int32.Parse(values[4]));
            rect.Fill = Gets.GetHelpPic(values[0]);
            Children.Add(rect);
        }
    }
    class HelpCanvas : Canvas
    {
        bool IsShow = false;
        HelpContent CurContent;
        HelpImage CurImage;
        public HelpCanvas()
        {
            Width = 1920;
            Height = 1080;
            Canvas.SetZIndex(this, 250);
            Background = Brushes.Black;

            //Rectangle Close = Common.GetRectangle(200, Gets.GetIntBoyaImage("NoHex"));
            //Canvas.SetZIndex(Close, 250);
            //Children.Add(Close); Canvas.SetLeft(Close, 1720);
            //Close.PreviewMouseDown += Close_PreviewMouseDown;
        }

        public void Close_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Hide();
        }

        public void Show(int id)
        {
            if (IsShow == true) return;
            if (Links.Controller.mainWindow.MainVbx.Child == Links.Controller.MainCanvas)
            {
                if (!Links.Controller.MainCanvas.Children.Contains(this))
                    Links.Controller.MainCanvas.Children.Add(this);
                IsShow = true;
            }
            else if (Links.Controller.mainWindow.MainVbx.Child == Links.Controller.IntBoya)
            {
                if (!Links.Controller.IntBoya.Children.Contains(this))
                    Links.Controller.IntBoya.Children.Add(this);
                IsShow = true;
            }
            else return;
            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
            this.BeginAnimation(Canvas.OpacityProperty, anim);
            if (Links.Controller.mainWindow.MainVbx.Child == Links.Controller.MainCanvas)
            {
                if (id == -2) Change(-1);
                else if (Links.Controller.CurPanel == GamePanels.Galaxy)
                    Change("Galaxy");
                else
                    Change(id);
            }
            else if (Links.Controller.mainWindow.MainVbx.Child == Links.Controller.IntBoya)
            {
                Change(id);
            }
            else Change(id);
            
        }
        public void Change(string text)
        {
            foreach (HelpTopic topic in HelpTopic.Topics.Values)
            {
                if (topic.StartMenu!=null && topic.StartMenu==text)
                {
                    Change(topic.ID); return;
                }
            }
            Change(-1);
        }
        public void Change(int id)
        {
            if (HelpImage.ShowWeaponCanvas != null) HelpImage.ShowWeaponCanvas.Close();
            if (CurContent != null) { Children.Remove(CurContent); CurContent = null; }
            if (CurImage!= null) { Children.Remove(CurImage); CurImage = null; }
            if (id==-1)
            {
                CurContent = new HelpContent();
                Children.Add(CurContent);
                Canvas.SetLeft(CurContent, 50);
                Canvas.SetTop(CurContent, 50);
            }
            else if (HelpTopic.Topics.ContainsKey(id)==false)
            {
                CurContent = new HelpContent();
                Children.Add(CurContent);
                Canvas.SetLeft(CurContent, 50);
                Canvas.SetTop(CurContent, 50);
            }
            else
            {
                CurImage = new HelpImage(id);
                Children.Add(CurImage);
            }
        }
        public void Hide()
        {
            ShootAnim.IsRealBattle = true;
            if (HelpImage.ShowWeaponCanvas != null) HelpImage.ShowWeaponCanvas.Close();
            if (IsShow == false) return;
            DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.3));
            anim.Completed += (sender, e) =>
            {
                if (Links.Controller.mainWindow.MainVbx.Child == Links.Controller.MainCanvas)
                {
                    if (Links.Controller.MainCanvas.Children.Contains(this))
                        Links.Controller.MainCanvas.Children.Remove(this);
                    IsShow = false;
                }
                else if (Links.Controller.mainWindow.MainVbx.Child == Links.Controller.IntBoya)
                {
                    if (Links.Controller.IntBoya.Children.Contains(this))
                        Links.Controller.IntBoya.Children.Remove(this);
                    IsShow = false;
                }
            };
            this.BeginAnimation(Canvas.OpacityProperty, anim);

        }
    }
}
