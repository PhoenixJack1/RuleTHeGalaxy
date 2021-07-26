using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class WeaponParams
    {
        public ItemSize Size;
        public WeaponGroup Group;
        public WeaponParams(ItemSize size, WeaponGroup group)
        {
            Size = size;
            Group = group;
        }
        public WeaponParams(string stringparams)
        {
            switch (stringparams[0])
            {
                case 'E': Group = WeaponGroup.Energy; break;
                case 'P': Group = WeaponGroup.Physic; break;
                case 'I': Group = WeaponGroup.Irregular; break;
                case 'C': Group = WeaponGroup.Cyber; break;
                case 'A': Group = WeaponGroup.Any; break;
            }
            Size = (ItemSize)Int32.Parse(stringparams.Substring(1, 1));
        }
        public bool isEqual(Weaponclass weapon)
        {
            if (Group != WeaponGroup.Any && Group != weapon.Group)
                return false;
            if (Size < weapon.Size) return false;
            return true;
            
        }
        public static string ToString(WeaponParams[] weapons)
        {
            List<char> result = new List<char>();
            if (weapons.Length == 0) return "";
            foreach (WeaponParams weapon in weapons)
            {
                switch (weapon.Group)
                {
                    case WeaponGroup.Energy: result.Add('E'); break;
                    case WeaponGroup.Physic: result.Add('P'); break;
                    case WeaponGroup.Irregular: result.Add('I'); break;
                    case WeaponGroup.Cyber: result.Add('C'); break;
                    case WeaponGroup.Any: result.Add('A'); break;
                }
                result.Add(((int)(weapon.Size)).ToString()[0]);
            }
            return new string(result.ToArray());
        }
        public static WeaponParams[] FromString(string stringparams)
        {
            int Length = stringparams.Length / 2;
            if (Length == 0) return null;
            WeaponParams[] result = new WeaponParams[Length];
            for (int i = 0; i < stringparams.Length; i += 2)
            {
                string oneweaponparam = stringparams.Substring(0 + i, 2);
                result[i / 2] = new WeaponParams(oneweaponparam);
            }
            return result;
        }
        public static WeaponParams GetClass(byte[] array, int startindex)
        {
            if (array[startindex] != 255)
                return new WeaponParams((ItemSize)array[startindex + 1], (WeaponGroup)array[startindex]);
            else
                return null;
        }
    }
}
