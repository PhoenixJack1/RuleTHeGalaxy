using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class ServerLinks
    {
        public static EGameMode GameMode = EGameMode.Single;
        public static DateTime NowTime;
        public static SortedList<int, GSAccount> GSAccountsByID;
        public static Random BattleRandom = new Random();
        public static byte[][] RandomArrays = GetRandomArrays();
        public static GSMarket Market;
        static byte[][] GetRandomArrays()
        {
            byte[][] result = new byte[20][];
            for (int i=0;i<20;i++)
            {
                result[i] = new byte[100];
                BattleRandom.NextBytes(result[i]);
            }
            return result;
        }
        public static byte GetRandomByte(Battle battle, int max, int delta)
        {
            if (battle.CurMode == BattleMode.Mode1)
                return (byte)BattleRandom.Next(max);
            return (byte)(battle.RandomArray[(battle.CurrentTurn + delta) % 100] * (max - 1) / 255);
        }
        public static byte GetRandomByte(ServerBattle battle, int max, int delta)
        {
            if (battle.CurMode == BattleMode.Mode1)
                return (byte)BattleRandom.Next(max);
            return (byte)(RandomArrays[battle.randomgroup][(battle.CurrentTurn + delta) % 100] * (max-1) / 255);
        }
        public static SortedList<int, GSPlayer> GSPlayersByID;
        public static GSPlayer MainPlayer;
        public static GSPlayer GreenTeam;
        public static GSPlayer TechnoTeam;
        public static GSPlayer Alien;
        public static GSPlayer MercTeam;
        public static GSPlayer PirateTeam;
        public static SortedList<string, ServerClan> GSClansByName;
        public static SortedList<int, ServerClan> GSClanByID;
        public static SortedList<long, ServerBattle> Battles = new SortedList<long, ServerBattle>();
        public static SortedList<int, ServerStar> GSStars = new SortedList<int, ServerStar>();
        public static SortedList<int, ServerPlanet> GSPlanets;
        public static SortedList<int, ServerLand> GSLands = new SortedList<int, ServerLand>();
        public static SortedList<int, ServerAvanpost> Avanposts = new SortedList<int, ServerAvanpost>();
        public static LandsIDclass LandsID = new LandsIDclass();
        public static SortedList<ushort, GSBuilding> GSBuildings=Links.Buildings;
        public static SortedList<ushort, Artefact> Artefacts = Links.Artefacts;
        public static SortedList<MissionType, MissionParam> Missions = MissionParam.GetMissions();
        public static bool IsLandLoadLastVar = false;
        public class Science
        {
            public static int ScienceLevelMax = 2;
            public static SortedList<ushort, GameScience> GameSciences = Links.Science.GameSciences;
            public static SortedList<ushort, GameScience> NewLands;
            public static Random ScienceRandom;
            public static List<int> BasicScienceList;
            public static int MaxScinecesNeedsToBeKnownToLearnNextlevel = 5;
            //public static SciencePrice[] SciencePrices = Links.Science.SciencePrices;
        }
        public class ShipParts
        {
            public static SortedList<ushort, ShipTypeclass> ShipTypes = Links.ShipTypes;
            public static SortedList<ushort, Computerclass> ComputerTypes = Links.ComputerTypes;
            public static SortedList<ushort, Engineclass> EngineTypes = Links.EngineTypes;
            public static SortedList<ushort, Generatorclass> GeneratorTypes = Links.GeneratorTypes;
            public static SortedList<ushort, Shieldclass> ShieldTypes = Links.ShieldTypes;
            public static SortedList<EWeaponType, WeaponModifer> Modifiers = Links.Modifiers;
            public static SortedList<ArmorType, ShipArmor> Armors = Links.Armors;
            public static SortedList<ushort, Weaponclass> WeaponTypes = Links.WeaponTypes;
            public static SortedList<ushort, Equipmentclass> EquipmentTypes = Links.EquipmentTypes;
            public static void Update()
            {
                ShipTypes = Links.ShipTypes;
                ComputerTypes = Links.ComputerTypes;
                EngineTypes = Links.EngineTypes;
                GeneratorTypes = Links.GeneratorTypes;
                ShieldTypes = Links.ShieldTypes;
                Modifiers = Links.Modifiers;
                Armors = Links.Armors;
                WeaponTypes = Links.WeaponTypes;
                EquipmentTypes = Links.EquipmentTypes;
            }
        }
        

        public class Parameters
        {
            public static TimeSpan ColonyLandTime = TimeSpan.FromDays(10); //время на захват колонии (4 часа - мульти, 10 дней - сингл)
            public static TimeSpan BaseFleetWaitTimeWin = TimeSpan.FromDays(1); //базовое время возвращения флота при победе (5 минут - мульти, 1 день - сингл)
            public static TimeSpan BaseFleetWaitTimeLose = TimeSpan.FromDays(5); //базовое время возвращения флота при поражении (30 минут - мульти, 5 дней - сингл)
            public static TimeSpan FleetSpeedModificator = TimeSpan.FromDays(1); //модификатор скорости движения флота (1 минута - мульти, 1 день - сингл)
            public static int MinShipForAttack = 5; //Минимальное число кораблей, необходимых для атаки игрока 5
            public static double UnProtectedResources = 0.9; //Количество ресурсов под охраной
            public static int NewClanPrice = 100000; //Цена создания нового клана
            public static double ClanChangeDelay = 86400.0; //Время задержки после смены клана, в секундах
            public static double InBattleTurnTime = 1; //Длительность одного хода в бою в минутах
            public static int MaxLandsCount = 4; //Количество колоний, доступное для владения в начале игры 3
            public static double RiotPrepare = 1.0 / 60; //Длительность подготовки к восстанию в часах 12
            public static double RiotLength = 4; //Длительность восстания в часах 4
            public static int EnemyGrowTime = 3600; //Время до роста врага в секундах 3600
            public static TimeSpan PeopleGrowTime = TimeSpan.FromDays(10); //Не используется, в сингле каждое первое число
            public static TimeSpan ResourceAddTime = TimeSpan.FromDays(1);//Время между прибавками ресурсов (1 минута - мульти, 1 день - сингл)
            public static TimeSpan MissionVeryRareGrowTime = TimeSpan.FromDays(20); //Время прироста очень редких миссий (4 часа - мульти, 20 дней - сингл)
            public static TimeSpan MissionRareGrowTime = TimeSpan.FromDays(10); //Время прироста редких миссий (1 час - мульти, 10 дней - сингл)
            public static TimeSpan MissionOftenGrowTime = TimeSpan.FromDays(5); //Время прироста обычных миссий (30 минут - мульти, 5 дней - сингл)
            public static int PremiumOrionPrice = 5000; //цена ориона
            public static int PremiumQuickStartPrice = 2500; //цена быстрого старта
            public static int PremiumNoMoneyCapacity = 1500; //цена снятия ограничений на количество денег
            public static int PremiumFullScience = 800; //цена снятия ограничений на уровень исследований
            public static int PremiumNoDelete = 300; //цена снятия ограничения на удаление аккаунта
            public static int PremiumFullPayPrice = 5000; //цена постоянного доступа
            public static int PremiumOneMonthPrice = 300; //цена одного месяца премиума
            public static int PremiumThreeMonthPrice = 800; //цена трёх месяцев премиума
            public static int PremiumSixMonthPrice = 1500; //цена 6 месяцев премиума
            public static int PremiumOneYearPrice = 2500; //цена одного года премимум
            public static int PremiumMaxScienceLevel = 9; //максимально доступный уровень заклинаний без премиума
            public static int PremiumMoneyCapacity = 1000000; //максимально доступное количество денег без премимиума
            public static int PremiumMaxLands = 4; //максимальное количество земель без премиума 2
            public static int PremiumMaxFleets = 3; //максимальное количество флотов без премиума
            public static int PremiumMaxShips = 15; //максимальное количество кораблей без премиума
            public static int PremiumOrionID = 2591; //ID Эдема
            public static int PremiumQuickStartLevel = 5; //Уровень технологий, получаемый при быстром страрте
            public static TimeSpan PremiumUpdateTime = TimeSpan.FromHours(24); //Время, в течение которого происходит прирост ресурсов после последнего захода для премиума
            public static TimeSpan NotPremiumUpdateTime = TimeSpan.FromHours(4); //Время, в течение которого происходит прирост ресурсов после последнего захода для не премиума
            public static int BaseSaveTime = 600; //Время сохранения базы в секундах 600
            public static double GreenPlanetGrowBonus = 0.01; //Бонус зелёной планеты к росту населения - 1% от максимального населения
            public static double BurnPlanetMetallAddBonus = 1.5;//Бонус выжженой планеты к добыче металла
            public static double GasPlanetAntiBonus = 1.5; //Бонус газовой планеты к добыче антиматерии
            public static double FreezedCapBonus = 1.2; //Бонус замороженной планеты к складу
            public static int PlayerMetallBaseCapacity = 1000; //Начальный склад металла (Без колоний)
            public static int PlayerChipsBaseCapacity = 1000; //Начальный склад микросхем (Без колоний)
            public static int PlayerAntiBaseCapacity = 1000; //Начальный склад антиматерии (Без колоний)
        }
        public class Enemy
        {
            //public static SortedSet<int> FreeStars;
            public static SortedSet<int> EnemyBases;
            public static SortedSet<int> EnemyBounds;
            public static SortedSet<int> CentralStars;
            public static SortedSet<int> PerimeterStars;
            public static List<MissionParam> VeryRareMissions;
            public static List<MissionParam> RareMissions;
            public static List<MissionParam> OftenMissions;
            /*public static void Init(Random rnd)
            {
                //FreeStars = new SortedSet<int>();
                //EnemyBases = new SortedSet<int>(new int[] { 241, 242, 330, 331, 323, 324, 325, 329, 327, 240 });
                //EnemyBounds = new SortedSet<int>(new int[] { 239, 238, 193, 328, 189, 188, 212, 190, 194, 192, 237, 235, 236 });
                CentralStars = new SortedSet<int>();
                PerimeterStars = new SortedSet<int>();
                EnemyBases = new SortedSet<int>();
                EnemyBounds = new SortedSet<int>();
                ServerStar sun = ServerLinks.GSStars[0];
                foreach (ServerStar star in ServerLinks.GSStars.Values)
                {
                    double d = Math.Sqrt(Math.Pow(star.X - sun.X, 2) + Math.Pow(star.Y - sun.Y, 2));
                    if (d > 130) PerimeterStars.Add(star.ID);
                    else CentralStars.Add(star.ID);
                    if (star.X<-200 && star.Y<-100)
                    {
                        int val = rnd.Next(2);
                        if (val == 0) EnemyBases.Add(star.ID);
                        else EnemyBounds.Add(star.ID);
                    }
                }

                //CentralStars = new SortedSet<int>(new int[] { 39, 206, 298, 223, 313, 26, 276, 72, 5, 81, 80, 55, 207, 191, 22, 63, 117, 154, 33, 319, 198, 45, 0, 131 });
                //PerimeterStars = new SortedSet<int>(new int[] {241,242,330,331,323,324,188,14,13,12,11,10,164,163,162,4,70,138,140,144,143,28,25,257,258,265,160,161,156,348,222,352,351,
                //23,145,146,149,150,151,356,221,359,357,374,381,373,220,219,277,264,363,366,367,368,62,61,60,59,238,239});
                VeryRareMissions = new List<MissionParam>(new MissionParam[] { ServerLinks.Missions[MissionType.PirateBase], ServerLinks.Missions[MissionType.ArtifactSearch] });
                RareMissions = new List<MissionParam>(new MissionParam[]{ServerLinks.Missions[MissionType.LongRangeRaid],ServerLinks.Missions[MissionType.BigCompetition],
                    ServerLinks.Missions[MissionType.AlienBases],ServerLinks.Missions[MissionType.ScienceExpedition],ServerLinks.Missions[MissionType.PirateShipyard]});
                OftenMissions = new List<MissionParam>(new MissionParam[]{ServerLinks.Missions[MissionType.ConvoyDestroy],ServerLinks.Missions[MissionType.ConvoyDefense],
                    ServerLinks.Missions[MissionType.Competition],ServerLinks.Missions[MissionType.AlienBounds]});
            }
            */
        }
       /* public class Pilots
        {
            public static byte Nicks;
            public static byte JapCities;
            public static byte JapFemNames;
            public static byte JapManNames;
            public static byte JapSurNames;
            public static byte RuCities;
            public static byte RuFemNames;
            public static byte RuManNames;
            public static byte RuSurNames;
            public static byte AmerCities;
            public static byte AmerFemNames;
            public static byte AmerManNames;
            public static byte AmerSurNames;


        }*/
    }
}
