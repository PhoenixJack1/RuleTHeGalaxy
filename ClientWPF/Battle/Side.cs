using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class Side
    {
        public SortedList<byte, ShipB> Ships;
        public byte Behavior;
        public long FleetID;
        public ShipSide SSide;
        public Battle CurBattle;
        public List<ShipB> OnField = new List<ShipB>();
        public List<ShipB> OnBase = new List<ShipB>();
        public List<ShipB> OnPort = new List<ShipB>();
        public List<ShipB> OnGate = new List<ShipB>();
        public List<ShipB> Portals = new List<ShipB>();
        public ShipB[] Gate = new ShipB[5];
        /// <summary> список команд для  </summary>
        public List<BattleCommand> CurrentCommand=new List<BattleCommand>();
        public SortedList<byte, SideArtefactInfo> Artefacts = new SortedList<byte, SideArtefactInfo>(); //Ключ - позиция артефакта, значение массив. 0 - Артефакт, 1 - время ожидания
        public Side(long fleetid, byte behavior, SortedList<byte, ShipB> ships, ShipSide side, Battle battle)
        {
            FleetID = fleetid;
            Behavior = behavior;
            Ships = ships;
            foreach (ShipB ship in Ships.Values)
                ship.Side = this;
            SSide = side;
            CurBattle = battle;
            //foreach (ShipB ship in Ships.Values)
            //    if (ship.States.IsPortal)
            //        Portal = ship;
        }
        public void StartBattle(bool IsVisual)
        {
            foreach (ShipB ship in Ships.Values)
            {
                
                if (ship.States.IsPortal)
                {
                    byte portalhex = ship.CurHex;
                    if (portalhex > CurBattle.Field.MaxHex)
                        portalhex = (byte)(SSide == ShipSide.Attack ? 0 : CurBattle.Field.MaxHex);
                    if (CurBattle.BattleField.ContainsKey(portalhex)) portalhex = 220;
                    ship.SetHex(portalhex);
                    if (portalhex <= CurBattle.Field.MaxHex)
                    {
                        OnField.Add(ship);
                        CurBattle.BattleField.Add(ship.CurHex, ship);
                        Portals.Add(ship);
                    }
                    //CurBattle.BattleFieldInfo.Add(string.Format("Постановка портала до начала боя Ataka={0} ID={1} Hex={2}", ship.States.Side, ship.BattleID, ship.CurHex));
                }
                else if (ship.States.BigSize)
                {
                    OnField.Add(ship);
                    CurBattle.BattleField.Add(ship.CurHex, ship);
                    //CurBattle.BattleFieldInfo.Add(string.Format("Постановка бигшипа до начала боя Ataka={0} ID={1} Hex={2}", ship.States.Side, ship.BattleID, ship.CurHex));

                }
                else if (ship.CurHex < CurBattle.Field.MaxHex)
                {
                    OnField.Add(ship);
                    CurBattle.BattleField.Add(ship.CurHex, ship);
                    ship.SetAngle((SSide == ShipSide.Attack ? 150 : 300), true);
                    //CurBattle.BattleFieldInfo.Add(string.Format("Постановка грузового корабля до начала боя Ataka={0} ID={1} Hex={2}", ship.States.Side, ship.BattleID, ship.CurHex));
                }
                else if (ship.StartHealth == 0)
                {
                    //ship.PanelB.Location.SetHex(220);
                    ship.SetHex(220);
                }
                
                else
                {
                    ship.SetHex(200);
                    //ship.PanelB.Location.SetHex(200);
                    OnBase.Add(ship);
                }
                if (IsVisual)
                    ship.AppendAllParams();
            }
           
        }
        public int GetFreeGateHex()
        {
            for (int i = 0; i < 5; i++)
                if (Gate[i] == null) return i;
            return -1;
        }
        ///<summary>Метод выдаёт хексы, на которые может прыгнуть корабль</summary>
        public List<byte> GetJumpHexes(byte range)
        {
            if (range > 2) throw new Exception();
            SortedSet<Hex> result = new SortedSet<Hex>();
            foreach (ShipB ship in Portals)
                foreach (Hex hex in ship.JumpHexes[range])
                    result.Add(hex);
            List<byte> answer = new List<byte>();
            foreach (Hex hex in result)
                if (CurBattle.BattleField.ContainsKey(hex.ID)==false)
                    answer.Add(hex.ID);
            return answer;
        }
        public void RoundStart(bool IsVisual)
        {
            foreach (ShipB ship in OnField)
                ship.RoundStart(IsVisual);
            for (int i = 0; i < OnPort.Count; i++)
            {
                if (OnPort[i].CurHex == 201)
                {
                    OnGate.Add(OnPort[i]);
                    OnPort[i].MoveToGate(IsVisual);
                    OnPort.Remove(OnPort[i]);
                    i--;
                }
                else OnPort[i].MoveInQueue(IsVisual);
            }
        }
        public void RoundEnd(bool IsVisual)
        {
            foreach (ShipB ship in OnField)
                ship.RoundEnd(IsVisual);
            foreach (KeyValuePair<byte, SideArtefactInfo> pair in Artefacts)
                if (pair.Value.WaitTime > 0)
                {
                    pair.Value.WaitTime--;
                    if (IntBoya.ControlledSide == SSide)
                        IntBoya.Artefacts[pair.Key].SetWaitTime(pair.Value.WaitTime);
                }
        }
        public void MoveShipToPort(ShipB ship, Hex Gatehex, int GatePos, bool IsVisual)
        {
            OnBase.Remove(ship);
            OnPort.Add(ship);
            Gate[GatePos] = ship;
            ship.MoveShipToPort(Gatehex, IsVisual);
        }
        public void FreeGate(ShipB ship)
        {
            for (int i = 0; i < 5; i++)
                if (Gate[i] == ship) Gate[i] = null;
            OnGate.Remove(ship);
            OnField.Add(ship);
        }
        public void MoveShipToField(ShipB ship, byte hex)
        {
            ship.MoveToField(hex);
            RecieveGoodEffectsFromShipPlace(ship, false, null);
            FreeGate(ship);
        }
        public void RecieveGoodEffectsFromShipMove(ShipB ship, byte lasthex, bool IsVisual)
        {
            if (ship.States.CritProtected) return;
            Hex LastHex = HexCanvas.Hexes[lasthex];
            Hex NewHex = HexCanvas.Hexes[ship.CurHex];
            List<Hex> Array = new List<Hex>(NewHex.NearHexes);
            foreach (Hex hex in LastHex.NearHexes)
                if (Array.Contains(hex)) Array.Remove(hex);
            RecieveGoodEffectsFromShipPlace(ship, IsVisual, Array.ToArray());
        }
        /// <summary> метод снимающий положительные эффекты с кораблей при перемещении </summary>
        public void RemoveGoodEffectsFromShipMove(ShipB ship, byte newhex, bool IsVisual)
        {
            if (ship.States.CritProtected) return;
            if (ship.CurHex > CurBattle.Field.MaxHex) return;
            Hex LastHex =  HexCanvas.Hexes[ship.CurHex];
            Hex NewHex = HexCanvas.Hexes[newhex];
            List<Hex> Array = new List<Hex>(LastHex.NearHexes);
            foreach (Hex hex in NewHex.NearHexes)
                if (Array.Contains(hex)) Array.Remove(hex);
            RemoveGoodEffectsFromShip(ship, IsVisual, Array.ToArray(), "перемещение1");
        }
        public void RemoveGoodEffectsFromShip(ShipB ship, bool IsVisual, Hex[] array, string reason)
        {
            if (ship.States.CritProtected) return;
            Hex hex = HexCanvas.Hexes[ship.CurHex];
            if (array == null) array = hex.NearHexes;
            foreach (Hex nearhex in array)
            {
                if (CurBattle.BattleField.ContainsKey(nearhex.ID))
                {
                    ShipB nearship = CurBattle.BattleField[nearhex.ID];
                    if (nearship.States.CritProtected) continue;
                    if (nearship.States.Side != SSide) continue;
                    if (ship.GroupEffects.Count != 0)
                    {
                        nearship.RemoveGoodEffectsFromShip(ship, ship.GroupEffects, IsVisual, reason);
                    }
                    if (nearship.GroupEffects.Count != 0)
                        ship.RemoveGoodEffectsFromShip(nearship, nearship.GroupEffects, IsVisual, reason);
                }
            }
        }
        public void RecieveGoodEffectsFromShipPlace(ShipB ship, bool IsVisual, Hex[] array)
        {

            Hex hex =  HexCanvas.Hexes[ship.CurHex];
            if (array == null) array = hex.NearHexes;
            foreach (Hex nearhex in array)
            {
                if (CurBattle.BattleField.ContainsKey(nearhex.ID))
                {
                    ShipB nearship = CurBattle.BattleField[nearhex.ID];
                    if (nearship.States.CritProtected) continue;
                    if (nearship.States.Side != SSide) continue;
                    if (ship.GroupEffects.Count!=0)
                        nearship.RecieveGoodEffectsFromShip(ship, ship.GroupEffects, IsVisual,"вход1");
                    if (nearship.GroupEffects.Count!=0)
                        ship.RecieveGoodEffectsFromShip(nearship, nearship.GroupEffects, IsVisual,"вход2");
                }
            }
        }
        public string CheckArtefactAvailable(byte pos)
        {
            Artefact art = Artefacts[pos].Artefact;
            //Расчёт затрат энергии
            int portalsum = 0;
            foreach (ShipB ship in Portals)
                portalsum += ship.Params.Health.GetCurrent;
            double percenthealth = art.EnergyCost / (double)portalsum;
            if (percenthealth > 1.0) return "Недостаточно энергии портала";
            switch (art.Ability)
            {
                case ArtefactAbility.BEnergyBlock: case ArtefactAbility.BPhysicBlock: case ArtefactAbility.BIrregularBlock:
                case ArtefactAbility.BCyberBlock:
                case ArtefactAbility.BEnergyIncrease: case ArtefactAbility.BPhysicIncrease: case ArtefactAbility.BIrregularIncrease: case ArtefactAbility.BCyberIncrease:
                    if (CurBattle.Restriction != Restriction.None) return "Модификатор уже активирован";
                    break;
            }
            return "";
        }
        public void UseArtefact(byte pos, byte hex1, byte hex2, bool fromServer)
        {
            Artefact art = Artefacts[pos].Artefact;
            //Расчёт затрат энергии
            int portalsum = 0;
            foreach (ShipB ship in Portals)
                portalsum += ship.Params.Health.GetCurrent;
            double percenthealth = art.EnergyCost / (double)portalsum;
            CurBattle.CurrentTurn++;
            
            DebugWindow.AddTB1("Ход " + IntBoya.Battle.CurrentTurn.ToString() + " ", true);
            DebugWindow.AddTB1(String.Format("Использование артефакта {0} {1} хекс1 = {2} хекс2 ={3}", SSide == ShipSide.Attack ? "атаки" : "защиты", art.GetName(), hex1, hex2), false);
            //Установка времени
            Artefacts[pos].WaitTime = art.WaitTime;
            if (SSide == IntBoya.ControlledSide)
                IntBoya.Artefacts[pos].SetWaitTime(art.WaitTime);
            //Потеря энергии порталами
            List<ShipB> portallist = new List<ShipB>(); portallist.AddRange(Portals);

            foreach (ShipB ship in portallist)
            {
                int spenthealth = (int)Math.Round(ship.Params.Health.GetCurrent * percenthealth, 0);
                DamageResult damageresult = ship.RecieveDamage(0, spenthealth);
                if (damageresult.IsDestroyed && fromServer == false)
                {
                    IntBoya.Battle.CurrentTurn++;
                    DebugWindow.AddTB1("Ход " + IntBoya.Battle.CurrentTurn.ToString() + " ", true);
                    IntBoya.Battle.ShipDestroyed(ship.CurHex, true);
                }
                else
                {
                    ship.ShowMessages(-damageresult.ToHealth, -damageresult.ToShield, 0);
                    ship.AppendAllParams();
                }
            }
           
            BW.SetWorking("Использование артефакта", true);
            switch (art.Ability)
            {
                case ArtefactAbility.BShipMove:
                    ShipB movedship = IntBoya.Battle.BattleField[hex1];
                    Hex targethexmove = HexCanvas.Hexes[hex2];
                    BattleController.EnterShipToField(movedship, targethexmove);
                    CurBattle.BattleField.Remove(hex1);
                    CurBattle.BattleField.Add(hex2, movedship);
                    break;
                case ArtefactAbility.BShipRestore:
                    ShipB restoreship = IntBoya.Battle.BattleField[hex1];
                    restoreship.SaveParams();
                    restoreship.Params.Health.SetMax();
                    restoreship.Params.Shield.SetMax();
                    restoreship.Params.Energy.SetMax();
                    restoreship.Params.Status.Clear();
                    restoreship.AppendChanges();
                    HexCanvas.AddInfo();
                    BW.SetWorking("Артефакт использован", false);
                        break;
                case ArtefactAbility.BAsteroidMove:
                    ShipB movedart = IntBoya.Battle.BattleField[hex1];
                    Hex targethex = HexCanvas.Hexes[hex2];
                    BattleController.EnterShipToField(movedart, targethex);
                    CurBattle.BattleField.Remove(hex1);
                    CurBattle.BattleField.Add(hex2, movedart);
                    CurBattle.Asteroids.Remove(hex1);
                    CurBattle.Asteroids.Add(hex2, movedart);
                    break;
                case ArtefactAbility.BPortalMove:
                    ShipB movedportal = IntBoya.Battle.BattleField[hex1];
                    Hex targethexmoveportal = HexCanvas.Hexes[hex2];
                    BattleController.EnterShipToField(movedportal, targethexmoveportal);
                    CurBattle.BattleField.Remove(hex1);
                    CurBattle.BattleField.Add(hex2, movedportal);
                    break;
                case ArtefactAbility.BEnergyBlock:
                    CurBattle.Restriction = Restriction.NoEnergy; CurBattle.RestrictionLength = art.Param1;
                    IntBoya.ShowRestrictionInfo(); BW.SetWorking("Артефакт использован", false); HexCanvas.AddInfo(); break;
                case ArtefactAbility.BPhysicBlock:
                    CurBattle.Restriction = Restriction.NoPhysic; CurBattle.RestrictionLength = art.Param1;
                    IntBoya.ShowRestrictionInfo(); BW.SetWorking("Артефакт использован", false); HexCanvas.AddInfo(); break;
                case ArtefactAbility.BIrregularBlock:
                    CurBattle.Restriction = Restriction.NoIrregular; CurBattle.RestrictionLength = art.Param1;
                    IntBoya.ShowRestrictionInfo(); BW.SetWorking("Артефакт использован", false); HexCanvas.AddInfo(); break;
                case ArtefactAbility.BCyberBlock:
                    CurBattle.Restriction = Restriction.NoCyber; CurBattle.RestrictionLength = art.Param1;
                    IntBoya.ShowRestrictionInfo(); BW.SetWorking("Артефакт использован", false); HexCanvas.AddInfo(); break;
                case ArtefactAbility.BEnergyIncrease:
                    CurBattle.Restriction = Restriction.DoubleEnergy; CurBattle.RestrictionLength = art.Param1;
                    IntBoya.ShowRestrictionInfo(); BW.SetWorking("Артефакт использован", false); HexCanvas.AddInfo(); break;
                case ArtefactAbility.BPhysicIncrease:
                    CurBattle.Restriction = Restriction.DoublePhysic; CurBattle.RestrictionLength = art.Param1;
                    IntBoya.ShowRestrictionInfo(); BW.SetWorking("Артефакт использован", false); HexCanvas.AddInfo(); break;
                case ArtefactAbility.BIrregularIncrease:
                    CurBattle.Restriction = Restriction.DoubleIrregular; CurBattle.RestrictionLength = art.Param1;
                    IntBoya.ShowRestrictionInfo(); BW.SetWorking("Артефакт использован", false); HexCanvas.AddInfo(); break;
                case ArtefactAbility.BCyberIncrease:
                    CurBattle.Restriction = Restriction.DoubleCyber; CurBattle.RestrictionLength = art.Param1;
                    IntBoya.ShowRestrictionInfo(); BW.SetWorking("Артефакт использован", false); HexCanvas.AddInfo(); break;
                case ArtefactAbility.BEnergyDamage:
                    BattleController.MakeEnergyDamage(hex1, fromServer, art); break;
                case ArtefactAbility.BPhysicDamage:
                    BattleController.MakePhysicDamage(hex1, fromServer, art); break;
                case ArtefactAbility.BIrregularDamage:
                    BattleController.MakeIrregularDamage(SSide, fromServer, art); break;
                case ArtefactAbility.BCyberDamage:
                    BattleController.MakeCyberDamage(hex1, hex2, fromServer, art); break;
            }
        }
    }
    public class SideArtefactInfo
    {
        public Artefact Artefact;
        public byte WaitTime;
        public SideArtefactInfo(Artefact artefact)
        {
            Artefact = artefact; WaitTime = artefact.WaitTime;
        }
    }
}
