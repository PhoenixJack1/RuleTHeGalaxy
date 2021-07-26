using System;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public enum Building2Parameter {PeopleGrow, BuildingEffect, MoneyEffect, MoneyAdd, Repair, RepairEffect, MetallAdd, MetallEffect, MetalCap, CapEffect,
    ChipAdd, ChipEffect, ChipCap, AntiAdd, AntiEffect, AntiCap, Range, Ships, MathPower }
    public enum RoundMode {d0_1, d0_5, d1, d10, d2z, d3z, d4z, d5z}
    public enum TagState { NextBuildng, FullBuilding, ResultInfo}
    public class GSBuilding2
    {
        public ushort ID;
        public int LandID;
        public byte BuildingPos;
        public byte SectorPos;
        public SectorTypes SectorType;
        public byte CurLevel;
        public Building2Parameter Parameter;
        public double BaseBonus;
        public RoundMode BonusRoundMode;
        public int BaseSize;
        public ItemPrice BasePrice;
        public double MaxPrice;
        public string BuildingPath = "M{0},{1} h165 l-50,-25 v-195 l-117,-65 l-104,60 v200 l-45,25z";
        public ImageBrush Image = Gets.GetBuildingBrush("Build011");
        public ImageBrush ImageA;
        public string Name;
        public string Items;
        public GSBuilding2(int landid, byte sectorpos, SectorTypes sector, byte buildingpos)
        {
            BuildingPos = buildingpos;
            LandID = landid;
            SectorPos = sectorpos;
            CurLevel = 0;
            switch (sector)
            {
                case SectorTypes.Live:
                    switch (buildingpos)
                    {
                        case 0: ID = 0; Parameter = Building2Parameter.PeopleGrow; BaseBonus = 0.03; BonusRoundMode = RoundMode.d0_1; BaseSize = 2;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 2E8; SectorType = SectorTypes.Live;
                            Image = Links.Brushes.Buildings.BuildLive1;
                            ImageA = Links.Brushes.Buildings.BuildLive1_A;
                            Name = "Лечебный центр";
                            Items = "млн. чел";
                            BuildingPath = "M{0},{1} h140 l-65,-35 l-5,-40 l-32,-10 v-10 l-55,-25  l-40,10 v20 l-10,10 v40 l-65,40z"; break;
                        case 1: ID = 1; Parameter = Building2Parameter.BuildingEffect; BaseBonus = 1.5; BonusRoundMode = RoundMode.d0_5; BaseSize = 2;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 3.5e8; SectorType = SectorTypes.Live;
                            Image = Links.Brushes.Buildings.BuildLive2;
                            ImageA = Links.Brushes.Buildings.BuildLive2_A;
                            Name = "Учебный центр";
                            Items = "%";
                            BuildingPath = "M{0},{1} h145 v-20 l-58,-33 v-40 l-88,-50 l-88,50 v40 l-58,33 v20z";
                            break;
                        case 2: ID = 2; Parameter = Building2Parameter.MoneyEffect; BaseBonus = 2; BonusRoundMode = RoundMode.d0_5; BaseSize = 1;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 1.5e8; SectorType = SectorTypes.Live;
                            Image = Links.Brushes.Buildings.BuildLive3;
                            ImageA = Links.Brushes.Buildings.BuildLive3_A;
                            Items = "%";
                            Name = "Торговый центр";
                            BuildingPath = "M{0},{1} h130 l-40,-20 v-145 l-20,-10 v-25 l-30,-40 l-50,-30 l-3,-25 l-3,28 l-17,10 l-12,15 v60 l-35,20 v30 l-48,25 v80 l-20,15 v15z";
                            break;
                        default: throw new Exception();
                    } break;
                case SectorTypes.Money:
                    switch (buildingpos)
                    {
                        case 0: ID = 10; Parameter = Building2Parameter.MoneyAdd; BaseBonus = 50; BonusRoundMode = RoundMode.d3z; BaseSize = 3;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 5e8; SectorType = SectorTypes.Money;
                            Image = Links.Brushes.Buildings.BuildMoney1;
                            ImageA = Links.Brushes.Buildings.BuildMoney1_A;
                            Items = "";
                            Name = "Выставочный центр";
                            BuildingPath = "M{0},{1} h145 v-20 l-58,-33 v-40 l-88,-50 l-88,50 v40 l-58,33 v20z";
                            break;
                        case 1: ID = 11; Parameter = Building2Parameter.MoneyEffect; BaseBonus = 2; BonusRoundMode = RoundMode.d0_5; BaseSize = 2;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 4e8; SectorType = SectorTypes.Money;
                            Image = Links.Brushes.Buildings.BuildMoney2;
                            ImageA = Links.Brushes.Buildings.BuildMoney2_A;
                            Items = "%";
                            Name = "Банковский центр";
                            BuildingPath = "M{0},{1} h140 l-60,-35 l18,-30 l-20,-85 l-45,-15 h-30 l-70,30 l-3,50 l3,40 l-65,45z";
                            break;
                        case 2: ID = 12; Parameter = Building2Parameter.BuildingEffect; BaseBonus = 1.5; BonusRoundMode = RoundMode.d0_5; BaseSize = 1;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 1.2e8; SectorType = SectorTypes.Money;
                            Image = Links.Brushes.Buildings.BuildMoney3;
                            ImageA = Links.Brushes.Buildings.BuildMoney3_A;
                            Items = "%";
                            Name = "Фондовая биржа";
                            BuildingPath = "M{0},{1} h145 v-20 l-58,-33 v-40 l-88,-50 l-88,50 v40 l-58,33 v20z";
                            break;
                        default: throw new Exception();
                    } break;
                case SectorTypes.Repair:
                    switch (buildingpos)
                    {
                        case 0:
                            ID = 20; Parameter = Building2Parameter.Repair; BaseBonus = 50; BonusRoundMode = RoundMode.d3z; BaseSize = 4;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 6e8; SectorType = SectorTypes.Repair;
                            Image = Links.Brushes.Buildings.BuildRepair1;
                            ImageA = Links.Brushes.Buildings.BuildRepair1_A;
                            Items = "";
                            Name = "Завод запасных частей";
                            BuildingPath = "M{0},{1} h145 v-20 l-58,-33 v-40 l-88,-50 l-88,50 v40 l-58,33 v20z";
                            break;
                        case 1:
                            ID = 21; Parameter = Building2Parameter.RepairEffect; BaseBonus = 2.5; BonusRoundMode = RoundMode.d0_5; BaseSize = 1;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 2.5e8; SectorType = SectorTypes.Repair;
                            Image = Links.Brushes.Buildings.BuildRepair2;
                            ImageA = Links.Brushes.Buildings.BuildRepair2_A;
                            Items = "%";
                            Name = "Испытательный полигон";
                            BuildingPath = "M{0},{1} h130 l-75,-45 v-23 l-7,-15 l-10,-5 v-10 l-90,-25 v65 l-13,8 l-5,-22 l-14,-7 v15 h-5 l-12,35 l-47,30z";
                            break;
                        case 2:
                            ID = 22; Parameter = Building2Parameter.BuildingEffect; BaseBonus = 1; BonusRoundMode = RoundMode.d0_5; BaseSize = 1;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 1e8; SectorType = SectorTypes.Repair;
                            Image = Links.Brushes.Buildings.BuildRepair3;
                            ImageA = Links.Brushes.Buildings.BuildRepair3_A;
                            Items = "%";
                            Name = "Ремонтный ангар";
                            BuildingPath = "M{0},{1} h145 v-20 l-58,-33 v-40 l-88,-50 l-88,50 v40 l-58,33 v20z";
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case SectorTypes.Metall:
                    switch (buildingpos)
                    {
                        case 0:
                            ID = 30; Parameter = Building2Parameter.MetallAdd; BaseBonus = 6; BonusRoundMode = RoundMode.d3z; BaseSize = 4;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 6e8; SectorType = SectorTypes.Metall;
                            Image = Links.Brushes.Buildings.BuildMetal1;
                            ImageA = Links.Brushes.Buildings.BuildMetal1_A;
                            Items = "";
                            Name = "Шахта";
                            BuildingPath = "M{0},{1} h145 v-20 l-58,-33 v-40 l-88,-50 l-88,50 v40 l-58,33 v20z";
                            break;
                        case 1:
                            ID = 31; Parameter = Building2Parameter.MetallEffect; BaseBonus = 2.5; BonusRoundMode = RoundMode.d0_5; BaseSize = 3;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 3e8; SectorType = SectorTypes.Metall;
                            Image = Links.Brushes.Buildings.BuildMetal2;
                            ImageA = Links.Brushes.Buildings.BuildMetal2_A;
                            Items = "%";
                            Name = "Сталеплавильный завод";
                            BuildingPath = "M{0},{1} h145 v-20 l-58,-33 v-40 l-88,-50 l-88,50 v40 l-58,33 v20z";
                            break;
                        case 2:
                            ID = 32; Parameter = Building2Parameter.MetalCap; BaseBonus = 200; BonusRoundMode = RoundMode.d3z; BaseSize = 1;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 1.2e8; SectorType = SectorTypes.Metall;
                            Image = Links.Brushes.Buildings.BuildMetal3;
                            ImageA = Links.Brushes.Buildings.BuildMetal3_A;
                            Items = "";
                            Name = "Отвал руды";
                            BuildingPath = "M{0},{1} h145 v-20 l-58,-33 v-40 l-88,-50 l-88,50 v40 l-58,33 v20z";
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case SectorTypes.MetalCap:
                    switch (buildingpos)
                    {
                        case 0:
                            ID = 40; Parameter = Building2Parameter.MetalCap; BaseBonus = 300; BonusRoundMode = RoundMode.d3z; BaseSize = 3;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 4e8; SectorType = SectorTypes.MetalCap;
                            Image = Links.Brushes.Buildings.BuildMetCap1;
                            ImageA = Links.Brushes.Buildings.BuildMetCap1_A;
                            Items = "";
                            Name = "Площадка для хранения металла";
                            BuildingPath = "M{0},{1} h145 v-20 l-58,-33 v-40 l-88,-50 l-88,50 v40 l-58,33 v20z";
                            break;
                        case 1:
                            ID = 41; Parameter = Building2Parameter.CapEffect; BaseBonus = 1; BonusRoundMode = RoundMode.d0_5; BaseSize = 2;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 2.5e8; SectorType = SectorTypes.MetalCap;
                            Image = Links.Brushes.Buildings.BuildMetCap2;
                            ImageA = Links.Brushes.Buildings.BuildMetCap2_A;
                            Items = "%";
                            Name = "Центр крупногабаритной транспортировки";
                            BuildingPath = "M{0},{1} h145 v-20 l-58,-33 v-40 l-88,-50 l-88,50 v40 l-58,33 v20z";
                            break;
                        case 2:
                            ID = 42; Parameter = Building2Parameter.MetallAdd; BaseBonus = 3; BonusRoundMode = RoundMode.d3z; BaseSize = 1;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 1.5e8; SectorType = SectorTypes.MetalCap;
                            Image = Links.Brushes.Buildings.BuildMetCap3;
                            ImageA = Links.Brushes.Buildings.BuildMetCap3_A;
                            Items = "";
                            Name = "Вторичная переработка металлолома";
                            BuildingPath = "M{0},{1} h145 v-20 l-58,-33 v-40 l-88,-50 l-88,50 v40 l-58,33 v20z";
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case SectorTypes.Chips:
                    switch (buildingpos)
                    {
                        case 0:
                            ID = 50; Parameter = Building2Parameter.ChipAdd; BaseBonus = 4; BonusRoundMode = RoundMode.d3z; BaseSize = 3;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 5e8; SectorType = SectorTypes.Chips;
                            Image = Links.Brushes.Buildings.BuildChip1;
                            ImageA = Links.Brushes.Buildings.BuildChip1_A;
                            Items = "";
                            Name = "Фабрика микроэлектроники";
                            BuildingPath = "M{0},{1}  h146 v-5 l-48,-25 v-170 l-100,-55 l-97,55 v165 l-20,10 v5 l-30,19z";
                            break;
                        case 1:
                            ID = 51; Parameter = Building2Parameter.ChipEffect; BaseBonus = 1.5; BonusRoundMode = RoundMode.d0_5; BaseSize = 2;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 3.5e8; SectorType = SectorTypes.Chips;
                            Image = Links.Brushes.Buildings.BuildChip2;
                            ImageA = Links.Brushes.Buildings.BuildChip2_A;
                            Items = "%";
                            Name = "Конструкторское бюро";
                            BuildingPath = "M{0},{1} h145 v-20 l-58,-33 v-40 l-88,-50 l-88,50 v40 l-58,33 v20z";
                            break;
                        case 2:
                            ID = 52; Parameter = Building2Parameter.ChipCap; BaseBonus = 300; BonusRoundMode = RoundMode.d3z; BaseSize = 1;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 1.4e8; SectorType = SectorTypes.Chips;
                            Image = Links.Brushes.Buildings.BuildChip3;
                            ImageA = Links.Brushes.Buildings.BuildChip3_A;
                            Items = "";
                            Name = "Сервисный центр";
                            BuildingPath = "M{0},{1}  h95 l-10,-7  l26,-16 l-35,-20 v-120 l-25,-13 l-25,-38 l-50,-30 l-25,-45 h-5 v15 l-30,10 l-20,60 v30 l-20,10 v65 l-5,5 v68 l-22,15 v10z";
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case SectorTypes.ChipsCap:
                    switch (buildingpos)
                    {
                        case 0:
                            ID = 60; Parameter = Building2Parameter.ChipCap; BaseBonus = 500; BonusRoundMode = RoundMode.d3z; BaseSize = 3;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 4.5e8; SectorType = SectorTypes.ChipsCap;
                            Image = Links.Brushes.Buildings.BuildChipCap1;
                            ImageA = Links.Brushes.Buildings.BuildChipCap1_A;
                            Items = "";
                            Name = "Корпус хранения микросхем";
                            BuildingPath = "M{0},{1}  h133 l-25,-14  l-7,-20 l-5,-8 l-33,-15 v-34 l-40,-20 h-55 l-35,20 v40 l-80,50z";
                            break;
                        case 1:
                            ID = 61; Parameter = Building2Parameter.CapEffect; BaseBonus = 3; BonusRoundMode = RoundMode.d0_5; BaseSize = 2;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 2.5e8; SectorType = SectorTypes.ChipsCap;
                            Image = Links.Brushes.Buildings.BuildChipCap2;
                            ImageA = Links.Brushes.Buildings.BuildChipCap2_A;
                            Items = "%";
                            Name = "Космодром";
                            BuildingPath = "M{0},{1}  h130 l-40,-20 v-4 l-5,-6 l11,-7 v-45 l-60,-30 l-25,5 v25 l-33,18 l-19,-12 l-46,22 v23 l-45,34z";
                            break;
                        case 2:
                            ID = 62; Parameter = Building2Parameter.ChipAdd; BaseBonus = 1; BonusRoundMode = RoundMode.d3z; BaseSize = 1;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 1.8e8; SectorType = SectorTypes.ChipsCap;
                            Image = Links.Brushes.Buildings.BuildChipCap3;
                            ImageA = Links.Brushes.Buildings.BuildChipCap3_A;
                            Items = "";
                            Name = "Хранилище списанной техники";
                            BuildingPath = "M{0},{1} h145 v-20 l-58,-33 v-40 l-88,-50 l-88,50 v40 l-58,33 v20z";
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case SectorTypes.Anti:
                    switch (buildingpos)
                    {
                        case 0:
                            ID = 70; Parameter = Building2Parameter.AntiAdd; BaseBonus = 5; BonusRoundMode = RoundMode.d3z; BaseSize = 5;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 1e9; SectorType = SectorTypes.Anti;
                            Image = Links.Brushes.Buildings.BuildAnti1;
                            ImageA = Links.Brushes.Buildings.BuildAnti1_A;
                            Items = "";
                            Name = "Ускоритель частиц";
                            BuildingPath = "M{0},{1} h145 v-20 l-58,-33 v-40 l-88,-50 l-88,50 v40 l-58,33 v20z";
                            break;
                        case 1:
                            ID = 71; Parameter = Building2Parameter.AntiEffect; BaseBonus = 2; BonusRoundMode = RoundMode.d0_5; BaseSize = 2;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 4e8; SectorType = SectorTypes.Anti;
                            Image = Links.Brushes.Buildings.BuildAnti2;
                            ImageA = Links.Brushes.Buildings.BuildAnti2_A;
                            Items = "%";
                            Name = "Научная лаборатория";
                            BuildingPath = "M{0},{1} h122 l-13,-7 v-48 l-15,-8 l-35,-5 v-25 h-35 l-10,-8 h-25 l-22,-14 l-22,-3 l-75,43 v62 l-20,13z";
                            break;
                        case 2:
                            ID = 72; Parameter = Building2Parameter.AntiCap; BaseBonus = 250; BonusRoundMode = RoundMode.d3z; BaseSize = 1;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 1.7e8; SectorType = SectorTypes.Anti;
                            Image = Links.Brushes.Buildings.BuildAnti3;
                            ImageA = Links.Brushes.Buildings.BuildAnti3_A;
                            Items = "";
                            Name = "Хранилище отработанного топлива";
                            BuildingPath = "M{0},{1} h145 v-20 l-58,-33 v-40 l-88,-50 l-88,50 v40 l-58,33 v20z";
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case SectorTypes.AntiCap:
                    switch (buildingpos)
                    {
                        case 0:
                            ID = 80; Parameter = Building2Parameter.AntiCap; BaseBonus = 400; BonusRoundMode = RoundMode.d3z; BaseSize = 3;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 8e8; SectorType = SectorTypes.AntiCap;
                            Image = Links.Brushes.Buildings.BuildAntiCap1;
                            ImageA = Links.Brushes.Buildings.BuildAntiCap1_A;
                            Items = "";
                            Name = "Центр накопления антиматерии";
                            BuildingPath = "M{0},{1} h145 v-20 l-58,-33 v-40 l-88,-50 l-88,50 v40 l-58,33 v20z";
                            break;
                        case 1:
                            ID = 81; Parameter = Building2Parameter.CapEffect; BaseBonus = 1.5; BonusRoundMode = RoundMode.d0_5; BaseSize = 2;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 2.5e8; SectorType = SectorTypes.AntiCap;
                            Image = Links.Brushes.Buildings.BuildAntiCap2;
                            ImageA = Links.Brushes.Buildings.BuildAntiCap2_A;
                            Items = "%";
                            Name = "Энергостанция";
                            BuildingPath = "M{0},{1} h135 l4,-3 l-46,-25 l-5,-12 l-25,-14   l-5,-10 l8,-14 a20,30 -15 0,0 -30,-38 l-12,-5 v-68 l-15,4 l-10,50 v50 l-40,18 v25 l-50,30 v7 l-10,7z";
                            break;
                        case 2:
                            ID = 82; Parameter = Building2Parameter.AntiAdd; BaseBonus = 2; BonusRoundMode = RoundMode.d3z; BaseSize = 1;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 2e8; SectorType = SectorTypes.AntiCap;
                            Image = Links.Brushes.Buildings.BuildAntiCap3;
                            ImageA = Links.Brushes.Buildings.BuildAntiCap3_A;
                            Items = "";
                            Name = "Термоядерный реактор";
                            BuildingPath = "M{0},{1} h120 l-45,-26 v-15 l-13,-6 v-60 l-10,-9 v-15  l-10,-5 v23 h-20 a50,50 0 0,0 -70,35 l-15,28 l-87,50z";
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case SectorTypes.War:
                    switch (buildingpos)
                    {
                        case 0:
                            ID = 90; Parameter = Building2Parameter.Range; BaseBonus = 1; BonusRoundMode = RoundMode.d3z; BaseSize = 2;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 7e8; SectorType = SectorTypes.War;
                            Image = Links.Brushes.Buildings.BuildWar0;
                            ImageA = Links.Brushes.Buildings.BuildWar0_A;
                            Items = "";
                            Name = "Радар";
                            BuildingPath = "M{0},{1} h52 a40,65 0 0,0 -20,-80 a95,50 35 0,0 -128,-20 l-5,55 h-15 l-20,10 v25 l-15,10 z";
                            break;
                        case 1:
                            ID = 91; Parameter = Building2Parameter.Ships; BaseBonus = 1; BonusRoundMode = RoundMode.d3z; BaseSize = 3;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 5e8; SectorType = SectorTypes.War;
                            Image = Links.Brushes.Buildings.BuildWar1;
                            ImageA = Links.Brushes.Buildings.BuildWar1_A;
                            Items = "";
                            Name = "Ангар";
                            BuildingPath = "M{0},{1} h146 l-20,-40 v-55 l-5,-25 l-10,-10 h-20 l-10,15 v10 l-20,50 l-20,-35 v-55 l-5,-25  l-10,-10 h-20 l-10,15 v10 l-20,60 l-12,5 l-16,-45 l-15,-5 l-17,5 l-10,30 v70 l-18,35z";
                            break;
                        case 2:
                            ID = 92; Parameter = Building2Parameter.MathPower; BaseBonus = 10; BonusRoundMode = RoundMode.d3z; BaseSize = 2;
                            BasePrice = new ItemPrice(4, 4, 5, 2); MaxPrice = 4e8; SectorType = SectorTypes.War;
                            Image = Links.Brushes.Buildings.BuildWar2;
                            ImageA = Links.Brushes.Buildings.BuildWar2_A;
                            Items = "";
                            Name = "Дата центр";
                            BuildingPath = "M{0},{1} h145 v-20 l-58,-33 v-40 l-88,-50 l-88,50 v40 l-58,33 v20z";
                            break;
                        default:
                            throw new Exception();
                    }
                    break;

            }
        }
        public string GetTagText(TagState state)
        {
            switch (Parameter)
            {
                case Building2Parameter.AntiAdd: if (state == TagState.NextBuildng) return "Прирост к базовому производству антиматерии";
                    else if (state == TagState.FullBuilding) return "Базовое производство антиматерии";
                    else return "Итоговое произовдство антиматерии в этой постройке";
                case Building2Parameter.AntiCap:
                    if (state == TagState.NextBuildng) return "Прирост к базовому хранению антиматерии";
                    else if (state == TagState.FullBuilding) return "Базовое хранение антиматерии";
                    else return "Итоговое хранение антиматерии в этой постройке";
                case Building2Parameter.AntiEffect:
                    if (state == TagState.NextBuildng) return "Прирост к эффективности добычи антиматерии";
                    else if (state == TagState.FullBuilding) return "Общий бонус постройки к добыче антиматерии";
                    else return "Итоговый бонус к добыче антиматерии колонии";
                case Building2Parameter.BuildingEffect:
                    if (state == TagState.NextBuildng) return "Прирост к общей эффективности построек колонии";
                    else if (state == TagState.FullBuilding) return "Общий бонус постройки к эффективности всех зданий";
                    else return "Итоговый бонус к эффективности всех зданий колонии";
                case Building2Parameter.CapEffect:
                    if (state == TagState.NextBuildng) return "Прирост к эффективности всех складских зданий";
                    else if (state == TagState.FullBuilding) return "Общий бонус постройки к складским зданиям";
                    else return "Итоговый бонус к складским зданиям колонии";
                case Building2Parameter.ChipAdd:
                    if (state == TagState.NextBuildng) return "Прирост к базовому производству микросхем";
                    else if (state == TagState.FullBuilding) return "Базовое производство микросхем";
                    else return "Итоговое произовдство микросхем в этой постройке";
                case Building2Parameter.ChipCap:
                    if (state == TagState.NextBuildng) return "Прирост к базовому хранению микросхем";
                    else if (state == TagState.FullBuilding) return "Базовое хранение микросхем";
                    else return "Итоговое хранение микросхем в этой постройке";
                case Building2Parameter.ChipEffect:
                    if (state == TagState.NextBuildng) return "Прирост к эффективности производства микросхем";
                    else if (state == TagState.FullBuilding) return "Общий бонус постройки к производству микросхем";
                    else return "Итоговый бонус к производству микросхем колонии";
                case Building2Parameter.MathPower:
                    if (state == TagState.NextBuildng) return "Прирост к базовой вычислительной мощи";
                    else if (state == TagState.FullBuilding) return "Базовая вычислительная мощь военной базы";
                    else return "Итоговая вычислительная мощь военной базы";
                case Building2Parameter.MetalCap:
                    if (state == TagState.NextBuildng) return "Прирост к базовому хранению металла";
                    else if (state == TagState.FullBuilding) return "Базовое хранение металла";
                    else return "Итоговое хранение металла в этой постройке";
                case Building2Parameter.MetallAdd:
                    if (state == TagState.NextBuildng) return "Прирост к базовому производству металла";
                    else if (state == TagState.FullBuilding) return "Базовое производство металла";
                    else return "Итоговое произовдство металла в этой постройке";
                case Building2Parameter.MetallEffect:
                    if (state == TagState.NextBuildng) return "Прирост к эффективности добычи металла";
                    else if (state == TagState.FullBuilding) return "Общий бонус постройки к добыче металла";
                    else return "Итоговый бонус к добыче металла колонии";
                case Building2Parameter.MoneyAdd:
                    if (state == TagState.NextBuildng) return "Прирост к базовому производству денег";
                    else if (state == TagState.FullBuilding) return "Базовое производство денег";
                    else return "Итоговое произовдство денег в этой постройке";
                case Building2Parameter.MoneyEffect:
                    if (state == TagState.NextBuildng) return "Прирост к эффективности получения денег";
                    else if (state == TagState.FullBuilding) return "Общий бонус постройки к получению денег";
                    else return "Итоговый бонус к получению денег колонии";
                case Building2Parameter.PeopleGrow:
                    if (state == TagState.NextBuildng) return "Прирост к базовому приросту населения";
                    else if (state == TagState.FullBuilding) return "Базовый прирост населения";
                    else return "Итоговый прирост населения в колонии";
                case Building2Parameter.Range:
                    if (state == TagState.NextBuildng) return "Прирост к дальности действия флота военной базы";
                    else if (state == TagState.FullBuilding) return "Базовая дальность действия флота военной базы";
                    else return "Итоговая дальность действия флота военной базы";
                case Building2Parameter.Repair:
                    if (state == TagState.NextBuildng) return "Прирост к базовой эффективности ремонта";
                    else if (state == TagState.FullBuilding) return "Базовая эффективность ремонта";
                    else return "Итоговая эффективность ремонта в этой постройке";
                case Building2Parameter.RepairEffect:
                    if (state == TagState.NextBuildng) return "Прирост к эффективности ремонта";
                    else if (state == TagState.FullBuilding) return "Общий бонус постройки к эффективности ремонта";
                    else return "Итоговый бонус к эффективности ремонта колонии";
                case Building2Parameter.Ships:
                    if (state == TagState.NextBuildng) return "Прирост к максимальному количеству кораблей флота военной базы";
                    else if (state == TagState.FullBuilding) return "Базовое максимальное количество кораблей флота военной базы";
                    else return "Итоговое максимальное количество корблей флота военной базы";
                default: return "Error";
            }
        }
        public string GetName()
        {
            if (CurLevel == 0) return Name;
            else return Name + " " + NameCreator.GetRoman(CurLevel);
        }
        /// <summary> возвращает сколько места будет занимать следующий уровень постройки</summary>
        public int GetNextSize()
        {

            return (CurLevel + 1) * BaseSize;
        }
        public int GetCurSize()
        {
            return CurLevel * BaseSize;
        }
        /// <summary> возвращает сколько места занимают все текущие уровни постройки</summary>
        public int GetTotalSize()
        {
            return (int)((BaseSize + CurLevel * BaseSize) / 2.0 * CurLevel);
        }
        public double GetCurValueRadar(byte level)
        {
            if (level == 0) return 10;
            else if (level <= 5) return 2;
            else return 1;
        }
        public double GetCurValueHangar(byte level)
        {
            if (level == 0) return 3;
            else if (level <= 2) return 2;
            else return 1;
        }
        /// <summary> возвращает бонус последнего построенного уровня постройки</summary>
        public double GetCurValue()
        {
            switch (Parameter)
            {
                case Building2Parameter.Range: return GetCurValueRadar(CurLevel);
                case Building2Parameter.Ships: return GetCurValueHangar(CurLevel);
                default: if (CurLevel == 0) return RoundValue(BaseBonus, BonusRoundMode);
                    return RoundValue(BaseSize * BaseBonus * Math.Pow(CurLevel, 2), BonusRoundMode);
            }
        }
        /// <summary> возвращает бонус следующего уровня постройки</summary>
        public double GetNextValue()
        {
            switch (Parameter)
            {
                case Building2Parameter.Range: return GetCurValueRadar((byte)(CurLevel+1));
                case Building2Parameter.Ships: return GetCurValueHangar((byte)(CurLevel+1));
                default: return RoundValue(BaseSize * BaseBonus * Math.Pow(CurLevel + 1, 2), BonusRoundMode);
            }
        }
        public double GetTotalValueRadar()
        {
            double result = 0;
            for (byte i = 0; i <= CurLevel; i++)
                result += GetCurValueRadar(i);
            return result;
        }
        public double GetTotalValueHangar()
        {
            double result = 0;
            for (byte i = 0; i <= CurLevel; i++)
                result += GetCurValueHangar(i);
            return result;
        }
        /// <summary> возвращает бонус всех построенных уровней постройки</summary>
        public double GetTotalValue()
        {
            switch (Parameter)
            {
                case Building2Parameter.Range: return GetTotalValueRadar();
                case Building2Parameter.Ships: return GetTotalValueHangar();
                default:
                    if (CurLevel == 0) return RoundValue(BaseBonus, BonusRoundMode);
                    int sum = 1;
                    for (int i = 1; i <= CurLevel; i++)
                        sum = sum + BaseSize * i * i;
                    return RoundValue(BaseBonus * sum, BonusRoundMode);
            }
        }
        /// <summary> возвращает эффект здания с учётом всех бонусов колонии </summary>
        public double GetFullResultValue(Land land)
        {
            double value = 0;
            //double PlanetGrowBonus = land.Planet.PlanetType == PlanetTypes.Green ? ServerLinks.Parameters.GreenPlanetGrowBonus : 1.0;
            double PlanetMetalAddBonus = land.Planet.PlanetType == PlanetTypes.Burned ? ServerLinks.Parameters.BurnPlanetMetallAddBonus : 1.0;
            double PlanetAntiAddBonus = land.Planet.PlanetType == PlanetTypes.Gas ? ServerLinks.Parameters.GasPlanetAntiBonus : 1.0;
            double PlanetCapacityBonus = land.Planet.PlanetType == PlanetTypes.Freezed ? ServerLinks.Parameters.FreezedCapBonus : 1.0;
            switch (Parameter)
            {
                case Building2Parameter.AntiAdd: value = GetTotalValue() / 100.0 *PlanetAntiAddBonus* (100 + land.Mult.Anti + land.Mult.Total); break;
                case Building2Parameter.AntiCap: value = GetTotalValue() / 100.0 *PlanetCapacityBonus* (100 + land.Mult.Cap + land.Mult.Total); break;
                case Building2Parameter.AntiEffect: return land.Mult.Anti; 
                case Building2Parameter.BuildingEffect: return land.Mult.Total; 
                case Building2Parameter.CapEffect: return land.Mult.Cap;
                case Building2Parameter.ChipAdd: value = GetTotalValue() / 100.0 * (100 + land.Mult.Chips + land.Mult.Total); break;
                case Building2Parameter.ChipCap: value = GetTotalValue() / 100.0 *PlanetCapacityBonus* (100 + land.Mult.Cap + land.Mult.Total); break;
                case Building2Parameter.ChipEffect: return land.Mult.Chips; 
                case Building2Parameter.MathPower: value = GetTotalValue() / 100.0 * (100 + land.Mult.Total); break;
                case Building2Parameter.MetalCap: value = GetTotalValue() / 100.0 *PlanetCapacityBonus* (100 + land.Mult.Cap + land.Mult.Total); break;
                case Building2Parameter.MetallAdd: value = GetTotalValue() / 100.0 *PlanetMetalAddBonus* (100 + land.Mult.Metall + land.Mult.Total); break;
                case Building2Parameter.MetallEffect: return land.Mult.Metall;
                case Building2Parameter.MoneyAdd: value = GetTotalValue() / 100.0 * (100 + land.Mult.Money + land.Mult.Total); break;
                case Building2Parameter.MoneyEffect: return land.Mult.Money;
                case Building2Parameter.PeopleGrow: value = GetTotalValue() / 100.0 * (100 + land.Mult.Total); break;
                case Building2Parameter.Range: return GetTotalValue();
                case Building2Parameter.Repair: value = GetTotalValue() / 100.0 * (100 + land.Mult.Repair + land.Mult.Total); break;
                case Building2Parameter.RepairEffect: return land.Mult.Repair;
                case Building2Parameter.Ships: return GetTotalValue();
            }
            return RoundValue(value, BonusRoundMode);
        }
        public ItemPrice GetNextPrice()
        {
            double maxeffect= RoundValue(BaseBonus * 10, BonusRoundMode);
            double k = Math.Pow(MaxPrice / 15, 1.0 / maxeffect);
            double nexteffect = RoundValue(BaseBonus * (CurLevel + 1), BonusRoundMode);
            double a = Math.Pow(k, nexteffect);
            double money = BasePrice.Money * 10 * a;
            money = RoundValue(money, RoundMode.d2z);
            double metal = RoundValue(BasePrice.Metall * a, RoundMode.d2z);
            double chips = RoundValue(BasePrice.Chips * a, RoundMode.d2z);
            double anti = RoundValue(BasePrice.Anti * a, RoundMode.d2z);
            return new ItemPrice((int)money, (int)metal, (int)chips, (int)anti);
        }
        public double RoundValue(double value, RoundMode mode)
        {
            int p = 0;
            switch (mode)
            {
                case RoundMode.d0_1: return Math.Round(value, 1);
                case RoundMode.d0_5: return Math.Round(value * 2, 0) / 2;
                case RoundMode.d1: return Math.Round(value, 0);
                case RoundMode.d10: return Math.Round(value / 10, 0) * 10;
                case RoundMode.d2z:p = 1; value = Math.Round(value, 0); break;
                case RoundMode.d3z: p = 2; value = Math.Round(value, 0); break;
                case RoundMode.d4z: p = 3; value = Math.Round(value, 0); break;
                case RoundMode.d5z: p = 4; value = Math.Round(value, 0); break; 
            }
            int a = (int)Math.Log10(value);
            double b = Math.Pow(10, a - p);
            return Math.Round(value / b, 0) * b;
        }
    }
}
