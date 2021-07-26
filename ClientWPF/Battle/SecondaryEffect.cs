using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public enum EffectTarget { Health, HealthRestore, Shield, ShieldRestore, Energy, EnergyRestore, Accuracy, Evasion, Damage, Ignore, Immune };
    public class SecondaryEffect
    {
        public byte Type;
        public int Value;
        public bool IsReal = false;
        public bool IsGroup = false;
        public System.Windows.Media.Brush Brush;
        public string Tip;
        public WeaponGroup Group;
        public SecondaryEffect(byte type, int value)
        {
            Type = type; Value = value; IsReal = true;
            if (Type > 29 && Type < 56) IsGroup = true;
            if (Type > 63) IsGroup = true;
            CalcTargetAndGroup();
        }
        void CalcTargetAndGroup()
        {
            switch (Type)
            {
                case 0: Brush = Pictogram.AccuracyEnergy; Tip = "EnAcc"; break;
                case 36: Brush = Pictogram.AccuracyEnergyGroup; Tip = "EnAccGr"; break;
                case 1: Brush = Pictogram.AccuracyPhysic; Tip = "PhAcc"; break;
                case 37: Brush = Pictogram.AccuracyPhysicGroup; Tip = "PhAccGr"; break;
                case 2: Brush = Pictogram.AccuracyIrregular; Tip = "IrAcc"; break;
                case 38: Brush = Pictogram.AccuracyIrregularGroup; Tip = "IrAccGr"; break;
                case 3: Brush = Pictogram.AccuracyCyber; Tip = "CyAcc"; break;
                case 39: Brush = Pictogram.AccuracyCyberGroup; Tip = "CyAccGr"; break;
                case 4: Brush = Pictogram.Accuracy; Tip = "AlAcc"; break;
                case 40: Brush = Pictogram.AccuracyGroup; Tip = "AlAccGr"; break;
                case 5: Brush = Pictogram.HealthBrush; Tip = "Hull"; break;
                case 30: Brush = Pictogram.HealthBrushGroup; Tip = "HullGroup"; break;
                case 6: Brush = Pictogram.RestoreHealth; Tip = "HullRestore"; break;
                case 31: Brush = Pictogram.RestoreHealthGroup; Tip = "HullRestoreGroup"; break;
                case 7: Brush = Pictogram.ShieldBrush; Tip = "Shield"; break;
                case 32: Brush = Pictogram.ShieldBrushGroup; Tip = "ShieldGroup"; break;
                case 8: Brush = Pictogram.RestoreShield; Tip = "ShieldRestore"; break;
                case 33: Brush = Pictogram.RestoreShieldGroup; Tip = "ShieldRestoreGroup"; break;
                case 9: Brush = Pictogram.EnergyBrush; Tip = "Energy"; break;
                case 34: Brush = Pictogram.EnergyBrushGroup; Tip = "EnergyGroup"; break;
                case 10: Brush = Pictogram.RestoreEnergy; Tip = "EnergyRestore"; break;
                case 35: Brush = Pictogram.RestoreEnergyGroup; Tip = "EnergyRestoreGroup"; break;
                case 11: Brush = Pictogram.EvasionEnergy; Tip = "EnEva"; break;
                case 41: Brush = Pictogram.EvasionEnergyGroup; Tip = "EnEvaGr"; break;
                case 12: Brush = Pictogram.EvasionPhysic; Tip = "PhEva"; break;
                case 42: Brush = Pictogram.EvasionPhysicGroup; Tip = "PhEvaGr"; break;
                case 13: Brush = Pictogram.EvasionIrregular; Tip = "IrEva"; break;
                case 43: Brush = Pictogram.EvasionIrregularGroup; Tip = "IrEvaGr"; break;
                case 14: Brush = Pictogram.EvasionCyber; Tip = "CyEva"; break;
                case 44: Brush = Pictogram.EvasionCyberGroup; Tip = "CyEvaGr"; break;
                case 15: Brush = Pictogram.Evasion; Tip = "AlEva"; break;
                case 45: Brush = Pictogram.EvasionGroup; Tip = "AlEvaGr"; break;
                case 16: Brush = Pictogram.IgnoreEnergy; Tip = "EnIgn"; break;
                case 46: Brush = Pictogram.IgnoreEnergyGroup; Tip = "EnIgnGr"; break;
                case 17: Brush = Pictogram.IgnorePhysic; Tip = "PhIgn"; break;
                case 47: Brush = Pictogram.IgnorePhysicGroup; Tip = "PhIgnGr"; break;
                case 18: Brush = Pictogram.IgnoreIrregular; Tip = "IrIgn"; break;
                case 48: Brush = Pictogram.IgnoreIrregularGroup; Tip = "IrIgnGr"; break;
                case 19: Brush = Pictogram.IgnoreCyber; Tip = "CyIgn"; break;
                case 49: Brush = Pictogram.IgnoreCyberGroup; Tip = "CyIgnGr"; break;
                case 20: Brush = Pictogram.Ignore; Tip = "AlIgn"; break;
                case 50: Brush = Pictogram.IgnoreGroup; Tip = "AlIgnGr"; break;
                case 21: Brush = Pictogram.DamageEnergy; Tip = "EnDam"; break;
                case 51: Brush = Pictogram.DamageEnergyGroup; Tip = "EnDamGr"; break;
                case 22: Brush = Pictogram.DamagePhysic; Tip = "PhDam"; break;
                case 52: Brush = Pictogram.DamagePhysicGroup; Tip = "PhDamGr"; break;
                case 23: Brush = Pictogram.DamageIrregular; Tip = "IrDam"; break;
                case 53: Brush = Pictogram.DamageIrregularGroup; Tip = "IrDamGr"; break;
                case 24: Brush = Pictogram.DamageCyber; Tip = "CyDam"; break;
                case 54: Brush = Pictogram.DamageCyberGroup; Tip = "CyDamGr"; break;
                case 25: Brush = Pictogram.Damage; Tip = "AlDam"; break;
                case 55: Brush = Pictogram.DamageGroup; Tip = "AlDamGr"; break;
                case 59: Brush = Pictogram.ImmuneEnergy; Tip = "EnImm"; break;
                case 60: Brush = Pictogram.ImmunePhysic; Tip = "PhImm"; break;
                case 61: Brush = Pictogram.ImmuneIrregular; Tip = "IrImm"; break;
                case 62: Brush = Pictogram.ImmuneCyber; Tip = "CyImm"; break;
                case 63: Brush = Pictogram.Immune; Tip = "AlImm"; break;
                case 64: Brush = Pictogram.ImmuneEnergyGroup; Tip = "EnImmGr"; break;
                case 65: Brush = Pictogram.ImmunePhysicGroup; Tip = "PhImmGr"; break;
                case 66: Brush = Pictogram.ImmuneIrregularGroup; Tip = "IrImmGr"; break;
                case 67: Brush = Pictogram.ImmuneCyberGroup; Tip = "CyImmGr"; break;
            }
        }
        public static SecondaryEffect GetEffect(byte type, byte level, double Talent)
        {
            int val = 0;
            switch (type)
            {
                case 0:
                case 1:
                case 2:
                case 3: val = 20 + level * 3; break; //Accuracy
                case 4: val = 10 + level * 2; break; //Accuracy total
                case 5: val = 50 + level * 10; break; //Health
                case 6: val = 20 + level * 3; break; //Health restrore
                case 7: val = 100 + level * 20; break;//Shield
                case 8: val = 25 + level * 15; break;//Shield restore
                case 9: val = 50 + level * 15; break;//Energy 
                case 10: val = 25 + level * 5; break;//Energy restore
                case 11:
                case 12:
                case 13:
                case 14: val = 20 + level * 3; break;//Evasion
                case 15: val = 10 + level * 2; break;//Evasion total
                case 16:
                case 17:
                case 18:
                case 19: val = 4 + level * 2; break;//Ignore
                case 20: val = 2 + level; break;//Ignore Total
                case 21:
                case 22:
                case 23:
                case 24: val = 20 + level * 3; break;//Damage
                case 25: val = 10 + level * 2; break;//Damage total
                case 30: val = 50 + level * 10; break; //Health group
                case 31: val = 20 + level * 3; break;//Health restore group
                case 32: val = 100 + level * 20; break;//Shield group
                case 33: val = 25 + level * 15; break;//Shield restore group
                case 34: val = 50 + level * 15; break;//Energy group
                case 35: val = 25 + level * 5; break;//Energy restore group
                case 36:
                case 37:
                case 38:
                case 39: val = 20 + level * 3; break;//Acuracy group
                case 40: val = 10 + level * 2; break;//Accuracy total group
                case 41:
                case 42:
                case 43:
                case 44: val = 20 + level * 3; break;//Evasion group
                case 45: val = 10 + level * 2; break;//Evasion total group
                case 46:
                case 47:
                case 48:
                case 49: val = 4 + level * 2; break;//Ignore group
                case 50: val = 2 + level; break;//Ignore group
                case 51:
                case 52:
                case 53:
                case 54: val = 20 + level * 3; break;//Damage group
                case 55: val = 10 + level * 2; break;//Damage total group
                case 59:
                case 60:
                case 61:
                case 62:
                case 63:
                case 64:
                case 65:
                case 66:
                case 67:
                    val = 100; break;//Immune
            }
            if (type <= 55)
                val = (int)(val * (1.0 + Talent / 3));
            return new SecondaryEffect(type, val);
        }
        public void RemoveBonusParams(BattleParams ship)
        {
            if (IsGroup == false) return;
            switch (Type)
            {
                case 36: ship.Accuracy.Bonus[WeaponGroup.Energy] -= Value; break;
                case 37: ship.Accuracy.Bonus[WeaponGroup.Physic] -= Value; break;
                case 38: ship.Accuracy.Bonus[WeaponGroup.Irregular] -= Value; break;
                case 39: ship.Accuracy.Bonus[WeaponGroup.Cyber] -= Value; break;
                case 40:
                    ship.Accuracy.Bonus[WeaponGroup.Energy] -= Value; ship.Accuracy.Bonus[WeaponGroup.Physic] -= Value;
                    ship.Accuracy.Bonus[WeaponGroup.Irregular] -= Value; ship.Accuracy.Bonus[WeaponGroup.Cyber] -= Value; break;
                case 30: ship.Health.RemoveBonusValue(Value); break;
                case 31: ship.Health.RemoveRestore(Value); break;
                case 32: ship.Shield.RemoveBonusValue(Value); break;
                case 33: ship.Shield.RemoveRestore(Value); break;
                case 34: ship.Energy.RemoveBonusValue(Value); break;
                case 35: ship.Energy.RemoveRestore(Value); break;
                case 41: ship.Evasion.Bonus[WeaponGroup.Energy] -= Value; break;
                case 42: ship.Evasion.Bonus[WeaponGroup.Physic] -= Value; break;
                case 43: ship.Evasion.Bonus[WeaponGroup.Irregular] -= Value; break;
                case 44: ship.Evasion.Bonus[WeaponGroup.Cyber] -= Value; break;
                case 45:
                    ship.Evasion.Bonus[WeaponGroup.Energy] -= Value; ship.Evasion.Bonus[WeaponGroup.Physic] -= Value;
                    ship.Evasion.Bonus[WeaponGroup.Irregular] -= Value; ship.Evasion.Bonus[WeaponGroup.Cyber] -= Value; break;
                case 46: ship.Ignore.Bonus[WeaponGroup.Energy] -= Value; break;
                case 47: ship.Ignore.Bonus[WeaponGroup.Physic] -= Value; break;
                case 48: ship.Ignore.Bonus[WeaponGroup.Irregular] -= Value; break;
                case 49: ship.Ignore.Bonus[WeaponGroup.Cyber] -= Value; break;
                case 50:
                    ship.Ignore.Bonus[WeaponGroup.Energy] -= Value; ship.Ignore.Bonus[WeaponGroup.Physic] -= Value;
                    ship.Ignore.Bonus[WeaponGroup.Irregular] -= Value; ship.Ignore.Bonus[WeaponGroup.Cyber] -= Value; break;
                case 51: ship.Increase.Bonus[WeaponGroup.Energy] -= Value; break;
                case 52: ship.Increase.Bonus[WeaponGroup.Physic] -= Value; break;
                case 53: ship.Increase.Bonus[WeaponGroup.Irregular] -= Value; break;
                case 54: ship.Increase.Bonus[WeaponGroup.Cyber] -= Value; break;
                case 55:
                    ship.Increase.Bonus[WeaponGroup.Energy] -= Value; ship.Increase.Bonus[WeaponGroup.Physic] -= Value;
                    ship.Increase.Bonus[WeaponGroup.Irregular] -= Value; ship.Increase.Bonus[WeaponGroup.Cyber] -= Value; break;
                case 59: ship.Immune.Bonus[WeaponGroup.Energy] -= Value; break;
                case 60: ship.Immune.Bonus[WeaponGroup.Physic] -= Value; break;
                case 61: ship.Immune.Bonus[WeaponGroup.Irregular] -= Value; break;
                case 62: ship.Immune.Bonus[WeaponGroup.Cyber] -= Value; break;
                case 63:
                    ship.Immune.Bonus[WeaponGroup.Energy] -= Value; ship.Immune.Bonus[WeaponGroup.Physic] -= Value;
                    ship.Immune.Bonus[WeaponGroup.Irregular] -= Value; ship.Immune.Bonus[WeaponGroup.Cyber] -= Value; break;
                case 64: ship.Immune.Bonus[WeaponGroup.Energy] -= Value; break;
                case 65: ship.Immune.Bonus[WeaponGroup.Physic] -= Value; break;
                case 66: ship.Immune.Bonus[WeaponGroup.Irregular] -= Value; break;
                case 67: ship.Immune.Bonus[WeaponGroup.Cyber] -= Value; break;
            }
        }
        public void AppendBonusParams(BattleParams ship)
        {
            if (IsGroup == false) return;
            switch (Type)
            {
                case 36: ship.Accuracy.Bonus[WeaponGroup.Energy] += Value; break;
                case 37: ship.Accuracy.Bonus[WeaponGroup.Physic] += Value; break;
                case 38: ship.Accuracy.Bonus[WeaponGroup.Irregular] += Value; break;
                case 39: ship.Accuracy.Bonus[WeaponGroup.Cyber] += Value; break;
                case 40:
                    ship.Accuracy.Bonus[WeaponGroup.Energy] += Value; ship.Accuracy.Bonus[WeaponGroup.Physic] += Value;
                    ship.Accuracy.Bonus[WeaponGroup.Irregular] += Value; ship.Accuracy.Bonus[WeaponGroup.Cyber] += Value; break;
                case 30: ship.Health.AddBonusValue(Value); break;
                case 31: ship.Health.AddRestore(Value); break;
                case 32: ship.Shield.AddBonusValue(Value); break;
                case 33: ship.Shield.AddRestore(Value); break;
                case 34: ship.Energy.AddBonusValue(Value); break;
                case 35: ship.Energy.AddRestore(Value); break;
                case 41: ship.Evasion.Bonus[WeaponGroup.Energy] += Value; break;
                case 42: ship.Evasion.Bonus[WeaponGroup.Physic] += Value; break;
                case 43: ship.Evasion.Bonus[WeaponGroup.Irregular] += Value; break;
                case 44: ship.Evasion.Bonus[WeaponGroup.Cyber] += Value; break;
                case 45:
                    ship.Evasion.Bonus[WeaponGroup.Energy] += Value; ship.Evasion.Bonus[WeaponGroup.Physic] += Value;
                    ship.Evasion.Bonus[WeaponGroup.Irregular] += Value; ship.Evasion.Bonus[WeaponGroup.Cyber] += Value; break;
                case 46: ship.Ignore.Bonus[WeaponGroup.Energy] += Value; break;
                case 47: ship.Ignore.Bonus[WeaponGroup.Physic] += Value; break;
                case 48: ship.Ignore.Bonus[WeaponGroup.Irregular] += Value; break;
                case 49: ship.Ignore.Bonus[WeaponGroup.Cyber] += Value; break;
                case 50:
                    ship.Ignore.Bonus[WeaponGroup.Energy] += Value; ship.Ignore.Bonus[WeaponGroup.Physic] += Value;
                    ship.Ignore.Bonus[WeaponGroup.Irregular] += Value; ship.Ignore.Bonus[WeaponGroup.Cyber] += Value; break;
                case 51: ship.Increase.Bonus[WeaponGroup.Energy] += Value; break;
                case 52: ship.Increase.Bonus[WeaponGroup.Physic] += Value; break;
                case 53: ship.Increase.Bonus[WeaponGroup.Irregular] += Value; break;
                case 54: ship.Increase.Bonus[WeaponGroup.Cyber] += Value; break;
                case 55:
                    ship.Increase.Bonus[WeaponGroup.Energy] += Value; ship.Increase.Bonus[WeaponGroup.Physic] += Value;
                    ship.Increase.Bonus[WeaponGroup.Irregular] += Value; ship.Increase.Bonus[WeaponGroup.Cyber] += Value; break;
                case 59: ship.Immune.Bonus[WeaponGroup.Energy] += Value; break;
                case 60: ship.Immune.Bonus[WeaponGroup.Physic] += Value; break;
                case 61: ship.Immune.Bonus[WeaponGroup.Irregular] += Value; break;
                case 62: ship.Immune.Bonus[WeaponGroup.Cyber] += Value; break;
                case 63:
                    ship.Immune.Bonus[WeaponGroup.Energy] += Value; ship.Immune.Bonus[WeaponGroup.Physic] += Value;
                    ship.Immune.Bonus[WeaponGroup.Irregular] += Value; ship.Immune.Bonus[WeaponGroup.Cyber] += Value; break;
                case 64: ship.Immune.Bonus[WeaponGroup.Energy] += Value; break;
                case 65: ship.Immune.Bonus[WeaponGroup.Physic] += Value; break;
                case 66: ship.Immune.Bonus[WeaponGroup.Irregular] += Value; break;
                case 67: ship.Immune.Bonus[WeaponGroup.Cyber] += Value; break;

            }
        }
        public void AppendBasicParams(ShipParamsClass schema)
        {
            if (IsReal == false) return;
            switch (Type)
            {
                case 0:
                case 36: schema.BasicWEnergyAccuracy += Value; break;
                case 1:
                case 37: schema.BasicWPhysicAccuracy += Value; break;
                case 2:
                case 38: schema.BasicWIrregularAccuracy += Value; break;
                case 3:
                case 39: schema.BasicWCyberAccuracy += Value; break;
                case 4:
                case 40:
                    schema.BasicWEnergyAccuracy += Value; schema.BasicWPhysicAccuracy += Value;
                    schema.BasicWIrregularAccuracy += Value; schema.BasicWCyberAccuracy += Value; break;
                case 5:
                case 30: schema.BasicHealth += Value; break;
                case 6:
                case 31: schema.BasicHealthRestore += Value; break;
                case 7:
                case 32: schema.BasicShieldCapacity += Value; break;
                case 8:
                case 33: schema.BasicShieldRecharge += Value; break;
                case 9:
                case 34: schema.BasicEnergyCapacity += Value; break;
                case 10:
                case 35: schema.BasicEnergyRecharge += Value; break;
                case 11:
                case 41: schema.BasicWEnergyEvasion += Value; break;
                case 12:
                case 42: schema.BasicWPhysicEvasion += Value; break;
                case 13:
                case 43: schema.BasicWIrregularEvasion += Value; break;
                case 14:
                case 44: schema.BasicWCyberEvasion += Value; break;
                case 15:
                case 45:
                    schema.BasicWEnergyEvasion += Value; schema.BasicWPhysicEvasion += Value;
                    schema.BasicWIrregularEvasion += Value; schema.BasicWCyberEvasion += Value; break;
                case 16:
                case 46: schema.BasicWEnergyIgnore += Value; break;
                case 17:
                case 47: schema.BasicWPhysicIgnore += Value; break;
                case 18:
                case 48: schema.BasicWIrregularIgnore += Value; break;
                case 19:
                case 49: schema.BasicWCyberIgnore += Value; break;
                case 20:
                case 50:
                    schema.BasicWEnergyIgnore += Value; schema.BasicWPhysicIgnore += Value;
                    schema.BasicWIrregularIgnore += Value; schema.BasicWCyberIgnore += Value; break;
                case 21:
                case 51: schema.BasicWEnergyIncrease += Value; break;
                case 22:
                case 52: schema.BasicWPhysicIncrease += Value; break;
                case 23:
                case 53: schema.BasicWIrregularIncrease += Value; break;
                case 24:
                case 54: schema.BasicWCyberIncrease += Value; break;
                case 25:
                case 55:
                    schema.BasicWEnergyIncrease += Value; schema.BasicWPhysicIncrease += Value;
                    schema.BasicWIrregularIncrease += Value; schema.BasicWCyberIncrease += Value; break;
                case 57: schema.Cargo += Value; break;
                case 58: schema.Colony += Value; break;
                case 59:
                case 64: schema.WEnergyImmune += Value; break;
                case 60:
                case 65: schema.WPhysicImmune += Value; break;
                case 61:
                case 66: schema.WIrregularImmune += Value; break;
                case 62:
                case 67: schema.WCyberImmune += Value; break;
                case 63:
                    schema.WEnergyImmune += Value; schema.WPhysicImmune += Value;
                    schema.WIrregularImmune += Value; schema.WCyberImmune += Value; break;
            }
        }
        public override string ToString()
        {
            if (Links.Lang == 0)
                switch (Type)
                {
                    case 0: return "Energy Accuracy " + Value;
                    case 1: return "Physical Acccuracy " + Value;
                    case 2: return "Irregular Accuracy " + Value;
                    case 3: return "Cyber Accuracy " + Value;
                    case 4: return "All Accuracy " + Value;
                    case 5: return "Health " + Value;
                    case 6: return "Health Restore " + Value;
                    case 7: return "Shield " + Value;
                    case 8: return "Shield Restore " + Value;
                    case 9: return "Energy " + Value;
                    case 10: return "Energy Restore" + Value;
                    case 11: return "Energy Evasion " + Value;
                    case 12: return "Physical Evasion " + Value;
                    case 13: return "Irregular Evasion " + Value;
                    case 14: return "Cyber Evasion " + Value;
                    case 15: return "All Evasion " + Value;
                    case 16: return "Energy Ignore " + Value;
                    case 17: return "Physical Ignore " + Value;
                    case 18: return "Irregular Ignore " + Value;
                    case 19: return "Cyber Ignore " + Value;
                    case 20: return "All Ignore " + Value;
                    case 21: return "Energy Damage " + Value;
                    case 22: return "Physical Damage " + Value;
                    case 23: return "Irregular Damage " + Value;
                    case 24: return "Cyber Damage " + Value;
                    case 25: return "All Damage " + Value;
                    case 30: return "Health Group " + Value;
                    case 31: return "Health Restore Group " + Value;
                    case 32: return "Shield Group " + Value;
                    case 33: return "Shield Restore Group " + Value;
                    case 34: return "Energy Group " + Value;
                    case 35: return "Energy Restore Group " + Value;
                    case 36: return "Group Energy Accucary " + Value;
                    case 37: return "Group Physical Accuracy " + Value;
                    case 38: return "Group Irregular Accuracy " + Value;
                    case 39: return "Group Cyber Accuracy " + Value;
                    case 40: return "Group All Accuracy " + Value;
                    case 41: return "Group Energy Evasion " + Value;
                    case 42: return "Group Physical Evasion " + Value;
                    case 43: return "Group Irregular Evasion " + Value;
                    case 44: return "Group Cyber Evasion " + Value;
                    case 45: return "Group All Evasion " + Value;
                    case 46: return "Group Energy Ignore " + Value;
                    case 47: return "Group Physical Ignore " + Value;
                    case 48: return "Group Irregular Ignore " + Value;
                    case 49: return "Group Cyber Ignore " + Value;
                    case 50: return "Group All Ignore " + Value;
                    case 51: return "Group Energy Damage " + Value;
                    case 52: return "Group Physical Damage " + Value;
                    case 53: return "Group Irregular Damage " + Value;
                    case 54: return "Group Cyber Damage " + Value;
                    case 55: return "Group All Damage " + Value;
                    case 59: return "Energy Immune " + Value;
                    case 60: return "Physical Immune " + Value;
                    case 61: return "Irregular Immune " + Value;
                    case 62: return "Cyber Immune " + Value;
                    case 63: return "All Immune " + Value;
                    case 64: return "Group Energy Immune " + Value;
                    case 65: return "Group Physic Immune " + Value;
                    case 66: return "Group Irregular Immune " + Value;
                    case 67: return "Group Cyber Immune " + Value;
                    default: return "ERROR SECONDARY EFFECT";
                }

            else
                switch (Type)
                {
                    case 0: return "Меткость энергетическая " + Value;
                    case 1: return "Меткость  физическая " + Value;
                    case 2: return "Меткость аномальная " + Value;
                    case 3: return "Меткость кибернетическая " + Value;
                    case 4: return "Метколсть универсальная " + Value;
                    case 5: return "Прочность корпуса " + Value;
                    case 6: return "Ремонт корпуса " + Value;
                    case 7: return "Прочность щита " + Value;
                    case 8: return "Восстановление щита " + Value;
                    case 9: return "Запас энергии " + Value;
                    case 10: return "Генерация энергии " + Value;
                    case 11: return "Уклонение энергетическое " + Value;
                    case 12: return "Уклонение физическое " + Value;
                    case 13: return "Уклонение аномальное " + Value;
                    case 14: return "Уклонение кибернетическое " + Value;
                    case 15: return "Общее уклонение " + Value;
                    case 16: return "Броня энергетическая " + Value;
                    case 17: return "Броня физическая " + Value;
                    case 18: return "Броня аномальное " + Value;
                    case 19: return "Броня кибернетическая " + Value;
                    case 20: return "Броня общая " + Value;
                    case 21: return "Усиление энергетического урона " + Value;
                    case 22: return "Усиление физического урона " + Value;
                    case 23: return "Усиление аномального урона " + Value;
                    case 24: return "Усиление кибернетического урона " + Value;
                    case 25: return "Усиление общего урона " + Value;
                    case 30: return "Групповая прочность корпуса " + Value;
                    case 31: return "Групповой ремонт корпуса " + Value;
                    case 32: return "Групповая прочность щита " + Value;
                    case 33: return "Групповое восстановление щита " + Value;
                    case 34: return "Групповая энергия генератора " + Value;
                    case 35: return "Групповая генерация энергии " + Value;
                    case 36: return "Групповая меткость энергетическая " + Value;
                    case 37: return "Групповая меткость физическая " + Value;
                    case 38: return "Групповая меткость аномальная " + Value;
                    case 39: return "Групповая меткость кибернетическая " + Value;
                    case 40: return "Групповая меткость общая " + Value;
                    case 41: return "Групповое уклонение энергетическое " + Value;
                    case 42: return "Групповое уклонение физическое " + Value;
                    case 43: return "Групповое уклонение аномальное " + Value;
                    case 44: return "Групповое уклонение кибернетическое " + Value;
                    case 45: return "Групповое уклонение общее " + Value;
                    case 46: return "Групповая броня энергетическая " + Value;
                    case 47: return "Групповая броня физическая " + Value;
                    case 48: return "Групповая броня аномальная " + Value;
                    case 49: return "Групповая броня кибернетическая " + Value;
                    case 50: return "Групповая броня общая" + Value;
                    case 51: return "Групповое усиление урона энергетическое " + Value;
                    case 52: return "Групповое усиление урона физическое " + Value;
                    case 53: return "Групповое усиление урона аномальное " + Value;
                    case 54: return "Групповое усиление урона кибернетическое " + Value;
                    case 55: return "Групповое усиление урона общее " + Value;
                    case 59: return "Иммунитет энергетический " + Value;
                    case 60: return "Иммунитет физический " + Value;
                    case 61: return "Иммунитет аномальный " + Value;
                    case 62: return "Иммунитет кибернетический " + Value;
                    case 63: return "Иммунитет общий " + Value;
                    case 64: return "Групповой иммунитет энергетический " + Value;
                    case 65: return "Групповой иммунитет физический " + Value;
                    case 66: return "Групповой иммунитет аномальный " + Value;
                    case 67: return "Групповой иммунитет кибернетический " + Value;
                    default: return "ERROR SECONDARY EFFECT";
                }
        }
        public string Title()
        {
            string s = "";
            if (Links.Lang == 0)
                switch (Type)
                {
                    case 0: s = "Energy Accuracy "; Group = WeaponGroup.Energy; break;
                    case 1: s = "Physical Acccuracy "; Group = WeaponGroup.Physic; break;
                    case 2: s = "Irregular Accuracy "; Group = WeaponGroup.Irregular; break;
                    case 3: s = "Cyber Accuracy "; Group = WeaponGroup.Cyber; break;
                    case 4: s = "All Accuracy "; Group = WeaponGroup.Any; break;
                    case 5: s = "Health "; Group = WeaponGroup.Any; break;
                    case 6: s = "Health Restore "; Group = WeaponGroup.Any; break;
                    case 7: s = "Shield "; Group = WeaponGroup.Any; break;
                    case 8: s = "Shield Restore "; Group = WeaponGroup.Any; break;
                    case 9: s = "Energy "; Group = WeaponGroup.Any; break;
                    case 10: s = "Energy Restore"; Group = WeaponGroup.Energy; break;
                    case 11: s = "Energy Evasion "; Group = WeaponGroup.Energy; break;
                    case 12: s = "Physical Evasion "; Group = WeaponGroup.Physic; break;
                    case 13: s = "Irregular Evasion "; Group = WeaponGroup.Irregular; break;
                    case 14: s = "Cyber Evasion "; Group = WeaponGroup.Cyber; break;
                    case 15: s = "All Evasion "; Group = WeaponGroup.Any; break;
                    case 16: s = "Energy Ignore "; Group = WeaponGroup.Energy; break;
                    case 17: s = "Physical Ignore "; Group = WeaponGroup.Physic; break;
                    case 18: s = "Irregular Ignore "; Group = WeaponGroup.Irregular; break;
                    case 19: s = "Cyber Ignore "; Group = WeaponGroup.Cyber; break;
                    case 20: s = "All Ignore "; Group = WeaponGroup.Any; break;
                    case 21: s = "Energy Damage "; Group = WeaponGroup.Energy; break;
                    case 22: s = "Physical Damage "; Group = WeaponGroup.Physic; break;
                    case 23: s = "Irregular Damage "; Group = WeaponGroup.Irregular; break;
                    case 24: s = "Cyber Damage "; Group = WeaponGroup.Cyber; break;
                    case 25: s = "All Damage "; Group = WeaponGroup.Any; break;
                    case 30: s = "Health Group "; Group = WeaponGroup.Any; break;
                    case 31: s = "Health Restore Group "; Group = WeaponGroup.Any; break;
                    case 32: s = "Shield Group "; Group = WeaponGroup.Any; break;
                    case 33: s = "Shield Restore Group "; Group = WeaponGroup.Any; break;
                    case 34: s = "Energy Group "; Group = WeaponGroup.Any; break;
                    case 35: s = "Energy Restore Group "; Group = WeaponGroup.Any; break;
                    case 36: s = "Group Energy Accucary "; Group = WeaponGroup.Energy; break;
                    case 37: s = "Group Physical Accuracy "; Group = WeaponGroup.Physic; break;
                    case 38: s = "Group Irregular Accuracy "; Group = WeaponGroup.Irregular; break;
                    case 39: s = "Group Cyber Accuracy "; Group = WeaponGroup.Cyber; break;
                    case 40: s = "Group All Accuracy "; Group = WeaponGroup.Any; break;
                    case 41: s = "Group Energy Evasion "; Group = WeaponGroup.Energy; break;
                    case 42: s = "Group Physical Evasion "; Group = WeaponGroup.Physic; break;
                    case 43: s = "Group Irregular Evasion "; Group = WeaponGroup.Irregular; break;
                    case 44: s = "Group Cyber Evasion "; Group = WeaponGroup.Cyber; break;
                    case 45: s = "Group All Evasion "; Group = WeaponGroup.Any; break;
                    case 46: s = "Group Energy Ignore "; Group = WeaponGroup.Energy; break;
                    case 47: s = "Group Physical Ignore "; Group = WeaponGroup.Physic; break;
                    case 48: s = "Group Irregular Ignore "; Group = WeaponGroup.Irregular; break;
                    case 49: s = "Group Cyber Ignore "; Group = WeaponGroup.Cyber; break;
                    case 50: s = "Group All Ignore "; Group = WeaponGroup.Any; break;
                    case 51: s = "Group Energy Damage "; Group = WeaponGroup.Energy; break;
                    case 52: s = "Group Physical Damage "; Group = WeaponGroup.Physic; break;
                    case 53: s = "Group Irregular Damage "; Group = WeaponGroup.Irregular; break;
                    case 54: s = "Group Cyber Damage "; Group = WeaponGroup.Cyber; break;
                    case 55: s = "Group All Damage "; Group = WeaponGroup.Any; break;
                    case 59: s = "Energy Immune "; Group = WeaponGroup.Energy; break;
                    case 60: s = "Physical Immune "; Group = WeaponGroup.Physic; break;
                    case 61: s = "Irregular Immune "; Group = WeaponGroup.Irregular; break;
                    case 62: s = "Cyber Immune "; Group = WeaponGroup.Cyber; break;
                    case 63: s = "All Immune "; Group = WeaponGroup.Any; break;
                    case 64: s = "Group Energy Immune "; Group = WeaponGroup.Energy; break;
                    case 65: s = "Group Physic Immune "; Group = WeaponGroup.Physic; break;
                    case 66: s = "Group Irregular Immune "; Group = WeaponGroup.Irregular; break;
                    case 67: s = "Group Cyber Immune "; Group = WeaponGroup.Cyber; break;
                    default: s = "ERROR SECONDARY EFFECT"; Group = WeaponGroup.Any; break;
                }

            else
                switch (Type)
                {
                    case 0: s = "Меткость энергетическая "; Group = WeaponGroup.Energy; break;
                    case 1: s = "Меткость  физическая "; Group = WeaponGroup.Physic; break;
                    case 2: s = "Меткость аномальная "; Group = WeaponGroup.Irregular; break;
                    case 3: s = "Меткость кибернетическая "; Group = WeaponGroup.Cyber; break;
                    case 4: s = "Метколсть универсальная "; Group = WeaponGroup.Any; break;
                    case 5: s = "Прочность корпуса "; Group = WeaponGroup.Any; break;
                    case 6: s = "Ремонт корпуса "; Group = WeaponGroup.Any; break;
                    case 7: s = "Прочность щита "; Group = WeaponGroup.Any; break;
                    case 8: s = "Восстановление щита "; Group = WeaponGroup.Any; break;
                    case 9: s = "Запас энергии "; Group = WeaponGroup.Any; break;
                    case 10: s = "Генерация энергии "; Group = WeaponGroup.Any; break;
                    case 11: s = "Уклонение энергетическое "; Group = WeaponGroup.Energy; break;
                    case 12: s = "Уклонение физическое "; Group = WeaponGroup.Physic; break;
                    case 13: s = "Уклонение аномальное "; Group = WeaponGroup.Irregular; break;
                    case 14: s = "Уклонение кибернетическое "; Group = WeaponGroup.Cyber; break;
                    case 15: s = "Общее уклонение "; Group = WeaponGroup.Any; break;
                    case 16: s = "Броня энергетическая "; Group = WeaponGroup.Energy; break;
                    case 17: s = "Броня физическая "; Group = WeaponGroup.Physic; break;
                    case 18: s = "Броня аномальное "; Group = WeaponGroup.Irregular; break;
                    case 19: s = "Броня кибернетическая "; Group = WeaponGroup.Cyber; break;
                    case 20: s = "Броня общая "; Group = WeaponGroup.Any; break;
                    case 21: s = "Усиление энергетического урона "; Group = WeaponGroup.Energy; break;
                    case 22: s = "Усиление физического урона "; Group = WeaponGroup.Physic; break;
                    case 23: s = "Усиление аномального урона "; Group = WeaponGroup.Irregular; break;
                    case 24: s = "Усиление кибернетического урона "; Group = WeaponGroup.Cyber; break;
                    case 25: s = "Усиление общего урона "; Group = WeaponGroup.Any; break;
                    case 30: s = "Групповая прочность корпуса "; Group = WeaponGroup.Any; break;
                    case 31: s = "Групповой ремонт корпуса "; Group = WeaponGroup.Any; break;
                    case 32: s = "Групповая прочность щита "; Group = WeaponGroup.Any; break;
                    case 33: s = "Групповое восстановление щита "; Group = WeaponGroup.Any; break;
                    case 34: s = "Групповая энергия генератора "; Group = WeaponGroup.Any; break;
                    case 35: s = "Групповая генерация энергии "; Group = WeaponGroup.Any; break;
                    case 36: s = "Групповая меткость энергетическая "; Group = WeaponGroup.Energy; break;
                    case 37: s = "Групповая меткость физическая "; Group = WeaponGroup.Physic; break;
                    case 38: s = "Групповая меткость аномальная "; Group = WeaponGroup.Irregular; break;
                    case 39: s = "Групповая меткость кибернетическая "; Group = WeaponGroup.Cyber; break;
                    case 40: s = "Групповая меткость общая "; Group = WeaponGroup.Any; break;
                    case 41: s = "Групповое уклонение энергетическое "; Group = WeaponGroup.Energy; break;
                    case 42: s = "Групповое уклонение физическое "; Group = WeaponGroup.Physic; break;
                    case 43: s = "Групповое уклонение аномальное "; Group = WeaponGroup.Irregular; break;
                    case 44: s = "Групповое уклонение кибернетическое "; Group = WeaponGroup.Cyber; break;
                    case 45: s = "Групповое уклонение общее "; Group = WeaponGroup.Any; break;
                    case 46: s = "Групповая броня энергетическая "; Group = WeaponGroup.Energy; break;
                    case 47: s = "Групповая броня физическая "; Group = WeaponGroup.Physic; break;
                    case 48: s = "Групповая броня аномальная "; Group = WeaponGroup.Irregular; break;
                    case 49: s = "Групповая броня кибернетическая "; Group = WeaponGroup.Cyber; break;
                    case 50: s = "Групповая броня общая"; Group = WeaponGroup.Any; break;
                    case 51: s = "Групповое усиление урона энергетическое "; Group = WeaponGroup.Energy; break;
                    case 52: s = "Групповое усиление урона физическое "; Group = WeaponGroup.Physic; break;
                    case 53: s = "Групповое усиление урона аномальное "; Group = WeaponGroup.Irregular; break;
                    case 54: s = "Групповое усиление урона кибернетическое "; Group = WeaponGroup.Cyber; break;
                    case 55: s = "Групповое усиление урона общее "; Group = WeaponGroup.Any; break;
                    case 59: s = "Иммунитет энергетический "; Group = WeaponGroup.Energy; break;
                    case 60: s = "Иммунитет физический "; Group = WeaponGroup.Physic; break;
                    case 61: s = "Иммунитет аномальный "; Group = WeaponGroup.Irregular; break;
                    case 62: s = "Иммунитет кибернетический "; Group = WeaponGroup.Cyber; break;
                    case 63: s = "Иммунитет общий "; Group = WeaponGroup.Any; break;
                    case 64: s = "Групповой иммунитет энергетический "; Group = WeaponGroup.Energy; break;
                    case 65: s = "Групповой иммунитет физический "; Group = WeaponGroup.Physic; break;
                    case 66: s = "Групповой иммунитет аномальный "; Group = WeaponGroup.Irregular; break;
                    case 67: s = "Групповой иммунитет кибернетический "; Group = WeaponGroup.Cyber; break;
                    default: s = "ERROR SECONDARY EFFECT"; Group = WeaponGroup.Any; break;
                }
            return s;
        }
    }
}
