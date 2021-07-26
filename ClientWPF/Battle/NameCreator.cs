using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    enum ShipNameType { Standard, Big, Portal, Cargo, Building }
    class NameCreator
    {
        static SortedList<byte, string> Array3;
        static SortedList<byte, string> Array2;
        static SortedList<byte, string> Array1;
        static SortedList<byte, string> BuildingNames;
        public static string GetName(ShipNameType type, Schema schema, byte[] array)
        {
            switch (type)
            {
                case ShipNameType.Standard:
                    switch (array[0])
                    {
                        case 0: //Обычные корабли
                            if (schema == null)
                            {
                                switch (array[1])
                                {
                                    case 0: return "Корабль" + Array2[array[2]] + Array3[array[3]];
                                    default: return Array1[array[1]] + Array2[array[2]] + Array3[array[3]];
                                }
                            }
                            else
                                return schema.ShipType.GetName()+ Array2[array[2]] + Array3[array[3]];             
                        case 200: //Имена из архива
                            {
                                return StoryLine2.StoryShipNames[array[1]] + Array2[array[2]] + Array3[array[3]];
                            }
                        case 251: //Зелёные
                            {
                                return Array1[array[1]] + " зелёного альянса" + Array2[array[2]] + Array3[array[3]];
                            }
                        case 252: //Пираты
                            {
                                return "Пиратский" + " " + Array1[array[1]].ToLower() + Array2[array[2]] + Array3[array[3]];
                            }
                        case 253: //Чужие
                            {
                                return Array1[array[1]] + " вторжения" + Array2[array[2]] + Array3[array[3]];
                            }
                        case 254: //Технократы
                            {
                                return Array1[array[1]] + " технократов" + Array2[array[2]] + Array3[array[3]];
                            }
                        case 255: //Наёмников
                            {
                                return Array1[array[1]] + " наёмников" + Array2[array[2]] + Array3[array[3]];
                            }

                    }
                    break;
                case ShipNameType.Big:
                    switch (array[0])
                    {
                        case 200: return StoryLine2.StoryShipNames[array[1]] + Array2[array[2]] + Array3[array[3]];
                        case 251: return "Титан зелёного альянса" + Array2[array[2]] + Array3[array[3]];
                        case 252: return "Пиратский титан" + Array2[array[2]] + Array3[array[3]];
                        case 253: return "Титан вторжения" + Array2[array[2]] + Array3[array[3]];
                        case 254: return "Титан технократов" + Array2[array[2]] + Array3[array[3]];
                        case 255: return "Титан наёмников" + Array2[array[2]] + Array3[array[3]];
                        default: return "Титан" + Array2[array[2]] + Array3[array[3]];
                    }
                case ShipNameType.Portal:
                    switch (array[0])
                    {
                        case 200: return StoryLine2.StoryShipNames[array[1]] + Array2[array[2]] + Array3[array[3]];
                        case 251: return "Портал зелёного альянса" + Array2[array[2]] + Array3[array[3]];
                        case 252: return "Пиратский портал" + Array2[array[2]] + Array3[array[3]];
                        case 253: return "Портал вторжения" + Array2[array[2]] + Array3[array[3]];
                        case 254: return "Портал технократов" + Array2[array[2]] + Array3[array[3]];
                        case 255: return "Портал наёмников" + Array2[array[2]] + Array3[array[3]];
                        default: return "Портал" + Array2[array[2]] + Array3[array[3]];
                    }
                case ShipNameType.Cargo:
                    switch (array[0])
                    {
                        case 200: return StoryLine2.StoryShipNames[array[1]] + Array2[array[2]] + Array3[array[3]];
                        case 251: return "Конвой зелёного альянса" + Array2[array[2]] + Array3[array[3]];
                        case 252: return "Пиратский конвой" + Array2[array[2]] + Array3[array[3]];
                        case 253: return "Конвой вторжения" + Array2[array[2]] + Array3[array[3]];
                        case 254: return "Конвой технократов" + Array2[array[2]] + Array3[array[3]];
                        case 255: return "Конвой наёмников" + Array2[array[2]] + Array3[array[3]];
                        default: return "Конвой" + Array2[array[2]] + Array3[array[3]];
                    }
                case ShipNameType.Building:
                    switch (array[0])
                    {
                        case 200: return StoryLine2.StoryShipNames[array[1]] + Array2[array[2]] + Array3[array[3]];
                        case 251: return BuildingNames[array[1]] + " зелёного альянса" + Array2[array[2]] + Array3[array[3]];
                        case 252: return BuildingNames[array[1]] + " пиратов" + Array2[array[2]] + Array3[array[3]];
                        case 253: return BuildingNames[array[1]] + " вторжения" + Array2[array[2]] + Array3[array[3]];
                        case 254: return BuildingNames[array[1]] + " технократов" + Array2[array[2]] + Array3[array[3]];
                        case 255: return BuildingNames[array[1]] + " наёмников" + Array2[array[2]] + Array3[array[3]];
                        default: return BuildingNames[array[1]] + Array2[array[2]] + Array3[array[3]];
                    }
            }
            return "Объект" + Array2[array[2]] + Array3[array[3]];
        }
        public static string GetRoman(byte k)
        {
            SortedList<int, string> BaseRoman = new SortedList<int, string>();
            BaseRoman.Add(1, "I");
            BaseRoman.Add(4, "IV");
            BaseRoman.Add(5, "V");
            BaseRoman.Add(9, "IX");
            BaseRoman.Add(10, "X");
            BaseRoman.Add(40, "XL");
            BaseRoman.Add(50, "L");
            BaseRoman.Add(90, "XC");
            BaseRoman.Add(100, "C");
            StringBuilder result = new StringBuilder();
            var neednumbers =
                from z in BaseRoman.Keys
                where z <= k
                orderby z descending
                select z;
            foreach (byte cur in neednumbers)
            {
                while ((k / cur) >= 1)
                {
                    k -= cur;
                    result.Append(BaseRoman[cur]);
                }
            }
            return result.ToString();
        }
        public static void Prepare()
        {
            Array1 = new SortedList<byte, string>();
            for (int i = 0; i < 256; i++)
                Array1.Add((byte)i, "Корабль");
            string others = "Разведичк Корвет Транспорт Линкор Фрегат Истебитель Дредноут Девостатор Воитель Крейсер";
            string[] othersA = others.Split(' ');
            for (byte i = 1; i < 11; i++)
                Array1[i] = othersA[i - 1];

            Array2 = new SortedList<byte, string>();
            for (int i = 0; i < 256; i++)
                Array2.Add((byte)i, "");
            others = "Мк Тип Объект";
            othersA = others.Split(' ');
            for (byte i = 1; i < 4; i++)
                Array2[i] = " " + othersA[i - 1];

            BuildingNames = new SortedList<byte, string>();
            for (int i = 0; i < 256; i++)
                BuildingNames.Add((byte)i, "Строение");
            others = "Шахта Склад Верфь Ангар База Станция Генератор";
            othersA = others.Split(' ');
            for (byte i = 1; i < 8; i++)
                BuildingNames[i] = othersA[i - 1];

            Array3 = new SortedList<byte, string>();
            for (int i = 0; i < 256; i++)
                Array3.Add((byte)i, "");
            for (byte i = 1; i < 101; i++)
                Array3[i] = "-" + i.ToString();
            for (int i = 201; i < 256; i++)
                Array3[(byte)i] = "-" + GetRoman((byte)(i - 200));
            others = "А Б В Г Д Е Ж З И К Л М Н О П Р С Т У Ф Х Ц Ч Ш Щ Ы Э Ю Я";
            othersA = others.Split(' ');
            for (byte i = 101; i < 130; i++)
                Array3[i] = "-" + othersA[i - 101];
            others = "Альфа Бета Гамма Дельта Эпсилон Лямбда";
            othersA = others.Split(' ');
            for (byte i = 131; i < 137; i++)
                Array3[i] = "-" + othersA[i - 131];
        }
    }
}
