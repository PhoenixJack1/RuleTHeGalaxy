using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    /// <summary> Класс, генерирующий боевые ходы </summary>
    public class AI
    {
        ServerBattle CurBattle;
        public AI(ServerBattle battle)
        {
            CurBattle = battle;
        }
        /// <summary> Метод показывает, какую силу надо иметь кораблю, что бы допрыгнуть до хекса </summary>
        public byte GetJumpPower(byte hex, ServerSide side)
        {
            if (side.GetJumpHexes(0).Contains(hex)) return 1;
            else if (side.GetJumpHexes(1).Contains(hex)) return 2;
            else if (side.GetJumpHexes(2).Contains(hex)) return 3;
            else return 4;
            /*
            if (side == ShipSide.Attack)
            {
                if (!CurBattle.Field.AIHexes[0, 2].Contains(hex)) return 4;
                if (!CurBattle.Field.AIHexes[0, 1].Contains(hex)) return 3;
                if (CurBattle.Field.AIHexes[0, 0].Contains(hex)) return 1; else return 2;
            }
            else if (side == ShipSide.Defense)
            {
                if (!CurBattle.Field.AIHexes[1, 2].Contains(hex)) return 4;
                if (!CurBattle.Field.AIHexes[1, 1].Contains(hex)) return 3;
                if (CurBattle.Field.AIHexes[1, 0].Contains(hex)) return 1; else return 2;
            }
            return 0;
            */
        }
        public void CreateMoveList(ServerSide side1, ServerSide side2, SortedList<byte, ServerShipB> Asteroids)
        {
            List<BattleCommand> Commands = new List<BattleCommand>();
            SortedSet<byte> BisyHexes = new SortedSet<byte>();
            SortedSet<byte> EnemyHexes = new SortedSet<byte>();
            SortedList<byte, byte> BigShipHexes = new SortedList<byte, byte>();
            List<FShip> Friends = new List<FShip>();
            foreach (byte b in side1.OnGate)
                Friends.Add(side1.Ships[b].GetFriendShip());
            foreach (byte b in side1.OnField)
                Friends.Add(side1.Ships[b].GetFriendShip());
            List<EShip> Enemy = new List<EShip>();
            foreach (byte b in side2.OnField)
                Enemy.Add(side2.Ships[b].GetEnemyShip());
            foreach (FShip ship in Friends)
            {
                if (ship.Hex < 200)
                    BisyHexes.Add(ship.Hex);
                if (ship.IsBig)
                {
                    foreach (Hex hex in CurBattle.Field.Hexes[ship.Hex].NearHexes)
                        BigShipHexes.Add(hex.ID, ship.ID);
                }
            }
            foreach (EShip ship in Enemy)
            {
                BisyHexes.Add(ship.Hex);
                if (ship.IsBig)
                    foreach (Hex hex in CurBattle.Field.Hexes[ship.Hex].NearHexes)
                        BisyHexes.Add(hex.ID);
                EnemyHexes.Add(ship.Hex);
            }
            foreach (ServerShipB astro in Asteroids.Values)
                BisyHexes.Add(astro.Hex);
            //Попытка выставить все корабли на поле
            foreach (FShip ship in Friends)
            {
                if (ship.Hex > 200)
                {

                    byte hex = GetHexForPlaceShipToField(BisyHexes, ship.Hex, side1, BigShipHexes);
                    if (hex != 255)
                    {
                        ship.Hex = hex;
                        BisyHexes.Add(hex);
                        Commands.Add(new BattleCommand(4, ship.ID, hex));
                    }
                }
            }
            //Попытка произвести выстрел или передвижение всеми кораблями
            foreach (FShip ship in Friends)
            {
                if (ship.ID == 0) continue;
                if (ship.Hex < 200)
                {

                    for (int i = 0; i < 3; i++)
                    {
                        if (ship.Weapons[i] == null) continue;
                        //пропускаем недоступные пушки
                        switch (CurBattle.Restriction)
                        {
                            case Restriction.NoEnergy: if (ship.Weapons[i].Group == WeaponGroup.Energy) continue; break;
                            case Restriction.NoPhysic: if (ship.Weapons[i].Group == WeaponGroup.Physic) continue; break;
                            case Restriction.NoIrregular: if (ship.Weapons[i].Group == WeaponGroup.Irregular) continue; break;
                            case Restriction.NoCyber: if (ship.Weapons[i].Group == WeaponGroup.Cyber) continue; break;
                        }
                        bool CanShipMove = ship.ShipMoved ? false : ship.Energy >= ship.ShipMoveCost;
                        SelectTargetResult2 result = SelectTarget(ship, i, Enemy, EnemyHexes, BisyHexes, CanShipMove, Asteroids, BigShipHexes);
                        if (result.HasResult)
                        {
                            if (result.IsMove == false)
                            {
                                Commands.Add(new BattleCommand((byte)(7 + i), ship.ID, result.Target));
                            }
                            else
                            {
                                Commands.Add(new BattleCommand(5, ship.ID, result.Target));
                                i -= 1;
                                BisyHexes.Remove(ship.Hex);
                                ship.Hex = result.Target;
                                BisyHexes.Add(ship.Hex);
                                ship.ShipMoved = true;
                                //Обновление занятых больших хексов
                                if (ship.IsBig)
                                {
                                    SortedList<byte, byte> newBigShipHexes = new SortedList<byte, byte>();
                                    foreach (KeyValuePair<byte, byte> pair in BigShipHexes)
                                        if (pair.Value != ship.ID)
                                            newBigShipHexes.Add(pair.Key, pair.Value);
                                    foreach (Hex hex in CurBattle.Field.Hexes[ship.Hex].NearHexes)
                                        newBigShipHexes.Add(hex.ID, ship.ID);
                                    BigShipHexes = newBigShipHexes;
                                }
                            }
                        }

                    }
                }
            }
            side1.CurrentCommand = Commands;
        }
        class SelectTargetResult2
        {
            //Метод должен выбирать хекс в качестве цели.
            //Если есть цель по которой можно стрельнуть с шансом более 50% (с учётом нулевого шанса для закрытых целей) - то стреляет.
            //Если цели нет и корабль не двигался - выбирается хекс для движения.
            //Хекс выбирается ближний к самой близкой цели без учёта блока
            public bool HasResult = false;
            public bool IsMove = false;
            public byte Target = 255;
            public SelectTargetResult2()
            { }
            public SelectTargetResult2(bool isMove, byte target)
            {
                HasResult = true;
                IsMove = isMove;
                Target = target;
            }
        }
        SelectTargetResult2 SelectTarget(FShip friend, int gun, List<EShip> enemies, SortedSet<byte> EnemyHexes,
            SortedSet<byte> BisyHexes, bool CanMove, SortedList<byte, ServerShipB> asteroids, SortedList<byte, byte> SelfBigShipHexes)
        {
            byte targethex = 255;
            byte target = 255;
            int accuracy = 0;
            foreach (EShip enemy in enemies)
            {
                int acc = CalculateFireAccuracy(friend, enemy, gun, EnemyHexes, asteroids);
                if (acc > accuracy)
                {
                    accuracy = acc;
                    target = enemy.ID;
                    targethex = enemy.Hex;
                }
            }
            if (accuracy >= 50)
                return new SelectTargetResult2(false, target);
            if (!CanMove)
                return new SelectTargetResult2();
            targethex = 255;
            accuracy = 0;
            foreach (EShip enemy in enemies)
            {
                int acc = CalculateMoveAccuracy(friend, enemy, gun, EnemyHexes);
                if (acc > accuracy)
                {
                    accuracy = acc;
                    targethex = enemy.Hex;
                }
            }
            if (targethex == 255)
                return new SelectTargetResult2();
            target = SelectHexForMove(friend.ID, friend.Hex, targethex, BisyHexes, SelfBigShipHexes, friend.IsBig);
            if (target == 255)
                return new SelectTargetResult2();
            else return new SelectTargetResult2(true, target);
        }
        public int CalculateMoveAccuracy(FShip friend, EShip enemy, int gun, SortedSet<byte> hexes)
        {
            int eships = 0;
            foreach (byte b in CurBattle.Field.IntersectHexes[friend.Hex, enemy.Hex, gun])
                if (hexes.Contains(b))
                    eships++;
            int result = friend.Weapons[gun].Accuracy  + (CurBattle.Restriction==Restriction.DoubleAccuracy? 70 : CurBattle.Restriction==Restriction.LowAccuracy?-70:0)
                - (int)((CurBattle.Field.DistAngleRot[friend.Hex, enemy.Hex, gun, 0] - 250) / 25) *
                ServerLinks.ShipParts.Modifiers[friend.Weapons[gun].Type].Accuracy -
                eships * 20 - enemy.Evasion[friend.Weapons[gun].Group];
            return result < 20 ? 20 : result;
        }
        public int CalculateFireAccuracy(FShip friend, EShip enemy, int gun, SortedSet<byte> hexes, SortedList<byte, ServerShipB> asteroids)
        {
            foreach (byte b in CurBattle.Field.IntersectHexes[friend.Hex, enemy.Hex, gun])
                if (asteroids.ContainsKey(b))
                    return -1;
            int eships = 0;
            foreach (byte b in CurBattle.Field.IntersectHexes[friend.Hex, enemy.Hex, gun])
                if (hexes.Contains(b))
                    eships++;
            int result = friend.Weapons[gun].Accuracy + (CurBattle.Restriction == Restriction.DoubleAccuracy ? 70 : CurBattle.Restriction == Restriction.LowAccuracy ? -70 : 0)
                - (int)((CurBattle.Field.DistAngleRot[friend.Hex, enemy.Hex, gun, 0] - 250) / 25) *
                ServerLinks.ShipParts.Modifiers[friend.Weapons[gun].Type].Accuracy
                - eships * 20 - enemy.Evasion[friend.Weapons[gun].Group];
            return result < 20 ? 20 : result;
        }

        class SelectTargetResult
        {
            public bool IsTargetShip;
            public byte Target;
            public SelectTargetResult(bool isTargetShip, byte target)
            {
                IsTargetShip = isTargetShip; Target = target;
            }
        }
        /// <summary> метод выбирает цель для стрельбы из врагов. выбирается цель с самым высоким шансом попадания. Если целей нет - возвращается 255, если нет цели с шансом 
        /// попадания больше 50 - то пытается выбрать хекс на который передвинутся ближе к цели. Если такого нет, то возвращает цель, если есть - то возвращает номер хекса</summary>


        SelectTargetResult SelectTargetForShoot(FShip friend, int gun, List<EShip> enemies, SortedSet<byte> EnemyHexes, SortedSet<byte> BisyHexes,
            bool CanMove, SortedList<byte, ServerShipB> asteroids, SortedList<byte, byte> SelfBigShipHexes)
        {
            byte target = 255;
            byte targethex = 255;
            int accuracy = 0;
            foreach (EShip enemy in enemies)
            {
                int acc = CalculateFinalAccuracy(friend, enemy, gun, EnemyHexes, asteroids);
                if (acc > accuracy)
                {
                    accuracy = acc;
                    target = enemy.ID;
                    targethex = enemy.Hex;
                }
            }
            if (target == 255)
                return new SelectTargetResult(true, 255);
            else if (!CanMove)
                return new SelectTargetResult(true, target);
            else if (accuracy >= 50)
                return new SelectTargetResult(true, target);
            else
            {
                byte movehex = SelectHexForMove(friend.ID, friend.Hex, targethex, BisyHexes, SelfBigShipHexes, friend.IsBig);
                if (movehex == 255)
                    return new SelectTargetResult(true, target);
                else
                    return new SelectTargetResult(false, movehex);
            }
        }
        /// <summary> метод выбирает свободный хекс из соседних хексов ближе к цели но не дальше чем текущий хекс. Если таковых нет - возвращает 255  </summary>
        public byte SelectHexForMove(byte shipid, byte shiphex, byte targethex, SortedSet<byte> hexes, SortedList<byte, byte> SelfBigShipHexes, bool IsBigShip)
        {
            byte target = 255;
            int distance = CurBattle.Field.DistAngleRot[shiphex, targethex, 1, 0];
            Hex[] NearHexes = CurBattle.Field.Hexes[shiphex].NearHexes;
            foreach (Hex hex in NearHexes)
                if (CurBattle.Field.DistAngleRot[hex.ID, targethex, 1, 0] < distance && !hexes.Contains(hex.ID))
                {
                    if (SelfBigShipHexes.ContainsKey(hex.ID) && SelfBigShipHexes[hex.ID] != shipid) continue;
                    if (IsBigShip)
                    {
                        bool iserror = false;
                        foreach (Hex nearhex in hex.NearHexes)
                        {
                            if (nearhex.ID == shiphex) continue;
                            if (hexes.Contains(nearhex.ID)) { iserror = true; break; }
                            if (SelfBigShipHexes.ContainsKey(nearhex.ID) && SelfBigShipHexes[nearhex.ID] != shipid) { iserror = true; break; }

                        }
                        if (iserror) continue;
                    }
                    distance = CurBattle.Field.DistAngleRot[hex.ID, targethex, 1, 0];
                    target = hex.ID;
                }

            return target;
        }
        public int CalculateFinalAccuracy(FShip friend, EShip enemy, int gun, SortedSet<byte> hexes, SortedList<byte, ServerShipB> asteroids)
        {
            int gunpos = gun + (friend.IsBig ? 3 : 0);
            foreach (byte b in CurBattle.Field.IntersectHexes[friend.Hex, enemy.Hex, gunpos])
                if (asteroids.ContainsKey(b))
                    return -1;
            int eships = 0;
            foreach (byte b in CurBattle.Field.IntersectHexes[friend.Hex, enemy.Hex, gunpos])
                if (hexes.Contains(b))
                    eships++;
            int result = friend.Weapons[gun].Accuracy - (int)((CurBattle.Field.DistAngleRot[friend.Hex, enemy.Hex, gunpos, 0] - 250) / 25) * ServerLinks.ShipParts.Modifiers[friend.Weapons[gun].Type].Accuracy - eships * 20 - enemy.Evasion[friend.Weapons[gun].Group];
            return result < 20 ? 20 : result;
        }
        public byte GetHexForPlaceShipToField(SortedSet<byte> hexes, byte Hex, ServerSide side, SortedList<byte, byte> SelfBigShipHexes)
        {
            List<byte> targethexes = side.GetJumpHexes((byte)(Hex - 211));
            /*
            if (side == ShipSide.Attack)
            {
                if (Hex == 211)
                    targethexes = CurBattle.Field.AIHexes[0, 0];
                else if (Hex == 212)
                    targethexes = CurBattle.Field.AIHexes[0, 1];
                else
                    targethexes = CurBattle.Field.AIHexes[0, 2];
            }
            else if (side == ShipSide.Defense)
            {
                if (Hex == 211)
                    targethexes = CurBattle.Field.AIHexes[1, 0];
                else if (Hex == 212)
                    targethexes = CurBattle.Field.AIHexes[1, 1];
                else
                    targethexes = CurBattle.Field.AIHexes[1, 2];
            }*/
            byte result = 255;
            foreach (byte b in targethexes)
            {
                if (hexes.Contains(b))
                    continue;
                if (SelfBigShipHexes.ContainsKey(b))
                    continue;
                else
                {
                    result = b;
                    break;
                }
            }
            return result;
        }
    }
}
