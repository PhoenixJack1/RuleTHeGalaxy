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
    /*public class Bonuses:Canvas
    {
        int pos = 0;
        ShipB Ship;
        StackPanel stack;
        Canvas canvas2;
        public Bonuses (ShipB ship)
        {
            Ship = ship;
            Width = 350;
            Height = 350;
            stack = new StackPanel();
            stack.Orientation = Orientation.Vertical;
            stack.Width = 350;
            stack.Height = 150;
            Children.Add(stack);
            Background = Brushes.Black;
            canvas2 = new Canvas();
            canvas2.Width = 350;
            canvas2.Height = 200;
            Children.Add(canvas2);
            Canvas.SetTop(canvas2, 150);
            //Refresh();
        }
        public void AddEffect(SecondaryEffect effect, string reason)
        {
            TextBlock block = Common.GetBlock(16, "++++++"+effect.ToString() +" "+ reason, Brushes.White, new Thickness());
            stack.Children.Add(block);
        }
        public void RemoveEffect(SecondaryEffect effect, string reason)
        {
            TextBlock block = Common.GetBlock(16, "----"+effect.ToString()+" "+reason, Brushes.White, new Thickness());
            stack.Children.Add(block);
        }
        public void Refresh()
        {
            canvas2.Children.Clear();
            pos = 0;
            foreach (SecondaryEffect effect in Ship.GroupEffects)
            {
                PutBonus(Common.GetRectangle(30, effect.Brush), effect.Value);
                //PutBonus(Pictogram.GetPict(effect.Pict, effect.Target, true, ""), effect.Value);
            }
            if (Ship.Params.Health.Bonus > 0) PutEffect(Pictogram.HealthBrush, Ship.Params.Health.Bonus);
            if (Ship.Params.Health.RestBonus > 0) PutEffect(Pictogram.RestoreHealth, Ship.Params.Health.RestBonus);
            if (Ship.Params.Shield.Bonus > 0) PutEffect(Pictogram.ShieldBrush, Ship.Params.Shield.Bonus);
            if (Ship.Params.Shield.RestBonus > 0) PutEffect(Pictogram.RestoreShield, Ship.Params.Shield.RestBonus);
        }
        void PutBonus(UIElement element, int val)
        {
            canvas2.Children.Add(element); Canvas.SetLeft(element, pos % 8 * 70); Canvas.SetTop(element, pos / 8 * 35);
            TextBlock block = Common.GetBlock(16, val.ToString(), Brushes.White, new Thickness());
            canvas2.Children.Add(block); Canvas.SetLeft(block, pos % 8 * 70 + 30); Canvas.SetTop(block, pos / 8 * 35);
            pos++;
        }
        void PutEffect(Brush brush, int val)
        {
            Rectangle rect = Common.GetRectangle(30, brush);
            canvas2.Children.Add(rect); Canvas.SetLeft(rect, pos % 8 * 70); Canvas.SetTop(rect, pos / 8 * 35);
            TextBlock block = Common.GetBlock(16, val.ToString(), Brushes.White, new Thickness());
            canvas2.Children.Add(block); Canvas.SetLeft(block, pos % 8 * 70 + 30); Canvas.SetTop(block, pos / 8 * 35);
            pos++;
        }
    }
    */
    public class HexShip : Canvas
    {
        Point MiddleGun = new Point(135, 0);
        Point LeftGun = new Point(75, 30);
        Point RightGun = new Point(195, 30);
        Point MiddleGunCenter = new Point(150, 0);
        Point LeftGunCenter = new Point(90, 30);
        Point RightGunCenter = new Point(210, 30);
        internal Point CenterPoint = new Point(150, 130);
        internal Point CenterShip = new Point(150, 180);
        public ShieldField ShieldField;
        public RotateTransform Transform;
        public ScaleTransform Scale;
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
        //HexShipStatus IDStatus;
        public ShipB Ship;
        Point CocpitPoint;
        public BlockingStar Blocking;
        public Path SelectPath;
        //public Bonuses Bonuses;
        public HexShip()
        {

        }
        public void ShowFullInfo(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                e.Handled = true;
                HexCanvas.PopUpCloseDirect();
                Links.Controller.ShipPopUp.Place(Ship);
                //if (Parent.GetType() == typeof(System.Windows.Controls.Primitives.Popup))
                //    ((System.Windows.Controls.Primitives.Popup)Parent).IsOpen = false;
            }
        }
        public static HexShip GetLargeBuildingHexShip(ShipB ship)
        {
            HexShip bigship = new HexShip();
            Canvas.SetZIndex(bigship, Links.ZIndex.Ships);
            bigship.Ship = ship;
            bigship.Width = 600; bigship.Height = 520;
            ImageShipModel shipmodel = ImageShipModel.LargeBuildings[0];
            Rectangle MainRectangle = new Rectangle();
            MainRectangle.Width = shipmodel.MainSize.Width; MainRectangle.Height = shipmodel.MainSize.Height;
            MainRectangle.Fill = shipmodel.MainImage;
            bigship.Children.Add(MainRectangle);
            Canvas.SetLeft(MainRectangle, shipmodel.MainLocation.X); Canvas.SetTop(MainRectangle, shipmodel.MainLocation.Y);
            bigship.SelectPath = new Path(); bigship.SelectPath.Data = shipmodel.Geometry;
            bigship.Children.Add(bigship.SelectPath);

            bigship.SelectPath.Fill = Links.Brushes.SelectedPathBrush;
            bigship.SelectPath.Tag = bigship.Ship;
            Canvas.SetZIndex(bigship.SelectPath, 255);

            bigship.RenderTransformOrigin = new Point(0.5, 0.5);
            bigship.Transform = new RotateTransform();
            bigship.Scale = new ScaleTransform();
            TransformGroup transformgroup = new TransformGroup();
            transformgroup.Children.Add(bigship.Transform);
            transformgroup.Children.Add(bigship.Scale);
            bigship.RenderTransform = transformgroup;
            bigship.ShieldField = new ShieldField(bigship, bigship.Ship.Params.ShieldProtection, 720, -60, -100);

            bigship.Health = new RoundIndicator(ship.Params.Health.GetMax, ship.Params.Health.GetCurrent, RIType.Health);
            bigship.Health.SetBigSize();
            bigship.Children.Add(bigship.Health);
            Canvas.SetZIndex(bigship.Health, 3);
            Canvas.SetLeft(bigship.Health, 45);
            Canvas.SetTop(bigship.Health, 120);

            bigship.Shield = new RoundIndicator(ship.Params.Shield.GetMax, ship.Params.Shield.GetCurrent, RIType.Shield);
            bigship.Shield.SetBigSize();
            //bigship.Children.Add(bigship.Shield);
            Canvas.SetZIndex(bigship.Shield, 3);
            Canvas.SetLeft(bigship.Shield, 530);
            Canvas.SetTop(bigship.Shield, 120);

            bigship.Energy = new RoundIndicator(ship.Params.Energy.GetMax, ship.Params.Energy.GetCurrent, RIType.Energy);
            bigship.Energy.SetBigSize();
            //bigship.Children.Add(bigship.Energy);
            Canvas.SetZIndex(bigship.Energy, 3);
            Canvas.SetLeft(bigship.Energy, 160);
            Canvas.SetTop(bigship.Energy, 490);

            bigship.LeftStatusPoint = new Point(100, 200);
            bigship.RightStatusPoint = new Point(170, 200);
            bigship.CenterShip = new Point(300, 260);
            bigship.CenterPoint = new Point(300, 260);

            bigship.Blocking = new BlockingStar(bigship.CenterShip, 480, 120);
            Canvas.SetZIndex(MainRectangle, 1);

            bigship.Children.Add(bigship.Blocking.StarLabel);
            bigship.Children.Add(bigship.Blocking.PsiLabel);
            bigship.Children.Add(bigship.Blocking.AntiLabel);

            bigship.PreviewMouseDown += bigship.ShowFullInfo;
            return bigship;
        }
        public static HexShip GetBigShipHexShip(ShipB ship)
        {
            HexShip bigship = new HexShip();
            Canvas.SetZIndex(bigship, Links.ZIndex.Ships);
            bigship.Ship = ship;
            bigship.Width = 600; bigship.Height = 520;
            ImageShipModel shipmodel = ImageShipModel.BigShips[ship.Model[0]];
            ImageGunModel gunmodel = ImageShipModel.BigGuns[0];
            Rectangle MainRectangle = new Rectangle();
            MainRectangle.Width = shipmodel.MainSize.Width; MainRectangle.Height = shipmodel.MainSize.Height;
            MainRectangle.Fill = shipmodel.MainImage;
            bigship.Children.Add(MainRectangle);
            Canvas.SetLeft(MainRectangle, shipmodel.MainLocation.X); Canvas.SetTop(MainRectangle, shipmodel.MainLocation.Y);
            bigship.group = new CombinedGeometry();
            bigship.group.Geometry1 = shipmodel.Geometry;
            Rectangle Gun1 = null;
            Rectangle Gun2 = null;
            Rectangle Gun3 = null;

            switch (ship.Weapons.Length)
            {
                case 1:
                    if (bigship.Ship.Weapons[0] != null)
                    {
                        bigship.group.Geometry2 = gunmodel.GetGun(shipmodel.MiddleGunConnection);
                        Gun1 = gunmodel.GetRectangle(shipmodel.MiddleGunConnection);
                        bigship.Children.Add(Gun1);
                        if (shipmodel.MiddleGunIsUpper)
                            Canvas.SetZIndex(Gun1, 2);
                    }
                    break;
                case 2:
                    if (bigship.Ship.Weapons[0] != null)
                    {
                        bigship.group.Geometry2 = gunmodel.GetGun(shipmodel.LeftGunConnection);
                        Gun1 = gunmodel.GetRectangle(shipmodel.LeftGunConnection);
                        bigship.Children.Add(Gun1);
                        CombinedGeometry group1 = new CombinedGeometry();
                        group1.Geometry1 = bigship.group;
                        bigship.group = group1;
                        if (shipmodel.LeftGunIsUpper)
                            Canvas.SetZIndex(Gun1, 2);
                    }
                    if (bigship.Ship.Weapons[1] != null)
                    {
                        bigship.group.Geometry2 = gunmodel.GetGun(shipmodel.RightGunConnection);
                        Gun2 = gunmodel.GetRectangle(shipmodel.RightGunConnection);
                        bigship.Children.Add(Gun2);
                        if (shipmodel.MiddleGunIsUpper)
                            Canvas.SetZIndex(Gun2, 2);
                    }
                    break;
                case 3:
                    if (bigship.Ship.Weapons[0] != null)
                    {
                        bigship.group.Geometry2 = gunmodel.GetGun(shipmodel.LeftGunConnection);
                        Gun1 = gunmodel.GetRectangle(shipmodel.LeftGunConnection);
                        bigship.Children.Add(Gun1);
                        CombinedGeometry group1 = new CombinedGeometry();
                        group1.Geometry1 = bigship.group;
                        bigship.group = group1;
                        if (shipmodel.LeftGunIsUpper)
                            Canvas.SetZIndex(Gun1, 2);
                    }
                    if (bigship.Ship.Weapons[1] != null)
                    {
                        bigship.group.Geometry2 = gunmodel.GetGun(shipmodel.MiddleGunConnection);
                        Gun2 = gunmodel.GetRectangle(shipmodel.MiddleGunConnection);
                        bigship.Children.Add(Gun2);
                        CombinedGeometry group1 = new CombinedGeometry();
                        group1.Geometry1 = bigship.group;
                        bigship.group = group1;
                        if (shipmodel.MiddleGunIsUpper)
                            Canvas.SetZIndex(Gun2, 2);
                    }
                    if (bigship.Ship.Weapons[2] != null)
                    {
                        bigship.group.Geometry2 = gunmodel.GetGun(shipmodel.RightGunConnection);
                        Gun3 = gunmodel.GetRectangle(shipmodel.RightGunConnection);
                        bigship.Children.Add(Gun3);
                        if (shipmodel.RightGunIsUpper)
                            Canvas.SetZIndex(Gun3, 2);
                    }
                    break;
            }
            bigship.RenderTransformOrigin = new Point(0.5, 0.5);
            bigship.Transform = new RotateTransform();
            bigship.Scale = new ScaleTransform();
            TransformGroup transformgroup = new TransformGroup();
            transformgroup.Children.Add(bigship.Transform);
            transformgroup.Children.Add(bigship.Scale);
            bigship.RenderTransform = transformgroup;
            bigship.ShieldField = new ShieldField(bigship, bigship.Ship.Params.ShieldProtection, 720, -60,-100);

            //bigship.Health = new RoundIndicator(ship.Params.Health.GetMax, ship.Params.Health.GetCurrent, Colors.Red);
            bigship.Health= new RoundIndicator(ship.Params.Health.GetMax, ship.Params.Health.GetCurrent, RIType.Health);
            bigship.Health.SetBigSize();
            bigship.Children.Add(bigship.Health);
            Canvas.SetZIndex(bigship.Health, 3);
            Canvas.SetLeft(bigship.Health, 45);
            Canvas.SetTop(bigship.Health, 120);

            //bigship.Shield = new RoundIndicator(ship.Params.Shield.GetMax, ship.Params.Shield.GetCurrent, Colors.Blue);
            bigship.Shield = new RoundIndicator(ship.Params.Shield.GetMax, ship.Params.Shield.GetCurrent, RIType.Shield);
            bigship.Shield.SetBigSize();
            bigship.Children.Add(bigship.Shield);
            Canvas.SetZIndex(bigship.Shield, 3);
            Canvas.SetLeft(bigship.Shield, 530);
            Canvas.SetTop(bigship.Shield, 120);

            //bigship.Energy = new RoundIndicator(ship.Params.Energy.GetMax, ship.Params.Energy.GetCurrent, Colors.Green);
            bigship.Energy = new RoundIndicator(ship.Params.Energy.GetMax, ship.Params.Energy.GetCurrent, RIType.Energy);
            bigship.Energy.SetBigSize();
            bigship.Children.Add(bigship.Energy);
            Canvas.SetZIndex(bigship.Energy, 3);
            Canvas.SetLeft(bigship.Energy, 160);
            Canvas.SetTop(bigship.Energy, 490);

            bigship.LeftStatusPoint = new Point(100, 200);
            bigship.RightStatusPoint = new Point(170, 200);
            bigship.LeftGunCenter = gunmodel.GetFirePoint(shipmodel.LeftGunConnection);
            bigship.RightGunCenter = gunmodel.GetFirePoint(shipmodel.RightGunConnection);
            bigship.MiddleGunCenter = gunmodel.GetFirePoint(shipmodel.MiddleGunConnection);
            bigship.CenterShip = new Point(300, 260);
            bigship.CenterPoint = new Point(300, 260);


            bigship.Blocking = new BlockingStar(bigship.CenterShip, 480, 120);
            Canvas.SetZIndex(MainRectangle, 1);

            bigship.Children.Add(bigship.Blocking.StarLabel);
            bigship.Children.Add(bigship.Blocking.PsiLabel);
            bigship.Children.Add(bigship.Blocking.AntiLabel);
            bigship.SelectPath = new Path(); bigship.SelectPath.Data = bigship.group;
            bigship.Children.Add(bigship.SelectPath);

            bigship.SelectPath.Fill = Links.Brushes.SelectedPathBrush;
            bigship.SelectPath.Tag = bigship.Ship;
            Canvas.SetZIndex(bigship.SelectPath, 255);
            bigship.PreviewMouseDown += bigship.ShowFullInfo;
            return bigship;
        }
        /*public static HexShip GetCargoShipHexShip(ShipB ship)
        {
            HexShip cargoship = new HexShip();
            Canvas.SetZIndex(cargoship, Links.ZIndex.Ships);
            cargoship.Ship = ship;
            cargoship.Width = 300; cargoship.Height = 260;
            ShipModelComplect shipmodelcomplect = new ShipModelComplect(ship.Model);
            ImageShipModel shipmodel = shipmodelcomplect.Model;
            Rectangle MainRectangle = new Rectangle();
            MainRectangle.Width = shipmodel.MainSize.Width; MainRectangle.Height = shipmodel.MainSize.Height;
            MainRectangle.Fill = shipmodel.MainImage;
            cargoship.Children.Add(MainRectangle);
            Canvas.SetLeft(MainRectangle, shipmodel.MainLocation.X); Canvas.SetTop(MainRectangle, shipmodel.MainLocation.Y);
            
            cargoship.RenderTransformOrigin = new Point(0.5, 0.5);
            cargoship.Transform = new RotateTransform();
            cargoship.Scale = new ScaleTransform();
            TransformGroup transformgroup = new TransformGroup();
            transformgroup.Children.Add(cargoship.Transform);
            transformgroup.Children.Add(cargoship.Scale);
            cargoship.RenderTransform = transformgroup;
            cargoship.ShieldField = new ShieldField(cargoship, cargoship.Ship.Params.ShieldProtection, 360, -30, -50);

            cargoship.Health = new RoundIndicator(ship.Params.Health.GetMax, ship.Params.Health.GetCurrent, RIType.Health);
            cargoship.Children.Add(cargoship.Health);
            Canvas.SetZIndex(cargoship.Health, 3);
            Canvas.SetLeft(cargoship.Health, 45);
            Canvas.SetTop(cargoship.Health, 60);

            cargoship.Shield = new RoundIndicator(ship.Params.Shield.GetMax, ship.Params.Shield.GetCurrent, RIType.Shield);
            cargoship.Children.Add(cargoship.Shield);
            Canvas.SetZIndex(cargoship.Shield, 3);
            Canvas.SetLeft(cargoship.Shield, 255);
            Canvas.SetTop(cargoship.Shield, 60);

            cargoship.Energy = new RoundIndicator(ship.Params.Energy.GetMax, ship.Params.Energy.GetCurrent, RIType.Energy);
            //cargoship.Children.Add(cargoship.Energy);
            Canvas.SetZIndex(cargoship.Energy, 3);
            Canvas.SetLeft(cargoship.Energy, 90);
            Canvas.SetTop(cargoship.Energy, 225);

            cargoship.LeftStatusPoint = new Point(100, 200);
            cargoship.RightStatusPoint = new Point(170, 200);
            cargoship.CenterShip = new Point(150, 130);
            Canvas.SetZIndex(MainRectangle, 1);
            cargoship.SelectPath = new Path(); cargoship.SelectPath.Data = shipmodel.Geometry;
            cargoship.Children.Add(cargoship.SelectPath);
            //SelectPath.Fill = Brushes.Red;
            cargoship.SelectPath.Fill = Links.Brushes.SelectedPathBrush;
            cargoship.SelectPath.Tag = cargoship.Ship;
            Canvas.SetZIndex(cargoship.SelectPath, 255);
            cargoship.PreviewMouseDown += cargoship.ShowFullInfo;
            return cargoship;
        }*/
        public CombinedGeometry group;
        public HexShip(ShipB ship, byte id, ShipBMode mode)
        {
            Canvas.SetZIndex(this, Links.ZIndex.Ships);
            Ship = ship;
            Width = 300; Height = 260;
            //ShipModelComplect shipmodelcomplect = new ShipModelComplect(ship.Model);
            //ImageShipModel shipmodel = shipmodelcomplect.Model;
            ImageShipModel shipmodel = ImageShipModel.GetModelNew(ship.Model);
            Rectangle MainRectangle = new Rectangle();
            MainRectangle.Width = shipmodel.MainSize.Width; MainRectangle.Height = shipmodel.MainSize.Height;
            MainRectangle.Fill = shipmodel.MainImage;
            //MainRectangle.Fill = shipmodelcomplect.Brush;//shipmodel.MainImage;
            Children.Add(MainRectangle);
            Canvas.SetLeft(MainRectangle, shipmodel.MainLocation.X); Canvas.SetTop(MainRectangle, shipmodel.MainLocation.Y);

            group = new CombinedGeometry();
            group.Geometry1 = shipmodel.Geometry;
            Rectangle Gun1 = null;
            Rectangle Gun2 = null;
            Rectangle Gun3 = null;
            if (ship.Weapons[0]!= null)
            {
                ImageGunModel gunmodel1 = ImageShipModel.Guns[(int)Ship.Weapons[0].Type]; //0
                group.Geometry2 = gunmodel1.GetGun(shipmodel.LeftGunConnection);
                Gun1 = gunmodel1.GetRectangle(shipmodel.LeftGunConnection);
                Children.Add(Gun1);
                CombinedGeometry group1 = new CombinedGeometry();
                group1.Geometry1 = group;
                group = group1;
                if (shipmodel.LeftGunIsUpper)
                    Canvas.SetZIndex(Gun1, 2);
                LeftGunCenter = gunmodel1.GetFirePoint(shipmodel.LeftGunConnection);
            }
            if (ship.Weapons[1]!= null)
            {
                ImageGunModel gunmodel2 = ImageShipModel.Guns[(int)Ship.Weapons[1].Type]; //1
                group.Geometry2 = gunmodel2.GetGun(shipmodel.MiddleGunConnection);
                Gun2 = gunmodel2.GetRectangle(shipmodel.MiddleGunConnection);
                Children.Add(Gun2);
                CombinedGeometry group1 = new CombinedGeometry();
                group1.Geometry1 = group;
                group = group1;
                if (shipmodel.MiddleGunIsUpper)
                    Canvas.SetZIndex(Gun2, 2);
                MiddleGunCenter = gunmodel2.GetFirePoint(shipmodel.MiddleGunConnection);
            }
            if (ship.Weapons[2]!= null)
            {
                ImageGunModel gunmodel3 = ImageShipModel.Guns[(int)Ship.Weapons[2].Type]; //(int)Ship.Schema.Weapons[2].Group//2
                group.Geometry2 = gunmodel3.GetGun(shipmodel.RightGunConnection);
                Gun3 = gunmodel3.GetRectangle(shipmodel.RightGunConnection);
                Children.Add(Gun3);
                if (shipmodel.RightGunIsUpper)
                    Canvas.SetZIndex(Gun3, 2);
                RightGunCenter = gunmodel3.GetFirePoint(shipmodel.RightGunConnection);
            }
          
            RenderTransformOrigin = new Point(0.5, 0.5);
            Transform = new RotateTransform();
            Scale = new ScaleTransform();
            TransformGroup transformgroup = new TransformGroup();
            transformgroup.Children.Add(Transform);
            transformgroup.Children.Add(Scale);
            RenderTransform = transformgroup;
            ShieldField = new ShieldField(this, Ship.Params.ShieldProtection, 360, -30, -50);

            Health = new RoundIndicator(ship.Params.Health.GetMax, ship.Params.Health.GetCurrent, RIType.Health);
            Children.Add(Health);
            Canvas.SetZIndex(Health, 3);
            Canvas.SetLeft(Health, 45);
            Canvas.SetTop(Health, 60);

            Shield = new RoundIndicator(ship.Params.Shield.GetMax, ship.Params.Shield.GetCurrent, RIType.Shield);
            Children.Add(Shield);
            Canvas.SetZIndex(Shield, 3);
            Canvas.SetLeft(Shield, 255);
            Canvas.SetTop(Shield, 60);

            Energy = new RoundIndicator(ship.Params.Energy.GetMax, ship.Params.Energy.GetCurrent, RIType.Energy);
            Children.Add(Energy);
            Canvas.SetZIndex(Energy, 3);
            Canvas.SetLeft(Energy, 90);
            Canvas.SetTop(Energy, 225);

            LeftStatusPoint = new Point(100,200);
            RightStatusPoint = new Point(170, 200);
            CenterShip = new Point(150, 130);
            
            Blocking = new BlockingStar(CenterShip, 240, 60);
            Canvas.SetZIndex(MainRectangle, 1);
            Children.Add(Blocking.StarLabel);
            Children.Add(Blocking.PsiLabel);
            Children.Add(Blocking.AntiLabel);
            SelectPath = new Path(); SelectPath.Data = group;
            Children.Add(SelectPath);
            //SelectPath.Fill = Brushes.Red;
            SelectPath.Fill = Links.Brushes.SelectedPathBrush;
            SelectPath.Tag = Ship;
            Canvas.SetZIndex(SelectPath, 255);
            if (mode == ShipBMode.Info && ship.StartHealth < 100)
            {
                Ellipse back = new Ellipse();
                back.Width = 250;
                back.Height = 240;
                Children.Add(back);
                Canvas.SetLeft(back, 25);
                Canvas.SetTop(back, 10);
                back.Fill = new SolidColorBrush(Color.FromArgb((byte)(255 - ship.StartHealth * 2), 255, 0, 0));
            }
            PreviewMouseDown += ShowFullInfo;
        }
        bool IsLighted = false;
        /// <summary> Выделяет корабль при наведении мышкой</summary>
        public void Lighted(bool value)
        {
            if (value)
            {
                if (IsSelected == false)
                {
                    SelectPath.StrokeThickness = 5;
                    if (Ship.States.Side == ShipSide.Attack) SelectPath.Stroke = Links.Brushes.SkyBlue;
                    else if (Ship.States.Side == ShipSide.Defense) SelectPath.Stroke = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    else if (Ship.States.Side == ShipSide.Neutral) SelectPath.Stroke = Brushes.White;
                }
                IsLighted = true;
            }
            else
            {
                if (IsSelected == false)
                    SelectPath.Stroke = null;
                IsLighted = false;
            }
        }
        bool IsSelected = false;
        /// <summary> Выделяет корабль кликом</summary>
        public void Select(bool value)
        {
            if (value)
            {
                SelectPath.StrokeThickness = 10;
                if (Ship.States.Side == ShipSide.Attack) SelectPath.Stroke = Links.Brushes.SkyBlue;
                else if (Ship.States.Side == ShipSide.Defense) SelectPath.Stroke = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                else if (Ship.States.Side == ShipSide.Neutral) SelectPath.Stroke = Brushes.White;
                IsSelected = true;
            }
            else
            {
                if (IsLighted)
                    SelectPath.StrokeThickness = 10;
                else
                    SelectPath.Stroke = null;
                IsSelected = false;
            }
        }
        public HexShip GetCopy()
        {
            HexShip copy;
            if (Ship.States.Side==ShipSide.Neutral) //Астероид
                copy= new AsteroidHex(Ship, Ship.BattleID);
            else
                copy = new HexShip(Ship, Ship.BattleID, ShipBMode.Battle);
            copy.Hex = HexCanvas.Hexes[0];
            Canvas.SetLeft(copy, copy.Hex.CenterX - copy.Width / 2);
            Canvas.SetTop(copy, copy.Hex.CenterY - copy.Height / 2);
            return copy;
        }
        public void AnimateDestroy()
        {
            BW.SetWorking("Ship Destroy Start", true);
            DestroyCloud cloud = new DestroyCloud(this);
            //new Cloud(0,100, this); new Cloud(-100, 0, this); new Cloud(100, 0, this);
            DoubleAnimation anim2 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(Links.ShootAnimSpeed/2));
            anim2.Completed += new EventHandler(ShipDestroy_Completed);
            this.BeginAnimation(Ellipse.OpacityProperty, anim2);
            if (Links.ShootAnimSpeed == 1)
                new MySound("Battle/Explosion.wav");
        }
        void ShipDestroy_Completed(object sender, EventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation(-1000, TimeSpan.Zero);
            this.BeginAnimation(Canvas.LeftProperty, anim);
            BW.SetWorking("Ship Destroy End", false);
            //BattleController.Working = false;
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
        PathGeometry GetGun(int pos)
        {
            string text = "M{0},{1} l2,2 l1,30 l1.5,3 l1.5,-3 l1,-30 l2,-2" +
                "l4,3 v2 l4,3 v14 a15,20 0 0,0 4,15 v30 a10,10 0 0,0 6,10 v25 a5,7 0 0,1 -9,0 v8 l-7,4 l-2,5 h-8" +
                "l-2,-5 l-7,-4 v-8 a5,7 0 0,1 -9,0 v-25 a10,10 0 0,0 6,-10 v-30 a15,20 0 0,0 4,-15 v-14 l4,-3 v-2 z";
            if (pos == 0) text = String.Format(text, "67", "33");
            else if (pos == 1) text = String.Format(text, "145", "52");
            else if (pos == 2) text = String.Format(text, "224", "33");
            return new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(text));
        }
        void PutGun(Point pt, Weaponclass weapon)
        {
            Canvas Image = WeapCanvas.GetCanvas(weapon.Type, 30, true);
            
            Children.Add(Image);
            Canvas.SetLeft(Image, pt.X);
            Canvas.SetTop(Image, pt.Y);
            Canvas.SetZIndex(Image, 1);
        }
        public void MoveToPoint(Point pt1, Point pt2, TimeSpan time)
        {
            double x1 = pt1.X - Width / 2;
            double y1 = pt1.Y - Height / 2;
            double x2 = pt2.X - this.Width / 2;
            double y2 = pt2.Y - this.Height / 2;
            DoubleAnimation leftanim = new DoubleAnimation(x1, x2, time);
            DoubleAnimation topanim = new DoubleAnimation(y1, y2, time);
            this.BeginAnimation(Canvas.LeftProperty, leftanim);
            this.BeginAnimation(Canvas.TopProperty, topanim);
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
            if (Double.IsNaN(da)) da = 0;
            DoubleAnimation rotateanim = new DoubleAnimation(Angle, rotate, TimeSpan.FromSeconds(da / Links.ShipRotateSpeed + 0.01));
            Transform.BeginAnimation(RotateTransform.AngleProperty, rotateanim);
            //Health.RotateTo(Angle - rotate);
            //Shield.RotateTo(Angle - rotate);
            //Energy.RotateTo(Angle - rotate);
            //IDStatus.RotateTo(Angle - rotate);
            if (LeftStatus != null) LeftStatus.RotateTo(Angle - rotate);
            if (RightStatus != null) RightStatus.RotateTo(Angle - rotate);
            //if (IDStatus != null) IDStatus.RotateTo(Angle - rotate);
            TimeSpan ts = TimeSpan.FromSeconds(da / Links.ShipRotateSpeed + Links.ShootAnimSpeed / 10);
            if (ts.Seconds > 10) throw new Exception();
            Angle = newangle;
            return ts;
        }
        public TimeSpan RotateTo(Hex hex)
        {
            double newangle = Common.CalcAngle(new Point(Hex.CenterX, Hex.CenterY), new Point(hex.CenterX, hex.CenterY));
            return RotateTo(newangle);
        }
        public void SetAngle(double angle)
        {
            //Health.SetAngle(Angle - angle);
            //Shield.SetAngle(Angle - angle);
            //Energy.SetAngle(Angle - angle);
            //IDStatus.SetAngle(Angle - angle);
            if (LeftStatus != null) LeftStatus.SetAngle(Angle - angle);
            if (RightStatus != null) RightStatus.SetAngle(Angle - angle);
            //if (IDStatus != null) IDStatus.SetAngle(Angle - angle);
            Angle = angle;
            DoubleAnimation anim = new DoubleAnimation(angle, Links.ZeroTime);
            Transform.BeginAnimation(RotateTransform.AngleProperty, anim);
            //Transform.Angle = angle;
        }
        /*public void SetSlow(bool value)
        {
            if (value)
            {
                if (LeftStatus != null) Children.Remove(LeftStatus);
                LeftStatus = PlaceLeftStatus(new HexShipStatus(EHexShipStasus.Slow, 0, Angle));
                LeftStatus.SetBig();
            }
            else if (RightStatus != null)
            {
                Children.Remove(LeftStatus);
                LeftStatus = null;
            }
        }*/
        public void SetRadiation(bool value)
        {
         /*   if (value)
            {
                if (LeftStatus != null) Children.Remove(LeftStatus);
                LeftStatus = PlaceLeftStatus(new HexShipStatus(EHexShipStasus.Radiation, 0, Angle));
                LeftStatus.SetBig();
            }
            else if (LeftStatus!= null)
            {
                Children.Remove(LeftStatus);
                LeftStatus = null;
            }*/
        }
        public void SetMarked(bool value)
        {
            if (value)
            {
                if (LeftStatus != null) Children.Remove(LeftStatus);
                LeftStatus = PlaceLeftStatus(new HexShipStatus(EHexShipStasus.Mark, 0, Angle));
            }
            else if (LeftStatus!= null)
            {
                Children.Remove(LeftStatus);
                LeftStatus = null;
            }
        }
        public void SetBlind(bool value)
        {
            if (value)
            {
                if (RightStatus != null) Children.Remove(RightStatus);
                RightStatus = PlaceRightStatus(new HexShipStatus(EHexShipStasus.Blind, 0, Angle));
                RightStatus.SetBig();
            }
            else if (RightStatus!=null)
            {
                Children.Remove(RightStatus);
                RightStatus = null;
            }
        }
        
        public void MoveShipToPort(byte hex, Hex GateHex, bool IsVisual)
        {
            Hex = GateHex;
         
        }
        HexShipStatus PlaceRightStatus(HexShipStatus status)
        {
            Children.Add(status);
            Canvas.SetLeft(status, RightStatusPoint.X);
            Canvas.SetTop(status, RightStatusPoint.Y);
            Canvas.SetZIndex(status, 1);
            return status;
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
        public Point GetMiddleGunPoint(double Angle)
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
        public Point GetLeftGunPoint(double Angle)
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
        public Point GetRightGunPoint(double Angle)
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
    class ShieldField2
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
        public ShieldField2(HexShip ship, byte ShieldSize)
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
            FlashTimer.Tick += new EventHandler(FlashTimer_Tick);
            switch (ShieldSize)
            {
                case 30: Waves.Add(GetWaveRectangle(0)); break;
                case 90:
                    Waves.Add(GetWaveRectangle(0));
                    Waves.Add(GetWaveRectangle(60));
                    Waves.Add(GetWaveRectangle(-60)); break;
                case 150:
                    Waves.Add(GetWaveRectangle(0));
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
                DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(Links.ShootAnimSpeed * 2));
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
                DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(Links.ShootAnimSpeed * 2));
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
    public class ShieldField
    {
        Rectangle ShieldEllipse;
        LinearGradientBrush shieldoppbrush;
        RotateTransform ShieldTransform;
        static Color v = Color.FromArgb(255, 255, 255, 255);
        static Color t = Color.FromArgb(0, 0, 0, 0);
        DispatcherTimer Timer;
        List<GradientStop> list;
        //static RadialGradientBrush WaveBrush = GetWaveBrush();
        //List<Rectangle> Waves = new List<Rectangle>();
        public bool IsVisible = true;
        public Ellipse Image;
        static PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
        static RadialGradientBrush MainBrush = GetMainBrush();
        static Brush Opacity300 = GetOpacityMask300();
        static Brush Opacity180 = GetOpacityMask180();
        static Brush Opacity60 = GetOpacityMask60();

        public ShieldField(HexShip ship, byte ShieldSize, int size, int dx, int dy)
        {
            ShieldEllipse = new Rectangle();
            ShieldEllipse.Width = size;// + dx;
            ShieldEllipse.Height = size; // + dx;
            //ShieldEllipse.Opacity = 0.8;
            //RadialGradientBrush brush = new RadialGradientBrush();
            //brush.GradientOrigin = new Point(0.8, 0.2);
            //brush.GradientStops.Add(new GradientStop(Colors.Blue, 1));
            //brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            //ShieldEllipse.Fill = brush;
            ShieldEllipse.Fill = Links.Brushes.Ships.ShipShield;
            ship.Children.Add(ShieldEllipse);
            Canvas.SetLeft(ShieldEllipse, dx );
            Canvas.SetTop(ShieldEllipse, dy );
            
            //Canvas.SetLeft(ShieldEllipse, dx / 2);
            //Canvas.SetTop(ShieldEllipse, (dy - dx) / 2);
            Canvas.SetZIndex(ShieldEllipse, 10);
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
            //Timer.Interval = TimeSpan.FromSeconds(0.33);
            Timer.Tick += new EventHandler(Timer_Tick);
            list = new List<GradientStop>();
            FlashTimer = new DispatcherTimer();
            FlashTimer.Tick+=new EventHandler(FlashTimer_Tick);
            Image = new Ellipse();
            Image.Width = size; Image.Height = size;
            Image.Fill = MainBrush;
            ship.Children.Add(Image);
            Canvas.SetLeft(Image, dx);
            Canvas.SetTop(Image, dy);
            if (ShieldSize == 150)
                Image.OpacityMask = Opacity300;
            else if (ShieldSize == 90)
                Image.OpacityMask = Opacity180;
            else if (ShieldSize == 30)
                Image.OpacityMask = Opacity60;
            else if (ShieldSize == 0)
                Image.Opacity = 0;
            
        }
        
        static RadialGradientBrush GetMainBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.5, 0.8);
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 32, 32, 255), 1));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(128, 80, 80, 255), 0.9));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 0, 0, 255), 0.89));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 255, 255, 255), 0.88));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 0, 0, 255), 0.87));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(128, 80, 80, 255), 0.86));
            brush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 32, 32, 255), 0.75));
            return brush;
        }
        static Brush GetOpacityMask300()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.5, 2.2);
            brush.GradientStops.Add(new GradientStop(Colors.White, 1));
            brush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.6));
            return brush;
        }

        static Brush GetOpacityMask180()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.Transparent, null, new PathGeometry((PathFigureCollection)conv.ConvertFrom(
                "M0,0 h100 v100 h-100 z"))));
            group.Children.Add(new GeometryDrawing(Brushes.White, null, new PathGeometry((PathFigureCollection)conv.ConvertFrom(
               "M50,0 a50,50 0 0,1 47,30 a8,8 0 0,1 -16,5 a35,15 0 0,0 -62,0 a8,8 0 0,1 -16,-5 a50,50 0 0,1 47,-30"))));
            LinearGradientBrush edgemask = new LinearGradientBrush();
            edgemask.StartPoint = new Point(0.5, 1);
            edgemask.EndPoint = new Point(0.5, 0);
            edgemask.GradientStops.Add(new GradientStop(Colors.White, 0.4));
            edgemask.GradientStops.Add(new GradientStop(Colors.Transparent, 0));
            group.Children.Add(new GeometryDrawing(edgemask, null, new PathGeometry((PathFigureCollection)conv.ConvertFrom(
               "M0,30 v20 h20 v-20z"))));
            group.Children.Add(new GeometryDrawing(edgemask, null, new PathGeometry((PathFigureCollection)conv.ConvertFrom(
               "M100,30 v20 h-20 v-20z"))));
            return new DrawingBrush(group);
        }
        static Brush GetOpacityMask60()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.Transparent, null, new PathGeometry((PathFigureCollection)conv.ConvertFrom(
                "M0,0 h100 v100 h-100 z"))));
            LinearGradientBrush edgemask = new LinearGradientBrush();
            edgemask.StartPoint = new Point(0, 0.5);
            edgemask.EndPoint = new Point(1, 0.5);
            edgemask.GradientStops.Add(new GradientStop(Colors.Transparent, 0));
            edgemask.GradientStops.Add(new GradientStop(Colors.White, 0.3));
            edgemask.GradientStops.Add(new GradientStop(Colors.White, 0.7));
            edgemask.GradientStops.Add(new GradientStop(Colors.Transparent, 1));
            group.Children.Add(new GeometryDrawing(edgemask, null, new PathGeometry((PathFigureCollection)conv.ConvertFrom(
               "M50,0 a50,50 0 0,1 20,3 a12,12 0 0,1 -10,20 h-20 a12,12 0 0,1 -10,-20 a50,50 0 0,1 20,-3"))));
            return new DrawingBrush(group);
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
                //foreach (Rectangle rect in Waves)
                //    rect.BeginAnimation(Rectangle.OpacityProperty, anim);
                Image.BeginAnimation(Ellipse.OpacityProperty, anim);
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
                Image.BeginAnimation(Ellipse.OpacityProperty, anim);
                //foreach (Rectangle rect in Waves)
                //    rect.BeginAnimation(Rectangle.OpacityProperty, anim);
                return;            
            }
        }
    }
    class AsteroidHex:HexShip
    {
        public AsteroidHex(ShipB ship, byte id)
        {
            Canvas.SetZIndex(this, Links.ZIndex.Portals);
            Ellipse el = new Ellipse();
            SelectPath = new Path();
            if (ship.States.BigSize == true)
            {
                Width = 520; Height = 520;
                el.Width = 520; el.Height = 520;
                CenterShip = new Point(300, 260);
                SelectPath.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,260 a260,260 0 1,1 0,0.1"));
            }
            else
            {
                Width = 260; Height = 260;
                el.Width = 260; el.Height = 260;
                CenterShip = new Point(150, 130);
                SelectPath.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,130 a130,130 0 1,1 0,0.1"));
            }
            Children.Clear();
            
            el.Fill = GetAsteroidModel(ship, id);

            Children.Add(el);
            
            SelectPath.Fill = Links.Brushes.SelectedPathBrush;
            SelectPath.Tag = Ship;
            Children.Add(SelectPath);

            RenderTransformOrigin = new Point(0.5, 0.5);
            Transform = new RotateTransform();
            Scale = new ScaleTransform();
            TransformGroup transformgroup = new TransformGroup();
            transformgroup.Children.Add(Transform);
            transformgroup.Children.Add(Scale);
            RenderTransform = transformgroup;
            Ship = ship;
        }
        /// <summary> Внешний вид астероида. 255 - случайный, 0-199 - сюжетый, 200-254 базовый
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Brush GetAsteroidModel(ShipB ship, byte id)
        {
            if (ship.Model[0] == 255)
                return Links.Brushes.Ships.Meteorits[(byte)((ship.BattleID + id + ship.CurHex) % 9 + 200)];
            else if (ship.Model[0] == 254)
                return Brushes.Transparent;
            else if (ship.Model[0] < 200)
                return StoryLine2.StoryAsteroids[ship.Model[0]];
            else
                return Links.Brushes.Ships.Meteorits[ship.Model[0]];
           /* switch (ship.Model[0])
            {
                case 0: return Links.Brushes.Ships.QuestMeteorit0;
                default: return Links.Brushes.Ships.Meteorits[(ship.BattleID + id + ship.CurHex) % Links.Brushes.Ships.Meteorits.Count];
            }*/
        }
    }
    class PortalB: HexShip
    {
        //static RadialGradientBrush PortalBrush = GetBrush();
       
        public PortalB(ShipB ship, byte id)
        {
            Canvas.SetZIndex(this, Links.ZIndex.Portals);
            Width = 260;
            Height = 260;
            Children.Clear();

            AddLayer(Links.Brushes.Ships.Portal6, 12);
            AddLayer(Links.Brushes.Ships.Portal5, 8);
            AddLayer(Links.Brushes.Ships.Portal4, 5);
            AddLayer(Links.Brushes.Ships.Portal3, 10);
            AddLayer(Links.Brushes.Ships.Portal2, 6);
            AddLayer(Links.Brushes.Ships.Portal1, 4);
            Ship = ship;

            
            CenterShip = new Point(150, 130);

            HealthPoint = new Point(130, 115);
            Health = new RoundIndicator(1000, 800, RIType.Health);
            Children.Add(Health);
            Canvas.SetLeft(Health, 45);
            Canvas.SetTop(Health, 60);
           SelectPath = new Path();
            SelectPath.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,130 a130,130 0 1,1 0,0.1"));
            SelectPath.Fill = Links.Brushes.SelectedPathBrush;
            SelectPath.Tag = Ship;
            Children.Add(SelectPath);

            RenderTransformOrigin = new Point(0.5, 0.5);
            Transform = new RotateTransform();
            Scale = new ScaleTransform();
            TransformGroup transformgroup = new TransformGroup();
            transformgroup.Children.Add(Transform);
            transformgroup.Children.Add(Scale);
            RenderTransform = transformgroup;
        }
        void AddLayer(Brush brush, int time)
        {
            Ellipse el = new Ellipse();
            el.Width = 360; el.Height = 360;
            el.Fill = brush;
            Children.Add(el);
            Canvas.SetLeft(el, -50); Canvas.SetTop(el, -50);
            if (Links.Animation == false) return;
            el.RenderTransformOrigin = new Point(0.5, 0.5);
            RotateTransform rotate = new RotateTransform();
            el.RenderTransform = rotate;
            DoubleAnimation anim = new DoubleAnimation(0, 360, TimeSpan.FromSeconds(time));
            anim.RepeatBehavior = RepeatBehavior.Forever;
            rotate.BeginAnimation(RotateTransform.AngleProperty, anim);
        }
        
    }
    public class BlockingStar
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
    class CopyShip:Canvas
    {
        public RotateTransform Transform;
        public CopyShip(ShipB ship)
        {
            Canvas.SetZIndex(this, Links.ZIndex.Arrows);
            Width = 300;
            Height = 260;
            RenderTransformOrigin = new Point(0.5, 0.5);
            Path path = new Path();
            Children.Add(path);
            path.Stroke = Brushes.White;
            path.StrokeThickness = 5;
            if (ship.HexShip.group != null)
                path.Data = ship.HexShip.group;
            else
            {
                ShipModelNew shipmodel = ShipModelNew.Models[ship.Model[0]];
                path.Data = shipmodel.GetShipGeometry(ship.Schema.ShipType.WeaponCapacity);
            }
            path.StrokeDashArray = new DoubleCollection(new double[] { 1 });
            Transform = new RotateTransform();
            this.RenderTransform = Transform;
        }
        
    }
   
    class DestroyCloud:Canvas
    {
        ImageBrush brush;
        //DateTime StartTime = DateTime.Now;
        public DestroyCloud(HexShip ship)
        {
            //brush = Links.Brushes.Explosion;
            brush= Gets.AddPic("S034_Explosion"); 
            Background = brush;
            Width = 500; Height = 500;
            RectAnimationUsingKeyFrames anim = new RectAnimationUsingKeyFrames();
            anim.Duration = TimeSpan.FromSeconds(1 * Links.ShootAnimSpeed);
            for (int i = 0; i < 48; i++)
            {
                Rect rect = new Rect(i % 8 * 0.125, (i / 8) / 6.0, 0.125, 1 / 6.0);
                anim.KeyFrames.Add(new DiscreteRectKeyFrame(rect, KeyTime.FromPercent(i / 47.0)));
            }
            anim.Completed += Anim_Completed;
            brush.BeginAnimation(ImageBrush.ViewboxProperty, anim);
            //brush.Viewbox = new Rect(0, 0, 0.125, 1.0 / 6);
            //CompositionTarget.Rendering += CompositionTarget_Rendering;
            HexCanvas.Inner.Children.Add(this);
            Canvas.SetLeft(this, ship.Hex.CenterX - 250);
            Canvas.SetTop(this, ship.Hex.CenterY - 250);
        }

        private void Anim_Completed(object sender, EventArgs e)
        {
            HexCanvas.Inner.Children.Remove(this);
        }
        /* int pos = 0;
private void CompositionTarget_Rendering(object sender, EventArgs e)
{
    DateTime now = DateTime.Now;
    TimeSpan ts = now - StartTime;
    pos = (int)(ts.TotalMilliseconds / 20.8);
    if(pos >= 48)
    {
        CompositionTarget.Rendering -= CompositionTarget_Rendering;
        HexCanvas.Inner.Children.Remove(this);
    }
    else
        brush.Viewbox = new Rect(pos % 8 * 0.125, pos / 8 * 1.0 / 6, 0.125, 1.0 / 6);
}*/
    }
    class Cloud
    {
        Canvas Parent;
        Rectangle rect;
        public Cloud(int x, int y, HexShip ship)
        {
            Parent = HexCanvas.Inner;
            rect = new Rectangle();
            rect.Width = 40; rect.Height = 40;
            rect.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform scale = new ScaleTransform();
            rect.Fill = Links.Brushes.CloudBrush;
            HexCanvas.Inner.Children.Add(rect);
            Canvas.SetLeft(rect,+ ship.Hex.CenterX+x);
            Canvas.SetTop(rect, ship.Hex.CenterY + y);
            Canvas.SetZIndex(rect, Links.ZIndex.Effects);
            DoubleAnimation scaleanim = new DoubleAnimation(1, 10, TimeSpan.FromSeconds(Links.ShootAnimSpeed));
            rect.RenderTransform = scale;
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleanim);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleanim);
            DoubleAnimation oppanim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(2*Links.ShootAnimSpeed));
            oppanim.Completed += new EventHandler(oppanim_Completed);
            rect.BeginAnimation(Rectangle.OpacityProperty, oppanim);
        }

        void oppanim_Completed(object sender, EventArgs e)
        {
            Parent.Children.Remove(rect);
        }
    }
    class ShipModelComplect
    {
        public ImageShipModel Model;
        public Brush Brush;
        public ShipModelComplect(ImageShipModel model, Brush brush)
        {
            Model = model; Brush = brush;
        }
        public ShipModelComplect(byte[] model)
        {
            switch (model[0])
            {
                case 252: //пират
                    if (model[1]==0)
                    {
                        Model = ImageShipModel.Ships[3]; Brush = Model.MainImage;
                    }
                    else
                    {
                        Model = ImageShipModel.Ships[4]; Brush = Model.MainImage;
                    }
                    break;
                case 251: //зелёный альянс
                    Model = ImageShipModel.Ships[1];
                    if (model[1] == 0)
                        Brush = Links.Brushes.Ships.ShipLG;
                    else if (model[1] == 1)
                        Brush = Links.Brushes.Ships.ShipMG;
                    else
                        Brush = Links.Brushes.Ships.ShipDG;
                    break;
                case 254: //технократы
                    Model = ImageShipModel.Ships[1];
                    if (model[1] == 0)
                        Brush = Links.Brushes.Ships.ShipLB;
                    else if (model[1] == 1)
                        Brush = Links.Brushes.Ships.ShipMB;
                    else
                        Brush = Links.Brushes.Ships.ShipDB;
                    break;
                case 255: //наёмники
                    Model = ImageShipModel.Ships[1];
                    if (model[1] == 0)
                        Brush = Links.Brushes.Ships.ShipW;
                    else if (model[1] == 1)
                        Brush = Links.Brushes.Ships.ShipG;
                    else
                        Brush = Links.Brushes.Ships.ShipB;
                    break;
                case 253: //вторжение
                    if (model[1] == 0)
                        Model = ImageShipModel.Ships[7];
                    else
                        Model = ImageShipModel.Ships[9];
                    Brush = Model.MainImage; break;

                case 250: //грузовой
                    Model = ImageShipModel.Ships[0];
                    Brush = Model.MainImage;
                    break;
                case 249: //Сюжетные
                    if (model[1] == 0)//Сюжетный корабль 1
                        Model = ImageShipModel.Ships[1];
                        Brush = Model.MainImage;
                    break;
                default:
                    if (model[0] % 3 == 0)
                    {
                        Model = ImageShipModel.Ships[5];
                        Brush = Model.GetSpecialImage(model);
                    }
                    else if (model[0]%3==1)
                    {
                        Model = ImageShipModel.Ships[6];
                        Brush = Model.GetSpecialImage(model);
                    }
                    else
                    {
                        Model = ImageShipModel.Ships[8];
                        Brush = Model.GetSpecialImage(model);
                    }
                    break;
                /*default:
                    Model = ImageShipModel.Ships[1];
                    int sum = model[0] + model[1] + model[2] + model[3];
                    if (sum % 3 == 0)
                        Brush = Links.Brushes.Ships.ShipLR;
                    else if (sum % 3 == 1)
                        Brush = Links.Brushes.Ships.ShipMR;
                    else
                        Brush = Links.Brushes.Ships.ShipDR;
                    break;
                    */
            }
        }
    }
}
