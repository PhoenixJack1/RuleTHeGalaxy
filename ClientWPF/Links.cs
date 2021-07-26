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
using System.ServiceModel;
namespace Client
{
    enum EGameMode { Single, Multi}
    class Links
    {
        public static EGameMode GameMode;
        public static int Lang;
        public static bool LoadImageFromDLL = false;
        public static long ClientHashVersion = -1398360386796390214;
        public static string InternetServer = "http://194.87.232.199:22742/LoginService";
        public static string TestServer = "http://194.87.232.199:22742/TestServer";
        public static string LocalServer = "http://localhost:22742/LoginService";
        public static bool Animation = false;
        public static string CurrentServer = LocalServer;
        public static SaveData SaveData;
        public static PathFigureCollectionConverter conv = new PathFigureCollectionConverter();
        public static SortedList<string, string[]> InterfaceText;
        public static Controllerclass Controller;
        public static Login_Status login_Status = Login_Status.Waiting_Login;
        public static ILoginInterface Loginproxy;
        public static char InternalSeparator = (char)166;
        public static char Divider = (char)164;
        public static int GalaxyWidth = 500;
        public static int GalaxyHeight = 300;
        public static int SectorWidth = 100;
        public static int SectorHeight = 60;
        public static double ShipRotateSpeed = 200;
        public static double ShipMoveSpeed = 500;
        public static double ShootAnimSpeed = 1;
        public static TimeSpan ZeroTime = new TimeSpan(0);
        //public static bool MusicOn = true;
        //public static bool SoundOn = true;
        public static float SoundVolume = 0.8f;
        public static float MusicVolume = 0.5f;
        //public static int CurrentTurn;
        public static SortedList<int, GSStar> Stars;
        public static SortedList<int, GSPlanet> Planets;
        public static SortedList<ushort, ShipTypeclass> ShipTypes;
        public static SortedList<ushort, Computerclass> ComputerTypes;
        public static SortedList<ushort, Engineclass> EngineTypes;
        public static SortedList<ushort, Generatorclass> GeneratorTypes;
        public static SortedList<ushort, Shieldclass> ShieldTypes;
        public static SortedList<EWeaponType, WeaponModifer> Modifiers;
        public static SortedList<ArmorType, ShipArmor> Armors;
        public static SortedList<ushort, Weaponclass> WeaponTypes;
        public static SortedList<ushort, Equipmentclass> EquipmentTypes;
        public static SortedList<ushort, GSBuilding> Buildings;
        public static SortedList<ushort, Artefact> Artefacts;


        public static SortedList<int, string> Quest_Ru = new SortedList<int, string>();
        public static SortedList<int, string> Quest_En = new SortedList<int, string>();

        public static FontFamily Font = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#Agru");
        
        //public static FontFamily Font = new FontFamily("Whitestone [RUS by Daymarius]");
        //public static FontFamily Font = new FontFamily(new Uri("pack://application:,,,/"), "./resources/#12227");
        public static BitmapImage LoginImage = new BitmapImage(new Uri("pack://application:,,,/resources/tree.png"));
        public static BitmapImage PersImage = new BitmapImage(new Uri("pack://application:,,,/resources/personazh.png"));
        //public static ImageBrush EnTitleBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/resources/Title_en.png")));
        //public static ImageBrush RuTitleBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/resources/Title_ru.png")));
        public static BitmapImage IconImage = new BitmapImage(new Uri("pack://application:,,,/resources/Icon2.png"));
        public static Style SmallTextStyle;
        public static Style TextStyle;
        public static Style SmallTextStyleWhite;
        public static Style MediumTextStyle;
        public static Style ButtonStyle = new Style();

        //public static SortedList<string, BitmapSource> PicsList;

        public static string Interface(string position)
        {return InterfaceText[position][Lang];}
        public static SortedList<WeaponGroup, string> WeaponGroupTips = Common.GetWeaponGroupTip();
        public static class Helper
        {
            public static void SetShip(GSShip ship) { Ship = ship; }
            public static GSShip GetShip() { GSShip ship = Ship; Ship = null; return ship; }
            static GSShip Ship;
            public static GSFleet Fleet;
            public static MouseButtonEventHandler ClickDelegate;
            public static GSStar Star;
            public static GSPlanet Planet;
            public static int LandID;
            public static GSClan Clan;
            public static FleetParamsPanel FleetParamsPanel;
            //public static Mission Mission;
            public static GameScience Science;
            public static LandInfoType LandInfoType;
            public static ArtefactOnSectorDelegate ArtefactOnSectorHandler;
            public static ushort ArtefactID;
        }
        //public delegate void SpaceObjectClickDelegate(object sender, RoutedEventArgs e);
        public class Brushes
        {
            public static SolidColorBrush SkyBlue = new SolidColorBrush(Color.FromRgb(0, 168, 255));
            public static SolidColorBrush Green = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            public static SolidColorBrush GreenBlue = new SolidColorBrush(Color.FromRgb(76, 255, 200));
            public class Artefact
            {
                public static ImageBrush Artefact0000 = Gets.AddPicArt("1_ResourceSintezatorSmall");
                public static ImageBrush Artefact0001 = Gets.AddPicArt("1_PeopleGrow");
                public static ImageBrush Artefact0002 = Gets.AddPicArt("1_DestroySector");
                public static ImageBrush Artefact0003 = Gets.AddPicArt("1_AddSector");
                public static ImageBrush Artefact0004 = Gets.AddPicArt("1_AvanpostCompliter");

