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

namespace Client
{
    //public enum RiotStatus { None, PrepareRiot, Riot }
    public enum SectorTypes { Clear, Live, Money, Metall, MetalCap, Chips, ChipsCap, Anti, AntiCap, Repair, War, Peace }
    public class FleetBase : PeaceSector
    {
        public int MaxShips = 3;
        int BasicMath = 300;
        public int MaxMath
        {
            get
            {
                if (Fleet != null)
                    return BasicMath * (100 - (Fleet.Ships.Count - 1) * 5) / 100;
                else return BasicMath;
            }
            set
            {
                BasicMath = value;
            }
        }
        int BasicRange = 10;
        public int Range
        {
            get
            {
                if (Fleet != null)
                    return BasicRange - Fleet.Ships.Count + 1;
                else return BasicRange;
            }
            set
            {
                BasicRange = value;
            }
        }
        public GSFleet Fleet { get; private set; }
        public FleetBase(Land land, int position, byte[] array, ref int i) : base(land, position, SectorTypes.War, array, ref i)
        {
            Fleet = null;
        }
        public override LandSectorType GetSectorType()
        {
            return LandSectorType.War;
        }
        public void SetFleet (GSFleet fleet)
        {
            Fleet = fleet;
        }
    }
    public class LandSectorParamInfo
    {
        public Building2Parameter Parameter;
        public double Value;
        public LandSectorParamInfo(Building2Parameter par, double value)
        {
            Parameter = par; Value = value;
        }
    }
    public class LandSector
    {
        public Land Land;
        public SectorTypes Type;
        public int Position;
        public LandSector(Land land, int position)
        {
            Land = land;
            Type = SectorTypes.Clear;
            Position = position;
        }
        public static LandSector GetLandSector(Land land, int position, byte[] array, ref int i)
        {
            SectorTypes type = (SectorTypes)array[i]; i++;
            if (type == SectorTypes.Clear)
                return new LandSector(land, position);
            else if (type == SectorTypes.War)
                return new FleetBase(land, position, array, ref i);
            else
                return new PeaceSector(land, position, type, array, ref i);
        }
        public virtual LandSectorType GetSectorType()
        {
            return LandSectorType.Clear;
        }
    }
    public class PeaceSector : LandSector
    {
        public List<GSBuilding2> Buildings;
        public PeaceSector(Land land, int position, SectorTypes type, byte[] array, ref int i) : base(land, position)
        {
            Type = type;
            Buildings = new List<GSBuilding2>();
            Buildings.Add(new GSBuilding2(land.ID, (byte)position, type, 0));
            Buildings.Add(new GSBuilding2(land.ID, (byte)position, type, 1));
            Buildings.Add(new GSBuilding2(land.ID, (byte)position, type, 2));
            Buildings[0].CurLevel = array[i]; i++;
            Buildings[1].CurLevel = array[i]; i++;
            Buildings[2].CurLevel = array[i]; i++;
        }

        public override LandSectorType GetSectorType()
        {
            return LandSectorType.Peace;
        }
       public List<LandSectorParamInfo> GetParamsList()
        {
            List<LandSectorParamInfo> list = new List<LandSectorParamInfo>();
            switch (Type)
            {
                case SectorTypes.Live:
                    list.Add(new LandSectorParamInfo(Building2Parameter.PeopleGrow, Math.Round(Buildings[0].GetTotalValue() / 100.0 * (100 + Land.Mult.Total), 1)));
                    list.Add(new LandSectorParamInfo(Building2Parameter.BuildingEffect, Buildings[1].GetTotalValue()));
                    list.Add(new LandSectorParamInfo(Building2Parameter.MoneyEffect, Buildings[2].GetTotalValue()));
                    break;
            }
            return list;
        }
    }
    public class Land
    {
        public int ID; //ID территории
        public GSString Name; //Название территории
        public int PlanetID; //ID планеты, на которой расположена территория
        //public int Peoples; //округленное население территории, расчётное
        public double Peoples; //население территории, точное
                               //public int LandLevel; //уровень территории, считывается с уровня планеты
                               //public int LandSize; //размер территории, считывается с параметров планеты
                               //public int BuildingsCount; //количество построек, расчитывается

