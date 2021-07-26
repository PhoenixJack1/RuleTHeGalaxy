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

namespace Client
{
    enum PilotsListMode {Academy, Ship, Select, Clear }
    class PilotsListPanel:Border
    {
        public static PilotsListPanel Panel;
        PilotsListMode Mode;
        public PilotsListPanel(PilotsListMode mode)
        {
            Mode = mode;
            Panel = this;
            BorderBrush = Brushes.Black;
            BorderThickness = new Thickness(2);
            Width = 1200;
            Height = 600;
            Grid grid = new Grid();
            Child = grid;
            Background = Brushes.LightGray;
            grid.RowDefinitions.Add(new RowDefinition()); grid.RowDefinitions[0].Height = new GridLength(50);
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition()); grid.RowDefinitions[2].Height = new GridLength(50);

            Label TitleLabel = new Label();
            TitleLabel.Style = Links.TextStyle;
            grid.Children.Add(TitleLabel);
            TitleLabel.Content = Links.Interface("Barracks");
            TitleLabel.HorizontalAlignment = HorizontalAlignment.Center;

            Grid ButtonPanel = new Grid();
            ButtonPanel.ColumnDefinitions.Add(new ColumnDefinition());
            ButtonPanel.ColumnDefinitions.Add(new ColumnDefinition());
            ButtonPanel.ColumnDefinitions.Add(new ColumnDefinition());
            ButtonPanel.ColumnDefinitions.Add(new ColumnDefinition());
            ButtonPanel.Width = 1200;
            grid.Children.Add(ButtonPanel);
            Grid.SetRow(ButtonPanel, 2);

            InterfaceButton Cancel = AddButtonToPanel(ButtonPanel, "Cancel", 3);
            Cancel.PreviewMouseDown += Cancel_Click;

            
            ScrollViewer PilotsViewer = new ScrollViewer();
            PilotsViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            PilotsViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            grid.Children.Add(PilotsViewer);
            Grid.SetRow(PilotsViewer, 1);

            WrapPanel PilotsPanel = new WrapPanel();
            PilotsPanel.Width = 1100;
            PilotsViewer.Content = PilotsPanel;
            
            foreach (GSPilot pilot in GSGameInfo.FreePilots)
                PilotsPanel.Children.Add(new PilotsImage(pilot, Mode, null));
             
            
        }
        
