using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    enum EnemyMarks { TankShield, TankHealth, DamageDPS, DamageAccuracy, Evasion, Support, Balanced }

    class ServerSchemeGenerator
    {
        static SortedList<EnemyType, SortedSet<EWeaponType>> EnemyWeapons = GetEnemyWeapons();
        static int randomgen = ServerLinks.BattleRandom.Next(0,1000);
        static byte[] HealthTankEquipList = new byte[] { 5, 20, 5, 16, 17, 5, 20, 18, 19 };
        static byte[] DamageAccEquipList = new byte[] { 0, 21 };
        static byte[] DamageDPSEquipList = new byte[] { 21, 8, 21, 7 };
        static byte[] EvasionEquipList = new byte[] { 15, 21, 15, 11, 12, 21, 15, 13, 14 };
        static byte[] SupportLargeEquipList = new byte[] { 20, 25, 4, 7, 8, 5, 10 };
        static byte[] SupportMediumEquipList = new byte[] { 5, 7, 20, 0 };
        static byte[] BalancedEquipList = new byte[] { 0, 5, 7, 8, 10, 15, 20, 21 };
        public static Schema GetEnemySchema(byte level, EnemyMarks mark, EnemyType enemy)
        {
            //Определение схемы корабля
            Schema schema = new Schema();
            if (level < 6) level = 6;
            byte minlevel = (byte)(level - 6);
            //byte minlevel = (byte)(level > 6 ? level - 6 : 0);
            ShipTypeclass shiptype = null;
            int energyrecharge = 0;
            int weaponspent = 0;
            switch (mark)
            {
                case EnemyMarks.TankShield:
                case EnemyMarks.TankHealth: shiptype = GetTankShipClass(level, minlevel); break;
                case EnemyMarks.DamageDPS: shiptype = GetDamageDPSShipClass(level, minlevel); break;
                case EnemyMarks.DamageAccuracy: shiptype = GetDamageAccShipClass(level, minlevel); break;
                case EnemyMarks.Evasion: shiptype = GetEvasionhipClass(level, minlevel); break;
                case EnemyMarks.Support: shiptype = GetSupportShipClass(level, minlevel); break;
                case EnemyMarks.Balanced: shiptype = GetBalancedShipClass(level, minlevel); break;

            }
            if (shiptype == null) return null;
            schema.SetShipType(shiptype);
            //Определение группы оружия
            WeaponGroup group = WeaponGroup.Any;
            foreach (WeaponParams onegroup in shiptype.WeaponsParam)
                if (onegroup.Group != WeaponGroup.Any) { group = onegroup.Group; break; }
            if (group == WeaponGroup.Any)
/*УБРАТЬ*/
                //group = WeaponGroup.Cyber;
                 { group = (WeaponGroup)(randomgen % 4); randomgen++; }
            schema.Group = group;
            //определение вооружения
            for (int i = 0; i < shiptype.WeaponCapacity; i++)
            {
                Weaponclass weapon = null;
                switch (mark)
                {
                    case EnemyMarks.TankShield:
                    case EnemyMarks.TankHealth:
                    case EnemyMarks.Evasion:
                        weapon = GetWeapon(level, minlevel, enemy, group, shiptype.WeaponsParam[i].Size, 2);
                        if (weapon != null) schema.SetWeapon(weapon, i);
                        break;
                    case EnemyMarks.DamageDPS:
                    case EnemyMarks.DamageAccuracy:
                    case EnemyMarks.Support:
                    case EnemyMarks.Balanced:
                        weapon = GetWeapon(level, minlevel, enemy, group, shiptype.WeaponsParam[i].Size, 5);
                        if (weapon != null) schema.SetWeapon(weapon, i);
                        break;
                }
                if (weapon != null) weaponspent += weapon.Consume;
            }
            //определение генератора, щита, вычислителя и двигателя
            Shieldclass shield = null;
            Generatorclass generator = null;
            Computerclass computer = null;
            Engineclass engine = null;
            switch (mark)
            {
                case EnemyMarks.TankShield:
                    shield = GetShield(level, minlevel, shiptype.ShieldSize, 5);
                    generator = GetGenerator(level, minlevel, shiptype.GeneratorSize, 2);
                    computer = GetComputer(level, minlevel, shiptype.ComputerSize, group, 3);
                    break;
                case EnemyMarks.TankHealth:
                    shield = GetShield(level, minlevel, shiptype.ShieldSize, 1);
                    generator = GetGenerator(level, minlevel, shiptype.GeneratorSize, 2);
                    computer = GetComputer(level, minlevel, shiptype.ComputerSize, group, 3);
                    break;
                case EnemyMarks.DamageDPS:
                    shield = GetShield(level, minlevel, shiptype.ShieldSize, 5);
                    generator = GetGenerator(level, minlevel, shiptype.GeneratorSize, 2);
                    computer = GetComputer(level, minlevel, shiptype.ComputerSize, group, 5);
                    break;
                case EnemyMarks.DamageAccuracy:
                    shield = GetShield(level, minlevel, shiptype.ShieldSize, 5);
                    generator = GetGenerator(level, minlevel, shiptype.GeneratorSize, 2);
                    computer = GetComputer(level, minlevel, shiptype.ComputerSize, group, 0);
                    break;
                case EnemyMarks.Evasion:
                    shield = GetShield(level, minlevel, shiptype.ShieldSize, 5);
                    generator = GetGenerator(level, minlevel, shiptype.GeneratorSize, 3);
                    computer = GetComputer(level, minlevel, shiptype.ComputerSize, group, 5);
                    engine = GetEngineUniverse(level, minlevel, shiptype.EngineSize);
                    break;
                case EnemyMarks.Support:
                    shield = GetShield(level, minlevel, shiptype.ShieldSize, 5);
                    generator = GetGenerator(level, minlevel, shiptype.GeneratorSize, 3);
                    computer = GetComputer(level, minlevel, shiptype.ComputerSize, group, 0);
                    break;
                case EnemyMarks.Balanced:
                    shield = GetShield(level, minlevel, shiptype.ShieldSize, 1);
                    generator = GetGenerator(level, minlevel, shiptype.GeneratorSize, 3);
                    computer = GetComputer(level, minlevel, shiptype.ComputerSize, group, 1);
                    break;
            }
            if (engine == null) engine = GetEngine(level, minlevel, shiptype.EngineSize);
            if (shield != null) schema.SetShield(shield); else return null;
            if (generator != null) schema.SetGenerator(generator); else return null;
            if (computer != null) schema.SetComputer(computer); else return null;
            if (engine != null) schema.SetEngine(engine); else return null;
            energyrecharge += generator.Recharge;
            energyrecharge -= shield.Consume;
            energyrecharge -= computer.Consume;
            energyrecharge -= engine.Consume;
            //определение дополнительных модулей
            for (int i = 0; i < shiptype.EquipmentCapacity; i++)
            {
                Equipmentclass equip = null;
                if (energyrecharge < weaponspent * 0.6)
                {
                    equip = GetEquipment(level, 0, shiptype.EquipmentsSize[i], 10);
                }
                /*
                if (i == shiptype.EquipmentCapacity - 1) //модуль регенерации, если регенрации мало
                {
                    int regen = generator.Recharge - shield.Consume - computer.Consume - engine.Consume;
                    for (int j = 0; j < shiptype.EquipmentCapacity - 1; j++)
                        if (schema.Equipments[j] == null) continue;
                        else if (schema.Equipments[j].Type == 10) regen += schema.Equipments[j].Value;
                        else regen -= schema.Equipments[j].Value;
                    if (regen < generator.Recharge * 0.4)
                    {
                        equip = GetEquipment(level, 0, shiptype.EquipmentsSize[i], 10);
                        continue;
                    }
                }
                */
                else
                    switch (mark)
                    {
                        case EnemyMarks.TankShield: equip = GetEquipment(level, minlevel, shiptype.EquipmentsSize[i], (byte)(7 + randomgen % 2)); randomgen++; break;
                        case EnemyMarks.TankHealth:
                            equip = GetEquipment(level, minlevel, shiptype.EquipmentsSize[i],
    HealthTankEquipList[randomgen % HealthTankEquipList.Length]); randomgen++; break;
                        case EnemyMarks.DamageAccuracy:
                            equip = GetEquipmentGroup(level, minlevel, shiptype.EquipmentsSize[i],
DamageAccEquipList[randomgen % DamageAccEquipList.Length], group); randomgen++; break;
                        case EnemyMarks.DamageDPS:
                            equip = GetEquipmentGroup(level, minlevel, shiptype.EquipmentsSize[i],
     DamageDPSEquipList[randomgen % DamageDPSEquipList.Length], group); randomgen++; break;
                        case EnemyMarks.Evasion:
                            equip = GetEquipmentGroup(level, minlevel, shiptype.EquipmentsSize[i],
       EvasionEquipList[randomgen % EvasionEquipList.Length], group); randomgen++; break;
                        case EnemyMarks.Support:
                            if (shiptype.EquipmentsSize[i] == ItemSize.Large)
                            { equip = GetEquipment((byte)(level + 3), minlevel, ItemSize.Large, SupportLargeEquipList[randomgen % SupportLargeEquipList.Length]); randomgen++; break; }
                            else
                            {
                                equip = GetEquipmentGroup(level, minlevel, shiptype.EquipmentsSize[i],
                                  SupportMediumEquipList[randomgen % SupportMediumEquipList.Length], group); randomgen++; break;
                            }
                        case EnemyMarks.Balanced:
                            equip = GetEquipmentGroup(level, minlevel, shiptype.EquipmentsSize[i],
      BalancedEquipList[randomgen % BalancedEquipList.Length], group); randomgen++; break;

                    }
                if (equip != null)
                {
                    schema.SetEquipment(equip, i);
                    if (equip.Type == 10) energyrecharge += equip.Value;
                    else energyrecharge -= equip.Consume;
                }
            }

            //определение брони
            switch (mark)
            {
                case EnemyMarks.TankShield: schema.Armor = ArmorType.Balanced; break;
                case EnemyMarks.TankHealth: schema.Armor = ArmorType.Balanced; break;
                case EnemyMarks.DamageDPS:
                case EnemyMarks.DamageAccuracy:
                case EnemyMarks.Evasion:
                case EnemyMarks.Support:
                case EnemyMarks.Balanced:
                    schema.Armor = GetArmor(shiptype.Armor); break;
            }
            if (enemy != EnemyType.None)
                schema.SetName(BitConverter.ToInt32(new byte[] { (byte)(251 + (byte)enemy), 0, 0, 0 }, 0));
            return schema;
        }
        static ArmorType GetArmor(int armor)
        {
            if (armor > 60) return ArmorType.Balanced;
            else if (armor > 40) return (ArmorType)(5 + randomgen % 3);
            else return (ArmorType)(1 + randomgen % 5);
        }
        static ShipTypeclass GetDamageDPSShipClass(byte level, byte minlevel)
        {
            ShipTypeclass best = null;
            int maxballs = 0;
            foreach (ShipTypeclass type in ServerLinks.ShipParts.ShipTypes.Values)
            {
                if (type.Level > level || type.Level < minlevel) continue;
                if (type.WeaponCapacity < 2) continue;
                if (type.GeneratorSize < ItemSize.Medium) continue;
                if (type.ComputerSize < ItemSize.Medium) continue;
                if (type.ShieldSize < ItemSize.Medium) continue;
                int balls = (type.ShieldSize == ItemSize.Large ? 2 : 0);
                balls += (type.GeneratorSize == ItemSize.Large ? 2 : 0);
                balls += (type.ComputerSize == ItemSize.Large ? 2 : 0);
                balls += (type.EngineSize == ItemSize.Large ? 2 : type.EngineSize == ItemSize.Medium ? 1 : 0);
                foreach (WeaponParams weapon in type.WeaponsParam)
                    balls += (2 + 2 * (int)weapon.Size) + (weapon.Group == WeaponGroup.Any ? 2 : 0);
                balls += type.EquipmentCapacity;
                balls += type.Armor / 10;
                balls += type.Health / 50;
                if (balls > maxballs)
                { best = type; maxballs = balls; }
            }
            return best;
        }
        static ShipTypeclass GetBalancedShipClass(byte level, byte minlevel)
        {
            ShipTypeclass best = null;
            int maxballs = 0;
            foreach (ShipTypeclass type in ServerLinks.ShipParts.ShipTypes.Values)
            {
                if (type.Level > level || type.Level < minlevel) continue;
                if (type.ShieldSize == ItemSize.Small) continue;
                if (type.GeneratorSize == ItemSize.Small) continue;
                if (type.ComputerSize == ItemSize.Small) continue;
                int balls = (type.ShieldSize == ItemSize.Large ? 4 : 0);
                balls += (type.ComputerSize == ItemSize.Large ? 4 : 0);
                balls += (type.GeneratorSize == ItemSize.Large ? 4 : 0);
                balls += (type.EngineSize == ItemSize.Large ? 3 : type.EngineSize == ItemSize.Medium ? 2 : 0);
                foreach (WeaponParams weapon in type.WeaponsParam)
                    balls += (2 + 2 * (int)weapon.Size) + (weapon.Group == WeaponGroup.Any ? 2 : 0);
                balls += type.EquipmentCapacity;
                balls += type.Armor / 10;
                balls += type.Health / 50;
                if (balls > maxballs)
                { best = type; maxballs = balls; }
            }
            return best;
        }
        static ShipTypeclass GetSupportShipClass(byte level, byte minlevel)
        {
            ShipTypeclass best = null;
            int maxballs = 0;
            foreach (ShipTypeclass type in ServerLinks.ShipParts.ShipTypes.Values)
            {
                if (type.Level > level || type.Level < minlevel) continue;
                if (type.EquipmentsSize[0] != ItemSize.Large) continue;
                if (type.GeneratorSize == ItemSize.Small) continue;
                int balls = (type.ShieldSize == ItemSize.Large ? 2 : type.ShieldSize == ItemSize.Medium ? 1 : 0);
                balls += (type.ComputerSize == ItemSize.Large ? 2 : type.ComputerSize == ItemSize.Medium ? 1 : 0);
                balls += (type.GeneratorSize == ItemSize.Large ? 4 : 0);
                if (type.EquipmentCapacity > 1 && type.EquipmentsSize[1] == ItemSize.Large) balls += 10;
                foreach (WeaponParams weapon in type.WeaponsParam)
                    balls += (2 + 2 * (int)weapon.Size) + (weapon.Group == WeaponGroup.Any ? 2 : 0);
                balls += type.EquipmentCapacity;
                balls += type.Armor / 10;
                balls += type.Health / 50;
                if (balls > maxballs)
                { best = type; maxballs = balls; }
            }
            return best;
        }
        static ShipTypeclass GetEvasionhipClass(byte level, byte minlevel)
        {
            ShipTypeclass best = null;
            int maxballs = 0;
            foreach (ShipTypeclass type in ServerLinks.ShipParts.ShipTypes.Values)
            {
                if (type.Level > level || type.Level < minlevel) continue;
                if (type.EngineSize != ItemSize.Large) continue;
                int balls = (type.ShieldSize == ItemSize.Large ? 2 : type.ShieldSize == ItemSize.Medium ? 1 : 0);
                balls += (type.ComputerSize == ItemSize.Large ? 2 : type.ComputerSize == ItemSize.Medium ? 1 : 0);
                balls += (type.GeneratorSize == ItemSize.Large ? 4 : type.EngineSize == ItemSize.Medium ? 2 : 0);
                foreach (WeaponParams weapon in type.WeaponsParam)
                    balls += (2 + 2 * (int)weapon.Size) + (weapon.Group == WeaponGroup.Any ? 2 : 0);
                balls += type.EquipmentCapacity;
                balls += type.Armor / 10;
                balls += type.Health / 50;
                if (balls > maxballs)
                { best = type; maxballs = balls; }
            }
            return best;
        }
        static ShipTypeclass GetDamageAccShipClass(byte level, byte minlevel)
        {
            ShipTypeclass best = null;
            int maxballs = 0;
            foreach (ShipTypeclass type in ServerLinks.ShipParts.ShipTypes.Values)
            {
                if (type.Level > level || type.Level < minlevel) continue;
                if (type.WeaponCapacity < 2) continue;
                if (type.GeneratorSize < ItemSize.Large) continue;
                if (type.ComputerSize < ItemSize.Medium) continue;
                int balls = (type.ShieldSize == ItemSize.Large ? 2 : type.ShieldSize == ItemSize.Medium ? 1 : 0);
                balls += (type.ComputerSize == ItemSize.Large ? 2 : 0);
                balls += (type.EngineSize == ItemSize.Large ? 2 : type.EngineSize == ItemSize.Medium ? 1 : 0);
                foreach (WeaponParams weapon in type.WeaponsParam)
                    balls += (2 + 2 * (int)weapon.Size) + (weapon.Group == WeaponGroup.Any ? 2 : 0);
                balls += type.EquipmentCapacity;
                balls += type.Armor / 10;
                balls += type.Health / 50;
                if (balls > maxballs)
                { best = type; maxballs = balls; }
            }
            return best;
        }
        static ShipTypeclass GetTankShipClass(byte level, byte minlevel)
        {
            ShipTypeclass best = null;
            int maxballs = 0;
            foreach (ShipTypeclass type in ServerLinks.ShipParts.ShipTypes.Values)
            {
                if (type.Level > level || type.Level < minlevel) continue;
                if (type.ShieldSize < ItemSize.Large) continue;
                int balls = (type.GeneratorSize == ItemSize.Large ? 4 : type.GeneratorSize == ItemSize.Medium ? 2 : 0);
                balls += (type.ComputerSize == ItemSize.Large ? 4 : type.ComputerSize == ItemSize.Medium ? 2 : 0);
                balls += (type.EngineSize == ItemSize.Large ? 4 : type.EngineSize == ItemSize.Medium ? 2 : 0);
                foreach (WeaponParams weapon in type.WeaponsParam)
                    balls += (2 + 2 * (int)weapon.Size) + (weapon.Group == WeaponGroup.Any ? 2 : 0);
                balls += type.EquipmentCapacity;
                balls += type.Armor / 5;
                if (balls > maxballs)
                { best = type; maxballs = balls; }
            }
            return best;
        }
        /// <summary> Метод подбирает пушку по параметрам. параметр badaccmodifier указывает то значение модификатора меткости, которое является нежелательным
        /// </summary>
        static Weaponclass GetWeapon(byte level, byte minlevel, EnemyType enemy, WeaponGroup group, ItemSize size, byte badaccmodifier)
        {
            Weaponclass best = null;
            int maxballs = 0;
            SortedSet<EWeaponType> set = EnemyWeapons[enemy];

            foreach (Weaponclass weapon in ServerLinks.ShipParts.WeaponTypes.Values)
            {
                if (weapon.Group != group) continue;
                if (group == WeaponGroup.Physic && set.Contains(EWeaponType.Cannon) && weapon.Type != EWeaponType.Cannon) continue;
/*УБРАТЬ*/                //if (group == WeaponGroup.Cyber && weapon.Type != EWeaponType.Drone) continue; // УБРАТЬ ТУТ
                if (weapon.Level > level || weapon.Level < minlevel) continue;
/*ВКЛЮЧИТЬ*/                if (!set.Contains(weapon.Type)) continue;
                if (weapon.AccModifier == badaccmodifier) continue;
                if (weapon.Size > size) continue;
                int balls = (weapon.HealthDamage + weapon.ShieldDamage) / 20 + 1;
                balls = (int)(balls * ((randomgen % 10) + 6 - ServerLinks.ShipParts.Modifiers[weapon.Type].Accuracy) * (weapon.Size == ItemSize.Large ? 2 : weapon.Size == ItemSize.Medium ? 1.5 : 1));
                if (balls > maxballs) { maxballs = balls; best = weapon; }
            }
            return best;
        }
        static Shieldclass GetShield(byte level, byte minlevel, ItemSize size, byte capmodifier)
        {
            Shieldclass best = null;
            int maxballs = 0;
            foreach (Shieldclass shield in ServerLinks.ShipParts.ShieldTypes.Values)
            {
                if (shield.Level > level || shield.Level < minlevel) continue;
                if (shield.Size > size) continue;
                int balls = shield.Capacity * capmodifier + shield.Recharge * 2;
                balls = (int)(balls * (shield.Size == ItemSize.Large ? 2.0 : shield.Size == ItemSize.Medium ? 1.5 : 1));
                if (balls > maxballs) { best = shield; maxballs = balls; }
            }
            return best;
        }
        static Computerclass GetComputer(byte level, byte minlevel, ItemSize size, WeaponGroup group, byte damagemodifier)
        {
            Computerclass best = null;
            int maxballs = 0;
            foreach (Computerclass comp in ServerLinks.ShipParts.ComputerTypes.Values)
            {
                if (comp.Level > level || comp.Level < minlevel) continue;
                if (comp.Size > size) continue;
                if (comp.group != WeaponGroup.Any && comp.group != group) continue;
                int balls = comp.MaxAccuracy + comp.MaxDamage * damagemodifier;
                balls = (int)(balls * (comp.Size == ItemSize.Large ? 1.2 : comp.Size == ItemSize.Medium ? 1.1 : 1));
                if (balls > maxballs) { best = comp; maxballs = balls; }
            }
            return best;
        }
        static Generatorclass GetGenerator(byte level, byte minlevel, ItemSize size, byte rechargemodi)
        {
            Generatorclass best = null;
            int maxballs = 0;
            foreach (Generatorclass generator in ServerLinks.ShipParts.GeneratorTypes.Values)
            {
                if (generator.Level > level || generator.Level < minlevel) continue;
                if (generator.Size > size) continue;
                int balls = generator.Capacity + generator.Recharge * rechargemodi;
                balls = (int)(balls * (generator.Size == ItemSize.Large ? 1.2 : generator.Size == ItemSize.Medium ? 1.1 : 1));
                if (balls > maxballs) { best = generator; maxballs = balls; }
            }
            return best;
        }
        static Engineclass GetEngineUniverse(byte level, byte minlevel, ItemSize size)
        {
            Engineclass best = null;
            int maxballs = 0;
            foreach (Engineclass engine in ServerLinks.ShipParts.EngineTypes.Values)
            {
                if (engine.Level > level || engine.Level < minlevel) continue;
                if (engine.Size > size) continue;
                int balls = engine.EnergyEvasion + engine.IrregularEvasion;
                balls = (int)(balls * (engine.Size == ItemSize.Large ? 2.0 : engine.Size == ItemSize.Medium ? 1.5 : 1));
                if (balls > maxballs) { best = engine; maxballs = balls; }
            }
            return best;
        }
        static Engineclass GetEngine(byte level, byte minlevel, ItemSize size)
        {
            Engineclass best = null;
            int maxballs = 0;
            foreach (Engineclass engine in ServerLinks.ShipParts.EngineTypes.Values)
            {
                if (engine.Level > level || engine.Level < minlevel) continue;
                if (engine.Size > size) continue;
                int balls = engine.Level;
                balls = (int)(balls * (engine.Size == ItemSize.Large ? 2.0 : engine.Size == ItemSize.Medium ? 1.5 : 1));
                if (balls > maxballs) { best = engine; maxballs = balls; }
            }
            return best;
        }
        /// <summary> метод подбирает оборудование по параметрам. В параметр type для оборудования для готорого возможно распределение по группам - указать первую позицию
        /// </summary>

        static Equipmentclass GetEquipmentGroup(byte level, byte minlevel, ItemSize size, byte type, WeaponGroup group)
        {
            Equipmentclass best = null;
            int maxballs = 0;
            foreach (Equipmentclass equip in ServerLinks.ShipParts.EquipmentTypes.Values)
            {
                if (equip.Level > level || equip.Level < minlevel) continue;
                if (equip.Size > size) continue;
                switch (type)
                {
                    case 0: if (type + (byte)group != equip.BasicType) continue; else break;
                    case 21: if (type + (byte)group != equip.BasicType) continue; else break;
                    default: if (type != equip.BasicType) continue; else break;
                }
                int balls = equip.Value;
                balls = (int)(balls * (equip.Size == ItemSize.Large ? 3.0 : 1));
                if (balls > maxballs) { best = equip; maxballs = balls; }
            }
            return best;
        }
        static Equipmentclass GetEquipment(byte level, byte minlevel, ItemSize size, byte type)
        {
            Equipmentclass best = null;
            int maxballs = 0;
            foreach (Equipmentclass equip in ServerLinks.ShipParts.EquipmentTypes.Values)
            {
                if (equip.BasicType != type) continue;
                if (equip.Level > level || equip.Level < minlevel) continue;
                if (equip.Size > size) continue;
                int balls = equip.Value;
                balls = (int)(balls * (equip.Size == ItemSize.Large ? 3.0 : 1));
                if (balls > maxballs) { best = equip; maxballs = balls; }
            }
            return best;
        }
        static SortedList<EnemyType, SortedSet<EWeaponType>> GetEnemyWeapons()
        {
            SortedList<EnemyType, SortedSet<EWeaponType>> list = new SortedList<EnemyType, SortedSet<EWeaponType>>();
            list.Add(EnemyType.Pirates, new SortedSet<EWeaponType>(new EWeaponType[] { EWeaponType.Laser, EWeaponType.Plasma, EWeaponType.Cannon, EWeaponType.Gauss,
                EWeaponType.Missle, EWeaponType.Dark, EWeaponType.Slicing, EWeaponType.Radiation }));
            list.Add(EnemyType.Aliens, new SortedSet<EWeaponType>(new EWeaponType[] { EWeaponType.Solar, EWeaponType.Gauss, EWeaponType.AntiMatter, EWeaponType.Psi,
            EWeaponType.Warp,EWeaponType.Time, EWeaponType.Radiation, EWeaponType.Magnet}));
            list.Add(EnemyType.GreenParty, new SortedSet<EWeaponType>(new EWeaponType[] { EWeaponType.Laser, EWeaponType.EMI, EWeaponType.Solar, EWeaponType.Cannon,
            EWeaponType.AntiMatter, EWeaponType.Psi, EWeaponType.Drone,EWeaponType.Magnet}));
            list.Add(EnemyType.Techno, new SortedSet<EWeaponType>(new EWeaponType[] { EWeaponType.EMI, EWeaponType.Gauss, EWeaponType.AntiMatter, EWeaponType.Warp,
            EWeaponType.Time, EWeaponType.Slicing, EWeaponType.Radiation, EWeaponType.Drone}));
            list.Add(EnemyType.Mercenaries, new SortedSet<EWeaponType>(new EWeaponType[] {EWeaponType.Plasma,EWeaponType.Solar, EWeaponType.Cannon, EWeaponType.Missle,
                EWeaponType.Dark, EWeaponType.Warp,EWeaponType.Slicing, EWeaponType.Magnet }));
            list.Add(EnemyType.None, new SortedSet<EWeaponType>(new EWeaponType[] { EWeaponType.Laser, EWeaponType.EMI, EWeaponType.Plasma, EWeaponType.Solar,
            EWeaponType.Cannon, EWeaponType.Gauss, EWeaponType.Missle, EWeaponType.AntiMatter, EWeaponType.Psi, EWeaponType.Dark, EWeaponType.Warp, EWeaponType.Time,
            EWeaponType.Slicing, EWeaponType.Radiation, EWeaponType.Drone, EWeaponType.Magnet}));
            return list;
        }
        public static ServerShip GetTestShip(int pos)
        {
            Schema schema = new Schema();
            schema.SetShipType(49245);
            schema.SetGenerator(50257);
            schema.SetShield(50262);
            schema.SetComputer(49280);
            schema.SetEngine(49276);
            schema.SetWeapon(49432, 0); schema.SetWeapon(49432, 1); schema.SetWeapon(48431, 2);
            schema.SetEquipment(49572, 0); schema.SetEquipment(48541, 1); schema.SetEquipment(48541, 2); schema.SetEquipment(46600, 3);
            schema.Armor = ArmorType.Balanced;
            ServerShip ship = new ServerShip(1000 + pos, schema, 100, new byte[4]);
            return ship;
        }
        static int selectmodel = 0;
        static int[] AlienMarks = new int[] { 6, 0, 1, 6, 2, 3, 6, 4, 6, 5 };
        static int[] GreenTeamMarks = new int[] { 1, 2, 0, 3, 1, 4, 0, 5, 1, 6 };
        static int[] TechMarks = new int[] { 5, 0, 1, 5, 2, 3, 5, 4, 5, 6 };
        static int[] PirateMarks = new int[] { 4, 2, 6, 3, 4, 0, 6, 5, 4, 0 };
        static int[] MercMarks = new int[] { 2, 3, 4, 2, 3, 6, 2, 4, 3, 5 };
        static int[] NoneMarks = new int[] { 0, 1, 2, 3, 4, 5, 6 };
        public static ServerShip GetShip(int pos, byte level, EnemyType enemy)
        {
            EnemyMarks mark = EnemyMarks.Balanced;
            switch (enemy)
            {
                case EnemyType.Aliens: mark = (EnemyMarks)AlienMarks[selectmodel % 10]; break;
                case EnemyType.GreenParty: mark = (EnemyMarks)GreenTeamMarks[selectmodel % 10]; break;
                case EnemyType.Techno: mark = (EnemyMarks)TechMarks[selectmodel % 10]; break;
                case EnemyType.Pirates: mark = (EnemyMarks)PirateMarks[selectmodel % 10]; break;
                case EnemyType.Mercenaries: mark = (EnemyMarks)MercMarks[selectmodel % 10]; break;
                case EnemyType.None: mark = (EnemyMarks)NoneMarks[selectmodel % 7]; break;
            }
            selectmodel++;
            int levelbonus = randomgen % 5 - 2;
            if (level > 8) level += (byte)levelbonus;
            if (level > 50) level = 50;
            randomgen++;
            Schema schema = GetEnemySchema(level, mark, enemy);
            if (schema == null) return null;
            ServerShip ship;
            if (enemy != EnemyType.None)
                ship = new ServerShip(1000 + pos, schema, 100, new byte[] { (byte)((int)enemy + 251), (byte)((randomgen % 3) +1), 0, 0 });
            else
                ship = new ServerShip(1000 + pos, schema, 100, new byte[] { (byte)((randomgen % 11)+1), (byte)((randomgen++) % 10), (byte)((randomgen++ + 3) % 10), (byte)((randomgen++ + 8) % 10) });
            ServerShipB testship = new ServerShipB(ship, 1, ShipSide.Attack, null, 255);
            int needenergy = (int)(testship.Params.HexMoveCost.GetCurrent * 1.5);
            GSPilot.Spec2E s2 = GSPilot.Spec2E.None; GSPilot.Spec3E s3 = GSPilot.Spec3E.None; GSPilot.Spec4E s4 = GSPilot.Spec4E.None;
            bool s5 = false;
            byte talent = (byte)(randomgen % 4); randomgen++;
            byte plevel = (byte)(level / 5);
            switch (enemy)
            {
                case EnemyType.Mercenaries: level += 2; if (level > 10) level = 10; if (talent < 3) talent++; break;
                case EnemyType.Pirates: if (level > 2) level -= 2; if (talent != 0) talent--; break;
            }
            if (needenergy < testship.Params.Energy.GetCurrent)
                s2 = GSPilot.Spec2E.EnergyRegen;
            else if (level > 10)
                switch (mark)
                {
                    case EnemyMarks.TankHealth: s2 = GSPilot.Spec2E.Health; break;
                    case EnemyMarks.TankShield: s2 = GSPilot.Spec2E.Shield; break;
                    case EnemyMarks.DamageAccuracy: s2 = GSPilot.Spec2E.EnergyRegen; break;
                    case EnemyMarks.DamageDPS: s2 = GSPilot.Spec2E.ShieldRegen; break;
                    case EnemyMarks.Evasion: s2 = GSPilot.Spec2E.HealthRegen; break;
                    case EnemyMarks.Support: s2 = GSPilot.Spec2E.EnergyRegen; break;
                    case EnemyMarks.Balanced: s2 = GSPilot.Spec2E.Health; break;
                    default: s2 = GSPilot.Spec2E.Shield; break;
                }
            if (level > 20)
                switch (mark)
                {
                    case EnemyMarks.TankHealth:
                    case EnemyMarks.TankShield:
                        s3 = GSPilot.Spec3E.Ignore; break;
                    case EnemyMarks.DamageAccuracy:
                    case EnemyMarks.DamageDPS:
                    case EnemyMarks.Balanced:
                        s3 = GSPilot.Spec3E.Damage; break;
                    case EnemyMarks.Evasion:
                    case EnemyMarks.Support:
                        s3 = GSPilot.Spec3E.Evasion; break;
                    default: s3 = GSPilot.Spec3E.Ignore; break;
                }
            if (level > 30 || enemy == EnemyType.Techno)
                switch (mark)
                {
                    case EnemyMarks.TankHealth: s4 = GSPilot.Spec4E.Health; break;
                    case EnemyMarks.TankShield: s4 = GSPilot.Spec4E.Shield; break;
                    case EnemyMarks.DamageAccuracy: s4 = GSPilot.Spec4E.Accuracy; break;
                    case EnemyMarks.DamageDPS: s4 = GSPilot.Spec4E.Damage; break;
                    case EnemyMarks.Evasion: s4 = GSPilot.Spec4E.Evasion; break;
                    case EnemyMarks.Support: s4 = GSPilot.Spec4E.Ignore; break;
                    case EnemyMarks.Balanced: s4 = GSPilot.Spec4E.EnergyRegen; break;
                    default: s4 = GSPilot.Spec4E.Health; break;
                }
            if (level > 40)
                s5 = true;
            GSPilot pilot = GSPilot.GetSpecPilot((byte)(level / 5), (byte)schema.Group, talent, s2, s3, s4, s5);
            ship.Pilot = pilot;
            return ship;
        }
    }
}
