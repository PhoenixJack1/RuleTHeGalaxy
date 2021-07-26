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
using System.Windows.Media.Animation;

namespace Client
{

    class PortalPanel : ShipPanel
    {
        public PortalPanel(ShipB ship)
        {
            VBX = new Viewbox();
            VBX.Width = 228 * 2;
            VBX.Height = 162 * 2;
            VBX.Child = this;

            Ship = ship;
            BorderBrush = Brushes.Black;
            BorderThickness = new Thickness(0.1);
            Margin = new Thickness(2);
            Width = 228; Height = 162;
            Canvas Main = new Canvas();
            Child = Main;
            Ellipse el = new Ellipse();
            el.Width = 160; el.Height = 160;
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Black, 1));
            brush.GradientStops.Add(new GradientStop(Colors.Purple, 0.7));
            brush.GradientStops.Add(new GradientStop(Colors.Violet, 0.6));
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.5));
            el.Fill = brush;
            Main.Children.Add(el);
            Health = new HealthCanvas();
            Main.Children.Add(Health);
            Canvas.SetLeft(Health, 170); Canvas.SetTop(Health, 60);
        }
    }
    class ShipPanel:Border
    {
        public ShipB Ship;
        public HealthCanvas Health;
        public ShieldCanvas Shield;
        public EnergyCanvas Energy;
        public EvasionCanvas Evasion;
        public AbsorpCanvas Absorp;
        public Location Location;
        public WeaponCanvas Weapon1;
        public WeaponCanvas Weapon2;
        public WeaponCanvas Weapon3;
        public Viewbox VBX;
        UIElement[] ImmuneElements = new UIElement[4];
        Canvas Main;
        public ShipPanel()
        {

        }
        public static ShipPanel GetCargoShipPanel(ShipB ship)
        {
            ShipPanel panel = new ShipPanel();
            panel.VBX = new Viewbox();
            panel.VBX.Width = 228 * 1.9;
            panel.VBX.Height = 162 * 1.9;
            StackPanel toppanel = new StackPanel();
            panel.VBX.Child = toppanel;
            toppanel.Children.Add(Common.GetBlock(20, "Cargo Ship", Brushes.Black, new Thickness()));
            panel.VBX.Child = toppanel;
            panel.Ship = ship;
            toppanel.Children.Add(panel);
            panel.BorderBrush = Brushes.Black;
            panel.BorderThickness = new Thickness(0.1);
            panel.Margin = new Thickness(2);
            panel.Width = 228; panel.Height = 162;
            panel.Main = new Canvas();
            panel.Child = panel.Main;
            Path ShipBorder = panel.GetShipBorder(3);
            panel.Main.Children.Add(ShipBorder);
            panel.Health = new HealthCanvas();
            panel.Main.Children.Add(panel.Health);
            Canvas.SetLeft(panel.Health, 130); Canvas.SetTop(panel.Health, 37);
            panel.Shield = new ShieldCanvas();
            panel.Main.Children.Add(panel.Shield);
            Canvas.SetLeft(panel.Shield, 48); Canvas.SetTop(panel.Shield, 37);
            panel.Energy = new EnergyCanvas();
            panel.Main.Children.Add(panel.Energy);
            Canvas.SetLeft(panel.Energy, 96); Canvas.SetTop(panel.Energy, 65);
            panel.Evasion = new EvasionCanvas();
            panel.Main.Children.Add(panel.Evasion);
            Canvas.SetLeft(panel.Evasion, 3); Canvas.SetTop(panel.Evasion, 56);
            panel.Absorp = new AbsorpCanvas();
            panel.Main.Children.Add(panel.Absorp);
            Canvas.SetLeft(panel.Absorp, 175);
            Canvas.SetTop(panel.Absorp, 56);
            panel.Location = new Location(ship.Params.HexMoveCost.GetCurrent, ship.Params.JumpDistance);
            //if (ship.Params.TimeToEnter == 99)
            //    ship.CurHex = 254;
            //else ship.CurHex = (byte)(200 + ship.Params.TimeToEnter);
            panel.Main.Children.Add(panel.Location);
            Canvas.SetLeft(panel.Location, 85);
            Canvas.SetTop(panel.Location, 115);

           
            return panel;
        }
        public static ShipPanel GetBigShipPanel(ShipB ship)
        {
            ShipPanel panel = new ShipPanel();
            panel.VBX = new Viewbox();
            panel.VBX.Width = 228 * 1.9;
            panel.VBX.Height = 162 * 1.9;
            StackPanel toppanel = new StackPanel();
            panel.VBX.Child = toppanel;
                toppanel.Children.Add(Common.GetBlock(20, "Big Ship", Brushes.Black, new Thickness()));
            panel.VBX.Child = toppanel;
            panel.Ship = ship;
            toppanel.Children.Add(panel);
            panel.BorderBrush = Brushes.Black;
            panel.BorderThickness = new Thickness(0.1);
            panel.Margin = new Thickness(2);
            panel.Width = 228; panel.Height = 162;
            panel.Main = new Canvas();
            panel.Child = panel.Main;
            Path ShipBorder = panel.GetShipBorder(3);
            panel.Main.Children.Add(ShipBorder);
            panel.Health = new HealthCanvas();
            panel.Main.Children.Add(panel.Health);
            Canvas.SetLeft(panel.Health, 130); Canvas.SetTop(panel.Health, 37);
            panel.Shield = new ShieldCanvas();
            panel.Main.Children.Add(panel.Shield);
            Canvas.SetLeft(panel.Shield, 48); Canvas.SetTop(panel.Shield, 37);
            panel.Energy = new EnergyCanvas();
            panel.Main.Children.Add(panel.Energy);
            Canvas.SetLeft(panel.Energy, 96); Canvas.SetTop(panel.Energy, 65);
            panel.Evasion = new EvasionCanvas();
            panel.Main.Children.Add(panel.Evasion);
            Canvas.SetLeft(panel.Evasion, 3); Canvas.SetTop(panel.Evasion, 56);
            panel.Absorp = new AbsorpCanvas();
            panel.Main.Children.Add(panel.Absorp);
            Canvas.SetLeft(panel.Absorp, 175);
            Canvas.SetTop(panel.Absorp, 56);
            panel.Location = new Location(ship.Params.HexMoveCost.GetCurrent, ship.Params.JumpDistance);
            //if (ship.Params.TimeToEnter == 99)
            //    ship.CurHex = 254;
            //else ship.CurHex = (byte)(200 + ship.Params.TimeToEnter);
            panel.Main.Children.Add(panel.Location);
            Canvas.SetLeft(panel.Location, 85);
            Canvas.SetTop(panel.Location, 115);

            panel.Weapon1 = WeaponCanvas.SetWeaponPosition(0, panel.Main, panel.Ship.Weapons[0]);
            panel.Weapon2 = WeaponCanvas.SetWeaponPosition(1, panel.Main, panel.Ship.Weapons[1]);
            panel.Weapon3 = WeaponCanvas.SetWeaponPosition(2, panel.Main, panel.Ship.Weapons[2]);
            for (int i = 0; i < ship.GroupEffects.Count; i++)
            {
                SecondaryEffect effect = ship.GroupEffects[i];
                UIElement element = Common.GetRectangle(20, effect.Brush, Links.Interface(effect.Tip));
                //UIElement element = Pictogram.GetPict(effect.Pict, effect.Target, true, Links.Interface(effect.Tip), 20);
                panel.Main.Children.Add(element);
                Canvas.SetLeft(element, 145);
                Canvas.SetTop(element, 100 + i * 20);
                TextBlock block = Common.GetBlock(14, effect.Value.ToString());
                panel.Main.Children.Add(block);
                Canvas.SetLeft(block, 175);
                Canvas.SetTop(block, 105 + i * 20);
            }
            return panel;
        }
        public ShipPanel(ShipB ship, Brush TitleBrush)
        {
            VBX = new Viewbox();
            VBX.Width = 228 * 1.9;
            VBX.Height = 162 * 1.9;
            StackPanel panel = new StackPanel();
            VBX.Child = panel;
            if (ship.States.BigSize == false)
                panel.Children.Add(Common.GetBlock(20,
                    ship.Schema.ShipType.GetName() + " " + SelectSchemaNameCanvas.GetNameResult(ship.Schema.CryptName), TitleBrush, new Thickness()));
            else
                panel.Children.Add(Common.GetBlock(20, "Big Ship", TitleBrush, new Thickness()));
            VBX.Child = panel;
            Ship = ship;
            panel.Children.Add(this);
            BorderBrush = Brushes.Black;
            BorderThickness = new Thickness(0.1);
            Margin = new Thickness(2);
            Width = 228; Height = 162;
            Main = new Canvas();
            Child = Main;
            Path ShipBorder = GetShipBorder(ship.Schema.ShipType.WeaponCapacity);
            Main.Children.Add(ShipBorder);
            Health = new HealthCanvas();
            Main.Children.Add(Health);
            Canvas.SetLeft(Health, 130); Canvas.SetTop(Health, 37);
            Shield = new ShieldCanvas();
            Main.Children.Add(Shield);
            Canvas.SetLeft(Shield, 48); Canvas.SetTop(Shield, 37);
            Energy = new EnergyCanvas();
            Main.Children.Add(Energy);
            Canvas.SetLeft(Energy, 96); Canvas.SetTop(Energy, 65);
            Evasion = new EvasionCanvas();
            Main.Children.Add(Evasion);
            Canvas.SetLeft(Evasion, 3); Canvas.SetTop(Evasion, 56);
            Absorp = new AbsorpCanvas();
            Main.Children.Add(Absorp);
            Canvas.SetLeft(Absorp, 175);
            Canvas.SetTop(Absorp, 56);
            Location = new Location(ship.Params.HexMoveCost.GetCurrent, ship.Params.JumpDistance);
            if (ship.Params.TimeToEnter == 99)
                ship.SetHex(254);
            else ship.SetHex((byte)(200 + ship.Params.TimeToEnter));
            Main.Children.Add(Location);
            Canvas.SetLeft(Location, 85);
            Canvas.SetTop(Location, 115);
            
            if (Ship.Schema.GetWeapon(0) != null)
                Weapon1 = WeaponCanvas.SetWeaponPosition(0, Main, Ship.Schema.GetWeapon(0));
            if (Ship.Schema.GetWeapon(1) != null)
                Weapon2 = WeaponCanvas.SetWeaponPosition(1, Main, Ship.Schema.GetWeapon(1));
            if (Ship.Schema.GetWeapon(2) != null)
                Weapon3 = WeaponCanvas.SetWeaponPosition(2, Main, Ship.Schema.GetWeapon(2));
            for (int i = 0; i < ship.GroupEffects.Count; i++)
            {
                SecondaryEffect effect = ship.GroupEffects[i];
                UIElement element = Common.GetRectangle(20, effect.Brush, Links.Interface(effect.Tip));
                //UIElement element = Pictogram.GetPict(effect.Pict, effect.Target, true, Links.Interface(effect.Tip), 20);
                Main.Children.Add(element);
                Canvas.SetLeft(element, 145);
                Canvas.SetTop(element, 100 + i * 20);
                TextBlock block = Common.GetBlock(14, effect.Value.ToString());
                Main.Children.Add(block);
                Canvas.SetLeft(block,175);
                Canvas.SetTop(block, 105 + i * 20);
            }
            if (ship.Params.BasicParams.Cargo > 0)
            {
                UIElement element = Common.GetRectangle(18,Pictogram.CargoBrush, Links.Interface("Cargo"));
                Main.Children.Add(element);
                Canvas.SetLeft(element, 35);
                Canvas.SetTop(element, 100);
                TextBlock block = Common.GetBlock(14, (ship.Params.BasicParams.Cargo / 10.0).ToString() + "%");
                Main.Children.Add(block);
                Canvas.SetLeft(block, 65);
                Canvas.SetTop(block, 105);
            }
            if (ship.Params.BasicParams.Colony > 0)
            {
                UIElement element = Common.GetRectangle(18, Pictogram.ColonyBrush, Links.Interface("Colony"));
                Main.Children.Add(element);
                Canvas.SetLeft(element, 35);
                Canvas.SetTop(element, 120);
                TextBlock block = Common.GetBlock(14, ship.Params.BasicParams.Colony.ToString());
                Main.Children.Add(block);
                Canvas.SetLeft(block, 65);
                Canvas.SetTop(block, 125);
            }
        }
        Path GetShipBorder(int weapons)
        {
            Path result = new Path();
            result.Stroke = Brushes.Black; result.StrokeThickness = 2;
            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure();
            figure.StartPoint = new Point(0, 30);
            if (weapons == 2 || weapons == 3)
            {
                figure.Segments.Add(new ArcSegment(new Point(15, 30), new Size(30, 30), 0, false, SweepDirection.Clockwise, true));
                figure.Segments.Add(new LineSegment(new Point(15, 0), true));
                figure.Segments.Add(new LineSegment(new Point(45, 0), true));
                figure.Segments.Add(new LineSegment(new Point(45, 30), true));
                figure.Segments.Add(new ArcSegment(new Point(60, 30), new Size(30, 30), 0, false, SweepDirection.Clockwise, true));
            }
            else
                figure.Segments.Add(new ArcSegment(new Point(60, 30), new Size(30, 10), 0, false, SweepDirection.Clockwise, true));
            figure.Segments.Add(new ArcSegment(new Point(84, 30), new Size(12, 5), 0, false, SweepDirection.Counterclockwise, true));
            if (weapons == 1 || weapons == 3)
            {
                figure.Segments.Add(new ArcSegment(new Point(99, 30), new Size(30, 30), 0, false, SweepDirection.Clockwise, true));
                figure.Segments.Add(new LineSegment(new Point(99, 0), true));
                figure.Segments.Add(new LineSegment(new Point(129, 0), true));
                figure.Segments.Add(new LineSegment(new Point(129, 30), true));
                figure.Segments.Add(new ArcSegment(new Point(144, 30), new Size(30, 30), 0, false, SweepDirection.Clockwise, true));
            }
            else
                figure.Segments.Add(new ArcSegment(new Point(144, 30), new Size(30, 10), 0, false, SweepDirection.Clockwise, true));
            figure.Segments.Add(new ArcSegment(new Point(168, 30), new Size(12, 5), 0, false, SweepDirection.Counterclockwise, true));
            if (weapons == 2 || weapons == 3)
            {
                figure.Segments.Add(new ArcSegment(new Point(183, 30), new Size(30, 30), 0, false, SweepDirection.Clockwise, true));
                figure.Segments.Add(new LineSegment(new Point(183, 0), true));
                figure.Segments.Add(new LineSegment(new Point(213, 0), true));
                figure.Segments.Add(new LineSegment(new Point(213, 30), true));
                figure.Segments.Add(new ArcSegment(new Point(228, 30), new Size(30, 30), 0, false, SweepDirection.Clockwise, true));
            }
            else
                figure.Segments.Add(new ArcSegment(new Point(228, 30), new Size(30, 10), 0, false, SweepDirection.Clockwise, true));
            figure.Segments.Add(new BezierSegment(new Point(228, 140), new Point(224, 140), new Point(114, 160), true));
            figure.Segments.Add(new BezierSegment(new Point(0, 140), new Point(0, 140), new Point(0, 30), true));
            geometry.Figures.Add(figure);
            result.Data = geometry;
            //result.Fill = Brushes.LightGray;
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Gray, 1));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Gray, 0));
            result.Fill = brush;
            return result;
        }
        static Brush[] ImmuneTargets = new Brush[] { Pictogram.ImmuneEnergy, Pictogram.ImmunePhysic, Pictogram.ImmuneIrregular, Pictogram.ImmuneCyber };
        static string[] ImmuneTips = new string[] { "EnImm", "PhImm", "IrImm", "CyImm" };
        public void ImmuneUpdate()
        {
            for (int i = 0; i < 4; i++)
            {
                byte b = (byte)Ship.Params.Immune.GetCurValue((WeaponGroup)i);
                if (b == 0)
                {
                    if (ImmuneElements[i] != null) Main.Children.Remove(ImmuneElements[i]);
                    ImmuneElements[i] = null;
                }
                else
                {
                    if (ImmuneElements[i] != null) continue;
                    UIElement element = Common.GetRectangle(15, ImmuneTargets[i], Links.Interface(ImmuneTips[i]));
                    Main.Children.Add(element);
                    Canvas.SetTop(element, 80);
                    Canvas.SetLeft(element, 45 + i * 15 + (i > 1 ? 60 : 0));
                    ImmuneElements[i] = element;
                }
            }
        }
        public void SetNotBattle()
        {
            if (Ship.StartHealth != 100)
            {
                Health.Capacity.Content = Ship.Params.BasicParams.BasicHealth.ToString();
                Health.Value.Foreground = Brushes.Red;
                Health.Value.FontWeight = FontWeights.Bold;
            }
        }
        public void WeaponUpdate()
        {
            if (Weapon1 != null) Weapon1.Update(Ship, 0);
            if (Weapon2 != null) Weapon2.Update(Ship, 1);
            if (Weapon3 != null) Weapon3.Update(Ship, 2);
        }
        public void Arm()
        {
            if (Weapon1 != null) Weapon1.Arm();
            if (Weapon2 != null) Weapon2.Arm();
            if (Weapon3 != null) Weapon3.Arm();
        }
    }
    class GridLabel
    {
        public Grid grid;
        public Label label;
    }
    class ShipPanelBrushes
    {
        public static byte oppacity = 90;
        public static RadialGradientBrush HealthBrush = GetHealthBrush();
        public static RadialGradientBrush ShieldBrush = GetShieldBrush();
        public static RadialGradientBrush EnergyBrush = GetEnergyBrush();
        public static RadialGradientBrush EnergyWBrush = GetEnergyWBrush();
        public static RadialGradientBrush PhysicBrush = GetPhysicBrush();
        public static RadialGradientBrush IrregularBrush = GetIrregularBrush();
        public static RadialGradientBrush CyberBrush = GetCyberBrush();
        public static RadialGradientBrush OnBaseBrush = GetOnBaseBrush();
        public static RadialGradientBrush PreparinBrush = GetPreparingBrush();
        public static RadialGradientBrush ReadyBrush = GetReadyBrush();
        public static RadialGradientBrush OnFieldBrush = GetOnFieldBrush();
        public static RadialGradientBrush WrappedBrush = GetWrappedBrush();
        public static RadialGradientBrush DestroyedBrush = GetDestroyedBrush();
        static RadialGradientBrush GetDestroyedBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.2);
            brush.GradientStops.Add(new GradientStop(Colors.Black, 1));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            return brush;
        }
        static RadialGradientBrush GetWrappedBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.4));
            brush.GradientStops.Add(new GradientStop(Colors.Violet, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Purple,0.7));
            brush.GradientStops.Add(new GradientStop(Colors.Black,1));
            return brush;
        }
        static RadialGradientBrush GetOnFieldBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.2);
            brush.GradientStops.Add(new GradientStop(Colors.Green, 1));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            return brush;
        }
        static RadialGradientBrush GetReadyBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.2);
            brush.GradientStops.Add(new GradientStop(Colors.Yellow, 1));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            ColorAnimation anim = new ColorAnimation(Colors.Yellow, Colors.Green, TimeSpan.FromSeconds(0.5));
            anim.AutoReverse = true;
            anim.RepeatBehavior = RepeatBehavior.Forever;
            brush.GradientStops[0].BeginAnimation(GradientStop.ColorProperty, anim);
            return brush;
        }
        static RadialGradientBrush GetPreparingBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.2);
            brush.GradientStops.Add(new GradientStop(Colors.Yellow, 1));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            return brush;
        }
        static RadialGradientBrush GetOnBaseBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.2);
            brush.GradientStops.Add(new GradientStop(Colors.Red, 1));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            return brush;
        }
        static RadialGradientBrush GetEnergyBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.2);
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(oppacity, 160, 255, 0), 1));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(oppacity, 255, 255, 255), 0));
            return brush;
        }
        static RadialGradientBrush GetCyberBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.2);
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(oppacity, 0, 255, 0), 1));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(oppacity, 255, 255, 255), 0));
            return brush;
        }
        static RadialGradientBrush GetIrregularBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.2);
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(oppacity, 160, 0, 160), 1));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(oppacity, 255, 255, 255), 0));
            return brush;
        }
        static RadialGradientBrush GetPhysicBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.2);
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(oppacity, 255, 0, 0), 1));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(oppacity, 255, 255, 255), 0));
            return brush;
        }
        static RadialGradientBrush GetEnergyWBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.2);
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(oppacity, 0, 0, 255), 1));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(oppacity, 255, 255, 255), 0));
            return brush;
        }
        static RadialGradientBrush GetShieldBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.2);
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(oppacity, 0, 0, 255), 1));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(oppacity, 255, 255, 255), 0));
            return brush;
        }
        static RadialGradientBrush GetHealthBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.2);
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(oppacity, 255, 0, 0), 1));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(oppacity, 255, 255, 255), 0));
            return brush;
        }
        public static GridLabel GetGridWithValue(double width, double top, double left, double fontsize, double angle)
        {
            GridLabel result = new GridLabel();
            result.grid = new Grid();
            result.grid.Width = width;
            Canvas.SetLeft(result.grid, left);
            Canvas.SetTop(result.grid, top);
            result.label = new Label();
            result.grid.Children.Add(result.label);
            result.label.HorizontalAlignment = HorizontalAlignment.Center;
            result.label.FontFamily = Links.Font;
            result.label.FontSize = fontsize;
            result.label.FontWeight = FontWeights.Bold;
            return result;
        }
    }
    class Location : Canvas
    {
        Path Turns;
        Label Moves;
        Label Energy;
        Path Back;
        static Brush BaseBrush = GetBrush(Colors.Red);
        static Brush DestroyBrush = GetBrush(Colors.Black);
        static Brush ArrowBrush = GetBrush(Colors.Purple);
        static Brush TurnBrush = GetBrush(Colors.Green);
        public Location(int energy, byte jump)
        {
            Width = 60; Height = 40;
            Path border = GetPath(Brushes.Gold, Brushes.Black, 0.5, "M0,20 L15,0 L45,0 L60,20 L45,40 L15,40 L0,20", 0, 0);
            Children.Add(border);

            Path inner = GetPath(Brushes.White, Brushes.Black, 0.5, "M2,20 L16,2 L44,2 L58,20 L44,38 L16,38 L2,20", 0, 0);
            Children.Add(inner);

            Turns = GetPath(TurnBrush, null, 0, "M8,8 v-8 a8,8 0 0,1 0,16z", 11, 4);
            Children.Add(Turns);

            Path Clock = GetPath(null, Brushes.Black, 1.25, "M0.63,8 a7.4,7.4 0 1,1 0,0.01 M8,0 v3.75 M0,8 h3.75 M16,8 h-3.75 M8,16 v-3.75", 11, 4);
            Children.Add(Clock);

            //Path arrow1 = GetPath(ArrowBrush, null, 0, "M1,12.8 a12.6,12.6 0 0,1 21,-1  v-1 a12.6,12.6 0 0,0 -22,-6", 27, 6);
            //Children.Add(arrow1);

            //Path arrow2 = GetPath(null, ArrowBrush, 2, "M17,11.8 h6 v-6 a8,8 0 0,1 -6,6", 27, 6);
            //Children.Add(arrow2);

            if (jump != 0)
            {
                Moves = PutLabel(12, 25, -2);
                Children.Add(Moves);
                Moves.Content = Common.GetRectangle(20, Computerclass.JumpRangeBrushes[jump - 1]);
            }
            Path hex1 = GetPath(Brushes.Black, Brushes.Black, 0.5, "M0,5.2 l3,-5.2 l6,0 l3,5.2 l-3.2,5.2 l-6,0z", 10, 22);
            Children.Add(hex1);

            Path hex2 = GetPath(Brushes.Green, Brushes.Black, 0.5, "M9,10.4 l3,-5.2 l6,0 l3,5.2 l-3.2,5.2 l-6,0z", 10, 22);
            Children.Add(hex2);

            Path arrow3 = GetPath(null, Brushes.White, 1, "M6,5.2 l9,5.2 M13,10.4 h2 l-1,-1.8 ", 10, 22);
            Children.Add(arrow3);

            Energy = PutLabel(12, 28, 18);
            Children.Add(Energy);
            Energy.Content = energy;

            Back = GetPath(null, null, 0, "M2,20 L16,2 L44,2 L58,20 L44,38 L16,38 L2,20", 0, 0);
            Children.Add(Back);
        }
        public void SetHex(byte CurHex)
        {
            switch (CurHex)
            {
                case 200: Back.Fill = BaseBrush; break;
                case 201: SetTurns(1); Back.Fill = null; break;
                case 202: SetTurns(2); Back.Fill = null; break;
                case 203: SetTurns(3); Back.Fill = null; break;
                case 211: SetTurns(0); Back.Fill = null; break;
                case 212: SetTurns(0); Back.Fill = null; break;
                case 213: SetTurns(0); Back.Fill = null; break;
                case 210: Back.Fill = BaseBrush; break;
                case 220: Back.Fill = DestroyBrush; break;
                case 254: SetTurns(4); Back.Fill = null; break;
                default: SetTurns(0); Back.Fill = null; break;
            }
        }
        void SetTurns(byte value)
        {
            switch (value)
            {
                case 0: Turns.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M8,8 v-8 a8,8 0 0,1 0,0z")); break;
                case 1: Turns.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M8,8 v-8 a8,8 0 0,1 8,8z")); break;
                case 2: Turns.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M8,8 v-8 a8,8 0 0,1 0,16z")); break;
                case 3: Turns.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M8,8 v-8 a8,8 0 1,1 -8,8z")); break;
                default: Turns.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M8,8 v-8 a8,8 0 1,0 0.01,0z")); break;
            }
        }
        public static Brush GetBrush(Color color)
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.3);
            brush.GradientStops.Add(new GradientStop(color, 1));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            return brush;
        }
        static Label PutLabel(int size, int left, int top)
        {
            Label lbl = new Label();
            lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
            lbl.FontSize = size;
            lbl.FontFamily = Links.Font;
            Canvas.SetLeft(lbl, left);
            Canvas.SetTop(lbl, top);
            lbl.FontWeight = FontWeights.Bold;
            return lbl;
        }
        static Path GetPath(Brush fill, Brush stroke, double stroketh, string data, int left, int top)
        {
            Path border = new Path();
            border.Fill = fill;
            border.Stroke = stroke;
            border.StrokeThickness = stroketh;
            border.StrokeLineJoin = PenLineJoin.Round;
            border.StrokeStartLineCap = PenLineCap.Round;
            border.StrokeEndLineCap = PenLineCap.Round;
            border.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(data));
            Canvas.SetLeft(border, left); Canvas.SetTop(border, top);
            return border;
        }
    }
    class AbsorpCanvas : Canvas
    {
        public Label Energy;
        public Label Physic;
        public Label Irregular;
        public Label Cyber;
        public AbsorpCanvas(int energy, int physic, int irregular, int cyber):this()
        {
            Energy.Content = energy;
            Physic.Content = physic;
            Irregular.Content = irregular;
            Cyber.Content = cyber;
            Width = 50; Height = 50;
        }
        public AbsorpCanvas()
        {
            Path border = new Path(); border.Stroke = Brushes.Black; border.StrokeThickness = 0.5; border.Fill = Brushes.Gold;
            GeometryGroup group = new GeometryGroup();
            group.Children.Add(new EllipseGeometry(new Point(25, 25), 25, 25));
            group.Children.Add(new EllipseGeometry(new Point(25, 25), 23, 23));
            group.Children.Add(new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M24,2 a15,15 0 0,0 0,23 a15,15 0 0,1 0,23 a23,23 0 0,0 2,0 15,15 0 0,0 0,-23 a15,15 0 0,1 0,-23 z")));
            border.Data = group;
            Children.Add(border);
            Energy = PutPath("M48,25 a15,15 0 0,0 -22,0 a15,15 0 0,1 0,-23 a 23,23 0 0,1 22,23", ShipPanelBrushes.EnergyWBrush, 21, 1);
            Physic = PutPath("M2,25 a15,15 0 0,0 22,0 a15,15 0 0,1 0,-23 a23,23 0 0,0 -22,23", ShipPanelBrushes.PhysicBrush, 0, 8);
            Irregular = PutPath("M48,25 a 15,15 0 0,0 -22,0 a15,15 0 0,1 0,23 a23,23 0 0,0 22,-23", ShipPanelBrushes.IrregularBrush, 26, 18);
            Cyber = PutPath("M2,25 a15,15 0 0,0 22,0 a15,15 0 0,1 0,23 a 23,23 0 0,1 -22,-23", ShipPanelBrushes.CyberBrush, 7 , 25);
        }
        Label PutPath(string text, Brush brush, double left, double top)
        {
            Path result = new Path();
            result.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(text));
            Children.Add(result);
            result.Fill = brush;
            Label lbl = new Label();
            lbl.FontFamily = Links.Font;
            lbl.FontWeight = FontWeights.Bold;
            lbl.FontSize = 12;
            Children.Add(lbl);
            Canvas.SetLeft(lbl, left);
            Canvas.SetTop(lbl, top);
            return lbl;
        }
    }
    class EvasionCanvas : Canvas
    {
        public Label Energy;
        public Label Physic;
        public Label Irregular;
        public Label Cyber;
        public EvasionCanvas(int energy, int physic, int irregular, int cyber):this()
        {
            Energy.Content = energy;
            Physic.Content = physic;
            Irregular.Content = irregular;
            Cyber.Content = cyber;
            Width = 50; Height = 50;
        }
        public EvasionCanvas()
        {
            Path border = new Path(); border.Stroke = Brushes.Black; border.StrokeThickness = 0.5; border.Fill = Brushes.Gold;
            GeometryGroup group = new GeometryGroup();
            group.Children.Add(new EllipseGeometry(new Point(25, 25), 25, 25));
            group.Children.Add(new EllipseGeometry(new Point(25, 25), 23, 23));
            group.Children.Add(new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M4,3 L47,46 L46,47 L3,4z")));
            group.Children.Add(new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M47,4 L4,47 L3,46 L46,3z")));
            border.Data = group;
            Children.Add(border);
            Energy = PutPath("M25,24 l16.0,-16.0 a23,23 0 0,0 -32.0,0z", ShipPanelBrushes.EnergyWBrush, 50,0,-2,13);
            Physic = PutPath("M24,25 l-16.0,-16.0 a23,23 0 0,0 0.0,32z", ShipPanelBrushes.PhysicBrush, 28, -3, 12, 13);
            Irregular = PutPath("M26,25 l16.0,-16.0 a23,23 0 0,1 0.0,32z", ShipPanelBrushes.IrregularBrush, 28, 25, 12, 13);
            Cyber = PutPath("M25,26 l16.0,16.0 a23,23 0 0,1 -32.0,0z", ShipPanelBrushes.CyberBrush, 50, 0, 28, 13);
        }
        Label PutPath(string text, Brush brush, double width, double left, double top, double fontsize)
        {
            Path result = new Path();
            result.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(text));
            Children.Add(result);
            result.Fill = brush;
            GridLabel Pack = ShipPanelBrushes.GetGridWithValue(width, top, left, fontsize, 0);
            Children.Add(Pack.grid);
            return Pack.label;
        }
    }
    class EnergyCanvas : Canvas
    {
        public Label Value;
        public Label Capacity;
        public Label Restore;
        public EnergyCanvas(int capacity, int value, int restore):this()
        {
            Capacity.Content = capacity;
            Value.Content = value;
            Restore.Content = "+" + restore.ToString();
            Width = 50; Height = 50;
        }
        public EnergyCanvas()
        {
            Path border = new Path(); border.StrokeThickness = 0.5; border.Stroke = Brushes.Black; border.StrokeLineJoin = PenLineJoin.Round;
            Children.Add(border);
            border.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M10,0 h25 l-10,15 h10 l-10,15 h10 l-25,10 l-25,-10 h10 l10,-15 h-10 l10,-15z"));
            border.Fill = ShipPanelBrushes.EnergyBrush;
            
            GridLabel Valuegrid = ShipPanelBrushes.GetGridWithValue(50, -5, -9, 12, 0);
            Children.Add(Valuegrid.grid);
            Value = Valuegrid.label;
            GridLabel Capacitygrid = ShipPanelBrushes.GetGridWithValue(50, 9, -9, 11, 0);
            Children.Add(Capacitygrid.grid);
            Capacity = Capacitygrid.label;
            GridLabel Restoregrid = ShipPanelBrushes.GetGridWithValue(50, 20, -13, 10, 0);
            Children.Add(Restoregrid.grid);
            Restore = Restoregrid.label;
            Restore.FontFamily = Links.Font;
        }
    }
    class WeaponCanvas : Canvas
    {
        static PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
        
        Path Path;
        Canvas Image;
        public Label ToShield;
        public Label ToHealth;
        public Label EnergyCost;
        Label SizeLabel;
        public Label Accuracy;
        Brush CurrentBrush;
        public WeaponCanvas()
        {
            Path = new Path();
            Path.Stroke = Brushes.Black; Path.StrokeThickness = 1;
            Path.Data = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M0,30 a25,25 0 0,0 15,25 h25 a25,25 0 0,0 15,-25 h-12 v-30 h -30 v30z"));
            Children.Add(Path);
            GridLabel ToShieldPack = ShipPanelBrushes.GetGridWithValue(28, 25, 0, 11, 0);
            Children.Add(ToShieldPack.grid);
            ToShield = ToShieldPack.label;
            ToShield.Foreground = Brushes.Blue;
            ToShield.Content = 1000;
            GridLabel ToHealthPack = ShipPanelBrushes.GetGridWithValue(28, 25, 28, 11, 0);
            Children.Add(ToHealthPack.grid);
            ToHealth = ToHealthPack.label;
            ToHealth.Content = 1200;
            ToHealth.Foreground = Brushes.Red;
            GridLabel EnergyCostPack = ShipPanelBrushes.GetGridWithValue(28, 37, 5, 11, 0);
            Children.Add(EnergyCostPack.grid);
            EnergyCost = EnergyCostPack.label;
            EnergyCost.Foreground = Brushes.Green;
            EnergyCost.Content = 500;
            SizeLabel = new Label();
            Children.Add(SizeLabel);
            SizeLabel.FontFamily = Links.Font;
            SizeLabel.FontWeight = FontWeights.Bold;
            Canvas.SetLeft(SizeLabel, 18);
            Canvas.SetTop(SizeLabel, 27);
            SizeLabel.FontSize = 6;
            GridLabel AccuracyPack = ShipPanelBrushes.GetGridWithValue(25, 37, 24, 11, 0);
            Accuracy = AccuracyPack.label;
            Children.Add(AccuracyPack.grid);
            Accuracy.Content = 130;
        }
        public void Update(ShipB ship, int weapon)
        {
            ToShield.Content = ship.Weapons[weapon].ToShield();
            ToHealth.Content = ship.Weapons[weapon].ToHealth();
            EnergyCost.Content = ship.Weapons[weapon].Consume;
            Accuracy.Content = ship.Weapons[weapon].Accuracy();
            if (ship.Weapons[weapon]._IsArmed) Arm();
            else DisArm();
        }
        public void Arm()
        {
            Path.Fill = CurrentBrush;
        }
        public void DisArm()
        {
            Path.Fill = Brushes.Gray;
        }
        public void SetWeaponType(BattleWeapon weapon)
        {
            switch (weapon.Group)
            {
                case WeaponGroup.Energy: CurrentBrush = ShipPanelBrushes.EnergyWBrush; Path.Fill = ShipPanelBrushes.EnergyWBrush; break;
                case WeaponGroup.Physic: CurrentBrush = ShipPanelBrushes.PhysicBrush; Path.Fill = ShipPanelBrushes.PhysicBrush; break;
                case WeaponGroup.Irregular: CurrentBrush = ShipPanelBrushes.IrregularBrush; Path.Fill = ShipPanelBrushes.IrregularBrush; break;
                case WeaponGroup.Cyber: CurrentBrush = ShipPanelBrushes.CyberBrush; Path.Fill = ShipPanelBrushes.CyberBrush; break;
            }
            Image = WeapCanvas.GetCanvas(weapon.Type, 30, false);

            int percent = Links.Modifiers[weapon.Type].Crit[weapon.Size];
            SizeLabel.Content = percent.ToString() + "%";
            if (percent < 16) SizeLabel.Foreground = Brushes.Red;
            else if (percent < 31) SizeLabel.Foreground = Brushes.Orange;
            else if (percent < 46) SizeLabel.Foreground = Brushes.Yellow;
            else if (percent < 61) SizeLabel.Foreground = Brushes.Green;
            else if (percent < 76) SizeLabel.Foreground = Brushes.SkyBlue;
            else if (percent < 91) SizeLabel.Foreground = Brushes.Blue;
            else SizeLabel.Foreground = Brushes.Purple;

            if (Image == null) return;
            Children.Add(Image);
            Image.Width = 30; Image.Height = 30;
            Canvas.SetLeft(Image, 13);
        }
        public void SetWeaponType(Weaponclass weapon)
        {    
            switch (weapon.GetWeaponGroup())
            {
                case WeaponGroup.Energy: CurrentBrush = ShipPanelBrushes.EnergyWBrush; Path.Fill = ShipPanelBrushes.EnergyWBrush; break;
                case WeaponGroup.Physic: CurrentBrush = ShipPanelBrushes.PhysicBrush; Path.Fill = ShipPanelBrushes.PhysicBrush; break;
                case WeaponGroup.Irregular: CurrentBrush = ShipPanelBrushes.IrregularBrush; Path.Fill = ShipPanelBrushes.IrregularBrush; break;
                case WeaponGroup.Cyber: CurrentBrush = ShipPanelBrushes.CyberBrush; Path.Fill = ShipPanelBrushes.CyberBrush; break;
            }
            Image = WeapCanvas.GetCanvas(weapon.Type, 30, false);

            int percent = Links.Modifiers[weapon.Type].Crit[weapon.Size];
            SizeLabel.Content = percent.ToString() + "%";
            if (percent < 16) SizeLabel.Foreground = Brushes.Red;
            else if (percent < 31) SizeLabel.Foreground = Brushes.Orange;
            else if (percent < 46) SizeLabel.Foreground = Brushes.Yellow;
            else if (percent < 61) SizeLabel.Foreground = Brushes.Green;
            else if (percent < 76) SizeLabel.Foreground = Brushes.SkyBlue;
            else if (percent < 91) SizeLabel.Foreground = Brushes.Blue;
            else SizeLabel.Foreground = Brushes.Purple;

            if (Image == null) return;
            Children.Add(Image);
            Image.Width = 30; Image.Height = 30;
            Canvas.SetLeft(Image, 13);     
        }
        public static WeaponCanvas SetWeaponPosition(int pos, Canvas canvas, BattleWeapon weapon)
        {
            WeaponCanvas W = new WeaponCanvas();
            canvas.Children.Add(W);
            W.SetWeaponType(weapon);
            Canvas.SetTop(W, 0);
            if (pos == 0)
                Canvas.SetLeft(W, 2);
            else if (pos == 1)
                Canvas.SetLeft(W, 86);
            else
                Canvas.SetLeft(W, 170);
            return W;
        }
        public static WeaponCanvas SetWeaponPosition(int pos, Canvas canvas, Weaponclass weapon)
        {
            WeaponCanvas W = new WeaponCanvas();
            canvas.Children.Add(W);
            W.SetWeaponType(weapon);
            Canvas.SetTop(W, 0);
            if (pos == 0)
                Canvas.SetLeft(W, 2);
            else if (pos == 1)
                Canvas.SetLeft(W, 86);
            else 
                Canvas.SetLeft(W,170);
            return W;
        }
    }
    class ShieldCanvas : Canvas
    {
        public Label Value;
        public Label Capacity;
        StackPanel cappanel;
        public Label Restore;
        public ShieldCanvas(int capacity, int value, int restore, byte orient):this()
        {
            Capacity.Content = capacity;
            Value.Content = value;
            Restore.Content = "+" + restore.ToString();
            SetShieldType(orient);
            Width = 50; Height = 50;
        }
        public ShieldCanvas()
        {
            Path border = new Path(); border.StrokeThickness = 0.5; border.Stroke = Brushes.Black; border.StrokeLineJoin = PenLineJoin.Round;
            Children.Add(border);
            border.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,12 a25,12 0 0,1 50,0 a30,50 0 0,1 -25,38 a30,50 0 0,1 -25,-38"));
            border.Fill = ShipPanelBrushes.ShieldBrush;
            GridLabel ValueGrid = ShipPanelBrushes.GetGridWithValue(50, 0, 0, 14, 0);
            Children.Add(ValueGrid.grid);
            Value = ValueGrid.label;
            GridLabel CapacityGrid = ShipPanelBrushes.GetGridWithValue(50, 16, 0, 11, 0);
            Children.Add(CapacityGrid.grid);
            CapacityGrid.grid.Children.Clear();
            cappanel = new StackPanel();
            cappanel.Orientation = Orientation.Horizontal; cappanel.HorizontalAlignment = HorizontalAlignment.Center;
            CapacityGrid.grid.Children.Add(cappanel);
            cappanel.Children.Add(CapacityGrid.label);
            Capacity = CapacityGrid.label;
            GridLabel RestoreGrid = ShipPanelBrushes.GetGridWithValue(50, 28, 0, 11, 0);
            Children.Add(RestoreGrid.grid);
            Restore = RestoreGrid.label;
        }
        public void SetShieldType(byte type)
        {
            cappanel.Children.Clear();
            cappanel.Children.Add(Capacity);
            Grid grid = new Grid(); grid.Width = 10; grid.Height = 10;
            cappanel.Children.Add(grid);
            Ellipse el = new Ellipse(); el.Stroke = Brushes.Black; el.StrokeThickness = 0.2; el.Fill = Brushes.SkyBlue;
            grid.Children.Add(el);
            Path path = new Path(); path.Stroke = Brushes.Blue; path.StrokeThickness = 1; path.Fill = Brushes.Blue;
            grid.Children.Add(path);
            grid.Margin = new Thickness(-3, 0, 0, 0);
            switch (type)
            {
                case 0: path.Data = null; break;
                case 30: path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M5,5 l3.0,-3.2 a4.5,4.5 0 0,0 -6.0,0z")); break;
                case 90: path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M5,5 h4.5 a4.5,4.5 0 0,0 -9,0z")); break;
                case 150: path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M5,5 l3.0,3.2 a4.5,4.5 0 1,0 -6.0,0z")); break;
            }
        }
    }
    class HealthCanvas : Canvas
    {   
        public Label Value;
        public Label Capacity;
        public Label Restore;
        public HealthCanvas()
        {
            Path border = new Path(); border.StrokeThickness = 0.5; border.Stroke = Brushes.Black; border.StrokeLineJoin = PenLineJoin.Round;
            Children.Add(border);
            border.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,12 a12.5,10 0 0,1 25.0,0 a12.5,10 0 0,1 25.0,0 a35,35 0 0,1 -25,38 a35,35 0 0,1 -25,-38"));
            border.Fill = ShipPanelBrushes.HealthBrush;
            GridLabel Valuegrid = ShipPanelBrushes.GetGridWithValue(50, 6, 0, 14, 0);
            Children.Add(Valuegrid.grid);
            Value = Valuegrid.label;
            GridLabel Capacitygrid = ShipPanelBrushes.GetGridWithValue(50, 20, 0, 11, 0);
            Children.Add(Capacitygrid.grid);
            Capacity = Capacitygrid.label;
            GridLabel Restoregrid = ShipPanelBrushes.GetGridWithValue(50, 30, 0, 10, 0);
            Children.Add(Restoregrid.grid);
            Restore = Restoregrid.label;
        }
        public HealthCanvas(int capacity, int value, int restore):this()
        {
            Capacity.Content = capacity;
            Value.Content = value;
            Restore.Content = "+" + restore.ToString();
            Width = 50; Height = 50;
        }
    
    }
    /*
    class ShieldOrient : Canvas
    {
        double angle;
        double da;
        LineSegment ls;
        ArcSegment arc;

        LineSegment ls1;
        LineSegment ls2;
        PathFigure fig2;
        public ShieldOrient(byte ShieldSize)
        {
            Width = 30;
            Height = 30;
            Path path = new Path();

            path.Fill = Brushes.Blue;
            //path.Stroke = Brushes.Blue;
            PathGeometry geom = new PathGeometry();
            PathFigure fig = new PathFigure();
            geom.Figures.Add(fig);
            path.Data = geom;
            fig.StartPoint = new Point(15, 15);
            ls = new LineSegment();
            fig.Segments.Add(ls);
            fig.IsClosed = true;
            arc = new ArcSegment();
            arc.SweepDirection = SweepDirection.Clockwise;
            fig.Segments.Add(arc);
            angle = 90;
            da = ShieldSize * 2;
           
            arc.Size = new Size(7.5, 7.5);

            Path arrow = new Path();
            arrow.Fill = Brushes.Red;
            ls1 = new LineSegment();
            ls2 = new LineSegment();
            fig2 = new PathFigure();
            fig2.Segments.Add(ls1);
            fig2.Segments.Add(ls2);
            fig2.IsClosed = true;
            PathGeometry geom2 = new PathGeometry();
            geom2.Figures.Add(fig2);
            arrow.Data = geom2;
            Children.Add(path);
            Children.Add(arrow);

            Draw();
        }
        public void SetAngle(double Angle)
        {
            angle = Angle;
            Draw();
        }
        void Draw()
        {
            double lowangle = angle - da / 2;
            double largeangle = angle + da / 2;
            double xlow = 7.5 * Math.Sin(lowangle * Math.PI / 180) + 15;
            double ylow = 15 - 7.5 * Math.Cos(lowangle * Math.PI / 180);
            double xlarge = 7.5 * Math.Sin(largeangle * Math.PI / 180) + 15;
            double ylarge = 15 - 7.5 * Math.Cos(largeangle * Math.PI / 180);
            ls.Point = new Point(xlow, ylow);
            arc.Point = new Point(xlarge, ylarge);
            arc.IsLargeArc = da > 180;

            double xmid = 10.0 * Math.Sin(angle * Math.PI / 180) + 15;
            double ymid = 15 - 10.0 * Math.Cos(angle * Math.PI / 180);
            double xlow1 = 7.5 * Math.Sin((angle - 15) * Math.PI / 180) + 15;
            double ylow1 = 15 - 7.5 * Math.Cos((angle - 15) * Math.PI / 180);
            double xlarge1 = 7.5 * Math.Sin((angle + 15) * Math.PI / 180) + 15;
            double ylarge1 = 15 - 7.5 * Math.Cos((angle + 15) * Math.PI / 180);
            fig2.StartPoint = new Point(xmid, ymid);
            ls1.Point = new Point(xlow1, ylow1);
            ls2.Point = new Point(xlarge1, ylarge1);
        }
    }
    */
}
