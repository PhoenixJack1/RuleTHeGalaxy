using System;
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
using System.Text.RegularExpressions;
using System.ServiceModel;
using System.Collections.Generic;

namespace Client
{
    public enum Login_Status { Off, Waiting_Login, Logging, Logged, Waiting_Sign, Signing };
    class Common
    {
        public static bool IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn,
                   @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
        }
        public static string CheckString(string text)
        {
            Queue<char> CharList = new Queue<char>();
            foreach (char label in text)
                if (label < 8470 && Crypt.decrypt_arr[0, label] != -1)
                    CharList.Enqueue(label);
            return new string(CharList.ToArray());
        }
        public static string LoadArchive1() //Загрузка файла с архивами №1
        {
            //DebugWindow.AddTB1("Start Load Archive\n");
            bool loadresult;
            if (!System.IO.File.Exists(Links.GameData.GameData1)) //если файла нет
            {
                if (Links.GameMode==EGameMode.Multi)
                    loadresult = Gets.LoadArchive1(); //то загружаем файл
                else return "Load Archive error";
                if (!loadresult) return "Load Archive error";
            }
            Links.GameData.GameArchive1 = new GSArchive(Links.GameData.GameData1, false);
            if (Links.GameData.GameArchive1.Status)
            {
                if (Links.GameMode == EGameMode.Multi)
                {
                    byte[] ServerHash = Gets.GetArchive1Hash();  //получение хеша с сервера
                    byte[] GameHash = GSArchive.GetHash(Links.GameData.GameData1); //расчёт хеша своего файла
                    if (ServerHash == null) return "Load Archive hash error";
                    bool hashsimilar = true; //сравнение хешей
                    if (ServerHash.Length != GameHash.Length)
                        hashsimilar = false;
                    else
                    {
                        for (int i = 0; i < ServerHash.Length; i++)
                            if (ServerHash[i] != GameHash[i]) { hashsimilar = false; break; }
                    }
                    if (!hashsimilar) //если хеши разные - то грузим архив
                    {
                        Links.GameData.GameArchive1 = null;
                        GC.Collect();
                        loadresult = Gets.LoadArchive1();
                        if (!loadresult) return "Load Archive error";
                        Links.GameData.GameArchive1 = new GSArchive(Links.GameData.GameData1, false);
                    }
                }
            }
            else
            {
                if (Links.GameMode == EGameMode.Multi)
                    loadresult = Gets.LoadArchive1();
                else
                    return "Load Archive error";
                if (!loadresult) return "Load Archive error";
                Links.GameData.GameArchive1 = new GSArchive(Links.GameData.GameData1, false);
            }
            //Gets.GetBasicSciences((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.BasicScienceGameData].File);
            //Gets.GetSciencePrices((string)Links.GameData.GameArchive1.Objects[Links.GameData.BasicSciencePriceGameData].File);
            Links.Stars = Gets.GetStars((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.StarsGameData].File);
            GSStar.FillNearPoints((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.BasicStarFiguresGameData].File);
            Gets.GetPlanets((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.PlanetsGameData].File);
            //Gets.GetShipTypes((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.ShipTypesGameData].File);
            //Gets.GetComputerTypes((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.ComputerTypesGameData].File);
            //Gets.GetEngineTypes((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.EngineTypesGameData].File);
            throw new Exception();
            //Gets.GetGeneratorTypes((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.GeneratorTypesGameData].File);
            Gets.GetWeaponModifiers((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.WeaponModifiersGameData].File);
            Gets.GetArmors((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.BasicShipArmorsGameData].File);
            Gets.GetWeaponTypes((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.WeaponTypesGameData].File);
            Gets.GetEquipmentTypes((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.EquipmentTypesGameData].File);
            //Gets.GetShieldTypes((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.ShieldTypesGameData].File);
            Gets.GetBasicBuildings((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.BuildingsGameData].File);
            GameScience.SetElement();
            GSGameInfo.BasicMaxLandsCount = BitConverter.ToInt32((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.BasicMaxLandsCounts].File, 0);
            //Mission.ResModificators = (byte[])Links.GameData.GameArchive1.Objects[Links.GameData.MissionResModificators].File;

            PilotPrepare(GSPilot.Nicks, "NickEn", "NickRu");
            PilotPrepare(GSPilot.JapCit, "JapCitEn", "JapCitRu");
            PilotPrepare(GSPilot.JapFem, "JapFemEn", "JapFemRu");
            PilotPrepare(GSPilot.JapMan, "JapManEn", "JapFemRu");
            PilotPrepare(GSPilot.JapSur, "JapSurEn", "JapSurRu");
            PilotPrepare(GSPilot.RuCit, "RuCitEn", "RuCitRu");
            PilotPrepare(GSPilot.RuFem, "RuFemEn", "RuFemRu");
            PilotPrepare(GSPilot.RuMan, "RuManEn", "RuManRu");
            PilotPrepare(GSPilot.RuSur, "RuSurEn", "RuSurRu");
            PilotPrepare(GSPilot.AmerCit, "AmerCitEn", "AmerCitRu");
            PilotPrepare(GSPilot.AmerFem, "AmerFemEn", "AmerFemRu");
            PilotPrepare(GSPilot.AmerMan, "AmerManEn", "AmerManRu");
            PilotPrepare(GSPilot.AmerSur, "AmerSurEn", "AmerSurRu");
            Gets.GetPics();
            CreateImageBrushes();
            return "";
        }
        
