/*using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    class StatsGrid
    {
        public static Grid Grid;
        public static void Create()
        {
            Grid = new Grid();
            Grid.RowDefinitions.Add(new RowDefinition());
            Grid.RowDefinitions.Add(new RowDefinition());
        }
        public static void StartBattle(Battle battle)
        {
            Grid.Children.Clear();
            ScrollViewer TopViewer = new ScrollViewer();
            TopViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            TopViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            Grid.Children.Add(TopViewer);
            WrapPanel TopPanel = new WrapPanel();
            TopViewer.Content = TopPanel;
            foreach (ShipB ship in battle.Side1.Ships.Values)
            {
                //TopPanel.Children.Add(ship.PanelB);
            }
            ScrollViewer LowViewer = new ScrollViewer();
            LowViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            LowViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            Grid.Children.Add(LowViewer);
            Grid.SetRow(LowViewer, 1);
            WrapPanel LowPanel = new WrapPanel();
            LowViewer.Content = LowPanel;
            foreach (ShipB ship in battle.Side2.Ships.Values)
            {
                //LowPanel.Children.Add(ship.PanelB);
            }
        }

    }
}
 */
