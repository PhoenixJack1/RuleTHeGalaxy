using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class ServerSide
    {
        public bool IsAutoControl;
        public bool IsHaveCommands = false;
        public SortedList<byte, ServerShipB> Ships;
        public byte[] Array;
        public GSPlayer Player;
        public ServerFleet Fleet;
        public byte[] Emblem;
        public byte Behavior;
        public ShipSide SSide;
        byte DefenseDx;
        public List<byte> OnBase = new List<byte>();
        public List<byte> OnPort = new List<byte>();
        public List<byte> OnGate = new List<byte>();
        public List<byte> OnField = new List<byte>();
        public List<ServerShipB> Portals = new List<ServerShipB>();
        public List<BattleCommand> CurrentCommand;
        //public List<byte> EnemyShips = new List<byte>();
        public ServerSide Enemy;
        ServerBattle CurBattle;
        public int Shoots = 0;
        public int Rating = 0;
        public bool IsReal;
        public bool IsResurrect = false;
        public SortedList<byte, SideArtefactInfo> Artefacts = new SortedList<byte, SideArtefactInfo>();
        //public ShipB ShipToDestroy = null;
        public ServerSide(SideBattleParam param, ShipSide side, ServerBattle battle)
        {
            CurBattle = battle;
            Emblem = param.Image;
            SSide = side;
            DefenseDx = (byte)(SSide == ShipSide.Attack ? 0 : 100);
            Ships = new SortedList<byte, ServerShipB>();
            IsReal = false;
            byte shipid = 0;
            foreach (ShipBattleParam sp in param.Ships)
            {
                Ships.Add(shipid, ServerShipB.GetStoryShip(shipid, side, battle, sp));
                shipid++;
            }
            //foreach (ServerShipB ship in Ships.Values)
            //    if (ship.States.IsPortal)
            //        Portal = ship;
            //foreach (Artefact art in fleet.Artefacts)
            //    Artefacts.Add(art);
            IsAutoControl = true;
        }
        public ServerSide(ServerFleet fleet, ShipSide side, ServerBattle battle)
        {
            CurBattle = battle;
            if (fleet.FleetBase != null)
                Player = fleet.FleetBase.Land.Player;
            Fleet = fleet;
            Emblem = fleet.Image;
            SSide = side;
            DefenseDx = (byte)(SSide == ShipSide.Attack ? 0 : 100);
            Ships = new SortedList<byte, ServerShipB>();
            int warphealth = 0;
            if (fleet.FleetBase != null)
            { warphealth = fleet.Target.Health; IsReal = true; }
            else
            { warphealth = fleet.Target.Health; IsReal = false; }
            Ships.Add(0, ServerShipB.GetPortalShip(0, warphealth, SSide, battle, 255, Emblem));
            //Ships.Add(0, new Portal(SSide, warphealth, battle));
            byte shipid = 0;
            foreach (ServerShip ship in fleet.Ships.Values)
            {
                shipid++;
                //if (ship.Health == 0) continue;
                Ships.Add(shipid, new ServerShipB(ship, shipid, SSide, battle));
            }
            //foreach (ServerShipB ship in Ships.Values)
            //    if (ship.States.IsPortal)
            //        Portal = ship;
            for (byte i = 0; i < fleet.Artefacts.Count; i++)
                Artefacts.Add(i, new SideArtefactInfo(fleet.Artefacts[i]));
        }
        public void CreateArray()
        {
            List<byte> list = new List<byte>();
            list.Add((byte)Ships.Count);
            foreach (ServerShipB ship in Ships.Values)
            { list.AddRange(ship.Array); Rating += ship.Rating; }
            list.Add((byte)Artefacts.Count);
            foreach (SideArtefactInfo artinfo in Artefacts.Values)
                list.AddRange(BitConverter.GetBytes(artinfo.Artefact.ID));
            Array = list.ToArray();
        }
        ServerSide(ServerBattle battle)
        {
            CurBattle = battle;
        }
        public static ServerSide GetCustomSide(ShipSide Sside, ServerBattle battle, ServerFleet fleet)
        {
            ServerSide side = new ServerSide(battle);
            side.Fleet = fleet;
            side.Emblem = fleet.Image;
            side.Behavior = 0;
            side.SSide = Sside;
            side.DefenseDx = (byte)(Sside == ShipSide.Attack ? 0 : 100);
            side.Ships = new SortedList<byte, ServerShipB>();
            side.Ships.Add(0, ServerShipB.GetPortalShip(0, 2000, Sside, battle, 255, side.Emblem));
            //int bigshipenemy = (fleet.Custom - 1) / 6; //0 - чужой, 1 - пират, 2 - прочее
            //byte level = (byte)(((fleet.Custom - 1) % 6) * 10 + 5);
            //EnemyType enemy = bigshipenemy == 0 ? EnemyType.Aliens : bigshipenemy == 1 ? EnemyType.Pirates : EnemyType.GreenParty;
            byte shipid = 1;
            switch (fleet.CustomParams.CustomID)
            {
                case 6: side.Ships.Add(1, ServerShipB.GetBigShip(1, Sside, battle, fleet.CustomParams.CustomLevel, EnemyType.Aliens, 56, 1, new byte[] { 253, 0, 0, 0 })); break;
                case 1: side.Ships.Add(1, ServerShipB.GetBigShip(1, Sside, battle, fleet.CustomParams.CustomLevel, EnemyType.Pirates, 108, 2, new byte[] { 252, 0, 0, 0 })); break;
                case 2:
                    side.Ships.Add(1, ServerShipB.GetBigShip(1, Sside, battle, fleet.CustomParams.CustomLevel, EnemyType.GreenParty, 78, 0, new byte[] { 251, 0, 0, 0 }));
                    side.Ships.Add(2, ServerShipB.GetBigShip(2, Sside, battle, fleet.CustomParams.CustomLevel, EnemyType.GreenParty, 122, 0, new byte[] { 251, 0, 0, 0 }));
                    shipid = 2; break;
                    //case 3: side.Ships.Add(1, ShipB.GetCargoShip(1, ShipSide.Attack, battle, fleet.CustomLevel, 18, 250));
                    //    side.ShipToDestroy = side.Ships[1];
                    //   break;
            }
            foreach (ServerShip ship in fleet.Ships.Values)
            {
                shipid++;
                //if (ship.Health == 0) continue;
                side.Ships.Add(shipid, new ServerShipB(ship, shipid, Sside, battle));
            }
            List<byte> list = new List<byte>();
            list.Add((byte)side.Ships.Count);
            foreach (ServerShipB ship in side.Ships.Values)
            { list.AddRange(ship.Array); side.Rating += ship.Rating; }
            side.Array = list.ToArray();
            return side;
        }

        /// <summary> метод определяет наличие свободных хекстов для входа корабля на поле </summary>
        bool CheckFreeHexForEnter(ServerShipB ship)
        {
            List<byte> array = GetJumpHexes((byte)(ship.Params.JumpDistance - 1));

            /*switch (ship.Params.JumpDistance)
            {
                case 1: array = SSide == ShipSide.Attack ? CurBattle.Field.AIHexes[0, 0] : CurBattle.Field.AIHexes[1, 0]; break;
                case 2: array = SSide == ShipSide.Attack ? CurBattle.Field.AIHexes[0, 1] : CurBattle.Field.AIHexes[1, 1]; break;
                case 3: array = SSide == ShipSide.Attack ? CurBattle.Field.AIHexes[0, 2] : CurBattle.Field.AIHexes[1, 2]; break;
            }*/
            foreach (byte b in array)
                if (!CurBattle.BattleField.ContainsKey(b)) return true;
            return false;
        }
        /// <summary> метод определяет, могут ли корабли выполнять ходы </summary>
        public bool ShipsCanMove()
        {

            foreach (byte b in OnGate)//если есть корабли в гейте и они могут прыгнуть на поле - то true
            {
                ServerShipB ship = Ships[b];
                if (CheckFreeHexForEnter(ship)) return true;
            }
            if (OnField.Count == 0) return false; //если кораблей на поле нет - то false
            if (OnField.Count == 1 && Ships[0].Params.Health.GetCurrent > 0) return false; //если корабль на поле один и это портал - то false
            foreach (byte b in OnField) //эсли корабли на поле есть но они парализованы - то false
            {
                ServerShipB ship = Ships[b];
                if (!ship.States.Controlled) continue; //если корабль портал - то смотрим следующий корабль
                if (ship.Params.Status.IsConfused > 0) continue; //если корабль парализован - то смотрим следующий корабль
                else return true;
            }
            return false;
        }
        
        /// <summary> метод пытается выполнить ход корабля в соответствии со списком ходов. Если ход выполнен, то возвразается true. </summary>
        public string MakeMove()
        {
            string reason = "Команд нет";
            for (;;)
            {
                if (CurrentCommand.Count == 0)
                    return reason;
                BattleCommand command = CurrentCommand[0];
                CurrentCommand.RemoveAt(0);    
                GameMove move;
                switch (command.Array[0])
                {
                    case 4:
                        move = TryMoveShipToField(command.Array[1], command.Array[2]);
                        if (move.IsTrue) { CurBattle.Moves.AddMove(move); return ""; }
                        else { reason = "Ошибка при выходе на поле боя"; continue; }
                    case 5:
                        move = TryMoveShip(command.Array[1], command.Array[2]);
                        if (move.IsTrue) { CurBattle.Moves.AddMove(move); return ""; }
                        else
                        { reason = "Ошибка при передвижении"; continue; }
                    case 6:
                        move = TryRotateShip(command.Array[1], command.Array[2]);
                        if (move.IsTrue) { CurBattle.Moves.AddMove(move); return ""; }
                        else { reason = "Ошибка при повороте"; continue; }
                    case 7:
                    case 8:
                    case 9:
                        shootfalsereason = "";
                        GameMove[] ShootMoves = TryShootToEnemy(command.Array[1], command.Array[2], command.Array[0] - 7);
                        if (ShootMoves == null) { reason = "Ошибка при выстреле"; continue; }
                        else
                        {
                            foreach (GameMove mov in ShootMoves)
                                CurBattle.Moves.AddMove(mov);
                            return "";
                        }
                    case 10:
                        move = TryShipReturned(command.Array[1]);
                        if (move == null)
                        {reason = "Ошибка при возврате"; continue;}
                        CurBattle.Moves.AddMove(move);
                        return "";
                    case 32:
                        if (UseArtefact(0, command.Array[1], command.Array[2]) == false)
                        { reason = "Ошибка при использовании артефакта"; continue; }
                        else return "";
                    case 33:
                        if (UseArtefact(1, command.Array[1], command.Array[2]) == false)
                        { reason = "Ошибка при использовании артефакта"; continue; }
                        else return "";
                    case 34:
                        if (UseArtefact(2, command.Array[1], command.Array[2]) == false)
                        { reason = "Ошибка при использовании артефакта"; continue; }
                        else return "";
                }
            }
        }
        public string artefactusefalsereason;
        public bool UseArtefact(byte pos, byte hex1, byte hex2)
        {
            artefactusefalsereason = "";
            if (pos>2) { artefactusefalsereason = "Неверный ID артефакта"; return false; }
            if (Artefacts.ContainsKey(pos) == false) { artefactusefalsereason = "Артефакта в данной позиции нет"; return false; }
            if (Artefacts[pos].WaitTime > 0) { artefactusefalsereason = "Артефакт разряжен"; return false; }
            switch(Artefacts[pos].Artefact.ID)
            {
                case 1002: if (CurBattle.Asteroids.ContainsKey(hex1)==false) { artefactusefalsereason = "На этом месте нет астероида"; return false; }
                if (CurBattle.BattleField.ContainsKey(hex2)) { artefactusefalsereason = "Место занято"; return false; }
                    break;
            }
            Artefact art = Artefacts[pos].Artefact;
            //Расчёт затрат энергии
            int portalsum = 0;
            foreach (ServerShipB ship in Portals)
                portalsum += ship.Params.Health.GetCurrent;
            double percenthealth = art.Battle.EnergyCost / (double)portalsum;
            if (percenthealth>1.0) { artefactusefalsereason = "Недостаточно энергии портала"; return false; }
           
            CurBattle.CurrentTurn++;
            DebugWindow.AddTB2("Ход " + CurBattle.CurrentTurn.ToString() + " ", true);
            DebugWindow.AddTB2(String.Format("Использование артефакта {0} {1} хекс1 = {2} хекс2 ={3}", SSide == ShipSide.Attack ? "атаки" : "защиты", art.GetName(), hex1, hex2), false);
            CurBattle.Moves.AddMove(new GameMove((byte)(32 + pos), hex1, hex2, 255));
            Artefacts[pos].WaitTime = Artefacts[pos].Artefact.Battle.WaitTime;
            List<ServerShipB> portallist = new List<ServerShipB>(); portallist.AddRange(Portals);
            foreach (ServerShipB ship in portallist)
            {
                int spenthealth = (int)Math.Round(ship.Params.Health.GetCurrent * percenthealth, 0);
                DamageResult damageresult = ship.RecieveDamage(0, spenthealth);
                if (damageresult.IsDestroyed)
                {
                    CurBattle.Moves.AddMove(new GameMove(14, ship.Hex, 255, 255));
                    if (SSide == ShipSide.Attack)
                        CurBattle.Side1.ShipDestroyed(ship.BattleID);
                    else if (SSide == ShipSide.Defense)
                        CurBattle.Side2.ShipDestroyed(ship.BattleID);
                }
            }
            
            List<GameMove> result = new List<GameMove>();
            List<ServerShipB> Targets;
            switch (Artefacts[pos].Artefact.ID)
            {
                case 1000:
                    ServerShipB movedship = CurBattle.BattleField[hex1];
                    Hex targethexmove = CurBattle.Field.Hexes[hex2];
                    movedship.SetHex(hex2);
                    CurBattle.BattleField.Remove(hex1);
                    CurBattle.BattleField.Add(hex2, movedship);
                    break;
                case 1001:
                    ServerShipB restoreship = CurBattle.BattleField[hex1];
                    restoreship.Params.Health.SetMax();
                    restoreship.Params.Shield.SetMax();
                    restoreship.Params.Energy.SetMax();
                    restoreship.Params.Status.Clear();
                    break;
                case 1002:
                    ServerShipB movedart = CurBattle.BattleField[hex1];
                    Hex targethex = CurBattle.Field.Hexes[hex2];
                    movedart.SetHex(hex2);
                    CurBattle.BattleField.Remove(hex1);
                    CurBattle.BattleField.Add(hex2, movedart);
                    CurBattle.Asteroids.Remove(hex1);
                    CurBattle.Asteroids.Add(hex2, movedart);
                    break;
                case 1010:
                    CurBattle.Restriction = Restriction.NoEnergy; CurBattle.RestrictionLength = 5; break;
                case 1011:
                    CurBattle.Restriction = Restriction.NoPhysic; CurBattle.RestrictionLength = 5; break;
                case 1012:
                    CurBattle.Restriction = Restriction.NoIrregular; CurBattle.RestrictionLength = 5; break;
                case 1013:
                    CurBattle.Restriction = Restriction.NoCyber; CurBattle.RestrictionLength = 5; break;
                case 1020:
                    CurBattle.Restriction = Restriction.DoubleEnergy; CurBattle.RestrictionLength = 3; break;
                case 1021:
                    CurBattle.Restriction = Restriction.DoublePhysic; CurBattle.RestrictionLength = 3; break;
                case 1022:
                    CurBattle.Restriction = Restriction.DoubleIrregular; CurBattle.RestrictionLength = 3; break;
                case 1023:
                    CurBattle.Restriction = Restriction.DoubleCyber; CurBattle.RestrictionLength = 3; break;
                case 1031:
                    Hex Hex1 = CurBattle.Field.Hexes[hex1];
                    Targets = new List<ServerShipB>();
                    foreach (Hex hex in CurBattle.Field.Hexes)
                    {
                        if (CurBattle.BattleField.ContainsKey(hex.ID) == false) continue;
                        if (CurBattle.BattleField[hex.ID].States.Indestructible == true) continue;
                        double d = Math.Sqrt(Math.Pow(hex.CenterX - Hex1.CenterX, 2) + Math.Pow(hex.CenterY - Hex1.CenterY, 2));
                        if (d < 550) Targets.Add(CurBattle.BattleField[hex.ID]);
                    }
                    break;
            }
            foreach (GameMove mov in result)
                CurBattle.Moves.AddMove(mov);
            return true;
        }
        public GameMove[] RecieveDamage(int ArtDamage, WeaponGroup group, Hex hex1, List<ServerShipB> targets)
        {
            List<GameMove> result = new List<GameMove>();
            foreach (ServerShipB ship in targets)
            {
                int angle = CurBattle.Field.DistAngleRot[hex1.ID, ship.Hex, 1, 1];
                bool destroyresult = ship.RecieveDamage(ArtDamage, group, angle);
                if (destroyresult== true)
                {
                    result.Add(new GameMove(14, ship.Hex, 255, 255));
                    if (ship.States.Side == ShipSide.Attack)
                        CurBattle.Side1.ShipDestroyed(ship.BattleID);
                    else if (ship.States.Side == ShipSide.Defense)
                        CurBattle.Side2.ShipDestroyed(ship.BattleID);
                }
            }
            return result.ToArray();
        }
        public string shootfalsereason;
        /// <summary>Метод делает выстрел корабля по цели, со всеми проверками </summary>
        public GameMove[] TryShootToEnemy(byte shipid, byte targetid, int gun)
        {
            List<GameMove> result = new List<GameMove>();
            //Проверить, что бы корабль был не портал
            if (shipid == 0) {shootfalsereason = "Корабль портал"; return null;}
            //Проверить что бы корабль был на поле
            if (!OnField.Contains(shipid)) { shootfalsereason = "Корабль не на поле"; return null; }
            ServerShipB ship = Ships[shipid];
            //Проверить что бы цель была на поле
            if (!Enemy.OnField.Contains(targetid)) { shootfalsereason = "Цель не на поле"; return null; }
            ServerShipB target = Enemy.Ships[targetid];
            if (target.States.Side == SSide) { shootfalsereason = "Стрельба по своим"; return null; }
            //Проверить отсутствие парализующих эффектов
            if (ship.Params.Status.IsConfused > 0) { shootfalsereason = "Корабль парализован"; return null; }
            //Проверить что пушка есть и заряжена
            if (ship.Weapons[gun] == null) { shootfalsereason = "Пушки нет"; return null; }
            ServerBattleWeapon weapon = ship.Weapons[gun];
            if (!weapon.IsArmed) { shootfalsereason = "Пушка разряжена"; return null; }
            //ограничение на применение вооружений
            switch (CurBattle.Restriction)
            {
                case Restriction.NoEnergy: if (weapon.Group == WeaponGroup.Energy) { shootfalsereason = "Запрет применения энергетического оружия"; return null; }break;
                case Restriction.NoPhysic: if (weapon.Group == WeaponGroup.Physic) { shootfalsereason = "Запрет применения физического оружия"; return null; } break;
                case Restriction.NoIrregular: if (weapon.Group == WeaponGroup.Irregular) { shootfalsereason = "Запрет применения аномального оружия"; return null; } break;
                case Restriction.NoCyber: if (weapon.Group == WeaponGroup.Cyber) { shootfalsereason = "Запрет применения кибернетического оружия"; return null; } break;
            }
            //Проверить наличие энергии в накопителе
            if (weapon.Consume > ship.Params.Energy.GetCurrent) { shootfalsereason = "Недостаточно энергии"; return null; }
            CurBattle.CurrentTurn++;
            DebugWindow.AddTB2("Ход " + CurBattle.CurrentTurn.ToString() + " ", true);
            DebugWindow.AddTB2(String.Format("Выстрел корабля {0} {1} пушка {2}", DefenseDx == 0 ? "атаки" : "защиты", shipid, gun), false);
            GameMove[] DestroyedShipsMoves;
            ServerShootResult shootresult = new ServerShootResult(ship, gun, this, target);
            
            int sizedx = ship.States.BigSize ? 3 : 0;
            int Rotate = ship.Battle.Field.DistAngleRot[ship.Hex, shootresult.Target.Hex, gun + sizedx, 2];
            ship.MakeFire(gun, Rotate);
            if (shootresult.EmiCrit) { RecieveEmiCritEffect(ship, weapon); DebugWindow.AddTB2("ЭмиКрит ", false); }
            if (shootresult.TimeCrit) { RecieveTimeCritEffect(ship, weapon); DebugWindow.AddTB2("ТаймКрит ", false); }
            if (!shootresult.ShootResult) DebugWindow.AddTB2("Промах", true);
            if (shootresult.AntiCrit)
            {
                DebugWindow.AddTB2(String.Format("Урон по себе корабля {0} {1} пушка {2}", ship.States.Side == ShipSide.Attack ? "атаки" : "защиты", ship.BattleID, gun), true);
                //Enemy.Shoots++;
                bool IsCrit = shootresult.AbilityResult;//ShootParams.GetCritResult(weapon, ship);
                
                //ServerTestBattle.AddSelfWeaponDamage(CurBattle.ID, CurBattle.CurrentTurn, ship.BattleID, SSide, target.BattleID,
                 //   (byte)gun, IsCrit, (short)CurBattle.Field.DistAngleRot[ship.Hex, target.Hex, gun, 2]);

                Enemy.Shoots += ship.Weapons[gun].Damage;
                result.Add(new GameMove((byte)(21 + gun + DefenseDx), shipid, target.Hex, (byte)(IsCrit ? 1 : 0)));
                DestroyedShipsMoves = RecieveDamage(ship, weapon, IsCrit, CurBattle.Field.DistAngleRot[ship.Hex, target.Hex, gun, 2]);
            }
            else if (shootresult.PsiCrit)
            {
                DebugWindow.AddTB2(String.Format("Урон по своему кораблю стороны {0} пушка {1}", ship.States.Side == ShipSide.Attack ? "атаки" : "защиты", gun), true);
                //if (shootresult.IsAsteroidAttack)
                //    ServerTestBattle.AddPsiAsteroidDamage(CurBattle.ID, CurBattle.CurrentTurn, ship.BattleID, SSide, 0, (byte)gun);
               // else
               //     ServerTestBattle.AddPsiWeaponDamage(CurBattle.ID, CurBattle.CurrentTurn, ship.BattleID, SSide,
               //     shootresult.Target.BattleID, (byte)gun, shootresult.AbilityResult, (short)shootresult.Angle, shootresult.ShootResult);
                if (shootresult.IsAsteroidAttack)
                {
                    return new GameMove[] { new GameMove((byte)(28 + gun + DefenseDx), shipid, shootresult.Target.Hex, shootresult.MaskedResult) };
                }
                if (shootresult.Accuracy >= 50)
                    Enemy.Shoots += ship.Weapons[gun].Damage;
                if (!shootresult.ShootResult)
                    return new GameMove[] { new GameMove((byte)(15 + gun + DefenseDx), shipid, shootresult.Target.Hex, (byte)(shootresult.MaskedResult)) };
                result.Add(new GameMove((byte)(15 + gun + DefenseDx), shipid, shootresult.Target.Hex, (byte)(shootresult.MaskedResult)));
                DestroyedShipsMoves = RecieveDamage(shootresult.Target, weapon, shootresult.AbilityResult, shootresult.Angle);
            }
            else
            {
                //Рассчитать точность, определить попадание, определить эффект
               // if (shootresult.IsAsteroidAttack)
               //     ServerTestBattle.AddAsteroidDamage(CurBattle.ID, CurBattle.CurrentTurn, ship.BattleID, SSide, 0, (byte)gun);
               // else
               //     ServerTestBattle.AddWeaponDamage(CurBattle.ID, CurBattle.CurrentTurn, ship.BattleID, SSide, target.BattleID, (byte)gun, shootresult.AbilityResult, (short)shootresult.Angle, shootresult.ShootResult);
                //Пересчитать параметры своего корабля
                if (shootresult.IsAsteroidAttack)
                {
                    return new GameMove[] { new GameMove((byte)(25 + gun + DefenseDx), shipid, target.Hex, shootresult.MaskedResult) };
                }
                if (shootresult.Accuracy >= 50)
                    Shoots += ship.Weapons[gun].Damage;
                //если промахнулся то вернуть ход с промахом
                if (!shootresult.ShootResult)
                    return new GameMove[] { new GameMove((byte)(7 + gun + DefenseDx), shipid, target.Hex, shootresult.MaskedResult) };
                //Выполнить выстрел
                result.Add(new GameMove((byte)(7 + gun + DefenseDx), shipid, target.Hex, (byte)(shootresult.MaskedResult)));
                //Shoots++;
                DestroyedShipsMoves = RecieveDamage(target, weapon, shootresult.AbilityResult, shootresult.Angle);
            }
            result.AddRange(DestroyedShipsMoves);
            return result.ToArray();
        }

        public GameMove[] RecieveDamage(ServerShipB target, ServerBattleWeapon weapon, bool IsCrit, int Angle)
        {
            List<GameMove> result = new List<GameMove>();
            List<ServerShipB> ShipsDestroyed = new List<ServerShipB>();

            bool shipdestroyed = target.RecieveDamage(weapon, IsCrit, Angle);
            if (shipdestroyed) ShipsDestroyed.Add(target);
            //учёт крита от солнечной пушки
            if (weapon.Type == EWeaponType.Solar && !target.States.CritProtected && IsCrit && shipdestroyed == false)
            { ShipsDestroyed.Add(target); }
            //учёт крита от Варпа
            if (weapon.Type == EWeaponType.Warp && !target.States.CritProtected && IsCrit && shipdestroyed == false)
            {
               // ServerTestBattle.AddWarpCrit(CurBattle.ID);
                if (target.States.Side == ShipSide.Attack)
                    CurBattle.Side1.ShipWarped(target.BattleID);
                else if (target.States.Side == ShipSide.Defense)
                    CurBattle.Side2.ShipWarped(target.BattleID);
            }
            //учёт крита от ракет
            if (weapon.Type == EWeaponType.Missle && !target.States.CritProtected && IsCrit)
            {
                //ServerTestBattle.AddMissleCrit(CurBattle.ID);
                foreach (Hex hex in CurBattle.Field.Hexes[target.Hex].NearHexes)
                {
                    if (!CurBattle.BattleField.ContainsKey(hex.ID)) continue;
                    ServerShipB nexttarget = CurBattle.BattleField[hex.ID];
                    if (nexttarget.States.Side != target.States.Side) continue;
                    if (nexttarget.States.CritProtected) continue;
                    bool nextargetdestroyed = nexttarget.RecieveDamage(weapon, false, CurBattle.Field.DistAngleRot[target.Hex, nexttarget.Hex, 1, 1]);
                    if (nextargetdestroyed)
                        ShipsDestroyed.Add(nexttarget);
                }
            }
            foreach (ServerShipB destroyed in ShipsDestroyed)
            {
                result.Add(new GameMove(14, destroyed.Hex, 255, 255));
                if (destroyed.States.Side == ShipSide.Attack)
                    CurBattle.Side1.ShipDestroyed(destroyed.BattleID);
                else if (destroyed.States.Side == ShipSide.Defense)
                    CurBattle.Side2.ShipDestroyed(destroyed.BattleID);
            }
            return result.ToArray();
        }
        public void RecieveTimeCritEffect(ServerShipB ship, ServerBattleWeapon weapon)
        {
            foreach (Hex hex in CurBattle.Field.Hexes[ship.Hex].NearHexes)
            {
                if (!CurBattle.BattleField.ContainsKey(hex.ID)) continue;
                ServerShipB nexttarget = CurBattle.BattleField[hex.ID];
                if (nexttarget.States.Side != ship.States.Side) continue;
                if (nexttarget.States.CritProtected) continue;
                nexttarget.Params.Health.AddParam(weapon.Damage / 2);
            }
        }
        public void RecieveEmiCritEffect(ServerShipB ship, ServerBattleWeapon weapon)
        {
            foreach (Hex hex in CurBattle.Field.Hexes[ship.Hex].NearHexes)
            {
                if (!CurBattle.BattleField.ContainsKey(hex.ID)) continue;
                ServerShipB nexttarget = CurBattle.BattleField[hex.ID];
                if (nexttarget.States.Side != ship.States.Side) continue;
                if (nexttarget.States.CritProtected) continue;
                nexttarget.Params.Energy.AddParam(weapon.Damage);
            }
        }
        public void ShipWarped(byte ShipID)
        {
            ServerShipB ship = Ships[ShipID];
            RemoveGoodEffectsFromShip(ship, null);
            OnField.Remove(ShipID);
            CurBattle.BattleField.Remove(ship.Hex);
            ship.SetHex(200);
            OnBase.Add(ShipID);

        }
        public GameMove TryShipReturned(byte shipid)
        {
            //Проверить, что бы корабль был не портал
            if (shipid == 0) return null;
            //Проверить что бы корабль был на поле
            if (!OnField.Contains(shipid)) return null;
            ServerShipB ship = Ships[shipid];
            //Проверить отсутствие парализующих эффектов
            if (ship.Params.Status.IsConfused > 0) return null;
            CurBattle.CurrentTurn++;
            DebugWindow.AddTB2("Ход " + CurBattle.CurrentTurn.ToString() + " ", true);
            DebugWindow.AddTB2(String.Format("Корабль {0} {1} покинул поле боя", ship.States.Side == ShipSide.Attack ? "атаки" : "защиты",
                    ship.Hex), false);
            //ServerTestBattle.AddShipReturned(CurBattle.ID, CurBattle.CurrentTurn, ship.BattleID, ship.States.Side);
            //Links.CurrentTurn++;
            RemoveGoodEffectsFromShip(ship, null);
            OnField.Remove(shipid);
            CurBattle.BattleField.Remove(ship.Hex);
            ship.SetHex(210);
            return new GameMove((byte)(DefenseDx + 10), shipid, 255, 255);
        }
        public void ShipDestroyed(byte ShipID)
        {
            ServerShipB ship = Ships[ShipID];
            CurBattle.CurrentTurn++;
            DebugWindow.AddTB2("Ход " + CurBattle.CurrentTurn.ToString() + " ", true);
            //Links.CurrentTurn++;
            //ServerTestBattle.AddShipDestroyed(CurBattle.ID, CurBattle.CurrentTurn, ShipID, SSide, ship.Hex);
            DebugWindow.AddTB2(String.Format("Корабль {0} в хексе {1} уничтожен", DefenseDx == 0 ? "атаки" : "защиты", ship.Hex), false);
            //TestBattle.WriteString(String.Format("{0} Корабль в хексе {1} сторона {2} ID {3} Уничтожен", Links.CurrentTurn, ship.Hex, ship.IsAttackers, ship.BattleID));
            RemoveGoodEffectsFromShip(ship, null);
            OnField.Remove(ShipID);
            CurBattle.BattleField.Remove(ship.Hex);
            ship.SetHex(220);
            ship.Params.Health.SetNull();
            ship.Params.Shield.SetNull();
            ship.Params.Energy.SetNull();
            if (ship.States.IsPortal) Portals.Remove(ship);
            if (!ship.States.CritProtected)
                CurBattle.DestroyedShips.Add(ship);

        }
        ///<summary>Метод выдаёт хексы, на которые может прыгнуть корабль</summary>
        public List<byte> GetJumpHexes(byte range)
        {
            if (range > 2) throw new Exception();
            SortedSet<Hex> result = new SortedSet<Hex>();
            foreach (ServerShipB ship in Portals)
                foreach (Hex hex in ship.JumpHexes[range])
                    result.Add(hex);
            List<byte> answer = new List<byte>();
            foreach (Hex hex in result)
                if (CurBattle.BattleField.ContainsKey(hex.ID)==false)
                    answer.Add(hex.ID);
            return answer;
        }
        public GameMove[] TryLoseBattle()
        {
            bool HasPortal = false;
            foreach (byte b in OnField)
            {
                ServerShipB ship = Ships[b];
                if (ship.States.Controlled) return null;
                if (ship.States.IsPortal) HasPortal = true;
            }
            if (HasPortal &&(OnGate.Count > 0 || OnPort.Count>0 || OnBase.Count>0)) return null;
            
            List<GameMove> list = new List<GameMove>();
            List<byte> onfield = new List<byte>();
            onfield.AddRange(OnField);
            foreach (byte b in onfield)
            {
                byte hex = Ships[b].Hex;
                ShipDestroyed(Ships[b].BattleID);
                list.Add(new GameMove(14, hex, 255, 255));
            }
            return list.ToArray();
        }
        public GameMove TryMoveShip(byte shipid, byte hex)
        {
            //проверить что бы корабль был на поле
            if (!OnField.Contains(shipid)) return new GameMove();
            if (hex > CurBattle.Field.MaxHex) return new GameMove();
            //if (CurBattle.BattleField.ContainsKey(hex)) return new GameMove();
            //проверить что бы корабль не был парализован
            ServerShipB ship = Ships[shipid];
            if (ship.CheckHex(hex) == false) return new GameMove();
            if (hex == ship.Hex) return new GameMove();
            if (ship.Params.Status.IsConfused > 0) return new GameMove();
            //проверить наличие энергии
            int distance = CurBattle.Field.DistAngleRot[ship.Hex, hex, 6, 0];
            int energycost = (int)(distance / 260.0 * ship.Params.HexMoveCost.GetCurrent);
            if (energycost > ship.Params.Energy.GetCurrent)
                return new GameMove();
            //if (ship.Params.Energy.GetCurrent < ship.Params.HexMoveCost.GetCurrent) return new GameMove();
            //выполнить действия по передвижению корабля
            //Links.CurrentTurn++;
            CurBattle.CurrentTurn++;
            DebugWindow.AddTB2("Ход " + CurBattle.CurrentTurn.ToString() + " ", true);
            DebugWindow.AddTB2(String.Format("Корабль {0} передвинулся с хекса {1} на хекс {2}\n", DefenseDx==0 ? "атаки" : "защиты",
                    ship.Hex, hex), false);
            int angle = CurBattle.Field.DistAngleRot[ship.Hex, hex, 1, 2];
            byte lasthex = ship.Hex;
            RemoveGoodEffectsFromShipMove(ship, hex);
            CurBattle.BattleField.Remove(ship.Hex);
            ship.SetHex(hex);
            CurBattle.BattleField.Add(ship.Hex, ship);
            ship.Params.Energy.RemoveParam(energycost);
            ship.Params.Status.SetMoving();
            //ship.Params.Energy.RemoveParam(ship.Params.HexMoveCost.GetCurrent);
            RecieveGoodEffectsFromShipMove(ship, lasthex);
            ship.Angle = angle;
            //ServerTestBattle.AddShipMove(CurBattle.ID, CurBattle.CurrentTurn, shipid, SSide, hex, (short)angle);
            //TestBattle.WriteString(String.Format("{0} Корабль сторона {1} ID {2} переместился в хекс {3} угол {4}",
            //    Links.CurrentTurn, IsAttackers, ship.BattleID, hex, ship.Angle));
            return new GameMove((byte)(5 + DefenseDx), shipid, hex, 255);

        }
        GameMove TryRotateShip(byte shipid, byte hex)
        {
            //проверить что бы корабль был на поле
            if (!OnField.Contains(shipid)) return new GameMove();
            if (hex > CurBattle.Field.MaxHex) return new GameMove();
            //проверить что бы корабль не был парализован
            ServerShipB ship = Ships[shipid];
            if (hex == ship.Hex) return new GameMove();
            if (ship.Params.Status.IsConfused > 0) return new GameMove();
            //выполнить действия по повороту корабля
            CurBattle.CurrentTurn++;
            //Links.CurrentTurn++;
            int angle = CurBattle.Field.DistAngleRot[ship.Hex, hex, 1, 2];
            ship.Angle = (int)angle;
            //ServerTestBattle.AddShipRotate(CurBattle.ID, CurBattle.CurrentTurn, shipid, SSide, hex, (short)angle);
            //TestBattle.WriteString(String.Format("{0} Корабль сторона {1} ID {2} повернулся к хексу {3} угол {4}",
            //    Links.CurrentTurn, IsAttackers, ship.BattleID, hex, ship.Angle));
            return new GameMove((byte)(6 + DefenseDx), shipid, hex, 255);

        }
        public GameMove TryMoveShipToField(byte shipid, byte hex)
        {
            //проверить что бы портал не был уничтожен
            if (OnField.Count == 0 || Ships[0].Params.Health.GetCurrent <= 0) return new GameMove();
            //Проверить что бы корабль был в гейте
            if (!OnGate.Contains(shipid)) return new GameMove();
            if (hex > CurBattle.Field.MaxHex) return new GameMove();
            //Проверить что бы возможности двигателя соответствовали хексу
            ServerShipB ship = Ships[shipid];
            byte needsJumpDistance = CurBattle.AI.GetJumpPower(hex, this);
            if (needsJumpDistance > ship.Params.JumpDistance) return new GameMove();
            //Проверить что бы хекс был свободен
            if (ship.CheckHex(hex) == false) return new GameMove();
            //if (CurBattle.BattleField.ContainsKey(hex)) return new GameMove();
            //Выполнить действия по размещению корабля на поле боя
            //Links.CurrentTurn++;
            CurBattle.CurrentTurn++;
            DebugWindow.AddTB2("Ход " + CurBattle.CurrentTurn.ToString() + " ", true);
            //TestBattle.WriteString(String.Format("{0} Корабль сторона {1} ID {2} вышел на поле хекс {3}", 
            //    Links.CurrentTurn, IsAttackers, ship.BattleID, hex));
            OnGate.Remove(shipid);
            OnField.Add(shipid);
            ship.SetHex(hex);
            ship.Angle = SSide == ShipSide.Attack ? 120 : 300;
            //ServerTestBattle.AddMoveShipToField(CurBattle.ID, CurBattle.CurrentTurn, shipid, SSide, hex);
            CurBattle.BattleField[hex] = ship;
            //применить усиления корабля на соседние хексы
            //применить усиления от кораблей с соседних клеток
            RecieveGoodEffectsFromShipPlace(ship, null);
            //Вернуть GameMove
            DebugWindow.AddTB2(String.Format("Корабль {0} {1} вышел на поле на хекс {2}\n", DefenseDx==0 ? "атаки" : "защиты", shipid, hex), false);
            return new GameMove((byte)(SSide == ShipSide.Attack ? 4 : 104), shipid, hex, 255);
        }
        void RemoveGoodEffectsFromShipMove(ServerShipB ship, byte newhex)
        {
            if (ship.States.CritProtected) return;
            Hex LastHex = CurBattle.Field.Hexes[ship.Hex];
            Hex NewHex = CurBattle.Field.Hexes[newhex];
            List<Hex> Array = new List<Hex>(LastHex.NearHexes);
            foreach (Hex hex in NewHex.NearHexes)
                if (Array.Contains(hex)) Array.Remove(hex);
            RemoveGoodEffectsFromShip(ship, Array.ToArray());
        }
        void RecieveGoodEffectsFromShipMove(ServerShipB ship, byte lasthex)
        {
            if (ship.States.CritProtected) return;
            Hex LastHex = CurBattle.Field.Hexes[lasthex];
            Hex NewHex = CurBattle.Field.Hexes[ship.Hex];
            List<Hex> Array = new List<Hex>(NewHex.NearHexes);
            foreach (Hex hex in LastHex.NearHexes)
                if (Array.Contains(hex)) Array.Remove(hex);
            RecieveGoodEffectsFromShipPlace(ship, Array.ToArray());
        }

        void RemoveGoodEffectsFromShip(ServerShipB ship, Hex[] array)
        {
            if (ship.States.CritProtected) return;
            Hex hex = CurBattle.Field.Hexes[ship.Hex];
            if (array == null) array = hex.NearHexes;
            foreach (Hex nearhex in array)
            {
                if (CurBattle.BattleField.ContainsKey(nearhex.ID))
                {
                    ServerShipB nearship = CurBattle.BattleField[nearhex.ID];
                    if (nearship.States.CritProtected) continue;
                    if (nearship.States.Side != SSide) continue;
                    if (ship.GroupEffects.Count != 0)
                        nearship.RemoveGoodEffectsFromShip(ship.GroupEffects);
                    if (nearship.GroupEffects.Count != 0)
                        ship.RemoveGoodEffectsFromShip(nearship.GroupEffects);
                }
            }
        }
        void RecieveGoodEffectsFromShipPlace(ServerShipB ship, Hex[] array)
        {
            Hex hex = CurBattle.Field.Hexes[ship.Hex];
            if (array == null) array = hex.NearHexes;
            foreach (Hex nearhex in array)
            {
                if (CurBattle.BattleField.ContainsKey(nearhex.ID))
                {
                    ServerShipB nearship = CurBattle.BattleField[nearhex.ID];
                    if (nearship.States.CritProtected) continue;
                    if (nearship.States.Side != SSide) continue;
                    if (ship.GroupEffects.Count != 0)
                        nearship.RecieveGoodEffectsFromShip(ship.GroupEffects);
                    if (nearship.GroupEffects.Count != 0)
                        ship.RecieveGoodEffectsFromShip(nearship.GroupEffects);
                }
            }
        }

        /// <summary>
        /// метод перемещает корабли внутри порта. 
        /// </summary>
        public void MoveShipInQueue()
        {
            byte[] ShipsInPort = OnPort.ToArray();
            foreach (byte id in ShipsInPort)
            {
                ServerShipB ship = Ships[id];
                if (ship.Hex == 201)
                {
                    OnPort.Remove(id);
                    OnGate.Add(id);
                    ship.MoveToGate();
                }
                else ship.MoveInQueue();
            }
        }
        /// <summary>
        /// метод перемещает корабль из базы 
        /// </summary>
        public byte MoveShipToPort()
        {
            if (OnBase.Count == 0 || OnPort.Count + OnGate.Count == 5) return 255; //в случае если кораблей на базе нет или число кораблей в порту более пяти, то передвижения нет
            CurBattle.CurrentTurn++;
            DebugWindow.AddTB2("Ход " + CurBattle.CurrentTurn.ToString() + " ", true);
            byte randomval = ServerLinks.GetRandomByte(CurBattle, OnBase.Count, 0);
            byte MovedShipID = OnBase[randomval];
            DebugWindow.AddTB2(String.Format("Попадание корабля {0} в порт {1}\n", DefenseDx==0?"атаки":"защиты", MovedShipID), false);
            OnBase.Remove(MovedShipID);
            OnPort.Add(MovedShipID);
            Ships[MovedShipID].MoveToPort();
            return MovedShipID;
        }
        public void StartBattle()
        {
            foreach (ServerShipB ship in Ships.Values)
            {
                if (ship.States.IsPortal) //портал размещается только на поле. Если хекс не определён - то в зависимости от стороны. Если занято - то на базе
                {
                    byte portalhex = ship.Hex;
                    if (portalhex > CurBattle.Field.MaxHex)
                        portalhex = (byte)(SSide == ShipSide.Attack ? 0 : CurBattle.Field.MaxHex);
                    if (CurBattle.BattleField.ContainsKey(portalhex)) portalhex = 220;
                    ship.SetHex(portalhex);
                    if (portalhex <= CurBattle.Field.MaxHex)
                    {
                        OnField.Add(ship.BattleID);
                        CurBattle.BattleField.Add(ship.Hex, ship);
                        Portals.Add(ship);
                    }
                }
                else if (ship.States.BigSize)
                {
                    OnField.Add(ship.BattleID);
                    CurBattle.BattleField.Add(ship.Hex, ship);
                    CurBattle.BigShips.Add(ship);
                }
                else if (ship.Hex < CurBattle.Field.MaxHex)
                {
                    OnField.Add(ship.BattleID);
                    CurBattle.BattleField.Add(ship.Hex, ship);
                }
                else if (ship.StartHealth == 0)
                {
                    ship.SetHex(220);
                }

                else
                {
                    ship.SetHex(200);
                    OnBase.Add(ship.BattleID);
                }
            }
        }

        public void RoundStart()
        {
            if (IsResurrect) //воскрешение корабля
            {
                List<ServerShipB> ResurrectionList = new List<ServerShipB>();
                foreach (ServerShipB ship in Ships.Values)
                {
                    if (ship.States.IsPortal || ship.States.BigSize) continue;
                    if (ship.Params.Health.GetCurrent == 0) ResurrectionList.Add(ship);
                }
                if (ResurrectionList.Count != 0)
                {
                    ServerShipB ship = ResurrectionList[ServerLinks.BattleRandom.Next(ResurrectionList.Count)];
                    ship.Params.Health.SetMax();
                    ship.Params.Shield.SetMax();
                    ship.Params.Energy.SetMax();
                    ship.SetHex(200);
                    OnBase.Add(ship.BattleID);
                    CurBattle.Moves.AddMove(new GameMove((byte)(31 + DefenseDx), ship.BattleID, 255, 255));
                    CurBattle.CurrentTurn++;
                }
            }
            foreach (byte shipid in OnField)
                Ships[shipid].RoundStart();
            MoveShipInQueue();
        }
        public GameMove[] RoundEnd()
        {
            List<GameMove> list = new List<GameMove>();
            for (int i = 0; i < OnField.Count; i++)
            {
                ServerShipB ship = Ships[OnField[i]];
                bool endresult = ship.RoundEnd();
                if (endresult)
                {
                    list.Add(new GameMove(14, ship.Hex, 255, 255));
                    ShipDestroyed(ship.BattleID);
                    i--;
                }
            }
            foreach (SideArtefactInfo info in Artefacts.Values)
                if (info.WaitTime > 0)
                    info.WaitTime--;
            //string teststring = (IsAttackers ? "Атака" : "Защита") + " Кораблей в порту " + OnPort.Count;
            //TestBattle.WriteString(teststring);
            return list.ToArray();
        }
        #region Mode2
        /// <summary> Метод выполняет раунд для стороны в режиме 2. Если возврат true - ход закончен, если false - ожидание хода игроком </summary>
        /// <returns></returns>
        public bool Mode2RoundStart() 
        {
            CurBattle.CurrentTurn++;
            DebugWindow.AddTB2("Ход " + CurBattle.CurrentTurn.ToString() + " ", true);
            DebugWindow.AddTB2(String.Format("Начало раунда {0} {1}\n", DefenseDx == 0 ? "атаки" : "защиты", CurBattle.CurrentRound), false);
            CurBattle.Moves.AddMove(new GameMove((byte)(1+DefenseDx), CurBattle.CurrentRound, 255, 255)); //начало раунда для стороны 1
            RoundStart();
            byte entership = MoveShipToPort();
            if (entership!=255)
                CurBattle.Moves.AddMove((byte)(3 + DefenseDx), entership, 255, 255);
            if (IsAutoControl || ShipsCanMove()==false)
            {
                //Автоматический ход
                Mode2AI.MakeMovesAuto(CurBattle, this, Enemy);
                CurBattle.CurrentTurn++;
                DebugWindow.AddTB2("Ход " + CurBattle.CurrentTurn.ToString() + " ", true);
                DebugWindow.AddTB2(String.Format("Конец раунда {0} {1}\n", DefenseDx == 0 ? "атаки" : "защиты", CurBattle.CurrentRound), false);
                CurBattle.Moves.AddMove(new GameMove((byte)(2 + DefenseDx), CurBattle.CurrentRound, 255, 255)); // конец раунда
                GameMove[] endmoves = RoundEnd();
                foreach (GameMove move in endmoves)
                    CurBattle.Moves.AddMove(move);
                return true;
            }
            else
            {
                //Ожидание ручного хода
                return false;
            }
        }
        public void Mode2RoundContinue()
        {
            //выполнение действий, переданных игроком
            //int commands = CurrentCommand.Count;
            if (IsHaveCommands)
            {
                for (; CurrentCommand.Count > 0;)
                {
                    string moveresult = MakeMove();
                    shootfalsereason += "";
                    if (moveresult != "")
                    {
                        IntBoya.Battle = IntBoya.Battle;
                        DebugWindow.Block1List = DebugWindow.Block1List;
                        DebugWindow.Block2List = DebugWindow.Block2List;
                        throw new Exception();
                    }
                }
            }
            else
                Mode2AI.MakeMovesAuto(CurBattle, this, Enemy);
            CurBattle.CurrentTurn++;
            DebugWindow.AddTB2("Ход " + CurBattle.CurrentTurn.ToString() + " ", true);
            DebugWindow.AddTB2(String.Format("Конец раунда {0} {1}\n", DefenseDx == 0 ? "атаки" : "защиты", CurBattle.CurrentRound), false);
            CurBattle.Moves.AddMove(new GameMove((byte)(2 + DefenseDx), CurBattle.CurrentRound, 255, 255)); // конец раунда
            GameMove[] endmoves = RoundEnd();
            foreach (GameMove move in endmoves)
                CurBattle.Moves.AddMove(move);
            
        }
        #endregion
    }
    class Mode2AI
    {
        public static void MakeMovesAuto(ServerBattle battle, ServerSide side1, ServerSide side2)
        {
            //Попытка ввести корабль в бой
            for (int i=0;i<side1.OnGate.Count; i++)
            {
                ServerShipB ship = side1.Ships[side1.OnGate[i]];
                byte EnterHex = ship.GetEnterToFieldSet();
                if (EnterHex == 255) continue;
                GameMove entermove = side1.TryMoveShipToField(side1.OnGate[i], EnterHex);
                if (entermove.IsTrue == true)
                {
                    battle.Moves.AddMove(entermove);
                    i--;
                }
                //ship.Params.JumpDistance
            }
            //Попытка выстрелить или передвинуться кораблями
            ServerShipB[] Ships = new ServerShipB[side1.OnField.Count];
            for (int i = 0; i < side1.OnField.Count; i++)
                Ships[i] = side1.Ships[side1.OnField[i]];
            for (int j=0;j<Ships.Length; j++)
            {
                ServerShipB ship = Ships[j];
                if (ship.States.IsPortal) continue;
                if (ship.Params.Status.IsConfused > 0) continue;
                if (ship.Hex > battle.Field.MaxHex) continue;
                //Если корабль может двигаться, то определить необходиомсть
                for (;;)
                {
                    if (ship.Params.Energy.GetCurrent < ship.Params.HexMoveCost.GetCurrent) break;
                    //Если в близи есть цель, шанс попасть в которую больше 50% - то не двигаемся. 
                    int maxacc = 0; ServerShipB target = null;
                    for (int i=0;i<ship.Weapons.Length;i++)
                    {
                        ServerBattleWeapon weapon = ship.Weapons[i];
                        if (weapon == null) continue;
                        if (ship.Params.Energy.GetCurrent < weapon.Consume) continue;
                        foreach (byte targetid in side2.OnField)
                        {
                            int curacc = ServerShootResult.GetAccuracy(ship, side2.Ships[targetid], i, ship.Battle.BattleField, false);
                            if (curacc>maxacc)
                            {
                                maxacc=curacc; target = side2.Ships[targetid];
                            }
                        }
                    }
                    //Если шанс попасть 0 - то двигаемся к порталу врага, если больше - то к цели
                    if (maxacc < 50) //если шанс попадания меньше 50 - то двигаемся
                    {
                        if (maxacc == 0) //если шанс попадания 0 - то двигаемся к порталу.
                        {
                            byte targethex = (byte)(side1.SSide == ShipSide.Attack ? battle.Field.MaxHex : 0);
                            bool moveresult = MoveToTarget(ship, battle, side1, targethex);
                            if (moveresult) continue; else break;
                        }
                        else //если шанс попадания больше 0 - то двигаетмся к цели
                        {
                            byte targethex = target.Hex;
                            bool moveresult = MoveToTarget(ship, battle, side1, targethex);
                            if (moveresult) continue; else break;
                        }
                    }
                    else //если шанс попадания больше 50 - то стреляем.
                        break;
                }
                //стрельба
                for (int i=0;i<3;i++)
                {
                    if (ship.Params.Status.IsConfused > 0) continue;
                    if (ship.Hex > battle.Field.MaxHex) continue;
                    ServerBattleWeapon weapon = ship.Weapons[i];
                    if (weapon == null) continue;
                    if (ship.Params.Energy.GetCurrent < weapon.Consume) continue;
                    switch (battle.Restriction)
                    {
                        case Restriction.NoEnergy: if (weapon.Group == WeaponGroup.Energy) continue; break;
                        case Restriction.NoPhysic: if (weapon.Group == WeaponGroup.Physic) continue; break;
                        case Restriction.NoIrregular: if (weapon.Group == WeaponGroup.Irregular) continue; break;
                        case Restriction.NoCyber: if (weapon.Group == WeaponGroup.Cyber) continue; break;
                    }
                    int maxacc = 0; ServerShipB target = null;
                    foreach (byte targetid in side2.OnField)
                    {
                        int curacc = ServerShootResult.GetAccuracy(ship, side2.Ships[targetid], i, ship.Battle.BattleField, false);
                        if (curacc > maxacc)
                        {
                            maxacc = curacc; target = side2.Ships[targetid];
                        }
                    }
                    if (maxacc == 0) continue;
                    GameMove[] moves = side1.TryShootToEnemy(ship.BattleID, target.BattleID, i);
                    string shootfalse = side1.shootfalsereason;
                    foreach (GameMove move in moves)
                        if (move.IsTrue)
                            battle.Moves.AddMove(move);
                }
               
            }

        }
        public static bool MoveToTarget(ServerShipB ship, ServerBattle battle, ServerSide side1, byte targethex)
        {
            Hex movedhex = null; double distance = Double.MaxValue;
            foreach (Hex hex in battle.Field.Hexes[ship.Hex].NearHexes)
            {
                if (ship.CheckHex(hex.ID) == false) continue;
                double curdist = battle.Field.DistAngleRot[hex.ID, targethex, 6, 0];
                if (curdist < distance)
                {
                    movedhex = hex; distance = curdist;
                }
            }
            if (movedhex != null)
            {
                GameMove move = side1.TryMoveShip(ship.BattleID, movedhex.ID);
                if (move.IsTrue == false)
                    throw new Exception();
                battle.Moves.AddMove(move);
                return true;
            }
            else
                return false;
        }
    }
  
    class ServerShootResult
    {
        public bool TimeCrit;
        public bool EmiCrit;
        public bool AntiCrit;
        public bool PsiCrit;
        public ServerShipB Target;
        public bool ShootResult { get; private set; }
        public bool AbilityResult { get; private set; }
        static byte SHOOT_MASK = 1;
        static byte ABILITY_MASK = 2;
        public byte MaskedResult { get; private set; }
        public int Angle { get; private set; }
        public int Accuracy { get; private set; }
        public bool IsAsteroidAttack;
        public ServerShootResult(ServerShipB Ship, int gun, ServerSide side, ServerShipB target)
        {
            Target = target;
            ServerBattleWeapon weapon = Ship.Weapons[gun];
            if (weapon.Type == EWeaponType.EMI)
            {
                int abilroll = ServerLinks.GetRandomByte(Ship.Battle, 100, 5); //ServerLinks.BattleRandom.Next(100);
                EmiCrit = abilroll <= ServerLinks.ShipParts.Modifiers[EWeaponType.EMI].Crit[weapon.Size];
            }
            if (weapon.Type == EWeaponType.Time)
            {
                int abiroll = ServerLinks.GetRandomByte(Ship.Battle, 100, 7); //ServerLinks.BattleRandom.Next(100);
                TimeCrit = abiroll <= ServerLinks.ShipParts.Modifiers[EWeaponType.Time].Crit[weapon.Size];
            }
            if (Ship.Params.Status.IsAntiCursed > 0)
            {
                AntiCrit = ServerLinks.GetRandomByte(Ship.Battle, 100, 8) > 49 ? true : false;
                //AntiWeaponCrit = ServerLinks.BattleRandom.Next(100) > 49 ? true : false;
            }
            if (AntiCrit == false && Ship.Params.Status.IsPsiCursed > 0)
            {
                PsiCrit = ServerLinks.GetRandomByte(Ship.Battle, 100, 9) > 49 ? true : false;
                //PsiWeaponCrit = ServerLinks.BattleRandom.Next(100) > 49 ? true : false;
                if (PsiCrit)
                {
                    if (side.OnField.Count == 1) PsiCrit = false;
                    else
                    {
                        List<byte> PsiTargets = new List<byte>(side.OnField.ToArray());
                        PsiTargets.Remove(Ship.BattleID);
                        List<byte> resultstargets = new List<byte>();
                        foreach (byte b in PsiTargets)
                        {
                            ServerShipB ship = side.Ships[b];
                            int acc = GetAccuracy(Ship, ship, gun, Ship.Battle.BattleField, false);
                            if (acc > 0) resultstargets.Add(b);
                        }
                        if (resultstargets.Count == 0) PsiCrit = false;
                        else
                        {
                            byte psitargetid = side.Ships[resultstargets[ServerLinks.GetRandomByte(Ship.Battle, resultstargets.Count, 11)]].BattleID;
                            Target = side.Ships[psitargetid];
                            //psitargetid = Ships[PsiTargets[ServerLinks.BattleRandom.Next(PsiTargets.Count)]].BattleID;
                        }
                    }
                }
            }
            Accuracy = GetAccuracy(Ship, Target, gun, Ship.Battle.BattleField, true);
            if (Accuracy==0)
                IsAsteroidAttack = true;
            CalculateAccuracy(Ship, Target, gun);
        }
       
        void CalculateAccuracy(ServerShipB MyShip, ServerShipB TargetShip, int weaponPos)
        {
            byte hex1 = MyShip.Hex;
            byte hex2 = TargetShip.Hex;
            ServerBattleWeapon weapon = MyShip.Weapons[weaponPos];
            int sizedx = MyShip.States.BigSize ? 3 : 0;
            Angle = MyShip.Battle.Field.DistAngleRot[hex1, hex2, weaponPos + sizedx, 1];
            if (Accuracy >= 100)
                ShootResult = true;
            else if (Accuracy == 0)
                ShootResult = false;
            else
            {
                int shootroll = ServerLinks.GetRandomByte(MyShip.Battle, 100, 10);// ServerLinks.BattleRandom.Next(100);
                DebugWindow.AddTB2(String.Format( "Ролл={0} ", shootroll), false);
                ShootResult = shootroll <= Accuracy;
            }

            if (ShootResult)
            {
                MaskedResult = SHOOT_MASK;
                if (weapon.Type == EWeaponType.EMI)
                {
                    AbilityResult = EmiCrit;
                }
                else if (weapon.Type == EWeaponType.Time)
                {
                    AbilityResult = TimeCrit;
                }
                else
                {
                    AbilityResult = GetCritResult(weapon, TargetShip);
                }
                if (AbilityResult)
                    MaskedResult = (byte)(MaskedResult | ABILITY_MASK);

            }
            else if (EmiCrit)
            {
                AbilityResult = EmiCrit;
                MaskedResult = ABILITY_MASK;
            }
            else if (TimeCrit)
            {
                AbilityResult = TimeCrit;
                MaskedResult = ABILITY_MASK;
            }
        }
        public static int GetAccuracy(ServerShipB myship, ServerShipB targetship, int weaponPos, SortedList<byte, ServerShipB> battlefield, bool real)
        {
            byte hex1 = myship.Hex;
            byte hex2 = targetship.Hex;
            ServerBattleWeapon weapon = myship.Weapons[weaponPos];
            int sizedx = myship.States.BigSize ? 3 : 0;
            int Distance = myship.Battle.Field.DistAngleRot[hex1, hex2, weaponPos + sizedx, 0];
            int IntersectedShips = 0;
            ServerSide side = myship.States.Side == ShipSide.Attack ? myship.Battle.Side1 : myship.Battle.Side2;
            if (myship.Battle.Field.IntersectHexes[hex1, hex2, weaponPos + sizedx] != null)
                foreach (byte b in myship.Battle.Field.IntersectHexes[hex1, hex2, weaponPos + sizedx])
                    if (battlefield.ContainsKey(b))
                    {
                        if (battlefield[b].States.Side != myship.States.Side) IntersectedShips++;
                        if (battlefield[b].States.Side == ShipSide.Neutral)
                        {
                            return 0;
                        }
                    }
            WeaponGroup group = weapon.Group;
            int Accuracy =
                weapon.Accuracy() + (myship.Battle.Restriction==Restriction.DoubleAccuracy?70:myship.Battle.Restriction==Restriction.LowAccuracy?-70:0)
                - (int)((Distance - 250) / 25) * ServerLinks.ShipParts.Modifiers[weapon.Type].Accuracy
                - IntersectedShips * 20
                - targetship.Params.Evasion.GetCurValue(group);
            Accuracy = (Accuracy / (targetship.Params.Status.IsMoving + 1));
            if (targetship.Params.Status.IsMarked > 0) Accuracy += 50;
            //if (targetship.Params.Status.IsSlowed > 0) Accuracy += 60;
            if (Distance < 350) Accuracy = 100;
            //Accuracy = Accuracy >= 20 ? Accuracy : 20;
            if (real)
            {
                DebugWindow.AddTB2(String.Format("Точность пушки={0} Дистанция={1} Движение={2}", weapon.Accuracy(), Distance, targetship.Params.Status.IsMoving), true);
                DebugWindow.AddTB2(String.Format("Модификатор={0} Помехи={1} Уклонение={2}",
                    ServerLinks.ShipParts.Modifiers[weapon.Type].Accuracy, IntersectedShips, targetship.Params.Evasion.GetCurValue(group)), true);
                DebugWindow.AddTB2(String.Format("Точность={0} ", Accuracy), true);
            }
            return Accuracy;
        }
        public static bool GetCritResult(ServerBattleWeapon weapon, ServerShipB Target)
        {
            int abilroll = ServerLinks.GetRandomByte(Target.Battle, 100, 15);// ServerLinks.BattleRandom.Next(100);
            int immuneroll = ServerLinks.GetRandomByte(Target.Battle, 100, 8);
            DebugWindow.AddTB2(String.Format("Крит шанс={0} Крит ролл={1} Иммунитет={2} Иммун ролл={3}", ServerLinks.ShipParts.Modifiers[weapon.Type].Crit[weapon.Size], abilroll, Target.Params.Immune.GetCurValue(weapon.Group), immuneroll), true);
            bool AbilityResult = abilroll <= ServerLinks.ShipParts.Modifiers[weapon.Type].Crit[weapon.Size];
            bool ImmuneResult = immuneroll <= Target.Params.Immune.GetCurValue(weapon.Group);
            if (AbilityResult == true)
            {
                if (ServerLinks.ShipParts.Modifiers[weapon.Type].ImmunePierce == true)
                    return true;
                else
                    return !ImmuneResult;
            }
            else return false;
        }
    }
}
