using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Client
{
    public enum ShipSide { Attack, Defense, Neutral, None }
    /// <summary> способ сохранения информации о корабле. Array - полная информация в архиве </summary>
    public enum ShipBArrayType { Standard, Portal, Asteroid, Array }
    public class ShipStates
    {
        public bool CritProtected;
        public bool Indestructible;
        public bool Controlled;
        public bool BigSize;
        public bool HaveShields;
        public bool HaveEnergy;
        public bool IsPortal;
        public ShipSide Side;
        public ShipStates(bool crit, bool indestr, bool control, bool isbig, ShipSide side, bool shields, bool energy, bool isPortal)
        {
            CritProtected = crit; Indestructible = indestr; Controlled = control; BigSize = isbig; Side = side;
            HaveShields = shields; HaveEnergy = energy; IsPortal = isPortal;
        }
        public static ShipStates GetTypicalShip(ShipSide side)
        {
            return new ShipStates(false, false, true, false, side, true, true, false);
        }
        public static ShipStates GetPortal(ShipSide side)
        {
            return new ShipStates(true, false, false, false, side, false, false, true);
        }
        public static ShipStates GetAsteroid(bool isbig)
        {
            return new ShipStates(true, true, false, isbig, ShipSide.Neutral, false, false, false);
        }
        public static ShipStates GetBigShip(ShipSide side)
        {
            return new ShipStates(true, false, true, true, side, true, true, false);
        }
        public static ShipStates GetBigBuilding(ShipSide side)
        {
            return new ShipStates(true, false, false, true, side, false, false, false);
        }
        public static ShipStates GetCargoShip(ShipSide side)
        {
            return new ShipStates(true, false, false, false, side, true, false,  false);
        }
    }
   
    public enum ShipBMode { Battle, Info }
    public class ShipB: IComparable
    {
        public byte BattleID;
        public Battle Battle;
        public Side Side;
        public Schema Schema;
        public byte[] Model;
        public GSPilot Pilot;
        public byte StartHealth;
        public byte CurHex { get; private set; }
        public ShipPanel2 Panel2;
        //public ShipPanel PanelB;
        public BattleParams Params;
        public BattleWeapon[] Weapons = new BattleWeapon[3];
        public List<SecondaryEffect> GroupEffects;
        public ShipStates States;
        //public ShipSide SSide;
        //public bool IsPortal = false;
        public bool CanRule;
        public HexShip HexShip;
        public int Angle;
        public int Rating = 0;
        public SortedSet<Hex>[] JumpHexes;
        public byte[] NameCrypt;
        public ShipB(byte battleID, Schema schema, byte health, GSPilot pilot, byte[] model, ShipSide side, Battle battle, bool canrule, byte[] cryptname, ShipBMode mode, byte hex)
        {
            BattleID = battleID;
            Battle = battle;
            CanRule = canrule;
            Schema = schema;
            if (schema != null)
                NameCrypt = BitConverter.GetBytes(schema.CryptName);
            else
                NameCrypt = cryptname;
            Model = model;
            StartHealth = health;
            Pilot = pilot;
            States = ShipStates.GetTypicalShip(side);
            GroupEffects = new List<SecondaryEffect>();
            Params = new BattleParams();
            Params.CalculateBase(this);
                if (Schema.GetWeapon(0) != null) Weapons[0] = new BattleWeapon(this, 0);
                if (Schema.GetWeapon(1) != null) Weapons[1] = new BattleWeapon(this, 1);
                if (Schema.GetWeapon(2) != null) Weapons[2] = new BattleWeapon(this, 2);
            Panel2 = new ShipPanel2(this, null);
            if (Params.TimeToEnter == 99)
                CurHex = 254;
            else if (hex != 255)
                CurHex = hex;
            else
                CurHex = (byte)(200 + Params.TimeToEnter);
            //PanelB = new ShipPanel(this, TitleBrush);
            HexShip = new HexShip(this, BattleID, mode);

            //Rating = CalcRating();
        }
        public void SetHex(byte hex)
        {
            CurHex = hex;
            if (States.IsPortal)
            {
                if (CurHex > Battle.Field.MaxHex) { JumpHexes = null; return; }
                JumpHexes = new SortedSet<Hex>[3];
                JumpHexes[0] = new SortedSet<Hex>(); JumpHexes[1] = new SortedSet<Hex>(); JumpHexes[2] = new SortedSet<Hex>();
                for (int i = 0; i < 3; i++)
                {
                    Hex portalhex = Battle.Field.Hexes[CurHex];
                    foreach (Hex nearhex in portalhex.NearHexes)
                        JumpHexes[i].Add(nearhex);
                    for (int j = 0; j < i; j++)
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
        public string GetName()
        {
            if (States.IsPortal)
                return NameCreator.GetName(ShipNameType.Portal, Schema, NameCrypt);
            else if (States.BigSize)
                return NameCreator.GetName(ShipNameType.Big, Schema, NameCrypt);
            else
                return NameCreator.GetName(ShipNameType.Standard, Schema, NameCrypt);
            /*
            if (States.IsPortal)
            {
                switch (Model[0])
                {
                    case 252: return "Пиратский портал";
                    case 251: return "Портал зелёного альянса";
                    case 254: return "Портал технократов";
                    case 255: return "Портал наёмников";
                    case 253: return "Портал чужих";
                    default: return "Портал";
                }
            }
            else if (States.BigSize)
            {
                switch (Model[0])
                {
                    case 252: return "Пиратский разрушитель";
                    case 251: return "Разрушитель зелёного альянса";
                    case 254: return "Разрушить технократов";
                    case 255: return "Разрушитель наёмников";
                    case 253: return "Разрушитель чужих";
                    default: return "Разрушитель";
                }
            }
            else
            {
                switch (Model[0])
                {
                    case 252: return "Пиратский корабль";
                    case 251: return "Корабль зелёного альянса";
                    case 254: return "Корабль технократов";
                    case 255: return "Корабль наёмников";
                    case 253: return "Корабль чужих";
                }
                if (Schema!= null)
                {
                    byte[] schemaname = BitConverter.GetBytes(Schema.CryptName);
                    switch (schemaname[0])
                    {
                        case 249: return StoryLine2.StoryShipNames[schemaname[1]];
                    }
                    return Schema.ShipType.GetName();
                }
                return "Корабль";
            }
            */
        }
        public void AddGroupEffect(SecondaryEffect effect)
        {
            foreach (SecondaryEffect cureffect in GroupEffects)
                if (cureffect.Type==effect.Type)
                {
                    if (cureffect.Type < 63)
                        cureffect.Value += effect.Value;
                    return;
                }
            GroupEffects.Add(effect);
        }
        public ShipB(BattleParams battleparams, byte battleID, ShipStates states, Battle battle, BattleWeapon[] weapons, List<SecondaryEffect> groupeffects, bool canrule, byte hex, byte[] model, byte[] cryptname)
        {
            BattleID = battleID;
            Battle = battle;
            if (groupeffects == null)
                GroupEffects = new List<SecondaryEffect>();
            else
                GroupEffects = groupeffects;
            Params = battleparams;
            if (weapons != null) Weapons = weapons;
            CurHex = hex;
            States = states;
            CanRule = canrule;
            Model = model;
            StartHealth = 100;
            NameCrypt = cryptname;
        }
        public static ShipB GetShipFromArray(byte[] array, ref int i, byte battleid, ShipSide side, Battle battle, bool canrule)
        {
            StoryShipType type = (StoryShipType)array[i]; i++;
            ShipStates states;
            switch (type)
            {
                case StoryShipType.StandardShip: states = ShipStates.GetTypicalShip(side); break;
                case StoryShipType.Portal: states = ShipStates.GetPortal(side); break;
                case StoryShipType.CargoShip: states = ShipStates.GetCargoShip(side); break;
                case StoryShipType.BigShip: states = ShipStates.GetBigShip(side); break;
                case StoryShipType.LargeBuilding: states = ShipStates.GetBigBuilding(side); break;
                default: throw new Exception();
            }
            BattleParams battleparams = new BattleParams();
            battleparams.CalculateFromArray(array, ref i);
            byte hex = array[i]; i++;
            int image = BitConverter.ToInt32(array, i); i += 4;
            byte[] cryptname = new byte[4]; for (int k = 0; k < 4; k++, i++) cryptname[k] = array[i];
            BattleWeapon[] weapons = BattleWeapon.GetWeaponsFromArray(array, ref i);
            byte groupeffectscount = array[i]; i++;
            List<SecondaryEffect> groupeffects = new List<SecondaryEffect>();
            for (int j=0;j<groupeffectscount;j++, i+=5)
                groupeffects.Add(new SecondaryEffect(array[i], BitConverter.ToInt32(array, i + 1)));
            ShipB ship = new ShipB(battleparams, battleid, states, battle, weapons, groupeffects, canrule, hex, BitConverter.GetBytes(image), cryptname);
            foreach (BattleWeapon weapon in weapons)
                if (weapon != null) weapon.Ship = ship;
            if (type == StoryShipType.BigShip)
                ship.HexShip = HexShip.GetBigShipHexShip(ship);
            else if (type == StoryShipType.LargeBuilding)
                ship.HexShip = HexShip.GetLargeBuildingHexShip(ship);
            else
                ship.HexShip = new HexShip(ship, battleid, ShipBMode.Battle);
            if (type == StoryShipType.CargoShip)
                ship.HexShip.Children.Remove(ship.HexShip.Energy);
            return ship;
        }
       /* public static ShipB GetCargoShip(BattleParams battleparams, byte battleID, ShipStates states, Battle battle, byte hex, byte model, byte[] cryptname)
        {
            ShipB ship = new ShipB(battleparams, battleID, states, battle, null, null, false, hex, new byte[] { model, 0, 0, 0 }, cryptname);
            ship.HexShip = HexShip.GetCargoShipHexShip(ship);
            //ship.PanelB = ShipPanel.GetCargoShipPanel(ship);
            //ship.HexShip.ToolTip = ship.PanelB.VBX;
            ship.SetAngle(states.Side == ShipSide.Attack ? 120 : 300, true);
            return ship;
        }*/
        public static ShipB GetBigShip(BattleParams battleparams, byte battleID, ShipStates states, Battle battle, BattleWeapon[] weapons, bool canrule, byte hex, byte model, byte[] cryptname)
        {
            ShipB ship = new ShipB(battleparams, battleID, states, battle, weapons, null, canrule, hex, new byte[] { model, 0, 0, 0 }, cryptname);
            foreach (BattleWeapon weapon in weapons)
                weapon.Ship = ship;
            ship.HexShip = HexShip.GetBigShipHexShip(ship);
            //ship.PanelB = ShipPanel.GetBigShipPanel(ship);
            //ship.HexShip.ToolTip = ship.PanelB.VBX;
            return ship;
        }
        public static ShipB GetPortalShip(byte battleID, int warphealth, ShipSide side, Battle battle, bool canrule, byte hex, byte[] cryptname)
        {
            BattleParams Params = new BattleParams();
            Params.CalculatePortal(warphealth);
            ShipB ship = new ShipB(Params, battleID, ShipStates.GetPortal(side), battle, null, null, canrule, hex, new byte[4], cryptname);
            ship.HexShip = new PortalB(ship, battleID);
            //ship.PanelB =  new PortalPanel(ship);
            //ship.HexShip.ToolTip = ship.PanelB.VBX;
            return ship;
        }
        public static ShipB GetAsteroid(byte battleid, Battle battle, byte hex, byte model, bool isbig)
        {
            ShipB ship = new ShipB(new BattleParams(), battleid, ShipStates.GetAsteroid(isbig), battle, null, null, false, hex, new byte[] { model, 0, 0, 0 }, new byte[4]);
            ship.HexShip = new AsteroidHex(ship, battleid);
            ship.Angle = 180; ship.HexShip.SetAngle(180);
            return ship;
        }
        public byte GetWeaponsCount()
        {
            if (Weapons[0] == null && Weapons[1] == null && Weapons[2] == null) return 0;
            else if (Weapons[0] == null && Weapons[1] != null && Weapons[2] == null) return 1;
            else if (Weapons[9] != null && Weapons[1] == null && Weapons[2] != null) return 2;
            else return 3;
        }
        public int CompareTo(object B)
        {
            ShipB b = (ShipB)B;
            if (States.Side > b.States.Side) return 1;
            else if (States.Side<b.States.Side) return -1;
            else if (BattleID > b.BattleID) return 1;
            else if (BattleID < b.BattleID) return -1;
            else return 0;
        }
       
        public void RemoveGoodEffectsFromShip(ShipB donor, List<SecondaryEffect> list, bool IsVisual, string reason)
        {
            foreach (SecondaryEffect effect in list)
            {
                effect.RemoveBonusParams(Params);
                //HexShip.Bonuses.RemoveEffect(effect, reason);
            }
            if (IsVisual) HexCanvas.PutEffectsGrid(list, false, donor.HexShip.Hex, HexShip.Hex);
            AppendAllParams();
        }
        public void RecieveGoodEffectsFromShip(ShipB donor, List<SecondaryEffect> list, bool IsVisual, string reason)
        {
            foreach (SecondaryEffect effect in list)
            {
                effect.AppendBonusParams(Params);
                //HexShip.Bonuses.AddEffect(effect, reason);
            }
            if (IsVisual) HexCanvas.PutEffectsGrid(list, true, HexShip.Hex, donor.HexShip.Hex);
            AppendAllParams();
        }
        int health, shield, energy;
        public void RoundStart(bool IsVisual)
        {
            if (IsVisual) SaveParams();
            Params.Health.RoundStart();
            Params.Shield.RoundStart();
            Params.Energy.RoundStart();
            if (States.HaveShields)
            {
                if (Params.Shield.GetCurrent == 0) HexShip.ShieldField.SwitchShield(false);
                else HexShip.ShieldField.SwitchShield(true);
            }
            if (Weapons[0] != null) Weapons[0].IsArmed(true, "Зарядка в начале раунда");
            if (Weapons[1] != null) Weapons[1].IsArmed(true, "Зарядка в начале раунда");
            if (Weapons[2] != null) Weapons[2].IsArmed(true, "Зарядка в начале раунда");
            if (IsVisual) AppendChanges();
            
        }
        public void FullRestore(bool IsVisual)
        {
            if (IsVisual) SaveParams();
            Params.Health.SetMax();
            Params.Shield.SetMax();
            if (States.HaveShields)
                HexShip.ShieldField.SwitchShield(true);
            Params.Energy.SetMax();
            Params.Status.Clear();
            HexShip.SetBlind(false);
            HexShip.SetMarked(false);
            //HexShip.SetSlow(false);
            if (!States.CritProtected)
            {
                HexShip.Blocking.StopStar();
                HexShip.Blocking.StopAnti();
                HexShip.Blocking.StopPsi();
            }
            //if (IsVisual) AppendAllParams();
            if (IsVisual) AppendChanges();
        }

        public void SaveParams()
        {
            health = Params.Health.GetCurrent;
            shield = Params.Shield.GetCurrent;
            energy = Params.Energy.GetCurrent;
        }
        /// <summary> метод выводит информацию об изменениях в виде табличек на кораблях </summary>
        public void AppendChanges()
        {
            health = Params.Health.GetCurrent - health;
            shield = Params.Shield.GetCurrent - shield;
            energy = Params.Energy.GetCurrent - energy;
            ShowMessages(health, shield, energy);
            AppendAllParams();
        }
        public void ShowMessages(int health, int shield, int energy)
        {
            SortedList<DamageLabelTarget, string> Parameters = new SortedList<DamageLabelTarget, string>();
            if (health != 0) Parameters.Add(DamageLabelTarget.Health, health > 0 ? "+" + health.ToString() : health.ToString());
            if (shield != 0) Parameters.Add(DamageLabelTarget.Shield, shield > 0 ? "+" + shield.ToString() : shield.ToString());
            if (energy != 0) Parameters.Add(DamageLabelTarget.Energy, energy > 0 ? "+" + energy.ToString() : energy.ToString());
            switch (Parameters.Count)
            {
                case 1: HexCanvas.PutOneDamageLabel(HexShip.Hex, Parameters.ElementAt(0).Key, Parameters.ElementAt(0).Value); break;
                case 2: HexCanvas.PutTwoDamageLabel(HexShip.Hex, Parameters.ElementAt(0).Key, Parameters.ElementAt(0).Value,
                    Parameters.ElementAt(1).Key, Parameters.ElementAt(1).Value); break;
                case 3: HexCanvas.PutThreeDamageLabel(HexShip.Hex, Parameters.ElementAt(0).Key, Parameters.ElementAt(0).Value,
                     Parameters.ElementAt(1).Key, Parameters.ElementAt(1).Value,
                     Parameters.ElementAt(2).Key, Parameters.ElementAt(2).Value); break;
            }
        }
        public bool RoundEnd(bool IsVisual)
        {
            if (IsVisual) SaveParams();
            Params.Health.RoundEnd();
            if (!States.CritProtected && Params.Health.Disease == 0) HexShip.SetRadiation(false);//HexShip.Health.RemoveCorrupted();
            Params.Shield.RoundEnd();
            if (States.HaveShields)
            {
                if (Params.Shield.GetCurrent == 0) HexShip.ShieldField.SwitchShield(false);
                else HexShip.ShieldField.SwitchShield(true);
                if (Params.Shield.Disease == 0) HexShip.Shield.RemoveCorrupted();
            }
            Params.Energy.RoundEnd();
            if (!States.CritProtected && Params.Energy.Disease == 0) HexShip.Energy.RemoveCorrupted();
            Params.Status.RoundEnd();
            if (Params.Status.IsBlinded == 0) 
                HexShip.SetBlind(false);
            if (Params.Status.IsMarked == 0)
                HexShip.SetMarked(false);
            //if (Params.Status.IsSlowed == 0)
            //    HexShip.SetSlow(false);
            if (!States.CritProtected)
            {
                if (Params.Status.IsConfused == 0) HexShip.Blocking.StopStar();
                if (Params.Status.IsAntiCursed == 0) HexShip.Blocking.StopAnti();
                if (Params.Status.IsPsiCursed == 0) HexShip.Blocking.StopPsi();
            }
            //    if (IsVisual) AppendAllParams();
            if (IsVisual) AppendChanges();
            if (Params.Health.GetCurrent == 0) return true; else return false;
        }
        public void RemoveEnergy(int value, bool IsVisual)
        {
            if (IsVisual) SaveParams();
            Params.Energy.RemoveParam(value, true);
            if (IsVisual) AppendAllParams();
            if (IsVisual) AppendChanges();
        }
        public void MoveToField(byte hexid)
        {
            
                CurHex = hexid;
                Angle = States.Side==ShipSide.Attack ? 120 : 300;
            
        }
        public void MoveToGate(bool IsVisual)
        {
            CurHex = (byte)(210 + Params.JumpDistance);
            //if (IsVisual)
            //    HexShip.MoveToGate(CurHex);
            //TestBattle.AddMoveShipToGate(Battle.ID, Battle.CurrentTurn, BattleID, States.Side, Params.JumpDistance);
            //TestBattle.WriteString(String.Format("{3} Сторона {0} Корабль {1} перешёл в гейт, дальность прыжка {2}", 
            //    IsAttackers, BattleID, Params.JumpDistance, Battle.CurrentTurn));
        }
        public void MoveInQueue(bool isVisual)
        {
            CurHex--;
            //if (isVisual)
             //   HexShip.MoveInQueue(CurHex);
        }
        static List<ShipB> CommonList = new List<ShipB>();
        public void PutShipToHexNotReal(Hex hex, int angle, byte curHex)
        {
            if (CurHex < IntBoya.Battle.Field.MaxHex)
            {
                Battle.BattleField.Remove(CurHex);
                //Battle.BattleFieldInfo.Add(string.Format("Временное убирание корабля с поля Attack={0} ID={1} Hex={2}", States.Side, BattleID, CurHex));
            }
            CurHex = curHex;
            Angle = angle;
            HexShip.SetAngle(angle);
            HexShip.Hex = hex;
            //HexShip.Hex = Hex.Hexes[hex.ID];
            double x = hex.CenterX - HexShip.Width / 2;
            double y = hex.CenterY - HexShip.Height / 2;
            DoubleAnimation xanim = new DoubleAnimation(x, Links.ZeroTime);
            DoubleAnimation yanim = new DoubleAnimation(y, Links.ZeroTime);
            HexShip.BeginAnimation(Canvas.LeftProperty, xanim);
            HexShip.BeginAnimation(Canvas.TopProperty, yanim);
            if (curHex < Battle.Field.MaxHex)
            {
                if (Battle.BattleField.ContainsKey(CurHex))
                    CommonList.Add(this);
                else
                {
                    Battle.BattleField.Add(CurHex, this);
                    //Battle.BattleFieldInfo.Add(string.Format("Временное восстановление корабля на поле боя Attack={0} ID={1} Hex={2}", States.Side, BattleID, CurHex));
                }
            }
                    for (int i = 0; i < CommonList.Count; i++)
                    if (!Battle.BattleField.ContainsKey(CommonList[i].CurHex))
                    {
                        Battle.BattleField.Add(CommonList[i].CurHex, CommonList[i]);
                        //Battle.BattleFieldInfo.Add(string.Format("Временное восстановление корабля на поле боя Attack={0} ID={1} Hex={2}", CommonList[i].States.Side, CommonList[i].BattleID, CommonList[i].CurHex));
                        CommonList.RemoveAt(i);
                        i--;
                    }

                
            
                //PanelB.Location.SetHex(curHex);
            
        }
        public void SetEnergy(int basicenergy, int bonusenergy)
        {
            Params.Energy.BonusValue = bonusenergy;
            Params.Energy.BasicValue = basicenergy;
            HexShip.Energy.Move(Params.Energy.GetCurrent);
            //PanelB.Energy.Value.Content = Params.Energy.GetCurrent;
        }
        public void SetAngle(double angle, bool IsVisual)
        {
            Angle = (int)angle;
            if (IsVisual) HexShip.SetAngle(angle);
        }
        public void MoveShipToPort(Hex gatehex, bool IsVisual)
        {
            CurHex = (byte)(200 + Params.TimeToEnter);
            HexShip.MoveShipToPort(CurHex, gatehex, IsVisual);
            //TestBattle.AddMoveShipToPort(Battle.ID, Battle.CurrentTurn, BattleID, States.Side, Params.TimeToEnter);
            //TestBattle.WriteString(String.Format("{3} Сторона {0} Корабль {1} заведён в порт, число ходов {2}",
            //                IsAttackers, BattleID, Params.TimeToEnter, Battle.CurrentTurn));
            if (IsVisual) SaveParams();
            Params.Shield.SetMax();
            Params.Energy.SetMax();
            if (States.HaveShields)
            {
                if (Params.Shield.GetCurrent == 0) HexShip.ShieldField.SwitchShield(false);
                else HexShip.ShieldField.SwitchShield(true);
            }
            if (Weapons[0] != null) Weapons[0].IsArmed(true, "Зарядка при попадании корабля в порт");
            if (Weapons[1] != null) Weapons[1].IsArmed(true, "Зарядка при попадании корабля в порт");
            if (Weapons[2] != null) Weapons[2].IsArmed(true, "Зарядка при попадании корабля в порт");
            if (IsVisual) AppendChanges();
        }
        /// <summary> метод изменяет состояние круговых индикаторов в зависимости от параметров корабля </summary>
        public void AppendAllParams()
        {
            HexShip.Health.SetMax(Params.Health.GetMax);
            HexShip.Health.Move(Params.Health.GetCurrent);
            //PanelB.Health.Capacity.Content = Params.Health.GetMax;
            //PanelB.Health.Value.Content = Params.Health.GetCurrent;
            //PanelB.Health.Restore.Content = Params.Health.GetRestoreString;
            if (States.HaveShields)
            {
                HexShip.Shield.SetMax(Params.Shield.GetMax);
                HexShip.Shield.Move(Params.Shield.GetCurrent);
                //PanelB.Shield.Capacity.Content = Params.Shield.GetMax;
                //PanelB.Shield.Value.Content = Params.Shield.GetCurrent;
                //PanelB.Shield.Restore.Content = Params.Shield.GetRestoreString;
            }
            if (States.HaveEnergy)
            {
                HexShip.Energy.SetMax(Params.Energy.GetMax);
                HexShip.Energy.Move(Params.Energy.GetCurrent);
                //PanelB.Energy.Capacity.Content = Params.Energy.GetMax;
                //PanelB.Energy.Value.Content = Params.Energy.GetCurrent;
                //PanelB.Energy.Restore.Content = Params.Energy.GetRestoreString;
            }
            if (Panel2!=null) Panel2.Update();
        }
        public DamageResult CalcBasicDamage(BattleWeapon weapon, double angle)
        {
            int restrictionmodifier = 1;
            switch (Battle.Restriction)
            {
                case Restriction.DoubleEnergy: if (weapon.Group == WeaponGroup.Energy) restrictionmodifier = 2; break;
                case Restriction.DoublePhysic: if (weapon.Group == WeaponGroup.Physic) restrictionmodifier = 2; break;
                case Restriction.DoubleIrregular: if (weapon.Group == WeaponGroup.Irregular) restrictionmodifier = 2; break;
                case Restriction.DoubleCyber: if (weapon.Group == WeaponGroup.Cyber) restrictionmodifier = 2; break;
            }
            int ShieldDamage = 0;
            if (IsShieldResist((int)angle, false))
            {
                ShieldDamage = weapon.ToShield() * restrictionmodifier;
                if (ShieldDamage <= Params.Shield.GetCurrent)
                    return new DamageResult(ShieldDamage, 0, 0, false);
                ShieldDamage = Params.Shield.GetCurrent;
            }
            double percentdamage = ShieldDamage / ((double)weapon.ToShield() * restrictionmodifier);
            int HealthDamage= (int)(weapon.ToHealth()*restrictionmodifier * (1-percentdamage) * (100 - Params.Ignore.GetCurValue(weapon.Group)) / 100.0);
            if (Params.Health.GetCurrent < HealthDamage)
                return new DamageResult(ShieldDamage, Params.Health.GetCurrent, 0, true);
            else
                return new DamageResult(ShieldDamage, HealthDamage, 0, false);
        }
        public DamageResult RecieveDamage(int toshield, int tohealth)
        {
            int shielddamage = 0; int healthdamage = 0;
            if (toshield>0)
            {
                if (Params.Shield.GetCurrent>toshield)
                {
                    shielddamage = toshield; Params.Shield.RemoveParam(toshield);
                }
                else
                {
                    shielddamage = Params.Shield.GetCurrent; Params.Shield.SetNull();
                }
            }
            if (tohealth>0)
            {
                if (Params.Health.GetCurrent>tohealth)
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
        public DamageResult RecieveDamage(int damage, WeaponGroup group, double angle)
        {
            DebugWindow.AddTB1(String.Format("Входящий урон {0} с угла {1}", damage, angle), true);
            int startshield = Params.Shield.GetCurrent;
            int starthealt = Params.Health.GetCurrent;
            int startenergy = Params.Energy.GetCurrent;
            double percentdamage = 100;
            if (IsShieldResist((int)angle, false))
            {

                int shield = Params.Shield.GetCurrent;
                int DamageLeft = Params.Shield.RemoveParam(damage, true);
                DebugWindow.AddTB1(String.Format("Урон по щиту {0}/{1} ", shield, Params.Shield.GetCurrent), false);
                if (DamageLeft == 0)
                {
                    HexShip.Shield.Move(Params.Shield.GetCurrent);
                    HexShip.Health.Move(Params.Health.GetCurrent);
                    HexShip.Energy.Move(Params.Energy.GetCurrent);
                    if (Params.Shield.GetCurrent == 0) HexShip.ShieldField.SwitchShield(false);
                    return new DamageResult(startshield - Params.Shield.GetCurrent, starthealt - Params.Health.GetCurrent, startenergy - Params.Energy.GetCurrent, false);
                }
                percentdamage = ((double)DamageLeft) / damage * 100.0;
            }
            int HealthDamage = (int)(damage * percentdamage * (100 - Params.Ignore.GetCurValue(group)) / 10000.0);
            int health = Params.Health.GetCurrent;
            int HealthDamageLeft = Params.Health.RemoveParam(HealthDamage, true);
            DebugWindow.AddTB1(String.Format("Урон по хп {0}/{1}", health, Params.Health.GetCurrent), false);
            if (Params.Health.GetCurrent != 0)
            {
                if (States.HaveShields)
                {
                    HexShip.Shield.Move(Params.Shield.GetCurrent);
                    if (Params.Shield.GetCurrent == 0) HexShip.ShieldField.SwitchShield(false);
                }
                HexShip.Health.Move(Params.Health.GetCurrent);
                if (States.HaveEnergy) HexShip.Energy.Move(Params.Energy.GetCurrent);
                return new DamageResult(startshield - Params.Shield.GetCurrent, starthealt - Params.Health.GetCurrent, startenergy - Params.Energy.GetCurrent, false);
            }
            else
            {
                if (States.HaveShields)
                {
                    HexShip.Shield.Move(Params.Shield.GetCurrent);
                    if (Params.Shield.GetCurrent == 0) HexShip.ShieldField.SwitchShield(false);
                }
                HexShip.Health.Move(Params.Health.GetCurrent);
                if (States.HaveEnergy) HexShip.Energy.Move(Params.Energy.GetCurrent);
                return new DamageResult(startshield - Params.Shield.GetCurrent, starthealt - Params.Health.GetCurrent, startenergy - Params.Energy.GetCurrent, true);
            }
        }
        public DamageResult RecieveDamage(BattleWeapon weapon, bool IsCrit, double angle, bool isReal)
        {
            DebugWindow.AddTB1(String.Format("Входящий урон {0}с угла {1}", IsCrit ? "с критом " : "", angle), true);
            //TestBattle.WriteString(string.Format("Раунд {0} входящий урон, оружие {1} крит {2} угол {3} сторона {4} ID {5}", Battle.CurrentTurn, weapon.Type.ToString(), IsCrit, angle, IsAttackers ? "Атакующие" : "Защищающиеся", BattleID));
            //TestBattle.WriteString(string.Format("Начальные параметры Щит {0}/{1} Здоровье {2}/{3}",Params.Shield.GetMax,Params.Shield.GetCurrent, Params.Health.GetMax,Params.Health.GetCurrent));
           // TestBattle.AddRecieveDamage(Battle.ID, Battle.CurrentTurn, States.Side, BattleID, (byte)weapon.Type, (short)angle);
            int startshield = Params.Shield.GetCurrent;
            int starthealt = Params.Health.GetCurrent;
            int startenergy=Params.Energy.GetCurrent;
            int plasmamodifier = 1;
            int absorpmodifier = 1;
            int restrictionmodifier = 1;
            bool GaussCrit=false;
            bool SolarCrit = false;
            if (IsCrit && !States.CritProtected)
                switch (weapon.Type)
                {
                    case EWeaponType.Laser: if (!isReal) break; Params.Status.SetBlind(); HexShip.SetBlind(true); break;// TestBattle.AddLaserCrit(Battle.ID); break;
                    case EWeaponType.Plasma: plasmamodifier = 2; break;//if (!isReal) break; new PlasmaCritFlash(HexShip);  break;// TestBattle.AddPlasmaCrit(Battle.ID); break;
                    case EWeaponType.Solar: if (!isReal) break; HexCanvas.MakeFlash(Brushes.White); SolarCrit = true; break;// TestBattle.AddSolarCrit(Battle.ID); break;
                    case EWeaponType.Cannon: if (!isReal) break; 
                        Params.Status.SetConfuse(); HexShip.Blocking.StartStar();break;// TestBattle.AddCannonCrit(Battle.ID); break;
                    case EWeaponType.Gauss: GaussCrit = true; if (!isReal) break; new GaussCritFlash(HexShip); break;// TestBattle.AddGaussCrit(Battle.ID); break;
                    case EWeaponType.Missle: if (!isReal) break; new MissleCritFlash(HexShip.Hex); break;
                    case EWeaponType.AntiMatter: if (!isReal)break; 
                        Params.Status.SetAntiCursed(); HexShip.Blocking.StartAnti(); break;// TestBattle.AddAntiCrit(Battle.ID); break;
                    case EWeaponType.Psi: if (!isReal) break;
                        Params.Status.SetPsiCursed(); HexShip.Blocking.StartPsi(); break;// TestBattle.AddPsiCrit(Battle.ID); break;
                    case EWeaponType.Dark: absorpmodifier = 0; if (!isReal) break; new DarkCritFlash(HexShip); break;// TestBattle.AddDarkCrit(Battle.ID); break;
                    case EWeaponType.Warp: break;
                    case EWeaponType.Slicing: Params.Health.RemovePercent(20, isReal); Params.Shield.RemovePercent(20, isReal); Params.Energy.RemovePercent(20, isReal);
                        if (!isReal) break; new SliceCritFlash(HexShip); break;// TestBattle.AddSliceCrit(Battle.ID); break;
                    case EWeaponType.Radiation: if (!isReal) break; Params.Health.AddDisease(weapon.Damage); HexShip.SetRadiation(true);// HexShip.Health.SetCorrupted();
                        break;//  TestBattle.AddRadCrit(Battle.ID); break;
                    case EWeaponType.Drone: if (!isReal) break; Params.Status.SetMark(); HexShip.SetMarked(true);  break; //new DroneCritFlash(HexShip);  break;
                    case EWeaponType.Magnet: if (!isReal) break; new MagnetCritFlash(HexShip); break;
                }
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
            if (isReal) DebugWindow.AddTB1("", true);
            if (IsShieldResist((int)angle, GaussCrit))
            {

                ShieldDamage = weapon.ToShield() * plasmamodifier * restrictionmodifier;
                int shield = Params.Shield.GetCurrent;
                DamageLeft = Params.Shield.RemoveParam(ShieldDamage, isReal);
                if (isReal) DebugWindow.AddTB1(String.Format("Урон по щиту {0}/{1} ", shield, Params.Shield.GetCurrent), false);
                if (DamageLeft == 0)
                {
                    if (isReal)
                    {
                        //TestBattle.AddShieldBlockDamage(Battle.ID, (short)ShieldDamage, (short)Params.Shield.GetCurrent, (short)Params.Shield.GetMax);
                        HexShip.Shield.Move(Params.Shield.GetCurrent);
                        HexShip.Health.Move(Params.Health.GetCurrent);
                        HexShip.Energy.Move(Params.Energy.GetCurrent);
                        if (Params.Shield.GetCurrent == 0) HexShip.ShieldField.SwitchShield(false);
                    }
                    LastCritEffect(weapon, IsCrit);
                    return new DamageResult(startshield - Params.Shield.GetCurrent, starthealt - Params.Health.GetCurrent, startenergy - Params.Energy.GetCurrent, SolarCrit);
                }
                percentdamage = ((double)DamageLeft) / ShieldDamage * 100.0;
            }
           // else if (isReal)
           //     TestBattle.AddShieldPierce(Battle.ID);
            //int HealthDamage = (int)(weapon.ToHealth() * percentdamage / 100 - Params.Ignore.GetCurValue(weapon.Group) * absorpmodifier);
            int HealthDamage = (int)(weapon.ToHealth() * percentdamage * (100 - Params.Ignore.GetCurValue(weapon.Group)*absorpmodifier) / 10000.0);
            int health = Params.Health.GetCurrent;
            int HealthDamageLeft = Params.Health.RemoveParam(HealthDamage, isReal);
            DebugWindow.AddTB1(String.Format("Урон по хп {0}/{1}", health, Params.Health.GetCurrent), false);
            if (Params.Health.GetCurrent!=0)
            {
                if (isReal)
                {
                    //TestBattle.AddHealthDamageLive(Battle.ID, (short)(ShieldDamage - DamageLeft), (short)(HealthDamage), (short)Params.Health.GetCurrent, (short)Params.Health.GetMax);
                    if (States.HaveShields)
                    {
                        HexShip.Shield.Move(Params.Shield.GetCurrent);
                        if (Params.Shield.GetCurrent == 0) HexShip.ShieldField.SwitchShield(false);
                    }
                    HexShip.Health.Move(Params.Health.GetCurrent);
                    if (States.HaveEnergy) HexShip.Energy.Move(Params.Energy.GetCurrent);
                }
                LastCritEffect(weapon, IsCrit);
                return new DamageResult(startshield - Params.Shield.GetCurrent, starthealt - Params.Health.GetCurrent, startenergy - Params.Energy.GetCurrent, SolarCrit);
            }
            else
            {
                if (isReal)
                {
                    //TestBattle.AddHealthDamageDestroy(Battle.ID, (short)(ShieldDamage - DamageLeft), (short)(HealthDamage - HealthDamageLeft));
                    if (States.HaveShields)
                    {
                        HexShip.Shield.Move(Params.Shield.GetCurrent);
                        if (Params.Shield.GetCurrent == 0) HexShip.ShieldField.SwitchShield(false);
                    }
                    HexShip.Health.Move(Params.Health.GetCurrent);
                    if (States.HaveEnergy) HexShip.Energy.Move(Params.Energy.GetCurrent);
                }
                return new DamageResult(startshield - Params.Shield.GetCurrent, starthealt - Params.Health.GetCurrent, startenergy - Params.Energy.GetCurrent, true);
            }
        }
        public void LastCritEffect(BattleWeapon weapon, bool IsCrit)
        {
            if (IsCrit && !States.CritProtected)
            {
                switch (weapon.Type)
                {
                    //case EWeaponType.Drone: Params.Shield.SetNull(); HexShip.Shield.Move(Params.Shield.GetCurrent); break;// TestBattle.AddDroneCrit(Battle.ID); break;
                    case EWeaponType.Magnet: Angle = (Angle + 180) % 360; HexShip.RotateTo(Angle); break;// TestBattle.AddMagnetCrit(Battle.ID); break;
                }
            }
        }
        public bool IsShieldResist(int DamageAngle, bool IsGaussCrit)
        {
            if (IsGaussCrit) return false;
            if (Params.Shield.GetCurrent == 0) return false;
            int minus = DamageAngle - Angle;
            if (minus > 0 && minus <= 180) { }
            else if (minus > 0 && minus > 180) minus -= 360;
            else if (minus < 0 && minus > -180) { }
            else if (minus < 0 && minus < -180) minus += 360;
            if (Math.Abs(minus) < Params.ShieldProtection) return true; else return false;
        }
        public string[] GetParams()
        {
            List<string> array = new List<string>();
            return array.ToArray();
        }
        public byte[] GetLogInfo()
        {
            List<byte> list = new List<byte>();
            list.Add((byte)States.Side);
            list.Add(BattleID);
            list.Add(CurHex);
            list.AddRange(Params.Health.GetLogInfo());
            list.AddRange(Params.Shield.GetLogInfo());
            list.AddRange(Params.Energy.GetLogInfo());
            if (Pilot != null)
                list.AddRange(Pilot.GetArray());
            else
                list.AddRange(GSPilot.GetNullPilot());
            return list.ToArray();
        }
        public static List<string> GetLogString(byte[] array, ref int i)
        {
            List<string> result = new List<string>();
            bool attack = BitConverter.ToBoolean(array, i); i++;
            string id = array[i].ToString(); i++;
            string hex = array[i].ToString(); i++;
            result.Add(string.Format("{0} ID={1} Hex={2}", (attack ? "АТАКА" : "ЗАЩИТА"), id, hex));
            result.Add(string.Format("ЗДОРОВЬЕ {0}", ValCapParameter.GetLogString(array, ref i)));
            result.Add(string.Format("ЩИТ {0}", ValCapParameter.GetLogString(array, ref i)));
            result.Add(string.Format("ЭНЕРГИЯ {0}", ValCapParameter.GetLogString(array, ref i)));
            string Pilot = GSPilot.GetPilotLogInfo(array, ref i);
            result.Add(Pilot);
            return result;
            //return string.Format("{0} {1} {2}\n{3}\n{4}\n{5}\n{6}", attackstring, id, hex, Health, Shield, Energy, Pilot);
        }
        public override string ToString()
        {
            string Health = "Health " + Params.Health.ToString();
            string Shield = "Shield " + Params.Shield.ToString();
            string Energy = "Energy " + Params.Energy.ToString();
            string ID = "ID " + BattleID.ToString() + States.Side.ToString();
            string hex = "Hex " + CurHex.ToString();
            if (Pilot!=null)
                return string.Format("{0} {1} {2} {3} {4}\n {5}", ID, hex, Health, Shield, Energy, Pilot.ToString());
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
            foreach (BattleWeapon weapon in Weapons)
            {
                if (weapon == null) continue;
                WeaponRating += (weapon.ToShield() * RatingModifiers.ToShieldDamage + weapon.ToHealth() * RatingModifiers.ToHealthDamage + weapon.Accuracy() * RatingModifiers.Accuracy)
                    * ((int)weapon.Size * RatingModifiers.WeaponSize + 1) * RatingModifiers.WeaponType[(int)weapon.Type];
            }
            double EvasionRating = (Params.Evasion.GetCurValue(WeaponGroup.Energy) + Params.Evasion.GetCurValue(WeaponGroup.Physic)
                + Params.Evasion.GetCurValue(WeaponGroup.Irregular) + Params.Evasion.GetCurValue(WeaponGroup.Cyber)) * RatingModifiers.Evasion;
            double AbsorbRating = (Params.Ignore.GetCurValue(WeaponGroup.Energy) + Params.Ignore.GetCurValue(WeaponGroup.Physic)
                + Params.Ignore.GetCurValue(WeaponGroup.Irregular) + Params.Ignore.GetCurValue(WeaponGroup.Cyber)) * RatingModifiers.Absorb;
            double ImmuneRating = (Params.Immune.GetCurValue(WeaponGroup.Energy) + Params.Immune.GetCurValue(WeaponGroup.Physic)
                + Params.Immune.GetCurValue(WeaponGroup.Irregular) + Params.Immune.GetCurValue(WeaponGroup.Cyber)) * RatingModifiers.Immune;
            double TotalRating = HealthRating + ShieldRating + EnergyRating + WeaponRating + EvasionRating + AbsorbRating + ImmuneRating;
            double ResultRating = TotalRating *
                ((int)Schema.Shield.Size * RatingModifiers.ShieldSize + 1) *
                ((int)Schema.Generator.Size * RatingModifiers.GeneratorSize + 1) *
                ((int)Schema.Computer.Size * RatingModifiers.ComputerSize + 1) *
                ((int)Schema.Engine.Size * RatingModifiers.EngineSize + 1) *
                (GroupEffects.Count * RatingModifiers.GroupEffect + 1);
            return (int)(ResultRating * StartHealth / 100);

        }
        /// <summary> Метод показывает, могут ли у корабля быть ходы. Если он парализован или в гейте но не готов к прыжку - то не могут.</summary>
        public bool CanCommand()
        {
            if (States.Controlled == false) return false;
            if (CurHex<=Battle.Field.MaxHex)
            {
                if (Params.Status.IsConfused > 0) return false;
                else return true;
            }
            else
            {
                if (CurHex == 211 || CurHex == 212 || CurHex == 213) return true;
                else return false;
            }
        }
        public int CheckMove()
        {
            foreach (BattleWeapon weapon in Weapons)
            {          
                if (weapon!=null && weapon._IsArmed && weapon.Consume <= Params.Energy.GetCurrent)
                    return 2;
            }
            if (Params.Energy.GetCurrent >= Params.HexMoveCost.GetCurrent)
                return 1;
            return 0;
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
}
