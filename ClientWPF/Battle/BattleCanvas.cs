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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    //enum ShipCommands {None, Enter, Move, Leave, Gun1, Gun2, Gun3, AllGun, TargetGun, Info }
    
    class TurnGrid:Grid
    {
        Label roundlbl;
        Rectangle Left, Right;
        public TurnGrid()
        {
            Width = 220; Height = 80;
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions[0].Width = new GridLength(60);
            ColumnDefinitions[1].Width = new GridLength(100);
            roundlbl = new Label();
            Children.Add(roundlbl);
            Grid.SetColumn(roundlbl, 1);
            roundlbl.FontSize = 30;
            roundlbl.VerticalAlignment = VerticalAlignment.Center;
            //roundlbl.Margin = new Thickness(0, -6, 0, 0);
            roundlbl.HorizontalAlignment = HorizontalAlignment.Center;
            roundlbl.FontFamily = Links.Font; roundlbl.Foreground = Brushes.Red;
            roundlbl.FontWeight = FontWeights.Bold;
            Left = Common.GetRectangle(50, Gets.GetIntBoyaImage("2/Left"));
            Right = Common.GetRectangle(50, Gets.GetIntBoyaImage("2/Right"));
            Children.Add(Left); Children.Add(Right);
            Grid.SetColumn(Right, 2);
            Left.Opacity = 0; Right.Opacity = 0;
           }
        public void SetParams(int round, int side)
        {
            roundlbl.Content = "Раунд " + round.ToString();
            roundlbl.Foreground = Brushes.Orange;
            if (side == 0)
            { Left.Opacity = 1; Right.Opacity = 0; }
            //roundlbl.Foreground = Brushes.Red;
            else
            { Left.Opacity = 0; Right.Opacity = 1; }
            //roundlbl.Foreground = Brushes.Green;
        }
    }
}
