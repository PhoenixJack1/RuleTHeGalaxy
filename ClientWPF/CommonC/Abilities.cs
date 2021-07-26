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

namespace Client
{
    class Abilities
    {
        public static int GetValues(byte pos, byte level)
        {
            switch (pos)
            {
                case 0: case 1: case 2: case 3: return 15 + level * 3; //Accuracy
                case 4: return 10 + level*2; //Accuracy total
                case 5: return 400 + level * 40; //Health
                case 6: return 100 + level * 10; //Health restore
                case 7: return 600 + level * 60; //Shield
                case 8: return 160 + level * 16; //Shield restore
                case 9: return 500 + level * 50; //Energy 
                case 10: return 130 + level * 13; //Energy restore
                case 11: case 12: case 13: case 14: return 15 + level * 3; //Evasion
                case 15: return 10 + level*2; //Evasion total
                case 16: case 17: case 18: case 19: return 10 + level * 2; //Ignore
                case 20: return 5 + level; //Ignore Total
                case 21: case 22: case 23: case 24: return 30 + level * 3; //Damage
                case 25: return 10 + level; //Damage total
                case 30: return 200 + level * 2; //Health group
                case 31: return 50 + level * 5; //Health restore group
                case 32: return 300 + level * 30; //Shield group
                case 33: return 80 + level * 8; //Shield restore group
                case 34: return 250 + level * 25; //Energy group
                case 35: return 65 + level * 12; //Energy restore group
                case 36: case 37: case 38: case 39: return 10 + level * 2; //Acuracy group
                case 40: return 5 + level * 2; //Accuracy total group
                case 41: case 42: case 43: case 44: return 10 + level * 2; //Evasion group
                case 45: return 5 + level * 2; //Evasion total group
                case 46: case 47: case 48: case 49: return 5+level; //Ignore group
                case 50: return 3+level; //Ignore group
                case 51: case 52: case 53: case 54: return 15 + level * 2; //Damage group
                case 55: return 10 + level; //Damage total group
                case 59: case 60: case 61: case 62: case 63: return 100; //Immune
                default: return 0;

            }
        }
        public static bool IsActiveSkill(byte pos)
        {
            if (pos >= 30 && pos <= 55) return true;
            else return false;
        }
        public static void AddSkillEffect(Equipmentclass equipment, ShipsParameters param)
        {
            AddSkillEffect((byte)equipment.Type, equipment.Value, param);
        }
        public static void AddSkillEffect(byte pos, int value, ShipsParameters param)
        {
            switch (pos)
            {
                case 0: case 36: param.Accuracy[WeaponGroup.Energy] += value; break;
                case 1: case 37: param.Accuracy[WeaponGroup.Physic] += value; break;
                case 2: case 38: param.Accuracy[WeaponGroup.Irregular] += value; break;
                case 3: case 39: param.Accuracy[WeaponGroup.Cyber] += value; break;
                case 4: case 40: param.Accuracy[WeaponGroup.Energy] += value; param.Accuracy[WeaponGroup.Physic] += value; 
                    param.Accuracy[WeaponGroup.Irregular] += value; param.Accuracy[WeaponGroup.Cyber]+=value; break;
                case 5: case 30: param.Hull += value; break;
                case 6: case 31: param.HullRestore += value; break;
                case 7: case 32: param.Shield += value; break;
                case 8: case 33: param.ShieldRestore += value; break;
                case 9: case 34: param.Energy += value; break;
                case 10: case 35: param.EnergyRestore += value; break;
                case 11: case 41: param.Evasion[WeaponGroup.Energy] += value; break;
                case 12: case 42: param.Evasion[WeaponGroup.Physic] += value; break;
                case 13: case 43: param.Evasion[WeaponGroup.Irregular] += value; break;
                case 14: case 44: param.Evasion[WeaponGroup.Cyber] += value; break;
                case 15: case 45: param.Evasion[WeaponGroup.Energy]+=value; param.Evasion[WeaponGroup.Physic]+=value;
                    param.Evasion[WeaponGroup.Irregular] += value; param.Evasion[WeaponGroup.Cyber]+=value; break;
                case 16: case 46: param.Absorp[WeaponGroup.Energy] += value; break;
                case 17: case 47: param.Absorp[WeaponGroup.Physic] += value; break;
                case 18: case 48: param.Absorp[WeaponGroup.Irregular] += value; break;
                case 19: case 49: param.Absorp[WeaponGroup.Cyber]+=value; break;
                case 20: case 50: param.Absorp[WeaponGroup.Energy] += value; param.Absorp[WeaponGroup.Physic] += value;
                    param.Absorp[WeaponGroup.Irregular] += value; param.Absorp[WeaponGroup.Cyber]+=value; break;
                case 21: case 51: param.Damage[WeaponGroup.Energy] +=value; break;
                case 22: case 52: param.Damage[WeaponGroup.Physic] +=value; break;
                case 23: case 53: param.Damage[WeaponGroup.Irregular] +=value; break;
                case 24: case 54: param.Damage[WeaponGroup.Cyber] +=value; break;
                case 25: case 55: param.Damage[WeaponGroup.Energy] +=value; param.Damage[WeaponGroup.Physic] +=value;
                    param.Damage[WeaponGroup.Irregular] +=value; param.Damage[WeaponGroup.Cyber]+=value; break;
                case 59: param.Immune[WeaponGroup.Energy] = true; break;
                case 60: param.Immune[WeaponGroup.Physic] = true; break;
                case 61: param.Immune[WeaponGroup.Irregular] = true; break;
                case 62: param.Immune[WeaponGroup.Cyber] = true; break;
                case 63: param.Immune[WeaponGroup.Energy] = true; param.Immune[WeaponGroup.Physic] = true;
                    param.Immune[WeaponGroup.Irregular] = true; param.Immune[WeaponGroup.Cyber]=true; break;
            }
        }
        /*public static EquipmentEffect GetEffect(byte pos)
        {
            switch (pos)
            {
                case 6:
                case 8:
                case 10: return EquipmentEffect.Restore;
                case 11: case 12: case 13: case 14: case 15: case 41: case 42: case 43: case 44 : case 45:case 16:
                case 17: case 18: case 19: case 20: case 46: case 47: case 48: case 49: case 50: case 59: case 60:
                case 61:
                case 62:
                case 63:
                case 5:
                case 7: return EquipmentEffect.Defense;
                default: return EquipmentEffect.Increase;
            }
        }
        */
        public static UIElement GetElement(byte pos)
        {
            
            switch (pos)
            {
                case 0: return Common.GetRectangle(30, Pictogram.AccuracyEnergy, Links.Interface("EnAcc"));
                case 1: return Common.GetRectangle(30, Pictogram.AccuracyPhysic, Links.Interface("PhAcc"));
                case 2: return Common.GetRectangle(30, Pictogram.AccuracyIrregular, Links.Interface("IrAcc"));
                case 3: return Common.GetRectangle(30, Pictogram.AccuracyCyber, Links.Interface("CyAcc"));
                case 4: return Common.GetRectangle(30, Pictogram.Accuracy, Links.Interface("AlAcc"));
                case 5: return Common.GetRectangle(30, Pictogram.HealthBrush, Links.Interface("Hull"));
                case 6: return Common.GetRectangle(30, Pictogram.RestoreHealth, Links.Interface("HullRestore"));
                case 7: return Common.GetRectangle(30, Pictogram.ShieldBrush, Links.Interface("Shield"));
                case 8: return Common.GetRectangle(30, Pictogram.RestoreShield, Links.Interface("ShieldRestore"));
                case 9: return Common.GetRectangle(30, Pictogram.EnergyBrush, Links.Interface("Energy"));
                case 10: return Common.GetRectangle(30, Pictogram.RestoreEnergy, Links.Interface("EnergyRestore"));
                case 11: return Common.GetRectangle(30, Pictogram.EvasionEnergy, Links.Interface("EnEva"));
                case 12: return Common.GetRectangle(30, Pictogram.EvasionPhysic, Links.Interface("PhEva"));
                case 13: return Common.GetRectangle(30, Pictogram.EvasionIrregular, Links.Interface("IrEva"));
                case 14: return Common.GetRectangle(30, Pictogram.EvasionCyber, Links.Interface("CyEva"));
                case 15: return Common.GetRectangle(30, Pictogram.Evasion, Links.Interface("AlEva"));
                case 16: return Common.GetRectangle(30, Pictogram.IgnoreEnergy, Links.Interface("EnIgn"));
                case 17: return Common.GetRectangle(30, Pictogram.IgnorePhysic, Links.Interface("PhIgn"));
                case 18: return Common.GetRectangle(30, Pictogram.IgnoreIrregular, Links.Interface("IrIgn"));
                case 19: return Common.GetRectangle(30, Pictogram.IgnoreCyber, Links.Interface("CyIgn"));
                case 20: return Common.GetRectangle(30, Pictogram.Ignore, Links.Interface("AlIgn"));
                case 21: return Common.GetRectangle(30, Pictogram.DamageEnergy, Links.Interface("EnDam"));
                case 22: return Common.GetRectangle(30, Pictogram.DamagePhysic, Links.Interface("PhDam"));
                case 23: return Common.GetRectangle(30, Pictogram.DamageIrregular, Links.Interface("IrDam"));
                case 24: return Common.GetRectangle(30, Pictogram.DamageCyber, Links.Interface("CyDam"));
                case 25: return Common.GetRectangle(30, Pictogram.Damage, Links.Interface("AlDam"));
                case 30: return Common.GetRectangle(30, Pictogram.HealthBrushGroup, Links.Interface("HullGroup"));
                case 31: return Common.GetRectangle(30, Pictogram.RestoreHealthGroup, Links.Interface("HullRestoreGroup"));
                case 32: return Common.GetRectangle(30, Pictogram.ShieldBrushGroup, Links.Interface("ShieldGroup"));
                case 33: return Common.GetRectangle(30, Pictogram.RestoreShieldGroup, Links.Interface("ShieldRestoreGroup"));
                case 34: return Common.GetRectangle(30, Pictogram.EnergyBrushGroup, Links.Interface("EnergyGroup"));
                case 35: return Common.GetRectangle(30, Pictogram.RestoreEnergyGroup, Links.Interface("EnergyRestoreGroup"));
                case 36: return Common.GetRectangle(30, Pictogram.AccuracyEnergyGroup, Links.Interface("EnAccGr"));
                case 37: return Common.GetRectangle(30, Pictogram.AccuracyPhysicGroup, Links.Interface("PhAccGr"));
                case 38: return Common.GetRectangle(30, Pictogram.AccuracyIrregularGroup, Links.Interface("IrAccGr"));
                case 39: return Common.GetRectangle(30, Pictogram.AccuracyCyberGroup, Links.Interface("CyAccGr"));
                case 40: return Common.GetRectangle(30, Pictogram.AccuracyGroup, Links.Interface("AlAccGr"));
                case 41: return Common.GetRectangle(30, Pictogram.EvasionEnergyGroup, Links.Interface("EnEvaGr"));
                case 42: return Common.GetRectangle(30, Pictogram.EvasionPhysicGroup, Links.Interface("PhEvaGr"));
                case 43: return Common.GetRectangle(30, Pictogram.EvasionIrregularGroup, Links.Interface("IrEvaGr"));
                case 44: return Common.GetRectangle(30, Pictogram.EvasionCyberGroup, Links.Interface("CyEvaGr"));
                case 45: return Common.GetRectangle(30, Pictogram.EvasionGroup, Links.Interface("AlEvaGr"));
                case 46: return Common.GetRectangle(30, Pictogram.IgnoreEnergyGroup, Links.Interface("EnIgnGr"));
                case 47: return Common.GetRectangle(30, Pictogram.IgnorePhysicGroup, Links.Interface("PhIgnGr"));
                case 48: return Common.GetRectangle(30, Pictogram.IgnoreIrregularGroup, Links.Interface("IrIgnGr"));
                case 49: return Common.GetRectangle(30, Pictogram.IgnoreCyberGroup, Links.Interface("CyIgnGr"));
                case 50: return Common.GetRectangle(30, Pictogram.IgnoreGroup, Links.Interface("AlIgnGr"));
                case 51: return Common.GetRectangle(30, Pictogram.DamageEnergyGroup, Links.Interface("EnDamGr"));
                case 52: return Common.GetRectangle(30, Pictogram.DamagePhysicGroup, Links.Interface("PhDamGr"));
                case 53: return Common.GetRectangle(30, Pictogram.DamageIrregularGroup, Links.Interface("IrDamGr"));
                case 54: return Common.GetRectangle(30, Pictogram.DamageCyberGroup, Links.Interface("CyDamGr"));
                case 55: return Common.GetRectangle(30, Pictogram.DamageGroup, Links.Interface("AlDamGr"));
                case 59: return Common.GetRectangle(30, Pictogram.ImmuneEnergy, Links.Interface("EnImm"));
                case 60: return Common.GetRectangle(30, Pictogram.ImmunePhysic, Links.Interface("PhImm"));
                case 61: return Common.GetRectangle(30, Pictogram.ImmuneIrregular, Links.Interface("IrImm"));
                case 62: return Common.GetRectangle(30, Pictogram.ImmuneCyber, Links.Interface("CyImm"));
                case 63: return Common.GetRectangle(30, Pictogram.Immune, Links.Interface("AlImm"));
            }
            return null;
        }
    }
}
