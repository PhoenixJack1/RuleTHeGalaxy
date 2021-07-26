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
    public class Shieldclass : Item, ISort, IComparable

    {
        public int Capacity;
        public int Recharge;
        public byte Level;
        public bool IsRare;
        public static SortedList<ushort, Shieldclass> GetList(string[] array)
        {
            SortedList<ushort, Shieldclass> list = new SortedList<ushort, Shieldclass>();
            for (int i = 0; i < array.Length;i++)
            {
                string[] ss = array[i].Split('\t');
                int z = 0;
                ushort id = ushort.Parse(ss[z]); z++;
                ItemSize size = (ItemSize)int.Parse(ss[z]); z++;
                int capacity = int.Parse(ss[z]); z++;
                int recharge = int.Parse(ss[z]); z++;
                int consume = int.Parse(ss[z]); z++;
                byte level = byte.Parse(ss[z]); z++;
                bool israre = bool.Parse(ss[z]); z++;
                int money = int.Parse(ss[z]); z++;
                int metall = int.Parse(ss[z]); z++;
                int chips = int.Parse(ss[z]); z++;
                int anti = int.Parse(ss[z]); z++;
                ItemPrice price = new ItemPrice(money, metall, chips, anti);
                Shieldclass shield = new Shieldclass(id, "", size, consume, capacity, recharge, price);
                shield.Level = level; shield.IsRare = israre;
                list.Add(id, shield);
            }
            /*List<string> sl = new List<string>();
            foreach (Shieldclass shield in list.Values)
            {
                string s = "";
                s += shield.ID.ToString() + "\t";
                s += ((int)shield.Size).ToString() + "\t";
                s += shield.Capacity.ToString() + "\t";
                s += shield.Recharge.ToString() + "\t";
                s += shield.Consume.ToString() + "\t";
                s += shield.Level.ToString() + "\t";
                s += shield.IsRare.ToString() + "\t";
                s += shield.Price.Money.ToString() + "\t";
                s += shield.Price.Metall.ToString() + "\t";
                s += shield.Price.Chips.ToString() + "\t";
                s += shield.Price.Anti.ToString();
                sl.Add(s);
            }
            System.IO.File.WriteAllLines("Shield.txt", sl.ToArray(), Encoding.GetEncoding(1251));
    */        
    return list;
        }
        public Shieldclass(ushort id, string name, ItemSize size, int consume, int capacity, int recharge, ItemPrice price)
            : base(id, name, size, consume, price)
        {
            Capacity = capacity;
            Recharge = recharge;
        }
        public int GetSizeDescription()
        {
            int type = ((ID - Level * 1000) / 265) == 1 ? 0 : 5;
            return (int)Size * 10 + type;
        }
        public void SetStats(ShipParamsClass schema)
        {
            schema.BasicShieldCapacity += Capacity;
            schema.BasicShieldRecharge += Recharge;
            schema.BasicEnergySpent += Consume;
        }
        public void SetPrice(ItemPrice price)
        {
            price.Add(Price);
        }

        public static UIElement[] GetShieldTypeColumnNames()
        {
            List<UIElement> list = new List<UIElement>();
            list.Add(Common.GetRectangle(30, Pictogram.ShieldBrush, Links.Interface("Shield")));
            list.Add(Common.GetRectangle(30, Pictogram.RestoreShield, Links.Interface("ShieldRestore")));
            list.Add(Common.GetRectangle(30, Pictogram.EnergyBrush, Links.Interface("EnergyConsume")));
            list.Add(Common.GetBlock(20, Links.Interface("Size")));
            list.Add(Common.GetRectangle(30, Links.Brushes.MoneyImageBrush));
            list.Add(Common.GetRectangle(30, Links.Brushes.MetalImageBrush));
            list.Add(Common.GetRectangle(30, Links.Brushes.ChipsImageBrush));
            list.Add(Common.GetRectangle(30, Links.Brushes.AntiImageBrush));
            return list.ToArray();

        }
        public int GetParam(int pos)
        {
            switch (pos)
            {
                case 0: return Capacity;
                case 1: return Recharge;
                case 2: return Consume;
                case 3: return (int)Size;
                case 4: return Price.Money;
                case 5: return Price.Metall;
                case 6: return Price.Chips;
                case 7: return Price.Anti;
                default: return 0;
            }
        }
        public string GetName()
        {
            return GameObjectName.GetShieldName(this);
        }
        public int GetID()
        {
            return ID;
        }
        public UIElement GetElement(int pos)
        {
            switch (pos)
            {
                case 3: return Common.GetSizeRect(15, Size, false);
                default: return null;
            }

        }
        public static DrawingBrush[] ProtectBrushes = new DrawingBrush[] { GetSmallShieldBrush(), GetMediumShieldBrush(), GetLargeShieldBrush() };
        public static DrawingBrush GetSmallShieldBrush()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.SkyBlue, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M5.1,0.5 a4.5,4.5 0 1,1 -0.1,0"))));
            group.Children.Add(new GeometryDrawing(Brushes.Blue, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M5,5 l3.0,-3.2 a4.5,4.5 0 0,0 -6.0,0z"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetMediumShieldBrush()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.SkyBlue, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M5.1,0.5 a4.5,4.5 0 1,1 -0.1,0"))));
            group.Children.Add(new GeometryDrawing(Brushes.Blue, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M5,5 h4.5 a4.5,4.5 0 0,0 -9,0z"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetLargeShieldBrush()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.SkyBlue, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M5.1,0.5 a4.5,4.5 0 1,1 -0.1,0"))));
            group.Children.Add(new GeometryDrawing(Brushes.Blue, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M5,5 l3.0,3.2 a4.5,4.5 0 1,0 -6.0,0z"))));
            return new DrawingBrush(group);
        }
        public int CompareTo(object b)
        {
            Shieldclass B = (Shieldclass)b;
            if (ID > B.ID) return 1;
            else if (ID < B.ID) return -1;
            else return 0;
        }
    }
}
