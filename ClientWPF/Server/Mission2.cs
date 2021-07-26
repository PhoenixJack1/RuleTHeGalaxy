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

namespace Client
{
    public enum Mission2Type
    {
        Metheorit, OreBelt, Anomaly, ShipWreck, PillageProtect, CrimePursuit, FieldDay, Experiment, ConvoyDefence,
        ConvoyAttack, SmallCompetition, LargeCompetition, RangeRaid, PirateBase, BlackMarket, No
    }
    public enum Mission2Location { FreeOrbit, OreBelt, FreePlanet, BadPlanet }
    public enum Mission2Content { Empty, FreeLow, FreeMedium, BattleMedium, BattleHigh, BlackMarket}
    public class ServerMission2 : FleetTargetI, ServerBattleSide2
    {
        public static SortedList<Mission2Type, ServerMission2Static> Statics = GetList();
        public Mission2Type Type;
        public EnemySide Side;
        public DateTime CreationTime;
        public Mission2Content Content; //Показывает, что будет в миссии - пусто, награда или бой.
        public int Seed;
        public byte Orbit;
        public DateTime EndTime;
        public ServerStar Star;
        public Mission2Location Location { get { return Statics[Type].Location; } }
        public byte MaxShips { get { return Statics[Type].MaxShips; } }
        public ServerMission2(Mission2Type type, ServerStar star, byte orbit)
        {
            Type = type; Orbit = orbit; Star = star;
            CreationTime = ServerLinks.NowTime;
            Statics[type].Fill(this);
            Seed = ServerLinks.BattleRandom.Next(1000000);
            Random rnd = new Random(Seed);
            ServerMission2Static missionstatic = Statics[Type];
            int sidepos = rnd.Next(missionstatic.Sides.Length);
            switch (missionstatic.Sides[sidepos])
            {
                case 'P': Side = EnemySide.Pirate; break;
                case 'M': Side = EnemySide.Mercs; break;
                case 'G': Side = EnemySide.GreenTeam; break;
                case 'T': Side = EnemySide.Techno; break;
                case 'A': Side = EnemySide.Alien; break;
                default: throw new Exception("Ошибка при генерации ресурсной миссии");
            }
        }
        public FleetTargetE GetTargetType()
        {
            return FleetTargetE.Mission;
        }
        /// <summary> массив с данными о миссии для цели флота </summary>
        public byte[] GetTargetArray()
        {
            List<byte> list = new List<byte>();
            list.Add((byte)FleetTargetE.Mission);
            list.Add((byte)Type);
            list.AddRange(BitConverter.GetBytes(Star.ID));
            list.Add(Orbit);
            return list.ToArray();
        }
        public BattleFieldGroup GetBattleField()
        {
            return Statics[Type].BattleField.GetBattleField();
        }
        public Restriction GetRestriction()
        {
            switch (Type)
            {
                case Mission2Type.Anomaly:
                case Mission2Type.RangeRaid: break;
                default: return Restriction.None;
            }
            Restriction[] list = new Restriction[] {Restriction.NoEnergy, Restriction.NoPhysic, Restriction.NoIrregular, Restriction.NoCyber,
                Restriction.DoubleEnergy, Restriction.DoublePhysic, Restriction.DoubleIrregular, Restriction.DoubleCyber};
            Random rnd = new Random(Seed);
            return list[rnd.Next(list.Length)];
        }
        /// <summary> Дополнительные параметры, такие как нестандартные корабли и нестандартные расположения </summary>
        public SideBattleParam GetSide1BattleParams(GSPlayer player, Restriction restriction)
        {
            List<ShipBattleParam> list = new List<ShipBattleParam>();
            ServerMission2Static missionstatic = Statics[Type];
            if (missionstatic.SpecInfo == "") return null;
            Random rnd = new Random(Seed + 1000);
            WeaponGroup[] ExclPrior = GetExcludePriorityGroup(restriction);
            int d = player.GetMission2Dificulty(Type);
            SideBattleParam sideparam = new SideBattleParam("");
            string[] ss = missionstatic.SpecInfo.Split(' ');
            foreach (string s in ss)
            {
                if (s[0] == 'A' && s[1] == 'A')
                {
                    string[] parameters = s.Split('-');
                    string type = parameters[1];
                    byte hex = Byte.Parse(parameters[2]);
                    bool prot = (parameters[3] == "P" ? true : false);
                    ShipBattleParam param = null;
                    if (type == "C")
                        param = GetShip(0, rnd, EnemySide.Player, ExclPrior[0], ExclPrior[1], d);
                    else if (type == "L")
                        param = ShipGenerator2.GetShip(ShipGenerator2Types.Large, EnemySide.Player, d, ExclPrior[0], ExclPrior[1], rnd);
                    else if (type == "B")
                        param = ShipGenerator2.GetShip(ShipGenerator2Types.Building, EnemySide.Player, d, ExclPrior[0], ExclPrior[1], rnd);
                    else
                        throw new Exception("Ошибка при генерации корабля миссии");
                    param.Hex = hex;
                    param.Protected = prot;
                    list.Add(param);
                }
                else if (s[0] == 'L' && s[1] == 'A')
                {
                    string[] parameters = s.Split('-');
                    for (int i = 1; i < parameters.Length; i++)
                        sideparam.ShipsHexes.Add(Byte.Parse(parameters[i]));//добавление хексов, в которых будут размещаться стандратные корабли
                }
                else if (s[0] == 'R' && s[1] == 'A')
                {
                    sideparam.IsRessurect = true;
                }
            }
            sideparam.Ships = list.ToArray();
            return sideparam;
        }
      
