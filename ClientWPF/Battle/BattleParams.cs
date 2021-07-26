using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class BattleParams
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
        public ShipParamsClass BasicParams;
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
            //Status = new ShipStatus();
            ShieldProtection = 0;
            TimeToEnter = 0;
            JumpDistance = 0;
            HexMoveCost = new FixSingleParameter(0);

        }
        public void CalculateFromArray(byte[] array, ref int i)
        {
            Health = new ValCapParameter(array, i); i += 6;
            Shield = new ValCapParameter(array, i); i += 6;
            Energy = new ValCapParameter(array, i); i += 6;
            Evasion = new FixedParameter(array, i); i += 8;
            Ignore = new FixedParameter(array, i); i += 8;
            Ignore.MaxValue = 90;
            Accuracy = new FixedParameter(array, i); i += 8;
            Increase = new FixedParameter(array, i); i += 8;
            Immune = new FixedParameter(array, i); i += 8;
            Immune.MaxValue = 100;
            Status = new ShipStatus();
            ShieldProtection = array[i]; i++;
            TimeToEnter = array[i]; i++;
            JumpDistance = array[i]; i++;
            HexMoveCost = new FixSingleParameter(BitConverter.ToUInt16(array, i)); i += 2;
        }
        public void CalculateBase(ShipB Ship)
        {
            BasicParams = new ShipParamsClass();
            if (Ship.Schema.ShipType!=null)
                Ship.Schema.ShipType.SetStats(BasicParams, Ship.Schema.Armor);
            if (Ship.Schema.Generator!=null)
                Ship.Schema.Generator.SetStats(BasicParams);
            if (Ship.Schema.Shield!=null)
                Ship.Schema.Shield.SetStats(BasicParams);
            if (Ship.Schema.Engine!=null)
                Ship.Schema.Engine.SetStats(BasicParams);
            if (Ship.Schema.Computer!=null)
                Ship.Schema.Computer.SetStats(BasicParams);
            if (Ship.Schema.ShipType!=null)
                for (int i = 0; i < Ship.Schema.ShipType.EquipmentCapacity; i++)
                    if (Ship.Schema.Equipments[i] != null)
                    {
                        SecondaryEffect effect = Ship.Schema.Equipments[i].GetEffect();
                        effect.AppendBasicParams(BasicParams);
                        if (effect.IsGroup)
                            Ship.AddGroupEffect(effect);
                        BasicParams.BasicEnergySpent += Ship.Schema.Equipments[i].Consume;
                    }
            if (Ship.Pilot != null)
            {
                SecondaryEffect[] pilotseffects = Ship.Pilot.GetEffects();
                foreach (SecondaryEffect effect in pilotseffects)
                {
                    effect.AppendBasicParams(BasicParams);
                    if (effect.IsGroup) Ship.AddGroupEffect(effect);
                }
            }
            if (Ship.Schema.Shield != null)
                ShieldProtection = (byte)(30 + (int)Ship.Schema.Shield.Size * 60);
            else ShieldProtection = 0;
            if (Ship.Schema.Generator != null)
                TimeToEnter = (byte)(3 - (byte)Ship.Schema.Generator.Size);
            else TimeToEnter = 99;
            if (Ship.Schema.Computer != null)
                JumpDistance = (byte)(Ship.Schema.Computer.Size + 1);
            else JumpDistance = 0;
            if (Ship.Schema.Engine != null)
                HexMoveCost = new FixSingleParameter(BasicParams.BasicEnergyRecharge * Ship.Schema.Engine.GetHexMoveSpent / 100);
            else HexMoveCost = new FixSingleParameter(999);
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
   
    
   
}