        //public List<SectorTypes> LocationTypes;
        public GSPlanet Planet; //ссылка на планету, на которой расположена территория
        //public RiotStatus Riot;
        //public DateTime RiotEnd;
        public LandSector[] Locations;
        public SortedList<long, GSFleet> Fleets;
        public LandAdd Add;
        public LandMult Mult;
        public LandCapacity Capacity;
        public int TrueCap;
        public int BuildingsCount;
        public List<FleetBase> FleetBases = new List<FleetBase>();
        public bool Pillage;
        public bool Conquer;
        public byte RiotIndicator;
        public Land(byte[] array, ref int i)
        {
            ID = BitConverter.ToInt32(array, i); i += 4;
            PlanetID = BitConverter.ToInt32(array, i); i += 4;
            Planet = Links.Planets[PlanetID];
            int PlayerID = BitConverter.ToInt32(array, i); i += 4;
            Name = new GSString(array, i); i += Name.Array.Length;
            Peoples = BitConverter.ToDouble(array, i); i += 8;
            byte locations = array[i]; i++;
            Locations = new LandSector[locations];
            for (int j = 0; j < locations; j++)
                Locations[j] = LandSector.GetLandSector(this, j, array, ref i);
            Pillage = array[i] > 0; i++; 
            Conquer = array[i] > 0; i++;
            RiotIndicator = array[i]; i++; 
            Fleets = new SortedList<long, GSFleet>();
            Calculate();
        }
        public double GetGrow()
        {
            LandInfo landinfo = new LandInfo(this);
            return landinfo.GetCurGrow();
        }
        public int[] GetFleetsAndShips()
        {
            int ships = 0;
            foreach (GSFleet fleet in Fleets.Values)
                ships += fleet.Ships.Count;
            return new int[] { Fleets.Count, ships };
        }
        public void Calculate()
        {
            BuildingsCount = 0;
            Add = new LandAdd();
            Mult = new LandMult();
            Capacity = new LandCapacity();
            foreach (LandSector sector in Locations)
            {
                if (sector.GetSectorType() != LandSectorType.Peace) continue;
                PeaceSector s = (PeaceSector)sector;
                foreach (GSBuilding2 building in s.Buildings)
                {
                    BuildingsCount += building.GetTotalSize();
                    switch (building.Parameter)
                    {
                        case Building2Parameter.PeopleGrow: Add.Grow += building.GetTotalValue(); break;
                        case Building2Parameter.BuildingEffect: Mult.Total += building.GetTotalValue(); break;
                        case Building2Parameter.MoneyEffect: Mult.Money += building.GetTotalValue(); break;
                        case Building2Parameter.MoneyAdd: Add.Money += (int)building.GetTotalValue(); break;
                        case Building2Parameter.Repair: Capacity.Repair += (int)building.GetTotalValue(); break;
                        case Building2Parameter.RepairEffect: Mult.Repair += building.GetTotalValue(); break;
                        case Building2Parameter.MetallAdd: Add.Metall += (int)building.GetTotalValue(); break;
                        case Building2Parameter.ChipAdd: Add.Chips += (int)building.GetTotalValue(); break;
                        case Building2Parameter.AntiAdd: Add.Anti += (int)building.GetTotalValue(); break;
                        case Building2Parameter.MetallEffect: Mult.Metall += building.GetTotalValue(); break;
                        case Building2Parameter.ChipEffect: Mult.Chips += building.GetTotalValue(); break;
                        case Building2Parameter.AntiEffect: Mult.Anti += building.GetTotalValue(); break;
                        case Building2Parameter.MetalCap: Capacity.Metall += (int)building.GetTotalValue(); break;
                        case Building2Parameter.ChipCap: Capacity.Chips += (int)building.GetTotalValue(); break;
                        case Building2Parameter.AntiCap: Capacity.Anti += (int)building.GetTotalValue(); break;
                        case Building2Parameter.CapEffect: Mult.Cap += building.GetTotalValue(); break;
                    }
                }
            }
            foreach (LandSector sector in Locations)
            {
                if (sector.GetSectorType() != LandSectorType.War) continue;
                FleetBase b = (FleetBase)sector;
                foreach (GSBuilding2 building in b.Buildings)
                {
                    BuildingsCount += building.GetTotalSize();
                    switch (building.Parameter)
                    {
                        case Building2Parameter.Range: b.Range = (int)building.GetTotalValue(); break;
                        case Building2Parameter.Ships: b.MaxShips = (int)building.GetTotalValue(); break;
                        case Building2Parameter.MathPower: b.MaxMath = (int)Math.Round(building.GetTotalValue() / 100.0 * (100 + Mult.Total + GSGameInfo.ScienceMult.Total), 0); break;
                    }
                }
            }
        }
        public double GetAddParameter(Building2Parameter par)
        {
            switch (par)
            {
                case Building2Parameter.PeopleGrow:
                    if (Planet.PlanetType == PlanetTypes.Green)
                        return Math.Round(Planet.MaxPopulation * ServerLinks.Parameters.GreenPlanetGrowBonus + 0.1 + Add.Grow / 100.0 * (100 + Mult.Total + GSGameInfo.ScienceMult.Total), 1);
                    else
                        return Math.Round(0.1 + Add.Grow / 100.0 * (100 + Mult.Total + GSGameInfo.ScienceMult.Total), 1);
                case Building2Parameter.MoneyAdd:
                    return Math.Round(Add.Money / 100.0 * (100 + Mult.Money + Mult.Total + GSGameInfo.ScienceMult.Money + GSGameInfo.ScienceMult.Total) + Peoples * 10 / 100.0 * (100 + Mult.Money + GSGameInfo.ScienceMult.Money), 0);
                case Building2Parameter.MetallAdd:
                    if (Planet.PlanetType == PlanetTypes.Burned)
                        return Math.Round(Add.Metall * ServerLinks.Parameters.BurnPlanetMetallAddBonus / 100.0 * (100 + Mult.Metall + Mult.Total + GSGameInfo.ScienceMult.Metall + GSGameInfo.ScienceMult.Total), 0);
                    else
                        return Math.Round(Add.Metall / 100.0 * (100 + Mult.Metall + Mult.Total + GSGameInfo.ScienceMult.Metall + GSGameInfo.ScienceMult.Total), 0);
                case Building2Parameter.ChipAdd:
                    return Math.Round(Add.Chips / 100.0 * (100 + Mult.Chips + Mult.Total + GSGameInfo.ScienceMult.Chips + GSGameInfo.ScienceMult.Total), 0);
                case Building2Parameter.AntiAdd:
                    if (Planet.PlanetType == PlanetTypes.Gas)
                        return Math.Round(Add.Anti * ServerLinks.Parameters.GasPlanetAntiBonus / 100.0 * (100 + Mult.Anti + Mult.Total + GSGameInfo.ScienceMult.Anti + GSGameInfo.ScienceMult.Total), 0);
                    else
                        return Math.Round(Add.Anti / 100.0 * (100 + Mult.Anti + Mult.Total + GSGameInfo.ScienceMult.Anti + GSGameInfo.ScienceMult.Total), 0);
                case Building2Parameter.MetalCap:
                    if (Planet.PlanetType == PlanetTypes.Freezed)
                        return Math.Round(Capacity.Metall * ServerLinks.Parameters.FreezedCapBonus / 100.0 * (100 + Mult.Cap + Mult.Total + GSGameInfo.ScienceMult.Cap + GSGameInfo.ScienceMult.Total), 0);
                    else
                        return Math.Round(Capacity.Metall / 100.0 * (100 + Mult.Cap + Mult.Total + GSGameInfo.ScienceMult.Cap + GSGameInfo.ScienceMult.Total), 0);
                case Building2Parameter.ChipCap:
                    if (Planet.PlanetType == PlanetTypes.Freezed)
                        return Math.Round(Capacity.Chips * ServerLinks.Parameters.FreezedCapBonus / 100.0 * (100 + Mult.Cap + Mult.Total + GSGameInfo.ScienceMult.Cap + GSGameInfo.ScienceMult.Total), 0);
                    else
                        return Math.Round(Capacity.Chips / 100.0 * (100 + Mult.Cap + Mult.Total + GSGameInfo.ScienceMult.Cap + GSGameInfo.ScienceMult.Total), 0);
                case Building2Parameter.AntiCap:
                    if (Planet.PlanetType == PlanetTypes.Freezed)
                        return Math.Round(Capacity.Anti * ServerLinks.Parameters.FreezedCapBonus / 100.0 * (100 + Mult.Cap + Mult.Total + GSGameInfo.ScienceMult.Cap + GSGameInfo.ScienceMult.Total), 0);
                    else
                        return Math.Round(Capacity.Anti / 100.0 * (100 + Mult.Cap + Mult.Total + GSGameInfo.ScienceMult.Cap + GSGameInfo.ScienceMult.Total), 0);
                case Building2Parameter.Repair:
                    return Math.Round(Capacity.Repair / 100.0 * (Mult.Repair + Mult.Total + GSGameInfo.ScienceMult.Repair + GSGameInfo.ScienceMult.Total), 0);
                default: throw new Exception();
            }
        }
        public int GetMaxShips()
        {
            int maxShips = 0;
            foreach (FleetBase b in FleetBases)
                maxShips += b.MaxShips;
            return maxShips;
        }
        public int CalcShipsCount(int count)
        {
            int ShipsCount = count;
            foreach (GSFleet fleet in Fleets.Values)
                ShipsCount += fleet.Ships.Count;
            return ShipsCount;
        }
        /// <summary> Метод создаёт панель с краткой информацией о колонии </summary>
        /// <param name="needevents">показывает, нужен ли эвент при клике с открытием колонии</param>
        public Border GetShortInfo(bool needevents, double width)
        {
            LandChanger.PlanetInfo info = new LandChanger.PlanetInfo(0, Planet, ID);
            info.Width = 300; info.Height = 300;
            info.Show();
            Border border = new Border();
            border.Width = width; border.Height = 300;
            border.Child = info;
            /*
            Border border = new Border(); border.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 168, 255));
            border.Width = 300; border.Height = 200;
            border.BorderThickness = new Thickness(2); border.CornerRadius = new CornerRadius(20);
            border.Background = Brushes.Black;
       
            Canvas canvas = new Canvas();
            canvas.Width = 300; canvas.Height = 200;
            border.Child = canvas;

            TextBlock Title = Common.GetBlock(30, Name.ToString(), Brushes.White, 300);
            canvas.Children.Add(Title); Canvas.SetTop(Title, 5);

            for (int i = 0; i < Locations.Length; i++)
            {
                Rectangle rect = Common.GetRectangle(30, SectorImages.List[Locations[i].Type].MainImage);
                canvas.Children.Add(rect); Canvas.SetLeft(rect, 10 + i * 35); Canvas.SetTop(rect, 40);
            }

            Canvas AddCanvas = GetAddResourceCanvas(new Brush[] { Links.Brushes.MoneyImageBrush, Links.Brushes.MetalImageBrush, Links.Brushes.ChipsImageBrush, Links.Brushes.AntiImageBrush }, 
                new Brush[] { Brushes.Green, Brushes.Gray, Brushes.Blue, Brushes.Red }, 
                new int[] { Add.Money, Add.Metall, Add.Chips, Add.Anti }, new double[] { 1000, 100, 100, 100 });
            canvas.Children.Add(AddCanvas);
            Canvas.SetLeft(AddCanvas, 10); Canvas.SetTop(AddCanvas, 90);
            Canvas CapCanvas=GetAddResourceCanvas(new Brush[] { Links.Brushes.MetalCapBrush, Links.Brushes.ChipCapBrush, Links.Brushes.AntiCapBrush, Links.Brushes.RepairPict},
                new Brush[] { Brushes.Gray, Brushes.Blue, Brushes.Red, Brushes.Yellow },
                new int[] { Capacity.Metall, Capacity.Chips, Capacity.Anti, Capacity.Repair }, 
                new double[] { 5000, 5000, 5000, 1000 });
            canvas.Children.Add(CapCanvas);
            Canvas.SetLeft(CapCanvas, 125); Canvas.SetTop(CapCanvas, 90);
            Canvas PeopeCanvas = GetPeopleInfoCanvas(new Brush[] { Links.Brushes.PeopleImageBrush, Links.Brushes.BuildingSizeBrush }, 
                new Brush[] { Brushes.SkyBlue, Brushes.Brown },
                new double[] { Capacity.Peoples, Peoples }, new double[] { Peoples, BuildingsCount },
                new double[] { Planet.MaxPopulation, Capacity.Peoples });
            canvas.Children.Add(PeopeCanvas);
            Canvas.SetLeft(PeopeCanvas, 240); Canvas.SetTop(PeopeCanvas, 90);
            */
            if (needevents)
                border.PreviewMouseDown += Border_PreviewMouseDown;
            border.Tag = this;
            return border;
            
        }

        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Colonies.CurrentPlanet = Planet;
            //Colonies.CurrentLand = this;
            //Colonies.CurrentAvanpost = null;
            Links.Controller.SelectPanel(GamePanels.Colonies, SelectModifiers.None);
        }
        public bool CheckArtefactAvailable(Artefact art)
        {
            switch (art.Ability)
            {
                case ArtefactAbility.PPeopleClone: return true;
                case ArtefactAbility.PSectorRemove: return true;
                case ArtefactAbility.PsectorCreate: if (Locations.Length < art.Param1) return true; else return false;
                default:return false;
            }
        }
        /*
        Canvas GetAddResourceCanvas(Brush[] brushes, Brush[] Colors, int[] values, double[] mults)
        {
            Canvas canvas = new Canvas();
            canvas.Width = 100; canvas.Height = 80;
            Rectangle rect = new Rectangle(); rect.Width = 100; rect.Height = 60; rect.Stroke = Brushes.White; rect.StrokeThickness = 2;
            canvas.Children.Add(rect);
            for (int i = 0; i < brushes.Length; i++)
            {
                Rectangle cub = new Rectangle(); cub.Width = 20; cub.Height = values[i] / mults[i];
                if (cub.Height > 56) cub.Height = 56; else if (cub.Height < 3) cub.Height = 3;
                canvas.Children.Add(cub); Canvas.SetLeft(cub, i*25+2.5); Canvas.SetTop(cub, 58.0 - cub.Height); cub.Fill = Colors[i];
                Rectangle img = Common.GetRectangle(20, brushes[i]); canvas.Children.Add(img); Canvas.SetLeft(img, i*25+5); Canvas.SetTop(img, 61);
            }
            return canvas;
        }
        Canvas GetPeopleInfoCanvas(Brush[] brushes, Brush[] Colors, double[] value1, double[] value2, double[] mult)
        {
            Canvas canvas = new Canvas();
            canvas.Width = 50; canvas.Height = 80;
            Rectangle rect = new Rectangle(); rect.Width = 50; rect.Height = 60; rect.Stroke = Brushes.White; rect.StrokeThickness = 2;
            canvas.Children.Add(rect);
            for (int i=0;i<brushes.Length; i++)
            {
                Rectangle cub1 = new Rectangle(); cub1.Width = 20; cub1.Height = value1[i]*58.0 / mult[i];
                if (cub1.Height > 56) cub1.Height = 56; else if (cub1.Height < 3) cub1.Height = 3;
                cub1.Stroke = Colors[i]; canvas.Children.Add(cub1); Canvas.SetLeft(cub1, i * 25 + 2.5); Canvas.SetTop(cub1, 58.0 - cub1.Height);
                Rectangle cub2 = new Rectangle(); cub2.Width = 20; cub2.Height = value2[i]*58.0 / mult[i];
                cub2.Fill = Colors[i]; canvas.Children.Add(cub2); Canvas.SetLeft(cub2, i * 25 + 2.5); Canvas.SetTop(cub2, 58.0 - cub2.Height);
                Rectangle img = Common.GetRectangle(20, brushes[i]); canvas.Children.Add(img); Canvas.SetLeft(img, i * 25 + 5); Canvas.SetTop(img, 61);
            }
            return canvas;
        }
        */
    }
   
    enum LandInfoType { Free, Normal, NPC, Builded }
    class ShortLandInfo
    {
        public int ID;
        //public int PlanetID;
        public GSString LandName;
        public GSString PlayerName;
        public GSPlanet Planet;
        public Border Border;
        public GSClan Clan;
        public object Tag;
        public ShortLandInfo(int LandID, GSPlanet planet)
        {
            Planet = planet;
            ID = LandID;
            if (!GSGameInfo.PlayerLands.ContainsKey(LandID))
            {
                Border = new Border();
                Border = Common.CreateBorder(200, 100, Brushes.Black, 2, 3);
                Border.Background = Brushes.Black;
                TextBlock tb = Common.GetBlock(20, Links.Interface("NotYourColony"));
                tb.Foreground = Brushes.White;
                Border.Child = tb;
            }
            else
            {
                Border = Common.CreateBorder(200, 100, Brushes.Black, 2, 3);
                Border.Background = Brushes.Black;
                TextBlock tb = Common.GetBlock(20, "Under Construction");
                tb.Foreground = Brushes.White;
                Border.Child = tb;// new LandInfoBorder(GSGameInfo.PlayerLands[LandID], false);

            }
        }
        /// <summary> Метод возвращает панель с краткой информацией о колонии
        /// </summary>
        /// <param name="array">массив с данными, полученными с сервера</param>
        /// <param name="i">позиция в массиве</param>
        /// <param name="needclick">флаг, показывающий, будет ли панель реагировать на нажатие</param>
        public ShortLandInfo(byte[] array, ref int i, bool needclick, GSPlanet planet)
        {
            Planet = planet;
            if (array != null)
            {
                byte mark = array[i]; i++;
                ID = BitConverter.ToInt32(array, i); i += 4;
                LandName = new GSString(array, i); i += LandName.Array.Length;
                if (mark != 5) //5 - значение для колонии нпс
                {
                    PlayerName = new GSString(array, i); i += PlayerName.Array.Length; i += 6;
                    GSClan clan = new GSClan(array, ref i);
                    if (clan.NoClan == false) Clan = clan;
                    if (mark == 2)
                        GetBorder(LandInfoType.Normal);
                    else if (mark == 3)
                        GetBorder(LandInfoType.Builded);
                }
                else
                {
                    byte Mark = array[i]; i++;
                    byte npcLevel = array[i]; i++;
                    Tag = new byte[] { Mark, npcLevel };
                    GetBorder(LandInfoType.NPC);
                }
            }
            else
            {
                ID = -1;
                GetBorder(LandInfoType.Free);
            }
            if (needclick)
            {
                Border.PreviewMouseDown += new MouseButtonEventHandler(Border_PreviewMouseDown);
                
            }
        }

        void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Links.Helper.LandID = ID;
            Links.Helper.Planet = Planet;
            Border border = (Border)sender;
            LandInfoType type = (LandInfoType)border.Tag;
            Links.Helper.LandInfoType = type;
            if (Clan != null) Links.Helper.Clan = Clan; else Links.Helper.Clan = null;
            Links.Helper.ClickDelegate(null, null);
        }
        void GetBorder(LandInfoType type)
        {
            Border = Common.CreateBorder(200, 100, Brushes.Black, 2, 3);
            Border.Background = Brushes.Black;
            Border.Tag = type;
            //Border.Background = Common.GetLinearBrush(Colors.Brown, Colors.SandyBrown, Colors.RosyBrown, new Point(0.5, 0), new Point(0.5, 1));
            Grid grid = new Grid();
            Border.Child = grid;
            if (type == LandInfoType.Free)
            {
                grid.Children.Add(AddLabel(Links.Interface("FreeLand"), 0, 0, 1, 1, 0));
            }
            else
            {
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions[1].Width = new GridLength(2, GridUnitType.Star);
                if (type == LandInfoType.Normal)
                {
                    grid.Children.Add(AddLabel(PlayerName.ToString(), 1, 1, 1, 1, -5));
                    grid.Children.Add(AddLabel(LandName.ToString(), 0, 0, 1, 2, -5));
                    if (Clan != null)
                    {
                        grid.Children.Add(AddLabel(Clan.Name.ToString(), 2, 1, 1, 1, -5));
                        grid.Children.Add(AddEmblem(Clan.Image));
                    }
                }
                else if (type == LandInfoType.Builded)
                {
                    grid.Children.Add(AddLabel(PlayerName.ToString(), 1, 1, 1, 1, -5));
                    grid.Children.Add(AddLabel(LandName.ToString(), 0, 0, 1, 2, -5));
                    if (Clan != null)
                    {
                        grid.Children.Add(AddLabel(Clan.Name.ToString(), 2, 1, 1, 1, -5));
                        grid.Children.Add(AddEmblem(Clan.Image));
                    }
                    grid.Children.Add(AddLabel(Links.Interface("BuildedColony"), 3, 0, 1, 2, -5));
                    grid.RowDefinitions.Add(new RowDefinition());
                }
                else if (type == LandInfoType.NPC)
                {
                    byte mark = ((byte[])Tag)[0];
                    grid.Children.Add(AddNPCEmblem(mark));
                    grid.Children.Add(AddLabel(LandName.ToString(), 0, 0, 1, 2, -5));
                }

            }

        }
        Label AddLabel(string text, int row, int column, int rowspan, int columnspan, int margin)
        {
            Label lbl = Common.CreateLabel(20, text);
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            lbl.VerticalAlignment = VerticalAlignment.Center;
            lbl.Foreground = Brushes.White;
            Grid.SetRow(lbl, row);
            Grid.SetColumn(lbl, column);
            Grid.SetRowSpan(lbl, rowspan);
            Grid.SetColumn(lbl, columnspan);
            if (margin != 0) lbl.Margin = new Thickness(0, margin, 0, margin);
            return lbl;
        }
        Viewbox AddEmblem(long image)
        {
            ClanEmblem emblem = new ClanEmblem(image);
            Viewbox vbx = emblem.GetSmallEmblem(60);
            Grid.SetRow(vbx, 1);
            Grid.SetRowSpan(vbx, 3);
            return vbx;
        }
        FleetEmblem AddNPCEmblem(byte mark)
        {
            FleetEmblem emblem = new FleetEmblem(BitConverter.ToInt32(new byte[] { mark, 0, 0, 0 },0), 60);
            Grid.SetRow(emblem, 0);
            Grid.SetRowSpan(emblem, 3);
            return emblem;
        }
    }
    class ShortLandList : Border
    {
        public ShortLandList(int planetID, bool needclick)
        {
            BorderBrush = Brushes.Black;
            BorderThickness = new Thickness(3);
            GSPlanet planet = Links.Planets[planetID];
            if (planet.LandsCount <= 0)
                return;
            byte[] LandsList = Events.GetPlanetLandsList(planetID);
            if (LandsList == null) return;
            List<ShortLandInfo> list=new List<ShortLandInfo>();
            for (int i = 0; i < LandsList.Length; )
                list.Add(new ShortLandInfo(LandsList, ref i, needclick, planet));
            int z = 0;
            for (; ; )
                if (list.Count < planet.LandsCount) 
                    list.Add(new ShortLandInfo(null, ref z, needclick, planet));
                else
                    break;
            int rows = 0; int columns = 0;
            switch (planet.LandsCount)
            {
                case 1: rows = 1; columns = 1;  break;
                case 2: rows = 1; columns = 2; break;
                case 3: rows = 1; columns = 3; break;
                case 4: rows = 2; columns = 2; break;
                case 5: rows = 2; columns = 3; break;
                case 6: rows = 2; columns = 3; break;
                case 7: rows = 2; columns = 4; break;
                case 8: rows = 2; columns = 4; break;
                case 9: rows = 3; columns = 3; break;
                case 10: rows = 2; columns = 5; break;
                default: rows = 3; columns = 5; break;
            }
            Width = columns * 200 + 10; 
            Height = rows * 100 + 10;
            Grid grid = new Grid();
            Child = grid;
            for (int i = 0; i < rows; i++)
                grid.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < columns; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < list.Count; i++)
            {
                grid.Children.Add(list[i].Border);
                int currow = i / columns;
                int curcolumn = i - currow * columns;
                Grid.SetRow(list[i].Border, currow);
                Grid.SetColumn(list[i].Border, curcolumn);
            }
            Links.Controller.PopUpCanvas.Place(this, true);
        }
    }
    
}
