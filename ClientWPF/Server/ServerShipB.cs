using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class ServerShipB
    {
        public long ShipID=-1;
        public byte BattleID;
        public byte HelpID;
        public byte[] Array;
        public Schema Schema;
        public GSPilot Pilot;
        public byte StartHealth;
        public byte Hex { get; private set; }
        public int Angle;
        public ServerBattleParams Params;
        public ServerBattleWeapon[] Weapons = new ServerBattleWeapon[3];
        public List<ServerSecondaryEffect> GroupEffects;
        public ShipStates States;
        //public ShipSide SSide;
        //public bool IsPortal = false;
        internal ServerBattle Battle;
        public int Rating = 0;
        public SortedSet<Hex>[] JumpHexes;
        public byte[] NameCrypt;
        public ServerShipB(ServerShip ship, byte battleID, ShipSide side, ServerBattle battle, byte hex)
        {
            ShipID = ship.ID;
            BattleID = battleID;
            Battle = battle;
            Schema = ship.Schema;
            NameCrypt = BitConverter.GetBytes(Schema.CryptName);
            Pilot = ship.Pilot;
            StartHealth = ship.Health;
            GroupEffects = new List<ServerSecondaryEffect>();
            Params = new ServerBattleParams();
            Params.CalculateBase(this);
            if (Schema.GetWeapon(0) != null) Weapons[0] = new ServerBattleWeapon(this, 0);
            if (Schema.GetWeapon(1) != null) Weapons[1] = new ServerBattleWeapon(this, 1);
            if (Schema.GetWeapon(2) != null) Weapons[2] = new ServerBattleWeapon(this, 2);
            List<byte> list = new List<byte>();
            list.Add((byte)ShipBArrayType.Standard);
            list.Add(BattleID);
            list.AddRange(ship.Schema.GetCrypt());
            list.Add(ship.Health);
            if (ship.Pilot != null) list.AddRange(ship.Pilot.GetArray()); else list.AddRange(GSPilot.GetNullPilot());
            list.AddRange(ship.Model);
            list.Add(hex);
            Array = list.ToArray();
            States = ShipStates.GetTypicalShip(side);
            Rating = CalcRating();
            SetHex(hex);
        }
        public void SetHex(byte hex)
        {
            Hex = hex;
            if (States.IsPortal)
            {
                if (Hex > Battle.Field.MaxHex) { JumpHexes = null; return; }
                JumpHexes = new SortedSet<Hex>[3];
                JumpHexes[0] = new SortedSet<Hex>(); JumpHexes[1] = new SortedSet<Hex>(); JumpHexes[2] = new SortedSet<Hex>();
                for (int i=0;i<3;i++)
                {
                    Hex portalhex = Battle.Field.Hexes[Hex];
                    foreach (Hex nearhex in portalhex.NearHexes)
                        JumpHexes[i].Add(nearhex);
                    for (int j=0;j< i;j++)
                    {
                        SortedSet<Hex> step2 = new SortedSet<Hex>();
                        foreach (Hex step2hex in JumpHexes[i])
                        {
                            step2.Add(step2hex);
                            foreach (Hex nearhex in step2hex.NearHexes)
                                step2.Add(nearhex);
                        }
                        JumpHexes[i] = step2;
                    }
                }
            }
        }
        
        public ServerShipB(ServerBattleParams battleparams, byte battleID, ShipStates states, ServerBattle battle, ServerBattleWeapon[] weapons, 
            List<ServerSecondaryEffect> groupeffects, byte[] array, byte hex)
        {
            ShipID = -1;
            BattleID = battleID;
            Battle = battle;
            if (groupeffects == null)
                GroupEffects = new List<ServerSecondaryEffect>();
            else
                GroupEffects = groupeffects;
            Params = battleparams;
            States = states;
            SetHex(hex);
            if (weapons != null) Weapons = weapons;
            Array = array;
            StartHealth = 100;
        }
        /// <summary> Метод создаёт корабль и его массив на основе полной задачи боевых характеристик </summary>
        public static ServerShipB GetStoryShip(byte battleID, ShipSide side, ServerBattle battle, ShipBattleParam param)
        {
            ShipStates states;
            switch (param.Type)
            {
                case StoryShipType.Portal: return ServerShipB.GetPortalShip(battleID, param.Health[0], side, battle, param.Hex, param.Name);
                case StoryShipType.StandardShip: states = ShipStates.GetTypicalShip(side); break;
                case StoryShipType.CargoShip: states = ShipStates.GetCargoShip(side); break;
                case StoryShipType.BigShip: states = ShipStates.GetBigShip(side); break;
                case StoryShipType.LargeBuilding: states = ShipStates.GetBigBuilding(side); break;
                default: throw new Exception();
            }
            ServerBattleParams Params = new ServerBattleParams();
            Params.CalculateStoryShip(param);
            List<byte> array = new List<byte>();
            array.Add((byte)ShipBArrayType.Array);
            array.Add((byte)param.Type);
            array.AddRange(Params.Health.GetStartArray());
            array.AddRange(Params.Shield.GetStartArray());
            array.AddRange(Params.Energy.GetStartArray());
            array.AddRange(Params.Evasion.GetStartArray());
            array.AddRange(Params.Ignore.GetStartArray());
            array.AddRange(Params.Accuracy.GetStartArray());
            array.AddRange(Params.Increase.GetStartArray());
            array.AddRange(Params.Immune.GetStartArray());
            array.Add(Params.ShieldProtection);
            array.Add(Params.TimeToEnter);
            array.Add(Params.JumpDistance);
            array.AddRange(BitConverter.GetBytes((ushort)Params.HexMoveCost.Basic));
            array.Add(param.Hex);
            array.AddRange(param.Image);
            array.AddRange(param.Name);
            ServerBattleWeapon[] weapons = new ServerBattleWeapon[3];
            if (param.Weapons[0] != null) weapons[0] = new ServerBattleWeapon(param.Weapons[0]);
            if (param.Weapons[1] != null) weapons[1] = new ServerBattleWeapon(param.Weapons[1]);
            if (param.Weapons[2] != null) weapons[2] = new ServerBattleWeapon(param.Weapons[2]);
            array.AddRange(ServerBattleWeapon.CreateArray(weapons));
            array.Add((byte)param.GroupEffects.Count);
            List< ServerSecondaryEffect >groupeffects=new List<ServerSecondaryEffect>();
            foreach (int[] val in param.GroupEffects)
            {
                groupeffects.Add(new ServerSecondaryEffect((byte)val[0], val[1]));
                array.Add((byte)val[0]); array.AddRange(BitConverter.GetBytes(val[1]));
            }
            ServerShipB ship = new ServerShipB(Params, battleID, states, battle, weapons, groupeffects, array.ToArray(), param.Hex);
            if (weapons[0] != null) weapons[0].Ship = ship;
            if (weapons[1]!= null) weapons[1].Ship = ship;
            if (weapons[2]!= null) weapons[2].Ship = ship;
            return ship;
            
        }
        /*public static ServerShipB GetCargoShip(byte battleID, ShipSide side, ServerBattle battle, byte level, byte hex, byte model, byte[] cryptname)
        {
            ServerBattleParams Params = new ServerBattleParams();
            Params.CalculateCargoShip(level);
            List<byte> array = new List<byte>();
            array.Add((byte)ShipBArrayType.Cargo);
            array.AddRange(Params.Health.GetStartArray());
            array.AddRange(Params.Shield.GetStartArray());
            array.AddRange(Params.Energy.GetStartArray());
            array.AddRange(Params.Evasion.GetStartArray());
            array.AddRange(Params.Ignore.GetStartArray());
            array.AddRange(Params.Accuracy.GetStartArray());
            array.AddRange(Params.Increase.GetStartArray());
            array.Add(Params.ShieldProtection);
            array.AddRange(BitConverter.GetBytes((ushort)Params.HexMoveCost.Basic));
            //byte hex = (byte)(battle.Field.MaxHex / 2);
            array.Add(hex);
            array.Add(model);
            array.AddRange(cryptname);
            ServerShipB cargoship = new ServerShipB(Params, battleID, ShipStates.GetCargoShip(side), battle, null, null, array.ToArray(), hex);
            return cargoship;
        }*/
        /*public static ServerShipB GetBigShip(byte battleID, ShipSide side, ServerBattle battle, byte level, EnemyType enemy, byte hex, byte model, byte[] cryptname)
        {
            ServerBattleParams Params = new ServerBattleParams();
            Params.CalculateBigShip(level);
            List<byte> array = new List<byte>();
            array.Add((byte)ShipBArrayType.BigShip);
            array.AddRange(Params.Health.GetStartArray());
            array.AddRange(Params.Shield.GetStartArray());
            array.AddRange(Params.Energy.GetStartArray());
            array.AddRange(Params.Evasion.GetStartArray());
            array.AddRange(Params.Ignore.GetStartArray());
            array.AddRange(Params.Accuracy.GetStartArray());
            array.AddRange(Params.Increase.GetStartArray());
            array.Add(Params.ShieldProtection);
            array.AddRange(BitConverter.GetBytes((ushort)Params.HexMoveCost.Basic));
            //byte hex = (byte)(battle.Field.MaxHex / 2);
            array.Add(hex);
            array.Add(model);
            array.AddRange(cryptname);
            ushort[] guns = GetBigShipWeapon(level, enemy);
            array.AddRange(BitConverter.GetBytes(guns[0]));
            array.AddRange(BitConverter.GetBytes(guns[1]));
            array.AddRange(BitConverter.GetBytes(guns[2]));
            ServerBattleWeapon[] weapon = new ServerBattleWeapon[3];
            weapon[0] = new ServerBattleWeapon(guns[0]);
            weapon[1] = new ServerBattleWeapon(guns[1]);
            weapon[2] = new ServerBattleWeapon(guns[2]);
            ServerShipB bigship = new ServerShipB(Params, battleID, ShipStates.GetBigShip(side), battle, weapon, null, array.ToArray(), hex);
            weapon[0].Ship = bigship; weapon[1].Ship = bigship; weapon[2].Ship = bigship;
            return bigship;
        }
        public static ushort[] GetBigShipWeapon(byte level, EnemyType enemy)
        {
            ushort[] result = new ushort[3];
            switch (level)
            {
                case 5:
                    if (enemy == EnemyType.Aliens) { result[0] = 7382; result[1] = 7392; result[2] = 8402; }
                    else if (enemy == EnemyType.Pirates) { result[0] = 5352; result[1] = 7422; result[2] = 6362; }
                    else { result[0] = 8452; result[1] = 5312; result[2] = 9332; }
                    break;
                case 15:
                    if (enemy == EnemyType.Aliens) { result[0] = 13382; result[1] = 13392; result[2] = 14402; }
                    else if (enemy == EnemyType.Pirates) { result[0] = 17352; result[1] = 13422; result[2] = 18362; }
                    else { result[0] = 14452; result[1] = 17312; result[2] = 15332; }
                    break;
                case 25:
                    if (enemy == EnemyType.Aliens) { result[0] = 25382; result[1] = 25392; result[2] = 26402; }
                    else if (enemy == EnemyType.Pirates) { result[0] = 23352; result[1] = 25422; result[2] = 24362; }
                    else { result[0] = 26452; result[1] = 23312; result[2] = 27332; }
                    break;
                case 35:
                    if (enemy == EnemyType.Aliens) { result[0] = 37382; result[1] = 37392; result[2] = 38402; }
                    else if (enemy == EnemyType.Pirates) { result[0] = 35352; result[1] = 37422; result[2] = 36362; }
                    else { result[0] = 38452; result[1] = 35312; result[2] = 33332; }
                    break;
                case 45:
                    if (enemy == EnemyType.Aliens) { result[0] = 43382; result[1] = 43392; result[2] = 44402; }
                    else if (enemy == EnemyType.Pirates) { result[0] = 41352; result[1] = 43422; result[2] = 42632; }
                    else { result[0] = 44452; result[1] = 41312; result[2] = 39332; }
                    break;
                case 55:
                    if (enemy == EnemyType.Aliens) { result[0] = 49382; result[1] = 49392; result[2] = 50402; }
                    else if (enemy == EnemyType.Pirates) { result[0] = 47352; result[1] = 49422; result[2] = 48362; }
                    else { result[0] = 50452; result[1] = 47312; result[2] = 45332; }
                    break;
                default:
                    if (enemy == EnemyType.Aliens) { result[0] = 300; result[1] = 300; result[2] = 300; }
                    else if (enemy == EnemyType.Pirates) { result[0] = 300; result[1] = 300; result[2] = 300; }
                    else { result[0] = 300; result[1] = 300; result[2] = 300; }
                    break;

            }
            return result;
        }*/
        public static ServerShipB GetPortalShip(byte battleID, int warphealth, ShipSide side, ServerBattle battle, byte hex, byte[] cryptname)
        {
            ServerBattleParams Params = new ServerBattleParams();
            Params.CalculatePortal(warphealth);
            List<byte> array = new List<byte>();
            array.Add((byte)ShipBArrayType.Portal);
            array.AddRange(BitConverter.GetBytes(warphealth));
            array.Add(hex);
            array.AddRange(cryptname);
            //byte hex = (byte)(side == ShipSide.Attack ? 0 : battle.Field.MaxHex);
            return new ServerShipB(Params, battleID, ShipStates.GetPortal(side), battle, null, null, array.ToArray(), hex);
        }
        public static ServerShipB GetAsteroid(byte battleid, byte hex, bool isbig)
        {
            return new ServerShipB(new ServerBattleParams(), battleid, ShipStates.GetAsteroid(isbig), null, null, null, new byte[] { (byte)ShipBArrayType.Asteroid }, hex);
        }
        public void AddGroupEffect(ServerSecondaryEffect effect)
        {
            foreach (ServerSecondaryEffect cureffect in GroupEffects)
                if (cureffect.Type == effect.Type)
                {
                    if (cureffect.Type < 63)
                        cureffect.Value += effect.Value;
                    return;
                }
            GroupEffects.Add(effect);
        }
        public DamageResult RecieveDamage(int toshield, int tohealth)
        {
            int shielddamage = 0; int healthdamage = 0;
            if (toshield > 0)
            {
                if (Params.Shield.GetCurrent > toshield)
                {
                    shielddamage = toshield; Params.Shield.RemoveParam(toshield);
                }
                else
                {
                    shielddamage = Params.Shield.GetCurrent; Params.Shield.SetNull();
                }
            }
            if (tohealth > 0)
            {
                if (Params.Health.GetCurrent > tohealth)
                {
                    healthdamage = tohealth; Params.Health.RemoveParam(tohealth);
                }
                else
                {
                    healthdamage = Params.Health.GetCurrent; Params.Health.SetNull();
                }
            }
            if (Params.Health.GetCurrent == 0)
                return new DamageResult(shielddamage, healthdamage, 0, true);
            else
                return new DamageResult(shielddamage, healthdamage, 0, false);
        }
        public bool RecieveDamage(int damage, WeaponGroup group, int angle)
        {
            DebugWindow.AddTB2(String.Format("Входящий урон {0} с угла {1}", damage, angle), true);
            int startshield = Params.Shield.GetCurrent;
            int starthealt = Params.Health.GetCurrent;
            double percentdamage = 100;
            if (IsShieldResist(angle))
            {

                int shield = Params.Shield.GetCurrent;
                int DamageLeft = Params.Shield.RemoveParam(damage, true);
                DebugWindow.AddTB2(String.Format("Урон по щиту {0}/{1} ", shield, Params.Shield.GetCurrent), false);
                if (DamageLeft == 0)
                {
                    return false;
                }
                percentdamage = ((double)DamageLeft) / damage * 100.0;
            }
            int HealthDamage = (int)(damage * percentdamage * (100 - Params.Ignore.GetCurValue(group)) / 10000.0);
            int health = Params.Health.GetCurrent;
            int HealthDamageLeft = Params.Health.RemoveParam(HealthDamage, true);
            DebugWindow.AddTB2(String.Format("Урон по хп {0}/{1}", health, Params.Health.GetCurrent), false);
            if (Params.Health.GetCurrent != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool RecieveDamage(ServerBattleWeapon weapon, bool IsCrit, int angle)
        {
            //ServerTestBattle.AddRecieveDamage(Battle.ID, Battle.CurrentTurn, States.Side, BattleID, (byte)weapon.Type, (short)angle);
            DebugWindow.AddTB2(String.Format("Входящий урон {0}с угла {1}", IsCrit ? "с критом " : "", angle), true);
            int startshield = Params.Shield.GetCurrent;
            int starthealt = Params.Health.GetCurrent;
            int plasmamodifier = 1;
            int absorpmodifier = 1;
            int restrictionmodifier = 1;
            bool GaussCrit = false;
            if (IsCrit && !States.CritProtected)
                switch (weapon.Type)
                {
                    case EWeaponType.Laser: Params.Status.SetBlind(); break;
                    case EWeaponType.Plasma: plasmamodifier = 2; break;
                    case EWeaponType.Cannon: Params.Status.SetConfuse();break;
                    case EWeaponType.Gauss: GaussCrit = true; break;
                    case EWeaponType.AntiMatter: Params.Status.SetAntiCursed(); break;
                    case EWeaponType.Psi: Params.Status.SetPsiCursed(); break;
                    case EWeaponType.Dark: absorpmodifier = 0; break;
                    case EWeaponType.Slicing: Params.Health.RemovePercent(20); Params.Shield.RemovePercent(20); Params.Energy.RemovePercent(20); break;
                    case EWeaponType.Radiation: Params.Health.AddDisease(weapon.Damage); break;
                    case EWeaponType.Drone: Params.Status.SetMark(); break;
                }
            //бонусы от ограничений в бою
            switch (Battle.Restriction)
            {
                case Restriction.DoubleEnergy: if (weapon.Group == WeaponGroup.Energy) restrictionmodifier = 2; break;
                case Restriction.DoublePhysic: if (weapon.Group == WeaponGroup.Physic) restrictionmodifier = 2; break;
                case Restriction.DoubleIrregular: if (weapon.Group == WeaponGroup.Irregular) restrictionmodifier = 2; break;
                case Restriction.DoubleCyber: if (weapon.Group == WeaponGroup.Cyber) restrictionmodifier = 2; break;
            }
            double percentdamage = 100 * plasmamodifier * restrictionmodifier;
            int ShieldDamage = 0;
            int DamageLeft = 0;
            DebugWindow.AddTB2("", true);
            if (GaussCrit == false && IsShieldResist(angle))
            {
                ShieldDamage = weapon.ToShield() * plasmamodifier * restrictionmodifier;
                int shield = Params.Shield.GetCurrent;
                DamageLeft = Params.Shield.RemoveParam(ShieldDamage);
                DebugWindow.AddTB2(String.Format("Урон по щиту {0}/{1} ", shield, Params.Shield.GetCurrent), false);
                if (DamageLeft == 0)
                {

                    //ServerTestBattle.AddShieldBlockDamage(Battle.ID, (short)ShieldDamage, (short)Params.Shield.GetCurrent, (short)Params.Shield.GetMax);
                    LastCritEffect(weapon, IsCrit);
                    return false;
                }
                percentdamage = ((double)DamageLeft) / ShieldDamage * 100.0;
            }
           // else
               // ServerTestBattle.AddShieldPierce(Battle.ID);
            //TestBattle.WriteString(String.Format("{0} Щит не сработал", Links.CurrentTurn));
            //int HealthDamage = (int)(weapon.ToHealth() * percentdamage / 100 - Params.Ignore.GetCurValue(weapon.Group) * absorpmodifier);
            int HealthDamage = (int)(weapon.ToHealth() * percentdamage * (100 - Params.Ignore.GetCurValue(weapon.Group) * absorpmodifier) / 10000.0);
            int health = Params.Health.GetCurrent;
            int HealthDamageLeft = Params.Health.RemoveParam(HealthDamage);
            DebugWindow.AddTB2(String.Format("Урон по хп {0}/{1}", health, Params.Health.GetCurrent), false);
            if (Params.Health.GetCurrent != 0)
            {
                //ServerTestBattle.AddHealthDamageLive(Battle.ID, (short)(ShieldDamage - DamageLeft), (short)HealthDamage, (short)Params.Health.GetCurrent, (short)Params.Health.GetMax);
                //TestBattle.WriteString(String.Format("{2} Урон по щиту {0} Урон по броне {1} корабль не уничтожен", ShieldDamage - DamageLeft, HealthDamage, Links.CurrentTurn));
                LastCritEffect(weapon, IsCrit);
                //TestBattle.WriteString(ToString());
                //Console.WriteLine("Щит после выстрела {0} ХП после выстрела {1}", Params.Shield.GetCurrent, Params.Health.GetCurrent);
                return false;
            }
            else
            {
               // ServerTestBattle.AddHealthDamageDestroy(Battle.ID, (short)(ShieldDamage - DamageLeft), (short)(HealthDamage - HealthDamageLeft));
                //TestBattle.WriteString(String.Format("{2} Урон по щиту {0} Урон по броне {1} корабль УНИЧТОЖЕН", ShieldDamage - DamageLeft, HealthDamage-HealthDamageLeft, Links.CurrentTurn));
                //TestBattle.WriteString(ToString());
                //Console.WriteLine("Щит после выстрела {0} ХП после выстрела {1}", Params.Shield.GetCurrent, Params.Health.GetCurrent);
                return true;
            }
        }
        public void LastCritEffect(ServerBattleWeapon weapon, bool IsCrit)
        {
            if (IsCrit && !States.CritProtected)
            {
                switch (weapon.Type)
                {
                    //case EWeaponType.Drone: Params.Shield.SetNull();  break;
                    case EWeaponType.Magnet: Angle = (Angle + 180) % 360; break;
                }
            }
        }
        public bool IsShieldResist(int DamageAngle)
        {
            if (Params.Shield.GetCurrent == 0) return false;
            int minus = DamageAngle - Angle;
            if (minus > 0 && minus <= 180) { }
            else if (minus > 0 && minus > 180) minus -= 360;
            else if (minus < 0 && minus > -180) { minus = -minus; }
            else if (minus < 0 && minus < -180) minus += +360;
            if (Math.Abs(minus) < Params.ShieldProtection) return true; else return false;
        }
        //метод проверяет, занят ли хекс для перемещения
        public bool CheckHex(byte hex)
        {
            if (Battle.BattleField.ContainsKey(hex)) return false;
            foreach (ServerShipB bigship in Battle.BigShips)
            {
                if (bigship == this) continue;
                if (bigship.Hex > Battle.Field.MaxHex) continue;
                Hex bigshiphex = Battle.Field.Hexes[bigship.Hex];
                foreach (Hex nearhex in bigshiphex.NearHexes)
                    if (nearhex.ID == hex) return false;
            }
            if (States.BigSize == false)
                return true;
            //Если корабль большой, то дополнительно определяется не блокированы ли все соседние с целевым хексом зоны
            Hex targethex = Battle.Field.Hexes[hex];
            foreach (Hex nearhex in targethex.NearHexes)
            {
                if (nearhex.ID == Hex) continue;
                if (Battle.BattleField.ContainsKey(nearhex.ID)) return false;
                foreach (ServerShipB bigship in Battle.BigShips)
                {
                    if (bigship == this) continue;
                    if (bigship.Hex > Battle.Field.MaxHex) continue;
                    foreach (Hex nearhexbig in nearhex.NearHexes)
                        if (nearhexbig.ID == nearhex.ID) return false;
                }
            }
            return true;

        }
        public void SetTimeEffect()
        {
            Params.Energy.SetNull();
            Params.Shield.SetNull();
            Params.Health.SetNull();
        }
        /// <summary> Поворачивает корабль, отнимает энергию, разряжает пушку /// </summary>
        public void MakeFire(int gun, int Rotate)
        {
            ServerBattleWeapon weapon = Weapons[gun];
            //повернуть корабль
            Angle = Rotate;
            //затратить энергию
            int energy = Params.Energy.GetCurrent;
            Params.Energy.RemoveParam(weapon.Consume);
            //разрядить оружие
            weapon.IsArmed = false;
            DebugWindow.AddTB2("Параметры: ", true);
            DebugWindow.AddTB2(String.Format("Энергия = {0} - {1} = {2}, Угол = {3} ",energy, weapon.Consume, Params.Energy.GetCurrent, Rotate), false);
            //ServerTestBattle.AddEnergySpent(Battle.ID, BattleID, States.Side, (byte)weapon.Type, (short)weapon.Consume);
            //TestBattle.WriteString(String.Format("Оружие {0} затрачено энергии {1}", weapon.Type, weapon.Consume));        
        }
        public void RemoveGoodEffectsFromShip(List<ServerSecondaryEffect> list)
        {
            foreach (ServerSecondaryEffect effect in list)
                effect.RemoveBonusParams(Params);
        }
        public void RecieveGoodEffectsFromShip(List<ServerSecondaryEffect> list)
        {
            foreach (ServerSecondaryEffect effect in list)
                effect.AppendBonusParams(Params);
        }
        public FShip GetFriendShip()
        {
            FShip ship = new FShip();
            ship.ID = BattleID;
            ship.Hex = Hex;
            ship.Energy = Params.Energy.GetCurrent;
            ship.ShipMoved = false;
            ship.ShipMoveCost = Params.HexMoveCost.GetCurrent;
            ship.HealthPercent = Params.Health.GetPercent;
            if (States.BigSize) ship.IsBig = true;
            for (int i = 0; i < 3; i++)
            {
                if (Weapons[i] != null)
                {
                    ship.Weapons[i] = new FWeapon();
                    ship.Weapons[i].Damage = (Weapons[i].ToShield() + Weapons[i].ToHealth()) / 2;
                    ship.Weapons[i].Accuracy = Weapons[i].Accuracy();
                    ship.Weapons[i].Consume = Weapons[i].Consume;
                    ship.Weapons[i].Group = Weapons[i].Group;
                    ship.Weapons[i].Type = Weapons[i].Type;
                }

            }
            return ship;
        }
        public EShip GetEnemyShip()
        {
            EShip ship = new EShip();
            ship.ID = BattleID;
            ship.Health = Params.Health.GetCurrent + Params.Shield.GetCurrent;
            ship.Evasion.Add(WeaponGroup.Energy, Params.Evasion.GetCurValue(WeaponGroup.Energy));
            ship.Evasion.Add(WeaponGroup.Physic, Params.Evasion.GetCurValue(WeaponGroup.Physic));
            ship.Evasion.Add(WeaponGroup.Irregular, Params.Evasion.GetCurValue(WeaponGroup.Irregular));
            ship.Evasion.Add(WeaponGroup.Cyber, Params.Evasion.GetCurValue(WeaponGroup.Cyber));
            ship.Ignore.Add(WeaponGroup.Energy, Params.Ignore.GetCurValue(WeaponGroup.Energy));
            ship.Ignore.Add(WeaponGroup.Physic, Params.Ignore.GetCurValue(WeaponGroup.Physic));
            ship.Ignore.Add(WeaponGroup.Irregular, Params.Ignore.GetCurValue(WeaponGroup.Irregular));
            ship.Ignore.Add(WeaponGroup.Cyber, Params.Ignore.GetCurValue(WeaponGroup.Cyber));
            ship.Hex = Hex;
            if (States.BigSize) ship.IsBig = true;
            return ship;
        }
        public void MoveInQueue()
        {
            Hex--;
        }
        public void MoveToGate()
        {
            Hex = (byte)(210 + Params.JumpDistance);
            //ServerTestBattle.AddMoveShipToGate(Battle.ID, Battle.CurrentTurn, BattleID, States.Side, Params.JumpDistance);
            //TestBattle.WriteString(String.Format("{3} Сторона {0} Корабль {1} перешёл в гейт, дальность прыжка {2}", IsAttackers, BattleID, Params.JumpDistance, Links.CurrentTurn));
        }
        public void MoveToPort()
        {
            Hex = (byte)(200 + Params.TimeToEnter);
            //Battle.CurrentTurn++;
            //Links.CurrentTurn++;
           // ServerTestBattle.AddMoveShipToPort(Battle.ID, Battle.CurrentTurn, BattleID, States.Side, Params.TimeToEnter);
            //TestBattle.WriteString(String.Format("{3} Сторона {0} Корабль {1} заведён в порт, число ходов {2}", IsAttackers, BattleID, Params.TimeToEnter, Links.CurrentTurn));
            Params.Shield.SetMax();
            Params.Energy.SetMax();
            for (int i = 0; i < 3; i++)
                if (Weapons[i] != null) Weapons[i].IsArmed = true;
        }
        public void RoundStart()
        {
            Params.Health.RoundStart();
            Params.Shield.RoundStart();
            Params.Energy.RoundStart();
            for (int i = 0; i < 3; i++)
                if (Weapons[i] != null) Weapons[i].IsArmed = true;
        }
        public bool RoundEnd()
        {
            Params.Health.RoundEnd();
            Params.Shield.RoundEnd();
            Params.Energy.RoundEnd();
            Params.Status.RoundEnd();

            //TestBattle.WriteString(ToString());
            if (Params.Health.GetCurrent == 0) return true; else return false;
        }
        public byte[] GetLogInfo()
        {
            List<byte> list = new List<byte>();
            list.Add((byte)States.Side);
            list.Add(BattleID);
            list.Add(Hex);
            list.AddRange(Params.Health.GetLogInfo());
            list.AddRange(Params.Shield.GetLogInfo());
            list.AddRange(Params.Energy.GetLogInfo());
            if (Pilot != null)
                list.AddRange(Pilot.GetArray());
            else
                list.AddRange(GSPilot.GetNullPilot());
            return list.ToArray();
        }
        public static string GetLogString(byte[] array, ref int i)
        {
            string id = array[i].ToString(); i++;
            bool attack = BitConverter.ToBoolean(array, i); i++;
            string attackstring;
            if (attack) attackstring = "Атака"; else attackstring = "Защита";
            string hex = array[i].ToString(); i++;
            string Health = ValCapParameter.GetLogString(array, ref i);
            string Shield = ValCapParameter.GetLogString(array, ref i);
            string Energy = ValCapParameter.GetLogString(array, ref i);
            string Pilot = GSPilot.GetPilotLogInfo(array, ref i);
            return string.Format("{0} {1} {2}/n{3}/n{4}/n{5}/n{6}", attackstring, id, hex, Health, Shield, Energy, Pilot);
        }
        public override string ToString()
        {
            if (States.Side == ShipSide.Neutral)
            { return String.Format("Asteroid Hex={0}", Hex); }
            string Health = "Health " + Params.Health.ToString();
            string Shield = "Shield " + Params.Shield.ToString();
            string Energy = "Energy " + Params.Energy.ToString();
            string ID = "ID " + BattleID.ToString() + (States.Side.ToString());
            string hex = "Hex " + Hex.ToString();
            if (Pilot != null)
                return string.Format("{0} {1} {2} {3} {4}/n {5}", ID, hex, Health, Shield, Energy, Pilot.ToString());
            else
                return string.Format("{0} {1} {2} {3} {4}", ID, hex, Health, Shield, Energy);
        }
        int CalcRating()
        {
            if (Pilot == null) return 0;
            if (StartHealth == 0) return 0;
            double HealthRating = Params.Health.GetMax * RatingModifiers.HealthMax + Params.Health.GetRestore * RatingModifiers.HealthRestore;
            double ShieldRating = Params.Shield.GetMax * RatingModifiers.ShieldMax + Params.Shield.GetRestore * RatingModifiers.ShieldRestore;
            double EnergyRating = Params.Energy.GetMax * RatingModifiers.EnergyMax + Params.Energy.GetRestore * RatingModifiers.EnergyRestore;
            double WeaponRating = 0;
            foreach (ServerBattleWeapon weapon in Weapons)
            {
                if (weapon == null) continue;
                WeaponRating += (weapon.ToShield() * RatingModifiers.ToShieldDamage + weapon.ToHealth() * RatingModifiers.ToHealthDamage + weapon.Accuracy() * RatingModifiers.Accuracy)
                    * ((int)weapon.Size * RatingModifiers.WeaponSize + 1) * RatingModifiers.WeaponType[(int)weapon.Type];
            }
            double EvasionRating = (Params.Evasion.GetCurValue(WeaponGroup.Energy) + Params.Evasion.GetCurValue(WeaponGroup.Physic)
                + Params.Evasion.GetCurValue(WeaponGroup.Irregular) + Params.Evasion.GetCurValue(WeaponGroup.Cyber)) * RatingModifiers.Evasion;
            double AbsorbRating = (Params.Ignore.GetCurValue(WeaponGroup.Energy) + Params.Ignore.GetCurValue(WeaponGroup.Physic)
                + Params.Ignore.GetCurValue(WeaponGroup.Irregular) + Params.Ignore.GetCurValue(WeaponGroup.Cyber)) * RatingModifiers.Absorb;
            double ImmuneRating = (Params.Immune.GetCurValue(WeaponGroup.Energy) + Params.Immune.GetCurValue(WeaponGroup.Physic)+
                + Params.Immune.GetCurValue(WeaponGroup.Irregular) + Params.Immune.GetCurValue(WeaponGroup.Cyber)) * RatingModifiers.Immune / 255;
            double TotalRating = HealthRating + ShieldRating + EnergyRating + WeaponRating + EvasionRating + AbsorbRating + ImmuneRating;
            double ResultRating = TotalRating *
                ((int)Schema.Shield.Size * RatingModifiers.ShieldSize + 1) *
                ((int)Schema.Generator.Size * RatingModifiers.GeneratorSize + 1) *
                ((int)Schema.Computer.Size * RatingModifiers.ComputerSize + 1) *
                ((int)Schema.Engine.Size * RatingModifiers.EngineSize + 1) *
                (GroupEffects.Count * RatingModifiers.GroupEffect + 1);
            return (int)(ResultRating * StartHealth / 100);

        }
        public byte GetEnterToFieldSet()
        {
            if (Battle == null) return 255;
            SortedSet<byte> result = new SortedSet<byte>();
            List<byte> temp;

            if (States.Side == ShipSide.Attack)
                temp = Battle.Side1.GetJumpHexes((byte)(Params.JumpDistance - 1));// Field.AIHexes[0, Params.JumpDistance - 1];
            else
                temp = Battle.Side2.GetJumpHexes((byte)(Params.JumpDistance - 1));// Field.AIHexes[1, Params.JumpDistance - 1];
            //result = new SortedSet<byte>();
            foreach (byte b in temp)
                if (CheckHex(b)) result.Add(b);
            if (result.Count == 0) return 255; else return result.Max;
        }
    }
    class RatingModifiers
    {
        public static double HealthMax = 3.0;
        public static double HealthRestore = 10.0;
        public static double ShieldMax = 1.0;
        public static double ShieldRestore = 1.5;
        public static double ShieldSize = 0.2;
        public static double EnergyMax = 1;
        public static double EnergyRestore = 2;
        public static double GeneratorSize = 0.05;
        public static double ToShieldDamage = 1.5;
        public static double ToHealthDamage = 2;
        public static double Accuracy = 1;
        public static double WeaponSize = 0.2;
        public static double ComputerSize = 0.05;
        public static double Evasion = 3;
        public static double EngineSize = 0.1;
        public static double Absorb = 10;
        public static double Immune = 400;
        public static double GroupEffect = 0.2;
        public static double[] WeaponType = new double[]
        {1.0,1.0,1.2,1.3,
            1.0,1.0,1.2,1.3,
            1.2,1.2,1.2,1.3,
            1.2,1.2,1.2,1.2};
        public static double PortalHealth = 5;
    }
}
