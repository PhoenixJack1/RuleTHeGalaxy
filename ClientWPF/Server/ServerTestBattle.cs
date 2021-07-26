using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    /*
    class ServerTestBattle
    {
        public static SortedList<long, List<byte>> Logs = new SortedList<long, List<byte>>();
        public static void AddBattle(long id)
        {
            if (Logs.ContainsKey(id))
            {
                Logs.Remove(id);
            }
            Logs.Add(id, new List<byte>());
            Logs[id].AddRange(BitConverter.GetBytes(DateTime.Now.Ticks));
        }
        public static void AddShipsCount(long id, byte count)
        {
            Logs[id].Add(count);
        }
        public static void AddShip(long id, ServerShipB ship)
        {
            Logs[id].AddRange(ship.GetLogInfo());
        }
        public static void AddBattleStart(long id, short currturn)
        {
            Logs[id].Add((byte)LogMessage.BattleStart);
            Logs[id].AddRange(BitConverter.GetBytes(currturn));
        }
        public static void AddRoundStart(long id, short currturn, byte round)
        {
            Logs[id].Add((byte)LogMessage.RoundStart);
            Logs[id].AddRange(BitConverter.GetBytes(currturn));
            Logs[id].Add(round);
        }
        public static void AddRoundEnd(long id, short currturn, byte round)
        {
            Logs[id].Add((byte)LogMessage.RoundEnd);
            Logs[id].AddRange(BitConverter.GetBytes(currturn));
            Logs[id].Add(round);
        }
        public static void AddRecieveDamage(long id, short currturn, ShipSide side, byte shipid, byte weapontype, short angle)
        {
            Logs[id].Add((byte)LogMessage.RecieveDamage);
            Logs[id].AddRange(BitConverter.GetBytes(currturn));
            Logs[id].Add((byte)side);
            Logs[id].Add(shipid);
            Logs[id].Add(weapontype);
            Logs[id].AddRange(BitConverter.GetBytes(angle));
        }
        #region WeaponCrit
        public static void AddLaserCrit(long id)
        {
            Logs[id].Add((byte)LogMessage.LaserCrit);
        }
        public static void AddEmiCrit(long id)
        {
            Logs[id].Add((byte)LogMessage.EMICrit);
        }
        public static void AddPlasmaCrit(long id)
        {
            Logs[id].Add((byte)LogMessage.PlasmaCrit);
        }
        public static void AddSolarCrit(long id)
        {
            Logs[id].Add((byte)LogMessage.SolarCrit);
        }
        public static void AddCannonCrit(long id)
        {
            Logs[id].Add((byte)LogMessage.CannonCrit);
        }
        public static void AddGaussCrit(long id)
        {
            Logs[id].Add((byte)LogMessage.GaussCrit);
        }
        public static void AddMissleCrit(long id)
        {
            Logs[id].Add((byte)LogMessage.MissleCrit);
        }
        public static void AddAntiCrit(long id)
        {
            Logs[id].Add((byte)LogMessage.AntiCrit);
        }
        public static void AddPsiCrit(long id)
        {
            Logs[id].Add((byte)LogMessage.PsiCrit);
        }
        public static void AddDarkCrit(long id)
        {
            Logs[id].Add((byte)LogMessage.DarkCrit);
        }
        public static void AddWarpCrit(long id)
        {
            Logs[id].Add((byte)LogMessage.WarpCrit);
        }
        public static void AddTimeCrit(long id)
        {
            Logs[id].Add((byte)LogMessage.TimeCrit);
        }
        public static void AddSliceCrit(long id)
        {
            Logs[id].Add((byte)LogMessage.SliceCrit);
        }
        public static void AddRadCrit(long id)
        {
            Logs[id].Add((byte)LogMessage.RadCrit);
        }
        public static void AddDroneCrit(long id)
        {
            Logs[id].Add((byte)LogMessage.DroneCrit);
        }
        public static void AddMagnetCrit(long id)
        {
            Logs[id].Add((byte)LogMessage.MagnetCrit);
        }
        #endregion
        public static void AddShieldBlockDamage(long id, short shielddamage, short shieldleft, short shieldmax)
        {
            Logs[id].Add((byte)LogMessage.ShieldBlockDamage);
            Logs[id].AddRange(BitConverter.GetBytes(shielddamage));
            Logs[id].AddRange(BitConverter.GetBytes(shieldleft));
            Logs[id].AddRange(BitConverter.GetBytes(shieldmax));
        }
        public static void AddShieldPierce(long id)
        {
            Logs[id].Add((byte)LogMessage.ShieldPierce);
        }
        public static void AddHealthDamageLive(long id, short shielddamage, short healthdamage, short healthleft, short healtmax)
        {
            Logs[id].Add((byte)LogMessage.HealthDamageLive);
            Logs[id].AddRange(BitConverter.GetBytes(shielddamage));
            Logs[id].AddRange(BitConverter.GetBytes(healthdamage));
            Logs[id].AddRange(BitConverter.GetBytes(healthleft));
            Logs[id].AddRange(BitConverter.GetBytes(healtmax));
        }
        public static void AddHealthDamageDestroy(long id, short shielddamage, short healthdamage)
        {
            Logs[id].Add((byte)LogMessage.HealthDamageDestroy);
            Logs[id].AddRange(BitConverter.GetBytes(shielddamage));
            Logs[id].AddRange(BitConverter.GetBytes(healthdamage));
        }
        public static void AddMoveShipToGate(long id, short currturn, byte shipid, ShipSide side, byte jumpdistance)
        {
            Logs[id].Add((byte)LogMessage.MoveToGate);
            Logs[id].AddRange(BitConverter.GetBytes(currturn));
            Logs[id].Add(shipid);
            Logs[id].Add((byte)side);
            Logs[id].Add(jumpdistance);
        }
        public static void AddMoveShipToPort(long id, short currturn, byte shipid, ShipSide side, byte waittime)
        {
            Logs[id].Add((byte)LogMessage.MoveToPort);
            Logs[id].AddRange(BitConverter.GetBytes(currturn));
            Logs[id].Add(shipid);
            Logs[id].Add((byte)side);
            Logs[id].Add(waittime);
        }
        public static void AddSelfWeaponDamage(long id, short currturn, byte shipid, ShipSide side, byte targetid, byte gun, bool iscrit, short angle)
        {
            Logs[id].Add((byte)LogMessage.SelfWeaponDamage);
            Logs[id].AddRange(BitConverter.GetBytes(currturn));
            Logs[id].Add(shipid);
            Logs[id].Add((byte)side);
            Logs[id].Add(targetid);
            Logs[id].Add(gun);
            Logs[id].AddRange(BitConverter.GetBytes(iscrit));
            Logs[id].AddRange(BitConverter.GetBytes(angle));

        }
        public static void AddPsiAsteroidDamage(long id, short currturn, byte shipid, ShipSide side, byte hex, byte gun)
        {
            Logs[id].Add((byte)LogMessage.PsiAsteroidDamage);
            Logs[id].AddRange(BitConverter.GetBytes(currturn));
            Logs[id].Add(shipid);
            Logs[id].Add((byte)side);
            Logs[id].Add(hex);
            Logs[id].Add(gun);
        }
        public static void AddPsiWeaponDamage(long id, short currturn, byte shipid, ShipSide side, byte targetid, byte gun, bool iscrit, short angle, bool aim)
        {
            Logs[id].Add((byte)LogMessage.PsiWeaponDamage);
            Logs[id].AddRange(BitConverter.GetBytes(currturn));
            Logs[id].Add(shipid);
            Logs[id].Add((byte)side);
            Logs[id].Add(targetid);
            Logs[id].Add(gun);
            Logs[id].AddRange(BitConverter.GetBytes(iscrit));
            Logs[id].AddRange(BitConverter.GetBytes(angle));
            Logs[id].AddRange(BitConverter.GetBytes(aim));
        }
        public static void AddAsteroidDamage(long id, short currturn, byte shipid, ShipSide side, byte asteroidhex, byte gun)
        {
            Logs[id].Add((byte)LogMessage.AsteroidDamage);
            Logs[id].AddRange(BitConverter.GetBytes(currturn));
            Logs[id].Add(shipid);
            Logs[id].Add((byte)side);
            Logs[id].Add(asteroidhex);
            Logs[id].Add(gun);
        }
        public static void AddWeaponDamage(long id, short currturn, byte shipid, ShipSide side, byte targetid, byte gun, bool iscrit, short angle, bool aim)
        {
            Logs[id].Add((byte)LogMessage.WeaponDamage);
            Logs[id].AddRange(BitConverter.GetBytes(currturn));
            Logs[id].Add(shipid);
            Logs[id].Add((byte)side);
            Logs[id].Add(targetid);
            Logs[id].Add(gun);
            Logs[id].AddRange(BitConverter.GetBytes(iscrit));
            Logs[id].AddRange(BitConverter.GetBytes(angle));
            Logs[id].AddRange(BitConverter.GetBytes(aim));
        }
        public static void AddShipReturned(long id, short currturn, byte shipid, ShipSide side)
        {
            Logs[id].Add((byte)LogMessage.ShipReturned);
            Logs[id].AddRange(BitConverter.GetBytes(currturn));
            Logs[id].Add(shipid);
            Logs[id].Add((byte)side);
        }
        public static void AddShipDestroyed(long id, short currturn, byte shipid, ShipSide side, byte hex)
        {
            Logs[id].Add((byte)LogMessage.ShipDestroyed);
            Logs[id].AddRange(BitConverter.GetBytes(currturn));
            Logs[id].Add(shipid);
            Logs[id].Add((byte)side);
            Logs[id].Add(hex);
        }
        public static void AddShipMove(long id, short currturn, byte shipid, ShipSide side, byte hex, short angle)
        {
            Logs[id].Add((byte)LogMessage.ShipMove);
            Logs[id].AddRange(BitConverter.GetBytes(currturn));
            Logs[id].Add(shipid);
            Logs[id].Add((byte)side);
            Logs[id].Add(hex);
            Logs[id].AddRange(BitConverter.GetBytes(angle));
        }
        public static void AddShipRotate(long id, short currturn, byte shipid, ShipSide side, byte hex, short angle)
        {
            Logs[id].Add((byte)LogMessage.RotateShip);
            Logs[id].AddRange(BitConverter.GetBytes(currturn));
            Logs[id].Add(shipid);
            Logs[id].Add((byte)side);
            Logs[id].Add(hex);
            Logs[id].AddRange(BitConverter.GetBytes(angle));
        }
        public static void AddMoveShipToField(long id, short currturn, byte shipid, ShipSide side, byte hex)
        {
            Logs[id].Add((byte)LogMessage.MoveToField);
            Logs[id].AddRange(BitConverter.GetBytes(currturn));
            Logs[id].Add(shipid);
            Logs[id].Add((byte)side);
            Logs[id].Add(hex);
        }
        public static void AddEnergySpent(long id, byte shipid, ShipSide side, byte weapontype, short energy)
        {
            Logs[id].Add((byte)LogMessage.EnergySpent);
            Logs[id].Add(shipid);
            Logs[id].Add((byte)side);
            Logs[id].Add(weapontype);
            Logs[id].AddRange(BitConverter.GetBytes(energy));
        }
        public static void AddBattleEnd(long id, short shoot1, short shoot2, int rating1, int rating2, double K1, double K2)
        {
            Logs[id].Add((byte)LogMessage.BattleEnd);
            Logs[id].AddRange(BitConverter.GetBytes(shoot1));
            Logs[id].AddRange(BitConverter.GetBytes(shoot2));
            Logs[id].AddRange(BitConverter.GetBytes(rating1));
            Logs[id].AddRange(BitConverter.GetBytes(rating2));
            Logs[id].AddRange(BitConverter.GetBytes(K1));
            Logs[id].AddRange(BitConverter.GetBytes(K2));
        }
    }
    */
}
