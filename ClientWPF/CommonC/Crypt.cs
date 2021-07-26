using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Client
{
    static class Crypt
    {
        public enum Crypt_Status : byte { Normal, Message_Symbol_Error, Key_Symbol_Error };
        static int[] Name_Symbols;
        static byte[] Name_Symbols_Check;
        static int[] encrypt_arr;
        public static int[,] decrypt_arr;
        static int[] english_symbols;
        public static string version = "V1.0";
        public static bool CheckName(string Name)
        {
            for (int i = 0; i < Name.Length; i++)
            {
                if (Name[i] >= Name_Symbols_Check.Length) return false;
                if (Name_Symbols_Check[Name[i]] == 0) return false;
            }
            return true;
        }
        public static bool CheckString(string line)
        {
            foreach (char label in line)
                if (label > 8470 || decrypt_arr[0, label] == -1) return false;
            return true;
        }
        public static string Encrypt(string line, string key, out Crypt_Status status)
        {
            long longHash = LongHash(key);
            char[] result = new char[line.Length];
            string crypt = "";
            for (int i = 0; i < line.Length; i++)
            {
                if (decrypt_arr[0, (int)line[i]] == -1) { status = Crypt_Status.Message_Symbol_Error; return ""; }
                if (decrypt_arr[0, (int)key[i % key.Length]] == -1) { status = Crypt_Status.Key_Symbol_Error; return ""; }
                int truekey = ((int)key[i % key.Length]) + (int)(longHash % key.Length);
                int beforekey = decrypt_arr[2, line[i]];
                int middlekey = (beforekey + truekey) % encrypt_arr.Length;
                int afterkey = encrypt_arr[middlekey];
                result[i] = (char)decrypt_arr[0, afterkey];
            }
            crypt = new string(result);
            status = Crypt_Status.Normal;
            return crypt;
        }
        public static string Encrypt(string line, string key)
        {
            Crypt_Status status;
            return Encrypt(line, key, out status);
        }
        public static string Decrypt(string crypt, string key, out Crypt_Status status)
        {
            long longHash = LongHash(key);
            char[] result = new char[crypt.Length];
            string line = "";
            for (int i = 0; i < crypt.Length; i++)
            {
                if (decrypt_arr[0, (int)crypt[i]] == -1) { status = Crypt_Status.Message_Symbol_Error; return ""; }
                if (decrypt_arr[0, (int)key[i % key.Length]] == -1) { status = Crypt_Status.Key_Symbol_Error; return ""; }
                int truekey = ((int)key[i % key.Length]) + (int)(longHash % key.Length);
                int antikey = encrypt_arr.Length - truekey%encrypt_arr.Length;
                int beforekey = decrypt_arr[1, crypt[i]];
                int middlekey = decrypt_arr[2, beforekey];
                int afterkey = (middlekey + antikey) % encrypt_arr.Length;
                result[i] = (char)encrypt_arr[afterkey];
            }
            line = new string(result);
            status = Crypt_Status.Normal;
            return line;
        }
        public static string Decrypt(string crypt, string key)
        {
            Crypt_Status status;
            return Decrypt(crypt, key, out status);
        }
        public static long LongHash(string line)
        {
            long result = 0;
            for (int i = 0; i < line.Length; i++)
            {
                result += ((int)line[i] + 13) * ((i + 1) * 17);
            }
            double k = 1;
            for (; ; )
            {
                if (result > 999999999999)
                    if (result < 10000000000000)
                        break;
                    else
                    { k += 0.2; result = (long)(result / k); }
                k += 0.1;
                result = (long)(result * k);

            }
            result = result - (int)(result / 100000000000) * 100000000000;
            return result;
        }
        public static string StringHash(string line)
        {
            string result = "";
            int[] array = new int[]{100,200,300,400,500,600,700,800,
                                    900,1000,1100,1200,1300,1400,1500,1600};
            for (int i = 0; i < line.Length; i++)
            {
                int label = line[i];
                array[0] = (array[0] + (label + 59) * ((i + 59) * 3)) % 8523645;
                array[1] = (array[1] + (label + 53) * ((i + 53) * 5)) % 8964532;
                array[2] = (array[2] + (label + 47) * ((i + 47) * 7)) % 2216544;
                array[3] = (array[3] + (label + 43) * ((i + 43) * 11)) % 1569746;
                array[4] = (array[4] + (label + 41) * ((i + 41) * 13)) % 6504930;
                array[5] = (array[5] + (label + 37) * ((i + 37) * 17)) % 9540479;
                array[6] = (array[6] + (label + 31) * ((i + 31) * 19)) % 8963047;
                array[7] = (array[7] + (label + 29) * ((i + 29) * 23)) % 9804563;
                array[8] = (array[8] + (label + 23) * ((i + 23) * 29)) % 9804023;
                array[9] = (array[9] + (label + 19) * ((i + 19) * 31)) % 4563217;
                array[10] = (array[10] + (label + 17) * ((i + 17) * 37)) % 4632104;
                array[11] = (array[11] + (label + 13) * ((i + 13) * 41)) % 6540123;
                array[12] = (array[12] + (label + 11) * ((i + 11) * 43)) % 7865023;
                array[13] = (array[13] + (label + 7) * ((i + 7) * 47)) % 9870456;
                array[14] = (array[14] + (label + 5) * ((i + 5) * 53)) % 3248703;
                array[15] = (array[15] + (label + 3) * ((i + 3) * 59)) % 4568702;
            }
            for (int i = 0; i < 16; i++)
            {
                result += ((char)english_symbols[array[i] % 62]).ToString();
            }

            return result;
        }
        public static void fill_Crypto()
        {
            Name_Symbols = new int[]{45,
                48,49,50,51,52,53,54,55,56,57,
                65,66,67,68,69,70,71,72,73,74,
                75,76,77,78,79,80,81,82,83,84,
                85,86,87,88,89,90,
                95,97,98,99,100,101,102,103,104,
                105,106,107,108,109,110,111,112,113,114,
                115,116,117,118,119,120,121,122,
                1025,1040,1041,1042,1043,1044,
                1045,1046,1047,1048,1049,1050,1051,1052,1053,1054,
                1055,1056,1057,1058,1059,1060,1061,1062,1063,1064,
                1065,1066,1067,1068,1069,1070,1071,1072,1073,1074,
                1075,1076,1077,1078,1079,1080,1081,1082,1083,1084,
                1085,1086,1087,1088,1089,1090,1091,1092,1093,1094,
                1095,1096,1097,1098,1099,1100,1101,1102,1103,1105};
            Name_Symbols_Check = new byte[1106];
            for (int i = 0; i < Name_Symbols.Length; i++)
                Name_Symbols_Check[Name_Symbols[i]] = 1;
            encrypt_arr = new int[]{48,93,119,68,1054,110,124,1060,
            1077,1052,1065,1063,98,125,122,51,108,1055,1046,42,
        83,1079,81,103,1061,43,72,1099,109,1064,102,1069,1057,
        59,100,106,1066,1080,1058,1078,1040,87,63,1051,1084,85,
            37,32,33,111,91,35,75,1090,96,71,41,1094,1096,1086,
            76,56,1070,1049,1081,1067,1088,1053,1103,1085,1041,
            114,1073,74,45,92,1045,57,1071,1083,115,54,94,95,1074,
            113,1068,40,97,116,84,1100,1050,82,1042,70,1087,53,
            1056,8470,1092,123,118,1102,1098,89,34,1082,39,65,112,
            77,60,55,69,50,47,73,99,1044,44,1048,86,1076,80,1105,
            88,79,1047,1043,62,1093,38,64,1091,58,1062,107,1097,
            1025,67,36,126,78,61,1059,121,1075,66,49,1072,120,1101,
            90,1089,105,104,46,117,52,101,10,1095};
            decrypt_arr = new int[3, 8471];
            for (int i = 0; i < decrypt_arr.GetLength(1); i++)
                decrypt_arr[0, i] = -1;
            decr(10, 64, 161);
            decr(32, 56, 47);
            decr(33, 100, 48);
            decr(34, 47, 106);
            decr(35, 1047, 51);
            decr(36, 1084, 141);
            decr(37, 101, 46);
            decr(38, 1082, 132);
            decr(39, 1053, 108);
            decr(40, 1094, 87);
            decr(41, 62, 56);
            decr(42, 1056, 19);
            decr(43, 1060, 25);
            decr(44, 1066, 120);
            decr(45, 71, 74);
            decr(46, 54, 157);
            decr(47, 1049, 116);
            decr(48, 1087, 0);
            decr(49, 1091, 149);
            decr(50, 73, 115);
            decr(51, 49, 15);
            decr(52, 50, 159);
            decr(53, 34, 97);
            decr(54, 107, 81);
            decr(55, 32, 113);
            decr(56, 1051, 61);
            decr(57, 76, 77);
            decr(58, 55, 135);
            decr(59, 79, 33);
            decr(60, 48, 112);
            decr(61, 80, 144);
            decr(62, 98, 130);
            decr(63, 114, 42);
            decr(64, 33, 133);
            decr(65, 110, 109);
            decr(66, 117, 148);
            decr(67, 57, 140);
            decr(68, 1088, 3);
            decr(69, 1061, 114);
            decr(70, 43, 95);
            decr(71, 106, 55);
            decr(72, 1045, 26);
            decr(73, 1054, 117);
            decr(74, 1103, 73);
            decr(75, 1069, 52);
            decr(76, 109, 60);
            decr(77, 1043, 111);
            decr(78, 1068, 143);
            decr(79, 1101, 127);
            decr(80, 46, 124);
            decr(81, 1055, 22);
            decr(82, 112, 93);
            decr(83, 1097, 20);
            decr(84, 1095, 90);
            decr(85, 1041, 45);
            decr(86, 1102, 122);
            decr(87, 122, 41);
            decr(88, 37, 126);
            decr(89, 60, 105);
            decr(90, 63, 153);
            decr(91, 104, 50);
            decr(92, 103, 75);
            decr(93, 65, 1);
            decr(94, 94, 82);
            decr(95, 89, 83);
            decr(96, 1078, 54);
            decr(97, 92, 88);
            decr(98, 116, 12);
            decr(99, 69, 118);
            decr(100, 126, 34);
            decr(101, 86, 160);
            decr(102, 1048, 30);
            decr(103, 1098, 23);
            decr(104, 119, 156);
            decr(105, 51, 155);
            decr(106, 1044, 35);
            decr(107, 1089, 137);
            decr(108, 96, 16);
            decr(109, 1077, 28);
            decr(110, 93, 5);
            decr(111, 36, 49);
            decr(112, 1065, 110);
            decr(113, 97, 85);
            decr(114, 1092, 71);
            decr(115, 1072, 80);
            decr(116, 1050, 89);
            decr(117, 44, 158);
            decr(118, 74, 102);
            decr(119, 99, 2);
            decr(120, 1096, 151);
            decr(121, 61, 146);
            decr(122, 120, 14);
            decr(123, 91, 101);
            decr(124, 1067, 6);
            decr(125, 115, 13);
            decr(126, 1086, 142);
            decr(1025, 108, 139);
            decr(1040, 1073, 40);
            decr(1041, 78, 70);
            decr(1042, 125, 94);
            decr(1043, 105, 129);
            decr(1044, 1042, 119);
            decr(1045, 41, 76);
            decr(1046, 85, 18);
            decr(1047, 59, 128);
            decr(1048, 1079, 121);
            decr(1049, 1059, 63);
            decr(1050, 77, 92);
            decr(1051, 70, 43);
            decr(1052, 66, 9);
            decr(1053, 1070, 67);
            decr(1054, 42, 4);
            decr(1055, 1057, 17);
            decr(1056, 1075, 98);
            decr(1057, 84, 32);
            decr(1058, 88, 38);
            decr(1059, 1100, 145);
            decr(1060, 1062, 7);
            decr(1061, 35, 24);
            decr(1062, 52, 136);
            decr(1063, 102, 11);
            decr(1064, 113, 29);
            decr(1065, 1074, 10);
            decr(1066, 10, 36);
            decr(1067, 1105, 65);
            decr(1068, 1099, 86);
            decr(1069, 1071, 31);
            decr(1070, 1090, 62);
            decr(1071, 1063, 78);
            decr(1072, 1040, 150);
            decr(1073, 72, 72);
            decr(1074, 87, 84);
            decr(1075, 45, 147);
            decr(1076, 1025, 123);
            decr(1077, 83, 8);
            decr(1078, 118, 39);
            decr(1079, 8470, 21);
            decr(1080, 1064, 37);
            decr(1081, 68, 64);
            decr(1082, 67, 107);
            decr(1083, 1076, 79);
            decr(1084, 123, 44);
            decr(1085, 38, 69);
            decr(1086, 1052, 59);
            decr(1087, 1093, 96);
            decr(1088, 1083, 66);
            decr(1089, 90, 154);
            decr(1090, 75, 53);
            decr(1091, 1058, 134);
            decr(1092, 1080, 100);
            decr(1093, 124, 131);
            decr(1094, 111, 57);
            decr(1095, 39, 162);
            decr(1096, 40, 58);
            decr(1097, 81, 138);
            decr(1098, 82, 104);
            decr(1099, 1046, 27);
            decr(1100, 1081, 91);
            decr(1101, 121, 152);
            decr(1102, 1085, 103);
            decr(1103, 95, 68);
            decr(1105, 58, 125);
            decr(8470, 53, 99);
            english_symbols = new int[62];
            int k = -1;
            for (int i = 48; i < 58; i++)
            { k++; english_symbols[k] = i; }
            for (int i = 65; i < 91; i++)
            { k++; english_symbols[k] = i; }
            for (int i = 97; i < 123; i++)
            { k++; english_symbols[k] = i; }
        }
        static void decr(int position, int value, int pos)
        {
            decrypt_arr[0, position] = value;
            decrypt_arr[1, value] = position;
            decrypt_arr[2, position] = pos;
        }
    }
    public class GSString : IComparable
    {
        static SortedList<byte, char> ByteToChar;
        static SortedList<char, byte> CharToByte;
        static SortedSet<byte> Symbols;
        static SortedSet<byte> Numbers;
        static SortedSet<byte> Letters;
        public byte[] Array { get; private set; }
        public GSString(string text)
        {
                Array = GetArrayFromString(text);
        }

        public GSString(byte[] array, int startindex)
        {

            int length = BitConverter.ToInt32(array, startindex);
            byte[] result = new byte[length + 4];
            for (int i = 0; i < result.Length; i++)
                result[i] = array[startindex + i];
            Array = result;

        }
        public static GSString GetGSString(byte[] array, int startindex)
        {
            if (CheckArray(array, startindex))
                return new GSString(array, startindex);
            else return null;
        }
        public static void Prepare()
        {
            ByteToChar = new SortedList<byte, char>();
            ByteToChar.Add(0, (char)10);
            for (int i = 1; i < 96; i++)
                ByteToChar.Add((byte)i, (char)(i + 31));
            ByteToChar.Add(96, (char)1025);
            for (int i = 97; i < 161; i++)
                ByteToChar.Add((byte)i, (char)(i + 943));
            ByteToChar.Add(161, (char)1105);
            ByteToChar.Add(162, (char)8470);
            CharToByte = new SortedList<char, byte>();
            foreach (KeyValuePair<byte, char> pair in ByteToChar)
                CharToByte.Add(pair.Value, pair.Key);
            Symbols = new SortedSet<byte>(new byte[] { 8, 9, 10, 14, 15,60,62, 64, 65, 95 });
            Numbers = new SortedSet<byte>(new byte[] { 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 });
            Letters = new SortedSet<byte>();
            for (byte i = 34; i < 60; i++)
                Letters.Add(i);
            for (byte i = 66; i < 92; i++)
                Letters.Add(i);
            for (byte i = 96; i < 162; i++)
                Letters.Add(i);
        }
        public bool CheckName()
        {
            if (Array.Length < 6 || Array.Length > 18) return false;
            if (!Letters.Contains(Array[4])) return false;
            for (int i = 5; i < Array.Length; i++)
            {
                if (Letters.Contains(Array[i]) || Symbols.Contains(Array[i]) || Numbers.Contains(Array[i])) continue;
                else return false;
            }
            return true;
        }
        public bool CheckPassword()
        {
            if (Array.Length < 8 || Array.Length > 24) return false;
            for (int i = 4; i < Array.Length; i++)
                if (!ByteToChar.ContainsKey(Array[i])) return false;
            return true;
        }
        public GSString DeCryptString(byte[] key)
        {
            byte[] shortArr = GetShortArray();
            for (int i = 0; i < shortArr.Length; i++)
                shortArr[i] = (byte)((shortArr[i] - key[i % key.Length] + 326) % ByteToChar.Count);
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(shortArr.Length));
            list.AddRange(shortArr);
            return new GSString(list.ToArray(), 0);
        }
        public GSString DeCryptString(GSString Key)
        {
            byte[] shortArr = GetShortArray();
            byte[] shortKey = Key.GetShortArray();
            for (int i = 0; i < shortArr.Length; i++)
                shortArr[i] = (byte)((shortArr[i] - shortKey[i % shortKey.Length] + ByteToChar.Count) % ByteToChar.Count);
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(shortArr.Length));
            list.AddRange(shortArr);
            return new GSString(list.ToArray(), 0);

        }
        public GSString GetPasswordString(byte[] key)
        {
            byte[] shortArr = GetShortArray();
            for (int i = 0; i < shortArr.Length; i++)
                shortArr[i] = (byte)((shortArr[i] + key[i%key.Length]) % ByteToChar.Count);
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(shortArr.Length));
            list.AddRange(shortArr);
            return new GSString(list.ToArray(), 0);
        }
        public GSString GetPasswordString(GSString Key)
        {
            byte[] shortArr = GetShortArray();
            byte[] shortKey = Key.GetShortArray();
            for (int i = 0; i < shortArr.Length; i++)
                shortArr[i] = (byte)((shortArr[i] + shortKey[i % shortKey.Length]) % ByteToChar.Count);
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(shortArr.Length));
            list.AddRange(shortArr);
            return new GSString(list.ToArray(), 0);
        }
        public byte[] GetShortArray()
        {
            byte[] result = new byte[Array.Length - 4];
            for (int i = 4; i < Array.Length; i++)
                result[i - 4] = Array[i];
            return result;
        }
        public long GetLongHash()
        {
            SHA256 sha = SHA256.Create();
            sha.ComputeHash(Array);
            byte[] arr = new byte[8];
            arr[0] = sha.Hash[3]; arr[1] = sha.Hash[12]; arr[2] = sha.Hash[17]; arr[3] = sha.Hash[7];
            arr[4] = sha.Hash[31]; arr[5] = sha.Hash[14]; arr[6] = sha.Hash[25]; arr[7] = sha.Hash[21];
            return BitConverter.ToInt64(arr, 0);
        }
        public static byte[] GetArrayFromString(string text)
        {
            List<byte> array = new List<byte>();
            foreach (char c in text)
            {
                if (!CharToByte.ContainsKey(c)) continue;
                array.Add(CharToByte[c]);
            }
            List<byte> result = new List<byte>();
            result.AddRange(BitConverter.GetBytes(array.Count));
            result.AddRange(array);
            return result.ToArray();
        }
        public static string GetStringFromArray(byte[] array, int start)
        {
            List<char> result = new List<char>();
            int length = BitConverter.ToInt32(array, start);
            start += 4;
            for (int i = start; i < start + length; i++)
                result.Add(ByteToChar[array[i]]);
            return new string(result.ToArray());
        }
        public static bool CheckString(string s)
        {
            foreach (char c in s)
                if (!CharToByte.ContainsKey(c)) return false;
            return true;
        }
        public static bool CheckArray(byte[] array, int startindex)
        {
            if (array.Length < 4 + startindex) return false;
            int length = BitConverter.ToInt32(array, startindex);
            if (array.Length < (4 + length + startindex)) return false;
            for (int i = 4 + startindex; i < 4 + length + startindex; i++)
                if (!ByteToChar.ContainsKey(array[i])) return false;
            return true;
        }
        public override string ToString()
        {
            return GetStringFromArray(Array, 0);
        }
        public int CompareTo(object string1)
        {
            GSString string2 = (GSString)string1;
            if (Array.Length < string2.Array.Length) return -1;
            else if (Array.Length > string2.Array.Length) return 1;
            else
            {
                for (int i = 0; i < Array.Length; i++)
                    if (Array[i] < string2.Array[i]) return -1;
                    else if (Array[i] > string2.Array[i]) return 1;
            }
            return 0;
        }
    }
}
