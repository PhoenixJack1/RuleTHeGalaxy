using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class ServerBattleWeapon
    {
        public EWeaponType Type;
        public WeaponGroup Group;
        public int Damage;
        public int ToShieldV;
        public int ToHealthV;
        public int Consume;
        public ItemSize Size;
        public ServerShipB Ship;
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
        public bool IsArmed;
        public ServerBattleWeapon(ServerShipB ship, int weapon)
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
            IsArmed = true;
        }
        public ServerBattleWeapon(ushort id)
        {
            Weaponclass Weapon = Links.WeaponTypes[id];
            Type = Weapon.Type;
            Group = Weapon.Group;
            Damage = Weapon.Damage;
            ToShieldV = Weapon.ShieldDamage;
            ToHealthV = Weapon.HealthDamage;
            Consume = Weapon.Consume;
            Size = Weapon.Size;
            IsArmed = true;
        }
        public ServerBattleWeapon(WeaponBattleParam param)
        {
            Type = param.Type;
            Group = Weaponclass.GetGroup(Type);
            Damage = param.Damage;
            ToShieldV = (Damage * (100 + Links.Modifiers[Type].ToShield) + Damage * 100) / 200;
            ToHealthV = (Damage * (100 + Links.Modifiers[Type].ToHealth) + Damage * 100) / 200;
            Consume = param.Consume;
            Size = param.Size;
            IsArmed = true;
        }
        /// <summary> метод создаёт массив с информацией о пушке для передачи клиенту. Может принимать нулевое значение </summary>
        public static byte[] CreateArray(ServerBattleWeapon[] weapons)
        {
            List<byte> result = new List<byte>();
            for (int i = 0; i < 3; i++)
            {
                if (weapons[i] == null)
                    result.Add(0);
                else
                {
                    result.Add(1);
                    result.Add((byte)weapons[i].Type);
                    result.Add((byte)weapons[i].Size);
                    result.AddRange(BitConverter.GetBytes(weapons[i].Damage));
                    result.AddRange(BitConverter.GetBytes(weapons[i].Consume));
                }
            }
            return result.ToArray();
        }
    }
}