                public static ImageBrush Artefact1000 = Gets.AddPicArt("2_MoveShip");
                public static ImageBrush Artefact1001 = Gets.AddPicArt("2_RestoreShip");
                public static ImageBrush Artefact1002 = Gets.AddPicArt("2_MoveAstro");
                public static ImageBrush Artefact1010 = Gets.AddPicArt("2_NoEnergy");
                public static ImageBrush Artefact1011 = Gets.AddPicArt("2_NoPhysic");
                public static ImageBrush Artefact1012 = Gets.AddPicArt("2_NoIrregular");
                public static ImageBrush Artefact1013 = Gets.AddPicArt("2_NoCyber");
                public static ImageBrush Artefact1020 = Gets.AddPicArt("2_DoubleEnergy");
                public static ImageBrush Artefact1021 = Gets.AddPicArt("2_DoublePhysic");
                public static ImageBrush Artefact1022 = Gets.AddPicArt("2_DoubleIrregular");
                public static ImageBrush Artefact1023 = Gets.AddPicArt("2_DoubleCyber");
                public static ImageBrush Artefact1030 = Gets.AddPicArt("2_SolarDamage");
                public static ImageBrush Artefact1031 = Gets.AddPicArt("2_PhysicDamage");
                public static ImageBrush Artefact1032 = Gets.AddPicArt("2_IrregularDamage");
                public static ImageBrush Artefact1033 = Gets.AddPicArt("2_CyberDamage");
            }
            public class Interface
            {
                public static ImageBrush MainImage = Gets.AddPicI("I001_Main");
                public static ImageBrush DownComp = Gets.AddPicI("I002_DownComp");
                public static ImageBrush LeftCorner =Gets.AddPicI("I003_LeftCorner");
                public static ImageBrush RightCorner = Gets.AddPicI("I004_RightCorner");
                public static ImageBrush Chat = Gets.AddPicI("I005_Chat");
                public static ImageBrush ResBlank = Gets.AddPicI("I007_ResBlank");
                public static ImageBrush Gear = Gets.AddPicI("I006_Gear");
                public static ImageBrush MBYgolokLeft = Gets.AddPicI("I008_YgolokLeft");
                public static ImageBrush MBYgolorRight = Gets.AddPicI("I009_YgolokRight");
                public static ImageBrush MBShester1 = Gets.AddPicI("I010_Shester1");
                public static ImageBrush MBShester2 = Gets.AddPicI("I011_Shester2");
                public static ImageBrush MBKlapon = Gets.AddPicI("I012_Klapon");
                public static ImageBrush MBShluz = Gets.AddPicI("I013_Shluz");
                public static ImageBrush MBStvorkaLeft = Gets.AddPicI("I014_StvorkaLeft");
                public static ImageBrush MBStvorkaRight = Gets.AddPicI("I015_StvorkaRight");
                public static ImageBrush MBButtonBack = Gets.AddPicI("I016_ButtonBack");
                public static ImageBrush NationPanelBack = Gets.AddGCB("GCB_08");
                public static ImageBrush RamkaSmall =Gets.AddPicI("I017_RamkaSmall");
                public static ImageBrush RamkaSmall2 = Gets.AddPicI("I018_RamkaSmall2");
                public static ImageBrush LeftArrow = Gets.AddPicI("I019_LeftArrow");
                public static ImageBrush RightArrow = Gets.AddPicI("I020_RightArrow");
                public static ImageBrush TextPanel = Gets.AddPicI("I021_TextPanel");
                public static ImageBrush Shesternya = Gets.AddPicI("I022_Shesternya");
                public static ImageBrush Zakryvashka = Gets.AddPicI("I023_Zakryvashka");
                public static ImageBrush ResBack = Gets.AddPicI("I024_ResBack");
                public static ImageBrush SchemaGenerator = Gets.AddPicI("I025_SchemaGenerator");
                public static ImageBrush BigSquare = Gets.AddPicI("I026_BigSquare");
                public static ImageBrush SmallSquare = Gets.AddPicI("I027_SmallSquare");
                public static ImageBrush WeaponRound = Gets.AddPicI("I028_WeaponRound");
                public static ImageBrush Refresh = Gets.AddPicI("I029_Refresh");
                public static ImageBrush AddPanel = Gets.AddPicI("I030_AddPanel");
                public static ImageBrush AddPanelSmall = Gets.AddPicI("I031_AddPanelSmall");
                public static ImageBrush BigSchema = Gets.AddPicI("I032_BigSchema");
                public static ImageBrush ImageBack = Gets.AddPicI("I054_ImageBack");

                public static ImageBrush RamkaMedium = Gets.AddPicI("I062_RamkaMedium");
                public static ImageBrush ButtonMedium = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/resources/BtnAdd.png")));
                public static ImageBrush RamkaWeapon = Gets.AddPicI("I064_RamkaWeapon");

                public static ImageBrush ClearSchema = Gets.AddPicI("I033_ClearSchema");
                public static ImageBrush SpeedShip = Gets.AddPicI("I034_SpeedShip");
                public static ImageBrush DamageShip = Gets.AddPicI("I035_DamageShip");
                public static ImageBrush TankShip = Gets.AddPicI("I036_TankShip");
                public static ImageBrush SupportShip = Gets.AddPicI("I037_SupportShip");
                //public static ImageBrush LoadSchema = new ImageBrush();
                public static ImageBrush SchemaBack = Gets.AddPicI("I039_SchemaBack");
                public static ImageBrush SchemaBack2 = Gets.AddPicI("I061_SchemaBack2");

                public static ImageBrush ShipCreateBorder = Gets.AddPicI("I040_ShipCreateBorder");
                public static ImageBrush ShipCreateLArrow = Gets.AddPicI("I041_ShipCreateLArrow");
                public static ImageBrush ShipCreareRArrow = Gets.AddPicI("I042_ShipCreateRArrow");
                public static ImageBrush ShipCreateR1 = Gets.AddPicI("I043_ShipCreateR1");
                public static ImageBrush ShipCreateR2 = Gets.AddPicI("I044_ShipCreateR2");
                public static ImageBrush ShipCreateR3 = Gets.AddPicI("I045_ShipCreateR3");
                public static ImageBrush ShipCreateR4 = Gets.AddPicI("I046_ShipCreateR4");
                public static ImageBrush ShipCreateR5 = Gets.AddPicI("I047_ShipCreateR5");
                public static ImageBrush ShipCreateR6 = Gets.AddPicI("I048_ShipCreateR6");
                public static ImageBrush ShipCreateP1 = Gets.AddPicI("I049_ShipCreateP1");
                public static ImageBrush ShipCreateP2 = Gets.AddPicI("I050_ShipCreateP2");
                public static ImageBrush ShipCreateP3 = Gets.AddPicI("I051_ShipCreateP3");
                public static ImageBrush ShipCreateP4 = Gets.AddPicI("I052_ShipCreateP4");
                public static ImageBrush ShipCreateP5 = Gets.AddPicI("I053_ShipCreateP5");

                public static ImageBrush ParamBtnR1 = Gets.AddPicI("I055_PB1");
                public static ImageBrush ParamBtnR2 = Gets.AddPicI("I056_PB2");
                public static ImageBrush ParamBtnR3 = Gets.AddPicI("I057_PB3");
                public static ImageBrush ParamBtnR4 = Gets.AddPicI("I058_PB4");
                public static ImageBrush ParamBtnS1 = Gets.AddPicI("I059_PBS1");
                public static ImageBrush ParamBtnS2 = Gets.AddPicI("I060_PBS2");


