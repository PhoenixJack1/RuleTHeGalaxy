using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class ServerBattleParams
    {
        public ValCapParameter Health;
        public ValCapParameter Shield;
        public ValCapParameter Energy;
        public FixedParameter Evasion;
        public FixedParameter Ignore;
        public FixedParameter Accuracy;
        public FixedParameter Increase;
        public FixedParameter Immune;
        public ShipStatus Status;
        public byte ShieldProtection;
        public byte TimeToEnter;
        public byte JumpDistance;
        public FixSingleParameter HexMoveCost;
        //public int Angle;
        public ShipParamsClass BasicParams;
        public void CalculateBigShip(byte level)
        {
            int hp, sd, sr, en, ac, ar, di;
            switch (level)
            {
                case 5: hp = 500; sd = 1000; sr = 200; en = 300; ac = 100; ar = 30; di = 50; break;
                case 15: hp = 1000; sd = 2000; sr = 400; en = 400; ac = 130; ar = 35; di = 70; break;
                case 25: hp = 1500; sd = 3000; sr = 600; en = 500; ac = 160; ar = 40; di = 90; break;
                case 35: hp = 2000; sd = 4000; sr = 800; en = 600; ac = 190; ar = 45; di = 110; break;
                case 45: hp = 2500; sd = 5000; sr = 1000; en = 700; ac = 220; ar = 50; di = 130; break;
                case 55: hp = 3000; sd = 6000; sr = 1200; en = 800; ac = 250; ar = 55; di = 150; break;
                default: hp = 0; sd = 0; sr = 0; en = 0; ac = 0; ar = 0; di = 0; break;
            }
            Health = new ValCapParameter(hp, 0, 100);
            Shield = new ValCapParameter(sd, sr, 100);
            Energy = new ValCapParameter(en * 2, en, 100);
            Evasion = new FixedParameter(0, 0, 0, 0);
            Ignore = new FixedParameter(ar, ar, ar, ar);
            Ignore.MaxValue = 90;
            Accuracy = new FixedParameter(ac, ac, ac, ac);
            Increase = new FixedParameter(di, di, di, di);
            Immune = new FixedParameter(0, 0, 0, 0);
            Immune.MaxValue = 100;
            Status = new ShipStatus();
            ShieldProtection = 180;
            TimeToEnter = 0;
            JumpDistance = 0;
            HexMoveCost = new FixSingleParameter((int)(en * 0.7));
        }
        public void CalculateStoryShip(ShipBattleParam param)
        {
            Health = new ValCapParameter(param.Health[0], param.Health[1], 100);
            Shield = new ValCapParameter(param.Shield[0], param.Shield[1], 100);
            Energy = new ValCapParameter(param.Energy[0], param.Energy[1], 100);
            Evasion = new FixedParameter(param.Evasion[0], param.Evasion[1], param.Evasion[2], param.Evasion[3]);
            Ignore = new FixedParameter(param.Ignore[0], param.Ignore[1], param.Ignore[2], param.Ignore[3]);
            Ignore.MaxValue = 90;
            Accuracy = new FixedParameter(param.Accuracy[0], param.Accuracy[1], param.Accuracy[2], param.Accuracy[3]);
            Increase = new FixedParameter(param.Increase[0], param.Increase[1], param.Increase[2], param.Increase[3]);
            Immune = new FixedParameter(param.Immune[0], param.Immune[1], param.Immune[2], param.Immune[3]);
            Immune.MaxValue = 100;
            Status = new ShipStatus();
            ShieldProtection = (byte)param.Shield[2];
            TimeToEnter = (byte)param.TimeToEnter;
            JumpDistance = (byte)param.JumpDistance;
            HexMoveCost = new FixSingleParameter(param.HexMoveCost);
        }
        public void CalculateCargoShip(byte level)
        {
            int hp, sd, sr, ar;
            switch (level)
            {
                case 5: hp = 1000; sd = 500; sr = 100; ar = 20; break;
                case 15: hp = 1500; sd = 750; sr = 150; ar = 25; break;
                case 25: hp = 2000; sd = 1000; sr = 200; ar = 30; break;
                case 35: hp = 2500; sd = 1250; sr = 250; ar = 35; break;
                case 45: hp = 3000; sd = 1500; sr = 300; ar = 40; break;
                case 55: hp = 3500; sd = 2000; sr = 400; ar = 45; break;
                default: hp = 0; sd = 0; sr = 0; ar = 0; break;
            }
            Health = new ValCapParameter(hp, 0, 100);
            Shield = new ValCapParameter(sd, sr, 100);
            Energy = new ValCapParameter(0, 0, 100);
            Evasion = new FixedParameter(0, 0, 0, 0);
            Ignore = new FixedParameter(ar, ar, ar, ar);
            Ignore.MaxValue = 90;
            Accuracy = new FixedParameter(0, 0, 0, 0);
            Increase = new FixedParameter(0, 0, 0, 0);
            Immune = new FixedParameter(0, 0, 0, 0);
            Immune.MaxValue = 100;
            Status = new ShipStatus();
            ShieldProtection = 150;
            TimeToEnter = 0;
            JumpDistance = 0;
            HexMoveCost = new FixSingleParameter((int)(999));
        }
        public void CalculatePortal(int HealthValue)
        {
            Health = new ValCapParameter(HealthValue, 0, 100);
            Shield = new ValCapParameter(0, 0, 100);
            Energy = new ValCapParameter(0, 0, 100);
            Evasion = new FixedParameter(0, 0, 0, 0);
            Ignore = new FixedParameter(20, 20, 20, 20);
            Ignore.MaxValue = 90;
            Accuracy = new FixedParameter(0, 0, 0, 0);
            Increase = new FixedParameter(0, 0, 0, 0);
            Immune = new FixedParameter(0, 0, 0, 0);
            Immune.MaxValue = 100;
            Status = new ShipStatus();
            ShieldProtection = 0;
            TimeToEnter = 0;
            JumpDistance = 0;
            HexMoveCost = new FixSingleParameter(0);

        }
        public void CalculateBase(ServerShipB Ship)
        {
            BasicParams = new ShipParamsClass();
            Ship.Schema.ShipType.SetStats(BasicParams, Ship.Schema.Armor);
            Ship.Schema.Generator.SetStats(BasicParams);
            Ship.Schema.Shield.SetStats(BasicParams);
            Ship.Schema.Engine.SetStats(BasicParams);
            Ship.Schema.Computer.SetStats(BasicParams);
            for (int i = 0; i < Ship.Schema.ShipType.EquipmentCapacity; i++)
                if (Ship.Schema.Equipments[i] != null)
                {
                    ServerSecondaryEffect effect = Ship.Schema.Equipments[i].GetServerEffect();
                    effect.AppendBasicParams(BasicParams);
                    if (effect.IsGroup) Ship.AddGroupEffect(effect);
                    BasicParams.BasicEnergySpent += Ship.Schema.Equipments[i].Consume;
                }

            ServerSecondaryEffect[] pilotseffects = Ship.Pilot.GetServerEffects();
            foreach (ServerSecondaryEffect effect in pilotseffects)
            {
                effect.AppendBasicParams(BasicParams);
                if (effect.IsGroup) Ship.AddGroupEffect(effect);
            }

            ShieldProtection = (byte)(30 + (int)Ship.Schema.Shield.Size * 60);
            TimeToEnter = (byte)(3 - (byte)Ship.Schema.Generator.Size);
            JumpDistance = (byte)(Ship.Schema.Computer.Size + 1);
            HexMoveCost = new FixSingleParameter(BasicParams.BasicEnergyRecharge * Ship.Schema.Engine.GetHexMoveSpent / 100);
            BasicParams.BasicEnergyRecharge -= BasicParams.BasicEnergySpent;
            Health = new ValCapParameter(BasicParams.BasicHealth, BasicParams.BasicHealthRestore, Ship.StartHealth);
            Shield = new ValCapParameter(BasicParams.BasicShieldCapacity, BasicParams.BasicShieldRecharge, 100);
            Energy = new ValCapParameter(BasicParams.BasicEnergyCapacity, BasicParams.BasicEnergyRecharge, 100);
            Evasion = new FixedParameter(BasicParams.BasicWEnergyEvasion, BasicParams.BasicWPhysicEvasion,
                BasicParams.BasicWIrregularEvasion, BasicParams.BasicWCyberEvasion);
            Ignore = new FixedParameter(BasicParams.BasicWEnergyIgnore, BasicParams.BasicWPhysicIgnore,
                BasicParams.BasicWIrregularIgnore, BasicParams.BasicWCyberIgnore);
            Ignore.MaxValue = 90;
            Accuracy = new FixedParameter(BasicParams.BasicWEnergyAccuracy, BasicParams.BasicWPhysicAccuracy,
                BasicParams.BasicWIrregularAccuracy, BasicParams.BasicWCyberAccuracy);
            Increase = new FixedParameter(BasicParams.BasicWEnergyIncrease, BasicParams.BasicWPhysicIncrease,
                BasicParams.BasicWIrregularIncrease, BasicParams.BasicWCyberIncrease);
            Immune = new FixedParameter(BasicParams.WEnergyImmune, BasicParams.WPhysicImmune,
                BasicParams.WIrregularImmune, BasicParams.WCyberImmune);
            Immune.MaxValue = 100;

            Status = new ShipStatus();
        }
    }

    public class FixSingleParameter
    {
        public int Basic;
        public int Bonus;
        public int GetCurrent
        { get { return Basic + Bonus; } }
        public FixSingleParameter(int basic)
        {
            Basic = basic; Bonus = 0;
        }

    }
    public class FixedParameter
    {
        public SortedList<WeaponGroup, int> Basic;
        public SortedList<WeaponGroup, int> Bonus;
        public int MaxValue = Int32.MaxValue;
        public FixedParameter(int e, int p, int i, int c)
        {
            Basic = new SortedList<WeaponGroup, int>();
            Basic.Add(WeaponGroup.Energy, e);
            Basic.Add(WeaponGroup.Physic, p);
            Basic.Add(WeaponGroup.Irregular, i);
            Basic.Add(WeaponGroup.Cyber, c);
            Bonus = new SortedList<WeaponGroup, int>();
            Bonus.Add(WeaponGroup.Energy, 0);
            Bonus.Add(WeaponGroup.Physic, 0);
            Bonus.Add(WeaponGroup.Irregular, 0);
            Bonus.Add(WeaponGroup.Cyber, 0);
        }
        public FixedParameter(byte[] array, int i)
        {
            Basic = new SortedList<WeaponGroup, int>();
            Basic.Add(WeaponGroup.Energy, BitConverter.ToUInt16(array, i)); i += 2;
            Basic.Add(WeaponGroup.Physic, BitConverter.ToUInt16(array, i)); i += 2;
            Basic.Add(WeaponGroup.Irregular, BitConverter.ToUInt16(array, i)); i += 2;
            Basic.Add(WeaponGroup.Cyber, BitConverter.ToUInt16(array, i)); i += 2;
            Bonus = new SortedList<WeaponGroup, int>();
            Bonus.Add(WeaponGroup.Energy, 0);
            Bonus.Add(WeaponGroup.Physic, 0);
            Bonus.Add(WeaponGroup.Irregular, 0);
            Bonus.Add(WeaponGroup.Cyber, 0);
        }
        public int GetCurValue(WeaponGroup group)
        {
            int result = Basic[group] + Bonus[group];
            if (result > MaxValue) return MaxValue;
            return result;
        }
        public override string ToString()
        {
            return string.Format("E {0}+{1}={2} P {3}+{4}={5} I {6}+{7}={8} C {9}+{10}={11}",
                Basic[WeaponGroup.Energy], Bonus[WeaponGroup.Energy], GetCurValue(WeaponGroup.Energy),
                Basic[WeaponGroup.Physic], Bonus[WeaponGroup.Physic], GetCurValue(WeaponGroup.Physic),
                Basic[WeaponGroup.Irregular], Bonus[WeaponGroup.Irregular], GetCurValue(WeaponGroup.Irregular),
                Basic[WeaponGroup.Cyber], Bonus[WeaponGroup.Cyber], GetCurValue(WeaponGroup.Cyber));
        }
        public byte[] GetStartArray()
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes((ushort)Basic[WeaponGroup.Energy]));
            list.AddRange(BitConverter.GetBytes((ushort)Basic[WeaponGroup.Physic]));
            list.AddRange(BitConverter.GetBytes((ushort)Basic[WeaponGroup.Irregular]));
            list.AddRange(BitConverter.GetBytes((ushort)Basic[WeaponGroup.Cyber]));
            return list.ToArray();
        }
    }
    public class ValCapParameter
    {
        int StartBasic; //начальное базовое значение
        public int Basic; //максимальное базовое здоровье
        public int BasicValue; //текущее базовое здоровье
        public int Bonus; //максимальное бонусное здоровье
        public int BonusValue; //текущее бонустное здоровье
        public int RestBasic; //базовое восстановление
        public int RestBonus; //бонусное восстановление
        public int RestCurr; //текущее восстановление
        public int Disease; //текущее снижение
        int MinValue; //минимальное значение
        public ValCapParameter(int basic, int rest, byte curr)
        {
            StartBasic = basic;
            Bonus = 0; BonusValue = 0; RestBonus = 0; Disease = 0; //все бонусные параметры обнуляются
            Basic = (int)(((double)curr / 100) * basic); //Начальное базовое здоровье равно проектному умноженному на текущую прочность корабля
            RestBasic = rest; //базовое восстановление равно проектному
            RestCurr = RestBasic; //текущее восстановление равно базовому
            BasicValue = Basic; //текущее базовое здоровье равно базовому
            MinValue = BasicValue;
        }
        public ValCapParameter(byte[] array, int i)
        {
            Bonus = 0; BonusValue = 0; RestBonus = 0; Disease = 0; //все бонусные параметры обнуляются
            Basic = BitConverter.ToInt32(array, i); i += 4;
            RestBasic = BitConverter.ToUInt16(array, i);
            RestCurr = RestBasic; //текущее восстановление равно базовому
            BasicValue = Basic; //текущее базовое здоровье равно базовому
        }
        public byte[] GetStartArray()
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(Basic));
            list.AddRange(BitConverter.GetBytes((ushort)RestCurr));
            return list.ToArray();
        }
        public int GetMax
        { get { return Basic + Bonus; } }
        public int GetCurrent
        { get { return BasicValue + BonusValue; } }
        public int GetRestore
        { get { return RestCurr - Disease; } }
        public byte GetPercent
        { get { return (byte)(MinValue * 100 / StartBasic); } }
        public string GetRestoreString
        { get { if (GetRestore < 0) return GetRestore.ToString(); else return "+" + GetRestore.ToString(); } }
        public void AddDisease(int value)
        {
            Disease += value;
        }
        public void RemoveDisease(int value)
        {
            Disease -= value;
        }
        public void AddRestore(int value)
        {
            RestBonus += value;
            RestCurr = RestBasic + RestBonus;
        }
        public void RemoveRestore(int value)
        {
            RestBonus -= value;
            if (RestBonus < 0) RestBonus = 0;
            RestCurr = RestBasic + RestBonus;
        }
        public void AddBonusValue(int value)
        {
            Bonus += value;
            BonusValue += value;
        }
        public void RemoveBonusValue(int value)
        {
            if (value >= Bonus)
            {
                Bonus = 0; BonusValue = 0;
            }
            else
            {
                double percent = (double)BonusValue / Bonus;
                Bonus -= value;
                BonusValue = (int)(Bonus * percent);
            }
        }
        public void AddParam(int value)
        {
            BasicValue += value;
            if (BasicValue <= Basic)
                return;
            BonusValue += BasicValue - Basic;
            BasicValue = Basic;
            if (BonusValue > Bonus)
                BonusValue = Bonus;
        }
        public void SetNull()
        {
            BasicValue = 0;
            BonusValue = 0;
            MinValue = 0;
        }
        public void SetMax()
        {
            BasicValue = Basic;
            BonusValue = Bonus;
            Disease = 0;
        }
        public int RemoveParam(int value)
        {
            BonusValue -= value;
            if (BonusValue >= 0)
                return 0;
            BasicValue += BonusValue;
            BonusValue = 0;
            if (BasicValue < 0)
            {
                int rest = -BasicValue;
                BasicValue = 0;
                MinValue = 0;
                return rest;
            }
            if (MinValue > BasicValue) MinValue = BasicValue;
            return 0;
        }
        public int RemovePercent(int percent)
        {
            int value = GetMax * percent / 100;
            return RemoveParam(value);
        }
        public int RemoveParam(int value, bool isReal)
        {
            if (isReal == false)
            {
                int BonusValueTemp = BonusValue - value;
                if (BonusValueTemp >= 0)
                    return 0;
                int BasicValueTemp = BonusValueTemp + BasicValue;
                if (BasicValueTemp < 0)
                    return -BasicValueTemp;
                else
                    return 0;
            }
            BonusValue -= value;
            if (BonusValue >= 0)
                return 0;
            BasicValue += BonusValue;
            BonusValue = 0;
            if (BasicValue < 0)
            {
                int rest = -BasicValue;
                BasicValue = 0;
                return rest;
            }
            return 0;
        }
        public int RemovePercent(int percent, bool isReal)
        {
            int value = GetMax * percent / 100;
            return RemoveParam(value, isReal);
        }
        public void RoundStart()
        {
            AddParam(RestCurr);
        }
        public void RoundEnd()
        {
            RemoveParam(Disease);
            Disease = (int)(Disease * 0.7);
            if (Disease < 50) Disease = 0;
        }
        public byte[] GetLogInfo()
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes((short)Basic));
            list.AddRange(BitConverter.GetBytes((short)Bonus));
            list.AddRange(BitConverter.GetBytes((short)GetMax));
            list.AddRange(BitConverter.GetBytes((short)BasicValue));
            list.AddRange(BitConverter.GetBytes((short)BonusValue));
            list.AddRange(BitConverter.GetBytes((short)GetCurrent));
            list.AddRange(BitConverter.GetBytes((short)RestBasic));
            list.AddRange(BitConverter.GetBytes((short)RestBonus));
            list.AddRange(BitConverter.GetBytes((short)RestCurr));
            list.AddRange(BitConverter.GetBytes((short)Disease));
            return list.ToArray();
        }
        public static string GetLogString(byte[] array, ref int i)
        {
            short[] Params = new short[10];
            for (int j = 0; j < 10; j++)
            {
                Params[j] = BitConverter.ToInt16(array, i); i += 2;
            }
            return string.Format("Capacity {0}+{1}={2} Value {3}+{4}={5} Restore {6}+{7}={8} Disease {9}",
                Params[0], Params[1], Params[2], Params[3], Params[4], Params[5], Params[6], Params[7], Params[8], Params[9]);
        }
        public override string ToString()
        {
            return string.Format("Capacity {0}+{1}={2} Value {3}+{4}={5} Restore {6}+{7}={8} Disease {9}",
                Basic, Bonus, GetMax, BasicValue, BonusValue, GetCurrent, RestBasic, RestBonus, RestCurr, Disease);
        }
    }
}