        static List<ShipGenerator2Types> Ship3GunList = new List<ShipGenerator2Types>
            (new ShipGenerator2Types[] { ShipGenerator2Types.Devostator, ShipGenerator2Types.Warrior, ShipGenerator2Types.Cruiser });
        static List<ShipGenerator2Types> Ship2GunList = new List<ShipGenerator2Types>
            (new ShipGenerator2Types[] { ShipGenerator2Types.Linkor, ShipGenerator2Types.Frigate, ShipGenerator2Types.Fighter, ShipGenerator2Types.Dreadnought });
        static List<ShipGenerator2Types> Ship1GunList = new List<ShipGenerator2Types>
        (new ShipGenerator2Types[] { ShipGenerator2Types.Scout, ShipGenerator2Types.Corvett, ShipGenerator2Types.Cargo });
        public SideBattleParam GetSide2BattleParams(GSPlayer player, Restriction restriction)
        {
            Random rnd = new Random(Seed);
            ServerMission2Static missionstatic = Statics[Type];
            string emblem = "";
            switch (Side)
            {
                case EnemySide.Pirate: emblem = "IMG:252,0,0,0"; break;
                case EnemySide.Alien: emblem = "IMG:253,0,0,0"; break;
                case EnemySide.GreenTeam: emblem = "IMG:251,0,0,0"; break;
                case EnemySide.Mercs: emblem = "IMG:255,0,0,0"; break;
                case EnemySide.Techno: emblem = "IMG:254,0,0,0"; break;
            }
            SideBattleParam sideparam = new SideBattleParam(emblem);
            List<ShipBattleParam> list = new List<ShipBattleParam>();
            WeaponGroup[] ExclPrior = GetExcludePriorityGroup(restriction);
           
            int d = player.GetMission2Dificulty(Type);
            foreach (int hex in missionstatic.Portals)
            {
                ShipBattleParam portal = ShipGenerator2.GetShip(ShipGenerator2Types.Portal, Side, d, ExclPrior[0], ExclPrior[1], rnd);
                if (hex != 255) portal.Hex = (byte)hex;
                list.Add(portal);
            }
            for (int i = 0; i < missionstatic.Ships3Gun; i++)
                list.Add(GetShip(3, rnd, Side, ExclPrior[0], ExclPrior[1], d));
            for (int i = 0; i < missionstatic.Ships2Gun; i++)
                list.Add(GetShip(2, rnd, Side, ExclPrior[0], ExclPrior[1], d));
            for (int i = 0; i < missionstatic.Ships1Gun; i++)
                list.Add(GetShip(1, rnd, Side, ExclPrior[0], ExclPrior[1], d));

            string[] ss = missionstatic.SpecInfo.Split(' ');
            if (missionstatic.SpecInfo!="") foreach (string s in ss)
            {
                if (s[0] == 'A' && s[1] == 'D')
                {
                    string[] parameters = s.Split('-');
                    string type = parameters[1];
                    byte hex = Byte.Parse(parameters[2]);
                    bool prot = (parameters[3] == "P" ? true : false);
                    ShipBattleParam param = null;
                    if (type == "C")
                        param = GetShip(0, rnd, EnemySide.Player, ExclPrior[0], ExclPrior[1], d);
                    else if (type == "L")
                        param = ShipGenerator2.GetShip(ShipGenerator2Types.Large, EnemySide.Player, d, ExclPrior[0], ExclPrior[1], rnd);
                    else if (type == "B")
                        param = ShipGenerator2.GetShip(ShipGenerator2Types.Building, EnemySide.Player, d, ExclPrior[0], ExclPrior[1], rnd);
                    else
                        throw new Exception("Ошибка при генерации корабля миссии");
                    param.Hex = hex;
                    param.Protected = prot;
                    list.Add(param);
                }
                else if (s[0] == 'L' && s[1] == 'D')
                {
                    string[] parameters = s.Split('-');
                    for (int i = 1; i < parameters.Length; i++)
                        sideparam.ShipsHexes.Add(Byte.Parse(parameters[i]));//добавление хексов, в которых будут размещаться стандратные корабли
                }
                else if (s[0] == 'R' && s[1] == 'D')
                {
                    sideparam.IsRessurect = true;
                }
            }

            sideparam.Ships = list.ToArray();
            return sideparam;
        }
        WeaponGroup[] GetExcludePriorityGroup(Restriction restriction)
        {
            WeaponGroup exclude = WeaponGroup.Any;
            switch (restriction)
            {
                case Restriction.NoEnergy: exclude = WeaponGroup.Energy; break;
                case Restriction.NoPhysic: exclude = WeaponGroup.Physic; break;
                case Restriction.NoIrregular: exclude = WeaponGroup.Irregular; break;
                case Restriction.NoCyber: exclude = WeaponGroup.Cyber; break;
            }
            WeaponGroup include = WeaponGroup.Any;
            switch (restriction)
            {
                case Restriction.DoubleEnergy: include = WeaponGroup.Energy; break;
                case Restriction.DoublePhysic: include = WeaponGroup.Physic; break;
                case Restriction.DoubleIrregular: include = WeaponGroup.Irregular; break;
                case Restriction.DoubleCyber: include = WeaponGroup.Cyber; break;
            }
            return new WeaponGroup[] { exclude, include };
        }
        ShipBattleParam GetShip(int guns, Random rnd, EnemySide side, WeaponGroup exclude, WeaponGroup include, int d)
        {
            List<ShipGenerator2Types> list;
            if (guns == 3)
            {
                list = new List<ShipGenerator2Types>(Ship3GunList);
                switch (side)
                {
                    case EnemySide.Pirate: list.Remove(ShipGenerator2Types.Warrior); break;
                    case EnemySide.Techno: list.Remove(ShipGenerator2Types.Devostator); break;
                    case EnemySide.Mercs: list.Remove(ShipGenerator2Types.Cruiser); break;
                }
            }
            else if (guns == 2)
            {
                list = new List<ShipGenerator2Types>(Ship2GunList);
                switch (side)
                {
                    case EnemySide.Pirate: list.Remove(ShipGenerator2Types.Frigate); break;
                    case EnemySide.GreenTeam: list.Remove(ShipGenerator2Types.Fighter); break;
                    case EnemySide.Mercs: list.Remove(ShipGenerator2Types.Linkor); break;
                    case EnemySide.Techno: list.Remove(ShipGenerator2Types.Dreadnought); break;
                }
            }
            else if (guns == 1)
            {
                list = new List<ShipGenerator2Types>(Ship1GunList);
                switch (side)
                {
                    case EnemySide.GreenTeam: list.Remove(ShipGenerator2Types.Scout); break;
                }
            }
            else if (guns == 0)
            {
                list = new List<ShipGenerator2Types>(new ShipGenerator2Types[] { ShipGenerator2Types.NoGun });
            }
            else
            {
                throw new Exception("Ошибка при подборе кораблей");
            }
            ShipGenerator2Types shiptype = list[rnd.Next(list.Count)];
            return ShipGenerator2.GetShip(shiptype, side, d, exclude, include, rnd);
        }
        static SortedList<Mission2Type, ServerMission2Static> GetList()
        {
            SortedList<Mission2Type, ServerMission2Static> result = new SortedList<Mission2Type, ServerMission2Static>();
            result.Add(Mission2Type.Metheorit, new ServerMission2Static(10, 90, 10, Mission2Location.FreeOrbit, 255,
                new BattleFieldParam("BF:0 AL:20 AI:255 AS:M"), "PMGTA", "", new byte[] { 255 }, 0, 1, 1));
            result[Mission2Type.Metheorit].SetReward(1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
            result[Mission2Type.Metheorit].SetReward(2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0);
            result[Mission2Type.Metheorit].SetReward(3, 0, 5, 0, 0, 1, 1, 0, 0, 0, 0);
            result.Add(Mission2Type.OreBelt, new ServerMission2Static(10, 90, 15, Mission2Location.OreBelt, 255,
                new BattleFieldParam("BF:1 RAND:12"), "PMGTA", "", new byte[] { 255 }, 1, 1, 2));
            result[Mission2Type.OreBelt].SetReward(1, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0);
            result[Mission2Type.OreBelt].SetReward(2, 0, 4, 0, 2, 0, 0, 0, 0, 0, 0);
            result[Mission2Type.OreBelt].SetReward(3, 0, 7, 0, 3, 1, 1, 0, 0, 0, 0);
            result.Add(Mission2Type.Anomaly, new ServerMission2Static(10, 90, 20, Mission2Location.FreeOrbit, 255,
                new BattleFieldParam("BF:0 RAND:3 RI:209"), "TGA", "", new byte[] { 255 }, 2, 2, 1));
            result[Mission2Type.Anomaly].SetReward(1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0);
            result[Mission2Type.Anomaly].SetReward(2, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0);
            result[Mission2Type.Anomaly].SetReward(3, 0, 0, 0, 7, 2, 0, 0, 0, 0, 0);
            result.Add(Mission2Type.ShipWreck, new ServerMission2Static(10, 90, 20, Mission2Location.FreeOrbit, 255,
                new BattleFieldParam("BF:1 AL:42 AI:230 AS:M RAND:2"), "M", "", new byte[] { 255 }, 1, 2, 2));
            result[Mission2Type.ShipWreck].SetReward(1, 0, 3, 3, 0, 0, 0, 0, 0, 0, 0);
            result[Mission2Type.ShipWreck].SetReward(2, 0, 0, 5, 3, 0, 0, 0, 0, 0, 0);
            result[Mission2Type.ShipWreck].SetReward(3, 0, 0, 7, 0, 2, 1, 0, 0, 0, 0);
            result.Add(Mission2Type.PillageProtect, new ServerMission2Static(10, 90, 20, Mission2Location.FreeOrbit, 255,
                new BattleFieldParam("BF:1 RAND:4"), "PA", "AA-B-27-P LD-71-81-82", new byte[] { }, 1, 1, 1));
            //AA-C-27-K - A - дополнительный, A - стороны атаки (D - стороны защиты), С - грузовая модель (L - большой корабль, B - здание (большой без пушек)), 
            //27 - в 27 хексе, P - защищаемый (N - обычный).
            //LA-0-1-2 - L - не дополнительный, A - стороны атаки (D - стороны защиты), 0-1-2 - перечень хексов, в которых располагаются корабли
            //RA - корабли воскрешаются А - сторона атаки (D - сторона защиты)
            
            result[Mission2Type.PillageProtect].SetReward(1, 0, 3, 3, 0, 0, 0, 0, 0, 0, 0);
            result[Mission2Type.PillageProtect].SetReward(2, 0, 0, 5, 3, 0, 0, 0, 0, 0, 0);
            result[Mission2Type.PillageProtect].SetReward(3, 0, 0, 7, 0, 2, 1, 0, 0, 0, 0);
            result.Add(Mission2Type.CrimePursuit, new ServerMission2Static(10, 90, 20, Mission2Location.FreeOrbit, 255,
                new BattleFieldParam("BF:2 RAND:4"), "PM", "LD-143-123-108-94", new byte[] { 255 }, 1, 1, 1));
            result.Add(Mission2Type.FieldDay, new ServerMission2Static(10, 90, 20, Mission2Location.FreeOrbit, 255,
                new BattleFieldParam("BF:2 RAND:8"), "GT", "", new byte[] { 255 }, 3, 3, 4));
            result.Add(Mission2Type.Experiment, new ServerMission2Static(10, 90, 20, Mission2Location.FreeOrbit, 255,
                new BattleFieldParam("BF:0"), "T", "AD-L-30-P", new byte[0], 0, 0, 0));
            result.Add(Mission2Type.ConvoyDefence, new ServerMission2Static(10, 90, 20, Mission2Location.FreeOrbit, 255,
                new BattleFieldParam("BF:2 RAND:4"), "PMG", "AA-C-70-P AA-C-52-P", new byte[] { 255 }, 2, 4, 2));
            result.Add(Mission2Type.ConvoyAttack, new ServerMission2Static(10, 90, 20, Mission2Location.FreeOrbit, 255,
                new BattleFieldParam("BF:1 RAND:4"), "TGM", "AD-C-42-P AD-C-56-P RD", new byte[] { 255 }, 3, 0, 0));
            result.Add(Mission2Type.SmallCompetition, new ServerMission2Static(10, 90, 20, Mission2Location.FreeOrbit, 5,
                new BattleFieldParam("BF:1 AL:19,27,56,64 AS:S,S,S,S,S"), "TGM", "", new byte[] { 255 }, 2, 2, 1));
            result.Add(Mission2Type.LargeCompetition, new ServerMission2Static(10, 90, 20, Mission2Location.FreeOrbit, 10,
                new BattleFieldParam("BF:2 AL:24,27,37,67,76,106,116,119 AS:S,S,S,S,S,S,S,S"), "TGM", "", new byte[] { 255 }, 5, 3, 2));
            result.Add(Mission2Type.RangeRaid, new ServerMission2Static(10, 90, 20, Mission2Location.FreeOrbit, 255,
                new BattleFieldParam("BF:1 RAND:3"), "PA", "AD-L-56-N", new byte[] { 255 }, 2, 2, 2));
            result.Add(Mission2Type.PirateBase, new ServerMission2Static(10, 90, 20, Mission2Location.FreePlanet, 255,
                new BattleFieldParam("BF:2 RAND:10"), "P", "AD-B-108-P RD LD-143-59-73-104", new byte[] { 255 }, 3, 2, 1));
            result[Mission2Type.PirateBase].SetReward(1, 0, 10, 10, 0, 0, 0, 0, 0, 0, 0);
            result[Mission2Type.PirateBase].SetReward(2, 0, 20, 20, 0, 0, 0, 0, 0, 0, 0);
            result[Mission2Type.PirateBase].SetReward(3, 0, 25, 25, 0, 0, 0, 0, 0, 0, 1);
            result.Add(Mission2Type.BlackMarket, new ServerMission2Static(0, 90, 10, Mission2Location.FreeOrbit, 255,
                new BattleFieldParam("BF:1  RAND:7"), "P", "",new byte[] { 255 }, 0, 0, 1));
            
            return result;
        }
        public class ServerMission2Static
        {
            byte EmptyChance; //Шанс что награды нет
            byte BattleChance; //Шанс вступить в бой
            byte Time; //Время в днях, сколько существует миссия
            public Mission2Location Location; //Где распологается миссия
            public byte MaxShips; //Ограничение на максимальное количество кораблей в миссии
            public BattleFieldParam BattleField; //Информация о поле боя
            public string Sides; //Стороны, которые могут быть противниками в миссии {"P-пираты, G-Зелёные, Т-технократы, М-наёмники, А-чужие
            public byte[] Portals; //Хексы порталов. Если 255 - то базовое расположение
            public int BigShips; //Количество больших кораблей в миссии
            public int Ships3Gun; //Количество кораблей с 3 пушками в миссии
            public int Ships2Gun;//Количество кораблей с 2 пушками в миссии
            public int Ships1Gun;//Количество кораблей с 1 пушкой в миссии
            public SortedList<RewardType, int> RewardLow; //Награды. В параметре - модификатор количества или сложности
            public SortedList<RewardType, int> RewardMedium;
            public SortedList<RewardType, int> RewardHigh;
            const int FreeMissionLowReward = 80;//80 Вероятность что в миссии без боя будет малая награда
            const int BattleMissionMediumReward = 90;//90 Вероятность что в миссии с боем будет большая награда
            public string SpecInfo; //дополнительная информация, такая как защищаемые корабли или стартовое расположение кораблей на поле боя
            public ServerMission2Static(byte emptychance, byte battlechance, byte time, Mission2Location location, byte maxships, 
                BattleFieldParam battlefield,string sides, string specInfo, byte[] portals, int ships3Gun, int ships2Gun, int ships1Gun)
            {
                EmptyChance = emptychance;
                BattleChance = battlechance;
                Time = time;
                Location = location;
                MaxShips = maxships;
                BattleField = battlefield;
                BattleField.PortalsDefense = portals;
                Sides = sides;
                Portals = portals;
                BigShips = 0;
                Ships3Gun = ships3Gun;
                Ships2Gun = ships2Gun;
                Ships1Gun = ships1Gun;
                SpecInfo = specInfo;
            }
            public void SetReward(int dif, int money, int metall, int chips, int anti, int exp, int arts, int science, int ships, int pilots, int lands)
            {
                SortedList<RewardType, int> rewards = new SortedList<RewardType, int>();
                if (money > 0) rewards.Add(RewardType.Money, money);
                if (metall > 0) rewards.Add(RewardType.Metall, metall);
                if (chips > 0) rewards.Add(RewardType.Chips, chips);
                if (anti > 0) rewards.Add(RewardType.Anti, anti);
                if (exp > 0) rewards.Add(RewardType.Experience, exp);
                if (arts > 0) rewards.Add(RewardType.Artefact, arts);
                if (science > 0) rewards.Add(RewardType.Science, science);
                if (ships > 0) rewards.Add(RewardType.Ship, ships);
                if (pilots > 0) rewards.Add(RewardType.Pilot, pilots);
                if (lands > 0) rewards.Add(RewardType.Land, lands);
                if (dif == 1) RewardLow = rewards;
                else if (dif == 2) RewardMedium = rewards;
                else RewardHigh = rewards;
            }
            /// <summary> Метод выдаёт вероятность боя для миссии. Используется при создании миссии </summary>
            public void Fill(ServerMission2 mission)
            {
                byte rand = (byte)ServerLinks.BattleRandom.Next(100);
                if (mission.Type == Mission2Type.BlackMarket)
                    mission.Content = Mission2Content.BlackMarket;
                else if (rand < EmptyChance) mission.Content = Mission2Content.Empty;
                else if (rand>(EmptyChance+BattleChance))
                {
                    rand = (byte)ServerLinks.BattleRandom.Next(100);
                    if (rand < FreeMissionLowReward) mission.Content = Mission2Content.FreeLow;
                    else mission.Content = Mission2Content.FreeMedium;
                }
                else
                {
                    rand = (byte)ServerLinks.BattleRandom.Next(100);
                    if (rand < BattleMissionMediumReward) mission.Content = Mission2Content.BattleMedium;
                    else mission.Content = Mission2Content.BattleHigh;
                }
                mission.EndTime = mission.CreationTime + TimeSpan.FromDays(Time);
            }
            
        }
        /// <summary> Метод рассчитывает награду за миссию, в зависимости от параметра сложности. Если активен параметр afterbattle, то с некоторым шансом увеличивается сложность </summary>
        public Reward2 CalcReward(GSPlayer player, bool afterbattle)
        {
            int dif = player.GetMission2Dificulty(Type);
            Random rnd = new Random(Seed);
            Reward2 reward = new Reward2();
            SortedList<RewardType, int> rewardlist;
            if (Content == Mission2Content.FreeLow)
                rewardlist = Statics[Type].RewardLow;
            else if (Content == Mission2Content.FreeMedium || Content == Mission2Content.BattleMedium)
                rewardlist = Statics[Type].RewardMedium;
            else rewardlist = Statics[Type].RewardHigh;
            if (rewardlist!=null) foreach (KeyValuePair<RewardType, int> pair in rewardlist)
            {
                switch (pair.Key)
                {
                    case RewardType.Money: reward.Money = (int)(1000 * Math.Pow(dif+1, 2) * pair.Value); break;
                    case RewardType.Metall:reward.Metall = (int)(200 * Math.Pow(dif+1, 2) * pair.Value); break;
                    case RewardType.Chips: reward.Chips = (int)(200 * Math.Pow(dif+1, 2) * pair.Value); break;
                    case RewardType.Anti: reward.Anti = (int)(200 * Math.Pow(dif+1, 2) * pair.Value); break;
                    case RewardType.Experience: reward.Experience = (dif + 10)/5 * pair.Value; break;
                    case RewardType.Artefact: reward.Artefacts = GetArtefact(pair.Value, rnd); break;
                    case RewardType.Science: reward.Sciences = new ushort[] { player.Sciences.LearnNewScienceFromBattle(null, 0) }; break;
                        //case RewardType.Avanpost: 
                }
            }
            reward.CreateArray();
            return reward;
        }
        ushort[] GetArtefact(int count, Random rnd)
        {
            List<ushort> result = new List<ushort>();
            for (int i = 0; i < count; i++)
                result.Add(ServerLinks.Artefacts.Keys[rnd.Next(ServerLinks.Artefacts.Count)]);
            return result.ToArray();
        }
    }
    public class ServerMission2Result
    {
        public Reward2 Reward;
        public ServerBattle Battle;
        public Mission2Content Content;
        public ServerMission2Result()
        {
            Content = Mission2Content.Empty; Reward = null; Battle = null; 
        }
        public ServerMission2Result(Reward2 reward, bool MediumReward)
        {
            if (MediumReward)
                Content = Mission2Content.FreeMedium;
            else
                Content = Mission2Content.FreeLow;
            Reward = reward;
            Battle = null;
        }
        public ServerMission2Result(ServerBattle battle, bool MediumReward)
        {
            if (MediumReward)
                Content = Mission2Content.BattleMedium;
            else
                Content = Mission2Content.BattleHigh;
            Battle = battle;
            Reward = null;
        }
        public static ServerMission2Result GetBlackMarketResult()
        {
            ServerMission2Result result = new ServerMission2Result();
            result.Content = Mission2Content.BlackMarket;
            return result;
        }
    }
    public class Mission2
    {
        static SortedList<Mission2Type, Mission2Static> Statics=GetList();
        public Mission2Type Type;
        public byte Time;
        public byte Orbit;
        public int StarID;
        public ImageBrush GetBrush { get { return Statics[Type].Brush; } }
        public Mission2(Mission2Type type, byte time, byte orbit, int starid)
        {
            StarID = starid;
            Type = type;
            Time = time;
            Orbit = orbit;
        }
        public string GetTitle()
        {
            switch (Type)
            {
                case Mission2Type.Metheorit:
                   return "Разработка метеорита";
                case Mission2Type.Anomaly:
                    return "Космическая аномалия";
                case Mission2Type.OreBelt:
                    return "Рейд в пояс астероидов";
                case Mission2Type.ShipWreck:
                    return "Исследование обломков корабля";
                case Mission2Type.PillageProtect:
                    return "Защита от грабежа";
                case Mission2Type.CrimePursuit:
                    return "Преследование преступников";
                case Mission2Type.FieldDay:
                    return "Маневры";
                case Mission2Type.Experiment:
                    return "Научный эксперимент";
                case Mission2Type.ConvoyDefence:
                    return "Сопровождение конвоя";
                case Mission2Type.ConvoyAttack:
                    return "Атака конвоя";
                case Mission2Type.SmallCompetition:
                    return "Участие в малом турнире";
                case Mission2Type.LargeCompetition:
                    return "Участие в большом турнире";
                case Mission2Type.RangeRaid:
                    return "Исследовательская миссия";
                case Mission2Type.PirateBase:
                    return "Атака на базу пиратов";
                case Mission2Type.BlackMarket:
                    return "Чёрный рынок";
                default:
                    return "Error";
            }
        }
        static SortedList<Mission2Type,Mission2Static> GetList()
        {
            SortedList<Mission2Type, Mission2Static> list = new SortedList<Mission2Type, Mission2Static>();
            list.Add(Mission2Type.Metheorit, new Mission2Static(Gets.AddPicMission("MeteoritMissionImage")));
            list.Add(Mission2Type.Anomaly, new Mission2Static(Gets.AddPicMission("AnomalyMissionImage")));
            list.Add(Mission2Type.OreBelt, new Mission2Static(Gets.AddPicMission("OreBeltMissionImage")));
            list.Add(Mission2Type.ShipWreck, new Mission2Static(Gets.AddPicMission("ShipWreckMissionImage")));
            list.Add(Mission2Type.PillageProtect, new Mission2Static(Gets.AddPicMission("PillageProtectMissionImage")));
            list.Add(Mission2Type.CrimePursuit, new Mission2Static(Gets.AddPicMission("CrimePursiutMissionImage")));
            list.Add(Mission2Type.FieldDay, new Mission2Static(Gets.AddPicMission("FieldDayMissionImage")));
            list.Add(Mission2Type.Experiment, new Mission2Static(Gets.AddPicMission("ExperimentMissionImage")));
            list.Add(Mission2Type.ConvoyDefence, new Mission2Static(Gets.AddPicMission("ConvoyDefenceMissionImage")));
            list.Add(Mission2Type.ConvoyAttack, new Mission2Static(Gets.AddPicMission("ConvoyAttackMissionImage")));
            list.Add(Mission2Type.SmallCompetition, new Mission2Static(Gets.AddPicMission("SmallCompetitionMissionImage")));
            list.Add(Mission2Type.LargeCompetition, new Mission2Static(Gets.AddPicMission("LargeCompetitionMissionImage")));
            list.Add(Mission2Type.RangeRaid, new Mission2Static(Gets.AddPicMission("RangeRaidMissionImage")));
            list.Add(Mission2Type.PirateBase, new Mission2Static(Gets.AddPicMission("PirateBaseMissionImage")));
            list.Add(Mission2Type.BlackMarket, new Mission2Static(Gets.AddPicMission("BlackMarketMissionImage")));
            return list;
        }
        public void ShowMissionPanel()
        {
            Mission2Panel panel = new Mission2Panel(this);
            Links.Controller.PopUpCanvas.Place(panel);
        }
        public Mission2Location GetLocation()
        {
            return ServerMission2.Statics[Type].Location;
        }
        class Mission2Static
        {
            public ImageBrush Brush;
            public Mission2Static(ImageBrush brush)
            {
                Brush = brush;
            }
        }
    }
    class Mission2Panel : FleetMissionPanel
    {
        enum RewardStatus { Basic, No, Low, Medium}
        public static Mission2 CurrentMission;
        public static new Mission2Panel CurrentPanel;
        InterfaceButton StartButton;
        public Mission2Panel(Mission2 mission)
        {
            CurrentMission = mission;
            CurrentPanel = this;
            string text = "";
            TitleBlock.Text = mission.GetTitle();
            switch (mission.Type)
            {
                case Mission2Type.Metheorit:
                    PersonName.Text = "Советник";
                    PutImage(Links.Brushes.Helper0);
                    text = "В системе обнаружен метеорит, который состоит из редких металлов. Предлагаем Вам отправить флот для его транспортировки и переработки. Будьте осторожны, возможно, хотя и маловероятно, столкновение интересов с другими сторонами. ";
                    break;
                case Mission2Type.Anomaly:
                    PersonName.Text = "Советник";
                    PutImage(Links.Brushes.Helper0);
                    text = "В системе зарегистрирована странная аномалия. Советую вам направить туда несколько кораблей для разведки. Очень часто в таких местах обнаруживается выброс антиматерии. Думаю о необходимости быть бдительным мне вам напоминать не стоит. ";
                    break;
                case Mission2Type.OreBelt:
                    PersonName.Text = "Советник";
                    PutImage(Links.Brushes.Helper0);
                    text = "В поясе астероидов обнаружена группа астероидов, в составе которых прогнозируется высокая концентрация ресурсных элементов. Будьте осторожны, в поясе астероидов велика вероятность нарваться на засаду.  ";
                    break;
                case Mission2Type.ShipWreck:
                    PersonName.Text = "Советник";
                    PutImage(Links.Brushes.Helper0);
                    text = "Наш радар обнаружил вывалившийся из варпа и дрейфующий остов грузового корабля. Видимо на нём произошла авария, а сигнал SOS подать не смогли. Думаю стоит его обследовать, возможно в его трюмах осталось что-то ценное. Но будьте готовы встретится с наёмниками, они любят подобные призы.";
                    break;
                case Mission2Type.PillageProtect:
                    PersonName.Text = "Советник";
                    PutImage(Links.Brushes.Helper0);
                    text = "Мы получили сигнал SOS от грузового корабля, который атакуют пираты. Нужно как можно скорее отправить один из вашиъ флотов на помощь. Тем более, нам обещают щедро заплатить за это. Но надо не просто победить, корабль нанимателей не должны уничтожить.";
                    break;
                case Mission2Type.CrimePursuit:
                    PersonName.Text = "Советник";
                    PutImage(Links.Brushes.Helper0);
                    text = "Нам поступила ориентировка на группу нарушителей, за головы которых объявлена солидная награда.";
                    break;
                case Mission2Type.FieldDay:
                    PersonName.Text = "Советник";
                    PutImage(Links.Brushes.Helper0);
                    text = "Бой на большом поле боя, где много кораблей";
                    break;
                case Mission2Type.Experiment:
                    PersonName.Text = "Советник";
                    PutImage(Links.Brushes.Helper0);
                    text = "Бой на малом поле боя против крупного корабля";
                    break;
                case Mission2Type.ConvoyDefence:
                    PersonName.Text = "Советник";
                    PutImage(Links.Brushes.Helper0);
                    text = "Бой на большом поле боя, где надо защитить конвой";
                    break;
                case Mission2Type.ConvoyAttack:
                    PersonName.Text = "Советник";
                    PutImage(Links.Brushes.Helper0);
                    text = "Бой на среднем поле, надо уничтожить конвой, бесконечная защита";
                    break;
                case Mission2Type.SmallCompetition:
                    PersonName.Text = "Советник";
                    PutImage(Links.Brushes.Helper0);
                    text = "Бой флотом до 5 кораблей на среднем поле боя с фиксированными препятствиями";
                    break;
                case Mission2Type.LargeCompetition:
                    PersonName.Text = "Советник";
                    PutImage(Links.Brushes.Helper0);
                    text = "Бой флотом до 10 кораблей на большом поле боя с фиксированными препятствиями";
                    break;
                case Mission2Type.RangeRaid:
                    PersonName.Text = "Советник";
                    PutImage(Links.Brushes.Helper0);
                    text = "Среднее поле боя, крупный корабль и малые";
                    break;
                case Mission2Type.PirateBase:
                    PersonName.Text = "Советник";
                    PutImage(Links.Brushes.Helper0);
                    text = "Большое поле боя, большая база и малые бессмертные";
                    break;
                case Mission2Type.BlackMarket:
                    PersonName.Text = "Адмирал";
                    PutImage(Links.Brushes.Helper2_1);
                    text = "Мы получили информацию об открытии подпольного чёрного рынка. Можем наведаться туда. Обычно на чёрном рынке щире ассортимент товаров и ниже цены. Но не стоит ходить туда самому, посылайте флот и посильнее. Рынок держат пираты, а им не свойственно джентельменство. По выходу с рынка вас могут попробовать ограбить. ";
                    break;
                default:
                    PersonName.Text = "Error";
                    text = "Error";
                    break;
            }

            FleetMissionTextBlock block = new FleetMissionTextBlock(text, 26, 690);
            textborder.Child = block;
            block.Start();

            //PlaceRewardAllInfo();
            StartButton = new InterfaceButton(200, 70, 7, 26);

            StartButton.SetText(Links.Interface("SelectFleet"));
            StartButton.PreviewMouseDown += SelectFleetForMission2;
            StartButton.PutToCanvas(this, 250, 600);

            PlaceRewardAllInfo(RewardStatus.Basic);
        }
        void PlaceRewardAllInfo(RewardStatus status)
        {
            List<RewardType> rewardtypes = new List<RewardType>();
            if (status == RewardStatus.Basic)
            {
                switch (CurrentMission.Type)
                {
                    case Mission2Type.Metheorit: rewardtypes.Add(RewardType.Metall); break;
                    case Mission2Type.OreBelt: rewardtypes.Add(RewardType.Metall); rewardtypes.Add(RewardType.Anti); break;
                    case Mission2Type.Anomaly: rewardtypes.Add(RewardType.Anti); break;
                }
            }
            else if (status==RewardStatus.Low)
            {
                switch (CurrentMission.Type)
                {
                    case Mission2Type.Metheorit: rewardtypes.Add(RewardType.Metall); break;
                    case Mission2Type.OreBelt: rewardtypes.Add(RewardType.Metall);break;
                    case Mission2Type.Anomaly: rewardtypes.Add(RewardType.Anti); break;
                }
            }
            else if (status==RewardStatus.Medium)
            {
                switch (CurrentMission.Type)
                {
                    case Mission2Type.Metheorit: rewardtypes.Add(RewardType.Metall); break;
                    case Mission2Type.OreBelt: rewardtypes.Add(RewardType.Metall); rewardtypes.Add(RewardType.Anti); break;
                    case Mission2Type.Anomaly: rewardtypes.Add(RewardType.Anti); break;
                }
            }
            Canvas RewardCanvas = new Canvas();
            RewardCanvas.Height = 145;
            rewardborder.Child = RewardCanvas;
            int rewardpos = 0;
            foreach (RewardType type in rewardtypes)
            {
                switch (type)
                {
                    case RewardType.Money: PutRewardInfo(Links.Brushes.MoneyImageBrush, rewardpos, RewardCanvas, "Деньги"); rewardpos += 110; break;
                    case RewardType.Metall: PutRewardInfo(Links.Brushes.MetalImageBrush, rewardpos, RewardCanvas, "Металлы"); rewardpos += 110; break;
                    case RewardType.Chips: PutRewardInfo(Links.Brushes.ChipsImageBrush, rewardpos, RewardCanvas, "Микросхемы"); rewardpos += 110; break;
                    case RewardType.Anti: PutRewardInfo(Links.Brushes.AntiImageBrush, rewardpos, RewardCanvas, "Антиматерия"); rewardpos += 110; break;
                    case RewardType.Experience: PutRewardInfo(Links.Brushes.ExperienceBrush, rewardpos, RewardCanvas, "Увеличенный опыт пилотов"); rewardpos += 110; break;
                    case RewardType.Artefact: PutRewardInfo(Links.Brushes.ArtefactsBrush, rewardpos, RewardCanvas, "Артефакты"); rewardpos += 110; break;
                    case RewardType.Avanpost: PutRewardInfo(LandChanger.PlanetInfo.GetBrush("BuildIcon"), rewardpos, RewardCanvas, "Аванпосты"); rewardpos += 110; break;
                    case RewardType.Land: PutRewardInfo(Links.Brushes.LandIconBrush, rewardpos, RewardCanvas, "Колония"); rewardpos += 110; break;
                    case RewardType.Pilot: PutRewardInfo(Gets.GetIntBoyaImage("Man"), rewardpos, RewardCanvas, "Пилот"); rewardpos += 110; break;
                    case RewardType.Science: PutRewardInfo(Links.Brushes.SciencePict, rewardpos, RewardCanvas, "Технологии"); rewardpos += 110; break;
                    case RewardType.Ship: PutRewardInfo(Links.Brushes.ShipCreate, rewardpos, RewardCanvas, "Корабль"); rewardpos += 110; break;
                }
            }
            RewardCanvas.Width = rewardpos;
            if (rewardpos > 700)
            {
                Canvas MovinCanvas = new Canvas(); MovinCanvas.Width = 695; MovinCanvas.Height = 145;
                rewardborder.Child = MovinCanvas;
                Path pathleft = new Path(); pathleft.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,70 l40,-70 v140 z"));
                pathleft.Fill = Links.Brushes.SkyBlue;
                MovinCanvas.Children.Add(pathleft); pathleft.Tag = -200; pathleft.PreviewMouseDown += RewardCanvasMove;
                Canvas.SetTop(pathleft, 2.5);
                Path pathright = new Path(); pathright.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 l40,70 l-40,70z"));
                pathright.Fill = Links.Brushes.SkyBlue;
                MovinCanvas.Children.Add(pathright); pathright.Tag = 200; pathright.PreviewMouseDown += RewardCanvasMove;
                Canvas.SetLeft(pathright, 655); Canvas.SetTop(pathright, 2.5);
                rewardviewer = new ScrollViewer();
                rewardviewer.Width = 615; rewardviewer.Height = 145;
                rewardviewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                rewardviewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                rewardviewer.Content = RewardCanvas;
                Canvas.SetLeft(rewardviewer, 40);
                MovinCanvas.Children.Add(rewardviewer);
            }


        }
        private void RewardCanvasMove(object sender, MouseButtonEventArgs e)
        {
            Path path = (Path)sender;
            int delta = (int)path.Tag;
            rewardviewer.ScrollToHorizontalOffset(rewardviewer.HorizontalOffset + delta);
        }
        public void PutRewardInfo(Brush brush, int left, Canvas canvas, string tip)
        {
            Rectangle rect = Common.GetRectangle(100, brush);
            canvas.Children.Add(rect);
            rect.ToolTip = tip;
            Canvas.SetLeft(rect, left);
            Canvas.SetTop(rect, 22);
        }
        /// <summary> Метод анализирует возвращаемую сервером информацию о направлении флота на миссию. 
        /// возвращаемое значение - надо или нет переключаться на панель флота </summary>
        /// <param name="battleid"></param>
        /// <returns></returns>
        public bool RecieveMissionResult(long battleid)
        {
            Children.Remove(StartButton);
            if (battleid==-4)
            {
                Gets.GetTotalInfo("После отправки флота на миссию");
                Links.Controller.SelectPanel(GamePanels.Market, SelectModifiers.BlackMarket);
                return false;
            }
            else if (battleid <= -1)
            {
                Gets.GetTotalInfo("После отправки флота на миссию");
                string text = "Error";
                if (battleid == -1)
                {
                    switch (Mission2Panel.CurrentMission.Type)
                    {
                        case Mission2Type.Metheorit: text = "К сожалению, метеорит оказался пустышкой, мы не нашли ничего ценного. Через день флот будет готов к новой задаче."; break;
                        case Mission2Type.OreBelt: text = "Наши разведчики ошиблись, это обычные ледяные астероиды, мы не смогли собрать ничего ценного."; break;
                        case Mission2Type.Anomaly: text = "Похоже аномалия успела рассеяться до прибытия ваших кораблей. Мы не смогли собрать ничего ценного. Через день флот будет готов к новой задаче."; break;
                        case Mission2Type.ShipWreck: text = "Разведичики докладывают, что мы опоздали, всё ценное из его трюмов кто-то уже выгреб. Ну что ж, в следующий раз будем быстрее."; break;
                        case Mission2Type.PillageProtect: text = "К сожалению, мы опоздали и корабль был уничтожен пиратами. Мы не смогли ничего заработать. "; break;
                        case Mission2Type.CrimePursuit: text = "Мы не получим награды, нарушители успели покинуть нашу территорию."; break;
                        case Mission2Type.FieldDay: text = "Нет награды"; break;
                        case Mission2Type.Experiment: text = "Нет награды"; break;
                        case Mission2Type.ConvoyDefence: text = "Нет награды"; break;
                        case Mission2Type.ConvoyAttack: text = "Нет награды"; break;
                        case Mission2Type.SmallCompetition: text = "Нет награды"; break;
                        case Mission2Type.LargeCompetition: text = "Нет награды"; break;
                        case Mission2Type.RangeRaid: text = "Нет награды"; break;
                        case Mission2Type.PirateBase: text = "Нет награды"; break;
                    }
                    PlaceRewardAllInfo(RewardStatus.No); 
                }
                else if (battleid == -2)
                {
                    switch (Mission2Panel.CurrentMission.Type)
                    {
                        case Mission2Type.Metheorit: text = "Мы нашли металлы, но не так много как хотелось бы. Но это тоже неплохо. Флоту понадобится 3 дня, что бы транспортировать метеорит на базу и подготовится к следующему заданию.";
                            break;
                        case Mission2Type.OreBelt: text = "Только один астероид представлял из себя нечто ценное. Но и один неплохо, мы получили немного металлов. "; break;
                        case Mission2Type.Anomaly: text = "Аномалия уже начала исчезать к вашему прибытию, но кое-что собрать удалось. Флоту понадобится 3 дня, что бы всё собрать, транспортировать на склад и подготовится к следующему заданию."; break;
                        case Mission2Type.ShipWreck: text = "Больших трофеев мы не получили, но кое что есть. Мы займёмся выгрузкой товаров."; break;
                        case Mission2Type.PillageProtect: text = "К нашему прибытию корабль уже сумел самостоятельно отбиться от нападения. Но за ложный вызов нам причитается небольшая компенсация."; break;
                        case Mission2Type.CrimePursuit: text = "Мы не получим награды, нарушители успели покинуть нашу территорию."; break;
                        case Mission2Type.FieldDay: text = "Малая награда"; break;
                        case Mission2Type.Experiment: text = "Малая награда"; break;
                        case Mission2Type.ConvoyDefence: text = "Малая награда"; break;
                        case Mission2Type.ConvoyAttack: text = "Малая награда"; break;
                        case Mission2Type.SmallCompetition: text = "Малая награда"; break;
                        case Mission2Type.LargeCompetition: text = "Малая награда"; break;
                        case Mission2Type.RangeRaid: text = "Малая награда"; break;
                        case Mission2Type.PirateBase: text = "Малая награда"; break;
                    }
                    PlaceRewardAllInfo(RewardStatus.Low); 
                    NoticeBorder.CheckNotice();
                }
                else if (battleid == -3)
                {
                    switch (Mission2Panel.CurrentMission.Type)
                    {
                        case Mission2Type.Metheorit: text = "Этот метеорит был весьма неплох, много редких металлов мы на нём обнаружили. Его надо будет охранять до полной разборки, так что флот будет занят около 5 дней.";
                             break;
                        case Mission2Type.OreBelt: text = "Астероиды были богаты металлами и мы даже нашли немного антиматерии. Это определённо удача. Флот будет готов к следующему заданию через 5 дней."; break;
                        case Mission2Type.Anomaly: text = "Эта аномалия была весьма богата антиматерией. Её надо будет охранять до полной сепарации, так что флот будет занят около 5 дней."; break;
                        case Mission2Type.ShipWreck: text = "Это был пиратский корабль и вёз он весьма ценный груз. Не думаю что кто-то предъявит на него права, смело можем взять всё что нашли себе."; break;
                        case Mission2Type.PillageProtect: text = "Как только мы прибыли грабители дали дёру! Наш наниматель был весьма рад такому результату и щедро оплатил беспокойство."; break;
                        case Mission2Type.CrimePursuit: text = "Увидев ваши войска нарушители сдались без боя. Что ж, победа есть победа, мы получим свою награду"; break;
                        case Mission2Type.FieldDay: text = "Средняя награда"; break;
                        case Mission2Type.Experiment: text = "Средняя награда"; break;
                        case Mission2Type.ConvoyDefence: text = "Средняя награда"; break;
                        case Mission2Type.ConvoyAttack: text = "Средняя награда"; break;
                        case Mission2Type.SmallCompetition: text = "Средняя награда"; break;
                        case Mission2Type.LargeCompetition: text = "Средняя награда"; break;
                        case Mission2Type.RangeRaid: text = "Средняя награда"; break;
                        case Mission2Type.PirateBase: text = "Средняя награда"; break;
                    }
                    PlaceRewardAllInfo(RewardStatus.Medium);
                    NoticeBorder.CheckNotice();
                }
                Mission2Panel.CurrentPanel.ChangeMainText(text);
                Links.Controller.PopUpCanvas.Place(this);
            }
            else
                new StartBattlePanel(battleid);
            return true;
        }
        public void ChangeMainText(string text)
        {
            FleetMissionTextBlock block = new FleetMissionTextBlock(text, 26, 690);
            textborder.Child = block;
            block.Start();
        }
        private void SelectFleetForMission2(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            Links.Controller.PopUpCanvas.Remove();
            Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.FleetForMission);
        }
    }
}
