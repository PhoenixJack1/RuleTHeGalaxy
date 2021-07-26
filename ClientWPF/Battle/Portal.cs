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
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Client
{
    class HexShip : Canvas
    {
        Point MiddleGun = new Point(135, 0);
        Point LeftGun = new Point(75, 30);
        Point RightGun = new Point(195, 30);
        Point MiddleGunCenter = new Point(150, 0);
        Point LeftGunCenter = new Point(90, 30);
        Point RightGunCenter = new Point(210, 30);
        Point CenterPoint = new Point(150, 130);
        internal Point CenterShip = new Point(150, 180);
        public ShieldField ShieldField;
        RotateTransform Transform;
        public Hex Hex;
        public double Angle;
        public RoundIndicator Health;
        internal Point HealthPoint;
        public RoundIndicator Shield;
        public RoundIndicator Energy;
        HexShipStatus LeftStatus;
        Point LeftStatusPoint;
        HexShipStatus RightStatus;
        Point RightStatusPoint;
        HexShipStatus IDStatus;
        ShipB Ship;
        Point CocpitPoint;
        public BlockingStar Blocking;
        public HexShip(ShipB ship, byte id, bool IsAtackers)
        {
            Ship = ship;
            Width = 300; Height = 260; RenderTransformOrigin = new Point(0.5, 0.5);
            if (id == 0) return;
            Path path = new Path();
            Children.Add(path);
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 2;
            ShipModelNew shipmodel = ShipModelNew.Models[(byte)(IsAtackers ? 0 : id % 2 + 1)];
            path.Data = shipmodel.GetShipGeometry(ship.Schema.ShipType.WeaponCapacity);
            switch (ship.Schema.ShipType.WeaponCapacity)
            {
                case 1: if (Ship.Schema.Weapons[0] != null) PutGun(shipmodel.MiddleGun, Ship.Schema.Weapons[0]); break;
                case 2: if (Ship.Schema.Weapons[0] != null) PutGun(shipmodel.LeftGun, Ship.Schema.Weapons[0]);
                    if (Ship.Schema.Weapons[1] != null) PutGun(shipmodel.RightGun, Ship.Schema.Weapons[1]); break;
                case 3: if (Ship.Schema.Weapons[0] != null) PutGun(shipmodel.LeftGun, Ship.Schema.Weapons[0]);
                    if (Ship.Schema.Weapons[1] != null) PutGun(shipmodel.MiddleGun, Ship.Schema.Weapons[1]);
                    if (Ship.Schema.Weapons[2] != null) PutGun(shipmodel.RightGun, Ship.Schema.Weapons[2]); break;
            }
            path.Fill = ShipBrush.HullBrush[(byte)(IsAtackers?0:1)];

            Path belt = new Path();
            Children.Add(belt);
            Canvas.SetZIndex(belt, 1);
            belt.Stroke = Brushes.Black;
            belt.Data = shipmodel.GetBeltGeometry();
            belt.Fill = ShipBrush.BeltBrush[(byte)(IsAtackers?2:0)];

            Transform = new RotateTransform();
            RenderTransform = Transform;
            ShieldField = new ShieldField(this, Ship.Params.ShieldProtection);

            IDStatus = new HexShipStatus(IsAtackers ? EHexShipStasus.Attack : EHexShipStasus.Defense, id, Angle);
            Children.Add(IDStatus);
            Canvas.SetZIndex(IDStatus, 1);
            Canvas.SetLeft(IDStatus, shipmodel.IDStatusPoint.X);
            Canvas.SetTop(IDStatus, shipmodel.IDStatusPoint.Y);

            Health = new RoundIndicator(1000, 800, Colors.Red);
            Health.SetCorrupted();
            Children.Add(Health);
            Canvas.SetZIndex(Health, 1);
            Canvas.SetLeft(Health, shipmodel.HealthPoint.X);
            Canvas.SetTop(Health, shipmodel.HealthPoint.Y);

            Shield = new RoundIndicator(2000, 1200, Colors.Blue);
            Shield.SetCorrupted();
            Children.Add(Shield);
            Canvas.SetZIndex(Shield, 1);
            Canvas.SetLeft(Shield, shipmodel.ShieldPoint.X);
            Canvas.SetTop(Shield, shipmodel.ShieldPoint.Y);

            Energy = new RoundIndicator(2000, 500, Colors.Green);
            Energy.SetCorrupted();
            Children.Add(Energy);
            Canvas.SetZIndex(Energy, 1);
            Canvas.SetLeft(Energy, shipmodel.EnergyPoint.X);
            Canvas.SetTop(Energy, shipmodel.EnergyPoint.Y);

            LeftStatusPoint = shipmodel.LeftStatusPoint;
            LeftGunCenter = shipmodel.LeftGunCenter;
            RightGunCenter = shipmodel.RightGunCenter;
            MiddleGunCenter = shipmodel.MiddleGunCenter;
            CenterShip = shipmodel.CenterShip;
            CocpitPoint = shipmodel.CocpitPoint;
            PutCocpit((byte)((cocran.Next(10) << 4) + cocran.Next(12)));

            Blocking = new BlockingStar(CenterShip, shipmodel.StarRadiusX, shipmodel.StarRadiusY);
            Canvas.SetZIndex(path, 1);
            Children.Add(Blocking.StarLabel);
            Children.Add(Blocking.PsiLabel);
            Children.Add(Blocking.AntiLabel);
        }
        static Random cocran = new Random();
        void PutCocpit(byte cocpit)
        {
            byte model = (byte)(cocpit - (cocpit >> 4 << 4));
            byte brush = (byte)(cocpit >> 4);
            Path Cocpit = Cockpit.GetCockpit(model, brush);
            Children.Add(Cocpit);
            Canvas.SetLeft(Cocpit, CocpitPoint.X);
            Canvas.SetTop(Cocpit, CocpitPoint.Y);
            Canvas.SetZIndex(Cocpit, 1);
        }
        void PutGun(Point pt, Weaponclass weapon)
        {
            Canvas Image=new Canvas();
            switch (weapon.Type)
            {
                case EWeaponType.Laser: Image = WeapCanvas.LaserCanvas(); break;
                case EWeaponType.EMI: Image = WeapCanvas.EMICanvas(); break;
                case EWeaponType.Plasma: Image = WeapCanvas.PlasmaCanvas(); break;
                case EWeaponType.Solar: Image = WeapCanvas.SolarCanvas(); break;
                case EWeaponType.Cannon: Image = WeapCanvas.CannonCanvas(); break;
                case EWeaponType.Gauss: Image = WeapCanvas.GaussCanvas(); break;
                case EWeaponType.Missle: Image = WeapCanvas.MissleCanvas(); break;
                case EWeaponType.AntiMatter: Image = WeapCanvas.AntiCanvas(); break;
                case EWeaponType.Psi: Image = WeapCanvas.PsiCanvas(); break;
                case EWeaponType.Dark: Image = WeapCanvas.DarkCanvas(); break;
                case EWeaponType.Warp: Image = WeapCanvas.WarpCanvas(); break;
                case EWeaponType.Time: Image = WeapCanvas.TimeCanvas(); break;
                case EWeaponType.Slicing: Image = WeapCanvas.SliceCanvas(); break;
                case EWeaponType.Radiation: Image = WeapCanvas.RadiationCanvas(); break;
                case EWeaponType.Drone: Image = WeapCanvas.DroneCanvas(); break;
                case EWeaponType.Magnet: Image = WeapCanvas.MagnetCanvas(); break;
            }
            Children.Add(Image);
            Canvas.SetLeft(Image, pt.X);
            Canvas.SetTop(Image, pt.Y);
            Canvas.SetZIndex(Image, 1);
        }
        public TimeSpan MoveTo(Hex hex)
        {
            double x1 = Canvas.GetLeft(this);
            double y1 = Canvas.GetTop(this);
            double x2 = hex.CenterX - this.Width / 2;
            double y2 = hex.CenterY - this.Height / 2;
            double delta = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
            DoubleAnimation leftanim = new DoubleAnimation(x1, x2, TimeSpan.FromSeconds(delta / Links.ShipMoveSpeed));
            DoubleAnimation topanim = new DoubleAnimation(y1, y2, TimeSpan.FromSeconds(delta / Links.ShipMoveSpeed));
            this.BeginAnimation(Canvas.LeftProperty, leftanim);
            this.BeginAnimation(Canvas.TopProperty, topanim);
            Hex = hex;
            return TimeSpan.FromSeconds(delta / Links.ShipMoveSpeed);
        }
        public TimeSpan RotateTo(double newangle)
        {
            double rotate = Common.CalcRotateTo(newangle, Angle);
            double da = Math.Abs(rotate - Angle);
            DoubleAnimation rotateanim = new DoubleAnimation(Angle, rotate, TimeSpan.FromSeconds(da / Links.ShipRotateSpeed + 0.01));
            Transform.BeginAnimation(RotateTransform.AngleProperty, rotateanim);
            Health.RotateTo(Angle - rotate);
            Shield.RotateTo(Angle - rotate);
            Energy.RotateTo(Angle - rotate);
            IDStatus.RotateTo(Angle - rotate);
            if (LeftStatus != null) LeftStatus.RotateTo(Angle - rotate);
            if (RightStatus != null) RightStatus.RotateTo(Angle - rotate);
            Angle = newangle;
            return TimeSpan.FromSeconds(da / Links.ShipRotateSpeed);
        }
        public TimeSpan RotateTo(Hex hex)
        {
            double newangle = Common.CalcAngle(new Point(Hex.CenterX, Hex.CenterY), new Point(hex.CenterX, hex.CenterY));
            return RotateTo(newangle);
        }
        public void SetAngle(double angle)
        {
            Health.SetAngle(Angle - angle);
            Shield.SetAngle(Angle - angle);
            Energy.SetAngle(Angle - angle);
            IDStatus.SetAngle(Angle - angle);
            if (LeftStatus != null) LeftStatus.SetAngle(Angle - angle);
            if (RightStatus != null) RightStatus.SetAngle(Angle - angle);
            Angle = angle;
            Transform.Angle = angle;
        }
        public void MoveToField()
        {
            Children.Remove(LeftStatus);
            LeftStatus = null;
        }
        public void MoveToGate(byte hex)
        {
            Children.Remove(LeftStatus);
            switch (hex)
            {
                case 211: LeftStatus = PlaceLeftStatus(new HexShipStatus(EHexShipStasus.Gate, 1, Angle)); break;
                case 212: LeftStatus = PlaceLeftStatus(new HexShipStatus(EHexShipStasus.Gate, 2, Angle)); break;
                case 213: LeftStatus = PlaceLeftStatus(new HexShipStatus(EHexShipStasus.Gate, 3, Angle)); break;
            }
        }
        public void MoveInQueue(byte hex)
        {
            Children.Remove(LeftStatus);
            switch (hex)
            {
                case 201: LeftStatus = PlaceLeftStatus(new HexShipStatus(EHexShipStasus.Port, 1, Angle)); break;
                case 202: LeftStatus = PlaceLeftStatus(new HexShipStatus(EHexShipStasus.Port, 2, Angle)); break;
            }
        }
        public void MoveShipToPort(byte hex, Hex GateHex, bool IsVisual)
        {
            if (IsVisual)
            {
                switch (hex)
                {
                    case 201: LeftStatus = PlaceLeftStatus(new HexShipStatus(EHexShipStasus.Port, 1, Angle)); break;
                    case 202: LeftStatus = PlaceLeftStatus(new HexShipStatus(EHexShipStasus.Port, 2, Angle)); break;
                    case 203: LeftStatus = PlaceLeftStatus(new HexShipStatus(EHexShipStasus.Port, 3, Angle)); break;
                }
            }
            Hex = GateHex;
        }
        HexShipStatus PlaceLeftStatus(HexShipStatus status)
        {
            Children.Add(status);
            Canvas.SetLeft(status, LeftStatusPoint.X);
            Canvas.SetTop(status, LeftStatusPoint.Y);
            Canvas.SetZIndex(status, 1);
            return status;
        }
        public Point GetMiddleGunPoint()
        {
            double dx = MiddleGunCenter.X - CenterPoint.X;
            double dy = MiddleGunCenter.Y - CenterPoint.Y;
            double X = CenterPoint.X - dy * Math.Sin(Angle * Math.PI / 180) + dx * Math.Cos(Angle * Math.PI / 180);
            double Y = CenterPoint.Y + dy * Math.Cos(Angle * Math.PI / 180) + dx * Math.Sin(Angle * Math.PI / 180);
            return new Point(Canvas.GetLeft(this) + X, Canvas.GetTop(this) + Y);
        }
        public Point GetLeftGunPoint()
        {
            double dx = LeftGunCenter.X - CenterPoint.X;
            double dy = LeftGunCenter.Y - CenterPoint.Y;
            double X = CenterPoint.X - dy * Math.Sin(Angle * Math.PI / 180) + dx * Math.Cos(Angle * Math.PI / 180);
            double Y = CenterPoint.Y + dy * Math.Cos(Angle * Math.PI / 180) + dx * Math.Sin(Angle * Math.PI / 180);
            return new Point(Canvas.GetLeft(this) + X, Canvas.GetTop(this) + Y);
        }
        public Point GetRightGunPoint()
        {
            double dx = RightGunCenter.X - CenterPoint.X;
            double dy = RightGunCenter.Y - CenterPoint.Y;
            double X = CenterPoint.X - dy * Math.Sin(Angle * Math.PI / 180) + dx * Math.Cos(Angle * Math.PI / 180);
            double Y = CenterPoint.Y + dy * Math.Cos(Angle * Math.PI / 180) + dx * Math.Sin(Angle * Math.PI / 180);
            return new Point(Canvas.GetLeft(this) + X, Canvas.GetTop(this) + Y);
        }
        public Point GetShipCenterPoint()
        {
            double dx = CenterShip.X - CenterPoint.X;
            double dy = CenterShip.Y - CenterPoint.Y;
            double X = CenterPoint.X - dy * Math.Sin(Angle * Math.PI / 180) + dx * Math.Cos(Angle * Math.PI / 180);
            double Y = CenterPoint.Y + dy * Math.Cos(Angle * Math.PI / 180) + dx * Math.Sin(Angle * Math.PI / 180);
            double x = Canvas.GetLeft(this);
            double y = Canvas.GetTop(this);
            return new Point(Canvas.GetLeft(this) + X, Canvas.GetTop(this) + Y);
        }
        public void ShieldFlash(double angle, int Shoots, TimeSpan Wavedelay)
        {
            if (ShieldField == null) return;
            ShieldField.ShieldFlash(angle, Angle, Shoots, Wavedelay);
        }
        
    }

    class ShieldField
    {
        Ellipse ShieldEllipse;
        LinearGradientBrush shieldoppbrush;
        RotateTransform ShieldTransform;
        static Color v = Color.FromArgb(255, 255, 255, 255);
        static Color t = Color.FromArgb(0, 0, 0, 0);
        DispatcherTimer Timer;
        List<GradientStop> list;
        static RadialGradientBrush WaveBrush = GetWaveBrush();
        List<Rectangle> Waves = new List<Rectangle>();
        public bool IsVisible = true;
        public ShieldField(HexShip ship, byte ShieldSize)
        {
            ShieldEllipse = new Ellipse();
            ShieldEllipse.Width = 300;
            ShieldEllipse.Height = 300;
            ShieldEllipse.Opacity = 0.8;
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.2);
            brush.GradientStops.Add(new GradientStop(Colors.Blue, 1));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            ShieldEllipse.Fill = brush;
            ship.Children.Add(ShieldEllipse);
            Canvas.SetTop(ShieldEllipse, -20);
            Canvas.SetZIndex(ShieldEllipse, 1);
            shieldoppbrush = new LinearGradientBrush();
            shieldoppbrush.StartPoint = new Point(0.5, 1); shieldoppbrush.EndPoint = new Point(0.5, 0);
            shieldoppbrush.GradientStops.Add(new GradientStop(t, -0.5));
            shieldoppbrush.GradientStops.Add(new GradientStop(t, 1.5));
            ShieldEllipse.OpacityMask = shieldoppbrush;
            ShieldEllipse.RenderTransformOrigin = new Point(0.5, 0.5);
            ShieldTransform = new RotateTransform();
            ShieldEllipse.RenderTransform = ShieldTransform;
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(0.03);
            Timer.Tick += new EventHandler(Timer_Tick);
            list = new List<GradientStop>();
            FlashTimer = new DispatcherTimer();
            FlashTimer.Tick+=new EventHandler(FlashTimer_Tick);
            switch (ShieldSize)
            {
                case 30: Waves.Add(GetWaveRectangle(0)); break;
                case 90: Waves.Add(GetWaveRectangle(0));
                    Waves.Add(GetWaveRectangle(60));
                    Waves.Add(GetWaveRectangle(-60)); break;
                case 150: Waves.Add(GetWaveRectangle(0));
                    Waves.Add(GetWaveRectangle(60));
                    Waves.Add(GetWaveRectangle(-60));
                    Waves.Add(GetWaveRectangle(120));
                    Waves.Add(GetWaveRectangle(-120)); break;
            }
            foreach (Rectangle rect in Waves)
                ship.Children.Add(rect);    
        }
        Rectangle GetWaveRectangle(double angle)
        {
            Rectangle rect = new Rectangle();
            rect.Width = 140; rect.Height = 10; rect.RadiusX = 5; rect.RadiusY = 5;
            rect.Fill = WaveBrush;
            Canvas.SetLeft(rect, 80);
            RotateTransform transform = new RotateTransform();
            transform.CenterX = 70;
            transform.CenterY = 130;
            rect.RenderTransform = transform;
            transform.Angle = angle;
            return rect;
        }
        static RadialGradientBrush GetWaveBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.3);
            brush.GradientStops.Add(new GradientStop(Colors.Blue, 1));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            DoubleAnimation anim = new DoubleAnimation(0, 0.3, TimeSpan.FromSeconds(0.2));
            anim.AutoReverse = true;
            anim.RepeatBehavior = RepeatBehavior.Forever;
            brush.GradientStops[1].BeginAnimation(GradientStop.OffsetProperty, anim);
            return brush;
        }
        void Timer_Tick(object sender, EventArgs e)
        {
            if (list.Count == 0) { Timer.Stop(); return; }
            GradientStop[] arr = list.ToArray();
            foreach (GradientStop gr in arr)
            {
                gr.Offset += 0.05 / Links.ShootAnimSpeed;
                if (gr.Offset > 1.3) { list.Remove(gr); shieldoppbrush.GradientStops.Remove(gr); }
            }
        }
        void AddMove(double length)
        {
            GradientStop gr1 = new GradientStop(t, -length);
            GradientStop gr2 = new GradientStop(v, -length / 2);
            GradientStop gr3 = new GradientStop(t, 0);
            list.Add(gr1); list.Add(gr2); list.Add(gr3);
            shieldoppbrush.GradientStops.Add(gr1);
            shieldoppbrush.GradientStops.Add(gr2);
            shieldoppbrush.GradientStops.Add(gr3);
            if (!Timer.IsEnabled) Timer.Start();
        }
        int curshoot;
        int Shoots;
        DispatcherTimer FlashTimer;
        public void ShieldFlash(double attackangle, double shipangle, int shoots, TimeSpan WaveDelay)
        {
            curshoot = 0; Shoots = shoots;
            ShieldTransform.Angle = attackangle - shipangle;
            if (FlashTimer.IsEnabled) FlashTimer.Stop();
            FlashTimer.Interval = WaveDelay;
            FlashTimer.Start();
        }

        void FlashTimer_Tick(object sender, EventArgs e)
        {
            if (curshoot == Shoots) { FlashTimer.Stop(); return; }
            if (Shoots == 1) AddMove(0.4); else AddMove(0.2);
            curshoot++;
        }
        public void SwitchShield(bool IsOn)
        {
            if (IsOn && IsVisible) return;
            if (IsOn)
            {
                IsVisible = true;
                DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(Links.ShootAnimSpeed*2));
                ElasticEase el = new ElasticEase();
                el.Oscillations = 3;
                el.Springiness = 1;
                anim.EasingFunction = el;
                foreach (Rectangle rect in Waves)
                    rect.BeginAnimation(Rectangle.OpacityProperty, anim);
                return;                
            }
            if (!IsOn && !IsVisible) return;
            if (!IsOn)
            {
                IsVisible = false;
                DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(Links.ShootAnimSpeed*2));
                ElasticEase el = new ElasticEase();
                el.Oscillations = 3;
                el.Springiness = 1;
                anim.EasingFunction = el;
                foreach (Rectangle rect in Waves)
                    rect.BeginAnimation(Rectangle.OpacityProperty, anim);
                return;            
            }
        }
    }
    class PortalB: HexShip
    {
        static RadialGradientBrush PortalBrush = GetBrush();
       
        public PortalB(ShipB ship, byte id, bool isatackers):base(ship, id, isatackers)
        {
            Width = 260;
            Height = 260;
            Children.Clear();
            Ellipse el = new Ellipse();
            el.Fill = PortalBrush;
            el.Width = 260;
            el.Height = 260;
            Children.Add(el);
            
            
            CenterShip = new Point(150, 130);

            HealthPoint = new Point(135, 115);
            Health = new RoundIndicator(1000, 800, Colors.Red);
            Children.Add(Health);
            Canvas.SetLeft(Health, HealthPoint.X - Health.Width / 2);
            Canvas.SetTop(Health, HealthPoint.Y - Health.Height / 2);
        }

        static RadialGradientBrush GetBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Black, 1));
            brush.GradientStops.Add(new GradientStop(Colors.Purple, 0.7));
            brush.GradientStops.Add(new GradientStop(Colors.Violet, 0.4));
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.3));
            DoubleAnimation anim = new DoubleAnimation(0.3, 0.5, TimeSpan.FromSeconds(8));
            anim.AutoReverse = true;
            anim.RepeatBehavior = RepeatBehavior.Forever;
            anim.DecelerationRatio = 1;
            brush.GradientStops[3].BeginAnimation(GradientStop.OffsetProperty, anim);
            DoubleAnimation anim1 = new DoubleAnimation(0.4, 0.6, TimeSpan.FromSeconds(8));
            anim1.AutoReverse = true;
            anim1.RepeatBehavior = RepeatBehavior.Forever;
            anim1.DecelerationRatio = 1;
            brush.GradientStops[2].BeginAnimation(GradientStop.OffsetProperty, anim1);
            return brush;
        }
    }
    class BlockingStar
    {
        static Brush StarBrush = GetStarBrush();
        public Label StarLabel;
        bool IsStarMoving;
        static Brush PsiBrush = GetPsiBrush();
        public Label PsiLabel;
        bool IsPsiMoving;
        static Brush AntiBrush = GetAntiBrush();
        public Label AntiLabel;
        bool IsAntiMoving;
        Point CenterPoint;
        double RadiusX;
        double RadiusY;
        double curXS; double curYS; double alphaS;
        double curXP; double curYP; double alphaP;
        double curXA; double curYA; double alphaA;
        DispatcherTimer timer;
        static Brush GetPsiBrush()
        {
            PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            PathGeometry geom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M0,0 a2,2 0 0,0 4,0 a 4,4 0 0,0 -8,0 a 6,6 0 0,0 12,0 a 8,8 0 0,0 -16,0 a 10,10 0 0,0 20,0 a10,10 0 1,0 0,0.1"));
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.7, 0.3);
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            brush.GradientStops.Add(new GradientStop(Colors.Purple, 0.8));
            brush.GradientStops.Add(new GradientStop(Colors.Black, 1.1));
            Pen pen = new Pen(brush, 1.5);
            pen.StartLineCap = PenLineCap.Round;
            GeometryDrawing draw = new GeometryDrawing(new SolidColorBrush(), pen, geom);
            DrawingBrush result = new DrawingBrush(draw);
            return result;
        }
        static Brush GetAntiBrush()
        {
            PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            PathGeometry geom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M150,130 l6,5 l3,-7 l1,7 l8,-3 l-7,6 l7,2 l-8,2 l3,6 l-6,-5 l-5,6 l2,-8 l-7,2 l6,-5 z"));
            Pen pen = new Pen(Brushes.Black, 0.5);
            pen.LineJoin = PenLineJoin.Round;
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            brush.GradientStops.Add(new GradientStop(Colors.Red, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Black, 1));
            GeometryDrawing draw = new GeometryDrawing(brush, pen, geom);
            DrawingBrush result = new DrawingBrush(draw);
            return result;
        }
        static Brush GetStarBrush()
        {
            PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            PathGeometry geom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M15,0 l4,7 l11,0 l-9,5 l4,8 l-10,-5 l-10,5 l4,-8 l-9,-5 l11,0z"));
            Pen pen = new Pen(Brushes.Black, 0.5);
            pen.LineJoin = PenLineJoin.Round;
            GeometryDrawing draw = new GeometryDrawing(Brushes.Yellow, pen, geom);
            DrawingBrush brush = new DrawingBrush(draw);
            return brush;
        }
        public BlockingStar(Point centerPoint, double radiusX, double radiusY)
        {
            CenterPoint = centerPoint; RadiusX = radiusX; RadiusY = radiusY;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.03);
            timer.Tick += new EventHandler(timer_Tick);

            StarLabel = new Label();
            StarLabel.Width = 30; StarLabel.Height = 20; StarLabel.Background = StarBrush;
            StarLabel.Opacity = 0;
            IsStarMoving = false;

            PsiLabel = new Label();
            PsiLabel.Width = 30; PsiLabel.Height = 30; PsiLabel.Background = PsiBrush;
            PsiLabel.Opacity = 0;
            IsPsiMoving = false;

            AntiLabel = new Label();
            AntiLabel.Width = 30; AntiLabel.Height = 30; AntiLabel.Background = AntiBrush;
            AntiLabel.Opacity = 0;
            IsAntiMoving = false;
        }
        public void StopStar()
        {
            timer.Stop();
            IsStarMoving = false;
            StarLabel.Opacity = 0;
            if (IsPsiMoving)
                StartPsi();
            if (IsAntiMoving)
                StartAnti();
        }
        public void StopPsi()
        {
            IsPsiMoving = false;
            PsiLabel.Opacity = 0;
            if (!IsStarMoving && !IsAntiMoving) timer.Stop();

        }
        public void StopAnti()
        {
            IsAntiMoving = false;
            AntiLabel.Opacity = 0;
            if (!IsStarMoving && !IsPsiMoving) timer.Stop();
        }
        public void StartStar()
        {
            alphaS = 0;
            StarLabel.Opacity = 1;
            Canvas.SetZIndex(StarLabel, 2);
            curYS = CenterPoint.Y;
            curXS = CenterPoint.X + RadiusX / 2;
            IsStarMoving = true;
            if (IsPsiMoving)
            {
                PsiLabel.Opacity = 0;
            }
            if (IsStarMoving)
            {
                AntiLabel.Opacity = 0;
            }
            if (!timer.IsEnabled) timer.Start();
        }
        public void StartAnti()
        {
            alphaA = 0;
            IsAntiMoving = true;
            if (!IsStarMoving)
            {
                AntiLabel.Opacity = 1;
                Canvas.SetZIndex(AntiLabel, 2);
                curYA = CenterPoint.Y + (IsPsiMoving ? 30 : 0);
                curYA = CenterPoint.X + RadiusX / 2;
                if (!timer.IsEnabled) timer.Start();
            }
        }
        public void StartPsi()
        {
            alphaP = 0;
            IsPsiMoving = true;
            if (!IsStarMoving)
            {
                PsiLabel.Opacity = 1;
                Canvas.SetZIndex(PsiLabel, 2);
                curYP = CenterPoint.Y - (IsAntiMoving ? 30 : 0);
                curXP = CenterPoint.X + RadiusX / 2;
                if (!timer.IsEnabled) timer.Start();
            }
        }
        void timer_Tick(object sender, EventArgs e)
        {
            if (IsStarMoving)
            {
                alphaS = (alphaS + 5) % 360;
                curXS = CenterPoint.X + Math.Sin(alphaS / 180 * Math.PI) * RadiusX / 2;
                double curYnS = CenterPoint.Y + Math.Cos(alphaS / 180 * Math.PI) * RadiusY / 2;
                Canvas.SetLeft(StarLabel, curXS - 15);
                Canvas.SetTop(StarLabel, curYnS - 10);
                if (curYnS >= CenterPoint.Y && curYS < CenterPoint.Y) Canvas.SetZIndex(StarLabel, 2);
                if (curYnS <= CenterPoint.Y && curYS > CenterPoint.Y) Canvas.SetZIndex(StarLabel, 0);
                curYS = curYnS;
            }
            else
            {
                if (IsPsiMoving)
                {
                    double dp = IsAntiMoving ? 30 : 0;
                    alphaP = (alphaP + 5) % 360;
                    curXP = CenterPoint.X + Math.Sin(alphaP / 180 * Math.PI) * RadiusX / 2;
                    double curYnP = CenterPoint.Y + Math.Cos(alphaP / 180 * Math.PI) * RadiusY / 2 - dp;
                    Canvas.SetLeft(PsiLabel, curXP - 15);
                    Canvas.SetTop(PsiLabel, curYnP - 15);
                    if (curYnP >= CenterPoint.Y - dp && curYP < CenterPoint.Y - dp) Canvas.SetZIndex(PsiLabel, 2);
                    if (curYnP <= CenterPoint.Y - dp && curYP > CenterPoint.Y - dp) Canvas.SetZIndex(PsiLabel, 0);
                    curYP = curYnP;
                }
                if (IsAntiMoving)
                {
                    double da = IsPsiMoving ? 30 : 0;
                    alphaA = (alphaA + 5) % 360;
                    curXA = CenterPoint.X + Math.Sin(alphaA / 180 * Math.PI) * RadiusX / 2;
                    double curYnA = CenterPoint.Y + Math.Cos(alphaA / 180 * Math.PI) * RadiusY / 2 + da;
                    Canvas.SetLeft(AntiLabel, curXA - 15);
                    Canvas.SetTop(AntiLabel, curYnA - 15);
                    if (curYnA >= CenterPoint.Y + da && curYA < CenterPoint.Y + da) Canvas.SetZIndex(AntiLabel, 2);
                    if (curYnA <= CenterPoint.Y + da && curYA > CenterPoint.Y + da) Canvas.SetZIndex(AntiLabel, 0);
                    curYA = curYnA;
                }
            }
        }
    }
}
