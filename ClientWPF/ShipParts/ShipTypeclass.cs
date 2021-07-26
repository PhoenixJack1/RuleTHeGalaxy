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
    public class ShipTypeclass : ISort, IComparable
    {
        public ushort ID;
        public string Name;
        public int Health;
        public int Armor;
        public ItemSize ShieldSize;
        public ItemSize GeneratorSize;
        public ItemSize EngineSize;
        public ItemSize ComputerSize;
        public ShipGenerator2Types Type;

        public WeaponParams[] WeaponsParam;
        public int WeaponCapacity;
        public ItemSize[] EquipmentsSize;
        public int EquipmentCapacity;
        public ItemPrice Price;

        public string En;
        public string Ru;
        public byte Level;
        public bool IsRare;

        public ShipTypeclass(ushort id, string name, int health, int armor, ItemSize shieldSize, ItemSize generatorSize,
            ItemSize computerSize, ItemSize engineSize, WeaponParams[] weaponsparams, ItemSize[] equipmentsSize, ItemPrice price)
        {
            ID = id;
            Name = name; Health = health; ShieldSize = shieldSize; Armor = armor;
            GeneratorSize = generatorSize; ComputerSize = computerSize; EngineSize = engineSize;
            WeaponsParam = weaponsparams; WeaponCapacity = weaponsparams.Length;
            EquipmentsSize = equipmentsSize; EquipmentCapacity = equipmentsSize.Length;
            Price = price;
            int type = ID%1000;
            type = (type / 10) + (type % 10 >= 5 ? 100 : 0);
            switch (type)
            {
                case 20: Type = ShipGenerator2Types.Scout; break;
                case 120: Type = ShipGenerator2Types.Corvett; break;
                case 21: Type = ShipGenerator2Types.Cargo; break;
                case 121: Type = ShipGenerator2Types.Linkor; break;
                case 22: Type = ShipGenerator2Types.Frigate; break;
                case 122: Type = ShipGenerator2Types.Fighter; break;
                case 23: Type = ShipGenerator2Types.Dreadnought; break;
                case 123: Type = ShipGenerator2Types.Devostator; break;
                case 24: Type = ShipGenerator2Types.Warrior; break;
                case 124: Type = ShipGenerator2Types.Cruiser; break;
                default: throw new Exception();
            }
        }
        public static SortedList<ushort, ShipTypeclass> GetList(string[] array)
        {
            SortedList<ushort, ShipTypeclass> list = new SortedList<ushort, ShipTypeclass>();
            //List<string> l1 = new List<string>(System.IO.File.ReadAllLines("GameData/ShipTypes1.txt", Encoding.GetEncoding(1251)));
            foreach (string line in array)
            {
                int i = 0;
                string[] s = line.Split('\t');

                ushort id = ushort.Parse(s[i]); i++;
                GSString En = new GSString(s[i]); i++;
                GSString Ru = new GSString(s[i]); i++;
                int health = int.Parse(s[i]); i ++;
                int armor = int.Parse(s[i]); i++;
                ItemSize gensize = (ItemSize)int.Parse(s[i]); i++;
                ItemSize shsize = (ItemSize)int.Parse(s[i]); i++;
                ItemSize ensize = (ItemSize)int.Parse(s[i]); i++;
                ItemSize comsize = (ItemSize)int.Parse(s[i]); i++;
                List<WeaponParams> weaplist = new List<WeaponParams>();
                int weapcount = int.Parse(s[i]); i++;
                for (int j = 0; j < weapcount; j++)
                {
                    ItemSize ws = (ItemSize)int.Parse(s[i]); i++;
                    WeaponGroup wg = (WeaponGroup)int.Parse(s[i]); i++;
                    weaplist.Add(new WeaponParams(ws, wg));
                }
                List<ItemSize> equiplist = new List<ItemSize>();
                int equipcount = int.Parse(s[i]); i++;
                for (int j=0; j<equipcount;j++)
                {
                    ItemSize es = (ItemSize)int.Parse(s[i]); i++;
                    equiplist.Add(es);
                }
                byte level = byte.Parse(s[i]); i++;
                bool israre = bool.Parse(s[i]); i++;
                int money = int.Parse(s[i]); i++;
                int metal = int.Parse(s[i]); i++;
                int chips = int.Parse(s[i]); i++;
                int anti = int.Parse(s[i]); i++;
                ItemPrice price = new ItemPrice(money, metal, chips, anti);
                ShipTypeclass shiptype = new ShipTypeclass(id, "", health, armor,
                    shsize, gensize, comsize, ensize, weaplist.ToArray(), equiplist.ToArray(), price);
                shiptype.En = En.ToString();
                shiptype.Ru = Ru.ToString();
                shiptype.Level = level;
                shiptype.IsRare = israre;
                list.Add(id, shiptype);
            }
            return list;
        }
        /*
        public static SortedList<ushort, ShipTypeclass> GetList2(byte[] array)
        {
            SortedList<ushort, ShipTypeclass> list = new SortedList<ushort, ShipTypeclass>();
            List<string> l1 = new List<string>(); int k = 0;
            for (int i = 0; i < array.Length; )
            {
                l1.Add("");
                ushort id = BitConverter.ToUInt16(array, i); i += 2;
                l1[k] += id.ToString()+"/";
                GSString En = new GSString(array, i); i += En.Array.Length;
                l1[k] += En.ToString() + "/";
                GSString Ru = new GSString(array, i); i += Ru.Array.Length;
                l1[k] += Ru.ToString() + "/";
                int health = BitConverter.ToInt32(array, i); i += 4;
                l1[k] += health.ToString() + "/";
                int armor = BitConverter.ToInt32(array, i); i += 4;
                l1[k] += armor.ToString() + "/";
                ItemSize gensize = (ItemSize)array[i]; i++;
                l1[k] += ((int)gensize).ToString() + "/";
                ItemSize shsize = (ItemSize)array[i]; i++;
                l1[k] += ((int)shsize).ToString() + "/";
                ItemSize ensize = (ItemSize)array[i]; i++;
                l1[k] += ((int)ensize).ToString() + "/";
                ItemSize comsize = (ItemSize)array[i]; i++;
                l1[k] += ((int)comsize).ToString() + "/";
                List<WeaponParams> weaplist = new List<WeaponParams>();
                if (WeaponParams.GetClass(array, i) != null) weaplist.Add(WeaponParams.GetClass(array, i)); i += 2;
                if (WeaponParams.GetClass(array, i) != null) weaplist.Add(WeaponParams.GetClass(array, i)); i += 2;
                if (WeaponParams.GetClass(array, i) != null) weaplist.Add(WeaponParams.GetClass(array, i)); i += 2;
                l1[k] += weaplist.Count.ToString() + "/";
                foreach (WeaponParams p in weaplist)
                {
                    l1[k] += ((int)p.Size).ToString() + "/";
                    l1[k] += ((int)p.Group).ToString() + "/";
                }
                List<ItemSize> equiplist = new List<ItemSize>();
                if (array[i] != 255) equiplist.Add((ItemSize)array[i]); i++;
                if (array[i] != 255) equiplist.Add((ItemSize)array[i]); i++;
                if (array[i] != 255) equiplist.Add((ItemSize)array[i]); i++;
                if (array[i] != 255) equiplist.Add((ItemSize)array[i]); i++;
                l1[k] += equiplist.Count.ToString() + "/";
                foreach (ItemSize s in equiplist)
                    l1[k] += ((int)s).ToString() + "/";
                byte level = array[i]; i++;
                l1[k] += level.ToString() + "/";
                bool israre = BitConverter.ToBoolean(array, i); i++;
                l1[k] += israre.ToString() + "/";
                ItemPrice price = ItemPrice.GetPrice(array, i); i += 16;
                l1[k] += price.Money.ToString() + "/";
                l1[k] += price.Metall.ToString() + "/";
                l1[k] += price.Chips.ToString() + "/";
                l1[k] += price.Anti.ToString() + "/";
                ShipTypeclass shiptype = new ShipTypeclass(id, "", health, armor, 
                    shsize, gensize, comsize, ensize, weaplist.ToArray(), equiplist.ToArray(), price);
                shiptype.En = En.ToString();
                shiptype.Ru = Ru.ToString();
                shiptype.Level = level;
                shiptype.IsRare = israre;
                list.Add(id, shiptype);
                k++;
            }
            System.IO.File.WriteAllLines("ShipTypes.txt", l1.ToArray(), Encoding.GetEncoding(1251));
            return list;
        }*/
        /*
        public ShipGenerator2Types GetShipType()
        {
            int type = ID - Level * 1000;
            type = (type / 10) + (type % 10 >= 5 ? 100 : 0);
            switch (type)
            {
                case 20: return ShipGenerator2Types.Scout;
                case 120: return ShipGenerator2Types.Corvett;
                case 21: return ShipGenerator2Types.Cargo;
                case 121: return ShipGenerator2Types.Linkor;
                case 22: return ShipGenerator2Types.Frigate;
                case 122: return ShipGenerator2Types.Fighter;
                case 23: return ShipGenerator2Types.Dreadnought;
                case 123: return ShipGenerator2Types.Devostator;
                case 24: return ShipGenerator2Types.Warrior;
                case 124: return ShipGenerator2Types.Cruiser;
                default: throw new Exception();
            }
        }
        public int GetShipTypeType()
        {
            int type = ID - Level * 1000;
            return (type / 10) + (type % 10 >= 5 ? 100 : 0);
        }*/

        public void SetStats(ShipParamsClass schema, ArmorType type)
        {
            schema.BasicHealth = Health;
            schema.BasicWEnergyIgnore = Links.Armors[type].ToEnergy * Armor / 100;
            schema.BasicWPhysicIgnore = Links.Armors[type].ToPhysic * Armor / 100;
            schema.BasicWIrregularIgnore = Links.Armors[type].ToIrregular * Armor / 100;
            schema.BasicWCyberIgnore = Links.Armors[type].ToCybernetic * Armor / 100;
        }
        public void SetPrice(ItemPrice price)
        {
            price.Add(Price);
        }
        Inline AddInline(string caption, Inline value)
        {
            Span Line = new Span(); Line.Inlines.Add(new Run(caption + ": "));
            Line.Inlines.Add(value); Line.Inlines.Add(new LineBreak());
            return Line;
        }
        public override string ToString()
        {
            if (Links.Lang == 0)
                return Common.ToUpper(En);
            else
                return Common.ToUpper(Ru);
        }
        public ItemSize GetSize()
        {
            return ItemSize.Any;
        }
        public WeaponGroup GetWeaponGroup()
        {
            WeaponGroup result = WeaponGroup.Any;
            foreach (WeaponParams par in WeaponsParam)
                if (par.Group != WeaponGroup.Any) 
                    result = par.Group;
            return result;
        }
        public WeaponParams GetWeaponParams(int pos)
        {
            if (pos == 0)
            {
                if (WeaponCapacity != 1)
                    return WeaponsParam[0];
                else return null;
            }
            else if (pos == 1)
            {
                if (WeaponCapacity == 2)
                    return null;
                else if (WeaponCapacity == 1)
                    return WeaponsParam[0];
                else
                    return WeaponsParam[1];
            }
            else
            {
                if (WeaponCapacity == 1)
                    return null;
                else if (WeaponCapacity == 2)
                    return WeaponsParam[1];
                else
                    return WeaponsParam[2];
            }
        }
       
        public static UIElement[] GetShipTypeColumnNames()
        {
            List<UIElement> result = new List<UIElement>();
            result.Add(Common.GetRectangle(30, Pictogram.HealthBrush,Links.Interface("Hull")));
            result.Add(Common.GetRectangle(30,Links.Brushes.WeaponGroupBrush[WeaponGroup.Any]));
            result.Add(Common.GetRectangle(30, Links.Brushes.EquipmentBrush));
            result.Add(Common.GetRectangle(30, Links.Brushes.GeneratorBrush));
            result.Add(Common.GetRectangle(30, Links.Brushes.ShieldBrush));
            result.Add(Common.GetRectangle(30, Links.Brushes.ComputerBrush));
            result.Add(Common.GetRectangle(30, Links.Brushes.EngineBrush));
            result.Add(Common.GetRectangle(30, Pictogram.HealthBrush, Links.Interface("AlIgn")));
            result.Add(Common.GetRectangle(30, Links.Brushes.MoneyImageBrush));
            result.Add(Common.GetRectangle(30, Links.Brushes.MetalImageBrush));
            result.Add(Common.GetRectangle(30, Links.Brushes.ChipsImageBrush));
            result.Add(Common.GetRectangle(30, Links.Brushes.AntiImageBrush));
            return result.ToArray();
        }
        public int GetParam(int pos)
        {
            int result = 0;
            switch (pos)
            {
                case 0: return Health;
                case 1: foreach (WeaponParams par in WeaponsParam)
                            result += ((int)(par.Size) + 1)*(par.Group==WeaponGroup.Any?2:1); return result;
                case 2: foreach (ItemSize s in EquipmentsSize)
                        result += ((int)(s) + 1); return result;
                case 3: return (int)GeneratorSize;
                case 4: return (int)ShieldSize;
                case 5: return (int)ComputerSize;
                case 6: return (int)EngineSize;
                case 7: return Armor;
                case 8: return Price.Money;
                case 9: return Price.Metall;
                case 10: return Price.Chips;
                case 11: return Price.Anti;
                default: return 0;
            }
        }
        public StackPanel GetWeaponsPanel()
        {
            StackPanel result = new StackPanel();
            result.HorizontalAlignment = HorizontalAlignment.Center;
            result.VerticalAlignment = VerticalAlignment.Center;
            result.Orientation = Orientation.Horizontal;
            foreach (WeaponParams par in WeaponsParam)
            {
                StackPanel panel = new StackPanel();
                panel.Children.Add(Common.GetRectangle(15, Links.Brushes.WeaponGroupBrush[par.Group]));
                panel.Children.Add(Common.GetSizeRect(15, par.Size, false));
                panel.Margin = new Thickness(2, 0, 2, 0);
                result.Children.Add(panel);
            }
            return result;
        }
        public string GetName()
        {
            if (Links.Lang == 0)
                return string.Format( "{1} \"{0}\"", Common.ToUpper(En), GetBattleGroupName());
            else
                return string.Format("{1} \"{0}\"", Common.ToUpper(Ru), GetBattleGroupName());
        }
        public string GetBattleGroupName()
        {
            //int type = GetShipTypeType();
            switch (Type)
            {
                case ShipGenerator2Types.Scout: return Links.Interface("Ship_Scout");
                case ShipGenerator2Types.Corvett: return Links.Interface("Ship_Corvette");
                case ShipGenerator2Types.Cargo: return Links.Interface("Ship_Transport");
                case ShipGenerator2Types.Linkor: return Links.Interface("Ship_Battle");
                case ShipGenerator2Types.Frigate: return Links.Interface("Ship_Frigate");
                case ShipGenerator2Types.Fighter: return Links.Interface("Ship_Fighter");
                case ShipGenerator2Types.Dreadnought: return Links.Interface("Ship_Dreadnought");
                case ShipGenerator2Types.Devostator: return Links.Interface("Ship_Devostator");
                case ShipGenerator2Types.Warrior: return Links.Interface("Ship_Warrior");
                case ShipGenerator2Types.Cruiser: return Links.Interface("Ship_Cruiser");
                default: return "";
            }
        }
        public int GetID()
        {
            return ID;
        }
        public UIElement GetElement(int pos)
        {
            switch (pos)
            {
                case 1:
                    //StackPanel WeaponPanel = new StackPanel();
                    //WeaponPanel.Orientation = Orientation.Horizontal;
                    //WeaponPanel.HorizontalAlignment = HorizontalAlignment.Center;
                    //WeaponPanel.VerticalAlignment = VerticalAlignment.Center;
                    //for (int i = 0; i < WeaponsParam.Length; i++)
                    //{
                    //    WeaponPanel.Children.Add(Common.GetRectangle(15,Links.Brushes.WeaponGroupBrush[WeaponsParam[i].Group]));
                    //    WeaponPanel.Children.Add(Common.GetSizeRect(15,WeaponsParam[i].Size,false));
                   // }
                   // return WeaponPanel;
                    return GetWeaponsPanel();
                case 2: StackPanel EquipmentPanel = new StackPanel();
                    EquipmentPanel.Orientation = Orientation.Horizontal;
                    EquipmentPanel.HorizontalAlignment = HorizontalAlignment.Center;
                    EquipmentPanel.VerticalAlignment = VerticalAlignment.Center;
                    for (int i = 0; i < EquipmentsSize.Length; i++)
                        EquipmentPanel.Children.Add(Common.GetSizeRect(15, EquipmentsSize[i], false));
                    return EquipmentPanel;
                case 3: return Common.GetSizeRect(15, GeneratorSize, false);
                case 4: return Common.GetSizeRect(15,ShieldSize,false);
                case 5: return Common.GetSizeRect(15, ComputerSize, false);
                case 6: return Common.GetSizeRect(15, EngineSize, false);
                default: return null;
            }
        
        }
        public int CompareTo(object B)
        {
            ShipTypeclass b = (ShipTypeclass)B;
            if (ID > b.ID) return 1;
            else if (ID < b.ID) return -1;
            else return 0;
        }
    }

}
