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

namespace Client
{

    partial class HexCanvas
    {
        static Pen TextPen = GetTextPen();
        static LinearGradientBrush SelectBrush = Common.GetLinearBrush(Colors.Green, Colors.White, Colors.Green);
        static RadialGradientBrush Metal = Common.GetRadialBrush(Colors.Gray, 0.8, 0.3);
        static RadialGradientBrush BeltBrush = Common.GetRadialBrush(Colors.Blue, 0.7, 0.2);
        public static DrawingBrush ShipEnterBrush = GetShipEnterBrush();
        public static DrawingBrush ShipEnterSelectBrush = GetShipEnterSelectBrush();
        public static DrawingBrush ShipMoveBrush = GetShipMoveBrush();
        public static DrawingBrush ShipMoveSelectBrush = GetShipMoveSelectBrush();
        public static DrawingBrush ShipLeaveBrush = GetShipLeaveBrush();
        public static DrawingBrush ShipLeaveSelectBrush = GetShipLeaveSelectBrush();
        public static DrawingBrush ShipGun1Brush = GetShipShootBrush(0);
        public static DrawingBrush ShipGun1SelectBrush = GetShipShootSelectBrush(0);
        public static DrawingBrush ShipGun2Brush = GetShipShootBrush(1);
        public static DrawingBrush ShipGun2SelectBrush = GetShipShootSelectBrush(1);
        public static DrawingBrush ShipGun3Brush = GetShipShootBrush(2);
        public static DrawingBrush ShipGun3SelectBrush = GetShipShootSelectBrush(2);
        public static DrawingBrush ShipAllGunBrush = GetShipShootBrush(3);
        public static DrawingBrush ShipAllGunSelectBrush = GetShipShootSelectBrush(3);
        public static DrawingBrush TargetGunBrush = GetTargetGunBrush();
        public static DrawingBrush TargetGunSelectBrush = GetTargetGunSelectBrush();
        public static DrawingBrush ShipInfoBrush = GetInfoBrush();
        public static DrawingBrush ShipInfoSelectBrush = GetInfoSelectBrush();
        public static Pen GetTextPen()
        {
            Pen pen = new Pen();
            pen.Brush = Brushes.Black;
            pen.Thickness = 10;
            pen.LineJoin = PenLineJoin.Round;
            return pen;
        }
        public static DrawingBrush GetInfoBrush()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.Transparent, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-150,0 h340 v150 h-340z"))));
            group.Children.Add(new GeometryDrawing(null, TextPen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-120,30 h50 M-95,30 v90 M-120,120 h50"))));

            PathGeometry Healthgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,40 a10,10 0 0,1 30,0 a10,10 0 0,1 30,0 a40,40 0 0,1 -30,40 a40,40 0 0,1 -30,-40"));
            GeometryDrawing health = new GeometryDrawing(Common.GetRadialBrush(Colors.Red, 0.7, 0.3), null, Healthgeom);
            group.Children.Add(health);

            PathGeometry Shieldgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M80,30 a28,16 0 0,1 50,0 a40,40 0 0,1 -25,50 a40,40 0 0,1 -25,-50"));
            GeometryDrawing shield = new GeometryDrawing(Common.GetRadialBrush(Colors.Blue, 0.7, 0.3), null, Shieldgeom);
            group.Children.Add(shield);

            PathGeometry Energygeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M60,90 h20 l-14,22 h14 l-32,32 l14,-26 h-10z" +
                " M58,94 a30,30 0 0,0 -10,48 a2,2 0 1,1 -5,0 a30,32 0 0,1 14,-52 a2,2 0 0,1 0,4.5 M86,94 a30,30 0 0,1 -24,48 a2,2 0 0,0 2,4 a30,32 0 0,0 23,-56 a2,2 0 0,0 -1,4"));
            GeometryDrawing energy = new GeometryDrawing(Common.GetLinearBrush(Colors.Green,Colors.Yellow,Colors.Green), null, Energygeom);
            group.Children.Add(energy);