                //public static ImageBrush BattleMainImage = new ImageBrush();
                //public static ImageBrush BTNConfirm = new ImageBrush();
                //public static ImageBrush BTNCancel = new ImageBrush();
                //public static ImageBrush BTNCancelX = new ImageBrush();
                //public static ImageBrush AutoPlayer = new ImageBrush();
                public static ImageBrush AutoComputer = Gets.AddPicI("BI006_Computer");
                public static ImageBrush BTNM = Gets.AddPicI("BI007_BTNM");
                public static ImageBrush BTNL = Gets.AddPicI("BI008_BTNL");
                public static ImageBrush BTNR = Gets.AddPicI("BI009_BTNR");
                public static ImageBrush BTNA = Gets.AddPicI("BI010_BTNA");
                //public static ImageBrush ScrollUp = new ImageBrush();
                //public static ImageBrush ScrollDown = new ImageBrush();
                public static ImageBrush BattleZastavka = Gets.AddPicI("BI013_Zastavka");
                /*
                public static ImageBrush BattleInfo1 = new ImageBrush();
                public static ImageBrush BattleInfo2 = new ImageBrush();
                public static ImageBrush BattleConfirm1 = new ImageBrush();
                public static ImageBrush BattleConfirm2 = new ImageBrush();
                public static ImageBrush BattleAttack1 = new ImageBrush();
                public static ImageBrush BattleAttack2 = new ImageBrush();
                public static ImageBrush BattleAuto1 = new ImageBrush();
                */
            }
            public class Colony
            {
                public static ImageBrush Colony1 = Gets.AddPicColony("Colony1");
                public static ImageBrush Colony2 = Gets.AddPicColony("Colony2");
                public static ImageBrush Colony3 = Gets.AddPicColony("Colony3");
                public static ImageBrush Colony4 = Gets.AddPicColony("Colony4");
                public static ImageBrush Colony5 = Gets.AddPicColony("Colony5");
                public static ImageBrush Colony6 = Gets.AddPicColony("Colony6");
                public static ImageBrush Colony7 = Gets.AddPicColony("Colony7");
                public static ImageBrush Colony8 = Gets.AddPicColony("Colony8");
                public static ImageBrush Colony9 = Gets.AddPicColony("Colony9");
            }
            public class Login
            {
                public static ImageBrush En0 = Gets.AddLoginPic("L001_En0");
                public static ImageBrush En1 = Gets.AddLoginPic("L002_En1");
                public static ImageBrush Ru0 = Gets.AddLoginPic("L003_Ru0");
                public static ImageBrush Ru1 = Gets.AddLoginPic("L004_Ru1");
                public static ImageBrush ExitE0 = Gets.AddLoginPic("L005_ExitE0");
                public static ImageBrush ExitE1 = Gets.AddLoginPic("L006_ExitE1");
                public static ImageBrush ExitR0 = Gets.AddLoginPic("L007_ExitR0");
                public static ImageBrush ExitR1 = Gets.AddLoginPic("L008_ExitR1");
                public static ImageBrush OnlineE0 = Gets.AddLoginPic("L009_OnlineE0");
                public static ImageBrush OnlineE1 = Gets.AddLoginPic("L010_OnlineE1");
                public static ImageBrush OnlineR0 = Gets.AddLoginPic("L011_OnlineR0");
                public static ImageBrush OnlineR1 = Gets.AddLoginPic("L012_OnlineR1");
                public static ImageBrush RandomE0 = Gets.AddLoginPic("L013_RandomE0");
                public static ImageBrush RandomE1 = Gets.AddLoginPic("L014_RandomE1");
                public static ImageBrush RandomR0 = Gets.AddLoginPic("L015_RandomR0");
                public static ImageBrush RandomR1 = Gets.AddLoginPic("L016_RandomR1");
                public static ImageBrush SingleE0 = Gets.AddLoginPic("L017_SingleE0");
                public static ImageBrush SingleE1 = Gets.AddLoginPic("L018_SingleE1");
                public static ImageBrush SingleR0 = Gets.AddLoginPic("L019_SingleR0");
                public static ImageBrush SingleR1 = Gets.AddLoginPic("L020_SingleR1");
                public static ImageBrush LogoGame = Gets.AddLoginPic("L021_LogoGame");
                public static ImageBrush LogoStudio = Gets.AddLoginPic("L022_LogoStudio");
                public static ImageBrush LogoBeta = Gets.AddLoginPic("L023_LogoBeta");
                public static ImageBrush VK0 = Gets.AddLoginPic("L024_LogoVK0");
                public static ImageBrush VK1 = Gets.AddLoginPic("L025_LogoVK1");
            }
            public class Science
            {
                //public static ImageBrush WallUp = Gets.
                //public static ImageBrush WallDown = Gets.
                public static ImageBrush Glass = Gets.AddPicScience("S003_Glass");
                public static ImageBrush Hand1 = Gets.AddPicScience("S004_Hand1");
                public static ImageBrush Hand2 = Gets.AddPicScience("S005_Hand2");
                public static ImageBrush Hand3 = Gets.AddPicScience("S006_Hand3");
                public static ImageBrush Hand4 = Gets.AddPicScience("S007_Hand4");
                public static ImageBrush WallUp2 = Gets.AddPicScience("S008_WallUp2");
                public static ImageBrush Round = Gets.AddPicScience("S009_Round");
                public static ImageBrush STV1 = Gets.AddPicScience("S010_STV1");
                public static ImageBrush STV2 = Gets.AddPicScience("S011_STV2");
                public static ImageBrush Regirator = Gets.AddPicScience("S012_Regirator");
                public static ImageBrush Button = Gets.AddPicScience("S013_Button");
                public static ImageBrush Border = Gets.AddPicScience("S014_Border");
            }
            public class Ships
            {
                public static ImageBrush Ship1 = Gets.AddPicShip("S001_Ship1");
                public static ImageBrush Ship2 = Gets.AddPicShip("S002_Ship2");
                public static ImageBrush Ship3 = Gets.AddPicShip("S003_Ship3");
                public static ImageBrush Ship4 = Gets.AddPicShip("S004_Ship4");
                public static ImageBrush Ship5 = Gets.AddPicShip("S005_Ship5");
                public static ImageBrush Ship6 = Gets.AddPicShip("S006_Ship6");
                public static ImageBrush Ship8 = Gets.AddPicShip("S008_Ship8");
                public static ImageBrush Ship10 = Gets.AddPicShip("S010_Ship10");

