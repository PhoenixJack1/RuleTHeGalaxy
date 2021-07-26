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
    public enum ArmorType { Balanced, Reflect, Composite, Anomal, Cybernetic, Traditional, Energy, Heavy }
    public class Schema
    {
        public ushort ShipTypeID { get; private set; }
        public ushort ShieldID { get; private set; }
        public ushort GeneratorID { get; private set; }
        public ushort EngineID { get; private set; }
        public ushort ComputerID { get; private set; }
        public int CryptName { get; private set; }
        public ShipTypeclass ShipType { get; private set; }
        public Generatorclass Generator { get; private set; }
        public Shieldclass Shield { get; private set; }
        public Computerclass Computer { get; private set; }
        public Engineclass Engine { get; private set; }

        public ushort[] WeaponsID { get; private set; }
        public Weaponclass[] Weapons { get; private set; }
        public ushort[] EquipmentsID { get; private set; }
        public Equipmentclass[] Equipments { get; private set; }
        public ArmorType Armor;
        public ItemPrice Price;
        public WeaponGroup Group;
        public bool IsReal;
        public Schema()
        {
            ShipTypeID = 0;
            ShieldID = 0;
            GeneratorID = 0;
            EngineID = 0;
            ComputerID = 0;
            CryptName = 0;
            Armor = ArmorType.Reflect;
            Weapons = new Weaponclass[0];
            Equipments = new Equipmentclass[0];
            WeaponsID = new ushort[0];
            EquipmentsID = new ushort[0];
        }
        public static Schema GetSchema(string param)
        {
            Schema schema = new Schema();
            TextPosition[] positions = TextPosition.GetPositions(param);
            foreach (TextPosition pos in positions)
            {
                switch (pos.Title)
                {
                    case "ST": schema.SetShipType((ushort)pos.IntValues[0]); break;
                    case "GEN": schema.SetGenerator((ushort)pos.IntValues[0]); break;
                    case "SH": schema.SetShield((ushort)pos.IntValues[0]); break;
                    case "ENG": schema.SetEngine((ushort)pos.IntValues[0]); break;
                    case "COMP": schema.SetComputer((ushort)pos.IntValues[0]); break;
                    case "W1": schema.SetWeapon((ushort)pos.IntValues[0], 0); break;
                    case "W2": schema.SetWeapon((ushort)pos.IntValues[0], 1); break;
                    case "W3": schema.SetWeapon((ushort)pos.IntValues[0], 2); break;
                    case "E1": schema.SetEquipment((ushort)pos.IntValues[0], 0); break;
                    case "E2": schema.SetEquipment((ushort)pos.IntValues[0], 1); break;
                    case "E3": schema.SetEquipment((ushort)pos.IntValues[0], 2); break;
                    case "E4": schema.SetEquipment((ushort)pos.IntValues[0], 3); break;
                    case "NAME": schema.SetName(BitConverter.ToInt32(pos.GetBytes(), 0)); break;
                    case "ARM": schema.Armor = (ArmorType)pos.IntValues[0]; break;
                }
            }
            if (schema.Check()) schema.IsReal = true;
            return schema;
        }
        public static Schema GetSchema(byte[] array, ref int s)
        {
            if (array.Length < s + 29) return null;
            Schema schema = new Schema();
            if (!schema.SetShipType(BitConverter.ToUInt16(array,s))) return null;
            schema.SetGenerator(BitConverter.ToUInt16(array, s+2));
            schema.SetShield(BitConverter.ToUInt16(array, s+4));
            schema.SetComputer(BitConverter.ToUInt16(array, s+6));
            schema.SetEngine(BitConverter.ToUInt16(array, s+8));
            schema.SetWeapon(BitConverter.ToUInt16(array,s+10),0, false);
            schema.SetWeapon(BitConverter.ToUInt16(array, s+12), 1, false);
            schema.SetWeapon(BitConverter.ToUInt16(array, s+14), 2, false);
            schema.SetEquipment(BitConverter.ToUInt16(array, s+16), 0);
            schema.SetEquipment(BitConverter.ToUInt16(array, s+18), 1);
            schema.SetEquipment(BitConverter.ToUInt16(array, s+20), 2);
            schema.SetEquipment(BitConverter.ToUInt16(array, s+22), 3);
            schema.SetName(BitConverter.ToInt32(array, s+24));
            schema.Armor = (ArmorType)array[s + 28];
            s += 29;
            if (schema.Check()) schema.IsReal = true;
            //schema.Calculate();
            return schema;
            
        }
        public void SetClearShipType()
        {
            ShipType = null; ShipTypeID = 0;
            Shield = null; ShieldID = 0;
            Generator = null; GeneratorID = 0;
            Computer = null; ComputerID = 0;
            Engine = null; EngineID = 0;
            Armor = ArmorType.Balanced;
            Weapons = new Weaponclass[0]; WeaponsID = new ushort[0];
            Equipments = new Equipmentclass[0]; EquipmentsID = new ushort[0];
        }
        public bool SetShipType(ushort shipTypeID)
        {
            if (shipTypeID == 0) { SetClearShipType(); return true; }
            if (!Links.ShipTypes.ContainsKey(shipTypeID)) return false;
            if (shipTypeID == ShipTypeID) return true;
            return SetShipType(Links.ShipTypes[shipTypeID]);
        }
        public bool SetShipType(ShipTypeclass shipType)
        {
            if (shipType == null) { SetClearShipType(); return true; }
            if (shipType == ShipType)
                return true;
            else
            {
                ShipTypeID = shipType.ID;
                ShipType = shipType;
                if (Shield != null && Shield.Size > shipType.ShieldSize) SetShield(0);
                if (Generator != null && Generator.Size > shipType.GeneratorSize) SetGenerator(0);
                if (Computer != null && Computer.Size > shipType.ComputerSize) SetComputer(0);
                if (Engine != null && Engine.Size > shipType.EngineSize) SetEngine(0);
                Weaponclass[] weapons=new Weaponclass[shipType.WeaponCapacity];
                WeaponsID = new ushort[shipType.WeaponCapacity];
                for (int i = 0; i < weapons.Length; i++)
                {
                    WeaponsID[i] = 0;
                    if (Weapons.Length<=i) continue;
                    if (weapons.Length <= i || Weapons[i]==null) continue;
                    if (shipType.WeaponsParam[i].isEqual(Weapons[i]))
                    {
                        weapons[i] = Weapons[i];
                        WeaponsID[i] = weapons[i].ID;
                    }
                }
                Weapons = weapons;
                Equipmentclass[] equipments = new Equipmentclass[shipType.EquipmentCapacity];
                EquipmentsID = new ushort[shipType.EquipmentCapacity];
                for (int i = 0; i < equipments.Length; i++)
                {
                    EquipmentsID[i] = 0;
                    if (Equipments.Length <= i) continue;
                    if (Equipments.Length <= i || Equipments[i] == null) continue;
                    if (shipType.EquipmentsSize[i] >= Equipments[i].Size)
                    {
                        equipments[i] = Equipments[i];
                        EquipmentsID[i] = equipments[i].ID;
                    }
                }
                Equipments = equipments;
                return true;
            }
        }
        public bool SetShield(ushort shieldID)
        {
            if (ShipType == null) return false;
            if (shieldID == 0) { ShieldID = 0; Shield = null; return true; }
            if (shieldID == ShieldID) return true;
            if (!Links.ShieldTypes.ContainsKey(shieldID) || Links.ShieldTypes[shieldID].Size > ShipType.ShieldSize) return false;
            ShieldID=shieldID; Shield=Links.ShieldTypes[shieldID]; return true;
        }
        public bool SetShield(Shieldclass shield)
        {
            if (ShipType == null) return false;
            if (shield == null) { ShieldID = 0; Shield = null; return true; }
            if (shield==Shield) return true;
            if (shield.Size <= ShipType.ShieldSize)
            {
                Shield = shield; ShieldID = shield.ID; return true;
            }
            else return false;
        }
        public bool SetGenerator(ushort generatorID)
        {
            if (ShipType == null) return false;
            if (generatorID == 0) { GeneratorID = 0; Generator = null; return true; }
            if (generatorID == GeneratorID) return true;
            if (!Links.GeneratorTypes.ContainsKey(generatorID) || Links.GeneratorTypes[generatorID].Size > ShipType.GeneratorSize) return false;
            GeneratorID=generatorID; Generator=Links.GeneratorTypes[generatorID]; return true;
        }
        public bool SetGenerator(Generatorclass generator)
        {
            if (ShipType == null) return false;
            if (generator == null) { GeneratorID = 0; Generator = null; return true; }
            if (generator == Generator) return true;
            if (generator.Size <= ShipType.GeneratorSize)
            { Generator = generator; GeneratorID = generator.ID; return true; }
            else return false;
        }
        public bool SetComputer(ushort computerID)
        {
            if (ShipType == null) return false;
            if (computerID == 0) { ComputerID = 0; Computer = null; return true; }
            if (computerID == ComputerID) return true;
            if (!Links.ComputerTypes.ContainsKey(computerID) || Links.ComputerTypes[computerID].Size > ShipType.ComputerSize) return false;
            ComputerID = computerID; Computer = Links.ComputerTypes[computerID]; return true;
        }
        public bool SetComputer(Computerclass computer)
        {
            if (ShipType == null) return false;
            if (computer == null) { ComputerID = 0; Computer = null; return true; }
            if (computer == Computer) return true;
            if (computer.Size <= ShipType.ComputerSize)
            { Computer = computer; ComputerID = computer.ID; return true; }
            else return false;
        }
        public bool SetEngine(ushort engineID)
        {
            if (ShipType == null) return false;
            if (engineID == 0) { EngineID = 0; Engine = null; return true; }
            if (engineID == EngineID) return true;
            if (!Links.EngineTypes.ContainsKey(engineID) || Links.EngineTypes[engineID].Size > ShipType.EngineSize) return false;
            EngineID = engineID; Engine = Links.EngineTypes[engineID]; return true;
        }
        public bool SetEngine(Engineclass engine)
        {
            if (ShipType == null) return false;
            if (engine == null) { EngineID = 0; Engine = null; return true; }
            if (engine==Engine) return true;
            if (engine.Size<=ShipType.EngineSize)
            {Engine=engine; EngineID=engine.ID; return true;}
            else return false;
        }
        public bool SetWeapon(ushort weaponID, int pos)
        {
            if (ShipType == null) return false;
            if (pos < 0 || pos >= ShipType.WeaponCapacity) return false;
            if (weaponID == 0) { WeaponsID[pos] = 0; Weapons[pos] = null; return true; }
            if (!ServerLinks.ShipParts.WeaponTypes.ContainsKey(weaponID) || !ShipType.WeaponsParam[pos].isEqual(ServerLinks.ShipParts.WeaponTypes[weaponID])) return false;
            WeaponsID[pos] = weaponID;
            Weapons[pos] = ServerLinks.ShipParts.WeaponTypes[weaponID];
            return true;
        }
        public bool SetWeapon(ushort weaponID, int pos, bool needposanalys)
        {
            if (ShipType == null) return false;
            if (needposanalys)
            {
                if (ShipType.WeaponCapacity == 1 && pos == 1) pos = 0;
                else if (ShipType.WeaponCapacity == 2 && pos == 2) pos = 1;
            }
            if (pos < 0 || pos >= ShipType.WeaponCapacity) return false;
            if (weaponID == 0) { WeaponsID[pos] = 0; Weapons[pos] = null; return true; }
            if (!Links.WeaponTypes.ContainsKey(weaponID) || !ShipType.WeaponsParam[pos].isEqual(Links.WeaponTypes[weaponID])) return false;
            WeaponsID[pos] = weaponID;
            Weapons[pos] = Links.WeaponTypes[weaponID];
            return true;
        }
        public bool SetWeapon(Weaponclass weapon, int pos)
        {
            if (ShipType == null) return false;
            if (pos < 0 || pos >= ShipType.WeaponCapacity) return false;
            if (weapon == null) { WeaponsID[pos] = 0; Weapons[pos] = null; return true; }
            if (!ShipType.WeaponsParam[pos].isEqual(weapon)) return false;
            Weapons[pos] = weapon;
            WeaponsID[pos] = weapon.ID;
            return true;
        }
        public bool SetEquipment(ushort equipmentID, int pos)
        {
            if (ShipType == null) return false;
            if (pos < 0 || pos >= ShipType.EquipmentCapacity) return false;
            if (equipmentID == 0) { EquipmentsID[pos] = 0; Equipments[pos] = null; return true; }
            if (!Links.EquipmentTypes.ContainsKey(equipmentID) || ShipType.EquipmentsSize[pos]<Links.EquipmentTypes[equipmentID].Size) return false;
            EquipmentsID[pos] = equipmentID;
            Equipments[pos] = Links.EquipmentTypes[equipmentID];
            return true;
        }
        public bool SetEquipment(Equipmentclass equipment, int pos)
        {
            if (ShipType == null) return false;
            if (pos < 0 || pos > ShipType.EquipmentCapacity) return false;
            if (equipment == null) { EquipmentsID[pos] = 0; Equipments[pos] = null; return true; }
            if (ShipType.EquipmentsSize[pos] < equipment.Size) return false;
            Equipments[pos] = equipment;
            EquipmentsID[pos] = equipment.ID;
            return true;
        }
        public void SetName(int cryptName)
        {
            CryptName = cryptName;
        }
        public int GetName()
        {
            return CryptName;
        }
        
        public void CalcPrice()
        {
            Price = new ItemPrice();
            if (ShipTypeID != 0) Price.Add(ShipType.Price);
            if (GeneratorID != 0) Price.Add(Generator.Price);
            if (ShieldID != 0) Price.Add(Shield.Price);
            if (ComputerID != 0) Price.Add(Computer.Price);
            if (EngineID != 0) Price.Add(Engine.Price);
            for (int i = 0; i < Weapons.Length; i++)
                if (WeaponsID[i] != 0) Price.Add(Weapons[i].Price);
            for (int i = 0; i < Equipments.Length; i++)
                if (EquipmentsID[i] != 0) Price.Add(Equipments[i].Price);
        
        }
         
        public bool Check()
        {
            if (ShipTypeID == 0) return false;
            if (GeneratorID == 0) return false;
            if (ShieldID == 0) return false;
            if (ComputerID == 0) return false;
            if (EngineID == 0) return false;
            return true;
        }
        public byte[] GetCrypt()
        {
            byte[] nullbyte = BitConverter.GetBytes((ushort)0);
           if (ShipTypeID == 0) return new byte[28];
            List<byte> result = new List<byte>();
            result.AddRange(BitConverter.GetBytes((ushort)ShipTypeID));
            if (GeneratorID != 0)
                result.AddRange(BitConverter.GetBytes((ushort)GeneratorID));
            else
                result.AddRange(nullbyte);
            if (ShieldID != 0)
                result.AddRange(BitConverter.GetBytes((ushort)ShieldID));
            else
                result.AddRange(nullbyte);
            if (ComputerID != 0)
                result.AddRange(BitConverter.GetBytes((ushort)ComputerID));
            else
                result.AddRange(nullbyte);
            if (EngineID != 0)
                result.AddRange(BitConverter.GetBytes((ushort)EngineID));
            else
                result.AddRange(nullbyte);
            for (int i = 0; i < 3; i++)
                if (i >= ShipType.WeaponCapacity || WeaponsID[i] == 0)
                    result.AddRange(nullbyte);
                else
                    result.AddRange(BitConverter.GetBytes((ushort)WeaponsID[i]));
            for (int i = 0; i < 4; i++)
                if (i >= ShipType.EquipmentCapacity || EquipmentsID[i] == 0)
                    result.AddRange(nullbyte);
                else
                    result.AddRange(BitConverter.GetBytes((ushort)EquipmentsID[i]));
            result.AddRange(BitConverter.GetBytes(CryptName));
            result.Add((byte)Armor);
            return result.ToArray();
        }
        /// <summary> метод возвращает оружие схемы. При этом 1 - левое, 2 - центральное, 3 - правое. Если оружия не установлено или непредусмотрено то null </summary>
        public Weaponclass GetWeapon(int pos)
        {
            switch (ShipType.WeaponCapacity)
            {
                case 1: if (pos == 1 && Weapons[0] != null) return Weapons[0]; else return null;
                case 2: if (pos == 0 && Weapons[0] != null) return Weapons[0];
                    else if (pos == 2 && Weapons[1] != null) return Weapons[1]; else return null;
                case 3: if (pos == 0 && Weapons[0] != null) return Weapons[0];
                    else if (pos == 1 && Weapons[1] != null) return Weapons[1];
                    else if (pos == 2 && Weapons[2] != null) return Weapons[2]; else return null;
                default: return null;
            }
        }
        public Equipmentclass GetEquipment(int pos)
        {
            if (Equipments.Length <= pos || Equipments[pos] == null)
                return null;
            else return Equipments[pos];
        }
        public override string ToString()
        {
            int weapon0 = Weapons.Length > 0 ? WeaponsID[0] : 0;
            int weapon1 = Weapons.Length > 1 ? WeaponsID[1] : 0;
            int weapon2 = Weapons.Length > 2 ? WeaponsID[2] : 0;
            int eq0 = GetEquipment(0) == null ? 0 : GetEquipment(0).ID;
            int eq1 = GetEquipment(1) == null ? 0 : GetEquipment(1).ID;
            int eq2 = GetEquipment(2) == null ? 0 : GetEquipment(2).ID;
            int eq3 = GetEquipment(3) == null ? 0 : GetEquipment(3).ID;
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}",
                ShipTypeID, GeneratorID, ShieldID, EngineID, ComputerID, weapon0, weapon1, weapon2,
                eq0, eq1, eq2, eq3, (int)Armor);
        }
        public void Calculate()
        {
            //ShipParams = new ShipParamsStruct();
            Price = new ItemPrice();
            if (ShipTypeID != 0) Price.Add(ShipType.Price);
            if (GeneratorID != 0) Price.Add(Generator.Price);
            if (ShieldID != 0) Price.Add(Shield.Price);
            if (ComputerID != 0) Price.Add(Computer.Price);
            if (EngineID != 0) Price.Add(Engine.Price);
            for (int i = 0; i < Weapons.Length; i++)
                if (WeaponsID[i] != 0) Price.Add(Weapons[i].Price);
            for (int i = 0; i < Equipments.Length; i++)
                if (EquipmentsID[i] != 0) Price.Add(Equipments[i].Price);
            //ShipParams.BasicEnergyRecharge -= ShipParams.BasicEnergySpent;

        }

        public float GetSchemaLevel()
        {
            int[] array = GetPartsArray();
            int sum = 0; int count = 0;
            for (int i = 0; i < 12; i++)
            {
                if (array[i] > 0)
                {
                    sum += Links.Science.GameSciences[(ushort)array[i]].Level;
                    count++;
                }
            }
            if (count > 0)
                return sum / (float)count;
            else
                return 0f;
        }
        public int[] GetPartsArray()
        {
            int[] array = new int[12];
            array[0] = ShipTypeID;
            array[1] = GeneratorID;
            array[2] = ShieldID;
            array[3] = ComputerID;
            array[4] = EngineID;
            switch (Weapons.Length)
            {
                case 1: array[5] = WeaponsID[0]; break;
                case 2:
                    array[5] = WeaponsID[0];
                    array[6] = WeaponsID[1]; break;
                case 3:
                    array[5] = WeaponsID[0];
                    array[6] = WeaponsID[1];
                    array[7] = WeaponsID[2]; break;
            }
            switch (Equipments.Length)
            {
                case 1: array[8] = EquipmentsID[0]; break;
                case 2:
                    array[8] = EquipmentsID[0];
                    array[9] = EquipmentsID[1]; break;
                case 3:
                    array[8] = EquipmentsID[0];
                    array[9] = EquipmentsID[1];
                    array[10] = EquipmentsID[2]; break;
                case 4:
                    array[8] = EquipmentsID[0];
                    array[9] = EquipmentsID[1];
                    array[10] = EquipmentsID[2];
                    array[11] = EquipmentsID[3]; break;
            }
            return array;
        }
    }
}
