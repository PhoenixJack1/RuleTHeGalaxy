using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class SchemaGenForMarket
    {
        enum EquipmentBlockForEnergy { None, First, Second }
        enum ShieldSelectMode { Recharge, Consume }
        enum CompSelectMode { Accuracy, Damage }
        public static Schema GetSchema(byte maxlevel)
        {
            ShipGenerator2Types type = (ShipGenerator2Types)ServerLinks.BattleRandom.Next(1, 11);
            //type = ShipGenerator2Types.Devostator;
            ShipTypeclass shiptype = ServerLinks.ShipParts.ShipTypes.Values[0];
            foreach (ShipTypeclass t in ServerLinks.ShipParts.ShipTypes.Values)
            {
                if (t.Type != type) continue;
                if (t.Level > maxlevel) continue;
                if (t.Level < shiptype.Level) continue;
                shiptype = t;
            }
            
            WeaponGroup group = WeaponGroup.Any;
            foreach (WeaponParams onegroup in shiptype.WeaponsParam)
                if (onegroup.Group != WeaponGroup.Any) { group = onegroup.Group; break; }
            if (group == WeaponGroup.Any)
                group = (WeaponGroup)(ServerLinks.BattleRandom.Next(4));
            Schema result = new Schema();
            result.SetShipType(shiptype);
            switch (type)
            {
                case ShipGenerator2Types.Cruiser:
                case ShipGenerator2Types.Linkor:
                    result.Armor = ArmorType.Balanced; break;
                case ShipGenerator2Types.Scout: case ShipGenerator2Types.Dreadnought: case ShipGenerator2Types.Warrior:
                    result.Armor = (ArmorType)ServerLinks.BattleRandom.Next(5, 8); break;
                default:
                    result.Armor = (ArmorType)ServerLinks.BattleRandom.Next(1, 5); break;
            }
            result.SetName(BitConverter.ToInt32(new byte[] { 0, 0, 0, (byte)(200 + maxlevel) }, 0));
            result.Group = group;
            Generatorclass curgen = ServerLinks.ShipParts.GeneratorTypes.Values[0];
            foreach (Generatorclass gen in ServerLinks.ShipParts.GeneratorTypes.Values)
            {
                if (gen.Level > maxlevel) continue;
                if (gen.Size > shiptype.GeneratorSize) continue;
                if (gen.Capacity < curgen.Capacity) continue;
                curgen = gen;
            }
            result.SetGenerator(curgen);
            byte elementlevel = maxlevel;
            ShieldSelectMode shieldmode = ShieldSelectMode.Consume;
            if (ServerLinks.BattleRandom.Next(2) == 0) shieldmode = ShieldSelectMode.Recharge;
            CompSelectMode compmode = CompSelectMode.Accuracy;
            if (ServerLinks.BattleRandom.Next(3) == 0) compmode = CompSelectMode.Damage;
            int excludeacc = 5;
            if (type == ShipGenerator2Types.Devostator || type == ShipGenerator2Types.Dreadnought)
                excludeacc = 5;
            else if (ServerLinks.BattleRandom.Next(2) == 0)
                excludeacc = 2;
            else
                excludeacc = 5;
            bool fillresult = false;
            for (int i = 0; i < 10; i++)
            {
                fillresult = FillSchema(maxlevel, result, maxlevel, EquipmentBlockForEnergy.None, shieldmode, compmode, excludeacc);
                if (fillresult == false) continue;
                fillresult = CheckEnergy(result, type);
                if (fillresult == true) break;
            }
            if (fillresult == true) return result;
            for (int i=0;i<10;i++)
            {
                elementlevel = (byte)(maxlevel - i); if (elementlevel < 10) elementlevel = 10;
                fillresult = FillSchema(maxlevel, result, elementlevel, EquipmentBlockForEnergy.None, shieldmode, compmode, excludeacc);
                if (fillresult == false) continue;
                fillresult = CheckEnergy(result, type);
                if (fillresult == true) break;
            }
            if (fillresult == true) return result;
            for (int i = 0; i < 10; i++)
            {
                elementlevel = (byte)(maxlevel - i); if (elementlevel < 10) elementlevel = 10;
                fillresult = FillSchema(maxlevel, result, elementlevel, EquipmentBlockForEnergy.First, shieldmode, compmode, excludeacc);
                if (fillresult == false) continue;
                fillresult = CheckEnergy(result, type);
                if (fillresult == true) break;
            }
            if (fillresult == true) return result;
            for (int i = 0; i < 10; i++)
            {
                elementlevel = (byte)(maxlevel - i); if (elementlevel < 10) elementlevel = 10;
                fillresult = FillSchema(maxlevel, result, elementlevel, EquipmentBlockForEnergy.Second, shieldmode, compmode, excludeacc);
                if (fillresult == false) continue;
                fillresult = CheckEnergy(result, type);
                if (fillresult == true) break;
            }
            
            //if (fillresult == true)
                return result;
            //CheckEnergy(result, type);
            //return null;
        }
        static bool CheckEnergy(Schema schema, ShipGenerator2Types type)
        {
            ServerShip ship = new ServerShip(0, schema, 100, new byte[4]);
            ServerShipB shipB = new ServerShipB(ship, 0, ShipSide.Attack, null, 255);
            switch (type)
            {
                case ShipGenerator2Types.Scout:
                    if (shipB.Params.HexMoveCost.GetCurrent * 5 > shipB.Params.Energy.GetMax) return false;
                    if (shipB.Params.HexMoveCost.GetCurrent * 3 > shipB.Params.Energy.GetRestore) return false;
                    if (shipB.Weapons[1].Consume + shipB.Params.HexMoveCost.GetCurrent > shipB.Params.Energy.GetRestore) return false;
                    break;
                case ShipGenerator2Types.Cargo:
                    if (shipB.Params.HexMoveCost.GetCurrent * 2 > shipB.Params.Energy.GetMax) return false;
                    if (shipB.Params.HexMoveCost.GetCurrent * 1.5 > shipB.Params.Energy.GetRestore) return false;
                    if (shipB.Weapons[1].Consume + shipB.Params.HexMoveCost.GetCurrent > shipB.Params.Energy.GetRestore) return false;
                    break;
                case ShipGenerator2Types.Corvett:
                    if (shipB.Params.HexMoveCost.GetCurrent * 2 > shipB.Params.Energy.GetMax) return false;
                    if (shipB.Weapons[1].Consume + shipB.Params.HexMoveCost.GetCurrent > shipB.Params.Energy.GetRestore) return false;
                    break;
                case ShipGenerator2Types.Linkor:
                    if (shipB.Params.HexMoveCost.GetCurrent > shipB.Params.Energy.GetMax) return false;
                    if (shipB.Weapons[0].Consume + shipB.Weapons[2].Consume > shipB.Params.Energy.GetRestore) return false;
                    break;
                case ShipGenerator2Types.Frigate:
                    if (shipB.Params.HexMoveCost.GetCurrent * 2 > shipB.Params.Energy.GetMax) return false;
                    if (shipB.Params.HexMoveCost.GetCurrent> shipB.Params.Energy.GetRestore) return false;
                    if (shipB.Weapons[0].Consume + shipB.Weapons[2].Consume > shipB.Params.Energy.GetRestore) return false;
                    break;
                case ShipGenerator2Types.Fighter:
                    if (shipB.Params.HexMoveCost.GetCurrent * 3 > shipB.Params.Energy.GetMax) return false;
                    if (shipB.Params.HexMoveCost.GetCurrent * 2 > shipB.Params.Energy.GetRestore) return false;
                    if (shipB.Weapons[0].Consume + shipB.Weapons[2].Consume + shipB.Params.HexMoveCost.GetCurrent > shipB.Params.Energy.GetRestore) return false;
                    break;
                case ShipGenerator2Types.Dreadnought:
                    if (shipB.Params.HexMoveCost.GetCurrent > shipB.Params.Energy.GetMax) return false;
                    if (shipB.Weapons[0].Consume + shipB.Weapons[2].Consume > shipB.Params.Energy.GetRestore) return false;
                    break;
                case ShipGenerator2Types.Devostator:
                    if (shipB.Params.HexMoveCost.GetCurrent > shipB.Params.Energy.GetMax) return false;
                    if (shipB.Weapons[0].Consume + shipB.Weapons[1].Consume + shipB.Weapons[2].Consume > shipB.Params.Energy.GetRestore*3/2) return false;
                    break;
                case ShipGenerator2Types.Warrior:
                    if (shipB.Params.HexMoveCost.GetCurrent > shipB.Params.Energy.GetMax) return false;
                    if (shipB.Weapons[0].Consume + shipB.Weapons[1].Consume + shipB.Weapons[2].Consume > shipB.Params.Energy.GetRestore * 3 / 2) return false;
                    break;
                case ShipGenerator2Types.Cruiser:
                    if (shipB.Params.HexMoveCost.GetCurrent > shipB.Params.Energy.GetMax) return false;
                    if (shipB.Weapons[0].Consume + shipB.Weapons[1].Consume + shipB.Weapons[2].Consume > shipB.Params.Energy.GetRestore * 3 / 2) return false;
                    break;

            }
            return true;
        }
        /// <summary> Метод заполняет схему корабля. Если параметр energyblock=0 - то без ограничений. (Выбирается новый элемент) 
        /// Если есть значение (в процентах), - то это максимальное потребление энергии от предыдущего элемента
        ///  </summary>
        static bool FillSchema(byte maxlevel, Schema schema, byte elementlevel, EquipmentBlockForEnergy energyequip, ShieldSelectMode shieldmode, CompSelectMode compmode, int excludeacc)
        {
            bool shieldresult = false;
            if (shieldmode==ShieldSelectMode.Recharge)
                shieldresult = SetRechargeShield(maxlevel, schema);
            else
                shieldresult = SetRechargeShield(maxlevel, schema);
            if (shieldresult == false) return false;
            bool engineresult = SetEngine(elementlevel, schema);
            if (engineresult == false) return false;
            bool compresult = false;
            if (compmode == CompSelectMode.Accuracy)
                compresult = SetAccComp(elementlevel, schema);
            else
                compresult = SetDamComp(elementlevel, schema);
            if (compresult == false) return false;
            bool weaponsresult = SetWeapons(elementlevel, schema, excludeacc);
            for (int i=0;i<schema.ShipType.EquipmentsSize.Length;i++)
            {
                if (i == 0 && (energyequip == EquipmentBlockForEnergy.First || energyequip==EquipmentBlockForEnergy.Second))
                {
                    bool equipresult = false;
                    if (schema.ShipType.EquipmentsSize[0] == ItemSize.Large)
                        equipresult = SetEnergyGenEquip(maxlevel, schema, 0, true);
                    else
                        equipresult = SetEnergyGenEquip(maxlevel, schema, 0, false);
                    if (equipresult == false) return false;
                }
                else if (i == 1 && energyequip == EquipmentBlockForEnergy.Second)
                {
                    bool equipresult = false;
                    if (schema.ShipType.EquipmentsSize[1] == ItemSize.Large)
                        equipresult = SetEnergyGenEquip(maxlevel, schema, 1, true);
                    else
                        equipresult = SetEnergyGenEquip(maxlevel, schema, 1, false);
                    if (equipresult == false) return false;
                }
                else if (schema.ShipType.EquipmentsSize[i]==ItemSize.Large)
                {
                    EquipmentType equiptype = LargeEquipList[ServerLinks.BattleRandom.Next(LargeEquipList.Count)];
                    bool largeequipresult = SetGroupEquipment(elementlevel, schema, i, equiptype);
                    if (largeequipresult == false) return false;
                }
                else
                {
                    List<EquipmentType> equiplist = GetEquipsList(schema);
                    EquipmentType curequip = equiplist[ServerLinks.BattleRandom.Next(equiplist.Count)];
                    bool equipresult = SetEquipment(elementlevel, schema, i, curequip);
                    if (equipresult == false) return false;
                }
            }

            return true;

        }
        static bool SetEnergyGenEquip(byte maxlevel, Schema schema, int pos, bool large)
        {
            Equipmentclass equip = null; int maxregen = 0;
            foreach (Equipmentclass e in ServerLinks.ShipParts.EquipmentTypes.Values)
            {
                if (e.EqType != EquipmentType.EnergyRegen) continue;
                if (e.Level > maxlevel) continue;
                if (large == true && e.Size != ItemSize.Large) continue;
                if (e.Size > schema.ShipType.EquipmentsSize[pos]) continue;
                if (e.Value < maxregen) continue;
                equip = e; maxregen = e.Value;
            }
            if (equip == null) return false;
            schema.SetEquipment(equip, pos);
            return true;
        }
        static List<EquipmentType> GetEquipsList(Schema schema)
        {
            List<EquipmentType> list = new List<EquipmentType>(new EquipmentType[] {EquipmentType.Energy,EquipmentType.EnergyRegen,EquipmentType.EvaAl,
                EquipmentType.EvaCy,EquipmentType.EvaEn,EquipmentType.EvaIr,EquipmentType.EvaPh,EquipmentType.Hull,EquipmentType.HullRegen,EquipmentType.IgnAl,
                EquipmentType.IgnCy,EquipmentType.IgnEn,EquipmentType.IgnIr,EquipmentType.IgnPh,EquipmentType.ImmAl,EquipmentType.ImmCy,EquipmentType.ImmEn,
                EquipmentType.ImmIr,EquipmentType.ImmPh,EquipmentType.Shield });
            if (schema.Shield.Recharge * 1.5 < schema.Shield.Capacity) list.Add(EquipmentType.ShieldRegen);
            if (schema.Group == WeaponGroup.Energy) list.AddRange(new EquipmentType[] { EquipmentType.DamEn, EquipmentType.AccEn });
            else if (schema.Group == WeaponGroup.Physic) list.AddRange(new EquipmentType[] { EquipmentType.DamPh, EquipmentType.AccPh });
            else if (schema.Group == WeaponGroup.Irregular) list.AddRange(new EquipmentType[] { EquipmentType.DamIr, EquipmentType.AccIr });
            else if (schema.Group == WeaponGroup.Cyber) list.AddRange(new EquipmentType[] { EquipmentType.DamCy, EquipmentType.AccCy });
            return list; 
        }
        static bool SetEquipment(byte maxlevel, Schema schema, int pos, EquipmentType type)
        {
            List<Equipmentclass> list = new List<Equipmentclass>();
            int minlevel = maxlevel - 10;
            foreach (Equipmentclass e in ServerLinks.ShipParts.EquipmentTypes.Values)
            {
                if (e.EqType != type) continue;
                if (e.Level > maxlevel || e.Level < minlevel) continue;
                if (e.Size > schema.ShipType.EquipmentsSize[pos]) continue;
                list.Add(e);
            }
            if (list.Count == 0) return false;
            schema.SetEquipment(list[ServerLinks.BattleRandom.Next(list.Count)], pos);
            return true;
        }
        static List<EquipmentType> LargeEquipList = new List<EquipmentType>(new EquipmentType[] { EquipmentType.AccAl, EquipmentType.DamAl,EquipmentType.Energy,
        EquipmentType.EnergyRegen,EquipmentType.EvaAl,EquipmentType.EvaCy,EquipmentType.EvaEn,EquipmentType.EvaIr,EquipmentType.EvaPh,EquipmentType.Hull,EquipmentType.HullRegen,
        EquipmentType.IgnAl,EquipmentType.IgnCy,EquipmentType.IgnEn,EquipmentType.IgnIr,EquipmentType.IgnPh,EquipmentType.ImmAl,EquipmentType.ImmCy,EquipmentType.ImmEn,
            EquipmentType.ImmIr,EquipmentType.ImmPh,EquipmentType.Shield,EquipmentType.ShieldRegen});
        static bool SetGroupEquipment(byte maxlevel, Schema schema, int pos, EquipmentType type)
        {
            List<Equipmentclass> list = new List<Equipmentclass>();
            int minlevel = maxlevel - 10;
            foreach (Equipmentclass e in ServerLinks.ShipParts.EquipmentTypes.Values)
            {
                if (e.EqType != type) continue;
                if (e.Level > maxlevel || e.Level < minlevel) continue;
                if (e.Size != ItemSize.Large) continue;
                list.Add(e);
            }
            if (list.Count == 0) return false;
            schema.SetEquipment(list[ServerLinks.BattleRandom.Next(list.Count)], pos);
            return true;
        }
        static bool SetWeapons(byte maxlevel, Schema schema, int excludeacc)
        {
            for (int i=0;i<schema.ShipType.WeaponCapacity;i++)
            {
                WeaponParams p = schema.ShipType.WeaponsParam[i];
                int minlevel = maxlevel - 6;
                List<Weaponclass> list = new List<Weaponclass>();
                foreach (Weaponclass w in ServerLinks.ShipParts.WeaponTypes.Values)
                {
                    if (w.Level > maxlevel || w.Level < minlevel) continue;
                    if (w.Group != schema.Group) continue;
                    if (w.Size > p.Size) continue;
                    list.Add(w);
                }
                if (list.Count == 0) return false;
                schema.SetWeapon(list[ServerLinks.BattleRandom.Next(list.Count)], i);
            }
            return true;
        }
        static bool SetDamComp(byte maxlevel, Schema schema)
        {
            Computerclass comp = null; int maxdam = 0;
            foreach (Computerclass c in ServerLinks.ShipParts.ComputerTypes.Values)
            {
                if (c.Level > maxlevel) continue;
                if (c.Size > schema.ShipType.ComputerSize) continue;
                if (c.group != schema.Group) continue;
                if (c.MaxDamage < maxdam) continue;
                comp = c; maxdam = c.MaxDamage;
            }
            /*if (comp == null)
            {
                int consume = Int32.MaxValue;
                foreach (Computerclass c in ServerLinks.ShipParts.ComputerTypes.Values)
                {
                    if (c.Level > maxlevel) continue;
                    if (c.Size > schema.ShipType.ComputerSize) continue;
                    if (c.group != WeaponGroup.Any && c.group != schema.Group) continue;
                    if (c.Consume > consume) continue;
                    comp = c; consume = c.Consume;
                }
            }*/
            if (comp == null) return false;
            schema.SetComputer(comp);
            return true;
        }
        static bool SetAccComp(byte maxlevel, Schema schema)
        {
            Computerclass comp = null; int maxacc = 0;
            foreach (Computerclass c in ServerLinks.ShipParts.ComputerTypes.Values)
            {
                if (c.Level > maxlevel) continue;
                if (c.Size > schema.ShipType.ComputerSize) continue;
                if (c.group != WeaponGroup.Any && c.group != schema.Group) continue;
                if (c.MaxAccuracy < maxacc) continue;
                comp = c; maxacc = c.MaxAccuracy;
            }
            /*if (comp== null)
            {
                int consume = Int32.MaxValue;
                foreach (Computerclass c in ServerLinks.ShipParts.ComputerTypes.Values)
                {
                    if (c.Level > maxlevel) continue;
                    if (c.Size > schema.ShipType.ComputerSize) continue;
                    if (c.group != WeaponGroup.Any && c.group != schema.Group) continue;
                    if (c.Consume > consume) continue;
                    comp = c; consume = c.Consume;
                }
            }*/
            if (comp == null) return false;
            schema.SetComputer(comp);
            return true;
        }
        static bool SetEngine(byte maxlevel, Schema schema)
        {
            Engineclass engine = null; int level = 0;
            foreach (Engineclass e in ServerLinks.ShipParts.EngineTypes.Values)
            {
                if (e.Level > maxlevel) continue;
                if (e.Size != schema.ShipType.EngineSize) continue;
                if (e.Level < level) continue;
                engine = e; level = e.Level;
            }
            /*if (engine == null)
            {
                int consume = Int32.MaxValue;
                foreach (Engineclass e in ServerLinks.ShipParts.EngineTypes.Values)
                {
                    if (e.Level > maxlevel) continue;
                    if (e.Size != schema.ShipType.EngineSize) continue;
                    if (e.Consume > consume) continue;
                    engine = e; consume = e.Consume;
                }
            }*/
            if (engine== null)  return false;
            schema.SetEngine(engine);
            return true;
        }
        static bool SetRechargeShield(byte maxlevel, Schema schema)
        {
            Shieldclass shield = null; int maxrecharge = 0;
            foreach (Shieldclass s in ServerLinks.ShipParts.ShieldTypes.Values)
            {
                if (s.Level > maxlevel) continue;
                if (s.Size != schema.ShipType.ShieldSize) continue;
                if (s.Recharge < maxrecharge) continue;
                shield = s; maxrecharge = s.Recharge;
            }
            if (shield == null) return false;
            schema.SetShield(shield);
            return true;
        }
        static bool SetCapacityShield(byte maxlevel, Schema schema)
        {
            Shieldclass shield = null; int maxcapacity = 0;
            foreach (Shieldclass s in ServerLinks.ShipParts.ShieldTypes.Values)
            {
                if (s.Level > maxlevel) continue;
                if (s.Size != schema.ShipType.ShieldSize) continue;
                if (s.Capacity<maxcapacity) continue;
                shield = s; maxcapacity = s.Capacity;
            }
            if (shield == null) return false;
            schema.SetShield(shield);
            return true;
        }
    }
}
