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
    public class Computerclass : Item, ISort, IComparable
    {
        public int[] Accuracy = new int[4];
        public int[] Damage = new int[4];
        public byte Level;
        public bool IsRare;
        public WeaponGroup group;
        public int MaxAccuracy;
        public int MaxDamage;
        public static SortedList<ushort, Computerclass> GetList(string[] array)
        {
            SortedList<ushort, Computerclass> list = new SortedList<ushort, Computerclass>();
            for (int i = 0; i < array.Length; i++)
            {
                string[] ss = array[i].Split('\t');
                int z = 0;
                ushort id = ushort.Parse(ss[z]); z++;
                ItemSize size = (ItemSize)int.Parse(ss[z]); z++;
                int[] accuracy = new int[4];
                for (int j = 0; j < 4; j++)
                { accuracy[j] = int.Parse(ss[z]); z++; }
                int[] damage = new int[4];
                for (int j = 0; j < 4; j++)
                { damage[j] = int.Parse(ss[z]); z++; }
                int consume = int.Parse(ss[z]); z++;
                byte level = byte.Parse(ss[z]); z++;
                bool israre = bool.Parse(ss[z]); z++;
                int money = int.Parse(ss[z]); z++;
                int metall= int.Parse(ss[z]); z++;
                int chips= int.Parse(ss[z]); z++;
                int anti= int.Parse(ss[z]); z++;
                ItemPrice price = new ItemPrice(money, metall, chips, anti);
                Computerclass computer = new Computerclass(id, "", size, consume, accuracy, damage, price);
                computer.Level = level; computer.IsRare = israre;
                list.Add(id, computer);
            }
            /*List<string> sl = new List<string>();
            foreach (Computerclass comp in list.Values)
            {
                string s = "";
                s += comp.ID.ToString() + "\t";
                s += ((int)comp.Size).ToString() + "\t";
                for (int i=0;i<4;i++)
                    s += comp.Accuracy[i].ToString() + "\t";
                for (int i=0;i<4;i++)
                    s += comp.Damage[i].ToString() + "\t";
                s += comp.Consume.ToString() + "\t";
                s += comp.Level.ToString() + "\t";
                s += comp.IsRare.ToString() + "\t";
                s += comp.Price.Money.ToString() + "\t";
                s += comp.Price.Metall.ToString() + "\t";
                s += comp.Price.Chips.ToString() + "\t";
                s += comp.Price.Anti.ToString();
                sl.Add(s);
            }
            System.IO.File.WriteAllLines("Computers.txt", sl.ToArray(), Encoding.GetEncoding(1251));*/
            return list;
        }
        public Computerclass(ushort id, string name, ItemSize size, int consume, int[] accuracy, int[] damage, ItemPrice price)
            : base(id, name, size, consume, price)
        {
            Accuracy = accuracy; Damage = damage;
            int type = id % 10;
            switch (type)
            {
                case 0:
                    group = WeaponGroup.Any;
                    MaxAccuracy = Accuracy[0];
                    MaxDamage = 0;
                    break;
                case 1:
                    group = WeaponGroup.Energy;
                    MaxAccuracy = Accuracy[0];
                    MaxDamage = Damage[0];
                    break;
                case 2:
                    group = WeaponGroup.Physic;
                    MaxAccuracy = Accuracy[1];
                    MaxDamage = Damage[1];
                    break;
                case 3:
                    group = WeaponGroup.Irregular;
                    MaxAccuracy = Accuracy[2];
                    MaxDamage = Damage[2];
                    break;
                case 4:
                    group = WeaponGroup.Cyber;
                    MaxAccuracy = Accuracy[3];
                    MaxDamage = Damage[3];
                    break;
                case 5:
                    group = WeaponGroup.Energy;
                    MaxAccuracy = Accuracy[0];
                    MaxDamage = Damage[0];
                    break;
                case 6:
                    group = WeaponGroup.Physic;
                    MaxAccuracy = Accuracy[1];
                    MaxDamage = Damage[1];
                    break;
                case 7:
                    group = WeaponGroup.Irregular;
                    MaxAccuracy = Accuracy[2];
                    MaxDamage = Damage[2];
                    break;
                case 8:
                    group = WeaponGroup.Cyber;
                    MaxAccuracy = Accuracy[3];
                    MaxDamage = Damage[3];
                    break;
            }
        }
        /*
        public int GetSizeDescription()
        {
            int type = ((ID - Level * 1000) / 285) == 1 ? 0 : 5;
            return (int)Size * 10 + type;
        }
        */
        public void SetStats(ShipParamsClass schema)
        {
            schema.BasicEnergySpent += Consume;
            schema.BasicWEnergyAccuracy += Accuracy[0];
            schema.BasicWPhysicAccuracy += Accuracy[1];
            schema.BasicWIrregularAccuracy += Accuracy[2];
            schema.BasicWCyberAccuracy += Accuracy[3];
            schema.BasicWEnergyIncrease += Damage[0];
            schema.BasicWPhysicIncrease += Damage[1];
            schema.BasicWIrregularIncrease += Damage[2];
            schema.BasicWCyberIncrease += Damage[3];
        }
        public void SetPrice(ItemPrice price)
        {
            price.Add(Price);
        }
       
        public static UIElement[] GetComputerTypeColumnNames()
        {
            List<UIElement> list = new List<UIElement>();
            list.Add(Common.GetBlock(20, Links.Interface("WeaponGroup")));
            list.Add(Common.GetRectangle(30, Pictogram.Accuracy, Links.Interface("AccuracyBonus")));
            list.Add(Common.GetRectangle(30, Pictogram.Damage, Links.Interface("DamageBonus")));
            //list.Add(Pictogram.GetPict(Picts.Accuracy, Target.Energy, false, Links.Interface("EnAcc")));
            //list.Add(Pictogram.GetPict(Picts.Accuracy, Target.Physic, false, Links.Interface("PhAcc")));
            //list.Add(Pictogram.GetPict(Picts.Accuracy, Target.Irregular, false, Links.Interface("IrAcc")));
            //list.Add(Pictogram.GetPict(Picts.Accuracy, Target.Cyber, false, Links.Interface("CyAcc")));
            list.Add(Common.GetRectangle(30, Pictogram.EnergyBrush, Links.Interface("EnergyConsume")));
            list.Add(Common.GetBlock(20, Links.Interface("Size")));
            list.Add(Common.GetRectangle(30, Links.Brushes.MoneyImageBrush));
            list.Add(Common.GetRectangle(30, Links.Brushes.MetalImageBrush));
            list.Add(Common.GetRectangle(30, Links.Brushes.ChipsImageBrush));
            list.Add(Common.GetRectangle(30, Links.Brushes.AntiImageBrush));
            return list.ToArray();
            
        }
        new public int GetType()
        {
            return ID - Level * 1000 - 280;
        }
        public int GetAccuracy()
        {
            int type = GetType();
            switch (type)
            {
                case 0:
                case 1:
                case 5: return Accuracy[0];
                case 2:
                case 6: return Accuracy[1];
                case 3:
                case 7: return Accuracy[2];
                case 4:
                case 8: return Accuracy[3];
            }
            return 0;
        }
        public int GetDamage()
        {
            int type = GetType();
            switch (type)
            {
                case 1:
                case 5: return Damage[0];
                case 2:
                case 6: return Damage[1];
                case 3:
                case 7: return Damage[2];
                case 4:
                case 8: return Damage[3];
                default: return 0;
            }
        }
        new public Rectangle GetWeaponGroup()
        {
            int type = GetType();
            switch (type)
            {
                case 0: return Common.GetRectangle(30, Links.Brushes.WeaponGroupBrush[WeaponGroup.Any]);
                case 1:
                case 5: return Common.GetRectangle(30, Links.Brushes.WeaponGroupBrush[WeaponGroup.Energy]);
                case 2:
                case 6: return Common.GetRectangle(30, Links.Brushes.WeaponGroupBrush[WeaponGroup.Physic]);
                case 3:
                case 7: return Common.GetRectangle(30, Links.Brushes.WeaponGroupBrush[WeaponGroup.Irregular]);
                case 4:
                case 8: return Common.GetRectangle(30, Links.Brushes.WeaponGroupBrush[WeaponGroup.Cyber]);
            }
            return null;
        }
        public int GetWeaponGroupValue()
        {
            switch (GetType())
            {
                case 1:
                case 5: return 0;
                case 2:
                case 6: return 1;
                case 3:
                case 7: return 2;
                case 4:
                case 8: return 3;
                default: return 4;
            }
        }
        public int GetParam(int pos)
        {
            switch (pos)
            {
                case 0: return GetWeaponGroupValue();
                case 1: return GetAccuracy();
                case 2: return GetDamage();
                case 3: return Consume;
                case 4: return (int)Size;
                case 5: return Price.Money;
                case 6: return Price.Metall;
                case 7: return Price.Chips;
                case 8: return Price.Anti;
                default: return 0;
            }
        }
        public string GetName()
        {
            return GameObjectName.GetComputerName(this);
        }
        public int GetID()
        {
            return ID;
        }
        public UIElement GetElement(int pos)
        {
            switch (pos)
            {
                case 0: return GetWeaponGroup();
                case 4: return Common.GetSizeRect(15, Size, false);
                default: return null;
            }

        }
        static RadialGradientBrush WarpBrush = GetWarpBrush();
        static RadialGradientBrush GetWarpBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.3));
            brush.GradientStops.Add(new GradientStop(Colors.Violet, 0.4));
            brush.GradientStops.Add(new GradientStop(Colors.Purple, 0.7));
            brush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));
            return brush;
        }
        public static DrawingBrush[] JumpRangeBrushes = new DrawingBrush[] { GetFirstLineJumpBrush(), GetSecondLineJumpBrush(), GetThirdLineJumpBrush() };
        static DrawingBrush GetFirstLineJumpBrush()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(WarpBrush, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M5,0 a5,10 0 1,1 -0.1,0"))));
            group.Children.Add(new GeometryDrawing(Brushes.Purple, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M5,8.5 l10,1 l-10,1z"))));
            group.Children.Add(new GeometryDrawing(Brushes.Purple, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
               "M10,7 l5,2.5 l-5,2.5 a5,5 0 0,0 0,-5"))));
            return new DrawingBrush(group);
        }
        static DrawingBrush GetSecondLineJumpBrush()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(WarpBrush, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M5,0 a5,10 0 1,1 -0.1,0"))));
            group.Children.Add(new GeometryDrawing(Brushes.Purple, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M5,4.5 l10,1 l-10,1z"))));
            group.Children.Add(new GeometryDrawing(Brushes.Purple, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
               "M10,3 l5,2.5 l-5,2.5 a5,5 0 0,0 0,-5"))));

            group.Children.Add(new GeometryDrawing(Brushes.Purple, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
    "M5,12.5 l10,1 l-10,1z"))));
            group.Children.Add(new GeometryDrawing(Brushes.Purple, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
               "M10,11 l5,2.5 l-5,2.5 a5,5 0 0,0 0,-5"))));
            return new DrawingBrush(group);
        }
        static DrawingBrush GetThirdLineJumpBrush()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(WarpBrush, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M5,0 a5,10 0 1,1 -0.1,0"))));
            group.Children.Add(new GeometryDrawing(Brushes.Purple, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M5,3.5 l10,1 l-10,1z"))));
            group.Children.Add(new GeometryDrawing(Brushes.Purple, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
               "M10,2 l5,2.5 l-5,2.5 a5,5 0 0,0 0,-5"))));

            group.Children.Add(new GeometryDrawing(Brushes.Purple, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
    "M5,8.5 l10,1 l-10,1z"))));
            group.Children.Add(new GeometryDrawing(Brushes.Purple, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
               "M10,7 l5,2.5 l-5,2.5 a5,5 0 0,0 0,-5"))));

            group.Children.Add(new GeometryDrawing(Brushes.Purple, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
    "M5,13.5 l10,1 l-10,1z"))));
            group.Children.Add(new GeometryDrawing(Brushes.Purple, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
               "M10,12 l5,2.5 l-5,2.5 a5,5 0 0,0 0,-5"))));
            return new DrawingBrush(group);
        }
        public int CompareTo(object b)
        {
            Computerclass B = (Computerclass)b;
            if (ID > B.ID) return 1;
            else if (ID < B.ID) return -1;
            else return 0;
        }
    }
}
