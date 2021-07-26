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
using System.Windows.Controls.Primitives;

namespace Client
{
    partial class HexCanvas
    {
        public static SortedList<byte, Hex> Hexes;
        public static Canvas Inner;
        public static Gate AttackGate;
        public static Gate DefenseGate;
        public static List<HexShip> Ships;
        public static List<HexShip> Asteroids;
        /// <summary> передвинутые корабли </summary>
        public static List<HexShip> MovedShips = new List<HexShip>();
        public static Viewbox Box;
        static Label BigTextLabel;
        static Border BigTextBorder;
        static Grid BigTextGrid;
        public static List<HexArrow> HexArrows = new List<HexArrow>();
        public static List<CopyShip> CopyShip = new List<CopyShip>();
        public static SortedList<ShipB, ShipRealParams> MoveShipsList = new SortedList<ShipB, ShipRealParams>();
        public static List<Canvas> Aim = new List<Canvas>();
        public static Canvas EventsButtonsCanvas;
        /// <summary> Метод вызывается перед началом боя</summary>
        public static void Create3()
        {
            Box.Child = null;
            
            Inner = new Canvas();
            Inner.Width = 20 + 600 + IntBoya.Battle.Field.Columns * 225 + 75;
            Inner.Height = IntBoya.Battle.Field.Rows * 260 + 130;
            Box.Child = Inner;
            EventsButtonsCanvas = new Canvas(); EventsButtonsCanvas.Width = Inner.Width; EventsButtonsCanvas.Height = Inner.Height;
            EventsButtonsCanvas.Background = Links.Brushes.Transparent; Canvas.SetZIndex(EventsButtonsCanvas, 250);
            switch (IntBoya.Battle.Field.Size)
            {
                case BattleFieldSize.Small: ShipEventButton.Size = 220; break;
                case BattleFieldSize.Medium: ShipEventButton.Size = 270; break;
                case BattleFieldSize.Large: ShipEventButton.Size = 400; break;
            }
            Hexes = new SortedList<byte, Hex>();
            for (byte i = 0; i < IntBoya.Battle.Field.Count; i++)
            {
                Hexes.Add(i, new Hex(i, (byte)(i / IntBoya.Battle.Field.Columns), (byte)(i % IntBoya.Battle.Field.Columns)));
                Hexes[i].NearHexes = IntBoya.Battle.Field.Hexes[i].NearHexes;
                Hexes[i].SetD(310, 0);
                Hexes[i].CreatePath();
                Inner.Children.Add(Hexes[i].canvas);
            }

            AttackGate = new Gate(true, 0, 0);
            Inner.Children.Add(AttackGate);
            DefenseGate = new Gate(false, 225 * IntBoya.Battle.Field.Columns + 75 + 320, 130 + (IntBoya.Battle.Field.Rows - 5) * 260);
            Inner.Children.Add(DefenseGate);
            Ships = new List<HexShip>();
            Asteroids = new List<HexShip>();
            BigTextGrid = new Grid();
            //Inner.Children.Add(BigTextGrid);
            Canvas.SetZIndex(BigTextGrid, 200);
            BigTextGrid.Width = Inner.Width;
            BigTextGrid.Height = Inner.Height;

            BigTextBorder = new Border();
            Canvas.SetZIndex(BigTextBorder, Links.ZIndex.BigTablets);
            BigTextGrid.Children.Add(BigTextBorder);
            BigTextBorder.CornerRadius = new CornerRadius(20);
            BigTextBorder.Padding = new Thickness(10);
            BigTextBorder.HorizontalAlignment = HorizontalAlignment.Center;
            BigTextBorder.VerticalAlignment = VerticalAlignment.Center;

            BigTextLabel = new Label();
            BigTextBorder.Child = BigTextLabel;
            BigTextLabel.FontFamily = Links.Font;
            BigTextLabel.FontSize = Inner.Width / 15;
            BigTextLabel.FontWeight = FontWeights.Bold;
            BigTextLabel.Foreground = Brushes.White;

            MovedHexes = new SortedSet<byte>();
            Inner.PreviewMouseDown += Inner_Traditional;
            Inner.MouseMove += MouseMove_Traditional;
        }
        public static void AddEventButtonCanvasRamka(ShipB ship, ShipB target)
        {
            Rectangle rect = Common.GetRectangle(ShipEventButton.Size*2.6, Gets.GetIntBoyaImage("ramka_ataki"));
            Point pt2 = target.HexShip.Hex.CenterPoint;
            Point pt1 = ship.HexShip.Hex.CenterPoint;
            double angle;
            if (pt1.Y == pt2.Y)
                if (pt1.X < pt2.X) angle = 90;
                else angle = -90;
            else if (pt1.X == pt2.X)
                if (pt1.Y > pt2.Y) angle = 0;
                else    angle = 180;
            else
            {
                double K = (pt2.Y - pt1.Y) / (pt2.X - pt1.X);
                angle = Math.Atan(K) / Math.PI * 180+90;
            }
            EventsButtonsCanvas.Children.Add(rect);
            Canvas.SetLeft(rect, pt2.X -rect.Width/2); Canvas.SetTop(rect, pt2.Y - rect.Height/2);
            rect.RenderTransformOrigin = new Point(0.5, 0.5);
            rect.RenderTransform = new RotateTransform(angle);
        }
        