        void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }
        InterfaceButton AddButtonToPanel(Grid panel, string text, int column)
        {
            InterfaceButton btn = new InterfaceButton(200,50,7,20);
            btn.SetText(Links.Interface(text));
            panel.Children.Add(btn);
            Grid.SetColumn(btn, column);
            return btn;
        }
    }
    class PilotsImage : Border
    {
        PilotsListMode Mode;
        public GSPilot Pilot;
        //static Brush JapBrush = GetJapBrush();
        //static Brush RusBrush = GetRusBrush();
        //static Brush AmerBrush = GetAmerBrush();
        Canvas canvas;
        GSShip Ship;
        public PilotsImage(GSPilot pilot, PilotsListMode mode, GSShip ship)
        {
            Mode = mode;
            Ship = ship;
            Pilot = pilot;
            CreateSimpleImage();
            this.PreviewMouseDown += new MouseButtonEventHandler(PilotsImage_PreviewMouseDown);
        }
        public Viewbox GetScaledSize(int size)
        {
            Viewbox vbx = new Viewbox(); vbx.Width = size; vbx.Height = size;
            vbx.Child = this;
            return vbx;
        }
        void PilotsImage_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            List<GSButton> Buttons = new List<GSButton>();
            if (Mode == PilotsListMode.Academy)
            {
                Buttons.Add(new GSButton(250, 50, Links.Interface("Dismiss"), Dismiss_Click, Pilot));
                Buttons.Add(GSButton.GetCancelButton());
            }
            else if (Mode == PilotsListMode.Ship)
            {
                if (Pilot == null)

                    Buttons.Add(new GSButton(250, 50, Links.Interface("PutPilot"), PutPilot_Click, null));
                else if (Ship.Health != 0 && Ship.Fleet == null)
                    Buttons.Add(new GSButton(250, 50, Links.Interface("RemovePilot"), RemovePilot_Click, this));
                Buttons.Add(GSButton.GetCancelButton());
            }
            else if (Mode == PilotsListMode.Select)
            {
                PutPilotToShip();
                return;
            }
            else return;
            //Buttons.Add(GSButton.GetCancelButton());
            Links.Controller.RoundButtons.Create(this, Buttons, false);
            Links.Controller.RoundButtons.Place();
        }
        void PutPilotToShip()
        {
            string result = Events.PutPilotToShip(Links.Helper.GetShip().ID, Pilot);
            if (result == "")
            {
                Links.Controller.PopUpCanvas.Remove();
                Gets.GetTotalInfo("После события по размещению пилота в корабле");
                Links.Controller.ShipsCanvas.Select();
            }
            else Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(result));
        }
        void PutPilot_Click(object sender, RoutedEventArgs e)
        {
            Links.Helper.SetShip(Ship);
            PilotsListPanel panel = new PilotsListPanel(PilotsListMode.Select);
            Links.Controller.RoundButtons.Remove();
            Links.Controller.PopUpCanvas.Place(panel);

        }
        void RemovePilot_Click(object sender, RoutedEventArgs e)
        {
            Links.Controller.RoundButtons.Remove();
            string result = Events.MovePilotFromShip(Ship.ID);
            if (result == "")
            {
                Links.Controller.PopUpCanvas.Remove();
                Gets.GetTotalInfo("После события по убиранию пилота из корабля");
                Links.Controller.ShipsCanvas.Select();
            }
            else Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(result), true);

        }
        static void Dismiss_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            GSPilot pilot = (GSPilot)btn.Tag;
            string result = Events.DismissPilot(pilot);
            Links.Controller.RoundButtons.Remove();
            if (result == "")
            {
                Links.Controller.PopUpCanvas.Remove();
                Gets.GetTotalInfo("После события по увольнению пилота");
                Links.Controller.PopUpCanvas.Place(new PilotsListPanel(PilotsListMode.Academy));
            }
            else Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(result), true);
        }
        void CreateSimpleImage()
        {
            Width = 200; Height = 200;
            Margin = new Thickness(2);
            BorderBrush = Brushes.Black;
            BorderThickness = new Thickness(2);
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Gray, 0.3));
            brush.GradientStops.Add(new GradientStop(Colors.DarkGray, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Gray, 0.8));
            Background = brush;
            if (Pilot == null)
            {
                Background = Common.GetLinearBrush(Colors.Black, Colors.White, Colors.Black);
                Common.PutBlock(20, this, Links.Interface("NoPilot"));
                return;
            }
            canvas = new Canvas();
            canvas.Width = 192; canvas.Height = 192;
            Child = canvas;
            SetFlagImage();
            Rectangle rating = GetRatingImage(Pilot.Specialization, Pilot.Talent, Pilot.Rang, 40);
            canvas.Children.Add(rating);
            Canvas.SetLeft(rating, 2);
            Canvas.SetTop(rating, 34);
            //SetRatings(Pilot.Specialization, Pilot.Talent, Pilot.Rang);
            PutText(Pilot.GetNick(), 26, 140, 60, 5);
            PutText(Pilot.GetName() + " " + Pilot.GetSurname(), 20, 150, 45, 45);
            PutText(Pilot.GetCity() + " " + Pilot.GetAge(), 18, 190, 5, 75);
            Rectangle exprect = GetPilotExp(Pilot);
            canvas.Children.Add(exprect);
            Canvas.SetLeft(exprect, 5);
            Canvas.SetTop(exprect, 100);
            //PutText(String.Format("{0} {1} ({2})", Pilot.Level, Links.Interface("Level"), Pilot.Exp*255+Pilot.Exp2), 20, 190, 5, 100);

            SecondaryEffect[] effects = Pilot.GetEffects();
            UIElement element = Abilities.GetElement(Pilot.Specialization);
            element.RenderTransformOrigin = new Point(0.5, 0.5);
            element.RenderTransform = new ScaleTransform(1.4, 1.4);
            element.Opacity = 0.7;
            canvas.Children.Add(element);
            Canvas.SetLeft(element, 81);
            Canvas.SetTop(element, 150);

            Label accurlbl = new Label();
            accurlbl.Style = Links.MediumTextStyle;
            accurlbl.Content = effects[0].Value.ToString();
            Grid accuregrid = new Grid();
            canvas.Children.Add(accuregrid);
            accuregrid.Width = 192;
            accuregrid.Children.Add(accurlbl);
            accurlbl.HorizontalAlignment = HorizontalAlignment.Center;
            accurlbl.VerticalAlignment = VerticalAlignment.Center;
            accuregrid.Height = 36;
            Canvas.SetTop(accuregrid, 146);
            accurlbl.FontSize = 24;
            accurlbl.FontWeight = FontWeights.Bold;
            for (byte i = 1; i < effects.Length; i++)
                PutEffect(effects[i], i);
            
        }
        public void PutEffect(SecondaryEffect effect, byte pos)
        {
            UIElement element = Abilities.GetElement(effect.Type);
            canvas.Children.Add(element);
            TextBlock text = Common.GetBlock(22, effect.Value.ToString());
            canvas.Children.Add(text);
            switch (pos)
            {
                case 1: Canvas.SetLeft(element, 2); Canvas.SetTop(element, 130); Canvas.SetLeft(text, 35); Canvas.SetTop(text, 130); break;
                case 2: Canvas.SetLeft(element, 158); Canvas.SetTop(element, 130); Canvas.SetLeft(text, 130); Canvas.SetTop(text, 130); break;
                case 3: Canvas.SetLeft(element, 2); Canvas.SetTop(element, 160); Canvas.SetLeft(text, 35); Canvas.SetTop(text, 160); break;
                case 4: Canvas.SetLeft(element, 158); Canvas.SetTop(element, 160); Canvas.SetLeft(text, 130); Canvas.SetTop(text, 160); break;
            }
           
        }
        public void PutText(string text, int textsize, int width, int left, int top)
        {
            TextBlock tb = new TextBlock();
            tb.FontFamily = Links.Font;
            tb.FontSize = textsize;
            tb.Text = text;
            canvas.Children.Add(tb);
            tb.FontWeight = FontWeights.Bold;
            tb.Foreground = Brushes.Black;
            tb.Width = width;
            tb.Height = 40;
            tb.TextWrapping = TextWrapping.NoWrap;
            tb.TextTrimming = TextTrimming.CharacterEllipsis;
            //tb.TextWrapping = TextWrapping.Wrap;
            tb.TextAlignment = TextAlignment.Center;
            tb.VerticalAlignment = VerticalAlignment.Center;
            Canvas.SetLeft(tb, left);
            Canvas.SetTop(tb, top);
        }
        public void SetFlagImage()
        {
            Rectangle rect = new Rectangle();
            rect.Width = 50;
            rect.Height = 30;
            rect.Stroke = Brushes.Black;
            rect.StrokeThickness = 1;
            canvas.Children.Add(rect);
            Canvas.SetLeft(rect, 2);
            Canvas.SetTop(rect, 2);
            rect.Fill = GetFlagImage(Pilot.RaceID, Pilot.NameID);
            /*switch (Pilot.RaceID)
            {
                case 0:
                case 1: rect.Fill = Links.Brushes.RusFlagBrush; break;
                case 2:
                case 3: rect.Fill = Links.Brushes.JapFlagBrush; break;
                case 4:
                case 5: rect.Fill = Links.Brushes.USAFlagBrush; break;
                case 100: rect.Fill = Links.Brushes.ZipImageBrush; break;
                case 200: switch (StoryLine2.StoryPilotsInfo[Pilot.NameID].Country)
                    {
                        case 0: rect.Fill = Links.Brushes.RusFlagBrush; break;
                        case 1: rect.Fill = Links.Brushes.JapFlagBrush; break;
                        case 2: rect.Fill = Links.Brushes.USAFlagBrush; break;
                        case 100: rect.Fill = Links.Brushes.ZipImageBrush; break;
                    } break;
                    
            }*/
        }
        public static Brush GetFlagImage(byte raceid, byte nameid)
        {
            switch (raceid)
            {
                case 0:
                case 1: return Links.Brushes.RusFlagBrush; 
                case 2:
                case 3: return Links.Brushes.JapFlagBrush;
                case 4:
                case 5: return Links.Brushes.USAFlagBrush;
                case 100: return Links.Brushes.ZipImageBrush;
                case 200:
                    switch (StoryLine2.StoryPilotsInfo[nameid].Country)
                    {
                        case 0: return Links.Brushes.RusFlagBrush;
                        case 1: return Links.Brushes.JapFlagBrush;
                        case 2: return Links.Brushes.USAFlagBrush;
                        case 100: return Links.Brushes.ZipImageBrush;
                    }
                    break;

            }
            return null;
        }
        static Pen WhitePen = new Pen(Brushes.White, 1);
        static SolidColorBrush TransBrush = new SolidColorBrush(Color.FromArgb(1, 255, 255, 255));

        public static Rectangle GetRatingImage(byte Spec, byte Talent, byte Rang, int size)
        {
            Rectangle rect = new Rectangle();
            rect.Width = size; rect.Height = size;
            DrawingGroup group = new DrawingGroup();

            Brush brush;
            switch (Spec)
            {
                case 0: brush = BlueRatingBrush; break;
                case 1: brush = RedRatingBrush; break;
                case 2: brush = PurplaRatingBrush; break;
                case 3: brush = GreenRatingBrush; break;
                default: brush = GoldRatingBrush; break;
            }
            switch (Rang)
            {
                case 0:
                    group.Children.Add(GetRatingImage(brush, Talent, 11, 8));
                    break;
                case 1:
                    group.Children.Add(GetRatingImage(brush, Talent, 2, 8));
                    group.Children.Add(GetRatingImage(brush, Talent, 21, 8));
                    break;
                case 2:
                    group.Children.Add(GetRatingImage(brush, Talent, 2, 19));
                    group.Children.Add(GetRatingImage(brush, Talent, 21, 19));
                    group.Children.Add(GetRatingImage(brush, Talent, 11, 2));
                    break;
                case 3:
                    group.Children.Add(GetRatingImage(brush, Talent, 2, 19));
                    group.Children.Add(GetRatingImage(brush, Talent, 21, 19));
                    group.Children.Add(GetRatingImage(brush, Talent, 2, 2));
                    group.Children.Add(GetRatingImage(brush, Talent, 21, 2));
                    break;
                case 4:
                    group.Children.Add(GetRatingImage(brush, Talent, 12, 9));
                    group.Children.Add(GetFiveStarLines(brush));
                    break;
            }
            group.Children.Add(new GeometryDrawing(TransBrush, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h40 v40 h-40z"))));    
            rect.Fill = new DrawingBrush(group);
            return rect;
        }
        
        static GeometryDrawing GetFiveStarLines(Brush brush)
        {
            PathGeometry geom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M1,1 l7,7 M1,38 l7,-7 M38,1 l-7,7 M38,38 l-7,-7"));
            return new GeometryDrawing(null, new Pen(brush, 3), geom);
        }
        
        static Pen RatingPen = new Pen(Brushes.Black, 0.2);
        static LinearGradientBrush GetRatingBrush(Color color)
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(color, 0.3));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.4));
            brush.GradientStops.Add(new GradientStop(color, 0.7));
            brush.StartPoint = new Point(0, 0.5);
            return brush;
        }
        static LinearGradientBrush RedRatingBrush = GetRatingBrush(Colors.Red);
        static LinearGradientBrush BlueRatingBrush = GetRatingBrush(Colors.Blue);
        static LinearGradientBrush PurplaRatingBrush = GetRatingBrush(Colors.Purple);
        static LinearGradientBrush GreenRatingBrush = GetRatingBrush(Colors.Green);
        static LinearGradientBrush GoldRatingBrush = GetRatingBrush(Colors.Gold);
        static GeometryDrawing GetRatingImage(Brush brush, byte Talent, int x, int y)
        {
            PathGeometry geom;
            switch (Talent)
            {
                case 0: geom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(String.Format("M{0},{1} h12 v8 h-12 z", 1 + x, 5 + y))); break;
                case 1: geom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(String.Format("M{0},{1} l7,-4 l7,4 v4 l-7,-4 l-7,4 z", x, 10+y))); break;
                case 2: geom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                    String.Format("M{0},{1} l1.6,6 l5,1.2 l-4.2,1.6 l1.6,6 l-4.2,-3.7 l-4.2,3.7 l1.6,-6 l-4.2,-1.6 l5,-1.2z", 8 + x, y))); break;
                default: geom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                    String.Format("M{0},{1} a1,1 0 1,1 1,0 l3,7 l3,-9 a1,1 0 1,1 1,0 l3,9 l3,-7 a1,1 0 1,1 1,0 v12 h-15z", x, 5 + y))); break;
            }
            return new GeometryDrawing(brush, RatingPen, geom);
        }
       
        StackPanel AddElementToPanel(SecondaryEffect effect, int row, int column, HorizontalAlignment alignment)
        {
            StackPanel result = new StackPanel();
            result.Orientation = Orientation.Horizontal;
            UIElement element = Abilities.GetElement(effect.Type);
            Label lbl = new Label();
            lbl.Content = effect.Value.ToString();
            lbl.Style = Links.MediumTextStyle;
            lbl.FontSize = 24;
            lbl.VerticalAlignment = VerticalAlignment.Center;
            if (alignment == HorizontalAlignment.Left)
            {
                result.Children.Add(element);
                result.Children.Add(lbl);
            }
            else
            {
                result.Children.Add(lbl);
                result.Children.Add(element);
            }
            result.HorizontalAlignment = alignment;
            result.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(result, row);
            Grid.SetColumn(result, column);
            return result;
        }

        void AddLabel(int row, int column, Grid grid, string text, int fontsize, int columnspan)
        {
            Label lbl = new Label();
            grid.Children.Add(lbl);
            Grid.SetRow(lbl, row);
            Grid.SetColumn(lbl, column);
            Grid.SetColumnSpan(lbl, columnspan);
            lbl.Content = text;
            lbl.VerticalAlignment = VerticalAlignment.Center;
            lbl.Style = Links.MediumTextStyle;
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            lbl.FontSize = fontsize;
        }
        static GlyphTypeface gt = new GlyphTypeface(new Uri("pack://application:,,,/resources/Agru.ttf", UriKind.Absolute), StyleSimulations.BoldSimulation);
        public static Rectangle GetPilotExp(GSPilot pilot)
        {
            ushort exp = (ushort)(pilot.Exp * 255 + pilot.Exp2);
            double expbase = 5 + pilot.Rang * 5;
            byte level = (byte)Math.Log(1 + exp / expbase, 2);
            ushort minexp, freeexp;
            if (level >= 10)
            {
                level = 10;
                minexp = (ushort)(expbase * ((1 - Math.Pow(2, 9)) * (-1)));
                freeexp = (ushort)(exp - minexp);
            }
            else
            {
                minexp = (ushort)(expbase * ((1 - Math.Pow(2, level)) * (-1)));
                freeexp = (ushort)(exp - minexp);

            }
            byte nextlevel = (byte)(level + 1);
            double percent = 1;
            ushort nextlevelexp;
            if (nextlevel < 11)
            {
                nextlevelexp = (ushort)(expbase * Math.Pow(2, nextlevel - 1));
                percent = freeexp / (double)nextlevelexp;
            }
            else
            {
                nextlevelexp = (ushort)(expbase * Math.Pow(2, 9));
            }
            Rectangle rect = new Rectangle();
            rect.Width = 180; rect.Height = 25;
            Pen pen = new Pen(Brushes.SkyBlue, 2);
            PathGeometry expgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,8 h180 v20 h-180 z"));
            LinearGradientBrush expbrush = new LinearGradientBrush();
            expbrush.StartPoint = new Point(0, 0.5); expbrush.EndPoint = new Point(1, 0.5);
            expbrush.GradientStops.Add(new GradientStop(Colors.DarkOrange, 0));
            expbrush.GradientStops.Add(new GradientStop(Colors.DarkOrange, percent));
            expbrush.GradientStops.Add(new GradientStop(Colors.Transparent, percent));
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(expbrush, pen, expgeom));
            for (byte i = 0; i < level; i++)
            {
                string s = String.Format("M{0},8 v-8 h10 v8 z", 4 + i * 18);
                PathGeometry geom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(s));
                group.Children.Add(new GeometryDrawing(Brushes.Gold, null, geom));
            }
            if (level == 0)
            {
                PathGeometry geom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M4,8 v-8 h8 v8 z"));
                group.Children.Add(new GeometryDrawing(Brushes.Transparent, null, geom));
            }

            string textstring = String.Format("{0}/{1}", freeexp, nextlevelexp);
            char[] chars = textstring.ToCharArray();
            double[] baselines = new double[chars.Length];
            for (int i = 0; i < baselines.Length; i++)
                baselines[i] = 7;
            Point baseLine = new Point(90 - chars.Length * 3.5, 26);
            ushort[] gtchars = new ushort[chars.Length];
            for (int i = 0; i < gtchars.Length; i++)
                gtchars[i] = gt.CharacterToGlyphMap[chars[i]];
            GlyphRun theGlyphRun = new GlyphRun(gt, 0, false, 16, gtchars,
            baseLine, baselines, null, null, null, null, null, null);
            GlyphRunDrawing gDrawing = new GlyphRunDrawing(Brushes.White, theGlyphRun);
            group.Children.Add(gDrawing);

            rect.Fill = new DrawingBrush(group);
            return rect;

        }
    }

}
