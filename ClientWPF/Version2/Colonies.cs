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
using System.Windows.Media.Effects;

namespace Client
{
    class Colonies:Canvas
    {
        public static Viewbox box;
        public static Canvas InnerCanvas;
        public Colonies()
        {
            Width = 1920;
            Height = 930;
            Canvas.SetTop(this, 100);
            box = new Viewbox();
            Children.Add(box);
            box.Width = 1920;
            box.Height = 930;
            box.Stretch = Stretch.Fill;
            InnerCanvas = new Canvas();
            InnerCanvas.Width = 1230;
            InnerCanvas.Height = 610;
            box.Child = InnerCanvas;
        }
        public static LandPanelISO CurPanel3D;
        //public static CurSectorPanel CurSectorPanel;
        /// <summary> Метод вызывается когда нужно обновить информацию по колонии в связи с постройками </summary>
        public static void InnerRefresh()
        {
            CurPanel3D.Refresh(CurrentPlanet);
          
        }
        public void Activate(Land land)
        {
            Links.Controller.Colonies.Select();
        }
        public static GSPlanet CurrentPlanet;
        public void Select()
        {
            InnerCanvas.Children.Clear();
            Gets.GetTotalInfo("После открытия панели колоний");
            LandPanelISO panel = new LandPanelISO(CurrentPlanet);
            InnerCanvas.Children.Add(panel);
            CurPanel3D = panel;
            
        }
       public void Refresh(bool b)
        {

        }
    }
    class LandBackground
    {
        public static SortedList<int, LandBackground> List = CreateBackgrounds();
        public int ID;
        public Brush Back;
        public Size BackSize;
        public Point BackDelta;
        public Brush Middle;
        public Size MiddleSize;
        public Point MiddleDelta;
        public Brush Front;
        public Size FrontSize;
        public Point FrontDelta;
        public int SectorWidth;
        public int SectorHeight;
        public List<Point> SectorPoints;
        public int SectorImageID;

