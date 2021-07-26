using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class SchemeGenerator
    {
        static Random rnd = new Random();
       
        public static Schema GetEnemySchema(int maxlevel, out WeaponGroup group)
        {
            group = WeaponGroup.Any;
            SortedList<ShipTypeclass, int> ShipTypes = new SortedList<ShipTypeclass, int>();
            foreach (ShipTypeclass type in Links.ShipTypes.Values)
                if (type.Level <= maxlevel)
                {
                    int balls = (type.GeneratorSize == ItemSize.Large ? 4 : type.GeneratorSize == ItemSize.Medium ? 2 : 0);
                    balls += (type.ComputerSize == ItemSize.Large ? 4 : type.ComputerSize == ItemSize.Medium ? 2 : 0);
                    balls += (type.ShieldSize == ItemSize.Large ? 4 : type.ShieldSize == ItemSize.Medium ? 2 : 0);
                    balls += (type.EngineSize == ItemSize.Large ? 2 : type.EngineSize == ItemSize.Medium ? 1 : 0);
                    foreach (WeaponParams weapon in type.WeaponsParam)
                        balls += (1 + 1 * (int)weapon.Size) + (weapon.Group == WeaponGroup.Any ? 1 : 0);
                    foreach (ItemSize size in type.EquipmentsSize)
                        balls += size == ItemSize.Large ? 6 : size == ItemSize.Medium ? 4 : 1;
                    balls += type.Armor / 5;
                    balls = balls * 2 + type.Level;
                    ShipTypes.Add(type, balls);
                }
            int maxball = 0;
            foreach (int val in ShipTypes.Values)
                if (val > maxball) maxball = val;
            maxball = (int)(maxball * 2.0 / 3.0);
            List<ShipTypeclass> ShipList = new List<ShipTypeclass>();
            foreach (KeyValuePair<ShipTypeclass, int> pair in ShipTypes)
                if (pair.Value >= maxball)
                    ShipList.Add(pair.Key);
            if (ShipList.Count == 0) return null;
            int shiptype = rnd.Next(ShipList.Count);
            Schema schema = new Schema();
            schema.SetShipType(ShipList[shiptype]);
            group = schema.ShipType.WeaponsParam[0].Group;
            for (int i = 1; i < schema.ShipType.WeaponCapacity; i++)
            {
                if (schema.ShipType.WeaponsParam[i].Group == group) continue;
                if (group != WeaponGroup.Any) continue;
                group = schema.ShipType.WeaponsParam[i].Group;
            }
            if (group == WeaponGroup.Any)
                group = (WeaponGroup)rnd.Next(4);
            SortedSet<EWeaponType> BadWeaponSet = new SortedSet<EWeaponType>();
            BadWeaponSet.Add(EWeaponType.Laser); BadWeaponSet.Add(EWeaponType.EMI);
            BadWeaponSet.Add(EWeaponType.Plasma); BadWeaponSet.Add(EWeaponType.Cannon);
            BadWeaponSet.Add(EWeaponType.Gauss); BadWeaponSet.Add(EWeaponType.Missle);
            BadWeaponSet.Add(EWeaponType.Slicing); BadWeaponSet.Add(EWeaponType.Drone);
            SortedSet<EWeaponType> NiceWeaponSet = new SortedSet<EWeaponType>();
            NiceWeaponSet.Add(EWeaponType.Solar); NiceWeaponSet.Add(EWeaponType.AntiMatter);
            NiceWeaponSet.Add(EWeaponType.Psi); NiceWeaponSet.Add(EWeaponType.Dark);
            NiceWeaponSet.Add(EWeaponType.Warp); NiceWeaponSet.Add(EWeaponType.Time);
            NiceWeaponSet.Add(EWeaponType.Radiation); NiceWeaponSet.Add(EWeaponType.Magnet);
            for (int i = 0; i < schema.ShipType.WeaponCapacity; i++)
            {
                ItemSize weaponsize = schema.ShipType.WeaponsParam[i].Size;
                schema.SetWeapon(GetWeapon(BadWeaponSet, NiceWeaponSet, group, 0.66, weaponsize, maxlevel), i);
            }
            schema.SetGenerator(GetGenerator(schema.ShipType.GeneratorSize, maxlevel));
            schema.SetShield(GetShield(schema.ShipType.ShieldSize, maxlevel));
            schema.SetComputer(GetComputer(schema.ShipType.ComputerSize, group, maxlevel));
            schema.SetEngine(GetEngine(schema.ShipType.EngineSize, maxlevel));
            for (int i = 0; i < schema.ShipType.EquipmentCapacity; i++)
            {
                SortedSet<EquipmentEffect> NiceEffects = new SortedSet<EquipmentEffect>();
                NiceEffects.Add(EquipmentEffect.Accuracy); NiceEffects.Add(EquipmentEffect.Damage);
                NiceEffects.Add(EquipmentEffect.Health); NiceEffects.Add(EquipmentEffect.ShieldCap);
                NiceEffects.Add(EquipmentEffect.ShieldRes); NiceEffects.Add(EquipmentEffect.Immune);
                NiceEffects.Add(EquipmentEffect.Ignore);
                ItemSize equipsize = schema.ShipType.EquipmentsSize[i];
                if (equipsize == ItemSize.Large)
                {
                    Equipmentclass equip = GetEquipmentDamageLarge(group, schema, NiceEffects, maxlevel);
                    if (equip != null) { schema.SetEquipment(equip, i); continue; }
                }
                Equipmentclass smallequip = GetEquipmentDamage(group, schema, equipsize, NiceEffects, maxlevel);
                schema.SetEquipment(smallequip, i);
            }
            SetArmor(schema);
            return schema;
        }
        public static Schema GetPirateSchema(int maxlevel, out WeaponGroup group)
        {
            group = WeaponGroup.Any;
            SortedList<ShipTypeclass, int> ShipTypes = new SortedList<ShipTypeclass, int>();
            foreach (ShipTypeclass type in Links.ShipTypes.Values)
                if (type.Level<=maxlevel)
                {
                    int balls = (type.GeneratorSize == ItemSize.Large ? 4 : type.GeneratorSize == ItemSize.Medium ? 2 : 0);
                    balls += (type.ComputerSize == ItemSize.Large ? 4 : type.ComputerSize == ItemSize.Medium ? 2 : 0);
                    balls += (type.ShieldSize == ItemSize.Large ? 4 : type.ShieldSize == ItemSize.Medium ? 2 : 0);
                    balls += (type.EngineSize == ItemSize.Large ? 2 : type.EngineSize == ItemSize.Medium ? 1 : 0);
                    foreach (WeaponParams weapon in type.WeaponsParam)
                        balls += (1 + 1 * (int)weapon.Size) + (weapon.Group == WeaponGroup.Any ? 1 : 0);
                    foreach (ItemSize size in type.EquipmentsSize)
                        balls += size == ItemSize.Large ? 6 : size == ItemSize.Medium ? 4 : 1;
                    balls += type.Armor / 5;
                    balls = balls * 2 + type.Level;
                    ShipTypes.Add(type, balls);
                }
            int maxball = 0;
            foreach (int val in ShipTypes.Values)
                if (val > maxball) maxball = val;
            maxball = (int)(maxball * 2.0 / 3.0);
            List<ShipTypeclass> ShipList = new List<ShipTypeclass>();
            foreach (KeyValuePair<ShipTypeclass, int> pair in ShipTypes)
                if (pair.Value >= maxball)
                    ShipList.Add(pair.Key);
            if (ShipList.Count == 0) return null;
            int shiptype = rnd.Next(ShipList.Count);
            Schema schema = new Schema();
            schema.SetShipType(ShipList[shiptype]);
            group = schema.ShipType.WeaponsParam[0].Group;
            for (int i = 1; i < schema.ShipType.WeaponCapacity; i++)
            {
                if (schema.ShipType.WeaponsParam[i].Group == group) continue;
                if (group != WeaponGroup.Any) continue;
                group = schema.ShipType.WeaponsParam[i].Group;
            }
            if (group == WeaponGroup.Any)
                group = (WeaponGroup)rnd.Next(4);
            SortedSet<EWeaponType> BadWeaponSet = new SortedSet<EWeaponType>();
            BadWeaponSet.Add(EWeaponType.EMI); BadWeaponSet.Add(EWeaponType.Solar);
            BadWeaponSet.Add(EWeaponType.AntiMatter); BadWeaponSet.Add(EWeaponType.Psi);
            BadWeaponSet.Add(EWeaponType.Dark); BadWeaponSet.Add(EWeaponType.Warp);
            BadWeaponSet.Add(EWeaponType.Time); BadWeaponSet.Add(EWeaponType.Drone);
            BadWeaponSet.Add(EWeaponType.Magnet); 
            SortedSet<EWeaponType> NiceWeaponSet = new SortedSet<EWeaponType>();
            NiceWeaponSet.Add(EWeaponType.Laser); NiceWeaponSet.Add(EWeaponType.Plasma);
            NiceWeaponSet.Add(EWeaponType.Cannon); NiceWeaponSet.Add(EWeaponType.Gauss);
            NiceWeaponSet.Add(EWeaponType.Missle); NiceWeaponSet.Add(EWeaponType.Slicing);
            NiceWeaponSet.Add(EWeaponType.Radiation);
            for (int i = 0; i < schema.ShipType.WeaponCapacity; i++)
            {
                ItemSize weaponsize = schema.ShipType.WeaponsParam[i].Size;
                schema.SetWeapon(GetWeapon(BadWeaponSet, NiceWeaponSet, group, 0.66, weaponsize, maxlevel), i);
            }
            schema.SetGenerator(GetGenerator(schema.ShipType.GeneratorSize, maxlevel));
            schema.SetShield(GetShield(schema.ShipType.ShieldSize, maxlevel));
            schema.SetComputer(GetComputer(schema.ShipType.ComputerSize, group, maxlevel));
            schema.SetEngine(GetEngine(schema.ShipType.EngineSize, maxlevel));
            for (int i = 0; i < schema.ShipType.EquipmentCapacity; i++)
            {
                SortedSet<EquipmentEffect> NiceEffects = new SortedSet<EquipmentEffect>();
                NiceEffects.Add(EquipmentEffect.Accuracy); NiceEffects.Add(EquipmentEffect.Damage);
                NiceEffects.Add(EquipmentEffect.Health); NiceEffects.Add(EquipmentEffect.ShieldCap);
                NiceEffects.Add(EquipmentEffect.ShieldRes); NiceEffects.Add(EquipmentEffect.Immune);
                NiceEffects.Add(EquipmentEffect.Ignore);
                ItemSize equipsize = schema.ShipType.EquipmentsSize[i];
                if (equipsize == ItemSize.Large)
                {
                    Equipmentclass equip = GetEquipmentDamageLarge(group, schema, NiceEffects, maxlevel);
                    if (equip != null) { schema.SetEquipment(equip, i); continue; }
                }
                Equipmentclass smallequip = GetEquipmentDamage(group, schema, equipsize, NiceEffects, maxlevel);
                schema.SetEquipment(smallequip, i);
            }
            SetArmor(schema);
            return schema;
        }
        public static Schema GetSupportSchema()
        {
            SortedList<ShipTypeclass, int> ShipTypes = new SortedList<ShipTypeclass, int>();
            foreach (ShipTypeclass type in Links.ShipTypes.Values)
                if (GSGameInfo.SciencesArray.Contains(type.ID))
                {
                    if (type.EquipmentsSize[0] != ItemSize.Large) continue;
                    int balls = (type.GeneratorSize == ItemSize.Large ? 4 : type.GeneratorSize == ItemSize.Medium ? 2 : 0);
                    balls += (type.ComputerSize == ItemSize.Large ? 4 : type.ComputerSize == ItemSize.Medium ? 2 : 0);
                    balls += (type.ShieldSize == ItemSize.Large ? 4 : type.ShieldSize == ItemSize.Medium ? 2 : 0);
                    balls += (type.EngineSize == ItemSize.Large ? 4 : type.EngineSize == ItemSize.Medium ? 2 : 0);
                    foreach (WeaponParams weapon in type.WeaponsParam)
                        balls += (2 + 2 * (int)weapon.Size) + (weapon.Group == WeaponGroup.Any ? 2 : 0);
                    balls += type.EquipmentCapacity;
                    balls += type.Armor / 10;
                    if (type.EquipmentCapacity > 1 && type.EquipmentsSize[1] == ItemSize.Large)
                        balls += 20;
                    balls = balls * 2 + type.Level;
                    ShipTypes.Add(type, balls);
                }
            int maxball = 0;
            foreach (int val in ShipTypes.Values)
                if (val > maxball) maxball = val;
            maxball = (int)(maxball * 2.0 / 4.0);
            List<ShipTypeclass> ShipList = new List<ShipTypeclass>();
            foreach (KeyValuePair<ShipTypeclass, int> pair in ShipTypes)
                if (pair.Value >= maxball)
                    ShipList.Add(pair.Key);
            if (ShipList.Count == 0) return null;
            int shiptype = rnd.Next(ShipList.Count);
            Schema schema = new Schema();
            schema.SetShipType(ShipList[shiptype]);
            WeaponGroup group = schema.ShipType.WeaponsParam[0].Group;
            for (int i = 1; i < schema.ShipType.WeaponCapacity; i++)
            {
                if (schema.ShipType.WeaponsParam[i].Group == group) continue;
                if (group != WeaponGroup.Any) continue;
                group = schema.ShipType.WeaponsParam[i].Group;
            }
            if (group == WeaponGroup.Any)
                group = (WeaponGroup)rnd.Next(4);
            SortedSet<EWeaponType> BadWeaponSet = new SortedSet<EWeaponType>();
            BadWeaponSet.Add(EWeaponType.Plasma); BadWeaponSet.Add(EWeaponType.Solar);
            BadWeaponSet.Add(EWeaponType.AntiMatter); BadWeaponSet.Add(EWeaponType.Cannon);
            BadWeaponSet.Add(EWeaponType.Warp); BadWeaponSet.Add(EWeaponType.Dark);
            BadWeaponSet.Add(EWeaponType.Slicing); BadWeaponSet.Add(EWeaponType.Radiation);
            SortedSet<EWeaponType> NiceWeaponSet = new SortedSet<EWeaponType>();
            NiceWeaponSet.Add(EWeaponType.Missle); NiceWeaponSet.Add(EWeaponType.Time);
            NiceWeaponSet.Add(EWeaponType.EMI); NiceWeaponSet.Add(EWeaponType.Drone);
            for (int i = 0; i < schema.ShipType.WeaponCapacity; i++)
            {
                ItemSize weaponsize = schema.ShipType.WeaponsParam[i].Size;
                schema.SetWeapon(GetWeapon(BadWeaponSet, NiceWeaponSet, group, 0.66, weaponsize), i);
            }
            schema.SetGenerator(GetGenerator(schema.ShipType.GeneratorSize));
            schema.SetShield(GetShield(schema.ShipType.ShieldSize));
            schema.SetComputer(GetComputer(schema.ShipType.ComputerSize, group));
            schema.SetEngine(GetEngine(schema.ShipType.EngineSize));
            for (int i = 0; i < schema.ShipType.EquipmentCapacity; i++)
            {
                
                SortedSet<EquipmentEffect> NiceEffects = new SortedSet<EquipmentEffect>();
                NiceEffects.Add(EquipmentEffect.Evasion);
                NiceEffects.Add(EquipmentEffect.Health);
                NiceEffects.Add(EquipmentEffect.ShieldCap);
                NiceEffects.Add(EquipmentEffect.ShieldRes);
                NiceEffects.Add(EquipmentEffect.Damage);
                NiceEffects.Add(EquipmentEffect.EnergyCap);
                ItemSize equipsize = schema.ShipType.EquipmentsSize[i];
                if (equipsize == ItemSize.Large)
                {
                    Equipmentclass equip = GetEquipmentDamageLarge(group, schema, NiceEffects);
                    if (equip != null) { schema.SetEquipment(equip, i); continue; }
                }
                Equipmentclass smallequip = GetEquipmentDamage(group, schema, equipsize, NiceEffects);
                schema.SetEquipment(smallequip, i);
            }
            SetArmor(schema);
            return schema;
        }
        public static Schema GetSpeedShipSchema()
        {
            SortedList<ShipTypeclass, int> ShipTypes = new SortedList<ShipTypeclass, int>();
            foreach (ShipTypeclass type in Links.ShipTypes.Values)
                if (GSGameInfo.SciencesArray.Contains(type.ID))
                {
                    if (type.EngineSize != ItemSize.Large) continue;
                    int balls = (type.GeneratorSize == ItemSize.Large ? 4 : type.GeneratorSize == ItemSize.Medium ? 2 : 0);
                    balls += (type.ComputerSize == ItemSize.Large ? 4 : type.ComputerSize == ItemSize.Medium ? 2 : 0);
                    balls += (type.ShieldSize == ItemSize.Large ? 4 : type.ShieldSize == ItemSize.Medium ? 2 : 0);
                    foreach (WeaponParams weapon in type.WeaponsParam)
                        balls += (2 + 2 * (int)weapon.Size) + (weapon.Group == WeaponGroup.Any ? 2 : 0);
                    balls += type.EquipmentCapacity;
                    balls += type.Armor / 10;
                    balls = balls * 2 + type.Level;
                    ShipTypes.Add(type, balls);
                }
            int maxball = 0;
            foreach (int val in ShipTypes.Values)
                if (val > maxball) maxball = val;
            maxball = (int)(maxball * 2.0 / 4.0);
            List<ShipTypeclass> ShipList = new List<ShipTypeclass>();
            foreach (KeyValuePair<ShipTypeclass, int> pair in ShipTypes)
                if (pair.Value >= maxball)
                    ShipList.Add(pair.Key);
            if (ShipList.Count == 0) return null;
            int shiptype = rnd.Next(ShipList.Count);
            Schema schema = new Schema();
            schema.SetShipType(ShipList[shiptype]);
            WeaponGroup group = schema.ShipType.WeaponsParam[0].Group;
            for (int i = 1; i < schema.ShipType.WeaponCapacity; i++)
            {
                if (schema.ShipType.WeaponsParam[i].Group == group) continue;
                if (group != WeaponGroup.Any) continue;
                group = schema.ShipType.WeaponsParam[i].Group;
            }
            if (group == WeaponGroup.Any)
                group = (WeaponGroup)rnd.Next(4);
            SortedSet<EWeaponType> BadWeaponSet = new SortedSet<EWeaponType>();
            BadWeaponSet.Add(EWeaponType.Missle); BadWeaponSet.Add(EWeaponType.Time);
            BadWeaponSet.Add(EWeaponType.Drone); BadWeaponSet.Add(EWeaponType.Laser);
            BadWeaponSet.Add(EWeaponType.EMI); BadWeaponSet.Add(EWeaponType.Gauss);
            BadWeaponSet.Add(EWeaponType.Psi); BadWeaponSet.Add(EWeaponType.Magnet);
            SortedSet<EWeaponType> NiceWeaponSet = new SortedSet<EWeaponType>();
            NiceWeaponSet.Add(EWeaponType.Plasma); NiceWeaponSet.Add(EWeaponType.AntiMatter);
            NiceWeaponSet.Add(EWeaponType.Warp); NiceWeaponSet.Add(EWeaponType.Solar);
            NiceWeaponSet.Add(EWeaponType.Cannon); NiceWeaponSet.Add(EWeaponType.Dark);
            NiceWeaponSet.Add(EWeaponType.Slicing); NiceWeaponSet.Add(EWeaponType.Radiation);
            for (int i = 0; i < schema.ShipType.WeaponCapacity; i++)
            {
                ItemSize weaponsize = schema.ShipType.WeaponsParam[i].Size;
                schema.SetWeapon(GetWeapon(BadWeaponSet, NiceWeaponSet, group, 0.66, weaponsize), i);
            }
            schema.SetGenerator(GetGenerator(schema.ShipType.GeneratorSize));
            schema.SetShield(GetShield(schema.ShipType.ShieldSize));
            schema.SetComputer(GetComputer(schema.ShipType.ComputerSize, group));
            schema.SetEngine(GetEngine(schema.ShipType.EngineSize));
            for (int i = 0; i < schema.ShipType.EquipmentCapacity; i++)
            {
                SortedSet<EquipmentEffect> NiceEffects = new SortedSet<EquipmentEffect>();
                NiceEffects.Add(EquipmentEffect.Evasion);
                NiceEffects.Add(EquipmentEffect.Health);
                NiceEffects.Add(EquipmentEffect.Damage);
                NiceEffects.Add(EquipmentEffect.EnergyCap);
                ItemSize equipsize = schema.ShipType.EquipmentsSize[i];
                if (equipsize == ItemSize.Large)
                {
                    Equipmentclass equip = GetEquipmentDamageLarge(group, schema, NiceEffects);
                    if (equip != null) { schema.SetEquipment(equip, i); continue; }
                }
                Equipmentclass smallequip = GetEquipmentDamage(group, schema, equipsize, NiceEffects);
                schema.SetEquipment(smallequip, i);
            }
            SetArmor(schema);
            return schema;
        }
        public static Schema GetDefenderSchema()
        {
            SortedList<ShipTypeclass, int> ShipTypes = new SortedList<ShipTypeclass, int>();
            foreach (ShipTypeclass type in Links.ShipTypes.Values)
                if (GSGameInfo.SciencesArray.Contains(type.ID))
                {
                    if (type.ShieldSize != ItemSize.Large) continue;
                    int balls = (type.GeneratorSize == ItemSize.Large ? 4 : type.GeneratorSize == ItemSize.Medium ? 2 : 0);
                    balls += (type.ComputerSize == ItemSize.Large ? 4 : type.ComputerSize == ItemSize.Medium ? 2 : 0);
                    balls += (type.EngineSize == ItemSize.Large ? 4 : type.EngineSize == ItemSize.Medium ? 2 : 0);
                    foreach (WeaponParams weapon in type.WeaponsParam)
                        balls += (2 + 2*(int)weapon.Size) + (weapon.Group == WeaponGroup.Any ? 2 : 0);
                    balls += type.EquipmentCapacity;
                    balls += type.Armor / 5;
                    balls = balls * 2 + type.Level;
                    ShipTypes.Add(type, balls);
                }
            int maxball = 0;
            foreach (int val in ShipTypes.Values)
                if (val > maxball) maxball = val;
            maxball = (int)(maxball * 2.0 / 4.0);
            List<ShipTypeclass> ShipList = new List<ShipTypeclass>();
            foreach (KeyValuePair<ShipTypeclass, int> pair in ShipTypes)
                if (pair.Value >= maxball)
                    ShipList.Add(pair.Key);
            if (ShipList.Count == 0) return null;
            int shiptype = rnd.Next(ShipList.Count);
            Schema schema = new Schema();
            schema.SetShipType(ShipList[shiptype]);
            WeaponGroup group = schema.ShipType.WeaponsParam[0].Group;
            for (int i = 1; i < schema.ShipType.WeaponCapacity; i++)
            {
                if (schema.ShipType.WeaponsParam[i].Group == group) continue;
                if (group != WeaponGroup.Any) continue;
                group = schema.ShipType.WeaponsParam[i].Group;
            }
            if (group == WeaponGroup.Any)
                group = (WeaponGroup)rnd.Next(4);
            SortedSet<EWeaponType> BadWeaponSet = new SortedSet<EWeaponType>();
            BadWeaponSet.Add(EWeaponType.Missle); BadWeaponSet.Add(EWeaponType.Time);
            BadWeaponSet.Add(EWeaponType.Drone);
            SortedSet<EWeaponType> NiceWeaponSet = new SortedSet<EWeaponType>();
            NiceWeaponSet.Add(EWeaponType.Plasma); NiceWeaponSet.Add(EWeaponType.AntiMatter);
            NiceWeaponSet.Add(EWeaponType.Warp); NiceWeaponSet.Add(EWeaponType.Solar);
            NiceWeaponSet.Add(EWeaponType.Cannon); NiceWeaponSet.Add(EWeaponType.Dark);
            NiceWeaponSet.Add(EWeaponType.Slicing); NiceWeaponSet.Add(EWeaponType.Radiation);
            for (int i = 0; i < schema.ShipType.WeaponCapacity; i++)
            {
                ItemSize weaponsize = schema.ShipType.WeaponsParam[i].Size;
                schema.SetWeapon(GetWeapon(BadWeaponSet, NiceWeaponSet, group, 0.66, weaponsize), i);
            }
            schema.SetGenerator(GetGenerator(schema.ShipType.GeneratorSize));
            schema.SetShield(GetShield(schema.ShipType.ShieldSize));
            schema.SetComputer(GetComputer(schema.ShipType.ComputerSize, group));
            schema.SetEngine(GetEngine(schema.ShipType.EngineSize));
            bool hasimmune = false;
            for (int i = 0; i < schema.ShipType.EquipmentCapacity; i++)
            {
                SortedSet<EquipmentEffect> NiceEffects = new SortedSet<EquipmentEffect>();
                NiceEffects.Add(EquipmentEffect.Ignore);
                if (GetShieldCapacity(schema) * 0.7 > GetShieldRestore(schema))
                    NiceEffects.Add(EquipmentEffect.ShieldRes);
                else
                    NiceEffects.Add(EquipmentEffect.ShieldCap);
                if (hasimmune== false)
                {
                    if (GetEnergyCapacity(schema) * 0.5 > GetEnergyRestore(schema)[0])
                        NiceEffects.Add(EquipmentEffect.Immune);
                }
                NiceEffects.Add(EquipmentEffect.Health);
                ItemSize equipsize = schema.ShipType.EquipmentsSize[i];
                if (equipsize == ItemSize.Large)
                {
                    Equipmentclass equip = GetEquipmentDamageLarge(group, schema, NiceEffects);
                    if (equip != null) { schema.SetEquipment(equip, i); continue; }
                }
                Equipmentclass smallequip = GetEquipmentDamage(group, schema, equipsize, NiceEffects);
                schema.SetEquipment(smallequip, i);
                if (schema.Equipments[i].Type > 58) hasimmune = true;
            }
            SetArmor(schema);
            return schema;
        }
        public static Schema GetWarriorSchema()
        {
            SortedList<ShipTypeclass, int> ShipTypes = new SortedList<ShipTypeclass,int>();
            foreach (ShipTypeclass type in Links.ShipTypes.Values)
                if (GSGameInfo.SciencesArray.Contains(type.ID))
                {
                    if (type.GeneratorSize == ItemSize.Small) continue;
                    if (type.ComputerSize == ItemSize.Small) continue;
                    if (type.WeaponCapacity == 1) continue;
                    int balls = 2 + (type.GeneratorSize == ItemSize.Large ? 4 : 0);
                    balls += 2 + (type.ComputerSize == ItemSize.Large ? 4 : 0);
                    foreach (WeaponParams weapon in type.WeaponsParam)
                        balls += (1 + (int)weapon.Size) + (weapon.Group == WeaponGroup.Any ? 2 : 0);
                    balls += type.EquipmentCapacity;
                    balls = balls * 3 + type.Level;
                    ShipTypes.Add(type, balls);
                }
            int maxball = 0;
            foreach (int val in ShipTypes.Values)
                if (val > maxball) maxball = val;
            maxball = (int)(maxball * 2.0 / 4.0);
            List<ShipTypeclass> ShipList = new List<ShipTypeclass>();
            foreach (KeyValuePair<ShipTypeclass, int> pair in ShipTypes)
                if (pair.Value >= maxball)
                    ShipList.Add(pair.Key);
            if (ShipList.Count == 0) return null;
            int shiptype = rnd.Next(ShipList.Count);
            Schema schema = new Schema();
            schema.SetShipType(ShipList[shiptype]);
            WeaponGroup group = schema.ShipType.WeaponsParam[0].Group;
            for (int i = 1; i < schema.ShipType.WeaponCapacity; i++)
            {
                if (schema.ShipType.WeaponsParam[i].Group == group) continue;
                if (group != WeaponGroup.Any) continue;
                group = schema.ShipType.WeaponsParam[i].Group;
            }
            if (group == WeaponGroup.Any)
                group = (WeaponGroup)rnd.Next(4);
            SortedSet<EWeaponType> BadWeaponSet = new SortedSet<EWeaponType>();
            BadWeaponSet.Add(EWeaponType.Plasma); BadWeaponSet.Add(EWeaponType.AntiMatter);
             BadWeaponSet.Add(EWeaponType.Warp);
            SortedSet<EWeaponType> NiceWeaponSet = new SortedSet<EWeaponType>();
            NiceWeaponSet.Add(EWeaponType.Laser); NiceWeaponSet.Add(EWeaponType.EMI);
            NiceWeaponSet.Add(EWeaponType.Gauss); NiceWeaponSet.Add(EWeaponType.Missle);
            NiceWeaponSet.Add(EWeaponType.Time); NiceWeaponSet.Add(EWeaponType.Psi);
            NiceWeaponSet.Add(EWeaponType.Drone); NiceWeaponSet.Add(EWeaponType.Magnet);
            for (int i = 0; i < schema.ShipType.WeaponCapacity; i++)
            {
                ItemSize weaponsize = schema.ShipType.WeaponsParam[i].Size;
                schema.SetWeapon(GetWeapon(BadWeaponSet, NiceWeaponSet, group, 0.66, weaponsize), i);
            }
            schema.SetGenerator(GetGenerator(schema.ShipType.GeneratorSize));
            schema.SetShield(GetShield(schema.ShipType.ShieldSize));
            schema.SetComputer(GetComputer(schema.ShipType.ComputerSize, group));
            schema.SetEngine(GetEngine(schema.ShipType.EngineSize));
            for (int i=0; i<schema.ShipType.EquipmentCapacity;i++)
            {
                SortedSet<EquipmentEffect> NiceEffects = new SortedSet<EquipmentEffect>();
                NiceEffects.Add(EquipmentEffect.Accuracy); NiceEffects.Add(EquipmentEffect.Damage);
                ItemSize equipsize = schema.ShipType.EquipmentsSize[i];
                if (equipsize==ItemSize.Large)
                {
                    Equipmentclass equip = GetEquipmentDamageLarge(group, schema, NiceEffects);
                    if (equip != null) { schema.SetEquipment(equip, i); continue; }
                }
                Equipmentclass smallequip = GetEquipmentDamage(group, schema, equipsize, NiceEffects);
                schema.SetEquipment(smallequip, i);
            }
            SetArmor(schema);
            return schema;
        }
        /*
        public static Equipmentclass GetSupportLargeEquip()
        {
            SortedList<Equipmentclass, int> EquipTypes = new SortedList<Equipmentclass, int>();
            foreach (Equipmentclass equip in Links.EquipmentTypes.Values)
            {
                if (equip.Size != ItemSize.Large) continue;
                if (!GSGameInfo.SciencesList.SciencesArray.Contains(equip.ID)) continue;
            }
        }
        */
        public static void SetArmor(Schema schema)
        {
            if (schema.ShipType == null) return;
            int armor = schema.ShipType.Armor;
            if (armor>120)
            {
                schema.Armor = ArmorType.Balanced;
            }
            else if (armor>60)
            {
                List<ArmorType> list = new List<ArmorType>();
                list.AddRange(new ArmorType[] { ArmorType.Balanced, ArmorType.Traditional, ArmorType.Energy, ArmorType.Heavy });
                schema.Armor = list[rnd.Next(list.Count)];
            }
            else
            {
                List<ArmorType> list = new List<ArmorType>();
                list.AddRange(new ArmorType[] {ArmorType.Traditional, ArmorType.Energy, ArmorType.Heavy, ArmorType.Reflect,
                ArmorType.Composite, ArmorType.Anomal, ArmorType.Cybernetic});
                schema.Armor = list[rnd.Next(list.Count)];
            }
        }
        public static int GetShieldRestore(Schema schema)
        {
            int result = 0;
            if (schema.ShipType == null) return result;
            if (schema.ShipType != null) return result += schema.Shield.Recharge;
            for (int i = 0; i < schema.ShipType.EquipmentCapacity; i++)
            {
                if (schema.Equipments[i] == null) continue;
                if (schema.Equipments[i].Type == 8 || schema.Equipments[i].Type == 33) result += schema.Equipments[i].Value;
            }
            return result;
        }
        public static int GetShieldCapacity (Schema schema)
        {
            int result = 0;
            if (schema.ShipType == null) return result;
            if (schema.ShipType != null) return result += schema.Shield.Capacity;
            for (int i = 0; i < schema.ShipType.EquipmentCapacity; i++)
            {
                if (schema.Equipments[i] == null) continue;
                if (schema.Equipments[i].Type == 7 || schema.Equipments[i].Type == 32) result += schema.Equipments[i].Value;
            }
            return result;
        }
        public static int GetEnergyCapacity(Schema schema)
        {
            int result = 0;
            if (schema.ShipType == null) return result;
            if (schema.Generator != null) result += schema.Generator.Capacity;
            for (int i = 0; i < schema.ShipType.EquipmentCapacity; i++)
            {
                if (schema.Equipments[i] == null) continue;
                if (schema.Equipments[i].Type == 9 || schema.Equipments[i].Type == 34) result += schema.Equipments[i].Value;
            }
            return result;
        }
        public static int[] GetEnergyRestore(Schema schema)
        {
            int[] result = new int[] { 0, 0 };
            int consume = 0;
            if (schema.ShipType == null) return result;
            if (schema.Generator != null) result[0] += schema.Generator.Recharge;
            if (schema.Shield != null) consume += schema.Shield.Consume;
            if (schema.Computer != null) consume += schema.Computer.Consume;
            if (schema.Engine != null) consume += schema.Engine.Consume;
            for (int i=0;i<schema.ShipType.WeaponCapacity;i++)
            {
                if (schema.Weapons[i] == null) continue;
                result[1] += schema.Weapons[i].Consume;
            }
            for (int i = 0; i < schema.ShipType.EquipmentCapacity; i++)
            {
                if (schema.Equipments[i] == null) continue;
                if (schema.Equipments[i].Type == 10 || schema.Equipments[i].Type == 35)
                    result[0] += schema.Equipments[i].Value;
            }
            for (int i = 0; i < schema.ShipType.EquipmentCapacity; i++)
            {
                if (schema.Equipments[i] == null) continue;
                if (schema.Equipments[i].Type < 59) consume += schema.Equipments[i].Consume;
                else consume += result[0] * schema.Equipments[i].Consume / 100;
            }
            result[0] -= consume;
            return result;
        }
       
        static Equipmentclass GetEquipmentDamage(WeaponGroup group, Schema schema, ItemSize size, SortedSet<EquipmentEffect> NiceEffects)
        {
            int[] regenparams = GetEnergyRestore(schema);
            if (regenparams[0] < regenparams[1])
            {
                NiceEffects.Clear();
                int capacityparam = GetEnergyCapacity(schema);
                if (capacityparam * 0.7 > regenparams[0])
                    NiceEffects.Add(EquipmentEffect.EnergyRes);
                else
                    NiceEffects.Add(EquipmentEffect.EnergyCap);
            }
            SortedList<Equipmentclass, int> EquipmentTypes = new SortedList<Equipmentclass, int>();
            foreach (Equipmentclass equip in Links.EquipmentTypes.Values)
            {
                if (!GSGameInfo.SciencesArray.Contains(equip.ID)) continue;
                if (equip.Size > size) continue;
                if (!NiceEffects.Contains(equip.GetEquipEffect())) continue;
                WeaponGroup equipgroup = equip.GetGroup();
                if (equipgroup != WeaponGroup.Any && equipgroup != group) continue;
                int balls = (int)(equip.Level * (equip.Size == ItemSize.Small ? 1.0 : 1.5));
                balls = (int)(balls * (equipgroup == WeaponGroup.Any ? 1.0 : 2.0));

                EquipmentTypes.Add(equip, balls);
            }
            if (EquipmentTypes.Count == 0) return null;
            return GetEquipFromSortedList(EquipmentTypes, 0.66);
        }
        static Equipmentclass GetEquipmentDamageLarge(WeaponGroup group, Schema schema, SortedSet<EquipmentEffect> NiceEffects)
        {
            int[] regenparams = GetEnergyRestore(schema); 
            if (regenparams[0]<regenparams[1])
            {
                int capacityparam = GetEnergyCapacity(schema);
                if (capacityparam * 0.7 > regenparams[0])
                    NiceEffects.Add(EquipmentEffect.EnergyRes);
                else
                    NiceEffects.Add(EquipmentEffect.EnergyCap);
            }
            SortedList<Equipmentclass, int> EquipmentTypes = new SortedList<Equipmentclass, int>();
            foreach (Equipmentclass equip in Links.EquipmentTypes.Values)
            {
                if (!GSGameInfo.SciencesArray.Contains(equip.ID)) continue;
                if (equip.Size != ItemSize.Large) continue;
                if (!NiceEffects.Contains(equip.GetEquipEffect())) continue;
                if (equip.GetGroup() != WeaponGroup.Any && equip.GetGroup() != group) continue;
                EquipmentTypes.Add(equip, equip.Level); 
            }
            if (EquipmentTypes.Count == 0) return null;
            return GetEquipFromSortedList(EquipmentTypes, 0.66);
        }
        
        static Engineclass GetEngine(ItemSize size)
        {
            SortedList<Engineclass, int> EngineTypes = new SortedList<Engineclass, int>();
            foreach (Engineclass engine in Links.EngineTypes.Values)
            {
                if (!GSGameInfo.SciencesArray.Contains(engine.ID)) continue;
                if (engine.Size > size) continue;
                int balls = engine.EnergyEvasion + engine.PhysicEvasion + engine.IrregularEvasion + engine.CyberEvasion;
                balls = (int)(balls * (engine.Size == ItemSize.Large ? 5.0 : engine.Size==ItemSize.Medium ? 2.5 : 1.0));
                EngineTypes.Add(engine, balls);
            }
            int maxballs = 0;
            foreach (int val in EngineTypes.Values)
                if (maxballs < val) maxballs = val;
            maxballs = (int)(maxballs * 2.0 / 3.0);
            List<Engineclass> EngineList = new List<Engineclass>();
            foreach (KeyValuePair<Engineclass, int> pair in EngineTypes)
                if (pair.Value >=maxballs) EngineList.Add(pair.Key);
            return EngineList[rnd.Next(EngineList.Count)];
        }
        static Equipmentclass GetEquipFromSortedList(SortedList<Equipmentclass, int> types, double ballsmodi)
        {
            int maxballs = 0;
            foreach (int val in types.Values)
                if (maxballs < val) maxballs = val;
            maxballs = (int)(maxballs * ballsmodi);
            List<Equipmentclass> List = new List<Equipmentclass>();
            foreach (KeyValuePair<Equipmentclass, int> pair in types)
                if (pair.Value >= maxballs) List.Add(pair.Key);
            return List[rnd.Next(List.Count)];
        }
        static Computerclass GetComputer(ItemSize size, WeaponGroup group)
        {
            SortedList<Computerclass, int> ComputerTypes = new SortedList<Computerclass, int>();
            foreach (Computerclass computer in Links.ComputerTypes.Values)
            {
                if (!GSGameInfo.SciencesArray.Contains(computer.ID)) continue;
                if (computer.Size > size) continue;
                if ((WeaponGroup)computer.GetWeaponGroupValue() != WeaponGroup.Any && (WeaponGroup)computer.GetWeaponGroupValue() != group) continue;
                int balls = 0;
                balls += computer.GetAccuracy()*3 + computer.GetDamage()-computer.Consume;
                balls = (int)(balls * (computer.Size == ItemSize.Large ? 1.2 : computer.Size == ItemSize.Medium ? 1.1 : 1.0));
                ComputerTypes.Add(computer, balls);
            }
            int maxballs = 0;
            foreach (int val in ComputerTypes.Values)
                if (maxballs < val) maxballs = val;
            maxballs = (int)(maxballs * 2.0 / 3.0);
            List<Computerclass> ComputerList = new List<Computerclass>();
            foreach (KeyValuePair<Computerclass, int> pair in ComputerTypes)
                if (pair.Value >= maxballs) ComputerList.Add(pair.Key);
            return ComputerList[rnd.Next(ComputerList.Count)];
        }
        static Shieldclass GetShield(ItemSize size)
        {
            SortedList<Shieldclass, int> ShieldTypes = new SortedList<Shieldclass, int>();
            foreach (Shieldclass shield in Links.ShieldTypes.Values)
            {
                if (!GSGameInfo.SciencesArray.Contains(shield.ID)) continue;
                if (shield.Size > size) continue;
                int balls = 0;
                balls += (int)(shield.Recharge * 1.0);
                balls += shield.Capacity;
                balls = (int)(balls * (shield.Size == ItemSize.Large ? 2.0 : shield.Size == ItemSize.Medium ? 1.5 : 1));
                ShieldTypes.Add(shield, balls);
            }
            int maxballs = 0;
            foreach (int val in ShieldTypes.Values)
                if (maxballs < val) maxballs = val;
            maxballs = (int)(maxballs * 2.0 / 3.0);
            List<Shieldclass> ShieldList = new List<Shieldclass>();
            foreach (KeyValuePair<Shieldclass, int> pair in ShieldTypes)
                if (pair.Value >= maxballs) ShieldList.Add(pair.Key);
            return ShieldList[rnd.Next(ShieldList.Count)];
        }
        static Generatorclass GetGenerator(ItemSize size)
        {
            SortedList<Generatorclass, int> GeneratorTypes = new SortedList<Generatorclass, int>();
            foreach (Generatorclass generator in Links.GeneratorTypes.Values)
            {
                if (!GSGameInfo.SciencesArray.Contains(generator.ID)) continue;
                if (generator.Size > size) continue;
                int balls = 0;
                balls += (int)(generator.Recharge * 1.5);
                balls += generator.Capacity;
                balls = (int)(balls * (generator.Size == ItemSize.Large ? 2.0 : generator.Size == ItemSize.Medium ? 1.5 : 1));
                GeneratorTypes.Add(generator, balls);
            }
            int maxballs = 0;
            foreach (int val in GeneratorTypes.Values)
                if (maxballs < val) maxballs = val;
            maxballs = (int)(maxballs * 2.0 / 3.0);
            List<Generatorclass> GeneratorList = new List<Generatorclass>();
            foreach (KeyValuePair<Generatorclass, int> pair in GeneratorTypes)
                if (pair.Value >= maxballs) GeneratorList.Add(pair.Key);
            return GeneratorList[rnd.Next(GeneratorList.Count)];
        }
        static Weaponclass GetWeapon(SortedSet<EWeaponType> BadWeaponSet, SortedSet<EWeaponType> NiceWeaponSet, WeaponGroup group, double ballsmodi, ItemSize weaponsize)
        {
            SortedList<Weaponclass, int> WeaponTypes = new SortedList<Weaponclass, int>();
            foreach (Weaponclass weapon in Links.WeaponTypes.Values)
            {
                if (!GSGameInfo.SciencesArray.Contains(weapon.ID)) continue;
                if (group != weapon.Group) continue;
                if (weapon.Size > weaponsize) continue;
                if (BadWeaponSet.Contains(weapon.Type)) continue;
                int balls = 0;
                balls += (weapon.HealthDamage + weapon.ShieldDamage) / 20 + 1;
                if (NiceWeaponSet.Contains(weapon.Type)) balls = (int)(balls * 1.5);
                balls = (int)(balls * (weapon.Size == ItemSize.Large ? 2 : weapon.Size == ItemSize.Medium ? 1.5 : 1));

                WeaponTypes.Add(weapon, balls);
            }
            int maxball = 0;
            foreach (int val in WeaponTypes.Values)
                if (maxball < val) maxball = val;
            maxball = (int)(maxball * ballsmodi);
            List<Weaponclass> WeaponList = new List<Weaponclass>();
            foreach (KeyValuePair<Weaponclass, int> pair in WeaponTypes)
                if (pair.Value >= maxball) WeaponList.Add(pair.Key);
            return WeaponList[rnd.Next(WeaponList.Count)];
        }
        static Weaponclass GetWeapon(SortedSet<EWeaponType> BadWeaponSet, SortedSet<EWeaponType> NiceWeaponSet, WeaponGroup group, double ballsmodi, ItemSize weaponsize, int maxlevel)
        {
            SortedList<Weaponclass, int> WeaponTypes = new SortedList<Weaponclass, int>();
            foreach (Weaponclass weapon in Links.WeaponTypes.Values)
            {
                if (weapon.Level>maxlevel) continue;
                if (group != weapon.Group) continue;
                if (weapon.Size > weaponsize) continue;
                if (BadWeaponSet.Contains(weapon.Type)) continue;
                int balls = 0;
                balls += (weapon.HealthDamage + weapon.ShieldDamage) / 20 + 1;
                if (NiceWeaponSet.Contains(weapon.Type)) balls = (int)(balls * 1.5);
                balls = (int)(balls * (weapon.Size == ItemSize.Large ? 2 : weapon.Size == ItemSize.Medium ? 1.5 : 1));

                WeaponTypes.Add(weapon, balls);
            }
            int maxball = 0;
            foreach (int val in WeaponTypes.Values)
                if (maxball < val) maxball = val;
            maxball = (int)(maxball * ballsmodi);
            List<Weaponclass> WeaponList = new List<Weaponclass>();
            foreach (KeyValuePair<Weaponclass, int> pair in WeaponTypes)
                if (pair.Value >= maxball) WeaponList.Add(pair.Key);
            if (WeaponList.Count == 0) return null;
            return WeaponList[rnd.Next(WeaponList.Count)];
        }
        static Engineclass GetEngine(ItemSize size, int maxlevel)
        {
            SortedList<Engineclass, int> EngineTypes = new SortedList<Engineclass, int>();
            foreach (Engineclass engine in Links.EngineTypes.Values)
            {
                if (engine.Level>maxlevel) continue;
                if (engine.Size > size) continue;
                int balls = engine.EnergyEvasion + engine.PhysicEvasion + engine.IrregularEvasion + engine.CyberEvasion;
                balls = (int)(balls * (engine.Size == ItemSize.Large ? 5.0 : engine.Size == ItemSize.Medium ? 2.5 : 1.0));
                EngineTypes.Add(engine, balls);
            }
            int maxballs = 0;
            foreach (int val in EngineTypes.Values)
                if (maxballs < val) maxballs = val;
            maxballs = (int)(maxballs * 2.0 / 3.0);
            List<Engineclass> EngineList = new List<Engineclass>();
            foreach (KeyValuePair<Engineclass, int> pair in EngineTypes)
                if (pair.Value >= maxballs) EngineList.Add(pair.Key);
            return EngineList[rnd.Next(EngineList.Count)];
        }
        
        static Computerclass GetComputer(ItemSize size, WeaponGroup group, int maxlevel)
        {
            SortedList<Computerclass, int> ComputerTypes = new SortedList<Computerclass, int>();
            foreach (Computerclass computer in Links.ComputerTypes.Values)
            {
                if (computer.Level>maxlevel) continue;
                if (computer.Size > size) continue;
                if ((WeaponGroup)computer.GetWeaponGroupValue() != WeaponGroup.Any && (WeaponGroup)computer.GetWeaponGroupValue() != group) continue;
                int balls = 0;
                balls += computer.GetAccuracy() * 3 + computer.GetDamage() - computer.Consume;
                balls = (int)(balls * (computer.Size == ItemSize.Large ? 1.2 : computer.Size == ItemSize.Medium ? 1.1 : 1.0));
                ComputerTypes.Add(computer, balls);
            }
            int maxballs = 0;
            foreach (int val in ComputerTypes.Values)
                if (maxballs < val) maxballs = val;
            maxballs = (int)(maxballs * 2.0 / 3.0);
            List<Computerclass> ComputerList = new List<Computerclass>();
            foreach (KeyValuePair<Computerclass, int> pair in ComputerTypes)
                if (pair.Value >= maxballs) ComputerList.Add(pair.Key);
            return ComputerList[rnd.Next(ComputerList.Count)];
        }
        static Shieldclass GetShield(ItemSize size, int maxlevel)
        {
            SortedList<Shieldclass, int> ShieldTypes = new SortedList<Shieldclass, int>();
            foreach (Shieldclass shield in Links.ShieldTypes.Values)
            {
                if (shield.Level>maxlevel) continue;
                if (shield.Size > size) continue;
                int balls = 0;
                balls += (int)(shield.Recharge * 1.0);
                balls += shield.Capacity;
                balls = (int)(balls * (shield.Size == ItemSize.Large ? 2.0 : shield.Size == ItemSize.Medium ? 1.5 : 1));
                ShieldTypes.Add(shield, balls);
            }
            int maxballs = 0;
            foreach (int val in ShieldTypes.Values)
                if (maxballs < val) maxballs = val;
            maxballs = (int)(maxballs * 2.0 / 3.0);
            List<Shieldclass> ShieldList = new List<Shieldclass>();
            foreach (KeyValuePair<Shieldclass, int> pair in ShieldTypes)
                if (pair.Value >= maxballs) ShieldList.Add(pair.Key);
            return ShieldList[rnd.Next(ShieldList.Count)];
        }
        static Generatorclass GetGenerator(ItemSize size, int maxlevel)
        {
            SortedList<Generatorclass, int> GeneratorTypes = new SortedList<Generatorclass, int>();
            foreach (Generatorclass generator in Links.GeneratorTypes.Values)
            {
                if (generator.Level>maxlevel) continue;
                if (generator.Size > size) continue;
                int balls = 0;
                balls += (int)(generator.Recharge * 1.5);
                balls += generator.Capacity;
                balls = (int)(balls * (generator.Size == ItemSize.Large ? 2.0 : generator.Size == ItemSize.Medium ? 1.5 : 1));
                GeneratorTypes.Add(generator, balls);
            }
            int maxballs = 0;
            foreach (int val in GeneratorTypes.Values)
                if (maxballs < val) maxballs = val;
            maxballs = (int)(maxballs * 2.0 / 3.0);
            List<Generatorclass> GeneratorList = new List<Generatorclass>();
            foreach (KeyValuePair<Generatorclass, int> pair in GeneratorTypes)
                if (pair.Value >= maxballs) GeneratorList.Add(pair.Key);
            return GeneratorList[rnd.Next(GeneratorList.Count)];
        }
        static Equipmentclass GetEquipmentDamageLarge(WeaponGroup group, Schema schema, SortedSet<EquipmentEffect> NiceEffects, int maxlevel)
        {
            int[] regenparams = GetEnergyRestore(schema);
            if (regenparams[0] < regenparams[1])
            {
                int capacityparam = GetEnergyCapacity(schema);
                if (capacityparam * 0.7 > regenparams[0])
                    NiceEffects.Add(EquipmentEffect.EnergyRes);
                else
                    NiceEffects.Add(EquipmentEffect.EnergyCap);
            }
            SortedList<Equipmentclass, int> EquipmentTypes = new SortedList<Equipmentclass, int>();
            foreach (Equipmentclass equip in Links.EquipmentTypes.Values)
            {
                if (equip.Level>maxlevel) continue;
                if (equip.Size != ItemSize.Large) continue;
                if (!NiceEffects.Contains(equip.GetEquipEffect())) continue;
                if (equip.GetGroup() != WeaponGroup.Any && equip.GetGroup() != group) continue;
                EquipmentTypes.Add(equip, equip.Level);
            }
            if (EquipmentTypes.Count == 0) return null;
            return GetEquipFromSortedList(EquipmentTypes, 0.66);
        }
        static Equipmentclass GetEquipmentDamage(WeaponGroup group, Schema schema, ItemSize size, SortedSet<EquipmentEffect> NiceEffects, int maxlevel)
        {
            int[] regenparams = GetEnergyRestore(schema);
            if (regenparams[0] < regenparams[1])
            {
                NiceEffects.Clear();
                int capacityparam = GetEnergyCapacity(schema);
                if (capacityparam * 0.7 > regenparams[0])
                    NiceEffects.Add(EquipmentEffect.EnergyRes);
                else
                    NiceEffects.Add(EquipmentEffect.EnergyCap);
            }
            SortedList<Equipmentclass, int> EquipmentTypes = new SortedList<Equipmentclass, int>();
            foreach (Equipmentclass equip in Links.EquipmentTypes.Values)
            {
                if (equip.Level>maxlevel) continue;
                if (equip.Size > size) continue;
                if (!NiceEffects.Contains(equip.GetEquipEffect())) continue;
                WeaponGroup equipgroup = equip.GetGroup();
                if (equipgroup != WeaponGroup.Any && equipgroup != group) continue;
                int balls = (int)(equip.Level * (equip.Size == ItemSize.Small ? 1.0 : 1.5));
                balls = (int)(balls * (equipgroup == WeaponGroup.Any ? 1.0 : 2.0));

                EquipmentTypes.Add(equip, balls);
            }
            if (EquipmentTypes.Count == 0) return null;
            return GetEquipFromSortedList(EquipmentTypes, 0.66);
        }
    }
}
