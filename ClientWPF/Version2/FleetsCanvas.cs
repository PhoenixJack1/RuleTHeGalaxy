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
    class FleetsCanvas:Canvas
    {
        StackPanel FleetsListPanel;
        public static GSShip Ship;
        FleetFilter Filter;
        public Viewbox box;
        TextBlock TitleBlock;
        GSStar DestinationStar;
        int Minships; int Maxships;
        bool Ispatrol; bool IsReturn;
        GameButton Ships, Artefacts;
        public FleetsCanvas()
        {
            box = new Viewbox();
            box.Width = 1920;
            box.Height = 930;
            box.Margin = new Thickness(0, 150, 0, 0);
            box.HorizontalAlignment = HorizontalAlignment.Center;
            box.VerticalAlignment = VerticalAlignment.Center;
            box.Child = this;
            put();
        }
        public void Select()
        {
            if (Links.Controller.SelectModifier == SelectModifiers.FleetForShip)
            {
                //Filter.ShowFreeFleets();
                DestinationStar = null;
                Minships = 0; Maxships = 255; Ispatrol = true; IsReturn = true;
                Ship = Links.Helper.GetShip();
                TitleBlock.Text = "Выберите флот, в котором разместить корабль";
                //Filter.Visibility = Visibility.Hidden;
                TitleBlock.Visibility = Visibility.Visible;
                Ships.Visibility = Visibility.Visible;
                Artefacts.Visibility = Visibility.Hidden;
            }
            else if (Links.Controller.SelectModifier == SelectModifiers.FleetForArtefact)
            {
                //Filter.ShowFreeFleets();
                DestinationStar = null;
                Minships = 0; Maxships = 255; Ispatrol = true; IsReturn = true;
                TitleBlock.Text = "Выберите флот, в котором разместить артефакт";
                //Filter.Visibility = Visibility.Hidden;
                TitleBlock.Visibility = Visibility.Visible;
                Ships.Visibility = Visibility.Hidden;
                Artefacts.Visibility = Visibility.Visible;
            }
            else if (Links.Controller.SelectModifier==SelectModifiers.FleetForMission)
            {
                //Filter.ShowFreeFleets();
                DestinationStar = Links.Stars[Mission2Panel.CurrentMission.StarID];
                Minships = 0; Maxships = 255; Ispatrol = true; IsReturn = true;
                TitleBlock.Text = "Выберите флот, который отправится на задание";
                //Filter.Visibility = Visibility.Hidden;
                TitleBlock.Visibility = Visibility.Visible;
                Ships.Visibility = Visibility.Hidden;
                Artefacts.Visibility = Visibility.Hidden;
            }
            else if (Links.Controller.SelectModifier==SelectModifiers.FleetForStory)
            {
                DestinationStar = StoryLinePanel.CurrentStory.GetStar();
                Minships = 0; Maxships = StoryLinePanel.CurrentStory.BattleFieldParam.MaxShips; Ispatrol = true; IsReturn = true;
                TitleBlock.Text = "Выберите флот, который отправится на сюжетное задание";
                //Filter.Visibility = Visibility.Hidden;
                TitleBlock.Visibility = Visibility.Visible;
                Ships.Visibility = Visibility.Hidden;
                Artefacts.Visibility = Visibility.Hidden;
            }
            else if (Links.Controller.SelectModifier==SelectModifiers.FleetForScout)
            {
                DestinationStar = Links.Helper.Star;
                Minships = 0; Maxships = 255; Ispatrol = true; IsReturn = true;
                TitleBlock.Text = "Выберите флот, который отправится на патрулирование";
                //Filter.Visibility = Visibility.Hidden;
                TitleBlock.Visibility = Visibility.Visible;
                Ships.Visibility = Visibility.Hidden;
                Artefacts.Visibility = Visibility.Hidden;
            }
            else if (Links.Controller.SelectModifier==SelectModifiers.FleetForPillage)
            {
                DestinationStar = Links.Helper.Planet.Star;
                Minships = 5; Maxships = 255; Ispatrol = true; IsReturn = true;
                TitleBlock.Text = "Выберите флот, который отрправится на грабёж";
                TitleBlock.Visibility = Visibility.Visible;
                Ships.Visibility = Visibility.Hidden;
                Artefacts.Visibility = Visibility.Hidden;
            }
            else if (Links.Controller.SelectModifier==SelectModifiers.FleetForConquer)
            {
                DestinationStar = Links.Helper.Planet.Star;
                Minships = 7; Maxships = 255; Ispatrol = true; IsReturn = true;
                TitleBlock.Text = "Выберите флот, который отправится на захват";
                TitleBlock.Visibility = Visibility.Visible;
                Ships.Visibility = Visibility.Hidden;
                Artefacts.Visibility = Visibility.Hidden;
            }
            else if (Links.Controller.SelectModifier==SelectModifiers.SpecialFleet)
            {
                DestinationStar = null;
                Minships = 0; Maxships = 255; Ispatrol = false; IsReturn = false;
                Ship = null;
                Gets.GetTotalInfo("После открытия панели с флотами и выбора конкретного флота");
                Filter.ShowAllFleets();
                Filter.Visibility = Visibility.Visible;
                TitleBlock.Visibility = Visibility.Hidden;
                Ships.Visibility = Visibility.Visible;
                Artefacts.Visibility = Visibility.Visible;
            }
            else
            {
                //Filter.ShowAllFleets();
                DestinationStar = null;
                Minships = 0; Maxships = 255; Ispatrol = false; IsReturn = false;
                Ship = null;
                Gets.GetTotalInfo("После открытия панели с флотами");
                Filter.Visibility = Visibility.Visible;
                TitleBlock.Visibility = Visibility.Hidden;
                Ships.Visibility = Visibility.Visible;
                Artefacts.Visibility = Visibility.Visible;
            }
            Refresh();
        }
        //public void Select(GSShip ship)
       // {
       //     Ship = ship;
       //     Refresh();
        //}
        public void put()
        {
            Width = 1280;
            Height = 600;
            Filter = new FleetFilter(new string[] { "Все", "Свободные", "В патруле", "Возвращающиеся"});
            Children.Add(Filter);
            Canvas.SetLeft(Filter, 450);
            Canvas.SetTop(Filter, 0); Canvas.SetZIndex(Filter, 20);
            TitleBlock = Common.GetBlock(40, "", Brushes.White, 1000);
            Children.Add(TitleBlock); Canvas.SetTop(TitleBlock, 50); Canvas.SetLeft(TitleBlock, 140);
            TitleBlock.Visibility = Visibility.Hidden;
            ScrollViewer viewer = new ScrollViewer();
            viewer.Width = 1270;
            viewer.Height = 530;
            //viewer.Background = Brushes.Blue;
            viewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            viewer.HorizontalContentAlignment = HorizontalAlignment.Center;
            Children.Add(viewer);
            FleetsListPanel = new StackPanel();
            FleetsListPanel.Orientation = Orientation.Horizontal;
            //FleetsListPanel.Background = Brushes.Red;
            viewer.Content = FleetsListPanel;
            Canvas.SetTop(viewer, 80);
            Canvas.SetLeft(viewer, 5);
            Ships = new GameButton(200, 60, "Корабли", 30);
            Children.Add(Ships); Canvas.SetLeft(Ships, 100);
            Ships.PreviewMouseDown += Ships_PreviewMouseDown;
            Artefacts = new GameButton(200, 60, "Артефакты", 30);
            Children.Add(Artefacts); Canvas.SetLeft(Artefacts, 900);
            Artefacts.PreviewMouseDown += Artefacts_PreviewMouseDown;
        }

        private void Artefacts_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.SelectPanel(GamePanels.Artefacts, SelectModifiers.None);
        }

        private void Ships_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.SelectPanel(GamePanels.ShipsCanvas, SelectModifiers.None);
        }

        public void Refresh()
        {
            FleetsListPanel.Children.Clear();
            FleetInfoPanelMode mode = FleetInfoPanelMode.Standard;
            if (Ship != null)
                mode = FleetInfoPanelMode.SelectForShip;
            else if (Links.Controller.SelectModifier == SelectModifiers.FleetForStory)
                mode = FleetInfoPanelMode.SelectForStory;
            else if (Links.Controller.SelectModifier == SelectModifiers.FleetForMission)
                mode = FleetInfoPanelMode.SelectForMission;
            else if (Links.Controller.SelectModifier == SelectModifiers.FleetForArtefact)
                mode = FleetInfoPanelMode.SelectForArtefact;
            else if (Links.Controller.SelectModifier == SelectModifiers.FleetForScout)
                mode = FleetInfoPanelMode.SelectForScout;
            else if (Links.Controller.SelectModifier == SelectModifiers.FleetForPillage)
                mode = FleetInfoPanelMode.SelectForPillage;
            else if (Links.Controller.SelectModifier == SelectModifiers.FleetForConquer)
                mode = FleetInfoPanelMode.SelectForConquer;
            List<FleetInfoPanel> infopanels = new List<FleetInfoPanel>();
            foreach (Land land in GSGameInfo.PlayerLands.Values)
            {
                foreach (GSFleet fleet in land.Fleets.Values)
                {
                    switch (Filter.curvalue)
                    {
                        case 0: break;
                        case 1: if (fleet.Target != null) continue; break;
                        case 2: if (fleet.Target == null || fleet.Target.Mission != FleetMission.Scout) continue; break;
                        case 3: if (fleet.Target == null || fleet.Target.Order!=FleetOrder.Return) continue; break;
                        default: throw new Exception();
                    }
                    if (Ispatrol)
                        if (fleet.Target != null && fleet.Target.Mission == FleetMission.Scout)
                        { infopanels.Add(new FleetInfoPanel(fleet, mode, FleetErrorInfo.OnPatrol)); continue; }
                    if (IsReturn)
                        if (fleet.Target != null && fleet.Target.Order == FleetOrder.Return)
                        { infopanels.Add(new FleetInfoPanel(fleet, mode, FleetErrorInfo.Returning)); continue; }
                    if (Minships != 0 && fleet.Ships.Count < Minships)
                    { infopanels.Add(new FleetInfoPanel(fleet, mode, FleetErrorInfo.TooFewShips)); continue; }
                    if (Maxships != 255 && fleet.Ships.Count > Maxships)
                    { infopanels.Add(new FleetInfoPanel(fleet, mode, FleetErrorInfo.TooManyShips)); continue; }
                    if (DestinationStar != null)
                    {
                        GSStar fleetstar = fleet.FleetBase.Land.Planet.Star;
                        double distance = Math.Sqrt(Math.Pow(fleetstar.X - DestinationStar.X, 2) + Math.Pow(fleetstar.Y - DestinationStar.Y, 2))/10;
                        if (distance > fleet.FleetBase.Range) { infopanels.Add(new FleetInfoPanel(fleet, mode, FleetErrorInfo.TooFar)); continue; }
                    }
                    infopanels.Add(new FleetInfoPanel(fleet, mode, FleetErrorInfo.No));
                }
            }
            if (infopanels.Count == 0)
            {
                TextBlock No = Common.GetBlock(40, "Нет подходящих флотов", Brushes.White, 1270);
                FleetsListPanel.Children.Add(No);
            }
            else if (Links.Controller.SelectModifier==SelectModifiers.SpecialFleet)
            {
                FleetInfoPanel panel = new FleetInfoPanel(Links.Helper.Fleet, FleetInfoPanelMode.Standard, FleetErrorInfo.No);
                FleetsListPanel.Children.Add(panel);
            }
            else
            {
                List<FleetInfoPanel> newlist = new List<FleetInfoPanel>();
                for (int i = 0; i < 6; i++)
                {
                    FleetErrorInfo curinfo = (FleetErrorInfo)i;
                    for (int j = 0; j < infopanels.Count; j++)
                        if (infopanels[j].ErrorInfo == curinfo)
                        {
                            newlist.Add(infopanels[j]); infopanels.RemoveAt(j); j--;
                        }
                }
                foreach (FleetInfoPanel panel in newlist)
                    FleetsListPanel.Children.Add(panel);
            }

        }
    }
    class ShortFleetInfo:Border
    {
        GSFleet Fleet;
        public ShortFleetInfo(GSFleet fleet)
        {
            Fleet = fleet;
            Width = 400; Height = 300;
            CornerRadius = new CornerRadius(20); BorderBrush = Links.Brushes.SkyBlue; BorderThickness = new Thickness(4);
            Background = Brushes.Black;
            Canvas canvas = new Canvas();
            Child = canvas;
            FleetEmblem emblem = new FleetEmblem(fleet.Image);
            canvas.Children.Add(emblem); Canvas.SetLeft(emblem, 150); Canvas.SetTop(emblem, 10);

            Common.PutRect(canvas, 50, Links.Brushes.ShipImageBrush, 15, 15, false);
            Label maxshiplabel = Common.PutLabel(canvas, 70, 20, 40, fleet.Ships.Count.ToString());
            maxshiplabel.Foreground = Brushes.White;
            
            Common.PutRect(canvas, 50, Links.Brushes.RepairPict, 15, 70, false);
            Label repairlabel;
            if (fleet.Repair)
                repairlabel = Common.PutLabel(canvas, 70, 75, 40, "AUTO");
            else
                repairlabel = Common.PutLabel(canvas, 70, 75, 40, "SEMI");
            repairlabel.Foreground = Brushes.White;

            if (fleet.Ships.Count == 0)
                Common.PutLabel(canvas, 60, 120, 40, "-----", Brushes.White);
            else
            {
                int shiphealthsumm = 0;
                foreach (GSShip ship in fleet.Ships.Values)
                    shiphealthsumm += ship.Health;
                shiphealthsumm = shiphealthsumm / fleet.Ships.Count;
                Common.PutLabel(canvas, 40, 120, 40, string.Format("({0}%)", shiphealthsumm), Brushes.White);
            }
            
            UIElement range = Common.GetRectangle(50, Links.Brushes.Radar, Links.Interface("FleetsSpeed"));
            canvas.Children.Add(range); Canvas.SetLeft(range, 260); Canvas.SetTop(range, 20);
            Common.PutLabel(canvas, 320, 20, 40, fleet.FleetBase.Range.ToString(), Brushes.White);
            
            UIElement health = Common.GetRectangle(50, new DrawingBrush(WeapCanvas.WarpDraw), Links.Interface("WarpHealth"));
            canvas.Children.Add(health); Canvas.SetLeft(health, 260); Canvas.SetTop(health, 70);
            Common.PutLabel(canvas, 310, 75, 40, fleet.FleetBase.MaxMath.ToString(), Brushes.White);
            
            Border InfoBorder = new Border();
            InfoBorder.Width = 380; InfoBorder.Height = 100; InfoBorder.CornerRadius = new CornerRadius(20);
            InfoBorder.BorderBrush = Links.Brushes.SkyBlue; InfoBorder.BorderThickness = new Thickness(2);
            canvas.Children.Add(InfoBorder);
            Canvas.SetLeft(InfoBorder, 9); Canvas.SetTop(InfoBorder, 190);
            if (fleet.Target==null)
            {
                InfoBorder.Child = Common.GetBlock(30, Links.Interface("FleetFree"), Brushes.White, 380);
            }
            else if (Fleet.Target.Order==FleetOrder.InBattle)
            {
                InfoBorder.Child = Common.GetBlock(30, Links.Interface("FleetInBattle"), Brushes.White, 380);
            }
            else if (Fleet.Target.Order==FleetOrder.Return)
            {
                InfoBorder.Child = new FleetInfoPanel.ReturnIndicator(Fleet);
            }
            else if (Fleet.Target.Order==FleetOrder.Defense && Fleet.Target.Mission==FleetMission.Scout)
            {
                InfoBorder.Child = Common.GetBlock(30, Links.Interface("FleetInScout"), Brushes.White, 380);
            }
            else if (fleet.Target.Mission==FleetMission.Pillage)
            {
                InfoBorder.Child = Common.GetBlock(30, Links.Interface("FleetPilage"), Brushes.White, 380);
            }
            else if (fleet.Target.Mission==FleetMission.Conquer)
            {
                InfoBorder.Child = Common.GetBlock(30, Links.Interface("FleetConquer"), Brushes.White, 300);
            }
            PreviewMouseDown += ShortFleetInfo_PreviewMouseDown;
        }

        private void ShortFleetInfo_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            FleetParamsPanel parampanel = new FleetParamsPanel(Fleet);
            if (Links.Controller.CurPanel==GamePanels.Galaxy)
                Links.Controller.PopUpCanvas.Change(parampanel,"Отображение панели по возврату флота");
            return;
        }
    }
}