                public static ImageBrush ShipLR = Gets.AddPicShip("S101_LR");
                public static ImageBrush ShipMR = Gets.AddPicShip("S102_MR");
                public static ImageBrush ShipDR = Gets.AddPicShip("S103_DR");
                public static ImageBrush ShipLG = Gets.AddPicShip("S201_LG");
                public static ImageBrush ShipMG = Gets.AddPicShip("S202_MG");
                public static ImageBrush ShipDG = Gets.AddPicShip("S203_DG");
                public static ImageBrush ShipLB = Gets.AddPicShip("S301_LB");
                public static ImageBrush ShipMB = Gets.AddPicShip("S302_MB");
                public static ImageBrush ShipDB = Gets.AddPicShip("S303_DB");
                public static ImageBrush ShipLP = Gets.AddPicShip("S401_LP");
                public static ImageBrush ShipMP = Gets.AddPicShip("S402_MP");
                public static ImageBrush ShipDP = Gets.AddPicShip("S403_DP");
                public static ImageBrush ShipW = Gets.AddPicShip("S501_W");
                public static ImageBrush ShipG = Gets.AddPicShip("S502_G");
                public static ImageBrush ShipB = Gets.AddPicShip("S503_B");

                public static ImageBrush Portal1 = Gets.AddPortalPic("SP01");
                public static ImageBrush Portal2 = Gets.AddPortalPic("SP02");
                public static ImageBrush Portal3 = Gets.AddPortalPic("SP03");
                public static ImageBrush Portal4 = Gets.AddPortalPic("SP04");
                public static ImageBrush Portal5 = Gets.AddPortalPic("SP05");
                public static ImageBrush Portal6 = Gets.AddPortalPic("SP06");

                public static ImageBrush PortalTeleport = Gets.AddPortalPic("portal_effr2");

                public static ImageBrush ShipShield = Gets.AddPicI("BI014_Shield");
                public static SortedList<byte, ImageBrush> Meteorits = Gets.GetBasicMeteorits();
                //public static ImageBrush QuestMeteorit0 = Gets.AddPicMeteor("M020_S0");
                public static SortedList<string, ImageBrush> Model6Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> Model7Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> Model9Brushes = new SortedList<string, ImageBrush>();

                public static SortedList<string, ImageBrush> ModelNew1Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> ModelNew2Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> ModelNew3Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> ModelNew4Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> ModelNew5Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> ModelNew6Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> ModelNew7Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> ModelNew8Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> ModelNew9Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> ModelNew10Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> ModelNew11Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> ModelNew101Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> ModelNew102Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> ModelNew103Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> ModelNew104Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> ModelNew105Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> ModelNew251Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> ModelNew252Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> ModelNew253Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> ModelNew254Brushes = new SortedList<string, ImageBrush>();
                public static SortedList<string, ImageBrush> ModelNew255Brushes = new SortedList<string, ImageBrush>();
            }
            public class Guns
            {
                public static ImageBrush Gun1 = Gets.AddGun("G001_Gun1");
                public static ImageBrush Gun2 = Gets.AddGun("G002_Gun2");
                public static ImageBrush Gun3 = Gets.AddGun("G003_Gun3");
                public static ImageBrush Gun4 = Gets.AddGun("G004_Gun4");
                public static ImageBrush Gun5 = Gets.AddGun("G005_Gun5");
                public static ImageBrush Gun6 = Gets.AddGun("G006_Gun6");
                public static ImageBrush Gun7 = Gets.AddGun("G007_Gun7");
                public static ImageBrush Gun8 = Gets.AddGun("G008_Gun8");
                public static ImageBrush Gun9 = Gets.AddGun("G009_Gun9");
                public static ImageBrush Gun10 = Gets.AddGun("G010_Gun10");
                public static ImageBrush Gun11 = Gets.AddGun("G011_Gun11");
                public static ImageBrush Gun12 = Gets.AddGun("G012_Gun12");
                public static ImageBrush Gun13 = Gets.AddGun("G013_Gun13");
                public static ImageBrush Gun14 = Gets.AddGun("G014_Gun14");
            }
            public static ImageBrush MoneyImageBrush = Gets.AddPic("R001_Money");
            public static ImageBrush MetalImageBrush = Gets.AddPic("R002_Metal");
            public static ImageBrush ChipsImageBrush = Gets.AddPic("R003_Chip");
            public static DrawingBrush AntiImageBrush = Pictogram.GetAntiResBrush();
            public static ImageBrush ZipImageBrush = Gets.AddPic("R005_Zip");
            public static ImageBrush MetalCapBrush = Gets.AddPic("R007_MetalCap");
            public static ImageBrush ChipCapBrush = Gets.AddPic("R008_ChipCap");
            public static ImageBrush AntiCapBrush = Gets.AddPic("R009_AntiCap");
            public static ImageBrush ZipCapBrush = Gets.AddPic("R010_ZipCap");
            public static ImageBrush MathPowerBrush = Gets.AddPic("R011_MathPower");
            
            public static ImageBrush BuildingSizeBrush = Gets.AddPic("R012_Size");
            public static SortedList<PlanetTypes, ImageBrush> PlanetTypeBrushes = new SortedList<PlanetTypes, ImageBrush>();

            public static ImageBrush HexSmallImage = Gets.AddPic("S030_Hex");
            public static ImageBrush TwoSwords = Gets.AddPic("S031_TwoSwords");

            public static ImageBrush Helper0 = Gets.AddPic("S032_Helper0");
            public static ImageBrush Helper1 = Gets.AddPic("S033_Helper1");
            public static ImageBrush Helper2_1 = Gets.AddPic("S043_Helper21");
            public static ImageBrush Helper2_2 = Gets.AddPic("S044_Helper22");

            public static ImageBrush ArmorSelect = Gets.AddPic("S048_ArmorSelect");

