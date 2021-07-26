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
    /*
    class StellarPanel:SelectedGrid
    {
        public int StarID = 0;
        Canvas StellarCanvas;
        TextBlock InfoBlock;
        //public bool PlanetSelectedMode = false;
        public Ellipse CurrentSelectedPlanet;
        public StellarPanel(string Name):base(Name, Links.Controller.mainWindow.Buttons[3])
        {
            Children.Clear();
            ColumnDefinitions.Add(new ColumnDefinition()); ColumnDefinitions[0].Width = new GridLength(1100);
            ColumnDefinitions.Add(new ColumnDefinition()); ColumnDefinitions[1].Width = new GridLength(180);
            //Background = Brushes.Black;
            Background = Links.Brushes.SpaceBack;
            Viewbox vbx = new Viewbox();
            StellarCanvas = new Canvas();
            vbx.Child = StellarCanvas;
            Children.Add(vbx);
            StellarCanvas.ClipToBounds = true;
            StellarCanvas.Width = 1600; StellarCanvas.Height = 800;

            InfoBlock = new TextBlock();
            InfoBlock.Style = Links.TextStyle;
            InfoBlock.Foreground = Brushes.White;
            Children.Add(InfoBlock);
            Grid.SetColumn(InfoBlock, 1);
        }
        public override void Select()
        {
            base.Select();
            CreateStellarSystem();
        }
        GradientStop stop;
        //DoubleAnimation staranim;
        void CreateStellarSystem()
        {
            StellarCanvas.Children.Clear();
            
            CurrentSelectedPlanet = null;
            GSStar star = Links.Stars[StarID];
            Ellipse ellipse = new Ellipse();
            StellarCanvas.Children.Add(ellipse);
            int radius = (12-(int)star.Class +star.GetSizeModifier()) * 50;
            ellipse.Width = 2 * radius;
            ellipse.Height = 2 * radius;
            Canvas.SetLeft(ellipse, -radius);
            Canvas.SetTop(ellipse, 400-radius);
            
            RadialGradientBrush brush = new RadialGradientBrush();
            Color starcolor=((SolidColorBrush)Links.starBrushes[(int)star.Class]).Color;
            brush.GradientStops.Add(new GradientStop(starcolor,0));
            brush.GradientStops.Add(new GradientStop(starcolor,0.3));
            stop=new GradientStop(Color.FromArgb(220,starcolor.R,starcolor.G,starcolor.B),0.31);
            brush.GradientStops.Add(stop);
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(0,0,0,0),1));
            ellipse.Fill = brush;
            Random rnd=new Random();
            double start = rnd.NextDouble();
            DoubleAnimation anim = new DoubleAnimation(0.31+0.34*start, 0.65, TimeSpan.FromSeconds(10*(1-start)));
            
            //anim.RepeatBehavior = RepeatBehavior.Forever;
            //anim.AutoReverse = true;
            anim.Completed += new EventHandler(anim_Completed);
            stop.BeginAnimation(GradientStop.OffsetProperty, anim);
            
            //Планеты
            foreach (int planetID in star.Planets)
            {
                InStellarPlanetShapes shapes = Links.Planets[planetID].GetPlanetCanvas();
                StellarCanvas.Children.Add(shapes.OrbitEllipse);
                StellarCanvas.Children.Add(shapes.PlanetEllipse);
                if (shapes.Belt != null)
                    StellarCanvas.Children.Add(shapes.Belt);
                for (int i = 0; i < shapes.Moons.Length; i++)
                {
                    StellarCanvas.Children.Add(shapes.MoonOrbits[i]);
                    StellarCanvas.Children.Add(shapes.Moons[i]);
                }
                shapes.PlanetEllipse.MouseDown += new MouseButtonEventHandler(PlanetEllipse_MouseDown);
                //if (Links.Controller.SpaceSelect == SpaceObjectSelect.Planet) shapes.PlanetEllipse.PreviewMouseDown += Links.Helper.ClickDelegate;
                //if (PlanetSelectedMode) shapes.PlanetEllipse.PreviewMouseDown += new MouseButtonEventHandler(PlanetEllipse_PreviewMouseDown);
                shapes.PlanetEllipse.MouseEnter += new MouseEventHandler(PlanetEllipse_MouseEnter);
            }
            //Random rnd = new Random();
            int z = rnd.Next(1, 4);
            for (int i = 0; i < z; i++)
                AddMeteorit(i);
        }

        void anim_Completed(object sender, EventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation(0.65, 0.31, TimeSpan.FromSeconds(10));
            anim.RepeatBehavior = RepeatBehavior.Forever;
            anim.AutoReverse = true;
            stop.BeginAnimation(GradientStop.OffsetProperty, anim);
        }

        void PlanetEllipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse el = (Ellipse)sender;
            InStellarPlanetShapes shapes = (InStellarPlanetShapes)el.Tag;

            if (Links.Controller.SpaceSelect == SpaceObjectSelect.Planet)
            {
                Links.Helper.Planet = shapes.Planet;
                Links.Helper.ClickDelegate(null, null);
            }
            else if (Links.Controller.SpaceSelect == SpaceObjectSelect.Land)
            {
                Links.Helper.Planet = shapes.Planet;
                new ShortLandList(shapes.Planet.ID, true);
            }
            else
            {
                Links.Helper.Planet = shapes.Planet;
                new ShortLandList(shapes.Planet.ID, false);
            }
            
        }
        
        void AddMeteorit(int seed)
        {
            Meteorit met = new Meteorit(seed);
            StellarCanvas.Children.Add(met.ellipse);
            met.Start();
            met.ellipse.MouseEnter += new MouseEventHandler(meteor_MouseEnter);
        }
        void meteor_MouseEnter(object sender, MouseEventArgs e)
        {
            InfoBlock.Inlines.Clear();
            Span span = new Span();
            span.Inlines.Add(new Run("Метеорит"));
            span.Inlines.Add(new LineBreak());
            InfoBlock.Inlines.Add(span);
        }
        
        void PlanetEllipse_MouseEnter(object sender, MouseEventArgs e)
        {
            Ellipse el = (Ellipse)sender;
            InStellarPlanetShapes shapes = (InStellarPlanetShapes)el.Tag;
            //SetTextBlockInfo(shapes.ID);
            if (Links.Controller.SpaceSelect == SpaceObjectSelect.Planet)
            {
                if (TargetCanvas != null)
                {
                    StellarCanvas.Children.Remove(TargetCanvas);
                    TargetCanvas = null;
                }
                TargetCanvas = GetTargetCanvas(el);
                StellarCanvas.Children.Add(TargetCanvas);
            }
        }
        Canvas TargetCanvas;
        public Canvas GetTargetCanvas(Ellipse ell)
        {
            Canvas result = new Canvas();
            int width = (int)ell.Width;
            Canvas.SetLeft(result, Canvas.GetLeft(ell) + width / 2);
            Canvas.SetTop(result, Canvas.GetTop(ell) + width / 2);

            Path path = new Path();
            path.Fill = Common.GetLinearBrush(Colors.Green, Colors.LightGreen, Colors.Green);
            PathFigure outradius = new PathFigure();
            outradius.StartPoint = new Point(-width, 0);
            outradius.Segments.Add(new ArcSegment(new Point(-width, 0.1), new Size(width, width), 0, true, SweepDirection.Clockwise, true));
            outradius.IsClosed = true;
            PathFigure inradius = new PathFigure();
            inradius.StartPoint = new Point((-0.8) * width, 0);
            inradius.Segments.Add(new ArcSegment(new Point(-0.8 * width, 0.1), new Size(width * 0.8, width * 0.8), 0, true, SweepDirection.Clockwise, true));
            inradius.IsClosed = true;

            PathFigure leftupsegment = new PathFigure();
            leftupsegment.StartPoint = new Point(-0.9 * width, -1 * width);
            leftupsegment.Segments.Add(new LineSegment(new Point(width * -0.35, width * -0.45), true));
            leftupsegment.Segments.Add(new LineSegment(new Point(width * -0.45, width * -0.35), true));
            leftupsegment.Segments.Add(new LineSegment(new Point(width * -1, width * -0.9), true));

            PathFigure rightupsegment = new PathFigure();
            rightupsegment.StartPoint = new Point(0.9 * width, -1 * width);
            rightupsegment.Segments.Add(new LineSegment(new Point(width * 0.35, width * -0.45), true));
            rightupsegment.Segments.Add(new LineSegment(new Point(width * 0.45, width * -0.35), true));
            rightupsegment.Segments.Add(new LineSegment(new Point(width * 1, width * -0.9), true));

            PathFigure leftdownsegment = new PathFigure();
            leftdownsegment.StartPoint = new Point(width * -0.9, width * 1);
            leftdownsegment.Segments.Add(new LineSegment(new Point(width * -0.35, width * 0.45), true));
            leftdownsegment.Segments.Add(new LineSegment(new Point(width * -0.45, width * 0.35), true));
            leftdownsegment.Segments.Add(new LineSegment(new Point(width * -1, width * 0.9), true));

            PathFigure rightdownsegment = new PathFigure();
            rightdownsegment.StartPoint = new Point(width * 0.9, width);
            rightdownsegment.Segments.Add(new LineSegment(new Point(width * 0.35, width * 0.45), true));
            rightdownsegment.Segments.Add(new LineSegment(new Point(width * 0.45, width * 0.35), true));
            rightdownsegment.Segments.Add(new LineSegment(new Point(width, width * 0.9), true));
            //M90,-100 l-55,55 l10,10 l55,-55z

            PathGeometry geom = new PathGeometry();
            geom.Figures.Add(outradius);
            geom.Figures.Add(inradius);
            geom.Figures.Add(leftupsegment);
            geom.Figures.Add(rightupsegment);
            geom.Figures.Add(leftdownsegment);
            geom.Figures.Add(rightdownsegment);
            path.Data = geom;
            result.Children.Add(path);

            result.RenderTransformOrigin = new Point(0.5, 0.5);
            RotateTransform rotate = new RotateTransform();
            result.RenderTransform = rotate;
            DoubleAnimation rotateanim = new DoubleAnimation(0, 360, TimeSpan.FromSeconds(5));
            rotateanim.RepeatBehavior = RepeatBehavior.Forever;
            rotate.BeginAnimation(RotateTransform.AngleProperty, rotateanim);

            return result;
        }
        
    }
    class Meteorit
    {
        public Ellipse ellipse;
        System.Windows.Threading.DispatcherTimer timer;
        Random rnd;
        bool upperline;
        double x;
        double a;
        double b;
        public Meteorit(int seed)
        {
            ellipse = new Ellipse();
            ellipse.Fill = Links.Brushes.MeteoritBrush;
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            rnd = new Random(DateTime.Now.Millisecond+seed);
            
            ellipse.Width = 10;
            ellipse.Height = 10;
            b = 100 + rnd.Next(300);
            a = 2400 + rnd.Next(600);
            x = 200 + rnd.Next(1600);
            upperline = rnd.Next(2) == 1 ? true : false;
            double y = Y();

            Canvas.SetLeft(ellipse, x-400-5);
            Canvas.SetTop(ellipse, y+400-5);
            timer.Tick += new EventHandler(timer_Tick);
            
        }
        public void Start()
        {
            timer.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            if (x >= a) upperline = false;
            else if (x <= 0) upperline = true;
            if (upperline) x += 1;
            else x -= 1;
            double y = Y();
            Canvas.SetLeft(ellipse, x - 400 - 5);
            Canvas.SetTop(ellipse, y + 400 - 5);
        }
        double Y()
        {
            if (upperline)
                return Math.Sqrt((1 - (x * x) / (a * a)) * (b * b));
            else
                return -Math.Sqrt((1 - (x * x) / (a * a)) * (b * b));
        }
    }
    
    class LandShortPanelInfo : Border
    {

    }
    */
    public class PlanetPanelInfo : Border
    {
        /*public PlanetPanelInfo(GSPlanet Planet, bool a)
        {
            Width = 130;
            Height = 100;
            BorderBrush = Brushes.Black;
            BorderThickness = new Thickness(2);
            LinearGradientBrush brush = Common.GetLinearBrush(Colors.LightGreen, Colors.White, Colors.LightGreen);
            Background = brush;

            Canvas mainCanvas = new Canvas();
            Child = mainCanvas;

            Border nameborder = Common.PutBorder(120, 30, null, 0, 5, 0, mainCanvas);
            Common.PutBlock(16, nameborder, Planet.Name.ToString());

            string landscount = Planet.LandsCount > 0 ? Planet.LandsCount.ToString() : Planet.LandsCount == 0 ? Links.Interface("Unlimited") : "0";
            TextBlock landsblock = Common.GetBlock(16, Links.Interface("Lands") + " " + landscount);
            landsblock.Width = 120; mainCanvas.Children.Add(landsblock);
            Canvas.SetTop(landsblock, 25); Canvas.SetLeft(landsblock, 5);

            TextBlock sizeblock = Common.GetBlock(16, Links.Interface("MaxPopulation") + " " + Planet.MaxPopulation);
            sizeblock.Width = 60; mainCanvas.Children.Add(sizeblock);
            Canvas.SetTop(sizeblock, 40); Canvas.SetLeft(sizeblock, 5);

            TextBlock atmoblock = Common.GetBlock(16, Links.Interface("Atmosphere") + " " + Planet.PlanetType.ToString());
            atmoblock.Width = 60; mainCanvas.Children.Add(atmoblock);
            Canvas.SetTop(atmoblock, 50); Canvas.SetLeft(atmoblock, 65);
        }*/
        public PlanetPanelInfo(GSPlanet Planet, bool fullinfo)
        {
            Width = 200;
            Height = 150;
            Background = Brushes.Black;
            BorderBrush = Links.Brushes.SkyBlue;
            BorderThickness = new Thickness(2);
            CornerRadius = new System.Windows.CornerRadius(10);
            Canvas mainCanvas = new Canvas();
            Child = mainCanvas;
            mainCanvas.ClipToBounds = true;
            Ellipse ellipse = new Ellipse();
            int planetradius = 30 + Planet.Size * 5;
            ellipse.Width = planetradius; ellipse.Height = planetradius;
            Canvas.SetLeft(ellipse, (80 - planetradius) / 2);
            Canvas.SetTop(ellipse, (80 - planetradius) / 2);
            mainCanvas.Children.Add(ellipse);
            ellipse.Fill = Links.Brushes.PlanetsBrushes[Planet.ImageType];
            if (Planet.HasBelt)
            {
                Ellipse belt = new Ellipse();
                belt.Width = planetradius * 1.4;
                belt.Height = planetradius * 0.6;
                //RadialGradientBrush 
                belt.Stroke = Brushes.Brown;
                belt.StrokeThickness = 10;
                LinearGradientBrush beltoppacitymask = new LinearGradientBrush(Colors.White, Color.FromArgb(0, 0, 0, 0), new Point(0.5, 1), new Point(0.5, 0));
                beltoppacitymask.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 0.7));
                belt.OpacityMask = beltoppacitymask;
                double BeltLeft = (80 - planetradius) / 2 - belt.Width * 0.15;
                double BeltTop = (80 - planetradius) / 2 + belt.Height / 2;
                Canvas.SetLeft(belt, BeltLeft);
                Canvas.SetTop(belt, BeltTop);
                mainCanvas.Children.Add(belt);
            }
            string name = Planet.Name.ToString();
            TextBlock lbl = Common.GetBlock(20, name);
            if (name.Length > 12) lbl.FontSize = 16;
            lbl.FontWeight = FontWeights.Normal;
            lbl.Foreground = Brushes.White;
            mainCanvas.Children.Add(lbl);
            lbl.Width = 120;
            Canvas.SetLeft(lbl, 80);
            if (Planet.PlanetSide == PlanetSide.Unknown && fullinfo == false)
            {
                TextBlock unknown = Common.GetBlock(14, Links.Interface("UnknownPlanet"), Brushes.White, 120);
                mainCanvas.Children.Add(unknown);
                Canvas.SetLeft(unknown, 70);
                Canvas.SetTop(unknown, 50);
            }
            else
            {
                for (int i = 0; i < Planet.Locations; i++)
                {
                    Rectangle rect = Common.GetRectangle(20, Links.Brushes.LandIconBrush);
                    mainCanvas.Children.Add(rect);
                    Canvas.SetLeft(rect, 65 + (i % 5) * 25);
                    Canvas.SetTop(rect, 85 + (i / 5) * 25);
                }

                Rectangle peop = Common.GetRectangle(20, Links.Brushes.PeopleImageBrush);
                mainCanvas.Children.Add(peop);
                Canvas.SetLeft(peop, 90);
                Canvas.SetTop(peop, 40);
                Label peop_lbl = Common.CreateLabel(16, "");
                mainCanvas.Children.Add(peop_lbl);
                peop_lbl.Foreground = Brushes.White;
                peop_lbl.FontWeight = FontWeights.Normal;
                Canvas.SetLeft(peop_lbl, 110);
                Canvas.SetTop(peop_lbl, 35);
                if (Planet.MaxPopulation > 0)
                    peop_lbl.Content = Planet.MaxPopulation.ToString() + " " + Links.Interface("mln");
                else
                {
                    peop_lbl.Content = "--";
                    TextBlock NoColonization = Common.GetBlock(18, Links.Interface("NoEnviroment"), Brushes.White, 120);
                    mainCanvas.Children.Add(NoColonization); Canvas.SetLeft(NoColonization, 70); Canvas.SetTop(NoColonization, 70);
                }
                if (Planet.PlanetSide == PlanetSide.Alien)
                {
                    int emblemsize = 20 + Planet.Size * 4;
                    Rectangle Emblem = Common.GetRectangle(emblemsize, Links.Brushes.AlienBrush);
                    mainCanvas.Children.Add(Emblem); Canvas.SetLeft(Emblem, 40 - emblemsize / 2); Canvas.SetTop(Emblem, 40 - emblemsize / 2);
                }
                else if (Planet.PlanetSide == PlanetSide.GreenTeam)
                {
                    int emblemsize = 20 + Planet.Size * 4;
                    Rectangle Emblem = Common.GetRectangle(emblemsize, Links.Brushes.GreenTeamBrush);
                    mainCanvas.Children.Add(Emblem); Canvas.SetLeft(Emblem, 40 - emblemsize / 2); Canvas.SetTop(Emblem, 40 - emblemsize / 2);
                }
                else if (Planet.PlanetSide == PlanetSide.Mercs)
                {
                    int emblemsize = 20 + Planet.Size * 4;
                    Rectangle Emblem = Common.GetRectangle(emblemsize, Links.Brushes.MercTeamBrush);
                    mainCanvas.Children.Add(Emblem); Canvas.SetLeft(Emblem, 40 - emblemsize / 2); Canvas.SetTop(Emblem, 40 - emblemsize / 2);
                }
                else if (Planet.PlanetSide == PlanetSide.Pirate)
                {
                    int emblemsize = 20 + Planet.Size * 4;
                    Rectangle Emblem = Common.GetRectangle(emblemsize, Links.Brushes.PirateBrush);
                    mainCanvas.Children.Add(Emblem); Canvas.SetLeft(Emblem, 40 - emblemsize / 2); Canvas.SetTop(Emblem, 40 - emblemsize / 2);
                }
                else if (Planet.PlanetSide == PlanetSide.Techno)
                {
                    int emblemsize = 20 + Planet.Size * 4;
                    Rectangle Emblem = Common.GetRectangle(emblemsize, Links.Brushes.TechTeamBrush);
                    mainCanvas.Children.Add(Emblem); Canvas.SetLeft(Emblem, 40 - emblemsize / 2); Canvas.SetTop(Emblem, 40 - emblemsize / 2);
                }
                if (Planet.Pillage==true)
                {
                    Rectangle rect = Common.GetRectangle(20, Links.Brushes.FleetPillageBrush);
                    mainCanvas.Children.Add(rect); Canvas.SetLeft(rect, 90); Canvas.SetTop(rect, 62);
                }
                if (Planet.Conquer==true)
                {
                    Rectangle rect = Common.GetRectangle(20, Links.Brushes.FleetConquerBrush);
                    mainCanvas.Children.Add(rect); Canvas.SetLeft(rect, 115); Canvas.SetTop(rect, 62);
                }
                if (Planet.RiotIndicator>0)
                {
                    TextBlock block = Common.GetBlock(12, Planet.RiotIndicator.ToString() + "%", Brushes.White, 80); block.TextAlignment = TextAlignment.Left;
                    mainCanvas.Children.Add(block); Canvas.SetLeft(block, 140); Canvas.SetTop(block, 66);
                    if (Planet.Conquer == false)
                    {
                        Rectangle rect = Common.GetRectangle(20, Links.Brushes.FleetConquerBrush); rect.Opacity = 0.5;
                        mainCanvas.Children.Add(rect); Canvas.SetLeft(rect, 115); Canvas.SetTop(rect, 62);
                    }
                }
                /*
            Rectangle build = Common.GetRectangle(20, Links.Brushes.BuildingSizeBrush);
            mainCanvas.Children.Add(build);
            Canvas.SetLeft(build, 90);
            Canvas.SetTop(build, 60);
            Label build_lbl = Common.CreateLabel(16, "");
            mainCanvas.Children.Add(build_lbl);
            build_lbl.Foreground = Brushes.White;
            build_lbl.FontWeight = FontWeights.Normal;
            Canvas.SetLeft(build_lbl, 110);
            Canvas.SetTop(build_lbl, 55);
            if (Planet.MaxPopulation > 0)
                build_lbl.Content = (Planet.MaxPopulation).ToString();
            else
                build_lbl.Content = "--";
            */
                Border atmospere = Common.GetPlanetAtmospere(Planet.PlanetType);
                mainCanvas.Children.Add(atmospere);
                Canvas.SetLeft(atmospere, 10);
                Canvas.SetTop(atmospere, 90);
            }
        }
    }
}
