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
    class PanelB2:Canvas
    {
        public ShipB Ship;
        MainParams Health;
        MainParams Shield;
        MainParams Energy;
        WeaponPanel[] Weapons = new WeaponPanel[3];
        ProtectParams Armor;
        ProtectParams Evasion;
        TextBlock MoveCost;
        Border NameBorder;
        List<AddPanel> Panels = new List<AddPanel>();
        public PanelB2(ShipB ship)
        {
            Ship = ship;
            Width = 1100; Height = 600;
            Rectangle main = new Rectangle(); main.Width = 1000; main.Height = 600;
            main.Fill = Links.Brushes.Interface.BigSchema;
            Children.Add(main);
            Health = new MainParams(Links.Interface("Hull2"), Pictogram.HealthBrush, 109, this);
            Shield = new MainParams(Links.Interface("Shield"), Pictogram.ShieldBrush, 232, this);
            Energy = new MainParams(Links.Interface("Energy2"), Pictogram.EnergyBrush, 360, this);
            AddText(Links.Interface("Weapons"), 400, 435, 24, 160);
            for (int i=0;i<3; i++)
            {
                if (ship.Weapons[i] == null) continue;
                Weapons[i] = new WeaponPanel(ship.Weapons[i], this, i);
            }
            Armor = new ProtectParams(656, Pictogram.IgnoreEnergy, Pictogram.IgnorePhysic, Pictogram.IgnoreIrregular, Pictogram.IgnoreCyber,
                Links.Interface("EnIgn"), Links.Interface("PhIgn"), Links.Interface("IrIgn"), Links.Interface("CyIgn"), this);
            Evasion = new ProtectParams(803, Pictogram.EvasionEnergy, Pictogram.EvasionPhysic, Pictogram.EvasionIrregular, Pictogram.EvasionCyber,
                Links.Interface("EnEva"), Links.Interface("PhEva"), Links.Interface("IrEva"), Links.Interface("CyEva"), this);
            AddText(Links.Interface("Move"), 753, 362, 24, 154);
            AddRect(Common.GetRectangle(60, Links.Brushes.ShipMoveBrush), 663, 392);
            MoveCost = AddText("", 731, 405, 30, 50);
            if (Ship.Params.JumpDistance>0)
                AddRect(Common.GetRectangle(40, Computerclass.JumpRangeBrushes[Ship.Params.JumpDistance-1]), 889, 400);
            if (Ship.Params.TimeToEnter>0 && Ship.Params.TimeToEnter<=3)
                AddRect(Common.GetRectangle(40, Generatorclass.TurnsBrushes[Ship.Params.TimeToEnter-1]), 812, 400);
            Update();
            PlaceShipImage();
            AddName();
            AddClose();
        }
        void AddClose()
        {
            Ellipse ell = new Ellipse();
            ell.Width = 55; ell.Height = 55; ell.Fill = Links.Brushes.Transparent;
            Children.Add(ell);
            Canvas.SetLeft(ell, 945); Canvas.SetTop(ell, 15);
            ell.PreviewMouseDown += Ell_PreviewMouseDown;
        }

        private void Ell_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.ShipPopUp.Remove();
        }

        void AddName()
        {
            NameBorder = Common.CreateBorder(350, 80, Brushes.Transparent, 0, -5);
            Children.Add(NameBorder);
            Canvas.SetLeft(NameBorder, 315); Canvas.SetTop(NameBorder, 0);
            TextBlock name = Common.GetBlock(32, Ship.GetName(), Brushes.White, 350);
            //if (Ship.States.BigSize)
            //    name = Common.GetBlock(32, "Big ship", Brushes.White, 350);
            //else
            //    name = Common.GetBlock(32, SelectSchemaNameCanvas.GetNameResult(Ship.Schema.ShipType.GetName(), Ship.Schema.CryptName), Brushes.White, 350);
            //name.Font
            //name = Common.GetBlock(40, Ship.Schema.ShipType.GetName() + " " + SelectSchemaNameCanvas.GetNameResult(Ship.Schema.CryptName), Brushes.White, 350);
            NameBorder.Child = name;
            //Children.Add(name);
            //Canvas.SetLeft(name, 310);
            //Canvas.SetTop(name, 10);
        }
        void PlaceShipImage()
        {
            Rectangle rect = new Rectangle(); rect.Width = 280; rect.Height = 260;
            rect.Fill = new VisualBrush(Ship.HexShip);
            Children.Add(rect);
            Canvas.SetLeft(rect, 336);
            Canvas.SetTop(rect, 120);
        }
        public void AddRect(Rectangle rect, int left, int top)
        {
            Children.Add(rect); Canvas.SetLeft(rect, left); Canvas.SetTop(rect, top);
        }
        public void Update()
        {
            Health.Update(Ship.Params.Health.GetCurrent, Ship.Params.Health.GetMax, Ship.Params.Health.GetRestore);
            Shield.Update(Ship.Params.Shield.GetCurrent, Ship.Params.Shield.GetMax, Ship.Params.Shield.GetRestore);
            Energy.Update(Ship.Params.Energy.GetCurrent, Ship.Params.Energy.GetMax, Ship.Params.Energy.GetRestore);
            Armor.Update(Ship.Params.Ignore.GetCurValue(WeaponGroup.Energy), Ship.Params.Ignore.GetCurValue(WeaponGroup.Physic),
                Ship.Params.Ignore.GetCurValue(WeaponGroup.Irregular), Ship.Params.Ignore.GetCurValue(WeaponGroup.Cyber));
            Evasion.Update(Ship.Params.Evasion.GetCurValue(WeaponGroup.Energy), Ship.Params.Evasion.GetCurValue(WeaponGroup.Physic),
                Ship.Params.Evasion.GetCurValue(WeaponGroup.Irregular), Ship.Params.Evasion.GetCurValue(WeaponGroup.Cyber));
            MoveCost.Text = Ship.Params.HexMoveCost.GetCurrent.ToString();
            foreach (AddPanel panel in Panels)
                Children.Remove(panel.Panel);
            Panels.Clear();
            if (Ship.Params.Immune.GetCurValue(WeaponGroup.Energy) > 0) Panels.Add(new GroupPanel(new SecondaryEffect(59, Ship.Params.Immune.GetCurValue(WeaponGroup.Energy))));
            if (Ship.Params.Immune.GetCurValue(WeaponGroup.Physic) > 0) Panels.Add(new GroupPanel(new SecondaryEffect(60, Ship.Params.Immune.GetCurValue(WeaponGroup.Physic))));
            if (Ship.Params.Immune.GetCurValue(WeaponGroup.Irregular) > 0) Panels.Add(new GroupPanel(new SecondaryEffect(61, Ship.Params.Immune.GetCurValue(WeaponGroup.Irregular))));
            if (Ship.Params.Immune.GetCurValue(WeaponGroup.Cyber) > 0) Panels.Add(new GroupPanel(new SecondaryEffect(62, Ship.Params.Immune.GetCurValue(WeaponGroup.Cyber))));
            foreach (SecondaryEffect effect in Ship.GroupEffects)
                Panels.Add(new GroupPanel(effect));
            
            if (Ship.Params.BasicParams!=null && Ship.Params.BasicParams.Cargo>0) Panels.Add(new NotBattlePanel(57, Ship.Params.BasicParams.Cargo));
            if (Ship.Params.BasicParams != null && Ship.Params.BasicParams.Colony > 0) Panels.Add(new NotBattlePanel(58, Ship.Params.BasicParams.Colony));
            int height = 70;
            bool isright = false;
            foreach (AddPanel panel in Panels)
            {
                if (height > 450)
                { isright = true; height = 60; }
                Children.Add(panel.Panel);
                Canvas.SetTop(panel.Panel, height);
                if (isright)
                {
                    panel.Reverse();
                    Canvas.SetLeft(panel.Panel, -panel.Width);
                }
                else
                {
                    Canvas.SetLeft(panel.Panel, 975);
                    
                }
                height += panel.Height+5;
            }
            for (int i=0;i<3;i++)
            {
                if (Weapons[i] == null) continue;
                Weapons[i].HealthDamage.Text = Ship.Weapons[i].ToHealth().ToString();
                Weapons[i].ShieldDamage.Text = Ship.Weapons[i].ToShield().ToString();
                Weapons[i].Accuracy.Text = Ship.Weapons[i].Accuracy().ToString();
                Weapons[i].EnergySpent.Text = Ship.Weapons[i].Consume.ToString();
            }
        }
        public TextBlock AddText(string text, int left, int top, int size, int width)
        {
            TextBlock tb = Common.GetBlock(size, text, Brushes.White, width);
            Children.Add(tb);
            Canvas.SetLeft(tb, left);
            Canvas.SetTop(tb, top);
            return tb;
        }
        abstract class AddPanel
        {
            public Canvas Panel;
            public int Height;
            public int Width;
            internal Rectangle Back;
            public abstract void Reverse();
        }
        /*class ImmunePanel:AddPanel
        {
            public ImmunePanel(Brush brush, string tag)
            {
                Panel = new Canvas();
                Height = 80;
                Width = 93;
                Panel.Width = 93; Panel.Height = 80;
                Back = Common.GetRectangle(93, Links.Brushes.Interface.AddPanelSmall); Back.Height = 80;
                Panel.Children.Add(Back);
                Rectangle rect = Common.GetRectangle(50, brush, tag);
                Panel.Children.Add(rect);
                Canvas.SetLeft(rect, 5);
                Canvas.SetTop(rect, 10);
            }
            public override void Reverse()
            {
                Back.RenderTransformOrigin = new Point(0.5, 0.5);
                ScaleTransform scale = new ScaleTransform(-1, 1);
                Back.RenderTransform = scale;
            }
        }*/
        class GroupPanel:AddPanel
        {
            UIElement rect;
            TextBlock block;
            public GroupPanel(SecondaryEffect effect)
            {
                Panel = new Canvas();
                Height = 110;
                Width = 116;
                Panel.Width = 116; Panel.Height = 110;
                Back = Common.GetRectangle(116, Links.Brushes.Interface.AddPanel); Back.Height = 110;
                Panel.Children.Add(Back);
                rect = Common.GetRectangle(50, effect.Brush, Links.Interface(effect.Tip));
                //rect = Pictogram.GetPict(effect.Pict, effect.Target, true, Links.Interface(effect.Tip), 50);
                Panel.Children.Add(rect);
                Canvas.SetLeft(rect, 25);
                Canvas.SetTop(rect, 15);
                block = Common.GetBlock(30, effect.Value.ToString(), Brushes.Black, 60);
                Panel.Children.Add(block);
                Canvas.SetLeft(block, 10);
                Canvas.SetTop(block, 60);
            }
            public override void Reverse()
            {
                Back.RenderTransformOrigin = new Point(0.5, 0.5);
                ScaleTransform scale = new ScaleTransform(-1, 1);
                Back.RenderTransform = scale;
                Canvas.SetLeft(rect, 55);
                Canvas.SetLeft(block, 45);
            }
        }
        class NotBattlePanel:AddPanel
        {
            UIElement rect;
            TextBlock block;
            public NotBattlePanel(byte element, int value)
            {
                Panel = new Canvas();
                Height = 110;
                Width = 116;
                Panel.Width = 116; Panel.Height = 110;
                Back = Common.GetRectangle(116, Links.Brushes.Interface.AddPanel); Back.Height = 110;
                Panel.Children.Add(Back);
                if (element == 57)
                {
                    rect = Common.GetRectangle(50, Pictogram.CargoBrush, Links.Interface("Cargo"));
                    block = Common.GetBlock(30, (value/10.0).ToString("#.0")+"%", Brushes.Black, 60);
                }
                else
                {
                    rect = Common.GetRectangle(50, Pictogram.ColonyBrush, Links.Interface("Colony"));
                    block = Common.GetBlock(30, value.ToString(), Brushes.Black, 60);
                }
                Panel.Children.Add(rect);
                Canvas.SetLeft(rect, 25);
                Canvas.SetTop(rect, 5);
                Panel.Children.Add(block);
                Canvas.SetLeft(block, 10);
                Canvas.SetTop(block, 60);
            }
            public override void Reverse()
            {
                Back.RenderTransformOrigin = new Point(0.5, 0.5);
                ScaleTransform scale = new ScaleTransform(-1, 1);
                Back.RenderTransform = scale;
                Canvas.SetLeft(rect, 55);
                Canvas.SetLeft(block, 45);
            }
        }
        class ProtectParams
        {
            public TextBlock Energy;
            public TextBlock Physic;
            public TextBlock Irregular;
            public TextBlock Cyber;
            public ProtectParams(int left, Brush br1, Brush br2, Brush br3, Brush br4, 
                string tip1, string tip2, string tip3, string tip4, Canvas canvas)
            {
                canvas.Children.Add(AddRect(left, 107, br1, tip1));
                canvas.Children.Add(Energy = AddText(left + 30, 107));
                canvas.Children.Add(AddRect(left, 170, br2, tip2));
                canvas.Children.Add(Physic = AddText(left + 30, 170));
                canvas.Children.Add(AddRect(left, 235, br3, tip3));
                canvas.Children.Add(Irregular = AddText(left + 30, 235));
                canvas.Children.Add(AddRect(left, 297, br4, tip4));
                canvas.Children.Add(Cyber = AddText(left + 30, 297));
            }
            public void Update(int energy, int physic, int irregular, int cyber)
            {
                Energy.Text = energy.ToString();
                Physic.Text = physic.ToString();
                Irregular.Text = irregular.ToString();
                Cyber.Text = cyber.ToString();
            }
            Rectangle AddRect(int left, int top, Brush brush, string tip)
            {
                Rectangle rect = Common.GetRectangle(38, brush);
                Canvas.SetLeft(rect, left); Canvas.SetTop(rect, top);
                rect.ToolTip = tip;
                return rect;
            }
            TextBlock AddText(int left, int top)
            {
                TextBlock tb = Common.GetBlock(30, "", Brushes.White, 85);
                Canvas.SetLeft(tb, left);
                Canvas.SetTop(tb, top);
                return tb;
            }
        }
        class MainParams
        {
            public TextBlock Parametr;
            public TextBlock Restore;
            public MainParams(string title, Brush image, int top, Canvas canvas)
            {
                Rectangle rect = Common.GetRectangle(63, image);
                canvas.Children.Add(rect); Canvas.SetLeft(rect, 70); Canvas.SetTop(rect, top);
                canvas.Children.Add(AddText(title, 97, top - 47, 24, 152));
                canvas.Children.Add(Parametr = AddText("500/100", 140, top - 1, 26, 160));
                canvas.Children.Add(Restore = AddText("+600 / ход", 140, top + 31, 24, 160));
            }
            TextBlock AddText(string text, int left, int top, int size, int width)
            {
                TextBlock tb = Common.GetBlock(size, text, Brushes.White, width);
                Canvas.SetLeft(tb, left);
                Canvas.SetTop(tb, top);
                return tb;
            }
            public void Update(int current, int max, int restore)
            {
                Parametr.Text = String.Format("{0}/{1}", current, max);
                Restore.Text = String.Format("{0} / {1}", restore, Links.Interface("round"));
            }
        }
        class WeaponPanel
        {
            public TextBlock HealthDamage;
            public TextBlock ShieldDamage;
            public TextBlock EnergySpent;
            public TextBlock Accuracy;
            static LinearGradientBrush CritBrush = GetCritBrush();
            public WeaponPanel(BattleWeapon weapon, Canvas canvas, int pos)
            {
                Rectangle image = Common.GetRectangle(60, Links.Brushes.WeaponsBrushes[weapon.Type]);
                canvas.Children.Add(image);
                Canvas.SetLeft(image, 162 + pos * 227);
                Canvas.SetTop(image, 492);
                Rectangle healthdamage = Common.GetRectangle(30, Pictogram.HealthDamage, Links.Interface("HealthDamage"));
                canvas.Children.Add(healthdamage);
                Canvas.SetLeft(healthdamage, 230 + pos * 227);
                Canvas.SetTop(healthdamage, 486);
                Rectangle shielddamage = Common.GetRectangle(30, Pictogram.ShieldDamage, Links.Interface("ShieldDamage"));
                canvas.Children.Add(shielddamage);
                Canvas.SetLeft(shielddamage, 340 + pos * 227);
                Canvas.SetTop(shielddamage, 486);
                Rectangle energyspent = Common.GetRectangle(30, Pictogram.EnergyBrush, Links.Interface("EnergyConsume"));
                canvas.Children.Add(energyspent);
                Canvas.SetLeft(energyspent, 230 + pos * 227);
                Canvas.SetTop(energyspent, 516);
                Rectangle accuracy;
                switch (weapon.Group)
                {
                    case WeaponGroup.Energy: accuracy = Common.GetRectangle(30, Pictogram.AccuracyEnergy, Links.Interface("EnAcc")); break;
                    case WeaponGroup.Physic: accuracy = Common.GetRectangle(30, Pictogram.AccuracyPhysic, Links.Interface("PhAcc")); break;
                    case WeaponGroup.Irregular: accuracy = Common.GetRectangle(30, Pictogram.AccuracyIrregular, Links.Interface("IrAcc")); break;
                    default: accuracy = Common.GetRectangle(30, Pictogram.AccuracyCyber, Links.Interface("CyAcc")); break;
                }
                canvas.Children.Add(accuracy);
                Canvas.SetLeft(accuracy, 340 + pos * 227);
                Canvas.SetTop(accuracy, 516);
                canvas.Children.Add(HealthDamage = AddText("100", 256 + pos * 227, 492, 16, 35));
                canvas.Children.Add(ShieldDamage = AddText("200", 305 + pos * 227, 492, 16, 35));
                canvas.Children.Add(EnergySpent = AddText("50", 256 + pos * 227, 522, 16, 35));
                canvas.Children.Add(Accuracy = AddText("150", 305 + pos * 227, 522, 16, 35));

                Rectangle critrect = new Rectangle(); critrect.Width = 140; critrect.Height = 10;
                canvas.Children.Add(critrect); Canvas.SetLeft(critrect, 230+pos*227); Canvas.SetTop(critrect, 548);
                critrect.Fill = CritBrush;
                LinearGradientBrush CritOppBrush = new LinearGradientBrush(Colors.White, Colors.Transparent, new Point(0, 0.5), new Point(1, 0.5));
                critrect.OpacityMask = CritOppBrush;
                int percent = Links.Modifiers[weapon.Type].Crit[weapon.Size];
                CritOppBrush.GradientStops[0].Offset = percent / 100.0;
                CritOppBrush.GradientStops[1].Offset = percent / 100.0;
                critrect.ToolTip = String.Format("{0} = {1}%", Links.Interface("CritChance"), percent);
                Rectangle critrectborder = new Rectangle(); critrectborder.Width = 140; critrectborder.Height = 10;
                canvas.Children.Add(critrectborder); Canvas.SetLeft(critrectborder, 230 + pos * 227); Canvas.SetTop(critrectborder, 548);
                critrectborder.Stroke = Brushes.SkyBlue; 
            }
            static LinearGradientBrush GetCritBrush()
            {
                LinearGradientBrush brush = new LinearGradientBrush();
                brush.StartPoint = new Point(0, 0.5); brush.EndPoint = new Point(1, 0.5);
                brush.GradientStops.Add(new GradientStop(Colors.Red, 0));
                brush.GradientStops.Add(new GradientStop(Colors.Orange, 0.17));
                brush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.33));
                brush.GradientStops.Add(new GradientStop(Colors.Green, 0.5));
                brush.GradientStops.Add(new GradientStop(Colors.SkyBlue, 0.67));
                brush.GradientStops.Add(new GradientStop(Colors.Blue, 0.83));
                brush.GradientStops.Add(new GradientStop(Colors.Purple, 1.0));
                return brush;
            }
            TextBlock AddText(string text, int left, int top, int size, int width)
            {
                TextBlock tb = Common.GetBlock(size, text, Brushes.White, width);
                Canvas.SetLeft(tb, left);
                Canvas.SetTop(tb, top);
                return tb;
            }
        }
    }
    class ShipPanelPopUp : Canvas
    {
        public static bool IsOpened = false;
        public ShipPanelPopUp()
        {
            Width = 1920;
            Height = 1080;
            Canvas.SetZIndex(this, 210);
            Background = new SolidColorBrush(Color.FromArgb(100, 100, 100, 100));
        }

        void Place()
        {
            if (Links.Controller.mainWindow.MainVbx.Child == Links.Controller.MainCanvas)
            {
                if (!Links.Controller.MainCanvas.Children.Contains(this))
                    Links.Controller.MainCanvas.Children.Add(this);
                IsOpened = true;
            }
            else
            {
                if (!Links.Controller.IntBoya.Children.Contains(this))
                    Links.Controller.IntBoya.Children.Add(this);
                IsOpened = true;
            }
        }
        public void Place(Schema schema)
        {
            Place();
            ShipB Ship = new ShipB(10, schema, 100, null, new byte[4], ShipSide.Attack, null, true, null, ShipBMode.Battle, 255);
            PanelB2 panel = new PanelB2(Ship);
            Children.Add(panel);
            Canvas.SetLeft(panel, (Width - panel.Width) / 2 + 50);
            Canvas.SetTop(panel, (Height - panel.Height) / 2 + 50);
        }
        public void Place(ShipB Ship)
        {
            Place();
            PanelB2 panel=new PanelB2(Ship);
            Children.Add(panel);
            Canvas.SetLeft(panel, (Width - panel.Width) / 2+50);
            Canvas.SetTop(panel, (Height - panel.Height) / 2+50);
        }
        public void Remove()
        {
            Children.Clear();
            IsOpened = false;
            if (Links.Controller.MainCanvas!=null && Links.Controller.MainCanvas.Children.Contains(this))
                Links.Controller.MainCanvas.Children.Remove(this);
            if (Links.Controller.IntBoya.Children.Contains(this))
                Links.Controller.IntBoya.Children.Remove(this);
        }
    }
}
