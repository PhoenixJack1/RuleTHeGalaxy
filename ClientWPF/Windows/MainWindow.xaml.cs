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
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Viewbox MainVbx;
        
        public MainWindow()
        {
            InitializeComponent();
            Icon = Links.IconImage;
            //Width = 1294;
            //Height = 758;
            WindowStartupLocation = WindowStartupLocation.Manual;
            Left = 0; Top = 0;
            MainVbx = new Viewbox();
            RenderOptions.SetBitmapScalingMode(MainVbx, BitmapScalingMode.Fant);
            MainVbx.Stretch = Stretch.Fill;
            Content = MainVbx;
            Interface.FillInterface();
            Common.SetSmalltextStyle();
            //GSArchive.GetSounds();
            Links.SaveData = Commands.LoadData();
            Links.Controller = new Controllerclass(this);
            Links.Controller.Debug = new DebugWindow();
            //Links.Controller.Debug.Show();
            Links.Controller.LoginCanvas = new LoginCanvas();
            MainVbx.Child = Links.Controller.LoginCanvas;
            KeyDown += new KeyEventHandler(MainWindow_KeyDown);
            Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
            Activated += MainWindow_Activated;
            Uri uri = new Uri("pack://application:,,,/resources/Cur4.cur");
            System.Windows.Resources.StreamResourceInfo sri = Application.GetResourceStream(uri);
            Cursor cursor = new Cursor(sri.Stream);
            Mouse.OverrideCursor = cursor;
            // FocusableChanged += MainWindow_FocusableChanged;
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Links.SaveData.SaveSize();
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            Activated -= MainWindow_Activated;

            if (Links.SaveData.FullScreen)
                SetFullScreen();
            Width = Links.SaveData.Width;
            Height = Links.SaveData.Height;
            Links.SoundVolume = Links.SaveData.Sound * 0.1f;
            Links.MusicVolume = Links.SaveData.Music * 0.1f;
            Links.Animation = Links.SaveData.Animation;
            SizeChanged += MainWindow_SizeChanged;
            Music.Play();
        }
        
        public void AddButtonsTitle(string text, int left, int top)
        {
            TextBlock block = new TextBlock();
            block.FontFamily = Links.Font;
            block.FontSize = 20;
            block.Foreground = Brushes.White;
            block.Width = 100;
            block.Text = text;
            block.FontWeight = FontWeights.Bold;
            block.TextAlignment = TextAlignment.Center;
            middlecanvas.Children.Add(block);
            Canvas.SetLeft(block, left);
            Canvas.SetTop(block, top);

        }
        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Links.Controller.loadWindow.Close();
            if (Music.CurMusic != null) { Music.CurMusic.forcestop = true; Music.CurMusic.Wave.Stop(); }
            Links.Controller.Debug.Close();
            //Links.Controller.Debug.Close();
        }
        public void SetMain()
        {
            MinWidth = 1080;
            MinHeight = 638;
            //MaxWidth = 1600;
            //MaxHeight = 900;
            MainVbx.Child = mainGrid;
            //this.SizeChanged += new SizeChangedEventHandler(MainWindow_SizeChanged);
        }

       
        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                if (WindowStyle != WindowStyle.None)
                {
                    Links.SaveData.FullScreen = true; Links.SaveData.SaveFullScreen();
                    SetFullScreen();
                }
                else
                {
                    Links.SaveData.FullScreen = false; Links.SaveData.SaveFullScreen();
                    SetNormalSize();
                }
                return;
            }
            if (e.Key==Key.F1)
            {
                Links.Controller.HelpCanvas.Show(-1);
                return;
            }
            if (e.Key==Key.F2)
            {
                Canvas c = new Canvas(); c.Width = 1920; c.Height = 1080; c.Background = Brushes.Black;
                Schema schema = new Schema(); schema.SetShipType(48235); schema.SetGenerator(50257); schema.SetShield(48261);
                schema.SetEngine(50277); schema.SetComputer(50280);
                schema.SetWeapon(6322, 0);//2301
                schema.SetWeapon(12362, 1);
                schema.SetWeapon(6322, 2);
                for (int i = 1; i < 12; i++)
                {
                    ShipB ship = new ShipB(0, schema, 100, null, new byte[] { (byte)i, 0, 0, 0 }, ShipSide.Attack, null, false, new byte[] { 0, 0, 0, 0 }, ShipBMode.Info,255);
                    ship.HexShip.Children.Remove(ship.HexShip.Health);
                    ship.HexShip.Children.Remove(ship.HexShip.Shield);
                    ship.HexShip.Children.Remove(ship.HexShip.Energy);
                    c.Children.Add(ship.HexShip); Canvas.SetLeft(ship.HexShip, 50+(i%4)*400); Canvas.SetTop(ship.HexShip, 50+(i/4)*300);
                }
                
                
                Links.Controller.PopUpCanvas.Place(c, true);
            }
            
            if (e.SystemKey==Key.M)
            {
                Music.ChangeVolume(0f);
                /*Links.MusicOn = !Links.MusicOn;
                if (Links.MusicOn) PlayMusic();*/
            }
            if (e.SystemKey==Key.C)
            {
                Events.Cheat();
                Gets.GetResources();
            }
            if (Links.Controller.mainWindow.MainVbx.Child == Links.Controller.LoginCanvas)
            {
                //if (e.SystemKey == Key.J) Commands.Login("jack_mega@mail.ru", "Annihilatio1", true);
                //else if (e.SystemKey == Key.V) Commands.Login("vasya@mail.ru", "AAAAAA", true);
                //else if (e.SystemKey == Key.A) Commands.Login("maria@mail.ru", "ghf2plyb3r!", true);
                //else 
                //if (e.Key == Key.Enter) Links.Controller.LoginCanvas.MainBtn_PreviewMouseDown(null, null);
                //if (e.SystemKey == Key.T) Commands.Start_Test_Game();
            }
            /*
            else if (Links.Controller.ControllerStatus == EControllerStatus.Create)
            {
                if (e.Key == Key.Enter) signGrid.SignButton_Click(null, null);
            }
            */
            else if (((Canvas)MainVbx.Child) == ((Canvas)Links.Controller.MainCanvas))
            {
                //if (e.SystemKey == Key.P) Links.Controller.PopUpCanvas.Place(new PilotGenerator(), false);
                //if (e.SystemKey == Key.F && Links.Controller.CurrentGrid == Links.Controller.galaxypanel)
                //    Links.Controller.galaxypanel.DrawFreeLandsPath();
                //if (e.SystemKey == Key.F && Links.Controller.CurrentGrid == Links.Controller.sectorpanel)
                //    Links.Controller.sectorpanel.DrawFreeLandsPath();
                if (e.Key==Key.F1)
                {
                    if (Links.Controller.CurPanel==GamePanels.Galaxy)
                    {
                        Links.Controller.PopUpCanvas.Place(new GalaxyHelpCanvas(), false);
                        return;
                    }
                }
                if (e.SystemKey == Key.S && Links.Controller.CurPanel == GamePanels.SchemasCanvas)
                {
                    Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(Links.Controller.NewSchemasCanvas.CurSchema.ToString()), true);
                    Clipboard.SetText(Links.Controller.NewSchemasCanvas.CurSchema.ToString());
                }

                if (Links.Controller.MainCanvas.LowBorder.Child == Links.Controller.Market.box && !Links.Controller.MainCanvas.Children.Contains(Links.Controller.PopUpCanvas))
                {
                    Links.Controller.Market.ButtonPressed(e.Key);
                    return;
                }
                if (e.SystemKey==Key.B)
                {
                    //BigWindow.ShowWindow();
                }
                //if (Links.Controller.MainCanvas.LowBorder.Child==Links.Controller.SchemasCanvas.box)
                //{
                    //if (e.SystemKey == Key.G)
                    //    Links.Controller.SchemasCanvas.GenerateSchema();
                    //return;
                //}
                if (e.SystemKey==Key.P)
                {
                    Links.Controller.PopUpCanvas.Place(new PilotGenerator());
                }
            }
            else if (IntBoya.IsActive)
            {
                /*
                switch (e.Key)
                {
                    case Key.M: IntBoya.SetShipCommands(ShipCommands.Move); break;
                    //case Key.L: BattleFieldCanvas.SetShipCommands(ShipCommands.Leave); break;
                    case Key.E: IntBoya.SetShipCommands(ShipCommands.Enter); break;
                    //case Key.I: BattleFieldCanvas.SetShipCommands(ShipCommands.Info); break;
                    case Key.D1: IntBoya.SetShipCommands(ShipCommands.Gun1); break;
                    case Key.D2: IntBoya.SetShipCommands(ShipCommands.Gun2); break;
                    case Key.D3: IntBoya.SetShipCommands(ShipCommands.Gun3); break;
                    case Key.D4: IntBoya.SetShipCommands(ShipCommands.AllGun); break;
                    case Key.A: IntBoya.SetShipCommands(ShipCommands.TargetGun); break;
                    case Key.Escape: IntBoya.SetShipCommands(ShipCommands.None); break;
                    case Key.Enter: IntBoya.BTNConfirm_PreviewMouseDown(null, null); break;
                }*/
                switch (e.SystemKey)
                {
                    //case Key.L: Links.Controller.PopUpCanvas.Place(new TestBattleBorder(IntBoya.Battle), false); break;
                }
                return;
            }
            if (e.SystemKey==Key.M)
            {
                /*
                GSShip ship = GSGameInfo.Ships.ElementAt(0).Value;
                ShipB shipb = new ShipB(1, ship.Schema, ship.Health, ship.Pilot, new byte[4], ShipSide.Attack, null, true, ShipBMode.Battle, Brushes.Black);
                Links.Controller.ShipPopUp.Place(shipb);
                */
            }
            /*
            if (e.SystemKey == Key.M)
            {
                Viewbox v = new Viewbox();
                v.Width = 1200;
                v.Height = 600;
                Canvas c = new Canvas();
                c.Width = 1200;
                c.Height = 600;
                v.Child = c;
                c.Background = new SolidColorBrush(Color.FromArgb(255,20,20,20));
                DrawElement(c, Pictogram.GetPict(Picts.Health, Target.None, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Health, Target.None, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Shield, Target.None, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Shield, Target.None, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Energy, Target.None, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Energy, Target.None, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Restore, Target.Health, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Restore, Target.Health, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Restore, Target.Shield, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Restore, Target.Shield, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Restore, Target.Generator, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Restore, Target.Generator, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.HealthDamage, Target.None, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.ShieldDamage, Target.None, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Accuracy, Target.Energy, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Accuracy, Target.Energy, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Accuracy, Target.Physic, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Accuracy, Target.Physic, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Accuracy, Target.Irregular, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Accuracy, Target.Irregular, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Accuracy, Target.Cyber, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Accuracy, Target.Cyber, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Accuracy, Target.Total, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Accuracy, Target.Total, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Evasion, Target.Energy, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Evasion, Target.Energy, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Evasion, Target.Physic, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Evasion, Target.Physic, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Evasion, Target.Irregular, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Evasion, Target.Irregular, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Evasion, Target.Cyber, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Evasion, Target.Cyber, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Evasion, Target.Total, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Evasion, Target.Total, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Damage, Target.Energy, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Damage, Target.Energy, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Damage, Target.Physic, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Damage, Target.Physic, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Damage, Target.Irregular, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Damage, Target.Irregular, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Damage, Target.Cyber, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Damage, Target.Cyber, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Damage, Target.Total, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Damage, Target.Total, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Ignore, Target.Energy, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Ignore, Target.Energy, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Ignore, Target.Physic, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Ignore, Target.Physic, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Ignore, Target.Irregular, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Ignore, Target.Irregular, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Ignore, Target.Cyber, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Ignore, Target.Cyber, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Ignore, Target.Total, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Ignore, Target.Total, true, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Immune, Target.Energy, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Immune, Target.Physic, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Immune, Target.Irregular, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Immune, Target.Cyber, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Immune, Target.Total, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Cargo, Target.None, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Colony, Target.None, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.CritDamage, Target.None, false, null, 45));
                DrawElement(c, Pictogram.GetPict(Picts.Speed, Target.None, false, null, 45));
                DrawElement(c, Common.GetRectangle(45, Shieldclass.GetSmallShieldBrush()));
                DrawElement(c, Common.GetRectangle(45, Shieldclass.GetMediumShieldBrush()));
                DrawElement(c, Common.GetRectangle(45, Shieldclass.GetLargeShieldBrush()));
                DrawElement(c, Common.GetRectangle(45, Computerclass.JumpRangeBrushes[0]));
                DrawElement(c, Common.GetRectangle(45, Computerclass.JumpRangeBrushes[1]));
                DrawElement(c, Common.GetRectangle(45, Computerclass.JumpRangeBrushes[2]));
                DrawElement(c, Common.GetRectangle(45, Generatorclass.TurnsBrushes[0]));
                DrawElement(c, Common.GetRectangle(45, Generatorclass.TurnsBrushes[1]));
                DrawElement(c, Common.GetRectangle(45, Generatorclass.TurnsBrushes[2]));
                DrawElement(c, Common.GetRectangle(45, Engineclass.MoveCostBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.SmallImageBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.MediumImageBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.LargeImageBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.WeaponGroupBrush[WeaponGroup.Energy]));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.WeaponGroupBrush[WeaponGroup.Physic]));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.WeaponGroupBrush[WeaponGroup.Irregular]));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.WeaponGroupBrush[WeaponGroup.Cyber]));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.WeaponGroupBrush[WeaponGroup.Any]));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.WeaponsBrushes[(EWeaponType)0]));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.WeaponsBrushes[(EWeaponType)1]));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.WeaponsBrushes[(EWeaponType)2]));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.WeaponsBrushes[(EWeaponType)3]));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.WeaponsBrushes[(EWeaponType)4]));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.WeaponsBrushes[(EWeaponType)5]));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.WeaponsBrushes[(EWeaponType)6]));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.WeaponsBrushes[(EWeaponType)7]));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.WeaponsBrushes[(EWeaponType)8]));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.WeaponsBrushes[(EWeaponType)9]));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.WeaponsBrushes[(EWeaponType)10]));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.WeaponsBrushes[(EWeaponType)11]));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.WeaponsBrushes[(EWeaponType)12]));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.WeaponsBrushes[(EWeaponType)13]));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.WeaponsBrushes[(EWeaponType)14]));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.WeaponsBrushes[(EWeaponType)15]));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.GeneratorBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.ShieldBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.ComputerBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.EngineBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.EquipmentBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.MoneyImageBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.MetalImageBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.ChipsImageBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.AntiImageBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.MetalCapBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.ChipCapBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.AntiCapBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.ZipImageBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.MathPowerBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.ShipImageBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.FleetBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.BuildingSizeBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.PeopleImageBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.TradeSector));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.Bank));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.Mine));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.ChipsFactory));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.ParticipleAccelerator));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.University));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.ScienceSector));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.Manufacture));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.Radar));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.DataCenter));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.RepairingBay));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.SciencePict));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.RareImageBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.LandIconBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.ColonizationBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.AttackEnemyBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.ResourceMissionBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.FleetPillageBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.FleetConquerBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.FleetLastDefenderBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.TwoSwords));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.FleetFreeBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.FleetDefenseBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.RiotBrush));
                DrawElement(c, Common.GetRectangle(45, Links.Brushes.RiotPrepareBrush));
                Links.Controller.PopUpCanvas.Place(v, true);
            }
            */
            /*
        if (e.SystemKey == Key.S) Links.Controller.galaxypanel.SaveFake();
        else if (e.SystemKey == Key.Z) //Создание фигур из звёзд
        {
            GalaxyPanel.StarLine starline = Links.Lines[Links.Lines.Count - 1];
            Links.Controller.galaxypanel.galaxyCanvas.Children.Remove(starline.Line);
            Links.Lines.Remove(starline);
        }
        else if (e.SystemKey == Key.X)
        {
            Links.Controller.galaxypanel.RemoveLastFakeStar();
        }
        else if (e.SystemKey == Key.L)
        {
            Links.Controller.galaxypanel.LoadFake();
        }
        else if (e.SystemKey == Key.C)
        {
            Links.Controller.galaxypanel.MakeCalc();
        }
        */
        }
        int pos = 0;
        int columns = 24;
        public void DrawElement(Canvas canvas, UIElement element)
        {
            int row = pos / columns;
            int col = pos - row * columns;
            canvas.Children.Add(element);
            Canvas.SetLeft(element, col * 50);
            Canvas.SetTop(element, row * 50);
            pos++;
        }

    

        void info_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }
        #region mainGrid
        public Grid mainGrid;
        public Label nameLabel;
        public ResourcePanel MoneyRes;
        public ResourcePanel MetalRes;
        public ResourcePanel ChipsRes;
        public ResourcePanel AntiRes;
        public ResourcePanel ZipRes;
        public ResourcePanel ShipsCount;

        public List<Path> Buttons;
        Canvas middlecanvas;
        //Button Shop_Button;
        
        public string ClickedButtonName;
        public Border LowBorder;
        //public QuestButton QuestButton;
        void InitMain()
        {
            
            mainGrid = new Grid();
            mainGrid.Width = 1280; mainGrid.Height = 720;
            Canvas MainCanvas = new Canvas();
            MainCanvas.Width = 1280;
            MainCanvas.Height = 720;
            mainGrid.Children.Add(MainCanvas);
            Grid.SetRowSpan(MainCanvas, 3);
            LinearGradientBrush Back = new LinearGradientBrush();
            Back.StartPoint = new Point(0.5, 0);
            Back.EndPoint = new Point(0.5, 1);
            Back.GradientStops.Add(new GradientStop(Colors.DarkGray, 1));
            Back.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            Back.GradientStops.Add(new GradientStop(Colors.DarkGray, 0));
            //MainCanvas.Background = Back;
            MainCanvas.Background = Brushes.Black;
            //Diod.Start(MainCanvas);
            //this.Content = mainGrid;
            mainGrid.RowDefinitions.Add(new RowDefinition()); mainGrid.RowDefinitions[0].Height = new GridLength(40); //верхняя панель
            mainGrid.RowDefinitions.Add(new RowDefinition()); mainGrid.RowDefinitions[1].Height = new GridLength(60); //панель с кнопками
            mainGrid.RowDefinitions.Add(new RowDefinition()); //основная панель

            Grid topGrid = new Grid(); mainGrid.Children.Add(topGrid);
            topGrid.ColumnDefinitions.Add(new ColumnDefinition()); //имя персонажа
            topGrid.ColumnDefinitions.Add(new ColumnDefinition()); //число кредитов
            topGrid.ColumnDefinitions.Add(new ColumnDefinition()); //число металла
            topGrid.ColumnDefinitions.Add(new ColumnDefinition()); //число микросхем
            topGrid.ColumnDefinitions.Add(new ColumnDefinition()); // число антиматерии
            topGrid.ColumnDefinitions.Add(new ColumnDefinition()); //число запасных деталей
            topGrid.ColumnDefinitions.Add(new ColumnDefinition()); //число кораблей
            nameLabel = new Label(); topGrid.Children.Add(nameLabel); nameLabel.Content = "Player`s Name";
            nameLabel.Foreground = Brushes.White;
            nameLabel.FontFamily = Links.Font;
            nameLabel.FontSize = 30;
            nameLabel.FontWeight = FontWeights.Bold;
            MoneyRes = new ResourcePanel(Links.Brushes.MoneyImageBrush, topGrid, 1, true);
            MetalRes = new ResourcePanel(Links.Brushes.MetalImageBrush, topGrid, 2,false);
            ChipsRes = new ResourcePanel(Links.Brushes.ChipsImageBrush, topGrid, 3, false);
            AntiRes = new ResourcePanel(Links.Brushes.AntiImageBrush, topGrid, 4, false);
            //ZipRes = new ResourcePanel(Links.Brushes.ZipImageBrush, topGrid, 5,false);
            ShipsCount = new ResourcePanel(Links.Brushes.ShipImageBrush, topGrid, 5, false);
            /*
            Shop_Button = new Button();
            Shop_Button.Width = 120;
            Shop_Button.Height = 30;
            Shop_Button.Content = Links.Interface("Buy_Premium");
            Shop_Button.FontFamily = Links.Font;
            Shop_Button.FontSize = 20;
            Shop_Button.Background = Brushes.Gold;
            topGrid.Children.Add(Shop_Button);
            Grid.SetColumn(Shop_Button, 6);
            Shop_Button.Margin = new Thickness(-100, 0, 0, 0);
            Shop_Button.Click += new RoutedEventHandler(Shop_Button_Click);
            */
           /* QuestButton = new QuestButton();
            topGrid.Children.Add(QuestButton);
            Grid.SetColumn(QuestButton, 6);
            QuestButton.Margin = new Thickness(100, 0, 0, 0);
            QuestButton.SetQuest();*/
          
            Buttons = new List<Path>();
            middlecanvas = new Canvas();
            middlecanvas.Width = 1280;
            middlecanvas.Height = 60;
            //StackPanel middlePanel = new StackPanel(); 
            //middlePanel.Orientation = Orientation.Horizontal;
            Border middlePanelBorder = new Border();
            //middlePanelBorder.Child = middlePanel; 
            middlePanelBorder.Child = middlecanvas;
            mainGrid.Children.Add(middlePanelBorder);
            Grid.SetRow(middlePanelBorder, 1); 
            middlePanelBorder.BorderThickness = new Thickness(5);
            middlePanelBorder.BorderBrush = Brushes.White;
            Buttons.Add(GetButtonPath(middlecanvas, 50, 10, 0, 0));
            Buttons.Add(GetButtonPath(middlecanvas, 150, 10, 1, 2));
            Buttons.Add(GetButtonPath(middlecanvas, 250, 10, 2, 2));
            Buttons.Add(GetButtonPath(middlecanvas, 350, 10, 3, 2));
            Buttons.Add(GetButtonPath(middlecanvas, 450, 10, 4, 2));
            Buttons.Add(GetButtonPath(middlecanvas, 550, 10, 5, 2));
            Buttons.Add(GetButtonPath(middlecanvas, 650, 10, 6, 2));
            Buttons.Add(GetButtonPath(middlecanvas, 750, 10, 7 ,2));
            Buttons.Add(GetButtonPath(middlecanvas, 850, 10, 8, 2));
            Buttons.Add(GetButtonPath(middlecanvas, 950, 10, 9, 2));
            Buttons.Add(GetButtonPath(middlecanvas, 1050, 10, 10, 1));


           

            LowBorder = new Border(); mainGrid.Children.Add(LowBorder); Grid.SetRow(LowBorder, 2);
            LowBorder.BorderThickness = new Thickness(5); LowBorder.BorderBrush = Brushes.White;

        }

       /*
        public void SetPremium(int value)
        {
            if (value==0)
            {
                Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate { Shop_Button.Content = Links.Interface("Buy_Premium"); }));
                //Shop_Button.Content = Links.Interface("Buy_Premium");
            }
            else if (value>9000)
            {
                Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate { Shop_Button.Content = Links.Interface("Premium_Forever"); }));
                //Shop_Button.Content = Links.Interface("Premium_Forever");
            }
            else
            {
                Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate { Shop_Button.Content = String.Format(Links.Interface("Premium_Left"), value.ToString()); }));
                //Shop_Button.Content = String.Format(Links.Interface("Premium_Left"), value.ToString());
            }
        }*/
        /*
        void Shop_Button_Click(object sender, RoutedEventArgs e)
        {
            Links.Controller.PopUpCanvas.Place(new PremiumWindow(GSGameInfo.Premium), false);
        }

  */
       
        Path GetButtonPath(Canvas canvas, int left, int top, int tag, int type)
        {
            Path path = new Path();
            //path.Stroke = Brushes.White;
            switch (type)
            {
                case 0: 
                    path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,15 l20,-20 a5,5 0 0,0 5,5 h75 v30 h-75 a5,5 0 0,0 -5,5z"));
                    break;
                case 1:
                    path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M100,15 l-20,20 a5,5 0 0,0 -5,-5 h-75 v-30 h75 a5,5 0 0,0 5,-5z"));
                    break;
                case 2:
                    path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M0,0 h100 v30 h-100 z"));
                    break;

            }
            canvas.Children.Add(path);
            Canvas.SetLeft(path, left);
            Canvas.SetTop(path, top);
            path.Fill = Links.Brushes.NotSelectedMainBrush;
            path.Tag = tag;
            return path;
        }
        
        
        
        public class ResourcePanel : StackPanel
        {
            public Label imageLabel;
            public TextBlock valueLabel;
            int Capacity=0;
            int IntValue=0;
            long LongValue = 0;
            //DoubleAnimation Increase;
            //DoubleAnimation Decrease;
            bool IsLong;
            static double[] stepsvalue = new double[] { 0, 0.3, 0.55, 0.75, 0.9, 0.95, 1.0 };
            static double[] stepstime = new double[] { 0, 0.05, 0.15, 0.3, 0.5, 0.75, 1.0 };
            public ResourcePanel(Brush image, Grid grid, int pos, bool isLong)
            {
                Orientation = Orientation.Horizontal;
                imageLabel = new Label();
                imageLabel.Content = Common.GetRectangle(30, image);
                this.Children.Add(imageLabel);
                valueLabel = new TextBlock();
                valueLabel.FontFamily = Links.Font;
                valueLabel.FontSize = 20;
                //valueLabel.Style = Links.TextStyle;
                valueLabel.Foreground = Brushes.White;
                valueLabel.FontWeight = FontWeights.Bold;
                valueLabel.Margin = new Thickness(0, 10, 0, 0);
                this.Children.Add(valueLabel);
                grid.Children.Add(this);
                Grid.SetColumn(this, pos);
                //Increase = new DoubleAnimation(20, 25, TimeSpan.FromSeconds(1));
                //Increase.Completed+=new EventHandler(Increase_Completed);
                //Decrease = new DoubleAnimation(25, 20, TimeSpan.FromSeconds(1));
                //Decrease.Completed += new EventHandler(Decrease_Completed);
                IsLong = isLong;
                //Calculate(0);
            }
            public void SetValue(long value, byte IsGrow)
            {
                Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
                {
                    long delta = value - LongValue;
                    StringAnimationUsingKeyFrames anim = new StringAnimationUsingKeyFrames();
                    anim.Duration = TimeSpan.FromSeconds(1);
                    anim.Completed += Anim_Completed;
                    for (int i = 0; i < 7; i++)
                        anim.KeyFrames.Add(new DiscreteStringKeyFrame((LongValue + delta * stepsvalue[i]).ToString("### ### ### ### ### ###"), KeyTime.FromPercent(stepstime[i])));
                    if (IsGrow == 1)
                    {
                        valueLabel.Foreground = Brushes.Red;
                        valueLabel.BeginAnimation(TextBlock.TextProperty, anim);
                    }
                    else
                    {
                        valueLabel.Foreground = Brushes.Green;
                        valueLabel.BeginAnimation(TextBlock.TextProperty, anim);
                    }
                    LongValue = value;
                }));
            }
            private void Anim_Completed(object sender, EventArgs e)
            {
                Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate { valueLabel.Foreground = Brushes.White; }));
            }
            public void SetValue(int value, byte IsGrow)
            {
                Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate
                {
                    long delta = value - IntValue;
                    StringAnimationUsingKeyFrames anim = new StringAnimationUsingKeyFrames();
                    anim.Duration = TimeSpan.FromSeconds(1);
                    anim.Completed += Anim_Completed;
                    for (int i = 0; i < 7; i++)
                        anim.KeyFrames.Add(new DiscreteStringKeyFrame((IntValue + delta * stepsvalue[i]).ToString("### ### ### ###"), KeyTime.FromPercent(stepstime[i])));
                    valueLabel.BeginAnimation(TextBlock.TextProperty, anim);
                    if (IsGrow == 1)
                    {
                        valueLabel.Foreground = Brushes.Red;
                        valueLabel.BeginAnimation(TextBlock.TextProperty, anim);
                    }
                    else
                    {
                        valueLabel.Foreground = Brushes.Green;
                        valueLabel.BeginAnimation(TextBlock.TextProperty, anim);
                    }
                    IntValue = value;
                }));
                
            }
          
            public void SetCapacity(int value, byte IsGrow)
            {
                Capacity = value;
                Dispatcher.BeginInvoke(new System.Threading.ThreadStart(delegate { valueLabel.Text=value.ToString(); }));
            }
            
        }
        #endregion
       
        private double LastHeight, LastWidth;
        private System.Windows.WindowState LastState;
        private void SetFullScreen()
        {
            LastHeight = Height;
            LastWidth = Width;
            LastState = WindowState;

            //Topmost = true;
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            Top = 0;
            Left = 0;
            WindowState = System.Windows.WindowState.Normal;
            WindowStyle = WindowStyle.None;
            ResizeMode = System.Windows.ResizeMode.NoResize;
        }
        private void SetNormalSize()
        {
            WindowStyle = WindowStyle.SingleBorderWindow;
            WindowState = LastState;
            ResizeMode = ResizeMode.CanResizeWithGrip;
            //Topmost = false;
            Width = 960;
            Height = 540;
        }
    }
}
