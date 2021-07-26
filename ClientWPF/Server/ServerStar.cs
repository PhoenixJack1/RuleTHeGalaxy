using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    
    public class ServerStar : FleetTargetI
    {
        public int ID;
        public GSString En;
        public GSString Ru;
        public double X;
        public double Y;
        public EStarClass Class;
        public byte SizeModifier;
        public List<int> PlanetsID;
        public SortedList<byte, ServerPlanet> Planets;
        //public byte Enemy;
        //public static byte[] EnemyArray;
        public static byte[] FreeLandsArray;
        public List<ServerStar> NearStars = new List<ServerStar>();
        public byte OreBelt = 255;//до 10 - конкретная орбита, 255 - нет, 20 - случайная позиция
        public SortedSet<byte> FreeOrbits;
        public Mission2Inspector MissionInspector;
        public List<ServerFleet> ScoutedFleets;
        public ServerStar(int id, GSString en, GSString ru, double x, double y, EStarClass starclass, byte sizemodi, int orebelt)
        {
            ID = id;
            En = en;
            Ru = ru;
            X = x;
            Y = y;
            Class = starclass;
            SizeModifier = sizemodi;
            PlanetsID = new List<int>();
            Planets = new SortedList<byte, ServerPlanet>();
            if (orebelt == -1) OreBelt = 20; else OreBelt = (byte)orebelt;
            ScoutedFleets = new List<ServerFleet>();
        }
        public class Mission2Inspector
        {
            ServerStar Star;
            SortedSet<byte> BadPlanetOrbits; //планеты не пригодные к колонизации
            SortedSet<byte> FreePlanetOrbits; //свободные планеты, пригодные к колонизации
            public SortedList<byte, ServerMission2> Missions;
            public byte[] Array;
            public Mission2Inspector (ServerStar star)
            {
                Star = star;
                BadPlanetOrbits = new SortedSet<byte>();
                FreePlanetOrbits = new SortedSet<byte>();
                foreach (int planetid in star.PlanetsID)
                {
                    ServerPlanet planet = ServerLinks.GSPlanets[planetid];
                    if (planet.MaxPopulation==0) BadPlanetOrbits.Add((byte)planet.Orbit);
                    if (planet.MaxPopulation>0 && planet.Lands.Count==0 && planet.NewLands.Count==0 && planet.QuestPlanet==false)
                        FreePlanetOrbits.Add((byte)planet.Orbit);
                }
                Missions = new SortedList<byte, ServerMission2>();
                CreateArrayForPlayer();
            }
            /// <summary> метод проверяет наличие миссии, снимает её с очереди и возвращает результат</summary>
            public ServerMission2Result StartMission(byte orbit, ServerFleet fleet)
            {
                ServerMission2 mission = Missions[orbit];
                Missions.Remove(orbit);
                CreateArrayForPlayer();
                fleet.Target = new ServerFleetTarget(mission, fleet, FleetMission.Mission);
                switch (mission.Content)
                {
                    case Mission2Content.Empty: return new ServerMission2Result();
                    case Mission2Content.FreeLow: return new ServerMission2Result(mission.CalcReward(fleet.Target.SelfPlayer, false), false);
                    case Mission2Content.FreeMedium: return new ServerMission2Result(mission.CalcReward(fleet.Target.SelfPlayer, false), true);
                    case Mission2Content.BattleMedium: ServerBattle battlemid = new ServerBattle(fleet, mission); return new ServerMission2Result(battlemid, true);
                    case Mission2Content.BattleHigh: ServerBattle battlehigh = new ServerBattle(fleet, mission); return new ServerMission2Result(battlehigh, false);

                    case Mission2Content.BlackMarket: return ServerMission2Result.GetBlackMarketResult();
                }
                return new ServerMission2Result();
            }
            public void AddMissions()
            {
                List<byte> BadMissions = new List<byte>();
                foreach (KeyValuePair<byte, ServerMission2> pair in Missions)
                {
                    if (pair.Value.EndTime <= ServerLinks.NowTime)
                        BadMissions.Add(pair.Key);
                }
                if (BadMissions.Count > 0)
                    foreach (byte b in BadMissions)
                        Missions.Remove(b);
                if (Star.FreeOrbits.Count!=0)
                {
                    foreach (byte b in Star.FreeOrbits)
                        if (Missions.ContainsKey(b))
                            continue;
                        else
                        {
                            int val = ServerLinks.BattleRandom.Next(14);
                            //int val = 13;
                            if (val == 0)
                                Missions.Add(b, new ServerMission2(Mission2Type.Metheorit, Star, b));
                            else if (val == 1)
                                Missions.Add(b, new ServerMission2(Mission2Type.Anomaly, Star, b));
                            else if (val == 2)
                                Missions.Add(b, new ServerMission2(Mission2Type.ShipWreck, Star, b));
                            else if (val == 3)
                                Missions.Add(b, new ServerMission2(Mission2Type.BlackMarket, Star, b));
                            else if (val == 4)
                                Missions.Add(b, new ServerMission2(Mission2Type.PillageProtect, Star, b));
                            else if (val == 5)
                                Missions.Add(b, new ServerMission2(Mission2Type.CrimePursuit, Star, b));
                            else if (val == 6)
                                Missions.Add(b, new ServerMission2(Mission2Type.FieldDay, Star, b));
                            else if (val == 7)
                                Missions.Add(b, new ServerMission2(Mission2Type.Experiment, Star, b));
                            else if (val == 8)
                                Missions.Add(b, new ServerMission2(Mission2Type.ConvoyDefence, Star, b));
                            else if (val == 9)
                                Missions.Add(b, new ServerMission2(Mission2Type.ConvoyAttack, Star, b));
                            else if (val == 10)
                                Missions.Add(b, new ServerMission2(Mission2Type.SmallCompetition, Star, b));
                            else if (val == 11)
                                Missions.Add(b, new ServerMission2(Mission2Type.LargeCompetition, Star, b));
                            else if (val == 12)
                                Missions.Add(b, new ServerMission2(Mission2Type.RangeRaid, Star, b));
                            else if (val==13 && FreePlanetOrbits.Count>0)
                            {
                                byte p = FreePlanetOrbits.ElementAt(ServerLinks.BattleRandom.Next(FreePlanetOrbits.Count));
                                if (Missions.ContainsKey(p)) continue;
                                Missions.Add(p, new ServerMission2(Mission2Type.PirateBase, Star, p));
                            }
                        }
                }
                if (Star.OreBelt < 10 && Missions.ContainsKey(Star.OreBelt) == false)
                {
                    Missions.Add(Star.OreBelt, new ServerMission2(Mission2Type.OreBelt, Star, Star.OreBelt));
                }
                CreateArrayForPlayer();
            }
            /// <summary> метод проверяет миссии, убирает несоответствующие </summary>
            public void Check()
            {
                ///Здесь должен быть код, убирающий миссии при смене типа планеты
                List<byte> BadMissions = new List<byte>();
                FreePlanetOrbits = new SortedSet<byte>();
                foreach (ServerPlanet planet in Star.Planets.Values)
                {
                    if (planet.MaxPopulation>0 && planet.Lands.Count==0 && planet.NewLands.Count==0 && planet.QuestPlanet==false)
                        FreePlanetOrbits.Add((byte)planet.Orbit);
                }
                foreach (KeyValuePair<byte, ServerMission2> pair in Missions)
                {
                    if (pair.Value.Location == Mission2Location.FreePlanet && FreePlanetOrbits.Contains(pair.Key)==false)
                        BadMissions.Add(pair.Key);
                }
                if (BadMissions.Count != 0)
                {
                    foreach (byte b in BadMissions)
                        Missions.Remove(b);
                    CreateArrayForPlayer();
                }
            }
            void CreateArrayForPlayer()
            {
                List<byte> list = new List<byte>();
                list.AddRange(BitConverter.GetBytes(Star.ID));
                list.Add((byte)Missions.Count);
                foreach (KeyValuePair<byte, ServerMission2> pair in Missions)
                {
                    list.Add(pair.Key);
                    list.Add((byte)pair.Value.Type);
                    list.Add((byte)(pair.Value.EndTime - pair.Value.CreationTime).TotalDays);
                }
                Array = list.ToArray();
            }
        }
       
        public static void FillOreBelts()
        {
            Random rnd = new Random();
            foreach (ServerStar star in ServerLinks.GSStars.Values)
            {
                star.FreeOrbits = new SortedSet<byte>(new byte[] { 1, 2, 3, 4, 5, 6, 7 });
                foreach (int planetid in star.PlanetsID)
                {
                    ServerPlanet planet = ServerLinks.GSPlanets[planetid];
                    if (star.FreeOrbits.Contains((byte)planet.Orbit)) star.FreeOrbits.Remove((byte)planet.Orbit);
                }

                
                if (star.OreBelt==20)
                {
                    int hasorbit = rnd.Next(4);
                    if (hasorbit > 0) star.OreBelt = 255;
                    else
                    {
                        if (star.FreeOrbits.Count == 0) star.OreBelt = 255;
                        else
                        {
                            star.OreBelt = star.FreeOrbits.ElementAt(rnd.Next(star.FreeOrbits.Count));

                        }
                    }
                }
                star.FreeOrbits.Remove(star.OreBelt);
                star.MissionInspector = new Mission2Inspector(star);
            }
            
        }
        /// <summary> Метод проверяет патруль звезды. Если патруль есть - то начинается бой. Если патруля нет и флот направлен в патруль - то флот становится в патруль. </summary>
        public ServerBattle StartPatrolBattle(ServerFleet fleet)
        {
            if (ScoutedFleets.Count == 0 || ScoutedFleets[0].Target.SelfPlayer==fleet.Target.SelfPlayer)
            {
                if (fleet.Target.Mission==FleetMission.Scout)
                    ScoutedFleets.Add(fleet);
                return null;
            }
            else
            {
                ServerFleet protector = ScoutedFleets[0];
                ScoutedFleets.Remove(protector);
                ServerBattle battle = new ServerBattle(fleet, fleet.Target.SelfPlayer.MainPlayer==false, protector, 
                    protector.Target.SelfPlayer.MainPlayer==false, BattleType.Space, new BattleFieldGroup(BattleField.Fields[0], new SortedList<byte, ServerShipB>(), new byte[2]));
                protector.Target.SelfPlayer.CreateMission2Array();
                protector.Target.SelfPlayer.CreatePlanetInfo();
                fleet.Target.SelfPlayer.AddBattle(battle);
                protector.Target.SelfPlayer.AddBattle(battle);
                protector.Target.SelfPlayer.FleetInBattle(protector);
                return battle;
            }
        }
        public static byte[] GetArray(SortedList<int, ServerStar> list)
        {
            List<byte> result = new List<byte>();
            foreach (ServerStar star in list.Values)
            {
                ServerCommon.AddBytesToList(result, new object[]{
                star.ID,
                star.En,
                star.Ru,
                star.X,
                star.Y,
                (byte)star.Class,
                star.SizeModifier,
                star.OreBelt
                });
            }
            return result.ToArray();
        }
       /* public static void CreateEnemyArray()
        {
            List<byte> list = new List<byte>();
            list.Add(9); list.Add(0);
            list.AddRange(BitConverter.GetBytes(ServerLinks.GSStars.Count));
            foreach (ServerStar star in ServerLinks.GSStars.Values)
                list.Add(star.Enemy);
            EnemyArray = list.ToArray();
        }*/
        public double GetDistance(ServerStar star)
        {
            return Math.Sqrt(Math.Pow(star.X - X, 2) + Math.Pow(star.Y - Y, 2));
        }
        public FleetTargetE GetTargetType()
        {
            return FleetTargetE.Star;
        }
        public byte[] GetTargetArray()
        {
            List<byte> list = new List<byte>();
            list.Add((byte)FleetTargetE.Star);
            list.AddRange(BitConverter.GetBytes(ID));
            return list.ToArray();
        }
        
    }


}