            public static DrawingBrush AddImageBrush = Pictogram.GetAddItemBrush();
            public static ImageBrush PeopleImageBrush = Gets.AddPic("R013_Peoples");
            //public static ImageBrush BuildingImageBrush = new ImageBrush();
            public static DrawingBrush ShipImageBrush = Pictogram.ShipBrush();
            public static DrawingBrush FleetBrush = Pictogram.FleetBrush();
            public static DrawingBrush RepairPict = Pictogram.GetRepairPictBrush();
            public static DrawingBrush SciencePict = Pictogram.GetSciencePictBrush();
            public static DrawingBrush[] ShipTypeBrushes = new DrawingBrush[] { null, Pictogram.ShipTypeBrush(1), Pictogram.ShipTypeBrush(2), Pictogram.ShipTypeBrush(3) };
            public static DrawingBrush ShipModelBrush = Pictogram.GetShipModelBrush();
            //public static ImageBrush SpeedImageBrush = new ImageBrush();
            public static DrawingBrush RareImageBrush = Pictogram.GetRarePictBrush();
            public static DrawingBrush SmallImageBrush = Pictogram.GetSmallItemBrush();
            public static DrawingBrush MediumImageBrush = Pictogram.GetMediumItemBrush();
            public static DrawingBrush LargeImageBrush = Pictogram.GetLargeItemBrush();
            public static DrawingBrush GetItemSizeBrush(ItemSize size)
            {
                if (size == ItemSize.Small) return SmallImageBrush;
                else if (size == ItemSize.Medium) return MediumImageBrush;
                else return LargeImageBrush;
            }
            public static DrawingBrush EmptyImageBrush = Pictogram.GetEmptyBrush();
            public static ImageBrush ArtefactsBrush = Gets.AddPicArt("Artefacts");
            //public static ImageBrush EmptyImageBrush = new ImageBrush();
            public static DrawingBrush CrownBrush = Pictogram.GetGoldCrownBrush();
            public static ImageBrush LandIconBrush = Gets.AddPic("S035_Land");
            //public static DrawingBrush LandIconBrush = Pictogram.GetLandIconBrush();
            public static DrawingBrush LeftArrowBrush = Pictogram.GetLeftArrowBrush();
            public static DrawingBrush RightArrowBrush = Pictogram.GetRightArrowBrush();
            public static DrawingBrush VBrush = Pictogram.GetVBrush();
            public static DrawingBrush CloudBrush = Pictogram.GetCloudBrush();
            public static DrawingBrush RiotBrush = Pictogram.GetRiotBrush();
            public static DrawingBrush RiotPrepareBrush = Pictogram.GetRiotPrepareBrush();
            public static ImageBrush ExperienceBrush = Gets.AddPic("R014_Experience");
            public static ImageBrush RusFlagBrush = Gets.AddPic("R015_RusFlag");
            public static ImageBrush JapFlagBrush = Gets.AddPic("R016_JapFlag");
            public static ImageBrush USAFlagBrush = Gets.AddPic("R017_USAFlag");
            public static ImageBrush Explosion = Gets.AddPic("S034_Explosion");

            public static DrawingBrush LoadWindowBrush = WeapCanvas.GetLoadWindowBrush();

            public static ImageBrush TradeSector = Gets.AddPic("B001_TS");
            public static ImageBrush Bank = Gets.AddPic("B002_BA");
            public static ImageBrush Mine = Gets.AddPic("B003_MI");
            public static ImageBrush ChipsFactory = Gets.AddPic("B004_CF");
            public static ImageBrush University = Gets.AddPic("B005_UN");
            public static ImageBrush ParticipleAccelerator = Gets.AddPic("B006_PA");
            public static ImageBrush ScienceSector = Gets.AddPic("B007_SC");
            public static ImageBrush Manufacture = Gets.AddPic("B008_MA");
            public static ImageBrush RepairingBay = Gets.AddPic("B009_RB");
            public static ImageBrush Radar = Gets.AddPic("B010_RS");
            public static ImageBrush DataCenter = Gets.AddPic("B011_DC");
            /*
            public static DrawingBrush GeneratorBrush = Pictogram.GetGeneratorBrush();
            public static DrawingBrush ShieldBrush = Pictogram.GetShieldBrush();
            public static DrawingBrush EngineBrush = Pictogram.GetEngineBrush();
            public static DrawingBrush ComputerBrush = Pictogram.GetComputerBrush();
    */
            public static ImageBrush GeneratorBrush = Gets.AddPic("S050_EnergyGenerator");
            public static ImageBrush ShieldBrush = Gets.AddPic("S052_ShieldGenerator");
            public static ImageBrush EngineBrush = Gets.AddPic("S051_Engine");
            public static ImageBrush ComputerBrush = Gets.AddPic("S049_AimComputer");
            public static ImageBrush ShipCreate = Gets.AddPic("S053_ShipCreate");
            public static class Items
            {
                public static SortedList<ItemSize, ImageBrush> GeneratorBrushes = new SortedList<ItemSize, ImageBrush>();
                public static SortedList<ItemSize, ImageBrush> ShieldBrushes = new SortedList<ItemSize, ImageBrush>();
                public static SortedList<ItemSize, ImageBrush> EngineBrushes = new SortedList<ItemSize, ImageBrush>();
                public class ComputerBrushesG
                {
                    static ImageBrush RedSmall = Gets.AddPicGI("Comp_Red_Small");
                    static ImageBrush RedMedium = Gets.AddPicGI("Comp_Red_Medium");
                    static ImageBrush RedLarge = Gets.AddPicGI("Comp_Red_Large");
                    static ImageBrush BlueSmall = Gets.AddPicGI("Comp_Blue_Small");
                    static ImageBrush BlueMedium = Gets.AddPicGI("Comp_Blue_Medium");
                    static ImageBrush BlueLarge = Gets.AddPicGI("Comp_Blue_Large");
                    static ImageBrush PurpleSmall = Gets.AddPicGI("Comp_Purple_Small");
                    static ImageBrush PurpleMedium = Gets.AddPicGI("Comp_Purple_Medium");
                    static ImageBrush PurpleLarge = Gets.AddPicGI("Comp_Purple_Large");
                    static ImageBrush GreenSmall = Gets.AddPicGI("Comp_Green_Small");
                    static ImageBrush GreenMedium = Gets.AddPicGI("Comp_Green_Medium");
                    static ImageBrush GreenLarge = Gets.AddPicGI("Comp_Green_Large");
                    static ImageBrush YellowSmall = Gets.AddPicGI("Comp_Yellow_Small");
                    static ImageBrush YellowMedium = Gets.AddPicGI("Comp_Yellow_Medium");
                    static ImageBrush YellowLarge = Gets.AddPicGI("Comp_Yellow_Large");
                    public static ImageBrush Get(WeaponGroup group, ItemSize size)
                    {
                        switch (group)
                        {
                            case WeaponGroup.Any:
                                if (size == ItemSize.Small) return YellowSmall;
                                else if (size == ItemSize.Medium) return YellowMedium;
                                else return YellowLarge;
                            case WeaponGroup.Energy:
                                if (size == ItemSize.Small) return BlueSmall;
                                else if (size == ItemSize.Medium) return BlueMedium;
                                else return BlueLarge;
                            case WeaponGroup.Physic:
                                if (size == ItemSize.Small) return RedSmall;
                                else if (size == ItemSize.Medium) return RedMedium;
                                else return RedLarge;
                            case WeaponGroup.Irregular:
                                if (size == ItemSize.Small) return PurpleSmall;
                                else if (size == ItemSize.Medium) return PurpleMedium;
                                else return PurpleLarge;
                            default:
                                if (size == ItemSize.Small) return GreenSmall;
                                else if (size == ItemSize.Medium) return GreenMedium;
                                else return GreenLarge;
                        }
                    }
                }
                public static SortedList<ItemSize, ImageBrush> ComputerBrushes = new SortedList<ItemSize, ImageBrush>();
                public static ImageBrush BorderBlue = Gets.AddPicGI("GI013_BorderBlue");
                public static ImageBrush BorderGreen = Gets.AddPicGI("GI024_BorderGreen");
                public static ImageBrush BorderRed = Gets.AddPicGI("GI025_BorderRed");
                public static ImageBrush BorderPink = Gets.AddPicGI("GI026_BorderPink");
                public static ImageBrush BorderGold = Gets.AddPicGI("GI027_BorderGold");
                public static ImageBrush BorderShield = Gets.AddPicGI("GI028_BorderShield");
                public static ImageBrush BorderGenerator = Gets.AddPicGI("GI029_BorderGenerator");
                public static ImageBrush BorderGenerator2 = Gets.AddPicGI("GI030_BorderGenerator2");
                public static ImageBrush BorderEngine = Gets.AddPicGI("GI031_BorderEngine");
                public static ImageBrush BorderEngine2 = Gets.AddPicGI("GI032_BorderEngine2");
                public static ImageBrush ShipSchema3 = Gets.AddPicGI("GI033_ShipSchema3");
                public static ImageBrush ShipSchema2 = Gets.AddPicGI("GI034_ShipSchema2");
                public static ImageBrush ShipSchema1 = Gets.AddPicGI("GI035_ShipSchema1");
                public static ImageBrush BorderBuilding = Gets.AddPicGI("GI036_BorderBuilding");
                public static ImageBrush BorderBlueE = Gets.AddPicGI("GI014_BorderBlueE");
                public static ImageBrush BorderBlueD = Gets.AddPicGI("GI015_BorderBlueD");
                public static ImageBrush BorderBlueW = Gets.AddPicGI("GI016_BorderBlueW");
                public static ImageBrush BorderPinkW = Gets.AddPicGI("GI017_BorderPinkW");
                public static ImageBrush BorderRedW = Gets.AddPicGI("GI018_BorderRedW");
                public static ImageBrush BorderGreenW = Gets.AddPicGI("GI019_BorderGreenW");
                public static ImageBrush EnSpent = Gets.AddPicGI("GI023_EnergySpent");

