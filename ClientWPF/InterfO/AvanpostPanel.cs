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
    class AvanpostPanel:Canvas
    {
        public SortedList<string, ImageBrush> Images;
        Rectangle Max, Max_active, Pos, Pos_active;
        AvanpostIndicator Money, Metall, Chips, Anti;
        Path MaxPath, BuildPath;
        Avanpost Avanpost;
        Rectangle X;
        ScaleTransform X_transform;
        public AvanpostPanel(Avanpost avanpost)
        {
            Avanpost = avanpost;
            FillAll();
            Width = 1671; Height = 812;
            Children.Add(AddRect(1652, 561, "fone", 9, 125, 0));
            Children.Add(Max = AddRect(312, 70, "max", 695, 570, 5));
            Children.Add(Max_active = AddRect(312, 70, "max_active", 695, 570, 5));
            Max_active.Opacity = 0;
            Children.Add(AddRect(1671, 436, "ui_fone1", 0, 376, 10));
            Children.Add(Pos = AddRect(369, 120, "pos", 645, 602, 15));
            Children.Add(Pos_active = AddRect(369, 120, "pos_active", 645, 602, 15));
            Pos_active.Opacity = 0;
            Children.Add(AddRect(1671, 685, "ui_fone2", 0, 0, 20));
            Money = new AvanpostIndicator(this, avanpost.HaveResources.Money, avanpost.NeedResources.Money, 0, 9);
            Metall = new AvanpostIndicator(this, avanpost.HaveResources.Metall, avanpost.NeedResources.Metall, 1, 1051);
            Chips = new AvanpostIndicator(this, avanpost.HaveResources.Chips, avanpost.NeedResources.Chips, 2, 266);
            Anti = new AvanpostIndicator(this, avanpost.HaveResources.Anti, avanpost.NeedResources.Anti, 3, 1310);
            Children.Add(AddRect(1577, 616, "ramk", 48, 75, 30));
            Children.Add(AddRect(1512, 253, "resource", 78, 580, 30));
            MaxPath = new Path(); MaxPath.Fill = Links.Brushes.Transparent;
            MaxPath.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M750,573 h175 l-10,8 h25 l19,5 l-30,25 a200,150 0 0,1 -190,0 l-30,-25 l45,-5z"));
            Children.Add(MaxPath); Canvas.SetZIndex(MaxPath, 150);
            MaxPath.MouseEnter += Maxpath_MouseEnter;
            MaxPath.MouseLeave += Maxpath_MouseLeave;
            MaxPath.PreviewMouseDown += (sender, e) => { Money.SetMaxMoney(); Metall.SetMaxMoney(); Chips.SetMaxMoney(); Anti.SetMaxMoney(); };
            BuildPath = new Path(); BuildPath.Fill = Links.Brushes.Transparent;
            BuildPath.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M700,605 a250,165 0 0,0 260,0 l-5,40 h50 l-19,38 a250,180 0 0,1 -313,0 l-25,-35 h70z"));
            Children.Add(BuildPath); Canvas.SetZIndex(BuildPath, 150);
            BuildPath.MouseEnter += BuildPath_MouseEnter;
            BuildPath.MouseLeave += BuildPath_MouseLeave;
            BuildPath.PreviewMouseDown += BuildPath_PreviewMouseDown;
            Children.Add(X = AddRect(50, 50, "x", 808, 734, 150));
            X.RenderTransformOrigin = new Point(0.5, 0.5);
            X.RenderTransform = X_transform = new ScaleTransform(1, 1);
            X.MouseEnter += X_MouseEnter;
            X.MouseLeave += X_MouseLeave;
            X.PreviewMouseDown += X_PreviewMouseDown;
            Grid NameGrid = new Grid(); Children.Add(NameGrid); Canvas.SetZIndex(NameGrid, 150);
            Canvas.SetLeft(NameGrid, 705); Canvas.SetTop(NameGrid, 123); NameGrid.Width = 260; NameGrid.Height = 45;
            BrightText title = new BrightText(Avanpost.Name.ToString(), Colors.Orange, Colors.Transparent, 20);
            if (Avanpost.Name.ToString().Length > 18) title.Resize(0.6); else title.Resize(0.8);
            NameGrid.Children.Add(title); title.HorizontalAlignment = HorizontalAlignment.Center; title.VerticalAlignment = VerticalAlignment.Center;
            Grid PlanetGrid = new Grid(); Children.Add(PlanetGrid); Canvas.SetZIndex(PlanetGrid, 150);
            Canvas.SetLeft(PlanetGrid, 681); Canvas.SetTop(PlanetGrid, 250); PlanetGrid.Width = 300; PlanetGrid.Width = 300;
            InStellarPlanetShapes shapes = Avanpost.Planet.GetPlanetCanvas2();

            Rectangle rect = Common.GetRectangle(300, 300, new VisualBrush(shapes.canvas));
            PlanetGrid.Children.Add(rect); 
        }

        private void X_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }

        private void X_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation(1.3, 1, TimeSpan.FromSeconds(0.1));
            X_transform.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            X_transform.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
        }

        private void X_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation(1, 1.3, TimeSpan.FromSeconds(0.1));
            X_transform.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            X_transform.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
        }

        private void BuildPath_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Money.lastmoney == 0 && Metall.lastmoney == 0 && Chips.lastmoney == 0 && Anti.lastmoney == 0) return;
            string eventresult = Events.BuildInNewColony(Avanpost, Money.lastmoney, Metall.lastmoney, Chips.lastmoney, Anti.lastmoney);
            if (eventresult == "")
            {
                int avanpostid = Avanpost.ID;
                Gets.GetResources();
                Gets.GetTotalInfo("После эвента по постройке здания");
                if (GSGameInfo.PlayerAvanposts.ContainsKey(avanpostid) == false)//аванпост преобразован в колонию
                {
                    Links.Controller.PopUpCanvas.Change(new SimpleInfoMessage("Колония построена"),"Строительство колонии");
                    Colonies.InnerRefresh();
                }
                else
                {
                    Avanpost = GSGameInfo.PlayerAvanposts[avanpostid];
                    Money.Refresh(Avanpost.HaveResources.Money, Avanpost.NeedResources.Money);
                    Metall.Refresh(Avanpost.HaveResources.Metall, Avanpost.NeedResources.Metall);
                    Chips.Refresh(Avanpost.HaveResources.Chips, Avanpost.NeedResources.Chips);
                    Anti.Refresh(Avanpost.HaveResources.Anti, Avanpost.NeedResources.Anti);
                }
            }
            else Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult));
        }

        private void BuildPath_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation anim1 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.3));
            Pos_active.BeginAnimation(Rectangle.OpacityProperty, anim1);
        }

        private void BuildPath_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation anim0 = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
            Pos_active.BeginAnimation(Rectangle.OpacityProperty, anim0); 
        }

        private void Maxpath_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation anim1 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.3));
            Max_active.BeginAnimation(Rectangle.OpacityProperty, anim1);
        }

        private void Maxpath_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation anim0 = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
            Max_active.BeginAnimation(Rectangle.OpacityProperty, anim0);
        }
        Rectangle AddRect(int width, int height, string fill, int left, int top, int z)
        {
            Rectangle rect = Common.GetRectangle(width, height, Images[fill]);
            Canvas.SetLeft(rect, left); Canvas.SetTop(rect, top); Canvas.SetZIndex(rect, z);
            return rect;
        }
        void FillAll()
        {
            Images = new SortedList<string, ImageBrush>();
            Images.Add("fone", Gets.AddAvanpostImage("fone"));
            Images.Add("max", Gets.AddAvanpostImage("max"));
            Images.Add("max_active", Gets.AddAvanpostImage("max_active"));
            Images.Add("pos", Gets.AddAvanpostImage("pos"));
            Images.Add("pos_active", Gets.AddAvanpostImage("pos_active"));
            Images.Add("ramk", Gets.AddAvanpostImage("ramk"));
            Images.Add("resource", Gets.AddAvanpostImage("resource"));
            Images.Add("sklyanka", Gets.AddAvanpostImage("sklyanka"));
            Images.Add("ui_fone1", Gets.AddAvanpostImage("ui_fone1"));
            Images.Add("ui_fone2", Gets.AddAvanpostImage("ui_fone2"));
            Images.Add("x", Gets.AddAvanpostImage("x"));
        }
    }
    class AvanpostIndicator
    {
        AvanpostPanel Panel;
        Rectangle Image;
        Path Main_Path;
        ScaleTransform Scale;
        TextBlock NeedBlock, SpentBlock;
        Ellipse FullEllipse;
        int ResID;
        int Need;
        int Have;
        public AvanpostIndicator(AvanpostPanel panel, int have, int need, int resid, int left)
        {
            Panel = panel;
            Have = have; Need = need;
            ResID = resid;
            Image = AddRect(354, 708, "sklyanka", left, 40, 25);
            panel.Children.Add(Image);
            Image.RenderTransform = Scale = new ScaleTransform(1, 1);
            Image.RenderTransformOrigin = new Point(0.5, 1);
            Main_Path = new Path();
            Main_Path.Fill = Links.Brushes.Transparent;
            Main_Path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h128 v445 h-128z"));
            panel.Children.Add(Main_Path); Canvas.SetLeft(Main_Path, left + 115); Canvas.SetTop(Main_Path, 165);
            Canvas.SetZIndex(Main_Path, 150);
            NeedBlock = Common.GetBlock(40, "125M", Brushes.White, 90);
            panel.Children.Add(NeedBlock); Canvas.SetLeft(NeedBlock, 135+left); Canvas.SetTop(NeedBlock, 108); Canvas.SetZIndex(NeedBlock, 100);
            Border SpentBlockBorder = new Border();
            SpentBlockBorder.Background = Brushes.Black;
            SpentBlockBorder.CornerRadius = new CornerRadius(10);
            panel.Children.Add(SpentBlockBorder); Canvas.SetLeft(SpentBlockBorder, 135+left); Canvas.SetTop(SpentBlockBorder, 350);
            Canvas.SetZIndex(SpentBlockBorder, 100);
            SpentBlock = Common.GetBlock(40, "125M", Brushes.White, 90);
            SpentBlockBorder.Child = SpentBlock;
            FullEllipse = new Ellipse();
            FullEllipse.Fill = Links.Brushes.Transparent;
            FullEllipse.Width = 100; FullEllipse.Height = 71;
            panel.Children.Add(FullEllipse); Canvas.SetLeft(FullEllipse, 128 + left); Canvas.SetTop(FullEllipse, 658);
            Canvas.SetZIndex(FullEllipse, 150);
            FullEllipse.PreviewMouseDown += FullEllipse_PreviewMouseDown;
            Start();
        }
        int CurMoney()
        {
            switch (ResID)
            {
                case 0: if (GSGameInfo.Money > Int32.MaxValue) return Int32.MaxValue; else return (int)GSGameInfo.Money;
                case 1: return GSGameInfo.Metals;
                case 2: return GSGameInfo.Chips;
                case 3: return GSGameInfo.Anti;
                default: return 0;
            }
        }
        public void Refresh(int have, int need)
        {
            Have = have; Need = need;
            Start();
        }
        private void FullEllipse_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        { 
             if (Need == Have) return;
            if (maxvalue == true)
                SetValueMoney(445);
            else
                SetValueMoney(0);
        }

        public void Start()
        {
            StringAnimationUsingKeyFrames nulltextanim = new StringAnimationUsingKeyFrames();
            nulltextanim.Duration = TimeSpan.Zero;
            nulltextanim.KeyFrames.Add(new DiscreteStringKeyFrame("0", KeyTime.FromPercent(1)));
            DoubleAnimation maxvalueanim = new DoubleAnimation(1, TimeSpan.Zero);
            DoubleAnimation minvalueanim = new DoubleAnimation(0.24, TimeSpan.Zero);
            NeedBlock.Text = GetValueText(Need - Have);
            SpentBlock.BeginAnimation(TextBlock.TextProperty, nulltextanim);
            Main_Path.MouseDown -= Main_Path_PreviewMouseDown;
            if (Have == Need)
                Scale.BeginAnimation(ScaleTransform.ScaleYProperty, maxvalueanim);
            else
            {
                Scale.BeginAnimation(ScaleTransform.ScaleYProperty, minvalueanim);
                Main_Path.MouseDown += Main_Path_PreviewMouseDown;
            }
            lastmoney = 0; maxvalue = false;
        }
        public void SetMaxMoney()
        {
            if (Need == Have) return;
                SetValueMoney(0);
        }

        private void Main_Path_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(Main_Path);
            SetValueMoney(p.Y);
        }
        public int lastmoney = 0;
        bool maxvalue = false;
        void SetValueMoney(double y)
        {
            if (CurMoney() == 0) return;
            maxvalue = false;
            double delta = (445 - y) / 445.0;
            if (delta > 0.99) maxvalue = true; 
            delta = Math.Round(delta, 2);

            long havemoney = CurMoney();
            if (havemoney > Need - Have) havemoney = Need - Have;
            int percent = (int)(Math.Round(delta * havemoney, 0));
            delta = percent / (double)havemoney;
            DoubleAnimation anim = new DoubleAnimation(delta * (1.0 - 0.24) + 0.24, TimeSpan.FromSeconds(0.3));
            Scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
            StringAnimationUsingKeyFrames anim1 = new StringAnimationUsingKeyFrames();
            anim1.Duration = TimeSpan.FromSeconds(0.3);
            for (int i = 0; i < 10; i++)
                if (percent > lastmoney)
                    anim1.KeyFrames.Add(new DiscreteStringKeyFrame(GetValueText((int)(Math.Abs(percent - lastmoney) / (double)9 * i + lastmoney)), KeyTime.FromPercent(i / 9.0)));
                else
                    anim1.KeyFrames.Add(new DiscreteStringKeyFrame(GetValueText((int)(lastmoney - Math.Abs(lastmoney - percent) / (double)9 * i)), KeyTime.FromPercent(i / 9.0)));
            SpentBlock.BeginAnimation(TextBlock.TextProperty, anim1);
            lastmoney = percent;
        }
        string GetValueText(int val)
        {
            if (val <= 999) return val.ToString();
            else if (val <= 99999) return (val / 1000.0).ToString("0.0") + "Т";
            else if (val <= 999999) return (val / 1000.0).ToString("0") + "T";
            else if (val <= 99999999) return (val / 1000000.0).ToString("0.0") + "М";
            else if (val <= 999999999) return (val / 1000000.0).ToString("0") + "М";
            else return (val / 1000000000.0).ToString("0") + "млрд";
        }
        Rectangle AddRect(int width, int height, string fill, int left, int top, int z)
        {
            Rectangle rect = Common.GetRectangle(width, height, Panel.Images[fill]);
            Canvas.SetLeft(rect, left); Canvas.SetTop(rect, top); Canvas.SetZIndex(rect, z);
            return rect;
        }
    }
}