        public LandBackground(int id, int sw, int sh, int sectorimageid)
        {
            ID = id; SectorWidth = sw; SectorHeight = sh; SectorImageID = sectorimageid;
        }
        public static SortedList<int, LandBackground> CreateBackgrounds()
        {
            SortedList<int, LandBackground> list = new SortedList<int, LandBackground>();
            //Планета - водопад
            list.Add(0, new LandBackground(0, 180, 600, 1));
            list[0].SetBack(Gets.AddPicColony2("vodopad"), new Size(1230, 640), new Point(0, 0));
            list[0].SetMiddle(Gets.AddPicColony2("zadniy_plan"), new Size(1230, 640), new Point(0, 0)); 
            list[0].SetFront(Gets.AddPicColony2("peredniy_plan"), new Size(1230, 640), new Point(0, 0));
            List<Point> list0p = new List<Point>();
            list0p.Add(new Point(400, 120));
            list0p.Add(new Point(800, 120));
            list0p.Add(new Point(300, 220));
            list0p.Add(new Point(600, 220));
            list0p.Add(new Point(900, 220));
            list0p.Add(new Point(400, 320));
            list0p.Add(new Point(800, 320));
            list0p.Add(new Point(600, 420));
            list[0].SectorPoints = list0p;
            //Огненная планета
            list.Add(1, new LandBackground(1, 180, 600, 1));
            list[1].SetBack(Gets.AddPicColony2("fon_ognen"), new Size(1230, 640), new Point(0, 0));
            list[1].SectorPoints = list0p;
            //Ледяная планета
            list.Add(2, new LandBackground(1, 180, 600, 1));
            list[2].SetBack(Gets.AddPicColony2("fon_ice"), new Size(1230, 640), new Point(0, 0));
            list[2].SectorPoints = list0p;
            return list;
        }
        public void SetBack(Brush image, Size size, Point delta)
        {
            Back = image; BackSize = size; BackDelta = delta;
        }
        public void SetMiddle(Brush image, Size size, Point delta)
        {
            Middle = image; MiddleSize = size; MiddleDelta = delta;
        }
        public void SetFront(Brush image, Size size, Point delta)
        {
            Front = image; FrontSize = size; FrontDelta = delta;
        }
    }
    public class LandPanelISO:Canvas
    {
        public Land Land;
        public Avanpost Avanpost;
        List<SectorImageShort> CurSectors = new List<SectorImageShort>();
        LandBackground CurBackground;
        Rectangle Button;
        Canvas HiddenCanvas;
        bool Hidden = false;
        Viewbox TargetInfo = null;
        Colony_Info Info;
        public LandPanelISO(GSPlanet planet)
        {
            Land = planet.GetLand();
            if (Land == null)
            {
                Avanpost = planet.GetAvanpost();
                if (Avanpost == null) throw new Exception();
            }
            Width = 1230; Height = 640;
            CurBackground = LandBackground.List[planet.LandBackgroundID];
          
            if (CurBackground.Back != null)
            {
                Rectangle back = Common.GetRectangle(CurBackground.BackSize.Width, CurBackground.BackSize.Height, CurBackground.Back);
                Children.Add(back); Canvas.SetLeft(back, CurBackground.BackDelta.X); Canvas.SetTop(back, CurBackground.BackDelta.Y);
            }
            if (CurBackground.Middle != null)
            {
                Rectangle middle = Common.GetRectangle(CurBackground.MiddleSize.Width, CurBackground.MiddleSize.Height, CurBackground.Middle);
                Children.Add(middle); Canvas.SetLeft(middle, CurBackground.MiddleDelta.X); Canvas.SetTop(middle, CurBackground.MiddleDelta.Y);
            }
            for (int i = 0; i < planet.Locations; i++)
            {
                SectorImageShort sector;
                if (Land != null)
                    sector = new SectorImageShort(Land, (byte)i, CurBackground.SectorPoints[i], CurBackground.SectorWidth, CurBackground.SectorHeight, CurBackground.SectorImageID);
                else
                    sector = new SectorImageShort(Avanpost, (byte)i, CurBackground.SectorPoints[i], CurBackground.SectorWidth, CurBackground.SectorHeight, CurBackground.SectorImageID);
                Children.Add(sector);
                CurSectors.Add(sector);
            }
            if (CurBackground.Front != null)
            {
                Rectangle front = Common.GetRectangle(CurBackground.FrontSize.Width, CurBackground.FrontSize.Height, CurBackground.Front);
                Children.Add(front); Canvas.SetLeft(front, CurBackground.FrontDelta.X); Canvas.SetTop(front, CurBackground.FrontDelta.Y);
                Canvas.SetZIndex(front, 200);
            }
            Children.Add(new OblakoCanvas());
            HiddenCanvas = new Canvas(); HiddenCanvas.Width = 1230; HiddenCanvas.Height = 640;
            HiddenCanvas.Background = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0));
            Children.Add(HiddenCanvas);
            Canvas.SetZIndex(HiddenCanvas, 250);
            HiddenCanvas.Opacity = 0;
            MouseMove += LandPanelISO_MouseMove;
            PreviewMouseDown += LandPanelISO_PreviewMouseDown;
            Info = new Colony_Info(planet);
            Children.Add(Info); Canvas.SetLeft(Info, 1230- Info.Width+50); Canvas.SetTop(Info, 100);
           /* if (Land != null)
            {
                Button = new Rectangle(); Button.Width = 50; Button.Height = 50; Button.Fill = Brushes.Red;
                Children.Add(Button); Canvas.SetLeft(Button, 1000); Canvas.SetTop(Button, 500);
                Canvas.SetZIndex(Button, 250);
                Button.PreviewMouseDown += Btn_PreviewMouseDown;
            }*/
        }
        void ShowHidden()
        {
            if (Hidden == true) return;
            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.4));
            HiddenCanvas.BeginAnimation(Canvas.OpacityProperty, anim);
            Hidden = true;
        }
        void HideHidden()
        {
            if (Hidden == false) return;
            DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.4));
            HiddenCanvas.BeginAnimation(Canvas.OpacityProperty, anim);
            Hidden = false;
        }
        void HideTargetInfo()
        {
            if (TargetInfo != null)
                Children.Remove(TargetInfo);
            TargetInfo = null;
        }
        void ShowTargetInfo(SectorImageShort sector)
        {
            if (sector.Sector == null) return;
            Viewbox vbx = new Viewbox();
            TargetInfo = vbx;
            vbx.Width = 527 * 0.5;
            vbx.Height = 224 * 0.5;
            Canvas canvas = new Canvas();
            canvas.Width = 527;
            canvas.Height = 224;
            vbx.Child = canvas;
            Canvas.SetZIndex(vbx, 255);
            Children.Add(vbx);
            Rectangle rect = Common.GetRectangle(527, 224, Gets.AddPicColony2("targ"));
            canvas.Children.Add(rect);
            rect.RenderTransformOrigin = new Point(0.5, 0.5);
            string title = "";
            if (sector.Sector.GetSectorType() == LandSectorType.Clear)
                title = "Не определён";
            else
            {
                PeaceSector s = (PeaceSector)sector.Sector;
                title = SectorImages.List[s.Type].ShortTitle;
            }
            TextBlock titleblock = Common.GetBlock(36, title, Links.Brushes.SkyBlue, 300);
            canvas.Children.Add(titleblock);
            if (Canvas.GetLeft(sector) > 1230 / 2)
            {
                rect.RenderTransform = new ScaleTransform(-1, 1);
                Canvas.SetLeft(vbx, Canvas.GetLeft(sector)-160*CurBackground.SectorWidth/180.0);
                Canvas.SetTop(vbx, Canvas.GetTop(sector) + 40*CurBackground.SectorWidth/300.0);
                Canvas.SetLeft(titleblock, 20); Canvas.SetTop(titleblock, 50);
            }
            else
            {
                Canvas.SetLeft(vbx, Canvas.GetLeft(sector)+ 200 * CurBackground.SectorWidth / 180.0);
                Canvas.SetTop(vbx, Canvas.GetTop(sector)+ 40 * CurBackground.SectorWidth / 300.0);
                Canvas.SetLeft(titleblock, 200); Canvas.SetTop(titleblock, 50);
            }
            DoubleAnimationUsingKeyFrames anim = new DoubleAnimationUsingKeyFrames();
            anim.Duration = TimeSpan.FromSeconds(0.4);
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0)));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromPercent(0.25)));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.5)));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromPercent(1)));
            vbx.BeginAnimation(Viewbox.OpacityProperty, anim);
        }
        private void Btn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.PopUpCanvas.Place(new LandInfo(Land));
        }

        private void LandPanelISO_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedSector != null)
                SelectedSector.Click();
        }

        SectorImageShort SelectedSector = null;
        private void LandPanelISO_MouseMove(object sender, MouseEventArgs e)
        {
            SectorImageShort Sector = null;
            foreach (SectorImageShort s in CurSectors)
            {
                HitTestResult result = VisualTreeHelper.HitTest(s.Ramka, e.GetPosition(s.Ramka));
                if (result!=null)
                {
                    if (Sector == null)
                        Sector = s;
                    else if (Sector == s)
                        continue;
                    else if (s.Z > Sector.Z)
                        Sector = s;
                }
            }
            if (Sector == null)
            {
                if (SelectedSector == null)
                    return;
                else if (SelectedSector != null)
                {
                    SelectedSector.Ramka.Stroke = null;
                    SelectedSector.Ramka2.Stroke = null;
                    HideHidden();
                    Canvas.SetZIndex(SelectedSector, SelectedSector.Z);
                    SelectedSector = null;
                    HideTargetInfo();
                }
            }
            else
            {
                if (SelectedSector == null)
                {
                    SelectedSector = Sector;
                    SelectedSector.Ramka.Stroke = Links.Brushes.GreenBlue;
                    SelectedSector.Ramka2.Stroke = Links.Brushes.SkyBlue;
                    ShowHidden();
                    Canvas.SetZIndex(SelectedSector, 254);
                    ShowTargetInfo(SelectedSector);
                }
                else if (SelectedSector == Sector)
                    return;
                else
                {
                    SelectedSector.Ramka.Stroke = null;
                    SelectedSector.Ramka2.Stroke = null;
                    Canvas.SetZIndex(SelectedSector, SelectedSector.Z);
                    HideTargetInfo();
                    SelectedSector = Sector;
                    SelectedSector.Ramka.Stroke = Links.Brushes.GreenBlue;
                    SelectedSector.Ramka2.Stroke = Links.Brushes.SkyBlue;
                    Canvas.SetZIndex(SelectedSector, 254);
                    ShowTargetInfo(SelectedSector);
                }
            }
        }
        public void Refresh(GSPlanet planet)
        {
            Land = planet.GetLand();
            if (Land == null)
            {
                Avanpost = planet.GetAvanpost();
                if (Avanpost == null) throw new Exception();
            }
            foreach (SectorImageShort image in CurSectors)
                Children.Remove(image);
            CurSectors.Clear();
            for (int i = 0; i < planet.Locations; i++)
            {
                SectorImageShort sector;
                if (Land != null)
                    sector = new SectorImageShort(Land, (byte)i, CurBackground.SectorPoints[i], CurBackground.SectorWidth, CurBackground.SectorHeight, CurBackground.SectorImageID);
                else
                    sector = new SectorImageShort(Avanpost, (byte)i, CurBackground.SectorPoints[i], CurBackground.SectorWidth, CurBackground.SectorHeight, CurBackground.SectorImageID);
                Children.Add(sector);
                CurSectors.Add(sector);
            }
            Children.Remove(Info);
            Info = new Colony_Info(planet);
            Children.Add(Info); Canvas.SetLeft(Info, 1230 - Info.Width+50); Canvas.SetTop(Info, 100);
            /*
            if (Button==null && Land != null)
            {
                Button = new Rectangle(); Button.Width = 50; Button.Height = 50; Button.Fill = Brushes.Red;
                Children.Add(Button); Canvas.SetLeft(Button, 1000); Canvas.SetTop(Button, 500);
                Canvas.SetZIndex(Button, 250);
                Button.PreviewMouseDown += Btn_PreviewMouseDown;
            }
            */
        }
        /*
        public void Refresh(Land land)
        {
            Land = land;
            //LandName.Text = Land.Name.ToString();
            //PeopleBlock.Text = Land.Peoples.ToString();
            //BuildingsBlock.Text = Land.BuildingsCount.ToString();
            for (int i = 0; i < land.Locations.Length; i++)
                Children.Remove(CurSectors[i]);
            for (int i = 0; i < land.Locations.Length; i++)
            {
                CurSectors[i] = new SectorImageShort(land, (byte)i, CurBackground.SectorPoints[i], 
                    CurBackground.SectorWidth, CurBackground.SectorHeight, CurBackground.SectorImageID);
                Children.Add(CurSectors[i]);
            }
        }
       */
    }
    class OblakoCanvas : Canvas
    {
        public static Random rnd = new Random();
        public static int Angle;
        public static int Speed;
        static Point Center = new Point(615, 320);
        public OblakoCanvas()
        {
            Canvas.SetZIndex(this, 210);
            Width = 1230; Height = 640; Canvas.SetTop(this, (640 - 1230) / 2);
            int clouds = 20;
            Angle = rnd.Next(0, 360);
            Speed = 20 * rnd.Next(1, 10);
            RenderTransformOrigin = new Point(0.5, 0.5);
            RenderTransform = new RotateTransform(Angle);
            for (int i = 0; i < clouds; i++)
                new Oblako(this);
        }

        class Oblako
        {
            Rectangle Rect;
            Canvas CurCanvas;
            public Oblako(Canvas canvas)
            {
                CurCanvas = canvas;
                Place(true);
            }
            public void Place(bool first)
            {
                Rect = new Rectangle();
                int size = rnd.Next(1, 6);
                Rect.Width = 50 * size; Rect.Height = Rect.Width / 2;
                int image = rnd.Next(1, 5);
                Rect.Fill = Gets.AddPicColony2("oblako_" + image.ToString());
                int time = Speed + rnd.Next(20, 40);
                CurCanvas.Children.Add(Rect);
                int top = rnd.Next(0, 1230);
                Canvas.SetTop(Rect, top - Rect.Height / 2);
                int left;
                if (first)
                    left = (int)(rnd.Next(0, 1230) - Rect.Width / 2);
                else
                    left = (int)(-500 - Rect.Width / 2);
                DoubleAnimation anim1 = new DoubleAnimation(left, left + 2230, TimeSpan.FromSeconds(time));
                anim1.Completed += Anim1_Completed;
                Rect.BeginAnimation(Canvas.LeftProperty, anim1);
                Rect.RenderTransformOrigin = new Point(0.5, 0.5);
                Rect.RenderTransform = new RotateTransform(-Angle);
            }

            private void Anim1_Completed(object sender, EventArgs e)
            {
                CurCanvas.Children.Remove(Rect);
                Place(false);
            }
        }
    }
    public class SectorImageShort:Viewbox
    {
        static List<SectorImageBase> Bases = GetBases();
        Canvas CurCanvas;
        Land Land;
        Avanpost Avanpost;
        byte Position;
        public LandSector Sector;
        public Path Ramka;
        public Path Ramka2;
        public int Z;
        List<FrameworkElement> CurElements = new List<FrameworkElement>();
        public SectorImageShort(Land land, byte pos, Point center, double basewidth, double baseheight, int sectorimageid)
        {
            Land = land;
            Position = pos;
            Sector = land.Locations[pos];
            Z = (int)(center.Y / 10);
            CurCanvas = new Canvas(); Child = CurCanvas; Canvas.SetZIndex(this, Z);
            SectorImageCreateResult createresult = null;
            if (Sector.GetSectorType() == LandSectorType.Clear)
                createresult = DrawClearBase2(center, basewidth, sectorimageid);
            else
                createresult = DrawBase(center, basewidth, sectorimageid);
            CreateRamka(createresult);
        }
        public SectorImageShort(Avanpost avanpost, byte pos, Point center, double basewidth, double baseheight, int sectorimageid)
        {
            Avanpost = avanpost;
            Position = pos;
            Z = (int)(center.Y / 10);
            CurCanvas = new Canvas(); Child = CurCanvas; Canvas.SetZIndex(this, Z);
            SectorImageCreateResult createresult = null;
            createresult = DrawFlashing(center, basewidth, baseheight, sectorimageid, 0.95);
           
            CreateRamka(createresult);
        }
        public void Click()
        {
            if (Land== null)
            {
                AvanpostPanel panel = new AvanpostPanel(Avanpost);
                Links.Controller.PopUpCanvas.Place(panel);
            }
            
            else if (Sector.GetSectorType() == LandSectorType.Clear)
            {
                SectorCreate panel = new SectorCreate(Sector.Land, Sector.Position);
                //NewSectorSelectPanel panel = new NewSectorSelectPanel(Sector.Land, Sector.Position);
                Links.Controller.PopUpCanvas.Place(panel);
            }
            else
            {
                CurSectorPanel panel = new CurSectorPanel(Land, Position);
                Links.Controller.PopUpCanvas.Place(panel);
            }
        }

        void CreateRamka(SectorImageCreateResult osnova)
        {
            Path path = new Path(); path.StrokeThickness = osnova.RamkaThickness/2;
            CombinedGeometry geom = new CombinedGeometry();
            geom.Geometry1 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                osnova.RamkaPath));
            foreach (string s in osnova.BuildingPaths)
            {
                geom.Geometry2 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(s));
                CombinedGeometry geom1 = new CombinedGeometry();
                geom1.Geometry1 = geom;
                geom = geom1;
            }
            path.Data = geom;
            path.Fill = Links.Brushes.Transparent;
            CurCanvas.Children.Add(path);
            Path path2 = new Path(); path2.StrokeThickness = osnova.RamkaThickness * 2;
            path2.Data = geom; CurCanvas.Children.Add(path2);
            Ramka = path; Canvas.SetZIndex(path, 20); 
            Ramka2 = path2; Canvas.SetZIndex(path2, 19); Ramka2.Opacity = 0.7;
            BlurEffect e = new BlurEffect();
            e.Radius = 30;
            Ramka2.Effect = e;
        }
        /// <summary> Создаётся мигающая площадка для аванпоста с указанным ID картинки </summary>
        SectorImageCreateResult DrawFlashing(Point center, double basewidth, double baseheight, int baseid, double flashdelta)
        {
            SectorImageBase curbase = Bases[baseid];
            Rectangle osnova = Common.GetRectangle(curbase.Size.Width, curbase.Size.Height, curbase.Image);
            CurCanvas.Width = curbase.Size.Width + curbase.Delta.X;
            CurCanvas.Height = curbase.Size.Height + curbase.Delta.Y;
            CurCanvas.Children.Add(osnova); Canvas.SetLeft(osnova, curbase.Delta.X); Canvas.SetTop(osnova, curbase.Delta.Y);
            CurElements.Add(osnova);
            double d = basewidth / 600;
            Width = CurCanvas.Width * d; Height = CurCanvas.Height * d;
            Canvas.SetLeft(this, center.X - Width + 300 * d); Canvas.SetTop(this, center.Y - 300 * d);

            double time = -2 * flashdelta + 2.5;
            double minval = 0.5 * flashdelta;
            double maxval = 0.8 * flashdelta + 0.2;
            DoubleAnimation anim = new DoubleAnimation(minval, maxval, TimeSpan.FromSeconds(time));
            anim.AutoReverse = true;
            anim.RepeatBehavior = RepeatBehavior.Forever;
            osnova.BeginAnimation(FrameworkElement.OpacityProperty, anim);

            Path path = new Path(); path.StrokeThickness = 5*basewidth/300; path.StrokeLineJoin = PenLineJoin.Round;
            int w = (int)(Math.Round(600.0 / 2, 0));
            int h = (int)Math.Round(600 / 2.0 / Math.Tan(60 / 180.0 * Math.PI), 0);
            int v = (int)Math.Round(baseheight, 0);
            path.Stroke = Brushes.Yellow;
            //CurCanvas.Width = 300; CurCanvas.Height = baseheight + h * 2;
            //Width = basewidth; Height = baseheight + 2 * Math.Round(basewidth / 2 / Math.Tan(60 / 180.0 * Math.PI));
            path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(String.Format(
                "M200,300 l{0},-{1} l{0},{1} l-{0},{1}z v{2} l{0},{1} l{0},-{1} v-{2} M{3},{4} v{2}",
                w, h, v, 200+ w, 300+ h)));
            CurCanvas.Children.Add(path);
            DoubleAnimation anim1 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(time));
            anim1.RepeatBehavior = RepeatBehavior.Forever;
            anim1.AutoReverse = true;
            path.BeginAnimation(FrameworkElement.OpacityProperty, anim1);
            //Canvas.SetLeft(this, center.X - basewidth / 2); Canvas.SetTop(this, center.Y - basewidth / 2 / Math.Tan(60 / 180.0 * Math.PI));

            SectorImageCreateResult result = new SectorImageCreateResult();
            result.RamkaThickness = 5 / d;
            result.RamkaPath = String.Format(
                "M200,300 l{0},-{1} l{0},{1} v{2} l-{0},{1} l-{0},-{1}z", w, h, v);
            return result;
        }
        /// <summary> Создаётся площадка с указанным ID картинки </summary>
        SectorImageCreateResult DrawBase(Point center, double basewidth, int baseid)
        {
            SectorImageBase curbase = Bases[baseid];
            Rectangle osnova = Common.GetRectangle(curbase.Size.Width, curbase.Size.Height, curbase.Image);
            CurCanvas.Width = curbase.Size.Width + curbase.Delta.X;
            CurCanvas.Height = curbase.Size.Height + curbase.Delta.Y;
            CurCanvas.Children.Add(osnova); Canvas.SetLeft(osnova, curbase.Delta.X); Canvas.SetTop(osnova, curbase.Delta.Y);
            CurElements.Add(osnova);
            double d = basewidth / 600;
            Width = CurCanvas.Width * d; Height = CurCanvas.Height * d;
            Canvas.SetLeft(this, center.X - Width + 300*d); Canvas.SetTop(this, center.Y - 300 * d);

            PeaceSector peacesector = (PeaceSector)Sector;
            List<Point> points = new List<Point>();
            points.Add(new Point(500, 386.5));
            points.Add(new Point(350,300));
            points.Add(new Point(650, 300));
            SectorImageCreateResult result = new SectorImageCreateResult();
            for (int i = 0; i < 3; i++)
            {
                GSBuilding2 building = peacesector.Buildings[i];
                int buildpixelwidth = ((BitmapSource)building.Image.ImageSource).PixelWidth;
                int buildpixelheight = ((BitmapSource)building.Image.ImageSource).PixelHeight;
                //double buildscale = imageworkwidth / 2 / buildpixelwidth;
                //double buildheight = buildpixelheight * buildscale;
                Rectangle rect = new Rectangle();
                rect.Fill = building.Image;
                rect.Width = 300; rect.Height = 300.0/buildpixelwidth*buildpixelheight;
                CurCanvas.Children.Add(rect);
                Canvas.SetLeft(rect, points[i].X - 300 / 2);
                //Canvas.SetTop(rect, points[i].Y);
                Canvas.SetTop(rect, points[i].Y - rect.Height+86.5);
                if (i == 0) Canvas.SetZIndex(rect, 10);
                result.BuildingPaths.Add(String.Format(building.BuildingPath, (int)points[i].X, (int)points[i].Y));
                CurElements.Add(rect); //rect.OpacityMask = new LinearGradientBrush();
            }
            for (int i=0;i<3;i++)
            {
                Canvas LevelCanvas = new Canvas(); LevelCanvas.Width = 100; LevelCanvas.Height = 100;
                Ellipse LevelEllipse = new Ellipse(); LevelEllipse.Width = 100; LevelEllipse.Height = 100;
                LevelCanvas.Children.Add(LevelEllipse);
                RadialGradientBrush LevelBrush = new RadialGradientBrush();
                LevelBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));
                LevelBrush.GradientStops.Add(new GradientStop(Links.Brushes.SkyBlue.Color, 0.9));
                LevelBrush.GradientStops.Add(new GradientStop(Links.Brushes.SkyBlue.Color, 0.8));
                LevelBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.7));
                LevelBrush.GradientStops.Add(new GradientStop(Links.Brushes.SkyBlue.Color, 0.65));
                LevelEllipse.Fill = LevelBrush;

                TextBlock LevelValue = Common.GetBlock(50, peacesector.Buildings[i].CurLevel.ToString(), Brushes.White, 100);
                LevelCanvas.Children.Add(LevelValue);
                Canvas.SetTop(LevelValue, 15);
                CurCanvas.Children.Add(LevelCanvas);
                Canvas.SetLeft(LevelCanvas, points[i].X - 50); Canvas.SetTop(LevelCanvas, points[i].Y + 100);
            }
            result.RamkaThickness = 2/d;
            result.RamkaPath = curbase.RamkaPath;
            return result;
            
        }
        public void Show(int pos)
        {
            if (CurElements.Count == 0) return;
            if (pos == -1)
            {
                foreach (FrameworkElement element in CurElements)
                    element.OpacityMask = new LinearGradientBrush();
                return;
            }
                FrameworkElement elt = CurElements[pos];
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0.5, 1); brush.EndPoint = new Point(0.5, 0);
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            GradientStop gr1, gr2;
            brush.GradientStops.Add(gr1 = new GradientStop(Colors.White, 0));
            brush.GradientStops.Add(gr2 = new GradientStop(Colors.Transparent, 0));
            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
            gr1.BeginAnimation(GradientStop.OffsetProperty, anim);
            gr2.BeginAnimation(GradientStop.OffsetProperty, anim);
            elt.OpacityMask = brush;
        }
        SectorImageCreateResult DrawClearBase2(Point center, double basewidth, int baseid)
        {
            SectorImageBase curbase = Bases[baseid];
            Rectangle osnova = Common.GetRectangle(curbase.Size.Width, curbase.Size.Height, curbase.Image);
            CurCanvas.Width = curbase.Size.Width + curbase.Delta.X;
            CurCanvas.Height = curbase.Size.Height + curbase.Delta.Y;
            CurCanvas.Children.Add(osnova); Canvas.SetLeft(osnova, curbase.Delta.X); Canvas.SetTop(osnova, curbase.Delta.Y);
            CurElements.Add(osnova);
            double d = basewidth / 600;
            Width = CurCanvas.Width * d; Height = CurCanvas.Height * d;
            Canvas.SetLeft(this, center.X - Width + 300 * d); Canvas.SetTop(this, center.Y - 300 * d);
            SectorImageCreateResult result = new SectorImageCreateResult();
            Rectangle rect = new Rectangle();
            rect.Fill = Gets.GetBuildingBrush("Clear");
            rect.Opacity = 0.7;
            rect.Width = 600*1.15; rect.Height = 346*1.15;
            CurCanvas.Children.Add(rect);
            Canvas.SetLeft(rect, 200-55);
            //Canvas.SetTop(rect, points[i].Y);
            Canvas.SetTop(rect, 300-173-35);
            CurElements.Add(rect); //rect.OpacityMask = new LinearGradientBrush();

            result.RamkaThickness = 2 / d;
            result.RamkaPath = curbase.RamkaPath;
            return result;
        }
        SectorImageCreateResult DrawClearBase(Point center, double basewidth, double baseheight)
        {
            Path path = new Path(); path.StrokeThickness = 5*basewidth/300.0; path.StrokeLineJoin = PenLineJoin.Round;
            int w = (int)(Math.Round(300.0 / 2, 0));
            int h = (int)Math.Round(300 / 2.0 / Math.Tan(60 / 180.0 * Math.PI));
            int v = (int)Math.Round(baseheight, 0);
            CurCanvas.Width = 300; CurCanvas.Height = baseheight + h * 2;
            Width = basewidth; Height = baseheight + 2*Math.Round(basewidth / 2 / Math.Tan(60 / 180.0 * Math.PI)); 
            path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(String.Format(
                "M0,{0} l{1},-{0} l{1},{0} l-{1},{0}z v{2} l{1},{0} l{1},-{0} l-{1},-{0} l-{1},{0} M{1},0 v{2} M{3},{0} v{2} M{1},{4} v{2}",
                h, w, v, 2 * w, 2 * h)));
            CurCanvas.Children.Add(path);
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0.5, 0); brush.EndPoint = new Point(0.5, 1.0);
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0));
            brush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.1));
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.2));
            brush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.3));
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.4));
            brush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.6));
            brush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.7));
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.8));
            brush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.9));
            brush.GradientStops.Add(new GradientStop(Colors.Black, 1.0));
            path.Stroke = brush;
            Canvas.SetLeft(this, center.X - basewidth/2); Canvas.SetTop(this, center.Y - basewidth/2/Math.Tan(60/180.0*Math.PI));
            SectorImageCreateResult result = new SectorImageCreateResult();
            result.RamkaPath = String.Format(
                "M0,{0} l{1},-{0} l{1},{0} v{2} l-{1},{0} l-{1},-{0}z", h, w, v);
            result.RamkaThickness = 2/basewidth*300;
            return result;
        }
        static List<SectorImageBase> GetBases()
        {
            List<SectorImageBase> result = new List<SectorImageBase>();
            result.Add(new SectorImageBase(0));
            result.Add(new SectorImageBase(1));
            result.Add(new SectorImageBase(2));
            return result;
        }
        class SectorImageCreateResult
        {
            public List<string> BuildingPaths=new List<string>(); //текст пути для зданий из которого формируется рамка
            public string RamkaPath; //текст пути, из которого формируется рамка
            public double RamkaThickness; //толщина пути, считается при генерации основы
        }
        class SectorImageBase
        {
            public ImageBrush Image;//ссылка на изображение
            public Size Size;  //размер изображения, в котором площадка вписывается в параллелограмм M200,300 l300,-173 l300,173 l-300,173z
            public Point Delta; //смещение изображения, в котором площадка вписывается в указанный выше параллелограмм
            public string RamkaPath; //текст пути, из которого формируется рамка
            public SectorImageBase (int id)
            {
                Fill(id);
            }
           void Fill(int ID)
            {
                switch (ID)
                {
                    case 0: //пустой сектор, заполняется при создании основа
                        break;
                    case 1: //сектор с парящей площадкой
                        Image = new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Colony2/{0}.png", "kub1"), UriKind.Relative)));
                        Size = new Size(890, 1050);
                        Delta = new Point(62, 26);
                        RamkaPath = "M200,300 l300,-173 l300,173 v600 l-300,150 l-300,-150z";
                        break;
                    case 2: //тестовый сектор
                        Image = new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Colony2/{0}.png", "kub2"), UriKind.Relative)));
                        Size = new Size(695, 950);
                        Delta = new Point(133, 100);
                        RamkaPath = "M200,300 l300,-173 l300,173 v600 l-300,150 l-300,-150z";
                        break;

                }
            }
        }
    }
    public class CurSectorPanel:Viewbox
    {
        enum PanelMode { Build, Info, Artefact, NewFleet, CurFleet, None}
        public int Position;
        Canvas CurCanvas;
        public LandSector Sector;
        int ShowStep = 0;
        SectorImageShort SectorImage;
        PanelMode CurMode = PanelMode.None;
        BuildingsPanel CurBuildingsPanel;
        ArtefactPanel CurArtefactPanel;
        SmallButton InfoBtn, BuildBtn, ArtefactBtn;
        System.Windows.Threading.DispatcherTimer Timer;
        public CurSectorPanel(Land land, byte pos)
        {
            Width = 1362; Height = 702; Sector = land.Locations[pos];
            CurCanvas = new Canvas(); Child = CurCanvas;
            CurCanvas.Width = 1815; CurCanvas.Height = 936;
            CurCanvas.Background = Gets.AddPicColony2("background");
            Rectangle Close = Common.GetRectangle(53, Gets.AddPicColony2("x"));
            CurCanvas.Children.Add(Close); Canvas.SetLeft(Close, 1762); Close.PreviewMouseDown += Close_PreviewMouseDown;

            InfoBtn = new SmallButton(Gets.AddPicColony2("lup"), PanelMode.Info);
            CurCanvas.Children.Add(InfoBtn); Canvas.SetLeft(InfoBtn, 30); Canvas.SetTop(InfoBtn, 200);
            InfoBtn.PreviewMouseDown += SmallButtonDown;

            BuildBtn = new SmallButton(Gets.AddPicColony2("dex"), PanelMode.Build);
            CurCanvas.Children.Add(BuildBtn); Canvas.SetLeft(BuildBtn, 30); Canvas.SetTop(BuildBtn, 400);
            BuildBtn.PreviewMouseDown += SmallButtonDown;

            ArtefactBtn = new SmallButton(Gets.AddPicColony2("apgrade"), PanelMode.Artefact);
            CurCanvas.Children.Add(ArtefactBtn); Canvas.SetLeft(ArtefactBtn, 30); Canvas.SetTop(ArtefactBtn, 600);
            ArtefactBtn.PreviewMouseDown += SmallButtonDown;

            Rectangle border = Common.GetRectangle(500, 732, Gets.AddPicColony2("border"));
            CurCanvas.Children.Add(border); Canvas.SetLeft(border, 200); Canvas.SetTop(border, 150);

            LandBackground curbackground = LandBackground.List[land.Planet.LandBackgroundID];
            SectorImageShort img = new SectorImageShort(land, pos, new Point(450, 350), 300, 100, curbackground.SectorImageID);
            img.Show(-1);
            CurCanvas.Children.Add(img); SectorImage = img;

            PeaceSector peace = (PeaceSector)land.Locations[pos];
            CurBuildingsPanel = new BuildingsPanel(peace);
            CurCanvas.Children.Add(CurBuildingsPanel);
            CurBuildingsPanel.Visibility = Visibility.Hidden;
            
            CurArtefactPanel = new ArtefactPanel();
            CurCanvas.Children.Add(CurArtefactPanel);
            CurArtefactPanel.Visibility = Visibility.Hidden;

            TextBlock SectorName = Common.GetBlock(40, SectorImages.List[peace.Type].ShortTitle, Links.Brushes.SkyBlue, 500);
            CurCanvas.Children.Add(SectorName); Canvas.SetLeft(SectorName, 400); Canvas.SetTop(SectorName, 55);
            SectorName.TextAlignment = TextAlignment.Left;

            if (peace.Type==SectorTypes.War)
            {
                FleetBase fb = (FleetBase)peace;
                SmallButton fleetbtn;
                if (fb.Fleet==null)
                    fleetbtn = new SmallButton(Gets.AddPicColony2("ship"), PanelMode.NewFleet);
                else
                {
                    FleetEmblem fe = new FleetEmblem(fb.Fleet.Image);
                    VisualBrush vb = new VisualBrush(fe);
                    fleetbtn = new SmallButton(vb, PanelMode.CurFleet);
                }
                CurCanvas.Children.Add(fleetbtn);
                Canvas.SetLeft(fleetbtn, 30);
                Canvas.SetTop(fleetbtn, 800);
                fleetbtn.PreviewMouseDown += SmallButtonDown;
            }

            Timer = new System.Windows.Threading.DispatcherTimer();
            Timer.Tick += Timer_Tick;
            Timer.Interval = TimeSpan.FromSeconds(0.3);
            Timer.Start();
            this.PreviewMouseDown += CurSectorPanel_PreviewMouseDown;
        }


        private void CurSectorPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Timer.IsEnabled)
            {
                Timer.Interval = TimeSpan.Zero;
            }
            CurBuildingsPanel.QuickEnd();
        }

        private void SmallButtonDown(object sender, MouseButtonEventArgs e)
        {
            SmallButton button = (SmallButton)sender;
            SelectPanel(button.CurMode);
        }
        void SelectPanel(PanelMode nextpanel)
        {
            if (CurMode == nextpanel) return;
            CurMode = nextpanel;
            switch (CurMode)
            {
                case PanelMode.Build:
                    BuildBtn.Select(); InfoBtn.DeSelect(); ArtefactBtn.DeSelect();
                    CurBuildingsPanel.Visibility = Visibility.Visible;
                    //CurInfoPanel.Visibility = Visibility.Hidden;
                    CurArtefactPanel.Visibility = Visibility.Hidden;
                    CurBuildingsPanel.SetBuildInfo();
                    break;
                case PanelMode.Info:
                    BuildBtn.DeSelect(); InfoBtn.Select(); ArtefactBtn.DeSelect();
                    CurBuildingsPanel.Visibility = Visibility.Visible;
                    //CurInfoPanel.Visibility = Visibility.Visible;
                    CurArtefactPanel.Visibility = Visibility.Hidden;
                    CurBuildingsPanel.SetStatInfo();
                    break;
                case PanelMode.Artefact:
                    BuildBtn.DeSelect(); InfoBtn.DeSelect(); ArtefactBtn.Select();
                    CurBuildingsPanel.Visibility = Visibility.Hidden;
                    //CurInfoPanel.Visibility = Visibility.Hidden;
                    CurArtefactPanel.Visibility = Visibility.Visible;
                    break;
                case PanelMode.NewFleet:
                    FleetBase fb = (FleetBase)Sector;
                    CreateFleetPanel createfleetpanel = new CreateFleetPanel(fb);
                    Links.Controller.PopUpCanvas.Change(createfleetpanel, "Создание нового флота");
                    break;
                case PanelMode.CurFleet:
                    FleetBase fb1 = (FleetBase)Sector;
                    Links.Helper.Fleet = fb1.Fleet;
                    Links.Controller.PopUpCanvas.Remove();
                    Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.SpecialFleet);
                    break;


            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            
            if (ShowStep < 4)
            {
                SectorImage.Show(ShowStep);
                ShowStep++;
            }
            else
            {
                if (CurMode==PanelMode.None)
                    SelectPanel(PanelMode.Build);
                CurBuildingsPanel.Show();
                ((System.Windows.Threading.DispatcherTimer)sender).Stop();
                /*if (CurMode==PanelMode.Build)
                {
                    BuildBtn.Select(); InfoBtn.DeSelect(); ArtefactBtn.DeSelect();
                    CurBuildingsPanel.Visibility = Visibility.Visible;
                    CurBuildingsPanel.Show();
                    ((System.Windows.Threading.DispatcherTimer)sender).Stop();
                }*/
            }

            
        }
      
        class ArtefactPanel:Canvas
        {

        }
        class BuildingsPanel:Canvas
        {
            List<OneBuildingPanel> BuildingPanels = new List<OneBuildingPanel>();
            System.Windows.Threading.DispatcherTimer Timer;
            public BuildingsPanel(PeaceSector sector)
            {
                Canvas.SetLeft(this, 690); Canvas.SetTop(this, 100);
                PeaceSector peace = (PeaceSector)sector;
                for (int i = 0; i < 3; i++)
                {
                    OneBuildingPanel panel = new OneBuildingPanel(peace.Buildings[i], this, i*360, 0);
                    panel.SetBuildInfo();
                    Children.Add(panel);
                    Canvas.SetLeft(panel,i * 360); 
                    BuildingPanels.Add(panel);

                }
                Timer = new System.Windows.Threading.DispatcherTimer();
                Timer.Interval = TimeSpan.FromSeconds(0.3);
                Timer.Tick += BuildingsPanelTick;
            }
            public void SetBuildInfo()
            {
                foreach (OneBuildingPanel panel in BuildingPanels)
                    panel.SetBuildInfo();
            }
            public void SetStatInfo()
            {
                foreach (OneBuildingPanel panel in BuildingPanels)
                    panel.SetStatInfo();
            }
            int curanim = 0;
            public void Show()
            {
                foreach (OneBuildingPanel panel in BuildingPanels)
                    panel.Start();
                curanim = 0;
                Timer.Stop();
                Timer.Start();
                BuildingsPanelTick(null, null);
            }
            public void QuickEnd()
            {
                //Timer.Stop();
                //for (int i = curanim; i < 3; i++)
                //    BuildingPanels[curanim].Show();
            }
            private void BuildingsPanelTick(object sender, EventArgs e)
            {
                if (curanim < 3)
                    BuildingPanels[curanim].Show();
                else
                    Timer.Stop();
                curanim++;
            }
        }
        class OneBuildingPanel:Viewbox
        {
            Canvas CurCanvas;
            GSBuilding2 Building;
            ScaleTransform ButtonScale;
            ScaleTransform ImageScale;
            Rectangle Image;
            TextBlock MoneyPrice;
            RotateTransform MoneyTransform;
            TextBlock MetalPrice;
            RotateTransform MetallTransform;
            TextBlock ChipsPrice;
            RotateTransform ChipsTransform;
            TextBlock AntiPrice;
            RotateTransform AntiTransform;
            Rectangle MoneyBacks;
            System.Windows.Threading.DispatcherTimer Timer;
            TextBlock BuildText;
            PanelMode PanelMode = PanelMode.None;
            TextBlock NameBlock;
            TextBlock SizeText;
            RotateTransform SizeTransform;
            TextBlock Parameter1Text;
            Rectangle SizeInfo;
            ParameterInfoCanvas SizeCanvas;
            Rectangle Parameter1Info;
            ParameterInfoCanvas Info1Canvas;
            Rectangle Parameter2Info;
            TextBlock Parameter2Text;
            ParameterInfoCanvas Info2Canvas;
            Canvas TopCanvas; int Left; int Top;
            StringAnimationUsingKeyFrames NullAnim;
            public OneBuildingPanel(GSBuilding2 building, Canvas topCanvas, int left, int top)
            {
                Building = building; TopCanvas = topCanvas; Left = left; Top = top;
                Width = 390; Height = 862;
                CurCanvas = new Canvas(); Child = CurCanvas;
                CurCanvas.Width = 390; CurCanvas.Height = 862;
                Rectangle back = Common.GetRectangle(340, 800, Gets.AddPicColony2("knop1"));
                CurCanvas.Children.Add(back); Canvas.SetLeft(back, 14); Canvas.SetTop(back, 50);

                MoneyBacks = Common.GetRectangle(340, 800, Gets.AddPicColony2("knop2"));
                CurCanvas.Children.Add(MoneyBacks); Canvas.SetLeft(MoneyBacks, 14); Canvas.SetTop(MoneyBacks, 50);
                MoneyBacks.Visibility = Visibility.Hidden;

                Canvas ButtonCanvas = new Canvas(); ButtonCanvas.Width = 300; ButtonCanvas.Height = 150;
                CurCanvas.Children.Add(ButtonCanvas); Canvas.SetLeft(ButtonCanvas, 75);
                ButtonCanvas.RenderTransformOrigin = new Point(0.9, 0.3);

                Rectangle buildbutton = Common.GetRectangle(300, 150, Gets.AddPicColony2("ramka"));
                ButtonCanvas.Children.Add(buildbutton);
                BuildText = Common.GetBlock(36, "Построить", Brushes.White, 150);
                BuildText.TextAlignment = TextAlignment.Left;
                ButtonCanvas.Children.Add(BuildText); Canvas.SetLeft(BuildText, 40);

                Rectangle fon = Common.GetRectangle(380, 380, Gets.AddPicColony2("fon"));
                CurCanvas.Children.Add(fon); Canvas.SetTop(fon, 30);

                int pixelwidth = ((BitmapImage)building.ImageA.ImageSource).PixelWidth;
                int pixelheight = ((BitmapImage)building.ImageA.ImageSource).PixelHeight;

                if (pixelwidth > pixelheight)
                {
                    double h = pixelheight * 280.0 / pixelwidth;
                    Image = Common.GetRectangle(280, h, building.ImageA);
                    Canvas.SetLeft(Image, 40); Canvas.SetTop(Image, 40 + 300 - h);
                }
                else
                {
                    double w = pixelwidth * 280.0 / pixelheight;
                    Image = Common.GetRectangle(w, 280, building.ImageA);
                    Canvas.SetLeft(Image, 190-w/2); Canvas.SetTop(Image, 70);
                }
                Image.RenderTransformOrigin = new Point(0.5, 0.5);
                Image.RenderTransform = ImageScale = new ScaleTransform(0, 1);
                CurCanvas.Children.Add(Image);

                Grid NameGrid = new Grid(); NameGrid.Width = 320; NameGrid.Height = 140; //NameGrid.Background = Brushes.Green;
                CurCanvas.Children.Add(NameGrid); Canvas.SetLeft(NameGrid, 20); Canvas.SetTop(NameGrid, 360);
                NameBlock = Common.GetBlock(36, "", Links.Brushes.SkyBlue, 320);
                NameGrid.Children.Add(NameBlock); NameBlock.VerticalAlignment = VerticalAlignment.Center; //Canvas.SetLeft(NameBlock, 20); Canvas.SetTop(NameBlock, 400);

                MoneyPrice = Common.GetBlock(36, "", Brushes.White, 200); MoneyPrice.TextAlignment = TextAlignment.Left;
                CurCanvas.Children.Add(MoneyPrice); Canvas.SetLeft(MoneyPrice, 80); Canvas.SetTop(MoneyPrice, 629);
                MoneyPrice.RenderTransformOrigin = new Point(0.5, 0.5);
                MoneyPrice.RenderTransform = MoneyTransform = new RotateTransform();

                MetalPrice = Common.GetBlock(36, "", Brushes.White, 200); MetalPrice.TextAlignment = TextAlignment.Left;
                CurCanvas.Children.Add(MetalPrice); Canvas.SetLeft(MetalPrice, 80); Canvas.SetTop(MetalPrice, 682);
                MetalPrice.RenderTransformOrigin = new Point(0.5, 0.5);
                MetalPrice.RenderTransform = MetallTransform = new RotateTransform();

                ChipsPrice = Common.GetBlock(36, "", Brushes.White, 200); ChipsPrice.TextAlignment = TextAlignment.Left;
                CurCanvas.Children.Add(ChipsPrice); Canvas.SetLeft(ChipsPrice, 80); Canvas.SetTop(ChipsPrice, 736);
                ChipsPrice.RenderTransformOrigin = new Point(0.5, 0.5);
                ChipsPrice.RenderTransform = ChipsTransform = new RotateTransform();

                AntiPrice = Common.GetBlock(36, "", Brushes.White, 200); AntiPrice.TextAlignment = TextAlignment.Left;
                CurCanvas.Children.Add(AntiPrice); Canvas.SetLeft(AntiPrice, 80); Canvas.SetTop(AntiPrice, 789);
                AntiPrice.RenderTransformOrigin = new Point(0.5, 0.5);
                AntiPrice.RenderTransform = AntiTransform = new RotateTransform();

                ButtonCanvas.RenderTransform = ButtonScale = new ScaleTransform();

                SizeInfo = Common.GetRectangle(60, Links.Brushes.PeopleImageBrush);
                CurCanvas.Children.Add(SizeInfo); Canvas.SetLeft(SizeInfo, 40); Canvas.SetTop(SizeInfo, 480);

                SizeInfo.MouseEnter += ParameterInfo_MouseEnter;

                SizeText = Common.GetBlock(50, "", Brushes.White, 220);
                CurCanvas.Children.Add(SizeText); Canvas.SetLeft(SizeText, 110); Canvas.SetTop(SizeText, 480);
                SizeText.RenderTransformOrigin = new Point(0.5, 0.5);
                SizeText.RenderTransform = SizeTransform = new RotateTransform();

                Parameter1Info = Common.GetRectangle(60, Links.Brushes.BuildingSizeBrush);
                CurCanvas.Children.Add(Parameter1Info); Canvas.SetLeft(Parameter1Info, 40); Canvas.SetTop(Parameter1Info, 555);

                Parameter1Info.MouseEnter += ParameterInfo_MouseEnter;

                Parameter1Text = Common.GetBlock(50, "", Brushes.White, 220);
                CurCanvas.Children.Add(Parameter1Text); Canvas.SetLeft(Parameter1Text, 110); Canvas.SetTop(Parameter1Text, 555);

                Parameter2Info = Common.GetRectangle(40, Links.Brushes.LandIconBrush);
                Parameter2Info.Visibility = Visibility.Hidden;
                CurCanvas.Children.Add(Parameter2Info); Canvas.SetLeft(Parameter2Info, 40); Canvas.SetTop(Parameter2Info, 630);

                Parameter2Text = Common.GetBlock(50, "EEE", Brushes.White, 220);
                Parameter2Text.Visibility = Visibility.Hidden;
                CurCanvas.Children.Add(Parameter2Text); Canvas.SetLeft(Parameter2Text, 110); Canvas.SetTop(Parameter2Text, 630);

                Parameter2Info.MouseEnter += ParameterInfo_MouseEnter;
                
                MouseEnter += OneBuildingPanel_MouseEnter;
                MouseLeave += OneBuildingPanel_MouseLeave;
                PreviewMouseDown += OneBuildingPanel_PreviewMouseDown;
                Timer = new System.Windows.Threading.DispatcherTimer();
                Timer.Interval = TimeSpan.FromSeconds(0.2);
                Timer.Tick += CreateFormTick;
                Start();
                NullAnim = new StringAnimationUsingKeyFrames();
                NullAnim.Duration = TimeSpan.Zero;
                NullAnim.KeyFrames.Add(new DiscreteStringKeyFrame("", KeyTime.FromPercent(1)));
            }

            private void OneBuildingPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            {
                if (PanelMode==PanelMode.Build)
                {
                    if (Building.CurLevel>=10)
                    {
                        return;
                    }
                    bool haserror = false;
                    ItemPrice nextprice = Building.GetNextPrice();
                        if (GSGameInfo.Money<nextprice.Money) { MakeShake(MoneyTransform); haserror = true; }
                    if (GSGameInfo.Metals < nextprice.Metall) { MakeShake(MetallTransform); haserror = true; }
                    if (GSGameInfo.Chips < nextprice.Chips) { MakeShake(ChipsTransform); haserror = true; }
                    if (GSGameInfo.Anti < nextprice.Anti) { MakeShake(AntiTransform); haserror = true; }
                    Land land = GSGameInfo.PlayerLands[Building.LandID];                                                      
                    if ((land.Peoples-land.BuildingsCount)<Building.GetNextSize())
                    { MakeShake(SizeTransform); haserror = true; }
                    if (haserror) return;
                    LandInfo landinfo = new LandInfo(land);
                    string eventresult = Events.BuildNewBuilding(land.ID, Building.SectorPos, Building.BuildingPos);
                    if (eventresult=="")
                    {
                        Gets.GetResources();
                        Gets.GetTotalInfo("Строительство здания");
                        Land newland = GSGameInfo.PlayerLands[Building.LandID];
                        Building.CurLevel++;
                        landinfo.Compare(newland, Building);
                        Links.Controller.PopUpCanvas.Change(landinfo, "строительство здания");
                        Colonies.InnerRefresh();
                    }
                }
            }
            void MakeShake(RotateTransform transform)
            {
                DoubleAnimationUsingKeyFrames anim = new DoubleAnimationUsingKeyFrames();
                anim.Duration = TimeSpan.FromSeconds(0.5);
                anim.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0)));
                anim.KeyFrames.Add(new LinearDoubleKeyFrame(-30, KeyTime.FromPercent(0.1)));
                anim.KeyFrames.Add(new LinearDoubleKeyFrame(25, KeyTime.FromPercent(0.3)));
                anim.KeyFrames.Add(new LinearDoubleKeyFrame(-20, KeyTime.FromPercent(0.5)));
                anim.KeyFrames.Add(new LinearDoubleKeyFrame(15, KeyTime.FromPercent(0.7)));
                anim.KeyFrames.Add(new LinearDoubleKeyFrame(-10, KeyTime.FromPercent(0.9)));
                anim.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(1.0)));
                transform.BeginAnimation(RotateTransform.AngleProperty, anim);
            }
            public void SetBuildInfo()
            {
                if (PanelMode == PanelMode.Info)
                {
                    Timer.Stop();
                    animstep = 0;
                    Timer.Tick -= SetInfoTick;
                    Timer.Tick += SetBuildTick;
                    Timer.Start();
                }
                else if (PanelMode == PanelMode.Build)
                    return;
                PanelMode = PanelMode.Build;
                MoneyBacks.Visibility = Visibility.Visible;
                BuildText.Text = "Построить";
                MoneyPrice.Visibility = Visibility.Visible; MetalPrice.Visibility = Visibility.Visible;
                ChipsPrice.Visibility = Visibility.Visible; AntiPrice.Visibility = Visibility.Visible;
                PanelMode = PanelMode.Build;
                MoneyPrice.Text = "";
                MetalPrice.Text = "";
                ChipsPrice.Text = "";
                AntiPrice.Text = "";
                //SizeText.Text = Building.GetNextSize().ToString();
                //Parameter1Text.Text = Building.GetNextValue().ToString();
                if (SizeCanvas != null && TopCanvas.Children.Contains(SizeCanvas))
                    TopCanvas.Children.Remove(SizeCanvas);
                SizeCanvas = new ParameterInfoCanvas("Требуемое свободное население");
                TopCanvas.Children.Add(SizeCanvas); Canvas.SetLeft(SizeCanvas, Left + 60); Canvas.SetTop(SizeCanvas, Top + 400);
                SizeInfo.Tag = SizeCanvas;
                if (Info1Canvas != null && TopCanvas.Children.Contains(Info1Canvas))
                    TopCanvas.Children.Remove(Info1Canvas);
                Info1Canvas = new ParameterInfoCanvas(Building.GetTagText(TagState.NextBuildng));
                TopCanvas.Children.Add(Info1Canvas); Canvas.SetLeft(Info1Canvas, Left + 60); Canvas.SetTop(Info1Canvas, Top + 555);
                Parameter1Info.Tag = Info1Canvas;
                Parameter2Info.Visibility = Visibility.Hidden;
                Parameter2Text.Visibility = Visibility.Hidden;
                SizeText.BeginAnimation(TextBlock.TextProperty, NullAnim);
                Parameter1Text.BeginAnimation(TextBlock.TextProperty, NullAnim);
                SetBuildTick(null, null);
            }

            public void SetStatInfo()
            {
                if (PanelMode == PanelMode.Build)
                {
                    Timer.Stop();
                    animstep = 0;
                    Timer.Tick -= SetBuildTick;
                    Timer.Tick += SetInfoTick;
                    Timer.Start();
                }
                else if (PanelMode == PanelMode.Info)
                    return;
                MoneyBacks.Visibility = Visibility.Hidden;
                BuildText.Text = "Информация";
                MoneyPrice.Visibility = Visibility.Hidden; MetalPrice.Visibility = Visibility.Hidden;
                ChipsPrice.Visibility = Visibility.Hidden; AntiPrice.Visibility = Visibility.Hidden;
                PanelMode = PanelMode.Info;
                if (SizeCanvas != null && TopCanvas.Children.Contains(SizeCanvas))
                    TopCanvas.Children.Remove(SizeCanvas);
                SizeCanvas = new ParameterInfoCanvas("Используемое население");
                TopCanvas.Children.Add(SizeCanvas); Canvas.SetLeft(SizeCanvas, Left + 90); Canvas.SetTop(SizeCanvas, Top + 400);
                SizeInfo.Tag = SizeCanvas;
                if (Info1Canvas != null && TopCanvas.Children.Contains(Info1Canvas))
                    TopCanvas.Children.Remove(Info1Canvas);
                Info1Canvas = new ParameterInfoCanvas(Building.GetTagText(TagState.FullBuilding));
                TopCanvas.Children.Add(Info1Canvas); Canvas.SetLeft(Info1Canvas, Left + 90); Canvas.SetTop(Info1Canvas, Top + 555);
                Parameter1Info.Tag = Info1Canvas;
                Parameter2Info.Visibility = Visibility.Visible;
                if (Info2Canvas != null && TopCanvas.Children.Contains(Info2Canvas))
                    TopCanvas.Children.Remove(Info2Canvas);
                Info2Canvas = new ParameterInfoCanvas(Building.GetTagText(TagState.ResultInfo));
                TopCanvas.Children.Add(Info2Canvas); Canvas.SetLeft(Info2Canvas, Left + 90); Canvas.SetTop(Info2Canvas, Top + 700);
                Parameter2Info.Tag = Info2Canvas;
                Parameter2Text.Visibility = Visibility.Visible;
                SizeText.BeginAnimation(TextBlock.TextProperty, NullAnim);
                Parameter1Text.BeginAnimation(TextBlock.TextProperty, NullAnim);
                Parameter2Text.BeginAnimation(TextBlock.TextProperty, NullAnim);
                SetInfoTick(null, null);
            }
            private void ParameterInfo_MouseEnter(object sender, MouseEventArgs e)
            {
                FrameworkElement element = (FrameworkElement)sender;
                ParameterInfoCanvas canvas = (ParameterInfoCanvas)element.Tag;
                canvas.Show();
            }

            int animstep = 0;
            public void Start()
            {
                OpacityMask = Brushes.Transparent;
                ImageScale.BeginAnimation(ScaleTransform.ScaleXProperty, new DoubleAnimation(0, 0, TimeSpan.Zero));
                
                Timer.Stop();
                animstep = 0;
            }
            public void Show()
            {
                Timer.Start();
                CreateFormTick(null, null);
            }
            private void CreateFormTick(object sender, EventArgs e)
            {
                switch (animstep)
                {
                    case 0:
                        LinearGradientBrush brush = new LinearGradientBrush();
                        brush.StartPoint = new Point(0.5, 0); brush.EndPoint = new Point(0.5, 1);
                        GradientStop gr1, gr2;
                        brush.GradientStops.Add(new GradientStop(Colors.White, 0));
                        brush.GradientStops.Add(gr2 = new GradientStop(Colors.White, 0));
                        brush.GradientStops.Add(gr1 = new GradientStop(Colors.Transparent, 0));
                        DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
                        gr1.BeginAnimation(GradientStop.OffsetProperty, anim);
                        gr2.BeginAnimation(GradientStop.OffsetProperty, anim);
                        OpacityMask = brush;
                        break;
                    case 1:
                        ImageScale.BeginAnimation(ScaleTransform.ScaleXProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.2)));
                        break;
                    case 2:
                        SetTextAnim(NameBlock, Building.GetName());
                        break;
                    case 3:
                        if (PanelMode == PanelMode.None)
                            Timer.Stop();
                        else if (PanelMode == PanelMode.Build)
                        {
                            Timer.Tick -= CreateFormTick;
                            animstep = 0;
                            Timer.Tick += SetBuildTick;
                            SetBuildTick(null, null);
                        }
                        else if (PanelMode==PanelMode.Info)
                        {
                            Timer.Tick -= CreateFormTick;
                            animstep = 0;
                            Timer.Tick += SetInfoTick;
                            SetInfoTick(null, null);
                        }
                        return;
                }
                animstep++;
            }
            private void SetBuildTick(object sender, EventArgs e)
            {
                switch (animstep)
                {
                    case 0:
                        int nextsize = Building.GetNextSize();
                        Brush brush = Brushes.White;
                        Land land = GSGameInfo.PlayerLands[Building.LandID];
                        if (land.Peoples - land.BuildingsCount < nextsize)
                            brush = Brushes.Red;
                        SetAnim(SizeText, Building.GetNextSize(), "млн. чел", brush);
                        break;
                    case 1:
                        SetAnim(Parameter1Text, Building.GetNextValue(), Building.Items, Brushes.White);
                        break;
                    case 2:
                        if (Building.CurLevel == 10)
                        {
                            MoneyPrice.Text = "Максимум"; MetalPrice.Text = "Максимум"; ChipsPrice.Text = "Максимум"; AntiPrice.Text = "Максимум";
                        }
                        else
                        {
                            ItemPrice price = Building.GetNextPrice();
                            SetAnim(MoneyPrice, price.Money, "", GSGameInfo.Money < price.Money ? Brushes.Red : Brushes.White);
                            SetAnim(MetalPrice, price.Metall,"", GSGameInfo.Metals < price.Metall ? Brushes.Red : Brushes.White);
                            SetAnim(ChipsPrice, price.Chips,"", GSGameInfo.Chips < price.Chips ? Brushes.Red : Brushes.White);
                            SetAnim(AntiPrice, price.Anti,"", GSGameInfo.Anti < price.Anti ? Brushes.Red : Brushes.White);
                        }
                        break;
                    case 3:
                        Timer.Stop();
                        break;
                }
                animstep++;
            }
            private void SetInfoTick(object sender, EventArgs e)
            {
                switch (animstep)
                {
                    case 0:
                        SetAnim(SizeText, Building.GetTotalSize(), "млн. чел", Brushes.White);
                        break;
                    case 1:
                        SetAnim(Parameter1Text, Building.GetTotalValue(), Building.Items, Brushes.White);
                        break;
                    case 2:
                        SetAnim(Parameter2Text, Building.GetFullResultValue(GSGameInfo.PlayerLands[Building.LandID]), Building.Items, Brushes.White);
                        break;
                    case 3:
                        Timer.Stop();
                        break;
                }
                animstep++;
                
            }
            void SetTextAnim(TextBlock block, string text)
            {
                StringAnimationUsingKeyFrames anim = new StringAnimationUsingKeyFrames();
                anim.Duration = TimeSpan.FromSeconds(0.05*text.Length);
                for (int i = 0; i < text.Length+1; i++)
                    anim.KeyFrames.Add(new DiscreteStringKeyFrame(text.Substring(0, i), KeyTime.FromPercent(((double)i) / text.Length)));
                block.BeginAnimation(TextBlock.TextProperty, anim);
            }
            void SetAnim(TextBlock block, double value, string items, Brush brush)
            {
                block.Foreground = brush;
                StringAnimationUsingKeyFrames moneyanim = new StringAnimationUsingKeyFrames();
                moneyanim.Duration = TimeSpan.FromSeconds(0.5);
                moneyanim.KeyFrames.Add(new DiscreteStringKeyFrame((value * 0.3).ToString("### ### ### ##0.# ") + items.ToString(), KeyTime.FromPercent(0.17)));
                moneyanim.KeyFrames.Add(new DiscreteStringKeyFrame((value * 0.6).ToString("### ### ### ##0.# ") + items.ToString(), KeyTime.FromPercent(0.34)));
                moneyanim.KeyFrames.Add(new DiscreteStringKeyFrame((value * 0.9).ToString("### ### ### ##0.# ") + items.ToString(), KeyTime.FromPercent(0.51)));
                moneyanim.KeyFrames.Add(new DiscreteStringKeyFrame((value * 0.95).ToString("### ### ### ##0.# ") + items.ToString(), KeyTime.FromPercent(0.68)));
                moneyanim.KeyFrames.Add(new DiscreteStringKeyFrame((value * 0.98).ToString("### ### ### ##0.# ") + items.ToString(), KeyTime.FromPercent(0.85)));
                moneyanim.KeyFrames.Add(new DiscreteStringKeyFrame((value * 1.0).ToString("### ### ### ##0.# ") + items.ToString(), KeyTime.FromPercent(1.0)));
                block.BeginAnimation(TextBlock.TextProperty, moneyanim);
            }
            void SetAnim(TextBlock block, int value, string items, Brush brush)
            {
                block.Foreground = brush;
                StringAnimationUsingKeyFrames moneyanim = new StringAnimationUsingKeyFrames();
                moneyanim.Duration = TimeSpan.FromSeconds(0.5);
                moneyanim.KeyFrames.Add(new DiscreteStringKeyFrame((value * 0.3).ToString("### ### ### ### ")+items.ToString(), KeyTime.FromPercent(0.17)));
                moneyanim.KeyFrames.Add(new DiscreteStringKeyFrame((value * 0.6).ToString("### ### ### ### ") + items.ToString(), KeyTime.FromPercent(0.34)));
                moneyanim.KeyFrames.Add(new DiscreteStringKeyFrame((value * 0.9).ToString("### ### ### ### ") + items.ToString(), KeyTime.FromPercent(0.51)));
                moneyanim.KeyFrames.Add(new DiscreteStringKeyFrame((value * 0.95).ToString("### ### ### ### ") + items.ToString(), KeyTime.FromPercent(0.68)));
                moneyanim.KeyFrames.Add(new DiscreteStringKeyFrame((value * 0.98).ToString("### ### ### ### ") + items.ToString(), KeyTime.FromPercent(0.85)));
                moneyanim.KeyFrames.Add(new DiscreteStringKeyFrame((value * 1.0).ToString("### ### ### ### ") + items.ToString(), KeyTime.FromPercent(1.0)));
                block.BeginAnimation(TextBlock.TextProperty, moneyanim);
            }
            private void OneBuildingPanel_MouseLeave(object sender, MouseEventArgs e)
            {
                if (PanelMode != PanelMode.Build) return;
                ButtonScale.ScaleX = 1; ButtonScale.ScaleY = 1;
            }

            private void OneBuildingPanel_MouseEnter(object sender, MouseEventArgs e)
            {
                if (PanelMode != PanelMode.Build) return;
                ButtonScale.ScaleX = 0.8; ButtonScale.ScaleY = 0.8;
            }
            class ParameterInfoCanvas:Grid
            {
                bool Showed = false;
                public ParameterInfoCanvas(string text)
                {
                    Canvas.SetZIndex(this, 20);
                    Background = Gets.AddPicColony2("border");
                    Visibility = Visibility.Hidden;
                    TextBlock tb = Common.GetBlock(36, text, Links.Brushes.SkyBlue, 300);
                    Children.Add(tb);
                    tb.Margin = new Thickness(25);
                }
                public void Show()
                {
                    if (Showed == true) return;
                    Showed = true;
                    Visibility = Visibility.Visible;
                    DoubleAnimation anim = new DoubleAnimation(0, 20, TimeSpan.FromSeconds(2.5));
                    anim.AutoReverse = true;
                    anim.Completed += Anim_Completed;
                    this.BeginAnimation(Canvas.OpacityProperty, anim);
                }

                private void Anim_Completed(object sender, EventArgs e)
                {
                    Showed = false; Visibility = Visibility.Hidden;
                }
            }
        }
        private void Close_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }

        public void Refresh(LandSector sector)
        {
            
        }
        class SmallButton:Canvas
        {
            ScaleTransform Scale;
            public PanelMode CurMode;
            public SmallButton(Brush InnerImage, PanelMode curMode)
            {
                CurMode = curMode;
                Width = 150; Height = 150; Background = Gets.AddPicColony2("kub");
                Rectangle rect = Common.GetRectangle(90, InnerImage);
                Children.Add(rect); Canvas.SetLeft(rect, 30); Canvas.SetTop(rect, 30);
                
                RenderTransformOrigin = new Point(0.5, 0.5);
                Scale = new ScaleTransform();
                RenderTransform = Scale;
                DeSelect();
                //MouseEnter += SmallButton_MouseEnter;
                //MouseLeave += SmallButton_MouseLeave;
            }
            public void Select()
            {
                Scale.ScaleX = 1.0; Scale.ScaleY = 1.0;
            }
            public void DeSelect()
            {
                Scale.ScaleX = 0.9; Scale.ScaleY = 0.9;
            }
           /* private void SmallButton_MouseLeave(object sender, MouseEventArgs e)
            {
                Scale.ScaleX = 1.0; Scale.ScaleY = 1.0;
            }

            private void SmallButton_MouseEnter(object sender, MouseEventArgs e)
            {
                Scale.ScaleX = 0.9; Scale.ScaleY = 0.9;
            }*/
        }
    }
    /*class AvanpostPanel : Canvas
    {
        public Avanpost Avanpost;

        TextBlock LandName;
        AvanpostPriceElement money, metal, chips, anti;
        public AvanpostPanel(Avanpost avanpost)
        {
            Avanpost = avanpost;
            Width = 1320;
            Height = 640;
            Background = Links.Brushes.Colony.Colony1;
            Canvas.SetLeft(this, -50);
            Brush border = Brushes.Red; int size = 0; int imleft = 20;
            Rectangle close = Common.GetRectangle(50, 50, Gets.AddPicColony2("x"));
            Children.Add(close); Canvas.SetLeft(close, 1300); Canvas.SetTop(close, -20);
            close.PreviewMouseDown += Close_PreviewMouseDown;
            switch (avanpost.Planet.PlanetType)
            {
                case PlanetTypes.Green: border = Brushes.Green; size = 150; break;
                case PlanetTypes.Burned: border = Brushes.Red; size = 130; imleft = 30; break;
                case PlanetTypes.Freezed: border = Brushes.Blue; size = 150; break;
                case PlanetTypes.Gas: border = Brushes.Silver; size = 130; break;
            }
            Rectangle image = Common.GetRectangle(size, Links.Brushes.PlanetTypeBrushes[avanpost.Planet.PlanetType]);
            Children.Add(image); Canvas.SetLeft(image, imleft); Canvas.SetTop(image, 30);
            LandName = Common.GetBlock(70, avanpost.Name.ToString(), Brushes.White, 580);
            Children.Add(LandName); Canvas.SetLeft(LandName, 500); Canvas.SetTop(LandName, 65);

            money = new AvanpostPriceElement(Avanpost.NeedResources.Money, Avanpost.HaveResources.Money,
                (int)(GSGameInfo.Money < Int32.MaxValue ? GSGameInfo.Money : Int32.MaxValue), Links.Brushes.MoneyImageBrush, Brushes.Green, Brushes.LightGreen);
            money.Place(this, 200, 200, 1.5);
            metal = new AvanpostPriceElement(Avanpost.NeedResources.Metall, Avanpost.HaveResources.Metall,
                GSGameInfo.Metals, Links.Brushes.MetalImageBrush, Brushes.DarkGray, Brushes.LightGray);
            metal.Place(this, 400, 200, 1.5);
            chips = new AvanpostPriceElement(Avanpost.NeedResources.Chips, Avanpost.HaveResources.Chips,
                GSGameInfo.Chips, Links.Brushes.ChipsImageBrush, Brushes.DarkBlue, Brushes.SkyBlue);
            chips.Place(this, 600, 200, 1.5);
            anti = new AvanpostPriceElement(Avanpost.NeedResources.Anti, Avanpost.HaveResources.Anti,
                GSGameInfo.Anti, Links.Brushes.AntiImageBrush, Brushes.Red, Brushes.Pink);
            anti.Place(this, 800, 200, 1.5);

            GameButton Full = new GameButton(200, 100, "Максимум", 26);
            Full.PutToCanvas(this, 1000, 250);
            Full.PreviewMouseDown += Full_PreviewMouseDown;

            GameButton Build = new GameButton(200, 100, "Построить", 26);
            Build.PutToCanvas(this, 1000, 400);
            Build.PreviewMouseDown += Build_PreviewMouseDown;

            if (Avanpost.Pillage == true)
            {
                Rectangle rect = Common.GetRectangle(60, Links.Brushes.FleetPillageBrush);
                Children.Add(rect); Canvas.SetLeft(rect, 1020); Canvas.SetTop(rect, 60);
                rect.ToolTip = "Аванпост грабят";
            }
            if (Avanpost.Conquer == true)
            {
                Rectangle rect = Common.GetRectangle(60, Links.Brushes.FleetConquerBrush);
                Children.Add(rect); Canvas.SetLeft(rect, 1020); Canvas.SetTop(rect, 120);
                rect.ToolTip = "Аванпост захватывают";
            }
            if (Avanpost.RiotIndicator > 0)
            {
                TextBlock block = Common.GetBlock(30, Avanpost.RiotIndicator.ToString() + "%", Brushes.White, 80); block.TextAlignment = TextAlignment.Left;
                Children.Add(block); Canvas.SetLeft(block, 1090); Canvas.SetTop(block, 140);
                if (Avanpost.Conquer == false)
                {
                    Rectangle rect = Common.GetRectangle(80, Links.Brushes.FleetConquerBrush); rect.Opacity = 0.5;
                    Children.Add(rect); Canvas.SetLeft(rect, 1110); Canvas.SetTop(rect, 460);
                    rect.ToolTip = "Уровень захвата";
                }
            }
        }

        private void Close_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }

        private void Build_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            string eventresult = Events.BuildInNewColony(Avanpost, money.CurValue, metal.CurValue, chips.CurValue, anti.CurValue);
            if (eventresult == "")
            {
                Gets.GetResources();
                Gets.GetTotalInfo("После эвента по постройке здания");
                Colonies.InnerRefresh();
            }
            else Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult));
        }

        private void Full_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            money.SetFull(); metal.SetFull(); chips.SetFull(); anti.SetFull();
        }

        class AvanpostPriceElement : Canvas
        {
            Canvas MoveIndicator;
            TextBlock Block1;
            TextBlock Block2;
            Rectangle left, right;
            int Max, Min, Mult;
            public int CurValue;
            public AvanpostPriceElement(int max, int cur, int res, Brush brush1, Brush brush2, Brush brush3)
            {
                Width = 100;
                Height = 220;
                if (res > max - cur) res = max - cur;
                Mult = max;
                Rectangle curvalue = new Rectangle(); curvalue.Width = 50; curvalue.Height = cur / (double)max * 200;
                Children.Add(curvalue); curvalue.Fill = brush2; curvalue.Stroke = Brushes.White;
                Canvas.SetLeft(curvalue, 25); Canvas.SetTop(curvalue, 20 + 200 - curvalue.Height);
                Min = cur; Max = cur + res;
                double delta = max - cur; if (res < delta) delta = res;
                double curpos = (delta + cur) / (double)max;
                Rectangle resvalue = new Rectangle(); resvalue.Width = 50; resvalue.Height = delta / max * 200;
                Children.Add(resvalue); resvalue.Fill = brush3; resvalue.Stroke = Brushes.White;
                Canvas.SetLeft(resvalue, 25); Canvas.SetTop(resvalue, 20 + 200 - curvalue.Height - resvalue.Height);

                Rectangle border = new Rectangle(); border.Width = 50; border.Height = 200;
                Children.Add(border); border.Stroke = Brushes.White;
                Canvas.SetLeft(border, 25); Canvas.SetTop(border, 20);

                MoveIndicator = new Canvas(); MoveIndicator.Width = 100; MoveIndicator.Height = 25;
                left = new Rectangle(); left.Width = 25; left.Height = 25;
                left.Fill = brush1; MoveIndicator.Children.Add(left);
                left.MouseDown += Left_MouseDown;
                left.MouseMove += Left_MouseMove;
                left.MouseUp += Left_MouseUp;
                right = new Rectangle(); right.Width = 25; right.Height = 25;
                right.Fill = brush1; MoveIndicator.Children.Add(right);
                Canvas.SetLeft(right, 75);
                right.MouseDown += Left_MouseDown;
                right.MouseMove += Left_MouseMove;
                right.MouseUp += Left_MouseUp;
                Rectangle middle = new Rectangle(); middle.Width = 50; middle.Height = 6;
                middle.Fill = Brushes.Gray; MoveIndicator.Children.Add(middle);
                Canvas.SetLeft(middle, 25); Canvas.SetTop(middle, 9.5);
                Children.Add(MoveIndicator);
                Canvas.SetTop(MoveIndicator, 20 + 200 - cur / (double)max * 200 - 12.5);


                Block1 = new TextBlock(); Children.Add(Block1); Block1.Width = 100;
                Block1.Foreground = Brushes.White; Block1.FontSize = 12; Block1.TextAlignment = TextAlignment.Center;
                Block1.Text = cur.ToString();

                Block2 = new TextBlock(); Children.Add(Block2); Block2.Width = 100;
                Block2.Foreground = Brushes.White; Block2.FontSize = 12; Block2.TextAlignment = TextAlignment.Center;
                Canvas.SetTop(Block2, 225); Block2.Text = "0";
                MouseMove += AvanpostPriceElement_MouseMove;
            }
            public void Place(Canvas canvas, int left, int top, double mult)
            {
                if (mult != 1.0)
                {
                    Viewbox box = new Viewbox();
                    box.Child = this; box.Width = 100 * mult; box.Height = 220 * mult;
                    canvas.Children.Add(box);
                    Canvas.SetLeft(box, left); Canvas.SetTop(box, top);
                }
            }
            int timerpos = 200;
            public void SetFull()
            {
                System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();
                Timer.Interval = TimeSpan.FromSeconds(0.01);
                Timer.Tick += Timer_Tick;
                timerpos = (int)(200 - Min / (double)Mult * 200);
                Timer.Start();
            }

            private void Timer_Tick(object sender, EventArgs e)
            {
                if (MoveTo(timerpos) == false) ((System.Windows.Threading.DispatcherTimer)sender).Stop();
                timerpos -= 2;
                if (timerpos < 0) ((System.Windows.Threading.DispatcherTimer)sender).Stop();
            }
            private void AvanpostPriceElement_MouseMove(object sender, MouseEventArgs e)
            {
                if (KeyPressed == false) return;
                HitTestResult result = VisualTreeHelper.HitTest(left, e.GetPosition(left));
                if (result == null)
                {
                    result = VisualTreeHelper.HitTest(right, e.GetPosition(right));
                    if (result != null) return;
                }
                else return;
                KeyPressed = false;
            }

            private void Left_MouseUp(object sender, MouseButtonEventArgs e)
            {
                KeyPressed = false;
            }

            private void Left_MouseLeave(object sender, MouseEventArgs e)
            {
                KeyPressed = false;
            }

            private void Left_MouseMove(object sender, MouseEventArgs e)
            {
                if (KeyPressed == false) return;
                Rectangle rect = (Rectangle)sender;
                Point pt = e.GetPosition(this);
                double Y1 = pt.Y - 20 + 12.5 - DY;
                MoveTo(Y1);
            }

            double Y;
            double DY;
            bool KeyPressed = false;
            private void Left_MouseDown(object sender, MouseButtonEventArgs e)
            {
                Rectangle rect = (Rectangle)sender;
                Point pt = e.GetPosition(this);
                Point pt2 = e.GetPosition(rect);
                DY = pt2.Y;
                Y = pt.Y - 20 + 12.5 - pt2.Y;
                KeyPressed = true;
            }
            bool MoveTo(double Y)
            {
                Y = Math.Round(Y / 2, 0) * 2;
                double pos = Y + 20 - 12.5;
                if (pos < 7.5 || pos > 207.5) return false;
                double Z = (((200 - Y) / 200) * Mult);
                if (Z < Min || Z > Max) return false;
                Canvas.SetTop(MoveIndicator, pos);
                Block1.Text = Z.ToString();
                Block2.Text = (Z - Min).ToString();
                CurValue = (int)(Z - Min);
                return true;

            }

        }
    }
   */
    class SectorImages
    {
        public static SortedList<SectorTypes, SectorImages> List = Get();
        public Brush MainImage;
        public string Title;
        public string Description;
        public string ShortTitle;
        public Brush Rect1;
        public Brush Rect2;
        public SectorImages(Brush main, string t, string st, string d, Brush b1, Brush b2)
        {
            MainImage = main; Title = t; ShortTitle = st; Description = d; Rect1 = b1; Rect2 = b2;
        }
        static SortedList<SectorTypes, SectorImages> Get()
        {
            SortedList<SectorTypes, SectorImages> result = new SortedList<SectorTypes, SectorImages>();
            result.Add(SectorTypes.Clear, new SectorImages(Links.Brushes.LandIconBrush, "свободный",
                "Свободный сектор", "", null, null));
            result.Add(SectorTypes.Live, new SectorImages(Links.Brushes.TradeSector, "Жилой сектор", "жилой",
                "Жилой сектор приспособлен под строительство социальных объектов. Эти сооружения позволяют размещать людей на своей территории, а также способствуют их приросту.",
                Links.Brushes.PeopleImageBrush, Links.Brushes.PeopleImageBrush));
            result.Add(SectorTypes.Money, new SectorImages(Links.Brushes.Bank, "Банковский сектор","банковский",
                "Банковский сектор предназначен для строительства финансовых учреждений. Все они тем или иным способом предназначены для получения вами денег.",
                Links.Brushes.MoneyImageBrush, null));
            result.Add(SectorTypes.Metall, new SectorImages(Links.Brushes.Mine, "Сектор добычи металла","добычи металла",
                "В секторе добычи металла располагаются шахты и прочие объекты добывающие металл. Так же на его территории можно располагать склады металла, но с очень низкой эффективностью.",
                Links.Brushes.MetalImageBrush, Links.Brushes.MetalCapBrush));
            result.Add(SectorTypes.MetalCap, new SectorImages(Links.Brushes.RepairingBay, "Сектор хранения металла","хранения металла",
                "Инфраструктура сектора хранения металла позволяет вам строить эффективные комплексы по складированию металла. Также здесь можно располагать низкоэффективные комплексы по его добыче.",
                Links.Brushes.MetalCapBrush, Links.Brushes.MetalImageBrush));
            result.Add(SectorTypes.Chips, new SectorImages(Links.Brushes.ChipsFactory, "Сектор производства микросхем","производства микросхем",
                "Производство микросхем требует особой инфраструктуры, которая имеется в данном секторе. Также на его территории можно хранить небольшие партии готовых микросхем.",
                Links.Brushes.ChipsImageBrush, Links.Brushes.ChipCapBrush));
            result.Add(SectorTypes.ChipsCap, new SectorImages(Links.Brushes.University, "Сектор складирования микросхем","складирования микросхем",
                "Производство микросхем требует особой инфраструктуры, которая имеется в данном секторе. Также на его территории можно хранить небольшие партии готовых микросхем.",
                Links.Brushes.ChipCapBrush, Links.Brushes.ChipsImageBrush));
            result.Add(SectorTypes.Anti, new SectorImages(Links.Brushes.ParticipleAccelerator, "Сектор выработки антиматерии","выработки антиматерии",
               "Синтез антиматерии крайне сложный процесс, требующих больших затрат энергии. Лучше всего его производить в специальном секторе как этот. Небольшие запасы антиматерии также можно расположить здесь.",
               Links.Brushes.AntiImageBrush, Links.Brushes.AntiCapBrush));
            result.Add(SectorTypes.AntiCap, new SectorImages(Links.Brushes.ScienceSector, "Сектор накопления антиматерии","накопления антиматерии",
               "Сектор накопления антиматерии отвечает самым высоким стандартам безопасности по её хранению. Так же антиматерия обладает свойством притягивания частиц из пространства, что может быть использовано для её добычи.",
               Links.Brushes.AntiCapBrush, Links.Brushes.AntiImageBrush));
            result.Add(SectorTypes.Repair, new SectorImages(Links.Brushes.Manufacture, "Сектор ремонта кораблей","ремонта кораблей",
                "В ремонтном секторе можно строить заводы запасных частей и ремонтные ангары, которые позволяют ремонтировать вам боевые корабли между миссиями.",
                Links.Brushes.RepairPict, null));
            result.Add(SectorTypes.War, new SectorImages(Links.Brushes.FleetBrush, "Военная база","военная база",
                "Военная база предназначена для размещения одного флота боевых кораблей. Вы также можете построить на её территории дополнительные объекты, увеличивающие возможности приписанного флота.",
                null, null));
            return result;
        }
    }
    class NewSectorSelectPanel : Viewbox
    {
        Land Land;
        int Position;
        TextBlock Info;
        Canvas CurCanvas;
        public NewSectorSelectPanel(Land land, int pos)
        {
            Land = land;
            Position = pos;
            Width = 1200; Height = 825;
            CurCanvas = new Canvas(); Child = CurCanvas;
            CurCanvas.Background = Links.Brushes.Colony.Colony5;
            CurCanvas.Width = 800;
            CurCanvas.Height = 550;
            SortedSet<SectorTypes> AllSectors = new SortedSet<SectorTypes>(new SectorTypes[] {SectorTypes.Live, SectorTypes.Money,
            SectorTypes.Metall, SectorTypes.MetalCap, SectorTypes.Chips, SectorTypes.ChipsCap, SectorTypes.Anti, SectorTypes.AntiCap, SectorTypes.Repair});
            List<SectorTypes> list = new List<SectorTypes>();
            foreach (LandSector s in land.Locations)
                if (AllSectors.Contains(s.Type))
                    AllSectors.Remove(s.Type);
            list.AddRange(AllSectors);
            list.Add(SectorTypes.War);
            TextBlock Title = Common.GetBlock(30, "Постройка нового сектора", Brushes.White, 600);
            CurCanvas.Children.Add(Title);
            Canvas.SetLeft(Title, 100); Canvas.SetTop(Title, 50);
            StackPanel stack = new StackPanel(); stack.Orientation = Orientation.Vertical;
            CurCanvas.Children.Add(stack); Canvas.SetTop(stack, 130); Canvas.SetLeft(stack, 130);
            for (int i = 0; i < list.Count; i++)
            {
                SectorTypes s = list[i];
                NewSectorButton btn = new NewSectorButton(SectorImages.List[s].Title);
                stack.Children.Add(btn);
                btn.Tag = s;
                btn.PreviewMouseDown += CreateNewSector;
                btn.MouseEnter += Border_MouseEnter;
            }
            Border InfoBorder = new Border(); InfoBorder.Width = 280; InfoBorder.Height = 440;
            //InfoBorder.BorderBrush = Brushes.White; InfoBorder.BorderThickness = new Thickness(3);
            //InfoBorder.CornerRadius = new CornerRadius(20);
            InfoBorder.Background = Links.Brushes.Colony.Colony8;

            CurCanvas.Children.Add(InfoBorder); Canvas.SetLeft(InfoBorder, 385); Canvas.SetTop(InfoBorder, 75);
            Info = Common.PutBlock(24, InfoBorder, "Описание сектора"); Info.Margin = new Thickness(20);
            Info.Foreground = Brushes.White; Info.LineHeight = 10;
            Ellipse closebtn = new Ellipse(); closebtn.Width = 45; closebtn.Height = 45;
            closebtn.Fill = Links.Brushes.Transparent;
            CurCanvas.Children.Add(closebtn); Canvas.SetLeft(closebtn, 650); Canvas.SetTop(closebtn, 25);
            //closebtn.PutToCanvas(canvas, 410, 440);
            closebtn.PreviewMouseDown += Closebtn_PreviewMouseDown;
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            Canvas border = (Canvas)sender;
            SectorTypes s = (SectorTypes)border.Tag;
            Info.Text = SectorImages.List[s].Description;
        }
        private void CreateNewSector(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
            Canvas border = (Canvas)sender;
            SectorTypes s = (SectorTypes)border.Tag;
            string eventresult = Events.BuildNewSector(Land, s, Position);
            if (eventresult == "")
            {
                Gets.GetTotalInfo("После эвента по строительству сектора");
                Colonies.InnerRefresh();
            }//Links.Controller.SelectPanel(GamePanels.Colonies, SelectModifiers.None);
        }
        private void Closebtn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }
        class NewSectorButton : Canvas
        {
            Rectangle Rect1;
            Rectangle Rect2;
            public NewSectorButton(string text)
            {
                Width = 260;
                Height = 40;
                Rect1 = new Rectangle(); Rect1.Width = 260; Rect1.Height = 40;
                Children.Add(Rect1);
                Rect1.Fill = Links.Brushes.Colony.Colony6;
                Rect2 = new Rectangle(); Rect2.Width = 260; Rect2.Height = 40;
                Children.Add(Rect2);
                Rect2.Fill = Links.Brushes.Colony.Colony7;
                Rect2.Opacity = 0;
                TextBlock block = Common.GetBlock(18, text, new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)), 240);
                Children.Add(block); Canvas.SetLeft(block, 10); Canvas.SetTop(block, 5);

            }
            protected override void OnMouseEnter(MouseEventArgs e)
            {
                DoubleAnimation anim1 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.3));
                Rect1.BeginAnimation(Rectangle.OpacityProperty, anim1);
                DoubleAnimation anim2 = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
                Rect2.BeginAnimation(Rectangle.OpacityProperty, anim2);
            }
            protected override void OnMouseLeave(MouseEventArgs e)
            {
                DoubleAnimation anim1 = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
                Rect1.BeginAnimation(Rectangle.OpacityProperty, anim1);
                DoubleAnimation anim2 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.3));
                Rect2.BeginAnimation(Rectangle.OpacityProperty, anim2);
            }
        }
    }
  
    struct GSRes
    {
        public int Money;
        public int Metal;
        public int Chips;
        public int Anti;
        public GSRes(int money, int metal, int chips, int anti)
        {
            Money = money; Metal = metal; Chips = chips; Anti = anti;
        }
    }
    enum BILLoc { Bottom, Left, Right, Top };
    class BIL
    {
        public Point pt;
        public BILLoc location;
        public BIL(int left, int top)
        {
            location = BILLoc.Bottom;
            pt = new Point(left, top);
        }
        public BIL(int left, int top, BILLoc loc)
        {
            location = loc;
            pt = new Point(left, top);
        }
    }
}
