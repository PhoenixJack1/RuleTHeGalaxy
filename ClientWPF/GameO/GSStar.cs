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
    public enum EStarClass { O, B, A, F, G, K, M, BH }
    public class GSStar:IComparable
    {
        public int ID;
        public GSString En;
        public GSString Ru;
        public double X;
        public double Y;
        public EStarClass Class;
        public byte SizeModifier;
        public TotalCoordintate Coordinate;
        public SortedList<int, GSPlanet> Planets;
        public List<Point> NearPoints = new List<Point>();
        public byte Enemy;
        public byte FreeLands;
        public byte OreBelt;

        public Ellipse el;
        public string Name;
        bool Visible = true;
        public Canvas NameCanvas;
        bool IsShowName = false;
        public bool NeedAnimate = false;
        bool IsAnimated = false;
        public Canvas PlanetsCanvas;
        bool IsPlanetsShow = false;


        MeteorBelt Belt;
        public SortedList<byte, Mission2> Missions;
        public static Canvas StoryCanvas;
        public static GSStar StoryStar;
        public static GSPlanet StoryPlanet;
        public DateTime UpdateTime = DateTime.MinValue;
        public GSStar(int id, GSString en, GSString ru, double x, double y, EStarClass starclass, byte sizemodi, byte orebelt)
        {
            ID = id;
            En = en;
            Ru = ru;
            if (Links.Lang == 0)
                Name = En.ToString();
            else
                Name = Ru.ToString();
            X = x;
            Y = y;
            Class = starclass;
            SizeModifier = sizemodi;
            Planets = new SortedList<int, GSPlanet>();
            Coordinate = new TotalCoordintate(x, y);
            Enemy = 0;
            OreBelt = orebelt;
            if (Class != EStarClass.BH)
            {
                el = CreateStar();
                Canvas.SetLeft(el, Links.GalaxyWidth/2.0 + X - el.Width / 2);
                Canvas.SetTop(el, Links.GalaxyHeight/2.0 + Y - el.Height / 2);
                Canvas.SetZIndex(el, 10);
                el.Tag = this;
                //el.ToolTip = new StarPanelInfo(this, "Добавление панели при создании звезды");
            }
            else
            {
                el = CreateBlackHole();
                Canvas.SetLeft(el, Links.GalaxyWidth/2.0 + X - el.Width / 2);
                Canvas.SetTop(el, Links.GalaxyHeight/2.0 + Y - el.Height / 2);
                Canvas.SetZIndex(el, 10);
                el.Tag = this;
            }
            PlanetsCanvas = new Canvas();
            PlanetsCanvas.Width = 20; PlanetsCanvas.Height = 20;
            Canvas.SetLeft(PlanetsCanvas, X + Links.GalaxyWidth/2.0 - 10); Canvas.SetTop(PlanetsCanvas, Y + Links.GalaxyHeight/2.0 - 10);
            Canvas.SetZIndex(PlanetsCanvas, 10);

        }
        public GSPlanet GetPlanetFromOrbit(byte orbit)
        {
            foreach (GSPlanet planet in Planets.Values)
                if (planet.Orbit == orbit) return planet;
            return null;
        }
        /// <summary> Метод убирает иконку старта миссии с планетарного канваса звезды. Вызывается из метода Get при считывании параметов сторилайна </summary>
        public static void RemoveStoryIcon()
        {
            if (StoryCanvas == null) return;
            StoryStar.PlanetsCanvas.Children.Remove(StoryCanvas);
            StoryCanvas = null; StoryStar = null;
            if (StoryPlanet != null)
                StoryPlanet.Star.CreateStellarSystem(true);
            StoryPlanet = null;
        }
        public static void PutStoryIcon()
        {
            if (StoryCanvas != null) RemoveStoryIcon();
            StoryLine2 story = StoryLine2.StoryLines[GSGameInfo.StoryLinePosition];
            if (story.StoryType == StoryType.SpaceBattle)
            {
                StoryStar = Links.Stars[story.LocationID];
                StoryCanvas = new Canvas(); StoryCanvas.Width = 2; StoryCanvas.Height = 2;
                StoryStar.PlanetsCanvas.Children.Add(StoryCanvas);
                Canvas.SetZIndex(StoryCanvas, 255);
                Canvas.SetLeft(StoryCanvas, StoryStar.PlanetsCanvas.Width / 2 + 2);
                Canvas.SetTop(StoryCanvas, StoryStar.PlanetsCanvas.Height / 2 - 2);
                Ellipse Image = new Ellipse(); Image.Width = 1; Image.Height = 1; Image.Fill = Links.Brushes.CrownBrush;
                StoryCanvas.Children.Add(Image); Canvas.SetLeft(Image, 0.5); Canvas.SetTop(Image, 0.5);
                Ellipse BackEllipse = new Ellipse(); BackEllipse.Width = 2; BackEllipse.Height = 2; StoryCanvas.Children.Add(BackEllipse);
                RadialGradientBrush brush = StoryLine2.GetStoryFlashBrush();
                BackEllipse.Fill = brush;
                Canvas.SetZIndex(Image, 1);
                StoryCanvas.PreviewMouseDown += StoryCanvas_PreviewMouseDown;
            }
            else
            {
                StoryPlanet = Links.Planets[story.LocationID];
                StoryPlanet.Star.CreateStellarSystem(true);
            }
        }
        private static void StoryCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            StoryLine2 story = StoryLine2.StoryLines[GSGameInfo.StoryLinePosition];
            story.PutPanel();
        }
        public StarPanelInfo PanelInfo;
        public void UpdateTips()
        {
            PanelInfo= new StarPanelInfo(this, "Добавление панели при создании звезды");
            //el.ToolTip = new StarPanelInfo(this, "Добавление панели при создании звезды");
        }
        Ellipse CreateStar()
        {

            double size =10- (int)Class + GetSizeModifier();
            Ellipse el = new Ellipse();
            el.Width = size;
            el.Height = size;
            el.Fill = Links.starBrushes[(int)Class];
            RadialGradientBrush oppbrush = new RadialGradientBrush();
            oppbrush.GradientStops.Add(new GradientStop(Colors.Black, 0.1));
            oppbrush.GradientStops.Add(new GradientStop(Color.FromArgb(50, 0, 0, 0), 0.4));
            oppbrush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));
            el.OpacityMask = oppbrush;
            return el;

        }
        Ellipse CreateBlackHole()
        {
            Ellipse bh = new Ellipse();
            double size = 15 - (int)Class;
            bh.Width = size; bh.Height = size;
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.2));
            brush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#e6e6fa"), 0.35));
            brush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#ea8df7"), 0.6));
            brush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#4b0082"), 0.8));
            brush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#660099"), 0.9));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 1));
            bh.Fill = brush;
            return bh;
        }
        public void SetVisible(bool value)
        {
            Visible = value;
            if (Visible)
                el.Visibility = Visibility.Visible;
            else
            { el.Visibility = Visibility.Hidden; ShowName(false); }
        }
        public void ShowName(bool value)
        {
            if (value == true)
            {
                if (IsShowName == false)
                {
                    Links.Controller.Galaxy.InnerCanvas.Children.Add(NameCanvas); IsShowName = true;
                }
            }
            else
            {
                if (IsShowName == true)
                {
                    Links.Controller.Galaxy.InnerCanvas.Children.Remove(NameCanvas); IsShowName = false;
                }
            }
        }
        public void Animate(bool value)
        {
            if (Class == EStarClass.BH) return;
            if (value == true)
            {
                NeedAnimate = true;
                if (Belt != null) Belt.Start();
                if (IsAnimated == false)
                {
                    DoubleAnimation anim = new DoubleAnimation(0.4, 0.8, TimeSpan.FromSeconds(6));
                    anim.AutoReverse = true;
                    anim.Completed += Anim_Completed;
                    RadialGradientBrush oppmask = (RadialGradientBrush)((Ellipse)el).OpacityMask;
                    GradientStop gr = oppmask.GradientStops[1];
                    gr.BeginAnimation(GradientStop.OffsetProperty, anim);
                    IsAnimated = true;
                }
            }
            else
            {
                NeedAnimate = false;
            }
        }

        private void Anim_Completed(object sender, EventArgs e)
        {
            if (NeedAnimate)
            {
                DoubleAnimation anim = new DoubleAnimation(0.4, 0.8, TimeSpan.FromSeconds(6));
                anim.AutoReverse = true;
                anim.Completed += Anim_Completed;
                RadialGradientBrush oppmask = (RadialGradientBrush)((Ellipse)el).OpacityMask;
                GradientStop gr = oppmask.GradientStops[1];
                gr.BeginAnimation(GradientStop.OffsetProperty, anim);
                IsAnimated = true;
            }
            else
                IsAnimated = false;
        }
        public void ShowPlanetsAndStorys(bool value)
        {
            if (value == true && IsPlanetsShow == false)
                Links.Controller.Galaxy.InnerCanvas.Children.Add(PlanetsCanvas);
            else if (value == false && IsPlanetsShow == true)
                Links.Controller.Galaxy.InnerCanvas.Children.Remove(PlanetsCanvas);
            IsPlanetsShow = value;
        }
        public MeteorBelt GetBelt(bool HaveMission)
        {
            int beltorbit = ServerLinks.GSStars[ID].OreBelt;// (ID + 4) % 6;
            if (Planets.ContainsKey(beltorbit)) return null;
            //if (ID % 4 != 0) return null;
            if (SizeModifier != 0) return null;
            return Belt = new MeteorBelt(this, beltorbit, HaveMission);
        }
        public int CompareTo(object b)
        {
            GSStar B = (GSStar)b;
            if (B.ID > ID) return -1;
            else if (B.ID < ID) return 1;
            else return 0;
        }
        public int GetSizeModifier()
        {
            if (SizeModifier < 10) return (int)SizeModifier;
            else return (int)((SizeModifier - 10) * (-1));
        }
        public static void FillNearPoints(byte[] array)
        {
            for (int i = 0; i < array.Length; )
            {
                int starid = BitConverter.ToInt32(array, i); i += 4;
                int points = BitConverter.ToInt32(array, i); i += 4;
                List<Point> list = new List<Point>();
                for (int j = 0; j < points; j++)
                {
                    list.Add(new Point(BitConverter.ToDouble(array, i), BitConverter.ToDouble(array, i + 8)));
                    i+=16;
                }
                Links.Stars[starid].NearPoints = list;
            }
        }
        public static SortedList<int, GSStar> GetList(byte[] array)
        {
            SortedList<int, GSStar> list = new SortedList<int, GSStar>();
            for (int i = 0; i < array.Length; )
            {
                int id = BitConverter.ToInt32(array, i); i += 4;
                GSString en = new GSString(array, i); i += en.Array.Length;
                GSString ru = new GSString(array, i); i += ru.Array.Length;
                double x = BitConverter.ToDouble(array, i); i += 8;
                double y = BitConverter.ToDouble(array, i); i += 8;
                EStarClass starclass = (EStarClass)array[i]; i++;
                byte sizemodi = array[i]; i++;
                byte orebelt = array[i]; i++;
                GSStar star = new GSStar(id, en, ru, x, y, starclass, sizemodi, orebelt);
                list.Add(id,star);
            }
            return list;
        }
        public string GetName()
        {
            if (Links.Lang == 0)
                return En.ToString();
            else
                return Ru.ToString();
        }
        public void CreateStellarSystem(bool force)
        {
            if (ID == 33)
            {
                ID = ID;
                MissionElements = MissionElements;
            }
            if (force==false && ServerLinks.NowTime == UpdateTime) return;
            else UpdateTime = ServerLinks.NowTime;
            PlanetsCanvas.Children.Clear();
            //Планеты
            foreach (GSPlanet planet in Planets.Values)
            {
                InStellarPlanetShapes shapes = planet.GetPlanetCanvas2();
                PlanetsCanvas.Children.Add(shapes.OrbitEllipse);
                PlanetsCanvas.Children.Add(shapes.canvas);
                planet.InGalaxyCanvas = shapes.canvas;
                shapes.canvas.PreviewMouseDown += SelectPlanet_Event;
                shapes.canvas.MouseEnter += Canvas_MouseEnter;
                shapes.canvas.MouseLeave += Canvas_MouseLeave;
                if (planet.PlanetSide == PlanetSide.Alien || planet.PlanetSide == PlanetSide.GreenTeam || planet.PlanetSide == PlanetSide.Mercs
                    || planet.PlanetSide == PlanetSide.Pirate || planet.PlanetSide == PlanetSide.Player || planet.PlanetSide == PlanetSide.Techno)
                {
                    Rectangle SideBack = new Rectangle();
                    SideBack.Width = (planet.Size + 2) *0.15+0.3;
                    SideBack.Height = SideBack.Width;
                    switch (planet.PlanetSide)
                    {
                        case PlanetSide.Alien: SideBack.Fill = Galaxy.StarSideBack.AlienBrush; break;
                        case PlanetSide.GreenTeam: SideBack.Fill = Galaxy.StarSideBack.GreenTeamBrush; break;
                        case PlanetSide.Mercs: SideBack.Fill = Galaxy.StarSideBack.MercsBrush; break;
                        case PlanetSide.Pirate: SideBack.Fill = Galaxy.StarSideBack.PirateBrush; break;
                        case PlanetSide.Player: SideBack.Fill = Galaxy.StarSideBack.PlayerBrush; break;
                        case PlanetSide.Techno: SideBack.Fill = Galaxy.StarSideBack.TechnoBrush; break;
                    }
                    shapes.canvas.Children.Add(SideBack);
                    Canvas.SetLeft(SideBack, -SideBack.Width / 2 + shapes.canvas.Width / 2); Canvas.SetTop(SideBack, -SideBack.Height / 2 + shapes.canvas.Height / 2);
                }
                if (StoryPlanet!=null && StoryPlanet == planet)
                {
                    Ellipse BackEllipse = new Ellipse();
                    BackEllipse.Width = 2;
                    BackEllipse.Height = 2;
                    shapes.canvas.Children.Add(BackEllipse);
                    Canvas.SetLeft(BackEllipse, -1+ shapes.canvas.Width/2); Canvas.SetTop(BackEllipse, -1+shapes.canvas.Height/2);
                    RadialGradientBrush brush = StoryLine2.GetStoryFlashBrush();
                    BackEllipse.Fill = brush;
                }
            }
            MeteorBelt belt = GetBelt(BeltMission);
            if (belt != null) { PlanetsCanvas.Children.Add(belt.canvas); }
            if (StoryStar != null && StoryStar == this && StoryCanvas != null && PlanetsCanvas.Children.Contains(StoryCanvas) == false)
            {
                PlanetsCanvas.Children.Add(StoryCanvas);
            }
            foreach (UIElement element in MissionElements)
            {
                if (PlanetsCanvas.Children.Contains(element) == false)
                    PlanetsCanvas.Children.Add(element);
                //Mission2 mission = (Mission2)((Canvas)element).Tag;
                //if (mission.GetLocation() == Mission2Location.FreeOrbit && PlanetsCanvas.Children.Contains(element) == false)
                 //   PlanetsCanvas.Children.Add(element);
                //else if (mission.GetLocation() == Mission2Location.FreePlanet && Planets[mission.Orbit].InGalaxyCanvas.Children.Contains(element) == false)
               // {
               //     GSPlanet planet = Planets[mission.Orbit];
               //     planet.InGalaxyCanvas.Children.Add(element);
               // }
            }
        }
        List<UIElement> MissionElements = new List<UIElement>();
        public static RadialGradientBrush RedFlash = GetRedFlash();
        bool BeltMission = false;
        public void DrawMissions2()
        {
            if(ID == 33)
                ID = ID;
            foreach (UIElement element in MissionElements)
            {
                PlanetsCanvas.Children.Remove(element);
                //Mission2 mission = (Mission2)((Canvas)element).Tag;
                //if (mission.GetLocation() == Mission2Location.FreeOrbit)
                //    PlanetsCanvas.Children.Remove(element);
                //else if (mission.GetLocation() == Mission2Location.FreePlanet)
                //    Planets[mission.Orbit].InGalaxyCanvas.Children.Remove(element);
            }
            MissionElements.Clear();
            BeltMission = false;
            foreach (KeyValuePair<byte, Mission2> pair in Missions)
            {
                if (pair.Value.Type == Mission2Type.OreBelt)
                {
                    BeltMission = true;
                }
                else if (pair.Value.GetLocation()==Mission2Location.FreePlanet)
                {

                    GSPlanet planet = GetPlanetFromOrbit(pair.Key);
                    Canvas result = new Canvas(); Canvas.SetZIndex(result, 60);
                    MissionElements.Add(result);
                    double angle = ((int)pair.Value.Type * 3 + pair.Key * 4 + pair.Value.StarID) % 20 * 18;
                    double alpha = Math.PI * angle / 180.0;
                    Canvas.SetLeft(result,Canvas.GetLeft(planet.InGalaxyCanvas)+planet.InGalaxyCanvas.Width/2-0.1);
                    Canvas.SetTop(result, Canvas.GetTop(planet.InGalaxyCanvas)+planet.InGalaxyCanvas.Height/2);

                    Ellipse flash = new Ellipse();
                    flash.Width = 2; flash.Height = 2;
                    result.Children.Add(flash); Canvas.SetLeft(flash, -1); Canvas.SetTop(flash, -1);
                    flash.Fill = RedFlash;

                    Ellipse el = new Ellipse();
                    el.Width = 0.5; el.Height = 0.5;
                    el.Fill = pair.Value.GetBrush;
                    result.Children.Add(el); Canvas.SetLeft(el, -0.25); Canvas.SetTop(el, -0.25);

                    result.Tag = pair.Value;
                    //planet.InGalaxyCanvas.Children.Add(result);
                    //result.ToolTip = String.Format("S:{0} P:{1}", planet.Star.ID, planet.ID);
                    PlanetsCanvas.Children.Add(result);
                    result.PreviewMouseDown += Mission2_PreviewMouseDown_Planet;
                }
                else
                {
                    double orbitradius = 4 + pair.Key * 0.6;
                    Canvas result = new Canvas(); Canvas.SetZIndex(result, 60);
                    MissionElements.Add(result);
                    double angle = ((int)pair.Value.Type * 3 + pair.Key * 4 + pair.Value.StarID) % 20 * 18;
                    double alpha = Math.PI * angle / 180.0;
                    Canvas.SetLeft(result, 10 + orbitradius * Math.Cos(alpha));
                    Canvas.SetTop(result, 10 + orbitradius * Math.Sin(alpha));

                    Ellipse flash = new Ellipse();
                    flash.Width = 2; flash.Height = 2;
                    result.Children.Add(flash); Canvas.SetLeft(flash, -1); Canvas.SetTop(flash, -1);
                    flash.Fill = RedFlash;

                    Ellipse el = new Ellipse();
                    el.Width = 0.5; el.Height = 0.5;
                    el.Fill = pair.Value.GetBrush;
                    result.Children.Add(el); Canvas.SetLeft(el, -0.25); Canvas.SetTop(el, -0.25);

                    result.Tag = pair.Value;
                    result.PreviewMouseDown += Mission2_PreviewMouseDown;

                    PlanetsCanvas.Children.Add(result);
                }
            }
        }
        private void Mission2_PreviewMouseDown_Planet(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Canvas canvas = (Canvas)sender;
            Mission2 mission = (Mission2)canvas.Tag;
            Planets[mission.Orbit].PlanetMission = mission;
            SelectPlanet_Event(Planets[mission.Orbit].InGalaxyCanvas, e);
        }
        private void Mission2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Canvas canvas = (Canvas)sender;
            Mission2 mission = (Mission2)canvas.Tag;
            mission.ShowMissionPanel();
        }
        
        static RadialGradientBrush GetRedFlash()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            GradientStop gr1, gr2, gr3;
            brush.GradientStops.Add(gr1 = new GradientStop(Colors.Transparent, 0));
            brush.GradientStops.Add(gr2 = new GradientStop(Colors.Red, 0.1));
            brush.GradientStops.Add(gr3 = new GradientStop(Colors.Transparent, 0.2));
            DoubleAnimation anim = new DoubleAnimation(); anim.Duration = TimeSpan.FromSeconds(2);
            anim.By = 0.8;
            anim.RepeatBehavior = RepeatBehavior.Forever;
            gr1.BeginAnimation(GradientStop.OffsetProperty, anim);
            gr2.BeginAnimation(GradientStop.OffsetProperty, anim);
            gr3.BeginAnimation(GradientStop.OffsetProperty, anim);
            return brush;
        }
        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            if (SelectPlanet != null)
            {
                PlanetsCanvas.Children.Remove(SelectPlanet);
                SelectPlanet = null;
            }
            Links.Controller.Galaxy.Map.RemoveElement();
            //Links.Controller.Galaxy.RemoveFromInfoBorder();// InfoBorder.Child = null;
        }

        static Ellipse SelectPlanet;
        private void Canvas_MouseEnter(object sender, MouseEventArgs e)
        {
            if (SelectPlanet!= null)
            {
                PlanetsCanvas.Children.Remove(SelectPlanet);
                SelectPlanet = null;
            }
            Canvas canvas = (Canvas)sender;
            InStellarPlanetShapes shapes = (InStellarPlanetShapes)canvas.Tag;
            Links.Controller.Galaxy.Map.PutElement(shapes.Planet.PanelInfo);
            //Links.Controller.Galaxy.PutInInfoBorder(shapes.Planet.PanelInfo);// Child = shapes.Planet.PanelInfo;
            //if (Links.Controller.SelectModifier != SelectModifiers.PlanetForColony) return;
           
            SelectPlanet = new Ellipse();
            double planetradius = (shapes.Planet.Size + 2) * 0.06;
            SelectPlanet.Width = 2; SelectPlanet.Height = 2;
            //ImageBrush brush = Gets.AddPic("cfer_r2");
            //SelectPlanet.Fill = brush;
            //RectAnimationUsingKeyFrames anim = new RectAnimationUsingKeyFrames();
            //anim.Duration = TimeSpan.FromSeconds(2);
            //anim.RepeatBehavior = RepeatBehavior.Forever;
            //for (int i = 0; i < 90; i++)
           // {
           //     Rect rect = new Rect(i % 10 * 0.1, (i / 10) / 9.0, 0.1, 1 / 9.0);
           //     anim.KeyFrames.Add(new DiscreteRectKeyFrame(rect, KeyTime.FromPercent(i / 89.0)));
            //}
            //brush.BeginAnimation(ImageBrush.ViewboxProperty, anim);
            RadialGradientBrush brush = new RadialGradientBrush();
            GradientStop gr1, gr2, gr3, gr4, gr5;
            brush.GradientStops.Add(gr1 = new GradientStop(Colors.Transparent, 1.05));
            brush.GradientStops.Add(gr2 = new GradientStop(Color.FromArgb(255, 0, 100, 255), 1.10));
            brush.GradientStops.Add(gr3 = new GradientStop(Colors.White, 1.15));
            brush.GradientStops.Add(gr4 = new GradientStop(Color.FromArgb(255, 0, 100, 255), 1.20));
            brush.GradientStops.Add(gr5 = new GradientStop(Colors.Transparent, 1.25));
            SelectPlanet.Fill = brush;
            DoubleAnimation anim = new DoubleAnimation();
            anim.By = -1.2;
            anim.Duration = TimeSpan.FromSeconds(1);
            anim.AccelerationRatio = 1;
            anim.RepeatBehavior = RepeatBehavior.Forever;
            gr1.BeginAnimation(GradientStop.OffsetProperty, anim);
            gr2.BeginAnimation(GradientStop.OffsetProperty, anim);
            gr3.BeginAnimation(GradientStop.OffsetProperty, anim);
            gr4.BeginAnimation(GradientStop.OffsetProperty, anim);
            gr5.BeginAnimation(GradientStop.OffsetProperty, anim);
            PlanetsCanvas.Children.Add(SelectPlanet);
            //Canvas.SetZIndex(SelectPlanet, 50);
            double left = Canvas.GetLeft(shapes.canvas) + planetradius - SelectPlanet.Width / 2;
            double top = Canvas.GetTop(shapes.canvas) + planetradius - SelectPlanet.Height / 2;
            Canvas.SetLeft(SelectPlanet, left); Canvas.SetTop(SelectPlanet, top);

        }

        private void SelectPlanet_Event(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Canvas canvas = (Canvas)sender;
            InStellarPlanetShapes shapes = (InStellarPlanetShapes)canvas.Tag;
            WrapPanel panel = new WrapPanel();
            int counts = 0;
            Border badinfopanel = null; //панель с информацией, что на планету нельзя лететь, так как на ней сюжетный квест в будущем
            switch (shapes.Planet.PlanetSide)
            {
                case PlanetSide.Alien:
                case PlanetSide.GreenTeam:
                case PlanetSide.Mercs:
                case PlanetSide.Pirate:
                case PlanetSide.Techno:
                    panel.Children.Add(Common.GetRectangle(400, 300, new VisualBrush(shapes.Planet.PanelInfo))); counts++;
                    bool pillagefleet = false;
                    foreach (GSFleet fleet in GSGameInfo.Fleets.Values)
                        if (fleet.Target!=null && fleet.Target.Mission==FleetMission.Pillage && fleet.Target.TargetID==shapes.Planet.ID && fleet.Target.Order!=FleetOrder.Return)
                        { panel.Children.Add(new ShortFleetInfo(fleet)); pillagefleet = true; break; }
                    if (pillagefleet == false && shapes.Planet.QuestPlanet == false && shapes.Planet.PlanetMission==null)
                    {
                        panel.Children.Add(new PillagePanel(shapes.Planet, null, 1));
                        counts++;
                    }
                    bool conquerfleet = false;
                    foreach (GSFleet fleet in GSGameInfo.Fleets.Values)
                        if (fleet.Target!=null && fleet.Target.Mission==FleetMission.Conquer&& fleet.Target.TargetID==shapes.Planet.ID && fleet.Target.Order!=FleetOrder.Return)
                        { panel.Children.Add(new ShortFleetInfo(fleet)); conquerfleet = true; break; }
                    if (conquerfleet == false && shapes.Planet.QuestPlanet==false && shapes.Planet.PlanetMission==null)
                    {
                       panel.Children.Add(new ConquerPanel(shapes.Planet, null, 1));
                        counts++;
                    }
                    if (shapes.Planet.QuestPlanet== true)
                    {
                        panel.Children.Add(badinfopanel = new BadMissionPanel(shapes.Planet, 1));
                        counts++;
                    }
                    break;
                case PlanetSide.Free: panel.Children.Add(Common.GetRectangle(400, 300, new VisualBrush(shapes.Planet.PanelInfo))); counts++;
                    if (shapes.Planet.QuestPlanet == false && shapes.Planet.PlanetMission==null)
                        panel.Children.Add(new CreateAvanpostPanel(shapes.Planet));
                    else 
                        panel.Children.Add(badinfopanel =  new BadMissionPanel(shapes.Planet, 1));
                    counts++;
                    break;
                case PlanetSide.No:
                    panel.Children.Add(Common.GetRectangle(400, 300, new VisualBrush(shapes.Planet.PanelInfo))); counts++; break;

                case PlanetSide.Player: 
                    if (shapes.Planet.LandOrAvanpost==0)
                    {
                        Land curland = null;
                        foreach (Land land in GSGameInfo.PlayerLands.Values)
                        {
                            if (land.PlanetID==shapes.Planet.ID) { curland = land; break; }
                        }
                        Border border = curland.GetShortInfo(true, 400);
                        panel.Children.Add(border);
                        counts++;
                        foreach (GSFleet fleet in curland.Fleets.Values)
                        {
                            panel.Children.Add(new ShortFleetInfo(fleet));
                            counts++;
                        }
                    }
                    else
                    {
                        Avanpost curavanpost = null;
                        foreach (Avanpost avanpost in GSGameInfo.PlayerAvanposts.Values)
                        {
                            if (avanpost.PlanetID == shapes.Planet.ID) { curavanpost = avanpost; break; }
                        }
                        Border border = curavanpost.GetShortInfo(true, 400);
                        panel.Children.Add(border);
                        counts++;
                    }
                    break;
                case PlanetSide.Unknown: panel.Children.Add(Common.GetRectangle(400, 300, new VisualBrush(shapes.Planet.PanelInfo))); counts++; break;
            }
            if (Galaxy.BattleStory!=null && Galaxy.BattleStory.StoryType==StoryType.PlanetBattle && Galaxy.BattleStory.LocationID==shapes.Planet.ID)
            {
                panel.Children.Add(new StartStoryLinePanel(1)); counts++;
                if (badinfopanel!= null) { panel.Children.Remove(badinfopanel); counts--;  badinfopanel = null; }
            }
            if (shapes.Planet.PlanetMission != null)
            {
                panel.Children.Add(new StartMission2Panel(1, shapes.Planet.PlanetMission)); counts++;
                if (badinfopanel != null) { panel.Children.Remove(badinfopanel); counts--; badinfopanel = null; }
                shapes.Planet.PlanetMission = null;
               
            }
            panel.Height = 300; panel.Width = counts * 400;
            Links.Controller.PopUpCanvas.Place(panel, true);
            /*if (shapes.Planet.LandsCount == 0) return;
            if (Links.Controller.SelectModifier == SelectModifiers.None)
            {
                StackPanel panel = new StackPanel(); panel.Orientation = Orientation.Horizontal;
                panel.Height = 300; int panelwidth = 0;
                if (shapes.Land != null) { panel.Children.Add(shapes.Land.GetShortInfo(true)); panelwidth += 300; }
                if (shapes.Fleets.Count != 0)
                    foreach (GSFleet fleet in shapes.Fleets)
                    {
                        FleetEmblem emblem = new FleetEmblem(fleet.Image, 200);
                        emblem.Tag = fleet;
                        panelwidth += 200;
                        emblem.PreviewMouseDown += Emblem_PreviewMouseDown;
                        panel.Children.Add(emblem);
                    }
                panel.Width = panelwidth;
                Links.Controller.PopUpCanvas.Place(panel, true);
            }
            //new ShortLandList(shapes.Planet.ID, false);
            else if (Links.Controller.SelectModifier == SelectModifiers.Land)
                new ShortLandList(shapes.Planet.ID, true);
            else if (Links.Controller.SelectModifier == SelectModifiers.PlanetForColony)
            {
                Links.Helper.Planet = shapes.Planet;
                Links.Helper.ClickDelegate(null, null);
            }*/
        }
        private void Emblem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            FleetEmblem emblem = (FleetEmblem)sender;
            GSFleet fleet = (GSFleet)emblem.Tag;
            FleetParamsPanel panel = new FleetParamsPanel(fleet);
            Links.Controller.PopUpCanvas.Change(panel,"Отображение панели флота при выборе флота с Галактики");
        }
    }
    public class StartMission2Panel:Border
    {
        Mission2 Mission;
        public StartMission2Panel(double size, Mission2 mission)
        {
            Mission = mission;
            Width = 400 * size;
            Height = 300 * size;
            CornerRadius = new CornerRadius(20 * size);
            BorderBrush = Links.Brushes.SkyBlue;
            BorderThickness = new Thickness(4 * size);
            Background = Brushes.Black;
            Canvas canvas = new Canvas(); Child = canvas;
            Rectangle Image = Common.GetRectangle(size * 160, Mission.GetBrush);
            canvas.Children.Add(Image); Canvas.SetLeft(Image, 120 * size); Canvas.SetTop(Image, size * 10);
            TextBlock Text = Common.GetBlock((int)(40 * size), mission.GetTitle(), Brushes.White, (int)(320 * size));
            canvas.Children.Add(Text); Canvas.SetLeft(Text, 40 * size); Canvas.SetTop(Text, 190 * size);
            PreviewMouseDown += StarMissionPanel_PreviewMouseDown;
        }
        private void StarMissionPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Mission.ShowMissionPanel();
        }
    }
    public class StartStoryLinePanel:Border
    {
        public StartStoryLinePanel(double size)
        {
            Width = 400 * size;
            Height = 300 * size;
            CornerRadius = new CornerRadius(20 * size);
            BorderBrush = Links.Brushes.SkyBlue;
            BorderThickness = new Thickness(4 * size);
            Background = Brushes.Black;
            Canvas canvas = new Canvas(); Child = canvas;
            Rectangle Image = Common.GetRectangle(size * 160, Links.Brushes.CrownBrush);
            canvas.Children.Add(Image); Canvas.SetLeft(Image, 120 * size);Canvas.SetTop(Image, size * 10);
            StoryLine2 story = Galaxy.BattleStory;
            TextBlock Text = Common.GetBlock((int)(40 * size), story.Title, Brushes.White, (int)(320 * size));
            canvas.Children.Add(Text); Canvas.SetLeft(Text, 40 * size); Canvas.SetTop(Text, 190 * size);
            PreviewMouseDown += StartStoryLinePanel_PreviewMouseDown;
        }

        private void StartStoryLinePanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            StoryLine2 story = Galaxy.BattleStory;
            StoryLinePanel panel = new StoryLinePanel(story);
            Links.Controller.PopUpCanvas.Change(panel, "Открытие панели сюжетной миссии с планетарной битвой");
        }
    }
    /// <summary>
    /// Панель, показываемая в случае если на планете предусмотрен квест в будущем и на неё нельзя лететь просто так
    /// </summary>
    public class BadMissionPanel:Border
    {
        GSPlanet Planet;
        public BadMissionPanel(GSPlanet planet, double size)
        {
            Planet = planet;
            Width = 400 * size;
            Height = 300 * size;
            CornerRadius = new CornerRadius(20 * size);
            BorderBrush = Links.Brushes.SkyBlue;
            BorderThickness = new Thickness(4 * size);
            Background = Brushes.Black;
            Canvas canvas = new Canvas(); Child = canvas;
            TextBlock Attention = Common.GetBlock((int)(40 * size), "Предупреждение", new SolidColorBrush(Color.FromRgb(255, 28, 133)), (int)(320 * size));
            canvas.Children.Add(Attention); Canvas.SetLeft(Attention, 40*size); Canvas.SetTop(Attention, 10*size);
            TextBlock ErrorText = Common.GetBlock((int)(28 * size),
                "На планете отмечается большое количество враждебных кораблей. Отправка наших людей будет ошибочным шагом", Brushes.White, (int)(380* size));
            canvas.Children.Add(ErrorText); Canvas.SetLeft(ErrorText, 10*size); Canvas.SetTop(ErrorText, 80*size);
        }
    }
    public class ConquerPanel:Border
    {
        GSPlanet Planet;
        GSFleet Fleet;
        public ConquerPanel(GSPlanet planet, GSFleet fleet, double size)
        {
            Fleet = fleet; Planet = planet;
            Width = 400 * size; Height = 300 * size; CornerRadius = new CornerRadius(20 * size); BorderBrush = Links.Brushes.SkyBlue;
            BorderThickness = new Thickness(4 * size);
            Background = Brushes.Black;
            Canvas canvas = new Canvas(); Child = canvas;
            InterfaceButton ConquerButton = new InterfaceButton((int)(320 * size), (int)(80 * size), (int)(7 * size), (int)(40 * size));
            ConquerButton.SetText(Links.Interface("FleetConquer"));
            ConquerButton.PutToCanvas(canvas, (int)(40 * size), (int)(210 * size));
            Rectangle conquerrect = Common.GetRectangle((int)(200 * size), Links.Brushes.FleetConquerBrush, null);
            canvas.Children.Add(conquerrect); Canvas.SetLeft(conquerrect, (int)(100 * size)); Canvas.SetTop(conquerrect, (int)(5 * size));
            PreviewMouseDown += ConquerPanel_PreviewMouseDown;

        }

        private void ConquerPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (Fleet == null)
            {
                Links.Controller.PopUpCanvas.Remove();
                Links.Helper.Planet = Planet;
                Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.FleetForConquer);
            }
        }
    }
    public class PillagePanel:Border
    {
        GSPlanet Planet;
        GSFleet Fleet;
        public PillagePanel(GSPlanet planet, GSFleet fleet, double size)
        {
            Fleet = fleet; Planet = planet;
            Width = 400 * size; Height = 300 * size; CornerRadius = new CornerRadius(20 * size); BorderBrush = Links.Brushes.SkyBlue;
            BorderThickness = new Thickness(4 * size);
            Background = Brushes.Black;
            Canvas canvas = new Canvas(); Child = canvas;
            InterfaceButton PillageButton = new InterfaceButton((int)(320 * size), (int)(80 * size), (int)(7 * size), (int)(40 * size));
            PillageButton.SetText(Links.Interface("FleetPilage"));
            PillageButton.PutToCanvas(canvas, (int)(40 * size), (int)(210 * size));
            Rectangle pillagerect = Common.GetRectangle((int)(200 * size), Links.Brushes.FleetPillageBrush, null);
            canvas.Children.Add(pillagerect); Canvas.SetLeft(pillagerect, (int)(100 * size)); Canvas.SetTop(pillagerect, (int)(5 * size));
            PreviewMouseDown += PillagePanel_PreviewMouseDown;

        }

        private void PillagePanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (Fleet == null)
            {
                Links.Controller.PopUpCanvas.Remove();
                Links.Helper.Planet = Planet;
                Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.FleetForPillage);
            }
        }
    }
    public class CreateAvanpostPanel:Border
    {
        GSPlanet Planet;
        public CreateAvanpostPanel(GSPlanet planet)
        {
            Planet = planet;
            Width = 400; Height = 300; BorderBrush = Links.Brushes.SkyBlue; BorderThickness = new Thickness(3); CornerRadius = new CornerRadius(20);
            Background = Brushes.Black;
            Canvas mainCanvas = new Canvas();
            Child = mainCanvas;
            Rectangle image = Common.GetRectangle(200, Links.Brushes.ColonizationBrush);
            mainCanvas.Children.Add(image); Canvas.SetLeft(image, 100); Canvas.SetTop(image, 10);
            TextBlock block = Common.GetBlock(30, "Построить аванпост", Brushes.White, 300);
            mainCanvas.Children.Add(block); Canvas.SetLeft(block, 50); Canvas.SetTop(block, 200);
            Rectangle money = Common.GetRectangle(50, Links.Brushes.MoneyImageBrush);
            mainCanvas.Children.Add(money); Canvas.SetLeft(money, 100); Canvas.SetTop(money, 240);
            TextBlock price = Common.GetBlock(30, Common.GetAvanpostPrice(Planet).ToString("### ### ### ### ### ### ###"), Brushes.White, 200); price.TextAlignment = TextAlignment.Left;
            mainCanvas.Children.Add(price); Canvas.SetLeft(price, 170); Canvas.SetTop(price, 240);
            PreviewMouseDown += CreateAvanpostPanel_PreviewMouseDown;
        }

        private void CreateAvanpostPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            string[] eventresult = Events.StartColonizeLand(Planet);
            if (eventresult[0]=="1")
            {
                Links.Controller.PopUpCanvas.Change(new SimpleInfoMessage(eventresult[1]), "Отображение ошибки при колонизации планеты");
            }
            else
            {
                Links.Controller.PopUpCanvas.Remove();
                int avanpostid = Int32.Parse(eventresult[1]);
                Gets.GetTotalInfo("После эевента по колонизации планеты");
                Gets.GetResources();
                Colonies.CurrentPlanet = GSGameInfo.PlayerAvanposts[avanpostid].Planet;
                //Colonies.CurrentAvanpost = GSGameInfo.PlayerAvanposts[avanpostid];
                Links.Controller.SelectPanel(GamePanels.Colonies, SelectModifiers.None);
            }
        }

    }
    public class TotalCoordintate
    {
        public StarCoord Absolute;
        public StarCoord Galaxy;
        public StarCoord Sector;
        public int SectorX;
        public int SectorY;
        public TotalCoordintate(double X, double Y)
        {
            Absolute.X = X; Absolute.Y = Y;
            Galaxy.X = Links.GalaxyWidth / 2 + Absolute.X;
            Galaxy.Y = Links.GalaxyHeight / 2 + Absolute.Y;
            SectorX = (int)(Galaxy.X / Links.SectorWidth);
            SectorY = (int)(Galaxy.Y / Links.SectorHeight);
            Sector.X = Galaxy.X - SectorX * Links.SectorWidth;
            Sector.Y = Galaxy.Y - SectorY * Links.SectorHeight;
        }
    }
    public struct StarCoord
    {
        public double X;
        public double Y;
    }
}
