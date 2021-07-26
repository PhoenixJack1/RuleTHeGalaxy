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
    /*
    class SectorPanel:SelectedGrid
    {
        public static int SectorX=2;
        public static int SectorY=2;
        Canvas sectorCanvas;
        //TextBlock InfoBlock;
        List<SectorStar> SectorStars = new List<SectorStar>();
        Path EnemyPath;
        public Path FreeLandsPath;
        public Popup Popup;
        public FrameworkElement PopUpElement;
        public SectorPanel(string Name):base(Name, Links.Controller.mainWindow.Buttons[2])
        {
            Children.RemoveAt(0);
            Viewbox vbx = new Viewbox();
            vbx.Stretch = Stretch.Fill;
            ColumnDefinitions.Add(new ColumnDefinition());//ColumnDefinitions[0].Width=new GridLength(1100);
            //ColumnDefinitions.Add(new ColumnDefinition()); ColumnDefinitions[1].Width = new GridLength(180);
            Children.Add(vbx);
            sectorCanvas = new Canvas();
            sectorCanvas.Width = Links.SectorWidth+10; sectorCanvas.Height = Links.SectorHeight+10;
            vbx.Child = sectorCanvas;
            sectorCanvas.Background = Links.Brushes.SpaceBack;
            Popup = new Popup();
            Popup.Placement = PlacementMode.Mouse;
            Popup.AllowsTransparency = true;
            //Popup.PopupAnimation = PopupAnimation.Slide;
            //popup.StaysOpen = false;
            Popup.MouseLeave += new MouseEventHandler(popup_MouseLeave);
            //sectorCanvas.Background = Brushes.Black;



            //InfoBlock = new TextBlock();
            //InfoBlock.Background = Brushes.Black;
            //InfoBlock.Style = Links.TextStyle;
            //InfoBlock.Foreground = Brushes.White;
            //Children.Add(InfoBlock);
            //Grid.SetColumn(InfoBlock, 1);
        }
        void popup_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.IsOpen = false;
            PopUpElement.ToolTip = "";
        }
       
        public override void Select()
        {
            base.Select();
            CreateSector();
            Refresh();
        }
        public override void DeSelect()
        {
            base.DeSelect();
            RemoveFreeLandsPath();
        }
        public void RemoveFreeLandsPath()
        {
            if (FreeLandsPath != null)
            {
                sectorCanvas.Children.Remove(FreeLandsPath);
                FreeLandsPath = null;
            }
        }
        public void Refresh()
        {
            Gets.GetTotalInfo();
            DrawPath();
            DrawIndicators();
            Links.Controller.CurrentPopup = Popup;
        }
        SortedList<int, StarIndicator> StarIndicators = new SortedList<int, StarIndicator>();
        public void DrawIndicators()
        {
            foreach (StarIndicator indicator in StarIndicators.Values)
            {
                sectorCanvas.Children.Remove(indicator.Ellipse);
            }
            StarIndicators.Clear();
            foreach (Land land in GSGameInfo.PlayerLands.Values)
            {
                GSPlanet planet = Links.Planets[land.PlanetID];
                GSStar star = Links.Stars[planet.StarID];
                if (star.Coordinate.SectorX != SectorX || star.Coordinate.SectorY != SectorY) continue;
                StarIndicator indicator;
                if (StarIndicators.ContainsKey(Links.Planets[land.PlanetID].StarID))
                    indicator = StarIndicators[Links.Planets[land.PlanetID].StarID];
                else
                {
                    indicator = new StarIndicator(Links.Planets[land.PlanetID].StarID, StarIndicatorPos.Sector);
                    StarIndicators.Add(indicator.StarID, indicator);
                    sectorCanvas.Children.Add(indicator.Ellipse);
                }
                indicator.AddLands(land);
            }
            foreach (Mission mission in GSGameInfo.Missions)
            {
                StarIndicator indicator;
                GSStar star = Links.Stars[mission.StarID];
                if (star.Coordinate.SectorX != SectorX || star.Coordinate.SectorY != SectorY) continue;

                if (StarIndicators.ContainsKey(mission.StarID))
                    indicator = StarIndicators[mission.StarID];
                else
                {
                    indicator = new StarIndicator(mission.StarID, StarIndicatorPos.Sector);
                    StarIndicators.Add(indicator.StarID, indicator);
                    sectorCanvas.Children.Add(indicator.Ellipse);
                }
                indicator.AddCommon(mission);
            }

        }
        void ClearSector()
        {
            sectorCanvas.Children.Clear();
        }
        void CreateSector()
        {
            ClearSector();
            SectorStars.Clear();

            Ellipse ell = new Ellipse();

            foreach (GSStar star in Links.Stars.Values)
            {
                if (star.Coordinate.SectorX == SectorX && star.Coordinate.SectorY == SectorY)
                {
                    SectorStar sectorstar = new SectorStar(star, sectorCanvas);
                    sectorstar.Place();
                    SectorStars.Add(sectorstar);
                    
                }
            }
            DrawNames(110, 70);
            
        }
        
        public void DrawNames(int x, int y)
        {
            List<Rect> rects = new List<Rect>();
            Rect MaxRect = new Rect(0, 0, x, y);
            int sectorx = SectorX * Links.SectorWidth - 250;
            int sectory = SectorY * Links.SectorHeight - 150;
            Size measuresize = new Size(20, 10);
            foreach (SectorStar sectorstar in SectorStars)
            {
                double starx = sectorstar.Star.X - sectorx+5;
                double stary = sectorstar.Star.Y - sectory+5;
                rects.Add(new Rect(new Point(starx - 3, stary - 3), new Point(starx + 3, stary + 3)));
            }
            List<Rect> Names = new List<Rect>();
            foreach (SectorStar sectorstar in SectorStars)
            {
                double starx = sectorstar.Star.X - sectorx+5;
                double stary = sectorstar.Star.Y - sectory+5;
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
                TextBlock tb = new TextBlock();
                tb.FontFamily = Links.Font;
                tb.Foreground = Brushes.White;
                tb.Inlines.Add(new Run(sectorstar.Star.GetName()));
                sectorCanvas.Children.Add(tb);
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
                if (sectorstar.Star.ID % 2 == 0)
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
                    vertline.X1 = point.X-0.1; vertline.Y1 = point.Y;
                    vertline.X2 = point.X-0.1; vertline.Y2 = point.Y + sz.Height;
                    vertline.Stroke = Brushes.White;
                    vertline.StrokeThickness = 0.1;
                    sectorCanvas.Children.Add(vertline);
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
                    sectorCanvas.Children.Add(horline);
                    Line lastline = new Line();
                    if (stary > point.Y)
                        lastline.Y1 = vertline.Y2;
                    else
                        lastline.Y1 = point.Y;
                    lastline.X1 = point.X - 0.1;
                    lastline.X2 = starx; lastline.Y2 = stary;
                    lastline.Stroke = Brushes.White;
                    lastline.StrokeThickness = 0.1;
                    sectorCanvas.Children.Add(lastline);
                    Canvas.SetLeft(tb, point.X);
                    Canvas.SetTop(tb, point.Y);
                    
                    Names.Add(curborder);
                    break;
                }
            }
        }
        public void DrawFreeLandsPath()
        {
            if (Gets.UpdateFreeLands() == false) return;
            CombinedGeometry resultgeom = new CombinedGeometry();
            CombinedGeometry combgeom = new CombinedGeometry();
            bool isfirst = true;

            foreach (SectorStar star in SectorStars)
            {
                if (star.Geometry == null) continue;
                if (star.Star.FreeLands == 0) continue;
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
                sectorCanvas.Children.Remove(FreeLandsPath);
            FreeLandsPath = new Path();
            FreeLandsPath.Data = resultgeom;
            FreeLandsPath.Stroke = Brushes.Blue;
            FreeLandsPath.StrokeThickness = 0.3;
            FreeLandsPath.Fill = new SolidColorBrush(Color.FromArgb(50, 0, 255, 0));

            sectorCanvas.Children.Add(FreeLandsPath);
        }
        public void DrawPath()
        {
            CombinedGeometry resultgeom = new CombinedGeometry();
            CombinedGeometry combgeom = new CombinedGeometry();
            bool isfirst = true;
            foreach (SectorStar star in SectorStars)
            {
                if (star.Geometry == null) continue;
                if (star.Star.Enemy == 0) continue;
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
                sectorCanvas.Children.Remove(EnemyPath);
            EnemyPath = new Path();
            EnemyPath.Data = resultgeom;
            EnemyPath.Stroke = Brushes.Red;
            EnemyPath.StrokeThickness = 0.3;
            EnemyPath.Fill = new SolidColorBrush(Color.FromArgb(50, 255, 0, 0));

            sectorCanvas.Children.Add(EnemyPath);
        }
        Path FocusStarPath;
        public void NeedStarFocus(SectorStar star)
        {
            if (FocusStarPath != null)
            {
                sectorCanvas.Children.Remove(FocusStarPath);
                FocusStarPath = null;
            }
            FocusStarPath = new Path();
            FocusStarPath.Data = star.Geometry;
            FocusStarPath.Stroke = Brushes.Green;
            FocusStarPath.StrokeThickness = 0.3;
            FocusStarPath.Fill = new SolidColorBrush(Color.FromArgb(100, 0, 255, 0));
            sectorCanvas.Children.Add(FocusStarPath);
        }
        public void RemoveStarFocus()
        {
            if (FocusStarPath != null)
            {
                sectorCanvas.Children.Remove(FocusStarPath);
                FocusStarPath = null;
            }
        }
        public class SectorStar
        {
            public GSStar Star;
            public Shape[] Shapes;
            Canvas Parent;
            public PathGeometry Geometry;
            public SectorStar(GSStar star, Canvas parent)
            {
                Star = star;
                Parent = parent;
                CreateSectorStarShape();
                CreateFigureGeometry();
            }



            public void Place()
            {
                foreach (Shape starShape in Shapes)
                {
                    Helper ShapeSize = (Helper)starShape.Tag;
                    Parent.Children.Add(starShape);
                    Canvas.SetLeft(starShape, 5+Star.Coordinate.Sector.X - ShapeSize.Width / 2);
                    Canvas.SetTop(starShape, 5+Star.Coordinate.Sector.Y - ShapeSize.Height / 2);
                    Canvas.SetZIndex(starShape, ShapeSize.ZIndex);
                }
            }
            void CreateFigureGeometry()
            {
                if (Star.NearPoints.Count == 0) return;
                PathFigure figure = new PathFigure();
                figure.StartPoint = TranslateFigurePoint(Star.NearPoints[0]);
                for (int i = 1; i < Star.NearPoints.Count; i++)
                    figure.Segments.Add(new LineSegment(TranslateFigurePoint(Star.NearPoints[i]), true));
                figure.IsClosed = true;
                PathGeometry geom = new PathGeometry();
                geom.Figures.Add(figure);
                Geometry = geom;
            }
            Point TranslateFigurePoint(Point pt)
            {
                Point ZeroPoint = new Point(Star.Coordinate.SectorX * Links.SectorWidth, Star.Coordinate.SectorY * Links.SectorHeight);
                return new Point(pt.X - ZeroPoint.X+5 , pt.Y - ZeroPoint.Y+5);
            }
            void CreateSectorStarShape()
            {
                switch (Star.Class)
                {
                    case EStarClass.BH:
                        Ellipse el = new Ellipse();
                        el.Width = 3; el.Height = 3;
                        RadialGradientBrush BHbrush = new RadialGradientBrush();
                        BHbrush.GradientStops.Add(new GradientStop(Colors.Black, 0.2));
                        BHbrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#e6e6fa"), 0.5));
                        BHbrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#ea8df7"), 0.6));
                        BHbrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#4b0082"), 0.8));
                        BHbrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#660099"), 0.9));
                        BHbrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 1));
                        DoubleAnimation BHAnim = new DoubleAnimation(0.2, 0.5, TimeSpan.FromSeconds(5));
                        BHAnim.RepeatBehavior = RepeatBehavior.Forever;
                        BHAnim.AutoReverse = true;
                        BHbrush.GradientStops[0].BeginAnimation(GradientStop.OffsetProperty, BHAnim);
                        el.Fill = BHbrush;
                        el.Tag = new Helper(3, 3, 1,Star.ID);
                        el.MouseEnter += new MouseEventHandler(bh_MouseEnter);
                        el.MouseLeave += new MouseEventHandler(bh_MouseLeave);
                        Shapes=new Shape[] { el }; break;
                    default:
                        
                        //Polygon pg1 = new Polygon();
                        //pg1.Fill = Links.starBrushes[(int)Star.Class];
                        double starsize = 10 - (int)Star.Class + Star.GetSizeModifier();
                        
                        Ellipse st = new Ellipse();
                        double modi = 0.8;
                        st.Width = modi*starsize;
                        st.Height = modi*starsize;
                        st.Fill = Links.starBrushes[(int)Star.Class];
                        st.OpacityMask = RadialTransparent;
                        st.Tag = new Helper(modi* starsize, modi*starsize, 1, Star.ID);
                        st.MouseEnter += new MouseEventHandler(pg1_MouseEnter);
                        st.MouseLeave += new MouseEventHandler(pg1_MouseLeave);
                        st.MouseDown += new MouseButtonEventHandler(pg1_MouseDown);
                        Shapes = new Shape[] { st };
                        st.ToolTip = new StarPanelInfo(Star);
                        st.ToolTipOpening += new ToolTipEventHandler(el_ToolTipOpening);
                        break;    
                    //Shapes= new Shape[] { pg1, pg2 }; break;
                }

            }
            void el_ToolTipOpening(object sender, ToolTipEventArgs e)
            {
                if (Links.Controller.sectorpanel.Popup.IsOpen)
                {
                    Links.Controller.sectorpanel.Popup.IsOpen = false;
                    Links.Controller.sectorpanel.PopUpElement.ToolTip = "";
                }
                Ellipse el = (Ellipse)sender;
                el.ToolTip = null;
                Links.Controller.sectorpanel.PopUpElement = el;
                if (Links.Controller.sectorpanel.StarIndicators.ContainsKey(Star.ID))
                {
                    StarIndicator indicator = Links.Controller.sectorpanel.StarIndicators[Star.ID];
                    Grid grid = indicator.GiveGrid();
                    Links.Controller.sectorpanel.Popup.Child = grid;
                }
                else
                    Links.Controller.sectorpanel.Popup.Child = new StarPanelInfo(Star);
                Links.Controller.sectorpanel.Popup.IsOpen = true;

                //el.ToolTip = new StarPanelInfo(star);

            }
            static RadialGradientBrush RadialTransparent = GetTransparentBrush();
            static RadialGradientBrush GetTransparentBrush()
            {
                RadialGradientBrush brush = new RadialGradientBrush();
                brush.GradientStops.Add(new GradientStop(Colors.Black, 0.2));
                brush.GradientStops.Add(new GradientStop(Color.FromArgb(50, 0, 0, 0), 0.5));
                brush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));
                return brush;
            }
            void pg1_MouseDown(object sender, MouseButtonEventArgs e)
            {
                if (Links.Controller.SpaceSelect == SpaceObjectSelect.Star)
                {
                    e.Handled = true;
                    Links.Helper.Star = Star;
                    Links.Helper.ClickDelegate(null, null);
                }
                else
                {
                    //Polygon pg = (Polygon)sender;
                    //Helper starsize = (Helper)pg.Tag;
                    //Links.Controller.stellarpanel.StarID = Star.ID;// starsize.StarID;
                    //Links.Controller.stellarpanel.Select();
                }
            }
            void pg1_MouseLeave(object sender, MouseEventArgs e)
            {
                //Polygon pg = (Polygon)sender;
                Ellipse pg = (Ellipse)sender;
                pg.RenderTransform = null;
                //Links.Controller.sectorpanel.ClearTextBlockInfo();
                Links.Controller.sectorpanel.RemoveStarFocus();
            }

            void pg1_MouseEnter(object sender, MouseEventArgs e)
            {
                //Polygon pg = (Polygon)sender;
                Ellipse pg = (Ellipse)sender;
                Helper starsize = (Helper)pg.Tag;
                ScaleTransform trans1 = new ScaleTransform(1, 1);
                trans1.CenterX = starsize.Width / 2;
                trans1.CenterY = starsize.Width / 2;
                pg.RenderTransform = trans1;
                DoubleAnimation anim2 = new DoubleAnimation(1, 2, TimeSpan.FromSeconds(2));
                anim2.RepeatBehavior = RepeatBehavior.Forever;
                anim2.AutoReverse = true;
                trans1.BeginAnimation(ScaleTransform.ScaleXProperty, anim2);
                trans1.BeginAnimation(ScaleTransform.ScaleYProperty, anim2);
                //Links.Controller.sectorpanel.ClearTextBlockInfo();
                //Links.Controller.sectorpanel.SetTextBlockInfo(Star.ID);
                if (Links.Controller.SpaceSelect == SpaceObjectSelect.Star)
                {
                    Links.Controller.sectorpanel.NeedStarFocus(this);
                }
            }

            void bh_MouseLeave(object sender, MouseEventArgs e)
            {
                Ellipse el = (Ellipse)sender;
                el.RenderTransform = null;
                //Links.Controller.sectorpanel.ClearTextBlockInfo();
            }

            void bh_MouseEnter(object sender, MouseEventArgs e)
            {
                Ellipse el = (Ellipse)sender;
                Helper bhsize = (Helper)el.Tag;
                ScaleTransform trans1 = new ScaleTransform(1, 1);
                trans1.CenterX = bhsize.Width / 2;
                trans1.CenterY = bhsize.Height / 2;
                el.RenderTransform = trans1;
                DoubleAnimation anim2 = new DoubleAnimation(1, 2, TimeSpan.FromSeconds(2));
                anim2.RepeatBehavior = RepeatBehavior.Forever;
                anim2.AutoReverse = true;
                trans1.BeginAnimation(ScaleTransform.ScaleXProperty, anim2);
                trans1.BeginAnimation(ScaleTransform.ScaleYProperty, anim2);
                //Links.Controller.sectorpanel.ClearTextBlockInfo();
                //Links.Controller.sectorpanel.SetTextBlockInfo(Star.ID);
            }
        }
        

        
        struct Helper
        {
            public double Width;
            public double Height;
            public int ZIndex;
            public int StarID;
            public Helper(double width, double height, int z, int starID)
            {
                Width = width;
                Height = height;
                ZIndex = z;
                StarID = starID;
            }

        }

    }
    */
}
