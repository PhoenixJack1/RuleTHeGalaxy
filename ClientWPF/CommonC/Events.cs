using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Client
{
    class Events
    {
        static int EventOrder = 0;
        public static void Cheat()
        {
            ByteMessage message = new ByteMessage(1, 10, new byte[0]);
            ByteMessage answer = ProcessEvent(message, true);
        }
        public static byte[] GetBlackMarket()
        {
            ByteMessage message = new ByteMessage(6, 12, new byte[0]);
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return answer.Message;
            else throw new Exception("Ошибка при считывании чёрного рынка");
        }
        public static byte[] GetMarkets()
        {
            ByteMessage message = new ByteMessage(6, 8, new byte[0]);
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return answer.Message;
            else throw new Exception("Ошибка при считывании рынка");
        }
        public static string TryMarketChange(byte markettype, byte element, int quantity)
        {
            List<byte> list = new List<byte>();
            list.Add(markettype);
            list.Add(element);
            list.AddRange(BitConverter.GetBytes(quantity));
            ByteMessage message = new ByteMessage(6, 9, list.ToArray());
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static StartBattleEventResult TryLeaveMarket(byte markettype)
        {
            List<byte> list = new List<byte>();
            list.Add(markettype);
            ByteMessage message = new ByteMessage(6, 10, list.ToArray());
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes)
            {
                long marketbattleid = BitConverter.ToInt64(answer.Message, 0);
                return new StartBattleEventResult(marketbattleid);
            }
             else
                return new StartBattleEventResult(Common.GetStringFromByteArray(answer.Message, 0));
        }
        public static string TryCreateGalaxyMarket()
        {
            ByteMessage message = new ByteMessage(6, 11, new byte[0]);
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string RemoveWarArtefactFromFleet(GSFleet fleet, Artefact artefact)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(fleet.FleetBase.Land.ID));
            list.AddRange(BitConverter.GetBytes(fleet.ID));
            list.AddRange(BitConverter.GetBytes(artefact.ID));
            ByteMessage message = new ByteMessage(11, 3, list.ToArray());
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string PutWarArtefactInFleet(GSFleet fleet)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(fleet.FleetBase.Land.ID));
            list.AddRange(BitConverter.GetBytes(fleet.ID));
            list.AddRange(BitConverter.GetBytes(Links.Helper.ArtefactID));
            ByteMessage message = new ByteMessage(11, 2, list.ToArray());
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static ByteMessage UsePeaceArtefact(ushort ArtefactID, byte[] information)
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(ArtefactID));
            if (information != null) list.AddRange(information);
            ByteMessage message = new ByteMessage(11, 1, list.ToArray());
            ByteMessage answer = ProcessEvent(message, true);
            return answer;
        }
        public static string ActivatePremium(byte period)
        {
            ByteMessage message = new ByteMessage(10, 1, new byte[] { period });
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes)
                return "";
            else
                return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string UpdateHelps()
        {
            ByteMessage message = new ByteMessage(0, 12, GSGameInfo.HelpArray);
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes)
                return "";
            else
                return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string LeavePlayer(int playerid)
        {
            ByteMessage message = new ByteMessage(5, 12, BitConverter.GetBytes(playerid));
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes)
                return "";
            else
                return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string DeclinePlayer(int playerid)
        {
            ByteMessage message = new ByteMessage(5, 11, BitConverter.GetBytes(playerid));
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes)
                return "";
            else
                return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string InvitePlayer(int playerid)
        {
            ByteMessage message = new ByteMessage(5, 10, BitConverter.GetBytes(playerid));
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes)
                return "";
            else
                return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static byte[] GetRequestList()
        {
            ByteMessage message = new ByteMessage(5, 9, null);
            ByteMessage answer = ProcessEvent(message, true);
            List<byte> fullanswer = new List<byte>();
            if (answer.Type == MessageResult.Yes)
                fullanswer.Add(0);
            else
                fullanswer.Add(1);
            fullanswer.AddRange(answer.Message);
            return fullanswer.ToArray();
        }
        public static string RemoveInviteRequest()
        {
            ByteMessage message = new ByteMessage(5, 8, null);
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes)
                return "";
            else
                return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string SendInviteRequest(int clanid)
        {
            ByteMessage message = new ByteMessage(5, 7, BitConverter.GetBytes(clanid));
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes)
                return "";
            else
                return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string LeaveClanEvent()
        {
            ByteMessage message = new ByteMessage(5, 6, null);
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes)
                return "";
            else
                return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static byte[] GetSelfClanInfo()
        {
            ByteMessage message = new ByteMessage(5, 5, null);
            ByteMessage answer = ProcessEvent(message, true);
            List<byte> fullanswer = new List<byte>();
            if (answer.Type == MessageResult.Yes)
                fullanswer.Add(0);
            else
                fullanswer.Add(1);
            fullanswer.AddRange(answer.Message);
            return fullanswer.ToArray();
        }
        public static byte[] GetClansList(int clanid)
        {
            ByteMessage message = new ByteMessage(5, 4, BitConverter.GetBytes(clanid));
            ByteMessage answer = ProcessEvent(message, true);
            List<byte> fullanswer = new List<byte>();
            if (answer.Type == MessageResult.Yes)
                fullanswer.Add(0);
            else
                fullanswer.Add(1);
            fullanswer.AddRange(answer.Message);
            return fullanswer.ToArray();
        }
        public static string CreateNewClan(long ClanImage, GSString ClanName)
        {
            List<byte> array = new List<byte>();
            array.AddRange(BitConverter.GetBytes(ClanImage));
            array.AddRange(ClanName.Array);
            ByteMessage message = new ByteMessage(5, 3, array.ToArray());
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes)
                return "";
            else
                return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static byte[] GetPlanetLandsList(int planetID)
        {
            ByteMessage message = new ByteMessage(9, 1, BitConverter.GetBytes(planetID));
            ByteMessage ResultMessage = ProcessEvent(message, true);
            if (ResultMessage.Type == MessageResult.Yes)
                return ResultMessage.Message;
            else
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Common.GetStringFromByteArray(ResultMessage.Message, 0)), true);
                return null;
            }
        }
        ///<summury>метод передаёт ходы игрока в тестовом бою</summury>
        public static string SetBattleMoveListTest(Battle battle)
        {
            List<byte> side1moves = new List<byte>();
            foreach (BattleCommand command in battle.Side1.CurrentCommand)
                side1moves.AddRange(command.Array);
            byte[] AnswerToQuery=LocalServer.SetTestBattleMoveList(BitConverter.GetBytes(battle.ID), side1moves.ToArray());
            battle.Side1.CurrentCommand.Clear();
            //byte[] AnswerToQuery = Links.Loginproxy.SetTestBattleMoveList(BitConverter.GetBytes(battle.ID), side1moves.ToArray());
            ByteMessage result = new ByteMessage(AnswerToQuery);
            if (result.Type == MessageResult.Yes) return result.Message[0] == 0 ? "0" : "1";
            else return Common.GetStringFromByteArray(result.Message, 0);
        }
        ///<summury>метод передаёт ходы игрока</summury>
        public static string SetBattleMoveList(Battle battle)
        {
            List<byte> side1moves = new List<byte>();
            foreach (BattleCommand command in battle.Side1.CurrentCommand)
                side1moves.AddRange(command.Array);
            List<byte> side2moves = new List<byte>();
            foreach (BattleCommand command in battle.Side2.CurrentCommand)
                side2moves.AddRange(command.Array);
            byte[] AnswerToQuery;
            if (Links.GameMode == EGameMode.Multi)
                AnswerToQuery = Links.Loginproxy.SetBattleMoveList(BitConverter.GetBytes(GSGameInfo.random), BitConverter.GetBytes(battle.ID), side1moves.ToArray(), side2moves.ToArray());
            else
            {
                AnswerToQuery = LocalServer.SetBattleMoveList(BitConverter.GetBytes(battle.ID), side1moves.ToArray(), side2moves.ToArray());
                battle.Side1.CurrentCommand.Clear();
                battle.Side2.CurrentCommand.Clear();
            }
                ByteMessage result = new ByteMessage(AnswerToQuery);
            if (result.Type == MessageResult.Yes) return result.Message[0] == 0 ? "0" : "1";
            else return Common.GetStringFromByteArray(result.Message, 0);
        }
        ///<summary> метод устанавливает бой в автоматический режим</summary>
        public static bool SetBattleToAutoMode(Battle battle)
        {
            byte[] AnswerToQuery;
            if (Links.GameMode == EGameMode.Multi)
                AnswerToQuery = Links.Loginproxy.SetBattleToAutoMode(BitConverter.GetBytes(battle.ID), BitConverter.GetBytes(GSGameInfo.random));
            else
                AnswerToQuery = LocalServer.SetBattleToAutoMode(BitConverter.GetBytes(battle.ID));
            ByteMessage ResultMessage = new ByteMessage(AnswerToQuery);
            if (ResultMessage.Message[0] == 0) return true; else return false;
        }
        public static bool SetBattleToAutoMode(long battleID)
        {
            byte[] AnswerToQuery;
            if (Links.GameMode == EGameMode.Multi)
                AnswerToQuery = Links.Loginproxy.SetBattleToAutoMode(BitConverter.GetBytes(battleID), BitConverter.GetBytes(GSGameInfo.random));
            else
                AnswerToQuery = LocalServer.SetBattleToAutoMode(BitConverter.GetBytes(battleID));
            ByteMessage ResultMessage = new ByteMessage(AnswerToQuery);
            if (ResultMessage.Message[0] == 0) return true; else return false;
        }
        public static StartBattleEventResult SendFleetToConquer()
        {
            GSFleet fleet = Links.Helper.Fleet;
            GSPlanet planet = Links.Helper.Planet;
            List<byte> array = new List<byte>();

            array.AddRange(BitConverter.GetBytes(fleet.FleetBase.Land.ID));
            array.AddRange(BitConverter.GetBytes(fleet.ID));
            array.AddRange(BitConverter.GetBytes(planet.ID));
            ByteMessage message = new ByteMessage(8, 8, array.ToArray());
            ByteMessage ResultMessage = ProcessEvent(message, true);
            if (ResultMessage.Type == MessageResult.Yes)
            {
                long battleid = BitConverter.ToInt64(ResultMessage.Message, 0);
                return new StartBattleEventResult(battleid);
            }
            else return new StartBattleEventResult(Common.GetStringFromByteArray(ResultMessage.Message, 0));
        }
        #region SendToMission
        public static StartBattleEventResult SendFleetToPillage()
        {
            GSFleet fleet = Links.Helper.Fleet;
            GSPlanet planet = Links.Helper.Planet;
            List<byte> array = new List<byte>();

            array.AddRange(BitConverter.GetBytes(fleet.FleetBase.Land.ID));
            array.AddRange(BitConverter.GetBytes(fleet.ID));
            array.AddRange(BitConverter.GetBytes(planet.ID));
            ByteMessage message = new ByteMessage(8, 5, array.ToArray());
            ByteMessage ResultMessage = ProcessEvent(message, true);
            if (ResultMessage.Type == MessageResult.Yes)
            {
                long battleid = BitConverter.ToInt64(ResultMessage.Message, 0);
                return new StartBattleEventResult(battleid);
            }
            else return new StartBattleEventResult(Common.GetStringFromByteArray(ResultMessage.Message, 0));
        }
       
        public static string[] StartColonizeLand(GSPlanet planet)
        {
            List<byte> array = new List<byte>();
            array.AddRange(BitConverter.GetBytes(planet.ID));
            ByteMessage message = new ByteMessage(8, 2, array.ToArray());
            ByteMessage ResultMessage = ProcessEvent(message, true);
            if (ResultMessage.Type == MessageResult.Yes)
            {
                int avanpostid = BitConverter.ToInt32(ResultMessage.Message, 0);
                return new string[] { "0", avanpostid.ToString() };
            }
            else return new string[] { "1", Common.GetStringFromByteArray(ResultMessage.Message, 0) };
        }
        
        public static StartBattleEventResult SendFleetToScout()
        {
            List<byte> array = new List<byte>();
            array.AddRange(BitConverter.GetBytes(Links.Helper.Fleet.FleetBase.Land.ID));
            array.AddRange(BitConverter.GetBytes(Links.Helper.Fleet.ID));
            array.AddRange(BitConverter.GetBytes(Links.Helper.Star.ID));
            ByteMessage message = new ByteMessage(8, 11, array.ToArray());
            ByteMessage ResultMessage = ProcessEvent(message, true);
            if (ResultMessage.Type == MessageResult.Yes)
            {
                long battleid = BitConverter.ToInt64(ResultMessage.Message, 0);
                if (battleid == -1)
                    return new StartBattleEventResult("");
                else
                    return new StartBattleEventResult(battleid);
            }
            else
                return new StartBattleEventResult(Common.GetStringFromByteArray(ResultMessage.Message, 0));
        }
        public static string RemoveFleet(GSFleet fleet)
        {
            List<byte> array = new List<byte>();
            array.AddRange(BitConverter.GetBytes(fleet.FleetBase.Land.ID));
            array.AddRange(BitConverter.GetBytes(fleet.ID));
            ByteMessage message = new ByteMessage(8, 4, array.ToArray());
            ByteMessage ResultMessage = ProcessEvent(message, true);
            if (ResultMessage.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(ResultMessage.Message, 0);
        }
        public static StartBattleEventResult SendFleetToMission2Battle(FleetParamsPanel panel, Mission2 mission)
        {
            List<byte> array = new List<byte>();
            array.AddRange(BitConverter.GetBytes(panel.Fleet.FleetBase.Land.ID));
            array.AddRange(BitConverter.GetBytes(panel.Fleet.ID));
            array.AddRange(BitConverter.GetBytes(mission.StarID));
            array.Add(mission.Orbit);
            ByteMessage message = new ByteMessage(8, 12, array.ToArray());
            //Отправление флота на миссию. Может вернуться -1 - нет награды, -2 - малая награда, -3 - средная награда, -4 - чёрный рынок
            ///>=0 - ID боя, False - текст ошибки.
            ByteMessage ResultMessage = ProcessEvent(message, true);
            if (ResultMessage.Type == MessageResult.Yes)
            {
                resourcemissionbattleid = BitConverter.ToInt64(ResultMessage.Message, 0);
                return new StartBattleEventResult(resourcemissionbattleid);
            }
            else
                return new StartBattleEventResult(Common.GetStringFromByteArray(ResultMessage.Message, 0));
        }
        static long resourcemissionbattleid;
       
        public static StartBattleEventResult SendFleetToStoryLine(FleetParamsPanel panel)
        {
            List<byte> array = new List<byte>();
            if (panel != null) //если миссия боевая - то отправляем параметры флота
             {
                array.AddRange(BitConverter.GetBytes(panel.Fleet.FleetBase.Land.ID));
                array.AddRange(BitConverter.GetBytes(panel.Fleet.ID));
            }
            ByteMessage message = new ByteMessage(8, 10, array.ToArray());
            ByteMessage ResultMessage = ProcessEvent(message, true);
            if (ResultMessage.Type == MessageResult.Yes)
            {
                if (panel == null) return new StartBattleEventResult(true, "");
                resourcemissionbattleid = BitConverter.ToInt64(ResultMessage.Message, 0);
                return new StartBattleEventResult(BitConverter.ToInt64(ResultMessage.Message, 0));
                //new StartBattlePanel(resourcemissionbattleid);
            }
            else return new StartBattleEventResult(Common.GetStringFromByteArray(ResultMessage.Message, 0));
        }
        #endregion
        public static StartBattleEventResult SendFleetToAttackEnemyBattle(FleetParamsPanel panel, GSStar star)
        {
            List<byte> array = new List<byte>();
            array.AddRange(BitConverter.GetBytes(panel.Fleet.FleetBase.Land.ID));
            array.AddRange(BitConverter.GetBytes(panel.Fleet.ID));
            array.AddRange(BitConverter.GetBytes(star.ID));
            ByteMessage message = new ByteMessage(8, 1, array.ToArray());
            ByteMessage ResultMessage = ProcessEvent(message, true);
            if (ResultMessage.Type == MessageResult.Yes) return new StartBattleEventResult(BitConverter.ToInt64(ResultMessage.Message, 0));
            else return new StartBattleEventResult(Common.GetStringFromByteArray(ResultMessage.Message, 0));
        }
        public static string MoveShipToFleet(GSFleet fleet, GSShip Ship)
        {
            List<byte> array = new List<byte>();
            array.AddRange(BitConverter.GetBytes(fleet.FleetBase.Land.ID));
            array.AddRange(BitConverter.GetBytes(fleet.ID));
                array.AddRange(BitConverter.GetBytes(Ship.ID));
            ByteMessage message=new ByteMessage(7,3,array.ToArray());
            ByteMessage ResultMessage = ProcessEvent(message, true);
            if (ResultMessage.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(ResultMessage.Message, 0);
        }
        public static string RemoveShipFromFleet(GSShip Ship)
        {
            List<byte> array = new List<byte>();
            array.AddRange(BitConverter.GetBytes(Ship.Fleet.FleetBase.Land.ID));
            array.AddRange(BitConverter.GetBytes(Ship.Fleet.ID));
            array.AddRange(BitConverter.GetBytes(Ship.ID));
            ByteMessage message = new ByteMessage(7, 5, array.ToArray());
            ByteMessage ResultMessage = ProcessEvent(message, true);
            if (ResultMessage.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(ResultMessage.Message, 0);
        }
        public static string MovePilotFromShip(long shipID)
        {
            List<byte> array = new List<byte>();
            array.AddRange(BitConverter.GetBytes(shipID));
            ByteMessage message = new ByteMessage(6, 7, array.ToArray());
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);

        }
        public static string PutPilotToShip(long shipID, GSPilot pilot)
        {
            List<byte> array = new List<byte>();
            array.AddRange(BitConverter.GetBytes(shipID));
            array.AddRange(pilot.GetArray());
            ByteMessage message = new ByteMessage(6, 6, array.ToArray());
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);

        }
        public static string ChangeFleetParams(GSFleet fleet, bool repair)
        {
            List<byte> array = new List<byte>();
            array.AddRange(BitConverter.GetBytes(fleet.FleetBase.Land.ID));
            array.AddRange(BitConverter.GetBytes(fleet.ID));
            ByteMessage message = new ByteMessage(7, 4, array.ToArray());
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string CreateFleet(FleetBase sector, byte[] image)
        {
            List<byte> array = new List<byte>();
            array.AddRange(BitConverter.GetBytes(sector.Land.ID));
            array.Add((byte)sector.Position);
            array.AddRange(image);
            ByteMessage message = new ByteMessage(7, 1, array.ToArray());
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string DismissPilot(GSPilot pilot)
        {
            ByteMessage message = new ByteMessage(6, 5, pilot.GetArray());
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string HirePilot(byte pos)
        {
            ByteMessage message = new ByteMessage(6, 3, new byte[] { pos });
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string ChangePilotsRelease()
        {
            ByteMessage message = new ByteMessage(6, 2, new byte[0]);
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string ChangeResources(byte position, int value)
        {
            List<byte> array = new List<byte>();
            array.Add(position);
            array.AddRange(BitConverter.GetBytes(value));
            ByteMessage message = new ByteMessage(5, 2,array.ToArray());
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string BuildNewShip(Schema schema, int model)
        {
            List<byte> array = new List<byte>();
            array.AddRange(schema.GetCrypt());
            array.AddRange(BitConverter.GetBytes(model));
            ByteMessage message = new ByteMessage(3, 3, array.ToArray());
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string RepairShip(GSShip ship)
        {
            ByteMessage message = new ByteMessage(3, 5, BitConverter.GetBytes(ship.ID));
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string DestroyShip(GSShip ship)
        {
            ByteMessage message = new ByteMessage(3, 6, BitConverter.GetBytes(ship.ID));
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string ReplaceSectors(Land land, byte donor, byte target)
        {
            List<byte> request = new List<byte>();
            request.AddRange(BitConverter.GetBytes(land.ID));
            request.Add(donor);
            request.Add(target);
            ByteMessage message = new ByteMessage(4, 5, request.ToArray());
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string ChangeLandName (Land land, string name)
        {
            List<byte> request = new List<byte>();
            request.AddRange(BitConverter.GetBytes(land.ID));
            request.AddRange((new GSString(name)).Array);
            ByteMessage message = new ByteMessage(4, 6, request.ToArray());
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static byte[] GetBattleLog(long battleid)
        {
            List<byte> request = new List<byte>();
            request.AddRange(BitConverter.GetBytes(battleid));
            ByteMessage message = new ByteMessage(10, 2, request.ToArray());
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return answer.Message;
            else return null;
        }
        public static string UpgradeBuilding(Land land, int Position, GSBuilding building, GSBuilding version, byte count)
        {
            List<byte> request = new List<byte>();
            request.AddRange(BitConverter.GetBytes(land.ID));
            request.Add((byte)Position);
            request.AddRange(BitConverter.GetBytes(building.ID));
            request.AddRange(BitConverter.GetBytes(version.ID));
            request.Add(count);
            ByteMessage message = new ByteMessage(4, 4, request.ToArray());
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string RemoveBuilding(Land land, int Position, GSBuilding building, byte count)
        {
            List<byte> request = new List<byte>();
            request.AddRange(BitConverter.GetBytes(land.ID));
            request.Add((byte)Position);
            request.AddRange(BitConverter.GetBytes(building.ID));
            request.Add(count);
            ByteMessage message = new ByteMessage(4, 3, request.ToArray());
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string BuildNewBuilding(int landid, byte SectorPos, byte buildpos)
        {
            List<byte> request = new List<byte>();
            request.AddRange(BitConverter.GetBytes(landid));
            request.Add(SectorPos);
            request.Add(buildpos);
            ByteMessage message = new ByteMessage(4, 2, request.ToArray());
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string BuildNewSector(Land land, SectorTypes sector, int pos)
        {
            List<byte> request = new List<byte>();
            request.AddRange(BitConverter.GetBytes(land.ID));
            request.Add((byte)sector);
            request.Add((byte)pos);
            ByteMessage message = new ByteMessage(4, 9, request.ToArray());
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);

        }
        public static string BuildInNewColony(Avanpost land, int money, int metall, int chips, int anti)
        {
            List<byte> request = new List<byte>();
            request.AddRange(BitConverter.GetBytes(land.ID));
            request.AddRange(BitConverter.GetBytes(money));
            request.AddRange(BitConverter.GetBytes(metall));
            request.AddRange(BitConverter.GetBytes(chips));
            request.AddRange(BitConverter.GetBytes(anti));
            ByteMessage message = new ByteMessage(4, 8, request.ToArray());
            ByteMessage answer = ProcessEvent(message, true);
            if (answer.Type == MessageResult.Yes) return "";
            else return Common.GetStringFromByteArray(answer.Message, 0);
        }
        public static string SendChatMessage(ChatMessage chatmessage)
        {
            ByteMessage message;
            switch (chatmessage.Type)
            {
                case EChatMessageType.Clan: message = new ByteMessage(2, 1, chatmessage.GetText()); break;
                case EChatMessageType.Private: message = new ByteMessage(2, 2, chatmessage.GetTargetText()); break;
                default: message = new ByteMessage(2, 3, chatmessage.GetText()); break;
            }
            ProcessEvent(message, false);
            return "";
        }
        public static void AppendChatMessages()
        {
            ByteMessage message = new ByteMessage(2, 4, new byte[0]);
            ByteMessage ResultMessage = ProcessEvent(message, true);
            if (ResultMessage.Type == MessageResult.Yes)
            {
                SortedList<uint, ChatMessage> Messages = new SortedList<uint, ChatMessage>();
                for (int i = 0; i < ResultMessage.Message.Length;)
                {
                    ChatMessage chatmessage = ChatMessage.GetChatMessage(ResultMessage.Message, ref i);
                    Messages.Add(chatmessage.ID, chatmessage);
                }
                //Links.Controller.chatpanel.AddChatMessages(Messages);
            }
        }
        public static bool AppendShipSchemas(byte[] array)
        {
            ByteMessage message = new ByteMessage(3, 1, array);
            ByteMessage ResultMessage = ProcessEvent(message, true);
            if (ResultMessage.Type != MessageResult.Yes)
            {
                SimpleInfoMessage window = new SimpleInfoMessage(Common.GetStringFromByteArray(ResultMessage.Message, 0));
                Links.Controller.PopUpCanvas.Place(window, true);
            }
            return true;
        }
        public static bool SimpleFinishQuest()
        {
            ByteMessage message = new ByteMessage(10, 4, new byte[0]);
            ByteMessage ResultMessage = ProcessEvent(message, true);
            if (ResultMessage.Type == MessageResult.Yes)
                return true;
            else
                return false;
        }
        static ByteMessage ProcessEvent(ByteMessage message, bool NeedAnswer)
        {
            EventOrder++;
            byte[] AnswerToQuery;
            if (Links.GameMode==EGameMode.Multi)
                AnswerToQuery = Links.Loginproxy.ProcessEvent(BitConverter.GetBytes(GSGameInfo.random), message.GetBytes(), EventOrder);
            else
                AnswerToQuery = LocalServer.ProcessEvent(BitConverter.GetBytes(GSGameInfo.random), message.GetBytes(), EventOrder);
            if (!NeedAnswer) return new ByteMessage(MessageResult.Yes);
            ByteMessage result = new ByteMessage(AnswerToQuery);
            return result;
        }
      
        public static int LearnScience(ScienceLearn  level)
        {
            List<byte> information = new List<byte>();
            information.AddRange(BitConverter.GetBytes((int)level));
            ByteMessage message = new ByteMessage(1, 1, information.ToArray());

            ByteMessage ResultMessage = ProcessEvent(message, true);

            if (ResultMessage.Type == MessageResult.Yes)
            {
                ushort LearnedScience = BitConverter.ToUInt16(ResultMessage.Message, 0);
                GameScience science = Links.Science.GameSciences[LearnedScience];
                return science.ID;
                //Gets.GetResources();
                //Links.Controller.Science2Canvas._ElementsCanvas.LearnNewScinece(science, true);
                //Links.Controller.ScienceCanvas.LearnNewScience(science);
                //BigWindow.LearnScience(science.ID);
            }
            else
            {
                SimpleInfoMessage window = new SimpleInfoMessage(Common.GetStringFromByteArray(ResultMessage.Message, 0));
                Links.Controller.PopUpCanvas.Place(window, true);
                return 0;
            }

        }

    }
}

