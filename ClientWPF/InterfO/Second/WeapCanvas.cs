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
    class WeapCanvas
    {
        static LinearGradientBrush EMIBrush = GetLinearbrush(false, Colors.Blue, 0.2, 0.8);
        static RadialGradientBrush PlasmaBrush = GetRadialbrush(0.8, 0.2, Colors.Blue, 0.7);
        static LinearGradientBrush SolarBodyBrush = GetLinearbrush(null, Colors.Blue, 0.7, 0.3);
        static LinearGradientBrush SolarBeamBrush = GetLinearbrush(false, Colors.Gold, 0.6, 0.4);
        static RadialGradientBrush SolarSunBrush = GetRadialbrush(0.8, 0.2, Colors.Gold, 0.7);
        static RadialGradientBrush CannonBulletBrush = GetRadialbrush(0.8, 0.1, Colors.Gray, 1);
        static RadialGradientBrush CannonBeltBrush = GetRadialbrush(0.8, 0.2, Colors.Red, 0.8);
        static LinearGradientBrush GaussBeamBrush = GetLinearbrush(false, Colors.DarkRed, 0.7, 0.3);
        static RadialGradientBrush GaussBulletBrush = GetRadialbrush(0.6, 0.2, Colors.Gray, 1);
        static RadialGradientBrush MissleBodyBrush = GetRadialbrush(0.6, 0.1, Colors.Gray, 0.7);
        public static RadialGradientBrush AntiBodyBrush = GetRadialbrush(0.8, 0.2, Colors.Red, 1);
        public static LinearGradientBrush AntiBeltBrush = GetLinearbrush(null, Colors.Gray, 0.7, 0.3);
        static SolidColorBrush DarkWaysBrush = new SolidColorBrush(Color.FromArgb(255, 255, 187, 0));
        static RadialGradientBrush DarkAtomBrush = GetRadialbrush(0.8, 0.2, Colors.Purple, 1);
        static RadialGradientBrush WarpBrush = GetWarpBrush();
        static RadialGradientBrush TimeVialBrush = GetTimeVialBrush();
        static RadialGradientBrush TimeSandBrush = GetTimeSandBrush();
        static RadialGradientBrush DroneBrush = GetDroneBrush();
        static RadialGradientBrush RadBaseBrush = GetRadialbrush(0.8, 0.2, Colors.Gray, 1);
        static RadialGradientBrush RadTopBrush = GetRadTopBrush();
        static LinearGradientBrush MagnetBodyBrush = GetMagnetBodyBrush();
        static RadialGradientBrush MagnetBeamBrush = GetMagnetBeamBrush();
        static RadialGradientBrush SwordBrush = GetSwordBrush();
        static RadialGradientBrush Shieldbrush = GetRadialbrush(0.9, 0.2, Colors.Black, 1);
        static Pen BasicPen = GetBasicPen();
        static PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
        public static Drawing MagnetDraw = GetMagnetDraw();
        public static Drawing RadiationDraw = GetRadiationDraw();
        public static Drawing DroneDraw = GetDroneDraw();
        public static Drawing SliceDraw = GetSliceDraw();
        public static Drawing TimeDraw = GetTimeDraw();
        public static Drawing WarpDraw = GetWarpDraw();
        public static Drawing DarkDraw = GetDarkDraw();
        public static Drawing PsiDraw = GetPsiDraw();
        public static Drawing AntiDraw = GetAntiDraw();
        public static Drawing MissleDraw = GetMissleDraw();
        public static Drawing GaussDraw = GetGaussDraw();
        public static Drawing CannonDraw = GetCannonDraw();
        public static Drawing SolarDraw = GetSolarDraw();
        public static Drawing PlasmaDraw = GetPlasmaDraw();
        public static Drawing EMIDraw = GetEMIDraw();
        public static Drawing LaserDraw = GetLaserDraw();
        public static SortedList<EWeaponType, Brush> GetBrushes()
        {
            SortedList<EWeaponType, Brush> list = new SortedList<EWeaponType, Brush>();
            list.Add(EWeaponType.Laser, Gets.AddGunIcon("laser"));
            list.Add(EWeaponType.EMI, Gets.AddGunIcon("EMI"));
            list.Add(EWeaponType.Plasma, Gets.AddGunIcon("Plasma"));
            list.Add(EWeaponType.Solar, Gets.AddGunIcon("Solar"));
            list.Add(EWeaponType.Cannon, Gets.AddGunIcon("Cannon"));
            list.Add(EWeaponType.Gauss, Gets.AddGunIcon("Gauss"));
            list.Add(EWeaponType.Missle, Gets.AddGunIcon("Missle"));
            list.Add(EWeaponType.AntiMatter, Gets.AddGunIcon("AntiMatter"));
            list.Add(EWeaponType.Psi, Gets.AddGunIcon("Psi"));
            list.Add(EWeaponType.Dark, Gets.AddGunIcon("DarkEnergy"));
            list.Add(EWeaponType.Warp, Gets.AddGunIcon("Warp"));
            list.Add(EWeaponType.Time, Gets.AddGunIcon("Time"));
            list.Add(EWeaponType.Slicing, Gets.AddGunIcon("Slice"));
            list.Add(EWeaponType.Radiation, Gets.AddGunIcon("Radiation"));
            list.Add(EWeaponType.Drone, Gets.AddGunIcon("Drone"));
            list.Add(EWeaponType.Magnet, Gets.AddGunIcon("Magnet"));
            /*for (int i = 0; i < 16; i++)
                list.Add((EWeaponType)i, new DrawingBrush(GetDrawing((EWeaponType)i)));*/
            return list;
        }
        public static Canvas GetCanvas(EWeaponType weapon, int size, bool HasBack)
        {
            Canvas result = new Canvas();
            result.Width = size; result.Height = size;
            if (HasBack)
                result.Background = GetColor(weapon);
            Label lbl = new Label();
            lbl.Width = size; lbl.Height = size; result.Children.Add(lbl);
            lbl.Background = Links.Brushes.WeaponsBrushes[weapon];
            return result;
        }
        /*
        public static Canvas GetCanvas(EWeaponType weapon, int size, bool HasBack)
        {
            Canvas result = new Canvas();
            result.Width = size; result.Height = size;
            if (HasBack)
                result.Background = GetColor(weapon);
            Label lbl = new Label();
            lbl.Width = size; lbl.Height = size; result.Children.Add(lbl);
            lbl.Background = new DrawingBrush(GetDrawing(weapon));
            return result;
        }
        */
        static SolidColorBrush PurpleBrush= new SolidColorBrush(Color.FromArgb(255, 255, 180, 180));
        public static SolidColorBrush GetColor(EWeaponType weapon)
        {
            switch ((int)weapon)
            {
                case 0:
                case 1:
                case 2:
                case 3: return Brushes.LightBlue;
                case 4:
                case 5:
                case 6:
                case 7: return PurpleBrush;
                case 8:
                case 9:
                case 10:
                case 11: return Brushes.Pink;
                default: return Brushes.LightGreen;
            }
        }
        public static Drawing GetDrawing(EWeaponType weapon)
        {
            switch (weapon)
            {
                case EWeaponType.Laser: return LaserDraw;
                case EWeaponType.EMI: return EMIDraw;
                case EWeaponType.Plasma: return PlasmaDraw;
                case EWeaponType.Solar: return SolarDraw;
                case EWeaponType.Cannon: return CannonDraw;
                case EWeaponType.Gauss: return GaussDraw;
                case EWeaponType.Missle: return MissleDraw;
                case EWeaponType.AntiMatter: return AntiDraw;
                case EWeaponType.Psi: return PsiDraw;
                case EWeaponType.Dark: return DarkDraw;
                case EWeaponType.Warp: return WarpDraw;
                case EWeaponType.Time: return TimeDraw;
                case EWeaponType.Slicing: return SliceDraw;
                case EWeaponType.Drone: return DroneDraw;
                case EWeaponType.Radiation: return RadiationDraw;
                case EWeaponType.Magnet: return MagnetDraw;
                default: return new GeometryDrawing();
            }
        }
        static Drawing GetMagnetDraw()
        {
            DrawingGroup result = new DrawingGroup();
            result.Children.Add(new GeometryDrawing(MagnetBodyBrush, BasicPen, GetPathGeometry("M7,15 h3 v5 a5,5 360 0,0 10,0v-5 h3 v5 a8,8 360 0,1 -16,0z"))); //body
            result.Children.Add(new GeometryDrawing(MagnetBeamBrush, null, GetPathGeometry("M7,15 l-3,-15 h9 l-3,15z  M28,0 v0.1 M2,0 v0.1"))); //left beam
            result.Children.Add(new GeometryDrawing(MagnetBeamBrush, null, GetPathGeometry("M20,15 l-3,-15 h9 l-3,15z"))); //right beam
            return result;
        }
        static Drawing GetRadiationDraw()
        {
            DrawingGroup result = new DrawingGroup();
            result.Children.Add(new GeometryDrawing(RadBaseBrush, BasicPen, new EllipseGeometry(new Point(15, 17), 12.5, 12.5))); //

            GeometryGroup shadegroup = new GeometryGroup();
            shadegroup.FillRule = FillRule.Nonzero;
            shadegroup.Children.Add(GetPathGeometry("M21.25,4.2 v2 L15,17 L8.75,6.2 v-2"));
            shadegroup.Children.Add(GetPathGeometry("M27.5,15 v2 A12.5,12.5 360 0,1 21.25,27.8 L15,17 v-2"));
            shadegroup.Children.Add(GetPathGeometry("M2.5,15 v2 A12.5,12.5 360 0,0 8.75,27.8 L15,17 l-1,-2"));
            result.Children.Add(new GeometryDrawing(Brushes.DarkGreen, BasicPen, shadegroup));

            GeometryGroup topgroup = new GeometryGroup();
            topgroup.Children.Add(GetPathGeometry("M15,15 L21.25,4.2 A12.5,12.5 360 0,0 8.75,4.2z"));
            topgroup.Children.Add(GetPathGeometry("M15,15 h12.5 A12.5,12.5 360 0,1 21.25,25.8z"));
            topgroup.Children.Add(GetPathGeometry("M15,15 h-12.5 A12.5,12.5 360 0,0 8.75,25.8z"));
            result.Children.Add(new GeometryDrawing(RadTopBrush, BasicPen, topgroup));

            return result;
        }

        public static Drawing GetDroneDraw()
        {
            DrawingGroup result = new DrawingGroup();

            GeometryGroup group = new GeometryGroup();
            group.Children.Add(new EllipseGeometry(new Point(15, 15), 7, 6));
            group.Children.Add(GetPathGeometry("M18,9.5 v-2 l-1,-1 v0.5 l0.7,0.7 v1.8z"));
            group.Children.Add(GetPathGeometry("M12,9.5 v-2 l1,-1 v0.5 l-0.7,0.7 v1.8z"));
            group.Children.Add(GetPathGeometry("M20,10.8 l3,-3 v-5 l1,9 l-1,-3 l-2.5,2.5z"));
            group.Children.Add(GetPathGeometry("M10,10.8 l-3,-3 v-5 l-1,9 l1,-3 l2.5,2.5z"));
            group.Children.Add(GetPathGeometry("M22,15 h2 v-1 l1,2 v5 l-1,-4 v-1 h-2z"));
            group.Children.Add(GetPathGeometry("M8,15 h-2 v-1 l-1,2 v5 l1,-4 v-1 h2z"));
            group.Children.Add(GetPathGeometry("M20.3,19 h1 l2,-1 l-2,8 l0.5,-6 h-3z"));
            group.Children.Add(GetPathGeometry("M9.7,19 h-1 l-2,-1 l2,8 l-0.5,-6 h3z"));
            result.Children.Add(new GeometryDrawing(DroneBrush, BasicPen, group));

            return result;
        }
        public static Drawing GetSliceDraw()
        {
            DrawingGroup result = new DrawingGroup();
            result.Children.Add(new GeometryDrawing(Brushes.White, BasicPen, GetPathGeometry("M2,2 a50,50 0 0,1 26,0 a50,50 0 0,1 0,20 c0,0 -13,31 -26,0 a50,50 0 0,1 0,-20 M33,0 v0.1 M-3,0 v0.1"))); //mask
            Pen BlackPen = new Pen(Brushes.DarkGreen, 1); BlackPen.LineJoin = PenLineJoin.Round;
            result.Children.Add(new GeometryDrawing(Brushes.Black, BlackPen, GetPathGeometry("M3,7 c0,0 3,-3 8,3 c0,0 -3,-6 -8,-3" +
                  "M27,7 c0,0 -2,-3 -8,3 c0,0 5,-6 8,-3 " +
                  "M4,12 a4,4 0 0,1 6,0 a4,4 0 0,1 -6,0" +
                  "M26,12 a4,4 0 0,1 -6,0 a4,4 0 0,1 6,0" +
                  "M5,20 h2 l3,4 h2 l2,-2 h2 l2,2 h2 l3,-4 h2" +
                  "l-4,6 h-4 l-2,-2 l-2,2 h-4z" +
                  "M12,27.5 h6" +
                  "M14,28.5 a2,2 0 0,1 0,2 a3,3 0 0,0 1,4 a3,3 0 0,0 1,-4 a 2,2 0 0,1 0,-2"))); //black
            Pen GrayPen = new Pen(Brushes.Gray, 1); GrayPen.LineJoin = PenLineJoin.Round;
            result.Children.Add(new GeometryDrawing(null, GrayPen, GetPathGeometry("M3,7 c0,0 3,-3 8,-1 M27,7 c0,0 -2,-3 -8,0" +
                  "M12.5,11 v7 a3,2 0 1,0 5,0 v-7" +
                  "M12.5,16 a5,5 0 0,0 -3,4 M17.5,16 a5,5 0 0,1 3,4" +
                  "M21,16 a4,6 0 0,0 5,0 a10,10 0 0,1 -4,12" +
                  "M8,16 a4,6 0 0,1 -5,0 a10,10 0 0,0 6,12"))); //gray
            return result;
        }

        public static Drawing GetTimeDraw()
        {
            DrawingGroup result = new DrawingGroup();
            result.Children.Add(new GeometryDrawing(TimeVialBrush, BasicPen, GetPathGeometry("M8,3 h14 v2 c0,6 -6,4 -6.5,10 c0,6 6,5 6.5,10 v2 h-14 v-2 c0,-5 7,-4 6.5,-10 c0,-5 -6,-5 -6.5,-10 v-2 M26,3 v0.1 M4,3 v0.1"))); //vial
            GeometryGroup sandgroup = new GeometryGroup();
            sandgroup.Children.Add(GetPathGeometry("M9.2,8 h12.1 c-5,4.5 -5.4,3.2 -5.8,7 l-0.4,0.5 l-0.4,-0.5 c-1,-3.0 0.2,-1.7 -5.3,-7"));
            sandgroup.Children.Add(GetPathGeometry("M15,19 c0.5,0.5 1,2 0,2 c-1,-0 -0.5,-1 0,-2"));
            sandgroup.Children.Add(GetPathGeometry("M8,25 h14 v2 h-14z"));
            GeometryDrawing sand = new GeometryDrawing(TimeSandBrush, null, sandgroup);
            result.Children.Add(sand);
            return result;
        }
        static Drawing GetWarpDraw()
        {
            DrawingGroup result = new DrawingGroup();
            result.Children.Add(new GeometryDrawing(WarpBrush, BasicPen, GetPathGeometry("M15,3 a5,5 360 0,0 0,5 a10,10 360 0,1 12,7 a5,5 360 0,0 -5,0 a10,10 360 0,1 -7,12" +
                                      "a5,5 360 0,0 0,-5 a10,10 360 0,1 -12,-7 a5,5 360 0,0 5,0 a10,10 360 0,1 7,-12"))); //body
            return result;
        }

        static Drawing GetDarkDraw()
        {
            DrawingGroup result = new DrawingGroup();
            Pen waypen = new Pen(Brushes.Black, 1);
            Point c = new Point(15, 15);
            GeometryGroup waysgroup = new GeometryGroup();
            waysgroup.Children.Add(new EllipseGeometry(c, 5, 12.5, new RotateTransform(45, 15, 15)));
            waysgroup.Children.Add(new EllipseGeometry(c, 5, 12.5));
            waysgroup.Children.Add(new EllipseGeometry(c, 12.5, 5));
            waysgroup.Children.Add(new EllipseGeometry(c, 5, 12.5, new RotateTransform(-45, 15, 15)));
            result.Children.Add(new GeometryDrawing(null, waypen, waysgroup));
            result.Children.Add(new GeometryDrawing(DarkAtomBrush, null, new EllipseGeometry(new Point(15, 2.5), 3, 3)));
            result.Children.Add(new GeometryDrawing(DarkAtomBrush, null, new EllipseGeometry(new Point(6, 6), 3, 3)));
            result.Children.Add(new GeometryDrawing(DarkAtomBrush, null, new EllipseGeometry(new Point(2.5, 15), 3, 3)));
            result.Children.Add(new GeometryDrawing(DarkAtomBrush, null, new EllipseGeometry(new Point(6, 24), 3, 3)));
            result.Children.Add(new GeometryDrawing(DarkAtomBrush, null, new EllipseGeometry(new Point(15, 27), 3, 3)));
            result.Children.Add(new GeometryDrawing(DarkAtomBrush, null, new EllipseGeometry(new Point(24, 24), 3, 3)));
            result.Children.Add(new GeometryDrawing(DarkAtomBrush, null, new EllipseGeometry(new Point(27.5, 15), 3, 3)));
            result.Children.Add(new GeometryDrawing(DarkAtomBrush, null, new EllipseGeometry(new Point(24, 6), 3, 3)));
            return result;
        }
        static Drawing GetPsiDraw()
        {
            DrawingGroup result = new DrawingGroup();
            RadialGradientBrush elfill = new RadialGradientBrush(); elfill.GradientOrigin = new Point(0.8, 0.3);
            elfill.GradientStops.Add(new GradientStop(Colors.Black, 1));
            elfill.GradientStops.Add(new GradientStop(Colors.White, 0));
            RadialGradientBrush psifill = new RadialGradientBrush(); psifill.GradientOrigin = new Point(0.8, 0.3);
            psifill.GradientStops.Add(new GradientStop(Colors.Purple, 1));
            psifill.GradientStops.Add(new GradientStop(Colors.White, 0));
            result.Children.Add(new GeometryDrawing(elfill, null, new EllipseGeometry(new Point(15, 15), 14, 14)));
            result.Children.Add(new GeometryDrawing(psifill, null, GetPathGeometry("M15,15 A14,14 0 0,0 15,1 A14,14 0 0,1 25,5 A14,14 0 0,1 15,15" +
                              "M15,15 A14,14 0 0,0 29,15 A14,14 0 0,1 25,25 A14,14 0 0,1 15,15" +
                              "M15,15 A14,14 0 0,0 15,29 A14,14 0 0,1 5,25 A14,14 0 0,1 15,15" +
                              "M15,15 A14,14 0 0,0 1,15 A14,14 0 0,1 5,5 A14,14 0 0,1 15,15"))); //lines
            return result;
        }
        static Drawing GetAntiDraw()
        {
            DrawingGroup result = new DrawingGroup();
            result.Children.Add(new GeometryDrawing(AntiBodyBrush, null, new EllipseGeometry(new Point(15, 15), 10, 10)));
            result.Children.Add(new GeometryDrawing(AntiBeltBrush, BasicPen, GetPathGeometry("M13,3  a12,12 360 0,1 4,0 a2,2 360 0,1 0,4 a4.5,4.5 360 0,0 6,6 a2,2 360 0,1 4,0" +
                                      "a12,12 360 0,1 0,4 a2,2 360 0,1 -4,0 a4.5,4.5 360 0,0 -6,6 a2,2 360 0,1 0,4" +
                                      "a12,12 360 0,1 -4,0 a2,2 360 0,1 0,-4 a4.5,4.5 360 0,0 -6,-6 a2,2 360 0,1 -4,0" +
                                      "a12,12 360 0,1 0,-4 a2,2 360 0,1 4,0 a4.5,4.5 360 0,0 6,-6 a2,2 360 0,1 0,-4")));
            return result;
        }
        static Drawing GetMissleDraw()
        {
            DrawingGroup result = new DrawingGroup();
            GeometryGroup bodygroup = new GeometryGroup();
            bodygroup.Children.Add(GetPathGeometry("M12,4 a2.6,3 360 0,1 6,0 a15,35 360 0,1 0,15 a5,10 360 0,1 -2,5 v3 h-2 v-3 a5,10 360 0,1 -2,-5 a15,35 360 0,1 0,-15"));
            bodygroup.Children.Add(GetPathGeometry("M18.4,15 a20,20 360 0,1 8,15 a20,20 360 0,0 -8.6,-10"));
            bodygroup.Children.Add(GetPathGeometry("M11.6,15 a20,20 360 0,0 -8,15 a20,20 360 0,1 8.6,-10"));
            GeometryDrawing missle = new GeometryDrawing(MissleBodyBrush, BasicPen, bodygroup);
            result.Children.Add(missle);
            result.Children.Add(new GeometryDrawing(CannonBeltBrush, BasicPen, GetPathGeometry("M11.6,9 a3,3 360 0,1 6.8,0 v5 a3,3 360 0,0 -6.8,0 z")));
            return result;
        }
        static Drawing GetGaussDraw()
        {
            DrawingGroup result = new DrawingGroup();
            Pen BeamPen = new Pen(GaussBeamBrush, 5);
            BeamPen.StartLineCap = PenLineCap.Round;
            BeamPen.EndLineCap = PenLineCap.Round;
            result.Children.Add(new GeometryDrawing(null, BeamPen, GetPathGeometry("M5,3 C15,15 -5,15 5,27")));
            result.Children.Add(new GeometryDrawing(null, BeamPen, GetPathGeometry("M25,3 C15,15 35,15 25,27")));
            result.Children.Add(new GeometryDrawing(GaussBulletBrush, BasicPen, GetPathGeometry("M15,5 l2,8 a20,13 360 0,0 1,6 v5 l-3,-4 l-3,4 v-5 a20,13 360 0,0 1,-6 z")));
            return result;
        }
        static Drawing GetCannonDraw()
        {
            DrawingGroup result = new DrawingGroup();
            Pen BeamPen = new Pen(GaussBeamBrush, 5);
            BeamPen.StartLineCap = PenLineCap.Round;
            BeamPen.EndLineCap = PenLineCap.Round;
            result.Children.Add(new GeometryDrawing(CannonBulletBrush, BasicPen, GetPathGeometry("M11,8 a3,3 360 0,1 8,0 v16 h-8z M0,0 v0.1 M30,30 v0.1")));
            result.Children.Add(new GeometryDrawing(CannonBeltBrush, BasicPen, GetPathGeometry("M11,14 a3,3 360 0,1 8,0 v5 a3,3 360 0,0 -8,0 z")));
            return result;
        }
        static Drawing GetLaserDraw()
        {
            DrawingGroup result = new DrawingGroup();
            RectangleGeometry beam = new RectangleGeometry(new Rect(11.5, 2.5, 7, 25), 10, 10);
            result.Children.Add(new GeometryDrawing(EMIBrush, null, beam));
            result.Children.Add(new GeometryDrawing(null, BasicPen, GetPathGeometry("M0,0 v0.1 M30,30 v0.1")));
            return result;
        }

        static Drawing GetEMIDraw()
        {
            DrawingGroup result = new DrawingGroup();
            Pen BeamPen = new Pen(EMIBrush, 5);
            BeamPen.StartLineCap = PenLineCap.Round;
            BeamPen.EndLineCap = PenLineCap.Round;
            result.Children.Add(new GeometryDrawing(null, BeamPen, GetPathGeometry("M8,3 C18,15 0,15 8,27")));
            result.Children.Add(new GeometryDrawing(null, BeamPen, GetPathGeometry("M22,3 C12,15 30,15 22,27")));
            return result;
        }
        static Drawing GetPlasmaDraw()
        {
            DrawingGroup result = new DrawingGroup();
            result.Children.Add(new GeometryDrawing(PlasmaBrush, null, GetPathGeometry("M6,7 A9.5,10 360 0,1 24,7 A15,20 360 0,1 15,27 A15,20 360 0,1 6,7")));
            result.Children.Add(new GeometryDrawing(null, BasicPen, GetPathGeometry("M0,0 v0.1 M30,30 v0.1")));
            return result;
        }

        static Drawing GetSolarDraw()
        {
            DrawingGroup result = new DrawingGroup();
            result.Children.Add(new GeometryDrawing(SolarBodyBrush, BasicPen, GetPathGeometry("M12,20 a5.4,5.4 360 1,0 6,0" +
                                      "a2,2 360 0,1 0,-2 l5,-8 l-6,-6 v2 l4,4 l-5,8 a2,2 360 0,0 0,2 a2,2 360 0,1 0,1.1" +
                                      "a3.5,3.5 360 1,1 -2,0" +
                                      "a2,2 360 0,1 0,-1.1 a2,2 360 0,0 0,-2 l-5,-8 l4,-4 v-2 l-6,6 l5,8 a2,2 360 0,1 0,2")));
            result.Children.Add(new GeometryDrawing(SolarBeamBrush, null, GetPathGeometry("M21,10 l-5,8 a2,2 360 0,0 0,2 a2,2 360 0,1 0,1.1" +
                                  "a3.5,3.5 360 0,0 -2,0 a2,2 360 0,1 0,-1.1 a2,2 360 0,0 0,-2 l-5,-8 l4,-4 v-6 h4 v6z")));
            result.Children.Add(new GeometryDrawing(SolarSunBrush, null, new EllipseGeometry(new Point(15, 24.5), 3.5, 3.5)));
            result.Children.Add(new GeometryDrawing(null, BasicPen, GetPathGeometry("M0,0 v0.1 M30,30 v0.1")));
            return result;
        }
        public static Canvas TwoSwordCanvas()
        {
            Canvas result = new Canvas();
            Path sword1 = new Path();
            sword1.Stroke = Brushes.Black; sword1.StrokeThickness = 0.2; sword1.StrokeLineJoin = PenLineJoin.Round;
            result.Children.Add(sword1); Canvas.SetZIndex(sword1, 1);
            sword1.Data = GetPathGeometry("M48,8 a2,5 0 0,1 4,0 a3,40 0 0,1 0,60 h10 a3,3 0 0,1 0,6 h-10" +
                                      "a1,2 0 0,1 0,4 a1,2 0 0,1 0,4 a1,2 0 0,1 0,4 a1,2 0 0,1 0,4 a4,4 0 1,1 -4,0" +
                                      "a1,2 0 0,1 0,-4 a1,2 0 0,1 0,-4 a1,2 0 0,1 0,-4 a1,2 0 0,1 0,-4" +
                                      "h-10 a3,3 0 0,1 0,-6 h10 a3,40 0 0,1 0,-60");
            sword1.Fill = SwordBrush;
            RotateTransform trans1 = new RotateTransform(40, 50, 50);
            sword1.RenderTransform = trans1;
            sword1.ToolTip = Links.Interface("FleetInBattle");
            Path sword2 = new Path();
            sword2.Stroke = Brushes.Black; sword2.StrokeThickness = 0.2; sword2.StrokeLineJoin = PenLineJoin.Round;
            result.Children.Add(sword2); Canvas.SetZIndex(sword2, 1);
            sword2.Data = GetPathGeometry("M48,8 a2,5 0 0,1 4,0 a3,40 0 0,1 0,60 h10 a3,3 0 0,1 0,6 h-10" +
                                      "a1,2 0 0,1 0,4 a1,2 0 0,1 0,4 a1,2 0 0,1 0,4 a1,2 0 0,1 0,4 a4,4 0 1,1 -4,0" +
                                      "a1,2 0 0,1 0,-4 a1,2 0 0,1 0,-4 a1,2 0 0,1 0,-4 a1,2 0 0,1 0,-4" +
                                      "h-10 a3,3 0 0,1 0,-6 h10 a3,40 0 0,1 0,-60");
            sword2.Fill = SwordBrush;
            RotateTransform trans2 = new RotateTransform(-40, 50, 50);
            sword2.RenderTransform = trans2;
            sword2.ToolTip = Links.Interface("FleetInBattle");
            return result;
        }
        public static Canvas OneSwordCanvas()
        {
            Canvas result = new Canvas();
            Path sword = new Path();
            sword.Stroke = Brushes.Black; sword.StrokeThickness = 0.2; sword.StrokeLineJoin = PenLineJoin.Round;
            result.Children.Add(sword); Canvas.SetZIndex(sword, 1);
            sword.Data = GetPathGeometry("M48,8 a2,5 0 0,1 4,0 a3,40 0 0,1 0,60 h10 a3,3 0 0,1 0,6 h-10" +
                                      "a1,2 0 0,1 0,4 a1,2 0 0,1 0,4 a1,2 0 0,1 0,4 a1,2 0 0,1 0,4 a4,4 0 1,1 -4,0" +
                                      "a1,2 0 0,1 0,-4 a1,2 0 0,1 0,-4 a1,2 0 0,1 0,-4 a1,2 0 0,1 0,-4" +
                                      "h-10 a3,3 0 0,1 0,-6 h10 a3,40 0 0,1 0,-60");
            sword.Fill = SwordBrush;
            sword.ToolTip = Links.Interface("FleetInScout");
            return result;
        }
        public static Canvas SwordShieldCanvas()
        {
            Canvas result = new Canvas();
            Path shield = new Path();
            shield.Stroke = Brushes.Black; shield.StrokeThickness = 0.2; shield.StrokeLineJoin = PenLineJoin.Round;
            result.Children.Add(shield); Canvas.SetZIndex(shield, 1);
            shield.Data = GetPathGeometry("M20,20 a40,30 0 0,1 60,0 a40,60 0 0,1 -30,80 a40,60 0 0,1 -30,-80");
            shield.Fill = Shieldbrush;
            shield.ToolTip = Links.Interface("FleetInDefense");
            Path sword = new Path();
            sword.Stroke = Brushes.Black; sword.StrokeThickness = 0.2; sword.StrokeLineJoin = PenLineJoin.Round;
            result.Children.Add(sword); Canvas.SetZIndex(sword, 1);
            sword.Data = GetPathGeometry("M48,8 a2,5 0 0,1 4,0 a3,40 0 0,1 0,60 h10 a3,3 0 0,1 0,6 h-10" +
                                      "a1,2 0 0,1 0,4 a1,2 0 0,1 0,4 a1,2 0 0,1 0,4 a1,2 0 0,1 0,4 a4,4 0 1,1 -4,0" +
                                      "a1,2 0 0,1 0,-4 a1,2 0 0,1 0,-4 a1,2 0 0,1 0,-4 a1,2 0 0,1 0,-4" +
                                      "h-10 a3,3 0 0,1 0,-6 h10 a3,40 0 0,1 0,-60");
            sword.Fill = SwordBrush;
            sword.ToolTip = Links.Interface("FleetInDefense");
            return result;
        }
        static RadialGradientBrush GetSwordBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.6, 0.4);
            brush.RadiusX = 0.6;
            brush.RadiusY = 0.7;
            brush.GradientStops.Add(new GradientStop(Colors.Red, 1.5));
            brush.GradientStops.Add(new GradientStop(Colors.Gray, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            return brush;
        }
        static RadialGradientBrush GetMagnetBeamBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.5, 1.5);
            brush.RadiusX = 3; brush.RadiusY = 0.6;
            brush.GradientStops.Add(new GradientStop(Colors.DarkGreen, 0.2));
            brush.GradientStops.Add(new GradientStop(Colors.Green, 0.3));
            brush.GradientStops.Add(new GradientStop(Colors.Green, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.LightGreen, 0.6));
            brush.GradientStops.Add(new GradientStop(Colors.LightGreen, 0.8));
            brush.GradientStops.Add(new GradientStop(Colors.White, 1));
            return brush;
        }
        static LinearGradientBrush GetMagnetBodyBrush()
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0, 0.5);
            brush.EndPoint = new Point(1, 0.5);
            brush.GradientStops.Add(new GradientStop(Colors.Red, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Blue, 0.5));
            return brush;
        }
        static RadialGradientBrush GetRadTopBrush()
        {
            RadialGradientBrush brush = GetRadialbrush(0.7, 0.1, Colors.Green, 1);
            brush.RadiusX = 1;
            brush.RadiusY = 1;
            return brush;
        }
        static RadialGradientBrush GetDroneBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.RadiusX = 0.53;
            brush.GradientStops.Add(new GradientStop(Colors.Green, 0));
            brush.GradientStops.Add(new GradientStop(Colors.DarkGreen, 0.2));
            brush.GradientStops.Add(new GradientStop(Colors.LightGreen, 0.2));
            brush.GradientStops.Add(new GradientStop(Colors.Gray, 0.3));
            brush.GradientStops.Add(new GradientStop(Colors.DarkGray, 0.55));
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.7));
            return brush;
        }
        static RadialGradientBrush GetTimeSandBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Purple, 1));
            brush.GradientStops.Add(new GradientStop(Colors.Violet, 0.5));
            return brush;
        }
        static RadialGradientBrush GetTimeVialBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.2);
            brush.RadiusX = 1.5; brush.RadiusY = 1.5;
            brush.GradientStops.Add(new GradientStop(Colors.SkyBlue, 1));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            return brush;
        }
        static RadialGradientBrush GetWarpBrush()
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.4));
            brush.GradientStops.Add(new GradientStop(Colors.Violet, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Purple, 0.6));
            brush.GradientStops.Add(new GradientStop(Colors.Black, 1));
            return brush;
        }
        static LinearGradientBrush GetLinearbrush(bool? IsHor, Color color, double grad1, double grad2)
        {
            LinearGradientBrush result = new LinearGradientBrush();
            if (IsHor == true)
            {
                result.StartPoint = new Point(0.5, 0);
                result.EndPoint = new Point(0.5, 1);
            }
            else if (IsHor == false)
            {
                result.StartPoint = new Point(0, 0.5);
                result.EndPoint = new Point(1, 0.5);
            }
            result.GradientStops.Add(new GradientStop(color, grad1));
            result.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            result.GradientStops.Add(new GradientStop(color, grad2));
            return result;
        }
        static RadialGradientBrush GetRadialbrush(double gradX, double gradY, Color color, double offset)
        {
            RadialGradientBrush result = new RadialGradientBrush();
            result.GradientOrigin = new Point(gradX, gradY);
            result.GradientStops.Add(new GradientStop(color, offset));
            result.GradientStops.Add(new GradientStop(Colors.White, 0));
            return result;
        }
        static PathGeometry GetPathGeometry(string path)
        {
            return new PathGeometry((PathFigureCollection)conv.ConvertFrom(path));
        }
        static Pen GetBasicPen()
        {
            Pen result = new Pen(Brushes.Black, 0.2);
            result.LineJoin = PenLineJoin.Round;
            return result;
        }

        public static DrawingBrush GetLoadWindowBrush()
        {
            PathGeometry geom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M100,0 A27,27 0 0,0 100,50 A66,66 0 0,1 200,100" +
                              "A27,27 0 0,0 150,100 A66,66 0 0,1 100,200" +
                              "A27,27 0 0, 0 100,150 A66,66 0 0, 1 0,100" +
                              "A27,27 0 0, 0 50,100 A66,66 0 0,1 100,0"));
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.3));
            brush.GradientStops.Add(new GradientStop(Colors.Violet, 0.4));
            brush.GradientStops.Add(new GradientStop(Colors.Purple, 0.7));
            brush.GradientStops.Add(new GradientStop(Colors.Black, 1));
            return new DrawingBrush(new GeometryDrawing(brush, new Pen(Brushes.Black, 1), geom));
        }
    }
}