        public static void PilotPrepare(List<string[]> array, string En, string Ru)
        {
            string[] EnArray = ((string)Links.GameData.GameArchive1.Objects[En].File).Split('/');
            string[] RuArray = ((string)Links.GameData.GameArchive1.Objects[Ru].File).Split('/');
            for (int i = 0; i < EnArray.Length - 1; i++)
                array.Add(new string[] { EnArray[i], RuArray[i] });
        }
        public static void PilotPrepareLocal(List<string[]> array, string En, string Ru)
        {
            string[] EnArray = En.Split('/');
            string[] RuArray = Ru.Split('/');
            for (int i = 0; i < EnArray.Length - 1; i++)
                array.Add(new string[] { EnArray[i], RuArray[i] });
        }
        public static int StringToInteger(string text)
        {
            List<byte> b = new List<byte>();
            for (int i = 0; i < 2; i++)
                b.AddRange(BitConverter.GetBytes(text[i]));
            return BitConverter.ToInt32(b.ToArray(), 0);
        }
        public static bool StringToBool(string text)
        {
            return text == "1" ? true : false;
        }
        public static double StringToDouble(string text)
        {
            List<byte> b = new List<byte>();
            for (int i = 0; i < 4; i++)
                b.AddRange(BitConverter.GetBytes(text[i]));
            return BitConverter.ToDouble(b.ToArray(), 0);
        }
        public static void CreateLoginProxy()
        {
            System.ServiceModel.Channels.Binding binding = new BasicHttpBinding();
            BasicHttpBinding bind = (BasicHttpBinding)binding;
            bind.MaxReceivedMessageSize = 252428800;
            bind.ReaderQuotas.MaxArrayLength = Int32.MaxValue;
            //System.ServiceModel.Channels.Binding nettcpbinding = new NetTcpBinding();
            binding.CloseTimeout = TimeSpan.FromSeconds(5);

            EndpointAddress adress = new EndpointAddress(Links.CurrentServer);
            //EndpointAddress nettcpadress = new EndpointAddress("net.tcp://172.16.4.21/LoginService");
            Links.Loginproxy = ChannelFactory<ILoginInterface>.CreateChannel(binding, adress);

        }
        public static string GetNumber(int z)
        {
            switch (z)
            {
                case 0: return "First";
                case 1: return "Second";
                case 2: return "Third";
                case 3: return "Forth";
                default: return (z + 1).ToString() + "th";
            }
        }
        public static Label GetSizeLabel(ItemSize size)
        {
            Label lbl = new Label();
            switch (size)
            {
                case ItemSize.Small: lbl.Content = "S"; break;
                case ItemSize.Medium: lbl.Content = "M"; break;
                case ItemSize.Large: lbl.Content = "L"; break;
            }
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            lbl.Foreground = Brushes.White;
            lbl.FontWeight = FontWeights.Bold;
            return lbl;
        }
        public static string GetStringFromByteArray(byte[] array, int startIndex, int Length)
        {
            List<char> result = new List<char>();
            for (int i = startIndex; i < startIndex + Length * 2; i += 2)
            {
                if (i >= array.Length) break;
                result.Add(BitConverter.ToChar(array, i));
            }
            return new string(result.ToArray());
        }
        public static string GetStringFromByteArray(byte[] array, int startIndex)
        {
            List<char> result = new List<char>();
            for (int i = startIndex; i < array.Length; i += 2)
                result.Add(BitConverter.ToChar(array, i));

            return new string(result.ToArray());
        }
        public static byte[] StringToByte(string text)
        {
            List<byte> result = new List<byte>();
            foreach (char c in text)
                result.AddRange(BitConverter.GetBytes(c));
            return result.ToArray();
        }
        /*public static void CreateLoginBrushes()
        {
            Links.Brushes.Login.En0.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L001_En0.png"));
            Links.Brushes.Login.En1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L002_En1.png"));
            Links.Brushes.Login.Ru0.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L003_Ru0.png"));
            Links.Brushes.Login.Ru1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L004_Ru1.png"));
            Links.Brushes.Login.ExitE0.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L005_ExitE0.png"));
            Links.Brushes.Login.ExitE1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L006_ExitE1.png"));
            Links.Brushes.Login.ExitR0.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L007_ExitR0.png"));
            Links.Brushes.Login.ExitR1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L008_ExitR1.png"));
            Links.Brushes.Login.OnlineE0.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L009_OnlineE0.png"));
            Links.Brushes.Login.OnlineE1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L010_OnlineE1.png"));
            Links.Brushes.Login.OnlineR0.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L011_OnlineR0.png"));
            Links.Brushes.Login.OnlineR1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L012_OnlineR1.png"));
            Links.Brushes.Login.RandomE0.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L013_RandomE0.png"));
            Links.Brushes.Login.RandomE1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L014_RandomE1.png"));
            Links.Brushes.Login.RandomR0.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L015_RandomR0.png"));
            Links.Brushes.Login.RandomR1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L016_RandomR1.png"));
            Links.Brushes.Login.SingleE0.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L017_SingleE0.png"));
            Links.Brushes.Login.SingleE1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L018_SingleE1.png"));
            Links.Brushes.Login.SingleR0.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L019_SingleR0.png"));
            Links.Brushes.Login.SingleR1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L020_SingleR1.png"));
            Links.Brushes.Login.LogoGame.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L021_LogoGame.png"));
            Links.Brushes.Login.LogoStudio.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L022_LogoStudio.png"));
            Links.Brushes.Login.LogoBeta.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L023_LogoBeta.png"));
            Links.Brushes.Login.VK0.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L024_LogoVK0.png"));
            Links.Brushes.Login.VK1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GraphicLibrary;component/Images/Login/L025_LogoVK1.png"));
        }*/
        public static void CreateImageBrushes()
        {
            //Links.Brushes.MoneyImageBrush.ImageSource = Links.PicsList["R001_Money"];
            //Links.Brushes.MetalImageBrush.ImageSource = Links.PicsList["R002_Metal"];
            //Links.Brushes.ChipsImageBrush.ImageSource = Links.PicsList["R003_Chip"];
            //Links.Brushes.AntiImageBrush.ImageSource=Links.PicsList["R004_Anti"];
            //Links.Brushes.ZipImageBrush.ImageSource = Links.PicsList["R005_Zip"];
            //Links.Brushes.MoneyCapBrush.ImageSource = Links.PicsList["R006_MoneyCap"];
            //Links.Brushes.MetalCapBrush.ImageSource = Links.PicsList["R007_MetalCap"];
            //Links.Brushes.ChipCapBrush.ImageSource = Links.PicsList["R008_ChipCap"];
            //Links.Brushes.AntiCapBrush.ImageSource = Links.PicsList["R009_AntiCap"];
            //Links.Brushes.ZipCapBrush.ImageSource = Links.PicsList["R010_ZipCap"];
            //Links.Brushes.MathPowerBrush.ImageSource = Links.PicsList["R011_MathPower"];
            //Links.Brushes.BuildingSizeBrush.ImageSource = Links.PicsList["R012_Size"];
            //Links.Brushes.PeopleImageBrush.ImageSource = Links.PicsList["R013_Peoples"];
            //Links.Brushes.ExperienceBrush.ImageSource = Links.PicsList["R014_Experience"];
           // Links.Brushes.RusFlagBrush.ImageSource = Links.PicsList["R015_RusFlag"];
            //Links.Brushes.JapFlagBrush.ImageSource = Links.PicsList["R016_JapFlag"];
            //Links.Brushes.USAFlagBrush.ImageSource = Links.PicsList["R017_USAFlag"];
            Links.Brushes.PlanetTypeBrushes.Add(PlanetTypes.Green, Gets.AddPic("Planet_Green"));
            Links.Brushes.PlanetTypeBrushes.Add(PlanetTypes.Burned, Gets.AddPic("Planet_Burn"));
            Links.Brushes.PlanetTypeBrushes.Add(PlanetTypes.Freezed, Gets.AddPic("Planet_Ice"));
            Links.Brushes.PlanetTypeBrushes.Add(PlanetTypes.Gas, Gets.AddPic("Planet_Gas"));

            //Links.Brushes.SelectImageBrush.ImageSource = Links.PicsList["S001_Select"];

            //Links.Brushes.EmptyImageBrush.ImageSource = Links.PicsList["S029_Empty"];

            //Links.Brushes.TradeSector.ImageSource = Links.PicsList["B001_TS"];
            //Links.Brushes.Bank.ImageSource = Links.PicsList["B002_BA"];
            //Links.Brushes.Bank.ImageSource = GraphicLibrary.Graphic.GetImage2();
            //Links.Brushes.Mine.ImageSource = Links.PicsList["B003_MI"];
            //Links.Brushes.ChipsFactory.ImageSource = Links.PicsList["B004_CF"];
            //Links.Brushes.University.ImageSource = Links.PicsList["B005_UN"];
            //Links.Brushes.ParticipleAccelerator.ImageSource = Links.PicsList["B006_PA"];
            //Links.Brushes.ScienceSector.ImageSource = Links.PicsList["B007_SC"];
            //Links.Brushes.Manufacture.ImageSource = Links.PicsList["B008_MA"];
            //Links.Brushes.RepairingBay.ImageSource = Links.PicsList["B009_RB"];
            //Links.Brushes.Radar.ImageSource = Links.PicsList["B010_RS"];
            //Links.Brushes.DataCenter.ImageSource = Links.PicsList["B011_DC"];

            //Links.Brushes.LandT1.ImageSource = Links.PicsList["LandT1"];

            //Links.Brushes.HexSmallImage.ImageSource = Links.PicsList["S030_Hex"];
            //Links.Brushes.TwoSwords.ImageSource = Links.PicsList["S031_TwoSwords"];

            //Links.Brushes.Helper0.ImageSource = Links.PicsList["S032_Helper0"];
            //Links.Brushes.Helper1.ImageSource = Links.PicsList["S033_Helper1"];
            //Links.Brushes.Helper2_1.ImageSource = Links.PicsList["S043_Helper21"];
            //Links.Brushes.Helper2_2.ImageSource = Links.PicsList["S044_Helper22"];
            //Links.Brushes.LandIconBrush.ImageSource = Links.PicsList["S035_Land"];
            //Links.Brushes.FleetPillageBrush.ImageSource = Links.PicsList["S036_Pillage"];
            //Links.Brushes.FleetDefenseBrush.ImageSource = Links.PicsList["S037_FleetDefense"];
            //Links.Brushes.FleetConquerBrush.ImageSource = Links.PicsList["S038_Conquer"];
            //Links.Brushes.ColonizationBrush.ImageSource = Links.PicsList["S039_Colonization"];
            //Links.Brushes.ResourceMissionBrush.ImageSource = Links.PicsList["S040_Mission"];
            //Links.Brushes.FleetLastDefenderBrush.ImageSource = Links.PicsList["S041_LastDefender"];
            //Links.Brushes.FleetFreeBrush.ImageSource = Links.PicsList["S042_FleetFree"];
            //Links.Brushes.ArmorSelect.ImageSource = Links.PicsList["S048_ArmorSelect"];
            //Links.Brushes.ComputerBrush.ImageSource = Links.PicsList["S049_AimComputer"];
            //Links.Brushes.GeneratorBrush.ImageSource = Links.PicsList["S050_EnergyGenerator"];
            //Links.Brushes.EngineBrush.ImageSource = Links.PicsList["S051_Engine"];
            //Links.Brushes.ShieldBrush.ImageSource = Links.PicsList["S052_ShieldGenerator"];
            //Links.Brushes.ShipCreate.ImageSource = Links.PicsList["S053_ShipCreate"];

            //Links.Brushes.SpaceBack.ImageSource = Links.PicsList["B_001"];

            //Links.Brushes.MeteoritBrush.ImageSource = Links.PicsList["S014_Meteorit"];
            //for (int i = 1; i < 10; i++)
            //    Links.Brushes.Ships.Meteorits.Add(Gets.AddPicMeteor(string.Format("M{0:000}_M{0}", i)));

            //Links.Brushes.Interface.MainImage.ImageSource = Links.PicsList["I001_Main"];
            //Links.Brushes.Interface.DownComp.ImageSource = Links.PicsList["I002_DownComp"];
            //Links.Brushes.Interface.LeftCorner.ImageSource = Links.PicsList["I003_LeftCorner"];
            //Links.Brushes.Interface.RightCorner.ImageSource = Links.PicsList["I004_RightCorner"];
            //Links.Brushes.Interface.Chat.ImageSource = Links.PicsList["I005_Chat"];
            //Links.Brushes.Interface.Gear.ImageSource = Links.PicsList["I006_Gear"];
            //Links.Brushes.Interface.ResBlank.ImageSource = Links.PicsList["I007_ResBlank"];
            //Links.Brushes.Interface.MBYgolokLeft.ImageSource = Links.PicsList["I008_YgolokLeft"];
            //Links.Brushes.Interface.MBYgolorRight.ImageSource = Links.PicsList["I009_YgolokRight"];
            //Links.Brushes.Interface.MBShester1.ImageSource = Links.PicsList["I010_Shester1"];
            //Links.Brushes.Interface.MBShester2.ImageSource = Links.PicsList["I011_Shester2"];
            //Links.Brushes.Interface.MBKlapon.ImageSource = Links.PicsList["I012_Klapon"];
            //Links.Brushes.Interface.MBShluz.ImageSource = Links.PicsList["I013_Shluz"];
            //Links.Brushes.Interface.MBStvorkaLeft.ImageSource = Links.PicsList["I014_StvorkaLeft"];
            //Links.Brushes.Interface.MBStvorkaRight.ImageSource = Links.PicsList["I015_StvorkaRight"];
            //Links.Brushes.Interface.MBButtonBack.ImageSource = Links.PicsList["I016_ButtonBack"];
            //Links.Brushes.Interface.RamkaSmall.ImageSource = Links.PicsList["I017_RamkaSmall"];
            //Links.Brushes.Interface.RamkaSmall2.ImageSource = Links.PicsList["I018_RamkaSmall2"];
            //Links.Brushes.Interface.LeftArrow.ImageSource = Links.PicsList["I019_LeftArrow"];
            //Links.Brushes.Interface.RightArrow.ImageSource = Links.PicsList["I020_RightArrow"];
            //Links.Brushes.Interface.TextPanel.ImageSource = Links.PicsList["I021_TextPanel"];
            //Links.Brushes.Interface.Shesternya.ImageSource = Links.PicsList["I022_Shesternya"];
            //Links.Brushes.Interface.Zakryvashka.ImageSource = Links.PicsList["I023_Zakryvashka"];
            //Links.Brushes.Interface.ResBack.ImageSource = Links.PicsList["I024_ResBack"];
            //Links.Brushes.Interface.SchemaGenerator.ImageSource = Links.PicsList["I025_SchemaGenerator"];
            //Links.Brushes.Interface.BigSquare.ImageSource = Links.PicsList["I026_BigSquare"];
            //Links.Brushes.Interface.SmallSquare.ImageSource = Links.PicsList["I027_SmallSquare"];
            //Links.Brushes.Interface.WeaponRound.ImageSource = Links.PicsList["I028_WeaponRound"];
            //Links.Brushes.Interface.Refresh.ImageSource = Links.PicsList["I029_Refresh"];
            //Links.Brushes.Interface.AddPanel.ImageSource = Links.PicsList["I030_AddPanel"];
            //Links.Brushes.Interface.AddPanelSmall.ImageSource = Links.PicsList["I031_AddPanelSmall"];
            //Links.Brushes.Interface.BigSchema.ImageSource = Links.PicsList["I032_BigSchema"];

           // Links.Brushes.Interface.ClearSchema.ImageSource = Links.PicsList["I033_ClearSchema"];
            //Links.Brushes.Interface.SpeedShip.ImageSource = Links.PicsList["I034_SpeedShip"];
            //Links.Brushes.Interface.DamageShip.ImageSource = Links.PicsList["I035_DamageShip"];
            //Links.Brushes.Interface.TankShip.ImageSource = Links.PicsList["I036_TankShip"];
            //Links.Brushes.Interface.SupportShip.ImageSource = Links.PicsList["I037_SupportShip"];
            //Links.Brushes.Interface.LoadSchema.ImageSource = Links.PicsList["I038_LoadSchema"];
            //Links.Brushes.Interface.SchemaBack.ImageSource = Links.PicsList["I039_SchemaBack"];
            //Links.Brushes.Interface.SchemaBack2.ImageSource = Links.PicsList["I061_SchemaBack2"];
            //Links.Brushes.Interface.ImageBack.ImageSource = Links.PicsList["I054_ImageBack"];

            //Links.Brushes.Interface.RamkaMedium.ImageSource = Links.PicsList["I062_RamkaMedium"];
            //Links.Brushes.Interface.ButtonMedium.ImageSource = Links.PicsList["I063_Button"];
            //Links.Brushes.Interface.RamkaWeapon.ImageSource = Links.PicsList["I064_RamkaWeapon"];

            //Links.Brushes.Interface.ParamBtnR1.ImageSource = Links.PicsList["I055_PB1"];
            //Links.Brushes.Interface.ParamBtnR2.ImageSource = Links.PicsList["I056_PB2"];
            //Links.Brushes.Interface.ParamBtnR3.ImageSource = Links.PicsList["I057_PB3"];
            //Links.Brushes.Interface.ParamBtnR4.ImageSource = Links.PicsList["I058_PB4"];
            //Links.Brushes.Interface.ParamBtnS1.ImageSource = Links.PicsList["I059_PBS1"];
            //Links.Brushes.Interface.ParamBtnS2.ImageSource = Links.PicsList["I060_PBS2"];

            //Links.Brushes.Interface.ShipCreateBorder.ImageSource = Links.PicsList["I040_ShipCreateBorder"];
            //Links.Brushes.Interface.ShipCreateLArrow.ImageSource = Links.PicsList["I041_ShipCreateLArrow"];
            //Links.Brushes.Interface.ShipCreareRArrow.ImageSource = Links.PicsList["I042_ShipCreateRArrow"];
            //Links.Brushes.Interface.ShipCreateR1.ImageSource = Links.PicsList["I043_ShipCreateR1"];
            //Links.Brushes.Interface.ShipCreateR2.ImageSource = Links.PicsList["I044_ShipCreateR2"];
            //Links.Brushes.Interface.ShipCreateR3.ImageSource = Links.PicsList["I045_ShipCreateR3"];
            //Links.Brushes.Interface.ShipCreateR4.ImageSource = Links.PicsList["I046_ShipCreateR4"];
            //Links.Brushes.Interface.ShipCreateR5.ImageSource = Links.PicsList["I047_ShipCreateR5"];
            //Links.Brushes.Interface.ShipCreateR6.ImageSource = Links.PicsList["I048_ShipCreateR6"];
            //Links.Brushes.Interface.ShipCreateP1.ImageSource = Links.PicsList["I049_ShipCreateP1"];
            //Links.Brushes.Interface.ShipCreateP2.ImageSource = Links.PicsList["I050_ShipCreateP2"];
            //Links.Brushes.Interface.ShipCreateP3.ImageSource = Links.PicsList["I051_ShipCreateP3"];
            //Links.Brushes.Interface.ShipCreateP4.ImageSource = Links.PicsList["I052_ShipCreateP4"];
            //Links.Brushes.Interface.ShipCreateP5.ImageSource = Links.PicsList["I053_ShipCreateP5"];

            Links.Brushes.Items.GeneratorBrushes.Add(ItemSize.Small, Gets.AddPicGI("GI001_GeneratorS"));
            Links.Brushes.Items.GeneratorBrushes.Add(ItemSize.Medium, Gets.AddPicGI("GI002_GeneratorM"));
            Links.Brushes.Items.GeneratorBrushes.Add(ItemSize.Large, Gets.AddPicGI("GI003_GeneratorL"));
            Links.Brushes.Items.ComputerBrushes.Add(ItemSize.Small, Gets.AddPicGI("GI004_ComputerS"));
            Links.Brushes.Items.ComputerBrushes.Add(ItemSize.Medium, Gets.AddPicGI("GI005_ComputerM"));
            Links.Brushes.Items.ComputerBrushes.Add(ItemSize.Large, Gets.AddPicGI("GI006_ComputerL"));
            Links.Brushes.Items.EngineBrushes.Add(ItemSize.Small, Gets.AddPicGI("GI007_EngineS"));
            Links.Brushes.Items.EngineBrushes.Add(ItemSize.Medium, Gets.AddPicGI("GI008_EngineM"));
            Links.Brushes.Items.EngineBrushes.Add(ItemSize.Large, Gets.AddPicGI("GI009_EngineL"));
            Links.Brushes.Items.ShieldBrushes.Add(ItemSize.Small, Gets.AddPicGI("GI010_ShieldS"));
            Links.Brushes.Items.ShieldBrushes.Add(ItemSize.Medium, Gets.AddPicGI("GI011_ShieldM"));
            Links.Brushes.Items.ShieldBrushes.Add(ItemSize.Large, Gets.AddPicGI("GI012_ShieldL"));
            //Links.Brushes.Items.BorderBlue.ImageSource = Links.PicsList["GI013_BorderBlue"];
            //Links.Brushes.Items.BorderBlueE.ImageSource = Links.PicsList["GI014_BorderBlueE"];
            //Links.Brushes.Items.BorderBlueD.ImageSource = Links.PicsList["GI015_BorderBlueD"];
            //Links.Brushes.Items.BorderBlueW.ImageSource = Links.PicsList["GI016_BorderBlueW"];
            //Links.Brushes.Items.BorderPinkW.ImageSource = Links.PicsList["GI017_BorderPinkW"];
            //Links.Brushes.Items.BorderRedW.ImageSource = Links.PicsList["GI018_BorderRedW"];
            //Links.Brushes.Items.BorderGreenW.ImageSource = Links.PicsList["GI019_BorderGreenW"];
            Links.Brushes.Items.SpeedSize.Add(ItemSize.Large, Gets.AddPicGI("GI020_SpeedL"));
            Links.Brushes.Items.SpeedSize.Add(ItemSize.Medium, Gets.AddPicGI("GI021_SpeedM"));
            Links.Brushes.Items.SpeedSize.Add(ItemSize.Small, Gets.AddPicGI("GI022_SpeedS"));
            //Links.Brushes.Items.EnSpent.ImageSource = Links.PicsList["GI023_EnergySpent"];
            //Links.Brushes.Items.BorderGreen.ImageSource = Links.PicsList["GI024_BorderGreen"];
            //Links.Brushes.Items.BorderRed.ImageSource = Links.PicsList["GI025_BorderRed"];
            //Links.Brushes.Items.BorderPink.ImageSource = Links.PicsList["GI026_BorderPink"];
            //Links.Brushes.Items.BorderGold.ImageSource = Links.PicsList["GI027_BorderGold"];
            //Links.Brushes.Items.BorderShield.ImageSource = Links.PicsList["GI028_BorderShield"];
            //Links.Brushes.Items.BorderGenerator.ImageSource = Links.PicsList["GI029_BorderGenerator"];
            //Links.Brushes.Items.BorderGenerator2.ImageSource = Links.PicsList["GI030_BorderGenerator2"];
            //Links.Brushes.Items.BorderEngine.ImageSource = Links.PicsList["GI031_BorderEngine"];
            //Links.Brushes.Items.BorderEngine2.ImageSource = Links.PicsList["GI032_BorderEngine2"];
            //Links.Brushes.Items.ShipSchema3.ImageSource = Links.PicsList["GI033_ShipSchema3"];
            //Links.Brushes.Items.ShipSchema2.ImageSource = Links.PicsList["GI034_ShipSchema2"];
            //Links.Brushes.Items.ShipSchema1.ImageSource = Links.PicsList["GI035_ShipSchema1"];
            //Links.Brushes.Items.BorderBuilding.ImageSource = Links.PicsList["GI036_BorderBuilding"];

            //Links.Brushes.Interface.BattleMainImage.ImageSource = Links.PicsList["BI001_Main"];
            //Links.Brushes.Interface.BTNConfirm.ImageSource = Links.PicsList["BI002_Confirm"];
            //Links.Brushes.Interface.BTNCancel.ImageSource = Links.PicsList["BI003_Cancel"];
            //Links.Brushes.Interface.BTNCancelX.ImageSource = Links.PicsList["BI004_CancelX"];
            //Links.Brushes.Interface.AutoPlayer.ImageSource = Links.PicsList["BI005_Player"];
            //Links.Brushes.Interface.AutoComputer.ImageSource = Links.PicsList["BI006_Computer"];
            //Links.Brushes.Interface.BTNM.ImageSource = Links.PicsList["BI007_BTNM"];
            //Links.Brushes.Interface.BTNL.ImageSource = Links.PicsList["BI008_BTNL"];
            //Links.Brushes.Interface.BTNR.ImageSource = Links.PicsList["BI009_BTNR"];
            //Links.Brushes.Interface.BTNA.ImageSource = Links.PicsList["BI010_BTNA"];
            //Links.Brushes.Interface.ScrollUp.ImageSource = Links.PicsList["BI011_ScrollUp"];
            //Links.Brushes.Interface.ScrollDown.ImageSource = Links.PicsList["BI012_ScrollDown"];
            //Links.Brushes.Interface.BattleZastavka.ImageSource = Links.PicsList["BI013_Zastavka"];

            //Links.Brushes.Colony.Colony1.ImageSource = Links.PicsList["Colony1"];
            //Links.Brushes.Colony.Colony2.ImageSource = Links.PicsList["Colony2"];
            //Links.Brushes.Colony.Colony3.ImageSource = Links.PicsList["Colony3"];
            //Links.Brushes.Colony.Colony4.ImageSource = Links.PicsList["Colony4"];
            //Links.Brushes.Colony.Colony5.ImageSource = Links.PicsList["Colony5"];
            //Links.Brushes.Colony.Colony6.ImageSource = Links.PicsList["Colony6"];
            //Links.Brushes.Colony.Colony7.ImageSource = Links.PicsList["Colony7"];
            //Links.Brushes.Colony.Colony8.ImageSource = Links.PicsList["Colony8"];
            //Links.Brushes.Colony.Colony9.ImageSource = Links.PicsList["Colony9"];

            //Links.Brushes.Ships.ShipShield.ImageSource = Links.PicsList["BI014_Shield"];
            //Links.Brushes.Interface.BattleInfo1.ImageSource = Links.PicsList["BI015_BTNInfo1"];
            //Links.Brushes.Interface.BattleInfo2.ImageSource = Links.PicsList["BI016_BTNInfo2"];
            //Links.Brushes.Interface.BattleConfirm1.ImageSource = Links.PicsList["BI017_BTNConfirm1"];
            //Links.Brushes.Interface.BattleConfirm2.ImageSource = Links.PicsList["BI018_BTNConfirm2"];
            //Links.Brushes.Interface.BattleAttack1.ImageSource = Links.PicsList["BI019_BTNAttack1"];
            //Links.Brushes.Interface.BattleAttack2.ImageSource = Links.PicsList["BI020_BTNAttack2"];
            //Links.Brushes.Interface.BattleAuto1.ImageSource = Links.PicsList["BI021_BTNAuto1"];

            //Links.Brushes.Science.WallUp.ImageSource = Links.PicsList["S001_WallUp"];
            //Links.Brushes.Science.WallDown.ImageSource = Links.PicsList["S002_WallDown"];
            /*Links.Brushes.Science.Glass.ImageSource = Links.PicsList["S003_Glass"];
            Links.Brushes.Science.Hand1.ImageSource = Links.PicsList["S004_Hand1"];
            Links.Brushes.Science.Hand2.ImageSource = Links.PicsList["S005_Hand2"];
            Links.Brushes.Science.Hand3.ImageSource = Links.PicsList["S006_Hand3"];
            Links.Brushes.Science.Hand4.ImageSource = Links.PicsList["S007_Hand4"];
            Links.Brushes.Science.WallUp2.ImageSource = Links.PicsList["S008_WallUp2"];
            Links.Brushes.Science.Round.ImageSource = Links.PicsList["S009_Round"];
            Links.Brushes.Science.STV1.ImageSource = Links.PicsList["S010_STV1"];
            Links.Brushes.Science.STV2.ImageSource = Links.PicsList["S011_STV2"];
            Links.Brushes.Science.Regirator.ImageSource = Links.PicsList["S012_Regirator"];
            Links.Brushes.Science.Button.ImageSource = Links.PicsList["S013_Button"];
            Links.Brushes.Science.Border.ImageSource = Links.PicsList["S014_Border"];*/

            /*Links.Brushes.Ships.Ship1.ImageSource = Links.PicsList["S001_Ship1"];
            Links.Brushes.Ships.Ship2.ImageSource = Links.PicsList["S002_Ship2"];
            Links.Brushes.Ships.Ship3.ImageSource = Links.PicsList["S003_Ship3"];
            Links.Brushes.Ships.Ship4.ImageSource = Links.PicsList["S004_Ship4"];
            Links.Brushes.Ships.Ship5.ImageSource = Links.PicsList["S005_Ship5"];
            Links.Brushes.Ships.Ship6.ImageSource = Links.PicsList["S006_Ship6"];
            Links.Brushes.Ships.Ship8.ImageSource = Links.PicsList["S008_Ship8"];
            Links.Brushes.Ships.Ship10.ImageSource = Links.PicsList["S010_Ship10"];

            Links.Brushes.Ships.ShipLR.ImageSource = Links.PicsList["S101_LR"];
            Links.Brushes.Ships.ShipMR.ImageSource = Links.PicsList["S102_MR"];
            Links.Brushes.Ships.ShipDR.ImageSource = Links.PicsList["S103_DR"];
            Links.Brushes.Ships.ShipLG.ImageSource = Links.PicsList["S201_LG"];
            Links.Brushes.Ships.ShipMG.ImageSource = Links.PicsList["S202_MG"];
            Links.Brushes.Ships.ShipDG.ImageSource = Links.PicsList["S203_DG"];
            Links.Brushes.Ships.ShipLB.ImageSource = Links.PicsList["S301_LB"];
            Links.Brushes.Ships.ShipMB.ImageSource = Links.PicsList["S302_MB"];
            Links.Brushes.Ships.ShipDB.ImageSource = Links.PicsList["S303_DB"];
            Links.Brushes.Ships.ShipLP.ImageSource = Links.PicsList["S401_LP"];
            Links.Brushes.Ships.ShipMP.ImageSource = Links.PicsList["S402_MP"];
            Links.Brushes.Ships.ShipDP.ImageSource = Links.PicsList["S403_DP"];
            Links.Brushes.Ships.ShipW.ImageSource = Links.PicsList["S501_W"];
            Links.Brushes.Ships.ShipG.ImageSource = Links.PicsList["S502_G"];
            Links.Brushes.Ships.ShipB.ImageSource = Links.PicsList["S503_B"];*/

            /* Links.Brushes.Ships.Portal1.ImageSource = Links.PicsList["SP01"];
             Links.Brushes.Ships.Portal2.ImageSource = Links.PicsList["SP02"];
             Links.Brushes.Ships.Portal3.ImageSource = Links.PicsList["SP03"];
             Links.Brushes.Ships.Portal4.ImageSource = Links.PicsList["SP04"];
             Links.Brushes.Ships.Portal5.ImageSource = Links.PicsList["SP05"];
             Links.Brushes.Ships.Portal6.ImageSource = Links.PicsList["SP06"];*/

            /* Links.Brushes.Guns.Gun1.ImageSource = Links.PicsList["G001_Gun1"];
             Links.Brushes.Guns.Gun2.ImageSource = Links.PicsList["G002_Gun2"];
             Links.Brushes.Guns.Gun3.ImageSource = Links.PicsList["G003_Gun3"];
             Links.Brushes.Guns.Gun4.ImageSource = Links.PicsList["G004_Gun4"];
             Links.Brushes.Guns.Gun5.ImageSource = Links.PicsList["G005_Gun5"];
             Links.Brushes.Guns.Gun6.ImageSource = Links.PicsList["G006_Gun6"];
             Links.Brushes.Guns.Gun7.ImageSource = Links.PicsList["G007_Gun7"];
             Links.Brushes.Guns.Gun8.ImageSource = Links.PicsList["G008_Gun8"];
             Links.Brushes.Guns.Gun9.ImageSource = Links.PicsList["G009_Gun9"];
             Links.Brushes.Guns.Gun10.ImageSource = Links.PicsList["G010_Gun10"];
             Links.Brushes.Guns.Gun11.ImageSource = Links.PicsList["G011_Gun11"];
             Links.Brushes.Guns.Gun12.ImageSource = Links.PicsList["G012_Gun12"];
             Links.Brushes.Guns.Gun13.ImageSource = Links.PicsList["G013_Gun13"];
             Links.Brushes.Guns.Gun14.ImageSource = Links.PicsList["G014_Gun14"];*/
            for (int i = 10; i < 40; i++)
                Links.Brushes.Ships.ModelNew1Brushes.Add(String.Format("S{0}", i), Gets.AddShipPic(String.Format("1/S{0}", i)));
            for (int i = 10; i < 40; i++)
                Links.Brushes.Ships.ModelNew2Brushes.Add(String.Format("S{0}", i), Gets.AddShipPic(String.Format("2/S{0}", i)));
            for (int i = 10; i < 40; i++)
                Links.Brushes.Ships.ModelNew3Brushes.Add(String.Format("S{0}", i), Gets.AddShipPic(String.Format("3/S{0}", i)));
            for (int i = 10; i < 40; i+=10)
                Links.Brushes.Ships.ModelNew4Brushes.Add(String.Format("S{0}", i), Gets.AddShipPic(String.Format("4/S{0}", i)));
            for (int i = 10; i < 40; i += 10)
                Links.Brushes.Ships.ModelNew5Brushes.Add(String.Format("S{0}", i), Gets.AddShipPic(String.Format("5/S{0}", i)));
            for (int i = 10; i < 40; i += 10)
                Links.Brushes.Ships.ModelNew6Brushes.Add(String.Format("S{0}", i), Gets.AddShipPic(String.Format("6/S{0}", i)));
            for (int i = 10; i < 40; i += 10)
                Links.Brushes.Ships.ModelNew7Brushes.Add(String.Format("S{0}", i), Gets.AddShipPic(String.Format("7/S{0}", i)));
            for (int i = 10; i < 40; i ++)
                Links.Brushes.Ships.ModelNew8Brushes.Add(String.Format("S{0}", i), Gets.AddShipPic(String.Format("8/S{0}", i)));
            for (int i = 10; i < 40; i++)
                Links.Brushes.Ships.ModelNew9Brushes.Add(String.Format("S{0}", i), Gets.AddShipPic(String.Format("9/S{0}", i)));
            for (int i = 10; i < 40; i+=10)
                Links.Brushes.Ships.ModelNew10Brushes.Add(String.Format("S{0}", i), Gets.AddShipPic(String.Format("10/S{0}", i)));
            for (int i = 10; i < 40; i += 10)
                Links.Brushes.Ships.ModelNew11Brushes.Add(String.Format("S{0}", i), Gets.AddShipPic(String.Format("11/S{0}", i)));
            for (int i = 1; i < 16; i++)
                Links.Brushes.Ships.ModelNew101Brushes.Add(String.Format("S{0}", i), Gets.AddShipPic(String.Format("101/S{0}", i)));
            for (int i = 1; i < 36; i++)
                Links.Brushes.Ships.ModelNew102Brushes.Add(String.Format("S{0}", i), Gets.AddShipPic(String.Format("102/S{0}", i)));
            for (int i = 1; i < 2; i++)
                Links.Brushes.Ships.ModelNew103Brushes.Add(String.Format("S{0}", i), Gets.AddShipPic(String.Format("103/S{0}", i)));
            for (int i = 1; i < 2; i++)
                Links.Brushes.Ships.ModelNew104Brushes.Add(String.Format("S{0}", i), Gets.AddShipPic(String.Format("104/S{0}", i)));
            for (int i = 1; i < 2; i++)
                Links.Brushes.Ships.ModelNew105Brushes.Add(String.Format("S{0}", i), Gets.AddShipPic(String.Format("105/S{0}", i)));
            for (int i = 1; i < 4; i++)
                Links.Brushes.Ships.ModelNew251Brushes.Add(String.Format("S{0}", i), Gets.AddShipPic(String.Format("251 (GreenTeam)/S{0}", i)));
            for (int i = 1; i < 4; i++)
                Links.Brushes.Ships.ModelNew252Brushes.Add(String.Format("S{0}", i), Gets.AddShipPic(String.Format("252 (Pirates)/S{0}", i)));
            for (int i = 1; i < 4; i++)
                Links.Brushes.Ships.ModelNew253Brushes.Add(String.Format("S{0}", i), Gets.AddShipPic(String.Format("253 (Aliens)/S{0}", i)));
            for (int i = 1; i < 4; i++)
                Links.Brushes.Ships.ModelNew254Brushes.Add(String.Format("S{0}", i), Gets.AddShipPic(String.Format("254 (Techs)/S{0}", i)));
            for (int i = 1; i < 4; i++)
                Links.Brushes.Ships.ModelNew255Brushes.Add(String.Format("S{0}", i), Gets.AddShipPic(String.Format("255 (Mercs)/S{0}", i)));

            for (int i = 610; i < 640; i++)
                Links.Brushes.Ships.Model6Brushes.Add(String.Format("S{0}", i), Gets.AddPicShipM6(String.Format("S{0}", i)));
            for (int i = 710; i < 740; i++)
                Links.Brushes.Ships.Model7Brushes.Add(String.Format("S{0}", i), Gets.AddPicShipM7(String.Format("S{0}", i)));
            for (int i = 910; i < 940; i++)
                Links.Brushes.Ships.Model9Brushes.Add(String.Format("S{0}", i), Gets.AddPicShipM9(String.Format("S{0}", i)));

           /* GlobalCanvasBtn.Brushes.Brush1.ImageSource = Links.PicsList["GCB_01"];
            GlobalCanvasBtn.Brushes.Brush2.ImageSource = Links.PicsList["GCB_02"];
            GlobalCanvasBtn.Brushes.Brush3.ImageSource = Links.PicsList["GCB_03"];
            GlobalCanvasBtn.Brushes.Brush4.ImageSource = Links.PicsList["GCB_04"];
            GlobalCanvasBtn.Brushes.Brush5.ImageSource = Links.PicsList["GCB_05"];
            GlobalCanvasBtn.Brushes.Brush6.ImageSource = Links.PicsList["GCB_06"];
            GlobalCanvasBtn.Brushes.Brush7.ImageSource = Links.PicsList["GCB_07"];*/
            //Links.Brushes.Interface.NationPanelBack.ImageSource = Links.PicsList["GCB_08"];
            for (int i = 1; i < 24; i++)
            {
                //string hash = String.Format("P_{0:00}", i);
                Links.Brushes.PlanetsBrushes.Add(Gets.AddPicP(i));
                //string hash = String.Format("P_{0:00}", i);
                //Links.Brushes.PlanetsBrushes[i - 1].ImageSource = Links.PicsList[hash];
            }
            /*for (int i = 1; i < 16; i++)
            {
                Links.Brushes.LandTextures.Add(new ImageBrush());
                string hash = String.Format("LandL{0}", i);
                Links.Brushes.LandTextures[i - 1].ImageSource = Links.PicsList[hash];
            }*/
            //Links.Brushes.ShipImageBrush.Drawing = Pictogram.ShipBrush().Drawing;

        }
        public static void SetSmalltextStyle()
        {
            Links.SmallTextStyle = new System.Windows.Style();
            Links.SmallTextStyle.Setters.Add(new System.Windows.Setter(Control.FontSizeProperty, 10.0));
            Links.SmallTextStyle.Setters.Add(new Setter(Control.ForegroundProperty, Brushes.Black));
            Links.SmallTextStyle.Setters.Add(new Setter(Control.FontWeightProperty, FontWeights.Bold));

            Links.TextStyle = new Style();
            Links.TextStyle.Setters.Add(new Setter(Control.FontSizeProperty, 22.0));
            Links.TextStyle.Setters.Add(new Setter(Control.ForegroundProperty, Brushes.Black));
            Links.TextStyle.Setters.Add(new Setter(Control.FontWeightProperty, FontWeights.Bold));
            Links.TextStyle.Setters.Add(new Setter(Control.FontFamilyProperty, Links.Font));
            Links.MediumTextStyle = new Style();
            Links.MediumTextStyle.Setters.Add(new Setter(Control.FontSizeProperty, 16.0));
            Links.MediumTextStyle.Setters.Add(new Setter(Control.ForegroundProperty, Brushes.Black));
            Links.MediumTextStyle.Setters.Add(new Setter(Control.FontWeightProperty, FontWeights.Bold));
            Links.MediumTextStyle.Setters.Add(new Setter(Control.FontFamilyProperty, Links.Font));
            Links.SmallTextStyleWhite = new System.Windows.Style();
            Links.SmallTextStyleWhite.Setters.Add(new System.Windows.Setter(Control.FontSizeProperty, 14.0));
            Links.SmallTextStyleWhite.Setters.Add(new Setter(Control.ForegroundProperty, Brushes.White));
            Links.SmallTextStyleWhite.Setters.Add(new Setter(Control.FontWeightProperty, FontWeights.Bold));
            Links.SmallTextStyleWhite.Setters.Add(new Setter(Control.HorizontalAlignmentProperty, HorizontalAlignment.Center));
            Links.SmallTextStyleWhite.Setters.Add(new Setter(Control.VerticalAlignmentProperty, VerticalAlignment.Center));

            LinearGradientBrush br = new LinearGradientBrush();
            br.StartPoint = new Point(0.5, 1);
            br.EndPoint = new Point(0.5, 0);
            br.GradientStops.Add(new GradientStop(Color.FromRgb(100, 100, 100), 0.0));
            br.GradientStops.Add(new GradientStop(Color.FromRgb(100, 100, 100), 0.3));
            br.GradientStops.Add(new GradientStop(Colors.Black, 0.3));
            br.GradientStops.Add(new GradientStop(Colors.Black, 0.7));
            br.GradientStops.Add(new GradientStop(Color.FromRgb(100, 100, 100), 0.7));
            br.GradientStops.Add(new GradientStop(Color.FromRgb(100, 100, 100), 1));

            Links.ButtonStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.Black));
            Links.ButtonStyle.Setters.Add(new Setter(Control.FontSizeProperty, 22.0));
            Links.ButtonStyle.Setters.Add(new Setter(Control.ForegroundProperty, Brushes.White));
            Links.ButtonStyle.Setters.Add(new Setter(Control.FontWeightProperty, FontWeights.Bold));
            Links.ButtonStyle.Setters.Add(new Setter(Control.FontFamilyProperty, Links.Font));


        }
        public static Inline GetItemSizeInline(string text, ItemSize size)
        {
            Span result = new Span(new Run(text));
            result.Inlines.Add(GetItemSizeInline(size));
            return result;
        }
        public static Inline GetItemSizeInline(ItemSize size)
        {
            InlineUIContainer cont = new InlineUIContainer();
            switch (size)
            {
                case ItemSize.Small: cont.Child = GetRectangle(30, Links.Brushes.SmallImageBrush); break;
                case ItemSize.Medium: cont.Child = GetRectangle(30, Links.Brushes.MediumImageBrush); break;
                case ItemSize.Large: cont.Child = GetRectangle(30, Links.Brushes.LargeImageBrush); break;
            }
            return cont;
        }
        public static Inline GetWeaponParamInline(int number, WeaponParams param)
        {
            Span Line = new Span();
            Run Caption = new Run(GetNumber(number) + " Weapon: ");
            Line.Inlines.Add(Caption);
            Run weapongroup = new Run(param.Group.ToString() + " ");
            switch (param.Group)
            {
                case WeaponGroup.Energy: weapongroup.Foreground = Brushes.Blue; break;
                case WeaponGroup.Physic: weapongroup.Foreground = Brushes.Red; break;
                case WeaponGroup.Irregular: weapongroup.Foreground = Brushes.Violet; break;
                case WeaponGroup.Cyber: weapongroup.Foreground = Brushes.LightGreen; break;
                case WeaponGroup.Any: weapongroup.Foreground = Brushes.Yellow; break;
            }
            Line.Inlines.Add(weapongroup);
            Line.Inlines.Add(GetItemSizeInline(param.Size));
            Line.Inlines.Add(new LineBreak());
            return Line;
        }
        public static string GetNumberString(long value)
        {
            if (value < 100000)
                return value.ToString();
            else if (value < 1000000)
            {
                long thousands = value / 1000;
                int k = (int)Math.Round(((double)(value - thousands * 1000)) / 100, 0);
                return thousands.ToString() + "K" + k.ToString();
            }
            else if (value < 1000000000)
            {
                long millions = value / 1000000;
                int k = (int)Math.Round(((double)(value - millions * 1000000)) / 100000, 0);
                return millions.ToString() + "M" + k.ToString();
            }
            else
            {
                long gigs = value / 1000000000;
                int k = (int)Math.Round(((double)(value - gigs * 1000000000)) / 100000000, 0);
                return gigs.ToString() + "G" + k.ToString();
            }
        }
        public static long GetAvanpostPrice(GSPlanet Planet)
        {
            long basesum = (long)(10 * (0.8 + Planet.MaxPopulation / 50 * 0.2) * (0.6 + Planet.Locations * 0.2) * (Math.Pow(GSGameInfo.PlayerLands.Count + GSGameInfo.PlayerAvanposts.Count, 2)));
            return basesum * 1000;
        }
        public static long GetAvanpostPrice(ServerPlanet Planet, GSPlayer player)
        {
            long basesum = (long)(10 * (0.8 + Planet.MaxPopulation / 50 * 0.2) * (0.6 + Planet.Locations * 0.2) * (Math.Pow(player.Lands.Count + player.NewLands.Count, 2)));
            return basesum * 1000;
        }
        public static Rectangle GetRectangle(double width, double height, Brush brush)
        {
            Rectangle result = new Rectangle();
            result.Width = width;
            result.Height = height;
            result.Fill = brush;
            result.VerticalAlignment = VerticalAlignment.Center;
            return result;
        }
        public static Rectangle GetRectangle(double size, Brush brush)
        {
            Rectangle result = new Rectangle();
            result.Width = size;
            result.Height = size;
            result.Fill = brush;
            result.VerticalAlignment = VerticalAlignment.Center;
            return result;
        }
        public static Ellipse GetEllipse(int size, Brush brush)
        {
            Ellipse result = new Ellipse();
            result.Width = size;
            result.Height = size;
            result.Fill = brush;
            result.VerticalAlignment = VerticalAlignment.Center;
            return result;
        }
        public static void DrawLine(Canvas canvas, int width, int top, int left, int length, Brush brush)
        {
            Line line = new Line();
            line.Stroke = brush;
            line.StrokeThickness = width;
            line.X1 = left; line.X2 = left+length;
            line.Y1 = top; line.Y2 = top;
            canvas.Children.Add(line);
        }
        public static Rectangle GetRectangle(int size, Brush brush, string tip)
        {
            Rectangle rect = GetRectangle(size, brush);
            rect.ToolTip = tip;
            return rect;
        }
        public static string[] GetLangStr(string En, string Ru)
        {
            return new string[] { En, Ru };
        }
        
        public static string GetThreeSymbol(double value)
        {
            double val = value;
            if (value < Math.Pow(10, 3))
                return value.ToString();
            else if (value < Math.Pow(10, 4)) //10k
                return Math.Round(val / Math.Pow(10, 3), 1).ToString() + "k";
            else if (value < Math.Pow(10, 5))//100k
                return Math.Round(val / Math.Pow(10, 3), 0).ToString() + "k";
            else if (value < Math.Pow(10, 7)) //10m
                return Math.Round(val / Math.Pow(10, 6), 1).ToString() + "M";
            else if (value < Math.Pow(10, 8)) //100m
                return Math.Round(val / Math.Pow(10, 6), 0).ToString() + "M";
            else if (value < Math.Pow(10, 10)) //10g
                return Math.Round(val / Math.Pow(10, 9), 1).ToString() + "G";
            else if (value < Math.Pow(10, 11))
                return Math.Round(val / Math.Pow(10, 9), 0).ToString() + "G";
            else if (value < Math.Pow(10, 13))
                return Math.Round(val / Math.Pow(10, 12), 1).ToString() + "T";
            else if (value < Math.Pow(10, 14))
                return Math.Round(val / Math.Pow(10, 12), 0).ToString() + "T";
            else
                return "XXX";
        }
        public static SortedList<WeaponGroup, string> GetWeaponGroupTip()
        {
            SortedList<WeaponGroup, string> list = new SortedList<WeaponGroup, string>();
            list.Add(WeaponGroup.Energy, "EnergyWeapon");
            list.Add(WeaponGroup.Physic, "PhysicWeapon");
            list.Add(WeaponGroup.Irregular, "IrregularWeapon");
            list.Add(WeaponGroup.Cyber, "CyberWeapon");
            list.Add(WeaponGroup.Any, "AllWeapon");
            return list;
        }
        public static Border CreateBorder(int width, int height, Brush stroke, int thickness, int margin)
        {
            Border border = new Border();
            border.Width = width;
            border.Height = height;
            border.BorderBrush = stroke;
            border.BorderThickness = new Thickness(thickness);
            border.Margin = new Thickness(margin);
            return border;
        }
        public static Border CreateBorder(int width, int height, Brush stroke, int thickness, int margin, Canvas canvas, int left, int top)
        {
            Border border = CreateBorder(width, height, stroke, thickness, margin);
            canvas.Children.Add(border);
            Canvas.SetLeft(border, left);
            Canvas.SetTop(border, top);
            return border;
        }
        public static Border CreateBorder(int width, Grid grid, int column, int row)
        {
            Border border = new Border();
            if (width != 0)
                border.Width = width;
            border.BorderBrush = Brushes.Black;
            border.BorderThickness = new Thickness(2);
            border.Margin = new Thickness(2);
            grid.Children.Add(border);
            Grid.SetRow(border, row);
            Grid.SetColumn(border, column);
            return border;
        }
        public static byte[] GetArray(byte[] array, int startindex, int length)
        {
            byte[] result = new byte[length];
            for (int i = 0; i < length; i++)
                result[i] = array[startindex + i];
            return result;
        }
        public static double CalcAngle(Point CenterPoint, Point TargetPoint)
        {
            double dx = TargetPoint.X - CenterPoint.X; double dy = CenterPoint.Y - TargetPoint.Y;
            double alpha;
            if (dy == 0)
                if (dx > 0) return 90; else return 270;
            if (dx >= 0 && dy > 0) alpha = Math.Atan(dx / dy) * 180 / Math.PI;
            else if (dx >= 0 && dy < 0) alpha = 180 + Math.Atan(dx / dy) * 180 / Math.PI;
            else if (dx <= 0 && dy < 0) alpha = 180 + Math.Atan(dx / dy) * 180 / Math.PI;
            else alpha = 360 + Math.Atan(dx / dy) * 180 / Math.PI;
            return alpha;
        }
        public static double CalcRotateTo(double newangle, double curangle)
        {
            double da = newangle - curangle;
            double result = newangle;
            if (da > 180) result = newangle - 360;
            else if (da < -180) result = 360 + newangle;
            return result;
        }
        public static byte[] CopyArray(byte[] array, int startindex, int length)
        {
            Queue<byte> result = new Queue<byte>();
            int end = startindex + length;
            for (int i = startindex; i < end; i++)
                result.Enqueue(array[i]);
            return result.ToArray();
        }
        public static TextBlock PutBlock(int size, Border border, string content)
        {
            TextBlock lbl = GetBlock(size, content);
            border.Child = lbl;
            return lbl;
        }
        public static TextBlock PutBlock(int size, Border border, string content, Brush brush)
        {
            TextBlock lbl = GetBlock(size, content);
            border.Child = lbl;
            lbl.Foreground = brush;
            return lbl;
        }
        public static TextBlock PutBlock(int size, Border border, string content, Brush brush, TextWrapping wrap)
        {
            TextBlock lbl = GetBlock(size, content);
            border.Child = lbl;
            lbl.Foreground = brush;
            lbl.TextWrapping = wrap;
            return lbl;
        }
        public static TextBlock PutBlock(int size, string content, Canvas canvas, int left, int top, int width)
        {
            TextBlock block = GetBlock(size, content);
            block.Foreground = Brushes.White;
            canvas.Children.Add(block);
            Canvas.SetLeft(block, left);
            Canvas.SetTop(block, top);
            block.Width = width;
            return block;
        }
        public static TextBlock GetBlock(int size, string content)
        {
            TextBlock lbl = new TextBlock();
            lbl.FontSize = size;
            lbl.FontFamily = Links.Font;
            lbl.FontWeight = FontWeights.Bold;
            lbl.Text = content;
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            lbl.VerticalAlignment = VerticalAlignment.Center;
            lbl.TextWrapping = TextWrapping.Wrap;
            lbl.TextAlignment = TextAlignment.Center;
            lbl.TextTrimming = TextTrimming.WordEllipsis;
            return lbl;
        }
        public static TextBlock GetBlock(int size, string content, Brush brush, Thickness margin)
        {
            TextBlock block = GetBlock(size, content);
            block.Foreground = brush;
            block.Margin = margin;
            return block;
        }
        public static TextBlock GetBlock (int size, string content, Brush brush, int width)
        {
            TextBlock block = GetBlock(size, content);
            block.Foreground = brush;
            block.Width = width;
            return block;
        }
        public static Border PutBorder(int width, int height, Brush brush, int thickness, int left, int top, Canvas canvas)
        {
            Border border = new Border();
            border.BorderBrush = brush;
            border.BorderThickness = new Thickness(thickness);
            border.Width = width;
            border.Height = height;
            canvas.Children.Add(border);
            Canvas.SetLeft(border, left);
            Canvas.SetTop(border, top);
            return border;
        }

        public static Rectangle PutRect(Grid grid, int row, int column, int size, Brush brush, string tip)
        {
            Rectangle rect = Common.GetRectangle(size, brush);
            grid.Children.Add(rect);
            Grid.SetRow(rect, row);
            Grid.SetColumn(rect, column);
            rect.ToolTip = tip;
            return rect;
        }
        public static Rectangle PutRect(Canvas canvas, int size, Brush brush, int left, int top, bool needborder)
        {
            Rectangle rect = Common.GetRectangle(size, brush);
            canvas.Children.Add(rect);
            Canvas.SetLeft(rect, left);
            Canvas.SetTop(rect, top);
            if (needborder) rect.Stroke = Brushes.Black;
            return rect;
        }
        public static Rectangle PutRect(Canvas canvas, int size, Brush brush, int left, int top, bool needborder, string tip)
        {
            Rectangle rect = Common.PutRect(canvas, size, brush,left,top,needborder);
            rect.ToolTip = tip;
            return rect;
        }
        public static Button PutButton(int left, int top, int width, string text, Canvas canvas)
        {
            Button btn = new Button();
            btn.Width = width;
            btn.Height = 40;
            btn.Content = text;
            btn.FontFamily = Links.Font;
            btn.FontSize = 20;
            canvas.Children.Add(btn);
            Canvas.SetLeft(btn, left);
            Canvas.SetTop(btn, top);
            btn.Background = Brushes.Black;
            btn.FontWeight = FontWeights.Bold;
            btn.Foreground = Brushes.White;
            return btn;

        }
        public static Label PutLabel(Canvas canvas, int left, int top, int size, string text, Brush brush)
        {
            Label lbl = PutLabel(canvas, left, top, size, text);
            lbl.Foreground = brush;
            return lbl;
        }
        public static Label PutLabel(Canvas canvas, int left, int top, int size, string text)
        {
            Label lbl = CreateLabel(size, text);
            canvas.Children.Add(lbl);
            Canvas.SetLeft(lbl, left);
            Canvas.SetTop(lbl, top);
            return lbl;
        }
        public static Label CreateLabel(int size, string text)
        {
            Label lbl = new Label();
            lbl.FontFamily = Links.Font;
            lbl.FontSize = size;
            lbl.Content = text;
            lbl.FontWeight = FontWeights.Bold;
            return lbl;
        }
        public static Label CreateLabel(int size, string text, Brush brush)
        {
            Label lbl = CreateLabel(size, text);
            lbl.Foreground = brush;
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            return lbl;
        }
        public static string ToUpper(string s)
        {
            return s[0].ToString().ToUpper() + s.Substring(1);
        }
        public static Rectangle GetSizeRect(int size, ItemSize itemsize, bool needborder)
        {
            Brush brush; string tip;
            if (itemsize == ItemSize.Small) { brush = Links.Brushes.SmallImageBrush; tip = Links.Interface("Small"); }
            else if (itemsize == ItemSize.Medium) { brush = Links.Brushes.MediumImageBrush; tip = Links.Interface("Medium"); }
            else { brush = Links.Brushes.LargeImageBrush; tip = Links.Interface("Large"); }
            Rectangle rect = Common.GetRectangle(size, brush);
            rect.ToolTip = tip;
            return rect;
        }
        public static Rectangle PutSizeRect(Canvas canvas, int size, ItemSize itemsize, int left, int top, bool needborder)
        {
            Brush brush; string tip;
            if (itemsize == ItemSize.Small) { brush = Links.Brushes.SmallImageBrush; tip = Links.Interface("Small"); }
            else if (itemsize == ItemSize.Medium) { brush = Links.Brushes.MediumImageBrush; tip = Links.Interface("Medium"); }
            else { brush = Links.Brushes.LargeImageBrush; tip = Links.Interface("Large"); }
            Rectangle rect = Common.PutRect(canvas, size, brush, left, top, needborder, tip);
            return rect;
        }
        public static RadialGradientBrush GetRadialBrush(Color color, double x, double y)
        {
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientOrigin = new Point(x, y);
            brush.GradientStops.Add(new GradientStop(color, 1));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0));
            return brush;
        }
        public static LinearGradientBrush GetLinearBrush(Color color1, Color color2)
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(color1, 1));
            brush.GradientStops.Add(new GradientStop(color2, 0));
            brush.StartPoint = new Point(0.5, 0);
            brush.EndPoint = new Point(0.5, 1);
            return brush;
        }
        public static LinearGradientBrush GetLinearBrush(Color color1, Color color2, Color color3)
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(color1, 1));
            brush.GradientStops.Add(new GradientStop(color2, 0.5));
            brush.GradientStops.Add(new GradientStop(color3, 0));
            return brush;
        }
        public static LinearGradientBrush GetVertLinearBrush(Color color1, Color color2, Color color3, double off1, double off2, double off3)
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.GradientStops.Add(new GradientStop(color1, off1));
            brush.GradientStops.Add(new GradientStop(color2, off2));
            brush.GradientStops.Add(new GradientStop(color3, off3));
            brush.StartPoint = new Point(0.5, 0);
            brush.EndPoint = new Point(0.5, 1);
            return brush;
        }
        public static LinearGradientBrush GetLinearBrush(Color color1, Color color2, Color color3, Point start, Point end)
        {
            LinearGradientBrush brush = GetLinearBrush(color1, color2, color3);
            brush.StartPoint = start;
            brush.EndPoint = end;
            return brush;
        }
        
        public static void DrawEllipse(Canvas canvas, int size, Point pt)
        {
            Ellipse el = new Ellipse();
            el.Width = size;
            el.Height = size;
            canvas.Children.Add(el);
            Canvas.SetZIndex(el, 10);
            Canvas.SetLeft(el, pt.X);
            Canvas.SetTop(el, pt.Y);
            el.Fill = Brushes.Black;
        }
        public static Border GetPlanetAtmospere(PlanetTypes type)
        {
            Border border = new Border();
            border.Background = Links.Brushes.PlanetTypeBrushes[type];
            /*Brush brush;
            switch (type)
            {
                case PlanetTypes.Green: brush = Brushes.Green; break;
                case PlanetTypes.Burned: brush = Brushes.Red; break;
                case PlanetTypes.Freezed: brush = Brushes.Blue; break;
                case PlanetTypes.Gas: brush = Brushes.Gray; break;
                default: brush = Brushes.Black; break;
            }*/
            border.Width = 50; border.Height = 50;
            //border.BorderBrush = brush;
            //border.BorderThickness = new Thickness(5);
            //border.CornerRadius = new CornerRadius(25);
            return border;
        }
        public static string GetAccuracyText(int value)
        {
            switch (value)
            {
                case 2: return Links.Interface("MaxAcc");
                case 3: return Links.Interface("HighAcc");
                case 4: return Links.Interface("MiddleAcc");
                case 5: return Links.Interface("LowAcc");
                default: return "";
            }
            
        }
        public static Canvas GetCloseCanvas(bool needPopUpClose)
        {
            Canvas canvas = new Canvas();
            canvas.Width = 50;
            canvas.Height = 50;
            Ellipse el = new Ellipse(); el.Width = 50; el.Height = 50;
            el.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 168, 255));
            el.StrokeThickness = 2;
            el.Fill = Brushes.Black;
            canvas.Children.Add(el);
            Rectangle rect1 = new Rectangle(); rect1.Width = 10; rect1.Height = 30;
            rect1.Fill = Brushes.Red;
            Rectangle rect2 = new Rectangle(); rect2.Width = 30; rect2.Height = 10;
            rect2.Fill = Brushes.Red;
            canvas.Children.Add(rect1); Canvas.SetLeft(rect1, 20); Canvas.SetTop(rect1, 10);
            canvas.Children.Add(rect2); Canvas.SetLeft(rect2, 10); Canvas.SetTop(rect2, 20);
            canvas.RenderTransformOrigin = new Point(0.5, 0.5);
            canvas.RenderTransform = new RotateTransform(45);
            if (needPopUpClose)
                canvas.PreviewMouseDown += Canvas_PreviewMouseDown;
            return canvas;
        }

        private static void Canvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }

        public static Canvas GetOkCanvas()
        {
            Canvas canvas = new Canvas();
            canvas.Width = 50;
            canvas.Height = 50;
            Ellipse el = new Ellipse(); el.Width = 50; el.Height = 50;
            el.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 168, 255));
            el.StrokeThickness = 2;
            el.Fill = Brushes.Black;
            canvas.Children.Add(el);
            Path path = new Path(); path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 l15,30 l15,-30"));
            path.StrokeLineJoin = PenLineJoin.Round;
            path.Stroke = Brushes.Green; path.StrokeThickness = 10;
            canvas.Children.Add(path); Canvas.SetLeft(path, 10); Canvas.SetTop(path, 10);
            return canvas;
        }
        /// <summary> Метод ищет в файле s строку под тегом title и возвращает её. Если такого тэга нет - то возвращает пустую строку
        public static string GetTaggedValue(string file, string tag)
        {
            Regex start = new Regex(String.Format("<{0}>", tag));
            Match m1 = start.Match(file);
            if (m1.Success == false) return "";
            Regex end = new Regex(String.Format("</{0}>", tag));
            Match m2 = end.Match(file, m1.Index + m1.Length);
            if (m2.Success == false) return "";
            //position = m2.Index + m2.Length;
            return file.Substring(m1.Index + m1.Length, m2.Index - m1.Index - m1.Length);
        }
        /// <summary> Метод ищет в файле s строку под тегом title и возвращает её. Если такого тэга нет - то возвращает пустую строку
        public static string GetTaggedValue(string file, string tag, int startpos)
        {
            Regex start = new Regex(String.Format("<{0}>", tag));
            Match m1 = start.Match(file, startpos);
            if (m1.Success == false) return "";
            Regex end = new Regex(String.Format("</{0}>", tag));
            Match m2 = end.Match(file, m1.Index + m1.Length);
            if (m2.Success == false) return "";
            //position = m2.Index + m2.Length;
            return file.Substring(m1.Index + m1.Length, m2.Index - m1.Index - m1.Length);
        }
        public static Battle GetFleetLastBattle(GSFleet fleet)
        {
            Battle result = null;
            foreach (Battle battle in GSGameInfo.Battles.Values)
            {
                if (battle.Fleet1ID == fleet.ID || battle.Fleet2ID == fleet.ID)
                    result = battle;
            }
            return result;
        }
        public static SortedList<string, string> SearchForHelp(string s)
        {
            SortedList<int, string> regexes = new SortedList<int, string>();
            int steppos = SearchRegex(s, "Step");
            if (steppos != -1) regexes.Add(steppos, "Step");
            int titlepos = SearchRegex(s, "Title");
            if (titlepos != -1) regexes.Add(titlepos, "Title");
            int textpos = SearchRegex(s, "Text");
            if (textpos != -1) regexes.Add(textpos, "Text");
            int backpos = SearchRegex(s, "Back");
            if (backpos != -1) regexes.Add(backpos, "Back");
            int textlocpos = SearchRegex(s, "TextPos");
            if (textlocpos != -1) regexes.Add(textlocpos, "TextPos");
            int selectionpos = SearchRegex(s, "Selection");
            if (selectionpos != -1) regexes.Add(selectionpos, "Selection");
            for (int i = 1; ; i++)
            {
                string r = String.Format("Arrow{0}", i);
                int pos = SearchRegex(s, r);
                if (pos != -1) regexes.Add(pos, r);
                else break;
            }
            int menupos = SearchRegex(s, "StartMenu");
            if (menupos != -1)
                regexes.Add(menupos, "StartMenu");
            int specpos = SearchRegex(s, "Spec");
            if (specpos != -1) regexes.Add(specpos, "Spec");
            SortedList<string, string> result = new SortedList<string, string>();
            for (int i = 0; i < regexes.Count; i++)
            {
                if (i == regexes.Count - 1)
                { result.Add(regexes.Values[i], s.Substring(regexes.Keys[i] + regexes.Values[i].Length + 1)); }
                else
                {
                    int startpos = regexes.Keys[i] + regexes.Values[i].Length + 1;
                    int endpos = regexes.Keys[i + 1];
                    result.Add(regexes.Values[i], s.Substring(startpos, endpos - startpos));
                }
            }
            return result;
        }
        static int SearchRegex(string text, string t)
        {
            Regex r = new Regex(t);
            Match m = r.Match(text);
            if (m.Success) return m.Index;
            else return -1;
        }
    }

    /// <summary> вспомогательный класс, ищущий в строке параметры, и возвращающий их распознанные</summary>
    class TextPosition
    {
        public string Title;
        public int[] IntValues;
        public string[] StringValues;
        public TextPosition(string s)
        {
            string[] invals = s.Split(':');
            Title = invals[0];
            byte pos = 1; if (invals.Length == 1) pos = 0;
            string[] values = invals[pos].Split(',');
            List<int> Ints = new List<int>();
            List<string> Strings = new List<string>();
            for (int i = 0; i < values.Length; i++)
            {
                int value = 0;
                if (Int32.TryParse(values[i], out value)) Ints.Add(value);
                else Strings.Add(values[i]);
            }
            IntValues = Ints.ToArray();
            StringValues = Strings.ToArray();
        }
        public byte[] GetBytes()
        {
            List<byte> list = new List<byte>();
            foreach (int i in IntValues)
                list.Add((byte)i);
            return list.ToArray();
        }
        public static TextPosition[] GetPositions(string s)
        {
            string[] stroke = s.Split(' ');
            TextPosition[] result = new TextPosition[stroke.Length];
            for (int i = 0; i < stroke.Length; i++)
                result[i] = new TextPosition(stroke[i]);
            return result;
        }
        
    }
    public class InterfaceButton:Canvas
    {
        Label block;
        Path path;
        public InterfaceButton(int width, int height, int fasks, int fontsize)
        {
            Width = width;
            Height = height;
            string s = String.Format("M0,{0} l{0},-{0} h{1} l{0},{0} v{2} l-{0},{0} h-{1} l-{0},-{0}z",fasks, width - fasks*2, height - fasks*2);
            Clip = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(s));
            ClipToBounds = true;
            Background = Links.Brushes.NotSelectedMainBrush;
            int nf = fasks - 2;
            string s2 = String.Format("M{0},{1} l{2},-{2} h{3} l{2},{2} v{4} l-{2},{2} h-{3} l-{2},-{2}z", nf, nf*2, nf, width - 4*nf, height - 4 * nf);
            path = new Path();
            path.Stroke = Brushes.LightGray;
            path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(s2));
            Children.Add(path);
            block = new Label();
            block.Width = width;
            block.Height = height;
            block.VerticalContentAlignment = VerticalAlignment.Center;
            block.HorizontalContentAlignment = HorizontalAlignment.Center;
            block.FontFamily = Links.Font;
            block.FontSize = fontsize;
            block.FontWeight = FontWeights.Bold;
            block.Foreground = Brushes.White;
            
            Children.Add(block);
        }
        public void SetText(string text)
        {
            block.Content = text;
        }
        public void SetImage(Brush brush)
        {
            block.Background = brush;
        }
        public void PutToCanvas(Canvas canvas, int left, int top)
        {
            canvas.Children.Add(this);
            Canvas.SetLeft(this, left);
            Canvas.SetTop(this, top);
        }
        public void SetAngle(double angle)
        {
            RenderTransformOrigin = new Point(0.5, 0.5);
            RotateTransform rotate = new RotateTransform();
            RenderTransform = rotate;
            rotate.Angle = angle;
        }
        public void SetSkew(double x, double y)
        {
            RenderTransformOrigin = new Point(0.5, 0.5);
            SkewTransform skew = new SkewTransform(x, y);
            RenderTransform = skew;
        }
        public void SetEnabled()
        {
            Background = Links.Brushes.NotSelectedMainBrush;
            IsEnabled = true;
        }
        public void SetDisabled()
        {
            Background = Links.Brushes.DisabledMainBrush;
            IsEnabled = false;
        }
        public void PutToGrid(Grid grid, int row, int column)
        {
            grid.Children.Add(this);
            Grid.SetRow(this, row); Grid.SetColumn(this, column);
        }
        public void SetForm(PathGeometry geometry)
        {
            Clip = geometry;
        }
        public void SetBorder(PathGeometry geometry)
        {
            path.Data = geometry;
        }
        public void SetTextRotate(double angle)
        {
            RotateTransform rotate = new RotateTransform(angle);
            block.RenderTransformOrigin = new Point(0.5, 0.5);
            block.RenderTransform = rotate;
        }
       
    }
}