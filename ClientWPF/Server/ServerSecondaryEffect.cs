using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class ServerSecondaryEffect
    {
        public byte Type;
        public int Value;
        public bool IsReal = false;
        public bool IsGroup = false;
        public ServerSecondaryEffect(byte type, int value)
        {
            Type = type; Value = value; IsReal = true;
            if (Type > 29 && Type < 56) IsGroup = true;
            if (Type > 63) IsGroup = true;
        }
        public override string ToString()
        {
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
        }
        public ServerSecondaryEffect(byte type, byte level, double Talent)
        {
            Type = type;
            IsReal = true;
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
                case 30: val = 50 + level * 10; IsGroup = true; break; //Health group
                case 31: val = 20 + level * 3; IsGroup = true; break;//Health restore group
                case 32: val = 100 + level * 20; IsGroup = true; break;//Shield group
                case 33: val = 25 + level * 15; IsGroup = true; break;//Shield restore group
                case 34: val = 50 + level * 15; IsGroup = true; break;//Energy group
                case 35: val = 25 + level * 5; IsGroup = true; break;//Energy restore group
                case 36:
                case 37:
                case 38:
                case 39: val = 20 + level * 3; IsGroup = true; break;//Acuracy group
                case 40: val = 10 + level * 2; IsGroup = true; break;//Accuracy total group
                case 41:
                case 42:
                case 43:
                case 44: val = 20 + level * 3; IsGroup = true; break;//Evasion group
                case 45: val = 10 + level * 2; IsGroup = true; break;//Evasion total group
                case 46:
                case 47:
                case 48:
                case 49: val = 4 + level * 2; IsGroup = true; break;//Ignore group
                case 50: val = 2 + level; IsGroup = true; break;//Ignore total group
                case 51:
                case 52:
                case 53:
                case 54: val = 20 + level * 3; IsGroup = true; break;//Damage group
                case 55: val = 10 + level; IsGroup = true; break;//Damage total group
                case 59:
                case 60:
                case 61:
                case 62:
                case 63:
                case 64:
                case 65:
                case 66:
                case 67: val = 100; break;//Immune
            }
            if (type <= 55)
                Value = (int)(val * (1.0 + Talent / 3));
            else
                Value = val;
        }
        public void RemoveBonusParams(ServerBattleParams ship)
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
                case 64: ship.Immune.Bonus[WeaponGroup.Energy]-=Value; break;
                case 65: ship.Immune.Bonus[WeaponGroup.Physic]-=Value; break;
                case 66: ship.Immune.Bonus[WeaponGroup.Irregular]-=Value; break;
                case 67: ship.Immune.Bonus[WeaponGroup.Cyber]-=Value; break;
                    //case 63: ship.Status.RemoveBonusImmune(WeaponGroup.Energy); ship.Status.RemoveBonusImmune(WeaponGroup.Physic);
                    //    ship.Status.RemoveBonusImmune(WeaponGroup.Irregular); ship.Status.RemoveBonusImmune(WeaponGroup.Cyber); break;

            }
        }
        public void AppendBonusParams(ServerBattleParams ship)
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
                case 64: ship.Immune.Bonus[WeaponGroup.Energy]+=Value; break;
                case 65: ship.Immune.Bonus[WeaponGroup.Physic]+=Value; break;
                case 66: ship.Immune.Bonus[WeaponGroup.Irregular]+=Value; break;
                case 67: ship.Immune.Bonus[WeaponGroup.Cyber]+=Value; break;
                    //case 63: ship.Status.AddBonusImmune(WeaponGroup.Energy); ship.Status.AddBonusImmune(WeaponGroup.Physic);
                    //    ship.Status.AddBonusImmune(WeaponGroup.Irregular); ship.Status.AddBonusImmune(WeaponGroup.Cyber); break;

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
                case 59:
                case 64:
                    schema.WEnergyImmune +=Value; break;
                case 60:
                case 65:
                    schema.WPhysicImmune +=Value; break;
                case 61:
                case 66:
                    schema.WIrregularImmune +=Value; break;
                case 62:
                case 67:
                    schema.WCyberImmune +=Value; break;
                case 63:
                    schema.WEnergyImmune +=Value; schema.WPhysicImmune +=Value;
                    schema.WIrregularImmune +=Value; schema.WCyberImmune +=Value; break;
            }
        }

    }
}