                public static SortedList<ItemSize, ImageBrush> SpeedSize = new SortedList<ItemSize, ImageBrush>();
            }
              public static class Buildings
            {
                public static ImageBrush BuildCran = Gets.GetBuildingBrush("Build000");
                public static ImageBrush BuildLive1 = Gets.GetBuildingBrush("B_1_Live_1");
                public static ImageBrush BuildLive2 = Gets.GetBuildingBrush("B_1_Live_2");
                public static ImageBrush BuildLive3 = Gets.GetBuildingBrush("B_1_Live_3");
                public static ImageBrush BuildMoney1 = Gets.GetBuildingBrush("B_2_Money_1");
                public static ImageBrush BuildMoney2 = Gets.GetBuildingBrush("B_2_Money_2");
                public static ImageBrush BuildMoney3 = Gets.GetBuildingBrush("B_2_Money_3");
                public static ImageBrush BuildMetal1 = Gets.GetBuildingBrush("B_4_Metall_1");
                public static ImageBrush BuildMetal2 = Gets.GetBuildingBrush("B_4_Metall_2");
                public static ImageBrush BuildMetal3 = Gets.GetBuildingBrush("B_4_Metall_3");
                public static ImageBrush BuildMetCap1 = Gets.GetBuildingBrush("B_5_MetCap_1");
                public static ImageBrush BuildMetCap2 = Gets.GetBuildingBrush("B_5_MetCap_2");
                public static ImageBrush BuildMetCap3 = Gets.GetBuildingBrush("B_5_MetCap_3");
                public static ImageBrush BuildChip1 = Gets.GetBuildingBrush("B_6_Chip_1");
                public static ImageBrush BuildChip2 = Gets.GetBuildingBrush("B_6_Chip_2");
                public static ImageBrush BuildChip3 = Gets.GetBuildingBrush("B_6_Chip_3");
                public static ImageBrush BuildChipCap1 = Gets.GetBuildingBrush("B_7_ChipCap_1");
                public static ImageBrush BuildChipCap2 = Gets.GetBuildingBrush("B_7_ChipCap_2");
                public static ImageBrush BuildChipCap3 = Gets.GetBuildingBrush("B_7_ChipCap_3");
                public static ImageBrush BuildAnti1 = Gets.GetBuildingBrush("B_8_Anti_1");
                public static ImageBrush BuildAnti2 = Gets.GetBuildingBrush("B_8_Anti_2");
                public static ImageBrush BuildAnti3 = Gets.GetBuildingBrush("B_8_Anti_3");
                public static ImageBrush BuildAntiCap1 = Gets.GetBuildingBrush("B_9_AntiCap_1");
                public static ImageBrush BuildAntiCap2 = Gets.GetBuildingBrush("B_9_AntiCap_2");
                public static ImageBrush BuildAntiCap3 = Gets.GetBuildingBrush("B_9_AntiCap_3");
                public static ImageBrush BuildRepair1 = Gets.GetBuildingBrush("B_3_Repair_1");
                public static ImageBrush BuildRepair2 = Gets.GetBuildingBrush("B_3_Repair_2");
                public static ImageBrush BuildRepair3 = Gets.GetBuildingBrush("B_3_Repair_3");
                public static ImageBrush BuildWar0 = Gets.GetBuildingBrush("B_10_WarBase_1");
                public static ImageBrush BuildWar1 = Gets.GetBuildingBrush("B_10_WarBase_2");
                public static ImageBrush BuildWar2 = Gets.GetBuildingBrush("B_10_WarBase_3");
                public static ImageBrush BuildLive1_A = Gets.GetBuildingBrush("A_1_Live_1");
                public static ImageBrush BuildLive2_A = Gets.GetBuildingBrush("A_1_Live_2");
                public static ImageBrush BuildLive3_A = Gets.GetBuildingBrush("A_1_Live_3");
                public static ImageBrush BuildMoney1_A = Gets.GetBuildingBrush("A_2_Money_1");
                public static ImageBrush BuildMoney2_A = Gets.GetBuildingBrush("A_2_Money_2");
                public static ImageBrush BuildMoney3_A = Gets.GetBuildingBrush("A_2_Money_3");
                public static ImageBrush BuildMetal1_A = Gets.GetBuildingBrush("A_4_Metall_1");
                public static ImageBrush BuildMetal2_A = Gets.GetBuildingBrush("A_4_Metall_2");
                public static ImageBrush BuildMetal3_A = Gets.GetBuildingBrush("A_4_Metall_3");
                public static ImageBrush BuildMetCap1_A = Gets.GetBuildingBrush("A_5_MetCap_1");
                public static ImageBrush BuildMetCap2_A = Gets.GetBuildingBrush("A_5_MetCap_2");
                public static ImageBrush BuildMetCap3_A = Gets.GetBuildingBrush("A_5_MetCap_3");
                public static ImageBrush BuildChip1_A = Gets.GetBuildingBrush("A_6_Chip_1");
                public static ImageBrush BuildChip2_A = Gets.GetBuildingBrush("A_6_Chip_2");
                public static ImageBrush BuildChip3_A = Gets.GetBuildingBrush("A_6_Chip_3");
                public static ImageBrush BuildChipCap1_A = Gets.GetBuildingBrush("A_7_ChipCap_1");
                public static ImageBrush BuildChipCap2_A = Gets.GetBuildingBrush("A_7_ChipCap_2");
                public static ImageBrush BuildChipCap3_A = Gets.GetBuildingBrush("A_7_ChipCap_3");
                public static ImageBrush BuildAnti1_A = Gets.GetBuildingBrush("A_8_Anti_1");
                public static ImageBrush BuildAnti2_A = Gets.GetBuildingBrush("A_8_Anti_2");
                public static ImageBrush BuildAnti3_A = Gets.GetBuildingBrush("A_8_Anti_3");
                public static ImageBrush BuildAntiCap1_A = Gets.GetBuildingBrush("A_9_AntiCap_1");
                public static ImageBrush BuildAntiCap2_A= Gets.GetBuildingBrush("A_9_AntiCap_2");
                public static ImageBrush BuildAntiCap3_A = Gets.GetBuildingBrush("A_9_AntiCap_3");
                public static ImageBrush BuildRepair1_A = Gets.GetBuildingBrush("A_3_Repair_1");
                public static ImageBrush BuildRepair2_A = Gets.GetBuildingBrush("A_3_Repair_2");
                public static ImageBrush BuildRepair3_A = Gets.GetBuildingBrush("A_3_Repair_3");
                public static ImageBrush BuildWar0_A = Gets.GetBuildingBrush("A_10_WarBase_1");
                public static ImageBrush BuildWar1_A = Gets.GetBuildingBrush("A_10_WarBase_2");
                public static ImageBrush BuildWar2_A = Gets.GetBuildingBrush("A_10_WarBase_3");
                public static ImageBrush BuildWar3 = Gets.GetBuildingBrush("Build103");
                public static ImageBrush BuildInfo2 = Gets.GetBuildingBrush("Build992");
                public static ImageBrush BuildInfo1 = Gets.GetBuildingBrush("Build991");
                public static ImageBrush BuildInfo3 = Gets.GetBuildingBrush("Build993");
                public static ImageBrush Back1 = Gets.GetBuildingBrush("Back1");
            }
            public static DrawingBrush EquipmentBrush = Pictogram.GetEquipmentBrush();
            public static DrawingBrush ShipMoveBrush = Pictogram.GetShipMoveBrush();

