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
    delegate DrawingBrush SpecialImage (byte[] image);
    class ImageShipModel
    {
        public static List<ImageShipModel> Ships;
        public static List<ImageGunModel> Guns;
        public static List<ImageShipModel> BigShips;
        public static List<ImageGunModel> BigGuns;
        public static List<ImageShipModel> LargeBuildings;
        public Brush MainImage;
        public Size MainSize;
        public Point MainLocation;
        public bool LeftGunIsUpper;
        public bool MiddleGunIsUpper;
        public bool RightGunIsUpper;
        public PathGeometry Geometry;
        public Point LeftGunConnection;
        public Point MiddleGunConnection;
        public Point RightGunConnection;
        public Point HealthPoint;
        public Point ShieldPoint;
        public Point EnergyPoint;
        public bool IsSpecialImage;
        public SpecialImage GetSpecialImage;
        #region NewModels
        public static ImageShipModel GetModelNew(byte[] image)
        {
            ImageShipModel model = new ImageShipModel();
            switch (image[0])
            {
                case 1: model.MainImage = GetModelNew1Image(image);
                    model.MainSize = new Size(260, 166);
                    model.MainLocation = new Point(20, 53);
                    model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                        "M114,218 l7,-8 v-4 l16,-28 l3,-20" +
                        "a16,14 0 1,1 20,0 l3,20 l16,28 v4 l7,8 v-44 l-10,-40 a100,90 0 0,0 103,50 l-2,-10 a80,70 0 0,1 -57,-26" +
                        "h15 l10,7 h4 v-15 l-62,-67 l-16,-2 a21,14 0 0,0 -42,0 l-16,2 l-62,67 v15 h5 l10,-7 h14 a80,70 0 0,1 -59,26" +
                        "l-2,10 a100,90 0 0,0 103,-50 l-8,40z"));
                    model.LeftGunConnection = new Point(85, 120);
                    model.MiddleGunConnection = new Point(150, 90);
                    model.RightGunConnection = new Point(212, 120);
                    break;
                case 2: model.MainImage = GetModelNew2Image(image);
                    model.MainSize = new Size(240, 240);
                    model.MainLocation = new Point(35, 30);
                    model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                        "M120,84 a30,36 0 0,1 60,0 l-5,10 a30,30 0 0,1 4,35 a15,10 60 0,0 24,6 l-8,-28 l5,-8" +
                        "a5,10 0 0,1 -1,-15 v-22 a70,83 0 0,1 26,126 a20,20 0 0,1 -20,0 v15 l5,5 v18 l-6,3 v22" +
                        "l-6,3 l-8,-10 v-10 l-7,-8 v-10 l-7,-9 a40,30 0 0,1 -55,0 l-7,9 v10 l-6,10 v9 l-8,10 l-6,-3" +
                        "v-24 l-6,-3 v-16 l5,-5 v-15 a20,20 0 0,1 -18,0 a70,83 0 0,1 26,-126 v22 a5,10 0 0,1 -1,15" +
                        " l5,8 l-8,28 a15,10 -60 0,0 24,-6 a30,30 0 0,1 4,-35z"));
                    model.LeftGunConnection = new Point(88, 93);
                    model.MiddleGunConnection = new Point(150, 63);
                    model.RightGunConnection = new Point(212, 93);
                    break;
                case 3: model.MainImage = GetModelNew3Image(image);
                    model.MainSize = new Size(200, 150);
                    model.MainLocation = new Point(50, 67);
                    model.LeftGunIsUpper = true;
                    model.RightGunIsUpper = true;
                    model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                        "M109,68 l12,22 l-7,10 l7,7 v-4 l15,-23 h28 " +
                        "l15,23 v4 l7,-7 l-7,-10 l12,-22 h12 a70,70 0 0,1 38,60 l-20,7 l-4,-5 h-6 l-3,-4 l-10,5 l3,8 l-4,3 l12,4" +
                        " l3,-4 h7 l2,-4 l18,6 v5 a100,85 0 0,1 5,69 l-18,-10 v-15 l-11,14 l-10,-13 l-13,12 l-30,-28 l-3,-8 l4,-4" +
                        " l-5,-23 h-15 l-5,23 l4,4 l-3,8 l-30,28 l-13,-12 l-10,13 l-11,-14 v15 l-18,10 a100,85 0 0,1 5,-69 v-5 " +
                        "  l18,-6 l2,4 h7 l3,4 l12,-4 l-4,-3 l3,-8 l-10,-5 l-3,4 h-6 l-4,5 l-20,-7 a70,70 0 0,1 38,-60 z"));
                    model.LeftGunConnection = new Point(90, 100);
                    model.MiddleGunConnection = new Point(150, 110);
                    model.RightGunConnection = new Point(212, 100);
                    break;
                case 4: model.MainImage = GetModelNew4Image(image);
                    model.MainSize = new Size(250, 250);
                    model.MainLocation = new Point(25, 20);
                    model.MiddleGunIsUpper = true;
                    model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                        "M163,28 l5,5 l5,20 l60,52 v25 l16,60" +
                        " l-12,15 l-12,-3 l-32,16 l5,12 l-10,20 a10,10 0 0,0 -13,8 l-8,1 a15,15 0 0,1 -12,-13 h-5" +
                        " a15,15 0 0,1 -12,13 l-8,-1 a10,10 0 0,0 -13,-8 l-10,-20 l5,-12 l-32,-16 l-12,3 l-12,-15" +
                        " l16,-60 v-25 l60,-52 l4,-20 l5,-5 v64 l-13,11 a50,50 0 0,0 -30,66 l15,7 v6 l7,-5 a20,40 0 0,1 13,-37" +
                        "  a20,14 0 0,1 38,0 a20,40 0 0,1 13,37 l7,5 v-6 l15,-7 a50,50 0 0,0 -30,-66 l-13,-11z"));
                    model.LeftGunConnection = new Point(75, 150);
                    model.MiddleGunConnection = new Point(152, 210);
                    model.RightGunConnection = new Point(230, 150);
                    break;
                case 5:
                    model.MainImage = GetModelNew5Image(image);
                    model.MainSize = new Size(250, 250);
                    model.MainLocation = new Point(25, 20);
                    model.MiddleGunIsUpper = true;
                    model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                        "M164,30 l5,1 l5,35 l10,8 h6" +
                        "l7,10 a80,110 0 0,1 32,62 l5,2 l7,38 a30,30 0 0,1 -15,28 l-5,-5 a10,10 0 0,0 -12,-7 l-20,12" +
                        "a3,3 0 1,0 5,7 a20,20 0 0,1 -8,20 h-5 v6 l-8,9 l-4,-3 h-10 l-7,-9 a3.5,3.5 0 1,0 -6,0 l-7,9" +
                        "h-10 l-4,3 l-7,-9 v-6 h-5 a20,20 0 0,1 -8,-20 a3,3 0 1,0 5,-7 l-20,-12 a10,10 0 0,0 -12,7" +
                        "l-5,5 a30,30 0 0,1 -15,-28 l7,-38 l5,-2 a80,110 0 0,1 32,-62 l7,-10 h6 l10,-8 l5,-35 l5,-1 " +
                        "v62 l4,6 a70,120 0 0,0 -34,80 a10,13 0 0,0 18,8 a30,30 0 0,1 6,-27 a30,50 0 0,1 41,0"+
                        "a30,30 0 0,1 6,27 a10,13 0 0,0 18,-8 a70,120 0 0,0 -34,-80 l4,-6z"));
                    model.LeftGunConnection = new Point(90, 130);
                    model.MiddleGunConnection = new Point(149, 210);
                    model.RightGunConnection = new Point(209, 130);
                    break;
                case 6:
                    model.MainImage = GetModelNew6Image(image);
                    model.MainSize = new Size(250, 250);
                    model.MainLocation = new Point(25, 30);
                    model.MiddleGunIsUpper = true;
                    model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                        "M155,60 l20,4 l14,22 h15 l12,10" +
                        "v-15 l3,-4 a15,10 0 0,1 24,0 l10,7 l15,70 a40,5 0 0,1 -49,5 l-6,7 l3,6 l-22,18 v5 l-7,6 " +
                        "  l10,10 l-2,6 l8,8 l7,18 l-9,8 l-16,-13 l-16,-8 l-5,12 l-4,-2 l-5,10 h-13 l-5,-10 -4,2 l-5,-12" +
                        "l-16,8 l-16,13 l-9,-8 l7,-18 l8,-8 l-2,-6 l10,-10 l-7,-6 v-5 l-22,-18 l3,-6 l-6,-7 a40,5 0 0,1 -49,-5" +
                        " l15,-70 l10,-7 a15,10 0 0,1 24,0 l3,4 v15 l12,-10 h15 l14,-22 l20,-4 l3,3 h8z"));
                    model.LeftGunConnection = new Point(60, 110);
                    model.MiddleGunConnection = new Point(150, 135);
                    model.RightGunConnection = new Point(239, 110);
                    break;
                case 7:
                    model.MainImage = GetModelNew7Image(image);
                    model.MainSize = new Size(250, 250);
                    model.MainLocation = new Point(25, 20);
                    model.MiddleGunIsUpper = true;
                    model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                        "M157,45 a30,30 0 0,1 29,22 l10,9 h13 l3,3 v6 l7,6 v6 h5 v-8 h4 v-12 h29 v12 l5,5 " +
                        "l2,24 l3,24 l4,4 l-9,17 a45,20 0 0,1 -52,0 l-5,-8 l-15,12 l12,17 h6 l8,14 l-3,5  " +
                        " h-16 l-24,-3 l4,4 v15 h5 l9,10 l-3,6 l-19,-5 v10 l-12,10 v6 l-6,7 h-3 l-6,-7 v-6 2" +
                        " l-12,-10 v-10 l-19,5 l-3,-6 l9,-10 h5 v-15 l4,-4 l-24,3 h-16 l-3,-5 l8,-14 h6 l12,-17" +
                        "  l-15,-12 l-5,8 a45,20 0 0,1 -52,0 l-9,-17 l4,-4 l3,-24 l2,-24 l5,-5 v-12 h29 v12 h4"+
                        "v8 h5 v-6 l7,-6 v-6 l3,-3 h13 l10,-9 a30,30 0 0,1 29,-22z"));
                    model.LeftGunConnection = new Point(55, 110);
                    model.MiddleGunConnection = new Point(150, 200);
                    model.RightGunConnection = new Point(245, 110);
                    break;
                case 8:
                    model.MainImage = GetModelNew8Image(image);
                    model.MainSize = new Size(250, 250);
                    model.MainLocation = new Point(25, 20);
                    model.MiddleGunIsUpper = true;
                    model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                        "M154,42 v2 l5,5 l2,15 l8,18 l4,20 v37 h7 v4 h7 v-10 l10,-5 l16,10 v5 h4 l5,-8 l-14,-35 l8,2 l17,18" +
                        "l33,47 v16 l-24,15 v4 l-3,3 h-7 l-2,-5 l-5,-4 h-14 v12 l-7,6 h-15 l-6,-6 v-6 h-12 v4 l-10,12 h-19 " +
                        "l-10,-12 v-4 h-12 v6 l-6,6 h-15 l-7,-6 v-12 h-14 l-5,4 l-2,5 h-7 l-3,-3 v-4 l-24,-15 v-15 l33,-47" +
                        " l17,-19 l8,-2 -14,35 l5,8 h4 v-5 l16,-10 l10,5 v10 h7 v-4 h7 v-37 l4,-20 l8,-18 l2,-15 l5,-5 v-2z"));
                    model.LeftGunConnection = new Point(105, 150);
                    model.MiddleGunConnection = new Point(151, 170);
                    model.RightGunConnection = new Point(198, 150);
                    break;
                case 9:
                    model.MainImage = GetModelNew9Image(image);
                    model.MainSize = new Size(250, 250);
                    model.MainLocation = new Point(25, 30);
                    model.LeftGunIsUpper = true;
                    model.RightGunIsUpper = true;
                    model.MiddleGunIsUpper = true;
                    model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                        "M150,58 l3,3 l12,16 h5 l14,19 l-9,7 l4,14 l-8,8 l3,14 h20 l11,8 l6,-17 l-11,-23 l4,-5 l19,15" +
                        "l46,49 l-1,14 l-8,20 l-7,-2 l-9,7 l1,7 l-4,7 h-13 l-3,-6 v-5 l-13,5 l1,6 l-6,11 h-17 l-4,-11" +
                        "l-14,-3 v5 l-7,7 v9 l-9,5 v5 l-5,6 h-1 l-5,-6 v-5 l-9,-5 v-9 l-7,-7 v-5 l-14,3 l-4,11 h-17" +
                        "l-6,-11 l1,-6 l-13,-5 v5 l-3,6 h-13 l-5,-7 l1,-7 l-9,-7 l-7,2 l-7,-20 l-1,-14 l46,-49 l19,-15"+
                        "l4,5 l-11,23 l6,17 l11,-8 h20 l3,-14 l-8,-8 l4,-14 l-9,-7 l14,-19 h5 l12,-16z"));
                    model.LeftGunConnection = new Point(75, 152);
                    model.MiddleGunConnection = new Point(150, 185);
                    model.RightGunConnection = new Point(225, 152);
                    break;
                case 10:
                    model.MainImage = GetModelNew10Image(image);
                    model.MainSize = new Size(230, 230);
                    model.MainLocation = new Point(35, 40);
                    model.LeftGunIsUpper = true;
                    model.RightGunIsUpper = true;
                    model.MiddleGunIsUpper = true;
                    model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                        "M115,73 a40,45 0 0,1 71,0 l3,-3 l5,10 l-1,6 a140,230 0 0,1 28,69 l-4,17 a50,30 20 0,1 -8,33 h-10 l-10,-5" +
                        "a30,20 0 0,0 -12,20 l6,-1 l12,15 l-2,7 l-21,-5 l-1,-5 h-3 a20,15 0 0,1 -12,10 a7,50 0 0,1 -11,0" +
                        "a20,15 0 0,1 -12,-10 h-3 l-1,5 l-21,5 l-2,-7 l12,-15 ,6,1 a30,20 0 0,0 -12,-20 l-10,5 h-10 a50,30 -20 0,1 -8,-33" +
                        "l-4,-17 a140,230 0 0,1 28,-69 l-1,-6 l5,-10z"));
                    model.LeftGunConnection = new Point(95, 154);
                    model.MiddleGunConnection = new Point(150, 170);
                    model.RightGunConnection = new Point(206, 154);
                    break;
                case 11:
                    model.MainImage = GetModelNew11Image(image);
                    model.MainSize = new Size(230, 230);
                    model.MainLocation = new Point(32, 40);
                    model.MiddleGunIsUpper = true;
                    model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                        "M112,80 a40,35 0 0,1 78,0 l5,9 a40,40 0 0,1 8,18 l3,7 a20,15 0 0,1 7,13 l3,7 a50,50 0 0,1 17,1" +
                        "a30,40 0 0,1 14,23 a50,50 0 0,1 -16,20 a50,50 0 0,1 -10,15 a10,16 50 0,1 -7,22 l11,10 l-1,6" +
                        " l-17,2 l-4,-7 l-7,10 l-4,14 l-6,-11 h-6 l-7,-5 h-3 a20,20 0 0,1 -15,13 a7,70 0 0,1 -9,0 " +
                        "a20,20 0 0,1 -15,-13 h-3 l-7,5 h-6 l-6,11 l-4,-14 l-7,-10 l-4,7 l-17,-2 l-1,-6 l11,-10"+
                        "a10,16 -50 0,1 -7,-22 a50,50 0 0,1 -10,-15 a50,50 0 0,1 -16,-20 a30,40 0 0,1 14,-23 a50,50 0 0,1 17,-1"+
                        "l3,-7 a20,15 0 0,1 7,-13 l3,-7 a40,40 0 0,1 8,-18z"));
                    model.LeftGunConnection = new Point(75, 154);
                    model.MiddleGunConnection = new Point(150, 165);
                    model.RightGunConnection = new Point(226, 154);
                    break;
                case 101: model.MainImage = Links.Brushes.Ships.ModelNew101Brushes[string.Format("S{0}", image[1])];
                    model.MainSize = new Size(220, 172);
                    model.MainLocation = new Point(40, 60);
                    model.LeftGunIsUpper = true;
                    model.RightGunIsUpper = true;
                    model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                        "M58,132 a13.5,22.5 0 0,1 27,0 l23,-10 l13,-26 v-7 a28.5,28 0 0,1 57,0" +
                        "v7 l13,26 l23,10 a13.5,22.5 0 0,1 27,0 l7,2 a40,55 0 0,1 -14,89 l-17,10 l1.5,-9 a10,10 0 0,0 -10,-12" +
                        "h-11 l-5,14 h-22 l-1,-9 h-7 a5,5 0 0,1 -11,0 h-4 a5,5 0 0,1 -11,0 h-7 l-1,9 h-22 l-5,-14 h-11" +
                        "a10,10 0 0,0 -10,12 l1.5,9 l-17,-10 a40,55 0 0,1 -14,-89z"));
                    model.LeftGunConnection = new Point(67, 126);
                    model.MiddleGunConnection = new Point(150, 94);
                    model.RightGunConnection = new Point(230, 126);
                    break;
                case 102: throw new Exception();
                case 103: model.MainImage = Links.Brushes.Ships.ModelNew103Brushes["S1"];
                    model.MainSize = new Size(250, 117);
                    model.MainLocation = new Point(25, 90);
                    model.LeftGunIsUpper = true;
                    model.MiddleGunIsUpper = true;
                    model.RightGunIsUpper = true;
                    model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                        "M25,140 a15,15 0 0,1 8,-20 a25,25 0 0,1 12,-28  a40,15 0 0,1 50,0 a25,25 0 0,1 15,18 a40,15 0 0,0 80,0 a25,25 0 0,1 15,-18 " +
                        "a40,15 0 0,1 50,0 a25,25 0 0,1 12,28 a15,15 0 0,1 8,20 l-30,60 a25,35 0 0,1 -38,-7" +
                        "a25,17 0 0,1 -23,-7 a16,16 0 0,1 -31,0 h-7 a16,16 0 0,1 -31,0 a25,17 0 0,1 -23,7  a25,35 0 0,1 -38,7z"));
                    model.LeftGunConnection = new Point(72, 126);
                    model.MiddleGunConnection = new Point(150, 150);
                    model.RightGunConnection = new Point(229, 126);
                    break;
                case 104: model.MainImage = Links.Brushes.Ships.ModelNew104Brushes["S1"];
                    model.MainSize = new Size(220, 192);
                    model.MainLocation = new Point(40, 50);
                    model.LeftGunIsUpper = true;
                    model.RightGunIsUpper = true;
                    model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                        "M140,94 h18 l11,14 l2,15 h8 l10,16 l24,-20" +
                        "v-40 l-5,-6 h-3 v-23 h3 l18,12 l34,105 l-23,45 h-7 l-3,15 l-10,14 h-10 v-15 h-7 v-6" +
                        "l-12,-15 l-6,5 v13 l-8,15 h-52 l-8,-15 v-13 l-4,-2 l-11,12 v6 h-7 v15 h-10 l-9,-14" +
                        " l-3,-15 h-7 l-23,-45 l34,-105 l18,-12 h3 v23 h-3 l-5,6 v40 l24,20 l10,-16 h6 l3,-15z"));
                    model.LeftGunConnection = new Point(74, 108);
                    model.MiddleGunConnection = new Point(150, 120);
                    model.RightGunConnection = new Point(225, 108);
                    break;
                case 105: model.MainImage = Links.Brushes.Ships.ModelNew105Brushes["S1"];
                    model.MainSize = new Size(180, 241);
                    model.MainLocation = new Point(60, 20);
                    model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                       "M140,60 h22 l2,-38 h12 l40,92 a80,80 0 0,1 18,38 l-12,4 v2 l9,3 v16 l-9,3 v2 l12,4 a80,80 0 0,1 -22,41" +
                       "l-3,-2 v10 a100,120 0 0,1 -118,0 v-10 l-3,2 a80,80 0 0,1 -22,-41 l12,-4 v-2 l-9,-3 v-16 l9,-3 v-2 l-12,-4" +
                       " a80,80 0 0,1 7,-20 l-9,-5 v-28 l5,-13 h10 l5,13 v10 l41,-88 h13z" ));
                    model.LeftGunConnection = new Point(120, 95);
                    model.MiddleGunConnection = new Point(150, 125);
                    model.RightGunConnection = new Point(180, 95);
                    break;
                case 251: model.MainImage = Links.Brushes.Ships.ModelNew251Brushes[string.Format("S{0}", image[1])];
                    model.MainSize = new Size(220, 172);
                    model.MainLocation = new Point(40, 60);
                    model.LeftGunIsUpper = true;
                    model.RightGunIsUpper = true;
                    model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                        "M58,132 a13.5,22.5 0 0,1 27,0 l23,-10 l13,-26 v-7 a28.5,28 0 0,1 57,0" +
                        "v7 l13,26 l23,10 a13.5,22.5 0 0,1 27,0 l7,2 a40,55 0 0,1 -14,89 l-17,10 l1.5,-9 a10,10 0 0,0 -10,-12" +
                        "h-11 l-5,14 h-22 l-1,-9 h-7 a5,5 0 0,1 -11,0 h-4 a5,5 0 0,1 -11,0 h-7 l-1,9 h-22 l-5,-14 h-11" +
                        "a10,10 0 0,0 -10,12 l1.5,9 l-17,-10 a40,55 0 0,1 -14,-89z"));
                    model.LeftGunConnection = new Point(67, 126);
                    model.MiddleGunConnection = new Point(150, 94);
                    model.RightGunConnection = new Point(230, 126);
                    break;
                case 252: model.MainImage= Links.Brushes.Ships.ModelNew252Brushes[string.Format("S{0}", image[1])];
                    switch (image[1])
                    {
                        case 1:
                            model.MainSize = new Size(200, 200);
                            model.MainLocation = new Point(50, 50);
                            model.LeftGunIsUpper = true;
                            model.RightGunIsUpper = true;
                            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                                "M140,54 a5,2 0 0,1 20,0 l19,35 l11,5 v-15" +
                                "l8,-4 h8 l17,32 l15,-20 l-10,40 v19 l20,-25 l-15,40 l15,-13 l-11,28 l2,15 h12 l-5,7" +
                                "l-8,5 l-2,7 h-10 l15,40 l-22,-22 h-4 l3,12 h-41 l3,-12 h-25 l-3,4 h-4 l-3,-4 h-27" +
                                "l3,12 h-41 l3,-12 h-4 l-22,22 l15,-40 h-10 l-2,-9 l-8,-5 l-2,-6 h10 l2,-15 l-11,-28" +
                                "l15,13 l-16,-40 l21,25 v-19 l-10,-40 l15,20 l16,-30 h10 l7,3 v15 l11,-5 l19,-35z"));
                            model.LeftGunConnection = new Point(92, 116);
                            model.MiddleGunConnection = new Point(150, 96);
                            model.RightGunConnection = new Point(204, 116);
                            break;
                        case 2:
                            model.MainSize = new Size(172, 200);
                            model.MainLocation = new Point(64, 50);
                            model.LeftGunIsUpper = true;
                            model.RightGunIsUpper = true;
                            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                                "M142,54 a5,3 0 0,1 14,0 l16,42 l10,16" +
                                "v13 l10,17 l6,3 v-10 l10,-3 l11,20 l8,-10 l-5,35 l11,-12 l-7,20 l8,-5 l-5,15 l0,10 h6 l-2,5" +
                                "l-4,3 l-2,5 h-5 l7,23 l-10,-10 h-2 l1,8  l-5,10 h-15 l-5,-10 v-10 h-20 v-8 h-11 l-4,-10 h-16" +
                                "l-4,10 h-11 v8 h-20 v10 l-5,10 h-15 l-5,-10 l-1,-8 h-2 l-10,10 l8,-23 h-5 l-2,-5 l-4,-3 l-2,-5" +
                                "h6 v-10 l-5,-15 l8,5 l-7,-20 l11,12 l-5,-35 l8,10 l11,-20 l10,3 v10 l6,-3 l10,-17 v-13 l10,-16z"));
                            model.LeftGunConnection = new Point(92, 155);
                            model.MiddleGunConnection = new Point(150, 95);
                            model.RightGunConnection = new Point(207, 155);
                            break;
                        case 3:
                            model.MainSize = new Size(260, 166);
                            model.MainLocation = new Point(20, 53);
                            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                                "M105,55 a 20,20 0 0,1 18,0 l20,17 h-10 l10,16 a7,6 0 0,1 13,0" +
                                "l10,-16 h-10 l20,-17 a20,20 0 0,1 18,0 l18,30 l13,6 v-15 a20,20 0 0,1 15,0 l15,24 l13,-17 l-10,35 v12 l20,-20 l-15,32 l13,-9 " +
                                "l-8,20 v13 h12 l-4,6 l-7,3 l-2,6 h-8 l11,30 l-20,-17 l-3,10 h-35 l4,-7 l-27,-2 v15 l-5,8 h-18 l-7,-10 h-18 l-7,10 h-18 l-5,-8 v-15 " +
                                "l-27,2 l4,7 h-35 l-3,-10 l-20,17 l11,-30 h-8 l-2,-6 l-7,-3 l-4,-6 h11 v-13 l-8,-20 l13,9 l-15,-32 l20,20 v-12 l-10,-35 l13,17 l15,-24"+
                                " a20,20 0 0,1 15,0 v15 l13,-6 l18,-30z"));
                            model.LeftGunConnection = new Point(85, 120);
                            model.MiddleGunConnection = new Point(150, 90);
                            model.RightGunConnection = new Point(212, 120);
                            break;
                        default: throw new Exception("Ошибка модели пиратского корабля");
                    }
                    break;
                case 253: model.MainImage = Links.Brushes.Ships.ModelNew253Brushes[string.Format("S{0}", image[1])];
                    switch (image[1])
                    {
                        case 1:
                            model.MainSize = new Size(200, 190);
                            model.MainLocation = new Point(50, 50);
                            model.LeftGunIsUpper = true;
                            model.RightGunIsUpper = true;
                            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                                "M113,82 a45,72 0 0,1 74,0 a100,100 0 0,1 32,66 l12,9 l7,-17 a50,60 0 0,1 10,60" +
                                "a20,20 0 0,1 -20,1 v-16 a20,20 0 0,0 -22,-3 l3,10 l-12,35 l-10,12 h-5 l-7,-7 v-7 l-8,-10 v-20 l-10,3 a7,17 0 0,1 -14,0" +
                                "l-10,-3 v20 l-8,10 v7 l-7,7 h-5 l-10,-12 l-12,-35 l3,-10 a20,20 0 0,0 -22,3 v16 a20,20 0 0,1 -20,-1 a50,60 0 0,1 10,-60" +
                                "l7,17 l12,-9 a100,100 0 0,1 32,-66"));
                            model.LeftGunConnection = new Point(59, 173);
                            model.MiddleGunConnection = new Point(150, 70);
                            model.RightGunConnection = new Point(240, 173);
                            break;
                        case 2:
                            model.MainSize = new Size(200, 200);
                            model.MainLocation = new Point(50, 57);
                            model.LeftGunIsUpper = true;
                            model.RightGunIsUpper = true;
                            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                                "M111,96 a50,100 0 0,1 78,0 l13,22 l-5,10 l2,7 l5,-2 l4,10 h8" +
                                "v26 h13 l6,-22 a65,60 0 0,1 9,64 a30,60 0 0,1 -27,0 l4,-15 l-10,-3 l-10,15 l-15,10 l-7,-7 l-9,5 l6,20 l-26,22 l-26,-22" +
                                " l6,-20 l-9,-5 l-7,7 l-15,-10 l-10,-15 l-10,3 l4,15 a30,60 0 0,1 -27,0 a65,60 0 0,1 9,-64 l6,22 h13 v-26 h8 l4,-10 l5,2 " +
                                "l2,-7 l-5,-10 l13,-22"));
                            model.LeftGunConnection = new Point(65, 180);
                            model.MiddleGunConnection = new Point(150, 90);
                            model.RightGunConnection = new Point(235, 180);
                            break;
                        case 3:
                            model.MainSize = new Size(200, 200);
                            model.MainLocation = new Point(50, 60);
                            model.LeftGunIsUpper = true;
                            model.RightGunIsUpper = true;
                            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                                "M150,60 l40,35 l10,20 l25,10 l23,45 v40 l-10,20 h-30 l-30,25 h-56" +
                                "l-30,-25 h-30 l-10,-20 v-40 l23,-45 l25,-10 l10,-20 z M184,143 a20,20 0 0,1 -9,30 a20,20 0 0,1 9,-30"+
                                "M117,143 a20,20 0 0,1 9,30 a20,20 0 0,1 -9,-30 M207,161 a20,20 0 0,0 11,7 a20,20 0 0,1 -8,29 a20,20 0 0,0 -23,12 a20,20 0 0,1 1,-20 a30,35 0 0,0 19,-28" +
                                "M94,161 a20,20 0 0,1 -11,7 a20,20 0 0,0 8,29 a20,20 0 0,1 23,12 a20,20 0 0,0 -1,-20 a30,35 0 0,1 -19,-28"));
                            model.LeftGunConnection = new Point(75, 185);
                            model.MiddleGunConnection = new Point(150, 95);
                            model.RightGunConnection = new Point(225, 185);
                            break;
                        default: throw new Exception("Ошибка модели корабля вторжения");
                    }
                    break;
                case 254:
                    model.MainImage = Links.Brushes.Ships.ModelNew254Brushes[string.Format("S{0}", image[1])];
                    model.MainSize = new Size(220, 172);
                    model.MainLocation = new Point(40, 60);
                    model.LeftGunIsUpper = true;
                    model.RightGunIsUpper = true;
                    model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                        "M58,132 a13.5,22.5 0 0,1 27,0 l23,-10 l13,-26 v-7 a28.5,28 0 0,1 57,0" +
                        "v7 l13,26 l23,10 a13.5,22.5 0 0,1 27,0 l7,2 a40,55 0 0,1 -14,89 l-17,10 l1.5,-9 a10,10 0 0,0 -10,-12" +
                        "h-11 l-5,14 h-22 l-1,-9 h-7 a5,5 0 0,1 -11,0 h-4 a5,5 0 0,1 -11,0 h-7 l-1,9 h-22 l-5,-14 h-11" +
                        "a10,10 0 0,0 -10,12 l1.5,9 l-17,-10 a40,55 0 0,1 -14,-89z"));
                    model.LeftGunConnection = new Point(67, 126);
                    model.MiddleGunConnection = new Point(150, 94);
                    model.RightGunConnection = new Point(230, 126);
                    break;
                case 255:
                    model.MainImage = Links.Brushes.Ships.ModelNew255Brushes[string.Format("S{0}", image[1])];
                    model.MainSize = new Size(220, 172);
                    model.MainLocation = new Point(40, 60);
                    model.LeftGunIsUpper = true;
                    model.RightGunIsUpper = true;
                    model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                        "M58,132 a13.5,22.5 0 0,1 27,0 l23,-10 l13,-26 v-7 a28.5,28 0 0,1 57,0" +
                        "v7 l13,26 l23,10 a13.5,22.5 0 0,1 27,0 l7,2 a40,55 0 0,1 -14,89 l-17,10 l1.5,-9 a10,10 0 0,0 -10,-12" +
                        "h-11 l-5,14 h-22 l-1,-9 h-7 a5,5 0 0,1 -11,0 h-4 a5,5 0 0,1 -11,0 h-7 l-1,9 h-22 l-5,-14 h-11" +
                        "a10,10 0 0,0 -10,12 l1.5,9 l-17,-10 a40,55 0 0,1 -14,-89z"));
                    model.LeftGunConnection = new Point(67, 126);
                    model.MiddleGunConnection = new Point(150, 94);
                    model.RightGunConnection = new Point(230, 126);
                    break;
            }
            return model;
        }
        public static ImageShipModel GetShipModelNew1()
        {
            ImageShipModel model = new ImageShipModel();
            model.MainImage = null;
            model.MainSize = new Size(260, 166);
            model.MainLocation = new Point(20, 53);
            model.LeftGunIsUpper = false;
            model.MiddleGunIsUpper = false;
            model.RightGunIsUpper = false;
            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M114,218 l7,-8 v-4 l16,-28 l3,-20" +
                "a16,14 0 1,1 20,0 l3,20 l16,28 v4 l7,8 v-44 l-10,-40 a100,90 0 0,0 103,50 l-2,-10 a80,70 0 0,1 -57,-26" +
                "h15 l10,7 h4 v-15 l-62,-67 l-16,-2 a21,14 0 0,0 -42,0 l-16,2 l-62,67 v15 h5 l10,-7 h14 a80,70 0 0,1 -59,26" +
                "l-2,10 a100,90 0 0,0 103,-50 l-8,40z"));
            model.LeftGunConnection = new Point(85, 120);
            model.MiddleGunConnection = new Point(150, 90);
            model.RightGunConnection = new Point(212, 120);
            model.HealthPoint = new Point(80, 187);
            model.ShieldPoint = new Point(191, 191);
            model.EnergyPoint = new Point(135, 160);
            model.IsSpecialImage = true;
            model.GetSpecialImage = GetModelNew1Image;
            return model;
        }
        public static DrawingBrush GetModelNew1Image(byte[] image)
        {
            string cabinpos = String.Format("S3{0}", image[3] % 10);
            string wingspos = String.Format("S2{0}", image[2] % 10);
            string basepos = String.Format("S1{0}", image[1] % 10);
            DrawingGroup group = new DrawingGroup();
            ImageBrush base1 = Links.Brushes.Ships.ModelNew1Brushes[basepos];
            ImageBrush wings = Links.Brushes.Ships.ModelNew1Brushes[wingspos];
            ImageBrush cabin = Links.Brushes.Ships.ModelNew1Brushes[cabinpos];
            group.Children.Add(new GeometryDrawing(cabin, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M51,-4 h158 v90 h-158z"))));
            group.Children.Add(new GeometryDrawing(wings, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,23 h260 v110 h-260z"))));
            group.Children.Add(new GeometryDrawing(base1, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M31,0 h198 v166 h-198z"))));
            return new DrawingBrush(group);
        }
        public static ImageShipModel GetShipModelNew2()
        {
            ImageShipModel model = new ImageShipModel();
            model.MainImage = null;
            model.MainSize = new Size(240, 240);
            model.MainLocation = new Point(35, 30);
            model.LeftGunIsUpper = false;
            model.MiddleGunIsUpper = false;
            model.RightGunIsUpper = false;
            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M120,84 a30,36 0 0,1 60,0 l-5,10 a30,30 0 0,1 4,35 a15,10 60 0,0 24,6 l-8,-28 l5,-8" +
                "a5,10 0 0,1 -1,-15 v-22 a70,83 0 0,1 26,126 a20,20 0 0,1 -20,0 v15 l5,5 v18 l-6,3 v22" +
                "l-6,3 l-8,-10 v-10 l-7,-8 v-10 l-7,-9 a40,30 0 0,1 -55,0 l-7,9 v10 l-6,10 v9 l-8,10 l-6,-3" +
                "v-24 l-6,-3 v-16 l5,-5 v-15 a20,20 0 0,1 -18,0 a70,83 0 0,1 26,-126 v22 a5,10 0 0,1 -1,15" +
                " l5,8 l-8,28 a15,10 -60 0,0 24,-6 a30,30 0 0,1 4,-35z"));
            model.LeftGunConnection = new Point(88, 93);
            model.MiddleGunConnection = new Point(150, 63);
            model.RightGunConnection = new Point(212, 93);
            model.HealthPoint = new Point(80, 187);
            model.ShieldPoint = new Point(191, 191);
            model.EnergyPoint = new Point(135, 160);
            model.IsSpecialImage = true;
            model.GetSpecialImage = GetModelNew2Image;
            return model;
        }
        public static DrawingBrush GetModelNew2Image(byte[] image)
        {
            string cabinpos = String.Format("S3{0}", image[3] % 10);
            string wingspos = String.Format("S2{0}", image[2] % 10);
            string basepos = String.Format("S1{0}", image[1] % 10);
            DrawingGroup group = new DrawingGroup();
            ImageBrush base1 = Links.Brushes.Ships.ModelNew2Brushes[basepos];
            ImageBrush wings = Links.Brushes.Ships.ModelNew2Brushes[wingspos];
            ImageBrush cabin = Links.Brushes.Ships.ModelNew2Brushes[cabinpos];
            group.Children.Add(new GeometryDrawing(cabin, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h240 v240 h-240z"))));
            group.Children.Add(new GeometryDrawing(wings, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h240 v240 h-240z"))));
            group.Children.Add(new GeometryDrawing(base1, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h240 v240 h-240z"))));
            return new DrawingBrush(group);
        }
        public static ImageShipModel GetShipModelNew3()
        {
            ImageShipModel model = new ImageShipModel();
            model.MainImage = null;
            model.MainSize = new Size(200, 150);
            model.MainLocation = new Point(50, 67);
            model.LeftGunIsUpper = true;
            model.MiddleGunIsUpper = false;
            model.RightGunIsUpper = true;
            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M109,68 l12,22 l-7,10 l7,7 v-4 l15,-23 h28 " +
                "l15,23 v4 l7,-7 l-7,-10 l12,-22 h12 a70,70 0 0,1 38,60 l-20,7 l-4,-5 h-6 l-3,-4 l-10,5 l3,8 l-4,3 l12,4" +
                " l3,-4 h7 l2,-4 l18,6 v5 a100,85 0 0,1 5,69 l-18,-10 v-15 l-11,14 l-10,-13 l-13,12 l-30,-28 l-3,-8 l4,-4" +
                " l-5,-23 h-15 l-5,23 l4,4 l-3,8 l-30,28 l-13,-12 l-10,13 l-11,-14 v15 l-18,10 a100,85 0 0,1 5,-69 v-5 " +
                "  l18,-6 l2,4 h7 l3,4 l12,-4 l-4,-3 l3,-8 l-10,-5 l-3,4 h-6 l-4,5 l-20,-7 a70,70 0 0,1 38,-60 z"));
            model.LeftGunConnection = new Point(90, 100);
            model.MiddleGunConnection = new Point(150, 110);
            model.RightGunConnection = new Point(212, 100);
            model.HealthPoint = new Point(80, 187);
            model.ShieldPoint = new Point(191, 191);
            model.EnergyPoint = new Point(135, 160);
            model.IsSpecialImage = true;
            model.GetSpecialImage = GetModelNew3Image;
            return model;
        }
        public static DrawingBrush GetModelNew3Image(byte[] image)
        {
            string cabinpos = String.Format("S1{0}", image[1] % 10);
            string wingspos = String.Format("S2{0}", image[2] % 10);
            string basepos = String.Format("S3{0}", image[3] % 10);
            DrawingGroup group = new DrawingGroup();
            ImageBrush base1 = Links.Brushes.Ships.ModelNew3Brushes[basepos];
            ImageBrush wings = Links.Brushes.Ships.ModelNew3Brushes[wingspos];
            ImageBrush cabin = Links.Brushes.Ships.ModelNew3Brushes[cabinpos];
            group.Children.Add(new GeometryDrawing(cabin, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M13,13 h174 v116 h-174z"))));
            group.Children.Add(new GeometryDrawing(wings, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M49,13 h102 v68 h-102z"))));
            group.Children.Add(new GeometryDrawing(base1, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h200 v150 h-200z"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetModelNew4Image(byte[] image)
        {
            string cabinpos = String.Format("S1{0}", image[1] % 1);
            string wingspos = String.Format("S2{0}", image[2] % 1);
            string basepos = String.Format("S3{0}", image[3] % 1);
            DrawingGroup group = new DrawingGroup();
            ImageBrush base1 = Links.Brushes.Ships.ModelNew4Brushes[basepos];
            ImageBrush wings = Links.Brushes.Ships.ModelNew4Brushes[wingspos];
            ImageBrush cabin = Links.Brushes.Ships.ModelNew4Brushes[cabinpos];
            group.Children.Add(new GeometryDrawing(cabin, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,20 h250 v250 h-250z"))));
            group.Children.Add(new GeometryDrawing(wings, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,20 h250 v250 h-250z"))));
            group.Children.Add(new GeometryDrawing(base1, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,20 h250 v250 h-250z"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetModelNew5Image(byte[] image)
        {
            string cabinpos = String.Format("S1{0}", image[1] % 1);
            string wingspos = String.Format("S2{0}", image[2] % 1);
            string basepos = String.Format("S3{0}", image[3] % 1);
            DrawingGroup group = new DrawingGroup();
            ImageBrush base1 = Links.Brushes.Ships.ModelNew5Brushes[basepos];
            ImageBrush wings = Links.Brushes.Ships.ModelNew5Brushes[wingspos];
            ImageBrush cabin = Links.Brushes.Ships.ModelNew5Brushes[cabinpos];
            group.Children.Add(new GeometryDrawing(cabin, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,20 h250 v250 h-250z"))));
            group.Children.Add(new GeometryDrawing(wings, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,20 h250 v250 h-250z"))));
            group.Children.Add(new GeometryDrawing(base1, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,20 h250 v250 h-250z"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetModelNew6Image(byte[] image)
        {
            string cabinpos = String.Format("S1{0}", image[1] % 1);
            string wingspos = String.Format("S2{0}", image[2] % 1);
            string basepos = String.Format("S3{0}", image[3] % 1);
            DrawingGroup group = new DrawingGroup();
            ImageBrush base1 = Links.Brushes.Ships.ModelNew6Brushes[basepos];
            ImageBrush wings = Links.Brushes.Ships.ModelNew6Brushes[wingspos];
            ImageBrush cabin = Links.Brushes.Ships.ModelNew6Brushes[cabinpos];
            group.Children.Add(new GeometryDrawing(cabin, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,30 h250 v250 h-250z"))));
            group.Children.Add(new GeometryDrawing(wings, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,30 h250 v250 h-250z"))));
            group.Children.Add(new GeometryDrawing(base1, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,30 h250 v250 h-250z"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetModelNew7Image(byte[] image)
        {
            string cabinpos = String.Format("S1{0}", image[1] % 1);
            string wingspos = String.Format("S2{0}", image[2] % 1);
            string basepos = String.Format("S3{0}", image[3] % 1);
            DrawingGroup group = new DrawingGroup();
            ImageBrush base1 = Links.Brushes.Ships.ModelNew7Brushes[basepos];
            ImageBrush wings = Links.Brushes.Ships.ModelNew7Brushes[wingspos];
            ImageBrush cabin = Links.Brushes.Ships.ModelNew7Brushes[cabinpos];
            group.Children.Add(new GeometryDrawing(cabin, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,20 h250 v250 h-250z"))));
            group.Children.Add(new GeometryDrawing(wings, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,20 h250 v250 h-250z"))));
            group.Children.Add(new GeometryDrawing(base1, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,20 h250 v250 h-250z"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetModelNew8Image(byte[] image)
        {
            string cabinpos = String.Format("S1{0}", image[1] % 10);
            string wingspos = String.Format("S2{0}", image[2] % 10);
            string basepos = String.Format("S3{0}", image[3] % 10);
            DrawingGroup group = new DrawingGroup();
            ImageBrush base1 = Links.Brushes.Ships.ModelNew8Brushes[basepos];
            ImageBrush wings = Links.Brushes.Ships.ModelNew8Brushes[wingspos];
            ImageBrush cabin = Links.Brushes.Ships.ModelNew8Brushes[cabinpos];
            group.Children.Add(new GeometryDrawing(cabin, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,20 h250 v250 h-250z"))));
            group.Children.Add(new GeometryDrawing(wings, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,20 h250 v250 h-250z"))));
            group.Children.Add(new GeometryDrawing(base1, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M25,20 h250 v250 h-250z"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetModelNew9Image(byte[] image)
        {
            string cabinpos = String.Format("S1{0}", image[1] % 10);
            string wingspos = String.Format("S2{0}", image[2] % 10);
            string basepos = String.Format("S3{0}", image[3] % 10);
            DrawingGroup group = new DrawingGroup();
            ImageBrush base1 = Links.Brushes.Ships.ModelNew9Brushes[basepos];
            ImageBrush wings = Links.Brushes.Ships.ModelNew9Brushes[wingspos];
            ImageBrush cabin = Links.Brushes.Ships.ModelNew9Brushes[cabinpos];
            group.Children.Add(new GeometryDrawing(cabin, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M40,50 h250 v250 h-250z"))));
            group.Children.Add(new GeometryDrawing(wings, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M40,50 h250 v250 h-250z"))));
            group.Children.Add(new GeometryDrawing(base1, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M40,50 h250 v250 h-250z"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetModelNew10Image(byte[] image)
        {
            string cabinpos = String.Format("S1{0}", image[1] % 1);
            string wingspos = String.Format("S2{0}", image[2] % 1);
            string basepos = String.Format("S3{0}", image[3] % 1);
            DrawingGroup group = new DrawingGroup();
            ImageBrush base1 = Links.Brushes.Ships.ModelNew10Brushes[basepos];
            ImageBrush wings = Links.Brushes.Ships.ModelNew10Brushes[wingspos];
            ImageBrush cabin = Links.Brushes.Ships.ModelNew10Brushes[cabinpos];
            group.Children.Add(new GeometryDrawing(cabin, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M35,40 h230 v230 h-230z"))));
            group.Children.Add(new GeometryDrawing(wings, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M35,40 h230 v230 h-230z"))));
            group.Children.Add(new GeometryDrawing(base1, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M35,40 h230 v230 h-230z"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetModelNew11Image(byte[] image)
        {
            string cabinpos = String.Format("S1{0}", image[1] % 1);
            string wingspos = String.Format("S2{0}", image[2] % 1);
            string basepos = String.Format("S3{0}", image[3] % 1);
            DrawingGroup group = new DrawingGroup();
            ImageBrush base1 = Links.Brushes.Ships.ModelNew11Brushes[basepos];
            ImageBrush wings = Links.Brushes.Ships.ModelNew11Brushes[wingspos];
            ImageBrush cabin = Links.Brushes.Ships.ModelNew11Brushes[cabinpos];
            group.Children.Add(new GeometryDrawing(cabin, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M32,40 h230 v230 h-230z"))));
            group.Children.Add(new GeometryDrawing(wings, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M32,40 h230 v230 h-230z"))));
            group.Children.Add(new GeometryDrawing(base1, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M32,40 h230 v230 h-230z"))));
            return new DrawingBrush(group);
        }
        #endregion
        public static ImageShipModel GetShipModel1()
        {
            ImageShipModel model = new ImageShipModel();
            model.MainImage = Links.Brushes.Ships.Ship1;
            model.MainSize = new Size(250, 117);
            model.MainLocation = new Point(25, 90);
            model.LeftGunIsUpper = true;
            model.MiddleGunIsUpper = true;
            model.RightGunIsUpper = true;
            model.Geometry= new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M25,140 a15,15 0 0,1 8,-20 a25,25 0 0,1 12,-28  a40,15 0 0,1 50,0 a25,25 0 0,1 15,18 a40,15 0 0,0 80,0 a25,25 0 0,1 15,-18 " +
                "a40,15 0 0,1 50,0 a25,25 0 0,1 12,28 a15,15 0 0,1 8,20 l-30,60 a25,35 0 0,1 -38,-7" +
                "a25,17 0 0,1 -23,-7 a16,16 0 0,1 -31,0 h-7 a16,16 0 0,1 -31,0 a25,17 0 0,1 -23,7  a25,35 0 0,1 -38,7z"));
            model.LeftGunConnection = new Point(72, 126);
            model.MiddleGunConnection = new Point(150, 150);
            model.RightGunConnection = new Point(229, 126);
            model.HealthPoint = new Point(57, 153);
            model.ShieldPoint = new Point(214, 153);
            model.EnergyPoint = new Point(118, 180);
            return model;
        }
        public static ImageShipModel GetShipModel2()
        {
            ImageShipModel model = new ImageShipModel();
            model.MainImage = Links.Brushes.Ships.Ship2;
            model.MainSize = new Size(220, 172);
            model.MainLocation = new Point(40, 60);
            model.LeftGunIsUpper = true;
            model.MiddleGunIsUpper = false;
            model.RightGunIsUpper = true;
            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M58,132 a13.5,22.5 0 0,1 27,0 l23,-10 l13,-26 v-7 a28.5,28 0 0,1 57,0" +
                "v7 l13,26 l23,10 a13.5,22.5 0 0,1 27,0 l7,2 a40,55 0 0,1 -14,89 l-17,10 l1.5,-9 a10,10 0 0,0 -10,-12" +
                "h-11 l-5,14 h-22 l-1,-9 h-7 a5,5 0 0,1 -11,0 h-4 a5,5 0 0,1 -11,0 h-7 l-1,9 h-22 l-5,-14 h-11"+
                "a10,10 0 0,0 -10,12 l1.5,9 l-17,-10 a40,55 0 0,1 -14,-89z"));
            model.LeftGunConnection = new Point(67, 126);
            model.MiddleGunConnection = new Point(150, 94);
            model.RightGunConnection = new Point(230, 126);
            model.HealthPoint = new Point(90, 175);
            model.ShieldPoint = new Point(180, 175);
            model.EnergyPoint = new Point(135, 165);
            return model;
        }
        public static ImageShipModel GetShipModel3()
        {
            ImageShipModel model = new ImageShipModel();
            model.MainImage = Links.Brushes.Ships.Ship3;
            model.MainSize = new Size(220, 192);
            model.MainLocation = new Point(40, 50);
            model.LeftGunIsUpper = true;
            model.MiddleGunIsUpper = false;
            model.RightGunIsUpper = true;
            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M140,94 h18 l11,14 l2,15 h8 l10,16 l24,-20" +
                "v-40 l-5,-6 h-3 v-23 h3 l18,12 l34,105 l-23,45 h-7 l-3,15 l-10,14 h-10 v-15 h-7 v-6" +
                "l-12,-15 l-6,5 v13 l-8,15 h-52 l-8,-15 v-13 l-4,-2 l-11,12 v6 h-7 v15 h-10 l-9,-14" +
                " l-3,-15 h-7 l-23,-45 l34,-105 l18,-12 h3 v23 h-3 l-5,6 v40 l24,20 l10,-16 h6 l3,-15z"));
            model.LeftGunConnection = new Point(74, 108);
            model.MiddleGunConnection = new Point(150, 120);
            model.RightGunConnection = new Point(225, 108);
            model.HealthPoint = new Point(67, 146);
            model.ShieldPoint = new Point(203, 146);
            model.EnergyPoint = new Point(135, 140);
            return model;
        }
        public static ImageShipModel GetShipModel4()
        {
            ImageShipModel model = new ImageShipModel();
            model.MainImage = Links.Brushes.Ships.Ship4;
            model.MainSize = new Size(200, 200);
            model.MainLocation = new Point(50, 50);
            model.LeftGunIsUpper = true;
            model.MiddleGunIsUpper = false;
            model.RightGunIsUpper = true;
            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M140,54 a5,2 0 0,1 20,0 l19,35 l11,5 v-15" +
                "l8,-4 h8 l17,32 l15,-20 l-10,40 v19 l20,-25 l-15,40 l15,-13 l-11,28 l2,15 h12 l-5,7" +
                "l-8,5 l-2,7 h-10 l15,40 l-22,-22 h-4 l3,12 h-41 l3,-12 h-25 l-3,4 h-4 l-3,-4 h-27" +
                "l3,12 h-41 l3,-12 h-4 l-22,22 l15,-40 h-10 l-2,-9 l-8,-5 l-2,-6 h10 l2,-15 l-11,-28"+
                "l15,13 l-16,-40 l21,25 v-19 l-10,-40 l15,20 l16,-30 h10 l7,3 v15 l11,-5 l19,-35z"));
            model.LeftGunConnection = new Point(92, 116);
            model.MiddleGunConnection = new Point(150, 96);
            model.RightGunConnection = new Point(204, 116);
            model.HealthPoint = new Point(82, 178);
            model.ShieldPoint = new Point(184, 178);
            model.EnergyPoint = new Point(135, 148);
            return model;
        }
        public static ImageShipModel GetShipModel5()
        {
            ImageShipModel model = new ImageShipModel();
            model.MainImage = Links.Brushes.Ships.Ship5;
            model.MainSize = new Size(172, 200);
            model.MainLocation = new Point(64, 50);
            model.LeftGunIsUpper = true;
            model.MiddleGunIsUpper = false;
            model.RightGunIsUpper = true;
            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M142,54 a5,3 0 0,1 14,0 l16,42 l10,16" +
                "v13 l10,17 l6,3 v-10 l10,-3 l11,20 l8,-10 l-5,35 l11,-12 l-7,20 l8,-5 l-5,15 l0,10 h6 l-2,5" +
                "l-4,3 l-2,5 h-5 l7,23 l-10,-10 h-2 l1,8  l-5,10 h-15 l-5,-10 v-10 h-20 v-8 h-11 l-4,-10 h-16" +
                "l-4,10 h-11 v8 h-20 v10 l-5,10 h-15 l-5,-10 l-1,-8 h-2 l-10,10 l8,-23 h-5 l-2,-5 l-4,-3 l-2,-5" +
                "h6 v-10 l-5,-15 l8,5 l-7,-20 l11,12 l-5,-35 l8,10 l11,-20 l10,3 v10 l6,-3 l10,-17 v-13 l10,-16z"));
            model.LeftGunConnection = new Point(92, 155);
            model.MiddleGunConnection = new Point(150, 95);
            model.RightGunConnection = new Point(207, 155);
            model.HealthPoint = new Point(80, 187);
            model.ShieldPoint = new Point(191, 191);
            model.EnergyPoint = new Point(135, 160);
            return model;
        }
        public static ImageShipModel GetShipModel6()
        {
            ImageShipModel model = new ImageShipModel();
            model.MainImage = null;
            model.MainSize = new Size(260,166);
            model.MainLocation = new Point(20,53);
            model.LeftGunIsUpper = false;
            model.MiddleGunIsUpper = false;
            model.RightGunIsUpper = false;
            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M114,218 l7,-8 v-4 l16,-28 l3,-20" +
                "a16,14 0 1,1 20,0 l3,20 l16,28 v4 l7,8 v-44 l-10,-40 a100,90 0 0,0 103,50 l-2,-10 a80,70 0 0,1 -57,-26" +
                "h15 l10,7 h4 v-15 l-62,-67 l-16,-2 a21,14 0 0,0 -42,0 l-16,2 l-62,67 v15 h5 l10,-7 h14 a80,70 0 0,1 -59,26" +
                "l-2,10 a100,90 0 0,0 103,-50 l-8,40z"));
            model.LeftGunConnection = new Point(85, 120);
            model.MiddleGunConnection = new Point(150, 90);
            model.RightGunConnection = new Point(212, 120);
            model.HealthPoint = new Point(80, 187);
            model.ShieldPoint = new Point(191, 191);
            model.EnergyPoint = new Point(135, 160);
            model.IsSpecialImage = true;
            model.GetSpecialImage = GetModel6Image;
            return model;
        }
        public static DrawingBrush GetModel6Image(byte[] image)
        {
            string cabinpos = String.Format("S63{0}", image[3] % 10);
            string wingspos= String.Format("S62{0}", image[2] % 10);
            string basepos = String.Format("S61{0}", image[1] % 10);
            DrawingGroup group = new DrawingGroup();
            ImageBrush base1 = Links.Brushes.Ships.Model6Brushes[basepos];
            ImageBrush wings = Links.Brushes.Ships.Model6Brushes[wingspos];
            ImageBrush cabin = Links.Brushes.Ships.Model6Brushes[cabinpos];
            group.Children.Add(new GeometryDrawing(cabin, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M51,-4 h158 v90 h-158z"))));
            group.Children.Add(new GeometryDrawing(wings, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,23 h260 v110 h-260z"))));
            group.Children.Add(new GeometryDrawing(base1, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M31,0 h198 v166 h-198z"))));
            return new DrawingBrush(group);
        }
        public static ImageShipModel GetShipModel7()
        {
            ImageShipModel model = new ImageShipModel();
            model.MainImage = null;
            model.MainSize = new Size(240, 240);
            model.MainLocation = new Point(35, 30);
            model.LeftGunIsUpper = false;
            model.MiddleGunIsUpper = false;
            model.RightGunIsUpper = false;
            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M120,84 a30,36 0 0,1 60,0 l-5,10 a30,30 0 0,1 4,35 a15,10 60 0,0 24,6 l-8,-28 l5,-8" +
                "a5,10 0 0,1 -1,-15 v-22 a70,83 0 0,1 26,126 a20,20 0 0,1 -20,0 v15 l5,5 v18 l-6,3 v22" +
                "l-6,3 l-8,-10 v-10 l-7,-8 v-10 l-7,-9 a40,30 0 0,1 -55,0 l-7,9 v10 l-6,10 v9 l-8,10 l-6,-3" +
                "v-24 l-6,-3 v-16 l5,-5 v-15 a20,20 0 0,1 -18,0 a70,83 0 0,1 26,-126 v22 a5,10 0 0,1 -1,15"+
                " l5,8 l-8,28 a15,10 -60 0,0 24,-6 a30,30 0 0,1 4,-35z"));
            model.LeftGunConnection = new Point(88, 93);
            model.MiddleGunConnection = new Point(150, 63);
            model.RightGunConnection = new Point(212, 93);
            model.HealthPoint = new Point(80, 187);
            model.ShieldPoint = new Point(191, 191);
            model.EnergyPoint = new Point(135, 160);
            model.IsSpecialImage = true;
            model.GetSpecialImage = GetModel7Image;
            return model;
        }
        public static DrawingBrush GetModel7Image(byte[] image)
        {
            string cabinpos = String.Format("S73{0}", image[3] % 10);
            string wingspos = String.Format("S72{0}", image[2] % 10);
            string basepos = String.Format("S71{0}", image[1] % 10);
            DrawingGroup group = new DrawingGroup();
            ImageBrush base1 = Links.Brushes.Ships.Model7Brushes[basepos];
            ImageBrush wings = Links.Brushes.Ships.Model7Brushes[wingspos];
            ImageBrush cabin = Links.Brushes.Ships.Model7Brushes[cabinpos];
            group.Children.Add(new GeometryDrawing(cabin, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h240 v240 h-240z"))));
            group.Children.Add(new GeometryDrawing(wings, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h240 v240 h-240z"))));
            group.Children.Add(new GeometryDrawing(base1, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h240 v240 h-240z"))));
            return new DrawingBrush(group);
        }
        public static ImageShipModel GetShipModel9()
        {
            ImageShipModel model = new ImageShipModel();
            model.MainImage = null;
            model.MainSize = new Size(200, 150);
            model.MainLocation = new Point(50, 67);
            model.LeftGunIsUpper = true;
            model.MiddleGunIsUpper = false;
            model.RightGunIsUpper = true;
            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M109,68 l12,22 l-7,10 l7,7 v-4 l15,-23 h28 " +
                "l15,23 v4 l7,-7 l-7,-10 l12,-22 h12 a70,70 0 0,1 38,60 l-20,7 l-4,-5 h-6 l-3,-4 l-10,5 l3,8 l-4,3 l12,4" +
                " l3,-4 h7 l2,-4 l18,6 v5 a100,85 0 0,1 5,69 l-18,-10 v-15 l-11,14 l-10,-13 l-13,12 l-30,-28 l-3,-8 l4,-4" +
                " l-5,-23 h-15 l-5,23 l4,4 l-3,8 l-30,28 l-13,-12 l-10,13 l-11,-14 v15 l-18,10 a100,85 0 0,1 5,-69 v-5 " +
                "  l18,-6 l2,4 h7 l3,4 l12,-4 l-4,-3 l3,-8 l-10,-5 l-3,4 h-6 l-4,5 l-20,-7 a70,70 0 0,1 38,-60 z"));
            model.LeftGunConnection = new Point(90, 100);
            model.MiddleGunConnection = new Point(150, 110);
            model.RightGunConnection = new Point(212, 100);
            model.HealthPoint = new Point(80, 187);
            model.ShieldPoint = new Point(191, 191);
            model.EnergyPoint = new Point(135, 160);
            model.IsSpecialImage = true;
            model.GetSpecialImage = GetModel9Image;
            return model;
        }
        public static DrawingBrush GetModel9Image(byte[] image)
        {
            string cabinpos = String.Format("S91{0}", image[3] % 10);
            string wingspos = String.Format("S92{0}", image[2] % 10);
            string basepos = String.Format("S93{0}", image[1] % 10);
            DrawingGroup group = new DrawingGroup();
            ImageBrush base1 = Links.Brushes.Ships.Model9Brushes[basepos];
            ImageBrush wings = Links.Brushes.Ships.Model9Brushes[wingspos];
            ImageBrush cabin = Links.Brushes.Ships.Model9Brushes[cabinpos];
            group.Children.Add(new GeometryDrawing(cabin, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M13,13 h174 v116 h-174z"))));
            group.Children.Add(new GeometryDrawing(wings, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M49,13 h102 v68 h-102z"))));
            group.Children.Add(new GeometryDrawing(base1, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h200 v150 h-200z"))));
            return new DrawingBrush(group);
        }
        public static ImageShipModel GetShipModel8()
        {
            ImageShipModel model = new ImageShipModel();
            model.MainImage = Links.Brushes.Ships.Ship8;
            model.MainSize = new Size(200, 190);
            model.MainLocation = new Point(50, 50);
            model.LeftGunIsUpper = true;
            model.MiddleGunIsUpper = false;
            model.RightGunIsUpper = true;
            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M113,82 a45,72 0 0,1 74,0 a100,100 0 0,1 32,66 l12,9 l7,-17 a50,60 0 0,1 10,60" +
                "a20,20 0 0,1 -20,1 v-16 a20,20 0 0,0 -22,-3 l3,10 l-12,35 l-10,12 h-5 l-7,-7 v-7 l-8,-10 v-20 l-10,3 a7,17 0 0,1 -14,0" +
                "l-10,-3 v20 l-8,10 v7 l-7,7 h-5 l-10,-12 l-12,-35 l3,-10 a20,20 0 0,0 -22,3 v16 a20,20 0 0,1 -20,-1 a50,60 0 0,1 10,-60" +
                "l7,17 l12,-9 a100,100 0 0,1 32,-66"));
            model.LeftGunConnection = new Point(59, 173);
            model.MiddleGunConnection = new Point(150, 70);
            model.RightGunConnection = new Point(240, 173);
            model.HealthPoint = new Point(80, 187);
            model.ShieldPoint = new Point(191, 191);
            model.EnergyPoint = new Point(135, 160);
            return model;
        }
        public static ImageShipModel GetShipModel10()
        {
            ImageShipModel model = new ImageShipModel();
            model.MainImage = Links.Brushes.Ships.Ship10;
            model.MainSize = new Size(200, 200);
            model.MainLocation = new Point(50, 57);
            model.LeftGunIsUpper = true;
            model.MiddleGunIsUpper = false;
            model.RightGunIsUpper = true;
            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M111,96 a50,100 0 0,1 78,0 l13,22 l-5,10 l2,7 l5,-2 l4,10 h8" +
                "v26 h13 l6,-22 a65,60 0 0,1 9,64 a30,60 0 0,1 -27,0 l4,-15 l-10,-3 l-10,15 l-15,10 l-7,-7 l-9,5 l6,20 l-26,22 l-26,-22" +
                " l6,-20 l-9,-5 l-7,7 l-15,-10 l-10,-15 l-10,3 l4,15 a30,60 0 0,1 -27,0 a65,60 0 0,1 9,-64 l6,22 h13 v-26 h8 l4,-10 l5,2 " +
                "l2,-7 l-5,-10 l13,-22"));
            model.LeftGunConnection = new Point(65, 180);
            model.MiddleGunConnection = new Point(150, 90);
            model.RightGunConnection = new Point(235, 180);
            model.HealthPoint = new Point(80, 187);
            model.ShieldPoint = new Point(191, 191);
            model.EnergyPoint = new Point(135, 160);
            return model;
        }
        public static ImageShipModel GetShipModelBig1()
        {
            ImageShipModel model = new ImageShipModel();
            model.MainImage = Links.Brushes.Ships.Ship2;
            model.MainSize = new Size(440, 344);
            model.MainLocation = new Point(80, 120);
            model.LeftGunIsUpper = true;
            model.MiddleGunIsUpper = false;
            model.RightGunIsUpper = true;
            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M116,264 a27,45 0 0,1 54,0 l46,-20 l26,-52 v-14 a57,56 0 0,1 114,0" +
                "v14 l26,52 l46,20 a27,45 0 0,1 54,0 l14,4 a80,110 0 0,1 -28,178 l-34,20 l3,-18 a20,20 0 0,0 -20,-24" +
                "h-22 l-10,28 h-44 l-2,-18 h-14 a10,10 0 0,1 -22,0 h-8 a10,10 0 0,1 -22,0 h-14 l-2,18 h-44 l-10,-28 h-22" +
                "a20,20 0 0,0 -20,24 l3,18 l-34,-20 a80,110 0 0,1 -28,-178z"));
            model.LeftGunConnection = new Point(137, 250);
            model.MiddleGunConnection = new Point(300, 135);
            model.RightGunConnection = new Point(462, 250);
            model.HealthPoint = new Point(205, 365);
            model.ShieldPoint = new Point(365, 365);
            model.EnergyPoint = new Point(285, 345);
            return model;
        }
        public static ImageShipModel GetShipModelBig2()
        {
            ImageShipModel model = new ImageShipModel();
            model.MainImage = Links.Brushes.Ships.Ship3;
            model.MainSize = new Size(440, 383);
            model.MainLocation = new Point(80, 120);
            model.LeftGunIsUpper = true;
            model.MiddleGunIsUpper = true;
            model.RightGunIsUpper = true;
            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M190,120 v45 l-14,14 v80 l43,38 l22,-33 h12 l5,-30 l22,-28 h39" +
                "l22,28 l5,30 h12 l22,33 l43,-38 v-80 l-14,-14 v-45 h10 l34,24 l68,210 l-22,52 l-22,38 h-14 l-2,12" +
                " l-5,5 v10 l-21,30 h-22 v-28 h-12 v-12 l-25,-28  l-10,5 v25 l-15,30 h-108 l-15,-30 v-25 l-10,-5" +
                "l-20,28 v12 h-12 v28 h-22 l-21,-30 v-10 l-5,-5 l-2,-12 h-14 l-22,-38 l-22,-52 l68,-210 l34,-24z"));
            model.LeftGunConnection = new Point(155, 340);
            model.MiddleGunConnection = new Point(300, 255);
            model.RightGunConnection = new Point(445, 340);
            model.HealthPoint = new Point(245, 385);
            model.ShieldPoint = new Point(345, 385);
            model.EnergyPoint = new Point(285, 305);
            return model;
        }
        public static ImageShipModel GetShipModelBigPir()
        {
            ImageShipModel model = new ImageShipModel();
            model.MainImage = Links.Brushes.Ships.Ship6;
            model.MainSize = new Size(500, 410);
            model.MainLocation = new Point(45, 100);
            model.LeftGunIsUpper = true;
            model.MiddleGunIsUpper = false;
            model.RightGunIsUpper = true;
            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M208,110 a20,20 0 0,1 35,0 l6,6 h10 l25,30 h23 " +
                "l25,-30 h10 l3,-6 a20,20 0 0,1 35,0 l35, 70 l22,10 v-32 a20,20 0 0,1 22,0 l34,40 l5,15 l23,-40 l-17,115" +
                "l35,-45 l-27,75 l28,-25 l-17,55 v31 h22 l-22,22 v15 h-20 l23,75 l-37,-40 h-8 l5,24 h-73 l5,-24 h-45 v33" +
                "h22 v-31 l-17,-55 l28,25 l-27,-75 l35,45 l-17,-115 l23,40 l5,-15 l32,-40 a20,20 0 0,1 22,0 v32 l22,-10 z" +
                ""));
            model.LeftGunConnection = new Point(135, 215);
            model.MiddleGunConnection = new Point(300, 175);
            model.RightGunConnection = new Point(465, 215);
            model.HealthPoint = new Point(245, 385);
            model.ShieldPoint = new Point(345, 385);
            model.EnergyPoint = new Point(285, 305);
            return model;
        }
        public static ImageShipModel GetShipModelBigSokol()
        {
            ImageShipModel model = new ImageShipModel();
            model.MainImage = Links.Brushes.Ships.ModelNew105Brushes["S1"];
            model.MainSize = new Size(360, 482);
            model.MainLocation = new Point(120, 40);
            model.LeftGunIsUpper = false;
            model.MiddleGunIsUpper = false;
            model.RightGunIsUpper = false;
            model.Geometry = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M280,120 h44 l4,-76 h24 l80,184 a160,160 0 0,1 36,76 l-24,8 v4 l18,6 v32 l-18,6 v4 l24,8 a160,160 0 0,1 -44,82" +
                "l-6,-4 v20 a200,240 0 0,1 -236,0 v-20 l-6,4 a160,160 0 0,1 -44,-82 l24,-8 v-4 l-18,-6 v-32 l18,-6 v-4 l-24,-8" +
                " a160,160 0 0,1 14,-40 l-18,-10 v-56 l10,-26 h20 l10,26 v20 l82,-176 h26z"));
            model.LeftGunConnection = new Point(235, 185);
            model.MiddleGunConnection = new Point(300, 245);
            model.RightGunConnection = new Point(365, 185);
            model.HealthPoint = new Point(245, 385);
            model.ShieldPoint = new Point(345, 385);
            model.EnergyPoint = new Point(285, 305);
            return model;
        }
        public static void CreateShipsAndGuns()
        {
            Ships = new List<ImageShipModel>();
            Ships.Add(GetShipModel1());//0
            Ships.Add(GetShipModel2());//1
            Ships.Add(GetShipModel3());//2
            Ships.Add(GetShipModel4());//3
            Ships.Add(GetShipModel5());//4
            Ships.Add(GetShipModel6());//5
            Ships.Add(GetShipModel7());//6
            Ships.Add(GetShipModel8());//7
            Ships.Add(GetShipModel9());//8
            Ships.Add(GetShipModel10());//9
            Guns = new List<ImageGunModel>();
            Guns.Add(ImageGunModel.GetGunModel12());
            Guns.Add(ImageGunModel.GetGunModel5());
            Guns.Add(ImageGunModel.GetGunModel2());
            Guns.Add(ImageGunModel.GetGunModel1());
            Guns.Add(ImageGunModel.GetGunModel9());
            Guns.Add(ImageGunModel.GetGunModel6());
            Guns.Add(ImageGunModel.GetGunModel7());
            Guns.Add(ImageGunModel.GetGunModel8());
            Guns.Add(ImageGunModel.GetGunModel14());
            Guns.Add(ImageGunModel.GetGunModel10());
            Guns.Add(ImageGunModel.GetGunModel2());
            Guns.Add(ImageGunModel.GetGunModel13());
            Guns.Add(ImageGunModel.GetGunModel11());
            Guns.Add(ImageGunModel.GetGunModel3());
            Guns.Add(ImageGunModel.GetGunModel2());
            Guns.Add(ImageGunModel.GetGunModel4());

            BigShips = new List<ImageShipModel>();
            BigShips.Add(GetShipModelBig1());
            BigShips.Add(GetShipModelBig2());
            BigShips.Add(GetShipModelBigPir());
            BigShips.Add(GetShipModelBigSokol());
            BigGuns = new List<ImageGunModel>();
            BigGuns.Add(ImageGunModel.GetGunModelBig2());
            LargeBuildings = new List<ImageShipModel>();
            LargeBuildings.Add(GetShipModelBigSokol());
        }
    }
    class ImageGunModel
    {
        public Brush GunImage;
        public Size GunSize;
        public Point ConnectionPoint;
        public string Geometry;
        public Point FirePoint;
        public Point GeometryDelta;
        public PathGeometry GetGun(Point GunConnection)
        {
            Point Location = GetGunLocation(GunConnection);
            PathGeometry geom = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                string.Format(Geometry, 
                (Location.X + GeometryDelta.X).ToString("0.#").Replace(',','.'), 
                (Location.Y + GeometryDelta.Y).ToString("0.#").Replace(',','.')
                )));
            return geom;
        }
        public Point GetGunLocation(Point GunConnection)
        {
            return new Point(GunConnection.X - GunSize.Width + ConnectionPoint.X, GunConnection.Y - ConnectionPoint.Y);
        }
        public Rectangle GetRectangle(Point GunConnection)
        {
            Rectangle rect = new Rectangle();
            rect.Width = GunSize.Width;
            rect.Height = GunSize.Height;
            rect.Fill = GunImage;
            Canvas.SetLeft(rect, GetGunLocation(GunConnection).X);
            Canvas.SetTop(rect, GetGunLocation(GunConnection).Y);
            return rect;
        }
        public Point GetFirePoint(Point GunConncetion)
        {
            Point Location = GetGunLocation(GunConncetion);
            return new Point(Location.X + FirePoint.X, Location.Y + FirePoint.Y);
        }
        public static ImageGunModel GetGunModel1() //Солнечная
        {
            ImageGunModel gun = new ImageGunModel();
            gun.GunImage = Links.Brushes.Guns.Gun1;
            gun.GunSize = new Size(52, 120);
            gun.ConnectionPoint = new Point(26, 89);
            gun.Geometry = "M{0},{1} l2,2 l1,30 l1.5,3 l1.5,-3 l1,-30 l2,-2" +
                "l4,3 v2 l4,3 v14 a15,20 0 0,0 4,15 v30 a10,10 0 0,0 6,10 v25 a5,7 0 0,1 -9,0 v8 l-7,4 l-2,5 h-8" +
                "l-2,-5 l-7,-4 v-8 a5,7 0 0,1 -9,0 v-25 a10,10 0 0,0 6,-10 v-30 a15,20 0 0,0 4,-15 v-14 l4,-3 v-2 z";
            gun.FirePoint = new Point(26, 0);
            gun.GeometryDelta = new Point(21, 4);
            return gun;
        }
        public static ImageGunModel GetGunModel2() //Плазма, Варп, дрон
        {
            ImageGunModel gun = new ImageGunModel();
            gun.GunImage = Links.Brushes.Guns.Gun2;
            gun.GunSize = new Size(55, 90);
            gun.ConnectionPoint = new Point(27, 71);
            gun.Geometry = "M{0},{1} h8 l4,11 h2 v11 h4 v22" +
                "h9 l4,5 v18 l-7,11 h-6 v12 h-27 v-12 h-6 l-7,-11 v-18 l4,-5 h9 v-22 h4 v-11 h3z";
            gun.FirePoint = new Point(27, 0);
            gun.GeometryDelta = new Point(23, 0);
            return gun;
        }
        public static ImageGunModel GetGunModel3() //Радиация
        {
            ImageGunModel gun = new ImageGunModel();
            gun.GunImage = Links.Brushes.Guns.Gun3;
            gun.GunSize = new Size(29, 120);
            gun.ConnectionPoint = new Point(14, 78);
            gun.Geometry = "M{0},{1} l7.5,1.5 v88 a7,6.5 0 0,1 5,11.5" +
                "a13.5,13.5 0 1,1 -25.5,0 a7,6.5 0 0,1 5,-11.5 v-88z";
            gun.FirePoint = new Point(14.5, 0);
            gun.GeometryDelta = new Point(14.5, 0);
            return gun;
        }
        public static ImageGunModel GetGunModel4() //Магнит
        {
            ImageGunModel gun = new ImageGunModel();
            gun.GunImage = Links.Brushes.Guns.Gun4;
            gun.GunSize = new Size(26, 120);
            gun.ConnectionPoint = new Point(13, 85);
            gun.Geometry = "M{0},{1} h11 l2,10 l-3,2.5" +
                "v4 l 4,8 v44 h-2 v5 l4,7 a20,30 0 0,1 -6,36 l-3,4 h-4 l-3,-4 a20,18 0 0,1 -6,-22"+
                "v -12 l5,-10 v-5 h-2 v-44 l4,-8 v-4 l-3,-2.5z";
            gun.FirePoint = new Point(13, 0);
            gun.GeometryDelta = new Point(8, 0);
            return gun;
        }
        public static ImageGunModel GetGunModel5() //Эми
        {
            ImageGunModel gun = new ImageGunModel();
            gun.GunImage = Links.Brushes.Guns.Gun5;
            gun.GunSize = new Size(30, 120);
            gun.ConnectionPoint = new Point(17.5, 85);
            gun.Geometry = "M{0},{1} a10,4 0 0,1 16,0 v3 l4,3" +
                "v20 l-6,3 v2 a8,10 0 0,1 2,20 v21 l5,8 l1.5,32 l-8,5 h-12 l-8,-5 l1.5,-32 l5,-8" +
                "v-21 a8,10 0 0,1 2,-20 v-2 l-6,-3 v-20 l3,-3z";
            gun.FirePoint = new Point(15, 0);
            gun.GeometryDelta = new Point(7, 3);
            return gun;
        }
        public static ImageGunModel GetGunModel6() //Гаусс
        {
            ImageGunModel gun = new ImageGunModel();
            gun.GunImage = Links.Brushes.Guns.Gun6;
            gun.GunSize = new Size(35, 120);
            gun.ConnectionPoint = new Point(22, 92);
            gun.Geometry = "M{0},{1} h20 v5 l5,4 v45 l8,8 l-4,3" +
                "l-6,-6 v15 l7,2 l-12,39 h-4 v4 h-8 v-7 l-9,-18 v-20 l1,-8 v-58 l2,-4z";
            gun.FirePoint = new Point(14, 0);
            gun.GeometryDelta = new Point(3, 0);
            return gun;
        }
        public static ImageGunModel GetGunModel7() //Ракеты
        {
            ImageGunModel gun = new ImageGunModel();
            gun.GunImage = Links.Brushes.Guns.Gun7;
            gun.GunSize = new Size(35, 120);
            gun.ConnectionPoint = new Point(13, 84);
            gun.Geometry = "M{0},{1} h7 l5,5 h1 l5,-5 h7 v5" +
                "l2,2 v20 l-4,12 v16 l3,1 l1,24 l-3,5 v17 l-3,2 l4,5 l-14,11 l-9,-4 l-2,-12 l-3,-2 l-3,-6 l-2,-15 v-7 l6,-8 v-60 l2,-3z";
            gun.FirePoint = new Point(20, 5);
            gun.GeometryDelta = new Point(8, 0);
            return gun;
        }
        public static ImageGunModel GetGunModel8() //Антиматерия
        {
            ImageGunModel gun = new ImageGunModel();
            gun.GunImage = Links.Brushes.Guns.Gun8;
            gun.GunSize = new Size(35, 120);
            gun.ConnectionPoint = new Point(17, 88);
            gun.Geometry = "M{0},{1} h2 l5,12 v10 h4 v-10 l5,-12" +
                "h2 v32 l7,5 v65 l-3,3 h-4 v5 l-4,3 v6 h-9 v-6 l-4,-3 v-5 h-5 l-3,-3 v-65 l7,-5z";
            gun.FirePoint = new Point(17, 10);
            gun.GeometryDelta = new Point(8, 0);
            return gun;
        }
        public static ImageGunModel GetGunModel9() //Орудие
        {
            ImageGunModel gun = new ImageGunModel();
            gun.GunImage = Links.Brushes.Guns.Gun9;
            gun.GunSize = new Size(48, 120);
            gun.ConnectionPoint = new Point(24, 95);
            gun.Geometry = "M{0},{1} h18 v57 h10 l4,6" +
                "v12 l-3,5 h-3 v18 l-4,7 h-2 v9 h-4 l-5,6 l-10,-3 l-10,-20 v-12 l-2,-3 v-15 l-4,-2 v-12 h15 z";
            gun.FirePoint = new Point(24, 0);
            gun.GeometryDelta = new Point(15, 0);
            return gun;
        }
        public static ImageGunModel GetGunModel10() //Дарк
        {
            ImageGunModel gun = new ImageGunModel();
            gun.GunImage = Links.Brushes.Guns.Gun10;
            gun.GunSize = new Size(42, 120);
            gun.ConnectionPoint = new Point(21, 92);
            gun.Geometry = "M{0},{1} l4,-4 h5 v-4 h4 v4 h5 l4,4" +
                "v51 l3,7 v15 h-4 v7 h5 l6,9 v10 l-17,13 h-8 l-17,-13 v-10 l6,-9 h5 v-7 h-4 v-15 l3,-7z";
            gun.FirePoint = new Point(21, 0);
            gun.GeometryDelta = new Point(10, 8);
            return gun;
        }
        public static ImageGunModel GetGunModel11() //Слайс
        {
            ImageGunModel gun = new ImageGunModel();
            gun.GunImage = Links.Brushes.Guns.Gun11;
            gun.GunSize = new Size(42, 120);
            gun.ConnectionPoint = new Point(21, 92);
            gun.Geometry = "M{0},{1} h8 v8 h3 l8,16 v10 l2,3 v13" +
                "l3,3 v9 h-3 v20 l6,8 v20 h-5 l-2,8 h-24 l-1,-8 h-5 v-16 a12,12 0 0,1 5,-20 v-2 a6,6 0 0,1 2,-11"+
                "v-3 h-2 v-47 h4 v-8 h1 z";
            gun.FirePoint = new Point(21, 0);
            gun.GeometryDelta = new Point(14, 0);
            return gun;
        }
        public static ImageGunModel GetGunModel12() //Лазер
        {
            ImageGunModel gun = new ImageGunModel();
            gun.GunImage = Links.Brushes.Guns.Gun12;
            gun.GunSize = new Size(26, 120);
            gun.ConnectionPoint = new Point(13, 85);
            gun.Geometry = "M{0},{1} h11 l2,10 l-3,2.5" +
                "v4 l 4,8 v44 h-2 v5 l4,7 a20,30 0 0,1 -6,36 l-3,4 h-4 l-3,-4 a20,18 0 0,1 -6,-22" +
                "v -12 l5,-10 v-5 h-2 v-44 l4,-8 v-4 l-3,-2.5z";
            gun.FirePoint = new Point(13, 0);
            gun.GeometryDelta = new Point(8, 0);
            return gun;
        }
        public static ImageGunModel GetGunModel13() //Время
        {
            ImageGunModel gun = new ImageGunModel();
            gun.GunImage = Links.Brushes.Guns.Gun13;
            gun.GunSize = new Size(26, 120);
            gun.ConnectionPoint = new Point(13, 85);
            gun.Geometry = "M{0},{1} h11 l2,10 l-3,2.5" +
                "v4 l 4,8 v44 h-2 v5 l4,7 a20,30 0 0,1 -6,36 l-3,4 h-4 l-3,-4 a20,18 0 0,1 -6,-22" +
                "v -12 l5,-10 v-5 h-2 v-44 l4,-8 v-4 l-3,-2.5z";
            gun.FirePoint = new Point(13, 0);
            gun.GeometryDelta = new Point(8, 0);
            return gun;
        }
        public static ImageGunModel GetGunModel14() //Пси
        {
            ImageGunModel gun = new ImageGunModel();
            gun.GunImage = Links.Brushes.Guns.Gun14;
            gun.GunSize = new Size(26, 120);
            gun.ConnectionPoint = new Point(13, 85);
            gun.Geometry = "M{0},{1} h11 l2,10 l-3,2.5" +
                "v4 l 4,8 v44 h-2 v5 l4,7 a20,30 0 0,1 -6,36 l-3,4 h-4 l-3,-4 a20,18 0 0,1 -6,-22" +
                "v -12 l5,-10 v-5 h-2 v-44 l4,-8 v-4 l-3,-2.5z";
            gun.FirePoint = new Point(13, 0);
            gun.GeometryDelta = new Point(8, 0);
            return gun;
        }
        public static ImageGunModel GetGunModelBig2()
        {
            ImageGunModel gun = new ImageGunModel();
            gun.GunImage = Links.Brushes.Guns.Gun2;
            gun.GunSize = new Size(110, 180);
            gun.ConnectionPoint = new Point(55, 135);
            gun.Geometry = "M{0},{1} h15 l5,20 l6,3 l3,20" +
                " l5,2 v45 l3,-4 h13 l9,11 v32 h-6 v12 l-7,11 h-7 l-5,-8 v30 h-7 l-3,-2 h-31 l-3,2" +
                "h-7 v-30 l-5,8 h-7 l-7,-11 v-12 h-6 v-32 l9,-11 h13 l3,4 v-45 l5,-2 l3,-20 l6,-3z";
            gun.FirePoint = new Point(55, 0);
            gun.GeometryDelta = new Point(49, 2);
            return gun;
        }

    }
    
}
