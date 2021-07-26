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
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace Client
{
    class BW
    {
        public static bool Working { get; private set; }
        public static void SetWorking(string text, bool value)
        {
            //if (value==false)
                //Links.Controller.mainWindow.Title = text;
            Working = value;
        }
    }
    class BattleController
    {
        public static bool AutoWork=false;
        //public static bool Working=false;
        public static bool CanCommand { get
            {
                if (AutoWork == true) return false;
                return !BW.Working;
            } }
        public static DispatcherTimer Timer = CreateTimer();
        static TimeSpan WorkingDelay = TimeSpan.Zero;
        public static DispatcherTimer CreateTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.2);
            timer.Tick += new EventHandler(timer_Tick);
            return timer;
        }
       // static long count = 0;
        static void timer_Tick(object sender, EventArgs e)
        {
//            count++;
//            DebugWindow.tb2.Text = count.ToString();
            WorkingDelay -= TimeSpan.FromSeconds(0.2);
            if (AutoWork == false) { Timer.Stop(); return; }
            if (BW.Working == true)  return; 
            if (WorkingDelay <= TimeSpan.Zero)
                IntBoya.Battle.ProcessVisualNextTurn();
            //Links.Controller.mainWindow.Title = HexCanvas.Inner.Children.Count.ToString();
            //Links.Controller.mainWindow.Dispatcher.BeginInvoke(new Action(BattleCanvas.NextTurn));
        }
        public static void AddWorkingDelay(TimeSpan time)
        {
            if (WorkingDelay < time) WorkingDelay = time;
        }
        public static void StartWorking(string reason)
        {
            //CanCommand = false;
            AutoWork = true;
            ClearTempObjects(reason);
            WorkingDelay = TimeSpan.Zero;
            Timer.Start();
        }
        public static void ClearTempObjects(string reason)
        {
            if (IntBoya.Battle.CurMode == BattleMode.Mode2)
            {
                HexCanvas.RemoveInfo();
                HexCanvas.ClearTarget("Передача хода в режиме 2");
                return;
            }
            foreach (HexShip ship in HexCanvas.MovedShips)
                ship.Opacity = 1;
            HexCanvas.MovedShips = new List<HexShip>();
            HexCanvas.ClearTarget("BattleController");
            //HexCanvas.SetBaseParams();
            foreach (HexArrow arrow in HexCanvas.HexArrows)
                HexCanvas.Inner.Children.Remove(arrow);
            foreach (CopyShip copy in HexCanvas.CopyShip)
                HexCanvas.Inner.Children.Remove(copy);
            foreach (KeyValuePair<ShipB, ShipRealParams> pair in HexCanvas.MoveShipsList)
            {
                pair.Key.PutShipToHexNotReal(pair.Value.hex, pair.Value.Angle, pair.Value.CurHex);
                pair.Key.SetEnergy(pair.Value.BasicEnergy, pair.Value.BonusEnergy);
                for (int i = 0; i < 3; i++)
                    if (pair.Key.Weapons[i] != null) pair.Key.Weapons[i].IsArmed(true, "Убирание временных объектов");
            }
            HexCanvas.HexArrows = new List<HexArrow>();
            HexCanvas.CopyShip = new List<CopyShip>();
            //HexCanvas.MouseMoveTarget = MouseMoveTarget.None;
            HexCanvas.MoveShipsList = new SortedList<ShipB, ShipRealParams>();
            IntBoya.Battle.Side1.CurrentCommand = new List<BattleCommand>();
            IntBoya.Battle.Side2.CurrentCommand = new List<BattleCommand>();
            //HexCanvas.HideTargetHex();
            //if (IntBoya.CurrentCommand == ShipCommands.TargetGun)
            //    IntBoya.SetShipCommands(ShipCommands.None);
            //HexCanvas.ClearAims();
            HexCanvas.RemoveInfo();
        }
        #region UseArtefact
        #endregion
        #region ShipEnterToBattle
        static HexShip EnterCopy; static ShipB EnterShip; static Hex EnterHex; static Hex CurrentHex;
        static Point CopyHelpPoint, ShipHelpPoint; 
        static System.Windows.Threading.DispatcherTimer EnterTimer;
        static Rectangle EnterPortal, EnterPortalCopy;
        public static void EnterShipToField(ShipB ship, Hex TargetHex)
        {
            if (Links.ShootAnimSpeed==1)
            new MySound("Battle/Teleport.mp3");
            EnterShip = ship; EnterHex = TargetHex; CurrentHex=ship.HexShip.Hex;
            EnterCopy = EnterShip.HexShip.GetCopy();
            EnterCopy.SetAngle(EnterShip.HexShip.Angle);
            HexCanvas.Inner.Children.Add(EnterCopy);
            EnterCopy.Hex = HexCanvas.Hexes[0];
            ship.HexShip.OpacityMask = Brushes.Transparent;
            ShipHelpPoint = GetBackPoint(TargetHex.CenterPoint, ship.HexShip.Angle + 270, ship.HexShip.Height);
            CopyHelpPoint = GetBackPoint(CurrentHex.CenterPoint, ship.HexShip.Angle + 90, EnterCopy.Height);
            EnterPortalCopy = PutPortal(GetBackPoint(CurrentHex.CenterPoint, ship.HexShip.Angle + 90, EnterCopy.Height/2), ship.HexShip.Angle+90);

            ship.HexShip.MoveToPoint(ship.HexShip.Hex.CenterPoint, ShipHelpPoint, TimeSpan.Zero);
            EnterCopy.MoveToPoint(HexCanvas.Hexes[0].CenterPoint, CurrentHex.CenterPoint, TimeSpan.Zero);

            EnterPortal = PutPortal(GetBackPoint(TargetHex.CenterPoint, ship.HexShip.Angle+270, ship.HexShip.Height/2), ship.HexShip.Angle+90);
            
            if (EnterTimer != null && EnterTimer.IsEnabled) throw new Exception();
            EnterTimer = new System.Windows.Threading.DispatcherTimer();
            EnterTimer.Interval = TimeSpan.FromMilliseconds(25);
            EnterTimer.Tick += new EventHandler(EnterTimer_Tick1);
            EnterTimer.Start();
        }
        static bool leavefromserver;
        public static void ShipReturned(ShipB ship, bool fromserver)
        {
            leavefromserver = fromserver;
            if (Links.ShootAnimSpeed == 1)
                new MySound("Battle/Teleport.mp3");
            EnterShip = ship; CurrentHex = ship.HexShip.Hex;
            ShipHelpPoint = GetBackPoint(CurrentHex.CenterPoint, ship.HexShip.Angle + 90, EnterShip.HexShip.Height);
            EnterPortal = PutPortal(GetBackPoint(CurrentHex.CenterPoint, ship.HexShip.Angle + 90, EnterShip.HexShip.Height/2), ship.HexShip.Angle+90);
            LinearGradientBrush shipbrush = GetTransparentGradient(true);
            TimeSpan time = TimeSpan.FromSeconds(Links.ShootAnimSpeed * 2);
            EnterShip.HexShip.OpacityMask = shipbrush;
            MoveTransparentBrush(shipbrush, time);
            EnterShip.HexShip.MoveToPoint(CurrentHex.CenterPoint, ShipHelpPoint, time);
            if (EnterTimer != null && EnterTimer.IsEnabled) throw new Exception();
            EnterTimer = new System.Windows.Threading.DispatcherTimer();
            EnterTimer.Interval = time;
            EnterTimer.Tick += new EventHandler(ReturnedTimer_Tick);
            EnterTimer.Start();
        }
        static void ReturnedTimer_Tick(object sender, EventArgs e)
        {
            EnterTimer.Stop();
            if (IntBoya.Battle == null) return;
            HexCanvas.Inner.Children.Remove(EnterPortal);
            EnterPortal = null;
            EnterShip.Battle.BattleField.Remove(EnterShip.CurHex);
            //EnterShip.Battle.BattleFieldInfo.Add(string.Format("Корабль сбежал с поля боя, работа таймера Ataka={0} Id={1} Hex={2}", EnterShip.States.Side, EnterShip.BattleID, EnterShip.CurHex));
            EnterShip.Side.RemoveGoodEffectsFromShip(EnterShip, true, null,"покидание2");
            EnterShip.Side.OnField.Remove(EnterShip);
            //ship.HexShip.Opacity = 0;
            System.Windows.Media.Animation.DoubleAnimation anim = new System.Windows.Media.Animation.DoubleAnimation(-1000, TimeSpan.FromSeconds(0));
            EnterShip.HexShip.BeginAnimation(Canvas.LeftProperty, anim);
            EnterShip.SetHex(210);
            EnterShip.HexShip.Hex = null;
            BW.SetWorking("Ship Return from Battle End", false);
            if (IntBoya.CurMode == BattleMode.Mode2 && EnterShip.States.Side == IntBoya.ControlledSide && leavefromserver==false)
            {
                HexCanvas.AddInfo();
            }
            //Working = false;
        }
        static void EnterTimer_Tick1(object sender, EventArgs e)
        {
            EnterTimer.Stop();
            if (IntBoya.Battle == null) return;
            LinearGradientBrush copybrush = GetTransparentGradient(true);
            LinearGradientBrush shipbrush = GetTransparentGradient(false);


            TimeSpan time = TimeSpan.FromSeconds(Links.ShootAnimSpeed*2);

            EnterShip.HexShip.OpacityMask = shipbrush;
            EnterCopy.OpacityMask = copybrush;
            MoveTransparentBrush(shipbrush, time);
            MoveTransparentBrush(copybrush, time);
            EnterCopy.MoveToPoint(CurrentHex.CenterPoint, CopyHelpPoint, time);
            EnterShip.HexShip.MoveToPoint(ShipHelpPoint, EnterHex.CenterPoint, time);
            EnterTimer.Interval = time;
            EnterTimer.Tick -= new EventHandler(EnterTimer_Tick1);
            EnterTimer.Tick += new EventHandler(EnterTimer_Tick2);
            EnterTimer.Start();
        }
        static void EnterTimer_Tick2(object sender, EventArgs e)
        {
            EnterTimer.Stop();
            if (IntBoya.Battle == null) return;
            HexCanvas.Inner.Children.Remove(EnterCopy);
            HexCanvas.Inner.Children.Remove(EnterPortal);
            HexCanvas.Inner.Children.Remove(EnterPortalCopy);
            EnterCopy = null;
            EnterPortal = null;
            EnterPortalCopy = null;
            EnterShip.SetHex(EnterHex.ID);
            EnterShip.HexShip.Hex = EnterHex;
            if (EnterShip.Side!=null)
                EnterShip.Side.FreeGate(EnterShip);
            if (EnterShip.States.Side==ShipSide.Attack)
                EnterShip.Battle.Side1.RecieveGoodEffectsFromShipPlace(EnterShip, true, null);
            else if (EnterShip.States.Side==ShipSide.Defense)
                EnterShip.Battle.Side2.RecieveGoodEffectsFromShipPlace(EnterShip, true, null);
            BW.SetWorking("Ship Enter to Battle End", false);
            if (IntBoya.CurMode == BattleMode.Mode2 && EnterShip.States.Side == IntBoya.ControlledSide)
            {
                HexCanvas.SetTarget(EnterShip);
                HexCanvas.AddInfo();
            }
                //Working = false;
        }
        static void MoveTransparentBrush(LinearGradientBrush brush, TimeSpan time)
        {
            DoubleAnimation anim0 = new DoubleAnimation(-0.01, 1, time);
            DoubleAnimation anim1 = new DoubleAnimation(0, 1.01, time);
            brush.GradientStops[0].BeginAnimation(GradientStop.OffsetProperty, anim0);
            brush.GradientStops[1].BeginAnimation(GradientStop.OffsetProperty, anim1);
        }
        static LinearGradientBrush GetTransparentGradient(bool visual)
        {
            LinearGradientBrush result = new LinearGradientBrush();

            if (visual)
            {
                result.StartPoint = new Point(0.5, 0); result.EndPoint = new Point(0.5, 1);
                result.GradientStops.Add(new GradientStop(Colors.Transparent, -0.01));
                result.GradientStops.Add(new GradientStop(Colors.Black, 0));
                result.GradientStops.Add(new GradientStop(Colors.Black, 2));
            }
            else
            {
                result.StartPoint = new Point(0.5, 0); result.EndPoint = new Point(0.5, 1);
                result.GradientStops.Add(new GradientStop(Colors.Black, -0.01));
                result.GradientStops.Add(new GradientStop(Colors.Transparent, 0));
                result.GradientStops.Add(new GradientStop(Colors.Transparent, 2));
            }
            return result;
        }
        static Rectangle PutPortal(Point pt, double angle)
        {
            Rectangle Portal = new Rectangle();
            //Portal.Width = 50; Portal.Height = 50;
            Portal.Width = 552;
            Portal.Height = 432;
            ImageBrush img = new ImageBrush(Links.Brushes.Ships.PortalTeleport.ImageSource);
            Portal.Fill = img;

            HexCanvas.Inner.Children.Add(Portal);
            Portal.RenderTransformOrigin = new Point(0.5, 0.5);
            Canvas.SetLeft(Portal, pt.X -276);
            Canvas.SetTop(Portal, pt.Y -216);
            //Canvas.SetLeft(Portal, pt.X + 100);
            //Canvas.SetTop(Portal, pt.Y -170 );
            RotateTransform rotate = new RotateTransform(angle);
            ScaleTransform scale = new ScaleTransform(1, 1);
            TransformGroup group = new TransformGroup();
            group.Children.Add(scale);
            group.Children.Add(rotate);
            Portal.RenderTransform = group;
            RectAnimationUsingKeyFrames anim = new RectAnimationUsingKeyFrames();
            anim.Duration = TimeSpan.FromSeconds(Links.ShootAnimSpeed*2.4);
            for (int i = 0; i < 46; i++)
            {
                Rect rect = new Rect(i % 10 * 0.1, (i / 10) / 5.0, 0.1, 1 / 5.0);
                anim.KeyFrames.Add(new DiscreteRectKeyFrame(rect, KeyTime.FromPercent(i / 45.0)));
            }
            img.BeginAnimation(ImageBrush.ViewboxProperty, anim);
            /*Rectangle Portal = new Rectangle();
            //Canvas.SetZIndex(Portal, Links.ZIndex.Ships);
            Portal.Width = 300;
            Portal.Height = 50;
            Portal.Fill = GetPortalBrush();
            HexCanvas.Inner.Children.Add(Portal);
            Portal.RenderTransformOrigin = new Point(0.5, 0.5);
            Canvas.SetLeft(Portal, pt.X - 150);
            Canvas.SetTop(Portal, pt.Y - 25);
            RotateTransform rotate = new RotateTransform(angle + 90);
            ScaleTransform scale = new ScaleTransform(0.1, 1);
            TransformGroup group = new TransformGroup();
            group.Children.Add(scale);
            group.Children.Add(rotate);
            Portal.RenderTransform = group;
            DoubleAnimation anim = new DoubleAnimation(0.1, 1, TimeSpan.FromSeconds(Links.ShootAnimSpeed));
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);*/

            return Portal;
        }
        static RadialGradientBrush GetPortalBrush()
        {
            RadialGradientBrush result = new RadialGradientBrush();
            result.GradientStops.Add(new GradientStop(Colors.Violet, 0));
            result.GradientStops.Add(new GradientStop(Colors.Black, 0.5));
            result.GradientStops.Add(new GradientStop(Colors.Purple, 0.7));
            result.GradientStops.Add(new GradientStop(Colors.Transparent, 1));
            return result;
        }
        static Point GetBackPoint(Point pt, double angle, double width)
        {
            double newangle = (angle + 180) % 360;
            double x = pt.X + width * Math.Cos(newangle * Math.PI / 180);
            double y = pt.Y + width * Math.Sin(newangle * Math.PI / 180);
            return new Point(x, y);
        }
        #endregion
        #region ShipMove
        static DispatcherTimer MoveTimer;
        class MoveShipParams
        {
            public ShipB Ship;
            public Hex TargetHex;
            public byte CurHex;
            public bool FromGate;
            public MoveShipParams(ShipB ship, Hex targetHex, bool fromGate)
            {
                Ship = ship; TargetHex = targetHex; FromGate = fromGate;
            }
        }
        static MoveShipParams MoveShipO;
        public static void MoveShip(ShipB ship, Hex TargetHex, bool FromGate)
        {
            if (Links.ShootAnimSpeed==1)
                new MySound("Battle/Move.wav");
            MoveShipO = new MoveShipParams(ship, TargetHex, FromGate);
            if (FromGate == false) MoveShipO.CurHex = ship.CurHex;
            TimeSpan RotateTime = ship.HexShip.RotateTo(TargetHex);
            if (RotateTime.TotalMilliseconds > 0)
            {
                if (MoveTimer != null && MoveTimer.IsEnabled) throw new Exception();
                MoveTimer = new DispatcherTimer();
                MoveTimer.Interval = RotateTime;
                MoveTimer.Tick += new EventHandler(RotateEnd_Tick);
                MoveTimer.Start();
            }
            else
                MoveShipStep2();
        }

        static void RotateEnd_Tick(object sender, EventArgs e)
        {
            MoveTimer.Stop();
            if (IntBoya.Battle == null) return;
            MoveShipStep2();
        }
        static void MoveShipStep2()
        {
            if (MoveShipO.FromGate == true)
            {
                //MoveShipO.Ship.HexShip.MoveToField();
                //MoveShipO.Ship.CurHex = MoveShipO.TargetHex.ID;
                //MoveShipO.Ship.Angle = MoveShipO.Ship.IsAttackers ? 120 : 300;
            }
            if (MoveShipO.FromGate == false)
            {
                if (MoveShipO.Ship.States.Side==ShipSide.Attack)
                    MoveShipO.Ship.Battle.Side1.RemoveGoodEffectsFromShipMove(MoveShipO.Ship, MoveShipO.TargetHex.ID, true);
                else
                    MoveShipO.Ship.Battle.Side2.RemoveGoodEffectsFromShipMove(MoveShipO.Ship, MoveShipO.TargetHex.ID, true);
                //Links.Controller.mainWindow.Title;
                int distance = IntBoya.Battle.Field.DistAngleRot[MoveShipO.Ship.CurHex, MoveShipO.TargetHex.ID, 6, 0];
                int energycost = (int)(distance / 260.0 * MoveShipO.Ship.Params.HexMoveCost.GetCurrent);
                MoveShipO.Ship.RemoveEnergy(energycost, true);
                //ShipO.Ship.RemoveEnergy(MoveShipO.Ship.Params.HexMoveCost.GetCurrent, true);
            }
            TimeSpan MoveTime = MoveShipO.Ship.HexShip.MoveTo(MoveShipO.TargetHex);
            if (MoveTime.TotalMilliseconds > 0)
            {
                if (MoveTimer != null && MoveTimer.IsEnabled) throw new Exception();
                MoveTimer = new DispatcherTimer();
                MoveTimer.Interval = MoveTime;
                MoveTimer.Tick += new EventHandler(MoveEnd_Tick);
                MoveTimer.Start();
            }
            else
                LastMoveStep();
        }

        static void MoveEnd_Tick(object sender, EventArgs e)
        {
            MoveTimer.Stop();
            if (IntBoya.Battle == null) return;
            LastMoveStep();
        }
        static void LastMoveStep()
        {
            if (MoveShipO.FromGate)
            {
                MoveShipO.Ship.HexShip.RotateTo(MoveShipO.Ship.States.Side==ShipSide.Attack ? 120 : 300);
                MoveShipO.Ship.SetHex(MoveShipO.TargetHex.ID);
                MoveShipO.Ship.Side.FreeGate(MoveShipO.Ship);
                if (MoveShipO.Ship.States.Side==ShipSide.Attack)
                    MoveShipO.Ship.Battle.Side1.RecieveGoodEffectsFromShipPlace(MoveShipO.Ship, true, null);
                else MoveShipO.Ship.Battle.Side2.RecieveGoodEffectsFromShipPlace(MoveShipO.Ship, true, null);
                BW.SetWorking("Ship Enter to Battle End Var2", false);
                //Working = false;
            }
            else
            {
                ShipB ship = MoveShipO.Ship;
                ship.Angle = IntBoya.Battle.Field.DistAngleRot[MoveShipO.CurHex, MoveShipO.TargetHex.ID, 1, 2];
                ship.Battle.BattleField.Remove(MoveShipO.CurHex);
                //ship.Battle.BattleFieldInfo.Add(string.Format("Корабль переместился, работа индикатора Ataka={0} ID={1} HexBefore={2} HexAfrer{3}", ship.States.Side, ship.BattleID, ship.CurHex, MoveShipO.TargetHex.ID));
                ship.SetHex(MoveShipO.TargetHex.ID);
                ship.Params.Status.SetMoving();
                ship.Battle.BattleField.Add(MoveShipO.TargetHex.ID,ship);
                if (ship.States.Side ==ShipSide.Attack)
                    ship.Battle.Side1.RecieveGoodEffectsFromShipMove(MoveShipO.Ship, MoveShipO.CurHex, true);
                else
                    ship.Battle.Side2.RecieveGoodEffectsFromShipMove(ship, MoveShipO.CurHex, true);
                BW.SetWorking("Ship move", false);
                //Working = false;
                //TestBattle.AddShipMove(ship.Battle.ID, ship.Battle.CurrentTurn, ship.BattleID, ship.States.Side, MoveShipO.TargetHex.ID, (short)MoveShipO.Ship.Angle);
                //TestBattle.WriteString(String.Format("{0} Корабль сторона {1} ID {2} переместился в хекс {3} угол {4}",
                //MoveShipO.Ship.Battle.CurrentTurn, MoveShipO.Ship.IsAttackers, MoveShipO.Ship.BattleID, MoveShipO.TargetHex.ID, MoveShipO.Ship.Angle));
            }
            if (IntBoya.CurMode == BattleMode.Mode2 && MoveShipO.Ship.States.Side== IntBoya.ControlledSide)
            {
                HexCanvas.SetTarget(MoveShipO.Ship);
                HexCanvas.AddInfo();
            }

        }
        #endregion
        #region ShipShoot
        static DispatcherTimer FireTimer;
        class MakeWeaponShootParams
        {
            public ShipB Ship;
            public ShipB Target;
            public byte Gun;
            public bool IsMiss;
            public double Angle; //Угол для отображения анимации выстрела а не для расчёта урона
            public int DamageAngle;
            public Hex TargetHex;
            public TimeSpan WaveDelay;
            public int Shoots;
            public bool IsCrit;
            //public DamageResult Result;
            public bool IsGaussCrit;
            public bool SelfDamage;
            public bool IsAsteroid;
            public bool FromServer;
            public byte CurShieldMode;
            public bool SelfSparks = false;
            public MakeWeaponShootParams( ShipB ship, ShipB target, byte gun, bool isMiss, bool isCrit, bool fromserver)
            { Ship = ship; Target = target; Gun = gun; IsMiss = isMiss; IsCrit = isCrit; FromServer = fromserver; }
        }
        static MakeWeaponShootParams MakeWeaponShootO;
        
        /// <summary> метод производящий анимацию выстрела по кораблю противника </summary>
        public static void MakeFire(ShipB ship, ShipB target, byte gun, bool IsMiss, bool IsCrit, bool selfdamage, bool asteroid, bool fromserver)
        {
            if (IsCrit)
                IsCrit = true;
            MakeWeaponShootO = new MakeWeaponShootParams(ship, target, gun, IsMiss, IsCrit, fromserver);
            BattleWeapon weapon = ship.Weapons[gun];
            MakeWeaponShootO.IsGaussCrit = (weapon.Type == EWeaponType.Gauss && IsCrit);
            //MakeWeaponShootO.Result = result;
            MakeWeaponShootO.SelfDamage = selfdamage;//флаг эффекта антипушки
            MakeWeaponShootO.IsAsteroid = asteroid; //флаг что выстрел по астероиду
            Hex targethex = HexCanvas.Hexes[target.CurHex];
            MakeWeaponShootO.TargetHex = targethex;
            int sizedx = ship.States.BigSize ? 3 : 0;
            ship.Angle = IntBoya.Battle.Field.DistAngleRot[ship.CurHex, targethex.ID, gun+sizedx, 2];
            TimeSpan rotateduration = ship.HexShip.RotateTo(targethex);
            if (rotateduration == TimeSpan.Zero) MakeEarlyCritAnim();
            else
            {
                // if (FireTimer != null && FireTimer.IsEnabled)
                //     for (; FireTimer.IsEnabled; )
                //         System.Threading.Thread.Sleep(200);
                if (FireTimer != null && FireTimer.IsEnabled) throw new Exception();
                FireTimer = new DispatcherTimer();
                FireTimer.Interval = rotateduration;
                FireTimer.Tick += new EventHandler(FireTimer_Tick);
                FireTimer.Start();
            }
        }

        static void FireTimer_Tick(object sender, EventArgs e)
        {
            FireTimer.Stop();
            if (IntBoya.Battle == null) return;
            MakeEarlyCritAnim();
        }
        static void MakeEarlyCritAnim()
        {
            BattleWeapon weapon = MakeWeaponShootO.Ship.Weapons[MakeWeaponShootO.Gun];
            if (weapon.Type == EWeaponType.Cannon && MakeWeaponShootO.IsMiss == false && MakeWeaponShootO.IsCrit && MakeWeaponShootO.IsAsteroid == false)
            {
                if (MakeWeaponShootO.SelfDamage)
                    new CannonAnim.CritFlash(HexCanvas.Inner, MakeWeaponShootO.Ship.HexShip.Hex);
                else
                    new CannonAnim.CritFlash(HexCanvas.Inner, MakeWeaponShootO.Target.HexShip.Hex);
            }
            else
                FireCannon();
        }
        //Выполнение анимации, после поворота.
        public static void FireCannon()
        {
            BattleWeapon weapon = MakeWeaponShootO.Ship.Weapons[MakeWeaponShootO.Gun];
            int sizedx = MakeWeaponShootO.Ship.States.BigSize ? 3 : 0;
            //Weaponclass weapon = MakeWeaponShootO.Ship.Schema.GetWeapon(MakeWeaponShootO.Gun);
            if (MakeWeaponShootO.SelfDamage)
            {
                MakeWeaponShootO.Angle = (180+ IntBoya.Battle.Field.DistAngleRot[MakeWeaponShootO.Ship.CurHex, MakeWeaponShootO.Target.CurHex, MakeWeaponShootO.Gun+sizedx, 2])%360;
                MakeWeaponShootO.DamageAngle = IntBoya.Battle.Field.DistAngleRot[MakeWeaponShootO.Ship.CurHex, MakeWeaponShootO.Target.CurHex, MakeWeaponShootO.Gun+sizedx, 2];
                int energy1 = MakeWeaponShootO.Ship.Params.Energy.GetCurrent;
                MakeWeaponShootO.Ship.Params.Energy.RemoveParam(MakeWeaponShootO.Ship.Weapons[MakeWeaponShootO.Gun].Consume, true);
                DebugWindow.AddTB1("Параметры: ", true);
                DebugWindow.AddTB1(String.Format("Энергия = {0} - {1} = {2}, Угол = {3} ", energy1, weapon.Consume, MakeWeaponShootO.Ship.Params.Energy.GetCurrent, MakeWeaponShootO.Ship.Angle), false);
                //MakeWeaponShootO.Ship.SetStats();
                HexCanvas.PutOneDamageLabel(HexCanvas.Hexes[MakeWeaponShootO.Ship.CurHex], DamageLabelTarget.Energy, "-" + MakeWeaponShootO.Ship.Weapons[MakeWeaponShootO.Gun].Consume.ToString());
                MakeWeaponShootO.Ship.Weapons[MakeWeaponShootO.Gun].IsArmed(false, "Разрядка от выстрела по себе");
                //TestBattle.AddEnergySpent(MakeWeaponShootO.Ship.Battle.ID, MakeWeaponShootO.Ship.BattleID, MakeWeaponShootO.Ship.States.Side, (byte)MakeWeaponShootO.Ship.Weapons[MakeWeaponShootO.Gun].Type, (short)MakeWeaponShootO.Ship.Weapons[MakeWeaponShootO.Gun].Consume);
                //TestBattle.WriteString(String.Format("Оружие {0} затрачено энергии {1}", MakeWeaponShootO.Ship.Weapons[MakeWeaponShootO.Gun].Type, MakeWeaponShootO.Ship.Weapons[MakeWeaponShootO.Gun].Consume));
                MakeWeaponShootO.Ship.HexShip.Energy.Move(MakeWeaponShootO.Ship.Params.Energy.GetCurrent);

                switch (weapon.Type)
                {
                    case EWeaponType.Laser:
                        ShieldFlashStarted(Links.ZeroTime, LaserAnim.Shoots(), LaserAnim.WaveDelay(), MakeWeaponShootO.Angle, LaserAnim.ShieldMode); break;
                    case EWeaponType.EMI:
                        ShieldFlashStarted(Links.ZeroTime, EMIAnim.Shoots(), EMIAnim.WaveDelay(), MakeWeaponShootO.Angle, EMIAnim.ShieldMode); break;
                    case EWeaponType.Plasma:
                        ShieldFlashStarted(Links.ZeroTime, PlasmaAnim.Shoots(), PlasmaAnim.WaveDelay(), MakeWeaponShootO.Angle, PlasmaAnim.ShieldMode); break;
                    case EWeaponType.Solar:
                        ShieldFlashStarted(Links.ZeroTime, SolarAnim.Shoots(), SolarAnim.WaveDelay(), MakeWeaponShootO.Angle, SolarAnim.ShieldMode); break;
                    case EWeaponType.Cannon:
                        ShieldFlashStarted(Links.ZeroTime, CannonAnim.Shoots(), CannonAnim.WaveDelay(), MakeWeaponShootO.Angle, CannonAnim.ShieldMode); break;
                    case EWeaponType.Gauss:
                        ShieldFlashStarted(Links.ZeroTime, GaussAnim.Shoots(), GaussAnim.WaveDelay(), MakeWeaponShootO.Angle, GaussAnim.ShieldMode); break;
                    case EWeaponType.Missle:
                        ShieldFlashStarted(Links.ZeroTime, MissleAnim.Shoots(), MissleAnim.WaveDelay(), MakeWeaponShootO.Angle, MissleAnim.ShieldMode); break;
                    case EWeaponType.AntiMatter:
                        ShieldFlashStarted(Links.ZeroTime, AntiAnim.Shoots(), AntiAnim.WaveDelay(), MakeWeaponShootO.Angle, AntiAnim.ShieldMode); break;
                    case EWeaponType.Psi:
                        ShieldFlashStarted(Links.ZeroTime, PsiAnim.Shoots(), PsiAnim.WaveDelay(), MakeWeaponShootO.Angle, PsiAnim.ShieldMode); break;
                    case EWeaponType.Dark:
                        ShieldFlashStarted(Links.ZeroTime, DarkAnim.Shoots(), DarkAnim.WaveDelay(), MakeWeaponShootO.Angle, DarkAnim.ShieldMode); break;
                    case EWeaponType.Warp:
                        ShieldFlashStarted(Links.ZeroTime, WarpAnim.Shoots(), WarpAnim.WaveDelay(), MakeWeaponShootO.Angle, WarpAnim.ShieldMode); break;
                    case EWeaponType.Time:
                        ShieldFlashStarted(Links.ZeroTime, TimeAnim.Shoots(), TimeAnim.WaveDelay(), MakeWeaponShootO.Angle, TimeAnim.ShieldMode); break;
                    case EWeaponType.Slicing:
                        ShieldFlashStarted(Links.ZeroTime, SliceAnim.Shoots(), SliceAnim.WaveDelay(), MakeWeaponShootO.Angle, SliceAnim.ShieldMode); break;
                    case EWeaponType.Radiation:
                        ShieldFlashStarted(Links.ZeroTime, RadAnim.Shoots(), RadAnim.WaveDelay(), MakeWeaponShootO.Angle, RadAnim.ShieldMode); break;
                    case EWeaponType.Drone:
                        ShieldFlashStarted(Links.ZeroTime, DroneAnim.Shoots(), DroneAnim.WaveDelay(), MakeWeaponShootO.Angle, DroneAnim.ShieldMode); break;
                    case EWeaponType.Magnet:
                        ShieldFlashStarted(Links.ZeroTime, MagnetAnim.Shoots(), MagnetAnim.WaveDelay(), MakeWeaponShootO.Angle, MagnetAnim.ShieldMode); break;
                }
                //MakeWeaponShootO.Target = MakeWeaponShootO.Ship;
                //MakeWeaponShootO.TargetHex = HexCanvas.Hexes[MakeWeaponShootO.Ship.CurHex];
                new AntiCritFlash(MakeWeaponShootO.Ship.HexShip, MakeWeaponShootO.Gun);
                if (weapon.Type == EWeaponType.EMI && MakeWeaponShootO.IsCrit)
                {
                    new EMICritFlash(MakeWeaponShootO.Ship.HexShip);
                    ShipB Target = MakeWeaponShootO.Ship;
                    Hex[] Nears = MakeWeaponShootO.Ship.HexShip.Hex.NearHexes;
                    foreach (Hex hex in Nears)
                        if (Target.Battle.BattleField.ContainsKey(hex.ID))
                        {
                            ShipB NextTarget = Target.Battle.BattleField[hex.ID];
                            if (NextTarget.States.Side != Target.States.Side) continue;
                            if (NextTarget.States.CritProtected) continue;
                            int curenergy = NextTarget.Params.Energy.GetCurrent;
                            NextTarget.Params.Energy.AddParam(weapon.Damage);
                            int addenergy = NextTarget.Params.Energy.GetCurrent - curenergy;
                            if (addenergy > 0)
                            {
                                HexCanvas.PutOneDamageLabel(HexCanvas.Hexes[NextTarget.CurHex], DamageLabelTarget.Energy, "+" + addenergy.ToString());
                                NextTarget.HexShip.Energy.Move(NextTarget.Params.Energy.GetCurrent);
                            }
                        }
                    DebugWindow.AddTB1("ЭмиКрит ", false);
                }
                if (weapon.Type == EWeaponType.Time && MakeWeaponShootO.IsCrit)
                {
                    new TimeCritFlash(MakeWeaponShootO.Ship.HexShip);
                    ShipB Target = MakeWeaponShootO.Ship;
                    Hex[] Nears = MakeWeaponShootO.Ship.HexShip.Hex.NearHexes;
                    foreach (Hex hex in Nears)
                        if (Target.Battle.BattleField.ContainsKey(hex.ID))
                        {
                            ShipB NextTarget = Target.Battle.BattleField[hex.ID];
                            if (NextTarget.States.Side != Target.States.Side) continue;
                            if (NextTarget.States.CritProtected) continue;
                            int curhealth = NextTarget.Params.Health.GetCurrent;
                            NextTarget.Params.Health.AddParam(weapon.Damage / 2);
                            int addhealth = NextTarget.Params.Health.GetCurrent - curhealth;
                            if (addhealth > 0)
                            {
                                HexCanvas.PutOneDamageLabel(HexCanvas.Hexes[NextTarget.CurHex], DamageLabelTarget.Health, "+" + addhealth.ToString());
                                NextTarget.HexShip.Health.Move(NextTarget.Params.Health.GetCurrent);
                            }
                        }
                    DebugWindow.AddTB1("ТаймКрит ", false);
                }
                return;
            }
            //weapon = new Weaponclass(20, "5", ItemSize.Small, 100, EWeaponType.Psi, 100, new ItemPrice());
            ShootAnim shoot = new ShootAnim();
            Point shootpoint, targetpoint;
            switch (MakeWeaponShootO.Gun)
            {
                case 0: shootpoint = MakeWeaponShootO.Ship.HexShip.GetLeftGunPoint(); break;
                case 1: shootpoint = MakeWeaponShootO.Ship.HexShip.GetMiddleGunPoint(); break;
                default: shootpoint = MakeWeaponShootO.Ship.HexShip.GetRightGunPoint(); break;
            }
            if (MakeWeaponShootO.IsAsteroid)
            {
                byte AsteroidID = SearchAsteroid(MakeWeaponShootO.Ship.CurHex, MakeWeaponShootO.Target.CurHex, MakeWeaponShootO.Gun, MakeWeaponShootO.Ship.States.BigSize);
                //HexShip asteroidhex = BattleFieldCanvas.Battle.Asteroids[AsteroidID].HexShip;
                targetpoint = IntBoya.Battle.Asteroids[AsteroidID].HexShip.GetShipCenterPoint();
            }
            else
                targetpoint = MakeWeaponShootO.Target.HexShip.GetShipCenterPoint();
            MakeWeaponShootO.DamageAngle = IntBoya.Battle.Field.DistAngleRot[MakeWeaponShootO.Ship.CurHex, MakeWeaponShootO.Target.CurHex, MakeWeaponShootO.Gun+sizedx, 1];
            bool InShield = false;
            if (MakeWeaponShootO.IsMiss == false)
                InShield = MakeWeaponShootO.Target.IsShieldResist(MakeWeaponShootO.DamageAngle, MakeWeaponShootO.IsGaussCrit);
            switch (weapon.Type)
            {
                case EWeaponType.Laser: shoot = new LaserAnim(shootpoint, targetpoint, MakeWeaponShootO.IsMiss); MakeWeaponShootO.SelfSparks = LaserAnim.HaveSelfSparks; break;
                case EWeaponType.Plasma:
                    MakeWeaponShootO.SelfSparks = PlasmaAnim.HaveSelfSparks;
                    shoot = new PlasmaAnim(shootpoint, targetpoint, MakeWeaponShootO.IsMiss, InShield==false, MakeWeaponShootO.IsCrit);
                    break;
                case EWeaponType.EMI: shoot = new EMIAnim(shootpoint, targetpoint, MakeWeaponShootO.IsMiss); MakeWeaponShootO.SelfSparks = EMIAnim.HaveSelfSparks; break;
                case EWeaponType.Solar: shoot = new SolarAnim(shootpoint, targetpoint, MakeWeaponShootO.IsMiss); MakeWeaponShootO.SelfSparks = SolarAnim.HaveSelfSparks; break;
                case EWeaponType.Cannon:
                    MakeWeaponShootO.SelfSparks = CannonAnim.HaveSelfSparks;
                    shoot = new CannonAnim(shootpoint, targetpoint, MakeWeaponShootO.IsMiss, InShield==false);
                     break;
                case EWeaponType.Gauss: shoot = new GaussAnim(shootpoint, targetpoint, MakeWeaponShootO.IsMiss); break;
                case EWeaponType.Missle:
                    MakeWeaponShootO.SelfSparks = MissleAnim.HaveSelfSparks;
                    shoot = new MissleAnim(shootpoint, targetpoint, MakeWeaponShootO.IsMiss, InShield==false); break;
                case EWeaponType.AntiMatter: shoot = new AntiAnim(shootpoint, targetpoint, MakeWeaponShootO.IsMiss); break;
                case EWeaponType.Psi: shoot = new PsiAnim(shootpoint, targetpoint, MakeWeaponShootO.IsMiss); break;
                case EWeaponType.Dark: shoot = new DarkAnim(shootpoint, targetpoint, MakeWeaponShootO.IsMiss); break;
                case EWeaponType.Warp: shoot = new WarpAnim(shootpoint, targetpoint, MakeWeaponShootO.IsMiss); break;
                case EWeaponType.Time: shoot = new TimeAnim(shootpoint, targetpoint, MakeWeaponShootO.IsMiss); break;
                case EWeaponType.Slicing: shoot = new SliceAnim(shootpoint, targetpoint, MakeWeaponShootO.IsMiss); break;
                case EWeaponType.Radiation: shoot = new RadAnim(shootpoint, targetpoint, MakeWeaponShootO.IsMiss); break;
                case EWeaponType.Drone:
                    MakeWeaponShootO.SelfSparks = DroneAnim.HaveSelfSparks;
                    shoot = new DroneAnim(shootpoint, targetpoint, MakeWeaponShootO.IsMiss, InShield==false, (Gun)MakeWeaponShootO.Gun);
                    if (MakeWeaponShootO.IsCrit) ((DroneAnim)shoot).AddCritAnim();
                    break;
                case EWeaponType.Magnet: shoot = new MagnetAnim(shootpoint, targetpoint, MakeWeaponShootO.IsMiss); break;
                default: shoot = new CannonAnim(shootpoint, targetpoint, MakeWeaponShootO.IsMiss, true); break;
            }
            BackMove();
            int energy = MakeWeaponShootO.Ship.Params.Energy.GetCurrent;
            MakeWeaponShootO.Ship.Params.Energy.RemoveParam(MakeWeaponShootO.Ship.Weapons[MakeWeaponShootO.Gun].Consume, true);
            DebugWindow.AddTB1("Параметры: ", true);
            DebugWindow.AddTB1(String.Format("Энергия = {0} - {1} = {2}, Угол = {3} ", energy, weapon.Consume, MakeWeaponShootO.Ship.Params.Energy.GetCurrent, MakeWeaponShootO.Ship.Angle), false);
            //MakeWeaponShootO.Ship.SetStats();
            HexCanvas.PutOneDamageLabel(HexCanvas.Hexes[MakeWeaponShootO.Ship.CurHex], DamageLabelTarget.Energy, "-" + MakeWeaponShootO.Ship.Weapons[MakeWeaponShootO.Gun].Consume.ToString());
            MakeWeaponShootO.Ship.Weapons[MakeWeaponShootO.Gun].IsArmed(false, "Разрядка от выстрела по цели");
           // TestBattle.AddEnergySpent(MakeWeaponShootO.Ship.Battle.ID, MakeWeaponShootO.Ship.BattleID, MakeWeaponShootO.Ship.States.Side, (byte)MakeWeaponShootO.Ship.Weapons[MakeWeaponShootO.Gun].Type, (short)MakeWeaponShootO.Ship.Weapons[MakeWeaponShootO.Gun].Consume);
            //TestBattle.WriteString(String.Format("Оружие {0} затрачено энергии {1}", MakeWeaponShootO.Ship.Weapons[MakeWeaponShootO.Gun].Type, MakeWeaponShootO.Ship.Weapons[MakeWeaponShootO.Gun].Consume));
            MakeWeaponShootO.Ship.HexShip.Energy.Move(MakeWeaponShootO.Ship.Params.Energy.GetCurrent);
            if (weapon.Type == EWeaponType.EMI && MakeWeaponShootO.IsCrit)
            {
                //TestBattle.AddEmiCrit(MakeWeaponShootO.Ship.Battle.ID);
                new EMICritFlash(MakeWeaponShootO.Ship.HexShip);
                ShipB Target = MakeWeaponShootO.Ship;
                Hex[] Nears = MakeWeaponShootO.Ship.HexShip.Hex.NearHexes;
                foreach (Hex hex in Nears)
                    if (Target.Battle.BattleField.ContainsKey(hex.ID))
                    {
                        ShipB NextTarget = Target.Battle.BattleField[hex.ID];
                        if (NextTarget.States.Side != Target.States.Side) continue;
                        if (NextTarget.States.CritProtected) continue;
                        int curenergy = NextTarget.Params.Energy.GetCurrent;
                        NextTarget.Params.Energy.AddParam(weapon.Damage);
                        int addenergy = NextTarget.Params.Energy.GetCurrent - curenergy;
                        if (addenergy>0)
                        {
                            HexCanvas.PutOneDamageLabel(HexCanvas.Hexes[NextTarget.CurHex], DamageLabelTarget.Energy, "+" + addenergy.ToString());
                            NextTarget.HexShip.Energy.Move(NextTarget.Params.Energy.GetCurrent);
                        }
                    }
                DebugWindow.AddTB1("ЭмиКрит ", false);
            }
            if (weapon.Type == EWeaponType.Time && MakeWeaponShootO.IsCrit)
            {
                //TestBattle.AddTimeCrit(MakeWeaponShootO.Ship.Battle.ID);
                new TimeCritFlash(MakeWeaponShootO.Ship.HexShip);
                ShipB Target = MakeWeaponShootO.Ship;
                Hex[] Nears = MakeWeaponShootO.Ship.HexShip.Hex.NearHexes;
                foreach (Hex hex in Nears)
                    if (Target.Battle.BattleField.ContainsKey(hex.ID))
                    {
                        ShipB NextTarget = Target.Battle.BattleField[hex.ID];
                        if (NextTarget.States.Side != Target.States.Side) continue;
                        if (NextTarget.States.CritProtected) continue;
                        int curhealth = NextTarget.Params.Health.GetCurrent;
                        NextTarget.Params.Health.AddParam(weapon.Damage / 2);
                        int addhealth = NextTarget.Params.Health.GetCurrent - curhealth;
                        if (addhealth > 0)
                        {
                            HexCanvas.PutOneDamageLabel(HexCanvas.Hexes[NextTarget.CurHex], DamageLabelTarget.Health, "+" + addhealth.ToString());
                            NextTarget.HexShip.Health.Move(NextTarget.Params.Health.GetCurrent);
                        }
                    }
                DebugWindow.AddTB1("ТаймКрит ", false);
            }
            HexCanvas.Inner.Children.Add(shoot.Canvas);
        }
        public static void BackMove()
        {
            double dx = Canvas.GetLeft(MakeWeaponShootO.Ship.HexShip)-30 * Math.Sin(Math.PI * MakeWeaponShootO.Angle / 180);
            double dy = Canvas.GetTop(MakeWeaponShootO.Ship.HexShip) + 30 * Math.Cos(Math.PI * MakeWeaponShootO.Angle / 180);
            DoubleAnimation animX = new DoubleAnimation(dx, TimeSpan.FromSeconds(Links.ShootAnimSpeed/2));
            DoubleAnimation animY = new DoubleAnimation(dy, TimeSpan.FromSeconds(Links.ShootAnimSpeed/2));
            animX.AutoReverse = true;
            animX.DecelerationRatio = 1;
            animY.AutoReverse = true;
            animY.DecelerationRatio = 1;
            MakeWeaponShootO.Ship.HexShip.BeginAnimation(Canvas.LeftProperty, animX);
            MakeWeaponShootO.Ship.HexShip.BeginAnimation(Canvas.TopProperty, animY);

        }
        public static byte SearchAsteroid(byte shiphex, byte targethex, int weaponpos, bool isShipBig)
        {
            Battle battle = IntBoya.Battle;
            List<byte> Asteroids = new List<byte>();
            int sizedx = isShipBig ? 3 : 0;
            foreach (byte b in battle.Field.IntersectHexes[shiphex, targethex, weaponpos+sizedx])
                if (battle.BattleField.ContainsKey(b))
                    if (battle.BattleField[b].States.Side==ShipSide.Neutral)
                        Asteroids.Add(b);
            if (Asteroids.Count == 1) return Asteroids[0];
            byte curastro = 0;
            int AstroDistance = battle.Field.DistAngleRot[shiphex, Asteroids[0], weaponpos+sizedx, 0];
            for (byte i=1; i<Asteroids.Count; i++)
            {
                int NewDistance = battle.Field.DistAngleRot[shiphex, Asteroids[i], weaponpos+sizedx, 0];
                if (NewDistance<AstroDistance) { AstroDistance = NewDistance; curastro = i; }
            }
            return Asteroids[curastro];
        }
        public static void ShieldFlashStarted(TimeSpan flashdelay, int Shoots, TimeSpan Wavedelay, double Angle, byte mode)
        {
            MakeWeaponShootO.Angle = Angle;
            MakeWeaponShootO.WaveDelay = Wavedelay;
            MakeWeaponShootO.Shoots = Shoots;
            MakeWeaponShootO.CurShieldMode = mode;
            if (flashdelay.TotalMilliseconds > 0)
            {
                FireTimer = new DispatcherTimer();
                FireTimer.Interval = flashdelay;
                FireTimer.Tick += new EventHandler(FireTimerShieldFlash_Tick);
                FireTimer.Start();
            }
            else
            {
                FireTimerShieldFlash_Tick(null, null);
            }
        }
        static void FireTimerShieldFlash_Tick(object sender, EventArgs e)
        {
            if (sender!=null) FireTimer.Stop();
            if (IntBoya.Battle == null) return;
            if (MakeWeaponShootO.IsMiss)
                HexCanvas.PutOneDamageLabel(MakeWeaponShootO.TargetHex, DamageLabelTarget.Miss, "MISS");
            else if (MakeWeaponShootO.IsAsteroid)
                HexCanvas.PutOneDamageLabel(MakeWeaponShootO.TargetHex, DamageLabelTarget.Miss, "BLOCK");
            else
            {
                //if (MakeWeaponShootO.Result.ToShield!=0)
                if (MakeWeaponShootO.SelfDamage)
                {
                    DebugWindow.AddTB1(String.Format("Урон по себе корабля {0} {1} пушка {2}", MakeWeaponShootO.Ship.States.Side == ShipSide.Attack ? "атаки" : "защиты", 
                        MakeWeaponShootO.Ship.BattleID, MakeWeaponShootO.Gun), true);
                    MakeWeaponShootO.Target = MakeWeaponShootO.Ship;
                    MakeWeaponShootO.TargetHex = HexCanvas.Hexes[MakeWeaponShootO.Ship.CurHex];
                }
                else if (MakeWeaponShootO.Ship.States.Side==MakeWeaponShootO.Target.States.Side)
                {
                    DebugWindow.AddTB1(String.Format("Урон по своему кораблю стороны {0} пушка {1}", 
                        MakeWeaponShootO.Ship.States.Side == ShipSide.Attack ? "атаки" : "защиты", MakeWeaponShootO.Gun), true);
                }
                if (MakeWeaponShootO.Target.IsShieldResist(MakeWeaponShootO.DamageAngle, MakeWeaponShootO.IsGaussCrit))
                {
                    MakeWeaponShootO.Target.HexShip.ShieldFlash(MakeWeaponShootO.Angle, MakeWeaponShootO.Shoots, MakeWeaponShootO.WaveDelay);
                }
                else if (MakeWeaponShootO.SelfSparks== false)
                    new BattleSpark(MakeWeaponShootO.Target.HexShip.Hex.CenterPoint);
                if (MakeWeaponShootO.Ship.Weapons[MakeWeaponShootO.Gun].Type == EWeaponType.Missle && MakeWeaponShootO.IsCrit == true)
                {
                    ShipB Target = MakeWeaponShootO.Target;
                    Hex[] Nears = MakeWeaponShootO.TargetHex.NearHexes;
                    foreach (Hex hex in Nears)
                        if (Target.Battle.BattleField.ContainsKey(hex.ID))
                        {
                            ShipB NextTarget = Target.Battle.BattleField[hex.ID];
                            if (NextTarget.States.Side != Target.States.Side) continue;
                            int NextAngle = IntBoya.Battle.Field.DistAngleRot[Target.CurHex, NextTarget.CurHex, 1, 1];
                            if (NextTarget.IsShieldResist(NextAngle, false))
                                NextTarget.HexShip.ShieldFlash(IntBoya.Battle.Field.DistAngleRot[Target.CurHex, NextTarget.CurHex, 1, 2], 1, MakeWeaponShootO.WaveDelay);
                            else
                                new BattleSpark(NextTarget.HexShip.Hex.CenterPoint);
                        }


                }
            }
            // if (FireTimer != null && FireTimer.IsEnabled)
            //    for (; FireTimer.IsEnabled; )
            //        System.Threading.Thread.Sleep(200);
            if (MakeWeaponShootO.WaveDelay.TotalMilliseconds > 0)
            {
                FireTimer = new DispatcherTimer();
                FireTimer.Interval = TimeSpan.FromSeconds(MakeWeaponShootO.Shoots * MakeWeaponShootO.WaveDelay.TotalSeconds);
                FireTimer.Tick += new EventHandler(FireTimerEnd_Tick);
                FireTimer.Start();
            }
            else
            {
                FireTimerEnd_Tick(null, null);
            }
        }
        static void FireTimerEnd_Tick(object sender, EventArgs e)
        {
            if (sender!=null)
                FireTimer.Stop();
            if (IntBoya.Battle == null) return;
            List<ShipB> DestroyedShips = new List<ShipB>();
            if (MakeWeaponShootO.IsMiss == false && MakeWeaponShootO.IsAsteroid==false)
            {
                
                DamageResult result = MakeWeaponShootO.Target.RecieveDamage(MakeWeaponShootO.Ship.Weapons[MakeWeaponShootO.Gun], 
                    MakeWeaponShootO.IsCrit, MakeWeaponShootO.DamageAngle, true);
                if (result.IsDestroyed) DestroyedShips.Add(MakeWeaponShootO.Target);
                //MakeWeaponShootO.Target.SetStats("Параметры защищающегося корабля после получения урона");
                PutDamageLabels(result, MakeWeaponShootO.Target.HexShip.Hex);
                if (result.ToHealth>0 && result.ToShield>0 && MakeWeaponShootO.SelfSparks==false)
                    new BattleSpark(MakeWeaponShootO.Target.HexShip.Hex.CenterPoint);
                if (MakeWeaponShootO.Ship.Weapons[MakeWeaponShootO.Gun].Type == EWeaponType.Missle && MakeWeaponShootO.IsCrit && !MakeWeaponShootO.Target.States.CritProtected)
                {
                    //TestBattle.AddMissleCrit(MakeWeaponShootO.Ship.Battle.ID);
                    //ShipB Target;
                    //if (MakeWeaponShootO.SelfDamage)
                    //    Target = MakeWeaponShootO.Ship;
                    //else
                    ShipB Target = MakeWeaponShootO.Target;
                    Hex[] Nears = MakeWeaponShootO.TargetHex.NearHexes;
                    foreach (Hex hex in Nears)
                        if (Target.Battle.BattleField.ContainsKey(hex.ID))
                        {
                            ShipB NextTarget = Target.Battle.BattleField[hex.ID];
                            //if (MakeWeaponShootO.SelfDamage && NextTarget.IsAttackers == Target.IsAttackers) continue;
                            if (NextTarget.States.Side != Target.States.Side) continue;
                            if (NextTarget.States.CritProtected) continue;
                            int NextAngle = IntBoya.Battle.Field.DistAngleRot[Target.CurHex, NextTarget.CurHex, 1, 1];
                            DamageResult NextResult = NextTarget.RecieveDamage(MakeWeaponShootO.Ship.Weapons[MakeWeaponShootO.Gun], false, NextAngle, true);
                            if (NextResult.IsDestroyed) DestroyedShips.Add(NextTarget);
                            PutDamageLabels(NextResult, NextTarget.HexShip.Hex);
                        }
                }
                if (MakeWeaponShootO.Ship.Weapons[MakeWeaponShootO.Gun].Type == EWeaponType.Warp && MakeWeaponShootO.IsCrit && !MakeWeaponShootO.Target.States.CritProtected && result.IsDestroyed == false)
                {
                    //TestBattle.AddWarpCrit(MakeWeaponShootO.Ship.Battle.ID);
                    new WarpCritFlash(MakeWeaponShootO.Target.HexShip);
                    ShipB target = MakeWeaponShootO.Target;
                    Side side = target.Side;
                    side.RemoveGoodEffectsFromShip(target, true, null,"варп");
                    side.OnField.Remove(target);
                    side.OnBase.Add(target);
                    target.Battle.BattleField.Remove(target.CurHex);
                    //target.Battle.BattleFieldInfo.Add(string.Format("Корабль выкинут варпом, работа таймера Ataka={0} ID={1} Hex={2}", target.States.Side, target.BattleID, target.CurHex));
                    target.HexShip.Hex = null;
                    target.SetHex(200);
                }

            }
            else if (MakeWeaponShootO.IsMiss)
            {
                DebugWindow.AddTB1("Промах", true);
            }
            if (MakeWeaponShootO.FromServer==false)
                foreach (ShipB ship in DestroyedShips)
                {
                    IntBoya.Battle.CurrentTurn++;
                    DebugWindow.AddTB1("Ход " + IntBoya.Battle.CurrentTurn.ToString() + " ", true);
                    IntBoya.Battle.ShipDestroyed(ship.CurHex, true);
                    //DebugWindow.AddTB1(String.Format("Корабль {0} уничтожен", ship.States.Side == ShipSide.Attack ? "атаки" : "защиты"), false);
                }
            BW.SetWorking("Ship Return from Battle End", false);
            if (IntBoya.CurMode == BattleMode.Mode2 && MakeWeaponShootO.Ship.States.Side == IntBoya.ControlledSide)
            {
                HexCanvas.SetTarget(MakeWeaponShootO.Ship);
                HexCanvas.AddInfo();
                HexCanvas.TryShowEventsIcons(MakeWeaponShootO.Gun);
            }
            //Working = false;
        }
        static void PutDamageLabels(DamageResult result, Hex hex)
        {
            int labels = 0;
            if (result.ToShield != 0) labels++;
            if (result.ToHealth != 0) labels++;
            if (result.ToEnergy != 0) labels++;
            switch (labels)
            {
                case 1: if (result.ToShield != 0) HexCanvas.PutOneDamageLabel(hex, DamageLabelTarget.Shield, "-" + result.ToShield.ToString());
                    else if (result.ToHealth != 0) HexCanvas.PutOneDamageLabel(hex, DamageLabelTarget.Health, "-" + result.ToHealth.ToString());
                    else HexCanvas.PutOneDamageLabel(hex, DamageLabelTarget.Energy, "-" + result.ToEnergy.ToString()); break;
                case 2:
                    if (result.ToEnergy == 0) HexCanvas.PutTwoDamageLabel(hex, DamageLabelTarget.Shield, "-" + result.ToShield.ToString(), DamageLabelTarget.Health, "-" + result.ToHealth.ToString());
                    else if (result.ToHealth == 0) HexCanvas.PutTwoDamageLabel(hex, DamageLabelTarget.Shield, "-" + result.ToShield.ToString(), DamageLabelTarget.Energy, "-" + result.ToEnergy.ToString());
                    else HexCanvas.PutTwoDamageLabel(hex, DamageLabelTarget.Health, "-" + result.ToHealth.ToString(), DamageLabelTarget.Energy, "-" + result.ToEnergy.ToString()); break;
                case 3:
                    HexCanvas.PutThreeDamageLabel(hex, DamageLabelTarget.Shield, "-" + result.ToShield.ToString(),
                        DamageLabelTarget.Health, "-" + result.ToHealth.ToString(),
                        DamageLabelTarget.Energy, "-" + result.ToEnergy.ToString()); break;
            }
        }


        #endregion
        #region UseArtefact
        static DestroyCloudBig Cloud;
        static WeaponGroup Group;
        static int ArtDamage;
        static List<Hex> LightJumpHexes;
        static int curjumplight;
        public static void MakeEnergyDamage(byte hex1, bool fromServer, Artefact art)
        {
            Group = WeaponGroup.Energy; ArtDamage = art.Param1;
            FromServer = fromServer;
            Targets = new List<ShipB>();
            Hex1 = HexCanvas.Hexes[hex1];
            List<ShipB> PassiveTargets = new List<ShipB>();
            foreach (ShipB ship in IntBoya.Battle.BattleField.Values)
                if (ship.States.Indestructible == false) PassiveTargets.Add(ship);
            LightJumpHexes = new List<Hex>();
            LightJumpHexes.Add(HexCanvas.Hexes[hex1]);
            Hex LastPoint = Hex1;
            int selecttargets = 0;
            if (IntBoya.Battle.BattleField.ContainsKey(hex1) && IntBoya.Battle.BattleField[hex1].States.Indestructible == false)
            { Targets.Add(IntBoya.Battle.BattleField[hex1]); PassiveTargets.Remove(Targets[0]); selecttargets++; }
            for (int i=selecttargets;i<5;i++)
            {
                if (PassiveTargets.Count == 0) break;
                ShipB NextTarget = PassiveTargets[0]; double distance = Double.MaxValue;
                foreach (ShipB ship in PassiveTargets)
                {
                    byte[] intersecthexes = IntBoya.Battle.Field.IntersectHexes[LastPoint.ID, ship.CurHex, 1];
                    bool errortarget = false;
                    foreach (byte b in intersecthexes)
                    {
                        if (IntBoya.Battle.BattleField.ContainsKey(b) && IntBoya.Battle.BattleField[b].States.Side==ShipSide.Neutral) { errortarget = true; break; }
                    }
                    if (errortarget == true) continue;
                    double d = IntBoya.Battle.Field.DistAngleRot[LastPoint.ID, ship.CurHex, 1, 0];
                    if (d<distance) { NextTarget = ship; distance = d; }
                }
                if (distance == Int32.MaxValue) break;
                else { Targets.Add(NextTarget); LightJumpHexes.Add(NextTarget.HexShip.Hex); LastPoint = NextTarget.HexShip.Hex; PassiveTargets.Remove(NextTarget); }
            }
            if (LightJumpHexes.Count == 0)
            {
                BW.SetWorking("Конец использование артефакта", false);
                HexCanvas.AddInfo();
                return;
            }
            PrismaticLight light1 = new PrismaticLight(LightJumpHexes[0].ID, LightJumpHexes[0].CenterPoint, LightJumpHexes[1].CenterPoint, Targets[0]);
            curjumplight = 1;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = light1.FlyTime;
            timer.Tick += Timer_Tick1;
            timer.Start();
        }

        private static void Timer_Tick1(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
            if (curjumplight+1==LightJumpHexes.Count)
            {
                BW.SetWorking("Конец использование артефакта", false);
                HexCanvas.AddInfo();
                return;
            }
            PrismaticLight light = new PrismaticLight(LightJumpHexes[curjumplight].ID, LightJumpHexes[curjumplight].CenterPoint, LightJumpHexes[curjumplight + 1].CenterPoint, Targets[curjumplight]);
            curjumplight++;
            timer.Interval = light.FlyTime;
            timer.Start();
        }

        class PrismaticLight
        {
            Canvas PathCanvas;
            ScaleTransform Scale;
            double RealLength;
            public TimeSpan FlyTime;
            ShipB Target;
            byte FirstHexID;
            public PrismaticLight(byte firsthexid, Point firstPoint, Point secondPoint, ShipB target)
            {
                Target = target; FirstHexID = firsthexid;
                if (firstPoint.X == secondPoint.X && firstPoint.Y == secondPoint.Y)
                {
                    firstPoint = new Point(0, 0); secondPoint = new Point(1, 1);
                }
                double dy = firstPoint.Y - secondPoint.Y;
                double dx = secondPoint.X - firstPoint.X;
                RealLength = Math.Sqrt(Math.Pow(firstPoint.X - secondPoint.X, 2) + Math.Pow(firstPoint.Y - secondPoint.Y, 2));
                double tga;


                dy = firstPoint.Y - secondPoint.Y;
                dx = secondPoint.X - firstPoint.X;
                double Length = Math.Sqrt(Math.Pow(firstPoint.X - secondPoint.X, 2) + Math.Pow(firstPoint.Y - secondPoint.Y, 2));
                double Angle;
                tga = dy / dx;
                if (dx == 0)
                    Angle = dy > 0 ? 0 : 180;
                else if (dx > 0)
                    Angle = 90 - Math.Atan(tga) * 180 / Math.PI;
                else
                    Angle = 270 - Math.Atan(tga) * 180 / Math.PI;
                PathCanvas = new Canvas();
                Canvas.SetZIndex(PathCanvas, Links.ZIndex.Shoots);
                double width = 40;
                PathCanvas.Width = width;
                PathCanvas.ClipToBounds = true;
                PathCanvas.Height = Length;
                Canvas.SetLeft(PathCanvas, firstPoint.X - PathCanvas.Width / 2);
                Canvas.SetTop(PathCanvas, firstPoint.Y);
                RotateTransform transform = new RotateTransform(Angle + 180);
                PathCanvas.RenderTransformOrigin = new Point(0.5, 0);
                PathCanvas.RenderTransform = transform;
                HexCanvas.Inner.Children.Add(PathCanvas);

                Rectangle beam = new Rectangle();
                beam.Height = Length;
                beam.Width = 30;
                LinearGradientBrush beambrush = new LinearGradientBrush();
                beambrush.StartPoint = new Point(0, 0.5);
                beambrush.EndPoint = new Point(1, 0.5);
                beambrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 255, 255, 0), 1));
                beambrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.5));
                beambrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 255, 255, 0), 0));
                beam.Fill = beambrush;
                Canvas.SetLeft(beam, 5.0);
                beam.RadiusX = 20;
                beam.RadiusY = 20;
                PathCanvas.Children.Add(beam);
                Scale = new ScaleTransform(0, 1);
                beam.RenderTransformOrigin = new Point(0.5, 0.5);
                beam.RenderTransform = Scale;

                DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(Links.ShootAnimSpeed));
                anim.Completed += new EventHandler(anim_Completed);
                Scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
                FlyTime = TimeSpan.FromSeconds((RealLength / 2000 + 1) * Links.ShootAnimSpeed);
            }
            void anim_Completed(object sender, EventArgs e)
            {
                Ellipse el = new Ellipse();
                RadialGradientBrush elbrush = new RadialGradientBrush();
                elbrush.GradientOrigin = new Point(0.7, 0.3);
                elbrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.21));
                elbrush.GradientStops.Add(new GradientStop(Colors.White, 0.2));

                el.Fill = elbrush;

                el.Width = 40;
                el.Height = 100;
                PathCanvas.Children.Add(el);
                Canvas.SetTop(el, -100);
                double time = RealLength / 2000 * Links.ShootAnimSpeed;
                if (time == 0)
                    anim2_Completed(null, null);
                DoubleAnimation anim2 = new DoubleAnimation(-100, RealLength + 100, TimeSpan.FromSeconds(RealLength / 2000 * Links.ShootAnimSpeed));
                if (time>0) anim2.Completed += new EventHandler(anim2_Completed);
                el.BeginAnimation(Canvas.TopProperty, anim2);
                int angle = IntBoya.Battle.Field.DistAngleRot[FirstHexID, Target.CurHex, 1, 1];
                if (Target.IsShieldResist(angle, false))
                {
                    time = (RealLength - 300) / 2000 * Links.ShootAnimSpeed; if (time < 0) { FlashShieldTarget(null, null); return; }
                    DoubleAnimation anim = new DoubleAnimation(1, 1, TimeSpan.FromSeconds(time));
                    anim.Completed += FlashShieldTarget;
                    PathCanvas.BeginAnimation(Canvas.OpacityProperty, anim);
                }
            }

            private void FlashShieldTarget(object sender, EventArgs e)
            {
                int angle = IntBoya.Battle.Field.DistAngleRot[FirstHexID, Target.CurHex, 1, 1];
                Target.HexShip.ShieldFlash(angle + 180, 1, TimeSpan.Zero);
            }

            void anim2_Completed(object sender, EventArgs e)
            {
                int angle = IntBoya.Battle.Field.DistAngleRot[FirstHexID, Target.CurHex, 1, 1];
                DamageResult damage = Target.RecieveDamage(ArtDamage, Group, angle);
                if (damage.IsDestroyed)
                {
                    IntBoya.Battle.CurrentTurn++;
                    DebugWindow.AddTB1("Ход " + IntBoya.Battle.CurrentTurn.ToString() + " ", true);
                    IntBoya.Battle.ShipDestroyed(Target.CurHex, true);
                }
                else
                    PutDamageLabels(damage, Target.HexShip.Hex);
                DoubleAnimation anim3 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5 * Links.ShootAnimSpeed));
                anim3.Completed += new EventHandler(Anim_Completed1);
                Scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim3);
            }
            private void Anim_Completed1(object sender, EventArgs e)
            {
                HexCanvas.Inner.Children.Remove(PathCanvas);
            }
        }
        public static void MakePhysicDamage(byte hex1, bool fromServer, Artefact art)
        {
            Group = WeaponGroup.Physic; ArtDamage = art.Param1;
            FromServer = fromServer;
            if (Links.ShootAnimSpeed == 1)
                new MySound("Battle/Explosion.wav");
            Hex1 = HexCanvas.Hexes[hex1];
            Targets = new List<ShipB>();
            //SortedSet<Hex> hexes = new SortedSet<Hex>(); hexes.Add(Hex1);
            foreach (Hex hex in HexCanvas.Hexes.Values)
            {
                if (IntBoya.Battle.BattleField.ContainsKey(hex.ID) == false) continue;
                if (IntBoya.Battle.BattleField[hex.ID].States.Indestructible == true) continue;
                double d = Math.Sqrt(Math.Pow(hex.CenterX - Hex1.CenterX, 2) + Math.Pow(hex.CenterY - Hex1.CenterY, 2));
                if (d < 550)
                {
                    ShipB ship = IntBoya.Battle.BattleField[hex.ID];
                    int DamageAngle = IntBoya.Battle.Field.DistAngleRot[hex1, ship.CurHex, 1, 1];
                    if (ship.States.HaveShields && ship.IsShieldResist(DamageAngle, false))
                    {
                        ship.HexShip.ShieldFlash(DamageAngle + 180, 1, Links.ZeroTime);
                    }
                    Targets.Add(ship);
                }
            }
            
            Cloud = new DestroyCloudBig(Hex1);
            Cloud.Opacity = 0.8;
            Cloud.anim.Completed += Anim_Completed;
            Cloud.anim.Completed += CyberFlashCompleted;
            Cloud.brush.BeginAnimation(ImageBrush.ViewboxProperty, Cloud.anim);
            /*
            foreach (Hex hex in hexes)
            {
                if (IntBoya.Battle.BattleField.ContainsKey(hex.ID) == false) continue;
                ShipB ship = IntBoya.Battle.BattleField[hex.ID];
                if (ship.States.Indestructible) continue;
                Targets.Add(ship);
                int DamageAngle = IntBoya.Battle.Field.DistAngleRot[hex1, ship.CurHex, 1, 1];
                if (ship.States.HaveShields && ship.IsShieldResist(DamageAngle, false))
                {
                    ship.HexShip.ShieldFlash(DamageAngle + 180, 1, Links.ZeroTime);
                }
            }*/
        }

        private static void Anim_Completed(object sender, EventArgs e)
        {
            HexCanvas.Inner.Children.Remove(Cloud);
        }
        public static void MakeIrregularDamage(ShipSide side, bool fromServer, Artefact art)
        {
            Group = WeaponGroup.Irregular; ArtDamage = art.Param1;
            FromServer = fromServer;
            Targets = new List<ShipB>();
            Restores = new List<ShipB>();
            foreach (ShipB ship in IntBoya.Battle.BattleField.Values)
            {
                if (ship.States.Side == side) Restores.Add(ship);
                else if (ship.States.Indestructible == false) Targets.Add(ship);
            }
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(Links.ShootAnimSpeed * 1.5);
            timer.Tick += IrregularDamageTick;
            HexCanvas.MakeFlash(Brushes.DarkViolet);
            timer.Start();

        }

        private static void IrregularDamageTick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
            List<ShipB> DestroyedShips = new List<ShipB>();
            foreach (ShipB ship in Targets)
            {
                int NextAngle = IntBoya.Battle.Field.DistAngleRot[IntBoya.Battle.Field.MaxHex/2, ship.CurHex, 1, 1];
                DamageResult NextResult = ship.RecieveDamage(ArtDamage, Group, NextAngle);
                if (NextResult.IsDestroyed) DestroyedShips.Add(ship);
                PutDamageLabels(NextResult, ship.HexShip.Hex);
            }
            if (FromServer == false)
                foreach (ShipB ship in DestroyedShips)
                {
                    IntBoya.Battle.CurrentTurn++;
                    DebugWindow.AddTB1("Ход " + IntBoya.Battle.CurrentTurn.ToString() + " ", true);
                    IntBoya.Battle.ShipDestroyed(ship.CurHex, true);
                    //DebugWindow.AddTB1(String.Format("Корабль {0} уничтожен", ship.States.Side == ShipSide.Attack ? "атаки" : "защиты"), false);
                }
            foreach (ShipB ship in Restores)
            {
                int curhealth = ship.Params.Health.GetCurrent;
                ship.Params.Health.AddParam(150);
                int addhealth = ship.Params.Health.GetCurrent - curhealth;
                if (addhealth > 0)
                {
                    HexCanvas.PutOneDamageLabel(HexCanvas.Hexes[ship.CurHex], DamageLabelTarget.Health, "+" + addhealth.ToString());
                    ship.HexShip.Health.Move(ship.Params.Health.GetCurrent);
                }
            }
            HexCanvas.AddInfo();
            BW.SetWorking("Артефакт использован", false);
        }

        static Canvas ArtefactCanvas;
        static List<ShipB> Targets;
        static List<ShipB> Restores;
        static Hex Hex1;
        static bool FromServer;
        public static void MakeCyberDamage(byte hex1, byte hex2, bool fromServer, Artefact art)
        {
            Group = WeaponGroup.Cyber; ArtDamage = art.Param1;
            FromServer = fromServer;
            if (Links.ShootAnimSpeed == 1)
                new MySound("Battle/Radiation.mp3");
            Hex1 = HexCanvas.Hexes[hex1]; Hex Hex2 = HexCanvas.Hexes[hex2];
            List<byte> hexes = new List<byte>(IntBoya.Battle.Field.IntersectHexes[hex1, hex2, 1]);
            hexes.Add(hex1); hexes.Add(hex2);
            Targets = new List<ShipB>();
            foreach (byte hex in hexes)
            {
                if (IntBoya.Battle.BattleField.ContainsKey(hex) == false) continue;
                ShipB ship = IntBoya.Battle.BattleField[hex];
                if (ship.States.Indestructible) continue;
                Targets.Add(ship);
                int DamageAngle = IntBoya.Battle.Field.DistAngleRot[hex1, ship.CurHex, 1, 1];
                if (ship.States.HaveShields && ship.IsShieldResist(DamageAngle, false))
                {
                    ship.HexShip.ShieldFlash(DamageAngle+180, 10, TimeSpan.FromSeconds(0.3 * Links.ShootAnimSpeed));
                }
            }
            Point FirstPoint = Hex1.CenterPoint; Point SecondPoint = Hex2.CenterPoint;
            double dy = FirstPoint.Y - SecondPoint.Y;
            double dx = SecondPoint.X - FirstPoint.X;
            double Length = Math.Sqrt(Math.Pow(FirstPoint.X - SecondPoint.X, 2) + Math.Pow(FirstPoint.Y - SecondPoint.Y, 2));
            double Angle;
            double tga = dy / dx;
            if (dx == 0)
                Angle = dy > 0 ? 0 : 180;
            else if (dx > 0)
                Angle = 90 - Math.Atan(tga) * 180 / Math.PI;
            else
                Angle = 270 - Math.Atan(tga) * 180 / Math.PI;

            ArtefactCanvas = new Canvas();
            Canvas.SetZIndex(ArtefactCanvas, Links.ZIndex.Shoots);
            ArtefactCanvas.Width = 300;
            ArtefactCanvas.ClipToBounds = true;
            HexCanvas.Inner.Children.Add(ArtefactCanvas);
            ArtefactCanvas.Height = Length;
            Canvas.SetLeft(ArtefactCanvas, FirstPoint.X - ArtefactCanvas.Width / 2);
            Canvas.SetTop(ArtefactCanvas, FirstPoint.Y);
            RotateTransform transform = new RotateTransform(Angle + 180);
            ArtefactCanvas.RenderTransformOrigin = new Point(0.5, 0);
            ArtefactCanvas.RenderTransform = transform;
            //ArtefactCanvas.Background = Brushes.White;
            CreatePathes(Length);
            CurCyberFlash = 0;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.3 * Links.ShootAnimSpeed);
            timer.Tick += Timer_Tick;
            timer.Start();
            Timer_Tick(null, null);
        }
        private static void Timer_Tick(object sender, EventArgs e)
        {
            ArtefactCanvas.Children.Add(Pathes[CurCyberFlash]);
            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(Links.ShootAnimSpeed / 2));
            if (CurCyberFlash == 9) { anim.Completed += CyberFlashCompleted; ((DispatcherTimer)sender).Stop(); }
                anim.AutoReverse = true;
            Pathes[CurCyberFlash].BeginAnimation(Path.OpacityProperty, anim);
            CurCyberFlash++;
        }

        private static void CyberFlashCompleted(object sender, EventArgs e)
        {

            List<ShipB> DestroyedShips = new List<ShipB>();
            foreach (ShipB ship in Targets)
            {
                int NextAngle = IntBoya.Battle.Field.DistAngleRot[Hex1.ID, ship.CurHex, 1, 1];
                DamageResult NextResult = ship.RecieveDamage(ArtDamage, Group, NextAngle);
                if (NextResult.IsDestroyed) DestroyedShips.Add(ship);
                PutDamageLabels(NextResult, ship.HexShip.Hex);
            }
            if (FromServer == false)
                foreach (ShipB ship in DestroyedShips)
                {
                    IntBoya.Battle.CurrentTurn++;
                    DebugWindow.AddTB1("Ход " + IntBoya.Battle.CurrentTurn.ToString() + " ", true);
                    IntBoya.Battle.ShipDestroyed(ship.CurHex, true);
                    //DebugWindow.AddTB1(String.Format("Корабль {0} уничтожен", ship.States.Side == ShipSide.Attack ? "атаки" : "защиты"), false);
                }

            HexCanvas.Inner.Children.Remove(ArtefactCanvas);
            HexCanvas.AddInfo();
            BW.SetWorking("Тест", false);
        }

        static Path[] Pathes;
        static int CurCyberFlash = 0;
        static void CreatePathes(double Length)
        {
            Random rnd = new Random();
            int parts = (int)Math.Round(Length / 45, 0);
            if (Length / 45 > parts) parts += 1;
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0, 0.5);
            brush.EndPoint = new Point(1, 0.5);
            brush.GradientStops.Add(new GradientStop(Colors.Green, 0.8));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Green, 0.2));

            int[,] array = new int[parts, 10];
            Pathes = new Path[10];
            for (int j = 0; j < 10; j++)
            {
                int last = 0;
                int val = 0;
                for (int i = 0; i < parts; i++)
                {
                    if (last == 3) { array[i, j] = 2; last = 2; continue; }
                    else if (last == -3) { array[i, j] = -2; last = -2; continue; }
                    val = rnd.Next(2);
                    val = val == 0 ? -1 : 1;
                    last = last + val;
                    array[i, j] = last;
                }
                Pathes[j] = new Path();
                Pathes[j].Stroke = brush;
                Pathes[j].StrokeThickness = 15;
                Pathes[j].StrokeStartLineCap = PenLineCap.Triangle;
                Pathes[j].StrokeEndLineCap = PenLineCap.Triangle;
                PathFigure figure = new PathFigure();
                Point LastPoint = new Point(100, 0);
                figure.StartPoint = LastPoint;
                for (int i = 0; i < parts; i++)
                {
                    Point NewPoint = new Point(100 + array[i, j] * 30, (i + 1) * 45);
                    figure.Segments.Add(new LineSegment(NewPoint, true));
                    LastPoint = NewPoint;
                }
                PathGeometry geom = new PathGeometry(); geom.Figures.Add(figure);
                Pathes[j].Data = geom;
            }

        }
        class DestroyCloudBig : Canvas
        {
            public ImageBrush brush;
            DateTime StartTime = DateTime.Now;
            public RectAnimationUsingKeyFrames anim;
            public DestroyCloudBig(Hex hex)
            {
                brush = Links.Brushes.Explosion;
                Background = brush;
                Width = 1500; Height = 1500;
                HexCanvas.Inner.Children.Add(this);
                Canvas.SetLeft(this, hex.CenterX - 750);
                Canvas.SetTop(this, hex.CenterY - 750);
                Canvas.SetZIndex(this, Links.ZIndex.Shoots);
                anim = new RectAnimationUsingKeyFrames();
                anim.Duration = TimeSpan.FromSeconds(2 * Links.ShootAnimSpeed);
                for (int i = 0; i < 48; i++)
                {
                    Rect rect = new Rect(i % 8 * 0.125, (i / 8) / 6.0, 0.125, 1 / 6.0);
                    anim.KeyFrames.Add(new DiscreteRectKeyFrame(rect, KeyTime.FromPercent(i / 47.0)));
                }
                //anim.Completed += Anim_Completed;
                //brush.BeginAnimation(ImageBrush.ViewboxProperty, anim);
                //cannon.PathCanvas.Children.Add(flash); Canvas.SetLeft(flash, -120); Canvas.SetTop(flash, Length - 170);
            }
           
        }
        #endregion

    }
    class BattleSpark
    {
        static Random rnd = new Random();
        List<Canvas> Rects = new List<Canvas>();
        public BattleSpark(Point pt)
        {
            int count = rnd.Next(15, 25);
            for (int i = 0; i < count; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Width = 5;

                rect.Height = rnd.Next(25, 55);
                /*
                int color = rnd.Next(4);
                switch (color)
                {
                    case 0: rect.Stroke = Brushes.Blue; break;
                    case 1: rect.Stroke = Brushes.Orange; break;
                    case 2: rect.Stroke = Brushes.Purple; break;
                    case 3: rect.Stroke = Brushes.Green; break;
                }
                */
                rect.Stroke = Brushes.Orange;
                //rect.Stroke = Brushes.Green;
                rect.Fill = Brushes.White;
                rect.StrokeThickness = 3;
                rect.Opacity = 1;
                Canvas c = new Canvas();
                Canvas.SetZIndex(c, 150);
                c.Width = 5;
                c.Height = rnd.Next(150, 600);
                c.ClipToBounds = true;
                c.Children.Add(rect);
                RotateTransform rotate = new RotateTransform();
                c.RenderTransformOrigin = new Point(0.5, 0);
                Canvas.SetLeft(c, pt.X - 2.5);
                Canvas.SetTop(c, pt.Y);
                c.RenderTransform = rotate;
                int angle = rnd.Next(0, 360);
                rotate.Angle = angle;
                Rects.Add(c);
                HexCanvas.Inner.Children.Add(c);
                int length = rnd.Next(3);
                if (length == 0)
                {
                    DoubleAnimation anim = new DoubleAnimation(0, 600, TimeSpan.FromSeconds(0.4));
                    anim.DecelerationRatio = 1;
                    anim.Completed += Anim_Completed;
                    rect.BeginAnimation(Canvas.TopProperty, anim);
                }
                else if (length == 1)
                {
                    DoubleAnimation anim = new DoubleAnimation(0, 450, TimeSpan.FromSeconds(0.4));
                    anim.DecelerationRatio = 1;
                    anim.Completed += Anim_Completed;
                    rect.BeginAnimation(Canvas.TopProperty, anim);
                }
                else
                {
                    DoubleAnimation anim = new DoubleAnimation(0, 150, TimeSpan.FromSeconds(0.4));
                    anim.DecelerationRatio = 1;
                    anim.Completed += Anim_Completed;
                    rect.BeginAnimation(Canvas.TopProperty, anim);
                }
            }
        }

        private void Anim_Completed(object sender, EventArgs e)
        {
            foreach (Canvas c in Rects)
                HexCanvas.Inner.Children.Remove(c);
        }
    }
}
