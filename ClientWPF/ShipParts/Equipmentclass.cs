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
    public class Equipmentclass : Item, ISort, IComparable
    {
        public int Type;
        public int BasicType;
        public int Value;
        public byte Level;
        public bool IsRare;
        public EquipmentType EqType;
        public static SortedList<ushort, Equipmentclass> GetList(byte[] array)
        {
            int[] HealthS = new int[] { 0, 6, 12, 18, 24, 30, 36, 42, 45, 48 };
            int[] HealthM = new int[] { 2, 8, 14, 20, 26, 32, 38, 43, 46, 49 };
            int[] HealthL = new int[] { 4, 10, 16, 22, 28, 34, 40, 44, 47, 50 };
            int[] HealthRS = new int[] { 1, 7, 13, 19, 25, 31, 37, 42, 45, 48 };
            int[] HealthRM = new int[] { 3, 9, 15, 21, 27, 33, 39, 43, 46, 49 };
            int[] HealthRL = new int[] { 5, 11, 17, 23, 29, 35, 41, 44, 47, 50 };
            int[] ES = new int[] { 0, 15, 30 };
            int[] PS = new int[] { 1, 16, 31 };
            int[] AS = new int[] { 2, 17, 32 };
            int[] EL = new int[] { 3, 18, 33, 46 };
            int[] PL = new int[] { 4, 19, 34, 47 };
            int[] AL = new int[] { 5, 20, 35, 50 };
            int[] IS = new int[] { 6, 21, 36 };
            int[] CS = new int[] { 7, 22, 37 };
            int[] IL = new int[] { 8, 23, 38, 48 };
            int[] CL = new int[] { 9, 24, 39, 49 };
            int[] ED = new int[] { 10, 25, 40 };
            int[] PD = new int[] { 11, 26, 41 };
            int[] ID = new int[] { 12, 27, 42 };
            int[] CD = new int[] { 13, 28, 43 };
            int[] AD = new int[] { 14, 29, 44 };
            
            
            SortedList<ushort, Equipmentclass> list = new SortedList<ushort, Equipmentclass>();
            AddGroup(list, ES, 500, "Малый энергетический сопроцессор", ItemSize.Small, 0, 30, 120, 5, 50, new int[] { 300, 10, 60, 0 }, 1, 5);
            AddGroup(list, ED, 500, "Средний энергетический сопроцессор", ItemSize.Medium, 0, 45, 180, 10, 100, new int[] { 300, 10, 60, 0 }, 3, 5);
            AddGroup(list, EL, 500, "Групповой энергетический сопроцессор", ItemSize.Large, 36,30,120, 15, 150, new int[] { 300, 10, 70, 20 },  5, 5);
            AddGroup(list, PS, 510, "Малый баллистический сопроцессор", ItemSize.Small, 1, 30, 120, 5, 50, new int[] { 300, 10, 60, 0 }, 1, 5);
            AddGroup(list, PD, 510, "Средний баллистический сопроцессор", ItemSize.Medium, 1, 45, 180, 10, 100, new int[] { 300, 10, 60, 0 }, 3, 5);
            AddGroup(list, PL, 510, "Групповой баллистический сопроцессор", ItemSize.Large, 37, 30, 120, 15, 150, new int[] { 300, 10, 70, 20 },5, 5);
            AddGroup(list, IS, 520, "Малый вероятностный сопроцессор", ItemSize.Small, 2, 30, 120, 5, 50, new int[] { 300, 10, 60, 0 },  1, 5);
            AddGroup(list, ID, 520, "Средний вероятностный сопроцессор", ItemSize.Medium, 2, 45, 180, 10, 100, new int[] { 300, 10, 60, 0 },  3, 5);
            AddGroup(list, IL, 520, "Групповой вероятностный сопроцессор", ItemSize.Large, 38, 30, 120, 15, 150, new int[] { 300, 10, 70, 20 },  5, 5);
            AddGroup(list, CS, 530, "Малый математический сопроцессор", ItemSize.Small, 3, 30, 120, 5, 50, new int[] { 300, 10, 60, 0 }, 1, 5);
            AddGroup(list, CD, 530, "Средний математический сопроцессор", ItemSize.Medium, 3, 45, 180, 10, 100, new int[] { 300, 10, 60, 0 },  3, 5);
            AddGroup(list, CL, 530, "Групповой математический сопроцессор", ItemSize.Large, 39, 30, 120, 15, 150, new int[] { 300, 10, 70, 20 }, 5, 5);
            AddGroup(list, AS, 540, "Малый универсальный сопроцессор", ItemSize.Small, 4, 10, 60, 10, 100, new int[] { 1000, 20, 120, 60 },  1, 5);
            AddGroup(list, AD, 540, "Средний универсальный сопроцессор", ItemSize.Medium, 4, 15, 90, 15, 150, new int[] { 1000, 20, 120, 60 },  3, 5);
            AddGroup(list, AL, 540, "Групповой универсальный сопроцессор", ItemSize.Large, 40, 10, 60, 20, 200, new int[] { 1000, 20, 130, 80 }, 5, 5);

            AddGroup(list, HealthS, 550, "Малый навесной броневой модуль", ItemSize.Small, 5, 20, 200, 0, 0, new int[] { 300, 60, 10, 0 }, 1, 10);
            AddGroup(list, HealthM, 550, "Средний навесной броневой модуль", ItemSize.Medium, 5, 30, 300, 0, 0, new int[] { 300, 60, 10, 0 }, 3, 10);
            AddGroup(list, HealthL, 550, "Групповой усилитель бронирования", ItemSize.Large, 30, 20, 200, 10, 150, new int[] { 300, 60, 20, 30 }, 5, 10);
            AddGroup(list, HealthRS, 560, "Малый ремонтный дроид", ItemSize.Small, 6, 10, 100, 4, 60, new int[] { 600, 10, 10, 20 }, 1, 5);
            AddGroup(list, HealthRM, 560, "Средний ремонтный дроид", ItemSize.Medium, 6, 15, 150, 6, 90, new int[] { 600, 10, 10, 20 }, 3, 5);
            AddGroup(list, HealthRL, 560, "Групповой ремонтный дроид", ItemSize.Large, 31, 10, 100, 10, 150, new int[] { 600, 10, 20, 40 }, 5, 5);

            AddGroup(list, HealthS, 570, "Малый резонаторный силовой модуль", ItemSize.Small, 7, 30, 300, 5, 75, new int[] { 300, 10, 50, 10 }, 1, 10);
            AddGroup(list, HealthM, 570, "Средний резонаторный силовой модуль", ItemSize.Medium, 7, 45, 450, 7, 100, new int[] { 300, 10, 50, 10 }, 3, 10);
            AddGroup(list, HealthL, 570, "Групповой резонаторный силовой модуль", ItemSize.Large, 32, 30, 300, 10, 150, new int[] { 300, 10, 60, 30 }, 5, 10);
            AddGroup(list, HealthRS, 580, "Малая дополнительная силовая обмотка", ItemSize.Small, 8, 20, 200, 5, 75, new int[] { 300, 10, 40, 20 }, 1, 10);
            AddGroup(list, HealthRM, 580, "Средняя дополнительная силовая обмотка", ItemSize.Medium, 8, 30, 300, 7, 100, new int[] { 300, 10, 40, 20 }, 3, 10);
            AddGroup(list, HealthRL, 580, "Групповая дополнительная силовая обмотка", ItemSize.Large, 33, 20, 200, 10, 150, new int[] { 300, 10, 50, 40 }, 5, 10);

            AddGroup(list, HealthS, 590, "Малый полимерный аккумулятор", ItemSize.Small, 9, 20, 200, 5, 50, new int[] { 300, 50, 10, 10 }, 1, 10);
            AddGroup(list, HealthM, 590, "Средний полимерный аккумулятор", ItemSize.Medium, 9, 30, 300, 7, 70, new int[] { 300, 50, 10, 10 }, 3, 10);
            AddGroup(list, HealthL, 590, "Групповой полимерный аккумулятор", ItemSize.Large, 34, 20, 200, 10, 150, new int[] { 300, 50, 20, 30 }, 5, 10);
            AddGroup(list, HealthRS, 600, "Малые топливные элементы", ItemSize.Small, 10, 15, 150, 0, 0, new int[] { 300, 40, 10, 20 }, 1, 10);
            AddGroup(list, HealthRM, 600, "Средние топливные элементы", ItemSize.Medium, 10, 25, 250, 0, 0, new int[] { 300, 40, 10, 20 }, 3, 10);
            AddGroup(list, HealthRL, 600, "Групповые топливные элементы", ItemSize.Large, 35, 15, 150, 0, 0, new int[] { 300, 40, 20, 40 }, 5, 10);

            AddGroup(list, ES, 610, "Малый комлект рассеивающих ловушек", ItemSize.Small, 11, 30, 120, 5, 50, new int[] { 700, 15, 15, 0 }, 1, 5);
            AddGroup(list, ED, 610, "Средний комлект рассеивающих ловушек", ItemSize.Medium, 11, 45, 180, 10, 100, new int[] { 700, 15, 15, 0 }, 3, 5);
            AddGroup(list, EL, 610, "Групповой комлект рассеивающих ловушек", ItemSize.Large, 41, 30, 120, 15, 150, new int[] { 700, 15, 25, 20 }, 5, 5);
            AddGroup(list, PS, 620, "Малый комплект инфракрасных ловушек", ItemSize.Small, 12, 30, 120, 5, 50, new int[] { 700, 15, 15, 0 }, 1, 5);
            AddGroup(list, PD, 620, "Средний комплект инфракрасных ловушек", ItemSize.Medium, 12, 45, 180, 10, 100, new int[] { 700, 15, 15, 0 }, 3, 5);
            AddGroup(list, PL, 620, "Групповой комплект инфракрасных ловушек", ItemSize.Large, 42, 30, 120, 15, 150, new int[] { 700, 15, 25, 20 }, 5, 5);
            AddGroup(list, IS, 630, "Малая вероятностная маскировка", ItemSize.Small, 13, 30, 120, 5, 50, new int[] { 700, 15, 15, 0 },  1, 5);
            AddGroup(list, ID, 630, "Средняя вероятностная маскировка", ItemSize.Medium, 13, 45, 180, 10, 100, new int[] { 700, 15, 15, 0 }, 3, 5);
            AddGroup(list, IL, 630, "Групповая вероятностная маскировка", ItemSize.Large, 43, 30, 120, 15, 150, new int[] { 700, 15, 25, 20 }, 5, 5);
            AddGroup(list, CS, 640, "Малый постановщик помех", ItemSize.Small, 14, 30, 120, 5, 50, new int[] { 700, 15, 15, 0 }, 1, 5);
            AddGroup(list, CD, 640, "Средний постановщик помех", ItemSize.Medium, 14, 45, 180, 10, 100, new int[] { 700, 15, 15, 0 }, 3, 5);
            AddGroup(list, CL, 640, "Групповой постановщик помех", ItemSize.Large, 44, 30, 120, 15, 150, new int[] { 700, 15, 25, 20 }, 5, 5);
            AddGroup(list, AS, 650, "Малые усиленные маневровые дюзы", ItemSize.Small, 15, 10, 60, 10, 100, new int[] { 1400, 30, 30, 100 }, 1, 5);
            AddGroup(list, AD, 650, "Средние усиленные маневровые дюзы", ItemSize.Medium, 15, 15, 90, 15, 150, new int[] { 1400, 30, 30, 100 }, 3, 5);
            AddGroup(list, AL, 650, "Групповые усиленные маневровые дюзы", ItemSize.Large, 45, 10, 60, 20, 200, new int[] { 1400, 30, 40, 120 }, 5, 5);

            AddGroup(list, ES, 660, "Малые отражательные элементы", ItemSize.Small, 16, 10, 50, 10, 80, new int[] { 300, 60, 10, 0 }, 1, 1);
            AddGroup(list, ED, 660, "Средние отражательные элементы", ItemSize.Medium, 16, 15, 75, 15, 120, new int[] { 300, 60, 10, 0 }, 3, 1);
            AddGroup(list, EL, 660, "Групповые отражательные элементы", ItemSize.Large, 46, 10, 50, 20, 160, new int[] { 300, 60, 20, 20 }, 5, 1);
            AddGroup(list, PS, 670, "Малая композитная броня", ItemSize.Small, 17, 10, 50, 10, 80, new int[] { 300, 60, 10, 0 }, 1, 1);
            AddGroup(list, PD, 670, "Средняя композитная броня", ItemSize.Medium, 17, 15, 75, 15, 120, new int[] { 300, 60, 10, 0 }, 3, 1);
            AddGroup(list, PL, 670, "Групповая композитная броня", ItemSize.Large, 47, 10, 50, 20, 160, new int[] { 300, 60, 20, 20 }, 5, 1);
            AddGroup(list, IS, 680, "Малое вероятностное экранирование", ItemSize.Small, 18, 10, 50, 10, 80, new int[] { 300, 60, 10, 0 }, 1, 1);
            AddGroup(list, ID, 680, "Среднее вероятностное экранирование", ItemSize.Medium, 18, 15, 75, 15, 120, new int[] { 300, 60, 10, 0 }, 3, 1);
            AddGroup(list, IL, 680, "Групповое вероятностное экранирование", ItemSize.Large, 48, 10, 50, 20, 160, new int[] { 300, 60, 20, 20 }, 5, 1);
            AddGroup(list, CS, 690, "Малые экранированные кабели", ItemSize.Small, 19, 10, 50, 10, 80, new int[] { 300, 60, 10, 0 }, 1, 1);
            AddGroup(list, CD, 690, "Средние экранированные кабели", ItemSize.Medium, 19, 15, 75, 15, 120, new int[] { 300, 60, 10, 0 }, 3, 1);
            AddGroup(list, CL, 690, "Групповые экранированные кабели", ItemSize.Large, 49, 10, 50, 20, 160, new int[] { 300, 60, 20, 20 }, 5, 1);
            AddGroup(list, AS, 700, "Малая усиленная конструкция", ItemSize.Small, 20, 5, 25, 15, 20, new int[] { 600, 110, 30, 100 }, 1, 1);
            AddGroup(list, AD, 700, "Средняя усиленная конструкция", ItemSize.Medium, 20, 7, 35, 20, 160, new int[] { 600, 110, 30, 100 }, 3, 1);
            AddGroup(list, AL, 700, "Групповая усиленная конструкция", ItemSize.Large, 50, 5, 25, 25, 200, new int[] { 600, 110, 40, 120 }, 5, 1);

            AddGroup(list, ES, 710, "Малый блок конденсаторов", ItemSize.Small, 21, 20, 160, 10, 80, new int[] { 300, 15, 15, 40 }, 1, 10);
            AddGroup(list, ED, 710, "Средний блок конденсаторов", ItemSize.Medium, 21, 30, 240, 15, 120, new int[] { 300, 15, 15, 40 }, 3, 10);
            AddGroup(list, EL, 710, "Групповой блок конденсаторов", ItemSize.Large, 51, 20, 160, 20, 160, new int[] { 300, 15, 25, 60 }, 5, 10);
            AddGroup(list, PS, 720, "Малые графеновые элементы", ItemSize.Small, 22, 20, 160, 10, 80, new int[] { 300, 15, 15, 40 }, 1, 10);
            AddGroup(list, PD, 720, "Средние графеновые элементы", ItemSize.Medium, 22, 30, 240, 15, 120, new int[] { 300, 15, 15, 40 }, 3, 10);
            AddGroup(list, PL, 720, "Групповые графеновые элементы", ItemSize.Large, 52, 20, 160, 20, 160, new int[] { 300, 15, 25, 60 }, 5, 10);
            AddGroup(list, IS, 730, "Малые кристаллы из антиматерии", ItemSize.Small, 23, 20, 160, 10, 80, new int[] { 300, 15, 15, 40 }, 1, 10);
            AddGroup(list, ID, 730, "Средние кристаллы из антиматерии", ItemSize.Medium, 23, 30, 240, 15, 120, new int[] { 300, 15, 15, 40 }, 3, 10);
            AddGroup(list, IL, 730, "Групповые кристаллы из антиматерии", ItemSize.Large, 53, 20, 160, 20, 160, new int[] { 300, 15, 25, 60 }, 5, 10);
            AddGroup(list, CS, 740, "Малые адаптивные алгоритмы", ItemSize.Small, 24, 20, 160, 10, 80, new int[] { 300, 15, 15, 40 }, 1, 10);
            AddGroup(list, CD, 740, "Средние адаптивные алгоритмы", ItemSize.Medium, 24, 30, 240, 15, 120, new int[] { 300, 15, 15, 40 }, 3, 10);
            AddGroup(list, CL, 740, "Групповые адаптивные алгоритмы", ItemSize.Large, 54, 20, 160, 20, 160, new int[] { 300, 15, 25, 60 }, 5, 10);
            AddGroup(list, AS, 750, "Малый анализатор уязвимых точек", ItemSize.Small, 25, 10, 80, 15, 120, new int[] { 800, 35, 55, 90 }, 1, 10);
            AddGroup(list, AD, 750, "Средний анализатор уязвимых точек", ItemSize.Medium, 25, 15, 120, 20, 160, new int[] { 800, 35, 55, 90 }, 3, 10);
            AddGroup(list, AL, 750, "Групповой анализатор уязвимых точек", ItemSize.Large, 55, 10, 80, 25, 200, new int[] { 800, 35, 65, 110 }, 5, 10);

            AddGroup(list, ES, 760, "Малый поглотитель энергетических возмущений", ItemSize.Small, 59, 20, 80, 15, 90, new int[] { 3000, 240, 240, 220 }, 1, 10);
            AddGroup(list, ED, 760, "Средний поглотитель энергетических возмущений", ItemSize.Medium, 59, 40, 120, 25, 150, new int[] { 3000, 240, 240, 220 }, 3, 10);
            AddGroup(list, EL, 760, "Групповой поглотитель энергетических возмущений", ItemSize.Large, 64, 20, 80, 25, 150, new int[] { 3000, 240, 250, 240 }, 5, 10);
            AddGroup(list, PS, 770, "Малый поглотитель физичесих возумущений", ItemSize.Small, 60, 20, 80, 15, 90, new int[] { 3000, 240, 240, 220 }, 1, 10);
            AddGroup(list, PD, 770, "Средний поглотитель физичесих возумущений", ItemSize.Medium, 60, 40, 120, 25, 150, new int[] { 3000, 240, 240, 220 }, 3, 10);
            AddGroup(list, PL, 770, "Групповой поглотитель физичесих возумущений", ItemSize.Large, 65, 20, 80, 25, 150, new int[] { 3000, 240, 250, 240 }, 5, 10);
            AddGroup(list, IS, 780, "Малый поглотитель аномальных возмущений", ItemSize.Small, 61, 20, 80, 15, 90, new int[] { 3000, 240, 240, 220 }, 1, 10);
            AddGroup(list, ID, 780, "Средний поглотитель аномальных возмущений", ItemSize.Medium, 61, 40, 120, 25, 150, new int[] { 3000, 240, 240, 220 }, 3, 10);
            AddGroup(list, IL, 780, "Групповой поглотитель аномальных возмущений", ItemSize.Large, 66, 20, 80, 25, 150, new int[] { 3000, 240, 250, 240 }, 5, 10);
            AddGroup(list, CS, 790, "Малый поглотитель кибернетических возмущений", ItemSize.Small, 62, 20, 80, 15, 90, new int[] { 3000, 240, 240, 220 }, 1, 10);
            AddGroup(list, CD, 790, "Средний поглотитель кибернетических возмущений", ItemSize.Medium, 62, 40, 120, 25, 150, new int[] { 3000, 240, 240, 220 }, 3, 10);
            AddGroup(list, CL, 790, "Групповой поглотитель кибернетических возмущений", ItemSize.Large, 67, 20, 80, 25, 150, new int[] { 3000, 240, 250, 240 }, 5, 10);
            AddGroup(list, AS, 800, "Малый универсальный поглотитель возмущений", ItemSize.Small, 63, 20, 80, 25, 150, new int[] { 5000, 500, 500, 500 }, 1, 10);
            AddGroup(list, AD, 800, "Средний универсальный поглотитель возмущений", ItemSize.Medium, 63, 40, 110, 35, 280, new int[] { 5000, 500, 500, 500 }, 3, 10);
            AddGroup(list, AL, 800, "Групповой универсальный поглотитель возмущений", ItemSize.Large, 68, 20, 80, 35, 280, new int[] { 5000, 500, 500, 500 }, 5, 10);
            //AddGroup(list, HealthS, )
           /* for (int i = 0; i < array.Length; )
            {
                ushort id = BitConverter.ToUInt16(array, i); i += 2;
                ItemSize size = (ItemSize)array[i]; i++;
                int consume = BitConverter.ToInt32(array, i); i += 4;
                byte type = array[i]; i++;
                int value = BitConverter.ToInt32(array, i); i += 4;
                byte level = array[i]; i++;
                bool israre = BitConverter.ToBoolean(array, i); i++;
                ItemPrice price = ItemPrice.GetPrice(array, i); i += 16;
                Equipmentclass equip = new Equipmentclass(id, "", size, consume, type, value, price);
                equip.Level = level; equip.IsRare = israre;
                list.Add(id, equip);
            }*/
            return list;
        }
        public static ItemPrice GetPrice(int[] m, int level, double mult)
        {
            double[] d = new double[] { 1.18, 1.19, 1.20, 1.21 };
            double[] r = new double[4];
            for (int i=0;i<4;i++)
            {
                r[i] = m[i]*Math.Pow(d[i], level)*mult;
                if (r[i]<1000)
                {
                    r[i] = Math.Round(r[i] / 10, 0) * 10;
                }
                else if (r[i]<10000)
                {
                    r[i] = Math.Round(r[i] / 100, 0) * 100;
                }
                else
                {
                    int q = (int)Math.Log10(r[i]) - 3;
                    r[i] = Math.Round(r[i] / Math.Pow(10, q), 0) * Math.Pow(10, q);
                }
            }
            return new ItemPrice((int)r[0], (int)r[1], (int)r[2], (int)r[3]);
        }
        static void AddGroup(SortedList<ushort, Equipmentclass> list, int[]levels, int idbase, string name, ItemSize size, int type, 
            int valmin, int valmax, int consmin, int consmax, int[] m, double pricemult, double valueround)
        {
            for (int i=0;i<levels.Length;i++)
            {
                int level = levels[i];
                ushort id = (ushort)(level * 1000 + idbase);
                string Name = string.Format("{0} {1}", name, i + 1);
                int consume = (int)((consmax - consmin) / 50.0 * level) + consmin;
                int value = (int)((valmax - valmin) / 50.0 * level) + valmin;
                value = (int)(Math.Round(value / valueround, 0) * valueround);
                ItemPrice price = GetPrice(m, level, pricemult);
                list.Add(id, new Equipmentclass(id, Name, level, size, consume, type, value, price));
            }
        }
        public Equipmentclass(ushort id, string name, int level, ItemSize size, int consume, int type, int value, ItemPrice price)
            : base(id, name, size, consume, price)
        {
            Type = type;
            Value = value;
            BasicType = Type;
            Level = (byte)level;

            switch (Type)
            {
                case 30:
                case 31:
                case 32:
                case 33:
                case 34:
                case 35: BasicType = Type - 25; break;
                case 36:
                case 37:
                case 38:
                case 39:
                case 40: BasicType = Type - 36; break;
                case 41: case 42: case 43: case 44: case 45: BasicType = Type - 30; break; //Уклонение
                case 46: case 47: case 48: case 49: case 50: BasicType = Type - 30; break; //Игнор
                case 51: case 52: case 53: case 54: case 55: BasicType = Type - 30; break; //Дамаг
                case 57: BasicType = 31; break;
                case 58: BasicType = 32; break;
                case 59: case 60: case 61: case 62: BasicType = Type - 33; break;
                case 63: BasicType = 30; break;
                case 64: case 65: case 66: case 67: BasicType = Type - 38; break;
                
            }
            switch (BasicType)
            {
                case 0: EqType = EquipmentType.AccEn; break;
                case 1: EqType = EquipmentType.AccPh; break;
                case 2: EqType = EquipmentType.AccIr; break;
                case 3: EqType = EquipmentType.AccCy; break;
                case 4: EqType = EquipmentType.AccAl; break;
                case 5: EqType = EquipmentType.Hull; break;
                case 6: EqType = EquipmentType.HullRegen; break;
                case 7: EqType = EquipmentType.Shield; break;
                case 8: EqType = EquipmentType.ShieldRegen; break;
                case 9: EqType = EquipmentType.Energy; break;
                case 10: EqType = EquipmentType.EnergyRegen; break;
                case 11: EqType = EquipmentType.EvaEn; break;
                case 12: EqType = EquipmentType.EvaPh; break;
                case 13: EqType = EquipmentType.EvaIr; break;
                case 14: EqType = EquipmentType.EvaCy; break;
                case 15: EqType = EquipmentType.EvaAl; break;
                case 16: EqType = EquipmentType.IgnEn; break;
                case 17: EqType = EquipmentType.IgnPh; break;
                case 18: EqType = EquipmentType.IgnIr; break;
                case 19: EqType = EquipmentType.IgnCy; break;
                case 20: EqType = EquipmentType.IgnAl; break;
                case 21: EqType = EquipmentType.DamEn; break;
                case 22: EqType = EquipmentType.DamPh; break;
                case 23: EqType = EquipmentType.DamIr; break;
                case 24: EqType = EquipmentType.DamCy; break;
                case 25: EqType = EquipmentType.DamAl;break;
                case 26: EqType = EquipmentType.ImmEn; break;
                case 27: EqType = EquipmentType.ImmPh; break;
                case 28: EqType = EquipmentType.ImmIr; break;
                case 29: EqType = EquipmentType.ImmCy; break;
                case 30: EqType = EquipmentType.ImmAl; break;

            }
        }
        
        static SortedSet<byte> AccuracyEffects = new SortedSet<byte>(new byte[] { 0, 1, 2, 3, 4, 36, 37, 38, 39, 40 });
        static SortedSet<byte> EvasionEffects = new SortedSet<byte>(new byte[] { 11, 12, 13, 14, 15, 41, 42, 43, 44, 45 });
        static SortedSet<byte> DamageEffects = new SortedSet<byte>(new byte[] { 21, 22, 23, 24, 25, 51, 52, 53, 54, 55 });
        static SortedSet<byte> IgnoreEffects = new SortedSet<byte>(new byte[] { 16, 17, 18, 19, 20, 46, 47, 48, 49, 50 });
        static SortedSet<byte> ImmuneEffects = new SortedSet<byte>(new byte[] { 59, 60, 61, 62, 63 });
        static SortedSet<byte> HealthEffects = new SortedSet<byte>(new byte[] { 5, 6, 30, 31 });
        static SortedSet<byte> ShieldCapEffects = new SortedSet<byte>(new byte[] { 7, 32 });
        static SortedSet<byte> ShieldResEffects = new SortedSet<byte>(new byte[] { 8, 33 });
        static SortedSet<byte> EnergyCapEffects = new SortedSet<byte>(new byte[] { 9, 34});
        static SortedSet<byte> EnergyResEffects = new SortedSet<byte>(new byte[] { 10, 35 });
        public EquipmentEffect GetEquipEffect()
        {
            byte type = (byte)Type;
            if (AccuracyEffects.Contains(type)) return EquipmentEffect.Accuracy;
            if (EvasionEffects.Contains(type)) return EquipmentEffect.Evasion;
            if (DamageEffects.Contains(type)) return EquipmentEffect.Damage;
            if (IgnoreEffects.Contains(type)) return EquipmentEffect.Ignore;
            if (ImmuneEffects.Contains(type)) return EquipmentEffect.Immune;
            if (HealthEffects.Contains(type)) return EquipmentEffect.Health;
            if (ShieldCapEffects.Contains(type)) return EquipmentEffect.ShieldCap;
            if (ShieldResEffects.Contains(type)) return EquipmentEffect.ShieldRes;
            if (EnergyCapEffects.Contains(type)) return EquipmentEffect.EnergyCap;
            if (EnergyResEffects.Contains(type)) return EquipmentEffect.EnergyRes;
            return EquipmentEffect.Other;
        }
        public WeaponGroup GetGroup()
        {
            switch (Type)
            {
                case 0:
                case 21:
                case 36:
                case 51: return WeaponGroup.Energy;
                case 1:
                case 22:
                case 37:
                case 52: return WeaponGroup.Physic;
                case 2:
                case 23:
                case 38:
                case 53:return WeaponGroup.Irregular;
                case 3:
                case 24:
                case 39:
                case 54: return WeaponGroup.Cyber;
                  
                default: return WeaponGroup.Any;
               
            }
        }
        public void SetPrice(ItemPrice price)
        {
            price.Add(Price);
        }
        int GetGroupWeaponCount(Schema schema, WeaponGroup group)
        {
            int result=0;
            ShipTypeclass shiptype = schema.ShipType;
            if (shiptype == null) return 0;
            for (int i = 0; i < shiptype.WeaponCapacity; i++)
            {
                Weaponclass weapon = schema.Weapons[i];
                if (weapon == null) continue;
                if (weapon.Group == group || group==WeaponGroup.Any) result++; 
            }
            return result;
        }
        
        public static UIElement[] GetEquipmentColumnNames()
        {
            List<UIElement> list = new List<UIElement>();
            list.Add(Common.GetBlock(20, Links.Interface("Type")));
            list.Add(Common.GetBlock(20, Links.Interface("Effect")));
            list.Add(Common.GetRectangle(30, Pictogram.EnergyBrush, Links.Interface("EnergyConsume")));
            list.Add(Common.GetBlock(20, Links.Interface("Size")));
            list.Add(Common.GetRectangle(30, Links.Brushes.MoneyImageBrush));
            list.Add(Common.GetRectangle(30, Links.Brushes.MetalImageBrush));
            list.Add(Common.GetRectangle(30, Links.Brushes.ChipsImageBrush));
            list.Add(Common.GetRectangle(30, Links.Brushes.AntiImageBrush));
            return list.ToArray();
            
        }
        public int GetParam(int pos)
        {
            switch (pos)
            {
                case 0: return (int)Type;
                case 1: return Value;
                case 2: return Consume;
                case 3: return (int)Size;
                case 4: return Price.Money;
                case 5: return Price.Metall;
                case 6: return Price.Chips;
                case 7: return Price.Anti;
                default: return 0;
            }
        }
        public string GetName()
        {
            return GameObjectName.GetEquipmentName(this);
        }
        public int GetID()
        {
            return ID;
        }
        
        public UIElement GetElement(int pos)
        {
            switch (pos)
            {
                case 0:
                    return Pictogram.GetPict(this, 30);
                case 1:
                    if (Type == 57)
                        return SelectItemCanvas.GetValueElement((Value / 10.0).ToString()+"%");
                    else return null;
                    //return Abilities.GetElement((byte)Type);
                case 3: return Common.GetSizeRect(15, Size, false);
                default: return null;
            }

        }
        public string GetValueString()
        {
            if (Type == 57)
                return (Value / 10.0).ToString() + "%";
            else return Value.ToString();
        }
        public SecondaryEffect GetEffect()
        {
            SecondaryEffect effect = new SecondaryEffect((byte)Type, Value);
            return effect;
        }
        public ServerSecondaryEffect GetServerEffect()
        {
            ServerSecondaryEffect effect = new ServerSecondaryEffect((byte)Type, Value);
            return effect;
        }
        public bool IsPassive()
        {
            if (Abilities.IsActiveSkill((byte)Type)) return false; else return true;
        }
        public int CompareTo(object b)
        {
            Equipmentclass B = (Equipmentclass)b;
            if (ID > B.ID) return 1;
            else if (ID < B.ID) return -1;
            else return 0;
        }
        /// <summary> Метод проверяет, подходит ли оборудование под тип эффекта и группу. Применяется для генерации схемы в AIShipGenerator </summary>
        
        public static SortedSet<EquipmentType> CheckCompartibility(EquipmentEffect effect, WeaponGroup group)
        {
            SortedSet<EquipmentType> result = new SortedSet<EquipmentType>();
            switch (effect)
            {
                case EquipmentEffect.Accuracy: 
                    switch(group)
                    {
                        case WeaponGroup.Energy:  result.Add(EquipmentType.AccEn); break;
                        case WeaponGroup.Physic: result.Add(EquipmentType.AccPh);break;
                        case WeaponGroup.Irregular: result.Add(EquipmentType.AccIr);break;
                        case WeaponGroup.Cyber: result.Add(EquipmentType.AccCy);break;
                    } break;
                case EquipmentEffect.Evasion:
                    result.Add(EquipmentType.EvaEn); result.Add(EquipmentType.EvaPh); result.Add(EquipmentType.EvaIr);
                    result.Add(EquipmentType.EvaCy); result.Add(EquipmentType.EvaAl);
                    break;
                case EquipmentEffect.Damage:
                    switch (group)
                    {
                        case WeaponGroup.Energy: result.Add(EquipmentType.DamEn); break;
                        case WeaponGroup.Physic: result.Add(EquipmentType.DamPh); break;
                        case WeaponGroup.Irregular: result.Add(EquipmentType.DamIr); break;
                        case WeaponGroup.Cyber: result.Add(EquipmentType.DamCy); break;
                    }break;
                case EquipmentEffect.EnergyCap: result.Add(EquipmentType.Energy); break;
                case EquipmentEffect.EnergyRes: result.Add(EquipmentType.EnergyRegen); break;
                case EquipmentEffect.Health: result.Add(EquipmentType.Hull); break;
                case EquipmentEffect.HealthRes: result.Add(EquipmentType.HullRegen); break;
                case EquipmentEffect.Ignore:
                    result.Add(EquipmentType.IgnEn); result.Add(EquipmentType.IgnPh); result.Add(EquipmentType.IgnIr);
                    result.Add(EquipmentType.IgnCy); result.Add(EquipmentType.IgnAl);
                    break;
                case EquipmentEffect.Immune:
                    result.Add(EquipmentType.ImmEn); result.Add(EquipmentType.ImmPh); result.Add(EquipmentType.ImmIr);
                    result.Add(EquipmentType.ImmCy);result.Add(EquipmentType.ImmAl);
                    break;
                case EquipmentEffect.ShieldCap: result.Add(EquipmentType.Shield); break;
                case EquipmentEffect.ShieldRes: result.Add(EquipmentType.ShieldRegen); break;
            }
            return result;
        }
    }
    public enum EquipmentEffect { Accuracy, Evasion, Damage, Ignore, Immune, Health, ShieldCap, ShieldRes, EnergyCap, EnergyRes, HealthRes, Other}
    //enum EquipmentEffect { Attack, Defense, Increase, Restore, Others }
}
