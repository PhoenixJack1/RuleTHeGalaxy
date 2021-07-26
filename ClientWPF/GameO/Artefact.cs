using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Client
{
    public enum ArtefactType { Peace, Battle}
    public enum ArtefactAbility {PResourceAdd, PPeopleClone, PSectorRemove, PsectorCreate, PColonyCreate,
    BShipMove, BShipRestore, BAsteroidMove, BPortalMove, BEnergyBlock, BPhysicBlock, BIrregularBlock, BCyberBlock, BEnergyIncrease, BPhysicIncrease,
    BIrregularIncrease, BCyberIncrease, BEnergyDamage, BPhysicDamage, BIrregularDamage, BCyberDamage    }
    public class Artefact:IComparable
    {
        public ushort ID;
        public ArtefactType Type;
        public ArtefactAbility Ability;
        public int Param1;
        public byte WaitTime;
        public int EnergyCost;
        public int Price;
        public BattleArtefact Battle {
            get { return BattleArtefactParams[Ability]; }
        }
        public static int[,] ArtPrices;
        public Artefact(ushort id, ArtefactType type, ArtefactAbility ability,int p, byte w, int e, int price)
        {
            ID = id; Type = type; Ability = ability;
            Param1 = p; WaitTime = w; EnergyCost = e; Price = price;
        }
        public int CompareTo(object b)
        {
            Artefact art = (Artefact)b;
            if (art.ID > ID) return 1;
            else if (art.ID < ID) return -1;
            else return 0;
        }
        
        public static SortedList<ushort, Artefact> GetList()
        {
            SortedList<ushort, Artefact> result = new SortedList<ushort, Artefact>();
            result.Add(0, new Artefact(0, ArtefactType.Peace, ArtefactAbility.PResourceAdd, 10, 0,0, 5000));
            result.Add(1000, new Artefact(1000, ArtefactType.Peace, ArtefactAbility.PResourceAdd, 15, 0, 0, 20000));
            result.Add(2000, new Artefact(2000, ArtefactType.Peace, ArtefactAbility.PResourceAdd, 20, 0, 0, 100000));
            result.Add(3000, new Artefact(3000, ArtefactType.Peace, ArtefactAbility.PResourceAdd, 25, 0, 0, 350000));
            result.Add(4000, new Artefact(4000, ArtefactType.Peace, ArtefactAbility.PResourceAdd, 30, 0, 0, 1000000));
            result.Add(1, new Artefact(1, ArtefactType.Peace, ArtefactAbility.PPeopleClone, 10, 0, 0, 10000));
            result.Add(1001, new Artefact(1001, ArtefactType.Peace, ArtefactAbility.PPeopleClone, 12, 0, 0, 50000));
            result.Add(2001, new Artefact(2001, ArtefactType.Peace, ArtefactAbility.PPeopleClone, 14, 0, 0, 200000));
            result.Add(3001, new Artefact(3001, ArtefactType.Peace, ArtefactAbility.PPeopleClone, 17, 0, 0, 500000));
            result.Add(4001, new Artefact(4001, ArtefactType.Peace, ArtefactAbility.PPeopleClone, 20, 0, 0, 2000000));
            result.Add(2, new Artefact(2, ArtefactType.Peace, ArtefactAbility.PSectorRemove, 0, 0, 0, 500000));
            result.Add(3, new Artefact(3, ArtefactType.Peace,ArtefactAbility.PsectorCreate,5, 0, 0, 300000));
            result.Add(1003, new Artefact(1003, ArtefactType.Peace, ArtefactAbility.PsectorCreate, 6, 0, 0, 1200000));
            result.Add(2003, new Artefact(2003, ArtefactType.Peace, ArtefactAbility.PsectorCreate, 7, 0, 0, 5000000));
            result.Add(4, new Artefact(4, ArtefactType.Peace, ArtefactAbility.PColonyCreate,400, 0, 0, 200000));
            result.Add(1004, new Artefact(1004, ArtefactType.Peace, ArtefactAbility.PColonyCreate, 400, 0, 0, 500000));
            result.Add(2004, new Artefact(2004, ArtefactType.Peace, ArtefactAbility.PColonyCreate, 600, 0, 0, 1000000));
            result.Add(3004, new Artefact(3004, ArtefactType.Peace, ArtefactAbility.PColonyCreate, 800, 0, 0, 2000000));
            result.Add(4004, new Artefact(4004, ArtefactType.Peace, ArtefactAbility.PColonyCreate, 1000, 0, 0, 5000000));
            result.Add(100, new Artefact(100, ArtefactType.Battle,ArtefactAbility.BShipMove, 0,5,200, 1000));
            result.Add(1100, new Artefact(1100, ArtefactType.Battle, ArtefactAbility.BShipMove, 0, 4, 400, 10000));
            result.Add(2100, new Artefact(2100, ArtefactType.Battle, ArtefactAbility.BShipMove, 0, 3, 1000, 100000));
            result.Add(3100, new Artefact(3100, ArtefactType.Battle, ArtefactAbility.BShipMove, 0, 2, 2000, 1000000));
            result.Add(4100, new Artefact(4100, ArtefactType.Battle, ArtefactAbility.BShipMove, 0, 1, 3000, 10000000));
            result.Add(101, new Artefact(101, ArtefactType.Battle, ArtefactAbility.BShipRestore,0,5,300, 2000));
            result.Add(1101, new Artefact(1101, ArtefactType.Battle, ArtefactAbility.BShipRestore, 0, 4, 600, 80000));
            result.Add(2101, new Artefact(2101, ArtefactType.Battle, ArtefactAbility.BShipRestore, 0, 3, 1200, 320000));
            result.Add(3101, new Artefact(3101, ArtefactType.Battle, ArtefactAbility.BShipRestore, 0, 2, 2400, 1280000));
            result.Add(4101, new Artefact(4101, ArtefactType.Battle, ArtefactAbility.BShipRestore, 0, 1, 4800, 5120000));
            result.Add(102, new Artefact(102, ArtefactType.Battle, ArtefactAbility.BAsteroidMove,0,7,200, 3000));
            result.Add(1102, new Artefact(1102, ArtefactType.Battle, ArtefactAbility.BAsteroidMove, 0, 6, 400, 15000));
            result.Add(2102, new Artefact(2102, ArtefactType.Battle, ArtefactAbility.BAsteroidMove, 0, 5, 600, 75000));
            result.Add(3102, new Artefact(3102, ArtefactType.Battle, ArtefactAbility.BAsteroidMove, 0, 4, 800, 375000));
            result.Add(4102, new Artefact(4102, ArtefactType.Battle, ArtefactAbility.BAsteroidMove, 0, 3, 1000, 1875000));
            result.Add(103, new Artefact(103, ArtefactType.Battle, ArtefactAbility.BPortalMove,0, 8,1000, 120000));
            result.Add(1103, new Artefact(1103, ArtefactType.Battle, ArtefactAbility.BPortalMove, 0, 7, 1500, 240000));
            result.Add(2103, new Artefact(2103, ArtefactType.Battle, ArtefactAbility.BPortalMove, 0, 6, 2000, 480000));
            result.Add(3103, new Artefact(3103, ArtefactType.Battle, ArtefactAbility.BPortalMove, 0, 5, 2500, 960000));
            result.Add(4103, new Artefact(4103, ArtefactType.Battle, ArtefactAbility.BPortalMove, 0, 4, 3000, 1920000));
            result.Add(110, new Artefact(110, ArtefactType.Battle, ArtefactAbility.BEnergyBlock,1, 5,500, 10000));
            result.Add(1110, new Artefact(1110, ArtefactType.Battle, ArtefactAbility.BEnergyBlock, 2, 6, 1500, 40000));
            result.Add(2110, new Artefact(2110, ArtefactType.Battle, ArtefactAbility.BEnergyBlock, 3, 7, 2000, 240000));
            result.Add(3110, new Artefact(3110, ArtefactType.Battle, ArtefactAbility.BEnergyBlock, 4, 8, 3000, 960000));
            result.Add(4110, new Artefact(4110, ArtefactType.Battle, ArtefactAbility.BEnergyBlock, 5, 9, 4500, 3840000));
            result.Add(111, new Artefact(111, ArtefactType.Battle, ArtefactAbility.BPhysicBlock, 1, 5, 500, 10000));
            result.Add(1111, new Artefact(1111, ArtefactType.Battle, ArtefactAbility.BPhysicBlock, 2, 6, 1500, 40000));
            result.Add(2111, new Artefact(2111, ArtefactType.Battle, ArtefactAbility.BPhysicBlock, 3, 7, 2000, 240000));
            result.Add(3111, new Artefact(3111, ArtefactType.Battle, ArtefactAbility.BPhysicBlock, 4, 8, 3000, 960000));
            result.Add(4111, new Artefact(4111, ArtefactType.Battle, ArtefactAbility.BPhysicBlock, 5, 9, 4500, 3840000));
            result.Add(112, new Artefact(112, ArtefactType.Battle, ArtefactAbility.BIrregularBlock, 1, 5, 500, 10000));
            result.Add(1112, new Artefact(1112, ArtefactType.Battle, ArtefactAbility.BIrregularBlock, 2, 6, 1500, 40000));
            result.Add(2112, new Artefact(2112, ArtefactType.Battle, ArtefactAbility.BIrregularBlock, 3, 7, 2000, 240000));
            result.Add(3112, new Artefact(3112, ArtefactType.Battle, ArtefactAbility.BIrregularBlock, 4, 8, 3000, 960000));
            result.Add(4112, new Artefact(4112, ArtefactType.Battle, ArtefactAbility.BIrregularBlock, 5, 9, 4500, 3840000));
            result.Add(113, new Artefact(113, ArtefactType.Battle, ArtefactAbility.BCyberBlock, 1, 5, 500, 10000));
            result.Add(1113, new Artefact(1113, ArtefactType.Battle, ArtefactAbility.BCyberBlock, 2, 6, 1500, 40000));
            result.Add(2113, new Artefact(2113, ArtefactType.Battle, ArtefactAbility.BCyberBlock, 3, 7, 2000, 240000));
            result.Add(3113, new Artefact(3113, ArtefactType.Battle, ArtefactAbility.BCyberBlock, 4, 8, 3000, 960000));
            result.Add(4113, new Artefact(4113, ArtefactType.Battle, ArtefactAbility.BCyberBlock, 5, 9, 4500, 3840000));
            result.Add(120, new Artefact(120, ArtefactType.Battle, ArtefactAbility.BEnergyIncrease,1,6,800, 20000));
            result.Add(1120, new Artefact(1120, ArtefactType.Battle, ArtefactAbility.BEnergyIncrease, 2, 7, 1600, 120000));
            result.Add(2120, new Artefact(2120, ArtefactType.Battle, ArtefactAbility.BEnergyIncrease, 3, 8, 3200, 720000));
            result.Add(3120, new Artefact(3120, ArtefactType.Battle, ArtefactAbility.BEnergyIncrease, 3, 7, 6400, 4320000));
            result.Add(4120, new Artefact(4120, ArtefactType.Battle, ArtefactAbility.BEnergyIncrease, 3, 6, 12800, 26000000));
            result.Add(121, new Artefact(121, ArtefactType.Battle, ArtefactAbility.BPhysicIncrease, 1, 6, 800, 20000));
            result.Add(1121, new Artefact(1121, ArtefactType.Battle, ArtefactAbility.BPhysicIncrease, 2, 7, 1600, 120000));
            result.Add(2121, new Artefact(2121, ArtefactType.Battle, ArtefactAbility.BPhysicIncrease, 3, 8, 3200, 720000));
            result.Add(3121, new Artefact(3121, ArtefactType.Battle, ArtefactAbility.BPhysicIncrease, 3, 7, 6400, 4320000));
            result.Add(4121, new Artefact(4121, ArtefactType.Battle, ArtefactAbility.BPhysicIncrease, 3, 6, 12800, 26000000));
            result.Add(122, new Artefact(122, ArtefactType.Battle, ArtefactAbility.BIrregularIncrease, 1, 6, 800, 20000));
            result.Add(1122, new Artefact(1122, ArtefactType.Battle, ArtefactAbility.BIrregularIncrease, 2, 7, 1600, 120000));
            result.Add(2122, new Artefact(2122, ArtefactType.Battle, ArtefactAbility.BIrregularIncrease, 3, 8, 3200, 720000));
            result.Add(3122, new Artefact(3122, ArtefactType.Battle, ArtefactAbility.BIrregularIncrease, 3, 7, 6400, 4320000));
            result.Add(4122, new Artefact(4122, ArtefactType.Battle, ArtefactAbility.BIrregularIncrease, 3, 6, 12800, 26000000));
            result.Add(123, new Artefact(123, ArtefactType.Battle, ArtefactAbility.BCyberIncrease, 1, 6, 800, 20000));
            result.Add(1123, new Artefact(1123, ArtefactType.Battle, ArtefactAbility.BCyberIncrease, 2, 7, 1600, 120000));
            result.Add(2123, new Artefact(2123, ArtefactType.Battle, ArtefactAbility.BCyberIncrease, 3, 8, 3200, 720000));
            result.Add(3123, new Artefact(3123, ArtefactType.Battle, ArtefactAbility.BCyberIncrease, 3, 7, 6400, 4320000));
            result.Add(4123, new Artefact(4123, ArtefactType.Battle, ArtefactAbility.BCyberIncrease, 3, 6, 12800, 26000000));
            result.Add(130, new Artefact(130, ArtefactType.Battle, ArtefactAbility.BEnergyDamage, 100, 4,500,5000));
            result.Add(1130, new Artefact(1130, ArtefactType.Battle, ArtefactAbility.BEnergyDamage, 150, 4, 1000, 50000));
            result.Add(2130, new Artefact(2130, ArtefactType.Battle, ArtefactAbility.BEnergyDamage, 200, 4, 1500, 250000));
            result.Add(3130, new Artefact(3130, ArtefactType.Battle, ArtefactAbility.BEnergyDamage, 250, 4, 2000, 1250000));
            result.Add(4130, new Artefact(4130, ArtefactType.Battle, ArtefactAbility.BEnergyDamage, 300, 4, 2500, 6250000));
            result.Add(131, new Artefact(131, ArtefactType.Battle, ArtefactAbility.BPhysicDamage,100, 4, 500, 5000));
            result.Add(1131, new Artefact(1131, ArtefactType.Battle, ArtefactAbility.BPhysicDamage, 150, 4, 1000, 50000));
            result.Add(2131, new Artefact(2131, ArtefactType.Battle, ArtefactAbility.BPhysicDamage, 200, 4, 1500, 250000));
            result.Add(3131, new Artefact(3131, ArtefactType.Battle, ArtefactAbility.BPhysicDamage, 250, 4, 2000, 1250000));
            result.Add(4131, new Artefact(4131, ArtefactType.Battle, ArtefactAbility.BPhysicDamage, 300, 4, 2500, 6250000));
            result.Add(132, new Artefact(132, ArtefactType.Battle, ArtefactAbility.BIrregularDamage,100, 4,750, 10000));
            result.Add(1132, new Artefact(1132, ArtefactType.Battle, ArtefactAbility.BIrregularDamage, 150, 4, 1500, 100000));
            result.Add(2132, new Artefact(2132, ArtefactType.Battle, ArtefactAbility.BIrregularDamage, 200, 4, 3000, 1000000));
            result.Add(3132, new Artefact(3132, ArtefactType.Battle, ArtefactAbility.BIrregularDamage, 200, 3, 6000, 10000000));
            result.Add(4132, new Artefact(4132, ArtefactType.Battle, ArtefactAbility.BIrregularDamage, 200, 2, 12000, 50000000));
            result.Add(133, new Artefact(133, ArtefactType.Battle, ArtefactAbility.BCyberDamage,200, 6, 750, 10000));
            result.Add(1133, new Artefact(1133, ArtefactType.Battle, ArtefactAbility.BCyberDamage, 250, 5, 1500, 100000));
            result.Add(2133, new Artefact(2133, ArtefactType.Battle, ArtefactAbility.BCyberDamage, 300, 4, 3000, 1000000));
            result.Add(3133, new Artefact(3133, ArtefactType.Battle, ArtefactAbility.BCyberDamage, 350, 3, 6000, 10000000));
            result.Add(4133, new Artefact(4133, ArtefactType.Battle, ArtefactAbility.BCyberDamage, 400, 2, 12000, 50000000));
            Artefact[] Arts = result.Values.ToArray();
            for (int i = 0; i < Arts.Length - 1; i++)
            {
                int MinPrice = Arts[i].Price; int minart = i;
                for (int j = i+1; j < Arts.Length; j++)
                {
                    if (Arts[j].Price < MinPrice) { MinPrice = Arts[j].Price; minart = j; }
                }
                Artefact temp = Arts[i]; Arts[i] = Arts[minart]; Arts[minart] = temp;
            }
            ArtPrices = new int[Arts.Length, 2];
            for (int i=0;i<Arts.Length;i++)
            {
                ArtPrices[i, 0] = Arts[i].ID;
                ArtPrices[i, 1] = Arts[i].Price;
            }
            return result;
        }
        public static Artefact GetRandomArt (int price)
        {
            int pos = 0;
            for (int i=0;i<ArtPrices.Length; i++)
            {
                if (ArtPrices[i, 1] > price) break;
                pos = i;
            }
            int maxpos = pos + 5;
            if (maxpos >= ArtPrices.Length) maxpos = ArtPrices.Length - 1;
            pos = ServerLinks.BattleRandom.Next(0, maxpos + 1);
            return ServerLinks.Artefacts[(ushort)ArtPrices[pos, 0]];
        }
        static SortedList<ArtefactAbility, BattleArtefact> BattleArtefactParams = GetBattleArtefactsParams();
        public static SortedList<ArtefactAbility, BattleArtefact> GetBattleArtefactsParams()
        {
            SortedList<ArtefactAbility, BattleArtefact> list = new SortedList<ArtefactAbility, BattleArtefact>();
            list.Add(ArtefactAbility.PResourceAdd, null);
            list.Add(ArtefactAbility.PPeopleClone, null);
            list.Add(ArtefactAbility.PColonyCreate, null);
            list.Add(ArtefactAbility.PsectorCreate, null);
            list.Add(ArtefactAbility.PSectorRemove, null);
            list.Add(ArtefactAbility.BShipMove, new BattleArtefact(BattleArtefactUse.SelfShip, BattleArtefactUse.FreeHex, "Переместить корабль"));
            list.Add(ArtefactAbility.BShipRestore, new BattleArtefact(BattleArtefactUse.SelfShip, BattleArtefactUse.No, "Восстановить полностью"));
            list.Add(ArtefactAbility.BAsteroidMove, new BattleArtefact(BattleArtefactUse.Asteroid, BattleArtefactUse.FreeHex, "Переместить астероид"));
            list.Add(ArtefactAbility.BPortalMove, new BattleArtefact(BattleArtefactUse.SelfPortal, BattleArtefactUse.FreeHex, "Переместить портал"));
            list.Add(ArtefactAbility.BEnergyBlock,  new BattleArtefact(BattleArtefactUse.No, BattleArtefactUse.No, "Заблокировать энергию"));
            list.Add(ArtefactAbility.BPhysicBlock, new BattleArtefact(BattleArtefactUse.No, BattleArtefactUse.No, "Заблокировать физику"));
            list.Add(ArtefactAbility.BIrregularBlock, new BattleArtefact(BattleArtefactUse.No, BattleArtefactUse.No, "Заблокировать аномальность"));
            list.Add(ArtefactAbility.BCyberBlock, new BattleArtefact(BattleArtefactUse.No, BattleArtefactUse.No, "Заблокировать кибернетику"));
            list.Add(ArtefactAbility.BEnergyIncrease,new BattleArtefact(BattleArtefactUse.No, BattleArtefactUse.No, "Усилить энергию"));
            list.Add(ArtefactAbility.BPhysicIncrease, new BattleArtefact(BattleArtefactUse.No, BattleArtefactUse.No, "Усилить физику"));
            list.Add(ArtefactAbility.BIrregularIncrease, new BattleArtefact(BattleArtefactUse.No, BattleArtefactUse.No, "Усилить аномальность"));
            list.Add(ArtefactAbility.BCyberIncrease, new BattleArtefact(BattleArtefactUse.No, BattleArtefactUse.No, "Усилить кибернетику"));
            list.Add(ArtefactAbility.BEnergyDamage, new BattleArtefact(BattleArtefactUse.AllHex, BattleArtefactUse.No, "Нанести урон"));
            list.Add(ArtefactAbility.BPhysicDamage, new BattleArtefact(BattleArtefactUse.AllHex, BattleArtefactUse.No, "Нанести урон"));
            list.Add(ArtefactAbility.BIrregularDamage,new BattleArtefact(BattleArtefactUse.No, BattleArtefactUse.No, "Активировать"));
            list.Add(ArtefactAbility.BCyberDamage, new BattleArtefact(BattleArtefactUse.AllHex, BattleArtefactUse.AllHex, "Нанести урон"));
            return list;
        }
        public string GetName()
        {
            switch (Ability)
            {
                #region Мирные артефакты
                case ArtefactAbility.PResourceAdd: return "Малый Синтезатор материи " + NameCreator.GetRoman((byte)(ID / 1000 + 1));
                case ArtefactAbility.PPeopleClone: return "Капсула клонирования " +NameCreator.GetRoman((byte)(ID / 1000 + 1));
                case ArtefactAbility.PSectorRemove: return "Эко-бомба " + NameCreator.GetRoman((byte)(ID / 1000 + 1));
                case ArtefactAbility.PsectorCreate: return "Фазовый планировщик " + NameCreator.GetRoman((byte)(ID / 1000 + 1));
                case ArtefactAbility.PColonyCreate: return "Малый комплект создания рая " + NameCreator.GetRoman((byte)(ID / 1000 + 1));
                #endregion
                #region Боевые артефакты
                case ArtefactAbility.BShipMove: return "Портативный телепорт " + NameCreator.GetRoman((byte)(ID / 1000 + 1));
                case ArtefactAbility.BShipRestore: return "Временной реставратор " + NameCreator.GetRoman((byte)(ID / 1000 + 1));
                case ArtefactAbility.BAsteroidMove: return "Кротовый перенос " + NameCreator.GetRoman((byte)(ID / 1000 + 1));
                case ArtefactAbility.BPortalMove: return "Трансклюкатор " + NameCreator.GetRoman((byte)(ID / 1000 + 1));
                case ArtefactAbility.BEnergyBlock: return "Распылитель частиц " + NameCreator.GetRoman((byte)(ID / 1000 + 1));
                case ArtefactAbility.BPhysicBlock: return "Магнитный резонатор " + NameCreator.GetRoman((byte)(ID / 1000 + 1));
                case ArtefactAbility.BIrregularBlock: return "Выпрямитель пространства " + NameCreator.GetRoman((byte)(ID / 1000 + 1));
                case ArtefactAbility.BCyberBlock: return "Симулякр помех " + NameCreator.GetRoman((byte)(ID / 1000 + 1));
                case ArtefactAbility.BEnergyIncrease: return "Пространственный ионизатор " + NameCreator.GetRoman((byte)(ID / 1000 + 1));
                case ArtefactAbility.BPhysicIncrease: return "Размегчитель структуры " + NameCreator.GetRoman((byte)(ID / 1000 + 1));
                case ArtefactAbility.BIrregularIncrease: return "Пси-тренинг аномальности " + NameCreator.GetRoman((byte)(ID / 1000 + 1));
                case ArtefactAbility.BCyberIncrease: return "Генератор сверхпроводимости " + NameCreator.GetRoman((byte)(ID / 1000 + 1));
                case ArtefactAbility.BEnergyDamage: return "Призматический вершитель " + NameCreator.GetRoman((byte)(ID / 1000 + 1));
                case ArtefactAbility.BPhysicDamage: return "Экстрактор антиматерии " + NameCreator.GetRoman((byte)(ID / 1000 + 1));
                case ArtefactAbility.BIrregularDamage: return "Локальный искривлятор " + NameCreator.GetRoman((byte)(ID / 1000 + 1));
                case ArtefactAbility.BCyberDamage: return "Кибернетический проектор " + NameCreator.GetRoman((byte)(ID / 1000 + 1));
                #endregion
                default: return "Error";
            }
        }
        public string GetDescription()
        {
            switch (Ability)
            {
                #region Мирные артефакты
                case ArtefactAbility.PResourceAdd: return string.Format("Мгновенно заполняет ваши склады на {0}-{1} процентов ресурсов.",Param1-5,Param1+5);
                case ArtefactAbility.PPeopleClone: return string.Format("Позволяет очень быстро увеличить население колонии. Ресурса хватит на {0} млн. копий. Людям надо где-то жить, так что не увеличивает сверх лимита жилых пространств.", Param1);
                case ArtefactAbility.PSectorRemove: return "Может быстро и безопасно очистить целый сектор вместе с планировкой. Будьте осторожны, людей, корабли и котов также очищает с территории.";
                case ArtefactAbility.PsectorCreate: return string.Format("Позволяет вам немного терраформировать планету, позволяя распланировать целый сектор. Не в состоянии увеличить число секторов свыше {0}.", Param1);
                case ArtefactAbility.PColonyCreate: return string.Format("Структурный анализ показывает, что этот комплект не может быть создан в нашей реальности, однако он существует. Позволяет быстро создать колонию из аванпоста. Его ресурса хватит только для колоний, с населением не более {0} млн. человек.", Param1);
                    #endregion
                #region Боевые артефакты
                case ArtefactAbility.BShipMove: return string.Format("Портативный телепорт создаёт локальный прокол пространства, что позволяет переместить корабль на любую позицию на поле боя. Стоиомсть {0} энергии, повтор через {1}", EnergyCost, GetWaitText(WaitTime));
                case ArtefactAbility.BShipRestore: return string.Format("Временной реставратор синхронизирует корабль со своим же экземпляром, существующим на начало боя, тем самым восстанавливая ему все характеристики.Стоиомсть {0} энергии, повтор через {1}", EnergyCost, GetWaitText(WaitTime));
                case ArtefactAbility.BAsteroidMove: return string.Format("Кротовый перенос локализует и активирует кротовую нору в пространстве, что позволяет переметить один астероид по полю боя. Стоиомсть {0} энергии, повтор через {1}", EnergyCost, GetWaitText(WaitTime));
                case ArtefactAbility.BPortalMove: return string.Format("Трансклюкатор создаёт в пространстве особую разность потенциалов, обеспечивающие порталу направленный импульс от отрицательного полюса к положительному. Это позволяет перемещать портал по полю боя. Стоиомсть {0} энергии, повтор через {1}", EnergyCost, GetWaitText(WaitTime));
                case ArtefactAbility.BEnergyBlock: return string.Format("Распылитель частиц распыляет на поле боя мелкодисперсные частицы, которые полностью блокируют применение энергетического оружия на {0}. Стоиомсть {1} энергии, повтор через {2}", GetWaitText(Param1), EnergyCost, GetWaitText(WaitTime));
                case ArtefactAbility.BPhysicBlock: return string.Format("Магнитный резонатор генерирует на поле боя магнитное поле особой напряжённости, что не позволяет применять физическое оружие в течение {0}. Стоиомсть {1} энергии, повтор через {2}", GetWaitText(Param1), EnergyCost, GetWaitText(WaitTime));
                case ArtefactAbility.BIrregularBlock: return string.Format("Выпрямитель пространства особым образом воздействует на пространство, нарушая законы течения аномальных технологий, что блокирует применение аномального оружия на {0}. Стоиомсть {1} энергии, повтор через {2}", GetWaitText(Param1), EnergyCost, GetWaitText(WaitTime));
                case ArtefactAbility.BCyberBlock: return string.Format("Симулякр помех вызывает в кибернетическом вооружении внутренние помехи, которые блокируют применение кибернетического оружия на {0}. Стоиомсть {1} энергии, повтор через {2}", GetWaitText(Param1), EnergyCost, GetWaitText(WaitTime));
                case ArtefactAbility.BEnergyIncrease: return string.Format("Пространственный ионизатор заряжает космические частицы отрицательными зарядами, увеличивая эффективность энергетического оружия на {0}. Стоиомсть {1} энергии, повтор через {2}", GetWaitText(Param1), EnergyCost, GetWaitText(WaitTime));
                case ArtefactAbility.BPhysicIncrease: return string.Format("Размегчитель структуры нарушает кристаллически связи в бронировании кораблей, увеличивая тем самым эффективность физического оружия на {0}. Стоиомсть {1} энергии, повтор через {2}", GetWaitText(Param1), EnergyCost, GetWaitText(WaitTime));
                case ArtefactAbility.BIrregularIncrease: return string.Format("Пси-тренинг аномальности вкладывает в головы пилотов временные знания мастерства по работе с аномальными технологиями увеличивая эффективность аномального оружия на {0}. Стоиомсть {1} энергии, повтор через {2}", GetWaitText(Param1), EnergyCost, GetWaitText(WaitTime));
                case ArtefactAbility.BCyberIncrease: return string.Format("Генератор сверхпроводимости переводит ключевые системы кибернетического вооружения в сверхпроводимое состояние, увеличивая эффективность кибернетического оружия на {0}. Стоиомсть {1} энергии, повтор через {2}", GetWaitText(Param1), EnergyCost, GetWaitText(WaitTime));
                case ArtefactAbility.BEnergyDamage: return string.Format("Призаматический вершитель активирует вспышку на ближайшей звезде, направляя выделевшуюся энергию на точку на поле боя, нанося тем самым {0} энергетического урона 5 ближайщим целям от точки. Стоиомсть {1} энергии, повтор через {2}", Param1, EnergyCost, GetWaitText(WaitTime));
                case ArtefactAbility.BPhysicDamage: return string.Format("Экстактор антиматерии трансформирует часть космических частиц в античастицы, вызывая мощнейший взрыв и наносит {0} физического урона всем целям в радиусе 2 хекса от точки. Стоиомсть {1} энергии, повтор через {2}", Param1, EnergyCost, GetWaitText(WaitTime));
                case ArtefactAbility.BIrregularDamage: return string.Format("Локальный искривлятор создаёт локальные искривления пространства в местах сосредоточения массы, неизвестным способом нанося {0} аномального урон всем вражеским целям и восстанавливая {0} прочности корпуса своим. Стоиомсть {1} энергии, повтор через {2}", Param1, EnergyCost, GetWaitText(WaitTime));
                case ArtefactAbility.BCyberDamage: return string.Format("Кибернетический проектор вызывает локальную, узконаправленную напряжённость кибернетического поля и наносит {0} кибернетического урона всем целям между двумя точками. Стоиомсть {1} энергии, повтор через {2}", Param1, EnergyCost, GetWaitText(WaitTime));
                #endregion
                default: return "Error";
            }
        }
        string GetWaitText(int time)
        {
            if (time == 1) return "1 ход";
            else if (time < 5) return time.ToString() + " хода";
            else return time.ToString() + " ходов"; 
        }
        public Rectangle GetInFleetRectangle(int size, GSFleet fleet)
        {
            Rectangle rect = Common.GetRectangle(size, GetImage());
            rect.Tag = fleet;
            rect.ToolTip = GetName();
            rect.PreviewMouseDown += RemoveArtefact;
            return rect;
        }

        private void RemoveArtefact(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            Rectangle rect = (Rectangle)sender;
            GSFleet fleet = (GSFleet)rect.Tag;
            if (fleet.Target!=null)
            { Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage("Флот на задании")); return; }
            string eventresult = Events.RemoveWarArtefactFromFleet(fleet, this);
            if (eventresult != "")
            {
                Links.Controller.PopUpCanvas.Place(new SimpleInfoMessage(eventresult)); return;
            }
            else
            {
                Gets.GetTotalInfo("После события по убираению артефакта из флота");
                Links.Controller.SelectPanel(GamePanels.FleetsCanvas, SelectModifiers.None);
            }
        }
        /*public Canvas GetRectangle (int size)
        {
            
        }*/
        public Brush GetImage()
        {
            switch (Ability)
            {
                #region Мирные артефакты
                case ArtefactAbility.PResourceAdd: return Links.Brushes.Artefact.Artefact0000;
                case ArtefactAbility.PPeopleClone: return Links.Brushes.Artefact.Artefact0001;
                case ArtefactAbility.PSectorRemove: return Links.Brushes.Artefact.Artefact0002;
                case ArtefactAbility.PsectorCreate: return Links.Brushes.Artefact.Artefact0003;
                case ArtefactAbility.PColonyCreate: return Links.Brushes.Artefact.Artefact0004;
                #endregion
                #region Боевые артефакты
                case ArtefactAbility.BShipMove: return Links.Brushes.Artefact.Artefact1000;
                case ArtefactAbility.BShipRestore: return Links.Brushes.Artefact.Artefact1001;
                case ArtefactAbility.BAsteroidMove: return Links.Brushes.Artefact.Artefact1002;
                case ArtefactAbility.BPortalMove: return Links.Brushes.Artefact.Artefact0004;
                case ArtefactAbility.BEnergyBlock: return Links.Brushes.Artefact.Artefact1010;
                case ArtefactAbility.BPhysicBlock: return Links.Brushes.Artefact.Artefact1011;
                case ArtefactAbility.BIrregularBlock: return Links.Brushes.Artefact.Artefact1012;
                case ArtefactAbility.BCyberBlock: return Links.Brushes.Artefact.Artefact1013;
                case ArtefactAbility.BEnergyIncrease: return Links.Brushes.Artefact.Artefact1020;
                case ArtefactAbility.BPhysicIncrease: return Links.Brushes.Artefact.Artefact1021;
                case ArtefactAbility.BIrregularIncrease: return Links.Brushes.Artefact.Artefact1022;
                case ArtefactAbility.BCyberIncrease: return Links.Brushes.Artefact.Artefact1023;
                case ArtefactAbility.BEnergyDamage: return Links.Brushes.Artefact.Artefact1030;
                case ArtefactAbility.BPhysicDamage: return Links.Brushes.Artefact.Artefact1031;
                case ArtefactAbility.BIrregularDamage: return Links.Brushes.Artefact.Artefact1032;
                case ArtefactAbility.BCyberDamage: return Links.Brushes.Artefact.Artefact1033;
                #endregion
                default: return null;
            }
        }
        
    }
    public enum BattleArtefactUse { No, SelfShip, EnemyShip, AnyShip, Asteroid, AllHex, FreeHex, SelfPortal}
    public class BattleArtefact
    {
        public BattleArtefactUse FirstTarget;
        public BattleArtefactUse SecondTarget;
        public string Text;
        public BattleArtefact(BattleArtefactUse firsttarget, BattleArtefactUse secondtarget, string text)
        {
            FirstTarget = firsttarget; SecondTarget = secondtarget; Text = text; 
        }
        public static byte[] GetSelectHexes(ArtefactAbility abil, byte hex1, byte targethex)
        {
            List<byte> list;
            Hex Hex1;
            switch (abil)
            {
                case ArtefactAbility.BShipMove:
                case ArtefactAbility.BAsteroidMove:
                case ArtefactAbility.BPortalMove:
                    if (hex1 == 255) return new byte[] { targethex };
                    else return new byte[] { hex1, targethex };
                case ArtefactAbility.BShipRestore:
                    return new byte[] { targethex };
                case ArtefactAbility.BEnergyDamage:
                    Hex1 = HexCanvas.Hexes[targethex];
                    List<ShipB> PassiveTargets = new List<ShipB>();
                    foreach (ShipB ship in IntBoya.Battle.BattleField.Values)
                        if (ship.States.Indestructible == false) PassiveTargets.Add(ship);
                    List<Hex> LightJumpHexes = new List<Hex>();
                    LightJumpHexes.Add(HexCanvas.Hexes[targethex]);
                    Hex LastPoint = Hex1;
                    int selecttargets = 0;
                    if (IntBoya.Battle.BattleField.ContainsKey(targethex) && IntBoya.Battle.BattleField[targethex].States.Indestructible == false)
                    { PassiveTargets.Remove(IntBoya.Battle.BattleField[targethex]); selecttargets++; }
                    for (int i = selecttargets; i < 5; i++)
                    {
                        if (PassiveTargets.Count == 0) break;
                        ShipB NextTarget = PassiveTargets[0]; double distance = Double.MaxValue;
                        foreach (ShipB ship in PassiveTargets)
                        {
                            byte[] intersecthexes = IntBoya.Battle.Field.IntersectHexes[LastPoint.ID, ship.CurHex, 1];
                            bool errortarget = false;
                            foreach (byte b in intersecthexes)
                            {
                                if (IntBoya.Battle.BattleField.ContainsKey(b) && IntBoya.Battle.BattleField[b].States.Side == ShipSide.Neutral) { errortarget = true; break; }
                            }
                            if (errortarget == true) continue;
                            double d = IntBoya.Battle.Field.DistAngleRot[LastPoint.ID, ship.CurHex, 1, 0];
                            if (d < distance) { NextTarget = ship; distance = d; }
                        }
                        if (distance == Int32.MaxValue) break;
                        else { LightJumpHexes.Add(NextTarget.HexShip.Hex); LastPoint = NextTarget.HexShip.Hex; PassiveTargets.Remove(NextTarget); }
                    }
                    list = new List<byte>();
                    foreach (Hex hex in LightJumpHexes)
                        list.Add(hex.ID);
                    return list.ToArray();
                case ArtefactAbility.BPhysicDamage:
                    list = new List<byte>();
                    Hex1 = IntBoya.Battle.Field.Hexes[targethex];
                    foreach (Hex hex in IntBoya.Battle.Field.Hexes)
                    {
                        double d = Math.Sqrt(Math.Pow(Hex1.CenterX - hex.CenterX, 2) + Math.Pow(Hex1.CenterY - hex.CenterY, 2));
                        if (d < 550) list.Add(hex.ID);
                    }
                    return list.ToArray();
                case ArtefactAbility.BCyberDamage:
                    if (hex1 == 255) return new byte[] { targethex };
                    list = new List<byte>();
                    list.Add(hex1);
                    list.AddRange(IntBoya.Battle.Field.IntersectHexes[hex1, targethex, 1]);
                    list.Add(targethex);
                    return list.ToArray();
                default: return new byte[0];
            }
        }
    }
    public class ArtefactUseResult
    {
        public bool Result;
        public int Value;
        byte type;
        public byte[] Array;
        public ArtefactUseResult(bool result, int value)
        {
            Result = result; Value = value; type = 0;
            List<byte> list = new List<byte>();
            list.Add(type);
            if (Result == false) list.Add(0); else list.Add(1);
            list.AddRange(BitConverter.GetBytes(Value));
            Array = list.ToArray();
        }
        public ArtefactUseResult(byte[] array)
        {
            int i = 0;
            type = array[i]; i++;
            switch (type)
            {
                case 0: if (array[i] == 0) Result = false; else Result = true; i++; Value = BitConverter.ToInt32(array, i); i += 4; break;
            }
        }
        public string GetErrorText()
        {
            if (Result == true) return "Успешно";
            switch (Value)
            {
                case 1: return "Ошибка 10.1: Неверный ID артефакта";
                case 2: return "Ошибка 10.2: У вас нет такого артефакта";
                case 3: return "Ошибка 10.3: Артефакт не мирный";
                case 4: return "Ошибка 10.4: Артефакт не военный";
                case 5: return "Ошибка 10.5: Дополнительные параметры не верны";
                case 6: return "Ошибка 10.6: Колония не принадлежит игроку";
                case 7: return "Ошибка 10.7: Неверный ID сектора";
                case 8: return "Ошибка 10.8: Артефакт нельзя применить к этому сектору";
                case 9: return "Ошибка 10.9: Артефакт нельзя применить к этой колонии";
                default: throw new Exception();
            }
        }
    }
}
