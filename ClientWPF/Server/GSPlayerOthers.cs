using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Client
{
    public partial class GSPlayer
    {
        /*static Schema BasicLinkorSchema;
        static Schema BasicScoutSchema;
        static Schema BasicCorvetteSchema;
        public static void CreateBasicShipsSchamas()
        {
            BasicLinkorSchema = Enemy.GetSchema(3215, 2251, 3262, 275, 280, 350, 340, 0, 510, 1580, 0, 0, 0, 0, 0, 0, 5);
            BasicScoutSchema = Enemy.GetSchema(200, 250, 1261, 4277, 280, 1320, 0, 0, 620, 1600, 0, 0, 0, 0, 0, 0, 2);
            BasicCorvetteSchema = Enemy.GetSchema(205, 2251, 260, 275, 1280, 310, 0, 0, 2572, 500, 0, 0, 0, 0, 0, 0, 1);
        }*/
        public class ScienceBonus
        {
            LandMult _LandMult = new LandMult();
            public LandMult Get
            {
                get { return _LandMult; }
            }
        }
        public static string GetPatch()
        {
            //string truncpatch = "ClientWPF\\ClientWPF\\bin\\Debug";
            string Dir = Directory.GetCurrentDirectory();
            Dir += "\\";
            //Dir = Dir.Remove(Dir.Length - truncpatch.Length);
            //Dir = Dir + "Bases\\";
            return Dir;
        }
        /*public static void LoadSingleGame()
        {
            string gamebase = "GameWorld.txt";
            byte[] array = File.ReadAllBytes(gamebase);
            int i = 0; i += 28; i += Long_SeporatorLength; i += 6; i += Long_SeporatorLength;
            ServerLinks.Market = new GSMarket();
            long market_metal = BitConverter.ToInt64(array, i); i += 8;
            long market_chips = BitConverter.ToInt64(array, i); i += 8;
            long market_anti = BitConverter.ToInt64(array, i); i += 8;
            ServerLinks.Market.Metal = market_metal;
            ServerLinks.Market.Chips = market_chips;
            ServerLinks.Market.Anti = market_anti;
            ServerLinks.Market.Calculate();
            i += Long_SeporatorLength; i += 1; i += Long_SeporatorLength; i += 4; i += Long_SeporatorLength;
            long dateticks = BitConverter.ToInt64(array, i); i += 8;
            ServerLinks.NowTime = new DateTime(dateticks);
            i += Long_SeporatorLength; i += 1; i += Long_SeporatorLength; i += 5; i += Long_SeporatorLength;
            int stars = BitConverter.ToInt32(array, i); i += 4;
            SortedList<int, GSStar> BasicStars = Gets.GetStars((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.StarsGameData].File);
            ServerLinks.GSStars = new SortedList<int, ServerStar>();
            for (int j=0;j< stars;j++)
            {
                int starid = BitConverter.ToInt32(array, i); i += 4;
                GSStar clientstar = BasicStars[starid];
                ServerStar star = new ServerStar(starid, clientstar.En, clientstar.Ru, clientstar.X, clientstar.Y, clientstar.Class, clientstar.SizeModifier, 255);
                ServerLinks.GSStars.Add(star.ID, star);
            }
            //ServerLinks.Enemy.Init(new Random());
            //ServerStar.CreateEnemyArray();
            byte[] starsarray = ServerStar.GetArray(ServerLinks.GSStars);
            Links.Stars = Gets.GetStars(starsarray);
            i += Long_SeporatorLength; i += 1; i += Long_SeporatorLength; i += 7; i += Long_SeporatorLength;
            int planets = BitConverter.ToInt32(array, i); i += 4;
            ServerLinks.GSPlanets = new SortedList<int, ServerPlanet>();
            for (int j = 0; j < planets; j++)
            {
                ServerPlanet planet = ServerPlanet.LoadPlanet(array, ref i);
                ServerLinks.GSPlanets.Add(planet.ID, planet);
            }
            Links.Planets = new SortedList<int, GSPlanet>();
            foreach (ServerPlanet planet in ServerLinks.GSPlanets.Values)
                Links.Planets.Add(planet.ID, new GSPlanet(planet.ID, planet.Name, planet.StarID, planet.Orbit, planet.Size, planet.Moons,
                    planet.HasBelt, planet.ImageType,planet.PlanetType, planet.MaxPopulation, planet.Locations, planet.RareBuilding));
            GameScience.SetBasicScienceList();
            i += Long_SeporatorLength; i += 1; i += Long_SeporatorLength; i += 7; i += Long_SeporatorLength;
            ServerLinks.GSClansByName = new SortedList<string, ServerClan>();
            ServerLinks.GSClanByID = new SortedList<int, ServerClan>();
            
           
            //ServerLinks.GSAccountsByEmail = new SortedList<GSString, GSAccount>();
            ServerLinks.GSAccountsByID = new SortedList<int, GSAccount>();
            //ServerLinks.GSPlayersByName = new SortedList<GSString, GSPlayer>();
            ServerLinks.GSPlayersByID = new SortedList<int, GSPlayer>();
            ServerLinks.GSLands = new SortedList<int, ServerLand>();
            for (;;)
            {
                byte znak = array[i]; i++;
                if (znak == Plus)
                {
                    int playerid = BitConverter.ToInt32(array, i); i += 4;
                    GSString playername = new GSString(array, i); i += playername.Array.Length;
                    int clanid = BitConverter.ToInt32(array, i); i += 4;
                    i += Short_SeporatorLength;
                    long credits = BitConverter.ToInt64(array, i); i += 8;
                    int metals = BitConverter.ToInt32(array, i); i += 4;
                    int chips = BitConverter.ToInt32(array, i); i += 4;
                    int anti = BitConverter.ToInt32(array, i); i += 4;
                    i += Short_SeporatorLength;
                    int sciencelength = BitConverter.ToInt32(array, i); i += 4;
                    byte[] sciencearray = CopyArray(array, i, sciencelength);
                    i += sciencelength; i += Short_SeporatorLength;
                    int schemaslength = BitConverter.ToInt32(array, i); i += 4;
                    byte[] schemasarray = CopyArray(array, i, schemaslength);
                    i += schemaslength; i += Short_SeporatorLength;
                    int landslength = BitConverter.ToInt32(array, i); i += 4;
                    byte[] landsarray = CopyArray(array, i, landslength);
                    i += landslength; i += Short_SeporatorLength;
                    int newlandslength = BitConverter.ToInt32(array, i); i += 4;
                    byte[] newlandsarray = CopyArray(array, i, newlandslength);
                    i += newlandslength; i += Short_SeporatorLength;
                    int fleetslength = BitConverter.ToInt32(array, i); i += 4;
                    byte[] fleetsarray = CopyArray(array, i, fleetslength);
                    i += fleetslength; i += Short_SeporatorLength;
                    int shipslength = BitConverter.ToInt32(array, i); i += 4;
                    byte[] shipsarray = CopyArray(array, i, shipslength);
                    i += shipslength; i += Short_SeporatorLength;
                    int pilotslength = BitConverter.ToInt32(array, i); i += 4;
                    byte[] pilotsarray = CopyArray(array, i, pilotslength);
                    i += pilotslength; i += Short_SeporatorLength;
                    
                    int premiumlength = BitConverter.ToInt32(array, i); i += 4;
                    byte[] premiumarr = CopyArray(array, i, premiumlength);
                    i += premiumlength; i += Short_SeporatorLength;
                    //int helplength = BitConverter.ToInt32(array, i); i += 4;
                    //byte[] helparray = CopyArray(array, i, helplength);
                    //i += helplength;
                    Quest_Position quest = (Quest_Position)array[i]; i++;
                    i += Short_SeporatorLength;
                    byte storyline = array[i]; i++;
                    i += Long_SeporatorLength;                
                    GSPlayer player = new GSPlayer(playerid, playername, credits, metals, chips, anti, sciencearray,
                        clanid, landsarray, shipsarray, schemasarray, pilotsarray, fleetsarray, quest, premiumarr, storyline);
                    ServerLinks.GSPlayersByID.Add(player.ID, player);
                    player.ReconShips(shipsarray);
                    player.ReconLands();
                    player.ReconFleets(fleetsarray);
                    player.ReconNewLands(newlandsarray);
                }
                else
                    break;
            }
            SciencePrice.CalculateBases();
            //ServerClan.SelectLeadersAll();
            //Console.WriteLine("Load Done Accounts {0} Players {1} Clans {2} Time {3}", ServerLinks.GSAccountsByID.Count, ServerLinks.GSPlayersByID.Count, ServerLinks.GSClanByID.Count, watch.ElapsedMilliseconds);

        }*/
        public static byte[] CopyArray(byte[] array, int startindex, int length)
        {
            byte[] result = new byte[length];
            for (int i = 0; i < length; i++, startindex++)
                result[i] = array[startindex];
            return result;
        }
        public static void SaveAll()
        {
            UTF8Encoding enc = new UTF8Encoding();
            List<byte> list = new List<byte>();
            DateTime now = DateTime.Now;
            list.AddRange(enc.GetBytes(string.Format("GameBase {0:00}.{1:00}.{2:0000} {3:00}:{4:00}:{5:00}", now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Second)));
            list.AddRange(Long_Seporator);
            list.AddRange(SaveMarket());
            list.AddRange(SaveDate());
            list.AddRange(SaveStars());
            list.AddRange(SavePlanets());
            //list.AddRange(SaveEnemy());
           // list.AddRange(SaveClans());
            list.AddRange(SavePlayers());
            string gamebase = string.Format("GameWorld.txt");
            File.WriteAllBytes(GetPatch() + gamebase, list.ToArray());
            
        }
        public static byte[] SaveDate()
        {
            UTF8Encoding enc = new UTF8Encoding();
            List<byte> list = new List<byte>();
            list.AddRange(enc.GetBytes(string.Format("DATE")));
            list.AddRange(Long_Seporator);
            list.AddRange(BitConverter.GetBytes(ServerLinks.NowTime.Ticks));
            list.AddRange(Long_Seporator);
            list.Add(Minus);
            list.AddRange(Long_Seporator);
            return list.ToArray();
        }
        public static byte[] SaveStars()
        {
            UTF8Encoding enc = new UTF8Encoding();
            List<byte> list = new List<byte>();
            list.AddRange(enc.GetBytes(string.Format("STARS")));
            list.AddRange(Long_Seporator);
            list.AddRange(BitConverter.GetBytes(ServerLinks.GSStars.Count));
            foreach (ServerStar star in ServerLinks.GSStars.Values)
                list.AddRange(BitConverter.GetBytes(star.ID));
            list.AddRange(Long_Seporator);
            list.Add(Minus);
            list.AddRange(Long_Seporator);
            return list.ToArray();
        }
        public static byte[] SavePlanets()
        {
            UTF8Encoding enc = new UTF8Encoding();
            List<byte> list = new List<byte>();
            list.AddRange(enc.GetBytes(string.Format("PLANETS")));
            list.AddRange(Long_Seporator);
            list.AddRange(BitConverter.GetBytes(ServerLinks.GSPlanets.Count));
            foreach (ServerPlanet planet in ServerLinks.GSPlanets.Values)
                list.AddRange(planet.GetArrayForSave());
            list.AddRange(Long_Seporator);
            list.Add(Minus);
            list.AddRange(Long_Seporator);
            return list.ToArray();
        }
        public static byte[] SaveMarket()
        {
            UTF8Encoding enc = new UTF8Encoding();
            List<byte> list = new List<byte>();
            list.AddRange(enc.GetBytes(string.Format("MARKET")));
            list.AddRange(Long_Seporator);
            list.AddRange(BitConverter.GetBytes(ServerLinks.Market.Metal));
            list.AddRange(BitConverter.GetBytes(ServerLinks.Market.Chips));
            list.AddRange(BitConverter.GetBytes(ServerLinks.Market.Anti));
            list.AddRange(Long_Seporator);
            list.Add(Minus);
            list.AddRange(Long_Seporator);
            return list.ToArray();
        }
        public static byte[] SavePlayers()
        {
            UTF8Encoding enc = new UTF8Encoding();
            List<byte> list = new List<byte>();
            list.AddRange(enc.GetBytes(string.Format("PLAYERS")));
            list.AddRange(Long_Seporator);
            foreach (GSPlayer player in ServerLinks.GSPlayersByID.Values)
                list.AddRange(player.SaveArray());
            list.Add(Minus);
            list.AddRange(Long_Seporator);
            return list.ToArray();
        }
        public byte[] SaveArray()
        {
            List<byte> list = new List<byte>();
            list.Add(Plus);
            list.AddRange(BitConverter.GetBytes(ID));
            list.AddRange(Name.Array);
            if (Clan != null) list.AddRange(BitConverter.GetBytes(Clan.ID)); else list.AddRange(BitConverter.GetBytes(-1));
            list.AddRange(Short_Seporator);
            list.AddRange(BitConverter.GetBytes(Resources._Money));
            list.AddRange(BitConverter.GetBytes(Resources._Metals));
            list.AddRange(BitConverter.GetBytes(Resources._Chips));
            list.AddRange(BitConverter.GetBytes(Resources._Anti));
            list.AddRange(Short_Seporator);
            list.AddRange(BitConverter.GetBytes(Sciences.ByteArray.Length));
            list.AddRange(Sciences.ByteArray);
            list.AddRange(Short_Seporator);
            if (Schemas != null)
            {
                list.AddRange(BitConverter.GetBytes(Schemas.Length));
                list.AddRange(Schemas);
            }
            else
                list.AddRange(BitConverter.GetBytes(0));
            list.AddRange(Short_Seporator);
            CreateCryptLand();
            list.AddRange(BitConverter.GetBytes(CryptLands.Length));
            list.AddRange(CryptLands);
            list.AddRange(Short_Seporator);
            CreateNewLandsArray();
            list.AddRange(BitConverter.GetBytes(NewLandsArray.Length));
            list.AddRange(NewLandsArray);
            list.AddRange(Short_Seporator);
            byte[] fleetarray = GetFleetsArrayForBase();
            list.AddRange(BitConverter.GetBytes(fleetarray.Length));
            list.AddRange(fleetarray);
            list.AddRange(Short_Seporator);
            if (ShipsArray != null)
            {
                list.AddRange(BitConverter.GetBytes(ShipsArray.Length));
                list.AddRange(ShipsArray);
            }
            else
                list.AddRange(BitConverter.GetBytes(0));
            list.AddRange(Short_Seporator);
            if (Pilots.FreePilotsArray != null)
            {
                list.AddRange(BitConverter.GetBytes(Pilots.FreePilotsArray.Length));
                list.AddRange(Pilots.FreePilotsArray);
            }
            else
                list.AddRange(BitConverter.GetBytes(0));
            list.AddRange(Short_Seporator);
            list.AddRange(BitConverter.GetBytes(Premium.Array.Length));
            list.AddRange(Premium.Array);
            list.AddRange(Short_Seporator);
            //list.AddRange(BitConverter.GetBytes(HelpArray.Length));
            //list.AddRange(HelpArray);
            list.Add((byte)Quest.Position);
            list.AddRange(Short_Seporator);
            list.Add(StoryLinePosition[0]);
            list.AddRange(Long_Seporator);
            return list.ToArray();
        }
        public byte[] GetFleetsArrayForBase()
        {
            List<byte> list = new List<byte>();
            foreach (ServerLand land in Lands.Values)
            {
                if (land.Fleets.Count == 0) continue;
                list.AddRange(BitConverter.GetBytes(land.ID));
                list.AddRange(BitConverter.GetBytes(land.Fleets.Count));
                foreach (ServerFleet fleet in land.Fleets.Values)
                    list.AddRange(fleet.GetArrayForBase());
            }
            return list.ToArray();
        }
        public static byte[] Long_Seporator;
        public static int Long_SeporatorLength;
        public static byte[] Short_Seporator;
        public static int Short_SeporatorLength;
        public static byte Plus;
        public static byte Minus;
        public static void Prepare()
        {
            string short_seporator = new string(new char[] { (char)13, (char)10, (char)33, (char)33, (char)33, (char)13, (char)10 });
            string long_seporator = new string(new char[] { (char)13, (char)10, (char)33, (char)33, (char)33, (char)33, (char)33, (char)33, (char)33, (char)33, (char)33, (char)33, (char)33, (char)33, (char)33, (char)33, (char)33, (char)13, (char)10 });
            UTF8Encoding enc = new UTF8Encoding();
            GSPlayer.Long_Seporator = enc.GetBytes(long_seporator);
            GSPlayer.Long_SeporatorLength = GSPlayer.Long_Seporator.Length;
            GSPlayer.Short_Seporator = enc.GetBytes(short_seporator);
            GSPlayer.Short_SeporatorLength = GSPlayer.Short_Seporator.Length;
            GSPlayer.Plus = enc.GetBytes("+")[0];
            GSPlayer.Minus = enc.GetBytes("-")[0];
        }
        /*public void CreateBasicShips()
        {
            ServerShip BasicLinkor = ServerShip.GetNewShip(BasicLinkorSchema, new byte[] { 2, 0, 0, 0 });
            ServerShip BasicScout = ServerShip.GetNewShip(BasicScoutSchema, new byte[] { 1, 0, 0, 0 });
            ServerShip BasicCorvette = ServerShip.GetNewShip(BasicCorvetteSchema, new byte[] { 3, 0, 0, 0 });
            Ships.Add(BasicLinkor.ID, BasicLinkor);
            BasicLinkor.Player = this;
            BasicLinkor.Pilot = GSPilot.GetStartPilot(1, 1, 1, 5, 255, 255);
            Ships.Add(BasicScout.ID, BasicScout);
            BasicScout.Player = this;
            BasicScout.Pilot = GSPilot.GetStartPilot(0, 0, 2, 255, 255, 255);
            Ships.Add(BasicCorvette.ID, BasicCorvette);
            BasicCorvette.Player = this;
            BasicCorvette.Pilot = GSPilot.GetStartPilot(4, 3, 0, 10, 15, 55);
            CalculateShipArray();
        }*/
    }
    public class PlayerResources
    {
        public long _Money { get; private set; }
        public int _Metals { get; private set; }
        public int _Chips { get; private set; }
        public int _Anti { get; private set; }
        public byte[] Resources;
        public BuildingValues Capacity;
        GSPlayer Player;
        public int IntMoney { get { return _Money > Int32.MaxValue ? Int32.MaxValue : (int)_Money; } }
        public PlayerResources(long money, int metal, int chips, int anti, GSPlayer player)
        {
            Player = player;
            _Money = money; _Metals = metal; _Chips = chips; _Anti = anti;
            SetResourcesArray();
        }
        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3}", _Money, _Metals, _Chips, _Anti);
        }
        /*
        public long Money { get { return _Money; } }
        public int Metals { get { return _Metals; } }
        public int Chips { get { return _Chips; } }
        public int Anti { get { return _Chips; } }
        public int Zip { get { return _Zip; } }
        */
        public BuildingValues RemovePillagedResources(BuildingValues cap)
        {
            BuildingValues result = new BuildingValues();
            if (_Metals < cap.Metall) { result.Metall = _Metals; _Metals = 0; } else { result.Metall = cap.Metall; _Metals -= cap.Metall; }
            if (_Chips < cap.Chips) { result.Chips = _Chips; _Chips = 0; } else { result.Chips = cap.Chips; _Chips -= cap.Chips; }
            if (_Anti < cap.Anti) { result.Anti = _Anti; _Anti = 0; }else { result.Anti = cap.Anti; _Anti -= cap.Anti; }
            SetResourcesArray();
            return result;
        }
        public BuildingValues RemoveConquerResources(ServerLand land)
        {
            int metals = (int)((double)land.Capacity.Metall / Capacity.Metall * _Metals);
            int chips = (int)((double)land.Capacity.Chips / Capacity.Chips * _Chips);
            int anti = (int)((double)land.Capacity.Anti / Capacity.Anti * _Anti);
            BuildingValues result = new BuildingValues(0, metals, chips, anti);
            _Metals -= result.Metall;
            _Chips -= result.Chips;
            _Anti -= result.Anti;
            SetResourcesArray();
            return result;
        }
        public void SetResourcesArray()
        {
            if (Player.MainPlayer == false) return;
            List<byte> result = new List<byte>();
            result.AddRange(BitConverter.GetBytes(_Money));
            result.AddRange(BitConverter.GetBytes(_Metals));
            result.AddRange(BitConverter.GetBytes(_Chips));
            result.AddRange(BitConverter.GetBytes(_Anti));
            if (Player.Premium == null)
                result.AddRange(BitConverter.GetBytes(0));
            else
                result.AddRange(BitConverter.GetBytes(Player.Premium.PremiumDays));
            Resources = result.ToArray();
        }
        public bool CheckPrice(ItemPrice price)
        {
            if (_Money < price.Money) return false;
            if (_Metals < price.Metall) return false;
            if (_Chips < price.Chips) return false;
            if (_Anti < price.Anti) return false;
            return true;
        }
        public bool CheckRepair(int money, int zip)
        {
            if (_Money < money) return false;
            return true;
        }
        public void CheckCapacity()
        {
            //if (_Money > Capacity.Money) _Money = Capacity.Money;
            //if (!Player.Premium.Money && _Money > ServerLinks.Parameters.PremiumMoneyCapacity) _Money = ServerLinks.Parameters.PremiumMoneyCapacity;
            if (_Metals > Capacity.Metall) _Metals = Capacity.Metall;
            if (_Chips > Capacity.Chips) _Chips = Capacity.Chips;
            if (_Anti > Capacity.Anti) _Anti = Capacity.Anti;
        }
        public void AddMoney(int money)
        {
            _Money += money;
            SetResourcesArray();
        }
        public void ExchangeMetal(int metal, long money)
        {
            _Money += money; _Metals += metal;
            CheckCapacity();
            SetResourcesArray();
        }
        public void ExchangeChips(int chips, long money)
        {
            _Money += money; _Chips += chips;
            CheckCapacity();
            SetResourcesArray();
        }
        public void ExchangeAnti(int anti, long money)
        {
            _Money += money; _Anti += anti;
            CheckCapacity();
            SetResourcesArray();
        }
        public void AddResources(ServerLand land)
        {
            _Money += (int)land.GetAddParameter(Building2Parameter.MoneyAdd);
            _Metals += (int)land.GetAddParameter(Building2Parameter.MetallAdd);
            _Chips += (int)land.GetAddParameter(Building2Parameter.ChipAdd);
            _Anti += (int)land.GetAddParameter(Building2Parameter.AntiAdd);
            CheckCapacity();
            SetResourcesArray();
        }
        public void AddResources(Reward2 reward)
        {
            _Money += reward.Money;
            _Metals += reward.Metall;
            _Chips += reward.Chips;
            _Anti += reward.Anti;
            CheckCapacity();
            SetResourcesArray();
        }
        public void FillWarehouse(double metal, double chips, double anti)
        {
            if (_Metals < Capacity.Metall * metal) _Metals = (int)(Capacity.Metall * metal);
            if (_Chips < Capacity.Chips * chips) _Chips = (int)(Capacity.Chips * chips);
            if (_Anti < Capacity.Anti * anti) _Anti = (int)(Capacity.Anti * anti);
            CheckCapacity();
            SetResourcesArray();
        }
        public void AddResources(BuildingValues value)
        {
            _Money += value.Money;
            _Metals += value.Metall;
            _Chips += value.Chips;
            _Anti += value.Anti;
            //Console.WriteLine("{0} {1} {2} {3} {4}", _Money, _Metals, _Chips, _Anti, _Zip);
            CheckCapacity();
            SetResourcesArray();
        }
        public void AddHalfPrice(ItemPrice value)
        {
            BuildingValues newvalue = new BuildingValues();
            newvalue.Money = value.Money / 2;
            newvalue.Metall = value.Metall / 2;
            newvalue.Chips = value.Chips / 2;
            newvalue.Anti = value.Anti / 2;
            AddResources(newvalue);
        }
        public bool RemovePrice(ItemPrice price)
        {
            if (!CheckPrice(price)) return false;
            _Money -= price.Money;
            _Metals -= price.Metall;
            _Chips -= price.Chips;
            _Anti -= price.Anti;
            SetResourcesArray();
            return true;
        }
        public bool RemovePrice(long money)
        {
            if (_Money < money) return false;
            _Money -= money;
            SetResourcesArray();
            return true;
        }
        /*
        public bool Repair(int money, int zip)
        {
            if (!CheckRepair(money, zip)) return false;
            _Money -= money;
            SetResourcesArray();
            return true;
        }
         */
        public string ForSave()
        {

            return "'" + _Money.ToString() + "','" +
                _Metals.ToString() + "','" +
                _Chips.ToString() + "','" +
                _Anti.ToString() + "'";
        }
    }
    public class PlayerPilots
    {
        public byte[] AcademyPilots;
        public byte[] AcademyPilotsStep1;
        public SortedSet<GSPilot> FreePilots;
        public byte[] FreePilotsArray;
        public byte[] FreePilotsStep1;
        public PlayerPilots(byte[] freepilotsarray)
        {
            FreePilots = new SortedSet<GSPilot>();
            if (freepilotsarray == null) return;
            for (int i = 0; i < freepilotsarray.Length; i += 10)
            {
                GSPilot pilot = GSPilot.GetPilot(freepilotsarray, i);
                FreePilots.Add(pilot);
            }
            CreateArray();
            //FreePilotsArray=freepilotsarray;
        }
        public void AddPilot(GSPilot pilot)
        {
            FreePilots.Add(pilot);
            CreateArray();
        }
        public void HirePilot(byte pos)
        {
            List<GSPilot> academy = new List<GSPilot>();
            for (int i = 0; i < AcademyPilots.Length; i += 10)
            {
                GSPilot pilot = GSPilot.GetPilot(AcademyPilots, i);
                academy.Add(pilot);
            }
            FreePilots.Add(academy[pos]);
            CreateArray();
            academy.RemoveAt(pos);
            List<byte> newacademy = new List<byte>();
            foreach (GSPilot pilot in academy)
                newacademy.AddRange(pilot.GetArray());
            AcademyPilots = newacademy.ToArray();

        }
        /// <summary> Метод удаляет пилота из перечня свободных пилотов </summary>
        public bool DismissPilot(GSPilot pilot)
        {
            if (!FreePilots.Contains(pilot)) return false;
            FreePilots.Remove(pilot);
            CreateArray();
            return true;
        }
        void CreateArray()
        {
            List<byte> array = new List<byte>();
            foreach (GSPilot pilot in FreePilots)
                array.AddRange(pilot.GetArray());
            FreePilotsArray = array.ToArray();
            array = new List<byte>();
            array.Add(6); array.Add(4);
            array.AddRange(BitConverter.GetBytes(FreePilotsArray.Length));
            array.AddRange(FreePilotsArray);
            FreePilotsStep1 = array.ToArray();
        }

    }
}
