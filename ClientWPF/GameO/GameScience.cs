using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Client
{
    //public enum EScienceField { Energy, Physic, Irregular, Cybernetic, None };
    public enum ScienceType
    {
       
        Ship1Gun, Ship2Gun, Ship3Gun,
        Laser, Emi, Plasma, Solar, Cannon, Gauss, Missle, Anti,
        Psi, Dark, Warp, Time, Slice, Rad, Drone, Magnet,
        GeneratorS, GeneratorM, GeneratorL,
        ShieldS, ShieldM, ShieldL,
        ComputerEn, ComputerPh, ComputerIr, ComputerCy, ComputerAl,
        EngineS, EngineM, EngineL,
        ModuleHealth, ModuleShield, ModuleEnergy,
        ModuleHRestore, ModuleSRestore, ModuleERestore,
        ModuleAccuracy, ModuleEvasion, ModuleDamage, ModuleIgnore, ModuleImmune,
             None
    }
    enum Elements
    {
        Other, ShipScount, ShipCorvette, ShipTransport, ShipBattle, ShipFrigate, ShipFighter, ShipDreadnought,
        ShipDevostator, ShipWarrior, ShipCruiser,
        Laser, Emi, Plasma, Solar, Cannon, Gauss, Missle, Anti, 
        Psi, Dark, Warp, Time, Slice, Rad, Drone, Magnet,
        GeneratorCap, GeneratorRegen, 
        ShieldCap, ShieldRegen, 
        ComputerBasic, ComputerDamEn, ComputerDamPh, ComputerDamIr, ComputerDamCy,
        ComputerAccEn, ComputerAccPh, ComputerAccIr, ComputerAccCy,
        EngineBasic, EngineUniverse, 
        Health, Shield, Energy, HealthRestore, ShieldRestore, EnergyRestore,
        AccuracyTotal, AccuracyEnergy, AccuracyPhysic, AccuracyIrregular, AccuracyCyber, 
        EvasionTotal, EvasionEnergy, EvasionPhysic, EvasionIrregular, EvasionCyber,
        IgnoreTotal, IgnoreEnergy, IgnorePhysic, IgnoreIrregular, IgnoreCyber,
        DamageTotal, DamageEnergy, DamagePhysic, DamageIrregular, DamageCyber,
        ImmuneTotal, ImmuneEnergy, ImmunePhysic, ImmuneIrregular, ImmuneCyber, 
        Cargo, Colony,
        BuildLive, BuildBank,  BuildMine, BuildChipFactory, BuildParticiple,
        BuildMetalCap, BuildChipCap, BuildAntiCap, BuildManufact, BuildRadar, BuildData, BuildEducat,
        GeneratorCapSize, GeneratorRegenSize, ShieldCapSize, ShieldRegenSize, EngineBasicSize, EngineUniverseSize,
    }
    class GameScience
    {
        public bool IsRare { get; private set; }
        public bool NotLearned = false;
        public string Name { get; set; }
        public ushort ID { get; private set; }
        public byte Level { get; private set; }
        public Links.Science.EType Type { get; private set; }
        public Elements Element;
        public ItemSize Size;
        public ScienceType SType;
        public GameScience(ushort id, string name, byte level, bool isRare, Links.Science.EType type)
        {
            ID = id; Name = name; Level = level;  IsRare = isRare; Type = type;
        }
        public static SortedList<ushort, GameScience> GetList(byte[] array)
        {
            SortedList<ushort, GameScience> list = new SortedList<ushort, GameScience>();
            for (int i = 0; i < array.Length; )
            {
                ushort id = BitConverter.ToUInt16(array, i); i += 2;
                byte level = array[i]; i++;
                bool israre = BitConverter.ToBoolean(array, i); i++;
                Links.Science.EType type = (Links.Science.EType)array[i]; i++;
                list.Add(id, new GameScience(id, "", level, israre, type));
                
            }
            return list;
        }
        public static void SetElement()
        {
            foreach (GameScience s in Links.Science.GameSciences.Values)
            {
                int shortid = s.ID % 1000;
                s.SType = ScienceType.None;
                switch (shortid)
                {
                    case 200: s.Element = Elements.ShipScount; s.Size = ItemSize.Any; s.SType = ScienceType.Ship1Gun; break;
                    case 205: s.Element = Elements.ShipFrigate; s.Size = ItemSize.Any; s.SType = ScienceType.Ship1Gun; break;
                    case 210: s.Element = Elements.ShipTransport; s.Size = ItemSize.Any; s.SType = ScienceType.Ship1Gun; break;
                    case 215: s.Element = Elements.ShipBattle; s.Size = ItemSize.Any; s.SType = ScienceType.Ship2Gun; break;
                    case 220: s.Element = Elements.ShipFrigate; s.Size = ItemSize.Any; s.SType = ScienceType.Ship2Gun; break;
                    case 225: s.Element = Elements.ShipFighter; s.Size = ItemSize.Any; s.SType = ScienceType.Ship2Gun; break;
                    case 230: s.Element = Elements.ShipDreadnought; s.Size = ItemSize.Any; s.SType = ScienceType.Ship2Gun; break;
                    case 235: s.Element = Elements.ShipDevostator; s.Size = ItemSize.Any;s.SType = ScienceType.Ship3Gun;  break;
                    case 240: s.Element = Elements.ShipWarrior; s.Size = ItemSize.Any; s.SType = ScienceType.Ship3Gun; break;
                    case 245: s.Element = Elements.ShipCruiser; s.Size = ItemSize.Any; s.SType = ScienceType.Ship3Gun; break;

                    default: s.Element = Elements.Other; s.Size = ItemSize.Any; break;
                }
                switch (s.Type)
                {
                    case Links.Science.EType.Building:
                        GSBuilding building = Links.Buildings[s.ID];
                        switch (building.Sector)
                        {
                            case SectorTypes.Live: s.Element = Elements.BuildLive; break;
                            case SectorTypes.Money: s.Element = Elements.BuildBank; break;
                            case SectorTypes.Metall: s.Element = Elements.BuildMine; break;
                            case SectorTypes.MetalCap: s.Element = Elements.BuildMetalCap; break;
                            case SectorTypes.Chips: s.Element = Elements.BuildChipFactory; break;
                            case SectorTypes.ChipsCap: s.Element = Elements.BuildChipCap; break;
                            case SectorTypes.Anti: s.Element = Elements.BuildParticiple; break;
                            case SectorTypes.AntiCap: s.Element = Elements.BuildAntiCap; break;
                            case SectorTypes.Repair: s.Element = Elements.BuildManufact; break;
                            case SectorTypes.War: s.Element = Elements.BuildRadar; break;
                        }
                        break;
                    case Links.Science.EType.Weapon:
                        Weaponclass weapon = Links.WeaponTypes[s.ID];
                        s.Size = weapon.Size;
                        switch (weapon.Type)
                        {
                            case EWeaponType.Laser: s.Element = Elements.Laser; s.SType = ScienceType.Laser; break;
                            case EWeaponType.EMI: s.Element = Elements.Emi; s.SType = ScienceType.Emi; break;
                            case EWeaponType.Plasma: s.Element = Elements.Plasma; s.SType = ScienceType.Plasma; break;
                            case EWeaponType.Solar: s.Element = Elements.Solar; s.SType = ScienceType.Solar; break;
                            case EWeaponType.Cannon: s.Element = Elements.Cannon; s.SType = ScienceType.Cannon; break;
                            case EWeaponType.Gauss: s.Element = Elements.Gauss; s.SType = ScienceType.Gauss; break;
                            case EWeaponType.Missle: s.Element = Elements.Missle; s.SType = ScienceType.Missle; break;
                            case EWeaponType.AntiMatter: s.Element = Elements.Anti; s.SType = ScienceType.Anti; break;
                            case EWeaponType.Psi: s.Element = Elements.Psi; s.SType = ScienceType.Psi; break;
                            case EWeaponType.Dark: s.Element = Elements.Dark; s.SType = ScienceType.Dark; break;
                            case EWeaponType.Warp: s.Element = Elements.Warp; s.SType = ScienceType.Warp; break;
                            case EWeaponType.Time: s.Element = Elements.Time;s.SType = ScienceType.Time;  break;
                            case EWeaponType.Slicing: s.Element = Elements.Slice; s.SType = ScienceType.Slice; break;
                            case EWeaponType.Radiation: s.Element = Elements.Rad; s.SType = ScienceType.Rad; break;
                            case EWeaponType.Drone: s.Element = Elements.Drone; s.SType = ScienceType.Drone; break;
                            case EWeaponType.Magnet: s.Element = Elements.Magnet; s.SType = ScienceType.Magnet; break;
                        }
                        break;
                    case Links.Science.EType.Generator:
                        Generatorclass generator = Links.GeneratorTypes[s.ID];
                        s.Size = generator.Size;
                        int gensizedescr = generator.GetSizeDescription();
                        switch (gensizedescr%10)
                        {
                            case 5: s.Element = Elements.GeneratorCap; break;
                            case 0: s.Element = Elements.GeneratorRegen; break;
                        }
                        switch (generator.Size)
                        {
                            case ItemSize.Small: s.SType = ScienceType.GeneratorS; break;
                            case ItemSize.Medium: s.SType = ScienceType.GeneratorM; break;
                            case ItemSize.Large: s.SType = ScienceType.GeneratorL; break;
                        }
                        break;
                    case Links.Science.EType.Shield:
                        Shieldclass shield = Links.ShieldTypes[s.ID];
                        s.Size = shield.Size;
                        int shieldsizedescr = shield.GetSizeDescription();
                        switch (shieldsizedescr%10)
                        {
                            case 5:s.Element = Elements.ShieldCap; break;
                            case 0: s.Element = Elements.ShieldRegen; break;
                        }
                        switch (shield.Size)
                        {
                            case ItemSize.Small: s.SType = ScienceType.ShieldS; break;
                            case ItemSize.Medium: s.SType = ScienceType.ShieldM; break;
                            case ItemSize.Large: s.SType = ScienceType.ShieldL; break;
                        }
                        break;
                    case Links.Science.EType.Computer:
                        Computerclass computer = Links.ComputerTypes[s.ID];
                        s.Size = computer.Size;
                        int compdescr = computer.GetType();
                        switch (compdescr)
                        {
                            case 0: s.Element = Elements.ComputerBasic; s.SType = ScienceType.ComputerAl; break;
                            case 1: s.Element = Elements.ComputerAccEn; s.SType = ScienceType.ComputerEn; break;
                            case 2: s.Element = Elements.ComputerAccPh; s.SType = ScienceType.ComputerPh; break;
                            case 3: s.Element = Elements.ComputerAccIr; s.SType = ScienceType.ComputerIr; break;
                            case 4: s.Element = Elements.ComputerAccCy; s.SType = ScienceType.ComputerCy; break;
                            case 5: s.Element = Elements.ComputerDamEn; s.SType = ScienceType.ComputerEn; break;
                            case 6: s.Element = Elements.ComputerDamPh; s.SType = ScienceType.ComputerPh; break;
                            case 7: s.Element = Elements.ComputerDamIr; s.SType = ScienceType.ComputerIr; break;
                            case 8: s.Element = Elements.ComputerDamCy; s.SType = ScienceType.ComputerCy; break;
                        }
                        break;
                    case Links.Science.EType.Engine:
                        Engineclass engine = Links.EngineTypes[s.ID];
                        s.Size = engine.Size;
                        int enginedescr = engine.GetSizeDescription();
                        switch (enginedescr%10)
                        {
                            case 5: s.Element = Elements.EngineBasic; break;
                            case 0: s.Element = Elements.EngineUniverse; break;
                        }
                        switch (engine.Size)
                        {
                            case ItemSize.Small: s.SType = ScienceType.EngineS; break;
                            case ItemSize.Medium: s.SType = ScienceType.EngineM; break;
                            case ItemSize.Large: s.SType = ScienceType.EngineL; break;
                        }
                        break;
                        
                    case Links.Science.EType.Equipment:
                        Equipmentclass equip = Links.EquipmentTypes[s.ID];
                        s.Size = equip.Size;
                        switch (equip.Type)
                        {
                            case 0: case 36: s.Element = Elements.AccuracyEnergy; s.SType = ScienceType.ModuleAccuracy; break;
                            case 1: case 37: s.Element = Elements.AccuracyPhysic; s.SType = ScienceType.ModuleAccuracy; break;
                            case 2: case 38: s.Element = Elements.AccuracyIrregular; s.SType = ScienceType.ModuleAccuracy; break;
                            case 3: case 39: s.Element = Elements.AccuracyCyber; s.SType = ScienceType.ModuleAccuracy; break;
                            case 4: case 40: s.Element = Elements.AccuracyTotal; s.SType = ScienceType.ModuleAccuracy; break;
                            case 5: case 30: s.Element = Elements.Health; s.SType = ScienceType.ModuleHealth; break;
                            case 6: case 31: s.Element = Elements.HealthRestore; s.SType = ScienceType.ModuleHRestore; break;
                            case 7: case 32: s.Element = Elements.Shield; s.SType = ScienceType.ModuleShield; break;
                            case 8: case 33: s.Element = Elements.ShieldRestore; s.SType = ScienceType.ModuleSRestore; break;
                            case 9: case 34: s.Element = Elements.Energy; s.SType = ScienceType.ModuleEnergy;  break;
                            case 10: case 35: s.Element = Elements.EnergyRestore; s.SType = ScienceType.ModuleERestore; break;
                            case 11: case 41: s.Element = Elements.EvasionEnergy; s.SType = ScienceType.ModuleEvasion; break;
                            case 12: case 42: s.Element = Elements.EvasionPhysic; s.SType = ScienceType.ModuleEvasion; break;
                            case 13: case 43: s.Element = Elements.EvasionIrregular; s.SType = ScienceType.ModuleEvasion; break;
                            case 14: case 44: s.Element = Elements.EvasionCyber; s.SType = ScienceType.ModuleEvasion; break;
                            case 15: case 45: s.Element = Elements.EvasionTotal; s.SType = ScienceType.ModuleEvasion; break;
                            case 16: case 46: s.Element = Elements.IgnoreEnergy; s.SType = ScienceType.ModuleIgnore; break;
                            case 17: case 47: s.Element = Elements.IgnorePhysic; s.SType = ScienceType.ModuleIgnore; break;
                            case 18: case 48: s.Element = Elements.IgnoreIrregular; s.SType = ScienceType.ModuleIgnore; break;
                            case 19: case 49: s.Element = Elements.IgnoreCyber; s.SType = ScienceType.ModuleIgnore; break;
                            case 20: case 50: s.Element = Elements.IgnoreTotal; s.SType = ScienceType.ModuleIgnore; break;
                            case 21: case 51: s.Element = Elements.DamageEnergy; s.SType = ScienceType.ModuleDamage; break;
                            case 22: case 52: s.Element = Elements.DamagePhysic; s.SType = ScienceType.ModuleDamage; break;
                            case 23: case 53: s.Element = Elements.DamageIrregular; s.SType = ScienceType.ModuleDamage; break;
                            case 24: case 54: s.Element = Elements.DamageCyber; s.SType = ScienceType.ModuleDamage; break;
                            case 25: case 55: s.Element = Elements.DamageTotal; s.SType = ScienceType.ModuleDamage; break;
                            case 59: s.Element = Elements.ImmuneEnergy; s.SType = ScienceType.ModuleImmune; break;
                            case 60: s.Element = Elements.ImmunePhysic; s.SType = ScienceType.ModuleImmune; break;
                            case 61: s.Element = Elements.ImmuneIrregular; s.SType = ScienceType.ModuleImmune; break;
                            case 62: s.Element = Elements.ImmuneCyber; s.SType = ScienceType.ModuleImmune; break;
                            case 63: s.Element = Elements.ImmuneTotal; s.SType = ScienceType.ModuleImmune; break;
                            case 64: s.Element = Elements.ImmuneEnergy; s.SType = ScienceType.ModuleImmune; break;
                            case 65: s.Element = Elements.ImmunePhysic; s.SType = ScienceType.ModuleImmune; break;
                            case 66: s.Element = Elements.ImmuneIrregular; s.SType = ScienceType.ModuleImmune; break;
                            case 67: s.Element = Elements.ImmuneCyber; s.SType = ScienceType.ModuleImmune; break;
                            case 57: s.Element = Elements.Cargo; break;
                            case 58: s.Element = Elements.Colony; break;
                        }
                        break;
                }
            }
        }
        public override string ToString()
        {
            return Name;
        }

        public static void SetBasicScienceList() //Метод создаёт список исследований, доступных игроку сначала.
        {
            ServerLinks.Science.BasicScienceList = new List<int>();
            //foreach (ushort id in Links.Science.GameSciences.Keys)
            //    ServerLinks.Science.BasicScienceList.Add(id);
            ServerLinks.Science.BasicScienceList.AddRange(new int[] {
                2200, 1205, 210, 3215, //корабли
            300,310,340,350,1360,1320, //Пушки
            250,2251,4252, //Генераторы
            260,1261,3262, //Щиты
            280,1280,2280,//Вычислители
            275,2276,4277, //Двигатели
            550,2550,4550, //Модули корпуса
            570,2570,4570, //Модули щита
            590,2590,4590, //Модули энергии
            1560,3560,5560, //Модули регена корпуса
            1580,3580,5580, //Модули регена щита
            1600,3600,5600, //Модули регена энергии
            500,1510,2540, 3500,4510,5540,//Модули меткости 
            610,1620,2650,3610,4620,5650, //Модули уклонения
            660,1670,2700,3660,4670,5700, //Модули брони
            710,1720,2750,3710,4720,5750, //Модули урона
            760,1770,2800,3760,4770,5800 //Модули иммунитета
            
            });
        }
    }
    
}
