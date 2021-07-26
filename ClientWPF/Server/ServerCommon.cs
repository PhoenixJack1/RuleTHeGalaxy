using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class ServerCommon
    {
        public static byte[] CopyArray(byte[] array, int startindex, int length)
        {
            Queue<byte> result = new Queue<byte>();
            int end = startindex + length;
            for (int i = startindex; i < end; i++)
                result.Enqueue(array[i]);
            return result.ToArray();
        }
        public static void AddBytesToList(List<byte> list, object[] array)
        {
            foreach (object obj in array)
            {
                Type type = obj.GetType();
                if (type == typeof(Int32))
                    list.AddRange(BitConverter.GetBytes((int)obj));
                else if (type == typeof(UInt16))
                    list.AddRange(BitConverter.GetBytes((ushort)obj));
                else if (type == typeof(Byte))
                    list.Add((byte)obj);
                else if (type == typeof(Boolean))
                    list.AddRange(BitConverter.GetBytes((bool)obj));
                else if (type == typeof(GSString))
                    list.AddRange(((GSString)obj).Array);
                else if (type == typeof(ItemPrice))
                    list.AddRange(((ItemPrice)obj).GetCode());
                else if (type == typeof(byte[]))
                    list.AddRange((byte[])obj);
                else if (type == typeof(Double))
                    list.AddRange(BitConverter.GetBytes((double)obj));
            }
        }
        public static int GetRandomValue(int[] chances, Random rnd)
        {
            int[] sumchances = new int[chances.Length];
            sumchances[0] = chances[0];
            for (int i = 1; i < chances.Length; i++)
                sumchances[i] = sumchances[i - 1] + chances[i];
            //int[] sumchances = chances;
            int val = rnd.Next(sumchances[sumchances.Length - 1]);
            for (int i = 0; i < sumchances.Length; i++)
                if (val < sumchances[i]) return i;
            return 0;

        }
    }
}
