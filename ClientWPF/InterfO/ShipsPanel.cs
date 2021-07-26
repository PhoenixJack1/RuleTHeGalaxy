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
using System.Threading;

namespace Client
{
    class ShipsPanel:SelectedGrid
    {
        WrapPanel ShipsWrap;
        ShipFleetFilter ShipFleetFilter;
        ShipCountFilter ShipCountFilter;
        public ShipsPanel(string Name)
            : base(Name, Links.Controller.mainWindow.Buttons[6])
        {
            put();
        }
        void put()
        {
            Children.Clear();
            //ShowGridLines = true;
            RowDefinitions.Add(new RowDefinition());
            RowDefinitions.Add(new RowDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            RowDefinitions[0].Height = new GridLength(70);

            ShipFleetFilter = new Client.ShipFleetFilter();
            Children.Add(ShipFleetFilter);

            ShipCountFilter = new ShipCountFilter();
            Children.Add(ShipCountFilter);
            Grid.SetColumn(ShipCountFilter, 1);
            //ColumnDefinitions.Add(new ColumnDefinition()); ColumnDefinitions[0].Width = new GridLength(1150);
            //ColumnDefinitions.Add(new ColumnDefinition());

            Border ShipsBorder = new Border();
            ShipsBorder.Width = 1270;
            ShipsBorder.Height = 536;
            ShipsBorder.BorderBrush = Brushes.Black;
            ShipsBorder.BorderThickness = new Thickness(2);
            ShipsBorder.Margin = new Thickness(2);
            Children.Add(ShipsBorder);
            Grid.SetRow(ShipsBorder, 1);
            Grid.SetColumnSpan(ShipsBorder, 2);

            ScrollViewer ShipsViewer = new ScrollViewer();
            ShipsViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            ShipsViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            ShipsBorder.Child = ShipsViewer;

            ShipsWrap = new WrapPanel();
            //ShipsBorder.Child = ShipsWrap;
            ShipsViewer.Content = ShipsWrap;
            //ShipsWrap.Height = 606;

            //Border ButtonBorder = new Border();
            //ButtonBorder.BorderBrush = Brushes.Black;
            //ButtonBorder.BorderThickness = new Thickness(2);
            //ButtonBorder.Margin = new Thickness(2);
            //Children.Add(ButtonBorder);
            //Grid.SetColumn(ButtonBorder, 1);

        }
        public override void Select()
        {
            base.Select();
            Gets.GetTotalInfo("После открытия панели с кораблями");
            //ShipFleetFilter.Refresh();
            //Gets.GetShips();
            Refresh(false);
        }
        public void Refresh(bool onlypages)
        {
            ShipsWrap.Children.Clear();
            long CurrentFilterID = ShipFleetFilter.CurrentFleetID;
            this.Dispatcher.BeginInvoke((ThreadStart)delegate()
            {
                if (onlypages == false)
                {
                    List<GSShip> Ships = new List<GSShip>();
                    foreach (GSShip ship in GSGameInfo.Ships.Values)
                    {
                        //-1 - все корабли
                        //-2 - без пилотов
                        //-3 - без флотов
                        //-4 - не отремонтированные
                        switch (CurrentFilterID)
                        {
                            case -4: if (ship.Health == 100) continue; break;
                            case -3: if (ship.Fleet != null) continue; break;
                            case -2: if (ship.Pilot != null) continue; break;
                            case -1: break;
                            default:
                                if (ship.Fleet == null || ship.Fleet.ID != CurrentFilterID)
                                    continue;
                                break;
                        }
                        //ShipInfoPanel2 panel = new ShipInfoPanel2(ship);
                        //ShipsWrap.Children.Add(panel);
                        Ships.Add(ship);
                    }
                    ShipCountFilter.SetShips(Ships);
                }
                int page = ShipCountFilter.CurrentPage;
                int minpos = (page - 1) * 6;
                int maxpos = page * 6;
                for (int i=minpos; i<maxpos; i++)
                {
                    if (i >= ShipCountFilter.Ships.Count) break;
                    ShipInfoPanel2 panel = new ShipInfoPanel2(ShipCountFilter.Ships[i], false);
                    ShipsWrap.Children.Add(panel);
                }
            });
        }
       
    }

