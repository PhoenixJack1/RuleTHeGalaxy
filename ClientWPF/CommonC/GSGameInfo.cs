using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;

namespace Client
{
    class GSGameInfo
    {
        public static string Login;
        public static long random;
        

        //public static int Count;
        public static string PlayerName { get; private set; }
        public static GSClan Clan;
        public static void SetServerTime()
        {
            Links.Controller.MainCanvas.NamePanel.SetText(Interface.GetDate(ServerTime));
        }
        public static void SetPlayerName(GSString name)
        {
            PlayerName = name.ToString();
            Links.Controller.MainCanvas.NamePanel.SetText(PlayerName);
           //Links.Controller.mainWindow.nameLabel.Content = name.ToString();
        }
        public static long Money {get; private set;}
        public static int Metals {get; private set;}
        public static int Chips {get; private set;}
        public static int Anti {get; private set;}
        public static GSRes Capacity { get; private set; }
        public static SortedList<long, GSShip> Ships = new SortedList<long, GSShip>();
        public static int ShipsCap { get; private set; }
        public static void SetMoney(long value)
        { if (Money != value) { Links.Controller.MainCanvas.MoneyPanel.SetLongValue(value, Money); Money = value; } }
        public static void SetMetals(int value)
        { if (Metals != value) { Links.Controller.MainCanvas.MetalPanel.SetIntValue(value, Metals); Metals = value; }
            Links.Controller.MainCanvas.MetalCap.SetCapacity(Metals / (double)Capacity.Metal);
        }
        public static void SetChips(int value)
        { if (Chips != value) { Links.Controller.MainCanvas.ChipsPanel.SetIntValue(value, Chips); Chips = value; }
            Links.Controller.MainCanvas.ChipsCap.SetCapacity(Chips / (double)Capacity.Chips);
        }
        public static void SetAnti(int value)
        { if (Anti != value) { Links.Controller.MainCanvas.AntiPanel.SetIntValue(value, Anti) ; Anti = value; }
            Links.Controller.MainCanvas.AntiCap.SetCapacity(Anti / (double)Capacity.Anti);
        }
        public static void SetPremium(int value)
        {
            //Links.Controller.mainWindow.SetPremium(value);
        }
        public static void CalculateLandsParams()
        {
            GSRes capacity = new GSRes();
            capacity.Metal = ServerLinks.Parameters.PlayerMetallBaseCapacity;
            capacity.Chips = ServerLinks.Parameters.PlayerChipsBaseCapacity;
            capacity.Anti = ServerLinks.Parameters.PlayerAntiBaseCapacity;
            //int shipsCap = 0;
            foreach (Land land in PlayerLands.Values)
            {
                capacity.Metal += (int)land.GetAddParameter(Building2Parameter.MetalCap);
                capacity.Chips += (int)land.GetAddParameter(Building2Parameter.ChipCap);
                capacity.Anti += (int)land.GetAddParameter(Building2Parameter.AntiCap);
                //shipsCap += land.Capacity.Ships;
            }
            /*
            if (Capacity.Metal != capacity.Metal)
                Links.Controller.mainWindow.MetalRes.SetCapacity(capacity.Metal, (byte)(Capacity.Metal < capacity.Metal ? 2 : 1));
            if (Capacity.Chips != capacity.Chips)
                Links.Controller.mainWindow.ChipsRes.SetCapacity(capacity.Chips, (byte)(Capacity.Chips < capacity.Chips ? 2 : 1));
            if (Capacity.Anti != capacity.Anti)
                Links.Controller.mainWindow.AntiRes.SetCapacity(capacity.Anti, (byte)(Capacity.Anti < capacity.Anti ? 2 : 1));
    */
            //if (ShipsCap != shipsCap)
            //    Links.Controller.mainWindow.ShipsCount.SetCapacity(shipsCap, (byte)(ShipsCap < shipsCap ? 2 : 1));
            ShipsCap = 10;
            //ShipsCap = shipsCap;
            Capacity = capacity;
        }
        public static byte[] StatisticInfo = new byte[0];
        public static SortedSet<ushort> SciencesArray;
        public static int MaxScienceLevel;
        public static LandMult ScienceMult = new LandMult();
        //public static PlayerSciencesList SciencesList;
        public static int LastChatID = 0;
        public static List<Schema> PlayerSchemas = new List<Schema>();
        public static SortedList<int, Land> PlayerLands = new SortedList<int, Land>();
        public static SortedList<int, Avanpost> PlayerAvanposts = new SortedList<int, Avanpost>();
        public static int BasicMaxLandsCount = 0;
        public static int MaxLandsCount = 3;
        public static SortedList<long, GSFleet> Fleets = new SortedList<long, GSFleet>();
        public static List<GSPilot> AcademyPilotsList;
        public static List<GSPilot> FreePilots;
        public static DateTime ServerTime;
        public static DateTime UpdateTime;
        public static SortedList<long, Battle> Battles = new SortedList<long, Battle>();
        //public static List<Mission> Missions = new List<Mission>();
        public static byte[] HelpArray;
        //public static Premium Premium;
        public static Quest_Position Quest=Quest_Position.Q21_All_Done;
        public static byte StoryLinePosition=255;
        public static byte StoryStatus; //если 0 - миссия не начата, если 1 - выполняется
        public static SortedList<Artefact, ushort> Artefacts;
        public static bool CheckPrice(ItemPrice price)
        {
            string answer = "";
            if (GSGameInfo.Money < price.Money)
                answer="Not enough money";
            if (GSGameInfo.Metals < price.Metall)
                answer="Not enough metal";
            if (GSGameInfo.Chips < price.Chips)
            answer="Not enough chips";
            if (GSGameInfo.Anti < price.Anti)
                answer = "Not enough anti";
                if (answer != "")
                {
                    //SimpleInfoMessage window = new SimpleInfoMessage(answer);
                    //Links.Controller.PopUpCanvas.Place(window, true);
                    return false;
                }
            return true;
        }
        
    }


}
