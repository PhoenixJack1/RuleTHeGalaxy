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
    public class Generatorclass : Item, ISort, IComparable
    {
        public int Capacity;
        public int Recharge;
        public byte Level;
        public bool IsRare;
        public static SortedList<ushort, Generatorclass> GetList(string[] array)
        {
            SortedList<ushort, Generatorclass> list = new SortedList<ushort, Generatorclass>();
            foreach (string s in array)
            {
                string[] ss = s.Split('\t');
                int z = 0;
                ushort id = ushort.Parse(ss[z]); z++;
                ItemSize size = (ItemSize)int.Parse(ss[z]); z++;
                int capacity = int.Parse(ss[z]); z++;
                int recharge = int.Parse(ss[z]); z++;
                byte level = byte.Parse(ss[z]); z++;
                bool israre = bool.Parse(ss[z]); z++;
                int money = int.Parse(ss[z]); z++;
                int metall= int.Parse(ss[z]); z++; 
                int chips= int.Parse(ss[z]); z++;
                int anti= int.Parse(ss[z]); z++;
                ItemPrice price = new ItemPrice(money, metall, chips, anti);
                Generatorclass generator = new Generatorclass(id, "", size, capacity, recharge, price);
                generator.Level = level; generator.IsRare = israre;
                list.Add(id, generator);
            }
            /*for (int i = 0; i < array.Length; )
            {
                ushort id = BitConverter.ToUInt16(array, i); i += 2;
                ItemSize size = (ItemSize)array[i]; i++;
                int capacity = BitConverter.ToInt32(array, i); i += 4;
                int recharge = BitConverter.ToInt32(array, i); i += 4;
                byte level = array[i]; i++;
                bool israre = BitConverter.ToBoolean(array, i); i++;
                ItemPrice price = ItemPrice.GetPrice(array, i); i += 16;
                Generatorclass generator = new Generatorclass(id, "", size, capacity, recharge, price);
                generator.Level = level; generator.IsRare = israre;
                list.Add(id, generator);
            }
            List<string> sl = new List<string>();
            foreach (Generatorclass gen in list.Values)
            {
                string s = "";
                s += gen.ID.ToString() +"\t";
                s += ((int)gen.Size).ToString() + "\t";
                s += gen.Capacity.ToString() + "\t";
                s += gen.Recharge.ToString() + "\t";
                s += gen.Level.ToString() + "\t";
                s += gen.IsRare.ToString() + "\t";
                s += gen.Price.Money.ToString() + "\t";
                s += gen.Price.Metall.ToString() + "\t";
                s += gen.Price.Chips.ToString() + "\t";
                s += gen.Price.Anti.ToString();
                sl.Add(s);
            }
            System.IO.File.WriteAllLines("Generators.txt", sl.ToArray(), Encoding.GetEncoding(1251));
            */return list;
        }
        public Generatorclass(ushort id, string name, ItemSize size, int capacity, int recharge, ItemPrice price)
            : base(id, name, size, 0, price)
        {
            Recharge = recharge; Capacity = capacity;
        }
        public int GetSizeDescription()
        {
            int type = ((ID - Level * 1000) / 255) == 1 ? 0 : 5;
            return (int)Size * 10 + type;
        }
        

        public void SetStats(ShipParamsClass schema)
        {
            schema.BasicEnergyCapacity += Capacity;
            schema.BasicEnergyRecharge += Recharge;
        }
       
      
        public static UIElement[] GetGeneratorTypeColumnNames()
        {
            List<UIElement> list = new List<UIElement>();
            list.Add(Common.GetRectangle(30, Pictogram.EnergyBrush, Links.Interface("Energy")));
            list.Add(Common.GetRectangle(30, Pictogram.RestoreEnergy, Links.Interface("EnergyRestore")));
            list.Add(Common.GetBlock(20,Links.Interface("Size")));
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
                case 2: return (int)Size;
                case 3: return Price.Money;
                case 4: return Price.Metall;
                case 5: return Price.Chips;
                case 6: return Price.Anti;
                default: return 0;
            }
        }
        public string GetName()
        {
            return GameObjectName.GetGeneratorName(this);
        }
        public int GetID()
        {
            return ID;
        }
        public UIElement GetElement(int pos)
        {
            switch (pos)
            {
                case 2: return Common.GetSizeRect(15, Size, false);
                default: return null;
            }

        }
        public static DrawingBrush[] TurnsBrushes = new DrawingBrush[] { OneTurnBrush(), TwoTurnsBrush(), ThreeTurnsBrush() };
        static DrawingBrush OneTurnBrush()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.Green, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M8,8 v-8 a8,8 0 0,1 8,8z"))));
            group.Children.Add(new GeometryDrawing(null, new Pen (Brushes.White,1), new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M0.63,8 a7.4,7.4 0 1,1 0,0.01 M8,0 v3.75 M0,8 h3.75 M16,8 h-3.75 M8,16 v-3.75"))));
            return new DrawingBrush(group);
        }
        static DrawingBrush TwoTurnsBrush()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.Green, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M8,8 v-8 a8,8 0 0,1 0,16z"))));
            group.Children.Add(new GeometryDrawing(null, new Pen(Brushes.White, 1), new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M0.63,8 a7.4,7.4 0 1,1 0,0.01 M8,0 v3.75 M0,8 h3.75 M16,8 h-3.75 M8,16 v-3.75"))));
            return new DrawingBrush(group);
        }
        static DrawingBrush ThreeTurnsBrush()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.Green, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M8,8 v-8 a8,8 0 1,1 -8,8z"))));
            group.Children.Add(new GeometryDrawing(null, new Pen(Brushes.White, 1), new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M0.63,8 a7.4,7.4 0 1,1 0,0.01 M8,0 v3.75 M0,8 h3.75 M16,8 h-3.75 M8,16 v-3.75"))));
            return new DrawingBrush(group);
        }
        public int CompareTo (object b)
        {
            Generatorclass B = (Generatorclass)b;
            if (ID > B.ID) return 1;
            else if (ID < B.ID) return -1;
            return 0;
        }
    }
}