            public static SortedList<WeaponGroup, DrawingBrush> WeaponGroupBrush = Pictogram.GetWeaponGroupBrushes(255);
            public static SortedList<EWeaponType, Brush> WeaponsBrushes = WeapCanvas.GetBrushes();

            //public static SortedList<EWeaponType, ImageBrush> WeaponBrushes = new SortedList<EWeaponType, ImageBrush>();
            //public static SortedList<EquipmentType, ImageBrush> EquipmentBrushes = new SortedList<EquipmentType, ImageBrush>();

            public static SolidColorBrush BuildingBrush = System.Windows.Media.Brushes.Brown;
            public static SolidColorBrush ShipTypeBrush = new SolidColorBrush(Color.FromArgb(255, 255, 144, 255));
            public static SolidColorBrush WeaponBrush = System.Windows.Media.Brushes.Red;
            public static SolidColorBrush OtherBrush = System.Windows.Media.Brushes.Gray;

            //Флаг
            public static ImageBrush ColonizationBrush = Gets.AddPic("S039_Colonization");
            //public static DrawingBrush ColonizationBrush = Pictogram.GetColonizationPictBrush(); //Колонизация новой колонии
            //Маска бетмена
            public static DrawingBrush AttackEnemyBrush = Pictogram.GetAttackEnemyBrush(); //Уничтожение вторжения
            //Кубок
            public static ImageBrush ResourceMissionBrush = Gets.AddPic("S040_Mission");
            //public static DrawingBrush ResourceMissionBrush=Pictogram.GetResourceMissionBrush(); //Ресурсная миссия
            public static ImageBrush FleetScoutBrush = Gets.AddPic("S042_FleetScout");
            //public static DrawingBrush FleetFreeBrush = Pictogram.GetFreeFleetBrush();
            //Щит
            public static ImageBrush FleetDefenseBrush = Gets.AddPic("S037_FleetDefense");
            //public static DrawingBrush FleetDefenseBrush = Pictogram.GetDefenseLandPictBrush(); //Защита
            //Маска грабежа
            public static ImageBrush FleetPillageBrush = Gets.AddPic("S036_Pillage");
            //public static DrawingBrush FleetPillageBrush = Pictogram.GetPillageEnemyBrush(); //Грабёж
            //Пламя
            public static ImageBrush FleetConquerBrush = Gets.AddPic("S038_Conquer");
            //public static DrawingBrush FleetConquerBrush = Pictogram.GetFleetConquerBrush(); //Захват
            //Корона?
            public static ImageBrush FleetLastDefenderBrush = Gets.AddPic("S041_LastDefender");
            //public static DrawingBrush FleetLastDefenderBrush=Pictogram.GetLastDefenderBrush(); //Последний защитник

            //public static ImageBrush MeteoritBrush = new ImageBrush();
            public static SolidColorBrush SelectedPathBrush = new SolidColorBrush(Color.FromArgb(1, 255, 255, 255));

            //public static ImageBrush LandT1 = new ImageBrush();

