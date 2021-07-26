using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public partial class GSPlayer
    {
        public GSError RemoveArtefactFromFleet(int landid, long fleetid, ushort artefactid)
        {
            if (Lands.ContainsKey(landid) == false) return new GSError(7, 1);
            ServerLand land = Lands[landid];
            if (land.Fleets.ContainsKey(fleetid) == false) return new GSError(7, 3);
            ServerFleet fleet = land.Fleets[fleetid];
            if (ServerLinks.Artefacts.ContainsKey(artefactid) == false) return new GSError(10, 1);
            Artefact artefact = ServerLinks.Artefacts[artefactid];
            if (fleet.RemoveArtefact(artefact) == false) return new GSError(7, 24);
            if (Artefacts.ContainsKey(artefact) == true) Artefacts[artefact]++; else Artefacts.Add(artefact, 1);
            CreateArtefactsArray();
            land.FleetsArray = land.GetFleetsArray();
            GetFleetsArray();
            CreateArrayForPlayer();
            return new GSError(7, 0);
        }
        public GSError PlaceArtefactInFleet(int landid, long fleetid, ushort artefactid)
        {
            if (Lands.ContainsKey(landid)==false) return new GSError(7, 1);
            ServerLand land = Lands[landid];
            if (land.Fleets.ContainsKey(fleetid) == false) return new GSError(7, 3);
            ServerFleet fleet = land.Fleets[fleetid];
            if (ServerLinks.Artefacts.ContainsKey(artefactid) == false) return new GSError(10, 1);
            Artefact artefact = ServerLinks.Artefacts[artefactid];
            if (Artefacts.ContainsKey(artefact) == false) return new GSError(10, 2);
            if (fleet.AddArtefact(artefact) == false) return new GSError(7, 24);
            if (Artefacts[artefact] == 1) Artefacts.Remove(artefact); else Artefacts[artefact]--;
            CreateArtefactsArray();
            land.FleetsArray = land.GetFleetsArray();
            GetFleetsArray();
            CreateArrayForPlayer();
            return new GSError(7, 0);
        }
        public ArtefactUseResult UsePeaceArtefact(ushort ArtefactID, byte[] parameters)
        {
            if (ServerLinks.Artefacts.ContainsKey(ArtefactID)==false) return new ArtefactUseResult(false, 1);
            Artefact artefact = ServerLinks.Artefacts[ArtefactID];
            if (Artefacts.ContainsKey(artefact) == false) return new ArtefactUseResult(false, 2);
            if (artefact.Type != ArtefactType.Peace) return new ArtefactUseResult(false, 3);
            ArtefactUseResult result;
            switch  (artefact.Ability)
            {
                case ArtefactAbility.PResourceAdd: //Малое заполнение складов
                    int value = ServerLinks.BattleRandom.Next(artefact.Param1-5, artefact.Param1+5);
                    Resources.FillWarehouse(value / 100.0, value / 100.0, value / 100.0);
                    result = new ArtefactUseResult(true, value);
                    break;
                case ArtefactAbility.PPeopleClone: //Население +20
                    if (parameters.Length!=6)
                    { result = new ArtefactUseResult(false, 5); break; }
                    int landID = BitConverter.ToInt32(parameters, 2);
                    if (Lands.ContainsKey(landID)==false) { result = new ArtefactUseResult(false, 6); break; }
                    ServerLand land = Lands[landID];
                    double addresult = land.AddPeoples(artefact.Param1);
                    result = new ArtefactUseResult(true, (int)(addresult * 10));
                    break;
                case ArtefactAbility.PSectorRemove://Очистить сектор
                    if (parameters.Length!=7)
                    { result = new ArtefactUseResult(false, 5); break;}
                    int landID2 = BitConverter.ToInt32(parameters, 2);
                    if (Lands.ContainsKey(landID2) == false) { result = new ArtefactUseResult(false, 6); break; }
                    ServerLand land2 = Lands[landID2];
                    byte sectorpos = parameters[6];
                    if (land2.Locations.Length < sectorpos) { result = new ArtefactUseResult(false, 7); break; }
                    bool clearresult = land2.ClearSector(sectorpos);
                    if (clearresult==false) { result = new ArtefactUseResult(false, 8); break; }
                    result = new ArtefactUseResult(true, 0);
                    break;
                case ArtefactAbility.PsectorCreate: //Добавить сектор
                    if (parameters.Length != 6)
                    { result = new ArtefactUseResult(false, 5); break; }
                    int landID3 = BitConverter.ToInt32(parameters, 2);
                    if (Lands.ContainsKey(landID3) == false) { result = new ArtefactUseResult(false, 6); break; }
                    ServerLand land3 = Lands[landID3];
                    bool addsectorresult = land3.AddSector();
                    if (addsectorresult==false) { result = new ArtefactUseResult(false, 9); break; }
                    result = new ArtefactUseResult(true, 0);
                    break;
                case ArtefactAbility.PColonyCreate: //Достроить аванпост
                    if (parameters.Length!=6) { result = new ArtefactUseResult(false, 5); break; }
                    int avanpostid = BitConverter.ToInt32(parameters, 2);
                    if (NewLands.ContainsKey(avanpostid) == false) { result = new ArtefactUseResult(false, 6); break; }
                    ServerAvanpost avanpost = NewLands[avanpostid];
                    if (avanpost.Planet.MaxPopulation>artefact.Param1) { result = new ArtefactUseResult(false, 6); break; }
                    NewLands.Remove(avanpostid);
                    avanpost.Planet.NewLands.Remove(avanpostid);
                    ServerLand.FinishAvanpost(avanpost);
                    CreateCryptLand();
                    CreateNewLandsArray();
                    CreateArrayForPlayer();
                    result = new ArtefactUseResult(true, 0);
                    break;

                default: throw new Exception();
            }
            if (result.Result==true)
            {
                Artefacts[artefact]--;
                if (Artefacts[artefact] == 0) Artefacts.Remove(artefact);
                CreateArtefactsArray();
                CreateArrayForPlayer();
            }
            return result;
        }
        public GSError FinishStoryLineMissionText()
        {
            if (StoryLinePosition[0] >= StoryLine2.StoryLines.Count) return new GSError(7, 20); //Ошибка при выборе миссии
            StoryLine2 story = StoryLine2.StoryLines[StoryLinePosition[0]];
            if (story.StoryType != StoryType.Text) return new GSError(7, 20);
            RecieveStoryReward(story, null);
            if (story.RemoveQuestBlocks!= null)
            {
                foreach (int planetid in story.RemoveQuestBlocks)
                {
                    ServerLinks.GSPlanets[planetid].QuestPlanet = false;
                    Links.Planets[planetid].QuestPlanet = false;
                }
            }
            StoryLinePosition[0]++; StoryLinePosition[1] = 0;
            CreateArrayForPlayer();
            return new GSError(7, 0);
        }
        public void RecieveMissionReward(Reward2 reward, Mission2Type type, ServerFleet fleet)
        {
            if (reward != null)
            {
                Resources.AddResources(reward);
                if (reward.Sciences != null && reward.Sciences.Length > 0)
                {
                    foreach (ushort scienceid in reward.Sciences)
                        Sciences.PutScienceToArray(scienceid);
                }
            }
            if (reward.BonusShip != null)
            {
                ServerShip servership = ServerShip.GetNewShip(reward.BonusShip.Schema, BitConverter.GetBytes(reward.BonusShip.Model));
                servership.Pilot = new GSPilot(reward.BonusShip.Pilot.GetArray(), 0);
                AddShip(servership);
            }
            if (fleet != null && reward != null && reward.Experience != 0)
            {
                GSPilot.DistributeExpirience(fleet, reward.Experience);
            }
            if (reward.AvanpostID != -1)
            {
                AddAvanpost(reward.AvanpostID, true, null);
            }
            if (reward.LandID != -1)
            {
                AddLand(reward.LandID, null);
            }
            if (type!=Mission2Type.No)
                Notice.GetRecieveRewardMissionNotice(reward, type);
        }
        void RecieveStoryReward(StoryLine2 story, ServerFleet fleet)
        {
            if (story.Reward != null)
            {
                Resources.AddResources(story.Reward);
                if (story.Reward.Sciences!=null && story.Reward.Sciences.Length>0)
                {
                    foreach (ushort scienceid in story.Reward.Sciences)
                        Sciences.PutScienceToArray(scienceid);
                }
            }
            if (story.Reward.BonusShip!=null)
            {
                ServerShip servership = ServerShip.GetNewShip(story.Reward.BonusShip.Schema, BitConverter.GetBytes(story.Reward.BonusShip.Model));
                servership.Pilot = new GSPilot(story.Reward.BonusShip.Pilot.GetArray(), 0);
                AddShip(servership);
            }
            if (fleet!= null && story.Reward!= null && story.Reward.Experience!=0)
            {
                GSPilot.DistributeExpirience(fleet, story.Reward.Experience);
            }
            if (story.Reward.AvanpostID!=-1)
            {
                AddAvanpost(story.Reward.AvanpostID, true, null);
            }
            if (story.Reward.LandID!=-1)
            {
                AddLand(story.Reward.LandID, null);
            }
            Notice.GetRecieceRewardNotice(story.Reward);
        }
        public GSError StartStoryLineMissionBattle(int landID, long fleetID, out long battleid)
        {
            battleid = -1;
            if (!Lands.ContainsKey(landID)) return new GSError(7, 1);
            ServerLand land = Lands[landID];
            if (!land.Fleets.ContainsKey(fleetID)) return new GSError(7, 3);
            ServerFleet fleet = land.Fleets[fleetID];
            if (StoryLinePosition[0] >= StoryLine2.StoryLines.Count) return new GSError(7, 20); //
            if (StoryLinePosition[1] == 1) return new GSError(7, 21);
            if (fleet.CheckHealth() == false) return new GSError(7, 13);
            if (StoryLinePosition[0] >= StoryLine2.StoryLines.Count) return new GSError(7, 20);
            if (StoryLinePosition[1] == 1) return new GSError(7, 21);
            StoryLine2 story = StoryLine2.StoryLines[StoryLinePosition[0]];
            if (fleet.Ships.Count > story.BattleFieldParam.MaxShips) return new GSError(7, 13);
            StoryLinePosition[1] = 1; //миссия начата
            fleet.Target = new ServerFleetTarget(story, fleet, FleetMission.StoryLine);
            ServerBattle battle = ServerBattle.CreateStoryLineMissionBattle(fleet, story);
            battleid = battle.ID;
            //Notice.GetStoryLineStartNotice(fleet, battle, story.ID);
            AddBattle(battle);
            FleetInBattle(fleet);

            return new GSError(8, 0);
        }
        public GSError StartMission2Battle(int landID, long fleetID, int starID, byte orbit, out long battleid)
        {
            battleid = -1;
            if (!Lands.ContainsKey(landID)) return new GSError(7, 1);
            ServerLand land = Lands[landID];
            if (!land.Fleets.ContainsKey(fleetID)) return new GSError(7, 3);
            ServerFleet fleet = land.Fleets[fleetID];
            if (fleet.CheckHealth() == false) return new GSError(7, 13);
            if (ServerLinks.GSStars.ContainsKey(starID) == false) return new GSError(7, 25);
            ServerStar star = ServerLinks.GSStars[starID];
            if (star.MissionInspector.Missions.ContainsKey(orbit) == false)
                return new GSError(7, 20);
            ServerMission2 mission = star.MissionInspector.Missions[orbit];
            double distance = Math.Sqrt(Math.Pow(star.X - mission.Star.X, 2) + Math.Pow(star.Y - mission.Star.Y, 2)) / 10;
            if (distance > fleet.FleetBase.Range) return new GSError(7, 23);
            if (fleet.Ships.Count > mission.MaxShips) return new GSError(7, 13);
            ServerMission2Result result = star.MissionInspector.StartMission(orbit, fleet);
            CreateMission2Array();
            switch (result.Content)
            {
                case Mission2Content.BattleMedium:
                case Mission2Content.BattleHigh:
                    battleid = result.Battle.ID;
                    FleetInBattle(fleet);
                    return new GSError(7, 0);
                case Mission2Content.Empty:
                    battleid = -1;
                    fleet.Target.FreeTime = ServerLinks.NowTime + TimeSpan.FromDays(1);
                    break;
                case Mission2Content.FreeLow:
                    battleid = -2;
                    RecieveMissionReward(result.Reward, mission.Type, null);
                    fleet.Target.FreeTime = ServerLinks.NowTime + TimeSpan.FromDays(3);
                    break;
                case Mission2Content.FreeMedium:
                    battleid = -3;
                    RecieveMissionReward(result.Reward, mission.Type, null);
                    fleet.Target.FreeTime = ServerLinks.NowTime + TimeSpan.FromDays(5);
                    break;
                case Mission2Content.BlackMarket:
                    battleid = -4;
                    BlackMarket = new ServerMarket(this, MarketType.Black);
                    BlackMarket.Fleet = fleet;
                    FleetInBattle(fleet);
                    return new GSError(7, 0);

            }
            fleet.Target.Order = FleetOrder.Return;
            GSTimer.AddFleetToQueue(fleet);
            FleetInBattle(fleet);
            return new GSError(7, 0);
        }
        public GSError StartResourceMissionBattle(int landID, long fleetID, byte missiontype, ushort missionid, out long battleid)
        {
            throw new Exception();
            /*
            battleid = -1;
            if (!Lands.ContainsKey(landID)) return new GSError(7, 1);
            ServerLand land = Lands[landID];
            if (!land.Fleets.ContainsKey(fleetID)) return new GSError(7, 3);
            ServerFleet fleet = land.Fleets[fleetID];
            int fleethealth = fleet.GetHealth();
            if (fleethealth <= 0) return new GSError(7, 13);
            if (fleet.Mission != FleetMission.Free) return new GSError(7, 5);
            MissionType type = (MissionType)missiontype;
            if (type == MissionType.Competition && fleet.Ships.Count > 5) return new GSError(7, 13);
            if (type == MissionType.BigCompetition && fleet.Ships.Count > 10) return new GSError(7, 13);
            Mission mission = Missions.GetMission(missiontype, missionid);
            if (mission == null) return new GSError(7, 20);
            if (mission.IsStarted) return new GSError(7, 21);
            mission.IsStarted = true;
            fleet.Target = new ServerFleetTarget(ServerLinks.GSStars[mission.StarID], land.Player);
            fleet.Target.Speed = 5; fleet.Target.Health = fleethealth;
            fleet.Mission = FleetMission.Resources;
            EnemyType enemy;
            ServerBattle battle = ServerBattle.CreateResourceMissionBattle(fleet, mission, out enemy);
            battleid = battle.ID;
            switch (enemy)
            {
                case EnemyType.Pirates: Notice.GetPirateAttackNotice(fleet, battle); break;
                case EnemyType.GreenParty: Notice.GetGreenTeamBattleStartNotice(fleet, battle); break;
                case EnemyType.Aliens: Notice.GetAlienBattleStartNotice(fleet, battle); break;
                case EnemyType.Techno: Notice.GetTechnoTeamBattleStartNotice(fleet, battle); break;
                case EnemyType.Mercenaries: Notice.GetMercBattleStartNotice(fleet, battle); break;
            }
            AddBattle(battle);
            if (!Quest.AllDone) Quest.ReasonFinish(Reason_Finish.Fleet_Sended);
            FleetInBattle(fleet);
            return new GSError(8, 0);
            */
        }
        public GSError AttackEnemyColonyCaptureEvent(int fleetland, long fleetID, int targetland, out long battleid)
        {
            throw new Exception();
            /*
            battleid = -1;
            if (!Lands.ContainsKey(fleetland)) return new GSError(7, 1);
            ServerLand land = Lands[fleetland];
            TargetLand target;
            if (ServerLinks.GSLands.ContainsKey(targetland))
                target = ServerLinks.GSLands[targetland];
            else if (ServerLinks.GSNewLands.ContainsKey(targetland))
                target = ServerLinks.GSNewLands[targetland];
            else if (ServerLinks.GSNPCLands.ContainsKey(targetland))
                target = ServerLinks.GSNPCLands[targetland];
            else return new GSError(7, 17);
            if (target.GetPlanet().LandsCount < 1) return new GSError(7, 18);
            if (Clan != null && target.GetPlayer() != null && target.GetPlayer().Clan == Clan) return new GSError(7, 18);
            if (!land.Fleets.ContainsKey(fleetID)) return new GSError(7, 3);
            ServerFleet fleet = land.Fleets[fleetID];
            int fleethealth = fleet.GetHealth();
            if (fleethealth <= 0) return new GSError(7, 13);
            if (fleet.Mission != FleetMission.Free) return new GSError(7, 5);
            if (!fleet.CheckReadyForColony(target)) return new GSError(7, 16);
            fleet.Target = new ServerFleetTarget(target.GetFleetTarget(), land.Player);
            fleet.Target.Health = fleethealth; fleet.Target.Speed = 5;
            fleet.Mission = FleetMission.ColonizeLand;
            battleid = AttackEnemyColonyCapture(fleet, target, true, 0);
            return new GSError(7, 0);
            */
        }
        public GSError RemoveFleetFromLandDefender(int fleetland, long fleetID)
        {
            throw new Exception();
            /*
            if (!Lands.ContainsKey(fleetland)) return new GSError(7, 1);
            ServerLand land = Lands[fleetland];
            if (!land.Fleets.ContainsKey(fleetID)) return new GSError(7, 3);
            ServerFleet fleet = land.Fleets[fleetID];
            if (fleet.FleetBase.Land.FleetDefender != fleet) return new GSError(7, 3);
            if (fleet.FreeTime.Ticks != 2) return new GSError(7, 19);
            fleet.FleetBase.Land.FleetDefender = null;
            RemoveFleetFromLandDefender(fleet);
            return new GSError(7, 0);
            */
        }
        public GSError SetFleetAsLastDefenderPlayer(int fleetland, long fleetID)
        {
            throw new Exception();
            /*
            if (!Lands.ContainsKey(fleetland)) return new GSError(7, 1);
            ServerLand land = Lands[fleetland];
            if (!land.Fleets.ContainsKey(fleetID)) return new GSError(7, 3);
            ServerFleet fleet = land.Fleets[fleetID];
            if (fleet.GetHealth() <= 0) return new GSError(7, 13);
            if (fleet.Mission != FleetMission.Free) return new GSError(7, 5);
            if (fleet.FleetBase.Land.FleetDefender != null)
                if (fleet.FleetBase.Land.FleetDefender.FreeTime.Ticks != 2)
                    return new GSError(7, 19);
                else
                    RemoveFleetFromLandDefender(fleet.FleetBase.Land.FleetDefender);
            SendFleetAsLastDefender(fleet);
            return new GSError(7, 0);
            */
        }
        public void SendFleetAsLastDefender(ServerFleet fleet)
        {
            throw new Exception();
            /*
            fleet.Mission = FleetMission.LandDefender;
            fleet.Target = new ServerFleetTarget(fleet.FleetBase.Land, this);
            fleet.FreeTime = new DateTime(2);
            fleet.FleetBase.Land.FleetDefender = fleet;
            FleetInBattle(fleet);
            */
        }
        public void RemoveFleetFromLandDefender(ServerFleet fleet)
        {
            throw new Exception();
            /*
            fleet.FreeTime = DateTime.Now + ServerLinks.Parameters.BaseFleetWaitTimeWin;
            fleet.Mission = FleetMission.LandDefenderReturn;
            GSTimer.AddFleetToQueue(fleet);
            FleetInBattle(fleet);
            */
        }
        public GSError StartConquerEvent(int fleetland, long fleetID, int targetplanet, out long battleid)
        {
            battleid = -1;
            if (!Lands.ContainsKey(fleetland)) return new GSError(7, 1);
            ServerLand land = Lands[fleetland];
            if (!land.Fleets.ContainsKey(fleetID)) return new GSError(7, 3);
            ServerFleet fleet = land.Fleets[fleetID];
            if (fleet.CheckHealth() == false) return new GSError(7, 13);
            if (fleet.GetActiveShips() < 7) return new GSError(7, 16);
            if (!ServerLinks.GSPlanets.ContainsKey(targetplanet)) return new GSError(7, 25);
            ServerPlanet planet = ServerLinks.GSPlanets[targetplanet];
            ServerStar star2 = planet.Star;
            ServerStar star1 = fleet.FleetBase.Land.Planet.Star;
            double distance = Math.Sqrt(Math.Pow(star1.X - star2.X, 2) + Math.Pow(star1.Y - star2.Y, 2)) / 10;
            if (distance > fleet.FleetBase.Range) return new GSError(7, 23);
            if (planet.MaxPopulation <= 0) return new GSError(7, 28);

            if (planet.Lands.Count + planet.NewLands.Count == 0) return new GSError(7, 28);
            TargetLand target;
            if (planet.Lands.Count > 0) target = planet.Lands.Values[0]; else target = planet.NewLands.Values[0];
            if (target.GetPlayer() == fleet.FleetBase.Land.Player) return new GSError(7, 28);
            fleet.Target = new ServerFleetTarget(planet, fleet, FleetMission.Conquer);
            fleet.Target.TargetPlayer = target.GetPlayer();

            battleid = StartConquer(fleet);
            return new GSError(7, 0);
        }
        public GSError StartPillageEvent(int fleetland, long fleetID, int targetplanet, out long battleid)
        {
            battleid = -1;
            if (!Lands.ContainsKey(fleetland)) return new GSError(7, 1);
            ServerLand land = Lands[fleetland];
            if (!land.Fleets.ContainsKey(fleetID)) return new GSError(7, 3);
            ServerFleet fleet = land.Fleets[fleetID];
            if (fleet.CheckHealth() == false) return new GSError(7, 13);
            if (fleet.GetActiveShips() < 5) return new GSError(7, 16);
            if (!ServerLinks.GSPlanets.ContainsKey(targetplanet)) return new GSError(7, 25);
            ServerPlanet planet = ServerLinks.GSPlanets[targetplanet];
            ServerStar star2 = planet.Star;
            ServerStar star1 = fleet.FleetBase.Land.Planet.Star;
            double distance = Math.Sqrt(Math.Pow(star1.X - star2.X, 2) + Math.Pow(star1.Y - star2.Y, 2)) / 10;
            if (distance > fleet.FleetBase.Range) return new GSError(7, 23);
            if (planet.MaxPopulation <= 0) return new GSError(7, 28);

            if (planet.Lands.Count + planet.NewLands.Count == 0) return new GSError(7, 28);
            TargetLand target;
            if (planet.Lands.Count > 0) target = planet.Lands.Values[0]; else target = planet.NewLands.Values[0];
            if (target.GetPlayer() == fleet.FleetBase.Land.Player) return new GSError(7, 28);
            fleet.Target = new ServerFleetTarget(planet, fleet, FleetMission.Pillage);
            fleet.Target.TargetPlayer = target.GetPlayer();

            battleid = StartPillage(fleet);
            return new GSError(7, 0);
            
        }
        long StartScout(ServerFleet fleet)
        {
            ServerStar star = fleet.Target.GetStar();
            ServerBattle scoutbattle = star.StartPatrolBattle(fleet);
            if (scoutbattle != null)
                return scoutbattle.ID;
            bool HaveTargetInBattle = false;
            foreach (ServerPlanet planet in star.Planets.Values)
            {
                if (planet.Lands.Count>0)
                {
                    ServerLand land = planet.Lands.Values[0];
                    if (land.Player != fleet.Target.SelfPlayer)
                        continue;
                    if (land.ConquerFleet!=null)
                    {
                        if (land.ConquerFleet.Target.Order==FleetOrder.InBattle) { HaveTargetInBattle = true;  continue; }
                        ServerBattle battle = new ServerBattle(fleet, fleet.Target.SelfPlayer.MainPlayer == false, land.ConquerFleet,
                        land.ConquerFleet.Target.SelfPlayer.MainPlayer == false, BattleType.Planet, new BattleFieldGroup(BattleField.Fields[0], new SortedList<byte, ServerShipB>(), new byte[2]));
                        fleet.Target.SelfPlayer.AddBattle(battle);
                        land.ConquerFleet.Target.SelfPlayer.AddBattle(battle);
                        land.ConquerFleet.Target.SelfPlayer.FleetInBattle(land.ConquerFleet);
                        return battle.ID;
                    }
                }
                if (planet.NewLands.Count>0)
                {
                    ServerAvanpost avanpost = planet.NewLands.Values[0];
                    if (avanpost.Player != fleet.Target.SelfPlayer) continue;
                    if (avanpost.ConquerFleet != null)
                    {
                        if (avanpost.ConquerFleet.Target.Order == FleetOrder.InBattle) { HaveTargetInBattle = true; continue; }
                        ServerBattle battle = new ServerBattle(fleet, fleet.Target.SelfPlayer.MainPlayer == false, avanpost.ConquerFleet,
                        avanpost.ConquerFleet.Target.SelfPlayer.MainPlayer == false, BattleType.Planet, new BattleFieldGroup(BattleField.Fields[0], new SortedList<byte, ServerShipB>(), new byte[2]));
                        fleet.Target.SelfPlayer.AddBattle(battle);
                        avanpost.ConquerFleet.Target.SelfPlayer.AddBattle(battle);
                        avanpost.ConquerFleet.Target.SelfPlayer.FleetInBattle(avanpost.ConquerFleet);
                        return battle.ID;
                    }
                }
                if (planet.Lands.Count > 0)
                {
                    ServerLand land = planet.Lands.Values[0];
                    if (land.Player != fleet.Target.SelfPlayer)
                        continue;
                    if (land.PillageFleet != null)
                    {
                        if (land.PillageFleet.Target.Order == FleetOrder.InBattle) { HaveTargetInBattle = true; continue; }
                        ServerBattle battle = new ServerBattle(fleet, fleet.Target.SelfPlayer.MainPlayer == false, land.PillageFleet,
                        land.PillageFleet.Target.SelfPlayer.MainPlayer == false, BattleType.Planet, new BattleFieldGroup(BattleField.Fields[0], new SortedList<byte, ServerShipB>(), new byte[2]));
                        fleet.Target.SelfPlayer.AddBattle(battle);
                        land.PillageFleet.Target.SelfPlayer.AddBattle(battle);
                        land.PillageFleet.Target.SelfPlayer.FleetInBattle(land.PillageFleet);
                        return battle.ID;
                    }
                }
                if (planet.NewLands.Count > 0)
                {
                    ServerAvanpost avanpost = planet.NewLands.Values[0];
                    if (avanpost.Player != fleet.Target.SelfPlayer) continue;
                    if (avanpost.PillageFleet != null)
                    {
                        if (avanpost.PillageFleet.Target.Order == FleetOrder.InBattle) { HaveTargetInBattle = true; continue; }
                        ServerBattle battle = new ServerBattle(fleet, fleet.Target.SelfPlayer.MainPlayer == false, avanpost.PillageFleet,
                        avanpost.PillageFleet.Target.SelfPlayer.MainPlayer == false, BattleType.Planet, new BattleFieldGroup(BattleField.Fields[0], new SortedList<byte, ServerShipB>(), new byte[2]));
                        fleet.Target.SelfPlayer.AddBattle(battle);
                        avanpost.PillageFleet.Target.SelfPlayer.AddBattle(battle);
                        avanpost.PillageFleet.Target.SelfPlayer.FleetInBattle(avanpost.PillageFleet);
                        return battle.ID;
                    }
                }
            }
            if (HaveTargetInBattle==true)
            {
                fleet.CalcFleetReturnTime(true);
                fleet.Target.Order = FleetOrder.Return;
                BattleEvents.FleetFly(fleet);
                return -1;
            }
            else
            {
                fleet.Target.Order = FleetOrder.Defense;
                CreateMission2Array();
                CreatePlanetInfo();
                return -1;
            }
        }
        /// <summary> Метод направляет флот на захват. Сначала проверяется готовность флота и цели, затем наличие патруля, затем имеющегося флота захвата</summary>
        long StartConquer(ServerFleet fleet)
        {
            if (fleet.GetActiveShips() < 7)
            {
                fleet.CalcFleetReturnTime(false);
                fleet.Target.Order = FleetOrder.Return;
                BattleEvents.FleetFly(fleet);
                FleetInBattle(fleet);
                return -1;
            }
            ServerPlanet planet = fleet.Target.GetPlanet();
            TargetLand target;
            if (planet.Lands.Count > 0) target = planet.Lands.Values[0]; else target = planet.NewLands.Values[0];
            if (target.GetPlayer() != fleet.Target.TargetPlayer) //Если принадлежность колонии изменилась - то флот возвращается домой
            {
                fleet.CalcFleetReturnTime(true);
                fleet.Target.Order = FleetOrder.Return;
                BattleEvents.FleetFly(fleet);
                FleetInBattle(fleet);
                return -1;
            }
            ServerFleet curconquer = target.GetConquerFleet();
            if (curconquer != fleet)//поиск и бой с патрулём. Патруль может быть любой стороны, кроме стороны самого флота
            {
                ServerBattle battle = planet.Star.StartPatrolBattle(fleet);
                if (battle != null)
                {
                    FleetInBattle(fleet);
                    return battle.ID;
                }
            }
            if (curconquer != null) //Если патруля нет, то бой с текущим флотом, грабящим колонию.
            {
                if (curconquer.Target.Order == FleetOrder.InBattle) //Если этот флот в бою
                {
                    if (curconquer.CurBattle == fleet.CurBattle)//Если это тот бой, который только что произошёл
                    {
                        fleet.Target.Order = FleetOrder.Defense;
                        FleetInBattle(fleet);
                        target.SetPillageFleet(fleet);
                        return -1;
                    }
                    else
                    {
                        fleet.CalcFleetReturnTime(true);
                        fleet.Target.Order = FleetOrder.Return;
                        BattleEvents.FleetFly(fleet);
                        FleetInBattle(fleet);
                        return -1;
                    }
                }
                ServerBattle battle = new ServerBattle(fleet, fleet.Target.SelfPlayer.MainPlayer == false, curconquer,
                    curconquer.Target.SelfPlayer.MainPlayer == false, BattleType.Planet, new BattleFieldGroup(BattleField.Fields[0], new SortedList<byte, ServerShipB>(), new byte[2]));
                fleet.Target.SelfPlayer.AddBattle(battle);
                curconquer.Target.SelfPlayer.AddBattle(battle);
                curconquer.Target.SelfPlayer.FleetInBattle(curconquer);
                FleetInBattle(fleet);
                return battle.ID;
            }
            else
            {
                fleet.Target.Order = FleetOrder.Defense;
                FleetInBattle(fleet);
                target.SetConquerFleet(fleet);
                return -1;
            }
        }
        /// <summary> Метод направляет флот на грабёж. Сначала проверяется готовность флота и цели, затем наличие патруля, затем имеющегося флота грабежа</summary>
        long StartPillage(ServerFleet fleet)
        {
            if (fleet.GetActiveShips() < 5)
            {
                fleet.CalcFleetReturnTime(false);
                fleet.Target.Order = FleetOrder.Return;
                BattleEvents.FleetFly(fleet);
                FleetInBattle(fleet);
                return -1;
            }
            ServerPlanet planet = fleet.Target.GetPlanet();
            TargetLand target;
            if (planet.Lands.Count > 0) target = planet.Lands.Values[0]; else target = planet.NewLands.Values[0];
            if (target.GetPlayer() != fleet.Target.TargetPlayer) //Если принадлежность колонии изменилась - то флот возвращается домой
            {
                fleet.CalcFleetReturnTime(true);
                fleet.Target.Order = FleetOrder.Return;
                BattleEvents.FleetFly(fleet);
                FleetInBattle(fleet);
                return -1;
            }
            ServerFleet curpillage = target.GetPillageFleet();
            if (curpillage != fleet)//поиск и бой с патрулём. Патруль может быть любой стороны, кроме стороны самого флота
            {
                ServerBattle battle = planet.Star.StartPatrolBattle(fleet);
                if (battle != null)
                {
                    FleetInBattle(fleet);
                    return battle.ID;
                }
            }
            if (curpillage!=null) //Если патруля нет, то бой с текущим флотом, грабящим колонию.
            {
                if (curpillage.Target.Order==FleetOrder.InBattle) //Если этот флот в бою
                {
                    if (curpillage.CurBattle == fleet.CurBattle)//Если это тот бой, который только что произошёл
                    {
                        fleet.Target.Order = FleetOrder.Defense;
                        FleetInBattle(fleet);
                        target.SetPillageFleet(fleet);
                        return -1;
                    }
                    else
                    {
                        fleet.CalcFleetReturnTime(true);
                        fleet.Target.Order = FleetOrder.Return;
                        BattleEvents.FleetFly(fleet);
                        FleetInBattle(fleet);
                        return -1;
                    }
                }
                ServerBattle battle = new ServerBattle(fleet, fleet.Target.SelfPlayer.MainPlayer == false, curpillage,
                    curpillage.Target.SelfPlayer.MainPlayer == false, BattleType.Planet, new BattleFieldGroup(BattleField.Fields[0], new SortedList<byte, ServerShipB>(), new byte[2]));
                fleet.Target.SelfPlayer.AddBattle(battle);
                curpillage.Target.SelfPlayer.AddBattle(battle);
                curpillage.Target.SelfPlayer.FleetInBattle(curpillage);
                FleetInBattle(fleet);
                return battle.ID;
            }
            else
            {
                fleet.Target.Order = FleetOrder.Defense;
                FleetInBattle(fleet);
                target.SetPillageFleet(fleet);
                return -1;
            }
        }
        public GSError RemoveFleet(int fleetland, long fleetID)
        {
            if (!Lands.ContainsKey(fleetland)) return new GSError(7, 1);
            ServerLand land = Lands[fleetland];
            if (!land.Fleets.ContainsKey(fleetID)) return new GSError(7, 3);
            ServerFleet fleet = land.Fleets[fleetID];
            if (fleet.Target== null) return new GSError(7, 26);
            if (fleet.Target.Order != FleetOrder.Defense) return new GSError(7, 27);
            if (fleet.Target.Mission==FleetMission.Scout)
            {
                ServerStar star = fleet.Target.GetStar();
                star.ScoutedFleets.Remove(fleet);
                fleet.Target.Order = FleetOrder.Return;
                CreateMission2Array();
            }
            else if (fleet.Target.Mission==FleetMission.Pillage)
            {
                ServerPlanet planet = fleet.Target.GetPlanet();
                TargetLand target = null;
                if (planet.Lands.Count > 0) target = planet.Lands.Values[0]; else if (planet.NewLands.Count > 0) target = planet.NewLands.Values[0];
                if (target != null)
                    target.SetPillageFleet(null);
            }
            fleet.CalcFleetReturnTime(false);
            fleet.Target.Order = FleetOrder.Return;
            BattleEvents.FleetFly(fleet);
            FleetInBattle(fleet); 
            CreatePlanetInfo();
            return new GSError(7, 0);
           
        }
        public GSError SendFleetToDefensePlayer(int fleetland, long fleetID)
        {
            throw new Exception();
            /*
            if (!Lands.ContainsKey(fleetland)) return new GSError(7, 1);
            ServerLand land = Lands[fleetland];
            if (!land.Fleets.ContainsKey(fleetID)) return new GSError(7, 3);
            ServerFleet fleet = land.Fleets[fleetID];
            if (fleet.GetHealth() <= 0) return new GSError(7, 13);
            if (fleet.Mission != FleetMission.Free) return new GSError(7, 5);
            SendFleetForDefense(fleet);
            return new GSError(7, 0);
            */
        }
        public void SendFleetForDefense(ServerFleet fleet)
        {
            throw new Exception();
            /*
            //ЕСЛИ У КОЛОНИИ ФЛОТА СМЕНИЛСЯ ХОЗЯИН - ЭТО ПРОВЕРЯЕТСЯ РАНЬШЕ
            fleet.Target = new ServerFleetTarget(fleet.FleetBase.Land, this);
            fleet.Mission = FleetMission.DefenseLand;
            fleet.FreeTime = new DateTime(2);
            DefenseFleets.Add(fleet);
            FleetInBattle(fleet);
            if (Clan != null) Clan.HasFleets = true;
            */
        }
        public GSError SendFleetToScout(int landID, long fleetID, int starID, out long battleid)
        {
            battleid = -1;
            if (!Lands.ContainsKey(landID)) return new GSError(7, 1);
            ServerLand land = Lands[landID];
            if (!land.Fleets.ContainsKey(fleetID)) return new GSError(7, 3);
            ServerFleet fleet = land.Fleets[fleetID];
            if (fleet.CheckHealth() == false) return new GSError(7, 13);
            if (!ServerLinks.GSStars.ContainsKey(starID)) return new GSError(7, 25);
            ServerStar star2 = ServerLinks.GSStars[starID];
            ServerStar star1 = fleet.FleetBase.Land.Planet.Star;
            double distance = Math.Sqrt(Math.Pow(star1.X - star2.X, 2) + Math.Pow(star1.Y - star2.Y, 2)) / 10;
            if (distance > fleet.FleetBase.Range) return new GSError(7, 23);
            fleet.Target = new ServerFleetTarget(star2, fleet, FleetMission.Scout);
            battleid = StartScout(fleet);
            /*ServerBattle battle = star2.StartPatrolBattle(fleet);
            if (battle != null)
                battleid = battle.ID;
            else
            {
                fleet.Target.Order = FleetOrder.Defense;
                CreateMission2Array();
                CreatePlanetInfo();
            }*/
            FleetInBattle(fleet);
            return new GSError(7, 0); 
        }

        public GSError StartColonizeLand(int planetID, out int avanpostid)
        {
            avanpostid = -1;
            if (!ServerLinks.GSPlanets.ContainsKey(planetID)) return new GSError(7, 8);
            ServerPlanet planet = ServerLinks.GSPlanets[planetID];
            if (planet.CheckColonizeLand(this) == false) return new GSError(7, 10);
            long avanpostprice = Common.GetAvanpostPrice(planet, this);
            if (Resources.RemovePrice(avanpostprice) == false) return new GSError(1, 4);
            ServerAvanpost newland = ServerAvanpost.GetAvanpost(planetID, ID);
            avanpostid = newland.ID;
            CreateNewLandsArray();
            CreatePlanetInfo();
            return new GSError(7, 0);
        }
        public GSError RemoveShipFromFleet(int landID, long fleetID, long ShipID)
        {
            if (!Lands.ContainsKey(landID)) return new GSError(7, 1); //если территория не принадлежит игроку то ошибка
            ServerLand land = Lands[landID];
            if (!land.Fleets.ContainsKey(fleetID)) return new GSError(7, 3); //если целевой флот не принадлежит территории то ошибка
            ServerFleet fleet = land.Fleets[fleetID]; //целевой флот
            if (!Ships.ContainsKey(ShipID)) return new GSError(7, 4);
            ServerShip ship = Ships[ShipID];
            if (ship.Fleet == null) return new GSError(7, 4);
            if (ship.Pilot == null) return new GSError(7, 7);
            if (fleet.Target != null) return new GSError(7, 5); //если целевой флот на задании то ошибка
            fleet.Ships.Remove(ShipID);
            ship.Fleet = null;
            FleetInBattle(fleet);
            return new GSError(7, 0);
        }
        public GSError ChangeFleetParams(int LandID, long FleetID, bool repair)
        {
            if (!Lands.ContainsKey(LandID))
                return new GSError(7, 1);
            ServerLand land = Lands[LandID];
            if (!land.Fleets.ContainsKey(FleetID))
                return new GSError(7, 3);
            ServerFleet fleet = land.Fleets[FleetID];
            if (fleet.Target != null)
                return new GSError(7, 5);
            fleet.Repair = repair; 
            FleetInBattle(fleet);
            fleet.Array = fleet.GetArray();
            land.FleetsArray = land.GetFleetsArray();
            GetFleetsArray();
            CreateArrayForPlayer();
            return new GSError(7, 0);
        }
        public GSError MoveShipToFleet(int landID, long fleetID, long ShipID)
        {
            if (!Lands.ContainsKey(landID)) return new GSError(7, 1); //если территория не принадлежит игроку то ошибка
            ServerLand land = Lands[landID];
            if (!land.Fleets.ContainsKey(fleetID)) return new GSError(7, 3); //если целевой флот не принадлежит территории то ошибка
            ServerFleet fleet = land.Fleets[fleetID]; //целевой флот
            if (!Ships.ContainsKey(ShipID)) return new GSError(7, 4);
            ServerShip ship = Ships[ShipID];
            if (ship.Fleet != null) return new GSError(7, 4);
            if (ship.Pilot == null) return new GSError(7, 7);
            //if (!land.CheckShipsCount(1)) return new GSError(7, 12); //если на планете избыток кораблей
            if (!fleet.IsHavePlace()) return new GSError(7, 13);//если во флоте нельзя разместить больше кораблей
            if (fleet.Target != null) return new GSError(7, 5); //если целевой флот на задании то ошибка
            fleet.Ships.Add(ShipID, ship);
            ship.Fleet = fleet;
            FleetInBattle(fleet);
            return new GSError(7, 0);
        }
        public GSError CreateFleet(int LandID, byte sectorpos, byte[] fleetimage, out ServerFleet fleet)
        {
            fleet = null;
            if (!Lands.ContainsKey(LandID))
                return new GSError(7, 1);
            ServerLand land = Lands[LandID];
            if (sectorpos > land.Planet.Locations 
                || land.Locations[sectorpos].Type != SectorTypes.War 
                || ((ServerFleetBase)land.Locations[sectorpos]).Fleet != null)
                return new GSError(7, 17);
            land.CreateFleet(fleetimage, sectorpos, out fleet);
            GetFleetsArray();
            CreateArrayForPlayer();
            return new GSError(7, 0);
        }
        public int GetFleetsCount()
        {
            int answer = 0;
            foreach (ServerLand land in Lands.Values)
                answer += land.Fleets.Count;
            return answer;
        }
        public GSError MovePilotFromShip(long shipID)
        {
            if (!Ships.ContainsKey(shipID)) return new GSError(4, 4);
            ServerShip ship = Ships[shipID];
            if (ship.Pilot.IsBasicPilot()) return new GSError(6, 6);
            if (ship.Health == 0) return new GSError(6, 7);
            if (ship.Fleet != null) return new GSError(6, 8);
            GSPilot pilot = ship.Pilot;
            Pilots.AddPilot(pilot);
            ship.Pilot = GSPilot.BasicPilot;
            CalculateShipArray();
            CreateArrayForPlayer();
            return new GSError(6, 0);
        }
        public GSError PutPilotToShip(long shipID, GSPilot pilot)
        {
            if (!Ships.ContainsKey(shipID)) return new GSError(4, 4);
            if (!Pilots.DismissPilot(pilot)) return new GSError(6, 6);
            Ships[shipID].Pilot = pilot;
            CalculateShipArray();
            CreateArrayForPlayer();
            return new GSError(6, 0);

        }
        public GSError DismissPilot(byte[] array)
        {
            if (array.Length != 10) return new GSError(6, 5);
            GSPilot pilot = GSPilot.GetPilot(array, 0);
            if (!Pilots.DismissPilot(pilot)) return new GSError(6, 3);
            CreateArrayForPlayer();
            return new GSError(6, 4);
        }
        public GSError HirePilot(byte pos)
        {
            if (pos >= Pilots.AcademyPilots.Length / 10) return new GSError(6, 3);
            Pilots.HirePilot(pos);
            CreateNewPilotsArray();
            if (!Quest.AllDone) Quest.ReasonFinish(Reason_Finish.Pilot_Hired);
            CreateArrayForPlayer();
            return new GSError(6, 2);
        }
        public GSError GenerateNewPilotGroup()
        {
            if (!Resources.RemovePrice(GSPilot.PilotChangePrice)) return new GSError(6, 1);
            Pilots.AcademyPilots = GSPilot.GetPilotGroup();
            CreateNewPilotsArray();
            CreateArrayForPlayer();
            return new GSError(6, 0);
        }
        public GSError ResourceExchange(byte target, int value)
        {
            long price;
            switch (target)
            {
                case 0://покупка металла
                    price = (long)(ServerLinks.Market.BuyMetal * value);
                    if (Resources._Money < price) return new GSError(5, 5);
                    Resources.ExchangeMetal(value, -price);
                    ServerLinks.Market.ChangeMetal(-value);
                    return new GSError(5, 0);
                case 1: //продажа металла
                    if (Resources._Metals < value) return new GSError(5, 1);
                    price = (long)(ServerLinks.Market.SellMetal * value);
                    Resources.ExchangeMetal(-value, price);
                    ServerLinks.Market.ChangeMetal(value);
                    return new GSError(5, 0);
                case 2:
                    price = (long)(ServerLinks.Market.BuyChips * value);
                    if (Resources._Money < price) return new GSError(5, 5);
                    Resources.ExchangeChips(value, -price);
                    ServerLinks.Market.ChangeChips(-value);
                    return new GSError(5, 0);
                case 3:
                    if (Resources._Chips < value) return new GSError(5, 2);
                    price = (long)(ServerLinks.Market.SellChips * value);
                    Resources.ExchangeChips(-value, price);
                    ServerLinks.Market.ChangeChips(value);
                    return new GSError(5, 0);
                case 4:
                    price = (long)(ServerLinks.Market.BuyAnti * value);
                    if (Resources._Money < price) return new GSError(5, 5);
                    Resources.ExchangeAnti(value, -price);
                    ServerLinks.Market.ChangeAnti(-value);
                    return new GSError(5, 0);
                case 5:
                    if (Resources._Anti < value) return new GSError(5, 3);
                    price = (long)(ServerLinks.Market.SellAnti * value);
                    Resources.ExchangeAnti(-value, price);
                    ServerLinks.Market.ChangeAnti(value);
                    return new GSError(5, 0);
            }
            return new GSError(5, 6);
        }
        public GSError BuildInNewColony(int newlandid, ItemPrice resources)
        {
            if (!NewLands.ContainsKey(newlandid)) return new GSError(1, 1);
            ServerAvanpost newland = NewLands[newlandid];
            ServerAvanpost.BuildResult result = newland.Build(resources);
            if (result == ServerAvanpost.BuildResult.ErrorPrice) return new GSError(1, 5);
            if (result == ServerAvanpost.BuildResult.NoMoney) return new GSError(1, 4);
            if (result == ServerAvanpost.BuildResult.Finished)
            {
                NewLands.Remove(newland.ID);
                newland.Planet.NewLands.Remove(newland.ID);
                ServerLand.FinishAvanpost(newland);
                CreateCryptLand();
            }
            CreateNewLandsArray();
            CreateArrayForPlayer();
            return new GSError(0, 0);
        }
        public GSError ChangeLandName(ServerLand land, GSString name)
        {
            if (!Lands.ContainsKey(land.ID)) return new GSError(1, 1);
            if (land.Name == name) return new GSError(1, 10);
            if (!ServerLand.CheckLandName(name)) return new GSError(1, 11);
            int Price = (int)((Math.Round(land.Peoples, 0) * 1000));
            if (!Resources.RemovePrice(new ItemPrice(Price, 0, 0, 0))) return new GSError(1, 4);
            land.Name = name;
            CreateCryptLand();
            CreateArrayForPlayer();
            return new GSError(1, 12);
        }
        /*
        public GSError ReplaceSector(ServerLand land, byte donor, byte target)
        {
            //1 чужая земля
            if (!Lands.ContainsKey(land.ID)) return new GSError(1, 1);
            land.ReplaceSectors(donor, target);
            CreateCryptLand();
            CreateArrayForPlayer();
            return new GSError(1, 8);
        }
        */
        /*
        public GSError UpgradeBuilding(ServerLand land, byte sectorid, GSBuilding building, GSBuilding version, byte count)
        {
            
            //1 чужая земля
            //2 постройка неисследована
            //4 нет денег
            //5 ошибка позиционирования
            //7 постройка улучшена
            if (!Lands.ContainsKey(land.ID)) return new GSError(1, 1);
            if (sectorid >= land.Planet.Locations) return new GSError(1, 15);
            ServerLandSector sector = land.Locations[sectorid];
            if (version.Type != building.Type) return new GSError(1, 16);

            if (version.Type == BuildingType.Portal)
            {
                ServerFleetBase fleetbase = (ServerFleetBase)sector;
                if (building.Level / 10 != fleetbase.PortalLevel) return new GSError(1, 17);
                if (version.Level / 10 - 1 != fleetbase.PortalLevel) return new GSError(1, 17);
            }
            else
            {
                if (!Sciences.SciencesArray.Contains(version.ID)) return new GSError(1, 2);
                if (version.Level < building.Level) return new GSError(1, 5);
                ServerPeaceSector peace = (ServerPeaceSector)sector;
                if (!peace.Buildings.ContainsKey(building.ID)) return new GSError(1, 5);
                if (peace.Buildings[building.ID] < count) return new GSError(1, 5);
            }
            ItemPrice upgradePrice = ItemPrice.GetUpgradePrice(building.Price, version.Price, count);
            if (!Resources.RemovePrice(upgradePrice)) return new GSError(1, 4);
            if (building.Type != BuildingType.Portal)
                building = building;
            land.UpgradeBuilding(sector, building, version, count);
            CalculateResCap();
            CreateCryptLand();
            CreateArrayForPlayer();
            return new GSError(1, 7);
            
        }*/
        /*
        public GSError DestroyBuilding(ServerLand land, byte sectorid, GSBuilding building, byte count)
        {
            //1 чужая земля
            //5 ошибка позиционирования
            //6 постройка снесена
            if (!Lands.ContainsKey(land.ID)) return new GSError(1, 1);
            if (sectorid >= land.Planet.Locations) return new GSError(1, 15);
            ServerLandSector sector = land.Locations[sectorid];
            if (building.Sector!=sector.Type) return new GSError(1, 15);
            ServerPeaceSector peace = (ServerPeaceSector)sector;
            if (!peace.Buildings.ContainsKey(building.ID)) return new GSError(1, 5);
            if (peace.Buildings[building.ID] < count) return new GSError(1, 5);
            land.RemoveBuilding(sector, building, true, count);
            CalculateResCap();
            Resources.AddHalfPrice(building.Price.Mult(count));
            CreateCryptLand();
            CreateArrayForPlayer();
            return new GSError(1, 6);
        }
        */
        static int c = 0;
        public GSError BuildBuilding(ServerLand land,byte sectorid, byte building)
        {
            //ответы E1 = 1
            //1 чужая земля
            //2 недоступно исследование
            //3 нету места
            //15 - неверный сектор
            //16 - неподходящий сектор для зданий
            //4 нет денег
            if (!Lands.ContainsKey(land.ID)) return new GSError(1, 1);
            //if (!Sciences.SciencesArray.Contains(building.ID)) return new GSError(1, 2);
            
            if (sectorid >= land.Planet.Locations) return new GSError(1, 15);
            if (land.Locations[sectorid].GetSectorType() == LandSectorType.Clear) return new GSError(1, 15);
            ServerPeaceSector sector = (ServerPeaceSector)land.Locations[sectorid];
            if (((int)land.Peoples - land.BuildingsCount) < sector.Buildings[building].GetNextSize()) return new GSError(1, 3);
            if (!Resources.RemovePrice(sector.Buildings[building].GetNextPrice())) return new GSError(1, 4);
            land.BuildBuilding(sectorid, building);
            //if (ServerLinks.TechnoTeam == this) { c++; Links.Controller.mainWindow.Title = c.ToString(); }
            CalculateResCap();
            CreateCryptLand();
            CreateArrayForPlayer();
            return new GSError(1, 0);
        }
        public GSError BuildSector(ServerLand land, SectorTypes type, int pos)
        {
            //1.1 - чужая земля
            //1.13 - сектор занят
            //1.14 - такой сектор уже есть
            //1.15 - текущая позиция сектора недоступна
            if (!Lands.ContainsKey(land.ID)) return new GSError(1, 1);
            if (land.Planet.Locations <= pos) return new GSError(1, 15);
            if (land.Locations[pos].Type != SectorTypes.Clear) return new GSError(1, 13);
            if (type!=SectorTypes.War)
            foreach (ServerLandSector s in land.Locations)
                    if (type == s.Type)
                        return new GSError(1, 14);
            land.BuildSector(type, pos);
            CalculateResCap();
            CreateCryptLand();
            CreateArrayForPlayer();
            return new GSError(1, 0);
        }
        public GSError DestroyShip(long shipid)
        {
            if (!Ships.ContainsKey(shipid)) return new GSError(4, 4);
            ServerShip ship = Ships[shipid];
            if (ship.Fleet != null) return new GSError(4, 6);
            Ships.Remove(shipid);
            CalculateShipArray();
            CreateArrayForPlayer();
            return new GSError(4, 0);
        }
        public GSError RepairShip(long shipid)
        {
            if (!Ships.ContainsKey(shipid)) return new GSError(4, 4);
            ServerShip ship = Ships[shipid];
            if (ship.Health == 100) return new GSError(4, 5);
            if (!TryRepairShip(ship, true)) return new GSError(4, 2);
            else return new GSError(4, 0);
        }
        public GSError BuildShip(Schema schema, byte[] model, out long shipid)
        {
            shipid = -1;
            //проверить соответствие схемы
            //построить корабль если денег достаточно
            if (!Sciences.CheckShipSchema(schema)) return new GSError(4, 1);
            if (Resources.RemovePrice(schema.Price) == true)
            {
                ServerShip ship = ServerShip.GetNewShip(schema, model);
                AddShip(ship);
                shipid = ship.ID;
                return new GSError(4, 0);
            }
            else
                return new GSError(4, 2);
        }
        public GSError LearnNewScience(byte[] information)
        {
            ScienceLearn level;
            if (information.Length != 4) return new GSError(2, 1); //ошибка в распозновании
            int intfield = BitConverter.ToInt32(information, 0);
            if (intfield < 0 || intfield > 2) return new GSError(2, 1);
            level = (ScienceLearn)intfield;
            int LearnResult = Sciences.SelectNewScience(level);
            ItemPrice SPrice = SciencePrice.GetSciencePrice(this, level);
            if (!Resources.RemovePrice(SPrice)) //если денег достаточно
                return new GSError(2, 2); //денег нелостаточно
            if (LearnResult >= 0)
            {
                Sciences.PutScienceToArray((ushort)LearnResult); //сохранить исследование
                Notice.GetLearnNewScience((ushort)LearnResult);
                CreateArrayForPlayer();
                return new GSError(3, (ushort)LearnResult); //результат исследования
            }
            else
                return new GSError(2, 3); //неудалось выполнить исследование*/
        }
    }
    public class GSError
    {
        public ushort E1;
        public ushort E2;
        public GSError(ushort e1, ushort e2)
        {
            E1 = e1;
            E2 = e2;
        }
        public string Text
        {
            get
            {
                switch (E1)
                {
                    case 0:
                        switch (E2)
                        {
                            case 0: return "Всё нормально";
                        }
                        break;
                    case 1: //постройки 
                        switch (E2)
                        {
                            case 0: return "Постройка построена";
                            case 1: return "Территория не принадлежит игроку";
                            case 2: return "Постройка неисследована";
                            case 3: return "Недостаточно места на территории";
                            case 4: return "Недостаточно денег";
                            case 5: return "Ошибка при позиционировании";
                            case 6: return "Постройка снесена";
                            case 7: return "Постройка улучшена";
                            case 8: return "Сектор перемещён";
                            case 9: return "Неверное число построек";
                            case 10: return "То же самое имя";
                            case 11: return "Неверное имя";
                            case 12: return "Имя изменено";
                            case 13: return "Сектор занят";
                            case 14: return "Такой сектор уже есть";
                            case 15: return "Текущая позиция сектора недоступна";
                            case 16: return "В этом секторе нельзя строить этот тип зданий";
                            case 17: return "Ошибка при улучшении портала";
                        }
                        break;
                    case 2: //исследования
                        switch (E2)
                        {
                            case 0: return "Исследование изучено";
                            case 1: return "Ошибка распознования";
                            case 2: return "Недостаточно денег";
                            case 3: return "Неудалось выполнить изучение";
                        }
                        break;
                    case 3: return E2.ToString();
                    case 4: //Корабли
                        switch (E2)
                        {
                            case 0: return "Корабль построен";
                            case 1: return "Исследования недоступны";
                            case 2: return "Недостаточно денег";
                            case 3: return "Недостаточно слотов под корабли";
                            case 4: return "Ошибка выбора корабля";
                            case 5: return "Корабль не повреждён";
                            case 6: return "Корабль занят";
                        }
                        break;

                    case 5: //Обмен
                        switch (E2)
                        {
                            case 0: return "Успешно";
                            case 1: return "Недостаточно металла";
                            case 2: return "Недостаточно микросхем";
                            case 3: return "Недостаточно антиматерии";
                            case 4: return "Недостаточно зипа";
                            case 5: return "Недостаточно денег";
                            case 6: return "Непредвиденная ошибка при обмене";
                        }
                        break;
                    case 6: //Пилоты
                        switch (E2)
                        {
                            case 0: return "Успешно";
                            case 1: return "Недостаточно денег";
                            case 2: return "Пилот нанят";
                            case 3: return "Ошибка позиции";
                            case 4: return "Пилот уволен";
                            case 5: return "Ошибка передачи данных";
                            case 6: return "Ошибка выбора пилота";
                            case 7: return "Корабль уничтожен";
                            case 8: return "Корабль во флоте";

                        }
                        break;
                    case 7: //Флоты
                        switch (E2)
                        {
                            case 0: return "Успешно";
                            case 1: return "Территория не принадлежит";
                            case 2: return "Слишком много флотов";
                            case 3: return "Ошибка выбора флота";
                            case 4: return "Ошибка выбора корабля";
                            case 5: return "Флот на задании";
                            case 6: return "Корабль на задании";
                            case 7: return "Нет пилота";
                            case 8: return "Ошибка при выборе планеты";
                            case 9: return "Флот не может выполнять колонизацию";
                            case 10: return "Невозможно создать колонии на этой планете";
                            case 11: return "Невозможно колонизировать новые колонии";
                            case 12: return "На планете избыток кораблей";
                            case 13: return "Избыток кораблей во флоте";
                            case 14: return "Неверно выбранная колония";
                            case 15: return "Невозможно взять под защиту эту колонию";
                            case 16: return "Недостаточно кораблей во флоте";
                            case 17: return "Ошибка выбора территории";
                            case 18: return "Невозможно атаковать эту территорию";
                            case 19: return "Текущий флот-защитник в бою";
                            case 20: return "Ошибка при выборе миссии";
                            case 21: return "Вы уже послали другой флот на эту миссию";
                            case 22: return "Недостаточно средств на создание базы флота";
                            case 23: return "Расстояние до цели слишком далеко";
                            case 24: return "Невозможно добавить артефакт флоту";
                            case 25: return "Ошибка при выборе звезды";
                            case 26: return "Флот не на патрулировании";
                            case 27: return "Невозможно вернуть флот";
                            case 28: return "Невозможно грабить эту планету";
                        }
                        break;
                    case 8: //Битвы
                        switch (E2)
                        {
                            case 0: return "Успешно";
                        }
                        break;
                    case 9: //Премиум
                        {
                            switch (E2)
                            {
                                case 0: return "Успешно";
                                case 1: return "Недостаточно премиум валюты";
                                case 2: return "Ошибка данных";
                            }
                        }
                        break;
                    case 10: //Артефакты
                        {
                            switch (E2)
                            {
                                case 0: return "10.0 Успешно";
                                case 1: return "Ошибка 10.1: Неверный ID артефакта";
                                case 2: return "Ошибка 10.2: У вас нет такого артефакта";
                                case 3: return "Ошибка 10.3: Артефакт не мирный";
                                case 4: return "Ошибка 10.4: Артефакт не военный";
                                case 5: return "Ошибка 10.5: Дополнительные параметры не верны";
                                case 6: return "Ошибка 10.6: Колония не принадлежит игроку";
                                case 7: return "Ошибка 10.7: Неверный ID сектора";
                                case 8: return "Ошибка 10.8: Артефакт нельзя применить к этому сектору";
                                case 9: return "Ошибка 10.9: Артефакт нельзя применить к этой колонии";
                            }
                        } break;
                }
                return "";
            }
        }
    }
}
