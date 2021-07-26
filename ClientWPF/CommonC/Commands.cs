using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using System.Threading.Tasks;
using System.Threading;


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
    class Commands
    {
        //public static void Start()
       // {
       //     Crypt.fill_Crypto();
       //     GSString.Prepare();
       //     Common.CreateLoginProxy();
       //     
       // }
        public static bool Create_Account(string Login, string Password, string Name)
        {     
            Links.login_Status = Login_Status.Signing;
            byte[] LoginArr = (new GSString(Login)).Array;
            byte[] PasswordArr = BitConverter.GetBytes((new GSString(Password)).GetLongHash());
            byte[] NameArr = (new GSString(Name)).Array;
            try
            {
                ByteMessage message = new ByteMessage(Links.Loginproxy.CreateAccount(LoginArr, PasswordArr, NameArr));
                if (message.Type == MessageResult.Yes)
                    return true;
                //MessageBox.Show("Account created");
                else
                {
                    MessageBox.Show(Common.GetStringFromByteArray(message.Message, 0));
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Connection error");
            }
            Links.login_Status = Login_Status.Waiting_Sign;
            return false;
        }
        public static void Login(string Login, string Password, bool save)
        {
            Links.Lang = (int)LoginCanvas.CurrentLanguage;
            SaveData(Login, Password, save);
            Links.login_Status = Login_Status.Logging;
            ByteMessage message;
            try
            {
                List<byte> array = new List<byte>();
                array.AddRange(new GSString(Login).Array);
                array.AddRange(BitConverter.GetBytes(Links.ClientHashVersion));
                message = new ByteMessage(Links.Loginproxy.LoginPhase1(array.ToArray()));
                if (message.Type != MessageResult.Yes)
                {
                    MessageBox.Show(Common.GetStringFromByteArray(message.Message,0));
                    return;
                }
                else
                {
                    long longhash = (new GSString(Password)).GetLongHash();
                    GSString cryptrandom = GSString.GetGSString(message.Message, 0);
                    GSString random = cryptrandom.DeCryptString(BitConverter.GetBytes(longhash));
                    //string random = Crypt.Decrypt(Common.GetStringFromByteArray(message.Message,0), Crypt.LongHash(Password).ToString());
                    GSString cryptPassword = (new GSString(Password)).GetPasswordString(random);
                    //string cryptPassword = Crypt.Encrypt(Password, random);
                    message = new ByteMessage(Links.Loginproxy.LoginPhase2((new GSString(Login)).Array, cryptPassword.Array));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Connection Error");
                Links.login_Status = Login_Status.Off;
                return;
            }
            if (message.Type == MessageResult.Yes)
            {
                //MessageBox.Show("Connection Sucsessful");
                Start_Game(Login, BitConverter.ToInt64(message.Message,0));
                Links.login_Status = Login_Status.Logged;
            }
            else
            {
                MessageBox.Show("!" + Common.GetStringFromByteArray(message.Message,0));
                Links.login_Status = Login_Status.Off;
            }



        }
        public static SaveData LoadData()
        {
            byte[] data = System.IO.File.ReadAllBytes("Parameters.txt");
            SaveData result = new Client.SaveData();
            result.Sound = data[0];
            result.Music = data[1];
            result.FullScreen = data[2] == 1;
            result.Width = BitConverter.ToUInt16(data, 3);
            result.Height = BitConverter.ToUInt16(data, 5);
            result.Lang = data[7];
            result.SavePass = data[8] == 1;
            result.Animation = data[9] == 1;
            int LoginLength = BitConverter.ToInt32(data, 10);
            int[] passpos = new int[] { 10, 26, 78, 215, 456, 212, 12, 2, 6, 8, 97 };
            List<char> l = new List<char>();
            int c = 0;
            int pos = 14;
            for (int i = 0; i < LoginLength; i++)
            {
                l.Add((char)((int)BitConverter.ToChar(data, pos) - passpos[c]));
                c++; c = c % passpos.Length;
                pos += 2;
            }
            result.Login = new string(l.ToArray());
            int PasswordLength = BitConverter.ToInt32(data, pos);
            pos += 4;
            List<char> p = new List<char>();
            for (int i = 0; i < PasswordLength; i++)
            {
                p.Add((char)((int)BitConverter.ToChar(data, pos) - passpos[c]));
                c++; c = c % passpos.Length;
                pos += 2;
            }
            result.Password = new string(p.ToArray());
            return result;
        }
        public static void SaveData(string Login, string Password, bool save)
        {
            int[] passpos = new int[] { 10, 26, 78, 215, 456, 212, 12, 2, 6, 8, 97 };
            int LoginLength, PasswordLength;
            if (save==false)
            {
                Random rnd = new Random();
                LoginLength = rnd.Next(5, 14);
                PasswordLength = rnd.Next(8, 16);
                List<char> l = new List<char>();
                for (int i = 0; i < LoginLength; i++)
                    l.Add((char)rnd.Next(32, 52));
                Login = new string(l.ToArray());
                List<char> p = new List<char>();
                for (int i = 0; i < PasswordLength; i++)
                    p.Add((char)rnd.Next(32, 52));
                Password = new string(l.ToArray());
            }
            LoginLength = Login.Length;
            PasswordLength = Password.Length;

            List<byte> list = new List<byte>();
            byte[] data = File.ReadAllBytes("Parameters.txt");
            
            list.Add(data[0]); //sound
            list.Add(data[1]); //music
            list.Add(data[2]);//fullscreen
            list.Add(data[3]); //width1;
            list.Add(data[4]); //width2;
            list.Add(data[5]); //height1;
            list.Add(data[6]); //height2;
            list.Add((byte)Links.Lang); //язык
            list.Add((byte)(save ? 1 : 0)); //сохранение данных
            list.Add((byte)(Links.Animation ? 1 : 0));//анимация
            list.AddRange(BitConverter.GetBytes(LoginLength));
            int c = 0;
            for (int i=0;i<Login.Length;i++)
            {
                char ch = (char)((int)Login[i] + (int)passpos[c]);
                c++; c = c % passpos.Length;
                list.AddRange(BitConverter.GetBytes(ch));
            }
            list.AddRange(BitConverter.GetBytes(PasswordLength));
            for (int i = 0; i < Password.Length; i++)
            {
                char ch = (char)((int)Password[i] + (int)passpos[c]);
                c++; c = c % passpos.Length;
                list.AddRange(BitConverter.GetBytes(ch));
            }
            System.IO.File.WriteAllBytes("Parameters.txt", list.ToArray());
        }
        public static void Load_Single_Game()
        {
            Links.Lang = (int)LoginCanvas.CurrentLanguage;
            Links.GameMode = EGameMode.Single;
            string loadarchiveerror = Common.LoadArchive1();
            if (loadarchiveerror != "")
            {
                MessageBox.Show(loadarchiveerror);
                return;
            }
            LocalServer.LoadSingleGame();
            BattleField.Fill();
            Links.Controller.MainCanvas = new MainCanvas();
            Links.Controller.mainWindow.MainVbx.Child = Links.Controller.MainCanvas;
            Links.Controller.BuildMainWindowPanels();
            SchemaImage.Prepare();
            //ScienceAnalys.Fill2();
            ShipBrush.FillHullBrushes();
            Pictogram.CreateBrushes2();
            ImageShipModel.CreateShipsAndGuns();
            //BigWindow w = new BigWindow();
            Gets.GetResources();
            NoticeBorder.CheckNotice();
        }
       
        public static void Start_Single_Game()
        {

            Links.Lang = (int)LoginCanvas.CurrentLanguage;
            Links.SaveData.SaveLang();
            Links.GameMode = EGameMode.Single;
           
            LoadGameData();
            LocalServer.StartSingleGame();
            HelpTopic.Create();
            BattleField.Fill();
            Links.Controller.MainCanvas = new MainCanvas();
            
            Links.Controller.mainWindow.MainVbx.Child = Links.Controller.MainCanvas;
            Links.Controller.BuildMainWindowPanels();
            SchemaImage.Prepare();
            NameCreator.Prepare();
            //ScienceAnalys.Fill2();
            ShipBrush.FillHullBrushes();
            Pictogram.CreateBrushes2();
            ImageShipModel.CreateShipsAndGuns();
            //BigWindow w = new BigWindow();
            Gets.GetResources();
            NoticeBorder.CheckNotice();
            GSGameInfo.SetServerTime();
            WriteBuildings();
            HelpStart start = new HelpStart(); start.Show();
        }
        static void WriteBuildings()
        {
            List<string> list = new List<string>();
            List<SectorTypes> sectors = new List<SectorTypes>() { SectorTypes.Live, SectorTypes.Money, SectorTypes.Metall,
            SectorTypes.MetalCap, SectorTypes.Chips, SectorTypes.ChipsCap, SectorTypes.Anti, SectorTypes.AntiCap, SectorTypes.Repair,
            SectorTypes.War};
            foreach (SectorTypes sector in sectors)
            {
                list.Add(SectorImages.List[sector].Title);
                for (int j = 0; j < 3; j++)
                {
                    GSBuilding2 b = new GSBuilding2(0, 0, sector, (byte)j);
                    list.Add(b.Name);
                    for (int i = 0; i < 11; i++)
                    {
                        b.CurLevel = (byte)i;
                        list.Add(String.Format("Уровень={0} Размер={1} Общий размер={2} Бонус={3} Общий бонус={4}", i, b.GetCurSize(), b.GetTotalSize(), b.GetCurValue(), b.GetTotalValue()));
                    }
                }
            }
            File.WriteAllLines("Buildings.txt", list.ToArray());
        }
        public static void LoadGameData()
        {
            //Gets.GetBasicSciences((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.BasicScienceGameData].File);
            //Gets.GetBasicSciences(File.ReadAllBytes("GameData/Sciences.txt"));
            //Gets.GetSciencePrices((string)Links.GameData.GameArchive1.Objects[Links.GameData.BasicSciencePriceGameData].File);
            
            //Links.Stars = Gets.GetStars((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.StarsGameData].File);
            //Links.Stars = Gets.GetStars(File.ReadAllBytes("GameData/Stars.txt"));
            //GSStar.FillNearPoints((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.BasicStarFiguresGameData].File);
            //GSStar.FillNearPoints(File.ReadAllBytes("GameData/FakeStars.txt"));
            //Gets.GetPlanets((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.PlanetsGameData].File);
            //Gets.GetPlanets(File.ReadAllBytes("GameData/Planets.txt"));
            //Gets.GetShipTypes((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.ShipTypesGameData].File);
            Gets.GetShipTypes(File.ReadAllLines("GameData/ShipTypes1.txt", Encoding.GetEncoding(1251)));
            //Gets.GetComputerTypes((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.ComputerTypesGameData].File);
            Gets.GetComputerTypes(File.ReadAllLines("GameData/Computers.txt", Encoding.GetEncoding(1251)));
            //Gets.GetEngineTypes((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.EngineTypesGameData].File);
            Gets.GetEngineTypes(File.ReadAllLines("GameData/Enginies.txt", Encoding.GetEncoding(1251)));
            //Gets.GetGeneratorTypes((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.GeneratorTypesGameData].File);
            Gets.GetGeneratorTypes(File.ReadAllLines("GameData/Generators.txt", Encoding.GetEncoding(1251)));
            //Gets.GetWeaponModifiers((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.WeaponModifiersGameData].File);
            Gets.GetWeaponModifiers(File.ReadAllBytes("GameData/WeaponModifiers.txt"));
            //Gets.GetArmors((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.BasicShipArmorsGameData].File);
            Gets.GetArmors(File.ReadAllBytes("GameData/ShipArmors.txt"));
            //Gets.GetWeaponTypes((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.WeaponTypesGameData].File);
            Gets.GetWeaponTypes(File.ReadAllBytes("GameData/WeaponTypes.txt"));
            //Gets.GetEquipmentTypes((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.EquipmentTypesGameData].File);
            Gets.GetEquipmentTypes(File.ReadAllBytes("GameData/Equipments.txt"));
            //Gets.GetShieldTypes((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.ShieldTypesGameData].File);
            Gets.GetShieldTypes(File.ReadAllLines("GameData/Shields.txt", Encoding.GetEncoding(1251)));
            //Gets.GetBasicBuildings((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.BuildingsGameData].File);
            Gets.GetBasicBuildings(File.ReadAllBytes("GameData/Buildings.txt"));
            Gets.GetArtefacts(null);
            LocalServer.SetGameSciences();
            Links.Science.GameSciences = ServerLinks.Science.GameSciences;
            //Gets.GetSciencePrices(File.ReadAllText("GameData/SciencePrices.txt"));
            //ServerLinks.Science.SciencePrices = Links.Science.SciencePrices;
            GameScience.SetElement();
            //GSGameInfo.BasicMaxLandsCount = BitConverter.ToInt32((byte[])Links.GameData.GameArchive1.Objects[Links.GameData.BasicMaxLandsCounts].File, 0);
            GSGameInfo.BasicMaxLandsCount = 5;
            //Mission.ResModificators = (byte[])Links.GameData.GameArchive1.Objects[Links.GameData.MissionResModificators].File;
            //Mission.ResModificators = File.ReadAllBytes("GameData/ResModificators.txt");
            Common.PilotPrepareLocal(GSPilot.Nicks, File.ReadAllText("GameData/NickEn.txt"), File.ReadAllText("GameData/NickRu.txt"));
            Common.PilotPrepareLocal(GSPilot.JapCit, File.ReadAllText("GameData/JapCitEn.txt"), File.ReadAllText("GameData/JapCitRu.txt"));
            Common.PilotPrepareLocal(GSPilot.JapFem, File.ReadAllText("GameData/JapFemEn.txt"), File.ReadAllText("GameData/JapFemRu.txt"));
            Common.PilotPrepareLocal(GSPilot.JapMan, File.ReadAllText("GameData/JapManEn.txt"), File.ReadAllText("GameData/JapFemRu.txt"));
            Common.PilotPrepareLocal(GSPilot.JapSur, File.ReadAllText("GameData/JapSurEn.txt"), File.ReadAllText("GameData/JapSurRu.txt"));
            Common.PilotPrepareLocal(GSPilot.RuCit, File.ReadAllText("GameData/RuCitEn.txt"), File.ReadAllText("GameData/RuCitRu.txt"));
            Common.PilotPrepareLocal(GSPilot.RuFem, File.ReadAllText("GameData/RuFemEn.txt"), File.ReadAllText("GameData/RuFemRu.txt"));
            Common.PilotPrepareLocal(GSPilot.RuMan, File.ReadAllText("GameData/RuManEn.txt"), File.ReadAllText("GameData/RuManRu.txt"));
            Common.PilotPrepareLocal(GSPilot.RuSur, File.ReadAllText("GameData/RuSurEn.txt"), File.ReadAllText("GameData/RuSurRu.txt"));
            Common.PilotPrepareLocal(GSPilot.AmerCit, File.ReadAllText("GameData/AmerCitEn.txt"), File.ReadAllText("GameData/AmerCitRu.txt"));
            Common.PilotPrepareLocal(GSPilot.AmerFem, File.ReadAllText("GameData/AmerFemEn.txt"), File.ReadAllText("GameData/AmerFemRu.txt"));
            Common.PilotPrepareLocal(GSPilot.AmerMan, File.ReadAllText("GameData/AmerManEn.txt"), File.ReadAllText("GameData/AmerManRu.txt"));
            Common.PilotPrepareLocal(GSPilot.AmerSur, File.ReadAllText("GameData/AmerSurEn.txt"), File.ReadAllText("GameData/AmerSurRu.txt"));
            Gets.GetPics();
            Common.CreateImageBrushes();
            //StoryLine.CreateStorys();
        }
        public static void Start_Test_Game(byte[] array)
        {
            Links.Lang = (int)LoginCanvas.CurrentLanguage;
            Links.GameMode = EGameMode.Single;
            long BattleID = 0;
            ByteMessage message;
            LoadGameData();
            BattleField.Fill();
            message = new ByteMessage(LocalServer.StartTestBattle(array));
            BattleID = BitConverter.ToInt64(message.Message, 0);
            
            Links.Controller.MainCanvas = new MainCanvas();
            //Links.Controller.BattleFieldCanvas = new BattleFieldCanvas();
            Links.Controller.IntBoya = new IntBoya();
            //Links.Controller.mainWindow.MainVbx.Child = Links.Controller.MainCanvas;
            SchemaImage.Prepare();
            NameCreator.Prepare();
            ShipBrush.FillHullBrushes();
            Pictogram.CreateBrushes2();
            ImageShipModel.CreateShipsAndGuns();
            new GSFleet(1201);
            IntBoya.Test = true;
            //BattleFieldCanvas.Test = true;
            Links.Controller.IntBoya.Place(BattleID);
            //Links.Controller.BattleFieldCanvas.Place(BattleID);
            
        }
        public static void Start_Game(string Login, long random)
        {
            ///Метод начинает игру.
            ///В начале сохраняется информация о логине и случайной последовательности.
            ///Затем, скрывается окно логина и запускается главное окно.
            ///Главное окно состоит из общих панелей и панелей специфичных для отображения каждого вида игровых данных
            ///Все специфичные панели должны быть наследниками базовой панели
            ///Функционал базовой панели должен включать таймер, опрос количества кредитов и опрос окна чата.
            GSGameInfo.Login = Login;
            GSGameInfo.random = random;
            Links.GameMode = EGameMode.Multi;
            //Interface.FillInterface();

            string loadarchiveerror = Common.LoadArchive1();
            if (loadarchiveerror != "")
            {
                MessageBox.Show(loadarchiveerror);
                return;
            }
            BattleField.Fill();

            Links.Controller.MainCanvas = new MainCanvas();
            Links.Controller.mainWindow.MainVbx.Child = Links.Controller.MainCanvas;
            //ScienceAnalys.GetMaxScinecesNeedsForNextlevel();
            //Common.SetSmalltextStyle();
            //Gets.GetName();
            
            Gets.GetTotalInfo("В начале игры");
            Gets.GetResources();
            Links.Controller.BuildMainWindowPanels();
            
            //CreateInterface();
            
            //Links.Controller.mainWindow.ClearLoginAndSign();

            SchemaImage.Prepare();
            //ScienceAnalys.Fill2();
            ShipBrush.FillHullBrushes();
            Pictogram.CreateBrushes2();
            ImageShipModel.CreateShipsAndGuns();
            //BattleCanvas.Create();
            //BigWindow w = new BigWindow();
            //Links.Controller.ControllerStatus = EControllerStatus.Main;
            Links.Controller.MainTimer = new System.Windows.Threading.DispatcherTimer();
            Links.Controller.MainTimer.Interval = TimeSpan.FromSeconds(30);
            Links.Controller.MainTimer.Tick += new EventHandler(MainTimer_Tick);
            Links.Controller.MainTimer.Start();
            NoticeBorder.CheckNotice();
        }

    
        static void MainTimer_Tick(object sender, EventArgs e)
        {
            Gets.GetResources();
            NoticeBorder.CheckNotice();    
        }

        public static void Stop_Game()
        {
            Links.Controller.mainWindow.Close();
            Links.Controller.loadWindow.Close();
            Links.Controller.Debug.Close();
        }
    }
    class SaveData
    {
        public ushort Width;
        public ushort Height;
        public bool FullScreen;
        public bool SavePass;
        public byte Sound;
        public byte Music;
        public string Login;
        public string Password;
        public byte Lang;
        public bool Animation;
        public void SaveFullScreen()
        {
            byte[] data = File.ReadAllBytes("Parameters.txt");
            if (FullScreen)
                data[2] = 1;
            else
                data[2] = 0;
            File.WriteAllBytes("Parameters.txt", data);
        }
        public void SaveLang()
        {
            byte[] data = File.ReadAllBytes("Parameters.txt");
            data[7] = (byte)Links.Lang;
            File.WriteAllBytes("Parameters.txt", data);
        }
        public void SaveSize()
        {
            if (Links.Controller.mainWindow.Width != Width)
                Width = (ushort)Links.Controller.mainWindow.Width;
            if (Links.Controller.mainWindow.Height!=Height)
            Height = (ushort)Links.Controller.mainWindow.Height;
            byte[] data = File.ReadAllBytes("Parameters.txt");
            byte[] w = BitConverter.GetBytes(Width);
            data[3] = w[0]; data[4] = w[1];
            byte[] h = BitConverter.GetBytes(Height);
            data[5] = h[0]; data[6] = h[1];
            File.WriteAllBytes("Parameters.txt", data);
        }
        public void SaveAudio()
        {
            byte[] data = File.ReadAllBytes("Parameters.txt");
            data[0] = (byte)(Links.SoundVolume * 10.0f);
            data[1] = (byte)(Links.MusicVolume * 10.0f);
            File.WriteAllBytes("Parameters.txt", data);
        }
        public void SaveAnimation()
        {
            byte[] data = File.ReadAllBytes("Parameters.txt");
            data[9] = (byte)(Links.Animation ? 1 : 0);
            File.WriteAllBytes("Parameters.txt", data);
        }
    }
}
