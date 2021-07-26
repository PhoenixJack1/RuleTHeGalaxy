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

namespace Client
{
    public enum GOImageType { Standard, Hided }
    enum ImageIcons
    {
        Money, Metall, Chips, Anti, CapMetall, CapChips, CapAnti, Peoples, Math, Zip, Ships, Size,
        EnergyGenerator, ShieldGenerator, AimComputer, Engine, Equipment,
        WeaponEnergy, WeaponPhysic, WeaponIrregular, WeaponCyber, WeaponAll, SizeSmall, SizeMedium, SizeLarge,
        Health, HealthRegen, Shield, ShieldRegen, Energy, EnergyRegen, HealthDamage, ShieldDamage, Crit,
        Accuracy, AccuracyEnergy, AccuracyPhysic, AccuracyIrregular, AccuracyCyber,
        Evasion, EvasionEnergy, EvasionPhysic, EvasionIrregular, EvasionCyber,
        Damage, DamageEnergy, DamagePhysic, DamageIrregular, DamageCyber,
        Absorb, AbsorbEnergy, AbsorbPhysic, AbsorbIrregular, AbsorbCyber,
        Immune, ImmuneEnergy, ImmunerPhysic, ImmuneIrregular, ImmuneCyber
    }
    public class GameObjectImage : Viewbox
    {
        static PathGeometry SmallSizeGeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
            "M0,20 a20,20 0 0,1 20,-20 h60 a20,20 0 0,1 20,20 v40 a20,20 0 0,1 -20,20 h-60 a20,20 0 0,1 -20,-20z"));
        static PathGeometry StandardSizeGeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
            "M0,20 a20,20 0 0,1 20,-20 h60 a20,20 0 0,1 20,20 v70 a20,20 0 0,1 -20,20 h-60 a20,20 0 0,1 -20,-20z"));
        static PathGeometry FullSizeGeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
            "M0,20 a20,20 0 0,1 20,-20 h60 a20,20 0 0,1 20,20 v110 a20,20 0 0,1 -20,20 h-60 a20,20 0 0,1 -20,-20z"));
        static PathGeometry SmallBorderGeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
            "M0,20 a20,20 0 0,1 20,-20 h60 a20,20 0 0,1 20,20 v40 a20,20 0 0,1 -20,20 h-60 a20,20 0 0,1 -20,-20z"));
        static PathGeometry StandardBorderGeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
        "M0,20 a20,20 0 0,1 20,-20 h58 a20,20 0 0,1 20,20 v68 a20,20 0 0,1 -20,20 h-58 a20,20 0 0,1 -20,-20z"));
        static PathGeometry FullBorderGeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
        "M0,20 a20,20 0 0,1 20,-20 h58 a20,20 0 0,1 20,20 v108 a20,20 0 0,1 -20,20 h-58 a20,20 0 0,1 -20,-20z"));
        RadialGradientBrush LevelBrush = GetLevelEllipseBrush();
        static PathGeometry Right1 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
            "M100,75 h-5 a20,20 0 0,0 -20,20 h25z"));
        static PathGeometry Right2 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
            "M100,60 h-5 a20,20 0 0,0 -20,20 h25z"));
        static PathGeometry Right3 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
            "M100,45 h-5 a20,20 0 0,0 -20,20 h25z"));
        static PathGeometry Right4 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
            "M100,30 h-5 a20,20 0 0,0 -20,20 h25z"));
        static PathGeometry RightLarge = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
            "M100,75 h-15 a20,20 0 0,0 -20,20 h35z"));
        static PathGeometry Left1 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
            "M0,75 h5 a20,20 0 0,1 20,20 h-25z"));
        static PathGeometry Left2 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
            "M0,60 h5 a20,20 0 0,1 20,20 h-25z"));
        static PathGeometry Left3 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
            "M0,45 h5 a20,20 0 0,1 20,20 h-25z"));
        static PathGeometry Left4 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
            "M0,30 h5 a20,20 0 0,1 20,20 h-25z"));
        static PathGeometry Left5 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
            "M0,15 h5 a20,20 0 0,1 20,20 h-25z"));
        static PathGeometry LeftLarge = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
            "M0,75 h15 a20,20 0 0,1 20,20 h-35z"));
        static PathGeometry Middle = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
            "M25,94 v-5 a15,15 0 0,1 15,-15 h20 a15,15 0 0,1 15,15 v5z"));
        static PathGeometry Spent1 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
            "M100,20 a15,15 0 0,0 -15,15 a15,15 0 0,0 15,15z"));
        static PathGeometry Spent2 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
            "M100,30 a15,15 0 0,0 -15,15 a15,15 0 0,0 15,15z"));

        Canvas canvas;
        Border NameBorder;
        public GameObjectImage() //Рандом
        {
            canvas = new Canvas();
            canvas.Background = Brushes.Gray;
            Child = canvas;
            Width = 200;
            canvas.Width = 100;
            canvas.Clip = StandardSizeGeom;
            Height = 220;
            canvas.Height = 110;
            canvas.ClipToBounds = true;

            Rectangle backimage = new Rectangle();
            backimage.Width = 50; backimage.Height = 50;
            backimage.Fill = Links.Brushes.RareImageBrush;
            canvas.Children.Add(backimage);
            Canvas.SetLeft(backimage, 25);
            Canvas.SetTop(backimage, 22.5);
            Rectangle topback = new Rectangle();
            topback.Width = 100; topback.Height = 10;
            topback.Fill = Brushes.Black;
            canvas.Children.Add(topback);
            Canvas.SetTop(topback, 4);
            SetTitle(Links.Interface("RandomTitle"));

            Rectangle lowback = new Rectangle();
            lowback.Width = 100; lowback.Height = 15;
            lowback.Fill = Brushes.Black;
            canvas.Children.Add(lowback);
            Canvas.SetTop(lowback, 93);
            SetName(Links.Interface("RandomText"));
        }
        public GameObjectImage(string text, Brush DarkBrush, Brush LightBrush, bool IsSmall)
        {
            canvas = new Canvas();
            canvas.Background = LightBrush;
            Child = canvas;
            Width = 200;
            canvas.Width = 100;
            canvas.Clip = StandardSizeGeom;
            Height = 220;
            canvas.Height = 110;

            Rectangle topback = new Rectangle();
            topback.Width = 100; topback.Height = 10;
            topback.Fill = DarkBrush;//Brushes.Black;
            canvas.Children.Add(topback);
            Canvas.SetTop(topback, 4);
            SetTitle(text);

            Rectangle lowback = new Rectangle();
            lowback.Width = 100; lowback.Height = 15;
            lowback.Fill = DarkBrush;//Brushes.Black;
            canvas.Children.Add(lowback);
            Canvas.SetTop(lowback, 93);
            SetName(Links.Interface("NoItem"));

            Path path = new Path();
            path.Stroke = DarkBrush;//Brushes.Black;
            path.StrokeThickness = 3;
            canvas.Children.Add(path);
            Canvas.SetLeft(path, 1);
            Canvas.SetTop(path, 1);
            path.Data = StandardBorderGeom;
            if (IsSmall)
            {
                canvas.Height = 80;
                Height = 160;
                path.Data = SmallBorderGeom;
                canvas.Clip = SmallSizeGeom;
                Canvas.SetTop(lowback, 65);
                Canvas.SetTop(NameBorder, 64);
                
             }
        }
        public ushort ScienceID;
        public GameObjectImage(GOImageType type, ushort scienceid)
        {
            ScienceID = scienceid;
            GameObjectImageParams param = new GameObjectImageParams(scienceid);
            canvas = new Canvas();
            canvas.Background = param.DarkBrush;
            Child = canvas;
            Width = 200;
            canvas.Width = 100;
            canvas.Height = 124;
            Height = 248;


            Rectangle backimage = new Rectangle();
            backimage.Width = param.BackImageSize; backimage.Height = param.BackImageSize;
            backimage.Fill = param.BackImage;
            canvas.Children.Add(backimage);
            Canvas.SetLeft(backimage, 25 + (50 - param.BackImageSize) / 2);
            Canvas.SetTop(backimage, 25 + (50 - param.BackImageSize) / 2);

            SetTitle(param.Title);
            SetName(param.Name);


            foreach (GameObjectOneParam oneparam in param.GameParams)
                AddGameParam(oneparam);
            SetLevel(param.Level);
            SetSize(param.Size);
            if (type==GOImageType.Hided && GSGameInfo.SciencesArray.Contains(scienceid)==false)
            {
                Rectangle rect = new Rectangle(); rect.Width = 100; rect.Height = 124;
                rect.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                rect.OpacityMask = param.DarkBrush;
                canvas.Children.Add(rect);
                Border hide = new Border(); hide.Width = 100; hide.Height = 30;
                hide.BorderBrush = Links.Brushes.SkyBlue;
                hide.BorderThickness = new Thickness(2);
                hide.RenderTransformOrigin = new Point(0.5, 0.5);
                hide.RenderTransform = new RotateTransform(-45);
                canvas.Children.Add(hide); Canvas.SetTop(hide, 50);

                TextBlock hidetext = Common.GetBlock(10, "НЕ ИССЛЕДОВАНО", Links.Brushes.SkyBlue, 100);
                hide.Child = hidetext;
            }
        }
        void AddDescription(string text)
        {
            ScrollViewer viewer = new ScrollViewer();
            viewer.Width = 90;
            viewer.Height = 30;
            viewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            canvas.Children.Add(viewer);
            Canvas.SetLeft(viewer, 5);
            Canvas.SetTop(viewer, 110);
            TextBlock tb = new TextBlock();
            tb.FontFamily = Links.Font;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.FontWeight = FontWeights.Bold;
            tb.TextAlignment = TextAlignment.Justify;
            tb.FontSize = 6;
            tb.Width = 90;
            tb.Text = text;
            viewer.Content = tb;

        }
        void AddSmallLeft1Param(GameObjectOneParam oneparam)
        {
            oneparam.Location = GOPLoc.Left3;
            AddGameParam(oneparam);
        }
        void AddGameParam(GameObjectOneParam oneparam)
        {
            Path path = new Path();
            //path.Stroke = Brushes.Black;
            //path.Fill = Brushes.Navy;
            //path.StrokeThickness = 1;
            //canvas.Children.Add(path);

            Rectangle image = new Rectangle();
            image.Width = 10; image.Height = 10;
            image.Fill = oneparam.Brush;
            canvas.Children.Add(image);

            TextBlock value = new TextBlock();
            int dy = 0;
            if (oneparam.Value == null)
            {
                foreach (Brush brush in oneparam.SecondBrush)
                    value.Inlines.Add(new InlineUIContainer(Common.GetEllipse(oneparam.Valuesize, brush)));
                dy = -4;
            }
            else
            {
                value.FontFamily = Links.Font;
                value.Text = oneparam.Value;
                value.FontSize = 7;
                value.Foreground = Brushes.White;
                value.FontWeight = FontWeights.Bold;
            }
            canvas.Children.Add(value);
            switch (oneparam.Location)
            {
                case GOPLoc.W1:
                    Canvas.SetLeft(image, 17); Canvas.SetTop(image, 22); image.Width = 8; image.Height = 8;
                    Canvas.SetLeft(value, 27); Canvas.SetTop(value, 22);
                    break;
                case GOPLoc.W2:
                    Canvas.SetLeft(image, 41); Canvas.SetTop(image, 22); image.Width = 8; image.Height = 8;
                    Canvas.SetLeft(value, 51); Canvas.SetTop(value, 22); 
                    break;
                case GOPLoc.W3:
                    Canvas.SetLeft(image, 66); Canvas.SetTop(image, 22); image.Width = 8; image.Height = 8;
                    Canvas.SetLeft(value, 76); Canvas.SetTop(value, 22);
                    break;
                case GOPLoc.Crit:
                    Canvas.SetLeft(image, 10); Canvas.SetTop(image, 75);
                    canvas.Children.Remove(value);
                    AddCritImage(oneparam.Value);
                    break;
                case GOPLoc.B1:
                    Canvas.SetLeft(image, 10); Canvas.SetTop(image, 107);
                    Canvas.SetLeft(value, 22); Canvas.SetTop(value, 109 + dy);
                    value.TextAlignment = TextAlignment.Left;
                    value.Width = 10;
                    break;
                case GOPLoc.B2:
                    Canvas.SetLeft(image, 30); Canvas.SetTop(image, 107);
                    Canvas.SetLeft(value, 42); Canvas.SetTop(value, 109 + dy);
                    value.TextAlignment = TextAlignment.Left;
                    value.Width = 10;
                    break;
                case GOPLoc.B3:
                    Canvas.SetLeft(image, 50); Canvas.SetTop(image, 107);
                    Canvas.SetLeft(value, 62); Canvas.SetTop(value, 109 + dy);
                    value.TextAlignment = TextAlignment.Left;
                    value.Width = 10;
                    break;
                case GOPLoc.B4:
                    Canvas.SetLeft(image, 70); Canvas.SetTop(image, 107);
                    Canvas.SetLeft(value, 82); Canvas.SetTop(value, 109 + dy);
                    value.TextAlignment = TextAlignment.Left;
                    value.Width = 10;
                    break;
                case GOPLoc.Middle:
                    path.Data = Middle;
                    Canvas.SetLeft(image, 30); Canvas.SetTop(image, 78);
                    Canvas.SetLeft(value, 40); Canvas.SetTop(value, 79);
                    //value.TextAlignment = TextAlignment.Center;
                    //value.Width = 40;
                    break;
                case GOPLoc.Left1:
                    path.Data = Left1;
                    Canvas.SetLeft(image, 10); Canvas.SetTop(image, 107);
                    Canvas.SetLeft(value, 22); Canvas.SetTop(value, 109);
                    value.TextAlignment = TextAlignment.Right;
                    value.Width = 10;
                    break;
                case GOPLoc.Left2:
                    path.Data = Left2;
                    image.Width = 8; image.Height = 8;
                    Canvas.SetLeft(image, 7); Canvas.SetTop(image, 90);
                    Canvas.SetLeft(value, 8); Canvas.SetTop(value, 95 + dy);
                    value.TextAlignment = TextAlignment.Right;
                    value.Width = 14;
                    break;
                case GOPLoc.Left3:
                    path.Data = Left3;
                    Canvas.SetLeft(image, 10); Canvas.SetTop(image, 38);
                    Canvas.SetLeft(value, 11); Canvas.SetTop(value, 47);
                    //value.TextAlignment = TextAlignment.Right;
                    //value.Width = 10;
                    break;
                case GOPLoc.Left4:
                    path.Data = Left4;
                    Canvas.SetLeft(image, 10); Canvas.SetTop(image, 55);
                    Canvas.SetLeft(value, 11); Canvas.SetTop(value, 64);
                    //value.TextAlignment = TextAlignment.Right;
                    //value.Width = 10;
                    break;
                case GOPLoc.Left5:
                    path.Data = Left5;
                    Canvas.SetLeft(image, 10); Canvas.SetTop(image, 107);
                    
                    Canvas.SetLeft(value, 22); Canvas.SetTop(value, 109);
                    value.TextAlignment = TextAlignment.Right;
                    value.Width = 10;
                    break;
                case GOPLoc.LeftLarge:
                    path.Data = LeftLarge;
                    Canvas.SetLeft(image, 10); Canvas.SetTop(image, 107);
                    value.FontSize = 6;
                    Canvas.SetLeft(value, 22); Canvas.SetTop(value, 109);
                    value.TextAlignment = TextAlignment.Right;
                    value.Width = 15;
                    break;
                case GOPLoc.Right1:
                    path.Data = Right1;
                    Canvas.SetLeft(image, 80); Canvas.SetTop(image, 107);
                    Canvas.SetLeft(value, 66); Canvas.SetTop(value, 109+dy);
                    break;
                case GOPLoc.Right2:
                    path.Data = Right2;
                    Canvas.SetLeft(image, 78); Canvas.SetTop(image, 68);
                    Canvas.SetLeft(value, 88); Canvas.SetTop(value, 70 + dy);
                    break;
                case GOPLoc.Right3:
                    path.Data = Right3;
                    Canvas.SetLeft(image, 81); Canvas.SetTop(image, 38);
                    Canvas.SetLeft(value, 82); Canvas.SetTop(value, 47);
                    break;
                case GOPLoc.Right4:
                    path.Data = Right4;
                    Canvas.SetLeft(image, 81); Canvas.SetTop(image, 55);
                    Canvas.SetLeft(value, 82); Canvas.SetTop(value, 64);
                    break;
                case GOPLoc.RightLarge:
                    path.Data = RightLarge;
                    Canvas.SetLeft(image, 70); Canvas.SetTop(image, 82);
                    Canvas.SetLeft(value, 82); Canvas.SetTop(value, 84+dy);
                    break;
                case GOPLoc.Spent1:
                    path.Data = Spent1;
                    image.Width = 8; image.Height = 8;
                    Canvas.SetLeft(image, 72); Canvas.SetTop(image, 72);
                    Canvas.SetLeft(value, 77); Canvas.SetTop(value, 78+dy);
                    value.TextAlignment = TextAlignment.Center;
                    value.Width = 10;
                    break;
                case GOPLoc.Spent2:
                    path.Data = Spent2;
                    Canvas.SetLeft(image, 88); Canvas.SetTop(image, 35);
                    Canvas.SetLeft(value, 75); Canvas.SetTop(value, 75 + dy);
                    value.TextAlignment = TextAlignment.Center;
                    value.Width = 10;
                    break;
            }
        }
        static LinearGradientBrush GetCritBrush()
        {
            LinearGradientBrush result = new LinearGradientBrush(); result.StartPoint = new Point(0.5, 1);
            result.EndPoint = new Point(0.5, 0);
            result.GradientStops.Add(new GradientStop(Colors.Red, 0.07));
            result.GradientStops.Add(new GradientStop(Colors.Orange, 0.214));
            result.GradientStops.Add(new GradientStop(Colors.Yellow, 0.357));
            result.GradientStops.Add(new GradientStop(Colors.Green, 0.5));
            result.GradientStops.Add(new GradientStop(Colors.SkyBlue, 0.643));
            result.GradientStops.Add(new GradientStop(Colors.Blue, 0.786));
            result.GradientStops.Add(new GradientStop(Colors.Purple, 0.929));
            return result;

        }
        static LinearGradientBrush CritBrush = GetCritBrush();
        void AddCritImage(string value)
        {
            double val = Int32.Parse(value) / 100.0;
            Rectangle rect1 = new Rectangle(); rect1.Width = 4; rect1.Height = 50;
            rect1.Fill = CritBrush;
            canvas.Children.Add(rect1); Canvas.SetLeft(rect1,13); Canvas.SetTop(rect1, 25);
            Rectangle rect2 = new Rectangle(); rect2.Width = 4;rect2.Height = 50;
            rect2.Stroke = Brushes.White; rect2.StrokeThickness = 0.5;
            canvas.Children.Add(rect2); Canvas.SetLeft(rect2, 13); Canvas.SetTop(rect2, 25);
            LinearGradientBrush OppMask = new LinearGradientBrush();
            OppMask.StartPoint = new Point(0.5, 1); OppMask.EndPoint = new Point(0.5, 0);
            OppMask.GradientStops.Add(new GradientStop(Colors.White, val));
            OppMask.GradientStops.Add(new GradientStop(Colors.Transparent, val));
            
            rect1.OpacityMask = OppMask;

        }
        void SetTitle(string text)
        {
            TextBlock tb = new TextBlock();
            tb.Foreground = Brushes.White;
            tb.FontSize = 8;
            canvas.Children.Add(tb);
            Canvas.SetLeft(tb, 24);
            Canvas.SetTop(tb, 6);
            tb.Text = text;
        }
        void SetName(string text)
        {
            NameBorder = new Border();
            NameBorder.Width = 62;
            NameBorder.Height = 15;
            canvas.Children.Add(NameBorder);
            Canvas.SetLeft(NameBorder, 28);
            Canvas.SetTop(NameBorder, 90);
            TextBlock tb = new TextBlock();
            tb.Foreground = Brushes.White;
            //if (text.Length > 25)
            //    tb.FontSize = 4.5;
            //else if (text.Length > 20)
            //    tb.FontSize = 5.5;
            //else 
                tb.FontSize = 5;
            NameBorder.Child = tb;
            //tb.Width = 90;
            tb.TextAlignment = TextAlignment.Center;
            //tb.Height = 17;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            //Canvas.SetTop(tb, 92);
            //Canvas.SetLeft(tb, 5);
            tb.Text = text;
        }
        void SetLevel(int level)
        {
            TextBlock tb = new TextBlock();
            tb.Foreground = Brushes.White;
            tb.FontSize = 8;
            canvas.Children.Add(tb);
            Canvas.SetLeft(tb, 7.5);
            Canvas.SetTop(tb, 5);
            tb.Text = level.ToString();
            tb.Width = 14;
            tb.TextAlignment = TextAlignment.Center;
            tb.FontWeight = FontWeights.Bold;
        }
        public void SetGroup(WeaponGroup group)
        {
            if (group == WeaponGroup.Any) return;
            Ellipse el = new Ellipse();
            el.Width = 20; el.Height = 20;
            el.Stroke = Brushes.Black;
            el.StrokeThickness = 2;
            canvas.Children.Add(el);
            Canvas.SetLeft(el, 78);
            Canvas.SetTop(el, 2);
            el.Fill = Links.Brushes.WeaponGroupBrush[group];
        }
        public void SetSize(ItemSize size)
        {
            if (size == ItemSize.Any) return;
            Rectangle el = new Rectangle();
            el.Width = 9; el.Height = 9;
            //el.Stroke = Brushes.Black;
            //el.StrokeThickness = 1;
            canvas.Children.Add(el);
            Canvas.SetLeft(el, 81);
            Canvas.SetTop(el, 7);
            if (size == ItemSize.Small)
                el.Fill = Links.Brushes.SmallImageBrush;
            else if (size == ItemSize.Medium)
                el.Fill = Links.Brushes.MediumImageBrush;
            else
                el.Fill = Links.Brushes.LargeImageBrush;
        }

        public static RadialGradientBrush GetLevelEllipseBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.8));
            brush.GradientStops.Add(new GradientStop(Colors.LightGray, 0));
            return brush;
        }
    }
    class GameObjectImageParams
    {
        public static SolidColorBrush DarkBrown = new SolidColorBrush(Color.FromRgb(40, 0, 0));
        public static SolidColorBrush LightBrown = new SolidColorBrush(Color.FromRgb(180, 130, 130));
        public static SolidColorBrush DarkGold = new SolidColorBrush(Color.FromRgb(90, 50, 12));
        public static SolidColorBrush LightGold = new SolidColorBrush(Color.FromRgb(255, 255, 155));
        public static SolidColorBrush DarkGreen = new SolidColorBrush(Color.FromRgb(0, 40, 0));
        public static SolidColorBrush LightGreen = new SolidColorBrush(Color.FromRgb(145, 255, 145));
        public static SolidColorBrush DarkBlue = new SolidColorBrush(Color.FromRgb(0, 0, 40));
        public static SolidColorBrush LightBlue = new SolidColorBrush(Color.FromRgb(145, 145, 255));
        public static SolidColorBrush DarkPurple = new SolidColorBrush(Color.FromRgb(40, 0, 40));
        public static SolidColorBrush LightPurle = new SolidColorBrush(Color.FromRgb(255, 190, 255));
        public static SolidColorBrush DarkRed = new SolidColorBrush(Color.FromRgb(40, 0, 0));
        public static SolidColorBrush LightRed = new SolidColorBrush(Color.FromRgb(255, 190, 190));
        public Brush BackImage;
        public int BackImageSize = 50;
        public string Title;
        public string Name;
        public int Level = 0;
        public WeaponGroup Group=WeaponGroup.Any;
        public ItemSize Size = ItemSize.Any;
        public Brush DarkBrush;
        public Brush LightBrush;
        public List<GameObjectOneParam> GameParams = new List<GameObjectOneParam>();
        public string Description;
        public bool CanSmall = false;
        public GameObjectImageParams(ushort id)
        {
            if (!Links.Science.GameSciences.ContainsKey(id))
            {
                BackImage = Links.Brushes.EmptyImageBrush;
                Title = "Error";
                Name = "Error";
                return;
            }
            GameScience science = Links.Science.GameSciences[id];
            Level = science.Level;
            switch (science.Type)
            {
                case Links.Science.EType.Building: SetBuildingType(id); break;
                case Links.Science.EType.ShipTypes: SetShipType(id); break;
                case Links.Science.EType.Generator: SetGenerator(id); break;
                case Links.Science.EType.Shield: SetShieldGenerator(id); break;
                case Links.Science.EType.Computer: SetAimComputer(id); break;
                case Links.Science.EType.Engine: SetEngine(id); break;
                case Links.Science.EType.Weapon: SetWeapon(id); break;
                case Links.Science.EType.Equipment: SetEquipment(id); break;
                case Links.Science.EType.Other: SetLandsCounts(id); break;
            }
        }
        public void SetLandsCounts(ushort id)
        {
            GameScience science = Links.Science.GameSciences[id];
            Title = Links.Interface("GOINewLand");
            Name = GameObjectName.GetNewLandsName(science);
            Description = GameObjectName.GetnewLandDescription();
            BackImage = Links.Brushes.LandIconBrush;
            DarkBrush = Brushes.DarkGreen;
            LightBrush = LightGreen;
            GameParams.Add(new GameObjectOneParam(Links.Brushes.LandIconBrush, "+1", GOPLoc.Left1));

        }
        public void SetEquipment(ushort id)
        {
            Equipmentclass equip = Links.EquipmentTypes[id];
            Title = Links.Interface("GOIEquipment");
            Name = GameObjectName.GetEquipmentName(equip);
            Description = GameObjectName.GetEquipmentDescription(equip);
            BackImage = Links.Brushes.EquipmentBrush;
            DarkBrush = Links.Brushes.Items.BorderBlue;
            LightBrush = Brushes.Gray;
            Size = equip.Size;
            CanSmall = true;
            if (equip.Type>=59)
                GameParams.Add(new GameObjectOneParam(Links.Brushes.Items.EnSpent, equip.Consume.ToString()+"%", GOPLoc.Spent1));
            else if (equip.Consume>0)
                GameParams.Add(new GameObjectOneParam(Links.Brushes.Items.EnSpent, equip.Consume.ToString(), GOPLoc.Spent1));
            switch (equip.Type)
            {
                case 0: GameParams.Add(new GameObjectOneParam(Pictogram.AccuracyEnergy, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Energy; BackImage = Pictogram.AccuracyEnergy; BackImageSize = 45; break;
                case 1:
                    GameParams.Add(new GameObjectOneParam(Pictogram.AccuracyPhysic, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Physic; BackImage = Pictogram.AccuracyPhysic; BackImageSize = 45; break;
                case 2:
                    GameParams.Add(new GameObjectOneParam(Pictogram.AccuracyIrregular, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Irregular; BackImage = Pictogram.AccuracyIrregular; BackImageSize = 45; break;
                case 3:
                    GameParams.Add(new GameObjectOneParam(Pictogram.AccuracyCyber, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Cyber; BackImage = Pictogram.AccuracyCyber; BackImageSize = 45; break;
                case 4:
                    GameParams.Add(new GameObjectOneParam(Pictogram.Accuracy, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.Accuracy; BackImageSize = 45; break;
                case 5:
                    GameParams.Add(new GameObjectOneParam(Pictogram.HealthBrush, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.HealthBrush; BackImageSize = 45;
                    break;
                case 6:
                    GameParams.Add(new GameObjectOneParam(Pictogram.RestoreHealth, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.RestoreHealth; BackImageSize = 45;
                    break;
                case 7:
                    GameParams.Add(new GameObjectOneParam(Pictogram.ShieldBrush, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.ShieldBrush; BackImageSize = 45;
                    break;
                case 8:
                    GameParams.Add(new GameObjectOneParam(Pictogram.RestoreShield, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.RestoreShield; BackImageSize = 45;
                    break;
                case 9:
                    GameParams.Add(new GameObjectOneParam(Pictogram.EnergyBrush, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.EnergyBrush; BackImageSize = 45;
                    break;
                case 10:
                    GameParams.Add(new GameObjectOneParam(Pictogram.RestoreEnergy, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.RestoreEnergy; BackImageSize = 45;
                    break;
                case 11:
                    GameParams.Add(new GameObjectOneParam(Pictogram.EvasionEnergy, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Energy; BackImage = Pictogram.EvasionEnergy; BackImageSize = 45; break;
                case 12:
                    GameParams.Add(new GameObjectOneParam(Pictogram.EvasionPhysic, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Physic; BackImage = Pictogram.EvasionPhysic; BackImageSize = 45; break;
                case 13:
                    GameParams.Add(new GameObjectOneParam(Pictogram.EvasionIrregular, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Irregular; BackImage = Pictogram.EvasionIrregular; BackImageSize = 45; break;
                case 14:
                    GameParams.Add(new GameObjectOneParam(Pictogram.EvasionCyber, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Cyber; BackImage = Pictogram.EvasionCyber; BackImageSize = 45; break;
                case 15:
                    GameParams.Add(new GameObjectOneParam(Pictogram.Evasion, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.Evasion; BackImageSize = 45; break;
                case 16:
                    GameParams.Add(new GameObjectOneParam(Pictogram.IgnoreEnergy, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Energy; BackImage = Pictogram.IgnoreEnergy; BackImageSize = 45; break;
                case 17:
                    GameParams.Add(new GameObjectOneParam(Pictogram.IgnorePhysic, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Physic; BackImage = Pictogram.IgnorePhysic; BackImageSize = 45; break;
                case 18:
                    GameParams.Add(new GameObjectOneParam(Pictogram.IgnoreIrregular, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Irregular; BackImage = Pictogram.IgnoreIrregular; BackImageSize = 45; break;
                case 19:
                    GameParams.Add(new GameObjectOneParam(Pictogram.IgnoreCyber, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Cyber; BackImage = Pictogram.IgnoreCyber; BackImageSize = 45; break;
                case 20:
                    GameParams.Add(new GameObjectOneParam(Pictogram.Ignore, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.Ignore; BackImageSize = 45; break;
                case 21:
                    GameParams.Add(new GameObjectOneParam(Pictogram.DamageEnergy, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Energy; BackImage = Pictogram.DamageEnergy; BackImageSize = 45; break;
                case 22:
                    GameParams.Add(new GameObjectOneParam(Pictogram.DamagePhysic, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Physic; BackImage = Pictogram.DamagePhysic; BackImageSize = 45; break;
                case 23:
                    GameParams.Add(new GameObjectOneParam(Pictogram.DamageIrregular, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Irregular; BackImage = Pictogram.DamageIrregular; BackImageSize = 45; break;
                case 24:
                    GameParams.Add(new GameObjectOneParam(Pictogram.DamageCyber, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Cyber; BackImage = Pictogram.DamageCyber; BackImageSize = 45; break;
                case 25:
                    GameParams.Add(new GameObjectOneParam(Pictogram.Damage, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.Damage; BackImageSize = 45; break;
                case 30:
                    GameParams.Add(new GameObjectOneParam(Pictogram.HealthBrushGroup, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.HealthBrushGroup; BackImageSize = 45;
                    break;
                case 31:
                    GameParams.Add(new GameObjectOneParam(Pictogram.RestoreHealthGroup, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.RestoreHealthGroup; BackImageSize = 45;
                    break;
                case 32:
                    GameParams.Add(new GameObjectOneParam(Pictogram.ShieldBrushGroup, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.ShieldBrushGroup; BackImageSize = 45;
                    break;
                case 33:
                    GameParams.Add(new GameObjectOneParam(Pictogram.RestoreShieldGroup, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.RestoreShieldGroup; BackImageSize = 45;
                    break;
                case 34:
                    GameParams.Add(new GameObjectOneParam(Pictogram.EnergyBrushGroup, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.EnergyBrushGroup; BackImageSize = 45;
                    break;
                case 35:
                    GameParams.Add(new GameObjectOneParam(Pictogram.RestoreEnergyGroup, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.RestoreEnergyGroup; BackImageSize = 45;
                    break;
                case 36:
                    GameParams.Add(new GameObjectOneParam(Pictogram.AccuracyEnergyGroup, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Energy; BackImage = Pictogram.AccuracyEnergyGroup; BackImageSize = 45; break;
                case 37:
                    GameParams.Add(new GameObjectOneParam(Pictogram.AccuracyPhysicGroup, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Physic; BackImage = Pictogram.AccuracyPhysicGroup; BackImageSize = 45; break;
                case 38:
                    GameParams.Add(new GameObjectOneParam(Pictogram.AccuracyIrregularGroup, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Irregular; BackImage = Pictogram.AccuracyIrregularGroup; BackImageSize = 45; break;
                case 39:
                    GameParams.Add(new GameObjectOneParam(Pictogram.AccuracyCyberGroup, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Cyber; BackImage = Pictogram.AccuracyCyberGroup; BackImageSize = 45; break;
                case 40:
                    GameParams.Add(new GameObjectOneParam(Pictogram.AccuracyGroup, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.AccuracyGroup; BackImageSize = 45; break;
                case 41:
                    GameParams.Add(new GameObjectOneParam(Pictogram.EvasionEnergyGroup, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Energy; BackImage = Pictogram.EvasionEnergyGroup; BackImageSize = 45; break;
                case 42:
                    GameParams.Add(new GameObjectOneParam(Pictogram.EvasionPhysicGroup, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Physic; BackImage = Pictogram.EvasionPhysicGroup; BackImageSize = 45; break;
                case 43:
                    GameParams.Add(new GameObjectOneParam(Pictogram.EvasionIrregularGroup, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Irregular; BackImage = Pictogram.EvasionIrregularGroup; BackImageSize = 45; break;
                case 44:
                    GameParams.Add(new GameObjectOneParam(Pictogram.EvasionCyberGroup, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Cyber; BackImage = Pictogram.EvasionCyberGroup; BackImageSize = 45; break;
                case 45:
                    GameParams.Add(new GameObjectOneParam(Pictogram.EvasionGroup, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.EvasionGroup; BackImageSize = 45; break;
                case 46:
                    GameParams.Add(new GameObjectOneParam(Pictogram.IgnoreEnergyGroup, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Energy; BackImage = Pictogram.IgnoreEnergyGroup; BackImageSize = 45; break;
                case 47:
                    GameParams.Add(new GameObjectOneParam(Pictogram.IgnorePhysicGroup, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Physic; BackImage = Pictogram.IgnorePhysicGroup; BackImageSize = 45; break;
                case 48:
                    GameParams.Add(new GameObjectOneParam(Pictogram.IgnoreIrregularGroup, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Irregular; BackImage = Pictogram.IgnoreIrregularGroup; BackImageSize = 45; break;
                case 49:
                    GameParams.Add(new GameObjectOneParam(Pictogram.IgnoreCyberGroup, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Cyber; BackImage = Pictogram.IgnoreCyberGroup; BackImageSize = 45; break;
                case 50:
                    GameParams.Add(new GameObjectOneParam(Pictogram.IgnoreGroup, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.IgnoreGroup; BackImageSize = 45; break;
                case 51:
                    GameParams.Add(new GameObjectOneParam(Pictogram.DamageEnergyGroup, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Energy; BackImage = Pictogram.DamageEnergyGroup; BackImageSize = 45; break;
                case 52:
                    GameParams.Add(new GameObjectOneParam(Pictogram.DamagePhysicGroup, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Physic; BackImage = Pictogram.DamagePhysicGroup; BackImageSize = 45; break;
                case 53:
                    GameParams.Add(new GameObjectOneParam(Pictogram.DamageIrregularGroup, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Irregular; BackImage = Pictogram.DamageIrregularGroup; BackImageSize = 45; break;
                case 54:
                    GameParams.Add(new GameObjectOneParam(Pictogram.DamageCyberGroup, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Cyber; BackImage = Pictogram.DamageCyberGroup; BackImageSize = 45; break;
                case 55:
                    GameParams.Add(new GameObjectOneParam(Pictogram.DamageGroup, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.DamageGroup; BackImageSize = 45; break;
                case 57:
                    GameParams.Add(new GameObjectOneParam(Pictogram.CargoBrush, equip.Value.ToString(), GOPLoc.Left1));
                    break;
                case 58:
                    GameParams.Add(new GameObjectOneParam(Pictogram.ColonyBrush, equip.Value.ToString(), GOPLoc.Left1));
                    break;
                case 59:
                    GameParams.Add(new GameObjectOneParam(Pictogram.ImmuneEnergy, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Energy; BackImage = Pictogram.ImmuneEnergy; BackImageSize = 45; break;
                case 60:
                    GameParams.Add(new GameObjectOneParam(Pictogram.ImmunePhysic, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Physic; BackImage = Pictogram.ImmunePhysic; BackImageSize = 45; break;
                case 61:
                    GameParams.Add(new GameObjectOneParam(Pictogram.ImmuneIrregular, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Irregular; BackImage = Pictogram.ImmuneIrregular; BackImageSize = 45; break;
                case 62:
                    GameParams.Add(new GameObjectOneParam(Pictogram.ImmuneCyber, equip.Value.ToString(), GOPLoc.Left1));
                    Group = WeaponGroup.Cyber; BackImage = Pictogram.ImmuneCyber; BackImageSize = 45; break;
                case 63:
                    GameParams.Add(new GameObjectOneParam(Pictogram.Immune, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.Immune; BackImageSize = 45; break;
                case 64:
                    GameParams.Add(new GameObjectOneParam(Pictogram.ImmuneEnergyGroup, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.ImmuneEnergyGroup;   BackImageSize = 45; break;
                case 65:
                    GameParams.Add(new GameObjectOneParam(Pictogram.ImmunePhysicGroup, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.ImmunePhysicGroup; BackImageSize = 45; break;
                case 66:
                    GameParams.Add(new GameObjectOneParam(Pictogram.ImmuneIrregularGroup, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.ImmuneIrregularGroup; BackImageSize = 45; break;
                case 67:
                    GameParams.Add(new GameObjectOneParam(Pictogram.ImmuneCyberGroup, equip.Value.ToString(), GOPLoc.Left1));
                    BackImage = Pictogram.ImmuneCyberGroup; BackImageSize = 45; break;


            }
        }
        public void SetWeapon(ushort id)
        {
            Weaponclass weapon = Links.WeaponTypes[id];
            Title = Links.Interface("GOIWeapon");
            Name = GameObjectName.GetWeaponName(weapon);
            Description = GameObjectName.GetWeaponDescription(weapon);
            BackImage = Links.Brushes.WeaponsBrushes[weapon.Type];
            Size = weapon.Size;
            Group = weapon.Group;
            GameParams.Add(new GameObjectOneParam(Links.Brushes.Items.EnSpent, weapon.Consume.ToString(), GOPLoc.Spent1));
            GameParams.Add(new GameObjectOneParam(Pictogram.ShieldDamage, weapon.ShieldDamage.ToString(), GOPLoc.Left1));
            GameParams.Add(new GameObjectOneParam(Pictogram.HealthDamage, weapon.HealthDamage.ToString(), GOPLoc.Right1));
            switch (weapon.Group)
            {
                case WeaponGroup.Energy:
                    DarkBrush = Links.Brushes.Items.BorderBlueW; LightBrush = LightBlue;
                    GameParams.Add(new GameObjectOneParam(Pictogram.AccuracyEnergy, Common.GetAccuracyText(Links.Modifiers[weapon.Type].Accuracy), GOPLoc.Left2));
                    break;
                case WeaponGroup.Physic:
                    DarkBrush = Links.Brushes.Items.BorderRedW ; LightBrush = LightRed;
                    GameParams.Add(new GameObjectOneParam(Pictogram.AccuracyPhysic, Common.GetAccuracyText(Links.Modifiers[weapon.Type].Accuracy), GOPLoc.Left2));
                    break;
                case WeaponGroup.Irregular:
                    DarkBrush = Links.Brushes.Items.BorderPinkW; LightBrush = LightPurle;
                    GameParams.Add(new GameObjectOneParam(Pictogram.AccuracyIrregular, Common.GetAccuracyText(Links.Modifiers[weapon.Type].Accuracy), GOPLoc.Left2));
                    break;
                case WeaponGroup.Cyber:
                    DarkBrush = Links.Brushes.Items.BorderGreenW; LightBrush = LightGreen;
                    GameParams.Add(new GameObjectOneParam(Pictogram.AccuracyCyber, Common.GetAccuracyText(Links.Modifiers[weapon.Type].Accuracy), GOPLoc.Left2));
                    break;
            }
            //DarkBrush = Links.Brushes.Items.BorderBlueW;
            GameParams.Add(new GameObjectOneParam(Pictogram.CriticalChance, Links.Modifiers[weapon.Type].Crit[weapon.Size].ToString(), GOPLoc.Crit));
            //GameParams.Add(new GameObjectOneParam(Pictogram.CriticalChance, Links.Modifiers[weapon.Type].Crit[weapon.Size].ToString() + "%", GOPLoc.Right2));
        }
        public void SetEngine(ushort id)
        {
            Engineclass engine = Links.EngineTypes[id];
            int sizedescr = engine.GetSizeDescription();
            Title = Links.Interface("GOIEngine");
            Name = GameObjectName.GetEngineName(engine);
            Description = GameObjectName.GetEngineDescription(engine);
            BackImage = Links.Brushes.Items.EngineBrushes[engine.Size];
            if (sizedescr % 10 == 0)
                DarkBrush = Links.Brushes.Items.BorderEngine;
            else
                DarkBrush = Links.Brushes.Items.BorderEngine2;
            LightBrush = LightPurle;
            Size = engine.Size;
            GameParams.Add(new GameObjectOneParam(Links.Brushes.Items.EnSpent, engine.Consume.ToString(), GOPLoc.Spent1));
            GameParams.Add(new GameObjectOneParam(Pictogram.EvasionEnergy, engine.EnergyEvasion.ToString(), GOPLoc.B1));
            GameParams.Add(new GameObjectOneParam(Pictogram.EvasionPhysic, engine.PhysicEvasion.ToString(), GOPLoc.B2));
            GameParams.Add(new GameObjectOneParam(Pictogram.EvasionIrregular, engine.IrregularEvasion.ToString(), GOPLoc.B3));
            GameParams.Add(new GameObjectOneParam(Pictogram.EvasionCyber, engine.CyberEvasion.ToString(), GOPLoc.B4));
            GameParams.Add(new GameObjectOneParam(null, Links.Brushes.Items.SpeedSize[engine.Size], 10, GOPLoc.Left2));
        }
        public void SetAimComputer(ushort id)
        {
            Computerclass comp = Links.ComputerTypes[id];
            Title = Links.Interface("GOIComputer");
            Name = GameObjectName.GetComputerName(comp);
            Description = GameObjectName.GetComputerDescription(comp);
            BackImage = Links.Brushes.Items.ComputerBrushesG.Get((WeaponGroup)comp.GetWeaponGroupValue(), comp.Size);
            //BackImage = Links.Brushes.Items.ComputerBrushes[comp.Size];
            
            LightBrush = LightRed;
            Size = comp.Size;
            GameParams.Add(new GameObjectOneParam(Links.Brushes.Items.EnSpent, comp.Consume.ToString(), GOPLoc.Spent1));
            int size = comp.GetType();
            switch (size)
            {
                case 0: GameParams.Add(new GameObjectOneParam(Pictogram.Accuracy, comp.Accuracy[0].ToString(), GOPLoc.Right1));
                    DarkBrush = Links.Brushes.Items.BorderGold;
                    break;
                case 1:
                case 5:
                    GameParams.Add(new GameObjectOneParam(Pictogram.AccuracyEnergy, comp.Accuracy[0].ToString(), GOPLoc.Right1));
                    GameParams.Add(new GameObjectOneParam(Pictogram.DamageEnergy, comp.Damage[0].ToString(), GOPLoc.Left1));
                    DarkBrush = Links.Brushes.Items.BorderBlue;
                    Group = WeaponGroup.Energy;
                    break;
                case 2:
                case 6:
                    GameParams.Add(new GameObjectOneParam(Pictogram.AccuracyPhysic, comp.Accuracy[1].ToString(), GOPLoc.Right1));
                    GameParams.Add(new GameObjectOneParam(Pictogram.DamagePhysic, comp.Damage[1].ToString(), GOPLoc.Left1));
                    DarkBrush = Links.Brushes.Items.BorderRed;
                    Group = WeaponGroup.Physic;
                    break;
                case 3:
                case 7:
                    GameParams.Add(new GameObjectOneParam(Pictogram.AccuracyIrregular, comp.Accuracy[2].ToString(), GOPLoc.Right1));
                    GameParams.Add(new GameObjectOneParam(Pictogram.DamageIrregular, comp.Damage[2].ToString(), GOPLoc.Left1));
                    DarkBrush = Links.Brushes.Items.BorderPink;
                    Group = WeaponGroup.Irregular;
                    break;
                case 4:
                case 8:
                    GameParams.Add(new GameObjectOneParam(Pictogram.AccuracyCyber, comp.Accuracy[3].ToString(), GOPLoc.Right1));
                    GameParams.Add(new GameObjectOneParam(Pictogram.DamageCyber, comp.Damage[3].ToString(), GOPLoc.Left1));
                    DarkBrush = Links.Brushes.Items.BorderGreen;
                    Group = WeaponGroup.Cyber;
                    break;
            }
            GameParams.Add(new GameObjectOneParam(null, Computerclass.JumpRangeBrushes[(int)comp.Size], 10,  GOPLoc.Left2));
        }
        public void SetShieldGenerator(ushort id)
        {
            Shieldclass shield = Links.ShieldTypes[id];
            int sizedescr = shield.GetSizeDescription();
            Title = Links.Interface("GOIShield");
            Name = GameObjectName.GetShieldName(shield);
            Description = GameObjectName.GetShieldDescription(shield);
            BackImage = Links.Brushes.Items.ShieldBrushes[shield.Size];
            //BackImage = Links.Brushes.ShieldBrush;
            if (sizedescr % 10 == 0)
                DarkBrush = Links.Brushes.Items.BorderShield;
            else
                DarkBrush = Links.Brushes.Items.BorderBlue;
            LightBrush = LightBlue;
            Size = shield.Size;
            GameParams.Add(new GameObjectOneParam(Pictogram.ShieldBrush, shield.Capacity.ToString(), GOPLoc.Left1));
            GameParams.Add(new GameObjectOneParam(Pictogram.RestoreShield, shield.Recharge.ToString(), GOPLoc.Right1));
            GameParams.Add(new GameObjectOneParam(Links.Brushes.Items.EnSpent, shield.Consume.ToString(), GOPLoc.Spent1));
            GameParams.Add(new GameObjectOneParam(null, Shieldclass.ProtectBrushes[(int)shield.Size], 10,  GOPLoc.Left2));

        }
        public void SetGenerator(ushort id)
        {
            Generatorclass generator = Links.GeneratorTypes[id];
            int sizedescr = generator.GetSizeDescription();
            Title = Links.Interface("GOIGenerator");
            Name = GameObjectName.GetGeneratorName(generator);
            Description = GameObjectName.GetGeneratorDescription(generator);
            BackImage = Links.Brushes.Items.GeneratorBrushes[generator.Size];
            //BackImage = Links.Brushes.GeneratorBrush;
            if (sizedescr % 10 == 0)
                DarkBrush = Links.Brushes.Items.BorderGenerator;
            else
                DarkBrush = Links.Brushes.Items.BorderGenerator2;
            LightBrush = LightGreen;
            Size = generator.Size;
            GameParams.Add(new GameObjectOneParam(Pictogram.EnergyBrush, generator.Capacity.ToString(), GOPLoc.Left1));
            GameParams.Add(new GameObjectOneParam(Pictogram.RestoreEnergy, generator.Recharge.ToString(), GOPLoc.Right1));
            GameParams.Add(new GameObjectOneParam(null, Generatorclass.TurnsBrushes[2 - (int)generator.Size], 10, GOPLoc.Left2));
            //GameParams.Add(new GameObjectOneParam(Generatorclass.TurnsBrushes[(int)generator.Size], ((int)generator.Size+1).ToString(), GOPLoc.Left2));
        }
        public void SetShipType(ushort id)
        {
            ShipTypeclass shiptype = Links.ShipTypes[id];
            Title = Links.Interface("GOIShipType");
            Name = shiptype.GetName();
            Description = GameObjectName.GetShipTypeDescription(shiptype);
            switch (shiptype.Type)
            {
                case ShipGenerator2Types.Scout: BackImage = Links.Brushes.Interface.SpeedShip; DarkBrush = Links.Brushes.Items.ShipSchema1; Size = ItemSize.Small; break;// return Links.Interface("Ship_Scout");
                case ShipGenerator2Types.Corvett: BackImage = Links.Brushes.Interface.SupportShip; DarkBrush = Links.Brushes.Items.ShipSchema1; Size = ItemSize.Small; break;// return Links.Interface("Ship_Corvette");
                case ShipGenerator2Types.Cargo: BackImage = Links.Brushes.Interface.ClearSchema; DarkBrush = Links.Brushes.Items.ShipSchema1; Size = ItemSize.Small; break;// return Links.Interface("Ship_Transport");
                case ShipGenerator2Types.Linkor: BackImage = Links.Brushes.Interface.TankShip; DarkBrush = Links.Brushes.Items.ShipSchema2; Size = ItemSize.Medium; break;// return Links.Interface("Ship_Battle");
                case ShipGenerator2Types.Frigate: BackImage = Links.Brushes.Interface.SupportShip; DarkBrush = Links.Brushes.Items.ShipSchema2; Size = ItemSize.Medium; break;// return Links.Interface("Ship_Frigate");
                case ShipGenerator2Types.Fighter: BackImage = Links.Brushes.Interface.SpeedShip; DarkBrush = Links.Brushes.Items.ShipSchema2; Size = ItemSize.Medium; break;// return Links.Interface("Ship_Fighter");
                case ShipGenerator2Types.Dreadnought: BackImage = Links.Brushes.Interface.DamageShip; DarkBrush = Links.Brushes.Items.ShipSchema2; Size = ItemSize.Medium; break;// return Links.Interface("Ship_Dreadnought");
                case ShipGenerator2Types.Devostator: BackImage = Links.Brushes.Interface.DamageShip; DarkBrush = Links.Brushes.Items.ShipSchema3; Size = ItemSize.Large; break;// return Links.Interface("Ship_Devostator");
                case ShipGenerator2Types.Warrior: BackImage = Links.Brushes.Interface.ClearSchema; DarkBrush = Links.Brushes.Items.ShipSchema3; Size = ItemSize.Large; break;// return Links.Interface("Ship_Warrior");
                case ShipGenerator2Types.Cruiser: BackImage = Links.Brushes.Interface.TankShip; DarkBrush = Links.Brushes.Items.ShipSchema3; Size = ItemSize.Large; break; // return Links.Interface("Ship_Cruiser");
                //default: return "";
            }
            LightBrush = Brushes.LightGray;
            //BackImage = Links.Brushes.ShipTypeBrushes[shiptype.WeaponCapacity];
            GameParams.Add(new GameObjectOneParam(Pictogram.HealthBrush, shiptype.Health.ToString(), GOPLoc.Left1));
            //GameParams.Add(new GameObjectOneParam(new VisualBrush(Pictogram.GetPict(Picts.Health, Target.None, false, "")), shiptype.Health.ToString(), GOPLoc.Left1));
            GameParams.Add(new GameObjectOneParam(Links.Brushes.EngineBrush, Links.Brushes.GetItemSizeBrush(shiptype.EngineSize), 8, GOPLoc.Right4));
            GameParams.Add(new GameObjectOneParam(Links.Brushes.ComputerBrush, Links.Brushes.GetItemSizeBrush(shiptype.ComputerSize), 8, GOPLoc.Right3));
            GameParams.Add(new GameObjectOneParam(Links.Brushes.ShieldBrush, Links.Brushes.GetItemSizeBrush(shiptype.ShieldSize), 8,  GOPLoc.Left4));
            GameParams.Add(new GameObjectOneParam(Links.Brushes.GeneratorBrush, Links.Brushes.GetItemSizeBrush(shiptype.GeneratorSize), 8, GOPLoc.Left3));
            GameParams.Add(new GameObjectOneParam(Pictogram.Ignore, shiptype.Armor.ToString(), GOPLoc.Right1));
            GameParams.Add(new GameObjectOneParam(null, Links.Brushes.ShipCreate, 10, GOPLoc.Left2));
            if (shiptype.WeaponCapacity==1)
            {
                GameParams.Add(new GameObjectOneParam(Links.Brushes.WeaponGroupBrush[shiptype.WeaponsParam[0].Group],
                Links.Brushes.GetItemSizeBrush(shiptype.WeaponsParam[0].Size), 8, GOPLoc.W2));
            }
            else if (shiptype.WeaponCapacity==2)
            {
                GameParams.Add(new GameObjectOneParam(Links.Brushes.WeaponGroupBrush[shiptype.WeaponsParam[0].Group],
                Links.Brushes.GetItemSizeBrush(shiptype.WeaponsParam[0].Size), 8, GOPLoc.W1));
                GameParams.Add(new GameObjectOneParam(Links.Brushes.WeaponGroupBrush[shiptype.WeaponsParam[1].Group],
               Links.Brushes.GetItemSizeBrush(shiptype.WeaponsParam[1].Size), 8, GOPLoc.W3));
            }
            else
            {
                GameParams.Add(new GameObjectOneParam(Links.Brushes.WeaponGroupBrush[shiptype.WeaponsParam[0].Group],
               Links.Brushes.GetItemSizeBrush(shiptype.WeaponsParam[0].Size), 8, GOPLoc.W1));
                GameParams.Add(new GameObjectOneParam(Links.Brushes.WeaponGroupBrush[shiptype.WeaponsParam[1].Group],
               Links.Brushes.GetItemSizeBrush(shiptype.WeaponsParam[1].Size), 8, GOPLoc.W2));
                GameParams.Add(new GameObjectOneParam(Links.Brushes.WeaponGroupBrush[shiptype.WeaponsParam[2].Group],
               Links.Brushes.GetItemSizeBrush(shiptype.WeaponsParam[2].Size), 8, GOPLoc.W3));
            }
            /*
            GameParams.Add(new GameObjectOneParam(Links.Brushes.WeaponGroupBrush[shiptype.WeaponsParam[0].Group], 
                Links.Brushes.GetItemSizeBrush(shiptype.WeaponsParam[0].Size), GOPLoc.Left3));
            if (shiptype.WeaponCapacity>1)
                GameParams.Add(new GameObjectOneParam(Links.Brushes.WeaponGroupBrush[shiptype.WeaponsParam[1].Group],
                Links.Brushes.GetItemSizeBrush(shiptype.WeaponsParam[1].Size), GOPLoc.Left4));
            if (shiptype.WeaponCapacity == 3)
                GameParams.Add(new GameObjectOneParam(Links.Brushes.WeaponGroupBrush[shiptype.WeaponsParam[2].Group],
                Links.Brushes.GetItemSizeBrush(shiptype.WeaponsParam[2].Size), GOPLoc.Left5));
            */
    List<Brush> equips = new List<Brush>();
            for (int i = 0; i < shiptype.EquipmentCapacity; i++)
                equips.Add(Links.Brushes.GetItemSizeBrush(shiptype.EquipmentsSize[i]));
            GameParams.Add(new GameObjectOneParam(Links.Brushes.EquipmentBrush, equips.ToArray(), 7, GOPLoc.Middle));
        }
        public void SetBuildingType(ushort id)
        {
            GSBuilding building = Links.Buildings[id];
            Title = Links.Interface("GOIBuilding");
            Name = GameObjectName.GetBuildingsName(building);
            Description = GameObjectName.GetBuildingDescription(building);
            DarkBrush = Links.Brushes.Items.BorderBuilding;
            LightBrush = LightBrown;
            GameParams.Add(new GameObjectOneParam(Links.Brushes.BuildingSizeBrush, building.Size.ToString()+"  ", GOPLoc.Left2));
            switch (building.Type)
            {
                case BuildingType.LiveS:
                    BackImage = Links.Brushes.Buildings.BuildLive1;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), "+"+(building.GetSeconParamValue()/1000.0).ToString("### ##0.#"), GOPLoc.Right1));
                    break;
                case BuildingType.LiveM:
                    BackImage = Links.Brushes.Buildings.BuildLive2;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), "+" + (building.GetFirstParamValue() / 1000.0).ToString("### ##0.#"), GOPLoc.LeftLarge));
                    break;
                case BuildingType.LiveL:
                    BackImage = Links.Brushes.Buildings.BuildLive3;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), "+" + (building.GetSeconParamValue() / 1000.0).ToString("### ##0.#"), GOPLoc.Right1));
                    break;
                case BuildingType.MoneyS:
                    BackImage = Links.Brushes.Buildings.BuildMoney1;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###") + "%", GOPLoc.Right1));                    
                    break;
                case BuildingType.MoneyM:
                    BackImage = Links.Brushes.Buildings.BuildMoney2;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###") + "%", GOPLoc.LeftLarge));
                    break;
                case BuildingType.MoneyL:
                    BackImage = Links.Brushes.Buildings.BuildMoney3;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###") + "%", GOPLoc.Right1));
                    break;
                case BuildingType.MetalS:
                    BackImage = Links.Brushes.Buildings.BuildMetal1;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###") + "%", GOPLoc.Right1));
                    break;
                case BuildingType.MetalM:
                    BackImage = Links.Brushes.Buildings.BuildMetal2;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###") + "%", GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###"), GOPLoc.Right1));
                    break;
                case BuildingType.MetalL:
                    BackImage = Links.Brushes.Buildings.BuildMetal3;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###") + "%", GOPLoc.Right1));
                    break;
                case BuildingType.MetalCapS:
                    BackImage = Links.Brushes.Buildings.BuildMetCap1;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###") + "%", GOPLoc.Right1));
                    break;
                case BuildingType.MetalCapM:
                    BackImage = Links.Brushes.Buildings.BuildMetCap2;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###"), GOPLoc.Right1));
                    break;
                case BuildingType.MetalCapL:
                    BackImage = Links.Brushes.Buildings.BuildMetCap3;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###") + "%", GOPLoc.Right1));
                    break;
                case BuildingType.ChipS:
                    BackImage = Links.Brushes.Buildings.BuildChip1;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###") + "%", GOPLoc.Right1));
                    break;
                case BuildingType.ChipM:
                    BackImage = Links.Brushes.Buildings.BuildChip2;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###")+"%", GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###"), GOPLoc.Right1));
                    break;
                case BuildingType.ChipL:
                    BackImage = Links.Brushes.Buildings.BuildChip3;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###") + "%", GOPLoc.Right1));
                    break;
                case BuildingType.ChipCapS:
                    BackImage = Links.Brushes.Buildings.BuildChipCap1;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###") + "%", GOPLoc.Right1));
                    break;
                case BuildingType.ChipCapM:
                    BackImage = Links.Brushes.Buildings.BuildChipCap2;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###"), GOPLoc.Right1));
                    break;
                case BuildingType.ChipCapL:
                    BackImage = Links.Brushes.Buildings.BuildChipCap3;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###") + "%", GOPLoc.Right1));
                    break;
                case BuildingType.AntiS:
                    BackImage = Links.Brushes.Buildings.BuildAnti1;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###") + "%", GOPLoc.Right1));
                    break;
                case BuildingType.AntiM:
                    BackImage = Links.Brushes.Buildings.BuildAnti2;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###")+"%", GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###"), GOPLoc.Right1));
                    break;
                case BuildingType.AntiL:
                    BackImage = Links.Brushes.Buildings.BuildAnti3;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###") + "%", GOPLoc.Right1));
                    break;
                case BuildingType.AntiCapS:
                    BackImage = Links.Brushes.Buildings.BuildAntiCap1;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###") + "%", GOPLoc.Right1));
                    break;
                case BuildingType.AntiCapM:
                    BackImage = Links.Brushes.Buildings.BuildAntiCap2;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###"), GOPLoc.Right1));
                    break;
                case BuildingType.AntiCapL:
                    BackImage = Links.Brushes.Buildings.BuildAntiCap3;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###")+ "%", GOPLoc.Right1));
                    break;
                case BuildingType.RepairS:
                    BackImage = Links.Brushes.Buildings.BuildRepair1;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###") + "%", GOPLoc.Right1));
                    break;
                case BuildingType.RepairM:
                    BackImage = Links.Brushes.Buildings.BuildRepair2;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###")+"%", GOPLoc.LeftLarge));
                    break;
                case BuildingType.RepairL:
                    BackImage = Links.Brushes.Buildings.BuildRepair3;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    GameParams.Add(new GameObjectOneParam(building.GetSecondParamBrush(), building.GetSeconParamValue().ToString("### ###") + "%", GOPLoc.Right1));
                    break;
                case BuildingType.Portal:
                    BackImage = Links.Brushes.Buildings.BuildWar0;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    break;
                case BuildingType.Radar:
                    BackImage = Links.Brushes.Buildings.BuildWar1;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    break;
                case BuildingType.DataCenter:
                    BackImage = Links.Brushes.Buildings.BuildWar2;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    break;
                case BuildingType.Education:
                    BackImage = Links.Brushes.Buildings.BuildWar3;
                    GameParams.Add(new GameObjectOneParam(building.GetFirstParamBrush(), building.GetFirstParamValue().ToString("### ###"), GOPLoc.LeftLarge));
                    break;


            }

        }
    }
    public enum GOPLoc { Right1, Right2, Right3, Right4, RightLarge, Left1, Left2, Left3, Left4, Left5, LeftLarge, Spent1, Spent2, Middle, B1, B2, B3, B4, Crit, W1, W2, W3}
    class GameObjectOneParam
    {
        public Brush Brush;
        public string Value;
        public GOPLoc Location;
        public Brush[] SecondBrush;
        public int Valuesize;
        public GameObjectOneParam (Brush brush, string value, GOPLoc location)
        {
            Brush = brush;
            Value = value;
            Location = location;
        }
        public GameObjectOneParam(Brush brush, Brush value, int size, GOPLoc location)
        {
            Brush = brush;
            SecondBrush = new Brush[] { value };
            Location = location;
            Valuesize = size;
        }
        public GameObjectOneParam(Brush brush, Brush[] value, int size, GOPLoc location)
        {
            Brush = brush;
            SecondBrush = value;
            Location = location;
            Valuesize = size;
        }
    }
}