        /// <summary> Вызывается при начале игры </summary>
        public static void Create()
        {
            Box = new Viewbox();
            Box.Stretch = Stretch.Fill;
            Box.Width = 1700;
            Box.Height = 790;
        }
        static ShipB MovedShip;
        static SortedSet<byte> MovedHexes;
        static TextShipInfo MouseMoveInfo;
        static HexArrow TempArrow;
        static int TempAngle = Int32.MaxValue;
        static void PutMouseMoveInfo(ShipB Gunner, ShipB Target)
        { 
            if (Gunner != null)
            {
                if (Gunner.CurHex > IntBoya.Battle.Field.MaxHex) return;
                int tag = 0;
                bool havegun = false;
                bool allblock = true;
                if (CheckWeaponAvailable(Gunner, 0))
                {
                    havegun = true;
                    tag = CalcMouseMoveInfoID(0, Gunner.BattleID, Target.BattleID);
                    int acc = CalculateAccuracy(Gunner, Target, 0);
                    if (acc>0)
                    {
                        TempArrow = PlaceArrow(Gunner, Target, 0);
                        PutMouseMoveInfo(Target, tag, acc.ToString() + "%");
                        return;
                    }
                    else if (acc != Int32.MinValue) allblock = false;
                }
                if (CheckWeaponAvailable(Gunner, 1))
                {
                    havegun = true;
                    tag = CalcMouseMoveInfoID(1, Gunner.BattleID, Target.BattleID);
                    int acc = CalculateAccuracy(Gunner, Target, 1);
                    if (acc > 0)
                    {
                        TempArrow = PlaceArrow(Gunner, Target, 1);
                        PutMouseMoveInfo(Target, tag, acc.ToString() + "%");
                        return;
                    }
                    else if (acc != Int32.MinValue) allblock = false;
                }
                if (CheckWeaponAvailable(Gunner, 2))
                {
                    havegun = true;
                    tag = CalcMouseMoveInfoID(2, Gunner.BattleID, Target.BattleID);
                    int acc = CalculateAccuracy(Gunner, Target, 2);
                    if (acc > 0)
                    {
                        TempArrow = PlaceArrow(Gunner, Target, 2);
                        PutMouseMoveInfo(Target, tag, acc.ToString() + "%");
                        return;
                    }
                    else if (acc != Int32.MinValue) allblock = false;
                }
                if (havegun)
                {
                    if (allblock)
                        PutMouseMoveInfo(Target, tag, "block");
                    else
                        PutMouseMoveInfo(Target, tag, "-----");
                }
            }
        }
        public static void PopUpOpen(object sender, MouseEventArgs e)
        {

            
            HexShip ship = (HexShip)sender;
            HexCanvasPopup.Place(ship);
        }
        public static void PopUpCloseDirect()
        {
            HexCanvasPopup.Remove();
        }
        public static void PopUpClose(object sender, MouseEventArgs e)
        {
            HexCanvasPopup.Remove();
        }
        static void PutMouseMoveInfo(ShipB Target, int tag, string text)
        {
            if (MouseMoveInfo!= null)
            {
                int mousetag = (int)MouseMoveInfo.Tag;
                if (mousetag == tag) return;
                else Inner.Children.Remove(MouseMoveInfo);
            }
            MouseMoveInfo = new TextShipInfo(text, Target.HexShip);
            MouseMoveInfo.Tag = tag;
        }
        static int CalcMouseMoveInfoID(byte gun, byte ship1, byte ship2)
        {
            return BitConverter.ToInt32(new byte[] { gun, (byte)(IntBoya.ControlledSide), ship1, ship2 }, 0);
        }
        static HexShip CurLightedShip = null;
        static void MouseMove_Traditional(object sender, MouseEventArgs e)
        {
            //Подсвечивает корабль, на который наведена мышка
            bool selectlight = false; 
            foreach (HexShip ship in Ships)
            {
                HitTestResult result = VisualTreeHelper.HitTest(ship.SelectPath, Mouse.GetPosition(ship.SelectPath));
                if (result != null)
                {
                    if (ship == CurLightedShip) return;
                    if (CurLightedShip!= null) CurLightedShip.Lighted(false);
                    ship.Lighted(true);
                    CurLightedShip = ship;
                    selectlight = true;
                    break;
                }
            }
            foreach (HexShip astro in Asteroids)
            {
                HitTestResult result = VisualTreeHelper.HitTest(astro.SelectPath, Mouse.GetPosition(astro.SelectPath));
                if (result!= null)
                {
                    if (astro == CurLightedShip) return;
                    if (CurLightedShip != null) CurLightedShip.Lighted(false);
                    astro.Lighted(true);
                    CurLightedShip = astro;
                    selectlight = true;
                    break;
                }
            }
            if (selectlight== false && CurLightedShip!= null) { CurLightedShip.Lighted(false); CurLightedShip = null; }
            if (TempArrow!= null)
            {
                Inner.Children.Remove(TempArrow);
                TempArrow = null;
                if (TempAngle != Int32.MaxValue && MovedShip!=null)
                { MovedShip.SetAngle(TempAngle, true); MovedShip.Angle = TempAngle; TempAngle = Int32.MaxValue; }
            }
            if (!BattleController.CanCommand) return;
            //Размещает вспомогательную ифнормацию на корабле, на котором размещена мышка
            foreach (HexShip ship in Ships)
            {
                //if (ship.Ship.States.Side==Ship)
                if (ship.Ship.States.Side == IntBoya.ControlledSide) continue;
                if (ship.Ship.CurHex > IntBoya.Battle.Field.MaxHex) continue;
                HitTestResult result = VisualTreeHelper.HitTest(ship.SelectPath, Mouse.GetPosition(ship.SelectPath));
                if (result != null)
                {
                    PutMouseMoveInfo(MovedShip, ship.Ship);
                    return;
                }
            }
            if (MouseMoveInfo != null)
            {
                Inner.Children.Remove(MouseMoveInfo);
                MouseMoveInfo = null;
            }
            //Покзывает область действия артефакта
            if (ActiveArtefact!= null)
            {
                foreach (Hex hex in Hexes.Values)
                    hex.SetBright(BrightSelect.None);
                foreach (Hex hex in Hexes.Values)
                {
                    HitTestResult result = VisualTreeHelper.HitTest(hex.path, Mouse.GetPosition(hex.path));
                    if (result != null)
                    {
                        byte[] hexes = BattleArtefact.GetSelectHexes(ActiveArtefact.Ability, FirstArtefactPoint, hex.ID);
                        foreach (byte b in hexes)
                            Hexes[b].SetBright(BrightSelect.Jump);
                        return;
                    }
                }
                
            }
            //Показывает энергию, которая будет затрачена на перемещение
            /*
            if (MovedShip == null) return;
            if (MovedShip.CurHex > IntBoya.Battle.Field.MaxHex) return;

            foreach (byte b in MovedHexes)
            {
                Hex hex = Hexes[b];
                HitTestResult result = VisualTreeHelper.HitTest(hex.path, Mouse.GetPosition(hex.path));
                if (result != null)
                {
                    int distance = IntBoya.Battle.Field.DistAngleRot[MovedShip.CurHex, hex.ID, 6, 0];
                    int energycost = (int)(distance / 250.0 * MovedShip.Params.HexMoveCost.GetCurrent);
                    MovedShip.HexShip.Energy.SetSelecting(energycost);
                    return;
                }
            }
            MovedShip.HexShip.Energy.RemoveSelecting();
            */
           
        }
        /// <summary> метод устанавливает иконки с выстрелами заново, если нужно </summary>
        public static void TryShowEventsIcons(int shootinggun)
        {
            if (ShipEventButton.CurShip1 == null) return;
            if (MovedShip == null) return;
            if (MovedShip.CurHex > IntBoya.Battle.Field.MaxHex) return;
            if (ShipEventButton.CurShip2.CurHex > IntBoya.Battle.Field.MaxHex) return;
            ShipB ship = ShipEventButton.CurShip2;
            List<ShipEvents> list = new List<ShipEvents>();
            SortedList<ShipEvents, bool> Events = new SortedList<ShipEvents, bool>();
            bool needcontinue = false;
            if (MovedShip.Weapons[0] != null) { Events.Add(ShipEvents.GunLeft, CheckWeaponAvailable(MovedShip, 0) && CalculateAccuracy(MovedShip, ship, 0) > 0 && shootinggun!=0); if (Events[ShipEvents.GunLeft] == true) needcontinue = true; }
            if (MovedShip.Weapons[1] != null) { Events.Add(ShipEvents.GunMiddle, CheckWeaponAvailable(MovedShip, 1) && CalculateAccuracy(MovedShip, ship, 1) > 0 && shootinggun!=1); if(Events[ShipEvents.GunMiddle] == true) needcontinue = true; }
            if (MovedShip.Weapons[2] != null) { Events.Add(ShipEvents.GunRight, CheckWeaponAvailable(MovedShip, 2) && CalculateAccuracy(MovedShip, ship, 2) > 0 && shootinggun!=2); if(Events[ShipEvents.GunRight] == true) needcontinue = true; }
            if (needcontinue == false) return;
            Events.Add(ShipEvents.No, true);
            for (;;)
            {
                if (Events.Count == 0) break;
                bool havetrue = false;
                foreach (KeyValuePair<ShipEvents, bool> pair in Events)
                    if (pair.Value == true) { list.Add(pair.Key); Events.Remove(pair.Key); havetrue = true; break; }
                if (havetrue == false) { list.Add(Events.Keys[0]); Events.RemoveAt(0); }
            }
            EventsButtonsCanvas.Children.Clear();
            for (int i = 0; i < list.Count; i++)
                new ShipEventButton(MovedShip, ship, list[i], (ShipEventsPos)i);
            Inner.Children.Add(EventsButtonsCanvas);
            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));
            anim.AccelerationRatio = 1;
            EventsButtonsCanvas.BeginAnimation(Canvas.OpacityProperty, anim);
        }
        static Artefact ActiveArtefact;
        static byte ActiveArtefactPos;
        //static List<ArtefactLight> ArtefactLights;
        static bool FirstArtefactPointSelected = false;
        static byte FirstArtefactPoint = 255;
        static byte SecondArtefactPoint = 255;
        public static void UseArtefact(Artefact artefact, byte pos)
        {
            ClearTarget("Активаация артефакта");
            ActiveArtefact = artefact;
            ActiveArtefactPos = pos;
            if (artefact.Battle.FirstTarget == BattleArtefactUse.No)
                ActivateArtefact();
            //else
            //    LightForArtefact(artefact.Battle.FirstTarget);
            FirstArtefactPoint = 255;
            SecondArtefactPoint = 255;
            FirstArtefactPointSelected = false;
        }
        /*
        public static void LightForArtefact(BattleArtefactUse use)
        {
            if (ArtefactLights != null)
                foreach (ArtefactLight light in ArtefactLights)
                {
                    Inner.Children.Remove(light);
                }
            List<ArtefactLight> lights = new List<ArtefactLight>();
            if (use==BattleArtefactUse.Asteroid)
            {
                foreach (HexShip ship in Asteroids)
                {
                    if (ship.Ship.Model[0] == 254) continue;
                    ArtefactLight astrolight = new ArtefactLight(true, false, ship.Hex, Brushes.White);
                    lights.Add(astrolight);
                    Inner.Children.Add(astrolight);
                }   
            }
            if (use == BattleArtefactUse.AllHex)
            {
                foreach (Hex hex in Hexes.Values)
                {
                    ArtefactLight allhexlight = new ArtefactLight(false, false, hex, Brushes.White);
                    lights.Add(allhexlight);
                    Inner.Children.Add(allhexlight);
                }
            }
            if (use==BattleArtefactUse.FreeHex)
            {
                foreach (Hex hex in Hexes.Values)
                {
                    if (IntBoya.Battle.BattleField.ContainsKey(hex.ID)) continue;
                    ArtefactLight hexlight = new ArtefactLight(false, false, hex, Brushes.White);
                    lights.Add(hexlight);
                    Inner.Children.Add(hexlight);
                }
            }
            if (use==BattleArtefactUse.AnyShip)
            {
                foreach (HexShip ship in Ships)
                {
                    if (ship.Ship.CurHex > IntBoya.Battle.Field.MaxHex) continue;
                    ArtefactLight allshiplight;
                    if (ship.Ship.States.Side == ShipSide.Attack) allshiplight = new ArtefactLight(true, ship.Ship.States.BigSize, ship.Hex, Brushes.Red);
                    else if (ship.Ship.States.Side == ShipSide.Defense) allshiplight = new ArtefactLight(true, ship.Ship.States.BigSize, ship.Hex, Brushes.Green);
                    else allshiplight = new ArtefactLight(true, ship.Ship.States.BigSize, ship.Hex, Brushes.White);
                    lights.Add(allshiplight);
                    Inner.Children.Add(allshiplight);
                }
            }
            if (use==BattleArtefactUse.EnemyShip)
            {
                foreach (HexShip ship in Ships)
                {
                    if (ship.Ship.CurHex > IntBoya.Battle.Field.MaxHex) continue;
                    if (IntBoya.ControlledSide == ShipSide.Defense && ship.Ship.States.Side != ShipSide.Attack) continue;
                    if (IntBoya.ControlledSide == ShipSide.Attack && ship.Ship.States.Side != ShipSide.Defense) continue;
                    ArtefactLight enemyshiplight = new ArtefactLight(true, ship.Ship.States.BigSize, ship.Hex, IntBoya.ControlledSide==ShipSide.Attack?Brushes.Red:Brushes.Green);
                    lights.Add(enemyshiplight);
                    Inner.Children.Add(enemyshiplight);
                }
            }
            if (use==BattleArtefactUse.SelfShip)
            {
                foreach (HexShip ship in Ships)
                {
                    if (ship.Ship.CurHex > IntBoya.Battle.Field.MaxHex) continue;
                    if (ship.Ship.States.Controlled == false) continue;
                    if (IntBoya.ControlledSide == ShipSide.Defense && ship.Ship.States.Side != ShipSide.Defense) continue;
                    if (IntBoya.ControlledSide == ShipSide.Attack && ship.Ship.States.Side != ShipSide.Attack) continue;
                    ArtefactLight selfshiplight = new ArtefactLight(true, ship.Ship.States.BigSize, ship.Hex, IntBoya.ControlledSide == ShipSide.Attack ? Brushes.Red : Brushes.Green);
                    lights.Add(selfshiplight);
                    Inner.Children.Add(selfshiplight);
                }
            }
            ArtefactLights = lights;
        }*/
       /* class ArtefactLight:Canvas
        {
            public ArtefactLight(bool isShip, bool isBig, Hex hex, Brush brush)
            {
                Canvas.SetZIndex(this, 250);
                if (isShip==true)
                {
                    if (isBig == false)
                    {
                        Width = 400; Height = 400; Path path = new Path(); path.Stroke = brush; path.StrokeThickness = 3; path.Fill = brush;
                        path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M175,0 l25,70 l25,-70 M175,400 l25,-70 l25,70 M45,85 l40,50 l-60,-10" +
                  "M355,85 l-40,50 l60,-10 M45,315 l40,-50 l-60,10 M355,315 l-40,-50 l60,10")); Children.Add(path);
                        Canvas.SetLeft(this, hex.CenterX - 200); Canvas.SetTop(this, hex.CenterY - 200);
                    }
                    else
                        throw new Exception();
                }
                else
                {
                    Width = 300; Height = 260; Path path = new Path(); path.Stroke = brush; path.StrokeThickness = 3; path.Fill = brush;
                    path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M125,70 l25,-70 l25,70 M125,190 l25,70 l25,-70 M80,115 l-40,-50 l60,10" +
              "M220,115 l40,-50 l-60,10 M80,145 l-40,50 l60,-10 M220,145 l40,50 l-60,-10")); Children.Add(path);
                    Canvas.SetLeft(this, hex.CenterX - 150); Canvas.SetTop(this, hex.CenterY - 130);
                }
            }
        }*/
        public static void RemoveArtefact()
        {
            ActiveArtefact = null;
            //if (ArtefactLights!= null)
            //    foreach (ArtefactLight light in ArtefactLights)
            //    {
            //        Inner.Children.Remove(light);
            //    }
            ClearTarget("Отключение артефакта");
        }
        static void ActivateArtefact()
        {
            EventsButtonsCanvas.Children.Clear();
            new ShipEventButton(FirstArtefactPoint, SecondArtefactPoint, Mouse.GetPosition(Inner), ActiveArtefactPos);
            new ShipEventButton(0, 0, Mouse.GetPosition(Inner), 255);
            Inner.Children.Add(EventsButtonsCanvas);
            RemoveArtefact();
            IntBoya.RemoveArtSelect();
        }
        enum ArtefactUseResult { Used, BadTarget, BadSide}
        static ArtefactUseResult ArtefactUseOnShip(HexShip ship)
        {
            if (ship.Ship.CurHex > IntBoya.Battle.Field.MaxHex) return ArtefactUseResult.BadSide;
            if (FirstArtefactPointSelected==false)
            {
                switch (ActiveArtefact.Battle.FirstTarget)
                {
                    case BattleArtefactUse.AnyShip: FirstArtefactPoint = ship.Ship.CurHex; break;
                    case BattleArtefactUse.EnemyShip:
                        if ((IntBoya.ControlledSide == ShipSide.Attack && ship.Ship.States.Side == ShipSide.Defense) ||
                            (IntBoya.ControlledSide == ShipSide.Defense && ship.Ship.States.Side == ShipSide.Attack))
                            FirstArtefactPoint = ship.Ship.CurHex;
                        else
                            return ArtefactUseResult.BadSide;
                        break;
                    case BattleArtefactUse.SelfShip:
                        if ((ship.Ship.States.Controlled==true && IntBoya.ControlledSide == ShipSide.Attack && ship.Ship.States.Side == ShipSide.Attack) ||
                            (ship.Ship.States.Controlled==true && IntBoya.ControlledSide == ShipSide.Defense && ship.Ship.States.Side == ShipSide.Defense))
                            FirstArtefactPoint = ship.Ship.CurHex;
                        else
                            return ArtefactUseResult.BadSide;
                        break;
                    case BattleArtefactUse.SelfPortal:
                        if ((ship.Ship.States.IsPortal == true && IntBoya.ControlledSide == ShipSide.Attack && ship.Ship.States.Side == ShipSide.Attack) ||
                          (ship.Ship.States.IsPortal == true && IntBoya.ControlledSide == ShipSide.Defense && ship.Ship.States.Side == ShipSide.Defense))
                            FirstArtefactPoint = ship.Ship.CurHex;
                        else
                            return ArtefactUseResult.BadSide;
                        break;
                    default: return ArtefactUseResult.BadTarget;
                }
                FirstArtefactPointSelected = true;
                if (ActiveArtefact.Battle.SecondTarget == BattleArtefactUse.No)
                    ActivateArtefact();
               // else LightForArtefact(ActiveArtefact.Battle.SecondTarget);
                return ArtefactUseResult.Used;
            }
            else
            {
                switch (ActiveArtefact.Battle.SecondTarget)
                {
                    case BattleArtefactUse.AnyShip: SecondArtefactPoint = ship.Ship.CurHex; break;
                    case BattleArtefactUse.EnemyShip:
                        if ((IntBoya.ControlledSide == ShipSide.Attack && ship.Ship.States.Side == ShipSide.Defense) ||
                            (IntBoya.ControlledSide == ShipSide.Defense && ship.Ship.States.Side == ShipSide.Attack))
                            SecondArtefactPoint = ship.Ship.CurHex;
                        else
                            return ArtefactUseResult.BadSide;
                        break;
                    case BattleArtefactUse.SelfShip:
                        if ((ship.Ship.States.Controlled==true && IntBoya.ControlledSide == ShipSide.Attack && ship.Ship.States.Side == ShipSide.Attack) ||
                            (ship.Ship.States.Controlled==true && IntBoya.ControlledSide == ShipSide.Defense && ship.Ship.States.Side == ShipSide.Defense))
                            SecondArtefactPoint = ship.Ship.CurHex;
                        else
                            return ArtefactUseResult.BadSide;
                        break;
                    case BattleArtefactUse.SelfPortal:
                        if ((ship.Ship.States.IsPortal == true && IntBoya.ControlledSide == ShipSide.Attack && ship.Ship.States.Side == ShipSide.Attack) ||
                          (ship.Ship.States.IsPortal == true && IntBoya.ControlledSide == ShipSide.Defense && ship.Ship.States.Side == ShipSide.Defense))
                            SecondArtefactPoint = ship.Ship.CurHex;
                        else
                            return ArtefactUseResult.BadSide;
                        break;
                    default: return ArtefactUseResult.BadTarget;
                }
                ActivateArtefact();
                return ArtefactUseResult.Used;
            }
        }
        static ArtefactUseResult ArtefactUseOnHex(Hex hex)
        {
            if (FirstArtefactPointSelected == false)
            {
                switch (ActiveArtefact.Battle.FirstTarget)
                {
                    case BattleArtefactUse.AllHex: FirstArtefactPoint = hex.ID; break;
                    case BattleArtefactUse.FreeHex: if (IntBoya.Battle.BattleField.ContainsKey(hex.ID)) return ArtefactUseResult.BadSide;
                        else FirstArtefactPoint = hex.ID;
                        break;
                    default: return ArtefactUseResult.BadTarget;
                }
                FirstArtefactPointSelected = true;
                if (ActiveArtefact.Battle.SecondTarget == BattleArtefactUse.No)
                    ActivateArtefact();
                //else LightForArtefact(ActiveArtefact.Battle.SecondTarget);
                return ArtefactUseResult.Used;
            }
            else
            {
                switch (ActiveArtefact.Battle.SecondTarget)
                {
                    case BattleArtefactUse.AllHex: SecondArtefactPoint = hex.ID; break;
                    case BattleArtefactUse.FreeHex:
                        if (IntBoya.Battle.BattleField.ContainsKey(hex.ID)) return ArtefactUseResult.BadSide;
                        else SecondArtefactPoint = hex.ID;
                        break;
                    default: return ArtefactUseResult.BadTarget;
                }
                ActivateArtefact();
                return ArtefactUseResult.Used;
            }
        }
        static bool ClickOnAsteroid(HexShip ship)
        {
            if (ActiveArtefact != null)
            {
                if (FirstArtefactPointSelected == false)
                {
                    if (ActiveArtefact.Battle.FirstTarget != BattleArtefactUse.Asteroid)
                        return false;
                    FirstArtefactPoint = ship.Ship.CurHex;
                    FirstArtefactPointSelected = true;
                    if (ActiveArtefact.Battle.SecondTarget == BattleArtefactUse.No)
                        ActivateArtefact();
                    //else LightForArtefact(ActiveArtefact.Battle.SecondTarget);
                    return true;
                }
                else
                {
                    if (ActiveArtefact.Battle.SecondTarget != BattleArtefactUse.Asteroid)
                        return false;
                    SecondArtefactPoint = ship.Ship.CurHex;
                    ActivateArtefact();
                    return true;
                }
            }
            return false;
        }
        static bool ClickOnHex(Hex hex)
        {
            if (ActiveArtefact!= null)
            {
                ArtefactUseResult artefactuseresult = ArtefactUseOnHex(hex);
                if (artefactuseresult == ArtefactUseResult.Used) return true;
                else if (artefactuseresult == ArtefactUseResult.BadTarget) return false;
                else
                {
                    PutBadMoveEffect(hex);
                    return true;
                }
            }
            if (MovedShip != null)
            {
                //если корабль может ходить на этот хекс то ходит, если нет, то сброс цели
                if (MovedHexes.Contains(hex.ID))
                    //если он на поле - то ходит, если в гейте то прыгает    
                    if (MovedShip.CurHex <= IntBoya.Battle.Field.MaxHex)
                    {
                        MakeMove(hex);
                        return true;
                    }
                    else
                    {
                        MakeEnterToField(hex);
                        return true;
                    }
                //else
               // {
                //    ClearTarget("No Move");
                //    return;
               // }
            }
            return false;
        }
        static bool ClickOnShip(HexShip ship)
        {
            if (ship.Ship==MovedShip) return true;
            //Действия с артефактом
            if (ActiveArtefact!=null)
            {
                ArtefactUseResult artefactuseresult = ArtefactUseOnShip(ship);
                if (artefactuseresult == ArtefactUseResult.Used) return true;
                else if (artefactuseresult == ArtefactUseResult.BadTarget) return false;
                else
                {
                    PutBadMoveEffect(ship.Hex);
                    return true;
                }
            }
            if (MovedShip != null) //Если первый корабль выбран, подразумевается что этим кораблём можно управлять
            {
                if (MovedShip.CurHex <= IntBoya.Battle.Field.MaxHex) //если свой корабль на поле боя
                {
                    if (ship.Ship.States.Side == IntBoya.ControlledSide) //Если кликнутый корабль под вашим управлением
                    {
                        if (ship.Ship.States.IsPortal == true) //Если это портал
                        {
                            //выложить кнопки с возвратом
                            EventsButtonsCanvas.Children.Clear();
                            new ShipEventButton(MovedShip, ship.Ship, ShipEvents.Leave, ShipEventsPos.Center);
                            new ShipEventButton(MovedShip, ship.Ship, ShipEvents.No, ShipEventsPos.Left);
                            Inner.Children.Add(EventsButtonsCanvas);
                            return true;
                        }
                        else if (ship.Ship.CanCommand() == true) //Если кораблём можно управлять
                        {
                            //Переключаем таргет на корабль
                            SetTarget(ship.Ship);
                            return true;
                        }
                        else
                        {
                            //Иконка что нельзя управлять
                            PutBadMoveEffect(ship.Hex);
                            return true;
                        }
                    }
                    else //Если клик по чужому кораблю
                    {
                        if (ship.Ship.CurHex <= IntBoya.Battle.Field.MaxHex)//Если цель на поле боя
                        {
                            //Кнопки с выстрелами
                            List<ShipEvents> list = new List<ShipEvents>();
                            SortedList<ShipEvents, bool> Events = new SortedList<ShipEvents, bool>();
                            if (MovedShip.Weapons[0] != null) Events.Add(ShipEvents.GunLeft, CheckWeaponAvailable(MovedShip, 0) && CalculateAccuracy(MovedShip, ship.Ship, 0) > 0);
                            if (MovedShip.Weapons[1] != null) Events.Add(ShipEvents.GunMiddle, CheckWeaponAvailable(MovedShip, 1) && CalculateAccuracy(MovedShip, ship.Ship, 1) > 0);
                            if (MovedShip.Weapons[2] != null) Events.Add(ShipEvents.GunRight, CheckWeaponAvailable(MovedShip, 2) && CalculateAccuracy(MovedShip, ship.Ship, 2) > 0);
                            Events.Add(ShipEvents.No, true);
                            for(;;)
                            {
                                if (Events.Count == 0) break;
                                bool havetrue = false;
                                foreach (KeyValuePair<ShipEvents, bool> pair in Events)
                                 if (pair.Value==true) { list.Add(pair.Key); Events.Remove(pair.Key); havetrue = true; break; }
                                if (havetrue== false) { list.Add(Events.Keys[0]); Events.RemoveAt(0); } 
                            }
                            EventsButtonsCanvas.Children.Clear();
                            AddEventButtonCanvasRamka(MovedShip, ship.Ship);
                            for (int i = 0; i < list.Count; i++)
                                new ShipEventButton(MovedShip, ship.Ship, list[i], (ShipEventsPos)i);
                            Inner.Children.Add(EventsButtonsCanvas);
                            return true;
                        }
                        else
                        {
                            //Иконка что нельзя стрелять
                            PutBadMoveEffect(ship.Hex);
                            return true;
                        }
                    }
                }
                else //Если свой корабль не на поле
                {
                    if (ship.Ship.States.Side == IntBoya.ControlledSide) //Если кликнутый корабль под вашим управлением
                    {
                        if (ship.Ship.CanCommand() == true) //Если кораблём можно управлять
                        {
                            //Переключаем таргет на корабль
                            SetTarget(ship.Ship);
                            return true;
                        }
                        else
                        {
                            //Иконка что нельзя управлять
                            PutBadMoveEffect(ship.Hex);
                            return true;
                        }
                    }
                    else
                    {
                        //Иконка что нельзя управлять
                        PutBadMoveEffect(ship.Hex);
                        return true;
                    }
                }
            }
            else //Если корабль 1 не выбран
            {
                if (ship.Ship.States.Side==IntBoya.ControlledSide) //если клик по своему кораблю
                {
                    if (ship.Ship.CanCommand()==true) //Если кораблём можно управлять
                    {
                        SetTarget(ship.Ship);
                        return true;
                    }
                }
                PutBadMoveEffect(ship.Hex);
                return true;
            }
        }
        enum ShipEvents { GunLeft, GunMiddle, GunRight, Leave, Change, No}
        enum ShipEventsPos { Center, Left, Right, Top}
        class ShipEventButton:Viewbox
        {
            public static int Size = 200;
            public static double delta = 1;
            public static ShipB CurShip1, CurShip2;
            ShipB Ship1, Ship2;
            ShipEvents CurEvent;
            TextBlock ShieldInfo;
            TextBlock HealthInfo;
            Canvas canvas;
            public ScaleTransform Scale;
            bool Status = false;
            Artefact Artefact;
            byte El1, El2;
            byte ArtPos;
            public ShipEventButton(byte element1, byte element2, Point pt, byte artPos)
            {
                ArtPos = artPos;
                Artefact = null; if (ArtPos<3)
                    Artefact = IntBoya.Artefacts[artPos].Artefact; El1 = element1; El2 = element2;
                //element1 - ID первой точки приложения артефакта (Хекс, корабль или астероид - зависит от параметров артефакта)
                //element2 - ID второй точки прилоежния артефакта(Хекс, корабль или астероид - зависит от параметров артефакта)
                //art - если пусто - то иконка отмены
                Width = Size; Height = Size; canvas = new Canvas(); Child = canvas;
                canvas.Width = 70; canvas.Height = 70;
                //Ellipse back = new Ellipse(); back.Stroke = Brushes.White; back.Fill = Brushes.Black;
                //back.Width = 70; back.Height = 70; canvas.Children.Add(back);
                Rectangle back = Common.GetRectangle(70, BackBrush);
                canvas.Children.Add(back);
                if (Artefact == null)
                {
                    CreateNo();
                    Canvas.SetLeft(this, pt.X - Size / 2);
                    Canvas.SetTop(this, pt.Y - Size / 2-Size);
                }
                else
                {
                    CreateArtefact();
                    Canvas.SetLeft(this, pt.X - Size / 2);
                    Canvas.SetTop(this, pt.Y - Size / 2);
                }
                HexCanvas.EventsButtonsCanvas.Children.Add(this);
                RenderTransformOrigin = new Point(0.5, 0.5);
                Scale = new ScaleTransform(0, 0);
                RenderTransform = Scale;
                DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.1));
                Scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
                Scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
                PreviewMouseDown += ArtefactUse_PreviewMouseDown;
            }

            private void ArtefactUse_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            {
                e.Handled = true;
                HexCanvas.Inner.Children.Remove(HexCanvas.EventsButtonsCanvas);
                if (Artefact != null)
                {
                    UseArtefact(ArtPos, El1, El2);
                }
                IntBoya.RemoveArtSelect();
                HexCanvas.RemoveArtefact();
            }
            static Brush BackBrush = Gets.GetIntBoyaImage("EventHex");
            public ShipEventButton(ShipB ship1, ShipB ship2, ShipEvents curevent, ShipEventsPos pos)
            {
                CurShip1 = null; CurShip2 = null;
                Ship1 = ship1; Ship2 = ship2; CurEvent = curevent;
                Width = Size; Height = Size; canvas = new Canvas(); Child = canvas;
                canvas.Width = 70; canvas.Height = 70; canvas.Opacity = 1;
                Rectangle back = Common.GetRectangle(70, BackBrush);
                canvas.Children.Add(back);
                //Ellipse back = new Ellipse(); back.Stroke = Brushes.White; back.Fill = Brushes.Black;
                //back.Width = 70; back.Height = 70; canvas.Children.Add(back);
                switch (curevent)
                {
                    case ShipEvents.GunLeft: CreateGunInfo(0); break;
                    case ShipEvents.GunMiddle: CreateGunInfo(1); break;
                    case ShipEvents.GunRight: CreateGunInfo(2); break;
                    case ShipEvents.No: CreateNo(); break;
                    case ShipEvents.Leave: CreateLeave(); break;
                }
                switch (pos)
                {
                    case ShipEventsPos.Center: PutToCenter(); break;
                    case ShipEventsPos.Left: PutToLeft(); break;
                    case ShipEventsPos.Right: PutToRight(); break;
                    case ShipEventsPos.Top: PutToTop(); break;
                }
                HexCanvas.EventsButtonsCanvas.Children.Add(this);
                RenderTransformOrigin = new Point(0.5, 0.5);
                Scale = new ScaleTransform(0, 0);
                RenderTransform = Scale;
                DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.1));
                Scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
                Scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
                PreviewMouseDown += ShipEventButton_PreviewMouseDown;
            }

            private void ShipEventButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            {
                e.Handled = true;
                HexCanvas.Inner.Children.Remove(HexCanvas.EventsButtonsCanvas);
                if (Status== true)
                {
                    CurShip1 = Ship1; CurShip2 = Ship2;
                    switch (CurEvent)
                    {
                        case ShipEvents.GunLeft: MakeShoot(Ship2, 0, true); break;
                        case ShipEvents.GunMiddle: MakeShoot(Ship2, 1, true); break;
                        case ShipEvents.GunRight: MakeShoot(Ship2, 2, true); break;
                        case ShipEvents.Leave: ReturnShip(Ship1); break;
                    }
                }
            }
            Point[] CalcPoints(Point pt1, double D, double K, double B)
            {
                double A = 1 + K * K;
                double B3 = (-2 * pt1.X + 2 * K * (B - pt1.Y));
                double C = pt1.X * pt1.X + Math.Pow(B - pt1.Y, 2) - D * D;
                double Discr = B3 * B3 - 4 * A * C;
                double X1 = (-B3 + Math.Sqrt(Discr)) / (2 * A);
                double X2 = (-B3 - Math.Sqrt(Discr)) / (2 * A);
                double Y11 = K * X1 + B;
                double Y22 = K * X2 + B;
                return new Point[] { new Point(X1, Y11), new Point(X2, Y22) };
            }
            void PutToTop()
            {
                Point pt1 = Ship1.HexShip.Hex.CenterPoint;
                Point pt2 = Ship2.HexShip.Hex.CenterPoint;
                Point points;
                if (pt1.Y == pt2.Y) points = new Point(pt2.X + Size*delta * (pt2.X - pt1.X) / Math.Abs(pt2.X - pt1.X), pt2.Y);
                else if (pt1.X == pt2.X) points = new Point(pt2.X, pt2.Y + Size*delta * (pt2.Y - pt1.Y) / Math.Abs(pt2.Y - pt1.Y));
                else
                {
                    double K = (pt2.Y - pt1.Y) / (pt2.X - pt1.X);
                    double B = pt1.Y - K * pt1.X;
                    Point[] pointsd = CalcPoints(pt2, Size*delta, K, B);
                    double D1 = Math.Sqrt(Math.Pow(pointsd[0].X - pt1.X, 2) + Math.Pow(pointsd[0].Y - pt1.Y, 2));
                    double D2 = Math.Sqrt(Math.Pow(pointsd[1].X - pt1.X, 2) + Math.Pow(pointsd[1].Y - pt1.Y, 2));
                    if (D1 > D2) points = pointsd[0];
                    else points = pointsd[1];
                }
                Canvas.SetLeft(this, points.X - Size / 2); Canvas.SetTop(this, points.Y - Size / 2);
            }
            void PutToRight()
            {
                Point pt1 = Ship1.HexShip.Hex.CenterPoint;
                Point pt2 = Ship2.HexShip.Hex.CenterPoint;
                Point points;
                if (pt1.X == pt2.X)
                {
                    double z = pt2.Y > pt1.Y ? 1 : -1;
                    double y = pt2.Y + Size * 0.5 * z;
                    points = new Point(pt1.X - Size * 0.8, y);
                }
                else if (pt1.Y == pt2.Y)
                {
                    double z = pt2.X > pt1.X ? 1 : -1;
                    double x = pt2.X + Size * 0.5 * z;
                    points = new Point(x, pt1.Y - Size * 0.8);
                }
                else
                {
                    double K = (pt2.Y - pt1.Y) / (pt2.X - pt1.X);
                    double B = pt1.Y - K * pt1.X;
                    Point[] pointsd = CalcPoints(pt2, Size * 0.5, K, B);
                    double D1 = Math.Sqrt(Math.Pow(pointsd[0].X - pt1.X, 2) + Math.Pow(pointsd[0].Y - pt1.Y, 2));
                    double D2 = Math.Sqrt(Math.Pow(pointsd[1].X - pt1.X, 2) + Math.Pow(pointsd[1].Y - pt1.Y, 2));
                    if (D1 > D2) points = pointsd[0];
                    else points = pointsd[1];
                    double K2 = -1 / K;
                    double B2 = points.Y - K2 * points.X;
                    points = CalcPoints(points, Size * 0.8, K2, B2)[0];
                }
                Canvas.SetLeft(this, points.X - Size / 2); Canvas.SetTop(this, points.Y - Size / 2);
            }
            void PutToLeft()
            {
                Point pt1 = Ship1.HexShip.Hex.CenterPoint;
                Point pt2 = Ship2.HexShip.Hex.CenterPoint;
                Point points;
                if (pt1.X == pt2.X)
                {
                    double z = pt2.Y > pt1.Y ? 1 : -1;
                    double y = pt2.Y+Size * 0.5 * z;
                    points = new Point(pt1.X + Size * 0.8, y);
                }
                else if (pt1.Y==pt2.Y)
                {
                    double z = pt2.X > pt1.X ? 1 : -1;
                    double x = pt2.X + Size * 0.5 * z;
                    points = new Point(x, pt1.Y + Size * 0.8);
                }
                else
                {
                    double K = (pt2.Y - pt1.Y) / (pt2.X - pt1.X);
                    double B = pt1.Y - K * pt1.X;
                    Point[] pointsd = CalcPoints(pt2, Size * 0.5, K, B);
                    double D1 = Math.Sqrt(Math.Pow(pointsd[0].X - pt1.X, 2) + Math.Pow(pointsd[0].Y - pt1.Y, 2));
                    double D2 = Math.Sqrt(Math.Pow(pointsd[1].X - pt1.X, 2) + Math.Pow(pointsd[1].Y - pt1.Y, 2));
                    if (D1 > D2) points = pointsd[0];
                    else points = pointsd[1];
                    double K2 = -1 / K;
                    double B2 = points.Y - K2 * points.X;
                    points = CalcPoints(points, Size * 0.8, K2, B2)[1];
                }
                    
                Canvas.SetLeft(this, points.X - Size / 2); Canvas.SetTop(this, points.Y - Size / 2);
            }
            void PutToCenter()
            {
                Canvas.SetLeft(this, Ship2.HexShip.Hex.CenterX - Size/2);
                Canvas.SetTop(this, Ship2.HexShip.Hex.CenterY - Size/2);
            }
            void CreateGunInfo(int pos)
            {
                PutImageInfo(Ship1.Weapons[pos].Type);
                switch (IntBoya.Battle.Restriction)
                {
                    case Restriction.NoEnergy: if (Ship1.Weapons[pos].Group == WeaponGroup.Energy) { PutRestrictedInfo(); return;} break;
                    case Restriction.NoPhysic: if (Ship1.Weapons[pos].Group == WeaponGroup.Physic) { PutRestrictedInfo(); return; } break;
                    case Restriction.NoIrregular: if (Ship1.Weapons[pos].Group == WeaponGroup.Irregular) { PutRestrictedInfo(); return; } break;
                    case Restriction.NoCyber: if (Ship1.Weapons[pos].Group == WeaponGroup.Cyber) { PutRestrictedInfo(); return; } break;
                }
                if (Ship1.Weapons[pos]._IsArmed== false) { PutDisArmInfo(); return; }
                if (Ship1.Params.Energy.GetCurrent < Ship1.Weapons[pos].Consume) { PutNoEnergyInfo(); return; }
                int acc = HexCanvas.CalculateAccuracy(Ship1, Ship2, pos);
                if (acc==Int32.MinValue) { PutBlockedInfo(); return; }
                if (acc <= 0) PutAccInfo("0%",0);
                else PutAccInfo(acc.ToString() + "%", acc);
                int angle = IntBoya.Battle.Field.DistAngleRot[Ship1.CurHex, Ship2.CurHex, pos, 1];
                DamageResult result = Ship2.CalcBasicDamage(Ship1.Weapons[pos], angle);
                string shielddamage = "---"; if (result.ToShield > 0) shielddamage = result.ToShield.ToString();
                string healthdamage = "---";if (result.ToHealth > 0) healthdamage = result.ToHealth.ToString();
                PutDamageInfo(shielddamage, healthdamage);
                /*if (result.ToShield > 0)
                    PutShieldInfo(result.ToShield.ToString());
                else PutShieldInfo("---");
                if (result.ToHealth > 0)
                    PutHealthInfo(result.ToHealth.ToString());
                else PutHealthInfo("---");*/
                if (acc > 0) Status = true;
            }
            void CreateArtefact()
            {
                Rectangle rect = Common.GetRectangle(30, Artefact.GetImage());
                canvas.Children.Add(rect); Canvas.SetLeft(rect, 20); Canvas.SetTop(rect, 8);
                PutTextInfo(Artefact.Battle.Text, 8);
                //TextBlock block = Common.GetBlock(8, Artefact.Battle.Text, Brushes.White, 50);
                //canvas.Children.Add(block);
                //Canvas.SetLeft(block, 10); Canvas.SetTop(block, 37);
            }
            void CreateLeave()
            {
                Path warp = new Path(); canvas.Children.Add(warp); warp.Stroke = Brushes.White;
                warp.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                    "M35,8 a8,8 0 0,0 0,8 a16,16 0 0,1 16,8 a8,8 0 0,0 -8,0 a16,16 0 0,1 -8,16 a8,8 0 0,0 0,-8 a16,16 0 0,1 -16,-8 a8,8 0 0,0 8,0 a16,16 0 0,1 8,-16"));
                warp.StrokeThickness = 0.5;
                //TextBlock leaveext = Common.GetBlock(8, "Покинуть бой", Brushes.White, 50);
                //canvas.Children.Add(leaveext); Canvas.SetLeft(leaveext, 10); Canvas.SetTop(leaveext, 50);
                RadialGradientBrush warpbrush = new RadialGradientBrush();
                warpbrush.GradientStops.Add(new GradientStop(Colors.Violet, 1));
                warpbrush.GradientStops.Add(new GradientStop(Colors.Purple, 0.5));
                warpbrush.GradientStops.Add(new GradientStop(Colors.Black, 0.3));
                warp.Fill = warpbrush;
                Status = true;
                PutTextInfo("Покинуть бой", 10);
            }
            static Brush NoBrush = Gets.GetIntBoyaImage("NoHex");
            void CreateNo()
            {
                canvas.Children.Clear();
                Rectangle back = Common.GetRectangle(50, NoBrush);
                canvas.Children.Add(back);
                Canvas.SetLeft(back, 10); Canvas.SetTop(back, 10);
                /*
                Path crest = new Path(); canvas.Children.Add(crest); crest.Stroke = Brushes.White;
                crest.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M23,12 l12,12  l12,-12 l5,5 l-12,12 l12,12 l-5,5 l-12,-12 l-12,12 l-5,-5 l12,-12 l-12,-12z"));
                crest.StrokeThickness = 0.5;
                TextBlock stopmove = Common.GetBlock(8, "Ничего не делать", Brushes.White, 50);
                canvas.Children.Add(stopmove); Canvas.SetLeft(stopmove, 10); Canvas.SetTop(stopmove, 50);
                RadialGradientBrush nohbrush = new RadialGradientBrush(); nohbrush.GradientOrigin = new Point(0.7, 0.3);
                nohbrush.GradientStops.Add(new GradientStop(Colors.Red, 1));
                nohbrush.GradientStops.Add(new GradientStop(Colors.White, 0));
                crest.Fill = nohbrush;*/
            }
            /// <summary> картинка с иконктой пушки</summary>
            void PutImageInfo(EWeaponType type)
            {
                Ellipse el = new Ellipse();
                el.Fill = Links.Brushes.WeaponsBrushes[type];
                //el.Width = 26; el.Height = 26;
                el.Width = 32; el.Height = 32;
                canvas.Children.Add(el);
                //Canvas.SetLeft(el, 22);
                Canvas.SetLeft(el, 19);
                //Canvas.SetTop(el, 2);
                Canvas.SetTop(el, 5);
            }
            void PutRestrictedInfo()
            {
                //PutShieldInfo("---");
                //PutHealthInfo("---");
                //PutAccInfo("Недоступно");
                PutTextInfo("Недоступно",0);
            }
            void PutDisArmInfo()
            {
                //PutShieldInfo("---");
                //PutHealthInfo("---");
                //PutAccInfo("Разряжено");
                PutTextInfo("Разряжено",0);
            }
            void PutNoEnergyInfo()
            {
                //PutShieldInfo("---");
                //PutHealthInfo("---");
                //PutAccInfo("Нет энергии");
                PutTextInfo("Нет энергии",0);
            }
            void PutBlockedInfo()
            {
                //PutShieldInfo("---");
                //PutHealthInfo("---");
                //PutAccInfo("Цель блокирована");
                PutTextInfo("Цель блокирована",0);
            }
            void PutTextInfo(string value, int topdelta)
            {
                TextBlock block = Common.GetBlock(10, value, Links.Brushes.SkyBlue, 50);
                block.FontWeight = FontWeights.Normal;
                canvas.Children.Add(block);
                Canvas.SetLeft(block, 10); Canvas.SetTop(block, 30+topdelta);
            }
            static Brush GreenBrush = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            void PutAccInfo(string value, int param)
            {
                TextBlock block = Common.GetBlock(10, value, Links.Brushes.SkyBlue, 50);
                block.TextAlignment = TextAlignment.Right; block.FontWeight = FontWeights.Normal;
                canvas.Children.Add(block);
                Canvas.SetLeft(block, -2); Canvas.SetTop(block, -4);
                if (param < 20) block.Foreground = Brushes.Red;
                else if (param < 50) block.Foreground = Brushes.Yellow;
                else block.Foreground = GreenBrush;
                /*Ellipse shoot = new Ellipse(); shoot.Stroke = Brushes.White; shoot.StrokeThickness = 0.5;
                shoot.Width = 16; shoot.Height = 16; canvas.Children.Add(shoot);
                Canvas.SetLeft(shoot, 27); Canvas.SetTop(shoot, 35);
                Path arrows = new Path(); canvas.Children.Add(arrows); arrows.Fill = Brushes.White; arrows.StrokeThickness = 0.5;
                arrows.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M34,33 v5 l1,2 l1,-2 v-5z M34,53 v-5 l1,-2 l1,2 v5z M25,42 h5 l2,1 l-2,1 h-5z M45,42 h-5 l-2,1 l2,1 h5z"));
                int textsize = value.Length < 5 ? 8 : 7;
                TextBlock AccInfo = Common.GetBlock(textsize, value, Brushes.White, 50);
                canvas.Children.Add(AccInfo); Canvas.SetLeft(AccInfo, 10); Canvas.SetTop(AccInfo, 53);*/
            }
            static Brush ShieldHealthDamageBrush = Gets.GetIntBoyaImage("ShieldHealth");
            void PutDamageInfo(string shield, string health)
            {
                Rectangle Image = new Rectangle();
                Image.Width = 10; Image.Height = 20; canvas.Children.Add(Image);
                Canvas.SetLeft(Image, 25); Canvas.SetTop(Image, 40);
                Image.Fill = ShieldHealthDamageBrush;

                TextBlock sb = Common.GetBlock(8, shield, Links.Brushes.SkyBlue, 50); sb.TextAlignment = TextAlignment.Left;
                canvas.Children.Add(sb); Canvas.SetLeft(sb, 37); Canvas.SetTop(sb, 40); sb.FontWeight = FontWeights.Normal;
                TextBlock hb = Common.GetBlock(8, health, Links.Brushes.SkyBlue, 50); hb.TextAlignment = TextAlignment.Left;
                canvas.Children.Add(hb); Canvas.SetLeft(hb, 37); Canvas.SetTop(hb, 52);hb.FontWeight = FontWeights.Normal;
            }
            void PutHealthInfo(string value)
            {
                Path health = new Path(); health.Stroke = Brushes.White; health.StrokeThickness = 0.5;
                canvas.Children.Add(health); Canvas.SetLeft(health, 47); Canvas.SetTop(health, 20);
                health.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,5 a6.5,10 0 0,1 10,0 a6.5,10 0 0,1 10,0 a20,20 0 0,1 -10,20 a20,20 0 0,1 -10,-20"));
                RadialGradientBrush healthbrush = new RadialGradientBrush(); healthbrush.GradientOrigin = new Point(0.7, 0.3);
                healthbrush.GradientStops.Add(new GradientStop(Colors.Red, 0.8));
                healthbrush.GradientStops.Add(new GradientStop(Colors.White, 0));
                health.Fill = healthbrush;
                HealthInfo = Common.GetBlock(8, value, Brushes.White, 20);
                canvas.Children.Add(HealthInfo); Canvas.SetLeft(HealthInfo, 47); Canvas.SetTop(HealthInfo, 45);
            }
            void PutShieldInfo(string value)
            {
                Path shield = new Path(); shield.Stroke = Brushes.White; shield.StrokeThickness = 0.5;
                canvas.Children.Add(shield); Canvas.SetLeft(shield, 3); Canvas.SetTop(shield, 20);
                shield.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,5 a13,10 0 0,1 20,0 a20,20 0 0,1 -10,20 a20,20 0 0,1 -10,-20"));
                RadialGradientBrush shieldbrush = new RadialGradientBrush(); shieldbrush.GradientOrigin = new Point(0.7, 0.3);
                shieldbrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 0, 128, 255), 0.8));
                shieldbrush.GradientStops.Add(new GradientStop(Colors.White, 0));
                shield.Fill = shieldbrush;
                ShieldInfo = Common.GetBlock(8, value, Brushes.White, 20);
                canvas.Children.Add(ShieldInfo); Canvas.SetLeft(ShieldInfo, 3); Canvas.SetTop(ShieldInfo, 45);
            }
        }
        /// <summary> событие, при клике по полю боя. Определяет по какому объекту сделан клик и вызывает соответствующий метод</summary>
        static void Inner_Traditional(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            if (!BattleController.CanCommand)
                return;
            if (IntBoya.Battle.IsFinished == true) return;
            if (Inner.Children.Contains(EventsButtonsCanvas)) return;
            ///Проверка, выполнен ли клик по кораблю, хексу или артефакту
            foreach (HexShip ship in Ships)
            {
                HitTestResult result = VisualTreeHelper.HitTest(ship.SelectPath, Mouse.GetPosition(ship.SelectPath));
                if (result!= null)
                {
                    //выполнен клик по кораблю
                    bool clickresult = ClickOnShip(ship);
                    if (clickresult == true) return;
                    //Необходимо определить, что это за корабль

                   
                }
            }
            foreach (Hex hex in Hexes.Values)
            {
                HitTestResult result = VisualTreeHelper.HitTest(hex.path, Mouse.GetPosition(hex.path));
                if (result!= null)
                {
                    bool clickresult = ClickOnHex(hex);
                    if (clickresult == true) return;
                   
                }
            }
            foreach (HexShip asteroid in Asteroids)
            {
                HitTestResult result = VisualTreeHelper.HitTest(asteroid.SelectPath, Mouse.GetPosition(asteroid.SelectPath));
                if (result!= null)
                {
                    bool clickresult = ClickOnAsteroid(asteroid);
                    if (clickresult == true) return;
                }
            }
            ClearTarget("No one target");
            //сброс цели
        }
        static void MakeShoot(ShipB target, int gun, bool addinfo)
        {
            if (IntBoya.CurMode == BattleMode.Mode1)
            {
                MakeShoot(MovedShip, target, gun);
                SetTarget(MovedShip);
                if (addinfo)
                    PutMouseMoveInfo(MovedShip, target);
            }
            else if (IntBoya.CurMode == BattleMode.Mode2)
            {
                if (CheckWeaponAvailable(MovedShip, gun) == false) return;
                RemoveInfo();
                if (TempAngle != Int32.MaxValue)
                {
                    MovedShip.SetAngle(TempAngle, true); MovedShip.Angle = TempAngle; TempAngle = Int32.MaxValue;
                }
                IntBoya.Battle.CurrentTurn++;
                DebugWindow.AddTB1("Ход " + IntBoya.Battle.CurrentTurn.ToString() + " ", true);
                DebugWindow.AddTB1(String.Format("Выстрел корабля {0} {1} пушка {2}", MovedShip.States.Side == ShipSide.Attack ? "атаки" : "защиты", MovedShip.BattleID, gun), false);
                ClientShootResult shootresult = new ClientShootResult(MovedShip, gun, MovedShip.Side, target);
                if (shootresult.Error) throw new Exception("Ощибка при инициации стрельбы");
                if (shootresult.PsiCrit)
                    new PsiCritFlash(MovedShip.HexShip);
                MovedShip.Side.CurrentCommand.Add(new BattleCommand((byte)(7 + gun), MovedShip.BattleID, target.BattleID));
                double RealAngle = IntBoya.Battle.Field.DistAngleRot[MovedShip.CurHex, target.CurHex, gun + 0, 1];
                //TestBattle.AddWeaponDamage(IntBoya.Battle.ID, IntBoya.Battle.CurrentTurn, MovedShip.BattleID, MovedShip.States.Side, target.BattleID,
                //    (byte)gun, false, (short)RealAngle, false);
                BW.SetWorking("Выстрел режим 2", true);
                BattleController.MakeFire(MovedShip, shootresult.Target, (byte)gun, !shootresult.ShootResult, shootresult.AbilityResult, shootresult.AntiCrit, shootresult.IsAsteroidAttack, false);

                ClearTarget("Выстрел корабля по режиму два");
                //BattleController.MakeFire(MovedShip, target, (byte)gun, false, false, false, false);
            }
        }
        static void UseArtefact(byte pos, byte hex1, byte hex2)
        {
            RemoveInfo();
            if (IntBoya.ControlledSide == ShipSide.Attack)
            {
                string canartuse = IntBoya.Battle.Side1.CheckArtefactAvailable(pos);
                if (canartuse != "")
                {
                    WriteBigText(canartuse, Colors.Red, 3);
                    return;
                }
                IntBoya.Battle.Side1.CurrentCommand.Add(new BattleCommand((byte)(32 + pos), hex1, hex2));
                IntBoya.Battle.Side1.UseArtefact(pos, hex1, hex2, false);
            }
            else
            {
                string canartuse = IntBoya.Battle.Side2.CheckArtefactAvailable(pos);
                if (canartuse != "")
                {
                    WriteBigText(canartuse, Colors.Green, 3);
                    return;
                }
                IntBoya.Battle.Side2.CurrentCommand.Add(new BattleCommand((byte)(32 + pos), hex1, hex2));
                IntBoya.Battle.Side2.UseArtefact(pos, hex1, hex2, false);
            }
        }
        static void MakeMove(Hex hex)
        {
            if (IntBoya.CurMode==BattleMode.Mode1)
            {
                MovedShip.Side.CurrentCommand.Add(new BattleCommand(5, MovedShip.BattleID, hex.ID));
                int distance = IntBoya.Battle.Field.DistAngleRot[MovedShip.CurHex, hex.ID, 6, 0];
                PlaceShipCopyAndArrows(hex, IntBoya.Battle.Field.DistAngleRot[MovedShip.CurHex, hex.ID, 0, 2]);
                int energycost = (int)(distance / 250.0 * MovedShip.Params.HexMoveCost.GetCurrent);
                MovedShip.RemoveEnergy(energycost, true);
                SetTarget(MovedShip);
                AddInfo();
            }
            else if (IntBoya.CurMode==BattleMode.Mode2)
            {
                RemoveInfo();
                IntBoya.Battle.CurrentTurn++;
                DebugWindow.AddTB1("Ход " + IntBoya.Battle.CurrentTurn.ToString() + " ", true);
                DebugWindow.AddTB1(String.Format("Корабль {0} передвинулся с хекса {1} на хекс {2}\n", MovedShip.States.Side == ShipSide.Attack ? "атаки" : "защиты",
                    MovedShip.CurHex, hex.ID), false);
                MovedShip.Side.CurrentCommand.Add(new BattleCommand(5, MovedShip.BattleID, hex.ID));
                BW.SetWorking("Передвижение режим 2", true);
                BattleController.MoveShip(MovedShip, hex, false);
                ClearTarget("Перемещение корабля");
            }
        }
        static void MakeEnterToField(Hex hex)
        {
            if (IntBoya.CurMode==BattleMode.Mode1)
            {
                MovedShip.Side.CurrentCommand.Add(new BattleCommand(4, MovedShip.BattleID, hex.ID));
                PlaceShipCopyAndArrows(hex, MovedShip.States.Side == ShipSide.Attack ? 120 : 300);
                SetTarget(MovedShip);
                AddInfo();
            }
            else if (IntBoya.CurMode==BattleMode.Mode2)
            {
                RemoveInfo();
                IntBoya.Battle.CurrentTurn++;
                DebugWindow.AddTB1("Ход " + IntBoya.Battle.CurrentTurn.ToString() + " ", true);
                //TestBattle.AddMoveShipToField(IntBoya.Battle.ID, IntBoya.Battle.CurrentTurn, MovedShip.BattleID, MovedShip.States.Side, hex.ID);
                MovedShip.Side.CurrentCommand.Add(new BattleCommand(4, MovedShip.BattleID, hex.ID));
                DebugWindow.AddTB1(String.Format("Корабль {0} {1} вышел на поле на хекс {2}\n", MovedShip.States.Side == ShipSide.Attack ? "атаки" : "защиты", 
                    MovedShip.BattleID, hex.ID), false);
                BW.SetWorking("Выход на поле боя режим 2", true);
                BattleController.EnterShipToField(MovedShip, hex);
                IntBoya.Battle.BattleField.Add(hex.ID, MovedShip);
                //IntBoya.Battle.BattleFieldInfo.Add(string.Format("Корабль вошёл на поле боя Ataka={0} ID={1} Hex={2}", MovedShip.States.Side, MovedShip.BattleID, hex.ID));
                ClearTarget("Начало входа корабля в бой по режиму два");
            }
        }
        static void MakeShoot(ShipB ship, ShipB target, int weapon)
        {
            ship.Side.CurrentCommand.Add(new BattleCommand((byte)(7 + weapon), ship.BattleID, target.BattleID));
            PlaceArrow(ship, target, weapon);
            ship.RemoveEnergy(ship.Weapons[weapon].Consume, true);
            ship.Weapons[weapon].IsArmed(false, "Разрядка после инициализации выстрела в режиме боя 1");
            AddInfo();
            if (MouseMoveInfo != null)
            {
                Inner.Children.Remove(MouseMoveInfo);
                MouseMoveInfo = null;
            }
        }
        /*static void MakeShootAll(ShipB target)
        {
            foreach (HexShip ship in Ships)
            {
                if (ship.Ship.CurHex > IntBoya.Battle.Field.MaxHex) continue;
                if (ship.Ship.States.Side != IntBoya.ControlledSide) continue;
                if (ship.Ship.Params.Status.IsConfused > 0) continue;
                for (int i = 0; i < 3; i++)
                {
                    if (CheckWeaponAvailable(ship.Ship, i))
                    {
                        int acc = CalculateAccuracy(ship.Ship, target, i);
                        if (acc == -1) continue;
                        MakeShoot(ship.Ship, target, i);
                    }
                }
            }
            AddInfo();
        }*/
        /// <summary> Метод выделяет корабль и все соответствующие хексы (для движения или телепортации)</summary>
        public static void SetTarget(ShipB ship)
        {
            ClearTarget("Set target");
            if (ship.Params.Status.IsConfused > 0) return;
            if (ship.CurHex > IntBoya.Battle.Field.MaxHex && ship.CurHex != 211 && ship.CurHex != 212 && ship.CurHex != 213)
                return;
            MovedShip = ship;
            //Выделяет хексы для движения корабля
            if (ship.CurHex <= IntBoya.Battle.Field.MaxHex)
            {
                int maxdistance = (int)((double)ship.Params.Energy.GetCurrent / ship.Params.HexMoveCost.GetCurrent * 250);
                foreach (Hex hex in Hexes.Values)
                {
                    if (hex == ship.HexShip.Hex) continue;
                    if (IntBoya.Battle.BattleField.ContainsKey(hex.ID)) continue;
                    bool isbigshipnearhex = false;
                    foreach (ShipB bigship in IntBoya.Battle.BigShips)
                    {
                        if (bigship.CurHex > IntBoya.Battle.Field.MaxHex) continue;
                        foreach (Hex bigshipnearhex in bigship.HexShip.Hex.NearHexes)
                            if (hex.ID==bigshipnearhex.ID)
                            {
                                isbigshipnearhex = true;
                                break;
                            }
                        if (isbigshipnearhex) break;
                    }
                    if (isbigshipnearhex) continue;
                    int curdistance = IntBoya.Battle.Field.DistAngleRot[ship.CurHex, hex.ID, 6, 0];
                    if (curdistance <= maxdistance)
                    {
                        MovedHexes.Add(hex.ID);
                        hex.SetBright(BrightSelect.Move);
                    }
                }
            }
            //выделяет хексы для телепортации корабля
            else
            {
                if (ship.Side.Portals.Count>0)
                {
                    List<byte> HexesArray = ship.Side.GetJumpHexes((byte)(ship.CurHex - 211));
                    
                    foreach (byte b in HexesArray)
                    {
                        if (IntBoya.Battle.BattleField.ContainsKey(b)) continue;
                        MovedHexes.Add(b);
                        Hexes[b].SetBright(BrightSelect.Jump);
                    }
                }
            }
            MovedShip.HexShip.Select(true);
            //new TargetAim(ship);
        }
        
        public static void ClearTarget(string reason)
        {
            foreach (byte b in Hexes.Keys)
                Hexes[b].SetBright(BrightSelect.None);
            if (MovedShip == null) return;
            MovedShip.HexShip.Select(false);
            foreach (Canvas canvas in Aim)
                Inner.Children.Remove(canvas);
            Aim.Clear();
            //foreach (byte b in MovedHexes)
            //    Hexes[b].SetBright(BrightSelect.None);
            MovedHexes.Clear();
            MovedShip = null;
        }
        static List<TextShipInfo> InfoList = new List<TextShipInfo>();
        public static void AddInfo()
        {
            RemoveInfo();
            foreach (HexShip ship in Ships)
            {
                if (ship.Ship.States.Side != IntBoya.ControlledSide) continue;
                if (ship.Hex == null) continue;
                if (ship.Ship.States.Controlled == false) continue;
                switch (ship.Ship.CurHex)
                {
                    case 201: InfoList.Add(new TextShipInfo("1", ship)); break;
                    case 202: InfoList.Add(new TextShipInfo("2", ship)); break;
                    case 203: InfoList.Add(new TextShipInfo("3", ship)); break;
                    case 211:
                    case 212:
                    case 213: InfoList.Add(new TextShipInfo("GO", ship)); break;
                    default:
                        if (ship.Ship.Params.Status.IsConfused > 0)
                        {
                            InfoList.Add(new TextShipInfo("Confused", ship));
                            continue;
                        }
                        int canmove = ship.Ship.CheckMove();
                        if (canmove == 2)
                            InfoList.Add(new TextShipInfo("Ready", ship));
                        else if (canmove == 1)
                            InfoList.Add(new TextShipInfo("Move", ship));
                        else
                            InfoList.Add(new TextShipInfo("No Energy", ship));
                        continue;
                }
                
            }
        }
        public static void RemoveInfo()
        {
            foreach (TextShipInfo info in InfoList)
            {
                Inner.Children.Remove(info);
            }
            InfoList.Clear();
        }
        
        public static void StartBattle(Battle battle)
        {
            if (Ships != null)
            {
                foreach (HexShip ship in Ships)
                    Inner.Children.Remove(ship);
            }
            //Ships = new List<HexShip>();
            foreach (ShipB ship in battle.Side1.Ships.Values)
            { 
                Ships.Add(ship.HexShip);
                Inner.Children.Add(ship.HexShip);
                if (ship.CurHex > IntBoya.Battle.Field.MaxHex)
                {
                    ship.HexShip.Opacity = 0;
                    Canvas.SetLeft(ship.HexShip, -1000);
                }
                else
                {
                    Hexes[ship.CurHex].PlaceShip(ship.HexShip);
                    ship.HexShip.Hex = Hexes[ship.CurHex];
                    ship.HexShip.SetAngle(ship.Angle);
                }
                ship.HexShip.MouseEnter += PopUpOpen;
                ship.HexShip.MouseLeave += PopUpClose;
                if (ship.Panel2 != null)
                { ship.Panel2.SetFleet(battle.Emblem1); ship.Panel2.UpdateLevel(); }
            }
            foreach (ShipB ship in battle.Side2.Ships.Values)
            {
                Ships.Add(ship.HexShip);
                Inner.Children.Add(ship.HexShip);
                if (ship.CurHex > IntBoya.Battle.Field.MaxHex)
                {
                    ship.HexShip.Opacity = 0;
                    Canvas.SetLeft(ship.HexShip, -1000);   
                }
                else
                {
                    Hexes[ship.CurHex].PlaceShip(ship.HexShip);
                    ship.HexShip.Hex = Hexes[ship.CurHex];
                }
                ship.HexShip.MouseEnter += PopUpOpen;
                ship.HexShip.MouseLeave += PopUpClose;
                if (ship.Panel2 != null)
                { ship.Panel2.SetFleet(battle.Emblem2); ship.Panel2.UpdateLevel(); }
        }
           
            foreach (ShipB asteroid in battle.Asteroids.Values)
            {
                Inner.Children.Add(asteroid.HexShip);
                Hexes[asteroid.CurHex].PlaceShip(asteroid.HexShip);
                if (asteroid.Model[0] != 254) Asteroids.Add(asteroid.HexShip);
                asteroid.HexShip.Hex = Hexes[asteroid.CurHex];
            }
            //SetBaseParams();
        }
        public static bool CheckWeaponAvailable(ShipB ship, int weapon)
        {

            if (ship.Weapons[weapon] == null || !ship.Weapons[weapon]._IsArmed || ship.Params.Energy.GetCurrent < ship.Weapons[weapon].Consume) return false;
            switch (ship.Battle.Restriction)
            {
                case Restriction.NoEnergy: if (ship.Weapons[weapon].Group == WeaponGroup.Energy) return false; break;
                case Restriction.NoPhysic: if (ship.Weapons[weapon].Group == WeaponGroup.Physic) return false; break;
                case Restriction.NoIrregular: if (ship.Weapons[weapon].Group == WeaponGroup.Irregular) return false; break;
                case Restriction.NoCyber: if (ship.Weapons[weapon].Group == WeaponGroup.Cyber) return false; break;
            }
            return true;
        }
       
        static void PutBadMoveEffect(Hex hex)
        {
            PutOneDamageLabel(hex, DamageLabelTarget.Health, "NO");
        }
      
        public static void ReturnShip(ShipB ship)
        {
            if (IntBoya.Battle.CurMode == BattleMode.Mode1)
            {
                if (ship.States.Side == ShipSide.Attack)
                {
                    HexArrow arrow = new HexArrow(ship.HexShip.GetMiddleGunPoint(), HexCanvas.Hexes[0].CenterPoint, WeaponGroup.Any);
                    Inner.Children.Add(arrow);
                    HexArrows.Add(arrow);
                }
                else
                {
                    HexArrow arrow = new HexArrow(ship.HexShip.GetMiddleGunPoint(), HexCanvas.Hexes[(byte)IntBoya.Battle.Field.MaxHex].CenterPoint, WeaponGroup.Any);
                    Inner.Children.Add(arrow);
                    HexArrows.Add(arrow);
                }
                ship.Side.CurrentCommand.Add(new BattleCommand(10, ship.BattleID, 255));
                AddInfo();
            }
            else if (IntBoya.Battle.CurMode == BattleMode.Mode2)
            {
                RemoveInfo();
                IntBoya.Battle.CurrentTurn++;
                DebugWindow.AddTB1("Ход " + IntBoya.Battle.CurrentTurn.ToString() + " ", true);
                DebugWindow.AddTB1(String.Format("Корабль {0} {1} покинул поле боя", MovedShip.States.Side == ShipSide.Attack ? "атаки" : "защиты",
                    MovedShip.CurHex), false);
                MovedShip.Side.CurrentCommand.Add(new BattleCommand(10, MovedShip.BattleID, 255));
                BW.SetWorking("Покидание поля боя режим 2", true);
                BattleController.ShipReturned(MovedShip, false);
                //BattleController.MoveShip(MovedShip, hex, false);
                ClearTarget("Покидание поля боя");
            }
        }
        
        /// <summary> метод используется для создании границы вокруг кнопки у кругового элемента </summary>
        static Border PutLabelToBorder(Label lbl)
        {
            Border border = new Border();
            border.Width = lbl.Width + 4;
            border.Height = lbl.Height + 4;
            border.BorderBrush = Brushes.DarkGray;
            border.BorderThickness = new Thickness(2);
            border.CornerRadius = new CornerRadius(4);
            //border.Background = Brushes.DarkGray;
            border.Child = lbl;
            return border;
        }
         
        public static HexArrow PlaceArrow(ShipB Gunner, ShipB Target, int weaponpos)
        {
            if (Target.HexShip.Hex == null) return null;
            if (Gunner.CurHex > IntBoya.Battle.Field.MaxHex) return null;
            BattleWeapon weapon = Gunner.Weapons[weaponpos];
            Point GunPoint;
            int angle = IntBoya.Battle.Field.DistAngleRot[Gunner.CurHex, Target.CurHex, weaponpos, 2];
            switch (weaponpos)
            {
                case 0: GunPoint = Gunner.HexShip.GetLeftGunPoint(angle); break;
                case 1: GunPoint = Gunner.HexShip.GetMiddleGunPoint(angle); break;
                default: GunPoint = Gunner.HexShip.GetRightGunPoint(angle); break;
            }
            HexArrow arrow = new HexArrow(GunPoint, new Point(Target.HexShip.Hex.CenterX, Target.HexShip.Hex.CenterY), weapon.Group);
            Inner.Children.Add(arrow);
            HexArrows.Add(arrow);
            if (!MoveShipsList.ContainsKey(Gunner))
                MoveShipsList.Add(Gunner, new ShipRealParams(Gunner));
            TempAngle = Gunner.Angle;
            Gunner.PutShipToHexNotReal(Gunner.HexShip.Hex, angle, Gunner.CurHex);
            return arrow;
        }
        public static void PlaceShipCopyAndArrows(Hex TargetHex, int angle)
        {
            ShipB ship = MovedShip;
            HexArrow arrow = new HexArrow(new Point(ship.HexShip.Hex.CenterX, ship.HexShip.Hex.CenterY), new Point(TargetHex.CenterX, TargetHex.CenterY), WeaponGroup.Any);
            Inner.Children.Add(arrow);
            HexArrows.Add(arrow);
            //HexCanvas.MovedShips.Add(ship.HexShip);
            CopyShip copy = new CopyShip(ship);
            Inner.Children.Add(copy);
            Canvas.SetLeft(copy, ship.HexShip.Hex.CenterX - copy.Width / 2);
            Canvas.SetTop(copy, ship.HexShip.Hex.CenterY - copy.Height / 2);
            copy.Transform.Angle = ship.Angle;
            CopyShip.Add(copy);
            if (!MoveShipsList.ContainsKey(ship))
            {

                MoveShipsList.Add(ship, new ShipRealParams(ship));
            }
            ship.PutShipToHexNotReal(TargetHex, angle, TargetHex.ID);

        }
        
        static DoubleAnimation BigTextAnim;
        public static void WriteBigText(string text, Color Color, double seconds)
        {
            BigTextLabel.Content = text;
            BigTextBorder.Background = new SolidColorBrush(Color);
            if (Inner.Children.Contains(BigTextGrid))
                BigTextAnim.Completed -= new EventHandler(anim_Completed);
            else
                Inner.Children.Add(BigTextGrid);
            BigTextAnim = new DoubleAnimation(2, 0, TimeSpan.FromSeconds(seconds));
            BigTextAnim.Completed += new EventHandler(anim_Completed);
            BigTextBorder.BeginAnimation(Border.OpacityProperty, BigTextAnim);
            BattleController.AddWorkingDelay(TimeSpan.FromSeconds(Links.ShootAnimSpeed));
        }

        static void anim_Completed(object sender, EventArgs e)
        {
            Inner.Children.Remove(BigTextGrid);
        }

        public static void PutEffectsGrid(List<SecondaryEffect> group, bool isIncrease, Hex hex, Hex donor)
        {
            List<DamageLabel> list = new List<DamageLabel>();
            foreach (SecondaryEffect effect in group)
                list.Add(new DamageLabel(effect, isIncrease));
            Grid grid = new Grid();
            Canvas.SetZIndex(grid, Links.ZIndex.Effects);
            switch (list.Count)
            {
                case 1: grid.Width = 120; grid.Height = 80; break;
                case 2: grid.Width = 120; grid.Height = 160; grid.RowDefinitions.Add(new RowDefinition()); grid.RowDefinitions.Add(new RowDefinition()); break;
                case 3: 
                case 4: grid.Width = 240; grid.Height = 160; grid.RowDefinitions.Add(new RowDefinition()); grid.RowDefinitions.Add(new RowDefinition()); 
                    grid.ColumnDefinitions.Add(new ColumnDefinition()); grid.ColumnDefinitions.Add(new ColumnDefinition()); break;
            }
            for (int i = 0; i < list.Count; i++)
            {
                grid.Children.Add(list[i]);
                int row = i % 2;
                int column = (i - row) / 2;
                Grid.SetRow(list[i], row);
                Grid.SetColumn(list[i], column);
            }
            DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(Links.ShootAnimSpeed*3));
            Inner.Children.Add(grid);
            
            EffectsGrid.Add(grid);
            Canvas.SetLeft(grid, donor.CenterX -grid.Width / 2);
            Canvas.SetTop(grid, donor.CenterY - grid.Height / 2);
            anim.Completed += new EventHandler(GridEffectsAnim_Completed);
            grid.BeginAnimation(Grid.OpacityProperty, anim);
            double delta = Math.Sqrt(Math.Pow(hex.CenterX - donor.CenterY, 2) + Math.Pow(hex.CenterY - donor.CenterY, 2));
            DoubleAnimation horizanim = new DoubleAnimation(donor.CenterX - grid.Width / 2, hex.CenterX - grid.Width / 2, TimeSpan.FromSeconds(delta / Links.ShipMoveSpeed*3));
            DoubleAnimation vertanim = new DoubleAnimation(donor.CenterY - grid.Height / 2, hex.CenterY - grid.Height / 2, TimeSpan.FromSeconds(delta / Links.ShipMoveSpeed*3));
            grid.BeginAnimation(Canvas.LeftProperty, horizanim);
            grid.BeginAnimation(Canvas.TopProperty, vertanim);
            BattleController.AddWorkingDelay(TimeSpan.FromSeconds(Links.ShootAnimSpeed ));
        }
        static List<Grid> EffectsGrid=new List<Grid>();
        static void GridEffectsAnim_Completed(object sender, EventArgs e)
        {
            foreach (Grid grid in EffectsGrid)
                Inner.Children.Remove(grid);
            EffectsGrid.Clear();
        }
        public static void PutOneDamageLabel(Hex hex, DamageLabelTarget target, string text)
        {
            if (hex == null) return;
            DamageLabel dl = new DamageLabel(target, text);
            dl.Place(new Point(hex.CenterX, hex.CenterY), Inner);

        }
        public static void PutTwoDamageLabel(Hex hex, DamageLabelTarget target1, string text1, DamageLabelTarget target2, string text2)
        {
            if (hex == null) return;
            DamageLabel dl1 = new DamageLabel(target1, text1);
            dl1.Place(new Point(hex.CenterX, hex.CenterY - 35), Inner);
            DamageLabel dl2 = new DamageLabel(target2, text2);
            dl2.Place(new Point(hex.CenterX, hex.CenterY + 35), Inner);
        }
        public static void PutThreeDamageLabel(Hex hex, DamageLabelTarget target1, string text1, DamageLabelTarget target2, string text2, DamageLabelTarget target3, string text3)
        {
            if (hex == null) return;
            DamageLabel dl1 = new DamageLabel(target1, text1);
            dl1.Place(new Point(hex.CenterX, hex.CenterY - 70), Inner);
            DamageLabel dl2 = new DamageLabel(target2, text2);
            dl2.Place(new Point(hex.CenterX, hex.CenterY), Inner);
            DamageLabel dl3 = new DamageLabel(target3, text3);
            dl3.Place(new Point(hex.CenterX, hex.CenterY + 70), Inner);
        }
        /// <summary> Расчёт вероятности попадания. Если  Int32.MinValue - то заблокировано </summary>
        public static int CalculateAccuracy(ShipB Gunner, ShipB Target, int weaponPos)
        {
            byte hex1 = Gunner.HexShip.Hex.ID;
            byte hex2 = Target.HexShip.Hex.ID;
            int sizedx = Gunner.States.BigSize ? 3 : 0;
            int Distance = IntBoya.Battle.Field.DistAngleRot[hex1, hex2, weaponPos+sizedx, 0];
            int IntersectedShips = 0;
            foreach (byte b in IntBoya.Battle.Field.IntersectHexes[hex1, hex2, weaponPos+sizedx])
                if (IntBoya.Battle.Asteroids.ContainsKey(b)) return Int32.MinValue;
            if (IntBoya.Battle.Field.IntersectHexes[hex1, hex2, weaponPos+sizedx] != null)
                foreach (byte b in IntBoya.Battle.Field.IntersectHexes[hex1, hex2, weaponPos+sizedx])
                    if (Gunner.Battle.BattleField.ContainsKey(b))
                        if (Gunner.Battle.BattleField[b].States.Side != Gunner.States.Side) IntersectedShips++;
            BattleWeapon weapon = Gunner.Weapons[weaponPos];
            int Accuracy = 
                weapon.Accuracy()+ (Gunner.Battle.Restriction == Restriction.DoubleAccuracy ? 70 : Gunner.Battle.Restriction == Restriction.LowAccuracy ? -70 : 0)
                - (int)((Distance - 250) / 25) * Links.Modifiers[weapon.Type].Accuracy
                - IntersectedShips * 20
                - Target.Params.Evasion.GetCurValue(weapon.Group);
            Accuracy = (Accuracy / (Target.Params.Status.IsMoving + 1));
            if (Target.Params.Status.IsMarked > 0) Accuracy += 50;
            //if (Target.Params.Status.IsSlowed > 0) Accuracy += 60;
            //Accuracy = weapon.Type == EWeaponType.Time ? 100 : Accuracy;
            Accuracy = Accuracy > 100 ? 100 : Accuracy;
            if (Distance < 350) Accuracy = 100;
            //Accuracy = Accuracy >= 20 ? Accuracy : 20;
            return Accuracy;
        }
        
        static Canvas FlashCanvas;
        public static void MakeFlash(Brush brush)
        {
            if (FlashCanvas != null)
                Inner.Children.Remove(FlashCanvas);
            FlashCanvas = new Canvas();
            FlashCanvas.Width = Inner.Width;
            FlashCanvas.Height = Inner.Height;
            FlashCanvas.Background = brush;
            FlashCanvas.Opacity = 0;
            Canvas.SetZIndex(FlashCanvas, Links.ZIndex.SolarFlash);
            Inner.Children.Add(FlashCanvas);
            DoubleAnimation FlashAnimStart = new DoubleAnimation(0, 2, TimeSpan.FromSeconds(Links.ShootAnimSpeed / 3));
            FlashAnimStart.Completed += new EventHandler(FlashAnimStart_Completed);
            FlashCanvas.BeginAnimation(Canvas.OpacityProperty, FlashAnimStart);

        }

        static void FlashAnimStart_Completed(object sender, EventArgs e)
        {
            DoubleAnimation FlashAnimEnd = new DoubleAnimation(2, 0, TimeSpan.FromSeconds(Links.ShootAnimSpeed * 1.5));
            FlashAnimEnd.Completed += new EventHandler(FlashAnimEnd_Completed);
            if (FlashCanvas != null)
                FlashCanvas.BeginAnimation(Canvas.OpacityProperty, FlashAnimEnd);
        }

        static void FlashAnimEnd_Completed(object sender, EventArgs e)
        {
            if (FlashCanvas != null)
            {
                Inner.Children.Remove(FlashCanvas);
                FlashCanvas = null;
            }
        }
        
    }
    class HexCanvasPopup
    {
        static ShipPanel2 CurPanel;
        static HexShip Ship;
        static Viewbox Box = new Viewbox();
        public static void Place(HexShip ship)
        {
            if (Ship == ship) return;
            if (CurPanel!=null) { HexCanvas.Inner.Children.Remove(Box); CurPanel = null; Ship = null; }
            if (ship.Ship.Panel2 == null) return;
            if (ship.Hex == null) return;
            ship.Ship.Panel2.Update();
            Box.Width = HexCanvas.Inner.Width / 3;
            Box.Height = Box.Width / 1.78;
            Box.Child = ship.Ship.Panel2;
            HexCanvas.Inner.Children.Add(Box);
            CurPanel = ship.Ship.Panel2;
            Ship = ship;
            Canvas.SetZIndex(Box, 250);
            Put();
        }
        public static void Remove()
        {
            if (CurPanel != null) { HexCanvas.Inner.Children.Remove(Box); CurPanel = null; Ship = null; }
        }
        static void Put()
        {
            if (Ship.Hex.CenterX<HexCanvas.Inner.Width/2)
                Canvas.SetLeft(Box, HexCanvas.Inner.Width - Box.Width - 400);
            else
                Canvas.SetLeft(Box, 400);
            if (Ship.Hex.CenterY<HexCanvas.Inner.Height/2)
                Canvas.SetTop(Box, HexCanvas.Inner.Height - Box.Height - 300);
            else
                Canvas.SetTop(Box, 300);
        }
    }
    public enum DamageLabelTarget { Health, Shield, Energy, Miss };
     class DamageLabel : Grid
     {
         static Brush HealthColor = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
         static Brush ShieldColor = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
         static Brush EnergyColor = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
         static Brush MissColor = new SolidColorBrush(Color.FromArgb(255, 255, 215, 0));
         static Brush CritColor = Brushes.Black;
         Rectangle rect;
         Label lbl;
         void CreateDamageLabel(int width)
         {
             Canvas.SetZIndex(this, Links.ZIndex.SmallTablets);
             Width = width; Height = 80;
             rect = new Rectangle();
             rect.Width = width; rect.Height = 80; rect.RadiusX = 20; rect.RadiusY = 20;
             lbl = new Label();
             lbl.HorizontalAlignment = HorizontalAlignment.Center;
             lbl.VerticalAlignment = VerticalAlignment.Center;
             lbl.FontFamily = Links.Font;
             lbl.FontWeight = FontWeights.Bold;
             lbl.Foreground = Brushes.White;
             lbl.FontSize = 50.0;
         }
         public DamageLabel(string CritPos)
         {
             CreateDamageLabel(100);
             Children.Add(rect);
             Children.Add(WeapCanvas.GetCanvas((EWeaponType)Int32.Parse(CritPos), 30, false));
            
         }
         public DamageLabel(DamageLabelTarget target, string value)
         {
             CreateDamageLabel(100);
             Children.Add(rect);
             switch (target)
             {
                 case DamageLabelTarget.Health: rect.Fill = HealthColor; break;
                 case DamageLabelTarget.Shield: rect.Fill = ShieldColor; break;
                 case DamageLabelTarget.Energy: rect.Fill = EnergyColor; break;
                 case DamageLabelTarget.Miss: rect.Fill = MissColor; break;
             }
             Children.Add(lbl);
             lbl.Content = value;
         }
         public DamageLabel(SecondaryEffect effect, bool isIncrease)
         {
             CreateDamageLabel(120);
             Children.Add(rect);
             if (isIncrease) rect.Fill = MissColor;
             else rect.Fill = Brushes.Black;
             Viewbox vbx = new Viewbox();
             StackPanel panel = new StackPanel();
             vbx.Width = 120; vbx.Height = 80;
             vbx.Child = panel;
             panel.Orientation = Orientation.Horizontal;
             Children.Add(vbx);
             panel.HorizontalAlignment = HorizontalAlignment.Center;
             panel.VerticalAlignment = VerticalAlignment.Center;
             UIElement element = Common.GetRectangle(30, effect.Brush);
             //UIElement element = Pictogram.GetPict(effect.Pict, effect.Target, false, "");
             panel.Children.Add(element);
             lbl.Content = effect.Value.ToString();
             lbl.FontSize = 30;
             panel.Children.Add(lbl);
             
         }
         public void Place(Point pt, Canvas canvas)
         {
             DoubleAnimation anim = new DoubleAnimation(1.5, 0, TimeSpan.FromSeconds(Links.ShootAnimSpeed));
             canvas.Children.Add(this);
             Canvas.SetLeft(this, pt.X - Width / 2);
             Canvas.SetTop(this, pt.Y - Height / 2);
             anim.Completed += new EventHandler(anim_Completed);
             BeginAnimation(Grid.OpacityProperty, anim);
             BattleController.AddWorkingDelay(TimeSpan.FromSeconds(Links.ShootAnimSpeed));
         }

         void anim_Completed(object sender, EventArgs e)
         {
         
             //Panel panel = (Panel)this.Parent;
             HexCanvas.Inner.Children.Remove(this);
             //panel.Children.Remove(this);
             GC.Collect();
         }
         
     }
    class Gate:Canvas
    {
        static Brush AttackBrush = Gets.GetIntBoyaImage("HexRed");// new ImageBrush(new BitmapImage(new Uri("C:/123/HexRed.png")));
        static Brush DefenseBrush = Gets.GetIntBoyaImage("HexGreen");// new ImageBrush(new BitmapImage(new Uri("C:/123/HexGreen.png")));
        bool IsAttacker;
        public Hex[] Hexes = new Hex[5];
        public Gate(bool isAttacker, double dx, double dy)
        {
            IsAttacker=isAttacker;
            Width = 300;
            Height = 1430;
            for (int i = 0; i < 5; i++)
            {
                Hexes[i] = new Hex((byte)i,(byte)i,0);
                Hexes[i].SetD(dx, dy);
                Hexes[i].CreatePath();
                Hexes[i].SetGateBrush(isAttacker);
                //if (isAttacker)
                //    Hexes[i].rect.Fill = AttackBrush;
                //else
                //    Hexes[i].rect.Fill = DefenseBrush;
                Hexes[i].IsGate = true;
                Children.Add(Hexes[i].canvas);
            }
        }

    }
    class HexArrow : Canvas
    {
        static PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
        public HexArrow(Point pt1, Point pt2, WeaponGroup group)
        {
            Canvas.SetZIndex(this, Links.ZIndex.Arrows);
            double length = CalcLength(pt1, pt2);
            Width = 15;
            Canvas.SetLeft(this, pt1.X - Width / 2);
            Canvas.SetTop(this, pt1.Y);
            RotateTransform transform = new RotateTransform();
            this.RenderTransformOrigin = new Point(0.5, 0);
            Height = length;
            transform.Angle = (180 + CalcAngle(pt1, pt2)) % 360;
            this.RenderTransform = transform;
            Path path = new Path();
            path.StrokeThickness = 5;
            path.StrokeStartLineCap = PenLineCap.Round;
            path.StrokeEndLineCap = PenLineCap.Round;
            path.StrokeDashArray = new DoubleCollection(new double[] { 5, 1 });
            path.Data = new PathGeometry((PathFigureCollection)(conv.ConvertFrom(string.Format("M7.5,0 v{0:0}", length))));
            Children.Add(path);

            Path p2 = new Path();
            p2.StrokeThickness = 5;
            p2.StrokeStartLineCap = PenLineCap.Round;
            p2.StrokeEndLineCap = PenLineCap.Round;
            p2.Data = new PathGeometry((PathFigureCollection)(conv.ConvertFrom(string.Format("M7.5,{0:0} l-7.5,-7.5 M7.5,{0:0} l7.5,-7.5", length))));
            Children.Add(p2);

            switch (group)
            {
                case WeaponGroup.Energy: path.Stroke = Brushes.Blue; p2.Stroke = Brushes.Blue; break;
                case WeaponGroup.Physic: path.Stroke = Brushes.Red; p2.Stroke = Brushes.Red; break;
                case WeaponGroup.Irregular: path.Stroke = Brushes.Violet; p2.Stroke = Brushes.Violet; break;
                case WeaponGroup.Cyber: path.Stroke = Brushes.Green; p2.Stroke = Brushes.Green; break;
                case WeaponGroup.Any: path.Stroke = Brushes.White; p2.Stroke = Brushes.White; break;
            }
        }
        public static double CalcLength(Point CenterPoint, Point TargetPoint)
        {
            return Math.Sqrt(Math.Pow(TargetPoint.X - CenterPoint.X, 2) + Math.Pow(TargetPoint.Y - CenterPoint.Y, 2));
        }
        public static double CalcAngle(Point CenterPoint, Point TargetPoint)
        {
            double dx = TargetPoint.X - CenterPoint.X; double dy = CenterPoint.Y - TargetPoint.Y;
            double alpha;
            if (dx >= 0 && dy > 0) alpha = Math.Atan(dx / dy) * 180 / Math.PI;
            else if (dx >= 0 && dy < 0) alpha = 180 + Math.Atan(dx / dy) * 180 / Math.PI;
            else if (dx <= 0 && dy < 0) alpha = 180 + Math.Atan(dx / dy) * 180 / Math.PI;
            else alpha = 360 + Math.Atan(dx / dy) * 180 / Math.PI;
            return alpha;
        }
    }
    class ShipRealParams
    {
        public Hex hex;
        public byte CurHex;
        public int Angle;
        public int BasicEnergy;
        public int BonusEnergy;
        public ShipRealParams(ShipB ship)
        {
            hex = ship.HexShip.Hex;
            CurHex = ship.CurHex;
            Angle = ship.Angle;
            BasicEnergy = ship.Params.Energy.BasicValue;
            BonusEnergy = ship.Params.Energy.BonusValue;
        }
    }
    /*
    class TargetAim:Canvas
    {
        static double x;
        static double y;
        public TargetAim(ShipB ship)
        {
            Canvas.SetZIndex(this, Links.ZIndex.Aims);
            GeometryGroup group = new GeometryGroup();
            Point CenterPoint = new Point(ship.HexShip.Hex.CenterX, ship.HexShip.Hex.CenterY);
            x = ship.HexShip.Hex.CenterX;
            y = ship.HexShip.Hex.CenterY;
            group.Children.Add(new EllipseGeometry(new Point(0, 0), 200, 200));
            Path path = new Path();
            path.StrokeThickness = 20;

            path.Data = group;
            Children.Add(path);
            path.Opacity = 0.7;
            path.Stroke = Brushes.Green;
            Canvas.SetLeft(this, CenterPoint.X);
            Canvas.SetTop(this, CenterPoint.Y);
            HexCanvas.Aim.Add(this);
            Opacity = 0.8;
            HexCanvas.Inner.Children.Add(this);
            //Tag = target;
        }
    }*/
    class ClientShootResult
    {
        public bool TimeCrit;
        public bool EmiCrit;
        public bool AntiCrit;
        public bool PsiCrit;
        public ShipB Target;
        public bool ShootResult { get; private set; }
        public bool AbilityResult { get; private set; }
        static byte SHOOT_MASK = 1;
        static byte ABILITY_MASK = 2;
        public byte MaskedResult { get; private set; }
        public int Angle { get; private set; }
        public int Accuracy { get; private set; }
        public bool IsAsteroidAttack;
        public bool Error = false;
        public ClientShootResult(ShipB Ship, int gun, Side side, ShipB target)
        {
            Target = target; if (target.CurHex>target.Battle.Field.MaxHex) { Error = true;  return; }
            BattleWeapon weapon = Ship.Weapons[gun];
            if (weapon.Type == EWeaponType.EMI)
            {
                int abilroll = ServerLinks.GetRandomByte(Ship.Battle, 100, 5); //ServerLinks.BattleRandom.Next(100);
                EmiCrit = abilroll <= ServerLinks.ShipParts.Modifiers[EWeaponType.EMI].Crit[weapon.Size];
            }
            if (weapon.Type == EWeaponType.Time)
            {
                int abiroll = ServerLinks.GetRandomByte(Ship.Battle, 100, 7); //ServerLinks.BattleRandom.Next(100);
                TimeCrit = abiroll <= ServerLinks.ShipParts.Modifiers[EWeaponType.Time].Crit[weapon.Size];
            }
            if (Ship.Params.Status.IsAntiCursed > 0)
            {
                AntiCrit = ServerLinks.GetRandomByte(Ship.Battle, 100, 8) > 49 ? true : false;
                //AntiWeaponCrit = ServerLinks.BattleRandom.Next(100) > 49 ? true : false;
            }
            if (AntiCrit == false && Ship.Params.Status.IsPsiCursed > 0)
            {
                PsiCrit = ServerLinks.GetRandomByte(Ship.Battle, 100, 9) > 49 ? true : false;
                //PsiWeaponCrit = ServerLinks.BattleRandom.Next(100) > 49 ? true : false;
                if (PsiCrit)
                {
                    if (side.OnField.Count == 1) PsiCrit = false;
                    else
                    {
                        List<byte> PsiTargets = new List<byte>();
                        foreach (ShipB ship in side.OnField)
                        {
                            if (ship == Ship) continue;
                            if (GetAccuracy(Ship, ship, gun, Ship.Battle.BattleField, false) <= 0) continue;
                            PsiTargets.Add(ship.BattleID);
                        }
                        if (PsiTargets.Count == 0) PsiCrit = false;
                        else
                        {
                            //    PsiTargets.Remove(Ship.BattleID);
                            byte psitargetid = side.Ships[PsiTargets[ServerLinks.GetRandomByte(Ship.Battle, PsiTargets.Count, 11)]].BattleID;
                            Target = side.Ships[psitargetid];
                            //psitargetid = Ships[PsiTargets[ServerLinks.BattleRandom.Next(PsiTargets.Count)]].BattleID;
                        }
                    }
                }
            }
            Accuracy = GetAccuracy(Ship, Target, gun, Ship.Battle.BattleField, true);
            if (Accuracy == 0)
                IsAsteroidAttack = true;
            CalculateAccuracy(Ship, Target, gun);
        }

        void CalculateAccuracy(ShipB MyShip, ShipB TargetShip, int weaponPos)
        {
            byte hex1 = MyShip.CurHex;
            byte hex2 = TargetShip.CurHex;
            BattleWeapon weapon = MyShip.Weapons[weaponPos];
            int sizedx = MyShip.States.BigSize ? 3 : 0;
            Angle = MyShip.Battle.Field.DistAngleRot[hex1, hex2, weaponPos + sizedx, 1];
            
            if (Accuracy >= 100)
                ShootResult = true;
            else if (Accuracy == 0)
                ShootResult = false;
            else
            {
                int shootroll = ServerLinks.GetRandomByte(MyShip.Battle, 100, 10);// ServerLinks.BattleRandom.Next(100);
                DebugWindow.AddTB1(String.Format("Ролл={0} ", shootroll), false);
                ShootResult = shootroll <= Accuracy;
            }

            if (ShootResult)
            {
                MaskedResult = SHOOT_MASK;
                if (weapon.Type == EWeaponType.EMI)
                {
                    AbilityResult = EmiCrit;
                }
                else if (weapon.Type == EWeaponType.Time)
                {
                    AbilityResult = TimeCrit;
                }
                else
                {
                    AbilityResult = GetCritResult(weapon, TargetShip);
                }
                if (AbilityResult)
                    MaskedResult = (byte)(MaskedResult | ABILITY_MASK);

            }
            else if (EmiCrit)
            {
                AbilityResult = EmiCrit;
                MaskedResult = ABILITY_MASK;
            }
            else if (TimeCrit)
            {
                AbilityResult = TimeCrit;
                MaskedResult = ABILITY_MASK;
            }
        }
        public static int GetAccuracy(ShipB myship, ShipB targetship, int weaponPos, SortedList<byte, ShipB> battlefield, bool real)
        {
            byte hex1 = myship.CurHex;
            byte hex2 = targetship.CurHex;
            if (hex1 > myship.Battle.Field.MaxHex || hex2 > myship.Battle.Field.MaxHex) return 0;
            BattleWeapon weapon = myship.Weapons[weaponPos];
            int sizedx = myship.States.BigSize ? 3 : 0;
            int Distance = myship.Battle.Field.DistAngleRot[hex1, hex2, weaponPos + sizedx, 0];
            int IntersectedShips = 0;
            Side side = myship.States.Side == ShipSide.Attack ? myship.Battle.Side1 : myship.Battle.Side2;
            if (myship.Battle.Field.IntersectHexes[hex1, hex2, weaponPos + sizedx] != null)
                foreach (byte b in myship.Battle.Field.IntersectHexes[hex1, hex2, weaponPos + sizedx])
                    if (battlefield.ContainsKey(b))
                    {
                        if (battlefield[b].States.Side != myship.States.Side) IntersectedShips++;
                        if (battlefield[b].States.Side == ShipSide.Neutral)
                        {
                            return 0;
                        }
                    }
            WeaponGroup group = weapon.Group;
            int Accuracy =
                weapon.Accuracy()+(myship.Battle.Restriction==Restriction.DoubleAccuracy?70:myship.Battle.Restriction==Restriction.LowAccuracy?-70:0)
                - (int)((Distance - 250) / 25) * ServerLinks.ShipParts.Modifiers[weapon.Type].Accuracy
                - IntersectedShips * 20
                - targetship.Params.Evasion.GetCurValue(group);
            Accuracy = (Accuracy / (targetship.Params.Status.IsMoving + 1));
            if (targetship.Params.Status.IsMarked > 0) Accuracy += 50;
            //if (targetship.Params.Status.IsSlowed > 0) Accuracy += 60;
            if (Distance < 350) Accuracy = 100;
            //Accuracy = Accuracy >= 20 ? Accuracy : 20;
            if (real)
            {
                DebugWindow.AddTB1(String.Format("Точность пушки={0} Дистанция={1} Движение={2}", weapon.Accuracy(), Distance, targetship.Params.Status.IsMoving), true);
                DebugWindow.AddTB1(String.Format("Модификатор={0} Помехи={1} Уклонение={2}",
                    ServerLinks.ShipParts.Modifiers[weapon.Type].Accuracy, IntersectedShips, targetship.Params.Evasion.GetCurValue(group)), true);
                DebugWindow.AddTB1(String.Format("Точность={0} ", Accuracy), true);
            }
            return Accuracy;
        }
        public static bool GetCritResult(BattleWeapon weapon, ShipB Target)
        {
            int abilroll = ServerLinks.GetRandomByte(Target.Battle, 100, 15);// ServerLinks.BattleRandom.Next(100);
            int immuneroll = ServerLinks.GetRandomByte(Target.Battle, 100, 8);
            DebugWindow.AddTB1(String.Format("Крит шанс={0} Крит ролл={1} Иммунитет={2} Иммун ролл={3}", ServerLinks.ShipParts.Modifiers[weapon.Type].Crit[weapon.Size], abilroll, Target.Params.Immune.GetCurValue(weapon.Group), immuneroll), true);
            bool AbilityResult = abilroll <= ServerLinks.ShipParts.Modifiers[weapon.Type].Crit[weapon.Size];
            bool ImmuneResult = immuneroll <= Target.Params.Immune.GetCurValue(weapon.Group);
            if (AbilityResult == true)
            {
                if (ServerLinks.ShipParts.Modifiers[weapon.Type].ImmunePierce == true)
                    return true;
                else
                    return !ImmuneResult;
            }
            else return false;
        }
    }
}
