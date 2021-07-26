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
    public enum FleetMission
    {
       Mission, Pillage, Conquer, StoryLine, Scout, TestBattle
    };
    public enum FleetOrder
    {
        Start,
        InBattle,
        Defense,
        Return
    }
    public class GSFleet
    {
        public SortedList<long, GSShip> Ships;
        public long ID;
        public byte[] Image;
        public bool Repair;
        public FleetTarget Target;
        public FleetBase FleetBase;
        public List<Artefact> Artefacts;
        /// <summary> добавление флота для тестовых боёв</summary>
        public GSFleet(long id)
        {
            GSGameInfo.Fleets.Add(id, this);
        }
        public GSFleet(int landid, byte[] array, ref int i)
        {
            ID = BitConverter.ToInt64(array, i); i += 8;
            Image = BitConverter.GetBytes(BitConverter.ToInt32(array, i)); i += 4;
            byte sectorpos = array[i]; i++;
            Repair = BitConverter.ToBoolean(array, i); i++;
            Artefacts = new List<Artefact>();
            byte artcount = array[i]; i++;
            for (int j=0;j<artcount; j++)
            { Artefacts.Add(Links.Artefacts[BitConverter.ToUInt16(array, i)]); i += 2; }
            Target = FleetTarget.GetFleetTarget(array, ref i);
            byte ships = array[i]; i++;
            Land land = GSGameInfo.PlayerLands[landid];
            FleetBase = (FleetBase)land.Locations[sectorpos];
            land.Fleets.Add(ID, this);
            GSGameInfo.Fleets.Add(ID, this);
            Ships = new SortedList<long, GSShip>();
            for (int j=0;j< ships;j++)
            {
                long shipid = BitConverter.ToInt64(array, i); i += 8;
                Ships.Add(shipid, GSGameInfo.Ships[shipid]);
                GSGameInfo.Ships[shipid].Fleet = this;
            }
            FleetBase.SetFleet(this);
        }
        public int GetActiveShips()
        {
            if (Ships.Count == 0) return 0;
            int count = 0;
            foreach (GSShip ship in Ships.Values)
                if (ship.Health > 0) count++;
            return count;
        }
        public bool CheckHealth()
        {
            if (Target != null) return false;
            if (Ships.Count == 0) return false;
            if (Ships.Count > FleetBase.MaxShips) return false;
            if (FleetBase.Range < 0) return false;
            if (FleetBase.MaxMath <= 0) return false;
            int shipsum = 0;
            foreach (GSShip ship in Ships.Values)
                shipsum += ship.Health;
            if (shipsum == 0) return false;
            return true;
        }        
        public FleetEmblem GetEnemyEmblem()
        {
            foreach (Battle battle in GSGameInfo.Battles.Values)
            {
                if (battle.Fleet1ID==ID) return new FleetEmblem(battle.Emblem2);
                if (battle.Fleet2ID == ID) return new FleetEmblem(battle.Emblem1);
            }
            return null;
        }
        public int GetColonyPower()
        {
            int result = 0;
            foreach (GSShip ship in Ships.Values)
            {
                if (ship.Health == 0 || ship.Pilot==null) continue;
                result += ship.Colony;
            }
            return result;
        }
        public int GetCargoPower()
        {
            int result = 0;
            foreach (GSShip ship in Ships.Values)
            {
                if (ship.Health == 0 || ship.Pilot == null) continue;
                result += ship.Cargo;
            }
            return result;
        }
           
    }
    enum FleetInfoPanelMode { Standard, SelectForShip, SelectForStory, SelectForMission, SelectForArtefact, SelectForScout, SelectForPillage, SelectForConquer }
    enum FleetErrorInfo { No, TooManyShips, TooFewShips, TooFar, OnPatrol, Returning }
    class FleetInfoPanel : Canvas
    {
        public GSFleet Fleet;
        static Point[] ViewListPoints = new Point[]{new Point(170,5),new Point(230,40),
            new Point(110,40),new Point(90,130), new Point(250,130),new Point(20,200),
            new Point(90,200),new Point(170,200), new Point(250,200), new Point(320,200),
            new Point(55,275), new Point(125,275), new Point(205,275), new Point(285,275)};
        public FleetErrorInfo ErrorInfo;
        public FleetInfoPanel(GSFleet fleet, FleetInfoPanelMode mode, FleetErrorInfo errorinfo)
        {
            Fleet = fleet;
            ErrorInfo = errorinfo;
            int ShipsCountModi = 0;
            if (fleet.Ships.Count > 5 && fleet.Ships.Count <= 10) ShipsCountModi = 1;
            else if (fleet.Ships.Count > 10) ShipsCountModi = 2;
            Width = 400;
            Height = 400 + ShipsCountModi * 75;
            Margin = new Thickness(2);
            Path path1 = new Path();
            path1.Stroke = Links.Brushes.SkyBlue;
            path1.StrokeThickness = 2;
            Children.Add(path1);
            path1.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(string.Format(
                "M0,200 a100,100 0 0,0 100,-100 a50,50 0 0,1 200,0 a100,100 0 0,0 100,100 v{0} h-400z", ShipsCountModi * 75)));

            path1.Fill = Common.GetLinearBrush(Colors.Black, Colors.DarkBlue, Colors.Black);
            path1.StrokeLineJoin = PenLineJoin.Round;

            Path path2 = new Path();
            path2.Stroke = Links.Brushes.SkyBlue;
            path2.StrokeThickness = 2;
            Children.Add(path2);
            path2.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(string.Format(
                "M0,{0} h400 v200 h-400z M0,{0} h400", 200 + ShipsCountModi * 75)));

            LinearGradientBrush back = new LinearGradientBrush();
            back.GradientStops.Add(new GradientStop(Colors.Black, 0.6));
            back.GradientStops.Add(new GradientStop(Colors.SkyBlue, 0));
            back.StartPoint = new Point(0.5, 1);
            back.EndPoint = new Point(0.5, 0);
            path2.Fill = back;

            Rectangle BackRect = new Rectangle();
            BackRect.Width = 398;
            BackRect.Height = 98;
            Canvas.SetTop(BackRect, 300 + ShipsCountModi * 75);
            Links.Brushes.HexSmallImage.TileMode = TileMode.FlipXY;
            Links.Brushes.HexSmallImage.Viewport = new Rect(0, 0, 0.2, 0.40);
            BackRect.Fill = Links.Brushes.HexSmallImage;
            LinearGradientBrush oppbrush = Common.GetVertLinearBrush(Colors.Black, Colors.Black, Colors.Transparent, 1, 0.8, 0);
            BackRect.OpacityMask = oppbrush;
            Children.Add(BackRect);
            //Common.GetLinearBrush(Colors.White, Colors.SkyBlue, Colors.LightSkyBlue);

            FleetEmblem emblem = new FleetEmblem(fleet.Image);
            Children.Add(emblem);
            Canvas.SetLeft(emblem, 150); Canvas.SetTop(emblem, 80);

            Viewbox[] ViewList = new Viewbox[fleet.Ships.Count];
            for (int i = 0; i < fleet.Ships.Count; i++)
            {
                Viewbox vbx = new Viewbox();
                vbx.Width = 60;
                vbx.Height = 60;
                Children.Add(vbx);
                Canvas.SetLeft(vbx, ViewListPoints[i].X);
                Canvas.SetTop(vbx, ViewListPoints[i].Y);
                ViewList[i] = vbx;
            }

            for (int i = 0; i < fleet.Ships.Count; i++)
            {
                //if (Fleet.Ships.Count <= i) break;
                GSShip ship = Fleet.Ships.Values[i];
                ShipB shipb = new ShipB((byte)(i + 1), ship.Schema, ship.Health, ship.Pilot, BitConverter.GetBytes(ship.Model), ShipSide.Attack, null, false, null, ShipBMode.Info,255);
                ViewList[i].Child = shipb.HexShip;
                shipb.HexShip.PreviewMouseDown += new MouseButtonEventHandler(ShipOut);
            }
            StackPanel TopPanel = new StackPanel();
            TopPanel.Orientation = Orientation.Vertical;
            TopPanel.Width = 400;
            Canvas.SetTop(TopPanel, 200 + ShipsCountModi * 75);
            Children.Add(TopPanel);

            Label LandNameLabel = Common.CreateLabel(20, fleet.FleetBase.Land.Name.ToString(), Brushes.White);
            TopPanel.Children.Add(LandNameLabel);

            Common.PutRect(this, 30, Links.Brushes.ShipImageBrush, 5, 235 + ShipsCountModi * 75, false);
            Label maxshiplabel = Common.PutLabel(this, 50, 235+ShipsCountModi*75, 20, string.Format("{0}/{1}", fleet.Ships.Count, fleet.FleetBase.MaxShips));
            maxshiplabel.Foreground = Brushes.White;

            Common.PutRect(this, 30, Links.Brushes.RepairPict, 110, 235 + ShipsCountModi * 75, false);
            Label repairlabel;
            if (fleet.Repair)
                repairlabel = Common.PutLabel(this, 140, 235 + ShipsCountModi * 75, 20, "AUTO");
            else
                repairlabel = Common.PutLabel(this, 140, 235 + ShipsCountModi * 75, 20, "SEMI");
            repairlabel.Foreground = Brushes.White;

            if (fleet.Ships.Count == 0)
                Common.PutLabel(this, 180, 235, 20, "(-)", Brushes.White);
            else
            {
                int shiphealthsumm = 0;
                foreach (GSShip ship in fleet.Ships.Values)
                    shiphealthsumm += ship.Health;
                shiphealthsumm = shiphealthsumm / fleet.Ships.Count;
                Common.PutLabel(this, 180, 235 + ShipsCountModi * 75, 20, string.Format("({0}%)", shiphealthsumm), Brushes.White);
            }

            UIElement range = Common.GetRectangle(30, Links.Brushes.Radar, Links.Interface("FleetsSpeed"));
            Children.Add(range); Canvas.SetLeft(range, 330); Canvas.SetTop(range, 205 + ShipsCountModi * 75);
            Common.PutLabel(this, 365, 205 + ShipsCountModi * 75, 20, fleet.FleetBase.Range.ToString(), Brushes.White);

            UIElement health = Common.GetRectangle(30, new DrawingBrush(WeapCanvas.WarpDraw), Links.Interface("WarpHealth"));
            Children.Add(health); Canvas.SetLeft(health, 10); Canvas.SetTop(health, 205 + ShipsCountModi * 75);
            Common.PutLabel(this, 40, 205 + ShipsCountModi * 75, 20, fleet.FleetBase.MaxMath.ToString(), Brushes.White);

            UIElement colony = Common.GetRectangle(30, Pictogram.ColonyBrush, Links.Interface("Colony"));
            Children.Add(colony); Canvas.SetLeft(colony, 10); Canvas.SetTop(colony, 265 + ShipsCountModi * 75);
            Common.PutLabel(this, 50, 265 + ShipsCountModi * 75, 20, Fleet.GetColonyPower().ToString(), Brushes.White);
            //Common.PutLabel(this, 50, 265, 20, "4000", Brushes.White);
            UIElement cargo = Common.GetRectangle(30, Pictogram.CargoBrush, Links.Interface("Cargo"));
            Children.Add(cargo); Canvas.SetLeft(cargo, 110); Canvas.SetTop(cargo, 265 + ShipsCountModi * 75);
            Common.PutLabel(this, 150, 265 + ShipsCountModi * 75, 20, (Fleet.GetCargoPower() / 10.0).ToString() + "%", Brushes.White);


            Border Art1 = Common.PutBorder(50, 50, Links.Brushes.SkyBlue, 2, 235, 240 + ShipsCountModi * 75, this);
            if (fleet.Artefacts.Count > 0)
                Art1.Child = Fleet.Artefacts[0].GetInFleetRectangle(45, Fleet);
            Border Art2 = Common.PutBorder(50, 50, Links.Brushes.SkyBlue, 2, 290, 240 + ShipsCountModi * 75, this);
            if (fleet.Artefacts.Count > 1)
                Art2.Child = Fleet.Artefacts[1].GetInFleetRectangle(45, Fleet);
            Border Art3 = Common.PutBorder(50, 50, Links.Brushes.SkyBlue, 2, 345, 240 + ShipsCountModi * 75, this);
            if (fleet.Artefacts.Count == 3)
                Art3.Child = Fleet.Artefacts[2].GetInFleetRectangle(45, fleet);

            Border FleetMissionBorder = Common.PutBorder(360, 90, Links.Brushes.SkyBlue, 4, 20, 300 + ShipsCountModi * 75, this);
            FleetMissionBorder.CornerRadius = new CornerRadius(2);
            FleetMissionBorder.Background = Common.GetVertLinearBrush(Colors.Blue, Colors.Blue, Colors.SkyBlue, 0, 0.2, 1);
            //Fleet.Mission = FleetMission.AtackEnemy;
            //Fleet.Mission = FleetMission.Resources;
            if (Fleet.Target == null)
            {
                FleetMissionBorder.Child = GetMissionBoxBlock("FleetFree");
            }
            else if (Fleet.Target.Order==FleetOrder.Return)
            {
                FleetMissionBorder.Child = new ReturnIndicator(Fleet);
            }
            else if (Fleet.Target.Order==FleetOrder.InBattle)
            {
                FleetMissionBorder.Child = GetMissionBoxBlock("FleetInBattle");
            }
            else
            {
                switch (Fleet.Target.Mission)
                {
                    case FleetMission.Scout:
                        FleetMissionBorder.Child = GetMissionBoxBlock("FleetInScout"); break;
                    case FleetMission.Conquer:
                        FleetMissionBorder.Child = GetMissionBoxBlock("FleetConquer"); break;
                    case FleetMission.Pillage:
                        FleetMissionBorder.Child = GetMissionBoxBlock("FleetPilage"); break;
                    case FleetMission.Mission:
                        FleetMissionBorder.Child = GetMissionBoxBlock("FleetResources"); break;
                    case FleetMission.StoryLine:
                        FleetMissionBorder.Child = GetStoryLineBoxBlock(Fleet.Target.TargetID); break;
                }
            }
            if (mode == FleetInfoPanelMode.SelectForShip && ErrorInfo == FleetErrorInfo.No)
                PreviewMouseDown += new MouseButtonEventHandler(SelectFleetForShip);
            else if (mode == FleetInfoPanelMode.SelectForStory && ErrorInfo == FleetErrorInfo.No)
                PreviewMouseDown += SelectFleetForStory;
            else if (mode == FleetInfoPanelMode.SelectForMission && ErrorInfo == FleetErrorInfo.No)
                PreviewMouseDown += SelectFleetForMission;
            else if (mode == FleetInfoPanelMode.SelectForArtefact && ErrorInfo == FleetErrorInfo.No)
                MouseDown += SelectFleetForArtefact;
            else if (mode == FleetInfoPanelMode.SelectForScout && ErrorInfo == FleetErrorInfo.No)
                MouseDown += SelectFleetForScout;
            else if (mode == FleetInfoPanelMode.SelectForPillage && ErrorInfo == FleetErrorInfo.No)
                MouseDown += SelectForPillage;
            else if (mode == FleetInfoPanelMode.SelectForConquer && ErrorInfo == FleetErrorInfo.No)
                MouseDown += SelectForConquer;
            else
                MouseDown += new MouseButtonEventHandler(FleetInfoPanel_MouseDown);
         
            if (errorinfo!=FleetErrorInfo.No)
            {
                Path path = new Path();
                path.Data = path1.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(string.Format(
                "M0,200 a100,100 0 0,0 100,-100 a50,50 0 0,1 200,0 a100,100 0 0,0 100,100 v{0} v200 h-400z", ShipsCountModi * 75)));
                Children.Add(path); path.Fill = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0));
                Rectangle rect = new Rectangle(); rect.Stroke = Brushes.Red; rect.StrokeThickness = 3; rect.Width = 300; rect.Height = 50;
                rect.RenderTransformOrigin = new Point(0.5, 0.5); rect.RenderTransform = new RotateTransform(-45);
                Children.Add(rect); Canvas.SetLeft(rect, 20); Canvas.SetTop(rect, 200);
                TextBlock block = Common.GetBlock(30, "", Brushes.White, 300);
                block.RenderTransformOrigin = new Point(0.5, 0.5); block.RenderTransform = new RotateTransform(-45);
                Children.Add(block); Canvas.SetLeft(block, 20); Canvas.SetTop(block, 200);
                switch (errorinfo)
                {
                    case FleetErrorInfo.OnPatrol: block.Text = "Флот на патрулировании"; break;
                    case FleetErrorInfo.Returning: block.Text = "Флот возвращается"; break;
                    case FleetErrorInfo.TooFar: block.Text = "Не сможет допрыгнуть"; break;
                    case FleetErrorInfo.TooFewShips: block.Text = "Недостаточно кораблей"; break;
                    case FleetErrorInfo.TooManyShips: block.Text = "Слишком много кораблей"; break;
                }
            }
        }
        private void SelectForConquer(object sender, MouseButtonEventArgs e)
        {
            Links.Helper.Fleet = Fleet;
            FleetMissions.ConquerFinal_Click(null, null);
        }
        private void SelectForPillage(object sender, MouseButtonEventArgs e)
        {
            Links.Helper.Fleet = Fleet;
            FleetMissions.PillageEnemyFinal_Click(null, null);
        }

        private void SelectFleetForScout(object sender, MouseButtonEventArgs e)
        {
            Links.Helper.Fleet = Fleet;
            FleetMissions.StartScout_Click(null, null);
        }

        private void SelectFleetForArtefact(object sender, MouseButtonEventArgs e)
        {
            if (Fleet.Artefacts.Count >= 3)
            { Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage("Во флоте уже есть 3 артефакта")); return; }
            string eventresult = Events.PutWarArtefactInFleet(Fleet);
            if (eventresult != "")
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult)); return;
            }
            else
            {
                Gets.GetTotalInfo("После события по размещению артефакта в флоте");
                Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.None);
            }
        }

        private void SelectFleetForMission(object sender, MouseButtonEventArgs e)
        {
            StartBattleEventResult result = Events.SendFleetToMission2Battle(new FleetParamsPanel(Fleet), Mission2Panel.CurrentMission);
            if (result.Result)
            {
                bool changetofleet = Mission2Panel.CurrentPanel.RecieveMissionResult(result.Battleid);
                if (changetofleet) Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.None);
            }
            else
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(result.ErrorText));
        }
        private void SelectFleetForStory(object sender, MouseButtonEventArgs e)
        {
            StartBattleEventResult result = Events.SendFleetToStoryLine(new FleetParamsPanel(Fleet));
            if (result.Result)
                new StartBattlePanel(result.Battleid);
            else
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(result.ErrorText));
        }

        void bisybox_PreviewMouseDown_EnterInBattle(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Battle battle = null;
            foreach (Battle bat in GSGameInfo.Battles.Values)
                if (bat.Fleet1ID == Fleet.ID || bat.Fleet2ID == Fleet.ID)
                    battle = bat;
            if (battle!=null)
                Links.Controller.IntBoya.Place(battle.ID);
        }
        TextBlock GetStoryLineBoxBlock(int id)
        {
            TextBlock tb = new TextBlock();
            tb.Text = StoryLine2.StoryLines[(byte)id].Title;
            tb.FontFamily = Links.Font;
            tb.FontSize = 24;
            tb.FontWeight = FontWeights.Bold;
            tb.Foreground = Brushes.White;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.TextAlignment = TextAlignment.Center;
            return tb;
        }
        TextBlock GetMissionBoxBlock(string key)
        {
            TextBlock tb = new TextBlock();
            tb.Text = Links.Interface(key);
            tb.FontFamily = Links.Font;
            tb.FontSize = 24;
            tb.FontWeight = FontWeights.Bold;
            tb.Foreground = Brushes.White;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.TextAlignment = TextAlignment.Center;
            return tb;
        }
        void FleetInfoPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FleetParamsPanel parampanel = new FleetParamsPanel(Fleet);
            Links.Controller.PopUpCanvas.Place(parampanel);
            return;
        }
        
        void ShipOut (object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (Fleet.Target != null)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("FleetBusy")), true);
                return;
            }
            HexShip hex = (HexShip)sender;
            List<GSShip> ships = new List<GSShip>();
            string result = "";
            for (int i = 0; i < Fleet.Ships.Count; i++)
                if (i + 1 == hex.Ship.BattleID)
                {
                    result = Events.RemoveShipFromFleet(Fleet.Ships.Values[i]);
                }
            //ships.Add(Fleet.Ships.Values[i]);
            //string result = Events.MoveShipsToFleet(Fleet, ships);
            if (result == "")
                Links.Controller.FleetsCanvas.Select();
            else
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(result), true);
        }
        void SelectFleetForShip(object sender, MouseButtonEventArgs e)
        {
            //постановка корабля во флот
            //Ошибки
            //Флот занят
            //Нет пилота - проверяется раньше
            //Слишком много кораблей во флоте
            //Слишком много кораблей в колонии
            if (Fleet.Target != null)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("FleetBusy")), true);
                return;
            }
            GSShip Ship = FleetsCanvas.Ship;
            if (Fleet.Ships.Count>=Fleet.FleetBase.MaxShips)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("MaxFleetShips")), true);
                Links.Controller.ShipsCanvas.Select();
                return;
            }
            if (FleetsCanvas.Ship.Fleet != null && FleetsCanvas.Ship.Fleet.Target != null)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("ShipBusy")), true);
                Links.Controller.ShipsCanvas.Select();
                return;
            }
            if (Fleet.Ships.Count>Fleet.FleetBase.MaxShips)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("MaxLandShips")), true);
                Links.Controller.ShipsCanvas.Select();
                return;
            }
            //List<GSShip> ships = new List<GSShip>();
            //foreach (GSShip ship in Fleet.Ships.Values)
            //    if (ship.ID != FleetsPanel.Ship.ID)
            //        ships.Add(ship);
            //ships.Add(FleetsPanel.Ship);

            string result = Events.MoveShipToFleet(Fleet, FleetsCanvas.Ship);
            if (result == "")
                Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.None);
            else
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(result), true);
        }
       
        public class ReturnIndicator : TextBlock
        {
            GSFleet Fleet;
            public ReturnIndicator(GSFleet fleet)
            {
                Fleet = fleet;
                Style = Links.TextStyle;
                Foreground = Brushes.White;
                HorizontalAlignment = HorizontalAlignment.Center;
                VerticalAlignment = VerticalAlignment.Center;
                FontSize = 30;
                TextWrapping = TextWrapping.Wrap;
                double days = (Fleet.Target.FreeTime - GSGameInfo.ServerTime).TotalDays;
                if ((days - (int)days) > 0) days = (int)days + 1; else days = (int)days;
                string text = string.Format(Links.Interface("PremiumLeft"), days);
                Text = text;

            }
        }
        /*
        static void EnterInBattle_btn(object sender, RoutedEventArgs e)
        {
            Links.Controller.RoundButtons.Remove();
            Button btn = (Button)sender;
            GSFleet fleet = (GSFleet)btn.Tag;
            long fleetid = fleet.ID;
            long battleid = -1;
            foreach (Battle battle in GSGameInfo.Battles.Values)
                if (battle.Fleet1ID == fleetid || battle.Fleet2ID == fleetid)
                {
                    battleid = battle.ID;
                    break;
                }
            if (battleid == -1)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("BattleNotFinded")), true);
                return;
            }
            
            BattleCanvas.Place(battleid);
        }
       */
        static void ReformFleet_btn(object sender, RoutedEventArgs e)
        {
            Links.Controller.RoundButtons.Remove();
            GSFleet fleet = (GSFleet)((Button)sender).Tag;
            List<GSShip> ShipList = new List<GSShip>();
            foreach (GSShip ship in GSGameInfo.Ships.Values)
                if (ship.Fleet == null || ship.Fleet.ID == fleet.ID)
                    if (ship.Pilot != null)
                        ShipList.Add(ship);
            if (ShipList.Count == 0)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("NoAvailableShips")), true);
                return;
            }
            
            //Links.Controller.PopUpCanvas.Place(new ShipSelectWindow(ShipList,fleet), false);
        }
    }
    
    class TextFilter:Canvas
    {
        public TextFilter()
        {
            Width = 160;
            Height = 20;
            Path mainpath = new Path();
            mainpath.Stroke = Brushes.Black;
            mainpath.Fill = Brushes.Black;
            mainpath.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M20,00 h120 a10,10 0 0,1 0,20 h-120 a10,10 0 0,1 0,-20"));
            Children.Add(mainpath);
            Ellipse leftellipse = new Ellipse();
            leftellipse.Width = 18;
            leftellipse.Height = 18;
            leftellipse.Fill = Brushes.Black;
            leftellipse.Stroke = Brushes.White;
            Children.Add(leftellipse);
            Canvas.SetLeft(leftellipse, 11);
            Canvas.SetTop(leftellipse, 1);

            Ellipse rightellipse = new Ellipse();
            rightellipse.Width = 18;
            rightellipse.Height = 18;
            rightellipse.Width = 18;
            rightellipse.Height = 18;
            rightellipse.Fill = Brushes.Black;
            rightellipse.Stroke = Brushes.White;
            Children.Add(rightellipse);
            Canvas.SetLeft(rightellipse, 131);
            Canvas.SetTop(rightellipse, 1);

            Path leftarrow = new Path(); leftarrow.Fill = Brushes.White;
            leftarrow.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M13,10 l7,-7 a7,7 0 0,1 0,14z"));
            Children.Add(leftarrow);

            Path rightarrow = new Path(); rightarrow.Fill = Brushes.White;
            rightarrow.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M147,10 l-7,-7 a7,7 0 0,0 0,14z"));
            Children.Add(rightarrow);
        }
    }
     
    public class FleetTarget
    {
        public FleetTargetE TargetType;
        public int TargetID;
        public FleetMission Mission;
        public FleetOrder Order;
        public DateTime FreeTime;
        /// <summary> Класс для хранения назначения флота
        /// </summary>
        /// <param name="mission">Основная миссия флота. От неё завистит дальнейшее поведение флота после битвы</param>
        /// <param name="order">Текущее состояние задания</param>
        /// <param name="freetime"></param>
        /// <param name="type"></param>
        /// <param name="id"></param>
        public FleetTarget(FleetMission mission, FleetOrder order, long freetime, FleetTargetE type, int id)
        {
            Mission = mission;
            Order = order;
            FreeTime = new DateTime(freetime);
            TargetType = type;
            TargetID = id;
        }
        public static FleetTarget GetFleetTarget(byte[] array, ref int i)
        {
            byte havetarget = array[i]; i++;
            if (havetarget == 0) return null;
            FleetMission mission = (FleetMission)array[i]; i++;
            FleetOrder order = (FleetOrder)array[i]; i++;
            long freetime = BitConverter.ToInt64(array, i); i+=8;
            FleetTargetE type = (FleetTargetE)array[i]; i++;
            int id = 0;
            switch (type)
            {
                case FleetTargetE.Star:
                case FleetTargetE.Planet:
                case FleetTargetE.Land:
                case FleetTargetE.Avanpost:
                case FleetTargetE.Story: id = BitConverter.ToInt32(array, i); i += 4; break;
                case FleetTargetE.Mission: id = array[i]; i++; int starid = BitConverter.ToInt32(array, i); i += 4; byte orbit = array[i]; i++; break;
            }
            return new FleetTarget(mission, order, freetime, type, id);
        }
    }
         
}
