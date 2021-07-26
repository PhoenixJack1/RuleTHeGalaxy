using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class ShipsParameters
    {
        GSShip Ship;
        public int Hull;
        public int HullRestore;
        public int Shield;
        public int ShieldRestore;
        public int Energy;
        public int EnergyRestore;
        public SortedList<WeaponGroup, int> Accuracy;
        public SortedList<WeaponGroup, int> Evasion;
        public SortedList<WeaponGroup, int> Damage;
        public SortedList<WeaponGroup, int> Absorp;
        public SortedList<WeaponGroup, bool> Immune;
        public List<SecondaryEffect> GroupEffects = new List<SecondaryEffect>();
        public ShipsParameters(GSShip ship)
        {
            Ship = ship;
            Calculate();
        }
        void Calculate()
        {
            int equipmentenergy = 0;
            Accuracy = FillSortedList();
            Evasion = FillSortedList();
            Damage = FillSortedList();
            Absorp = FillSortedList();
            Immune = new SortedList<WeaponGroup, bool>();
            Immune[WeaponGroup.Energy] = false; Immune[WeaponGroup.Physic] = false;
            Immune[WeaponGroup.Irregular] = false; Immune[WeaponGroup.Cyber] = false;
            Hull = Ship.Schema.ShipType.Health;
            HullRestore = 0;
            Shield = Ship.Schema.Shield.Capacity;
            ShieldRestore = Ship.Schema.Shield.Recharge;
            equipmentenergy += Ship.Schema.Shield.Consume;
            Energy = Ship.Schema.Generator.Capacity;
            EnergyRestore = Ship.Schema.Generator.Recharge;
            Accuracy[WeaponGroup.Energy] = Ship.Schema.Computer.Accuracy[0];
            Accuracy[WeaponGroup.Physic] = Ship.Schema.Computer.Accuracy[1];
            Accuracy[WeaponGroup.Irregular] = Ship.Schema.Computer.Accuracy[2];
            Accuracy[WeaponGroup.Cyber] = Ship.Schema.Computer.Accuracy[3];
            Damage[WeaponGroup.Energy] = Ship.Schema.Computer.Damage[0];
            Damage[WeaponGroup.Physic] = Ship.Schema.Computer.Damage[1];
            Damage[WeaponGroup.Irregular] = Ship.Schema.Computer.Damage[2];
            Damage[WeaponGroup.Cyber] = Ship.Schema.Computer.Damage[3];
            equipmentenergy += Ship.Schema.Computer.Consume;
            Evasion[WeaponGroup.Energy] = Ship.Schema.Engine.EnergyEvasion;
            Evasion[WeaponGroup.Physic] = Ship.Schema.Engine.PhysicEvasion;
            Evasion[WeaponGroup.Irregular] = Ship.Schema.Engine.IrregularEvasion;
            Evasion[WeaponGroup.Cyber] = Ship.Schema.Engine.CyberEvasion;
            equipmentenergy += Ship.Schema.Engine.Consume;
            for (int i = 0; i < Ship.Schema.Equipments.Length; i++)
                if (Ship.Schema.Equipments[i] != null)
                {
                    Abilities.AddSkillEffect(Ship.Schema.Equipments[i], this);
                    equipmentenergy += Ship.Schema.Equipments[i].Consume;
                }
            EnergyRestore -= equipmentenergy;
            if (Ship.Pilot != null)
            {
                SecondaryEffect[] effects = Ship.Pilot.GetEffects();
                foreach (SecondaryEffect effect in effects)
                {
                    Abilities.AddSkillEffect(effect.Type, effect.Value, this);
                    if (effect.IsGroup)
                        GroupEffects.Add(effect);
                }
            }   
        }
        SortedList<WeaponGroup, int> FillSortedList()
        {
            SortedList<WeaponGroup, int> result = new SortedList<WeaponGroup, int>();
            result[WeaponGroup.Energy] = 0;
            result[WeaponGroup.Physic]=0;
            result[WeaponGroup.Irregular] = 0;
            result[WeaponGroup.Cyber] = 0;
            return result;
        }
        
    }
}
