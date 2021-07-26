using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class GSGameEvent
    {
        static ByteMessage Cheat(GSPlayer player, byte[] information)
        {
            player.Resources.AddResources(new BuildingValues(100000, 10000, 10000, 10000));
            return new ByteMessage(MessageResult.Yes);
        }
        static ByteMessage RemoveWarArtefactInFleet(GSPlayer player, byte[] information)
        {
            if (information.Length != 14) throw new Exception();
            int landid = BitConverter.ToInt32(information, 0);
            long fleetid = BitConverter.ToInt64(information, 4);
            ushort artefactid = BitConverter.ToUInt16(information, 12);
            GSError error = player.RemoveArtefactFromFleet(landid, fleetid, artefactid);
            if (error.E2 == 0) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        static ByteMessage PutWarArtefactInFleet(GSPlayer player, byte[] information)
        {
            if (information.Length != 14) throw new Exception();
            int landid = BitConverter.ToInt32(information, 0);
            long fleetid = BitConverter.ToInt64(information, 4);
            ushort artefactid = BitConverter.ToUInt16(information, 12);
            GSError error = player.PlaceArtefactInFleet(landid, fleetid, artefactid);
            if (error.E2 == 0) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
       static ByteMessage UsePeaceArtefact(GSPlayer player, byte[] information)
        {
            if (information.Length < 2) throw new Exception();
            ushort artefactID = BitConverter.ToUInt16(information, 0);
            ArtefactUseResult result = player.UsePeaceArtefact(artefactID, information);
            return new ByteMessage(result.Result, result.Array);
        }
        static ByteMessage GetPlanetLands(GSPlayer player, byte[] information)
        {
            if (information.Length != 4) throw new Exception();
            int planetid = BitConverter.ToInt32(information, 0);
            if (!ServerLinks.GSPlanets.ContainsKey(planetid))
                throw new Exception();
            ServerPlanet planet = ServerLinks.GSPlanets[planetid];
            if (planet.LandsCount <= 0)
                throw new Exception();
            return new ByteMessage(true, planet.GetLandsArray());
        }
        /// <summary> выполняет миссию. Если миссия с боем, то возвращает ID боя, если текстовая - то ничего не возвращает </summary>
        static ByteMessage StartStoryLineMission(GSPlayer player, byte[] information)
        {
            if (information.Length != 12 && information.Length!=0) throw new Exception();
            if (information.Length == 12)
            {
                int landid = BitConverter.ToInt32(information, 0);
                long fleetid = BitConverter.ToInt64(information, 4);
                long battleid;
                GSError error = player.StartStoryLineMissionBattle(landid, fleetid, out battleid);
                if (error.E2 == 0) return new ByteMessage(true, BitConverter.GetBytes(battleid));
                else return new ByteMessage(error.Text);
            }
            else
            {
                GSError error = player.FinishStoryLineMissionText();
                if (error.E2 == 0) return new ByteMessage(MessageResult.Yes);
                else return new ByteMessage(error.Text);
            }
        }
        static ByteMessage StartMission2Battle(GSPlayer player, byte[] information)
        {
            if (information.Length != 17) throw new Exception();
            int landid = BitConverter.ToInt32(information, 0);
            long fleetid = BitConverter.ToInt64(information, 4);
            int starid = BitConverter.ToInt32(information, 12);
            byte orbit = information[16];
            long battleid;
            //флот отправляется на задание. Результат отображается в battleid. >=0 - бой, -1 - награды нет, -2 - награда малая, -3 - награда средняя, -4 - чёрный рынок
            //значение Е2 в error - отличное от нуля - ошибка
            //сама награда отображается в Notice.
            GSError error = player.StartMission2Battle(landid, fleetid, starid, orbit, out battleid);
            if (error.E2 == 0) return new ByteMessage(true, BitConverter.GetBytes(battleid));
            else return new ByteMessage(error.Text);
        }
        /*static ByteMessage StartResourceMissionBattle(GSPlayer player, byte[] information)
        {
            if (information.Length != 18) throw new Exception();
            int landid = BitConverter.ToInt32(information, 0);
            long fleetid = BitConverter.ToInt64(information, 4);
            byte missiontype = information[12];
            if (missiontype >= MissionInspector.missionscount) throw new Exception();
            ushort missionid = BitConverter.ToUInt16(information, 13);
            bool repair = BitConverter.ToBoolean(information, 17);
            long battleid;
            GSError error = player.StartResourceMissionBattle(landid, fleetid, missiontype, missionid, out battleid);
            if (error.E2 == 0) return new ByteMessage(true, BitConverter.GetBytes(battleid));
            else return new ByteMessage(error.Text);
        }*/
        static ByteMessage SendFleetToConquer(GSPlayer player, byte[] information)
        {
            if (information.Length != 16) throw new Exception();
            int landid = BitConverter.ToInt32(information, 0);
            long fleetid = BitConverter.ToInt64(information, 4);
            int planetid = BitConverter.ToInt32(information, 12);
            long battleid;
            GSError error = player.StartConquerEvent(landid, fleetid, planetid, out battleid);
            if (error.E2 == 0) return new ByteMessage(true, BitConverter.GetBytes(battleid));
            else return new ByteMessage(error.Text);
        }
        static ByteMessage RemoveFleetAsLandDefender(GSPlayer player, byte[] information)
        {
            if (information.Length != 12) throw new Exception();
            int landid = BitConverter.ToInt32(information, 0);
            long fleetid = BitConverter.ToInt64(information, 4);
            GSError error = player.RemoveFleetFromLandDefender(landid, fleetid);
            if (error.E2 == 0) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        static ByteMessage SetFleetAsLandDefender(GSPlayer player, byte[] information)
        {
            if (information.Length != 15) throw new Exception();
            int landid = BitConverter.ToInt32(information, 0);
            long fleetid = BitConverter.ToInt64(information, 4);
            GSError error = player.SetFleetAsLastDefenderPlayer(landid, fleetid);
            if (error.E2 == 0) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        static ByteMessage SendFleetToPillage(GSPlayer player, byte[] information)
        {
            if (information.Length != 16) throw new Exception();
            int landid = BitConverter.ToInt32(information, 0);
            long fleetid = BitConverter.ToInt64(information, 4);
            int planetid = BitConverter.ToInt32(information, 12);
            long battleid;
            GSError error = player.StartPillageEvent(landid, fleetid, planetid, out battleid);
            if (error.E2 == 0) return new ByteMessage(true, BitConverter.GetBytes(battleid));
            else return new ByteMessage(error.Text);
        }
        static ByteMessage RemoveFleet(GSPlayer player, byte[] information)
        {
            if (information.Length != 12) throw new Exception();
            int landid = BitConverter.ToInt32(information, 0);
            long fleetid = BitConverter.ToInt64(information, 4);
            GSError error = player.RemoveFleet(landid, fleetid);
            if (error.E2 == 0) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        static ByteMessage SendFleetToDefensePlayer(GSPlayer player, byte[] information)
        {
            if (information.Length != 15) throw new Exception();
            int landid = BitConverter.ToInt32(information, 0);
            long fleetid = BitConverter.ToInt64(information, 4);
            GSError error = player.SendFleetToDefensePlayer(landid, fleetid);
            if (error.E2 == 0) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        static ByteMessage StartColonizeLand(GSPlayer player, byte[] information)
        {
            if (information.Length != 4) throw new Exception();
            int planetid = BitConverter.ToInt32(information, 0);
            int avanpostid;
            GSError error = player.StartColonizeLand(planetid, out avanpostid);
            if (error.E2 == 0)
                return new ByteMessage(true, BitConverter.GetBytes(avanpostid));
            else
                return new ByteMessage(error.Text);
        }
        static ByteMessage StartScout(GSPlayer player, byte[] information)
        {
            if (information.Length != 16)
                throw new Exception();
            int landid = BitConverter.ToInt32(information, 0);
            long fleetid = BitConverter.ToInt64(information, 4);
            int StarID = BitConverter.ToInt32(information, 12);
            long battleid;
            GSError error = player.SendFleetToScout(landid, fleetid, StarID, out battleid);
            if (error.E2 == 0)
                return new ByteMessage(true, BitConverter.GetBytes(battleid));
            else
                return new ByteMessage(error.Text);
        }
        static ByteMessage RemoveShipFromFleet(GSPlayer player, byte[] information)
        {
            if (information.Length != 20)
                throw new Exception();
            int landid = BitConverter.ToInt32(information, 0);
            long fleetid = BitConverter.ToInt64(information, 4);
            long ShipID = BitConverter.ToInt64(information, 12);
            GSError error = player.RemoveShipFromFleet(landid, fleetid, ShipID);
            if (error.E2 == 0) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        static ByteMessage ChangeFleetParams(GSPlayer player, byte[] information)
        {
            if (information.Length != 15) throw new Exception();
            int landid = BitConverter.ToInt32(information, 0);
            long fleetid = BitConverter.ToInt64(information, 4);
            bool repair = BitConverter.ToBoolean(information, 13);
            GSError error = player.ChangeFleetParams(landid, fleetid, repair);
            if (error.E2 == 0) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        static ByteMessage MoveShipToFleet(GSPlayer player, byte[] information)
        {
            if (information.Length != 20)
                throw new Exception();
            int landid = BitConverter.ToInt32(information, 0);
            long fleetid = BitConverter.ToInt64(information, 4);
            long ShipID = BitConverter.ToInt64(information, 12);
            GSError error = player.MoveShipToFleet(landid, fleetid, ShipID);
            if (error.E2 == 0) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        static ByteMessage CreateFleet(GSPlayer player, byte[] information)
        {
            //if (player.Premium.IsPremium && player.GetFleetsCount() >= ServerLinks.Parameters.PremiumMaxFleets)
            //    throw new Exception();
            if (information.Length != 9) throw new Exception();
            int landid = BitConverter.ToInt32(information, 0);
            byte sectorpos = information[4];
            byte[] image = new byte[4];
            image[0] = information[5]; image[1] = information[6];
            image[2] = information[7]; image[3] = information[8];
            ServerFleet fleet;
            GSError error = player.CreateFleet(landid, sectorpos, image, out fleet);
            if (error.E2 == 0) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        static ByteMessage ShowMarkets(GSPlayer player, byte[] information)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(ServerMarket.GetGlobalChangePrice(player)));
            list.AddRange(player.LocalMarket.MarketArray);
            if (player.GlobalMarket!= null)
                 list.AddRange(player.GlobalMarket.MarketArray);            
            return new ByteMessage(true, list.ToArray());
        }
        static ByteMessage ShowBlackMarket(GSPlayer player, byte[] information)
        {
            if (player.BlackMarket != null)
                return new ByteMessage(true, player.BlackMarket.MarketArray);
            else
                return new ByteMessage(false, new byte[0]);

        }
        static ByteMessage TryChangeNew(GSPlayer player, byte[] information)
        {
            if (information.Length != 6) throw new Exception();
            if (information[0] > 2) throw new Exception();
            MarketType type = (MarketType)information[0];
            ServerMarket market = null;
            switch (type)
            {
                case MarketType.Local: if (player.LocalMarket == null) throw new Exception(); else market = player.LocalMarket; break;
                case MarketType.Galaxy: if (player.GlobalMarket == null) throw new Exception(); else market = player.GlobalMarket; break;
                case MarketType.Black: if (player.BlackMarket == null) throw new Exception(); else market = player.BlackMarket; break;
            }
            byte element = information[1];
            int quantity = BitConverter.ToInt32(information, 2);
            bool changeresult = market.ChangeResources(element, quantity);
            if (changeresult == false) throw new Exception();
            return new ByteMessage(MessageResult.Yes);
        }
        static ByteMessage LeaveMarket(GSPlayer player, byte[] information)
        {
            if (information.Length != 1) throw new Exception();
            if (information[0] > 2) throw new Exception();
            MarketType type = (MarketType)information[0];
            ServerMarket market = null;
            switch (type)
            {
                case MarketType.Local: if (player.LocalMarket == null) throw new Exception(); else market = player.LocalMarket; break;
                case MarketType.Galaxy: if (player.GlobalMarket == null) throw new Exception(); else market = player.GlobalMarket; break;
                case MarketType.Black: if (player.BlackMarket == null) throw new Exception(); else market = player.BlackMarket; break;
            }
            long leaveresult = market.LeaveMarket(false);
            return new ByteMessage(true, BitConverter.GetBytes(leaveresult));
        }
        static ByteMessage CreateGalaxyMarket(GSPlayer player, byte[] information)
        {
            if (player.Resources._Money < ServerMarket.GetGlobalChangePrice(player)) throw new Exception();
            player.Resources.RemovePrice(ServerMarket.GetGlobalChangePrice(player));
            player.GlobalMarket = new ServerMarket(player, MarketType.Galaxy);
            return new ByteMessage(MessageResult.Yes);
        }
        static ByteMessage MovePilotFromShip(GSPlayer player, byte[] information)
        {
            if (information.Length != 8) throw new Exception();
            long shipID = BitConverter.ToInt64(information, 0);
            GSError error = player.MovePilotFromShip(shipID);
            if (error.E2 == 0) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        static ByteMessage PutPilotToShip(GSPlayer player, byte[] information)
        {
            if (information.Length != 18) throw new Exception();
            long shipID = BitConverter.ToInt64(information, 0);
            GSPilot pilot = GSPilot.GetPilot(information, 8);
            GSError error = player.PutPilotToShip(shipID, pilot);
            if (error.E2 == 0) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        static ByteMessage DismissPilot(GSPlayer player, byte[] information)
        {
            GSError error = player.DismissPilot(information);
            if (error.E2 == 4) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        static ByteMessage HirePilot(GSPlayer player, byte[] information)
        {
            if (information.Length != 1) throw new Exception();
            GSError error = player.HirePilot(information[0]);
            if (error.E2 == 2) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        static ByteMessage ChangePilotsRelease(GSPlayer player, byte[] information)
        {
            GSError error = player.GenerateNewPilotGroup();
            if (error.E2 == 0) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        static ByteMessage ResourceExchange(GSPlayer player, byte[] information)
        {
            if (information.Length != 5)
                throw new Exception();
            if (information[0] > 7)
                throw new Exception();
            int exchangevalue = BitConverter.ToInt32(information, 1);
            if (exchangevalue < 0 || exchangevalue > 1000000)
                throw new Exception();

            GSError error = player.ResourceExchange(information[0], exchangevalue);
            if (error.E2 == 0)
            {
                //if (!player.Quest.AllDone) player.Quest.ReasonFinish(Reason_Finish.Trade_Maked);
                return new ByteMessage(MessageResult.Yes);
            }
            else return new ByteMessage(error.Text);


        }
        static ByteMessage BuildInNewLand(GSPlayer player, byte[] information)
        {
            if (information.Length != 20) throw new Exception();
            int landid = BitConverter.ToInt32(information, 0);
            int money = BitConverter.ToInt32(information, 4);
            int metall = BitConverter.ToInt32(information, 8);
            int chips = BitConverter.ToInt32(information, 12);
            int anti = BitConverter.ToInt32(information, 16);
            if (money < 0 || metall < 0 || chips < 0 || anti < 0) throw new Exception();
            ItemPrice price = new ItemPrice(money, metall, chips, anti);
            GSError error = player.BuildInNewColony(landid, price);
            if (error.E2 == 0) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        static ByteMessage ChangeLandName(GSPlayer player, byte[] information)
        {
            if (information.Length < 9)
                throw new Exception();
            int landid = BitConverter.ToInt32(information, 0);
            if (!ServerLinks.GSLands.ContainsKey(landid))
                return new ByteMessage("Wrong Land ID");
            if (!GSString.CheckArray(information, 4)) throw new Exception();
            GSString landname = new GSString(information, 4);
            ServerLand land = ServerLinks.GSLands[landid];
            GSError error = player.ChangeLandName(land, landname);
            if (error.E2 == 12) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        /*
        static ByteMessage ReplaceSectors(GSPlayer player, byte[] information)
        {
            if (information.Length != 6)
                throw new Exception();
            int landid = BitConverter.ToInt32(information, 0);
            if (!ServerLinks.GSLands.ContainsKey(landid))
                return new ByteMessage("Wrong Land ID");
            byte donor = information[4];
            byte target = information[5];
            if (donor == 0 || donor > 11)
                return new ByteMessage("Wrong donor position");
            if (target == 0 || target > 11)
                return new ByteMessage("Wrong target position");
            ServerLand land = ServerLinks.GSLands[landid];
            GSError error = player.ReplaceSector(land, donor, target);
            if (error.E2 == 8) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        */
        /*
        static ByteMessage UpgradeBuilding(GSPlayer player, byte[] information)
        {
            if (information.Length != 10)
                throw new Exception();
            int landid = BitConverter.ToInt32(information, 0);
            byte sectorid = information[4];
            ushort buildingid = BitConverter.ToUInt16(information, 5);
            ushort versionid = BitConverter.ToUInt16(information, 7);
            byte buildingscount = information[9];
            if (buildingscount > 5 || buildingscount == 0)
                throw new Exception();
            if (!ServerLinks.GSLands.ContainsKey(landid))
                return new ByteMessage("Wrong Land ID");
            if (!ServerLinks.GSBuildings.ContainsKey(buildingid))
                return new ByteMessage("Wrong building ID");
            if (!ServerLinks.GSBuildings.ContainsKey(versionid))
                return new ByteMessage("Wrong building ID");
            ServerLand land = ServerLinks.GSLands[landid];
            GSError error = player.UpgradeBuilding(land, sectorid, ServerLinks.GSBuildings[buildingid], ServerLinks.GSBuildings[versionid], buildingscount);
            if (error.E2 == 0) return new ByteMessage(MessageResult.Yes);
            if (error.E2 == 7)
                return new ByteMessage(MessageResult.Yes);
            else
                return new ByteMessage(error.Text);
        }
        */
        /*
        static ByteMessage RemoveBuilding(GSPlayer player, byte[] information)
        {
            if (information.Length != 8)
                throw new Exception();
            int landid = BitConverter.ToInt32(information, 0);
            byte sectorid = information[4];
            ushort buildingid = BitConverter.ToUInt16(information, 5);
            byte buildingscount = information[7];
            if (buildingscount > 5 || buildingscount == 0)
                throw new Exception();
            if (!ServerLinks.GSLands.ContainsKey(landid))
                return new ByteMessage("Wrong Land ID");
            if (!ServerLinks.GSBuildings.ContainsKey(buildingid))
                return new ByteMessage("Wrong building ID");
            ServerLand land = ServerLinks.GSLands[landid];
            //if (buildingtag < 0)
            //    return new ByteMessage("Wrong tag");
            GSError error = player.DestroyBuilding(land, sectorid, ServerLinks.GSBuildings[buildingid], buildingscount);
            if (error.E2 == 6) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        */
        static ByteMessage BuildNewBuilding(GSPlayer player, byte[] information)
        {
            if (information.Length != 6)
                throw new Exception();
            int landid = BitConverter.ToInt32(information, 0);
            byte sectorid = information[4];
            byte building = information[5];
            if (building > 2)
                throw new Exception();
            if (!ServerLinks.GSLands.ContainsKey(landid))
                return new ByteMessage("Wrong Land ID");
            ServerLand land = ServerLinks.GSLands[landid];
            GSError error = player.BuildBuilding(land,sectorid,  building);
            if (error.E2 == 0) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        static ByteMessage CreateSector(GSPlayer player, byte[] information)
        {
            int landid = BitConverter.ToInt32(information, 0);
            byte sectortype = information[4];
            if (sectortype > 18)
                throw new Exception();
            SectorTypes type = (SectorTypes)sectortype;
            byte pos = information[5];
            if (!ServerLinks.GSLands.ContainsKey(landid))
                return new ByteMessage("Wrong Land ID");
            ServerLand land = ServerLinks.GSLands[landid];
            GSError error = player.BuildSector(land, type, pos);
            if (error.E2 == 0) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        static ByteMessage DestroyShip(GSPlayer player, byte[] information)
        {
            if (information.Length != 8)
                throw new Exception();
            long shipid = BitConverter.ToInt64(information, 0);
            GSError error = player.DestroyShip(shipid);
            if (error.E2 == 0) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        static ByteMessage RepairShip(GSPlayer player, byte[] information)
        {
            if (information.Length != 8)
              throw new Exception();
            long shipid = BitConverter.ToInt64(information, 0);
            GSError error = player.RepairShip(shipid);
            if (error.E2 == 0) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        static ByteMessage BuildShip(GSPlayer player, byte[] information)
        {
            if (information.Length != 33)
                throw new Exception();
            int i = 0;
            Schema schema = Schema.GetSchema(information, ref i);
            if (schema == null || !schema.Check()) return new ByteMessage("Wrong schema");
            byte[] model = new byte[4];
            model[0] = information[29]; model[1] = information[30];
            model[2] = information[31]; model[3] = information[32];
            long shipid;
            GSError error = player.BuildShip(schema, model, out shipid);
            if (error.E2 == 0) return new ByteMessage(MessageResult.Yes);
            else return new ByteMessage(error.Text);
        }
        static ByteMessage AppendShipSchemas(GSPlayer player, byte[] information)
        {
            if (information.Length % 29 != 0)
                return new ByteMessage("Ship Schemas Error");
            player.AppendShipSchemas(information);
            return new ByteMessage(MessageResult.Yes);
        }
        static ByteMessage LearnNewScience(GSPlayer player, byte[] information)
        {
            GSError LearnResult = player.LearnNewScience(information);
            if (LearnResult.E1 == 2)
            {
                ByteMessage result = new ByteMessage(LearnResult.Text);
                return result;
            }
            else return new ByteMessage(true, BitConverter.GetBytes(LearnResult.E2));
        }
        public static ByteMessage MakeEvent(GSPlayer player, ByteMessage message)
        {
            switch (message.Field1)
            {
                case 0:
                    break;
                case 1:
                    switch (message.Field2)
                    {
                        case 1:
                            return LearnNewScience(player, message.Message);
                        case 10:
                            return Cheat(player, message.Message);      
                    }
                    break;
                case 3:
                    switch (message.Field2)
                    {
                        case 1:
                            return AppendShipSchemas(player, message.Message);
                        case 3:
                            return BuildShip(player, message.Message);
                        case 5:
                            return RepairShip(player, message.Message);
                        case 6:
                            return DestroyShip(player, message.Message);
                    }
                    break;
                case 4:
                    switch (message.Field2)
                    {
                        case 2:
                            return BuildNewBuilding(player, message.Message);
                       // case 3:
                       //     return RemoveBuilding(player, message.Message);
                       // case 4:
                       //     return UpgradeBuilding(player, message.Message);
                        //case 5:
                            //return ReplaceSectors(player, message.Message);
                        case 6:
                            return ChangeLandName(player, message.Message);
                        case 8:
                            return BuildInNewLand(player, message.Message);
                        case 9:
                            return CreateSector(player, message.Message);
                    }
                    break;
                case 5:
                    switch (message.Field2)
                    {
                        case 2:
                            return ResourceExchange(player, message.Message);
                            /* СОБЫТИЯ КЛАНА НЕ НУЖНЫ В ОДИНОЧНОЙ ВЕРСИИ
                        case 3:
                            GSGameEvent CreateNewClanEvent = new GSGameEvent(account, EEventType.CreateClan, message.Message);
                            ServerLinks.Events.CurrentQueue.Enqueue(CreateNewClanEvent);
                            return CreateNewClanEvent.ID;
                        case 4:
                            GSGameEvent ClansListClanEvent = new GSGameEvent(account, EEventType.GetClansList, message.Message);
                            ServerLinks.Events.CurrentQueue.Enqueue(ClansListClanEvent);
                            return ClansListClanEvent.ID;
                        case 5:
                            GSGameEvent SelfClanInfoEvent = new GSGameEvent(account, EEventType.GetSelfClanInfo, message.Message);
                            ServerLinks.Events.CurrentQueue.Enqueue(SelfClanInfoEvent);
                            return SelfClanInfoEvent.ID;
                        case 6:
                            GSGameEvent LeaveClanEvent = new GSGameEvent(account, EEventType.LeaveClan, message.Message);
                            ServerLinks.Events.CurrentQueue.Enqueue(LeaveClanEvent);
                            return LeaveClanEvent.ID;
                        case 7:
                            GSGameEvent SendInviteRequestEvent = new GSGameEvent(account, EEventType.SendInviteRequest, message.Message);
                            ServerLinks.Events.CurrentQueue.Enqueue(SendInviteRequestEvent);
                            return SendInviteRequestEvent.ID;
                        case 8:
                            GSGameEvent RemoveInviteRequestEvent = new GSGameEvent(account, EEventType.RemoveInviteRequest, message.Message);
                            ServerLinks.Events.CurrentQueue.Enqueue(RemoveInviteRequestEvent);
                            return RemoveInviteRequestEvent.ID;
                        case 9:
                            GSGameEvent GetInviteRequestListEvent = new GSGameEvent(account, EEventType.GetInviteRequestList, message.Message);
                            ServerLinks.Events.CurrentQueue.Enqueue(GetInviteRequestListEvent);
                            return GetInviteRequestListEvent.ID;
                        case 10:
                            GSGameEvent InvitePlayerEvent = new GSGameEvent(account, EEventType.InvitePlayer, message.Message);
                            ServerLinks.Events.CurrentQueue.Enqueue(InvitePlayerEvent);
                            return InvitePlayerEvent.ID;
                        case 11:
                            GSGameEvent DeclinePlayerEvent = new GSGameEvent(account, EEventType.DeclinePlayer, message.Message);
                            ServerLinks.Events.CurrentQueue.Enqueue(DeclinePlayerEvent);
                            return DeclinePlayerEvent.ID;
                        case 12:
                            GSGameEvent LeavePlayerEvent = new GSGameEvent(account, EEventType.LeavePlayer, message.Message);
                            ServerLinks.Events.CurrentQueue.Enqueue(LeavePlayerEvent);
                            return LeavePlayerEvent.ID;
                            */
                    }
                    break;
                case 6:
                    switch (message.Field2)
                    {
                        case 2:
                            return ChangePilotsRelease(player, message.Message);
                        case 3:
                            return HirePilot(player, message.Message);
                        case 5:
                            return DismissPilot(player, message.Message);
                        case 6:
                            return PutPilotToShip(player, message.Message);
                        case 7:
                            return MovePilotFromShip(player, message.Message);
                        case 8:
                            return ShowMarkets(player, message.Message);
                        case 9:
                            return TryChangeNew(player, message.Message);
                        case 10:
                            return LeaveMarket(player, message.Message);
                        case 11:
                            return CreateGalaxyMarket(player, message.Message);
                        case 12:
                            return ShowBlackMarket(player, message.Message);
                    }
                    break;
                case 7:
                    switch (message.Field2)
                    {
                        case 1:
                            return CreateFleet(player, message.Message);
                        case 3:
                            return MoveShipToFleet(player, message.Message);
                        case 4:
                            return ChangeFleetParams(player, message.Message);
                        case 5:
                            return RemoveShipFromFleet(player, message.Message);
                    }
                    break;
                case 8:
                    switch (message.Field2)
                    {
                        case 1: throw new Exception();
                            //GSGameEvent EnemyBattle = new GSGameEvent(account, EEventType.StartAttackEnemyBattle, message.Message);
                            //ServerLinks.Events.CurrentQueue.Enqueue(EnemyBattle);
                            //return EnemyBattle.ID;
                        case 2:
                            return StartColonizeLand(player, message.Message);
                        case 3:
                            return SendFleetToDefensePlayer(player, message.Message);
                        case 4:
                            return RemoveFleet(player, message.Message);
                        case 5:
                            return SendFleetToPillage(player, message.Message);
                        case 6:
                            return SetFleetAsLandDefender(player, message.Message);
                        case 7:
                            return RemoveFleetAsLandDefender(player, message.Message);
                        case 8:
                            return SendFleetToConquer(player, message.Message);
                        //case 9:
                        //    return StartResourceMissionBattle(player, message.Message);
                        case 10:
                            return StartStoryLineMission(player, message.Message);
                        case 11:
                            return StartScout(player, message.Message);
                        case 12:
                            return StartMission2Battle(player, message.Message);
                    }
                    break;
                case 9:
                    switch (message.Field2)
                    {
                        case 1:
                            return GetPlanetLands(player, message.Message);
                    }
                    break;
                case 10:
                    throw new Exception();/*
                    switch (message.Field2)
                    {
                        case 1:
                            GSGameEvent ActivatePremium = new GSGameEvent(account, EEventType.ActivatePremium, message.Message);
                            ServerLinks.Events.CurrentQueue.Enqueue(ActivatePremium);
                            return ActivatePremium.ID;
                        case 2:
                            GSGameEvent BattleLogEvent = new GSGameEvent(account, EEventType.GetBattleLogs, message.Message);
                            ServerLinks.Events.CurrentQueue.Enqueue(BattleLogEvent);
                            return BattleLogEvent.ID;
                        case 4:
                            GSGameEvent QuestSimpleFinishEvent = new GSGameEvent(account, EEventType.FinishSimpleQuest, message.Message);
                            ServerLinks.Events.CurrentQueue.Enqueue(QuestSimpleFinishEvent);
                            return QuestSimpleFinishEvent.ID;
                    }
                    break;*/
                case 11:
                    switch (message.Field2)
                    {
                        case 1:
                            return UsePeaceArtefact(player, message.Message);
                        case 2:
                            return PutWarArtefactInFleet(player, message.Message);
                        case 3:
                            return RemoveWarArtefactInFleet(player, message.Message);
                    }
                    break;
            }
            throw new Exception();

        }
    }
   
}
