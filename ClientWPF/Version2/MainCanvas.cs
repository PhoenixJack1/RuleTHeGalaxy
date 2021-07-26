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
using System.Windows.Threading;

namespace Client
{
    class MainCanvas:Canvas
    {
        public ResourcePanel NamePanel;
        public ResourcePanel MoneyPanel;
        public ResourcePanel MetalPanel;
        public ResourcePanel ChipsPanel;
        public ResourcePanel AntiPanel;
        public CapacityPanel MetalCap, ChipsCap, AntiCap;
        public Border LowBorder;
        public Rectangle Chat;
        //public MenuButtons Menu;
        Path MenuLoad, MenuSave, MenuOptions, MenuExit;
        Canvas MenuCanvas;
        ScaleTransform DateScale;
        Rectangle EndTurnBlank;
        public MainCanvas()
        {
            Width = 1920; Height = 1080;
            Background = Brushes.Black;// GetBrush("Background");

            PutRectangle(593, 166, 665, 0, this, "Main1", 10);

            PutRectangle(959, 204, 481, 0, this, "Main2", 10);

            PutRectangle(1920, 184, 0, 0, this, "Main3", 10);
            
            PutRectangle(1920, 177, 0, 0, this, "Main4", 10);

            PutRectangle(1713, 111, 91, 969, this, "Main5", 10);

            //PutRectangle(1046, 280, 478, 0, this, "Menu", 20);
            //Rectangle Flash = PutRectangle(1046, 247, 478, 0, this, "Menu");
            //Flash.Clip = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
            //    "M0,92 h76 a70,30 0 0,0 70,25 l35,42 h6 v12 h10 l15,10 h15 v5 a10,10 0 0,0 7,4 h67" +
            //    "v-8 h368 v8 h70 a10,10 0 0,0 5,-4 v-5 h15 l15,-10 h2 v-12 h8 l35,-42  a90,30 0 0,0 70,-27 h46 v130 l-27,27 h-883 l-25,-25z"));
            //Canvas.SetZIndex(Flash, ZIndexes.Flash);
            Rectangle options = PutRectangle(102, 95, 1820, 987, this, "options", 30);
            options.PreviewMouseDown += Options_MouseDown;

            MenuCanvas = new Canvas();
            MenuCanvas.Visibility = Visibility.Hidden;
            Children.Add(MenuCanvas); MenuCanvas.Width = 352; MenuCanvas.Height = 392;
            Canvas.SetLeft(MenuCanvas, 1570); Canvas.SetTop(MenuCanvas, 607); Canvas.SetZIndex(MenuCanvas, 30);
            MenuCanvas.Clip = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h352 v350 h-100 v42 h-252 z"));
            PutRectangle(352, 392, 0, 0, MenuCanvas, "Buttons", 0);
            MenuLoad = GetMenuPath("M95,112 l25,-9 v5 l147,-43 v-5 l33,-9 l-10,60 l-32,9 v-4 l-144,38 v3 l-25,7 z", MenuCanvas);
            MenuLoad.PreviewMouseDown += MenuLoad_PreviewMouseDown;
            MenuSave = GetMenuPath("M87,172 l25,-8 v6 l143,-36 v-5 l32,-7 l-9,59 l-31,6 v-4 l-141,31 v3 l-25,7 z", MenuCanvas);
            MenuSave.PreviewMouseDown += MenuSave_PreviewMouseDown;
            MenuOptions = GetMenuPath("M81,233 l25,-6 v6 l139,-30 v-5 l32,-7 l-9,59 l-30,6 v-5 l-138,24 v4 l-25,4 z", MenuCanvas);
            MenuOptions.PreviewMouseDown += MenuOptions_PreviewMouseDown;
            MenuExit = GetMenuPath("M76,293 l23,-4 v5 l137,-23 v-3 l32,-6 l-9,57 l-30,4 v-5 l-135,18 v3 l-24,4 z", MenuCanvas);
            MenuExit.PreviewMouseDown += MenuExit_PreviewMouseDown;
            MenuCanvas.Opacity = 0;
            MenuCanvas.IsEnabled = false;

            //LeftArrow = PutRectangle(89, 93, 703, 94, this, "Left", 30);
            //LeftArrow.PreviewMouseDown += LeftArrow_PreviewMouseDown;
            //RightArrow = PutRectangle(89, 93, 1136, 94, this, "Right", 30);
            //RightArrow.PreviewMouseDown += RightArrow_PreviewMouseDown;
            
            Chat = new Rectangle(); Chat.Width = 80; Chat.Height = 50;
            Children.Add(Chat); Canvas.SetZIndex(Chat, ZIndexes.Envelope);
            Chat.Fill = Links.Brushes.Interface.Chat;
            Canvas.SetLeft(Chat, 910); Canvas.SetTop(Chat, 920); //Chat.Opacity = 1;
            Chat.Visibility = Visibility.Hidden;
            Chat.PreviewMouseDown += Chat_PreviewMouseDown;
            /*
            ParameterButton parbtn = new ParameterButton();
            Children.Add(parbtn);
            Canvas.SetLeft(parbtn, 1150);
            Canvas.SetZIndex(parbtn, 20);
            parbtn.AddText(0, Links.Interface("ParametersB"));
            parbtn.AddText(1, "Save");
            parbtn.AddText(4, Links.Interface("Exit"));
            parbtn.r11.PreviewMouseDown  += Parameters_PreviewMouseDown;
            parbtn.r21.PreviewMouseDown += Save_Single;
            parbtn.r51.PreviewMouseDown += Exit;
            */
            Rectangle rr = new Rectangle(); rr.Width = 10; rr.Height = 2; rr.Fill = Brushes.Red;
            rr.RenderTransformOrigin = new Point(0.5, 0.5); RotateTransform rrrot = new RotateTransform();
            rr.RenderTransform = rrrot;
            DoubleAnimation rranim = new DoubleAnimation(0, 360, TimeSpan.FromSeconds(3));
            rranim.RepeatBehavior = RepeatBehavior.Forever;
            rrrot.BeginAnimation(RotateTransform.AngleProperty, rranim);
            Children.Add(rr); Canvas.SetLeft(rr, 1910); Canvas.SetTop(rr, 1070);
            Canvas.SetZIndex(rr, 255);

            Canvas EndTurnCanvas = new Canvas(); EndTurnCanvas.Width = 426; EndTurnCanvas.Height = 70;
            Children.Add(EndTurnCanvas); Canvas.SetLeft(EndTurnCanvas, 733); Canvas.SetTop(EndTurnCanvas, 982);
            Canvas.SetZIndex(EndTurnCanvas, 35);
            //PutRectangle(250, 99, 1550, 980, this, "TextPanel", 30);
            EndTurnBlank = PutRectangle(426, 70, 0, 0, EndTurnCanvas, "ResBlankRed", 30);
            EndTurnCanvas.Clip = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M16,0 h397 l13,27 l-32,43 h-360 l-32,-43z"));

            NamePanel = new ResourcePanel(null); Canvas.SetTop(NamePanel, 0);
            EndTurnCanvas.Children.Add(NamePanel); Canvas.SetLeft(NamePanel, 90);
            NamePanel.Width = 250;
            Canvas.SetZIndex(NamePanel, 30);
            EndTurnCanvas.PreviewMouseDown += EndTurnCanvas_PreviewMouseDown;
            EndTurnCanvas.RenderTransformOrigin = new Point(0.5, 0.5);
            EndTurnCanvas.RenderTransform = DateScale = new ScaleTransform(1, 1);
            //NamePanel.SetText("PhoenixJack");

            MoneyPanel = new ResourcePanel(null);
            //MoneyPanel.SetText("1.456.563");
            Children.Add(MoneyPanel); Canvas.SetLeft(MoneyPanel, 150);

            MetalPanel = new ResourcePanel(null);
            //MetalPanel.SetText("23.568");
            Children.Add(MetalPanel); Canvas.SetLeft(MetalPanel, 1180);
            MetalCap = new CapacityPanel();
            Children.Add(MetalCap); Canvas.SetLeft(MetalCap, 1180);

            ChipsPanel = new ResourcePanel(null);
            //.SetText("12.235");
            Children.Add(ChipsPanel); Canvas.SetLeft(ChipsPanel, 450);
            ChipsCap = new CapacityPanel();
            Children.Add(ChipsCap); Canvas.SetLeft(ChipsCap, 660);

            AntiPanel = new ResourcePanel(null);
            //AntiPanel.SetText("236");
            Children.Add(AntiPanel); Canvas.SetLeft(AntiPanel, 1495);
            AntiCap = new CapacityPanel();
            Children.Add(AntiCap); Canvas.SetLeft(AntiCap, 1495);
 
            LowBorder = new Border(); 
            LowBorder.Width = 1920;
            LowBorder.Height = 1080;
            Children.Add(LowBorder);
            
            text1 = new MovingText("Государство", 190, 49, 40, MenuElements.Nation); Children.Add(text1); Canvas.SetLeft(text1, 70); Canvas.SetTop(text1, 33);
            text2 = new MovingText("Колонии", 190, 49, 40, MenuElements.Colony); Children.Add(text2); Canvas.SetLeft(text2, 353); Canvas.SetTop(text2, 33);
            text3 = new MovingText("Галактика", 356, 70, 70, MenuElements.Galaxy); Children.Add(text3); Canvas.SetLeft(text3, 781); Canvas.SetTop(text3, 88);
            text4 = new MovingText("Флоты", 190, 49, 40, MenuElements.Fleets); Children.Add(text4); Canvas.SetLeft(text4, 1359); Canvas.SetTop(text4, 33);
            text5 = new MovingText("Наука", 190, 49, 40, MenuElements.Science); Children.Add(text5); Canvas.SetLeft(text5, 1642); Canvas.SetTop(text5, 34);

            EndTimer = new DispatcherTimer();
            EndTimer.Interval = TimeSpan.FromSeconds(1.0);
            EndTimer.Tick += EndTimer_Tick;
        }

        

