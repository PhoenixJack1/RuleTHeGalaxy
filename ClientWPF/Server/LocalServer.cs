using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class LocalServer
    {
        //public static DateTime NowTime = new DateTime(2317, 1, 1, 0, 0, 0);
        public static void LoadSingleGame()
        {
           /* GSPlayer.Prepare();
            PlayerSciencesList.SciencePreparation();
            FillNewLandsSciences();
            ServerLinks.ShipParts.Update();
            GSPlayer.CreateBasicShipsSchamas();
            SciencePrice.CalculateBases();
            GSPlayer.LoadSingleGame();*/
        }
        /// <summary>метод находит координаты для звезды. 2 варианта - либо в диапазоне minx, maxx, miny, maxy
        ///, либо на расстоянии равном r от targetstar. Обязательное условие - не ближе d от других звёзд  </summary> 
       static double[] GetRandomStarCoords(int minx, int miny, int maxx, int maxy, int d, int targetstar, int r, Random rnd)
        {
            for (int i = 0; i < 1000; i++)
            {
                double[] result = new double[2];
                if (targetstar >= 0)
                {
                    double alpha = rnd.Next(0, 360);
                    double sina = Math.Sin(alpha / 180 * Math.PI) * r;
                    double cosa = Math.Cos(alpha / 180 * Math.PI) * r;
                    ServerStar target = ServerLinks.GSStars[targetstar];
                    result[0] = target.X + sina; result[1] = target.Y + cosa;
                    if (result[0] < minx || result[0] > maxx || result[1] < miny || result[1] > maxy) continue;
                }
                else
                {
                    result[0] = rnd.Next(minx, maxx); result[1] = rnd.Next(miny, maxy);
                }
                bool error = false;
                foreach (ServerStar star in ServerLinks.GSStars.Values)
                {
                    double curd = Math.Sqrt(Math.Pow(star.X - result[0], 2) + Math.Pow(star.Y - result[1], 2));
                    if (curd < d) { error = true; break; }
                }
                if (error == true) continue;
                else return result;
            }
            return null;
        }
        public static void StartSingleGame()
        {
            GSPlayer.Prepare();
            PlayerSciencesList.SciencePreparation();
            FillNewLandsSciences();
            ServerLinks.Market = new GSMarket();
            ServerLinks.ShipParts.Update();
            //GSPlayer.CreateBasicShipsSchamas();
            //SciencePrice.CalculateBases();
            ServerLinks.NowTime = new DateTime(2317, 1, 1, 0, 0, 0);
            string galaxyfile = System.IO.File.ReadAllText("Galaxy.txt", Encoding.GetEncoding(1251));
            string[] starnames = System.IO.File.ReadAllLines("StarNames.txt");
            for (;;)
            {
                rnd = new Random();
                //Генерация галактики
                //Считывание параметров галактики из файла
                
                SortedList<int, TextPosition> StarNamesPositions = new SortedList<int, TextPosition>();
                foreach (String p in starnames)
                {
                    TextPosition pos = new TextPosition(p);
                    StarNamesPositions.Add(pos.IntValues[0], pos);
                }
                //Расшифровка размеров галактики и расстояния генерации звёзд
                TextPosition[] GalaxySizes = TextPosition.GetPositions(Common.GetTaggedValue(galaxyfile, "GALAXYSIZE"));
                int GalaxyWidth = 0; int GalaxyHeight = 0; int RandomDistance = 0;
                foreach (TextPosition pos in GalaxySizes)
                    switch (pos.Title)
                    {
                        case "W": GalaxyWidth = pos.IntValues[0]; break;
                        case "H": GalaxyHeight = pos.IntValues[0]; break;
                        case "D": RandomDistance = pos.IntValues[0]; break;
                    }
                Links.GalaxyWidth = GalaxyWidth; Links.GalaxyHeight = GalaxyHeight;
                //считывание и расстановка звёзд с фиксированным координатами
                TextPosition[] FixedStars = TextPosition.GetPositions(Common.GetTaggedValue(galaxyfile, "FIXEDSTARS"));
                ServerLinks.GSStars = new SortedList<int, ServerStar>();
                foreach (TextPosition pos in FixedStars)
                {
                    int id = pos.IntValues[0];
                    TextPosition star = StarNamesPositions[id];
                    ServerLinks.GSStars.Add(id, new ServerStar(id, new GSString(star.StringValues[0]), new GSString(star.StringValues[1]), pos.IntValues[1],
                        pos.IntValues[2], (EStarClass)star.IntValues[2], (byte)star.IntValues[1], star.IntValues[3]));
                    StarNamesPositions.Remove(id);
                }
                //считывание и расстановка групп звёзд
                SortedSet<int> GlobalFreeLocs = new SortedSet<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
                bool groupstarresult = true;
                for (int i = 1; i < 10; i++)
                {
                    string text = Common.GetTaggedValue(galaxyfile, String.Format("GROUP{0}", i));
                    if (text == "") break;
                    TextPosition[] grouppositions = TextPosition.GetPositions(text);
                    int[] locs = null; int mainstar = -1;
                    SortedList<int, int[]> GroupStars = new SortedList<int, int[]>();
                    foreach (TextPosition pos in grouppositions)
                    {
                        switch (pos.Title)
                        {
                            case "LOC": locs = pos.IntValues; break; //перчень возможных регионов для главной звезды
                            case "MS": mainstar = pos.IntValues[0]; break; //главная звезда
                            case "S1": GroupStars.Add(1, pos.IntValues); break;
                            case "S2": GroupStars.Add(2, pos.IntValues); break;
                            case "S3": GroupStars.Add(3, pos.IntValues); break;
                            case "S4": GroupStars.Add(4, pos.IntValues); break;
                            case "S5": GroupStars.Add(5, pos.IntValues); break;
                            case "S6": GroupStars.Add(6, pos.IntValues); break;
                            case "S7": GroupStars.Add(7, pos.IntValues); break;
                            case "S8": GroupStars.Add(8, pos.IntValues); break;
                            case "S9": GroupStars.Add(9, pos.IntValues); break;
                            case "S10": GroupStars.Add(10, pos.IntValues); break;
                        }
                    }
                    if (locs == null || locs.Length == 0) continue;
                    //Определение конкретного региона
                    List<int> curfreelocs = new List<int>();
                    foreach (int j in locs) if (GlobalFreeLocs.Contains(j)) curfreelocs.Add(j);
                    int curloc = curfreelocs[rnd.Next(curfreelocs.Count)];
                    GlobalFreeLocs.Remove(curloc);
                    //расчёт границ региона
                    double minx, miny, maxx, maxy;
                    minx = GalaxyWidth / 3.0 * ((curloc - 1) % 3) - GalaxyWidth / 2; maxx = minx + GalaxyWidth / 3.0; minx += 5; maxx -= 5;
                    miny = GalaxyHeight / 3.0 * ((curloc - 1) / 3) - GalaxyHeight / 2; maxy = miny + GalaxyHeight / 3.0; miny += 5; maxy -= 5;
                    //получение координат центральной звезды региона
                    double[] mainstarcoords = GetRandomStarCoords((int)minx, (int)miny, (int)maxx, (int)maxy, RandomDistance, -1, 0, rnd);
                    if (mainstarcoords == null) { groupstarresult = false;  break; }
                    TextPosition star = StarNamesPositions[mainstar];
                    ServerStar servermainstar = new ServerStar(mainstar, new GSString(star.StringValues[0]), new GSString(star.StringValues[1]),
                        mainstarcoords[0], mainstarcoords[1], (EStarClass)star.IntValues[2], (byte)star.IntValues[1], star.IntValues[3]);
                    ServerLinks.GSStars.Add(servermainstar.ID, servermainstar);
                    StarNamesPositions.Remove(servermainstar.ID);
                    //получение зависимых звёзд
                    bool innerstarsresult = true;
                    foreach (int[] group in GroupStars.Values)
                    {
                        double[] coords = GetRandomStarCoords(-GalaxyWidth / 2 + 5, -GalaxyHeight / 2 + 5, GalaxyWidth / 2 - 5, GalaxyHeight / 2 - 5, RandomDistance, group[1], group[2], rnd);
                        if (coords == null) { innerstarsresult = false;  break; }
                        star = StarNamesPositions[group[0]];
                        ServerStar serverstar = new ServerStar(group[0], new GSString(star.StringValues[0]), new GSString(star.StringValues[1]),
                            coords[0], coords[1], (EStarClass)star.IntValues[2], (byte)star.IntValues[1], star.IntValues[3]);
                        ServerLinks.GSStars.Add(serverstar.ID, serverstar);
                        StarNamesPositions.Remove(serverstar.ID);
                    }
                    if (innerstarsresult == false) { groupstarresult = false; break;  }
                }
                if (groupstarresult == false) continue;
                //получение остальных звёзд
                TextPosition[] RandomsStarsPositions = TextPosition.GetPositions(Common.GetTaggedValue(galaxyfile, "RANDOMSTARS"));
                int stars = RandomsStarsPositions[0].IntValues[0];
                bool randomstarsresult = true;
                for (int i = 0; i < stars; i++)
                {
                    KeyValuePair<int, TextPosition> pair = StarNamesPositions.ElementAt(rnd.Next(StarNamesPositions.Count));
                    double[] coords = GetRandomStarCoords(-GalaxyWidth / 2 + 5, -GalaxyHeight / 2 + 5, GalaxyWidth / 2 - 5, GalaxyHeight / 2 - 5, RandomDistance, -1, 0, rnd);
                    if (coords== null) { randomstarsresult = false; break; }
                    TextPosition star = pair.Value;
                    ServerStar serverstar = new ServerStar(pair.Key, new GSString(star.StringValues[0]), new GSString(star.StringValues[1]),
                           coords[0], coords[1], (EStarClass)star.IntValues[2], (byte)star.IntValues[1], star.IntValues[3]);
                    ServerLinks.GSStars.Add(serverstar.ID, serverstar);
                    StarNamesPositions.Remove(serverstar.ID);

                }
                if (randomstarsresult == false) continue;
                break;
            }
            //перенос звёзд на клиент
            byte[] starsarray = ServerStar.GetArray(ServerLinks.GSStars);
            Links.Stars = Gets.GetStars(starsarray);

            //расстановка фиксированных планет
            Links.Planets = new SortedList<int, GSPlanet>();
            string[] planetstext = Common.GetTaggedValue(galaxyfile, "FIXEDPLANETS").Split('\n');
            foreach (string s in planetstext)
            {
                if (s.Length <= 5) continue;
                TextPosition[] positions = TextPosition.GetPositions(s);
                int id=-1; string name=""; int starid = -1; int orbit = -1; int size = 0; int moons = 0; bool belt = false;
                int image = 0; PlanetTypes type = PlanetTypes.Green; int population = 0; int locs = 0; 
                foreach (TextPosition pos in positions)
                switch (pos.Title)
                    {
                        case "ID": id = pos.IntValues[0]; break;
                        case "N": name = pos.StringValues[0]; break;
                        case "STAR": starid = pos.IntValues[0]; break;
                        case "ORB": orbit = pos.IntValues[0]; break;
                        case "SIZE": size = pos.IntValues[0]; break;
                        case "MOONS": moons = pos.IntValues[0]; break;
                        case "BELT": belt = true; break;
                        case "IMAGE": image = pos.IntValues[0]; break;
                        case "TYPE": type = pos.StringValues[0] == "B" ? PlanetTypes.Burned : pos.StringValues[0] == "F" ? PlanetTypes.Freezed :
                                pos.StringValues[0] == "S" ? PlanetTypes.Gas : PlanetTypes.Green; break;
                        case "POP": population = pos.IntValues[0]; break;
                        case "LOCS": locs = pos.IntValues[0]; break;
                    }
                GSPlanet planet = new GSPlanet(id, new GSString(name), starid, orbit, size, moons, belt, image, type, population, locs, RareBuildings.No);
                Links.Planets.Add(planet.ID, planet);
            }
            //генерация случайных планет
            TextPosition[] randomplanets = TextPosition.GetPositions(Common.GetTaggedValue(galaxyfile, "RANDOMPLANETS"));
            PlanetsNames = new List<string>(System.IO.File.ReadAllLines("planets.txt"));
            int midplanets = 3; int delta = 3; SortedSet<int> exstars=new SortedSet<int>();
            foreach (TextPosition pos in randomplanets)
            {
                switch (pos.Title)
                {
                    case "MID": midplanets = pos.IntValues[0]; break; // среднее количество планет в системе
                    case "DELTA": delta = pos.IntValues[0]; break; //максимальное отклонение от среднего значения
                    case "EXSTAR": exstars = new SortedSet<int>(pos.IntValues); break; //Исключённые звёзды. На этих звёздах планеты не генерируются
                }
            }
            foreach (GSStar star in Links.Stars.Values)
            {
                if (exstars.Contains(star.ID)) continue;
                int needplanets = midplanets + rnd.Next(-delta, delta);
                int planets = star.Planets.Count;
                SortedSet<byte> orbits = new SortedSet<byte>(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
                if (star.OreBelt < 10) orbits.Remove(star.OreBelt);
                foreach (GSPlanet planet in star.Planets.Values)
                    orbits.Remove((byte)planet.Orbit);
                for (int i=planets;i<needplanets;i++)
                {
                    if (orbits.Count == 0) continue;
                    byte orbit = orbits.ElementAt(rnd.Next(orbits.Count));
                    orbits.Remove(orbit);
                    GSPlanet planet = GetPlanet(star, orbit);
                    Links.Planets.Add(planet.ID, planet);
                }
            }
            foreach (GSPlanet planet in Links.Planets.Values)
            {
                if (planet.Name.ToString() != "RND") continue;
                int pos = 1;
                for (int i = 0; i < planet.Orbit; i++)
                    if (planet.Star.Planets.ContainsKey(i)) pos++;
                planet.Name = new GSString(String.Format("{0} {1}", planet.Star.Name, NameCreator.GetRoman((byte)pos)));
            }
            ServerLinks.GSPlanets = new SortedList<int, ServerPlanet>();
            foreach (GSPlanet planet in Links.Planets.Values)
                ServerLinks.GSPlanets.Add(planet.ID, new ServerPlanet(planet.ID, planet.Name, planet.StarID, planet.Orbit, planet.Size, planet.Moons,
                    planet.HasBelt, planet.ImageType, planet.PlanetType, planet.MaxPopulation, planet.Locations, planet.RareBuilding));
            //ServerLinks.Enemy.Init(rnd);
            //ServerStar.CreateEnemyArray();

            ServerStar.FillOreBelts();

            GameScience.SetBasicScienceList();
            //Определение сюжетных планет, недоступных для миссий любого типа
            foreach (StoryLine2 story in StoryLine2.StoryLines.Values)
            {
                if (story.StoryType==StoryType.PlanetBattle)
                {
                    ServerPlanet splanet = ServerLinks.GSPlanets[story.LocationID];
                    splanet.QuestPlanet = true;
                    GSPlanet cplanet = Links.Planets[story.LocationID];
                    cplanet.QuestPlanet = true;
                }
            }
            //считывание вспомогательной информации
            string miscfile = System.IO.File.ReadAllText("Galaxy_Misc.txt", Encoding.GetEncoding(1251));
            MiscLands = new SortedList<int, string>();
            for (int i=1; ;i++)
            {
                string tag = String.Format("LAND{0}", i.ToString());
                string line = Common.GetTaggedValue(miscfile, tag);
                if (line != "") MiscLands.Add(i, line);
                else break;
            }
            MiscAvanposts = new SortedList<int, string>();
            for (int i=1; ;i++)
            {
                string tag = String.Format("AVANPOST{0}", i.ToString());
                string line = Common.GetTaggedValue(miscfile, tag);
                if (line != "") MiscAvanposts.Add(i, line);
                else break;
            }
            MiscSchemas = new SortedList<int, string>();
            for (int i=1; ;i++)
            {
                string tag = String.Format("SCHEMA{0}", i.ToString());
                string line = Common.GetTaggedValue(miscfile, tag);
                if (line != "") MiscSchemas.Add(i, line);
                else break;
            }
            MiscPilots = new SortedList<int, string>();
            for (int i = 1; ; i++)
            {
                string tag = String.Format("PILOT{0}", i.ToString());
                string line = Common.GetTaggedValue(miscfile, tag);
                if (line != "") MiscPilots.Add(i, line);
                else break;
            }
            MiscShipImages = new SortedList<int, string>();
            for (int i = 1; ; i++)
            {
                string tag = String.Format("SHIPIMAGE{0}", i.ToString());
                string line = Common.GetTaggedValue(miscfile, tag);
                if (line != "") MiscShipImages.Add(i, line);
                else break;
            }
            MiscFleetEmblems = new SortedList<int, string>();
            for (int i = 1; ; i++)
            {
                string tag = String.Format("FLEETEMBLEM{0}", i.ToString());
                string line = Common.GetTaggedValue(miscfile, tag);
                if (line != "") MiscFleetEmblems.Add(i, line);
                else break;
            }
            //настройка параметров игроков
            ServerLinks.GSPlayersByID = new SortedList<int, GSPlayer>();
            CreatePlayerInfo(galaxyfile, "PLAYER", 0, 0, true, null);
            ServerLinks.MainPlayer = ServerLinks.GSPlayersByID[0];
            CreatePlayerInfo(galaxyfile, "GREENTEAM", 1, 255, false, new GlobalAIParams(1, EnemySide.GreenTeam));
            ServerLinks.GreenTeam = ServerLinks.GSPlayersByID[1];
            CreatePlayerInfo(galaxyfile, "TECHNO", 2, 255,false, new GlobalAIParams(2, EnemySide.Techno));
            ServerLinks.TechnoTeam = ServerLinks.GSPlayersByID[2];
            CreatePlayerInfo(galaxyfile, "ALIEN", 3, 255, false, new GlobalAIParams(3, EnemySide.Alien));
            ServerLinks.Alien = ServerLinks.GSPlayersByID[3];
            ServerLinks.GSPlayersByID[0].CreatePlanetInfo();
            ServerLinks.MainPlayer.LocalMarket = new ServerMarket(ServerLinks.MainPlayer, MarketType.Local);
            
            //ServerLinks.TechnoTeam.SendFleetToScout(testland.ID, 1, 22, out battleid);
            //ServerLinks.TechnoTeam.SendFleetToScout(testland.ID, 2, 22, out battleid);
            /*
            player.AddArtefacts();
            

           

            GSPlayer alien = new GSPlayer(3, new GSString("Alien"), 0, 0, 0, 0, null, -1,
                null, null, new byte[0], new byte[0], null, Quest_Position.Q01_Basic_Info, null, 255);
            ServerLinks.GSPlayersByID.Add(alien.ID, alien);
            alien.AddLand(12); 

            player.CreatePlanetInfo();
            string[] BuildingsInfo = GSBuilding.GetTextInfo();
            System.IO.File.WriteAllLines("GameData/StoryLine/Ru/Buildings.txt", BuildingsInfo, Encoding.GetEncoding(1251));*/
            CreateMissions();
        }
        static SortedList<int, string> MiscLands;
        static SortedList<int, string> MiscAvanposts;
        static SortedList<int, string> MiscSchemas;
        static SortedList<int, string> MiscPilots;
        static SortedList<int, string> MiscShipImages;
        static SortedList<int, string> MiscFleetEmblems;
        static void CreatePlayerInfo(string file, string tag, int id, byte story, bool mainplayer, GlobalAIParams aiparams)
        {
            TextPosition[] playerpositions = TextPosition.GetPositions(Common.GetTaggedValue(file, tag));
            int playermoney = 0; int playermetal = 0; int playerchips = 0; int playeranti = 0; int[] landsinfo = new int[0];
            int[] fleetsparams = new int[0]; int[] shipsparams = new int[0]; int[] avanpostparam = new int[0];
            int[] arts = new int[0];
            foreach (TextPosition pos in playerpositions)
            {
                switch (pos.Title)
                {
                    case "MONEY": playermoney = pos.IntValues[0]; break;
                    case "METALL": playermetal = pos.IntValues[0]; break;
                    case "CHIPS": playerchips = pos.IntValues[0]; break;
                    case "ANTI": playeranti = pos.IntValues[0]; break;
                    case "LANDS": landsinfo = pos.IntValues; break;
                    case "FLEETS": fleetsparams = pos.IntValues; break;
                    case "SHIPS": shipsparams = pos.IntValues; break;
                    case "AVANPOSTS": avanpostparam = pos.IntValues; break;
                    case "ARTS": arts = pos.IntValues; break;
                }
            }
            GSPlayer player = new GSPlayer(id, new GSString(tag), playermoney, playermetal, playerchips, playeranti, story, mainplayer, aiparams);
            ServerLinks.GSPlayersByID.Add(player.ID, player);
            for (int i = 0; i < landsinfo.Length; i += 2)
            {
                string landparams = null; if (landsinfo[i + 1] >= 1) landparams = MiscLands[landsinfo[i + 1]];
                player.AddLand(landsinfo[i], landparams);
            }
            for (int i = 0; i < fleetsparams.Length; i += 3)
            {
                byte[] emblem = TextPosition.GetPositions(MiscFleetEmblems[fleetsparams[i + 2]])[0].GetBytes();
                ServerFleet fleet;
                player.CreateFleet(ServerLinks.GSPlanets[fleetsparams[i]].Lands.Values[0].ID, (byte)(fleetsparams[i + 1]), emblem, out fleet);
            }
            for (int i = 0; i < shipsparams.Length; i += 5)
            {
                Schema schema = Schema.GetSchema(MiscSchemas[shipsparams[i + 2]]);
                ServerShip ship = ServerShip.GetNewShip(schema, TextPosition.GetPositions(MiscShipImages[shipsparams[i + 3]])[0].GetBytes());
                player.Ships.Add(ship.ID, ship);
                ship.Player = player;
                ship.Pilot = GSPilot.GetStartPilot(MiscPilots[shipsparams[i + 4]]);
                if (shipsparams[i] != -1)
                {
                    int planetid = shipsparams[i];
                    byte sector = (byte)shipsparams[i + 1];
                    int landid = ServerLinks.GSPlanets[planetid].Lands.Values[0].ID;
                    long fleetid = ((ServerFleetBase)ServerLinks.GSPlanets[planetid].Lands.Values[0].Locations[sector]).Fleet.ID;
                    player.MoveShipToFleet(landid,fleetid, ship.ID);
                }
            }
            player.CalculateShipArray();
            for (int i = 0; i < avanpostparam.Length; i += 2)
            {
                string avanpostinfo = null; if (avanpostparam[i + 1] >= 0) avanpostinfo = MiscAvanposts[avanpostparam[i + 1]];
                player.AddAvanpost(avanpostparam[i], true, avanpostinfo);
            }
            player.AddArtefacts(arts);
            player.CreatePlanetInfo();
        }
        public static void CreateMissions()
        {
            foreach (ServerStar star in ServerLinks.GSStars.Values)
                star.MissionInspector.AddMissions();
            foreach (GSPlayer player in ServerLinks.GSPlayersByID.Values)
            {
                player.CreateMission2Array();
                player.CreateArrayForPlayer();
            }
        }
        /*public static SortedList<int, ServerStar> CalcStars(int count)
        {
            SortedList<int, ServerStar> stars = new SortedList<int,ServerStar>();
            ServerStar sun = new ServerStar(0, new GSString("Sun"), new GSString("Солнце"), 0, 0, EStarClass.G, 0);
            stars.Add(0,sun);
            int starscount = Links.Stars.Count - 1;
            SortedList<double, ServerStar> Distances = new SortedList<double, ServerStar>();
            for (int i =1;i<starscount; i++)
            {
                GSStar clientstar = Links.Stars[i];
                ServerStar star = new ServerStar(i, clientstar.En, clientstar.Ru, clientstar.X, clientstar.Y, clientstar.Class, clientstar.SizeModifier);
                double dist = Math.Sqrt(Math.Pow(star.X, 2) + Math.Pow(star.Y, 2));
                Distances.Add(dist, star);
            }
            double maxdistance = 20;
            double step = 1;
            for (;;)
            {
                foreach (ServerStar star in Distances.Values)
                {
                    bool iserror = false;
                    foreach (ServerStar curstar in stars.Values)
                    {
                        double dist = Math.Sqrt(Math.Pow(star.X-curstar.X, 2) + Math.Pow(star.Y-curstar.Y, 2));
                        if (dist<maxdistance) { iserror = true;  break; }
                    }
                    if (iserror == false)
                        stars.Add(star.ID, star);
                }
                if (stars.Count == count) break;
                else if (stars.Count > count)
                {
                    maxdistance += step;
                    stars.Clear(); stars.Add(0, sun);
                }
                else
                {
                    maxdistance -= step;
                    step = step * 0.1;
                    maxdistance += step;
                    stars.Clear(); stars.Add(0, sun);
                }
            }
            return stars;
        }*/
       /* static void GetNearStars(int stars, double X, double Y, SortedList<int, GSStar> BasicStars)
        {
            SortedList<double, int> stardist = new SortedList<double, int>();
            for (int i = 0; i < BasicStars.Count; i++)
            {
                GSStar star = BasicStars.Values[i];
                double d = Math.Sqrt(Math.Pow(star.X - X, 2) + Math.Pow(star.Y - Y, 2));
                if (stardist.ContainsKey(d)) d += 0.000000001;
                stardist.Add(d, star.ID);
            }

            for (int i = 0; i < stars; i++)
            {
                int starid = stardist.Values[i];
                GSStar star = BasicStars[starid];
                BasicStars.Remove(star.ID);
                ServerLinks.GSStars.Add(star.ID, new ServerStar(star.ID, star.En, star.Ru, star.X, star.Y, star.Class, star.SizeModifier));
            }
        }*/
        static List<string> PlanetsNames;
        static Random rnd;
        static int planetid = 7;
        static List<int> FirePlanets = new List<int>(new int[] { 1, 3, 13, 15, 19, 20, 22 });
        static List<int> ColdPlanets = new List<int>(new int[] { 5, 6, 7 });
        static List<int> GasGiantPlanets = new List<int>(new int[] { 0, 4, 12, 16, 17, 21 });
        static List<int> LivePlanets = new List<int>(new int[] { 2, 8, 9, 11, 14, 18 });
        static int count = 0;
        public static GSPlanet GetPlanet(GSStar star, int orbit)
        {
            string Name = "RND";
            if (PlanetsNames.Count > 0 && rnd.Next(2) == 0)
            {
                Name = PlanetsNames[rnd.Next(PlanetsNames.Count)];
                PlanetsNames.Remove(Name);
            }
            planetid = Links.Planets.Keys[Links.Planets.Count - 1] + 1;
            int planetsize; switch (orbit) { case 0: planetsize = rnd.Next(4); break; case 1: planetsize = rnd.Next(5); break;
                case 2: planetsize = rnd.Next(6); break;
                case 3: planetsize = rnd.Next(7); break;
                case 4: planetsize = rnd.Next(8); break;
                case 5: planetsize = rnd.Next(9); break;
                default: planetsize = rnd.Next(10); break;}
            int moons; int moonrandom = rnd.Next(100); if (moonrandom < 40) moons = 0; else if (moonrandom < 70) moons = 1; else if (moonrandom < 90) moons = 2; else moons = 3;
            if (orbit < 3 && moons > 1) moons = 1;
            if (planetsize < 2 && moons > 1) moons = 1;
            if (planetsize < 6 && moons > 2) moons = 2;
            int landsize = rnd.Next(100, 301) + planetsize * rnd.Next(20, 61) + moons * 50;
            int blagmodi = 2 - Math.Abs(4 - (int)star.Class);
            switch (orbit)
            {
                case 0: case 7: case 8: case 9: blagmodi -= 2; break;
                case 2: blagmodi += 2; break;
                case 3: case 4: blagmodi += 1; break;
                case 6: blagmodi -= 1; break;
            }
            switch (planetsize)
            {
                case 1: case 5: blagmodi += 1; break;
                case 2: case 3: case 4: blagmodi += 2; break;
                case 7: blagmodi = -1; break;
                case 8: case 9: blagmodi -= 2; break; 
            }
            blagmodi += moons;
            blagmodi += rnd.Next(3);
            if (blagmodi > 2 && planetsize >= 7) blagmodi = 2;
            if (blagmodi < 0) blagmodi = 0;
            if (blagmodi > 4) blagmodi = 4;
            if (blagmodi == 0) landsize = 0;
            int image;
            PlanetTypes type;
            if (blagmodi > 2) { image = LivePlanets[rnd.Next(LivePlanets.Count)]; type = PlanetTypes.Green; }
            else if (planetsize >= 7) { image = GasGiantPlanets[rnd.Next(GasGiantPlanets.Count)]; type = PlanetTypes.Gas; }
            else if (orbit < 5) { image = FirePlanets[rnd.Next(FirePlanets.Count)]; type = PlanetTypes.Burned; }
            else { image = ColdPlanets[rnd.Next(ColdPlanets.Count)]; type = PlanetTypes.Freezed; }
            if (blagmodi > 1) { landsize = landsize / blagmodi;  landsize += (blagmodi - 1) * 50; }
            bool belt = rnd.Next(100) < 20;
            landsize = (int)(Math.Round(landsize / 10.0, 0) * 10);
            if (blagmodi > 0) count++;
            int maxpopulation = landsize * blagmodi;
            int locations = rnd.Next(1,5) + blagmodi + moons;
            if (locations > 8) locations = 8;
            if (maxpopulation == 0) locations = 0;
            GSPlanet planet = new GSPlanet(planetid, new GSString(Name), star.ID, orbit, planetsize, moons, belt, image, type, maxpopulation, locations, RareBuildings.No);
            return planet;
        }
        public static byte[] StartTestBattle(byte[] array)
        {
            
            ServerLinks.GSStars = new SortedList<int, ServerStar>();
            ServerLinks.GSStars.Add(0, new ServerStar(0, new GSString("Sun"), new GSString("Солнце"), 0, 0, EStarClass.G, 0, 4));
            ServerLinks.ShipParts.Update();
            byte level = (byte)(5 + array[1] * 10);
            byte ships = (byte)(3 + array[2] * 3);
            byte field = array[3];
            byte asteroids = array[4];//(byte)((array[3] + 1) * array[4]);
            byte level1 = level; byte level2 = level;
            byte ships1 = ships; byte ships2 = ships;
            if (array[0] == 0) { level1 += 5; ships1 += 2; }
            else if (array[0] == 2) { level2 += 5; ships2 += 2; }

            ServerFleet fleet1 = new ServerFleet(1201, new byte[4], null, false, new long[0], new FleetCustomParams(2000));
            fleet1.Target = new ServerFleetTarget(ServerLinks.GSStars[0], fleet1, FleetMission.TestBattle);
            List<ushort> Part1 = new List<ushort>(new ushort[] { 100, 101, 102, 103 });
            List<ushort> Part2 = new List<ushort>(new ushort[] { 110, 111, 112, 113, 120, 121, 122, 123 });
            List<ushort> Part3 = new List<ushort>(new ushort[] { 130, 131, 132, 133 });
            Random rnd = new Random();
            fleet1.Artefacts.Add(ServerLinks.Artefacts[Part1[rnd.Next(Part1.Count)]]);
            fleet1.Artefacts.Add(ServerLinks.Artefacts[Part2[rnd.Next(Part2.Count)]]);
            fleet1.Artefacts.Add(ServerLinks.Artefacts[Part3[rnd.Next(Part3.Count)]]);

            for (int i = 0; i < ships1; i++)
            {
                //fleet1.Ships.Add(1000+i, ServerSchemeGenerator.GetTestShip(i));
                ServerShip ship = ServerSchemeGenerator.GetShip(1000 + i, level1, EnemyType.None);
                if (ship != null)
                    fleet1.Ships.Add(ship.ID, ship);
            }
            ServerFleet fleet2 = new ServerFleet(2201, new byte[4], null, false, new long[0], new FleetCustomParams(2000));
            fleet2.Target = new ServerFleetTarget(ServerLinks.GSStars[0], fleet2, FleetMission.TestBattle);
            fleet2.Target.Health = level * 100;
            EnemyType enemy = (EnemyType)(DateTime.Now.Second % 5);
            for (int i = 0; i < ships2; i++)
            {
                //fleet2.Ships.Add(1000 + i, ServerSchemeGenerator.GetTestShip(i));
                ServerShip ship = ServerSchemeGenerator.GetShip(2000 + i, level2, enemy);
                if (ship != null)
                    fleet2.Ships.Add(ship.ID, ship);
            }
            BattleFieldGroup group = BattleFieldGroup.GetGroup(field, field, false, false, false, asteroids);
            ServerBattle battle = new ServerBattle(fleet1, false, fleet2, true, BattleType.Test, group);
            return new ByteMessage(true, BitConverter.GetBytes(battle.ID)).GetBytes();
        }
        public static byte[] GetBattleBasicInfo(byte[] battleID)
        {
            long battleid = BitConverter.ToInt64(battleID, 0);
            ServerBattle battle = ServerLinks.Battles[battleid];
            return new ByteMessage(true, battle.BasicInfo).GetBytes();
        }
        /// <summary> метод получает от сервера стартовую информацию о сторонах, участвующих в бою </summary>
        public static byte[] GetBattleStartInfo(byte[] battleID)
        {
            long battleid = BitConverter.ToInt64(battleID, 0);
            ServerBattle battle = ServerLinks.Battles[battleid];
            return new ByteMessage(true, battle.StartArray).GetBytes();
        }
        /// <summary> метод возвращает список ходов выбранного боя </summary>
        public static byte[] GetBattleMoveList(byte[] battleID)
        {
            long battleid = BitConverter.ToInt64(battleID, 0);
            ServerBattle battle = ServerLinks.Battles[battleid];
            return new ByteMessage(true, battle.Moves.Array).GetBytes();
        }
        /// <summary> метод возвращает время в секундах до завершения хода </summary>
        public static byte[] GetBattleTurnEndTime(byte[] battleID)
        {
            long battleid = BitConverter.ToInt64(battleID, 0);
            ServerBattle battle = ServerLinks.Battles[battleid];
            short result = 0;
            if (battle.TurnEndTime == DateTime.MinValue) result = 0;
            else result = (short)((battle.TurnEndTime - DateTime.Now).TotalSeconds + 2);
            byte[] array = BitConverter.GetBytes(result);
            return new ByteMessage(true, array).GetBytes();
        }
        /// <summary> метод задаёт ходы для тестового боя </summary>
        public static byte[] SetTestBattleMoveList(byte[] battleID, byte[] sidemoves)
        {
            long battleid = BitConverter.ToInt64(battleID, 0);
            ServerBattle battle = ServerLinks.Battles[battleid];
            List<BattleCommand> side1Commands = new List<BattleCommand>();
            for (int i = 0; i < sidemoves.Length; i += 3)
                side1Commands.Add(new BattleCommand(sidemoves[i], sidemoves[i + 1], sidemoves[i + 2]));
            battle.Side1.CurrentCommand = side1Commands;
            battle.Side1.IsHaveCommands = true;
            battle.TurnEndTime = DateTime.MinValue;
            if (battle.CurMode == BattleMode.Mode1)
                battle.RoundNext();
            else
                battle.RoundNextMode2();
            return new ByteMessage(true, new byte[] { 0 }).GetBytes(); //необходимо сбросить таймер
        }
        ///<summary> метод устанавливает бой в автоматический режим</summary>
        public static byte[] SetBattleToAutoMode(byte[] battleID)
        {
            long battleid = BitConverter.ToInt64(battleID, 0);
            ServerBattle battle = ServerLinks.Battles[battleid];
            if (battle.IsFinished) return new ByteMessage(true, new byte[] { 0 }).GetBytes();
            if (battle.Side1.IsAutoControl == false)
            {
                battle.Side1.IsAutoControl = true;
                if (battle.CurMode == BattleMode.Mode1)
                    battle.Moves.SetSide1Auto();
            }
            else if (battle.Side2.IsAutoControl == false)
            {
                battle.Side2.IsAutoControl = true;
                if (battle.CurMode==BattleMode.Mode1)
                    battle.Moves.SetSide2Auto();
            }
            if (battle.CurMode == BattleMode.Mode2)
                battle.RoundNextMode2();
            battle.TurnEndTime = DateTime.MinValue;
                return new ByteMessage(true, new byte[] { 0 }).GetBytes(); //необходимо сбросить таймер
        }
        public static byte[] GetInformation(byte[] randomArr, byte[] query)
        {
            if (randomArr == null || query == null) throw new Exception();
            if (randomArr.Length != 8) throw new Exception();
            long random = BitConverter.ToInt64(randomArr, 0);
            ByteMessage message = ByteMessage.SaveReconMessage(query);
            if (message == null) throw new Exception();
            GSPlayer player = ServerLinks.GSPlayersByID[0];
            byte[] answer = message.ResponseToInformationRequest(player);
            message = new ByteMessage(true, answer);
            return message.GetBytes();
        }
        public static byte[] ProcessEvent(byte[] randomArr, byte[] query, int EventOrder)
        {
            if (randomArr == null || query == null) throw new Exception();
            if (randomArr.Length != 8) throw new Exception();
            long random = BitConverter.ToInt64(randomArr, 0);

            ByteMessage message = ByteMessage.SaveReconMessage(query);
            if (message == null) throw new Exception();
            GSPlayer player = ServerLinks.GSPlayersByID[0];
            return GSGameEvent.MakeEvent(player, message).GetBytes();
        }
        public static void FillNewLandsSciences()
        {
            ServerLinks.Science.NewLands = new SortedList<ushort, GameScience>();
            foreach (GameScience science in ServerLinks.Science.GameSciences.Values)
                if (science.ID % 1000 == 900)
                    ServerLinks.Science.NewLands.Add(science.ID, science);
        }
        /// <summary>метод задаёт список ходов выбранного боя</summary>
        public static byte[] SetBattleMoveList(byte[] battleID, byte[] side1moves, byte[] side2moves)
        {
            if (battleID == null || battleID.Length != 8) throw new Exception();
            //if (randomArr == null || randomArr.Length != 8) throw new Exception();
            if (side1moves == null || side2moves == null) throw new Exception();
            if (side1moves.Length % 3 != 0 || side2moves.Length % 3 != 0) throw new Exception();
            long battleid = BitConverter.ToInt64(battleID, 0);
            if (battleid < 0) throw new Exception();
            if (!ServerLinks.Battles.ContainsKey(battleid)) throw new Exception();
            ServerBattle battle = ServerLinks.Battles[battleid];
            if (battle.IsFinished) throw new Exception();
            //long random = BitConverter.ToInt64(randomArr, 0);
            //if (!ServerLinks.GSLoggedAccounts.ContainsKey(random)) return ByteMessage.WrongAutorization; //проверка на соответсвие логина/пароля
            //GSLoggedAccount acc = ServerLinks.GSLoggedAccounts[random];
            if (battle.Side1.Player != ServerLinks.GSPlayersByID[0] && battle.Side2.Player != ServerLinks.GSPlayersByID[0]) throw new Exception();
            //acc.LoggedTime = DateTime.Now;
            if (battle.Side1.Player == ServerLinks.GSPlayersByID[0] && battle.Side1.IsAutoControl == false)
            {
                List<BattleCommand> side1Commands = new List<BattleCommand>();
                for (int i = 0; i < side1moves.Length; i += 3)
                    side1Commands.Add(new BattleCommand(side1moves[i], side1moves[i + 1], side1moves[i + 2]));
                battle.Side1.CurrentCommand = side1Commands;
                battle.Side1.IsHaveCommands = true;
                if (battle.Side2.IsAutoControl || battle.Side2.IsHaveCommands)
                {
                    battle.TurnEndTime = DateTime.MinValue;
                    if(battle.CurMode == BattleMode.Mode1)
                    battle.RoundNext();
                else
                    battle.RoundNextMode2();
                    return new ByteMessage(true, new byte[] { 0 }).GetBytes(); //необходимо сбросить таймер
                }
            }
            if (battle.Side2.Player == ServerLinks.GSPlayersByID[0] && battle.Side2.IsAutoControl == false)
            {
                List<BattleCommand> side2Commands = new List<BattleCommand>();
                for (int i = 0; i < side2moves.Length; i += 3)
                    side2Commands.Add(new BattleCommand(side2moves[i], side2moves[i + 1], side2moves[i + 2]));
                battle.Side2.CurrentCommand = side2Commands;
                battle.Side2.IsHaveCommands = true;
                if (battle.Side1.IsAutoControl || battle.Side1.IsHaveCommands)
                {
                    battle.TurnEndTime = DateTime.MinValue;
                    if (battle.CurMode == BattleMode.Mode2)
                        battle.RoundNext();
                    else
                        battle.RoundNextMode2();
                    return new ByteMessage(true, new byte[] { 0 }).GetBytes();//необходимо сбросить таймер
                }
            }
            return new ByteMessage(true, new byte[] { 1 }).GetBytes(); //ненадо сбрасывать таймер
        }
        public static void MakeTurn()
        {
            ServerLinks.NowTime += TimeSpan.FromDays(1);
            GSTimer.GameEventsPool();
        }
        public static void SaveGame()
        {
            GSPlayer.SaveAll();
        }
        static byte GetGroup(ushort id, bool can100)
        {
            byte level = (byte)(id / 1000.0);
            int ostatok = (int)(id - level * 1000);
            byte type = (byte)(ostatok / 10.0);
            if (can100 && (ostatok - type * 10) >= 5) type += 100;
            return type;

        }
        static void AddBuildingScience(GSBuilding build)
        {
            ServerLinks.Science.GameSciences.Add((ushort)build.ID, new GameScience((ushort)build.ID, build.Name, build.Level,  false, Links.Science.EType.Building));
        }
        static void AddEquipmentScience(Equipmentclass equip)
        {
            ServerLinks.Science.GameSciences.Add((ushort)equip.ID, new GameScience((ushort)equip.ID, equip.Name, equip.Level,  equip.IsRare, Links.Science.EType.Equipment));
        }
        static void AddWeaponScience(Weaponclass weapon)
        {
            ServerLinks.Science.GameSciences.Add((ushort)weapon.ID, new GameScience((ushort)weapon.ID, weapon.Name, weapon.Level, weapon.IsRare, Links.Science.EType.Weapon));
        }
        static void AddComputerScience(Computerclass comp)
        {
            ServerLinks.Science.GameSciences.Add((ushort)comp.ID, new GameScience((ushort)comp.ID, comp.Name, comp.Level, comp.IsRare, Links.Science.EType.Computer));
        }
        static void AddEngineScience(Engineclass engine)
        {
            ServerLinks.Science.GameSciences.Add((ushort)engine.ID, new GameScience((ushort)engine.ID, engine.Name, engine.Level, engine.IsRare, Links.Science.EType.Engine));
        }
        static void AddShieldScience(Shieldclass shield)
        {
            ServerLinks.Science.GameSciences.Add((ushort)shield.ID, new GameScience((ushort)shield.ID, shield.Name, shield.Level, shield.IsRare, Links.Science.EType.Shield));
        }
        static void AddGeneratorScience(Generatorclass generator)
        {
            ServerLinks.Science.GameSciences.Add((ushort)generator.ID, new GameScience((ushort)generator.ID, generator.Name, generator.Level, generator.IsRare, Links.Science.EType.Generator));
        }
        static void AddShipTypeScience(ShipTypeclass shiptype)
        {
            ServerLinks.Science.GameSciences.Add((ushort)shiptype.ID, new GameScience((ushort)shiptype.ID, shiptype.En, shiptype.Level,  shiptype.IsRare, Links.Science.EType.ShipTypes));
        }
        static byte GetCompGroup(ushort id)
        {
            return (byte)(id % 10);
        }
        public static void SetGameSciences()
        {
            ServerLinks.Science.GameSciences = new SortedList<ushort, GameScience>();
            foreach (ShipTypeclass shiptype in ServerLinks.ShipParts.ShipTypes.Values)
                AddShipTypeScience(shiptype);
            foreach (Generatorclass generator in ServerLinks.ShipParts.GeneratorTypes.Values)
                AddGeneratorScience(generator);
            ServerLinks.ShipParts.ShieldTypes = Links.ShieldTypes;
            foreach (Shieldclass shield in ServerLinks.ShipParts.ShieldTypes.Values)
                AddShieldScience(shield);
            foreach (Engineclass engine in ServerLinks.ShipParts.EngineTypes.Values)
                AddEngineScience(engine);
            foreach (Computerclass comp in ServerLinks.ShipParts.ComputerTypes.Values)
                AddComputerScience(comp);
            ServerLinks.ShipParts.WeaponTypes = Links.WeaponTypes;
            foreach (Weaponclass weapon in ServerLinks.ShipParts.WeaponTypes.Values)
                AddWeaponScience(weapon);
            ServerLinks.ShipParts.EquipmentTypes = Links.EquipmentTypes;
            foreach (Equipmentclass equip in ServerLinks.ShipParts.EquipmentTypes.Values)
                AddEquipmentScience(equip);
        }
    }
}
