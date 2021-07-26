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
    public class ShipPanel2:Canvas
    {
        TextBlock HD, SD, MS;
        public ShipB Ship;
        public GSShip GSShip;
        UIElement ImmuneBlock;
        SortedList<WeaponGroup, AbsorpElement2> Absorp;
        SortedList<WeaponGroup, AbsorpElement2> Evasion;
        MainParamElement Health;
        MainParamElement Shield;
        MainParamElement Energy;
        UIElement fleetrect;
        static SolidColorBrush LightRed = new SolidColorBrush(Color.FromRgb(255, 100, 100));
        Rectangle ramka;
        static SolidColorBrush ShieldBrush = new SolidColorBrush(Color.FromRgb(60, 60, 255));
        public static ShipPanel2 GetShipPanel2 (Schema schema)
        {
            ShipB ship  = new ShipB(1, schema, 100, new GSPilot(GSPilot.GetNullPilot(), 0), new byte[4], ShipSide.Attack, null, false,null,  ShipBMode.Info, 255);
            return new ShipPanel2(ship, null);
        }
        public ShipPanel2(ShipB ship, GSShip gsship)
        {
            Ship = ship;
            GSShip = gsship;
            Width = 675; Height = 380;
            ramka = new Rectangle();
            ramka.Width = 675; ramka.Height = 380;
            ramka.Fill = Links.Brushes.Interface.RamkaSmall;
            Children.Add(ramka);
            //Background = Brushes.Black;
            Canvas.SetZIndex(ramka, 20);
            //UpdateLevel();
            HD = Common.GetBlock(26, "1200", Brushes.Red, 60);
            Children.Add(HD);
            Canvas.SetLeft(HD, 200);
            Canvas.SetTop(HD, 305);
            Canvas.SetZIndex(HD, 30);

            SD = Common.GetBlock(26, "800", ShieldBrush, 60);
            Children.Add(SD);
            Canvas.SetLeft(SD, 140);
            Canvas.SetTop(SD, 305);
            Canvas.SetZIndex(SD, 30);

            MS = Common.GetBlock(26, "100%", Brushes.Green, 60);
            Children.Add(MS);
            Canvas.SetLeft(MS, 170);
            Canvas.SetTop(MS, 48);
            Canvas.SetZIndex(MS, 30);

            Absorp = new SortedList<WeaponGroup, AbsorpElement2>();
            Absorp.Add(WeaponGroup.Energy, new AbsorpElement2(Pictogram.IgnoreEnergy, this, -112, Brushes.SkyBlue, Brushes.Blue, false));
            Absorp.Add(WeaponGroup.Physic, new AbsorpElement2(Pictogram.IgnorePhysic, this, -157, LightRed, Brushes.Red, false));
            Absorp.Add(WeaponGroup.Irregular, new AbsorpElement2(Pictogram.IgnoreIrregular, this, -202, Brushes.Pink, Brushes.Purple, false));
            Absorp.Add(WeaponGroup.Cyber, new AbsorpElement2(Pictogram.IgnoreCyber, this, -247, Brushes.LightGreen, Brushes.Green, false));

            Evasion = new SortedList<WeaponGroup, AbsorpElement2>();
            Evasion.Add(WeaponGroup.Energy, new AbsorpElement2(Pictogram.EvasionEnergy, this, -65, Brushes.SkyBlue, Brushes.Blue, true));
            Evasion.Add(WeaponGroup.Physic, new AbsorpElement2(Pictogram.EvasionPhysic, this, -20, LightRed, Brushes.Red, true));
            Evasion.Add(WeaponGroup.Irregular, new AbsorpElement2(Pictogram.EvasionIrregular, this, 25, Brushes.Pink, Brushes.Purple, true));
            Evasion.Add(WeaponGroup.Cyber, new AbsorpElement2(Pictogram.EvasionCyber, this,70 , Brushes.LightGreen, Brushes.Green, true));
          
            Health = new MainParamElement(this, 310, 127, Pictogram.HealthBrush, 35, Brushes.Red);
            Shield = new MainParamElement(this, 330, 170, Pictogram.ShieldBrush, 35, ShieldBrush);
            Energy = new MainParamElement(this, 345, 215, Pictogram.EnergyBrush, 30, Brushes.Green);

            Update();
            //PreviewMouseDown += ShipPanel2_PreviewMouseDown;
        }
        /*
        private void ShipPanel2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton==MouseButtonState.Pressed)
            {
                e.Handled = true;
                Links.Controller.ShipPopUp.Place(Ship);
                if (Parent.GetType() == typeof(System.Windows.Controls.Primitives.Popup))
                    ((System.Windows.Controls.Primitives.Popup)Parent).IsOpen = false;
                /*
                if (Ship.Schema != null)
                {
                    Links.Controller.ShipPopUp.Place(Ship.Schema);
                    if (Parent.GetType() == typeof(System.Windows.Controls.Primitives.Popup))
                        ((System.Windows.Controls.Primitives.Popup)Parent).IsOpen = false;
                }
                */
          //  }
        //}

        public static ShipPanel2 FromSchema(Schema schema)
        {
            GSShip ship = new GSShip(100, schema, 100, 0);
            ShipB shipb = new ShipB(10, schema, 100, null, new byte[4], ShipSide.Attack, null, true, null, ShipBMode.Info, 255);
            ShipPanel2 panel = new ShipPanel2(shipb, ship);
            return panel;
        }
        public void SetPilot()
        {
            if (GSShip == null) return;
            if (GSShip.Pilot != null)
            {
                Rectangle pilotexp = PilotsImage.GetPilotExp(GSShip.Pilot);
                Children.Add(pilotexp);
                Canvas.SetLeft(pilotexp, 410);
                Canvas.SetTop(pilotexp, 267);
                Canvas.SetZIndex(pilotexp, 30);
                pilotexp.RenderTransformOrigin = new Point(0.5, 0.5);
                ScaleTransform scale = new ScaleTransform();
                scale.ScaleX = 1.5;
                pilotexp.RenderTransform = scale;
                Rectangle pilotrating = PilotsImage.GetRatingImage(GSShip.Pilot.Specialization, GSShip.Pilot.Talent, GSShip.Pilot.Rang, 55);
                Children.Add(pilotrating);
                Canvas.SetLeft(pilotrating, 277);
                Canvas.SetTop(pilotrating, 265);
                Canvas.SetZIndex(pilotrating, 30);
                pilotrating.ToolTip = new PilotsImage(GSShip.Pilot, PilotsListMode.Ship, GSShip);
                if (GSShip.Pilot.IsBasicPilot())
                    pilotrating.MouseDown += PutPilot_Click;
                else
                    pilotrating.MouseDown += RemovePilot_Click;
            }
            else
            {
                Rectangle nopilot = Common.GetRectangle(55, Links.Brushes.EmptyImageBrush);
                Children.Add(nopilot); Canvas.SetLeft(nopilot, 277); Canvas.SetTop(nopilot, 265); Canvas.SetZIndex(nopilot, 30);
                nopilot.PreviewMouseDown += PutPilot_Click;
            }
        }
        void PutPilot_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Links.Helper.SetShip(GSShip);
            PilotsListPanel panel = new PilotsListPanel(PilotsListMode.Select);
            Links.Controller.PopUpCanvas.Place(panel);

        }
        void RemovePilot_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            string result = Events.MovePilotFromShip(GSShip.ID);
            if (result == "")
            {
                Links.Controller.PopUpCanvas.Remove();
                Links.Controller.SelectPanel(GamePanels.ShipsCanvas, SelectModifiers.None);
            }
            else Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(result), true);

        }
        public void SetFleet(byte[] Image)
        {
            FleetEmblem emblem = new FleetEmblem(BitConverter.ToInt32(Image, 0), 70);
            Children.Add(emblem);
            Canvas.SetLeft(emblem, 52); Canvas.SetTop(emblem, 265); Canvas.SetZIndex(emblem, 30);
        }
        public void SetFleet(GSFleet fleet, bool canchange)
        {
            if (fleetrect != null) Children.Remove(fleetrect);
            if (fleet == null)
            {
                fleetrect = Common.GetRectangle(70, Links.Brushes.EmptyImageBrush);
                Children.Add(fleetrect); Canvas.SetLeft(fleetrect, 52); Canvas.SetTop(fleetrect, 265); Canvas.SetZIndex(fleetrect, 30);
                if (canchange) fleetrect.PreviewMouseDown += fleetborder_PreviewMouseDown;
                ramka.Fill = Links.Brushes.Interface.RamkaSmall2;
                Rectangle shesternya = Common.GetRectangle(78, Links.Brushes.Interface.Shesternya, Links.Interface("DestroyShip"));
                Children.Add(shesternya); Canvas.SetLeft(shesternya, 45); Canvas.SetTop(shesternya, 40); Canvas.SetZIndex(shesternya, 30);
                shesternya.PreviewMouseDown += Deletebutton_PreviewMouseDown;
            }
            else
            {
                fleetrect = new FleetEmblem(BitConverter.ToInt32(fleet.Image, 0), 70);
                Children.Add(fleetrect);
                Canvas.SetLeft(fleetrect, 52); Canvas.SetTop(fleetrect, 265); Canvas.SetZIndex(fleetrect, 30);
                if (canchange) fleetrect.PreviewMouseDown += RemoveFleet_Click;
            }
            if (GSShip.Health!=100)
            {
                ramka.Fill = Links.Brushes.Interface.RamkaSmall2;
                int repairvalue = 0;
                if (fleet == null)
                {
                    repairvalue = 0;
                }
                else if (fleet.Target!= null)
                {
                    repairvalue = -1;
                }
                else
                {
                    GSShip.Schema.CalcPrice();
                    int RepairPrice = (GSShip.Schema.Price.Money / 10 + GSShip.Schema.Price.Metall + GSShip.Schema.Price.Chips + GSShip.Schema.Price.Anti) / 10;
                    repairvalue = ((int)GSShip.Fleet.FleetBase.Land.GetAddParameter(Building2Parameter.Repair) / RepairPrice);
                }
                Rectangle repairindicator = GetRepairIndicator(GSShip.Health, repairvalue);
                Children.Add(repairindicator);
                Canvas.SetLeft(repairindicator, 295);
                Canvas.SetTop(repairindicator, 36);
                Canvas.SetZIndex(repairindicator, 30);
                Border repairpanel = GetRepairPanel();
                Children.Add(repairpanel);
                Canvas.SetLeft(repairpanel, 385);
                Canvas.SetTop(repairpanel, 30);
                Canvas.SetZIndex(repairpanel, 30);
                Rectangle repairbutton = Common.GetRectangle(50, Links.Brushes.Interface.Shesternya);
                Children.Add(repairbutton);
                Canvas.SetLeft(repairbutton, 610);
                Canvas.SetTop(repairbutton, 25);
                Canvas.SetZIndex(repairbutton, 30);
                repairbutton.PreviewMouseDown += ShipRepairPanel_PreviewMouseDown;
                repairbutton.ToolTip = Links.Interface("Repair");
                if (fleet!= null)
                {
                    Rectangle zakryvashka1= Common.GetRectangle(78, Links.Brushes.Interface.Zakryvashka);
                    Children.Add(zakryvashka1); Canvas.SetLeft(zakryvashka1, 45); Canvas.SetTop(zakryvashka1, 40); Canvas.SetZIndex(zakryvashka1, 30);
                }
            }
            else if (fleet==null)
            {
                Rectangle zakryvashka2 = Common.GetRectangle(50, Links.Brushes.Interface.Zakryvashka);
                Children.Add(zakryvashka2); Canvas.SetLeft(zakryvashka2, 610); Canvas.SetTop(zakryvashka2, 25); Canvas.SetZIndex(zakryvashka2, 40);
            }
        }
        void ShipRepairPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (!GSShip.Schema.Price.CheckPrice(Ship.StartHealth))
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("NoMoney")), true);
                return;
            }
            string repairresult = Events.RepairShip(GSShip);
            if (repairresult == "")
            {
                Gets.GetResources();
                Links.Controller.SelectPanel(GamePanels.ShipsCanvas, SelectModifiers.None);
                return;
            }
            else
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(repairresult), true);
            }
        }
        Border GetRepairPanel()
        {
            Border border = new Border();
            //border.BorderBrush = Brushes.White;
            //border.BorderThickness = new Thickness(3);
            //border.CornerRadius = new CornerRadius(10);
            border.Width = 210;
            border.Height = 85;
            border.Background = Links.Brushes.Interface.ResBack;
            Canvas canvas = new Canvas();
            border.Child = canvas;

            GSShip.Schema.CalcPrice();
            ItemPrice RepairPrice;
            if (GSShip.Health == 0)
                RepairPrice = Ship.Schema.Price;
            else
                RepairPrice = new ItemPrice(Ship.Schema.Price.Money / 100 * (100 - GSShip.Health),
                    Ship.Schema.Price.Metall / 100 * (100 - GSShip.Health),
                    Ship.Schema.Price.Chips / 100 * (100 - GSShip.Health),
                    Ship.Schema.Price.Anti / 100 * (100 - GSShip.Health));
            Common.PutRect(canvas, 30, Links.Brushes.MoneyImageBrush, 5, 5, false);
            Common.PutLabel(canvas, 35, 3, 24, RepairPrice.Money.ToString(), Brushes.White);
            Common.PutRect(canvas, 30, Links.Brushes.MetalImageBrush, 5, 45, false);
            Common.PutLabel(canvas, 35, 42, 24, RepairPrice.Metall.ToString(), Brushes.White);
            Common.PutRect(canvas, 30, Links.Brushes.ChipsImageBrush, 110, 5, false);
            Common.PutLabel(canvas, 145, 3, 24, RepairPrice.Chips.ToString(), Brushes.White);
            Common.PutRect(canvas, 30, Links.Brushes.AntiImageBrush, 110, 45, false);
            Common.PutLabel(canvas, 145, 42, 24, RepairPrice.Anti.ToString(), Brushes.White);
            return border;
        }
        static GlyphTypeface gt = new GlyphTypeface(new Uri("pack://application:,,,/resources/Agru.ttf", UriKind.Absolute), StyleSimulations.BoldSimulation);
        static Pen RedPen = new Pen(Brushes.Red, 10);
        static RadialGradientBrush redbrush = Common.GetRadialBrush(Colors.Red, 0.8, 0.3);
        static Pen BlackPen = new Pen(Brushes.Black, 10);
        static Rectangle GetRepairIndicator(byte percent, int add)
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
        private void Deletebutton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            string eventresult = Events.DestroyShip(GSShip);
            if (eventresult == "")
            {
                Links.Controller.SelectPanel(GamePanels.ShipsCanvas, SelectModifiers.None);
                return;
            }
            else
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult), true);
        }
        void RemoveFleet_Click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (GSShip.Fleet.Target!=null)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("ShipBusy")), true);
                return;
            }
            string eventresult = Events.RemoveShipFromFleet(GSShip);
            if (eventresult == "")
            {
                Links.Controller.ShipsCanvas.Select();
            }
            else
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult), true);
            }
        }
        void fleetborder_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (GSShip.Pilot == null)
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Interface("NoPilot")), true);
                return;
            }
            Links.Helper.SetShip(GSShip);
            Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.FleetForShip);
        }
        public void Update()
        {
            UpdateMain();
            UpdateImmune();
            UpdateArmor();
            UpdateEvasion();
            UpdateWeapon();
            UpdateSpeed();
        }
        public void UpdateMain()
        {
            Health.SetValue(Ship.Params.Health.GetMax, Ship.Params.Health.GetRestore);
            Health.SetOpp(Ship.Params.Health.GetMax / 1400.0);
            Shield.SetValue(Ship.Params.Shield.GetMax, Ship.Params.Shield.GetRestore);
            Shield.SetOpp(Ship.Params.Shield.GetMax / 1400.0);
            Energy.SetValue(Ship.Params.Energy.GetMax, Ship.Params.Energy.GetRestore);
            Energy.SetOpp(Ship.Params.Energy.GetMax / 1400.0);
        }
        public void UpdateSpeed()
        {
            if (Ship.Params.Energy.GetMax <= 0) { MS.Text = "999%"; return; }
            int speedspent = Ship.Params.HexMoveCost.GetCurrent*100 / Ship.Params.Energy.GetMax;
            MS.Text = speedspent.ToString()+"%";
        }
        public void UpdateWeapon()
        {
            int healthdamage = 0; int shielddamage = 0;
            foreach (BattleWeapon weapon in Ship.Weapons)
            { if (weapon == null) continue; healthdamage += weapon.ToHealth(); shielddamage += weapon.ToShield(); }
            HD.Text = healthdamage.ToString(); SD.Text = shielddamage.ToString();
        }
        public void UpdateLevel()
        {
            if (Ship.Schema == null) return;
            int sum = 0; int count = 0;
            if (Ship.Schema.ShipType != null) { sum += Ship.Schema.ShipType.Level; count++; }
            if (Ship.Schema.Generator != null) { sum += Ship.Schema.Generator.Level; count++; }
            if (Ship.Schema.Shield != null) { sum += Ship.Schema.Shield.Level; count++; }
            if (Ship.Schema.Engine != null) { sum += Ship.Schema.Engine.Level; count++; }
            if (Ship.Schema.Computer!=null) { sum += Ship.Schema.Computer.Level; count++; }
            if (Ship.Schema.GetWeapon(0)!=null) { sum += Ship.Schema.GetWeapon(0).Level; count++; }
            if (Ship.Schema.GetWeapon(1) != null) { sum += Ship.Schema.GetWeapon(1).Level; count++; }
            if (Ship.Schema.GetWeapon(2) != null) { sum += Ship.Schema.GetWeapon(2).Level; count++; }
            if (Ship.Schema.GetEquipment(0)!=null) { sum += Ship.Schema.GetEquipment(0).Level; count++; }
            if (Ship.Schema.GetEquipment(1) != null) { sum += Ship.Schema.GetEquipment(1).Level; count++; }
            if (Ship.Schema.GetEquipment(2) != null) { sum += Ship.Schema.GetEquipment(2).Level; count++; }
            if (Ship.Schema.GetEquipment(3) != null) { sum += Ship.Schema.GetEquipment(3).Level; count++; }
            int level = (int)Math.Round(sum / (double)count, 0);
            Ellipse he = new Ellipse();
            he.Width = 80; he.Height = 80;
            Children.Add(he); Canvas.SetLeft(he, 265); Canvas.SetTop(he, 250); Canvas.SetZIndex(he, 40);
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Black, 1));
            brush.GradientStops.Add(new GradientStop(Color.FromRgb(128,128,128), 0));
            he.Fill = brush;
            TextBlock block = Common.GetBlock(40, level.ToString(), Brushes.White, 70);
            Children.Add(block);
            Canvas.SetLeft(block, 270); Canvas.SetTop(block, 265); Canvas.SetZIndex(block, 40);

        }
        public void UpdateArmor()
        {
            Absorp[WeaponGroup.Energy].SetValue(Ship.Params.Ignore.GetCurValue(WeaponGroup.Energy));
            Absorp[WeaponGroup.Physic].SetValue(Ship.Params.Ignore.GetCurValue(WeaponGroup.Physic));
            Absorp[WeaponGroup.Irregular].SetValue(Ship.Params.Ignore.GetCurValue(WeaponGroup.Irregular));
            Absorp[WeaponGroup.Cyber].SetValue(Ship.Params.Ignore.GetCurValue(WeaponGroup.Cyber));
        }
        public void UpdateEvasion()
        {
            Evasion[WeaponGroup.Energy].SetValue(Ship.Params.Evasion.GetCurValue(WeaponGroup.Energy));
            Evasion[WeaponGroup.Physic].SetValue(Ship.Params.Evasion.GetCurValue(WeaponGroup.Physic));
            Evasion[WeaponGroup.Irregular].SetValue(Ship.Params.Evasion.GetCurValue(WeaponGroup.Irregular));
            Evasion[WeaponGroup.Cyber].SetValue(Ship.Params.Evasion.GetCurValue(WeaponGroup.Cyber));
        }
                public void UpdateImmune()
        {
            if (ImmuneBlock != null) Children.Remove(ImmuneBlock);
            if (Ship.Params.Immune.GetCurValue(WeaponGroup.Energy) == 0 && Ship.Params.Immune.GetCurValue(WeaponGroup.Physic) == 0 &&
                 Ship.Params.Immune.GetCurValue(WeaponGroup.Irregular) == 0 && Ship.Params.Immune.GetCurValue(WeaponGroup.Cyber) == 0)
            {
                TextBlock Block = Common.GetBlock(16, "Иммунитетов нет", Brushes.White, 100);
                Block.Height = 25; Children.Add(Block); Canvas.SetLeft(Block, 20); Canvas.SetTop(Block, 175);
                Canvas.SetZIndex(Block, 30); Block.RenderTransformOrigin = new Point(0.5, 0.5);
                RotateTransform immunerotate = new RotateTransform(-90); Block.RenderTransform = immunerotate;
                ImmuneBlock = Block;
            }
            else
            {
                StackPanel ImmunePanel = new StackPanel();
                ImmunePanel.Orientation = Orientation.Vertical; Children.Add(ImmunePanel); Canvas.SetLeft(ImmunePanel, 52);
                Canvas.SetTop(ImmunePanel, 140); Canvas.SetZIndex(ImmunePanel, 30);
                ImmuneBlock = ImmunePanel;

                if (Ship.Params.Immune.GetCurValue(WeaponGroup.Energy) > 0) ImmunePanel.Children.Add(Common.GetRectangle(27, Pictogram.ImmuneEnergy));
                if (Ship.Params.Immune.GetCurValue(WeaponGroup.Physic) > 0) ImmunePanel.Children.Add(Common.GetRectangle(27, Pictogram.ImmunePhysic));
                if (Ship.Params.Immune.GetCurValue(WeaponGroup.Irregular) > 0) ImmunePanel.Children.Add(Common.GetRectangle(27, Pictogram.ImmuneIrregular));
                if (Ship.Params.Immune.GetCurValue(WeaponGroup.Cyber) > 0) ImmunePanel.Children.Add(Common.GetRectangle(27, Pictogram.ImmuneCyber));
            }
        }
        
        
    }
    class AbsorpElement2
    {
        static PathGeometry Element15 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-0.2,-5 h0.5 M-5,-17 h10 M-10,-29 h20 M-15,-41 h30 M-20,-53 h40"));
        static PathGeometry Element14 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-5,-17 h10 M-10,-29 h20 M-15,-41 h30 M-20,-53 h40"));
        static PathGeometry Element13 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-10,-29 h20 M-15,-41 h30 M-20,-53 h40"));
        static PathGeometry Element12 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-15,-41 h30 M-20,-53 h40"));
        static PathGeometry Element11 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-20,-53 h40"));
        static PathGeometry Element25 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-3,-11 h6 M-7.5,-23 h15 M-12.5,-35 h25 M-17.5,-47 h35 M-22.5,-59 h45"));
        static PathGeometry Element24 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-7.5,-23 h15 M-12.5,-35 h25 M-17.5,-47 h35 M-22.5,-59 h45"));
        static PathGeometry Element23 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-12.5,-35 h25 M-17.5,-47 h35 M-22.5,-59 h45"));
        static PathGeometry Element22 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-17.5,-47 h35 M-22.5,-59 h45"));
        static PathGeometry Element21 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-22.5,-59 h45"));
        static PathGeometry WhitePath = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(" M0,0 l-34,-85 a91,91 0 0,1 68,0z"));
        Path Path1;
        Path Path2;
        static int[] stepssmall = new int[] { 9, 18, 27, 36, 45, 54, 63, 72, 81, 90 };
        static int[] stepslarge = new int[] { 15, 30, 45, 60, 75, 90, 105, 120, 135, 150 };
        int[] steps;
        static SolidColorBrush Transparent = new SolidColorBrush((Color.FromArgb(40, 0, 0, 0)));
        public AbsorpElement2(Brush Image, Canvas canvas, int angle, Brush ligntbrush, Brush darkbrush, bool islargestep)
        {
            Path1 = new Path();
            steps = islargestep ? stepslarge : stepssmall;
            Path1.Stroke = ligntbrush;
            Path1.StrokeThickness = 6;
            canvas.Children.Add(Path1);
            Canvas.SetLeft(Path1, 196);
            Canvas.SetTop(Path1, 191);
            Canvas.SetZIndex(Path1, 30);
            RotateTransform rotate = new RotateTransform();
            rotate.CenterX = 0; rotate.CenterY = 0; rotate.Angle = angle;
            Path1.RenderTransform = rotate;
            Path1.StrokeStartLineCap = PenLineCap.Round; Path1.StrokeEndLineCap = PenLineCap.Round;

            Path2 = new Path();
            Path2.Stroke = darkbrush;
            Path2.StrokeThickness = 6;
            canvas.Children.Add(Path2);
            Canvas.SetLeft(Path2, 196);
            Canvas.SetTop(Path2, 191);
            Canvas.SetZIndex(Path2, 30);
            Path2.RenderTransform = rotate;
            Path2.StrokeStartLineCap = PenLineCap.Round; Path2.StrokeEndLineCap = PenLineCap.Round;

            Path Path3 = new Path();
            Path3.Stroke = Brushes.White;
            canvas.Children.Add(Path3);
            Canvas.SetLeft(Path3, 196);
            Canvas.SetTop(Path3, 191);
            Canvas.SetZIndex(Path3, 25);
            Path3.RenderTransform = rotate;
            Path3.Data = WhitePath;
            Path3.StrokeDashArray = new DoubleCollection(new double[] { 5 });
            Path3.Fill = darkbrush;
            Path3.OpacityMask = Transparent;

            Rectangle rect = Common.GetRectangle(20, Image);
            canvas.Children.Add(rect);
            double x = 80 * Math.Sin(angle * Math.PI / 180) - 10 + 196;
            double y = -80 * Math.Cos(angle * Math.PI / 180) - 10 + 191;
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            Canvas.SetZIndex(rect, 30);
        }
        public void SetValue(int val)
        {

            if (val < steps[0]) { Path1.Data = null; Path2.Data = null; }
            else if (val < steps[1]) { Path1.Data = null; Path2.Data = Element21; }
            else if (val < steps[2]) { Path1.Data = Element11; Path2.Data = Element21; }
            else if (val < steps[3]) { Path1.Data = Element11; Path2.Data = Element22; }
            else if (val < steps[4]) { Path1.Data = Element12; Path2.Data = Element22; }
            else if (val < steps[5]) { Path1.Data = Element12; Path2.Data = Element23; }
            else if (val < steps[6]) { Path1.Data = Element13; Path2.Data = Element23; }
            else if (val < steps[7]) { Path1.Data = Element13; Path2.Data = Element24; }
            else if (val < steps[8]) { Path1.Data = Element14; Path2.Data = Element24; }
            else if (val < steps[9]) { Path1.Data = Element14; Path2.Data = Element25; }
            else { Path1.Data = Element15;  Path2.Data = Element25; }
        }
    }
    
    class MainParamElement:StackPanel
    {
        static PathGeometry BigLines = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
            "M5,0 v35 M15,0 v35 M25,0 v35 M35,0 v35 M45,0 v35 M55,0 v35 M65,0 v35 M75,0 v35 M85,0 v35 M95,0 v35 M95,0 v35 M105,0 v35 M115,0 v35 M125,0 v35 M135,0 v35"));
        static PathGeometry SmallLines = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
            "M5,0 v30 M15,0 v30 M25,0 v30 M35,0 v30 M45,0 v30 M55,0 v30 M65,0 v30 M75,0 v30 M85,0 v30 M95,0 v30 M95,0 v30 M105,0 v30 M115,0 v30 M125,0 v30 M135,0 v30"));
        LinearGradientBrush oppbrush;
        TextBlock block;
        public MainParamElement(Canvas canvas, int left, int top, Brush image, int imagesize, Brush brush)
        {
            Orientation = Orientation.Horizontal;
            canvas.Children.Add(this);
            Canvas.SetLeft(this, left); Canvas.SetTop(this, top); Canvas.SetZIndex(this, 30);
            Children.Add(Common.GetRectangle(imagesize, image));
            Path path = new Path();
            path.Stroke = brush; path.StrokeThickness = 3;
            if (imagesize == 35)
                path.Data = BigLines;
            else path.Data = SmallLines;
            Children.Add(path);
            oppbrush = new LinearGradientBrush();
            oppbrush.StartPoint = new Point(0, 0.5); oppbrush.EndPoint = new Point(1, 0.5);
            oppbrush.GradientStops.Add(new GradientStop(Colors.White, 0));
            oppbrush.GradientStops.Add(new GradientStop(Colors.White, 1));
            oppbrush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));
            path.OpacityMask = oppbrush;
            block = Common.GetBlock(24, "9999 (+999)", brush, 140);
            Children.Add(block);
        }
        public void SetValue(int val, int restore)
        {
            block.Text = String.Format("{0} (+{1})", val, restore);
        }
        public void SetOpp(double offset)
        {
            oppbrush.GradientStops[1].Offset = offset;
            oppbrush.GradientStops[2].Offset = offset;
        }
    }

}
