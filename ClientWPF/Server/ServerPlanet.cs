using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public enum PlanetTypes { Green, Burned, Freezed, Gas, Meteor}
    public enum RareBuildings { No, Raskopki}
    public enum PlanetSide { Player, GreenTeam, Techno, Alien, Pirate, Free, No, Mercs, Unknown}
    public class ServerPlanet:FleetTargetI, IComparable
    {
        public int ID;
        public GSString Name;
        public int StarID;
        public ServerStar Star;
        public int Orbit;
        public int Size;
        public int Moons;
        public bool HasBelt;
        public int ImageType;
        public PlanetTypes PlanetType;
        public int LandsCount;
        public int MaxPopulation;
        public int Locations;
        public RareBuildings RareBuilding;
        //public PlanetSide PlanetSide;
        public SortedList<int, ServerLand> Lands;
        public SortedList<int, ServerAvanpost> NewLands;
        public bool QuestPlanet = false;
        public ServerPlanet(int id, GSString name, int starID, int orbit, int size, int moons, bool hasbelt, int imageType, PlanetTypes type, int maxpopulation, int locations, RareBuildings rare)
        {
            ID = id;
            Name = name;
            StarID = starID;
            Orbit = orbit;
            Size = size;
            Moons = moons;
            HasBelt = hasbelt;
            ImageType = imageType;
            LandsCount = 1;
            PlanetType = type;
            MaxPopulation = maxpopulation;
            Locations = locations;
            RareBuilding = rare;
            Star = ServerLinks.GSStars[starID];
            Star.PlanetsID.Add(ID);
            Star.Planets.Add((byte)orbit, this);
            Lands = new SortedList<int, ServerLand>();
            NewLands = new SortedList<int, ServerAvanpost>();
            //if (MaxPopulation > 0)
            //    PlanetSide = PlanetSide.Free;
            //else
            //    PlanetSide = PlanetSide.No;
        }
        public int CompareTo(object b)
        {
            ServerPlanet planet = (ServerPlanet)b;
            if (planet.ID > ID) return 1;
            else if (planet.ID < ID) return -1;
            else return 0;
        }
        public FleetTargetE GetTargetType()
        {
            return FleetTargetE.Planet;
        }
        public TargetLand GetTargetLand()
        {
            if (Lands.Count > 0) return Lands.Values[0];
            else if (NewLands.Count > 0) return NewLands.Values[0];
            else return null;
        }
        public byte[] GetTargetArray()
        {
            List<byte> list = new List<byte>();
            list.Add((byte)FleetTargetE.Planet);
            list.AddRange(BitConverter.GetBytes(ID));
            return list.ToArray();
        }
        public byte[] GetArrayForSave()
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(ID));
            list.AddRange(BitConverter.GetBytes(StarID));
            list.AddRange(Name.Array);
            list.Add((byte)Orbit);
            list.Add((byte)Size);
            list.Add((byte)Moons);
            list.Add((byte)(HasBelt == true ? 1 : 0));
            list.Add((byte)ImageType);
            list.Add((byte)LandsCount);
            list.Add((byte)PlanetType);
            list.AddRange(BitConverter.GetBytes(MaxPopulation));
            list.Add((byte)Locations);
            list.Add((byte)RareBuilding);
            return list.ToArray();
        }
        public static ServerPlanet LoadPlanet(byte[] array, ref int index)
        {
            int id = BitConverter.ToInt32(array, index); index += 4;
            int starID = BitConverter.ToInt32(array, index); index += 4;
            GSString name = new GSString(array, index); index += name.Array.Length;
            int orbit = array[index]; index++;
            int size = array[index]; index++;
            int moons = array[index]; index++;
            bool belt = array[index] == 1; index++;
            int imagetype = array[index]; index++;
            int landscount = array[index]; index++;
            PlanetTypes type = (PlanetTypes)array[index]; index++;
            int populat = BitConverter.ToInt32(array, index); index += 4;
            int locations = array[index]; index++;
            RareBuildings rare = (RareBuildings)array[index]; index++;
            return new ServerPlanet(id, name, starID, orbit, size, moons, belt, imagetype, type, populat, locations, rare);
        }
        public byte[] GetLandsArray()
        {
            List<byte> list = new List<byte>();
            foreach (ServerLand land in Lands.Values)
            {
                list.AddRange(land.GetLandInfoArray());
            }
            foreach (ServerAvanpost land in NewLands.Values)
            {
                list.AddRange(land.GetLandInfoArray());
            }
            return list.ToArray();
        }
        public static bool AddLand(ServerPlanet planet, ServerLand land)
        {
            if (planet.CheckLandsCount())
            {
                planet.Lands.Add(land.ID, land);
                planet.Star.MissionInspector.Check();
                return true;
            }
            else return false;
        }
        public static bool AddNewLand(ServerPlanet planet, ServerAvanpost land)
        {
            lock (planet)
            {
                if (planet.CheckLandsCount())
                {
                    planet.NewLands.Add(land.ID, land);
                    planet.Star.MissionInspector.Check();
                    return true;
                }
                else return false;
            }
        }
        public void RemoveAllLands()
        {
            if (Lands.Count>0)
                foreach (ServerLand land in Lands.Values)
                    land.Destroy();
            Lands = new SortedList<int, ServerLand>();
            if (NewLands.Count > 0)
                foreach (ServerAvanpost avanpost in NewLands.Values)
                    avanpost.Destroy();
            NewLands = new SortedList<int, ServerAvanpost>();
            Star.MissionInspector.Check();
        }
        public bool CheckLandsCount()
        {
            if (LandsCount == 0 || Lands.Count + NewLands.Count< LandsCount) return true; else return false;
        }
       /* public ServerBattle GetColonizeBattle(ServerFleet fleet)
        {
            ServerBattle battle = ServerBattle.CreateColonizeNewPlanetBattle(fleet, this);
            return battle;
        }*/
        public bool CheckColonizeLand(GSPlayer player)
        {
            if (LandsCount <= 0 || Lands.Count + NewLands.Count>= LandsCount) return false;
            if (player.MainPlayer==false)
            {
                foreach (ServerFleet fleet in Star.ScoutedFleets)
                    if (fleet.Target.SelfPlayer != player) return false; else break;
                return true;
            }
            else
            {
                foreach (ServerFleet fleet in Star.ScoutedFleets)
                    if (fleet.Target.SelfPlayer == player) return true; else break;
                foreach (ServerPlanet planet in Star.Planets.Values)
                    if (planet.Lands.Count > 0 && planet.Lands.Values[0].Player == player) return true;
                return false;
            }  
        }
        public static void RemoveLand(ServerPlanet planet, ServerLand land)
        {
            if (planet.Lands.ContainsKey(land.ID)) planet.Lands.Remove(land.ID);
            planet.Star.MissionInspector.Check();
        }
        public static void RemoveLand(ServerPlanet planet, ServerAvanpost land)
        {
            if (planet.NewLands.ContainsKey(land.ID)) planet.NewLands.Remove(land.ID);
            planet.Star.MissionInspector.Check();
        }
    }
}
