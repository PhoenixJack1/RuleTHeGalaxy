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
    /*
    enum LogMessage
    {
        BattleStart, RoundStart, RoundEnd, RecieveDamage,
        LaserCrit, EMICrit, PlasmaCrit, SolarCrit,
        CannonCrit, GaussCrit, MissleCrit, AntiCrit,
        PsiCrit, DarkCrit, WarpCrit, TimeCrit,
        SliceCrit, RadCrit, DroneCrit, MagnetCrit,
        ShieldBlockDamage, ShieldPierce, HealthDamageLive, HealthDamageDestroy,
        MoveToGate, MoveToPort, SelfWeaponDamage, PsiAsteroidDamage, PsiWeaponDamage, AsteroidDamage, WeaponDamage, ShipReturned,
        ShipDestroyed, ShipMove, RotateShip, MoveToField, EnergySpent, BattleEnd
    };
    class TestBattle
    {
        public static List<string> ReconLog(byte[] array, long id)
        {
            //byte[] array = Logs[id].ToArray();
            List<string> result = new List<string>();
            DateTime StartTime = new DateTime(BitConverter.ToInt64(array, 0));
            result.Add(String.Format("Начало боя ID={0} Время={1}",id,StartTime.ToLongTimeString()));
            byte side1shipscount = array[8]; int i = 9;
            for (int j = 0; j < side1shipscount; j++)
            {
                result.AddRange(ShipB.GetLogString(array, ref i));
            }
            byte side2shipscount = array[i]; i++;
            for (int j = 0; j < side2shipscount; j++)
            {
                result.AddRange(ShipB.GetLogString(array, ref i));
            }
            for (;i<array.Length;)
            {
                LogMessage message = (LogMessage)array[i]; i++;
                switch (message)
                {
                    case LogMessage.BattleStart: result.AddRange(BattleStart(array, ref i)); break;
                    case LogMessage.RoundStart: result.AddRange(RoundStart(array, ref i)); break;
                    case LogMessage.RoundEnd: result.AddRange(RoundEnd(array, ref i)); break;
                    case LogMessage.RecieveDamage: result.AddRange(RecieveDamage(array, ref i)); break;
                    case LogMessage.LaserCrit: result.Add("КРИТ ЛАЗЕРА"); break;
                    case LogMessage.EMICrit: result.Add("КРИТ ЭМИ"); break;
                    case LogMessage.PlasmaCrit: result.Add("КРИТ ПЛАЗМЫ"); break;
                    case LogMessage.SolarCrit: result.Add("КРИТ СОЛНЕЧНОЙ ПУШКИ"); break;
                    case LogMessage.CannonCrit: result.Add("КРИТ ПУШКИ"); break;
                    case LogMessage.GaussCrit: result.Add("КРИТ ГАУССА"); break;
                    case LogMessage.MissleCrit: result.Add("КРИТ РАКЕТ"); break;
                    case LogMessage.AntiCrit: result.Add("КРИТ ПУШКИ АНТИМАТЕРИИ");  break;
                    case LogMessage.PsiCrit: result.Add("КРИТ ПСИ ПУШКИ"); break;
                    case LogMessage.DarkCrit: result.Add("КРИТ ПУШКИ ТЁМНОЙ ЭНЕРГИИ"); break;
                    case LogMessage.WarpCrit: result.Add("КРИТ ВАРПА"); break;
                    case LogMessage.TimeCrit: result.Add("КРИТ ВРЕМЕННОЙ ПУШКИ"); break;
                    case LogMessage.SliceCrit: result.Add("КРИТ ХАКЕРСКОЙ ПУШКИ"); break;
                    case LogMessage.RadCrit: result.Add("КРИТ РАДИОАКТИВНОЙ ПУШКИ"); break;
                    case LogMessage.DroneCrit: result.Add("КРИТ ДРОНА"); break;
                    case LogMessage.MagnetCrit: result.Add("КРИТ МАГНИТНОЙ ПУШКИ"); break;
                    case LogMessage.ShieldBlockDamage: result.AddRange(ShieldBlockDamage(array, ref i)); break;
                    case LogMessage.ShieldPierce: result.Add("Щит не сработал"); break;
                    case LogMessage.HealthDamageLive: result.AddRange(HealthDamageLive(array, ref i)); break;
                    case LogMessage.HealthDamageDestroy: result.AddRange(HealthDamageDestroy(array, ref i)); break;
                    case LogMessage.MoveToGate: result.AddRange(MoveShipToGate(array, ref i)); break;
                    case LogMessage.MoveToPort: result.AddRange(MoveShipToPort(array, ref i)); break;
                    case LogMessage.SelfWeaponDamage:result.AddRange(SelfWeaponDamage(array, ref i));  break;
                    case LogMessage.PsiAsteroidDamage: result.AddRange(PsiAsteroidDamage(array, ref i)); break;
                    case LogMessage.PsiWeaponDamage: result.AddRange(PsiWeaponDamage(array, ref i)); break;
                    case LogMessage.AsteroidDamage: result.AddRange(AsteroidDamage(array, ref i)); break;
                    case LogMessage.WeaponDamage: result.AddRange(WeaponDamage(array, ref i)); break;
                    case LogMessage.ShipReturned: result.AddRange(ShipReturned(array, ref i)); break;
                    case LogMessage.ShipDestroyed: result.AddRange(ShipDestroyd(array, ref i)); break;
                    case LogMessage.ShipMove: result.AddRange(ShipMove(array, ref i)); break;
                    case LogMessage.RotateShip: result.AddRange(ShipRotate(array, ref i)); break;
                    case LogMessage.MoveToField: result.AddRange(MoveShipToField(array, ref i)); break;
                    case LogMessage.EnergySpent: result.AddRange(EnergySpent(array, ref i)); break;
                    case LogMessage.BattleEnd: result.AddRange(BattleEnd(array, ref i)); break;
                }
            }
            return result;
        }
        static List<string> BattleStart(byte[] array, ref int i)
        {
            List<string> list = new List<string>();
            short currturn = BitConverter.ToInt16(array, i); i += 2;
            list.Add(String.Format("{0} НАЧАЛО БОЯ", currturn));
            return list;
        }
        static List<string> RoundStart(byte[] array, ref int i)
        {
            List<string> list = new List<string>();
            short currturn = BitConverter.ToInt16(array, i); i += 2;
            byte round = array[i]; i++;
            list.Add(String.Format("{0} НАЧАЛО РАУНДА {1}", currturn, round));
            return list;
        }
        static List<string> RoundEnd(byte[] array, ref int i)
        {
            List<string> list = new List<string>();
            short currturn = BitConverter.ToInt16(array, i); i += 2;
            byte round = array[i]; i++;
            list.Add(String.Format("{0} КОНЕЦ РАУНДА {1}", currturn, round));
            return list;
        }
        static List<string> RecieveDamage(byte[] array, ref int i)
        {
            List<string> list = new List<string>();
            short currturn = BitConverter.ToInt16(array, i); i += 2;
            ShipSide side = (ShipSide)array[i];i++;
            byte shipid = array[i]; i++;
            EWeaponType weapon = (EWeaponType)array[i]; i++;
            short angle = BitConverter.ToInt16(array, i); i += 2;
            list.Add(String.Format("{0} ПОЛУЧЕНИЕ УРОНА", currturn));
            list.Add(String.Format("{0} ID={1} ОРУЖИЕ {2} УГОЛ={3}", side.ToString(), shipid, weapon.ToString(), angle));
            return list;
        }
        static List<string> ShieldBlockDamage(byte[] array, ref int i)
        {
            List<string> list = new List<string>();
            short shielddamage = BitConverter.ToInt16(array, i); i += 2;
            short shieldleft = BitConverter.ToInt16(array, i); i += 2;
            short shieldmax = BitConverter.ToInt16(array, i); i += 2;
            list.Add(String.Format("Щит не пробит. Урон по щиту {0}, остаток щита {1}/{2}", shielddamage, shieldleft, shieldmax));
            return list;
        }
        static List<string> HealthDamageLive(byte[] array, ref int i)
        {
            List<string> list = new List<string>();
            short shielddamage = BitConverter.ToInt16(array, i); i += 2;
            short healthdamage = BitConverter.ToInt16(array, i); i += 2;
            short healthleft = BitConverter.ToInt16(array, i); i += 2;
            short healthmax = BitConverter.ToInt16(array, i); i += 2;
            list.Add(String.Format("Щит пробит. Урон по щиту {0}, урон по броне {1}, остаток брони {2}/{3}", shielddamage, healthdamage, healthleft, healthmax));
            return list;
        }
        static List<string> HealthDamageDestroy(byte[] array, ref int i)
        {
            List<string> list = new List<string>();
            short shielddamage = BitConverter.ToInt16(array, i); i += 2;
            short healthdamage = BitConverter.ToInt16(array, i); i += 2;
            list.Add(String.Format("Щит пробит. Корабль УНИЧТОЖЕН. Урон по щиту {0}, урон по броне {1}.", shielddamage, healthdamage));
            return list;
        }
        static List<string> MoveShipToGate(byte[] array, ref int i)
        {
            List<string> list = new List<string>();
            short currturn = BitConverter.ToInt16(array, i); i += 2;
            byte shipid = array[i]; i++;
            ShipSide side = (ShipSide)array[i]; i++;
            byte jumpdistance = array[i]; i++;
            list.Add(String.Format("{0} КОРАБЛЬ ЗАШЁЛ В ГЕЙТ", currturn));
            list.Add(String.Format("{0} ID={1} Дальность прыжка {2}", side.ToString(), shipid, jumpdistance));
            return list;
        }
        static List<string> MoveShipToPort(byte[] array, ref int i)
        {
            List<string> list = new List<string>();
            short currturn = BitConverter.ToInt16(array, i); i += 2;
            byte shipid = array[i]; i++;
            ShipSide side = (ShipSide)array[i]; i++;
            byte waittime = array[i]; i++;
            list.Add(String.Format("{0} КОРАБЛЬ ЗАШЁЛ В ПОРТ", currturn));
            list.Add(String.Format("{0} ID={1} Время ожидания {2}", side.ToString(), shipid, waittime));
            return list;
        }
        static List<string> SelfWeaponDamage(byte[] array, ref int i)
        {
            List<string> list = new List<string>();
            short currturn = BitConverter.ToInt16(array, i); i += 2;
            byte shipid = array[i]; i++;
            ShipSide side = (ShipSide)array[i]; i++;
            byte targetid = array[i]; i++;
            byte gun = array[i]; i++;
            bool iscrit = BitConverter.ToBoolean(array, i); i++;
            short angle = BitConverter.ToInt16(array, i); i += 2;
            list.Add(string.Format("{0} ДЕТОНАЦИЯ ВЫСТРЕЛА", currturn));
            list.Add(string.Format("{0} ID={1} ID цели={2} Пушка {3} {4}Угол {5}", side.ToString(), shipid, targetid, gun, (iscrit ? "КРИТ " : ""), angle));
            return list;
        }
        static List<string> PsiAsteroidDamage(byte[] array, ref int i)
        {
            List<string> list = new List<string>();
            short currturn = BitConverter.ToInt16(array, i); i += 2;
            byte shipid = array[i]; i++;
            ShipSide side = (ShipSide)array[i]; i++;
            byte hex = array[i]; i++;
            byte gun = array[i]; i++;
            list.Add(string.Format("{0} ВЫСТРЕЛ ПО СВОИМ СБЛОКИРОВАННЫЙ АСТЕРОИДОМ", currturn));
            list.Add(string.Format("{0} ID={1} ХЕКС={2} Пушка {3}", side.ToString(), shipid, hex, gun));
            return list;
        }
        static List<string> PsiWeaponDamage(byte[] array, ref int i)
        {
            List<string> list = new List<string>();
            short currturn = BitConverter.ToInt16(array, i); i += 2;
            byte shipid = array[i]; i++;
            ShipSide side = (ShipSide)array[i]; i++;
            byte targetid = array[i]; i++;
            byte gun = array[i]; i++;
            bool iscrit = BitConverter.ToBoolean(array, i); i++;
            short angle = BitConverter.ToInt16(array, i); i += 2;
            bool isaim = BitConverter.ToBoolean(array, i); i++;
            list.Add(string.Format("{0} ВЫСТРЕЛ ПО СВОИМ", currturn));
            list.Add(string.Format("{0} ID={1} ID цели={2} Пушка {3} {4}Угол {5}", side.ToString(), shipid, targetid, gun, (iscrit ? "КРИТ " : ""), angle));
            list.Add(string.Format("{0}", (isaim ? "ПОПАДАНИЕ" : "ПРОМАХ")));
            return list;
        }
        static List<string> AsteroidDamage(byte[] array, ref int i)
        {
            List<string> list = new List<string>();
            short currturn = BitConverter.ToInt16(array, i); i += 2;
            byte shipid = array[i]; i++;
            ShipSide side = (ShipSide)array[i]; i++;
            byte hex = array[i]; i++;
            byte gun = array[i]; i++;
            list.Add(string.Format("{0} ВЫСТРЕЛ ПО АСТЕРОИДУ", currturn));
            list.Add(string.Format("{0} ID={1} ХЕКС={2}  Пушка {3}", side.ToString(), shipid, hex, gun));
            return list;
        }
        static List<string> WeaponDamage(byte[] array, ref int i)
        {
            List<string> list = new List<string>();
            short currturn = BitConverter.ToInt16(array, i); i += 2;
            byte shipid = array[i]; i++;
            ShipSide side = (ShipSide)array[i]; i++;
            byte targetid = array[i]; i++;
            byte gun = array[i]; i++;
            bool iscrit = BitConverter.ToBoolean(array, i); i++;
            short angle = BitConverter.ToInt16(array, i); i += 2;
            bool isaim = BitConverter.ToBoolean(array, i); i++;
            list.Add(string.Format("{0} ВЫСТРЕЛ ПО ВРАГУ", currturn));
            list.Add(string.Format("{0} ID={1} ID цели={2} Пушка {3} {4}Угол {5}", side.ToString(), shipid, targetid, gun, (iscrit ? "КРИТ " : ""), angle));
            list.Add(string.Format("{0}", (isaim ? "ПОПАДАНИЕ" : "ПРОМАХ")));
            return list;
        }
        static List<string> ShipReturned(byte[] array, ref int i)
        {
            List<string> list = new List<string>();
            short currturn = BitConverter.ToInt16(array, i); i += 2;
            byte shipid = array[i]; i++;
            ShipSide side = (ShipSide)array[i]; i++;
            list.Add(string.Format("{0} ВОЗВРАЩЕНИЕ НА БАЗУ {1} ID={2}", currturn, side.ToString(), shipid));
            return list;
        }
        static List<string> ShipDestroyd(byte[] array, ref int i)
        {
            List<string> list = new List<string>();
            short currturn = BitConverter.ToInt16(array, i); i += 2;
            byte shipid = array[i]; i++;
            ShipSide side = (ShipSide)array[i]; i++;
            byte hex = array[i];i++;
            list.Add(string.Format("{0} КОРАБЛЬ УНИЧТОЖЕН {1} ID={2} Hex={3}", currturn, side.ToString(), shipid, hex));
            return list;
        }
        static List<string> ShipMove(byte[] array, ref int i)
        {
            List<string> list = new List<string>();
            short currturn = BitConverter.ToInt16(array, i); i += 2;
            byte shipid = array[i]; i++;
            ShipSide side = (ShipSide)array[i]; i++;
            byte hex = array[i]; i++;
            short angle = BitConverter.ToInt16(array, i); i += 2;
            list.Add(string.Format("{0} КОРАБЛЬ ПЕРЕМЕСТИЛСЯ {1} ID={2} Hex={3} Угол={4}", currturn, side.ToString(), shipid, hex, angle));
            return list;
        }
        static List<string> ShipRotate(byte[] array, ref int i)
        {
            List<string> list = new List<string>();
            short currturn = BitConverter.ToInt16(array, i); i += 2;
            byte shipid = array[i]; i++;
            ShipSide side = (ShipSide)array[i]; i++;
            byte hex = array[i]; i++;
            short angle = BitConverter.ToInt16(array, i); i += 2;
            list.Add(string.Format("{0} КОРАБЛЬ ПОВЕРНУЛСЯ {1} ID={2} Hex={3} Угол={4}", currturn, side.ToString(), shipid, hex, angle));
            return list;
        }
        static List<string> MoveShipToField(byte[] array, ref int i)
        {
            List<string> list = new List<string>();
            short currturn = BitConverter.ToInt16(array, i); i += 2;
            byte shipid = array[i]; i++;
            ShipSide side = (ShipSide)array[i]; i++;
            byte hex = array[i]; i++;
            list.Add(string.Format("{0} КОРАБЛЬ ВЫШЕЛ НА ПОЛЕ {1} ID={2} Hex={3}", currturn, side.ToString(), shipid, hex));
            return list;
        }
        static List<string> EnergySpent(byte[] array, ref int i)
        {
            List<string> list = new List<string>();
            byte shipid = array[i]; i++;
            ShipSide side = (ShipSide)array[i]; i++;
            EWeaponType weapon = (EWeaponType)array[i]; i++;
            short energy = BitConverter.ToInt16(array, i); i += 2;
            list.Add(string.Format("{0} ID={1} Затрачено энергии на выстрел {2} Пушка={3}", side.ToString(), shipid, energy, weapon.ToString()));
            return list;
        }
        static List<string> BattleEnd(byte[] array, ref int i)
        {
            List<string> list = new List<string>();
            short shoot1 = BitConverter.ToInt16(array, i); i += 2;
            short shoot2 = BitConverter.ToInt16(array, i); i += 2;
            int rating1 = BitConverter.ToInt32(array, i); i += 4;
            int rating2 = BitConverter.ToInt32(array, i); i += 4;
            double K1 = BitConverter.ToDouble(array, i); i += 8;
            double K2 = BitConverter.ToDouble(array, i); i += 8;
            list.Add(string.Format("БОЙ ЗАКОНЧЕН"));
            list.Add(string.Format("Выстрелы АТАКА {0} ЗАЩИТА {1}", shoot1, shoot2));
            list.Add(string.Format("Рейтинг АТАКА {0} ЗАЩИТА {1}", rating1, rating2));
            list.Add(string.Format("Коэффициент АТАКА {0} ЗАЩИТА {1}", K1, K2));
            return list;
        }
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
        public static void AddShip(long id, ShipB ship)
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
    
    class TestBattleBorder:Border
    {
        List<string> list1;
        List<string> list2;
        TextBlock block1;
        TextBlock block2;
        public TestBattleBorder(Battle battle)
        {
            Width = 1200;
            Height = 600;
            Background = Brushes.Black;
            BorderBrush = Brushes.White;
            BorderThickness = new Thickness(3);
            CornerRadius = new CornerRadius(20);
            Grid grid = new Grid();
            Child = grid;
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition()); grid.RowDefinitions[1].Height = new GridLength(50);
            ScrollViewer viewer1 = new ScrollViewer();
            viewer1.Margin = new Thickness(10);
            viewer1.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            viewer1.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            grid.Children.Add(viewer1);
            block1 = new TextBlock();
            block1.FontFamily = Links.Font;
            block1.FontSize = 18;
            block1.Foreground = Brushes.White;
            block1.TextWrapping = TextWrapping.Wrap;
            list1 = TestBattle.ReconLog(TestBattle.Logs[battle.ID].ToArray(), battle.ID);
            foreach (string s in list1)
            { block1.Inlines.Add(new Run(s)); block1.Inlines.Add(new LineBreak()); }

            viewer1.Content = block1;

            ScrollViewer viewer2 = new ScrollViewer();
            viewer2.Margin = new Thickness(10);
            viewer2.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            viewer2.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            grid.Children.Add(viewer2);
            Grid.SetColumn(viewer2, 1);
            block2 = new TextBlock();
            block2.FontFamily = Links.Font;
            block2.FontSize = 18;
            block2.Foreground = Brushes.White;
            block2.TextWrapping = TextWrapping.Wrap;
            byte[] array = Events.GetBattleLog(battle.ID);
            if (array == null) { list2 = new List<string>(); list2.Add("ERROR"); }
            else
            {
                list2 = TestBattle.ReconLog(array, battle.ID);
            }
            foreach (string s in list2)
            { block2.Inlines.Add(new Run(s)); block2.Inlines.Add(new LineBreak()); }

            viewer2.Content = block2;

            Button CloseButton = new Button();
            CloseButton.Width = 200; CloseButton.Height = 50;
            CloseButton.FontFamily = Links.Font; CloseButton.Foreground = Brushes.White;
            CloseButton.Background = Brushes.Black; CloseButton.FontSize = 20;
            CloseButton.Content = Links.Interface("Close");
            CloseButton.Click += CloseButton_Click;
            grid.Children.Add(CloseButton);
            Grid.SetRow(CloseButton, 1); Grid.SetColumn(CloseButton, 1);

            Button CheckButton = new Button();
            CheckButton.Width = 200; CheckButton.Height = 50;
            CheckButton.FontFamily = Links.Font; CheckButton.Foreground = Brushes.White;
            CheckButton.Background = Brushes.Black; CheckButton.FontSize = 20;
            CheckButton.Content = "Сравнить";
            grid.Children.Add(CheckButton);
            Grid.SetRow(CheckButton, 1);
            CheckButton.Click += CheckButton_Click;
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            block1.Inlines.Clear();
            block2.Inlines.Clear();
            int count = 0;
            if (list1.Count > list2.Count) count = list2.Count; else count = list1.Count;
            for (int i=0;i<count;i++)
            {
                if (list1[i]==list2[i])
                {
                    block1.Inlines.Add(new Run(list1[i]));
                    block1.Inlines.Add(new LineBreak());

                    block2.Inlines.Add(new Run(list2[i]));
                    block2.Inlines.Add(new LineBreak());
                }
                else
                {
                    block1.Inlines.Add(GetRedRun(list1[i]));
                    block1.Inlines.Add(new LineBreak());

                    block2.Inlines.Add(GetRedRun(list2[i]));
                    block2.Inlines.Add(new LineBreak());

                }
            }
            if (list1.Count>count)
            {
                for (int i = count+1; i < list1.Count; i++)
                {
                        block1.Inlines.Add(GetRedRun(list1[i]));
                        block1.Inlines.Add(new LineBreak());
                }
            }
            if (list2.Count > count)
            {
                for (int i = count + 1; i < list2.Count; i++)
                {
                    block2.Inlines.Add(GetRedRun(list2[i]));
                    block2.Inlines.Add(new LineBreak());
                }
            }
        }
        Run GetRedRun(string s)
        {
            Run r = new Run(s);
            r.Foreground = Brushes.Red;
            return r;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }
    */
}