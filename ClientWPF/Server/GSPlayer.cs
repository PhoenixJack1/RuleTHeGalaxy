using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    partial class GSPlayer
    {
        public int ID;
        public bool MainPlayer = false;
        public GSString Name;
        byte[] NameArrayStep1; //именной массив с номером
        public ServerClan Clan;
        public ServerClan RequestClan; //клан, в который направлена заявка
        public DateTime ClanChangesTime; //дата последнего изменения клана
        public long Random;
        public PlayerResources Resources { get; private set; }
        //public int MaxLandsCounts;
        public SortedList<int, ServerLand> Lands;
        public SortedList<int, ServerAvanpost> NewLands;
        public byte[] CryptLands;
        public byte[] NewLandsArray;
        public byte[] LandsArrayStep1;
        public byte[] NewLandsArrayStep1;
        public PlayerSciencesList Sciences;
        public ScienceBonus ScienceMult = new ScienceBonus();
        public byte[] Schemas; //список схем
        public byte[] SchemasArrayStep1; //список схем с номером
        public SortedList<long, ServerShip> Ships { get; private set; }
        public byte[] ShipsArray;
        public byte[] ShipsArrayStep1;
        public PlayerPilots Pilots;
        public byte[] FleetsArray;
        public byte[] FleetsArrayStep1;
        public byte[] ArrayStep; //сложенные в массив данные о 0)имени игрока 1)имеющиеся исследования 2)схемы кораблей 3)список пилотов (новые и свободные)
                                 //4) колонии 5)корабли 6)флоты
        public SortedList<long, ServerBattle> Battles = new SortedList<long, ServerBattle>();
        public byte[] BasicBattleArray;
        public ServerGameNotice Notice = new ServerGameNotice();
        public ServerPremium Premium;
        public ServerQuest Quest;
        public byte[] StoryLinePosition;
        static byte[] Nulllongarray = new byte[8];
        public byte[] PlanetSidesArray2 = new byte[0];
        public SortedList<Artefact, ushort> Artefacts = new SortedList<Artefact, ushort>();
        public byte[] ArtefactsArray = new byte[0];
        public byte[] Mission2Array = new byte[0];
        public SortedList<Mission2Type, byte> Mission2Dificulties;
        public GlobalAIParams GlobalAIParams;
        public ServerMarket LocalMarket;
        public ServerMarket GlobalMarket;
        public ServerMarket BlackMarket;
        public GSPlayer(int id, GSString name, long money, int metals, int chips, int anti, byte storyline, bool mainplayer, GlobalAIParams aiparams)
        {
            ID = id;
            MainPlayer = mainplayer;
            GlobalAIParams = aiparams;
           
            Name = name;
            CreateNameArray();
            Resources = new PlayerResources(money, metals, chips, anti, this);
            //MaxLandsCounts = ServerLinks.Parameters.MaxLandsCount;
            Sciences = new PlayerSciencesList(null, this);
            if (GlobalAIParams != null) GlobalAIParams.SetPlayer(this);

            Lands = new SortedList<int, ServerLand>();
            CreateCryptLand();
            NewLands = new SortedList<int, ServerAvanpost>();
            CreateNewLandsArray();

            StoryLinePosition = new byte[] { storyline, 0 }; //0 - миссия не начата, 1 - начата
            AppendShipSchemas(null);
            Ships = new SortedList<long, ServerShip>();
            Pilots = new PlayerPilots(null);
            byte[] pilotgroup = GSPilot.GetPilotGroup();
            Pilots.AcademyPilots = pilotgroup;
            //LocalMarket = new ServerMarket(this, MarketType.Local);
            //FleetsArray = 
            GetFleetsArray();
            CreateNewPilotsArray();
            CreateArtefactsArray();
            CreateArrayForPlayer();

            Mission2Dificulties = new SortedList<Mission2Type, byte>();
            for (int i = 0; i < 15; i++) Mission2Dificulties.Add((Mission2Type)i, 0);
        }
        /// <summary> Метод возвращает сложность миссии, равную как сложность данного вида миссий и среднюю сложность всех остальных видов миссий </summary>
        public int GetMission2Dificulty(Mission2Type type)
        {
            int sum = 0;
            foreach (int v in Mission2Dificulties.Values)
                sum += v;
            return Mission2Dificulties[type] + sum / Mission2Dificulties.Count;
        }
        public void CreateArtefactsArray()
        {
            List<byte> list = new List<byte>();
            list.Add(11); list.Add(0);
            list.AddRange(BitConverter.GetBytes(Artefacts.Count * 4));
            foreach (KeyValuePair<Artefact, ushort> pair in Artefacts)
            {
                list.AddRange(BitConverter.GetBytes(pair.Key.ID));
                list.AddRange(BitConverter.GetBytes(pair.Value));
            }
            ArtefactsArray = list.ToArray();
        }
        public void CreatePlanetInfo()
        {
            if (MainPlayer == false) return;
            SortedSet<ServerPlanet> KnownPlanets = new SortedSet<ServerPlanet>();
            foreach (ServerLand land in Lands.Values)
            {
                foreach (ServerPlanet planet in land.Planet.Star.Planets.Values)
                    if (KnownPlanets.Contains(planet) == false) KnownPlanets.Add(planet);
                foreach (ServerFleet fleet in land.Fleets.Values)
                    if (fleet.Target != null && fleet.Target.Mission == FleetMission.Scout && fleet.Target.Order == FleetOrder.Defense)
                        foreach (ServerPlanet planet in fleet.Target.GetStar().Planets.Values)
                            if (KnownPlanets.Contains(planet) == false) KnownPlanets.Add(planet);
            }
            foreach (ServerAvanpost avanpost in NewLands.Values)
                if (KnownPlanets.Contains(avanpost.Planet) == false) KnownPlanets.Add(avanpost.Planet);
            if (ServerLinks.GreenTeam != null)
                foreach (ServerLand land in ServerLinks.GreenTeam.Lands.Values)
                    if (KnownPlanets.Contains(land.Planet) == false) KnownPlanets.Add(land.Planet);
            if (ServerLinks.TechnoTeam != null)
                foreach (ServerLand land in ServerLinks.TechnoTeam.Lands.Values)
                    if (KnownPlanets.Contains(land.Planet) == false) KnownPlanets.Add(land.Planet);
            if (ServerLinks.Alien != null)
                foreach (ServerLand land in ServerLinks.Alien.Lands.Values)
                    if (KnownPlanets.Contains(land.Planet) == false) KnownPlanets.Add(land.Planet);
            List<byte> result = new List<byte>();
            result.Add(9); result.Add(4);
            result.AddRange(BitConverter.GetBytes(ServerLinks.GSPlanets.Count * 5));
            byte[] NoSideInfo = new byte[] { 255, 0, 0, 0 };
            foreach (ServerPlanet planet in ServerLinks.GSPlanets.Values)
                if (KnownPlanets.Contains(planet))
                {
                    if (planet.MaxPopulation == 0) { result.Add((byte)PlanetSide.No); result.AddRange(NoSideInfo); }
                    else if (planet.Lands.Count == 0 && planet.NewLands.Count == 0) { result.Add((byte)PlanetSide.Free); result.AddRange(NoSideInfo); }
                    else if (planet.Lands.Count > 0)
                    {
                        ServerLand land = planet.Lands.Values[0];
                        result.AddRange(land.GetPlanetInfoArray());
                        //if (land.Player == ServerLinks.MainPlayer) { result.Add((byte)PlanetSide.Player); result.Add(0); result.Add(land.PillageOrConquer); }
                        //else if (land.Player == ServerLinks.GreenTeam) { result.Add((byte)PlanetSide.GreenTeam); result.Add(0); result.Add(land.PillageOrConquer); }
                        //else if (land.Player == ServerLinks.TechnoTeam) { result.Add((byte)PlanetSide.Techno); result.Add(0); result.Add(land.PillageOrConquer); }
                        //else if (land.Player == ServerLinks.Alien) { result.Add((byte)PlanetSide.Alien); result.Add(0); result.Add(land.PillageOrConquer); }
                        //else if (land.Player == ServerLinks.MercTeam) { result.Add((byte)PlanetSide.Mercs); result.Add(0); result.Add(land.PillageOrConquer); }
                        //else if (land.Player == ServerLinks.PirateTeam) { result.Add((byte)PlanetSide.Pirate); result.Add(0); result.Add(land.PillageOrConquer); }
                    }
                    else if (planet.NewLands.Count>0)
                    {
                        ServerAvanpost avanpost = planet.NewLands.Values[0];
                        result.AddRange(avanpost.GetPlanetInfoArray());
                        //if (avanpost.Player == ServerLinks.MainPlayer) { result.Add((byte)PlanetSide.Player); result.Add(1); result.Add(avanpost.PillageOrConquer); }
                        //else if (avanpost.Player == ServerLinks.GreenTeam) { result.Add((byte)PlanetSide.GreenTeam); result.Add(1); result.Add(avanpost.PillageOrConquer); }
                        //else if (avanpost.Player == ServerLinks.TechnoTeam) { result.Add((byte)PlanetSide.Techno); result.Add(1); result.Add(avanpost.PillageOrConquer); }
                        //else if (avanpost.Player == ServerLinks.Alien) { result.Add((byte)PlanetSide.Alien); result.Add(1); result.Add(avanpost.PillageOrConquer); }
                        //else if (avanpost.Player == ServerLinks.MercTeam) { result.Add((byte)PlanetSide.Mercs); result.Add(1); result.Add(avanpost.PillageOrConquer); }
                        //else if (avanpost.Player == ServerLinks.PirateTeam) { result.Add((byte)PlanetSide.Pirate); result.Add(1); result.Add(avanpost.PillageOrConquer); }
                    }
                }
            else { result.Add((byte)PlanetSide.Unknown); result.AddRange(NoSideInfo); }
            /*foreach (ServerPlanet planet in ServerLinks.GSPlanets.Values)
            {
                result.AddRange(BitConverter.GetBytes((ushort)planet.ID));
                if (planet.PlanetSide == PlanetSide.Alien || planet.PlanetSide == PlanetSide.GreenTeam || planet.PlanetSide == PlanetSide.Techno)
                    result.Add((byte)planet.PlanetSide);
                else if (planet.Star.CheckVision())
                    result.Add((byte)planet.PlanetSide);
                else
                    result.Add((byte)PlanetSide.Unknown);
            }*/
            PlanetSidesArray2 = result.ToArray();
            CreateArrayForPlayer();
        }
        void CreateNameArray()
        {
            List<byte> result = new List<byte>();
            result.Add(0); result.Add(1);
            //result.AddRange(BitConverter.GetBytes(Name.Array.Length));
            result.AddRange(BitConverter.GetBytes(Name.Array.Length));
            result.AddRange(Name.Array);
            NameArrayStep1 = result.ToArray();
        }
        public void ReconFleets(byte[] fleets)
        {
            if (fleets == null || fleets.Length == 0) return;
            for (int i = 0; i < fleets.Length;)
            {
                int landid = BitConverter.ToInt32(fleets, i);
                i += 4;
                ServerLand land = Lands[landid];
                int fleetscount = BitConverter.ToInt32(fleets, i);
                i += 4;
                for (int j = 0; j < fleetscount; j++)
                {
                    ServerFleet fleet = ServerFleet.GetFleet(fleets, ref i, landid, this);
                    //i += fleet.Array.Length;
                }
            }
            GetFleetsArray();
            CreateArrayForPlayer();
        }
        public void ReconShips(byte[] ships)
        {
            if (ships != null)
            {
                for (int i = 0; i < ships.Length;)
                    ServerShip.GetShip(ships, ref i);
            }
            CreateArrayForPlayer();
        }/// <summary> Метод создаёт новую колонию на планете и присваивает её игроку. Если на планете были колонии - то они уничтожаются
        /// </summary>
        public void AddLand(int PlanetID, string parameters)
        {
            ServerPlanet planet = ServerLinks.GSPlanets[PlanetID];
            planet.RemoveAllLands();
            ServerLand land = ServerLand.GetLand(PlanetID, ID);
            ServerLinks.GSLands.Add(land.ID, land);
            if (parameters!= null)
            {
                TextPosition[] positions = TextPosition.GetPositions(parameters);
                foreach (TextPosition pos in positions)
                {
                    byte sectorpos = Byte.Parse(pos.Title.Substring(1));
                    switch (pos.StringValues[0])
                    {
                        case "L": land.BuildSector(SectorTypes.Live, sectorpos); break;
                        case "Mo": land.BuildSector(SectorTypes.Money, sectorpos); break;
                        case "Me": land.BuildSector(SectorTypes.Metall, sectorpos); break;
                        case "MC": land.BuildSector(SectorTypes.MetalCap, sectorpos); break;
                        case "C": land.BuildSector(SectorTypes.Chips, sectorpos); break;
                        case "CC": land.BuildSector(SectorTypes.ChipsCap, sectorpos); break;
                        case "A": land.BuildSector(SectorTypes.Anti, sectorpos); break;
                        case "AC": land.BuildSector(SectorTypes.AntiCap, sectorpos); break;
                        case "R": land.BuildSector(SectorTypes.Repair, sectorpos); break;
                        case "W": land.BuildSector(SectorTypes.War, sectorpos); break;
                        //case "Rnd": 
                    }
                    for (int i = 0; i < pos.IntValues.Length; i++)
                        for (int j = 0; j < pos.IntValues[i]; j++)
                            land.BuildBuilding(sectorpos, (byte)i);
                    //foreach (int i in pos.IntValues)
                    //    land.BuildBuilding(ServerLinks.GSBuildings[(ushort)i], sectorpos, 1);
                }
            }
            //Resources.SetResourcesArray();
            //CalculateResCap();
            CreateCryptLand();
            CreateArrayForPlayer();
        }
        public void AddAvanpost(int PlanetID, bool force, string parameters)
        {
            if (force)
            {
                ServerPlanet planet = ServerLinks.GSPlanets[PlanetID];
                planet.RemoveAllLands();
            }
            ServerAvanpost avanpost = ServerAvanpost.GetAvanpost(PlanetID, ID);
            if (parameters!= null)
            {
                int moneypercent = 0; int metalpercent = 0; int chipspercent = 0; int antipercent = 0;
                TextPosition[] positions = TextPosition.GetPositions(parameters);
                foreach (TextPosition pos in positions) 
                    switch (pos.Title)
                    {
                        case "MONEY": moneypercent = pos.IntValues[0]; break;
                        case "METALL": metalpercent = pos.IntValues[0]; break;
                        case "CHIPS": chipspercent = pos.IntValues[0]; break;
                        case "ANTI": antipercent = pos.IntValues[0]; break;
                    }
                avanpost.SetBuild(moneypercent, metalpercent, chipspercent, antipercent);
            }
            CreateNewLandsArray();
            CreatePlanetInfo();
            CreateArrayForPlayer();
        }
        public void AddArtefacts(int[] list)
        {
            foreach (int i in list)
                Artefacts.Add(Links.Artefacts[(ushort)i], 1);
            CreateArtefactsArray();
            CreateArrayForPlayer();
        }
        //метод расшифровывает параметры земли. Если строка пуста, то создаёт новую
        public void ReconLands()
        {
            if (CryptLands == null)
            {
                ServerLand land = ServerLand.GetBasicLand(ID);
                if (land.PlayerID != ID) Console.WriteLine("ERROR while creating land at player id=" + ID.ToString());
                if (ServerLinks.LandsID.ID < land.ID) ServerLinks.LandsID.ID = land.ID;
                ServerLinks.GSLands.Add(land.ID, land);
            }
            else
            {
                ServerLand land;
                byte[] curCrypt = CryptLands;
                for (int s = 0; s < curCrypt.Length;)
                {
                    land = ServerLand.GetLand(curCrypt, ref s);
                    if (land.PlayerID != ID) Console.WriteLine("ERROR while recon land at player id=" + ID.ToString());
                    if (ServerLinks.LandsID.ID < land.ID) ServerLinks.LandsID.ID = land.ID;
                    ServerLinks.GSLands.Add(land.ID, land);
                }
            }
            CreateCryptLand();
            CreateArrayForPlayer();
        }
        public void ReconNewLands(byte[] array)
        {
            for (int s = 0; s < array.Length;)
            {
                ServerAvanpost newland = ServerAvanpost.GetLand(array, ref s);
                if (newland.PlayerID != ID) Console.WriteLine("ERROR while recon land at player id=" + ID.ToString());
                if (ServerLinks.LandsID.ID < newland.ID) ServerLinks.LandsID.ID = newland.ID;
            }
            CreateNewLandsArray();
            CreateArrayForPlayer();
        }
        public void CreateArrayForPlayer()
        {
            if (MainPlayer == false) return;
            //public byte[] ArrayStep1; //сложенные в массив данные о 0)имени игрока 1)имеющиеся исследования 2)схемы кораблей 3)список пилотов (новые и свободные)
            //4) колонии 5)корабли 6)флоты
            List<byte> result = new List<byte>();
            result.AddRange(Nulllongarray);
            result.AddRange(NameArrayStep1);
            if (Clan == null)
                result.AddRange(ServerClan.NullBasicArray);
            else
                result.AddRange(Clan.BasicArray);
            result.AddRange(Sciences.ByteArrayStep1);
            result.AddRange(SchemasArrayStep1);
            if (ShipsArrayStep1 != null) result.AddRange(ShipsArrayStep1);
            if (LandsArrayStep1 != null) result.AddRange(LandsArrayStep1);
            if (NewLandsArrayStep1 != null) result.AddRange(NewLandsArrayStep1);
            if (Pilots != null && Pilots.AcademyPilotsStep1 != null)
                result.AddRange(Pilots.AcademyPilotsStep1);
            if (Pilots != null && Pilots.FreePilotsStep1 != null)
                result.AddRange(Pilots.FreePilotsStep1);
            if (FleetsArrayStep1 != null)
                result.AddRange(FleetsArrayStep1);
            if (BasicBattleArray != null)
                result.AddRange(BasicBattleArray);
            //result.AddRange(ServerStar.EnemyArray);
            result.AddRange(PlanetSidesArray2);
            //result.AddRange(Missions.Array);
            //result.AddRange(HelpArray);
            //if (ServerLinks.GameMode==EGameMode.Multi)
            //    result.AddRange(Premium.Array);
            //result.AddRange(Quest.Array);
            result.AddRange(new byte[] { 0, 13, 2, 0, 0, 0, StoryLinePosition[0], StoryLinePosition[1] });
            result.AddRange(ArtefactsArray);
            result.AddRange(Mission2Array);
            ArrayStep = result.ToArray();
        }
        /// <summary> Метод создаёт массив с миссиями по типу 2, доступными игроку</summary>
        public void CreateMission2Array()
        {
            if (MainPlayer == false) return;
            List<byte> result = new List<byte>();
            result.Add(0);
            result.Add(14);
            result.AddRange(new byte[4]);
            SortedSet<int> AvailableStars = new SortedSet<int>();
            foreach (ServerLand land in Lands.Values)
            {
                AvailableStars.Add(land.Planet.StarID);
                foreach (ServerFleet fleet in land.Fleets.Values)
                    if (fleet.Target != null && fleet.Target.Mission == FleetMission.Scout && fleet.Target.Order==FleetOrder.Defense)
                        AvailableStars.Add(((ServerStar)fleet.Target.Target).ID);
            }
            foreach (int i in AvailableStars)
            {
                ServerStar star = ServerLinks.GSStars[i];
                if (star.MissionInspector!= null)
                    result.AddRange(star.MissionInspector.Array);
            }
            byte[] sum = BitConverter.GetBytes(result.Count - 6);
            result[2] = sum[0]; result[3] = sum[1]; result[4] = sum[2]; result[5] = sum[3];
            Mission2Array =  result.ToArray();
            
        }
        /// <summary> Метод создаёт у игрока массив с информацией о флотах </summary>
        public void GetFleetsArray()
        {
            if (MainPlayer == false) return;
            List<byte> array = new List<byte>();
            foreach (ServerLand land in Lands.Values)
                array.AddRange(land.FleetsArray);
            FleetsArray = array.ToArray();
            array = new List<byte>();
            array.Add(7); array.Add(2);
            array.AddRange(BitConverter.GetBytes(FleetsArray.Length));
            array.AddRange(FleetsArray);
            FleetsArrayStep1 = array.ToArray();
        }
        /// <summary> метод создаёт у игрока массив с информацией о колониях.</summary>
        public void CreateCryptLand()
        {
            if (MainPlayer == false) return;
            List<byte> array = new List<byte>();
            foreach (ServerLand land in Lands.Values)
                array.AddRange(land.GetArray());
            CryptLands = array.ToArray();
            array = new List<byte>();
            array.Add(4); array.Add(1);
            array.AddRange(BitConverter.GetBytes(CryptLands.Length));
            array.AddRange(CryptLands);
            LandsArrayStep1 = array.ToArray();
        }
        public void CreateNewLandsArray()
        {
            if (MainPlayer == false) return;
            List<byte> array = new List<byte>();
            foreach (ServerAvanpost land in NewLands.Values)
                array.AddRange(land.GetArray());
            NewLandsArray = array.ToArray();
            array = new List<byte>();
            array.Add(4); array.Add(7);
            array.AddRange(BitConverter.GetBytes(NewLandsArray.Length));
            array.AddRange(NewLandsArray);
            NewLandsArrayStep1 = array.ToArray();
        }
        public void CalculateShipArray()
        {
            if (MainPlayer == false) return;
            List<byte> array = new List<byte>();
            foreach (ServerShip value in Ships.Values)
                array.AddRange(value.GetArray());
            ShipsArray = array.ToArray();
            List<byte> step1 = new List<byte>();
            step1.Add(3); step1.Add(4);
            step1.AddRange(BitConverter.GetBytes(ShipsArray.Length));
            step1.AddRange(ShipsArray);
            ShipsArrayStep1 = step1.ToArray();
        }
        public void CreateBattleArray()
        {
            List<byte> temp = new List<byte>();
            temp.Add(8); temp.Add(0);
            List<byte> array = new List<byte>();
            foreach (ServerBattle bat in Battles.Values)
                array.AddRange(bat.BasicInfo);
            temp.AddRange(BitConverter.GetBytes(array.Count));
            temp.AddRange(array);
            BasicBattleArray = temp.ToArray();
        }
        public void AddBattle(ServerBattle battle)
        {
            Battles.Add(battle.ID, battle);
            CreateBattleArray();
        }
        public void RemoveBattle(ServerBattle battle)
        {
            if (battle == null) return;
            if (Battles.ContainsKey(battle.ID))
            {
                Battles.Remove(battle.ID);
                CreateBattleArray();
            }
        }
        /// <summary> метод создаёт все массивы для флота, территории и игрока о введении флота в бой </summary>
        public void FleetInBattle(ServerFleet fleet)
        {
            if (MainPlayer == false) return;
            fleet.Array = fleet.GetArray();
            fleet.FleetBase.Land.FleetsArray = fleet.FleetBase.Land.GetFleetsArray();
            GetFleetsArray();
            CalculateShipArray();
            CreateArrayForPlayer();
        }
       
        public void BasicCalculateResCap()
        {
            Resources.Capacity = new BuildingValues();
            Resources.Capacity.Metall = ServerLinks.Parameters.PlayerMetallBaseCapacity;
            Resources.Capacity.Chips = ServerLinks.Parameters.PlayerChipsBaseCapacity;
            Resources.Capacity.Anti = ServerLinks.Parameters.PlayerAntiBaseCapacity;
            //ShipsCap = 0;
            foreach (ServerLand land in Lands.Values)
            {
                Resources.Capacity.AddCap(land);
                //ShipsCap += land.Capacity.Ships;
            }
        }
        public void CalculateResCap()
        {
            Resources.Capacity = new BuildingValues();
            Resources.Capacity.Metall = ServerLinks.Parameters.PlayerMetallBaseCapacity;
            Resources.Capacity.Chips = ServerLinks.Parameters.PlayerChipsBaseCapacity;
            Resources.Capacity.Anti = ServerLinks.Parameters.PlayerAntiBaseCapacity;
            //ShipsCap = 0;
            foreach (ServerLand land in Lands.Values)
            {
                Resources.Capacity.AddCap(land);
                //ShipsCap += land.Capacity.Ships;
            }
            Resources.CheckCapacity();
        }
        
        /*public List<byte> GetInfoForClan()
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(ID));
            list.AddRange(Name.Array);
            list.AddRange(BitConverter.GetBytes(Lands.Count));
            double peoples = 0;
            int fleets = 0;
            foreach (ServerLand land in Lands.Values)
            {
                peoples += land.Peoples;
                fleets += land.Fleets.Count;
            }
            list.AddRange(BitConverter.GetBytes(peoples));
            list.AddRange(BitConverter.GetBytes(fleets));
            list.AddRange(BitConverter.GetBytes(DefenseFleets.Count));
            list.AddRange(BitConverter.GetBytes(Battles.Count));
            list.AddRange(BitConverter.GetBytes(Ships.Count));
            list.AddRange(BitConverter.GetBytes(Sciences.SciencesArray.Count));
            return list;
        }*/
        public static bool AddLand(GSPlayer player, ServerLand land, bool isbasic, bool needcheckcapacity)
        {
                player.Lands.Add(land.ID, land);
                if (isbasic)
                    player.BasicCalculateResCap();
                else
                    player.CalculateResCap();
                player.CreateCryptLand();
                return true;

        }
        public static bool AddNewLand(GSPlayer player, ServerAvanpost land)
        {
                player.NewLands.Add(land.ID, land);
                player.CreateCryptLand();
                return true;

        }
        //метод проверяет, можно ли присвоить игроку ещё одну землю
        public bool CheckLandsCount()
        {
            return true;
        }
        public static void RemoveLand(GSPlayer player, ServerAvanpost land)
        {
           if (player.NewLands.ContainsKey(land.ID)) player.NewLands.Remove(land.ID);
        }
        public static void RemoveLand(GSPlayer player, ServerLand land)
        {
            if (player.Lands.ContainsKey(land.ID)) player.Lands.Remove(land.ID);
        }
        
        public void CreateNewPilotsArray()
        {
            List<byte> array = new List<byte>();
            array.Add(6); array.Add(1);
            array.AddRange(BitConverter.GetBytes(Pilots.AcademyPilots.Length));
            array.AddRange(Pilots.AcademyPilots);
            Pilots.AcademyPilotsStep1 = array.ToArray();
        }
        public bool AddShip(ServerShip ship)
        {
            if (Ships.ContainsKey(ship.ID)) return false;
            ship.Player = this;
            Ships.Add(ship.ID, ship);
            CalculateShipArray();
            //if (!Quest.AllDone) Quest.ReasonFinish(Reason_Finish.Ship_Builded);
            CreateArrayForPlayer();
            return true;

        }
        public bool TryRepairShip(ServerShip ship, bool isoneship)
        {
            if (Resources.RemovePrice(ship.GetRepairPrice()))
                ship.Health = 100;
            else
                return false;
            if (isoneship)
            {
                CalculateShipArray();
                CreateArrayForPlayer();
            }
            return true;
        }
        public void PassiveRepair()
        {
            bool HasRepair = false;
            foreach (ServerShip ship in Ships.Values)
            {
                if (ship.Health == 100) continue;
                if (ship.Fleet == null || ship.Fleet.Target != null) continue;
                if (ship.Fleet.FleetBase.Land.PillageFleet != null) continue;
                HasRepair = true;
                byte repairvalue = (byte)(ship.Fleet.FleetBase.Land.GetAddParameter(Building2Parameter.Repair)*5 / ship.RepairPrice);
                ship.Health += repairvalue;
                if (ship.Health > 100) ship.Health = 100;
            }
            if (HasRepair)
            {
                CalculateShipArray();
                //CreateArrayForPlayer();
            }
        }
        public void RecieveBattleResult(ServerFleet fleet, SortedList<long, byte> ShipsHealth, bool iswinner)
        {
            if (ShipsHealth.Count>0)
            {
                foreach (KeyValuePair<long, byte> pair in ShipsHealth)
                    fleet.Ships[pair.Key].Health = pair.Value;
            }
            Reward2 reward=null;
            switch (fleet.Target.Mission)
            {
                case FleetMission.StoryLine:
                    if (iswinner)
                    {
                        StoryLine2 story = (StoryLine2)fleet.Target.Target;
                        reward = story.Reward;
                        Notice.GetStoryLineWinNotice(fleet, reward, StoryLinePosition[0]);
                        RecieveStoryReward(StoryLine2.StoryLines[StoryLinePosition[0]], fleet);
                        StoryLinePosition[0]++; StoryLinePosition[1] = 0;
                        
                    }
                    else
                    {
                        Notice.GetStoryLineLoseNotice(fleet, StoryLinePosition[0]);
                        StoryLinePosition[1] = 0;
                    }
                    fleet.Target.Order = FleetOrder.Return;
                    break;
                case FleetMission.Mission:
                    ServerMission2 mission = (ServerMission2)fleet.Target.Target;
                    if (iswinner)
                    {
                        if (mission.Type == Mission2Type.BlackMarket)
                        {
                            BlackMarket.LeaveMarket(true);
                            BlackMarket = null;
                        }
                        else
                        {
                            reward = mission.CalcReward(this, true);
                            RecieveMissionReward(reward, mission.Type, fleet);
                        }
                    }
                    else
                    {
                        if (mission.Type == Mission2Type.BlackMarket)
                        {
                            BlackMarket = null;
                        }
                    }
                    fleet.Target.Order = FleetOrder.Return;
                    break;
                case FleetMission.Scout:
                    if (iswinner)
                    {
                        //Если флот победил, то он повторно направляется в патрулирование. 
                        //Если при этом снова начинается бой, то игрок должен получить информацию о том, что надо начать бой.
                        //Надо добавить игроку массив, куда будут добавляться бои, попадаемые из автоматического режима и переходить в общую информацию.
                        //Информация о нападении будет располагаться на глобальной карте 
                        //Удаляться эта информация будет...
                        StartScout(fleet);
                        FleetInBattle(fleet);
                        return;
                    }
                    else
                        //Если флот проиграл - он возвращается на базу. 
                        fleet.Target.Order = FleetOrder.Return;
                    break;
                case FleetMission.Pillage:
                    if (iswinner)
                    {
                        StartPillage(fleet);
                        return;
                    }
                    else
                        fleet.Target.Order = FleetOrder.Return;
                    break;
                case FleetMission.Conquer:
                    if (iswinner)
                    {
                        StartConquer(fleet); return;
                    }
                    else
                        fleet.Target.Order = FleetOrder.Return;
                    break;
            }
            if (iswinner && reward != null && reward.Experience > 0)
                GSPilot.DistributeExpirience(fleet, reward.Experience);
           

            fleet.CalcFleetReturnTime(iswinner);
            BattleEvents.FleetFly(fleet);
            FleetInBattle(fleet);
        }
        public void FleetReturned(ServerFleet fleet)
        {
            if (fleet.Target.SelfPlayer != fleet.FleetBase.Land.Player)
            {
                fleet.FleetDestroy();
                return;
            }

            if (fleet.Repair)
                TryRepairFleetAfterReturn(fleet);
            RemoveBattle(fleet.CurBattle);
            fleet.Target = null;
            fleet.CurBattle = null;


            fleet.Array = fleet.GetArray();
            fleet.FleetBase.Land.FleetsArray = fleet.FleetBase.Land.GetFleetsArray();
            GetFleetsArray();
            CalculateShipArray();
            CreateArrayForPlayer();
        }
        public void TryRepairFleetAfterReturn(ServerFleet fleet)
        {
            foreach (ServerShip ship in fleet.Ships.Values)
                TryRepairShip(ship, false);
            CalculateShipArray();
            CreateArrayForPlayer();
        }
       
        public void AppendShipSchemas(byte[] array)
        {
            Schemas = array;
            List<byte> result = new List<byte>();
            result.Add(3); result.Add(2);
            if (array != null)
            {
                result.AddRange(BitConverter.GetBytes(array.Length));
                result.AddRange(array);
            }
            else
                result.AddRange(BitConverter.GetBytes(0));
            SchemasArrayStep1 = result.ToArray();
            //if (!Quest.AllDone) Quest.ReasonFinish(Reason_Finish.Schema_combined);
            CreateArrayForPlayer();
        }
    }
}
