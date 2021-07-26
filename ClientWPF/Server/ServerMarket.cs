using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    /// <summary> Тип рынка 
    /// - местный, малый ассортимент, меняется сам,
    /// - галактический - хороший ассортимент, меняется за деньги, посещение стоит денег
    /// - чёрный - отличный ассортимент, посещение один раз - по миссии, забрать ресурсы можно только после боя (необходим флот)  </summary>
    public enum MarketType { Local, Galaxy, Black}
    public class ServerMarket
    {
        MarketType Type;
        public SortedList<int, MarketElement> Elements;
        public GSPlayer Player;
        public ServerFleet Fleet;
        Random Rnd;
        public byte[] MarketArray;
        public DateTime EndTime;//Время закрытия рынка. Локальный по окончании обновляется, Галактический - распускается
        public SortedList<RewardType, int> ResourcesBuyed;
        public List<BuyedElement> BuyedElements;
        public ServerMarket(GSPlayer player, MarketType type)
        {
            Player = player;
            Type = type;
            ResourcesBuyed = new SortedList<RewardType, int>();
            ResourcesBuyed.Add(RewardType.Metall, 0);
            ResourcesBuyed.Add(RewardType.Chips, 0);
            ResourcesBuyed.Add(RewardType.Anti, 0);
            BuyedElements = new List<BuyedElement>();
            Rnd = new Random();
            Generate();
            CreateArray();
        }
        public bool ChangeResources(int element, int quantity)
        {
            if (Elements.ContainsKey(element) == false) return false;
            MarketElement Melement = Elements[element];
            int resources;
            if (Melement.PriceType == RewardType.Money)
                resources = (int)(Math.Round(quantity / Melement.PriceForOne));
            else
                resources = quantity;
            if (Melement.Quantity < resources) return false;
            switch (Melement.PriceType)
            {
                case RewardType.Money: if (Player.Resources._Money<quantity) return false;
                    switch (Melement.ElementType)
                    {
                        case RewardType.Metall: ResourcesBuyed[RewardType.Metall] += resources; break;
                        case RewardType.Chips: ResourcesBuyed[RewardType.Chips] += resources; break;
                        case RewardType.Anti: ResourcesBuyed[RewardType.Anti] += resources; break;
                        case RewardType.Pilot: BuyedElements.Add(new BuyedElement(RewardType.Pilot, Melement.ElementDescription)); break;
                        case RewardType.Artefact: BuyedElements.Add(new BuyedElement(RewardType.Artefact, Melement.ElementDescription)); break;
                        case RewardType.Ship: BuyedElements.Add(new BuyedElement(RewardType.Ship, Melement.ElementDescription)); break;
                        case RewardType.Science: BuyedElements.Add(new BuyedElement(RewardType.Science, Melement.ElementDescription)); break;
                        case RewardType.Avanpost: BuyedElements.Add(new BuyedElement(RewardType.Avanpost, Melement.ElementDescription)); break;
                        default: return false;
                    }
                    Player.Resources.RemovePrice(quantity);
                    Melement.Quantity -= resources;
                    break;
                case RewardType.Metall: if (Player.Resources._Metals+ResourcesBuyed[RewardType.Metall] < quantity) return false;
                    ResourcesBuyed[RewardType.Metall] -= quantity;
                    if (ResourcesBuyed[RewardType.Metall]<0)
                    {
                        Player.Resources.RemovePrice(new ItemPrice(0, -ResourcesBuyed[RewardType.Metall], 0, 0));
                        ResourcesBuyed[RewardType.Metall] = 0;
                    }
                    Melement.Quantity -= quantity;
                    Player.Resources.AddMoney((int)Math.Round(quantity * Melement.PriceForOne, 0));
                    break;
                case RewardType.Chips: if (Player.Resources._Chips+ResourcesBuyed[RewardType.Chips] < quantity) return false;
                    ResourcesBuyed[RewardType.Chips] -= quantity;
                    if (ResourcesBuyed[RewardType.Chips] < 0)
                    {
                        Player.Resources.RemovePrice(new ItemPrice(0,0, -ResourcesBuyed[RewardType.Chips],  0));
                        ResourcesBuyed[RewardType.Chips] = 0;
                    }
                    Melement.Quantity -= quantity;
                    Player.Resources.AddMoney((int)Math.Round(quantity * Melement.PriceForOne, 0));
                    break;
                case RewardType.Anti: if (Player.Resources._Anti+ResourcesBuyed[RewardType.Anti] < quantity) return false;
                    ResourcesBuyed[RewardType.Anti] -= quantity;
                    if (ResourcesBuyed[RewardType.Anti] < 0)
                    {
                        Player.Resources.RemovePrice(new ItemPrice(0, 0,0,-ResourcesBuyed[RewardType.Anti]));
                        ResourcesBuyed[RewardType.Anti] = 0;
                    }
                    Melement.Quantity -= quantity;
                    Player.Resources.AddMoney((int)Math.Round(quantity * Melement.PriceForOne, 0));
                    break;
                default: return false;
            }
            CreateArray();
            return true;
        }
        public long LeaveMarket(bool winblackbattle)
        {
            if (winblackbattle || Type != MarketType.Black)
            {
                Player.Resources.AddResources(new BuildingValues(0, ResourcesBuyed[RewardType.Metall], ResourcesBuyed[RewardType.Chips], ResourcesBuyed[RewardType.Anti]));
                ResourcesBuyed[RewardType.Metall] = 0;
                ResourcesBuyed[RewardType.Chips] = 0;
                ResourcesBuyed[RewardType.Anti] = 0;
                foreach (BuyedElement element in BuyedElements)
                {
                    switch (element.Type)
                    {
                        case RewardType.Pilot:
                            Player.Pilots.AddPilot(new GSPilot(element.Description, 0)); break;
                        case RewardType.Artefact:
                            Player.AddArtefacts(new int[] { BitConverter.ToUInt16(element.Description, 0) }); break;
                        case RewardType.Ship:
                            int s = 0;
                            Schema schema = Schema.GetSchema(element.Description, ref s);
                            byte[] model = new byte[4];
                            Array.Copy(element.Description, s, model, 0, 4);
                            ServerShip ship = ServerShip.GetNewShip(schema, model);
                            Player.AddShip(ship);
                            break;
                        case RewardType.Science:
                            ushort scienceid = BitConverter.ToUInt16(element.Description, 0);
                            Player.Sciences.PutScienceToArray(scienceid);
                            break;
                        case RewardType.Avanpost:
                            int planetid = BitConverter.ToInt32(element.Description, 0);
                            ServerPlanet planet = ServerLinks.GSPlanets[planetid];
                            Player.AddAvanpost(planetid, true, null);
                            break;
                    }
                }
                BuyedElements.Clear();
                Player.CreateArrayForPlayer();
                CreateArray();
                return -1;
            }
            else
            {
                ServerMission2 mission = (ServerMission2)Fleet.Target.Target;
                ServerBattle battle = new ServerBattle(Fleet, mission);
                return battle.ID;
            }
        }
        public static int GetGlobalChangePrice(GSPlayer player)
        {
            return 10000;
        }
        void CreateArray()
        {
            List<byte> list = new List<byte>();
            list.Add((byte)Type);
            list.AddRange(BitConverter.GetBytes(EndTime.Ticks));
            list.Add((byte)Elements.Count);
            foreach (KeyValuePair<int, MarketElement> pair in Elements)
            {
                list.Add((byte)pair.Key);
                list.AddRange(pair.Value.GetArray());
            }
            foreach (KeyValuePair<RewardType, int> pair in ResourcesBuyed)
                list.AddRange(BitConverter.GetBytes(pair.Value));
            list.Add((byte)BuyedElements.Count);
            foreach (BuyedElement element in BuyedElements)
            {
                list.Add((byte)element.Type);
                list.AddRange(BitConverter.GetBytes(element.Description.Length));
                list.AddRange(element.Description);
            }
            MarketArray = list.ToArray();
        }
        /// <summary> метод генерирует наполнение рынка - 12 элементов 
        /// возможные варианты - покупка всех типов ресурсов, артефактов, технологий, пилотов, кораблей, планет, аванпостов (за деньги)
        /// - продажа ресурсов за деньги
        /// </summary>
        void Generate()
        {
            switch (Type)
            {
                case MarketType.Local: int curmonth = ServerLinks.NowTime.Month;
                    if (curmonth != 12)
                        EndTime = new DateTime(ServerLinks.NowTime.Year, curmonth + 1, 1);
                    else
                        EndTime = new DateTime(ServerLinks.NowTime.Year + 1, curmonth, 1);
                    break;
                case MarketType.Galaxy: EndTime = ServerLinks.NowTime + TimeSpan.FromDays(60); break;
                case MarketType.Black: EndTime = ServerLinks.NowTime + TimeSpan.FromDays(1) ; break;
            }
            Elements = new SortedList<int, MarketElement>();
            Random rnd = new Random();
            //генерация типов
            Elements[0] = new MarketElement(RewardType.Metall, RewardType.Money);
            Elements[1] = new MarketElement(RewardType.Money, RewardType.Metall);
            Elements[2] = new MarketElement(RewardType.Chips, RewardType.Money);
            Elements[3] = new MarketElement(RewardType.Money, RewardType.Chips);
            Elements[4] = new MarketElement(RewardType.Anti, RewardType.Money);
            Elements[5] = new MarketElement(RewardType.Money, RewardType.Anti);
            Elements[6] = new MarketElement(RewardType.Ship, RewardType.Money);
            Elements[7] = new MarketElement(RewardType.Pilot, RewardType.Money);
            Elements[8] = new MarketElement(RewardType.Artefact, RewardType.Money);
            Elements[9] = new MarketElement(RewardType.Artefact, RewardType.Money);
            Elements[10] = new MarketElement(RewardType.Pilot, RewardType.Money);
            Elements[11] = new MarketElement(RewardType.Pilot, RewardType.Money);
            Elements[12] = new MarketElement(RewardType.Science, RewardType.Money);
            Elements[13] = new MarketElement(RewardType.Ship, RewardType.Money);
            Elements[14] = new MarketElement(RewardType.Avanpost, RewardType.Money);

            //Если рынок чёрный - то возможно сбросить несколько базовых типов
            //заполнение типов
            List<int> BadElements = new List<int>();
            for(int i = 0;i<Elements.Count;i++)
            {
                MarketElement element = Elements.Values[i];
                bool createresult = true;
                switch (element.ElementType)
                {
                    case RewardType.Metall: FillElementResourceBuy(element); break;
                    case RewardType.Chips: FillElementResourceBuy(element); break;
                    case RewardType.Anti: FillElementResourceBuy(element); break;
                    case RewardType.Money: FillElementResourceSell(element); break;
                    case RewardType.Pilot: FillElementPilotBuy(element); break;
                    case RewardType.Artefact: FillElementArtefactBuy(element); break;
                    case RewardType.Ship: createresult = FillElementShipBuy(element); break;
                    case RewardType.Science: createresult = FillScienceBuy(element); break;
                    case RewardType.Avanpost: createresult = FillAvanpostBuy(element); break;
                }
                if (createresult == false)
                    BadElements.Add(i);
            }
            foreach (int i in BadElements)
                Elements.Remove(i);
        }
        bool FillAvanpostBuy(MarketElement element)
        {
            List<ServerPlanet> list = new List<ServerPlanet>();
            foreach (ServerPlanet planet in ServerLinks.GSPlanets.Values)
            {
                if (planet.MaxPopulation == 0) continue;
                if (planet.QuestPlanet == true) continue;
                if (planet.Lands.Count + planet.NewLands.Count > 0) continue;
                list.Add(planet);
            }
            if (list.Count == 0) return false;
            ServerPlanet result = list[Rnd.Next(list.Count)];
            element.ElementDescription = BitConverter.GetBytes(result.ID);
            element.Quantity = 1;
            long price = Common.GetAvanpostPrice(result, Player);
            switch (Type)
            {
                case MarketType.Local: element.PriceForOne = Math.Round(price / 1000.0 * (5 + 5 * Rnd.NextDouble())) * 1000; break;
                case MarketType.Galaxy: element.PriceForOne = Math.Round(price / 1000.0 * (4 + 4 * Rnd.NextDouble())) * 1000; break;
                case MarketType.Black: element.PriceForOne = Math.Round(price / 1000.0 * (3 + 3 * Rnd.NextDouble())) * 1000; break;
            }
            return true;
        }
        bool FillScienceBuy(MarketElement element)
        {
            int scienceinfo = -1;
            for (int i = 0; i < 10; i++)
            {
                switch (Type)
                {
                    case MarketType.Local: scienceinfo = Player.Sciences.SelectNewScienceToLearnRandom(80,20,0); break;
                    case MarketType.Galaxy: scienceinfo = Player.Sciences.SelectNewScienceToLearnRandom(40,40,20); break;
                    case MarketType.Black: scienceinfo = Player.Sciences.SelectNewScienceToLearnRandom(20,50,30); break;
                }
                if (scienceinfo > 0) break;
            }
            if (scienceinfo < 0) return false;
            GameScience science = ServerLinks.Science.GameSciences[(ushort)scienceinfo];
            element.Quantity = 1;
            element.PriceForOne = 500*Math.Pow(1.33,science.Level)+500*Math.Pow(science.Level,3);
            switch (Type)
            {
                case MarketType.Local:
                    element.PriceForOne = Math.Round(element.PriceForOne / 1000 * (1.1 + 1.4 * Rnd.NextDouble()), 0) * 1000; break;
                case MarketType.Galaxy:
                    element.PriceForOne = Math.Round(element.PriceForOne / 1000 * (0.5 + 1.5 * Rnd.NextDouble()), 0) * 1000; break;
                case MarketType.Black:
                    element.PriceForOne = Math.Round(element.PriceForOne / 1000 * (0.2 + 1.6 * Rnd.NextDouble()), 0) * 1000; break;
            }
            element.ElementDescription = BitConverter.GetBytes(scienceinfo);
            return true;
        }
        bool FillElementShipBuy(MarketElement element)
        {
            byte level = (byte)(Rnd.Next(10,51));
            Schema Schema = SchemaGenForMarket.GetSchema(level);
            if (Schema == null) return false;
            List<byte> list = new List<byte>();
            list.AddRange(Schema.GetCrypt());
            list.AddRange(new byte[] { (byte)(Rnd.Next(1,4)), (byte)(Rnd.Next(1,11)), (byte)(Rnd.Next(1, 11)), (byte)(Rnd.Next(1, 11)) });
            element.ElementDescription = list.ToArray();
            Schema.CalcPrice();
            int baseprice = Schema.Price.Money + (Schema.Price.Metall + Schema.Price.Chips + Schema.Price.Anti) * 10;
            switch (Type)
            {
                case MarketType.Black: element.PriceForOne = Math.Round(baseprice * (0.5 + Rnd.NextDouble() * 3) / 1000, 0) * 1000; break;
                case MarketType.Galaxy: element.PriceForOne = Math.Round(baseprice * (0.8 + Rnd.NextDouble() * 4.7) / 1000, 0) * 1000; break;
                case MarketType.Local: element.PriceForOne = Math.Round(baseprice * (1.2 + Rnd.NextDouble() * 6.3) / 1000, 0) * 1000; break;
            }
            element.Quantity = 1;
            return true;
        }
        void FillElementArtefactBuy(MarketElement element)
        {
            int price = 50000;
            Artefact art = Artefact.GetRandomArt(price);
            element.ElementDescription = BitConverter.GetBytes(art.ID);
            switch (Type)
            {
                case MarketType.Local: element.PriceForOne = (int)((Rnd.NextDouble() * 1.5 + 1.5) * art.Price); break;
                case MarketType.Galaxy: element.PriceForOne = (int)((Rnd.NextDouble() * 1.1 + 0.9) * art.Price); break;
                case MarketType.Black: element.PriceForOne = (int)((Rnd.NextDouble() * 0.9 + 0.6) * art.Price); break;
            }
            element.PriceForOne = Math.Round(element.PriceForOne / 1000, 0) * 1000;
            element.Quantity = 1;
        }
        void FillElementPilotBuy(MarketElement element)
        {
            int bonus = 0;
            switch (Type)
            {
                case MarketType.Galaxy: bonus = 2; break;
                case MarketType.Black: bonus = 5; break;
            }
            element.ElementDescription = GSPilot.GetNewPilot(20, bonus);
            GSPilot pilot = new GSPilot(element.ElementDescription, 0);
            element.PriceForOne = (Math.Pow(pilot.Rang, 3)+1) * (Math.Pow(pilot.Talent, 2)+1) * (Math.Pow(pilot.Level, 2)+1) * 1000;
            switch (Type)
            {
                case MarketType.Local:
                    element.PriceForOne = Math.Round(element.PriceForOne * (1.1 + 2.2 * Rnd.NextDouble()) / 1000, 0) * 1000; break;
                case MarketType.Galaxy:
                    element.PriceForOne = Math.Round(element.PriceForOne * (0.8 + 1.7 * Rnd.NextDouble()) / 1000, 0) * 1000; break;
                case MarketType.Black:
                    element.PriceForOne = Math.Round(element.PriceForOne * (0.5 + 1.5 * Rnd.NextDouble()) / 1000, 0) * 1000; break;
            }
            element.Quantity = 1;
            
        }
        void FillElementResourceBuy(MarketElement element)
        {
            element.ElementDescription = new byte[0];
            switch (Type)
            {
                case MarketType.Local: element.PriceForOne = Math.Round(Rnd.NextDouble() * (30 - 16) + 16,1);
                    break;
                case MarketType.Galaxy: element.PriceForOne = Math.Round(Rnd.NextDouble() * (20 - 13) + 13,1); break;
                case MarketType.Black: element.PriceForOne = Math.Round(Rnd.NextDouble() * (15 - 10) + 10, 1); break;
            }
            element.Quantity = 1000;
        }
        void FillElementResourceSell(MarketElement element)
        {
            element.ElementDescription = new byte[0];
            switch (Type)
            {
                case MarketType.Local:
                    element.PriceForOne = Math.Round(Rnd.NextDouble() * (7 - 3) + 3, 1);
                    break;
                case MarketType.Galaxy: element.PriceForOne = Math.Round(Rnd.NextDouble() * (9 - 6) + 6, 1); break;
                case MarketType.Black: element.PriceForOne = Math.Round(Rnd.NextDouble() * (10 - 8) + 8, 1); break;
            }
            element.Quantity = 1000;
        }
    }
    public class MarketElement
    {
        public RewardType ElementType;
        public byte[] ElementDescription;
        public int Quantity;
        public RewardType PriceType;
        public double PriceForOne;
        public  MarketElement(RewardType etype, RewardType pricetype)
        {
            ElementType = etype;
            Quantity = 1;
            PriceType = pricetype;
            PriceForOne = 1.0;
        }
        public MarketElement(byte[] array, ref int i)
        {
            ElementType = ((RewardType)array[i]); i++;
            int descrlength = BitConverter.ToInt32(array, i); i += 4;
            ElementDescription = new byte[descrlength];
            Array.Copy(array, i, ElementDescription, 0, descrlength);
            i += descrlength;
            Quantity = BitConverter.ToInt32(array, i); i += 4;
            PriceType = ((RewardType)array[i]); i++;
            PriceForOne = BitConverter.ToDouble(array, i); i += 8;
        }
        public byte[] GetArray()
        {
            List<byte> list = new List<byte>();
            list.Add((byte)ElementType);
            list.AddRange(BitConverter.GetBytes(ElementDescription.Length));
            list.AddRange(ElementDescription);
            list.AddRange(BitConverter.GetBytes(Quantity));
            list.Add((byte)PriceType);
            list.AddRange(BitConverter.GetBytes(PriceForOne));
            return list.ToArray();
        }

    }
    public class BuyedElement
    {
        public RewardType Type;
        public byte[] Description;
        public BuyedElement(RewardType type, byte[] description)
        {
            Type = type; Description = description;
        }
    }
}
