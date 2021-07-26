using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class BattleWeapon
    {
        public EWeaponType Type;
        public WeaponGroup Group;
        public int Damage;
        public int ToShieldV;
        public int ToHealthV;
        public int Consume;
        public ItemSize Size;
        public ShipB Ship;
        //public Weaponclass Weapon;
        public int Accuracy()
        {
            int BlindModifier = Ship.Params.Status.IsBlinded == 0 ? 1 : 2;
            return (100 + Ship.Params.Accuracy.GetCurValue(Group)) / BlindModifier;
        }
        public int ToShield()
        {
            int NewDamage = Damage + Ship.Params.Increase.GetCurValue(Group);
            return (NewDamage * (100 + Links.Modifiers[Type].ToShield) + NewDamage * 100) / 200;
            //return (ToShieldV * (100 + Ship.Params.Increase.GetCurValue(Group))) / 100;
        }
        public int ToHealth()
        {
            int NewDamage = Damage + Ship.Params.Increase.GetCurValue(Group);
            return (NewDamage * (100 + Links.Modifiers[Type].ToHealth) + NewDamage * 100) / 200;
            //return (ToHealthV * (100 + Ship.Params.Increase.GetCurValue(Group))) / 100;
        }
        public bool _IsArmed { get; private set; }
        string LastReason;
        public void IsArmed(bool value, string reason)
        {
            _IsArmed = value; LastReason = reason;
        }
        public BattleWeapon(ShipB ship, int weapon)
        {
            Ship = ship;
            Weaponclass Weapon = ship.Schema.GetWeapon(weapon);
            Type = Weapon.Type;
            Group = Weapon.Group;
            Damage = Weapon.Damage;
            ToShieldV = Weapon.ShieldDamage;
            ToHealthV = Weapon.HealthDamage;
            Consume = Weapon.Consume;
            Size = Weapon.Size;
            IsArmed(true, "Создание оружия");
        }
        public BattleWeapon(ushort id)
        {
            Weaponclass Weapon = Links.WeaponTypes[id];
            Type = Weapon.Type;
            Group = Weapon.Group;
            Damage = Weapon.Damage;
            ToShieldV = Weapon.ShieldDamage;
            ToHealthV = Weapon.HealthDamage;
            Consume = Weapon.Consume;
            Size = Weapon.Size;
            IsArmed(true, "Создание оружия");
        }
        public BattleWeapon(EWeaponType type, ItemSize size, int damage, int consume)
        {
            Type = type;
            Group = Weaponclass.GetGroup(type);
            Size = size;
            Damage = damage;
            Consume = consume;
            ToShieldV = (Damage * (100 + Links.Modifiers[Type].ToShield) + Damage * 100) / 200;
            ToHealthV = (Damage * (100 + Links.Modifiers[Type].ToHealth) + Damage * 100) / 200;
            IsArmed(true, "Создание оружия");
        }
        public static BattleWeapon[] GetWeaponsFromArray(byte[] array, ref int i)
        {
            BattleWeapon[] result = new BattleWeapon[3];
            for (int j = 0; j < 3; j++)
            {
                byte real = array[i]; i++;
                if (real==1)
                {
                    EWeaponType type = (EWeaponType)array[i]; i++;
                    ItemSize size = (ItemSize)array[i]; i++;
                    int damage = BitConverter.ToInt32(array, i); i += 4;
                    int consume = BitConverter.ToInt32(array, i); i += 4;
                    result[j] = new BattleWeapon(type, size, damage, consume);
                }
            }
            return result;
        }
    }
    public class DamageResult
    {
        public int ToShield;
        public int ToHealth;
        public int ToEnergy;
        public bool IsDestroyed;
        public DamageResult(int toShield, int toHealth, int toEnergy, bool isdestroyed)
        {
            ToShield = toShield;
            ToHealth = toHealth;
            ToEnergy = toEnergy;
            IsDestroyed = isdestroyed;
        }
    }
}
