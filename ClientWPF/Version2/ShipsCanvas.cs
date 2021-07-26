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
    class ShipsCanvas:Canvas
    {
        WrapPanel ShipsWrap;
        ShipFleetFilter ShipFleetFilter;
        ShipCountFilter ShipCountFilter;
        public Viewbox box;
        List<ShipPanel2> PanelList = new List<ShipPanel2>();
        public ShipsCanvas()
        {
            box = new Viewbox();
            box.Width = 1920;
            box.Height = 930;
            box.HorizontalAlignment = HorizontalAlignment.Center;
            box.VerticalAlignment = VerticalAlignment.Center;
            box.Child = this;
            put();
            Width = 1280; Height = 600;
             }
        //ShipPanel2 CurPanel;
        
        void put()
        {
            Children.Clear();
            //ShowGridLines = true;
            
            ShipFleetFilter = new Client.ShipFleetFilter();
            Children.Add(ShipFleetFilter);
            Canvas.SetLeft(ShipFleetFilter, 190); Canvas.SetTop(ShipFleetFilter, 50);

            ShipCountFilter = new ShipCountFilter();
            Children.Add(ShipCountFilter);
            Canvas.SetLeft(ShipCountFilter, 790); Canvas.SetTop(ShipCountFilter, 50);
            //ColumnDefinitions.Add(new ColumnDefinition()); ColumnDefinitions[0].Width = new GridLength(1150);
            //ColumnDefinitions.Add(new ColumnDefinition());

            Border ShipsBorder = new Border();
            ShipsBorder.Width = 1270;
            ShipsBorder.Height = 490;
            ShipsBorder.BorderBrush = Brushes.Black;
            ShipsBorder.BorderThickness = new Thickness(2);
            ShipsBorder.Margin = new Thickness(2);
            Children.Add(ShipsBorder);
            Canvas.SetTop(ShipsBorder, 110);
            Canvas.SetLeft(ShipsBorder, 5);
            
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
        public void Select()
        {
            Gets.GetTotalInfo("После открытия панели кораблей");
            //ShipFleetFilter.Refresh();
            //Gets.GetShips();
            Refresh(false);
        }
        public void Refresh(bool onlypages)
        {
            ShipsWrap.Children.Clear();
            long CurrentFilterID = ShipFleetFilter.CurrentFleetID;
            this.Dispatcher.BeginInvoke((System.Threading.ThreadStart)delegate ()
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
                int minpos = (page - 1) * 4;
                int maxpos = page * 4;
                PanelList.Clear();
                for (int i = minpos; i < maxpos; i++)
                {
                    if (i >= ShipCountFilter.Ships.Count) break;
                    if (i < 0) break;
                    ShipInfoPanel2 panel = new ShipInfoPanel2(ShipCountFilter.Ships[i], false);
                    ShipsWrap.Children.Add(panel);
                    PanelList.Add(panel.panel2);
                }
            });
        }

    }
}
