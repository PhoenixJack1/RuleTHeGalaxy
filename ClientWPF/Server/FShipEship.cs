using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class FShip
    {
        public byte Hex;
        public int Energy;
        public byte ID;
        public bool ShipMoved = false;
        public int ShipMoveCost;
        public byte HealthPercent;
        public bool IsBig;
        public FWeapon[] Weapons = new FWeapon[3];
    }
    public class FWeapon
    {
        public int Damage;
        public int Accuracy;
        public int Consume;
        public WeaponGroup Group;
        public bool IsArmed = true;
        public EWeaponType Type;
    }
    public class EShip
    {
        public int Health;
        public byte Hex;
        public byte ID;
        public bool IsBig;
        public SortedList<WeaponGroup, int> Evasion = new SortedList<WeaponGroup, int>();
        public SortedList<WeaponGroup, int> Ignore = new SortedList<WeaponGroup, int>();
    }
}
