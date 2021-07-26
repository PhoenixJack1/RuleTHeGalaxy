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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    
    class SchemaImage:Border
    {
        
        public static SortedList<int, string> BaseRoman;
        //static SortedList<int, char> Volumes;
        //static string[] NameList;
        static string GetName(string typename, int name)
        {
            byte[] array = BitConverter.GetBytes(name);
            string result = typename;
            if (Links.Lang == 0)
            {
                if (array[0] != 0 && array[0] < EnTitle.Length)
                    result += " " + EnTitle[array[0]];
                if (array[1] != 0 && array[1] < EnLetters.Length)
                    result += " " + EnLetters[array[1]];
                if (array[2] != 0)
                    result += "-" + GetRoman(array[2]);
                if (array[3] != 0)
                    result += "-" + array[3].ToString();
            }
            else
            {
                if (array[0] != 0 && array[0] < RuTitle.Length)
                    result += " " + EnTitle[array[0]];
                if (array[1] != 0 && array[1] < RuLetters.Length)
                    result += " " + EnLetters[array[1]];
                if (array[2] != 0)
                    result += "-" + GetRoman(array[2]);
                if (array[3] != 0)
                    result += "-" + array[3].ToString();

            }
            return result;
        }
        public static void Prepare()
        {
            BaseRoman = new SortedList<int, string>();
            BaseRoman.Add(1, "I");
            BaseRoman.Add(4, "IV");
            BaseRoman.Add(5, "V");
            BaseRoman.Add(9, "IX");
            BaseRoman.Add(10, "X");
            BaseRoman.Add(40, "XL");
            BaseRoman.Add(50, "L");
            BaseRoman.Add(90, "XC");
            BaseRoman.Add(100, "C");
            if (Links.Lang == 0)
            {
                SelectSchemaNameCanvas.TitleList = SchemaImage.EnTitle;
                SelectSchemaNameCanvas.Letters = SchemaImage.EnLetters;
            }
            else
            {
                SelectSchemaNameCanvas.TitleList = SchemaImage.RuTitle;
                SelectSchemaNameCanvas.Letters = SchemaImage.RuLetters;
            }
            SelectSchemaNameCanvas.BaseRoman = SchemaImage.BaseRoman;
            /*
            NameList = new string[] {"", "Mk", "Type", "Var", "Ver", "Vol", "Group", "Param", "Obj" };
            Volumes=new SortedList<int,char>();
            for (int i = 1; i < 27; i++)
                Volumes.Add(i, (char)(i + 64));
            for (int i = 27; i < 53; i++)
                Volumes.Add(i, (char)(i + 70));
            for (int i = 53; i < 85; i++)
                Volumes.Add(i, (char)(i + 987));
            Volumes.Add(86, 'Ё');
            for (int i = 87; i < 119; i++)
                Volumes.Add(i, (char)(i + 985));
            Volumes.Add(120, 'ё');
            */
        }
        static string GetRoman(byte k)
        {
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
        public static string[] EnTitle = new string[] { "", "Mk","Type","Ver.","Group","Status","Code","Tank","Attack" };
        public static string[] RuTitle = new string[] { "", "Мк", "Тип", "Вер.", "Группа", "Статус", "Код", "Танк", "Атака" };
        public static string[] EnLetters = new string[] {"","A","B","C","D","E","F","G","H","I","J","K","L","M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W",
        "X","Y","Z" };
        public static string[] RuLetters = new string[] {"","А","Б","С","Д","Е","Ф","Г","Ш","И","Ж","К","Л", "М", "Н", "О", "П", "Кью", "Р", "С", "Т", "Ю", "ДВ",
        "Х","У","З"};
    }
}
