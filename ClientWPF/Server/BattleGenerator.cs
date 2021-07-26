using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class SideBattleParam
    {
        public byte[] Image;
        public ShipBattleParam[] Ships;
        public List<byte> ShipsHexes = new List<byte>();
        public bool IsRessurect = false;
        public SideBattleParam(string text)
        {
            Image = new byte[4];
            string[] texts = text.Split(' ');
            foreach (string t in texts)
            {
                string[] parms = t.Split(':');
                switch (parms[0])
                {
                    case "IMG": Image = GetBytes(parms[1]); break;
                }
            }
        }
        byte[] GetBytes(string s)
        {
            string[] split = s.Split(',');
            List<byte> list = new List<byte>();
            foreach (string ss in split)
                list.Add(Byte.Parse(ss));
            return list.ToArray();
        }
    }
    public enum StoryShipType { StandardShip, Portal, CargoShip, BigShip, SmallBuilding, LargeBuilding }
    public class ShipBattleParam
    {
        public StoryShipType Type = StoryShipType.StandardShip;
        public byte[] Image = new byte[4];
        public byte[] Name = new byte[4];
        public int[] Health = new int[2];
        public int[] Shield = new int[3];
        public int[] Energy = new int[2];
        public int[] Evasion = new int[4];
        public int[] Ignore = new int[4];
        public int[] Accuracy = new int[4];
        public int[] Increase = new int[4];
        public int[] Immune = new int[4];
        public List<int[]> GroupEffects = new List<int[]>();
        public int TimeToEnter = 3;
        public int JumpDistance = 1;
        public int HexMoveCost = 999;
        public byte Hex = 255;
        public WeaponBattleParam[] Weapons = new WeaponBattleParam[3];
        public bool Protected = false;
        public ShipBattleParam()
        {

        }
        public ShipBattleParam(string text)
        {
            string[] texts = text.Split(' ');
            foreach (string t in texts)
            {
                string[] parms = t.Split(':');
                switch (parms[0])
                {
                    case "TYPE": Type = GetType(parms[1]); break;
                    case "IMG": Image = GetBytes(parms[1]); break;
                    case "NAME": Name = GetBytes(parms[1]); break;
                    case "HE": Health = GetInts(parms[1]); break;
                    case "SH": Shield = GetInts(parms[1]); break;
                    case "EN": Energy = GetInts(parms[1]); break;
                    case "W1": Weapons[0] = new WeaponBattleParam(parms[1]); break;
                    case "W2": Weapons[1] = new WeaponBattleParam(parms[1]); break;
                    case "W3": Weapons[2] = new WeaponBattleParam(parms[1]); break;
                    case "MC": HexMoveCost = Int32.Parse(parms[1]); break;
                    case "JD": JumpDistance = Int32.Parse(parms[1]); break;
                    case "WT": TimeToEnter = Int32.Parse(parms[1]); break;
                    case "HEX": Hex = byte.Parse(parms[1]); break;
                    case "EVA": Evasion = GetInts(parms[1]); break;
                    case "ARM": Ignore = GetInts(parms[1]); break;
                    case "ACC": Accuracy = GetInts(parms[1]); break;
                    case "IMM": Immune = GetInts(parms[1]); break;
                    case "B1": GroupEffects.Add(GetGroupEffect(parms[1])); break;
                    case "B2": GroupEffects.Add(GetGroupEffect(parms[1])); break;
                    case "B3": GroupEffects.Add(GetGroupEffect(parms[1])); break;
                }
            }
        }
        StoryShipType GetType(string text)
        {
            switch (text)
            {
                case "S": return StoryShipType.StandardShip;
                case "P": return StoryShipType.Portal;
                default: throw new Exception();
            }
        }
        byte[] GetBytes(string s)
        {
            string[] split = s.Split(',');
            List<byte> list = new List<byte>();
            foreach (string ss in split)
                list.Add(Byte.Parse(ss));
            return list.ToArray();
        }
        int[] GetInts(string s)
        {
            string[] split = s.Split(',');
            List<int> list = new List<int>();
            foreach (string ss in split)
                list.Add(Int32.Parse(ss));
            return list.ToArray();
        }
        int[] GetGroupEffect(string s)
        {
            string[] split = s.Split(',');
            switch (split[0])
            {
                case "HU": return new int[] { 30, Int32.Parse(split[1]) };
                case "HUR": return new int[] { 31, Int32.Parse(split[1]) };
                case "SH": return new int[] { 32, Int32.Parse(split[1]) };
                case "SHR": return new int[] { 33, Int32.Parse(split[1]) };
                case "EN": return new int[] { 34, Int32.Parse(split[1]) };
                case "ENR": return new int[] { 35, Int32.Parse(split[1]) };
                case "ACE": return new int[] { 36, Int32.Parse(split[1]) };
                case "ACP": return new int[] { 37, Int32.Parse(split[1]) };
                case "ACI": return new int[] { 38, Int32.Parse(split[1]) };
                case "ACC": return new int[] { 39, Int32.Parse(split[1]) };
                case "ACA": return new int[] { 40, Int32.Parse(split[1]) };
                case "EVE": return new int[] { 41, Int32.Parse(split[1]) };
                case "EVP": return new int[] { 42, Int32.Parse(split[1]) };
                case "EVI": return new int[] { 43, Int32.Parse(split[1]) };
                case "EVC": return new int[] { 44, Int32.Parse(split[1]) };
                case "EVA": return new int[] { 45, Int32.Parse(split[1]) };
                case "ARE": return new int[] { 46, Int32.Parse(split[1]) };
                case "ARP": return new int[] { 47, Int32.Parse(split[1]) };
                case "ARI": return new int[] { 48, Int32.Parse(split[1]) };
                case "ARC": return new int[] { 49, Int32.Parse(split[1]) };
                case "ARA": return new int[] { 50, Int32.Parse(split[1]) };
                case "DAE": return new int[] { 51, Int32.Parse(split[1]) };
                case "DAP": return new int[] { 52, Int32.Parse(split[1]) };
                case "DAI": return new int[] { 53, Int32.Parse(split[1]) };
                case "DAC": return new int[] { 54, Int32.Parse(split[1]) };
                case "DAA": return new int[] { 55, Int32.Parse(split[1]) };
                case "IME": return new int[] { 64, Int32.Parse(split[1]) };
                case "IMP": return new int[] { 65, Int32.Parse(split[1]) };
                case "IMI": return new int[] { 66, Int32.Parse(split[1]) };
                case "IMC": return new int[] { 67, Int32.Parse(split[1]) };
                case "IMA": return new int[] { 68, Int32.Parse(split[1]) };
                default: throw new Exception("Ошибка при расшифровке групповых эффектов");
            }
        }
    }
    public class WeaponBattleParam
    {
        public EWeaponType Type;
        public int Damage;
        public int Consume;
        public ItemSize Size;
        public WeaponBattleParam(EWeaponType type, int damage, int consume, ItemSize size)
        {
            Type = type; Damage = damage; Consume = consume; Size = size;
        }
        public WeaponBattleParam(string text)
        {
            string[] s = text.Split(',');
            switch (s[0])
            {
                case "E":
                    switch (s[1])
                    {
                        case "L": Type = EWeaponType.Laser; break;
                        case "E": Type = EWeaponType.EMI; break;
                        case "P": Type = EWeaponType.Plasma; break;
                        case "S": Type = EWeaponType.Solar; break;
                        default: throw new Exception("Ошибка в названии орудия");
                    }
                    break;
                case "P":
                    switch (s[1])
                    {
                        case "C": Type = EWeaponType.Cannon; break;
                        case "G": Type = EWeaponType.Gauss; break;
                        case "M": Type = EWeaponType.Missle; break;
                        case "A": Type = EWeaponType.AntiMatter; break;
                        default: throw new Exception("Ошибка в названии орудия");
                    }
                    break;
                case "I":
                    switch (s[1])
                    {
                        case "P": Type = EWeaponType.Psi; break;
                        case "D": Type = EWeaponType.Dark; break;
                        case "W": Type = EWeaponType.Warp; break;
                        case "T": Type = EWeaponType.Time; break;
                        default: throw new Exception("Ошибка в названии орудия");
                    }
                    break;
                case "C":
                    switch (s[1])
                    {
                        case "S": Type = EWeaponType.Slicing; break;
                        case "R": Type = EWeaponType.Radiation; break;
                        case "D": Type = EWeaponType.Drone; break;
                        case "M": Type = EWeaponType.Magnet; break;
                        default: throw new Exception("Ошибка в названии орудия");
                    }
                    break;
                default: throw new Exception("Ошибка в группе оружия");
            }
            switch (s[2])
            {
                case "S": Size = ItemSize.Small; break;
                case "M": Size = ItemSize.Medium; break;
                case "L": Size = ItemSize.Large; break;
            }
            Damage = Int32.Parse(s[3]);
            Consume = Int32.Parse(s[4]);
        }
    }
    /// <summary> класс с параметрами поля боя </summary>
    public class BattleFieldParam
    {
        public byte Field; //Размеры поля боя
        public byte[] AstroLocs; //Расположения астероидов
        public byte[] AstroImages; //Изображения астероидов
        public byte[] AstroSizes; //Размеры астероидов (0 - малый, 1 - средний)
        public byte[] Backs; //Задник поля боя
        public byte Random; //Количество рандомных астероидов
        public byte[] RandomImages; //Изображения рандомных астероидов
        public byte[] PortalsAttack = new byte[] { 255 }; //Расположения базовых порталов атаки (255 - стандартный), используется для рандомной генерациии астероидов
        public byte[] PortalsDefense = new byte[] { 255 }; //Расположение базовых порталов защиты (255 - стандартный), используется для рандомной генерациии астероидов
        public byte MaxShips;
        public BattleFieldParam(string text)
        {
            Field = 0; AstroLocs = new byte[0]; AstroImages = null; Backs = new byte[] { 0, 0 };
            MaxShips = 255;
            string[] texts = text.Split(' ');
            foreach (string t in texts)
            {
                string[] parms = t.Split(':');
                switch (parms[0])
                {
                    case "BF": Field = Byte.Parse(parms[1]); break;
                    case "AL": AstroLocs = GetBytes(parms[1]); break;
                    case "AI": AstroImages = GetBytes(parms[1]); break;
                    case "AS": AstroSizes = GetAstroSizes(parms[1]); break;
                    case "B": AstroImages = GetBytes(parms[1]); break;
                    case "RAND": Random = GetBytes(parms[1])[0]; break;
                    case "RI": RandomImages = GetBytes(parms[1]); break;
                }
            }
        }
        byte[] GetAstroSizes(string s)
        {
            string[] split = s.Split(',');
            List<byte> list = new List<byte>();
            foreach (string ss in split)
                if (ss == "S") list.Add(0);
                else if (ss == "M") list.Add(1);
            return list.ToArray();
        }
        byte[] GetBytes(string s)
        {
            string[] split = s.Split(',');
            List<byte> list = new List<byte>();
            foreach (string ss in split)
                list.Add(Byte.Parse(ss));
            return list.ToArray();
        }
        public BattleFieldGroup GetBattleField()
        {
            return new BattleFieldGroup(BattleField.Fields[Field], GetAsteroids(Field), Backs);
        }
        /// <summary> метод возвращает список астероидов, где массив 1 - перечень точек, где стоят астероиды, массив 2 - перечень изображений астероидов 
        /// 0-199 - квестовые астероиды, 200-253 - базовые астероиды, 254 - прозрачный астероид, 255 - случайный астероид</summary>
        SortedList<byte, ServerShipB> GetAsteroids(byte fieldsize)
        {
            SortedList<byte, ServerShipB> result = new SortedList<byte, ServerShipB>();
            byte id = 0;
            for (byte i = 0; i < AstroLocs.Length; i++)
            {
                if (AstroSizes[i] == 1)//Если астероид большой, то добавляем астероид с параметром IsBig и группу прозрачных астероидов рядом с ним
                {
                    BattleField field = BattleField.Fields[fieldsize];
                    byte largehex = AstroLocs[i];
                    Hex largeastrohex = field.Hexes[largehex];
                    foreach (Hex hex in largeastrohex.NearHexes)
                    {
                        if (result.ContainsKey(hex.ID)) continue;
                        result.Add(hex.ID, ServerShipB.GetAsteroid(id, hex.ID, false));
                        result[hex.ID].HelpID = 254;
                        id++;
                    }
                    result.Add(largehex, ServerShipB.GetAsteroid(id, largehex, true));
                    if (AstroImages == null)
                        result[AstroLocs[i]].HelpID = 255;
                    else
                        result[AstroLocs[i]].HelpID = AstroImages[i];
                    id++;
                }
                else
                {
                    result.Add(AstroLocs[i], ServerShipB.GetAsteroid(id, AstroLocs[i], false));
                    if (AstroImages == null)
                        result[AstroLocs[i]].HelpID = 255;
                    else
                        result[AstroLocs[i]].HelpID = AstroImages[i];
                    id++;
                }
            }
            if (Random>0) //Добавление случайных астероидов
            {
                SortedSet<byte> hexes = new SortedSet<byte>();
                BattleField field = BattleField.Fields[Field];
                foreach (Hex hex in field.Hexes) hexes.Add(hex.ID);
                foreach (byte b in result.Keys) hexes.Remove(b);
                foreach (byte portalattack in PortalsAttack)
                {
                    byte hex = portalattack; if (hex == 255) hex = 0;
                    byte[] portalhexes = field.GetPortalHexes(hex);
                    foreach (byte b in portalhexes)
                        hexes.Remove(b);
                }
                foreach (byte portaldefence in PortalsDefense)
                {
                    byte hex = portaldefence; if (hex == 255) hex = (byte)field.MaxHex;
                    byte[] portalhexes = field.GetPortalHexes(hex);
                    foreach (byte b in portalhexes)
                        hexes.Remove(b);
                }
                Random rnd = new Random();
                for (int i=0;i<Random;i++)
                {
                    byte hex = hexes.ElementAt(rnd.Next(hexes.Count));
                    hexes.Remove(hex);
                    result.Add(hex, ServerShipB.GetAsteroid(id, hex, false));
                    if (RandomImages != null)
                        result[hex].HelpID = RandomImages[rnd.Next(RandomImages.Length)];
                    else
                        result[hex].HelpID = 255;
                    id++;
                }
                    
            }
            return result;
        }
    }
    public enum ShipGenerator2Types { Portal, Scout, Cargo, Corvett, Linkor, Frigate, Fighter, Dreadnought, Devostator, Warrior, Cruiser, NoGun, Large, Building }
    public enum EnemySide { Pirate, GreenTeam, Techno, Alien, Mercs, Player }
    class ShipGenerator2
    {
        public static ShipBattleParam GetShip(ShipGenerator2Types type, EnemySide side, int d, WeaponGroup exclude, WeaponGroup priority, Random rnd)
        {
            if (type == ShipGenerator2Types.Portal) return GetPortal(d, side);
            WeaponGroup group = SelectMainWeaponGroup(rnd, exclude, priority);
            ShipBattleParam param = new ShipBattleParam();
            param.Health = GetHealth(type, side, d);
            param.Shield = GetShield(type, side, d);
            param.HexMoveCost = GetMoveCost(type, d);
            param.Energy = GetEnergy(type, d, rnd);
            param.Accuracy = GetAccuracy(type, side, d);
            param.Evasion = GetEvasion(type, side, d, group, rnd);
            param.Ignore = GetArmor(type, side, d, group, rnd);
            param.Immune = GetImmune(d, rnd, group, side, type);
            param.GroupEffects = GetGroupEffects(rnd, d, type);
            param.Name = GetName(d, type, side);
            param.Image = GetImages(type, side);
            switch (type)
            {
                case ShipGenerator2Types.Scout:
                    param.Weapons[1] = GetWeapon(d, side, true, group, ItemSize.Large);
                    param.TimeToEnter = 1;
                    param.JumpDistance = 3;
                    break;
                case ShipGenerator2Types.Corvett:
                    param.Weapons[1] = GetWeapon(d, side, false, group, ItemSize.Large);
                    param.TimeToEnter = 1;
                    param.JumpDistance = 1;

                    break;
                case ShipGenerator2Types.Cargo:
                    param.Weapons[1] = GetWeapon(d, side, false, group, ItemSize.Medium);
                    param.TimeToEnter = 1;
                    param.JumpDistance = 2;
                    break;
                case ShipGenerator2Types.Linkor:
                    param.Weapons[0] = GetWeapon(d, side, true, group, ItemSize.Small);
                    param.Weapons[2] = GetWeapon(d, side, true, group, ItemSize.Small);
                    param.TimeToEnter = 2;
                    param.JumpDistance = 2;
                    break;
                case ShipGenerator2Types.Frigate:
                    param.Weapons[0] = GetWeapon(d, side, false, group, ItemSize.Medium);
                    param.Weapons[2] = GetWeapon(d, side, false, group, ItemSize.Medium);
                    param.TimeToEnter = 2;
                    param.JumpDistance = 1;
                    break;
                case ShipGenerator2Types.Fighter:
                    param.Weapons[0] = GetWeapon(d, side, true, group, ItemSize.Small);
                    param.Weapons[2] = GetWeapon(d, side, true, group, ItemSize.Small);
                    param.TimeToEnter = 2;
                    param.JumpDistance = 3;
                    break;
                case ShipGenerator2Types.Dreadnought:
                    param.Weapons[0] = GetWeapon(d, side, false, group, ItemSize.Small);
                    param.Weapons[2] = GetWeapon(d, side, false, group, ItemSize.Small);
                    param.TimeToEnter = 2;
                    param.JumpDistance = 1;
                    break;
                case ShipGenerator2Types.Devostator:
                    param.Weapons[0] = GetWeapon(d, side, false, group, ItemSize.Large);
                    param.Weapons[1] = GetWeapon(d, side, false, group, ItemSize.Medium);
                    param.Weapons[2] = GetWeapon(d, side, false, group, ItemSize.Small);
                    param.TimeToEnter = 3;
                    param.JumpDistance = 1;
                    break;
                case ShipGenerator2Types.Warrior:
                    param.Weapons[0] = GetWeapon(d, side, false, group, ItemSize.Small);
                    param.Weapons[1] = GetWeapon(d, side, true, group, ItemSize.Small);
                    param.Weapons[2] = GetWeapon(d, side, true, group, ItemSize.Small);
                    param.TimeToEnter = 3;
                    param.JumpDistance = 2;
                    break;
                case ShipGenerator2Types.Cruiser:
                    param.Weapons[0] = GetWeapon(d, side, true, group, ItemSize.Medium);
                    param.Weapons[1] = GetWeapon(d, side, true, group, ItemSize.Medium);
                    param.Weapons[2] = GetWeapon(d, side, true, group, ItemSize.Small);
                    param.TimeToEnter = 3;
                    param.JumpDistance = 1;
                    break;
                case ShipGenerator2Types.NoGun:
                    param.TimeToEnter = 3; param.JumpDistance = 1;
                    param.Type = StoryShipType.CargoShip;
                    break;
                case ShipGenerator2Types.Building:
                    param.TimeToEnter = 3; param.JumpDistance = 1;
                    param.Type = StoryShipType.LargeBuilding;
                    break;
                case ShipGenerator2Types.Large:
                    param.TimeToEnter = 3; param.JumpDistance = 1;
                    param.Type = StoryShipType.BigShip;
                    param.Weapons[0] = GetWeapon(d+5, side, false, group, ItemSize.Large);
                    param.Weapons[1] = GetWeapon(d+5, side, false, group, ItemSize.Small);
                    param.Weapons[2] = GetWeapon(d, side, false, group, ItemSize.Large);
                    break;
            }

            return param;
        }
        static ShipBattleParam GetPortal(int d, EnemySide side)
        {
            ShipBattleParam param = new ShipBattleParam();
            param.Type = StoryShipType.Portal;
            int health = 200 + d * 20;
            switch (side)
            {
                case EnemySide.Pirate: health = (int)(health * 0.7); break;
                case EnemySide.GreenTeam: health = (int)(health * 1.2); break;
                case EnemySide.Techno: health = (int)(health * 1.2); break;
                case EnemySide.Alien: health = (int)(health * 1.2); break;
            }
            param.Health = new int[] { health, 0 };
            return param;
        }
        static int[] GetHealth(ShipGenerator2Types type, EnemySide side, int d)
        {
            int healthbase = 100;
            int healthmod = 0;
            int regenbase = 0;
            int regenmod = 0;
            double sidemod = 1.0;
            switch (type)
            {
                case ShipGenerator2Types.Scout: healthbase = 150; healthmod = 8; regenbase = -30; regenmod = 3; break;
                case ShipGenerator2Types.Corvett: healthbase = 100; healthmod = 5; regenbase = -30; regenmod = 5; break;
                case ShipGenerator2Types.Cargo: healthbase = 150; healthmod = 8; regenbase = -60; regenmod = 3; break;
                case ShipGenerator2Types.Linkor: healthbase = 240; healthmod = 12; regenbase = 0; regenmod = 5; break;
                case ShipGenerator2Types.Frigate: healthbase = 180; healthmod = 9; regenbase = 0; regenmod = 3; break;
                case ShipGenerator2Types.Fighter: healthbase = 120; healthmod = 6; regenbase = -30; regenmod = 3; break;
                case ShipGenerator2Types.Dreadnought: healthbase = 180; healthmod = 9; regenbase = -30; regenmod = 5; break;
                case ShipGenerator2Types.Devostator: healthbase = 210; healthmod = 11; regenbase = -30; regenmod = 3; break;
                case ShipGenerator2Types.Warrior: healthbase = 280; healthmod = 14; regenbase = 20; regenmod = 10; break;
                case ShipGenerator2Types.Cruiser: healthbase = 280; healthmod = 14; regenbase = 0; regenmod = 5; break;
                case ShipGenerator2Types.NoGun: healthbase = 500; healthmod = 20; regenbase = 0; regenmod = 0; break;
                case ShipGenerator2Types.Large: healthbase = 1000; healthmod = 50; regenbase = 20; regenmod = 10; break;
                case ShipGenerator2Types.Building: healthbase = 2000; healthmod = 100; regenbase = -200; regenmod = 20; break;
            }
            switch (side)
            {
                case EnemySide.GreenTeam: sidemod = 1.2; break;
                case EnemySide.Mercs: sidemod = 0.8; break;
                case EnemySide.Alien: sidemod = 1.2; break;
            }
            int health = (int)((healthbase + d * healthmod) * sidemod);
            int regen = (int)((regenbase + d * regenmod) * sidemod);
            if (regen < 0) regen = 0;
            return new int[] { health, regen };
        }
        static WeaponGroup SelectMainWeaponGroup(Random rnd, WeaponGroup exclude, WeaponGroup priority)
        {
            if (priority != WeaponGroup.Any)
            {
                int val = rnd.Next(100); if (val < 40) return priority;
            }
            List<WeaponGroup> list = new List<WeaponGroup>(new WeaponGroup[] { WeaponGroup.Energy, WeaponGroup.Physic, WeaponGroup.Irregular, WeaponGroup.Cyber });
            if (exclude != WeaponGroup.Any)
                list.Remove(exclude);
            int val1 = rnd.Next(list.Count);
            return list[val1];
        }
        static WeaponBattleParam GetWeapon(int d, EnemySide side, bool IsNear, WeaponGroup group, ItemSize size)
        {
            int basicdam = 30; int basicmod = 3; double sidemod = 1; int bonusdam = 0; int bonusmod = 0;
            int basiccons = 12; int consmodi = 1;
            EWeaponType type = EWeaponType.Laser;
            switch (side)
            {
                case EnemySide.Pirate: sidemod = 0.8;
                    switch (group)
                    {
                        case WeaponGroup.Energy: type = IsNear ? EWeaponType.Plasma : EWeaponType.Laser; break;
                        case WeaponGroup.Physic: type = IsNear ? EWeaponType.Cannon : EWeaponType.Missle; break;
                        case WeaponGroup.Irregular: type = IsNear ? EWeaponType.Warp : EWeaponType.Psi; break;
                        case WeaponGroup.Cyber: type = IsNear ? EWeaponType.Slicing : EWeaponType.Drone; break;
                    }
                    break;
                case EnemySide.GreenTeam:
                    switch (group)
                    {
                        case WeaponGroup.Energy: type = IsNear ? EWeaponType.Solar : EWeaponType.Laser; break;
                        case WeaponGroup.Physic: type = IsNear ? EWeaponType.Cannon : EWeaponType.Missle; break;
                        case WeaponGroup.Irregular: type = IsNear ? EWeaponType.Warp : EWeaponType.Time; break;
                        case WeaponGroup.Cyber: type = IsNear ? EWeaponType.Slicing : EWeaponType.Magnet; break;
                    } break;
                case EnemySide.Techno: sidemod = 0.8;
                    switch (group)
                    {
                        case WeaponGroup.Energy: type = IsNear ? EWeaponType.Solar : EWeaponType.EMI; break;
                        case WeaponGroup.Physic: type = IsNear ? EWeaponType.AntiMatter : EWeaponType.Gauss; break;
                        case WeaponGroup.Irregular: type = IsNear ? EWeaponType.Warp : EWeaponType.Time; break;
                        case WeaponGroup.Cyber: type = IsNear ? EWeaponType.Radiation : EWeaponType.Magnet; break;
                    } break;
                case EnemySide.Mercs:
                    switch (group)
                    {
                        case WeaponGroup.Energy: type = IsNear ? EWeaponType.Plasma : EWeaponType.EMI; break;
                        case WeaponGroup.Physic: type = IsNear ? EWeaponType.Cannon : EWeaponType.Gauss; break;
                        case WeaponGroup.Irregular: type = IsNear ? EWeaponType.Dark : EWeaponType.Time; break;
                        case WeaponGroup.Cyber: type = IsNear ? EWeaponType.Radiation : EWeaponType.Drone; break;
                    } break;
                case EnemySide.Alien: sidemod = 1.2;
                    switch (group)
                    {
                        case WeaponGroup.Energy: type = IsNear ? EWeaponType.Solar : EWeaponType.Laser; break;
                        case WeaponGroup.Physic: type = IsNear ? EWeaponType.AntiMatter : EWeaponType.Missle; break;
                        case WeaponGroup.Irregular: type = IsNear ? EWeaponType.Dark : EWeaponType.Psi; break;
                        case WeaponGroup.Cyber: type = IsNear ? EWeaponType.Radiation : EWeaponType.Magnet; break;
                    }
                    break;
            }
            switch (size)
            {
                case ItemSize.Small: bonusdam = 20; bonusmod = 2; break;
                case ItemSize.Medium: bonusdam = bonusdam = 10; bonusmod = 1; break;
            }
            int damage = (int)((basicdam + bonusdam + (basicmod + bonusmod) * d) * sidemod);
            int consume = basiccons + consmodi * d;
            return new WeaponBattleParam(type, damage, consume, size);
        }
        static int[] GetShield(ShipGenerator2Types type, EnemySide side, int d)
        {
            int valuebase = 50;
            int valuemod = 20;
            int regenbase = 30;
            int regenmod = 10;
            int angle = 30;
            double sidemod = 1.0;
            switch (type)
            {
                case ShipGenerator2Types.Scout: valuebase = 100; valuemod = 10; regenbase = 70; regenmod = 7; angle = 90; break;
                case ShipGenerator2Types.Corvett: valuebase = 50; valuemod = 5; regenbase = 50; regenmod = 5; angle = 90; break;
                case ShipGenerator2Types.Cargo: valuebase = 50; valuemod = 5; regenbase = 50; regenmod = 5; angle = 150; break;
                case ShipGenerator2Types.Linkor: valuebase = 120; valuemod = 12; regenbase = 90; regenmod = 9; angle = 150; break;
                case ShipGenerator2Types.Frigate: valuebase = 180; valuemod = 18; regenbase = 120; regenmod = 12; angle = 30; break;
                case ShipGenerator2Types.Fighter: valuebase = 120; valuemod = 12; regenbase = 90; regenmod = 9; angle = 30; break;
                case ShipGenerator2Types.Dreadnought: valuebase = 180; valuemod = 18; regenbase = 60; regenmod = 6; angle = 90; break;
                case ShipGenerator2Types.Devostator: valuebase = 140; valuemod = 14; regenbase = 90; regenmod = 9; angle = 30; break;
                case ShipGenerator2Types.Warrior: valuebase = 210; valuemod = 21; regenbase = 60; regenmod = 6; angle = 90; break;
                case ShipGenerator2Types.Cruiser: valuebase = 210; valuemod = 21; regenbase = 60; regenmod = 6; angle = 150; break;
                case ShipGenerator2Types.NoGun: valuebase = 180; valuemod = 18; regenbase = 60; regenmod = 6; angle = 90; break;
                case ShipGenerator2Types.Large: valuebase = 300; valuemod = 30; regenbase = 60; regenmod = 6; angle = 180; break;
                case ShipGenerator2Types.Building: valuebase = 0; valuemod = 0; regenbase = 0; regenmod = 0; angle = 0; break;
            }
            switch (side)
            {
                case EnemySide.Pirate: sidemod = 0.8; break;
                case EnemySide.Techno: sidemod = 1.3; break;
                case EnemySide.Mercs: sidemod = 0.9; break;
            }
            int value = (int)((valuebase + d * valuemod) * sidemod);
            int regen = (int)((regenbase + d * regenmod) * sidemod);
            if (regen < 0) regen = 0;
            return new int[] { value, regen, angle };
        }
        static int GetMoveCost(ShipGenerator2Types type, int d)
        {
            int weaponshootconsume = 12 + d;
            switch (type)
            {
                case ShipGenerator2Types.Scout: return weaponshootconsume / 2;
                case ShipGenerator2Types.Corvett: return weaponshootconsume;
                case ShipGenerator2Types.Cargo: return weaponshootconsume;
                case ShipGenerator2Types.Linkor: return weaponshootconsume * 2 / 3;
                case ShipGenerator2Types.Frigate: return weaponshootconsume * 2 / 3;
                case ShipGenerator2Types.Fighter: return weaponshootconsume / 2;
                case ShipGenerator2Types.Dreadnought: return weaponshootconsume * 2 / 3;
                case ShipGenerator2Types.Devostator: return weaponshootconsume;
                case ShipGenerator2Types.Warrior: return weaponshootconsume * 2 / 3;
                case ShipGenerator2Types.Cruiser: return weaponshootconsume;
                case ShipGenerator2Types.NoGun: return 999;
                case ShipGenerator2Types.Building: return 999;
                case ShipGenerator2Types.Large: return weaponshootconsume * 2;
                default: throw new Exception();
            }
        }
        static int[] GetEnergy(ShipGenerator2Types type, int d, Random rnd)
        {
            switch (type)
            {
                case ShipGenerator2Types.NoGun: return new int[] { 0, 0 };
                case ShipGenerator2Types.Building: return new int[] { 0, 0 };
            }
            int shootcost = 12 + d;
            int energy = shootcost * 4;
            int regenmod = rnd.Next(5, 8);
            int regen = energy * regenmod / 10;
            return new int[] { energy, regen };
        }
        static int[] GetAccuracy(ShipGenerator2Types type, EnemySide side, int d)
        {
            int basic = 30; int bonus = 1; double sidemod = 1.0;
            switch (type)
            {
                case ShipGenerator2Types.Linkor:
                case ShipGenerator2Types.Cruiser: basic = 30; bonus = 1; break;
                case ShipGenerator2Types.Scout:
                case ShipGenerator2Types.Cargo:
                case ShipGenerator2Types.Fighter:
                case ShipGenerator2Types.Warrior: basic = 50; bonus = 2; break;
                case ShipGenerator2Types.Corvett:
                case ShipGenerator2Types.Frigate:
                case ShipGenerator2Types.Dreadnought:
                case ShipGenerator2Types.Devostator: basic = 70; bonus = 3; break;
                case ShipGenerator2Types.NoGun: return new int[4];
                case ShipGenerator2Types.Building: return new int[4];
                case ShipGenerator2Types.Large: basic = 70; bonus = 5; break;
            }
            switch (side)
            {
                case EnemySide.Pirate: sidemod = 0.9; break;
                case EnemySide.GreenTeam: sidemod = 0.9; break;
                case EnemySide.Mercs: sidemod = 1.1; break;
                case EnemySide.Alien: sidemod = 0.9; break;
            }
            int value = (int)((basic + d * bonus) * sidemod);
            return new int[] { value, value, value, value };
        }
        static int[] GetEvasion(ShipGenerator2Types type, EnemySide side, int d, WeaponGroup group, Random rnd)
        {
            double sidemod = 1.0;
            switch (side)
            {
                case EnemySide.GreenTeam: sidemod = 0.8; break;
                case EnemySide.Mercs: sidemod = 1.1; break;
                case EnemySide.Alien: sidemod = 0.9; break;
            }
            int max = (int)((80 + d * 4) * sidemod);
            int mid = (int)((50 + d * 2) * sidemod);
            int min = (int)((20 + d) * sidemod);
            List<WeaponGroup> list = new List<WeaponGroup>(new WeaponGroup[] { WeaponGroup.Energy, WeaponGroup.Physic, WeaponGroup.Irregular, WeaponGroup.Cyber });
            list.Remove(group);
            WeaponGroup randomgroup1 = list[rnd.Next(3)];
            WeaponGroup randomgroup2 = list[rnd.Next(2)];
            WeaponGroup group4 = list[0];
            switch (type)
            {
                case ShipGenerator2Types.Scout: return new int[] { group==WeaponGroup.Energy?max:randomgroup1==WeaponGroup.Energy?max:randomgroup2==WeaponGroup.Energy?mid:min,
                        group == WeaponGroup.Physic ? max : randomgroup1 == WeaponGroup.Physic ? max : randomgroup2 == WeaponGroup.Physic ? mid : min,
                        group == WeaponGroup.Irregular ? max : randomgroup1 == WeaponGroup.Irregular ? max : randomgroup2 == WeaponGroup.Irregular ? mid : min,
                        group == WeaponGroup.Cyber ? max : randomgroup1 == WeaponGroup.Cyber ? max : randomgroup2 == WeaponGroup.Cyber ? mid : min };
                case ShipGenerator2Types.Corvett: return new int[] {group==WeaponGroup.Energy?max: randomgroup1==WeaponGroup.Energy?max:mid,
                group==WeaponGroup.Physic?max: randomgroup1==WeaponGroup.Physic?max:mid, group==WeaponGroup.Irregular?max: randomgroup1==WeaponGroup.Irregular?max:mid,
                group==WeaponGroup.Cyber?max: randomgroup1==WeaponGroup.Cyber?max:mid};
                case ShipGenerator2Types.Cargo: return new int[] { min, min, min, min };
                case ShipGenerator2Types.Linkor: return new int[] {group==WeaponGroup.Energy?max:min, group==WeaponGroup.Physic?max:min,
                group==WeaponGroup.Irregular?max:min, group==WeaponGroup.Cyber?max:min};
                case ShipGenerator2Types.Frigate: return new int[]{ group==WeaponGroup.Energy?max:randomgroup1==WeaponGroup.Energy?mid:randomgroup2==WeaponGroup.Energy?mid:min,
                        group == WeaponGroup.Physic ? max : randomgroup1 == WeaponGroup.Physic ? mid : randomgroup2 == WeaponGroup.Physic ? mid : min,
                        group == WeaponGroup.Irregular ? max : randomgroup1 == WeaponGroup.Irregular ? mid : randomgroup2 == WeaponGroup.Irregular ? mid : min,
                        group == WeaponGroup.Cyber ? max : randomgroup1 == WeaponGroup.Cyber ? mid : randomgroup2 == WeaponGroup.Cyber ? mid : min };
                case ShipGenerator2Types.Fighter: return new int[] {group4==WeaponGroup.Energy?mid:max, group4==WeaponGroup.Physic?mid:max,
                group4==WeaponGroup.Irregular?mid:max, group4==WeaponGroup.Cyber?mid:max};
                case ShipGenerator2Types.Dreadnought:
                    return new int[] {group==WeaponGroup.Energy?mid: randomgroup1==WeaponGroup.Energy?mid:min,
                group==WeaponGroup.Physic?mid: randomgroup1==WeaponGroup.Physic?mid:min, group==WeaponGroup.Irregular?mid: randomgroup1==WeaponGroup.Irregular?mid:min,
                group==WeaponGroup.Cyber?mid: randomgroup1==WeaponGroup.Cyber?mid:min};
                case ShipGenerator2Types.Devostator:
                    return new int[] {group==WeaponGroup.Energy?max: randomgroup1==WeaponGroup.Energy?mid:min,
                        group ==WeaponGroup.Physic?max:randomgroup1==WeaponGroup.Physic?mid:min, group==WeaponGroup.Irregular?max:randomgroup1==WeaponGroup.Irregular?mid:min,
                    group==WeaponGroup.Cyber?max:randomgroup1==WeaponGroup.Cyber?mid:min};
                case ShipGenerator2Types.Warrior:
                    return new int[] {group==WeaponGroup.Energy?max: randomgroup1==WeaponGroup.Energy?max:min,
                group==WeaponGroup.Physic?max: randomgroup1==WeaponGroup.Physic?max:min, group==WeaponGroup.Irregular?max: randomgroup1==WeaponGroup.Irregular?max:min,
                group==WeaponGroup.Cyber?max: randomgroup1==WeaponGroup.Cyber?max:min};
                case ShipGenerator2Types.Cruiser:
                    return new int[] { group == WeaponGroup.Energy ? mid : min, group == WeaponGroup.Physic ? mid : min, group == WeaponGroup.Irregular ? mid : min, group == WeaponGroup.Cyber ? mid : min };
                case ShipGenerator2Types.NoGun:
                    return new int[] { min, min, min, min };
                case ShipGenerator2Types.Building:
                    return new int[4];
                case ShipGenerator2Types.Large:
                    return new int[4];
                default:
                    throw new Exception();
            }
        }
        static int[] GetArmor(ShipGenerator2Types type, EnemySide side, int d, WeaponGroup group, Random rnd)
        {
            int sidemod = 0;
            switch (side)
            {
                case EnemySide.GreenTeam: sidemod = 10; break;
                case EnemySide.Techno: sidemod = -15; break;
                case EnemySide.Alien: sidemod = 10; break;
            }
            int max = (int)((40 + d * 2) + sidemod); if (max < 0) max = 0; if (max > 90) max = 90;
            int mid = (int)((20 + d) + sidemod); if (mid < 0) mid = 0; if (mid > 90) mid = 90;
            int min = (int)((0 + d) + sidemod); if (min < 0) min = 0; if (min > 90) min = 90;
            List<WeaponGroup> list = new List<WeaponGroup>(new WeaponGroup[] { WeaponGroup.Energy, WeaponGroup.Physic, WeaponGroup.Irregular, WeaponGroup.Cyber });
            list.Remove(group);
            WeaponGroup randomgroup1 = list[rnd.Next(3)];
            WeaponGroup randomgroup2 = list[rnd.Next(2)];
            WeaponGroup group4 = list[0];
            switch (type)
            {
                case ShipGenerator2Types.Scout:
                    return new int[] {group==WeaponGroup.Energy?mid: randomgroup1==WeaponGroup.Energy?mid:min,
                group==WeaponGroup.Physic?mid: randomgroup1==WeaponGroup.Physic?mid:min, group==WeaponGroup.Irregular?mid: randomgroup1==WeaponGroup.Irregular?mid:min,
                group==WeaponGroup.Cyber?mid: randomgroup1==WeaponGroup.Cyber?mid:min};
                case ShipGenerator2Types.Corvett:
                    return new int[] { group == WeaponGroup.Energy ? mid : min, group == WeaponGroup.Physic ? mid : min, group == WeaponGroup.Irregular ? mid : min, group == WeaponGroup.Cyber ? mid : min };
                case ShipGenerator2Types.Cargo:
                    return new int[] { group4 == WeaponGroup.Energy ? min : mid, group4 == WeaponGroup.Physic ? min : mid, group4 == WeaponGroup.Irregular ? min : mid, group4 == WeaponGroup.Cyber ? min : mid };
                case ShipGenerator2Types.Linkor:
                    return new int[] { group4 == WeaponGroup.Energy ? min : max, group4 == WeaponGroup.Physic ? min : max, group4 == WeaponGroup.Irregular ? min : max, group4 == WeaponGroup.Cyber ? min : max };
                case ShipGenerator2Types.Frigate:
                    return new int[] {group==WeaponGroup.Energy?max:min, group==WeaponGroup.Physic?max:min, group==WeaponGroup.Irregular?max:min, group==WeaponGroup.Cyber?max:min};
                case ShipGenerator2Types.Fighter:
                    return new int[] {group == WeaponGroup.Energy ? max : randomgroup1 == WeaponGroup.Energy ? mid : min,
                        group == WeaponGroup.Physic ? max : randomgroup1 == WeaponGroup.Physic ? mid : min,
                        group == WeaponGroup.Irregular ? max : randomgroup1 == WeaponGroup.Irregular ? mid : min,
                    group == WeaponGroup.Cyber ? max : randomgroup1 == WeaponGroup.Cyber ? mid : min};
                case ShipGenerator2Types.Dreadnought:
                    return new int[] {group==WeaponGroup.Energy?max: randomgroup1==WeaponGroup.Energy?max:mid,
                group==WeaponGroup.Physic?max: randomgroup1==WeaponGroup.Physic?max:mid, group==WeaponGroup.Irregular?max: randomgroup1==WeaponGroup.Irregular?max:mid,
                group==WeaponGroup.Cyber?max: randomgroup1==WeaponGroup.Cyber?max:mid};
                case ShipGenerator2Types.Devostator:
                    return new int[]{ group==WeaponGroup.Energy?max:randomgroup1==WeaponGroup.Energy?mid:randomgroup2==WeaponGroup.Energy?mid:min,
                        group == WeaponGroup.Physic ? max : randomgroup1 == WeaponGroup.Physic ? mid : randomgroup2 == WeaponGroup.Physic ? mid : min,
                        group == WeaponGroup.Irregular ? max : randomgroup1 == WeaponGroup.Irregular ? mid : randomgroup2 == WeaponGroup.Irregular ? mid : min,
                        group == WeaponGroup.Cyber ? max : randomgroup1 == WeaponGroup.Cyber ? mid : randomgroup2 == WeaponGroup.Cyber ? mid : min };
                case ShipGenerator2Types.Warrior:
                    return new int[] { mid, mid, mid, mid };
                case ShipGenerator2Types.Cruiser:
                    return new int[] {group4==WeaponGroup.Energy?mid:max, group4==WeaponGroup.Physic?mid:max,
                group4==WeaponGroup.Irregular?mid:max, group4==WeaponGroup.Cyber?mid:max};
                case ShipGenerator2Types.NoGun:
                    return new int[] { mid, mid, mid, mid };
                case ShipGenerator2Types.Building:
                    return new int[] { mid, mid, mid, mid };
                case ShipGenerator2Types.Large:
                    return new int[] {group4==WeaponGroup.Energy?mid:max, group4==WeaponGroup.Physic?mid:max,
                group4==WeaponGroup.Irregular?mid:max, group4==WeaponGroup.Cyber?mid:max};
                default: throw new Exception();

            }
        }
        static int[] GetImmune(int d, Random rnd, WeaponGroup group, EnemySide side, ShipGenerator2Types type )
        {
            if (type == ShipGenerator2Types.NoGun || type==ShipGenerator2Types.Building || type==ShipGenerator2Types.Large) return new int[] { 100, 100, 100, 100 };
            int ver = 20 + d * 2;
            int[] result = new int[4];
            bool hasimmune1, hasimmune2, hasimmune3, hasimmune4;
            hasimmune1 = hasimmune2 = hasimmune3 = hasimmune4 = false;
            hasimmune1 = rnd.Next(100) < ver;
            if (hasimmune1) hasimmune2 = rnd.Next(100) < ver-20;
            if (hasimmune2) hasimmune3 = rnd.Next(100) < ver - 40;
            if (hasimmune3) hasimmune4 = rnd.Next(100) < ver - 60;
            int sidemod = 0;
            switch (side)
            {
                case EnemySide.Pirate: sidemod = -20; break;
                case EnemySide.GreenTeam: sidemod = 10; break;
                case EnemySide.Mercs: sidemod = 10; break;
                case EnemySide.Alien: sidemod = 10; break;
            }
            int val = 30 + d * 3 + sidemod; if (val > 100) val = 100; 
            if (hasimmune1 == false) return result;
            if (hasimmune1 == true)
            {
                if (group == WeaponGroup.Energy) result[0] = val;
                else if (group == WeaponGroup.Physic) result[1] = val;
                else if (group == WeaponGroup.Irregular) result[2] = val;
                else result[3] = val;
            }
            if (hasimmune2 == false) return result;
            List<WeaponGroup> list = new List<WeaponGroup>(new WeaponGroup[] { WeaponGroup.Energy, WeaponGroup.Physic, WeaponGroup.Irregular, WeaponGroup.Cyber });
            list.Remove(group);
            if (hasimmune2==true)
            {
                WeaponGroup group2 = list[rnd.Next(3)];
                list.Remove(group2);
                val -= rnd.Next(20);
                if (val <= 0) return result;
                if (group2 == WeaponGroup.Energy) result[0] = val;
                else if (group2 == WeaponGroup.Physic) result[1] = val;
                else if (group2 == WeaponGroup.Irregular) result[2] = val;
                else result[3] = val;
            }
            if (hasimmune3 == true)
            {
                WeaponGroup group3 = list[rnd.Next(2)];
                list.Remove(group3);
                val -= rnd.Next(20);
                if (val <= 0) return result;
                if (group3 == WeaponGroup.Energy) result[0] = val;
                else if (group3 == WeaponGroup.Physic) result[1] = val;
                else if (group3 == WeaponGroup.Irregular) result[2] = val;
                else result[3] = val;
            }
            if (hasimmune4 == true)
            {
                WeaponGroup group4 = list[0];
                val -= rnd.Next(20);
                if (val <= 0) return result;
                if (group4 == WeaponGroup.Energy) result[0] = val;
                else if (group4 == WeaponGroup.Physic) result[1] = val;
                else if (group4 == WeaponGroup.Irregular) result[2] = val;
                else result[3] = val;
            }
            return result;
        }
        static List<int[]> GetGroupEffects(Random rnd, int d, ShipGenerator2Types type)
        {
            int effectsrand = 20 + d * 2;
            int effects = rnd.Next(55) < effectsrand ? 1 : 0;
            switch (type)
            {
                case ShipGenerator2Types.Corvett: effects++; break;
                case ShipGenerator2Types.Frigate: effects += 2; break;
                case ShipGenerator2Types.NoGun: return new List<int[]>();
                case ShipGenerator2Types.Building: return new List<int[]>();
                case ShipGenerator2Types.Large: return new List<int[]>();
            }
            if (effects == 0) return new List<int[]>();
            List<int[]> result = new List<int[]>();
            for (int i=0;i<effects;i++)
            {
                int chance = rnd.Next(100);
                switch (chance)
                {
                    case 0: case 1: case 2: result.Add(new int[] { 30, 50 + d * 5 }); break;
                    case 3:
                    case 4:
                    case 5: result.Add(new int[] { 31, 25 + d * 3 }); break;
                    case 6:
                    case 7:
                    case 8: result.Add(new int[] { 32, 70 + d * 7 }); break;
                    case 9:
                    case 10: result.Add(new int[] { 33, 40 + d * 4 }); break;
                    case 11:
                    case 12: result.Add(new int[] { 34, 12 + d }); break;
                    case 13:
                    case 14:
                    case 15: result.Add(new int[] { 35, 8 + d }); break;
                    case 16: result.Add(new int[] { 36, 50 + d * 2 }); break;
                    case 17: result.Add(new int[] { 37, 50 + d * 2 }); break;
                    case 18: result.Add(new int[] { 38, 50 + d * 2 }); break;
                    case 19: result.Add(new int[] { 39, 50 + d * 2 }); break;
                    case 20:
                    case 21: result.Add(new int[] { 40, 30 + d }); break;
                    case 22: result.Add(new int[] { 41, 50 + d * 2 }); break;
                    case 23: result.Add(new int[] { 42, 50 + d * 2 }); break;
                    case 24: result.Add(new int[] { 43, 50 + d * 2 }); break;
                    case 25: result.Add(new int[] { 44, 50 + d * 2 }); break;
                    case 26:
                    case 27: result.Add(new int[] { 45, 30 + d }); break;
                    case 28:
                    case 29: result.Add(new int[] { 46, 15 + d }); break;
                    case 30:
                    case 31: result.Add(new int[] { 47, 15 + d }); break;
                    case 32:
                    case 33: result.Add(new int[] { 48, 15 + d }); break;
                    case 34:
                    case 35: result.Add(new int[] { 49, 15 + d }); break;
                    case 36:
                    case 37:
                    case 38: result.Add(new int[] { 50, 5 + d }); break;
                    case 39: result.Add(new int[] { 51, 30 + d * 3 }); break;
                    case 40: result.Add(new int[] { 52, 30 + d * 3 }); break;
                    case 41: result.Add(new int[] { 53, 30 + d * 3 }); break;
                    case 42: result.Add(new int[] { 54, 30 + d * 3 }); break;
                    case 43:
                    case 44:
                    case 45: result.Add(new int[] { 55, 15 + d * 2 }); break;
                    case 46:
                    case 47: result.Add(new int[] { 64, 40 + d * 2 }); break;
                    case 48:
                    case 49: result.Add(new int[] { 65, 40 + d * 2 }); break;
                    case 50:
                    case 51: result.Add(new int[] { 66, 40 + d * 2 }); break;
                    case 52:
                    case 53: result.Add(new int[] { 67, 40 + d * 2 }); break;
                    case 54: result.Add(new int[] { 68, 30 + d }); break;
                }
            }
            return result;
        }
        static byte[] GetName(int d, ShipGenerator2Types type, EnemySide side)
        {
            byte[] result = new byte[4];
            switch (side)
            {
                case EnemySide.Alien: result[0] = 253; if (d < 100) result[3] = (byte)d; else result[3] = 100; result[2] = 3; break;
                case EnemySide.GreenTeam: result[0] = 251; if (d < 100) result[3] = (byte)d; else result[3] = 100; result[2] = 1; break;
                case EnemySide.Mercs: result[0] = 255; if (d < 110) result[3] = (byte)(d/2+201); else result[3] = 255; result[2] = 1; break;
                case EnemySide.Pirate: result[0] = 252; if (d < 100) result[3] = (byte)d; else result[3] = 100; break;
                case EnemySide.Techno: result[0] = 254; if (d < 100) result[3] = (byte)d; else result[3] = 100; result[2] = 2; break;
            }
            switch (type)
            {
                case ShipGenerator2Types.Scout: result[1] = 1; break;
                case ShipGenerator2Types.Corvett: result[1] = 2; break;
                case ShipGenerator2Types.Cargo: result[1] = 3; break;
                case ShipGenerator2Types.Linkor: result[1] = 4; break;
                case ShipGenerator2Types.Frigate: result[1] = 5; break;
                case ShipGenerator2Types.Fighter: result[1] = 6; break;
                case ShipGenerator2Types.Dreadnought: result[1] = 7; break;
                case ShipGenerator2Types.Devostator: result[1] = 8; break;
                case ShipGenerator2Types.Warrior: result[1] = 9; break;
                case ShipGenerator2Types.Cruiser: result[1] = 10; break;
                case ShipGenerator2Types.NoGun: result[1] = 11; break;
                case ShipGenerator2Types.Large: result[1] = 12; break;
                case ShipGenerator2Types.Building: result[1] = 13; break;
            }
            return result;
        }
        static byte[] GetImages(ShipGenerator2Types type, EnemySide side)
        {
            byte[] result = new byte[4];
            switch (side)
            {
                case EnemySide.Pirate: result[0] = 252; break;
                case EnemySide.Alien: result[0] = 253; break;
                case EnemySide.GreenTeam: result[0] = 251; break;
                case EnemySide.Mercs: result[0] = 255; break;
                case EnemySide.Techno: result[0] = 254; break;
            }
            switch (type)
            {
                case ShipGenerator2Types.Scout:
                case ShipGenerator2Types.Fighter:
                case ShipGenerator2Types.Warrior: result[1] = 2; break;
                case ShipGenerator2Types.Corvett:
                case ShipGenerator2Types.Frigate:
                case ShipGenerator2Types.Dreadnought:
                case ShipGenerator2Types.Devostator: result[1] = 1; break;
                case ShipGenerator2Types.Cargo:
                case ShipGenerator2Types.Linkor:
                case ShipGenerator2Types.Cruiser: result[1] = 3; break;
                case ShipGenerator2Types.NoGun: result[0] = 103; break;
                case ShipGenerator2Types.Building: result[0] = 0; break;
                case ShipGenerator2Types.Large: result[0] = 3; break;
            }
            return result;
        }
    }
}
