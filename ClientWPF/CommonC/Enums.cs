using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public enum EControllerStatus { Main, Login, Create }
    public enum EMainWindowStatus { Off, Galaxy, Sector, Stellar, Buildings, Market, Science, Fleets, Schematics }
    public enum EWeaponType { Laser, EMI, Plasma, Solar, Cannon, Gauss, Missle, AntiMatter, Psi, Dark, Warp, Time, Slicing, Radiation, Drone, Magnet};
    public enum WeaponGroup { Energy, Physic, Irregular, Cyber, Any }
    public enum Quest_Position
    {
        Q01_Basic_Info, Q02_Build_Bank, Q03_Build_Mine, Q04_Build_Chips, Q05_Build_Anti, Q06_Scheme_Info, Q07_Create_Scheme, Q08_Build_Radar,
        Q09_Build_Ships, Q10_Hire_Pilots, Q11_Create_Fleet, Q12_Move_Ships_to_Fleet, Q13_Send_To_Mission, Q14_Battle_End, Q15_Build_Factory, Q16_Learn_Science,
        Q17_Make_Trade, Q18_Conquer_Info, Q19_Alliance_Info, Q20_Chat_Info, Q21_All_Done
    }
    //public enum TargetMods { Clear, NewPlanetForLand };
    //public enum GlobalObjects { Building, Item, StarObject, Other }
    //public enum ItemObjects { ShipType, Generator, Shield, Computer, Engine, Weapon, Equipment }
    class Enums
    {
       
        public static EWeaponType GetWeaponType(int value)
        {
            if (value < 0 || value > 15) throw new Exception("Значение за границами перечисления");
            else return (EWeaponType)value;
        }
      
        public static WeaponGroup GetWeaponGroup(int value)
        {
            if (value < 0 || value > 4) throw new Exception("Значение за границами перечисления");
            else return (WeaponGroup)value;
        }
    }
    /*
    interface GameObject
    {
        GameObjectInfo GetObjectInfo();

    }
     */
    public enum EquipmentType
    {
        AccEn, AccPh, AccIr, AccCy, AccAl,
        Hull, HullRegen,
        Shield, ShieldRegen,
        Energy, EnergyRegen,
        EvaEn, EvaPh, EvaIr, EvaCy, EvaAl,
        IgnEn, IgnPh, IgnIr, IgnCy, IgnAl,
        DamEn, DamPh, DamIr, DamCy, DamAl,
        ImmEn, ImmPh, ImmIr, ImmCy, ImmAl
    }//38
}