    class ShipInfoPanel2: Border
    {
        Canvas mainCanvas;
        GSShip Ship;
        public ShipPanel2 panel2;
        public ShipInfoPanel2(GSShip ship, bool simple)
        {
            Ship = ship;
            Width = 600;
            Height = 200;
            BorderThickness = new Thickness(1);
            BorderBrush = Links.Brushes.SkyBlue;
            Margin = new Thickness(3, 3, 3, 3);
            Viewbox vbx = new Viewbox();
            Child = vbx;
            mainCanvas = new Canvas();
            vbx.Child = mainCanvas;
            mainCanvas.Width = 900;
            mainCanvas.Height = 300;
            ShipB shipb = new ShipB(1, ship.Schema, ship.Health, ship.Pilot, BitConverter.GetBytes(ship.Model), ShipSide.Defense, null, false, null, ShipBMode.Info,255);
            //shipb.PanelB.SetNotBattle();
            Viewbox v = new Viewbox();//d
            v.Width = 500; v.Height = 308; //d
            panel2 = new ShipPanel2(shipb, ship);//d
            if (simple == false) panel2.SetFleet(ship.Fleet, true);//d
            else panel2.UpdateLevel();
            panel2.SetPilot(); //d
            v.Child = panel2; //d
            mainCanvas.Children.Add(v); //d
            Canvas.SetLeft(v, 330); //d 
            //mainCanvas.Children.Add(shipb.PanelB.VBX);
            mainCanvas.Children.Add(shipb.HexShip);
            //shipb.HexShip.PreviewMouseDown += new MouseButtonEventHandler(HexShip_PreviewMouseDown);
            //Canvas.SetLeft(shipb.HexShip, 400);
            Canvas.SetTop(shipb.HexShip, 20);
           
        }


