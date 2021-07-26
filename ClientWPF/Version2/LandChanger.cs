using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    class LandChangerBase:Canvas
    {
        enum LandChangerState { Land, Changer, None}
        LandChangerState CurState = LandChangerState.None;
        LandChanger CurChanger;
        public LandChangerBase()
        {
            Width = 1920;
            Height = 1080;
        }
        public void Select()
        {
            Children.Clear();
            Gets.GetTotalInfo("После открытия панели по выбору колоний");
            if (Colonies.CurrentPlanet!=null)
            {
                //Links.Controller.MainCanvas.LowBorder.Child = Links.Controller.Colonies;
                CurState = LandChangerState.Land;
                Children.Add(Links.Controller.Colonies);
                Links.Controller.Colonies.Select();
            }
            else if (GSGameInfo.PlayerLands.Count + GSGameInfo.PlayerAvanposts.Count == 1)
            {
                CurState = LandChangerState.Land;
                Colonies.CurrentPlanet = GSGameInfo.PlayerLands.Values[0].Planet;
                //Links.Controller.MainCanvas.LowBorder.Child = Links.Controller.Colonies;
                Children.Add(Links.Controller.Colonies);
                Links.Controller.Colonies.Select();
            }
            else if (GSGameInfo.PlayerLands.Count + GSGameInfo.PlayerAvanposts.Count > 1)
            {
                CurState = LandChangerState.Changer;
                CurChanger = new LandChanger();
                Children.Add(CurChanger);
            }
        }
        public static void EndTurn()
        {
            if (Links.Controller.LandChangerBase.CurState == LandChangerState.None)
                throw new Exception();
            else if (Links.Controller.LandChangerBase.CurState==LandChangerState.Land)
                Colonies.InnerRefresh();
            else
            {
                LandChanger.PlanetInfo info = Links.Controller.LandChangerBase.CurChanger.CurImages[0];
                Links.Controller.SelectPanel(GamePanels.Galaxy, SelectModifiers.None);
                Links.Controller.Galaxy.ShowOneStar(info.Planet.Star, Double.NaN, 0);
            }
        }
    }
    class LandChanger:Viewbox
    {
        public static LandChanger CurWindow;
        Canvas MainCanvas;
        Canvas RearCanvas;
        ScaleTransform RearViewScale;
        Rectangle Ramka2;
        Rectangle Ramka1;
        Rectangle Loading;
        StackPanel Stack0;
        TextBlock MainBlock;
        Rectangle Hero;
        Rectangle Next;
        Rectangle Button;
        bool IsAnimated = false;
        public SortedList<int, PlanetInfo> CurImages;
        int LastPos;
        SortedList<int, GSPlanet> Planets;
        Rectangle RearRect;
        public LandChanger()
        {
            CurWindow = this;
            Stretch = Stretch.Fill;
            Width = 1920; Height = 1080;
            MainCanvas = new Canvas(); MainCanvas.Width = 1922; MainCanvas.Height = 1082; Child = MainCanvas;
            MainCanvas.Background = GetBrush("fon");
            PutRectangle(1922, 1081, "heks", MainCanvas, 0, 0);
            PutRectangle(1922, 1081, "setka", MainCanvas, 0, 0);
            Viewbox RearView = new Viewbox(); RearView.Width = 1922; RearView.Height = 594; MainCanvas.Children.Add(RearView);
            Canvas.SetTop(RearView, 488); RearView.RenderTransformOrigin = new Point(0.5, 0.5);
            RearCanvas = new Canvas(); RearCanvas.Width = 1922; RearCanvas.Height = 594; RearView.Child = RearCanvas;
            RearView.RenderTransform = RearViewScale = new ScaleTransform(0.9, 1);
            PutRectangle(1743, 1082, "maket", MainCanvas, 90, 0);
            PutRectangle(644, 215, "grafa", MainCanvas, 1270, 843);
            Ramka2 = PutRectangle(1377, 718, "ramka 2", MainCanvas, 545, 100);
            Ramka1 = PutRectangle(657, 312, "ramka 1", MainCanvas, 0, 770);
            Loading = PutRectangle(372, 56, "loading", MainCanvas, 900, 225);
            Canvas MainText = new Canvas(); MainText.Width = 401; MainText.Height = 380; MainText.ClipToBounds = true;
            MainCanvas.Children.Add(MainText); Canvas.SetLeft(MainText, 650); Canvas.SetTop(MainText, 280);
            Stack0 = new StackPanel(); Stack0.Orientation = Orientation.Vertical; MainText.Children.Add(Stack0);
            Stack0.Children.Add(PutRectangle(401, 519, "1 infa", null, 0, 0));
            Stack0.Children.Add(MainBlock = Common.GetBlock(24, "", new SolidColorBrush(Color.FromRgb(160, 208, 255)), 401));
            Hero = PutRectangle(873, 924, "hero", MainCanvas, 850, 158);
            Next = PutRectangle(99, 90, "next", MainCanvas, 80, 170); Canvas.SetZIndex(Next, 5);
            PlaceRotateRect(142, 142, "Krug", MainCanvas, 1756, 860, 3);
            PlaceRotateRect(50, 50, "Krug2", MainCanvas, 1618, 954, 3);
            PlaceMovingText(166, 47, "next kruga", MainCanvas, 1686, 1031, new int[] { 0, -13, -13, -24, -24, -47, -47 }, 1.5);
            Button = PutRectangle(250, 230, "colony inform", MainCanvas, 70, 784);
            Button.PreviewMouseDown += OpenLand_Event;
            Button.MouseEnter += Button_MouseEnter;
            Button.MouseLeave += Button_MouseLeave;
            PlaceMovingText(309, 180, "inform", MainCanvas, 261, 791, new int[] { 0, -26, -26, -40, -40, -78, -78, - 97, -97, -134, -134, -152, -152, -180, -180 }, 2.5);
            PutRectangle(1353, 209, "osnowa", MainCanvas, 284, 873);
            ColorAnimation anim = new ColorAnimation(Color.FromRgb(160, 205, 255), Colors.White, TimeSpan.FromSeconds(0.2));
            anim.RepeatBehavior = RepeatBehavior.Forever;

            Start();
        }
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button.RenderTransform = null;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button.RenderTransformOrigin = new Point(0.5, 0.5);
            Button.RenderTransform = new ScaleTransform(0.9, 0.9);
        }

        private void OpenLand_Event(object sender, MouseButtonEventArgs e)
        {
            if (CurImages[0].Select() == true) Links.Controller.PopUpCanvas.Remove();

        }

        ImageBrush GetBrush(string name)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/LandChanger/{0}.png", name))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/LandChanger/{0}.png", name), UriKind.Relative)));
        }
        Rectangle PutRectangle(int width, int height, string brushname, Canvas canvas, int left, int top)
        {
            Rectangle rect = new Rectangle(); rect.Width = width; rect.Height = height;
            rect.Fill = GetBrush(brushname);
            if (canvas!= null)
            {
                canvas.Children.Add(rect); Canvas.SetLeft(rect, left); Canvas.SetTop(rect, top);
            }
            return rect;
        }
        void PlaceRotateRect(int width, int height, string brushname, Canvas canvas, int left, int top, double time)
        {
            Rectangle rect = PutRectangle(width, height, brushname, canvas, left, top);
            rect.RenderTransformOrigin = new Point(0.5, 0.5);
            RotateTransform rotate = new RotateTransform();
            rect.RenderTransform = rotate;
            DoubleAnimation anim = new DoubleAnimation(0, 360, TimeSpan.FromSeconds(time));
            anim.RepeatBehavior = RepeatBehavior.Forever;
            rotate.BeginAnimation(RotateTransform.AngleProperty, anim);
        }
        void PlaceMovingText(int width, int height, string brushname, Canvas parrent, int left, int top, int[] points, double time)
        {
            Canvas canvas = new Canvas(); canvas.Width = width; canvas.Height = height; canvas.ClipToBounds = true;
            parrent.Children.Add(canvas); Canvas.SetLeft(canvas, left); Canvas.SetTop(canvas, top);
            StackPanel panel = new StackPanel(); panel.Orientation = Orientation.Vertical; canvas.Children.Add(panel);
            panel.Children.Add(PutRectangle(width, height, brushname, null, 0, 0));
            panel.Children.Add(PutRectangle(width,  height, brushname, null, 0, 0));
            DoubleAnimationUsingKeyFrames anim = new DoubleAnimationUsingKeyFrames();
            anim.RepeatBehavior = RepeatBehavior.Forever;
            anim.Duration = TimeSpan.FromSeconds(time);
            for (int i = 0; i < points.Length; i++)
                anim.KeyFrames.Add(new LinearDoubleKeyFrame(points[i], KeyTime.FromPercent((double)i/(points.Length-1))));
            panel.BeginAnimation(Canvas.TopProperty, anim);
        }
        public void Start()
        {
            DoubleAnimation heroanim = new DoubleAnimation(1922, 850, TimeSpan.FromSeconds(1.0));
            Hero.BeginAnimation(Canvas.LeftProperty, heroanim);
            Next.PreviewMouseDown += Next_PreviewMouseDown;
            CurImages = new SortedList<int, PlanetInfo>();
            Planets = new SortedList<int, GSPlanet>();
            foreach (Land land in GSGameInfo.PlayerLands.Values)
                Planets.Add(land.ID, land.Planet);
            foreach (Avanpost avanpost in GSGameInfo.PlayerAvanposts.Values)
                Planets.Add(avanpost.ID, avanpost.Planet);
            for (int i = 0; i < 6; i++)
            {
                GSPlanet planet;
                planet =  Planets.Values[i % Planets.Count];
                PlanetInfo info = new PlanetInfo(i, planet, Planets.Keys[i%Planets.Count]);
                MainCanvas.Children.Add(info);
                if (info.Opacity != 0)
                {
                    DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1.0));
                    info.BeginAnimation(Viewbox.OpacityProperty, anim);
                }
                CurImages.Add(i, info);
                LastPos = i;
                if (i == 0)
                    info.Show();
                info.PreviewMouseDown += Next_PreviewMouseDown;
            }
            if (Planets.Count > 6)
            {
                GSPlanet planet = Planets.Values[Planets.Count - 1];
                PlanetInfo info = new PlanetInfo(-1, planet, Planets.Keys[Planets.Count - 1]);
                MainCanvas.Children.Add(info);
                CurImages.Add(-1, info);
            }
            DoubleAnimation rearviewanim = new DoubleAnimation(1, 1.6, TimeSpan.FromSeconds(25));
            rearviewanim.RepeatBehavior = RepeatBehavior.Forever;
            rearviewanim.AutoReverse = true;
            RearViewScale.BeginAnimation(ScaleTransform.ScaleXProperty, rearviewanim);
            RearViewScale.BeginAnimation(ScaleTransform.ScaleYProperty, rearviewanim);
            StartLoad(CurImages[0]);
        }

        private void Next_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            DoubleAnimation anim = new DoubleAnimation(100, -1000, TimeSpan.FromSeconds(0.5));
            anim.AutoReverse = true;
            anim.AccelerationRatio = 1;
            Ramka2.BeginAnimation(Canvas.TopProperty, anim);
            DoubleAnimation anim1 = new DoubleAnimation(770, 2000, TimeSpan.FromSeconds(0.5));
            anim1.AutoReverse = true;
            anim1.AccelerationRatio = 1;
            Ramka1.BeginAnimation(Canvas.TopProperty, anim1);
            MoveOne();
        }
        public void MoveOne()
        {
            if (IsAnimated == true) return;
            IsAnimated = true;
            CurImages.Remove(-1);
            foreach (PlanetInfo info in CurImages.Values)
            {
                info.MoveNext();
            }
        }
        public void FinishLoad(PlanetInfo info)
        {
            IsAnimated = false;
            SortedList<int, PlanetInfo> NewCurImages = new SortedList<int, PlanetInfo>();
            foreach (PlanetInfo i in CurImages.Values)
                NewCurImages.Add(i.Pos, i);
            LastPos++;
            NewCurImages.Add(LastPos, new PlanetInfo(5, Planets.Values[LastPos % Planets.Count], Planets.Keys[LastPos%Planets.Count]));
            MainCanvas.Children.Add(NewCurImages[LastPos]);
            NewCurImages[LastPos].PreviewMouseDown += Next_PreviewMouseDown;
            CurImages = NewCurImages;
            StartLoad(info);
        }
        public void RefreshMainBlock(PlanetInfo CurInfo)
        {
            MainBlock.Inlines.Clear();
            List<Inline> list = CurInfo.GetTextInfo();
            MainBlock.Inlines.AddRange(list);
        }
        public void StartLoad(PlanetInfo CurInfo)
        {
            MainBlock.Inlines.Clear();
            List<Inline> list = CurInfo.GetTextInfo();
            MainBlock.Inlines.AddRange(list);
            PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            //DoubleAnimation showanim = new DoubleAnimation(0, 1, TimeSpan.Zero);
            //infa1.BeginAnimation(Rectangle.OpacityProperty, showanim);
            DoubleAnimationUsingKeyFrames moveanim = new DoubleAnimationUsingKeyFrames();
            moveanim.KeyFrames.Add(new LinearDoubleKeyFrame(450, KeyTime.FromPercent(0)));
            moveanim.KeyFrames.Add(new LinearDoubleKeyFrame(320, KeyTime.FromPercent(0.07)));
            moveanim.KeyFrames.Add(new LinearDoubleKeyFrame(320, KeyTime.FromPercent(0.14)));
            moveanim.KeyFrames.Add(new LinearDoubleKeyFrame(260, KeyTime.FromPercent(0.21)));
            moveanim.KeyFrames.Add(new LinearDoubleKeyFrame(260, KeyTime.FromPercent(0.28)));
            moveanim.KeyFrames.Add(new LinearDoubleKeyFrame(150, KeyTime.FromPercent(0.35)));
            moveanim.KeyFrames.Add(new LinearDoubleKeyFrame(150, KeyTime.FromPercent(0.42)));
            moveanim.KeyFrames.Add(new LinearDoubleKeyFrame(-10, KeyTime.FromPercent(0.49)));
            moveanim.KeyFrames.Add(new LinearDoubleKeyFrame(-10, KeyTime.FromPercent(0.56)));
            moveanim.KeyFrames.Add(new LinearDoubleKeyFrame(-120, KeyTime.FromPercent(0.64)));
            moveanim.KeyFrames.Add(new LinearDoubleKeyFrame(-120, KeyTime.FromPercent(0.8)));
            moveanim.KeyFrames.Add(new LinearDoubleKeyFrame(-460, KeyTime.FromPercent(1)));
            moveanim.Duration = TimeSpan.FromSeconds(3);
            Stack0.BeginAnimation(Canvas.TopProperty, moveanim);
            PathGeometry loadpath = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M0,0 h1 h-1 h1 h-1 h1 h-1"));
            DoubleAnimationUsingPath loadanim = new DoubleAnimationUsingPath();
            loadanim.PathGeometry = loadpath;
            loadanim.Source = PathAnimationSource.X;
            loadanim.Duration = TimeSpan.FromSeconds(3);
            Loading.BeginAnimation(Rectangle.OpacityProperty, loadanim);
            if (RearRect != null && CurImages[-1].Rear == CurImages[0].Rear) return;
            if (RearRect != null)
            {
                DoubleAnimation rearhide = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(3));
                RearRect.BeginAnimation(Rectangle.OpacityProperty, rearhide);
                DoubleAnimation rearmovedown = new DoubleAnimation(600, TimeSpan.FromSeconds(3));
                rectforremove = RearRect;
                rearmovedown.Completed += Rearmovedown_Completed;
                RearRect.BeginAnimation(Canvas.TopProperty, rearmovedown);
            }
            RearRect = new Rectangle(); RearRect.Width = 1922; RearRect.Height = 594;
            RearRect.Fill = CurImages[0].Rear;
            RearCanvas.Children.Add(RearRect);
            Canvas.SetTop(RearRect, 600);
            DoubleAnimation rearshow = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(3));
            RearRect.BeginAnimation(Rectangle.OpacityProperty, rearshow);
            DoubleAnimation rearmoveup = new DoubleAnimation(0, TimeSpan.FromSeconds(3));
            RearRect.BeginAnimation(Canvas.TopProperty, rearmoveup);
        }
        Rectangle rectforremove;
        private void Rearmovedown_Completed(object sender, EventArgs e)
        {
            RearCanvas.Children.Remove(rectforremove);
        }
        public class PlanetInfo : Viewbox
        {
            static SortedList<int, int[]> Points = GetPoints();
            static SortedList<PlanetTypes, Brush> FrontBrushes = GetFrontBrushes();
            static SortedList<PlanetTypes, Brush> BackBrushes = GetBackBrushes();
            static SortedList<int, PathGeometry> SectorPathes = GetSectorPathes();
            static SortedList<int, Point[]> SectorPoints = GetSectorPoints();
            static SortedList<int, int[]> GetPoints()
            {
                SortedList<int, int[]> result = new SortedList<int, int[]>();
                result.Add(5, new int[] { 1150, 770, 150, 0 });
                result.Add(4, new int[] { 1000, 770, 150, 1 });
                result.Add(3, new int[] { 850, 770, 150, 1 });
                result.Add(2, new int[] { 700, 770, 150, 1 });
                result.Add(1, new int[] { 550, 770, 150, 1 });
                result.Add(0, new int[] { -30, 100, 740, 1 });
                result.Add(-1, new int[] { -770, 100, 740, 0 });
                return result;
            }
            static SortedList<PlanetTypes, Brush> GetFrontBrushes()
            {
                SortedList<PlanetTypes, Brush> result = new SortedList<PlanetTypes, Brush>();
                result.Add(PlanetTypes.Green, GetBrush("2planet"));
                result.Add(PlanetTypes.Burned, GetBrush("3planet"));
                result.Add(PlanetTypes.Freezed, GetBrush("4planet"));
                result.Add(PlanetTypes.Gas, GetBrush("1planet"));
                return result;
            }
            static SortedList<PlanetTypes, Brush> GetBackBrushes()
            {
                SortedList<PlanetTypes, Brush> result = new SortedList<PlanetTypes, Brush>();
                result.Add(PlanetTypes.Green, GetBrush("planet3"));
                result.Add(PlanetTypes.Burned, GetBrush("planet4"));
                result.Add(PlanetTypes.Freezed, GetBrush("planet"));
                result.Add(PlanetTypes.Gas, GetBrush("planet2"));
                return result;
            }
            static SortedList<int, PathGeometry> GetSectorPathes()
            {
                SortedList<int, PathGeometry> result = new SortedList<int, PathGeometry>();
                result.Add(2, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M30,0 v60")));
                result.Add(3, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M30,30 l26,-15 M4,15 l26,15 v30")));
                result.Add(4, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                    "M15,30 l7.5,-13 h15 l7.5,13 l-7.5,13 h-15z M22.5,17 v-15 M22.5,43 v15 M45,30 h15")));
                result.Add(5, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                    "M15,30 l7.5,-13 h15 l7.5,13 l-7.5,13 h-15z M22.5,17 l-10.5,-10.5 M37.5,17 l10.5,-10.5 M37.5,43 l10.5,10.5 M22.5,43 l-10.5,10.5")));
                result.Add(6, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                    "M13,30 l7.5,-15 h19 l7.5,15 l-7.5,15 h-19z M30,15 v30 M20.5,15 l-8,-8 M39.5,15 l8,-8 M39.5,45 l8,8 M20.5,45 l-8,8")));
                result.Add(7, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                    "M15,30 l7.5,-13 h15 l7.5,13 l-7.5,13 h-15z M22.5,17 l-10.5,-10.5 M37.5,17 l10.5,-10.5 M37.5,43 l10.5,10.5 M22.5,43 l-10.5,10.5 M15,30 h-15 M45,30 h15")));
                result.Add(8, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                    "M13,30 l7.5,-15 h19 l7.5,15 l-7.5,15 h-19z M30,15 v30 M20.5,15 l-8,-8 M39.5,15 l8,-8 M39.5,45 l8,8 M20.5,45 l-8,8 M13,30 h-13 M47,30 h13")));
                return result;
            }
            static SortedList<int, Point[]> GetSectorPoints()
            {
                SortedList<int, Point[]> result = new SortedList<int, Point[]>();
                result.Add(2, new Point[] { new Point(10, 25), new Point(40, 25) });
                result.Add(3, new Point[] { new Point(10, 35), new Point(40, 35), new Point(25, 10) });
                result.Add(4, new Point[] { new Point(25, 25), new Point(3, 25), new Point(35, 5), new Point(35, 45) });
                result.Add(5, new Point[] { new Point(25, 25), new Point(3, 25), new Point(25, 5), new Point(25, 45), new Point(47, 25) });
                result.Add(6, new Point[] { new Point(18, 25), new Point(32, 25), new Point(2, 25), new Point(25, 3), new Point(25, 47), new Point(48, 25) });
                result.Add(7, new Point[] { new Point(25,25), new Point(5,15), new Point(5,35), new Point(25,3), new Point(25,47),
                    new Point(45,15), new Point(45,15)});
                result.Add(8, new Point[] { new Point(18,25), new Point(32,25), new Point(5,15), new Point(5,35), new Point(25,3),
                    new Point(25,47), new Point(45,15), new Point(45,35)});
                return result;
            }
            public static ImageBrush GetBrush(string name)
            {
                if (Links.LoadImageFromDLL)
                    return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/LandChanger/{0}.png", name))));
                else
                    return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/LandChanger/{0}.png", name), UriKind.Relative)));
            }
            public GSPlanet Planet;
            public int Pos;
            public Brush Front;
            public Brush Rear;
            int[] CurPoints;
            Canvas canvas;
            int ID;
            public PlanetInfo(int pos, GSPlanet planet, int id)
            {
                Planet = planet; Pos = pos;
                ID = id;
                Front = FrontBrushes[planet.PlanetType];
                Rear = BackBrushes[planet.PlanetType];
                int[] PosArr = Points[pos];
                CurPoints = PosArr;
                Width = PosArr[2]; Height = PosArr[2];
                Canvas.SetLeft(this, PosArr[0]); Canvas.SetTop(this, PosArr[1]);
                Opacity = PosArr[3];
                canvas = new Canvas(); canvas.Width = 150; canvas.Height = 150; Child = canvas;
                Rectangle front = new Rectangle(); front.Width = 150; front.Height = 150;
                front.Fill = Front; canvas.Children.Add(front);
                Canvas.SetZIndex(front, 5);
                Rectangle planetrect = new Rectangle(); planetrect.Width = 60; planetrect.Height = 60;
                planetrect.Fill = Links.Brushes.PlanetsBrushes[planet.ImageType];
                canvas.Children.Add(planetrect); Canvas.SetTop(planetrect, 45); Canvas.SetLeft(planetrect, 45);
                Rectangle icon; TextBlock text;
                if (GSGameInfo.PlayerLands.ContainsKey(id))
                {
                    icon = Common.GetRectangle(20, Links.Brushes.PeopleImageBrush);
                    Land land = GSGameInfo.PlayerLands[id];
                    int places = (int)(land.Peoples - land.BuildingsCount); if (places < 0) places = 0;
                    text = Common.GetBlock(16, places.ToString(), Brushes.White, 30);
                    text.TextAlignment = TextAlignment.Left;
                    if (land.Pillage==true)
                    {
                        Rectangle rect = Common.GetRectangle(20, Links.Brushes.FleetPillageBrush);
                        canvas.Children.Add(rect); Canvas.SetLeft(rect, 30); Canvas.SetTop(rect, 45); Canvas.SetZIndex(rect, 20);
                    }
                    if (land.Conquer==true)
                    {
                        Rectangle rect = Common.GetRectangle(20, Links.Brushes.FleetConquerBrush);
                        canvas.Children.Add(rect); Canvas.SetLeft(rect, 100); Canvas.SetTop(rect, 45); Canvas.SetZIndex(rect, 20);
                    }
                }
                else
                {
                    icon = Common.GetRectangle(20, GetBrush("BuildIcon"));
                    Avanpost avanpost = GSGameInfo.PlayerAvanposts[id];
                    int percent = (avanpost.HaveResources.Money / 10 + avanpost.HaveResources.Metall + avanpost.HaveResources.Chips + avanpost.HaveResources.Anti) * 100 /
                        (avanpost.NeedResources.Money / 10 + avanpost.NeedResources.Metall + avanpost.NeedResources.Chips + avanpost.NeedResources.Anti);
                    text = Common.GetBlock(16, percent.ToString() + "%", Brushes.White, 30);
                    if (avanpost.Pillage == true)
                    {
                        Rectangle rect = Common.GetRectangle(20, Links.Brushes.FleetPillageBrush);
                        canvas.Children.Add(rect); Canvas.SetLeft(rect, 30); Canvas.SetTop(rect, 45); Canvas.SetZIndex(rect, 20);
                    }
                    if (avanpost.Conquer == true)
                    {
                        Rectangle rect = Common.GetRectangle(20, Links.Brushes.FleetConquerBrush);
                        canvas.Children.Add(rect); Canvas.SetLeft(rect, 100); Canvas.SetTop(rect, 45); Canvas.SetZIndex(rect, 20);
                    }
                }
                canvas.Children.Add(icon); Canvas.SetLeft(icon, 70); Canvas.SetTop(icon, 100);
                canvas.Children.Add(text); Canvas.SetLeft(text, 95); Canvas.SetTop(text, 102);
            }
            public bool Select()
            {
                if (GSGameInfo.PlayerLands.ContainsKey(ID))
                {
                    Land land = GSGameInfo.PlayerLands[ID];
                    Colonies.CurrentPlanet = land.Planet;
                    //Colonies.CurrentLand = land;
                    Links.Controller.SelectPanel(GamePanels.Colonies, SelectModifiers.None);
                    return true;
                }
                else if (GSGameInfo.PlayerAvanposts.ContainsKey(ID))
                {
                    Avanpost avanpost = GSGameInfo.PlayerAvanposts[ID];
                    Colonies.CurrentPlanet = avanpost.Planet;
                    //Colonies.CurrentAvanpost = avanpost;
                    Links.Controller.SelectPanel(GamePanels.Colonies, SelectModifiers.None);
                    return true;
                }
                return false;
            }
            static TimeSpan t = TimeSpan.FromSeconds(1);
            public void MoveNext()
            {
                Pos--;
                int[] PosArr = Points[Pos];

                if (PosArr[0] != CurPoints[0])
                {
                    DoubleAnimation anim = new DoubleAnimation(CurPoints[0], PosArr[0], t);
                    if (Pos == 0)
                        anim.Completed += Anim_Completed;
                    this.BeginAnimation(Canvas.LeftProperty, anim);
                }
                if (PosArr[1] != CurPoints[1])
                {
                    DoubleAnimation anim = new DoubleAnimation(CurPoints[1], PosArr[1], t);
                    this.BeginAnimation(Canvas.TopProperty, anim);
                }
                if (PosArr[2] != CurPoints[2])
                {
                    DoubleAnimation anim = new DoubleAnimation(CurPoints[2], PosArr[2], t);
                    this.BeginAnimation(Viewbox.WidthProperty, anim);
                    this.BeginAnimation(Viewbox.HeightProperty, anim);
                    Show();
                }
                if (PosArr[3] != CurPoints[3])
                {
                    DoubleAnimation anim = new DoubleAnimation(CurPoints[3], PosArr[3], t);
                    this.BeginAnimation(Viewbox.OpacityProperty, anim);
                }
                CurPoints = PosArr;
            }
            public void Show()
            {
                Canvas c = new Canvas(); canvas.Children.Add(c);
                Canvas.SetLeft(c, 45); Canvas.SetTop(c, 45); 
                Path path = new Path(); path.Stroke = Links.Brushes.SkyBlue;
                path.StrokeThickness = 0.5;
                path.StrokeDashArray = new DoubleCollection(new double[] { 0.5 });
                c.Children.Add(path);
                path.Data = SectorPathes[Planet.Locations];
                Point[] points = SectorPoints[Planet.Locations];
                for (int i=0;i<points.Length;i++)
                {
                    Point p = points[i];
                    if (GSGameInfo.PlayerLands.ContainsKey(ID))
                    {
                        Land land = GSGameInfo.PlayerLands[ID];
                        LandSector sector = land.Locations[i];
                        Rectangle rect = Common.GetRectangle(10, SectorImages.List[sector.Type].MainImage);
                        c.Children.Add(rect); Canvas.SetLeft(rect, p.X); Canvas.SetTop(rect, p.Y);
                    }
                    else
                    {
                        Rectangle rect = Common.GetRectangle(10, SectorImages.List[SectorTypes.Clear].MainImage);
                        c.Children.Add(rect); Canvas.SetLeft(rect, p.X); Canvas.SetTop(rect, p.Y);
                    }
                    /*Rectangle rect = new Rectangle(); rect.Width = 10; rect.Height = 10;
                    rect.RadiusX = 2; rect.RadiusY = 2; rect.Fill = Links.Brushes.SkyBlue;
                    c.Children.Add(rect);
                    Canvas.SetLeft(rect, p.X); Canvas.SetTop(rect, p.Y);
                    TextBlock block = Common.GetBlock(8, (i + 1).ToString(), Brushes.White, 10);
                    c.Children.Add(block);
                    Canvas.SetLeft(block, p.X); Canvas.SetTop(block, p.Y);*/
                }
                Rectangle nameback = new Rectangle(); nameback.Width = 100; nameback.Height = 15;
                RadialGradientBrush namebackbrush = new RadialGradientBrush();
                namebackbrush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));
                namebackbrush.GradientStops.Add(new GradientStop(Color.FromArgb(178, 78, 143, 255), 0.6));
                nameback.Fill = namebackbrush;
                nameback.RadiusX = 10; nameback.RadiusY = 10;
                c.Children.Add(nameback); Canvas.SetLeft(nameback, -25); Canvas.SetTop(nameback, -15);
                TextBlock Name;
                if (GSGameInfo.PlayerLands.ContainsKey(ID))
                {
                    Land land = GSGameInfo.PlayerLands[ID];
                    Name = Common.GetBlock(10, land.Name.ToString(), Brushes.White, 130);
                    if (land.RiotIndicator > 0)
                    {
                        TextBlock RiotInfo = Common.GetBlock(6, land.RiotIndicator.ToString() + "%", Brushes.White, 20);
                        c.Children.Add(RiotInfo); Canvas.SetLeft(RiotInfo, 55); Canvas.SetTop(RiotInfo, 20);
                        if (land.Conquer == false)
                        {
                            Rectangle rect = Common.GetRectangle(20, Links.Brushes.FleetConquerBrush); rect.Opacity = 0.5;
                            c.Children.Add(rect); Canvas.SetLeft(rect, 55); Canvas.SetTop(rect, 0);
                        }
                    }
                }
                else
                {
                    Avanpost avanpost = GSGameInfo.PlayerAvanposts[ID];
                    Name = Common.GetBlock(10, avanpost.Name.ToString(), Brushes.White, 130);
                    if (avanpost.RiotIndicator > 0)
                    {
                        TextBlock RiotInfo = Common.GetBlock(6, avanpost.RiotIndicator.ToString() + "%", Brushes.White, 20);
                        c.Children.Add(RiotInfo); Canvas.SetLeft(RiotInfo, 55); Canvas.SetTop(RiotInfo, 20);
                        if (avanpost.Conquer == false)
                        {
                            Rectangle rect = Common.GetRectangle(20, Links.Brushes.FleetConquerBrush); rect.Opacity = 0.5;
                            c.Children.Add(rect); Canvas.SetLeft(rect, 55); Canvas.SetTop(rect, 0);
                        }
                    }
                }
                c.Children.Add(Name); Canvas.SetLeft(Name, -40); Canvas.SetTop(Name, -14);
                DoubleAnimation anim = new DoubleAnimation(0, 1, t);
                c.BeginAnimation(Canvas.OpacityProperty, anim);
            }
            public List<Inline> GetTextInfo()
            {
                List<Inline> list = new List<Inline>();
                if (GSGameInfo.PlayerLands.ContainsKey(ID))
                {
                    Land land = GSGameInfo.PlayerLands[ID];
                    list.Add(new Run(land.Name.ToString()));
                    list.Add(new LineBreak());
                    switch (Planet.PlanetType)
                    {
                        case PlanetTypes.Green: list.Add(new Run("Атмосфера: Зелёная планета")); break;
                        case PlanetTypes.Burned: list.Add(new Run("Атмосфера: Выжженная планета")); break;
                        case PlanetTypes.Freezed: list.Add(new Run("Атмосфера: Ледяная планета")); break;
                        case PlanetTypes.Gas: list.Add(new Run("Атмосфера: Газовый гигант")); break;
                    }
                    list.Add(new LineBreak());
                    if (land.RiotIndicator == 0 && land.Conquer == false)
                        list.Add(new Run("Угроза восстания: отсутствует"));
                    else if (land.Conquer == true)
                        list.Add(new Run(String.Format("Угроза восстания: {0}% и растёт", land.RiotIndicator)));
                    else
                        list.Add(new Run(String.Format("Угроза восстания: {0}% и снижается", land.RiotIndicator)));
                    if (land.Pillage==true)
                    {
                        list.Add(new LineBreak());
                        Run r = new Run("Колонию грабят");
                        r.TextDecorations.Add(TextDecorations.Underline);
                        list.Add(r);
                    }
                    list.Add(new LineBreak());
                    int[] army = land.GetFleetsAndShips();
                    list.Add(new Run("Вооружённые силы: "));
                    list.Add(new LineBreak());
                    list.Add(new Run("Флотов - " + army[0].ToString() + "; Кораблей - " + army[1].ToString()));
                    list.Add(new LineBreak());
                    list.Add(new Run(String.Format("Оборона: {0}", GetProtectedFleets())));
                    list.Add(new LineBreak());
                    list.Add(new Run("Население: " + land.Peoples + " миллионов"));
                    list.Add(new LineBreak());
                    list.Add(new Run("Рождаемость: " + (land.GetGrow()).ToString("0.0") + " млн. чел./мес"));
                    list.Add(new LineBreak());
                    int places = (int)(land.Peoples - land.BuildingsCount);
                    if (places <= 0)
                        list.Add(new Run("Безработица: отсутствует"));
                    else
                        list.Add(new Run("Безработица: "+places.ToString()+" миллионов"));
                    list.Add(new LineBreak());
                    list.Add(new Run("Собираемость налогов: "+land.Add.Money.ToString()));  
                }
                else
                {
                    Avanpost avanpost = GSGameInfo.PlayerAvanposts[ID];
                    list.Add(new Run(avanpost.Name.ToString()));
                    list.Add(new LineBreak());
                    switch (Planet.PlanetType)
                    {
                        case PlanetTypes.Green: list.Add(new Run("Атмосфера: Зелёная планета")); break;
                        case PlanetTypes.Burned: list.Add(new Run("Атмосфера: Выжженная планета")); break;
                        case PlanetTypes.Freezed: list.Add(new Run("Атмосфера: Ледяная планета")); break;
                        case PlanetTypes.Gas: list.Add(new Run("Атмосфера: Газовый гигант")); break;
                    }
                    list.Add(new LineBreak());
                    if (avanpost.RiotIndicator == 0 && avanpost.Conquer == false)
                        list.Add(new Run("Угроза восстания: отсутствует"));
                    else if (avanpost.Conquer == true)
                        list.Add(new Run(String.Format("Угроза восстания: {0}% и растёт", avanpost.RiotIndicator)));
                    else
                        list.Add(new Run(String.Format("Угроза восстания: {0}% и снижается", avanpost.RiotIndicator)));
                    if (avanpost.Pillage == true)
                    {
                        list.Add(new LineBreak());
                        Run r = new Run("Аванпост грабят");
                        r.TextDecorations.Add(TextDecorations.Underline);
                        list.Add(r);
                    }
                    list.Add(new LineBreak());
                    list.Add(new Run(String.Format("Оборона: {0}", GetProtectedFleets())));
                    list.Add(new LineBreak());
                    list.Add(new Run("Финансирование: " + (avanpost.HaveResources.Money * 100 / avanpost.NeedResources.Money).ToString("0") + "%"));
                    list.Add(new LineBreak());
                    list.Add(new Run("Поставка металлов: " + (avanpost.HaveResources.Metall * 100 / avanpost.NeedResources.Metall).ToString("0") + "%"));
                    list.Add(new LineBreak());
                    list.Add(new Run("Поставка микросхем: " + (avanpost.HaveResources.Chips * 100 / avanpost.NeedResources.Chips).ToString("0") + "%"));
                    list.Add(new LineBreak());
                    list.Add(new Run("Поставка антиматерии: " + (avanpost.HaveResources.Anti * 100 / avanpost.NeedResources.Anti).ToString("0") + "%"));
                }
                return list;
            }
            string GetProtectedFleets()
            {
                GSStar star = Planet.Star;
                int result = 0;
                foreach (GSFleet fleet in GSGameInfo.Fleets.Values)
                    if (fleet.Target != null && fleet.Target.Mission == FleetMission.Scout && fleet.Target.TargetID == star.ID && fleet.Target.Order == FleetOrder.Defense)
                        result++;
                if (result == 0) return "отсутствует";
                else if (result == 1) return "1 флот";
                else if (result == 2 || result == 3 || result == 4) return String.Format("{0} флота", result);
                else return String.Format("{0} флотов", result);
            }
            private void Anim_Completed(object sender, EventArgs e)
            { 
                LandChanger.CurWindow.FinishLoad(this);  
            }
        }
    }
}
