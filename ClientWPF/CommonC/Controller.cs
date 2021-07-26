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
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace Client
{
    enum GamePanels { Galaxy, Colonies, ScienceCanvas, Nation, Market, ClansCanvas, SchemasCanvas, ShipsCanvas, FleetsCanvas, Artefacts}
    enum SelectModifiers {None, FleetForShip, Mission, Land, FleetForStory, FleetForMission, SectorForArtefact, FleetForArtefact,
        StarForScout, FleetForScout, FleetForPillage, FleetForConquer, BlackMarket, SpecialFleet}
    class Controllerclass
    {
        public MainWindow mainWindow;
        public LoadWindow loadWindow;
        public DebugWindow Debug;
        public MainCanvas MainCanvas;
        public LoginCanvas LoginCanvas;
        public Galaxy Galaxy;
        public Colonies Colonies;
        public LandChangerBase LandChangerBase;
        //public ScienceCanvas ScienceCanvas;
        //public Science2Canvas Science2Canvas;
        public Science3Canvas Science3Canvas;
        public Nation Nation;
        public Market Market;
        public NewMarket NewMarket;
        public ClansCanvas ClansCanvas;
        public SchemasCanvas SchemasCanvas;
        public NewSchemasCanvas NewSchemasCanvas;
        public ShipsCanvas ShipsCanvas;
        public FleetsCanvas FleetsCanvas;
        public ArtefactsCanvas ArtefactCanvas;
        //public BattleFieldCanvas BattleFieldCanvas;
        public IntBoya IntBoya;
        public System.Windows.Threading.DispatcherTimer MainTimer;
        public SelectModifiers SelectModifier;
        public GamePanels CurPanel;
        UIElement CurElement;
        public UIElement NextElement;
        public void SelectPanel(GamePanels panel, SelectModifiers modifier)
        {
            if (CurPanel == GamePanels.Market && NewMarket.ForceLeave == false)
            {
                bool leaveresult = NewMarket.LeaveCurrentMarket();
                if (leaveresult == false) return;
            }
            SelectModifier = modifier;
            CurPanel = panel;
            switch (panel)
            {
                case GamePanels.Galaxy:
                    NextElement = Galaxy;
                    //Links.Controller.MainCanvas.LowBorder.Child = Galaxy;
                    Galaxy.Select();
                    Links.Controller.MainCanvas.ElementChange(MenuElements.Galaxy);
                    break;
                case GamePanels.Colonies:
                    NextElement = LandChangerBase;
                    //Links.Controller.MainCanvas.LowBorder.Child = LandChangerBase;
                    LandChangerBase.Select();
                    Links.Controller.MainCanvas.ElementChange(MenuElements.Colony);
                    //Links.Controller.MainCanvas.LowBorder.Child = Colonies;
                    //Colonies.Select();
                    break;
                case GamePanels.ScienceCanvas:
                    NextElement = Science3Canvas.VBX;
                    Science3Canvas.Select();
                    Links.Controller.MainCanvas.ElementChange(MenuElements.Science);
                    /*
                    NextElement = Science2Canvas.VBX;
                    //Links.Controller.MainCanvas.LowBorder.Child = Science2Canvas.VBX;
                    Science2Canvas.Select();
                    Links.Controller.MainCanvas.ElementChange(MenuElements.Science);
    */                
                    break;
                case GamePanels.Nation:
                    NextElement = Nation.box;
                    //Links.Controller.MainCanvas.LowBorder.Child = Nation.box;
                    Nation.Select();
                    Links.Controller.MainCanvas.ElementChange(MenuElements.Nation);
                    break;
                case GamePanels.Market:
                    NextElement = NewMarket.box;
                    //NextElement = Market.box;
                    //Links.Controller.MainCanvas.LowBorder.Child = Market.box;
                    NewMarket.Select();
                    //Market.Select();
                    Links.Controller.MainCanvas.ElementChange(MenuElements.Nation);
                    break;
                case GamePanels.ClansCanvas:
                    NextElement = ClansCanvas;
                    //Links.Controller.MainCanvas.LowBorder.Child = ClansCanvas;
                    ClansCanvas.Select();
                    Links.Controller.MainCanvas.ElementChange(MenuElements.Nation);
                    break;
                case GamePanels.SchemasCanvas:
                    NextElement = NewSchemasCanvas.box;
                    //Links.Controller.MainCanvas.LowBorder.Child = NewSchemasCanvas.box;
                    NewSchemasCanvas.Select();
                    //Links.Controller.MainCanvas.LowBorder.Child = SchemasCanvas.box;
                    //SchemasCanvas.Select();
                    Links.Controller.MainCanvas.ElementChange(MenuElements.Nation);
                    break;
                case GamePanels.ShipsCanvas:
                    NextElement = ShipsCanvas.box;
                    //Links.Controller.MainCanvas.LowBorder.Child = ShipsCanvas.box;
                    ShipsCanvas.Select();
                    Links.Controller.MainCanvas.ElementChange(MenuElements.Nation);
                    break;
                case GamePanels.FleetsCanvas:
                    NextElement = FleetsCanvas.box;
                    //Links.Controller.MainCanvas.LowBorder.Child = FleetsCanvas.box;
                    FleetsCanvas.Select();
                    Links.Controller.MainCanvas.ElementChange(MenuElements.Fleets);
                    break;
                case GamePanels.Artefacts:
                    NextElement = ArtefactCanvas.box;
                    //Links.Controller.MainCanvas.LowBorder.Child = ArtefactCanvas.box;
                    ArtefactCanvas.Select();
                    Links.Controller.MainCanvas.ElementChange(MenuElements.Nation);
                    break;
            }
            if (CurElement == null)
                HideCurElement_Completed(null, null);
            else if (CurElement == NextElement)
            {

            }
            else
            {
                DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.1));
                anim.Completed += HideCurElement_Completed;
                CurElement.BeginAnimation(UIElement.OpacityProperty, anim);
                Links.Controller.MainCanvas.StopAutoChange();
            }
        }

        private void HideCurElement_Completed(object sender, EventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.1));
            NextElement.BeginAnimation(UIElement.OpacityProperty, anim);
            Links.Controller.MainCanvas.LowBorder.Child = NextElement;
            CurElement = NextElement;
        }

        bool _isError;
        public bool isError
        {
            get { return _isError; }
            set { if (value) { _isError = true; Commands.Stop_Game(); } }
        }
        
       
        
        public SelectedGrid LastGrid;
        public SelectedGrid CurrentGrid;


        //public Load2 LoadWindow2;
        public PopUpCanvas PopUpCanvas;
        public HelpCanvas HelpCanvas;
        public BattlePopUp BattlePopUp;
        public ShipPanelPopUp ShipPopUp;
        public RoundButtons RoundButtons;
        //public BattleCanvas BattleCanvas;
        public System.Windows.Controls.Primitives.Popup CurrentPopup;
      
        public Controllerclass(MainWindow window)
        {
            Links.Controller = this;
            Crypt.fill_Crypto();
            GSString.Prepare();
            Common.CreateLoginProxy();
            
            mainWindow = window;
            

            //LoadWindow2 = new Load2();
            PopUpCanvas = new PopUpCanvas();
            HelpCanvas = new HelpCanvas();
            BattlePopUp = new BattlePopUp();
            ShipPopUp = new ShipPanelPopUp();
            RoundButtons = new RoundButtons();
            loadWindow = new LoadWindow();
        }
        public void BuildMainWindowPanels()
        {
            Galaxy = new Galaxy();
            Colonies = new Colonies();
            LandChangerBase = new LandChangerBase();
            //ScienceCanvas = new ScienceCanvas();
            //Science2Canvas = new Science2Canvas();
            Science3Canvas = new Science3Canvas();
            Nation = new Nation();
            Market = new Market();
            NewMarket = new NewMarket();
            ClansCanvas = new ClansCanvas();
            NewSchemasCanvas = new NewSchemasCanvas();
            ShipsCanvas = new ShipsCanvas();
            FleetsCanvas = new FleetsCanvas();
            ArtefactCanvas = new ArtefactsCanvas();
            IntBoya = new IntBoya();
            Links.Controller.SelectPanel(GamePanels.Galaxy, SelectModifiers.None);

        }
        
    }
}
