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
    class ShipBrushes
    {
        public static RadialGradientBrush RedPanel = GetBrush(0.8, 0.2, Colors.Red);
        public static RadialGradientBrush BluePanel = GetBrush(0.8, 0.2, Colors.Blue);
        public static RadialGradientBrush GreenPanel = GetBrush(0.8, 0.2, Colors.Green);
        public static RadialGradientBrush GoldPanel = GetBrush(0.8, 0.2, Colors.Gold);
        public static RadialGradientBrush SilverPanel = GetBrush(0.8, 0.2, Colors.Silver);
        public static RadialGradientBrush GrayPanel = GetBrush(0.8, 0.2, Colors.Gray);
        public static RadialGradientBrush BlackPanel = GetBrush(0.8, 0.2, Colors.Black);
        public static RadialGradientBrush OrangePanel = GetBrush(0.8, 0.2, Colors.Orange);
        public static RadialGradientBrush VioletPanel = GetBrush(0.8, 0.2, Colors.Violet);
        public static RadialGradientBrush BrownBrush = GetBrush(0.8, 0.2, Colors.Brown);
        public static RadialGradientBrush DarkBluePanel = GetBrush(0.8, 0.2, Colors.DarkBlue);
        public static RadialGradientBrush DarkGoldPanel = GetBrush(0.8, 0.2, Colors.DarkGoldenrod);
        public static RadialGradientBrush IndigoPanel = GetBrush(0.8, 0.2, Colors.Indigo);
        public static RadialGradientBrush LightGrayPanel = GetBrush(0.8, 0.2, Colors.LightSlateGray);
        public static RadialGradientBrush OlivePanel = GetBrush(0.8, 0.2, Colors.Olive);

        public static RadialGradientBrush Glass1 = GetGlassBrush(Colors.Turquoise);
        public static RadialGradientBrush Glass2 = GetGlassBrush(Colors.Teal);
        public static RadialGradientBrush Glass3 = GetGlassBrush(Colors.SteelBlue);
        public static RadialGradientBrush Glass4 = GetGlassBrush(Colors.CadetBlue);
        public static RadialGradientBrush Glass5 = GetGlassBrush(Colors.CornflowerBlue);
        public static RadialGradientBrush Glass6 = GetGlassBrush(Colors.Crimson);
        public static RadialGradientBrush Glass7 = GetGlassBrush(Colors.Cyan);
        public static RadialGradientBrush Glass8 = GetGlassBrush(Colors.DarkCyan);        
        public static RadialGradientBrush Glass9 = GetGlassBrush(Colors.DarkSlateBlue);
        public static RadialGradientBrush Glass10 = GetGlassBrush(Colors.DeepSkyBlue);
        public static RadialGradientBrush Glass11 = GetGlassBrush(Colors.RoyalBlue);
        public static RadialGradientBrush Glass12 = GetGlassBrush(Colors.LightBlue);
        public static RadialGradientBrush Glass13 = GetGlassBrush(Colors.LightSteelBlue);
        public static RadialGradientBrush Glass14 = GetGlassBrush(Colors.DodgerBlue);
        public static RadialGradientBrush Glass15 = GetGlassBrush(Colors.SeaGreen);
        public static RadialGradientBrush Glass16 = GetGlassBrush(Colors.Black);

        
        public static RadialGradientBrush GetGlassBrush(Color color)
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.2);
            brush.GradientStops.Add(new GradientStop(color, 0.7));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            return brush;
        }
        public static RadialGradientBrush GetBrush(double x, double y, Color color)
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(x, y);
            brush.GradientStops.Add(new GradientStop(color, 1));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            return brush;
        }
    }
}
