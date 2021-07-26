using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class ServerFleet
    {
        public long ID;
        public SortedList<long, ServerShip> Ships;
        public byte[] Image;
        public bool Repair;
        public byte[] Array;
        public ServerFleetTarget Target;
        public ServerBattle CurBattle;
        public FleetCustomParams CustomParams;
        public ServerFleetBase FleetBase;
        public List<Artefact> Artefacts;
        //public int Rating;
        public ServerFleet(long id, byte[] image, ServerFleetBase fleetbase, bool repair, long[] ShipsID, FleetCustomParams customparams)
        {
            ID = id;
            FleetID.CheckFleetID(id);
            Image = image;
            Ships = new SortedList<long, ServerShip>();
            FleetBase = fleetbase;
            if (FleetBase != null)
                FleetBase.SetFleet(this);
            Repair = repair;
            CustomParams = customparams;
            Artefacts = new List<Artefact>();
            if (fleetbase != null)
            {
                GSPlayer player = FleetBase.Land.Player;
                for (int i = 0; i < ShipsID.Length; i++)
                {
                    ServerShip ship = player.Ships[ShipsID[i]];
                    Ships.Add(ship.ID, ship);
                    ship.Fleet = this;
                }
                Array = GetArray();
                fleetbase.Land.AddFleet(this);
            }
            //else
            //    throw new Exception();
        }
        public bool RemoveArtefact(Artefact artefact)
        {
            if (Target != null) return false;
            if (Artefacts.Contains(artefact) == false) return false;
            Artefacts.Remove(artefact);
            Array = GetArray();
            return true;
        }
        public bool AddArtefact(Artefact artefact)
        {
            if (Target != null) return false;
            if (Artefacts.Count >= 3) return false;
            Artefacts.Add(artefact);
            Array = GetArray();
            return true;
        }
        public byte[] GetArrayForBase()
        {
            throw new Exception();
            /*
            List<byte> array = new List<byte>();
            array.AddRange(BitConverter.GetBytes(ID));
            array.AddRange(Image);
            array.Add(Behavior);
            array.AddRange(BitConverter.GetBytes(Repair));
            array.Add(Speed);
            if (Mission == FleetMission.DefenseLand || Mission == FleetMission.DefenseLandRepair)
                array.Add(1);
            else if (Mission == FleetMission.LandDefender || Mission == FleetMission.LandDefenderRepair)
                array.Add(2);
            else
                array.Add(0);
            array.AddRange(BitConverter.GetBytes(Ships.Count));
            foreach (long id in Ships.Keys)
                array.AddRange(BitConverter.GetBytes(id));
            return array.ToArray();*/
        }
        
        public byte[] GetArray()
        {
            List<byte> array = new List<byte>();
            array.AddRange(BitConverter.GetBytes(ID));
            array.AddRange(Image);
            if (FleetBase != null) array.Add((byte)FleetBase.Position); else throw new Exception();
            array.AddRange(BitConverter.GetBytes(Repair));
            array.Add((byte)Artefacts.Count);
            foreach (Artefact art in Artefacts)
                array.AddRange(BitConverter.GetBytes(art.ID));
            array.AddRange(ServerFleetTarget.GetArray(Target));
            array.Add((byte)Ships.Count);
            foreach (long id in Ships.Keys)
                array.AddRange(BitConverter.GetBytes(id));
            return array.ToArray();
        }
        
        //считывание флота с базы данных
        public static ServerFleet GetFleet(byte[] array, ref int i, int landid, GSPlayer player)
        {
            long id = BitConverter.ToInt64(array, i);
            i += 8;
            byte[] image = new byte[4];
            for (int j = 0; j < 4; j++)
                image[j] = array[i + j];
            i += 4;
            byte sectorpos = array[i]; i++;
            bool repair = BitConverter.ToBoolean(array, i); i++;
            i += 8;
            byte mission = array[i]; i++;
            i += 4;
            int shipscount = BitConverter.ToInt32(array, i);
            i += 4;
            List<long> shipsid = new List<long>();
            for (int j = 0; j < shipscount; j++)
            {
                shipsid.Add(BitConverter.ToInt64(array, i));
                i += 8;
            }
            //ServerFleet fleet = new ServerFleet(id, image, landid, behavior, repair, speed, 0, shipsid.ToArray(), FleetMission.Free);
            if (mission == 0) { }
            else if (mission == 1)
            {
                //TODO Сделать замену событиям для локального сервера
                //SendFleetToDefenseInnerEvent sendevent = new SendFleetToDefenseInnerEvent(player, fleet, EEventType.SendFleetToDefenseInner);
                //ServerLinks.Events.CurrentQueue.Enqueue(sendevent);
            }
            else if (mission == 2)
            {
                //TODO Сделать замену событиям для локального сервера
                //SendFleetToDefenseInnerEvent sendevent = new SendFleetToDefenseInnerEvent(player, fleet, EEventType.SendFleetAsLastDefenderInner);
                //ServerLinks.Events.CurrentQueue.Enqueue(sendevent);
            }
            return null;
            //return fleet;
        }
        
        public static ServerFleet GetNewFleet(byte[] image, ServerFleetBase fleetbase)
        {
            long id = FleetID.GetFleetID();
            ServerFleet fleet = new ServerFleet(id, image, fleetbase, false, new long[0], null);
            return fleet;
        }
        public int GetMaxShips()
        {
            return FleetBase.MaxShips;
        }
        public bool IsHavePlace()
        {
            int MaxShips = FleetBase.MaxShips;
            if (MaxShips <= Ships.Count) return false;
            return true;
        }
        /// <summary> Проверка флота на готовность</summary>
        public bool CheckHealth()
        {
            if (Target != null) return false;
            if (Ships.Count == 0) return false;
            if (Ships.Count > FleetBase.MaxShips) return false;
            if (FleetBase.Range < 0) return false;
            if (FleetBase.MaxMath <= 0) return false;
            int shipsum = 0;
            foreach (ServerShip ship in Ships.Values)
                shipsum += ship.Health;
            if (shipsum == 0) return false;
            return true;
        }


        public int GetActiveShips()
        {
            if (Ships.Count == 0) return 0;
            int count = 0;
            foreach (ServerShip ship in Ships.Values)
                if (ship.Health > 0) count++;
            return count;
        }
        public byte GetFleetHealth()
        {
            int totalshipshealth = 0;
            foreach (ServerShip ship in Ships.Values)
            {
                totalshipshealth += ship.Health;
            }
            if (Ships.Count  == 0) return 0;
            return (byte)(totalshipshealth / Ships.Count);
        }
        public bool CheckFleetReadyForDefense()
        {
            byte fleethealth = GetFleetHealth();
            if (fleethealth == 0) return false;
            //if (Behavior == 0) return true;
            //if (fleethealth >= Behavior) return true;
            else return false;
        }
        public int GetFleetRating()
        {
            int result = 0;
            result += (int)(FleetBase.MaxMath * RatingModifiers.PortalHealth);
            foreach (ServerShip ship in Ships.Values)
                result += ship.CalcRating();
            return result;
        }
        public void CalcFleetReturnTime(bool isWin)
        {
            if (Target.Mission == FleetMission.Scout && Target.Order == FleetOrder.Defense)
                Target.FreeTime = ServerLinks.NowTime + TimeSpan.FromDays(5);
            else if (isWin == false)
                Target.FreeTime = ServerLinks.NowTime + TimeSpan.FromDays(10);
            else if (Target.Mission == FleetMission.Pillage|| Target.Mission == FleetMission.Conquer)
                Target.FreeTime = ServerLinks.NowTime + TimeSpan.FromDays(5);
            else
                Target.FreeTime = ServerLinks.NowTime + TimeSpan.FromDays(1);
        }
        /// <summary>
        ///Метод уничтожает флот. Если флот в тот момент находится в бою или возвращается - то он будет уничтожен по возвращении из боя или при передаче хода.
        ///Из Флотов колонии он убирается после вызова этого метода
        /// </summary>
        public void FleetDestroy()
        {
            if (Ships != null)
                foreach (ServerShip ship in Ships.Values)
                    ship.Fleet = null;
            if (FleetBase!= null) FleetBase = null;
            Ships = null;
            if (Target != null)
            {
                if (Target.Destroy == true)
                    Target = null;
                else if (Target.Order == FleetOrder.InBattle)
                    Target.Destroy = true;
                else if (Target.Order == FleetOrder.Return)
                    Target.Destroy = true;
                else if (Target.Order == FleetOrder.Defense)
                {
                    if (Target.Mission == FleetMission.Scout)
                    {
                        Target.GetStar().ScoutedFleets.Remove(this);
                        Target.SelfPlayer.CreateMission2Array();
                        Target.SelfPlayer.CreatePlanetInfo();
                    }
                    else if (Target.Mission == FleetMission.Pillage)
                    {
                        ServerPlanet planet = Target.GetPlanet();
                        TargetLand targetland = planet.GetTargetLand();
                        if (targetland != null) targetland.SetPillageFleet(null);
                    }
                    else if (Target.Mission == FleetMission.Conquer)
                    {
                        ServerPlanet planet = Target.GetPlanet();
                        TargetLand targetland = planet.GetTargetLand();
                        if (targetland != null) targetland.SetConquerFleet(null);
                    }
                    Target = null;
                }
                else Target = null;
            }
        }
    }
    class FleetID
    {
        public long ID = -1;
        static FleetID FleetIDobject = new FleetID();
        public static long GetFleetID()
        {
            lock (FleetIDobject)
            {
                FleetIDobject.ID++;
                return FleetIDobject.ID;
            }
        }
        public static void CheckFleetID(long id)
        {
            if (id == -1) return;
            if (FleetIDobject.ID < id) FleetIDobject.ID = id;
        }
    }
    public class FleetCustomParams
    {
        public int Health;
        public GSPlayer Player;
        public int CustomID;
        public byte CustomLevel;
        public FleetCustomParams(int health)
        {
            Health = health; Player = null; CustomID = 0; CustomLevel = 0;
        }
        public FleetCustomParams(int health, GSPlayer player, int customid, byte customlevel)
        {
             Health = health; Player = player; CustomID = customid; CustomLevel = customlevel;
        }
    }
    public enum FleetTargetE { Star, Planet, Land, Avanpost, Mission, Story}
    public interface FleetTargetI
    {
        FleetTargetE GetTargetType();
        byte[] GetTargetArray();
    }
    public class ServerFleetTarget
    {
        public FleetTargetI Target;
        public GSPlayer TargetPlayer;
        public GSPlayer SelfPlayer;
        public int Health;
        public int Range;
        public FleetMission Mission;
        public FleetOrder Order;
        public DateTime FreeTime;
        public bool Destroy = false; //поле указывает, что этот флот должен быть уничтожен при первой возможности (окончание боя)
        public ServerFleetTarget(FleetTargetI target, ServerFleet fleet, FleetMission mission)
        {
            Target = target;
            if (fleet.CustomParams!= null)
            {
                Health = fleet.CustomParams.Health;
                Range = 0;
                SelfPlayer = fleet.CustomParams.Player;
            }
            else
            {
                Health = fleet.FleetBase.MaxMath;
                Range = fleet.FleetBase.Range;
                SelfPlayer = fleet.FleetBase.Land.Player;
            }
            Mission = mission;
            Order = FleetOrder.Start;
            FreeTime = DateTime.MinValue;
            FleetTargetE type = Target.GetTargetType();
            if (type == FleetTargetE.Land)
                TargetPlayer = ((ServerLand)target).Player;
            else if (type == FleetTargetE.Avanpost)
                TargetPlayer = ((ServerAvanpost)target).Player;
            else
                TargetPlayer = null;
        }
        public ServerStar GetStar()
        {
            FleetTargetE type = Target.GetTargetType();
            switch (type)
            {
                case FleetTargetE.Star: return (ServerStar)Target;
                case FleetTargetE.Planet: ServerPlanet planet = (ServerPlanet)Target; return planet.Star;
                case FleetTargetE.Land: ServerLand land = (ServerLand)Target; return land.Planet.Star;
                case FleetTargetE.Avanpost: ServerAvanpost newland = (ServerAvanpost)Target; return newland.Planet.Star;
                case FleetTargetE.Mission: Mission2 mission = (Mission2)Target; return ServerLinks.GSStars[mission.StarID];
                case FleetTargetE.Story: StoryLine2 story = (StoryLine2)Target; if (story.StoryType == StoryType.SpaceBattle) return ServerLinks.GSStars[story.LocationID]; else return ServerLinks.GSPlanets[story.LocationID].Star;
                default: return null;
            }
        }
        public ServerPlanet GetPlanet()
        {
            FleetTargetE type = Target.GetTargetType();
            switch (type)
            {
                case FleetTargetE.Planet: return (ServerPlanet)Target;
                case FleetTargetE.Land: ServerLand land = (ServerLand)Target; return land.Planet;
                case FleetTargetE.Avanpost: ServerAvanpost newland = (ServerAvanpost)Target; return newland.Planet;
                default: return null;
            }
        }
        public TargetLand GetLand()
        {
            FleetTargetE type = Target.GetTargetType();
            switch (type)
            {
                case FleetTargetE.Land: return (ServerLand)Target;
                case FleetTargetE.Avanpost: return (ServerAvanpost)Target;
                default: return null;
            }
        }
        public static byte[] GetArray(ServerFleetTarget target)
        {
            if (target == null) return new byte[] { 0 };
            List<byte> list = new List<byte>();
            list.Add(1);
            list.Add((byte)target.Mission);
            list.Add((byte)target.Order);
            list.AddRange(BitConverter.GetBytes(target.FreeTime.Ticks));
            list.AddRange(target.Target.GetTargetArray());
            return list.ToArray();
        }


    }
}