        Border GetRepairPanel()
        {
            Border border = new Border();
            border.BorderBrush = Brushes.White;
            border.BorderThickness = new Thickness(3);
            border.CornerRadius = new CornerRadius(10);
            border.Width = 120;
            border.Height = 150;
            border.Background = Brushes.Black;

            Canvas canvas = new Canvas();
            border.Child = canvas;

            Ship.Schema.CalcPrice();
            ItemPrice RepairPrice;
            if (Ship.Health == 0)
                RepairPrice = Ship.Schema.Price;
            else
                RepairPrice = new ItemPrice(Ship.Schema.Price.Money / 100 * (100 - Ship.Health),
                    Ship.Schema.Price.Metall / 100 * (100 - Ship.Health),
                    Ship.Schema.Price.Chips / 100 * (100 - Ship.Health),
                    Ship.Schema.Price.Anti / 100 * (100 - Ship.Health));
            Common.PutRect(canvas, 30, Links.Brushes.MoneyImageBrush, 5, 5, false);
            Common.PutLabel(canvas, 35, 5, 20, RepairPrice.Money.ToString(), Brushes.White);
            Common.PutRect(canvas, 30, Links.Brushes.MetalImageBrush, 5, 40, false);
            Common.PutLabel(canvas, 35, 40, 20, RepairPrice.Metall.ToString(), Brushes.White);
            Common.PutRect(canvas, 30, Links.Brushes.ChipsImageBrush, 5, 75, false);
            Common.PutLabel(canvas, 35, 75, 20, RepairPrice.Chips.ToString(), Brushes.White);
            Common.PutRect(canvas, 30, Links.Brushes.AntiImageBrush, 5, 110, false);
            Common.PutLabel(canvas, 35, 110, 20, RepairPrice.Anti.ToString(), Brushes.White);
            return border;
        }
        void ShipRepairPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!Ship.Schema.Price.CheckPrice())
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("NoMoney")), true);
                return;
            }
            string repairresult = Events.RepairShip(Ship);
            if (repairresult == "")
            {
                Gets.GetResources();
                Links.Controller.ShipsCanvas.Select();
                return;
            }
            else
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(repairresult), true);
            }
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
            string result = Events.MovePilotFromShip(Ship.ID);
            if (result == "")
            {
                Links.Controller.PopUpCanvas.Remove();
                //Gets.GetTotalInfo();
                Links.Controller.ShipsCanvas.Select();
            }
            else Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(result), true);

        }
        
        void fleetborder_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Ship.Pilot == null)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("NoPilot")), true);
                return;
            }
            Links.Helper.SetShip(Ship);
            Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.FleetForShip);
            //Links.Controller.FleetsCanvas.Select(Ship);
        }
        /*void HexShip_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Ship.Fleet != null) return;
            List<GSButton> List = new List<GSButton>();
            List.Add(new GSButton(200, 50, Links.Interface("DestroyShip"), DestroyShip_Click, null));
            List.Add(GSButton.GetCancelButton());

            Links.Controller.RoundButtons.Create(this, List, false);
            Links.Controller.RoundButtons.Place();
        }*/
        void DestroyShip_Click(object sender, RoutedEventArgs e)
        {
            Links.Controller.RoundButtons.Remove();
            string eventresult = Events.DestroyShip(Ship);
            if (eventresult == "")
            {
                Links.Controller.ShipsCanvas.Select();
                return;
            }
            else
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult), true);
        }

        static GlyphTypeface gt = new GlyphTypeface(new Uri("pack://application:,,,/resources/Agru.ttf", UriKind.Absolute), StyleSimulations.BoldSimulation);
        static Pen RedPen = new Pen(Brushes.Red, 10);
        static RadialGradientBrush redbrush = Common.GetRadialBrush(Colors.Red, 0.8, 0.3);
        static Pen BlackPen = new Pen(Brushes.Black, 10);
        
        public static Rectangle GetRepairIndicator(byte percent, int add)
        {
            Rectangle rect = new Rectangle();
            rect.Width = 75; rect.Height = 75;
            
            DrawingGroup group = new DrawingGroup();

            PathGeometry blackgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M250,0 a250,250 0 1,0 0.1,0"));
            group.Children.Add(new GeometryDrawing(Brushes.Black, RedPen, blackgeom));

            PathGeometry redgeom;
            if (percent == 100)
            {
                redgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M250,0 a250,250 0 1,0 0.1,0"));
            }
            else
            {
                double angle = (360.0 - 3.6 * percent) / 180 * Math.PI;
                string s = String.Format("M250,0 v250 L{0},{1} A250,250 0 {2},1 250,0", Math.Round(250.0 + 250.0 * Math.Sin(angle), 0),
                    Math.Round(250.0 - 250.0 * Math.Cos(angle), 0), (percent > 50 ? 1 : 0));
                redgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(s));
            }
            group.Children.Add(new GeometryDrawing(redbrush, null, redgeom));
            if (add == -1)
            {
                group.Children.Add(new GeometryDrawing(Brushes.SkyBlue, BlackPen, new PathGeometry((PathFigureCollection)
                    Links.conv.ConvertFrom("M200,120 h100 v30 l-100,200 v30 h100 v-30 l-100,-200 z"))));
            }
            else
            {
                if (add > 99) add = 99;
                string textstring = String.Format("+{0}/m", add);
                char[] chars = textstring.ToCharArray();
                double[] baselines = new double[chars.Length];
                for (int i = 0; i < baselines.Length; i++)
                    baselines[i] = 70;
                Point baseLine = new Point(250 - chars.Length * 35, 300);
                ushort[] gtchars = new ushort[chars.Length];
                for (int i = 0; i < gtchars.Length; i++)
                    gtchars[i] = gt.CharacterToGlyphMap[chars[i]];
                GlyphRun theGlyphRun = new GlyphRun(gt, 0, false, 160, gtchars,
                baseLine, baselines, null, null, null, null, null, null);
                GlyphRunDrawing gDrawing = new GlyphRunDrawing(Brushes.White, theGlyphRun);
                group.Children.Add(gDrawing);
            }
            rect.Fill = new DrawingBrush(group);
            return rect;
        }
    }
   
    class ShipCountFilter : Canvas
    {
        public int CurrentPage { get; private set; }
        Border Place;
        public ShipCountFilter()
        {
           Rectangle rect = new Rectangle();
            rect.Width = 256; rect.Height = 60;
            rect.Fill = Links.Brushes.Interface.TextPanel;
            Children.Add(rect);
            Canvas.SetLeft(rect, 48);
            Width = 360; Height = 60;
            Background = Brushes.Black;

            Rectangle LeftArrow = Common.GetRectangle(50, Links.Brushes.Interface.LeftArrow);
            Children.Add(LeftArrow);

            LeftArrow.PreviewMouseDown += new MouseButtonEventHandler(LeftArrow_PreviewMouseDown);


            Rectangle RightArrow = Common.GetRectangle(50, Links.Brushes.Interface.RightArrow);
            Children.Add(RightArrow);
            Canvas.SetLeft(RightArrow, 302);
            RightArrow.PreviewMouseDown += new MouseButtonEventHandler(RightArrow_PreviewMouseDown);

            Place = new Border();
            Place.Width = 160;
            Place.Height = 45;
            Children.Add(Place);
            Canvas.SetLeft(Place, 100);
            Canvas.SetTop(Place, 7.5);

            CurrentPage = 1;
            Ships = new List<GSShip>();
            Draw();
        }
        void Draw()
        {
            int pages = Ships.Count / 4;
            if (pages * 4 < Ships.Count) pages++;
            if (CurrentPage > pages) CurrentPage = 1;
            else if (CurrentPage < 1) CurrentPage = pages;
            Place.Child = Common.GetBlock(30, String.Format("{0}/{1}",CurrentPage,pages), Brushes.White, new Thickness());
        }
        public List<GSShip> Ships { get; private set; }
        public void SetShips(List<GSShip> ships)
        {
            Ships = ships;
            CurrentPage = 1;
            Draw();
        }
        void RightArrow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            CurrentPage++;
            Links.Controller.ShipsCanvas.Refresh(true);
            Draw();
        }
        void LeftArrow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            CurrentPage--;
            Links.Controller.ShipsCanvas.Refresh(true);
            Draw();
        }
    }
    class ShipFleetFilter : Canvas
    {
        //-1 - все корабли
        //-2 - без пилотов
        //-3 - без флотов
        //-4 - не отремонтированные
        public long CurrentFleetID { get; private set; }
        Border Place;
        static TextBlock AllFleetsBlock = GetAllFleetsTextBlock();
        static TextBlock NoPilotBlock = GetNoPilotTextBlock();
        static TextBlock NoFleetBlock = GetNoFleetTextBlock();
        static TextBlock NeedRepaidBlock = GetNeedRepairTextBlock();
        public ShipFleetFilter()
        {
            Rectangle rect = new Rectangle();
            rect.Width = 256; rect.Height = 60;
            rect.Fill = Links.Brushes.Interface.TextPanel;
            Children.Add(rect);
            Canvas.SetLeft(rect, 48);
            Width = 360; Height = 60;
            Background = Brushes.Black;

            Rectangle LeftArrow = Common.GetRectangle(50, Links.Brushes.Interface.LeftArrow);
            Children.Add(LeftArrow);

            LeftArrow.PreviewMouseDown += new MouseButtonEventHandler(LeftArrow_PreviewMouseDown);


            Rectangle RightArrow = Common.GetRectangle(50, Links.Brushes.Interface.RightArrow);
            Children.Add(RightArrow);
            Canvas.SetLeft(RightArrow, 302);
            RightArrow.PreviewMouseDown += new MouseButtonEventHandler(RightArrow_PreviewMouseDown);
            
            Place = new Border();
            Place.Width = 160;
            Place.Height = 45;
            Children.Add(Place);
            Canvas.SetLeft(Place, 100);
            Canvas.SetTop(Place, 7.5);

            CurrentFleetID = -1;
            Draw();
        }

        void RightArrow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            switch (CurrentFleetID)
            {
                case -4: CurrentFleetID = -3; break;
                case -3: CurrentFleetID = -2; break;
                case -2: CurrentFleetID = -1; break;
                case -1:
                    if (GSGameInfo.Fleets.Count == 0)
                    {
                        CurrentFleetID = -4;
                    }
                    else
                    {
                        CurrentFleetID = GSGameInfo.Fleets.Keys[0];
                    }
                    break;
                default:
                    if (!GSGameInfo.Fleets.ContainsKey(CurrentFleetID))
                    {
                        CurrentFleetID = -1;
                    }
                    else
                    {
                        int pos = GSGameInfo.Fleets.IndexOfKey(CurrentFleetID);
                        pos++;
                        if (pos == GSGameInfo.Fleets.Count)
                            CurrentFleetID = -4;
                        else
                            CurrentFleetID = GSGameInfo.Fleets.Keys[pos];
                    } break;
            }
            Draw();
            Links.Controller.ShipsCanvas.Refresh(false);
        }

        void LeftArrow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            switch (CurrentFleetID)
            {
                case -4: 
                    if (GSGameInfo.Fleets.Count == 0)
                    {
                        CurrentFleetID = -1;
                    }
                    else
                    {
                        CurrentFleetID = GSGameInfo.Fleets.Keys[GSGameInfo.Fleets.Count - 1];
                    }
                    break;
                case -3: CurrentFleetID = -4; break;
                case -2: CurrentFleetID = -3; break;
                case -1: CurrentFleetID = -2; break;
                    
                default:
                    if (!GSGameInfo.Fleets.ContainsKey(CurrentFleetID))
                    {
                        CurrentFleetID = -1;
                    }
                    else
                    {
                        int pos = GSGameInfo.Fleets.IndexOfKey(CurrentFleetID);
                        pos--;
                        if (pos == -1)
                            CurrentFleetID = -1;
                        else
                            CurrentFleetID = GSGameInfo.Fleets.Keys[pos];
                    } break;
            }
            Draw();
            Links.Controller.ShipsCanvas.Refresh(false);
        }
        /*
        public void Refresh()
        {
            if (!GSGameInfo.Fleets.ContainsKey(CurrentFleetID))
                CurrentFleetID = -1;
            Draw();
        }
         */
        public void Draw()
        {
            switch (CurrentFleetID)
            {
                case -4: Place.Child = NeedRepaidBlock; break;
                case -3: Place.Child = NoFleetBlock; break;
                case -2: Place.Child = NoPilotBlock; break;
                case -1: Place.Child = AllFleetsBlock; break;
                default:
                    if (GSGameInfo.Fleets.Count==0 || !GSGameInfo.Fleets.ContainsKey(CurrentFleetID))
                    {
                        CurrentFleetID=-1; Place.Child=AllFleetsBlock;
                    }
                    else
                    {
                        FleetEmblem emblem = new FleetEmblem(BitConverter.ToInt32(GSGameInfo.Fleets[CurrentFleetID].Image, 0), 45);
                        Place.Child = emblem;
                    } break;
            }
            
        }
        static TextBlock GetAllFleetsTextBlock()
        {
            TextBlock block = Common.GetBlock(20, Links.Interface("AllFleets"));
            block.Foreground = Brushes.White;
            return block;
        }
        static TextBlock GetNoPilotTextBlock()
        {
            TextBlock block = Common.GetBlock(20, Links.Interface("NoPilot"));
            block.Foreground = Brushes.White;
            return block;
        }
        static TextBlock GetNoFleetTextBlock()
        {
            TextBlock block = Common.GetBlock(20, Links.Interface("NoFleet"));
            block.Foreground = Brushes.White;
            return block;
        }
        static TextBlock GetNeedRepairTextBlock()
        {
            TextBlock block = Common.GetBlock(20, Links.Interface("NeedRepair"));
            block.Foreground = Brushes.White;
            return block;
        }
    }
}
