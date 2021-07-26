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
    class Nation:Canvas
    {
        GlobalInfoBorder GlobalBorder;
        public Viewbox box;
        public Nation()
        {
            Width = 1230; Height = 600;
            //Rectangle Back = new Rectangle();
            //Back.Width = 788; Back.Height = 548;
            //Back.Fill = Links.Brushes.Interface.NationPanelBack;
            //Children.Add(Back);
            //Canvas.SetLeft(Back, 221);
            //Canvas.SetTop(Back, 52);
            box = new Viewbox(); box.Width = 1920; box.Height = 930;
            box.Margin = new Thickness(0, 150, 0, 0);
            box.Child = this;
            //Border NoticeBorder = Common.PutBorder(400, 600, Brushes.White, 3, 420, 0, this);// CreateBorder(0, this, 1, 0);
            //NoticeBorder.BorderBrush = Brushes.White;
            //NoticeBorder.BorderThickness = new Thickness(3);
            /*
            ScrollViewer SelfNoticeViewer = new ScrollViewer();
            SelfNoticeViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            SelfNoticeViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            SelfNotice = new StackPanel();
            SelfNotice.Orientation = Orientation.Vertical;
            SelfNoticeViewer.Content = SelfNotice;
            NoticeBorder.Child = SelfNoticeViewer;
            */
            Player = new SideInfoBorder(EnemySide.Player);
            Children.Add(Player); Canvas.SetLeft(Player, 120); Canvas.SetTop(Player, -20);;

            GreenTeam = new SideInfoBorder(EnemySide.GreenTeam); 
            Children.Add(GreenTeam); Canvas.SetLeft(GreenTeam, 620); Canvas.SetTop(GreenTeam, -20);

            Techno = new SideInfoBorder(EnemySide.Techno);
            Children.Add(Techno); Canvas.SetLeft(Techno, 120); Canvas.SetTop(Techno, 260);

            Alien = new SideInfoBorder(EnemySide.Alien);
            Children.Add(Alien); Canvas.SetLeft(Alien, 620); Canvas.SetTop(Alien, 260);

            GlobalCanvasBtn Market = new GlobalCanvasBtn();
            Market.Rotate(); 
            Market.SetText(Links.Interface("Market"));
            Market.PutToCanvas(this, 890, 100);
            Market.PreviewMouseDown += Market_PreviewMouseDown;

            /*GlobalCanvasBtn Clan = new GlobalCanvasBtn();
            Clan.Rotate();
            Clan.SetText(Links.Interface("Clan"));
            Clan.PutToCanvas(this, 890, 300);
            Clan.PreviewMouseDown += Clan_PreviewMouseDown;*/

            GlobalCanvasBtn Ships = new GlobalCanvasBtn();
            Ships.SetText(Links.Interface("Ships")); 
            Ships.PutToCanvas(this, -70, 50);
            Ships.PreviewMouseDown += Ships_PreviewMouseDown;

            GlobalCanvasBtn Schemas = new GlobalCanvasBtn();
            Schemas.SetText(Links.Interface("Schemas"));
            Schemas.PutToCanvas(this, -70, 200);
            Schemas.PreviewMouseDown += Schemas_PreviewMouseDown;

            GlobalCanvasBtn Artefacts = new GlobalCanvasBtn();
            Artefacts.SetText("Артефакты");
            Artefacts.PutToCanvas(this, -70, 350);
            Artefacts.PreviewMouseDown += Artefacts_PreviewMouseDown;
        }

        private void Artefacts_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.SelectPanel(GamePanels.Artefacts, SelectModifiers.None);
        }

        private void Ships_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.SelectPanel(GamePanels.ShipsCanvas, SelectModifiers.None);
        }

        private void Schemas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.SelectPanel(GamePanels.SchemasCanvas, SelectModifiers.None);
        }

       /* private void Clan_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.SelectPanel(GamePanels.ClansCanvas, SelectModifiers.None);
        }*/

        private void Market_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.SelectPanel(GamePanels.Market, SelectModifiers.None);
        }
        SideInfoBorder Player, GreenTeam, Techno, Alien;
        public void Select()
        {
            Gets.GetTotalInfo("После открытия панели нации");
            Gets.GetStatisticInfo();
            int p = Player.FillInfo(0);
            p = GreenTeam.FillInfo(p);
            p = Techno.FillInfo(p);
            Alien.FillInfo(p);
            //if (GlobalBorder != null)
            //    Children.Remove(GlobalBorder);
            //GlobalBorder = new GlobalInfoBorder();
            //Children.Add(GlobalBorder);    
        }
    }
    class SideInfoBorder:Border
    {
        Canvas mainCanvas;
        TextBlock Info1, Info2;
        public SideInfoBorder(EnemySide side)
        {
            mainCanvas = new Canvas();
            mainCanvas.Width = 500;
            mainCanvas.Height = 300;
            Child = mainCanvas;
            mainCanvas.Background= Links.Brushes.Interface.NationPanelBack;

            Rectangle Image = new Rectangle(); Image.Width = 100; Image.Height = 100;
            mainCanvas.Children.Add(Image); Canvas.SetLeft(Image, 100); Canvas.SetTop(Image, 20);
            switch (side)
            {
                case EnemySide.GreenTeam: Image.Fill = Links.Brushes.GreenTeamBrush; break;
                case EnemySide.Techno: Image.Fill = Links.Brushes.TechTeamBrush; break;
                case EnemySide.Alien: Image.Fill = Links.Brushes.AlienBrush; break;
            }
            /* Rectangle LandIconRect = Common.GetRectangle(50, Links.Brushes.LandIconBrush);
             mainCanvas.Children.Add(LandIconRect); Canvas.SetLeft(LandIconRect, 150); Canvas.SetTop(LandIconRect, 20);

             Rectangle MoneyRect = Common.GetRectangle(50, Links.Brushes.MoneyImageBrush);
             mainCanvas.Children.Add(MoneyRect); Canvas.SetLeft(MoneyRect, 150); Canvas.SetTop(MoneyRect, 200);
             */
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, 50, 218, 255));
            Info1 = Common.GetBlock(16, "", brush, 200);
            mainCanvas.Children.Add(Info1); Canvas.SetLeft(Info1, 180); Canvas.SetTop(Info1, 30);
            Info1.Inlines.Clear();
            Info1.Inlines.AddRange(GetInlines("Колоний: ", "2"));
            Info1.Inlines.AddRange(GetInlines("Население: ", "200/2000"));
            Info1.Inlines.AddRange(GetInlines("Постройки: ", "1500"));
            Info1.Inlines.AddRange(GetInlines("Аванпосты: ", "4", ", Готовность: ", "25%"));
            Info1.Inlines.AddRange(GetInlines("Технологии: ", "200"));
            Info2 = Common.GetBlock(16, "", brush, 250);
            mainCanvas.Children.Add(Info2); Canvas.SetLeft(Info2, 120); Canvas.SetTop(Info2, 125);
            Info2.Inlines.Clear();
            Info2.Inlines.AddRange(GetInlines("Максимальный уровень: ", "35"));
            Info2.Inlines.AddRange(GetInlines("Флотов: ", "15", ", Кораблей: ", "456"));
            Info2.Inlines.AddRange(GetInlines("Средний уровень кораблей: ", "21,5"));
            Info2.Inlines.AddRange(GetInlines("Капитал: ", ">200М", ", Металлы: ", ">10К"));
            Info2.Inlines.AddRange(GetInlines("Микросхемы: ", ">300К", ", Антиматерия: ", ">1К"));
        }
        public int FillInfo(int pos)
        {
            Info1.Inlines.Clear(); Info2.Inlines.Clear();
            if (GSGameInfo.StatisticInfo.Length==0)
            {
                Info2.Inlines.Add(new Run("Нет информации"));
                return 0;
            }
            else
            {
                byte[] arr = GSGameInfo.StatisticInfo;
                int landscount = BitConverter.ToInt32(arr, pos); pos += 4;
                double peoplescur = BitConverter.ToDouble(arr, pos); pos += 8;
                int peoplesmax = BitConverter.ToInt32(arr, pos); pos += 4;
                int buildings = BitConverter.ToInt32(arr, pos); pos += 4;
                int avanpostcount = BitConverter.ToInt32(arr, pos); pos += 4;
                double avanpostconst = BitConverter.ToDouble(arr, pos); pos += 8;
                int sciences = BitConverter.ToInt32(arr, pos); pos += 4;
                byte maxlevel = arr[pos]; pos++;
                int fleetscount = BitConverter.ToInt32(arr, pos); pos += 4;
                int shipsinfleet = BitConverter.ToInt32(arr, pos); pos += 4;
                float averagelvl = BitConverter.ToSingle(arr, pos); pos += 4;
                long money = BitConverter.ToInt64(arr, pos); pos += 8;
                int metal = BitConverter.ToInt32(arr, pos); pos += 4;
                int chips = BitConverter.ToInt32(arr, pos); pos += 4;
                int anti = BitConverter.ToInt32(arr, pos); pos += 4;
                Info1.Inlines.AddRange(GetInlines("Колоний: ", landscount.ToString()));
                Info1.Inlines.AddRange(GetInlines("Население: ", string.Format("{0:0}/{1}", peoplescur, peoplesmax)));
                Info1.Inlines.AddRange(GetInlines("Постройки: ", buildings.ToString()));
                Info1.Inlines.AddRange(GetInlines("Аванпосты: ", avanpostcount.ToString(), ", Готовность: ", String.Format("{0:0}%", avanpostconst*100)));
                Info1.Inlines.AddRange(GetInlines("Технологии: ", sciences.ToString()));
                Info2.Inlines.AddRange(GetInlines("Максимальный уровень: ", maxlevel.ToString()));
                Info2.Inlines.AddRange(GetInlines("Флотов: ", fleetscount.ToString(), ", Кораблей: ", shipsinfleet.ToString()));
                Info2.Inlines.AddRange(GetInlines("Средний уровень кораблей: ", averagelvl.ToString("0.0")));
                Info2.Inlines.AddRange(GetInlines("Капитал: ", GetResourceInfo(money), ", Металлы: ", GetResourceInfo(metal)));
                Info2.Inlines.AddRange(GetInlines("Микросхемы: ", GetResourceInfo(chips), ", Антиматерия: ", GetResourceInfo(anti)));
                return pos;
           }
        }
        string GetResourceInfo(long val)
        {
            int log = (int)Math.Log10(val);
            int first = (int)(val / Math.Pow(10, log));
            int dims = (int)(log / 3);
            string letter = ""; if (dims == 1) letter = "K"; else if (dims == 2) letter = "M"; else if (dims == 3) letter = "B";
            int last = log - dims * 3;
            return String.Format(">{0}{1}", first * Math.Pow(10, last), letter);
        }
        List<Inline> GetInlines(string text, string parameter)
        {
            List<Inline> result = new List<Inline>();
            result.Add(new Run(text));
            Run r = new Run(parameter);
            r.Foreground = Brushes.White;
            result.Add(r);
            result.Add(new LineBreak());
            return result;
        }
        List<Inline> GetInlines(string text1, string parameter1, string text2, string parameter2)
        {
            List<Inline> result = new List<Inline>();
            result.Add(new Run(text1));
            Run r = new Run(parameter1);
            r.Foreground = Brushes.White;
            result.Add(r);
            result.Add(new Run(text2));
            Run r2 = new Run(parameter2);
            r2.Foreground = Brushes.White;
            result.Add(r2);
            result.Add(new LineBreak());
            return result;
        }
    }
    class GlobalInfoBorder : Border
    {
        Canvas mainCanvas;
        public GlobalInfoBorder()
        {
            Width = 420;
            Height = 540;
            //BorderBrush = Brushes.Black;
            //BorderThickness = new Thickness(3);
            Canvas.SetLeft(this, 405);
            Canvas.SetTop(this, 75);
            mainCanvas = new Canvas();
            Child = mainCanvas;
            Border titleborder = Common.PutBorder(410, 40, null, 2, 5, 5, mainCanvas);
            Common.PutBlock(30, titleborder, Links.Interface("GenInfo"), Brushes.White);
            Common.PutRect(mainCanvas, 35, Links.Brushes.LandIconBrush, 40, 50, false);
            PutLabel(24, 90, 50, String.Format("{0}", GSGameInfo.PlayerLands.Count));
            Common.DrawLine(mainCanvas, 1, 90, 0, 420, Brushes.SkyBlue);
            int count = 0;
            double CurPeoples = 0;
            double MaxPeoples = 0;
            double AddPeoples = 0;
            int BuildingsMax = 0;
            int BuildingsCurrent = 0;

            foreach (Land land in GSGameInfo.PlayerLands.Values)
            {
                CurPeoples += land.Peoples;
                AddPeoples += land.Add.Grow;
                MaxPeoples += land.Planet.MaxPopulation;
                BuildingsMax += (int)land.Peoples;
                BuildingsCurrent += land.BuildingsCount;
                //if (land.Riot == RiotStatus.None) continue;
                //count++;
                //RiotIcon riot = new RiotIcon(land);
                //mainCanvas.Children.Add(riot);
                //Canvas.SetLeft(riot, 100 + 50 * count);
                //Canvas.SetTop(riot, 53);
            }
            Common.PutRect(mainCanvas, 35, Links.Brushes.PeopleImageBrush, 40, 95, false);
            PutLabel(24, 95, 95, String.Format("{0:0.0}M (+{1:0.0}M) / {2:0.0}M", CurPeoples, AddPeoples, MaxPeoples));
            Common.DrawLine(mainCanvas, 1, 132, 0, 420, Brushes.SkyBlue);

            Common.PutRect(mainCanvas, 40, Links.Brushes.BuildingSizeBrush, 40, 137, false);
            PutLabel(24, 95, 137, String.Format("{0} / {1}", BuildingsCurrent, BuildingsMax));
            Common.DrawLine(mainCanvas, 1, 174, 0, 420, Brushes.SkyBlue);

            int ScienceMax = 0;
            int RareSciences = 0;
            foreach (ushort scienceid in GSGameInfo.SciencesArray)
            {
                GameScience science = Links.Science.GameSciences[scienceid];
                if (science.IsRare) RareSciences++;
                if (ScienceMax < science.Level) ScienceMax = science.Level;
            }

            StackPanel SciencePanel = new StackPanel(); SciencePanel.Orientation = Orientation.Horizontal;
            SciencePanel.Children.Add(Common.GetRectangle(35, Links.Brushes.SciencePict));
            SciencePanel.Children.Add(Common.GetBlock(24, GSGameInfo.SciencesArray.Count.ToString(), Brushes.White, new Thickness(10, 0, 10, 0)));
            SciencePanel.Children.Add(Common.GetRectangle(35, Links.Brushes.RareImageBrush));
            SciencePanel.Children.Add(Common.GetBlock(24, RareSciences.ToString(), Brushes.White, new Thickness(10, 0, 10, 0)));
            SciencePanel.Children.Add(Common.GetRectangle(30, Pictogram.IncreaseBrush));
            SciencePanel.Children.Add(Common.GetBlock(24, (ScienceMax + 1).ToString(), Brushes.White, new Thickness(10, 0, 0, 0)));
            mainCanvas.Children.Add(SciencePanel);
            Canvas.SetLeft(SciencePanel, 40); Canvas.SetTop(SciencePanel, 179);
            Common.DrawLine(mainCanvas, 1, 216, 0, 420, Brushes.SkyBlue);

            int Battlefleets = 0;
            int Defendendfleets = 0;
            int Freefleets = 0;
            int FreeShips = 0;
            foreach (GSFleet fleet in GSGameInfo.Fleets.Values)
            {
                if (fleet.Target == null) Freefleets++;
                else if (fleet.Target!=null && fleet.Target.Order == FleetOrder.InBattle) Battlefleets++;
                else if (fleet.Target!=null && fleet.Target.Mission == FleetMission.Scout) Defendendfleets++;
            }
            foreach (GSShip ship in GSGameInfo.Ships.Values)
            {
                if (ship.Fleet == null) FreeShips++;
            }
            StackPanel FleetsPanel = new StackPanel(); FleetsPanel.Orientation = Orientation.Horizontal;
            FleetsPanel.Children.Add(Common.GetRectangle(35, Links.Brushes.FleetBrush));
            FleetsPanel.Children.Add(Common.GetBlock(24, GSGameInfo.Fleets.Count.ToString(), Brushes.White, new Thickness(10, 0, 10, 0)));
            FleetsPanel.Children.Add(Common.GetRectangle(35, Links.Brushes.TwoSwords));
            FleetsPanel.Children.Add(Common.GetBlock(24, Battlefleets.ToString(), Brushes.White, new Thickness(10, 0, 10, 0)));
            FleetsPanel.Children.Add(Common.GetRectangle(35, Links.Brushes.FleetDefenseBrush));
            FleetsPanel.Children.Add(Common.GetBlock(24, Defendendfleets.ToString(), Brushes.White, new Thickness(10, 0, 10, 0)));
            FleetsPanel.Children.Add(Common.GetRectangle(35, Links.Brushes.FleetScoutBrush));
            FleetsPanel.Children.Add(Common.GetBlock(24, Freefleets.ToString(), Brushes.White, new Thickness(10, 0, 0, 0)));
            mainCanvas.Children.Add(FleetsPanel);
            Canvas.SetLeft(FleetsPanel, 40); Canvas.SetTop(FleetsPanel, 221);
            Common.DrawLine(mainCanvas, 1, 258, 0, 420, Brushes.SkyBlue);

            StackPanel ShipsPanel = new StackPanel(); ShipsPanel.Orientation = Orientation.Horizontal;
            ShipsPanel.Children.Add(Common.GetRectangle(35, Links.Brushes.ShipImageBrush));
            ShipsPanel.Children.Add(Common.GetBlock(24, GSGameInfo.Ships.Count.ToString(), Brushes.White, new Thickness(10, 0, 10, 0)));
            ShipsPanel.Children.Add(Common.GetRectangle(35, Links.Brushes.FleetScoutBrush));
            ShipsPanel.Children.Add(Common.GetBlock(24, FreeShips.ToString(), Brushes.White, new Thickness(10, 0, 0, 0)));
            mainCanvas.Children.Add(ShipsPanel);
            Canvas.SetLeft(ShipsPanel, 40); Canvas.SetTop(ShipsPanel, 263);
            Common.DrawLine(mainCanvas, 1, 300, 0, 420, Brushes.SkyBlue);

            GSRes add = CalcTotalAdd();
            GSRes cap = CalcTotalCap();
            GSRes time = CalcTime(add, cap);
            Common.PutRect(mainCanvas, 35, Links.Brushes.MoneyImageBrush, 40, 305, false);
            PutLabel(24, 80, 305, String.Format("+{0}", add.Money));
            Common.DrawLine(mainCanvas, 1, 342, 0, 420, Brushes.SkyBlue);

            Common.PutRect(mainCanvas, 35, Links.Brushes.MetalImageBrush, 40, 347, true);
            PutLabel(24, 80, 347, String.Format("+{0}/{1} ({2} {3})", add.Metal, cap.Metal, time.Metal, Links.Interface("Min")));
            Common.DrawLine(mainCanvas, 1, 384, 0, 420, Brushes.SkyBlue);

            Common.PutRect(mainCanvas, 35, Links.Brushes.ChipsImageBrush, 40, 389, true);
            PutLabel(24, 80, 389, String.Format("+{0}/{1} ({2} {3})", add.Chips, cap.Chips, time.Chips, Links.Interface("Min")));
            Common.DrawLine(mainCanvas, 1, 426, 0, 420, Brushes.SkyBlue);

            Common.PutRect(mainCanvas, 35, Links.Brushes.AntiImageBrush, 40, 431, true);
            PutLabel(24, 80, 431, String.Format("+{0}/{1} ({2} {3})", add.Anti, cap.Anti, time.Anti, Links.Interface("Min")));


        }
        GSRes CalcTime(GSRes add, GSRes cap)
        {
            GSRes val = new GSRes();
            if (add.Metal == 0) val.Metal = 0;
            else
                val.Metal = (int)Math.Round((double)((cap.Metal - GSGameInfo.Metals) / add.Metal));
            if (add.Chips == 0) val.Chips = 0;
            else
                val.Chips = (int)Math.Round((double)((cap.Chips - GSGameInfo.Chips) / add.Chips));
            if (add.Anti == 0) val.Anti = 0;
            else
                val.Anti = (int)Math.Round((double)((cap.Anti - GSGameInfo.Anti) / add.Anti));
            return val;
        }

        GSRes CalcTotalAdd()
        {
            GSRes val = new GSRes();
            foreach (Land land in GSGameInfo.PlayerLands.Values)
            {
                val.Money += (int)land.GetAddParameter(Building2Parameter.MoneyAdd);
                val.Metal += (int)land.GetAddParameter(Building2Parameter.MetallAdd);
                val.Chips += (int)land.GetAddParameter(Building2Parameter.ChipAdd);
                val.Anti += (int)land.GetAddParameter(Building2Parameter.AntiAdd);   
            }
            return val;
        }
        GSRes CalcTotalCap()
        {
            GSRes val = new GSRes(0, 
                ServerLinks.Parameters.PlayerMetallBaseCapacity, 
                ServerLinks.Parameters.PlayerChipsBaseCapacity,
                ServerLinks.Parameters.PlayerAntiBaseCapacity);
            foreach (Land land in GSGameInfo.PlayerLands.Values)
            {
                val.Metal += (int)land.GetAddParameter(Building2Parameter.MetalCap);
                val.Chips += (int)land.GetAddParameter(Building2Parameter.ChipCap);
                val.Anti += (int)land.GetAddParameter(Building2Parameter.AntiCap);
            }
            return val;
        }



        Label PutLabel(int size, int left, int top, string content)
        {
            Label lbl = new Label();
            lbl.FontSize = size;
            lbl.FontFamily = Links.Font;
            lbl.FontWeight = FontWeights.Bold;
            mainCanvas.Children.Add(lbl);
            Canvas.SetLeft(lbl, left);
            Canvas.SetTop(lbl, top);
            lbl.Content = content;
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            lbl.VerticalAlignment = VerticalAlignment.Center;
            lbl.Foreground = Brushes.White;
            return lbl;
        }
    }
}
