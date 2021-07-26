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
    class IntBoya : Canvas
    {
        public static IntBoya CurCanvas;
        static long BattleID;
        public static Battle Battle;
        public static ShipSide ControlledSide;
        public static BattleMode CurMode;
        public static bool IsActive = false;
        public static bool Test = false;
        public static TimeIndicator Timer;
        public static AutoModePanel AutoModePanel;
        public static TurnGrid TurnsLabel;
        public static byte SpeedValue;

        public static bool Status = false;
        bool IsYellowLine = false;
        bool IsBlueLine = false;
        public bool IsBigFire = false;
        public bool IsSmallFire = false;
        bool IsRightLine = false;
        //BigFireMove BigFire;
        //SmallFireMove SmallFire;
        Canvas BottomCanvas;
        Canvas UpCanvas;
        Canvas BackCanvas;
        RotateTransform End0R, End1R, End2R;
        Canvas EndTurnCanvas;
        Canvas ExitCanvas;
        RotateTransform Exit0R, Exit1R, Exit2R;
        public IntBoya()
        {
            Width = 1920; Height = 1080;
            CurCanvas = this;
            ClipToBounds = true;
            HexCanvas.Create();
            Children.Add(HexCanvas.Box);
            Canvas.SetLeft(HexCanvas.Box, 90);
            Canvas.SetTop(HexCanvas.Box, 120);
            Canvas.SetZIndex(HexCanvas.Box, 100);
            Timer = new TimeIndicator();

            AutoModePanel = new AutoModePanel();
            //Children.Add(AutoModePanel);
            Canvas.SetLeft(AutoModePanel, 1810);
            Canvas.SetTop(AutoModePanel, 600);
            Canvas.SetZIndex(AutoModePanel, 105);
            AutoModePanel.PreviewMouseDown += AutoButton_PreviewMouseDown;
            TurnsLabel = new TurnGrid();
            Children.Add(TurnsLabel);
            Canvas.SetZIndex(TurnsLabel, 105);
            Canvas.SetLeft(TurnsLabel, 850); Canvas.SetTop(TurnsLabel, 30);

            SpeedButton BTNSpeed = new SpeedButton();
            Children.Add(BTNSpeed); Canvas.SetLeft(BTNSpeed, 50); Canvas.SetTop(BTNSpeed, 1000);
            Canvas.SetZIndex(BTNSpeed, 105);
            SpeedValue = 1;
            PutImage(1920, 1080, 0, 0, "2/fone3", this);
            PutImage(1510, 1080, 205, 0, "2/fone2", this);
            PutImage(1920, 580, 0, 500, "2/fone1", this);
            PutImage(1920, 192, 0, 0, "2/Top", this);
            //PutImage(1904, 221, 8, 192, "2/Top1", this);
            PutImage(1920, 1080, 0, 0, "2/Top2", this);
            PutImage(1920, 1080, 0, 0, "2/Bottom", this);
            PutImage(1920, 57, 0, 1023, "2/Bottom2", this);
            PutImage(1832, 83, 44, 993, "2/Bottom3", this);
            PutImage(329, 99, 796, 0, "2/Middle", this);
            BattleButton btn = new BattleButton("Конец хода");
            Children.Add(btn); Canvas.SetLeft(btn, 320); Canvas.SetTop(btn, 955);
            btn.PreviewMouseDown += BTNConfirm_PreviewMouseDown;
            BattleButton auto = new BattleButton("Автобой");
            Children.Add(auto); Canvas.SetLeft(auto, 1280); Canvas.SetTop(auto, 955);
            auto.PreviewMouseDown += AutoButton_PreviewMouseDown;
            Rectangle exit = PutImage(85, 79, 1795, 995, "2/Х", this);
            exit.PreviewMouseDown += ExitCanvas_PreviewMouseDown;
            /*
            Background = Brushes.Black;
            BackCanvas = new Canvas();
            Children.Add(BackCanvas);
            BackCanvas.Width = 25;
            BackCanvas.Height = 1080 * 2;
            Canvas.SetLeft(BackCanvas, 1895);
            PutImage(25, 1080, 0, 0, "YellowRight", BackCanvas);
            PutImage(25, 1080, 0, 1080, "YellowRight", BackCanvas);
            PutImage(1152, 56, 540, 1014, "Bottom", this);
            BottomCanvas = new Canvas();
            Children.Add(BottomCanvas);
            BottomCanvas.Width = 1920 * 2;
            BottomCanvas.Height = 25;
            Canvas.SetTop(BottomCanvas, 1055);
            PutImage(1920, 25, 0, 0, "SideYellow", BottomCanvas);
            PutImage(1920, 25, 1920, 0, "SideYellow", BottomCanvas);

            UpCanvas = new Canvas();
            Children.Add(UpCanvas);
            UpCanvas.Width = 1920 * 2;
            UpCanvas.Height = 25;
            Canvas.SetTop(UpCanvas, 1030);
            PutImage(1920, 25, 0, 0, "SideBlue", UpCanvas);
            PutImage(1920, 25, 1920, 0, "SideBlue", UpCanvas);
            */
            /*PutImage(241, 207, 1679, 873, "RightBottomCorner", this);
            PutImage(552, 171, 0, 909, "LeftBottomCorner", this);
            PutImage(627, 924, 1293, 0, "RightUpCorner", this);
            PutImage(228, 559, 0, 0, "LeftUpCorner", this);
            PutImage(914, 88, 450, 0, "TopCenter", this);*/
            //BigFire = new BigFireMove(this);
            //SmallFire = new SmallFireMove(this);
            //PutImage(103, 102, 1803, 180, "Items", this);
            //PutImage(103, 102, 1803, 305, "Items", this);
            //PutImage(103, 102, 1803, 437, "Items", this);
            /*EndTurnCanvas = new Canvas(); EndTurnCanvas.Width = 185; EndTurnCanvas.Height = 185;
            Children.Add(EndTurnCanvas); Canvas.SetLeft(EndTurnCanvas, 1715); Canvas.SetTop(EndTurnCanvas, 882);
            Rectangle r0 = PutImage(185, 185, 0, 0, "EndTurn0", EndTurnCanvas);
            r0.RenderTransformOrigin = new Point(0.5, 0.5);
            r0.RenderTransform = End0R = new RotateTransform();
            Rectangle r1 = PutImage(129, 129, 29, 28, "EndTurn1", EndTurnCanvas);
            r1.RenderTransformOrigin = new Point(0.5, 0.5);
            r1.RenderTransform = End1R = new RotateTransform();
            Rectangle r2 = PutImage(185, 185, 0, 0, "EndTurn2", EndTurnCanvas);
            r2.RenderTransformOrigin = new Point(0.5, 0.5);
            r2.RenderTransform = End2R = new RotateTransform();
            EndTurnCanvas.PreviewMouseDown += IntBoya_PreviewMouseDown;*/

            /*ExitCanvas = new Canvas(); ExitCanvas.Width = 139; ExitCanvas.Height = 139;
            Children.Add(ExitCanvas); Canvas.SetLeft(ExitCanvas, 1770); Canvas.SetTop(ExitCanvas, 9);
            Rectangle ex0 = PutImage(139, 139, 0, 0, "Exit0", ExitCanvas);
            ex0.RenderTransformOrigin = new Point(0.5, 0.5);
            ex0.RenderTransform = Exit0R = new RotateTransform();
            Rectangle ex1 = PutImage(108, 108, 15, 14, "Exit1", ExitCanvas);
            ex1.RenderTransformOrigin = new Point(0.5, 0.5);
            ex1.RenderTransform = Exit1R = new RotateTransform();
            Rectangle ex2 = PutImage(82, 82, 29, 28, "Exit2", ExitCanvas);
            ex2.RenderTransformOrigin = new Point(0.5, 0.5);
            ex2.RenderTransform = Exit2R = new RotateTransform();
            ExitCanvas.MouseEnter += ExitCanvas_MouseEnter;
            ExitCanvas.PreviewMouseDown += ExitCanvas_PreviewMouseDown;*/
        }
        class BattleButton:Canvas
        {
            Rectangle rect;
            public BattleButton(string text)
            {
                Width = 319; Height = 114;
                Background = Links.Brushes.Transparent;
                rect = Common.GetRectangle(319, 114, Gets.GetIntBoyaImage("2/btn"));
                Children.Add(rect);
                TextBlock block = Common.GetBlock(30, text, Brushes.White, 319);
                Children.Add(block); Canvas.SetTop(block, 40);
                MouseEnter += BattleButton_MouseEnter;
                MouseLeave += BattleButton_MouseLeave;
            }

            private void BattleButton_MouseLeave(object sender, MouseEventArgs e)
            {
                rect.BeginAnimation(Rectangle.OpacityProperty, null);
            }

            private void BattleButton_MouseEnter(object sender, MouseEventArgs e)
            {
                DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.4));
                anim.RepeatBehavior = RepeatBehavior.Forever;
                anim.AutoReverse = true;
                rect.BeginAnimation(Rectangle.OpacityProperty, anim);
            }
        }
        private void AutoButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (Test) return;
            e.Handled = true;
            bool eventresult = Events.SetBattleToAutoMode(Battle);
            if (eventresult) Timer.Time(2);
            else Gets.ReadTurns(Battle);
        }
        private void ExitCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Quit();

        }
        static Canvas RestrictionCanvas;
        public static void ShowRestrictionInfo()
        {
            if (RestrictionCanvas != null) CurCanvas.Children.Remove(RestrictionCanvas);
            RestrictionCanvas = new Canvas(); Canvas.SetZIndex(RestrictionCanvas, 255);
            RestrictionCanvas.Width = 165; RestrictionCanvas.Height = 95;
            Rectangle rect1 = null; Rectangle rect2 = null;
            switch (Battle.Restriction)
            {
                case Restriction.DoubleEnergy: 
                case Restriction.NoEnergy: rect1 = Common.GetRectangle(60, Links.Brushes.WeaponGroupBrush[WeaponGroup.Energy]); break;
                case Restriction.DoublePhysic:
                case Restriction.NoPhysic: rect1 = Common.GetRectangle(60, Links.Brushes.WeaponGroupBrush[WeaponGroup.Physic]); break;
                case Restriction.DoubleIrregular:
                case Restriction.NoIrregular: rect1 = Common.GetRectangle(60, Links.Brushes.WeaponGroupBrush[WeaponGroup.Irregular]); break;
                case Restriction.DoubleCyber:
                case Restriction.NoCyber: rect1 = Common.GetRectangle(60, Links.Brushes.WeaponGroupBrush[WeaponGroup.Cyber]); break;
                case Restriction.LowAccuracy:
                case Restriction.DoubleAccuracy: rect1 = Common.GetRectangle(60, Pictogram.Accuracy); break;
                    
            }
            switch (Battle.Restriction)
            {
                case Restriction.NoEnergy:
                case Restriction.NoPhysic:
                case Restriction.NoIrregular:
                case Restriction.NoCyber:
                    rect2 = Common.GetRectangle(90, Links.Brushes.EmptyImageBrush); rect2.Opacity = 0.4;
                    Canvas.SetLeft(rect2, 35); Canvas.SetTop(rect2, 0); break;
                case Restriction.DoubleEnergy:
                case Restriction.DoublePhysic:
                case Restriction.DoubleIrregular:
                case Restriction.DoubleCyber:
                case Restriction.DoubleAccuracy:
                    rect2 = Common.GetRectangle(30, Pictogram.IncreaseBrush);
                    Canvas.SetLeft(rect2, 115); Canvas.SetTop(rect2, 45); break;
                case Restriction.LowAccuracy:
                    rect2 = Common.GetRectangle(30, Pictogram.DecreaseBrush);
                    Canvas.SetLeft(rect2, 115); Canvas.SetTop(rect2, 45); break;
            }
            if (rect1 != null)
            {
                Canvas.SetLeft(rect1, 50); Canvas.SetTop(rect1, 15);
                RestrictionCanvas.Children.Add(rect1);
            }
            if (rect2!= null)
                RestrictionCanvas.Children.Add(rect2);
            if (Battle.RestrictionLength!=0 && Battle.RestrictionLength<50)
            {
                TextBlock wait = Common.GetBlock(40, Battle.RestrictionLength.ToString(), Links.Brushes.SkyBlue, 40);
                RestrictionCanvas.Children.Add(wait); Canvas.SetTop(wait, 40);
            }    

            Canvas.SetLeft(RestrictionCanvas, 255); Canvas.SetTop(RestrictionCanvas, 945);
            CurCanvas.Children.Add(RestrictionCanvas);
        }
        public static Border ArtefactInfo;
        public static void ShowArtefactInfo(Artefact artefact)
        {
            if (ArtefactInfo != null) IntBoya.CurCanvas.Children.Remove(ArtefactInfo);
            ArtefactInfo = GetArtefactInfo(artefact);
            IntBoya.CurCanvas.Children.Add(ArtefactInfo);
            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
            ArtefactInfo.BeginAnimation(Border.OpacityProperty, anim);
        }
        public static Border GetArtefactInfo(Artefact artefact)
        {
            Border border = new Border(); border.Width = 400; border.Height = 400;
            Canvas.SetZIndex(border, 255); Canvas.SetLeft(border, 760); Canvas.SetTop(border, 400);
            border.BorderBrush = Links.Brushes.SkyBlue; border.BorderThickness = new Thickness(2); border.CornerRadius = new CornerRadius(20);
            border.Background = Brushes.Black;
            Canvas canvas = new Canvas(); border.Child = canvas;
            TextBlock Title = Common.GetBlock(30, artefact.GetName(), Brushes.White, 290);
            canvas.Children.Add(Title); Canvas.SetLeft(Title, 100); Canvas.SetTop(Title, 15);
            Rectangle Image = Common.GetRectangle(70, artefact.GetImage());
            canvas.Children.Add(Image); Canvas.SetLeft(Image, 10); Canvas.SetTop(Image, 10);
            TextBlock Descr = Common.GetBlock(26, artefact.GetDescription(), Brushes.White, 380);
            canvas.Children.Add(Descr); Canvas.SetLeft(Descr, 10); Canvas.SetTop(Descr, 150);
            TextBlock EnergySpent = Common.GetBlock(26, "Затраты энергии портала: ", Brushes.White, 380);
            EnergySpent.TextAlignment = TextAlignment.Left;
            Run spentrun = new Run(artefact.EnergyCost.ToString()); spentrun.Foreground = Links.Brushes.SkyBlue;
            EnergySpent.Inlines.Add(spentrun);
            EnergySpent.Inlines.Add(new LineBreak());
            EnergySpent.Inlines.Add(new Run("Повторное использование: "));
            Run WaitRun = new Run(String.Format("Через {0} раундов", artefact.WaitTime)); WaitRun.Foreground = Links.Brushes.SkyBlue;
            EnergySpent.Inlines.Add(WaitRun);
            canvas.Children.Add(EnergySpent); Canvas.SetLeft(EnergySpent, 10); Canvas.SetTop(EnergySpent, 85);
            return border;
        }
        public class ArtefactPlace:Canvas
        {
            public Artefact Artefact;
            public ScaleTransform Scale;
            Side Side;
            byte Pos;
            TextBlock WaitBlock;
            public ArtefactPlace(byte pos, Artefact artefact, Side side)
            {
                Artefact = artefact; Side = side; Pos = pos;
                Width = 100; Height = 100;
                Rectangle rect = Common.GetRectangle(100, artefact.GetImage());
                Children.Add(rect);
                Canvas.SetTop(this, 950);
                switch (pos) { case 0: Canvas.SetLeft(this,680); break; case 1: Canvas.SetLeft(this,835); break; case 2: Canvas.SetLeft(this, 990); break; }
                Scale = new ScaleTransform();
                rect.RenderTransformOrigin = new Point(0.5, 0.5);
                rect.RenderTransform = Scale;
                this.PreviewMouseDown += UseBattleArtefact;

                WaitBlock = Common.GetBlock(60, side.Artefacts[pos].WaitTime.ToString(), Brushes.White, 50);
                Children.Add(WaitBlock); Canvas.SetLeft(WaitBlock, 25); Canvas.SetTop(WaitBlock, 20);
                WaitBlock.Opacity = 0.5;
                this.MouseEnter += ArtefactPlace_MouseEnter;
                this.MouseLeave += ArtefactPlace_MouseLeave;
            }

            private void ArtefactPlace_MouseLeave(object sender, MouseEventArgs e)
            {
                if (IntBoya.ArtefactInfo!= null)
                {
                    IntBoya.CurCanvas.Children.Remove(IntBoya.ArtefactInfo);
                    IntBoya.ArtefactInfo = null;
                }
            }

            private void ArtefactPlace_MouseEnter(object sender, MouseEventArgs e)
            {
                IntBoya.ShowArtefactInfo(Artefact);

            }

            public void SetWaitTime(byte val)
            {
                if (val == 0)
                    WaitBlock.Visibility = Visibility.Hidden;
                else
                {
                    WaitBlock.Text = val.ToString();
                    WaitBlock.Visibility = Visibility.Visible;
                }
            }
            private void UseBattleArtefact(object sender, MouseButtonEventArgs e)
            {
                if (BattleController.CanCommand == false) return;
                if ((Side.Artefacts[Pos].WaitTime) > 0) return;
                if (CurArtRect != null && CurArtRect != this) { RemoveArtSelect(); HexCanvas.RemoveArtefact(); }
                if (CurArtRect == this)
                {
                    RemoveArtSelect();
                    HexCanvas.RemoveArtefact();
                    return;
                }
                CurArtRect = this;
                DoubleAnimation anim = new DoubleAnimation(1, 1.3, TimeSpan.FromSeconds(0.2));
                Scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
                Scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
                HexCanvas.UseArtefact(Artefact, Pos);
            }
        }
        public static List<ArtefactPlace> Artefacts = new List<ArtefactPlace>();
        static ArtefactPlace CurArtRect;
        public void AddArtefacts(Side side)
        {
            foreach (ArtefactPlace rect in Artefacts)
                Children.Remove(rect);
            Artefacts.Clear();
            if (side == null) return;
            for (int i = 0; i < side.Artefacts.Count; i++)
            {
                Artefact art = side.Artefacts[(byte)i].Artefact;
                ArtefactPlace place = new ArtefactPlace((byte)i, art, side);
                Children.Add(place);
                Artefacts.Add(place);
            }
        }
        public static void RemoveArtSelect()
        {
            if (CurArtRect == null) return;
            DoubleAnimation anim = new DoubleAnimation(1.3, 1, TimeSpan.FromSeconds(0.2));
            CurArtRect.Scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            CurArtRect.Scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
            CurArtRect = null;
        }

        public static void Quit()
        {
            Links.Controller.Debug.Hide();
            if (Links.Controller.mainWindow.MainVbx.Child == Links.Controller.IntBoya)
            {
                Links.Controller.mainWindow.MainVbx.Child = Links.Controller.MainCanvas;
                Music.ChangeType(MusicType.Strategy);
            }
            BattleController.AutoWork = false;
            BattleController.Timer.Stop();
            //DebugWindow.tb1.Text = "Стоп";
            Timer.Stop();
            BW.SetWorking("BattleQuit", false);
            if (Battle != null) Battle.IsVisual = false;
            Battle battle = Battle;
            Battle = null;
            IsActive = false;
            Stop();
            if (Test)
            {
                Links.Controller.mainWindow.Close();
                return;
            }
            Events.SetBattleToAutoMode(battle);
            Gets.GetResources();
            Gets.GetTotalInfo("После выхода из боя");
            NoticeBorder.CheckNotice();
            if (Links.Controller.MainCanvas.LowBorder.Child == Links.Controller.Galaxy)
                Links.Controller.SelectPanel(GamePanels.Galaxy, SelectModifiers.None);
            else if (Links.Controller.MainCanvas.LowBorder.Child == Links.Controller.FleetsCanvas.box)
                Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.None);
        }
        public static void BTNConfirm_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e != null)
                e.Handled = true;
            if (Battle.IsFinished) { Quit(); return; }
            RemoveArtSelect();
            HexCanvas.RemoveArtefact();
            HexCanvas.Inner.Children.Remove(HexCanvas.EventsButtonsCanvas);
            if (BattleController.CanCommand)
            {
                //ConfirmStop.Color = Colors.Green;
                //BTNConfirm.Select();
                string eventresult;
                if (Test == false)
                    eventresult = Events.SetBattleMoveList(Battle);
                else
                    eventresult = Events.SetBattleMoveListTest(Battle);
                if (eventresult == "0")
                    Timer.Time(2);
                else if (eventresult != "1")
                    Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult), true);
                else Timer.Time(5);
            }
        }
        public void Place(long battleid)
        {
            Links.Controller.Debug.Show();
            if (!GSGameInfo.Battles.ContainsKey(battleid))
            {
                if (Gets.GetBattle(battleid) == null) return;
            }
            Links.Controller.mainWindow.MainVbx.Child = this;
            Music.ChangeType(MusicType.Battle);
            BattleID = battleid;
            Battle = GSGameInfo.Battles[battleid];
            Battle.Update();
            if (GSGameInfo.Fleets.ContainsKey(Battle.Side1.FleetID)) { ControlledSide = ShipSide.Attack; AddArtefacts(Battle.Side1); }
            else if (GSGameInfo.Fleets.ContainsKey(Battle.Side2.FleetID)) { ControlledSide = ShipSide.Defense; AddArtefacts(Battle.Side2); }
            else { ControlledSide = ShipSide.None; AddArtefacts(null); }
            CurMode = Battle.CurMode;
            Battle.Moves = null;
            Battle.IsVisual = true;
            HexCanvas.Create3();
            Background = Gets.GetIntBoyaImage("fon");// new ImageBrush(new BitmapImage(new Uri("C:/123/fon.png")));
            //Background = Brushes.Black;
            IsActive = true;
            Timer.Battle = Battle;
            //Links.Controller.BattlePopUp.Place(new MediumInfo(Links.Interface("ShowLearn"), ShowLesson1));
            Timer.Time(2);
            Restart();
        }
        public static void SetAutoMode()
        {
            bool eventresult = Events.SetBattleToAutoMode(Battle);
            if (eventresult) Timer.Time(2);
            else Gets.ReadTurns(Battle);
        }
        /*public static void SetShipCommands(ShipCommands cur)
        {
            if (CurrentCommand == cur)
            {
                SwitchOffButtons(cur);
                CurrentCommand = ShipCommands.None;
            }
            else
            {
                SwitchOffButtons(CurrentCommand);
                if (cur == ShipCommands.TargetGun && !BattleController.CanCommand) return;
                SelectButtons(cur);
                CurrentCommand = cur;
            }
        }*/
        /*
        static void SelectButtons(ShipCommands comm)
        {
            foreach (BattleButton button in DependencyButtons)
                if (button.Command == comm)
                    button.SimpleSelect();
        }
        static void SwitchOffButtons(ShipCommands comm)
        {
            foreach (BattleButton button in DependencyButtons)
            {
                if (button.IsSelected) button.SimpleDeSelect();
            }
        }*/
        private void ExitCanvas_MouseEnter(object sender, EventArgs e)
        {
            if (Links.Animation == false) return;
            if (VisualTreeHelper.HitTest(ExitCanvas, Mouse.GetPosition(ExitCanvas)) == null) return;
            DoubleAnimation anim0 = new DoubleAnimation(0, 360, TimeSpan.FromSeconds(2));
            anim0.AccelerationRatio = 0.5; anim0.DecelerationRatio = 0.5;
            anim0.Completed += ExitCanvas_MouseEnter;
            Exit0R.BeginAnimation(RotateTransform.AngleProperty, anim0);
            DoubleAnimation anim1 = new DoubleAnimation(0, -360, TimeSpan.FromSeconds(2));
            Exit1R.BeginAnimation(RotateTransform.AngleProperty, anim1);
            DoubleAnimation anim2 = new DoubleAnimation(0, 360, TimeSpan.FromSeconds(2));
            Exit2R.BeginAnimation(RotateTransform.AngleProperty, anim2);
        }

        public void EndTurn()
        {
            DoubleAnimation anim0 = new DoubleAnimation(0, 360, TimeSpan.FromSeconds(2));
            anim0.AccelerationRatio = 0.5; anim0.DecelerationRatio = 0.5;
            End0R.BeginAnimation(RotateTransform.AngleProperty, anim0);
            DoubleAnimation anim1 = new DoubleAnimation(0, 90, TimeSpan.FromSeconds(1));
            anim1.AutoReverse = true;
            End1R.BeginAnimation(RotateTransform.AngleProperty, anim1);
            DoubleAnimation anim2 = new DoubleAnimation(360, 0, TimeSpan.FromSeconds(2));
            anim2.AccelerationRatio = 0.5; anim2.DecelerationRatio = 0.5;
            End2R.BeginAnimation(RotateTransform.AngleProperty, anim2);
        }
        private void IntBoya_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            EndTurn();
            BTNConfirm_PreviewMouseDown(null, null);
        }

       /* private void StartRightLine(object sender, EventArgs e)
        {
            if (Links.Animation == false) return;
            if (Status)
            {
                DoubleAnimation anim = new DoubleAnimation(0, -1080, TimeSpan.FromSeconds(30));
                anim.Completed += StartRightLine;
                BackCanvas.BeginAnimation(Canvas.TopProperty, anim);
                IsRightLine = true;
            }
            else
                IsRightLine = false;
        }
        private void StartBlueLine(object sender, EventArgs e)
        {
            if (Links.Animation == false) return;
            if (Status)
            {
                DoubleAnimation anim = new DoubleAnimation(0, -1920, TimeSpan.FromSeconds(25));
                anim.Completed += StartBlueLine;
                UpCanvas.BeginAnimation(Canvas.LeftProperty, anim);
                IsBlueLine = true;
            }
            else
                IsBlueLine = false;
        }*/
       /* private void StartYellowLine(object sender, EventArgs e)
        {
            if (Links.Animation == false) return;
            if (Status)
            {
                DoubleAnimation anim = new DoubleAnimation(0, -1920, TimeSpan.FromSeconds(35));
                anim.Completed += StartYellowLine;
                BottomCanvas.BeginAnimation(Canvas.LeftProperty, anim);
                IsYellowLine = true;
            }
            else
                IsYellowLine = false;
        }*/

        public void Restart()
        {
            Status = true;
            //if (IsYellowLine == false) StartYellowLine(null, null);
            if (IsBigFire == false)
                ;
                //BigFire.Start();
           // if (IsBlueLine == false) StartBlueLine(null, null);

            if (IsSmallFire == false)
                ;
                //SmallFire.Start();
          //  if (IsRightLine == false) StartRightLine(null, null);
        }
        public static void Stop()
        {
            Status = false;
        }
        /*
        public void Start()
        {
            Status = true;
            BigFire.Start();
            StartYellowLine(null, null);
            StartBlueLine(null, null);
            SmallFire.Start();
            StartRightLine(null, null);
        }
        */
        Rectangle PutImage(int width, int height, int left, int top, string brush, Canvas canvas)
        {
            Rectangle rect = new Rectangle(); rect.Width = width; rect.Height = height;
            canvas.Children.Add(rect); Canvas.SetLeft(rect, left); Canvas.SetTop(rect, top);
            rect.Fill = Gets.GetIntBoyaImage(brush);
            return rect;
        }

        /*
        class SmallFireMove
        {
            enum Locations { Top1, Top2, Top3, Top4, Middle1, Middle2, Middle3, Middle4, Down1, Down2, Down3, Down4 }
            Locations Location = Locations.Middle3;
            Rectangle rect;
            RotateTransform rotate;
            IntBoya Main;
            public SmallFireMove(IntBoya canvas)
            {
                Main = canvas;
                rect = new Rectangle(); rect.Width = 16; rect.Height = 28;
                rect.Fill = Gets.GetIntBoyaImage("FireSmall");
                canvas.Children.Add(rect);
                rect.RenderTransformOrigin = new Point(0.5, 0.5);
                rect.RenderTransform = rotate = new RotateTransform();
            }
            public void Start()
            {
                if (Links.Animation == false) return;
                Next(null, null);
            }
            private void Next(object sender, EventArgs e)
            {
                if (Status == true)
                    Main.IsSmallFire = true;
                else
                {
                    Main.IsSmallFire = false;
                    return;
                }
                Location = (Locations)(((int)Location + 1) % 12);
                float x1, x2, xt = 0, y1, y2, yt = 0, ht = 3;
                switch (Location)
                {
                    case Locations.Top1:
                        rotate.Angle = 0;
                        x1 = 1797; x2 = 1797; y1 = 160; y2 = 260; yt = 3;
                        break;
                    case Locations.Top2:
                        rotate.Angle = -90;
                        x1 = 1800; x2 = 1890; xt = 3; y1 = 265; y2 = 265;
                        break;
                    case Locations.Top3:
                        rotate.Angle = 180;
                        x1 = 1895; x2 = 1895; y1 = 270; y2 = 170; yt = 3;
                        break;
                    case Locations.Top4:
                        rotate.Angle = 90;
                        x1 = 1890; x2 = 1800; xt = 3; y1 = 168; y2 = 168;
                        break;
                    case Locations.Middle1:
                        rotate.Angle = 0;
                        x1 = 1797; x2 = 1797; y1 = 290; y2 = 390; yt = 3;
                        break;
                    case Locations.Middle2:
                        rotate.Angle = -90;
                        x1 = 1800; x2 = 1890; xt = 3; y1 = 390; y2 = 390;
                        break;
                    case Locations.Middle3:
                        rotate.Angle = 180;
                        x1 = 1895; x2 = 1895; y1 = 395; y2 = 300; yt = 3;
                        break;
                    case Locations.Middle4:
                        rotate.Angle = 90;
                        x1 = 1890; x2 = 1800; xt = 3; y1 = 293; y2 = 293;
                        break;
                    case Locations.Down1:
                        rotate.Angle = 0;
                        x1 = 1797; x2 = 1797; y1 = 420; y2 = 520; yt = 3;
                        break;
                    case Locations.Down2:
                        rotate.Angle = -90;
                        x1 = 1800; x2 = 1890; xt = 3; y1 = 522; y2 = 522;
                        break;
                    case Locations.Down3:
                        rotate.Angle = 180;
                        x1 = 1895; x2 = 1895; y1 = 520; y2 = 430; yt = 3;
                        break;
                    case Locations.Down4:
                        rotate.Angle = 90;
                        x1 = 1890; x2 = 1800; xt = 3; y1 = 425; y2 = 425;
                        break;
                    default: x1 = 0; x2 = 0; xt = 0; y1 = 0; y2 = 0; yt = 0; ht = 0; break;
                }
                DoubleAnimation animx = new DoubleAnimation(x1, x2, TimeSpan.FromSeconds(xt));
                rect.BeginAnimation(Canvas.LeftProperty, animx);
                DoubleAnimation animy = new DoubleAnimation(y1, y2, TimeSpan.FromSeconds(yt));
                rect.BeginAnimation(Canvas.TopProperty, animy);
                DoubleAnimationUsingKeyFrames animh = new DoubleAnimationUsingKeyFrames();
                animh.Duration = TimeSpan.FromSeconds(ht);
                animh.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.0))));
                animh.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.5))));
                animh.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(ht - 0.5))));
                animh.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(ht))));
                animh.Completed += Next;
                rect.BeginAnimation(Rectangle.OpacityProperty, animh);

            }

        }*/
        /*
        class BigFireMove
        {
            enum Locations { Down1, Down2, Up1, Up2, Left1, Right1, Right2, Right3, Right4 }
            Locations Location = Locations.Right4;
            Rectangle rect;
            RotateTransform rotate;
            IntBoya Main;
            public BigFireMove(IntBoya canvas)
            {
                Main = canvas;
                rect = new Rectangle(); rect.Width = 80; rect.Height = 32;
                rect.Fill = Gets.GetIntBoyaImage("FireBig");
                canvas.Children.Add(rect);
                rect.RenderTransformOrigin = new Point(0.5, 0.5);
                rect.RenderTransform = rotate = new RotateTransform();
            }
            public void Start()
            {
                if (Links.Animation == false) return;
                Next(null, null);
            }
            private void Next(object sender, EventArgs e)
            {
                if (Status == true)
                    Main.IsBigFire = true;
                else
                {
                    Main.IsBigFire = false;
                    return;
                }
                Location = (Locations)(((int)Location + 1) % 9);
                float x1, x2, xt, y1, y2, yt, ht;
                switch (Location)
                {
                    case Locations.Down1:
                        rotate.Angle = 0;
                        x1 = 0; x2 = 410; xt = 8; y1 = 895; y2 = 895; yt = 0; ht = 8f;
                        break;
                    case Locations.Down2:
                        rotate.Angle = 60;
                        x1 = 437; x2 = 500; xt = 3; y1 = 925; y2 = 1025; yt = 3; ht = 3;// M428,910 l90,145 v40"
                        break;
                    case Locations.Up1:
                        rotate.Angle = 0;
                        x1 = 460; x2 = 1080; xt = 13; y1 = 72; y2 = 72; yt = 0; ht = 13;
                        break;
                    case Locations.Up2:
                        rotate.Angle = 0;
                        x1 = 1180; x2 = 1290; xt = 3; y1 = 53; y2 = 53; yt = 0; ht = 3;
                        break;
                    case Locations.Left1:
                        rotate.Angle = 90;
                        x1 = 30; x2 = 30; xt = 0; y1 = 100; y2 = 420; yt = 6; ht = 6;
                        break;
                    case Locations.Right1:
                        rotate.Angle = 0;
                        x1 = 1350; x2 = 1650; xt = 5; y1 = 33; y2 = 33; yt = 0; ht = 5;
                        break;
                    case Locations.Right2:
                        rotate.Angle = 90;
                        x1 = 1675; x2 = 1675; xt = 0; y1 = 60; y2 = 120; yt = 1.5f; ht = 1.5f;
                        break;
                    case Locations.Right3:
                        rotate.Angle = 180;
                        x1 = 1670; x2 = 1400; xt = 4; y1 = 133; y2 = 133; yt = 0; ht = 4;
                        break;
                    case Locations.Right4:
                        rotate.Angle = 245;
                        x1 = 1370; x2 = 1345; xt = 2; y1 = 110; y2 = 50; yt = 2; ht = 2;
                        break;
                    default: x1 = 0; x2 = 0; xt = 0; y1 = 0; y2 = 0; yt = 0; ht = 0; break;
                }
                DoubleAnimation animx = new DoubleAnimation(x1, x2, TimeSpan.FromSeconds(xt));
                rect.BeginAnimation(Canvas.LeftProperty, animx);
                DoubleAnimation animy = new DoubleAnimation(y1, y2, TimeSpan.FromSeconds(yt));
                rect.BeginAnimation(Canvas.TopProperty, animy);
                DoubleAnimationUsingKeyFrames animh = new DoubleAnimationUsingKeyFrames();
                animh.Duration = TimeSpan.FromSeconds(ht);
                animh.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.0))));
                animh.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.5))));
                animh.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(ht - 0.5))));
                animh.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(ht))));
                animh.Completed += Next;
                rect.BeginAnimation(Rectangle.OpacityProperty, animh);

            }
        }
        */
    }
    /*class BattleButton : Canvas
    {
        public bool IsSelected = false;
        public List<BattleButton> DependecnyButtons;
        public ShipCommands Command = ShipCommands.None;
        Ellipse Ell;
        Rectangle rect;
        ScaleTransform Scale;
        public BattleButton(Brush Brush, bool needevent)
        {
            Width = 80; Height = 80;
            rect = new Rectangle();
            rect.Width = 80; rect.Height = 80; rect.Fill = Brush;
            Children.Add(rect);
            Ell = new Ellipse(); Ell.Width = 64; Ell.Height = 64;
            Children.Add(Ell); Canvas.SetLeft(Ell, 8); Canvas.SetTop(Ell, 8);
            Scale = new ScaleTransform();
            RenderTransformOrigin = new Point(0.5, 0.5);
            RenderTransform = Scale;
            if (needevent)
                this.PreviewMouseDown += BattleButton_PreviewMouseDown;
        }
        public void PutToCanvas(Canvas canvas, double left, double top)
        {
            canvas.Children.Add(this);
            Canvas.SetLeft(this, left);
            Canvas.SetTop(this, top);
        }
        private void BattleButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsSelected) DeSelect();
            else Select();
        }
        public void Select()
        {
            IsSelected = true;
            Scale.ScaleX = 0.9; Scale.ScaleY = 0.9;
            Ell.Fill = new SolidColorBrush(Color.FromArgb(50, 0, 255, 0));
            if (DependecnyButtons != null)
                foreach (BattleButton btn in DependecnyButtons)
                {
                    if (btn == this) continue;
                    if (btn.IsSelected) btn.SimpleDeSelect();
                }
            if (Command != ShipCommands.None)
                IntBoya.CurrentCommand = Command;
        }
        public void SimpleSelect()
        {
            Scale.ScaleX = 0.9; Scale.ScaleY = 0.9;
            Ell.Fill = new SolidColorBrush(Color.FromArgb(50, 0, 255, 0));
            IsSelected = true;
        }
        public void SimpleDeSelect()
        {
            Scale.ScaleX = 1.0; Scale.ScaleY = 1.0;
            Ell.Fill = Brushes.Transparent;
            IsSelected = false;
        }
        public void DeSelect()
        {
            Scale.ScaleX = 1.0; Scale.ScaleY = 1.0;
            Ell.Fill = Brushes.Transparent;
            IsSelected = false;
            if (Command != ShipCommands.None)
                IntBoya.CurrentCommand = ShipCommands.None;
        }
    }
    */
    class SpeedButton : Canvas
    {
        //Ellipse SpeedEllipse;
        //Label SpeedLabel;
        Rectangle Rect;
        public SpeedButton() : base()
        {
            Width = 60; Height = 70;
            Rect = Common.GetRectangle(70, Gets.GetIntBoyaImage("2/x1"));
            Children.Add(Rect);
            //SpeedEllipse = new Ellipse();
            //SpeedEllipse.Width = 40; SpeedEllipse.Height = 40;
            //SpeedEllipse.Stroke = Brushes.Black; SpeedEllipse.StrokeThickness = 3;
            //SpeedEllipse.Fill = GetColorBrush(Colors.Red);
            //Children.Add(SpeedEllipse);
            //Canvas.SetLeft(SpeedEllipse, 20);
            //Canvas.SetTop(SpeedEllipse, 20);
            //SpeedLabel = new Label();
            //SpeedLabel.Width = 40; SpeedLabel.Height = 40;
            //SpeedLabel.Style = Links.MediumTextStyle;
            //SpeedLabel.Content = "X1";
            //SpeedLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            //SpeedLabel.VerticalContentAlignment = VerticalAlignment.Center;
            //Children.Add(SpeedLabel);
            //Canvas.SetLeft(SpeedLabel, 20); Canvas.SetTop(SpeedLabel, 20);
            PreviewMouseDown += Btn_PreviewMouseDown;
        }

        private void Btn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            switch (IntBoya.SpeedValue)
            {
                case 1: IntBoya.SpeedValue = 2; Rect.Fill = Gets.GetIntBoyaImage("2/x2"); Links.ShootAnimSpeed = 0.5; Links.ShipMoveSpeed = 1000; Links.ShipRotateSpeed = 400; break;
                case 2: IntBoya.SpeedValue = 4; Rect.Fill = Gets.GetIntBoyaImage("2/x4"); Links.ShootAnimSpeed = 0.25; Links.ShipMoveSpeed = 2000; Links.ShipRotateSpeed = 800; break;
                case 4: IntBoya.SpeedValue = 1; Rect.Fill = Gets.GetIntBoyaImage("2/x1"); Links.ShootAnimSpeed = 1; Links.ShipMoveSpeed = 500; Links.ShipRotateSpeed = 200; break;
                //case 1: IntBoya.SpeedValue = 2; SpeedEllipse.Fill = GetColorBrush(Colors.Green); SpeedLabel.Content = "X2"; Links.ShootAnimSpeed = 0.5; Links.ShipMoveSpeed = 1000; Links.ShipRotateSpeed = 400; break;
               // case 2: IntBoya.SpeedValue = 4; SpeedEllipse.Fill = GetColorBrush(Colors.Blue); SpeedLabel.Content = "X4"; Links.ShootAnimSpeed = 0.25; Links.ShipMoveSpeed = 2000; Links.ShipRotateSpeed = 800; break;
               // case 4: IntBoya.SpeedValue = 1; SpeedEllipse.Fill = GetColorBrush(Colors.Red); SpeedLabel.Content = "X1"; Links.ShootAnimSpeed = 1; Links.ShipMoveSpeed = 500; Links.ShipRotateSpeed = 200; break;
            }
        }
        static Brush GetColorBrush(Color color)
        {
            RadialGradientBrush result = new RadialGradientBrush();
            result.GradientOrigin = new Point(0.7, 0.3);
            result.GradientStops.Add(new GradientStop(Colors.White, 0));
            result.GradientStops.Add(new GradientStop(color, 1));
            return result;
        }
    }

    class AutoModePanel : StackPanel
    {
        Label LeftLabel = new Label();
        Label RightLabel = new Label();
        static ImageBrush Man = Gets.GetIntBoyaImage("Man");
        static ImageBrush Comp = Gets.GetIntBoyaImage("Comp");
        //ImageBrush Manipulator = new ImageBrush(Links.PicsList["I002_Manipulator"]);
        public AutoModePanel()
        {
            Orientation = Orientation.Vertical;
            LeftLabel.Width = 100;
            LeftLabel.Height = 100;
            RightLabel.Width = 100;
            RightLabel.Height = 100;
            Path path = new Path();
            path.Height = 10;
            path.Stroke = Brushes.Black;
            path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,2.5 h35"));
            Children.Add(LeftLabel);
            Children.Add(path);
            Children.Add(RightLabel);
            SetImages(0);
        }
        public void SetImages(byte b)
        {
            if ((b & 1) > 0)
                LeftLabel.Background = Comp;
            else
                LeftLabel.Background = Man;
            if ((b & 2) > 0)
                RightLabel.Background = Comp;
            else
                RightLabel.Background = Man;
        }
    }
}
