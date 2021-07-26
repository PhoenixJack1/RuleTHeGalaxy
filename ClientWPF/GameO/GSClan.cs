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
using System.Windows.Shapes;

namespace Client
{
    class GSClan
    {
        public int ID;
        public GSString Name;
        public long Image;
        public bool NoClan = false;
        public bool IsClanLeader = false;
        public GSClan(byte[] array, ref int i)
        {

            ID = BitConverter.ToInt32(array, i); i += 4;
            if (ID == -1) { NoClan = true; return; }
            Image = BitConverter.ToInt64(array, i); i += 8;
            Name = new GSString(array, i); i += Name.Array.Length; 
        }
    }
    enum EColor { red, green, blue };
    public class ClanEmblem : Canvas
    {
        Path MainPath;
        Path LinesPath;
        Path CenterPath;
        SolidColorBrush MainBrush;
        SolidColorBrush LineBrush;
        Brush CenterBrush;
        public static byte[] ColorValues = new byte[] { 0, 85, 170, 255 };
        public static Color[] RG = InnerColors1();
        public static PathGeometry[] Lines = CreateLines();
        public static PathGeometry[] Centers = CreateCenters();
        public static Brush[] CenterBrushes = CreateCenterBrushes();
        public long Image;
        public ClanEmblem(long emblem)
        {
            Width = 70;
            Height = 100;
            MainPath = new Path();
            MainPath.Stroke = Brushes.White;
            MainPath.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h70 v65 l-35,35 l-35,-35z"));
            Children.Add(MainPath);
            MainBrush = new SolidColorBrush();
            MainPath.Fill = MainBrush;

            LineBrush = new SolidColorBrush();
            LinesPath = new Path();
            LinesPath.Fill = LineBrush;
            Children.Add(LinesPath);

            CenterPath = new Path();
            CenterPath.StrokeLineJoin = PenLineJoin.Round;
            CenterPath.Stroke = Brushes.Black;
            Children.Add(CenterPath);

            CenterBrush = new RadialGradientBrush();
            CenterPath.Fill = CenterBrush;
            Image = emblem;
            SetImage(BitConverter.GetBytes(Image));
        }
        public Viewbox GetSmallEmblem(int height)
        {
            Viewbox box = new Viewbox();
            box.Height = height;
            box.Width = height * 0.7;
            box.Child = this;
            return box;
        }
        public static Color[] InnerColors1()
        {
            Color[, ,] RGB = new Color[4, 4, 4];
            RG = new Color[64];
            RG[0]=RGB[0, 0, 0] = Color.FromRgb(255, 255, 255); //белый
            RG[1]=RGB[1, 1, 1] = Color.FromRgb(170, 170, 170); //светлосерый
            RG[2]=RGB[2, 2, 2] = Color.FromRgb(85, 85, 85); //тёмносерый

            RG[3]=RGB[3, 3, 3] = Color.FromRgb(0, 0, 0); //чёрный
            RG[4]=RGB[0, 0, 1] = Color.FromRgb(170, 170, 255); //синий
            RG[5]=RGB[0, 0, 2] = Color.FromRgb(85, 85, 255); //
            RG[6]=RGB[0, 0, 3] = Color.FromRgb(0, 0, 255);
            RG[7]=RGB[1, 1, 2] = Color.FromRgb(0, 0, 212);
            RG[8]=RGB[1, 1, 3] = Color.FromRgb(0, 0, 170);
            RG[9]=RGB[2, 2, 3] = Color.FromRgb(0, 0, 85);

            RG[10]=RGB[0, 1, 0] = Color.FromRgb(170, 255, 170); //зелёный
            RG[11]=RGB[0, 2, 0] = Color.FromRgb(85, 255, 85);
            RG[12]=RGB[0, 3, 0] = Color.FromRgb(0, 255, 0);
            RG[13]=RGB[1, 2, 1] = Color.FromRgb(0, 212, 0);
            RG[14]=RGB[1, 3, 1] = Color.FromRgb(0, 170, 0);
            RG[15]=RGB[2, 3, 2] = Color.FromRgb(0, 85, 0);

            RG[16]=RGB[1, 0, 0] = Color.FromRgb(255, 170, 170); //красный
            RG[17]=RGB[2, 0, 0] = Color.FromRgb(255, 85, 85);
            RG[18]=RGB[3, 0, 0] = Color.FromRgb(255, 0, 0);
            RG[19]=RGB[2, 1, 1] = Color.FromRgb(212, 0, 0);
            RG[20]=RGB[3, 1, 1] = Color.FromRgb(170, 0, 0);
            RG[21]=RGB[3, 2, 2] = Color.FromRgb(85, 0, 0);

            RG[22]=RGB[1, 0, 1] = Color.FromRgb(255, 170, 255); //розовый
            RG[23]=RGB[2, 0, 2] = Color.FromRgb(255, 85, 255);
            RG[24]=RGB[2, 1, 2] = Color.FromRgb(255, 42, 255);
            RG[25]=RGB[3, 0, 3] = Color.FromRgb(255, 0, 255);
            RG[26]=RGB[3, 1, 3] = Color.FromRgb(170, 0, 170);
            RG[27]=RGB[3, 2, 3] = Color.FromRgb(85, 0, 85);

            RG[28]=RGB[1, 1, 0] = Color.FromRgb(255, 255, 170); //жёлтый
            RG[29]=RGB[2, 2, 0] = Color.FromRgb(255, 255, 85);
            RG[30]=RGB[2, 2, 1] = Color.FromRgb(255, 255, 42);
            RG[31]=RGB[3, 3, 0] = Color.FromRgb(255, 255, 0);
            RG[32]=RGB[3, 3, 1] = Color.FromRgb(170, 170, 0);
            RG[33]=RGB[3, 3, 2] = Color.FromRgb(85, 85, 0);

            RG[34]=RGB[0, 1, 1] = Color.FromRgb(170, 255, 255); //голубой
            RG[35]=RGB[0, 2, 2] = Color.FromRgb(85, 255, 255);
            RG[36]=RGB[1, 2, 2] = Color.FromRgb(42, 255, 255);
            RG[37]=RGB[0, 3, 3] = Color.FromRgb(0, 255, 255);
            RG[38]=RGB[1, 3, 3] = Color.FromRgb(0, 170, 170);
            RG[39]=RGB[2, 3, 3] = Color.FromRgb(0, 85, 85);

            RG[40]=RGB[0, 1, 2] = Color.FromRgb(85, 170, 255); //грязносиние цвета
            RG[41] = RGB[0, 1, 3] = Color.FromRgb(0, 170, 255);
            RG[42] = RGB[0, 2, 3] = Color.FromRgb(0, 85, 255);
            RG[43] = RGB[1, 2, 3] = Color.FromRgb(0, 85, 170);

            RG[44]=RGB[0, 2, 1] = Color.FromRgb(85, 255, 170); //синезелёные цвета
            RG[45] = RGB[0, 3, 1] = Color.FromRgb(0, 255, 170);
            RG[46] = RGB[0, 3, 2] = Color.FromRgb(0, 255, 85);
            RG[47] = RGB[1, 3, 2] = Color.FromRgb(0, 170, 85);

            RG[48]=RGB[1, 0, 2] = Color.FromRgb(170, 85, 255); //фиолетовые цвета
            RG[49] = RGB[1, 0, 3] = Color.FromRgb(170, 0, 255);
            RG[50] = RGB[2, 0, 3] = Color.FromRgb(85, 0, 255);
            RG[51] = RGB[2, 1, 3] = Color.FromRgb(85, 0, 170);

            RG[52]=RGB[2, 0, 1] = Color.FromRgb(255, 85, 170); //розовые цвета
            RG[53] = RGB[3, 0, 1] = Color.FromRgb(255, 0, 170);
            RG[54] = RGB[3, 0, 2] = Color.FromRgb(255, 0, 85);
            RG[55] = RGB[3, 1, 2] = Color.FromRgb(170, 0, 85);

            RG[56]=RGB[1, 2, 0] = Color.FromRgb(170, 255, 85); //жёлтозелёные цвета
            RG[57] = RGB[1, 3, 0] = Color.FromRgb(170, 255, 0);
            RG[58] = RGB[2, 3, 0] = Color.FromRgb(85, 255, 0);
            RG[59] = RGB[2, 3, 1] = Color.FromRgb(85, 170, 0);

            RG[60]=RGB[2, 1, 0] = Color.FromRgb(255, 170, 85); //коричневые цвета
            RG[61]=RGB[3, 1, 0] = Color.FromRgb(255, 170, 0);
            RG[62]=RGB[3, 2, 0] = Color.FromRgb(255, 85, 0);
            RG[63]=RGB[3, 2, 1] = Color.FromRgb(170, 85, 0);

            //RG = new Color[64];
            //int z = 0;
            //foreach (Color color in RGB)
           // {
            //    RG[z] = color; z++;
           // }
            return RG;
        }
        public void SetImage(byte[] list)
        {
            Image = BitConverter.ToInt64(list, 0);
            MainBrush.Color = RG[list[0]];
            LinesPath.Data = Lines[list[1]];
            LineBrush.Color = RG[list[2]];
            CenterPath.Data = Centers[list[3]];
            CenterPath.Fill = CenterBrushes[list[4]];
        }
        static Brush[] CreateCenterBrushes()
        {
            List<Brush> list = new List<Brush>();
            list.Add(new RadialGradientBrush());
            list.Add(GetRadialBrush(Colors.Gray, Colors.White, 0.7, 0.3));
            list.Add(GetRadialBrush(Colors.Red, Colors.White, 0.7, 0.3));
            list.Add(GetRadialBrush(Colors.Blue, Colors.White, 0.7, 0.3));
            list.Add(GetRadialBrush(Colors.Green, Colors.White, 0.7, 0.3));
            list.Add(GetRadialBrush(Colors.Yellow, Colors.White, 0.7, 0.3));
            list.Add(GetRadialBrush(Colors.SkyBlue, Colors.White, 0.7, 0.3));
            list.Add(GetRadialBrush(Colors.Purple, Colors.White, 0.7, 0.3));
            list.Add(GetRadialBrush(Colors.Black, Colors.White, 0.7, 0.3));
            list.Add(GetRadialBrush(Colors.DarkBlue, Colors.White, 0.7, 0.3));
            list.Add(GetRadialBrush(Colors.DarkRed, Colors.White, 0.7, 0.3));
            list.Add(GetRadialBrush(Colors.DarkGreen, Colors.White, 0.7, 0.3));
            list.Add(GetLinearBrush(Colors.Gray, Colors.White));
            list.Add(GetLinearBrush(Colors.Red, Colors.White));
            list.Add(GetLinearBrush(Colors.Blue, Colors.White));
            list.Add(GetLinearBrush(Colors.Green, Colors.White));
            list.Add(GetLinearBrush(Colors.Yellow, Colors.White));
            list.Add(GetLinearBrush(Colors.SkyBlue, Colors.White));
            list.Add(GetLinearBrush(Colors.Purple, Colors.White));
            list.Add(GetLinearBrush(Colors.Black, Colors.White));
            list.Add(GetLinearBrush(Colors.DarkBlue, Colors.White));
            list.Add(GetLinearBrush(Colors.DarkRed, Colors.White));
            list.Add(GetLinearBrush(Colors.DarkGreen, Colors.White));
            list.Add(new SolidColorBrush(Colors.Gray));
            list.Add(new SolidColorBrush(Colors.Red));
            list.Add(new SolidColorBrush(Colors.Blue));
            list.Add(new SolidColorBrush(Colors.Green));
            list.Add(new SolidColorBrush(Colors.Yellow));
            list.Add(new SolidColorBrush(Colors.SkyBlue));
            list.Add(new SolidColorBrush(Colors.Purple));
            list.Add(new SolidColorBrush(Colors.Black));
            list.Add(new SolidColorBrush(Colors.DarkBlue));
            list.Add(new SolidColorBrush(Colors.DarkRed));
            list.Add(new SolidColorBrush(Colors.DarkGreen));
            return list.ToArray();
        }
        static Brush GetLinearBrush(Color outcolor, Color incolor)
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(outcolor, 1));
            brush.GradientStops.Add(new GradientStop(incolor, 0.5));
            brush.GradientStops.Add(new GradientStop(outcolor, 0));
            return brush;
        }
        static Brush GetRadialBrush(Color outcolor, Color incolor, double offsetx, double offsety)
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(offsetx, offsety);
            brush.GradientStops.Add(new GradientStop(incolor, 0));
            brush.GradientStops.Add(new GradientStop(outcolor, 1));
            return brush;
        }
        static PathGeometry[] CreateCenters()
        {
            List<PathGeometry> list = new List<PathGeometry>();
            list.Add(GetGeom(""));
            list.Add(GetGeom("M25,10 h20 v20 h20 v20 h-20 v20 h-20 v-20 h-20 v-20 h20z")); //крест большой
            list.Add(GetGeom("M30,15 h10 v20 h20 v10 h-20 v20 h-10 v-20 h-20 v-10 h20z")); //крест малый
            list.Add(GetGeom("M20,10 a20,10 0 0,1 30,0 l-10,20 l20,-10 a10,20 0 0,1 0,30 l-20,-10 l10,20 a20,10 0 0,1 -30,0 l10,-20 l-20,10 a10,20 0 0,1 0,-30 l20,10z")); //орден
            list.Add(GetGeom("M20,10 a20,10 0 0,0 30,0 l-10,20 l20,-10 a10,20 0 0,0 0,30 l-20,-10 l10,20 a20,10 0 0,0 -30,0 l10,-20 l-20,10 a10,20 0 0,0 0,-30 l20,10z")); //мальтийский крест
            list.Add(GetGeom("M20,20 a25,25 0 1,1 0,40 a21,21 0 1,0 0,-40")); //полумесяц левый
            list.Add(GetGeom("M50,20 a25,25 0 1,0 0,40 a21,21 0 1,1 0,-40")); //полумесяц правый
            list.Add(GetGeom("M35,10 l8,18 l20,10 l-15,10 l5,16 l-18,-9 l-18,9 l5,-16 l-15,-10 l20,-10z"));//звезда 1
            list.Add(GetGeom("M35,10 l8,18 l20,0 l-15,15 l7,20 l-20,-11 l-20,11 l7,-20 l-15,-15 l20,-0z")); //звезда 2
            list.Add(GetGeom("M15,40 a20,20 0 1,1 0,0.1")); //круг
            list.Add(GetGeom("M20,30 a15,10 0 1,1 0,0.1 M20,30 v5 a15,10 0 1,0 30,0 v-5 a15,10 0 1,1 -30,0" +
                  "M20,35 v5 a15,10 0 1,0 30,0 v-5 a15,10 0 1,1 -30,0 M20,40 v5 a15,10 0 1,0 30,0 v-5 a15,10 0 1,1 -30,0" +
                  "M20,45 v5 a15,10 0 1,0 30,0 v-5 a15,10 0 1,1 -30,0 M20,50 v5 a15,10 0 1,0 30,0 v-5 a15,10 0 1,1 -30,0")); //монеты
            list.Add(GetGeom("M32,65 h6 v-10 a10,10 0 0,0 15,-10 a20,40 0 0,0 -8,-17 l-10,-12 l-10,12 a20,40 0 0,0 -8,17 a10,10 0 0,0 15,10z")); //лист
            list.Add(GetGeom("M35,25 a25,25 0 0,1 25,10 a10,10 0 0,0 -15,0 a25,25 0 0,1 -10,25 a10,10 0 0,0 0,-15"+ 
            "a25,25 0 0,1 -25,-10 a10,10 0 0,0 15,0 a25,25 0 0,1 10,-25 a10,10 0 0,0 0,15")); //варп
            list.Add(GetGeom("M20,60 a4,4 0 1,0 3,3 a5,5 0 0,0 6,-6 a5,5 0 0,0 6,-6 l5,5 a4,4 0 1,0 4,-4 l-4,-4 a35,35 0 0,0 15,-45"+
                  "a40,40 0 0,1 -20,40 l-4,-4 a4,4 0 1,0 -4,4 l5,5 a5,5 0 0,0 -6,6 a5,5 0 0,0 -6,6")); //нож
            list.Add(GetGeom("M35,15 a15,20 0 0,1 5,20 a15,15 0 0,0 10,-15 a15,15 0 0,1 0,20 a10,10 0 0,1 -0,15"+ 
                  "a25,25 0 0,1 -30,0 a10,10 0 0,1 0,-15 a15,15 0 0,1 5,-20 a15,15 0 0,0 5,15 a10,20 0 0,0 5,-20")); //типа пламя
            list.Add(GetGeom("M35,15 a35,40 0 0,1 15,30 a10,10 0 1,1 -30,0 a35,40 0 0,1 15,-30")); //капля
            return list.ToArray();
        }
        static PathGeometry[] CreateLines()
        {
            List<PathGeometry> list = new List<PathGeometry>();
            list.Add(GetGeom(""));
            list.Add(GetGeom("M0,40 h70 v20 h-70z")); //одна линия
            list.Add(GetGeom("M0,20 h70 v15 h-70z M0,50 h70 v15 h-70z")); //две линии
            list.Add(GetGeom("M0,10 h70 v15 h-70z M0,35 h70 v15 h-70z M0,60 h70 v5 l-10,10 h-50 l-10,-10z")); //три толстых линии
            list.Add(GetGeom("M0,10 h70 v10 h-70z M0,35 h70 v10 h-70z M0,60 h70 v5 l-5,5 h-60 l-5,-5z")); //три тонких линии
            list.Add(GetGeom("M0,5 h70 v5 h-70z")); //одна линия сверху
            list.Add(GetGeom("M0,5 h70 v5 h-70z M0,15 h70 v5 h-70z")); //две линии сверху
            list.Add(GetGeom("M0,5 h70 v5 h-70z M0,15 h70 v5 h-70z M0,25 h70 v5 h-70z")); //три линии сверху
            list.Add(GetGeom("M0,5 h70 v5 h-70z M0,15 h70 v5 h-70z M0,25 h70 v5 h-70z M0,35 h70 v5 h-70z")); //четыре линии сверху
            list.Add(GetGeom("M0,5 h70 v5 h-70z M0,15 h70 v5 h-70z M0,25 h70 v5 h-70z M0,35 h70 v5 h-70z M0,45 h70 v5 h-70z")); //пять линий сверху
            list.Add(GetGeom("M0,5 h70 v5 h-70z M0,15 h70 v5 h-70z M0,25 h70 v5 h-70z M0,35 h70 v5 h-70z M0,45 h70 v5 h-70z M0,55 h70 v5 h-70z")); //шесть линий сверху
            list.Add(GetGeom("M25,0 v90 l10,10 l10,-10 v-90z")); //одна вертикальная линия
            list.Add(GetGeom("M20,0 v85 l10,10 v-95z M40,0 v95 l10,-10 v-85z")); //две вертикальные линии
            list.Add(GetGeom("M10,0 v75 l10,10 v-85z M30,0 v95 l5,5 l5,-5 v-95z M50,0 v85 l10,-10 v-75z")); //три вертикальные линии
            list.Add(GetGeom("M5,0v70l7.5,7.5v-77.5z M17.5,0v82.5l7.5,7.5v-90z M30,0v95l5,5l5,-5v-95z" +
                  "M45,0v90l7.5,-7.5v-82.5z M57.5,0 v77.5l7.5,-7.5v-70z")); //пять вертикальных линий
            list.Add(GetGeom("M2.5,0v67.5l5,5v-72.5z M12.5,0v77.5l5,5v-82.5z M22.5,0v87.5l5,5v-92.5z M32.5,0v97.5l2.5,2.5l2.5,-2.5v-97.5z" +
                  "M42.5,0 v92.5 l5,-5 v-87.5z M52.5,0 v82.5l5,-5v-77.5z M62.5,0v72.5l5,-5v-67.5")); //семь вертикальных линий
            list.Add(GetGeom("M10,0 l60,60 v5 l-7.5,7.5 l-62.5,-62.5 v-10z")); //одна диагональная линия
            list.Add(GetGeom("M30,0l40,40v15l-55,-55h-10z M0,10l62.5,62.5l-7.5,7.5l-55,-55z")); //две диагональные линии
            list.Add(GetGeom("M45,0l25,25v15l-40,-40h-10z M10,0l60,60v5l-5,5l-65,-65v-5z M0,25l55,55l-7.5,7.5l-47.5,-47.5z")); //три диагональные линии
            list.Add(GetGeom("M45,0l25,25v10l-35,-35h-10z M20,0l50,50v10l-60,-60z M0,5l65,65l-5,5l-60,-60z M0,30l52.5,52.5l-5,5 l-47.5,-47.5z")); //четыре диагональные линии
            list.Add(GetGeom("M60,0 l-60,60 v5 l7.5,7.5 l62.5,-62.5 v-10z")); //одна контрдиагональная линия
            list.Add(GetGeom("M40,0l-40,40v15l55,-55h-10z M70,10l-62.5,62.5l7.5,7.5l55,-55z")); //две контрдиагональные линии
            list.Add(GetGeom("M25,0l-25,25v15l40,-40h-10z M60,0l-60,60v5l5,5l65,-65v-5z M70,25l-55,55l7.5,7.5l47.5,-47.5z")); //три контрдиагональные линии
            list.Add(GetGeom("M25,0l-25,25v10l35,-35h-10z M50,0l-50,50v10l60,-60z M70,5l-65,65l5,5l60,-60z M70,30l-52.5,52.5l5,5 l47.5,-47.5z")); //четыре контрдиагональные линии
            list.Add(GetGeom("M60,0l-60,60v5l5,5l65,-65v-5z M10,0 l25,25 l-7.5,7.5 l-27.5,-27.5v-5z M42.5,32.5 l27.5,27.5 v5 l-5,5 l-30,-30")); //крест
            list.Add(GetGeom("M0,0 h35 v35 h-35z M35,35 h35v30l-5,5 h-30z")); //шахматка 2х2
            list.Add(GetGeom("M0,0 h25 v25 h-25z M25,25 h20 v20 h-20z M45,0 h25v25h-25" +
                  "M0,45 h25 v25h-20 l-5,-5z M45,45 h25 v20 l-5,5 h-20z M25,70 h20 v20h-20z")); //шахматка 3х3
            list.Add(GetGeom("M10,20 a10,10 0 1,1 0.0,1 M40,20 a10,10 0 1,1 0.0,1" +
                  "M10,50 a10,10 0 1,1 0.0,1 M40,50 a10,10 0 1,1 0.0,1 M25,80 a10,10 0 1,1 0.0,1")); //круги х5
            list.Add(GetGeom("M0,65 h70 l-35,35z")); //уголок низ
            list.Add(GetGeom("M0,0 h35 l-35,35z")); //уголок левый верхний
            list.Add(GetGeom("M35,0 l35,35 v-35z")); //уголок правый верхний

            return list.ToArray();
        }
        static PathGeometry GetGeom(string str)
        {
            return new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(str));
        }
    }
   
   
}
