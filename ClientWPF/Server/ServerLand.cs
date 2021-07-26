using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public enum LandType { Basic, New}
    public class ServerFleetBase:ServerPeaceSector
    {
        public int MaxShips = 3;
        int BasicMath = 300;
        public int MaxMath
        {
            get
            {
                if (Fleet!= null)
                    return BasicMath * (100 - (Fleet.Ships.Count - 1) * 5) / 100;
                else return BasicMath;
            }
            set
            {
                BasicMath = value;
            }
        }
        int BasicRange = 10;
        public int Range
        {
            get
            {
                if (Fleet!= null)
                    return BasicRange - Fleet.Ships.Count + 1;
                else return BasicRange;
            }
            set
            {
                BasicRange = value;
            }
        }
        public ServerFleet Fleet { get; private set; }
        public ServerFleetBase(ServerLand land, int position) : base(land, SectorTypes.War, position)
        {
            Fleet = null;
            CreateArray();
        }
        public override string ToString()
        {
            if (Fleet == null)
                return "Fleet Base Empty";
            else
                return "Fleet Base ID=" + Fleet.ID.ToString() + " " + (Fleet.Target == null ? "Free" : Fleet.Target.Mission.ToString());
        }
        public void SetFleet(ServerFleet fleet)
        {
            Fleet = fleet;
        }
        public override LandSectorType GetSectorType()
        {
            return LandSectorType.War;
        }
        
    }
    public enum LandSectorType { Clear, Peace, War}
    public class ServerLandSector
    {
        public ServerLand Land;
        public SectorTypes Type;
        public byte[] Array;
        public int Position;
        public ServerLandSector(ServerLand land, int position)
        {
            Land = land;
            Type = SectorTypes.Clear;
            Position = position;
            if (Land.Player.MainPlayer == true) CreateArray();
        }
        public virtual void CreateArray()
        {
            List<byte> list = new List<byte>();
            list.Add((byte)Type);
            Array = list.ToArray();
        }
        public virtual LandSectorType GetSectorType()
        {
            return LandSectorType.Clear;
        }
        public void BuildBuilding(byte pos)
        {
            ServerPeaceSector sector = (ServerPeaceSector)this;
            sector.Buildings[pos].CurLevel++;
            if (Land.Player.MainPlayer == true) sector.CreateArray();
        }
    }
    public class ServerPeaceSector:ServerLandSector
    {
        public List<GSBuilding2> Buildings;
        public ServerPeaceSector(ServerLand land, SectorTypes type, int position) : base(land, position)
        {
            Type = type;
            Buildings = new List<GSBuilding2>();
            Buildings.Add(new GSBuilding2(land.ID, (byte)position, type, 0));
            Buildings.Add(new GSBuilding2(land.ID, (byte)position, type, 1));
            Buildings.Add(new GSBuilding2(land.ID, (byte)position, type, 2));
            CreateArray();
        }
        public override string ToString()
        {
            string answer = "";
            switch (Type)
            {
                case SectorTypes.Live: answer+= "Live " ; break;
                case SectorTypes.Money: answer += "Money "; break;
                case SectorTypes.Metall: answer += "Metall "; break;
                case SectorTypes.MetalCap: answer += "MetallCap "; break;
                case SectorTypes.Chips: answer += "Chip "; break;
                case SectorTypes.ChipsCap: answer += "ChipCap "; break;
                case SectorTypes.Anti: answer += "Anti "; break;
                case SectorTypes.AntiCap: answer += "AntiCap "; break;
                case SectorTypes.Repair: answer += "Repair "; break;
                default: return "Error";
            }
            answer += (Buildings[0].GetTotalSize() + Buildings[1].GetTotalSize() + Buildings[2].GetTotalSize()).ToString();
            answer += " B1=" + Buildings[0].GetCurValue().ToString() + " B2=" + Buildings[1].GetCurValue().ToString() + " B3=" + Buildings[2].GetCurValue().ToString();
            return answer;
        }
        public override void CreateArray()
        {
            List<byte> list = new List<byte>();
            list.Add((byte)Type);
            if (Buildings != null)
            {
                list.Add(Buildings[0].CurLevel);
                list.Add(Buildings[1].CurLevel);
                list.Add(Buildings[2].CurLevel);
            }
            Array = list.ToArray();
        }
        public override LandSectorType GetSectorType()
        {
            return LandSectorType.Peace;
        }
    }
    public interface TargetLand
    {
        GSPlayer GetPlayer();
        byte[] GetLandInfoArray();
        int GetID();
        FleetTargetI GetFleetTarget();
        int GetLandMaxSize();
        void ConquerLand();
        ServerPlanet GetPlanet();
        LandType GetLandType();
        ServerFleet GetPillageFleet();
        void SetPillageFleet(ServerFleet fleet);
        BuildingValues GetPillagedResources();
        ServerFleet GetConquerFleet();
        void SetConquerFleet(ServerFleet fleet);
    }
    public class ServerAvanpost:FleetTargetI, TargetLand
    {
        public enum BuildResult { NoMoney, ErrorPrice, Ok, Finished }
        public int ID; //ID территории
        public GSString Name; //Название территории
        public int PlanetID; //ID планеты, на которой расположена территория
        public int PlayerID; //ID владельца территории
        //public int LandLevel; //уровень территории, считывается с уровня планеты
        //public int LandSize; //размер территории, считывается с параметров планеты
        public ServerPlanet Planet; //ссылка на планету, на которой расположена территория
        public GSPlayer Player; //ссылка на игрока, которому принадлежит территория
        public LandAdd NeedResources;
        public LandAdd HaveResources;
        public ServerFleet PillageFleet;
        public ServerFleet ConquerFleet;
        public byte RiotIndicator = 0;
        public ServerAvanpost(int id, int planetid, int playerid, GSString name, LandAdd need, LandAdd have)
        {
            ID = id;
            PlanetID = planetid;
            Planet = ServerLinks.GSPlanets[planetid];
            PlayerID = playerid;
            Player = ServerLinks.GSPlayersByID[playerid];
            Name = name;
            //LandLevel = Planet.LandLevel;
            //LandSize = Planet.LandSize;
            if (need == null)
            {
                NeedResources = new LandAdd();
                NeedResources.Money = CalcValue2(200, 2, 28);
                NeedResources.Metall = CalcValue2(200, 1.6, 15);
                NeedResources.Chips = CalcValue2(500, 1.2, 9);
                NeedResources.Anti = CalcValue2(350, 1.4, 11);
                //NeedResources.Metall = CalcValue(0.00003, -2000.20002, 0.0002451, 0.9960784);
                //NeedResources.Chips = CalcValue(0.00001, -700.070007, 0.0022059, 0.9647059);
                //NeedResources.Anti = CalcValue(0.000001, 0, 0.0242647, 0.6117647);
                //NeedResources.Money = CalcValue(0.0001, 0, 0.0002451, 0.9960784);
            }
            else NeedResources = need;
            if (have == null) HaveResources = new LandAdd();
            else HaveResources = have;
            ServerLinks.Avanposts.Add(ID, this);
        }
        int CalcValue2(double k1, double k2, double k3)
        {
            double A1 = Math.Pow(k1 + Planet.MaxPopulation, k2 + Planet.Locations / k3);
            return ((int)(A1/1000))*1000;
        }
        int CalcValue(double k1, double b1, double k2, double b2)
        {
            double A1 = (k1 * Math.Pow(Planet.MaxPopulation, 4) + b1);
            double A2 = (k2 * Math.Pow(Planet.Locations, 4) + b2);
            double A3 = A1 * A2;
            double log = (int)Math.Log10(A3)-2;
            double div = Math.Pow(10, log);
            A3 = (int)(A3 / div);
            return (int)(A3 * div);
        }
        public GSPlayer GetPlayer()
        {
            return Player;
        }

        public void ConquerLand()
        {
            if (ConquerFleet == null || ConquerFleet.Target.SelfPlayer == Player) return;
            Player.NewLands.Remove(ID);
            Player.GetFleetsArray();
            Player.CreateNewLandsArray();
            Player.CreateArrayForPlayer();
            ConquerFleet.Target.Order = FleetOrder.Return;
            ConquerFleet.CalcFleetReturnTime(true);
            BattleEvents.FleetFly(ConquerFleet);
            ConquerFleet.Target.SelfPlayer.FleetInBattle(ConquerFleet);
            GSPlayer newowner = ConquerFleet.Target.SelfPlayer;
            ConquerFleet = null;
            newowner.NewLands.Add(ID, this);
            RiotIndicator = 0;
            Player = newowner;

            newowner.GetFleetsArray();
            newowner.CreateNewLandsArray();
            newowner.CreateMission2Array();
            newowner.CreateArrayForPlayer();
        }

        public int GetID()
        {
            return ID;
        }
        public int GetLandMaxSize()
        {
            return Planet.MaxPopulation;
        }
        public ServerPlanet GetPlanet()
        {
            return Planet;
        }
        public LandType GetLandType()
        {
            return LandType.New;
        }
        public ServerFleet GetPillageFleet()
        {
            return PillageFleet;
        }
        public void SetPillageFleet(ServerFleet fleet)
        {
            PillageFleet = fleet;
            Player.CreateNewLandsArray();
            ServerLinks.MainPlayer.CreatePlanetInfo();
        }
        public BuildingValues GetPillagedResources()
        {
            BuildingValues val = new BuildingValues(NeedResources.Money / 100, NeedResources.Metall / 100, NeedResources.Chips / 100, NeedResources.Anti / 100);
            if (HaveResources.Money < val.Money) { val.Money = HaveResources.Money; HaveResources.Money = 0; } else HaveResources.Money -= val.Money;
            if (HaveResources.Metall < val.Metall) { val.Metall = HaveResources.Metall; HaveResources.Metall = 0; } else HaveResources.Metall -= val.Metall;
            if (HaveResources.Chips < val.Chips) { val.Chips = HaveResources.Chips; HaveResources.Chips = 0; } else HaveResources.Chips -= val.Chips;
            if (HaveResources.Anti < val.Anti) { val.Anti = HaveResources.Anti; HaveResources.Anti = 0; } else HaveResources.Anti -= val.Anti;
            return val;
        }
        public ServerFleet GetConquerFleet()
        {
            return ConquerFleet;
        }
        public void SetConquerFleet(ServerFleet fleet)
        {
            ConquerFleet = fleet;
            Player.CreateNewLandsArray();
            ServerLinks.MainPlayer.CreatePlanetInfo();
        }
        public void Destroy()
        {
            Player.NewLands.Remove(ID);
            ServerLinks.Avanposts.Remove(ID);
            Player.CreateNewLandsArray();
            Player.CreateArrayForPlayer();
        }
        public FleetTargetI GetFleetTarget()
        {
            return this;
        }
        public static ServerAvanpost GetLand(byte[] array, ref int s)
        {
            int id = BitConverter.ToInt32(array, s);
            int planetid = BitConverter.ToInt32(array, s + 4);
            int playerid = BitConverter.ToInt32(array, s + 8);
            s += 12;
            GSString name = new GSString(array, s);
            s += name.Array.Length;
            LandAdd need = new LandAdd();
            need.Money = BitConverter.ToInt32(array, s); s += 4;
            need.Metall = BitConverter.ToInt32(array, s); s += 4;
            need.Chips = BitConverter.ToInt32(array, s); s += 4;
            need.Anti = BitConverter.ToInt32(array, s); s += 4;
            LandAdd have = new LandAdd();
            have.Money = BitConverter.ToInt32(array, s); s += 4;
            have.Metall = BitConverter.ToInt32(array, s); s += 4;
            have.Chips = BitConverter.ToInt32(array, s); s += 4;
            have.Anti = BitConverter.ToInt32(array, s); s += 4;


            ServerAvanpost land = new ServerAvanpost(id, planetid, playerid, name, need, have);
            if (land.SetParents(true))
                return land;
            else
                return null;
        }
        //метод проверяет возможность присвоить землю планете и игрогу параметр force убирает колонию на планете если она там ранее была
        public bool SetParents(bool frombase)
        {

            if (Planet.CheckLandsCount() && (frombase || Player.CheckLandsCount()))
                if (ServerPlanet.AddNewLand(Planet, this) && GSPlayer.AddNewLand(Player, this))
                    return true;
                else
                {
                    ServerPlanet.RemoveLand(Planet, this);
                    GSPlayer.RemoveLand(Player, this);
                    return false;
                }
            else return false;
        }
        public byte[] GetArray()
        {
            List<byte> array = new List<byte>();
            array.AddRange(BitConverter.GetBytes(ID));
            array.AddRange(BitConverter.GetBytes(PlanetID));
            array.AddRange(BitConverter.GetBytes(PlayerID));
            array.AddRange(Name.Array);
            array.AddRange(BitConverter.GetBytes(NeedResources.Money));
            array.AddRange(BitConverter.GetBytes(NeedResources.Metall));
            array.AddRange(BitConverter.GetBytes(NeedResources.Chips));
            array.AddRange(BitConverter.GetBytes(NeedResources.Anti));
            array.AddRange(BitConverter.GetBytes(HaveResources.Money));
            array.AddRange(BitConverter.GetBytes(HaveResources.Metall));
            array.AddRange(BitConverter.GetBytes(HaveResources.Chips));
            array.AddRange(BitConverter.GetBytes(HaveResources.Anti));
            array.Add((byte)(PillageFleet == null ? 0 : 1));
            //array.Add(1);
            //array.Add(1);
            array.Add((byte)(ConquerFleet == null ? 0 : 1));
            array.Add(RiotIndicator);
            //array.AddRange(BitConverter.GetBytes(DefenderFleet.Land.ID));
            //array.AddRange(BitConverter.GetBytes(DefenderFleet.ID));
            return array.ToArray();
        }
        public byte[] GetPlanetInfoArray()
        {
            byte[] result = new byte[5];
            switch (Player.ID)
            {
                case 0: result[0] = (byte)PlanetSide.Player; break;
                case 1: result[0] = (byte)PlanetSide.GreenTeam; break;
                case 2: result[0] = (byte)PlanetSide.Techno; break;
                case 3: result[0] = (byte)PlanetSide.Alien; break;
                case 4: result[0] = (byte)PlanetSide.Mercs; break;
                case 5: result[0] = (byte)PlanetSide.Pirate; break;
            }
            result[1] = 1;
            if (PillageFleet != null)
                result[2] = 1;
            if (ConquerFleet != null)
                result[3] = 1;
            result[4] = RiotIndicator;
            return result;
        }
        public static ServerAvanpost GetAvanpost(int planetID, int playerID)
        {
            int id;
            lock (ServerLinks.LandsID)
                id = ServerLinks.LandsID.GetID();
            GSString landName = new GSString("Аванпост "+"\""+ServerLinks.GSPlanets[planetID].Name + "\"");
            ServerAvanpost land = new ServerAvanpost(id, planetID, playerID, landName, null, null);
            if (!land.SetParents(false)) return null;
            return land;
        }
        public FleetTargetE GetTargetType()
        {
            return FleetTargetE.Avanpost;
        }
        /// <summary> массив с данными о новой земле для цели флота </summary>
        public byte[] GetTargetArray()
        {
            List<byte> list = new List<byte>();
            list.Add((byte)FleetTargetE.Avanpost);
            list.AddRange(BitConverter.GetBytes(ID));
            return list.ToArray();

        }
        public byte[] GetLandInfoArray()
        {
            List<byte> list = new List<byte>();
            list.Add(3);
            list.AddRange(BitConverter.GetBytes(ID));
            list.AddRange(Name.Array);
            list.AddRange(Player.Name.Array);
            if (Player.Clan != null) list.AddRange(Player.Clan.BasicArray);
            else list.AddRange(ServerClan.NullBasicArray);
            return list.ToArray();
        }
        public BuildResult Build(ItemPrice resources)
        {
            if (NeedResources.Money - HaveResources.Money < resources.Money) return BuildResult.ErrorPrice;
            if (NeedResources.Metall - HaveResources.Metall < resources.Metall) return BuildResult.ErrorPrice;
            if (NeedResources.Chips - HaveResources.Chips < resources.Chips) return BuildResult.ErrorPrice;
            if (NeedResources.Anti - HaveResources.Anti < resources.Anti) return BuildResult.ErrorPrice;
            if (!Player.Resources.RemovePrice(resources)) return BuildResult.NoMoney;
            HaveResources.Money += resources.Money;
            HaveResources.Metall += resources.Metall;
            HaveResources.Chips += resources.Chips;
            HaveResources.Anti += resources.Anti;
            if (NeedResources.Money > HaveResources.Money) return BuildResult.Ok;
            if (NeedResources.Metall > HaveResources.Metall) return BuildResult.Ok;
            if (NeedResources.Chips > HaveResources.Chips) return BuildResult.Ok;
            if (NeedResources.Anti > HaveResources.Anti) return BuildResult.Ok;
            return BuildResult.Finished;
        }
        public void SetBuild(int moneypercent, int metalpercent, int chipspercent, int antipercent)
        {
            HaveResources.Money = (int)(NeedResources.Money / 100.0 * moneypercent);
            HaveResources.Metall = (int)(NeedResources.Metall / 100.0 * metalpercent);
            HaveResources.Chips = (int)(NeedResources.Chips / 100.0 * chipspercent);
            HaveResources.Anti = (int)(NeedResources.Anti / 100.0 * antipercent);
        }
    }
    public class ServerLand : TargetLand, FleetTargetI
    {
        public int ID; //ID территории
        public GSString Name; //Название территории
        public int PlanetID; //ID планеты, на которой расположена территория
        public int PlayerID; //ID владельца территории
        //public int LandLevel; //уровень территории, считывается с уровня планеты
        //public int LandSize; //размер территории, считывается с параметров планеты
        public ServerPlanet Planet; //ссылка на планету, на которой расположена территория
        public GSPlayer Player; //ссылка на игрока, которому принадлежит территория
        public double Peoples; //население территории, точное
        public int BuildingsCount; //количество построек, расчитывается
        //public Riot Riot; //Индикатор восстания
        //public List<ushort> Buildings; //список построек
        public LandAdd Add;
        public LandMult Mult;
        public LandCapacity Capacity;
        //public int BuildingsMax;
        public SortedList<long, ServerFleet> Fleets;
        public ServerLandSector[] Locations;
        //public byte[] SectorPositions;
        public byte[] FleetsArray;
        public ServerFleet PillageFleet;
        public ServerFleet ConquerFleet;
        public byte RiotIndicator = 0;
        public ServerLand(int id, int planetid, int playerid, GSString name, double peoples, SectorTypes[] locations)
        {
            ID = id;
            PlanetID = planetid;
            Planet = ServerLinks.GSPlanets[planetid];
            PlayerID = playerid;
            Player = ServerLinks.GSPlayersByID[playerid];
            Name = name;
            //LandLevel = Planet.LandLevel;
            //LandSize = Planet.LandSize;
            SetPeoples(peoples);
            Locations = new ServerLandSector[Planet.Locations];
            for (int i = 0; i < locations.Length; i++)
            {
                if (locations[i] == SectorTypes.Clear)
                    Locations[i] = new ServerLandSector(this, i);
                else if ((int)locations[i] <= 9)
                    Locations[i] = new ServerPeaceSector(this, locations[i], i);
            }
            for (int i = locations.Length; i < Planet.Locations; i++)
                Locations[i] = new ServerLandSector(this, i);
            Fleets = new SortedList<long, ServerFleet>();
            Calculate();
            FleetsArray = GetFleetsArray();
        }
        public static ServerLand GetLand(byte[] array, ref int s)
        {
            int id = BitConverter.ToInt32(array, s);
            int planetid = BitConverter.ToInt32(array, s + 4);
            int playerid = BitConverter.ToInt32(array, s + 8);
            s += 12;
            GSString name = new GSString(array, s);
            s += name.Array.Length;

            double _peoples = BitConverter.ToDouble(array, s); s += 8;

            byte locationslength = array[s]; s++;
            SectorTypes[] types = new SectorTypes[locationslength];
            for (int i = 0; i < locationslength; i++)
                types[i] = (SectorTypes)array[i + s];
            s += locationslength;
            //byte[] sectorpositions = ServerCommon.CopyArray(array, s, 12);
            //s += 12;
            if (!ServerLinks.IsLandLoadLastVar)
            {
                byte riotstatus = array[s]; s++;
                if (riotstatus != 0) s += 8;
            }
            ServerLand land = new ServerLand(id, planetid, playerid, name, _peoples, types);
            if (land.SetParents(true, true))
                return land;
            else
                return null;
        }
        public GSPlayer GetPlayer()
        {
            return Player;
        }
        public void ConquerLand()
        {
            if (ConquerFleet == null || ConquerFleet.Target.SelfPlayer == Player) return;
            foreach (ServerFleet fleet in Fleets.Values)
                fleet.FleetDestroy();
            Fleets = new SortedList<long, ServerFleet>();
            Player.Lands.Remove(ID);
            Player.GetFleetsArray();
            Player.CreateCryptLand();
            Player.CreateArrayForPlayer();
            ConquerFleet.Target.Order = FleetOrder.Return;
            ConquerFleet.CalcFleetReturnTime(true);
            BattleEvents.FleetFly(ConquerFleet);
            ConquerFleet.Target.SelfPlayer.FleetInBattle(ConquerFleet);
            GSPlayer newowner = ConquerFleet.Target.SelfPlayer;
            ConquerFleet = null;
            newowner.Lands.Add(ID, this);
            RiotIndicator = 0;
            Player = newowner;
            Calculate();
            foreach (ServerLandSector sector in Locations)
                sector.CreateArray();
            FleetsArray = GetFleetsArray();
            newowner.GetFleetsArray();
            newowner.CreateCryptLand();
            newowner.CreateMission2Array();
            newowner.CreateArrayForPlayer();
        }
        /*public Reward2 ConquerLand(GSPlayer newowner)
        {
            return Player.ConquerLand(this, newowner);
        }*/
        public int GetID()
        {
            return ID;
        }
        public int GetLandMaxSize()
        {
            return Planet.MaxPopulation;
        }
        public ServerPlanet GetPlanet()
        {
            return Planet;
        }
        public LandType GetLandType()
        {
            return LandType.Basic;
        }
        public ServerFleet GetPillageFleet()
        {
            return PillageFleet;
        }
        public void SetPillageFleet(ServerFleet fleet)
        {
            PillageFleet = fleet;
            Player.CreateCryptLand();
            ServerLinks.MainPlayer.CreatePlanetInfo();
        }
        public BuildingValues GetPillagedResources()
        {
            BuildingValues val = new BuildingValues(0, Capacity.Metall/100, Capacity.Chips / 100, Capacity.Anti / 100);
            Player.Resources.RemovePillagedResources(val);
            val.Money += Add.Money; val.Metall += Add.Metall; val.Chips += Add.Chips; val.Anti += Add.Anti;
            return val;
        }
        public ServerFleet GetConquerFleet()
        {
            return ConquerFleet;
        }
        public void SetConquerFleet(ServerFleet fleet)
        {
            ConquerFleet = fleet;
            Player.CreateNewLandsArray();
            ServerLinks.MainPlayer.CreatePlanetInfo();
        }
        public FleetTargetI GetFleetTarget()
        {
            return this;
        }
        public byte[] GetArray()
        {
            List<byte> array = new List<byte>();
            array.AddRange(BitConverter.GetBytes(ID));
            array.AddRange(BitConverter.GetBytes(PlanetID));
            array.AddRange(BitConverter.GetBytes(PlayerID));
            array.AddRange(Name.Array);
            array.AddRange(BitConverter.GetBytes(Peoples));
            array.Add((byte)Locations.Length);
            foreach (ServerLandSector sector in Locations)
                array.AddRange(sector.Array);
            array.Add((byte)(PillageFleet == null ? 0 : 1));
           // array.Add(1);
            array.Add((byte)(ConquerFleet == null ? 0 : 1));
            //array.Add(1);
            array.Add(RiotIndicator);
            //foreach (ushort building in Buildings)
            //    array.AddRange(BitConverter.GetBytes(building));
            //if (Riot == null)
            //array.Add(0);
            //else if (Riot.Status == RiotStatus.PrepareRiot)
            //{ array.Add(1); array.AddRange(BitConverter.GetBytes(Riot.EndTime.Ticks)); }
            //else
            //{ array.Add(2); array.AddRange(BitConverter.GetBytes(Riot.EndTime.Ticks)); }
            return array.ToArray();
        }
        public byte[] GetPlanetInfoArray()
        {
            byte[] result = new byte[5];
            switch (Player.ID)
            {
                case 0: result[0] = (byte)PlanetSide.Player; break;
                case 1: result[0] = (byte)PlanetSide.GreenTeam; break;
                case 2: result[0] = (byte)PlanetSide.Techno; break;
                case 3: result[0] = (byte)PlanetSide.Alien; break;
                case 4: result[0] = (byte)PlanetSide.Mercs; break;
                case 5: result[0] = (byte)PlanetSide.Pirate; break;
            }
            if (PillageFleet != null)
                result[2] = 1;
            if (ConquerFleet != null)
                result[3] = 1;
            result[4] = RiotIndicator;
            return result;
        }
        public byte[] GetFleetsArray()
        {
            List<byte> array = new List<byte>();
            array.AddRange(BitConverter.GetBytes(ID));
            array.AddRange(BitConverter.GetBytes(Fleets.Count));
            foreach (ServerFleet fleet in Fleets.Values)
                array.AddRange(fleet.Array);
            return array.ToArray();

        }
        /*
        public bool CheckShipsCount(int value)
        {
            foreach (ServerFleet fleet in Fleets.Values)
                value += fleet.Ships.Count;
            if (value <= Capacity.Ships) return true; else return false;
        }
        */
        public void AddFleet(ServerFleet fleet)
        {
            Fleets.Add(fleet.ID, fleet);
            FleetsArray = GetFleetsArray();
        }
        public void CreateFleet(byte[] image, byte sectorpos, out ServerFleet fleet)
        {
            ServerLandSector sector = Locations[sectorpos];
            ServerFleetBase fleetbase = (ServerFleetBase)sector;
            fleet = ServerFleet.GetNewFleet(image, fleetbase);
            if (Player.MainPlayer==true) FleetsArray = GetFleetsArray();
            
        }
        public void AddPeoples()
        {
            if (PillageFleet != null) return;
            double add = GetAddParameter(Building2Parameter.PeopleGrow);
            double curadd = add / 30;
            SetPeoples(Math.Round(Peoples+curadd, 3));
            Player.CreateCryptLand();
            //Player.CreateArrayForPlayer();
        }
        public double AddPeoples(int value)
        {
            double peoples = Peoples;
            SetPeoples(Math.Round(Peoples + value, 3));
            Player.CreateCryptLand();
            Player.CreateArrayForPlayer();
            return Peoples - peoples;
        }
        public bool ClearSector(byte pos)
        {
            ServerLandSector sector = Locations[pos];
            if (sector.Type == SectorTypes.Peace || sector.Type == SectorTypes.Live)
                return false;
            if (sector.Type == SectorTypes.War && ((ServerFleetBase)sector).Fleet != null && ((ServerFleetBase)sector).Fleet.Target != null)
                return false;
            if (sector.Type == SectorTypes.War && ((ServerFleetBase)sector).Fleet != null)
            {
                ServerFleet fleet = ((ServerFleetBase)sector).Fleet;
                fleet.FleetDestroy();
                Fleets.Remove(fleet.ID);
                FleetsArray = GetFleetsArray();
                Player.GetFleetsArray();
            }
            Locations[pos] = new ServerLandSector(this, pos);
            Calculate();
            Player.CreateCryptLand();
            Player.CreateArrayForPlayer();
            return true;
        }
        public bool AddSector()
        {
            if (Locations.Length > 7) return false;
            Planet.Locations++;
            List<ServerLandSector> list = new List<ServerLandSector>(Locations);
            list.Add(new ServerLandSector(this, Locations.Length));
            Locations = list.ToArray();
            Player.CreateCryptLand();
            Player.CreateArrayForPlayer();
            return true;

        }
        /*
        //метод расчитывает маскимальное число построек на земле
        public void SetBuildingsCount()
        {
            BuildingsMax = (int)(Peoples * (double)LandLevel);
        }
        */
        //метод строит колонию из колонизируемой
        public static ServerLand FinishAvanpost(ServerAvanpost avanpost)
        {
            double peoples = Math.Round(avanpost.Planet.MaxPopulation * 0.05, 1);
            GSString landName = new GSString(string.Format("Колония \"{0}\"", avanpost.Planet.Name.ToString()));
            ServerLand land = new ServerLand(avanpost.ID, avanpost.PlanetID, avanpost.PlayerID, landName, peoples, new SectorTypes[] {  });
            if (!land.SetParents(false, false)) return null;
            ServerLinks.Avanposts.Remove(avanpost.ID);
            ServerLinks.GSLands.Add(avanpost.ID, land);
            return land;
        }

        public static ServerLand GetNewLand(int planetID, int playerID)
        {
            int id;
            lock (ServerLinks.LandsID)
                id = ServerLinks.LandsID.GetID();
            GSString landName = new GSString(ServerLinks.GSPlanets[planetID].Name + "Land");
            double peoples = Math.Round(ServerLinks.GSPlanets[planetID].MaxPopulation * 0.05, 1);
            ServerLand land = new ServerLand(id, planetID, playerID, landName, peoples, new SectorTypes[] { SectorTypes.Live });
            if (!land.SetParents(false, false)) return null;
            ServerLinks.GSLands.Add(id, land);
            return land;
        }
        /*
        //метод создаём землю на Эдеме, присваивает её земле и игроку, а затем возвращает на неё ссылку
        public static ServerLand GetOrionLand(int playerID)
        {
            int id;
            lock (ServerLinks.LandsID)
                id = ServerLinks.LandsID.GetID();
            GSString landName = new GSString(ServerLinks.GSPlayersByID[playerID].Name + "Edem");
            double peoples = 10; //устанавливает население Эдема
            ServerLand land = new ServerLand(id, ServerLinks.Parameters.PremiumOrionID, playerID, landName, peoples, new SectorTypes[] { SectorTypes.Live });
            if (!land.SetParents(false, true)) return null;
            return land;
        }
        */
        
        //метод создаёт базовую землю и присваивает её земле и игроку, а также возвращает на неё ссылку
        public static ServerLand GetBasicLand(int playerID)
        {
            int id;
            lock (ServerLinks.LandsID)
                id = ServerLinks.LandsID.GetID();
            GSString landName = new GSString("Колония \"Земля\"");
            double peoples = 0.05 * 1000;//устанавливает население базового мира в 50 млн.
            ServerLand land = new ServerLand(id, 2, playerID, landName, peoples, new SectorTypes[] { SectorTypes.Live, SectorTypes.MetalCap, SectorTypes.ChipsCap, SectorTypes.AntiCap });
            if (!land.SetParents(false, true)) return null;
            return land;
        }
        
        //метод создаёт колонию на планете, присваивает её игроку, возвращает на неё ссылку
        public static ServerLand GetLand(int PlanetID, int PlayerID)
        {
            ServerPlanet planet = ServerLinks.GSPlanets[PlanetID];
            if (planet.MaxPopulation <= 0) throw new Exception();
            if (planet.Lands.Count > 0 || planet.NewLands.Count>0) throw new Exception();
            int id;
            lock (ServerLinks.LandsID)
                id = ServerLinks.LandsID.GetID();
            GSString landName = new GSString(string.Format("Колония \"{0}\"", planet.Name.ToString()));
            double peoples = 0.05 * planet.MaxPopulation;
            ServerLand land = new ServerLand(id, planet.ID, PlayerID, landName, peoples, new SectorTypes[0]);
            if (!land.SetParents(false, true)) return null;
            return land;
        }
        
        void SetPeoples(double value)
        {
            Peoples = value;
            if (Peoples>Planet.MaxPopulation) Peoples = Planet.MaxPopulation;
        }
        //метод проверяет возможность присвоить землю планете и игрогу
        public bool SetParents(bool isbasic, bool frombase)
        {
            if (Planet.CheckLandsCount() && (frombase || Player.CheckLandsCount()))
                if (ServerPlanet.AddLand(Planet, this) && GSPlayer.AddLand(Player, this, isbasic, frombase))
                    return true;
                else
                {
                    ServerPlanet.RemoveLand(Planet, this);
                    GSPlayer.RemoveLand(Player, this);
                    return false;
                }
            else return false;
        }
        public void BuildRandomSectors()
        {
            Random rnd = new Random();
            int minpeoples = Planet.MaxPopulation / 10;
            int maxpeoples = Planet.MaxPopulation - minpeoples;
            int Sum = (maxpeoples + 2) * (maxpeoples + 1)/2;
            double D = (maxpeoples + 1.5) * (maxpeoples + 1.5) - 2 * rnd.Next(Sum);
            int curpeoples = (int)((maxpeoples + 1.5) - Math.Sqrt(D));
            SetPeoples(minpeoples + curpeoples);
            
            SortedSet<SectorTypes> Sectors = new SortedSet<SectorTypes>(new SectorTypes[] {SectorTypes.Live, SectorTypes.Money,
            SectorTypes.Repair, SectorTypes.Metall, SectorTypes.MetalCap, SectorTypes.Chips, SectorTypes.ChipsCap, SectorTypes.Anti, SectorTypes.AntiCap, SectorTypes.War});
            foreach (ServerLandSector sector in Locations)
                if (sector.GetSectorType() == LandSectorType.Peace)
                    Sectors.Remove(sector.Type);
            List<ServerLandSector> list = new List<ServerLandSector>(Locations);
            foreach (ServerLandSector sector in list)
            {
                if (sector.GetSectorType() != LandSectorType.Clear) continue;
                int val = rnd.Next(100);
                if (val > 30) continue;
                val = rnd.Next(Sectors.Count);
                BuildSector(Sectors.ElementAt(val), sector.Position);
                if (Locations[sector.Position].Type != SectorTypes.War) Sectors.Remove(Locations[sector.Position].Type);
                ServerPeaceSector cursector = (ServerPeaceSector)Locations[sector.Position];
                for (int i = 0; i < 3; i++)
                {
                    GSBuilding2 building = cursector.Buildings[i];
                    val = (int)((110 - Math.Sqrt(110 * 110 - 20 * rnd.Next(605))) / 10);
                    for (int j = 0; j < val; j++)
                    {
                        if ((Peoples - BuildingsCount) < building.GetNextSize()) break;
                        BuildBuilding((byte)cursector.Position, (byte)i);
                    }
                }
            }
        }
        public void BuildSector(SectorTypes sector, int pos)
        {
            if (sector == SectorTypes.War)
                Locations[pos] = new ServerFleetBase(this, pos);
            else
                Locations[pos] = new ServerPeaceSector(this, sector, pos);
            Calculate();
        }
        public void BuildBuilding(byte sectorid, byte pos)
        {
            ServerLandSector sector = Locations[sectorid];
            sector.BuildBuilding(pos);
            Calculate();
        }
        public double GetAddParameter(Building2Parameter par)
        {
            switch (par)
            {
                case Building2Parameter.PeopleGrow:
                    if (Planet.PlanetType == PlanetTypes.Green)
                        return Math.Round(Planet.MaxPopulation * ServerLinks.Parameters.GreenPlanetGrowBonus + 0.1 + Add.Grow / 100.0 * (100 + Mult.Total + Player.ScienceMult.Get.Total), 1);
                    else
                        return Math.Round(0.1 + Add.Grow / 100.0 * (100 + Mult.Total + Player.ScienceMult.Get.Total), 1);
                case Building2Parameter.MoneyAdd:
                    return Math.Round(Add.Money/100.0 * (100+Mult.Money + Mult.Total + Player.ScienceMult.Get.Money+ Player.ScienceMult.Get.Total) + Peoples * 10/100.0*(100+ Mult.Money+Player.ScienceMult.Get.Money),0);
                case Building2Parameter.MetallAdd:
                    if (Planet.PlanetType == PlanetTypes.Burned)
                        return Math.Round(Add.Metall * ServerLinks.Parameters.BurnPlanetMetallAddBonus / 100.0 * (100 + Mult.Metall + Mult.Total + Player.ScienceMult.Get.Metall + Player.ScienceMult.Get.Total), 0);
                    else
                        return Math.Round(Add.Metall / 100.0 * (100 + Mult.Metall + Mult.Total + Player.ScienceMult.Get.Metall + Player.ScienceMult.Get.Total), 0);
                case Building2Parameter.ChipAdd:
                    return Math.Round(Add.Chips / 100.0 * (100 + Mult.Chips + Mult.Total + Player.ScienceMult.Get.Chips + Player.ScienceMult.Get.Total), 0);
                case Building2Parameter.AntiAdd:
                    if (Planet.PlanetType == PlanetTypes.Gas)
                        return Math.Round(Add.Anti * ServerLinks.Parameters.GasPlanetAntiBonus / 100.0 * (100 + Mult.Anti + Mult.Total + Player.ScienceMult.Get.Anti + Player.ScienceMult.Get.Total), 0);
                    else
                        return Math.Round(Add.Anti / 100.0 * (100 + Mult.Anti + Mult.Total + Player.ScienceMult.Get.Anti + Player.ScienceMult.Get.Total), 0);
                case Building2Parameter.MetalCap:
                    if (Planet.PlanetType == PlanetTypes.Freezed)
                        return Math.Round(Capacity.Metall * ServerLinks.Parameters.FreezedCapBonus / 100.0 * (100 + Mult.Cap + Mult.Total + Player.ScienceMult.Get.Cap + Player.ScienceMult.Get.Total), 0);
                    else
                        return Math.Round(Capacity.Metall / 100.0 * (100 + Mult.Cap + Mult.Total + Player.ScienceMult.Get.Cap + Player.ScienceMult.Get.Total), 0);
                case Building2Parameter.ChipCap:
                    if (Planet.PlanetType == PlanetTypes.Freezed)
                        return Math.Round(Capacity.Chips * ServerLinks.Parameters.FreezedCapBonus / 100.0 * (100 + Mult.Cap + Mult.Total + Player.ScienceMult.Get.Cap + Player.ScienceMult.Get.Total), 0);
                    else
                        return Math.Round(Capacity.Chips / 100.0 * (100 + Mult.Cap + Mult.Total + Player.ScienceMult.Get.Cap + Player.ScienceMult.Get.Total), 0);
                case Building2Parameter.AntiCap:
                    if (Planet.PlanetType == PlanetTypes.Freezed)
                        return Math.Round(Capacity.Anti * ServerLinks.Parameters.FreezedCapBonus / 100.0 * (100 + Mult.Cap + Mult.Total + Player.ScienceMult.Get.Cap + Player.ScienceMult.Get.Total), 0);
                    else
                        return Math.Round(Capacity.Anti / 100.0 * (100 + Mult.Cap + Mult.Total + Player.ScienceMult.Get.Cap + Player.ScienceMult.Get.Total), 0);
                case Building2Parameter.Repair:
                    return Math.Round(Capacity.Repair / 100.0 * (Mult.Repair + Mult.Total + Player.ScienceMult.Get.Repair + Player.ScienceMult.Get.Total), 0);
                default: throw new Exception();
            }
        }
        /// <summary> Калькуляция выполняется только в колонии, при изменении колонии. 
        /// Сервер отдельно сохраняет все бонусы прибавления и умножения
        /// Прирост ресурсов выполняется сложением умножений модификаций планеты и игрока, умножений на модификации планеты и на прибавление. 
        /// </summary>
        public void Calculate()
        {
            BuildingsCount = 0;
            Add = new LandAdd();
            Mult = new LandMult();
            Capacity = new LandCapacity();
            foreach (ServerLandSector sector in Locations)
            {
                if (sector.GetSectorType() != LandSectorType.Peace) continue;
                ServerPeaceSector s = (ServerPeaceSector)sector;
                foreach (GSBuilding2 building in s.Buildings)
                {
                    BuildingsCount += building.GetTotalSize();
                    switch (building.Parameter)
                    {
                        case Building2Parameter.PeopleGrow: Add.Grow += building.GetTotalValue(); break;
                        case Building2Parameter.BuildingEffect: Mult.Total += building.GetTotalValue(); break;
                        case Building2Parameter.MoneyEffect: Mult.Money += building.GetTotalValue(); break;
                        case Building2Parameter.MoneyAdd: Add.Money += (int)building.GetTotalValue(); break;
                        case Building2Parameter.Repair: Capacity.Repair += (int)building.GetTotalValue(); break;
                        case Building2Parameter.RepairEffect: Mult.Repair += building.GetTotalValue(); break;
                        case Building2Parameter.MetallAdd: Add.Metall += (int)building.GetTotalValue(); break;
                        case Building2Parameter.ChipAdd: Add.Chips += (int)building.GetTotalValue(); break;
                        case Building2Parameter.AntiAdd: Add.Anti += (int)building.GetTotalValue(); break;
                        case Building2Parameter.MetallEffect: Mult.Metall += building.GetTotalValue(); break;
                        case Building2Parameter.ChipEffect: Mult.Chips += building.GetTotalValue(); break;
                        case Building2Parameter.AntiEffect: Mult.Anti += building.GetTotalValue(); break;
                        case Building2Parameter.MetalCap: Capacity.Metall += (int)building.GetTotalValue(); break;
                        case Building2Parameter.ChipCap: Capacity.Chips += (int)building.GetTotalValue(); break;
                        case Building2Parameter.AntiCap: Capacity.Anti += (int)building.GetTotalValue(); break;
                        case Building2Parameter.CapEffect: Mult.Cap += building.GetTotalValue(); break;
                    }
                }
            }
            foreach (ServerLandSector sector in Locations)
            {
                if (sector.GetSectorType() != LandSectorType.War) continue;
                ServerFleetBase b = (ServerFleetBase)sector;
                foreach (GSBuilding2 building in b.Buildings)
                {
                    BuildingsCount += building.GetTotalSize();
                    switch (building.Parameter)
                    {
                        case Building2Parameter.Range: b.Range = (int)building.GetTotalValue(); break;
                        case Building2Parameter.Ships: b.MaxShips = (int)building.GetTotalValue(); break;
                        case Building2Parameter.MathPower:b.MaxMath = (int)Math.Round(building.GetTotalValue()/100.0*(100+Mult.Total+Player.ScienceMult.Get.Total),0); break;
                    }
                }
            }
        }
        public FleetTargetE GetTargetType()
        {
            return FleetTargetE.Land;
        }
        /// <summary> массив с данными о земле для цели флота </summary>
        public byte[] GetTargetArray()
        {
            List<byte> list = new List<byte>();
            list.Add((byte)FleetTargetE.Land);
            list.AddRange(BitConverter.GetBytes(ID));
            return list.ToArray();

        }
        /// <summary> массив с данными о земле для карточки </summary>
        public byte[] GetLandInfoArray()
        {
            List<byte> list = new List<byte>();
            list.Add(2);
            list.AddRange(BitConverter.GetBytes(ID));
            list.AddRange(Name.Array);
            list.AddRange(Player.Name.Array);
            if (Player.Clan != null) list.AddRange(Player.Clan.BasicArray);
            else list.AddRange(ServerClan.NullBasicArray);
            return list.ToArray();
        }
        #region ChangeLandName
        static SortedSet<char> BigLetters = GetBigLetters();
        static SortedSet<char> SmallLettersAndDigits = GetSmallLettersAndDigits();
        static SortedSet<char> Others = GetOthers();
        static SortedSet<char> GetBigLetters()
        {
            string let = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            SortedSet<char> set = new SortedSet<char>();
            foreach (char c in let)
                set.Add(c);
            return set;
        }
        static SortedSet<char> GetSmallLettersAndDigits()
        {
            string let = "abcdefghijklmnopqrstuvwxyz0123456789";
            SortedSet<char> set = new SortedSet<char>();
            foreach (char c in let)
                set.Add(c);
            return set;
        }
        static SortedSet<char> GetOthers()
        {
            string let = "- ";
            SortedSet<char> set = new SortedSet<char>();
            foreach (char c in let)
                set.Add(c);
            return set;
        }
        public static bool CheckLandName(GSString name)
        {
            string text = name.ToString();
            if (text.Length < 3 && text.Length > 14) return false;
            if (!BigLetters.Contains(text[0])) return false;
            char last = text[text.Length - 1];
            if (!BigLetters.Contains(last) && !SmallLettersAndDigits.Contains(last)) return false;
            for (int i = 1; i < text.Length - 1; i++)
                if (!BigLetters.Contains(text[i]) && !SmallLettersAndDigits.Contains(text[i]) && !Others.Contains(text[i])) return false;
            if (text.Trim(' ').Length != text.Length) return false;
            if (text.Trim('-').Length != text.Length) return false;
            if (text.Replace("  ", "").Length != text.Length) return false;
            if (text.Replace("--", "").Length != text.Length) return false;
            if (text.Replace("- ", "").Length != text.Length) return false;
            if (text.Replace(" -", "").Length != text.Length) return false;
            return true;
        }

        #endregion
        /// <summary> Метод уничтожает колонию и все её флоты </summary>
        public void Destroy()
        {
            foreach (ServerFleet fleet in Fleets.Values)
            {
                fleet.FleetDestroy();
            }
            Fleets = null;
            Player.GetFleetsArray();
            Player.Lands.Remove(ID);
            ServerLinks.GSLands.Remove(ID);
            Player.CreateCryptLand();
            Player.CreateArrayForPlayer();
        }
    }
    class LandsIDclass
    {
        public int ID = -1;
        public int GetID()
        {
            ID++;
            return ID;
        }
    }
   /* public class Riot
    {
        public DateTime StartTime;
        public DateTime EndTime;
        public RiotStatus Status;
        public TargetLand Land;
        public bool NeedStop = false;
        GSPlayer Player;
        public Riot(TargetLand land)
        {
            Land = land;
            //Land.Player.CreateCryptLand();
            //Land.Player.CreateArrayForPlayer();
            StartTime = DateTime.Now;
            EndTime = StartTime + TimeSpan.FromHours(ServerLinks.Parameters.RiotPrepare);
            Status = RiotStatus.PrepareRiot;
            BattleEvents.WaitRiot(this);
            Land.SetRiot(this);
            Player = Land.GetPlayer();
            if (Player != null)
            {
                Player.CreateCryptLand();
                Player.CreateArrayForPlayer();
            }
        }
        public void BeginRiot()
        {
            StartTime = DateTime.Now;
            EndTime = StartTime + TimeSpan.FromHours(ServerLinks.Parameters.RiotLength);
            Status = RiotStatus.Riot;
            BattleEvents.WaitRiot(this);
            if (Player != null)
            {
                Player.CreateCryptLand();
                Player.CreateArrayForPlayer();
            }
        }
        public void EndRiot()
        {
            Land.SetRiot(null);
            if (Player != null)
            {
                Player.CreateCryptLand();
                Player.CreateArrayForPlayer();
            }
        }
        public void ChangeStatus()
        {
            if (Status == RiotStatus.PrepareRiot)
                BeginRiot();
            else
                EndRiot();
        }
        public void Conquer()
        {
            NeedStop = true;
            Land.SetRiot(null);
        }
    }*/
}