        MenuElements CurElement = MenuElements.Galaxy;
        MovingText text1, text2, text3, text4, text5;
        public void ElementSelect(MenuElements element)
        {
            if (element==MenuElements.Nation)
            {
                if (CurElement == MenuElements.Nation)
                    Links.Controller.SelectPanel(GamePanels.Galaxy, SelectModifiers.None);
                else
                    Links.Controller.SelectPanel(GamePanels.Nation, SelectModifiers.None);
            }
            else if (element==MenuElements.Colony)
            {
                if (CurElement == MenuElements.Colony)
                    Links.Controller.SelectPanel(GamePanels.Galaxy, SelectModifiers.None);
                else
                {
                    Colonies.CurrentPlanet = null;
                    //Colonies.CurrentAvanpost = null; Colonies.CurrentLand = null;
                    Links.Controller.SelectPanel(GamePanels.Colonies, SelectModifiers.None);
                }
            }
            else if (element==MenuElements.Fleets)
            {
                if (CurElement == MenuElements.Fleets)
                    Links.Controller.SelectPanel(GamePanels.Galaxy, SelectModifiers.None);
                else
                    Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.None);
            }
            else if (element==MenuElements.Science)
            {
                if (CurElement == MenuElements.Science)
                    Links.Controller.SelectPanel(GamePanels.Galaxy, SelectModifiers.None);
                else
                    Links.Controller.SelectPanel(GamePanels.ScienceCanvas, SelectModifiers.None);
            }
            else if (element==MenuElements.Galaxy)
            {
                if (CurElement == MenuElements.Galaxy)
                    Links.Controller.SelectPanel(GamePanels.Galaxy, SelectModifiers.None);
                else if (CurElement == MenuElements.Nation)
                    Links.Controller.SelectPanel(GamePanels.Nation, SelectModifiers.None);
                else if (CurElement == MenuElements.Colony)
                {
                    Colonies.CurrentPlanet = null;
                    //Colonies.CurrentAvanpost = null; Colonies.CurrentLand = null;
                    Links.Controller.SelectPanel(GamePanels.Colonies, SelectModifiers.None);
                }
                else if (CurElement == MenuElements.Fleets)
                    Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.None);
                else if (CurElement == MenuElements.Science)
                    Links.Controller.SelectPanel(GamePanels.ScienceCanvas, SelectModifiers.None);
            }
        }
        public void ElementChange(MenuElements element)
        {

            if (MovingText.IsAnimated == true) return;
            if (element == MenuElements.Nation)
            {
                if (CurElement == MenuElements.Nation) return;
                text1.Change("Галактика", true, false);
                text3.Change("Государство", false, true);
                if (CurElement == MenuElements.Colony)
                    text2.Change("Колонии", false, true);
                else if (CurElement == MenuElements.Fleets)
                    text4.Change("Флоты", false, true);
                else if (CurElement == MenuElements.Science)
                    text5.Change("Наука", false, true);
                CurElement = MenuElements.Nation;
               
            }
            else if (element == MenuElements.Colony)
            {
                if (CurElement == MenuElements.Colony) return;
                text2.Change("Галактика", true, false);
                text3.Change("Колонии", false, true);
                    if (CurElement == MenuElements.Nation)
                        text1.Change("Государство", false, true);
                    else if (CurElement == MenuElements.Fleets)
                        text4.Change("Флоты", false, true);
                    else if (CurElement == MenuElements.Science)
                        text5.Change("Наука", false, true);
                    CurElement = MenuElements.Colony;
                
            }
            else if (element == MenuElements.Fleets)
            {
                if (CurElement == MenuElements.Fleets) return;    
                    text4.Change("Галактика", true, false);
                    text3.Change("Флоты", false, true);
                    if (CurElement == MenuElements.Nation)
                        text1.Change("Государство", false, true);
                    else if (CurElement == MenuElements.Colony)
                        text2.Change("Колонии", false, true);
                    else if (CurElement == MenuElements.Science)
                        text5.Change("Наука", false, true);
                    CurElement = MenuElements.Fleets;                
            }
            else if (element == MenuElements.Science)
            { 
                    text5.Change("Галактика", true, false);
                    text3.Change("Наука", false, true);
                    if (CurElement == MenuElements.Nation)
                        text1.Change("Государство", false, true);
                    else if (CurElement == MenuElements.Colony)
                        text2.Change("Колонии", false, true);
                    else if (CurElement == MenuElements.Fleets)
                        text4.Change("Флоты", false, true);
                    CurElement = MenuElements.Science;
            }
            else if (element==MenuElements.Galaxy)
            {
                text3.Change("Галактика", true, true);
                if (CurElement == MenuElements.Nation)
                    text1.Change("Государство", false, true);
                else if (CurElement == MenuElements.Colony)
                    text2.Change("Колонии", false, true);
                else if (CurElement == MenuElements.Fleets)
                    text4.Change("Флоты", false, true);
                else if (CurElement == MenuElements.Science)
                    text5.Change("Наука", false, true);
                CurElement = MenuElements.Galaxy;

            }

        }
    
        class MovingText : Viewbox
        {
            Canvas canvas;
            Border border;
            Border border1;
            static SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(1, 255, 255, 255));
            public static bool IsAnimated = false;
            MenuElements Baseelement;
            double curwidth, curheight, curfont;
            public MovingText(string title, int width, int height, int font, MenuElements baseelement)
            {
                Baseelement = baseelement;
                curwidth = width; curheight = height * 1.3; curfont = font;
                Width = width; Height = height; Stretch = Stretch.Fill; Canvas.SetZIndex(this, 50);
                canvas = new Canvas();  canvas.Width = width; canvas.Height = height * 1.3;
                Child = canvas;
                border = new Border(); border.Width = width; border.Height = canvas.Height;
                border.Background = brush;
                TextBlock block = Common.GetBlock(font, title, Brushes.White, width);
                canvas.Children.Add(border);
                border.Child = block;
                PreviewMouseDown += MovingText_PreviewMouseDown;
            }

            private void MovingText_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            {
                e.Handled = true;
                Links.Controller.MainCanvas.ElementSelect(Baseelement);
            }

            public void Change(string text, bool leftdirection, bool force)
            {
                if (force == false && IsAnimated == true) return;
                IsAnimated = true;
                border1 = new Border(); border1.Width = curwidth; border1.Height = curheight;
                border1.Background = brush;
                TextBlock block = Common.GetBlock((int)curfont, text, Brushes.White, (int)curwidth);
                canvas.Children.Add(border1);
                border1.Child = block;
                LinearGradientBrush gb1 = new LinearGradientBrush();
                GradientStop gr1, gr2, gr3, gr4;
                gb1.GradientStops.Add(gr1 = new GradientStop(Colors.White, 0));
                gb1.GradientStops.Add(gr2 = new GradientStop(Colors.Transparent, 0));
                LinearGradientBrush gb2 = new LinearGradientBrush();
                gb2.GradientStops.Add(gr3 = new GradientStop(Colors.White, 1));
                gb2.GradientStops.Add(gr4 = new GradientStop(Colors.Transparent, 1));
                if (leftdirection)
                {
                    gb1.StartPoint = new Point(1, 0.5); gb1.EndPoint = new Point(0, 0.5);
                    gb2.StartPoint = new Point(0, 0.5); gb2.EndPoint = new Point(1, 0.5);
                }
                else
                {
                    gb1.StartPoint = new Point(0, 0.5); gb1.EndPoint = new Point(1, 0.5);
                    gb2.StartPoint = new Point(1, 0.5); gb2.EndPoint = new Point(0, 0.5);
                }
                DoubleAnimation anim1 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
                DoubleAnimation anim2 = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
                gr1.BeginAnimation(GradientStop.OffsetProperty, anim1);
                gr2.BeginAnimation(GradientStop.OffsetProperty, anim1);
                gr3.BeginAnimation(GradientStop.OffsetProperty, anim2);
                anim2.Completed += Anim2_Completed;
                gr4.BeginAnimation(GradientStop.OffsetProperty, anim2);
                border.OpacityMask = gb1;
                border1.OpacityMask = gb2;
            }

            private void Anim2_Completed(object sender, EventArgs e)
            {
                canvas.Children.Remove(border);
                border = border1;
                IsAnimated = false;
            }
        }
        private void EndTurnCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (EndTimer.IsEnabled==false)
            {
                bool endturnresult = EndTurn(false);
                if (endturnresult == false) return;
                StartEndTurn();
            }
            else
            {
                StopAutoChange();
            }
        }
        public void StartEndTurn()
        {
            EndTurnBlank.Fill = GetBrush("ResBlank");
            EndTimer.Start();
            GSGameInfo.SetServerTime();
            //DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.1));
            //anim.Completed += HideDateAnim_Completed;
            //DateScale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
        }
        private void HideDateAnim_Completed(object sender, EventArgs e)
        {
            //GSGameInfo.SetServerTime();
            //DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.1));
            //DateScale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
        }

        private void Options_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = (Rectangle)sender;
            rect.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform scale = new ScaleTransform(1, 1);
            rect.RenderTransform = scale;
            DoubleAnimation anim = new DoubleAnimation(1, 0.9, TimeSpan.FromSeconds(0.05));
            anim.AutoReverse = true;
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);

            DoubleAnimationUsingKeyFrames show = new DoubleAnimationUsingKeyFrames();
            show.Duration = TimeSpan.FromSeconds(0.5);
            show.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromPercent(0.0)));
            show.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.3)));
            show.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromPercent(0.6)));
            show.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.75)));
            show.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromPercent(0.9)));
            show.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.95)));
            show.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromPercent(1.0)));
            show.Completed += ShowMenu_Completed;
            MenuCanvas.BeginAnimation(Canvas.OpacityProperty, show);
            MenuCanvas.Visibility = Visibility.Visible;
        }

        private void ShowMenu_Completed(object sender, EventArgs e)
        {
            MenuCanvas.IsEnabled = true;
            DoubleAnimationUsingKeyFrames hide = new DoubleAnimationUsingKeyFrames();
            hide.Duration = TimeSpan.FromSeconds(3);
            hide.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromPercent(0)));
            hide.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromPercent(0.9)));
            hide.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(1)));
            hide.Completed += HideMenu_Completed;
            MenuCanvas.BeginAnimation(Canvas.OpacityProperty, hide);
        }

        private void HideMenu_Completed(object sender, EventArgs e)
        {
            MenuCanvas.IsEnabled = false;
            MenuCanvas.Visibility = Visibility.Hidden;
        }

        private void MenuExit_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Commands.Stop_Game();
        }

        private void MenuOptions_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.PopUpCanvas.Place(new Parameters(GlobalWindows.Main));
        }

        private void MenuSave_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void MenuLoad_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        Path GetMenuPath(string way, Canvas canvas)
        {
            Path path = new Path();
            path.Fill = Links.Brushes.Transparent;
            path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(way));
            canvas.Children.Add(path);
            return path;
        }
       /* private void RightArrow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Menu.MoveRight(true);
        }

        private void LeftArrow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Menu.MoveLeft(true);
        }*/

        Rectangle PutRectangle(int width, int height, int left, int top, Canvas canvas, string brush, byte z)
        {
            Rectangle rect = new Rectangle(); rect.Width = width; rect.Height = height;
            canvas.Children.Add(rect); Canvas.SetLeft(rect, left); Canvas.SetTop(rect, top);
            rect.Fill = GetBrush(brush);
            Canvas.SetZIndex(rect, z);
            return rect;
        }
        ImageBrush GetBrush(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Int/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Int/{0}.png", text), UriKind.Relative)));
        }
        private void Save_Single(object sender, MouseButtonEventArgs e)
        {
            LocalServer.SaveGame();
        }

        DispatcherTimer EndTimer = new DispatcherTimer();

        /*private void VipGear_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            bool endturnresult = EndTurn(false);
            if (endturnresult == false) return;
            EndTimer.Start();
            
        }*/
        public void StopAutoChange()
        {
            EndTimer.Stop();
            EndTurnBlank.Fill = GetBrush("ResBlankRed");
        }
        private void EndTimer_Tick(object sender, EventArgs e)
        {
            bool endturnresult = EndTurn(false);
            if (endturnresult == false)
            {
                StopAutoChange();
            }
            else
            {
                StartEndTurn();
            }
        }
        public bool EndTurn(bool force)
        {
            if (force == false)
            {
                foreach (GSFleet fleet in GSGameInfo.Fleets.Values)
                {
                    if (fleet.Target != null && fleet.Target.Order == FleetOrder.InBattle)
                    {
                        Links.Controller.PopUpCanvas.Place(new BattleInProcessMessage());
                        return false;
                    }
                }
            }
            else
            {
                for (;;)
                {
                    bool hasswitches = false;
                    foreach (GSFleet fleet in GSGameInfo.Fleets.Values)
                    {
                        if (fleet.Target != null && fleet.Target.Order == FleetOrder.InBattle)
                        {
                            Battle battle = Common.GetFleetLastBattle(fleet);
                            if (battle != null) Events.SetBattleToAutoMode(battle);
                            hasswitches = true;
                        }
                    }
                    if (hasswitches == true) Gets.GetTotalInfo("После конца усиленного хода");
                    else break;
                }
            }
            LocalServer.MakeTurn();
            Gets.GetResources();
            Gets.GetTotalInfo("После конца хода");
            NoticeBorder.CheckNotice();
            if (Links.Controller.CurPanel == GamePanels.Colonies)
                LandChangerBase.EndTurn();
            //Colonies.InnerRefresh();

            else if (Links.Controller.CurPanel != GamePanels.ScienceCanvas)
                Links.Controller.SelectPanel(Links.Controller.CurPanel, SelectModifiers.None);
            return true;
        }



        private void Chat_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Chat.Visibility = Visibility.Hidden;
            Links.Controller.PopUpCanvas.Place(new NoticeBorder(), true);
        }
      
        public static class ZIndexes
        {
            public static byte MainRectangle = 0;
            public static byte LeftCorner = 10;
            public static byte RightCorner = 10;
            public static byte VipGear = 20;
            public static byte ResourcePanel = 20;
            public static byte Buttons = 30;
            public static byte LowBorder = 50;
            public static byte DownComp = 40;
            public static byte MenuButtons = 70;
            public static byte Flash = 60;
            public static byte Envelope = 110;
        }
    }
    class CapacityPanel:Viewbox
    {
        Canvas canvas;
        Rectangle rect1, rect2, rect3, rect4, rect5;
        public CapacityPanel()
        {
            Width = 40; Height = 30; Canvas.SetTop(this, 1013);
            Canvas.SetZIndex(this, MainCanvas.ZIndexes.ResourcePanel);
            canvas = new Canvas(); canvas.Width = 40; canvas.Height = 30;
            Path path = new Path(); path.Fill = Brushes.White; Child = canvas;
            path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M12,4 v3 h-9 v20 h40 v-20 h-9 v-3 h12 v26 h-46 v-26z"));
            canvas.Children.Add(path);
            canvas.Children.Add(rect1 = Common.GetRectangle(15, 6, Brushes.White));
            canvas.Children.Add(rect2 = Common.GetRectangle(15, 6, Brushes.White));
            canvas.Children.Add(rect3 = Common.GetRectangle(15, 6, Brushes.White));
            canvas.Children.Add(rect4 = Common.GetRectangle(15, 6, Brushes.White));
            canvas.Children.Add(rect5 = Common.GetRectangle(15, 6, Brushes.White));
            Canvas.SetLeft(rect1, 6); Canvas.SetTop(rect1, 18);
            Canvas.SetLeft(rect2, 25); Canvas.SetTop(rect2, 18);
            Canvas.SetLeft(rect3, 6); Canvas.SetTop(rect3, 10);
            Canvas.SetLeft(rect4, 25); Canvas.SetTop(rect4, 10);
            Canvas.SetLeft(rect5, 15); Canvas.SetTop(rect5, 2);
            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.2));
            anim.RepeatBehavior = RepeatBehavior.Forever;
            anim.AutoReverse = true;
            rect5.BeginAnimation(Rectangle.OpacityProperty, anim);
        }
        public void SetCapacity(double value)
        {
            if (value < 0.2)
            { rect1.Opacity = 0; rect2.Opacity = 0; rect3.Opacity = 0; rect4.Opacity = 0; rect5.Visibility = Visibility.Hidden; }
            else if (value < 0.4)
            { rect1.Opacity = 1; rect2.Opacity = 0; rect3.Opacity = 0; rect4.Opacity = 0; rect5.Visibility = Visibility.Hidden; }
            else if (value<0.6)
            { rect1.Opacity = 1; rect2.Opacity = 1; rect3.Opacity = 0; rect4.Opacity = 0; rect5.Visibility = Visibility.Hidden; }
            else if (value<0.8)
            { rect1.Opacity = 1; rect2.Opacity = 1; rect3.Opacity =1; rect4.Opacity = 0; rect5.Visibility = Visibility.Hidden; }
            else if (value<0.95)
            { rect1.Opacity = 1; rect2.Opacity = 1; rect3.Opacity = 1; rect4.Opacity = 1; rect5.Visibility = Visibility.Hidden; }
            else
            { rect1.Opacity = 1; rect2.Opacity = 1; rect3.Opacity = 1; rect4.Opacity = 1; rect5.Visibility = Visibility.Visible; }
        }
    }
    class ResourcePanel:Canvas
    {
        TextBlock box;
        static double[] stepsvalue = new double[] { 0.3, 0.55, 0.75, 0.9, 0.95, 1.0 };
        static double[] stepstime = new double[] { 0.05, 0.15, 0.3, 0.5, 0.75, 1.0 };
        public ResourcePanel(Brush Image)
        {
            Width = 250; Height = 45; Canvas.SetTop(this, 990);

            Canvas.SetZIndex(this, MainCanvas.ZIndexes.ResourcePanel);

            if (Image!=null)
            {
                Rectangle rect = Common.GetRectangle(45, Image);
                Children.Add(rect); Canvas.SetLeft(rect, 5); Canvas.SetTop(rect, 15);
            }
            box = new TextBlock();
            box.FontFamily = Links.Font;
            box.FontSize = 36;
            box.Foreground = Brushes.White;
            //box.Foreground = new SolidColorBrush(Color.FromArgb(255, 32, 255, 255));
            if (Image== null)
            {
                box.Width = 250; box.TextAlignment = TextAlignment.Center;
            }
            Children.Add(box);
            Canvas.SetLeft(box, 5 + (Image != null ? 55 : 0));
            Canvas.SetTop(box, 19); 
            //box.Text = "Privet";
        }
        public void SetText(string text)
        {
            box.Text = text;
        }
        public void SetLongValue(long newvalue, long lastvalue)
        {
            long delta = newvalue - lastvalue;
            StringAnimationUsingKeyFrames anim = new StringAnimationUsingKeyFrames();
            anim.Duration = TimeSpan.FromSeconds(1);
            for (int i = 0; i < 6; i++)
            {
                string t = (lastvalue + delta * stepsvalue[i]).ToString("### ### ### ### ### ###");
                t = t.TrimStart();
                t = t.Replace(' ', '.');
                anim.KeyFrames.Add(new DiscreteStringKeyFrame(t, KeyTime.FromPercent(stepstime[i])));
            }
            box.BeginAnimation(TextBlock.TextProperty, anim);
        }
        public void SetIntValue(int newvalue, int lastvalue)
        {
            int delta = newvalue - lastvalue;
            StringAnimationUsingKeyFrames anim = new StringAnimationUsingKeyFrames();
            anim.Duration = TimeSpan.FromSeconds(1);
            for (int i = 0; i < 6; i++)
            {
                string t = (lastvalue + delta * stepsvalue[i]).ToString("### ### ### ###");
                t = t.TrimStart();
                t = t.Replace(' ', '.');
                anim.KeyFrames.Add(new DiscreteStringKeyFrame(t, KeyTime.FromPercent(stepstime[i])));
            }
            box.BeginAnimation(TextBlock.TextProperty, anim);
            //box.Text = "999 999 999";
        }
    }
    class NoticeBorder:Canvas
    {
        public static byte[] array;
        public NoticeBorder()
        {
            Width = 434; Height = 600;
            Rectangle Back = new Rectangle();
            Back.Width = 434; Back.Height = 600;
            Back.Fill = Brushes.Black;
            Back.Stroke = Brushes.SkyBlue;
            Back.StrokeThickness = 2;
            Children.Add(Back);
            
            ScrollViewer SelfNoticeViewer = new ScrollViewer();
            SelfNoticeViewer.Width = 430; SelfNoticeViewer.Height = 596;
            SelfNoticeViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            SelfNoticeViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            StackPanel SelfNotice = new StackPanel();
            SelfNotice.Orientation = Orientation.Vertical;
            SelfNoticeViewer.Content = SelfNotice;
            Children.Add(SelfNoticeViewer);
            Canvas.SetLeft(SelfNoticeViewer, 2); Canvas.SetTop(SelfNoticeViewer, 2);

            List<TextBlock> list = GameNotice.GetNoticePanels(array);
            SelfNotice.Children.Clear();
            SelfNotice.Children.Add(Common.CreateLabel(20, Links.Interface("SelfNotice"), Brushes.White));
            for (int i = list.Count - 1; i >= 0; i--)
            {
                Border border = new Border();
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(2);
                border.Child = (list[i]);
                SelfNotice.Children.Add(border);
            }
        }
        public static void CheckNotice()
        {
            byte[] curarray = Gets.GetNoticeList();
            bool newdata = false;
            if (array==null && curarray== null)
            {
                return;
            }
            if (array == null && curarray != null)
            {
                array = curarray;
                if (array.Length!=0) newdata = true;
            }
            else if (array != null && curarray == null)
            {
                array = curarray;
                newdata = false;
            }
            else if (array.Length != curarray.Length)
            {
                array = curarray;
                newdata = true;
            }
            else
            {
                int maxlength = array.Length > curarray.Length ? curarray.Length : array.Length;
                newdata = false;
                for (int i = 0; i < maxlength; i++)
                    if (array[i] != curarray[i])
                    {
                        newdata = true;
                        break;
                    }
            }
            if (newdata && Links.Controller.MainCanvas.Chat.Visibility==Visibility.Hidden)
            {
                Links.Controller.MainCanvas.Chat.Visibility = Visibility.Visible;
            }
        }
    }
    class Parameters:Canvas
    {
        GlobalWindows Window;
        InterfaceButton Animation;
        public Parameters(GlobalWindows window)
        {
            Window = window;
            Width = 400;
            Height = 400;
            PathGeometry geom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h400 v400 h-400z"));
            Background = new DrawingBrush(new GeometryDrawing(Brushes.Black,
                new Pen(new SolidColorBrush(Color.FromArgb(255, 0, 168, 255)), 3), geom));
            BuildBasic();
            }
        void BuildBasic()
        {
            Children.Clear();
            TextBlock Title = Common.GetBlock(26, Links.Interface("Parameters"), Brushes.White, 380);
            Children.Add(Title); Canvas.SetLeft(Title, 10); Canvas.SetTop(Title, 5);
            InterfaceButton audio = new InterfaceButton(350, 40, 7, 24);
            audio.PutToCanvas(this, 25, 50);
            audio.SetText(Links.Interface("Audio"));
            audio.PreviewMouseDown += Audio_PreviewMouseDown;
            Animation = new InterfaceButton(350, 40, 7, 24);
            Animation.PutToCanvas(this, 25, 100);
            if (Links.Animation == true)
                Animation.SetText("Animation On");
            else
                Animation.SetText("Animation Off");
            Animation.PreviewMouseDown += Animation_PreviewMouseDown;
            AddClose();
        }

        private void Animation_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Animation = Links.Animation == true ? false : true;
            if (Links.Animation == true)
                Animation.SetText("Animation On");
            else
                Animation.SetText("Animation Off");
            Links.SaveData.SaveAnimation();
        }

        private void Audio_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            BuildAudio();
        }
        Slider musicslider, soundslider;
        void BuildAudio()
        {
            Children.Clear();
            TextBlock Title = Common.GetBlock(26, Links.Interface("Audio"), Brushes.White, 380);
            Children.Add(Title); Canvas.SetLeft(Title, 10); Canvas.SetTop(Title, 5);
            TextBlock music = Common.GetBlock(20, Links.Interface("Music"), Brushes.White, 180);
            Children.Add(music); Canvas.SetLeft(music, 10); Canvas.SetTop(music, 50);
            TextBlock sound = Common.GetBlock(20, Links.Interface("Sound"), Brushes.White, 180);
            Children.Add(sound); Canvas.SetLeft(sound, 210); Canvas.SetTop(sound, 50);

            musicslider = new Slider(); musicslider.Orientation = Orientation.Vertical;
            musicslider.Width = 40; musicslider.Height = 250; musicslider.Maximum = 1;
            musicslider.Minimum = 0; musicslider.ValueChanged += Musicslider_ValueChanged;
            Children.Add(musicslider); Canvas.SetLeft(musicslider, 80); Canvas.SetTop(musicslider, 80);
            musicslider.Value = Links.MusicVolume;

            soundslider = new Slider(); soundslider.Orientation = Orientation.Vertical;
            soundslider.Width = 40; soundslider.Height = 250; soundslider.Maximum = 1;
            soundslider.Minimum = 0; soundslider.ValueChanged += Musicslider_ValueChanged;
            Children.Add(soundslider); Canvas.SetLeft(soundslider, 280); Canvas.SetTop(soundslider, 80);
            soundslider.Value = Links.SoundVolume;

            InterfaceButton OkButton = new InterfaceButton(100, 40, 7, 24);
            OkButton.SetText(Links.Interface("Ok"));
            OkButton.PutToCanvas(this, 50, 350);
            OkButton.PreviewMouseDown += OkButton_PreviewMouseDown;

            AddClose();
        }
        void AddClose()
        {
            InterfaceButton CloseButton = new InterfaceButton(100, 40, 7, 24);
            CloseButton.SetText(Links.Interface("Close"));
            CloseButton.PutToCanvas(this, 250, 350);
            CloseButton.PreviewMouseDown += CloseButton_PreviewMouseDown;
        }

        private void CloseButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Window==GlobalWindows.Login)
                Links.Controller.LoginCanvas.Children.Remove(this);
            else if (Window==GlobalWindows.Main)
                Links.Controller.PopUpCanvas.Remove();
        }

        private void OkButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Music.ChangeVolume((float)musicslider.Value);
            Links.SoundVolume = (float)soundslider.Value;
            Links.SaveData.SaveAudio();
            Links.Controller.PopUpCanvas.Remove();
        }

        private void Musicslider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = (Slider)sender;
            slider.Value = Math.Round(slider.Value, 1);
        }
    }
    
    public enum MenuElements { Nation, Galaxy, Colony, Fleets, Science }
   
}
