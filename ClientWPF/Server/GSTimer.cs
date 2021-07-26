using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class GSTimer
    {
        static Queue<ServerFleet> CurrentFleetQueue = new Queue<ServerFleet>();
        public static void AddFleetToQueue(ServerFleet fleet)
        {
            CurrentFleetQueue.Enqueue(fleet);
        }
        static DateTime ResourcesAdd = ServerLinks.NowTime + ServerLinks.Parameters.ResourceAddTime;
        static DateTime PeoplesGrow = ServerLinks.NowTime + ServerLinks.Parameters.PeopleGrowTime;
        public static void GameEventsPool()
        {
            //прирост населения
            if (PeoplesGrow <= ServerLinks.NowTime)
            {
                PeoplesGrow = ServerLinks.NowTime.AddMonths(1);
                PeoplesGrow = new DateTime(PeoplesGrow.Year, PeoplesGrow.Month, 1);
               
            }
            //прирост ресурсов и пассивный ремонт
            if (ResourcesAdd <=ServerLinks.NowTime)
            {
                ResourcesAdd += ServerLinks.Parameters.ResourceAddTime;
                AddPeoples();
                AddResource();
            }
            //прирост миссий
            LocalServer.CreateMissions();
            //возврат флотов
            ProcessFleetsReturn();
           

            //Управление захватами
            CheckConquer();
            //Получение ресурсов с грабежа
            AddPillageResources();
            //Обновление рынков
            CheckMarkets();
            //Ходы ИИ
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            //MakeAITurns(ServerLinks.Alien);
            //MakeAITurns(ServerLinks.GreenTeam);
            //MakeAITurns(ServerLinks.TechnoTeam);
            watch.Stop();
            //Links.Controller.mainWindow.Title = watch.ElapsedTicks.ToString();
            //MakeAITurns(ServerLinks.MercTeam);
            //MakeAITurns(ServerLinks.PirateTeam);
            
            //ServerLand testland = ServerLinks.GSPlanets[23].Lands.Values[0];
            //long battleid;
            //ServerLinks.TechnoTeam.AttackEnemyColonyPillageEvent(testland.ID, 1, 8, out battleid);
            ServerLinks.GSPlayersByID[0].CreatePlanetInfo();
            MadeStatistic();
        }
        static void CheckMarkets()
        {
            int time = (int)(ServerLinks.MainPlayer.LocalMarket.EndTime - ServerLinks.NowTime).TotalDays;
            if (time <= 0) ServerLinks.MainPlayer.LocalMarket = new ServerMarket(ServerLinks.MainPlayer, MarketType.Local);
            if (ServerLinks.MainPlayer.GlobalMarket!= null)
            {
                time= (int)(ServerLinks.MainPlayer.GlobalMarket.EndTime - ServerLinks.NowTime).TotalDays;
                if (time <= 0) ServerLinks.MainPlayer.GlobalMarket = null;
            }
        }
        public static byte[] Statictic = new byte[0];
        static void MadeStatistic()
        {
            List<GSPlayer> list = new List<GSPlayer>();
            list.Add(ServerLinks.MainPlayer);
            list.Add(ServerLinks.GreenTeam);
            list.Add(ServerLinks.TechnoTeam);
            list.Add(ServerLinks.Alien);
            List<byte> result = new List<byte>();
            foreach (GSPlayer player in list)
            {
                result.AddRange(BitConverter.GetBytes(player.Lands.Count));
                double peoplescur = 0; int peoplesmax = 0; int buildings = 0;
                int fleetscount = 0; int shipsinfleet = 0; float averagelvl = 0;
                foreach (ServerLand land in player.Lands.Values)
                {
                    peoplescur += land.Peoples; peoplesmax += land.Planet.MaxPopulation; buildings += land.BuildingsCount;
                    fleetscount += land.Fleets.Count;
                    foreach (ServerFleet fleet in land.Fleets.Values)
                    {
                        shipsinfleet += fleet.Ships.Count;
                        foreach (ServerShip ship in fleet.Ships.Values)
                            averagelvl += ship.AverageLevel;
                    }
                }
                result.AddRange(BitConverter.GetBytes(peoplescur));
                result.AddRange(BitConverter.GetBytes(peoplesmax));
                result.AddRange(BitConverter.GetBytes(buildings));
                result.AddRange(BitConverter.GetBytes(player.NewLands.Count));
                double avanpostconst = 0;
                foreach (ServerAvanpost avanpost in player.NewLands.Values)
                {
                    avanpostconst += ((double)avanpost.HaveResources.Money / avanpost.NeedResources.Money + (double)avanpost.HaveResources.Metall / avanpost.NeedResources.Metall +
                        (double)avanpost.HaveResources.Chips / avanpost.NeedResources.Chips + (double)avanpost.HaveResources.Anti / avanpost.NeedResources.Anti) / 4.0;
                }
                if (player.NewLands.Count > 0) avanpostconst = avanpostconst / player.NewLands.Count;
                result.AddRange(BitConverter.GetBytes(avanpostconst));
                result.AddRange(BitConverter.GetBytes(player.Sciences.SciencesArray.Count / 2));
                result.Add(player.Sciences.MaxLevel);
                result.AddRange(BitConverter.GetBytes(fleetscount));
                result.AddRange(BitConverter.GetBytes(shipsinfleet));
                if (shipsinfleet > 0) averagelvl = averagelvl / shipsinfleet;
                result.AddRange(BitConverter.GetBytes(averagelvl));
                result.AddRange(BitConverter.GetBytes(player.Resources._Money));
                result.AddRange(BitConverter.GetBytes(player.Resources._Metals));
                result.AddRange(BitConverter.GetBytes(player.Resources._Chips));
                result.AddRange(BitConverter.GetBytes(player.Resources._Anti));
                Statictic = result.ToArray();
            }
        }
        /*
        static void MakeAITurns(GSPlayer player)
        {
            //Получение некого бонуса - аналога выполнения миссий
            player.RecieveMissionReward(player.GlobalAIParams.GetBonusReward(), Mission2Type.No, null);
            //Колонизация планет - у каждоЙ стороны есть своя частота колонизаций
            player.GlobalAIParams.ColonizePlanets();
            //Строительство секторов - застраиваем все сектора, но делим планеты по принципу - боевая - не боевая, создание флотов в базах
            player.GlobalAIParams.BuildSectors();
            //Строительство зданий - стараемся ёмкость всех ресурсов держать на одном уровне, остальное в ресурсы. Боевые - примерно одного типа
            player.GlobalAIParams.BuildBuildings();
            //Улучшение зданий - улучшение по мере возможности. Редкое событие.  
            player.GlobalAIParams.UpgradeBuildings();
            //Уничтожение зданий - Можно убирать лишние радары или здания населения
            player.GlobalAIParams.RemoveBuildings();
            //Строительство аванпостов - тратим от 10 до 30% оставшихся ресурсов
            player.GlobalAIParams.BuildInAvanposts();
            //Исследования - тратим от 10 до 50% оставшихся ресурсов
            player.GlobalAIParams.MakeResearches();
            //Создание кораблей - тратим от 10 до 50% оставшихся ресурсов
            player.GlobalAIParams.CreateShips();
            //Размещение кораблей по флотам
            
            //Получение пилотов - если свободных пилотов много - то не делаем
            //Замена кораблей - сравниваем корабли по рейтингу, ставим самые сильные
            //Уничтожение кораблей - уничтожаем до 10% слабых кораблей
            //Ремонт кораблей - если планета не боевая - то ремонт кораблей
            //Отправка флотов на защиту
            //Отправка флотов на захват
            //Отправка флотов на грабёж
            //Торговля ресурсов - если ресурсов какого-типа избыток или недостаток - то обмен
            player.GlobalAIParams.ExchangeResources();
        }
        */
        static void AddPeoples()
        {
            foreach (ServerLand land in ServerLinks.GSLands.Values)
            {
                land.AddPeoples();
            }
        }
        static void CheckConquer()
        {
            foreach (ServerLand land in ServerLinks.GSLands.Values)
            {
                if (land.ConquerFleet == null && land.RiotIndicator == 0) continue;
                if (land.ConquerFleet == null)
                { land.RiotIndicator -= 1; land.Player.CreateCryptLand(); land.Player.CreateArrayForPlayer(); }
                else land.RiotIndicator += 5;
                if (land.RiotIndicator >= 100)
                    land.ConquerLand();
            }
            foreach (ServerAvanpost avanpost in ServerLinks.Avanposts.Values)
            {
                if (avanpost.ConquerFleet == null && avanpost.RiotIndicator == 0) continue;
                if (avanpost.ConquerFleet == null)
                { avanpost.RiotIndicator -= 1; avanpost.Player.CreateNewLandsArray(); avanpost.Player.CreateArrayForPlayer(); }
                else avanpost.RiotIndicator += 10;
                if (avanpost.RiotIndicator >= 100) avanpost.ConquerLand();
            }
            ServerLinks.MainPlayer.CreatePlanetInfo();
        }
        static void AddPillageResources()
        {
            foreach (GSPlayer player in ServerLinks.GSPlayersByID.Values)
                foreach (ServerLand land in player.Lands.Values)
                    foreach (ServerFleet fleet in land.Fleets.Values)
                        if (fleet.Target!=null && fleet.Target.Mission==FleetMission.Pillage && fleet.Target.Order!=FleetOrder.Return)
                        {
                            ServerPlanet planet = fleet.Target.GetPlanet();
                            TargetLand target = planet.GetTargetLand();
                            if (target == null || target.GetPlayer() != fleet.Target.TargetPlayer)
                            {
                                fleet.Target.Order = FleetOrder.Return;
                                fleet.CalcFleetReturnTime(true);
                                BattleEvents.FleetFly(fleet);
                                fleet.Target.SelfPlayer.FleetInBattle(fleet);
                            }
                            else
                            {
                                BuildingValues pillage = target.GetPillagedResources();
                                player.Resources.AddResources(pillage);
                            }
                        }

        }
        static void AddResource()
        {
            foreach (GSPlayer player in ServerLinks.GSPlayersByID.Values)
                //if (player.Premium.CheckUpdateTime())
                    player.PassiveRepair();
            foreach (ServerLand land in ServerLinks.GSLands.Values)
            {
                if (land.PillageFleet != null) continue;
                //if (land.Player.Premium.CheckUpdateTime())
                    land.Player.Resources.AddResources(land);
            }
        }
      
        static void ProcessFleetsReturn()
        {
            DateTime Now = ServerLinks.NowTime;
            Queue<ServerFleet> newqueue = new Queue<ServerFleet>();
            while (CurrentFleetQueue.Count != 0)
            {
                ServerFleet fleet = CurrentFleetQueue.Dequeue();
                if (fleet.Target.Destroy == true) continue;
                if (fleet.Target.FreeTime>Now)
                    newqueue.Enqueue(fleet);
                else
                    fleet.FleetBase.Land.Player.FleetReturned(fleet);
            }
            CurrentFleetQueue = newqueue;
        }
     
    }
    public class GlobalAIParams
    {
        public byte AIImageBase;
        public GlobalAIParams (int id, EnemySide side)
        {

        }

        public void SetPlayer(GSPlayer player)
        {

        }
    }
    /*
    public class GlobalAIParams
    {
        enum AILandType { Peace, Battle, HaveRepair, NotHaveRepair }
        enum ResourceAvailable { Low, Middle, High }
        public GSPlayer Player { get; private set; }
        SortedList<int, AILandType> Lands;
        int WarLands = 0;
        int PeaceLands = 0;
        SortedList<SectorTypes, int> SectorCounts;
        Random Rnd;
        public byte AIImageBase;
        EnemySide Enemy;
        AIShipGenerator ShipGenerator;
        SortedList<long, FleetAIInfo> Fleets = new SortedList<long, FleetAIInfo>();
        public GlobalAIParams(int seed, EnemySide type)
        {
            Lands = new SortedList<int, AILandType>();
            SectorCounts = new SortedList<SectorTypes, int>();
            Rnd = new Random(seed);
            Enemy = type;
            switch (Enemy)
            {
                case EnemySide.GreenTeam: AIImageBase = 251; break;
                case EnemySide.Alien: AIImageBase = 253; break;
                case EnemySide.Mercs: AIImageBase = 255; break;
                case EnemySide.Pirate: AIImageBase = 252; break;
                case EnemySide.Techno: AIImageBase = 254; break;
            }
        }
        enum FleetAIType { Scout, Pillage, Defense, Protect }
        class FleetAIInfo
        {
            public long FleetID;
            public int LandID;
            public FleetAIType AIType;
            public FleetAIInfo(ServerFleet fleet)
            {
                FleetID = fleet.ID;
                LandID = fleet.FleetBase.Land.ID;
            }
            public int GetShips()
            {
                ServerFleet fleet = Excists();
                if (fleet == null) return 0;
                return fleet.FleetBase.Range + fleet.Ships.Count - 7;
            }
            public ServerFleet Excists()
            {
                if (ServerLinks.GSLands.ContainsKey(LandID) == false) return null;
                ServerLand land = ServerLinks.GSLands[LandID];
                if (land.Fleets.ContainsKey(FleetID) == false) return null;
                return land.Fleets[FleetID];
            }
        }
        public void SetPlayer(GSPlayer player)
        {
            Player = player;
            ShipGenerator = new AIShipGenerator(Player);
        }
        /// <summary> Метод рассчитывает бонус, получаемый аи игроком в начале хода. Является компенсацией за невыполнение игровых миссий </summary>
        public Reward2 GetBonusReward()
        {
            int difsum = 0;
            foreach (byte val in ServerLinks.MainPlayer.Mission2Dificulties.Values)
                difsum += val;
            Reward2 reward = new Reward2();
            reward.Money = 1000000;
            reward.Metall = 1000000;
            reward.Chips = 1000000;
            reward.Anti = 1000000;
            //reward.Money = (int)(1000 * Math.Pow(difsum + 1, 2) * 2);
            //reward.Metall = (int)(200 * Math.Pow(difsum + 1, 2) * 2);
            //reward.Chips = (int)(200 * Math.Pow(difsum + 1, 2) * 2);
            //reward.Anti = (int)(200 * Math.Pow(difsum + 1, 2) * 2);
            return reward;
        }
        public void PutShipsToFleets()
        {
            if (Rnd.Next(50) != 0) return;
            if (Player.Ships.Count == 0) return;
            int curshippos = 0;
            foreach (ServerLand land in Player.Lands.Values)
            {
                foreach (ServerFleet fleet in land.Fleets.Values)
                {
                    bool error = false;
                    //for (int i=curshippos;i<)
                    if (fleet.Ships.Count >= 15) continue;
                    if (fleet.GetMaxShips() == fleet.Ships.Count) continue;
                    if (fleet.FleetBase.Range <= 7) continue;
                    
                }
            }
        }
        public void CreateShips()
        {
            int needships = 0;
            if (Rnd.Next(50) != 0) return;
            foreach (FleetAIInfo info in Fleets.Values)
                needships += info.GetShips();
            if (needships <= Player.Ships.Count) return;
            ItemPrice MaxPrice = new ItemPrice(Player.Resources.IntMoney, Player.Resources._Metals, Player.Resources._Chips, Player.Resources._Anti);
            for (int i = 0; i < 10;)
            {
                if (MaxPrice.Money < 10000) return;
                ShipGenerator2Types shiptype = (ShipGenerator2Types)Rnd.Next(10);
                switch (Enemy)
                {
                    case EnemySide.Pirate: if (shiptype == ShipGenerator2Types.Frigate || shiptype == ShipGenerator2Types.Warrior) continue; else break;
                    case EnemySide.GreenTeam: if (shiptype == ShipGenerator2Types.Scout || shiptype == ShipGenerator2Types.Fighter) continue; else break;
                    case EnemySide.Techno: if (shiptype == ShipGenerator2Types.Devostator || shiptype == ShipGenerator2Types.Dreadnought) continue; else break;
                    case EnemySide.Mercs: if (shiptype == ShipGenerator2Types.Cruiser || shiptype == ShipGenerator2Types.Linkor) continue; else break;

                }
                ServerShip ship = ShipGenerator.GetEnemyShip(shiptype, Enemy, MaxPrice);
                if (ship == null) i++;
                else
                {
                    MaxPrice.Remove(ship.Schema.Price);
                    //Links.Controller.mainWindow.Title = String.Format("{0} Ships {1}/{2}", Player.Name.ToString(), Player.Ships.Count, needships);
                    if (needships <= Player.Ships.Count) return;
                }
                
            }
        }
        public void MakeResearches()
        {
            if (Rnd.Next(5) != 0) return;
            int percent = Rnd.Next(10, 50);
            double totalmoney = Player.Resources._Money / 100 * percent;
            double totalmetal = Player.Resources._Metals / 100 * percent;
            double totalchips = Player.Resources._Chips / 100 * percent;
            double totalanti = Player.Resources._Anti / 100 * percent;
            for (;;)
            {
                EScienceField result = EScienceField.None;
                double MinVal = 5;
                ItemPrice MinPrice = new ItemPrice();
                for (int i = 0; i < 4; i++)
                {
                    ItemPrice p = SciencePrice.GetSciencePrice(Player, (EScienceField)i);
                    if (p.Money > totalmoney || p.Metall > totalmetal || p.Chips > totalchips || p.Anti > totalanti)
                        continue;
                    double d = p.Money / totalmoney + p.Metall / totalmoney + p.Chips / totalchips + p.Anti / totalanti;
                    if (MinVal > d) { MinVal = d; result = (EScienceField)i; MinPrice = p; }
                }
                if (result != EScienceField.None)
                {
                    //c++; Links.Controller.mainWindow.Title = c.ToString();
                    totalmoney -= MinPrice.Money; totalmetal -= MinPrice.Metall; totalchips -= MinPrice.Chips; totalanti -= MinPrice.Anti;
                    Player.LearnNewScience(BitConverter.GetBytes((int)result)); continue;

                }
                else break;
            }
        }
        public void BuildInAvanposts()
        {
            if (Player.NewLands.Count == 0) return;
            //if (Rnd.Next(50) != 0) return;
            long totalmoney = Player.Resources._Money / 100 * Rnd.Next(10, 30);
            int totalmetal = Player.Resources._Metals / 100 * Rnd.Next(10, 30);
            int totalchips = Player.Resources._Chips / 100 * Rnd.Next(10, 30);
            int totalanti = Player.Resources._Anti / 100 * Rnd.Next(10, 30);
            for (;;)
            {
                bool restart = false;
                foreach (ServerAvanpost avanpost in Player.NewLands.Values)
                {
                    int spentmoney = 0; int spentmetall = 0; int spentchips = 0; int spentanti = 0;
                    if (totalmoney > 0)
                    {
                        int needmoney = avanpost.NeedResources.Money - avanpost.HaveResources.Money;
                        if (needmoney > 0 && needmoney > totalmoney) { spentmoney = (int)totalmoney; totalmoney = 0; }
                        else if (needmoney > 0 && needmoney <= totalmoney) { spentmoney = needmoney; totalmoney -= needmoney; }
                    }
                    if (totalmetal > 0)
                    {
                        int needmetal = avanpost.NeedResources.Metall - avanpost.HaveResources.Metall;
                        if (needmetal > 0 && needmetal > totalmetal) { spentmetall = totalmetal; totalmetal = 0; }
                        else if (needmetal > 0 && needmetal <= totalmetal) { spentmetall = needmetal; totalmetal -= needmetal; }
                    }
                    if (totalchips > 0)
                    {
                        int needchips = avanpost.NeedResources.Chips - avanpost.HaveResources.Chips;
                        if (needchips > 0 && needchips > totalchips) { spentchips = totalchips; totalchips = 0; }
                        else if (needchips > 0 && needchips <= totalchips) { spentchips = needchips; totalchips -= needchips; }
                    }
                    if (totalanti > 0)
                    {
                        int needanti = avanpost.NeedResources.Anti - avanpost.HaveResources.Anti;
                        if (needanti > 0 && needanti > totalanti) { spentanti = totalanti; totalanti = 0; }
                        else if (needanti > 0 && needanti <= totalanti) { spentanti = needanti; totalanti -= needanti; }
                    }
                    if (spentmoney > 0 || spentmetall > 0 || spentchips > 0 || spentanti > 0)
                    {
                        Player.BuildInNewColony(avanpost.ID, new ItemPrice(spentmoney, spentmetall, spentchips, spentanti));
                        if (Player.NewLands.ContainsKey(avanpost.ID) == false)
                        {
                            restart = true; break;
                        }

                    }
                }
                if (restart == true) continue; else break;
            }
        }
        public void ExchangeResources()
        {
            ResourceAvailable Metals, Chips, Anti;
            if (Player.Resources._Metals < Player.Resources.Capacity.Metall / 3) Metals = ResourceAvailable.Low;
            else if (Player.Resources._Metals > Player.Resources.Capacity.Metall / 4 * 3) Metals = ResourceAvailable.High;
            else Metals = ResourceAvailable.Middle;
            if (Player.Resources._Chips < Player.Resources.Capacity.Chips / 3) Chips = ResourceAvailable.Low;
            else if (Player.Resources._Chips > Player.Resources.Capacity.Chips / 4 * 3) Chips = ResourceAvailable.High;
            else Chips = ResourceAvailable.Middle;
            if (Player.Resources._Anti < Player.Resources.Capacity.Anti / 3) Anti = ResourceAvailable.Low;
            else if (Player.Resources._Anti > Player.Resources.Capacity.Anti / 4 * 3) Anti = ResourceAvailable.High;
            else Anti = ResourceAvailable.Middle;
            if (Metals == ResourceAvailable.High)
            {
                int count = Player.Resources._Metals - Player.Resources.Capacity.Metall / 3 * 2;
                Player.ResourceExchange(1, count);
            }
            if (Chips == ResourceAvailable.High)
            {
                int count = Player.Resources._Chips - Player.Resources.Capacity.Chips / 3 * 2;
                Player.ResourceExchange(3, count);
            }
            if (Anti == ResourceAvailable.High)
            {
                int count = Player.Resources._Anti - Player.Resources.Capacity.Anti / 3 * 2;
                Player.ResourceExchange(5, count);
            }
            if (Metals == ResourceAvailable.Low && Player.Resources._Money / ServerLinks.Market.BuyMetal > Player.Resources._Metals)
            {
                int count = Player.Resources.Capacity.Metall / 3 - Player.Resources._Metals;
                long price = (long)(ServerLinks.Market.BuyMetal * count);
                if (Player.Resources._Money > price * 2)
                    Player.ResourceExchange(0, count);
                else
                {
                    count = (int)((Player.Resources._Money / 2) / ServerLinks.Market.BuyMetal);
                    Player.ResourceExchange(0, count);
                }
            }
            if (Chips == ResourceAvailable.Low && Player.Resources._Money/ServerLinks.Market.BuyChips>Player.Resources._Chips)
            {
                int count = Player.Resources.Capacity.Chips / 3 - Player.Resources._Chips;
                long price = (long)(ServerLinks.Market.BuyChips * count);
                if (Player.Resources._Money > price * 2)
                    Player.ResourceExchange(2, count);
                else
                {
                    count = (int)((Player.Resources._Money / 2) / ServerLinks.Market.BuyChips);
                    Player.ResourceExchange(2, count);
                }
            }
            if (Anti == ResourceAvailable.Low && Player.Resources._Money/ServerLinks.Market.BuyAnti>Player.Resources._Anti)
            {
                int count = Player.Resources.Capacity.Anti / 3 - Player.Resources._Anti;
                long price = (long)(ServerLinks.Market.BuyAnti * count);
                if (Player.Resources._Money > price * 2)
                    Player.ResourceExchange(4, count);
                else
                {
                    count = (int)((Player.Resources._Money / 2) / ServerLinks.Market.BuyAnti);
                    Player.ResourceExchange(4, count);
                }
            }
        }
        public void ColonizePlanets()
        {
            if (Player.NewLands.Count > 3) return;
            if (Player.Resources._Money < 100000) return;
            //создание новых аванпостов
            List<ServerPlanet> Planets = new List<ServerPlanet>();
            List<ServerStar> SelfStars = new List<ServerStar>();
            //В первую очередь колонизируются те планеты, которые в тех же системах где и свои планеты.
            foreach (ServerLand land in Player.Lands.Values)
            {
                ServerStar star = land.Planet.Star;
                if (SelfStars.Contains(star) == false) SelfStars.Add(star);
            }
            foreach (ServerAvanpost avanpost in Player.NewLands.Values)
            {
                ServerStar star = avanpost.Planet.Star;
                if (SelfStars.Contains(star) == false) SelfStars.Add(star);
            }
            foreach (ServerStar star in SelfStars)
                foreach (ServerPlanet planet in star.Planets.Values)
                {
                    if (planet.MaxPopulation <= 0 || planet.Lands.Count + planet.NewLands.Count > 0 || planet.QuestPlanet==true) continue;
                    Planets.Add(planet);
                }
            foreach (ServerPlanet planet in Planets)
            {
                if (Player.NewLands.Count > 3 || Player.Resources._Money < 100000) return;
                int avanpostid;
                Player.StartColonizeLand(planet.ID, out avanpostid);
            }
            //Затем выбираются ближайшие звёзды
            foreach (ServerStar star in ServerLinks.GSStars.Values)
            {
                if (Player.NewLands.Count > 3 || Player.Resources._Money < 100000) return;
                if (SelfStars.Contains(star)) continue;
                bool haveplanets = false;
                foreach (ServerPlanet planet in star.Planets.Values)
                {
                    if (planet.MaxPopulation > 0 && planet.Lands.Count + planet.NewLands.Count == 0 && planet.QuestPlanet==false) { haveplanets = true; break; }
                }
                if (haveplanets == false) continue;
                bool nearstar = false;
                foreach (ServerStar selfstar in SelfStars)
                {
                    double distance = Math.Sqrt(Math.Pow(star.X - selfstar.X, 2) + Math.Pow(star.Y - selfstar.Y, 2));
                    if (distance < 60) { nearstar = true; break; }
                }
                if (nearstar == false) continue;
                foreach (ServerPlanet planet in star.Planets.Values)
                {
                    if (planet.MaxPopulation > 0 && planet.Lands.Count + planet.NewLands.Count == 0)
                    {
                        int avanpostid;
                        GSError error = Player.StartColonizeLand(planet.ID, out avanpostid);
                        if (error.E2 == 0 && Player.NewLands.Count > 3) return;
                    }
                }
            }
        }
        public void RemoveBuildings()
        {
            foreach (ServerLand land in Player.Lands.Values)
            {
                //убирание зданий прироста, если население максимум
                ServerPeaceSector sector = (ServerPeaceSector)land.Locations[0];
                if (land.Peoples >= land.Planet.MaxPopulation)
                {
                    for (;;)
                    {
                        bool sucsess = false;
                        foreach (KeyValuePair<ushort, ushort> pair in sector.Buildings)
                        {
                            GSBuilding building = ServerLinks.GSBuildings[pair.Key];
                            if (building.Params.Length == 1)
                            {
                                GSError error = Player.DestroyBuilding(land, 0, building, (byte)(pair.Value > 255 ? 255 : pair.Value));
                                if (error.E2 == 6) { sucsess = true; break; }
                            }
                            else
                            {
                                //необходимо убрать постройку с приростом населения, не изменив при этом ёмкость населения
                                if (land.Planet.PlanetType == PlanetTypes.Green)
                                {
                                    GSError error = Player.DestroyBuilding(land, 0, building, (byte)(pair.Value > 255 ? 255 : pair.Value));
                                    if (error.E2 == 6) { sucsess = true; break; }
                                }
                                else
                                {
                                    int capsum = CalcBuildingsCapacity(sector);
                                    if (capsum <= land.Planet.MaxPopulation) continue;
                                    if (capsum - building.Params[0].Value > -land.Planet.MaxPopulation)
                                    {
                                        GSError error = Player.DestroyBuilding(land, 0, building, 1);
                                        if (error.E2 == 6) { sucsess = true; break; }
                                    }
                                }
                            }
                        }
                        if (sucsess == false) return;
                    }
                }
                //убирание зданий населения, если запас больше максимума. Убираем, если прирост при этом не упадёт ниже 2% от максимума
                int capsum2 = CalcBuildingsCapacity(sector);
                if (land.Planet.PlanetType == PlanetTypes.Green || capsum2 > land.Planet.MaxPopulation)
                {
                    for (;;)
                    {
                        bool sucsess = false;
                        foreach (KeyValuePair<ushort, ushort> pair in sector.Buildings)
                        {
                            GSBuilding building = ServerLinks.GSBuildings[pair.Key];
                            if (building.Params.Length == 1) continue;
                            if ((land.Add.Grow - building.Params[1].Value) / 5 > land.Planet.MaxPopulation)
                            {
                                GSError error = Player.DestroyBuilding(land, 0, building, 1);
                                if (error.E2 == 6) { sucsess = true; break; }
                            }
                        }
                        if (sucsess == false) break;
                    }
                }
                //убирание радаров, если кораблей больше 20.
                foreach (ServerLandSector s in land.Locations)
                {
                    if (s.Type != SectorTypes.War) continue;
                    ServerFleetBase fleetbase = (ServerFleetBase)s;
                    if (fleetbase.MaxShips <= 20) continue;
                    for (;;)
                    {
                        bool sucsess = false;
                        foreach (KeyValuePair<ushort, ushort> pair in fleetbase.Buildings)
                        {
                            GSBuilding building = ServerLinks.GSBuildings[pair.Key];
                            if (building.Type != BuildingType.Radar) continue;
                            if (fleetbase.MaxShips - building.Params[0].Value > 20)
                            {
                                GSError error = Player.DestroyBuilding(land, (byte)fleetbase.Position, building, 1);
                                if (error.E2 == 6) { sucsess = true; break; }
                            }
                        }
                        if (sucsess == false) break;
                    }
                }
            }
        }
        public int CalcBuildingsCapacity(ServerPeaceSector sector)
        {
            int result = 10;
            foreach (KeyValuePair<ushort, ushort> pair in sector.Buildings)
            {
                GSBuilding building = ServerLinks.GSBuildings[pair.Key];
                if (building.Params[0].Type != EBuildingParam.People) continue;
                result += building.Params[0].Value * pair.Value;
            }
            return result;
        }
        public void UpgradeBuildings()
        {
            foreach (ServerLand land in Player.Lands.Values)
            {
                foreach (ServerLandSector sector in land.Locations)
                {
                    if (Player.Resources._Money < 10000) return;
                    if (sector.GetSectorType() == LandSectorType.Peace)
                    {
                        ServerPeaceSector peace = (ServerPeaceSector)sector;
                        for (;;)
                        {
                            bool buildresult = false;
                            foreach (KeyValuePair<ushort, ushort> pair in peace.Buildings)
                            {
                                GSBuilding building = ServerLinks.GSBuildings[pair.Key];
                                GSBuilding next = GetUpgradeVersion(building);
                                if (next == null) continue;
                                byte count = 10; ItemPrice price = ItemPrice.GetUpgradePrice(building.Price, next.Price, count);
                                if (count >= pair.Value && Player.Resources.CheckPrice(price) == true) { GSError error = Player.UpgradeBuilding(land, (byte)sector.Position, building, next, count); if (error.E2 == 7) { buildresult = true; break; } }
                                count = 5; price = ItemPrice.GetUpgradePrice(building.Price, next.Price, count);
                                if (count >= pair.Value && Player.Resources.CheckPrice(price) == true) { GSError error = Player.UpgradeBuilding(land, (byte)sector.Position, building, next, count); if (error.E2 == 7) { buildresult = true; break; } }
                                count = 2; price = ItemPrice.GetUpgradePrice(building.Price, next.Price, count);
                                if (count >= pair.Value && Player.Resources.CheckPrice(price) == true) { GSError error = Player.UpgradeBuilding(land, (byte)sector.Position, building, next, count); if (error.E2 == 7) { buildresult = true; break; } }
                                count = 1; price = ItemPrice.GetUpgradePrice(building.Price, next.Price, count);
                                if (count >= pair.Value && Player.Resources.CheckPrice(price) == true) { GSError error = Player.UpgradeBuilding(land, (byte)sector.Position, building, next, count); if (error.E2 == 7) { buildresult = true; break; } }

                            }
                            if (buildresult == false) break;
                        }
                    }
                    else if (sector.GetSectorType() == LandSectorType.War)
                    {
                        ServerFleetBase fleetbase = (ServerFleetBase)sector;
                        if (fleetbase.PortalLevel < 5)
                        {
                            GSBuilding curportal = ServerLinks.GSBuildings[(ushort)((fleetbase.PortalLevel) * 10000 + 100)];
                            GSBuilding nextportal = ServerLinks.GSBuildings[(ushort)((fleetbase.PortalLevel + 1) * 10000 + 100)];
                            Player.UpgradeBuilding(land, (byte)fleetbase.Position, curportal, nextportal, 1);
                        }
                        for (;;)
                        {
                            bool buildresult = false;
                            foreach (KeyValuePair<ushort, ushort> pair in fleetbase.Buildings)
                            {
                                GSBuilding building = ServerLinks.GSBuildings[pair.Key];

                                GSBuilding next = GetUpgradeVersion(building);
                                if (next == null) continue;
                                byte count = 10; ItemPrice price = ItemPrice.GetUpgradePrice(building.Price, next.Price, count);
                                if (count >= pair.Value && Player.Resources.CheckPrice(price) == true) { GSError error = Player.UpgradeBuilding(land, (byte)sector.Position, building, next, count); if (error.E2 == 7) { buildresult = true; break; } }
                                count = 5; price = ItemPrice.GetUpgradePrice(building.Price, next.Price, count);
                                if (count >= pair.Value && Player.Resources.CheckPrice(price) == true) { GSError error = Player.UpgradeBuilding(land, (byte)sector.Position, building, next, count); if (error.E2 == 7) { buildresult = true; break; } }
                                count = 2; price = ItemPrice.GetUpgradePrice(building.Price, next.Price, count);
                                if (count >= pair.Value && Player.Resources.CheckPrice(price) == true) { GSError error = Player.UpgradeBuilding(land, (byte)sector.Position, building, next, count); if (error.E2 == 7) { buildresult = true; break; } }
                                count = 1; price = ItemPrice.GetUpgradePrice(building.Price, next.Price, count);
                                if (count >= pair.Value && Player.Resources.CheckPrice(price) == true) { GSError error = Player.UpgradeBuilding(land, (byte)sector.Position, building, next, count); if (error.E2 == 7) { buildresult = true; break; } }

                            }
                            if (buildresult == false) break;
                        }
                    }
                }
            }
        }
        GSBuilding GetUpgradeVersion(GSBuilding building)
        {
            int curpos = GSBuilding.AllBuildings[building.Type].IndexOfKey(building.Level);
            for (int i = curpos + 1; i < GSBuilding.AllBuildings[building.Type].Count; i++)
            {
                GSBuilding next = GSBuilding.AllBuildings[building.Type].ElementAt(i).Value;
                if (Player.Sciences.SciencesArray.Contains(next.ID) == false) continue;
                return next;
            }
            return null;
        }
        public void BuildBuildings()
        {
            foreach (ServerLand land in Player.Lands.Values)
            {
                if (Player.Resources._Money < 1000) return;
                if (Player.ID == 3)
                    Player.ID = 3;
                for (;;)
                {
                    int freeplaces = (int)(land.Peoples - land.BuildingsCount);
                    if (freeplaces < 1) break;
                    if (land.Peoples >= land.Planet.MaxPopulation && land.Capacity.Peoples >= land.Planet.MaxPopulation) { }
                    else if (land.Capacity.Peoples >= land.Planet.MaxPopulation && land.Add.Grow / 10.0 >= land.Planet.MaxPopulation) { }
                    else if (land.Planet.PlanetType != PlanetTypes.Green && land.Capacity.Peoples - land.Peoples < 20) //строительство зданий расположения населения
                    {
                        GSBuilding capacitybuilding = GetAvilableBuilding(EBuildingParam.People, SectorTypes.Live);
                        if (capacitybuilding != null)
                        {
                            GSError error = Player.BuildBuilding(land, 0, capacitybuilding, 1);
                            if (error.E1 == 1 && error.E2 == 0) { continue; }
                        }
                    }
                    else if (land.Add.Grow / 10.0 < land.Planet.MaxPopulation) //Строительство зданий прироста населения, если прирост менее 1% от максимума
                    {
                        GSBuilding growbuilding = GetAvilableBuilding(EBuildingParam.Grow, SectorTypes.Live);
                        if (growbuilding != null)
                        {
                            GSError error = Player.BuildBuilding(land, 0, growbuilding, 1);
                            if (error.E1 == 1 && error.E2 == 0) { continue; }
                        }
                    }
                    int sectorbuildings = (int)((land.Peoples - land.Locations[0].BuildingsCount) / (land.Locations.Length - 1));
                    bool sucsess = false;
                    foreach (ServerLandSector sector in land.Locations)
                    {
                        if (Player.Resources._Money < 1000) return;
                        if (sector.Type == SectorTypes.Live) continue;
                        if (sector.BuildingsCount > sectorbuildings) continue;
                        GSBuilding building = null;
                        int val;
                        switch (sector.Type)
                        {
                            case SectorTypes.Money: val = Rnd.Next(3);
                                if (val == 2)
                                    building = GetAvilableBuilding(EBuildingParam.MoneyMult, SectorTypes.Money);
                                else
                                    building = GetAvilableBuilding(EBuildingParam.MoneyAdd, SectorTypes.Money);
                                break;
                            case SectorTypes.Metall: val = Rnd.Next(6);
                                if (val < 3)
                                    building = GetAvilableBuilding(EBuildingParam.MetallAdd, SectorTypes.Metall);
                                else if (val < 5)
                                    building = GetAvilableBuilding(EBuildingParam.MetalMult, SectorTypes.Metall);
                                else
                                    building = GetAvilableBuilding(EBuildingParam.MetalCap, SectorTypes.Metall);
                                break;
                            case SectorTypes.MetalCap: val = Rnd.Next(6);
                                if (val < 3)
                                    building = GetAvilableBuilding(EBuildingParam.MetalCap, SectorTypes.MetalCap);
                                else if (val < 5)
                                    building = GetAvilableBuilding(EBuildingParam.MetalCapMult, SectorTypes.MetalCap);
                                else
                                    building = GetAvilableBuilding(EBuildingParam.MetallAdd, SectorTypes.MetalCap);
                                break;
                            case SectorTypes.Chips: val = Rnd.Next(6);
                                if (val < 3)
                                    building = GetAvilableBuilding(EBuildingParam.ChipsAdd, SectorTypes.Chips);
                                else if (val < 5)
                                    building = GetAvilableBuilding(EBuildingParam.ChipMult, SectorTypes.Chips);
                                else
                                    building = GetAvilableBuilding(EBuildingParam.ChipCap, SectorTypes.Chips);
                                break;
                            case SectorTypes.ChipsCap: val = Rnd.Next(6);
                                if (val < 3)
                                    building = GetAvilableBuilding(EBuildingParam.ChipCap, SectorTypes.ChipsCap);
                                else if (val < 5)
                                    building = GetAvilableBuilding(EBuildingParam.ChipCapMult, SectorTypes.ChipsCap);
                                else
                                    building = GetAvilableBuilding(EBuildingParam.ChipsAdd, SectorTypes.ChipsCap);
                                break;
                            case SectorTypes.Anti: val = Rnd.Next(6);
                                if (val < 3)
                                    building = GetAvilableBuilding(EBuildingParam.AntiAdd, SectorTypes.Anti);
                                else if (val < 5)
                                    building = GetAvilableBuilding(EBuildingParam.AntiMult, SectorTypes.Anti);
                                else
                                    building = GetAvilableBuilding(EBuildingParam.AntiCap, SectorTypes.Anti);
                                break;
                            case SectorTypes.AntiCap: val = Rnd.Next(6);
                                if (val < 3)
                                    building = GetAvilableBuilding(EBuildingParam.AntiCap, SectorTypes.AntiCap);
                                else if (val < 5)
                                    building = GetAvilableBuilding(EBuildingParam.AntiCapMult, SectorTypes.AntiCap);
                                else
                                    building = GetAvilableBuilding(EBuildingParam.AntiAdd, SectorTypes.AntiCap);
                                break;
                            case SectorTypes.Repair: val = Rnd.Next(3);
                                if (val < 2)
                                    building = GetAvilableBuilding(EBuildingParam.RepairPower, SectorTypes.Repair);
                                else
                                    building = GetAvilableBuilding(EBuildingParam.RepairMult, SectorTypes.Repair);
                                break;
                            case SectorTypes.War: val = Rnd.Next(5);
                                if (val < 2)
                                    building = GetAvilableBuilding(EBuildingParam.Ships, SectorTypes.War);
                                else if (val < 4)
                                    building = GetAvilableBuilding(EBuildingParam.MathPower, SectorTypes.War);
                                else
                                    building = GetAvilableBuilding(EBuildingParam.Education, SectorTypes.War);
                                break;
                        }
                        if (building != null)
                        {
                            GSError error = Player.BuildBuilding(land, (byte)sector.Position, building, 1);
                            if (error.E2 == 0)
                            {
                                sucsess = true; break;
                            }
                        }
                    }
                    if (sucsess == true) continue;

                    break;
                }
            }
        }
        public GSBuilding GetAvilableBuilding(EBuildingParam param, SectorTypes sector)
        {
            SortedList<GSBuilding, int> AllBuildings = new SortedList<GSBuilding, int>();
            foreach (GSBuilding building in ServerLinks.GSBuildings.Values)
            {
                if (building.Sector != sector) continue;
                if (Player.Sciences.SciencesArray.Contains(building.ID) == false) continue;
                if (building.Params.Length > 0 && building.Params[0].Type == param) AllBuildings.Add(building, building.Params[0].Value);
                else if (building.Params.Length > 1 && building.Params[1].Type == param) AllBuildings.Add(building, building.Params[1].Value);
            }
            if (AllBuildings.Count == 0) return null;
            int maxvalue = 0;
            GSBuilding result = null;
            foreach (KeyValuePair<GSBuilding, int> pair in AllBuildings)
                if (pair.Value > maxvalue) { maxvalue = pair.Value; result = pair.Key; }
            return result;
        }
        void CalcSectors()
        {
            SectorCounts = new SortedList<SectorTypes, int>();
            SectorCounts.Add(SectorTypes.Anti, 0); SectorCounts.Add(SectorTypes.AntiCap, 0); SectorCounts.Add(SectorTypes.Chips, 0);
            SectorCounts.Add(SectorTypes.ChipsCap, 0); SectorCounts.Add(SectorTypes.MetalCap, 0); SectorCounts.Add(SectorTypes.Metall, 0);
            SectorCounts.Add(SectorTypes.Money, 0);
            foreach (ServerLand land in Player.Lands.Values)
                foreach (ServerLandSector sector in land.Locations)
                {
                    switch (sector.Type)
                    {
                        case SectorTypes.War:
                        case SectorTypes.Live:
                        case SectorTypes.Repair:
                        case SectorTypes.Clear:
                            continue;
                        default: SectorCounts[sector.Type]++; break;
                    }
                }
        }
        SectorTypes GetLowSector()
        {
            SectorTypes result = SectorTypes.Money;
            int minval = Int32.MaxValue;
            foreach (KeyValuePair<SectorTypes, int> pair in SectorCounts)
                if (pair.Value < minval)
                { minval = pair.Value; result = pair.Key; }
            return result;
        }
        public void BuildSectors()
        {
            bool calcsecor = false;
            //Добавление новых колоний в перечень и определение их типа
            foreach (ServerLand land in Player.Lands.Values)
            {
                if (Lands.ContainsKey(land.ID)) continue;
                //Определение, является ли эта колония новой, то есть без секторов
                byte clearsectors = 0;
                byte warsectors = 0;
                byte peacesectors = 0;
                bool hasrepair = false;
                foreach (ServerLandSector sector in land.Locations)
                {
                    if (sector.Type == SectorTypes.Clear)
                        clearsectors++;
                    else if (sector.Type == SectorTypes.War || sector.Type == SectorTypes.Repair)
                    {
                        warsectors++;
                        if (sector.Type == SectorTypes.War)
                        {
                            ServerFleetBase fleetbase = (ServerFleetBase)sector;
                            if (fleetbase.Fleet == null)
                            {
                                ServerFleet fleet;
                                Player.CreateFleet(land.ID, (byte)fleetbase.Position, new byte[] { AIImageBase, 0, 0, 0 }, out fleet);
                                if (fleet != null) Fleets.Add(fleet.ID, new FleetAIInfo(fleet));
                            }
                        }
                    }
                    else if (sector.Type != SectorTypes.Live)
                        peacesectors++;
                    if (sector.Type == SectorTypes.Repair)
                        hasrepair = true;
                }
                if (clearsectors == land.Locations.Length - 1) //колония новая
                {
                    if (land.Planet.ID == 22)
                    {
                        Lands.Add(land.ID, AILandType.Battle); WarLands++;
                    }
                    else if (land.Locations.Length < 5 && PeaceLands - WarLands <= 2)
                    {
                        Lands.Add(land.ID, AILandType.Peace); PeaceLands++;
                    }
                    else if (WarLands == PeaceLands)
                    {
                        if (Rnd.Next(2) == 0)
                        { Lands.Add(land.ID, AILandType.Peace); PeaceLands++; }
                        else { Lands.Add(land.ID, AILandType.Battle); WarLands++; }
                    }
                    else if (WarLands > PeaceLands)
                    { Lands.Add(land.ID, AILandType.Peace); PeaceLands++; }
                    else { Lands.Add(land.ID, AILandType.Battle); WarLands++; }
                }
                else if (warsectors > 0 && peacesectors == 0) //колония боевая
                {
                    if (hasrepair == false && clearsectors == 0)
                        Lands.Add(land.ID, AILandType.NotHaveRepair);
                    else
                    { Lands.Add(land.ID, AILandType.Battle); WarLands++; }
                }
                else if (peacesectors > 0 && warsectors == 0) //колония мирная
                {
                    Lands.Add(land.ID, AILandType.Peace); PeaceLands++;
                }
                else if (hasrepair || clearsectors > 0) //колония с ремонтом
                {
                    Lands.Add(land.ID, AILandType.HaveRepair);
                }
                else //колония без ремонта
                {
                    Lands.Add(land.ID, AILandType.NotHaveRepair);
                }
                //Необходимо достроить сектора, в соответствии с типом
                if (calcsecor == false) { CalcSectors(); calcsecor = true; }
                switch (Lands[land.ID])
                {
                    case AILandType.Battle:
                        foreach (ServerLandSector sector in land.Locations)
                            if (sector.Type == SectorTypes.Clear)
                            {
                                if (hasrepair == false)
                                {
                                    Player.BuildSector(land, SectorTypes.Repair, sector.Position); hasrepair = true;
                                }
                                else
                                {
                                    Player.BuildSector(land, SectorTypes.War, sector.Position);
                                    ServerFleet fleet;
                                    Player.CreateFleet(land.ID, (byte)sector.Position, new byte[] { AIImageBase, 0, 0, 0 }, out fleet);
                                    if (fleet != null) Fleets.Add(fleet.ID, new FleetAIInfo(fleet));
                                }
                            }
                        break;
                    case AILandType.Peace:
                    case AILandType.HaveRepair:
                    case AILandType.NotHaveRepair:
                        foreach (ServerLandSector sector in land.Locations)
                            if (sector.Type == SectorTypes.Clear)
                            {
                                SectorTypes type = GetLowSector();
                                Player.BuildSector(land, type, sector.Position);
                                SectorCounts[type]++;
                            }
                        break;
                }
            }
            //Удаление колоний из перечня
            List<int> landstoremove = new List<int>();
            foreach (KeyValuePair<int, AILandType> pair in Lands)
            {
                if (ServerLinks.GSLands.ContainsKey(pair.Key) == true)
                {
                    ServerLand land = ServerLinks.GSLands[pair.Key];
                    if (land.Player == Player) continue;
                }
                if (pair.Value == AILandType.Battle) WarLands--;
                else if (pair.Value == AILandType.Peace) PeaceLands--;
                landstoremove.Add(pair.Key);
            }
            foreach (int key in landstoremove)
                Lands.Remove(key);
            //удаление флотов из перечня
            List<long> FleetsRemove = new List<long>();
            foreach (KeyValuePair<long, FleetAIInfo> pair in Fleets)
                if (pair.Value.Excists() == null) FleetsRemove.Add(pair.Key);
            foreach (long key in FleetsRemove)
                Fleets.Remove(key);
        }
    }
    */
    class AIShipGenerator
    {
        enum WeaponAcc { Low, Middle, High}
        static Random rnd = new Random();
        GSPlayer Player;
        SortedSet<ushort> sciences;

        public AIShipGenerator(GSPlayer player)
        {
            Player = player;
            sciences = Player.Sciences.SciencesArray;
        }
        static SortedList<WeaponGroup, SortedList<WeaponAcc, EWeaponType[]>> GreenTeamWeapons = GetTeamWeapons(new EWeaponType[] { EWeaponType.Solar, EWeaponType.Laser, EWeaponType.Cannon, EWeaponType.Missle, EWeaponType.Warp, EWeaponType.Time, EWeaponType.Slicing, EWeaponType.Magnet });
        static SortedList<WeaponGroup, SortedList<WeaponAcc, EWeaponType[]>> AlienWeapons = GetTeamWeapons(new EWeaponType[] { EWeaponType.Solar, EWeaponType.Laser, EWeaponType.AntiMatter, EWeaponType.Missle, EWeaponType.Dark, EWeaponType.Psi, EWeaponType.Radiation, EWeaponType.Magnet });
        static SortedList<WeaponGroup, SortedList<WeaponAcc, EWeaponType[]>> TechnoWeapons = GetTeamWeapons(new EWeaponType[] { EWeaponType.Solar, EWeaponType.EMI, EWeaponType.AntiMatter, EWeaponType.Gauss, EWeaponType.Warp, EWeaponType.Time, EWeaponType.Radiation, EWeaponType.Magnet });
        static SortedList<WeaponGroup, SortedList<WeaponAcc, EWeaponType[]>> PirateWeapons = GetTeamWeapons(new EWeaponType[] { EWeaponType.Plasma, EWeaponType.Laser, EWeaponType.Cannon, EWeaponType.Missle, EWeaponType.Warp, EWeaponType.Psi, EWeaponType.Slicing, EWeaponType.Drone });
        static SortedList<WeaponGroup, SortedList<WeaponAcc, EWeaponType[]>> MercsWeapons = GetTeamWeapons(new EWeaponType[] { EWeaponType.Plasma, EWeaponType.EMI, EWeaponType.Cannon, EWeaponType.Gauss, EWeaponType.Dark, EWeaponType.Time, EWeaponType.Radiation, EWeaponType.Drone });
        public ServerShip GetEnemyShip(ShipGenerator2Types shiptype, EnemySide enemy, ItemPrice MaxPrice)
        {
            Schema result = new Schema();
            ShipTypeclass model = null;
            foreach (ShipTypeclass stc in ServerLinks.ShipParts.ShipTypes.Values)
            {
                if (sciences.Contains(stc.ID) == false || stc.Type != shiptype) continue;
                if (model == null || model.Level < stc.Level) model = stc;
            }
            if (model == null) return null;
            result.SetShipType(model);
            WeaponGroup group = model.GetWeaponGroup();
            if (group == WeaponGroup.Any)
                group = (WeaponGroup)rnd.Next(4);
            WeaponAcc weaponacc = WeaponAcc.Middle;
            switch (shiptype)
            { case ShipGenerator2Types.Corvett: case ShipGenerator2Types.Frigate: case ShipGenerator2Types.Dreadnought: case ShipGenerator2Types.Devostator: weaponacc = WeaponAcc.High; break;
                case ShipGenerator2Types.Linkor: case ShipGenerator2Types.Cruiser: weaponacc = WeaponAcc.Low; break;
            }
            for (int i = 0; i < model.WeaponCapacity; i++)
            {
                EWeaponType[] types=null;
                switch (enemy)
                {
                    case EnemySide.GreenTeam: types = GreenTeamWeapons[group][weaponacc]; break;
                    case EnemySide.Alien: types = AlienWeapons[group][weaponacc]; break;
                    case EnemySide.Techno: types=TechnoWeapons[group][weaponacc]; break;
                    case EnemySide.Pirate: types=PirateWeapons[group][weaponacc]; break;
                    case EnemySide.Mercs: types=MercsWeapons[group][weaponacc]; break;
                }
                EWeaponType type;
                if (types.Length == 1) type = types[0]; else type = types[rnd.Next(types.Length)];
                Weaponclass weapon = null; int damage = 0;
                foreach (Weaponclass w in ServerLinks.ShipParts.WeaponTypes.Values)
                {
                    if (sciences.Contains(w.ID)==false || w.Type!=type || w.Size>model.WeaponsParam[i].Size) continue;
                    if (w.Damage>damage) { weapon = w;  damage = weapon.Damage; }
                }
                if (weapon == null)
                    return null;
                result.SetWeapon(weapon.ID, i);
            }

            bool compdamage = true;
            switch (shiptype)
            { case ShipGenerator2Types.Corvett: case ShipGenerator2Types.Cargo: case ShipGenerator2Types.Frigate: case ShipGenerator2Types.Dreadnought: compdamage = false; break; }
            Computerclass comp = null; int comppar = 0;
            foreach (Computerclass c in ServerLinks.ShipParts.ComputerTypes.Values)
            {
                if (sciences.Contains(c.ID) == false || (c.group!=WeaponGroup.Any && c.group != group) || c.Size > model.ComputerSize) continue;
                if (compdamage==true && c.MaxDamage >= comppar) { comp = c;  comppar = c.MaxDamage; }
                else if (compdamage==false && c.MaxAccuracy >= comppar) { comp = c;  comppar = c.MaxAccuracy; }
            }
            if (comp == null) return null;
            result.SetComputer(comp);

            bool shieldregen = true; bool shieldsizeprimary = false;
            switch (shiptype)
            {
                case ShipGenerator2Types.Scout: case ShipGenerator2Types.Cargo: case ShipGenerator2Types.Fighter: case ShipGenerator2Types.Warrior: shieldregen = false; break;
                case ShipGenerator2Types.Linkor: case ShipGenerator2Types.Cruiser: shieldregen = false; shieldsizeprimary = true; break;
            }
            Shieldclass shield = null; int shieldpar = 0; ItemSize shieldsize = ItemSize.Small;
            foreach (Shieldclass s in ServerLinks.ShipParts.ShieldTypes.Values)
            {
                if (sciences.Contains(s.ID) == false || s.Size > model.ShieldSize) continue;
                if (shieldsizeprimary && shieldregen == false && s.Size > shieldsize) { shield = s; shieldpar = s.Capacity; shieldsize = s.Size; }
                else if (shieldsizeprimary && shieldregen == false && s.Size==shieldsize&& s.Capacity > shieldpar) { shield = s;  shieldpar = s.Capacity; shieldsize = s.Size; }
                else if (shieldsizeprimary==false && shieldregen==true&& s.Recharge >= shieldpar) { shield = s;  shieldpar = s.Recharge; }
                else if (shieldsizeprimary==false && shieldregen==false && s.Capacity >= shieldpar) { shield = s;  shieldpar = s.Capacity; }  
            }
            if (shield == null) return null;
            result.SetShield(shield);
            Engineclass engine = null; int engineeva = 0; ItemSize enginesize = ItemSize.Small;
            foreach (Engineclass e in ServerLinks.ShipParts.EngineTypes.Values)
            {
                if (sciences.Contains(e.ID) == false || e.Size > model.EngineSize) continue;
                if (e.Size > enginesize) { engine = e;  engineeva = e.AverageEvasion; enginesize = e.Size; }
                else if (e.Size==enginesize && e.AverageEvasion > engineeva) { engine = e;  engineeva = e.AverageEvasion; enginesize = e.Size; }
            }
            if (engine == null) return null;
            result.SetEngine(engine);
            Generatorclass generator = null; int energygen = 0; 
            foreach (Generatorclass g in ServerLinks.ShipParts.GeneratorTypes.Values)
            {
                if (sciences.Contains(g.ID) == false || g.Size > model.GeneratorSize) continue;
                if (g.Recharge > energygen) { generator = g;  energygen = g.Recharge; }
            }
            if (generator == null) return null;
            result.SetGenerator(generator);
            energygen = energygen - comp.Consume - shield.Consume - engine.Consume;
            int shootcost = 0;
            int hextomove = 1;
            switch (shiptype)
            {
                case ShipGenerator2Types.Scout: hextomove = 4; shootcost = result.Weapons[0].Consume; break;
                case ShipGenerator2Types.Cargo:
                case ShipGenerator2Types.Corvett:
                    shootcost = result.Weapons[0].Consume; break;
                case ShipGenerator2Types.Linkor:
                case ShipGenerator2Types.Frigate:
                case ShipGenerator2Types.Dreadnought:
                    shootcost = result.Weapons[0].Consume + result.Weapons[1].Consume; break;
                case ShipGenerator2Types.Fighter:
                    int maxconsumepos = result.Weapons[0].Consume > result.Weapons[1].Consume ? 0 : 1;
                    shootcost = result.Weapons[maxconsumepos].Consume + result.Weapons[1 - maxconsumepos].Consume / 2;
                    hextomove = 4;
                    break;
                case ShipGenerator2Types.Devostator:
                case ShipGenerator2Types.Cruiser:
                    int minconsumepos = 0;
                    for (int i = 1; i < 3; i++)
                        if (result.Weapons[i].Consume < result.Weapons[minconsumepos].Consume) minconsumepos = i;
                    for (int i = 0; i < 3; i++)
                        if (i != minconsumepos) shootcost = result.Weapons[i].Consume;
                    break;
                case ShipGenerator2Types.Warrior:
                    int mincomsumepos = 0;
                    for (int i = 1; i < 3; i++)
                        if (result.Weapons[i].Consume < result.Weapons[mincomsumepos].Consume) mincomsumepos = i;
                    for (int i = 0; i < 3; i++)
                        if (i != mincomsumepos) shootcost = result.Weapons[i].Consume;
                    hextomove = 2;
                    break;
            }
            int enginemove = engine.GetHexMoveSpent;
            int basicrecharge = generator.Recharge;
            for (int i=0;i<model.EquipmentCapacity;i++)
            {
                int energyneed = shootcost + basicrecharge * enginemove / 100 * hextomove;
                if (energyneed>energygen) //выбираем модуль регенерации энергии
                {
                    Equipmentclass equip = null; int equipvalue = 0;
                    foreach (Equipmentclass eq in ServerLinks.ShipParts.EquipmentTypes.Values)
                    {
                        if (eq.EqType != EquipmentType.EnergyRegen || sciences.Contains(eq.ID) == false || model.EquipmentsSize[i] < eq.Size) continue;
                        if (model.EquipmentsSize[i] == ItemSize.Large && eq.Size != ItemSize.Large) continue;
                        if (eq.Value>equipvalue)
                        {
                            equip = eq; equipvalue = eq.Value;
                        }
                    }
                    if (equip == null) return null;
                    result.SetEquipment(equip, i);
                    basicrecharge += equip.Value; energygen += equip.Value;
                }
                else //выбираем иные модули
                {
                    //Если модуль большой - то выбираем из особенного перечня
                    if (model.EquipmentsSize[i] == ItemSize.Large)
                    {
                       for (int j=0;j<10;j++)
                        {
                            EquipmentType eqt = BigItemsList[rnd.Next(BigItemsList.Length)];
                            Equipmentclass equip = null; int equipvalue = 0;
                            foreach (Equipmentclass eq in ServerLinks.ShipParts.EquipmentTypes.Values)
                            {
                                if (eq.EqType != eqt || eq.Size != ItemSize.Large || sciences.Contains(eq.ID) == false) continue;
                                if (equipvalue<eq.Value) { equip = eq;  equipvalue = eq.Value; }
                            }
                            if (equip == null) continue;
                            result.SetEquipment(equip, i);
                            if (equip.EqType == EquipmentType.EnergyRegen) basicrecharge += equip.Value;
                            energygen -= equip.Consume;
                            break;
                        }
                        if (result.Equipments[i] == null) return null;
                    }
                    else //Если модуль не большой - то выбираем специализированный
                    {
                        EquipmentEffect[] EquipList = new EquipmentEffect[0];
                        switch (shiptype)
                        {
                            case ShipGenerator2Types.Scout: EquipList = new EquipmentEffect[] { EquipmentEffect.Evasion, EquipmentEffect.Health }; break;
                            case ShipGenerator2Types.Corvett: EquipList = new EquipmentEffect[] { EquipmentEffect.EnergyCap, EquipmentEffect.Accuracy }; break;
                            case ShipGenerator2Types.Cargo: EquipList = new EquipmentEffect[] { EquipmentEffect.ShieldCap, EquipmentEffect.ShieldRes, EquipmentEffect.Health, EquipmentEffect.Accuracy }; break;
                            case ShipGenerator2Types.Linkor: EquipList = new EquipmentEffect[] { EquipmentEffect.ShieldRes, EquipmentEffect.ShieldCap, EquipmentEffect.Health, EquipmentEffect.Ignore, EquipmentEffect.Immune }; break;
                            case ShipGenerator2Types.Frigate: EquipList = new EquipmentEffect[] { EquipmentEffect.EnergyCap, EquipmentEffect.Accuracy }; break;
                            case ShipGenerator2Types.Fighter: EquipList = new EquipmentEffect[] { EquipmentEffect.Evasion, EquipmentEffect.Damage }; break;
                            case ShipGenerator2Types.Dreadnought: EquipList = new EquipmentEffect[] { EquipmentEffect.Damage, EquipmentEffect.Health, EquipmentEffect.ShieldCap, EquipmentEffect.ShieldRes }; break;
                            case ShipGenerator2Types.Devostator: EquipList = new EquipmentEffect[] { EquipmentEffect.Damage, EquipmentEffect.Accuracy }; break;
                            case ShipGenerator2Types.Warrior: EquipList = new EquipmentEffect[] { EquipmentEffect.ShieldCap, EquipmentEffect.ShieldRes, EquipmentEffect.Ignore, EquipmentEffect.Damage }; break;
                            case ShipGenerator2Types.Cruiser: EquipList = new EquipmentEffect[] { EquipmentEffect.ShieldRes, EquipmentEffect.ShieldCap, EquipmentEffect.Health, EquipmentEffect.Ignore, EquipmentEffect.Immune }; break;
                        }
                        for (int j = 0; j < 15; j++)
                        {
                            EquipmentEffect CurEffect = EquipList[rnd.Next(EquipList.Length)];
                            SortedSet<EquipmentType> Set = Equipmentclass.CheckCompartibility(CurEffect, group);
                            Equipmentclass equip = null; int equiplvl = -1;
                            foreach (Equipmentclass eq in ServerLinks.ShipParts.EquipmentTypes.Values)
                            {
                                if (Set.Contains(eq.EqType) == false || sciences.Contains(eq.ID) == false || eq.Size > model.EquipmentsSize[i]) continue;
                                if (eq.Level > equiplvl) { equip = eq; equiplvl = eq.Level; }
                            }
                            if (equip == null) continue;
                            result.SetEquipment(equip, i);
                            if (equip.EqType == EquipmentType.EnergyRegen) basicrecharge += equip.Value;
                            energygen -= equip.Consume;
                            break;
                        }
                        if (result.Equipments[i] == null) return null;
                    }
                }
            }
            result.CalcPrice();
            //if (result.Price.CheckPrice(MaxPrice) == false) return null;
            int armors=1;
            switch (shiptype)
            { case ShipGenerator2Types.Scout:
                case ShipGenerator2Types.Cargo:
                case ShipGenerator2Types.Dreadnought:
                case ShipGenerator2Types.Warrior: armors = 2; break;
                case ShipGenerator2Types.Linkor:
                case ShipGenerator2Types.Cruiser: armors = 4; break;
            }
            switch (armors)
            {
                case 1: 
                    switch (group)
                    {
                        case WeaponGroup.Energy: result.Armor = ArmorType.Reflect; break;
                        case WeaponGroup.Physic: result.Armor = ArmorType.Composite; break;
                        case WeaponGroup.Irregular: result.Armor = ArmorType.Anomal; break;
                        case WeaponGroup.Cyber: result.Armor = ArmorType.Cybernetic; break;
                 }break;
                case 2:
                    switch (group)
                    {
                        case WeaponGroup.Energy: result.Armor = ArmorType.Traditional; break;
                        case WeaponGroup.Physic: result.Armor = ArmorType.Traditional; break;
                        case WeaponGroup.Irregular: result.Armor = ArmorType.Heavy; break;
                        case WeaponGroup.Cyber: result.Armor = ArmorType.Energy; break;
                    }break;
                case 3:
                    result.Armor = ArmorType.Balanced; break;
            }
            byte[] image = new byte[4];
            image[0] = Player.GlobalAIParams.AIImageBase;
            image[1] = (byte)rnd.Next(3);
            long shipid;
            GSError error = Player.BuildShip(result, image, out shipid);
            if (error.E2 != 0) return null;
            c++;
            float level=result.GetSchemaLevel();
            //Links.Controller.mainWindow.Title = "Ship Price=" + result.Price.ToString()+" "+Player.Name.ToString();
            return Player.Ships[shipid];
        }
        static int c = 0;
        static EquipmentType[] BigItemsList = new EquipmentType[] {EquipmentType.AccAl, EquipmentType.DamAl, EquipmentType.EvaEn, EquipmentType.EvaPh,
                        EquipmentType.EvaIr, EquipmentType.EvaCy, EquipmentType.EvaAl, EquipmentType.Hull, EquipmentType.HullRegen, EquipmentType.Shield, EquipmentType.ShieldRegen, EquipmentType.Energy,
                        EquipmentType.EnergyRegen, EquipmentType.IgnEn, EquipmentType.IgnPh, EquipmentType.IgnIr, EquipmentType.IgnCy, EquipmentType.IgnAl, EquipmentType.ImmEn, EquipmentType.ImmPh,
                        EquipmentType.ImmIr, EquipmentType.ImmCy,EquipmentType.ImmAl };
        static SortedList<WeaponGroup, SortedList<WeaponAcc, EWeaponType[]>> GetTeamWeapons(EWeaponType[] arr)
        {
            SortedList<WeaponGroup, SortedList<WeaponAcc, EWeaponType[]>> result = new SortedList<WeaponGroup, SortedList<WeaponAcc, EWeaponType[]>>();
            result.Add(WeaponGroup.Energy, new SortedList<WeaponAcc, EWeaponType[]>());
            result[WeaponGroup.Energy].Add(WeaponAcc.Low, new EWeaponType[] { arr[0] });
            result[WeaponGroup.Energy].Add(WeaponAcc.Middle, new EWeaponType[] { arr[0], arr[1] });
            result[WeaponGroup.Energy].Add(WeaponAcc.High, new EWeaponType[] { arr[1] });
            result.Add(WeaponGroup.Physic, new SortedList<WeaponAcc, EWeaponType[]>());
            result[WeaponGroup.Physic].Add(WeaponAcc.Low, new EWeaponType[] { arr[2]});
            result[WeaponGroup.Physic].Add(WeaponAcc.Middle, new EWeaponType[] { arr[2], arr[3] });
            result[WeaponGroup.Physic].Add(WeaponAcc.High, new EWeaponType[] { arr[3] });
            result.Add(WeaponGroup.Irregular, new SortedList<WeaponAcc, EWeaponType[]>());
            result[WeaponGroup.Irregular].Add(WeaponAcc.Low, new EWeaponType[] { arr[4] });
            result[WeaponGroup.Irregular].Add(WeaponAcc.Middle, new EWeaponType[] { arr[4], arr[5] });
            result[WeaponGroup.Irregular].Add(WeaponAcc.High, new EWeaponType[] {arr[5]});
            result.Add(WeaponGroup.Cyber, new SortedList<WeaponAcc, EWeaponType[]>());
            result[WeaponGroup.Cyber].Add(WeaponAcc.Low, new EWeaponType[] { arr[6] });
            result[WeaponGroup.Cyber].Add(WeaponAcc.Middle, new EWeaponType[] { arr[6], arr[7] });
            result[WeaponGroup.Cyber].Add(WeaponAcc.High, new EWeaponType[] { arr[7] });
            return result;
        }
    }
}
