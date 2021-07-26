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
    public class Engineclass : Item, ISort, IComparable
    {
        public int EnergyEvasion;
        public int PhysicEvasion;
        public int IrregularEvasion;
        public int CyberEvasion;
        public int AverageEvasion;
        public byte Level;
        public bool IsRare;
        public static SortedList<ushort, Engineclass> GetList(string[] array)
        {
            SortedList<ushort, Engineclass> list = new SortedList<ushort, Engineclass>();
            for (int i = 0; i < array.Length; i++)
            {
                string[] ss = array[i].Split('\t');
                int z = 0;
                ushort id = ushort.Parse(ss[z]); z++;
                ItemSize size = (ItemSize)int.Parse(ss[z]); z++;
                int eneva = int.Parse(ss[z]); z++;
                int pheva = int.Parse(ss[z]); z++;
                int irreva = int.Parse(ss[z]); z++;
                int cybeva = int.Parse(ss[z]); z++;
                int consume = int.Parse(ss[z]); z++;
                byte level = byte.Parse(ss[z]); z++;
                bool israre = bool.Parse(ss[z]); z++;
                int money= int.Parse(ss[z]); z++;
                int metal= int.Parse(ss[z]); z++;
                int chips= int.Parse(ss[z]); z++;
                int anti= int.Parse(ss[z]); z++;
                ItemPrice price = new ItemPrice(money, metal, chips, anti);
                Engineclass engine = new Engineclass(id, "", size, consume, eneva, pheva, irreva, cybeva, price);
                engine.Level = level; engine.IsRare = israre;
                list.Add(id, engine);
            }
           /* List<string> sl = new List<string>();
            foreach (Engineclass engine in list.Values)
            {
                string s = "";
                s += engine.ID.ToString() + "\t";
                s += ((int)engine.Size).ToString() + "\t";
                s += engine.EnergyEvasion.ToString() + "\t";
                s += engine.PhysicEvasion.ToString() + "\t";
                s += engine.IrregularEvasion.ToString() + "\t";
                s += engine.CyberEvasion.ToString() + "\t";
                s += engine.Consume.ToString() + "\t";
                s += engine.Level.ToString() + "\t";
                s += engine.IsRare.ToString() + "\t";
                s += engine.Price.Money.ToString() + "\t";
                s += engine.Price.Metall.ToString() + "\t";
                s += engine.Price.Chips.ToString() + "\t";
                s += engine.Price.Anti.ToString();
                sl.Add(s);
            }
            System.IO.File.WriteAllLines("Engines.txt", sl.ToArray(), Encoding.GetEncoding(1251)); */
            return list;
        }
        public int GetHexMoveSpent
        { get { if (Size == ItemSize.Small) return 60; else if (Size == ItemSize.Medium) return 40; else return 20; } }

        public Engineclass(ushort id, string name, ItemSize size, int consume, int energyevasion, int physicevasion, int irregularEvasion, int cyberEvasion, ItemPrice price)
            : base(id, name, size, consume, price)
        {
            EnergyEvasion = energyevasion; PhysicEvasion = physicevasion; IrregularEvasion = irregularEvasion; CyberEvasion = cyberEvasion;
            AverageEvasion = (EnergyEvasion + PhysicEvasion + IrregularEvasion + CyberEvasion) / 4;
        }
        public int GetSizeDescription()
        {
            int type = ((ID - Level * 1000) / 275) == 1 ? 0 : 5;
            return (int)Size * 10 + type;
        }
        public void SetStats(ShipParamsClass schema)
        {
            schema.BasicEnergySpent += Consume;
            schema.BasicWEnergyEvasion += EnergyEvasion;
            schema.BasicWPhysicEvasion += PhysicEvasion;
            schema.BasicWIrregularEvasion += IrregularEvasion;
            schema.BasicWCyberEvasion += CyberEvasion;
        }
        public void SetPrice(ItemPrice price)
        {
            price.Add(Price);
        }
        
        public static UIElement[] GetEngineTypeColumnNames()
        {
            List<UIElement> list = new List<UIElement>();
            list.Add(Common.GetRectangle(30, Pictogram.EvasionEnergy, Links.Interface("EnEva")));
            list.Add(Common.GetRectangle(30, Pictogram.EvasionPhysic, Links.Interface("PhEva")));
            list.Add(Common.GetRectangle(30, Pictogram.EvasionIrregular, Links.Interface("IrEva")));
            list.Add(Common.GetRectangle(30, Pictogram.EvasionCyber, Links.Interface("CyEva")));
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
                case 0: return EnergyEvasion;
                case 1: return PhysicEvasion;
                case 2: return IrregularEvasion;
                case 3: return CyberEvasion;
                case 4: return Consume;
                case 5: return (int)Size;
                case 6: return Price.Money;
                case 7: return Price.Metall;
                case 8: return Price.Chips;
                case 9: return Price.Anti;
                default: return 0;
            }
        }
        public string GetName()
        {
            return GameObjectName.GetEngineName(this);
        }
        public int GetID()
        {
            return ID;
        }
        public UIElement GetElement(int pos)
        {
            switch (pos)
            {
                case 5: return Common.GetSizeRect(15, Size, false);
                default: return null;
            }

        }
        public static DrawingBrush MoveCostBrush = GetMoveCostBrush();
        public static DrawingBrush GetMoveCostBrush()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.Black, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M0,5.2 l3,-5.2 l6,0 l3,5.2 l-3.2,5.2 l-6,0z"))));
            group.Children.Add(new GeometryDrawing(Brushes.Green, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M9,10.4 l3,-5.2 l6,0 l3,5.2 l-3.2,5.2 l-6,0z"))));
            group.Children.Add(new GeometryDrawing(null, new Pen(Brushes.White,1), new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M6,5.2 l9,5.2 M13,10.4 h2 l-1,-1.8"))));
            return new DrawingBrush(group);
        }
        public int CompareTo(object b)
        {
            Engineclass B = (Engineclass)b;
            if (ID > B.ID) return 1;
            else if (ID < B.ID) return -1;
            else return 0;
        }
    }
}
