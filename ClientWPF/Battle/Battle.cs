using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace Client
{
    public enum BattleType { Space, Planet, Test };
    public class Battle
    {
        public long ID;
        public long Fleet1ID;
        public long Fleet2ID;
        public byte[] Emblem1;
        public byte[] Emblem2;
        public byte[] Description;
        public string BackGround;
        public byte[] StartArray;
        public GameMove[] Moves;
        public Side Side1;
        public Side Side2;
        public SortedList<byte, ShipB> BattleField;
        //public List<string> BattleFieldInfo;
        public bool IsVisual;
        public short CurrentTurn = -1;
        public byte CurrentRound;
        BattleType BattleType;
        //public Reward Reward;
        public BattleField Field;
        public SortedList<byte, ShipB> Asteroids;
        public List<ShipB> BigShips = new List<ShipB>();
        public Restriction Restriction;
        public int RestrictionLength;
        public BattleMode CurMode;
        public byte[] RandomArray;
        public bool IsFinished;
        public Battle(long id, long fleet1ID, long fleet2ID, byte[] emblem1, byte[] emblem2, byte[] description, BattleType type)
        {
            ID = id;
            CurMode = (BattleMode)description[0];
            Field = Client.BattleField.Fields[description[1]];
            BattleType = type;
            Fleet1ID = fleet1ID; 
            Fleet2ID = fleet2ID; 
            Description = description; 
            Emblem1 = emblem1; 
            Emblem2 = emblem2;
        }
        public void ReconStartArray()
        {
            int j = 0;
            for (int fleet = 0; fleet < 2; fleet++)
            {
                long FleetID = BitConverter.ToInt64(StartArray, j); j += 8;
                byte fleetbehavior = StartArray[j]; j++;
                byte fleetshipscount = (byte)(StartArray[j]); j++;
                bool fleetcanrule = GSGameInfo.Fleets.ContainsKey(Fleet1ID);
                ShipSide fleetSide = fleet == 0 ? ShipSide.Attack : ShipSide.Defense;
                SortedList<byte, ShipB> fleetships = new SortedList<byte, ShipB>();
                for (byte i = 0; i < fleetshipscount; i++)
                {
                    ShipBArrayType format = (ShipBArrayType)StartArray[j]; j++;
                    switch (format)
                    {
                        case ShipBArrayType.Array:
                            ShipB ship = ShipB.GetShipFromArray(StartArray, ref j, i, fleetSide, this, fleetcanrule);
                            fleetships.Add(i, ship);
                            if (ship.States.BigSize == true) BigShips.Add(ship);
                            break;
                        case ShipBArrayType.Portal:
                            int portalhealthh = BitConverter.ToInt32(StartArray, j); j += 4;
                            byte portalhex = StartArray[j]; j++;
                            byte[] cryptname = new byte[4];
                            for (int k = 0; k < 4; k++, j++) cryptname[k] = StartArray[j];
                            //byte portalhex = (byte)(fleetSide == ShipSide.Attack ? 0 : Field.MaxHex);
                            fleetships.Add(i, ShipB.GetPortalShip(i, portalhealthh, fleetSide, this, fleetcanrule, portalhex, cryptname));
                            break;
                        case ShipBArrayType.Standard:
                            byte battleID = StartArray[j]; j++;
                            Schema schema = Schema.GetSchema(StartArray, ref j);
                            byte health = StartArray[j]; j++;
                            GSPilot pilot = new GSPilot(StartArray, j); j += 10;
                            byte[] model = Common.GetArray(StartArray, j, 4); j += 4;
                            byte hex = StartArray[j]; j++;
                            //if (schema.ShipType.WeaponCapacity == 2)
                            //    pilot = pilot;
                            fleetships.Add(i, new ShipB(battleID, schema, health, pilot, model, fleetSide, this, fleetcanrule, null, ShipBMode.Battle, hex));
                            break;
                      /*  case ShipBArrayType.BigShip:
                            BattleParams Params = new BattleParams();
                            Params.CalculateFromArray(StartArray, ref j);
                            byte hex = StartArray[j]; j++;
                            byte bigmodel = StartArray[j]; j++;
                            byte[] cryptnamebig = new byte[4]; for (int k = 0; k < 4; k++, j++) cryptnamebig[j] = StartArray[j];
                            ushort weapon1id = BitConverter.ToUInt16(StartArray, j); j += 2;
                            ushort weapon2id = BitConverter.ToUInt16(StartArray, j); j += 2;
                            ushort weapon3id = BitConverter.ToUInt16(StartArray, j); j += 2;
                            BattleWeapon[] weapons = new BattleWeapon[3];
                            weapons[0] = new BattleWeapon(weapon1id);
                            weapons[1] = new BattleWeapon(weapon2id);
                            weapons[2] = new BattleWeapon(weapon3id);
                            ShipB bigship = ShipB.GetBigShip(Params, i, ShipStates.GetBigShip(fleetSide), this, weapons, fleetcanrule, hex, bigmodel, cryptnamebig);
                            BigShips.Add(bigship);
                            //ShipB bigship = new ShipB(Params, i, ShipStates.GetBigShip(fleetSide), this, weapons, fleetcanrule, hex);

                            fleetships.Add(i, bigship);
                            break;*/
                       /* case ShipBArrayType.Cargo:
                            BattleParams CargoParams = new BattleParams();
                            CargoParams.CalculateFromArray(StartArray, ref j);
                            byte cargohex = StartArray[j]; j++;
                            byte cargomodel = StartArray[j]; j++;
                            byte[] cryptnamecargo = new byte[4]; for (int k = 0; k < 4; k++, j++) cryptnamecargo[j] = StartArray[j];
                            ShipB cargoship = ShipB.GetCargoShip(CargoParams, i, ShipStates.GetCargoShip(fleetSide), this, cargohex, cargomodel, cryptnamecargo);
                            //ShipB bigship = new ShipB(Params, i, ShipStates.GetBigShip(fleetSide), this, weapons, fleetcanrule, hex);
                            fleetships.Add(i, cargoship);
                            break;*/
                    }
                }
                Side side = new Side(FleetID, fleetbehavior, fleetships, fleetSide, this);
                if (fleet == 0) Side1 = side; else Side2 = side;
                byte artefactscount = StartArray[j]; j++;
                for (int i = 0; i < artefactscount; i++)
                { side.Artefacts.Add((byte)i, new SideArtefactInfo( Links.Artefacts[BitConverter.ToUInt16(StartArray, j)])); j+=2; }
            }


            Restriction = (Restriction)StartArray[j]; j++;
            RestrictionLength = BitConverter.ToInt32(StartArray, j); j += 4;
            Asteroids = new SortedList<byte, ShipB>();
            int descrpos = 2;
            byte astrocount = Description[descrpos];descrpos++;
            for (byte i=0;i<astrocount;i++)
            {
                byte hex = Description[descrpos]; descrpos++;
                byte astrmodel = Description[descrpos]; descrpos++;
                bool isbig = Description[descrpos] == 1; descrpos++;
                ShipB asteroid = ShipB.GetAsteroid(i, this, hex, astrmodel, isbig);//new AsteroidB(i, this,hex);
                Asteroids.Add(hex, asteroid);
            }
            byte backtype = Description[descrpos];descrpos++;
            byte backpos = Description[descrpos]; descrpos++;
            BackGround = String.Format("Back{0}_00{1}", backtype == 0 ? "P" : "S", backpos.ToString());
            RandomArray = new byte[100];
            int maxK = descrpos + 100;
            for (int i=0; descrpos < maxK; descrpos++, i++)
                RandomArray[i] = Description[descrpos];
        }
        public void CalcBattle(int StopTurn)
        {
            ReconStartArray();
            //TestBattle.AddBattle(ID);


           // foreach (ShipB ship in Side1.Ships.Values)
            //    TestBattle.AddShip(ID, ship);
            //foreach (ShipB ship in Side2.Ships.Values)
           //     TestBattle.AddShip(ID, ship);
            for (CurrentTurn = 0; CurrentTurn <= StopTurn; CurrentTurn++)
            {
                if (Moves.Length <= CurrentTurn) break;
                GameMove move = Moves[CurrentTurn];
                NextTurn(move, false);
            }
        }
        public void Update()
        {
            Side1 = null;
            Side2 = null;
            CurrentTurn = -1;
            Gets.UpdateBattle(this);
        }
        public void ProcessVisualNextTurn()
        {
            BW.SetWorking("ProcessVisualNextTurn", true);
            CurrentTurn++;
            bool isstop = false;
            //IntBoya.TurnsLabel.SetParams(CurrentRound, CurrentTurn, Moves.Length);
            if (Moves==null || Moves.Length <= CurrentTurn)
            {
                CurrentTurn--;
                isstop = true;
                BattleController.AutoWork = false;
                BW.SetWorking("No More Moves", false);
                //BattleController.CanCommand = true;
                HexCanvas.AddInfo();
                return;
            }
            if (isstop==false) DebugWindow.AddTB1("Ход " + CurrentTurn.ToString() + " ", true);
            GameMove move = Moves[CurrentTurn];
            
            NextTurn(move, true);
        }
        void NextTurn(GameMove move, bool IsVisual)
        {
            switch (move.Array[0])
            {
                case 0:
                    DebugWindow.AddTB1("Start Battle\n", false);
                    BattleField = new SortedList<byte, ShipB>();
                    //BattleFieldInfo = new List<string>();
                    //TestBattle.AddBattle(ID);
                    CurrentRound = 0;
                    Side1.StartBattle(IsVisual);
                    Side2.StartBattle(IsVisual);
                    foreach (ShipB asteroid in Asteroids.Values)
                            BattleField.Add(asteroid.CurHex, asteroid);
                    
                        if (IsVisual)
                    {
                        HexCanvas.StartBattle(this);
                    }
                    //TestBattle.AddShipsCount(ID, (byte)Side1.Ships.Count);
                    //foreach (ShipB ship in Side1.Ships.Values) TestBattle.AddShip(ID, ship); 
                    //TestBattle.AddShipsCount(ID, (byte)Side2.Ships.Count);
                    //foreach (ShipB ship in Side2.Ships.Values) TestBattle.AddShip(ID, ship); 
                    //TestBattle.AddBattleStart(ID, CurrentTurn);
                    break;
                case 1:
                    if (CurMode == BattleMode.Mode1)
                    {
                        CurrentRound++;
                        IntBoya.TurnsLabel.SetParams(CurrentRound, 0);
                        DebugWindow.AddTB1("Начало раунда " + CurrentRound.ToString() + "\n", false);
                        //TestBattle.AddRoundStart(ID, CurrentTurn, CurrentRound);
                        if (IsVisual) HexCanvas.WriteBigText("Round " + CurrentRound, Colors.Red, Links.ShootAnimSpeed);
                        Side1.RoundStart(IsVisual); Side2.RoundStart(IsVisual);
                    }
                    else if (CurMode==BattleMode.Mode2)
                    {
                        CurrentRound++;
                        IntBoya.TurnsLabel.SetParams(CurrentRound, 0);
                        DebugWindow.AddTB1("Начало раунда атаки " + CurrentRound.ToString() + "\n", false);
                        if (IsVisual) HexCanvas.WriteBigText("Attack Round " + CurrentRound, Colors.Red, Links.ShootAnimSpeed);
                        Side1.RoundStart(IsVisual);
                    }
                    break; //начало хода. Увеличить характеристики все кораблей на поле боя. Передвинуть корабли в порту
                case 101:
                    IntBoya.TurnsLabel.SetParams(CurrentRound, 1);
                    DebugWindow.AddTB1("Начало раунда защиты " + CurrentRound.ToString() + "\n", false);
                    if (IsVisual) HexCanvas.WriteBigText("Defense Round " + CurrentRound, Colors.Green, Links.ShootAnimSpeed);
                    Side2.RoundStart(IsVisual);
                    break;
                case 2:
                    if (CurMode == BattleMode.Mode1)
                    {
                        DebugWindow.AddTB1("Конец раунда " + CurrentRound.ToString() + "\n", false);
                        //TestBattle.AddRoundEnd(ID, CurrentTurn, CurrentRound);
                        Side1.RoundEnd(IsVisual); Side2.RoundEnd(IsVisual);
                    }
                    else if (CurMode==BattleMode.Mode2)
                    {
                        DebugWindow.AddTB1("Конец раунда атаки " + CurrentRound.ToString() + "\n", false);
                        Side1.RoundEnd(IsVisual);
                    }
                        break; //конец хода. На все корабли необходимо наложить отрицательные доты и проверить снижение дебафов
                case 102:
                    DebugWindow.AddTB1("Конец раунда защиты " + CurrentRound.ToString() + "\n", false);
                    Side2.RoundEnd(IsVisual);
                    if (RestrictionLength > 0) RestrictionLength--;
                    if (RestrictionLength == 0) Restriction = Restriction.None;
                    IntBoya.ShowRestrictionInfo();
                    break;
                case 3:
                    DebugWindow.AddTB1("Попадание корабля атаки в порт " + move.Array[1] + "\n", false);
                    MoveShipToPort(Side1, move.Array[1], IsVisual); break; //корабль атакующей стороны попадает в порт
                case 103:
                    DebugWindow.AddTB1("Попадание корабля защиты в порт " + move.Array[1] + "\n", false);
                    MoveShipToPort(Side2, move.Array[1], IsVisual); break; //корабль защищающейся стороны попадает в порт
                case 4:
                    DebugWindow.AddTB1(String.Format("Корабль атаки {0} вышел на поле на хекс {1}\n", move.Array[1], move.Array[2]), false);
                    MoveShipToField(Side1, move.Array[1], move.Array[2], IsVisual); return;
                case 104:
                    DebugWindow.AddTB1(String.Format("Корабль защиты {0} вышел на поле на хекс {1}\n", move.Array[1], move.Array[2]), false);
                    MoveShipToField(Side2, move.Array[1], move.Array[2], IsVisual); return;
                case 5: ShipMove(Side1, move.Array[1], move.Array[2], IsVisual); return;
                case 105: ShipMove(Side2, move.Array[1], move.Array[2], IsVisual); return;
                case 6: RotateShip(Side1, move.Array[1], move.Array[2], IsVisual); break;
                case 106: RotateShip(Side2, move.Array[1], move.Array[2], IsVisual); break;
                case 7: MakeWeaponShoot(Side1, move.Array[1], 0, move.Array[2], move.Array[3]); return;
                case 8: MakeWeaponShoot(Side1, move.Array[1], 1, move.Array[2], move.Array[3]); return;
                case 9: MakeWeaponShoot(Side1, move.Array[1], 2, move.Array[2], move.Array[3]); return;
                case 107: MakeWeaponShoot(Side2, move.Array[1], 0, move.Array[2], move.Array[3]); return;
                case 108: MakeWeaponShoot(Side2, move.Array[1], 1, move.Array[2], move.Array[3]); return;
                case 109: MakeWeaponShoot(Side2, move.Array[1], 2, move.Array[2], move.Array[3]); return;
                case 10: ShipReturned(Side1, move.Array[1], IsVisual); return;
                case 110: ShipReturned(Side2, move.Array[1], IsVisual); return;
                case 14: ShipDestroyed(move.Array[1], IsVisual); return;
                case 15: MakeSelfTargetDamage(Side1, move.Array[1], 0, move.Array[2], move.Array[3]); return;
                case 16: MakeSelfTargetDamage(Side1, move.Array[1], 1, move.Array[2], move.Array[3]); return;
                case 17: MakeSelfTargetDamage(Side1, move.Array[1], 2, move.Array[2], move.Array[3]); return;
                case 115: MakeSelfTargetDamage(Side2, move.Array[1], 0, move.Array[2], move.Array[3]); return;
                case 116: MakeSelfTargetDamage(Side2, move.Array[1], 1, move.Array[2], move.Array[3]); return;
                case 117: MakeSelfTargetDamage(Side2, move.Array[1], 2, move.Array[2], move.Array[3]); return;
                case 21: MakeAntiCurseEffect(Side1, move.Array[1], 0, move.Array[2], move.Array[3]); return;
                case 22: MakeAntiCurseEffect(Side1, move.Array[1], 1, move.Array[2], move.Array[3]); return;
                case 23: MakeAntiCurseEffect(Side1, move.Array[1], 2, move.Array[2], move.Array[3]); return;
                case 121: MakeAntiCurseEffect(Side2, move.Array[1], 0, move.Array[2], move.Array[3]); return;
                case 122: MakeAntiCurseEffect(Side2, move.Array[1], 1, move.Array[2], move.Array[3]); return;
                case 123: MakeAntiCurseEffect(Side2, move.Array[1], 2, move.Array[2], move.Array[3]); return;
                case 25: MakeAsteroidShoot(Side1, move.Array[1], 0, move.Array[2], move.Array[3]); return;
                case 26: MakeAsteroidShoot(Side1, move.Array[1], 1, move.Array[2], move.Array[3]); return;
                case 27: MakeAsteroidShoot(Side1, move.Array[1], 2, move.Array[2], move.Array[3]); return;
                case 125: MakeAsteroidShoot(Side2, move.Array[1], 0, move.Array[2], move.Array[3]); return;
                case 126: MakeAsteroidShoot(Side2, move.Array[1], 1, move.Array[2], move.Array[3]); return;
                case 127: MakeAsteroidShoot(Side2, move.Array[1], 2, move.Array[2], move.Array[3]); return;
                case 28: MakeSelfTargeAsteroidtDamage(Side1, move.Array[1], 0, move.Array[2], move.Array[3]); return;
                case 29: MakeSelfTargeAsteroidtDamage(Side1, move.Array[1], 1, move.Array[2], move.Array[3]); return;
                case 30: MakeSelfTargeAsteroidtDamage(Side1, move.Array[1], 2, move.Array[2], move.Array[3]); return;
                case 128: MakeSelfTargeAsteroidtDamage(Side2, move.Array[1], 0, move.Array[2], move.Array[3]); return;
                case 129: MakeSelfTargeAsteroidtDamage(Side2, move.Array[1], 1, move.Array[2], move.Array[3]); return;
                case 130: MakeSelfTargeAsteroidtDamage(Side2, move.Array[1], 2, move.Array[2], move.Array[3]); return;
                case 31: ResurrectShip(Side1, move.Array[1], IsVisual); break;
                case 131: ResurrectShip(Side2, move.Array[1], IsVisual); break;
                case 32: UseArtefact(Side1, 0, move.Array[1], move.Array[2]); return;
                case 33: UseArtefact(Side1, 1, move.Array[1], move.Array[2]); return;
                case 34: UseArtefact(Side1, 2, move.Array[1], move.Array[2]); break;
                case 132: UseArtefact(Side2, 0, move.Array[1], move.Array[2]); return;
                case 133: UseArtefact(Side2, 1, move.Array[1], move.Array[2]); return;
                case 134: UseArtefact(Side2, 2, move.Array[1], move.Array[2]); break;
                case 50: BattleEnd(move.Array[1]); break;// TestBattle.AddBattleEnd(ID, 0, 0, 0, 0, 0, 0); break;
           }
            if (IsVisual) BW.SetWorking("Next Turn End", false); 
            

        }
        void UseArtefact(Side side, byte pos, byte hex1, byte hex2)
        {
            side.UseArtefact(pos, hex1, hex2, true);
        }
        void ShipReturned(Side side, byte shipid, bool IsVisual)
        {
            ShipB ship = side.Ships[shipid];
            if (IsVisual)
            {
                DebugWindow.AddTB1(String.Format("Корабль {0} {1} покинул поле боя", ship.States.Side == ShipSide.Attack ? "атаки" : "защиты",
                    ship.CurHex), false);
                BattleController.ShipReturned(ship, true);
            }
            else
            {
                side.RemoveGoodEffectsFromShip(ship, IsVisual, null,"покидание");
                side.OnField.Remove(ship);
                BattleField.Remove(ship.CurHex);
                //BattleFieldInfo.Add(string.Format("Корабль сбегает с поля боя Атака={0} ID={1} Hex={2}", side.SSide, shipid, ship.CurHex));
                ship.SetHex(210);
            }
        }
        void ShipMove(Side side, byte shipid, byte hex, bool IsVisual)
        {
            ShipB ship = side.Ships[shipid];
            
            if (IsVisual)
            {
                DebugWindow.AddTB1(String.Format("Корабль {0} передвинулся с хекса {1} на хекс {2}\n", side.SSide == ShipSide.Attack ? "атаки" : "защиты",
                    side.Ships[shipid].CurHex, hex), false);
                BattleController.MoveShip(ship, HexCanvas.Hexes[hex], false);
            }
            else
            {
                side.RemoveGoodEffectsFromShipMove(ship, hex, false);
                ship.Angle = Field.DistAngleRot[ship.CurHex, hex, 1, 2];
                ship.Battle.BattleField.Remove(ship.CurHex);
                //ship.Battle.BattleFieldInfo.Add(string.Format("Корабль переместился Атака={0} ID={1} Старый хекс={2} Новый хекс={3}", side.SSide, shipid, ship.CurHex, hex));
                byte lasthex=ship.CurHex;
                ship.SetHex(hex);
                ship.RemoveEnergy(ship.Params.HexMoveCost.GetCurrent, false);
                ship.Battle.BattleField.Add(ship.CurHex, ship);
                side.RecieveGoodEffectsFromShipMove(ship, lasthex, false);
                //TestBattle.AddShipMove(ID, CurrentTurn, ship.BattleID, ship.States.Side, hex, (short)ship.Angle);
            }
            
        }
        void BattleEnd(byte result)
        {
            switch (result)
            {
                case 0: HexCanvas.WriteBigText("Red Team Won", Colors.Red, 10); break;
                case 1: HexCanvas.WriteBigText("Green Team Won", Colors.Green, 10); break;
                case 2: HexCanvas.WriteBigText("No winners", Colors.Gold, 10); break;
            }
            BattleController.AutoWork = false;
            //switch (BattleType)
            //{
            //    case BattleType.AttackEnemy: if (result == 0)
            //        {
            //            Gets.GetBattleReward(this);
            //            Links.Controller.PopUpCanvas.Place(Reward, true);
            //        } break;
            //}
        }
        public void ShipDestroyed(byte hexid, bool IsVisual)
        {
            ShipB ship = BattleField[hexid];
            Side side = ship.States.Side == ShipSide.Attack ? Side1 : Side2;
            side.RemoveGoodEffectsFromShip(ship, IsVisual, null,"уничтожение");
            side.OnField.Remove(ship);
            BattleField.Remove(hexid);
            //BattleFieldInfo.Add(string.Format("Корабль уничтожен Атака={0} ID={1} Hex={2}", ship.States.Side, ship.BattleID, hexid));
            ship.HexShip.AnimateDestroy();
            DebugWindow.AddTB1(String.Format("Корабль {0} в хексе {1} уничтожен", ship.States.Side==ShipSide.Attack ? "атаки" : "защиты", hexid), false);
            ship.SetHex(220);
            if (IsVisual) ship.SaveParams();
            ship.Params.Health.SetNull();
            ship.Params.Shield.SetNull();
            ship.Params.Energy.SetNull();
            if (ship.States.HaveShields)
                ship.HexShip.ShieldField.SwitchShield(false);
            if (ship.States.IsPortal) ship.Side.Portals.Remove(ship);
            if (IsVisual) ship.AppendChanges();
            //TestBattle.AddShipDestroyed(ID, CurrentTurn, ship.BattleID, ship.States.Side, hexid);
            ship.HexShip.Hex = null;
        }
        void MakeSelfTargeAsteroidtDamage(Side side, byte shipid, byte weapon, byte hex, byte shootresult)
        {
            ShipB ship = side.Ships[shipid];
            ShipB target = BattleField[hex];
            bool isCrit = (shootresult & 2) != 0;
            //TestBattle.AddPsiAsteroidDamage(ID, CurrentTurn, ship.BattleID, ship.States.Side, hex, weapon);
            new PsiCritFlash(ship.HexShip);
            BattleController.MakeFire(ship, target, weapon, true, isCrit, false, true, true);
            /*
            switch (weapon)
            {
                case 0: ship.PanelB.Weapon1.DisArm(); break;
                case 1: ship.PanelB.Weapon2.DisArm(); break;
                case 2: ship.PanelB.Weapon3.DisArm(); break;
            }
            */
        }
        void MakeSelfTargetDamage(Side side, byte shipid, byte weapon, byte hex, byte shootresult)
        {
            DebugWindow.AddTB1(String.Format("Выстрел корабля {0} {1} пушка {2}", side.SSide == ShipSide.Attack ? "атаки" : "защиты", shipid, weapon), false);
            ClientShootResult result = new ClientShootResult(side.Ships[shipid], weapon, side, BattleField[hex]);
            if (result.Error) throw new Exception("Ощибка при стрельбе по своим кораблям");
            ShipB ship = side.Ships[shipid];
            ShipB target = BattleField[hex];
            bool isMiss = (shootresult & 1) == 0;
            bool isCrit = (shootresult & 2) != 0;
            double RealAngle = Field.DistAngleRot[ship.CurHex, target.CurHex, weapon, 1];
            //TestBattle.AddPsiWeaponDamage(ID, CurrentTurn, ship.BattleID, ship.States.Side, target.BattleID, weapon, isCrit, (short)RealAngle, !isMiss);
            new PsiCritFlash(ship.HexShip);
            BattleController.MakeFire(ship, target, weapon, isMiss, isCrit, false, false, true);
            /*
            switch (weapon)
            {
                case 0: ship.PanelB.Weapon1.DisArm(); break;
                case 1: ship.PanelB.Weapon2.DisArm(); break;
                case 2: ship.PanelB.Weapon3.DisArm(); break;
            }
            */
        }
        void MakeAntiCurseEffect(Side side, byte shipid, byte weapon, byte hex, byte critresult)
        {
            DebugWindow.AddTB1(String.Format("Выстрел корабля {0} {1} пушка {2}", side.SSide == ShipSide.Attack ? "атаки" : "защиты", shipid, weapon), false);
            ClientShootResult result = new ClientShootResult(side.Ships[shipid], weapon, side, BattleField[hex]);
            if (result.Error) throw new Exception("Ощибка при детонации орудия");
            ShipB ship = side.Ships[shipid];
            ShipB target = BattleField[hex];
            bool isCrit = critresult == 1 ? true : false;
            double RealAngle = Field.DistAngleRot[ship.CurHex, target.CurHex, weapon, 2];
            //TestBattle.AddSelfWeaponDamage(ID, CurrentTurn, ship.BattleID, ship.States.Side, target.BattleID, weapon, isCrit, (short)RealAngle);
          BattleController.MakeFire(ship, target, weapon, false, isCrit, true,false, true);        
        }
        void MakeAsteroidShoot(Side side, byte shipid, byte weapon, byte hex, byte shootresult)
        {
            ShipB ship = side.Ships[shipid];
            ShipB target = BattleField[hex];
            bool isCrit = (shootresult & 2) != 0;
            //TestBattle.AddAsteroidDamage(ID, CurrentTurn, shipid, ship.States.Side, hex, weapon);
            BattleController.MakeFire(ship, target, weapon, false, isCrit, false, true, true);
        }
        void MakeWeaponShoot(Side side, byte shipid, byte weapon, byte hex, byte shootresult)
        {
            DebugWindow.AddTB1(String.Format("Выстрел корабля {0} {1} пушка {2}", side.SSide == ShipSide.Attack ? "атаки" : "защиты", shipid, weapon), false);
            ClientShootResult result = new ClientShootResult(side.Ships[shipid], weapon, side, BattleField[hex]);
            if (result.Error) throw new Exception("Ощибка при стрельбе");
            //DebugWindow.AddTB1("Параметры: ", true);
            ShipB ship = side.Ships[shipid];
            int sizedx = ship.States.BigSize ? 3 : 0;
            ShipB target = BattleField[hex];
            bool isMiss = (shootresult & 1) == 0;
            bool isCrit = (shootresult & 2) != 0;
            double RealAngle = Field.DistAngleRot[ship.CurHex, target.CurHex, weapon + sizedx, 1];
            //TestBattle.AddWeaponDamage(ID, CurrentTurn, shipid, ship.States.Side, target.BattleID, weapon, isCrit, (short)RealAngle, !isMiss);
            BattleController.MakeFire(ship, target, weapon, (shootresult & 1) == 0, (shootresult & 2) != 0, false,false, true);
        }
        void RotateShip(Side side, byte shipid, byte hex, bool IsVisual)
        {
            ShipB ship = side.Ships[shipid];
            int angle = Field.DistAngleRot[ship.CurHex, hex, 1, 2];
            //TestBattle.AddShipRotate(ID, CurrentTurn, shipid, ship.States.Side, hex, (short)angle);
            ship.Angle = angle;
            if (IsVisual) ship.HexShip.RotateTo(angle);
        }
        void MoveShipToField(Side side, byte shipid, byte hex, bool IsVisual)
        {
           // TestBattle.AddMoveShipToField(ID, CurrentTurn, shipid, side.SSide, hex);
           if (IsVisual)
                BattleController.EnterShipToField(side.Ships[shipid], HexCanvas.Hexes[hex]);
            else
                side.MoveShipToField(side.Ships[shipid], hex);
           
            BattleField.Add(hex, side.Ships[shipid]);
            //BattleFieldInfo.Add(string.Format("Корабль вошёл на поле боя Ataka={0} ID={1} Hex={2}", side.SSide, shipid, hex));
        }
        void MoveShipToPort(Side side, byte shipid, bool IsVisual)
        {
            ShipB ship = side.Ships[shipid];
            ship.SetAngle(ship.States.Side==ShipSide.Attack ? 120 : 300, IsVisual);
            int GatePos = side.GetFreeGateHex();
            Hex Gatehex = ship.States.Side==ShipSide.Attack ? HexCanvas.AttackGate.Hexes[GatePos] : HexCanvas.DefenseGate.Hexes[GatePos];
            side.MoveShipToPort(ship, Gatehex, GatePos, IsVisual);
            if (IsVisual)
            {
                DoubleAnimation oppani = new DoubleAnimation(1, Links.ZeroTime);
                ship.HexShip.BeginAnimation(Canvas.OpacityProperty, oppani);
                Gatehex.PlaceShip(ship.HexShip);
            }

        }
        void ResurrectShip(Side side, byte shipid, bool IsVisual)
        {
            ShipB ship = side.Ships[shipid];
            ship.Params.Health.SetMax();
            ship.Params.Shield.SetMax();
            ship.Params.Energy.SetMax();
            ship.Params.Status.Clear();
        }
    }


    public class GameMove
    {
        public byte[] Array;
        public bool IsTrue = false;
        public GameMove() //Нужно для Сервера
        {
        }
        public GameMove(byte command, byte par1, byte par2, byte par3)
        {
            IsTrue = true;
            Array = new byte[] { command, par1, par2, par3 };
        }
        public static GameMove[] GetMoves(byte[] array)
        {
            List<GameMove> list = new List<GameMove>();
            for (int i = 1; i < array.Length; i += 4)            
                list.Add(new GameMove(array[i], array[i+1], array[i+2], array[i+3]));
            return list.ToArray();
        }
    }
    public class BattleCommand
    {
        public byte[] Array;
        public BattleCommand(byte command, byte target, byte aim)
        {
            Array = new byte[] { command, target, aim };
        }
    }
    public enum RewardType {End, Money, Metall, Chips, Anti, Experience, Science, Artefact, Ship, Pilot, Land, Avanpost}
    public class Reward2
    {
        public int Money = 0;
        public int Metall = 0;
        public int Chips = 0;
        public int Anti = 0;
        public int Experience = 0;
        public ushort[] Sciences;
        public ushort[] Artefacts;
        public GSShip BonusShip;
        public GSPilot Pilot;
        public int LandID = -1;
        public int AvanpostID = -1;
        public byte[] Array;
        public Reward2()
        {

        }
        public Reward2(BuildingValues val)
        {
            throw new Exception();
        }
        public Reward2(byte[] array, ref int i)
        {
            for (;;)
            {
                RewardType type = (RewardType)array[i]; i++;
                switch (type)
                {
                    case RewardType.End: return;
                    case RewardType.Money: Money = BitConverter.ToInt32(array, i); i += 4; break;
                    case RewardType.Metall: Metall = BitConverter.ToInt32(array, i); i += 4; break;
                    case RewardType.Chips: Chips = BitConverter.ToInt32(array, i); i += 4; break;
                    case RewardType.Anti: Anti = BitConverter.ToInt32(array, i); i+=4; break;
                    case RewardType.Experience: Experience = BitConverter.ToInt32(array, i); i += 4; break;
                    case RewardType.Science: byte sciencecount = array[i]; i++;
                        Sciences = new ushort[sciencecount];
                        for (int j = 0; j < sciencecount; j++, i+=2)
                        Sciences[j] = BitConverter.ToUInt16(array, i); 
                        break;
                    case RewardType.Artefact: byte artcount = array[i]; i++;
                        Artefacts = new ushort[artcount];
                        for (int j = 0; j < artcount; j++, i += 2)
                            Artefacts[j] = BitConverter.ToUInt16(array, i);
                        break;
                    case RewardType.Ship: Schema schema = Schema.GetSchema(array, ref i);
                        int model = BitConverter.ToInt32(array, i);i += 4; GSPilot pilot = new GSPilot(array, i);
                        i += pilot.GetArray().Length;
                        BonusShip = new GSShip(0, schema, 100, model); BonusShip.Pilot = pilot;
                        break;
                    case RewardType.Pilot:
                        Pilot = new GSPilot(array, i); i += Pilot.GetArray().Length; break;
                    case RewardType.Land: LandID = BitConverter.ToInt32(array, i); i += 4; break;
                    case RewardType.Avanpost: AvanpostID = BitConverter.ToInt32(array, i); i += 4; break;
                }
            }
        }
        public void CreateArray()
        {
            List<byte> list = new List<byte>();
            if (Money > 0) { list.Add((byte)RewardType.Money); list.AddRange(BitConverter.GetBytes(Money)); }
            if (Metall > 0) { list.Add((byte)RewardType.Metall); list.AddRange(BitConverter.GetBytes(Metall)); }
            if (Chips > 0) { list.Add((byte)RewardType.Chips); list.AddRange(BitConverter.GetBytes(Chips)); }
            if (Anti > 0) { list.Add((byte)RewardType.Anti); list.AddRange(BitConverter.GetBytes(Anti)); }
            if (Experience > 0) { list.Add((byte)RewardType.Experience); list.AddRange(BitConverter.GetBytes(Experience)); }
            if (Sciences!=null && Sciences.Length!=0)
            {
                list.Add((byte)RewardType.Science); list.Add((byte)Sciences.Length);
                foreach (ushort scienceid in Sciences)
                    list.AddRange(BitConverter.GetBytes(scienceid));
            }
            if (Artefacts!=null && Artefacts.Length!=0)
            {
                list.Add((byte)RewardType.Artefact); list.Add((byte)Artefacts.Length);
                foreach (ushort artefactid in Artefacts)
                    list.AddRange(BitConverter.GetBytes(artefactid));
            }
            if (BonusShip!= null)
            {
                list.Add((byte)RewardType.Ship); list.AddRange(BonusShip.Schema.GetCrypt());
                list.AddRange(BitConverter.GetBytes(BonusShip.Model)); list.AddRange(BonusShip.Pilot.GetArray());
            }
            if (Pilot!= null)
            {
                list.Add((byte)RewardType.Pilot); list.AddRange(Pilot.GetArray());
            }
            if (LandID!=-1){list.Add((byte)RewardType.Land); list.AddRange((BitConverter.GetBytes(LandID)));}
            if (AvanpostID!=-1) { list.Add((byte)RewardType.Avanpost); list.AddRange(BitConverter.GetBytes(AvanpostID)); }
            list.Add((byte)RewardType.End);
            Array = list.ToArray();
        }
    }
    /*public class Reward
    {
        public int Money = 0;
        public int Metall = 0;
        public int Chips = 0;
        public int Anti = 0;
        public int Experience = 0;
        public ushort[] Science;
        public int BonusShip = -1;
        public byte[] Array;//Нужно для сервера
        #region НужноДляСервера
        public Reward(ServerBattle battle, ServerSide Winner, Reward MissionReward) 
        {
            //int Cargo = CalcCargo(battle, Winner);
            //if (Cargo > 0)
            //    CalcBattleReward(battle, Winner, Cargo);
            if (MissionReward != null)
            {
                Money = MissionReward.Money;
                Science = MissionReward.Science;
               // if (Cargo > 0)
               // {
               //     Metall += (int)(MissionReward.Metall / 1000.0 * Cargo);
                //    Chips += (int)(MissionReward.Chips / 1000.0 * Cargo);
               //     Anti += (int)(MissionReward.Anti / 1000.0 * Cargo);
               // }
            }
            CalcBattleExperience(battle, Winner);
            Science = new ushort[] { 0 };
            Array = GetArray();
        }
        public Reward(Reward pillage, int experience)
        {
            Metall = pillage.Metall;
            Chips = pillage.Chips;
            Anti = pillage.Anti;
            Experience = experience;
            Science = new ushort[] { 0 };
            Array = GetArray();
        }
        public void CalcBattleExperience(ServerBattle battle, ServerSide Winner)
        {
            double SumRating = (battle.Side1.Rating + battle.Side2.Rating) / 300;
            double Side1DamageRating = battle.Side1.Shoots / SumRating;
            if (Side1DamageRating > 20) Side1DamageRating = 20;
            double Side2DamageRating = battle.Side2.Shoots / SumRating;
            if (Side2DamageRating > 20) Side1DamageRating = 20;
            double Side1DeltaRating = Math.Pow(1.0 * battle.Side2.Rating / battle.Side1.Rating, 0.5);
            double Side2DeltaRating = Math.Pow(1.0 * battle.Side1.Rating / battle.Side2.Rating, 0.5);
            if (Winner == battle.Side1)
                Experience = (int)Math.Round(Side1DamageRating * Side1DeltaRating * Math.Pow(Side2DamageRating, 0.5));
            else
                Experience = (int)Math.Round(Side2DamageRating * Side2DeltaRating * Math.Pow(Side1DamageRating, 0.5));
        }
        public int CalcCargo(ServerBattle battle, ServerSide Winner)
        {
            int Cargo = 0;
            for (byte i = 1; i < Winner.Ships.Count; i++)
            {
                if (Winner.Ships[i].Hex != 220 && Winner.Cargo.ContainsKey(i))
                    Cargo += Winner.Cargo[i];
            }
            if (Cargo > 1000) return 1000;
            return Cargo;
        }
        public void CalcBattleReward(ServerBattle battle, ServerSide Winner, int Cargo)
        {
            ItemPrice DesctroyedShipsPrice = new ItemPrice();
            foreach (ServerShipB ship in battle.DestroyedShips)
                DesctroyedShipsPrice.Add(ship.Schema.Price);
            Metall = (int)(DesctroyedShipsPrice.Metall / 100.0 * 70 / 1000.0 * Cargo);
            Chips = (int)(DesctroyedShipsPrice.Chips / 100.0 * 30 / 1000.0 * Cargo);
        }
        public Reward(int money, int metal, int chips, int anti, int exp, ushort[] science)
        {
            Money = money; Metall = metal; Chips = chips; Anti = anti; Experience = exp; Science = science;
            Array = GetArray();
        }
        public Reward(int money, int metal, int chips, int anti, int exp, ushort[] science, int bonusship)
        {
            Money = money; Metall = metal; Chips = chips; Anti = anti; Experience = exp; Science = science; BonusShip = bonusship;
            Array = GetArray();
        }
        #endregion
        public Reward ()
        {

        }
        public byte[] GetArray()
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(Money));
            list.AddRange(BitConverter.GetBytes(Metall));
            list.AddRange(BitConverter.GetBytes(Chips));
            list.AddRange(BitConverter.GetBytes(Anti));
            list.AddRange(BitConverter.GetBytes(Experience));
            list.Add((byte)Science.Length);
            foreach (ushort science in Science)
                list.AddRange(BitConverter.GetBytes(science));
            list.AddRange(BitConverter.GetBytes(BonusShip));
            return list.ToArray();
        }
        public Reward(BuildingValues pillage)
        {
            Metall = pillage.Metall;
            Chips = pillage.Chips;
            Anti = pillage.Anti;
            Science = new ushort[] { 0 };
            Array = GetArray();
        }
        public Reward(int experience)
        {
            Experience = experience;
           // CreateBorder();
        }
        public Reward(byte[] array, ref int i)
        {
            Money = BitConverter.ToInt32(array, i); i += 4;
            Metall = BitConverter.ToInt32(array, i); i += 4;
            Chips = BitConverter.ToInt32(array, i); i += 4;
            Anti = BitConverter.ToInt32(array, i); i += 4;
            Experience = BitConverter.ToInt32(array, i); i += 4;
            byte scienceslength = array[i]; i++;
            Science = new ushort[scienceslength];
            if (scienceslength > 0)
                for (int j = 0; j < scienceslength; j++)
                { Science[j] = BitConverter.ToUInt16(array, i); i += 2; }
            BonusShip = BitConverter.ToInt32(array, i); i += 4;
            //CreateBorder();
        }
        public Reward(byte[] array)
        {
            int i = 0;
            Money = BitConverter.ToInt32(array, i); i += 4;
            Metall = BitConverter.ToInt32(array, i); i += 4;
            Chips = BitConverter.ToInt32(array, i); i += 4;
            Anti = BitConverter.ToInt32(array, i); i += 4;
            Experience = BitConverter.ToInt32(array, i); i += 4;
            byte scienceslength = array[i]; i++;
            Science = new ushort[scienceslength];
            if (scienceslength > 0)
                for (int j = 0; j < scienceslength; j++)
                { Science[j] = BitConverter.ToUInt16(array, i); i += 2; }
            BonusShip = BitConverter.ToInt32(array, i); i += 4;
            //CreateBorder();
        }
       
        int GetX(int pos)
        {
            return (pos % 2) * 200;
        }
        int GetY(int pos)
        {
            return 40 + (pos / 2) * 60;
        }


    }*/
}
