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
    enum GalaxyMode { Galaxy, Sector, Stellar };
    class Galaxy:Viewbox
    {
        static TimeSpan toptime = TimeSpan.FromSeconds(1);
        static TimeSpan innertime = TimeSpan.FromSeconds(1);
        public Canvas TopCanvas;
        public Canvas HighCanvas;
        public Viewbox HighBox;
        public Canvas InnerCanvas;
        public Rectangle MiddleImage;
        GalaxyMode CurMode = GalaxyMode.Galaxy;
        double InnerX, InnerY;
        SortedSet<GSStar> SectorStars = new SortedSet<GSStar>();
        public GalaxyMap Map;
        double MiddleSectorW, MiddleSectorH, MiddleStellarW, MiddleStellarH;
        public Galaxy()
        {
            Stretch = Stretch.Fill;
            HighCanvas = new Canvas(); HighCanvas.Width = 1920; HighCanvas.Height = 1080;
            Child = HighCanvas;
            HighCanvas.Background = Links.Brushes.SpaceBack;
            HighBox = new Viewbox(); HighBox.Width = Links.GalaxyWidth/300.0*800; HighBox.Height = Links.GalaxyHeight/300*800;
            HighCanvas.Children.Add(HighBox); Canvas.SetLeft(HighBox, (1920-HighBox.Width)/2); Canvas.SetTop(HighBox, 1080-HighBox.Height-100);
            TopCanvas = new Canvas(); TopCanvas.Width = Links.GalaxyWidth; TopCanvas.Height = Links.GalaxyHeight;
            MiddleImage = new Rectangle(); MiddleImage.Fill = Links.Brushes.SpaceMiddle;
            MiddleImage.Width = Links.GalaxyWidth; MiddleImage.Height = Links.GalaxyHeight;
            MiddleSectorW = Links.GalaxyWidth / 3; MiddleSectorH = Links.GalaxyHeight / 3;
            MiddleStellarW = Links.GalaxyWidth / 8; MiddleStellarH = Links.GalaxyHeight / 8;
            Canvas.SetZIndex(MiddleImage, 25);
            Canvas.SetLeft(MiddleImage, 0); Canvas.SetTop(MiddleImage, 0);
            InnerCanvas = new Canvas(); InnerCanvas.Width = Links.GalaxyWidth; InnerCanvas.Height = Links.GalaxyHeight;
            Canvas.SetZIndex(InnerCanvas, 50);
            Canvas.SetLeft(InnerCanvas, 0); Canvas.SetTop(InnerCanvas, 0);
            TopCanvas.Background = Links.Brushes.Transparent;
            HighBox.Child = TopCanvas; 
            TopCanvas.Children.Add(InnerCanvas); TopCanvas.Children.Add(MiddleImage);
            Map = new GalaxyMap();
            HighCanvas.Children.Add(Map); Canvas.SetLeft(Map, 1650); Canvas.SetTop(Map, 180);
            Map.PreviewMouseDown += Map_PreviewMouseDown;
            foreach (GSStar star in Links.Stars.Values)
            {
                InnerCanvas.Children.Add(star.el);
                star.el.PreviewMouseDown += El_PreviewMouseDown;
                //star.CreateStellarSystem(false);
                star.el.MouseEnter += El_MouseEnter;
                star.el.MouseLeave += El_MouseLeave;
            }
            Rectangle border = new Rectangle(); border.Stroke = Brushes.White;
            border.Opacity = 0.5; border.Width = Links.GalaxyWidth; border.Height = Links.GalaxyHeight;
            InnerCanvas.Children.Add(border); border.RadiusX = 5; border.RadiusY = 5;
            border.StrokeThickness = 0.2; border.StrokeDashArray = new DoubleCollection(new double[] { 5 });
            //TopCanvas.MouseDown += Canvas_PreviewMouseDown;
            CreateNames(Links.GalaxyWidth, Links.GalaxyHeight);
            TopCanvas.PreviewMouseWheel += TopCanvas_PreviewMouseWheel;
            HighCanvas.MouseDown += HighCanvas_MouseDown;
            HighCanvas.MouseUp += HighCanvas_MouseUp;
            HighCanvas.MouseMove += HighCanvas_MouseMove;
            HighCanvas.MouseLeave += HighCanvas_MouseLeave;
        }
        public class GalaxyMap:Border
        {
            Viewbox Box;
            ScaleTransform Scale;
            Canvas CurCanvas;
            Rectangle Rect;
            Point Pt;
            GalaxyMode Mode;
            public GalaxyMap()
            {
                Width = 250; Height = 180;
                BorderBrush = Links.Brushes.SkyBlue; BorderThickness = new Thickness(3);
                CornerRadius = new CornerRadius(20);
                Box = new Viewbox(); Box.Width = 250; Box.Height = 180;
                Scale = new ScaleTransform(1, 1);
                Box.RenderTransformOrigin = new Point(0.5, 0.5);
                Box.RenderTransform = Scale;
                Child = Box;
                CurCanvas = new Canvas();
                CurCanvas.Width = Links.GalaxyWidth;
                CurCanvas.Height = Links.GalaxyHeight;
                CurCanvas.Background = Links.Brushes.SpaceMiddle;
                Box.Child = CurCanvas;
                Rect = new Rectangle(); Rect.Stroke = Brushes.White; Rect.StrokeThickness = 5;
                CurCanvas.Children.Add(Rect);
                Rect.Width = 100; Rect.Height = 60;
                Pt = new Point(0, 0);
                ChangeZoom(GalaxyMode.Galaxy);
            }
            public void ChangePT(Point pt)
            {
                Pt = pt;
                switch (Mode)
                {
                    case GalaxyMode.Galaxy:
                        Canvas.SetLeft(Rect, 0); Canvas.SetTop(Rect, 0); break;
                    case GalaxyMode.Sector:
                        Canvas.SetLeft(Rect, Pt.X - 50); Canvas.SetTop(Rect, Pt.Y - 30); break;
                    case GalaxyMode.Stellar:
                        Canvas.SetLeft(Rect, Pt.X - 10); Canvas.SetTop(Rect, Pt.Y - 6); break;
                }
            }
            public void ChangeZoom(GalaxyMode mode)
            {
                Mode = mode;
                switch (mode)
                {
                    case GalaxyMode.Galaxy: Rect.Width = Links.GalaxyWidth; Rect.Height = Links.GalaxyHeight; 
                        Canvas.SetLeft(Rect, 0); Canvas.SetTop(Rect, 0);break;
                    case GalaxyMode.Sector: Rect.Width = 100; Rect.Height = 60;
                        Canvas.SetLeft(Rect, Pt.X - 50); Canvas.SetTop(Rect, Pt.Y - 30); break;
                    case GalaxyMode.Stellar: Rect.Width = 20; Rect.Height = 12;
                        Canvas.SetLeft(Rect, Pt.X - 10); Canvas.SetTop(Rect, Pt.Y - 6); break;
                }

            }
            UIElement CurElement;
            public void PutElement(UIElement element)
            {
                CurElement = element;
                DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.1));
                anim.Completed += Anim_Completed;
                Scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
            }
            public void RemoveElement()
            {
                DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.1));
                anim.Completed += Anim_Completed1;
                Scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
            }
            private void Anim_Completed(object sender, EventArgs e)
            {
                DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.1));
                Box.Child = CurElement;
                Scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
            }
            private void Anim_Completed1(object sender, EventArgs e)
            {
                DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.1));
                Box.Child = CurCanvas;
                Scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
            }
        }
        private void TopCanvas_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta>0)
            {
                if (CurMode==GalaxyMode.Galaxy)
                {
                    MouseButtonEventArgs g = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left);
                    g.RoutedEvent = e.RoutedEvent;
                    Canvas_PreviewMouseDown(TopCanvas, g);
                }
                else if (CurMode==GalaxyMode.Sector)
                {
                    Point pt = e.GetPosition(InnerCanvas);
                    ShowOneStar(null, pt.X, pt.Y);
                }
            }
            else if (e.Delta<0)
            {
                if (CurMode!=GalaxyMode.Galaxy)
                {
                    MouseButtonEventArgs g = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left);
                    g.RoutedEvent = e.RoutedEvent;
                    Map_PreviewMouseDown(Map, g);
                }
            }
        }

     
        private void HighCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            if (KeyPressed== true)
            {
                KeyPressed = false;
                if (CurMode == GalaxyMode.Sector)
                {
                    double starx = pt.X - Links.GalaxyWidth/2.0;
                    double stary = pt.Y - Links.GalaxyHeight / 2.0;
                    foreach (GSStar star in Links.Stars.Values)
                    {
                        if (star.X < starx - 20) { star.SetVisible(false); SectorStars.Remove(star); continue; }
                        if (star.X > starx + 100 + 20) { star.SetVisible(false); SectorStars.Remove(star); continue; }
                        if (star.Y < stary - 20) { star.SetVisible(false); SectorStars.Remove(star); continue; }
                        if (star.Y > stary + 60 + 20) { star.SetVisible(false); SectorStars.Remove(star); continue; }
                        SectorStars.Add(star); star.SetVisible(true); star.ShowName(true);
                    }
                    DrawSectorPlanetsSideInfo();
                }
                else if (CurMode == GalaxyMode.Stellar)
                    ShowStellarStars();
            }
        }

        private void HighCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (KeyPressed == true)
            {
                KeyPressed = false;
                if (CurMode == GalaxyMode.Sector)
                {
                    double starx = pt.X - Links.GalaxyWidth / 2.0;
                    double stary = pt.Y - Links.GalaxyHeight / 2.0;
                    foreach (GSStar star in Links.Stars.Values)
                    {
                        if (star.X < starx - 60) { star.SetVisible(false); SectorStars.Remove(star); continue; }
                        if (star.X > starx + 60) { star.SetVisible(false); SectorStars.Remove(star); continue; }
                        if (star.Y < stary - 40) { star.SetVisible(false); SectorStars.Remove(star); continue; }
                        if (star.Y > stary + 40) { star.SetVisible(false); SectorStars.Remove(star); continue; }
                        SectorStars.Add(star); star.SetVisible(true); star.ShowName(true);
                    }
                    DrawSectorPlanetsSideInfo();
                }
                else if (CurMode == GalaxyMode.Stellar)
                    ShowStellarStars();
            }
        }

        private void HighCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (KeyPressed == false) return;
            double sizex, sizey;
            double maxX, maxY;
            if (CurMode == GalaxyMode.Sector)
            {
                sizex = 100; sizey = 60;
            }
            else
            {
                sizex = 20; sizey = 12;
            }
            maxX = sizex - Links.GalaxyWidth; maxY = sizey - Links.GalaxyHeight;
            Point targetpt = e.GetPosition(InnerCanvas);
            Point delta = new Point((targetpt.X - movept.X)*1.5, (targetpt.Y - movept.Y)*1.5);
            movept = new Point(targetpt.X - delta.X , targetpt.Y - delta.Y );
            DoubleAnimation animx = new DoubleAnimation();
            animx.Duration = TimeSpan.Zero;
            DoubleAnimation animy = new DoubleAnimation();
            animy.Duration = TimeSpan.Zero;
            
            double newX = Canvas.GetLeft(InnerCanvas) + delta.X;
            double newY = Canvas.GetTop(InnerCanvas) + delta.Y;
            Links.Controller.mainWindow.Title = String.Format("{0:0.0} {1:0.0}", newX, newY);

            if (newX< maxX) 
                newX = maxX; 
            else if (newX>0)
                newX = 0; 
            animx.To = newX;
            if (newY< maxY) 
                newY = maxY;
            else if (newY>0)
                newY = 0; 
            animy.To = newY;
            double middlew = MiddleSectorW; double middleh = MiddleSectorH; double topw = 100; double toph = 60;
            if (CurMode == GalaxyMode.Stellar) { middlew = MiddleStellarW; middleh = MiddleStellarH; topw = 20; toph = 12; }
            double MiddleX = newX * (middlew - topw) / (Links.GalaxyWidth - topw);
            double MiddleY = newY * (middleh - toph) / (Links.GalaxyHeight - toph);
            DoubleAnimation MoveXM = new DoubleAnimation(MiddleX, TimeSpan.Zero);
            DoubleAnimation MoveYM = new DoubleAnimation(MiddleY, TimeSpan.Zero);
            InnerCanvas.BeginAnimation(Canvas.LeftProperty, animx);
            InnerCanvas.BeginAnimation(Canvas.TopProperty, animy);
            MiddleImage.BeginAnimation(Canvas.LeftProperty, MoveXM);
            MiddleImage.BeginAnimation(Canvas.TopProperty, MoveYM);
            pt = new Point(sizex / 2 - newX, sizey / 2 - newY);
            Map.ChangePT(pt);
        }

        bool KeyPressed = false;
        Point movept;

        private void HighCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyPressed = true;
            movept = e.GetPosition(InnerCanvas);
        }

        private void Map_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurMode == GalaxyMode.Galaxy) return;
            e.Handled = true;
            CurMode = GalaxyMode.Galaxy;
            Map.ChangePT(new Point(0, 0));
            Map.ChangeZoom(GalaxyMode.Galaxy);
            foreach (GSStar star in Links.Stars.Values)
            {
                star.ShowName(false); star.Animate(false); star.ShowPlanetsAndStorys(false);
            }

            TopCanvas.Children.Clear();
            TopCanvas.Children.Add(InnerCanvas);
            TopCanvas.Children.Add(MiddleImage);
            DoubleAnimation TopW = new DoubleAnimation(Links.GalaxyWidth, toptime);
            DoubleAnimation TopH = new DoubleAnimation(Links.GalaxyHeight, toptime);
            DoubleAnimation MoveX = new DoubleAnimation(0, innertime);
            DoubleAnimation MoveY = new DoubleAnimation(0, innertime);
            MoveY.Completed += Galaxy_Show_Completed;
            InnerX = 0; InnerY = 0;
            TopCanvas.BeginAnimation(Canvas.WidthProperty, TopW);
            TopCanvas.BeginAnimation(Canvas.HeightProperty, TopH);
            MiddleImage.BeginAnimation(Rectangle.WidthProperty, TopW);
            MiddleImage.BeginAnimation(Rectangle.HeightProperty, TopH);
            InnerCanvas.BeginAnimation(Canvas.LeftProperty, MoveX);
            InnerCanvas.BeginAnimation(Canvas.TopProperty, MoveY);
            MiddleImage.BeginAnimation(Canvas.LeftProperty, MoveX);
            MiddleImage.BeginAnimation(Canvas.TopProperty, MoveY);
            pt = new Point(Links.GalaxyWidth/2.0, Links.GalaxyHeight/2.0);
            KeyPressed = false;
        }
    
     
        private void El_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Aim != null)
            { InnerCanvas.Children.Remove(Aim); Aim = null; }
            Map.RemoveElement();
        }

        private void El_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Aim != null)
            { InnerCanvas.Children.Remove(Aim); Aim = null; }
            Ellipse el = (Ellipse)sender;
            GSStar star = (GSStar)el.Tag;
            Map.PutElement(star.PanelInfo);
            if (CurMode != GalaxyMode.Galaxy) return;
            Aim = new GalaxyAim();
            InnerCanvas.Children.Add(Aim);
            Canvas.SetLeft(Aim, star.X + Links.GalaxyWidth/2.0 - Aim.Width / 2);
            Canvas.SetTop(Aim, star.Y + Links.GalaxyHeight/2.0 - Aim.Height / 2);
            
        }

        public void Select()
        {
            Gets.GetTotalInfo("После открытия карты галакатики");
            foreach (GSStar star in Links.Stars.Values)
            {
                //star.CreateStellarSystem();
                star.UpdateTips();
            }
            DrawAllPlanetSideInfo();

            if (CurMode == GalaxyMode.Stellar)
            {
                ShowStellarStars();
            }
            if (Radar != null)
            { InnerCanvas.Children.Remove(Radar); Radar = null; }
            if (Links.Controller.SelectModifier==SelectModifiers.StarForScout)
            {
                Radar = new GalaxyRadar();
            }
        }
        Canvas TextStoryIcon;
        public void AddTextStoryIcon()
        {
            if (TextStoryIcon != null) HighCanvas.Children.Remove(TextStoryIcon);
            TextStoryIcon = new Canvas();
            TextStoryIcon.Width = 200;
            TextStoryIcon.Height = 200;
            HighCanvas.Children.Add(TextStoryIcon); 
            Canvas.SetLeft(TextStoryIcon, 0);
            Canvas.SetTop(TextStoryIcon, 800);
            Canvas.SetZIndex(TextStoryIcon, 200);
            Ellipse el = new Ellipse(); el.Width = 200; el.Height = 200;
            TextStoryIcon.Children.Add(el);
            el.Fill = StoryLine2.GetStoryFlashBrush();
            
            Rectangle Icon = Common.GetRectangle(60, Links.Brushes.CrownBrush);
            TextStoryIcon.Children.Add(Icon); Canvas.SetLeft(Icon, 70); Canvas.SetTop(Icon, 70);
            TextStoryIcon.PreviewMouseDown += TextStoryIcon_PreviewMouseDown;
        }
        public void RemoveTextStoryIcon()
        {
            if (TextStoryIcon != null) HighCanvas.Children.Remove(TextStoryIcon);
            TextStoryIcon = null;
        }
        private void TextStoryIcon_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            StoryLine2 story = StoryLine2.StoryLines[GSGameInfo.StoryLinePosition];
            story.PutPanel();
        }

        GalaxyRadar Radar;
        GalaxyAim Aim;
        List<UIElement> PlanetSideList = new List<UIElement>();
        
        void HidePlanetSideInfo()
        {
            foreach (UIElement path in PlanetSideList)
                InnerCanvas.Children.Remove(path);
            PlanetSideList.Clear();
            if (StoryEllipse!=null)
            {
                InnerCanvas.Children.Remove(StoryEllipse); StoryEllipse = null;
            }
        }
        void DrawOnePlanetSideInfo(GSStar star)
        {
            SortedSet<PlanetSide> list = new SortedSet<PlanetSide>();
            bool Pillage = false; bool Conquer = false;
            foreach (GSPlanet planet in star.Planets.Values)
            {
                switch (planet.PlanetSide)
                {
                    case PlanetSide.Alien: if (list.Contains(PlanetSide.Alien) == false) list.Add(PlanetSide.Alien); break;
                    case PlanetSide.Pirate: if (list.Contains(PlanetSide.Pirate) == false) list.Add(PlanetSide.Pirate); break;
                    case PlanetSide.GreenTeam: if (list.Contains(PlanetSide.GreenTeam) == false) list.Add(PlanetSide.GreenTeam); break;
                    case PlanetSide.Mercs: if (list.Contains(PlanetSide.Mercs) == false) list.Add(PlanetSide.Mercs); break;
                    case PlanetSide.Player: if (list.Contains(PlanetSide.Player) == false) list.Add(PlanetSide.Player); if (planet.Pillage) Pillage = true; if (planet.Conquer) Conquer = true; break;
                    case PlanetSide.Techno: if (list.Contains(PlanetSide.Techno) == false) list.Add(PlanetSide.Techno); break;
                }
            }
            bool Scouted = false; bool IsAttacked = false;
            foreach (Land land in GSGameInfo.PlayerLands.Values)
                foreach (GSFleet fleet in land.Fleets.Values)

                    if (fleet.Target != null && fleet.Target.Mission == FleetMission.Scout && fleet.Target.TargetID == star.ID)
                    {
                        if (fleet.Target.Order == FleetOrder.Defense)
                            Scouted = true;
                        else if (fleet.Target.Order == FleetOrder.InBattle)
                            IsAttacked = true;
                    }
                
            if (list.Count == 0 && Scouted==false) return;
            Canvas canvas = new StarSideBack(list, Pillage, Conquer, Scouted, IsAttacked);
            
            PlanetSideList.Add(canvas);
            InnerCanvas.Children.Add(canvas);
            Canvas.SetLeft(canvas, star.X + Links.GalaxyWidth/2.0 - 8);
            Canvas.SetTop(canvas, star.Y + Links.GalaxyHeight/2.0 - 8);
        }

        public class StarSideBack : Canvas
        {
            public static ImageBrush PirateBrush = GetBrush("HexRed2");
            public static ImageBrush AlienBrush = GetBrush("HexPurple2");
            public static ImageBrush GreenTeamBrush = GetBrush("HexGreen2");
            public static ImageBrush MercsBrush = GetBrush("HexBlack2");
            public static ImageBrush PlayerBrush = GetBrush("HexWhite2");
            public static ImageBrush TechnoBrush = GetBrush("Hex");
            public static ImageBrush ScoutBrush = GetBrush("HexYellow2");
            int curElement = 0;
            int Elements = 0;
            List<Rectangle> Rects;
            List<Rectangle> PillageConquer;
            int pillagesteps;
            int curpillagestep = 0;
            public StarSideBack(SortedSet<PlanetSide> list, bool Pillage, bool Conquer, bool isScouted, bool IsAttacked)
            {
                Width = 16; Height = 16;
                if (list.Count == 1)
                {
                    switch (list.ElementAt(0))
                    {
                        case PlanetSide.Alien: Background = AlienBrush; break;
                        case PlanetSide.Pirate: Background = PirateBrush; break;
                        case PlanetSide.GreenTeam: Background = GreenTeamBrush; break;
                        case PlanetSide.Mercs: Background = MercsBrush; break;
                        case PlanetSide.Player: Background = PlayerBrush; break;
                        case PlanetSide.Techno: Background = TechnoBrush; break;
                    }
                }
                else if (list.Count > 1)
                {
                    Elements = list.Count;
                    Rects = new List<Rectangle>();
                    foreach (PlanetSide side in list)
                    {
                        Rectangle rect = new Rectangle(); rect.Width = 16; rect.Height = 16;
                        switch (side)
                        {
                            case PlanetSide.Alien: rect.Fill = AlienBrush; break;
                            case PlanetSide.Pirate: rect.Fill = PirateBrush; break;
                            case PlanetSide.GreenTeam: rect.Fill = GreenTeamBrush; break;
                            case PlanetSide.Mercs: rect.Fill = MercsBrush; break;
                            case PlanetSide.Player: rect.Fill = PlayerBrush; break;
                            case PlanetSide.Techno: rect.Fill = TechnoBrush; break;
                        }
                        Rects.Add(rect); rect.Opacity = 0; Children.Add(rect);
                    }
                    Rects[0].Opacity = 1;
                    StartAnim();
                }
                if (Pillage || Conquer)
                {
                    PillageConquer = new List<Rectangle>();
                    pillagesteps = 1;
                    if (Pillage)
                    {
                        Rectangle rect = Common.GetRectangle(10, Links.Brushes.FleetPillageBrush);
                        Canvas.SetLeft(rect, 3); Canvas.SetTop(rect, 3);
                        PillageConquer.Add(rect); Children.Add(rect); pillagesteps++; rect.Opacity = 0; 
                    }
                    if (Conquer)
                    {
                        Rectangle rect = Common.GetRectangle(10, Links.Brushes.FleetConquerBrush);
                        Canvas.SetLeft(rect, 3); Canvas.SetTop(rect, 3);
                        PillageConquer.Add(rect); Children.Add(rect); rect.Opacity = 0;
                        pillagesteps++;
                    }
                    StartPillageAnim();
                }
                if (isScouted || IsAttacked)
                {
                    Rectangle rect = GetScoutedPath(IsAttacked);
                    Children.Add(rect);
                }
            }
            static TimeSpan PillageChangeTime = TimeSpan.FromSeconds(1);
            void StartPillageAnim()
            {
                curpillagestep++; curpillagestep = curpillagestep % pillagesteps;
                switch (curpillagestep)
                {
                    case 1: DoubleAnimation anim1 = new DoubleAnimation(0, 1, PillageChangeTime);
                        anim1.Completed += PillageConquerAnimCompleted;
                        PillageConquer[0].BeginAnimation(Rectangle.OpacityProperty, anim1); break;
                    case 2: 
                        if (pillagesteps==3)
                        {
                            DoubleAnimation anim2 = new DoubleAnimation(1, 0, PillageChangeTime);
                            PillageConquer[0].BeginAnimation(Rectangle.OpacityProperty, anim2);
                            DoubleAnimation anim3 = new DoubleAnimation(0, 1, PillageChangeTime);
                            anim3.Completed += PillageConquerAnimCompleted;
                            PillageConquer[1].BeginAnimation(Rectangle.OpacityProperty, anim3);
                        }
                        else
                        {
                            DoubleAnimation anim4 = new DoubleAnimation(1, 0, PillageChangeTime);
                            anim4.Completed += PillageConquerAnimCompleted;
                            PillageConquer[0].BeginAnimation(Rectangle.OpacityProperty, anim4);
                        } break;
                    case 0:
                        if (pillagesteps == 3)
                        {
                            DoubleAnimation anim5 = new DoubleAnimation(1, 0, PillageChangeTime);
                            anim5.Completed += PillageConquerAnimCompleted;
                            PillageConquer[1].BeginAnimation(Rectangle.OpacityProperty, anim5);
                        }
                        else
                        {
                            DoubleAnimation anim6 = new DoubleAnimation(1, 0, PillageChangeTime);
                            anim6.Completed += PillageConquerAnimCompleted;
                            PillageConquer[0].BeginAnimation(Rectangle.OpacityProperty, anim6);
                        }
                        break;
                }
            }

            private void PillageConquerAnimCompleted(object sender, EventArgs e)
            {
                StartPillageAnim();
            }

            static SolidColorBrush ScoutIsAttackedBrush = GetScoutIsAttackedBrush();
            static SolidColorBrush GetScoutIsAttackedBrush()
            {
                SolidColorBrush brush = new SolidColorBrush();
                ColorAnimation anim = new ColorAnimation(Colors.Transparent, Colors.Red, TimeSpan.FromSeconds(0.5));
                anim.RepeatBehavior = RepeatBehavior.Forever;
                anim.AutoReverse = true;
                brush.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                return brush;
            }
            Rectangle GetScoutedPath(bool underattack)
            {
                Rectangle rect = new Rectangle();
                rect.Width = 28; rect.Height = 28;
                PathGeometry geom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M50,10 a30,30 0 0,0 40,20 a50,160 0 0,1 -80,0 a30,30 0 0,0 40,-20"));
                rect.Fill = new DrawingBrush(new GeometryDrawing(underattack ? ScoutIsAttackedBrush : null, new Pen(Brushes.White, 3), geom));
                Canvas.SetLeft(rect, -6); Canvas.SetTop(rect, -6);
                return rect;
            }
            void StartAnim()
            {
                Rectangle Cur = Rects[curElement];
                if (Rects.Count - 1 == curElement)
                    curElement = 0;
                else
                    curElement++;
                Rectangle Next = Rects[curElement];
                DoubleAnimation anim1 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));
                Cur.BeginAnimation(Rectangle.OpacityProperty, anim1);
                DoubleAnimation anim2 = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));
                anim2.Completed += Anim2_Completed;
                Next.BeginAnimation(Rectangle.OpacityProperty, anim2);
            }

            private void Anim2_Completed(object sender, EventArgs e)
            {
                StartAnim();
            }

            static ImageBrush GetBrush(string text)
            {
                if (Links.LoadImageFromDLL)
                    return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Int/{0}.png", text))));
                else
                    return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Int/{0}.png", text), UriKind.Relative)));
            }
        }
        ImageBrush GetBrush(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Int/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Int/{0}.png", text), UriKind.Relative)));
        }
        public static StoryLine2 BattleStory;        
        Ellipse StoryEllipse;
        public void DrawOneStoryInfo()
        {
            if (StoryEllipse != null) { InnerCanvas.Children.Remove(StoryEllipse); StoryEllipse = null; }
            if (BattleStory == null) return;
            if (Links.Controller.Galaxy.CurMode == GalaxyMode.Stellar) return;
            StoryEllipse = new Ellipse();
            StoryEllipse.Width = 26; StoryEllipse.Height = 26;
            InnerCanvas.Children.Add(StoryEllipse);
            GSStar StoryStar;
            if (BattleStory.StoryType == StoryType.SpaceBattle)
                StoryStar = Links.Stars[BattleStory.LocationID];
            else
                StoryStar = Links.Planets[BattleStory.LocationID].Star;
            Canvas.SetLeft(StoryEllipse, StoryStar.X + Links.GalaxyWidth/2.0 - 13);
            Canvas.SetTop(StoryEllipse, StoryStar.Y + Links.GalaxyHeight/2.0 - 13);
            RadialGradientBrush brush = StoryLine2.GetStoryFlashBrush();
            StoryEllipse.Fill = brush;
        }
        void DrawAllPlanetSideInfo()
        {
            HidePlanetSideInfo();
            foreach (GSStar star in Links.Stars.Values)
            {
                DrawOnePlanetSideInfo(star);
            }
                DrawOneStoryInfo();
           
        }
        void DrawSectorPlanetsSideInfo()
        {
            HidePlanetSideInfo();
            foreach (GSStar star in SectorStars)
                DrawOnePlanetSideInfo(star);
            //if (GSGameInfo.StoryLinePosition != 255 && GSGameInfo.StoryStatus == 0)
            //{
           //     StoryLine story = StoryLine.Storys[GSGameInfo.StoryLinePosition];
           //     GSStar storystar = Links.Stars[story.StarID];
           //     if (SectorStars.Contains(storystar))
                DrawOneStoryInfo();
           // }
        }
        Point pt;

        private void Canvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurMode == GalaxyMode.Galaxy)
            {
                    e.Handled = true;
                El_MouseLeave(null, null);
                pt = e.GetPosition(InnerCanvas);
                InnerX = -pt.X + 50;
                if (InnerX > 0) InnerX = 0; else if (InnerX < -(Links.GalaxyWidth-100)) InnerX = -(Links.GalaxyWidth-100);
                InnerY = -pt.Y + 30;
                double starx = -Links.GalaxyWidth/2.0 - InnerX;
                double stary = -Links.GalaxyHeight/2.0 - InnerY;
                SectorStars.Clear();
                foreach (GSStar star in Links.Stars.Values)
                {
                    if (star.X < starx - 20) { continue; }
                    if (star.X > starx + 100 + 20) { continue; }
                    if (star.Y < stary - 20) { continue; }
                    if (star.Y > stary + 60 + 20) { continue; }
                    SectorStars.Add(star);
                }
                if (SectorStars.Count == 0) return;

                CurMode = GalaxyMode.Sector;
                //zoomer.SetPosition(GalaxyMode.Sector);
                DoubleAnimation TopW = new DoubleAnimation(100, toptime);
                DoubleAnimation TopH = new DoubleAnimation(60, toptime);
                DoubleAnimation TopWM = new DoubleAnimation(MiddleSectorW, toptime);
                DoubleAnimation TopHM = new DoubleAnimation(MiddleSectorH, toptime);
                InnerX = -pt.X + 50;
                if (InnerX > 0) InnerX = 0; else if (InnerX < -(Links.GalaxyWidth-100)) InnerX = -(Links.GalaxyWidth-100);
                InnerY = -pt.Y + 30;
                if (InnerY > 0) InnerY = 0; else if (InnerY < -(Links.GalaxyHeight-60)) InnerY = -(Links.GalaxyHeight-60);
                pt = new Point(50 - InnerX, 30 - InnerY);
                Map.ChangePT(pt);
                Map.ChangeZoom(GalaxyMode.Sector);
                DoubleAnimation MoveX = new DoubleAnimation(InnerX, innertime);
                DoubleAnimation MoveY = new DoubleAnimation(InnerY, innertime);
                double MiddleX = InnerX * (MiddleSectorW - 100) / (Links.GalaxyWidth - 100);
                double MiddleY = InnerY * (MiddleSectorH - 60) / (Links.GalaxyHeight - 60);
                DoubleAnimation MoveXM = new DoubleAnimation(MiddleX, innertime);
                DoubleAnimation MoveYM = new DoubleAnimation(MiddleY, innertime);
                MoveY.Completed += SectorSelect_Complite;
                TopCanvas.BeginAnimation(Canvas.WidthProperty, TopW);
                TopCanvas.BeginAnimation(Canvas.HeightProperty, TopH);
                MiddleImage.BeginAnimation(Rectangle.WidthProperty, TopWM);
                MiddleImage.BeginAnimation(Rectangle.HeightProperty, TopHM);
                InnerCanvas.BeginAnimation(Canvas.LeftProperty, MoveX);
                InnerCanvas.BeginAnimation(Canvas.TopProperty, MoveY);
                MiddleImage.BeginAnimation(Canvas.LeftProperty, MoveXM);
                MiddleImage.BeginAnimation(Canvas.TopProperty, MoveYM);

                SectorStars.Clear();
                foreach (GSStar star in Links.Stars.Values)
                {
                    if (star.X < starx - 20) { star.SetVisible(false); continue; }
                    if (star.X > starx + 100 + 20) { star.SetVisible(false); continue; }
                    if (star.Y < stary - 20) { star.SetVisible(false); continue; }
                    if (star.Y > stary + 60 + 20) { star.SetVisible(false); continue; }
                    SectorStars.Add(star);
                }
                //StarMark.HideMarks(SectorStars);
                return;
            }
        }
        private void Galaxy_Show_Completed(object sender, EventArgs e)
        {
            foreach (GSStar star in Links.Stars.Values)
            {
                star.SetVisible(true);
            }
            DrawAllPlanetSideInfo();
        }
        private void SectorSelect_Complite(object sender, EventArgs e)
        {
           // TopCanvas.Children.Add(p1); TopCanvas.Children.Add(p2); TopCanvas.Children.Add(p3); TopCanvas.Children.Add(p4);
            DrawNames();
            DrawSectorPlanetsSideInfo();
        }
        private void El_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Ellipse el = (Ellipse)sender;
            GSStar curstar = (GSStar)el.Tag;
            El_MouseLeave(null, null);
            if (Links.Controller.SelectModifier == SelectModifiers.StarForScout)
            {
                Links.Helper.Star = curstar;
                Links.Helper.ClickDelegate(null, null);
                return;
            }
            WrapPanel panel = new WrapPanel();
            int count = 0;
            if (CurMode != GalaxyMode.Stellar)
            {
                Rectangle enterbutton = new Rectangle(); enterbutton.Width = 400; enterbutton.Height = 300;
                enterbutton.Fill = new VisualBrush(curstar.PanelInfo); panel.Children.Add(enterbutton); count++;
                enterbutton.Tag = curstar;
                enterbutton.PreviewMouseDown += EnterButton_PreviewMouseDown;
            }
            FleetParamsPanel.FleetScoutButton sendscout = new FleetParamsPanel.FleetScoutButton(null, curstar,1.0);
            panel.Children.Add(sendscout); count++;
            foreach (GSFleet fleet in GSGameInfo.Fleets.Values)
                if (fleet.Target!=null && fleet.Target.Mission==FleetMission.Scout && fleet.Target.Order!=FleetOrder.Return && fleet.Target.TargetID==curstar.ID)
                {
                    ShortFleetInfo fleetinfo = new ShortFleetInfo(fleet); panel.Children.Add(fleetinfo); count++;
                }

            
            panel.Width = count * 400; panel.Height = 300;
            Links.Controller.PopUpCanvas.Place(panel, true);
            //if (CurMode == GalaxyMode.Stellar) return;
            /*
            
            if (Links.Controller.SelectModifier == SelectModifiers.StarForScout)
            {
                Links.Helper.Star = curstar;
                Links.Helper.ClickDelegate(null, null);
            }
            else
                ShowOneStar(curstar, Double.NaN, 0);
                */
            
        }

        private void EnterButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = (Rectangle)sender;
            GSStar star = (GSStar)rect.Tag;
            ShowOneStar(star, Double.NaN, 0);
        }

        public void ShowOneStar(GSStar curstar, double X, double Y)
        {
            //if (curstar != null)
                foreach (GSStar star in Links.Stars.Values)
                {
                    if (star != curstar)
                        star.SetVisible(false);
                    star.ShowName(false);
                }
            TopCanvas.Children.Clear(); TopCanvas.Children.Add(InnerCanvas);  TopCanvas.Children.Add(MiddleImage);//if (TextStoryIcon != null) HighCanvas.Children.Add(TextStoryIcon);
            CurMode = GalaxyMode.Stellar;
            //zoomer.SetPosition(GalaxyMode.Stellar);
            //CreateStellarSystem(curstar, true);
            if (Double.IsNaN(X))
                pt = new Point(curstar.X + Links.GalaxyWidth / 2.0, curstar.Y + Links.GalaxyHeight / 2.0);
            else
            {
                if (X < 10) X = 10; else if (X > Links.GalaxyWidth - 10) X = Links.GalaxyWidth - 10;
                if (Y < 6) Y = 6; else if (Y > Links.GalaxyHeight - 6) Y = Links.GalaxyHeight - 6; 
                pt = new Point(X, Y);
                Links.Controller.mainWindow.Title = String.Format("{0:0.0} {1:0.0}", X, Y);
            }
            InnerX = 10 - pt.X; InnerY = 6 - pt.Y;
            Map.ChangePT(pt);
            Map.ChangeZoom(GalaxyMode.Stellar);
            //pt = new Point(-250 - curstar.X+10, -150 - curstar.Y+6);
            DoubleAnimation TopW = new DoubleAnimation(20, toptime);
            DoubleAnimation TopH = new DoubleAnimation(12, toptime);
            DoubleAnimation TopWM = new DoubleAnimation(MiddleStellarW, toptime);
            DoubleAnimation TopHM = new DoubleAnimation(MiddleStellarH, toptime);
            DoubleAnimation MoveX = new DoubleAnimation(InnerX, innertime);
            DoubleAnimation MoveY = new DoubleAnimation(InnerY, innertime);
            double MiddleX = InnerX * (MiddleStellarW - 20) / (Links.GalaxyWidth - 20);
            //Links.Controller.mainWindow.Title = String.Format("{0:0.0} {1:0.0}", InnerX, MiddleX);
            double MiddleY = InnerY * (MiddleStellarH - 12) / (Links.GalaxyHeight - 12);
            DoubleAnimation MoveXM = new DoubleAnimation(MiddleX, innertime);
            DoubleAnimation MoveYM = new DoubleAnimation(MiddleY, innertime);
            TopCanvas.BeginAnimation(Canvas.WidthProperty, TopW);
            TopCanvas.BeginAnimation(Canvas.HeightProperty, TopH);
            MiddleImage.BeginAnimation(Rectangle.WidthProperty, TopWM);
            MiddleImage.BeginAnimation(Rectangle.HeightProperty, TopHM);
            InnerCanvas.BeginAnimation(Canvas.LeftProperty, MoveX);
            InnerCanvas.BeginAnimation(Canvas.TopProperty, MoveY);
            MiddleImage.BeginAnimation(Canvas.LeftProperty, MoveXM);
            MiddleImage.BeginAnimation(Canvas.TopProperty, MoveYM);
            ShowStellarStars();
        }
        void ShowStellarStars()
        {
            /*foreach (UIElement element in Arrows)
            {
                InnerCanvas.Children.Remove(element);
            }
            Arrows.Clear();*/
            HidePlanetSideInfo();
            foreach (GSStar star in Links.Stars.Values)
            {
                double d = Math.Sqrt(Math.Pow(pt.X - Links.GalaxyWidth/2.0 - star.X, 2) + Math.Pow(pt.Y - Links.GalaxyHeight/2.0 - star.Y, 2));
                if (d < 30)
                {
                    star.CreateStellarSystem(false); star.Animate(true); star.ShowPlanetsAndStorys(true); star.SetVisible(true);
                }
                else
                {
                    star.Animate(false); star.ShowPlanetsAndStorys(false); star.SetVisible(false);
                }
            }
        }
        public void CreateNames(int x, int y)
        {
            List<Rect> rects = new List<Rect>();
            Rect MaxRect = new Rect(0, 0, x, y);
            double sectorx = -Links.GalaxyWidth/2.0;
            double sectory = -Links.GalaxyHeight/2.0;
            Size measuresize = new Size(20, 10);
            foreach (GSStar sectorstar in Links.Stars.Values)
            {
                double starx = sectorstar.X - sectorx;
                double stary = sectorstar.Y - sectory;
                rects.Add(new Rect(new Point(starx - 3, stary - 3), new Point(starx + 3, stary + 3)));
            }
            List<Rect> Names = new List<Rect>();
            foreach (GSStar sectorstar in Links.Stars.Values)
            {
                double starx = sectorstar.X - sectorx;
                double stary = sectorstar.Y - sectory;
                int minx = (int)(starx - 10); int maxx = minx + 20;
                int miny = (int)(stary - 10); int maxy = miny + 20;
                if (minx < 0) minx = 0; if (maxx > x) maxx = x;
                if (miny < 0) miny = 0; if (maxy > y) maxy = y;
                List<Point> points = new List<Point>();
                for (int i = minx; i < maxx; i += 2)
                    for (int j = miny; j < maxy; j += 2)
                    {
                        bool hasintersect = false;
                        Point pt = new Point(i, j);
                        foreach (Rect rect in rects)
                        {
                            if (rect.Contains(pt))
                            {
                                hasintersect = true;
                                break;
                            }
                        }
                        if (hasintersect) continue;
                        points.Add(pt);

                    }
                Canvas canvas = new Canvas();
                TextBlock tb = new TextBlock();
                tb.FontFamily = Links.Font;
                tb.Foreground = Brushes.White;
                tb.Inlines.Add(new Run(sectorstar.Name));
                canvas.Children.Add(tb);
                tb.FontSize = 1.5;
                tb.TextWrapping = TextWrapping.Wrap;
                tb.Measure(measuresize);
                Size sz = tb.DesiredSize;
                if (sz.Width > 8)
                {
                    tb.Width = 8;
                    tb.Measure(measuresize);
                    sz = tb.DesiredSize;
                }
                if (sectorstar.ID % 2 == 0)
                {
                    List<Point> reversepoints = new List<Point>();
                    for (int i = points.Count - 1; i > 0; i--)
                        reversepoints.Add(points[i]);
                    points = reversepoints;
                }
                foreach (Point point in points)
                {
                    Rect curborder = new Rect(point.X, point.Y, sz.Width, sz.Height);
                    if (MaxRect.Contains(curborder) == false) continue;
                    bool hasintersect = false;
                    foreach (Rect rect in rects)
                        if (rect.IntersectsWith(curborder))
                        {
                            hasintersect = true;
                            break;
                        }
                    if (hasintersect) continue;
                    foreach (Rect rect in Names)
                        if (rect.IntersectsWith(curborder))
                        {
                            hasintersect = true;
                            break;
                        }
                    if (hasintersect) continue;
                    Line vertline = new Line();
                    vertline.X1 = point.X - 0.1; vertline.Y1 = point.Y;
                    vertline.X2 = point.X - 0.1; vertline.Y2 = point.Y + sz.Height;
                    vertline.Stroke = Brushes.White;
                    vertline.StrokeThickness = 0.1;
                    canvas.Children.Add(vertline);
                    Line horline = new Line();
                    if (sz.Height > 2)
                    {
                        horline.X1 = point.X - 0.1; horline.Y1 = point.Y + 1.74;
                        horline.X2 = point.X + sz.Width; horline.Y2 = point.Y + 1.74;
                    }
                    else
                    {
                        horline.X1 = point.X - 0.1; horline.Y1 = point.Y + sz.Height;
                        horline.X2 = point.X + sz.Width; horline.Y2 = point.Y + sz.Height;
                    }
                    horline.Stroke = Brushes.White;
                    horline.StrokeThickness = 0.1;
                    canvas.Children.Add(horline);
                    Line lastline = new Line();
                    if (stary > point.Y)
                        lastline.Y1 = vertline.Y2;
                    else
                        lastline.Y1 = point.Y;
                    lastline.X1 = point.X - 0.1;
                    lastline.X2 = starx; lastline.Y2 = stary;
                    lastline.Stroke = Brushes.White;
                    lastline.StrokeThickness = 0.1;
                    canvas.Children.Add(lastline);
                    Canvas.SetLeft(tb, point.X);
                    Canvas.SetTop(tb, point.Y);

                    Names.Add(curborder);
                    sectorstar.NameCanvas = canvas;
                    Canvas.SetZIndex(canvas, 10);

                    break;
                }
            }
        }
        public void DrawNames()
        {
            foreach (GSStar star in SectorStars)
            {
                star.ShowName(true);
            }
        }
    }
  
    class GalaxyAim:Canvas
    {
        static Pen pen = GetPen();
        public GalaxyAim()
        {
            Width = 30; Height = 30;
            Background = new DrawingBrush(
                new GeometryDrawing(null, pen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                    "M100,0 a100,100 0 1,1 -0.1,0 M100,10 a 90,90 0 1,1 -0.1,0 M100,70 v-60 M130,100 h60 M100,130 v60 M70,100 h-60 M100,70 a30,30 0 1,1 -0.1,0 M121,79 l43,-43 M121,121 l43,43 M79,121 l-43,43 M79,79 l-43,-43"))));
        }
        static Pen GetPen()
        {
            Pen Pen = new Pen();
            Pen.Thickness = 5; Pen.Brush = new SolidColorBrush(Color.FromRgb(0, 168, 255));
            Pen.DashStyle = DashStyles.DashDot;
            return Pen;
        }
    }
    class GalaxyHelpCanvas:Canvas
    {
        enum Stapes { Begin, ShowText1, Text1End, ShowText2, Text2End, ShowText3, Text3End, ShowText4, Text4End, ShowText5, Text5End, ShowText6, End}
        bool needstop = false;
        Stapes Stap = Stapes.Begin;
        TextBlock Block;
        InterfaceButton Button;
        System.Windows.Threading.DispatcherTimer timer;
        List<UIElement> Elements = new List<UIElement>();
        string text1 = "Добро пожаловать в игру Управляй галактикой. Далее будет показано небольшое обучение, которое рассакажет Вам об основных элементах игры.";
        string text2 = "В верхней части экрана расположена информация о ваших основных ресурсах. Это Деньги, Металлы, Микросхемы и Антиматерия.";
        string text3 = "Ниже расположены кнопки вызова основных меню. Это экран Галактики (сейчас активирован), экран Государства, экран Колонии, экран Флота и экран Науки.";
        string text4 = "На экране галактики отображается наша галактика. Для колонизации Вам доступно более 2000 планет, рассеяных на почти 400 звёздах. В центре галактики расположено наше Солнце, рядом с которой вращается наша планета - Земля. Для удобства звёзды, рядом с которыми у вас есть колонии, выделены зелёным кружком.";
        string text5 = "Другими цветами указаны различные задания, доступные Вам. Жёлтым цветом обозначена миссия сюжетной цепочки. Красным цветом - одиночная миссия, как правило за неё предполагается приличная награда. Также вам будут доступны многие другие задания. Для выполнения задания Вам понадобится флот кораблей. Корабли - это ваша основная сила. Подробнее о них будет рассказано в меню флот и корабли.";
        string text6 = "Кажется ваши помощники обнаружили подходящую планету для колонизации в системе Денеб. Выберите её и нажмите на значок миссии";
        int curtextpos = 0;
        public GalaxyHelpCanvas()
        {
            Width = 1280;
            Height = 720;
            Background = new SolidColorBrush(Color.FromArgb(80, 0, 0, 0));
            Block = new TextBlock();
            Children.Add(Block);
            Block.FontFamily = Links.Font;
            Block.Width = 800;
            Block.Height = 250;
            Block.FontSize = 30;
            Block.TextWrapping = TextWrapping.Wrap;
            Block.Foreground = Brushes.White;
            Button = new InterfaceButton(200,100,7, 26);
            Button.PutToCanvas(this, 700, 620);
            Button.Visibility = Visibility.Hidden;
            Rectangle rect = new Rectangle(); rect.Width = 250; rect.Height = 500;
            rect.Fill = Links.Brushes.Helper0;
            Children.Add(rect);
            Canvas.SetLeft(rect, 1030); Canvas.SetTop(rect, 220);
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.04);
            timer.Tick += Timer_Tick;
            timer.Start();
            PreviewMouseDown += GalaxyHelpCanvas_PreviewMouseDown;
        }

        private void GalaxyHelpCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            needstop = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            switch (Stap)
            {
                case Stapes.Begin:
                    Stap = Stapes.ShowText1;
                    curtextpos = 1;
                    Block.Text = "";
                    Canvas.SetLeft(Block, 200);
                    Canvas.SetTop(Block, 200);
                    break;
                case Stapes.ShowText1:
                    if (!ShowNextText(text1))
                        Stap = Stapes.Text1End;
                    break;
                case Stapes.Text1End:
                    Button.SetText("Далее");
                    Button.PreviewMouseDown += Text1End;
                    Button.Visibility = Visibility.Visible;
                    timer.Stop();
                    break;
                case Stapes.ShowText2:
                    if (!ShowNextText(text2))
                        Stap = Stapes.Text2End;
                    break;
                case Stapes.Text2End:
                    Button.PreviewMouseDown -= Text1End;
                    Button.PreviewMouseDown += Text2End;
                    Button.Visibility = Visibility.Visible;
                    timer.Stop();
                    break;
                case Stapes.ShowText3:
                    if (!ShowNextText(text3))
                        Stap = Stapes.Text3End;
                    break;
                case Stapes.Text3End:
                    Button.PreviewMouseDown -= Text2End;
                    Button.PreviewMouseDown += Text3End;
                    Button.Visibility = Visibility.Visible;
                    timer.Stop();
                    break;
                case Stapes.ShowText4:
                    if (!ShowNextText(text4))
                        Stap = Stapes.Text4End;
                    break;
                case Stapes.Text4End:
                    Button.PreviewMouseDown -= Text3End;
                    Button.PreviewMouseDown += Text4End;
                    Button.Visibility = Visibility.Visible;
                    timer.Stop();
                    break;
                case Stapes.ShowText5:
                    if (!ShowNextText(text5))
                        Stap = Stapes.Text5End;
                    break;
                case Stapes.Text5End:
                    Button.PreviewMouseDown -= Text4End;
                    Button.PreviewMouseDown += Text5End;
                    Button.Visibility = Visibility.Visible;
                    timer.Stop();
                    break;
                case Stapes.ShowText6:
                    if (!ShowNextText(text6))
                        Stap = Stapes.End;
                    break;
                case Stapes.End:
                    Button.PreviewMouseDown -= Text5End;
                    Button.PreviewMouseDown += End;
                    Button.SetText(Links.Interface("Close"));
                    Button.Visibility = Visibility.Visible;
                    timer.Stop();
                    break;
            }
        }

        private void Text1End(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            needstop = false;
            Elements.Add(PutLine(450, 70, 300, 300));
            Elements.Add(PutLine(650, 70, 350, 300));
            Elements.Add(PutLine(850, 70, 400, 300));
            Elements.Add(PutLine(1050, 70, 450, 300));
            Canvas.SetTop(Block, 300);
            Button.Visibility = Visibility.Hidden;
            Stap = Stapes.ShowText2;
            curtextpos = 0;
            Block.Text = "";
            timer.Start();
        }
        private void Text2End(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            needstop = false;
            foreach (UIElement element in Elements)
            {
                Children.Remove(element);
            }
            Elements.Clear();
            Elements.Add(PutLine(250, 130, 300, 300));
            Elements.Add(PutLine(450, 130, 350, 300));
            Elements.Add(PutLine(650, 130, 400, 300));
            Elements.Add(PutLine(850, 130, 450, 300));
            Elements.Add(PutLine(1050, 130, 500, 300));
            Canvas.SetTop(Block, 300);
            Button.Visibility = Visibility.Hidden;
            Stap = Stapes.ShowText3;
            curtextpos = 0;
            Block.Text = "";
            timer.Start();
        }
        private void Text3End(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            needstop = false;
            foreach (UIElement element in Elements)
            {
                Children.Remove(element);
            }
            Elements.Clear();
            Elements.Add(PutLine(620, 390, 300, 440));
            Canvas.SetTop(Block, 450);
            Button.Visibility = Visibility.Hidden;
            Stap = Stapes.ShowText4;
            curtextpos = 0;
            Block.Text = "";
            timer.Start();
        }
        private void Text4End(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            needstop = false;
            foreach (UIElement element in Elements)
            {
                Children.Remove(element);
            }
            Elements.Clear();
            Elements.Add(PutLine(180, 190, 300, 400));
            Canvas.SetTop(Block, 400);
            Button.Visibility = Visibility.Hidden;
            Stap = Stapes.ShowText5;
            curtextpos = 0;
            Block.Text = "";
            timer.Start();
        }
        private void Text5End(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            needstop = false;
            foreach (UIElement element in Elements)
            {
                Children.Remove(element);
            }
            Elements.Clear();
            Elements.Add(PutLine(180, 190, 300, 300));
            Canvas.SetTop(Block, 300);
            Rectangle rect = Pictogram.GetStoryLineRectangle();
            Children.Add(rect); rect.Width = 50; rect.Height = 50;
            Canvas.SetLeft(rect, 200); Canvas.SetTop(rect, 500);
            Elements.Add(PutLine(230, 490, 300, 400));
            Button.Visibility = Visibility.Hidden;
            Stap = Stapes.ShowText6;
            curtextpos = 0;
            Block.Text = "";
            timer.Start();
        }
        private void End(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }
        public Line PutLine(double x1, double y1, double x2, double y2)
        {
            Line l = new Line(); l.X1 = x1; l.Y1 = y1;
            l.X2 = x2; l.Y2 = y2; Children.Add(l);
            l.Stroke = Brushes.Red;
            l.StrokeThickness = 5;
            l.StrokeStartLineCap = PenLineCap.Triangle;
            l.StrokeEndLineCap = PenLineCap.Round;
            return l;
        }
        public bool ShowNextText(string text)
        {
            if (needstop) curtextpos = text.Length;
            Block.Text = text.Substring(0, curtextpos);
            curtextpos++;
            if (text.Length >= curtextpos) return true;
            else return false;
        }
        
    }
    public class MeteorBelt
    {
        System.Windows.Threading.DispatcherTimer Timer;
        public Canvas canvas;
        double orbitradius;
        GSStar Star;
        List<OneMeteor> meteorlist = new List<OneMeteor>();
        public MeteorBelt(GSStar star, int orbitpos, bool HaveMission)
        {
            Star = star;
            orbitradius = 4 + orbitpos * 0.6;
            //maxangle = Math.Asin(3.1 / orbitradius) / Math.PI * 180;
            //double dr2 = orbitradius + 0.3;
            //double alfa2 = Math.Asin(3 / dr2);
            //double b2 = dr2 * Math.Cos(alfa2);

            canvas = new Canvas();
            canvas.Width = 2*orbitradius;
            canvas.Height = 2*orbitradius;
            Canvas.SetLeft(canvas, 10 - orbitradius);
            Canvas.SetTop(canvas, 10 - orbitradius);

            for (int i = 5; i < 12; i++)
                meteorlist.Add(new OneMeteor(canvas, orbitradius, MeteorType.Simple));
            if (HaveMission)
            {
                OneMeteor[] missions = OneMeteor.GetMissionMeteors(canvas, orbitradius);
                meteorlist.AddRange(missions);
                missions[0].rect.Tag = star;
                missions[0].rect.PreviewMouseDown += OreBeltMission_Run;
                missions[1].rect.Tag = star;
                missions[1].rect.PreviewMouseDown += OreBeltMission_Run;
            }
            Timer = new System.Windows.Threading.DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(1 / 20.0);
            Timer.Tick += Timer_Tick;
        }

        private void OreBeltMission_Run(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Rectangle rect = (Rectangle)sender;
            GSStar star = (GSStar)rect.Tag;
            Mission2 mission = star.Missions[star.OreBelt];
            mission.ShowMissionPanel();
        }

        public void Start()
        {
            Timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Star.NeedAnimate==false)
            {
                Stop();
                return;
            }
            foreach (OneMeteor meteor in meteorlist)
            {
                meteor.Move();
            }
        }
        public void Stop()
        {
            Timer.Stop();
        }
        enum MeteorType { Simple, Mission, Flash}
        class OneMeteor
        {
            static Random rnd = new Random();
            public Rectangle rect;
            public double Radius;
            public double Angle;
            public double Size;
            public double Speed;
            public int RotateSpeed;
            public double RotateAngle;
            public RotateTransform Rotate;
            public bool CanRotate = true;
            public double MaxRadius;
            public OneMeteor(Canvas canvas, double radius, MeteorType type)
            {
                MaxRadius = radius;
                Radius = radius + rnd.Next(-20, 20) / 100.0;
                rect = new Rectangle();
                Speed = rnd.Next(5, 30) / 100.0;
                Angle = rnd.Next(0, 360);
                switch (type)
                {
                    case MeteorType.Simple:
                        Size = rnd.Next(15, 30) / 100.0;
                        rect.Fill = Links.Brushes.Ships.Meteorits[(byte)rnd.Next(200, 209)];
                        rect.RenderTransform = Rotate = new RotateTransform(RotateAngle);
                        break;
                    case MeteorType.Mission:
                        Size = 0.6;
                        rect.Fill=Gets.AddPicMission("OreBeltMissionImage");
                        rect.RenderTransform = Rotate = new RotateTransform(RotateAngle);
                        break;
                    case MeteorType.Flash:
                        Size = 1;
                        rect.Fill = GSStar.RedFlash;
                        CanRotate = false;
                        break;
                }
                rect.Width = Size;
                rect.Height = Size;
                rect.RenderTransformOrigin = new Point(0.5, 0.5);
                RotateAngle = rnd.Next(0, 90);
                RotateSpeed = rnd.Next(-3, 4);
                canvas.Children.Add(rect);
                Place();
            }
            public static OneMeteor[] GetMissionMeteors(Canvas canvas, double radius)
            {
                OneMeteor[] result = new OneMeteor[2];
                result[0] = new OneMeteor(canvas, radius, MeteorType.Flash);
                result[1] = new OneMeteor(canvas, radius, MeteorType.Mission);
                result[0].Angle = result[1].Angle;
                result[0].Speed = result[1].Speed;
                result[0].Radius = result[1].Radius;
                result[0].Place();
                return result;
            }
            void Place()
            {
                double currad = Angle / 180.0 * Math.PI;
                double x = MaxRadius+  Radius * Math.Cos(currad) - Size / 2;
                double y = MaxRadius + Radius * Math.Sin(currad) - Size / 2;
                Canvas.SetLeft(rect,x);
                Canvas.SetTop(rect, y);
                if (CanRotate) Rotate.Angle = RotateAngle;
            }
            public void Move()
            {
                Angle += Speed;
                RotateAngle += RotateSpeed;
                Place();
            }
        }
    }
    class GalaxyRadar : Viewbox
    {
        static PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
        static SolidColorBrush CurBrush = new SolidColorBrush(Color.FromRgb(0, 100, 255));
        ScaleTransform scale;
        RotateTransform rotate, pathrotate;
        public GalaxyRadar()
        {
            int range = Links.Helper.Fleet.FleetBase.Range;
            double size = range / 25.0 * 250;
            Links.Controller.Galaxy.InnerCanvas.Children.Add(this);
            GSStar star = Links.Stars[Links.Helper.Fleet.FleetBase.Land.Planet.StarID];
            Canvas.SetLeft(this, star.X + Links.GalaxyWidth/2.0 - size);
            Canvas.SetTop(this, star.Y + Links.GalaxyHeight/2.0 - size);
            Width = 2*size; Height = 2*size;
            Canvas canvas = new Canvas(); canvas.Width = 300; canvas.Height = 300; Child = canvas;
            Ellipse round = new Ellipse(); round.Width = 300; round.Height = 300; round.Stroke = CurBrush;
            round.StrokeThickness = 15.0/range; canvas.Children.Add(round); round.Fill = new SolidColorBrush(Color.FromArgb(20, 0, 100, 255));
            Ellipse flash = new Ellipse(); flash.Width = 300; flash.Height = 300; flash.RenderTransformOrigin = new Point(0.5, 0.5);
            RadialGradientBrush flashbrush = new RadialGradientBrush(); canvas.Children.Add(flash);
            flashbrush.GradientStops.Add(new GradientStop(Color.FromArgb(176, 0, 100, 255), 0));
            flashbrush.GradientStops.Add(new GradientStop(Color.FromArgb(64, 0, 100, 255), 0.8));
            flashbrush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));
            flash.Fill = flashbrush;
            flash.Clip = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M150,150 h150 a150,150 0 0,0 -44,-106"));
            
            flash.RenderTransform = rotate = new RotateTransform();

            Path path = new Path(); path.Stroke = CurBrush; canvas.Children.Add(path); path.StrokeThickness = 5.0 / range;
            path.Data = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M150,120 a30,30 0 1,1 -0.1,0 M150, 90 a60,60 0 1,1 -0.1,0 M150,60 a90,90 0 1,1 -0.1,0" +
                "  M150,30 a120,120 0 1,1 -0.1,0 M150,0 v300 M0,150 h300 M256,256 l -212,-212 M256,44 l -212,212"));
            path.Clip = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M150,0 a150,150 0 1,1 -0.01,0"));
            GeometryDrawing draw = new GeometryDrawing(Brushes.White, null, new PathGeometry((PathFigureCollection)conv.ConvertFrom(
                "M0,0 h1 M300,300 h-1 M150,150 h150 a150,150 0 0,0 -44,-106z")));
            DrawingBrush pathbrush = new DrawingBrush(draw);
            pathbrush.Transform = pathrotate = new RotateTransform(0, 150, 150);
            path.OpacityMask = pathbrush;

            RenderTransformOrigin = new Point(0.5, 0.5);
            RenderTransform = scale = new ScaleTransform(0.2, 0.2);
            DoubleAnimation anim = new DoubleAnimation(0.2, 1, TimeSpan.FromSeconds(1));
            anim.Completed += Anim_Completed;
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            
           
        }

        private void Anim_Completed(object sender, EventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation(0.2, 1, TimeSpan.FromSeconds(0.5));
            anim.Completed += Anim_Completed1;
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
        }

        private void Anim_Completed1(object sender, EventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation(0, 360, TimeSpan.FromSeconds(10));
            anim.RepeatBehavior = RepeatBehavior.Forever;
            rotate.BeginAnimation(RotateTransform.AngleProperty, anim);
            pathrotate.BeginAnimation(RotateTransform.AngleProperty, anim);
        }
    }
}