            public static RadialGradientBrush Glass1 = Common.GetRadialBrush(Colors.Black, 0.8, 0.3);
            public static RadialGradientBrush Glass2 = Common.GetRadialBrush(Colors.Red, 0.8, 0.3);
            public static RadialGradientBrush Glass3 = Common.GetRadialBrush(Colors.Blue, 0.8, 0.3);
            public static RadialGradientBrush Glass4 = Common.GetRadialBrush(Colors.Green, 0.8, 0.3);
            public static RadialGradientBrush Glass5 = Common.GetRadialBrush(Colors.Violet, 0.8, 0.3);
            public static RadialGradientBrush Glass6 = Common.GetRadialBrush(Colors.Yellow, 0.8, 0.3);
            public static RadialGradientBrush Glass7 = Common.GetRadialBrush(Colors.LightBlue, 0.8, 0.3);
            public static RadialGradientBrush Glass8 = Common.GetRadialBrush(Colors.Orange, 0.8, 0.3);
            public static RadialGradientBrush Glass9 = Common.GetRadialBrush(Colors.Purple, 0.8, 0.3);
            public static RadialGradientBrush Glass10 = Common.GetRadialBrush(Colors.Teal, 0.8, 0.3);
            public static RadialGradientBrush Glass11 = Common.GetRadialBrush(Colors.Silver, 0.8, 0.3);
            public static RadialGradientBrush Glass12 = Common.GetRadialBrush(Colors.Navy, 0.8, 0.3);
            public static RadialGradientBrush Glass13 = Common.GetRadialBrush(Colors.YellowGreen, 0.8, 0.3);
            public static RadialGradientBrush Glass14 = Common.GetRadialBrush(Colors.SandyBrown, 0.8, 0.3);
            public static RadialGradientBrush Glass15 = Common.GetRadialBrush(Colors.SkyBlue, 0.8, 0.3);
            public static RadialGradientBrush Glass16 = Common.GetRadialBrush(Colors.DarkGray, 0.8, 0.3);

            public static List<ImageBrush> PlanetsBrushes = new List<ImageBrush>();
            //public static List<ImageBrush> LandTextures = new List<ImageBrush>();
            //public static ImageBrush Premium_Blank = new ImageBrush();

            public static ImageBrush SpaceBack = Gets.AddPicB2("B_001");
            public static ImageBrush SpaceMiddle = Gets.AddPicB2("kosmos4");


            public static DrawingBrush AlienBrush = Pictogram.GetAlienBrush();
            public static DrawingBrush PirateBrush = Pictogram.GetPirateBrush();
            public static DrawingBrush GreenTeamBrush = Pictogram.GetGreenTeamBrush();
            public static DrawingBrush TechTeamBrush = Pictogram.GetTechTeamBrush();
            public static DrawingBrush MercTeamBrush = Pictogram.GetMercTeamBrush();
            public static SortedList<EnemyType, DrawingBrush> EnemyBrushes = GetEnemyBrushes();
            public static SolidColorBrush Transparent = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));

            public static LinearGradientBrush NotSelectedMainBrush = Common.GetLinearBrush(Colors.DarkGray, Colors.Black);
            public static LinearGradientBrush SelectedMainBrush = Common.GetLinearBrush(Colors.SkyBlue, Colors.Navy);
            public static LinearGradientBrush DisabledMainBrush = Common.GetLinearBrush(Colors.Gray, Colors.LightGray);
            static SortedList<EnemyType, DrawingBrush> GetEnemyBrushes()
            {
                SortedList<EnemyType, DrawingBrush> list = new SortedList<EnemyType, DrawingBrush>();
                list.Add(EnemyType.GreenParty, GreenTeamBrush);
                list.Add(EnemyType.Pirates, PirateBrush);
                list.Add(EnemyType.Aliens, AlienBrush);
                list.Add(EnemyType.Techno, TechTeamBrush);
                list.Add(EnemyType.Mercenaries, MercTeamBrush);
                return list;
            }
        }
        public class Science
        {
            //public static int ScienceLevelMax = 2;
            public static SortedList<ushort, GameScience> GameSciences;
            public static SortedList<ushort, GameScience> NewLandsSciences=new SortedList<ushort, GameScience>();
            public static SciencePrice[] SciencePrices;
            //public static Random ScienceRandom;
            public enum EType { Building, ShipTypes, Generator, Shield, Computer, Engine, Weapon, Equipment, Other };
        }
        public class GameData
        {
            public static string GameData1 = "GameData.db";
            public static string BasicScienceGameData = "BasicScience.txt";
            public static string StarsGameData = "Stars.txt";
            public static string BasicStarFiguresGameData = "StarFigures.txt";
            public static string PlanetsGameData = "Planets.txt";
            public static string ShipTypesGameData = "ShipTypes.txt";
            public static string ComputerTypesGameData = "ComputerTypes.txt";
            public static string EngineTypesGameData = "EngineTypes.txt";
            public static string GeneratorTypesGameData = "GeneratorTypes.txt";
            public static string ShieldTypesGameData = "ShieldTypes.txt";
            public static string WeaponModifiersGameData = "WeaponModifiers.txt";
            public static string BasicShipArmorsGameData = "Armors.txt";
            public static string WeaponTypesGameData = "WeaponTypes.txt";
            public static string EquipmentTypesGameData = "Equipments.txt";
            public static string BasicSciencePriceGameData = "SciencePrices.txt";
            public static string BuildingsGameData = "Buildings.txt";
            public static string BasicMaxLandsCounts = "NewLandsCount.txt";
            public static string MissionResModificators = "MissionModificators.txt";

            public static GSArchive GameArchive1;
            public static GSArchive Sounds;
        }
        public static Color[] starColors = new Color[]{
            Color.FromRgb(66,170,255), 
            Color.FromRgb(175,238,238),
            Color.FromRgb(255,255,255),
            Color.FromRgb(242,232,201),
            Color.FromRgb(251,236,93),
            Color.FromRgb(244,196,48),
            Color.FromRgb(255,117,25),
            Colors.Purple};
        public static Brush[] starBrushes = new Brush[]{
                new SolidColorBrush(Color.FromRgb(66,170,255)), //Голубой O
                new SolidColorBrush(Color.FromRgb(175,238,238)), //Бледно-синий B
                new SolidColorBrush(Color.FromRgb(255,255,255)), //Белый A
                new SolidColorBrush(Color.FromRgb(242,232,201)), //Сливочный F
                new SolidColorBrush(Color.FromRgb(251,236,93)), //Кукурузный G
                new SolidColorBrush(Color.FromRgb(244,196,48)), //Шафрановый K
                new SolidColorBrush(Color.FromRgb(255,117,25)), //Тыквенный M
                new SolidColorBrush(Colors.Purple)}; 

        public class ZIndex
        {
            public static int Hexes = 0; //done
            public static int TopHexes = 2;
            public static int WarpFlash = 10; //done
            public static int Portals = 15; //done
            public static int Ships = 20; //done
            public static int Arrows = 25; //done
            public static int Aims = 30; //done
            public static int Shoots = 35; //done
            public static int Flashes = 40; //done
            public static int Effects = 45; //done
            public static int SmallTablets = 50; //done
            public static int SolarFlash = 55; //done
            public static int BigTablets = 60; //done
        }

    }
    delegate void ArtefactOnSectorDelegate(int landid, byte sectorpos);
}
