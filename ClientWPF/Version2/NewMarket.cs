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
using System.Windows.Media.Animation;

namespace Client
{
    class NewMarket:Canvas
    {
        public enum NewMarketMode { Base, LocalMarket, GlobalMarket, BlackMarket}
        public Viewbox box;
        MarketEnterButton LocalMarketButton;
        MarketEnterButton GlobalMarketButton;
        MarketChangeButton LocalMarketChange;
        MarketChangeButton GlobalMarketChange;
        Border CreateGlobalMarket;
        public NewMarketMode CurMode;
        MarketWindow CurrentWindow;
        MarketInfo LocalMarket;
        MarketInfo GlobalMarket;
        MarketInfo BlackMarket;
        int GlobalChangePrice;
        public bool ForceLeave = false;
        public NewMarket()
        {
            box = new Viewbox();
            box.Width = 1600; box.Height = 750;
            box.HorizontalAlignment = HorizontalAlignment.Center;
            box.VerticalAlignment = VerticalAlignment.Center;
            box.Child = this;
            box.Stretch = Stretch.Fill;
            Width = 1280; Height = 600;
            
        }
        public void Select()
        {
            ForceLeave = false;
            if (Links.Controller.SelectModifier == SelectModifiers.BlackMarket)
                BuildBlackMarket();
            else
            {
                CurMode = NewMarketMode.Base;
                //Собирает базовую информацию о рынках с сервера. Включает время работы рынков, стоимость обновления и посещения
                UpdateInfo();
                Children.Clear();
                BuildBase();
            }
        }
        public void Refresh(MarketType markettype, int position)
        {
            if (CurMode==NewMarketMode.BlackMarket)
            {
                int i = 0;
                BlackMarket = new MarketInfo(Events.GetBlackMarket(), MarketType.Black, ref i);
            }
            else
                UpdateInfo();
            Gets.GetResources();
            Children.Clear();
            switch (markettype)
            {
                case MarketType.Local:MarketWindow lwindow = new MarketWindow(LocalMarket, position); Children.Add(lwindow); break;
                case MarketType.Galaxy: MarketWindow gwindow = new MarketWindow(GlobalMarket, position); Children.Add(gwindow); break;
                case MarketType.Black: MarketWindow bwindow = new MarketWindow(BlackMarket, position); Children.Add(bwindow); break;
            }
        }
        public bool LeaveCurrentMarket()
        {
            switch (CurMode)
            {
                case NewMarketMode.Base:
                    return true;
                case NewMarketMode.LocalMarket:
                case NewMarketMode.GlobalMarket:
                    StartBattleEventResult eventresult = Events.TryLeaveMarket((byte)(CurrentWindow.Info.Type));
                    if (eventresult.Result==false)
                        Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult.ErrorText));
                    else
                    {
                        Gets.GetResources();
                        Links.Controller.NewMarket.Select();
                    }
                    return true;
                case NewMarketMode.BlackMarket:
                    StartBattleEventResult battlestart = Events.TryLeaveMarket((byte)(CurrentWindow.Info.Type));
                    new StartBattlePanel(battlestart.Battleid);
                    ForceLeave = true;
                    Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.None);
                    return true;
                    //throw new Exception();
            }
            return false;
        }
        void BuildBlackMarket()
        {
            byte[] blackmarketarray = Events.GetBlackMarket();
            int i = 0;
            BlackMarket = new MarketInfo(blackmarketarray, MarketType.Black, ref i);
            CurMode = NewMarketMode.BlackMarket;
            Children.Clear();
            CurrentWindow = new MarketWindow(BlackMarket, 0);
            Children.Add(CurrentWindow);
        }
        void UpdateInfo()
        {
            byte[] markets = Events.GetMarkets();
            int i = 0;
            GlobalChangePrice = BitConverter.ToInt32(markets, i); i += 4;
            LocalMarket = new MarketInfo(markets, MarketType.Local, ref i);
            GlobalMarket = null;
            if (markets.Length > i)
                GlobalMarket = new MarketInfo(markets, MarketType.Galaxy, ref i);
        }
        public void SelectLocal(int pos)
        {
            CurMode = NewMarketMode.LocalMarket;
            Children.Clear();
            CurrentWindow = new MarketWindow(LocalMarket, pos);
            Children.Add(CurrentWindow);
        }
        public void SelectGalaxy(int pos)
        {
            CurMode = NewMarketMode.GlobalMarket;
            Children.Clear();
            CurrentWindow = new MarketWindow(GlobalMarket, pos);
            Children.Add(CurrentWindow);
        }
        void BuildBase()
        {
            LocalMarketButton = new MarketEnterButton(MarketType.Local);
            Children.Add(LocalMarketButton); Canvas.SetLeft(LocalMarketButton, 20); Canvas.SetTop(LocalMarketButton, 20);
            int endtime = (int)(LocalMarket.EndTime - ServerLinks.NowTime).TotalDays;
            LocalMarketButton.SetTime(endtime);
            LocalMarketChange = new MarketChangeButton();
            Children.Add(LocalMarketChange); Canvas.SetLeft(LocalMarketChange, 385); Canvas.SetTop(LocalMarketChange, 365);
            PutLocalChangeInfo((int)(LocalMarket.EndTime - ServerLinks.NowTime).TotalDays);
            if (GlobalMarket == null)
            {
                AddGlobalMarketButton();
                Children.Add(CreateGlobalMarket); Canvas.SetLeft(CreateGlobalMarket, 660); Canvas.SetTop(CreateGlobalMarket, 20);
            }
            else
            {
                GlobalMarketButton = new MarketEnterButton(MarketType.Galaxy);
                Children.Add(GlobalMarketButton); Canvas.SetLeft(GlobalMarketButton, 660); Canvas.SetTop(GlobalMarketButton, 20);
                GlobalMarketChange = new MarketChangeButton();
                Children.Add(GlobalMarketChange); Canvas.SetLeft(GlobalMarketChange, 1025); Canvas.SetTop(GlobalMarketChange, 365);
                GlobalMarketChange.PreviewMouseDown += GlobalMarketChange_PreviewMouseDown;
                endtime = (int)(GlobalMarket.EndTime - ServerLinks.NowTime).TotalDays;
                GlobalMarketButton.SetTime(endtime);
                PutGlobalChangeInfo(GlobalChangePrice);
            }
            
        }
        public void AddGlobalMarketButton()
        {
            CreateGlobalMarket = new Border(); CreateGlobalMarket.Width = 600; CreateGlobalMarket.Height = 580;
            CreateGlobalMarket.BorderBrush = Links.Brushes.SkyBlue; CreateGlobalMarket.BorderThickness = new Thickness(3);
            CreateGlobalMarket.CornerRadius = new CornerRadius(20); CreateGlobalMarket.Background = Brushes.Black;
            Canvas canvas = new Canvas();
            CreateGlobalMarket.Child = canvas;
            TextBlock Title = Common.GetBlock(40, "Организовать галактический рынок", Brushes.White, 580);
            canvas.Children.Add(Title);Canvas.SetLeft(Title, 10); Canvas.SetTop(Title, 10);
            Rectangle Image = Common.GetRectangle(200, Gets.AddPicI("Market_Galaxy"));
            canvas.Children.Add(Image); Canvas.SetLeft(Image,200); Canvas.SetTop(Image, 80);
            TextBlock Text = Common.GetBlock(30, "На галактическом рынке шире ассортимент предлагаемых товаров и более выгодные цены", Brushes.White, 580);
            canvas.Children.Add(Text); Canvas.SetLeft(Text, 10); Canvas.SetTop(Text, 300);

            List<Inline> list = new List<Inline>();
            list.Add(new Run("Созыв галактического"));
            list.Add(new LineBreak());
            list.Add(new Run("рынка будет стоить:"));
            list.Add(new LineBreak());
            Run r1 = new Run(GlobalChangePrice.ToString()); r1.Foreground = Links.Brushes.SkyBlue;
            list.Add(r1);
            list.Add(new InlineUIContainer(Common.GetRectangle(20, Links.Brushes.MoneyImageBrush)));
            TextBlock price = Common.GetBlock(36, "", Brushes.White, 580);
            price.Inlines.AddRange(list.ToArray());
            canvas.Children.Add(price); Canvas.SetLeft(price, 10); Canvas.SetTop(price, 400);

            CreateGlobalMarket.MouseEnter += CreateGlobalMarket_MouseEnter;
            CreateGlobalMarket.MouseLeave += CreateGlobalMarket_MouseLeave;
            CreateGlobalMarket.PreviewMouseDown += CreateGlobalMarket_PreviewMouseDown;
        }
        void ShowGlobalMarket(bool fromnull)
        {
            DoubleAnimation anim = new DoubleAnimation(660, 2000, TimeSpan.FromSeconds(0.5));
            if (fromnull)
                CreateGlobalMarket.BeginAnimation(Canvas.LeftProperty, anim);
            else
            {
                GlobalMarketButton.BeginAnimation(Canvas.LeftProperty, anim);
                GlobalMarketChange.BeginAnimation(Canvas.LeftProperty, anim);
            }
            GlobalMarketButton = new MarketEnterButton(MarketType.Galaxy);
            Children.Add(GlobalMarketButton); Canvas.SetLeft(GlobalMarketButton, 660); Canvas.SetTop(GlobalMarketButton, 20);
            GlobalMarketChange = new MarketChangeButton();
            Children.Add(GlobalMarketChange); Canvas.SetLeft(GlobalMarketChange, 1025); Canvas.SetTop(GlobalMarketChange, 365);
            GlobalMarketChange.PreviewMouseDown += GlobalMarketChange_PreviewMouseDown;
            int endtime = (int)(GlobalMarket.EndTime - ServerLinks.NowTime).TotalDays;
            GlobalMarketButton.SetTime(endtime);

            PutGlobalChangeInfo(GlobalChangePrice);
            DoubleAnimation anim1 = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));
            GlobalMarketButton.BeginAnimation(Border.OpacityProperty, anim1);
            GlobalMarketChange.BeginAnimation(Border.OpacityProperty, anim1);
        }
        private void CreateGlobalMarket_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (GSGameInfo.Money < GlobalChangePrice)
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage("Недостаточно средств"));
            else
            {
                string eventreuslt = Events.TryCreateGalaxyMarket();
                if (eventreuslt != "")
                    Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventreuslt));
                else
                {
                    Gets.GetResources();
                    UpdateInfo();
                    ShowGlobalMarket(true);
                }
            }
        }

        private void CreateGlobalMarket_MouseLeave(object sender, MouseEventArgs e)
        {
            CreateGlobalMarket.BorderThickness = new Thickness(3);
        }

        private void CreateGlobalMarket_MouseEnter(object sender, MouseEventArgs e)
        {
            CreateGlobalMarket.BorderThickness = new Thickness(10);
        }

        private void GlobalMarketChange_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (GSGameInfo.Money < GlobalChangePrice)
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage("Недостаточно средств"));
            else
            {
                string eventreuslt = Events.TryCreateGalaxyMarket();
                if (eventreuslt != "")
                    Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventreuslt));
                else
                {
                    Gets.GetResources();
                    UpdateInfo();
                    ShowGlobalMarket(false);
                }
            }
        }

        void PutLocalChangeInfo(int time)
        {
            List<Inline> list = new List<Inline>();
            list.Add(new Run("Обновление ассортимента:"));
            list.Add(new LineBreak());
            list.Add(new Run("ожидается через"));
            list.Add(new LineBreak());
            Run r1 = new Run(time.ToString()); r1.Foreground = Links.Brushes.SkyBlue;
            list.Add(r1);
            if (time == 1) list.Add(new Run(" день"));
            else if (time < 5) list.Add(new Run(" дня"));
            else list.Add(new Run(" дней"));
            LocalMarketChange.PutText(list);
        }
        void PutGlobalChangeInfo(int price)
        {
            List<Inline> list = new List<Inline>();
            list.Add(new Run("Созыв нового галактического"));
            list.Add(new LineBreak());
            list.Add(new Run("рынка будет стоить:"));
            list.Add(new LineBreak());
            Run r1 = new Run(price.ToString()); r1.Foreground = Links.Brushes.SkyBlue;
            list.Add(r1);
            list.Add(new InlineUIContainer(Common.GetRectangle(20, Links.Brushes.MoneyImageBrush)));
            GlobalMarketChange.PutText(list);
        }
    }
    /// <summary> Подробное окно с рынком </summary>
    class MarketWindow:Border
    {
        public MarketInfo Info;
        public int Position;
        public bool HasResources = false;
        Canvas Canvas;
        StackPanel WareHouse;
        Canvas ElementCanvas;
        ElementsSort Sort;
        public MarketWindow(MarketInfo info, int position)
        {
            Info = info; Position = position; Sort = new ElementsSort(Info);
            Width = 1280; Height = 600; BorderBrush = Links.Brushes.SkyBlue; BorderThickness = new Thickness(3);
            Canvas canvas = new Canvas(); Child = canvas;
            Canvas = canvas;
            Canvas closeCanvas = Common.GetCloseCanvas(false);
            canvas.Children.Add(closeCanvas);
            Canvas.SetLeft(closeCanvas, 1230); Canvas.SetTop(closeCanvas, -10);
            closeCanvas.PreviewMouseDown += CloseCanvas_PreviewMouseDown;
            Rectangle Image = Common.GetRectangle(70, null);
            TextBlock Title = Common.GetBlock(50, "", Brushes.White, 1000);
            canvas.Children.Add(Title); Canvas.SetLeft(Title, 50); Canvas.SetTop(Title, 10);
            switch (info.Type)
            {
                case MarketType.Local: Title.Text = "Локальный рынок"; Image.Fill = Gets.AddPicI("Market_Local"); break;
                case MarketType.Galaxy: Title.Text = "Галактический рынок"; Image.Fill = Gets.AddPicI("Market_Galaxy"); break;
                case MarketType.Black: Title.Text = "Чёрный рынок"; Image.Fill = Gets.AddPicI("Market_Black"); break;
            }
            canvas.Children.Add(Image); Canvas.SetLeft(Image, 10); Canvas.SetTop(Image, 10);
            //int minelement = 0;
            //if (info.Elements.Count>0)
            //    minelement = info.Elements.Keys[info.Elements.Count - 1];
            Border ElementsBorder = new Border(); ElementsBorder.ClipToBounds = true;
            ElementsBorder.Width = 950; ElementsBorder.Height = 500;
            ElementsBorder.BorderBrush = Links.Brushes.SkyBlue;
            ElementsBorder.BorderThickness = new Thickness(2);
            canvas.Children.Add(ElementsBorder);
            Canvas.SetLeft(ElementsBorder, 10); Canvas.SetTop(ElementsBorder, 90);
            Canvas TopCanvas = new Canvas(); ElementsBorder.Child = TopCanvas;
            ElementCanvas = new Canvas();
            ElementCanvas.Width = 900;
            ElementCanvas.Height = Sort.Array.Count * 210;
            //ElementCanvas.Height = (minelement / 4+1) * 210;
            TopCanvas.Children.Add(ElementCanvas);
            Canvas.SetTop(ElementCanvas, Position * -225);
            foreach (KeyValuePair<int, MarketElement> pair in Info.Elements)
            {
                ChangeElement element = new ChangeElement(pair.Value, pair.Key, this);
                ElementCanvas.Children.Add(element);
                Canvas.SetLeft(element, 12.5 + Sort.Positions[pair.Key][1] * 225);
                Canvas.SetTop(element, 12.5 + Sort.Positions[pair.Key][0] * 225);
            }
            
            //КНОПКИ СКРОЛА
            Path top = GetArrow(true);
            canvas.Children.Add(top);
            Canvas.SetLeft(top, 925); Canvas.SetTop(top, 95);
            top.PreviewMouseDown += ScrollTop_PreviewMouseDown;
            Path down = GetArrow(false);
            canvas.Children.Add(down);
            Canvas.SetLeft(down, 925); Canvas.SetTop(down, 555);
            down.PreviewMouseDown += ScrollDown_PreviewMouseDown;
            //НАДО ДОБАВИТЬ СЮДА ОКНО С КУПЛЕННЫМИ РЕСУРСАМИ
            Border CapacityBorder = new Border(); CapacityBorder.Width = 300; CapacityBorder.Height = 500;
            CapacityBorder.BorderBrush = Links.Brushes.SkyBlue;
            CapacityBorder.BorderThickness = new Thickness(2);
            canvas.Children.Add(CapacityBorder);
            Canvas.SetLeft(CapacityBorder, 970); Canvas.SetTop(CapacityBorder, 90);
            WareHouse = new StackPanel(); WareHouse.Orientation = Orientation.Vertical;
            WareHouse.Width = 290; WareHouse.Height = 480;
            canvas.Children.Add(WareHouse); Canvas.SetLeft(WareHouse, 975); Canvas.SetTop(WareHouse, 100);
             
            TextBlock CapTitle = Common.GetBlock(30, "Товары на складе рынка", Brushes.White, 300);
            WareHouse.Children.Add(CapTitle);
            //canvas.Children.Add(CapTitle); Canvas.SetLeft(CapTitle, 970); Canvas.SetTop(CapTitle, 100);
            PlaceWarehouseInfo();
        }
        /// <summary> класс распологает элементы рынка в массиве 4 эл-та шириной и выдаёт позиции каждого элемента </summary>
        class ElementsSort
        {
            public List<byte[]> Array;
            public SortedList<int, int[]> Positions;
            public ElementsSort(MarketInfo info)
            {
                Array = new List<byte[]>();
                for (int i = 0; i < info.Elements.Count; i++)
                    PlaceElement(info.Elements.Values[i], (byte)info.Elements.Keys[i]);
                Positions = new SortedList<int, int[]>();
                for (int i = 0; i < Array.Count; i++)
                    for (int j = 0; j < 4; j++)
                        if (Array[i][j] == 255 || Array[i][j] == 254) continue;
                        else Positions.Add(Array[i][j], new int[] { i, j });                       
            }
            void PlaceElement(MarketElement element, byte pos)
            {
                bool largesize = false;
                if (element.ElementType == RewardType.Ship)
                    largesize = true;
                for (int j = 0; j < 2; j++)
                {
                    foreach (byte[] line in Array)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (largesize == false)
                            { if (line[i] == 255) { line[i] = pos; return; } }
                            else
                            { if (line[i] == 255 && i < 2 && line[i + 1] == 255 && line[i+2]==255) { line[i] = pos; line[i+1] = 254; line[i + 2] = 254; return; } }

                        }
                    }
                    Array.Add(new byte[] { 255, 255, 255, 255 });
                }
            }
        }
        private void ScrollTop_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Position == 0) return;
            Position--;
            DoubleAnimation anim = new DoubleAnimation();
            anim.To = Position * -225;
            anim.Duration = TimeSpan.FromSeconds(0.3);
            anim.AccelerationRatio = 0.5;
            anim.DecelerationRatio = 0.5;
            ElementCanvas.BeginAnimation(Canvas.TopProperty, anim);
        }

        private void ScrollDown_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            int MaxPosition = Sort.Array.Count - 2;
            if (MaxPosition < 0) MaxPosition = 0;
            if (Position == MaxPosition) return;
            else Position++;
            DoubleAnimation anim = new DoubleAnimation();
            anim.To = Position * -225;
            anim.Duration = TimeSpan.FromSeconds(0.3);
            anim.AccelerationRatio = 0.5;
            anim.DecelerationRatio = 0.5;
            ElementCanvas.BeginAnimation(Canvas.TopProperty, anim);
        }

        Path GetArrow(bool IsUpDirection)
        {
            Path path = new Path();
            if (IsUpDirection)
                path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M5,0 h 20 a5,5 0 0,1 5,5 v 20 a 5,5 0 0,1 -5,5 h-20 a5,5 0 0,1 -5,-5 v-20 a5,5 0 0,1 5,-5" +
                "M15,5 l3,10 h-1 v10 h-4 v-10 h-1z"));
            else
                path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M5,0 h 20 a5,5 0 0,1 5,5 v 20 a 5,5 0 0,1 -5,5 h-20 a5,5 0 0,1 -5,-5 v-20 a5,5 0 0,1 5,-5" +
                "M15,25 l3,-10 h-1 v-10 h-4 v10 h-1z"));
            path.Fill = Links.Brushes.SkyBlue;
            return path;
        }
        void PlaceWarehouseInfo()
        {
            if (Info.ResourcesBuyed[RewardType.Metall] > 0 || Info.ResourcesBuyed[RewardType.Chips] > 0 || Info.ResourcesBuyed[RewardType.Anti] > 0)
                HasResources = true;
            else if (Info.BuyedElements.Count > 0)
                HasResources = true;
            //int pos = 140;
            foreach (KeyValuePair<RewardType, int> pair in Info.ResourcesBuyed)
            {
                TextBlock block = Common.GetBlock(30, "", Brushes.White, 280);
                block.TextAlignment = TextAlignment.Left;
                Rectangle rect = null;
                switch (pair.Key)
                {
                    case RewardType.Metall: rect = Common.GetRectangle(30, Links.Brushes.MetalImageBrush); break;
                    case RewardType.Chips: rect = Common.GetRectangle(30, Links.Brushes.ChipsImageBrush); break;
                    case RewardType.Anti: rect = Common.GetRectangle(30, Links.Brushes.AntiImageBrush); break;
                }
                block.Inlines.Add(new InlineUIContainer(rect));
                block.Inlines.Add(new Run("  "+pair.Value.ToString()));
                WareHouse.Children.Add(block);
                /*Canvas.Children.Add(block);
                Canvas.SetTop(block, pos); Canvas.SetLeft(block, 980);
                pos+=40;*/  
            }
            List<GameScience> sciences = new List<GameScience>();
            List<GSPilot> pilots = new List<GSPilot>();
            List<Artefact> artefacts = new List<Artefact>();
            List<string> ships = new List<string>();
            List<GSPlanet> avanposts = new List<GSPlanet>();
            foreach (BuyedElement element in Info.BuyedElements)
            {
                if (element.Type == RewardType.Pilot) pilots.Add(new GSPilot(element.Description, 0));
                if (element.Type == RewardType.Artefact) artefacts.Add(Links.Artefacts[BitConverter.ToUInt16(element.Description, 0)]);
                if (element.Type==RewardType.Ship)
                {
                    int s = 0; Schema schema = Schema.GetSchema(element.Description, ref s);
                    ships.Add(NameCreator.GetName(ShipNameType.Standard, schema, new byte[4]));
                }
                if (element.Type == RewardType.Science) sciences.Add(Links.Science.GameSciences[BitConverter.ToUInt16(element.Description, 0)]);
                if (element.Type == RewardType.Avanpost) avanposts.Add(Links.Planets[BitConverter.ToInt32(element.Description, 0)]);
            }
            if (sciences.Count>0)
            {
                TextBlock sciencetext = Common.GetBlock(30, "Приобретённые технологии", Brushes.White, 290);
                WareHouse.Children.Add(sciencetext);
                foreach (GameScience science in sciences)
                {
                    TextBlock scienceinfo = Common.GetBlock(30, GameObjectName.GetScienceName(science), Links.Brushes.SkyBlue, 290);
                    WareHouse.Children.Add(scienceinfo);
                }
            }
            if (pilots.Count > 0)
            {
                TextBlock pilottext = Common.GetBlock(30, "Нанятые пилоты", Brushes.White, 290);
                WareHouse.Children.Add(pilottext);
                //Canvas.Children.Add(pilottext); Canvas.SetLeft(pilottext, 970); Canvas.SetTop(pilottext, pos); pos += 40;
                foreach (GSPilot pilot in pilots)
                {
                    TextBlock pilinfo = Common.GetBlock(30, "", Brushes.White, 290); pilinfo.TextAlignment = TextAlignment.Left;
                    Run pilname = new Run(String.Format("{0} \"{1}\" {2} ", pilot.GetName(), pilot.GetNick(), pilot.GetSurname()));
                    switch (pilot.Specialization)
                    {
                        case 0: pilname.Foreground = Brushes.Blue; break;
                        case 1: pilname.Foreground = Brushes.Red; break;
                        case 2: pilname.Foreground = Brushes.Purple; break;
                        case 3: pilname.Foreground = Brushes.Green; break;
                        default: pilname.Foreground = Brushes.Gold; break;
                    }
                    pilinfo.Inlines.Add(pilname);
                    WareHouse.Children.Add(pilinfo);
                    //Canvas.Children.Add(pilinfo); Canvas.SetLeft(pilinfo, 980); Canvas.SetTop(pilinfo, pos); pos += 40;
                }
            }
            if (artefacts.Count>0)
            {
                TextBlock arttext = Common.GetBlock(30, "Приобретённые артефакты", Brushes.White, 290);
                WareHouse.Children.Add(arttext);
                foreach (Artefact art in artefacts)
                {
                    TextBlock artinfo = Common.GetBlock(30, art.GetName(), Links.Brushes.SkyBlue, 290); 
                    WareHouse.Children.Add(artinfo);
                }
            }
            if (ships.Count>0)
            {
                TextBlock shiptext = Common.GetBlock(30, "Приобретённые корабли", Brushes.White, 290);
                WareHouse.Children.Add(shiptext);
                foreach (string s in ships)
                {
                    TextBlock shipinfo = Common.GetBlock(30, s, Links.Brushes.SkyBlue, 290);
                    WareHouse.Children.Add(shipinfo);
                }
            }
            if (avanposts.Count>0)
            {
                TextBlock avanposttext = Common.GetBlock(30, "Приобретённые аванпосты", Brushes.White, 290);
                WareHouse.Children.Add(avanposttext);
                foreach (GSPlanet p in avanposts)
                {
                    TextBlock avanpostinfo = Common.GetBlock(30, p.Name.ToString(), Links.Brushes.SkyBlue, 290);
                    WareHouse.Children.Add(avanpostinfo);
                }
            }
        }

        private void CloseCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.NewMarket.LeaveCurrentMarket();
            /*string eventresult = Events.TryLeaveMarket((byte)Info.Type);
            if (eventresult != "")
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult));
            else
            {
                Gets.GetResources();
                Links.Controller.NewMarket.Select();
            }*/
        }
        /// <summary> окно с одним элементом рынка </summary>
        class ChangeElement:Canvas
        {
            MarketElement Element;
            MarketWindow MWindow;
            int Key;
            public ChangeElement(MarketElement element, int key, MarketWindow window)
            {
                Element = element; MWindow = window; Key=key;
                Width = 200; Height = 200;
                Canvas HelpIcon;
                UIElement Image = null; int dleft = 0;
                switch (element.ElementType)
                {
                    case RewardType.Metall: Image = GetChangeImage(RewardType.Money, RewardType.Metall); break;
                    case RewardType.Chips: Image = GetChangeImage(RewardType.Money, RewardType.Chips); break;
                    case RewardType.Anti: Image = GetChangeImage(RewardType.Money, RewardType.Anti); break;
                    case RewardType.Money:
                        if (element.PriceType == RewardType.Metall) Image = GetChangeImage(RewardType.Metall, RewardType.Money);
                        else if (element.PriceType == RewardType.Chips) Image = GetChangeImage(RewardType.Chips, RewardType.Money);
                        else if (element.PriceType == RewardType.Anti) Image = GetChangeImage(RewardType.Anti, RewardType.Money);
                        break;
                    case RewardType.Pilot:
                        GSPilot pilot = new GSPilot(element.ElementDescription, 0);
                        PilotsImage pt = new PilotsImage(pilot, PilotsListMode.Clear, null);
                        Image = pt.GetScaledSize(150);
                        HelpIcon = GetHelpIcon(this, 175, 140); HelpIcon.Tag = pilot;
                        HelpIcon.MouseEnter += PilotImageShowFull;
                        //HelpIcon.PreviewMouseDown += PilotImageShowFull;
                        break;
                    case RewardType.Artefact:
                        ushort artid = BitConverter.ToUInt16(element.ElementDescription, 0);
                        Artefact art = Links.Artefacts[artid];
                        Image = Common.GetRectangle(150, art.GetImage());
                        HelpIcon = GetHelpIcon(this, 175, 140); HelpIcon.Tag = art;
                        HelpIcon.MouseEnter += ArtefactShowFull;
                        break;
                    case RewardType.Ship:
                        int s = 0;
                        Schema schema = Schema.GetSchema(element.ElementDescription, ref s);
                        byte[] shipimage = new byte[4];
                        Array.Copy(element.ElementDescription, s, shipimage, 0, 4);
                        GSShip ship = new GSShip(0, schema, 100, BitConverter.ToInt32(shipimage, 0));
                        //ShipB shipb = new ShipB(0, schema, 100, ship.Pilot, shipimage, ShipSide.Attack, null, false, BitConverter.GetBytes(schema.GetName()), ShipBMode.Info);
                        Image = new ShipInfoPanel2(ship, true);
                        break;
                    case RewardType.Science:
                        ushort scienceid = BitConverter.ToUInt16(element.ElementDescription, 0);
                        GameScience science = Links.Science.GameSciences[scienceid];
                        GameObjectImage scienceimage = new GameObjectImage(GOImageType.Standard, scienceid);
                        scienceimage.Height = 170; scienceimage.Width = 138;
                        Image = scienceimage; dleft = 6;
                        HelpIcon = GetHelpIcon(this, 175, 140); HelpIcon.Tag = science;
                        HelpIcon.MouseEnter += ScienceImageShowFull;
                        //Image.PreviewMouseDown += ScienceImageShowFull;
                        break;
                    case RewardType.Avanpost:
                        int planetid = BitConverter.ToInt32(element.ElementDescription, 0);
                        GSPlanet planet = Links.Planets[planetid];
                        PlanetPanelInfo panel = new PlanetPanelInfo(planet, true);
                        Image = panel; dleft = -25;
                        HelpIcon = GetHelpIcon(this, 170, 120); HelpIcon.Tag = planet;
                        HelpIcon.MouseEnter += ShowPlanet;
                        break;

                }
                Children.Add(Image); Canvas.SetLeft(Image, 25+dleft);
                
               if (element.Quantity == 0)
                {
                    Border SoldBorder = new Border();
                    SoldBorder.BorderBrush = Brushes.Red; SoldBorder.BorderThickness = new Thickness(3);
                    TextBlock SoldText = null;
                    if (element.ElementType == RewardType.Ship)
                    {
                        SoldBorder.Width = 400;
                        SoldText = Common.GetBlock(40, "Продано", Brushes.Red, 400);
                        SoldBorder.RenderTransform = new RotateTransform(10 + (key * 5 + (int)(element.PriceForOne % 7)) % 20);
                        Canvas.SetLeft(SoldBorder, 100);
                    }
                    else
                    {
                        SoldBorder.Width = 200;
                        SoldText = Common.GetBlock(40, "Продано", Brushes.Red, 200);
                        SoldBorder.RenderTransform = new RotateTransform(30 + (key * 5 + (int)(element.PriceForOne % 7)) % 20);
                    }
                    SoldBorder.Height = 50; SoldBorder.RenderTransformOrigin = new Point(0.5, 0.5);
                    Children.Add(SoldBorder); AbsorpCanvas.SetTop(SoldBorder, 80);

                    SoldBorder.Background = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0));
                    SoldBorder.Child = SoldText;
                }
                else
                {
                    if (element.Quantity>1)
                    {
                        TextBlock quantity = Common.GetBlock(20, string.Format("x{0:### ### ### ###}", element.Quantity), Brushes.White, 200);
                        Children.Add(quantity); Canvas.SetTop(quantity, 145);
                    }
                    Path path = new Path(); path.Stroke = Links.Brushes.SkyBlue; path.StrokeThickness = 1;
                    Children.Add(path);

                    TextBlock price = Common.GetBlock(20, "", Brushes.White, 160);
                    List<Inline> inlines = new List<Inline>();
                    inlines.Add(new Run(element.PriceForOne.ToString("### ### ### ##0.# ")));
                    inlines.Add(new InlineUIContainer(Common.GetRectangle(18, Links.Brushes.MoneyImageBrush)));
                    if (element.PriceType != RewardType.Money)
                    {
                        inlines.Add(new Run(" за "));
                        switch (element.PriceType)
                        {
                            case RewardType.Metall: inlines.Add(new InlineUIContainer(Common.GetRectangle(18, Links.Brushes.MetalImageBrush))); break;
                            case RewardType.Chips: inlines.Add(new InlineUIContainer(Common.GetRectangle(18, Links.Brushes.ChipsImageBrush))); break;
                            case RewardType.Anti: inlines.Add(new InlineUIContainer(Common.GetRectangle(18, Links.Brushes.AntiImageBrush))); break;
                        }
                    }
                    price.Inlines.AddRange(inlines.ToArray());
                    Children.Add(price);
                    Canvas.SetTop(price, 172);
                    if (element.ElementType == RewardType.Ship)
                    {
                        Canvas.SetLeft(price, 420);
                        path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M420,170 h180 l-20,30 h-180z"));
                    }
                    else
                    {
                        Canvas.SetLeft(price, 20);
                        path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M20,170 h180 l-20,30 h-180z"));
                    }
                    PreviewMouseDown += ChangeElement_PreviewMouseDown;
                }
            }

            private void ArtefactShowFull(object sender, MouseEventArgs e)
            {
                Canvas canvas = (Canvas)sender;
                Artefact art = (Artefact)canvas.Tag;
                Border info = IntBoya.GetArtefactInfo(art);
                Links.Controller.PopUpCanvas.Place(info, true);
            }

            Canvas GetHelpIcon(Canvas canvas, int left, int top)
            {
                Canvas c = new Canvas(); c.Width = 25; c.Height = 25;
                canvas.Children.Add(c); Canvas.SetLeft(c, left); Canvas.SetTop(c, top); Canvas.SetZIndex(c, 5);
                Path path = new Path();
                path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M12.5,0 a12.5,12.5 0 1,1 -0.1,0z M8.5,7 a4,4 0 1,1 6,3 a6,8 0 0,0 -2.5,7.5 M12,20 a0.75,0.75 0 1,1 -0.1,0z"));
                path.StrokeThickness = 2;
                LinearGradientBrush brush = new LinearGradientBrush();
                GradientStop gr1, gr2, gr3;
                brush.GradientStops.Add(gr1 = new GradientStop(Colors.Gold, 0));
                brush.GradientStops.Add(gr2 = new GradientStop(Colors.White, 0.5));
                brush.GradientStops.Add(gr3 = new GradientStop(Colors.Gold, 1));
                path.Stroke = brush; path.Fill = Links.Brushes.Transparent;
                DoubleAnimation anim1 = new DoubleAnimation(-4, 3, TimeSpan.FromSeconds(3));
                anim1.RepeatBehavior = RepeatBehavior.Forever;
                gr1.BeginAnimation(GradientStop.OffsetProperty, anim1);

                DoubleAnimation anim2 = new DoubleAnimation(-3.5, 3.5, TimeSpan.FromSeconds(3));
                anim2.RepeatBehavior = RepeatBehavior.Forever;
                gr2.BeginAnimation(GradientStop.OffsetProperty, anim2);

                DoubleAnimation anim3 = new DoubleAnimation(-3, 4, TimeSpan.FromSeconds(3));
                anim3.RepeatBehavior = RepeatBehavior.Forever;
                gr3.BeginAnimation(GradientStop.OffsetProperty, anim3);
                c.Children.Add(path);
                Ellipse el = new Ellipse(); el.Width = 25; el.Height = 25;
                c.Children.Add(el);
                GradientStop gr4, gr5, gr6;
                LinearGradientBrush brush1 = new LinearGradientBrush();
                brush1.GradientStops.Add(gr4 = new GradientStop(Colors.Transparent, 0));
                brush1.GradientStops.Add(gr5 = new GradientStop(Colors.Gold, 0.5));
                brush1.GradientStops.Add(gr6 = new GradientStop(Colors.Transparent, 1));
                gr4.BeginAnimation(GradientStop.OffsetProperty, anim1);
                gr5.BeginAnimation(GradientStop.OffsetProperty, anim2);
                gr6.BeginAnimation(GradientStop.OffsetProperty, anim3);
                el.Fill = brush1;
                return c;
            }
            private void PilotImageShowFull(object sender, MouseEventArgs e)
            {
                Canvas path = (Canvas)sender;
                GSPilot pilot = (GSPilot)path.Tag;
                PilotLargeInfo info = new PilotLargeInfo(pilot);
                Links.Controller.PopUpCanvas.Place(info, true);
            }

            private void ShowPlanet(object sender, MouseEventArgs e)
            {
                GSPlanet planet = (GSPlanet)((Canvas)sender).Tag;
                Canvas canvas = new Canvas(); canvas.Width = Links.GalaxyWidth; canvas.Height = Links.GalaxyHeight;
                canvas.Background = Links.Brushes.SpaceBack;
                Links.Controller.PopUpCanvas.Place(canvas, true);
                foreach (Land land in GSGameInfo.PlayerLands.Values)
                {
                    Ellipse el = new Ellipse(); el.Width = 20; el.Height = 20; el.Fill = Brushes.White;
                    canvas.Children.Add(el);
                    Canvas.SetLeft(el, land.Planet.Star.X + Links.GalaxyWidth / 2 - 10);
                    Canvas.SetTop(el, land.Planet.Star.Y + Links.GalaxyHeight / 2 - 10);
                }
                Ellipse el1 = new Ellipse(); el1.Width = 20; el1.Height = 20; el1.Fill = Brushes.Red;
                canvas.Children.Add(el1);
                Canvas.SetLeft(el1,planet.Star.X + Links.GalaxyWidth / 2 - 10);
                Canvas.SetTop(el1, planet.Star.Y + Links.GalaxyHeight / 2 - 10);
            }

            private void ScienceImageShowFull(object sender, MouseEventArgs e)
            {
                Canvas image = (Canvas)sender;
                GameScience science = (GameScience)image.Tag;
                GameObjectImage largeimage = new GameObjectImage(GOImageType.Standard, science.ID);
                largeimage.Height = 500; largeimage.Width = 400;
                Links.Controller.PopUpCanvas.Place(largeimage, true);
            }

            private void ChangeElement_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            {
                if (e.LeftButton != MouseButtonState.Pressed) return;
                if (Element.ElementType == RewardType.Metall || Element.ElementType == RewardType.Chips || Element.ElementType == RewardType.Anti
                    || Element.ElementType == RewardType.Money)
                {
                    ChangeWindow window = new ChangeWindow(Element, Key, MWindow);
                    Links.Controller.PopUpCanvas.Place(window);
                }
                else if (GSGameInfo.Money<Element.PriceForOne)
                {
                    Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage("Недостаточно средств"));
                }
                else if (Element.ElementType==RewardType.Pilot)
                {
                    GSPilot pilot = new GSPilot(Element.ElementDescription, 0);
                    List<Inline> list = new List<Inline>();
                    list.Add(new Run("Нанять пилота "));
                    Run name = new Run(String.Format("{0} \"{1}\" {2} ", pilot.GetName(), pilot.GetNick(), pilot.GetSurname()));
                    switch (pilot.Specialization)
                    {
                        case 0: name.Foreground = Brushes.Blue; break;
                        case 1: name.Foreground = Brushes.Red; break;
                        case 2: name.Foreground = Brushes.Purple; break;
                        case 3: name.Foreground = Brushes.Green; break;
                        case 4: name.Foreground = Brushes.Gold; break;
                    }
                    list.Add(name);
                    list.Add(new Run("за "));
                    Run price = new Run(Element.PriceForOne.ToString("### ### ###")+" ");
                    price.Foreground = Links.Brushes.SkyBlue;
                    list.Add(price);
                    list.Add(new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.MoneyImageBrush)));
                    SimpleConfirmMessage window = new SimpleConfirmMessage(list);
                    window.Ok.PreviewMouseDown += Ok_PreviewMouseDown;
                    Links.Controller.PopUpCanvas.Place(window);
                }
                else if (Element.ElementType==RewardType.Artefact)
                {
                    ushort artid = BitConverter.ToUInt16(Element.ElementDescription, 0);
                    Artefact art = Links.Artefacts[artid];
                    List<Inline> list = new List<Inline>();
                    list.Add(new Run("Приобрести артефакт "));
                    Run name = new Run(art.GetName()); name.Foreground = Links.Brushes.SkyBlue;
                    list.Add(name);
                    list.Add(new Run(" за "));
                    Run price = new Run(Element.PriceForOne.ToString("### ### ###") + " ");
                    price.Foreground = Links.Brushes.SkyBlue;
                    list.Add(price);
                    list.Add(new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.MoneyImageBrush)));
                    SimpleConfirmMessage window = new SimpleConfirmMessage(list);
                    window.Ok.PreviewMouseDown += Ok_PreviewMouseDown;
                    Links.Controller.PopUpCanvas.Place(window);
                }
                else if (Element.ElementType==RewardType.Ship)
                {
                    int s = 0;
                    Schema schema = Schema.GetSchema(Element.ElementDescription, ref s);
                    List<Inline> list = new List<Inline>();
                    list.Add(new Run("Приобрести корабль "));
                    Run name = new Run(NameCreator.GetName(ShipNameType.Standard, schema, BitConverter.GetBytes(schema.GetName()))); name.Foreground = Links.Brushes.SkyBlue;
                    list.Add(name);
                    list.Add(new Run(" за "));
                    Run price = new Run(Element.PriceForOne.ToString("### ### ###") + " ");
                    price.Foreground = Links.Brushes.SkyBlue;
                    list.Add(price);
                    list.Add(new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.MoneyImageBrush)));
                    SimpleConfirmMessage window = new SimpleConfirmMessage(list);
                    window.Ok.PreviewMouseDown += Ok_PreviewMouseDown;
                    Links.Controller.PopUpCanvas.Place(window);
                }
                else if (Element.ElementType==RewardType.Science)
                {
                    ushort scienceid = BitConverter.ToUInt16(Element.ElementDescription, 0);
                    GameScience science = Links.Science.GameSciences[scienceid];
                    List<Inline> list = new List<Inline>();
                    list.Add(new Run("Приобрести технологию "));
                    Run name = new Run(GameObjectName.GetScienceName(science)); name.Foreground = Links.Brushes.SkyBlue;
                    list.Add(name);
                    list.Add(new Run(" за "));
                    Run price = new Run(Element.PriceForOne.ToString("### ### ###") + " ");
                    price.Foreground = Links.Brushes.SkyBlue;
                    list.Add(price);
                    list.Add(new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.MoneyImageBrush)));
                    SimpleConfirmMessage window = new SimpleConfirmMessage(list);
                    window.Ok.PreviewMouseDown += Ok_PreviewMouseDown;
                    Links.Controller.PopUpCanvas.Place(window);
                }
                else if (Element.ElementType==RewardType.Avanpost)
                {
                    int planetid = BitConverter.ToInt32(Element.ElementDescription, 0);
                    GSPlanet planet = Links.Planets[planetid];
                    List<Inline> list = new List<Inline>();
                    list.Add(new Run("Приобрести аванпост на планете "));
                    Run name = new Run(planet.Name.ToString()); name.Foreground = Links.Brushes.SkyBlue;
                    list.Add(name);
                    list.Add(new Run(" за "));
                    Run price = new Run(Element.PriceForOne.ToString("### ### ###") + " ");
                    price.Foreground = Links.Brushes.SkyBlue;
                    list.Add(price);
                    list.Add(new InlineUIContainer(Common.GetRectangle(30, Links.Brushes.MoneyImageBrush)));
                    SimpleConfirmMessage window = new SimpleConfirmMessage(list);
                    window.Ok.PreviewMouseDown += Ok_PreviewMouseDown;
                    Links.Controller.PopUpCanvas.Place(window);
                }

            }

            private void Ok_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            {
                string eventresult = Events.TryMarketChange((byte)MWindow.Info.Type, (byte)Key, (int)Element.PriceForOne);
                if (eventresult != "")
                    Links.Controller.PopUpCanvas.Change(new SimpleInfoMessage(eventresult), "Ошибка при обмене");
                else
                {
                    Links.Controller.PopUpCanvas.Remove();
                    Links.Controller.NewMarket.Refresh(MWindow.Info.Type, MWindow.Position);
                }
            }

            Canvas GetChangeImage(RewardType from, RewardType to)
            {
                Canvas canvas = new Canvas(); canvas.Width = 150; canvas.Height = 150;
                Rectangle fromrect = null;
                switch (from)
                {
                    case RewardType.Metall: fromrect = Common.GetRectangle(75, Links.Brushes.MetalImageBrush); break;
                    case RewardType.Chips: fromrect=Common.GetRectangle(75, Links.Brushes.ChipsImageBrush); break;
                    case RewardType.Anti: fromrect = Common.GetRectangle(75, Links.Brushes.AntiImageBrush); break;
                    case RewardType.Money: fromrect = Common.GetRectangle(75, Links.Brushes.MoneyImageBrush); break;
                }
                canvas.Children.Add(fromrect);
                Rectangle torect = null;
                switch (to)
                {
                    case RewardType.Metall: torect = Common.GetRectangle(75, Links.Brushes.MetalImageBrush); break;
                    case RewardType.Chips: torect = Common.GetRectangle(75, Links.Brushes.ChipsImageBrush); break;
                    case RewardType.Anti: torect = Common.GetRectangle(75, Links.Brushes.AntiImageBrush); break;
                    case RewardType.Money: torect = Common.GetRectangle(75, Links.Brushes.MoneyImageBrush); break;
                }
                canvas.Children.Add(torect); Canvas.SetLeft(torect, 75); Canvas.SetTop(torect, 75);
                Path path = new Path(); path.Stroke = Links.Brushes.SkyBlue;
                path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                    "M85, 20 h20 a20,20 0 0,1 20,20 v10 h5 l-15,20 l-15,-20 h5 v-10 a5,5 0 0,0 -5,-5 h-20z"));
                canvas.Children.Add(path); 
                return canvas;
            }
        }
    }
    /// <summary> Окно с обменом пака ресурсов </summary>
    class ChangeWindow:Border
    {
        MarketElement Element;
        MarketWindow MWindow;
        Canvas Canvas;
        TextBlock Block1; TextBlock Block2; TextBlock Quantity;
        int val = 0;
        int MaxRes1;
        int MarketKey;
        public ChangeWindow(MarketElement element, int key, MarketWindow mwindow)
        {
            Element = element; MWindow = mwindow; MarketKey = key;
            Width = 800; Height = 600; BorderThickness = new Thickness(3); BorderBrush = Links.Brushes.SkyBlue; CornerRadius = new CornerRadius(20);
            Background = Brushes.Black;
            Canvas canvas = new Canvas(); Child = canvas;
            Canvas = canvas;
            Canvas closeCanvas = Common.GetCloseCanvas(true);
            closeCanvas.PreviewMouseDown += CloseCanvas_PreviewMouseDown;
            canvas.Children.Add(closeCanvas);
            Canvas.SetLeft(closeCanvas, 750); Canvas.SetTop(closeCanvas, -10);

            Border brd1 = new Border(); brd1.BorderBrush = Links.Brushes.SkyBlue; brd1.BorderThickness = new Thickness(1);
            brd1.Width = 300; brd1.Height = 80;
            canvas.Children.Add(brd1); Canvas.SetLeft(brd1, 10); Canvas.SetTop(brd1, 10);

            Border brd2 = new Border(); brd2.BorderBrush = Links.Brushes.SkyBlue; brd2.BorderThickness = new Thickness(1);
            brd2.Width = 300; brd2.Height = 80;
            canvas.Children.Add(brd2); Canvas.SetLeft(brd2, 440); Canvas.SetTop(brd2, 10);

            Path path = new Path(); path.Stroke = Links.Brushes.SkyBlue;
            path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M320,30 h70 v-20 l40,40 l-40,40 v-20 h-70z"));
            canvas.Children.Add(path);

            UIElement Image1 = null;
            switch (element.ElementType)
            {
                case RewardType.Metall: Image1 = Common.GetRectangle(70, Links.Brushes.MetalImageBrush); break;
                case RewardType.Chips: Image1 = Common.GetRectangle(70, Links.Brushes.ChipsImageBrush); break;
                case RewardType.Anti: Image1 = Common.GetRectangle(70, Links.Brushes.AntiImageBrush);break;
                case RewardType.Money: Image1 = Common.GetRectangle(70, Links.Brushes.MoneyImageBrush); break;
            }
            canvas.Children.Add(Image1); Canvas.SetLeft(Image1, 445); Canvas.SetTop(Image1, 15);

            UIElement Image2 = null;
            switch (element.PriceType)
            {
                case RewardType.Metall: Image2 = Common.GetRectangle(70, Links.Brushes.MetalImageBrush); break;
                case RewardType.Chips: Image2 = Common.GetRectangle(70, Links.Brushes.ChipsImageBrush); break;
                case RewardType.Anti: Image2 = Common.GetRectangle(70, Links.Brushes.AntiImageBrush); break;
                case RewardType.Money: Image2 = Common.GetRectangle(70, Links.Brushes.MoneyImageBrush); break;
            }
            canvas.Children.Add(Image2); Canvas.SetLeft(Image2, 15); Canvas.SetTop(Image2, 15);
            Numpads = new SortedList<MarketsButtons, Border>();
            PlaceButton(MarketsButtons.all, 190, 140, 590, 150, "Все");
            PlaceButton(MarketsButtons.half, 190, 140, 590, 300, "Половина");
            PlaceButton(MarketsButtons.clear, 190, 130, 590, 450, "Очистить");
            PlaceButton(MarketsButtons.ok, 275, 100, 305, 480, "Обмен");
            PlaceButton(MarketsButtons.n0, 275, 100, 20, 480, "0");
            PlaceButton(MarketsButtons.n1, 180, 100, 20, 370, "1");
            PlaceButton(MarketsButtons.n2, 180, 100, 210, 370, "2");
            PlaceButton(MarketsButtons.n3, 180, 100, 400, 370, "3");
            PlaceButton(MarketsButtons.n4, 180, 100, 20, 260, "4");
            PlaceButton(MarketsButtons.n5, 180, 100, 210, 260, "5");
            PlaceButton(MarketsButtons.n6, 180, 100, 400, 260, "6");
            PlaceButton(MarketsButtons.n7, 180, 100, 20, 150, "7");
            PlaceButton(MarketsButtons.n8, 180, 100, 210, 150, "8");
            PlaceButton(MarketsButtons.n9, 180, 100, 400, 150, "9");

            Block1 = Common.GetBlock(40, "0", Brushes.White, 200); Block1.TextAlignment = TextAlignment.Left;
            Canvas.Children.Add(Block1); Canvas.SetLeft(Block1, 90); Canvas.SetTop(Block1, 25);

            Block2 = Common.GetBlock(40, "0", Brushes.White, 200); Block2.TextAlignment = TextAlignment.Left;
            Canvas.Children.Add(Block2); Canvas.SetLeft(Block2, 520); Canvas.SetTop(Block2, 25);

            Quantity = Common.GetBlock(36, string.Format("x{0:### ### ### ### ###}", element.Quantity), Brushes.White, 600);
            Canvas.Children.Add(Quantity); Canvas.SetLeft(Quantity, 80); Canvas.SetTop(Quantity, 100);

            Links.Controller.mainWindow.KeyDown += ChangeWindow_PreviewKeyDown;
            SelectMinValues();
        }
        void SelectMinValues()
        {
            if (Element.ElementType==RewardType.Money)
            {
                int reshave = 0;
                if (Element.PriceType == RewardType.Metall) reshave = GSGameInfo.Metals + MWindow.Info.ResourcesBuyed[RewardType.Metall];
                else if (Element.PriceType == RewardType.Chips) reshave = GSGameInfo.Chips + MWindow.Info.ResourcesBuyed[RewardType.Chips];
                else if (Element.PriceType == RewardType.Anti) reshave = GSGameInfo.Anti + MWindow.Info.ResourcesBuyed[RewardType.Anti];
                MaxRes1 = reshave;
                if (Element.Quantity < reshave) MaxRes1 = Element.Quantity;          
            }
            else
            {
                int rescanbuy=0;
                if (Element.ElementType == RewardType.Metall) rescanbuy = GSGameInfo.Capacity.Metal - GSGameInfo.Metals - MWindow.Info.ResourcesBuyed[RewardType.Metall];
                else if (Element.ElementType == RewardType.Chips) rescanbuy = GSGameInfo.Capacity.Chips - GSGameInfo.Chips - MWindow.Info.ResourcesBuyed[RewardType.Chips];
                else if (Element.ElementType == RewardType.Anti) rescanbuy = GSGameInfo.Capacity.Anti - GSGameInfo.Anti - MWindow.Info.ResourcesBuyed[RewardType.Anti];
                if (rescanbuy < 0) rescanbuy = 0;
                if (Element.Quantity < rescanbuy) rescanbuy = Element.Quantity;
                if (Math.Round(GSGameInfo.Money / Element.PriceForOne, 0) < rescanbuy) rescanbuy = (int)Math.Round(GSGameInfo.Money / Element.PriceForOne, 0);
                MaxRes1 = (int)Math.Round(rescanbuy * Element.PriceForOne, 0);
            }

        }
        private void CloseCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.mainWindow.KeyDown -= ChangeWindow_PreviewKeyDown;
        }

        private void ChangeWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.NumPad0: case Key.D0: ButtonPressed(MarketsButtons.n0); break;
                case Key.NumPad1: case Key.D1: ButtonPressed(MarketsButtons.n1); break;
                case Key.NumPad2: case Key.D2: ButtonPressed(MarketsButtons.n2); break;
                case Key.NumPad3: case Key.D3: ButtonPressed(MarketsButtons.n3); break;
                case Key.NumPad4: case Key.D4: ButtonPressed(MarketsButtons.n4); break;
                case Key.NumPad5: case Key.D5: ButtonPressed(MarketsButtons.n5); break;
                case Key.NumPad6: case Key.D6: ButtonPressed(MarketsButtons.n6); break;
                case Key.NumPad7: case Key.D7: ButtonPressed(MarketsButtons.n7); break;
                case Key.NumPad8: case Key.D8: ButtonPressed(MarketsButtons.n8); break;
                case Key.NumPad9: case Key.D9: ButtonPressed(MarketsButtons.n9); break;
                case Key.Back: ButtonPressed(MarketsButtons.Backspace); break;
                case Key.Enter: ButtonPressed(MarketsButtons.ok); break;
            }
        }

        SortedList<MarketsButtons, Border> Numpads;
        void ButtonPressed(MarketsButtons btn)
        {
            switch (btn)
            {
                case MarketsButtons.n1: if (val == 0) val = 1; else val = val * 10 + 1; break;
                case MarketsButtons.n2: if (val == 0) val = 2; else val = val * 10 + 2; break;
                case MarketsButtons.n3: if (val == 0) val = 3; else val = val * 10 + 3; break;
                case MarketsButtons.n4: if (val == 0) val = 4; else val = val * 10 + 4; break;
                case MarketsButtons.n5: if (val == 0) val = 5; else val = val * 10 + 5; break;
                case MarketsButtons.n6: if (val == 0) val = 6; else val = val * 10 + 6; break;
                case MarketsButtons.n7: if (val == 0) val = 7; else val = val * 10 + 7; break;
                case MarketsButtons.n8: if (val == 0) val = 8; else val = val * 10 + 8; break;
                case MarketsButtons.n9: if (val == 0) val = 9; else val = val * 10 + 9; break;
                case MarketsButtons.n0: if (val == 0) val = 0; else val = val * 10; break;
                case MarketsButtons.Backspace: if (val < 10) val = 0; else val = val / 10; break;
                case MarketsButtons.clear: val = 0; break;
                case MarketsButtons.all: val = MaxRes1; break;
                case MarketsButtons.half: val = MaxRes1 / 2; break;
                case MarketsButtons.ok: if (val > 0) { TryChangeResources(); return; } break;
            }
            if (val > MaxRes1) val = MaxRes1;
            Block1.Text = val.ToString();
            if (Element.ElementType == RewardType.Money)
                Block2.Text = ((int)Math.Round(val * Element.PriceForOne, 0)).ToString();
            else
                Block2.Text = ((int)Math.Round(val / Element.PriceForOne, 0)).ToString();
                        if (Numpads.ContainsKey(btn))
            {
                Border brd = Numpads[btn];
                ScaleTransform scale = (ScaleTransform)brd.RenderTransform;
                DoubleAnimation anim = new DoubleAnimation(1, 0.9, TimeSpan.FromSeconds(0.1));
                anim.AutoReverse = true;
                scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
                scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
            }
        }
        void TryChangeResources()
        {
            Links.Controller.mainWindow.KeyDown -= ChangeWindow_PreviewKeyDown;
            string eventresult = Events.TryMarketChange((byte)MWindow.Info.Type, (byte)MarketKey, val);
            if (eventresult!="")
                Links.Controller.PopUpCanvas.Change(new SimpleInfoMessage(eventresult), "Ошибка при обмене");
            else
            {
                Links.Controller.PopUpCanvas.Remove();
                Links.Controller.NewMarket.Refresh(MWindow.Info.Type, MWindow.Position);
            }
        }
        enum MarketsButtons { n0,n1,n2,n3,n4,n5,n6,n7, n8,n9, all, half, clear, ok, Backspace}
        void PlaceButton(MarketsButtons btn, int width, int height, int left, int top, string text)
        {
            Border brd = new Border(); brd.Width = width; brd.Height = height;
            brd.BorderBrush = Links.Brushes.SkyBlue; brd.BorderThickness = new Thickness(2); brd.CornerRadius = new CornerRadius(10);
            brd.Background = Brushes.Black;
            Canvas.Children.Add(brd);
            Canvas.SetLeft(brd, left); Canvas.SetTop(brd, top);
            brd.Tag = btn;
            TextBlock tb = Common.GetBlock(50, text, Brushes.White, width - 10);
            brd.Child = tb;
            brd.RenderTransformOrigin = new Point(0.5, 0.5);
            brd.RenderTransform = new ScaleTransform(1, 1);
            brd.MouseEnter += NumBrd_MouseEnter;
            brd.MouseLeave += NumBrd_MouseLeave;
            brd.PreviewMouseDown += NumBrd_PreviewMouseDown;
            Numpads.Add(btn, brd);
        }

        private void NumBrd_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Border brd = (Border)sender;
            MarketsButtons btn = (MarketsButtons)brd.Tag;
            ButtonPressed(btn);
        }

        private void NumBrd_MouseLeave(object sender, MouseEventArgs e)
        {
            Border brd = (Border)sender;
            brd.Background = Brushes.Black;
        }

        private void NumBrd_MouseEnter(object sender, MouseEventArgs e)
        {
            Border brd = (Border)sender;
            brd.Background = Brushes.Navy;
        }
    }
    class MarketEnterButton:Border
    {
        Path Path;
        Canvas Canvas;
        TextBlock Title;
        TextBlock CloseText;
        MarketType Type;
        TextBlock EndTime;
        public MarketEnterButton(MarketType type)
        {
            Type = type;
            Width = 600; Height = 580;
            Canvas = new Canvas(); Child = Canvas;
            Path = new Path(); Path.StrokeThickness = 3;
            Path.Stroke = Links.Brushes.SkyBlue;
            Path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M0,20 a20,20 0 0,1 20,-20 h560 a20,20 0 0,1 20,20 v300 a20,20 0 0,1 -20,20 h-200 a20,20 0 0,0 -20,20 v200 a20,20 0 0,1 -20,20 h-320 a20,20 0 0,1 -20,-20z"));
            Canvas.Children.Add(Path);
            MouseEnter += MarketEnterButton_MouseEnter;
            MouseLeave += MarketEnterButton_MouseLeave;
            PreviewMouseDown += MarketEnterButton_PreviewMouseDown;
            Path.Fill = Brushes.Black;
            SetText(type);
            SetImage(type);
            EndTime = Common.GetBlock(32, "", Brushes.White, 300);
            Canvas.Children.Add(EndTime);
            Canvas.SetLeft(EndTime, 20); Canvas.SetTop(EndTime, 500);
            //SetTime(time);
        }

        private void MarketEnterButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Type == MarketType.Local)
                Links.Controller.NewMarket.SelectLocal(0);
            else if (Type == MarketType.Galaxy)
                Links.Controller.NewMarket.SelectGalaxy(0);
        }
        public void SetTime(int time)
        {
            EndTime.Inlines.Clear();
            EndTime.Inlines.Add(new Run("До закрытия рынка осталось: "));
            Run days = new Run(time.ToString()); days.Foreground = Links.Brushes.SkyBlue;
            EndTime.Inlines.Add(days);
            Run end = new Run();
            if (time == 1) end.Text = " день";
            else if (time > 4) end.Text = " дней";
            else end.Text = " дня";
            EndTime.Inlines.Add(end);
            
        }
        void SetText(MarketType type)
        {
            Title = Common.GetBlock(50, "", Brushes.White, 500);
            Canvas.Children.Add(Title);
            Canvas.SetLeft(Title, 50); Canvas.SetTop(Title, 30);

            CloseText = Common.GetBlock(30, "", Brushes.White, 300);
            Canvas.Children.Add(CloseText);
            Canvas.SetLeft(CloseText, 10); Canvas.SetTop(CloseText, 500);

            switch (type)
            {
                case MarketType.Local:
                    Title.Text = "Местный рынок";
                    break;
                case MarketType.Galaxy:
                    Title.Text = "Галактический рынок";
                    break;
            }
            
        }
        void SetImage(MarketType type)
        {
            Rectangle Image=null;
            switch (type)
            {
                case MarketType.Local:
                    Image = Common.GetRectangle(250, Gets.AddPicI("Market_Local")); break;
                case MarketType.Galaxy:
                    Image = Common.GetRectangle(250, Gets.AddPicI("Market_Galaxy")); break;
                case MarketType.Black:
                    Image = Common.GetRectangle(250, Gets.AddPicI("Market_Black")); break;
            }
            Canvas.Children.Add(Image); Canvas.SetLeft(Image, 180); Canvas.SetTop(Image, 90);
        }
        public void PutCloseText(int close)
        {
            CloseText.Inlines.Add(new Run("До закрытия осталось:"));

        }
        private void MarketEnterButton_MouseLeave(object sender, MouseEventArgs e)
        {
            Path.StrokeThickness = 3;
        }

        private void MarketEnterButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Path.StrokeThickness = 10;
        }
    }
    class MarketInfo
    {
        public MarketType Type;
        public SortedList<int, MarketElement> Elements;
        public DateTime EndTime;//Время закрытия рынка. Локальный по окончании обновляется, Галактический - распускается
        public SortedList<RewardType, int> ResourcesBuyed;
        public List<BuyedElement> BuyedElements;
        public MarketInfo(byte[] info, MarketType type, ref int i)
        {
            Type = type; i++;
            EndTime = new DateTime(BitConverter.ToInt64(info, i)); i += 8;
            Elements = new SortedList<int, MarketElement>();
            byte count = info[i]; i++;
            for (int j = 0; j < count; j++)
            {
                byte pos = info[i]; i++;
                Elements.Add(pos, new MarketElement(info, ref i));
            }
            ResourcesBuyed = new SortedList<RewardType, int>();
            ResourcesBuyed.Add(RewardType.Metall, BitConverter.ToInt32(info, i)); i += 4;
            ResourcesBuyed.Add(RewardType.Chips, BitConverter.ToInt32(info, i)); i += 4;
            ResourcesBuyed.Add(RewardType.Anti, BitConverter.ToInt32(info, i)); i += 4;
            BuyedElements = new List<BuyedElement>();
            byte elementscount = info[i]; i++;
            for (int j = 0; j < elementscount; j++)
            {
                RewardType etype = (RewardType)info[i]; i++;
                int descrlength = BitConverter.ToInt32(info, i); i += 4;
                byte[] descr = new byte[descrlength];
                Array.Copy(info, i, descr, 0, descrlength); i += descrlength;
                BuyedElements.Add(new BuyedElement(etype, descr));
            }
        }
    }

    class MarketChangeButton:Border
    {
        Path Path;
        Canvas Canvas;
        TextBlock Title;
        public MarketChangeButton()
        {
            Width = 240; Height = 240;
            Canvas = new Canvas(); Child = Canvas;
            Path = new Path(); Path.StrokeThickness = 3;
            Path.Stroke = Links.Brushes.SkyBlue;
            Path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom(
                "M5,25 a20,20 0 0,1 20,-20 h190 a20,20 0 0,1 20,20 v190 a20,20 0 0,1 -20,20 h-190 a20,20 0 0,1 -20,-20z"));
            Canvas.Children.Add(Path);
            MouseEnter += MarketEnterButton_MouseEnter;
            MouseLeave += MarketEnterButton_MouseLeave;
            Path.Fill = Brushes.Black;

        }
        public void PutText(List<Inline> list)
        {
            if (Title == null)
            {
                Title = Common.GetBlock(30, "", Brushes.White, 230); Title.TextAlignment = TextAlignment.Center;
                Canvas.Children.Add(Title); Canvas.SetLeft(Title, 5); Canvas.SetTop(Title, 55);
            }
            else Title.Inlines.Clear();
            foreach (Inline inl in list)
                Title.Inlines.Add(inl);
        }
        private void MarketEnterButton_MouseLeave(object sender, MouseEventArgs e)
        {
            Path.StrokeThickness = 3;
        }

        private void MarketEnterButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Path.StrokeThickness = 10;
        }
    }
}
