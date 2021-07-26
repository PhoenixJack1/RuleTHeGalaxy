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
    class ShipModelNew
    {
        static PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
        public static SortedList<byte, ShipModelNew> Models = GetModels();
        public Point MiddleGun;
        public Point LeftGun;
        public Point RightGun;
        public Point MiddleGunCenter;
        public Point LeftGunCenter;
        public Point RightGunCenter;
        public Point CenterShip;
        public Point HealthPoint;
        public Point ShieldPoint;
        public Point EnergyPoint;
        public Point LeftStatusPoint;
        public Point RightStatusPoint;
        public Point IDStatusPoint;
        public Point CocpitPoint;
        public string ShipFigure;
        public string LeftGunString;
        public string MiddleGunString;
        public string RightGunString;
        public string BeltString;
        public double StarRadiusX;
        public double StarRadiusY;
        public static SortedList<int, Brush[]> NewBrushes = GetNewBrushes();
        static SortedList<int, Brush[]> GetNewBrushes()
        {
            SortedList<int, Brush[]> list = new SortedList<int, Brush[]>();
            list.Add(0, new Brush[] { Brushes.Red, new SolidColorBrush(Color.FromRgb(255, 100, 100)) });
            list.Add(1, new Brush[] { Brushes.Navy, Brushes.SkyBlue });
            list.Add(2, new Brush[] { Brushes.LightBlue, Brushes.SkyBlue });
            list.Add(3, new Brush[] { Brushes.Gold, Brushes.LightYellow });
            list.Add(4, new Brush[] { Brushes.Violet, Brushes.Pink });
            list.Add(5, new Brush[] { Brushes.Green, new SolidColorBrush(Color.FromRgb(100, 255, 100)) });
            list.Add(6, new Brush[] { Brushes.Brown, Brushes.RosyBrown });
            return list;
        }
        static ShipModelNew GetModel15()
        {
            ShipModelNew model = new ShipModelNew();
            model.MiddleGun = new Point(135, 24);
            model.LeftGun = new Point(85, 77);
            model.RightGun = new Point(185, 77);
            model.MiddleGunCenter = new Point(150, 23);
            model.LeftGunCenter = new Point(100, 76);
            model.RightGunCenter = new Point(200, 76);
            model.CenterShip = new Point(150, 130);
            model.HealthPoint = new Point(100, 165);
            model.ShieldPoint = new Point(170, 165);
            model.EnergyPoint = new Point(135, 165);
            model.LeftStatusPoint = new Point(52, 115);
            model.RightStatusPoint = new Point(218, 115);
            model.IDStatusPoint = new Point(135, 200);
            model.CocpitPoint = new Point(125, 130);
            model.ShipFigure = "M70,50 a100,140 0 0,0 20,190 l10,-30 l20,10 l-20,30 a100,100 0 0,1 100,0 l-20,-30 l20,-10 l10,30 a100,140 0 0,0 20,-190 l-10,60 l-40,30 l-30,-80 l-30,80 l-40,-30z";
            model.LeftGunString = "M90,108 v9.5 l20,15 v-24.5 h6v-32h-32v32z";
            model.RightGunString = "M210,108 v9.5 l-20,15 v-24.5 h-6v-32h32v32z";
            model.MiddleGunString = "M140,55 v32 l10,-27 l10,27 v-32 h6v-32h-32v32z";
            model.BeltString = "M150,70 l20,55 h-40z M85,125 l30,25 l-40,50z M215,125 l-30,25 l40,50z";
            model.StarRadiusX = 240;
            model.StarRadiusY = 60;
            return model;
        }
        static ShipModelNew GetModel14()
        {
            ShipModelNew model = new ShipModelNew();
            model.MiddleGun = new Point(135, 19);
            model.LeftGun = new Point(55, 79);
            model.RightGun = new Point(215, 79);
            model.MiddleGunCenter = new Point(150, 18);
            model.LeftGunCenter = new Point(70, 78);
            model.RightGunCenter = new Point(230, 78);
            model.CenterShip = new Point(150, 130);
            model.HealthPoint = new Point(115, 110);
            model.ShieldPoint = new Point(155, 110);
            model.EnergyPoint = new Point(135, 145);
            model.LeftStatusPoint = new Point(70, 135);
            model.RightStatusPoint = new Point(200, 135);
            model.IDStatusPoint = new Point(135, 180);
            model.CocpitPoint = new Point(125, 75);
            model.ShipFigure = "M40,180 a40,40 0 0,1 20,-50 h20 a40,40 0 0,0 40,-40 v-10 a20,20 0 0,1 20,-20 h20 a20,20 0 0,1 20,20 v10 a40,40 0 0,0 40,40h20 a40,40 0 0,1 20,50 a40,100 0 0,1 -30,50 a40,40 0 0,0 -70,0 h-20 a40,40 0 0,0 -70,0 a40,100 0 0,1 -30,-50";
            model.LeftGunString = "M60,110 v20 h20 v-20h6v-32h-32v32z";
            model.RightGunString = "M220,110 v20 h20 v-20h6v-32h-32v32z";
            model.MiddleGunString = "M140,50 v10 h20 v-10h6v-32h-32v32z";
            model.BeltString = "M60,130 a100,100 0 0,0 24,85 a40,40 0 0,0 -14,15 a40,100 0 0,1 -30,-50 a40,40 0 0,1 20,-50 M240,130 a100,100 0 0,1 -24,85 a40,34 0 0,1 14,15 a40,100 0 0,0 30,-50 a40,40 0 0,0 -20,-50";
            model.StarRadiusX = 240;
            model.StarRadiusY = 60;
            return model;
        }
        static ShipModelNew GetModel13()
        {
            ShipModelNew model = new ShipModelNew();
            model.MiddleGun = new Point(135, 69);
            model.LeftGun = new Point(55, 49);
            model.RightGun = new Point(215, 49);
            model.MiddleGunCenter = new Point(150, 68);
            model.LeftGunCenter = new Point(70, 48);
            model.RightGunCenter = new Point(230, 48);
            model.CenterShip = new Point(150, 130);
            model.HealthPoint = new Point(80, 110);
            model.ShieldPoint = new Point(190, 110);
            model.EnergyPoint = new Point(85, 145);
            model.LeftStatusPoint = new Point(30, 120);
            model.RightStatusPoint = new Point(240, 120);
            model.IDStatusPoint = new Point(185, 145);
            model.CocpitPoint = new Point(125, 125);
            model.ShipFigure = "M20,130 l20,-40 h60 l20,30 h60 l20,-30h60l20,40 l-30,60 h-40 l-30,-20 h-60 l-30,20 h-40z";
            model.LeftGunString = "M60,80 v10 h20 v-10h6v-32h-32v32z";
            model.RightGunString = "M220,80 v10 h20 v-10h6v-32h-32v32z";
            model.MiddleGunString = " M140,100 v20 h20 v-20h6v-32h-32v32z";
            model.BeltString = "M45,95 h40 l5,5 l-10,5 l-10,35 l10,35 l10,5 l-5,5 h-30 l-5,-5 l10,-5 l5,-35 l-5,-25 l-10,-10 l-10,-5 l5,-5"+
              "M255,95 h-40 l-5,5 l10,5 l10,35 l-10,35 l-10,5 l5,5 h30 l5,-5 l-10,-5 l-5,-35 l5,-25 l10,-10 l10,-5 l-5,-5";
            model.StarRadiusX = 240;
            model.StarRadiusY = 60;
            return model;
        }
        static ShipModelNew GetModel12()
        {
            ShipModelNew model = new ShipModelNew();
            model.MiddleGun = new Point(135, 9);
            model.LeftGun = new Point(75, 19);
            model.RightGun = new Point(195, 19);
            model.MiddleGunCenter = new Point(150, 8);
            model.LeftGunCenter = new Point(90, 18);
            model.RightGunCenter = new Point(210, 18);
            model.CenterShip = new Point(150, 130);
            model.HealthPoint = new Point(90, 130);
            model.ShieldPoint = new Point(180, 130);
            model.EnergyPoint = new Point(135, 155);
            model.LeftStatusPoint = new Point(55, 140);
            model.RightStatusPoint = new Point(215, 140);
            model.IDStatusPoint = new Point(135, 220);
            model.CocpitPoint = new Point(125, 120);
            model.ShipFigure = "M150,50 a100,100 0 1,0 0.1,0z M90,110 a70,70 0 0,1 120,0 a30,30 0 0,1 -40,0 a30,30 0 0,0 -40,0 a30,30 0 0,1 -40,0 M90,185 a70,70 0 0,0 120,0 a30,30 0 0,0 -40,0 a30,30 0 0,1 -40,0 a30,30 0 0,0 -40,0";
            model.LeftGunString = "M80,50 v28.5 a100,100 0 0,1 20,-15 v-13.5 h6v-32h-32v32z";
            model.RightGunString = "M220,50 v28.5 a100,100 0 0,0 -20,-15 v-13.5 h-6v-32h32v32z";
            model.MiddleGunString = "M140,40 v10.5 a100,100 0 0,1 20,0 v-10.5 h6v-32h-32v32z";
            model.BeltString = "M75,95 a90,90 0 0,1 150,0 a90,50 0 0,0 -150,0 M65,190 a90,90 0 0,0 55,50 a110,0 0 0,1 -55,-50 M235,190 a90,90 0 0,1 -55,50 a110,0 0 0,1 55,-50";
            model.StarRadiusX = 240;
            model.StarRadiusY = 60;
            return model;
        }
        static ShipModelNew GetModel11()
        {
            ShipModelNew model = new ShipModelNew();
            model.MiddleGun = new Point(135, 69);
            model.LeftGun = new Point(75, 28);
            model.RightGun = new Point(195, 28);
            model.MiddleGunCenter = new Point(150, 67);
            model.LeftGunCenter = new Point(90, 26);
            model.RightGunCenter = new Point(210, 26);
            model.CenterShip = new Point(150, 130);
            model.HealthPoint = new Point(75, 100);
            model.ShieldPoint = new Point(195, 100);
            model.EnergyPoint = new Point(180, 150);
            model.LeftStatusPoint = new Point(55, 130);
            model.RightStatusPoint = new Point(215, 130);
            model.IDStatusPoint = new Point(90, 150);
            model.CocpitPoint = new Point(125, 155);
            model.ShipFigure = "M110,100 a40,40 0 0,0 80,0 a20,30 0 0,1 40,0 a20,20 0 0,0 20,20 a70,70 0 0,1 -20,110 a120,120 0 0,0 -160,0 a70,70 0 0,1 -20,-110 a20,20 0 0,0 20,-20 a20,30 0 0,1 40,0";
            model.LeftGunString = "M80,59 v15 a20,30 0 0,1 20,0 v-15 h6v-32h-32v32z";
            model.RightGunString = "M200,59 v15 a20,30 0 0,1 20,0 v-15 h6v-32h-32v32z";
            model.MiddleGunString = "M140,100 v39 a40,40 0 0,0 20,0 v-39 h6v-32h-32v32z";
            model.BeltString = "M50,140 a60,60 0 0,0 10,70 a120,120 0 0,1 30,-20 a40,40 0 0,1 -40,-50 M250,140 a60,60 0 0,1 -10,70 a120,120 0 0,0 -30,-20 a40,40 0 0,0 40,-50"+
              "M75,90 a20,40 0 0,1 30,0 a20,10 0 0,0 -30,0 M195,90 a20,40 0 0,1 30,0 a20,10 0 0,0 -30,0";
            model.StarRadiusX = 240;
            model.StarRadiusY = 60;
            return model;
        }
        static ShipModelNew GetModel10()
        {
            ShipModelNew model = new ShipModelNew();
            model.MiddleGun = new Point(135, 14);
            model.LeftGun = new Point(45, 59);
            model.RightGun = new Point(225, 59);
            model.MiddleGunCenter = new Point(150, 12);
            model.LeftGunCenter = new Point(60, 57);
            model.RightGunCenter = new Point(240, 57);
            model.CenterShip = new Point(150, 130);
            model.HealthPoint = new Point(85, 130);
            model.ShieldPoint = new Point(185, 130);
            model.EnergyPoint = new Point(135, 150);
            model.LeftStatusPoint = new Point(100, 160);
            model.RightStatusPoint = new Point(170, 160);
            model.IDStatusPoint = new Point(135,215 );
            model.CocpitPoint = new Point(125, 100);
            model.ShipFigure = "M150,50 l30,40 l10,30 h40 l30,-20 l10,40 l-20,60 l-60,20 l20,30 h-120 l20,-30 l-60,-20 l-20,-60 l10,-40 l30,20 h40 l10,-30z";
            model.LeftGunString = "M50,90 v16.5 l20,13.5 v-30 h6v-32v-h-32v32z";
            model.RightGunString = "M250,90 v16.5 l-20,13.5 v-30 h-6v-32v-h32v32z";
            model.MiddleGunString = "M140,45 v19 l10,-14 l10,14 v-19h6v-32h-32v32z";
            model.BeltString = "M150,60 l20,25 l-5,5 l-15,-20 l-15,20 l-5,-5z M45,110 l22,15 h20v5h-20l-17,-10l-5,20l20,50 l30,10 l-5,5 l-30,-10 l-23,-55z"+
              "M255,110 l-22,15 h-20v5h20l17,-10l5,20l-20,50 l-30,10 l5,5 l30,-10 l23,-55z";
            model.StarRadiusX = 240;
            model.StarRadiusY = 60;
            return model;
        }
        static ShipModelNew GetModel9()
        {
            ShipModelNew model = new ShipModelNew();
            model.MiddleGun = new Point(135, 4);
            model.LeftGun = new Point(55, 69);
            model.RightGun = new Point(215, 69);
            model.MiddleGunCenter = new Point(150, 2);
            model.LeftGunCenter = new Point(70, 67);
            model.RightGunCenter = new Point(230, 67);
            model.CenterShip = new Point(150, 130);
            model.HealthPoint = new Point(75, 160);
            model.ShieldPoint = new Point(195, 160);
            model.EnergyPoint = new Point(135, 180);
            model.LeftStatusPoint = new Point(70, 190);
            model.RightStatusPoint = new Point(200, 190);
            model.IDStatusPoint = new Point(135, 215);
            model.CocpitPoint = new Point(125, 100);
            model.ShipFigure = "M30,120 a40,40 0 0,0 80,0 a40,80 0 0,1 80,0 a40,40 0 0,0 80,0 a50,50 0 0,1 10,30 a150,200 0 0,1 -260,0 a50,50 0 0,1 10,-30";
            model.LeftGunString = "M60,100 v59 a40,40 0 0,0 20,0 v-59 h6v-32h-32v32z";
            model.RightGunString = "M220,100 v59 a40,40 0 0,0 20,0 v-59 h6v-32h-32v32z";
            model.MiddleGunString = "M140,35 v8 a40,80 0 0,1 20,0v-8h6v-32h-32v32z";
            model.BeltString = "M120,110 a30,30 0 0,1 60,0 a50,50 0 0,0 10,50 a100,100 0 0,1 -10,60 a30,50 0 0,0 -60,0 a100,100 0 0,1 -10,-60 a50,50 0 0,0 10,-50";
            model.StarRadiusX = 240;
            model.StarRadiusY = 60;
            return model;
        }
        static ShipModelNew GetModel8()
        {
            ShipModelNew model = new ShipModelNew();
            model.MiddleGun = new Point(135, 29);
            model.LeftGun = new Point(50, 70);
            model.RightGun = new Point(220, 70);
            model.MiddleGunCenter = new Point(150, 27);
            model.LeftGunCenter = new Point(65, 68);
            model.RightGunCenter = new Point(235, 68);
            model.CenterShip = new Point(150, 130);
            model.HealthPoint = new Point(100, 130);
            model.ShieldPoint = new Point(170, 130);
            model.EnergyPoint = new Point(135, 160);
            model.LeftStatusPoint = new Point(55, 125);
            model.RightStatusPoint = new Point(215, 125);
            model.IDStatusPoint = new Point(135, 210);
            model.CocpitPoint = new Point(125, 90);
            model.ShipFigure = "M20,150 a60,60 0 0,1 70,-30 a65,70 0 0,1 120,0 a60,60 0 0,1 70,30 a90,60 0 0,0 -70,20 a40,55 0 0,1 -120,0 a90,60 0 0,0 -70,-20";
            model.LeftGunString = "M55,100 v21 a60,60 0 0,1 20,-4 v-16 h6v-32h-32v32z";
            model.RightGunString = "M245,100 v21 a60,60 0 0,0 -20,-4 v-16 h-6v-32h32v32z";
            model.MiddleGunString = "M140,60 v18 a65,70 0 0,1 20,0 v-18h6v-32h-32v32z";
            model.BeltString = "M20,150 a60,60 0 0,1 70,-30 a65,70 0 0,1 120,0 a60,60 0 0,1 70,30 a90,60 0 0,0 -70,20 a40,55 0 0,1 -120,0 a90,60 0 0,0 -70,-20"+
              "M30,145 a60,60 0 0,1 65,-20 a60,70 0 0,1 110,0 a60,60 0 0,1 65,20 a90,60 0 0,0 -70,25 a40,60 0 0,1 -100,0 a90,60 0 0,0 -70,-25";
            model.StarRadiusX = 240;
            model.StarRadiusY = 60;
            return model;
        }
        static ShipModelNew GetModel7()
        {
            ShipModelNew model = new ShipModelNew();
            model.MiddleGun = new Point(135, 11);
            model.LeftGun = new Point(70, 50);
            model.RightGun = new Point(200, 50);
            model.MiddleGunCenter = new Point(150, 9);
            model.LeftGunCenter = new Point(85, 48);
            model.RightGunCenter = new Point(215, 48);
            model.CenterShip = new Point(150, 130);
            model.HealthPoint = new Point(115, 120);
            model.ShieldPoint = new Point(155, 120);
            model.EnergyPoint = new Point(135, 145);
            model.LeftStatusPoint = new Point(70, 140);
            model.RightStatusPoint = new Point(200, 140);
            model.IDStatusPoint = new Point(135, 180);
            model.CocpitPoint = new Point(125, 70);
            model.ShipFigure = "M120,80 a30,30 0 0,1 60,0 v40 a35,35 0 0,1 70,0 v80 a35,35 0 0,1 -70,0 a40,40 0 0,1-60,0 a35,35 0 0,1 -70,0 v-80 a35,35 0 0,1 70,0z";
            model.LeftGunString = "M95,86.5 v-25 h6 v-32 h-32 v32 h6 v25 a35,35 0 0,1 20,0";
            model.RightGunString = "M205,86.5 v-25 h-6 v-32 h32 v32 h-6 v25 a35,35 0 0,0 -20,-0";
            model.MiddleGunString = "M140,52 v-10 h-6 v-32 h32 v32 h-6 v10 a30,30 0 0,0 -20,0";
            model.BeltString = "M60,110 a35,35 0 0,1 50,0 v100 a35,35 0 0,1 -50,0z M190,110 a35,35 0 0,1 50,0 v100 a35,35 0 0,1 -50,0z";
            model.StarRadiusX = 240;
            model.StarRadiusY = 60;
            return model;
        }

        static ShipModelNew GetModel6()
        {
            ShipModelNew model = new ShipModelNew();
            model.MiddleGun = new Point(135, 21);
            model.LeftGun = new Point(65, 77);
            model.RightGun = new Point(205, 77);
            model.MiddleGunCenter = new Point(150, 19);
            model.LeftGunCenter = new Point(80, 75);
            model.RightGunCenter = new Point(220, 75);
            model.CenterShip = new Point(150, 130);
            model.HealthPoint = new Point(115, 130);
            model.ShieldPoint = new Point(155, 130);
            model.EnergyPoint = new Point(135, 165);
            model.LeftStatusPoint = new Point(65, 205);
            model.RightStatusPoint = new Point(205, 205);
            model.IDStatusPoint = new Point(135, 195);
            model.CocpitPoint = new Point(125, 100);
            model.ShipFigure = "M130,70 a25,25 0 0,1 40,0 a140,230 0 0,1 70,150 a20,20 0 0,1 -20,20 a200,200 0 0,0 -140,0 a20,20 0 0,1 -20,-20 a140,230 0 0,1 70,-150";
            model.LeftGunString = "M90,123 v-15 h6 v-32 h-32 v32 h6 v65 a140,230 0 0,1 20,-51";
            model.RightGunString = "M210,123 v-15 h-6 v-32 h32 v32 h-6 v65 a140,230 0 0,0 -20.-51";
            model.MiddleGunString = "M140,62 v-10 h-6 v-32 h32 v32 h-6 v10 a25,25 0 0,0 -20,0";
            model.BeltString = "M110,110 a50,50 0 0,1 80,0 a100,100 0 0,1 20,40 a150,150 0 0,1 -120,0 a100,100 0 0,1 20,-40";
            model.StarRadiusX = 240;
            model.StarRadiusY = 60;
            return model;
        }
        static ShipModelNew GetModel5()
        {
            ShipModelNew model = new ShipModelNew();
            model.MiddleGun = new Point(135, 59);
            model.LeftGun = new Point(75, 93);
            model.RightGun = new Point(195, 93);
            model.MiddleGunCenter = new Point(150, 57);
            model.LeftGunCenter = new Point(90, 91);
            model.RightGunCenter = new Point(210, 91);
            model.CenterShip = new Point(150, 130);
            model.HealthPoint = new Point(115, 165);
            model.ShieldPoint = new Point(155, 165);
            model.EnergyPoint = new Point(135, 195);
            model.LeftStatusPoint = new Point(80, 145);
            model.RightStatusPoint = new Point(190, 145);
            model.IDStatusPoint = new Point(135, 230);
            model.CocpitPoint = new Point(125, 130);
            model.ShipFigure = "M120,60 v50 h60 v-50 a70,100 0 0,1 0,200 h-60 a70,100 0 0,1 0,-200";
            model.LeftGunString = "M100,134 v-10 h6 v-32 h-32 v32 h6 v24 a70,100 0 0,1 20,-14";
            model.RightGunString = "M200,134 v-10 h-6 v-32 h32 v32 h-6 v24 a70,100 0 0,0 -20.-14";
            model.MiddleGunString = "M140,110 v-20 h-6 v-32 h32 v32 h-6 v20z";
            model.BeltString = "M70,160 a100,100 0 0,1 160,0 v60 a100,100 0 0,1 -160,0z";
            model.StarRadiusX = 240;
            model.StarRadiusY = 60;
            return model;
        }

        static ShipModelNew GetModel4()
        {
            ShipModelNew model = new ShipModelNew();
            model.MiddleGun = new Point(135, 39);
            model.LeftGun = new Point(60, 49);
            model.RightGun = new Point(210, 49);
            model.MiddleGunCenter = new Point(150, 38);
            model.LeftGunCenter = new Point(75, 48);
            model.RightGunCenter = new Point(225, 48);
            model.CenterShip = new Point(150, 140);
            model.HealthPoint = new Point(70, 110);
            model.ShieldPoint = new Point(200, 110);
            model.EnergyPoint = new Point(135, 145);
            model.LeftStatusPoint = new Point(115, 180);
            model.RightStatusPoint = new Point(155, 180);
            model.IDStatusPoint = new Point(135, 215);
            model.CocpitPoint = new Point(125, 90);
            model.ShipFigure = "M50,120 L100,90 L200,90 L250,120 L250,160 L190,160 A90,90 0 0,0 220,250 L80,250 A90,90 0 0,0 110,160 L50,160z";
            model.LeftGunString = "M65,111 v-32 h-5 v-30 h30 v30 h-5 v20z";
            model.RightGunString = " M235,111 v-32 h5 v-30 h-30 v30 h5 v20z";
            model.MiddleGunString = "M140,90 v-21 h-5 v-30 h30 v30 h-5 v21z";
            model.BeltString = "M105,90 A46,50 0 0,0 195,90 h5 l5,3 A55.5,55 0 0,1 95,93 L100,90z";
            model.StarRadiusX = 240;
            model.StarRadiusY = 60;
            return model;
        }
        static ShipModelNew GetModel3()
        {
            ShipModelNew model = new ShipModelNew();
            model.MiddleGun = new Point(135, 39);
            model.LeftGun = new Point(60, 49);
            model.RightGun = new Point(210, 49);
            model.MiddleGunCenter = new Point(150, 38);
            model.LeftGunCenter = new Point(75, 48);
            model.RightGunCenter = new Point(225, 48);
            model.CenterShip = new Point(150, 170);
            model.HealthPoint = new Point(110, 150);
            model.ShieldPoint = new Point(160, 150);
            model.EnergyPoint = new Point(135, 185);
            model.LeftStatusPoint = new Point(90, 180);
            model.RightStatusPoint = new Point(180, 180);
            model.IDStatusPoint = new Point(135, 220);
            model.CocpitPoint = new Point(125, 115);
            model.ShipFigure = "M150,80 A90,90 0 1,0 150.1,80";
            model.LeftGunString = " M85,107.5 V80 H91 V48 H59 V80 H65 V140.5 A90,90 0 0,1 85,107.5";
            model.RightGunString = "M215,107.5 V80 H209 V48 H241 V80 H235 V140.5 A90,90 0 0,0 215,107.5";
            model.MiddleGunString = "M140,80.5 V70 H134 V38 H166 V70 H160 V80.5 A80,80 0 0,0 140,80.5";
            model.BeltString = "M150,90 A80,80 0 1,0 150.1,90 M150,110 A70,70 0 1,0 150.1,110";
            model.StarRadiusX=240;
            model.StarRadiusY=60;
            return model;
        }
        static ShipModelNew GetModel2()
        {
            ShipModelNew model = new ShipModelNew();
            model.MiddleGun = new Point(135, 39);
            model.LeftGun = new Point(65, 59);
            model.RightGun = new Point(205, 59);
            model.MiddleGunCenter = new Point(150, 38);
            model.LeftGunCenter = new Point(80, 58);
            model.RightGunCenter = new Point(220, 58);
            model.CenterShip = new Point(150, 160);
            model.HealthPoint = new Point(85, 105);
            model.ShieldPoint = new Point(185, 105);
            model.EnergyPoint = new Point(135, 140);
            model.LeftStatusPoint = new Point(71, 150);
            model.RightStatusPoint = new Point(199, 150);
            model.IDStatusPoint = new Point(135, 180);
            model.CocpitPoint = new Point(125, 88);
            model.ShipFigure = "M150,80 A80,80 0 1,0 150.1,80";
            model.LeftGunString = " M90,107 V90 H96 V58 H64 V90 H70 V159 A80,80 0 0,1 90,107";
            model.RightGunString = "M210,107 V90 H204 V58 H236 V90 H230 V159 A80,80 0 0,0 210,107";
            model.MiddleGunString = "M140,80.5 V70 H134 V38 H166 V70 H160 V80.5 A80,80 0 0,0 140,80.5";
            model.BeltString = "M150,120 A50,50 0 1,0 150.1,120 M150,130 A40,40 0 1,0 150.1,130";
            model.StarRadiusX = 240;
            model.StarRadiusY = 60;
            return model;
        }
        static ShipModelNew GetModel1()
        {
            ShipModelNew model = new ShipModelNew();
            model.MiddleGun = new Point(135, 9);
            model.LeftGun = new Point(55, 89);
            model.RightGun = new Point(215, 89);
            model.MiddleGunCenter = new Point(150, 8);
            model.LeftGunCenter = new Point(70, 88);
            model.RightGunCenter = new Point(230, 88);
            model.CenterShip = new Point(150, 170);
            model.HealthPoint = new Point(115, 140);
            model.ShieldPoint = new Point(155, 140);
            model.EnergyPoint = new Point(115, 190);
            model.LeftStatusPoint = new Point(55, 160);
            model.RightStatusPoint = new Point(215, 160);
            model.IDStatusPoint = new Point(155, 190);
            model.CocpitPoint = new Point(125, 100);
            model.ShipFigure = "M140,50 h20 l40,50 l-10,30 v20 l20,10 l10,-10 h20 l10,10 v30 l-10,10 h-20 l-10,-10 l-20,-10 l10,50" +
                              "h20 v15 h-30 l-40,-10 l-40,10 h-30 v-15 h20 l10,-50 l-20,10 l-10,10 h-20 l-10,-10 v-30 l10,-10 h20 l10,10 l20,-10 v-20 l-10,-30z";
            model.LeftGunString = "M60,120 v30 h20 v-30 h6 v-32 h-32 v32z";
            model.RightGunString = "M220,120 v30 h20 v-30 h6 v-32 h-32 v32z";
            model.MiddleGunString = "M140,50 v-10 h-6 v-32 h32 v32 h-6 v10z";
            model.BeltString = "M140,70 h20 l20,20 h12 l8,10 l-10,30 v30 l-20,20 h-40 l-20,-20 v-30 l-10,-30 l8,-10 h12 l20,-20";
            model.StarRadiusX = 240;
            model.StarRadiusY = 60;
            return model;
        }
        static ShipModelNew GetModel30()
        {
            ShipModelNew model = new ShipModelNew();
            model.MiddleGun = new Point(135, 39);
            model.LeftGun = new Point(65, 69);
            model.RightGun = new Point(205, 69);
            model.MiddleGunCenter = new Point(150, 38);
            model.LeftGunCenter = new Point(80, 68);
            model.RightGunCenter = new Point(220, 68);
            model.CenterShip = new Point(150, 170);
            model.HealthPoint = new Point(110, 130);
            model.ShieldPoint = new Point(160, 130);
            model.EnergyPoint = new Point(135, 150);
            model.LeftStatusPoint = new Point(65, 169);
            model.RightStatusPoint = new Point(205, 169);
            model.IDStatusPoint = new Point(135, 186);
            model.CocpitPoint = new Point(125, 90);
            model.ShipFigure = "M100,130 A50,50 0 0,1 200,130 V220 H100z M110,220 L95,250 H145 L130,220z M190,220 L205,250 H155 L170,220z" +
                              "M200,140 L240,180 V200 H200z M100,140 L60,180 V200 H100z";
            model.LeftGunString = "M90,150 V100 H96 V68 H64 V100 H70 V170z";
            model.RightGunString = "M210,150 V100 H204 V68 H236 V100 H230 V170z";
            model.MiddleGunString = "M140,81 V70 H134 V38 H166 V70 H160 V81 A50,50 0 0,0 140,81";
            model.BeltString = "M100,140 A80,80 0 0,1 200,140 V200 A80,80 0 0,0 100,200";
            model.StarRadiusX = 240;
            model.StarRadiusY = 60;
            return model;
        }
        public PathGeometry GetGunsGeometry(int weapons)
        {
            string guns = "";
            if (weapons == 1)
                guns = MiddleGunString;
            else if (weapons == 2)
                guns = LeftGunString + " " + RightGunString;
            else
                guns = LeftGunString + " " + MiddleGunString + " " + RightGunString;
            PathGeometry geom = new PathGeometry((PathFigureCollection)conv.ConvertFrom(guns));
            return geom;
        }
        public PathGeometry GetShipGeometry(int weapons)
        {
            string guns = ShipFigure;
            if (weapons == 1)
                guns +=" "+ MiddleGunString;
            else if (weapons == 2)
                guns +=" "+ LeftGunString + " " + RightGunString;
            else
                guns +=" "+ LeftGunString + " " + MiddleGunString + " " + RightGunString;
            PathGeometry geom = new PathGeometry((PathFigureCollection)conv.ConvertFrom(guns));
            return geom;
        }
        public PathGeometry GetBodyGeometry()
        {
            string guns = ShipFigure;
            PathGeometry geom = new PathGeometry((PathFigureCollection)conv.ConvertFrom(guns));
            return geom;
        }
        public static DrawingBrush GetShipModelBrush(int weapons, byte model)
        {
            ShipModelNew shipmodel = Models[model];
            PathGeometry body = shipmodel.GetBodyGeometry();
            PathGeometry guns = shipmodel.GetGunsGeometry(weapons);
            PathGeometry belt = shipmodel.GetBeltGeometry();
            DrawingGroup group = new DrawingGroup();
            Pen pen = new Pen(Brushes.Black, 0.5);
            group.Children.Add(new GeometryDrawing(Common.GetRadialBrush(Colors.Gray, 0.7, 0.3), pen, body));
            group.Children.Add(new GeometryDrawing(Common.GetRadialBrush(Colors.Gray, 0.7, 0.3), pen, guns));
            group.Children.Add(new GeometryDrawing(Common.GetRadialBrush(Colors.Blue, 0.7, 0.3), pen, belt));
            return new DrawingBrush(group);
        }
        public PathGeometry GetBeltGeometry()
        {
            return new PathGeometry((PathFigureCollection)conv.ConvertFrom(BeltString));
        }
        static SortedList<byte, ShipModelNew> GetModels()
        {
            SortedList<byte, ShipModelNew> list = new SortedList<byte, ShipModelNew>();
            list.Add(0, GetModel1());
            list.Add(1, GetModel2());
            list.Add(2, GetModel3());
            list.Add(3, GetModel4());
            list.Add(4, GetModel5());
            list.Add(5, GetModel6());
            list.Add(6, GetModel7());
            list.Add(7, GetModel8());
            list.Add(8, GetModel9());
            list.Add(9, GetModel10());
            list.Add(10, GetModel11());
            list.Add(11, GetModel12());
            list.Add(12, GetModel13());
            list.Add(13, GetModel14());
            list.Add(14, GetModel15());
            return list;
        }
    }
    class ShipBrush
    {
        public static SortedList<byte, Brush> HullBrush;
        public static SortedList<byte, Brush> BeltBrush;
        static Brush GetThreeColorRadialBrush(double dx, double dy, Color color1, double offset1, Color color2, double offset2, Color color3, double offset3)
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(dx, dy);
            brush.GradientStops.Add(new GradientStop(color1, offset1));
            brush.GradientStops.Add(new GradientStop(color2, offset2));
            brush.GradientStops.Add(new GradientStop(color3, offset3));
            return brush;
        }
        public static void FillHullBrushes()
        {
            SortedList<byte, Brush> list = new SortedList<byte, Brush>();
            Color[] colorarray = GetBasicColors();

            for (int i = 0; i < 16; i++)
                list.Add((byte)i,Common.GetRadialBrush(colorarray[i], 0.8, 0.3));
            for (int i=0;i<16;i++)
                list.Add((byte)(i+16),Common.GetLinearBrush(colorarray[i],Colors.White,colorarray[i]));
            for (int i = 0; i < 16; i++)
                list.Add((byte)(i + 32), GetCrestBrush(colorarray[i]));
            for (int i = 0; i < 16; i++)
                list.Add((byte)(i + 48), new SolidColorBrush(colorarray[i]));
            for (int i = 0; i < 16; i++)
                list.Add((byte)(i + 64), GetThreeColorRadialBrush(0.8, 0.3, Colors.White, 0, colorarray[i], 0.6, Colors.Black, 1));
            for (int i = 0; i < 16; i++)
                list.Add((byte)(i + 80), GetFishBrush(colorarray[i]));
            for (int i = 0; i < 16; i++)
                list.Add((byte)(i + 96), Get5ColorBrush(colorarray[i]));
            for (int i = 0; i < 16; i++)
                list.Add((byte)(i + 112), GetWhiteBrush(colorarray[i]));
            HullBrush = list;
            BeltBrush = list;
        }
        public static LinearGradientBrush Get5ColorBrush(Color color)
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Black, 1));
            brush.GradientStops.Add(new GradientStop(color, 0.75));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            brush.GradientStops.Add(new GradientStop(color, 0.25));
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0));
            return brush;
        }
        public static DrawingBrush GetFishBrush(Color color)
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Common.GetRadialBrush(color,0.8,0.3), null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h100 v100 h-100z"))));
            group.Children.Add(new GeometryDrawing(Brushes.Black, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M0,0 h5 a30,50 0 0,1 0,100 h-5 v-5 a25,45 0 0,0 0,-90z M100,0 h-5 a30,50 0 0,0 0,100 h5 v-5 a25,45 0 0,1 0,-90z M35,45 h30 v10h-30z"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetWhiteBrush(Color color)
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(Brushes.White, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h100 v100 h-100z"))));
            group.Children.Add(new GeometryDrawing(new SolidColorBrush(color), null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M40,10 l-20,40 l-15,-10z M60,10 l20,40 l15,-10z M45,30 l-10,40 l-10,-5 M55,30 l10,40 l10,-5 M45,60 l-10,30 h10z M55,60 l10,30 h-10z "+
                  "M5, 50 l20, 40 l -10,5z M95, 50 l -20, 40 l10, 5z M5, 5 l15, 15 l -10, 10z M95, 5 l -15, 15 l10, 10z"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetCrestBrush(Color color)
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(new GeometryDrawing(new SolidColorBrush(color), null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h100 v100 h-100z"))));
            group.Children.Add(new GeometryDrawing(Brushes.Black, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M0,10 l90,90 h10 v-10 l-90,-90 h-10z M90,0 l-90,90 v10h10l90,-90v-10z"))));
            return new DrawingBrush(group);
        }
        public static DrawingBrush GetSpecialBrush(int i)
        {
            DrawingGroup group = new DrawingGroup();
            switch (i)
            {
                case 0:
                    group.Children.Add(new GeometryDrawing(Brushes.Red, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h100 v100 h-100z"))));
                    group.Children.Add(new GeometryDrawing(Brushes.Black, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                        "M0,10 l90,90 h10 v-10 l-90,-90 h-10z M90,0 l-90,90 v10h10l90,-90v-10z"))));
                    break;
                case 1:
                    group.Children.Add(new GeometryDrawing(Brushes.Green, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h100 v100 h-100z"))));
                    group.Children.Add(new GeometryDrawing(Brushes.Black, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                        "M0,10 l90,90 h10 v-10 l-90,-90 h-10z M90,0 l-90,90 v10h10l90,-90v-10z"))));
                    break;
                case 2:
                    group.Children.Add(new GeometryDrawing(Brushes.Blue, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h100 v100 h-100z"))));
                    group.Children.Add(new GeometryDrawing(Brushes.Black, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                        "M0,10 l90,90 h10 v-10 l-90,-90 h-10z M90,0 l-90,90 v10h10l90,-90v-10z"))));
                    break;
                case 3:
                    group.Children.Add(new GeometryDrawing(new SolidColorBrush(Color.FromRgb(0, 143, 64)), null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h100 v100 h-100z"))));
                    group.Children.Add(new GeometryDrawing(null, GetRoundPen(Brushes.Brown, 5), new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M20,20v10 M40,40v10 M60,60v10 M80,80v10 M60,20 v10 M80,40 v10 M20,60 v10 M40,80 v10"))));
                    break;
                case 4:
                    group.Children.Add(new GeometryDrawing(Brushes.Black, null, new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M-2.5,-2.5 h105 v105 h-105z"))));
                    group.Children.Add(new GeometryDrawing(null, GetRoundPen(new SolidColorBrush(Color.FromRgb(255,220,220)), 2), new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M20,20l10,-10l10,10 M20,60l10,-10l10,10 M20,100l10,-10l10,10"+
                                             "M40,40l10,-10l10,10 M40,80l10,-10l10,10 M60,20l10,-10l10,10 M60,60l10,-10l10,10 M60,100l10,-10l10,10 M80,40l10,-10l10,10 M80,80l10,-10l10,10"))));
                    break;
                
            }
            return new DrawingBrush(group);
        }
        public static Pen GetRoundPen(Brush brush, int thickness)
        {
            Pen pen = new Pen(brush, thickness);
            pen.StartLineCap = PenLineCap.Round; pen.EndLineCap = PenLineCap.Round;
            return pen;
        }
        
        public static Color[] GetBasicColors()
        {
            Color[] array = new Color[16];
            array[0] = Colors.Black; array[1] = Colors.Red;
            array[2] = Colors.Blue; array[3] = Colors.Green;
            array[4] = Colors.Purple; array[5] = Colors.Gold;
            array[6] = Colors.LightBlue; array[7] = Colors.Orange;
            array[8] = Colors.Violet; array[9] = Colors.Teal;
            array[10] = Colors.Silver; array[11] = Colors.Navy;
            array[12] = Colors.YellowGreen; array[13] = Colors.Goldenrod;
            array[14] = Colors.SkyBlue; array[15] = Colors.DarkGray;
            return array;
        }
    }
}
