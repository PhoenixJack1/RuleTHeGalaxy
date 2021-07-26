using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Client
{
    class Gets
    {
        static ByteMessage GetResponceToQuery(ByteMessage query, bool isImportant, string Text)
        {
            if (Links.GameMode == EGameMode.Multi)
            {
                if (isImportant)
                    return Links.Controller.loadWindow.GetResult(query, Text);

                else
                {
                    ByteMessage responce = null;
                    try
                    {
                        responce = new ByteMessage(Links.Loginproxy.GetInformation(BitConverter.GetBytes(GSGameInfo.random), query.GetBytes()));
                    }
                    catch (Exception e)
                    {
                        System.Windows.MessageBox.Show(e.Message);
                    }
                    return responce;
                }
            }
            else
                return new ByteMessage(LocalServer.GetInformation(BitConverter.GetBytes(GSGameInfo.random), query.GetBytes()));
        }
        public static byte[] GetArray(byte[] array, int startindex, int length)
        {
            byte[] result = new byte[length];
            for (int i = 0; i < length; i++, startindex++)
                result[i] = array[startindex];
            return result;
        }

        public static byte[] GetNoticeList()
        {
            ByteMessage ServerRequest = new ByteMessage(0, 8, null);
            ByteMessage Responce = GetResponceToQuery(ServerRequest, false, "");
            if (Responce.Type == MessageResult.Yes)
                return Responce.Message;
            else
                return null;
        }
        /*
        public static void GetTotalInfoNew(WorkMethod method)
        {
            ByteMessage query = new ByteMessage(0, 0, null);
            Links.Controller.LoadWindow2.LoadImportantInfo(method, query, "Loading", AnalysTotalInfo);
        }
        */
        static DateTime LastUpdateTime = DateTime.MaxValue;
        public static bool GetTotalInfo(string reason)
        {
            TimeSpan delta = DateTime.Now - LastUpdateTime;
            LastUpdateTime = DateTime.Now;

            ByteMessage Responce = GetResponceToQuery(new ByteMessage(0, 0, null), true, "Loading");
            //ByteMessage Responce = Links.Controller.LoadWindow2.LoadImportantInfo(new ByteMessage(0, 0, null), "Loading");
            bool a = AnalysTotalInfo(Responce);
            return a;

        }
        public static bool GetStatisticInfo()
        {
            ByteMessage Responce = GetResponceToQuery(new ByteMessage(0, 15, null), true, "Loading");
            GSGameInfo.StatisticInfo = Responce.Message;
            return true;
        }

        public static bool AnalysTotalInfo(ByteMessage Responce)
        {
            bool needstop = false;
            if (Responce.Type == MessageResult.Yes)
            {
                long servertimetiks = BitConverter.ToInt64(Responce.Message, 0);
                GSGameInfo.UpdateTime = DateTime.Now;
                GSGameInfo.ServerTime = new DateTime(servertimetiks);
                for (int i = 8; i < Responce.Message.Length;)
                {
                    byte field1 = Responce.Message[i];
                    i++;
                    byte field2 = Responce.Message[i];
                    i++;
                    int messageLength = BitConverter.ToInt32(Responce.Message, i);
                    i += 4;
                    byte[] array = GetArray(Responce.Message, i, messageLength);
                    i += messageLength;
                    switch (field1)
                    {
                        case 0:
                            switch (field2)
                            {
                                case 1: GetName(array); break;
                                case 4: GetPlayerSciences(array); break;
                                case 9: int pos = 0; GSGameInfo.Clan = new GSClan(array, ref pos); break;
                                //case 10: UpdateMissions(array); break;
                                case 11: GetHelpArray(array); break;
                                case 13: GetStoryLine(array); break;
                                case 14: GetAllMission2(array); break;
                            }
                            break;
                        case 3:
                            switch (field2)
                            {
                                case 2: GetShipSchemas(array); break;
                                case 4: GetShips(array); break;
                            }
                            break;
                        case 4:
                            switch (field2)
                            {
                                case 1: GetLands(array); break;
                                case 7: GetNewLands(array); break;
                            }
                            break;
                        case 6:
                            switch (field2)
                            {
                                case 1: GetNewPilotsList(array); break;
                                case 4: GetFreePilotsList(array); break;
                            }
                            break;
                        case 7:
                            switch (field2)
                            {
                                case 2: GetFleets(array); break;
                            }
                            break;
                        case 8:
                            switch (field2)
                            {
                                case 0: GetBattles(array); break;
                            }
                            break;
                        case 9:
                            switch (field2)
                            {
                                case 0: UpdateEnemys(array); break;
                                case 4: UpdatePlanetSides2(array); break;
                            }
                            break;
                        case 10:
                            switch (field2)
                            {
                                //case 0: GetPremium(array); break;
                                case 3: needstop = GetQuest(array); break;
                            }
                            break;
                        case 11:
                            switch (field2)
                            {
                                case 0: UpdateArtefacts(array); break;
                            }
                            break;
                    }
                }
            }
            return needstop;
        }
        /*
        public static Reward GetBattleReward(long battleID)
        {
            byte[] array = Links.Loginproxy.GetBattleRewardInfo(BitConverter.GetBytes(battleID));
            ByteMessage message = new ByteMessage(array);
            if (message.Type == MessageResult.Yes)
            {
                return new Reward(message.Message);
            }
            else
                return null;
        }
        public static string GetBattleReward(Battle Battle)
        {
            byte[] array = Links.Loginproxy.GetBattleRewardInfo(BitConverter.GetBytes(Battle.ID));
            ByteMessage message = new ByteMessage(array);
            if (message.Type == MessageResult.Yes)
            {
                Battle.Reward = new Reward(message.Message);
                return "";
            }
            else
                return Common.GetStringFromByteArray(message.Message, 0);
            
        }
        */
        public static void UpdateArtefacts(byte[] array)
        {
            GSGameInfo.Artefacts = new SortedList<Artefact, ushort>();
            for (int i=0;i<array.Length;)
            {
                ushort id = BitConverter.ToUInt16(array, i); i += 2;
                ushort value = BitConverter.ToUInt16(array, i); i += 2;
                Artefact art = Links.Artefacts[id];
                GSGameInfo.Artefacts.Add(art, value);
            }
        }
        public static short GetTurnEndTime(Battle Battle)
        {
            byte[] array;
            if (Links.GameMode == EGameMode.Multi)
                array = Links.Loginproxy.GetBattleTurnEndTime(BitConverter.GetBytes(Battle.ID));
            else
                array = LocalServer.GetBattleTurnEndTime(BitConverter.GetBytes(Battle.ID));
            ByteMessage message = new ByteMessage(array);
            if (message.Type == MessageResult.Yes)
                return BitConverter.ToInt16(message.Message, 0);
            else return -1;
        }
        public static bool ReadTurns(Battle Battle)
        {
            byte[] array;
            if (Links.GameMode == EGameMode.Multi)
                array = Links.Loginproxy.GetBattleMoveList(BitConverter.GetBytes(Battle.ID));
            else
                array = LocalServer.GetBattleMoveList(BitConverter.GetBytes(Battle.ID));
            ByteMessage message = new ByteMessage(array);
            if (message.Type == MessageResult.Yes)
            {
                IntBoya.AutoModePanel.SetImages(message.Message[0]);
                Battle.Moves = GameMove.GetMoves(message.Message);
            }
            return true;
        }
        public static bool UpdateBattle(Battle Battle)
        {
            if (Links.GameMode == EGameMode.Multi)
            {
                byte[] array = Links.Loginproxy.GetBattleStartInfo(BitConverter.GetBytes(Battle.ID));
                ByteMessage message = new ByteMessage(array);
                if (message.Type == MessageResult.Yes)
                    Battle.StartArray = message.Message;
                else
                {
                    Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Common.GetStringFromByteArray(message.Message, 0)), true);
                    return false;
                }
            }
            else
                Battle.StartArray = new ByteMessage(LocalServer.GetBattleStartInfo(BitConverter.GetBytes(Battle.ID))).Message;

            Battle.ReconStartArray();
            return true;

        }
        /*public static void GetPremium(byte[] array)
        {
            GSGameInfo.Premium = new Premium(array);
        }*/
        public static bool GetQuest(byte[] array)
        {
            return false;
            /*
            Quest_Position newquest = (Quest_Position)array[0];
            if (GSGameInfo.Quest==newquest)
            {
                return false;
            }
            else
            {
                GSGameInfo.Quest = newquest;
                Links.Controller.mainWindow.QuestButton.QuestButton_Click(null, null);
                Links.Controller.mainWindow.QuestButton.SetQuest();
                return true;
            }
            
            */
        }
        public static void GetBattles(byte[] array)
        {
            for (int i = 0; i < array.Length; )
            {
                long battleid = BitConverter.ToInt64(array, i);
                i += 8;
                BattleType type = (BattleType)array[i]; i++;
                long fleet1id = BitConverter.ToInt64(array, i);
                i += 8;
                byte[] emblem1 = Common.GetArray(array, i, 4); i += 4;
                long fleet2id = BitConverter.ToInt64(array, i);
                i += 8;
                byte[] emblem2 = Common.GetArray(array, i, 4); i += 4;
                int descrlength = BitConverter.ToInt32(array, i); i += 4;
                byte[] Description = Common.GetArray(array, i, descrlength); i += descrlength;
                //GSString Description = new GSString(array, i);
                //i += Description.Array.Length;
                if (GSGameInfo.Battles.Keys.Contains(battleid))
                    continue;
                else GSGameInfo.Battles.Add(battleid, new Battle(battleid, fleet1id, fleet2id, emblem1, emblem2, Description, type));
            }
        }
        
        public static void GetFleets(byte[] array)
        {
            //foreach (Land land in GSGameInfo.PlayerLands.Values)
            //    land.Fleets = new SortedList<long, GSFleet>();
            GSGameInfo.Fleets = new SortedList<long, GSFleet>();
            for (int i = 0; i < array.Length; )
            {
                int landid = BitConverter.ToInt32(array, i);
                i += 4;
                int fleetscount = BitConverter.ToInt32(array, i);
                i += 4;
                for (int j = 0; j < fleetscount; j++)
                    new GSFleet(landid, array, ref i);
            }
        }
        public static void GetFreePilotsList(byte[] array)
        {
            GSGameInfo.FreePilots = new List<GSPilot>();
            for (int i = 0; i < array.Length; i += 10)
            {
                GSPilot pilot = new GSPilot(array, i);
                GSGameInfo.FreePilots.Add(pilot);
            }
        }
        public static void GetNewPilotsList(byte[] array)
        {
            //ByteMessage ServerRequest = new ByteMessage(6, 1, null);
            //ByteMessage Pilots = GetResponceToQuery(ServerRequest, false, "");
            GSGameInfo.AcademyPilotsList = new List<GSPilot>();
            for (int i = 0; i < array.Length; i += 10)
            {
                GSPilot pilot = new GSPilot(array, i);
                GSGameInfo.AcademyPilotsList.Add(pilot);
            }
            //if (Pilots.Type == MessageResult.Yes)
            //    return Pilots.Message;
            //else return null;
        }
        public static double[] GetMarketPrices()
        {
            ByteMessage ServerRequest = new ByteMessage(5, 1, null);
            ByteMessage Prices = GetResponceToQuery(ServerRequest, false, "");
            if (Prices.Type == MessageResult.Yes)
            {
                List<double> result = new List<double>();
                result.Add(BitConverter.ToDouble(Prices.Message, 0));
                result.Add(BitConverter.ToDouble(Prices.Message, 8));
                result.Add(BitConverter.ToDouble(Prices.Message, 16));
                //result.Add(BitConverter.ToDouble(Prices.Message, 24));
                return result.ToArray();
            }
            return null;
        }
        public static void GetShips(byte[] array)
        {
            //ByteMessage ServerRequest = new ByteMessage(3, 4, null);
            //ByteMessage ShipsList = GetResponceToQuery(ServerRequest, false, "");
            //if (ShipsList.Type == MessageResult.Yes)
            //{
            int ships = GSGameInfo.Ships.Count;
                GSGameInfo.Ships = new SortedList<long, GSShip>();
                for (int i = 0; i < array.Length; )
                {
                    GSShip ship = GSShip.GetShip(array, ref i);
                    GSGameInfo.Ships.Add(ship.ID, ship);
                }

            //}
            //if (ships!=GSGameInfo.Ships.Count)
            //    Links.Controller.mainWindow.ShipsCount.SetValue(GSGameInfo.Ships.Count, (byte)(GSGameInfo.Ships.Count>ships?1:2));
        }
        public static void GetLands(byte[] array)
        {
            //ByteMessage ServerRequest = new ByteMessage(4, 1,null);
            //ByteMessage LandsList = GetResponceToQuery(ServerRequest, false, "");
            //if (LandsList.Type == MessageResult.Yes)
            //{
                GSGameInfo.PlayerLands = new SortedList<int, Land>();
                for (int i = 0; i < array.Length; )
                {
                    Land land = new Land(array, ref i);
                    GSGameInfo.PlayerLands.Add(land.ID, land);
                land.Planet.LocationID = land.ID;
                }
            //}
            GSGameInfo.CalculateLandsParams();
        }
        public static void GetNewLands(byte[] array)
        {
            GSGameInfo.PlayerAvanposts = new SortedList<int, Avanpost>();
            for (int i = 0; i < array.Length;)
            {
                Avanpost land = Avanpost.GetLand(array, ref i);
                GSGameInfo.PlayerAvanposts.Add(land.ID, land);
                land.Planet.LocationID = land.ID;
            }
        }
        public static void GetShipSchemas(byte[] array)
        {    
                GSGameInfo.PlayerSchemas = new List<Schema>();
                for (int i = 0; i < array.Length; )
                {
                    Schema schema = Schema.GetSchema(array, ref i);
                    GSGameInfo.PlayerSchemas.Add(schema);
                }               
        }

        public static void GetName(byte[] array)
        {
            if (Links.GameMode == EGameMode.Multi)
                GSGameInfo.SetPlayerName(new GSString(array, 0));
            //else
            //    GSGameInfo.SetServerTime();
        }
        public static void GetResources()
        {
            
            ByteMessage query = new ByteMessage(0, 2, null);
            ByteMessage ResourceMessage = GetResponceToQuery(query, false, "");
            if (ResourceMessage == null) return;
            if (ResourceMessage.Type == MessageResult.Yes)
            {
                byte[] array = ResourceMessage.Message;
                GSGameInfo.SetMoney(BitConverter.ToInt64(array,0));
                GSGameInfo.SetMetals(BitConverter.ToInt32(array,8));
                GSGameInfo.SetChips(BitConverter.ToInt32(array,12));
                GSGameInfo.SetAnti(BitConverter.ToInt32(array,16));
                GSGameInfo.SetPremium(BitConverter.ToInt32(array, 20));
            }
        }
        public static void GetBasicSciences(byte[] array)
        {
            Links.Science.GameSciences = GameScience.GetList(array);
            Links.Science.NewLandsSciences = new SortedList<ushort, GameScience>();
            int i = 1;
            foreach (GameScience science in Links.Science.GameSciences.Values)
            {
                if (science.Type == Links.Science.EType.Other)
                    Links.Science.NewLandsSciences.Add(science.ID, science);
                science.Name = i.ToString();
            }
        }
        /*public static void GetSciencePrices(string PriceCodes)
        {
            ScienceAnalys.CalculateMaxScienceLevel();
            Links.Science.SciencePrices = new SciencePrice[4];
            byte[] array=Common.StringToByte(PriceCodes);
            for (int i = 0; i < array.Length; )
            {
                SciencePrice scPrice = new SciencePrice(array, ref i);
                Links.Science.SciencePrices[(int)scPrice.Field] = scPrice;
            }
        }*/
        /*public static void GetSciencePriceBases()
        {
            ByteMessage query = new ByteMessage(0, 7, null);
            ByteMessage BasesMessage = GetResponceToQuery(query, true, "");
            if (BasesMessage == null) return;
            if (BasesMessage.Type == MessageResult.Yes)
            {
                byte[] array = BasesMessage.Message;
                SciencePrice.LearnedScience[0] = BitConverter.ToInt32(array, 0);
                SciencePrice.LearnedScience[1] = BitConverter.ToInt32(array, 4);
                SciencePrice.LearnedScience[2] = BitConverter.ToInt32(array, 8);
                SciencePrice.LearnedScience[3] = BitConverter.ToInt32(array, 12);
            }
        }*/
        /*public static void UpdateMissions(byte[] array)
        {
            GSGameInfo.Missions.Clear();
            for (int i = 0; i < array.Length; )
                GSGameInfo.Missions.Add(new Mission(array, ref i));
        }*/
        static SortedSet<int> StarsWithMissions = new SortedSet<int>();
        public static void GetAllMission2(byte[] array)
        {
            SortedSet<int> CurStars = new SortedSet<int>();
            for (int i=0;i<array.Length;)
            {
                int starid = BitConverter.ToInt32(array, i); i += 4;
                byte missioncount = array[i]; i++;
                SortedList<byte, Mission2> Missions = new SortedList<byte, Mission2>();
                for (int j=0;j<missioncount;j++)
                {
                    byte orbit = array[i]; i++;
                    Mission2Type type = (Mission2Type)array[i]; i++;
                    byte time = array[i]; i++;

                    Missions.Add(orbit, new Mission2(type, time, orbit, starid));
                }
                Links.Stars[starid].Missions = Missions;
                Links.Stars[starid].DrawMissions2();
                CurStars.Add(starid);
            }
            foreach (int id in StarsWithMissions)
                if (CurStars.Contains(id) == false) { Links.Stars[id].Missions = new SortedList<byte, Mission2>(); Links.Stars[id].DrawMissions2(); }
            StarsWithMissions = CurStars;
        }
        //static StoryLine2 CurrentStory;
        public static void GetStoryLine(byte[] storyline)
        {
            //Если миссия такая-же - то ничего не делаем. - СДЕЛАНО
            if (GSGameInfo.StoryLinePosition == storyline[0] && GSGameInfo.StoryStatus == storyline[1])
                return;
            GSGameInfo.StoryLinePosition = storyline[0];
            GSGameInfo.StoryStatus = storyline[1];
            //Если миссии в списке нету - то:
            //Панель убрать - СДЕЛАНО
            //Иконки текстовые убрать - СДЕЛАНО
            //Иконки боевые убрать - СДЕЛАНО
            if (StoryLine2.StoryLines.ContainsKey(GSGameInfo.StoryLinePosition) == false)
            {
                Links.Controller.Galaxy.RemoveTextStoryIcon();
                if (StoryLinePanel.PanelIsCreated) Links.Controller.PopUpCanvas.Remove();
                GSStar.RemoveStoryIcon();
                Galaxy.BattleStory = null;
            }
            else
            {
                //Получаем миссию - СДЕЛАНО
                StoryLine2 story = StoryLine2.StoryLines[GSGameInfo.StoryLinePosition];
                //Если миссия текстовая и панель открыта - меняем текст - СДЕЛАНО
                //Если миссия текстовая и иконки текстовой нет - ставим текстовую иконку - СДЕЛАНО
                //Если миссия текстовая и иконки боевой миссии есть - убираем боевую икноку - СДЕЛАНО
                if (story.StoryType == StoryType.Text)
                {
                    Links.Controller.Galaxy.AddTextStoryIcon();
                    if (StoryLinePanel.PanelIsCreated) ((StoryLinePanel)(StoryLinePanel.CurrentPanel)).ChangeStory(story);
                    GSStar.RemoveStoryIcon();
                    Galaxy.BattleStory = null;
                }
                //Если миссия боевая и не запущена - отображаем иконку миссии - СДЕЛАНО
                //И если панель открыта - то закрываем - СДЕЛАНО
                //И если есть текстовая иконка - убираем - СДЕЛАНО
                else if (story.StoryType == StoryType.SpaceBattle)
                {
                    Links.Controller.Galaxy.RemoveTextStoryIcon();
                    if (StoryLinePanel.PanelIsCreated) Links.Controller.PopUpCanvas.Remove();
                    if (GSGameInfo.StoryStatus == 0)
                    {
                        GSStar.PutStoryIcon();
                        Galaxy.BattleStory = story;
                    }
                    else
                    {
                        GSStar.RemoveStoryIcon();
                        Galaxy.BattleStory = null;
                    }
                }
                else if (story.StoryType == StoryType.PlanetBattle)
                {
                    Links.Controller.Galaxy.RemoveTextStoryIcon();
                    if (StoryLinePanel.PanelIsCreated) Links.Controller.PopUpCanvas.Remove();
                    if (GSGameInfo.StoryStatus == 0)
                    {
                        GSStar.PutStoryIcon();
                        Galaxy.BattleStory = story;
                    }
                    else
                    {
                        GSStar.RemoveStoryIcon();
                        Galaxy.BattleStory = null;
                    }
                }
            }
            Links.Controller.Galaxy.DrawOneStoryInfo();
        }
        public static void GetHelpArray(byte[] array)
        {
            GSGameInfo.HelpArray = array;
        }
        public static void UpdateEnemys(byte[] array)
        {
            for (int i = 0; i < array.Length; i++)
                Links.Stars.Values[i].Enemy = array[i];
        }
        public static void UpdatePlanetSides2(byte[] array)
        {
            for (int i = 0; i < array.Length;)
            {
                PlanetSide side = (PlanetSide)array[i]; i++;
                GSPlanet planet = Links.Planets.ElementAt(i/5).Value;
                planet.PlanetSide = side;
                planet.LandOrAvanpost = array[i]; i++;
                planet.Pillage = array[i] > 0; i++;
                planet.Conquer = array[i] > 0; i++;
                planet.RiotIndicator = array[i];i++;
                //ushort id = BitConverter.ToUInt16(array, i); i += 2;
                //Links.Planets[id].PlanetSide = (PlanetSide)array[i]; i++;
            }
        }
        static DateTime UpdateFreeLandsTime = new DateTime(100, 1, 1);
        public static bool UpdateFreeLands()
        {
            if (UpdateFreeLandsTime + TimeSpan.FromSeconds(100) > DateTime.Now) return true;
             ByteMessage ServerRequest = new ByteMessage(9, 2, null);
            ByteMessage Responce = GetResponceToQuery(ServerRequest, false, "");
            if (Responce.Type == MessageResult.Yes)
            {
                for (int i = 0; i < Responce.Message.Length; i++)
                    Links.Stars[i].FreeLands = Responce.Message[i];
                UpdateFreeLandsTime = DateTime.Now;
                return true;
            }
            else return false;
        }
        public static SortedList<int, GSStar> GetStars(byte[] array)
        {
            return GSStar.GetList(array);
        }
        public static void GetPlanets(byte[] array)
        {
            throw new Exception("Метод не поправлен");
            //Links.Planets = GSPlanet.GetList(array);
        }
        public static void GetShipTypes(string[] array)
        {
            Links.ShipTypes = ShipTypeclass.GetList(array);
        }
        public static void GetComputerTypes(string[] array)
        {
            Links.ComputerTypes = Computerclass.GetList(array);
        }
        public static void GetEngineTypes(string[] array)
        {
            Links.EngineTypes = Engineclass.GetList(array);
        }
        public static void GetGeneratorTypes(string[] array)
        {
            Links.GeneratorTypes = Generatorclass.GetList(array);
        }
        public static void GetShieldTypes(string[] array)
        {
            Links.ShieldTypes = Shieldclass.GetList(array);
        }
        public static void GetWeaponModifiers(byte[] array)
        {
            int i=0;
            Links.Modifiers = WeaponModifer.GetList(array, ref i);
            
        }
        public static void GetArmors(byte[] array)
        {
            Links.Armors = ShipArmor.GetList(array);
        }
        public static void GetWeaponTypes(byte[] array)
        {
            Links.WeaponTypes = Weaponclass.GetList(array);
        }
        public static void GetEquipmentTypes(byte[] array)
        {
            Links.EquipmentTypes = Equipmentclass.GetList(array);
        }
        public static void GetPlayerSciences(byte[] array)
        {
            GSGameInfo.SciencesArray = PlayerSciencesList.GetSciencesArray(array);
            GSGameInfo.MaxLandsCount = GSGameInfo.BasicMaxLandsCount;
            GSGameInfo.MaxScienceLevel = 0;
            foreach (ushort scienceid in GSGameInfo.SciencesArray)
            {
                if (Links.Science.NewLandsSciences.ContainsKey(scienceid))
                    GSGameInfo.MaxLandsCount++;
                GameScience science = Links.Science.GameSciences[scienceid];
                if (science.Level > GSGameInfo.MaxScienceLevel)
                    GSGameInfo.MaxScienceLevel = science.Level;
            }
        }
        public static byte[] GetArchive1Hash()
        {
            //DebugWindow.AddTB1("Get Hash Started \n");
            ByteMessage query = new ByteMessage(0, 5, null);
            ByteMessage HashInString = GetResponceToQuery(query, true, "Important info");
            //DebugWindow.AddTB1("Get Hash Ended \n");
            if (HashInString.Type == MessageResult.Yes)
                return HashInString.Message;
            else
                return null;
        }
       
        public static void GetBasicBuildings(byte[] array)
        {
            Links.Buildings = GSBuilding.GetList(array);
        }
        public static void GetArtefacts(byte[] array)
        {
            Links.Artefacts = Artefact.GetList();
        }
        public static Battle GetBattle(long battleID)
        {
            if (Links.GameMode == EGameMode.Multi)
            {
                ByteMessage response = new ByteMessage(Links.Loginproxy.GetBattleBasicInfo(BitConverter.GetBytes(battleID)));
                if (response.Type == MessageResult.Yes)
                {
                    GetBattles(response.Message);
                    return (GSGameInfo.Battles[battleID]);
                }
                else
                {
                    Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Common.GetStringFromByteArray(response.Message, 0)), true);
                    return null;
                }
            }
            else
            {
                GetBattles(new ByteMessage(LocalServer.GetBattleBasicInfo(BitConverter.GetBytes(battleID))).Message);
                return GSGameInfo.Battles[battleID];
            }
        }
        public static ImageBrush AddPicColony(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Colony/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Colony/{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush AddPicColonyShort(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Colony2/Short/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Colony2/Short/{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush AddPic(string text)
        {
            if (Links.LoadImageFromDLL)
               return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Basic/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Basic/{0}.png", text), UriKind.Relative)));
            //Links.PicsList.Add(text, new BitmapImage(new Uri(String.Format("Images//{0}.png", text), UriKind.Relative)));
        }
       // static void AddPicB(string text)
        //{
       //     Links.PicsList.Add("Back" + text, new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Backs/{0}.jpg", text))));
            //Links.PicsList.Add("Back"+text, new BitmapImage(new Uri(String.Format("Images//Backs//{0}.jpg", text), UriKind.Relative)));
       // }
        public static ImageBrush AddPicB2(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Backs/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Backs/{0}.png", text), UriKind.Relative)));
            //Links.PicsList.Add(text, new BitmapImage(new Uri(String.Format("Images//Backs//{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush AddPicP(int i)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Planets/P_{0}.png", i))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Planets/P_{0}.png", i),UriKind.Relative)));
            //Links.PicsList.Add(string.Format("P_{0:00}", i), new BitmapImage(new Uri(String.Format("Images//planets//P_{0}.png", i), UriKind.Relative)));
        }
        //static void AddPicLand(int i)
       // {
        //    Links.PicsList.Add(string.Format("LandL{0}", i), new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Texture/LandL{0}.png", i))));
            //Links.PicsList.Add(string.Format("LandL{0}", i), new BitmapImage(new Uri(String.Format("Images//Texture//LandL{0}.png", i), UriKind.Relative)));
       // }
       public static ImageBrush AddPicArt(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Artefact/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Artefact/{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush AddPicI(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Interface/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Interface/{0}.png", text), UriKind.Relative)));
            //Links.PicsList.Add(text, new BitmapImage(new Uri(String.Format("Images//Interface//{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush AddPicGI(string text)
        {
            if (Links.LoadImageFromDLL)
               return new ImageBrush( new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Items/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Items/{0}.png", text), UriKind.Relative)));
            //Links.PicsList.Add(text, new BitmapImage(new Uri(String.Format("Images//Items//{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush AddLoginPic(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Login/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Login/{0}.png", text), UriKind.Relative)));
            //Links.PicsList.Add(text, new BitmapImage(new Uri(String.Format("Images//Science//{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush GetHelpPic(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Help/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Help/{0}.png", text), UriKind.Relative)));

        }
        public static ImageBrush AddPicScience(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Science/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Science/{0}.png", text),UriKind.Relative)));
            //Links.PicsList.Add(text, new BitmapImage(new Uri(String.Format("Images//Science//{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush AddSectorCreate(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/SectorCreate/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/SectorCreate/{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush AddAvanpostImage(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/AvanpostPanel/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/AvanpostPanel/{0}.png", text), UriKind.Relative)));
        }
        public static SortedList<byte, ImageBrush> GetBasicMeteorits()
        {
            SortedList<byte, ImageBrush> list = new SortedList<byte, ImageBrush>();
            list.Add(200, AddPicMeteor("M001_M1"));
            list.Add(201, AddPicMeteor("M002_M2"));
            list.Add(202, AddPicMeteor("M003_M3"));
            list.Add(203, AddPicMeteor("M004_M4"));
            list.Add(204, AddPicMeteor("M005_M5"));
            list.Add(205, AddPicMeteor("M006_M6"));
            list.Add(206, AddPicMeteor("M007_M7"));
            list.Add(207, AddPicMeteor("M008_M8"));
            list.Add(208, AddPicMeteor("M009_M9"));
            list.Add(209, AddPicMeteor("M010_M10"));
            list.Add(230, AddPicMeteor("M031_M31"));
            return list;
        }
        public static ImageBrush GetBrushInt(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Int/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Int/{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush AddPicMeteor(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Ships/Meteorit/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Ships/Meteorit/{0}.png", text), UriKind.Relative)));
            //Links.PicsList.Add(text, new BitmapImage(new Uri(String.Format("Images//Ships//Meteorit//{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush AddPicMission(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/MissionImages/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/MissionImages/{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush AddPicColony2(string name)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Colony2/{0}.png", name))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Colony2/{0}.png", name), UriKind.Relative)));
        }
        public static ImageBrush AddPicShip(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Ships/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Ships/{0}.png", text), UriKind.Relative)));
            //Links.PicsList.Add(text, new BitmapImage(new Uri(String.Format("Images//Ships//{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush AddShipPic(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Ships/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Ships/{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush AddPicShipM6(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Ships/Model6/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Ships/Model6/{0}.png", text), UriKind.Relative)));
            //Links.PicsList.Add(text, new BitmapImage(new Uri(String.Format("Images//Ships//Model6//{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush AddPicShipM7(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Ships/Model7/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Ships/Model7/{0}.png", text), UriKind.Relative)));
            //Links.PicsList.Add(text, new BitmapImage(new Uri(String.Format("Images//Ships//Model7//{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush AddPicShipM9(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Ships/Model9/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Ships/Model9/{0}.png", text), UriKind.Relative)));
            //Links.PicsList.Add(text, new BitmapImage(new Uri(String.Format("Images//Ships//Model9//{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush AddPortalPic(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Ships/Portal/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Ships/Portal/{0}.png", text), UriKind.Relative)));
            //Links.PicsList.Add(text, new BitmapImage(new Uri(String.Format("Images//Ships//Portal//{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush AddGunIcon(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Guns/Icons/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Guns/Icons/{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush AddGun(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Guns/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Guns/{0}.png", text), UriKind.Relative)));
            //Links.PicsList.Add(text, new BitmapImage(new Uri(String.Format("Images//Guns//{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush AddGCB(string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/GlobalCanvasButton/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/GlobalCanvasButton/{0}.png", text), UriKind.Relative)));
            //Links.PicsList.Add(text, new BitmapImage(new Uri(String.Format("Images//GlobalCanvasButton//{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush GetBuildingBrush (string text)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/Build/{0}.png", text))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/Build/{0}.png", text), UriKind.Relative)));
        }
        public static ImageBrush GetIntBoyaImage(string brush)
        {
            if (Links.LoadImageFromDLL)
                return new ImageBrush(new BitmapImage(new Uri(String.Format("pack://application:,,,/GraphicLibrary;component/Images/IB/{0}.png", brush))));
            else
                return new ImageBrush(new BitmapImage(new Uri(String.Format("Images/IB/{0}.png", brush),UriKind.Relative)));
        }
        public static void GetPics()
        {
            
            
        }
        public static bool LoadArchive1()
        {

            byte[] FileArray = Links.Controller.loadWindow.LoadArchive().Message;
            //byte[] FileArray = Links.Loginproxy.LoadFile(BitConverter.GetBytes(GSGameInfo.random), Links.GameData.GameData1);
            if (FileArray == null)
                return false;
            System.IO.File.WriteAllBytes(Links.GameData.GameData1, FileArray);
            return true;
        }
    }
}