            return new DrawingBrush(group);
        }
        public static DrawingBrush GetInfoSelectBrush()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(SelectBrush, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-150,0 h340 v150 h-340z"))));
            group.Children.Add(new GeometryDrawing(null, TextPen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-120,30 h50 M-95,30 v90 M-120,120 h50"))));

            PathGeometry Healthgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,40 a10,10 0 0,1 30,0 a10,10 0 0,1 30,0 a40,40 0 0,1 -30,40 a40,40 0 0,1 -30,-40"));
            GeometryDrawing health = new GeometryDrawing(Common.GetRadialBrush(Colors.Red, 0.7, 0.3), null, Healthgeom);
            group.Children.Add(health);

            PathGeometry Shieldgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M80,30 a28,16 0 0,1 50,0 a40,40 0 0,1 -25,50 a40,40 0 0,1 -25,-50"));
            GeometryDrawing shield = new GeometryDrawing(Common.GetRadialBrush(Colors.Blue, 0.7, 0.3), null, Shieldgeom);
            group.Children.Add(shield);

            PathGeometry Energygeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M60,90 h20 l-14,22 h14 l-32,32 l14,-26 h-10z" +
                " M58,94 a30,30 0 0,0 -10,48 a2,2 0 1,1 -5,0 a30,32 0 0,1 14,-52 a2,2 0 0,1 0,4.5 M86,94 a30,30 0 0,1 -24,48 a2,2 0 0,0 2,4 a30,32 0 0,0 23,-56 a2,2 0 0,0 -1,4"));
            GeometryDrawing energy = new GeometryDrawing(Common.GetLinearBrush(Colors.Green, Colors.Yellow, Colors.Green), null, Energygeom);
            group.Children.Add(energy);

            return new DrawingBrush(group);
        }
        public static DrawingBrush GetShipRotateBrush()
        {
            PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            Pen pen = new Pen(Brushes.Black, 1);
            PathGeometry Arrowgeom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M140,100 a30,35 0 0,0 0,-60 l-10,8 l5,-40 l35,15 l-10,8 a35,40 0 0,1 0,70z"));
            RadialGradientBrush arrowbrush = new RadialGradientBrush();
            arrowbrush.GradientOrigin = new Point(0.8, 0.3);
            arrowbrush.GradientStops.Add(new GradientStop(Colors.White, 0));
            arrowbrush.GradientStops.Add(new GradientStop(Colors.Green, 1));
            GeometryDrawing arrow = new GeometryDrawing(arrowbrush, pen, Arrowgeom);
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(arrow);
            PathGeometry Bodygeom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M0,50 a50,50 0 1,1 0,0.1"));
            GeometryDrawing body = new GeometryDrawing(Metal, pen, Bodygeom);
            group.Children.Add(body);
            PathGeometry Beltgeom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M45,10 a40,40 0 0,1 0,80 a40,44 0 0,0 0,-80"));
            GeometryDrawing belt = new GeometryDrawing(BeltBrush, pen, Beltgeom);
            group.Children.Add(belt);
            PathGeometry leftgeom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M130,15 h-44.5 a40,40 0 0,1 8,10 h36.5z"));
            GeometryDrawing left = new GeometryDrawing(Metal, pen, leftgeom);
            group.Children.Add(left);
            PathGeometry rightgeom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M130,85 h-44.5 a40,40 0 0,0 8,-10 h36.5z"));
            GeometryDrawing right = new GeometryDrawing(Metal, pen, rightgeom);
            group.Children.Add(right);
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetShipMoveBrush()
        {
            Pen pen = new Pen(Brushes.Black, 1);
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.Transparent, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-150,0 h340 v150 h-340z"))));
            group.Children.Add(new GeometryDrawing(null, TextPen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-120,120 v-90 l30,45 l30,-45 v90"))));
            PathGeometry Arrowgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M140,55 h30 v-5 l30,20 l-30,20 v-5 h-30z"));//
            RadialGradientBrush arrowbrush = Common.GetRadialBrush(Colors.Green, 0.8, 0.3);
            GeometryDrawing arrow = new GeometryDrawing(arrowbrush, pen, Arrowgeom);
            
            group.Children.Add(arrow);
            PathGeometry Bodygeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,70 a50,50 0 1,1 0,0.1"));
            GeometryDrawing body = new GeometryDrawing(Metal, pen, Bodygeom);
            group.Children.Add(body);
            PathGeometry Beltgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M45,30 a40,40 0 0,1 0,80 a40,44 0 0,0 0,-80"));
            GeometryDrawing belt = new GeometryDrawing(BeltBrush, pen, Beltgeom);
            group.Children.Add(belt);
            PathGeometry leftgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M130,35 h-44.5 a40,40 0 0,1 8,10 h36.5z"));
            GeometryDrawing left = new GeometryDrawing(Metal, pen, leftgeom);
            group.Children.Add(left);
            PathGeometry rightgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M130,105 h-44.5 a40,40 0 0,0 8,-10 h36.5z"));
            GeometryDrawing right = new GeometryDrawing(Metal, pen, rightgeom);
            group.Children.Add(right);
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetShipMoveSelectBrush()
        {
            Pen pen = new Pen(Brushes.Black, 1);
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(SelectBrush, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-150,0 h340 v150 h-340z"))));
            group.Children.Add(new GeometryDrawing(null, TextPen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-120,120 v-90 l30,45 l30,-45 v90"))));
            PathGeometry Arrowgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M140,55 h30 v-5 l30,20 l-30,20 v-5 h-30z"));//
            RadialGradientBrush arrowbrush = Common.GetRadialBrush(Colors.Green, 0.8, 0.3);
            GeometryDrawing arrow = new GeometryDrawing(arrowbrush, pen, Arrowgeom);

            group.Children.Add(arrow);
            PathGeometry Bodygeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,70 a50,50 0 1,1 0,0.1"));
            GeometryDrawing body = new GeometryDrawing(Metal, pen, Bodygeom);
            group.Children.Add(body);
            PathGeometry Beltgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M45,30 a40,40 0 0,1 0,80 a40,44 0 0,0 0,-80"));
            GeometryDrawing belt = new GeometryDrawing(BeltBrush, pen, Beltgeom);
            group.Children.Add(belt);
            PathGeometry leftgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M130,35 h-44.5 a40,40 0 0,1 8,10 h36.5z"));
            GeometryDrawing left = new GeometryDrawing(Metal, pen, leftgeom);
            group.Children.Add(left);
            PathGeometry rightgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M130,105 h-44.5 a40,40 0 0,0 8,-10 h36.5z"));
            GeometryDrawing right = new GeometryDrawing(Metal, pen, rightgeom);
            group.Children.Add(right);
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetShipEnterBrush()
        {
            PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.Transparent, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-150,0 h340 v150 h-340z"))));
            group.Children.Add(new GeometryDrawing(null, TextPen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-120,30 v90 h50 M-120,30 h50 M-120,75 h30"))));
            PathGeometry Warpgeom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M25,0 a25,75 0 1,0 0.1,0"));
            RadialGradientBrush warpbrush = new RadialGradientBrush();
            warpbrush.GradientStops.Add(new GradientStop(Colors.Gray, 0));
            warpbrush.GradientStops.Add(new GradientStop(Colors.Black, 0.4));
            warpbrush.GradientStops.Add(new GradientStop(Colors.Violet, 0.6));
            warpbrush.GradientStops.Add(new GradientStop(Colors.Purple, 0.8));
            warpbrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 1));
            GeometryDrawing warp = new GeometryDrawing(warpbrush, new Pen(), Warpgeom);
            
            group.Children.Add(warp);
            Pen pen = new Pen(Brushes.Black, 1);
            PathGeometry Bodygeom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M25,45 a50,50 0 1,1 0,60z"));
            GeometryDrawing body = new GeometryDrawing(Metal, pen, Bodygeom);
            group.Children.Add(body);
            PathGeometry Beltgeom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M65,35 a40,40 0 0,1 0,80 a40,44 0 0,0 0,-80"));
            GeometryDrawing belt = new GeometryDrawing(BeltBrush, pen, Beltgeom);
            group.Children.Add(belt);
            PathGeometry leftgeom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M145,40 h-44.5 a40,40 0 0,1 8,10 h36.5z"));
            GeometryDrawing left = new GeometryDrawing(Metal, pen, leftgeom);
            group.Children.Add(left);
            PathGeometry rightgeom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M145,110 h-44.5 a40,40 0 0,0 8,-10 h36.5z"));
            GeometryDrawing right = new GeometryDrawing(Metal, pen, rightgeom);
            group.Children.Add(right);
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetShipEnterSelectBrush()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(SelectBrush, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-150,0 h340 v150 h-340z"))));
            group.Children.Add(new GeometryDrawing(null, TextPen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-120,30 v90 h50 M-120,30 h50 M-120,75 h30"))));
            PathGeometry Warpgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,0 a25,75 0 1,0 0.1,0"));
            RadialGradientBrush warpbrush = new RadialGradientBrush();
            warpbrush.GradientStops.Add(new GradientStop(Colors.Gray, 0));
            warpbrush.GradientStops.Add(new GradientStop(Colors.Black, 0.4));
            warpbrush.GradientStops.Add(new GradientStop(Colors.Violet, 0.6));
            warpbrush.GradientStops.Add(new GradientStop(Colors.Purple, 0.8));
            warpbrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 1));
            GeometryDrawing warp = new GeometryDrawing(warpbrush, new Pen(), Warpgeom);
            
            
            group.Children.Add(warp);
            Pen pen = new Pen(Brushes.Black, 1);
            PathGeometry Bodygeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,45 a50,50 0 1,1 0,60z"));
            GeometryDrawing body = new GeometryDrawing(Metal, pen, Bodygeom);
            group.Children.Add(body);
            PathGeometry Beltgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M65,35 a40,40 0 0,1 0,80 a40,44 0 0,0 0,-80"));
            GeometryDrawing belt = new GeometryDrawing(BeltBrush, pen, Beltgeom);
            group.Children.Add(belt);
            PathGeometry leftgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M145,40 h-44.5 a40,40 0 0,1 8,10 h36.5z"));
            GeometryDrawing left = new GeometryDrawing(Metal, pen, leftgeom);
            group.Children.Add(left);
            PathGeometry rightgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M145,110 h-44.5 a40,40 0 0,0 8,-10 h36.5z"));
            GeometryDrawing right = new GeometryDrawing(Metal, pen, rightgeom);
            group.Children.Add(right);
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetCancelBrush()
        {
            PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            PathGeometry Geom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M0,15 a10,10 0 0,1 20,-8 l23,23 a8,8 0 0,0 13,0 l23,-23 a10,10 0 0,1 20,8" +
              "l-30,30 a10,14 0 0,0 0,12 l30,30 a10,10 0 0,1 -20,8 l-23,-23 a8,8 0 0 0 -13,0 l-23,23" +
              "a10,10 0 0,1 -20,-8 l30,-30 a10,14 0 0,0 0,-12z"));
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.22);
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.05));
            brush.GradientStops.Add(new GradientStop(Colors.Pink, 0.1));
            brush.GradientStops.Add(new GradientStop(Colors.Red, 1));
            GeometryDrawing draw = new GeometryDrawing(brush, new Pen(), Geom);
            return new DrawingBrush(draw);
        }
        public static DrawingBrush GetTargetGunBrush()
        {
            Pen pen = new Pen(Brushes.Black, 1);
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.Transparent, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-150,0 h340 v150 h-340z"))));
            group.Children.Add(new GeometryDrawing(null, TextPen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-50,30 h-50 l-10,50 a30,30 0 1,1 0,30"))));
            PathGeometry Bodygeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,70 a50,50 0 1,1 0,0.1"));
            GeometryDrawing body = new GeometryDrawing(Metal, pen, Bodygeom);
            group.Children.Add(body);
            PathGeometry Beltgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M45,30 a40,40 0 0,1 0,80 a40,44 0 0,0 0,-80"));
            GeometryDrawing belt = new GeometryDrawing(BeltBrush, pen, Beltgeom);
            group.Children.Add(belt);
            PathGeometry leftgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M130,35 h-44.5 a40,40 0 0,1 8,10 h36.5z"));
            GeometryDrawing left = new GeometryDrawing(Metal, pen, leftgeom);
            group.Children.Add(left);
            PathGeometry middlegeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M140,65 h-44,5 a40,40 0 0,1 0,10 h44.5z"));
            GeometryDrawing middle = new GeometryDrawing(Metal, pen, middlegeom);
            group.Children.Add(middle);
            PathGeometry rightgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M130,105 h-44.5 a40,40 0 0,0 8,-10 h36.5z"));
            GeometryDrawing right = new GeometryDrawing(Metal, pen, rightgeom);
            group.Children.Add(right);
            PathGeometry aimgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M50,0 a70,70 0 1,1 -0.1,0 M-20,70 h50 M120,70 h-50 M50,0 v50 M50,140 v-50"));
            GeometryDrawing aim = new GeometryDrawing(null, new Pen(Brushes.Red, 10), aimgeom);
            group.Children.Add(aim);
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetTargetGunSelectBrush()
        {
            Pen pen = new Pen(Brushes.Black, 1);
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(SelectBrush, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-150,0 h340 v150 h-340z"))));
            group.Children.Add(new GeometryDrawing(null, TextPen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-50,30 h-50 l-10,50 a30,30 0 1,1 0,30"))));
            PathGeometry Bodygeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,70 a50,50 0 1,1 0,0.1"));
            GeometryDrawing body = new GeometryDrawing(Metal, pen, Bodygeom);
            group.Children.Add(body);
            PathGeometry Beltgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M45,30 a40,40 0 0,1 0,80 a40,44 0 0,0 0,-80"));
            GeometryDrawing belt = new GeometryDrawing(BeltBrush, pen, Beltgeom);
            group.Children.Add(belt);
            PathGeometry leftgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M130,35 h-44.5 a40,40 0 0,1 8,10 h36.5z"));
            GeometryDrawing left = new GeometryDrawing(Metal, pen, leftgeom);
            group.Children.Add(left);
            PathGeometry middlegeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M140,65 h-44,5 a40,40 0 0,1 0,10 h44.5z"));
            GeometryDrawing middle = new GeometryDrawing(Metal, pen, middlegeom);
            group.Children.Add(middle);
            PathGeometry rightgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M130,105 h-44.5 a40,40 0 0,0 8,-10 h36.5z"));
            GeometryDrawing right = new GeometryDrawing(Metal, pen, rightgeom);
            group.Children.Add(right);
            PathGeometry aimgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M50,0 a70,70 0 1,1 -0.1,0 M-20,70 h50 M120,70 h-50 M50,0 v50 M50,140 v-50"));
            GeometryDrawing aim = new GeometryDrawing(null, new Pen(Brushes.Red, 10), aimgeom);
            group.Children.Add(aim);
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetShipShootBrush(int pos)
        {
            Pen pen = new Pen(Brushes.Black, 1);
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.Transparent, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-150,0 h340 v150 h-340z"))));
            switch (pos)
            {
                case 0:
                    group.Children.Add(new GeometryDrawing(null, TextPen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-110,75 a50,50 0 0,0 25,-45 l-0,90 M-110,120 h50"))));
                    break;
                case 1:
                    group.Children.Add(new GeometryDrawing(null, TextPen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-110,55 a27,27 0 1,1 50,15 l-50,50 h55"))));
                    break;
                case 2:
                    group.Children.Add(new GeometryDrawing(null, TextPen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-110,30 h50 l-30,40 a27,27 0 1,1 -20,25"))));
                    break;
                case 3:
                    group.Children.Add(new GeometryDrawing(null, TextPen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-90,30 l-30,60 h45 M-70,60 v70"))));
                    break;
            }
            //group.Children.Add(WeapCanvas.GetDrawing(ship.Weapons[pos].Type));
            PathGeometry Bodygeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,70 a50,50 0 1,1 0,0.1"));
            GeometryDrawing body = new GeometryDrawing(Metal, pen, Bodygeom);
            group.Children.Add(body);
            PathGeometry Beltgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M45,30 a40,40 0 0,1 0,80 a40,44 0 0,0 0,-80"));
            GeometryDrawing belt = new GeometryDrawing(BeltBrush, pen, Beltgeom);
            group.Children.Add(belt);
            PathGeometry leftgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M130,35 h-44.5 a40,40 0 0,1 8,10 h36.5z"));
            GeometryDrawing left = new GeometryDrawing((pos==0 || pos==3)?(Brush)Brushes.Red:Metal, pen, leftgeom);
            group.Children.Add(left);
            PathGeometry middlegeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M140,65 h-44,5 a40,40 0 0,1 0,10 h44.5z"));
            GeometryDrawing middle = new GeometryDrawing((pos==1|| pos==3)?(Brush)Brushes.Red:Metal, pen, middlegeom);
            group.Children.Add(middle);
            PathGeometry rightgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M130,105 h-44.5 a40,40 0 0,0 8,-10 h36.5z"));
            GeometryDrawing right = new GeometryDrawing((pos==2 || pos==3)?(Brush)Brushes.Red:Metal, pen, rightgeom);
            group.Children.Add(right);
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetShipShootSelectBrush(int pos)
        {
            Pen pen = new Pen(Brushes.Black, 1);
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(SelectBrush, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-150,0 h340 v150 h-340z"))));
            switch (pos)
            {
                case 0:
                    group.Children.Add(new GeometryDrawing(null, TextPen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-110,75 a50,50 0 0,0 25,-45 l-0,90 M-110,120 h50"))));
                    break;
                case 1:
                    group.Children.Add(new GeometryDrawing(null, TextPen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-110,55 a27,27 0 1,1 50,15 l-50,50 h55"))));
                    break;
                case 2:
                    group.Children.Add(new GeometryDrawing(null, TextPen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-110,30 h50 l-30,40 a27,27 0 1,1 -20,25"))));
                    break;
                case 3:
                    group.Children.Add(new GeometryDrawing(null, TextPen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-90,30 l-30,60 h45 M-70,60 v70"))));
                    break;
            }
            //group.Children.Add(WeapCanvas.GetDrawing(ship.Weapons[pos].Type));
            PathGeometry Bodygeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,70 a50,50 0 1,1 0,0.1"));
            GeometryDrawing body = new GeometryDrawing(Metal, pen, Bodygeom);
            group.Children.Add(body);
            PathGeometry Beltgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M45,30 a40,40 0 0,1 0,80 a40,44 0 0,0 0,-80"));
            GeometryDrawing belt = new GeometryDrawing(BeltBrush, pen, Beltgeom);
            group.Children.Add(belt);
            PathGeometry leftgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M130,35 h-44.5 a40,40 0 0,1 8,10 h36.5z"));
            GeometryDrawing left = new GeometryDrawing((pos == 0 || pos == 3) ? (Brush)Brushes.Red : Metal, pen, leftgeom);
            group.Children.Add(left);
            PathGeometry middlegeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M140,65 h-44,5 a40,40 0 0,1 0,10 h44.5z"));
            GeometryDrawing middle = new GeometryDrawing((pos == 1 || pos == 3) ? (Brush)Brushes.Red : Metal, pen, middlegeom);
            group.Children.Add(middle);
            PathGeometry rightgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M130,105 h-44.5 a40,40 0 0,0 8,-10 h36.5z"));
            GeometryDrawing right = new GeometryDrawing((pos == 2 || pos == 3) ? (Brush)Brushes.Red : Metal, pen, rightgeom);
            group.Children.Add(right);
            return new DrawingBrush(group);
        }
        /*
        public static DrawingBrush GetShipShootBrush(ShipB ship, int pos)
        {
            PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            Pen pen = new Pen(Brushes.Black, 1);
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(WeapCanvas.GetDrawing(ship.Weapons[pos].Type));
            RadialGradientBrush Metal = new RadialGradientBrush();
            Metal.GradientOrigin = new Point(0.8, 0.3);
            Metal.GradientStops.Add(new GradientStop(Colors.Gray, 1));
            Metal.GradientStops.Add(new GradientStop(Colors.White, 0));
            RadialGradientBrush BeltBrush = new RadialGradientBrush();
            BeltBrush.GradientOrigin = new Point(0.7, 0.2);
            BeltBrush.GradientStops.Add(new GradientStop(Colors.Blue, 1));
            BeltBrush.GradientStops.Add(new GradientStop(Colors.White, 0));
            PathGeometry Bodygeom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M-65,15 a25,25 0 1,1 0,0.1"));
            GeometryDrawing body = new GeometryDrawing(Metal, pen, Bodygeom);
            group.Children.Add(body);
            PathGeometry Beltgeom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M-40,-5 a20,20 0 0,1 0,40 a20,22 0 0,0 0,-40"));
            GeometryDrawing belt = new GeometryDrawing(BeltBrush, pen, Beltgeom);
            group.Children.Add(belt);
            if (ship.Weapons[0] != null)
            {
                PathGeometry leftgeom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M0,-2.5 h-22.25 a20,20 0 0,1 4,5 h18.25z"));
                GeometryDrawing left = new GeometryDrawing(pos == 0 ? (Brush)Brushes.Red : Metal, pen, leftgeom);
                group.Children.Add(left);
            }
            if (ship.Weapons[1] != null)
            {
                PathGeometry middlegeom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M0,12.5 v5 h-15.15 a20,20 0 0,0 0,-5z"));
                GeometryDrawing middle = new GeometryDrawing(pos == 1 ? (Brush)Brushes.Red : Metal, pen, middlegeom);
                group.Children.Add(middle);
            }
            if (ship.Weapons[2] != null)
            {
                PathGeometry rightgeom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M0,32.5 h-22.25 a20,20 0 0,0 4,-5 h18.25z"));
                GeometryDrawing right = new GeometryDrawing(pos == 2 ? (Brush)Brushes.Red : Metal, pen, rightgeom);
                group.Children.Add(right);
            }
            return new DrawingBrush(group);
        }
         */
        public static DrawingBrush GetShipLeaveBrush()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.Transparent, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-150,0 h340 v150 h-340z"))));
            group.Children.Add(new GeometryDrawing(null, TextPen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-120,30 v90 h50")))); 
            PathGeometry Warpgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M125,0 a25,75 0 1,0 0.1,0"));
            RadialGradientBrush warpbrush = new RadialGradientBrush();
            warpbrush.GradientStops.Add(new GradientStop(Colors.Gray, 0));
            warpbrush.GradientStops.Add(new GradientStop(Colors.Black, 0.4));
            warpbrush.GradientStops.Add(new GradientStop(Colors.Violet, 0.6));
            warpbrush.GradientStops.Add(new GradientStop(Colors.Purple, 0.8));
            warpbrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 1));
            GeometryDrawing warp = new GeometryDrawing(warpbrush, new Pen(), Warpgeom);
            group.Children.Add(warp);
            Pen pen = new Pen(Brushes.Black, 1);
            PathGeometry Bodygeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M15,75 a50,50 0 1,1 0,0.1"));
            GeometryDrawing body = new GeometryDrawing(Metal, pen, Bodygeom);
            group.Children.Add(body);
            PathGeometry Beltgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M65,35 a40,40 0 0,1 0,80 a40,44 0 0,0 0,-80"));
            GeometryDrawing belt = new GeometryDrawing(BeltBrush, pen, Beltgeom);
            group.Children.Add(belt);
            PathGeometry leftgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M125,40 h-24.5 a40,40 0 0,1 8,10 h16.5z"));
            GeometryDrawing left = new GeometryDrawing(Metal, pen, leftgeom);
            group.Children.Add(left);
            PathGeometry rightgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M125,110 h-24.5 a40,40 0 0,0 8,-10 h16.5z"));
            GeometryDrawing right = new GeometryDrawing(Metal, pen, rightgeom);
            group.Children.Add(right);
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetShipLeaveSelectBrush()
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(SelectBrush, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-150,0 h340 v150 h-340z"))));
            group.Children.Add(new GeometryDrawing(null, TextPen, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-120,30 v90 h50"))));
            PathGeometry Warpgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M125,0 a25,75 0 1,0 0.1,0"));
            RadialGradientBrush warpbrush = new RadialGradientBrush();
            warpbrush.GradientStops.Add(new GradientStop(Colors.Gray, 0));
            warpbrush.GradientStops.Add(new GradientStop(Colors.Black, 0.4));
            warpbrush.GradientStops.Add(new GradientStop(Colors.Violet, 0.6));
            warpbrush.GradientStops.Add(new GradientStop(Colors.Purple, 0.8));
            warpbrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 1));
            GeometryDrawing warp = new GeometryDrawing(warpbrush, new Pen(), Warpgeom);
            group.Children.Add(warp);
            Pen pen = new Pen(Brushes.Black, 1);
            PathGeometry Bodygeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M15,75 a50,50 0 1,1 0,0.1"));
            GeometryDrawing body = new GeometryDrawing(Metal, pen, Bodygeom);
            group.Children.Add(body);
            PathGeometry Beltgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M65,35 a40,40 0 0,1 0,80 a40,44 0 0,0 0,-80"));
            GeometryDrawing belt = new GeometryDrawing(BeltBrush, pen, Beltgeom);
            group.Children.Add(belt);
            PathGeometry leftgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M125,40 h-24.5 a40,40 0 0,1 8,10 h16.5z"));
            GeometryDrawing left = new GeometryDrawing(Metal, pen, leftgeom);
            group.Children.Add(left);
            PathGeometry rightgeom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M125,110 h-24.5 a40,40 0 0,0 8,-10 h16.5z"));
            GeometryDrawing right = new GeometryDrawing(Metal, pen, rightgeom);
            group.Children.Add(right);
            return new DrawingBrush(group);
        }
    }
   /* class PlasmaCritFlash
    {
        public static Brush PlasmaCritBrush = GetPlasmaCritBrush();
        Rectangle plasmacritrect;
        public PlasmaCritFlash(HexShip ship)
        {
            Point Center = ship.TranslatePoint(ship.CenterShip, HexCanvas.Inner);
            plasmacritrect = new Rectangle();
            Canvas.SetZIndex(plasmacritrect, Links.ZIndex.Flashes);
            plasmacritrect.Fill = PlasmaCritBrush;
            plasmacritrect.Width = 80; plasmacritrect.Height = 50;
            HexCanvas.Inner.Children.Add(plasmacritrect);
            Canvas.SetLeft(plasmacritrect, Center.X - 40);
            Canvas.SetTop(plasmacritrect, Center.Y - 25);
            plasmacritrect.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform scale = new ScaleTransform();
            plasmacritrect.RenderTransform = scale;
            DoubleAnimation scaleanim = new DoubleAnimation(1, 3, TimeSpan.FromSeconds(Links.ShootAnimSpeed / 2));
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleanim);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleanim);
            DoubleAnimation oppanim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(Links.ShootAnimSpeed));
            oppanim.Completed += new EventHandler(plasmacritoppanim_Completed);
            plasmacritrect.BeginAnimation(Ellipse.OpacityProperty, oppanim);
        }
        void plasmacritoppanim_Completed(object sender, EventArgs e)
        {
            HexCanvas.Inner.Children.Remove(plasmacritrect);
            plasmacritrect = null;
        }
        static Brush GetPlasmaCritBrush()
        {
            PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            PathGeometry geom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M2,8 a4,4 0 0,1 8,-3 l15,19 l15,-19 a4,4 0 0,1 8,3 l-18,22 l18,22 a4,4 0 0,1 -8,3 l-15,-19 l-15,19 a4,4 0 0,1 -8,-3 l18,-22z" +
              "M51,20 a4,4 0 0,0 8,0 a11,11 0 0,1 22,0 l-27,38 h33 v-6 h-20 l22,-32 a19,19 0 0,0 -38,0"));
            Pen pen = new Pen(Brushes.Black, 1);
            pen.LineJoin = PenLineJoin.Round;
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.4));
            brush.GradientStops.Add(new GradientStop(Colors.SkyBlue, 0.6));
            brush.GradientStops.Add(new GradientStop(Colors.Blue, 0.8));
            brush.GradientStops.Add(new GradientStop(Colors.Black, 1));
            SkewTransform skew = new SkewTransform(-15, 0);
            geom.Transform = skew;
            GeometryDrawing draw = new GeometryDrawing(brush, pen, geom);
            DrawingBrush result = new DrawingBrush(draw);
            result.TileMode = TileMode.None;
            return result;
        }
    }*/
    class GaussCritFlash
    {
        Path gausscrit;
        GradientStop red = new GradientStop(Colors.Red, -1);
        public GaussCritFlash(HexShip ship)
        {
            Point Center = ship.TranslatePoint(ship.CenterShip, HexCanvas.Inner);
            gausscrit = new Path();
            Canvas.SetZIndex(gausscrit, Links.ZIndex.Flashes);
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(1, 0.5);
            brush.GradientStops.Add(red);
            brush.GradientStops.Add(new GradientStop(Colors.Blue, 1));
            brush.RadiusX = 1; brush.RadiusY = 1;
            gausscrit.Fill = brush;
            gausscrit.Width = 200; gausscrit.Height = 220;
            gausscrit.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,40 a130,100 0 0,1 200,0 a120,170 0 0,1 -100,180 a120,170 0 0,1 -100,-180"));
            gausscrit.Stroke = Brushes.Black;
            HexCanvas.Inner.Children.Add(gausscrit);
            Canvas.SetLeft(gausscrit, Center.X - 100);
            Canvas.SetTop(gausscrit, Center.Y - 110);
            gausscrit.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform scale = new ScaleTransform();
            gausscrit.RenderTransform = scale;
            DoubleAnimation scaleanim = new DoubleAnimation(0.2, 1.5, TimeSpan.FromSeconds(Links.ShootAnimSpeed / 2));
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleanim);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleanim);
            DoubleAnimation oppanim = new DoubleAnimation(2, 0, TimeSpan.FromSeconds(Links.ShootAnimSpeed * 2.5));
            oppanim.Completed += new EventHandler(oppanim_Completed);
            gausscrit.BeginAnimation(Path.OpacityProperty, oppanim);
            DoubleAnimation offsetanim = new DoubleAnimation(-1, 0.9, TimeSpan.FromSeconds(Links.ShootAnimSpeed * 2.5));
            red.BeginAnimation(GradientStop.OffsetProperty, offsetanim);
        }

        void oppanim_Completed(object sender, EventArgs e)
        {
            HexCanvas.Inner.Children.Remove(gausscrit);
        }
    }
    class DroneCritFlash
    {
        Path gausscrit;
        GradientStop blue = new GradientStop(Colors.Blue, 1);
        public DroneCritFlash(HexShip ship)
        {
            Point Center = ship.TranslatePoint(ship.CenterShip, HexCanvas.Inner);
            gausscrit = new Path();
            Canvas.SetZIndex(gausscrit, Links.ZIndex.Flashes);
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.8, 0.3);
            brush.GradientStops.Add(blue);
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            gausscrit.Fill = brush;
            gausscrit.Width = 200; gausscrit.Height = 220;
            gausscrit.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,40 a130,100 0 0,1 200,0 a120,170 0 0,1 -100,180 a120,170 0 0,1 -100,-180"));
            gausscrit.Stroke = Brushes.Black;
            HexCanvas.Inner.Children.Add(gausscrit);
            Canvas.SetLeft(gausscrit, Center.X - 100);
            Canvas.SetTop(gausscrit, Center.Y - 110);
            gausscrit.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform scale = new ScaleTransform();
            gausscrit.RenderTransform = scale;
            DoubleAnimation scaleanim = new DoubleAnimation(0.2, 1.5, TimeSpan.FromSeconds(Links.ShootAnimSpeed / 2));
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleanim);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleanim);
            DoubleAnimation oppanim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(Links.ShootAnimSpeed * 1.5));
            oppanim.Completed += new EventHandler(oppanim_Completed);
            gausscrit.BeginAnimation(Path.OpacityProperty, oppanim);
            ColorAnimation coloranim = new ColorAnimation(Colors.Blue, Colors.Black, TimeSpan.FromSeconds(Links.ShootAnimSpeed));
            blue.BeginAnimation(GradientStop.ColorProperty, coloranim);
        }

        void oppanim_Completed(object sender, EventArgs e)
        {
            HexCanvas.Inner.Children.Remove(gausscrit);
        }
    }
    class MissleCritFlash
    {
        List<Canvas> list;
        static int[] dx = new int[] { 0, 225, 225, 0, -225, -225 };
        static int[] dy = new int[] { -260, -130, 130, 260, 130, -130 };
        static Brush SmallMissle = GetSmallMissleBrush();
        public MissleCritFlash(Hex hex)
        {
            Point pt = hex.CenterPoint;
            list = new List<Canvas>();
            DoubleAnimation anim = new DoubleAnimation(0, 260, TimeSpan.FromSeconds(Links.ShootAnimSpeed));
            anim.Completed += new EventHandler(anim_Completed);
            for (int i = 0; i < 6; i++)
            {
                Point pt1 = new Point(pt.X + dx[i], pt.Y + dy[i]);
                double ddy = pt.Y - pt1.Y;
                double ddx = pt1.X - pt.X;
                double tga = ddy / ddx;
                double Angle;
                if (ddx == 0) Angle = ddy > 0 ? 180 : 0;
                else if (ddx > 0)
                    Angle = 270 - Math.Atan(tga) * 180 / Math.PI;
                else
                    Angle = 90 - Math.Atan(tga) * 180 / Math.PI;
                Canvas c = new Canvas();
                Canvas.SetZIndex(c, Links.ZIndex.Flashes);
                list.Add(c);
                c.Width = 1; c.Height = 260;
                HexCanvas.Inner.Children.Add(c);
                Canvas.SetLeft(c, pt.X - c.Width / 2);
                Canvas.SetTop(c, pt.Y);
                c.RenderTransformOrigin = new Point(0.5, 0);
                RotateTransform trans = new RotateTransform(Angle);
                c.RenderTransform = trans;
                Rectangle missle = new Rectangle();
                missle.Width = 30; missle.Height = 35;
                missle.Fill = SmallMissle;
                c.Children.Add(missle);
                Canvas.SetLeft(missle, -15);
                RotateTransform mr = new RotateTransform(180);
                missle.RenderTransform = mr;
                missle.RenderTransformOrigin = new Point(0.5, 0.5);
                missle.BeginAnimation(Canvas.TopProperty, anim);
            }
        }

        void anim_Completed(object sender, EventArgs e)
        {
            foreach (Canvas c in list)
                HexCanvas.Inner.Children.Remove(c);
        }
        static Brush GetSmallMissleBrush()
        {
            PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            PathGeometry body = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M20,0 l5,20" +
              "a14,10 0 0,1 15,10 a12,8 0 0,0 -12,-5" +
              "a10,10 0 0,1 8,10 a25,20 0 0,0 -32,0 a10,10 0 0,1 8,-10" +
              "a12,18 0 0,0 -12,5 a14,10 0 0,1 15,-10z"));
            Pen pen = new Pen(Brushes.Black, 1);
            pen.LineJoin = PenLineJoin.Round;
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(0.6, 0.4);
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            brush.GradientStops.Add(new GradientStop(Colors.Gray, 1));
            GeometryDrawing bodydraw = new GeometryDrawing(brush, pen, body);
            PathGeometry belt = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M20,1 l4,17 a8,5 0 0,0 -8,0z"));
            RadialGradientBrush brush1 = new RadialGradientBrush();
            brush1.GradientOrigin = new Point(0.6, 0.4);
            brush1.GradientStops.Add(new GradientStop(Colors.White, 0));
            brush1.GradientStops.Add(new GradientStop(Colors.Red, 1));
            GeometryDrawing beltdraw = new GeometryDrawing(brush1, null, belt);
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(bodydraw);
            group.Children.Add(beltdraw);
            DrawingBrush result = new DrawingBrush(group);

            return result;
        }
    }
    class EMICritFlash
    {
        public static Brush EMICritBrush = GetEMICritBrush();
        Rectangle emicrit;
        public EMICritFlash(HexShip ship)
        {
            Point Center = ship.Hex.CenterPoint;
            emicrit = new Rectangle();
            Canvas.SetZIndex(emicrit, Links.ZIndex.Flashes);
            emicrit.Fill = EMICritBrush;
            emicrit.Width = 600; emicrit.Height = 600;
            HexCanvas.Inner.Children.Add(emicrit);
            Canvas.SetLeft(emicrit, Center.X - 300);
            Canvas.SetTop(emicrit, Center.Y - 300);

            LinearGradientBrush oppbrush = new LinearGradientBrush();
            GradientStop gr1 = new GradientStop(Colors.Transparent, -1);
            GradientStop gr2 = new GradientStop(Colors.Black, -0.5);
            GradientStop gr3 = new GradientStop(Colors.Transparent, 0);
            oppbrush.GradientStops.Add(gr1); oppbrush.GradientStops.Add(gr2); oppbrush.GradientStops.Add(gr3);
            emicrit.OpacityMask = oppbrush;
            DoubleAnimation anim1 = new DoubleAnimation(-1, 1, TimeSpan.FromSeconds(Links.ShootAnimSpeed * 2));
            DoubleAnimation anim2 = new DoubleAnimation(-0.5, 1.5, TimeSpan.FromSeconds(Links.ShootAnimSpeed * 2));
            DoubleAnimation anim3 = new DoubleAnimation(0, 2, TimeSpan.FromSeconds(Links.ShootAnimSpeed * 2));
            gr1.BeginAnimation(GradientStop.OffsetProperty, anim1);
            gr2.BeginAnimation(GradientStop.OffsetProperty, anim2);
            anim3.Completed += plasmacritoppanim_Completed;
            gr3.BeginAnimation(GradientStop.OffsetProperty, anim3);
        }
        void plasmacritoppanim_Completed(object sender, EventArgs e)
        {
            HexCanvas.Inner.Children.Remove(emicrit);

        }
        static Brush GetEMICritBrush()
        {
            PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            PathGeometry geom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M150,130 h6 v8 h8 v6 h-8 v8 h-6 v-8 h-8 v-6 h8z"));
            Pen pen = new Pen(Brushes.Black, 0.1);
            pen.LineJoin = PenLineJoin.Round;
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Green, 0.7));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Green, 0.3));
            GeometryDrawing draw = new GeometryDrawing(brush, pen, geom);
            DrawingBrush result = new DrawingBrush(draw);
            return result;
        }
    }
    class TimeCritFlash
    {
        public static Brush TimeCritBrush = GetTimeCritBrush();
        Rectangle timecrit;
        public TimeCritFlash(HexShip ship)
        {
            Point Center = ship.Hex.CenterPoint;
            timecrit = new Rectangle();
            Canvas.SetZIndex(timecrit, Links.ZIndex.Flashes);
            timecrit.Fill = TimeCritBrush;
            timecrit.Width = 600; timecrit.Height = 600;
            HexCanvas.Inner.Children.Add(timecrit);
            Canvas.SetLeft(timecrit, Center.X - 300);
            Canvas.SetTop(timecrit, Center.Y - 300);

            LinearGradientBrush oppbrush = new LinearGradientBrush();
            GradientStop gr1 = new GradientStop(Colors.Transparent, -1);
            GradientStop gr2 = new GradientStop(Colors.Black, -0.5);
            GradientStop gr3 = new GradientStop(Colors.Transparent, 0);
            oppbrush.GradientStops.Add(gr1); oppbrush.GradientStops.Add(gr2); oppbrush.GradientStops.Add(gr3);
            timecrit.OpacityMask = oppbrush;
            DoubleAnimation anim1 = new DoubleAnimation(-1, 1, TimeSpan.FromSeconds(Links.ShootAnimSpeed*2));
            DoubleAnimation anim2 = new DoubleAnimation(-0.5, 1.5, TimeSpan.FromSeconds(Links.ShootAnimSpeed*2));
            DoubleAnimation anim3 = new DoubleAnimation(0, 2, TimeSpan.FromSeconds(Links.ShootAnimSpeed*2));
            gr1.BeginAnimation(GradientStop.OffsetProperty, anim1);
            gr2.BeginAnimation(GradientStop.OffsetProperty, anim2);
            anim3.Completed += plasmacritoppanim_Completed;
            gr3.BeginAnimation(GradientStop.OffsetProperty, anim3);
        }
        void plasmacritoppanim_Completed(object sender, EventArgs e)
        {
            HexCanvas.Inner.Children.Remove(timecrit);

        }
        static Brush GetTimeCritBrush()
        {
            PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            PathGeometry geom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M150,130 h6 v8 h8 v6 h-8 v8 h-6 v-8 h-8 v-6 h8z"));
            Pen pen = new Pen(Brushes.Black, 0.1);
            pen.LineJoin = PenLineJoin.Round;
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Red, 0.7));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Red, 0.3));
            GeometryDrawing draw = new GeometryDrawing(brush, pen, geom);
            DrawingBrush result = new DrawingBrush(draw);
            return result;
        }
    }
    class AntiCritFlash
    {
        public static Brush AntiCritBrush = GetAntiCritBrush();
        Rectangle anticrit;
        public AntiCritFlash(HexShip ship, byte weapon)
        {
            Point Center;
            switch (weapon)
            {
                case 0: Center = ship.GetLeftGunPoint(); break;
                case 1: Center = ship.GetMiddleGunPoint(); break;
                default: Center = ship.GetRightGunPoint(); break;
            }
            anticrit = new Rectangle();
            Canvas.SetZIndex(anticrit, Links.ZIndex.Flashes);
            anticrit.Fill = AntiCritBrush;
            anticrit.Width = 50; anticrit.Height = 50;
            HexCanvas.Inner.Children.Add(anticrit);
            Canvas.SetLeft(anticrit, Center.X - 25);
            Canvas.SetTop(anticrit, Center.Y - 25);
            anticrit.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform scale = new ScaleTransform();
            anticrit.RenderTransform = scale;
            DoubleAnimation scaleanim = new DoubleAnimation(1, 5, TimeSpan.FromSeconds(Links.ShootAnimSpeed / 2));
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleanim);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleanim);
            DoubleAnimation oppanim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(Links.ShootAnimSpeed));
            oppanim.Completed += new EventHandler(plasmacritoppanim_Completed);
            anticrit.BeginAnimation(Ellipse.OpacityProperty, oppanim);
        }
        void plasmacritoppanim_Completed(object sender, EventArgs e)
        {
            HexCanvas.Inner.Children.Remove(anticrit);
            
        }
        static Brush GetAntiCritBrush()
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
    }
    class PsiCritFlash
    {
        public static Brush PsiCritBrush = GetPsiCritBrush();
        Rectangle psicrit;
        public PsiCritFlash(HexShip ship)
        {
            Point Center = ship.Hex.CenterPoint;
            psicrit = new Rectangle();
            Canvas.SetZIndex(psicrit, Links.ZIndex.Flashes);
            psicrit.Fill = PsiCritBrush;
            psicrit.Width = 50; psicrit.Height = 50;
            HexCanvas.Inner.Children.Add(psicrit);
            Canvas.SetLeft(psicrit, Center.X - 25);
            Canvas.SetTop(psicrit, Center.Y - 25);
            psicrit.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform scale = new ScaleTransform();
            DoubleAnimation scaleanim = new DoubleAnimation(1, 5, TimeSpan.FromSeconds(Links.ShootAnimSpeed));
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleanim);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleanim);
            RotateTransform rotate = new RotateTransform();
            DoubleAnimation rotateanim = new DoubleAnimation(0, 720, TimeSpan.FromSeconds(Links.ShootAnimSpeed*2));
            rotate.BeginAnimation(RotateTransform.AngleProperty, rotateanim);
            TransformGroup group = new TransformGroup();
            group.Children.Add(scale);
            group.Children.Add(rotate);
            psicrit.RenderTransform = group;
            DoubleAnimation oppanim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(Links.ShootAnimSpeed*2));
            oppanim.Completed += new EventHandler(plasmacritoppanim_Completed);
            psicrit.BeginAnimation(Ellipse.OpacityProperty, oppanim);
        }
        void plasmacritoppanim_Completed(object sender, EventArgs e)
        {
            HexCanvas.Inner.Children.Remove(psicrit);

        }
        static Brush GetPsiCritBrush()
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
    }
    class WarpCritFlash
    {
        static Brush brush = new DrawingBrush(WeapCanvas.GetDrawing(EWeaponType.Warp));
        Rectangle warp;
        HexShip Ship;
        public WarpCritFlash(HexShip ship)
        {
            warp = new Rectangle();
            Canvas.SetZIndex(warp, Links.ZIndex.WarpFlash);
            Ship = ship;
            warp.Width = 100; warp.Height = 100;
            warp.Fill = brush;
            HexCanvas.Inner.Children.Add(warp);
            Point Center = ship.Hex.CenterPoint;
            Canvas.SetLeft(warp, Center.X - warp.Width / 2);
            Canvas.SetTop(warp, Center.Y - warp.Height / 2);
            RotateTransform rotate = new RotateTransform();
            ScaleTransform scale = new ScaleTransform();
            TransformGroup group = new TransformGroup();
            group.Children.Add(scale);
            group.Children.Add(rotate);
            warp.RenderTransformOrigin = new Point(0.5, 0.5);
            warp.RenderTransform = group;
            DoubleAnimation rotateanim = new DoubleAnimation(0, 3600, TimeSpan.FromSeconds(Links.ShootAnimSpeed * 3.5));
            rotate.BeginAnimation(RotateTransform.AngleProperty, rotateanim);
            DoubleAnimation scaleanim = new DoubleAnimation(1, 5, TimeSpan.FromSeconds(Links.ShootAnimSpeed));
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleanim);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleanim);
            DoubleAnimation oppanim = new DoubleAnimation(2, 0, TimeSpan.FromSeconds(Links.ShootAnimSpeed * 3));
            oppanim.Completed += new EventHandler(oppanim_Completed);
            ship.BeginAnimation(Canvas.OpacityProperty, oppanim);
            BattleController.AddWorkingDelay(TimeSpan.FromSeconds(Links.ShootAnimSpeed * 3));
        }

        void oppanim_Completed(object sender, EventArgs e)
        {
            DoubleAnimation oppanim2 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(Links.ShootAnimSpeed / 2));
            oppanim2.Completed += new EventHandler(oppanim2_Completed);
            warp.BeginAnimation(Rectangle.OpacityProperty, oppanim2);
            DoubleAnimation clearanim = new DoubleAnimation(-1000, Links.ZeroTime);
            Ship.BeginAnimation(Canvas.LeftProperty, clearanim);
        }

        void oppanim2_Completed(object sender, EventArgs e)
        {
            HexCanvas.Inner.Children.Remove(warp);
            Canvas.SetZIndex(Ship, 1);
        }
    }
    class DarkCritFlash
    {
        public static Brush DarkCritBrush = new DrawingBrush(WeapCanvas.DarkDraw);
        Rectangle darkcrit;
        public DarkCritFlash(HexShip ship)
        {
            Point Center = ship.Hex.CenterPoint;
            darkcrit = new Rectangle();
            Canvas.SetZIndex(darkcrit, Links.ZIndex.Flashes);
            darkcrit.Fill = DarkCritBrush;
            darkcrit.Width = 50; darkcrit.Height = 50;
            HexCanvas.Inner.Children.Add(darkcrit);
            Canvas.SetLeft(darkcrit, Center.X - 25);
            Canvas.SetTop(darkcrit, Center.Y - 25);
            darkcrit.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform scale = new ScaleTransform();
            DoubleAnimation scaleanim = new DoubleAnimation(1, 5, TimeSpan.FromSeconds(Links.ShootAnimSpeed));
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleanim);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleanim);
            RotateTransform rotate = new RotateTransform();
            DoubleAnimation rotateanim = new DoubleAnimation(0, 720, TimeSpan.FromSeconds(Links.ShootAnimSpeed * 2));
            rotate.BeginAnimation(RotateTransform.AngleProperty, rotateanim);
            TransformGroup group = new TransformGroup();
            group.Children.Add(scale);
            group.Children.Add(rotate);
            darkcrit.RenderTransform = group;
            DoubleAnimation oppanim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(Links.ShootAnimSpeed * 2));
            oppanim.Completed += new EventHandler(plasmacritoppanim_Completed);
            darkcrit.BeginAnimation(Ellipse.OpacityProperty, oppanim);
        }
        void plasmacritoppanim_Completed(object sender, EventArgs e)
        {
            HexCanvas.Inner.Children.Remove(darkcrit);

        
        }
    }
      
    class SliceCritFlash
    {
        static LinearGradientBrush BlueIsolationBrush = GetIsolationBrush(Colors.Blue, Colors.White);
        static LinearGradientBrush RedIsolationBrush = GetIsolationBrush(Colors.Red, Colors.White);
        static LinearGradientBrush EarthIsolationBrush = GetIsolationBrush(Colors.Yellow, Colors.Green);
        static LinearGradientBrush CopperBrush = GetIsolationBrush(Colors.Orange, Colors.White);
        static Brush LinesBrush = GetBrush();
        static Brush SparkBrush = GetSparkBrush();
        Canvas result;
        public SliceCritFlash(HexShip ship)
        {
            result = new Canvas();
            HexCanvas.Inner.Children.Add(result);
            result.Width = 53; result.Height = 44;
            Canvas.SetZIndex(result, Links.ZIndex.Flashes);
            Canvas.SetLeft(result, ship.Hex.CenterPoint.X - 26.5);
            Canvas.SetTop(result, ship.Hex.CenterPoint.Y - 22);
            Rectangle Lines = new Rectangle();
            Lines.Width = 45; Lines.Height = 32;
            result.Children.Add(Lines);
            Canvas.SetTop(Lines, 6);
            Lines.Fill = LinesBrush;
            Rectangle Sparks = new Rectangle();
            Sparks.Width = 22; Sparks.Height = 44;
            result.Children.Add(Sparks);
            Canvas.SetLeft(Sparks, 32);
            Sparks.Fill = SparkBrush;
            ScaleTransform scale = new ScaleTransform();
            DoubleAnimation scaleanim = new DoubleAnimation(0, 3, TimeSpan.FromSeconds(Links.ShootAnimSpeed / 2));
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleanim);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleanim);
            result.RenderTransformOrigin = new Point(0.5, 0.5);
            result.RenderTransform = scale;
            DoubleAnimation oppanim = new DoubleAnimation(3, 0, TimeSpan.FromSeconds(Links.ShootAnimSpeed * 2));
            oppanim.Completed += new EventHandler(oppanim_Completed);
            result.BeginAnimation(Canvas.OpacityProperty, oppanim);
            DoubleAnimation sparkanim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(Links.ShootAnimSpeed * 1.5));
            ElasticEase el = new ElasticEase();
            el.Oscillations = 10;
            el.EasingMode = EasingMode.EaseIn;
            sparkanim.EasingFunction = el;
            Sparks.BeginAnimation(Rectangle.OpacityProperty, sparkanim);
        }

        void oppanim_Completed(object sender, EventArgs e)
        {
            HexCanvas.Inner.Children.Remove(result);
        }
        static LinearGradientBrush GetIsolationBrush(Color color1, Color color2)
        {
            LinearGradientBrush result = new LinearGradientBrush();
            result.StartPoint = new Point(0.5, 0);
            result.EndPoint=new Point(0.5,1);
            result.GradientStops.Add(new GradientStop(color1, 0.1));
            result.GradientStops.Add(new GradientStop(color2, 0.3));
            result.GradientStops.Add(new GradientStop(color1, 0.5));
            return result;
        }
        static Brush GetSparkBrush()
        {
            Pen pen = new Pen(Brushes.Black, 0.2);
            pen.LineJoin = PenLineJoin.Round;
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            PathGeometry spark1 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M35,-4 l6,5 l3,-7 l1,7 l8,-3 l-7,6 l7,2 l-8,2 l3,6 l-6,-5 l-5,6 l2,-8 l-7,2 l6,-5z"));
            PathGeometry spark2 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M35,7 l6,5 l3,-7 l1,7 l8,-3 l-7,6 l7,2 l-8,2 l3,6 l-6,-5 l-5,6 l2,-8 l-7,2 l6,-5z"));
            PathGeometry spark3 = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M35,18 l6,5 l3,-7 l1,7 l8,-3 l-7,6 l7,2 l-8,2 l3,6 l-6,-5 l-5,6 l2,-8 l-7,2 l6,-5 z"));
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(brush, pen, spark1));
            group.Children.Add(new GeometryDrawing(brush, pen, spark2));
            group.Children.Add(new GeometryDrawing(brush, pen, spark3));
            return new DrawingBrush(group);
        }
        static Brush GetBrush()
        {
            PathGeometry line1geom=new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h30 v10 h-30z"));
            PathGeometry line2geom=new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,11 h30 v10 h-30z"));
            PathGeometry line3geom=new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,22 h30 v10 h-30z"));
            PathGeometry cop1geom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M30,2 h10 v6 h-10z"));
            PathGeometry cop2geom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M30,13 h10 v6 h-10z"));
            PathGeometry cop3geom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M30,24 h10 v6 h-10z"));
            /*
            PathGeometry cop1geom=new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M30,2 h10 a3,3 0 0,1 0,6 h-10z"));
            PathGeometry cop2geom=new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M30,13 h10 a3,3 0 0,1 0,6 h-10z"));
            PathGeometry cop3geom=new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M30,24 h10 a3,3 0 0,1 0,6 h-10z"));
            */
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(BlueIsolationBrush, null, line1geom));
            group.Children.Add(new GeometryDrawing(RedIsolationBrush, null, line2geom));
            group.Children.Add(new GeometryDrawing(EarthIsolationBrush, null, line3geom));
            group.Children.Add(new GeometryDrawing(CopperBrush, null, cop1geom));
            group.Children.Add(new GeometryDrawing(CopperBrush, null, cop2geom));
            group.Children.Add(new GeometryDrawing(CopperBrush, null, cop3geom));
            return new DrawingBrush(group);
        }
    }
    class MagnetCritFlash
    {
        public static Brush MagnetCritBrush = GetMagnetCritBrush();
        Rectangle magnetcrit;
        public MagnetCritFlash(HexShip ship)
        {
            Point Center = ship.Hex.CenterPoint;
            magnetcrit = new Rectangle();
            Canvas.SetZIndex(magnetcrit, Links.ZIndex.Flashes);
            magnetcrit.Fill = MagnetCritBrush;
            magnetcrit.Width = 50; magnetcrit.Height = 50;
            HexCanvas.Inner.Children.Add(magnetcrit);
            Canvas.SetLeft(magnetcrit, Center.X - 25);
            Canvas.SetTop(magnetcrit, Center.Y - 25);
            magnetcrit.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform scale = new ScaleTransform();
            DoubleAnimation scaleanim = new DoubleAnimation(1, 5, TimeSpan.FromSeconds(Links.ShootAnimSpeed));
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleanim);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleanim);
            RotateTransform rotate = new RotateTransform();
            DoubleAnimation rotateanim = new DoubleAnimation(0, 720, TimeSpan.FromSeconds(Links.ShootAnimSpeed * 2));
            rotate.BeginAnimation(RotateTransform.AngleProperty, rotateanim);
            TransformGroup group = new TransformGroup();
            group.Children.Add(scale);
            group.Children.Add(rotate);
            magnetcrit.RenderTransform = group;
            DoubleAnimation oppanim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(Links.ShootAnimSpeed * 2));
            oppanim.Completed += new EventHandler(plasmacritoppanim_Completed);
            magnetcrit.BeginAnimation(Ellipse.OpacityProperty, oppanim);
        }
        void plasmacritoppanim_Completed(object sender, EventArgs e)
        {
            HexCanvas.Inner.Children.Remove(magnetcrit);

        }
        static Brush GetMagnetCritBrush()
        {
            PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
            PathGeometry geom = new PathGeometry((PathFigureCollection)conv.ConvertFrom("M0,0 h20 v30 a30,40 0 0,0 60,0 v-30 h20 v30 a35,45 0 0,1 -100,0z"));
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Point(0, 0.5);
            brush.EndPoint = new Point(1, 0.5);
            brush.GradientStops.Add(new GradientStop(Colors.Red, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Blue, 0.5));
            Pen pen = new Pen(Brushes.Black, 1.0);
            GeometryDrawing draw = new GeometryDrawing(brush, pen, geom);
            DrawingBrush result = new DrawingBrush(draw);
            return result;
        }
        
    }
}
