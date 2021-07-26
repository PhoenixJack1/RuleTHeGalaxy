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
    public class Weaponclass : Item, ISort, IComparable
    {
        public EWeaponType Type;
        public int Damage;
        public WeaponGroup Group;
        public int ShieldDamage;
        public int HealthDamage;
        public byte Level;
        public bool IsRare;
        public byte AccModifier;
        public Weaponclass(ushort id, string name, ItemSize size, EWeaponType type, int damage, int consume, ItemPrice price)
            : base(id, name, size, consume, price)
        {
            Type = type;
            Damage = damage;
            if ((int)type < 4) Group = WeaponGroup.Energy;
            else if ((int)type < 8) Group = WeaponGroup.Physic;
            else if ((int)type < 12) Group = WeaponGroup.Irregular;
            else Group = WeaponGroup.Cyber;
            ShieldDamage = (Damage * (100 + Links.Modifiers[Type].ToShield) + Damage * 100) / 200;
            HealthDamage = (Damage * (100 + Links.Modifiers[Type].ToHealth) + Damage * 100) / 200;
            AccModifier = (byte)ServerLinks.ShipParts.Modifiers[Type].Accuracy;
        }
        public static SortedList<ushort, Weaponclass> GetList(byte[] array)
        {
            SortedList<ushort, Weaponclass> list = new SortedList<ushort, Weaponclass>();
            List<String> l = new List<string>();
            for (int i = 0; i < array.Length; )
            {
                string s = "";
                ushort id = BitConverter.ToUInt16(array, i); i += 2;
                s += id + "\t";
                ItemSize size = (ItemSize)array[i]; i++;
                s += (int)size + "\t";
                EWeaponType type = (EWeaponType)array[i]; i++;
                s += type.ToString() + "\t\t";
                s+=(int)type + "\t";
                int consume = BitConverter.ToInt32(array, i); i += 4;
                s+=consume.ToString() + "\t";
                int damage = BitConverter.ToInt32(array, i); i += 4;
                s+=damage.ToString() + "\t";
                byte level = array[i]; i++;
                s += level.ToString() + "\t";
                bool israre = BitConverter.ToBoolean(array, i); i++;
                s+=israre.ToString() + "\t";
                ItemPrice price = ItemPrice.GetPrice(array, i); i += 16;
                s += price.Money.ToString() + "\t";
                s+=price.Metall.ToString() + "\t";
                s+=price.Chips.ToString() + "\t";
                s+=price.Anti.ToString() + "\t";
                Weaponclass weapon = new Weaponclass(id, "", size, type, damage, consume, price);
                weapon.Level = level; weapon.IsRare = israre;
                list.Add(id, weapon);
                l.Add(s);
            }
            System.IO.File.WriteAllLines("weapons.txt", l.ToArray());
            return list;
        }
        
        public void SetStats(ShipParamsClass schema)
        {
        }
 
        public void SetPrice(ItemPrice price)
        {
            price.Add(Price);
        }
       
        new public WeaponGroup GetWeaponGroup()
        {
            return Group;
        }
        public static bool GroupsAreEqual(WeaponGroup First, WeaponGroup Second)
        {
            if (Second == WeaponGroup.Any) return true;
            else if (First == Second) return true;
            else return false;
        }
      
        public static UIElement[] GetWeaponColumnNames()
        {
            List<UIElement> list = new List<UIElement>();
            list.Add(Common.GetBlock(20, Links.Interface("Type")));
            list.Add(Common.GetRectangle(30, Pictogram.ShieldDamage, Links.Interface("ShieldDamage")));
            list.Add(Common.GetRectangle(30, Pictogram.HealthDamage, Links.Interface("HealthDamage")));
            list.Add(Common.GetRectangle(30, Pictogram.Accuracy, Links.Interface("Accuracy")));
            list.Add(Common.GetRectangle(30, Pictogram.CriticalChance, Links.Interface("CritChance")));
            list.Add(Common.GetBlock(20, Links.Interface("Size")));
            list.Add(Common.GetRectangle(30, Pictogram.EnergyBrush, Links.Interface("EnergySpent")));
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
                case 0: return (int)Type;
                case 1: return ShieldDamage;
                case 2: return HealthDamage;
                case 3: return Links.Modifiers[Type].Accuracy;
                case 4: return Links.Modifiers[Type].Crit[Size];
                case 5: return (int)Size;
                case 6: return Consume;
                case 7: return Price.Money;
                case 8: return Price.Metall;
                case 9: return Price.Chips;
                case 10: return Price.Anti;
                default: return 0;
            }
        }
        public string GetName()
        {
            return GameObjectName.GetWeaponName(this);
        }
        public override string ToString()
        {
            return string.Format("{0} {1} {2}-{3}", Type.ToString(), Size.ToString(), Level.ToString(), Damage.ToString());
        }
        public int GetID()
        {
            return ID;
        }
        public UIElement GetElement(int pos)
        {
            switch (pos)
            {
                case 0:
                    return WeapCanvas.GetCanvas(Type, 30, true);
                    //Rectangle weaprect = SchemaImage.GetWeaponRectangle(Type);
                    //return weaprect;
                case 3: return Common.GetBlock(20, Common.GetAccuracyText(Links.Modifiers[Type].Accuracy));
                case 5: return Common.GetSizeRect(15, Size, false); 
                //return Common.GetSizeLabel(Size);
                default: return null;
            }

        }
        public bool CheckWeaponGroup(WeaponGroup group)
        {
            if (group == WeaponGroup.Any || group == Group) return true;
            else return false;
        }
        public int CompareTo(object B)
        {
            Weaponclass b = (Weaponclass)B;
            if (ID > b.ID) return 1;
            else if (ID < b.ID) return -1;
            else return 0;
        }
        public static WeaponGroup GetGroup(EWeaponType type)
        {
            switch (type)
            {
                case EWeaponType.Laser:
                case EWeaponType.EMI:
                case EWeaponType.Plasma:
                case EWeaponType.Solar:
                    return WeaponGroup.Energy;
                case EWeaponType.Cannon:
                case EWeaponType.Gauss:
                case EWeaponType.Missle:
                case EWeaponType.AntiMatter:
                    return WeaponGroup.Physic;
                case EWeaponType.Psi:
                case EWeaponType.Dark:
                case EWeaponType.Warp:
                case EWeaponType.Time:
                    return WeaponGroup.Irregular;
                case EWeaponType.Slicing:
                case EWeaponType.Radiation:
                case EWeaponType.Drone:
                case EWeaponType.Magnet:
                    return WeaponGroup.Cyber;
                default: throw new Exception();
            }
        }
    }
    struct WeaponModifer
    {
        public int ToShield;
        public int ToHealth;
        public int Consume;
        public int Accuracy;
        public SortedList<ItemSize, int> Crit;
        public bool ImmunePierce;
        public WeaponModifer(int toShield, int toHealth, int consume, int accuracy, int critS, int critM, int critL, bool immunepierce)
        {
            ToShield = toShield; ToHealth = toHealth; Consume = consume; Accuracy = accuracy;
            Crit = new SortedList<ItemSize, int>();
            Crit.Add(ItemSize.Small, critS); Crit.Add(ItemSize.Medium, critM); Crit.Add(ItemSize.Large, critL);
            ImmunePierce = immunepierce;
        }
        public static SortedList<EWeaponType, WeaponModifer> GetList(byte[] array, ref int i)
        {
            SortedList<EWeaponType, WeaponModifer> list = new SortedList<EWeaponType, WeaponModifer>();
            list.Add(EWeaponType.Laser, new WeaponModifer(25, -25, 0, 3, 40, 60, 80, false));
            list.Add(EWeaponType.EMI, new WeaponModifer(100, -100, 25, 3, 50, 75, 100, false));
            list.Add(EWeaponType.Plasma, new WeaponModifer(100, 100, -100, 5, 20, 35, 50, true));
            list.Add(EWeaponType.Solar, new WeaponModifer(25, 25, -75, 4, 5, 15, 25, false));
            list.Add(EWeaponType.Cannon, new WeaponModifer(-75, 25, 100, 4, 20, 40, 60, false));
            list.Add(EWeaponType.Gauss, new WeaponModifer(0, 0, -25, 3, 20, 35, 50, false));
            list.Add(EWeaponType.Missle, new WeaponModifer(-100, 25, -25, 2, 40, 60, 80, true));
            list.Add(EWeaponType.AntiMatter, new WeaponModifer(25, 50, -50, 5, 40, 60, 80, false));
            list.Add(EWeaponType.Psi, new WeaponModifer(-25, -50, 25, 3, 40, 60, 80, false));
            list.Add(EWeaponType.Dark, new WeaponModifer(50, 25, -50, 4, 50, 75, 100, true));
            list.Add(EWeaponType.Warp, new WeaponModifer(0, 100, -50, 5, 10, 25, 40, false));
            list.Add(EWeaponType.Time, new WeaponModifer(-50, 0, -50, 2, 40, 60, 80, false));
            list.Add(EWeaponType.Slicing, new WeaponModifer(-50, -50, 50, 4, 20, 35, 50, false));
            list.Add(EWeaponType.Radiation, new WeaponModifer(-25, -50, -75, 4, 40, 60, 80, false));
            list.Add(EWeaponType.Drone, new WeaponModifer(-25, 0, -50, 2, 20, 35, 50, false));
            list.Add(EWeaponType.Magnet, new WeaponModifer(25, -25, -25, 3, 50, 75, 100, true));
            return list;
        }
    }
    class ShipArmor:ISort
    {
        public int ID;
        public int ToEnergy;
        public int ToPhysic;
        public int ToIrregular;
        public int ToCybernetic;
        public ShipArmor(int id, int toenergy, int tophysic, int toirregular, int tocybernetic)
        {
            ID = id; ToEnergy = toenergy; ToPhysic = tophysic; ToIrregular = toirregular; ToCybernetic = tocybernetic;
        }

        public static SortedList<ArmorType, ShipArmor> GetList(byte[] array)
        {
            SortedList<ArmorType, ShipArmor> list = new SortedList<ArmorType, ShipArmor>();
            int i = 0;
            for (int pos = 0; pos < 8; pos++)
            {
                int toenergy = BitConverter.ToInt32(array, i); i += 4;
                int tophysic = BitConverter.ToInt32(array, i); i += 4;
                int toirregular = BitConverter.ToInt32(array, i); i += 4;
                int tocybernetic = BitConverter.ToInt32(array, i); i += 4;
                list.Add((ArmorType)pos, new ShipArmor(pos, toenergy, tophysic, toirregular, tocybernetic));
            }
            return list;
        }
        public static UIElement[] GetArmorColumnNames()
        {
            List<UIElement> list = new List<UIElement>();
            list.Add(Common.GetRectangle(30, Pictogram.IgnoreEnergy, Links.Interface("EnIgn")));
            list.Add(Common.GetRectangle(30, Pictogram.IgnorePhysic, Links.Interface("PhIgn")));
            list.Add(Common.GetRectangle(30, Pictogram.IgnoreIrregular, Links.Interface("IrIgn")));
            list.Add(Common.GetRectangle(30, Pictogram.IgnoreCyber, Links.Interface("CyIgn")));
            return list.ToArray();
        }
        public int GetParam(int pos)
        {
            switch (pos)
            {
                case 0: return ToEnergy;
                case 1: return ToPhysic;
                case 2: return ToIrregular;
                case 3: return ToCybernetic;
                default: return 0;
            }
        }
        public string GetName()
        {
            return Links.Interface(string.Format("ArmorType{0}", ID));
        }
        public int GetID()
        {
            return ID;
        }
        public UIElement GetElement(int pos)
        {
            switch (pos)
            {
                case 0: return Common.CreateLabel(16,ToEnergy.ToString() + "%");
                case 1: return Common.CreateLabel(16, ToPhysic.ToString() + "%");
                case 2: return Common.CreateLabel(16, ToIrregular.ToString() + "%");
                case 3: return Common.CreateLabel(16, ToCybernetic.ToString() + "%");
                default: return null;
            }
        }
    }
}
