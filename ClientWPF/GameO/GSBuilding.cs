using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Media;

namespace Client
{
    public enum BuildingType { LiveS, LiveM, LiveL,
        MoneyS, MoneyM, MoneyL,
        MetalS, MetalM, MetalL,
        MetalCapS, MetalCapM, MetalCapL,
        ChipS, ChipM, ChipL,
        ChipCapS, ChipCapM, ChipCapL,
        AntiS, AntiM, AntiL,
        AntiCapS, AntiCapM, AntiCapL,
        RepairS, RepairM, RepairL,
        Portal, Radar, Education, DataCenter}
    public class GSBuilding:IComparable
    {
        public ushort ID;
        public int Size;
        public string Name;
        public ItemPrice Price;
        public byte Level;
        public BuildingParam[] Params;
        public SectorTypes Sector;
        public BuildingType Type;
        public static SortedList<BuildingType, SortedList<byte, GSBuilding>> AllBuildings;
        public static SortedList<ushort, GSBuilding> GetList(byte[] array)
        {
            SortedList<BuildingType, byte[]> Levels = new SortedList<BuildingType, byte[]>();
            Levels.Add(BuildingType.LiveS, new byte[] { 0, 5,10,15,20,25,30,35,40,45});
            Levels.Add(BuildingType.LiveM, new byte[] { 1,6,11,16,21,26,31,36,41,46 });
            Levels.Add(BuildingType.LiveL, new byte[] { 4,9,14,19,24,29,34,39,44,49 });
            Levels.Add(BuildingType.MoneyS, new byte[] { 0, 5,10,15,20,25,30,35,40,45});
            Levels.Add(BuildingType.MoneyM, new byte[] { 2,7,12,17,22,27,32,37,42,47 });
            Levels.Add(BuildingType.MoneyL, new byte[] { 3,8,13,18,23,28,33,38,43,48 });
            Levels.Add(BuildingType.MetalS, new byte[] { 0,5,10,15,20,25,30,35,40,45 });
            Levels.Add(BuildingType.MetalM, new byte[] { 2,7,12,17,22,27,32,37,42,47 });
            Levels.Add(BuildingType.MetalL, new byte[] { 4,9,14,19,24,29,34,39,44,49 });
            Levels.Add(BuildingType.MetalCapS, new byte[] { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45 });
            Levels.Add(BuildingType.MetalCapM, new byte[] { 1, 6, 11, 16, 21, 26, 31, 36, 41, 46 });
            Levels.Add(BuildingType.MetalCapL, new byte[] { 4, 9, 14, 19, 24, 29, 34, 39, 44, 49 });
            Levels.Add(BuildingType.ChipS, new byte[] { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45 });
            Levels.Add(BuildingType.ChipM, new byte[] { 2, 7, 12, 17, 22, 27, 32, 37, 42, 47 });
            Levels.Add(BuildingType.ChipL, new byte[] { 3, 8, 13, 18, 23, 28, 33, 38, 43, 48 });
            Levels.Add(BuildingType.ChipCapS, new byte[] { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45 });
            Levels.Add(BuildingType.ChipCapM, new byte[] { 1, 6, 11, 16, 21, 26, 31, 36, 41, 46 });
            Levels.Add(BuildingType.ChipCapL, new byte[] { 4, 9, 14, 19, 24, 29, 34, 39, 44, 49 });
            Levels.Add(BuildingType.AntiS, new byte[] { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45 });
            Levels.Add(BuildingType.AntiM, new byte[] { 1, 6, 11, 16, 21, 26, 31, 36, 41, 46 });
            Levels.Add(BuildingType.AntiL, new byte[] { 4, 9, 14, 19, 24, 29, 34, 39, 44, 49 });
            Levels.Add(BuildingType.AntiCapS, new byte[] { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45 });
            Levels.Add(BuildingType.AntiCapM, new byte[] { 2, 7, 12, 17, 22, 27, 32, 37, 42, 47 });
            Levels.Add(BuildingType.AntiCapL, new byte[] { 3, 8, 13, 18, 23, 28, 33, 38, 43, 48 });
            Levels.Add(BuildingType.RepairS, new byte[] { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45 });
            Levels.Add(BuildingType.RepairM, new byte[] { 2, 7, 12, 17, 22, 27, 32, 37, 42, 47 });
            Levels.Add(BuildingType.RepairL, new byte[] { 3, 8, 13, 18, 23, 28, 33, 38, 43, 48 });
            Levels.Add(BuildingType.Portal, new byte[] { 0, 10, 20, 30, 40, 50 });
            Levels.Add(BuildingType.Radar, new byte[] { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45 });
            Levels.Add(BuildingType.DataCenter, new byte[] { 2, 7, 12, 17, 22, 27, 32, 37, 42, 47 });
            Levels.Add(BuildingType.Education, new byte[] { 4, 9, 14, 19, 24, 29, 34, 39, 44, 49 });
            AllBuildings = new SortedList<BuildingType, SortedList<byte, GSBuilding>>();
            foreach (BuildingType type in Levels.Keys)
                AllBuildings.Add(type, new SortedList<byte, GSBuilding>());
            SortedList<ushort, GSBuilding> list = new SortedList<ushort, GSBuilding>();
            foreach (KeyValuePair<BuildingType, byte[]> pair in Levels)
            {
                for (int i=0;i<pair.Value.Length; i++)
                {
                    byte b = pair.Value[i];
                    switch (pair.Key)
                    {
                        case BuildingType.LiveS:
                            list.Add((ushort)(b * 1000 + 10), GetBuilding2((ushort)(b * 1000 + 10), "Жилой блок " + (i + 1).ToString(),
                            EBuildingParam.People, (int)(3 * (1 + b / 5.0)), EBuildingParam.Grow, (int)(100 * (1 + b / 5.0)), BuildingType.LiveS, SectorTypes.Live, 1,
                            new double[] { 0.7, 0.2, 0.1, 0}, 1));
                            break;
                        case BuildingType.LiveM:
                            list.Add((ushort)(b * 1000 + 11), GetBuilding1((ushort)(b * 1000 + 11), "Больничный комплекс " + (i + 1).ToString(),
                            EBuildingParam.Grow, (int)(400 * (1 + b / 5.0)), BuildingType.LiveM, SectorTypes.Live, 2,
                            new double[] { 0.5, 0.3, 0.2, 0 },4));
                            break;
                        case BuildingType.LiveL:
                            list.Add((ushort)(b * 1000 + 12), GetBuilding2((ushort)(b * 1000 + 12), "Жилой массив " + (i + 1).ToString(),
                            EBuildingParam.People, (int)(30 * (1 + b / 5.0)), EBuildingParam.Grow, (int)(300 * (1 + b / 5.0)), BuildingType.LiveL, SectorTypes.Live, 4,
                            new double[] { 0.3, 0.5, 0.1, 0.1 }, 25));
                            break;

                        case BuildingType.MoneyS:
                            list.Add((ushort)(b * 1000 + 20), GetBuilding2((ushort)(b * 1000 + 20), "Торговый павильон " + (i + 1).ToString(),
                            EBuildingParam.MoneyAdd, (int)(100 * (1 + b / 5.0)), EBuildingParam.MoneyMult, 1, BuildingType.MoneyS, SectorTypes.Money, 1,
                            new double[] { 0.2, 0.3, 0.3, 0.2 }, 1));
                            break;
                        case BuildingType.MoneyM:
                            list.Add((ushort)(b * 1000 + 21), GetBuilding1((ushort)(b * 1000 + 21), "Банк " + (i + 1).ToString(),
                            EBuildingParam.MoneyMult, (int)(6  + b / 5.0), BuildingType.MoneyM, SectorTypes.Money, 3,
                            new double[] { 0.1, 0.4, 0.4, 0.1 },8));
                            break;
                        case BuildingType.MoneyL:
                            list.Add((ushort)(b * 1000 + 22), GetBuilding2((ushort)(b * 1000 + 22), "Торговый центр " + (i + 1).ToString(),
                            EBuildingParam.MoneyAdd, (int)(800 * (1 + b / 5.0)), EBuildingParam.MoneyMult, 4, BuildingType.MoneyL, SectorTypes.Money, 4,
                            new double[] { 0.1, 0.6, 0.2, 0.1 }, 25));
                            break;

                        case BuildingType.MetalS:
                            list.Add((ushort)(b * 1000 + 30), GetBuilding2((ushort)(b * 1000 + 30), "Малая выработка " + (i + 1).ToString(),
                            EBuildingParam.MetallAdd, (int)(10 * (1 + b / 5.0)), EBuildingParam.MetalMult, 1, BuildingType.MetalS, SectorTypes.Metall, 1,
                            new double[] { 0.5, 0.1, 0.1, 0.3 }, 1));
                            break;
                        case BuildingType.MetalM:
                            list.Add((ushort)(b * 1000 + 31), GetBuilding2((ushort)(b * 1000 + 31), "Сталеплавильный завод " + (i + 1).ToString(),
                            EBuildingParam.MetalMult, (int)(3+b/5.0), EBuildingParam.MetalCap, (int)(1000 * (1 + b / 5.0)), BuildingType.MetalM, SectorTypes.Metall, 2,
                            new double[] { 0.4, 0.1, 0.4, 0.1 }, 5));
                            break;
                        case BuildingType.MetalL:
                            list.Add((ushort)(b * 1000 + 32), GetBuilding2((ushort)(b * 1000 + 32), "Большая шахта " + (i + 1).ToString(),
                            EBuildingParam.MetallAdd, (int)(110 * (1 + b / 5.0)), EBuildingParam.MetalMult, 2, BuildingType.MetalL, SectorTypes.Metall, 4,
                            new double[] { 0.2, 0.3, 0.1, 0.4 }, 24));
                            break;

                        case BuildingType.MetalCapS:
                            list.Add((ushort)(b * 1000 + 40), GetBuilding2((ushort)(b * 1000 + 40), "Склад металла " + (i + 1).ToString(),
                            EBuildingParam.MetalCap, (int)(1000 * (1 + b / 5.0)), EBuildingParam.MetalCapMult, 1 , BuildingType.MetalCapS, SectorTypes.MetalCap, 1,
                            new double[] { 0.6, 0, 0.2, 0.2 }, 1));
                            break;
                        case BuildingType.MetalCapM:
                            list.Add((ushort)(b * 1000 + 41), GetBuilding2((ushort)(b * 1000 + 41), "Переработка металлолома " + (i + 1).ToString(),
                            EBuildingParam.MetallAdd, (int)(10 * (1 + b / 5.0)), EBuildingParam.MetalCap, (int)(1000 * (1 + b / 5.0)), BuildingType.MetalCapM, SectorTypes.MetalCap, 2,
                            new double[] { 0.4, 0.1, 0.2, 0.3 },2));
                            break;
                        case BuildingType.MetalCapL:
                            list.Add((ushort)(b * 1000 + 42), GetBuilding2((ushort)(b * 1000 + 42), "Центр транспортировки металла " + (i + 1).ToString(),
                            EBuildingParam.MetalCap, (int)(14000 * (1 + b / 5.0)), EBuildingParam.MetalCapMult, 2 , BuildingType.MetalCapL, SectorTypes.MetalCap, 5,
                            new double[] { 0.4, 0.3, 0.15, 0.15 }, 32));
                            break;

                        case BuildingType.ChipS:
                            list.Add((ushort)(b * 1000 + 50), GetBuilding2((ushort)(b * 1000 + 50), "Завод микросхем " + (i + 1).ToString(),
                            EBuildingParam.ChipsAdd, (int)(10 * (1 + b / 5.0)), EBuildingParam.ChipMult, 1, BuildingType.ChipS, SectorTypes.Chips, 1,
                            new double[] { 0.4, 0.2, 0.1, 0.3 }, 1));
                            break;
                        case BuildingType.ChipM:
                            list.Add((ushort)(b * 1000 + 51), GetBuilding2((ushort)(b * 1000 + 51), "Центр разработки " + (i + 1).ToString(),
                            EBuildingParam.ChipMult, (int)(2 + b / 5.0), EBuildingParam.ChipCap, (int)(3000 * (1 + b / 5.0)), BuildingType.ChipM, SectorTypes.Chips, 3,
                            new double[] { 0.3, 0.4, 0.2, 0.1 }, 6));
                            break;
                        case BuildingType.ChipL:
                            list.Add((ushort)(b * 1000 + 52), GetBuilding2((ushort)(b * 1000 + 52), "Корпорация микроэлектроники " + (i + 1).ToString(),
                            EBuildingParam.ChipsAdd, (int)(80 * (1 + b / 5.0)), EBuildingParam.ChipMult, 1, BuildingType.ChipL, SectorTypes.Chips, 5,
                            new double[] { 0.2, 0.5, 0.1, 0.2 }, 30));
                            break;

                        case BuildingType.ChipCapS:
                            list.Add((ushort)(b * 1000 + 60), GetBuilding2((ushort)(b * 1000 + 60), "Склад микросхем " + (i + 1).ToString(),
                            EBuildingParam.ChipCap, (int)(1000 * (1 + b / 5.0)), EBuildingParam.ChipCapMult, 1 , BuildingType.ChipCapS, SectorTypes.ChipsCap, 1,
                            new double[] { 0.5, 0.2, 0, 0.3 }, 1));
                            break;
                        case BuildingType.ChipCapM:
                            list.Add((ushort)(b * 1000 + 61), GetBuilding2((ushort)(b * 1000 + 61), "Фабрика утилизация техники " + (i + 1).ToString(),
                            EBuildingParam.ChipsAdd, (int)(10 * (1 + b / 5.0)), EBuildingParam.ChipCap, (int)(3000 * (1 + b / 5.0)), BuildingType.ChipCapM, SectorTypes.ChipsCap, 3,
                            new double[] { 0.3, 0.3, 0.2, 0.2 }, 4));
                            break;
                        case BuildingType.ChipCapL:
                            list.Add((ushort)(b * 1000 + 62), GetBuilding2((ushort)(b * 1000 + 62), "Центр транспортировки микросхем " + (i + 1).ToString(),
                            EBuildingParam.ChipCap, (int)(7000 * (1 + b / 5.0)), EBuildingParam.ChipCapMult, 5, BuildingType.ChipCapL, SectorTypes.ChipsCap, 4,
                            new double[] { 0.2, 0.4, 0.2, 0.2 }, 30));
                            break;

                        case BuildingType.AntiS:
                            list.Add((ushort)(b * 1000 + 70), GetBuilding2((ushort)(b * 1000 + 70), "Электростанция " + (i + 1).ToString(),
                            EBuildingParam.AntiAdd, (int)(10 * (1 + b / 5.0)), EBuildingParam.AntiMult, 1, BuildingType.AntiS, SectorTypes.Anti, 1,
                            new double[] { 0.6, 0.2, 0.2, 0 }, 1));
                            break;
                        case BuildingType.AntiM:
                            list.Add((ushort)(b * 1000 + 71), GetBuilding2((ushort)(b * 1000 + 71), "Научная лаборатория " + (i + 1).ToString(),
                            EBuildingParam.AntiMult, (int)(3  + b / 5.0), EBuildingParam.AntiCap, (int)(2000 * (1 + b / 5.0)), BuildingType.AntiM, SectorTypes.Anti, 3,
                            new double[] { 0.4, 0.3, 0.2, 0.1 }, 5));
                            break;
                        case BuildingType.AntiL:
                            list.Add((ushort)(b * 1000 + 72), GetBuilding2((ushort)(b * 1000 + 72), "Ускоритель частиц " + (i + 1).ToString(),
                            EBuildingParam.AntiAdd, (int)(100 * (1 + b / 5.0)), EBuildingParam.AntiMult, 5, BuildingType.AntiL, SectorTypes.Anti, 5,
                            new double[] { 0.1, 0.6, 0.2, 0.1 }, 31));
                            break;

                        case BuildingType.AntiCapS:
                            list.Add((ushort)(b * 1000 + 80), GetBuilding2((ushort)(b * 1000 + 80), "Склад антиматерии " + (i + 1).ToString(),
                            EBuildingParam.AntiCap, (int)(1000 * (1 + b / 5.0)), EBuildingParam.AntiCapMult, 1, BuildingType.AntiCapS, SectorTypes.AntiCap, 1, 
                            new double[] { 0.6, 0.2, 0.2, 0 }, 1));
                            break;
                        case BuildingType.AntiCapM:
                            list.Add((ushort)(b * 1000 + 81), GetBuilding2((ushort)(b * 1000 + 81), "Умножитель антиматерии " + (i + 1).ToString(),
                            EBuildingParam.AntiAdd, (int)(10 * (1 + b / 5.0)), EBuildingParam.AntiCap, (int)(2000 * (1 + b / 5.0)), BuildingType.AntiCapM, SectorTypes.AntiCap, 2, 
                            new double[] { 0.5, 0.3, 0.1, 0.1 }, 3));
                            break;
                        case BuildingType.AntiCapL:
                            list.Add((ushort)(b * 1000 + 82), GetBuilding2((ushort)(b * 1000 + 82), "Хранилище антиматерии " + (i + 1).ToString(),
                            EBuildingParam.AntiCap, (int)(10000 * (1 + b / 5.0)), EBuildingParam.AntiCapMult, 3, BuildingType.AntiCapL, SectorTypes.AntiCap, 4,
                            new double[] { 0.1, 0.5, 0.3, 0.1 }, 28));
                            break;

                        case BuildingType.RepairS:
                            list.Add((ushort)(b * 1000 + 90), GetBuilding2((ushort)(b * 1000 + 90), "Ремонтный ангар " + (i + 1).ToString(),
                            EBuildingParam.RepairPower, (int)(10 * (1 + b / 5.0)), EBuildingParam.RepairMult, 1 , BuildingType.RepairS, SectorTypes.Repair, 1,
                            new double[] { 0.4, 0.25, 0.25, 0.1},1));
                            break;
                        case BuildingType.RepairM:
                            list.Add((ushort)(b * 1000 + 91), GetBuilding1((ushort)(b * 1000 + 91), "Производство деталей " + (i + 1).ToString(),
                            EBuildingParam.RepairMult, (int)(4  + b / 5.0), BuildingType.RepairM, SectorTypes.Repair, 2,
                            new double[] { 0.2, 0.3, 0.4, 0.1 }, 5));
                            break;
                        case BuildingType.RepairL:
                            list.Add((ushort)(b * 1000 + 92), GetBuilding2((ushort)(b * 1000 + 92), "Испытательный полигон " + (i + 1).ToString(),
                            EBuildingParam.RepairPower, (int)(100 * (1 + b / 5.0)), EBuildingParam.RepairMult, 5, BuildingType.RepairL, SectorTypes.Repair, 5, 
                            new double[] { 0.1, 0.2, 0.5, 0.2 }, 30));
                            break;
                        case BuildingType.Portal:
                            list.Add((ushort)(b * 1000 + 100), GetBuilding1((ushort)(b * 1000 + 100), "Военный портал " + (i + 1).ToString(), EBuildingParam.Range,
                                10 + i * 3, BuildingType.Portal, SectorTypes.War, 0, new double[] { 0.25, 0.25, 0.25, 0.25 }, 1));
                            break;
                        case BuildingType.Radar:
                            list.Add((ushort)(b * 1000 + 101), GetBuilding1((ushort)(b * 1000 + 101), "Радар " + (i + 1).ToString(), EBuildingParam.Ships,
                                1 + i, BuildingType.Radar, SectorTypes.War, 2, new double[] { 0.1, 0.2, 0.3, 0.5 }, 4));
                            break;
                        case BuildingType.DataCenter:
                            list.Add((ushort)(b*1000+102), GetBuilding1((ushort)(b*1000+102), "Дата центр "+(i+1).ToString(), EBuildingParam.MathPower,
                                300+i*200, BuildingType.DataCenter, SectorTypes.War, 2, new double[] { 0.15, 0.1, 0.4, 0.35 }, 4));
                            break;
                        case BuildingType.Education:
                            list.Add((ushort)(b*1000+103), GetBuilding1((ushort)(b*1000+103), "Учебный центр "+(i+1).ToString(), EBuildingParam.Education,
                                3+i*2, BuildingType.Education, SectorTypes.War, 2, new double[] { 0.2, 0.4, 0.3, 0.1 }, 4));
                            break;
                    }
                }
            }
            foreach (GSBuilding building in list.Values)
                AllBuildings[building.Type].Add(building.Level, building);
            /*
            list.Add(10, GetBuilding2(10, "Жилой массив S1", EBuildingParam.People, 3, EBuildingParam.Grow, 100, BuildingType.LiveS, SectorTypes.Live, 1));
            list.Add(11, GetBuilding1(11, "Жилой массив M1", EBuildingParam.Grow, 400, BuildingType.LiveM, SectorTypes.Live, 2));
            list.Add(12, GetBuilding2(12, "Жилой массив L1", EBuildingParam.People, 30, EBuildingParam.Grow, 300, BuildingType.LiveL, SectorTypes.Live, 4));

            list.Add(20, GetBuilding2(20, "Банк S1", EBuildingParam.MoneyAdd, 100, EBuildingParam.MoneyMult, 1, BuildingType.MoneyS,SectorTypes.Money, 1));
            list.Add(21, GetBuilding1(21, "Банк M1", EBuildingParam.MoneyMult, 6, BuildingType.MoneyM, SectorTypes.Money, 3));
            list.Add(22, GetBuilding2(22, "Банк L1", EBuildingParam.MoneyAdd, 800, EBuildingParam.MoneyMult, 4, BuildingType.MoneyL, SectorTypes.Money, 4));
            
            list.Add(30, GetBuilding2(30, "Шахта S1", EBuildingParam.MetallAdd, 10, EBuildingParam.MetalMult, 1, BuildingType.MetalS, SectorTypes.Metall, 1));
            list.Add(31, GetBuilding2(31, "Шахта M1", EBuildingParam.MetalMult, 3, EBuildingParam.MetalCap, 1000, BuildingType.MetalM, SectorTypes.Metall, 2));
            list.Add(32, GetBuilding2(32, "Шахта L1", EBuildingParam.MetallAdd, 110, EBuildingParam.MetalMult, 2, BuildingType.MetalL, SectorTypes.Metall, 4));
            
            list.Add(40, GetBuilding2(40, "Склад металла S1", EBuildingParam.MetalCap, 1000, EBuildingParam.MetalCapMult, 1, BuildingType.MetalCapS, SectorTypes.MetalCap,1));
            list.Add(41, GetBuilding2(41, "Склад металла M1", EBuildingParam.MetallAdd, 10, EBuildingParam.MetalCap, 1000, BuildingType.MetalCapM, SectorTypes.MetalCap, 2));
            list.Add(42, GetBuilding2(42, "Склад металла L1", EBuildingParam.MetalCap, 14000, EBuildingParam.MetalCapMult, 2, BuildingType.MetalCapL, SectorTypes.MetalCap, 5));
            */

            //list.Add(50, GetBuilding2(50, "Фабрика микросхем", EBuildingParam.ChipsAdd, 100, EBuildingParam.ChipMult, 3, BuildingType.ChipS, SectorTypes.Chips,1));
            //list.Add(60, GetBuilding2(60, "Склад микросхем", EBuildingParam.ChipCap, 100, EBuildingParam.ChipCapMult, 3, BuildingType.ChipCapS, SectorTypes.ChipsCap,1));
            //list.Add(70, GetBuilding2(70, "Ускоритель частиц", EBuildingParam.AntiAdd, 100, EBuildingParam.AntiMult, 3, BuildingType.AntiS, SectorTypes.Anti,1));
            //list.Add(80, GetBuilding2(80, "Склад антиматерии", EBuildingParam.AntiCap, 100, EBuildingParam.AntiCapMult, 3, BuildingType.AntiCapS, SectorTypes.AntiCap,1));
            //list.Add(90, GetBuilding2(90, "Завод", EBuildingParam.RepairPower, 100, EBuildingParam.RepairMult, 3, BuildingType.RepairS, SectorTypes.Repair,1));
            //list.Add(100, GetBuilding1(100, "Радар", EBuildingParam.Ships, 1, BuildingType.Radar, SectorTypes.War,2,new double[] { 0.1, 0.2, 0.3, 0.5 }, 4));
            //list.Add(110, GetBuilding1(110, "Дата центр", EBuildingParam.MathPower, 1000, BuildingType.DataCenter, SectorTypes.War,2,new double[] { 0.15, 0.1, 0.4, 0.35 }, 4));
            //list.Add(120, GetBuilding1(120, "Учебный центр", EBuildingParam.Education, 1, BuildingType.Education, SectorTypes.War,2, new double[] { 0.2, 0.4, 0.3, 0.1 }, 4));
            return list;
        }
        public int CompareTo(object b)
        {
            GSBuilding building = (GSBuilding)b;
            if (building.ID > ID) return 1;
            else if (building.ID < ID) return -1;
            else return 0;
        }
        public static string[] GetTextInfo()
        {
            List<string> result = new List<string>();
            for (int i = 0; i < 31; i++)
            {
                BuildingType type = (BuildingType)i;
                foreach (GSBuilding b in Links.Buildings.Values)
                    if (b.Type==type)
                        result.Add(b.ToString());
            }
            return result.ToArray();
        }
        static GSBuilding GetBuilding2(ushort id, string name, EBuildingParam param1, int val1, EBuildingParam param2, int val2, BuildingType type, SectorTypes sector, int size, double[] price, int pricemodi)
        {
            byte level = (byte)(id / 1000);
            GSBuilding building = new GSBuilding(id, size, name, GetPrice(level, pricemodi, price), level,
                new BuildingParam[] { new BuildingParam(param1, val1), new BuildingParam(param2, val2) }, type, sector);
            return building;
        }
        static ItemPrice GetPrice(byte level, int pricemodi, double[] price)
        {
            long sumprice = (long)(1000 + 1000 * (Math.Pow(level, 2)));
            sumprice = pricemodi * sumprice;
            double chipdelta = level / 100.0;
            double[] price1 = new double[4];
            price1[0] = price[0] * chipdelta; price1[1] = price[1] * chipdelta; price1[3] = price[3] * chipdelta;
            price1[2] = price1[0] + price1[1] + price1[3];
            price[0] -= price1[0]; price[1] -= price1[1]; price[2] += price1[2]; price[3] -= price1[3];
            double antidelta = level > 20 ? (level - 20) / 50.0 : 0;
            double[] price2 = new double[4];
            price2[0] = price[0] * antidelta; price2[1] = price[1] * antidelta; price2[2] = price[2] * antidelta;
            price2[3] = price2[0] + price2[1] + price2[2];
            price[0] -= price2[0]; price[1] -= price2[1]; price[2] -= price2[2]; price[3] += price2[3];
            int money = (int)(Math.Round(price[0] * sumprice / 1000, 0) * 1000);
            if (money == 0) money = 1000;
            int metal = GetRound((int)(price[1]*sumprice/10));// (int)(price[1] * sumprice/10); metal = metal / 50; metal = metal * 50;
            int chips = GetRound((int)(price[2] * sumprice / 10));// (int)(price[2] * sumprice/10); chips = chips / 50; chips = chips * 50;
            int anti = GetRound((int)(price[3] * sumprice / 10)); //(int)(price[3] * sumprice/10); anti = anti / 50; anti = anti * 50;
            return new ItemPrice(money, metal, chips, anti);
        }
        static int GetRound(int value)
        {
            if (value < 100000)
            {
                return (int)(Math.Round(value / 50.0, 0) * 50);
            }
            else
            {
                int dimensions = (int)Math.Log10(value) - 2;
                Console.WriteLine(dimensions);
                return (int)(Math.Round(value / Math.Pow(10, dimensions)) * Math.Pow(10, dimensions));
            }
        }
        static GSBuilding GetBuilding1(ushort id, string name, EBuildingParam param1, int val1, BuildingType type, SectorTypes sector, int size, double[] price, int pricemodi)
        {
            byte level = (byte)(id / 1000);
            GSBuilding building = new GSBuilding(id, size, name, GetPrice(level, pricemodi, price), level,
                new BuildingParam[] { new BuildingParam(param1, val1) }, type, sector);
            return building;
        }
        public Brush GetFirstParamBrush()
        {
            switch (Type)
            {
                case BuildingType.LiveS: return Links.Brushes.PeopleImageBrush;
                case BuildingType.LiveM: return Links.Brushes.PeopleImageBrush;
                case BuildingType.LiveL: return Links.Brushes.PeopleImageBrush;
                case BuildingType.MoneyS: return Links.Brushes.MoneyImageBrush;
                case BuildingType.MoneyM: return Links.Brushes.MoneyImageBrush;
                case BuildingType.MoneyL: return Links.Brushes.MoneyImageBrush;
                case BuildingType.MetalS: return Links.Brushes.MetalImageBrush;
                case BuildingType.MetalM: return Links.Brushes.MetalImageBrush;
                case BuildingType.MetalL: return Links.Brushes.MetalImageBrush;
                case BuildingType.MetalCapS: return Links.Brushes.MetalCapBrush;
                case BuildingType.MetalCapM: return Links.Brushes.MetalImageBrush;
                case BuildingType.MetalCapL: return Links.Brushes.MetalCapBrush;
                case BuildingType.ChipS: return Links.Brushes.ChipsImageBrush;
                case BuildingType.ChipM: return Links.Brushes.ChipsImageBrush;
                case BuildingType.ChipL: return Links.Brushes.ChipsImageBrush;
                case BuildingType.ChipCapS: return Links.Brushes.ChipCapBrush;
                case BuildingType.ChipCapM: return Links.Brushes.ChipsImageBrush;
                case BuildingType.ChipCapL: return Links.Brushes.ChipCapBrush;
                case BuildingType.AntiS: return Links.Brushes.AntiImageBrush;
                case BuildingType.AntiM: return Links.Brushes.AntiImageBrush;
                case BuildingType.AntiL: return Links.Brushes.AntiImageBrush;
                case BuildingType.AntiCapS: return Links.Brushes.AntiCapBrush;
                case BuildingType.AntiCapM: return Links.Brushes.AntiImageBrush;
                case BuildingType.AntiCapL: return Links.Brushes.AntiCapBrush;
                case BuildingType.RepairS: return Links.Brushes.RepairPict;
                case BuildingType.RepairM: return Links.Brushes.RepairPict;
                case BuildingType.RepairL: return Links.Brushes.RepairPict;
                case BuildingType.Portal: return Links.Brushes.Radar;
                case BuildingType.Radar: return Links.Brushes.ShipImageBrush;
                case BuildingType.DataCenter: return Links.Brushes.MathPowerBrush;
                case BuildingType.Education: return Links.Brushes.ExperienceBrush;
                default: return null;
            }
        }
        public int GetFirstParamValue()
        {
            return Params[0].Value;
        }
        public Brush GetSecondParamBrush()
        {
            switch (Type)
            {
                case BuildingType.LiveS: return Links.Brushes.PeopleImageBrush;
                case BuildingType.LiveL: return Links.Brushes.PeopleImageBrush;
                case BuildingType.MoneyS: return Links.Brushes.MoneyImageBrush;
                case BuildingType.MoneyL: return Links.Brushes.MoneyImageBrush;
                case BuildingType.MetalS: return Links.Brushes.MetalImageBrush;
                case BuildingType.MetalM: return Links.Brushes.MetalCapBrush;
                case BuildingType.MetalL: return Links.Brushes.MetalImageBrush;
                case BuildingType.MetalCapS: return Links.Brushes.MetalCapBrush;
                case BuildingType.MetalCapM: return Links.Brushes.MetalCapBrush;
                case BuildingType.MetalCapL: return Links.Brushes.MetalCapBrush;
                case BuildingType.ChipS: return Links.Brushes.ChipsImageBrush;
                case BuildingType.ChipM: return Links.Brushes.ChipCapBrush;
                case BuildingType.ChipL: return Links.Brushes.ChipsImageBrush;
                case BuildingType.ChipCapS: return Links.Brushes.ChipCapBrush;
                case BuildingType.ChipCapM: return Links.Brushes.ChipCapBrush;
                case BuildingType.ChipCapL: return Links.Brushes.ChipCapBrush;
                case BuildingType.AntiS: return Links.Brushes.AntiImageBrush;
                case BuildingType.AntiM: return Links.Brushes.AntiCapBrush;
                case BuildingType.AntiL: return Links.Brushes.AntiImageBrush;
                case BuildingType.AntiCapS: return Links.Brushes.AntiCapBrush;
                case BuildingType.AntiCapM: return Links.Brushes.AntiCapBrush;
                case BuildingType.AntiCapL: return Links.Brushes.AntiCapBrush;
                case BuildingType.RepairS: return Links.Brushes.RepairPict;
                case BuildingType.RepairL: return Links.Brushes.RepairPict;
                default: return null;
            }
        }
        public override string ToString()
        {
            if (Params.Length == 1)
                return String.Format("ID={0} Level={1} Size={2} {3} {4}", ID, Level, Size, Name, Params[0]);
            else
                return String.Format("ID={0} Level={1} Size={2} {3} {4} {5}", ID, Level, Size, Name, Params[0], Params[1]);
        }
        public int GetSeconParamValue()
        {
            return Params[1].Value;
        }
        public GSBuilding(ushort id, int size, string name, ItemPrice price, byte level,  BuildingParam[] Params, BuildingType type, SectorTypes sector)
        {
            ID = id;
            Size = size;
            Name = name;
            Price = price;
            Level = level;
            this.Params = Params;
            Type = type;
            Sector = sector;
        }
    }
    public class LandCapacity
    {
        public int Metall = 0;
        public int Chips = 0;
        public int Anti = 0;
        public int Repair = 0;
        public void Add(LandCapacity cap)
        {
            Metall += cap.Metall;
            Chips += cap.Chips;
            Anti += cap.Anti;
            Repair += cap.Repair;
        }
    }
    public class LandAdd
    {
        public double Grow = 0;
        public int Money = 0;
        public int Metall = 0;
        public int Chips = 0;
        public int Anti = 0;
        public void Add(LandAdd add)
        {
            Grow += add.Grow;
            Money += add.Money;
            Metall += add.Metall;
            Chips += add.Chips;
            Anti += add.Anti;
        }
    }
    public class LandMult
    {
        public double Money = 0;
        public double Metall = 0;
        public double Chips = 0;
        public double Anti = 0;
        public double Repair = 0;
        public double Cap = 0;
        public double Total = 0;
    }
    public enum EBuildingParam
    {
        Grow, People,
        MoneyAdd, MoneyMult,
        MetallAdd,  MetalMult, MetalCap, MetalCapMult,
        ChipsAdd, ChipMult, ChipCap, ChipCapMult,
        AntiAdd, AntiMult, AntiCap, AntiCapMult,
        RepairPower, RepairMult,
        Range,
        Ships,
        MathPower, 
        Education
    }
    public class BuildingParam
    {
        public int Value;
        public EBuildingParam Type;
        public BuildingParam(EBuildingParam type, int value)
        {
            Type = type; Value = value;
        }
        public override string ToString()
        {
            switch (Type)
            {
                case EBuildingParam.Grow: return string.Format("Прирост населения={0}", Value / 1000.0);
                case EBuildingParam.People: return string.Format("Расселение={0}", Value);
                case EBuildingParam.MoneyAdd: return string.Format("Прирост денег={0}", Value);
                case EBuildingParam.MoneyMult: return string.Format("Умножение денег={0}%", Value);
                case EBuildingParam.MetallAdd: return string.Format("Прирост металла={0}", Value);
                case EBuildingParam.MetalMult: return string.Format("Умножение металла={0}%", Value);
                case EBuildingParam.MetalCap: return string.Format("Склад металла={0}", Value);
                case EBuildingParam.MetalCapMult: return string.Format("Умножение склада металла={0}%", Value);
                case EBuildingParam.ChipsAdd: return string.Format("Прирост микросхем={0}", Value);
                case EBuildingParam.ChipMult: return string.Format("Умножение микросхем={0}%", Value);
                case EBuildingParam.ChipCap: return string.Format("Склад микросхем={0}", Value);
                case EBuildingParam.ChipCapMult: return string.Format("Умножение склада микросхем={0}%", Value);
                case EBuildingParam.AntiAdd: return string.Format("Прирост антиматерии={0}", Value);
                case EBuildingParam.AntiMult: return string.Format("Умножение аниматерии={0}%", Value);
                case EBuildingParam.AntiCap: return string.Format("Склад антиматерии={0}", Value);
                case EBuildingParam.AntiCapMult: return string.Format("Умножение склада антиматерии={0}", Value);
                case EBuildingParam.RepairPower: return string.Format("Ремонт={0}", Value);
                case EBuildingParam.RepairMult: return string.Format("Умножение ремонта={0}%", Value);
                case EBuildingParam.Range: return string.Format("Дальность флота={0}", Value);
                case EBuildingParam.Ships: return string.Format("Максимум кораблей={0}", Value);
                case EBuildingParam.MathPower: return string.Format("Прочность портала={0}", Value);
                case EBuildingParam.Education: return string.Format("Бонус к обучению={0}", Value);
            }
            return "Ошибка";
        }
        public byte[] GetArray()
        {
            List<byte> array = new List<byte>();
            array.Add((byte)Type);
            array.AddRange(BitConverter.GetBytes(Value));
            return array.ToArray();
        }
        public BuildingParam(byte[] array, ref int i)
        {
            Type = (EBuildingParam)array[i]; i++;
            Value = BitConverter.ToInt32(array, i); i += 4;
        }
    }
    public class BuildingValues
    {
        public int Money;
        public int Metall;
        public int Chips;
        public int Anti;
        public BuildingValues()
        { }
        public BuildingValues(int money, int metal, int chips, int anti)
        {
            Money = money;
            Metall = metal;
            Chips = chips;
            Anti = anti;
        }
        public void AddCap(ServerLand land)
        {
            Metall += (int)land.GetAddParameter(Building2Parameter.MetalCap);
            Chips += (int)land.GetAddParameter(Building2Parameter.ChipCap);
            Anti += (int)land.GetAddParameter(Building2Parameter.AntiCap);
        }
        public BuildingValues(byte[] array, ref int startindex)
        {
            Money = BitConverter.ToInt32(array, startindex); startindex += 4;
            Metall = BitConverter.ToInt32(array, startindex); startindex += 4;
            Chips = BitConverter.ToInt32(array, startindex); startindex += 4;
            Anti = BitConverter.ToInt32(array, startindex); startindex += 4;
        }

    }
}
