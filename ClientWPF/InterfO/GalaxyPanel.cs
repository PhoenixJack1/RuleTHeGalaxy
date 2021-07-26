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
using System.Windows.Controls.Primitives;

namespace Client
{
    //enum GalaxyPanelSelectedMode { Standard, SelectStarsForEnemy };
    /*
    class GalaxyPanel : SelectedGrid
    {
        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();
        //GalaxyPanelSelectedMode Mode;
        public GalaxyPanel(string Name)
            : base(Name, Links.Controller.mainWindow.Buttons[1])
        {
            put();
            Timer = new System.Windows.Threading.DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(5);
            Timer.Tick += new EventHandler(Timer_Tick);

        }

        void Timer_Tick(object sender, EventArgs e)
        {
            if (BattleFieldCanvas.IsActive) Timer.Stop();
            Refresh();
        }
        public Canvas galaxyCanvas;
        public Path EnemyPath;
        public Path FreeLandsPath;
        public SortedList<int, Star> Stars = new SortedList<int, Star>();
        public Popup Popup;
        public FrameworkElement PopUpElement;
        SortedList<int, Path> GreenPlaces = new SortedList<int, Path>();
        SolidColorBrush GreenBrush = new SolidColorBrush(Color.FromArgb(100, 0, 255, 0));
        public void put()
        {
            Children.RemoveAt(0);
            Viewbox vbx = new Viewbox();
            Children.Add(vbx);
            galaxyCanvas = new Canvas();
            galaxyCanvas.ClipToBounds = true;
            vbx.Child = galaxyCanvas;
            vbx.Stretch = Stretch.Fill;
            //galaxyCanvas.Background = Brushes.Black;
            galaxyCanvas.Background = Links.Brushes.SpaceBack;
            galaxyCanvas.Width = Links.GalaxyWidth;
            galaxyCanvas.Height = Links.GalaxyHeight;
            foreach (GSStar star in Links.Stars.Values)
            {
                Star galaxystar = new Star(star, galaxyCanvas);
                Stars.Add(galaxystar.ID, galaxystar);
            }
            SolidColorBrush LineBrush = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
            for (int i = Links.SectorWidth; i < galaxyCanvas.Width; i += Links.SectorWidth)
            {
                Line line = new Line();
                line.Stroke = LineBrush;
                line.StrokeThickness = 1;
                line.X1 = i; line.Y1 = 0;
                line.X2 = i; line.Y2 = galaxyCanvas.Height;
                line.StrokeDashArray = new DoubleCollection { 1.0, 2.0 };
                galaxyCanvas.Children.Add(line);
            }
            for (int i = Links.SectorHeight; i < galaxyCanvas.Height; i += Links.SectorHeight)
            {
                Line line = new Line();
                line.Stroke = LineBrush;
                line.StrokeThickness = 1;
                line.X1 = 0; line.Y1 = i;
                line.X2 = galaxyCanvas.Width; line.Y2 = i;
                line.StrokeDashArray = new DoubleCollection { 1.0, 2.0 };
                galaxyCanvas.Children.Add(line);
            }
            Popup = new Popup();
            Popup.Placement = PlacementMode.Mouse;
            Popup.AllowsTransparency = true;
            //Popup.PopupAnimation = PopupAnimation.Slide;
            //popup.StaysOpen = false;
            Popup.MouseLeave += new MouseEventHandler(popup_MouseLeave);
            galaxyCanvas.MouseDown += new MouseButtonEventHandler(galaxyCanvas_MouseDown);
            
            for (int i = 0; i < Links.Stars.Count; i++)
            {
                Star star = Stars[i];
                Path path = new Path();
                path.Data = star.Geometry;
                path.Stroke = Brushes.Green;
                path.StrokeThickness = 2;
                path.Visibility = Visibility.Hidden;
                path.Fill = GreenBrush; //new SolidColorBrush(Color.FromArgb(100, 0, 255, 0));
                Canvas.SetZIndex(path, 20);
                GreenPlaces.Add(i, path);
                galaxyCanvas.Children.Add(path);


            }
           
        }
        void popup_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.IsOpen = false;
            PopUpElement.ToolTip = "";
        }
        public override void Select()
        {
            base.Select();
            Refresh();
            Timer.Start();
        }/*
        public void Select(GalaxyPanelSelectedMode mode)
        {
            Select();
            Mode = mode;
        }
          
        public override void DeSelect()
        {
            base.DeSelect();
            Timer.Stop();
            RemoveFreeLandsPath();
        }
        
        public void Refresh()
        {
            Gets.GetTotalInfo();
            //Gets.GetTotalInfoNew(RefreshPart2);
            RefreshPart2();
        }
        public void RefreshPart2()
        {
            DrawPath();
            DrawIndicators();
            Links.Controller.CurrentPopup = Popup;
            if (GalaxyRadius != null && !galaxyCanvas.Children.Contains(GalaxyRadius))
                galaxyCanvas.Children.Add(GalaxyRadius);
            if (!Timer.IsEnabled) Timer.Start();
        }
        SortedList<int, StarIndicator> StarIndicators = new SortedList<int, StarIndicator>();
        public void DrawIndicators()
        {
            foreach (StarIndicator indicator in StarIndicators.Values)
            {
                galaxyCanvas.Children.Remove(indicator.Ellipse);
            }
            StarIndicators.Clear();
            foreach (Land land in GSGameInfo.PlayerLands.Values)
            {
                StarIndicator indicator;
                if (StarIndicators.ContainsKey(Links.Planets[land.PlanetID].StarID))
                    indicator = StarIndicators[Links.Planets[land.PlanetID].StarID];
                else
                {
                    indicator = new StarIndicator(Links.Planets[land.PlanetID].StarID, StarIndicatorPos.Galaxy);
                    StarIndicators.Add(indicator.StarID, indicator);
                    galaxyCanvas.Children.Add(indicator.Ellipse);
                }
                indicator.AddLands(land);
            }
            foreach (Mission mission in GSGameInfo.Missions)
            {
                StarIndicator indicator;
                if (StarIndicators.ContainsKey(mission.StarID))
                    indicator = StarIndicators[mission.StarID];
                else
                {
                    indicator = new StarIndicator(mission.StarID, StarIndicatorPos.Galaxy);
                    StarIndicators.Add(indicator.StarID, indicator);
                    galaxyCanvas.Children.Add(indicator.Ellipse);
                }
                indicator.AddCommon(mission);
            }

        }
        public void RemoveFreeLandsPath()
        {
            if (FreeLandsPath != null)
            {
                galaxyCanvas.Children.Remove(FreeLandsPath);
                FreeLandsPath = null;
            }
        }
        public void DrawFreeLandsPath()
        {
            if (Gets.UpdateFreeLands() == false) return;
            CombinedGeometry resultgeom = new CombinedGeometry();
            CombinedGeometry combgeom = new CombinedGeometry();
            bool isfirst = true;

            foreach (Star star in Stars.Values)
            {
                if (star.Geometry == null) continue;
                if (star.star.FreeLands == 0) continue;
                PathGeometry geom = star.Geometry;
                if (isfirst)
                {
                    combgeom.Geometry1 = geom; isfirst = false;
                    resultgeom.Geometry1 = geom;
                }
                else
                {
                    combgeom.Geometry2 = geom;
                    resultgeom = combgeom;
                    combgeom = new CombinedGeometry();
                    combgeom.Geometry1 = resultgeom;
                }
            }

            if (FreeLandsPath != null)
                galaxyCanvas.Children.Remove(FreeLandsPath);
            FreeLandsPath = new Path();
            FreeLandsPath.Data = resultgeom;
            FreeLandsPath.Stroke = Brushes.Blue;
            FreeLandsPath.StrokeThickness = 0.8;
            FreeLandsPath.Fill = new SolidColorBrush(Color.FromArgb(50, 0, 255, 0));

            galaxyCanvas.Children.Add(FreeLandsPath);
        }
        public void DrawPath()
        {
            CombinedGeometry resultgeom = new CombinedGeometry();
            CombinedGeometry combgeom = new CombinedGeometry();
            bool isfirst = true;
            
            foreach (Star star in Stars.Values)
            {
                if (star.Geometry == null) continue;
                if (star.star.Enemy == 0) continue;
                PathGeometry geom = star.Geometry;
                if (isfirst)
                {
                    combgeom.Geometry1 = geom; isfirst = false;
                    resultgeom.Geometry1 = geom;
                }
                else
                {
                    combgeom.Geometry2 = geom;
                    resultgeom = combgeom;
                    combgeom = new CombinedGeometry();
                    combgeom.Geometry1 = resultgeom;
                }
            }
            if (EnemyPath != null)
                galaxyCanvas.Children.Remove(EnemyPath);
            EnemyPath = new Path();
            EnemyPath.Data = resultgeom;
            EnemyPath.Stroke = Brushes.Red;
            EnemyPath.StrokeThickness = 0.8;
            EnemyPath.Fill = new SolidColorBrush(Color.FromArgb(50, 255, 0, 0));
            galaxyCanvas.Children.Add(EnemyPath);

            
        }
        Path FocusStarPath;
        public void NeedStarFocus(Star star)
        {
            
            if (FocusStarPath != null)
            {
                FocusStarPath.Visibility = Visibility.Hidden;
                //FocusStarPath.Fill = Brushes.Transparent;
                //FocusStarPath.Stroke = Brushes.Transparent;
                FocusStarPath = null;
            }
            FocusStarPath = GreenPlaces[star.ID];
            FocusStarPath.Visibility = Visibility.Visible;
            //FocusStarPath = new Path();
            //FocusStarPath.Data = star.Geometry;
            //FocusStarPath.Stroke = Brushes.Green;
            //FocusStarPath.StrokeThickness = 2;
            //FocusStarPath.Fill = GreenBrush; //new SolidColorBrush(Color.FromArgb(100, 0, 255, 0));
            //galaxyCanvas.Children.Add(FocusStarPath);
            
            /*
             if (FocusStarPath != null)
             {
                 galaxyCanvas.Children.Remove(FocusStarPath);
                 FocusStarPath = null;
             }
             FocusStarPath = new Path();
             FocusStarPath.Data = star.Geometry;
             FocusStarPath.Stroke = Brushes.Green;
             FocusStarPath.StrokeThickness = 2;
             FocusStarPath.Fill = new SolidColorBrush(Color.FromArgb(100, 0, 255, 0));
             galaxyCanvas.Children.Add(FocusStarPath);
             
        }
        public void RemoveStarFocus()
        {
                        
            if (FocusStarPath != null)
            {
                FocusStarPath.Visibility = Visibility.Hidden;
                //FocusStarPath.Fill = Brushes.Transparent;
                //FocusStarPath.Stroke = Brushes.Transparent;
                FocusStarPath = null;
            }
            
            /*
            if (FocusStarPath != null)
            {
                galaxyCanvas.Children.Remove(FocusStarPath);
                FocusStarPath = null;
            }
            
        }
        /*
        public void StarSelected(Star star)
        {
            if (Mode == GalaxyPanelSelectedMode.SelectStarsForEnemy)
            {
                string eventresult = Events.SendFleetToAttackEnemyBattle(Links.Helper.Fleet, star.star);
                if (eventresult == "") Links.Controller.fleetpanel.Select();
            }
        }
         
        void galaxyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point MouseClick = Mouse.GetPosition(galaxyCanvas);
                int xSector = (int)(MouseClick.X / Links.SectorWidth);
                int ySector = (int)(MouseClick.Y / Links.SectorHeight);
                SectorPanel.SectorX = xSector;
                SectorPanel.SectorY = ySector;
                Links.Controller.sectorpanel.Select();
            }
        }
        public GalaxyRadius GalaxyRadius;
        public void PutRadius(int radius, int PlanetID)
        {
            radius = radius * 10;
            if (GalaxyRadius!=null)
            {
                GalaxyRadius.Remove();
                GalaxyRadius = null;
            }
            GSPlanet planet = Links.Planets[PlanetID];
            GSStar star = Links.Stars[planet.StarID];
            GalaxyRadius = new GalaxyRadius(radius, star.Coordinate.Galaxy.X, star.Coordinate.Galaxy.Y);
        }
        public class Star
        {
            public GSStar star;
            Shape Figure;
            Size FigureSize;
            public Point Point;
            public int ID;
            public PathGeometry Geometry;
            public Star(GSStar star, Canvas parent)
            {
                this.star = star;
                ID = star.ID;
                Point = new Point(Links.GalaxyWidth / 2 + star.X, Links.GalaxyHeight / 2 + star.Y);
                switch (star.Class)
                {
                    case EStarClass.BH: CreateBlackHole(); break;
                    default: CreateStar(); break;
                }

                parent.Children.Add(Figure);
                double left = star.Coordinate.Galaxy.X - FigureSize.Width / 2;
                double top = star.Coordinate.Galaxy.Y - FigureSize.Height / 2;
                Canvas.SetLeft(Figure, left);
                Canvas.SetTop(Figure, top);
                Canvas.SetZIndex(Figure, 10);

                PathFigure figure = new PathFigure();
                if (star.NearPoints.Count == 0) return;
                figure.StartPoint = star.NearPoints[0];
                for (int i = 1; i < star.NearPoints.Count; i++)
                    figure.Segments.Add(new LineSegment(star.NearPoints[i], true));
                figure.IsClosed = true;
                Geometry = new PathGeometry();
                Geometry.Figures.Add(figure);

                Figure.PreviewMouseDown += new MouseButtonEventHandler(Figure_PreviewMouseDown);
                Figure.MouseEnter += new MouseEventHandler(Figure_MouseEnter);
            }

            void Figure_MouseEnter(object sender, MouseEventArgs e)
            {
                //GSGameInfo.SetPlayerName(new GSString(ID.ToString()));
                if (Links.Controller.SpaceSelect == SpaceObjectSelect.Star)
                {
                    Links.Controller.galaxypanel.NeedStarFocus(this);
                }
            }

            void Figure_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            {
                if (Links.Controller.SpaceSelect == SpaceObjectSelect.Star)
                {
                    e.Handled = true;
                    Links.Helper.Star = star;
                    Links.Helper.ClickDelegate(null, null);
                }
                    //Links.Controller.galaxypanel.StarSelected(this);
           }
            void CreateBlackHole()
            {
                Ellipse bh = new Ellipse();
                double size = 15 - (int)star.Class;
                bh.Width = size; bh.Height = size;
                RadialGradientBrush brush = new RadialGradientBrush();
                brush.GradientStops.Add(new GradientStop(Colors.Black, 0.2));
                brush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#e6e6fa"), 0.35));
                brush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#ea8df7"), 0.6));
                brush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#4b0082"), 0.8));
                brush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#660099"), 0.9));
                brush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 1));
                bh.Fill = brush;
                Figure = bh;
                FigureSize.Width = size; FigureSize.Height = size;
            }
            void CreateStar()
            {
                
                double size = 10 - (int)star.Class + star.GetSizeModifier();
                Ellipse el = new Ellipse();
                el.Width = size;
                el.Height = size;
                el.Fill = Links.starBrushes[(int)star.Class];
                RadialGradientBrush oppbrush = new RadialGradientBrush();
                oppbrush.GradientStops.Add(new GradientStop(Colors.Black, 0.1));
                oppbrush.GradientStops.Add(new GradientStop(Color.FromArgb(50, 0, 0, 0), 0.4));
                oppbrush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));
                el.OpacityMask = oppbrush;
                Figure = el;
                FigureSize.Width = (int)size;
                FigureSize.Height = (int)size;
                el.ToolTipOpening += new ToolTipEventHandler(el_ToolTipOpening);
                el.ToolTip = new StarPanelInfo(star);

             }

            void el_ToolTipOpening(object sender, ToolTipEventArgs e)
            {
                if (Links.Controller.galaxypanel.Popup.IsOpen)
                {
                    Links.Controller.galaxypanel.Popup.IsOpen = false;
                    Links.Controller.galaxypanel.PopUpElement.ToolTip = "";
                }
                Ellipse el = (Ellipse)sender;
                el.ToolTip = null;
                Links.Controller.galaxypanel.PopUpElement = el;
                if (Links.Controller.galaxypanel.StarIndicators.ContainsKey(ID))
                {
                    StarIndicator indicator = Links.Controller.galaxypanel.StarIndicators[ID];
                    Grid grid = indicator.GiveGrid();
                    Links.Controller.galaxypanel.Popup.Child = grid;
                }
                else
                    Links.Controller.galaxypanel.Popup.Child = new StarPanelInfo(star);
                Links.Controller.galaxypanel.Popup.IsOpen = true;
                
                //el.ToolTip = new StarPanelInfo(star);
                
            }
        }
    }
    */
    public class StarPanelInfo : Border
    {
        public StarPanelInfo(GSStar Star, string reason)
        {
            Width = 200;
            Height = 150;
            BorderBrush = Links.Brushes.SkyBlue;
            BorderThickness = new Thickness(2);
            Background = Brushes.Black;
            CornerRadius = new System.Windows.CornerRadius(10);
            ClipToBounds = true;
            Canvas mainCanvas = new Canvas();
            Child = mainCanvas;
            mainCanvas.ClipToBounds = true;
            Geometry geom = new RectangleGeometry(new Rect(new Size(200, 150)), 10, 10);
            mainCanvas.Clip = geom;
            //mainCanvas.Background = Brushes.Black;
            Ellipse ellipse = new Ellipse();
            ellipse.Width = 120; ellipse.Height = 120;
            mainCanvas.Children.Add(ellipse);
            Canvas.SetLeft(ellipse, -60);
            Canvas.SetTop(ellipse, -60);
            RadialGradientBrush sunbrush = new RadialGradientBrush();
            sunbrush.GradientStops.Add(new GradientStop(Links.starColors[(int)Star.Class], 0.3));
            sunbrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 255, 255, 255), 1));
            ellipse.Fill = sunbrush;

            string name = Star.GetName() + " " + Star.ID.ToString();
            TextBlock lbl = Common.GetBlock(20, name);
            lbl.FontWeight = FontWeights.Normal;
            lbl.Foreground = Brushes.White;
            mainCanvas.Children.Add(lbl);
            lbl.Width = 140;
            Canvas.SetLeft(lbl, 60);
            if (Star.Enemy > 0)
            {
                Ellipse infection = new Ellipse();
                infection.Width = 200;
                infection.Height = 20+Star.Enemy;
                mainCanvas.Children.Add(infection);
                //Canvas.SetLeft(infection, 100);
                Canvas.SetTop(infection, 100);
                RadialGradientBrush inf_brush = new RadialGradientBrush();
                inf_brush.GradientStops.Add(new GradientStop(Colors.Purple, 0.3));
                inf_brush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 255, 255, 255), 1));
                infection.Fill = inf_brush;

                Label inf_lbl = Common.CreateLabel(20, Star.Enemy.ToString() + "%");
                inf_lbl.Foreground = Brushes.White;
                mainCanvas.Children.Add(inf_lbl);
                Canvas.SetLeft(inf_lbl, 80);
                Canvas.SetTop(inf_lbl, 115);
            }
            foreach (GSPlanet planet in Star.Planets.Values)
            {
                Ellipse planet_el = new Ellipse();
                int size = 10 + planet.Size * 2;
                planet_el.Width = size;
                planet_el.Height = size;
                planet_el.Fill = Links.Brushes.PlanetsBrushes[planet.ImageType];
                int x = 45 + (planet.Orbit % 5) * 30+10-size/2;
                int y = 60 + (planet.Orbit / 5) * 30+10-size/2;
                mainCanvas.Children.Add(planet_el);
                Canvas.SetLeft(planet_el, x);
                Canvas.SetTop(planet_el, y);
            }
            
        }

    }
    /*enum StarIndicatorType { None, Common, Clan, Lands, CommonClan, CommonLands, ClanLands, Full }
    enum StarIndicatorPos { Galaxy, Sector}
    class StarIndicator
    {
        SortedList<ushort, Mission> Common = new SortedList<ushort, Mission>();
        List<Mission> Clan = new List<Mission>();
        List<Land> Lands = new List<Land>();
        static SortedList<StarIndicatorType, Brush> Brushes = GetBrushes();
        static RadialGradientBrush OpacityMask = GetOpacityMask();
        LinearGradientBrush IndicatorBrush;
        public Ellipse Ellipse;
        public int StarID { get; private set; }
        public StarIndicator(int id, StarIndicatorPos pos)
        {
            StarID = id;
            IndicatorBrush = new LinearGradientBrush();
            Ellipse = new Ellipse();
            Ellipse.Width = 7;
            Ellipse.Height = 7;
            Ellipse.StrokeThickness = 0.2;
            Ellipse.Stroke = System.Windows.Media.Brushes.White;
            //Ellipse.Fill = IndicatorBrush;
            Ellipse.OpacityMask = OpacityMask;
            GSStar star = Links.Stars[id];
            double left, top;
            if (pos == StarIndicatorPos.Galaxy)
            {
                left = star.Coordinate.Galaxy.X - 3.5;
                top = star.Coordinate.Galaxy.Y - 3.5;
            }
            else
            {
                left = star.Coordinate.Sector.X + 1.5;
                top = star.Coordinate.Sector.Y +1.5;
            }
            Canvas.SetLeft(Ellipse, left);
            Canvas.SetTop(Ellipse, top);
            DoubleAnimation anim = new DoubleAnimation(1, 0.5, TimeSpan.FromSeconds(1));
            anim.RepeatBehavior = RepeatBehavior.Forever;
            anim.AutoReverse = true;
            Ellipse.BeginAnimation(Ellipse.OpacityProperty, anim);
        }
        public Grid GiveGrid()
        {
            int counts = 1 + Lands.Count + Common.Count;
            Grid grid = new Grid();
            int rows = 1; int columns = 1; int ColumnSpan = 1;
            switch (counts)
            {
                case 2: rows = 2; columns = 1;  break;
                case 3: rows = 2; columns = 2; ColumnSpan = 2; break;
                case 4: rows = 2; columns = 3; ColumnSpan = 3; break;
                case 5: rows = 3; columns = 2; ColumnSpan = 2; break;
            }
            grid.Height = rows * 150;
            grid.Width = columns * 200;
            for (int i = 0; i < rows; i++)
                grid.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < columns; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            StarPanelInfo starpanel = new StarPanelInfo(Links.Stars[StarID], "Создание коллажа из панели звезды и миссий");
            grid.Children.Add(starpanel);
            Grid.SetColumnSpan(starpanel, ColumnSpan);
            int item = 0;
            for (int i = 0; i < Lands.Count; i++)
            {
                Border border = GetSelfLandInfoBorder(Lands[i]);
                grid.Children.Add(border);
                Grid.SetRow(border, 1 + item / columns);
                Grid.SetColumn(border, item % columns);
                item++;
            }
            for (ushort i = 0; i < Common.Count; i++)
            {
                Border border = Common.Values[i].GetMissionBorder();
                grid.Children.Add(border);
                Grid.SetRow(border, 1 + item / columns);
                Grid.SetColumn(border, item % columns);
                item++;
            }
            return grid;
        }
        Border GetSelfLandInfoBorder(Land land)
        {
            Border border = new Border();
            border.Background = System.Windows.Media.Brushes.Black;
            border.BorderBrush = System.Windows.Media.Brushes.White;
            border.BorderThickness = new Thickness(2);
            border.CornerRadius = new CornerRadius(10);
            border.Width = 200;
            border.Height = 150;
            Canvas canvas = new Canvas();
            border.Child = canvas;
            
            GSPlanet planet = Links.Planets[land.PlanetID];
            //GSPlanet planet = Links.Planets[4];
            Ellipse planet_el = new Ellipse();
            int size = 20 + planet.Size * 4;
            planet_el.Width = size;
            planet_el.Height = size;
            planet_el.Fill = Links.Brushes.PlanetsBrushes[planet.ImageType];
            canvas.Children.Add(planet_el);
            Canvas.SetLeft(planet_el, 5);
            Canvas.SetTop(planet_el, 5);
            
            Client.Common.PutBlock(16, land.Name.ToString(), canvas, 40, 3, 120);
            
            Client.Common.PutRect(canvas, 30, Links.Brushes.PeopleImageBrush, 80, 40, false);
            Client.Common.PutLabel(canvas, 110, 40, 22, string.Format("{0}/{1}", Client.Common.GetThreeSymbol(land.Peoples * 1000000), Client.Common.GetThreeSymbol(planet.MaxPopulation * 1000000)), System.Windows.Media.Brushes.White);
            StackPanel panel1 = new StackPanel(); panel1.Orientation = Orientation.Horizontal; panel1.HorizontalAlignment = HorizontalAlignment.Center;
            Rectangle rect1 = Client.Common.PutRect(canvas, 30, Links.Brushes.BuildingSizeBrush, 5, 70, false);
            canvas.Children.Remove(rect1);
            panel1.Children.Add(rect1);
            Label lbl1=Client.Common.PutLabel(canvas, 35, 70, 20, string.Format("{0}/{1}", land.BuildingsCount, (int)land.Peoples),System.Windows.Media.Brushes.White);
            canvas.Children.Remove(lbl1); panel1.Children.Add(lbl1);
            //canvas.Children.Add(panel1); Canvas.SetLeft(panel1, 5); Canvas.SetTop(panel1, 70);
            Grid grid = new Grid();
            grid.Width = 190; canvas.Children.Add(grid); Canvas.SetLeft(grid, 5); Canvas.SetTop(grid, 70);
            grid.RowDefinitions.Add(new RowDefinition()); grid.RowDefinitions.Add(new RowDefinition());
            grid.Children.Add(panel1);
            StackPanel panel2 = new StackPanel(); panel2.Orientation = Orientation.Horizontal; panel2.HorizontalAlignment = HorizontalAlignment.Center;
            Rectangle rect2 = Client.Common.PutRect(canvas, 30, Links.Brushes.ShipImageBrush, 5, 105, false);
            canvas.Children.Remove(rect2);
            panel2.Children.Add(rect2);
            Label lbl2 = Client.Common.PutLabel(canvas, 35, 105, 20, string.Format("{0}/{1}", land.CalcShipsCount(0), land.GetMaxShips()), System.Windows.Media.Brushes.White);
            canvas.Children.Remove(lbl2);
            panel2.Children.Add(lbl2);
            grid.Children.Add(panel2);
            Grid.SetRow(panel2, 1);
            border.Tag = land;
            border.PreviewMouseDown += new MouseButtonEventHandler(landborder_PreviewMouseDown);
            return border;
        }

        void landborder_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Land land=(Land)(((Border)sender).Tag);
            Links.Controller.Colonies.Activate(land);
        }
        public void AddCommon(Mission mission)
        {
            Common.Add(mission.ID, mission);
            SelectBrush();
        }
         
        public void AddLands(Land land)
        {
            Lands.Add(land);
            SelectBrush();
        }
       // void SetBrush(StarIndicatorType type)
       // {
            //IndicatorBrush.GradientStops.Clear();
           // IndicatorBrush.GradientStops = Brushes[type].GradientStops;
       // }
        void SelectBrush()
        {
            if (Common.Count > 0)
            {
                if (Clan.Count > 0)
                {
                    if (Lands.Count > 0)
                        Ellipse.Fill=Brushes[StarIndicatorType.Full];
                    else
                        Ellipse.Fill=Brushes[StarIndicatorType.CommonClan];
                }
                else if (Lands.Count > 0)
                    Ellipse.Fill=Brushes[StarIndicatorType.CommonLands];
                else
                    Ellipse.Fill=Brushes[StarIndicatorType.Common];
            }
            else if (Clan.Count > 0)
            {
                if (Lands.Count > 0)
                    Ellipse.Fill=Brushes[StarIndicatorType.ClanLands];
                else
                    Ellipse.Fill=Brushes[StarIndicatorType.Clan];
            }
            else if (Lands.Count > 0)
                Ellipse.Fill=Brushes[StarIndicatorType.Lands];
            else
                Ellipse.Fill = Brushes[StarIndicatorType.None];

        }
        static RadialGradientBrush GetOpacityMask()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.5));
            return brush;
        }
        static SortedList<StarIndicatorType, Brush> GetBrushes()
        {
            SortedList<StarIndicatorType, Brush> List = new SortedList<StarIndicatorType, Brush>();
            List.Add(StarIndicatorType.None, new SolidColorBrush());
            List.Add(StarIndicatorType.Common, System.Windows.Media.Brushes.Red);
            List.Add(StarIndicatorType.Clan, System.Windows.Media.Brushes.Blue);
            List.Add(StarIndicatorType.Lands, System.Windows.Media.Brushes.Green);
            List.Add(StarIndicatorType.CommonClan, GetTwoColorBrush(Colors.Red, Colors.Blue));
            List.Add(StarIndicatorType.CommonLands, GetTwoColorBrush(Colors.Red, Colors.Green));
            List.Add(StarIndicatorType.ClanLands, GetTwoColorBrush(Colors.Blue, Colors.Green));
            List.Add(StarIndicatorType.Full, GetLinearBrush(Colors.Gold, Colors.White));
            return List;
        }
        static LinearGradientBrush GetTwoColorBrush(Color color1, Color color2)
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0, 0.5); brush.EndPoint = new Point(1, 0.5);
            brush.GradientStops.Add(new GradientStop(color1, 0.5));
            brush.GradientStops.Add(new GradientStop(color2, 0.5));
            return brush;
        }
        static SolidColorBrush GetLinearBrush(Color color1, Color color2)
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = color1;
            ColorAnimation anim = new ColorAnimation(color1, color2, TimeSpan.FromSeconds(1));
            anim.RepeatBehavior = RepeatBehavior.Forever;
            anim.AutoReverse = true;
            brush.BeginAnimation(SolidColorBrush.ColorProperty, anim);
            return brush;
        }
    }*/
    class GalaxyRadius : Canvas
    {
        Ellipse el;
        Rectangle line;
        Path path;
        public GalaxyRadius(int radius, double x, double y)
        {
            Width = radius * 2;
            Height = radius * 2;
            //Links.Controller.galaxypanel.galaxyCanvas.Children.Add(this);
            Canvas.SetLeft(this, x - Width / 2);
            Canvas.SetTop(this, y - Height / 2);
            el = new Ellipse();
            el.Stroke = Brushes.Green;
            el.Width = radius * 2;
            el.Height = radius * 2;
            Children.Add(el);

            line = new Rectangle();
            Children.Add(line);
            line.Height = radius;
            line.Width = 3;
            line.RadiusX = 3; line.RadiusY = 3;
            Canvas.SetLeft(line, radius - 1.5);
            Canvas.SetTop(line, radius);
            Canvas.SetZIndex(line, 5);
            line.Fill = Brushes.Green;
            line.RenderTransformOrigin = new Point(0.5, 0);

            RotateTransform rotate = new RotateTransform();
            line.RenderTransform = rotate;
            DoubleAnimation anim = new DoubleAnimation(-180, 180, TimeSpan.FromSeconds(3));
            anim.RepeatBehavior = RepeatBehavior.Forever;
            rotate.BeginAnimation(RotateTransform.AngleProperty, anim);


            path = new Path();
            PathFigure figure = new PathFigure();
            figure.StartPoint = new Point(0, 0);
            figure.Segments.Add(new LineSegment(new Point(0, radius), true));
            figure.Segments.Add(new ArcSegment(new Point(radius, 0), new Size(radius, radius), 0, false, SweepDirection.Counterclockwise, true));
            figure.IsClosed = true;
            PathGeometry geom = new PathGeometry();
            geom.Figures.Add(figure);
            path.Data = geom;
            Children.Add(path);
            Canvas.SetLeft(path, radius);
            Canvas.SetTop(path, radius);
            path.Fill = Brushes.Green;
            path.RenderTransformOrigin = new Point(0, 0);
            path.RenderTransform = rotate;

            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Point(1, 0.5); brush.EndPoint = new Point(0, 2);
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(180, 0, 255, 0), 1));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 255, 0), 0.04));
            path.Fill = brush;

        }
        public void Remove()
        {
            //Links.Controller.galaxypanel.galaxyCanvas.Children.Remove(this);
        }
    }
}
