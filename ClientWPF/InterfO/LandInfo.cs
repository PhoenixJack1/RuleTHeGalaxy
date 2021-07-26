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
    class LandInfo:Viewbox
    {
        enum Panels { Peoples, Grow, Money, Metall, Chips, Anti, MetalCap, ChipsCap, AntiCap, Repair}
        enum GrowParams { Base, GreenPlanet, MedBase, BuildTotal, PlayerTotal, MedReuslt, Result}
        enum MoneyParams { TaxBase, TaxLandBonus, TaxPlayerBonus, TaxResult, ExpoBase, MoneyBonus, BuildTotal, PlayerMoney, PlayerTotal, ExpoResult, Result}
        enum ResParams {Main,  MainBonus, MainBuild, MainPlayerBonus, MainPlayerBuild, MainPlanetBonus, MainResult, SecondBase, SecondBonus, SecondBuild, SecondPlayerBonus, SecontPlayerBuild, SecondPlanetBonus, SecondResult, Result }
        Land Land;
        Canvas CurCanvas;
        TextBlock Title;
        TextBlock WasBlock;
        TextBlock WillBlock;

        Grid CurGrid;
        Canvas GridCanvas;
        ScrollViewer Viewer;
        InfoCanvasButton peop, grow, money, metall, chips, anti, metallcap, chipscap, anticap, repair;
        ArrowCanvas back, up, down;
        List<FrameworkElement> ShowList = new List<FrameworkElement>();
        System.Windows.Threading.DispatcherTimer Timer;
        public LandInfo (Land land)
        {
            Land = land;
            Width = 1402; Height = 884;
            CurCanvas = new Canvas(); CurCanvas.Width = 1402; CurCanvas.Height = 884;
            Child = CurCanvas; CurCanvas.Background = Links.Brushes.Colony.Colony5;

            Ellipse Exit = new Ellipse(); Exit.Width = 70; Exit.Height = 70; Exit.Fill = Links.Brushes.Transparent;
            CurCanvas.Children.Add(Exit); Canvas.SetLeft(Exit, 1140); Canvas.SetTop(Exit, 43);
            Exit.PreviewMouseDown += Exit_PreviewMouseDown;

            Title = Common.GetBlock(40, "Производство в колонии", Brushes.White, 700);
            CurCanvas.Children.Add(Title); Canvas.SetLeft(Title, 340); Canvas.SetTop(Title, 85);

            GridCanvas = new Canvas();
            CurCanvas.Children.Add(GridCanvas); Canvas.SetLeft(GridCanvas, 250); Canvas.SetTop(GridCanvas, 200);
            Viewer = new ScrollViewer(); GridCanvas.Children.Add(Viewer); Viewer.Width = 900; Viewer.Height = 600;
            Viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            CurGrid = new Grid(); CurGrid.Width = 900; Viewer.Content = CurGrid; CurGrid.VerticalAlignment = VerticalAlignment.Top;
          
            WasBlock = Common.GetBlock(36, "Было", Brushes.White, 225);
            CurCanvas.Children.Add(WasBlock); Canvas.SetLeft(WasBlock, 700); Canvas.SetTop(WasBlock, 150);
            WasBlock.BeginAnimation(FrameworkElement.OpacityProperty, new DoubleAnimation(1, 0, TimeSpan.Zero));
            WillBlock = Common.GetBlock(36, "Стало", Brushes.White, 225);
            CurCanvas.Children.Add(WillBlock); Canvas.SetLeft(WillBlock, 925); Canvas.SetTop(WillBlock, 150);
            WillBlock.BeginAnimation(FrameworkElement.OpacityProperty, new DoubleAnimation(1, 0, TimeSpan.Zero));

            peop = new InfoCanvasButton("Свободное население", Panels.Peoples);
            Grid.SetRow(peop, 0);
            peop.PreviewMouseDown += SelectPanel;

            grow = new InfoCanvasButton("Прирост населения", Panels.Grow);
            Grid.SetRow(grow, 1);
            grow.PreviewMouseDown += SelectPanel;

            money = new InfoCanvasButton("Получение денег", Panels.Money);
            Grid.SetRow(money, 2);
            money.PreviewMouseDown += SelectPanel;

            metall = new InfoCanvasButton("Добыча металла", Panels.Metall);
            Grid.SetRow(metall, 3);
            metall.PreviewMouseDown += SelectPanel;

            metallcap = new InfoCanvasButton("Склад металла", Panels.MetalCap);
            Grid.SetRow(metallcap, 4);
            metallcap.PreviewMouseDown += SelectPanel;

            chips = new InfoCanvasButton("Производство микросхем", Panels.Chips);
            Grid.SetRow(chips, 5);
            chips.PreviewMouseDown += SelectPanel;

            chipscap = new InfoCanvasButton("Склад микросхем", Panels.ChipsCap);
            Grid.SetRow(chipscap, 6);
            chipscap.PreviewMouseDown += SelectPanel;

            anti = new InfoCanvasButton("Выработка антиматерии", Panels.Anti);
            Grid.SetRow(anti, 7);
            anti.PreviewMouseDown += SelectPanel;

            anticap = new InfoCanvasButton("Склад антиматерии", Panels.AntiCap);
            Grid.SetRow(anticap, 8);
            anticap.PreviewMouseDown += SelectPanel;

            repair = new InfoCanvasButton("Ремонт кораблей", Panels.Repair);
            Grid.SetRow(repair, 9);
            repair.PreviewMouseDown += SelectPanel;

            back = new ArrowCanvas(ArrowDirection.Left);
            Canvas.SetLeft(back, 180); Canvas.SetTop(back, 40);
            CurCanvas.Children.Add(back); //Canvas.SetLeft(back, 475); Canvas.SetTop(back, 780);
            back.PreviewMouseDown += Back_PreviewMouseDown;
            DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.Zero);
            back.BeginAnimation(FrameworkElement.OpacityProperty, anim);

            up = new ArrowCanvas(ArrowDirection.Up);
            CurCanvas.Children.Add(up); Canvas.SetLeft(up, 1140); Canvas.SetTop(up, 200);
            up.PreviewMouseDown += Up_PreviewMouseDown;

            down = new ArrowCanvas(ArrowDirection.Down);
            CurCanvas.Children.Add(down); Canvas.SetLeft(down, 1140); Canvas.SetTop(down, 730);
            down.PreviewMouseDown += Down_PreviewMouseDown;

            Timer = new System.Windows.Threading.DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(0.1);
            Timer.Tick += Timer_Tick;

            SetBaseGrid();
           
        }
        
        public double GetCurGrow()
        {
            return CalcGrow()[GrowParams.Result];
        }
        public void Compare(Land newLand, GSBuilding2 newbuilding)
        {
            CurGrid.ColumnDefinitions.Add(new ColumnDefinition());
            CurGrid.ColumnDefinitions[1].Width = new GridLength(225);
            CurGrid.ColumnDefinitions[2].Width = new GridLength(225);
            ShowList.Add(WasBlock); ShowList.Add(WillBlock);
            Title.Text = newbuilding.GetName() + " построен"; 
            Land = newLand;
            PutTextElement(0, 2, (Land.Peoples - Land.BuildingsCount).ToString("### ##0.0"), " млн. чел");
            SortedList<GrowParams, double> growvalue = CalcGrow();
            PutTextElement(1, 2, growvalue[GrowParams.Result].ToString("### ##0.0"), " млн. чел/мес");
            double moneyadd = CalcMoney()[MoneyParams.Result];
            PutTextElement(2, 2, moneyadd.ToString("### #### ### ##0"), "/сут");
            double metaladd = CalcMetall()[ResParams.Result];
            PutTextElement(3, 2, metaladd.ToString("### ### ### ##0"), "/сут");
            double metalcap = CalcMetallCap()[ResParams.Result];
            PutTextElement(4, 2, metalcap.ToString("### ### ### ##0"), "");
            double chipsval = CalcChips()[ResParams.Result];
            PutTextElement(5, 2, chipsval.ToString("### ### ### ##0"), "/сут");
            double chipcap = CalcChipCap()[ResParams.Result];
            PutTextElement(6, 2, chipcap.ToString("### ### ### ##0"), "");
            double antival = CalcAnti()[ResParams.Result];
            PutTextElement(7, 2, antival.ToString("### ### ### ##0"), "/сут");
            double anticapval = CalcAntiCap()[ResParams.Result];
            PutTextElement(8, 2, anticapval.ToString("### ### ### ##0"), "");
            double repairval = CalcRepair()[ResParams.Result];
            PutTextElement(9, 2, repairval.ToString("### ### ### ##0"), "/сут");
            ClearElements();
            Timer.Start();
        }
        private void Down_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Viewer.ScrollToVerticalOffset(Viewer.VerticalOffset + 300);
        }
        private void Up_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Viewer.ScrollToVerticalOffset(Viewer.VerticalOffset - 300);
        }
        private void Back_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SetBaseGrid();
        }
        /// <summary> метод задаёт всем отображаемым элементам начальную прозрачность </summary>
        void ClearElements()
        {
            DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.Zero);
            foreach (FrameworkElement element in ShowList)
                element.BeginAnimation(FrameworkElement.OpacityProperty, anim);
            up.BeginAnimation(FrameworkElement.OpacityProperty, anim);
            down.BeginAnimation(FrameworkElement.OpacityProperty, anim);
            WasBlock.BeginAnimation(FrameworkElement.OpacityProperty, anim);
            WillBlock.BeginAnimation(FrameworkElement.OpacityProperty, anim);
            if (ShowList.Count > 15)
                Timer.Interval = TimeSpan.FromSeconds(0.05);
            else
                Timer.Interval = TimeSpan.FromSeconds(0.1);
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            FrameworkElement element = ShowList[0];
            ShowList.RemoveAt(0);
            DoubleAnimation anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
            element.BeginAnimation(FrameworkElement.OpacityProperty, anim);
            if (ShowList.Count == 0) Timer.Stop();
        }
        void SetRepairGrid()
        {
            PrepareGridStep2("Ремонт кораблей");
            SortedList<ResParams, double> repair = CalcRepair();
            int elements = 0;
            foreach (KeyValuePair<ResParams, double> pair in repair)
            {
                TextBlock[] blocks = new TextBlock[] { null, null };
                switch (pair.Key)
                {
                    case ResParams.Main:
                        blocks = GetLeftRightBlocks(30, "Базовый ремонт завода запасных частей", pair.Value.ToString("### ### ### ##0"), "/сут");
                        break;
                    case ResParams.MainBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус ремонта кораблей колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainPlayerBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус государства к ремонту кораблей", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainPlayerBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий государства", pair.Value.ToString(), " %");
                        break;
                    case ResParams.Result:
                        blocks = GetLeftRightBlocks(36, "Итоговый ремонт кораблей", pair.Value.ToString("### ### ### ##0"), "/сут");
                        break;
                }
                CurGrid.Children.Add(blocks[0]); CurGrid.Children.Add(blocks[1]);
                Grid.SetRow(blocks[0], elements);
                Grid.SetRow(blocks[1], elements);
                Grid.SetColumn(blocks[1], 1);
                elements++;
                ShowList.Add(blocks[0]); ShowList.Add(blocks[1]);
            }
            for (int i = 0; i < elements; i++)
                CurGrid.RowDefinitions.Add(new RowDefinition());
            ClearElements();
            Timer.Start();
        }
        SortedList<ResParams, double> CalcRepair()
        {
            SortedList<ResParams, double> result = new SortedList<ResParams, double>();
            double mainresult = 0;
            foreach (LandSector sector in Land.Locations)
            {
                if (sector.Type == SectorTypes.Repair)
                {
                    PeaceSector peace = (PeaceSector)sector;
                    result.Add(ResParams.Main, peace.Buildings[0].GetTotalValue());
                    if (Land.Mult.Repair > 0)
                        result.Add(ResParams.MainBonus, Land.Mult.Repair);
                    if (Land.Mult.Total > 0)
                        result.Add(ResParams.MainBuild, Land.Mult.Total);
                    if (GSGameInfo.ScienceMult.Repair > 0)
                        result.Add(ResParams.MainPlayerBonus, GSGameInfo.ScienceMult.Repair);
                    if (GSGameInfo.ScienceMult.Total > 0)
                        result.Add(ResParams.MainPlayerBuild, GSGameInfo.ScienceMult.Total);
                    mainresult = Math.Round(peace.Buildings[0].GetTotalValue() / 100.0 * (100 + Land.Mult.Repair + Land.Mult.Total + GSGameInfo.ScienceMult.Repair + GSGameInfo.ScienceMult.Total), 0);
                    result.Add(ResParams.MainResult, mainresult);
                }
            }
            result.Add(ResParams.Result, mainresult);
            return result;
        }
        void SetAntiCapGrid()
        {
            PrepareGridStep2("Склад антиматерии");
            SortedList<ResParams, double> anticap = CalcAntiCap();
            int elements = 0;
            foreach (KeyValuePair<ResParams, double> pair in anticap)
            {
                TextBlock[] blocks = new TextBlock[] { null, null };
                switch (pair.Key)
                {
                    case ResParams.Main:
                        blocks = GetLeftRightBlocks(30, "Базовое хранение антиматерии склада", pair.Value.ToString("### ### ### ###"), "");
                        break;
                    case ResParams.MainBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус хранения колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainPlayerBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус государства к хранению", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainPlayerBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий государства", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainPlanetBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус ледяной планеты", pair.Value.ToString("x#.#"), "");
                        break;
                    case ResParams.MainResult:
                        blocks = GetLeftRightBlocks(36, "Итоговое хранение антиматерии склада", pair.Value.ToString("### ### ### ###"), "");
                        break;
                    case ResParams.SecondBase:
                        blocks = GetLeftRightBlocks(30, "Базовое хранение антиматерии хранилища отработанного топлива", pair.Value.ToString("### ### ### ###"), "");
                        break;
                    case ResParams.SecondBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус хранения колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecondBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecondPlayerBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус государства к хранению", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecontPlayerBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий государства", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecondPlanetBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус ледяной планеты", pair.Value.ToString("x#.#"), "");
                        break;
                    case ResParams.SecondResult:
                        blocks = GetLeftRightBlocks(36, "Итоговое хранение антиматерии хранилища отработанного топлива", pair.Value.ToString("### ### ### ###"), "");
                        break;
                    case ResParams.Result:
                        blocks = GetLeftRightBlocks(36, "Итоговое хранение антиматерии", pair.Value.ToString("### ### ### ##0"), "");
                        break;
                }
                CurGrid.Children.Add(blocks[0]); CurGrid.Children.Add(blocks[1]);
                Grid.SetRow(blocks[0], elements);
                Grid.SetRow(blocks[1], elements);
                Grid.SetColumn(blocks[1], 1);
                elements++;
                ShowList.Add(blocks[0]); ShowList.Add(blocks[1]);
            }
            for (int i = 0; i < elements; i++)
            {
                CurGrid.RowDefinitions.Add(new RowDefinition());
                //CurGrid.RowDefinitions[i].Height=new GridLength(50, GridUnitType.
            }
            ClearElements();
            Timer.Start();
        }
        SortedList<ResParams, double> CalcAntiCap()
        {
            SortedList<ResParams, double> result = new SortedList<ResParams, double>();
            double PlanetBonus = 1;
            if (Land.Planet.PlanetType == PlanetTypes.Freezed)
                PlanetBonus = ServerLinks.Parameters.FreezedCapBonus;
            double mainresult = 0;
            double secondresult = 0;
            foreach (LandSector sector in Land.Locations)
            {
                if (sector.Type == SectorTypes.AntiCap)
                {
                    PeaceSector peace = (PeaceSector)sector;
                    result.Add(ResParams.Main, peace.Buildings[0].GetTotalValue());
                    if (Land.Mult.Cap > 0)
                        result.Add(ResParams.MainBonus, Land.Mult.Cap);
                    if (Land.Mult.Total > 0)
                        result.Add(ResParams.MainBuild, Land.Mult.Total);
                    if (GSGameInfo.ScienceMult.Cap > 0)
                        result.Add(ResParams.MainPlayerBonus, GSGameInfo.ScienceMult.Cap);
                    if (GSGameInfo.ScienceMult.Total > 0)
                        result.Add(ResParams.MainPlayerBuild, GSGameInfo.ScienceMult.Total);
                    if (PlanetBonus > 1)
                        result.Add(ResParams.MainPlanetBonus, PlanetBonus);
                    mainresult = Math.Round(peace.Buildings[0].GetTotalValue() * PlanetBonus / 100.0 * (100 + Land.Mult.Cap + Land.Mult.Total + GSGameInfo.ScienceMult.Cap + GSGameInfo.ScienceMult.Total), 0);
                    result.Add(ResParams.MainResult, mainresult);
                }
                if (sector.Type == SectorTypes.Anti)
                {
                    PeaceSector peace = (PeaceSector)sector;
                    result.Add(ResParams.SecondBase, peace.Buildings[2].GetTotalValue());
                    if (Land.Mult.Cap > 0)
                        result.Add(ResParams.SecondBonus, Land.Mult.Cap);
                    if (Land.Mult.Total > 0)
                        result.Add(ResParams.SecondBuild, Land.Mult.Total);
                    if (GSGameInfo.ScienceMult.Cap > 0)
                        result.Add(ResParams.SecondPlayerBonus, GSGameInfo.ScienceMult.Cap);
                    if (GSGameInfo.ScienceMult.Total > 0)
                        result.Add(ResParams.SecontPlayerBuild, GSGameInfo.ScienceMult.Total);
                    if (PlanetBonus > 1)
                        result.Add(ResParams.SecondPlanetBonus, PlanetBonus);
                    secondresult = Math.Round(peace.Buildings[2].GetTotalValue() * PlanetBonus / 100.0 * (100 + Land.Mult.Cap + Land.Mult.Total + GSGameInfo.ScienceMult.Cap + GSGameInfo.ScienceMult.Total), 0);
                    result.Add(ResParams.SecondResult, secondresult);
                }
            }
            result.Add(ResParams.Result, mainresult + secondresult);
            return result;
        }
        void SetAntiGrid()
        {
            PrepareGridStep2("Выработка антиматерии");
            SortedList<ResParams, double> anti = CalcAnti();
            int elements = 0;
            foreach (KeyValuePair<ResParams, double> pair in anti)
            {
                TextBlock[] blocks = new TextBlock[] { null, null };
                switch (pair.Key)
                {
                    case ResParams.Main:
                        blocks = GetLeftRightBlocks(30, "Базовая выработка ускорителя частиц", pair.Value.ToString("### ### ### ##0"), "/сут");
                        break;
                    case ResParams.MainBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус выработки антиматерии колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainPlayerBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус государства к выработке антиматерии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainPlayerBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий государства", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainResult:
                        blocks = GetLeftRightBlocks(36, "Итоговая выработка антиматерии ускорителя частиц", pair.Value.ToString("### ### ### ##0"), "/сут");
                        break;
                    case ResParams.SecondBase:
                        blocks = GetLeftRightBlocks(30, "Базовая выработка антиматерии термоядерного реактора", pair.Value.ToString("### ### ### ##0"), "/сут");
                        break;
                    case ResParams.SecondBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус выработки антиматерии колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecondBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecondPlayerBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус государства к выработке антиматерии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecontPlayerBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий государства", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecondResult:
                        blocks = GetLeftRightBlocks(36, "Итоговая выработка антиматерии термоядерного реактора", pair.Value.ToString("### ### ### ##0"), "/сут");
                        break;
                    case ResParams.Result:
                        blocks = GetLeftRightBlocks(36, "Итоговая выработка антиматерии", pair.Value.ToString("### ### ### ##0"), "/сут");
                        break;
                }
                CurGrid.Children.Add(blocks[0]); CurGrid.Children.Add(blocks[1]);
                Grid.SetRow(blocks[0], elements);
                Grid.SetRow(blocks[1], elements);
                Grid.SetColumn(blocks[1], 1);
                elements++;
                ShowList.Add(blocks[0]); ShowList.Add(blocks[1]);
            }
            for (int i = 0; i < elements; i++)
                CurGrid.RowDefinitions.Add(new RowDefinition());
            ClearElements();
            Timer.Start();
        }
        SortedList<ResParams, double> CalcAnti()
        {
            SortedList<ResParams, double> result = new SortedList<ResParams, double>();
            double mainresult = 0;
            double secondresult = 0;
            foreach (LandSector sector in Land.Locations)
            {
                if (sector.Type == SectorTypes.Anti)
                {
                    PeaceSector peace = (PeaceSector)sector;
                    result.Add(ResParams.Main, peace.Buildings[0].GetTotalValue());
                    if (Land.Mult.Anti > 0)
                        result.Add(ResParams.MainBonus, Land.Mult.Anti);
                    if (Land.Mult.Total > 0)
                        result.Add(ResParams.MainBuild, Land.Mult.Total);
                    if (GSGameInfo.ScienceMult.Anti > 0)
                        result.Add(ResParams.MainPlayerBonus, GSGameInfo.ScienceMult.Anti);
                    if (GSGameInfo.ScienceMult.Total > 0)
                        result.Add(ResParams.MainPlayerBuild, GSGameInfo.ScienceMult.Total);
                    mainresult = Math.Round(peace.Buildings[0].GetTotalValue() / 100.0 * (100 + Land.Mult.Anti + Land.Mult.Total + GSGameInfo.ScienceMult.Anti + GSGameInfo.ScienceMult.Total), 0);
                    result.Add(ResParams.MainResult, mainresult);
                }
                if (sector.Type == SectorTypes.AntiCap)
                {
                    PeaceSector peace = (PeaceSector)sector;
                    result.Add(ResParams.SecondBase, peace.Buildings[2].GetTotalValue());
                    if (Land.Mult.Anti > 0)
                        result.Add(ResParams.SecondBonus, Land.Mult.Anti);
                    if (Land.Mult.Total > 0)
                        result.Add(ResParams.SecondBuild, Land.Mult.Total);
                    if (GSGameInfo.ScienceMult.Anti > 0)
                        result.Add(ResParams.SecondPlayerBonus, GSGameInfo.ScienceMult.Anti);
                    if (GSGameInfo.ScienceMult.Total > 0)
                        result.Add(ResParams.SecontPlayerBuild, GSGameInfo.ScienceMult.Total);
                    secondresult = Math.Round(peace.Buildings[2].GetTotalValue() / 100.0 * (100 + Land.Mult.Anti + Land.Mult.Total + GSGameInfo.ScienceMult.Anti + GSGameInfo.ScienceMult.Total), 0);
                    result.Add(ResParams.SecondResult, secondresult);
                }
            }
            result.Add(ResParams.Result, mainresult + secondresult);
            return result;
        }
        void SetChipCapGrid()
        {
            PrepareGridStep2("Склад микросхем");
            SortedList<ResParams, double> chipcap = CalcChipCap();
            int elements = 0;
            foreach (KeyValuePair<ResParams, double> pair in chipcap)
            {
                TextBlock[] blocks = new TextBlock[] { null, null };
                switch (pair.Key)
                {
                    case ResParams.Main:
                        blocks = GetLeftRightBlocks(30, "Базовое хранение микросхем склада", pair.Value.ToString("### ### ### ###"), "");
                        break;
                    case ResParams.MainBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус хранения колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainPlayerBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус государства к хранению", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainPlayerBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий государства", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainPlanetBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус ледяной планеты", pair.Value.ToString("x#.#"), "");
                        break;
                    case ResParams.MainResult:
                        blocks = GetLeftRightBlocks(36, "Итоговое хранение микросхем склада", pair.Value.ToString("### ### ### ###"), "");
                        break;
                    case ResParams.SecondBase:
                        blocks = GetLeftRightBlocks(30, "Базовое хранение микросхем сервисного центра", pair.Value.ToString("### ### ### ###"), "");
                        break;
                    case ResParams.SecondBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус хранения колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecondBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecondPlayerBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус государства к хранению", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecontPlayerBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий государства", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecondPlanetBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус ледяной планеты", pair.Value.ToString("x#.#"), "");
                        break;
                    case ResParams.SecondResult:
                        blocks = GetLeftRightBlocks(36, "Итоговое хранение микросхем сервисного центра", pair.Value.ToString("### ### ### ###"), "");
                        break;
                    case ResParams.Result:
                        blocks = GetLeftRightBlocks(36, "Итоговое хранение микросхем", pair.Value.ToString("### ### ### ##0"), "");
                        break;
                }
                CurGrid.Children.Add(blocks[0]); CurGrid.Children.Add(blocks[1]);
                Grid.SetRow(blocks[0], elements);
                Grid.SetRow(blocks[1], elements);
                Grid.SetColumn(blocks[1], 1);
                elements++;
                ShowList.Add(blocks[0]); ShowList.Add(blocks[1]);
            }
            for (int i = 0; i < elements; i++)
            {
                CurGrid.RowDefinitions.Add(new RowDefinition());
                //CurGrid.RowDefinitions[i].Height=new GridLength(50, GridUnitType.
            }
            ClearElements();
            Timer.Start();
        }
        SortedList<ResParams, double> CalcChipCap()
        {
            SortedList<ResParams, double> result = new SortedList<ResParams, double>();
            double PlanetBonus = 1;
            if (Land.Planet.PlanetType == PlanetTypes.Freezed)
                PlanetBonus = ServerLinks.Parameters.FreezedCapBonus;
            double mainresult = 0;
            double secondresult = 0;
            foreach (LandSector sector in Land.Locations)
            {
                if (sector.Type == SectorTypes.ChipsCap)
                {
                    PeaceSector peace = (PeaceSector)sector;
                    result.Add(ResParams.Main, peace.Buildings[0].GetTotalValue());
                    if (Land.Mult.Cap > 0)
                        result.Add(ResParams.MainBonus, Land.Mult.Cap);
                    if (Land.Mult.Total > 0)
                        result.Add(ResParams.MainBuild, Land.Mult.Total);
                    if (GSGameInfo.ScienceMult.Cap > 0)
                        result.Add(ResParams.MainPlayerBonus, GSGameInfo.ScienceMult.Cap);
                    if (GSGameInfo.ScienceMult.Total > 0)
                        result.Add(ResParams.MainPlayerBuild, GSGameInfo.ScienceMult.Total);
                    if (PlanetBonus > 1)
                        result.Add(ResParams.MainPlanetBonus, PlanetBonus);
                    mainresult = Math.Round(peace.Buildings[0].GetTotalValue() * PlanetBonus / 100.0 * (100 + Land.Mult.Cap + Land.Mult.Total + GSGameInfo.ScienceMult.Cap + GSGameInfo.ScienceMult.Total), 0);
                    result.Add(ResParams.MainResult, mainresult);
                }
                if (sector.Type == SectorTypes.Chips)
                {
                    PeaceSector peace = (PeaceSector)sector;
                    result.Add(ResParams.SecondBase, peace.Buildings[2].GetTotalValue());
                    if (Land.Mult.Cap > 0)
                        result.Add(ResParams.SecondBonus, Land.Mult.Cap);
                    if (Land.Mult.Total > 0)
                        result.Add(ResParams.SecondBuild, Land.Mult.Total);
                    if (GSGameInfo.ScienceMult.Cap > 0)
                        result.Add(ResParams.SecondPlayerBonus, GSGameInfo.ScienceMult.Cap);
                    if (GSGameInfo.ScienceMult.Total > 0)
                        result.Add(ResParams.SecontPlayerBuild, GSGameInfo.ScienceMult.Total);
                    if (PlanetBonus > 1)
                        result.Add(ResParams.SecondPlanetBonus, PlanetBonus);
                    secondresult = Math.Round(peace.Buildings[2].GetTotalValue() * PlanetBonus / 100.0 * (100 + Land.Mult.Cap + Land.Mult.Total + GSGameInfo.ScienceMult.Cap + GSGameInfo.ScienceMult.Total), 0);
                    result.Add(ResParams.SecondResult, secondresult);
                }
            }
            result.Add(ResParams.Result, mainresult + secondresult);
            return result;
        }
        SortedList<ResParams, double> CalcChips()
        {
            SortedList<ResParams, double> result = new SortedList<ResParams, double>();
            double mainresult = 0;
            double secondresult = 0;
            foreach (LandSector sector in Land.Locations)
            {
                if (sector.Type == SectorTypes.Chips)
                {
                    PeaceSector peace = (PeaceSector)sector;
                    result.Add(ResParams.Main, peace.Buildings[0].GetTotalValue());
                    if (Land.Mult.Chips > 0)
                        result.Add(ResParams.MainBonus, Land.Mult.Chips);
                    if (Land.Mult.Total > 0)
                        result.Add(ResParams.MainBuild, Land.Mult.Total);
                    if (GSGameInfo.ScienceMult.Chips > 0)
                        result.Add(ResParams.MainPlayerBonus, GSGameInfo.ScienceMult.Chips);
                    if (GSGameInfo.ScienceMult.Total > 0)
                        result.Add(ResParams.MainPlayerBuild, GSGameInfo.ScienceMult.Total);
                    mainresult = Math.Round(peace.Buildings[0].GetTotalValue() / 100.0 * (100 + Land.Mult.Chips + Land.Mult.Total + GSGameInfo.ScienceMult.Chips + GSGameInfo.ScienceMult.Total), 0);
                    result.Add(ResParams.MainResult, mainresult);
                }
                if (sector.Type == SectorTypes.ChipsCap)
                {
                    PeaceSector peace = (PeaceSector)sector;
                    result.Add(ResParams.SecondBase, peace.Buildings[2].GetTotalValue());
                    if (Land.Mult.Chips > 0)
                        result.Add(ResParams.SecondBonus, Land.Mult.Chips);
                    if (Land.Mult.Total > 0)
                        result.Add(ResParams.SecondBuild, Land.Mult.Total);
                    if (GSGameInfo.ScienceMult.Chips > 0)
                        result.Add(ResParams.SecondPlayerBonus, GSGameInfo.ScienceMult.Chips);
                    if (GSGameInfo.ScienceMult.Total > 0)
                        result.Add(ResParams.SecontPlayerBuild, GSGameInfo.ScienceMult.Total);
                    secondresult = Math.Round(peace.Buildings[2].GetTotalValue()  / 100.0 * (100 + Land.Mult.Chips + Land.Mult.Total + GSGameInfo.ScienceMult.Chips + GSGameInfo.ScienceMult.Total), 0);
                    result.Add(ResParams.SecondResult, secondresult);
                }
            }
            result.Add(ResParams.Result, mainresult + secondresult);
            return result;
        }
        void SetChipsGrid()
        {
            PrepareGridStep2("Производство микросхем");
            SortedList<ResParams, double> chips = CalcChips();
            int elements = 0;
            foreach (KeyValuePair<ResParams, double> pair in chips)
            {
                TextBlock[] blocks = new TextBlock[] { null, null };
                switch (pair.Key)
                {
                    case ResParams.Main:
                        blocks = GetLeftRightBlocks(30, "Базовое производство фабрики", pair.Value.ToString("### ### ### ##0"), "/сут");
                        break;
                    case ResParams.MainBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус производства микросхем колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainPlayerBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус государства к производству микросхем", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainPlayerBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий государства", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainResult:
                        blocks = GetLeftRightBlocks(36, "Итоговое производство микросхем фабрики", pair.Value.ToString("### ### ### ##0"), "/сут");
                        break;
                    case ResParams.SecondBase:
                        blocks = GetLeftRightBlocks(30, "Базовое производство микросхем хранилища", pair.Value.ToString("### ### ### ##0"), "/сут");
                        break;
                    case ResParams.SecondBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус производства микросхем колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecondBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecondPlayerBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус государства к производству микросхем", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecontPlayerBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий государства", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecondResult:
                        blocks = GetLeftRightBlocks(36, "Итоговое производство микросхем переработки", pair.Value.ToString("### ### ### ##0"), "/сут");
                        break;
                    case ResParams.Result:
                        blocks = GetLeftRightBlocks(36, "Итоговое производство микросхем", pair.Value.ToString("### ### ### ##0"), "/сут");
                        break;
                }
                CurGrid.Children.Add(blocks[0]); CurGrid.Children.Add(blocks[1]);
                Grid.SetRow(blocks[0], elements);
                Grid.SetRow(blocks[1], elements);
                Grid.SetColumn(blocks[1], 1);
                elements++;
                ShowList.Add(blocks[0]); ShowList.Add(blocks[1]);
            }
            for (int i = 0; i < elements; i++)
                CurGrid.RowDefinitions.Add(new RowDefinition());
            ClearElements();
            Timer.Start();
        }
        SortedList<ResParams, double> CalcMetallCap()
        {
            SortedList<ResParams, double> result = new SortedList<ResParams, double>();
            double PlanetBonus = 1;
            if (Land.Planet.PlanetType == PlanetTypes.Freezed)
                PlanetBonus = ServerLinks.Parameters.FreezedCapBonus;
            double mainresult = 0;
            double secondresult = 0;
            foreach (LandSector sector in Land.Locations)
            {
                if (sector.Type == SectorTypes.MetalCap)
                {
                    PeaceSector peace = (PeaceSector)sector;
                    result.Add(ResParams.Main, peace.Buildings[0].GetTotalValue());
                    if (Land.Mult.Cap > 0)
                        result.Add(ResParams.MainBonus, Land.Mult.Cap);
                    if (Land.Mult.Total > 0)
                        result.Add(ResParams.MainBuild, Land.Mult.Total);
                    if (GSGameInfo.ScienceMult.Cap > 0)
                        result.Add(ResParams.MainPlayerBonus, GSGameInfo.ScienceMult.Cap);
                    if (GSGameInfo.ScienceMult.Total > 0)
                        result.Add(ResParams.MainPlayerBuild, GSGameInfo.ScienceMult.Total);
                    if (PlanetBonus > 1)
                        result.Add(ResParams.MainPlanetBonus, PlanetBonus);
                    mainresult = Math.Round(peace.Buildings[0].GetTotalValue() * PlanetBonus / 100.0 * (100 + Land.Mult.Cap + Land.Mult.Total + GSGameInfo.ScienceMult.Cap + GSGameInfo.ScienceMult.Total), 0);
                    result.Add(ResParams.MainResult, mainresult);
                }
                if (sector.Type == SectorTypes.Metall)
                {
                    PeaceSector peace = (PeaceSector)sector;
                    result.Add(ResParams.SecondBase, peace.Buildings[2].GetTotalValue());
                    if (Land.Mult.Cap > 0)
                        result.Add(ResParams.SecondBonus, Land.Mult.Cap);
                    if (Land.Mult.Total > 0)
                        result.Add(ResParams.SecondBuild, Land.Mult.Total);
                    if (GSGameInfo.ScienceMult.Cap > 0)
                        result.Add(ResParams.SecondPlayerBonus, GSGameInfo.ScienceMult.Cap);
                    if (GSGameInfo.ScienceMult.Total > 0)
                        result.Add(ResParams.SecontPlayerBuild, GSGameInfo.ScienceMult.Total);
                    if (PlanetBonus > 1)
                        result.Add(ResParams.SecondPlanetBonus, PlanetBonus);
                    secondresult = Math.Round(peace.Buildings[2].GetTotalValue() * PlanetBonus / 100.0 * (100 + Land.Mult.Cap + Land.Mult.Total + GSGameInfo.ScienceMult.Cap + GSGameInfo.ScienceMult.Total), 0);
                    result.Add(ResParams.SecondResult, secondresult);
                }
            }
            result.Add(ResParams.Result, mainresult + secondresult);
            return result;
        }
        void SetMetalCapGrid()
        {
            PrepareGridStep2("Склад металла");
            SortedList<ResParams, double> metalcap = CalcMetallCap();
            int elements = 0;
            foreach (KeyValuePair<ResParams, double> pair in metalcap)
            {
                TextBlock[] blocks = new TextBlock[] { null, null };
                switch (pair.Key)
                {
                    case ResParams.Main:
                        blocks = GetLeftRightBlocks(30, "Базовое хранение металла склада", pair.Value.ToString("### ### ### ###"), "");
                        break;
                    case ResParams.MainBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус хранения колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainPlayerBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус государства к хранению", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainPlayerBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий государства", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainPlanetBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус ледяной планеты", pair.Value.ToString("x#.#"), "");
                        break;
                    case ResParams.MainResult:
                        blocks = GetLeftRightBlocks(36, "Итоговое хранение металла склада", pair.Value.ToString("### ### ### ###"), "");
                        break;
                    case ResParams.SecondBase:
                        blocks = GetLeftRightBlocks(30, "Базовое хранение металла отвала руды", pair.Value.ToString("### ### ### ###"), "");
                        break;
                    case ResParams.SecondBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус хранения колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecondBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecondPlayerBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус государства к хранению", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecontPlayerBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий государства", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecondPlanetBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус ледяной планеты", pair.Value.ToString("x#.#"), "");
                        break;
                    case ResParams.SecondResult:
                        blocks = GetLeftRightBlocks(36, "Итоговое хранение металла отвала руды", pair.Value.ToString("### ### ### ###"), "");
                        break;
                    case ResParams.Result:
                        blocks = GetLeftRightBlocks(36, "Итоговое хранение металла", pair.Value.ToString("### ### ### ##0"), "");
                        break;
                }
                CurGrid.Children.Add(blocks[0]); CurGrid.Children.Add(blocks[1]);
                Grid.SetRow(blocks[0], elements);
                Grid.SetRow(blocks[1], elements);
                Grid.SetColumn(blocks[1], 1);
                elements++;
                ShowList.Add(blocks[0]); ShowList.Add(blocks[1]);
            }
            for (int i = 0; i < elements; i++)
            {
                CurGrid.RowDefinitions.Add(new RowDefinition());
                //CurGrid.RowDefinitions[i].Height=new GridLength(50, GridUnitType.
            }
            ClearElements();
            Timer.Start();
        }
        SortedList<ResParams, double> CalcMetall()
        {
            SortedList<ResParams, double> result = new SortedList<ResParams, double>();
            double PlanetBonus = 1;
            if (Land.Planet.PlanetType == PlanetTypes.Burned)
                PlanetBonus = ServerLinks.Parameters.BurnPlanetMetallAddBonus;
            double mineresult = 0;
            double resycleresult = 0;
            foreach (LandSector sector in Land.Locations)
            {
                if (sector.Type==SectorTypes.Metall)
                {
                    PeaceSector peace = (PeaceSector)sector;
                    result.Add(ResParams.Main, peace.Buildings[0].GetTotalValue());
                    if (Land.Mult.Metall > 0)
                        result.Add(ResParams.MainBonus, Land.Mult.Metall);
                    if (Land.Mult.Total > 0)
                        result.Add(ResParams.MainBuild, Land.Mult.Total);
                    if (GSGameInfo.ScienceMult.Metall > 0)
                        result.Add(ResParams.MainPlayerBonus, GSGameInfo.ScienceMult.Metall);
                    if (GSGameInfo.ScienceMult.Total > 0)
                        result.Add(ResParams.MainPlayerBuild, GSGameInfo.ScienceMult.Total);
                    if (PlanetBonus > 1)
                        result.Add(ResParams.MainPlanetBonus, PlanetBonus);
                    mineresult = Math.Round(peace.Buildings[0].GetTotalValue() * PlanetBonus / 100.0 * (100 + Land.Mult.Metall + Land.Mult.Total + GSGameInfo.ScienceMult.Metall + GSGameInfo.ScienceMult.Total), 0);
                    result.Add(ResParams.MainResult, mineresult);
                }
                if (sector.Type==SectorTypes.MetalCap)
                {
                    PeaceSector peace = (PeaceSector)sector;
                    result.Add(ResParams.SecondBase, peace.Buildings[2].GetTotalValue());
                    if (Land.Mult.Metall > 0)
                        result.Add(ResParams.SecondBonus, Land.Mult.Metall);
                    if (Land.Mult.Total > 0)
                        result.Add(ResParams.SecondBuild, Land.Mult.Total);
                    if (GSGameInfo.ScienceMult.Metall > 0)
                        result.Add(ResParams.SecondPlayerBonus, GSGameInfo.ScienceMult.Metall);
                    if (GSGameInfo.ScienceMult.Total > 0)
                        result.Add(ResParams.SecontPlayerBuild, GSGameInfo.ScienceMult.Total);
                    if (PlanetBonus > 1)
                        result.Add(ResParams.SecondPlanetBonus, PlanetBonus);
                    resycleresult = Math.Round(peace.Buildings[2].GetTotalValue() * PlanetBonus / 100.0 * (100 + Land.Mult.Metall + Land.Mult.Total + GSGameInfo.ScienceMult.Metall + GSGameInfo.ScienceMult.Total), 0);
                    result.Add(ResParams.SecondResult, resycleresult);
                }
            }
            result.Add(ResParams.Result, mineresult + resycleresult);
            return result;
        }
        void SetMetalGrid()
        {
            PrepareGridStep2("Добыча металла");
            SortedList<ResParams, double> metall = CalcMetall();
            int elements = 0;
            foreach (KeyValuePair<ResParams, double> pair in metall)
            {
                TextBlock[] blocks = new TextBlock[] { null, null };
                switch (pair.Key)
                {
                    case ResParams.Main:
                        blocks = GetLeftRightBlocks(30, "Базовая добыча шахты", pair.Value.ToString("### ### ### ###"), "/сут");
                        break;
                    case ResParams.MainBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус добычи металла колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainPlayerBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус государства к добыче маталла", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainPlayerBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий государства", pair.Value.ToString(), " %");
                        break;
                    case ResParams.MainPlanetBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус выжженой планеты", pair.Value.ToString("x#.#"), "");
                        break;
                    case ResParams.MainResult:
                        blocks=GetLeftRightBlocks(36, "Итоговая добыча шахты", pair.Value.ToString("### ### ### ###"), "/сут");
                        break;
                    case ResParams.SecondBase:
                        blocks = GetLeftRightBlocks(30, "Базовая добыча переработки", pair.Value.ToString("### ### ### ###"), "/сут");
                        break;
                    case ResParams.SecondBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус добычи металла колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecondBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий колонии", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecondPlayerBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус государства к добыче маталла", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecontPlayerBuild:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий государства", pair.Value.ToString(), " %");
                        break;
                    case ResParams.SecondPlanetBonus:
                        blocks = GetLeftRightBlocks(30, "Бонус выжженой планеты", pair.Value.ToString("x#.#"), "");
                        break;
                    case ResParams.SecondResult:
                        blocks = GetLeftRightBlocks(36, "Итоговая добыча переработки", pair.Value.ToString("### ### ### ###"), "/сут");
                        break;
                    case ResParams.Result:
                        blocks = GetLeftRightBlocks(36, "Итоговая добыча металла", pair.Value.ToString("### ### ### ###"), "/сут");
                        break;
                }
                CurGrid.Children.Add(blocks[0]); CurGrid.Children.Add(blocks[1]);
                Grid.SetRow(blocks[0], elements);
                Grid.SetRow(blocks[1], elements);
                Grid.SetColumn(blocks[1], 1);
                elements++;
                ShowList.Add(blocks[0]); ShowList.Add(blocks[1]);
            }
            for (int i = 0; i < elements; i++)
            {
                CurGrid.RowDefinitions.Add(new RowDefinition());
                //CurGrid.RowDefinitions[i].Height = new GridLength(50);
            }
            ClearElements();
            Timer.Start();
        }
        SortedList<MoneyParams, double> CalcMoney()
        {
            SortedList<MoneyParams, double> result = new SortedList<MoneyParams, double>();
            double taxbase = Math.Round(Land.Peoples * 10, 0);
            result.Add(MoneyParams.TaxBase, taxbase);
            if (Land.Mult.Money > 0)
                result.Add(MoneyParams.TaxLandBonus, Land.Mult.Money);
            if (GSGameInfo.ScienceMult.Money>0)
                result.Add(MoneyParams.TaxPlayerBonus, GSGameInfo.ScienceMult.Money);
            double taxresult = Math.Round(taxbase / 100.0 * (100 + Land.Mult.Money + GSGameInfo.ScienceMult.Money), 0);
            result.Add(MoneyParams.TaxResult, taxresult);
            double exporesult = 0;
            foreach (LandSector sector in Land.Locations)
            {
                if (sector.Type == SectorTypes.Money)
                {
                    PeaceSector peace = (PeaceSector)sector;
                    result.Add(MoneyParams.ExpoBase, peace.Buildings[0].GetTotalValue());
                    if (Land.Mult.Money > 0)
                        result.Add(MoneyParams.MoneyBonus, Land.Mult.Money);
                    if (Land.Mult.Total > 0)
                        result.Add(MoneyParams.BuildTotal, Land.Mult.Total);
                    if (GSGameInfo.ScienceMult.Money > 0)
                        result.Add(MoneyParams.PlayerMoney, GSGameInfo.ScienceMult.Money);
                    if (GSGameInfo.ScienceMult.Total > 0)
                        result.Add(MoneyParams.PlayerTotal, GSGameInfo.ScienceMult.Total);
                    exporesult = Math.Round(peace.Buildings[0].GetTotalValue() / 100.0 * 
                        (100 + Land.Mult.Money + Land.Mult.Total + GSGameInfo.ScienceMult.Money + GSGameInfo.ScienceMult.Total),0);
                    result.Add(MoneyParams.ExpoResult,exporesult);
                }
            }
            result.Add(MoneyParams.Result,taxresult+ exporesult);
            return result;
        }
        void SetMoneyGrid()
        {
            PrepareGridStep2("Получение денег");
            SortedList<MoneyParams, double> money = CalcMoney();
            int elements = 0;
            foreach (KeyValuePair<MoneyParams, double> pair in money)
            {
                TextBlock[] blocks = new TextBlock[] { null, null };
                switch (pair.Key)
                {
                    case MoneyParams.TaxBase:
                        blocks=GetLeftRightBlocks(30, "Базовый налог",pair.Value.ToString("### ### ### ###"), "/сут");
                        break;
                    case MoneyParams.TaxLandBonus:
                        blocks = GetLeftRightBlocks(30, "Финансовый бонус колонии", pair.Value.ToString(), " %");
                        break;
                    case MoneyParams.TaxPlayerBonus:
                        blocks = GetLeftRightBlocks(30, "Финансовый бонус государства", pair.Value.ToString(), " %");
                        break;
                    case MoneyParams.TaxResult:
                        blocks = GetLeftRightBlocks(36, "Итоговый налог", pair.Value.ToString("### ### ### ###"), "/сут");
                        break;
                    case MoneyParams.ExpoBase:
                        blocks = GetLeftRightBlocks(30, "База выставочоного центра", pair.Value.ToString("### ### ### ###"), "/сут");
                        break;
                    case MoneyParams.MoneyBonus:
                        blocks = GetLeftRightBlocks(30, "Финансовый бонус колонии", pair.Value.ToString(), " %");
                        break;
                    case MoneyParams.BuildTotal:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий колонии", pair.Value.ToString(), " %");
                        break;
                    case MoneyParams.PlayerMoney:
                        blocks = GetLeftRightBlocks(30, "Финансовый бонус государства", pair.Value.ToString(), " %");
                        break;
                    case MoneyParams.PlayerTotal:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий государства", pair.Value.ToString(), " %");
                        break;
                    case MoneyParams.ExpoResult:
                        blocks = GetLeftRightBlocks(36, "Результат выставочного центра", pair.Value.ToString("### ### ### ###"), "/сут");
                        break;
                    case MoneyParams.Result:
                        blocks = GetLeftRightBlocks(36, "Всего", pair.Value.ToString("### ### ### ###"), "/сут");
                        break;
                }
                CurGrid.Children.Add(blocks[0]); CurGrid.Children.Add(blocks[1]);
                Grid.SetRow(blocks[0], elements);
                Grid.SetRow(blocks[1], elements);
                Grid.SetColumn(blocks[1], 1);
                elements++;
                ShowList.Add(blocks[0]); ShowList.Add(blocks[1]);
            }
            for (int i = 0; i < elements; i++)
            {
                CurGrid.RowDefinitions.Add(new RowDefinition());
                //CurGrid.RowDefinitions[i].Height = new GridLength(50);
            }
            ClearElements();
            Timer.Start();
        }
        TextBlock[] GetLeftRightBlocks(int size, string title, string t1, string t2)
        {
            TextBlock[] result = new TextBlock[2];
            result[0] = Common.GetBlock(size, title, Brushes.White, 450);
            result[1] = Common.GetBlock(size, "", Brushes.White, 450);
            result[1].Inlines.AddRange(AddText(t1, t2));
            return result;
        }
        void SetGrowGrid()
        {
            PrepareGridStep2("Прирост населения");
            SortedList<GrowParams, double> growparams = CalcGrow();
            int elements = 0;
            foreach (KeyValuePair<GrowParams, double> pair in growparams)
            {
                TextBlock[] blocks = new TextBlock[] { null, null };
                switch (pair.Key)
                {
                    case GrowParams.Base:
                        blocks=GetLeftRightBlocks(36, "Базовый прирост", pair.Value.ToString("### ##0.0"), " млн. чел/мес");
                        break;
                    case GrowParams.GreenPlanet:
                        blocks = GetLeftRightBlocks(36, "Бонус зелёной планеты", pair.Value.ToString("### ##0.0"), " млн. чел/мес");
                        break;
                    case GrowParams.MedBase:
                        blocks = GetLeftRightBlocks(30, "База медицинского центра", pair.Value.ToString("### ##0.0"), " млн. чел/мес");
                        break;
                    case GrowParams.BuildTotal:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий колонии", pair.Value.ToString(), " %");
                        break;
                    case GrowParams.PlayerTotal:
                        blocks = GetLeftRightBlocks(30, "Бонус зданий государства", pair.Value.ToString(), " %");
                        break;
                    case GrowParams.MedReuslt:
                        blocks = GetLeftRightBlocks(36, "Бонус медицинского центра", pair.Value.ToString("### ##0.0"), " млн. чел/мес");
                        break;
                    case GrowParams.Result:
                        blocks = GetLeftRightBlocks(36, "Итоговый прирост населения", pair.Value.ToString("### ##0.0"), " млн. чел/мес");
                        break;
                }
                CurGrid.Children.Add(blocks[0]); CurGrid.Children.Add(blocks[1]);
                Grid.SetRow(blocks[0], elements);
                Grid.SetRow(blocks[1], elements);
                Grid.SetColumn(blocks[1], 1);
                elements++;
                ShowList.Add(blocks[0]); ShowList.Add(blocks[1]);
            }
            for (int i = 0; i < elements; i++)
            {
                CurGrid.RowDefinitions.Add(new RowDefinition());
                //CurGrid.RowDefinitions[i].Height = new GridLength(50);
            }
            ClearElements();
            Timer.Start();
        }
        SortedList<GrowParams, double> CalcGrow()
        {
            SortedList<GrowParams, double> result = new SortedList<GrowParams, double>();
            result.Add(GrowParams.Base, 0.1);
            double planetbonus = 0;
            if (Land.Planet.PlanetType == PlanetTypes.Green)
            {
                planetbonus = Math.Round(Land.Planet.MaxPopulation * ServerLinks.Parameters.GreenPlanetGrowBonus, 1);
                result.Add(GrowParams.GreenPlanet, planetbonus);
            }
            double medresult = 0;
            foreach (LandSector sector in Land.Locations)
            {
                if (sector.Type == SectorTypes.Live)
                {
                    PeaceSector peace = (PeaceSector)sector;
                    result.Add(GrowParams.MedBase, peace.Buildings[0].GetTotalValue());
                    if (Land.Mult.Total > 0)
                        result.Add(GrowParams.BuildTotal, Land.Mult.Total);
                    if (GSGameInfo.ScienceMult.Total > 0)
                        result.Add(GrowParams.PlayerTotal, GSGameInfo.ScienceMult.Total);
                    medresult = Math.Round(peace.Buildings[0].GetTotalValue() / 100.0 * (100 + Land.Mult.Total + GSGameInfo.ScienceMult.Total), 1);
                    result.Add(GrowParams.MedReuslt, medresult);
                }
            }
            result.Add(GrowParams.Result, 0.1 + planetbonus + medresult);
            return result;
        }
        void PrepareGridStep2(string title)
        {
            Timer.Stop(); ShowList.Clear();
            ShowList.Add(back);
            Title.Text = title;
            CurGrid.Children.Clear(); CurGrid.ColumnDefinitions.Clear(); CurGrid.RowDefinitions.Clear();
            CurGrid.ColumnDefinitions.Add(new ColumnDefinition());
            CurGrid.ColumnDefinitions.Add(new ColumnDefinition());
            CurGrid.ColumnDefinitions[0].Width = new GridLength(450);
            CurGrid.ColumnDefinitions[1].Width = new GridLength(450);
        }
        void SetPeopleGrid()
        {
            PrepareGridStep2("Население");
            
            int elements = 3;
            TextBlock max = Common.GetBlock(36, "Максимум колонии", Brushes.White, 450);
            ShowList.Add(max); CurGrid.Children.Add(max);
            TextBlock maxValue = Common.GetBlock(36, "", Brushes.White,450);
            maxValue.Inlines.AddRange(AddText(Land.Planet.MaxPopulation.ToString("### ###.0"), " млн. чел"));
            ShowList.Add(maxValue); CurGrid.Children.Add(maxValue); Grid.SetColumn(maxValue, 1);
            TextBlock all = Common.GetBlock(36, "Всего", Brushes.White, 450);
            ShowList.Add(all);CurGrid.Children.Add(all);  Grid.SetRow(all, 1);
            TextBlock allValue = Common.GetBlock(36, "", Brushes.White, 450);
            allValue.Inlines.AddRange(AddText(Land.Peoples.ToString("### ###.0"), " млн. чел"));
            ShowList.Add(allValue); CurGrid.Children.Add(allValue);  Grid.SetColumn(allValue, 1); Grid.SetRow(allValue, 1);
            foreach (LandSector sector in Land.Locations)
            {
                if (sector.GetSectorType() == LandSectorType.Clear)
                    continue;
                PeaceSector peace = (PeaceSector)sector;
                elements++;
                TextBlock namesector = Common.GetBlock(36, SectorImages.List[peace.Type].Title, Brushes.White, 450);
                ShowList.Add(namesector); CurGrid.Children.Add(namesector); Grid.SetRow(namesector, elements - 2);
                TextBlock sectorvalue = Common.GetBlock(36, "", Brushes.White, 450);
                ShowList.Add(sectorvalue); CurGrid.Children.Add(sectorvalue); Grid.SetRow(sectorvalue, elements - 2); Grid.SetColumn(sectorvalue, 1);
                int size = 0;
                foreach (GSBuilding2 build in peace.Buildings)
                {
                    elements++;
                    TextBlock buildname = Common.GetBlock(30, build.Name.ToString() + " " + build.CurLevel.ToString(), Brushes.White, 400);
                    buildname.TextAlignment = TextAlignment.Left; buildname.Margin = new Thickness(50, 0, 0, 0);
                    ShowList.Add(buildname); CurGrid.Children.Add(buildname); Grid.SetRow(buildname, elements - 2);
                    size += build.GetTotalSize();
                    TextBlock buildvalue = Common.GetBlock(30, "", Brushes.White, 450);
                    ShowList.Add(buildvalue); CurGrid.Children.Add(buildvalue);
                    Grid.SetRow(buildvalue, elements - 2); Grid.SetColumn(buildvalue, 1);
                    buildvalue.Inlines.AddRange(AddText(build.GetTotalSize().ToString(), " млн. чел"));

                }
                sectorvalue.Inlines.AddRange(AddText(size.ToString(), " млн. чел"));
            }
            TextBlock free = Common.GetBlock(36, "Свободно", Brushes.White, 450);
            ShowList.Add(free); CurGrid.Children.Add(free); Grid.SetRow(free, elements-1);
            TextBlock freeValue = Common.GetBlock(36, "", Brushes.White, 450);
            freeValue.Inlines.AddRange(AddText((Land.Peoples-Land.BuildingsCount).ToString("### ###.0"), " млн. чел"));
            ShowList.Add(freeValue); CurGrid.Children.Add(freeValue); Grid.SetRow(freeValue, elements-1); Grid.SetColumn(freeValue, 1);
            for (int i = 0; i < elements; i++)
                CurGrid.RowDefinitions.Add(new RowDefinition());
            if (elements>16)
            {
                ShowList.Add(up); ShowList.Add(down);
            }
            ClearElements();
            Timer.Start();
        }
        void SetBaseGrid()
        {
            Title.Text = "Производство в колонии";
            CurGrid.Children.Clear(); CurGrid.ColumnDefinitions.Clear(); CurGrid.RowDefinitions.Clear();
            CurGrid.ColumnDefinitions.Add(new ColumnDefinition());
            CurGrid.ColumnDefinitions.Add(new ColumnDefinition());
            CurGrid.ColumnDefinitions[0].Width = new GridLength(450);
            CurGrid.ColumnDefinitions[1].Width = new GridLength(450);
            for (int i = 0; i < 10; i++)
                CurGrid.RowDefinitions.Add(new RowDefinition());
            ShowList.Clear();

            CurGrid.Children.Add(peop); ShowList.Add(peop);
            PutTextElement(0, 1, (Land.Peoples - Land.BuildingsCount).ToString("### ##0.0"), " млн. чел");

            CurGrid.Children.Add(grow); ShowList.Add(grow);
            SortedList<GrowParams, double> growvalue = CalcGrow();
            PutTextElement(1, 1, growvalue[GrowParams.Result].ToString("### ##0.0"), " млн. чел/мес");

            CurGrid.Children.Add(money); ShowList.Add(money);
            double moneyadd = CalcMoney()[MoneyParams.Result];
            PutTextElement(2, 1, moneyadd.ToString("### #### ### ##0"), "/сут");

            CurGrid.Children.Add(metall); ShowList.Add(metall);
            double metaladd = CalcMetall()[ResParams.Result];
            PutTextElement(3, 1, metaladd.ToString("### ### ### ##0"), "/сут");

            CurGrid.Children.Add(metallcap); ShowList.Add(metallcap);
            double metalcap = CalcMetallCap()[ResParams.Result];
            PutTextElement(4, 1, metalcap.ToString("### ### ### ##0"), "");

            CurGrid.Children.Add(chips); ShowList.Add(chips);
            double chipsval = CalcChips()[ResParams.Result];
            PutTextElement(5, 1, chipsval.ToString("### ### ### ##0"), "/сут");

            CurGrid.Children.Add(chipscap); ShowList.Add(chipscap);
            double chipcap = CalcChipCap()[ResParams.Result];
            PutTextElement(6, 1, chipcap.ToString("### ### ### ##0"), "");

            CurGrid.Children.Add(anti); ShowList.Add(anti);
            double antival = CalcAnti()[ResParams.Result];
            PutTextElement(7, 1, antival.ToString("### ### ### ##0"), "/сут");

            CurGrid.Children.Add(anticap); ShowList.Add(anticap);
            double anticapval = CalcAntiCap()[ResParams.Result];
            PutTextElement(8, 1, anticapval.ToString("### ### ### ##0"), "");

            CurGrid.Children.Add(repair); ShowList.Add(repair);
            double repairval = CalcRepair()[ResParams.Result];
            PutTextElement(9, 1, repairval.ToString("### ### ### ##0"), "/сут");

            DoubleAnimation anim = new DoubleAnimation(1, 0, TimeSpan.Zero);
            back.BeginAnimation(FrameworkElement.OpacityProperty, anim);
            ClearElements();
            Timer.Start();
        }
        void PutTextElement(int row, int column, string bluetext, string whitetext)
        {
            TextBlock tb = Common.GetBlock(36, "", Brushes.White, 450);
            CurGrid.Children.Add(tb); Grid.SetColumn(tb, column); Grid.SetRow(tb, row);
            tb.Inlines.AddRange(AddText(bluetext, whitetext));
            ShowList.Add(tb);
        }
        List<Inline> AddText(string blue, string white2)
        {
            List<Inline> list = new List<Inline>();
            int pos = 0;
            if (blue != null)
            {
                list.Add(new Run(blue));
                list[pos].Foreground = Links.Brushes.SkyBlue;
                pos++;
            }
            if (white2!= null)
            {
                list.Add(new Run(white2));
                list[pos].Foreground = Brushes.White;
                pos++;
            }
            return list;
        }
        private void SelectPanel(object sender, MouseButtonEventArgs e)
        {
            InfoCanvasButton btn = (InfoCanvasButton)sender;
            Panels panel = (Panels)btn.Tag;
            switch (panel)
            {
                case Panels.Peoples: SetPeopleGrid(); break;
                case Panels.Grow: SetGrowGrid(); break;
                case Panels.Money: SetMoneyGrid(); break;
                case Panels.Metall: SetMetalGrid(); break;
                case Panels.MetalCap: SetMetalCapGrid(); break;
                case Panels.Chips: SetChipsGrid(); break;
                case Panels.ChipsCap: SetChipCapGrid(); break;
                case Panels.Anti: SetAntiGrid(); break;
                case Panels.AntiCap: SetAntiCapGrid(); break;
                case Panels.Repair: SetRepairGrid(); break;
            }
        }

        private void Exit_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Controller.PopUpCanvas.Remove();
        }
        enum ArrowDirection { Left, Up, Down}
        /// <summary> кнопка со стрелочкой </summary>
        class ArrowCanvas:Canvas
        {
            ScaleTransform Scale;
            public ArrowCanvas(ArrowDirection direction)
            {
                Width = 75; Height = 75; 
                Ellipse el = new Ellipse(); el.Width = 75; el.Height = 75; el.Stroke = Links.Brushes.SkyBlue;
                el.StrokeThickness = 4; el.Fill = Brushes.Black; Children.Add(el);
                Path path = new Path(); path.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 200, 0));
                path.Data = new PathGeometry((PathFigureCollection)Links.conv.ConvertFrom("M10,37.5 l25,-25 v15 h25 v20 h-25 v15z"));
                if (direction==ArrowDirection.Up)
                {
                    this.RenderTransformOrigin = new Point(0.5, 0.5);
                    this.RenderTransform = new RotateTransform(90);
                }
                else if (direction==ArrowDirection.Down)
                {
                    this.RenderTransformOrigin = new Point(0.5, 0.5);
                    this.RenderTransform = new RotateTransform(-90);
                }
                Children.Add(path);
                path.RenderTransformOrigin = new Point(0.5, 0.5);
                path.RenderTransform = Scale = new ScaleTransform(1, 1);
                this.MouseEnter += Path_MouseEnter;
                this.MouseLeave += Path_MouseLeave;
            }

            private void Path_MouseLeave(object sender, MouseEventArgs e)
            {
                Scale.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                Scale.BeginAnimation(ScaleTransform.ScaleYProperty, null);
            }

            private void Path_MouseEnter(object sender, MouseEventArgs e)
            {
                DoubleAnimationUsingKeyFrames anim = new DoubleAnimationUsingKeyFrames();
                anim.Duration = TimeSpan.FromSeconds(0.7);
                anim.RepeatBehavior = RepeatBehavior.Forever;
                anim.KeyFrames.Add(new DiscreteDoubleKeyFrame(1, KeyTime.FromPercent(0)));
                anim.KeyFrames.Add(new DiscreteDoubleKeyFrame(0.7, KeyTime.FromPercent(0.5)));
                Scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
                Scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
            }
        }
        class InfoCanvasButton:Canvas
        {
            Rectangle Blue; Rectangle Yellow; TextBlock Block;
            SolidColorBrush BlockColor;
            public InfoCanvasButton (string text, Panels CurPanel)
            {
                Width = 450; Height = 60;
                Blue = Common.GetRectangle(450, 60, Links.Brushes.Colony.Colony6);
                Children.Add(Blue);
                Yellow = Common.GetRectangle(450, 60, Links.Brushes.Colony.Colony7);
                Children.Add(Yellow); Yellow.Opacity = 0;
                BlockColor = new SolidColorBrush(Colors.White);
                Block = Common.GetBlock(36, text, BlockColor, 450);
                Children.Add(Block); Canvas.SetTop(Block, 10);
                Tag = CurPanel;
                MouseEnter += InfoCanvasButton_MouseEnter;
                MouseLeave += InfoCanvasButton_MouseLeave;
            }

            private void InfoCanvasButton_MouseLeave(object sender, MouseEventArgs e)
            {
                DoubleAnimation anim1 = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
                Blue.BeginAnimation(Rectangle.OpacityProperty, anim1);
                DoubleAnimation anim2 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
                Yellow.BeginAnimation(Rectangle.OpacityProperty, anim2);
                ColorAnimation anim3 = new ColorAnimation(Colors.Purple, Colors.White, TimeSpan.FromSeconds(0.5));
                BlockColor.BeginAnimation(SolidColorBrush.ColorProperty, anim3);
            }

            private void InfoCanvasButton_MouseEnter(object sender, MouseEventArgs e)
            {
                DoubleAnimation anim1 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
                Blue.BeginAnimation(Rectangle.OpacityProperty, anim1);
                DoubleAnimation anim2 = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
                Yellow.BeginAnimation(Rectangle.OpacityProperty, anim2);
                ColorAnimation anim3 = new ColorAnimation(Colors.White, Colors.Purple, TimeSpan.FromSeconds(0.5));
                BlockColor.BeginAnimation(SolidColorBrush.ColorProperty, anim3);
            }
        }
    }
}
